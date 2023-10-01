# 0.7.18 Changes #

* Reworked Secret Garden's encounters to result in fewer level spikes, added Comfey
* Wild Pokemon warped in via Summon Trap, Beat Up, and Illuminate will not act immediately after summoning.
* Allies transitively touching the leader and who cant move any closer will attack in Stick Together
* Fixed an issue where AI didn't use Pursuit properly
* Fixed an issue where retreater AI was smart enough to use items
* Effects that change moves such as Mimic and Transform will always set the temporary move's PP to 5
* Binding Band works like King's Rock for Immobilization
* Telekinesis affected by Grip Claw
* "Quad Weakness" damage multiplier 2x -> 2.25x
* Inferno uses a new Cross range, PP 14->10, BP 100->90
* Perish Count drops if-and-only-if hearing Perish Song
* Flail no longer hits allies
* Stone Edge Acc 80->100
* Fury Cutter BP 10->20
* Aromatherapy heals only allies
* Heal Order PP 10->12
* Moonlight PP 11->10, default healing is 1/3 instead of 1/4
* Morning Sun PP 10->11, default healing is 1/4 instead of 1/3
* Quick Guard PP 15->13
* Wide Guard PP 16->15
* Grudge Trap breaks after trigger
* Added VFX for various moves
* Altered the EXP gain curve to contain a linear increase and harmonic decrease
* Dev: Item Finder AI now prioritizes looking for items when exploring