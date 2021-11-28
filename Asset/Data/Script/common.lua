--[[
    common.lua
    A collection of frequently used functions and values!
]]--
require 'common_gen'

----------------------------------------
-- Debugging
----------------------------------------
DEBUG = 
{
  EnableDbgCoro = function() end, --Call this function inside coroutines you want to allow debugging of, at the start. Default is empty
  --FIXME: fix mobdebug and sockets
  IsDevMode = function() return false end, --RogueEssence.DiagManager.Instance.DevMode end,
  GroundAIShowDebugInfo = false,
}

DEBUG.GroundAIShowDebugInfo = false--DEBUG.IsDevMode()

--Disable debugging for non devs
if DEBUG.IsDevMode() then
  ___mobdebug = require('mobdebug')
  ___mobdebug.coro() --Enable coroutine debugging
  ___mobdebug.checkcount = 1 --Increase debugger update frequency
  ___mobdebug.verbose=true --Enable debugger verbose mode
  ___mobdebug.start() --Enable debugging
  DEBUG.EnableDbgCoro = function() require('mobdebug').on() end --Set the content of the function to this in dev mode, so it does something
end


----------------------------------------
-- Lib Definitions
----------------------------------------
--Reserve the "class" symbol for instantiating classes
Class    = require 'lib.middleclass'
--Reserve the "Mediator" symbol for instantiating the message lib class
Mediator = require 'lib.mediator' 
--Reserve the "Serpent" symbol for the serializer
Serpent = require 'lib.serpent'

----------------------------------------------------------
-- Console Writing
----------------------------------------------------------

--Prints to console!
function PrintInfo(text)
  if DiagManager then 
    DiagManager.Instance:LogInfo( '[SE]:' .. text) 
  else
    print('[SE]:' .. text)
  end
end

--Prints to console!
function PrintError(text)
  if DiagManager then 
    DiagManager.Instance:LogInfo( '[SE]:' .. text) 
  else
    print('[SE](ERROR): ' .. text)
  end
end

--Will print the stack and the specified error message
function PrintStack(err)
  PrintInfo(debug.traceback(tostring(err),2)) 
end

function PrintSVAndStrings(mapstr)
  print("DUMPING SCRIPT VARIABLE STATE..")
  print(Serpent.block(SV))
  print(Serpent.block(mapstr))
  print("-------------------------------")
end

----------------------------------------
-- Common namespace
----------------------------------------
COMMON = {}

--Automatically load the appropriate localization for the specified package, or defaults to english!
function COMMON.AutoLoadLocalizedStrings()
  PrintInfo("AutoLoading Strings!..")
  --Get the package path
  local packagepath = SCRIPT:CurrentScriptDir()
  
  --After we made the path, load the file
  return STRINGS:MakePackageStringTable(packagepath)
end

COMMON.MISSION_TYPE_RESCUE = 0
COMMON.MISSION_TYPE_ESCORT = 1
COMMON.MISSION_TYPE_OUTLAW = 2

COMMON.MISSION_INCOMPLETE = 0
COMMON.MISSION_COMPLETE = 1

