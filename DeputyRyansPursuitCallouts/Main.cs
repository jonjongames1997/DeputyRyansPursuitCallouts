using System;
using LSPD_First_Response.Mod.API;
using Rage;
using DeputyRyansPursuitCallouts.Callouts;
using System.Configuration;

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
            if (Settings.ArmedRobberyGetaway) { Functions.RegisterCallout(typeof(ArmedRobberyGetaway)); }
            if (Settings.DrunkDriverPursuit) { Functions.RegisterCallout(typeof(DrunkDriverPursuit)); }
            if (Settings.HitAndRunPursuit) { Functions.RegisterCallout(typeof(HitAndRunPursuit)); }
            if (Settings.LargeVehiclePursuit) { Functions.RegisterCallout(typeof(LargeVehiclePursuit)); }
            if (Settings.LargeVehiclePursuit2) { Functions.RegisterCallout(typeof(LargeVehiclePursuit2)); }
            if (Settings.PursuitInProgress) { Functions.RegisterCallout(typeof(PursuitInProgress)); }
            if (Settings.RecklessDriverPursuit) { Functions.RegisterCallout(typeof(RecklessDriverPursuit)); }
            if (Settings.RoadRagePursuit) { Functions.RegisterCallout(typeof(RoadRagePursuit)); }
            if (Settings.StolenPoliceVehiclePursuit) { Functions.RegisterCallout(typeof(StolenPoliceVehiclePursuit)); }
            if (Settings.StolenVehiclePursuit) { Functions.RegisterCallout(typeof(StolenVehiclePursuit)); }
            if (Settings.VehicleTheftPursuit) { Functions.RegisterCallout(typeof(VehicleTheftPursuit)); }
            if (Settings.VehicleTheftPursuit2) { Functions.RegisterCallout(typeof(VehicleTheftPursuit2)); }
        }

        public override void Finally()
        {
            Game.LogTrivial("DeputyRyansPursuitCallouts has been cleaned up.");
        }
    }
}
