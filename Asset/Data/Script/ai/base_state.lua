--[[
    Base AI state class
]]--
require 'common'

local BaseState = Class('BaseState')

--Constructor
function BaseState:initialize(parentai)
  assert(self, "BaseState:initialize(): Error, self is nil!")
  self.parentAI = parentai --Reference to owner of the state, so it can be accessed through this variable
end

-- Called when transitioning from a state to this one!
-- prevstate may be nil
function BaseState:Begin(prevstate, entity)
  assert(self, "BaseState:Begin(): Error, self is nil!")
  -- default does nothing
  
  if DEBUG.GroundAIShowDebugInfo then print("Beginning AI state ",tostring(self)) end
end

-- When the entity is in this state this method is called to execute
-- state related code
function BaseState:Run(entity)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  assert(self, "BaseState:Run(): Error, self is nil!")
  -- default does nothing
end

-- When ending the current state to move to another one.
-- nextstate may be nil
function BaseState:End(nextstate, entity)
  assert(self, "BaseState:End(): Error, self is nil!")
  -- default does nothing
end

return BaseState