require 'common'

local guild_path = {}
local MapStrings = {}
--------------------------------------------------
-- Map Callbacks
--------------------------------------------------
function guild_path.Init(map)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo("=>> Init_guild_path")
  MapStrings = COMMON.AutoLoadLocalizedStrings()
  GROUND:RefreshPlayer()
end

function guild_path.Enter(map)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine

  GAME:FadeIn(20)
end

function guild_path.Update(map, time)
end

--------------------------------------------------
-- Map Begin Functions
--------------------------------------------------

--------------------------------------------------
-- Objects Callbacks
--------------------------------------------------
function guild_path.East_Exit_Touch(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  GAME:FadeOut(false, 20)
  GAME:EnterGroundMap("base_camp", "entrance_west")
end

function guild_path.Hut_Entrance_Touch(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  GAME:FadeOut(false, 20)
  GAME:EnterGroundMap("guild_hut", "entrance_south")
end

return guild_path