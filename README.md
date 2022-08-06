# PMD: Origins #

---

Open source Pokémon roguelike with modding tools.

Thread on Pokécommunity
https://www.pokecommunity.com/showthread.php?p=10325347

Tutorials on modding
https://drive.google.com/drive/folders/1Oq0AhABTGbTIVsnYsZaAtTO5C2TH7bH_?ths=true

This repository contains only code that builds the data used in PMDO, its updater, and scripts that are run to deploy it.  It does not contain the base engine of the game!  The submodules supply those things:

Submodules
* PMDC: Contains battle system code roguelike engine that PMDO runs on.
* DumpAsset: Contains the full asset data for PMDO.
* RawAsset: Contains unconverted graphics.

Pull all submodules recursively.
* Run `git submodule update --init --recursive` to get all the submodules.
* You may need to regenerate NuGet packages for the RogueEssence solution first, before building.

Building Game
* Run `dotnet publish -c Release -r win-x86 PMDC/PMDC/PMDC.csproj` to publish to Windows x86.
* Run `dotnet publish -c Release -r win-x64 PMDC/PMDC/PMDC.csproj` to publish to Windows.
* Run `dotnet publish -c Release -r linux-x64 PMDC/PMDC/PMDC.csproj` to publish to Linux.
* Run `dotnet publish -c Release -r osx-x64 PMDC/PMDC/PMDC.csproj` to publish to Mac.
* Files will appear in the `publish` folder.

Building Server
* Run `dotnet publish -c Release -r win-x64 PMDC/RogueEssence/WaypointServer/WaypointServer.csproj` to publish to Windows.
* Run `dotnet publish -c Release -r linux-x64 PMDC/RogueEssence/WaypointServer/WaypointServer.csproj` to publish to Linux.

Building Installer/Updater
* Run `dotnet publish -c Release -r win-x64 PMDOSetup/PMDOSetup.csproj` to publish to Windows.
* Run `dotnet publish -c Release -r linux-x64 PMDOSetup/PMDOSetup.csproj` to publish to Linux.
* Run `dotnet publish -c Release -r osx-x64 PMDOSetup/PMDOSetup.csproj` to publish to Linux.
* Files will appear in the `publish` folder.

DataGenerator and MapGenTest are two projects not meant to be deployed.
* DataGenerator is used to construct data files of all dungeons, Pokémon, etc.
* MapGenTest is used to bulk test and debug dungeon maps.

DataGenerator Deployment Order
* One-time: Run `-itemprep` to generate monster/status/element tables needed for items.
* Run `Scripts/item_sync.py` to update exclusive item spreadsheet with data generated above. It will generate a csv of exclusive items to be used in the `-dump` step.

* Reserialize Skills and Monster (Or regenerate Monster) using `-reserialize Skill` or `-reserialize Monster`
* Dump all data using `-dump`.  It depends on the csv of exclusive items to generate that exclusive items (item creation). It also generates an XML to map species to family items (spawning lookup), and a common_gen.lua containing tables of generic trades and specific trades.

* Generate tables for string merge with `-strings out`.
* Sync the translation table using `Scripts/strings_sync.py`
* Uptake tables for string merge with `-strings in`.