--Import stuff from the submodules
require 'common'
require 'services.baseservice'
require 'services.chatter.topics'

--Declare class ChatterEngine
local ChatterEngine = Class('ChatterEngine', BaseService)

--[[---------------------------------------------------------------
    ChatterEngine:new()
      ChatterEngine class constructor
---------------------------------------------------------------]]
function ChatterEngine:initialize()
  BaseService.initialize(self)
  PrintInfo('ChatterEngine:initialize()')
end


--[[---------------------------------------------------------------
    ChatterEngine:Init()
      Called on initialization of the script engine by the game!
---------------------------------------------------------------]]
function ChatterEngine:OnInit()
  PrintInfo('ChatterEngine:OnInit()')
end

--[[---------------------------------------------------------------
    ChatterEngine:Deinit()
      Called on de-initialization of the script engine by the game!
---------------------------------------------------------------]]
function ChatterEngine:OnDeinit()
  PrintInfo('ChatterEngine:OnDeinit()')
end


function ChatterEngine:Subscribe(med)
  med:Subscribe("ChatterEngine", EngineServiceEvents.Init,   function() self.OnInit(self) end );
  med:Subscribe("ChatterEngine", EngineServiceEvents.Deinit, function() self.OnDeinit(self) end );
end

---Summary
-- un-subscribe to all channels this service subscribed to
function ChatterEngine:UnSubscribe(med)
end

--Add our service
SCRIPT:AddService("ChatterEngine", ChatterEngine:new())
return ChatterEngine