COMMON.PERSONALITY = { }
COMMON.PERSONALITY[0] = { -- partner
  FULL = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11},
  HALF = {0, 1, 2},
  PINCH = {0, 1, 2},
  WAIT = {0, 1, 2, 3}
}
COMMON.PERSONALITY[1] = { -- confident
  FULL = {20, 21, 22, 23, 24, 25, 26},
  HALF = {5, 6, 7},
  PINCH = {5, 6, 7},
  WAIT = {5, 6, 7}
}
COMMON.PERSONALITY[2] = { -- nervous
  FULL = {40, 41, 42, 43, 44, 45, 46, 47, 48, 49},
  HALF = {10, 11, 12, 13},
  PINCH = {10, 11, 12},
  WAIT = {10, 11, 12}
}
COMMON.PERSONALITY[3] = { -- cautious
  FULL = {60, 61, 62, 63, 64, 65, 66},
  HALF = {15, 16, 17},
  PINCH = {15, 16, 17},
  WAIT = {15, 16, 17}
}
COMMON.PERSONALITY[4] = { -- musing
  FULL = {80, 81, 82, 83, 84, 85, 86},
  HALF = {20, 21, 22, 23},
  PINCH = {20, 21, 22},
  WAIT = {20, 21, 22}
}
COMMON.PERSONALITY[5] = { -- legend
  FULL = {100, 101, 102, 103, 104},
  HALF = {25, 26, 27},
  PINCH = {25, 26, 27},
  WAIT = {25, 26}
}
COMMON.PERSONALITY[6] = { -- robot
  FULL = {120, 121, 122, 123, 124, 125, 126},
  HALF = {30, 31, 32},
  PINCH = {30, 31, 32},
  WAIT = {30, 31, 32}
}
COMMON.PERSONALITY[7] = { -- jock
  FULL = {140, 141, 142, 143, 144, 145, 146, 147},
  HALF = {35, 36, 37},
  PINCH = {35, 36, 37},
  WAIT = {35, 36, 37}
}
COMMON.PERSONALITY[8] = { -- child
  FULL = {160, 161, 162, 163, 142, 144},
  HALF = {40, 41, 42},
  PINCH = {40, 41, 42},
  WAIT = {40, 41, 42}
}
COMMON.PERSONALITY[9] = { -- selfless
  FULL = {180, 181, 182, 183, 184, 2, 6, 10},
  HALF = {45, 46, 47},
  PINCH = {45, 46, 47},
  WAIT = {45, 46, 47, 0, 3}
}
COMMON.PERSONALITY[10] = { -- reckless
  FULL = {200, 201, 202, 203, 204, 205, 206},
  HALF = {50, 51, 52},
  PINCH = {50, 51, 52},
  WAIT = {50, 51, 52}
}
COMMON.PERSONALITY[11] = { -- lazy
  FULL = {220, 221, 222, 223, 224, 225, 226, 227, 228},
  HALF = {55, 56, 57},
  PINCH = {55, 56, 57},
  WAIT = {55, 56, 57, 58}
}
COMMON.PERSONALITY[12] = { -- loud
  FULL = {240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252},
  HALF = {60, 61, 62},
  PINCH = {60, 61, 62},
  WAIT = {60, 61, 62, 63}
}
COMMON.PERSONALITY[13] = { -- snooty
  FULL = {260, 261, 262, 263, 264, 265, 266, 267, 268},
  HALF = {65, 66, 67},
  PINCH = {65, 66, 67},
  WAIT = {65, 66, 67}
}
COMMON.PERSONALITY[14] = { -- hyped
  FULL = {280, 281, 282, 283, 284, 285, 261, 266},
  HALF = {70, 71, 72},
  PINCH = {70, 71, 72},
  WAIT = {70, 71, 72}
}
COMMON.PERSONALITY[15] = { -- reserved
  FULL = {300, 301, 302, 303, 304, 305, 22},
  HALF = {75, 76, 77},
  PINCH = {75, 76, 77},
  WAIT = {75, 76, 77}
}
COMMON.PERSONALITY[16] = { -- greedy
  FULL = {320, 321, 322, 323, 324, 325, 326, 327},
  HALF = {80, 81, 82},
  PINCH = {80, 81, 82},
  WAIT = {80, 81, 82}
}
COMMON.PERSONALITY[24] = { -- baby / manaphy
  FULL = {640, 641, 642, 643, 644, 645, 646, 647, 648},
  HALF = {135, 136, 137, 138, 139},
  PINCH = {135, 136, 137, 138, 139},
  WAIT = {135, 136, 137, 138, 139}
}
COMMON.PERSONALITY[26] = { -- formal
  FULL = {680, 681, 682, 683, 684, 685, 686, 687},
  HALF = {145, 146, 147, 148, 149},
  PINCH = {145, 146, 147, 148, 149},
  WAIT = {145, 146, 147, 148, 149}
}
COMMON.PERSONALITY[29] = { -- knight
  FULL = {740, 741, 742, 743, 744, 745},
  HALF = {160, 161, 162, 163},
  PINCH = {160, 161, 162, 163},
  WAIT = {150, 151, 152, 153}
}
COMMON.PERSONALITY[30] = { -- teenager
  FULL = {750, 751, 752, 753, 754, 755},
  HALF = {165, 166, 167, 168},
  PINCH = {165, 166, 167, 168},
  WAIT = {155, 156, 157, 158}
}
COMMON.PERSONALITY[31] = { -- normal
  FULL = {760, 761, 762, 763, 764, 765},
  HALF = {170, 171, 172, 173},
  PINCH = {170, 171, 172, 173},
  WAIT = {160, 161, 162, 163}
}
COMMON.PERSONALITY[32] = { -- dialga
  FULL = {770, 771, 772, 773, 774, 775},
  HALF = {175, 176, 177, 178},
  PINCH = {175, 176, 177, 178},
  WAIT = {165, 166, 167, 168}
}
COMMON.PERSONALITY[33] = { -- palkia
  FULL = {780, 781, 782, 783, 784, 785},
  HALF = {180, 181, 182, 183},
  PINCH = {180, 181, 182, 183},
  WAIT = {170, 171, 172, 173}
}
COMMON.PERSONALITY[34] = { -- regigigas
  FULL = {790, 791, 792, 793, 794, 795},
  HALF = {185, 186, 187, 188},
  PINCH = {185, 186, 187, 188},
  WAIT = {175, 176, 177, 178}
}
COMMON.PERSONALITY[35] = { -- darkrai
  FULL = {800, 801, 802, 803, 804, 805},
  HALF = {190, 191, 192, 193},
  PINCH = {190, 191, 192, 193},
  WAIT = {180, 181, 182, 183}
}
COMMON.PERSONALITY[36] = { -- shopkeeper
  FULL = {810, 811, 812, 813, 814, 815},
  HALF = {195, 196, 197, 198},
  PINCH = {195, 196, 197, 198},
  WAIT = {185, 186, 187, 188}
}
COMMON.PERSONALITY[37] = { -- escort
  FULL = {820},
  HALF = {200},
  PINCH = {200},
  WAIT = {190}
}

COMMON.ESSENTIALS = {
  { Index = 1, Hidden = 0, Price = 50},
  { Index = 2, Hidden = 0, Price = 150},
  { Index = 6, Hidden = 0, Price = 500},
  { Index = 10, Hidden = 0, Price = 80},
  { Index = 11, Hidden = 0, Price = 80},
  { Index = 12, Hidden = 0, Price = 120},
  { Index = 101, Hidden = 0, Price = 500},
  { Index = 210, Hidden = 0, Price = 400}
}
  
COMMON.UTILITIES = {
  { Index = 37, Hidden = 0, Price = 100},
  { Index = 38, Hidden = 0, Price = 100},
  { Index = 108, Hidden = 0, Price = 80},
  { Index = 110, Hidden = 0, Price = 80},
  { Index = 112, Hidden = 0, Price = 200},
  { Index = 113, Hidden = 0, Price = 80},
  { Index = 114, Hidden = 0, Price = 350},
  { Index = 115, Hidden = 0, Price = 80},
  { Index = 116, Hidden = 0, Price = 80},
  { Index = 117, Hidden = 0, Price = 150},
  { Index = 118, Hidden = 0, Price = 150},
  { Index = 183, Hidden = 0, Price = 120},
  { Index = 184, Hidden = 0, Price = 250},
  { Index = 185, Hidden = 0, Price = 80}
}
  
