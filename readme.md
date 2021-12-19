# Skyrim SE Launcher
 
Allows running SKSE and game files from Steam (Bnet is untested).

## Installation
1. Download from [releases](https://github.com/thepwrtank18/SkyrimSELauncher/releases), or find a bleeding edge (may have bugs, requires GitHub account) release [here](https://github.com/thepwrtank18/SkyrimSELauncher/actions).
2. Run the exe, and follow the instructions.
3. It now just replaced your original SE launcher with this one.
4. Done! You can see your options on the screen.

## Command Line Usage
`-vanilla` will start the real launcher.
\
`-skse` will start SKSE.
\
`-viewinstall` will open the game's folder.
\
`-viewconfig` will open the game's configuration folder (includes settings and save games).
\
`-uninstall` will uninstall the launcher, as if you never used it. Note that this requires you put `-iknowwhatimdoing` as well, as you need to know what you are doing.

These can be used by right clicking on the Steam listing for Skyrim SE, go to Properties, and you should see a place to put launch options. Put one of the above in (unless you are uninstalling, then put what you need to).

## Building
Simply open this in Visual Studio (JetBrains Rider is recommended) to edit and build the launcher.
