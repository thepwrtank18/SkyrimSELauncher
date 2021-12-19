# SkyrimSELauncher
 
Allows running SKSE (and more) from Steam.

## Compatibility
SkyrimSELauncher is compatible with the Steam release. The BNet and Microsoft versions are untested. You are welcome to try, though.

## Installation
### Automatic Installation
1. Download from [releases](https://github.com/thepwrtank18/SkyrimSELauncher/releases), or find a bleeding edge (may have bugs, requires GitHub account) release [here](https://github.com/thepwrtank18/SkyrimSELauncher/actions).
2. Run the exe, and follow the instructions.
3. It now just replaced your original SE launcher with this one, so you can run it from Steam now.
4. Done! You can see your options on the screen.
### Manual Installation
1. Download from [releases](https://github.com/thepwrtank18/SkyrimSELauncher/releases), or find a bleeding edge (may have bugs, requires GitHub account) release [here](https://github.com/thepwrtank18/SkyrimSELauncher/actions).
2. Put the exe in Skyrim SE's directory.
3. Rename `SkyrimSELauncher.exe` to `SkyrimSELauncher_real.exe`.
4. Rename `SkyrimSELauncher_fake.exe` to `SkyrimSELauncher.exe`.
5. Done!

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
\
`-noupdate` will disable checking for updates.
These can be used by right clicking on the Steam listing for Skyrim SE, go to Properties, and you should see a place to put launch options. Note that the top 4 arguments will conflict eachother, so you can only use one of those 4.

## Building
Simply open this in Visual Studio (JetBrains Rider is recommended) to edit and build the launcher.
