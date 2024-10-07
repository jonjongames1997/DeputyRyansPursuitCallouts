using CalloutInterfaceAPI;

namespace DeputyRyansPursuitCallouts.Callouts
{
    [CalloutInterface("Armed Robbery Getaway", CalloutProbability.High, "A getaway after an armed robbery.", "Code 3", "LSPD")]
    public class ArmedRobberyGetaway : Callout
    {
        private static Vector3 spawnPoint;
        private static Ped suspect;
        private static readonly Vehicle suspectVehicle;
        private static Blip suspectBlip;
        private static LHandle pursuit;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnPoint = World.GetNextPositionOnStreet(MainPlayer.Position.Around(500f));
            suspectVehicle = new Vehicle("BUFFALO", spawnPoint);

            if (!suspectVehicle.Exists())
                return false;

            suspect = suspectVehicle.CreateRandomDriver();

            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 30f);
            AddMinimumDistanceCheck(50f, spawnPoint);

            CalloutInterfaceAPI.Functions.SendMessage(this, "Officer, an armed robbery suspect is making a getaway. The suspect is considered armed and dangerous. Proceed with caution.");
            CalloutMessage = "Armed Robbery Getaway";
            CalloutPosition = spawnPoint;
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("CRIME_ARMED_ROBBERY_01");

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            suspectBlip = suspectVehicle.AttachBlip();
            suspectBlip.IsFriendly = false;
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;

            suspect.Tasks.CruiseWithVehicle(suspectVehicle, 90f, VehicleDrivingFlags.Emergency);

            pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(pursuit, suspect);
            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(pursuit, true);

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (suspect) suspect.Delete();
            if(suspectVehicle) suspectVehicle.Delete();
            if(suspectBlip) suspectBlip.Delete();
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
            Game.LogTrivial("ArmedRobberyGetaway callout has ended.");
        }
    }
}
