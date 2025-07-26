# 0.8.10 Changes #

* Reworked the turn order system to make speed boosts feel more fair https://wiki.pmdo.pmdcollab.org/User:IDK/Speed_Rework
* Fixed AI quirk that caused allies to spam attacks that can be absorbed by an ally's ability
* Freeze: 10->5 turns
* CounterBasher exclusive items reflect full damage.
* Fixed damage underflow in circumstances with low base power and high multiplier.
* Various renewable recovery effects don't work when starving
* Fixed an issue that caused permadeath in storymode
* Fixed a hub map AI race condition
* Emergency Exit and Wimp Out resolve at the start of the ability holder's turn.
* Minor menu behavior fixes
* Using a Monster Orb on a map that has no item spawns will no longer give an error
* Mission Board Mod: Fixed issue where taking two or more escort missions would not have all escorts join the party
* RC: Added a consistency guarantee for spawning a correct apricorn for Sleeping Caldera's guardian
* Dev: Removed common_talk.lua, which means COMMON.PERSONALITY is removed.  Possible personalities are automatically calculated; check the update to COMMON.DungeonInteract
* Scriptable Extra Spawn features and conditions
* Dev: Fixed PreventAction event's choosing of actions in editor
* Dev: Map position in ground/map editor
* Dev: Intrinsic3 chance spawn feature
* Dev: It's possible to add map BGs to dungeons
* Dev: You can now add custom upgrade code to your mods that catches upgrade cases from specific versions.
* Dev: Added UI:SetCustomDialogue() for setting custom-made dialogue boxes (Extremely avanced)
* Fixed an off-by-one issue with level range display in spawn editor