COMMON.AMMO = {
  { Index = 200, Hidden = 9, Price = 45},
  { Index = 201, Hidden = 9, Price = 90},
  { Index = 202, Hidden = 9, Price = 90},
  { Index = 203, Hidden = 9, Price = 90},
  { Index = 204, Hidden = 9, Price = 360},
  { Index = 207, Hidden = 9, Price = 45},
  { Index = 208, Hidden = 9, Price = 90},
  { Index = 220, Hidden = 9, Price = 180},
  { Index = 221, Hidden = 9, Price = 180},
  { Index = 222, Hidden = 9, Price = 180},
  { Index = 223, Hidden = 9, Price = 180},
  { Index = 225, Hidden = 9, Price = 180},
  { Index = 226, Hidden = 9, Price = 180},
  { Index = 228, Hidden = 9, Price = 180},
  { Index = 231, Hidden = 9, Price = 180},
  { Index = 232, Hidden = 9, Price = 180},
  { Index = 233, Hidden = 9, Price = 180},
  { Index = 234, Hidden = 9, Price = 180}
}
COMMON.ORBS = {
  { Index = 251, Hidden = 0, Price = 150},
  { Index = 252, Hidden = 0, Price = 250},
  { Index = 253, Hidden = 0, Price = 350},
  { Index = 254, Hidden = 0, Price = 200},
  { Index = 256, Hidden = 0, Price = 250},
  { Index = 257, Hidden = 0, Price = 150},
  { Index = 258, Hidden = 0, Price = 150},
  { Index = 259, Hidden = 0, Price = 350},
  { Index = 263, Hidden = 0, Price = 150},
  { Index = 264, Hidden = 0, Price = 300},
  { Index = 265, Hidden = 0, Price = 150},
  { Index = 266, Hidden = 0, Price = 150},
  { Index = 267, Hidden = 0, Price = 250},
  { Index = 268, Hidden = 0, Price = 250},
  { Index = 269, Hidden = 0, Price = 200},
  { Index = 270, Hidden = 0, Price = 200},
  { Index = 271, Hidden = 0, Price = 250},
  { Index = 272, Hidden = 0, Price = 250},
  { Index = 273, Hidden = 0, Price = 250},
  { Index = 274, Hidden = 0, Price = 250},
  { Index = 275, Hidden = 0, Price = 250},
  { Index = 276, Hidden = 0, Price = 250},
  { Index = 277, Hidden = 0, Price = 250},
  { Index = 278, Hidden = 0, Price = 150},
  { Index = 281, Hidden = 0, Price = 350},
  { Index = 282, Hidden = 0, Price = 250},
  { Index = 283, Hidden = 0, Price = 150},
  { Index = 284, Hidden = 0, Price = 150},
  { Index = 286, Hidden = 0, Price = 250},
  { Index = 287, Hidden = 0, Price = 250},
  { Index = 288, Hidden = 0, Price = 150},
  { Index = 289, Hidden = 0, Price = 250}
}
  
COMMON.SPECIAL = {
  { Index = 455, Hidden = 3, Price = 4000},
  { Index = 260, Hidden = 0, Price = 1000},
  { Index = 250, Hidden = 0, Price = 150},
  { Index = 250, Hidden = 0, Price = 150}
}
  

----------------------------------------------------------
-- Convenience Scription Functions
----------------------------------------------------------
function COMMON.RespawnStarterPartner()
  -- SV.test_grounds.Starter.Gender = LUA_ENGINE:EnumToNumeric(Gender.Female)
  local character = RogueEssence.Dungeon.CharData()
  character.BaseForm = RogueEssence.Dungeon.MonsterID(SV.test_grounds.Starter.Species, SV.test_grounds.Starter.Form, SV.test_grounds.Starter.Skin, LUA_ENGINE:LuaCast(SV.test_grounds.Starter.Gender, Gender))
  GROUND:SetPlayer(character)
  GROUND:RemoveCharacter("Partner")
  local p = RogueEssence.Dungeon.CharData()
  p.BaseForm = RogueEssence.Dungeon.MonsterID(SV.test_grounds.Partner.Species, SV.test_grounds.Partner.Form, SV.test_grounds.Partner.Skin, LUA_ENGINE:LuaCast(SV.test_grounds.Partner.Gender, Gender))
  GROUND:SpawnerSetSpawn("PARTNER_SPAWN", p)
  local chara = GROUND:SpawnerDoSpawn("PARTNER_SPAWN")
end

function COMMON.RespawnAllies()
  GROUND:RefreshPlayer()
  

  local party = GAME:GetPlayerPartyTable()
  local playeridx = GAME:GetTeamLeaderIndex()

  --Place player teammates
  for i = 1,3,1
  do
    GROUND:RemoveCharacter("Teammate" .. tostring(i))
  end
  local total = 1
  for i,p in ipairs(party) do
    if i ~= (playeridx + 1) then --Indices in lua tables begin at 1
      GROUND:SpawnerSetSpawn("TEAMMATE_" .. tostring(total),p)
      local chara = GROUND:SpawnerDoSpawn("TEAMMATE_" .. tostring(total))
      --GROUND:GiveCharIdleChatter(chara)
      total = total + 1
    end
  end
end

function COMMON.ShowTeamAssemblyMenu(obj, init_fun)
  SOUND:PlaySE("Menu/Skip")
  UI:AssemblyMenu()
  UI:WaitForChoice()
  result = UI:ChoiceResult()
  if result then
    GAME:WaitFrames(10)
	SOUND:PlayBattleSE("EVT_Assembly_Bell")
	GROUND:ObjectSetAnim(obj, 6, -1, -1, RogueElements.Dir8.Down, 3)
	GAME:FadeOut(false, 20)
	init_fun()
    GAME:FadeIn(20)
  end
end
function COMMON.ShowDestinationMenu(dungeon_entrances,ground_entrances)
  
  --check for unlock of dungeons
  local open_dungeons = {}
  for i = 1,#dungeon_entrances,1 do
    if GAME:DungeonUnlocked(dungeon_entrances[i]) then
      table.insert(open_dungeons, dungeon_entrances[i])
	end
  end
  
  --check for unlock of grounds
  local open_grounds = {}
  for i = 1,#ground_entrances,1 do
    if ground_entrances[i].Flag then
      table.insert(open_grounds, ground_entrances[i])
	end
  end
  
  local dest = RogueEssence.Dungeon.ZoneLoc.Invalid
  if #open_dungeons + #open_grounds == 1 then
    if #open_dungeons == 1 then
      
      UI:ResetSpeaker()
      SOUND:PlaySE("Menu/Skip")
	  UI:DungeonChoice(open_dungeons[1])
      UI:WaitForChoice()
      if UI:ChoiceResult() then
	    dest = RogueEssence.Dungeon.ZoneLoc(open_dungeons[1], RogueEssence.Dungeon.SegLoc(0,0))
	  end
	elseif #open_grounds == 1 then
	  local ground_id = open_grounds[1].Zone
	  local zone = RogueEssence.Data.DataManager.Instance:GetZone(ground_id)
	  local ground = RogueEssence.Data.DataManager.Instance:GetGround(zone.GroundMaps[open_grounds[1].ID])
	  local ground_name = ground:GetColoredName()
      
	  UI:ChoiceMenuYesNo(STRINGS:FormatKey("DLG_ASK_ENTER_GROUND", ground_name))
      UI:WaitForChoice()
      if UI:ChoiceResult() then
	    dest = RogueEssence.Dungeon.ZoneLoc(open_grounds[1].Zone, RogueEssence.Dungeon.SegLoc(-1, open_grounds[1].ID), open_grounds[1].Entry)
	  end
	end
  elseif #open_dungeons + #open_grounds > 1 then
    UI:ResetSpeaker()
    SOUND:PlaySE("Menu/Skip")
    UI:DungeonMenu(open_dungeons, open_grounds)
	UI:WaitForChoice()
	dest = UI:ChoiceResult()
  end
  
  if dest:IsValid() then
    SOUND:PlayBGM("", true)
    GAME:FadeOut(false, 20)
	if dest.StructID.Segment > -1 then
	  GAME:EnterDungeon(dest.ID, dest.StructID.Segment, dest.StructID.ID, dest.EntryPoint, RogueEssence.Data.GameProgress.DungeonStakes.Risk, true, false)
	else
	  GAME:EnterZone(dest.ID, dest.StructID.Segment, dest.StructID.ID, dest.EntryPoint)
	end
  end
