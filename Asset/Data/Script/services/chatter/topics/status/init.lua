--[[
    StatusTopic module
      Discussion topic centered around the talker's current status.
--]]
require 'common'
local BaseTopic = require 'services.chatter.topics.basetopic'

local StatusTopic = Class('StatusTopic', BaseTopic)

--Include the lines
local Statuses = dofile(SCRIPT:ServiceDirectoryPath() .. '/chatter/topics/status/lines.lua')

--[[
--]]
function StatusTopic:initialize()
  BaseTopic.initialize(self)
  
  self.NextTalkTopics = {} -- since we determine inside "HasSomethingToSay" what topic to use next, we'll store said topic in here
end

--[[
--]]
function StatusTopic:OnLoad()
end

--[[
--]]
function StatusTopic:HasSomethingToSay(talker, activator)
  
  --Skip if we already have some topics waiting
  if self.NextTalkTopics and #(self.NextTalkTopics) > 0 then
    return true
  end
  
  for k,v in pairs(Statuses) do
    if v.Predicate(talker, activator) then
      table.insert(self.NextTalkTopics, k)
      return true
    end
  end
  
  --Unset topic and return false if we have nothing to talk of
  self.NextTalkTopics = {}
  return false
end

--[[
--]]
function StatusTopic:Talk(talker, activator)
  assert(self.NextTalkTopics, "StatusTopic:Talk(): nil next topics!")
  local mostrelevant = 1
  
  if #(self.NextTalkTopics) == 0 then
    return
  end
  
  for i,v in ipairs(self.NextTalkTopics) do    
    if self.NextTalkTopics[mostrelevant].Relevance() > v.Relevance() then
      mostrelevant = i
    end
  end
  
  --Do most relevant
  local topic = self.NextTalkTopics[mostrelevant]
  Statuses[topic].Talk(talker, activator)
  
  --Remove from the list
  table.remove(self.NextTalkTopics, mostrelevant)
end

return StatusTopic