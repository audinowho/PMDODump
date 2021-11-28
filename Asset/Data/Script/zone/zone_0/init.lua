require 'common'

local zone_0 = {}
--------------------------------------------------
-- Map Callbacks
--------------------------------------------------
function zone_0.Init(zone)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo("=>> Init_zone_0")
  

end

function zone_0.Rescued(zone, name, mail)
  COMMON.Rescued(zone, name, mail)
end

function zone_0.EnterSegment(zone, rescuing, segmentID, mapID)
  PrintInfo("=>> EnterSegment_zone_0")
  if rescuing ~= true then
    COMMON.BeginDungeon(zone.ID, segmentID, mapID)
  end
end

function zone_0.ExitSegment(zone, result, rescue, segmentID, mapID)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  
  COMMON.ExitDungeonMissionCheck(zone.ID, segmentID)
  if rescue == true then
    COMMON.EndRescue(zone, result, segmentID)
  else
    COMMON.EndSession(result, 0, -1, 0, 0)
  end
end

return zone_0