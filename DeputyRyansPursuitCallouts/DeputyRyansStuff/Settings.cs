using System.Windows.Forms;
using System.Xml;

namespace DeputyRyansPursuitCallouts
{

    internal static class Settings
    {
        // Add your callouts to the config settings for the ini //
        internal static bool ArmedRobberyGetaway = true;
        internal static bool DrunkDriverPursuit = true;
        internal static bool HitAndRunPursuit = true;
        internal static bool LargeVehiclePursuit = true;
        internal static bool LargeVehiclePursuit2 = true;
        internal static bool RecklessDrivingPursuit = true;
        internal static bool RoadRagePursuit = true;
        internal static bool StolenPoliceVehiclePursuit = true;
        internal static bool StolenVehiclePursuit = true;
        internal static bool VehicleTheftPursuit = true;
        internal static bool VehicleTheftPursuit2 = true;
        internal static bool PursuitInProgress = true;
        internal static InitializationFile ini;
        internal static string inipath = "Plugins/LSPDFR/DeputyRyansPursuitCallouts.ini";


        internal static void LoadSettings()
        {
            Game.Console.Print("[LOG]: Loading config file from DeputyRyansPursuitCallouts.");
            var path = "Plugins/LSPDFR/DeputyRyansPursuitCallouts.ini";
            var ini = new InitializationFile(path);
            ini.Create();
            Game.LogTrivial("Initializing Config for DeputyRyansPursuitCallouts....");
            Settings.ArmedRobberyGetaway = ini.ReadBoolean("Callouts", "ArmedRobberyGetaway", true);
            Settings.DrunkDriverPursuit = ini.ReadBoolean("Callouts", "DrunkDriverPursuit", true);
            Settings.HitAndRunPursuit = ini.ReadBoolean("Callouts", "HitAndRunPursuit", true);
            Settings.LargeVehiclePursuit = ini.ReadBoolean("Callouts", "LargeVehiclePursuit", true);
            Settings.LargeVehiclePursuit2 = ini.ReadBoolean("Callouts", "LargeVehiclePursuit2", true);
            Settings.PursuitInProgress = ini.ReadBoolean("Callouts", "PursuitInProgress", true);
            Settings.RecklessDrivingPursuit = ini.ReadBoolean("Callouts", "RecklessDrvingPursuit", true);
            Settings.RoadRagePursuit = ini.ReadBoolean("Callouts", "RoadRagePursuit", true);
            Settings.StolenPoliceVehiclePursuit = ini.ReadBoolean("Callouts", "StolenPoliceVehiclePursuit", true);
            Settings.StolenVehiclePursuit = ini.ReadBoolean("Callouts", "StolenVehiclePursuit", true);
            Settings.VehicleTheftPursuit = ini.ReadBoolean("Callouts", "VehicleTheftPursuit", true);
            Settings.VehicleTheftPursuit2 = ini.ReadBoolean("Callouts", "VehicleTheftPursuit2", true);
        }

        public static readonly string PluginVersion = "Beta 4";
    }
}