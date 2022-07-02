# 0.5.18 Changes #

* Modified the move Petal Dance, Outrage, Thrash to always move the user
* Modified the move Hidden Power to be random every new floor, but the same within that floor
* Modified the item Power Herb to only prevent forced PP drops, and also prevent recharging
* Lum Berry can be used on targets with statuses that prevent them from using items
* Edible items thrown at allies will not be caught and always used
* Fixed inventory overflow issues with gifted items and Honey Gather
* Iron Ball lowers attack speed, not movement speed
* Pass Scarf costs hunger on activation
* Moxie activates half of the time, requiring moves
* Fix AI erroring for Mime Jr., Delcatty, and similar enemies
* Fix AI for projectile attackers that find themselves blocked by an opponent with an immunity, ex. Yamna vs. ghosts
* Moved Mute Music key to F8, and made Pause, Frame Advance, Speed Down and Speed Up keys available out of dev mode
* Prevent Ditto from photobombing team pictures
* Fixed an issue where PP-saving exclusive items prevented PP from dropping altogether
* Fix an issue with Nicknames not showing on enemies
* The updater now backs up the PMDO/SAVE/ directory to SAVE.bak/
* Fixed an issue with reserialization that nullified mod data on error
* Dungeon Editor: PlaceTerrainMobsStep added to spawn enemies in specific type of terrain
* Dungeon Editor: RoomGenLoadMap allowed to prevent changes to the room
* Dungeon Editor: Can create wrapped random maps
* Map Editor: Stats preview for adding entities