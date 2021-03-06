using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Reflection;
// the wall of disabled warnings
// ReSharper disable RedundantAssignment
// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace SkyrimSELauncher
{
    internal static class Program
    {
        private const decimal Version = 1.21m;

        private static string _realDir = "";
        // set console color templates
        private const ConsoleColor ErrorColor = ConsoleColor.Red;
        private const ConsoleColor InfoColor = ConsoleColor.Blue;
        private const ConsoleColor WarningColor = ConsoleColor.Yellow;
        private const ConsoleColor SuccessColor = ConsoleColor.Green;
        static string GetStringFromList(string[] str)
        {
            var newStr = "";
            if (str.Length > 0)
            {
                foreach (Object obj in str)
                {
                    newStr = newStr + obj + " ";
                }
            }

            return newStr;
        }
        
        static void Main(string[] args)
        {
            try
            {
                var message2 = "";
                var arguments = GetStringFromList(args);
                
                Console.Clear();

                if (!arguments.Contains("-noupdate"))
                {

                    if (Debugger.IsAttached && !arguments.Contains("-debugupdate")) // if debugger is attached and args don't have -debugupdate
                    {
                        bool CanCheckForUpdates = false;
                        if (CanCheckForUpdates)
                        {
                            if (CheckForUpdates())
                            {
                                message2 = "[6]: Update to the latest version\n         ";
                            }
                            else
                            {
                                message2 = "";
                            }
                        }
                        else
                        {
                            HandleMessage(InfoColor,
                                "Update check failed (set CanCheckForUpdates to true or add -debugupdate to enable updates)");
                        }
                    }
                    else
                    {
                        if (CheckForUpdates())
                        {
                            message2 = "[6]: Update to the latest version\n         ";
                        }
                        else
                        {
                            message2 = "";
                        }
                    }
                }
                else
                {
                    HandleMessage(InfoColor, "Update check failed (opted out).");
                }
                var name = Path.GetFileName(Assembly.GetExecutingAssembly().Location); 
                if (!File.Exists("SkyrimSE.exe")) // if it's not in the Skyrim SE folder
                {
                    _realDir = Directory.GetCurrentDirectory();
                    HandleMessage(InfoColor, "Please insert directory of Skyrim SE.\n         Press enter to use current directory.");
                    var defaultBkColor = Console.BackgroundColor;
                    var defaultFgColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write("  Input:");
                    Console.ForegroundColor = defaultFgColor;
                    Console.BackgroundColor = defaultBkColor;
                    Console.Write(" ");
                    SpecifyDir(name);
                }
                else // if it's in the Skyrim SE folder
                {
                    if (File.Exists("SkyrimSELauncher.exe"))
                    {
                        var skyrimSettingsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My Games\Skyrim Special Edition";

                        if (arguments.Contains("-vanilla"))
                        {
                            HandleMessage(SuccessColor, "Opening Skyrim.");
                            Process.Start("SkyrimSELauncher_real.exe");
                            HandleMessage(InfoColor, "It is now safe to close this window.");
                            Environment.Exit(0);
                        }
                        if (arguments.Contains("-skse"))
                        {
                            if (File.Exists("skse64_loader.exe"))
                            {
                                HandleMessage(SuccessColor, "Opening SKSE.");
                                Process.Start("skse64_loader.exe");
                                HandleMessage(InfoColor, "It is now safe to close this window.");
                                Environment.Exit(0);
                            }
                            else
                            {
                                HandleMessage(ErrorColor, "Cannot find SKSE. Opening browser window.");
                                Process.Start("explorer", "https://skse.silverlock.org/");
                                HandleMessage(InfoColor, "It is now safe to close this window."); 
                                Environment.Exit(0);
                            }
                        }
                        if (arguments.Contains("-viewinstall"))
                        {
                            HandleMessage(SuccessColor, "Opening directory of Skyrim.");
                            Process.Start("explorer", Directory.GetCurrentDirectory());
                            HandleMessage(InfoColor, "It is now safe to close this window.");
                            Environment.Exit(0);
                        }
                        if (arguments.Contains("-viewconfig"))
                        {
                            HandleMessage(SuccessColor, "Opening configuration location.");
                            Process.Start("explorer", skyrimSettingsPath);
                            HandleMessage(InfoColor, "It is now safe to close this window.");
                            Environment.Exit(0);
                        }   

                        if (arguments.Contains("-uninstall"))
                        {
                            Console.WriteLine(arguments);
                            if (arguments.Contains("-iknowwhatimdoing"))
                            {
                                HandleMessage(InfoColor, "Uninstalling. Please wait.");
                                var pslocation = $@"{Environment.GetFolderPath(Environment.SpecialFolder.System)}\WindowsPowerShell\v1.0";
                                Process.Start($@"{pslocation}\powershell.exe", "-command \"Write-Host \"Uninstalling, please wait...\"\"; \"Stop-Process -Name SkyrimSELauncher.exe -Force -ErrorAction SilentlyContinue\"; \"Write-Host \"Stopped process.\"\"; \"Remove-Item SkyrimSELauncher.exe -Force\"; \"Write-Host \"Removed fake launcher.\"\"; \"Rename-Item SkyrimSELauncher_real.exe SkyrimSELauncher.exe\"; \"Write-Host \"Replaced fake launcher with real one.\"\";");
                                Environment.Exit(0);
                                // powershell.exe -command "Write-Host "Uninstalling, please wait...""; "Stop-Process -Name SkyrimSELauncher.exe -Force -ErrorAction SilentlyContinue"; "Write-Host "Stopped process.""; "Remove-Item SkyrimSELauncher.exe -Force"; "Write-Host "Removed fake launcher.""; "Rename-Item SkyrimSELauncher_real.exe SkyrimSELauncher.exe"; "Write-Host "Replaced fake launcher with real one."";
                            }
                            else
                            {
                                HandleMessage(WarningColor, "This will revert the changes done by the launcher, as if you never used it.\n         To uninstall, add -iknowwhatimdoing to the arguments.\n         Press any key to exit.");
                                Console.ReadKey();
                                HandleMessage(InfoColor, "It is now safe to close this window.");
                                Environment.Exit(0);
                            }
                        }

                        if (arguments.Contains("-nevergonnagiveyouupnevergonnaletyoudown")) // never gonna run around and desert you
                        {
                            HandleMessage(WarningColor, "Update in progress. Do not close this window.");
                            File.Delete("SkyrimSELauncher.exe");
                            File.Copy("SkyrimSELauncher_update.exe", "SkyrimSELauncher.exe");
                            HandleMessage(SuccessColor, "Update complete. Restarting...");
                            Process.Start("SkyrimSELauncher.exe");
                            Environment.Exit(0);
                        }
                        else if (File.Exists("SkyrimSELauncher_update.exe"))
                        {
                            File.Delete("SkyrimSELauncher_update.exe");
                        }
                        
                        HandleMessage(SuccessColor, "Skyrim SE detected.");
                    
                        HandleMessage(SuccessColor, $"Got save directory: {skyrimSettingsPath}");
                        var message1 = "";
                        if (!File.Exists("./skse64_loader.exe"))
                        {
                            HandleMessage(WarningColor, "SKSE not found. You can download it by pressing 1.");
                            message1 = "Download SKSE (opens browser window)";
                        }
                        else
                        {
                            message1 = "Play Skyrim with SKSE";
                        }

                        if (!File.Exists($"{skyrimSettingsPath}/Skyrim.ini"))
                        {
                            HandleMessage(WarningColor, "Skyrim.ini not found. It is recommended to run the game at least once without SKSE.");
                        }

                        if (!File.Exists($"{skyrimSettingsPath}/SkyrimPrefs.ini"))
                        {
                            HandleMessage(WarningColor, "SkyrimPrefs.ini not found. It is recommended to run the game at least once without SKSE.");
                        }
                        HandleMessage(InfoColor, "What would you like to do?\n         " +
                                                     $"[1]: {message1}\n         " +
                                                     "[2]: Play Skyrim without SKSE (opens real launcher)\n         " +
                                                     "[3]: View Skyrim installation folder\n         " +
                                                     "[4]: View Skyrim save/config folder\n         " +
                                                     "[5]: Exit\n         " + 
                                                     $"{message2}" + 
                                                     "[9]: Uninstall and revert changes");
                        var defaultBkColor = Console.BackgroundColor;
                        var defaultFgColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("  Input:");
                        Console.ForegroundColor = defaultFgColor;
                        Console.BackgroundColor = defaultBkColor;
                        Console.Write(" ");
                        switch (Console.ReadLine())
                        {
                            case "1":
                            switch (message1)
                            {
                                case "Download SKSE (opens browser window)":
                                    Process.Start("explorer","https://skse.silverlock.org/");
                                    break;
                                case "Play Skyrim with SKSE":
                                    Process.Start("skse64_loader.exe");
                                    break;
                                default:
                                    throw new NullReferenceException();
                            }
                            Environment.Exit(0);
                            break;
                            case "2":
                                Process.Start("SkyrimSELauncher_real.exe");
                                HandleMessage(InfoColor, "It is now safe to close this window.");
                                Environment.Exit(0);
                                break;
                            case "3":
                                Process.Start("explorer", Directory.GetCurrentDirectory());
                                HandleMessage(InfoColor, "It is now safe to close this window.");
                                Environment.Exit(0);
                                break;
                            case "4":
                                Process.Start("explorer", skyrimSettingsPath);
                                HandleMessage(InfoColor, "It is now safe to close this window.");
                                Environment.Exit(0);
                                break;
                            case "5":
                                HandleMessage(InfoColor, "It is now safe to close this window.");
                                Environment.Exit(0);
                                break;
                            case "6":
                                switch (message2)
                                {
                                    case "": // no update available
                                        Main(args);
                                        break;
                                    case "[6]: Update to the latest version\n         ":
                                        HandleMessage(WarningColor, "Update in progress. Do not close this window.");
                                        WebClient webClient = new WebClient();
                                        var latestVersion = webClient.DownloadString("https://raw.githubusercontent.com/thepwrtank18/SkyrimSELauncher/master/version.txt");
                                        string cleanedLink = $"https://github.com/thepwrtank18/SkyrimSELauncher/releases/download/{latestVersion}/SkyrimSELauncher_fake.exe".Replace("\n", "").Replace("\r", "");
                                        Console.WriteLine(cleanedLink);
                                        webClient.DownloadFile(
                                            cleanedLink, "SkyrimSELauncher_update.exe");
                                        Process.Start("SkyrimSELauncher_update.exe", "-nevergonnagiveyouupnevergonnaletyoudown");
                                        Environment.Exit(0);
                                        break;
                                }
                                break;
                            case "9":
                                Console.Clear();
                                HandleMessage(WarningColor, "This will revert the changes done by the launcher, as if you never used it.\n         Are you sure?\n         [N]: No\n         [Y]: Yes (to prevent accidental uninstallation, press the Shift key while doing so)");
                                Uninstall(defaultFgColor, defaultBkColor, args);
                                break;
                            default:
                                Main(args);
                                break;
                        }
                    }
                    else
                    {
                        HandleMessage(ErrorColor, "File must be named \"SkyrimSELauncher.exe\". Please try again.");
                        Console.ReadKey();
                        HandleMessage(InfoColor, "It is now safe to close this window.");
                        Environment.Exit(0);
                    }

                }
            }
            catch (Exception e)
            {
                HandleMessage(ErrorColor,
                    $"A fatal error has occurred. It is being logged to \"LauncherLog.txt\" in your Skyrim directory.\n         Error: {e}");
                if (!File.Exists("./LauncherLog.txt")) File.Create("./LauncherLog.txt").Dispose();
                File.WriteAllText("./LauncherLog.txt", e.ToString());
                Thread.Sleep(3000);
                Environment.Exit(1);
            }
        }

        static bool CheckForUpdates()
        {
            try
            {
                WebClient webClient = new WebClient();
                string latestVersionStr =
                    webClient.DownloadString(
                        "https://raw.githubusercontent.com/thepwrtank18/SkyrimSELauncher/master/version.txt");
                decimal latestVersion = Convert.ToDecimal(latestVersionStr);
            
                if (latestVersion > Version)
                {
                    HandleMessage(WarningColor, $"A new version of the launcher is available!\n         " +
                                                $"Your version is {Version}. The latest version is {latestVersion}.\n         " +
                                                $"You can download the latest version from https://github.com/thepwrtank18/SkyrimSELauncher/releases/tag/{latestVersionStr}");
                    return true;
                }
                else if (latestVersion < Version)
                {
                    HandleMessage(WarningColor, "Update check failed (newer than latest version).");
                    return false;
                }
                else
                {
                    HandleMessage(SuccessColor, "You are running the latest version of the launcher.");
                    return false;
                }
            }
            catch (WebException)
            {
                HandleMessage(ErrorColor, "Update check failed (Failed to connect).");
                return false;
            }
        }
        
        static void Uninstall(ConsoleColor defaultFgColor, ConsoleColor defaultBkColor, string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("  Input:");
            Console.ForegroundColor = defaultFgColor;
            Console.BackgroundColor = defaultBkColor;
            Console.Write(" ");
            switch (Console.ReadLine())
            {
                case "Y":
                    var pslocation = $@"{Environment.GetFolderPath(Environment.SpecialFolder.System)}\WindowsPowerShell\v1.0";
                    Process.Start($@"{pslocation}\powershell.exe", "-command \"Write-Host \"Uninstalling, please wait...\"\"; \"Stop-Process -Name SkyrimSELauncher.exe -Force -ErrorAction SilentlyContinue\"; \"Write-Host \"Stopped process.\"\"; \"Remove-Item SkyrimSELauncher.exe -Force\"; \"Write-Host \"Removed fake launcher.\"\"; \"Rename-Item SkyrimSELauncher_real.exe SkyrimSELauncher.exe\"; \"Write-Host \"Replaced fake launcher with real one.\"\";");
                    // powershell.exe -command "Write-Host "Uninstalling, please wait...""; "Stop-Process -Name SkyrimSELauncher.exe -Force -ErrorAction SilentlyContinue"; "Write-Host "Stopped process.""; "Remove-Item SkyrimSELauncher.exe -Force"; "Write-Host "Removed fake launcher.""; "Rename-Item SkyrimSELauncher_real.exe SkyrimSELauncher.exe"; "Write-Host "Replaced fake launcher with real one."";
                    break;
                case "y":
                    HandleMessage(ErrorColor, "To confirm, press the Shift key while pressing Y.");
                    Uninstall(defaultFgColor, defaultBkColor, args);
                    break;
                default:
                    Main(args); // calls itself to restart program
                    break;
            }
        }

        static void SpecifyDir(string name)
        {
            var defaultBkColor = Console.BackgroundColor;
            var defaultFgColor = Console.ForegroundColor;
            string directory = Console.ReadLine();
            try
            {
                if (!string.IsNullOrEmpty(directory)) Directory.SetCurrentDirectory(directory);
            }
            catch (DirectoryNotFoundException)
            {
                HandleMessage(ErrorColor, "Directory does not exist.");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("  Input:");
                Console.ForegroundColor = defaultFgColor;
                Console.BackgroundColor = defaultBkColor;
                Console.Write(" ");
                SpecifyDir(name);
            }
            catch (ArgumentException)
            {
                HandleMessage(ErrorColor, "Directory is unreadable.");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("  Input:");
                Console.ForegroundColor = defaultFgColor;
                Console.BackgroundColor = defaultBkColor;
                Console.Write(" ");
                SpecifyDir(name);
            }
            catch (IOException)
            {
                HandleMessage(ErrorColor, "Error occured reading directory.");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("  Input:");
                Console.ForegroundColor = defaultFgColor;
                Console.BackgroundColor = defaultBkColor;
                Console.Write(" ");
                SpecifyDir(name);
            }

            if (!File.Exists("./SkyrimSELauncher.exe"))
            {
                HandleMessage(ErrorColor, "Real launcher not found in directory.");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("  Input:");
                Console.ForegroundColor = defaultFgColor;
                Console.BackgroundColor = defaultBkColor;
                Console.Write(" ");
                SpecifyDir(name);
            }
            else
            {
                if (!File.Exists("./SkyrimSELauncher_real.exe"))
                {
                    File.Move("./SkyrimSELauncher.exe", "./SkyrimSELauncher_real.exe");
                    HandleMessage(InfoColor, "Copied real launcher.");
                }
                else
                {
                    HandleMessage(InfoColor, "Real launcher already exists, ignoring.");
                }
                if (File.Exists("./SkyrimSELauncher.exe"))
                {
                    File.Delete("./SkyrimSELauncher.exe");
                }
                File.Copy($"{_realDir}/{name}", "./SkyrimSELauncher.exe");
                HandleMessage(InfoColor, "Copied fake launcher.");
                Process.Start($"{Directory.GetCurrentDirectory()}/SkyrimSELauncher.exe");
                HandleMessage(SuccessColor, "Starting!");
                HandleMessage(InfoColor, "It is now safe to close this window.");
                Environment.Exit(0);
            }
        }

        static void HandleMessage(ConsoleColor background, string info)
        {
            ConsoleColor defaultBkColor = Console.BackgroundColor;
            ConsoleColor defaultFgColor = Console.ForegroundColor;
            Console.BackgroundColor = background;
            Console.ForegroundColor = ConsoleColor.Black;
            switch (background)
            {
                case ConsoleColor.Red:
                    Console.Write("  Error:");
                    break;
                case ConsoleColor.Blue:
                    Console.Write(" Notice:");
                    break;
                case ConsoleColor.Green:
                    Console.Write("Success:");
                    break;
                case ConsoleColor.Yellow:
                    Console.Write("Warning:");
                    break;
                default:
                    throw new NullReferenceException("Invalid console color. Must be Red, Blue, Green or Yellow.");
            }
            Console.BackgroundColor = defaultBkColor;
            Console.ForegroundColor = background;
            Console.Write(" " + info + "\n");
            Console.ForegroundColor = defaultFgColor;
        }
    }
}
