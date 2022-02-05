# 0.5.7 Changes #

* NOTE FOR MODDERS: It is ideal to reserialize on this version due to index changes!
* Fixed errors occurring with ground/map editors on the Linux build.
* Fixed deadlocks occurring when importing textures on the Linux build.
* Fixed unexpected map init call when reloading scripts in editor mode.
* Mods are now separated into two types: Mods and Quests. Only one Quest cna be active at a time, and they have their own save file. Mods do not use a separate save file and multiple can be active at a time.
* Mods now have a Mod.xml that indicates name, ID, version, and if it's a Quest.  Quests will use new save files while Mods add to the current save file. This xml can be regenerated in the Mod tab if you currently own a mod with it missing.
* Allow DTEF import into Autotiles.
* For mods: files with extension of .idx will be applied as a patch to the base index, instead of replacing it. This means that .idx files will no longer be used for data that doesn't change. 
* Enabled quest/mods are remembered between play sessions.
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