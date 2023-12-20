﻿using System;
using System.Collections.Generic;
using RogueEssence.Dungeon;
using RogueEssence.Content;
using RogueElements;
using RogueEssence;
using RogueEssence.Data;
using PMDC.Dungeon;
using PMDC;
using PMDC.Data;

namespace DataGenerator.Data
{
    public partial class SkillInfo
    {

        static void FillSkillsPMD(SkillData skill, int ii, ref string fileName)
        {
            if (ii == 0)
            {
                skill.Name = new LocalText("Attack");
                skill.Desc = new LocalText("A regular attack.");
                skill.BaseCharges = 0;
                skill.Data.Element = "none";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(30));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Attack";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 1)
            {
                skill.Name = new LocalText("Pound");
                skill.Desc = new LocalText("The target is physically pounded with a long tail, a foreleg, or the like.");
                skill.BaseCharges = 25;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(45));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(08);//Strike
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Pound";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 2)
            {
                skill.Name = new LocalText("-Karate Chop");
                skill.Desc = new LocalText("The target is attacked with a sharp chop. Critical hits land more easily.");
                skill.BaseCharges = 22;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(09);//Chop
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 3)
            {
                skill.Name = new LocalText("-Double Slap");
                skill.Desc = new LocalText("The target is slapped repeatedly, back and forth, four times in a row.");
                skill.BaseCharges = 22;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 50;
                skill.Data.SkillStates.Set(new BasePowerState(20));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 4;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(12);//Slap
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 4)
            {
                skill.Name = new LocalText("Comet Punch");
                skill.Desc = new LocalText("The target is hit with a flurry of punches that strike four times in a row.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new FistState());
                skill.Data.HitRate = 50;
                skill.Data.SkillStates.Set(new BasePowerState(30));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 4;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(11);//Punch
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Punch";
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Print_Fist", 12));
            }
            else if (ii == 5)
            {
                skill.Name = new LocalText("Mega Punch");
                skill.Desc = new LocalText("The target is slugged by a punch thrown with muscle-packed power.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new FistState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(11);//Punch
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Punch";
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Print_Fist", 12));
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Mega_Background_Yellow", 3));
                endAnim.Layer = DrawLayer.Back;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 6)
            {
                skill.Name = new LocalText("Pay Day");
                skill.Desc = new LocalText("Numerous coins are hurled at the target to inflict damage. It causes the target to drop a portion of its money.");
                skill.BaseCharges = 14;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new KnockMoneyEvent(4)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Pay_Day", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Ice_Shard";
            }
            else if (ii == 7)
            {
                skill.Name = new LocalText("Fire Punch");
                skill.Desc = new LocalText("The target is punched with a fiery fist. This may also leave the target with a burn.");
                skill.BaseCharges = 18;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new FistState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("burn", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(11);//Punch
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).LagBehindTime = 5;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Punch";
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Print_Fist", 12));
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Flamethrower", 3));
                endAnim.LocHeight = 14;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Flamethrower_2";
            }
            else if (ii == 8)
            {
                skill.Name = new LocalText("Ice Punch");
                skill.Desc = new LocalText("The target is punched with an icy fist. This may also leave the target frozen.");
                skill.BaseCharges = 18;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new FistState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("freeze", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(11);//Punch
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).LagBehindTime = 5;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Punch";
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Print_Fist", 12));
                FiniteReleaseEmitter endAnim = new FiniteReleaseEmitter(new AnimData("Ice_Pieces", 6, 0, 0), new AnimData("Ice_Pieces", 12, 1, 1), new AnimData("Ice_Pieces", 12, 1, 1));
                endAnim.BurstTime = 2;
                endAnim.ParticlesPerBurst = 4;
                endAnim.Bursts = 4;
                endAnim.StartDistance = 4;
                endAnim.Speed = 60;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Ice_Beam_2";
            }
            else if (ii == 9)
            {
                skill.Name = new LocalText("Thunder Punch");
                skill.Desc = new LocalText("The target is punched with an electrified fist. This may also leave the target with paralysis.");
                skill.BaseCharges = 18;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new FistState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("paralyze", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(11);//Punch
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).LagBehindTime = 5;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Punch";
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Print_Fist", 12));
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Spark", 3));
                skill.Data.HitFX.Sound = "DUN_Shock_Wave";
            }
            else if (ii == 10)
            {
                skill.Name = new LocalText("Scratch");
                skill.Desc = new LocalText("Hard, pointed, sharp claws rake the target to inflict damage.");
                skill.BaseCharges = 24;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(10);//Scratch
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Scratch";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Scratch", 3));
                endAnim.LocHeight = 4;
                endAnim.Offset = -8;
                ((AttackAction)skill.HitboxAction).Emitter = endAnim;
                skill.Data.HitFX.Sound = "-";
            }
            else if (ii == 11)
            {
                skill.Name = new LocalText("-Vice Grip");
                skill.Desc = new LocalText("The target is gripped and squeezed from both sides to inflict damage. The target becomes immobilized.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("immobilized", true, false)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(18);//Bite
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Spore";
            }
            else if (ii == 12)
            {
                skill.Name = new LocalText("Guillotine");
                skill.Desc = new LocalText("A vicious, tearing attack with big pincers. The target faints instantly if it hits.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 50;
                skill.Data.BeforeHits.Add(0, new ExplorerImmuneEvent());
                skill.Data.OnHits.Add(-1, new OHKODamageEvent());
                //Other sounds: _UNK_DUN_Rustle_2 _UNK_DUN_Spray_Soft
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", terrainEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(13);//Slice
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Guillotine_Front", 1));
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Fury_Cutter";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Guillotine_Back", 1));
                endAnim.Layer = DrawLayer.Back;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Delay = 10;
            }
            else if (ii == 13)
            {
                skill.Name = new LocalText("Razor Wind");
                skill.Desc = new LocalText("A two-turn attack. Blades of wind hit opposing Pokémon on the second turn. Critical hits land more easily.");
                skill.BaseCharges = 14;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                SelfAction altAction = new SelfAction();
                altAction.CharAnimData = new CharAnimFrameType(06);//Charge
                altAction.TargetAlignments |= Alignment.Self;
                SingleEmitter altAnim = new SingleEmitter(new AnimData("Gust", 1));
                altAnim.LocHeight = 24;
                altAction.ActionFX.Emitter = altAnim;
                altAction.ActionFX.Sound = "DUN_Tailwind";
                skill.Data.BeforeTryActions.Add(0, new ChargeOrReleaseEvent("charging_razor_wind", altAction));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", terrainEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Full;
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 30;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Razor_Wind", 2));
                emitter.BurstTime = 2;
                emitter.ParticlesPerBurst = 2;
                emitter.Bursts = 30;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Tailwind_2";
            }
            else if (ii == 14)
            {
                skill.Name = new LocalText("Swords Dance");
                skill.Desc = new LocalText("A frenetic dance to uplift the fighting spirit. This sharply raises the Attack stat.");
                skill.BaseCharges = 12;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 2));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(27);//Twirl
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Swords_Dance", 3));
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Icicle_Spear";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Sound = "DUN_Swords_Dance_2";
            }
            else if (ii == 15)
            {
                skill.Name = new LocalText("Cut");
                skill.Desc = new LocalText("The target is cut with a scythe or claw.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", terrainEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Around;
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Cut", 2));
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Cut";
            }
            else if (ii == 16)
            {
                skill.Name = new LocalText("Gust");
                skill.Desc = new LocalText("A gust of wind is whipped up by wings and launched at the target to inflict damage.");
                skill.BaseCharges = 22;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(45));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(32);//FlapAround
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 5;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                SingleEmitter shootEmitter = new SingleEmitter(new AnimData("Gust", 1));
                shootEmitter.LocHeight = 24;
                skill.HitboxAction.ActionFX.Emitter = shootEmitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Gust_Trap";
            }
            else if (ii == 17)
            {
                skill.Name = new LocalText("Wing Attack");
                skill.Desc = new LocalText("The target is struck with large, imposing wings spread wide to inflict damage.");
                skill.BaseCharges = 16;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.Wide;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                ((DashAction)skill.HitboxAction).LagBehindTime = 10;
                skill.HitboxAction.ActionFX.Sound = "DUN_Wing_Attack_2";
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe; skill.HitboxAction.TileEmitter = new SingleEmitter(new AnimData("Air_Slash_Slash", 2));
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Wing_Attack", 2));
                endAnim.LocHeight = 24;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 18)
            {
                skill.Name = new LocalText("Whirlwind");
                skill.Desc = new LocalText("The user generates a powerful wind that blows away other Pokémon.");
                skill.BaseCharges = 12;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new KnockBackEvent(8));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(32);//FlapAround
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 5;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Gust_Wind", 1));
                emitter.ParticlesPerTile = 1;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                SingleEmitter shootEmitter = new SingleEmitter(new AnimData("Gust", 1));
                shootEmitter.LocHeight = 24;
                skill.HitboxAction.ActionFX.Emitter = shootEmitter;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Gust_Trap";
            }
            else if (ii == 19)
            {
                skill.Name = new LocalText("Fly");
                skill.Desc = new LocalText("The user soars and then strikes its target any time in the next 5 turns.");
                skill.BaseCharges = 16;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                SelfAction altAction = new SelfAction();
                altAction.CharAnimData = new CharAnimProcess(CharAnimProcess.ProcessType.Fly);//Fly
                altAction.TargetAlignments |= Alignment.Self;

                BattleFX altPreFX = new BattleFX();
                altPreFX.Sound = "DUN_Fly";
                altAction.PreActions.Add(altPreFX);

                skill.Data.BeforeTryActions.Add(0, new ChargeOrReleaseEvent("flying", altAction));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).AppearanceMod = DashAction.DashAppearance.DropDown;
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Fly";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Wing_Attack", 2));
                endAnim.LocHeight = 24;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 20)
            {
                skill.Name = new LocalText("Bind");
                skill.Desc = new LocalText("Things such as long bodies or tentacles are used to bind and squeeze the target for three turns.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 70;
                skill.Data.SkillStates.Set(new BasePowerState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new GiveContinuousDamageEvent("bind", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Wrap_White_Back", 3), new AnimData("Wrap_White_Front", 3));
                emitter.HeightBack = 16;
                emitter.HeightFront = 16;
                ((AttackAction)skill.HitboxAction).Emitter = emitter;
                ((AttackAction)skill.HitboxAction).LagBehindTime = 30;
                skill.HitboxAction.ActionFX.Sound = "DUN_Wrap";
            }
            else if (ii == 21)
            {
                skill.Name = new LocalText("Slam");
                skill.Desc = new LocalText("The target is slammed with a long tail, vines, or the like to inflict damage.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Wide;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Attack";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 22)
            {
                skill.Name = new LocalText("Vine Whip");
                skill.Desc = new LocalText("The target is struck with slender, whiplike vines to inflict damage.");
                skill.BaseCharges = 20;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(45));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Wide;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Vine_Whip", 3));
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Punishment";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Vine_Whip_2", 3));
            }
            else if (ii == 23)
            {
                skill.Name = new LocalText("=Stomp");
                skill.Desc = new LocalText("The target is stomped with a big foot. This may also make the target flinch.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(65));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(24);//Stomp
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Print_Foot", 12));
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Stomp";
            }
            else if (ii == 24)
            {
                skill.Name = new LocalText("Double Kick");
                skill.Desc = new LocalText("The target is quickly kicked twice in succession using both feet.");
                skill.BaseCharges = 20;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 75;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 2;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(21);//Kick
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Double_Kick";
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Print_Foot", 12));
            }
            else if (ii == 25)
            {
                skill.Name = new LocalText("Mega Kick");
                skill.Desc = new LocalText("The target is attacked by a kick launched with muscle-packed power.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 75;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Shadow_Force";
                BattleFX introFX = new BattleFX();
                introFX.Emitter = new SingleEmitter(new AnimData("Print_Foot", 12));
                skill.Data.IntroFX.Add(introFX);
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Mega_Background_Yellow", 3));
                endAnim.Layer = DrawLayer.Back;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Double_Kick";
            }
            else if (ii == 26)
            {
                skill.Name = new LocalText("Jump Kick");
                skill.Desc = new LocalText("The user jumps up high, then strikes with a kick. If the kick misses, the user hurts itself.");
                skill.BaseCharges = 14;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(85));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new CrashLandEvent(6));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 21;//Kick
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Tackle";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Print_Foot", 12));
                skill.Data.HitFX.Sound = "DUN_Double_Kick";
            }
            else if (ii == 27)
            {
                skill.Name = new LocalText("Rolling Kick");
                skill.Desc = new LocalText("The user lashes out with a quick, spinning kick. This may also make the target flinch.");
                skill.BaseCharges = 14;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 75;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Wide;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Rock_Climb";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Print_Foot", 12));
            }
            else if (ii == 28)
            {
                skill.Name = new LocalText("Sand Attack");
                skill.Desc = new LocalText("Sand is hurled in the target's face, reducing the target's Accuracy.");
                skill.BaseCharges = 20;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_accuracy", true, false, -1));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Sand_Tomb_Sand", 3));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 4;
                emitter.Bursts = 3;
                emitter.StartDistance = 4;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "_UNK_DUN_Sneeze";
            }
            else if (ii == 29)
            {
                skill.Name = new LocalText("Headbutt");
                skill.Desc = new LocalText("The user sticks out its head and attacks by charging straight into the target. This may also make the target flinch.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Tackle";
            }
            else if (ii == 30)
            {
                skill.Name = new LocalText("Horn Attack");
                skill.Desc = new LocalText("The target is jabbed with a sharply pointed horn to inflict damage. Critical hits land more easily.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 20;//Jab
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                ((DashAction)skill.HitboxAction).AnimOffset = 12;
                ((DashAction)skill.HitboxAction).Anim = new AnimData("Fury_Attack", 2);
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Fury_Attack";
            }
            else if (ii == 31)
            {
                skill.Name = new LocalText("Fury Attack");
                skill.Desc = new LocalText("The target is jabbed repeatedly with a horn or beak five times in a row.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 50;
                skill.Data.SkillStates.Set(new BasePowerState(20));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 5;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 20;//Jab
                ((DashAction)skill.HitboxAction).Range = 1;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                ((DashAction)skill.HitboxAction).AnimOffset = 12;
                ((DashAction)skill.HitboxAction).Anim = new AnimData("Fury_Attack", 2);
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Fury_Attack";
            }
            else if (ii == 32)
            {
                skill.Name = new LocalText("Horn Drill");
                skill.Desc = new LocalText("The user stabs the target with a horn that rotates like a drill. The target faints instantly if it hits.");
                skill.BaseCharges = 8;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 50;
                skill.Data.BeforeHits.Add(0, new ExplorerImmuneEvent());
                skill.Data.OnHits.Add(-1, new OHKODamageEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Wall_Break", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Rollout", terrainEmitter, new FlagType(typeof(WallTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 20;//Jab
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                ((DashAction)skill.HitboxAction).AnimOffset = 12;
                ((DashAction)skill.HitboxAction).Anim = new AnimData("Fury_Attack", 2);
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Fire_Blast";
            }
            else if (ii == 33)
            {
                skill.Name = new LocalText("Tackle");
                skill.Desc = new LocalText("A physical attack in which the user charges and slams into the target with its whole body.");
                skill.BaseCharges = 22;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(45));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Tackle";
            }
            else if (ii == 34)
            {
                skill.Name = new LocalText("-Body Slam");
                skill.Desc = new LocalText("The user drops onto the target with its full body weight. This may also leave the target with paralysis.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("paralyze", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(24);//Stomp
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 35)
            {
                skill.Name = new LocalText("Wrap");
                skill.Desc = new LocalText("A long body or vines are used to wrap and squeeze the target for three turns.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(20));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new GiveContinuousDamageEvent("wrap", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Wrap_White_Back", 3), new AnimData("Wrap_White_Front", 3));
                emitter.HeightBack = 16;
                emitter.HeightFront = 16;
                ((AttackAction)skill.HitboxAction).Emitter = emitter;
                ((AttackAction)skill.HitboxAction).LagBehindTime = 30;
                skill.HitboxAction.ActionFX.Sound = "DUN_Wrap";
            }
            else if (ii == 36)
            {
                skill.Name = new LocalText("Take Down");
                skill.Desc = new LocalText("A reckless, full-body charge attack for slamming into the target. This also damages the user a little.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new HPRecoilEvent(5, false));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 4;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Hammer_Arm";
            }
            else if (ii == 37)
            {
                skill.Name = new LocalText("Thrash");
                skill.Desc = new LocalText("The user rampages and attacks for three turns. The user then becomes confused.");
                skill.BaseCharges = 14;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new StatusBattleEvent("thrash", false, true));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 23;//Slam
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Rage", 2));
                endAnim.Layer = DrawLayer.Back;
                endAnim.LocHeight = 44;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = endAnim;
                preFX.Sound = "DUN_Rage";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 38)
            {
                skill.Name = new LocalText("=Double-Edge");
                skill.Desc = new LocalText("A reckless, life-risking tackle. This also damages the user quite a lot.");
                skill.BaseCharges = 8;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new HPRecoilEvent(4, false));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 5;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Close_Combat";
            }
            else if (ii == 39)
            {
                skill.Name = new LocalText("Tail Whip");
                skill.Desc = new LocalText("The user wags its tail cutely, making opposing Pokémon less wary and lowering their Defense stat.");
                skill.BaseCharges = 22;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, -1));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(28);//TailWhip
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Tail_Whip";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 40)
            {
                skill.Name = new LocalText("Poison Sting");
                skill.Desc = new LocalText("The user stabs the target with a poisonous stinger. This may also poison the target.");
                skill.BaseCharges = 22;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("poison", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 2;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Pin_Missile", 3);
                skill.HitboxAction.ActionFX.Sound = "DUN_Throw_Spike";
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                SqueezedAreaEmitter emitter = new SqueezedAreaEmitter(new AnimData("Bubbles_Purple", 3));
                emitter.BurstTime = 3;
                emitter.Bursts = 4;
                emitter.ParticlesPerBurst = 1;
                emitter.Range = 12;
                emitter.StartHeight = -4;
                emitter.HeightSpeed = 12;
                emitter.SpeedDiff = 4;
                skill.Data.HitFX.Emitter = emitter;
            }
            else if (ii == 41)
            {
                skill.Name = new LocalText("Twineedle");
                skill.Desc = new LocalText("The user damages the target twice in succession by jabbing it with two spikes. This may also poison the target.");
                skill.BaseCharges = 18;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(30));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("poison", true, true)));
                skill.Strikes = 2;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 20;//Jab
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                ((DashAction)skill.HitboxAction).AnimOffset = 12;
                ((DashAction)skill.HitboxAction).Anim = new AnimData("Twin_Needle", 2);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Fury_Attack";
            }
            else if (ii == 42)
            {
                skill.Name = new LocalText("Pin Missile");
                skill.Desc = new LocalText("Sharp spikes are shot at the target in rapid succession. They hit four times in a row.");
                skill.BaseCharges = 14;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 50;
                skill.Data.SkillStates.Set(new BasePowerState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 4;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 3;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).Rays = ProjectileAction.RayCount.Three;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Pin_Missile", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "_UNK_DUN_Rustle_2";
            }
            else if (ii == 43)
            {
                skill.Name = new LocalText("Leer");
                skill.Desc = new LocalText("The user gives opposing Pokémon an intimidating leer that lowers the Defense stat.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, -1));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).Speed = 15;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Leer", 2));
                preFX.Sound = "DUN_Leer_2";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 44)
            {
                skill.Name = new LocalText("Bite");
                skill.Desc = new LocalText("The target is bitten with viciously sharp fangs. This may also make the target flinch.");
                skill.BaseCharges = 18;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new JawState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(18);//Bite
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).LagBehindTime = 10;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Bite", 3));
                emitter.Offset = -8;
                ((AttackAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.ActionFX.Sound = "DUN_Bite";
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 45)
            {
                skill.Name = new LocalText("Growl");
                skill.Desc = new LocalText("The user growls in an endearing way, making opposing Pokémon less wary. This lowers their Attack stats.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, -1));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(30);//Sound
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Growl", 2), 2);
                emitter.Offset = 12;
                emitter.Layer = DrawLayer.Top;
                ((AreaAction)skill.HitboxAction).ActionFX.Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Growl";
            }
            else if (ii == 46)
            {
                skill.Name = new LocalText("Roar");
                skill.Desc = new LocalText("The user makes a terrifying roar that blows back opposing Pokémon.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new KnockBackEvent(8));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Growl", 2), 2);
                emitter.Offset = 12;
                emitter.Layer = DrawLayer.Top;
                ((AreaAction)skill.HitboxAction).ActionFX.Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Growl";
            }
            else if (ii == 47)
            {
                skill.Name = new LocalText("=Sing");
                skill.Desc = new LocalText("A soothing lullaby is sung in a calming voice that puts the target into a deep slumber.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 85;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("sleep", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(29);//Sing
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 3;
                CircleSquareSprinkleEmitter emitter = new CircleSquareSprinkleEmitter();
                for (int nn = 0; nn < 44; nn++)
                    emitter.Anims.Add(new ParticleAnim(new AnimData("Music_Notes", 30, nn, nn)));
                emitter.ParticlesPerTile = 1.5;
                emitter.HeightSpeed = 30;
                emitter.SpeedDiff = 30;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Sing";
            }
            else if (ii == 48)
            {
                skill.Name = new LocalText("=Supersonic");
                skill.Desc = new LocalText("The user generates odd sound waves from its body that confuse the target.");
                skill.BaseCharges = 14;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 50;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("confuse", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(30);//Sound
                ((ProjectileAction)skill.HitboxAction).Range = 3;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Wave_Circle_Yellow", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Leech_Seed_2";
                RepeatEmitter endAnim = new RepeatEmitter(new AnimData("Circle_Thick_Red_Out", 2));
                endAnim.Bursts = 4;
                endAnim.BurstTime = 6;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 49)
            {
                skill.Name = new LocalText("Sonic Boom");
                skill.Desc = new LocalText("The target is hit with a destructive shock wave that always inflicts 1/4 of the target's max HP.");
                skill.BaseCharges = 14;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(-1, new MaxHPDamageEvent(4));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(30);//Sound
                ((ProjectileAction)skill.HitboxAction).Range = 3;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Sonic_Boom", 3));
                shotAnim.StartDistance = 16;
                shotAnim.Shots = 8;
                shotAnim.BurstTime = 4;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Swift";
            }
            else if (ii == 50)
            {
                skill.Name = new LocalText("Disable");
                skill.Desc = new LocalText("This move prevents the target from using the move it last used.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new DisableBattleEvent("disable", "last_used_move_slot"));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 15;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Leer", 2));
                preFX.Sound = "DUN_Psycho_Shift";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 51)
            {
                skill.Name = new LocalText("=Acid");
                skill.Desc = new LocalText("The opposing Pokémon are attacked with a spray of harsh acid. This may also lower their Sp. Def stats.");
                skill.BaseCharges = 14;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_special_defense", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 3;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                AttachReleaseEmitter emitter = new AttachReleaseEmitter(new AnimData("Bubbles_Green", 3));
                emitter.BurstTime = 3;
                emitter.ParticlesPerBurst = 1;
                emitter.Speed = 12;
                emitter.StartDistance = 6;
                ((ProjectileAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Bubble";
                skill.Data.HitFX.Sound = "DUN_Bubble_2";
            }
            else if (ii == 52)
            {
                skill.Name = new LocalText("Ember");
                skill.Desc = new LocalText("The target is attacked with small flames. This may also leave the target with a burn.");
                skill.BaseCharges = 15;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("burn", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Fire_Spin_Fireball", 1, 0, 0), new AnimData("Fire_Spin_Fireball", 1, 1, 1), new AnimData("Fire_Spin_Fireball", 1, 2, 2), new AnimData("Fire_Spin_Fireball", 1, 3, 3), new AnimData("Fire_Spin_Fireball", 1, 4, 4), new AnimData("Fire_Spin_Fireball", 1, 5, 5), new AnimData("Fire_Spin_Fireball", 1, 6, 6));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 2;
                emitter.Bursts = 4;
                emitter.StartDistance = 4;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Ember";
                SingleEmitter endEmitter = new SingleEmitter(new AnimData("Ember", 3));
                endEmitter.LocHeight = 8;
                skill.Data.HitFX.Emitter = endEmitter;
                skill.Data.HitFX.Sound = "DUN_Ember_2";
            }
            else if (ii == 53)
            {
                skill.Name = new LocalText("Flamethrower");
                skill.Desc = new LocalText("The target is scorched with an intense blast of fire. This may also leave the target with a burn.");
                skill.BaseCharges = 8;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("burn", true, true)));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 5;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Flamethrower_2", 2));
                shotAnim.StartDistance = 8;
                shotAnim.Shots = 8;
                shotAnim.BurstTime = 5;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Flamethrower_2";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Flamethrower", 3));
                endAnim.LocHeight = 14;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 54)
            {
                skill.Name = new LocalText("Mist");
                skill.Desc = new LocalText("The user cloaks itself and its allies in a white mist that prevents any of their stats from being lowered for ten turns.");
                skill.BaseCharges = 18;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("mist", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(33);//Gas
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 6;
                CircleSquareSprinkleEmitter emitter = new CircleSquareSprinkleEmitter(new AnimData("Smoke_White", 3));
                emitter.ParticlesPerTile = 1.5;
                emitter.HeightSpeed = 10;
                emitter.SpeedDiff = 10;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Sound = "DUN_Pokemon_Trap";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Smoke_White", 3));
            }
            else if (ii == 55)
            {
                skill.Name = new LocalText("Water Gun");
                skill.Desc = new LocalText("The target is blasted with a forceful shot of water.");
                skill.BaseCharges = 19;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Hydro_Pump_RSE", 3));
                shotAnim.StartDistance = 16;
                shotAnim.Shots = 7;
                shotAnim.BurstTime = 5;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Water_Gun_2";
                FiniteReleaseEmitter endAnim = new FiniteReleaseEmitter(new AnimData("Hydro_Cannon", 5, 2, -1));
                endAnim.BurstTime = 2;
                endAnim.ParticlesPerBurst = 2;
                endAnim.Bursts = 4;
                endAnim.StartDistance = 4;
                endAnim.Speed = 60;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 56)
            {
                skill.Name = new LocalText("Hydro Pump");
                skill.Desc = new LocalText("The target is blasted by a huge volume of water launched under great pressure.");
                skill.BaseCharges = 8;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Puff_Brown", 3));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Transform", terrainEmitter, new FlagType(typeof(LavaTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Hydro_Pump_RSE", 3));
                shotAnim.StartDistance = 16;
                shotAnim.Shots = 7;
                shotAnim.BurstTime = 5;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Hydro_Pump";
                FiniteReleaseEmitter endAnim = new FiniteReleaseEmitter(new AnimData("Hydro_Cannon", 5, 1, -1));
                endAnim.BurstTime = 2;
                endAnim.ParticlesPerBurst = 2;
                endAnim.Bursts = 4;
                endAnim.StartDistance = 4;
                endAnim.Speed = 60;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 57)
            {
                skill.Name = new LocalText("Surf");
                skill.Desc = new LocalText("The user attacks everything around it by swamping its surroundings with a giant wave.");
                skill.BaseCharges = 9;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Puff_Brown", 3));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Transform", terrainEmitter, new FlagType(typeof(LavaTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 5;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.Wide;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Water_Column_Ranger", 3));
                emitter.LocHeight = 24;
                skill.HitboxAction.TileEmitter = emitter;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Surf";
                FiniteReleaseEmitter endAnim = new FiniteReleaseEmitter(new AnimData("Hydro_Cannon", 5, 2, -1));
                endAnim.BurstTime = 2;
                endAnim.ParticlesPerBurst = 2;
                endAnim.Bursts = 4;
                endAnim.StartDistance = 4;
                endAnim.Speed = 60;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 58)
            {
                skill.Name = new LocalText("Ice Beam");
                skill.Desc = new LocalText("The target is struck with an icy-cold beam of energy. This may also leave the target frozen.");
                skill.BaseCharges = 8;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("freeze", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).Speed = 12;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Power_Gem_Shot", 1, 0, 0));
                shotAnim.StartDistance = 16;
                shotAnim.Shots = 14;
                shotAnim.BurstTime = 3;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Ice_Beam";
                FiniteReleaseEmitter endAnim = new FiniteReleaseEmitter(new AnimData("Ice_Pieces", 6, 0, 0), new AnimData("Ice_Pieces", 12, 1, 1), new AnimData("Ice_Pieces", 12, 1, 1));
                endAnim.BurstTime = 2;
                endAnim.ParticlesPerBurst = 4;
                endAnim.Bursts = 4;
                endAnim.StartDistance = 4;
                endAnim.Speed = 60;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Ice_Beam_2";
            }
            else if (ii == 59)
            {
                skill.Name = new LocalText("Blizzard");
                skill.Desc = new LocalText("A howling blizzard is summoned to strike opposing Pokémon. This may also leave the opposing Pokémon frozen.");
                skill.BaseCharges = 6;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 70;
                skill.Data.SkillStates.Set(new BasePowerState(85));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnActions.Add(0, new WeatherNeededEvent("hail", new SetAccuracyEvent(-1)));
                skill.Data.OnActions.Add(0, new WeatherNeededEvent("sunny", new SetAccuracyEvent(50)));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("freeze", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("Blizzard", 3);
                overlay.TotalTime = 60;
                overlay.Movement = new RogueElements.Loc(-4, 3);
                overlay.Layer = DrawLayer.Top;
                skill.HitboxAction.ActionFX.Emitter = overlay;
                skill.HitboxAction.ActionFX.Sound = "DUN_Tailwind_2";
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                FiniteReleaseEmitter endAnim = new FiniteReleaseEmitter(new AnimData("Ice_Pieces", 6, 0, 0), new AnimData("Ice_Pieces", 12, 1, 1), new AnimData("Ice_Pieces", 12, 1, 1));
                endAnim.BurstTime = 2;
                endAnim.ParticlesPerBurst = 4;
                endAnim.Bursts = 4;
                endAnim.StartDistance = 4;
                endAnim.Speed = 60;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Ice_Beam_2";
            }
            else if (ii == 60)
            {
                skill.Name = new LocalText("Psybeam");
                skill.Desc = new LocalText("The target is attacked with a peculiar ray. This may also leave the target confused.");
                skill.BaseCharges = 12;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("confuse", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).Speed = 12;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Aurora_Beam_Custom", 3));
                shotAnim.StartDistance = 16;
                shotAnim.Shots = 12;
                shotAnim.BurstTime = 3;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Psybeam";
            }
            else if (ii == 61)
            {
                skill.Name = new LocalText("Bubble Beam");
                skill.Desc = new LocalText("A spray of bubbles is forcefully ejected at the target. This may also lower its Movement Speed.");
                skill.BaseCharges = 12;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_speed", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).Speed = 12;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Wind_Bubbles", 1, 2, 2));
                shotAnim.StartDistance = 16;
                shotAnim.Shots = 8;
                shotAnim.BurstTime = 3;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_BubbleBeam";
                FiniteReleaseEmitter endAnim = new FiniteReleaseEmitter(new AnimData("Bubbles_Blue", 3));
                endAnim.Speed = 38;
                endAnim.Bursts = 3;
                endAnim.BurstTime = 3;
                endAnim.ParticlesPerBurst = 2;
                endAnim.StartDistance = 4;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 62)
            {
                skill.Name = new LocalText("Aurora Beam");
                skill.Desc = new LocalText("The target is hit with a rainbow-colored beam. This may also lower the target's Attack stat.");
                skill.BaseCharges = 11;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_attack", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                AttachAreaEmitter emitter = new AttachAreaEmitter();
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Blue", 3, -1, -1, 192)));
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Green", 3, -1, -1, 192)));
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Pink", 3, -1, -1, 192)));
                emitter.BurstTime = 5;
                emitter.ParticlesPerBurst = 1;
                emitter.Range = GraphicsManager.TileSize / 2;
                ((ProjectileAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Aurora_Beam";
            }
            else if (ii == 63)
            {
                skill.Name = new LocalText("Hyper Beam");
                skill.Desc = new LocalText("The user attacks with a destructive beam. The user can't move on the next three turns.");
                skill.BaseCharges = 5;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Wall_Break", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", terrainEmitter, new FlagType(typeof(WallTerrainState))));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(4));
                skill.Data.AfterActions.Add(0, new StatusStateBattleEvent("paused", false, true, statusStates));
                skill.Strikes = 1;
                skill.HitboxAction = new WaveMotionAction();
                ((WaveMotionAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((WaveMotionAction)skill.HitboxAction).Range = 8;
                ((WaveMotionAction)skill.HitboxAction).Speed = 10;
                ((WaveMotionAction)skill.HitboxAction).Linger = 6;
                ((WaveMotionAction)skill.HitboxAction).Wide = true;
                ((WaveMotionAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.ActionFX.ScreenMovement = new ScreenMover(0, 8, 30);
                ((WaveMotionAction)skill.HitboxAction).Anim = new BeamAnimData("Beam", 2);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Protect";
            }
            else if (ii == 64)
            {
                skill.Name = new LocalText("Peck");
                skill.Desc = new LocalText("The target is jabbed with a sharply pointed beak or horn.");
                skill.BaseCharges = 24;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(45));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(20);//Jab
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Peck";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 65)
            {
                skill.Name = new LocalText("Drill Peck");
                skill.Desc = new LocalText("A corkscrewing attack with a sharp beak acting as a drill.");
                skill.BaseCharges = 15;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 20;//Jab
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                ((DashAction)skill.HitboxAction).AnimOffset = 12;
                ((DashAction)skill.HitboxAction).Anim = new AnimData("Fury_Attack", 2);
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Fury_Attack";
            }
            else if (ii == 66)
            {
                skill.Name = new LocalText("-Submission");
                skill.Desc = new LocalText("The user grabs the target and recklessly dives for the ground. This also damages the user a little.");
                skill.BaseCharges = 16;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 75;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new HPRecoilEvent(5, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 67)
            {
                skill.Name = new LocalText("Low Kick");
                skill.Desc = new LocalText("A powerful low kick that makes the target fall over. The heavier the target, the greater the move's power.");
                skill.BaseCharges = 20;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState());
                skill.Data.BeforeHits.Add(0, new WeightBasePowerEvent());
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new DropItemEvent(false, true, "", new HashSet<FlagType>(), new StringKey(), false)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(21);//Kick
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Dragon_Rush";
                SingleEmitter emitter = new SingleEmitter(new AnimData("Print_Foot", 12));
                emitter.LocHeight = -6;
                ((AttackAction)skill.HitboxAction).Emitter = emitter;
                skill.Data.HitFX.Sound = "DUN_Punch";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Hammer_Arm_Smoke", 3));
            }
            else if (ii == 68)
            {
                skill.Name = new LocalText("Counter");
                skill.Desc = new LocalText("A retaliation move that counters any physical attack, inflicting double the damage taken.");
                skill.BaseCharges = 15;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("counter", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Move_Start";
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 69)
            {
                skill.Name = new LocalText("Seismic Toss");
                skill.Desc = new LocalText("The target is thrown using the power of gravity. It inflicts damage equal to the user's level.");
                skill.BaseCharges = 20;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new ThrowBackEvent(2, new LevelDamageEvent(false, 1, 1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 70)
            {
                skill.Name = new LocalText("-Strength");
                skill.Desc = new LocalText("The target is slugged with a punch thrown at maximum power, pushing them back.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new KnockBackEvent(1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 71)
            {
                skill.Name = new LocalText("Absorb");
                skill.Desc = new LocalText("A nutrient-draining attack. The user's HP is restored by half the damage taken by the target.");
                skill.BaseCharges = 22;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(30));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new HPDrainEvent(2));
                skill.Data.SkillStates.Set(new HealState());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 5;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Absorb";
                FiniteGatherEmitter emitter = new FiniteGatherEmitter(new AnimData("Absorb", 3));
                emitter.ParticlesPerBurst = 1;
                emitter.Bursts = 1;
                emitter.TravelTime = 30;
                emitter.UseDest = true;
                skill.HitboxAction.TileEmitter = emitter;
            }
            else if (ii == 72)
            {
                skill.Name = new LocalText("Mega Drain");
                skill.Desc = new LocalText("A nutrient-draining attack. The user's HP is restored by half the damage taken by the target.");
                skill.BaseCharges = 16;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new HPDrainEvent(2));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 5;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Focus_Blast";
                FiniteGatherEmitter emitter = new FiniteGatherEmitter(new AnimData("Absorb", 3));
                emitter.ParticlesPerBurst = 1;
                emitter.Bursts = 1;
                emitter.TravelTime = 30;
                emitter.UseDest = true;
                skill.HitboxAction.TileEmitter = emitter;
            }
            else if (ii == 73)
            {
                skill.Name = new LocalText("Leech Seed");
                skill.Desc = new LocalText("A seed is planted on the target. It steals some HP from the target every turn.");
                skill.BaseCharges = 12;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 85;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("leech_seed", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 8;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Seed_RSE", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Leech_Seed";
                SqueezedAreaEmitter endAnim = new SqueezedAreaEmitter(new ParticleAnim(new AnimData("Leech_Seed_Sprout", 4), 6));
                endAnim.Range = 10;
                endAnim.StartHeight = 0;
                endAnim.Bursts = 3;
                endAnim.BurstTime = 4;
                endAnim.ParticlesPerBurst = 2;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Leech_Seed_2";
            }
            else if (ii == 74)
            {
                skill.Name = new LocalText("Growth");
                skill.Desc = new LocalText("The user's body grows all at once, raising the Attack and Sp. Atk stats.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new WeatherStackEvent("mod_attack", true, false, "sunny"));
                skill.Data.OnHits.Add(0, new WeatherStackEvent("mod_special_attack", true, false, "sunny"));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Growth_2";
                preFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Green_Out", 2));
                skill.HitboxAction.PreActions.Add(preFX);
                ((SelfAction)skill.HitboxAction).LagBehindTime = 10;
            }
            else if (ii == 75)
            {
                skill.Name = new LocalText("Razor Leaf");
                skill.Desc = new LocalText("Sharp-edged leaves are launched to slash at the opposing Pokémon. Critical hits land more easily.");
                skill.BaseCharges = 12;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(55));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", terrainEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Razor_Leaf", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Razor_Leaf";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Razor_Leaf_Charge", 2));
                endAnim.LocHeight = 20;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 76)
            {
                skill.Name = new LocalText("Solar Beam");
                skill.Desc = new LocalText("A two-turn attack. The user gathers light, then blasts a bundled beam on the next turn.");
                skill.BaseCharges = 8;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.BeforeTryActions.Add(-1, new WeatherNeededEvent("sunny", new AddContextStateEvent(new MoveCharge())));
                SelfAction altAction = new SelfAction();
                altAction.CharAnimData = new CharAnimFrameType(06);//Charge
                altAction.TargetAlignments |= Alignment.Self;
                altAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Solar_Beam_Charge", 1));
                altAction.ActionFX.Sound = "DUN_HP_Drain_2";
                skill.Data.BeforeTryActions.Add(0, new ChargeOrReleaseEvent("charging_solar_beam", altAction));
                skill.Data.OnActions.Add(0, new WeatherNeededEvent("rain", new MultiplyDamageEvent(2, 3)));
                skill.Data.OnActions.Add(0, new WeatherNeededEvent("sandstorm", new MultiplyDamageEvent(2, 3)));
                skill.Data.OnActions.Add(0, new WeatherNeededEvent("hail", new MultiplyDamageEvent(2, 3)));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Wall_Break", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", terrainEmitter, new FlagType(typeof(WallTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new WaveMotionAction();
                ((WaveMotionAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((WaveMotionAction)skill.HitboxAction).Range = 8;
                ((WaveMotionAction)skill.HitboxAction).Speed = 10;
                ((WaveMotionAction)skill.HitboxAction).Linger = 6;
                ((WaveMotionAction)skill.HitboxAction).Wide = true;
                ((WaveMotionAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.ActionFX.ScreenMovement = new ScreenMover(0, 8, 30);
                ((WaveMotionAction)skill.HitboxAction).Anim = new BeamAnimData("Beam_2", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.HitboxAction.ActionFX.Sound = "DUN_Protect";
            }
            else if (ii == 77)
            {
                skill.Name = new LocalText("Poison Powder");
                skill.Desc = new LocalText("The user scatters a cloud of poisonous dust that poisons the target.");
                skill.BaseCharges = 16;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 50;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("poison", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(19);//Shake
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 5;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 30;
                CircleSquareSprinkleEmitter emitter = new CircleSquareSprinkleEmitter(new AnimData("Spores_RSE_Purple", 6), 0, 60);
                emitter.ParticlesPerTile = 3;
                emitter.StartHeight = 24;
                emitter.HeightSpeed = -15;
                emitter.SpeedDiff = 15;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Stun_Spore";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Puff_Purple", 3));
            }
            else if (ii == 78)
            {
                skill.Name = new LocalText("Stun Spore");
                skill.Desc = new LocalText("The user scatters a cloud of numbing powder that paralyzes the target.");
                skill.BaseCharges = 11;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 50;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("paralyze", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(19);//Shake
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 5;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 30;
                CircleSquareSprinkleEmitter emitter = new CircleSquareSprinkleEmitter(new AnimData("Spores_RSE_Yellow", 6), 0, 60);
                emitter.ParticlesPerTile = 3;
                emitter.StartHeight = 24;
                emitter.HeightSpeed = -15;
                emitter.SpeedDiff = 15;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Stun_Spore";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Puff_Brown", 3));
            }
            else if (ii == 79)
            {
                skill.Name = new LocalText("Sleep Powder");
                skill.Desc = new LocalText("The user scatters a big cloud of sleep-inducing dust around the target.");
                skill.BaseCharges = 10;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 50;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("sleep", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(19);//Shake
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 5;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 30;
                CircleSquareSprinkleEmitter emitter = new CircleSquareSprinkleEmitter(new AnimData("Spores_RSE_Green", 6), 0, 60);
                emitter.ParticlesPerTile = 3;
                emitter.StartHeight = 24;
                emitter.HeightSpeed = -15;
                emitter.SpeedDiff = 15;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Stun_Spore";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Puff_Green", 3));
            }
            else if (ii == 80)
            {
                skill.Name = new LocalText("Petal Dance");
                skill.Desc = new LocalText("The user attacks the target by scattering petals for three turns. The user then becomes confused.");
                skill.BaseCharges = 12;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new StatusBattleEvent("petal_dance", false, true));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(27);//Twirl
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Petal_Dance_Back", 1), new AnimData("Petal_Dance_Front", 1));
                emitter.HeightBack = 32;
                emitter.HeightFront = 32;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Petal_Dance";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 81)
            {
                skill.Name = new LocalText("String Shot");
                skill.Desc = new LocalText("The opposing Pokémon are bound with silk blown from the user's mouth that lowers Movement Speed.");
                skill.BaseCharges = 15;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, -1));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 12;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("String_Shot_Ball", 3, 0, 0));
                shotAnim.StartDistance = 16;
                shotAnim.Shots = 14;
                shotAnim.BurstTime = 2;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_String_Shot";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("String_Shot_Web", 3));
                skill.Data.HitFX.Sound = "DUN_String_Shot_2";
            }
            else if (ii == 82)
            {
                skill.Name = new LocalText("Dragon Rage");
                skill.Desc = new LocalText("This attack hits the target with a shock wave of pure rage. This attack inflicts damage equal to the amount of HP the user is missing.");
                skill.BaseCharges = 12;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(-1, new UserHPDamageEvent(true));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 2;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                AttachAreaEmitter emitter = new AttachAreaEmitter(new AnimData("Blast_Burn", 3));
                emitter.BurstTime = 2;
                emitter.ParticlesPerBurst = 1;
                emitter.Range = 12;
                emitter.AddHeight = 8;
                ((ProjectileAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Sacred_Fire";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Flamethrower", 3));
                endAnim.LocHeight = 14;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 83)
            {
                skill.Name = new LocalText("Fire Spin");
                skill.Desc = new LocalText("The target becomes trapped within a fierce vortex of fire that rages for three turns.");
                skill.BaseCharges = 14;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new StatusBattleEvent("fire_spin", true, true)));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((OffsetAction)skill.HitboxAction).Range = 3;
                ((OffsetAction)skill.HitboxAction).Speed = 10;
                ((OffsetAction)skill.HitboxAction).LagBehindTime = 30;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Fire_Spin_Back", 1), new AnimData("Fire_Spin_Front", 1));
                emitter.HeightBack = 32;
                emitter.HeightFront = 32;
                skill.HitboxAction.TileEmitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Fire_Spin";
            }
            else if (ii == 84)
            {
                skill.Name = new LocalText("Thunder Shock");
                skill.Desc = new LocalText("A jolt of electricity crashes down on the target to inflict damage. This may also leave the target with paralysis.");
                skill.BaseCharges = 20;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(30));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("paralyze", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(34);//Shock
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Move_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                SingleEmitter emitter = new SingleEmitter(new BeamAnimData("Lightning", 3));
                emitter.LocHeight = 8;
                skill.Explosion.ExplodeFX.Emitter = emitter;
                skill.Explosion.ExplodeFX.Sound = "DUN_Thunder_Shock";
            }
            else if (ii == 85)
            {
                skill.Name = new LocalText("Thunderbolt");
                skill.Desc = new LocalText("A strong electric blast crashes down on the target. This may also leave the target with paralysis.");
                skill.BaseCharges = 14;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(85));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("paralyze", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(34);//Shock
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 8;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Move_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Sound = "DUN_Shock_Wave";
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter();
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Lightning", 3)));
                emitter.LocHeight = 8;
                emitter.ParticlesPerTile = 0.7;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
            }
            else if (ii == 86)
            {
                skill.Name = new LocalText("Thunder Wave");
                skill.Desc = new LocalText("The user launches a weak jolt of electricity that paralyzes the target.");
                skill.BaseCharges = 14;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("paralyze", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 2;
                ((ProjectileAction)skill.HitboxAction).Speed = 8;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Wave_Circle_Yellow", 1));
                shotAnim.StartDistance = 16;
                shotAnim.Shots = 3;
                shotAnim.BurstTime = 4;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Zap_Cannon";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Spark", 3));
            }
            else if (ii == 87)
            {
                skill.Name = new LocalText("-Thunder");
                skill.Desc = new LocalText("A wicked thunderbolt is dropped on the target to inflict damage. This may also leave the target with paralysis.");
                skill.BaseCharges = 12;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 70;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnActions.Add(0, new WeatherNeededEvent("rain", new SetAccuracyEvent(-1)));
                skill.Data.OnActions.Add(0, new WeatherNeededEvent("sunny", new SetAccuracyEvent(50)));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("paralyze", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(34);//Shock
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                skill.HitboxAction.ActionFX.Sound = "DUN_Shock_Wave";
                skill.HitboxAction.TileEmitter = new SingleEmitter(new BeamAnimData("Lightning_RSE", 2));
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 88)
            {
                skill.Name = new LocalText("Rock Throw");
                skill.Desc = new LocalText("The user picks up and throws a small rock at the target to attack.");
                skill.BaseCharges = 15;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(55));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Rock_Pieces", 3, 5, 5);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Rock_Throw";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Bonemarang_Hit", 3));
            }
            else if (ii == 89)
            {
                skill.Name = new LocalText("Earthquake");
                skill.Desc = new LocalText("The user sets off an earthquake that strikes every Pokémon around it.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHitTiles.Add(0, new RemoveTrapEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(31);//Rumble
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.ActionFX.ScreenMovement = new ScreenMover(0, 12, 30);
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Trapbust_Orb";
                skill.Data.HitFX.Sound = "DUN_Trapbust_Orb";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Green_Out", 3));
            }
            else if (ii == 90)
            {
                skill.Name = new LocalText("Fissure");
                skill.Desc = new LocalText("The user opens up a fissure in the ground and drops foes in. The target faints instantly if it hits.");
                skill.BaseCharges = 5;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 50;
                skill.Data.BeforeHits.Add(0, new ExplorerImmuneEvent());
                skill.Data.OnHits.Add(-1, new OHKODamageEvent());
                skill.Data.OnHitTiles.Add(0, new RemoveTrapEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(31);//Rumble
                ((ProjectileAction)skill.HitboxAction).Range = 3;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.ActionFX.ScreenMovement = new ScreenMover(0, 8, 30);
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "EVT_CH20_Tower_Cracks";
                SingleEmitter emitter = new SingleEmitter(new AnimData("Fissure", 2));
                emitter.Layer = DrawLayer.Bottom;
                skill.HitboxAction.TileEmitter = emitter;
            }
            else if (ii == 91)
            {
                skill.Name = new LocalText("Dig");
                skill.Desc = new LocalText("The user burrows, then attacks any time in the next 5 turns.");
                skill.BaseCharges = 14;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                SelfAction altAction = new SelfAction();
                altAction.CharAnimData = new CharAnimFrameType(43);//Hop
                altAction.TargetAlignments |= Alignment.Self;
                SingleEmitter altAnim = new SingleEmitter(new AnimData("Dig", 3));
                altAnim.LocHeight = 16;
                altAction.ActionFX.Emitter = altAnim;
                altAction.ActionFX.Sound = "DUN_Dig";
                skill.Data.BeforeTryActions.Add(0, new ChargeOrReleaseEvent("digging", altAction));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHitTiles.Add(0, new RemoveTrapEvent());
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.FrontAndCorners;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                SingleEmitter anim = new SingleEmitter(new AnimData("Dig", 3));
                anim.LocHeight = 16;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = anim;
                preFX.Sound = "DUN_Dig";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 92)
            {
                skill.Name = new LocalText("Toxic");
                skill.Desc = new LocalText("A move that leaves the target badly poisoned. Its poison damage worsens every turn.");
                skill.BaseCharges = 14;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 85;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("poison_toxic", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 2;
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Shadow_Force_Back", 2), new AnimData("Shadow_Force_Front", 2));
                emitter.HeightBack = 4;
                emitter.HeightFront = 4;
                skill.HitboxAction.TileEmitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Toxic";
                skill.Data.HitFX.Delay = 40;
            }
            else if (ii == 93)
            {
                skill.Name = new LocalText("Confusion");
                skill.Desc = new LocalText("The target is hit by a weak telekinetic force. This may also confuse the target.");
                skill.BaseCharges = 20;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("confuse", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((OffsetAction)skill.HitboxAction).Range = 3;
                ((OffsetAction)skill.HitboxAction).Speed = 10;
                ((OffsetAction)skill.HitboxAction).LagBehindTime = 5;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                preFX.Sound = "DUN_Move_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                RepeatEmitter emitter = new RepeatEmitter(new AnimData("Circle_Thick_Red_Out", 2));
                emitter.Bursts = 3;
                emitter.BurstTime = 8;
                skill.HitboxAction.TileEmitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Confusion";
            }
            else if (ii == 94)
            {
                skill.Name = new LocalText("Psychic");
                skill.Desc = new LocalText("The target is hit by a strong telekinetic force. This may also lower the target's Sp. Def stat.");
                skill.BaseCharges = 9;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_special_defense", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((OffsetAction)skill.HitboxAction).HitArea = OffsetAction.OffsetArea.Area;
                ((OffsetAction)skill.HitboxAction).Range = 3;
                ((OffsetAction)skill.HitboxAction).Speed = 10;
                ((OffsetAction)skill.HitboxAction).LagBehindTime = 5;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("Psychic", 3, -1, -1, 128);
                overlay.TotalTime = 60;
                overlay.Movement = new Loc(0, 0);
                overlay.Layer = DrawLayer.Bottom;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = overlay;
                preFX.Sound = "DUN_Confusion";
                skill.HitboxAction.PreActions.Add(preFX);
                RepeatEmitter emitter = new RepeatEmitter(new AnimData("Circle_Thick_Red_Out", 2));
                emitter.Bursts = 3;
                emitter.BurstTime = 8;
                skill.HitboxAction.TileEmitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Move_Start";
            }
            else if (ii == 95)
            {
                skill.Name = new LocalText("Hypnosis");
                skill.Desc = new LocalText("The user employs hypnotic suggestion to make the target fall into a deep sleep.");
                skill.BaseCharges = 16;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 50;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("sleep", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Wave_Circle_Purple", 3));
                shotAnim.StartDistance = 16;
                shotAnim.Shots = 4;
                shotAnim.BurstTime = 10;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("Psychic", 3, -1, -1, 128);
                overlay.TotalTime = 60;
                overlay.Movement = new RogueElements.Loc(0, 0);
                overlay.Layer = DrawLayer.Bottom;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = overlay;
                preFX.Sound = "DUN_Move_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Psybeam";
                RepeatEmitter endAnim = new RepeatEmitter(new AnimData("Circle_Thick_Red_Out", 2));
                endAnim.Bursts = 3;
                endAnim.BurstTime = 10;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 96)
            {
                skill.Name = new LocalText("Meditate");
                skill.Desc = new LocalText("The user meditates to awaken the power deep within its body and raise its Attack stat.");
                skill.BaseCharges = 20;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("Psychic", 3, -1, -1, 128);
                overlay.TotalTime = 60;
                overlay.Movement = new RogueElements.Loc(0, 0);
                overlay.Layer = DrawLayer.Bottom;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = overlay;
                preFX.Sound = "DUN_Move_Start";
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 97)
            {
                skill.Name = new LocalText("=Agility");
                skill.Desc = new LocalText("The user relaxes and lightens the body to raise Movement Speed.");
                skill.BaseCharges = 15;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(41);//Double
                ((AreaAction)skill.HitboxAction).Range = 3;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Double_Team";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Sound = "DUN_Identify_2";
            }
            else if (ii == 98)
            {
                skill.Name = new LocalText("Quick Attack");
                skill.Desc = new LocalText("The user lunges at the target at a speed that makes it almost invisible.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = false;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                AfterImageEmitter emitter = new AfterImageEmitter();
                emitter.Alpha = 128;
                emitter.AnimTime = 8;
                emitter.BurstTime = 1;
                ((DashAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Move_Start";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 99)
            {
                skill.Name = new LocalText("Rage");
                skill.Desc = new LocalText("As long as this move is in use, the power of rage raises the Attack stat each time the user is hit in battle.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("enraged", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Rage", 2));
                endAnim.Layer = DrawLayer.Back;
                endAnim.LocHeight = 44;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Rage";
            }
            else if (ii == 100)
            {
                skill.Name = new LocalText("Teleport");
                skill.Desc = new LocalText("The user warps itself and nearby allies to a random location on the floor.");
                skill.BaseCharges = 18;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new RandomGroupWarpEvent(80, true));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                preFX.Sound = "DUN_Move_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Red_Out", 3));
            }
            else if (ii == 101)
            {
                skill.Name = new LocalText("Night Shade");
                skill.Desc = new LocalText("The user makes the target see a frightening mirage. It inflicts damage equal to the user's level.");
                skill.BaseCharges = 15;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(-1, new LevelDamageEvent(false, 1, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TileEmitter = new SingleEmitter(new AnimData("Dark_Pulse_Ranger", 3));
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Night_Shade_3";
            }
            else if (ii == 102)
            {
                skill.Name = new LocalText("=Mimic");
                skill.Desc = new LocalText("The user copies the target's last move. The move can be used during battle until the next floor is reached.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new MimicBattleEvent("last_used_move", 5));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 4;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
            }
            else if (ii == 103)
            {
                skill.Name = new LocalText("Screech");
                skill.Desc = new LocalText("An earsplitting screech harshly lowers the target's Defense stat.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 85;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, -2));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Wave_Circle_Green", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Screech";
            }
            else if (ii == 104)
            {
                skill.Name = new LocalText("Double Team");
                skill.Desc = new LocalText("By moving rapidly, the user makes illusory copies of itself to raise its evasiveness.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_evasion", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(41);//Double
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Double_Team";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 105)
            {
                skill.Name = new LocalText("Recover");
                skill.Desc = new LocalText("Restoring its own cells, the user restores its own HP by half of its max HP.");
                skill.BaseCharges = 12;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new RestoreHPEvent(1, 2, true));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                FiniteGatherEmitter endAnim = new FiniteGatherEmitter(new AnimData("Circle_Tiny_Yellow", 3, 4, -1));
                endAnim.BurstTime = 2;
                endAnim.ParticlesPerBurst = 1;
                endAnim.Bursts = 8;
                endAnim.TravelTime = 16;
                endAnim.EndDistance = 4;
                endAnim.StartDistance = 24;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = endAnim;
                preFX.Sound = "DUN_Absorb";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 106)
            {
                skill.Name = new LocalText("Harden");
                skill.Desc = new LocalText("The user stiffens all the muscles in its body to raise its Defense stat.");
                skill.BaseCharges = 21;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_White_In", 2));
                preFX.Sound = "DUN_Harden";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 107)
            {
                skill.Name = new LocalText("Minimize");
                skill.Desc = new LocalText("The user compresses its body to make itself look smaller, which sharply raises its evasiveness.");
                skill.BaseCharges = 12;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_evasion", true, false, 2));
                skill.Data.OnHits.Add(0, new StatusBattleEvent("minimized", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Green_In", 2));
                preFX.Sound = "DUN_Wing_Attack";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 108)
            {
                skill.Name = new LocalText("Smokescreen");
                skill.Desc = new LocalText("The user releases an obscuring cloud of smoke or ink. This lowers the target's Accuracy.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_accuracy", true, false, -1));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Puff_Black", 3));
                shotAnim.StartDistance = 8;
                shotAnim.Shots = 7;
                shotAnim.BurstTime = 6;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Smokescreen";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Puff_Black", 3));
            }
            else if (ii == 109)
            {
                skill.Name = new LocalText("Confuse Ray");
                skill.Desc = new LocalText("The target is exposed to a sinister ray that triggers confusion.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("confuse", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Confuse_Ray", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Move_Start";
                FiniteReleaseRangeEmitter endAnim = new FiniteReleaseRangeEmitter(new AnimData("Confuse_Ray", 3));
                endAnim.Range = GraphicsManager.TileSize * 3 / 2;
                endAnim.Speed = 64;
                endAnim.Bursts = 6;
                endAnim.BurstTime = 6;
                endAnim.ParticlesPerBurst = 1;
                endAnim.StartDistance = 4;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Confuse_Ray";
            }
            else if (ii == 110)
            {
                skill.Name = new LocalText("Withdraw");
                skill.Desc = new LocalText("The user withdraws its body into its hard shell, raising its Defense stat.");
                skill.BaseCharges = 22;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Clamp";
                ClampEmitter emitter = new ClampEmitter();
                emitter.Anim1 = new AnimData("Withdraw", 3, RogueElements.Dir8.Right);
                emitter.Anim2 = new AnimData("Withdraw", 3, RogueElements.Dir8.Left);
                emitter.HalfOffset = new RogueElements.Loc(-16, 0);
                emitter.LingerStart = 2;
                emitter.LingerEnd = 4;
                emitter.MoveTime = 12;
                skill.Data.HitFX.Emitter = emitter;
                skill.Data.HitFX.Delay = 10;
            }
            else if (ii == 111)
            {
                skill.Name = new LocalText("Defense Curl");
                skill.Desc = new LocalText("The user curls up to conceal weak spots and raise its Defense stat.");
                skill.BaseCharges = 21;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, 1));
                skill.Data.OnHits.Add(0, new StatusBattleEvent("defense_curl", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Attack";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Defense_Curl", 2), 2);
                endAnim.LocHeight = 2;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Delay = 32;
            }
            else if (ii == 112)
            {
                skill.Name = new LocalText("Barrier");
                skill.Desc = new LocalText("The user throws up a sturdy wall that sharply raises the party's Defense stat.");
                skill.BaseCharges = 18;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, 2));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Sound = "DUN_Light_Screen";
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 3, -1, -1, 192));
            }
            else if (ii == 113)
            {
                skill.Name = new LocalText("Light Screen");
                skill.Desc = new LocalText("A wondrous wall of light is put up to reduce damage from special attacks for ten turns.");
                skill.BaseCharges = 15;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("light_screen", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Sound = "DUN_Light_Screen_2";
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Screen_RSE_Yellow", 3, -1, -1, 192));
            }
            else if (ii == 114)
            {
                skill.Name = new LocalText("Haze");
                skill.Desc = new LocalText("The user creates a haze that eliminates every stat change.");
                skill.BaseCharges = 15;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new RemoveStateStatusBattleEvent(typeof(StatChangeState), true, new StringKey("MSG_BUFF_REMOVE")));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(33);//Gas
                ((AreaAction)skill.HitboxAction).Range = 5;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Foe | Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Foe | Alignment.Self | Alignment.Friend);
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("Haze", 3, 0, 0, 128);
                overlay.TotalTime = 60;
                overlay.Movement = new RogueElements.Loc(1, 0);
                overlay.Layer = DrawLayer.Top;
                skill.HitboxAction.ActionFX.Emitter = overlay;
                skill.HitboxAction.ActionFX.Sound = "DUN_Foggy";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Smoke_Black", 3));
            }
            else if (ii == 115)
            {
                skill.Name = new LocalText("Reflect");
                skill.Desc = new LocalText("A wondrous wall of light is put up to reduce damage from physical attacks for ten turns.");
                skill.BaseCharges = 18;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("reflect", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Sound = "DUN_Light_Screen";
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Screen_RSE_Blue", 3, -1, -1, 192));
            }
            else if (ii == 116)
            {
                skill.Name = new LocalText("Focus Energy");
                skill.Desc = new LocalText("The user takes a deep breath and focuses so that critical hits land more easily.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("focus_energy", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Wing_Attack";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
            }
            else if (ii == 117)
            {
                skill.Name = new LocalText("Bide");
                skill.Desc = new LocalText("The user endures attacks for two turns, then strikes back to cause double the damage taken.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new BasePowerState());
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = -1;
                SingleEmitter altAnim = new SingleEmitter(new AnimData("Circle_Small_White_In", 2));
                skill.Data.BeforeTryActions.Add(0, new BideOrReleaseEvent("biding", altAnim, "DUN_Bide"));
                skill.Data.OnHits.Add(-1, new BasePowerDamageEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Wide;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Gravity";
            }
            else if (ii == 118)
            {
                skill.Name = new LocalText("Metronome");
                skill.Desc = new LocalText("The user waggles a finger and stimulates its brain into randomly using nearly any move.");
                skill.BaseCharges = 25;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new RandomMoveEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Metronome", 2));
                emitter.LocHeight = 24;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Metronome";
                preFX.Delay = 50;
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 119)
            {
                skill.Name = new LocalText("-Mirror Move");
                skill.Desc = new LocalText("The user counters the target by mimicking the last move used on it.");
                skill.BaseCharges = 22;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new MirrorMoveEvent("last_move_hit_by_other"));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
            }
            else if (ii == 120)
            {
                skill.Name = new LocalText("Self-Destruct");
                skill.Desc = new LocalText("The user attacks everything around it by causing an explosion. The user takes some damage too.");
                skill.BaseCharges = 5;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                {
                    BattleData newData = new BattleData();
                    newData.Element = "normal";
                    newData.Category = BattleData.SkillCategory.Physical;
                    newData.HitRate = -1;
                    newData.OnHits.Add(-1, new CutHPDamageEvent());
                    newData.ElementEffects.Add(0, new NormalizeEvent());
                    newData.OnHitTiles.Add(0, new RemoveItemEvent(true));
                    newData.OnHitTiles.Add(0, new RemoveTrapEvent());
                    newData.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState))));
                    SingleEmitter cuttingEmitter2 = new SingleEmitter(new AnimData("Grass_Clear", 2));
                    newData.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter2, new FlagType(typeof(FoliageTerrainState))));
                    skill.Data.BeforeHits.Add(-5, new AlignmentDifferentEvent(Alignment.Self, newData));
                }

                skill.Data.OnHits.Add(-1, new CutHPDamageEvent());
                skill.Data.OnHitTiles.Add(0, new RemoveItemEvent(true));
                skill.Data.OnHitTiles.Add(0, new RemoveTrapEvent());
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState))));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.Explosion.Range = 1;
                skill.Explosion.HitTiles = true;
                skill.Explosion.Speed = 10;
                skill.Explosion.ExplodeFX.Sound = "DUN_Self-Destruct";
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Explosion", 3));
                emitter.ParticlesPerTile = 0.7;
                skill.Explosion.Emitter = emitter;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(39);//Swell
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Self-Destruct";
            }
            else if (ii == 121)
            {
                skill.Name = new LocalText("Egg Bomb");
                skill.Desc = new LocalText("A large egg is hurled at the target with maximum force to inflict damage.");
                skill.BaseCharges = 11;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 75;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHitTiles.Add(0, new RemoveItemEvent(true));
                skill.Data.OnHitTiles.Add(0, new RemoveTrapEvent());
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState))));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.Explosion.Range = 1;
                skill.Explosion.HitTiles = true;
                skill.Explosion.Speed = 10;
                skill.Explosion.ExplodeFX.Sound = "DUN_Self-Destruct";
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Blast_Seed", 3));
                emitter.ParticlesPerTile = 0.8;
                skill.Explosion.Emitter = emitter;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Egg_Bomb_Egg", 2);
                skill.HitboxAction.ActionFX.Sound = "DUN_Attack";
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 122)
            {
                skill.Name = new LocalText("Lick");
                skill.Desc = new LocalText("The target is licked with a long tongue. This may also leave the target with paralysis.");
                skill.BaseCharges = 20;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("paralyze", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(22);//Lick
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Lick", 3));
                emitter.LocHeight = 2;
                ((AttackAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Absorb";
            }
            else if (ii == 123)
            {
                skill.Name = new LocalText("Smog");
                skill.Desc = new LocalText("The target is attacked with a discharge of filthy gases. This may also poison the target.");
                skill.BaseCharges = 16;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("poison", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(33);//Gas
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Smoke_Brown", 3));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 2;
                emitter.Bursts = 4;
                emitter.StartDistance = 4;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Mist";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Smoke_Brown", 3));
            }
            else if (ii == 124)
            {
                skill.Name = new LocalText("-Sludge");
                skill.Desc = new LocalText("Unsanitary sludge is hurled at the target. This may also poison the target.");
                skill.BaseCharges = 16;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(55));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("poison", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 2;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TileEmitter = new SingleEmitter(new AnimData("Puff_Purple", 3));
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_BubbleBeam";
            }
            else if (ii == 125)
            {
                skill.Name = new LocalText("Bone Club");
                skill.Desc = new LocalText("The user clubs the target with a bone. This may also make the target flinch.");
                skill.BaseCharges = 18;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(65));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(08);//Strike
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Attack";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Bonemarang_Hit", 3));
                skill.Data.HitFX.Sound = "DUN_Punch";
            }
            else if (ii == 126)
            {
                skill.Name = new LocalText("Fire Blast");
                skill.Desc = new LocalText("The target is attacked with an intense blast of all-consuming fire. This may also leave the target with a burn.");
                skill.BaseCharges = 9;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("burn", true, true)));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 2;
                ((ProjectileAction)skill.HitboxAction).Speed = 5;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Rays = ProjectileAction.RayCount.Five;
                skill.HitboxAction.ActionFX.Sound = "DUN_Fire_Blast_2";
                AttachAreaEmitter emitter = new AttachAreaEmitter(new AnimData("Fire_Blast", 4));
                emitter.BurstTime = 2;
                emitter.ParticlesPerBurst = 1;
                emitter.Range = 10;
                emitter.AddHeight = 12;
                ((ProjectileAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Flamethrower", 3));
                endAnim.LocHeight = 14;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Flamethrower_3";
            }
            else if (ii == 127)
            {
                skill.Name = new LocalText("Waterfall");
                skill.Desc = new LocalText("The user charges at the target with enough force to climb a waterfall. This may also make the target flinch.");
                skill.BaseCharges = 15;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Waterfall";
                SingleEmitter tileAnim = new SingleEmitter(new AnimData("Aqua_Tail_Splash", 3));
                tileAnim.LocHeight = 8;
                skill.HitboxAction.TileEmitter = tileAnim;
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Water_Column_Ranger", 3));
                endAnim.LocHeight = 24;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Aqua_Tail_2";
            }
            else if (ii == 128)
            {
                skill.Name = new LocalText("Clamp");
                skill.Desc = new LocalText("The target is clamped and squeezed by the user's very thick and sturdy shell for three turns.");
                skill.BaseCharges = 14;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(20));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new GiveContinuousDamageEvent("clamp", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Clamp", 2));
                ((AttackAction)skill.HitboxAction).LagBehindTime = 10;
                skill.HitboxAction.ActionFX.Sound = "DUN_Clamp";
            }
            else if (ii == 129)
            {
                skill.Name = new LocalText("Swift");
                skill.Desc = new LocalText("Star-shaped rays are shot at the opposing Pokémon. This attack never misses.");
                skill.BaseCharges = 14;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Swift_RSE", 1, 0, 0), new AnimData("Swift_RSE", 1, 1, 1), new AnimData("Swift_RSE", 1, 2, 2), new AnimData("Swift_RSE", 1, 2, 2));
                emitter.BurstTime = 3;
                emitter.ParticlesPerBurst = 3;
                emitter.Bursts = 8;
                emitter.StartDistance = 4;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Swift";
                FiniteReleaseRangeEmitter endAnim = new FiniteReleaseRangeEmitter(new AnimData("Swift_RSE", 1, 0, 0), new AnimData("Swift_RSE", 1, 1, 1), new AnimData("Swift_RSE", 1, 2, 2));
                endAnim.Range = GraphicsManager.TileSize * 3 / 2;
                endAnim.Speed = 64;
                endAnim.Bursts = 3;
                endAnim.BurstTime = 3;
                endAnim.ParticlesPerBurst = 2;
                endAnim.StartDistance = 4;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 130)
            {
                skill.Name = new LocalText("Skull Bash");
                skill.Desc = new LocalText("The user tucks in its head to raise its Defense in the first turn, then rams the target on the next turn.");
                skill.BaseCharges = 14;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                SelfAction altAction = new SelfAction();
                altAction.CharAnimData = new CharAnimFrameType(06);//Charge
                altAction.TargetAlignments |= Alignment.Self;
                altAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                altAction.ActionFX.Sound = "DUN_Move_Start";
                skill.Data.BeforeTryActions.Add(0, new ChargeOrReleaseEvent("charging_skull_bash", altAction));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                skill.HitboxAction.ActionFX.Sound = "DUN_Tackle";
                ((DashAction)skill.HitboxAction).Range = 6;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Brave_Bird", 3));
            }
            else if (ii == 131)
            {
                skill.Name = new LocalText("Spike Cannon");
                skill.Desc = new LocalText("Sharp spikes are shot at the target in rapid succession. They hit five times in a row.");
                skill.BaseCharges = 12;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 75;
                skill.Data.SkillStates.Set(new BasePowerState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 5;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 16;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Spike_Cannon", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Throw_Spike";
            }
            else if (ii == 132)
            {
                skill.Name = new LocalText("Constrict");
                skill.Desc = new LocalText("The target is attacked with long, creeping tentacles or vines. This will also lower the target's Movement Speed.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(35));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_speed", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Wrap_Green_Back", 3), new AnimData("Wrap_Green_Front", 3));
                emitter.HeightBack = 16;
                emitter.HeightFront = 16;
                ((AttackAction)skill.HitboxAction).Emitter = emitter;
                ((AttackAction)skill.HitboxAction).LagBehindTime = 30;
                skill.HitboxAction.ActionFX.Sound = "DUN_Wrap";
            }
            else if (ii == 133)
            {
                skill.Name = new LocalText("Amnesia");
                skill.Desc = new LocalText("The user temporarily empties its mind to forget its concerns. This sharply raises the user's Sp. Def stat.");
                skill.BaseCharges = 20;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_defense", true, false, 2));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Nasty_Plot_Think", 3));
                emitter.LocHeight = 20;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Curse";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.Data.HitFX.Delay = 40;
            }
            else if (ii == 134)
            {
                skill.Name = new LocalText("Kinesis");
                skill.Desc = new LocalText("The user distracts the target by bending a spoon. This lowers the target's Accuracy.");
                skill.BaseCharges = 18;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 75;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_accuracy", true, false, -1));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Kinesis";
            }
            else if (ii == 135)
            {
                skill.Name = new LocalText("=Soft-Boiled");
                skill.Desc = new LocalText("The user restores an ally's HP by up to half of its max HP.");
                skill.BaseCharges = 14;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new RestoreHPEvent(1, 2, true));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 4;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Soft_Boiled", 3, 0, 4);
                skill.HitboxAction.TargetAlignments = Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Friend;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Soft_Boiled", 2));
                skill.Data.HitFX.Delay = 20;
            }
            else if (ii == 136)
            {
                skill.Name = new LocalText("=High Jump Kick");
                skill.Desc = new LocalText("The target is attacked with a knee kick from a jump. If it misses, the user is hurt instead.");
                skill.BaseCharges = 8;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 75;
                skill.Data.SkillStates.Set(new BasePowerState(95));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new CrashLandEvent(4));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 21;//Kick
                ((DashAction)skill.HitboxAction).Range = 4;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Punch";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Stat_Green_Line", 3));
            }
            else if (ii == 137)
            {
                skill.Name = new LocalText("Glare");
                skill.Desc = new LocalText("The user intimidates the target with a snake glare to cause paralysis.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("paralyze", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).Speed = 20;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Leer", 2));
                preFX.Sound = "DUN_Leer_2";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Scary_Face", 3));
                skill.Data.HitFX.Sound = "DUN_Night_Shade";
                skill.Data.HitFX.Delay = 10;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 138)
            {
                skill.Name = new LocalText("Dream Eater");
                skill.Desc = new LocalText("The user eats the dreams of a target, doing more damage if it is sleeping. It absorbs half the damage caused to heal its own HP.");
                skill.BaseCharges = 16;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.BeforeHits.Add(0, new StatusPowerEvent("sleep", true));
                skill.Data.AfterActions.Add(0, new HPDrainEvent(2));
                skill.Data.SkillStates.Set(new HealState());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 3;
                ((AreaAction)skill.HitboxAction).Speed = 8;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Nasty_Plot";
                FiniteGatherEmitter emitter = new FiniteGatherEmitter(new AnimData("Absorb", 3));
                emitter.ParticlesPerBurst = 1;
                emitter.Bursts = 1;
                emitter.TravelTime = 30;
                emitter.UseDest = true;
                skill.HitboxAction.TileEmitter = emitter;
            }
            else if (ii == 139)
            {
                skill.Name = new LocalText("Poison Gas");
                skill.Desc = new LocalText("A cloud of poison gas is sprayed in the face of opposing Pokémon, poisoning those hit.");
                skill.BaseCharges = 16;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 85;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("poison", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(33);//Gas
                ((ProjectileAction)skill.HitboxAction).Range = 3;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Smoke_Purple", 2));
                shotAnim.StartDistance = 8;
                shotAnim.Shots = 7;
                shotAnim.BurstTime = 6;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.ActionFX.Sound = "DUN_Night_Shade";
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Smoke_Purple", 3));
            }
            else if (ii == 140)
            {
                skill.Name = new LocalText("Barrage");
                skill.Desc = new LocalText("Round objects are hurled at the target to strike five times in a row.");
                skill.BaseCharges = 14;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(15));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 5;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Barrage", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Throw_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Sound = "DUN_Throw_Arc";
            }
            else if (ii == 141)
            {
                skill.Name = new LocalText("Leech Life");
                skill.Desc = new LocalText("The user drains the target's blood. The user's HP is restored by the damage taken by the target.");
                skill.BaseCharges = 20;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new HPDrainEvent(1));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(18);//Bite
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Fangs_Red", 3));
                ((AttackAction)skill.HitboxAction).LagBehindTime = 5;
                skill.HitboxAction.ActionFX.Sound = "DUN_Revenge";
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                FiniteGatherEmitter endAnim = new FiniteGatherEmitter(new AnimData("Absorb", 3));
                endAnim.ParticlesPerBurst = 1;
                endAnim.Bursts = 1;
                endAnim.TravelTime = 30;
                endAnim.UseDest = true;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Absorb";
            }
            else if (ii == 142)
            {
                skill.Name = new LocalText("Lovely Kiss");
                skill.Desc = new LocalText("With a scary face, the user tries to force a kiss on the target. If it succeeds, the target falls asleep.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("sleep", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 6;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Lovely_Kiss";
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Lovely_Kiss_Lips", 2));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 2;
                emitter.Bursts = 4;
                emitter.StartDistance = 4;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Lovely_Kiss_Hit", 6));
                skill.Data.HitFX.Delay = 10;
            }
            else if (ii == 143)
            {
                skill.Name = new LocalText("Sky Attack");
                skill.Desc = new LocalText("A two-turn attack. This may also make the target flinch.");
                skill.BaseCharges = 6;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                SelfAction altAction = new SelfAction();
                altAction.CharAnimData = new CharAnimFrameType(06);//Charge
                altAction.TargetAlignments |= Alignment.Self;
                altAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Sky_Attack_Charge", 1));
                altAction.ActionFX.Sound = "DUN_Luminous_Orb";
                skill.Data.BeforeTryActions.Add(0, new ChargeOrReleaseEvent("charging_sky_attack", altAction));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).AppearanceMod = DashAction.DashAppearance.Invisible;
                ((DashAction)skill.HitboxAction).Range = 6;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.Wide;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                ((DashAction)skill.HitboxAction).Anim = new AnimData("Sky_Attack", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Sky_Attack";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Slash_Blue_RSE", 3));
            }
            else if (ii == 144)
            {
                skill.Name = new LocalText("Transform");
                skill.Desc = new LocalText("The user transforms into a copy of the target right down to having the same move set.");
                skill.BaseCharges = 5;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new TransformEvent(false, "transformed", 5));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 2;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Throw_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Puff_Green", 3));
                skill.HitboxAction.ActionFX.Sound = "DUN_Substitute";
            }
            else if (ii == 145)
            {
                skill.Name = new LocalText("Bubble");
                skill.Desc = new LocalText("A spray of countless bubbles is jetted at the target. This may also lower the target's Movement Speed.");
                skill.BaseCharges = 18;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(30));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_speed", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 6;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Bubble_Pop_Blue", 3));
                emitter.BurstTime = 5;
                emitter.ParticlesPerBurst = 2;
                emitter.Bursts = 5;
                emitter.StartDistance = 4;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Bubble";
                skill.Data.HitFX.Sound = "DUN_Bubble_2";
            }
            else if (ii == 146)
            {
                skill.Name = new LocalText("Dizzy Punch");
                skill.Desc = new LocalText("The target is hit with rhythmically launched punches. This may also leave the target confused.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new FistState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("confuse", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(11);//Punch
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Dizzy_Punch";
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Print_Fist", 12));
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Dizzy_Punch_Hit", 2));
            }
            else if (ii == 147)
            {
                skill.Name = new LocalText("Spore");
                skill.Desc = new LocalText("The user scatters bursts of spores that induce sleep.");
                skill.BaseCharges = 8;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("sleep", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(19);//Shake
                ((AreaAction)skill.HitboxAction).Range = 3;
                ((AreaAction)skill.HitboxAction).Speed = 5;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 30;
                skill.HitboxAction.ActionFX.Sound = "DUN_Spore";
                CircleSquareSprinkleEmitter emitter = new CircleSquareSprinkleEmitter(new AnimData("Spore", 15));
                emitter.ParticlesPerTile = 3;
                emitter.StartHeight = 24;
                emitter.HeightSpeed = -15;
                emitter.SpeedDiff = 15;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Puff_Brown", 3));
                skill.Data.HitFX.Sound = "DUN_Sleep";
            }
            else if (ii == 148)
            {
                skill.Name = new LocalText("Flash");
                skill.Desc = new LocalText("The user flashes a bright light that cuts the target's accuracy. It also reveals part of the floor.");
                skill.BaseCharges = 12;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 90;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_accuracy", true, false, -1));
                skill.Data.AfterActions.Add(0, new MapOutRadiusEvent(20));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 5;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Sparkle_RSE", 3));
                emitter.ParticlesPerTile = 2;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("White", 1, -1, -1, 128);
                overlay.TotalTime = 60;
                overlay.Movement = new RogueElements.Loc(0, -1);
                overlay.Layer = DrawLayer.Bottom;
                skill.HitboxAction.ActionFX.Emitter = overlay;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Disable";
            }
            else if (ii == 149)
            {
                skill.Name = new LocalText("-Psywave");
                skill.Desc = new LocalText("The target is attacked with an odd psychic wave. The attack varies in intensity.");
                skill.BaseCharges = 16;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(-1, new PsywaveDamageEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Aurora_Beam_Custom", 3));
                shotAnim.StartDistance = 16;
                shotAnim.Shots = 12;
                shotAnim.BurstTime = 3;
                skill.HitboxAction.ActionFX.Sound = "DUN_Psybeam";
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 150)
            {
                skill.Name = new LocalText("Splash");
                skill.Desc = new LocalText("The user flops over the target to get behind it.");
                skill.BaseCharges = 25;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new HopEvent(2, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(43);//Hop
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Attack";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 151)
            {
                skill.Name = new LocalText("Acid Armor");
                skill.Desc = new LocalText("The user alters its cellular structure to liquefy itself, sharply raising its Defense stat.");
                skill.BaseCharges = 18;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, 2));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                ((SelfAction)skill.HitboxAction).LagBehindTime = 10;
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                SqueezedAreaEmitter emitter = new SqueezedAreaEmitter(new AnimData("Bubbles_Blue", 4));
                emitter.BurstTime = 3;
                emitter.Bursts = 4;
                emitter.ParticlesPerBurst = 1;
                emitter.Range = 12;
                emitter.StartHeight = -4;
                emitter.HeightSpeed = 24;
                emitter.SpeedDiff = 4;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Toxic";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 152)
            {
                skill.Name = new LocalText("-Crabhammer");
                skill.Desc = new LocalText("The target is hammered with a large pincer. Critical hits land more easily.");
                skill.BaseCharges = 16;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(08);//Strike
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 153)
            {
                skill.Name = new LocalText("Explosion");
                skill.Desc = new LocalText("The user attacks everything around it by causing a tremendous explosion. The user takes damage as well.");
                skill.BaseCharges = 5;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;

                BattleData newData = new BattleData();
                newData.Element = "normal";
                newData.Category = BattleData.SkillCategory.Physical;
                newData.HitRate = -1;
                newData.OnHits.Add(-1, new CutHPDamageEvent());
                newData.ElementEffects.Add(0, new NormalizeEvent());
                newData.OnHitTiles.Add(0, new RemoveItemEvent(true));
                newData.OnHitTiles.Add(0, new RemoveTrapEvent());
                newData.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState))));
                SingleEmitter cuttingEmitter2 = new SingleEmitter(new AnimData("Grass_Clear", 2));
                newData.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter2, new FlagType(typeof(FoliageTerrainState))));
                skill.Data.BeforeHits.Add(-5, new AlignmentDifferentEvent(Alignment.Self, newData));

                skill.Data.OnHits.Add(-1, new CutHPDamageEvent());
                skill.Data.OnHitTiles.Add(0, new RemoveItemEvent(true));
                skill.Data.OnHitTiles.Add(0, new RemoveTrapEvent());
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState))));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.Explosion.Range = 2;
                skill.Explosion.HitTiles = true;
                skill.Explosion.Speed = 10;
                skill.Explosion.ExplodeFX.Sound = "DUN_Self-Destruct";
                skill.Explosion.ExplodeFX.ScreenMovement = new ScreenMover(0, 8, 30);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Explosion", 3));
                emitter.ParticlesPerTile = 0.5;
                skill.Explosion.Emitter = emitter;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(39);//Swell
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Explosion";
            }
            else if (ii == 154)
            {
                skill.Name = new LocalText("Fury Swipes");
                skill.Desc = new LocalText("The target is raked with sharp claws or scythes quickly five times in a row.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 70;
                skill.Data.SkillStates.Set(new BasePowerState(15));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", terrainEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 5;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(14);//MultiScratch
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Fury_Swipes";
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Fury_Swipes", 3));
            }
            else if (ii == 155)
            {
                skill.Name = new LocalText("Bonemerang");
                skill.Desc = new LocalText("The user throws the bone it holds. The bone loops to hit the target twice, coming and going.");
                skill.BaseCharges = 12;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 20;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = false;
                ((ProjectileAction)skill.HitboxAction).Boomerang = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Bonemarang", 2);
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Throw_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Sound = "DUN_Throw_Arc";
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Bonemarang_Hit", 3));
                skill.Data.HitFX.Sound = "DUN_Punch";
            }
            else if (ii == 156)
            {
                skill.Name = new LocalText("Rest");
                skill.Desc = new LocalText("The user goes to sleep for several turns. This fully restores the user's HP and heals any status conditions.");
                skill.BaseCharges = 10;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new RestEvent("sleep"));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Radar_Orb";
            }
            else if (ii == 157)
            {
                skill.Name = new LocalText("Rock Slide");
                skill.Desc = new LocalText("Large boulders are hurled at the opposing Pokémon to inflict damage. This may also make the target flinch.");
                skill.BaseCharges = 16;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Puff_Brown", 3));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Transform", terrainEmitter, new FlagType(typeof(WaterTerrainState)), new FlagType(typeof(LavaTerrainState)), new FlagType(typeof(AbyssTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((OffsetAction)skill.HitboxAction).HitArea = OffsetAction.OffsetArea.Sides;
                ((OffsetAction)skill.HitboxAction).Range = 2;
                ((OffsetAction)skill.HitboxAction).Speed = 10;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                ((OffsetAction)skill.HitboxAction).LagBehindTime = 35;
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Rock_Slide_Back", 1), new AnimData("Rock_Slide_Front", 1));
                emitter.HeightBack = 112;
                emitter.HeightFront = 112;
                skill.HitboxAction.TileEmitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Rock_Slide";
            }
            else if (ii == 158)
            {
                skill.Name = new LocalText("Hyper Fang");
                skill.Desc = new LocalText("The user bites hard on the target with its sharp front fangs. This may also make the target flinch.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(18);//Bite
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Fangs_Red", 3));
                ((AttackAction)skill.HitboxAction).LagBehindTime = 10;
                skill.HitboxAction.ActionFX.Sound = "DUN_Revenge";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Mega_Background_Yellow", 3));
                endAnim.Layer = DrawLayer.Back;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 159)
            {
                skill.Name = new LocalText("Sharpen");
                skill.Desc = new LocalText("The user lowers its polygon count to make itself more jagged, sharply raising the Attack stat.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 2));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Sharpen", 2));
                endAnim.LocHeight = 20;
                skill.Data.HitFX.Delay = 40;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Sharpen";
            }
            else if (ii == 160)
            {
                skill.Name = new LocalText("Conversion");
                skill.Desc = new LocalText("The user gives itself the Conversion status, which changes its type to match its moves.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("conversion", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Conversion";
                FiniteReleaseEmitter emitter = new FiniteReleaseEmitter(new AnimData("Puff_Green", 3), new AnimData("Puff_Yellow", 3), new AnimData("Puff_Blue", 3), new AnimData("Puff_Red", 3));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 1;
                emitter.Bursts = 5;
                emitter.Speed = 48;
                emitter.StartDistance = 4;
                skill.Data.HitFX.Emitter = emitter;
            }
            else if (ii == 161)
            {
                skill.Name = new LocalText("Tri Attack");
                skill.Desc = new LocalText("The user strikes with a simultaneous three-beam attack. May also burn, freeze, or paralyze the target.");
                skill.BaseCharges = 9;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(75));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                AdditionalEvent additionalEffect = new AdditionalEvent();
                additionalEffect.BaseEvents.Add(new StatusBattleEvent("burn", true, true));
                additionalEffect.BaseEvents.Add(new StatusBattleEvent("freeze", true, true));
                additionalEffect.BaseEvents.Add(new StatusBattleEvent("paralyze", true, true));
                skill.Data.OnHits.Add(0, additionalEffect);
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).Speed = 16;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Tri_Attack", 2);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Blowback_Orb";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Tri_Attack_Hit", 3));
                skill.Data.HitFX.Sound = "DUN_Tri_Attack";
            }
            else if (ii == 162)
            {
                skill.Name = new LocalText("Super Fang");
                skill.Desc = new LocalText("The user chomps hard on the target with its sharp front fangs. This cuts the target's HP in half.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(-1, new CutHPDamageEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(18);//Bite
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Fangs_Yellow", 3));
                ((AttackAction)skill.HitboxAction).LagBehindTime = 10;
                skill.HitboxAction.ActionFX.Sound = "DUN_Revenge";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Mega_Background_Red", 3));
                endAnim.Layer = DrawLayer.Back;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 163)
            {
                skill.Name = new LocalText("Slash");
                skill.Desc = new LocalText("The target is attacked with a slash of claws or blades. Critical hits land more easily.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(13);//Slice
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.ActionFX.Sound = "DUN_Slash";
                SingleEmitter single = new SingleEmitter(new AnimData("Slash_Ranger", 3));
                single.Offset = 16;
                skill.HitboxAction.ActionFX.Emitter = single;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 164)
            {
                skill.Name = new LocalText("Substitute");
                skill.Desc = new LocalText("The user turns the target into a decoy, making it a target for other Pokémon.");
                skill.BaseCharges = 14;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                //skill.SkillEffect.OnActions.Add(-1, new HPActionCheckEffect(4));
                //skill.SkillEffect.HitEffects.Add(0, new MaxHPDamageEffect(4));
                skill.Data.OnHits.Add(0, new StatusBattleEvent("decoy", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(42);//Rotate
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Throw_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Puff_Green", 3));
                skill.Data.HitFX.Sound = "DUN_Trace";
            }
            else if (ii == 165)
            {
                skill.Name = new LocalText("=Struggle");
                skill.Desc = new LocalText("An attack that is used in desperation only if the user has no PP. This also damages the user a little.");
                skill.BaseCharges = 1;
                skill.Data.Element = "none";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new ChipDamageEvent(6, new StringKey(), true, true));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Overheat";
            }
            else if (ii == 166)
            {
                skill.Name = new LocalText("=Sketch");
                skill.Desc = new LocalText("It enables the user to permanently learn the target's moves. Once used, Sketch disappears.");
                skill.BaseCharges = 1;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new SketchBattleEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Sticky_Trap";
            }
            else if (ii == 167)
            {
                skill.Name = new LocalText("-Triple Kick");
                skill.Desc = new LocalText("A consecutive three-kick attack that becomes more powerful with each successive hit.");
                skill.BaseCharges = 12;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 50;
                skill.Data.SkillStates.Set(new BasePowerState(20));
                skill.Data.BeforeHits.Add(0, new RepeatStrikeEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 3;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimProcess(CharAnimProcess.ProcessType.Spin);//Spin
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Around;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Print_Foot", 12));
                skill.Data.HitFX.Sound = "DUN_Double_Kick";
            }
            else if (ii == 168)
            {
                skill.Name = new LocalText("Thief");
                skill.Desc = new LocalText("The user attacks and steals the target's held item simultaneously.");
                skill.BaseCharges = 20;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new StealItemEvent(true, false, "seed_decoy", new HashSet<FlagType>(), new StringKey("MSG_STEAL_ITEM"), true, false)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 08;//Strike
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.ActionFX.Sound = "DUN_Drill Peck";
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Shadow_Force_Hit", 2));
            }
            else if (ii == 169)
            {
                skill.Name = new LocalText("-Spider Web");
                skill.Desc = new LocalText("The user ensnares the target with thin, gooey silk, immobilizing it.");
                skill.BaseCharges = 14;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("immobilized", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                ((AreaAction)skill.HitboxAction).Range = 3;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TileEmitter = new SingleEmitter(new AnimData("String_Shot_Web", 3));
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 170)
            {
                skill.Name = new LocalText("=Mind Reader");
                skill.Desc = new LocalText("The user senses the target's movements with its mind to ensure its next attack does not miss the target.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("sure_shot", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Mind_Reader";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Mean_Look_RSE", 3));
            }
            else if (ii == 171)
            {
                skill.Name = new LocalText("Nightmare");
                skill.Desc = new LocalText("Sleeping targets see a nightmare that inflicts some damage every turn.");
                skill.BaseCharges = 15;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("nightmare", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 5;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 30;
                CircleSquareSprinkleEmitter emitter = new CircleSquareSprinkleEmitter(new AnimData("Curse", 5));
                emitter.ParticlesPerTile = 1;
                emitter.StartHeight = 0;
                emitter.HeightSpeed = 18;
                emitter.SpeedDiff = 15;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Nightmare";
            }
            else if (ii == 172)
            {
                skill.Name = new LocalText("Flame Wheel");
                skill.Desc = new LocalText("The user cloaks itself in fire and charges at the target. This may also leave the target with a burn.");
                skill.BaseCharges = 16;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("burn", true, true)));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                ((DashAction)skill.HitboxAction).Anim = new AnimData("Flame_Wheel", 2);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Drill Peck";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Flamethrower", 3));
                endAnim.LocHeight = 14;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 173)
            {
                skill.Name = new LocalText("-Snore");
                skill.Desc = new LocalText("An attack that can be used only if the user is asleep. The harsh noise may also make the target flinch.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.SkillStates.Set(new AdditionalEffectState(70));
                skill.Data.BeforeActions.Add(-1, new AddContextStateEvent(new SleepAttack()));
                skill.Data.OnActions.Add(-1, new StatusNeededEvent("sleep", new StringKey("MSG_NOT_ASLEEP")));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(30);//Sound
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Snore";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Thorn_Gold", 3));
            }
            else if (ii == 174)
            {
                skill.Name = new LocalText("Curse");
                skill.Desc = new LocalText("A move that works differently for the Ghost type than for all other types.");
                skill.BaseCharges = 15;
                skill.Data.Element = "none";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, 1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, -1));

                ThrowAction altAction = new ThrowAction();
                altAction.CharAnimData = new CharAnimFrameType(06);//Charge
                altAction.Coverage = ThrowAction.ArcCoverage.WideAngle;
                altAction.Range = 3;
                altAction.TargetAlignments = Alignment.Foe;
                SingleEmitter introEmitter = new SingleEmitter(new AnimData("Curse_Nail", 5));
                introEmitter.LocHeight = 4;
                BattleFX altPreFX = new BattleFX();
                altPreFX.Emitter = introEmitter;
                altPreFX.Sound = "DUN_Wing_Attack";
                altAction.PreActions.Add(altPreFX);
                altAction.ActionFX.Sound = "DUN_Curse";
                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = Alignment.Foe;

                BattleData newData = new BattleData();
                newData.Element = "ghost";
                newData.Category = BattleData.SkillCategory.Status;
                newData.HitRate = -1;
                newData.OnHits.Add(0, new ChipDamageEvent(5));
                newData.OnHits.Add(0, new StatusHPBattleEvent("cursed", true, true, false, 5));
                FiniteSprinkleEmitter emitter = new FiniteSprinkleEmitter(new AnimData("Curse", 5));
                emitter.HeightSpeed = 36;
                emitter.TotalParticles = 1;
                emitter.StartHeight = 16;
                newData.HitFX.Emitter = emitter;
                newData.HitFX.Sound = "DUN_Curse_2";

                skill.Data.OnActions.Add(-3, new ElementDifferentUseEvent("ghost", altAction, altExplosion, newData));

                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(42);//Rotate
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                preFX.Sound = "DUN_Move_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
            }
            else if (ii == 175)
            {
                skill.Name = new LocalText("-Flail");
                skill.Desc = new LocalText("The user flails about aimlessly to attack. The less HP the user has, the greater the move's power.");
                skill.BaseCharges = 14;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState());
                skill.Data.OnActions.Add(0, new HPBasePowerEvent(100, true, false));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimProcess(CharAnimProcess.ProcessType.Spin);//Spin
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Around;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Spin_Trap";
            }
            else if (ii == 176)
            {
                skill.Name = new LocalText("Conversion 2");
                skill.Desc = new LocalText("The user gives itself the Conversion 2 status, which changes its type to resist the opponent's moves.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("conversion_2", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Conversion2";
                FiniteReleaseEmitter emitter = new FiniteReleaseEmitter(new AnimData("Puff_Green", 3), new AnimData("Puff_Yellow", 3), new AnimData("Puff_Blue", 3), new AnimData("Puff_Red", 3));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 1;
                emitter.Bursts = 5;
                emitter.Speed = 48;
                emitter.StartDistance = 4;
                skill.Data.HitFX.Emitter = emitter;
            }
            else if (ii == 177)
            {
                skill.Name = new LocalText("-Aeroblast");
                skill.Desc = new LocalText("A vortex of air is shot at the target to inflict damage. Critical hits land more easily.");
                skill.BaseCharges = 6;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", terrainEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Dragon_Rush";
            }
            else if (ii == 178)
            {
                skill.Name = new LocalText("Cotton Spore");
                skill.Desc = new LocalText("The user releases cotton-like spores that cling to the opposing Pokémon, which lowers their Movement Speeds.");
                skill.BaseCharges = 14;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 85;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, -1));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(19);//Shake
                ((AreaAction)skill.HitboxAction).Range = 3;
                ((AreaAction)skill.HitboxAction).Speed = 5;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 20;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Spore", 3, 0, 0), new AnimData("Spore", 3, 1, 1));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 6;
                emitter.StartDistance = 12;
                emitter.Bursts = 8;
                emitter.StartDistance = 4;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Wrap";
            }
            else if (ii == 179)
            {
                skill.Name = new LocalText("=Reversal");
                skill.Desc = new LocalText("An all-out attack that becomes more powerful the less HP the user has.");
                skill.BaseCharges = 12;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState());
                skill.Data.OnActions.Add(0, new HPBasePowerEvent(150, true, false));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Move_Start";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 180)
            {
                skill.Name = new LocalText("Spite");
                skill.Desc = new LocalText("The user unleashes its grudge on the move last used by the target, reducing its PP.");
                skill.BaseCharges = 18;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new SpiteEvent("last_used_move_slot", 5));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Night_Shade_2";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Spite", 3));
                endAnim.Layer = DrawLayer.Back;
                endAnim.LocHeight = 24;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Spite";
            }
            else if (ii == 181)
            {
                skill.Name = new LocalText("Powder Snow");
                skill.Desc = new LocalText("The user attacks with a chilling gust of powdery snow. This may also freeze the opposing Pokémon.");
                skill.BaseCharges = 16;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(35));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("freeze", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("Snow", 3);
                overlay.TotalTime = 60;
                overlay.Layer = DrawLayer.Top;
                skill.HitboxAction.ActionFX.Emitter = overlay;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Snowy_Weather";
            }
            else if (ii == 182)
            {
                skill.Name = new LocalText("Protect");
                skill.Desc = new LocalText("It enables the user to evade all attacks.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("protect", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Protect", 2), 4);
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Protect";
            }
            else if (ii == 183)
            {
                skill.Name = new LocalText("=Mach Punch");
                skill.Desc = new LocalText("The user throws a punch at blinding speed.");
                skill.BaseCharges = 18;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new FistState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 11;//Punch
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                AfterImageEmitter emitter = new AfterImageEmitter();
                emitter.Alpha = 128;
                emitter.AnimTime = 8;
                emitter.BurstTime = 1;
                ((DashAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Rage";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Print_Fist", 12));
            }
            else if (ii == 184)
            {
                skill.Name = new LocalText("Scary Face");
                skill.Desc = new LocalText("The user frightens the target with a scary face to harshly lower its Movement Speed.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 70;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, -2));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                preFX.Sound = "DUN_Move_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Scary_Face", 3));
                skill.Data.HitFX.Sound = "DUN_Night_Shade";
                skill.Data.HitFX.Delay = 20;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 185)
            {
                skill.Name = new LocalText("Feint Attack");
                skill.Desc = new LocalText("The user approaches the target disarmingly, then throws a sucker punch. This attack never misses.");
                skill.BaseCharges = 20;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                SingleEmitter intro = new SingleEmitter(new AnimData("Feint_Attack", 1), 20);
                intro.Layer = DrawLayer.Bottom;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = intro;
                preFX.Sound = "DUN_Feint_Attack";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 186)
            {
                skill.Name = new LocalText("Sweet Kiss");
                skill.Desc = new LocalText("The user kisses the target with a sweet, angelic cuteness that causes confusion.");
                skill.BaseCharges = 18;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("confuse", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Sweet_Kiss";
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Sweet_Kiss_Angel", 4), 7);
                ((AttackAction)skill.HitboxAction).LagBehindTime = 20;
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Charm", 2));
                endAnim.LocHeight = 12;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 187)
            {
                skill.Name = new LocalText("Belly Drum");
                skill.Desc = new LocalText("The user maximizes its Attack stat in exchange for half its max HP.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnActions.Add(-1, new HPActionCheckEvent(2));
                skill.Data.OnHits.Add(0, new MaxHPDamageEvent(2));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 12));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                SingleEmitter emitter = new SingleEmitter(new AnimData("Belly_Drum", 2));
                emitter.LocHeight = 12;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Belly_Drum";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
            }
            else if (ii == 188)
            {
                skill.Name = new LocalText("Sludge Bomb");
                skill.Desc = new LocalText("Unsanitary sludge is hurled at the target. This may also poison the target.");
                skill.BaseCharges = 8;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("poison", true, true)));
                skill.Explosion.Range = 1;
                skill.Explosion.HitTiles = true;
                skill.Explosion.Speed = 10;
                skill.Explosion.ExplodeFX.Sound = "DUN_Whirlpool";
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                CircleSquareSprinkleEmitter emitter = new CircleSquareSprinkleEmitter(new AnimData("Bubbles_Purple", 3));
                emitter.ParticlesPerTile = 3;
                emitter.HeightSpeed = 20;
                emitter.SpeedDiff = 20;
                skill.Explosion.Emitter = emitter;
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Poison_Ball_Ranger", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Throw_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Sound = "DUN_Throw_Arc";
            }
            else if (ii == 189)
            {
                skill.Name = new LocalText("Mud-Slap");
                skill.Desc = new LocalText("The user hurls mud in the target's face to inflict damage and lower its Accuracy.");
                skill.BaseCharges = 16;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(25));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_accuracy", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 2;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.ActionFX.Sound = "DUN_Mud_Sport";
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Puff_Brown", 3));
                shotAnim.StartDistance = 8;
                shotAnim.Shots = 7;
                shotAnim.BurstTime = 6;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 190)
            {
                skill.Name = new LocalText("Octazooka");
                skill.Desc = new LocalText("The user attacks by spraying ink at the target's face or eyes. This may also lower the target's Accuracy.");
                skill.BaseCharges = 11;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_accuracy", true, true, -1)));
                skill.Strikes = 1;
                skill.Explosion.Range = 1;
                skill.Explosion.HitTiles = true;
                skill.Explosion.Speed = 10;
                skill.Explosion.ExplodeFX.Sound = "DUN_Smokescreen";
                skill.Explosion.ExplodeFX.Delay = 10;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Puff_Black", 3));
                emitter.ParticlesPerTile = 1;
                skill.Explosion.Emitter = emitter;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Octazooka", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Octazooka";
            }
            else if (ii == 191)
            {
                skill.Name = new LocalText("-Spikes");
                skill.Desc = new LocalText("The user lays a down trap of spikes in front of itself. The trap hurts Pokémon that step on it.");
                skill.BaseCharges = 20;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHitTiles.Add(0, new SetTrapEvent("trap_spikes"));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 3;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 192)
            {
                skill.Name = new LocalText("Zap Cannon");
                skill.Desc = new LocalText("The user fires an electric blast like a cannon to inflict damage and cause paralysis.");
                skill.BaseCharges = 10;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 50;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("paralyze", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 16;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Shock_Wave", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Flash_Cannon";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Spark", 3));
                skill.Data.HitFX.Sound = "DUN_Charge_Beam_2";
            }
            else if (ii == 193)
            {
                skill.Name = new LocalText("Foresight");
                skill.Desc = new LocalText("The user identifies the target, removing its resistances and immunities. This also enables an evasive target to be hit.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("exposed", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).Speed = 40;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                preFX.Sound = "DUN_Move_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Foresight_Glass", 5), 5);
                skill.Data.HitFX.Sound = "DUN_Foresight";
                skill.Data.HitFX.Delay = 30;
            }
            else if (ii == 194)
            {
                skill.Name = new LocalText("Destiny Bond");
                skill.Desc = new LocalText("When the user takes damage, the target takes the same amount of damage.");
                skill.BaseCharges = 12;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("destiny_bond", false, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("White", 3);
                overlay.TotalTime = 80;
                overlay.Color = Microsoft.Xna.Framework.Color.Black;
                overlay.Layer = DrawLayer.Bottom;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = overlay;
                preFX.Sound = "DUN_Wing_Attack";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Sound = "DUN_Destiny_Bond";
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Destiny_Bond", 1));
                shotAnim.StartDistance = 0;
                shotAnim.Layer = DrawLayer.Bottom;
                shotAnim.LocHeight = -8;
                shotAnim.Shots = 1;
                shotAnim.BurstTime = 0;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 195)
            {
                skill.Name = new LocalText("Perish Song");
                skill.Desc = new LocalText("Any Pokémon that hears this song will faint when its countdown reaches 0.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("perish_song", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(29);//Sing
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 6;
                ((AreaAction)skill.HitboxAction).ActionFX.Delay = 30;
                skill.HitboxAction.ActionFX.Emitter = new BetweenEmitter(new AnimData("Perish_Song_Back", 1), new AnimData("Perish_Song_Front", 1));
                CircleSquareAreaEmitter areaEmitter = new CircleSquareAreaEmitter(new AnimData("Dark_Pulse_Ranger", 3));
                areaEmitter.ParticlesPerTile = 0.8f;
                areaEmitter.RangeDiff = -12;
                ((AreaAction)skill.HitboxAction).Emitter = areaEmitter;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Perish_Song";
            }
            else if (ii == 196)
            {
                skill.Name = new LocalText("Icy Wind");
                skill.Desc = new LocalText("The user attacks with a gust of chilled air. This also lowers the opposing Pokémon's Movement Speed.");
                skill.BaseCharges = 14;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_speed", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Icy_Wind_Particle", 1));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 5;
                emitter.Bursts = 5;
                emitter.StartDistance = 4;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Icy_Wind";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Icy_Wind", 3));
            }
            else if (ii == 197)
            {
                skill.Name = new LocalText("Detect");
                skill.Desc = new LocalText("It enables the user to evade all attacks.");
                skill.BaseCharges = 16;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("detect", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Mind_Reader";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Leer", 2));
            }
            else if (ii == 198)
            {
                skill.Name = new LocalText("Bone Rush");
                skill.Desc = new LocalText("The user strikes the target with a hard bone five times in a row.");
                skill.BaseCharges = 18;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 50;
                skill.Data.SkillStates.Set(new BasePowerState(20));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 5;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 08;//Strike
                ((DashAction)skill.HitboxAction).Range = 1;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Cut";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Bonemarang_Hit", 3));
                skill.Data.HitFX.Sound = "DUN_Punch";
            }
            else if (ii == 199)
            {
                skill.Name = new LocalText("Lock-On");
                skill.Desc = new LocalText("The user takes aim, and ensures the next attack does not miss the target.");
                skill.BaseCharges = 12;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("sure_shot", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Lock_On", 1));
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Move_Start";
                ((SelfAction)skill.HitboxAction).LagBehindTime = 26;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Leer", 3));
                skill.Data.HitFX.Sound = "DUN_Mind_Reader";
            }
            else if (ii == 200)
            {
                skill.Name = new LocalText("Outrage");
                skill.Desc = new LocalText("The user rampages and attacks for three turns. It then becomes confused, however.");
                skill.BaseCharges = 14;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new StatusBattleEvent("outrage", false, true));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Wide;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                SingleEmitter introAnim = new SingleEmitter(new AnimData("Rage", 2));
                introAnim.Layer = DrawLayer.Back;
                introAnim.LocHeight = 44;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = introAnim;
                preFX.Sound = "DUN_Rage";
                skill.HitboxAction.PreActions.Add(preFX);
                StaticAreaEmitter emitter = new StaticAreaEmitter(new AnimData("Blast_Burn", 3));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 1;
                emitter.Bursts = 3;
                emitter.Range = 12;
                emitter.LocHeight = 8;
                skill.HitboxAction.TileEmitter = emitter;
            }
            else if (ii == 201)
            {
                skill.Name = new LocalText("Sandstorm");
                skill.Desc = new LocalText("A sandstorm is summoned to hurt all combatants except the Rock, Ground, and Steel types.");
                skill.BaseCharges = 10;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new GiveMapStatusEvent("sandstorm", 0, new StringKey(), typeof(ExtendWeatherState)));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Move_Start";
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 202)
            {
                skill.Name = new LocalText("Giga Drain");
                skill.Desc = new LocalText("A nutrient-draining attack. The user's HP is restored by half the damage taken by the target.");
                skill.BaseCharges = 10;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new HPDrainEvent(2));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                ((ProjectileAction)skill.HitboxAction).Range = 2;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Rest";
                FiniteGatherEmitter emitter = new FiniteGatherEmitter(new AnimData("Absorb", 3));
                emitter.ParticlesPerBurst = 1;
                emitter.Bursts = 1;
                emitter.TravelTime = 30;
                emitter.UseDest = true;
                skill.HitboxAction.TileEmitter = emitter;
            }
            else if (ii == 203)
            {
                skill.Name = new LocalText("Endure");
                skill.Desc = new LocalText("The user endures any attack with at least 1 HP.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("endure", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Focus_Energy";
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                preFX.Sound = "DUN_Move_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                SqueezedAreaEmitter emitter = new SqueezedAreaEmitter(new AnimData("Focus_Energy", 4));
                emitter.Bursts = 4;
                emitter.ParticlesPerBurst = 2;
                emitter.BurstTime = 6;
                emitter.Range = 16;
                emitter.StartHeight = 0;
                emitter.HeightSpeed = 160;
                skill.Data.HitFX.Emitter = emitter;
                skill.Data.HitFX.Delay = 20;
            }
            else if (ii == 204)
            {
                skill.Name = new LocalText("Charm");
                skill.Desc = new LocalText("The user gazes at the target rather charmingly, making it less wary. This harshly lowers its Attack stat.");
                skill.BaseCharges = 18;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 80;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, -2));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Charm", 1));
                endAnim.LocHeight = 16;
                skill.HitboxAction.ActionFX.Emitter = endAnim;
                skill.HitboxAction.ActionFX.Sound = "DUN_Charm";
                ((AttackAction)skill.HitboxAction).LagBehindTime = 20;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 205)
            {
                skill.Name = new LocalText("-Rollout");
                skill.Desc = new LocalText("The user rolls into the target from a distance. It becomes more powerful the farther the target is.");
                skill.BaseCharges = 14;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(15));
                skill.Data.BeforeHits.Add(0, new TipPowerEvent());
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 4;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "_UNK_DUN_Heave_Up";
                skill.Data.HitFX.Sound = "DUN_Rollout";
            }
            else if (ii == 206)
            {
                skill.Name = new LocalText("False Swipe");
                skill.Desc = new LocalText("A restrained attack that prevents the target from fainting. The target is left with at least 1 HP.");
                skill.BaseCharges = 24;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.BeforeHits.Add(0, new AddContextStateEvent(new AttackEndure()));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", terrainEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(13);//Slice
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Wrap";
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("False_Swipe", 2));
                skill.Data.HitFX.Delay = 10;
            }
            else if (ii == 207)
            {
                skill.Name = new LocalText("Swagger");
                skill.Desc = new LocalText("The user enrages and confuses the target. However, this also sharply raises the target's Attack stat.");
                skill.BaseCharges = 12;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 90;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 2));
                skill.Data.OnHits.Add(0, new StatusBattleEvent("confuse", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Rage", 2));
                endAnim.Layer = DrawLayer.Back;
                endAnim.LocHeight = 44;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = endAnim;
                preFX.Sound = "DUN_Rage";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Foe | Alignment.Friend;
            }
            else if (ii == 208)
            {
                skill.Name = new LocalText("Milk Drink");
                skill.Desc = new LocalText("The user restores the party's HP by up to a half of its max HP");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new RestoreHPEvent(1, 2, true));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Friend;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Milk_Drink", 3));
                emitter.LocHeight = 24;
                skill.Data.HitFX.Emitter = emitter;
                skill.Data.HitFX.Sound = "DUN_Milk_Drink";
                skill.Data.HitFX.Delay = 20;
            }
            else if (ii == 209)
            {
                skill.Name = new LocalText("Spark");
                skill.Desc = new LocalText("The user throws an electrically charged tackle at the target. This may also leave the target with paralysis.");
                skill.BaseCharges = 16;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("paralyze", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Spark", 3));
                preFX.Sound = "DUN_Spark";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Spark", 3));
                skill.Data.HitFX.Sound = "DUN_Spark";
            }
            else if (ii == 210)
            {
                skill.Name = new LocalText("Fury Cutter");
                skill.Desc = new LocalText("The target is slashed with scythes or claws. This attack becomes more powerful if it hits in succession.");
                skill.BaseCharges = 22;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(20));
                skill.Data.BeforeHits.Add(0, new RepeatHitEvent("last_used_move", "times_move_used", 10, 1, false));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", terrainEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(13);//Slice
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                SingleEmitter emitter = new SingleEmitter();
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Fury_Cutter", 2));
                skill.HitboxAction.ActionFX.Sound = "DUN_Fury_Cutter";
                skill.Data.HitFX.Delay = 10;
            }
            else if (ii == 211)
            {
                skill.Name = new LocalText("Steel Wing");
                skill.Desc = new LocalText("The target is hit with wings of steel. This may also raise the user's Defense stat.");
                skill.BaseCharges = 15;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new AdditionalEndEvent(new StatusStackBattleEvent("mod_defense", false, true, 1)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.Wide;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                ((DashAction)skill.HitboxAction).LagBehindTime = 10;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Harden_RSE", 3));
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Wing_Attack_2";
                skill.HitboxAction.TileEmitter = new SingleEmitter(new AnimData("Air_Slash_Slash", 2));
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Steel_Wing", 2));
                endAnim.LocHeight = 24;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 212)
            {
                skill.Name = new LocalText("Mean Look");
                skill.Desc = new LocalText("The user pins the target with a dark, arresting look. The target becomes immobilized.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("immobilized", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).Speed = 40;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                preFX.Sound = "DUN_Move_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Mean_Look", 3));
                skill.Data.HitFX.Sound = "DUN_Mean_Look";
                skill.Data.HitFX.Delay = 30;
            }
            else if (ii == 213)
            {
                skill.Name = new LocalText("Attract");
                skill.Desc = new LocalText("The user infatuates the target, making it unable to hurt the user.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("in_love", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Charm_Heart", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Charm";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Charm", 1));
                endAnim.LocHeight = 8;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Charm";
            }
            else if (ii == 214)
            {
                skill.Name = new LocalText("Sleep Talk");
                skill.Desc = new LocalText("While it is asleep, the user performs the strongest move it knows.");
                skill.BaseCharges = 25;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.BeforeActions.Add(-1, new AddContextStateEvent(new SleepAttack()));
                skill.Data.OnActions.Add(-1, new StatusNeededEvent("sleep", new StringKey("MSG_NOT_ASLEEP")));
                skill.Data.OnHits.Add(0, new StrongestMoveEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(30);//Sound
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Move_Start";
            }
            else if (ii == 215)
            {
                skill.Name = new LocalText("Heal Bell");
                skill.Desc = new LocalText("The user makes a soothing bell chime to heal the status conditions of all Pokémon.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = -1;
                skill.Data.BeforeActions.Add(-1, new AddContextStateEvent(new CureAttack()));
                skill.Data.OnHits.Add(0, new RemoveStateStatusBattleEvent(typeof(BadStatusState), true, new StringKey("MSG_CURE_ALL")));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(29);//Sing
                ((AreaAction)skill.HitboxAction).Range = 5;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Heal_Bell", 4));
                emitter.LocHeight = 24;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Heal_Bell";
                preFX.Delay = 30;
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2));
                skill.Data.HitFX.Sound = "DUN_Healing_Wish_2";
            }
            else if (ii == 216)
            {
                skill.Name = new LocalText("-Return");
                skill.Desc = new LocalText("A full-power attack that grows more powerful the more allies there are nearby.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(25));
                skill.Data.OnActions.Add(0, new AllyBasePowerEvent(false));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 217)
            {
                skill.Name = new LocalText("Present");
                skill.Desc = new LocalText("The user attacks by giving the target a gift with a hidden trap. It restores HP if used on an ally.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());

                BattleData newData = new BattleData();
                newData.Element = "normal";
                newData.Category = BattleData.SkillCategory.Status;
                newData.HitRate = -1;
                newData.OnHits.Add(0, new RestoreHPEvent(1, 3, true));
                skill.Data.BeforeHits.Add(-5, new AlignmentDifferentEvent(Alignment.Friend | Alignment.Self, newData));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Present", 3, 0, 0);
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Pound";
            }
            else if (ii == 218)
            {
                skill.Name = new LocalText("-Frustration");
                skill.Desc = new LocalText("A full-power attack that grows more powerful if the user has no allies near it.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnActions.Add(0, new AllyBasePowerEvent(true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 219)
            {
                skill.Name = new LocalText("Safeguard");
                skill.Desc = new LocalText("The user creates a protective field that prevents status conditions for ten turns.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("safeguard", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Safeguard_Back", 3), new AnimData("Safeguard_Front", 3));
                emitter.HeightBack = 32;
                emitter.HeightFront = 32;
                skill.HitboxAction.ActionFX.Emitter = emitter;
                skill.HitboxAction.ActionFX.Sound = "DUN_Safeguard";
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
            }
            else if (ii == 220)
            {
                skill.Name = new LocalText("-Pain Split");
                skill.Desc = new LocalText("The user adds its HP to the target's HP, then equally shares the combined HP with the target.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new PainSplitEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 4;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Mega_Drain_3";
            }
            else if (ii == 221)
            {
                skill.Name = new LocalText("-Sacred Fire");
                skill.Desc = new LocalText("The target is razed with a mystical fire of great intensity. This may also leave the target with a burn.");
                skill.BaseCharges = 8;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("burn", true, true)));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 5;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.Wide;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                AttachAreaEmitter emitter = new AttachAreaEmitter(new AnimData("Fire_Blast", 3));
                emitter.BurstTime = 1;
                emitter.ParticlesPerBurst = 8;
                emitter.Range = 44;
                emitter.AddHeight = 12;
                ((DashAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Fire_Column_Ranger", 3));
                endAnim.LocHeight = 32;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 222)
            {
                skill.Name = new LocalText("=Magnitude");
                skill.Desc = new LocalText("The user attacks everything around it with a ground-shaking quake. It does less damage to targets far away.");
                skill.BaseCharges = 12;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.BeforeHits.Add(0, new DistanceDropEvent());
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(31);//Rumble
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.ActionFX.ScreenMovement = new ScreenMover(0, 8, 30);
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Flame_Wheel";
            }
            else if (ii == 223)
            {
                skill.Name = new LocalText("Dynamic Punch");
                skill.Desc = new LocalText("The user punches the target with full, concentrated power. This confuses the target if it hits.");
                skill.BaseCharges = 18;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new FistState());
                skill.Data.HitRate = 50;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("confuse", true, true)));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState))));
                skill.Strikes = 1;
                skill.Explosion.Range = 1;
                skill.Explosion.HitTiles = true;
                skill.Explosion.Speed = 10;
                skill.Explosion.ExplodeFX.Sound = "DUN_Self-Destruct";
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                CircleSquareAreaEmitter explosionEmitter = new CircleSquareAreaEmitter(new AnimData("Blast_Seed", 3));
                explosionEmitter.ParticlesPerTile = 0.8;
                skill.Explosion.Emitter = explosionEmitter;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(11);//Punch
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.FrontAndCorners;
                ((AttackAction)skill.HitboxAction).BurstTiles = TileAlignment.Wall;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Print_Fist", 12));
                skill.HitboxAction.ActionFX.Sound = "DUN_Punch";
            }
            else if (ii == 224)
            {
                skill.Name = new LocalText("-Megahorn");
                skill.Desc = new LocalText("Using its tough and impressive horn, the user rams into the target with no letup.");
                skill.BaseCharges = 12;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 75;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 20;//Jab
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Megahorn_Back", 3), new AnimData("Megahorn_Front", 3));
                skill.Data.HitFX.Emitter = emitter;
            }
            else if (ii == 225)
            {
                skill.Name = new LocalText("Dragon Breath");
                skill.Desc = new LocalText("The user exhales a mighty gust that inflicts damage. This may also leave the target with paralysis.");
                skill.BaseCharges = 10;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("paralyze", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 5;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Flamethrower_2", 2));
                shotAnim.StartDistance = 8;
                shotAnim.Shots = 8;
                shotAnim.BurstTime = 5;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Flamethrower";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Flamethrower", 3));
                endAnim.LocHeight = 14;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 226)
            {
                skill.Name = new LocalText("-Baton Pass");
                skill.Desc = new LocalText("The user passes along its stat changes and minor status conditions to the target.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new TransferStatusEvent(false, false, true, true, true));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(42);//Rotate
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
            }
            else if (ii == 227)
            {
                skill.Name = new LocalText("Encore");
                skill.Desc = new LocalText("The user compels the target to keep using only the move it last used for five turns.");
                skill.BaseCharges = 12;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("encore", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 2;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Encore_Clap", 3));
                emitter.LocHeight = 24;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Encore";
                skill.HitboxAction.PreActions.Add(preFX);
                ((ThrowAction)skill.HitboxAction).LagBehindTime = 30;
                BetweenEmitter endAnim = new BetweenEmitter(new AnimData("Encore_Spotlight_Base", 2), new AnimData("Encore_Spotlight_Front", 2));
                endAnim.HeightBack = 56;
                endAnim.HeightFront = 56;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Encore_2";
                skill.Data.HitFX.Delay = 20;
            }
            else if (ii == 228)
            {
                skill.Name = new LocalText("-Pursuit");
                skill.Desc = new LocalText("The user chases down the target, preventing it from getting away.");
                skill.BaseCharges = 15;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("chasing", false, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 4;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 229)
            {
                skill.Name = new LocalText("Rapid Spin");
                skill.Desc = new LocalText("A spin attack that can eliminate moves such as Wrap or Leech Seed. It also destroys traps.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.BeforeActions.Add(-1, new AddContextStateEvent(new BoundAttack()));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHitTiles.Add(0, new RemoveTrapEvent());
                skill.Data.AfterActions.Add(0, new RemoveStatusBattleEvent("wrap", false));
                skill.Data.AfterActions.Add(0, new RemoveStatusBattleEvent("bind", false));
                skill.Data.AfterActions.Add(0, new RemoveStatusBattleEvent("fire_spin", false));
                skill.Data.AfterActions.Add(0, new RemoveStatusBattleEvent("whirlpool", false));
                skill.Data.AfterActions.Add(0, new RemoveStatusBattleEvent("sand_tomb", false));
                skill.Data.AfterActions.Add(0, new RemoveStatusBattleEvent("immobilized", false));
                skill.Data.AfterActions.Add(0, new RemoveStatusBattleEvent("leech_seed", false));
                skill.Data.AfterActions.Add(0, new RemoveStatusBattleEvent("clamp", false));
                skill.Data.AfterActions.Add(0, new RemoveStatusBattleEvent("infestation", false));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimProcess(CharAnimProcess.ProcessType.Spin);//Spin
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Around;
                SingleEmitter shootEmitter = new SingleEmitter(new AnimData("Gust", 1));
                shootEmitter.LocHeight = 24;
                skill.HitboxAction.ActionFX.Emitter = shootEmitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Gust_Trap";
            }
            else if (ii == 230)
            {
                skill.Name = new LocalText("Sweet Scent");
                skill.Desc = new LocalText("A sweet scent that lowers the evasiveness of opposing Pokémon.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                //skill.SkillEffect.HitEffects.Add(0, new WarpHereEffect());
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_evasion", true, false, -1));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(19);//Shake
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 8;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Puff_Pink", 3));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 4;
                emitter.Bursts = 5;
                emitter.StartDistance = 4;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Sweet_Scent";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Puff_Pink", 3));
            }
            else if (ii == 231)
            {
                skill.Name = new LocalText("=Iron Tail");
                skill.Desc = new LocalText("The target is slammed with a steel-hard tail. This may also lower the target's Defense stat. It can break walls.");
                skill.BaseCharges = 16;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 75;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_defense", true, true, -1)));
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Wall_Break", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Rollout", terrainEmitter, new FlagType(typeof(WallTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Wide;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Move_Start";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 232)
            {
                skill.Name = new LocalText("=Metal Claw");
                skill.Desc = new LocalText("The target is raked with steel claws. This may also raise the user's Attack stat. It can break walls.");
                skill.BaseCharges = 20;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(55));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Wall_Break", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Rollout", terrainEmitter, new FlagType(typeof(WallTerrainState))));
                skill.Data.AfterActions.Add(0, new AdditionalEndEvent(new StatusStackBattleEvent("mod_attack", false, true, 1)));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(10);//Scratch
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.FrontAndCorners;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Metal_Claw", 2));
                skill.HitboxAction.ActionFX.Sound = "DUN_Fury_Cutter";
            }
            else if (ii == 233)
            {
                skill.Name = new LocalText("-Vital Throw");
                skill.Desc = new LocalText("The user attacks with a throw move that never misses.");
                skill.BaseCharges = 15;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(0, new ThrowBackEvent(2, new DamageFormulaEvent()));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 234)
            {
                skill.Name = new LocalText("-Morning Sun");
                skill.Desc = new LocalText("The user restores the party's HP. The amount of HP regained varies with the weather.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = -1;
                Dictionary<string, bool> weather = new Dictionary<string, bool>();
                weather.Add("sunny", true);
                weather.Add("rain", false);
                weather.Add("sandstorm", false);
                weather.Add("hail", false);
                skill.Data.OnHits.Add(0, new WeatherHPEvent(4, weather));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.ActionFX.Sound = "DUN_Sunny_Day";
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Data.HitFX.Emitter = new SingleEmitter(new BeamAnimData("Column_Yellow", 3, -1, -1, 192));
            }
            else if (ii == 235)
            {
                skill.Name = new LocalText("Synthesis");
                skill.Desc = new LocalText("The user restores its own HP. The amount of HP regained varies with the weather.");
                skill.BaseCharges = 10;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = -1;
                Dictionary<string, bool> weather = new Dictionary<string, bool>();
                weather.Add("sunny", true);
                weather.Add("rain", false);
                weather.Add("sandstorm", false);
                weather.Add("hail", false);
                skill.Data.OnHits.Add(0, new WeatherHPEvent(6, weather));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                SqueezedAreaEmitter emitter = new SqueezedAreaEmitter(new ParticleAnim(new AnimData("Synthesis_Sparkle", 3), 2));
                emitter.HeightSpeed = -12;
                emitter.Range = 16;
                emitter.StartHeight = 16;
                emitter.HeightDiff = 16;
                emitter.Bursts = 5;
                emitter.BurstTime = 3;
                emitter.ParticlesPerBurst = 2;
                skill.Data.HitFX.Emitter = emitter;
                skill.Data.HitFX.Sound = "DUN_Synthesis";
                skill.Data.HitFX.Delay = 20;
            }
            else if (ii == 236)
            {
                skill.Name = new LocalText("Moonlight");
                skill.Desc = new LocalText("The user restores the party's HP. The amount of HP regained varies with the weather.");
                skill.BaseCharges = 10;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = -1;
                Dictionary<string, bool> weather = new Dictionary<string, bool>();
                weather.Add("sunny", true);
                weather.Add("rain", false);
                weather.Add("sandstorm", false);
                weather.Add("hail", false);
                skill.Data.OnHits.Add(0, new WeatherHPEvent(3, weather));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 30;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("Moonlight", 3);
                overlay.TotalTime = 60;
                overlay.Layer = DrawLayer.Bottom;
                skill.HitboxAction.ActionFX.Emitter = overlay;
                skill.HitboxAction.ActionFX.Sound = "DUN_Moonlight";
            }
            else if (ii == 237)
            {
                skill.Name = new LocalText("=Hidden Power");
                skill.Desc = new LocalText("A unique attack that varies in type each time it is used.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnActions.Add(-1, new HiddenPowerEvent());
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ProjectileAction)skill.HitboxAction).Range = 2;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).Rays = ProjectileAction.RayCount.Eight;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Hidden_Power_Ball_RSE", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Petrify_Orb";
            }
            else if (ii == 238)
            {
                skill.Name = new LocalText("Cross Chop");
                skill.Desc = new LocalText("The user delivers a double chop with its forearms crossed. Critical hits land more easily.");
                skill.BaseCharges = 15;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(09);//Chop
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Cross_Chop", 3));
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Cross_Chop";
                skill.Data.HitFX.Delay = 10;
            }
            else if (ii == 239)
            {
                skill.Name = new LocalText("Twister");
                skill.Desc = new LocalText("The user whips up a vicious tornado to tear at the opposing Pokémon. This may also make them flinch.");
                skill.BaseCharges = 15;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((OffsetAction)skill.HitboxAction).HitArea = OffsetAction.OffsetArea.Area;
                ((OffsetAction)skill.HitboxAction).Range = 2;
                ((OffsetAction)skill.HitboxAction).Speed = 10;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Gust", 1));
                emitter.ParticlesPerTile = 0.5;
                emitter.LocHeight = 24;
                ((OffsetAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Twister";
            }
            else if (ii == 240)
            {
                skill.Name = new LocalText("Rain Dance");
                skill.Desc = new LocalText("The user summons a heavy rain on the floor, powering up Water-type moves.");
                skill.BaseCharges = 10;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new GiveMapStatusEvent("rain", 0, new StringKey(), typeof(ExtendWeatherState)));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Move_Start";
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 241)
            {
                skill.Name = new LocalText("Sunny Day");
                skill.Desc = new LocalText("The user intensifies the sunlight for the entire floor, powering up Fire-type moves.");
                skill.BaseCharges = 10;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new GiveMapStatusEvent("sunny", 0, new StringKey(), typeof(ExtendWeatherState)));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Move_Start";
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 242)
            {
                skill.Name = new LocalText("Crunch");
                skill.Desc = new LocalText("The user crunches up the target with sharp fangs. This may also lower the target's Defense stat.");
                skill.BaseCharges = 16;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new JawState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_defense", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(18);//Bite
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).LagBehindTime = 20;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Crunch", 3));
                emitter.Offset = -8;
                ((AttackAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.ActionFX.Sound = "DUN_Bite";
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Sound = "DUN_Bite";
            }
            else if (ii == 243)
            {
                skill.Name = new LocalText("Mirror Coat");
                skill.Desc = new LocalText("A retaliation move that counters any special attack, inflicting double the damage taken.");
                skill.BaseCharges = 15;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("mirror_coat", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Light_Screen";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Screen_RSE_Pink", 3, -1, -1, 192));
            }
            else if (ii == 244)
            {
                skill.Name = new LocalText("Psych Up");
                skill.Desc = new LocalText("The user hypnotizes itself into copying any stat change made by the target.");
                skill.BaseCharges = 15;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                HashSet<string> statChanges = new HashSet<string>();
                statChanges.Add("mod_speed");
                statChanges.Add("mod_attack");
                statChanges.Add("mod_defense");
                statChanges.Add("mod_special_attack");
                statChanges.Add("mod_special_defense");
                statChanges.Add("mod_accuracy");
                statChanges.Add("mod_evasion");
                skill.Data.OnHits.Add(0, new ReflectStatsEvent(statChanges));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 4;
                skill.HitboxAction.ActionFX.Sound = "DUN_Psych_Up";
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Psych_Up", 3));
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
            }
            else if (ii == 245)
            {
                skill.Name = new LocalText("-Extreme Speed");
                skill.Desc = new LocalText("The user charges the target at blinding speed.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 6;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                AfterImageEmitter emitter = new AfterImageEmitter();
                emitter.Alpha = 128;
                emitter.AnimTime = 8;
                emitter.BurstTime = 1;
                ((DashAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Fury_Attack";
            }
            else if (ii == 246)
            {
                skill.Name = new LocalText("Ancient Power");
                skill.Desc = new LocalText("The user attacks with a prehistoric power. It may also raise all the user's stats at once.");
                skill.BaseCharges = 11;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(55));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                GroupEvent groupEvent = new GroupEvent();
                groupEvent.BaseEvents.Add(new StatusStackBattleEvent("mod_speed", false, true, 1));
                groupEvent.BaseEvents.Add(new StatusStackBattleEvent("mod_attack", false, true, 1));
                groupEvent.BaseEvents.Add(new StatusStackBattleEvent("mod_defense", false, true, 1));
                groupEvent.BaseEvents.Add(new StatusStackBattleEvent("mod_special_attack", false, true, 1));
                groupEvent.BaseEvents.Add(new StatusStackBattleEvent("mod_special_defense", false, true, 1));
                skill.Data.AfterActions.Add(0, new AdditionalEndEvent(groupEvent));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(31);//Rumble
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Ancient_Power_Back", 2), new AnimData("Ancient_Power_Front", 2));
                emitter.HeightBack = 8;
                emitter.HeightFront = 8;
                skill.HitboxAction.TileEmitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Superpower";
            }
            else if (ii == 247)
            {
                skill.Name = new LocalText("Shadow Ball");
                skill.Desc = new LocalText("The user hurls a shadowy blob at the target. This may also lower the target's Sp. Def stat.");
                skill.BaseCharges = 9;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_special_defense", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Shadow_Ball", 2);
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Shadow_Ball";
            }
            else if (ii == 248)
            {
                skill.Name = new LocalText("Future Sight");
                skill.Desc = new LocalText("Two turns after this move is used, a hunk of psychic energy attacks the target's area.");
                skill.BaseCharges = 13;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new FutureAttackEvent("future_sight", true, false, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).LagBehindTime = 20;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Copycat_2";
                skill.HitboxAction.PreActions.Add(preFX);
                RepeatEmitter endAnim = new RepeatEmitter(new AnimData("Circle_Thick_Red_In", 2));
                endAnim.Bursts = 3;
                endAnim.BurstTime = 12;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Confusion";
            }
            else if (ii == 249)
            {
                skill.Name = new LocalText("Rock Smash");
                skill.Desc = new LocalText("The user attacks with a punch. This may also lower the target's Defense stat. This move will shatter walls.");
                skill.BaseCharges = 25;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(20));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_defense", true, true, -1)));
                skill.Data.OnHitTiles.Add(0, new ShatterTerrainEvent("wall"));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(11);//Punch
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.FrontAndCorners;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Move_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Rock_Smash", 2));
                skill.Data.HitFX.Sound = "DUN_Rollout";
            }
            else if (ii == 250)
            {
                skill.Name = new LocalText("Whirlpool");
                skill.Desc = new LocalText("The user traps the target in a violent swirling whirlpool for three turns.");
                skill.BaseCharges = 15;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new StatusBattleEvent("whirlpool", true, true)));
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Puff_Brown", 3));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Transform", terrainEmitter, new FlagType(typeof(LavaTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((OffsetAction)skill.HitboxAction).HitArea = OffsetAction.OffsetArea.Tile;
                ((OffsetAction)skill.HitboxAction).Range = 2;
                ((OffsetAction)skill.HitboxAction).Speed = 10;
                ((OffsetAction)skill.HitboxAction).LagBehindTime = 30;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Gust_Blue", 1));
                emitter.LocHeight = 24;
                skill.HitboxAction.TileEmitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Whirlpool";
            }
            else if (ii == 251)
            {
                skill.Name = new LocalText("=Beat Up");
                skill.Desc = new LocalText("The user calls over ally Pokémon to it.");
                skill.BaseCharges = 12;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new WarpAlliesInEvent(80, 3, false, new StringKey("MSG_SUMMON_ALLIES"), false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                SingleEmitter emitter = new SingleEmitter(new AnimData("Beat_Up", 3));
                emitter.LocHeight = 24;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Rollcall_Orb";
                preFX.Delay = 20;
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
            }
            else if (ii == 252)
            {
                skill.Name = new LocalText("-Fake Out");
                skill.Desc = new LocalText("An attack that hits from a distance and makes the target flinch. It fails on targets directly in front.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.BeforeHits.Add(0, new DistanceOnlyEvent());
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 4;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Feint_2";
            }
            else if (ii == 253)
            {
                skill.Name = new LocalText("Uproar");
                skill.Desc = new LocalText("The user attacks in an uproar, preventing everyone from falling asleep.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new StatusBattleEvent("sleepless", true, false));

                BattleData newData = new BattleData();
                newData.Element = "normal";
                newData.Category = BattleData.SkillCategory.Status;
                newData.HitRate = -1;
                newData.OnHits.Add(-1, new StatusBattleEvent("sleepless", true, true));
                skill.Data.BeforeHits.Add(-5, new AlignmentDifferentEvent(Alignment.Friend | Alignment.Self, newData));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                ((AreaAction)skill.HitboxAction).Range = 3;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Uproar", 2));
                emitter.ParticlesPerTile = 0.25;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Uproar";
            }
            else if (ii == 254)
            {
                skill.Name = new LocalText("-Stockpile");
                skill.Desc = new LocalText("The user charges up power and raises both its Defense and Sp. Def. stats. The move can be used three times.");
                skill.BaseCharges = 24;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("stockpile", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(39);//Swell
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
            }
            else if (ii == 255)
            {
                skill.Name = new LocalText("Spit Up");
                skill.Desc = new LocalText("The power stored using the move Stockpile is released at once in an attack. The more power is stored, the more powerful the attack.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;

                Dictionary<int, Tuple<CombatAction, ExplosionData, BattleData>> tiers = new Dictionary<int, Tuple<CombatAction, ExplosionData, BattleData>>();
                {
                    AreaAction altAction = new AreaAction();
                    altAction.PreActions.Add(new BattleFX(new EmptyFiniteEmitter(), "DUN_Wing_Attack", 0));
                    altAction.CharAnimData = new CharAnimFrameType(07);//Shoot
                    altAction.ActionFX.Sound = "DUN_Spit_Up";
                    altAction.HitArea = Hitbox.AreaLimit.Cone;
                    altAction.Range = 2;
                    altAction.Speed = 10;
                    CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Swallow_Balls", 1, 0, 0));
                    emitter.BurstTime = 6;
                    emitter.ParticlesPerBurst = 3;
                    emitter.Bursts = 6;
                    emitter.StartDistance = 4;
                    altAction.Emitter = emitter;
                    altAction.LagBehindTime = 16;
                    altAction.TargetAlignments = Alignment.Foe;

                    ExplosionData altExplosion = new ExplosionData();
                    altExplosion.TargetAlignments = Alignment.Foe;

                    BattleData newData = new BattleData();
                    newData.Element = "normal";
                    newData.Category = BattleData.SkillCategory.Magical;
                    newData.HitRate = 100;
                    newData.SkillStates.Set(new BasePowerState(80));
                    newData.OnHits.Add(-1, new DamageFormulaEvent());
                    newData.AfterActions.Add(0, new RemoveStatusBattleEvent("stockpile", false));
                    //newData.HitFX.Emitter = new SingleEmitter(new AnimData("Hit_Neutral", 3));
                    tiers.Add(1, new Tuple<CombatAction, ExplosionData, BattleData>(altAction, altExplosion, newData));
                }
                {
                    AreaAction altAction = new AreaAction();
                    altAction.PreActions.Add(new BattleFX(new EmptyFiniteEmitter(), "DUN_Wing_Attack", 0));
                    altAction.CharAnimData = new CharAnimFrameType(07);//Shoot
                    altAction.ActionFX.Sound = "DUN_Spit_Up";
                    altAction.HitArea = Hitbox.AreaLimit.Cone;
                    altAction.Range = 3;
                    altAction.Speed = 14;
                    CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Swallow_Balls", 1, 5, 5));
                    emitter.BurstTime = 6;
                    emitter.ParticlesPerBurst = 4;
                    emitter.Bursts = 6;
                    emitter.StartDistance = 4;
                    altAction.Emitter = emitter;
                    altAction.LagBehindTime = 16;
                    altAction.TargetAlignments = Alignment.Foe;

                    ExplosionData altExplosion = new ExplosionData();
                    altExplosion.TargetAlignments = Alignment.Foe;

                    BattleData newData = new BattleData();
                    newData.Element = "normal";
                    newData.Category = BattleData.SkillCategory.Magical;
                    newData.HitRate = 100;
                    newData.SkillStates.Set(new BasePowerState(80));
                    newData.OnHits.Add(-1, new DamageFormulaEvent());
                    newData.AfterActions.Add(0, new RemoveStatusBattleEvent("stockpile", false));
                    //newData.HitFX.Emitter = new SingleEmitter(new AnimData("Hit_Neutral", 3));
                    tiers.Add(2, new Tuple<CombatAction, ExplosionData, BattleData>(altAction, altExplosion, newData));
                }
                {
                    AreaAction altAction = new AreaAction();
                    altAction.PreActions.Add(new BattleFX(new EmptyFiniteEmitter(), "DUN_Wing_Attack", 0));
                    altAction.CharAnimData = new CharAnimFrameType(07);//Shoot
                    altAction.ActionFX.Sound = "DUN_Spit_Up";
                    altAction.HitArea = Hitbox.AreaLimit.Cone;
                    altAction.Range = 4;
                    altAction.Speed = 18;
                    CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Swallow_Balls", 1, 3, 3));
                    emitter.BurstTime = 6;
                    emitter.ParticlesPerBurst = 5;
                    emitter.Bursts = 6;
                    emitter.StartDistance = 4;
                    altAction.Emitter = emitter;
                    altAction.LagBehindTime = 16;
                    altAction.TargetAlignments = Alignment.Foe;

                    ExplosionData altExplosion = new ExplosionData();
                    altExplosion.TargetAlignments = Alignment.Foe;

                    BattleData newData = new BattleData();
                    newData.Element = "normal";
                    newData.Category = BattleData.SkillCategory.Magical;
                    newData.HitRate = 100;
                    newData.SkillStates.Set(new BasePowerState(80));
                    newData.OnHits.Add(-1, new DamageFormulaEvent());
                    newData.AfterActions.Add(0, new RemoveStatusBattleEvent("stockpile", false));
                    //newData.HitFX.Emitter = new SingleEmitter(new AnimData("Hit_Neutral", 3));
                    tiers.Add(3, new Tuple<CombatAction, ExplosionData, BattleData>(altAction, altExplosion, newData));
                }
                skill.Data.OnActions.Add(-3, new StatusStackDifferentEvent("stockpile", new StringKey("MSG_STOCKPILE_NONE"), tiers));

                skill.Data.AfterActions.Add(0, new RemoveStatusBattleEvent("stockpile", false));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 256)
            {
                skill.Name = new LocalText("Swallow");
                skill.Desc = new LocalText("The power stored using the move Stockpile is absorbed by the user to heal itself. Storing more power increases its effectiveness.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = -1;

                Dictionary<int, Tuple<CombatAction, ExplosionData, BattleData>> tiers = new Dictionary<int, Tuple<CombatAction, ExplosionData, BattleData>>();
                {
                    SelfAction altAction = new SelfAction();
                    altAction.CharAnimData = new CharAnimFrameType(06);//Charge
                    altAction.TargetAlignments = Alignment.Self;

                    ExplosionData altExplosion = new ExplosionData();
                    altExplosion.TargetAlignments = Alignment.Self;

                    BattleData newData = new BattleData();
                    newData.Element = "normal";
                    newData.Category = BattleData.SkillCategory.Status;
                    newData.HitRate = -1;
                    newData.OnHits.Add(0, new RestoreHPEvent(1, 2, true));
                    newData.AfterActions.Add(0, new RemoveStatusBattleEvent("stockpile", false));
                    SingleEmitter emitter = new SingleEmitter(new AnimData("Swallow_Blue", 2));
                    emitter.LocHeight = 32;
                    newData.HitFX.Emitter = emitter;
                    newData.HitFX.Delay = 30;
                    newData.HitFX.Sound = "DUN_Swallow";
                    tiers.Add(1, new Tuple<CombatAction, ExplosionData, BattleData>(altAction, altExplosion, newData));
                }
                {
                    SelfAction altAction = new SelfAction();
                    altAction.CharAnimData = new CharAnimFrameType(06);//Charge
                    altAction.TargetAlignments = Alignment.Self;

                    ExplosionData altExplosion = new ExplosionData();
                    altExplosion.TargetAlignments = Alignment.Self;

                    BattleData newData = new BattleData();
                    newData.Element = "normal";
                    newData.Category = BattleData.SkillCategory.Status;
                    newData.HitRate = -1;
                    newData.OnHits.Add(0, new RestoreHPEvent(1, 1, true));
                    newData.AfterActions.Add(0, new RemoveStatusBattleEvent("stockpile", false));
                    SingleEmitter emitter = new SingleEmitter(new AnimData("Swallow_Red", 2));
                    emitter.LocHeight = 32;
                    newData.HitFX.Emitter = emitter;
                    newData.HitFX.Delay = 30;
                    newData.HitFX.Sound = "DUN_Swallow";
                    tiers.Add(2, new Tuple<CombatAction, ExplosionData, BattleData>(altAction, altExplosion, newData));
                }
                {
                    SelfAction altAction = new SelfAction();
                    altAction.CharAnimData = new CharAnimFrameType(06);//Charge
                    altAction.TargetAlignments = Alignment.Self;

                    ExplosionData altExplosion = new ExplosionData();
                    altExplosion.TargetAlignments = Alignment.Self;

                    BattleData newData = new BattleData();
                    newData.Element = "normal";
                    newData.Category = BattleData.SkillCategory.Status;
                    newData.HitRate = -1;
                    newData.OnHits.Add(-1, new AddContextStateEvent(new CureAttack()));
                    newData.OnHits.Add(0, new RestoreHPEvent(1, 1, true));
                    newData.OnHits.Add(0, new RemoveStateStatusBattleEvent(typeof(BadStatusState), true, new StringKey("MSG_CURE_ALL")));
                    newData.AfterActions.Add(0, new RemoveStatusBattleEvent("stockpile", false));
                    SingleEmitter emitter = new SingleEmitter(new AnimData("Swallow_Yellow", 2));
                    emitter.LocHeight = 32;
                    newData.HitFX.Emitter = emitter;
                    newData.HitFX.Delay = 30;
                    newData.HitFX.Sound = "DUN_Swallow";
                    tiers.Add(3, new Tuple<CombatAction, ExplosionData, BattleData>(altAction, altExplosion, newData));
                }
                skill.Data.OnActions.Add(-3, new StatusStackDifferentEvent("stockpile", new StringKey("MSG_STOCKPILE_NONE"), tiers));

                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
            }
            else if (ii == 257)
            {
                skill.Name = new LocalText("Heat Wave");
                skill.Desc = new LocalText("The user attacks by exhaling hot breath on the opposing Pokémon.");
                skill.BaseCharges = 8;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(65));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(32);//FlapAround
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Heat_Wave";
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("Heat_Wave", 3, 0, 0, 128);
                overlay.TotalTime = 60;
                overlay.Movement = new RogueElements.Loc(2, 0);
                overlay.Layer = DrawLayer.Top;
                skill.HitboxAction.ActionFX.Emitter = overlay;
                SingleEmitter endEmitter = new SingleEmitter(new AnimData("Ember", 3));
                endEmitter.LocHeight = 8;
                skill.Data.HitFX.Emitter = endEmitter;
                skill.Data.HitFX.Sound = "DUN_Ember_2";
            }
            else if (ii == 258)
            {
                skill.Name = new LocalText("Hail");
                skill.Desc = new LocalText("The user summons a hailstorm on the floor. It damages all Pokémon except the Ice type.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new GiveMapStatusEvent("hail", 0, new StringKey(), typeof(ExtendWeatherState)));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Move_Start";
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 259)
            {
                skill.Name = new LocalText("Torment");
                skill.Desc = new LocalText("The user torments and enrages the target, making it incapable of using the same move twice in a row.");
                skill.BaseCharges = 18;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("torment", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Torment", 3));
                emitter.LocHeight = 28;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Torment";
                skill.HitboxAction.PreActions.Add(preFX);
                ((ThrowAction)skill.HitboxAction).LagBehindTime = 30;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitCharAction = new CharAnimFrameType(04);//Hurt
            }
            else if (ii == 260)
            {
                skill.Name = new LocalText("=Flatter");
                skill.Desc = new LocalText("Flattery is used to confuse the target. However, this also raises the target's Sp. Atk stat.");
                skill.BaseCharges = 12;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 90;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, 2));
                skill.Data.OnHits.Add(0, new StatusBattleEvent("confuse", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 2;
                skill.HitboxAction.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Foe | Alignment.Friend;
                BetweenEmitter endAnim = new BetweenEmitter(new AnimData("Flatter_Back", 2), new AnimData("Flatter_Front", 2));
                endAnim.HeightBack = 56;
                endAnim.HeightFront = 56;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Encore_2";
                skill.Data.HitFX.Delay = 30;
            }
            else if (ii == 261)
            {
                skill.Name = new LocalText("Will-O-Wisp");
                skill.Desc = new LocalText("The user shoots a sinister, bluish-white flame at the target to inflict a burn.");
                skill.BaseCharges = 16;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 75;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("burn", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                AttachAreaEmitter emitter = new AttachAreaEmitter(new AnimData("Grudge_Lights", 3));
                emitter.BurstTime = 2;
                emitter.ParticlesPerBurst = 1;
                emitter.Range = 12;
                emitter.AddHeight = 12;
                ((ProjectileAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Fire_Spin";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Will_O_Wisp", 3));
                endAnim.LocHeight = 18;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 262)
            {
                skill.Name = new LocalText("-Memento");
                skill.Desc = new LocalText("This move harshly lowers the target's Attack and Sp. Atk stats. The user then drops to 1 HP, and warps away.");
                skill.BaseCharges = 12;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, -2));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, -2));
                skill.Data.AfterActions.Add(0, new HPTo1Event(false));
                skill.Data.AfterActions.Add(0, new RandomWarpEvent(50, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Spite", 3));
                emitter.Layer = DrawLayer.Back;
                emitter.LocHeight = 24;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Flare_Blitz_2";
            }
            else if (ii == 263)
            {
                skill.Name = new LocalText("=Façade");
                skill.Desc = new LocalText("An attack move that increases in power if the user is suffering a major status condition.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(65));
                skill.Data.OnActions.Add(0, new MajorStatusPowerEvent(false, 2, 1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Tackle";
            }
            else if (ii == 264)
            {
                skill.Name = new LocalText("-Focus Punch");
                skill.Desc = new LocalText("The user focuses its mind before launching a punch. This move fails if the user is hit before it is used.");
                skill.BaseCharges = 15;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new FistState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(130));
                SelfAction altAction = new SelfAction();
                altAction.CharAnimData = new CharAnimFrameType(06);//Charge
                altAction.TargetAlignments |= Alignment.Self;
                altAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                altAction.ActionFX.Sound = "DUN_Move_Start";
                skill.Data.BeforeTryActions.Add(0, new ChargeOrReleaseEvent("charging_focus_punch", altAction));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 11;//Punch
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Print_Fist", 12));
            }
            else if (ii == 265)
            {
                skill.Name = new LocalText("Smelling Salts");
                skill.Desc = new LocalText("This attack inflicts double damage on a target with paralysis, and cures its condition. Allies targeted by this move will not take damage.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));

                BattleData newData = new BattleData();
                newData.Element = "normal";
                newData.Category = BattleData.SkillCategory.Status;
                newData.HitRate = -1;
                newData.HitFX.Delay = 20;
                newData.OnHits.Add(0, new RemoveStatusBattleEvent("paralyze", true));
                skill.Data.BeforeHits.Add(-5, new AlignmentDifferentEvent(Alignment.Friend | Alignment.Self, newData));

                skill.Data.BeforeHits.Add(0, new StatusPowerEvent("paralyze", true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new RemoveStatusBattleEvent("paralyze", true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(12);//Slap
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Smelling_Salts", 2));
                emitter.LocHeight = 12;
                ((AttackAction)skill.HitboxAction).Emitter = emitter;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Attack";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Sound = "DUN_Smelling_Salt";
                skill.HitboxAction.TargetAlignments = Alignment.Friend | Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Friend | Alignment.Foe;
                skill.Data.HitFX.Delay = 20;
            }
            else if (ii == 266)
            {
                skill.Name = new LocalText("Follow Me");
                skill.Desc = new LocalText("The user draws attention to itself, redirecting all enemy attacks to the user.");
                skill.BaseCharges = 14;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("follow_me", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Nasty_Plot_Point", 3));
                emitter.LocHeight = 20;
                skill.HitboxAction.ActionFX.Emitter = emitter;
                skill.HitboxAction.ActionFX.Sound = "DUN_Tail_Whip";
            }
            else if (ii == 267)
            {
                skill.Name = new LocalText("-Nature Power");
                skill.Desc = new LocalText("An attack that makes use of nature's power. Its effects vary depending on the user's environment.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                Dictionary<string, string> terrain = new Dictionary<string, string>();
                terrain.Add("electric_terrain", "thunderbolt");
                terrain.Add("grassy_terrain", "energy_ball");
                terrain.Add("misty_terrain", "moonblast");

                Dictionary<string, string> types = new Dictionary<string, string>();
                types.Add(Text.Sanitize(((ElementInfo.Element)01).ToString()).ToLower(), "bug_buzz");
                types.Add(Text.Sanitize(((ElementInfo.Element)02).ToString()).ToLower(), "dark_pulse");
                types.Add(Text.Sanitize(((ElementInfo.Element)03).ToString()).ToLower(), "draco_meteor");
                types.Add(Text.Sanitize(((ElementInfo.Element)04).ToString()).ToLower(), "thunderbolt");
                types.Add(Text.Sanitize(((ElementInfo.Element)05).ToString()).ToLower(), "moonblast");
                types.Add(Text.Sanitize(((ElementInfo.Element)06).ToString()).ToLower(), "aura_sphere");
                types.Add(Text.Sanitize(((ElementInfo.Element)07).ToString()).ToLower(), "flamethrower");
                types.Add(Text.Sanitize(((ElementInfo.Element)08).ToString()).ToLower(), "air_slash");
                types.Add(Text.Sanitize(((ElementInfo.Element)09).ToString()).ToLower(), "shadow_ball");
                types.Add(Text.Sanitize(((ElementInfo.Element)10).ToString()).ToLower(), "energy_ball");
                types.Add(Text.Sanitize(((ElementInfo.Element)11).ToString()).ToLower(), "earth_power");
                types.Add(Text.Sanitize(((ElementInfo.Element)12).ToString()).ToLower(), "frost_breath");
                types.Add(Text.Sanitize(((ElementInfo.Element)13).ToString()).ToLower(), "tri_attack");
                types.Add(Text.Sanitize(((ElementInfo.Element)14).ToString()).ToLower(), "sludge_bomb");
                types.Add(Text.Sanitize(((ElementInfo.Element)15).ToString()).ToLower(), "extrasensory");
                types.Add(Text.Sanitize(((ElementInfo.Element)16).ToString()).ToLower(), "power_gem");
                types.Add(Text.Sanitize(((ElementInfo.Element)17).ToString()).ToLower(), "flash_cannon");
                types.Add(Text.Sanitize(((ElementInfo.Element)18).ToString()).ToLower(), "hydro_pump");
                skill.Data.OnHits.Add(0, new NatureMoveEvent(terrain, types));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
            }
            else if (ii == 268)
            {
                skill.Name = new LocalText("-Charge");
                skill.Desc = new LocalText("The user boosts the power of the Electric move it uses on the next turn. The user takes less damage from special attacks while charged.");
                skill.BaseCharges = 20;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("charge", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Charge";
            }
            else if (ii == 269)
            {
                skill.Name = new LocalText("-Taunt");
                skill.Desc = new LocalText("The target is taunted into a rage that allows it to use only attack moves.");
                skill.BaseCharges = 20;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("taunted", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 4;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Taunt", 5));
                emitter.LocHeight = 20;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Taunt";
                skill.HitboxAction.PreActions.Add(preFX);
                ((ThrowAction)skill.HitboxAction).LagBehindTime = 20;
            }
            else if (ii == 270)
            {
                skill.Name = new LocalText("Helping Hand");
                skill.Desc = new LocalText("The user assists its allies, boosting their Attack and Sp. Atk.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(42);//Rotate
                ((AreaAction)skill.HitboxAction).Range = 3;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 30;
                skill.HitboxAction.TargetAlignments = Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Friend;
                skill.HitboxAction.ActionFX.Sound = "DUN_Helping_Hand";
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Helping_Hand", 3));
            }
            else if (ii == 271)
            {
                skill.Name = new LocalText("-Trick");
                skill.Desc = new LocalText("The user catches the target off guard and swaps places with it.");
                skill.BaseCharges = 18;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new SwitcherEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Sound = "DUN_Switcher";
            }
            else if (ii == 272)
            {
                skill.Name = new LocalText("-Role Play");
                skill.Desc = new LocalText("The user mimics the target completely, copying the target's natural Ability.");
                skill.BaseCharges = 16;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new ReflectAbilityEvent(false));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 4;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Evasion_Up_2";
                //TODO: animations that end with the user
                skill.Data.HitFX.Delay = 20;
            }
            else if (ii == 273)
            {
                skill.Name = new LocalText("Wish");
                skill.Desc = new LocalText("Three turns after this move is used, the target's HP is restored by half the user's max HP.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusHPBattleEvent("wish", true, false, false, 2));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 8;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Wish", 2));
                emitter.ParticlesPerTile = 0.5;
                emitter.LocHeight = 24;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("White", 3);
                overlay.TotalTime = 80;
                overlay.Color = Microsoft.Xna.Framework.Color.Black;
                overlay.Layer = DrawLayer.Bottom;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = overlay;
                preFX.Sound = "DUN_Wish";
                skill.HitboxAction.PreActions.Add(preFX);
                SqueezedAreaEmitter endAnim = new SqueezedAreaEmitter(new ParticleAnim(new AnimData("Synthesis_Sparkle", 3), 2));
                endAnim.HeightSpeed = -16;
                endAnim.Range = 16;
                endAnim.StartHeight = 24;
                endAnim.HeightDiff = 8;
                endAnim.Bursts = 5;
                endAnim.BurstTime = 3;
                endAnim.ParticlesPerBurst = 2;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Metal_Sound";
            }
            else if (ii == 274)
            {
                skill.Name = new LocalText("=Assist");
                skill.Desc = new LocalText("The user hurriedly uses the last move it saw an ally use.");
                skill.BaseCharges = 22;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new MirrorMoveEvent("last_ally_move"));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                StaticAreaEmitter emitter = new StaticAreaEmitter(new AnimData("Assist", 3));
                emitter.Range = 16;
                emitter.ParticlesPerBurst = 1;
                emitter.BurstTime = 6;
                emitter.Bursts = 5;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Ice_Ball";
                preFX.Delay = 40;
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 275)
            {
                skill.Name = new LocalText("Ingrain");
                skill.Desc = new LocalText("The user lays roots that restore its HP on every turn. Because it is rooted, it can't move around or be forced off its position.");
                skill.BaseCharges = 20;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("ingrain", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Ingrain", 3));
                skill.Data.HitFX.Sound = "DUN_Ingrain";
                skill.Data.HitFX.Delay = 20;
            }
            else if (ii == 276)
            {
                skill.Name = new LocalText("Superpower");
                skill.Desc = new LocalText("The user attacks the target with great power. However, this also lowers the user's Attack and Defense stats.");
                skill.BaseCharges = 12;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Wall_Break", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Rollout", terrainEmitter, new FlagType(typeof(WallTerrainState))));
                skill.Data.AfterActions.Add(0, new StatusStackBattleEvent("mod_attack", false, true, -1));
                skill.Data.AfterActions.Add(0, new StatusStackBattleEvent("mod_defense", false, true, -1));
                skill.Strikes = 1;
                skill.Explosion.ExplodeFX.ScreenMovement = new ScreenMover(0, 8, 30);
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 23;//Slam
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Superpower", 1));
                preFX.Sound = "DUN_Superpower";
                preFX.Delay = 20;
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Rock_Climb_2";
            }
            else if (ii == 277)
            {
                skill.Name = new LocalText("Magic Coat");
                skill.Desc = new LocalText("The user puts up a barrier that reflects status moves back to the user.");
                skill.BaseCharges = 16;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("magic_coat", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Sound = "DUN_Light_Screen";
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Screen_RSE_Green", 3, -1, -1, 192));
            }
            else if (ii == 278)
            {
                skill.Name = new LocalText("=Recycle");
                skill.Desc = new LocalText("The user recycles Plain Seeds and Grimy Food held by the team.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                List<string> foodTypes = new List<string>();
                foodTypes.Add("food_apple");
                foodTypes.Add("food_apple_big");
                skill.Data.OnHits.Add(0, new ItemRestoreEvent(true, "food_grimy", foodTypes, new StringKey("MSG_RECYCLE")));
                List<string> seedTypes = new List<string>();
                seedTypes.Add("seed_reviver");
                seedTypes.Add("seed_joy");
                seedTypes.Add("seed_doom");
                seedTypes.Add("seed_hunger");
                seedTypes.Add("seed_warp");
                seedTypes.Add("seed_sleep");
                seedTypes.Add("seed_blast");
                seedTypes.Add("seed_blinker");
                seedTypes.Add("seed_pure");
                seedTypes.Add("seed_ice");
                seedTypes.Add("seed_decoy");
                seedTypes.Add("seed_last_chance");
                seedTypes.Add("seed_ban");
                skill.Data.OnHits.Add(0, new ItemRestoreEvent(true, "seed_plain", seedTypes, new StringKey("MSG_RECYCLE")));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2));
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Sound = "DUN_Teeter_Dance";
            }
            else if (ii == 279)
            {
                skill.Name = new LocalText("=Revenge");
                skill.Desc = new LocalText("An attack move that inflicts double the damage if the user was last hit by the target.");
                skill.BaseCharges = 16;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.BeforeHits.Add(0, new RevengeEvent("last_targeted_by", 2, 1, false, true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Revenge", 3));
                endAnim.LocHeight = 4;
                ((AttackAction)skill.HitboxAction).Emitter = endAnim;
            }
            else if (ii == 280)
            {
                skill.Name = new LocalText("=Brick Break");
                skill.Desc = new LocalText("The user attacks with a swift chop. It can also break walls, as well as barriers, such as Light Screen and Reflect.");
                skill.BaseCharges = 18;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("reflect", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("light_screen", true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Wall_Break", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Rollout", terrainEmitter, new FlagType(typeof(WallTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(09);//Chop
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.FrontAndCorners;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "_UNK_DUN_Break";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Print_Hand", 12));
                skill.Data.HitFX.Sound = "DUN_Rollout";
            }
            else if (ii == 281)
            {
                skill.Name = new LocalText("-Yawn");
                skill.Desc = new LocalText("The user lets loose a huge yawn that lulls the target into falling asleep in a few turns.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("yawning", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(30);//Sound
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Confusion";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 282)
            {
                skill.Name = new LocalText("Knock Off");
                skill.Desc = new LocalText("The user slaps off the target's held item, making it fly off from the target.");
                skill.BaseCharges = 20;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new KnockItemEvent(false, true, "", new HashSet<FlagType>())));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(12);//Slap
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Knock_Off", 3));
                emitter.LocHeight = 20;
                ((AttackAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.ActionFX.Sound = "DUN_Arm_Thrust";
                skill.Data.HitFX.Delay = 10;
            }
            else if (ii == 283)
            {
                skill.Name = new LocalText("-Endeavor");
                skill.Desc = new LocalText("An attack move that cuts down the target's HP to equal the user's HP.");
                skill.BaseCharges = 8;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(-1, new EndeavorEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                SingleEmitter emitter = new SingleEmitter();
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 284)
            {
                skill.Name = new LocalText("Eruption");
                skill.Desc = new LocalText("The user attacks opposing Pokémon with explosive fury. The lower the user's HP, the lower the move's power.");
                skill.BaseCharges = 8;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState());
                skill.Data.OnActions.Add(0, new HPBasePowerEvent(100, false, false));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(31);//Rumble
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 5;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                CircleSquareSprinkleEmitter emitter = new CircleSquareSprinkleEmitter();
                for (int nn = 0; nn < DirExt.DIR8_COUNT; nn++)
                    emitter.Anims.Add(new ParticleAnim(new AnimData("Eruption_Rocks", 40, nn, nn)));
                emitter.ParticlesPerTile = 1.5;
                emitter.StartHeight = 0;
                emitter.HeightSpeed = 320;
                emitter.SpeedDiff = 2;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                SingleEmitter shootEmitter = new SingleEmitter(new AnimData("Eruption", 2));
                shootEmitter.LocHeight = 20;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = shootEmitter;
                preFX.Sound = "EVT_Tower_Quake";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.ScreenMovement = new ScreenMover(0, 8, 30);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 285)
            {
                skill.Name = new LocalText("-Skill Swap");
                skill.Desc = new LocalText("The user employs its psychic power to exchange Abilities with the target.");
                skill.BaseCharges = 24;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new SwapAbilityEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).Speed = 0;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                skill.HitboxAction.PreActions.Add(new BattleFX(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 2)), "DUN_Growth_2", 0));
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);

                SwingSwitchEmitter emitter = new SwingSwitchEmitter(new AnimData("Confuse_Ray", 3));
                emitter.AxisRatio = 0.5f;
                emitter.Amount = 1;
                emitter.RotationTime = 20;
                skill.Data.IntroFX.Add(new BattleFX(emitter, "", 20));
                SingleEmitter destAnim = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2));
                destAnim.UseDest = true;
                skill.Data.IntroFX.Add(new BattleFX(destAnim, "", 0));
                skill.Data.IntroFX.Add(new BattleFX(new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2)), "DUN_Growth", 0));
            }
            else if (ii == 286)
            {
                skill.Name = new LocalText("=Imprison");
                skill.Desc = new LocalText("This move prevents the target from using the move it last used.");
                skill.BaseCharges = 11;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new DisableBattleEvent("disable", "last_used_move_slot"));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 3;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Sealed";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("X_RSE", 30));
            }
            else if (ii == 287)
            {
                skill.Name = new LocalText("=Refresh");
                skill.Desc = new LocalText("The user cures the party of major status conditions.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.BeforeActions.Add(-1, new AddContextStateEvent(new CureAttack()));
                skill.Data.OnHits.Add(0, new RemoveStateStatusBattleEvent(typeof(MajorStatusState), true, new StringKey("MSG_CURE_MAJOR")));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 30;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("Refresh", 3, -1, -1, 128);
                overlay.TotalTime = 60;
                overlay.Movement = new RogueElements.Loc(0, -3);
                overlay.Layer = DrawLayer.Bottom;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = overlay;
                preFX.Sound = "DUN_Water_Sport";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2));
                skill.Data.HitFX.Sound = "DUN_Wonder_Tile";
            }
            else if (ii == 288)
            {
                skill.Name = new LocalText("Grudge");
                skill.Desc = new LocalText("If the user is damaged, the user's grudge depletes the PP of every move the opponent knows.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("grudge", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Grudge_Back", 3), new AnimData("Grudge_Front", 3));
                emitter.HeightBack = 12;
                emitter.HeightFront = 12;
                skill.Data.HitFX.Emitter = emitter;
                skill.Data.HitFX.Sound = "DUN_Grudge";
                skill.Data.HitFX.Delay = 20;
            }
            else if (ii == 289)
            {
                skill.Name = new LocalText("Snatch");
                skill.Desc = new LocalText("The user steals the effects of any attempts to use a healing or stat-changing move.");
                skill.BaseCharges = 14;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("snatch", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Move_Start";
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 290)
            {
                skill.Name = new LocalText("-Secret Power");
                skill.Desc = new LocalText("An attack whose additional effects depend on the condition of the floor.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());

                Dictionary<string, BattleEvent> terrain = new Dictionary<string, BattleEvent>();
                terrain.Add("electric_terrain", new StatusBattleEvent("paralyze", true, true));
                terrain.Add("grassy_terrain", new StatusBattleEvent("sleep", true, true));
                terrain.Add("misty_terrain", new StatusStackBattleEvent("mod_attack", true, true, -1));

                Dictionary<string, BattleEvent> types = new Dictionary<string, BattleEvent>();
                types.Add(Text.Sanitize(((ElementInfo.Element)01).ToString()).ToLower(), new StatusStackBattleEvent("mod_accuracy", false, true, 1));
                types.Add(Text.Sanitize(((ElementInfo.Element)02).ToString()).ToLower(), new StatusBattleEvent("flinch", true, true));
                types.Add(Text.Sanitize(((ElementInfo.Element)03).ToString()).ToLower(), new StatusStackBattleEvent("mod_special_defense", false, true, 1));
                types.Add(Text.Sanitize(((ElementInfo.Element)04).ToString()).ToLower(), new StatusBattleEvent("paralyze", true, true));
                types.Add(Text.Sanitize(((ElementInfo.Element)05).ToString()).ToLower(), new StatusStackBattleEvent("mod_attack", true, true, -1));
                types.Add(Text.Sanitize(((ElementInfo.Element)06).ToString()).ToLower(), new StatusStackBattleEvent("mod_attack", false, true, 1));
                types.Add(Text.Sanitize(((ElementInfo.Element)07).ToString()).ToLower(), new StatusBattleEvent("burn", true, true));
                types.Add(Text.Sanitize(((ElementInfo.Element)08).ToString()).ToLower(), new StatusStackBattleEvent("mod_speed", false, true, 1));
                types.Add(Text.Sanitize(((ElementInfo.Element)09).ToString()).ToLower(), new StatusBattleEvent("confuse", true, true));
                types.Add(Text.Sanitize(((ElementInfo.Element)10).ToString()).ToLower(), new StatusBattleEvent("sleep", true, true));
                types.Add(Text.Sanitize(((ElementInfo.Element)11).ToString()).ToLower(), new StatusBattleEvent("immobilized", true, false));
                types.Add(Text.Sanitize(((ElementInfo.Element)12).ToString()).ToLower(), new StatusBattleEvent("freeze", true, true));
                types.Add(Text.Sanitize(((ElementInfo.Element)13).ToString()).ToLower(), new StatusStackBattleEvent("mod_evasion", true, true, -1));
                types.Add(Text.Sanitize(((ElementInfo.Element)14).ToString()).ToLower(), new StatusBattleEvent("poison_toxic", true, true));
                types.Add(Text.Sanitize(((ElementInfo.Element)15).ToString()).ToLower(), new StatusStackBattleEvent("mod_special_defense", true, true, -1));
                types.Add(Text.Sanitize(((ElementInfo.Element)16).ToString()).ToLower(), new StatusStackBattleEvent("mod_defense", true, true, -1));
                types.Add(Text.Sanitize(((ElementInfo.Element)17).ToString()).ToLower(), new StatusStackBattleEvent("mod_defense", false, true, 1));
                types.Add(Text.Sanitize(((ElementInfo.Element)18).ToString()).ToLower(), new StatusStackBattleEvent("mod_speed", true, true, -1));
                skill.Data.OnHits.Add(0, new AdditionalEvent(new NatureSpecialEvent(terrain, types)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Wide;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Move_Start";
            }
            else if (ii == 291)
            {
                skill.Name = new LocalText("Dive");
                skill.Desc = new LocalText("Diving at first, the user floats up and attacks any time in the next five turns.");
                skill.BaseCharges = 18;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                SelfAction altAction = new SelfAction();
                altAction.CharAnimData = new CharAnimFrameType(43);//Hop
                altAction.TargetAlignments |= Alignment.Self;
                SingleEmitter altAnim = new SingleEmitter(new AnimData("Dive", 3));
                altAnim.LocHeight = 16;
                altAction.ActionFX.Emitter = altAnim;
                altAction.ActionFX.Sound = "DUN_Dive";
                skill.Data.BeforeTryActions.Add(0, new ChargeOrReleaseEvent("diving", altAction));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                SingleEmitter anim = new SingleEmitter(new AnimData("Dive", 3));
                anim.LocHeight = 16;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = anim;
                preFX.Sound = "DUN_Dive";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 292)
            {
                skill.Name = new LocalText("Arm Thrust");
                skill.Desc = new LocalText("The user lets loose a flurry of open-palmed arm thrusts that hit five times in a row.");
                skill.BaseCharges = 18;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 50;
                skill.Data.SkillStates.Set(new BasePowerState(20));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 5;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 08;//Strike
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Arm_Thrust";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Arm_Thrust", 3));
            }
            else if (ii == 293)
            {
                skill.Name = new LocalText("-Camouflage");
                skill.Desc = new LocalText("The user's type is changed depending on its environment.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                Dictionary<string, string> terrain = new Dictionary<string, string>();
                terrain.Add("electric_terrain", "electric");
                terrain.Add("grassy_terrain", "grass");
                terrain.Add("misty_terrain", "fairy");
                skill.Data.OnHits.Add(0, new NatureElementEvent(terrain));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Move_Start";
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 294)
            {
                skill.Name = new LocalText("-Tail Glow");
                skill.Desc = new LocalText("The user stares at flashing lights to focus its mind, drastically raising its Sp. Atk stat.");
                skill.BaseCharges = 12;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, 3));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
            }
            else if (ii == 295)
            {
                skill.Name = new LocalText("-Luster Purge");
                skill.Desc = new LocalText("The user lets loose a damaging burst of light. This will also lower the target's Sp. Def stat.");
                skill.BaseCharges = 10;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_special_defense", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 296)
            {
                skill.Name = new LocalText("=Mist Ball");
                skill.Desc = new LocalText("A mist-like flurry of down envelops and damages the target. This will also lower the target's Sp. Atk stat.");
                skill.BaseCharges = 11;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_special_attack", true, true, -1)));
                skill.Strikes = 1;
                skill.Explosion.Range = 1;
                skill.Explosion.Speed = 10;
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Smoke_White", 3));
                emitter.ParticlesPerTile = 1.5;
                skill.Explosion.Emitter = emitter;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Mist_Ball", 2);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 297)
            {
                skill.Name = new LocalText("Feather Dance");
                skill.Desc = new LocalText("The user covers the target's body with a mass of down that harshly lowers its Attack stat.");
                skill.BaseCharges = 18;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 85;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, -2));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(26);//Dance
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 6;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 30;
                CircleSquareSprinkleEmitter emitter = new CircleSquareSprinkleEmitter();
                emitter.Anims.Add(new ParticleAnim(new AnimData("Feather", 5, RogueElements.Dir8.Left), 0, 60));
                emitter.Anims.Add(new ParticleAnim(new AnimData("Feather", 5, RogueElements.Dir8.Right), 0, 60));
                emitter.ParticlesPerTile = 3.5;
                emitter.StartHeight = 24;
                emitter.HeightSpeed = -20;
                emitter.SpeedDiff = 15;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Feather_Dance";
            }
            else if (ii == 298)
            {
                skill.Name = new LocalText("-Teeter Dance");
                skill.Desc = new LocalText("The user performs a wobbly dance that confuses the Pokémon around it.");
                skill.BaseCharges = 11;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("confuse", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(26);//Dance
                ((AreaAction)skill.HitboxAction).Range = 3;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Confused";
            }
            else if (ii == 299)
            {
                skill.Name = new LocalText("Blaze Kick");
                skill.Desc = new LocalText("The user launches a kick that lands a critical hit more easily. This may also leave the target with a burn.");
                skill.BaseCharges = 18;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("burn", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(21);//Kick
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Double_Kick";
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Print_Foot", 12));
                ((AttackAction)skill.HitboxAction).LagBehindTime = 5;
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Flamethrower", 3));
                endAnim.LocHeight = 14;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Flamethrower_2";
            }
            else if (ii == 300)
            {
                skill.Name = new LocalText("Mud Sport");
                skill.Desc = new LocalText("The user covers everyone with mud. This adds the Ground-type to their typing.");
                skill.BaseCharges = 12;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new AddElementEvent("ground"));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(26);//Dance
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 4;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 30;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                CircleSquareFountainEmitter emitter = new CircleSquareFountainEmitter();
                emitter.Anims.Add(new EmittingAnim(new AnimData("Mud", 1, 0, 0), new StaticAnim(new AnimData("Mud", 1, 0, 0), 12), DrawLayer.Normal));
                emitter.Anims.Add(new EmittingAnim(new AnimData("Mud", 1, 1, 1), new StaticAnim(new AnimData("Mud", 1, 1, 1), 12), DrawLayer.Normal));
                emitter.BurstTime = 4;
                emitter.ParticlesPerBurst = 5;
                emitter.Bursts = 8;
                emitter.RangeDiff = 16;
                emitter.HeightRatio = 0.8f;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.ActionFX.Sound = "DUN_Mud_Sport";
            }
            else if (ii == 301)
            {
                skill.Name = new LocalText("-Ice Ball");
                skill.Desc = new LocalText("The user rolls into the target from a distance. It becomes more powerful the farther the target is.");
                skill.BaseCharges = 14;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(15));
                skill.Data.BeforeHits.Add(0, new TipPowerEvent());
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 4;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Ice_Beam_2";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Thorn_Brown", 3));
            }
            else if (ii == 302)
            {
                skill.Name = new LocalText("Needle Arm");
                skill.Desc = new LocalText("The user attacks by wildly swinging its thorny arms. This may also make the target flinch.");
                skill.BaseCharges = 18;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(11);//Punch
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).LagBehindTime = 5;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Punch";
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Print_Fist", 12));
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Needle_Arm_Hit", 3));
                skill.Data.HitFX.Sound = "DUN_Frenzy_Plant";
            }
            else if (ii == 303)
            {
                skill.Name = new LocalText("-Slack Off");
                skill.Desc = new LocalText("The user slacks off, restoring its own HP by up to half of its max HP.");
                skill.BaseCharges = 14;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new RestoreHPEvent(1, 2, true));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
            }
            else if (ii == 304)
            {
                skill.Name = new LocalText("=Hyper Voice");
                skill.Desc = new LocalText("The user lets loose a horribly echoing shout with the power to inflict damage.");
                skill.BaseCharges = 8;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(30);//Sound
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 3;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Wave_Circle_White", 3));
                emitter.ParticlesPerTile = 0.6;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Hyper_Voice";
            }
            else if (ii == 305)
            {
                skill.Name = new LocalText("=Poison Fang");
                skill.Desc = new LocalText("The user bites the target with toxic fangs. This may also leave the target badly poisoned.");
                skill.BaseCharges = 20;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new JawState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(55));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("poison_toxic", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(18);//Bite
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Fangs_Purple", 3));
                ((AttackAction)skill.HitboxAction).LagBehindTime = 10;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                SqueezedAreaEmitter emitter = new SqueezedAreaEmitter(new AnimData("Bubbles_Purple", 3));
                emitter.BurstTime = 3;
                emitter.Bursts = 4;
                emitter.ParticlesPerBurst = 1;
                emitter.Range = 12;
                emitter.StartHeight = -4;
                emitter.HeightSpeed = 12;
                emitter.SpeedDiff = 4;
                skill.Data.HitFX.Emitter = emitter;
            }
            else if (ii == 306)
            {
                skill.Name = new LocalText("=Crush Claw");
                skill.Desc = new LocalText("The user slashes the target with hard and sharp claws. This will also lower the target's Defense.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(55));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_defense", true, true, -1)));
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Wall_Break", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Rollout", terrainEmitter, new FlagType(typeof(WallTerrainState))));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(10);//Scratch
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.FrontAndCorners;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Metal_Claw", 2));
                skill.HitboxAction.ActionFX.Sound = "DUN_Thunder_Fang";
            }
            else if (ii == 307)
            {
                skill.Name = new LocalText("-Blast Burn");
                skill.Desc = new LocalText("The target is razed by a fiery explosion. The user can't move on the next turn.");
                skill.BaseCharges = 8;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(130));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Data.AfterActions.Add(0, new StatusBattleEvent("paused", false, true));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 6;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Blast_Burn", 3));
                emitter.ParticlesPerTile = 2;
                emitter.LocHeight = 8;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Burned", 3));
            }
            else if (ii == 308)
            {
                skill.Name = new LocalText("Hydro Cannon");
                skill.Desc = new LocalText("The target is hit by a watery blast. The user can't move on the next turn.");
                skill.BaseCharges = 8;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new StatusBattleEvent("paused", false, true));
                skill.Data.OnHitTiles.Add(0, new RemoveItemEvent(true));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState)), new FlagType(typeof(LavaTerrainState))));
                skill.Strikes = 1;
                skill.Explosion.Range = 1;
                skill.Explosion.HitTiles = true;
                skill.Explosion.Speed = 10;
                skill.Explosion.ExplodeFX.Sound = "DUN_Waterfall";
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                SingleEmitter tileAnim = new SingleEmitter(new AnimData("Aqua_Tail_Splash", 3));
                tileAnim.LocHeight = 8;
                skill.Explosion.TileEmitter = tileAnim;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Hydro_Cannon", 1, 0, 0);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Dragon_Dance";
            }
            else if (ii == 309)
            {
                skill.Name = new LocalText("=Meteor Mash");
                skill.Desc = new LocalText("The target is hit with a hard punch fired like a meteor. This may also raise the user's Attack stat.");
                skill.BaseCharges = 10;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new FistState());
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new AdditionalEndEvent(new StatusStackBattleEvent("mod_attack", false, true, 1)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 11;//Punch
                ((DashAction)skill.HitboxAction).Range = 4;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Feint_2";
            }
            else if (ii == 310)
            {
                skill.Name = new LocalText("-Astonish");
                skill.Desc = new LocalText("The user attacks the target while shouting in a startling fashion. This may also make the target flinch.");
                skill.BaseCharges = 22;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(35));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.FrontAndCorners;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Payback_2";
            }
            else if (ii == 311)
            {
                skill.Name = new LocalText("Weather Ball");
                skill.Desc = new LocalText("A move that varies in type depending on the weather.");
                skill.BaseCharges = 14;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                Dictionary<string, BattleData> weather = new Dictionary<string, BattleData>();
                {
                    BattleData newData = new BattleData();
                    newData.Element = "water";
                    newData.Category = BattleData.SkillCategory.Magical;
                    newData.HitRate = 100;
                    newData.SkillStates.Set(new BasePowerState(60));
                    newData.OnHits.Add(-1, new DamageFormulaEvent());
                    newData.HitFX.Sound = "DUN_Weather_Ball_Water";
                    newData.HitFX.Emitter = new SingleEmitter(new AnimData("Weather_Ball_Water", 3));
                    weather.Add("rain", newData);
                }
                {
                    BattleData newData = new BattleData();
                    newData.Element = "fire";
                    newData.Category = BattleData.SkillCategory.Magical;
                    newData.HitRate = 100;
                    newData.SkillStates.Set(new BasePowerState(60));
                    newData.OnHits.Add(-1, new DamageFormulaEvent());
                    newData.HitFX.Sound = "DUN_Weather_Ball_Fire";
                    newData.HitFX.Emitter = new SingleEmitter(new AnimData("Weather_Ball_Fire", 3));
                    newData.HitFX.Emitter.LocHeight = 16;
                    weather.Add("sunny", newData);
                }
                {
                    BattleData newData = new BattleData();
                    newData.Element = "rock";
                    newData.Category = BattleData.SkillCategory.Magical;
                    newData.HitRate = 100;
                    newData.SkillStates.Set(new BasePowerState(60));
                    newData.OnHits.Add(-1, new DamageFormulaEvent());
                    newData.HitFX.Sound = "DUN_Weather_Ball_Rock";
                    newData.HitFX.Emitter = new SingleEmitter(new AnimData("Rock_Smash", 2));
                    weather.Add("sandstorm", newData);
                }
                {
                    BattleData newData = new BattleData();
                    newData.Element = "ice";
                    newData.Category = BattleData.SkillCategory.Magical;
                    newData.HitRate = 100;
                    newData.SkillStates.Set(new BasePowerState(60));
                    newData.OnHits.Add(-1, new DamageFormulaEvent());
                    newData.HitFX.Sound = "DUN_Ice_Ball_2";

                    FiniteReleaseEmitter endAnim = new FiniteReleaseEmitter(new AnimData("Ice_Pieces", 6, 0, 0), new AnimData("Ice_Pieces", 12, 1, 1), new AnimData("Ice_Pieces", 12, 1, 1));
                    endAnim.BurstTime = 2;
                    endAnim.ParticlesPerBurst = 4;
                    endAnim.Bursts = 4;
                    endAnim.StartDistance = 4;
                    endAnim.Speed = 60;
                    newData.HitFX.Emitter = endAnim;

                    weather.Add("hail", newData);
                }
                skill.Data.BeforeHits.Add(-5, new WeatherDifferentEvent(weather));

                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Weather_Ball", 2);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Weather_Ball";
            }
            else if (ii == 312)
            {
                skill.Name = new LocalText("-Aromatherapy");
                skill.Desc = new LocalText("The user releases a soothing scent that heals all of the target's bad status conditions.");
                skill.BaseCharges = 18;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.BeforeActions.Add(-1, new AddContextStateEvent(new CureAttack()));
                skill.Data.OnHits.Add(0, new RemoveStateStatusBattleEvent(typeof(BadStatusState), true, new StringKey("MSG_CURE_ALL")));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(19);//Shake
                ((ProjectileAction)skill.HitboxAction).Range = 5;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Aromatherapy_2";
            }
            else if (ii == 313)
            {
                skill.Name = new LocalText("-Fake Tears");
                skill.Desc = new LocalText("The user feigns crying to fluster the target, harshly lowering its Sp. Def stat.");
                skill.BaseCharges = 14;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 80;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_defense", true, false, -2));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Fake_Tears";
            }
            else if (ii == 314)
            {
                skill.Name = new LocalText("Air Cutter");
                skill.Desc = new LocalText("The user launches razor-like wind to slash the opposing Pokémon. Critical hits land more easily.");
                skill.BaseCharges = 14;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 3;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).Rays = ProjectileAction.RayCount.Three;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Sonic_Boom", 3));
                shotAnim.StartDistance = 16;
                shotAnim.Shots = 8;
                shotAnim.BurstTime = 4;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Mist";
                FiniteReleaseRangeEmitter endAnim = new FiniteReleaseRangeEmitter(new AnimData("Razor_Wind", 2));
                endAnim.BurstTime = 2;
                endAnim.ParticlesPerBurst = 3;
                endAnim.Bursts = 2;
                endAnim.StartDistance = 6;
                endAnim.Speed = 60;
                endAnim.Range = GraphicsManager.TileSize;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 315)
            {
                skill.Name = new LocalText("Overheat");
                skill.Desc = new LocalText("The user attacks the target at full power. The attack's recoil harshly lowers the user's Sp. Atk stat.");
                skill.BaseCharges = 6;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Data.AfterActions.Add(0, new StatusStackBattleEvent("mod_special_attack", false, true, -2));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 6;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Flamethrower", 3));
                emitter.ParticlesPerTile = 1;
                emitter.LocHeight = 14;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Overheat_2";
                skill.Data.HitFX.Sound = "DUN_Overheat";
            }
            else if (ii == 316)
            {
                skill.Name = new LocalText("-Odor Sleuth");
                skill.Desc = new LocalText("The user identifies the target, removing its resistances and immunities. This also enables an evasive target to be hit.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("exposed", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Identify_2";
            }
            else if (ii == 317)
            {
                skill.Name = new LocalText("Rock Tomb");
                skill.Desc = new LocalText("Boulders are hurled at the target. This also lowers the target's Speed stat.");
                skill.BaseCharges = 18;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_speed", true, true, -1)));
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Puff_Brown", 3));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Transform", terrainEmitter, new FlagType(typeof(WaterTerrainState)), new FlagType(typeof(LavaTerrainState)), new FlagType(typeof(AbyssTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(31);//Rumble
                ((OffsetAction)skill.HitboxAction).HitArea = OffsetAction.OffsetArea.Tile;
                ((OffsetAction)skill.HitboxAction).Range = 3;
                ((OffsetAction)skill.HitboxAction).Speed = 10;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                ((OffsetAction)skill.HitboxAction).LagBehindTime = 20;
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Rock_Tomb_Back", 2), new AnimData("Rock_Tomb_Front", 2));
                skill.HitboxAction.TileEmitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Giga_Impact";
            }
            else if (ii == 318)
            {
                skill.Name = new LocalText("Silver Wind");
                skill.Desc = new LocalText("The target is attacked with powdery scales blown by wind. This may also raise all the user's stats.");
                skill.BaseCharges = 9;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                GroupEvent groupEvent = new GroupEvent();
                groupEvent.BaseEvents.Add(new StatusStackBattleEvent("mod_speed", false, true, 1));
                groupEvent.BaseEvents.Add(new StatusStackBattleEvent("mod_attack", false, true, 1));
                groupEvent.BaseEvents.Add(new StatusStackBattleEvent("mod_defense", false, true, 1));
                groupEvent.BaseEvents.Add(new StatusStackBattleEvent("mod_special_attack", false, true, 1));
                groupEvent.BaseEvents.Add(new StatusStackBattleEvent("mod_special_defense", false, true, 1));
                skill.Data.AfterActions.Add(0, new AdditionalEndEvent(groupEvent));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(32);//FlapAround
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("Silver_Wind", 3, 0, 0, 128);
                overlay.TotalTime = 60;
                overlay.Movement = new RogueElements.Loc(8, 0);
                overlay.Layer = DrawLayer.Top;
                skill.HitboxAction.ActionFX.Emitter = overlay;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Silver_Wind";
            }
            else if (ii == 319)
            {
                skill.Name = new LocalText("Metal Sound");
                skill.Desc = new LocalText("A horrible sound like scraping metal harshly lowers the target's Sp. Def stat.");
                skill.BaseCharges = 16;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 75;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_defense", true, false, -2));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(35);//Emit
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Metal_Sound_Wave", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Metal_Sound";
            }
            else if (ii == 320)
            {
                skill.Name = new LocalText("Grass Whistle");
                skill.Desc = new LocalText("The user plays a pleasant melody that lulls the target into a deep sleep.");
                skill.BaseCharges = 12;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 65;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("sleep", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(29);//Sing
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Grass_Whistle", 2);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Grass_Whistle";
            }
            else if (ii == 321)
            {
                skill.Name = new LocalText("Tickle");
                skill.Desc = new LocalText("The user tickles the target into laughing, reducing its Attack and Defense stats.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, -1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, -1));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Tickle";
            }
            else if (ii == 322)
            {
                skill.Name = new LocalText("-Cosmic Power");
                skill.Desc = new LocalText("The user absorbs a mystical power from space to raise the party's Defense and Sp. Def stats.");
                skill.BaseCharges = 18;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, 1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_defense", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 30;
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("Cosmic_Power", 3);
                overlay.TotalTime = 60;
                overlay.Movement = new RogueElements.Loc(0, -1);
                overlay.Layer = DrawLayer.Bottom;
                skill.HitboxAction.ActionFX.Emitter = overlay;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Sound = "DUN_Cosmic_Power";
            }
            else if (ii == 323)
            {
                skill.Name = new LocalText("Water Spout");
                skill.Desc = new LocalText("The user spouts water to damage opposing Pokémon. The lower the user's HP, the lower the move's power.");
                skill.BaseCharges = 8;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState());
                skill.Data.OnActions.Add(0, new HPBasePowerEvent(100, false, false));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Puff_Brown", 3));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Transform", terrainEmitter, new FlagType(typeof(LavaTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                SingleEmitter emitter = new SingleEmitter(new AnimData("Water_Spout_Up", 1));
                emitter.LocHeight = 80;
                skill.HitboxAction.PreActions.Add(new BattleFX(emitter, "DUN_Water_Spout", 24));
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(31);//Rumble
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 8;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                ((AreaAction)skill.HitboxAction).ActionFX.Sound = "DUN_Water_Spout_2";
                CircleSquareAreaEmitter areaEmitter = new CircleSquareAreaEmitter(new AnimData("Water_Spout_Down_Single", 1));
                areaEmitter.ParticlesPerTile = 1.8f;
                areaEmitter.RangeDiff = -6;
                areaEmitter.LocHeight = 120;
                ((AreaAction)skill.HitboxAction).Emitter = areaEmitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Delay = 12;
            }
            else if (ii == 324)
            {
                skill.Name = new LocalText("Signal Beam");
                skill.Desc = new LocalText("The user attacks with a sinister beam of light. This may also confuse the target.");
                skill.BaseCharges = 12;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("confuse", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.ActionFX.Sound = "DUN_Copycat";
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Signal_Beam", 2));
                shotAnim.StartDistance = 16;
                shotAnim.Shots = 15;
                shotAnim.BurstTime = 3;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 325)
            {
                skill.Name = new LocalText("Shadow Punch");
                skill.Desc = new LocalText("The user throws a punch from the shadows. This attack never misses.");
                skill.BaseCharges = 20;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new FistState());
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(11);//Punch
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.FrontAndCorners;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("White", 3);
                overlay.TotalTime = 60;
                overlay.Color = Microsoft.Xna.Framework.Color.Black;
                overlay.Layer = DrawLayer.Bottom;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = overlay;
                preFX.Sound = "DUN_Night_Shade_2";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Sound = "DUN_Punch";
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Print_Fist", 12));
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 326)
            {
                skill.Name = new LocalText("-Extrasensory");
                skill.Desc = new LocalText("The user attacks with an odd, unseeable power. This may also make the target flinch.");
                skill.BaseCharges = 15;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 327)
            {
                skill.Name = new LocalText("-Sky Uppercut");
                skill.Desc = new LocalText("The user attacks the target with an uppercut thrown skyward with force.");
                skill.BaseCharges = 18;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new FistState());
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(16);//Uppercut
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 328)
            {
                skill.Name = new LocalText("Sand Tomb");
                skill.Desc = new LocalText("The user traps the target inside a harshly raging sandstorm for three turns.");
                skill.BaseCharges = 18;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new StatusBattleEvent("sand_tomb", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 20;
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Sand_Tomb_Back", 1), new AnimData("Sand_Tomb_Front", 1));
                emitter.HeightBack = 32;
                emitter.HeightFront = 32;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Sand_Tomb";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 329)
            {
                skill.Name = new LocalText("=Sheer Cold");
                skill.Desc = new LocalText("The user attacks with a blast of absolute-zero cold. The target faints instantly if it hits.");
                skill.BaseCharges = 5;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 50;
                skill.Data.BeforeHits.Add(0, new ExplorerImmuneEvent());
                skill.Data.OnHits.Add(-1, new OHKODamageEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter();
                emitter.Anims.Add(new BetweenEmitter(new AnimData("Sheer_Cold_Back", 2), new AnimData("Sheer_Cold_Front", 2)));
                emitter.ParticlesPerTile = 0.7;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Delay = 20;
            }
            else if (ii == 330)
            {
                skill.Name = new LocalText("=Muddy Water");
                skill.Desc = new LocalText("The user attacks by shooting muddy water at the opposing Pokémon. This may also lower their Accuracy.");
                skill.BaseCharges = 10;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_accuracy", true, true, -1)));
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Puff_Brown", 3));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Transform", terrainEmitter, new FlagType(typeof(LavaTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new WaveMotionAction();
                ((WaveMotionAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((WaveMotionAction)skill.HitboxAction).Range = 5;
                ((WaveMotionAction)skill.HitboxAction).Speed = 10;
                ((WaveMotionAction)skill.HitboxAction).Linger = 6;
                ((WaveMotionAction)skill.HitboxAction).Wide = true;
                ((WaveMotionAction)skill.HitboxAction).StopAtWall = true;
                ((WaveMotionAction)skill.HitboxAction).HitTiles = true;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Water_Column_Ranger", 3));
                emitter.LocHeight = 24;
                skill.HitboxAction.TileEmitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Surf";
            }
            else if (ii == 331)
            {
                skill.Name = new LocalText("Bullet Seed");
                skill.Desc = new LocalText("The user forcefully shoots seeds at the target five times in a row.");
                skill.BaseCharges = 15;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 65;
                skill.Data.SkillStates.Set(new BasePowerState(20));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 5;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 16;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Bullet_Seed_Seed", 3, 0, 3), new AnimData("Bullet_Seed_Seed", 3, 4, 7));
                shotAnim.StartDistance = 16;
                shotAnim.Shots = 5;
                shotAnim.BurstTime = 3;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Bullet_Seed";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Vine_Whip_2", 3));
            }
            else if (ii == 332)
            {
                skill.Name = new LocalText("Aerial Ace");
                skill.Desc = new LocalText("The user confounds the target with speed, then slashes. This attack never misses.");
                skill.BaseCharges = 16;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 13;//Slice
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Guillotine";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Slash_Blue_RSE", 3));
            }
            else if (ii == 333)
            {
                skill.Name = new LocalText("-Icicle Spear");
                skill.Desc = new LocalText("The user launches sharp icicles at the target five times in a row.");
                skill.BaseCharges = 16;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 70;
                skill.Data.SkillStates.Set(new BasePowerState(20));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 5;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Icicle_Spear", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 334)
            {
                skill.Name = new LocalText("-Iron Defense");
                skill.Desc = new LocalText("The user hardens its body's surface like iron, sharply raising its Defense stat.");
                skill.BaseCharges = 18;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, 2));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Stat_Up_2";
            }
            else if (ii == 335)
            {
                skill.Name = new LocalText("-Block");
                skill.Desc = new LocalText("The user continually blocks the target's way with arms spread wide to prevent escape.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("chasing", false, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.FrontAndCorners;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("X_RSE", 30));
            }
            else if (ii == 336)
            {
                skill.Name = new LocalText("-Howl");
                skill.Desc = new LocalText("The user howls loudly to raise the party's spirit, which raises the Attack stat.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                ((AreaAction)skill.HitboxAction).Range = 3;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Sound = "_UNK_DUN_Mecha";
            }
            else if (ii == 337)
            {
                skill.Name = new LocalText("=Dragon Claw");
                skill.Desc = new LocalText("The user slashes the target with huge, sharp claws.");
                skill.BaseCharges = 18;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(10);//Scratch
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                BetweenEmitter introAnim = new BetweenEmitter(new AnimData("Fire_Spin_Back", 1), new AnimData("Fire_Spin_Front", 1));
                introAnim.HeightBack = 32;
                introAnim.HeightFront = 32;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = introAnim;
                preFX.Sound = "DUN_Fire_Spin";
                preFX.Delay = 10;
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Fury_Cutter";
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Metal_Claw", 2));
            }
            else if (ii == 338)
            {
                skill.Name = new LocalText("-Frenzy Plant");
                skill.Desc = new LocalText("The user slams the target with an enormous tree. The user can't move on the next turn.");
                skill.BaseCharges = 8;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(130));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new StatusBattleEvent("paused", false, true));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 3;
                ((AreaAction)skill.HitboxAction).Speed = 4;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Volt_Tackle";
            }
            else if (ii == 339)
            {
                skill.Name = new LocalText("-Bulk Up");
                skill.Desc = new LocalText("The user tenses its muscles to bulk up its body, raising both its Attack and Defense stats.");
                skill.BaseCharges = 16;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
            }
            else if (ii == 340)
            {
                skill.Name = new LocalText("Bounce");
                skill.Desc = new LocalText("The user bounces up high, then drops on the target on the second turn. This may also leave the target with paralysis.");
                skill.BaseCharges = 16;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                SelfAction altAction = new SelfAction();
                altAction.CharAnimData = new CharAnimProcess(CharAnimProcess.ProcessType.Fly);//Fly
                altAction.TargetAlignments |= Alignment.Self;
                BattleFX altPreFX = new BattleFX();
                altPreFX.Sound = "DUN_Fly";
                altAction.PreActions.Add(altPreFX);
                skill.Data.BeforeTryActions.Add(0, new ChargeOrReleaseEvent("bouncing", altAction));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("paralyze", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).AppearanceMod = DashAction.DashAppearance.DropDown;
                ((DashAction)skill.HitboxAction).Range = 4;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                skill.HitboxAction.ActionFX.Sound = "DUN_Tackle";
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 341)
            {
                skill.Name = new LocalText("-Mud Shot");
                skill.Desc = new LocalText("The user attacks by hurling a blob of mud at the target. This also lowers the target's Movement Speed.");
                skill.BaseCharges = 12;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_speed", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "_UNK_DUN_Mecha";
            }
            else if (ii == 342)
            {
                skill.Name = new LocalText("-Poison Tail");
                skill.Desc = new LocalText("The user hits the target with its tail. This may also poison the target. Critical hits land more easily.");
                skill.BaseCharges = 18;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("poison", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Wide;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Hammer_Arm";
            }
            else if (ii == 343)
            {
                skill.Name = new LocalText("-Covet");
                skill.Desc = new LocalText("The user cutely begs to obtain an item from the foe.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 80;
                skill.Data.OnHits.Add(0, new BegItemEvent(true, false, "seed_decoy", new HashSet<FlagType>()));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Charm_Heart", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                //SingleEmitter endAnim = new SingleEmitter();
                //endAnim.Anim = new AnimData("Hit_Neutral", 3);
                //skill.SkillEffect.EndAnim = endAnim;
            }
            else if (ii == 344)
            {
                skill.Name = new LocalText("-Volt Tackle");
                skill.Desc = new LocalText("The user electrifies itself, then charges. This also damages the user quite a lot. This may leave the target with paralysis.");
                skill.BaseCharges = 9;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("paralyze", true, true)));
                skill.Data.AfterActions.Add(0, new HPRecoilEvent(4, false));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 5;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Flash_Cannon";
            }
            else if (ii == 345)
            {
                skill.Name = new LocalText("Magical Leaf");
                skill.Desc = new LocalText("The user scatters curious leaves that chase the target. This attack never misses.");
                skill.BaseCharges = 14;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(55));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Magical_Leaf", 1);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Magical_Leaf";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Magical_Leaf_Charge", 2));
                endAnim.LocHeight = 20;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 346)
            {
                skill.Name = new LocalText("Water Sport");
                skill.Desc = new LocalText("The user soaks everyone with water. This adds the Water-type to their typing.");
                skill.BaseCharges = 12;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new AddElementEvent("water"));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 3;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 30;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                CircleSquareFountainEmitter emitter = new CircleSquareFountainEmitter();
                emitter.Anims.Add(new EmittingAnim(new AnimData("Water_Sport", 1, 0, 0), new StaticAnim(new AnimData("Water_Sport", 1, 0, 3)), DrawLayer.Normal));
                emitter.Anims.Add(new EmittingAnim(new AnimData("Water_Sport", 1, 4, 4), new StaticAnim(new AnimData("Water_Sport", 1, 4, 7)), DrawLayer.Normal));
                emitter.BurstTime = 4;
                emitter.ParticlesPerBurst = 5;
                emitter.Bursts = 8;
                emitter.RangeDiff = 16;
                emitter.HeightRatio = 0.8f;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.ActionFX.Sound = "DUN_Water_Sport";
            }
            else if (ii == 347)
            {
                skill.Name = new LocalText("-Calm Mind");
                skill.Desc = new LocalText("The user quietly focuses its mind and calms its spirit to raise its Sp. Atk and Sp. Def stats.");
                skill.BaseCharges = 15;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, 1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_defense", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("White", 3);
                overlay.TotalTime = 60;
                overlay.Color = Microsoft.Xna.Framework.Color.Black;
                overlay.Layer = DrawLayer.Bottom;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = overlay;
                skill.HitboxAction.PreActions.Add(preFX);
                RepeatEmitter endAnim = new RepeatEmitter(new AnimData("Circle_Thick_Red_In", 2));
                endAnim.Bursts = 3;
                endAnim.BurstTime = 12;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 348)
            {
                skill.Name = new LocalText("-Leaf Blade");
                skill.Desc = new LocalText("The user handles a sharp leaf like a sword and attacks by cutting its target. Critical hits land more easily.");
                skill.BaseCharges = 16;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(13);//Slice
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 349)
            {
                skill.Name = new LocalText("Dragon Dance");
                skill.Desc = new LocalText("The user vigorously performs a mystic, powerful dance that boosts its Attack and Movement Speed.");
                skill.BaseCharges = 15;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(27);//Twirl
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Dragon_Dance", 2));
                preFX.Sound = "DUN_Dragon_Dance";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
            }
            else if (ii == 350)
            {
                skill.Name = new LocalText("-Rock Blast");
                skill.Desc = new LocalText("The user hurls hard rocks at the target. Five rocks are launched in a row.");
                skill.BaseCharges = 15;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 50;
                skill.Data.SkillStates.Set(new BasePowerState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 5;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "_UNK_DUN_Break";
            }
            else if (ii == 351)
            {
                skill.Name = new LocalText("Shock Wave");
                skill.Desc = new LocalText("The user strikes the target with a quick jolt of electricity. This attack never misses.");
                skill.BaseCharges = 10;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Shock_Wave";
                SingleEmitter emitter = new SingleEmitter(new BeamAnimData("Lightning", 3));
                emitter.LocHeight = 8;
                skill.HitboxAction.TileEmitter = emitter;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Spark", 3));
                skill.Data.HitFX.Sound = "DUN_Shock_Wave_2";
            }
            else if (ii == 352)
            {
                skill.Name = new LocalText("=Water Pulse");
                skill.Desc = new LocalText("The user attacks the target with a pulsing blast of water. This may also confuse the target.");
                skill.BaseCharges = 12;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new PulseState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("confuse", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 3;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Wave_Circle_Blue", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "_UNK_DUN_Water_Echo_2";
            }
            else if (ii == 353)
            {
                skill.Name = new LocalText("-Doom Desire");
                skill.Desc = new LocalText("Five turns after this move is used, the user blasts the target's area with a concentrated bundle of light.");
                skill.BaseCharges = 8;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(130));
                skill.Data.OnHits.Add(-1, new FutureAttackEvent("doom_desire", true, false, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 4;
                ((ThrowAction)skill.HitboxAction).LagBehindTime = 20;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Leer", 2));
                preFX.Sound = "DUN_Psycho_Shift";
                skill.HitboxAction.PreActions.Add(preFX);
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("White", 3);
                overlay.TotalTime = 60;
                overlay.Color = Microsoft.Xna.Framework.Color.Black;
                overlay.Layer = DrawLayer.Bottom;
                skill.Data.HitFX.Emitter = overlay;
                skill.Data.HitFX.Sound = "_UNK_DUN_Suspense";
                //skill.Data.Emitter = new ColumnEmitter(new AnimData("Column_Yellow", 3, -1, -1, 192));
            }
            else if (ii == 354)
            {
                skill.Name = new LocalText("Psycho Boost");
                skill.Desc = new LocalText("The user attacks the target at full power. The attack's recoil harshly lowers the user's Sp. Atk stat.");
                skill.BaseCharges = 6;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new StatusStackBattleEvent("mod_special_attack", false, true, -2));
                skill.Strikes = 1;
                skill.Explosion.Range = 1;
                skill.Explosion.HitTiles = true;
                skill.Explosion.Speed = 10;
                skill.Explosion.ExplodeFX.Sound = "DUN_Zen_Headbutt";
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Giga_Impact_Back", 2), new AnimData("Giga_Impact_Front", 2));
                skill.Explosion.ExplodeFX.Emitter = emitter;
                skill.HitboxAction = new ProjectileAction();
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("Psychic", 3, -1, -1, 128);
                overlay.TotalTime = 60;
                overlay.Movement = new RogueElements.Loc(0, 0);
                overlay.Layer = DrawLayer.Bottom;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = overlay;
                preFX.Sound = "DUN_Psycho_Boost";
                skill.HitboxAction.PreActions.Add(preFX);
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Focus_Blast_Ball", 2);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Blowback_Orb";
            }
            else if (ii == 355)
            {
                skill.Name = new LocalText("Roost");
                skill.Desc = new LocalText("The user lands and rests its body. It restores the user's HP by up to half of its max HP.");
                skill.BaseCharges = 10;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new RestoreHPEvent(1, 2, true));
                skill.Data.OnHits.Add(0, new StatusBattleEvent("roosting", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                BetweenEmitter endAnim = new BetweenEmitter(new AnimData("Roost_Back", 2), new AnimData("Roost_Front", 2));
                endAnim.HeightBack = 12;
                endAnim.HeightFront = 12;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Roost";
                skill.Data.HitFX.Delay = 20;
            }
            else if (ii == 356)
            {
                skill.Name = new LocalText("Gravity");
                skill.Desc = new LocalText("Gravity is intensified for the entire floor, making moves involving flying unusable and negating Levitate.");
                skill.BaseCharges = 15;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new GiveMapStatusEvent("gravity"));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Gravity";
                ((SelfAction)skill.HitboxAction).LagBehindTime = 24;
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Gravity", 1));
                skill.Explosion.TargetAlignments = Alignment.Self;
            }
            else if (ii == 357)
            {
                skill.Name = new LocalText("=Miracle Eye");
                skill.Desc = new LocalText("The user takes on the Miracle Eye status, allowing it to ignore resistances and immunities. This also enables it to hit evasive targets.");
                skill.BaseCharges = 18;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("miracle_eye", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Miracle_Eye";
                ((SelfAction)skill.HitboxAction).LagBehindTime = 16;
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Miracle_Eye_Eye", 2));
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Miracle_Eye_Glow", 2));
            }
            else if (ii == 358)
            {
                skill.Name = new LocalText("Wake-Up Slap");
                skill.Desc = new LocalText("This attack inflicts big damage on a sleeping target, immediately waking it up. Allies targeted by this move will not take damage.");
                skill.BaseCharges = 18;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));

                BattleData newData = new BattleData();
                newData.Element = "fighting";
                newData.Category = BattleData.SkillCategory.Status;
                newData.HitRate = -1;
                newData.OnHits.Add(0, new StatusBattleEvent("sleepless", true, false));
                skill.Data.BeforeHits.Add(-5, new AlignmentDifferentEvent(Alignment.Friend | Alignment.Self, newData));

                skill.Data.BeforeHits.Add(0, new StatusPowerEvent("sleep", true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new StatusBattleEvent("sleepless", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(12);//Slap
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Friend | Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Friend | Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Wake_Up_Slap";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Wake_Up_Slap", 2));
                endAnim.LocHeight = 8;
                ((AttackAction)skill.HitboxAction).Emitter = endAnim;
                skill.Data.HitFX.Delay = 30;
            }
            else if (ii == 359)
            {
                skill.Name = new LocalText("-Hammer Arm");
                skill.Desc = new LocalText("The user swings and hits with its strong and heavy fist. It lowers the user's Movement Speed, however.");
                skill.BaseCharges = 18;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new FistState());
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new StatusStackBattleEvent("mod_speed", false, true, -1));
                skill.Strikes = 1;
                skill.Explosion.ExplodeFX.ScreenMovement = new ScreenMover(0, 8, 30);
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(23);//Slam
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Hammer_Arm_Fist", 2));
                ((AttackAction)skill.HitboxAction).LagBehindTime = 40;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Feint_2";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Hammer_Arm_Smoke", 3));
            }
            else if (ii == 360)
            {
                skill.Name = new LocalText("Gyro Ball");
                skill.Desc = new LocalText("The user tackles the target with a high-speed spin. The slower the user compared to the target, the greater the move's power.");
                skill.BaseCharges = 15;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.BeforeHits.Add(0, new SpeedPowerEvent(true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 17;//Ricochet
                ((DashAction)skill.HitboxAction).Range = 4;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Gyro_Ball", 1));
                preFX.Sound = "DUN_Gyro_Ball";
                preFX.Delay = 15;
                skill.HitboxAction.PreActions.Add(preFX);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Gyro_Ball_Hit", 2));
            }
            else if (ii == 361)
            {
                skill.Name = new LocalText("-Healing Wish");
                skill.Desc = new LocalText("The user's HP drops to 1. In return, the rest of the party will have its HP restored and status conditions cured.");
                skill.BaseCharges = 6;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = -1;
                skill.Data.BeforeActions.Add(-1, new AddContextStateEvent(new CureAttack()));
                skill.Data.OnHits.Add(0, new RestoreHPEvent(1, 1, true));
                skill.Data.OnHits.Add(0, new RemoveStateStatusBattleEvent(typeof(BadStatusState), true, new StringKey("MSG_CURE_ALL")));
                skill.Data.AfterActions.Add(0, new HPTo1Event(false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Healing_Wish_Charge", 3));
                preFX.Sound = "DUN_Healing_Wish";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Healing_Wish_Projectile", 3));
                skill.HitboxAction.TargetAlignments = Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Friend;
                skill.Explosion.ExplodeFX.Emitter = new SingleEmitter(new AnimData("Healing_Wish_Projectile", 3));
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Healing_Wish_Hit", 2));
                skill.Data.HitFX.Sound = "DUN_Healing_Wish_2";
            }
            else if (ii == 362)
            {
                skill.Name = new LocalText("Brine");
                skill.Desc = new LocalText("If the target's HP is half or less, this attack will hit with double the power.");
                skill.BaseCharges = 15;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.BeforeHits.Add(0, new BrineEvent());
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Puff_Brown", 3));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Transform", terrainEmitter, new FlagType(typeof(LavaTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new WaveMotionAction();
                skill.HitboxAction.PreActions.Add(new BattleFX(new SingleEmitter(new AnimData("Charge_Up", 3)), "DUN_Move_Start", 0));
                ((WaveMotionAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((WaveMotionAction)skill.HitboxAction).Range = 1;
                ((WaveMotionAction)skill.HitboxAction).Speed = 10;
                ((WaveMotionAction)skill.HitboxAction).Linger = 6;
                ((WaveMotionAction)skill.HitboxAction).Wide = true;
                ((WaveMotionAction)skill.HitboxAction).StopAtWall = true;
                ((WaveMotionAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Brine";
                SingleEmitter emitter = new SingleEmitter(new AnimData("Brine", 1));
                emitter.LocHeight = 96;
                skill.HitboxAction.TileEmitter = emitter;
                skill.Data.HitFX.Delay = 16;
            }
            else if (ii == 363)
            {
                skill.Name = new LocalText("Natural Gift");
                skill.Desc = new LocalText("The user draws power to attack by using its held Berry. The Berry determines the move's type.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));

                Dictionary<string, string> itemPair = new Dictionary<string, string>();
                itemPair.Add("berry_oran", "poison");
                itemPair.Add("berry_leppa", "fighting");
                itemPair.Add("berry_lum", "flying");
                itemPair.Add("berry_sitrus", "psychic");

                itemPair.Add("berry_tanga", "bug");
                itemPair.Add("berry_colbur", "dark");
                itemPair.Add("berry_haban", "dragon");
                itemPair.Add("berry_wacan", "electric");
                itemPair.Add("berry_chople", "fighting");
                itemPair.Add("berry_occa", "fire");
                itemPair.Add("berry_coba", "flying");
                itemPair.Add("berry_kasib", "ghost");
                itemPair.Add("berry_rindo", "grass");
                itemPair.Add("berry_shuca", "ground");
                itemPair.Add("berry_yache", "ice");
                itemPair.Add("berry_chilan", "normal");
                itemPair.Add("berry_kebia", "poison");
                itemPair.Add("berry_payapa", "psychic");
                itemPair.Add("berry_charti", "rock");
                itemPair.Add("berry_babiri", "steel");
                itemPair.Add("berry_passho", "water");
                itemPair.Add("berry_roseli", "fairy");

                itemPair.Add("berry_jaboca", "dragon");
                itemPair.Add("berry_rowap", "dark");
                itemPair.Add("berry_liechi", "grass");
                itemPair.Add("berry_ganlon", "ice");
                itemPair.Add("berry_salac", "fighting");
                itemPair.Add("berry_petaya", "poison");
                itemPair.Add("berry_apicot", "ground");
                itemPair.Add("berry_starf", "psychic");
                itemPair.Add("berry_micle", "rock");
                itemPair.Add("berry_enigma", "bug");

                skill.Data.OnActions.Add(-1, new ItemPowerEvent(itemPair));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Wide;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Natural_Gift", 1));
                endAnim.LocHeight = 8;
                endAnim.Offset = 12;

                BattleFX preFX = new BattleFX();
                preFX.Emitter = endAnim;
                preFX.Sound = "DUN_Natural_Gift";
                skill.HitboxAction.PreActions.Add(preFX);
                ((AttackAction)skill.HitboxAction).LagBehindTime = 25;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 364)
            {
                skill.Name = new LocalText("-Feint");
                skill.Desc = new LocalText("An attack that hits a target trying to protect itself. This also lifts the effects of those moves.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("wide_guard", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("quick_guard", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("protect", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("all_protect", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("detect", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("kings_shield", true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 11;//Punch
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Feint_2";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Feint_Hit", 2));
            }
            else if (ii == 365)
            {
                skill.Name = new LocalText("Pluck");
                skill.Desc = new LocalText("The user pecks the target. If the target is holding a Berry, the user eats it and gains its effect.");
                skill.BaseCharges = 16;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                HashSet<FlagType> eligibles = new HashSet<FlagType>();
                eligibles.Add(new FlagType(typeof(EdibleState)));
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new UseFoeItemEvent(false, false, "seed_decoy", eligibles, true, false)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(20);//Jab
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Attack";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Sound = "DUN_Pluck";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Pluck", 1));
            }
            else if (ii == 366)
            {
                skill.Name = new LocalText("Tailwind");
                skill.Desc = new LocalText("The user whips up a turbulent whirlwind that sharply raises the Movement Speed of the user and its allies.");
                skill.BaseCharges = 12;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, 2));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 6;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Sound = "DUN_Tailwind_2";
                skill.HitboxAction.TileEmitter = new SingleEmitter(new AnimData("Tailwind", 1));
            }
            else if (ii == 367)
            {
                skill.Name = new LocalText("Acupressure");
                skill.Desc = new LocalText("The user applies pressure to an ally's stress points, sharply boosting its highest stat.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new AffectHighestStatBattleEvent(true, "mod_attack", "mod_defense", "mod_special_attack", "mod_special_defense", true, 2));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 20;//Jab
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.Front;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Friend;
                skill.HitboxAction.ActionFX.Sound = "DUN_SpDef_Up";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Acupressure", 2));
                endAnim.LocHeight = 16;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Accupressure";
                skill.Data.HitFX.Delay = 30;
            }
            else if (ii == 368)
            {
                skill.Name = new LocalText("Metal Burst");
                skill.Desc = new LocalText("When damaged, the user retaliates with much greater power against nearby opponents.");
                skill.BaseCharges = 8;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("metal_burst", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Metal_Burst_Charge", 2));
                skill.Data.HitFX.Sound = "DUN_Metal_Burst";
            }
            else if (ii == 369)
            {
                skill.Name = new LocalText("U-turn");
                skill.Desc = new LocalText("After making its attack, the user jumps back several steps.");
                skill.BaseCharges = 20;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new OnHitAnyEvent(true, 100, new HopEvent(3, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                ((DashAction)skill.HitboxAction).AppearanceMod = DashAction.DashAppearance.Invisible;
                ((DashAction)skill.HitboxAction).Anim = new AnimData("U_Turn_Travel", 2);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("U_Turn", 1));
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Sound = "DUN_U-Turn";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("U_Turn_Hit", 2));
                skill.Data.HitFX.Sound = "DUN_U-Turn_2";
            }
            else if (ii == 370)
            {
                skill.Name = new LocalText("Close Combat");
                skill.Desc = new LocalText("The user fights the target up close without guarding itself. This also lowers the user's Defense and Sp. Def stats.");
                skill.BaseCharges = 14;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new StatusStackBattleEvent("mod_defense", false, true, -1));
                skill.Data.AfterActions.Add(0, new StatusStackBattleEvent("mod_special_defense", false, true, -1));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(15);//MultiStrike
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Close_Combat", 2));
                skill.HitboxAction.ActionFX.Sound = "DUN_Close_Combat";
            }
            else if (ii == 371)
            {
                skill.Name = new LocalText("Payback");
                skill.Desc = new LocalText("The user stores power, then attacks. If the user was last hit by the target, this attack's power will be doubled.");
                skill.BaseCharges = 18;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.BeforeHits.Add(0, new RevengeEvent("last_targeted_by", 2, 1, false, true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Payback_2";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Payback_Hit", 2));
            }
            else if (ii == 372)
            {
                skill.Name = new LocalText("Assurance");
                skill.Desc = new LocalText("If the target was last attacked by the user, this attack's power is doubled.");
                skill.BaseCharges = 16;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.BeforeHits.Add(0, new RevengeEvent("last_targeted_by", 2, 1, true, true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(08);//Strike
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Assurance", 2));
                skill.Data.HitFX.Delay = 20;
                skill.HitboxAction.ActionFX.Sound = "DUN_Assurance";
            }
            else if (ii == 373)
            {
                skill.Name = new LocalText("Embargo");
                skill.Desc = new LocalText("This move prevents foes from using any items.");
                skill.BaseCharges = 16;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("embargo", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 3;
                ((AreaAction)skill.HitboxAction).Speed = 24;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Embargo";
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Embargo_Front", 2), new AnimData("Embargo_Back", 2));
                skill.Data.HitFX.Emitter = emitter;
            }
            else if (ii == 374)
            {
                skill.Name = new LocalText("Fling");
                skill.Desc = new LocalText("The user flings the target into the distance. The target takes damage unless it lands on another Pokémon.");
                skill.BaseCharges = 17;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new ThrowBackEvent(6, new MaxHPDamageEvent(8)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                preFX.Sound = "DUN_Move_Start";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 375)
            {
                skill.Name = new LocalText("Psycho Shift");
                skill.Desc = new LocalText("Using its psychic power of suggestion, the user transfers its bad status conditions to the target.");
                skill.BaseCharges = 15;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new TransferStatusEvent(true, true, true, true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 4;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Psycho_Shift_Charge", 2));
                preFX.Sound = "DUN_Roar_of_Time";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Psycho_Shift_Hit", 2));
                skill.Data.HitFX.Delay = 10;
            }
            else if (ii == 376)
            {
                skill.Name = new LocalText("Trump Card");
                skill.Desc = new LocalText("The more PP this move has, the greater its power.");
                skill.BaseCharges = 5;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState());
                skill.Data.BeforeHits.Add(0, new PPBasePowerEvent(150, false, false));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Trump_Card_Card", 3);
                AttachReleaseRangeEmitter emitter = new AttachReleaseRangeEmitter(new AnimData("Trump_Card_Card", 1, 0, 0), new AnimData("Trump_Card_Card", 1, 1, 1), new AnimData("Trump_Card_Card", 1, 2, 2));
                emitter.BurstTime = 3;
                emitter.ParticlesPerBurst = 1;
                emitter.Range = 8;
                emitter.Speed = 12;
                ((ProjectileAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Blowback_Orb";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Trump_Card_Hit", 2));
                skill.Data.HitFX.Sound = "DUN_Trump_Card";
                skill.Data.HitFX.Delay = 45;
            }
            else if (ii == 377)
            {
                skill.Name = new LocalText("Heal Block");
                skill.Desc = new LocalText("The user prevents the opposing team from recovering HP.");
                skill.BaseCharges = 16;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("heal_block", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                CircleSquareSprinkleEmitter emitter = new CircleSquareSprinkleEmitter();
                emitter.Anims.Add(new EmittingAnim(new AnimData("Heal_Block_Sparkle", 10, 0, 2), new StaticAnim(new AnimData("Heal_Block_Sparkle", 20, 2, 2)), DrawLayer.Normal, 0, 14));
                emitter.Anims.Add(new EmittingAnim(new AnimData("Heal_Block_Sparkle", 10, 3, 5), new StaticAnim(new AnimData("Heal_Block_Sparkle", 20, 5, 5)), DrawLayer.Normal, 0, 14));
                emitter.Anims.Add(new EmittingAnim(new AnimData("Heal_Block_Sparkle", 10, 6, 8), new StaticAnim(new AnimData("Heal_Block_Sparkle", 20, 8, 8)), DrawLayer.Normal, 0, 14));
                emitter.ParticlesPerTile = 0.8;
                emitter.HeightSpeed = 24;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Heal_Block";
            }
            else if (ii == 378)
            {
                skill.Name = new LocalText("Wring Out");
                skill.Desc = new LocalText("The user powerfully wrings the target. The more HP the target has, the greater the move's power.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState());
                skill.Data.BeforeHits.Add(0, new HPBasePowerEvent(100, false, true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Wring_Out", 1));
                ((AttackAction)skill.HitboxAction).LagBehindTime = 10;
                skill.HitboxAction.ActionFX.Sound = "DUN_Wring_Out";
            }
            else if (ii == 379)
            {
                skill.Name = new LocalText("=Power Trick");
                skill.Desc = new LocalText("The user employs its psychic power to switch its Attack with its Defense stat.");
                skill.BaseCharges = 12;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new PowerTrickEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Power_Trick", 2));
            }
            else if (ii == 380)
            {
                skill.Name = new LocalText("Gastro Acid");
                skill.Desc = new LocalText("The user hurls up its stomach acids on the target. The fluid eliminates the effect of the target's Ability.");
                skill.BaseCharges = 18;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new ChangeToAbilityEvent("none", true));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Gastro_Acid_Ball", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_BubbleBeam";
                skill.Data.HitFX.Emitter = new BetweenEmitter(new AnimData("Gastro_Acid_Back", 3), new AnimData("Gastro_Acid_Front", 3));
                skill.Data.HitFX.Sound = "DUN_Gastro_Acid";
            }
            else if (ii == 381)
            {
                skill.Name = new LocalText("=Lucky Chant");
                skill.Desc = new LocalText("The user chants an incantation toward the sky, preventing opposing Pokémon from landing critical hits or additional effects.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("lucky_chant", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(29);//Sing
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 6;
                ((AreaAction)skill.HitboxAction).ActionFX.Delay = 40;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Sound = "DUN_Petrify_Orb";
                ((AreaAction)skill.HitboxAction).ActionFX.Emitter = new BetweenEmitter(new AnimData("Lucky_Chant_Back", 2), new AnimData("Lucky_Chant_Front", 2));
                CircleSquareAreaEmitter areaEmitter = new CircleSquareAreaEmitter(new AnimData("Lucky_Chant_Star_Violet", 3, 0, 3), new AnimData("Lucky_Chant_Star_Violet", 3, 4, 7));
                areaEmitter.ParticlesPerTile = 1.5;
                ((AreaAction)skill.HitboxAction).Emitter = areaEmitter;
            }
            else if (ii == 382)
            {
                skill.Name = new LocalText("=Me First");
                skill.Desc = new LocalText("The user cuts ahead of the target to steal and use the target's strongest move. It fails if the target has no attacking moves.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StrongestMoveEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(43);//Hop
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 4;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Me_First_Rings", 2));
                skill.HitboxAction.ActionFX.Sound = "DUN_Copycat_2";
                //anims 1022, 1023
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Me_First_Gather", 1));
                skill.Data.HitFX.Delay = 40;
            }
            else if (ii == 383)
            {
                skill.Name = new LocalText("Copycat");
                skill.Desc = new LocalText("The user mimics the move last used by the target. The move fails if the target hasn't used a move yet.");
                skill.BaseCharges = 25;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new MirrorMoveEvent("last_used_move"));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 4;
                skill.HitboxAction.TargetAlignments = (Alignment.Foe | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Foe | Alignment.Friend);
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Copycat_Back", 2));
                preFX.Sound = "DUN_Copycat_2";
                preFX.Delay = 20;
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Copycat_Front", 3));
                skill.Data.HitFX.Delay = 30;
            }
            else if (ii == 384)
            {
                skill.Name = new LocalText("-Power Swap");
                skill.Desc = new LocalText("The user employs its psychic power to switch changes to its Attack and Sp. Atk stats with the target.");
                skill.BaseCharges = 16;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                HashSet<string> statChanges = new HashSet<string>();
                statChanges.Add("mod_attack");
                statChanges.Add("mod_special_attack");
                skill.Data.OnHits.Add(0, new SwapStatsEvent(statChanges));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 8;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Copycat_2";
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Heart_Swap_Hit", 1));
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Heart_Swap_Hit", 1));
                skill.Data.HitFX.Sound = "DUN_Copycat_2";
            }
            else if (ii == 385)
            {
                skill.Name = new LocalText("-Guard Swap");
                skill.Desc = new LocalText("The user employs its psychic power to switch changes to its Defense and Sp. Def stats with the target.");
                skill.BaseCharges = 16;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                HashSet<string> statChanges = new HashSet<string>();
                statChanges.Add("mod_defense");
                statChanges.Add("mod_special_defense");
                skill.Data.OnHits.Add(0, new SwapStatsEvent(statChanges));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Copycat_2";
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Heart_Swap_Hit", 1));
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Heart_Swap_Hit", 1));
                skill.Data.HitFX.Sound = "DUN_Copycat_2";
            }
            else if (ii == 386)
            {
                skill.Name = new LocalText("Punishment");
                skill.Desc = new LocalText("The more the target has powered up with stat changes, the greater the move's power.");
                skill.BaseCharges = 18;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                HashSet<string> statChanges = new HashSet<string>();
                statChanges.Add("mod_speed");
                statChanges.Add("mod_attack");
                statChanges.Add("mod_defense");
                statChanges.Add("mod_special_attack");
                statChanges.Add("mod_special_defense");
                statChanges.Add("mod_accuracy");
                statChanges.Add("mod_evasion");
                skill.Data.BeforeHits.Add(0, new StatBasePowerEvent(20, true, statChanges));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(08);//Strike
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Defense_Curl";
                SingleEmitter emitter = new SingleEmitter(new AnimData("Punishment_Whip", 3));
                emitter.LocHeight = 12;
                ((AttackAction)skill.HitboxAction).Emitter = emitter;
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Punishment_Hit", 3));
                endAnim.LocHeight = 12;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Delay = 20;
            }
            else if (ii == 387)
            {
                skill.Name = new LocalText("Last Resort");
                skill.Desc = new LocalText("The less total PP the user has, the greater the move's power.");
                skill.BaseCharges = 5;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState());
                skill.Data.BeforeHits.Add(0, new PPBasePowerEvent(150, true, true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                ((AreaAction)skill.HitboxAction).ActionFX.Delay = 20;
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Last_Resort_Back", 1), new AnimData("Last_Resort_Front", 1));
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Last_Resort";
                skill.HitboxAction.PreActions.Add(preFX);
                CircleSquareAreaEmitter areaEmitter = new CircleSquareAreaEmitter(new AnimData("Lucky_Chant_Star_Yellow", 3, 0, 3), new AnimData("Lucky_Chant_Star_Yellow", 3, 4, 7));
                areaEmitter.ParticlesPerTile = 1.5;
                ((AreaAction)skill.HitboxAction).Emitter = areaEmitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 388)
            {
                skill.Name = new LocalText("Worry Seed");
                skill.Desc = new LocalText("A seed that causes worry is planted on the target. It prevents sleep by making the target's Ability Insomnia.");
                skill.BaseCharges = 16;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new ChangeToAbilityEvent("insomnia", true));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 4;
                ((ThrowAction)skill.HitboxAction).Speed = 6;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Worry_Seed_Seed", 3);
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Bullet_Seed";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Worry_Seed_Hit", 2));
                endAnim.LocHeight = 16;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Worry_Seed";
            }
            else if (ii == 389)
            {
                skill.Name = new LocalText("Sucker Punch");
                skill.Desc = new LocalText("This attacks foes from a distance. It fails on targets directly in front.");
                skill.BaseCharges = 16;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.BeforeHits.Add(0, new DistanceOnlyEvent());
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 4;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Sucker_Punch";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Sucker_Punch_Hit", 3));
            }
            else if (ii == 390)
            {
                skill.Name = new LocalText("-Toxic Spikes");
                skill.Desc = new LocalText("The user lays a trap of poison spikes around itself. They badly poison opposing Pokémon that step on it.");
                skill.BaseCharges = 18;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHitTiles.Add(0, new SetTrapEvent("trap_toxic_spikes"));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 391)
            {
                skill.Name = new LocalText("Heart Swap");
                skill.Desc = new LocalText("The user employs its psychic power to switch stat changes with the target.");
                skill.BaseCharges = 15;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                HashSet<string> statChanges = new HashSet<string>();
                statChanges.Add("mod_speed");
                statChanges.Add("mod_attack");
                statChanges.Add("mod_defense");
                statChanges.Add("mod_special_attack");
                statChanges.Add("mod_special_defense");
                statChanges.Add("mod_accuracy");
                statChanges.Add("mod_evasion");
                skill.Data.OnHits.Add(0, new SwapStatsEvent(statChanges));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 4;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Copycat_2";
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Heart_Swap_Hit", 1));
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Heart_Swap_Hit", 1));
                skill.Data.HitFX.Sound = "DUN_Copycat_2";
            }
            else if (ii == 392)
            {
                skill.Name = new LocalText("Aqua Ring");
                skill.Desc = new LocalText("The user envelops the party in a veil made of water. It restores some HP every turn.");
                skill.BaseCharges = 16;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("aqua_ring", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Aqua_Ring", 2));
                skill.HitboxAction.ActionFX.Sound = "DUN_Aqua_Ring";
            }
            else if (ii == 393)
            {
                skill.Name = new LocalText("Magnet Rise");
                skill.Desc = new LocalText("The user levitates itself and allies using electrically generated magnetism.");
                skill.BaseCharges = 16;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("magnet_rise", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).ActionFX.Delay = 40;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Sound = "DUN_Magnet_Rise";
                BetweenEmitter introAnim = new BetweenEmitter(new AnimData("Magnet_Rise_Back", 2), new AnimData("Magnet_Rise_Front", 2));
                introAnim.HeightBack = 8;
                introAnim.HeightFront = 8;
                skill.HitboxAction.ActionFX.Emitter = introAnim;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Magnet_Rise_Line", 1));
                emitter.ParticlesPerTile = 1.5;
                emitter.LocHeight = 8;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
            }
            else if (ii == 394)
            {
                skill.Name = new LocalText("=Flare Blitz");
                skill.Desc = new LocalText("The user cloaks itself in fire and charges the target. This also damages the user quite a lot. This may leave the target with a burn.");
                skill.BaseCharges = 8;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("burn", true, true)));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Data.AfterActions.Add(0, new HPRecoilEvent(4, false));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 5;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Flare_Blitz_Charge_Back", 2), new AnimData("Flare_Blitz_Charge_Front", 2));
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Sound = "DUN_Fissure";
                BetweenEmitter endAnim = new BetweenEmitter(new AnimData("Flare_Blitz_Hit_Back", 2), new AnimData("Flare_Blitz_Hit_Front", 2));
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 395)
            {
                skill.Name = new LocalText("=Force Palm");
                skill.Desc = new LocalText("The target is attacked with a shock wave. This may also leave the target with paralysis.");
                skill.BaseCharges = 15;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("paralyze", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 08;//Strike
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Eruption";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Force_Palm", 3));
                skill.Data.HitFX.Delay = 10;
            }
            else if (ii == 396)
            {
                skill.Name = new LocalText("Aura Sphere");
                skill.Desc = new LocalText("The user lets loose a blast of aura power from deep within its body at the target. This attack never misses.");
                skill.BaseCharges = 8;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new PulseState());
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 14;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Aura_Sphere", 2);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Aura_Sphere";
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Aura_Sphere_Shoot", 3));
                endAnim.Offset = 12;
                skill.HitboxAction.ActionFX.Emitter = endAnim;
            }
            else if (ii == 397)
            {
                skill.Name = new LocalText("Rock Polish");
                skill.Desc = new LocalText("The user polishes its body to reduce drag. This can sharply raise its Movement Speed.");
                skill.BaseCharges = 20;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, 2));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Rock_Polish", 1));
                skill.Data.HitFX.Sound = "DUN_Rock_Polish";
                skill.Data.HitFX.Delay = 45;
            }
            else if (ii == 398)
            {
                skill.Name = new LocalText("Poison Jab");
                skill.Desc = new LocalText("The target is stabbed with a tentacle or arm steeped in poison. This may also poison the target.");
                skill.BaseCharges = 16;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("poison", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 20;//Jab
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Poison_Jab";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Poison_Jab", 1));
            }
            else if (ii == 399)
            {
                skill.Name = new LocalText("Dark Pulse");
                skill.Desc = new LocalText("The user releases a horrible aura imbued with dark thoughts. This may also make the target flinch.");
                skill.BaseCharges = 11;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new PulseState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 30;
                skill.HitboxAction.ActionFX.Emitter = new BetweenEmitter(new AnimData("Dark_Pulse_Back", 2), new AnimData("Dark_Pulse_Front", 2));
                CircleSquareAreaEmitter areaEmitter = new CircleSquareAreaEmitter(new AnimData("Dark_Pulse_Particle", 3), new AnimData("Dark_Pulse_Ranger", 3));
                areaEmitter.ParticlesPerTile = 1.5;
                areaEmitter.RangeDiff = -12;
                ((AreaAction)skill.HitboxAction).Emitter = areaEmitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Radar_Orb";
            }
            else if (ii == 400)
            {
                skill.Name = new LocalText("Night Slash");
                skill.Desc = new LocalText("The user slashes the target the instant an opportunity arises. Critical hits land more easily.");
                skill.BaseCharges = 16;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 13;//Slice
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                ((DashAction)skill.HitboxAction).LagBehindTime = 12;
                skill.HitboxAction.TileEmitter = new SingleEmitter(new AnimData("Night_Slash", 1));
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Night_Slash";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Night_Slash_Hit", 1));
            }
            else if (ii == 401)
            {
                skill.Name = new LocalText("Aqua Tail");
                skill.Desc = new LocalText("The user attacks by swinging its tail as if it were a vicious wave in a raging storm.");
                skill.BaseCharges = 15;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Wide;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                SingleEmitter introAnim = new SingleEmitter(new AnimData("Aqua_Tail_Wave", 2));
                introAnim.Layer = DrawLayer.Bottom;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = introAnim;
                preFX.Sound = "DUN_Aqua_Tail_3";
                skill.HitboxAction.PreActions.Add(preFX);
                SingleEmitter tileAnim = new SingleEmitter(new AnimData("Aqua_Tail_Splash", 3));
                tileAnim.LocHeight = 8;
                skill.HitboxAction.TileEmitter = tileAnim;
                skill.HitboxAction.ActionFX.Sound = "DUN_Aqua_Tail_2";
            }
            else if (ii == 402)
            {
                skill.Name = new LocalText("Seed Bomb");
                skill.Desc = new LocalText("The user slams a barrage of hard-shelled seeds on the target from above.");
                skill.BaseCharges = 10;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHitTiles.Add(0, new RemoveItemEvent(true));
                skill.Data.OnHitTiles.Add(0, new RemoveTrapEvent());
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState))));
                skill.Strikes = 1;
                skill.Explosion.Range = 1;
                skill.Explosion.HitTiles = true;
                skill.Explosion.Speed = 10;
                skill.Explosion.ExplodeFX.Sound = "DUN_Seed_Bomb";
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Seed_Bomb_Blast", 1));
                emitter.ParticlesPerTile = 1.5;
                skill.Explosion.Emitter = emitter;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 8;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Seed_RSE", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Worry_Seed";
            }
            else if (ii == 403)
            {
                skill.Name = new LocalText("Air Slash");
                skill.Desc = new LocalText("The user attacks with a blade of air that slices even the sky. This may also make the target flinch.");
                skill.BaseCharges = 18;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(55));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((OffsetAction)skill.HitboxAction).HitArea = OffsetAction.OffsetArea.Sides;
                ((OffsetAction)skill.HitboxAction).Range = 3;
                ((OffsetAction)skill.HitboxAction).Speed = 10;
                ((OffsetAction)skill.HitboxAction).LagBehindTime = 10;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Punishment";
                skill.HitboxAction.TileEmitter = new SingleEmitter(new AnimData("Air_Slash_Slash", 2));
                FiniteReleaseRangeEmitter endAnim = new FiniteReleaseRangeEmitter(new AnimData("Air_Slash_Particle", 4, RogueElements.Dir8.Left), new AnimData("Air_Slash_Particle", 4, RogueElements.Dir8.Right));
                endAnim.BurstTime = 2;
                endAnim.ParticlesPerBurst = 2;
                endAnim.Bursts = 4;
                endAnim.StartDistance = 4;
                endAnim.Speed = 60;
                endAnim.Range = GraphicsManager.TileSize * 3 / 2;
                skill.Data.HitFX.Emitter = endAnim;
                //skill.SkillEffect.EndSound = 453;
            }
            else if (ii == 404)
            {
                skill.Name = new LocalText("=X-Scissor");
                skill.Desc = new LocalText("The user slashes at the target by crossing its scythes or claws as if they were a pair of scissors.");
                skill.BaseCharges = 18;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(13);//Slice
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("X_Scissor", 2));
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Trawl_Orb";
                skill.Data.HitFX.Delay = 10;
            }
            else if (ii == 405)
            {
                skill.Name = new LocalText("Bug Buzz");
                skill.Desc = new LocalText("The user vibrates its wings to generate a damaging sound wave. This may also lower the target's Sp. Def stat.");
                skill.BaseCharges = 15;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_special_defense", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(35);//Emit
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 6;
                ((AreaAction)skill.HitboxAction).ActionFX.Delay = 12;
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Bug_Buzz", 2));
                CircleSquareAreaEmitter areaEmitter = new CircleSquareAreaEmitter(new AnimData("Circle_Thick_Red_In", 3, 3, -1));
                areaEmitter.ParticlesPerTile = 1.2;
                ((AreaAction)skill.HitboxAction).Emitter = areaEmitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Bug_Buzz";
            }
            else if (ii == 406)
            {
                skill.Name = new LocalText("=Dragon Pulse");
                skill.Desc = new LocalText("The target is attacked with a shock wave generated by the user's gaping mouth.");
                skill.BaseCharges = 8;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new PulseState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Dragon_Pulse_Charge", 2);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Dragon_Pulse";
                FiniteReleaseEmitter endEmit = new FiniteReleaseEmitter(new AnimData("Dragon_Pulse_Particle_Purple", 3));
                endEmit.BurstTime = 2;
                endEmit.ParticlesPerBurst = 3;
                endEmit.Speed = 128;
                endEmit.Bursts = 4;
                endEmit.StartDistance = 4;
                skill.Data.HitFX.Emitter = endEmit;
            }
            else if (ii == 407)
            {
                skill.Name = new LocalText("=Dragon Rush");
                skill.Desc = new LocalText("The user tackles the target while exhibiting overwhelming menace. This may also make the target flinch.");
                skill.BaseCharges = 12;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 70;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 4;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Rock_Slide";
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Dragon_Rush_Back", 2), new AnimData("Dragon_Rush_Front", 2));
                skill.Data.HitFX.Emitter = emitter;
                skill.Data.HitFX.Delay = 10;
            }
            else if (ii == 408)
            {
                skill.Name = new LocalText("Power Gem");
                skill.Desc = new LocalText("The user attacks with rays of light that sparkle as if they were made of gemstones.");
                skill.BaseCharges = 14;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 3;
                ((ProjectileAction)skill.HitboxAction).Speed = 8;
                ((ProjectileAction)skill.HitboxAction).Rays = ProjectileAction.RayCount.Three;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Power_Gem_Charge", 2));
                preFX.Sound = "DUN_Power_Gem";
                skill.HitboxAction.PreActions.Add(preFX);
                AttachAreaEmitter emitter = new AttachAreaEmitter(new AnimData("Power_Gem_Shot", 3));
                emitter.BurstTime = 3;
                emitter.ParticlesPerBurst = 1;
                ((ProjectileAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Power_Gem_2";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Power_Gem_Hit", 2));
            }
            else if (ii == 409)
            {
                skill.Name = new LocalText("Drain Punch");
                skill.Desc = new LocalText("An energy-draining punch. The user's HP is restored by half the damage taken by the target.");
                skill.BaseCharges = 15;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new FistState());
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new HPDrainEvent(2));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(11);//Punch
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Punch";
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Print_Fist", 12));
                FiniteGatherEmitter endAnim = new FiniteGatherEmitter(new AnimData("Absorb", 3));
                endAnim.ParticlesPerBurst = 1;
                endAnim.Bursts = 1;
                endAnim.TravelTime = 30;
                endAnim.UseDest = true;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Absorb";
            }
            else if (ii == 410)
            {
                skill.Name = new LocalText("=Vacuum Wave");
                skill.Desc = new LocalText("The user whirls its fists to send a wave of pure vacuum at the target.");
                skill.BaseCharges = 16;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(11);//Punch
                ((ProjectileAction)skill.HitboxAction).Range = 3;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Sucker_Punch";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Vacuum_Wave", 1));
            }
            else if (ii == 411)
            {
                skill.Name = new LocalText("Focus Blast");
                skill.Desc = new LocalText("The user heightens its mental focus and unleashes its power. This may also lower the target's Sp. Def.");
                skill.BaseCharges = 9;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 75;
                skill.Data.SkillStates.Set(new BasePowerState(85));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_special_defense", true, true, -1)));
                skill.Data.OnHitTiles.Add(0, new RemoveItemEvent(true));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState))));
                skill.Strikes = 1;
                skill.Explosion.Range = 1;
                skill.Explosion.HitTiles = true;
                skill.Explosion.Speed = 10;
                skill.Explosion.ExplodeFX.Sound = "DUN_Self-Destruct";
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.ExplodeFX.Emitter = new SingleEmitter(new AnimData("Focus_Blast_Hit", 2));
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 9;
                ((ProjectileAction)skill.HitboxAction).Speed = 12;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Focus_Blast_Ball", 2);
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Focus_Blast_3";
            }
            else if (ii == 412)
            {
                skill.Name = new LocalText("Energy Ball");
                skill.Desc = new LocalText("The user draws power from nature and fires it at the target. This may also lower the target's Sp. Def.");
                skill.BaseCharges = 8;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_special_defense", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 8;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Energy_Ball", 3);
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Energy_Ball";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Energy_Ball_Hit", 3));
                skill.Data.HitFX.Sound = "DUN_Energy_Ball_3";
            }
            else if (ii == 413)
            {
                skill.Name = new LocalText("=Brave Bird");
                skill.Desc = new LocalText("The user tucks in its wings and charges from a low altitude. This also damages the user quite a lot.");
                skill.BaseCharges = 8;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new HPRecoilEvent(4, false));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 5;
                ((DashAction)skill.HitboxAction).AppearanceMod = DashAction.DashAppearance.Invisible;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                ((DashAction)skill.HitboxAction).Anim = new AnimData("Brave_Bird", 3);
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Brave_Bird", 3));
            }
            else if (ii == 414)
            {
                skill.Name = new LocalText("Earth Power");
                skill.Desc = new LocalText("The user makes the ground under the target erupt with power. This may also lower the target's Sp. Def.");
                skill.BaseCharges = 8;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(65));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_special_defense", true, true, -1)));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WaterTerrainState)), new FlagType(typeof(LavaTerrainState)), new FlagType(typeof(AbyssTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new WaveMotionAction();
                ((WaveMotionAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(31);//Rumble
                ((WaveMotionAction)skill.HitboxAction).Range = 5;
                ((WaveMotionAction)skill.HitboxAction).Speed = 10;
                ((WaveMotionAction)skill.HitboxAction).Linger = 6;
                ((WaveMotionAction)skill.HitboxAction).Wide = true;
                ((WaveMotionAction)skill.HitboxAction).StopAtWall = true;
                ((WaveMotionAction)skill.HitboxAction).HitTiles = true;
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Earth_Power_Back", 1), new AnimData("Earth_Power_Front", 1));
                emitter.HeightBack = 24;
                emitter.HeightFront = 24;
                skill.HitboxAction.TileEmitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Earth_Power";
            }
            else if (ii == 415)
            {
                skill.Name = new LocalText("-Switcheroo");
                skill.Desc = new LocalText("The user trades items with the target faster than the eye can follow.");
                skill.BaseCharges = 20;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new SwitchHeldItemEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2));
                skill.Data.HitFX.Sound = "DUN_Switcher";
            }
            else if (ii == 416)
            {
                skill.Name = new LocalText("Giga Impact");
                skill.Desc = new LocalText("The user charges at the target using every bit of its power. The user can't move for the next three turns.");
                skill.BaseCharges = 8;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(130));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(4));
                skill.Data.AfterActions.Add(0, new StatusStateBattleEvent("paused", false, true, statusStates));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState))));
                skill.Strikes = 1;
                skill.Explosion.Range = 1;
                skill.Explosion.HitTiles = true;
                skill.Explosion.Speed = 10;
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Giga_Impact_Back", 2), new AnimData("Giga_Impact_Front", 2));
                skill.Explosion.ExplodeFX.Emitter = emitter;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Giga_Impact";
            }
            else if (ii == 417)
            {
                skill.Name = new LocalText("Nasty Plot");
                skill.Desc = new LocalText("The user stimulates its brain by thinking bad thoughts. This sharply raises the user's Sp. Atk.");
                skill.BaseCharges = 12;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, 2));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Nasty_Plot", 3));
                emitter.LocHeight = 20;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Nasty_Plot";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.Data.HitFX.Delay = 65;
            }
            else if (ii == 418)
            {
                skill.Name = new LocalText("Bullet Punch");
                skill.Desc = new LocalText("The user strikes the target with tough punches as fast as bullets.");
                skill.BaseCharges = 18;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new FistState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 11;//Punch
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Move_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Bullet_Punch", 2));
                skill.Data.HitFX.Sound = "DUN_Bullet_Punch";
                skill.Data.HitFX.Delay = 10;
            }
            else if (ii == 419)
            {
                skill.Name = new LocalText("Avalanche");
                skill.Desc = new LocalText("An attack move that inflicts double the damage if the user was attacked in its previous turn.");
                skill.BaseCharges = 18;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnActions.Add(0, new StatusPowerEvent("was_hurt_last_turn", false));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(31);//Rumble
                ((OffsetAction)skill.HitboxAction).HitArea = OffsetAction.OffsetArea.Sides;
                ((OffsetAction)skill.HitboxAction).Range = 1;
                ((OffsetAction)skill.HitboxAction).LagBehindTime = 30;
                ((OffsetAction)skill.HitboxAction).Speed = 10;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Rock_Slide";
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Avalanche_Back", 2), new AnimData("Avalanche_Front", 2));
                emitter.HeightBack = 80;
                emitter.HeightFront = 80;
                skill.HitboxAction.TileEmitter = emitter;
                //SingleEmitter endAnim = new SingleEmitter();
                //endAnim.Anim = new AnimData("Avalanche_Hit", 2);
                //skill.SkillEffect.Emitter = endAnim;
            }
            else if (ii == 420)
            {
                skill.Name = new LocalText("Ice Shard");
                skill.Desc = new LocalText("The user flash-freezes chunks of ice and hurls them at the target.");
                skill.BaseCharges = 18;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 3;
                ((ProjectileAction)skill.HitboxAction).Speed = 8;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Ice_Shard", 4);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Ice_Shard";
                skill.Data.HitFX.Sound = "DUN_Ice_Shard_2";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Ice_Shard_Hit", 1));
            }
            else if (ii == 421)
            {
                skill.Name = new LocalText("Shadow Claw");
                skill.Desc = new LocalText("The user slashes with a sharp claw made from shadows. Critical hits land more easily.");
                skill.BaseCharges = 18;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(10);//Scratch
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.FrontAndCorners;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Shadow_Claw", 2));
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Knock_Off";
                skill.Data.HitFX.Delay = 10;
            }
            else if (ii == 422)
            {
                skill.Name = new LocalText("Thunder Fang");
                skill.Desc = new LocalText("The user bites with electrified fangs. This may also make the target flinch or leave it with paralysis.");
                skill.BaseCharges = 18;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new JawState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(65));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("paralyze", true, true), new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(18);//Bite
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Thunder_Fang";
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Thunder_Fang_Fang", 2));
                ((AttackAction)skill.HitboxAction).LagBehindTime = 10;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Spark", 3));
            }
            else if (ii == 423)
            {
                skill.Name = new LocalText("Ice Fang");
                skill.Desc = new LocalText("The user bites with cold-infused fangs. This may also make the target flinch or leave it frozen.");
                skill.BaseCharges = 18;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new JawState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(65));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("freeze", true, true), new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(18);//Bite
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Ice_Fang";
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Ice_Fang_Fang", 2));
                ((AttackAction)skill.HitboxAction).LagBehindTime = 10;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Ice_Fang_Hit", 3));
            }
            else if (ii == 424)
            {
                skill.Name = new LocalText("=Fire Fang");
                skill.Desc = new LocalText("The user bites with flame-cloaked fangs. This may also make the target flinch or leave it with a burn.");
                skill.BaseCharges = 18;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new JawState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(65));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("burn", true, true), new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(18);//Bite
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Fire_Fang_Fang", 2));
                ((AttackAction)skill.HitboxAction).LagBehindTime = 10;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Fire_Fang_Hit", 3));
            }
            else if (ii == 425)
            {
                skill.Name = new LocalText("Shadow Sneak");
                skill.Desc = new LocalText("The user extends its shadow and attacks the target from behind.");
                skill.BaseCharges = 16;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).AppearanceMod = DashAction.DashAppearance.Invisible;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                AttachAreaEmitter emitter = new AttachAreaEmitter(new AnimData("Shadow_Sneak_Shadow", 10));
                emitter.BurstTime = 1;
                emitter.ParticlesPerBurst = 1;
                emitter.AddHeight = -6;
                emitter.Layer = DrawLayer.Bottom;
                ((DashAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Shadow_Sneak";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Shadow_Sneak_Hit", 2));
                skill.Data.HitFX.Sound = "DUN_Snadow_Sneak_2";
            }
            else if (ii == 426)
            {
                skill.Name = new LocalText("Mud Bomb");
                skill.Desc = new LocalText("The user launches a hard-packed mud ball to attack. This may also lower the target's Accuracy.");
                skill.BaseCharges = 11;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(45));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_accuracy", true, true, -1)));
                skill.Strikes = 1;
                skill.Explosion.Range = 1;
                skill.Explosion.HitTiles = true;
                skill.Explosion.Speed = 10;
                skill.Explosion.ExplodeFX.Sound = "DUN_Mud_Bomb";
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                SingleEmitter emitter = new SingleEmitter(new AnimData("Mud_Bomb_Hit", 1));
                emitter.LocHeight = 4;
                skill.Explosion.ExplodeFX.Emitter = emitter;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Mud_Bomb_Ball", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Throw_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Sound = "DUN_Throw_Arc";
            }
            else if (ii == 427)
            {
                skill.Name = new LocalText("-Psycho Cut");
                skill.Desc = new LocalText("The user tears at the target with blades formed by psychic power. Critical hits land more easily.");
                skill.BaseCharges = 16;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(13);//Slice
                ((OffsetAction)skill.HitboxAction).HitArea = OffsetAction.OffsetArea.Sides;
                ((OffsetAction)skill.HitboxAction).Range = 2;
                ((OffsetAction)skill.HitboxAction).Speed = 10;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.TileEmitter = new SingleEmitter(new AnimData("Air_Slash_Slash", 2));
                skill.HitboxAction.ActionFX.Sound = "DUN_Safeguard";
            }
            else if (ii == 428)
            {
                skill.Name = new LocalText("Zen Headbutt");
                skill.Desc = new LocalText("The user focuses its willpower to its head and attacks the target. This may also make the target flinch.");
                skill.BaseCharges = 14;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Zen_Headbutt", 1));
                emitter.LocHeight = 12;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Zen_Headbutt";
                preFX.Delay = 10;
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Sound = "DUN_Zen_Headbutt_2";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Zen_Headbutt_Hit", 1));
            }
            else if (ii == 429)
            {
                skill.Name = new LocalText("Mirror Shot");
                skill.Desc = new LocalText("The user lets loose a flash of energy at the target from its polished body. This may also lower the target's Accuracy.");
                skill.BaseCharges = 16;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_accuracy", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 2;
                ((ProjectileAction)skill.HitboxAction).Speed = 8;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                FiniteReleaseRangeEmitter introEmit = new FiniteReleaseRangeEmitter(new AnimData("Mirror_Shot_Particle", 1, 0, 0), new AnimData("Mirror_Shot_Particle", 1, 1, 1), new AnimData("Mirror_Shot_Particle", 1, 2, 2), new AnimData("Mirror_Shot_Particle", 1, 3, 3));
                introEmit.BurstTime = 2;
                introEmit.ParticlesPerBurst = 2;
                introEmit.Range = GraphicsManager.TileSize * 4 / 3;
                introEmit.Speed = 128;
                introEmit.Bursts = 8;
                introEmit.StartDistance = 4;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = introEmit;
                preFX.Sound = "DUN_Mirror_Shot";
                skill.HitboxAction.PreActions.Add(preFX);
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Mirror_Shot_Glow", 2);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Sound = "DUN_Mirror_Shot_2";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Mirror_Shot_Hit", 2));
            }
            else if (ii == 430)
            {
                skill.Name = new LocalText("Flash Cannon");
                skill.Desc = new LocalText("The user gathers all its light energy and releases it at once. This may also lower the target's Sp. Def stat.");
                skill.BaseCharges = 8;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_special_defense", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Flash_Cannon", 2);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Flash_Cannon";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Flash_Cannon_Release", 2));
                skill.Data.HitFX.Sound = "DUN_Flash_Cannon_2";
            }
            else if (ii == 431)
            {
                skill.Name = new LocalText("-Rock Climb");
                skill.Desc = new LocalText("The user attacks the target by smashing into it with incredible force. This may also confuse the target.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 75;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("confuse", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 5;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Rock_Smash";
            }
            else if (ii == 432)
            {
                skill.Name = new LocalText("Defog");
                skill.Desc = new LocalText("A strong wind blows away traps, barriers, and map conditions.");
                skill.BaseCharges = 16;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new RemoveStatusBattleEvent("reflect", true));
                skill.Data.OnHits.Add(0, new RemoveStatusBattleEvent("light_screen", true));
                skill.Data.OnHits.Add(0, new RemoveStatusBattleEvent("mist", true));
                skill.Data.AfterActions.Add(0, new GiveMapStatusEvent("clear", 0, new StringKey(), typeof(ExtendWeatherState)));
                skill.Data.OnHitTiles.Add(0, new RemoveTrapEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(32);//FlapAround
                ((AreaAction)skill.HitboxAction).Range = 5;
                ((AreaAction)skill.HitboxAction).Speed = 8;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.ActionFX.Sound = "DUN_Defog";
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Gust_Wind", 2));
                emitter.ParticlesPerTile = 0.8;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 433)
            {
                skill.Name = new LocalText("-Trick Room");
                skill.Desc = new LocalText("The user creates a bizarre area in which slowed Pokémon move faster.");
                skill.BaseCharges = 16;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new GiveMapStatusEvent("trick_room"));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Move_Start";
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 434)
            {
                skill.Name = new LocalText("Draco Meteor");
                skill.Desc = new LocalText("Comets are summoned down from the sky onto the target. The attack's recoil harshly lowers the user's Special Attack stat.");
                skill.BaseCharges = 5;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new StatusStackBattleEvent("mod_special_attack", false, true, -2));
                skill.Data.OnHitTiles.Add(0, new RemoveItemEvent(true));
                skill.Data.OnHitTiles.Add(0, new RemoveTrapEvent());
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState))));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                SingleEmitter preEmitter = new SingleEmitter(new AnimData("Draco_Meteor", 2));
                preEmitter.LocHeight = 96;
                BattleFX fx1 = new BattleFX(preEmitter, "", 12);
                skill.Explosion.IntroFX.Add(fx1);
                BattleFX fx2 = new BattleFX(new EmptyFiniteEmitter(), "DUN_Draco_Meteor", 22);
                skill.Explosion.IntroFX.Add(fx2);
                skill.Explosion.Range = 1;
                skill.Explosion.HitTiles = true;
                skill.Explosion.Speed = 6;
                //skill.Explosion.HitTiles = true;
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Draco_Meteor_Explosion", 2));
                emitter.ParticlesPerTile = 0.8;
                emitter.RangeDiff = -12;
                skill.Explosion.Emitter = emitter;
                skill.HitboxAction = new AreaAction();
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                preFX.Sound = "DUN_Move_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                SingleEmitter preEmitter2 = new SingleEmitter(new AnimData("Draco_Meteor_Streaks", 2));
                preEmitter2.LocHeight = 96;
                skill.HitboxAction.PreActions.Add(new BattleFX(preEmitter2, "", 0));
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(31);//Rumble
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 435)
            {
                skill.Name = new LocalText("Discharge");
                skill.Desc = new LocalText("The user strikes everything around it by letting loose a flare of electricity. This may also cause paralysis.");
                skill.BaseCharges = 14;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("paralyze", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(34);//Shock
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Discharge";
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Discharge", 2));
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Discharge_Hit", 3));
                skill.Data.HitFX.Sound = "DUN_Discharge_2";
            }
            else if (ii == 436)
            {
                skill.Name = new LocalText("=Lava Plume");
                skill.Desc = new LocalText("The user torches everything around it with an inferno of scarlet flames. This may also leave those hit with a burn.");
                skill.BaseCharges = 14;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("burn", true, true)));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(35);//Emit
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 8;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Lava_Plume_Smoke", 2));
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter();
                for (int nn = 0; nn < 9; nn++)
                    emitter.Anims.Add(new ParticleAnim(new AnimData("Lava_Plume_Fire", 1, nn, nn)));
                emitter.BurstTime = 3;
                emitter.ParticlesPerBurst = 3;
                emitter.Bursts = 6;
                emitter.StartDistance = 4;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
            }
            else if (ii == 437)
            {
                skill.Name = new LocalText("-Leaf Storm");
                skill.Desc = new LocalText("The user whips up a storm of leaves around the target. The attack's recoil harshly lowers the user's Sp. Atk stat.");
                skill.BaseCharges = 6;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new StatusStackBattleEvent("mod_special_attack", false, true, -2));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 5;
                ((ProjectileAction)skill.HitboxAction).Speed = 16;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Leaf_Storm_Leaf", 1, 0, 0), new AnimData("Leaf_Storm_Leaf", 1, 1, 1), new AnimData("Leaf_Storm_Leaf", 1, 2, 2));
                shotAnim.StartDistance = 8;
                shotAnim.Shots = 16;
                shotAnim.BurstTime = 4;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("Leaf_Storm", 3, -1, -1, 128);
                overlay.TotalTime = 60;
                overlay.Movement = new Loc(10, 0);
                overlay.Layer = DrawLayer.Bottom;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = overlay;
                preFX.Sound = "DUN_Leaf_Storm_3";
                skill.HitboxAction.PreActions.Add(preFX);
                //also needs anim 1170
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Leaf_Storm_Hit", 2));
                skill.Data.HitFX.Sound = "DUN_Leaf_Storm_2";
            }
            else if (ii == 438)
            {
                skill.Name = new LocalText("Power Whip");
                skill.Desc = new LocalText("The user violently whirls its vines or tentacles to harshly lash the target.");
                skill.BaseCharges = 16;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 75;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((ProjectileAction)skill.HitboxAction).Range = 2;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                AttachAreaEmitter emitter = new AttachAreaEmitter(new AnimData("Power_Whip_Whip", 2));
                emitter.BurstTime = 2;
                emitter.ParticlesPerBurst = 1;
                emitter.AddHeight = 8;
                emitter.Range = GraphicsManager.TileSize / 2;
                ((ProjectileAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Power_Whip";
                BetweenEmitter endAnim = new BetweenEmitter(new AnimData("Power_Whip_Hit_Back", 3), new AnimData("Power_Whip_Hit_Front", 3));
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 439)
            {
                skill.Name = new LocalText("Rock Wrecker");
                skill.Desc = new LocalText("The user launches a huge boulder at the target to attack. The user can't move on the next turn.");
                skill.BaseCharges = 8;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new StatusBattleEvent("paused", false, true));
                skill.Data.OnHitTiles.Add(0, new RemoveItemEvent(true));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState))));
                skill.Strikes = 1;
                skill.Explosion.Range = 1;
                skill.Explosion.HitTiles = true;
                skill.Explosion.Speed = 10;
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.ExplodeFX.Sound = "DUN_Giga_Impact";
                skill.Explosion.ExplodeFX.ScreenMovement = new ScreenMover(0, 8, 30);
                skill.Explosion.ExplodeFX.Emitter = new SingleEmitter(new AnimData("Rock_Wrecker_Hit", 2));
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 12;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Rock_Wrecker_Boulder", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Rock_Wrecker";
            }
            else if (ii == 440)
            {
                skill.Name = new LocalText("Cross Poison");
                skill.Desc = new LocalText("A slashing attack with a poisonous blade that may also poison the target. Critical hits land more easily.");
                skill.BaseCharges = 16;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("poison", true, true)));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(13);//Slice
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Cross_Poison", 2));
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Cross_Poison";
                skill.Data.HitFX.Delay = 10;
            }
            else if (ii == 441)
            {
                skill.Name = new LocalText("Gunk Shot");
                skill.Desc = new LocalText("The user shoots filthy garbage at the target to attack.");
                skill.BaseCharges = 11;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 75;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Gunk_Shot", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.HitboxAction.ActionFX.Sound = "DUN_Gunk_Shot";
                StaticAreaEmitter emitter = new StaticAreaEmitter(new AnimData("Gunk_Shot_Hit", 1));
                emitter.Range = 14;
                emitter.ParticlesPerBurst = 1;
                emitter.BurstTime = 6;
                emitter.Bursts = 3;
                skill.Data.HitFX.Emitter = emitter;
                skill.Data.HitFX.Sound = "DUN_Gunk_Shot_2";
                skill.Data.HitFX.Delay = 20;
            }
            else if (ii == 442)
            {
                skill.Name = new LocalText("=Iron Head");
                skill.Desc = new LocalText("The user slams the target with its steel-hard head. This may also make the target flinch.");
                skill.BaseCharges = 15;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Head_Smash";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Iron_Head", 2));
            }
            else if (ii == 443)
            {
                skill.Name = new LocalText("-Magnet Bomb");
                skill.Desc = new LocalText("The user launches steel bombs that stick to the target. This attack never misses.");
                skill.BaseCharges = 10;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHitTiles.Add(0, new RemoveItemEvent(true));
                skill.Data.OnHitTiles.Add(0, new RemoveTrapEvent());
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState))));
                skill.Strikes = 1;
                skill.Explosion.Range = 1;
                skill.Explosion.HitTiles = true;
                skill.Explosion.Speed = 10;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Blast_Seed", 3));
                emitter.ParticlesPerTile = 0.8;
                skill.Explosion.Emitter = emitter;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Metal_Ball_Ranger", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Magnet_Bomb";
            }
            else if (ii == 444)
            {
                skill.Name = new LocalText("Stone Edge");
                skill.Desc = new LocalText("The user stabs the target with sharpened stones from below. Critical hits land more easily.");
                skill.BaseCharges = 16;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(31);//Rumble
                ((OffsetAction)skill.HitboxAction).Range = 2;
                ((OffsetAction)skill.HitboxAction).Speed = 10;
                ((OffsetAction)skill.HitboxAction).LagBehindTime = 30;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Stone_Edge";
                SingleEmitter emitter = new SingleEmitter(new AnimData("Stone_Edge", 2));
                emitter.LocHeight = 96;
                skill.HitboxAction.TileEmitter = emitter;
            }
            else if (ii == 445)
            {
                skill.Name = new LocalText("Captivate");
                skill.Desc = new LocalText("The opposing Pokémon is charmed, which harshly lowers its Sp. Atk stat.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, -2));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 2;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Captivate_Heart", 1, 2, 2);
                SingleEmitter emitter = new SingleEmitter(new AnimData("Captivate", 3));
                emitter.LocHeight = 8;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Captivate";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Delay = 20;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 446)
            {
                skill.Name = new LocalText("=Stealth Rock");
                skill.Desc = new LocalText("The user lays a trap of levitating stones around the target. The trap hurts opposing Pokémon that walk into it with a Rock-type attack.");
                skill.BaseCharges = 20;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHitTiles.Add(0, new SetTrapEvent("trap_stealth_rock"));
                skill.Strikes = 1;
                skill.Explosion.Range = 1;
                skill.Explosion.HitTiles = true;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Stone_Edge_Rock", 1, 3, 3);
                skill.HitboxAction.ActionFX.Sound = "DUN_Super_Fang";
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Stealth_Rock_Back", 3), new AnimData("Stealth_Rock_Front", 3));
                skill.HitboxAction.TileEmitter = emitter;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 447)
            {
                skill.Name = new LocalText("Grass Knot");
                skill.Desc = new LocalText("The user snares the target with grass and trips it. The heavier the target, the greater the move's power.");
                skill.BaseCharges = 20;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState());
                skill.Data.BeforeHits.Add(0, new WeightBasePowerEvent());
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new DropItemEvent(false, true, "", new HashSet<FlagType>(), new StringKey(), false)));
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((OffsetAction)skill.HitboxAction).Range = 2;
                ((OffsetAction)skill.HitboxAction).Speed = 10;
                ((OffsetAction)skill.HitboxAction).LagBehindTime = 12;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Grass_Knot_Back", 1), new AnimData("Grass_Knot_Front", 1));
                emitter.HeightBack = 8;
                emitter.HeightFront = 8;
                skill.HitboxAction.TileEmitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Grass_Knot";
            }
            else if (ii == 448)
            {
                skill.Name = new LocalText("Chatter");
                skill.Desc = new LocalText("The user attacks the target with sound waves of deafening chatter. This confuses the target.");
                skill.BaseCharges = 14;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(55));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("confuse", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 8;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Chatter", 2));
                emitter.ParticlesPerTile = 0.25;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Sing";
            }
            else if (ii == 449)
            {
                skill.Name = new LocalText("=Judgment");
                skill.Desc = new LocalText("The user draws from the power of its plates and releases countless shots of light at the target.");
                skill.BaseCharges = 5;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(20));
                Dictionary<string, string> plate = new Dictionary<string, string>();
                {
                    plate["held_insect_plate"] = "bug";
                    plate["held_dread_plate"] = "dark";
                    plate["held_draco_plate"] = "dragon";
                    plate["held_zap_plate"] = "electric";
                    plate["held_pixie_plate"] = "fairy";
                    plate["held_fist_plate"] = "fighting";
                    plate["held_flame_plate"] = "fire";
                    plate["held_sky_plate"] = "flying";
                    plate["held_spooky_plate"] = "ghost";
                    plate["held_meadow_plate"] = "grass";
                    plate["held_earth_plate"] = "ground";
                    plate["held_icicle_plate"] = "ice";
                    plate["held_blank_plate"] = "normal";
                    plate["held_toxic_plate"] = "poison";
                    plate["held_mind_plate"] = "psychic";
                    plate["held_stone_plate"] = "rock";
                    plate["held_iron_plate"] = "steel";
                    plate["held_splash_plate"] = "water";
                }
                skill.Data.BeforeActions.Add(-1, new PrepareJudgmentEvent(plate));
                skill.Data.OnActions.Add(-1, new PassJudgmentEvent());
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 15;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter();
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Blue", 3, -1, -1, 192)));
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Green", 3, -1, -1, 192)));
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Purple", 3, -1, -1, 192)));
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Red", 3, -1, -1, 192)));
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Yellow", 3, -1, -1, 192)));
                emitter.ParticlesPerTile = 0.7;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Aurora_Beam";
            }
            else if (ii == 450)
            {
                skill.Name = new LocalText("Bug Bite");
                skill.Desc = new LocalText("The user bites the target. If the target is holding a Berry, the user eats it and gains its effect.");
                skill.BaseCharges = 18;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                HashSet<FlagType> eligibles = new HashSet<FlagType>();
                eligibles.Add(new FlagType(typeof(EdibleState)));
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new UseFoeItemEvent(false, false, "seed_decoy", eligibles, true, false)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(18);//Bite
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Attack";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Bug_Bite", 1));
                skill.Data.HitFX.Sound = "DUN_Bug_Bite";
            }
            else if (ii == 451)
            {
                skill.Name = new LocalText("Charge Beam");
                skill.Desc = new LocalText("The user attacks with an electric charge. The user may use any remaining electricity to raise its Sp. Atk stat.");
                skill.BaseCharges = 15;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.SkillStates.Set(new AdditionalEffectState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new AdditionalEndEvent(new StatusStackBattleEvent("mod_special_attack", false, true, 1)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Charge_Beam_Shot", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Charge_Beam";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Charge_Beam_Hit", 2));
                skill.Data.HitFX.Sound = "DUN_Lava_Plume";
            }
            else if (ii == 452)
            {
                skill.Name = new LocalText("Wood Hammer");
                skill.Desc = new LocalText("The user slams its rugged body into the target to attack. This also damages the user quite a lot.");
                skill.BaseCharges = 12;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new HPRecoilEvent(4, false));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 23;//Slam
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Avalanche";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Wood_Hammer_Sticks", 3));
                skill.Data.HitFX.Sound = "DUN_Wood_Hammer";
            }
            else if (ii == 453)
            {
                skill.Name = new LocalText("Aqua Jet");
                skill.Desc = new LocalText("The user lunges at the target at a speed that makes it almost invisible.");
                skill.BaseCharges = 18;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Aqua_Tail";
                SingleEmitter tileAnim = new SingleEmitter(new AnimData("Aqua_Tail_Splash", 3));
                tileAnim.LocHeight = 8;
                skill.HitboxAction.TileEmitter = tileAnim;
            }
            else if (ii == 454)
            {
                skill.Name = new LocalText("=Attack Order");
                skill.Desc = new LocalText("The user calls out its underlings to pummel the target. Critical hits land more easily.");
                skill.BaseCharges = 10;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Attack_Order_Bee", 1, 0, 0), new AnimData("Attack_Order_Bee", 1, 1, 1), new AnimData("Attack_Order_Bee", 1, 2, 2), new AnimData("Attack_Order_Bee", 1, 3, 3));
                emitter.BurstTime = 2;
                emitter.ParticlesPerBurst = 2;
                emitter.Bursts = 36;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Delay = 35;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Attack_Order", 2));
            }
            else if (ii == 455)
            {
                skill.Name = new LocalText("=Defend Order");
                skill.Desc = new LocalText("The user calls out its underlings to shield the party, raising their Defense and Sp. Def stats.");
                skill.BaseCharges = 16;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, 1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_defense", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Sound = "DUN_Bug_Buzz";
                skill.Data.HitFX.Delay = 35;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Defend_Order", 2));
            }
            else if (ii == 456)
            {
                skill.Name = new LocalText("=Heal Order");
                skill.Desc = new LocalText("The user calls out its underlings to heal the party. Targets regain up to half of their max HP.");
                skill.BaseCharges = 10;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new RestoreHPEvent(1, 2, true));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Sound = "DUN_Heal_Block_2";
                skill.Data.HitFX.Delay = 35;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Heal_Order", 2));
            }
            else if (ii == 457)
            {
                skill.Name = new LocalText("Head Smash");
                skill.Desc = new LocalText("The user attacks the target with a hazardous, full-power headbutt. This also damages the user terribly.");
                skill.BaseCharges = 10;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 75;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new HPRecoilEvent(3, false));
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Wall_Break", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Rollout", terrainEmitter, new FlagType(typeof(WallTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 05;//Attack
                ((DashAction)skill.HitboxAction).Range = 5;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                skill.HitboxAction.ActionFX.Sound = "DUN_Close_Combat";
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.HitTiles = true;
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Head_Smash_Back", 2), new AnimData("Head_Smash_Front", 2));
                skill.Data.HitFX.Emitter = emitter;
                skill.Data.HitFX.Sound = "DUN_Head_Smash";
                skill.Data.HitFX.Delay = 6;
            }
            else if (ii == 458)
            {
                skill.Name = new LocalText("=Double Hit");
                skill.Desc = new LocalText("The user slams the target with a long tail, vines, or a tentacle. The target is hit twice in a row.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 2;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(08);//Strike
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Double_Hit", 2));
                endAnim.LocHeight = 8;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 459)
            {
                skill.Name = new LocalText("-Roar of Time");
                skill.Desc = new LocalText("The user blasts the target with power that distorts even time. The user and targets can't move on the next turn.");
                skill.BaseCharges = 4;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new StatusBattleEvent("paused", true, true)));
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(4));
                skill.Data.AfterActions.Add(0, new StatusStateBattleEvent("paused", false, true, statusStates));
                skill.Strikes = 1;
                skill.HitboxAction = new WaveMotionAction();
                ((WaveMotionAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(30);//Sound
                ((WaveMotionAction)skill.HitboxAction).Range = 8;
                ((WaveMotionAction)skill.HitboxAction).Speed = 10;
                ((WaveMotionAction)skill.HitboxAction).Linger = 6;
                ((WaveMotionAction)skill.HitboxAction).Wide = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 460)
            {
                skill.Name = new LocalText("-Spacial Rend");
                skill.Desc = new LocalText("The user tears the target along with the space around it. Critical hits land more easily.");
                skill.BaseCharges = 5;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(10);//Scratch
                ((AreaAction)skill.HitboxAction).Range = 5;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 461)
            {
                skill.Name = new LocalText("=Lunar Dance");
                skill.Desc = new LocalText("The user's HP drops to 1. In return, the party Pokémon will have their status and HP fully restored.");
                skill.BaseCharges = 14;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = -1;
                skill.Data.BeforeActions.Add(-1, new AddContextStateEvent(new CureAttack()));
                skill.Data.OnHits.Add(0, new RestoreHPEvent(1, 1, true));
                skill.Data.OnHits.Add(0, new RemoveStateStatusBattleEvent(typeof(BadStatusState), true, new StringKey("MSG_CURE_ALL")));
                skill.Data.AfterActions.Add(0, new HPTo1Event(false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(26);//Dance
                ((AreaAction)skill.HitboxAction).Range = 5;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 10;
                skill.HitboxAction.TargetAlignments = Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Friend;
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("Lunar_Dance", 3);
                overlay.TotalTime = 60;
                overlay.Layer = DrawLayer.Bottom;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = overlay;
                skill.HitboxAction.PreActions.Add(preFX);
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Lunar_Dance_Back", 1), new AnimData("Lunar_Dance_Front", 1));
                emitter.HeightBack = 24;
                emitter.HeightFront = 24;
                skill.HitboxAction.ActionFX.Emitter = emitter;
            }
            else if (ii == 462)
            {
                skill.Name = new LocalText("=Crush Grip");
                skill.Desc = new LocalText("The target is crushed with great force. The more HP the target has left, the greater this move's power.");
                skill.BaseCharges = 8;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState());
                skill.Data.BeforeHits.Add(0, new HPBasePowerEvent(150, false, true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Crush_Grip_Hand", 2));
                ((AttackAction)skill.HitboxAction).LagBehindTime = 40;
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Crush_Grip_Hit", 2));
                endAnim.LocHeight = 12;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 463)
            {
                skill.Name = new LocalText("Magma Storm");
                skill.Desc = new LocalText("The target becomes trapped within a maelstrom of fire that rages for three turns.");
                skill.BaseCharges = 5;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 75;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new StatusBattleEvent("magma_storm", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(34);//Shock
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                ((ThrowAction)skill.HitboxAction).LagBehindTime = 30;
                skill.HitboxAction.ActionFX.Sound = "DUN_Magma_Storm";
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Move_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Magma_Storm_Back", 1), new AnimData("Magma_Storm_Front", 1));
                emitter.HeightBack = 32;
                emitter.HeightFront = 32;
                skill.HitboxAction.TileEmitter = emitter;
            }
            else if (ii == 464)
            {
                skill.Name = new LocalText("Dark Void");
                skill.Desc = new LocalText("Opposing Pokémon are dragged into a world of total darkness that makes them sleep.");
                skill.BaseCharges = 10;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 85;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("sleep", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 5;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TileEmitter = new SingleEmitter(new AnimData("Dark_Pulse_Ranger", 3));
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BetweenEmitter endAnim = new BetweenEmitter(new AnimData("Dark_Void_Back", 1), new AnimData("Dark_Void_Front", 1));
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Delay = 24;
                skill.HitboxAction.ActionFX.Sound = "DUN_Confusion";
            }
            else if (ii == 465)
            {
                skill.Name = new LocalText("-Seed Flare");
                skill.Desc = new LocalText("The user emits a shock wave from its body to attack its target. This may harshly lower the target's Sp. Def.");
                skill.BaseCharges = 9;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_special_defense", true, true, -2)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(35);//Emit
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Seed_Flare";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Vine_Whip_2", 3));
            }
            else if (ii == 466)
            {
                skill.Name = new LocalText("Ominous Wind");
                skill.Desc = new LocalText("The user blasts the target with a gust of repulsive wind. This may also raise all the user's stats at once.");
                skill.BaseCharges = 9;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                GroupEvent groupEvent = new GroupEvent();
                groupEvent.BaseEvents.Add(new StatusStackBattleEvent("mod_speed", false, true, 1));
                groupEvent.BaseEvents.Add(new StatusStackBattleEvent("mod_attack", false, true, 1));
                groupEvent.BaseEvents.Add(new StatusStackBattleEvent("mod_defense", false, true, 1));
                groupEvent.BaseEvents.Add(new StatusStackBattleEvent("mod_special_attack", false, true, 1));
                groupEvent.BaseEvents.Add(new StatusStackBattleEvent("mod_special_defense", false, true, 1));
                skill.Data.AfterActions.Add(0, new AdditionalEndEvent(groupEvent));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(32);//FlapAround
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("Ominous_Wind", 3, 0, 0, 128);
                overlay.TotalTime = 60;
                overlay.Movement = new RogueElements.Loc(6, 0);
                overlay.Layer = DrawLayer.Top;
                skill.HitboxAction.ActionFX.Emitter = overlay;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Ominous_Wind";
            }
            else if (ii == 467)
            {
                skill.Name = new LocalText("-Shadow Force");
                skill.Desc = new LocalText("The user disappears, then strikes the target any time in the next 5 turns. This move hits even if the target protects itself.");
                skill.BaseCharges = 5;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                SelfAction altAction = new SelfAction();
                altAction.CharAnimData = new CharAnimFrameType(06);//Charge
                altAction.TargetAlignments |= Alignment.Self;
                altAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                altAction.ActionFX.Sound = "DUN_Move_Start";
                skill.Data.BeforeTryActions.Add(0, new ChargeOrReleaseEvent("shadow_force", altAction));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("wide_guard", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("quick_guard", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("protect", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("all_protect", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("detect", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("kings_shield", true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 4;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Night_Shade_3";
            }
        }
    }
}
