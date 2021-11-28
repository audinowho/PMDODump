--[[
  include.lua
  
  This file is loaded persistently.
  Its main purpose is to include dot net namespaces.
]]--


FNA = import 'FNA'
Microsoft = luanet.namespace('Microsoft')
Microsoft.Xna = luanet.namespace('Microsoft.Xna')
Microsoft.Xna.Framework = luanet.namespace('Microsoft.Xna.Framework')
RogueElements = import 'RogueElements'
RogueEssence = import 'RogueEssence'
RogueEssence.Content = luanet.namespace('RogueEssence.Content')
RogueEssence.Data = luanet.namespace('RogueEssence.Data')
RogueEssence.Dungeon = luanet.namespace('RogueEssence.Dungeon')
RogueEssence.Ground = luanet.namespace('RogueEssence.Ground')
RogueEssence.Script = luanet.namespace('RogueEssence.Script')
RogueEssence.Menu = luanet.namespace('RogueEssence.Menu')
RogueEssence.LevelGen = luanet.namespace('RogueEssence.LevelGen')
RogueEssence.Resources = luanet.namespace('RogueEssence.Resources')
RogueEssence.Network = luanet.namespace('RogueEssence.Network')
PMDC = import 'PMDC'
PMDC.Data = luanet.namespace('PMDC.Data')
PMDC.Dungeon = luanet.namespace('PMDC.Dungeon')
PMDC.LevelGen = luanet.namespace('PMDC.LevelGen')

FrameType   = luanet.import_type('RogueEssence.Content.FrameType')
DrawLayer   = luanet.import_type('RogueEssence.Content.DrawLayer')
MonsterID       = luanet.import_type('RogueEssence.Dungeon.MonsterID')
Gender      = luanet.import_type('RogueEssence.Data.Gender')
Direction   = luanet.import_type('RogueElements.Dir8')
GameTime    = luanet.import_type('Microsoft.Xna.Framework.GameTime')
Color       = luanet.import_type('Microsoft.Xna.Framework.Color')
ActivityType = luanet.import_type('RogueEssence.Network.ActivityType')
TimeSpan    = luanet.import_type('System.TimeSpan')