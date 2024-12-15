# 0.8.8 Changes #

* New dungeon, unlocked from where Secret Garden could once be found.
* Secret Garden is being moved to a new location.  Temporarily, it can be unlocked by finishing the new dungeon.
* Minor vault guardian rebalance
* Fixed Wooper's ability to evolve into Clodsire, even if not a Paldean form.
* Fixed an issue where errors would occur after finishing a level-reset, move-reset dungeon
* Fixed an issue where deserialization would fail on a save load.
* Fixed an issue where various unevolved Pokemon that existed in gen 7+ had lower than expected recruit rates
* RC: Secret Slab functionality
* Dev: Absolute Delay can be used for VFX that must have the specified delay even if the player is using a different battle speed.
* Dev: Soft-restart properly clears menu transparency changes
* Dev: Fixed an issue where text tags were applied out of order, such as scripts firing before pauses when specified after them.
* Dev: MusicMenu constructor change.  Check the base game for example use.
* Dev: Songs in the base game have been renamed. If you have any PMDO base music you use in your custom dungeons/maps, Do a global find/replace in your Data/ directory from: "\w\d\d\. (.+?\.ogg)", replace to: "\1"