--[[
    Example Service
    
    This is an example to demonstrate how to use the BaseService class to implement a game service.
    
    **NOTE:** After declaring you service, you have to include your package inside the main.lua file!
]]--
require 'common'
require 'services.baseservice'

--Declare class DebugTools
local DebugTools = Class('DebugTools', BaseService)

--[[---------------------------------------------------------------
    DebugTools:initialize()
      DebugTools class constructor
---------------------------------------------------------------]]
function DebugTools:initialize()
  BaseService.initialize(self)
  PrintInfo('DebugTools:initialize()')
end

--[[---------------------------------------------------------------
    DebugTools:__gc()
      DebugTools class gc method
      Essentially called when the garbage collector collects the service.
  ---------------------------------------------------------------]]
function DebugTools:__gc()
  PrintInfo('*****************DebugTools:__gc()')
end

--[[---------------------------------------------------------------
    DebugTools:OnInit()
      Called on initialization of the script engine by the game!
---------------------------------------------------------------]]
function DebugTools:OnInit()
  assert(self, 'DebugTools:OnInit() : self is null!')
  PrintInfo("\n<!> ExampleSvc: Init..")
end

--[[---------------------------------------------------------------
    DebugTools:OnDeinit()
      Called on de-initialization of the script engine by the game!
---------------------------------------------------------------]]
function DebugTools:OnDeinit()
  assert(self, 'DebugTools:OnDeinit() : self is null!')
  PrintInfo("\n<!> ExampleSvc: Deinit..")
end

--[[---------------------------------------------------------------
    DebugTools:OnNewGame()
      When a new save file is loaded this is called!
---------------------------------------------------------------]]
function DebugTools:OnNewGame()
  assert(self, 'DebugTools:OnNewGame() : self is null!')
  
  for ii = 1, _DATA.StartChars.Count, 1 do
    _DATA.Save:RogueUnlockMonster(_DATA.StartChars[ii-1].Item1.Species)
  end
  
  if _DATA.Save.ActiveTeam.Players.Count > 0 then
    local talk_evt = RogueEssence.Dungeon.BattleScriptEvent("AllyInteract")
    _DATA.Save.ActiveTeam.Players[0].ActionEvents:Add(talk_evt)
	_DATA.Save:RegisterMonster(_DATA.Save.ActiveTeam.Players[0].BaseForm.Species)
	
	_DATA.Save.ActiveTeam:SetRank(1)
	if not GAME:InRogueMode() then
      _DATA.Save.ActiveTeam.Bank = 1000
	end
  else
    PrintInfo("\n<!> ExampleSvc: Preparing debug save file")
    _DATA.Save.ActiveTeam:SetRank(1)
    _DATA.Save.ActiveTeam.Name = "Debug"
    _DATA.Save.ActiveTeam.Money = 1000
    _DATA.Save.ActiveTeam.Bank = 1000000
  
    local mon_id = RogueEssence.Dungeon.MonsterID(1, 0, 0, Gender.Male)
    _DATA.Save.ActiveTeam.Players:Add(_DATA.Save.ActiveTeam:CreatePlayer(_DATA.Save.Rand, mon_id, 50, -1, 0))
    mon_id = RogueEssence.Dungeon.MonsterID(4, 0, 0, Gender.Male)
    _DATA.Save.ActiveTeam.Players:Add(_DATA.Save.ActiveTeam:CreatePlayer(_DATA.Save.Rand, mon_id, 50, -1, 0))
    mon_id = RogueEssence.Dungeon.MonsterID(7, 0, 0, Gender.Male)
    _DATA.Save.ActiveTeam.Players:Add(_DATA.Save.ActiveTeam:CreatePlayer(_DATA.Save.Rand, mon_id, 50, -1, 0))
	
	
    local talk_evt = RogueEssence.Dungeon.BattleScriptEvent("AllyInteract")
    _DATA.Save.ActiveTeam.Players[0].ActionEvents:Add(talk_evt)
	_DATA.Save.ActiveTeam:SetRank(1)
	talk_evt = RogueEssence.Dungeon.BattleScriptEvent("AllyInteract")
    _DATA.Save.ActiveTeam.Players[1].ActionEvents:Add(talk_evt)
	talk_evt = RogueEssence.Dungeon.BattleScriptEvent("AllyInteract")
    _DATA.Save.ActiveTeam.Players[2].ActionEvents:Add(talk_evt)
	
    _DATA.Save.ActiveTeam.Leader.IsFounder = true
	
	_DATA.Save:UpdateTeamProfile(true)
  
    for ii = 1, _DATA.Save.DungeonUnlocks.Length, 1 do
      _DATA.Save.DungeonUnlocks[ii-1] = RogueEssence.Data.GameProgress.UnlockState.Discovered
    end
  
    SV.base_camp.ExpositionComplete = true
    SV.base_camp.IntroComplete = true
  end
end

---Summary
-- Subscribe to all channels this service wants callbacks from
function DebugTools:Subscribe(med)
  med:Subscribe("DebugTools", EngineServiceEvents.Init,                function() self.OnInit(self) end )
  med:Subscribe("DebugTools", EngineServiceEvents.Deinit,              function() self.OnDeinit(self) end )
  med:Subscribe("DebugTools", EngineServiceEvents.NewGame,        function() self.OnNewGame(self) end )
--  med:Subscribe("DebugTools", EngineServiceEvents.GraphicsUnload,      function() self.OnGraphicsUnload(self) end )
--  med:Subscribe("DebugTools", EngineServiceEvents.Restart,             function() self.OnRestart(self) end )
end

---Summary
-- un-subscribe to all channels this service subscribed to
function DebugTools:UnSubscribe(med)
end

---Summary
-- The update method is run as a coroutine for each services.
function DebugTools:Update(gtime)
--  while(true)
--    coroutine.yield()
--  end
end

--Add our service
SCRIPT:AddService("DebugTools", DebugTools:new())
return DebugTools