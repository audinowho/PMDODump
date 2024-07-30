# 0.8.3 Changes #

* Dev: Modded data (such as Items, Monsters, Universal, etc) can now be saved as a patch instead of overwriting the entire file.  This allows your mod to play nice with other mods.  Use Right Click.
* Dev: Building standalone games now works with the lua diff mod system.
* Dev: Services can now use the OnAddMenu event to intercept and change menus.
* Dev: Menus and menu elements now have labels, allowing modders to accurately change specific parts of specific menus they want.
* Dev: ScriptableMenu.MenuElements is now DEPRECATED. Please use Elements instead.  Support will be dropped on v0.9