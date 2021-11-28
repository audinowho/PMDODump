require 'common'

local guildmaster_summit = {}
local MapStrings = {}
--------------------------------------------------
-- Map Callbacks
--------------------------------------------------
function guildmaster_summit.Init(map)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo("=>> Init_guildmaster_summit")
  MapStrings = COMMON.AutoLoadLocalizedStrings()
  COMMON.RespawnAllies()
end

function guildmaster_summit.Enter(map)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine

  if SV.guildmaster_summit.ExpositionComplete then
    GROUND:Hide("Xatu")
    GROUND:Hide("Lucario")
    GROUND:Hide("Wigglytuff")
    GAME:FadeIn(20)
  else
    if not SV.guildmaster_summit.BattleComplete then
      SV.base_camp.ExpositionComplete = true
      SV.base_camp.FirstTalkComplete = true
      guildmaster_summit.PreBattle()
    else
      guildmaster_summit.PostBattle()
      SV.guildmaster_summit.ExpositionComplete = true
    end
  end
  
  
end

function guildmaster_summit.Update(map, time)
end

--------------------------------------------------
-- Map Begin Functions
--------------------------------------------------
function guildmaster_summit.PreBattle()
  -- play cutscene
  GAME:CutsceneMode(true)
  UI:WaitShowTitle(GAME:GetCurrentGround().Name:ToLocal(), 20)
  GAME:WaitFrames(30)
  UI:WaitHideTitle(20)
  -- move characters to beside the player
  local player = CH('PLAYER')
  local team1 = CH('Teammate1')
  local team2 = CH('Teammate2')
  local team3 = CH('Teammate3')
  GROUND:TeleportTo(player, 196, 196, Direction.Up)
  if team1 ~= nil then
    GROUND:TeleportTo(team1, 168, 196, Direction.Up)
  end
  if team2 ~= nil then
    GROUND:TeleportTo(team2, 224, 196, Direction.Up)
  end
  if team3 ~= nil then
    GROUND:TeleportTo(team3, 196, 224, Direction.Up)
  end
  
  
  GAME:FadeIn(20)
  local xatu = CH('Xatu')
  local lucario = CH('Lucario')
  local wigglytuff = CH('Wigglytuff')
  -- trigger the battle and set a variable indicating its triggering
  UI:SetSpeaker("*", true, -1)
  UI:WaitShowDialogue(STRINGS:Format(MapStrings['Expo_Cutscene_Line_001']))
  GAME:WaitFrames(10)
  GAME:MoveCamera(0, -72, 60, true)
  GAME:WaitFrames(10)
  GROUND:CharAnimateTurnTo(xatu, Direction.Down, 4)
  GROUND:CharAnimateTurnTo(lucario, Direction.Down, 4)
  GROUND:CharAnimateTurnTo(wigglytuff, Direction.Down, 4)
  GAME:WaitFrames(20)
  UI:SetSpeaker(xatu)
  UI:WaitShowDialogue(STRINGS:Format(MapStrings['Expo_Cutscene_Line_002']))
  
  if GAME:GetTeamName() == "" then
    UI:WaitShowDialogue(STRINGS:Format(MapStrings['Expo_Cutscene_Line_004']))
  else
    UI:WaitShowDialogue(STRINGS:Format(MapStrings['Expo_Cutscene_Line_003'], GAME:GetTeamName()))
  end
  GAME:WaitFrames(10)
  UI:WaitShowDialogue(STRINGS:Format(MapStrings['Expo_Cutscene_Line_005']))
  GAME:WaitFrames(10)
  UI:SetSpeaker(lucario)
  UI:WaitShowDialogue(STRINGS:Format(MapStrings['Expo_Cutscene_Line_006']))
  GAME:WaitFrames(10)
  UI:SetSpeaker(wigglytuff)
  UI:WaitShowDialogue(STRINGS:Format(MapStrings['Expo_Cutscene_Line_007']))
  SOUND:FadeOutBGM()
  GAME:WaitFrames(10)
  
  if GAME:GetTeamName() == "" then
    
    UI:SetSpeaker(lucario)
    UI:WaitShowDialogue(STRINGS:Format(MapStrings['Expo_Cutscene_Line_008'], GAME:GetTeamName()))
    UI:SetSpeaker(wigglytuff)
    UI:SetSpeakerEmotion("Stunned")
    UI:WaitShowDialogue(STRINGS:Format(MapStrings['Expo_Cutscene_Line_009']))
    GAME:WaitFrames(10)
    GROUND:CharAnimateTurnTo(xatu, Direction.Right, 4)
    GAME:WaitFrames(50)
    GROUND:CharAnimateTurnTo(xatu, Direction.Down, 4)
	UI:SetSpeaker(xatu)
	UI:WaitShowDialogue(STRINGS:Format(MapStrings['Expo_Cutscene_Line_010']))
	
    local ch = false
    local name = ""
    while not ch do
      UI:NameMenu(STRINGS:FormatKey("INPUT_TEAM_TITLE"), STRINGS:Format(""))
      UI:WaitForChoice()
      name = UI:ChoiceResult()
    
      UI:ChoiceMenuYesNo(STRINGS:Format(MapStrings['Expo_Cutscene_Line_011'], name), true)
      UI:WaitForChoice()
      ch = UI:ChoiceResult()
    end
    GAME:SetTeamName(name)
    
  end
  
  
  UI:SetSpeaker(xatu)
  UI:WaitShowDialogue(STRINGS:Format(MapStrings['Expo_Cutscene_Line_012']))
  UI:WaitShowDialogue(STRINGS:Format(MapStrings['Expo_Cutscene_Line_013']))
  
  SOUND:PlayBGM("C06. Final Battle.ogg", false)
  UI:WaitShowDialogue(STRINGS:Format(MapStrings['Expo_Cutscene_Line_014'], GAME:GetTeamName()))
  GAME:WaitFrames(10)
  SOUND:PlayBattleSE("DUN_Bird_Caw")
  GROUND:CharPoseAnim(xatu, "Shoot")--110
  GAME:WaitFrames(10)
  GROUND:CharPoseAnim(lucario, "Chop")--100
  GROUND:CharPoseAnim(wigglytuff, "Shoot")--100
  
  GAME:WaitFrames(10)
  SOUND:PlayBattleSE("EVT_Fade_White")
  GAME:FadeOut(true, 80)
  GAME:CutsceneMode(false)
  
  GAME:ContinueDungeon(19, 1,0, 0)
