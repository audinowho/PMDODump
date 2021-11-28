--[[
  Basic generic ground mode AI implementation.
  
  An entity running this AI will randomly turn and wander a bit from time to time.
]]--
require 'common'
local BaseAI = require 'ai.ground_baseai'
local BaseState = require 'ai.base_state'

-------------------------------
-- States Class Definitions
-------------------------------
--[[------------------------------------------------------------------------
    Idle state:
      The entity will determine an idle action to take after a certain amount of time has passed.
      It will play its idle animation meanwhile. And if it has a current task to execute it will 
      switch to the "Act" state.
]]--------------------------------------------------------------------------
local StateIdle = Class('StateIdle', BaseState)

--The range of possible wait time between idle movements, in number of calls to the state's "Run" method/frames
StateIdle.WaitMin = 40
StateIdle.WaitMax = 180

function StateIdle:initialize(parentai)
  StateIdle.super.initialize(self, parentai)

  --Set a time in ticks for the next idle action
  self.idletimer = GAME.Rand:Next(self.parentAI.IdleDelayMin, self.parentAI.IdleDelayMax) 
end

function StateIdle:Begin(prevstate, entity)
  assert(self, "StateIdle:Begin(): Error, self is nil!")
  StateIdle.super.Begin(self, prevstate, entity)
  --Play Idle anim
end

function StateIdle:Run(entity)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  assert(self, "StateIdle:Run(): Error, self is nil!")
  StateIdle.super.Run(self, entity)
  local ent = LUA_ENGINE:CastToGroundAIUser(entity)
  
  -- If a task is set, move to act
  if ent:CurrentTask() then self.parentAI:SetState("Act") end
  
  --Suspend while interacting with the entity
  if ent.IsInteracting then 
    return 
  end
  
  --If enough time passed, wander or turn
  if self.idletimer <= 0 then
    self:DoIdle()
  end
  
  --Tick down the idle timer
  self.idletimer = self.idletimer - 1 
end

-- Does pick an idle action to perform
function StateIdle:DoIdle()
  self.idletimer = GAME.Rand:Next(StateIdle.WaitMin, StateIdle.WaitMax)  -- Set the idle time for the next update
  local choice = GAME.Rand:Next(0, 5) --Randomly decide to turn or wander
  
  if choice == 0 then
    self.parentAI:SetState("IdleTurn")
  else
    self.parentAI:SetState("IdleWander")
  end
end

--[[------------------------------------------------------------------------
    Act state:
      In this special state, the AI will run the currently assigned
      task if there is one. Then fall back to idle!
]]--------------------------------------------------------------------------
local StateAct = Class('StateAct', BaseState)

function StateAct:Begin(prevstate, entity)
  assert(self, "StateAct:Begin(): Error, self is nil!")
  StateAct.super.Begin(self, prevstate, entity)
  --Stop Idle anim
end

function StateAct:Run(entity)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  assert(self, "StateAct:Run(): Error, self is nil!")
  StateAct.super.Run(self, entity)
  
  --Run current task if any
  local ent = LUA_ENGINE:CastToGroundAIUser(entity)
  
  --When interaction stopped, return to idle
  if not ent:CurrentTask() then self.parentAI:SetState("Idle") end
end

--[[------------------------------------------------------------------------
    IdleWander state:
      When the entity is in this state, it will try to move to a random position.
]]--------------------------------------------------------------------------
local StateIdleWander = Class('StateIdleWander', BaseState)
StateIdleWander.WanderStepMin = 16
StateIdleWander.WanderStepMax = 32

function StateIdleWander:initialize(parentai)
  assert(self, "StateIdleWander:initialize(): Error, self is nil!")
  StateIdleWander.super.initialize(self, parentai)
  
  --Set a maximum time to reach the position, since we don't really do pathfinding or anything, and something
  -- in the way can get the entity stuck in this state forever
  self.timeout = StateIdle.WaitMax
  self.timeWandering = 0
  self.WanderComplete = false
end

function StateIdleWander:Begin(prevstate, entity)
  assert(self, "StateIdleWander:Begin(): Error, self is nil!")
  StateIdleWander.super.Begin(self, prevstate, entity)
  self.timeWandering = 0
  
  self.wanderRadius = GAME.Rand:Next(self.parentAI.WanderStepMin, self.parentAI.WanderStepMax)
  self:CalculateWanderPos(entity)
  self:SetTask(entity)
end

-- Determine a random position around the entity to move to
function StateIdleWander:CalculateWanderPos(entity)
  assert(self, "StateIdleWander:Begin(): Error, self is nil!")
  
  self.WanderPos = entity.Position
  self.WanderPos.X = self.WanderPos.X + (GAME.Rand:Next(0, self.wanderRadius * 2) - self.wanderRadius)
  self.WanderPos.Y = self.WanderPos.Y + (GAME.Rand:Next(0, self.wanderRadius * 2) - self.wanderRadius)

  --Clamp target position within the allowed move zone
  if self.WanderPos.X > self.parentAI.WanderZonePos.X + self.parentAI.WanderZoneSize.X then
    self.WanderPos.X = self.parentAI.WanderZonePos.X + self.parentAI.WanderZoneSize.X
  elseif self.WanderPos.X < self.parentAI.WanderZonePos.X then
     self.WanderPos.X = self.parentAI.WanderZonePos.X
  end
  if self.WanderPos.Y > self.parentAI.WanderZonePos.Y + self.parentAI.WanderZoneSize.Y then
    self.WanderPos.Y = self.parentAI.WanderZonePos.Y + self.parentAI.WanderZoneSize.Y
  elseif self.WanderPos.Y < self.parentAI.WanderZonePos.Y then
     self.WanderPos.Y = self.parentAI.WanderZonePos.Y
  end
  
  local travelvector = self.WanderPos - entity.Position
  local distance = math.sqrt(travelvector.X * travelvector.X) + 
                   math.sqrt(travelvector.Y * travelvector.Y);
                   
  if distance == 0 then
    self.wanderStep = RogueElements.Loc()
  else
    local xpos = travelvector.X / distance
    local ypos = travelvector.Y / distance
    self.wanderStep = {X = xpos, Y = ypos}
  end
