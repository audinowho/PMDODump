# 0.5.15 Changes #

* Modified the moves Protect, Detect, Sucker Punch, Wake-up Slap, Earth Power, Muddy Water
* Fixed AI targeting of moves such as Fake Out
* Sleep wears off on the afflicted's turn
* Renamed Link Box to Recall Box to reduce confusion
* Minor tweaks to encounters in Secret Garden
* Fixed autotiling issue with StableTiles
* Add developer option to make ground maps wrap around.  Dungeon map wrapping is WIP.
* Added documentation for move VFX and map editor
* Maps can be loaded as custom rooms into a floor generator.  Use RoomGenLoadMap
* Temprary Object support with GroundMap:AddTempObject, GroundMap:RemoveTempObject, GroundMap:GetTempObj, etc.
* Death is now processed in Universal, with PreDeath and SetDeath events.  Mods that modify Universal.bin will need to update OnDeath with these events.
