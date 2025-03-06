# 0.8.9 Changes #

* Tiny Tunnel Team Size 3 -> 2, removed the chance for enemies to spawn in groups for this dungeon
* New Exclusive Item effects
* Added the ability Cud Chew
* Heart Scale Price 50 -> 100
* Fixed an issue that prevented Friend Bow from working at all
* Fixed an issue that gave all Pokemon increased Sp. Def in Sandstorm, instead of just Rock-types.
* Fixed outlaw music in mission mod
* Fixed outlaws failing to spawn in mission mod when the first team member is fainted
* Fixed ally AI refusing to move when in the middle of hazardous terrain
* Fixed an issue where pauses displayed incorrectly on dialogueboxes
* Fixed errors occurring at Training Maze
* Fixed an issue where Pokemon spawned in Guildmaster Trail Monster Houses only had moves at level 1
* Fixed dungeons generating secret exit guards incorrectly
* Shedinja properly marked as released
* Fixed Kingambit's evo requirement
* Optimized assembly menu, summary menu, item menu, adventure completion to be less file intensive
* RC: Lowered the vitamin boosts given to bosses in Relic Tower; they were boosted on a range of level 1 to 50, when players enter with their own vitamin boosts removed.
* RC: Fixed an issue where mysterious distortions would fail to expire on wrapped maps
* RC: Snowy weather can appear naturally in certain dungeon maps. It is considered identical to Hail, except it increases Ice-types' Defense instead of damagin non-ice-type Pokemon. It cannot be triggered by any other means.
* Dev: Added RoomGenLoadBoss, an editor-friendly way to spawn boss rooms by editing a static boss map, and then selecting it as the map to load into the random dungeon.
* Dev: Added RoomGenLoadEvo, an editor-friendly way to spawn evo rooms.  The hardcoded rooms are now deprecated.
* You can now hold CTRL+Cancel (Cancel is typically mapped to Z) to clear all dialogue boxes to skip text
* Dev: Fixed errors occurring when maps are resized more than once with NPCs on them
* Dev: Improvements to ConnectBranchStep that make it work better in wrapped maps
* Dev: Stairs Steps now have a MinDistance, where you can specify how far away the exit should at least be from the start
* Dev: Fixed cleanups when restarting using CTRL+F12
* Dev: ModeInDirection, AnimateToPosition and AnimateInDirection can use float speed
* Dev: Item indexing has changed.  Be sure to reserialize your mods!