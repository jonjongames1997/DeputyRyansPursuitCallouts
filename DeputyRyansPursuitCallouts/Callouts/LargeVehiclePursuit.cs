using CalloutInterfaceAPI;

namespace DeputyRyansPursuitCallouts.Callouts
{
    [CalloutInterface("Large Vehicle Pursuit", CalloutProbability.Medium, "A pursuit involving a large vehicle.", "Code 3", "LSPD")]
    
    public class LargeVehiclePursuit : Callout
    {
        private static Vector3 spawnPoint;
        private static Ped suspect;
        private static readonly Vehicle suspectVehicle;
        private static Blip suspectBlip;
        private static LHandle pursuit;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnPoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around(500f));
            suspectVehicle = new Vehicle("PHANTOM", spawnPoint);

            if (!suspectVehicle.Exists())
                return false;

            suspect = suspectVehicle.CreateRandomDriver();

            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 30f);
            AddMinimumDistanceCheck(50f, spawnPoint);

            CalloutInterfaceAPI.Functions.SendMessage(this, "Officer, a pursuit involving a large vehicle is in progress. The suspect is driving a Phantom. Proceed with caution.");
            CalloutMessage = "Large Vehicle Pursuit";
            CalloutPosition = spawnPoint;
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("WE_HAVE CRIME_PURSUIT_IN_PROGRESS");

            return base.OnBeforeCalloutDisplayed();
        }

        public override void OnCalloutAccepted()
        {
            suspectBlip = suspectVehicle.AttachBlip();
            suspectBlip.IsFriendly = false;

            suspect.Tasks.CruiseWithVehicle(suspectVehicle, 80f, VehicleDrivingFlags.FollowTraffic);

            pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(pursuit, suspect);
            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(pursuit, true);

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (suspect) suspect.Delete();
            if (suspectBlip) suspectBlip.Delete();
            if (suspectVehicle) suspectVehicle.Delete();
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
            Game.LogTrivial("LargeVehiclePursuit callout has ended.");
        }
    }
}
