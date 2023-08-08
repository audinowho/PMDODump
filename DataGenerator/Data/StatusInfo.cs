using System;
using System.Collections.Generic;
using RogueEssence.Dungeon;
using RogueEssence.Content;
using RogueEssence;
using RogueEssence.Data;
using PMDC.Dungeon;
using PMDC;
using PMDC.Data;

namespace DataGenerator.Data
{
    public static class StatusInfo
    {
        public const int MAX_STATUSES = 150;

        public static void AddStatusData()
        {
            DataInfo.DeleteIndexedData(DataManager.DataType.Status.ToString());
            for (int ii = 0; ii < MAX_STATUSES; ii++)
            {
                (string, StatusData) status = GetStatusData(ii);
                if (status.Item1 != "")
                    DataManager.SaveData(status.Item1, DataManager.DataType.Status.ToString(), status.Item2);
            }
        }
        public static (string, StatusData) GetStatusData(int ii)
        {
            string fileName = "";
            StatusData status = new StatusData();
            if (ii == 0)
            {
                status.Name = new LocalText("None");
                //status.BeforeStatusAdds.Add(0, new OKStatusCheck(null));
                //status.OnStatusAdds.Add(-5, new ReplaceMajorStatusEffect());
            }
            else if (ii == 1)
            {
                status.Name = new LocalText("Asleep");
                fileName = "sleep";
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is asleep and incapable of any action until it wakes up. This status wears off after a few turns, or if the Pokémon is attacked.");
                status.Emoticon = "Sleep";
                status.DrawEffect = DrawEffect.Sleeping;
                status.StatusStates.Set(new MajorStatusState());
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_SLEEP_ALREADY")));
                status.BeforeStatusAdds.Add(0, new OKStatusCheck(new StringKey("MSG_SLEEP_FAIL")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SLEEP_START"), true));
                status.OnStatusAdds.Add(-5, new ReplaceMajorStatusEvent());
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SLEEP_END")));
                status.StatusStates.Set(new CountDownState(5));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Throw));
                status.BeforeTryActions.Add(0, new PreventItemActionEvent(new StringKey("MSG_CANT_USE_ITEM"), new FlagType(typeof(CurerState))));
                status.BeforeActions.Add(0, new SleepEvent());
                status.StatusStates.Set(new RecentState());
                status.BeforeBeingHits.Add(0, new RemoveRecentEvent());
                status.AfterBeingHits.Add(0, new ForceWakeEvent());
                status.OnRefresh.Add(0, new ImmobilizationEvent(typeof(SleepWalkerState)));
                status.OnRefresh.Add(0, new AttackOnlyEvent());
            }
            else if (ii == 2)
            {
                status.Name = new LocalText("Burned");
                fileName = "burn";
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon will take some damage every turn it attacks, and its Attack will be lowered.");
                status.Emoticon = "Burn";
                status.StatusStates.Set(new MajorStatusState());
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.StatusStates.Set(new AttackedThisTurnState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_BURN_ALREADY")));
                status.BeforeStatusAdds.Add(0, new OKStatusCheck(new StringKey("MSG_BURN_FAIL")));
                status.BeforeStatusAdds.Add(0, new TypeCheck("fire", new StringKey("MSG_BURN_FAIL_ELEMENT")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_BURN_START"), true));
                status.OnStatusAdds.Add(-5, new ReplaceMajorStatusEvent());
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_BURN_END")));
                status.OnActions.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Physical, 2, 3));
                //status.AfterHittings.Add(0, new StatusSpreadEffect(true));
                //status.AfterBeingHits.Add(0, new StatusSpreadEffect(false));
                status.AfterActions.Add(0, new OnAggressionEvent(new AttackedThisTurnEvent()));
                status.OnTurnEnds.Add(0, new BurnEvent(8));
            }
            else if (ii == 3)
            {
                status.Name = new LocalText("Frozen");
                fileName = "freeze";
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is frozen and incapable of acting or being affected by moves. This status wears off after a few turns, or if the Pokémon is hit by a Fire-type attack.");
                status.FreeEmote = "Freeze";
                status.DrawEffect = DrawEffect.Stopped;
                status.StatusStates.Set(new MajorStatusState());
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_FREEZE_ALREADY")));
                status.BeforeStatusAdds.Add(0, new OKStatusCheck(new StringKey("MSG_FREEZE_FAIL")));
                status.BeforeStatusAdds.Add(0, new TypeCheck("ice", new StringKey("MSG_FREEZE_FAIL_ELEMENT")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_FREEZE_START"), true));
                status.OnStatusAdds.Add(-5, new ReplaceMajorStatusEvent());
                FiniteReleaseEmitter endAnim = new FiniteReleaseEmitter(new AnimData("Ice_Pieces", 6, 0, 0), new AnimData("Ice_Pieces", 12, 1, 1), new AnimData("Ice_Pieces", 12, 1, 1));
                endAnim.BurstTime = 2;
                endAnim.ParticlesPerBurst = 4;
                endAnim.Bursts = 4;
                endAnim.StartDistance = 8;
                endAnim.Speed = 60;
                endAnim.Layer = DrawLayer.Front;
                status.OnStatusRemoves.Add(0, new StatusAnimEvent(endAnim, "DUN_Ice_Ball_2", 0, true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_FREEZE_END")));
                status.StatusStates.Set(new CountDownState(10));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Throw));
                status.BeforeTryActions.Add(0, new PreventItemActionEvent(new StringKey("MSG_CANT_USE_ITEM"), new FlagType(typeof(CurerState))));
                status.BeforeActions.Add(0, new FreezeEvent());
                status.BeforeBeingHits.Add(0, new ThawEvent());
                status.OnRefresh.Add(0, new ImmobilizationEvent());
                status.OnRefresh.Add(0, new AttackOnlyEvent());
            }
            else if (ii == 4)
            {
                status.Name = new LocalText("Paralyzed");
                fileName = "paralyze";
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is paralyzed and cannot move in consecutive turns. This status wears off after a few turns.");
                status.DrawEffect = DrawEffect.Shaking;
                status.StatusStates.Set(new MajorStatusState());
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.StatusStates.Set(new ParalyzeState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_PARALYZE_ALREADY")));
                status.BeforeStatusAdds.Add(0, new OKStatusCheck(new StringKey("MSG_PARALYZE_FAIL")));
                status.BeforeStatusAdds.Add(0, new TypeCheck("electric", new StringKey("MSG_PARALYZE_FAIL_TYPE")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_PARALYZE_START"), true));
                status.OnStatusAdds.Add(-5, new ReplaceMajorStatusEvent());
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_PARALYZE_END")));
                status.StatusStates.Set(new CountDownState(7));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
                status.BeforeTryActions.Add(0, new PreventItemParalysisEvent(new StringKey("MSG_CANT_USE_ITEM"), new FlagType(typeof(CurerState))));
                status.BeforeActions.Add(0, new ParalysisEvent(new BattleAnimEvent(new SingleEmitter(new AnimData("Spark", 3)), "DUN_Paralyzed", false)));
                status.OnTurnEnds.Add(0, new AlternateParalysisEvent());
                status.OnRefresh.Add(0, new ParaPauseEvent());
            }
            else if (ii == 5)
            {
                status.Name = new LocalText("Poisoned");
                fileName = "poison";
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon takes damage on every action, and its ability to recover HP is reduced. This status wears off after a few turns.");
                status.Emoticon = "Skull_White";
                status.StatusStates.Set(new MajorStatusState());
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.StatusStates.Set(new CountState(2));
                status.StatusStates.Set(new CountDownState(6));
                status.StatusStates.Set(new AttackedThisTurnState());
                status.StatusStates.Set(new WalkedThisTurnState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_POISON_ALREADY")));
                status.BeforeStatusAdds.Add(0, new PreventStatusCheck("poison_toxic", new StringKey("MSG_POISON_ALREADY")));
                status.BeforeStatusAdds.Add(0, new OKStatusCheck(new StringKey("MSG_POISON_FAIL")));
                status.BeforeStatusAdds.Add(0, new ExceptionStatusContextEvent(typeof(Corrosion), new TypeCheck("poison", new StringKey("MSG_POISON_FAIL_ELEMENT"))));
                status.BeforeStatusAdds.Add(0, new ExceptionStatusContextEvent(typeof(Corrosion), new TypeCheck("steel", new StringKey("MSG_POISON_FAIL_ELEMENT"))));
                status.OnStatusAdds.Add(-5, new ReplaceMajorStatusEvent());
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_POISON_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_POISON_END")));
                status.AfterActions.Add(0, new OnAggressionEvent(new PoisonEvent(false)));
                status.AfterActions.Add(0, new OnAggressionEvent(new AttackedThisTurnEvent()));
                status.OnWalks.Add(0, new PoisonSingleEvent(false, false, 16, 16));
                status.OnWalks.Add(0, new WalkedThisTurnEvent(false));
                status.OnTurnEnds.Add(1, new PoisonEndEvent(false, true, 16, 16));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.ModifyHPs.Add(0, new HealMultEvent(0, 1));
                status.RestoreHPs.Add(0, new HealMultEvent(1, 2));
            }
            else if (ii == 6)
            {
                status.Name = new LocalText("Badly Poisoned");
                fileName = "poison_toxic";
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon takes damage on every action, with the damage worsening if it attacks. Its ability to recover HP is also reduced. This status wears off after a few turns.");
                status.Emoticon = "Skull_Purple";
                status.StatusStates.Set(new MajorStatusState());
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.StatusStates.Set(new CountState(2));
                status.StatusStates.Set(new CountDownState(6));
                status.StatusStates.Set(new AttackedThisTurnState());
                status.StatusStates.Set(new WalkedThisTurnState());
                status.BeforeStatusAdds.Add(0, new PreventStatusCheck("poison", new StringKey("MSG_POISON_ALREADY")));
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_POISON_ALREADY")));
                status.BeforeStatusAdds.Add(0, new OKStatusCheck(new StringKey("MSG_POISON_FAIL")));
                status.BeforeStatusAdds.Add(0, new TypeCheck("poison", new StringKey("MSG_POISON_FAIL_ELEMENT")));
                status.BeforeStatusAdds.Add(0, new TypeCheck("steel", new StringKey("MSG_POISON_FAIL_ELEMENT")));
                status.OnStatusAdds.Add(-5, new ReplaceMajorStatusEvent());
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_POISON_TOXIC_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_POISON_END")));
                status.AfterActions.Add(0, new OnAggressionEvent(new PoisonEvent(true)));
                status.AfterActions.Add(0, new OnAggressionEvent(new AttackedThisTurnEvent()));
                status.OnWalks.Add(0, new PoisonSingleEvent(false, false, 16, 16));
                status.OnWalks.Add(0, new WalkedThisTurnEvent(false));
                status.OnTurnEnds.Add(1, new PoisonEndEvent(false, true, 16, 16));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.ModifyHPs.Add(0, new HealMultEvent(0, 1));
                status.RestoreHPs.Add(0, new HealMultEvent(1, 2));
            }
            else if (ii == 7)
            {
                status.Name = new LocalText("Confused");
                fileName = "confuse";
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is confused, causing its movements and attacks to vary wildly. Attacks that target foes can also target friends, and vice versa.");
                status.Emoticon = "Confuse";
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_CONFUSE_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_CONFUSE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_CONFUSE_END")));
                status.StatusStates.Set(new CountDownState(10));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
                status.OnRefresh.Add(0, new MovementScrambleEvent());
                status.OnActions.Add(0, new TraitorEvent());
            }
            else if (ii == 8)
            {
                status.Name = new LocalText("Cringing");
                fileName = "flinch";
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is cringing and cannot attack or use items. This status wears off on the next turn.");
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck());
                status.OnStatusAdds.Add(0, new StatusEmoteEvent(new EmoteFX(new AnimData("Emote_Exclaim", 1), 24, "", 0), true));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_CRINGE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_CRINGE_END")));
                status.StatusStates.Set(new CountDownState(2));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Throw));
                status.BeforeTryActions.Add(0, new PreventItemActionEvent(new StringKey("MSG_CANT_USE_ITEM"), new FlagType(typeof(CurerState))));
                status.BeforeActions.Add(0, new PreventActionEvent(new StringKey("MSG_CRINGE"), BattleActionType.Throw, BattleActionType.Skill));
                status.BeforeActions.Add(0, new PreventItemActionEvent(new StringKey("MSG_CRINGE"), new FlagType(typeof(CurerState))));
            }
            else if (ii == 9)
            {
                status.Name = new LocalText("Movement Speed");
                fileName = "mod_speed";
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon's Movement Speed has been modified to move faster or slower than usual.");
                status.StatusStates.Set(new StackState());
                status.StatusStates.Set(new StatChangeState(Stat.Speed));
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(1, new StringStackCheck(-3, 3, new StringKey("MSG_SPEED_BUFF_NO_MORE"), new StringKey("MSG_SPEED_BUFF_NO_LESS")));
                status.StatusStates.Set(new CountDownState(20));
                status.OnStatusAdds.Add(0, new ShowStatChangeEvent("DUN_Speed_Up", "DUN_Speed_Down", "Stat_Blue_Ring", "Stat_Blue_Line"));
                status.OnStatusAdds.Add(0, new ReportSpeedEvent(true));
                status.OnStatusAdds.Add(0, new RemoveStackZeroEvent());
                status.OnStatusRemoves.Add(0, new ReportSpeedEvent());
                status.OnMapTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.OnRefresh.Add(0, new SpeedStackEvent());
            }
            else if (ii == 10)
            {
                status.Name = new LocalText("Attack");
                fileName = "mod_attack";
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon's Attack has been modified, doing more or less damage with Physical attacks.");
                status.Emoticon = "Arrow_Up_Red";
                status.DropEmoticon = "Arrow_Down_Red";
                status.StatusStates.Set(new StackState());
                status.StatusStates.Set(new StatChangeState(Stat.Attack));
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(1, new StatStackCheck(-6, 6, Stat.Attack));
                status.OnStatusAdds.Add(0, new ShowStatChangeEvent("DUN_Attack_Up", "DUN_Atk_Down", "Stat_Red_Ring", "Stat_Red_Line"));
                status.OnStatusAdds.Add(0, new ReportStatEvent(Stat.Attack));
                status.OnStatusAdds.Add(0, new RemoveStackZeroEvent());
                status.OnStatusRemoves.Add(0, new ReportStatRemoveEvent(Stat.Attack, true));
                status.OnActions.Add(0, new UserStatBoostEvent(Stat.Attack));
                status.BeforeBeingHits.Add(-5, new TargetStatBoostEvent(Stat.Attack));
            }
            else if (ii == 11)
            {
                status.Name = new LocalText("Defense");
                fileName = "mod_defense";
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon's Defense has been modified, causing it to take more or less damage from Physical attacks.");
                status.Emoticon = "Arrow_Up_Green";
                status.DropEmoticon = "Arrow_Down_Green";
                status.StatusStates.Set(new StackState());
                status.StatusStates.Set(new StatChangeState(Stat.Defense));
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(1, new StatStackCheck(-6, 6, Stat.Defense));
                status.OnStatusAdds.Add(0, new ShowStatChangeEvent("DUN_Stat_Up_3", "DUN_Defense_Down", "Stat_Green_Ring", "Stat_Green_Line"));
                status.OnStatusAdds.Add(0, new ReportStatEvent(Stat.Defense));
                status.OnStatusAdds.Add(0, new RemoveStackZeroEvent());
                status.OnStatusRemoves.Add(0, new ReportStatRemoveEvent(Stat.Defense, true));
                status.OnActions.Add(0, new UserStatBoostEvent(Stat.Defense));
                status.BeforeBeingHits.Add(-5, new TargetStatBoostEvent(Stat.Defense));
            }
            else if (ii == 12)
            {
                status.Name = new LocalText("Special Attack");
                fileName = "mod_special_attack";
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon's Special Attack has been modified, doing more or less damage with Special attacks.");
                status.Emoticon = "Arrow_Up_Purple";
                status.DropEmoticon = "Arrow_Down_Purple";
                status.StatusStates.Set(new StackState());
                status.StatusStates.Set(new StatChangeState(Stat.MAtk));
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(1, new StatStackCheck(-6, 6, Stat.MAtk));
                status.OnStatusAdds.Add(0, new ShowStatChangeEvent("DUN_SpAtk_Up", "DUN_SpAtk_Down", "Stat_Purple_Ring", "Stat_Purple_Line"));
                status.OnStatusAdds.Add(0, new ReportStatEvent(Stat.MAtk));
                status.OnStatusAdds.Add(0, new RemoveStackZeroEvent());
                status.OnStatusRemoves.Add(0, new ReportStatRemoveEvent(Stat.MAtk, true));
                status.OnActions.Add(0, new UserStatBoostEvent(Stat.MAtk));
                status.BeforeBeingHits.Add(-5, new TargetStatBoostEvent(Stat.MAtk));
            }
            else if (ii == 13)
            {
                status.Name = new LocalText("Special Defense");
                fileName = "mod_special_defense";
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon's Special Defense has been modified, causing it to take more or less damage from Special attacks.");
                status.Emoticon = "Arrow_Up_White";
                status.DropEmoticon = "Arrow_Down_White";
                status.StatusStates.Set(new StackState());
                status.StatusStates.Set(new StatChangeState(Stat.MDef));
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(1, new StatStackCheck(-6, 6, Stat.MDef));
                status.OnStatusAdds.Add(0, new ShowStatChangeEvent("DUN_SpDef_Up", "DUN_SpDef_Down", "Stat_White_Ring", "Stat_White_Line"));
                status.OnStatusAdds.Add(0, new ReportStatEvent(Stat.MDef));
                status.OnStatusAdds.Add(0, new RemoveStackZeroEvent());
                status.OnStatusRemoves.Add(0, new ReportStatRemoveEvent(Stat.MDef, true));
                status.OnActions.Add(0, new UserStatBoostEvent(Stat.MDef));
                status.BeforeBeingHits.Add(-5, new TargetStatBoostEvent(Stat.MDef));
            }
            else if (ii == 14)
            {
                status.Name = new LocalText("Accuracy");
                fileName = "mod_accuracy";
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon's Accuracy has been modified, allowing its attacks to hit more or less often.");
                status.Emoticon = "Arrow_Up_Pink";
                status.DropEmoticon = "Arrow_Down_Pink";
                status.StatusStates.Set(new StackState());
                status.StatusStates.Set(new StatChangeState(Stat.HitRate));
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(1, new StatStackCheck(-6, 6, Stat.HitRate));
                status.OnStatusAdds.Add(0, new ShowStatChangeEvent("DUN_Stat_Up_3", "DUN_Stat_Down", "Stat_Pink_Ring", "Stat_Pink_Line"));
                status.OnStatusAdds.Add(0, new ReportStatEvent(Stat.HitRate));
                status.OnStatusAdds.Add(0, new RemoveStackZeroEvent());
                status.OnStatusRemoves.Add(0, new ReportStatRemoveEvent(Stat.HitRate, true));
                status.OnActions.Add(0, new UserStatBoostEvent(Stat.HitRate));
            }
            else if (ii == 15)
            {
                status.Name = new LocalText("Evasion");
                fileName = "mod_evasion";
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon's Evasion has been modified, allowing it to dodge attacks more or less often.");
                status.Emoticon = "Arrow_Up_Yellow";
                status.DropEmoticon = "Arrow_Down_Yellow";
                status.StatusStates.Set(new StackState());
                status.StatusStates.Set(new StatChangeState(Stat.DodgeRate));
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(1, new StatStackCheck(-6, 6, Stat.DodgeRate));
                status.OnStatusAdds.Add(0, new ShowStatChangeEvent("DUN_Evasion_Up", "DUN_Evasion_Down", "Stat_Yellow_Ring", "Stat_Yellow_Line"));
                status.OnStatusAdds.Add(0, new ReportStatEvent(Stat.DodgeRate));
                status.OnStatusAdds.Add(0, new RemoveStackZeroEvent());
                status.OnStatusRemoves.Add(0, new ReportStatRemoveEvent(Stat.DodgeRate, true));
                status.BeforeBeingHits.Add(-5, new TargetStatBoostEvent(Stat.DodgeRate));
            }
            else if (ii == 16)
            {
                status.Name = new LocalText("Range");
                fileName = "mod_range";
                status.MenuName = true;
                status.StatusStates.Set(new StackState());
                status.StatusStates.Set(new StatChangeState(Stat.Range));
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(1, new StatStackCheck(-3, 3, Stat.Range));
                status.StatusStates.Set(new CountDownState(20));
                status.OnStatusAdds.Add(0, new ShowStatChangeEvent("DUN_Evasion_Up", "DUN_Evasion_Down", "Stat_Pink_Ring", "Stat_Pink_Line"));
                status.OnStatusAdds.Add(0, new ReportStatEvent(Stat.Range));
                status.OnStatusAdds.Add(0, new RemoveStackZeroEvent());
                status.OnStatusRemoves.Add(0, new ReportStatRemoveEvent(Stat.Range, true));
                status.OnMapTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.OnActions.Add(0, new UserStatBoostEvent(Stat.Range));
            }
            else if (ii == 17)
            {
                status.Name = new LocalText("Reflect");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon takes half damage from physical attacks. This status wears off after a few turns.");
                status.Emoticon = "Shield_Blue";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_REFLECT_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_REFLECT_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.StatusStates.Set(new CountDownState(10));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Blue", 1, -1, -1, 192));
                status.BeforeBeingHits.Add(0, new ExceptInfiltratorEvent(false, new MultiplyCategoryEvent(BattleData.SkillCategory.Physical, 1, 2, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10))));
            }
            else if (ii == 18)
            {
                status.Name = new LocalText("Light Screen");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon takes half damage from special attacks. This status wears off after a few turns.");
                status.Emoticon = "Shield_Yellow";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_LIGHT_SCREEN_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_LIGHT_SCREEN_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.StatusStates.Set(new CountDownState(10));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Yellow", 1, -1, -1, 192));
                status.BeforeBeingHits.Add(0, new ExceptInfiltratorEvent(false, new MultiplyCategoryEvent(BattleData.SkillCategory.Magical, 1, 2, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10))));
            }
            else if (ii == 19)
            {
                status.Name = new LocalText("Wrap");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is rendered incapable of any action, and takes gradual damage from the foe. This status wears off after a few turns.");
                status.Targeted = true;
                status.DrawEffect = DrawEffect.Hurt;
                status.BeforeStatusAdds.Add(-1, new CountDownBoostMod(typeof(GripState), 3, 2));
                status.BeforeStatusAdds.Add(-1, new StatusHPBoostMod(typeof(BindState)));
                status.BeforeStatusAdds.Add(0, new SameTargetedStatusCheck(new StringKey("MSG_WRAP_ALREADY")));
                status.OnStatusAdds.Add(0, new TargetedBattleLogEvent(new StringKey("MSG_WRAP_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_TRAP_END")));
                status.StatusStates.Set(new CountDownState(4));
                status.StatusStates.Set(new HPState());
                status.OnTurnStarts.Add(0, new CheckNullTargetEvent(true));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.BeforeActions.Add(0, new BoundEvent(new StringKey("MSG_WRAP")));
                status.OnActions.Add(-1, new SnapDashBackEvent());
                status.OnRefresh.Add(0, new ImmobilizationEvent());
                status.OnRefresh.Add(0, new AttackOnlyEvent());
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Wrap_White_Back", 3), new AnimData("Wrap_White_Front", 3));
                emitter.HeightBack = 16;
                emitter.HeightFront = 16;
                status.TargetPassive.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.TargetPassive.BeforeActions.Add(0, new WrapTrapEvent(new StringKey("MSG_WRAP_ATTACK"), 06, new AnimEvent(emitter, "DUN_Wrap", 30), new AnimEvent(new SingleEmitter(new AnimData("Hit_Neutral", 3)), "DUN_Hit_Neutral")));
                status.TargetPassive.OnRefresh.Add(0, new ImmobilizationEvent());
                status.TargetPassive.OnRefresh.Add(0, new AttackOnlyEvent());
            }
            else if (ii == 20)
            {
                status.Name = new LocalText("Bind");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is rendered incapable of any action, and takes gradual damage from the foe. This status wears off after a few turns.");
                status.Targeted = true;
                status.DrawEffect = DrawEffect.Hurt;
                status.BeforeStatusAdds.Add(-1, new CountDownBoostMod(typeof(GripState), 3, 2));
                status.BeforeStatusAdds.Add(-1, new StatusHPBoostMod(typeof(BindState)));
                status.BeforeStatusAdds.Add(0, new SameTargetedStatusCheck(new StringKey("MSG_BIND_ALREADY")));
                status.OnStatusAdds.Add(0, new TargetedBattleLogEvent(new StringKey("MSG_BIND_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_TRAP_END")));
                status.StatusStates.Set(new CountDownState(4));
                status.StatusStates.Set(new StackState());
                status.OnTurnStarts.Add(0, new CheckNullTargetEvent(true));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.BeforeActions.Add(0, new BoundEvent(new StringKey("MSG_BIND")));
                status.OnActions.Add(-1, new SnapDashBackEvent());
                status.OnRefresh.Add(0, new ImmobilizationEvent());
                status.OnRefresh.Add(0, new AttackOnlyEvent());

                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Wrap_White_Back", 3), new AnimData("Wrap_White_Front", 3));
                emitter.HeightBack = 16;
                emitter.HeightFront = 16;
                status.TargetPassive.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.TargetPassive.BeforeActions.Add(0, new WrapTrapEvent(new StringKey("MSG_WRAP_ATTACK"), 06, new AnimEvent(emitter, "DUN_Wrap", 30), new AnimEvent(new SingleEmitter(new AnimData("Hit_Neutral", 3)), "DUN_Hit_Neutral")));
                status.TargetPassive.OnRefresh.Add(0, new ImmobilizationEvent());
                status.TargetPassive.OnRefresh.Add(0, new AttackOnlyEvent());
            }
            else if (ii == 21)
            {
                status.Name = new LocalText("In Love");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is infatuated with the enemy and cannot target it with any moves or items. This status wears off after a few turns.");
                status.Emoticon = "Exclaim_Pink";
                status.Targeted = true;
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameTargetedStatusCheck(new StringKey("MSG_LOVE_ALREADY")));
                status.BeforeStatusAdds.Add(0, new GenderStatusCheck(new StringKey("MSG_LOVE_FAIL")));
                status.OnStatusAdds.Add(0, new TargetedBattleLogEvent(new StringKey("MSG_LOVE_START"), true));
                status.OnStatusRemoves.Add(0, new TargetedBattleLogEvent(new StringKey("MSG_LOVE_END")));
                status.StatusStates.Set(new CountDownState(10));
                status.OnTurnStarts.Add(0, new CheckNullTargetEvent(false));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
                status.BeforeHittings.Add(0, new CantAttackTargetEvent(false, new StringKey("MSG_LOVE")));
            }
            else if (ii == 22)
            {
                status.Name = new LocalText("Rage Powder");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is enraged by the enemy and cannot target anyone else with any moves or items. This status wears off after a few turns.");
                status.Emoticon = "Fist_Red";
                status.Targeted = true;
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameTargetedStatusCheck(new StringKey("MSG_RAGE_POWDER_ALREADY")));
                status.OnStatusAdds.Add(0, new TargetedBattleLogEvent(new StringKey("MSG_RAGE_POWDER_START"), true));
                status.OnStatusRemoves.Add(0, new TargetedBattleLogEvent(new StringKey("MSG_RAGE_POWDER_END")));
                status.StatusStates.Set(new CountDownState(5));
                status.OnTurnStarts.Add(0, new CheckNullTargetEvent(false));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
                status.BeforeHittings.Add(0, new CantAttackTargetEvent(true, new StringKey("MSG_RAGE_POWDER")));
            }
            else if (ii == 23)
            {
                status.Name = new LocalText("Destiny Bond");
                status.MenuName = true;
                status.Desc = new LocalText("Any damage done to this Pokémon from direct attacks is also done to the target of the Destiny Bond. This status wears off after a few turns, or if the Pokémon moves.");
                status.Emoticon = "Skull_DarkBlue";
                status.Targeted = true;
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameTargetedStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.OnStatusAdds.Add(0, new TargetedBattleLogEvent(new StringKey("MSG_DESTINY_BOND_START"), true));
                status.OnStatusRemoves.Add(0, new TargetedBattleLogEvent(new StringKey("MSG_DESTINY_BOND_END")));
                status.StatusStates.Set(new CountDownState(4));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.OnWalks.Add(0, new RemoveEvent(true));
                status.AfterBeingHits.Add(0, new DestinyBondEvent());
            }
            else if (ii == 24)
            {
                status.Name = new LocalText("Chasing");
                status.MenuName = true;
                status.Desc = new LocalText("Every time the target makes a move, this Pokémon will automatically warp to it. This status wears off after a few turns.");
                status.Emoticon = "Exclaim_DarkBlue";
                status.Targeted = true;
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameTargetedStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.OnStatusAdds.Add(0, new TargetedBattleLogEvent(new StringKey("MSG_CHASE"), true));
                status.OnStatusRemoves.Add(0, new TargetedBattleLogEvent(new StringKey("MSG_CHASE_END")));
                CountDownRemoveEvent remove = new CountDownRemoveEvent(true);
                status.StatusStates.Set(new CountDownState(5));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.TargetPassive.OnTurnEnds.Add(0, new PursuitEvent());
            }
            else if (ii == 25)
            {
                status.Name = new LocalText("Last Targeted By");
                status.Targeted = true;
                status.StatusStates.Set(new HPState());//damage dealt
                //status.StatusStates.Set(new CountState());//turns passed since it was initially targeted
            }
            else if (ii == 26)
            {
                status.Name = new LocalText("Last Used Move Slot");
                status.StatusStates.Set(new SlotState());
                status.OnSkillChanges.Add(0, new UpdateIndicesEvent());
            }
            else if (ii == 27)
            {
                status.Name = new LocalText("Last Used Move");
                status.StatusStates.Set(new IDState());
            }
            else if (ii == 28)
            {
                status.Name = new LocalText("Times Move Used");
                status.StatusStates.Set(new RecentState());
                status.StatusStates.Set(new CountState(1));
                status.StatusStates.Set(new CountDownState());//turns passed since it initially used the move
                status.OnTurnEnds.Add(0, new CountUpEvent());
            }
            else if (ii == 29)
            {
                status.Name = new LocalText("Last Ally Move");
                status.StatusStates.Set(new IDState());
            }
            else if (ii == 30)
            {
                status.Name = new LocalText("Last Move Hit By Other");
                status.StatusStates.Set(new IDState());
                //status.StatusStates.Set(new CountState());//turns passed since it was initially hit by the move
            }
            else if (ii == 31)
            {
                status.Name = new LocalText("Was Hurt Last Turn");
                status.StatusStates.Set(new StackState(1));
                status.OnTurnEnds.Add(0, new RemoveEvent(false));
            }
            else if (ii == 32)
            {
                status.Name = new LocalText("Charging Solar Beam");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is charging Solar Beam. It will unleash the move on its next turn.");
                status.DrawEffect = DrawEffect.Charging;
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SOLAR_BEAM_CHARGE")));
                status.BeforeTryActions.Add(-1, new ForceMoveEvent("solar_beam"));
                status.BeforeTryActions.Add(-1, new AddContextStateEvent(new MoveCharge()));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.BeforeActions.Add(0, new RemoveOnActionEvent(false));
                status.OnRefresh.Add(0, new ImmobilizationEvent(typeof(ChargeWalkerState)));
                status.OnRefresh.Add(0, new AttackOnlyEvent());
            }
            else if (ii == 33)
            {
                status.Name = new LocalText("Charging Skull Bash");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is charging Skull Bash. It will unleash the move on its next turn.");
                status.DrawEffect = DrawEffect.Charging;
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SKULL_BASH_CHARGE")));
                status.BeforeTryActions.Add(-1, new ForceMoveEvent("skull_bash"));
                status.BeforeTryActions.Add(-1, new AddContextStateEvent(new MoveCharge()));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.BeforeActions.Add(0, new RemoveOnActionEvent(false));
                status.OnRefresh.Add(0, new ImmobilizationEvent(typeof(ChargeWalkerState)));
                status.OnRefresh.Add(0, new AttackOnlyEvent());
                status.BeforeBeingHits.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Physical, 1, 2));
            }
            else if (ii == 34)
            {
                status.Name = new LocalText("Charging Razor Wind");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is charging Razor Wind. It will unleash the move on its next turn.");
                status.DrawEffect = DrawEffect.Charging;
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_RAZOR_WIND_CHARGE")));
                status.BeforeTryActions.Add(-1, new ForceMoveEvent("razor_wind"));
                status.BeforeTryActions.Add(-1, new AddContextStateEvent(new MoveCharge()));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.BeforeActions.Add(0, new RemoveOnActionEvent(false));
                status.OnRefresh.Add(0, new ImmobilizationEvent(typeof(ChargeWalkerState)));
                status.OnRefresh.Add(0, new AttackOnlyEvent());
            }
            else if (ii == 35)
            {
                status.Name = new LocalText("Charging Sky Attack");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is charging Sky Attack. It will unleash the move on its next turn.");
                status.DrawEffect = DrawEffect.Charging;
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SKY_ATTACK_CHARGE")));
                status.BeforeTryActions.Add(-1, new ForceMoveEvent("sky_attack"));
                status.BeforeTryActions.Add(-1, new AddContextStateEvent(new MoveCharge()));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.BeforeActions.Add(0, new RemoveOnActionEvent(false));
                status.OnRefresh.Add(0, new ImmobilizationEvent(typeof(ChargeWalkerState)));
                status.OnRefresh.Add(0, new AttackOnlyEvent());
            }
            else if (ii == 36)
            {
                status.Name = new LocalText("Charging Focus Punch");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is charging Focus Punch. It will unleash the move on its next attack.");
                status.DrawEffect = DrawEffect.Charging;
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_FOCUS_PUNCH_CHARGE")));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_FOCUS_PUNCH_END")));
                status.BeforeTryActions.Add(-1, new ForceMoveEvent("focus_punch"));
                status.BeforeTryActions.Add(-1, new AddContextStateEvent(new MoveCharge()));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.BeforeActions.Add(0, new RemoveOnActionEvent(false));
                status.AfterBeingHits.Add(0, new RemoveOnDamageEvent());
                status.OnRefresh.Add(0, new AttackOnlyEvent());
            }
            else if (ii == 37)
            {
                status.Name = new LocalText("Digging");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is digging underground and can avoid most moves and obstacles. It will resurface in a few turns, or when it chooses to attack.");
                status.DrawEffect = DrawEffect.Absent;
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_DIG_CHARGE")));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_DIG_END")));
                SingleEmitter anim = new SingleEmitter(new AnimData("Dig", 3));
                anim.LocHeight = 16;
                status.OnStatusRemoves.Add(0, new StatusAnimEvent(anim, "DUN_Dig", 0, true));

                status.StatusStates.Set(new CountDownState(6));
                List<SingleCharEvent> effects = new List<SingleCharEvent>();
                effects.Add(new RemoveEvent(true));
                effects.Add(new RemoveLocTrapEvent());
                effects.Add(new RemoveLocTerrainEvent("wall"));
                status.OnTurnEnds.Add(0, new CountDownEvent(effects));
                status.BeforeTryActions.Add(-1, new ForceMoveEvent("dig"));
                status.BeforeTryActions.Add(-1, new AddContextStateEvent(new MoveCharge()));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.BeforeActions.Add(0, new OnActionEvent(new BattlelessEvent(false, new RemoveLocTrapEvent()), new BattlelessEvent(false, new RemoveLocTerrainEvent("wall")), new RemoveBattleEvent(false)));
                status.BeforeBeingHits.Add(-2, new SemiInvulEvent(new string[3] { "earthquake", "fissure", "magnitude" }));
                status.OnRefresh.Add(0, new AttackOnlyEvent());
                status.OnRefresh.Add(0, new MiscEvent(new TrapState()));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Block));
            }
            else if (ii == 38)
            {
                status.Name = new LocalText("Flying");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is flying and can avoid most moves and obstacles. It will drop back down in a few turns, or when it chooses to attack.");
                status.DrawEffect = DrawEffect.Absent;
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_FLY_CHARGE")));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_FLY_END")));
                status.StatusStates.Set(new CountDownState(6));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.BeforeTryActions.Add(-1, new ForceMoveEvent("fly"));
                status.BeforeTryActions.Add(-1, new AddContextStateEvent(new MoveCharge()));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.BeforeActions.Add(0, new RemoveOnActionEvent(false));
                status.BeforeBeingHits.Add(-2, new SemiInvulEvent(new string[8] { "gust", "whirlwind", "thunder", "twister", "sky_uppercut", "smack_down", "hurricane", "thousand_arrows" }));
                status.OnRefresh.Add(0, new AttackOnlyEvent());
                status.OnRefresh.Add(0, new MiscEvent(new TrapState()));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Block));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Water));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Lava));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Abyss));
            }
            else if (ii == 39)
            {
                status.Name = new LocalText("Diving");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is hiding underwater and can avoid most moves. It will resurface in a few turns, or when it chooses to attack.");
                status.DrawEffect = DrawEffect.Absent;
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_DIVE_CHARGE")));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_DIVE_END")));
                SingleEmitter anim = new SingleEmitter(new AnimData("Dive", 3));
                anim.LocHeight = 16;
                status.OnStatusRemoves.Add(0, new StatusAnimEvent(anim, "DUN_Dive", 0, true));

                status.StatusStates.Set(new CountDownState(6));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.BeforeTryActions.Add(-1, new ForceMoveEvent("dive"));
                status.BeforeTryActions.Add(-1, new AddContextStateEvent(new MoveCharge()));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.BeforeActions.Add(0, new RemoveOnActionEvent(false));
                status.BeforeBeingHits.Add(-2, new SemiInvulEvent(new string[2] { "surf", "whirlpool" }));
                status.OnRefresh.Add(0, new AttackOnlyEvent());
                status.OnRefresh.Add(0, new MiscEvent(new TrapState()));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Water));
            }
            else if (ii == 40)
            {
                status.Name = new LocalText("Bouncing");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is in mid-air from a bounce and can avoid most moves. It will unleash the move on its next turn.");
                status.DrawEffect = DrawEffect.Absent;
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_BOUNCE_CHARGE")));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_BOUNCE_END")));
                status.BeforeTryActions.Add(-1, new ForceMoveEvent("bounce"));
                status.BeforeTryActions.Add(-1, new AddContextStateEvent(new MoveCharge()));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.BeforeActions.Add(0, new RemoveOnActionEvent(false));
                status.OnRefresh.Add(0, new ImmobilizationEvent());
                status.BeforeBeingHits.Add(-2, new SemiInvulEvent(new string[8] { "gust", "whirlwind", "thunder", "twister", "sky_uppercut", "smack_down", "hurricane", "thousand_arrows" }));
                status.OnRefresh.Add(0, new AttackOnlyEvent());
                status.OnRefresh.Add(0, new MiscEvent(new TrapState()));
            }
            else if (ii == 41)
            {
                status.Name = new LocalText("Phantom Force");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon vanished completely and can avoid moves and obstacles. It will reappear in a few turns, or when it chooses to attack.");
                status.DrawEffect = DrawEffect.Absent;
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SHADOW_FORCE_CHARGE")));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SHADOW_FORCE_END")));
                status.OnStatusRemoves.Add(0, new StatusAnimEvent(new SingleEmitter(new AnimData("Dark_Void_Sparkle", 1)), "DUN_Snadow_Sneak_2", 0, true));

                status.StatusStates.Set(new CountDownState(6));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.BeforeTryActions.Add(-1, new ForceMoveEvent("phantom_force"));
                status.BeforeTryActions.Add(-1, new AddContextStateEvent(new MoveCharge()));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.BeforeActions.Add(0, new RemoveOnActionEvent(false));
                status.BeforeBeingHits.Add(-2, new SemiInvulEvent(new string[0] { }));
                status.OnRefresh.Add(0, new AttackOnlyEvent());
                status.OnRefresh.Add(0, new MiscEvent(new TrapState()));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Block));
            }
            else if (ii == 42)
            {
                status.Name = new LocalText("Shadow Force");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon vanished completely and can avoid moves and obstacles. Its Movement Speed is also boosted. It will reappear in a few turns, or when it chooses to attack.");
                status.DrawEffect = DrawEffect.Absent;
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SHADOW_FORCE_CHARGE")));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SHADOW_FORCE_END")));
                status.StatusStates.Set(new CountDownState(11));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.BeforeTryActions.Add(-1, new ForceMoveEvent("shadow_force"));
                status.BeforeTryActions.Add(-1, new AddContextStateEvent(new MoveCharge()));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.BeforeActions.Add(0, new RemoveOnActionEvent(false));
                status.BeforeBeingHits.Add(-2, new SemiInvulEvent(new string[0] { }));
                status.OnRefresh.Add(0, new AttackOnlyEvent());
                status.OnRefresh.Add(0, new MiscEvent(new TrapState()));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Block));
                status.OnRefresh.Add(0, new AddSpeedEvent(1));
            }
            else if (ii == 43)
            {
                status.Name = new LocalText("Biding");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is storing energy for an attack. In a few turns, it will unleash the energy, doing double the amount of damage it took while biding.");
                status.DrawEffect = DrawEffect.Charging;
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_BIDE_CHARGE")));
                status.StatusStates.Set(new HPState());
                status.BeforeTryActions.Add(-1, new ForceMoveEvent("bide"));
                status.BeforeTryActions.Add(-1, new AddContextStateEvent(new MoveBide()));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.StatusStates.Set(new CountDownState(2));
                status.BeforeActions.Add(0, new UnleashEvent());
                status.AfterBeingHits.Add(0, new BideEvent());
                status.OnRefresh.Add(0, new ImmobilizationEvent(typeof(ChargeWalkerState)));
                status.OnRefresh.Add(0, new AttackOnlyEvent());
            }
            else if (ii == 44)
            {
                status.Name = new LocalText("Fire Spin");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is trapped in a vortex of fire, taking gradual damage and preventing it from moving. This status wears off after a few turns.");
                status.DrawEffect = DrawEffect.Spinning;
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(-1, new CountDownBoostMod(typeof(GripState), 3, 2));
                status.BeforeStatusAdds.Add(-1, new StatusStackBoostMod(typeof(BindState), 2));
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey()));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_FIRE_SPIN_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_TRAP_END")));
                status.OnActions.Add(-1, new SnapDashBackEvent());

                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Fire_Spin_Back", 1), new AnimData("Fire_Spin_Front", 1));
                emitter.HeightBack = 32;
                emitter.HeightFront = 32;
                status.OnMapTurnEnds.Add(0, new PartialTrapEvent(new StringKey("MSG_HURT_BY"), new AnimEvent(emitter, "DUN_Fire_Spin", 30), new AnimEvent(new SingleEmitter(new AnimData("Hit_Neutral", 3)), "DUN_Hit_Neutral")));
                status.StatusStates.Set(new CountDownState(3));
                status.StatusStates.Set(new StackState(2));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
                status.OnRefresh.Add(0, new ImmobilizationEvent());
            }
            else if (ii == 45)
            {
                status.Name = new LocalText("Whirlpool");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is trapped in a whirlpool, taking gradual damage and preventing it from moving. This status wears off after a few turns.");
                status.DrawEffect = DrawEffect.Spinning;
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(-1, new CountDownBoostMod(typeof(GripState), 3, 2));
                status.BeforeStatusAdds.Add(-1, new StatusStackBoostMod(typeof(BindState), 2));
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey()));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_WHIRLPOOL_START"), true));
                status.OnActions.Add(-1, new SnapDashBackEvent());
                SingleEmitter emitter = new SingleEmitter(new AnimData("Gust_Blue", 1));
                emitter.LocHeight = 24;
                status.OnMapTurnEnds.Add(0, new PartialTrapEvent(new StringKey("MSG_HURT_BY"), new AnimEvent(emitter, "DUN_Whirlpool", 30), new AnimEvent(new SingleEmitter(new AnimData("Hit_Neutral", 3)), "DUN_Hit_Neutral")));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_TRAP_END")));
                status.StatusStates.Set(new CountDownState(3));
                status.StatusStates.Set(new StackState(2));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
                status.OnRefresh.Add(0, new ImmobilizationEvent());
            }
            else if (ii == 46)
            {
                status.Name = new LocalText("Sand Tomb");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is trapped in quicksand, taking gradual damage and preventing it from moving. This status wears off after a few turns.");
                status.DrawEffect = DrawEffect.Hurt;
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(-1, new CountDownBoostMod(typeof(GripState), 3, 2));
                status.BeforeStatusAdds.Add(-1, new StatusStackBoostMod(typeof(BindState), 2));
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey()));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SAND_TOMB_START"), true));
                status.OnActions.Add(-1, new SnapDashBackEvent());

                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Sand_Tomb_Back", 1), new AnimData("Sand_Tomb_Front", 1));
                emitter.HeightBack = 32;
                emitter.HeightFront = 32;
                status.OnMapTurnEnds.Add(0, new PartialTrapEvent(new StringKey("MSG_HURT_BY"), new AnimEvent(emitter, "DUN_Sand_Tomb", 20), new AnimEvent(new SingleEmitter(new AnimData("Hit_Neutral", 3)), "DUN_Hit_Neutral")));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_TRAP_END")));
                status.StatusStates.Set(new CountDownState(3));
                status.StatusStates.Set(new StackState(2));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
                status.OnRefresh.Add(0, new ImmobilizationEvent());
            }
            else if (ii == 47)
            {
                status.Name = new LocalText("Petal Dance");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon will use Petal Dance repeatedly until the status wears off. This status wears off after a few turns, leaving the Pokémon confused from fatigue.");
                status.DrawEffect = DrawEffect.Spinning;
                status.BeforeStatusAdds.Add(0, new SameStatusCheck());
                status.BeforeTryActions.Add(-1, new ForceMoveEvent("petal_dance"));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                List<SingleCharEvent> effects = new List<SingleCharEvent>();
                effects.Add(new RemoveEvent(true));
                status.StatusStates.Set(new CountDownState(4));
                StateCollection<StatusState> states = new StateCollection<StatusState>();
                states.Set(new CountDownState(10));
                effects.Add(new GiveStatusEvent("confuse", states));
                status.OnTurnEnds.Add(0, new CountDownEvent(effects));
                status.OnRefresh.Add(0, new ImmobilizationEvent());
                status.OnRefresh.Add(0, new AttackOnlyEvent());
            }
            else if (ii == 48)
            {
                status.Name = new LocalText("Thrash");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon will use Thrash repeatedly until the status wears off. This status wears off after a few turns, leaving the Pokémon confused from fatigue.");
                status.BeforeStatusAdds.Add(0, new SameStatusCheck());
                status.BeforeTryActions.Add(-1, new ForceMoveEvent("thrash"));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                List<SingleCharEvent> effects = new List<SingleCharEvent>();
                effects.Add(new RemoveEvent(true));
                status.StatusStates.Set(new CountDownState(4));
                StateCollection<StatusState> states = new StateCollection<StatusState>();
                states.Set(new CountDownState(10));
                effects.Add(new GiveStatusEvent("confuse", states));
                status.OnTurnEnds.Add(0, new CountDownEvent(effects));
                status.OnRefresh.Add(0, new ImmobilizationEvent());
                status.OnRefresh.Add(0, new AttackOnlyEvent());
            }
            else if (ii == 49)
            {
                status.Name = new LocalText("Outrage");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon will use Outrage repeatedly until the status wears off. This status wears off after a few turns, leaving the Pokémon confused from fatigue.");
                status.BeforeStatusAdds.Add(0, new SameStatusCheck());
                status.BeforeTryActions.Add(-1, new ForceMoveEvent("outrage"));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                List<SingleCharEvent> effects = new List<SingleCharEvent>();
                effects.Add(new RemoveEvent(true));
                status.StatusStates.Set(new CountDownState(4));
                StateCollection<StatusState> states = new StateCollection<StatusState>();
                states.Set(new CountDownState(10));
                effects.Add(new GiveStatusEvent("confuse", states));
                status.OnTurnEnds.Add(0, new CountDownEvent(effects));
                status.OnRefresh.Add(0, new ImmobilizationEvent());
                status.OnRefresh.Add(0, new AttackOnlyEvent());
            }
            else if (ii == 50)
            {
                status.Name = new LocalText("Cursed");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is cursed and will take damage each time it attacks a foe. This status wears off after a few turns.");
                status.Emoticon = "Skull_Red";
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_CURSE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_CURSE_END")));
                status.StatusStates.Set(new HPState());

                FiniteSprinkleEmitter emitter = new FiniteSprinkleEmitter(new AnimData("Curse", 5));
                emitter.HeightSpeed = 36;
                emitter.TotalParticles = 1;
                emitter.StartHeight = 16;
                status.AfterHittings.Add(0, new CurseEvent(new BattleAnimEvent(emitter, "DUN_Curse_2", false, 0)));
                status.StatusStates.Set(new CountDownState(5));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 51)
            {
                status.Name = new LocalText("Defense Curl");
                status.StatusStates.Set(new TransferStatusState());
            }
            else if (ii == 52)
            {
                status.Name = new LocalText("Minimized");
                status.StatusStates.Set(new TransferStatusState());
            }
            else if (ii == 53)
            {
                status.Name = new LocalText("Stockpile");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon has stockpiled energy, to use in the moves Swallow or Spit Up. It also increases its Defense and Special Defense. This status wears off after many turns have passed.");
                //needs stacking logic
                status.StatusStates.Set(new StackState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new StringStackCheck(0, 3, new StringKey("MSG_STOCKPILE_NO_MORE"), new StringKey("")));
                status.StatusStates.Set(new CountDownState(25));
                status.OnStatusAdds.Add(0, new StatusLogStackEvent(new StringKey("MSG_STOCKPILE"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STOCKPILE_END")));
                status.OnMapTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.OnActions.Add(0, new UserStatBoostEvent(Stat.Defense));
                status.BeforeBeingHits.Add(-5, new TargetStatBoostEvent(Stat.Defense));
                status.OnActions.Add(0, new UserStatBoostEvent(Stat.MDef));
                status.BeforeBeingHits.Add(-5, new TargetStatBoostEvent(Stat.MDef));
            }
            else if (ii == 54)
            {
                status.Name = new LocalText("Perish Song");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon has heard Perish Song, and will faint when its countdown reaches zero.");
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.StatusStates.Set(new CountDownState(61));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_PERISH_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.OnMapTurnEnds.Add(0, new PerishEvent(20));
            }
            else if (ii == 55)
            {
                status.Name = new LocalText("Paused");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is rendered incapable of taking any action. This status wears off after a few turns.");
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_PAUSE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_PAUSE_END")));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Throw));
                status.BeforeTryActions.Add(0, new PreventItemActionEvent(new StringKey("MSG_CANT_USE_ITEM"), new FlagType(typeof(CurerState))));
                status.BeforeActions.Add(0, new PreventActionEvent(new StringKey("MSG_PAUSE"), BattleActionType.Throw, BattleActionType.Skill));
                status.BeforeActions.Add(0, new PreventItemActionEvent(new StringKey("MSG_PAUSE"), new FlagType(typeof(CurerState))));
                status.StatusStates.Set(new CountDownState(2));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
                status.OnRefresh.Add(0, new ImmobilizationEvent());
                status.OnRefresh.Add(0, new AttackOnlyEvent());
            }
            else if (ii == 56)
            {
                status.Name = new LocalText("Future Sight");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon will be attacked a few turns in the future. Nearby allies will also take splash damage when the attack hits.");
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_FUTURE_SIGHT_START"), true));
                List<SingleCharEvent> effects = new List<SingleCharEvent>();
                effects.Add(new RemoveEvent(true));
                effects.Add(new WaitAnimsOverEvent());
                effects.Add(new BattleLogOwnerEvent(new StringKey("MSG_FUTURE_SIGHT_ATTACK")));
                status.StatusStates.Set(new HPState());
                FiniteAreaEmitter emitter = new FiniteAreaEmitter(new AnimData("U_Turn_Out", 2));
                emitter.Range = 36;
                emitter.Speed = 72;
                emitter.TotalParticles = 6;
                effects.Add(new DamageAreaEvent(1, new AnimEvent(emitter, "DUN_Blowback_Orb", 30)));
                status.StatusStates.Set(new CountDownState(3));
                status.OnTurnEnds.Add(0, new CountDownEvent(effects));
            }
            else if (ii == 57)
            {
                status.Name = new LocalText("Wish");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon will have its HP restored a few turns in the future. The amount of HP restored depends on the Maximum HP of the user.");
                status.Emoticon = "Exclaim_Yellow";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_WISH_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_WISH_START"), true));
                List<SingleCharEvent> effects = new List<SingleCharEvent>();
                effects.Add(new RemoveEvent(true));
                effects.Add(new BattleLogEvent(new StringKey("MSG_WISH_ATTACK")));
                //effects.Add(new BattleLogEffect(""));
                status.StatusStates.Set(new HPState());
                effects.Add(new HealEvent());
                status.StatusStates.Set(new CountDownState(3));
                status.OnTurnEnds.Add(0, new CountDownEvent(effects));
            }
            else if (ii == 58)
            {
                status.Name = new LocalText("Telekinesis");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is floating from telekinesis, preventing it from moving or avoiding attacks. This status wears off after a few turns.");
                status.DrawEffect = DrawEffect.Hurt;
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_TELEKINESIS_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_TRAP_END")));
                status.StatusStates.Set(new CountDownState(4));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                //status.AfterActions.Add(0, new CountDownOnActionEvent(true));
                status.OnRefresh.Add(0, new ImmobilizationEvent());
                status.OnActions.Add(-1, new SnapDashBackEvent());
                status.BeforeBeingHits.Add(0, new SureShotEvent());
                status.TargetElementEffects.Add(0, new TypeImmuneEvent("ground"));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Water));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Abyss));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Lava));
            }
            else if (ii == 59)
            {
                status.Name = new LocalText("Charge");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is charging electrical energy, boosting the power of an Electric-type attack. This status wears off after a few turns.");
                status.StatusStates.Set(new TransferStatusState());
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_CHARGE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_CHARGE_END")));
                status.StatusStates.Set(new RecentState());
                status.BeforeActions.Add(0, new RemoveRecentEvent());
                status.OnActions.Add(0, new MultiplyElementEvent("electric", 2, 1, false));
                status.StatusStates.Set(new CountDownState(6));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                //status.AfterActions.Add(0, new CountDownOnActionEffect(true));
                status.AfterActions.Add(0, new ExceptionStatusEvent(typeof(RecentState), new RemoveOnActionEvent(true)));
                status.BeforeBeingHits.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Magical, 1, 2));
            }
            else if (ii == 60)
            {
                status.Name = new LocalText("Disable");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon has one of its moves disabled and cannot use it.");
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new EmptySlotStatusCheck(new StringKey("MSG_DISABLE_FAIL")));
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_DISABLE_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusLogMoveSlotEvent(new StringKey("MSG_DISABLE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_DISABLE_END")));
                status.StatusStates.Set(new SlotState());
                status.OnRefresh.Add(0, new DisableEvent());
                status.OnSkillChanges.Add(0, new UpdateIndicesEvent());
            }
            else if (ii == 61)
            {
                status.Name = new LocalText("Yawning");
                status.Emoticon = "Yawn";
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is yawning and will fall asleep in a few turns.");
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new OKStatusCheck(new StringKey("MSG_YAWN_FAIL")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_YAWN_START"), true));
                status.OnStatusAdds.Add(-5, new ReplaceMajorStatusEvent());
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_YAWN_END")));
                List<SingleCharEvent> effects = new List<SingleCharEvent>();
                effects.Add(new RemoveEvent(false));
                StateCollection<StatusState> states = new StateCollection<StatusState>();
                states.Set(new CountDownState(10));
                effects.Add(new GiveStatusEvent("sleep", states));
                status.StatusStates.Set(new CountDownState(3));
                status.OnTurnEnds.Add(0, new CountDownEvent(effects));
            }
            else if (ii == 62)
            {
                status.Name = new LocalText("Exposed");
                status.MenuName = true;
                status.Desc = new LocalText("All attacks made on the Pokémon will ignore its resistances, immunities, and evasion changes.");
                status.Emoticon = "Exposed_Yellow";
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_EXPOSE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_EXPOSE_END")));
                status.TargetElementEffects.Add(0, new NoImmunityEvent());
                status.TargetElementEffects.Add(0, new NoResistanceEvent());
                status.BeforeBeingHits.Add(0, new IgnoreHaxEvent());
                status.StatusStates.Set(new CountDownState(30));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 63)
            {
                status.Name = new LocalText("Miracle Eye");
                status.MenuName = true;
                status.Desc = new LocalText("All attacks made by the Pokémon will ignore the target's resistances, immunities, and evasion changes.");
                status.Emoticon = "Sight_Red";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_MIRACLE_EYE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.UserElementEffects.Add(0, new NoImmunityEvent());
                status.BeforeHittings.Add(0, new IgnoreHaxEvent());
                status.StatusStates.Set(new CountDownState(10));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 64)
            {
                status.Name = new LocalText("Endure");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is bracing itself, preventing it from fainting from a direct attack. This status wears off after a few turns, or if the Pokémon moves.");
                status.Emoticon = "Exclaim_Red";
                status.DrawEffect = DrawEffect.Charging;
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_ENDURE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_ENDURE_END")));
                status.BeforeBeingHits.Add(0, new AddContextStateEvent(new AttackEndure()));
                status.StatusStates.Set(new CountDownState(4));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.OnWalks.Add(0, new RemoveEvent(true));
            }
            else if (ii == 65)
            {
                status.Name = new LocalText("Ingrain");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon's roots are planted in the ground, gradually restoring its HP and preventing it from moving. This status wears off after a few turns.");
                status.DrawEffect = DrawEffect.Charging;
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_INGRAIN_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.OnTurnEnds.Add(0, new FractionHealEvent(6, new StringKey("MSG_HEAL_WITH")));
                status.OnRefresh.Add(0, new ImmobilizationEvent());
                status.OnRefresh.Add(0, new MiscEvent(new AnchorState()));
                status.OnActions.Add(-1, new SnapDashBackEvent());
                status.TargetElementEffects.Add(1, new TypeVulnerableEvent("ground"));
                status.StatusStates.Set(new CountDownState(10));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 66)
            {
                status.Name = new LocalText("Mirror Coat");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon will return damage from special attacks back at the user with double the power. This status wears off after a few turns.");
                status.Emoticon = "Shield_Pink";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_MIRROR_COAT_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_MIRROR_COAT_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));

                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Pink", 2, -1, -1, 192), 3);
                SingleEmitter targetEmitter = new SingleEmitter(new AnimData("Hit_Neutral", 3));
                status.AfterBeingHits.Add(0, new CounterCategoryEvent(BattleData.SkillCategory.Magical, 2, 1, new BattleAnimEvent(emitter, "DUN_Light_Screen", true, 30), new BattleAnimEvent(targetEmitter, "DUN_Hit_Neutral", false)));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.StatusStates.Set(new CountDownState(4));
            }
            else if (ii == 67)
            {
                status.Name = new LocalText("Counter");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon will return damage from physical attacks back at the user with double the power. This status wears off after a few turns.");
                status.Emoticon = "Sword_Brown";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_COUNTER_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_COUNTER_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));

                SingleEmitter emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                SingleEmitter targetEmitter = new SingleEmitter(new AnimData("Close_Combat", 2));
                status.AfterBeingHits.Add(0, new CounterCategoryEvent(BattleData.SkillCategory.Physical, 2, 1, new BattleAnimEvent(emitter, "DUN_Move_Start", true, 20), new BattleAnimEvent(targetEmitter, "DUN_Bullet_Punch", false, 10)));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.StatusStates.Set(new CountDownState(4));
            }
            else if (ii == 68)
            {
                status.Name = new LocalText("Safeguard");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is protected from bad status effects with a veil. This status wears off after many turns have passed.");
                status.Emoticon = "Shield_Tan";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_ALREADY_PROTECTED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SAFEGUARD_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SAFEGUARD_END")));
                SingleEmitter emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));
                status.BeforeStatusAdds.Add(0, new ExceptInfiltratorStatusEvent(true, new StateStatusCheck(typeof(BadStatusState), new StringKey("MSG_PROTECT_STATUS"), new StatusAnimEvent(emitter, "DUN_Screen_Hit", 10))));
                status.StatusStates.Set(new CountDownState(15));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 69)
            {
                status.Name = new LocalText("Mist");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is surrounded in mist, preventing others from dropping its stats. This status wears off after many turns have passed.");
                status.Emoticon = "Shield_White";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_MIST_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_MIST_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                FiniteSprinkleEmitter emitter = new FiniteSprinkleEmitter(new AnimData("Smoke_White", 3));
                emitter.TotalParticles = 3;
                emitter.Range = 12;
                emitter.Speed = 36;
                emitter.HeightSpeed = 10;
                emitter.SpeedDiff = 10;
                status.BeforeStatusAdds.Add(0, new ExceptInfiltratorStatusEvent(true, new StatChangeCheck(new List<Stat>(), new StringKey("MSG_STAT_DROP_PROTECT"), true, false, true, new StatusAnimEvent(emitter, "DUN_Pokemon_Trap", 10))));
                status.StatusStates.Set(new CountDownState(15));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 70)
            {
                status.Name = new LocalText("Sleepless");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is prevented from becoming drowsy or sleeping. This status wears off after many turns have passed.");
                status.Emoticon = "Exposed_Red";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_SLEEPLESS_ALREADY")));
                status.BeforeStatusAdds.Add(0, new PreventStatusCheck("sleep", new StringKey("MSG_SLEEPLESS")));
                status.BeforeStatusAdds.Add(0, new PreventStatusCheck("yawning", new StringKey("MSG_SLEEPLESS")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SLEEPLESS_START"), true));
                status.OnStatusAdds.Add(0, new ThisStatusGivenEvent(new StatusCharEvent(new RemoveStatusEvent("sleep"), true)));
                status.OnStatusAdds.Add(0, new ThisStatusGivenEvent(new StatusCharEvent(new RemoveStatusEvent("yawning"), true)));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SLEEPLESS_END")));
                status.StatusStates.Set(new CountDownState(50));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 71)
            {
                status.Name = new LocalText("Enraged");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon will gain a boost to its Attack each time it takes damage from an attack. This status wears off after a few turns.");
                status.Emoticon = "Fist_Yellow";
                status.StatusStates.Set(new TransferStatusState());
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_RAGE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_RAGE_END")));
                status.AfterBeingHits.Add(0, new OnHitEvent(true, false, 100, new StatusStackBattleEvent("mod_attack", true, true, 1)));
                status.StatusStates.Set(new CountDownState(10));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 72)
            {
                status.Name = new LocalText("Taunted");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is taunted into only using attacking moves. This status wears off after many turns have passed.");
                status.Emoticon = "Fist_DarkBlue";
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_TAUNT_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_TAUNT_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.OnRefresh.Add(0, new TauntEvent());
                status.StatusStates.Set(new CountDownState(15));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 73)
            {
                status.Name = new LocalText("Encore");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is given an encore, forcing it to repeat the last move it used. This status wears off after a few turns.");
                status.Emoticon = "Exclaim_Pink";
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_ENCORE_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_ENCORE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_ENCORE_END")));
                status.OnRefresh.Add(0, new MoveLockEvent("last_used_move_slot", true));
                status.StatusStates.Set(new CountDownState(5));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 74)
            {
                status.Name = new LocalText("Torment");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is prevented from using the same move twice in a row. This status wears off after many turns have passed.");
                status.Emoticon = "Exclaim_DarkBlue";
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_TORMENT_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.OnRefresh.Add(0, new MoveLockEvent("last_used_move_slot", false));
                status.StatusStates.Set(new CountDownState(25));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 75)
            {
                status.Name = new LocalText("Grudge");
                status.MenuName = true;
                status.Desc = new LocalText("Enemies that damage this Pokémon with attacks will have all their moves lose PP. This status wears off after many turns have passed.");
                status.Emoticon = "Shield_Purple";
                status.StatusStates.Set(new TransferStatusState());
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_GRUDGE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.AfterBeingHits.Add(0, new GrudgeEvent());
                status.StatusStates.Set(new CountDownState(25));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 76)
            {
                status.Name = new LocalText("Lucky Chant");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is protected from critical hits and the additional effects of attacking moves. This status wears off after many turns have passed.");
                status.Emoticon = "Cycle_White";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_LUCKY_CHANT_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_LUCKY_CHANT_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.BeforeBeingHits.Add(0, new ExceptInfiltratorEvent(false, new BlockCriticalEvent()));
                status.BeforeBeingHits.Add(0, new ExceptInfiltratorEvent(false, new BlockAdditionalEvent()));
                status.StatusStates.Set(new CountDownState(25));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 77)
            {
                status.Name = new LocalText("Aqua Ring");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is surrounded by a veil of water that gradually restores its HP. This status wears off after a few turns.");
                status.Emoticon = "Cycle_Blue";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_AQUA_RING_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.OnTurnEnds.Add(0, new FractionHealEvent(8, new StringKey("MSG_HEAL_WITH")));
                status.StatusStates.Set(new CountDownState(10));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 78)
            {
                status.Name = new LocalText("Focus Energy");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is focused and can land critical hits more easily. This status wears off after many turns have passed.");
                status.Emoticon = "Sword_Pink";
                status.StatusStates.Set(new TransferStatusState());

                SqueezedAreaEmitter emitter = new SqueezedAreaEmitter(new AnimData("Focus_Energy", 4));
                emitter.Bursts = 4;
                emitter.ParticlesPerBurst = 2;
                emitter.BurstTime = 6;
                emitter.Range = 16;
                emitter.StartHeight = 0;
                emitter.HeightSpeed = 240;
                status.OnStatusAdds.Add(0, new StatusAnimEvent(emitter, "DUN_Focus_Energy", 20, true));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_FOCUS_ENERGY_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.OnActions.Add(0, new BoostCriticalEvent(2));
                status.StatusStates.Set(new CountDownState(30));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 79)
            {
                status.Name = new LocalText("Embargo");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is rendered incapable of using any items, and its held item item loses its effect. This status wears off after many turns have passed.");
                status.Emoticon = "X_Brown";
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_EMBARGO_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_EMBARGO_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_EMBARGO_END")));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_EMBARGO"), BattleActionType.Throw));
                status.BeforeTryActions.Add(0, new PreventItemActionEvent(new StringKey("MSG_EMBARGO"), new FlagType(typeof(CurerState))));
                status.BeforeActions.Add(0, new PreventActionEvent(new StringKey("MSG_EMBARGO"), BattleActionType.Throw));
                status.BeforeActions.Add(0, new PreventItemActionEvent(new StringKey("MSG_EMBARGO"), new FlagType(typeof(CurerState))));
                status.StatusStates.Set(new CountDownState(50));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.OnRefresh.Add(-5, new NoHeldItemEvent());
            }
            else if (ii == 80)
            {
                status.Name = new LocalText("Area Counter");
                status.MenuName = true;
                status.Desc = new LocalText("All damage done to this Pokémon of a certain category of moves will be reflected on all foes within 3 tiles. This status wears off after many turns have passed.");
                status.Emoticon = "Shield_DarkBlue";
                status.StatusStates.Set(new TransferStatusState());
                status.OnStatusAdds.Add(0, new StatusLogCategoryEvent(new StringKey("MSG_AREA_COUNTER_START"), true));
                status.OnStatusRemoves.Add(0, new StatusLogCategoryEvent(new StringKey("MSG_AREA_COUNTER_END")));
                status.StatusStates.Set(new CategoryState(BattleData.SkillCategory.None));
                FiniteAreaEmitter emitter = new FiniteAreaEmitter(new AnimData("Circle_Small_Green_Out", 2));
                emitter.TotalParticles = 24;
                emitter.Range = 84;
                emitter.Speed = 240;
                status.AfterBeingHits.Add(0, new ReflectAllEvent(1, 1, 3, new BattleAnimEvent(emitter, "DUN_Ice_Beam", true)));
                status.StatusStates.Set(new CountDownState(15));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 81)
            {
                status.Name = new LocalText("Heal Block");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is prevented from restoring its HP. This status wears off after many turns have passed.");
                status.Emoticon = "X_Gray";
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_HEAL_BLOCK_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_HEAL_BLOCK_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_HEAL_BLOCK_END")));
                status.ModifyHPs.Add(1, new HealMultEvent(0, 1));
                status.RestoreHPs.Add(0, new HealMultEvent(0, 1));
                status.StatusStates.Set(new CountDownState(50));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 82)
            {
                status.Name = new LocalText("Magic Coat");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon will reflect any moves that inflict status back at the user. This status wears off after a few turns.");
                status.Emoticon = "Shield_Green";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_ALREADY_PROTECTED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_MAGIC_COAT_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Green", 2, -1, -1, 192), 3);
                status.BeforeBeingHits.Add(-3, new ExceptInfiltratorEvent(true, new BounceStatusEvent(new StringKey("MSG_MAGIC_COAT"), new BattleAnimEvent(emitter, "DUN_Light_Screen", true, 30))));
                status.StatusStates.Set(new CountDownState(15));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 83)
            {
                status.Name = new LocalText("Metal Burst");
                status.MenuName = true;
                status.Desc = new LocalText("All damage done to this Pokémon will be reflected on all nearby foes. This status wears off after a few turns, or if the Pokémon moves.");
                status.Emoticon = "Shield_DarkBlue";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_METAL_BURST_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_METAL_BURST_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.StatusStates.Set(new CategoryState(BattleData.SkillCategory.None));
                FiniteAreaEmitter emitter = new FiniteAreaEmitter(new AnimData("Metal_Burst_Hit", 2));
                emitter.TotalParticles = 24;
                emitter.Range = 84;
                emitter.Speed = 240;
                status.AfterBeingHits.Add(0, new ReflectAllEvent(1, 1, 3, new BattleAnimEvent(new SingleEmitter(new AnimData("Metal_Burst", 2)), "DUN_Metal_Burst_2", true), new BattleAnimEvent(emitter, "", true)));
                status.StatusStates.Set(new CountDownState(4));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.OnWalks.Add(0, new RemoveEvent(true));
            }
            else if (ii == 84)
            {
                status.Name = new LocalText("Type-Boosted");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon has moves of a certain type boosted. This status wears off after many turns have passed.");
                status.Emoticon = "Arrow_Up_Red";
                status.StatusStates.Set(new TransferStatusState());

                SqueezedAreaEmitter emitter = new SqueezedAreaEmitter(new AnimData("Focus_Energy", 4));
                emitter.Bursts = 4;
                emitter.ParticlesPerBurst = 2;
                emitter.BurstTime = 6;
                emitter.Range = 16;
                emitter.StartHeight = 0;
                emitter.HeightSpeed = 240;
                status.OnStatusAdds.Add(0, new StatusAnimEvent(emitter, "DUN_Focus_Blast_3", 20, true));
                status.OnStatusAdds.Add(0, new StatusLogElementEvent(new StringKey("MSG_TYPE_BOOST_START"), true));
                status.OnStatusRemoves.Add(0, new StatusLogElementEvent(new StringKey("MSG_TYPE_BOOST_END")));
                status.StatusStates.Set(new ElementState());
                status.StatusStates.Set(new CountDownState(50));
                status.OnActions.Add(0, new MultiplyStatusElementEvent(3, 2));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 85)
            {
                status.Name = new LocalText("Type-Converted");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon has all of its moves converted to a single type.");
                status.Emoticon = "Sword_Yellow";
                status.StatusStates.Set(new TransferStatusState());
                status.OnStatusAdds.Add(0, new StatusLogElementEvent(new StringKey("MSG_TYPE_CONVERT_START"), true));
                status.OnStatusRemoves.Add(0, new StatusLogElementEvent(new StringKey("MSG_TYPE_CONVERT_END")));
                status.StatusStates.Set(new ElementState());
                status.BeforeHittings.Add(-1, new ChangeMoveElementStateEvent());
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 86)
            {
                status.Name = new LocalText("Status Protection");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is protected from a certain status effect. This status wears off after many turns have passed.");
                status.Emoticon = "Shield_Brown";
                status.StatusStates.Set(new IDState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new PreventAnyStatusCheck(new StringKey("MSG_STATUS_PROTECT"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
                status.OnStatusAdds.Add(0, new StatusLogStatusEvent(new StringKey("MSG_STATUS_PROTECT"), true));
                status.OnStatusRemoves.Add(0, new StatusLogStatusEvent(new StringKey("MSG_STATUS_PROTECT_END")));
                status.StatusStates.Set(new CountDownState(50));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 87)
            {
                status.Name = new LocalText("Wide Guard");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is protected from moves that hit in a wide range. This status wears off after a few turns.");
                status.Emoticon = "Shield_White";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_ALREADY_PROTECTED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_WIDE_GUARD_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_NO_LONGER_PROTECTED")));
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Pink", 1, -1, -1, 192));
                status.BeforeBeingHits.Add(0, new WideGuardEvent(new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
                status.StatusStates.Set(new CountDownState(5));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 88)
            {
                status.Name = new LocalText("Quick Guard");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is protected from moves that have a long range. This status wears off after a few turns.");
                status.Emoticon = "Shield_Red";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_ALREADY_PROTECTED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_QUICK_GUARD_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_NO_LONGER_PROTECTED")));
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                status.BeforeBeingHits.Add(0, new LongRangeGuardEvent(new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
                status.StatusStates.Set(new CountDownState(5));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 89)
            {
                status.Name = new LocalText("Protect");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is protected from all moves. This status wears off after a few turns, or if the Pokémon attacks.");
                status.Emoticon = "Shield_Green";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_PROTECT_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_PROTECT"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                SingleEmitter emitter = new SingleEmitter(new AnimData("Protect", 2));
                status.BeforeBeingHits.Add(0, new ProtectEvent(new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
                status.StatusStates.Set(new RecentState());
                status.StatusStates.Set(new CountDownState(3));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.BeforeActions.Add(0, new RemoveRecentEvent());
                status.AfterActions.Add(0, new ExceptionStatusEvent(typeof(RecentState), new RemoveOnActionEvent(true)));
            }
            else if (ii == 90)
            {
                status.Name = new LocalText("Immobilized");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is immobilized and cannot move. This status wears off after a few turns.");
                status.DrawEffect = DrawEffect.Shaking;
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(-1, new CountDownBoostMod(typeof(GripState), 3, 2));
                status.BeforeStatusAdds.Add(0, new TypeCheck("ghost", new StringKey("MSG_IMMOBILIZE_FAIL_ELEMENT")));
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_IMMOBILIZE_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_IMMOBILIZE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_IMMOBILIZE_END")));
                status.OnRefresh.Add(0, new ImmobilizationEvent());
                status.OnActions.Add(-1, new SnapDashBackEvent());
                status.StatusStates.Set(new CountDownState(5));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 91)
            {
                status.Name = new LocalText("Follow Me");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is the center of attention and will redirect any attack targetting its allies. This status wears off after a few turns.");
                status.Emoticon = "Exclaim_Pink";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_FOLLOW_ME_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_FOLLOW_ME_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_FOLLOW_ME_END")));
                status.ProximityEvent.Radius = 3;
                status.ProximityEvent.TargetAlignments = Alignment.Foe;
                status.ProximityEvent.BeforeExplosions.Add(0, new DrawAttackEvent(Alignment.Friend, "none", new StringKey("MSG_REDIRECT_ATTACK")));
                status.StatusStates.Set(new CountDownState(3));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 92)
            {
                status.Name = new LocalText("Super Mobile");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is given the ability to traverse walls.");
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_MOBILE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_MOBILE_END")));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Block));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Water));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Lava));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Abyss));
            }
            else if (ii == 93)
            {
                status.Name = new LocalText("Slip");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is given the ability to traverse water.");
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SLIP_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SLIP_END")));
                //status.OnstatutRemoves.Add(0, new statutBattleLogEffect("));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Water));
            }
            else if (ii == 94)
            {
                status.Name = new LocalText("Snatch");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is ready to snatch any stat-boosting move that foes use on themselves. This status wears off after a few turns.");
                status.Emoticon = "Question_DarkBlue";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SNATCH_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SNATCH_END")));
                status.ProximityEvent.Radius = 3;
                status.ProximityEvent.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                status.ProximityEvent.BeforeBeingHits.Add(-5, new SnatchEvent(new SingleEmitter(new AnimData("Charge_Up", 3)), "DUN_Move_Start"));
                status.StatusStates.Set(new CountDownState(3));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 95)
            {
                status.Name = new LocalText("Roosting");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is roosting, eliminating its Flying-type matchups. This status wears off when the Pokémon moves.");
                status.DrawEffect = DrawEffect.Sleeping;
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck());
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_ROOST_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_ROOST_END")));
                status.TargetElementEffects.Add(0, new RemoveTypeMatchupEvent("flying"));
                status.OnWalks.Add(0, new RemoveEvent(true));
            }
            else if (ii == 96)
            {
                status.Name = new LocalText("Belch");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is ready to belch, unleashing the attack whenever it eats a food item. This status wears off after a few turns.");
                status.Emoticon = "Exclaim_Purple";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_BELCH_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_BELCH_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_BELCH_END")));

                ProjectileAction altAction = new ProjectileAction();
                ((ProjectileAction)altAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)altAction).Range = 4;
                ((ProjectileAction)altAction).Speed = 10;
                ((ProjectileAction)altAction).StopAtWall = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Smoke_Purple", 2));
                shotAnim.StartDistance = 8;
                shotAnim.Shots = 7;
                shotAnim.BurstTime = 6;
                ((ProjectileAction)altAction).StreamEmitter = shotAnim;
                altAction.TargetAlignments |= Alignment.Foe;
                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments |= Alignment.Foe;
                BattleData newData = new BattleData();
                newData.Category = BattleData.SkillCategory.Magical;
                newData.Element = "poison";
                newData.HitRate = 100;
                newData.SkillStates.Set(new BasePowerState(100));
                newData.OnHits.Add(-1, new DamageFormulaEvent());
                status.AfterBeingHits.Add(0, new FoodNeededEvent(new InvokeCustomBattleEvent(altAction, altExplosion, newData, new StringKey("MSG_BELCH"), true)));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.StatusStates.Set(new CountDownState(4));
            }
            else if (ii == 97)
            {
                status.Name = new LocalText("Electrified");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon has all of its moves converted to the Electric-type. This status wears off after a few turns.");
                status.Emoticon = "Cycle_Yellow";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_ELECTRIFY_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_ELECTRIFY_END")));
                status.OnActions.Add(-1, new ChangeMoveElementEvent("none", "electric"));
                status.StatusStates.Set(new CountDownState(3));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 98)
            {
                status.Name = new LocalText("Sure Shot");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon's attacks cannot miss. This status wears off after many turns have passed.");
                status.Emoticon = "Sword_LightBlue";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SURE_SHOT_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_SURE_SHOT_END")));
                status.OnActions.Add(0, new SureShotEvent());
                status.StatusStates.Set(new CountDownState(25));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 99)
            {
                status.Name = new LocalText("Power-Charged");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is able to use moves without charging or recharging, and its PP is prevented from dropping. This status wears off after many turns have passed.");
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));

                SqueezedAreaEmitter emitter = new SqueezedAreaEmitter(new AnimData("Focus_Energy", 4));
                emitter.Bursts = 4;
                emitter.ParticlesPerBurst = 2;
                emitter.BurstTime = 6;
                emitter.Range = 16;
                emitter.StartHeight = 0;
                emitter.HeightSpeed = 240;
                status.OnStatusAdds.Add(0, new StatusAnimEvent(emitter, "DUN_Focus_Blast_3", 20, true));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_POWER_CHARGE_START"), true));
                status.BeforeStatusAdds.Add(0, new PreventStatusCheck("paused", new StringKey()));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_POWER_CHARGE_END")));
                status.BeforeTryActions.Add(-1, new AddContextStateEvent(new MoveCharge()));
                status.OnRefresh.Add(0, new PPSaverEvent());
                status.StatusStates.Set(new CountDownState(50));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 100)
            {
                status.Name = new LocalText("Mini-Counter");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon will return damage from all attacks back at the user. This status wears off after many turns have passed.");
                status.Emoticon = "Shield_Pink";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_MINI_COUNTER_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_MINI_COUNTER_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));

                SingleEmitter emitter = new SingleEmitter(new BeamAnimData("Column_Blue", 3, -1, -1, 192));
                SingleEmitter targetEmitter = new SingleEmitter(new AnimData("Hit_Neutral", 3));
                status.AfterBeingHits.Add(0, new CounterCategoryEvent(BattleData.SkillCategory.None, 1, 1, new BattleAnimEvent(emitter, "DUN_Light_Screen", true, 10), new BattleAnimEvent(targetEmitter, "DUN_Hit_Neutral", false)));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.StatusStates.Set(new CountDownState(50));
            }
            else if (ii == 101)
            {
                status.Name = new LocalText("Leech Seed");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon's HP is sapped and given to a foe every turn. This status wears off if there are no foes around.");
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new TypeCheck("grass", new StringKey("MSG_LEECH_SEED_FAIL_ELEMENT")));
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_LEECH_SEED_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_LEECH_SEED_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_LEECH_SEED_END")));
                status.OnTurnEnds.Add(0, new LeechSeedEvent(new StringKey("MSG_LEECH_SEED"), 4, 12));
            }
            else if (ii == 102)
            {
                status.Name = new LocalText("Magnet Rise");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is levitating with electromagnetism, allowing it to avoid Ground-type attacks. This status wears off after many turns have passed.");
                status.Emoticon = "Cycle_Yellow";
                status.StatusStates.Set(new TransferStatusState());
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_MAGNET_RISE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.StatusStates.Set(new CountDownState(25));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.TargetElementEffects.Add(0, new TypeImmuneEvent("ground"));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Water));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Abyss));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Lava));
            }
            else if (ii == 103)
            {
                status.Name = new LocalText("Clamp");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is rendered incapable of any action, and takes gradual damage from the foe. This status wears off after a few turns.");
                status.Targeted = true;
                status.DrawEffect = DrawEffect.Hurt;
                status.BeforeStatusAdds.Add(-1, new CountDownBoostMod(typeof(GripState), 3, 2));
                status.BeforeStatusAdds.Add(-1, new StatusHPBoostMod(typeof(BindState)));
                status.BeforeStatusAdds.Add(0, new SameTargetedStatusCheck(new StringKey("MSG_TRAP_ALREADY")));
                //status.BeforestatutAdds.Add(0, new SameTargetedStatusCheck("));
                status.OnStatusAdds.Add(0, new TargetedBattleLogEvent(new StringKey("MSG_CLAMP_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_TRAP_END")));
                status.StatusStates.Set(new CountDownState(4));
                status.StatusStates.Set(new StackState());
                status.OnTurnStarts.Add(0, new CheckNullTargetEvent(true));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.BeforeActions.Add(0, new BoundEvent(new StringKey("MSG_CLAMP")));
                status.OnActions.Add(-1, new SnapDashBackEvent());
                status.OnRefresh.Add(0, new ImmobilizationEvent());
                status.OnRefresh.Add(0, new AttackOnlyEvent());
                status.TargetPassive.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.TargetPassive.BeforeActions.Add(0, new WrapTrapEvent(new StringKey("MSG_WRAP_ATTACK"), 06, new AnimEvent(new SingleEmitter(new AnimData("Clamp", 2)), "DUN_Clamp", 10), new AnimEvent(new SingleEmitter(new AnimData("Hit_Neutral", 3)), "DUN_Hit_Neutral")));
                status.TargetPassive.OnRefresh.Add(0, new ImmobilizationEvent());
                status.TargetPassive.OnRefresh.Add(0, new AttackOnlyEvent());
            }
            else if (ii == 104)
            {
                status.Name = new LocalText("Infestation");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is trapped by an infestation, taking gradual damage and preventing it from moving. This status wears off after a few turns.");
                status.DrawEffect = DrawEffect.Hurt;
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(-1, new CountDownBoostMod(typeof(GripState), 3, 2));
                status.BeforeStatusAdds.Add(-1, new StatusStackBoostMod(typeof(BindState), 2));
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_TRAP_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_INFESTATION_START"), true));
                status.OnActions.Add(-1, new SnapDashBackEvent());
                status.OnMapTurnEnds.Add(0, new PartialTrapEvent(new StringKey("MSG_HURT_BY"), new AnimEvent(new SingleEmitter(new AnimData("Attack_Order", 2)), "DUN_Gunk_Shot_2", 35), new AnimEvent(new SingleEmitter(new AnimData("Hit_Neutral", 3)), "DUN_Hit_Neutral")));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_TRAP_END")));
                status.StatusStates.Set(new CountDownState(3));
                status.StatusStates.Set(new StackState(2));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
                status.OnRefresh.Add(0, new ImmobilizationEvent());
            }
            else if (ii == 105)
            {
                status.Name = new LocalText("Nightmare");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon takes gradual damage every turn when it is sleeping. This status wears off after many turns have passed.");
                status.Emoticon = "Skull_Pink";
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NIGHTMARE_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_NIGHTMARE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_NIGHTMARE_END")));

                FiniteSprinkleEmitter emitter = new FiniteSprinkleEmitter(new AnimData("Curse", 5));
                emitter.HeightSpeed = 36;
                emitter.TotalParticles = 1;
                emitter.StartHeight = 16;
                status.OnTurnEnds.Add(0, new NightmareEvent("sleep", 8, new StringKey("MSG_NIGHTMARE"), new AnimEvent(emitter, "DUN_Curse_2")));
                status.StatusStates.Set(new CountDownState(25));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 106)
            {
                status.Name = new LocalText("Magma Storm");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is trapped by a vortex of magma, taking gradual damage and preventing it from moving. This status wears off after a few turns.");
                status.DrawEffect = DrawEffect.Hurt;
                status.StatusStates.Set(new BadStatusState());
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(-1, new CountDownBoostMod(typeof(GripState), 3, 2));
                status.BeforeStatusAdds.Add(-1, new StatusStackBoostMod(typeof(BindState), 2));
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey()));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_MAGMA_STORM_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_TRAP_END")));
                status.OnActions.Add(-1, new SnapDashBackEvent());
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Magma_Storm_Back", 1), new AnimData("Magma_Storm_Front", 1));
                emitter.HeightBack = 32;
                emitter.HeightFront = 32;
                status.OnMapTurnEnds.Add(0, new PartialTrapEvent(new StringKey("MSG_HURT_BY"), new AnimEvent(emitter, "DUN_Magma_Storm", 30), new AnimEvent(new SingleEmitter(new AnimData("Hit_Neutral", 3)), "DUN_Hit_Neutral")));
                status.StatusStates.Set(new CountDownState(3));
                status.StatusStates.Set(new StackState(2));
                status.OnTurnStarts.Add(0, new CountDownRemoveEvent(true));
                status.OnRefresh.Add(0, new ImmobilizationEvent());
            }
            else if (ii == 107)
            {
                status.Name = new LocalText("Retaliate");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is ready to retaliate, unleashing the move whenever a nearby foe attacks an ally. This status wears off after a few turns.");
                status.Emoticon = "Sword_Brown";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_RETALIATE_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_RETALIATE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_RETALIATE_END")));
                status.ProximityEvent.Radius = 1;
                status.ProximityEvent.TargetAlignments = Alignment.Foe;
                status.ProximityEvent.AfterHittings.Add(0, new FollowUpEvent("retaliate", false, 1, new StringKey("MSG_RETALIATE")));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.StatusStates.Set(new CountDownState(6));
            }
            else if (ii == 108)
            {
                status.Name = new LocalText("Water Pledge");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is ready to unleash a follow-up attack whenever a foe is attacked. This status wears off after a few turns.");
                status.Emoticon = "Sword_Brown";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_PLEDGE_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_PLEDGE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.ProximityEvent.Radius = 3;
                status.ProximityEvent.TargetAlignments = Alignment.Foe;
                status.ProximityEvent.AfterBeingHits.Add(0, new FollowUpEvent("water_pledge", true, 0, new StringKey("MSG_PLEDGE")));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.StatusStates.Set(new CountDownState(4));
            }
            else if (ii == 109)
            {
                status.Name = new LocalText("Fire Pledge");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is ready to unleash a follow-up attack whenever a foe is attacked. This status wears off after a few turns.");
                status.Emoticon = "Sword_Brown";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_PLEDGE_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_PLEDGE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.ProximityEvent.Radius = 3;
                status.ProximityEvent.TargetAlignments = Alignment.Foe;
                status.ProximityEvent.AfterBeingHits.Add(0, new FollowUpEvent("fire_pledge", true, 0, new StringKey("MSG_PLEDGE")));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.StatusStates.Set(new CountDownState(4));
            }
            else if (ii == 110)
            {
                status.Name = new LocalText("Grass Pledge");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is ready to unleash a follow-up attack whenever a foe is attacked. This status wears off after a few turns.");
                status.Emoticon = "Sword_Brown";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_PLEDGE_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_PLEDGE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.ProximityEvent.Radius = 3;
                status.ProximityEvent.TargetAlignments = Alignment.Foe;
                status.ProximityEvent.AfterBeingHits.Add(0, new FollowUpEvent("grass_pledge", true, 0, new StringKey("MSG_PLEDGE")));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.StatusStates.Set(new CountDownState(4));
            }
            else if (ii == 111)
            {
                status.Name = new LocalText("Illusion");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon has disguised itself as a different Pokémon. Wild Pokémon of the same species will not attack it.");
                status.StatusStates.Set(new TransferStatusState());

                FiniteReleaseEmitter emitter = new FiniteReleaseEmitter(new AnimData("Puff_Green", 3), new AnimData("Puff_Yellow", 3), new AnimData("Puff_Blue", 3), new AnimData("Puff_Red", 3));
                emitter.BurstTime = 4;
                emitter.ParticlesPerBurst = 2;
                emitter.Bursts = 2;
                emitter.Speed = 48;
                emitter.StartDistance = 4;
                status.OnStatusRemoves.Add(0, new StatusAnimEvent(emitter, "DUN_Transform", 20, true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.StatusStates.Set(new MonsterIDState());
                status.OnRefresh.Add(0, new IllusionEvent());
                status.OnDeaths.Add(-11, new RemoveEvent(true));
                //status.AfterBeingHits.Add(0, new RemoveOnDamageEvent());
            }
            else if (ii == 112)
            {
                status.Name = new LocalText("Decoy");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is made into a Decoy, making it a target of everyone's attacks. This status wears off after many turns have passed.");
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_DECOY_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_DECOY_START"), true));

                FiniteReleaseEmitter emitter = new FiniteReleaseEmitter(new AnimData("Puff_Green", 3), new AnimData("Puff_Yellow", 3), new AnimData("Puff_Blue", 3), new AnimData("Puff_Red", 3));
                emitter.BurstTime = 4;
                emitter.ParticlesPerBurst = 2;
                emitter.Bursts = 2;
                emitter.Speed = 48;
                emitter.StartDistance = 4;
                status.OnStatusRemoves.Add(0, new StatusAnimEvent(emitter, "DUN_Transform", 20, true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_DECOY_END")));
                status.ProximityEvent.Radius = 1;
                status.ProximityEvent.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                status.ProximityEvent.BeforeExplosions.Add(0, new DrawAttackEvent((Alignment.Friend | Alignment.Foe), "none", new StringKey("MSG_REDIRECT_ATTACK")));
                status.StatusStates.Set(new CountDownState(15));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.OnRefresh.Add(0, new FriendlyFiredEvent());
                status.OnRefresh.Add(0, new AppearanceEvent(new MonsterID("missingno", 1, "normal", Gender.Genderless)));
            }
            else if (ii == 113)
            {
                status.Name = new LocalText("Type-Protected");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is protected from moves of a certain type. This status wears off after many turns have passed.");
                status.Emoticon = "Shield_Green";
                status.StatusStates.Set(new TransferStatusState());
                status.OnStatusAdds.Add(0, new StatusLogElementEvent(new StringKey("MSG_TYPE_PROTECT_START"), true));
                status.OnStatusRemoves.Add(0, new StatusLogElementEvent(new StringKey("MSG_TYPE_PROTECT_END")));
                status.StatusStates.Set(new ElementState());
                status.StatusStates.Set(new CountDownState(50));
                status.BeforeBeingHits.Add(0, new MultiplyStatusElementEvent(0, 1, new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", true, 10)));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 114)
            {
                status.Name = new LocalText("Blinker");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon has its Attack Range limited, allowing it to hit only targets in front. This status wears off after many turns have passed.");
                status.Emoticon = "Blind_Blue";
                status.StatusStates.Set(new TransferStatusState());
                status.OnStatusAdds.Add(0, new StatusAnimEvent(new SingleEmitter(new AnimData("Puff_Black", 3)), "DUN_Smokescreen", 30, true));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_BLINKER_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_BLINKER_END")));
                status.OnActions.Add(-1, new AddRangeEvent(-6));
                status.StatusStates.Set(new CountDownState(50));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 115)
            {
                status.Name = new LocalText("Pierce");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon's attacks and items can hit through multiple targets and walls.");
                status.Emoticon = "Sword_LightBlue";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_PIERCE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_PIERCE_END")));
                status.OnActions.Add(0, new PierceEvent(true, true, true, true));
            }
            else if (ii == 116)
            {
                status.Name = new LocalText("Conversion");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon will change its type to match its moves. This status wears off after a few turns.");
                status.Emoticon = "Cycle_Green";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_CONVERSION_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.StatusStates.Set(new CountDownState(10));
                status.OnActions.Add(0, new ConversionEvent(false));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 117)
            {
                status.Name = new LocalText("Conversion 2");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon will change its type to resist incoming moves. This status wears off after a few turns.");
                status.Emoticon = "Cycle_White";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_CONVERSION_2_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                status.StatusStates.Set(new CountDownState(10));
                status.BeforeBeingHits.Add(0, new Conversion2Event());
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 118)
            {
                status.Name = new LocalText("Recruit Boost");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon's chances of recruiting other members is temporarily increased.");
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_RECRUIT_BOOST_START"), true));
                status.BeforeHittings.Add(0, new FlatRecruitmentEvent(50));
                status.BeforeBeingHits.Add(0, new FlatRecruitmentEvent(50));
            }
            else if (ii == 119)
            {
                status.Name = new LocalText("Stat-Charged");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is protected from all stat changes, including its own.");
                status.Emoticon = "Cycle_White";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));

                SqueezedAreaEmitter emitter = new SqueezedAreaEmitter(new AnimData("Focus_Energy", 4));
                emitter.Bursts = 4;
                emitter.ParticlesPerBurst = 2;
                emitter.BurstTime = 6;
                emitter.Range = 16;
                emitter.StartHeight = 0;
                emitter.HeightSpeed = 240;
                status.OnStatusAdds.Add(0, new StatusAnimEvent(emitter, "DUN_Safeguard", 20, true));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STAT_CHARGE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STAT_CHARGE_END")));
                status.BeforeStatusAdds.Add(0, new StatChangeCheck(new List<Stat>(), new StringKey("MSG_STAT_CHARGE"), true, true, true));
            }
            else if (ii == 120)
            {
                status.Name = new LocalText("Mental-Charged");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is protected against move-binding effects. This status wears off after many turns have passed.");
                status.Emoticon = "Cycle_Red";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.BeforeStatusAdds.Add(0, new PreventStatusCheck("taunted", new StringKey("MSG_MENTAL_CHARGE")));
                status.BeforeStatusAdds.Add(0, new PreventStatusCheck("encore", new StringKey("MSG_MENTAL_CHARGE")));
                status.BeforeStatusAdds.Add(0, new PreventStatusCheck("torment", new StringKey("MSG_MENTAL_CHARGE")));
                status.BeforeStatusAdds.Add(0, new PreventStatusCheck("disable", new StringKey("MSG_MENTAL_CHARGE")));

                SqueezedAreaEmitter emitter = new SqueezedAreaEmitter(new AnimData("Focus_Energy", 4));
                emitter.Bursts = 4;
                emitter.ParticlesPerBurst = 2;
                emitter.BurstTime = 6;
                emitter.Range = 16;
                emitter.StartHeight = 0;
                emitter.HeightSpeed = 240;
                status.OnStatusAdds.Add(0, new StatusAnimEvent(emitter, "DUN_Safeguard", 20, true));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_MENTAL_CHARGE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_MENTAL_CHARGE_END")));
                status.OnRefresh.Add(1, new FreeMoveEvent());
                status.StatusStates.Set(new CountDownState(50));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 121)
            {
                status.Name = new LocalText("Weakness Drain");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon will have its HP restored if hit by a super-effective move. This status wears off after many turns have passed.");
                status.Emoticon = "Cycle_DarkBlue";
                status.StatusStates.Set(new TransferStatusState());
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_WEAKNESS_DRAIN_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_WEAKNESS_DRAIN_END")));
                status.StatusStates.Set(new CountDownState(15));
                status.BeforeBeingHits.Add(5, new AbsorbWeaknessEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Drink", new RestoreHPEvent(1, 4, true)));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 122)
            {
                status.Name = new LocalText("Doom Desire");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon will be attacked at a few turns in the future. Nearby allies will also take splash damage when the attack hits.");
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_FUTURE_SIGHT_START"), true));
                List<SingleCharEvent> effects = new List<SingleCharEvent>();
                effects.Add(new RemoveEvent(true));
                effects.Add(new BattleLogOwnerEvent(new StringKey("MSG_DOOM_DESIRE_ATTACK")));
                status.StatusStates.Set(new HPState());

                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("White", 3);
                overlay.TotalTime = 60;
                overlay.Color = Microsoft.Xna.Framework.Color.Black;
                overlay.Layer = DrawLayer.Bottom;

                FiniteAreaEmitter emitter = new FiniteAreaEmitter();
                emitter.Range = 84;
                emitter.Speed = 240;
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Blue", 3, -1, -1, 192)));
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Green", 3, -1, -1, 192)));
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Purple", 3, -1, -1, 192)));
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Red", 3, -1, -1, 192)));
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Yellow", 3, -1, -1, 192)));
                emitter.TotalParticles = 16;
                effects.Add(new DamageAreaEvent(3, new AnimEvent(overlay, "", 10), new AnimEvent(emitter, "DUN_Escape_Orb", 40)));
                status.StatusStates.Set(new CountDownState(6));
                status.OnTurnEnds.Add(0, new CountDownEvent(effects));
            }
            else if (ii == 123)
            {
                status.Name = new LocalText("Powder");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is covered in a powder that causes an explosion when exposed to fire or electricity.");
                status.Emoticon = "Exclaim_White";
                status.StatusStates.Set(new TransferStatusState());
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_POWDER_START"), true));

                status.OnActions.Add(0, new ElementNeededEvent("fire", new RemoveBattleEvent(false)));
                status.OnActions.Add(0, new ElementNeededEvent("electric", new RemoveBattleEvent(false)));
                status.OnActions.Add(0, new ElementNeededEvent("fire", new PreventActionEvent(new StringKey())));
                status.OnActions.Add(0, new ElementNeededEvent("electric", new PreventActionEvent(new StringKey())));

                {
                    AreaAction altAction = new AreaAction();
                    altAction.HitTiles = true;
                    altAction.BurstTiles = TileAlignment.Any;
                    altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                    ExplosionData altExplosion = new ExplosionData();
                    altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                    altExplosion.Range = 1;
                    altExplosion.HitTiles = true;
                    altExplosion.Speed = 10;
                    altExplosion.ExplodeFX.Sound = "DUN_Self-Destruct";
                    CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Explosion", 3));
                    emitter.ParticlesPerTile = 0.5;
                    altExplosion.Emitter = emitter;
                    BattleData newData = new BattleData();
                    newData.Element = "none";
                    newData.HitRate = -1;
                    newData.OnHits.Add(-1, new MaxHPDamageEvent(4));
                    newData.OnHitTiles.Add(0, new RemoveTrapEvent());
                    newData.OnHitTiles.Add(0, new RemoveItemEvent(true));
                    newData.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState)), new FlagType(typeof(FoliageTerrainState))));
                    status.OnActions.Add(0, new ElementNeededEvent("fire", new InvokeCustomBattleEvent(altAction, altExplosion, newData, new StringKey("MSG_POWDER"), true)));
                }

                {
                    AreaAction altAction = new AreaAction();
                    altAction.HitTiles = true;
                    altAction.BurstTiles = TileAlignment.Any;
                    altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                    ExplosionData altExplosion = new ExplosionData();
                    altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                    altExplosion.Range = 1;
                    altExplosion.HitTiles = true;
                    altExplosion.Speed = 10;
                    altExplosion.ExplodeFX.Sound = "DUN_Self-Destruct";
                    CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Explosion", 3));
                    emitter.ParticlesPerTile = 0.5;
                    altExplosion.Emitter = emitter;
                    BattleData newData = new BattleData();
                    newData.Element = "none";
                    newData.HitRate = -1;
                    newData.OnHits.Add(-1, new MaxHPDamageEvent(4));
                    newData.OnHitTiles.Add(0, new RemoveTrapEvent());
                    newData.OnHitTiles.Add(0, new RemoveItemEvent(true));
                    newData.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState)), new FlagType(typeof(FoliageTerrainState))));
                    status.OnActions.Add(0, new ElementNeededEvent("electric", new InvokeCustomBattleEvent(altAction, altExplosion, newData, new StringKey("MSG_POWDER"), true)));
                }
                status.AfterBeingHits.Add(0, new ElementNeededEvent("fire", new RemoveBattleEvent(false)));
                status.AfterBeingHits.Add(0, new ElementNeededEvent("electric", new RemoveBattleEvent(false)));

                {
                    AreaAction altAction = new AreaAction();
                    altAction.HitTiles = true;
                    altAction.BurstTiles = TileAlignment.Any;
                    altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                    ExplosionData altExplosion = new ExplosionData();
                    altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                    altExplosion.Range = 1;
                    altExplosion.HitTiles = true;
                    altExplosion.Speed = 10;
                    altExplosion.ExplodeFX.Sound = "DUN_Self-Destruct";
                    CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Explosion", 3));
                    emitter.ParticlesPerTile = 0.5;
                    altExplosion.Emitter = emitter;
                    BattleData newData = new BattleData();
                    newData.Element = "none";
                    newData.HitRate = -1;
                    newData.OnHits.Add(-1, new MaxHPDamageEvent(4));
                    newData.OnHitTiles.Add(0, new RemoveTrapEvent());
                    newData.OnHitTiles.Add(0, new RemoveItemEvent(true));
                    newData.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState)), new FlagType(typeof(FoliageTerrainState))));
                    status.AfterBeingHits.Add(0, new ElementNeededEvent("fire", new InvokeCustomBattleEvent(altAction, altExplosion, newData, new StringKey("MSG_POWDER"), true)));
                }


                {
                    AreaAction altAction = new AreaAction();
                    altAction.HitTiles = true;
                    altAction.BurstTiles = TileAlignment.Any;
                    altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                    ExplosionData altExplosion = new ExplosionData();
                    altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                    altExplosion.Range = 1;
                    altExplosion.HitTiles = true;
                    altExplosion.Speed = 10;
                    altExplosion.ExplodeFX.Sound = "DUN_Self-Destruct";
                    CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Explosion", 3));
                    emitter.ParticlesPerTile = 0.5;
                    altExplosion.Emitter = emitter;
                    BattleData newData = new BattleData();
                    newData.Element = "none";
                    newData.HitRate = -1;
                    newData.OnHits.Add(-1, new MaxHPDamageEvent(4));
                    newData.OnHitTiles.Add(0, new RemoveTrapEvent());
                    newData.OnHitTiles.Add(0, new RemoveItemEvent(true));
                    newData.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState)), new FlagType(typeof(FoliageTerrainState))));
                    status.AfterBeingHits.Add(0, new ElementNeededEvent("electric", new InvokeCustomBattleEvent(altAction, altExplosion, newData, new StringKey("MSG_POWDER"), true)));
                }
            }
            else if (ii == 124)
            {
                status.Name = new LocalText("Regeneration");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon gradually regains HP each turn. This status wears off after a few turns.");
                status.Emoticon = "Cycle_Green";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_NOTHING_HAPPENED")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_REGEN_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_REGEN_END")));
                status.OnTurnEnds.Add(0, new FractionHealEvent(4, new StringKey("MSG_REGEN")));
                status.StatusStates.Set(new CountDownState(10));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 125)
            {
                status.Name = new LocalText("Detect");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is protected from all moves. This status wears off after a few turns.");
                status.Emoticon = "Shield_Brown";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_PROTECT_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_PROTECT"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                SingleEmitter emitter = new SingleEmitter(new AnimData("Leer", 1));
                status.BeforeBeingHits.Add(0, new ProtectEvent(new BattleAnimEvent(emitter, "DUN_Mind_Reader", true, 10)));
                status.StatusStates.Set(new CountDownState(3));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 126)
            {
                status.Name = new LocalText("Missed All Last Turn");
                status.StatusStates.Set(new CountDownState(2));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(false));
            }
            else if (ii == 127)
            {
                status.Name = new LocalText("Crits Landed");
                status.StatusStates.Set(new StackState(1));
            }
            else if (ii == 128)
            {
                status.Name = new LocalText("Shopkeeper");
                status.OnRefresh.Add(0, new AddSpeedEvent(1));
            }
            else if (ii == 129)
            {
                status.Name = new LocalText("Invisible");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is invisible and cannot be seen by other Pokémon. This status wears off after a few turns.");
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_INVISIBLE_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_INVISIBLE_START"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_INVISIBLE_END")));
                status.OnRefresh.Add(0, new VanishEvent());
                status.StatusStates.Set(new CountDownState(30));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 130)
            {
                status.Name = new LocalText("Veiled");
                status.OnRefresh.Add(0, new NoNameEvent());
            }
            else if (ii == 131)
            {
                status.Name = new LocalText("Friendly Fire");
                status.OnRefresh.Add(0, new FriendlyFireToEvent());
            }
            else if (ii == 132)
            {
                status.Name = new LocalText("Sky Drop");
                fileName = "sky_drop_user";
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is holding its target in the sky and can avoid most moves. It will drop back down on the next turn.");
                status.Targeted = true;
                status.DrawEffect = DrawEffect.Absent;
                status.OnStatusAdds.Add(0, new TargetedBattleLogEvent(new StringKey("MSG_SKY_DROP_CHARGE"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_FLY_END")));
                status.OnStatusRemoves.Add(0, new RemoveTargetStatusEvent("sky_drop_target", false));
                status.OnTurnStarts.Add(0, new CheckNullTargetEvent(true));
                status.BeforeTryActions.Add(-1, new ForceMoveEvent("sky_drop"));
                status.BeforeTryActions.Add(-1, new AddContextStateEvent(new MoveCharge()));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.BeforeActions.Add(0, new ForceFaceTargetEvent());
                status.BeforeActions.Add(0, new RemoveOnActionEvent(false));
                status.BeforeBeingHits.Add(-2, new SemiInvulEvent(new string[8] { "gust", "whirlwind", "thunder", "twister", "sky_uppercut", "smack_down", "hurricane", "thousand_arrows" }));
                status.OnRefresh.Add(0, new AttackOnlyEvent());
                status.OnRefresh.Add(0, new ImmobilizationEvent());
                status.OnRefresh.Add(0, new MiscEvent(new TrapState()));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Block));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Water));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Lava));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Abyss));
            }
            else if (ii == 133)
            {
                status.Name = new LocalText("Sky Drop");
                fileName = "sky_drop_target";
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is being held in the sky and is rendered incapable of any action. Most moves will miss the Pokémon during this time.");
                status.Targeted = true;
                status.DrawEffect = DrawEffect.Absent;
                status.BeforeStatusAdds.Add(0, new SameTargetedStatusCheck(new StringKey("MSG_SKILL_FAILED")));
                status.OnStatusAdds.Add(0, new StatusAnimEvent(new EmptyFiniteEmitter(), "DUN_Fly", 0, true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_TRAP_END")));
                status.OnStatusRemoves.Add(0, new RemoveTargetStatusEvent("sky_drop_user", true));
                status.OnStatusRemoves.Add(0, new StatusCharAnimEvent(new CharAnimProcess(CharAnimProcess.ProcessType.Drop, 04)));
                status.OnTurnStarts.Add(0, new CheckNullTargetEvent(true));
                status.BeforeTryActions.Add(0, new PreventActionEvent(new StringKey("MSG_CANT_USE_ITEM"), BattleActionType.Item, BattleActionType.Throw));
                status.BeforeActions.Add(0, new PreventActionEvent(new StringKey(), BattleActionType.Skill, BattleActionType.Item, BattleActionType.Throw));
                status.BeforeBeingHits.Add(-2, new SemiInvulEvent(new string[8] { "gust", "whirlwind", "thunder", "twister", "sky_uppercut", "smack_down", "hurricane", "thousand_arrows" }));
                status.OnRefresh.Add(0, new ImmobilizationEvent());
                status.OnRefresh.Add(0, new AttackOnlyEvent());
                status.OnRefresh.Add(0, new MiscEvent(new TrapState()));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Block));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Water));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Lava));
                status.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Abyss));
            }
            else if (ii == 134)
            {
                status.Name = new LocalText("All Protect");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is protected from all moves. This status wears off after a few turns.");
                status.Emoticon = "Shield_Green";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_PROTECT_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_PROTECT"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                SingleEmitter emitter = new SingleEmitter(new AnimData("Protect", 2));
                status.BeforeBeingHits.Add(0, new ProtectEvent(new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
                status.StatusStates.Set(new CountDownState(3));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
            }
            else if (ii == 135)
            {
                status.Name = new LocalText("Transformed");
                status.StatusStates.Set(new HPState());//HP before transform
            }
            else if (ii == 136)
            {
                status.Name = new LocalText("King's Shield");
                status.MenuName = true;
                status.Desc = new LocalText("The Pokémon is protected from all damaging moves. This status wears off after a few turns, or if the Pokémon attacks. ");
                status.Emoticon = "Shield_Yellow";
                status.StatusStates.Set(new TransferStatusState());
                status.BeforeStatusAdds.Add(0, new SameStatusCheck(new StringKey("MSG_PROTECT_ALREADY")));
                status.OnStatusAdds.Add(0, new StatusBattleLogEvent(new StringKey("MSG_PROTECT"), true));
                status.OnStatusRemoves.Add(0, new StatusBattleLogEvent(new StringKey("MSG_STATUS_END")));
                SingleEmitter emitter = new SingleEmitter(new AnimData("Protect_Yellow", 2));
                status.BeforeBeingHits.Add(0, new AttackingMoveNeededEvent(new ProtectEvent(new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10))));
                status.BeforeBeingHits.Add(15, new AttackingMoveNeededEvent(new StatusStackBattleEvent("mod_attack", false, true, -1)));
                status.StatusStates.Set(new RecentState());
                status.StatusStates.Set(new CountDownState(3));
                status.OnTurnEnds.Add(0, new CountDownRemoveEvent(true));
                status.BeforeActions.Add(0, new RemoveRecentEvent());
                status.AfterActions.Add(0, new ExceptionStatusEvent(typeof(RecentState), new RemoveOnActionEvent(true)));
            }

            if (status.Name.DefaultText.StartsWith("**"))
                status.Name.DefaultText = status.Name.DefaultText.Replace("*", "");
            else if (status.Name.DefaultText != "")
                status.Released = true;

            if (fileName == "")
                fileName = Text.Sanitize(status.Name.DefaultText).ToLower();

            return (fileName, status);
        }


    }
}
