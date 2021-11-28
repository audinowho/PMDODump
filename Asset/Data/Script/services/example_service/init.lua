--[[
    Example Service
    
    This is an example to demonstrate how to use the BaseService class to implement a game service.
    
    **NOTE:** After declaring you service, you have to include your package inside the main.lua file!
]]--
require 'common'
require 'services.baseservice'

--Declare class ExampleService
local ExampleService = Class('ExampleService', BaseService)

--[[---------------------------------------------------------------
    ExampleService:initialize()
      ExampleService class constructor
---------------------------------------------------------------]]
function ExampleService:initialize()
  BaseService.initialize(self)
  self.mapname = ""
  PrintInfo('ExampleService:initialize()')
end

--[[---------------------------------------------------------------
    ExampleService:__gc()
      ExampleService class gc method
      Essentially called when the garbage collector collects the service.
  ---------------------------------------------------------------]]
function ExampleService:__gc()
  PrintInfo('*****************ExampleService:__gc()')
end

--[[---------------------------------------------------------------
    ExampleService:OnInit()
      Called on initialization of the script engine by the game!
---------------------------------------------------------------]]
function ExampleService:OnInit()
  assert(self, 'ExampleService:OnInit() : self is null!')
  PrintInfo("\n<!> ExampleSvc: Init..")
end

--[[---------------------------------------------------------------
    ExampleService:OnDeinit()
      Called on de-initialization of the script engine by the game!
---------------------------------------------------------------]]
function ExampleService:OnDeinit()
  assert(self, 'ExampleService:OnDeinit() : self is null!')
  PrintInfo("\n<!> ExampleSvc: Deinit..")
end

--[[---------------------------------------------------------------
    ExampleService:OnGroundMapPrepare(mapid)
      When a new ground map is loaded this is  called!
---------------------------------------------------------------]]
function ExampleService:OnGroundMapPrepare(mapid)
  assert(self, 'ExampleService:OnGroundMapPrepare() : self is null!')
  PrintInfo("\n<!> ExampleSvc: Preparing map " .. tostring(mapid))
  self.mapname = mapid
end

--[[---------------------------------------------------------------
    ExampleService:OnGroundMapEnter()
      When the player enters a ground map this is called
---------------------------------------------------------------]]
function ExampleService:OnGroundMapEnter(mapid, entryinf)
  assert(self, 'ExampleService:OnGroundMapEnter() : self is null!')
  PrintInfo("\n<!> ExampleSvc: Entered map " .. tostring(mapid))
  self.mapname = mapid
end

--[[---------------------------------------------------------------
    ExampleService:OnGroundMapExit(mapid)
      When the player leaves a ground map this is called
---------------------------------------------------------------]]
function ExampleService:OnGroundMapExit(mapid)
  assert(self, 'ExampleService:OnGroundMapExit() : self is null!')
  PrintInfo("\n<!> ExampleSvc: Exited map " .. tostring(mapid))
end


---Summary
-- Subscribe to all channels this service wants callbacks from
function ExampleService:Subscribe(med)
  med:Subscribe("ExampleService", EngineServiceEvents.Init,                function() self.OnInit(self) end )
  med:Subscribe("ExampleService", EngineServiceEvents.Deinit,              function() self.OnDeinit(self) end )
--  med:Subscribe("ExampleService", EngineServiceEvents.GraphicsLoad,        function() self.OnGraphicsLoad(self) end )
--  med:Subscribe("ExampleService", EngineServiceEvents.GraphicsUnload,      function() self.OnGraphicsUnload(self) end )
--  med:Subscribe("ExampleService", EngineServiceEvents.Restart,             function() self.OnRestart(self) end )
--  med:Subscribe("ExampleService", EngineServiceEvents.Update,              function(curtime) self.OnUpdate(self,curtime) end )
--  med:Subscribe("ExampleService", EngineServiceEvents.GroundEntityInteract,function(activator, target) self.OnGroundEntityInteract(self, activator, target) end )
--  med:Subscribe("ExampleService", EngineServiceEvents.DungeonModeBegin,    function() self.OnDungeonModeBegin(self) end )
--  med:Subscribe("ExampleService", EngineServiceEvents.DungeonModeEnd,      function() self.OnDungeonModeEnd(self) end )
--  med:Subscribe("ExampleService", EngineServiceEvents.DungeonFloorPrepare, function(dungeonloc) self.OnDungeonFloorPrepare(self, dungeonloc) end )
--  med:Subscribe("ExampleService", EngineServiceEvents.DungeonFloorBegin,   function(dungeonloc, entryinf) self.OnDungeonFloorBegin(self, dungeonloc, entrypos) end )
--  med:Subscribe("ExampleService", EngineServiceEvents.DungeonFloorEnd,     function(dungeonloc, result) self.OnDungeonFloorEnd(self, dungeonloc, result) end )
--  med:Subscribe("ExampleService", EngineServiceEvents.GroundModeBegin,     function() self.OnGroundModeBegin(self) end )
--  med:Subscribe("ExampleService", EngineServiceEvents.GroundModeEnd,       function() self.OnGroundModeEnd(self) end )
  med:Subscribe("ExampleService", EngineServiceEvents.GroundMapPrepare,    function(mapid) self.OnGroundMapPrepare(self, mapid) end )
  med:Subscribe("ExampleService", EngineServiceEvents.GroundMapEnter,      function(mapid, entryinf) self.OnGroundMapEnter(self, mapid, entryinf) end )
  med:Subscribe("ExampleService", EngineServiceEvents.GroundMapExit,       function(mapid) self.OnGroundMapExit(self, mapid) end )
end

---Summary
-- un-subscribe to all channels this service subscribed to
function ExampleService:UnSubscribe(med)
end

---Summary
-- The update method is run as a coroutine for each services.
function ExampleService:Update(gtime)
--  while(true)
--    coroutine.yield()
--  end
end

--Add our service
SCRIPT:AddService("ExampleService", ExampleService:new())
return ExampleService