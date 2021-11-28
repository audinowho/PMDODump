--[[
    This class is stored inside an NPC and handles the talking!
    
    Is also used to determine personality of the talker.
--]]
require 'common'
local BaseTalker = Class('BaseTalker')

--[[
    Construction
--]]
function BaseTalker:initialize(chara)
  self.Owner    = chara
  
  -- Get the running chatter service instance
  local cheng = SCRIPT:GetService("ChatterEngine")
  
  -- Load topics from the instance
  self.Topics = {}
  for k,v in pairs(cheng.GetTopics()) do
    local prio = tonumber(v.Priority)
    
    if not self.Topics[prio] then
      self.Topics[prio] = {}
    end
    self.Topics[prio][k] = v.Module()
  end
end

--[[
    OnInteract
      Should be used as a coroutine. Run it to make the talker talk.
--]]
function BaseTalker:OnInteract(activator)
  -- Decide what topic to come up with
  for i,v in ipairs(self.Topics) do
    for key,topic in pairs(v) do
      if topic.HasSomethingToSay(self.Owner, activator) then
        topic.Talk(self.Owner, activator)
      end
    end
  end
  
end


--[[
    GetLine
      Returns a line for a given line name for the current talker.
--]]
function BaseTalker:GetLine(linename)
  local found = self.Dialogue[linename]
  
  if found == nil then
    --Fallback to default
  else
    --Return the line!
  end
  
end


return BaseTalker