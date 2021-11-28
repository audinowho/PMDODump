--[[
  Inherit this class by using
      NewClass = Class("NewClass", BaseService)
  To make a new class object meant to run a lua service.
  
  A lua service is a persistent lua script that can receive generic callbacks from the engine during execution.
  This allows it to override or modify usual game behavior as the game runs, or offer additional services to script packages.
]]--

require 'common'


-------------------------------------------------------
-- BaseService 
-------------------------------------------------------
BaseService = Class("BaseService")

---Summary
-- Service constructor
function BaseService:initialize()
end


---Summary
-- Subscribe to all channels this service wants callbacks from
function BaseService:Subscribe(med)
--  med:Subscribe("BaseService", EngineServiceEvents.Init,                function() self.OnInit(self) end )
--  med:Subscribe("BaseService", EngineServiceEvents.Deinit,              function() self.OnDeinit(self) end )
--  med:Subscribe("BaseService", EngineServiceEvents.GraphicsLoad,        function() self.OnGraphicsLoad(self) end )
--  med:Subscribe("BaseService", EngineServiceEvents.GraphicsUnload,      function() self.OnGraphicsUnload(self) end )
--  med:Subscribe("BaseService", EngineServiceEvents.Restart,             function() self.OnRestart(self) end )
--  med:Subscribe("BaseService", EngineServiceEvents.Update,              function(curtime) self.OnUpdate(self,curtime) end )
--  med:Subscribe("BaseService", EngineServiceEvents.GroundEntityInteract,function(activator, target) self.OnGroundEntityInteract(self, activator, target) end )
--  med:Subscribe("BaseService", EngineServiceEvents.DungeonModeBegin,    function() self.OnDungeonModeBegin(self) end )
--  med:Subscribe("BaseService", EngineServiceEvents.DungeonModeEnd,      function() self.OnDungeonModeEnd(self) end )
--  med:Subscribe("BaseService", EngineServiceEvents.DungeonFloorPrepare, function(dungeonloc) self.OnDungeonFloorPrepare(self, dungeonloc) end )
--  med:Subscribe("BaseService", EngineServiceEvents.DungeonFloorBegin,   function(dungeonloc, entryinf) self.OnDungeonFloorBegin(self, dungeonloc, entrypos) end )
--  med:Subscribe("BaseService", EngineServiceEvents.DungeonFloorEnd,     function(dungeonloc, result) self.OnDungeonFloorEnd(self, dungeonloc, result) end )
--  med:Subscribe("BaseService", EngineServiceEvents.GroundModeBegin,     function() self.OnGroundModeBegin(self) end )
--  med:Subscribe("BaseService", EngineServiceEvents.GroundModeEnd,       function() self.OnGroundModeEnd(self) end )
--  med:Subscribe("BaseService", EngineServiceEvents.GroundMapPrepare,    function(mapid) self.OnGroundMapPrepare(self, mapid) end )
--  med:Subscribe("BaseService", EngineServiceEvents.GroundMapEnter,      function(mapid, entryinf) self.OnGroundMapEnter(self, mapid, entryinf) end )
--  med:Subscribe("BaseService", EngineServiceEvents.GroundMapExit,       function(mapid) self.OnGroundMapExit(self, mapid) end )
end

---Summary
-- un-subscribe to all channels this service subscribed to
function BaseService:UnSubscribe(med)
end

---Summary
-- The update method is run as a coroutine for each services.
function BaseService:Update(gtime)
--  while(true)
--    coroutine.yield()
--  end
end