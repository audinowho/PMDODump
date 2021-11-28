--[[
    Example init.lua
    
  ****Each maps must have init.lua! ****
    
    Each map needs to have a init.lua, and a actions.lua file that defines its own instance of the GroundMap class.
    This will be used to determine how to run the map events and more!
    
    If the base implementation of GroundMap isn't enough, you can override any methods of the 
    GroundMap class in here as needed!
    
    You can add any number of scripts in the map folder as you want! Some special constants are available to fetch the current map's directory
    to load the scripts using a dofile or require function call.
    
    _SCRIPT_MAP_DIR = global containing the path from the Script root folder to the current folder
]]--
require('common')

local test_grounds = {}

local MapStrings = {}
--------------------------------------------------
-- Map Callbacks
--------------------------------------------------
function test_grounds.Init(map)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo("=>> Init_test_grounds <<=")
  MapStrings = COMMON.AutoLoadLocalizedStrings()
  COMMON.RespawnStarterPartner()
  
  local partner = CH('Partner')
  AI:SetCharacterAI(partner, "ai.ground_partner", CH('PLAYER'), partner.Position)
  partner.CollisionDisabled = true
	
  --Set Poochy AI
  local poochy = CH("Poochy")
  --Set the area to wander in
  local poochzone = {X = poochy.X - 100, Y = poochy.Y - 100, 
                     W = 200, H = 200};
  AI:SetCharacterAI(poochy,                                      --[[Entity that will use the AI]]--
                    "ai.ground_default",                         --[[Class path to the AI class to use]]--
                    RogueElements.Loc(poochzone.X, poochzone.Y), --[[Top left corner pos of the allowed idle wander area]]--
                    RogueElements.Loc(poochzone.W, poochzone.H), --[[Width and Height of the allowed idle wander area]]--
                    0.5,                                         --[[Wandering speed]]--
                    48,                                          --[[Min move distance for a single wander]]--
                    64,                                          --[[Max move distance for a single wander]]--
                    320,                                         --[[Min delay between idle actions]]--
                    620);                                        --[[Max delay between idle actions]]--
    


  local coord_table = {}
  coord_table[1] = { 200, 192, Direction.Down }
  coord_table[2] = { 200, 224, Direction.Down }
  coord_table[3] = { 200, 256, Direction.Down }
  
  --get assembly ready
  local assemblyCount = GAME:GetPlayerAssemblyCount()
  
  --Place player teammates
  for i = 1,5,1 do
    GROUND:RemoveCharacter("Assembly" .. tostring(i))
  end
  total = assemblyCount
  if total > 5 then
    total = 5
  end
  for i = 1,total,1 do
    p = GAME:GetPlayerAssemblyMember(i-1)
    GROUND:SpawnerSetSpawn("ASSEMBLY_" .. tostring(i),p)
    local chara = GROUND:SpawnerDoSpawn("ASSEMBLY_" .. tostring(i))
    --GROUND:GiveCharIdleChatter(chara)
  end
  
  --Spawn our spawner npcs
  GROUND:SpawnerDoSpawn('MerchantSpawner')
  
  GROUND:SpawnerDoSpawn('MerchantSpawner2')
end


--Called when the screen fades in as the player enters the map
function test_grounds.Enter(map)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo('Enter_test_grounds')
  SV.base_camp.ExpositionComplete = true
  SV.base_camp.FirstTalkComplete = true
  
  local caterQuest = SV.test_grounds.Missions["CaterQuest"]
  if caterQuest == nil or caterQuest.Complete == COMMON.MISSION_INCOMPLETE then
	GROUND:Hide("Caterpie")
  end
  local volmiseQuest = SV.test_grounds.Missions["VolmiseQuest"]
  if volmiseQuest == nil or volmiseQuest.Complete == COMMON.MISSION_INCOMPLETE then
	GROUND:Hide("Illumise")
  end
  
  GAME:SetTeamName(STRINGS:FormatKey("TEAM_NAME", "Guildmaster"))
  GAME:FadeIn(60)
  GAME:MoveCamera(0, 0, 60, true)
  UI:ResetSpeaker()
  --UI:WaitShowDialogue(STRINGS:Format("Congratulations on completing the toughest dungeon in the demo![pause=0] Enjoy the debug room!"))
  entrance = MRKR("entrance")
  PrintInfo("Entrance Loc: X"..tostring(entrance.Position.X).." Y"..tostring(entrance.Position.Y))