end

function guildmaster_summit.PostBattle()
  --warp the player back to the start, set exposition complete, save and continue, and play credits.
  
  GAME:CutsceneMode(true)
  
  
  local player = CH('PLAYER')
  local team1 = CH('Teammate1')
  local team2 = CH('Teammate2')
  local team3 = CH('Teammate3')
  GROUND:TeleportTo(player, 196, 196, Direction.Up)
  if team1 ~= nil then
    GROUND:TeleportTo(team1, 168, 196, Direction.Up)
  end
  if team2 ~= nil then
    GROUND:TeleportTo(team2, 224, 196, Direction.Up)
  end
  if team3 ~= nil then
    GROUND:TeleportTo(team3, 196, 224, Direction.Up)
  end
  
  local xatu = CH('Xatu')
  local lucario = CH('Lucario')
  local wigglytuff = CH('Wigglytuff')
  
  GROUND:EntTurn(xatu, Direction.Down)
  GROUND:EntTurn(lucario, Direction.Down)
  GROUND:EntTurn(wigglytuff, Direction.Down)
  
  SOUND:PlayBGM("A08. Aftermath.ogg", false)
  
  GAME:MoveCamera(0, -72, 0, true)
  GAME:FadeIn(40)
  

  UI:SetSpeaker(xatu)
  UI:WaitShowDialogue(STRINGS:Format(MapStrings['Ending_Cutscene_Line_001']))
  UI:WaitShowDialogue(STRINGS:Format(MapStrings['Ending_Cutscene_Line_002']))
  GAME:WaitFrames(10)
  UI:SetSpeaker(lucario)
  UI:WaitShowDialogue(STRINGS:Format(MapStrings['Ending_Cutscene_Line_003']))
  GAME:WaitFrames(10)
  UI:SetSpeaker(wigglytuff)
  UI:WaitShowDialogue(STRINGS:Format(MapStrings['Ending_Cutscene_Line_004']))

  UI:SetSpeaker(lucario)
  UI:WaitShowDialogue(STRINGS:Format(MapStrings['Ending_Cutscene_Line_005']))
  UI:WaitShowDialogue(STRINGS:Format(MapStrings['Ending_Cutscene_Line_006']))
  GAME:WaitFrames(10)
  UI:SetSpeaker(xatu)
  UI:WaitShowDialogue(STRINGS:Format(MapStrings['Ending_Cutscene_Line_007'], GAME:GetTeamName()))

  SOUND:PlayBattleSE("DUN_Bird_Caw")
  GROUND:CharPoseAnim(xatu, "Shoot")--200
  GAME:WaitFrames(80)

  GAME:FadeOut(false, 40)
  GAME:CutsceneMode(true)

  if GAME:InRogueMode() then

	  GAME:WaitFrames(60)
	  UI:WaitShowVoiceOver(STRINGS:Format(MapStrings['Ending_Cutscene_Line_008'], GAME:GetTeamName()), -1)
	  GAME:WaitFrames(20);
	  UI:WaitShowVoiceOver(STRINGS:Format(MapStrings['Ending_Cutscene_Line_009']), -1)
	  GAME:WaitFrames(20);
	  UI:WaitShowVoiceOver(STRINGS:Format(MapStrings['Ending_Cutscene_Line_010']), -1)
	  GAME:WaitFrames(20);
	  UI:WaitShowVoiceOver(STRINGS:Format(MapStrings['Ending_Cutscene_Line_011']), -1)
	  GAME:WaitFrames(20);
	  UI:WaitShowVoiceOver(STRINGS:Format(MapStrings['Ending_Cutscene_Line_012']), -1)
	  GAME:AddToPlayerMoneyBank(100000)
  else
  
	  GAME:WaitFrames(180)
	  UI:WaitShowTitle(STRINGS:Format(MapStrings['Credits_Line_001']), 60)
	  GAME:WaitFrames(180)
	  UI:WaitHideTitle(60)
	  GAME:WaitFrames(60)
	  UI:WaitShowVoiceOver(STRINGS:Format(MapStrings['Credits_Line_002']), 210)
	  GAME:WaitFrames(60)
	  UI:WaitShowVoiceOver(STRINGS:Format(MapStrings['Credits_Line_003']), 210)
	  GAME:WaitFrames(60)
	  UI:WaitShowVoiceOver(STRINGS:Format(MapStrings['Credits_Line_004']), 210)
	  GAME:WaitFrames(60)
	  UI:WaitShowVoiceOver(STRINGS:Format(MapStrings['Credits_Line_005']), 210)
	  GAME:WaitFrames(60)
	  UI:WaitShowVoiceOver(STRINGS:Format(MapStrings['Credits_Line_006']), 210)
	  GAME:WaitFrames(60)
	  UI:WaitShowVoiceOver(STRINGS:Format(MapStrings['Credits_Line_007']), 210)
	  GAME:WaitFrames(60)
	  UI:WaitShowVoiceOver(STRINGS:Format(MapStrings['Credits_Line_008']), 210)
	  GAME:WaitFrames(210)

	  UI:WaitShowVoiceOver(STRINGS:Format(MapStrings['Credits_Line_009']), 240)
	  
  end
  
  SOUND:FadeOutBGM()
  GAME:WaitFrames(160);

  COMMON.EndDayCycle()
  GAME:EndDungeonRun(RogueEssence.Data.GameProgress.ResultType.Cleared, 1, -1, 1, 0, true, false)
  GAME:RestartToTitle()
