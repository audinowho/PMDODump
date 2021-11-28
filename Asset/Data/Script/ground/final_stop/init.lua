require 'common'

local final_stop = {}
local MapStrings = {}
--------------------------------------------------
-- Map Callbacks
--------------------------------------------------
function final_stop.Init(map)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo("=>> Init_final_stop")
  MapStrings = COMMON.AutoLoadLocalizedStrings()
  COMMON.RespawnAllies()
  
  GROUND:AddMapStatus(7)
end

function final_stop.Enter(map)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine

  SV.checkpoint = 
  {
    Zone    = 1, Segment  = -1,
    Map  = 7, Entry  = 1
  }
  
  --when arriving the first time, play this cutscene
  if not SV.final_stop.ExpositionComplete then
    final_stop.BeginExposition()
    SV.final_stop.ExpositionComplete = true
  else
    GAME:FadeIn(20)
  end
end

function final_stop.Update(map, time)
end

--------------------------------------------------
-- Map Begin Functions
--------------------------------------------------
function final_stop.BeginExposition()
  
  UI:WaitShowTitle(GAME:GetCurrentGround().Name:ToLocal(), 20);
  GAME:WaitFrames(30);
  UI:WaitHideTitle(20);
  GAME:FadeIn(20)
  
  GAME:UnlockDungeon(7)
end

--------------------------------------------------
-- Objects Callbacks
--------------------------------------------------
function final_stop.North_Exit_Touch(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  
  local dungeon_entrances = { 7 }
  --also dungeon 23: the sky is accessible by talking to a shaymin
  local ground_entrances = {{Flag=SV.guildmaster_summit.ExpositionComplete,Zone=1,ID=8,Entry=0}}
  COMMON.ShowDestinationMenu(dungeon_entrances,ground_entrances)
end

function final_stop.South_Exit_Touch(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  local dungeon_entrances = { }
  local ground_entrances = {{Flag=true,Zone=1,ID=1,Entry=3},
  {Flag=SV.forest_camp.ExpositionComplete,Zone=1,ID=3,Entry=2},
  {Flag=SV.cliff_camp.ExpositionComplete,Zone=1,ID=4,Entry=2},
  {Flag=SV.canyon_camp.ExpositionComplete,Zone=1,ID=5,Entry=2},
  {Flag=SV.rest_stop.ExpositionComplete,Zone=1,ID=6,Entry=2}}
  COMMON.ShowDestinationMenu(dungeon_entrances,ground_entrances)
end

function final_stop.Assembly_Action(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  UI:ResetSpeaker()
  COMMON.ShowTeamAssemblyMenu(obj, COMMON.RespawnAllies)
end

function final_stop.Storage_Action(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  COMMON:ShowTeamStorageMenu()
end



function final_stop.Teammate1_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  COMMON.GroundInteract(activator, chara, true)
end

function final_stop.Teammate2_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  COMMON.GroundInteract(activator, chara, true)
end

function final_stop.Teammate3_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  COMMON.GroundInteract(activator, chara, true)
end

return final_stop