end

function StateIdleWander:SetTask(entity)
  local wanderpos = self.WanderPos
  local state = self
  TASK:StartEntityTask(entity, 
    function()
      GROUND:MoveToPosition(entity, wanderpos.X, wanderpos.Y, false, self.parentAI.WanderSpeed)
      state.WanderComplete = true
    end)
end

function StateIdleWander:Run(entity)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  assert(self, "StateIdleWander:Run(): Error, self is nil!")
  StateIdleWander.super.Run(self, entity)
  
  local ent = LUA_ENGINE:CastToGroundChar(entity)
  
  -- If a task is set, move to act
  if ent:CurrentTask() then self.parentAI:SetState("Act") end
  
  --Suspend while interacting with the entity
  if ent.IsInteracting then 
    self.parentAI:SetState("Idle")
  end
    
  -- When the position is reached, or we've been trying to wander for too long, go back to idle
  local entXDiff = math.abs(ent.X - self.WanderPos.X)
  local entYDiff = math.abs(ent.Y - self.WanderPos.Y)
  
  if (entXDiff <= 1.0 or entYDiff <= 1.0) or self.WanderComplete or self.timeWandering >= self.timeout then
    self.parentAI:SetState("Idle")
  end
    
  -- Step towards a random position decided at the transition to wander
--  local dir = GAME:VectorToDirection(self.wanderStep.X, self.wanderStep.Y)
--  if dir ~= RogueElements.Dir8.None then
--    ent.CurrentCommand = RogueEssence.Dungeon.GameAction(RogueEssence.Dungeon.GameAction.ActionType.Move, dir, 0);
--  end
  
  --Increment timeout counter
  self.timeWandering = self.timeWandering + 1
end

--[[------------------------------------------------------------------------
  IdleTurn state:
    When the entity is in this state, it will perform a slow turn towards a 
    random direction.
]]--------------------------------------------------------------------------
local StateIdleTurn = Class('IdleTurn', BaseState)

function StateIdleTurn:Begin(prevstate, entity)
  assert(self, "StateIdleTurn:Begin(): Error, self is nil!")
  StateIdleTurn.super.Begin(self, prevstate, entity)
  
  -- Determine a random direction
  self.turnDir = GAME.Rand:Next(0, 7)
end

function StateIdleTurn:Run(entity)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  assert(self, "StateIdleTurn:Run(): Error, self is nil!")
  StateIdleTurn.super.Run(self, entity)
  local ent = LUA_ENGINE:CastToGroundAIUser(entity)

  -- If a task is set, move to act
  if ent:CurrentTask() then self.parentAI:SetState("Act") end
  
  --Suspend while interacting with the entity
  if ent.IsInteracting then 
    self.parentAI:SetState("Idle")
  end
  
  local dir = GAME:RandomDirection()
  if dir ~= RogueElements.Dir8.None then 
    GROUND:EntTurn(ent, dir)
  end
  
  -- When the direction is reached, go back to idle
  --TODO: If we turn over several frams, add a conditional here!
  self.parentAI:SetState("Idle")
end

--------------------------
-- ground_default AI Class
--------------------------
-- Basic ground character AI template
local ground_default = Class('ground_default', BaseAI)

--Constructor
function ground_default:initialize(wanderzoneloc, wanderzonesize, wanderspeed, wanderstepmin, wanderstepmax, idledelaymin, idledelaymax)
  assert(self, "ground_default:initialize(): Error, self is nil!")
  ground_default.super.initialize(self)
  self.Memory = {} -- Where the AI will store any state shared variables it needs to keep track of at runtime (not serialized)
  self.NextState = "Idle" --Always set the initial state as the next state, so "Begin" is run!
  
  --Set the wander area
  if not wanderzoneloc then
    self.WanderZonePos = RogueElements.Loc()
  else
    self.WanderZonePos = wanderzoneloc
  end
  
  if not wanderzonesize then
    self.WanderZoneSize = RogueElements.Loc()
  else
    self.WanderZoneSize = wanderzonesize
  end
  
  --Set the wander speed
  if wanderspeed >= 1 then
    self.WanderSpeed = wanderspeed
  else
    self.WanderSpeed = 1
  end
  
  --Set the delay between idle actions
  if not idledelaymin then
    self.IdleDelayMin = StateIdle.WaitMin
  else
    self.IdleDelayMin = idledelaymin
  end
  if not idledelaymax then
    self.IdleDelayMax = StateIdle.WaitMax
  else
    self.IdleDelayMax = idledelaymax
  end
  
  --Set the min and max step possible
  if not wanderstepmin then
    self.WanderStepMin = StateIdleWander.WanderStepMin
  else
    self.WanderStepMin = wanderstepmin
  end
  if not wanderstepmax then
    self.WanderStepMax = StateIdleWander.WanderStepMax
  else
    self.WanderStepMax = wanderstepmax
  end
  
  -- Place the instances of the states classes into the States table
  self.States.Idle        = StateIdle:new(self)
  self.States.Act         = StateAct:new(self)
  self.States.IdleWander  = StateIdleWander:new(self)
  self.States.IdleTurn    = StateIdleTurn:new(self)
end

--Return the class
return ground_default