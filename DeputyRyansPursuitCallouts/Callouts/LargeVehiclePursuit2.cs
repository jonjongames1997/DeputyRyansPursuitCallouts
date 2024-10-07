using CalloutInterfaceAPI;

namespace DeputyRyansPursuitCallouts.Callouts
{
    [CalloutInterface("Large Vehicle Pursuit", CalloutProbability.Low, "A large vehicle, such as a truck, is fleeing the scene.", "Code 3", "LSPD")]
    
    public class LargeVehiclePursuit2 : Callout
    {
        private static Vector3 spawnPoint;
        private static Ped suspect;
        private static readonly Vehicle suspectVehicle;
        private static Blip suspectBlip;
        private static LHandle pursuit;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(800f));
            suspectVehicle = new Vehicle("HAULER", spawnPoint);

            if (!suspectVehicle.Exists())
                return false;

            suspect = suspectVehicle.CreateRandomDriver();

            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 50f);
            AddMinimumDistanceCheck(70f, spawnPoint);

            CalloutInterfaceAPI.Functions.SendMessage(this, "Officer, a large vehicle is fleeing the scene. Be prepared for a lengthy pursuit.");
            CalloutMessage = "Large Vehicle Pursuit";
            CalloutPosition = spawnPoint;
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("CRIME_GRAND_THEFT_AUTO");

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            suspectBlip = suspectVehicle.AttachBlip();
            suspectBlip.IsFriendly = false;
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;

            suspect.Tasks.CruiseWithVehicle(suspectVehicle, 50f, VehicleDrivingFlags.Emergency);

            pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(pursuit, suspect);
            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(pursuit, true);

            return base.OnCalloutAccepted();
        }

        public override bool OnCalloutNotAccepted()
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
            Game.LogTrivial("LargeVehiclePursuit callout has ended.");
        }
    }
}
