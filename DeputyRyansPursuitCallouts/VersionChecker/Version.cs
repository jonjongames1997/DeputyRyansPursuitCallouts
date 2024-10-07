namespace DeputyRyansPursuitCallouts.VersionChecker
{
    public class PluginCheck
    {
        // Check for updates
        public static bool IsUpdateAvailable()
        {
            string curVersion = Settings.PluginVersion;
            Uri latestVersionUri = new Uri("https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=48930&textOnly=1"); // URL to store the current version
            WebClient webClient = new WebClient();
            string receivedData = string.Empty;

            try
            {
                receivedData = webClient.DownloadString(latestVersionUri).Trim();
            }
            catch (WebException)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~DeputyRyansPursuitCallouts Warning", "~r~Failed to check for an update", "Please ensure you are ~y~connected~w~ to the internet or try to ~y~reload~w~ the plugin.");
                Game.Console.Print();
                Game.Console.Print("================================================== DeputyRyansPursuitCallouts ===================================================");
                Game.Console.Print();
                Game.Console.Print("[WARNING]: Failed to check for an update.");
                Game.Console.Print("[LOG]: Please ensure you are connected to the internet or try to reload the plugin.");
                Game.Console.Print();
                Game.Console.Print("================================================== DeputyRyansPursuitCallouts ===================================================");
                Game.Console.Print();
                return false;
            }

            if (recieveData != Settings.PluginVersion)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~DeputyRyansPursuitCallouts Warning", "~y~A new update is available!", "Current Version: ~r~" + curVersion + "~w~<br>New Version: ~y~" + recieveData + "<br>~r~Please Update to the latest build for new callouts and improvments!");
                Game.Console.Print();
                Game.Console.Print("===================================================== DeputyRyansPursuitCallouts ===========================================");
                Game.Console.Print();
                Game.Console.Print("[WARNING!]: A new version of DeputyRyansPursuitCallouts is NOW AVAILABLE to download!");
                Game.Console.Print("[LOG]: Current Version:" + curVersion);
                Game.Console.Print("[LOG]: New Version:" + recieveData);
                Game.Console.Print();
                Game.Console.Print("===================================================== DeputyRyansPursuitCallouts ===========================================");
                Game.Console.Print();
                return true;
            }
            else
            {
                Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "DeputyRyansPursuitCallouts", "~g~Callouts Detected the latest build!");
                return false;
            }
        }
    }
}
