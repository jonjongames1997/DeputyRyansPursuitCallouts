using CalloutInterfaceAPI;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using LSPD_First_Response.Mod.API;

namespace DeputyRyansPursuitCallouts.Callouts
{
    [CalloutInterface("Vehicle Theft Pursuit", CalloutProbability.High, "A pursuit involving a stolen vehicle.", "Code 3", "LSPD")]
    public class VehicleTheftPursuit : Callout
    {
        private Vector3 spawnPoint;
        private Ped suspect;
        private Vehicle suspectVehicle;
        private Blip suspectBlip;
        private LHandle pursuit;

        public override bool OnBeforeCalloutDisplayed()
        {
            // Find a road position for the spawn point
            spawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(500f));
            suspectVehicle = new Vehicle("DUKES", spawnPoint);

            if (!suspectVehicle.Exists())
                return false;

            suspect = suspectVehicle.CreateRandomDriver();

            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 30f);
            AddMinimumDistanceCheck(50f, spawnPoint);

            CalloutMessage = "Vehicle Theft Pursuit";
            CalloutPosition = spawnPoint;
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("WE_HAVE CRIME_VEHICLE_THEFT");

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            suspectBlip = suspectVehicle.AttachBlip();
            suspectBlip.IsFriendly = false;

            suspect.Tasks.CruiseWithVehicle(suspectVehicle, 100f, VehicleDrivingFlags.FollowTraffic);

            CalloutInterfaceAPI.Functions.SendMessage(this, "Officer, a pursuit involving a vehicle theft is in progress. The suspect is driving a Dukes. Proceed with caution.");

            pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(pursuit, suspect);
            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(pursuit, true);

            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();

            if (!suspect.IsAlive || LSPD_First_Response.Mod.API.Functions.IsPedArrested(suspect))
            {
                End();
            }
        }

        public override void End()
        {
            base.End();

            if (suspectBlip.Exists()) suspectBlip.Delete();
            if (suspect.Exists()) suspect.Dismiss();
            if (suspectVehicle.Exists()) suspectVehicle.Dismiss();
            Game.LogTrivial("VehicleTheftPursuit callout has ended.");
        }
    }
}
