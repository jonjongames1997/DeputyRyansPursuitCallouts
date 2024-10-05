using CalloutInterfaceAPI;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using LSPD_First_Response.Mod.API;

namespace DeputyRyansPursuitCallouts.Callouts
{
    [CalloutInterface("Pursuit in Progress", CalloutProbability.High, "A vehicle pursuit in progress.", "Code 3", "LSPD")]
    public class PursuitInProgress : Callout
    {
        private Vector3 spawnPoint;
        private Ped suspect;
        private Vehicle suspectVehicle;
        private Blip suspectBlip;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(500f));
            suspectVehicle = new Vehicle("ADDER", spawnPoint);

            if (!suspectVehicle.Exists())
                return false;

            suspect = suspectVehicle.CreateRandomDriver();

            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 30f);
            AddMinimumDistanceCheck(50f, spawnPoint);

            CalloutMessage = "Pursuit in Progress";
            CalloutPosition = spawnPoint;
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("WE_HAVE CRIME_PURSUIT_IN_PROGRESS");

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            suspectBlip = suspectVehicle.AttachBlip();
            suspectBlip.IsFriendly = false;

            suspect.Tasks.CruiseWithVehicle(suspectVehicle, 100f, VehicleDrivingFlags.FollowTraffic);

            CalloutInterfaceAPI.Functions.SendMessage(this, "Officer, a pursuit is in progress. The suspect is driving a super fast car. Proceed with caution.");

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
            Game.LogTrivial("PursuitInProgress callout has ended.");
        }
    }
}
