# 0.5.14 Changes #

* Modified the moves: Extreme Speed, Fake Out, Sucker Punch
* Fixed issue with autotile computation failing at times
* Fixed issue preventing priorities from being edited in a priority list
* When generating a floor in developer mode, the game will print out the list of steps it intends to run through
* When generating a floor, errors in generation will not prevent the rest of the floor from generating, and a fallback floor will be created if nothing is generated at all
* Dialogue text that is too long will be automatically scrolled to the next textbox
* Menu text that is too long will automatically shift the menu position
* Added documentation for move editor and item editor
* Fixed an issue that prevented enemies from being placed/moved in map editor
* Moved the following classes:
* PMDC.LevelGen.ScriptGenStep to RogueEssence.LevelGen.ScriptGenStep
* PMDC.LevelGen.ScriptZoneStep to RogueEssence.LevelGen.ScriptZoneStep
* RogueElements.IntrudingBlobWaterStep to RogueEssence.IntrudingBlobWaterStep
* In the event that this prevents dungeon loading, do a find+replace on the file.
