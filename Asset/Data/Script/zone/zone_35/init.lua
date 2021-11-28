require 'common'

local zone_35 = {}
--------------------------------------------------
-- Map Callbacks
--------------------------------------------------
function zone_35.Init(zone)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo("=>> Init_zone_35")
  

end

function zone_35.Rescued(zone, name, mail)
  COMMON.Rescued(zone, name, mail)
end

function zone_35.EnterSegment(zone, rescuing, segmentID, mapID)
  if rescuing ~= true then
    COMMON.BeginDungeon(zone.ID, segmentID, mapID)
  end
end

function zone_35.ExitSegment(zone, result, rescue, segmentID, mapID)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo("=>> ExitSegment_zone_35 result "..tostring(result).." segment "..tostring(segmentID))
  
  --first check for rescue flag; if we're in rescue mode then take a different path
  if result ~= RogueEssence.Data.GameProgress.ResultType.Cleared then
    COMMON.EndSession(result, 1, -1, 1, 0)
  else
    -- TODO: make noctowl say something.  requires setting save variables
    COMMON.EndSession(result, 1, -1, 1, 0)
  end
  
end

return zone_35