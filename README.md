Important notes

---

DataGenerator Deployment Order
* One-time: Run `-itemprep` to generate monster/status/element tables needed for items.
* Run `Scripts/item_sync.py` to update exclusive item spreadsheet with data generated above. It will generate a csv of exclusive items to be used in the `-dump` step.

* Reserialize Skills and Monster (Or regenerate Monster) using `-reserialize Skill` or `-reserialize Monster`
* Dump all data using `-dump`.  It depends on the csv of exclusive items to generate that exclusive items (item creation). It also generates an XML to map species to family items (spawning lookup), and a common_gen.lua containing tables of generic trades and specific trades.

* Generate tables for string merge with `-strings out`.
* Sync the translation table using `Scripts/strings_sync.py`
* Uptake tables for string merge with `-strings in`.