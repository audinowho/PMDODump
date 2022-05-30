# 0.5.14 Changes #

* Fixed issue with autotile computation failing at times
* Fixed issue preventing priorities from being edited in a priority list
* When generating a floor in developer mode, the game will print out the list of steps it intends to run through
* When generating a floor, errors in generation will not prevent the rest of the floor from generating
* Moved PMDC.LevelGen.ScriptGenStep and PMDC.LevelGen.ScriptZoneStep to RogueEssence.LevelGen.ScriptGenStep and RogueEssence.LevelGen.ScriptZoneStep. In the event that this prevents dungeon loading, do a find+replace on the file.