end

--Called constantly while the map is running
function test_grounds.Update(map, time)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
end

function test_grounds.Test_Action()
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo('Test_Action')

  UI:SetSpeaker(CH('PLAYER'))
  UI:WaitShowDialogue("So cool!")
  GROUND:Unhide("Illumise")
  GROUND:Unhide("Caterpie")
end

--------------------------------------------------
-- Objects Callbacks
--------------------------------------------------
function test_grounds.Sign1_Action(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo('Sign1_Action')
  UI:ResetSpeaker()
  UI:SetCenter(true)
  UI:WaitShowDialogue(MapStrings['Sign1_Action_Line0'])
  UI:WaitShowVoiceOver("Test Voice Over[pause=0]\n1 2 3[br]Extracted Graphics from\n\nPokémon Emerald\nPokémon Mystery Dungeon: Red Rescue Team\nPokémon Mystery Dungeon: Explorers of Sky\nPokémon Ranger: Guardian Signs[scroll]Test Voice Over[scroll]Test Voice Over", -1)
  GAME:WaitFrames(30)
  UI:SetAutoFinish(true)
  UI:WaitShowVoiceOver("Test Voice Over[pause=0]\n1 2 3[br]Test Voice Over\n4 5 6[scroll]Test Voice Over\n7 8 9", -1)
  
  TASK:WaitStartEntityTask(activator, function()
    SOUND:PlayBattleSE("EVT_Emote_Confused")
    GROUND:CharSetEmote(activator, 6, 1)
    GAME:WaitFrames(60)
    GROUND:MoveInDirection(activator, Direction.Down, 30, false, 2)
    GROUND:MoveInDirection(activator, Direction.DownLeft, 30, false, 2)
    GROUND:MoveInDirection(activator, Direction.DownRight, 30, false, 2)
    GROUND:MoveInDirection(activator, Direction.UpRight, 30, false, 2)
    GROUND:MoveInDirection(activator, Direction.UpLeft, 30, false, 2)
  end)

  UI:SetAutoFinish(false)
  TASK:WaitEntityTask(activator)
  UI:WaitShowDialogue("Ye,[pause=0] you just moved around by yourself.")
end


function test_grounds.Concurrent_Sequence(turnTime)
	local chara = CH('PLAYER')
  
  GROUND:CharSetAnim(chara, "None", true)
  GROUND:CharAnimateTurnTo(chara, Direction.Left, turnTime)
  GAME:WaitFrames(20)
  GROUND:CharAnimateTurn(chara, Direction.Right, turnTime, false)
  GAME:WaitFrames(20)
  GROUND:CharAnimateTurn(chara, Direction.Left, turnTime, true)
  GAME:WaitFrames(20)
  GROUND:CharAnimateTurn(chara, Direction.Right, turnTime, false)
  GAME:WaitFrames(20)
  GROUND:CharAnimateTurn(chara, Direction.Left, turnTime, true)
  GAME:WaitFrames(20)
  GROUND:CharAnimateTurn(chara, Direction.Right, turnTime, false)
  GAME:WaitFrames(20)
  GROUND:CharAnimateTurnTo(chara, Direction.Up, turnTime)
  GAME:WaitFrames(20)
  SOUND:PlayBattleSE("EVT_Emote_Confused")
  GROUND:CharSetEmote(chara, 6, 1)
  GAME:WaitFrames(20)
  GROUND:CharSetAnim(chara, "None", false)
end

function test_grounds.Sign2_Action(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  local chara = CH('PLAYER')
  PrintInfo('Sign2_Action')
  UI:ResetSpeaker()
  UI:SetAutoFinish(true)
  UI:WaitShowDialogue(MapStrings['Sign2_Action_Line0'])
  GAME:FadeOut(false, 30)
  GAME:MoveCamera(0, 120, 1, true)
  --perform this fade without waiting for its completion
  local coro1 = TASK:BranchCoroutine(GAME:_FadeIn(60))
  local coro2 = TASK:BranchCoroutine(function() test_grounds.Concurrent_Sequence(4) end)
  GAME:MoveCamera(0, 0, 60, true)
  --TODO: wait to join coroutines before giving control back to the player
  TASK:JoinCoroutines({coro1, coro2})
  
  
  coro1 = TASK:BranchCoroutine(test_grounds.Concurrent_BG)
  coro2 = TASK:BranchCoroutine(test_grounds.Concurrent_Title)
  TASK:JoinCoroutines({coro1, coro2})
end


function test_grounds.Concurrent_Title()

  UI:WaitShowTitle("AAA\nbbb\nccc", 60)
  GAME:WaitFrames(120)
  UI:WaitHideTitle(60)
end

function test_grounds.Concurrent_BG()

  UI:WaitShowBG("Sky", 1, 60)
  GAME:WaitFrames(120)
  UI:WaitHideBG(60)
end

ListType = luanet.import_type('System.Collections.Generic.List`1')
MenuTextChoiceType = luanet.import_type('RogueEssence.Menu.MenuTextChoice')

function test_grounds.Sign3_Action(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  local chara = CH('PLAYER')
  PrintInfo('Sign3_Action')
  UI:ResetSpeaker()
  UI:SetCenter(true, true)
  UI:WaitShowDialogue("Try to show a custom menu.[scroll]Are you ready?")
  
  local choices = LUA_ENGINE:MakeGenericType(ListType, { MenuTextChoiceType }, { })
  choices:Add(RogueEssence.Menu.MenuTextChoice("Choice 0", UI:GetChoiceAction(0)))
  choices:Add(RogueEssence.Menu.MenuTextChoice("Choice one", UI:GetChoiceAction(1)))
  choices:Add(RogueEssence.Menu.MenuTextChoice("Choice two", UI:GetChoiceAction(2)))
  choices:Add(RogueEssence.Menu.MenuTextChoice("Choice tres", UI:GetChoiceAction(3)))
  choices:Add(RogueEssence.Menu.MenuTextChoice("Choice quatro", UI:GetChoiceAction(4)))
  choices:Add(RogueEssence.Menu.MenuTextChoice("Choice V", UI:GetChoiceAction(5)))
  choices:Add(RogueEssence.Menu.MenuTextChoice("Choice VI", UI:GetChoiceAction(6)))
  local custom_menu = RogueEssence.Menu.CustomMultiPageMenu(RogueElements.Loc(8, 16), 144, "Custom Menu Test", choices:ToArray(), 5, 4, UI:GetChoiceAction(-1), nil)
  UI:ChooseCustomMenu(custom_menu)
  UI:WaitForChoice()
  local chres = UI:ChoiceResult()
  if chres > -1 then
	UI:WaitShowDialogue("You chose option " .. tostring(chres))
  else
	UI:WaitShowDialogue("You cancelled.")
  end
end

function test_grounds.Assembly_Action(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  UI:ResetSpeaker()
  SOUND:PlaySE("Menu/Skip")
  UI:AssemblyMenu()
  UI:WaitForChoice()
  result = UI:ChoiceResult()
  UI:WaitShowDialogue("Unlike the assembly objects in other maps, this one doesn't reload the map on team change.")
end

function test_grounds.SouthExit_Touch(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine

  local dungeon_entrances = { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40 }
  local ground_entrances = { {Flag=true,Zone=1,ID=1,Entry=3},
  {Flag=true,Zone=1,ID=3,Entry=0},
  {Flag=true,Zone=1,ID=4,Entry=0},
  {Flag=true,Zone=1,ID=5,Entry=0},
  {Flag=true,Zone=1,ID=6,Entry=0},
  {Flag=true,Zone=1,ID=7,Entry=0},
  {Flag=true,Zone=1,ID=8,Entry=0}}
  COMMON.ShowDestinationMenu(dungeon_entrances,ground_entrances)
  
end
--------------------------------------------------
-- Characters Callbacks
--------------------------------------------------
function test_grounds.Mew_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  local mew = CH('Mew')
  local player = CH('PLAYER')
  local state = {olddir = mew.CharDir}

  GROUND:CharTurnToChar(mew,player)
  GROUND:CharSetEmote(mew, -1, 0)

  UI:SetSpeaker(mew)
  UI:WaitShowTimedDialogue(STRINGS:Format(MapStrings['Mew_Action_Line0']), 120)
  GROUND:CharSetEmote(mew, 1, 0)

  local coro1 = TASK:BranchCoroutine(function() test_grounds.Walk_Sequence(mew) end)
  local coro2 = TASK:BranchCoroutine(function() test_grounds.Walk_Sequence(player) end)
  
  TASK:JoinCoroutines({coro1, coro2})

  CH('Mew').CharDir = state.olddir
  
  GROUND:CharHopAnim(CH('PLAYER'), "Hurt", 10, 10)
end

function test_grounds.Walk_Sequence(chara)
  
  GROUND:MoveInDirection(chara, Direction.Up, 20, false, 2)
  GAME:WaitFrames(60)
  GROUND:AnimateInDirection(chara, "Hurt", Direction.Down, Direction.Up, 20, 1, 5)
  GAME:WaitFrames(60)
  GROUND:MoveInDirection(chara, Direction.Down, 20, true, 7)
end


function test_grounds.Caterpie_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo('Caterpie_Action')
  local olddir = chara.CharDir
  GROUND:CharTurnToCharAnimated(chara, CH('PLAYER'), 4)
  
  UI:SetSpeaker(chara)
  UI:WaitShowDialogue("So cool!")
  
end


function test_grounds.Magnezone_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo('Magnezone_Action')
  GROUND:CharTurnToCharAnimated(chara, CH('PLAYER'), 4)
  
  UI:SetSpeaker(chara)
  -- check for quest presence
  local quest = SV.test_grounds.Missions["OutlawQuest"]
  if quest == nil then
    -- no outlaw quest? ask to start one
    UI:ChoiceMenuYesNo("No Outlaw mission detected. Do you want to start one?")
    UI:WaitForChoice()
    local chres = UI:ChoiceResult() 
    if chres then
	  -- Type 0 = Rescue
	  SV.test_grounds.Missions["OutlawQuest"] = { Complete = COMMON.MISSION_INCOMPLETE, Type = COMMON.MISSION_TYPE_OUTLAW, DestZone = 0, DestSegment = 4, DestFloor = 9, TargetSpecies = 447 }
      UI:WaitShowDialogue("You can find the perpetrator at Replay Test Zone 10F.  Good luck!")
    end
  else
    if quest.Complete == 2 then
	  -- there is an outlaw quest, and it has been completed- thank you note
	  UI:WaitShowDialogue("Outlaw mission state: Rewarded.  Thank you for apprehending Riolu!")
	elseif quest.Complete == 1 then
	  UI:WaitShowDialogue("Outlaw mission state: Complete.  Give a reward and mark mission as rewarded.")
	  quest.Complete = 2
	else
	  -- there is an outlaw quest, but it hasn't been completed?  ask to abandon
      UI:ChoiceMenuYesNo("Outlaw mission state: Incomplete.  Do you want to abandon the mission?")
      UI:WaitForChoice()
      local chres = UI:ChoiceResult() 
      if chres then
	    SV.test_grounds.Missions["OutlawQuest"] = nil
        UI:WaitShowDialogue("Outlaw mission removed.")
      end
	end
  end
end


function test_grounds.Butterfree_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo('Butterfree_Action')
  GROUND:CharTurnToCharAnimated(chara, CH('PLAYER'), 4)
  
  UI:SetSpeaker(chara)
  -- check for quest presence
  local quest = SV.test_grounds.Missions["CaterQuest"]
  if quest == nil then
    -- no caterpie quest? ask to start one
    UI:ChoiceMenuYesNo("No Caterpie mission detected. Do you want to start one?")
    UI:WaitForChoice()
    local chres = UI:ChoiceResult() 
    if chres then
	  -- Type 0 = Rescue
	  SV.test_grounds.Missions["CaterQuest"] = { Complete = COMMON.MISSION_INCOMPLETE, Type = COMMON.MISSION_TYPE_RESCUE, DestZone = 0, DestSegment = 4, DestFloor = 4, TargetSpecies = 10 }
      UI:WaitShowDialogue("You can find Caterpie at Replay Test Zone 5F.  Good luck!")
    end
  else
    if quest.Complete == 2 then
	  -- there is a caterpie quest, and it has been completed- thank you note
	  UI:WaitShowDialogue("Caterpie mission state: Rewarded.  Thank you for rescuing Caterpie!")
	elseif quest.Complete == 1 then
	  UI:WaitShowDialogue("Caterpie mission state: Complete.  Give a reward and mark mission as rewarded.")
	  quest.Complete = 2
	else
	  -- there is a caterpie quest, but it hasn't been completed?  ask to abandon
      UI:ChoiceMenuYesNo("Caterpie mission state: Incomplete.  Do you want to abandon the mission?")
      UI:WaitForChoice()
      local chres = UI:ChoiceResult() 
      if chres then
	    SV.test_grounds.Missions["CaterQuest"] = nil
        UI:WaitShowDialogue("Caterpie mission removed.")
      end
	end
  end
end


function test_grounds.Illumise_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo('Illumise_Action')
  local olddir = chara.CharDir
  GROUND:CharTurnToCharAnimated(chara, CH('PLAYER'), 4)
  
  UI:SetSpeaker(chara)
  UI:WaitShowDialogue("Thank you for rescuing me!")
  
end

function test_grounds.Volbeat_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo('Volbeat_Action')
  GROUND:CharTurnToCharAnimated(chara, CH('PLAYER'), 4)
  
  UI:SetSpeaker(chara)
  -- check for quest presence
  local quest = SV.test_grounds.Missions["VolmiseQuest"]
  if quest == nil then
    -- no caterpie quest? ask to start one
    UI:ChoiceMenuYesNo("No Volmise mission detected. Do you want to start one?")
    UI:WaitForChoice()
    local chres = UI:ChoiceResult() 
    if chres then
	  -- Type 1 = Escort
	  SV.test_grounds.Missions["VolmiseQuest"] = { Complete = COMMON.MISSION_INCOMPLETE, Type = COMMON.MISSION_TYPE_ESCORT, DestZone = 0, DestSegment = 4, DestFloor = 3, TargetSpecies = 314, EscortSpecies = 313 }
      UI:WaitShowDialogue("You can find Illumise at Replay Test Zone 4F.  I'll join you when you enter!")
    end
  else
    if quest.Complete == 2 then
	  -- there is a caterpie quest, and it has been completed- thank you note
	  UI:WaitShowDialogue("Volmise mission state: Rewarded.  Thank you for rescuing Illumise!")
	elseif quest.Complete == 1 then
	  UI:WaitShowDialogue("Volmise mission state: Complete.  Give a reward and mark mission as rewarded.")
	  quest.Complete = 2
	else
	  -- there is a caterpie quest, but it hasn't been completed?  ask to abandon
      UI:ChoiceMenuYesNo("Volmise mission state: Incomplete.  Do you want to abandon the mission?")
      UI:WaitForChoice()
      local chres = UI:ChoiceResult() 
      if chres then
	    SV.test_grounds.Missions["VolmiseQuest"] = nil
        UI:WaitShowDialogue("Volmise mission removed.")
      end
	end
  end
end


function test_grounds.Hungrybox_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo('Hungrybox_Action')
  local player = CH('PLAYER')
  local hbox = chara
  local olddir = hbox.CharDir
  GROUND:Hide("PLAYER")
  GROUND:CharTurnToCharAnimated(hbox, player, 4)
  UI:SetSpeaker(hbox)
  UI:TextDialogue(STRINGS:Format(MapStrings['Hungrybox_Action_Line0']), 120)
  UI:WaitDialog()
  GROUND:CharAnimateTurnTo(hbox, olddir, 4)
  chara.CollisionDisabled = true
  GROUND:Unhide("PLAYER")
end


function test_grounds.Poochy_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintSVAndStrings()
  
  local olddir = chara.CharDir
  GROUND:CharTurnToCharAnimated(chara, activator, 4)
  UI:SetSpeaker(chara)
  
  if SV.base_camp.AcceptedPooch then
    --If we already talked with poochy and got him to set the ScriptVar
    UI:TextDialogue(STRINGS:Format(MapStrings['Pooch_Action_Line3']))
    UI:WaitDialog()
  else
    --If we haven't gotten poochy to set the script var
    
    if not SV.base_camp.SpokeToPooch then
      --If we never spoke to poochy before
      UI:TextDialogue(STRINGS:Format(MapStrings['Pooch_Action_Line0A']))
      UI:WaitDialog()
      SV.base_camp.SpokeToPooch = true
    else
      --If we already spoke to poochy before, but didn't get him to set the script var
      UI:TextDialogue(STRINGS:Format(MapStrings['Pooch_Action_Line0B']))
      UI:WaitDialog()
    end
    
    UI:ChoiceMenuYesNo(STRINGS:Format(MapStrings['Pooch_Action_Question1']))
    UI:WaitForChoice()
    local ch = UI:ChoiceResult()
    
    if ch then
      --The player told poochy to set the script var
      SV.base_camp.AcceptedPooch = true
      SOUND:PlayFanfare("Fanfare/MissionClear")
      SOUND:WaitFanfare()
      UI:TextDialogue(STRINGS:Format(MapStrings['Pooch_Action_Line2A']))
    else
      --The player told poochy NOT to set the script var
      UI:TextDialogue(STRINGS:Format(MapStrings['Pooch_Action_Line2B']))
    end
    
    UI:WaitDialog()
    
  end -- if SV.AcceptedPooch
  UI:ResetSpeaker()
  GROUND:CharAnimateTurnTo(chara, olddir, 4)
end

--------------------------------------------------
-- Spawners Callbacks
--------------------------------------------------

--[[
--]]
function test_grounds.MerchantSpawner_EntSpawned(spawner, spawnedent)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo("test_grounds.MerchantSpawner_EntSpawned()!!")
end

--[[
    
--]]
function test_grounds.Merchant_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  UI:ResetSpeaker()
  
  local olddir = chara.CharDir
  GROUND:CharTurnToCharAnimated(chara, activator, 4)
  UI:SetSpeaker(chara)
  
  UI:TextDialogue(STRINGS:Format(MapStrings['Merchant_Greet']))
  UI:WaitDialog()
  GROUND:CharAnimateTurnTo(chara, olddir, 4)
end

--[[
--]]
function test_grounds.MerchantSpawner2_EntSpawned(spawner, spawnedent)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo("test_grounds.MerchantSpawner_EntSpawned()!!")
end

--[[
    
--]]
function test_grounds.Merchant2_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  UI:ResetSpeaker()
  
  local olddir = chara.CharDir
  GROUND:CharTurnToCharAnimated(chara, activator, 4)
  UI:SetSpeaker(chara)
  
  UI:TextDialogue(STRINGS:Format("HELLO!"))
  UI:WaitDialog()
  GROUND:CharAnimateTurnTo(chara, olddir, 4)
end

function test_grounds.Teammate1_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  UI:SetSpeaker("", false, chara.CurrentForm.Species, chara.CurrentForm.Form, chara.CurrentForm.Skin, chara.CurrentForm.Gender)
  
  local tbl = LTBL(chara)
  
  if tbl.TalkAmount == nil then
    UI:WaitShowDialogue("I have script vars specific to me.")
    UI:WaitShowDialogue("Switch with me by pressing 2 and 1.")
    UI:WaitShowDialogue("We will remember how many times we've been talked to.")
	tbl.TalkAmount = 1
  else
	tbl.TalkAmount = tbl.TalkAmount + 1
  end
  UI:WaitShowDialogue("You've talked to me "..tostring(tbl.TalkAmount).." times.")
end

function test_grounds.Teammate2_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  COMMON.GroundInteract(activator, chara, true)
end

function test_grounds.Teammate3_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  COMMON.GroundInteract(activator, chara, true)
end

return test_grounds