end


function COMMON.UnlockWithFanfare(dungeon_id)
  if not GAME:DungeonUnlocked(dungeon_id) then
    UI:WaitShowDialogue(STRINGS:FormatKey("DLG_NEW_AREA_TO"))
    GAME:UnlockDungeon(dungeon_id)
	local zone = RogueEssence.Data.DataManager.Instance.DataIndices[RogueEssence.Data.DataManager.DataType.Zone].Entries[dungeon_id]
    SOUND:PlayFanfare("Fanfare/NewArea")
    UI:WaitShowDialogue(STRINGS:FormatKey("DLG_NEW_AREA", zone:GetColoredName()))
  end

end

function COMMON.ClearPlayerPrices()
  local item_count = GAME:GetPlayerBagCount()
  for item_idx = 0, item_count-1, 1 do
    local inv_item = GAME:GetPlayerBagItem(item_idx)
	inv_item.Price = 0
  end
  local player_count = _DUNGEON.ActiveTeam.Players.Count
  for player_idx = 0, player_count-1, 1 do
    local inv_item = GAME:GetPlayerEquippedItem(player_idx)
	inv_item.Price = 0
  end
end

function COMMON.ClearAllPrices()
  COMMON.ClearPlayerPrices()

  -- clear map prices
  local item_count = _ZONE.CurrentMap.Items.Count
  for item_idx = 0, item_count-1, 1 do
    local map_item = _ZONE.CurrentMap.Items[item_idx]
	map_item.Price = 0
  end
end

function COMMON.PayDungeonCartPrice(price)
  COMMON.ClearPlayerPrices()
  GAME:RemoveFromPlayerMoney(price)
  local security_status = 38
  if _ZONE.CurrentMap.Status:ContainsKey(security_status) then
    local status = _ZONE.CurrentMap.Status[security_status]
	if status.StatusStates:Contains(luanet.ctype(ShopPriceType)) then
	  local security_price = status.StatusStates:Get(luanet.ctype(ShopPriceType))
	  security_price.Amount = security_price.Amount - price
	end
  end
end

function COMMON.PayDungeonSellPrice(price)
  -- set prices for all items placed on the shop
  local item_count = _ZONE.CurrentMap.Items.Count
  for item_idx = 0, item_count-1, 1 do
    local map_item = _ZONE.CurrentMap.Items[item_idx]
	-- they should not already have a price
	if map_item.Price <= 0 then
	  local sell_value = map_item:GetSellValue()
	  -- they should have value as sold items
	  if sell_value > 0 then
	    local tile = _ZONE.CurrentMap.Tiles[map_item.TileLoc.X][map_item.TileLoc.Y]
	    -- only add price if on shop mat
	    if tile.Effect.ID == 45 then
	      map_item.Price = sell_value * 2
	    end
	  end
	end
  end
  
  GAME:AddToPlayerMoney(price)
  local security_status = 38
  if _ZONE.CurrentMap.Status:ContainsKey(security_status) then
    local status = _ZONE.CurrentMap.Status[security_status]
	if status.StatusStates:Contains(luanet.ctype(ShopPriceType)) then
	  local security_price = status.StatusStates:Get(luanet.ctype(ShopPriceType))
	  security_price.Amount = security_price.Amount + price * 2
	end
  end
end

ShopPriceType = luanet.import_type('PMDC.Dungeon.ShopPriceState')

function COMMON.GetDungeonCartPrice()

  local price = 0
  local security_status = 38
  if _ZONE.CurrentMap.Status:ContainsKey(security_status) then
    local status = _ZONE.CurrentMap.Status[security_status]
	if status.StatusStates:Contains(luanet.ctype(ShopPriceType)) then
	  local security_price = status.StatusStates:Get(luanet.ctype(ShopPriceType))
	  price = security_price.Amount
	end
  end
  
  -- iterate items on shop mats and subtract total price
  local item_count = _ZONE.CurrentMap.Items.Count
  for item_idx = 0, item_count-1, 1 do
    local map_item = _ZONE.CurrentMap.Items[item_idx]
	if map_item.Price > 0 then
	  local tile = _ZONE.CurrentMap.Tiles[map_item.TileLoc.X][map_item.TileLoc.Y]
	  -- only subtract price if on shop mat
	  if tile.Effect.ID == 45 then
	    price = price - map_item.Price
	  end
	end
  end
  return price
end

function COMMON.GetDungeonSellPrice()

  local price = 0
  -- iterate items on shop mats and add total price
  local item_count = _ZONE.CurrentMap.Items.Count
  for item_idx = 0, item_count-1, 1 do
    local map_item = _ZONE.CurrentMap.Items[item_idx]
	-- they should not already have a price
	if map_item.Price <= 0 then
	  local sell_value = map_item:GetSellValue()
	  -- they should have value as sold items
	  if sell_value > 0 then
	    local tile = _ZONE.CurrentMap.Tiles[map_item.TileLoc.X][map_item.TileLoc.Y]
	    -- only add price if on shop mat
	    if tile.Effect.ID == 45 then
	      price = price + sell_value
	    end
	  end
	end
  end
  return price
