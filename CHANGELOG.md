# 0.5.8 Changes #

* Outdated replays will no longer outright fail on load
* Fix shopkeepers asking twice to pay for sold items
* Fix free items appearing in shops
* Explosions that destroy walls will not destroy items inside walls.
* Moves that cannot miss will hit through abilities/status that cause automatic miss
* Fix error with switching starters when selecting alternate forms
* If a save file is loaded with missing mods, the player will be warned
* When save files are upgraded, they will trigger the UpgradeSave callback
* Fix issue with script generation when saving a newly created ground map.
* Added script calls: DUNGEON:CharStartAnim, CharEndAnim, PlayVFX, etc.