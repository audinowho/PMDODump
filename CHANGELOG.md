# 0.5.15 Changes #

* Modified the moves Protect, Detect, Sucker Punch, Wake-up Slap, Earth Power, Muddy Water
* Sleep wears off on the afflicted's turn
* Fixed autotiling issue with StableTiles
* Add developer option to make ground maps wrap around.  Dungeon map wrapping is WIP.
* Added documentation for move VFX and map editor
* Death is now processed in Universal, with PreDeath and SetDeath events.  Mods that modify Universal.bin will need to update OnDeath with these events.
