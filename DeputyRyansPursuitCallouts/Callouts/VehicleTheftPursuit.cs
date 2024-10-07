using CalloutInterfaceAPI;

namespace DeputyRyansPursuitCallouts.Callouts
{
    [CalloutInterface("Vehicle Theft Pursuit", CalloutProbability.High, "A pursuit involving a stolen vehicle.", "Code 3", "LSPD")]
    
    public class VehicleTheftPursuit : Callout
    {
        private static Vector3 spawnPoint;
        private static Ped suspect;
        private static readonly Vehicle suspectVehicle;
        private static Blip suspectBlip;
        private static LHandle pursuit;

        public override bool OnBeforeCalloutDisplayed()
        {
            // Find a road position for the spawn point
            spawnPoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around(500f));
            suspectVehicle = new Vehicle("DUKES", spawnPoint);

            if (!suspectVehicle.Exists())
                return false;

            suspect = suspectVehicle.CreateRandomDriver();

            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 30f);
            AddMinimumDistanceCheck(50f, spawnPoint);

            CalloutInterfaceAPI.Functions.SendMessage(this, "Officer, a pursuit involving a vehicle theft is in progress. The suspect is driving a Dukes. Proceed with caution.");
            CalloutMessage = "Vehicle Theft Pursuit";
            CalloutPosition = spawnPoint;
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("WE_HAVE CRIME_VEHICLE_THEFT");

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            suspectBlip = suspectVehicle.AttachBlip();
            suspectBlip.IsFriendly = false;
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;

            suspect.Tasks.CruiseWithVehicle(suspectVehicle, 100f, VehicleDrivingFlags.FollowTraffic);

            pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(pursuit, suspect);
            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(pursuit, true);

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (suspect) suspect.Delete();
            if (suspectVehicle) suspectVehicle.Delete();
            if (suspectBlip) suspectBlip.Delete();
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
