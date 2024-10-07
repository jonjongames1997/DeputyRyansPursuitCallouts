using System;
using LSPD_First_Response.Mod.API;
using Rage;
using DeputyRyansPursuitCallouts.Callouts;

namespace DeputyRyansPursuitCallouts
{
    public class Main : Plugin
    {
        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnDutyStateChanged;
            Settings.LoadSettings();

            Game.LogTrivial("DeputyRyansPursuitCallouts has been initialized.");
        }

        private void OnDutyStateChanged(bool onDuty)
        {
            if (onDuty)
            GameFiber.StartNew(delegate
            {
                RegisterCallouts();
                Game.Console.Print();
                Game.Console.Print("=============================================== DeputyRyansPursuitCallouts by DeputyRyan32 ================================================");
                Game.Console.Print();
                Game.Console.Print("[LOG]: Callouts and settings were loaded successfully.");
                Game.Console.Print("[LOG]: The config file was loaded successfully.");
                Game.Console.Print("[VERSION]: Detected Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
                Game.Console.Print("[LOG]: Checking for a new DeputyRyansPursuitCallouts version...");
                Game.Console.Print();
                Game.Console.Print();
                Game.Console.Print();
                Game.Console.Print();
                Game.Console.Print();
                Game.Console.Print();
                Game.Console.Print("=============================================== DeputyRyansPursuitCallouts by DeputyRyan32 ================================================");
                Game.Console.Print();

                VersionChecker.Version.IsUpdateAvailable();

                // In-game notification log
                Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "DeputyRyansPursuitCallouts", "~g~Callouts Initialized", "All callouts have been successfully loaded.");

                GameFiber.Wait(300);
            });
        }

        // Register your callouts here //

        private static void RegisterCallouts()
        {
            Functions.RegisterCallout(typeof(ArmedRobberyGetaway));
            Functions.RegisterCallout(typeof(DrunkDriverPursuit));
            Functions.RegisterCallout(typeof(HitAndRunPursuit));
            Functions.RegisterCallout(typeof(LargeVehiclePursuit));
            Functions.RegisterCallout(typeof(LargeVehiclePursuit2));
            Functions.RegisterCallout(typeof(PursuitInProgress));
            Functions.RegisterCallout(typeof(RecklessDriverPursuit));
            Functions.RegisterCallout(typeof(RoadRagePursuit));
            Functions.RegisterCallout(typeof(StolenPoliceVehiclePursuit));
            Functions.RegisterCallout(typeof(StolenVehiclePursuit));
            Functions.RegisterCallout(typeof(VehicleTheftPursuit));
            Functions.RegisterCallout(typeof(VehicleTheftPursuit2));
        }

        public override void Finally()
        {
            Game.LogTrivial("DeputyRyansPursuitCallouts has been cleaned up.");
        }
    }
}
