using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
// the great wall of disabled warnings
// ReSharper disable RedundantAssignment
// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace SkyrimSELauncher
{
    internal static class Program
    {
        private static string _realDir = "";
        // set console color templates
        private static ConsoleColor _errorColor = ConsoleColor.Red;
        private static ConsoleColor _infoColor = ConsoleColor.Blue;
        private static ConsoleColor _warningColor = ConsoleColor.Yellow;
        private static ConsoleColor _successColor = ConsoleColor.Green;

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
                Console.Clear();

                if (!File.Exists("./SkyrimSELauncher_fake.exe") && !File.Exists("SkyrimSE.exe"))
                {
                    HandleMessage(_errorColor, "Executable name must be SkyrimSELauncher_fake.exe for installation.\n         Please rename this executable.");
                    Console.ReadKey();
                    Process.Start("explorer", Directory.GetCurrentDirectory());
                    Environment.Exit(0);
                }
                else if (!File.Exists("SkyrimSE.exe"))
                {
                    _realDir = Directory.GetCurrentDirectory();
                    HandleMessage(_infoColor, "Please insert directory of Skyrim SE.\n         Press enter to use current directory.");
                    var defaultBkColor = Console.BackgroundColor;
                    var defaultFgColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write("  Input:");
                    Console.ForegroundColor = defaultFgColor;
                    Console.BackgroundColor = defaultBkColor;
                    Console.Write(" ");
                    SpecifyDir();
                }
                else
                {
                    
                    var skyrimSettingsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My Games\Skyrim Special Edition";
                    var arguments = GetStringFromList(args);
                    
                    if (arguments.Contains("-vanilla"))
                    {
                        HandleMessage(_successColor, "Opening Skyrim.");
                        Process.Start("SkyrimSELauncher_real.exe");
                        HandleMessage(_infoColor, "It is now safe to close this window.");
                        Environment.Exit(0);
                    }
                    if (arguments.Contains("-skse"))
                    {
                        if (File.Exists("skse64_loader.exe"))
                        {
                            HandleMessage(_successColor, "Opening SKSE.");
                            Process.Start("skse64_loader.exe");
                            HandleMessage(_infoColor, "It is now safe to close this window.");
                            Environment.Exit(0);
                        }
                        else
                        {
                            HandleMessage(_errorColor, "Cannot find SKSE. Opening browser window.");
                            Process.Start("explorer", "https://skse.silverlock.org/");
                            HandleMessage(_infoColor, "It is now safe to close this window.");
                            Environment.Exit(0);
                        }
                    }
                    if (arguments.Contains("-viewinstall"))
                    {
                        HandleMessage(_successColor, "Opening directory of Skyrim.");
                        Process.Start("explorer", Directory.GetCurrentDirectory());
                        HandleMessage(_infoColor, "It is now safe to close this window.");
                        Environment.Exit(0);
                    }
                    if (arguments.Contains("-viewconfig"))
                    {
                        HandleMessage(_successColor, "Opening configuration location.");
                        Process.Start("explorer", skyrimSettingsPath);
                        HandleMessage(_infoColor, "It is now safe to close this window.");
                        Environment.Exit(0);
                    }

                    if (arguments.Contains("-uninstall"))
                    {
                        Console.WriteLine(arguments);
                        if (arguments.Contains("-iknowwhatimdoing"))
                        {
                            HandleMessage(_infoColor, "Uninstalling. Please wait.");
                            var pslocation = $@"{Environment.GetFolderPath(Environment.SpecialFolder.System)}\WindowsPowerShell\v1.0";
                            Process.Start($@"{pslocation}\powershell.exe", "-command \"Write-Host \"Uninstalling, please wait...\"\"; \"Stop-Process -Name SkyrimSELauncher.exe -Force -ErrorAction SilentlyContinue\"; \"Write-Host \"Stopped process.\"\"; \"Remove-Item SkyrimSELauncher.exe -Force\"; \"Write-Host \"Removed fake launcher.\"\"; \"Rename-Item SkyrimSELauncher_real.exe SkyrimSELauncher.exe\"; \"Write-Host \"Replaced fake launcher with real one.\"\";");
                            Environment.Exit(0);
                            // powershell.exe -command "Write-Host "Uninstalling, please wait...""; "Stop-Process -Name SkyrimSELauncher.exe -Force -ErrorAction SilentlyContinue"; "Write-Host "Stopped process.""; "Remove-Item SkyrimSELauncher.exe -Force"; "Write-Host "Removed fake launcher.""; "Rename-Item SkyrimSELauncher_real.exe SkyrimSELauncher.exe"; "Write-Host "Replaced fake launcher with real one."";
                        }
                        else
                        {
                            HandleMessage(_warningColor, "This will revert the changes done by the launcher, as if you never used it.\n         To uninstall, add -iknowwhatimdoing to the arguments.\n         Press any key to exit.");
                            Console.ReadKey();
                            HandleMessage(_infoColor, "It is now safe to close this window.");
                            Environment.Exit(0);
                        }
                    }

                    HandleMessage(_successColor, "Skyrim SE detected.");
                    
                    HandleMessage(_successColor, $"Got save directory: {skyrimSettingsPath}");
                    var message1 = "";
                    if (!File.Exists("./skse64_loader.exe"))
                    {
                        HandleMessage(_warningColor, "SKSE not found. You can download it by pressing 1.");
                        message1 = "Download SKSE (opens browser window)";
                    }
                    else
                    {
                        message1 = "Play Skyrim with SKSE";
                    }

                    if (!File.Exists($"{skyrimSettingsPath}/Skyrim.ini"))
                    {
                        HandleMessage(_warningColor, "Skyrim.ini not found. It is recommended to run the game at least once without SKSE.");
                    }

                    if (!File.Exists($"{skyrimSettingsPath}/SkyrimPrefs.ini"))
                    {
                        HandleMessage(_warningColor, "SkyrimPrefs.ini not found. It is recommended to run the game at least once without SKSE.");
                    }
                    HandleMessage(_infoColor, "What would you like to do?\n         " +
                                                     $"[1]: {message1}\n         " +
                                                     "[2]: Play Skyrim without SKSE (opens real launcher)\n         " +
                                                     "[3]: View Skyrim installation folder\n         " +
                                                     "[4]: View Skyrim save/config folder\n         " +
                                                     "[5]: Exit\n         " + 
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
                            HandleMessage(_infoColor, "It is now safe to close this window.");
                            Environment.Exit(0);
                            break;
                        case "3":
                            Process.Start("explorer", Directory.GetCurrentDirectory());
                            HandleMessage(_infoColor, "It is now safe to close this window.");
                            Environment.Exit(0);
                            break;
                        case "4":
                            Process.Start("explorer", skyrimSettingsPath);
                            HandleMessage(_infoColor, "It is now safe to close this window.");
                            Environment.Exit(0);
                            break;
                        case "5":
                            HandleMessage(_infoColor, "It is now safe to close this window.");
                            Environment.Exit(0);
                            break;
                        case "9":
                            Console.Clear();
                            HandleMessage(_warningColor, "This will revert the changes done by the launcher, as if you never used it.\n         Are you sure?\n         [N]: No\n         [Y]: Yes (to prevent accidental uninstallation, press shift key while doing so)");
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
                                default:
                                    Main(args); // calls itself to restart program
                                    break;
                            }
                            break;
                        default:
                            HandleMessage(_infoColor, "It is now safe to close this window.");
                            Environment.Exit(0);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                HandleMessage(_errorColor,
                    $"A fatal error has occurred. It is being logged to \"LauncherLog.txt\" in your Skyrim directory.\n         Error: {e}");
                if (!File.Exists("./LauncherLog.txt")) File.Create("./LauncherLog.txt").Dispose();
                File.WriteAllText("./LauncherLog.txt", e.ToString());
                Thread.Sleep(3000);
                Environment.Exit(1);
            }
        }

        static void SpecifyDir()
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
                HandleMessage(_errorColor, "Directory does not exist.");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("  Input:");
                Console.ForegroundColor = defaultFgColor;
                Console.BackgroundColor = defaultBkColor;
                Console.Write(" ");
                SpecifyDir();
            }

            if (!File.Exists("./SkyrimSELauncher.exe"))
            {
                HandleMessage(_errorColor, "Real launcher not found in directory.");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("  Input:");
                Console.ForegroundColor = defaultFgColor;
                Console.BackgroundColor = defaultBkColor;
                Console.Write(" ");
                SpecifyDir();
            }
            else
            {
                if (!File.Exists("./SkyrimSELauncher_real.exe"))
                {
                    File.Move("./SkyrimSELauncher.exe", "./SkyrimSELauncher_real.exe");
                    HandleMessage(_infoColor, "Copied real launcher.");
                }
                else
                {
                    HandleMessage(_infoColor, "Real launcher already exists, ignoring.");
                }
                if (File.Exists("./SkyrimSELauncher.exe"))
                {
                    File.Delete("./SkyrimSELauncher.exe");
                }
                File.Copy($"{_realDir}/SkyrimSELauncher_fake.exe", "./SkyrimSELauncher.exe");
                HandleMessage(_infoColor, "Copied fake launcher.");
                Process.Start($"{Directory.GetCurrentDirectory()}/SkyrimSELauncher.exe");
                HandleMessage(_successColor, "Starting!");
                HandleMessage(_infoColor, "It is now safe to close this window.");
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
