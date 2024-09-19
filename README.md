# AffinityEditor

A simple mod that displays your affinity values numerically and in relation to affinity-based unlocks. It also allows you to edit characters' affinities.
## Features
![image](https://github.com/Comahar/AffinityEditor/blob/main/.github/AffinityEditor.jpg)
- **View Affinity Points:** Displays each character's affinity points numerically, providing an easy way to track them while playing.
- **Show Affinity Unlocks:** Shows you when an unlock will happen and how many points it requires.
- **Edit Affinity Points:** Modify affinity points for each character through the interface.

## Installation
### Thunderstore
- Download one of these mod managers
	- [ebkr/r2modmanPlus: A simple and easy to use mod manager for several games using Thunderstore](https://github.com/ebkr/r2modmanPlus) [(Release)](https://github.com/ebkr/r2modmanPlus/releases/latest)
	-  [Thunderstore Mod Manager - Desktop App on Overwolf](https://www.overwolf.com/app/Thunderstore-Thunderstore_Mod_Manager)
- Select *Goodbye Volcano High*
- Download [AffinityEditor](https://thunderstore.io/c/goodbye-volcano-high/p/Comahar/AffinityEditor/)
### Manual
- Download and install [BepinEx](https://docs.bepinex.dev/articles/user_guide/installation/index.html).
    - You can also follow onyx-mp4's [BepinEx installation guide](https://onyx-mp4.github.io/?scene=bepinex-unzip).
- Download latest release from [Releases](https://github.com/Comahar/AffinityEditor/releases/latest).
- Extract the zip file to your game folder.

## Credits
- Special thanks to `@hadradavus` for their input and [affinity guide](https://steamcommunity.com/sharedfiles/filedetails/?id=3038565144).
## Development
- Ignore the errors when opening the project.
- Install BepInEx
	- Tools > ThunderKit > Packages
	- ThunderKit Extensions > Bep In Ex Pack > Install
- Import Game
	- Tools > ThunderKit > Settings
	- ThunderKit Settings
	- Select Game exe
	- Import
	- Restart when prompted and ignore Safe Mode warnings.
- If you encounter the error `The type 'HarmonyPatch' exists in both '0Harmony ...' and '0Harmony20 ...'`, deleting `Packages/BepInExPack/BepInEx/core/0Harmony20.dll` should resolve the issue.

### Bugs
- [ ] Close the interface switching between tabs.
- [ ] Selectable objects blinking when UI opens.
- [ ] Editor open button position may be inconsistent.
- [ ] Indicator texts may be outside the screen.

### Roadmap
- [ ] Localization support.
- [ ] Gamepad support (maybe).
- [ ] Mark save files as dirty if editing is used.
- [ ] Disable achievements if the save file is marked as dirty.

## License
[LGPL-3.0](https://github.com/Comahar/AffinityEditor/blob/main/LICENSE)