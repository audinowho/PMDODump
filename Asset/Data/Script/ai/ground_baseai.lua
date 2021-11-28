--[[
    A base AI class that does nothing.
]]--
require 'common'
local BaseState = require 'ai.base_state'

local BaseAI = Class('BaseAI')

function BaseAI:initialize()
  assert(self, "BaseAI:initialize(): Error, self is nil!")
  local inst = self
  self.States = 
  {
    None = Class('StateNone', BaseState):new(inst) --Base state does nothing
  }
  self.CurrentState = "None"
  self.NextState = ""
  self.LastState = ""
end

function BaseAI:SetState(nextstate)
  assert(self, "BaseAI:SetState(): Error, self is nil!")
  --PrintInfo("Next State "..tostring(nextstate))
  self.NextState = nextstate
end

---
-- Updates the current state as needed!
function BaseAI:Update(entity)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  assert(self, "BaseAI:Update(): Error, self is nil!")
  
  --local debugUI = "STATE:"..tostring(self.CurrentState) .. "\nAI X:"..entity.Position.X .. " Y:" .. entity.Position.Y
  --GAME:SetDebugUI(debugUI)
  
  --Update state
  curstate = self.States[self.CurrentState]
  if curstate then curstate:Run(entity); end
  
  --Check if we update the current state, or switch to a new one!
  if self.NextState and string.len(self.NextState) ~= 0 then 
    --PrintInfo("Switch State "..tostring(self.CurrentState).." -> "..tostring(self.NextState))
	--Do state switch!
    local laststate = self.States[self.CurrentState]
    local nextstate = self.States[self.NextState]
    
    self.LastState = self.CurrentState
    self.CurrentState = self.NextState
    self.NextState = ""
    
    if laststate then laststate:End(self.NextState, entity); end
    if nextstate then nextstate:Begin(self.LastState, entity); end
  end
  
  
end

return BaseAI