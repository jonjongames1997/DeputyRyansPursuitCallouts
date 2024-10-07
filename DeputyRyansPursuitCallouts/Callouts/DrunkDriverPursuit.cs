using CalloutInterfaceAPI;

namespace DeputyRyansPursuitCallouts.Callouts
{
    [CalloutInterface("Drunk Driver Pursuit", CalloutProbability.Medium, "A pursuit involving a drunk driver.", "Code 3", "LSPD")]
    public class DrunkDriverPursuit : Callout
    {
        private static Vector3 spawnPoint;
        private static Ped suspect;
        private static readonly Vehicle suspectVehicle;
        private static Blip suspectBlip;
        private static LHandle pursuit;
        private static readonly string theVehicle[] = { "STANIER" };

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(500f));
            suspectVehicle = new Vehicle(theVehicle[new Random().Next((int)theVehicle.Length)], spawnPoint);

            if (!suspectVehicle.Exists())
                return false;

            suspect = suspectVehicle.CreateRandomDriver();

            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 30f);
            AddMinimumDistanceCheck(50f, spawnPoint);

            CalloutInterfaceAPI.Functions.SendMessage(this, "Officer, a pursuit involving a drunk driver is in progress. The suspect is driving erratically. Proceed with caution.");
            CalloutMessage = "Drunk Driver Pursuit";
            CalloutPosition = spawnPoint;
            LSPD_First_Response.Mod.API.Functions.PlayScannerAudio("WE_HAVE CRIME_DRUNK_DRIVER");

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            suspectBlip = suspectVehicle.AttachBlip();
            suspectBlip.IsFriendly = false;
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;

            suspect.Tasks.CruiseWithVehicle(suspectVehicle, 70f, VehicleDrivingFlags.FollowTraffic);

            pursuit = LSPD_First_Response.Mod.API.Functions.CreatePursuit();
            LSPD_First_Response.Mod.API.Functions.AddPedToPursuit(pursuit, suspect);
            LSPD_First_Response.Mod.API.Functions.SetPursuitIsActiveForPlayer(pursuit, true);

            return base.OnCalloutAccepted();
        }

        // If the player does not accept the callout, it will clear this out //
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
            Game.LogTrivial("DrunkDriverPursuit callout has ended.");
        }
    }
}
