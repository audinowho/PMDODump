require 'common'

local rest_stop = {}
local MapStrings = {}
--------------------------------------------------
-- Map Callbacks
--------------------------------------------------
function rest_stop.Init(map)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo("=>> Init_rest_stop")
  MapStrings = COMMON.AutoLoadLocalizedStrings()
  COMMON.RespawnAllies()
end

function rest_stop.Enter(map)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine

  SV.checkpoint = 
  {
    Zone    = 1, Segment  = -1,
    Map  = 6, Entry  = 1
  }
  
  --when arriving the first time, play this cutscene
  if not SV.rest_stop.ExpositionComplete then
    rest_stop.BeginExposition()
    SV.rest_stop.ExpositionComplete = true
  else
    GAME:FadeIn(20)
  end
end

function rest_stop.Update(map, time)
end

--------------------------------------------------
-- Map Begin Functions
--------------------------------------------------
function rest_stop.BeginExposition()
  
  UI:WaitShowTitle(GAME:GetCurrentGround().Name:ToLocal(), 20);
  GAME:WaitFrames(30);
  UI:WaitHideTitle(20);
  GAME:FadeIn(20)
  
  GAME:UnlockDungeon(6)
end

--------------------------------------------------
-- Objects Callbacks
--------------------------------------------------
function rest_stop.North_Exit_Touch(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  
  local dungeon_entrances = { 6 }
  --also dungeon 21: royal halls, is accessible by ???
  --also dungeon 22: cave of solace, is accessible by having 8 key items
  local ground_entrances = {{Flag=SV.final_stop.ExpositionComplete,Zone=1,ID=7,Entry=0},
  {Flag=SV.guildmaster_summit.ExpositionComplete,Zone=1,ID=8,Entry=0}}
  COMMON.ShowDestinationMenu(dungeon_entrances,ground_entrances)
end

function rest_stop.South_Exit_Touch(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  
  local dungeon_entrances = { }
  local ground_entrances = {{Flag=true,Zone=1,ID=1,Entry=3},
  {Flag=SV.forest_camp.ExpositionComplete,Zone=1,ID=3,Entry=2},
  {Flag=SV.cliff_camp.ExpositionComplete,Zone=1,ID=4,Entry=2},
  {Flag=SV.canyon_camp.ExpositionComplete,Zone=1,ID=5,Entry=2}}
  COMMON.ShowDestinationMenu(dungeon_entrances,ground_entrances)
end

function rest_stop.Assembly_Action(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  UI:ResetSpeaker()
  COMMON.ShowTeamAssemblyMenu(obj, COMMON.RespawnAllies)
end

function rest_stop.Storage_Action(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  COMMON:ShowTeamStorageMenu()
end




function rest_stop.Teammate1_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  COMMON.GroundInteract(activator, chara, true)
end

function rest_stop.Teammate2_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  COMMON.GroundInteract(activator, chara, true)
end

function rest_stop.Teammate3_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  COMMON.GroundInteract(activator, chara, true)
end


return rest_stop