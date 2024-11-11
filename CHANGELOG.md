# 0.8.6 Changes #

* Updated icons for countering statuses
* Fix Round: Added Sound-based flag
* Fix Powder: No longer sticks around after being hit by a fire/electric move.  Now properly prevents action when attempting a fire/electric move.  No longer causes orbital strike when triggering with range boosters.
* Poison Heal: 1/16 HP healed when resting, 1/8 HP healed otherwise.  No longer animates and does not give battle messages when at full HP.
* Fix menu showing version diffs when attempting rescue
* Self-hitting effects and traps are no longer counted as a "hit" in regards to the no-miss-twice rule
* Fix an issue where fake item activation did not remove the item if equipped
* Fix traps generating on chokepoints if near a shop
* Fix items spawning on stairs if in monster house
* Fix Leech Seed effect near a wrapped map border
* Dev: Linux: Fixed an issue where error windows repeatedly appeared
* Dex: Fix errors occurring when switching edit maps while entities are selected
* Dev: Fixed running game with the -convert flag
* Dev: Added a new editor for dungeon enemies
* Dev: SpecificTeamSpawner no longer ignores Mob Check Save Var clauses
* Dev: CTRL+F12 Soft Reset breaks through cutscenes now