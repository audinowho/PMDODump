# 0.7.18 Changes #

* Reworked Secret Garden's encounters to result in fewer level spikes, added Comfey
* Reworked Faded Trail encounters, added Petilil
* Allies transitively touching the leader and who cant move any closer will attack in Stick Together
* Wild Pokemon warped in via Summon Trap, Beat Up, and Illuminate will not act immediately after summoning.
* Grudge Trap breaks after trigger
* Improved item sort order
* Fixed an issue where AI didn't use Pursuit properly
* Fixed an issue where retreater AI was smart enough to use items
* Fixed an issue where One-Shot orb cannot miss, instantly KOing everything in range
* Effects that change moves such as Mimic and Transform will always set the temporary move's PP to 5
* Binding Band works like King's Rock for Immobilization
* Telekinesis affected by Grip Claw
* Nullify Orb affects the entire floor
* AI is more conscious of freeze status
* "Quad Weakness" damage multiplier 2x -> 2.25x
* Perish Count drops if-and-only-if hearing Perish Song
* Stone Edge Acc 80->100
* Fury Cutter BP 10->20
* Skull Bash BP 100->110
* Facade BP 60->65
* Heal Order PP 10->12
* Quick Guard PP 15->13
* Wide Guard PP 16->15
* Flail no longer hits allies
* Aromatherapy heals only allies
* Inferno uses a new Cross range, PP 14->10, BP 100->90
* Future Sight BP 90->80, fixed an issue that prevented the AoE from working
* Outrage PP 14->10, BP 90->85, Duration 3->2, No longer hits front and sides, explodes out from the center.
* Petal Dance PP 12->10, BP 80->90, Duration 3->2, Range +1
* Thrash PP 14->12, BP 90->100, Duration 3->2
* Moonlight PP 11->10, default healing is 1/3 instead of 1/4
* Morning Sun PP 10->11, default healing is 1/4 instead of 1/3
* Added VFX for various moves
* Altered the EXP gain curve to contain a linear increase and harmonic decrease
* Dev: Item Finder AI now prioritizes looking for items when exploring