# 0.5.7 Changes #

* Fixed errors occurring with ground/map editors on the Linux build.
* Fixed deadlocks occurring when importing textures on the Linux build.
* Fixed unexpected map init call when reloading scripts in editor mode.
* Mods now have a Mod.xml that indicates name, ID, version, and if it's a Quest.  Quests will use new save files while Mods add to the current save file.  This xml can be regenerated for currently existing mods in the Mod tab.
* Enabled mods are remembered between play sessions.
* Fixed issues with calling scripts that add/remove party members in dungeons.
* Added GAME:QueueLeaderEvent for scripting.
* Made strings fall back to base if they do not exist in mods.
* Fixed an issue where enemies catching thrown held items would mark them for EXP, giving an infinite source of EXP-tagging.
* Changed the move Telekinesis.
* Changed the ability Run Away.
* Change the item Shed Shell.
* Allies will only attack Tit for Tat AI on turns where the player attacks.
* Fixed an issue that caused Dratini to spawn in unintended areas.
* Added support for 8bitdo controllers.
* Minor fixes in rescue info text.