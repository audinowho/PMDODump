require 'common'

local zone_19 = {}
--------------------------------------------------
-- Map Callbacks
--------------------------------------------------
function zone_19.Init(zone)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo("=>> Init_zone_19")
  

end

function zone_19.Rescued(zone, name, mail)
  COMMON.Rescued(zone, name, mail)
end

function zone_19.EnterSegment(zone, rescuing, segmentID, mapID)
  if rescuing ~= true then
    COMMON.BeginDungeon(zone.ID, segmentID, mapID)
  end
end

function zone_19.ExitSegment(zone, result, rescue, segmentID, mapID)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo("=>> ExitSegment_zone_19 result "..tostring(result).." segment "..tostring(segmentID))
  
  --TODO: rogue mode endings
  --need to restart to title
  
  --first check for rescue flag; if we're in rescue mode then take a different path
  COMMON.ExitDungeonMissionCheck(zone.ID, segmentID)
  if rescue == true then
    COMMON.EndRescue(zone, result, segmentID)
  else
	if not SV.base_camp.ExpositionComplete then
	  GAME:SetRescueAllowed(true)
	end
    if result ~= RogueEssence.Data.GameProgress.ResultType.Cleared then
    --if result is defeat, unknown, timed out, or escaped, end the game with the destination as the last checkpoint
    --defeat is the same for all segments
      COMMON.EndDungeonDay(result, SV.checkpoint.Zone, SV.checkpoint.Segment, SV.checkpoint.Map, SV.checkpoint.Entry)
    else
      if segmentID == 0 then
        --if the result is victory, send to the destination zone, or just win to the destination zone
	    if SV.guildmaster_summit.ExpositionComplete then
          COMMON.EndDungeonDay(result, 1,-1,8,0)
	    else
	      --GAME:EnterZone(1,-1,8, 0)
		  COMMON.EndDungeonDay(result, 1,-1,0,0)
	    end
      elseif segmentID == 1 then
    --for the boss segment, set a save variable
    --the MAP's script will play the final cutscene
    --AND it will end the game
        SV.guildmaster_summit.BattleComplete = true
        GAME:EnterZone(1, -1, 8, 0)
      else
        PrintInfo("No exit procedure found!")
	  COMMON.EndDungeonDay(result, SV.checkpoint.Zone, SV.checkpoint.Segment, SV.checkpoint.Map, SV.checkpoint.Entry)
      end
    end
  end
end

return zone_19