end


function COMMON.ThiefReturn()
  _GAME:BGM("", false)
  COMMON.ClearAllPrices()
  
  local thief_check_idx = 38
  local thief_idx = 31
  local check_status = _ZONE.CurrentMap.Status[thief_check_idx]
  
  local index_from = check_status.StatusStates:Get(luanet.ctype(MapIndexType))
  UI:WaitShowDialogue(RogueEssence.StringKey(string.format("TALK_SHOP_SUSPECT_%04d", index_from.Index)):ToLocal())
  _DUNGEON:LogMsg(RogueEssence.StringKey(string.format("TALK_SHOP_THIEF_RETURN_%04d", index_from.Index)):ToLocal())
  
  local thief_status = RogueEssence.Dungeon.MapStatus(thief_idx)
  thief_status:LoadFromData()
  -- put spawner from security status in thief status
  local security_to = thief_status.StatusStates:Get(luanet.ctype(ShopSecurityType))
  local security_from = check_status.StatusStates:Get(luanet.ctype(ShopSecurityType))
  security_to.Security = security_from.Security
  TASK:WaitTask(_DUNGEON:RemoveMapStatus(thief_check_idx))
  TASK:WaitTask(_DUNGEON:AddMapStatus(thief_status))
  GAME:WaitFrames(60)
end

function COMMON.ShopTileCheck(baseLoc, dir)
  local dirLoc = RogueElements.DirExt.GetLoc(dir)
  local loc = RogueElements.Loc(baseLoc.X + dirLoc.X, baseLoc.Y + dirLoc.Y)
  if not RogueElements.Collision.InBounds(_ZONE.CurrentMap.Width, _ZONE.CurrentMap.Height, loc) then
    return false
  end
  return (_ZONE.CurrentMap.Tiles[loc.X][loc.Y].Effect.ID == 45)
end



function COMMON.FindNpcWithTable(foes, key, value)
  local team_list = _ZONE.CurrentMap.AllyTeams
  if foes then
    team_list = _ZONE.CurrentMap.MapTeams
  end
  local team_count = team_list.Count
  for team_idx = 0, team_count-1, 1 do
	-- search for a wild mon with the table value
	local player_count = team_list[team_idx].Players.Count
	for player_idx = 0, player_count-1, 1 do
	  player = team_list[team_idx].Players[player_idx]
	  local player_tbl = LTBL(player)
	  if player_tbl[key] == value then
		return player
	  end
	end
  end
  return nil
end

function COMMON.CanTalk(chara)
  if chara:GetStatusEffect(1) ~= nil then
    return false
  end
  if chara:GetStatusEffect(3) ~= nil then
    return false
  end
  if chara:GetStatusEffect(7) ~= nil then
    return false
  end
  return true
end


