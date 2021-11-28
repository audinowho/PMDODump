require 'common'

local guild_hut = {}
local MapStrings = {}
--------------------------------------------------
-- Map Callbacks
--------------------------------------------------
function guild_hut.Init(map)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo("=>> Init_guild_hut")
  MapStrings = COMMON.AutoLoadLocalizedStrings()
end

function guild_hut.Enter(map)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine

  GAME:FadeIn(20)
end

function guild_hut.Update(map, time)
end

--------------------------------------------------
-- Map Begin Functions
--------------------------------------------------

--------------------------------------------------
-- Objects Callbacks
--------------------------------------------------
function guild_hut.South_Exit_Touch(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  GAME:FadeOut(false, 20)
  GAME:EnterGroundMap("guild_path", "entrance_hut")
end

function guild_hut.Card_Portal_Touch(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  
  local dungeon_entrances = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40 }
  local ground_entrances = { }
  COMMON.ShowDestinationMenu(dungeon_entrances,ground_entrances)
end

return guild_hut