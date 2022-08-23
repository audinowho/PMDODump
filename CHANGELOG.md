# 0.6.2 Changes #

* Fixed an issue with the updater that left old data from previous installations
* Add namespaces to mods.  Ex: "origin:bulbasaur"
* Add dependency/load order to mods
* Developers will need to reserialize on this version.  Additionally, all script calls to _DATA.DataIndices[<datatype>].Entries[<entry name>] must be replaced with _DATA.DataIndices[<datatype>]:Get(<entry name>)
* Modified the moves Fake Out and Punishment