function COMMON.DungeonInteract(chara, target, action_cancel, turn_cancel)
  action_cancel.Cancel = true
  -- TODO: create a charstate for being unable to talk and have talk-interfering statuses cause it
  if COMMON.CanTalk(target) then
    
    local ratio = target.HP * 100 // target.MaxHP
    
    local mon = RogueEssence.Data.DataManager.Instance:GetMonster(target.BaseForm.Species)
    local form = mon.Forms[target.BaseForm.Form]
    
    local personality = form:GetPersonalityType(target.Discriminator)
    
    local personality_group = COMMON.PERSONALITY[personality]
    local pool = {}
    local key = ""
    if ratio <= 25 then
      UI:SetSpeakerEmotion("Pain")
      pool = personality_group.PINCH
      key = "TALK_PINCH_%04d"
    elseif ratio <= 50 then
      UI:SetSpeakerEmotion("Worried")
      pool = personality_group.HALF
      key = "TALK_HALF_%04d"
    else
      pool = personality_group.FULL
      key = "TALK_FULL_%04d"
    end
    
    local running_pool = {table.unpack(pool)}
    local valid_quote = false
    local chosen_quote = ""
    
    while not valid_quote and #running_pool > 0 do
      valid_quote = true
      local chosen_idx = math.random(1, #running_pool)
  	  local chosen_pool_idx = running_pool[chosen_idx]
      chosen_quote = RogueEssence.StringKey(string.format(key, chosen_pool_idx)):ToLocal()
  	
      chosen_quote = string.gsub(chosen_quote, "%[player%]", chara:GetDisplayName(true))
      chosen_quote = string.gsub(chosen_quote, "%[myname%]", target:GetDisplayName(true))
      
      if string.find(chosen_quote, "%[move%]") then
        local moves = {}
  	    for move_idx = 0, 3 do
  	      if target.BaseSkills[move_idx].SkillNum > 0 then
  	        table.insert(moves, target.BaseSkills[move_idx].SkillNum)
  	      end
  	    end
  	    if #moves > 0 then
  	      local chosen_move = RogueEssence.Data.DataManager.Instance:GetSkill(moves[math.random(1, #moves)])
  	      chosen_quote = string.gsub(chosen_quote, "%[move%]", chosen_move:GetIconName())
  	    else
  	      valid_quote = false
  	    end
      end
      
      if string.find(chosen_quote, "%[kind%]") then
  	    if GAME:GetCurrentFloor().TeamSpawns.CanPick then
          local team_spawn = GAME:GetCurrentFloor().TeamSpawns:Pick(GAME.Rand)
  	      local chosen_list = team_spawn:ChooseSpawns(GAME.Rand)
  	      if chosen_list.Count > 0 then
  	        local chosen_mob = chosen_list[math.random(0, chosen_list.Count-1)]
  	        local mon = RogueEssence.Data.DataManager.Instance:GetMonster(chosen_mob.BaseForm.Species)
            chosen_quote = string.gsub(chosen_quote, "%[kind%]", mon:GetColoredName())
  	      else
  	        valid_quote = false
  	      end
  	    else
  	      valid_quote = false
  	    end
      end
      
      if string.find(chosen_quote, "%[item%]") then
        if GAME:GetCurrentFloor().ItemSpawns.CanPick then
          local item = GAME:GetCurrentFloor().ItemSpawns:Pick(GAME.Rand)
          chosen_quote = string.gsub(chosen_quote, "%[item%]", item:GetDisplayName())
  	    else
  	      valid_quote = false
  	    end
      end
  	
  	  if not valid_quote then
        -- PrintInfo("Rejected "..chosen_quote)
  	    table.remove(running_pool, chosen_idx)
  	    chosen_quote = ""
  	  end
    end
    -- PrintInfo("Selected "..chosen_quote)
	
	local oldDir = target.CharDir
    DUNGEON:CharTurnToChar(target, chara)
  
    UI:SetSpeaker(target)
  
    UI:WaitShowDialogue(chosen_quote)
  
    target.CharDir = oldDir
  else
  
    UI:ResetSpeaker()
	
	local chosen_quote = RogueEssence.StringKey("TALK_CANT"):ToLocal()
    chosen_quote = string.gsub(chosen_quote, "%[myname%]", target:GetDisplayName(true))
	
    UI:WaitShowDialogue(chosen_quote)
  
  end
  
  --characters can comment on:

  --flavour for the dungeon (talk about the area and its lore)
  --being able to evolve
  --nearly reaching the end of the dungeon
  --being hungry
  --out of PP
  --being able to recruit someone
  --tutorial tips
  ---gaining EXP while in assembly
  ---apricorns
  ---sleeping pokemon
  --commenting on how long they've been on the team
  ---I know we just met, but...
  ---We've been together for over 10 floors now, don't leave me now!
  --beginning a fight "do you think we can take them?"
  --upon recruitment "explore the world, you say?  I'm in!" "yay, friends!" "fine, I'll come with you.  just don't slow me down" "oh my... you smell heavenly.  can I come with you?"
  --"you look tasty- I mean X, can I come with you?", "Team X, huh?  I like the sound of that!", "you lot seem smart/wise, I think you have what it takes to reach the top."
  --when/after someone faints "X isn't with us anymore... but we have to keep going."
  --"if you faint as the leader, I'll gladly take your place."
  --specific personalities for mechanical creatures
  --specific personalities for childish creatures
  --verbal tics (chirp/tweet, meow, woof/arf, squeak, pika)
  --third-person dialogue
  --burned "ow, ow ,ow!  hot, hot!, hot!"
  --paralyzed "I can't feel my legs..." (check body type for this)
  --when there's only 2 members left, and no means to summon more recruits "we're the only ones left." "come on!  we can't give up!"
  --"I wonder what [fainted mon] would've done..."
  --lore on the legendary guildmasters
  ---
  --the rise of recent disasters- mystery dungeons
  --the guildmaster's treasures?
  --a challenge that no one has passed yet; people stopped bothering
  --but as the disasters appear, the pokemon begin to search for a leader
  --At low HP: "X's resolve is being tested..."
  --isn't my fur just luxurious? (vain personality)
  --first priority is for phrases that occur at each break point
  --would you like to hear a song? (if knows sing, round, or perish song)
  --"so it's just us now, huh?" (two pokemon, one or more has fainted)
  --We're going to be okay, right?  Tell me we're going to be okay.
  --Hey... if I don't make it out of here, tell (assembly mon) I love her.
  --talk about how one of their moves is a type, and it will be super-effective on an enemy found on the floor
  --rumors spread of a treasure in a dangerous location
  ----actually a trap; an ambush spot
  --My body wasn't ready for this...
  --Hey, what is it like to have arms?
  --That Red apricorn you have there.  We can use it to recruit a Fire-type Pokemon.
  --Let's find a strong Fire-type Pokemon to give that Red Apricorn to.
  --That Oran Berry we just picked up should help us in a pinch.
  --Those Mankeys are fighting-type Pokemon.  We should hit them with Flying type moves!
  --So why are you in this exploration? [Treasure][Meet Pokemon][Guildmaster][I dunno]
  --To set foot in this dungeon... you're either very brave, or foolhardy.
  --I've gotta hand it to you. Not many Exploration teams make it this far.
  --Too bad I don't have any hands.
  --You know what's weird?  We've been talking all this time and those (visible Pokemon) over there haven't made a single step towards us.  Do you think they're just that polite?
  --I don't like the look that (enemy pokemon) is giving me...
  --But enough talk, we need to get to the next floor.
  --But enough talk, we've got a situation here! (if multiple enemies are in sight)
  --Anyways, we need to keep moving.
  --Finally!  The stairs! (cheer when the stairs come into view, and load this dialogue up in time)
  --sleep
  --"Zzz...[pause=0] another five minutes please..."
  --"Zzz...[pause=10] Zzz..."
  --decoy
  --"...What?"
  --"...What's a \"decoy\"?"
  --"Do I look like I know what a \"decoy\" is?"
  --"Hey Einstein, I'm on your side!"
  --"Is there something on my face?"

  --evolve
  --"Hey, I think I'm ready to evolve now!"
  --"Hey, I think I can evolve into a {0} now!"
  --"I sense something... getting ready to evolve now"
  --"Did you know?  I'm ready to evolve now!"
  --"What should I evolve into?  Any ideas?"
  --"Oh, I just can't wait till I evolve!"
  --"I'm getting excited just thinking about it!"
  --"I swear to god when I evolve, I am going to kill you all!"

  --"Let's make it all the way to the end!"
  --"I wanna be...[pause=0]\nthe very best..."
  --"You teach me, and I'll teach you."
  --"I'll rescue you and if I do you gotta rescue me."
  --"Who should we recruit next?"
  --"Some apricorns can only recruit certain types of Pokémon.[pause=0] You know that, yes?"
  --"Rumor has it that whoever reaches the summit will be given the title of Guildmaster."
  --"So why do they call you the {0} Pokémon?", DataManager.Instance.GetMonster(character.BaseForm.Species).Title.ToLocal()

  --"Press {0} if you want me to lead the team.", (ii + 1).ToString()
  --"press 1 if you feel bad for [fainted mon]"

  
end

function COMMON.ShowTeamStorageMenu()
  UI:ResetSpeaker()
  
  local state = 0
  
  while state > -1 do
    
	local has_items = GAME:GetPlayerBagCount() + GAME:GetPlayerEquippedCount() > 0
	local has_storage = GAME:GetPlayerStorageCount() > 0
	
	
	local storage_choices = { { STRINGS:FormatKey('MENU_STORAGE_STORE'), has_items},
	{ STRINGS:FormatKey('MENU_STORAGE_TAKE_ITEM'), has_storage},
	{ STRINGS:FormatKey("MENU_STORAGE_MONEY"), true},
	{ STRINGS:FormatKey("MENU_CANCEL"), true}}
	UI:BeginChoiceMenu(STRINGS:FormatKey('DLG_WHAT_DO'), storage_choices, 1, 4)
	UI:WaitForChoice()
	local result = UI:ChoiceResult()
	
	if result == 1 then
	  UI:StorageMenu()
	  UI:WaitForChoice()
	elseif result == 2 then
	  UI:WithdrawMenu()
	  UI:WaitForChoice()
	elseif result == 3 then
	  UI:BankMenu()
	  UI:WaitForChoice()
	elseif result == 4 then
	  state = -1
	end
	
  end
  
end

function COMMON.GroundInteract(chara, target)
  GROUND:CharTurnToChar(target, chara)
  UI:SetSpeaker(target)
  
  local mon = RogueEssence.Data.DataManager.Instance:GetMonster(target.CurrentForm.Species)
  local form = mon.Forms[target.CurrentForm.Form]
  
  local personality = form:GetPersonalityType(target.Data.Discriminator)
  
  local personality_group = COMMON.PERSONALITY[personality]
  local pool = personality_group.WAIT
  local key = "TALK_WAIT_%04d"
  
  local running_pool = {table.unpack(pool)}
  local valid_quote = false
  local chosen_quote = ""
  
  while not valid_quote and #running_pool > 0 do
    valid_quote = true
    local chosen_idx = math.random(1, #running_pool)
	local chosen_pool_idx = running_pool[chosen_idx]
    chosen_quote = RogueEssence.StringKey(string.format(key, chosen_pool_idx)):ToLocal()
	
    chosen_quote = string.gsub(chosen_quote, "%[hero%]", chara:GetDisplayName())
    
	if not valid_quote then
      -- PrintInfo("Rejected "..chosen_quote)
	  table.remove(running_pool, chosen_idx)
	  chosen_quote = ""
	end
  end
  -- PrintInfo("Selected "..chosen_quote)
  
  
  UI:WaitShowDialogue(chosen_quote)
end

function COMMON.Rescued(zone, name, mail)
  

  if _DATA.CurrentReplay ~= nil then
    SOUND:PlayBattleSE("EVT_Title_Intro")
    GAME:FadeOut(true, 0)
    GAME:FadeIn(20)
    SOUND:PlayBGM("C05. Rescue.ogg", true)
	_DUNGEON:LogMsg(STRINGS:FormatKey("MSG_RESCUED_BY", name))
  else
                --//spawn the rescuers based on mail
    local leaderLoc = null;

    local team = RogueEssence.Dungeon.ExplorerTeam()
    team.Name = mail.RescuedBy

                --for (int ii = 0; ii < mail.RescuingTeam.Length; ii++)
                --{
                --    CharData character = new CharData();
                --    character.BaseForm = mail.RescuingTeam[ii];
                --    character.Nickname = mail.RescuingNames[ii];
                --    character.Level = 1;

                --    Character new_mob = new Character(character, team);

                --    Loc? destLoc = ZoneManager.Instance.CurrentMap.GetClosestTileForChar(new_mob, leaderLoc.HasValue ? leaderLoc.Value : ActiveTeam.Leader.CharLoc);

                --    if (destLoc == null)
                --        break;

                --    leaderLoc = destLoc;

                --    team.Players.Add(new_mob);
                --    new_mob.CharLoc = destLoc.Value;
                --    new_mob.CharDir = DirExt.ApproximateDir8(ActiveTeam.Leader.CharLoc - destLoc.Value);
                --    AITactic tactic = DataManager.Instance.GetAITactic(0);
                --    new_mob.Tactic = new AITactic(tactic);
                --}
                --ZoneManager.Instance.CurrentMap.MapTeams.Add(team);

                --//foreach (Character member in team.Players)
                --//    member.RefreshTraits();

    SOUND:PlayBattleSE("EVT_Title_Intro")
    GAME:FadeOut(true, 0)
    GAME:FadeIn(20)
                --yield return CoroutineManager.Instance.StartCoroutine(GameManager.Instance.FadeIn());

    SOUND:PlayBGM("C05. Rescue.ogg", true)
    TASK:WaitTask(_MENU:SetDialogue(STRINGS:FormatKey("MSG_RESCUES_LEFT", _DATA.Save.RescuesLeft)))
    GAME:WaitFrames(10)
  


                --//TODO: make the rescuers talk
                --yield return CoroutineManager.Instance.StartCoroutine(MenuManager.Instance.SetDialogue(team.Leader.Appearance, team.Leader:GetDisplayName(true), new EmoteStyle(Emotion.Normal), true, Text.Format("{0}, right?", ActiveTeam:GetDisplayName())));
                --yield return CoroutineManager.Instance.StartCoroutine(MenuManager.Instance.SetDialogue(team.Leader.Appearance, team.Leader:GetDisplayName(true), new EmoteStyle(Emotion.Normal), true, Text.Format("Glad we could make it! We got your SOS just in time!")));
                --yield return CoroutineManager.Instance.StartCoroutine(MenuManager.Instance.SetDialogue(team.Leader.Appearance, team.Leader:GetDisplayName(true), new EmoteStyle(Emotion.Normal), true, Text.Format("It's all in a day's work for {0}!", team:GetDisplayName())));
                --yield return CoroutineManager.Instance.StartCoroutine(MenuManager.Instance.SetDialogue(team.Leader.Appearance, team.Leader:GetDisplayName(true), new EmoteStyle(Emotion.Normal), true, Text.Format("Good luck, the rest is up to you!")));

                --//warp them out

                --for (int ii = team.Players.Count - 1; ii >= 0; ii--)
                --{
                --    GameManager.Instance.BattleSE("DUN_Send_Home");
                --    SingleEmitter emitter = new SingleEmitter(new BeamAnimData("Column_Yellow", 3));
                --    emitter.Layer = DrawLayer.Front;
                --    emitter.SetupEmit(team.Players[ii].MapLoc, team.Players[ii].MapLoc, team.Players[ii].CharDir);
                --    CreateAnim(emitter, DrawLayer.NoDraw);
                --    yield return CoroutineManager.Instance.StartCoroutine(team.Players[ii].DieSilent());
                --    yield return new WaitForFrames(20);
                --}
                --RemoveDeadTeams();
  end
  
  local rescue_idx = 39
  local rescue_status = RogueEssence.Dungeon.MapStatus(rescue_idx)
  rescue_status:LoadFromData()
  TASK:WaitTask(_DUNGEON:AddMapStatus(rescue_status))
end

function COMMON.EndRescue(zone, result, rescue, segmentID)
  COMMON.EndDayCycle()
  local zoneId = 1
  local structureId = -1
  local mapId = 12
  local entryId = 1
  GAME:EndDungeonRun(result, zoneId, structureId, mapId, entryId, true, true)
  SV.General.Rescue = result
  GAME:EnterZone(zoneId, structureId, mapId, entryId)
end

function COMMON.BeginDungeon(zoneId, segmentID, mapId)
  COMMON.EnterDungeonMissionCheck(zoneId, segmentID)
end

function COMMON.EnterDungeonMissionCheck(zoneId, segmentID)
  for name, mission in pairs(SV.test_grounds.Missions) do
    PrintInfo("Checking Mission: "..tostring(name))
	if mission.Complete == 0 and zoneId == mission.DestZone and segmentID == mission.DestSegment then
	  if mission.Type == 1 then -- escort
		
		-- add escort to team
		local mon_id = RogueEssence.Dungeon.MonsterID(mission.EscortSpecies, 0, 0, Gender.Male)
        local new_mob = _DATA.Save.ActiveTeam:CreatePlayer(_DATA.Save.Rand, mon_id, 50, -1, -1)
        _DATA.Save.ActiveTeam.Guests:Add(new_mob)
		
		-- place in a legal position on map
		local dest = _ZONE.CurrentMap:GetClosestTileForChar(new_mob, _DATA.Save.ActiveTeam.Leader.CharLoc)
        local endLoc = _DATA.Save.ActiveTeam.Leader.CharLoc
		if dest.HasValue then
		  endLoc = dest
		end
        new_mob.CharLoc = endLoc
		
		local talk_evt = RogueEssence.Dungeon.BattleScriptEvent("EscortInteract")
        new_mob.ActionEvents:Add(talk_evt)
		
		local tbl = LTBL(new_mob)
		tbl.Escort = name
	    
        UI:ResetSpeaker()
        UI:WaitShowDialogue("Added ".. new_mob.Name .." to the party as a guest.")
	  end
	end
  end
end


function COMMON.ExitDungeonMissionCheck(zoneId, segmentID)
  for name, mission in pairs(SV.test_grounds.Missions) do
    PrintInfo("Checking Mission: "..tostring(name))
	if mission.Complete == 0 and zoneId == mission.DestZone and segmentID == mission.DestSegment then
	  if mission.Type == 1 then -- escort
	    -- remove the escort from the party
		local escort = COMMON.FindMissionEscort(name)
		if escort then
		  _DUNGEON:RemoveChar(escort)
		end
	  end
	end
  end
end



function COMMON.FindMissionEscort(missionId)
  local escort = nil
  PrintInfo("Name: "..missionId)
  local party = GAME:GetPlayerGuestTable()
  for i, p in ipairs(party) do
    local e_tbl = LTBL(p)
	PrintInfo("Escort: "..e_tbl.Escort)
	if e_tbl.Escort == missionId then
	  escort = p
	  break
	end
  end
  return escort
end

function COMMON.EndDungeonDay(result, zoneId, structureId, mapId, entryId)
  COMMON.EndDayCycle()
  GAME:EndDungeonRun(result, zoneId, structureId, mapId, entryId, true, true)
  if GAME:InRogueMode() then
    GAME:RestartToTitle()
  else
	GAME:EnterZone(zoneId, structureId, mapId, entryId)
  end
end

function COMMON.EndSession(result, zoneId, structureId, mapId, entryId)
  GAME:EndDungeonRun(result, zoneId, structureId, mapId, entryId, false, false)
  GAME:EnterZone(zoneId, structureId, mapId, entryId)
end


function COMMON.EndDayCycle()
  --reshuffle items

  SV.adventure.Thief = false
  SV.base_shop = { }
  
  math.randomseed(GAME:GetDailySeed())
  --roll a random index from 1 to length of category
  --add the item in that index category to the shop
  --2 essentials, always
  local type_count = 2
	for ii = 1, type_count, 1 do
		local base_data = COMMON.ESSENTIALS[math.random(1, #COMMON.ESSENTIALS)]
		table.insert(SV.base_shop, base_data)
	end
  
  --1-2 ammo, always
  type_count = math.random(1, 2)
	for ii = 1, type_count, 1 do
		local base_data = COMMON.AMMO[math.random(1, #COMMON.AMMO)]
		table.insert(SV.base_shop, base_data)
	end
  
  --2-3 utilities, always
  type_count = math.random(3, 4)
	for ii = 1, type_count, 1 do
		local base_data = COMMON.UTILITIES[math.random(1, #COMMON.UTILITIES)]
		table.insert(SV.base_shop, base_data)
	end
  
  --1-2 orbs, always
  type_count = math.random(1, 2)
	for ii = 1, type_count, 1 do
		local base_data = COMMON.ORBS[math.random(1, #COMMON.ORBS)]
		table.insert(SV.base_shop, base_data)
	end
  
  --1-2 special item, always
  type_count = math.random(1, 2)
	for ii = 1, type_count, 1 do
		local base_data = COMMON.SPECIAL[math.random(1, #COMMON.SPECIAL)]
		table.insert(SV.base_shop, base_data)
	end
  
  
  local catalog = {}
  
  for ii = 1, #COMMON_GEN.TRADES_RANDOM, 1 do
	local base_data = COMMON_GEN.TRADES_RANDOM[ii]
	table.insert(catalog, base_data)
  end

  SV.base_trades = { }
  type_count = 5
	for ii = 1, type_count, 1 do
		local idx = math.random(1, #catalog)
		local base_data = catalog[idx]
		table.insert(SV.base_trades, base_data)
		table.remove(catalog, idx)
	end
end