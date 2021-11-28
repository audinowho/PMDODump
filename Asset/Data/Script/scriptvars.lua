--[[
    scriptvars.lua
      This file contains all the default values for the script variables. AKA on a new game this file is loaded!
      Script variables are stored in a table  that gets saved when the game is saved.
      Its meant to be used for scripters to add data to be saved and loaded during a playthrough.
      
      You can simply refer to the "SV" global table like any other table in any scripts!
      You don't need to write a default value in this lua script to add a new value.
      However its good practice to set a default value when you can!
      
    --Examples:
    SV.SomeVariable = "Smiles go for miles!"
    SV.AnotherVariable = 2526
    SV.AnotherVariable = { something={somethingelse={} } }
    SV.AnotherVariable = function() PrintInfo('lmao') end
]]--

-----------------------------------------------
-- Services Defaults
-----------------------------------------------
SV.Services =
{
  --Anything that applies to services should be put in here, or assigned to this or a subtable of this in the service's definition script
}

-----------------------------------------------
-- General Defaults
-----------------------------------------------
SV.General =
{
  Rescue = nil
  --Anything that applies to more than a single level, and that is too small to make a sub-table for, should be put in here ideally, or a sub-table of this
}

SV.checkpoint = 
{
  Zone    = 1, Segment  = -1,
  Map  = 1, Entry  = 0
}

-----------------------------------------------
-- Level Specific Defaults
-----------------------------------------------
SV.test_grounds =
{
  SpokeToPooch = false,
  AcceptedPooch = false,
  Missions = { },
  CurrentOutlaws = { },
  FinishedMissions = { },
  Starter = { Species=25, Form=0, Skin=0, Gender=2 },
  Partner = { Species=133, Form=0, Skin=0, Gender=1 },
}


SV.base_camp = 
{
  IntroComplete    = false,
  ExpositionComplete  = false,
  FirstTalkComplete  = false
}

SV.base_shop = {
	{ Index = 1, Hidden = 0, Price = 50},
	{ Index = 2, Hidden = 0, Price = 150},
	{ Index = 6, Hidden = 0, Price = 500},
	{ Index = 9, Hidden = 0, Price = 80},
	{ Index = 11, Hidden = 0, Price = 80}
}
SV.base_trades = {
	{ Item=902, ReqItem={-1,-1}},
	{ Item=908, ReqItem={-1,-1}},
	{ Item=914, ReqItem={-1,-1}}
}

SV.base_town = 
{
  Song    = "A02. Base Town.ogg"
}

SV.adventure = 
{
  Thief    = false
}

SV.luminous_spring = 
{
  Returning    = false
}

SV.forest_camp = 
{
  ExpositionComplete  = false
}

SV.cliff_camp = 
{
  ExpositionComplete  = false,
  TeamRetreatIntro = false
}

SV.canyon_camp = 
{
  ExpositionComplete  = false
}

SV.rest_stop = 
{
  ExpositionComplete  = false
}

SV.final_stop = 
{
  ExpositionComplete  = false
}

SV.guildmaster_summit = 
{
  ExpositionComplete  = false,
  BattleComplete = false
}

SV.garden_end = 
{
  ExpositionComplete  = false
}

----------------------------------------------