end

--------------------------------------------------
-- Objects Callbacks
--------------------------------------------------

function guildmaster_summit.South_Exit_Touch(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  
  local dungeon_entrances = { }
  local ground_entrances = {{Flag=true,Zone=1,ID=1,Entry=3},
  {Flag=SV.forest_camp.ExpositionComplete,Zone=1,ID=3,Entry=2},
  {Flag=SV.cliff_camp.ExpositionComplete,Zone=1,ID=4,Entry=2},
  {Flag=SV.canyon_camp.ExpositionComplete,Zone=1,ID=5,Entry=2},
  {Flag=SV.rest_stop.ExpositionComplete,Zone=1,ID=6,Entry=2},
  {Flag=SV.final_stop.ExpositionComplete,Zone=1,ID=7,Entry=2}}
  COMMON.ShowDestinationMenu(dungeon_entrances,ground_entrances)
end


function guildmaster_summit.Summit_Action(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  UI:ResetSpeaker()
  UI:WaitShowDialogue(STRINGS:Format("Witness it, the rising of the sun."))
end


function guildmaster_summit.Teammate1_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  COMMON.GroundInteract(activator, chara, true)
end

function guildmaster_summit.Teammate2_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  COMMON.GroundInteract(activator, chara, true)
end

function guildmaster_summit.Teammate3_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  COMMON.GroundInteract(activator, chara, true)
end


return guildmaster_summit