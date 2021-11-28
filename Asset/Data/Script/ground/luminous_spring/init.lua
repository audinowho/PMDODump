require 'common'

local luminous_spring = {}
local MapStrings = {}
--------------------------------------------------
-- Map Callbacks
--------------------------------------------------
function luminous_spring.Init(map)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  PrintInfo("=>> Init_luminous_spring")
  MapStrings = COMMON.AutoLoadLocalizedStrings()
  COMMON.RespawnAllies()
end

function luminous_spring.Enter(map)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine

  SV.luminous_spring.Returning = false
  GAME:FadeIn(20)
end

function luminous_spring.Update(map, time)
end

--------------------------------------------------
-- Map Begin Functions
--------------------------------------------------

--------------------------------------------------
-- Objects Callbacks
--------------------------------------------------

function luminous_spring.South_Exit_Touch(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  GAME:FadeOut(false, 20)
  GAME:EnterGroundMap("base_camp_2", "entrance_north")
end

function luminous_spring.Spring_Touch(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
	UI:ResetSpeaker()
	
	local state = 0
	local repeated = false
	local member = nil
	local evo = nil
	local player = CH('PLAYER')
	
	GAME:CutsceneMode(true)
	GAME:MoveCamera(300, 152, 90, false)
	GROUND:TeleportTo(player, 292, 312, Direction.Down)
	
	if not SV.luminous_spring.Returning then
		UI:WaitShowDialogue(STRINGS:Format(MapStrings['Evo_Intro_1']))
		UI:WaitShowDialogue(STRINGS:Format(MapStrings['Evo_Intro_2']))
	end
	SV.luminous_spring.Returning = true
	while state > -1 do
		if state == 0 then
			local evo_choices = {STRINGS:Format(MapStrings['Evo_Option_Evolve']),
			STRINGS:FormatKey("MENU_INFO"),
			STRINGS:FormatKey("MENU_EXIT")}
			UI:BeginChoiceMenu(STRINGS:Format(MapStrings['Evo_Ask']), evo_choices, 1, 3)
			UI:WaitForChoice()
			local result = UI:ChoiceResult()
			repeated = true
			if result == 1 then
				state = 1
			elseif result == 2 then
				UI:WaitShowDialogue(STRINGS:Format(MapStrings['Evo_Info_001']))
				UI:WaitShowDialogue(STRINGS:Format(MapStrings['Evo_Info_002']))
				UI:WaitShowDialogue(STRINGS:Format(MapStrings['Evo_Info_003']))
				UI:WaitShowDialogue(STRINGS:Format(MapStrings['Evo_Info_004']))
				UI:WaitShowDialogue(STRINGS:Format(MapStrings['Evo_Info_005']))
				UI:WaitShowDialogue(STRINGS:Format(MapStrings['Evo_Info_006']))
				UI:WaitShowDialogue(STRINGS:Format(MapStrings['Evo_Info_007']))
			else
				UI:WaitShowDialogue(STRINGS:Format(MapStrings['Evo_End']))
				state = -1
			end
		elseif state == 1 then
			UI:WaitShowDialogue(STRINGS:Format(MapStrings['Evo_Ask_Who']))
			UI:ShowPromoteMenu()
			UI:WaitForChoice()
			local result = UI:ChoiceResult()
			if result > -1 then
				state = 2
				member = GAME:GetPlayerPartyMember(result)--GAME:GetPlayerAssemblyMember(result)
			else
				state = 0
			end
		elseif state == 2 then
			if not GAME:CanPromote(member) then
				UI:WaitShowDialogue(STRINGS:Format(MapStrings['Evo_None'], member:GetDisplayName(true)))
				state = 1
			else
				local branches = GAME:GetAvailablePromotions(member, 349)
				if #branches == 0 then
					UI:WaitShowDialogue(STRINGS:Format(MapStrings['Evo_None_Now'], member:GetDisplayName(true)))
					state = 1
				elseif #branches == 1 then
					local branch = branches[1]
					local evo_item = -1
					for detail_idx = 0, branch.Details.Count  - 1 do
						local detail = branch.Details[detail_idx]
						if detail.GiveItem > -1 then
							evo_item = detail.GiveItem
							break
						end
					end
					-- harmony scarf hack-in
					if member.EquippedItem.ID == 349 then
						evo_item = 349
					end
					local mon = RogueEssence.Data.DataManager.Instance:GetMonster(branch.Result)
					if evo_item > -1 then
						local item = RogueEssence.Data.DataManager.Instance:GetItem(evo_item)
						UI:ChoiceMenuYesNo(STRINGS:Format(MapStrings['Evo_Confirm_Item'], member:GetDisplayName(true), item:GetIconName(), mon:GetColoredName()), false)
					else
						UI:ChoiceMenuYesNo(STRINGS:Format(MapStrings['Evo_Confirm'], member:GetDisplayName(true), mon:GetColoredName()), false)
					end
					UI:WaitForChoice()
					local result = UI:ChoiceResult()
					if result then
						evo = branch
						state = 3
					else
						state = 1
					end
				else
					local evo_names = {}
					for branch_idx = 1, #branches do
						local mon = RogueEssence.Data.DataManager.Instance:GetMonster(branches[branch_idx].Result)
						table.insert(evo_names, mon:GetColoredName())
					end
					table.insert(evo_names, STRINGS:FormatKey("MENU_CANCEL"))
					UI:BeginChoiceMenu(STRINGS:Format(MapStrings['Evo_Choice'], member:GetDisplayName(true)), evo_names, 1, #evo_names)
					UI:WaitForChoice()
					local result = UI:ChoiceResult()
					if result < #evo_names then
						evo = branches[result]
						state = 3
					else
						state = 1
					end
				end
			end
		elseif state == 3 then
			--execute evolution
			local mon = RogueEssence.Data.DataManager.Instance:GetMonster(evo.Result)
			
			GROUND:SpawnerSetSpawn("EVO_SUBJECT",member)
			local subject = GROUND:SpawnerDoSpawn("EVO_SUBJECT")
			
			GROUND:MoveInDirection(subject, Direction.Up, 60, false, 2)
			GROUND:EntTurn(subject, Direction.Down)
			
			UI:WaitShowDialogue(STRINGS:Format(MapStrings['Evo_Begin']))
			
			SOUND:PlayBattleSE("EVT_Evolution_Start")
			GAME:FadeOut(true, 20)
			
			local pastName = member:GetDisplayName(true)
			GAME:PromoteCharacter(member, evo, 349)
			COMMON.RespawnAllies()
			GROUND:RemoveCharacter("EvoSubject")
			--GROUND:SpawnerSetSpawn("EVO_SUBJECT",member)
			subject = GROUND:SpawnerDoSpawn("EVO_SUBJECT")
			GROUND:TeleportTo(subject, 292, 192, Direction.Down)
			
			GAME:WaitFrames(30)
			
			SOUND:PlayBattleSE("EVT_Title_Intro")
			GAME:FadeIn(20)
			SOUND:PlayFanfare("Fanfare/Promotion")
			
			
			UI:WaitShowDialogue(STRINGS:Format(MapStrings['Evo_Complete'], pastName, mon:GetColoredName()))
			GAME:CheckLevelSkills(member, 0)
			if member.Level > 1 then
				GAME:CheckLevelSkills(member, member.Level-1)
			end
			
			GROUND:MoveInDirection(subject, Direction.Down, 60, false, 2)
			
			GROUND:RemoveCharacter("EvoSubject")
			
			state = 0
		end
	end
	
	GAME:MoveCamera(0, 0, 90, true)
	GAME:CutsceneMode(false)
end

function luminous_spring.Assembly_Action(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  UI:ResetSpeaker()
  COMMON.ShowTeamAssemblyMenu(obj, COMMON.RespawnAllies)
end

function luminous_spring.Storage_Action(obj, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  COMMON:ShowTeamStorageMenu()
end


function luminous_spring.Teammate1_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  COMMON.GroundInteract(activator, chara, true)
end

function luminous_spring.Teammate2_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  COMMON.GroundInteract(activator, chara, true)
end

function luminous_spring.Teammate3_Action(chara, activator)
  DEBUG.EnableDbgCoro() --Enable debugging this coroutine
  COMMON.GroundInteract(activator, chara, true)
end


return luminous_spring