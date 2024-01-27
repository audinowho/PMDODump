# 0.7.24 Changes #

* New EXP balancing for all dungeons
* Gummis prefer to boost one stat, and raise more stats the more effective the gummi type is to the pokemon type.
* Fixed issue where AI controlled characters used negative status moves or items on each other
* Magic Coat deflects all status moves, as does mirror orb
* Fixed issue with Harvest causing multi-activation with Wide Lens
* Psych Up fixed to be Normal Type
* Infestation fixed to be Special Category
* Fixed Amber Tear stacks of 0 in certain dungeons
* Fixed vision issues at seams fo wraparound maps
* Snatch Duration 3->10 turns
* Removed the extra gracidea from Secret Garden
* RC: Mysterious distortions give out exclusive items based on the team that entered them
* RC: Operational Juice Shop
* RC: Enemies no longer stabilize the mystery distortion stairs
* DEV: Fixed an issue where multi-choice menus did not obey text alignment params
* DEV: A hacky-supported mass-validate option is now in the Replays menu: Hold CTRL and press Minimap.
* DEV: You can specify Minimum game required version for mods
* BREAKING: COMMON.ShowDestinationMenu has been refactored.  You can call it the same way, but if you have your own version please update it. The old one will fail to call UI:DestinationMenu due to input and output change