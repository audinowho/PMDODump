using System;
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

        static void FillSkillsGen5Plus(SkillData skill, int ii, ref string fileName)
        {
            if (ii == 468)
            {
                skill.Name = new LocalText("-Hone Claws");
                skill.Desc = new LocalText("The user sharpens its claws to boost its Attack stat and Accuracy.");
                skill.BaseCharges = 16;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_accuracy", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Fury_Cutter";
            }
            else if (ii == 469)
            {
                skill.Name = new LocalText("Wide Guard");
                skill.Desc = new LocalText("The user and its allies are protected from wide-ranging attacks for five turns.");
                skill.BaseCharges = 16;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("wide_guard", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Sound = "DUN_Aurora_Beam_2";
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Screen_RSE_Pink", 3, -1, -1, 192));
            }
            else if (ii == 470)
            {
                skill.Name = new LocalText("-Guard Split");
                skill.Desc = new LocalText("The user employs its psychic powers to average its Defense and Sp. Def stats with those of the target.");
                skill.BaseCharges = 10;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatSplitEvent(false));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 2;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Teeter_Dance";
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Power_Trick", 2));
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Power_Trick", 2));
                skill.Data.HitFX.Sound = "DUN_Teeter_Dance";
            }
            else if (ii == 471)
            {
                skill.Name = new LocalText("Power Split");
                skill.Desc = new LocalText("The user employs its psychic power to average its Attack and Sp. Atk stats with those of the target.");
                skill.BaseCharges = 10;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatSplitEvent(true));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 2;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Safeguard";
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Power_Trick", 2));
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Power_Trick", 2));
                skill.Data.HitFX.Sound = "DUN_Safeguard";
            }
            else if (ii == 472)
            {
                skill.Name = new LocalText("-Wonder Room");
                skill.Desc = new LocalText("The user creates a bizarre area in which Physical attacks and Special attacks are swapped.");
                skill.BaseCharges = 10;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new GiveMapStatusEvent("wonder_room"));
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
            else if (ii == 473)
            {
                skill.Name = new LocalText("-Psyshock");
                skill.Desc = new LocalText("The user materializes an odd psychic wave to attack the target. This attack does physical damage.");
                skill.BaseCharges = 16;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnActions.Add(4, new FlipCategoryEvent(true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((OffsetAction)skill.HitboxAction).Range = 2;
                ((OffsetAction)skill.HitboxAction).Speed = 10;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Aura_Sphere";
            }
            else if (ii == 474)
            {
                skill.Name = new LocalText("-Venoshock");
                skill.Desc = new LocalText("The user drenches the target in a special poisonous liquid. This move's power is doubled if the target is poisoned.");
                skill.BaseCharges = 16;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.BeforeHits.Add(0, new StatusPowerEvent("poison", true));
                skill.Data.BeforeHits.Add(0, new StatusPowerEvent("poison_toxic", true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((OffsetAction)skill.HitboxAction).HitArea = OffsetAction.OffsetArea.Sides;
                ((OffsetAction)skill.HitboxAction).Range = 2;
                ((OffsetAction)skill.HitboxAction).Speed = 10;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Teeter_Dance";
            }
            else if (ii == 475)
            {
                skill.Name = new LocalText("-Autotomize");
                skill.Desc = new LocalText("The user sheds part of its body to make itself lighter and sharply raise its Movement Speed.");
                skill.BaseCharges = 15;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, 2));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
            }
            else if (ii == 476)
            {
                skill.Name = new LocalText("=Rage Powder");
                skill.Desc = new LocalText("The user scatters a cloud of irritating powder to draw attention to itself. Opponents aim only at the user.");
                skill.BaseCharges = 18;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("rage_powder", true, false));
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
                skill.HitboxAction.ActionFX.Sound = "_UNK_DUN_Rustle";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Puff_Brown", 3));
            }
            else if (ii == 477)
            {
                skill.Name = new LocalText("-Telekinesis");
                skill.Desc = new LocalText("The user makes the target float with its psychic power. The target is easier to hit for three turns.");
                skill.BaseCharges = 15;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("telekinesis", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Leer", 2));
                skill.HitboxAction.ActionFX.Sound = "DUN_Leer_2";
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Data.HitFX.Sound = "DUN_Morning_Sun";
            }
            else if (ii == 478)
            {
                skill.Name = new LocalText("-Magic Room");
                skill.Desc = new LocalText("The user creates a bizarre area in which Pokémon's abilities and held items lose their effects.");
                skill.BaseCharges = 15;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new GiveMapStatusEvent("magic_room"));
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
            else if (ii == 479)
            {
                skill.Name = new LocalText("-Smack Down");
                skill.Desc = new LocalText("The user throws a stone or similar projectile to attack an opponent. A flying Pokémon will fall to the ground when it's hit.");
                skill.BaseCharges = 14;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new GroupEvent(new RemoveAbilityEvent("levitate"),
                    new RemoveElementEvent("flying"), new RemoveStatusBattleEvent("flying", true), new RemoveStatusBattleEvent("bouncing", true),
                    new RemoveStatusBattleEvent("telekinesis", true), new RemoveStatusBattleEvent("magnet_rise", true))));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Shock_Wave_2";
            }
            else if (ii == 480)
            {
                skill.Name = new LocalText("-Storm Throw");
                skill.Desc = new LocalText("The user strikes the target with a fierce blow. This attack always results in a critical hit.");
                skill.BaseCharges = 15;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(4));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Wrap";
            }
            else if (ii == 481)
            {
                skill.Name = new LocalText("Flame Burst");
                skill.Desc = new LocalText("The user attacks the target with a bursting flame. The bursting flame damages opposing Pokémon next to the target as well.");
                skill.BaseCharges = 11;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.Explosion.Range = 1;
                skill.Explosion.HitTiles = true;
                skill.Explosion.Speed = 10;
                skill.Explosion.ExplodeFX.Sound = "DUN_Self-Destruct";
                skill.Explosion.TargetAlignments = Alignment.Foe;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Burned", 3));
                emitter.ParticlesPerTile = 0.8;
                emitter.LocHeight = 14;
                skill.Explosion.Emitter = emitter;

                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Blast_Seed", 1, 0, 0);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Fire_Spin";
                SingleEmitter endEmitter = new SingleEmitter(new AnimData("Ember", 3));
                endEmitter.LocHeight = 8;
                skill.Data.HitFX.Emitter = endEmitter;
            }
            else if (ii == 482)
            {
                skill.Name = new LocalText("Sludge Wave");
                skill.Desc = new LocalText("The user strikes everything around it by swamping the area with a giant sludge wave. This may also poison those hit.");
                skill.BaseCharges = 12;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("poison", true, true)));

                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 3;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;

                CircleSquareSprinkleEmitter emitter = new CircleSquareSprinkleEmitter(new AnimData("Bubbles_Purple", 3));
                emitter.ParticlesPerTile = 0.5;
                emitter.HeightSpeed = 20;
                emitter.SpeedDiff = 20;
                skill.Explosion.Emitter = emitter;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Magma_Storm";
            }
            else if (ii == 483)
            {
                skill.Name = new LocalText("-Quiver Dance");
                skill.Desc = new LocalText("The user lightly performs a beautiful, mystic dance. This boosts the user's Sp. Atk, Sp. Def, and Movement Speed.");
                skill.BaseCharges = 12;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, 1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_defense", true, false, 1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(32);//FlapAround
                SingleEmitter single = new SingleEmitter();
                SqueezedAreaEmitter emitter = new SqueezedAreaEmitter(new ParticleAnim(new AnimData("Synthesis_Sparkle", 3), 3));
                emitter.HeightSpeed = 30;
                emitter.Range = 8;
                emitter.StartHeight = -6;
                emitter.Bursts = 5;
                emitter.BurstTime = 3;
                emitter.ParticlesPerBurst = 3;
                emitter.SpeedDiff = 3;
                skill.HitboxAction.ActionFX.Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
            }
            else if (ii == 484)
            {
                skill.Name = new LocalText("=Heavy Slam");
                skill.Desc = new LocalText("The user slams into the target with its heavy body. The more the user outweighs the target, the greater the move's power.");
                skill.BaseCharges = 15;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState());
                skill.Data.BeforeHits.Add(0, new WeightCrushBasePowerEvent());
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(24);//Stomp
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Rage";
            }
            else if (ii == 485)
            {
                skill.Name = new LocalText("Synchronoise");
                skill.Desc = new LocalText("Using an odd shock wave, the user inflicts massive damage on any Pokémon of the same type in the area around it.");
                skill.BaseCharges = 13;
                skill.Data.Element = "none";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.BeforeHits.Add(0, new SynchroTypeEvent());
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(35);//Emit
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Power_Trick", 2));
                emitter.ParticlesPerTile = 0.4;
                emitter.RangeDiff = -12;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Heart_Swap";
            }
            else if (ii == 486)
            {
                skill.Name = new LocalText("Electro Ball");
                skill.Desc = new LocalText("The user hurls an electric orb at the target. The faster the user is than the target, the greater the move's power.");
                skill.BaseCharges = 15;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(30));
                skill.Data.BeforeHits.Add(0, new SpeedPowerEvent(false));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Shock_Wave", 3);
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Flash_Cannon";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Charge_Beam_Hit", 2));
                skill.Data.HitFX.Sound = "DUN_Discharge_2";
            }
            else if (ii == 487)
            {
                skill.Name = new LocalText("-Soak");
                skill.Desc = new LocalText("The user shoots a torrent of water at the target and changes the target's type to Water.");
                skill.BaseCharges = 18;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new ChangeToElementEvent("water"));
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Puff_Brown", 3));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Transform", terrainEmitter, new FlagType(typeof(LavaTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Surf";
            }
            else if (ii == 488)
            {
                skill.Name = new LocalText("-Flame Charge");
                skill.Desc = new LocalText("Cloaking itself in flame, the user attacks. Then, building up more power, the user raises its Movement Speed.");
                skill.BaseCharges = 18;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Data.AfterActions.Add(0, new AdditionalEndEvent(new StatusStackBattleEvent("mod_speed", false, true, 1)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Rock_Slide";
            }
            else if (ii == 489)
            {
                skill.Name = new LocalText("-Coil");
                skill.Desc = new LocalText("The user coils up and concentrates. This raises its Attack and Defense stats as well as its Accuracy.");
                skill.BaseCharges = 16;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, 1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_accuracy", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Conversion2";
            }
            else if (ii == 490)
            {
                skill.Name = new LocalText("-Low Sweep");
                skill.Desc = new LocalText("The user makes a swift attack on the target's legs, which lowers the target's Movement Speed.");
                skill.BaseCharges = 16;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_speed", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(21);//Kick
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Wide;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Drill Peck_2";
            }
            else if (ii == 491)
            {
                skill.Name = new LocalText("Acid Spray");
                skill.Desc = new LocalText("The user spits fluid that works to melt the target. This harshly lowers the target's Sp. Def stat.");
                skill.BaseCharges = 14;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(30));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_special_defense", true, true, -2)));
                skill.Strikes = 1;
                skill.Explosion.Range = 1;
                skill.Explosion.HitTiles = true;
                skill.Explosion.Speed = 10;
                skill.Explosion.ExplodeFX.Sound = "DUN_Magma_Storm";
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                CircleSquareSprinkleEmitter emitter = new CircleSquareSprinkleEmitter(new AnimData("Bubbles_Green", 3));
                emitter.ParticlesPerTile = 3;
                emitter.HeightSpeed = 20;
                emitter.SpeedDiff = 20;
                skill.Explosion.Emitter = emitter;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Gastro_Acid_Ball", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Aqua_Tail";
            }
            else if (ii == 492)
            {
                skill.Name = new LocalText("-Foul Play");
                skill.Desc = new LocalText("The user turns the target's power against it. The higher the target's Attack stat, the greater the move's power.");
                skill.BaseCharges = 14;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.BeforeHits.Add(-4, new FoulPlayEvent());
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Secret_Power";
            }
            else if (ii == 493)
            {
                skill.Name = new LocalText("Simple Beam");
                skill.Desc = new LocalText("The user's mysterious psychic wave changes the target's Ability to Simple.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new ChangeToAbilityEvent("simple", true));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Wave_Circle_White", 3));
                shotAnim.StartDistance = 16;
                shotAnim.Shots = 10;
                shotAnim.BurstTime = 3;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Encore_2";
            }
            else if (ii == 494)
            {
                skill.Name = new LocalText("Entrainment");
                skill.Desc = new LocalText("The user dances with an odd rhythm that compels the target to mimic it, making the target's Ability the same as the user's.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new ReflectAbilityEvent(true));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(26);//Dance
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 4;
                ((ThrowAction)skill.HitboxAction).LagBehindTime = 30;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Emote_Glowing", 2));
                emitter.LocHeight = 4;
                ((ThrowAction)skill.HitboxAction).ActionFX.Emitter = emitter;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Kinesis";
                skill.Data.HitCharAction = new CharAnimFrameType(26);//Dance
                SingleEmitter endEmitter = new SingleEmitter(new AnimData("Emote_Glowing", 2));
                endEmitter.LocHeight = 4;
                skill.Data.HitFX.Emitter = endEmitter;
                skill.Data.HitFX.Sound = "DUN_Kinesis";
                skill.Data.HitFX.Delay = 30;
            }
            else if (ii == 495)
            {
                skill.Name = new LocalText("-After You");
                skill.Desc = new LocalText("The user helps the target and raises its Movement Speed to the highest level.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, 6));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.FrontAndCorners;
                skill.HitboxAction.TargetAlignments = Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Friend;
            }
            else if (ii == 496)
            {
                skill.Name = new LocalText("-Round");
                skill.Desc = new LocalText("The user attacks the target with a song. Nearby allies will join in the Round and do extra damage.");
                skill.BaseCharges = 16;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.Explosion.Range = 1;
                skill.Explosion.Speed = 10;
                skill.Explosion.ExplodeFX.Sound = "DUN_Sing";
                skill.Explosion.TargetAlignments = Alignment.Foe;
                RepeatEmitter single = new RepeatEmitter(new AnimData("Circle_Thick_Red_Out", 2));
                single.Bursts = 4;
                single.BurstTime = 8;
                skill.Explosion.ExplodeFX.Emitter = single;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter();
                for (int nn = 0; nn < 44; nn++)
                    emitter.Anims.Add(new ParticleAnim(new AnimData("Music_Notes", 1, nn, nn)));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 2;
                emitter.Bursts = 4;
                emitter.StartDistance = 8;
                skill.Explosion.Emitter = emitter;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(29);//Sing
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
            }
            else if (ii == 497)
            {
                skill.Name = new LocalText("Echoed Voice");
                skill.Desc = new LocalText("The user attacks the target with an echoing voice. If this move is used every turn, it does greater damage.");
                skill.BaseCharges = 14;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(30));
                skill.Data.BeforeHits.Add(0, new RepeatHitEvent("last_used_move", "times_move_used", 5, 1, true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(30);//Sound
                ((ProjectileAction)skill.HitboxAction).Range = 3;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Wave_Circle_Yellow", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Hyper_Voice";
                RepeatEmitter endAnim = new RepeatEmitter(new AnimData("Circle_Thick_Red_Out", 2));
                endAnim.Bursts = 4;
                endAnim.BurstTime = 8;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 498)
            {
                skill.Name = new LocalText("-Chip Away");
                skill.Desc = new LocalText("Looking for an opening, the user strikes consistently. The target's stat changes don't affect this attack's damage.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnActions.Add(0, new AddContextStateEvent(new Infiltrator(new StringKey("MSG_INFILTRATOR_SKILL"))));
                skill.Data.BeforeHits.Add(4, new IgnoreStatsEvent(true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Cut";
            }
            else if (ii == 499)
            {
                skill.Name = new LocalText("Clear Smog");
                skill.Desc = new LocalText("The user attacks by throwing a clump of special mud. All stat changes are returned to normal.");
                skill.BaseCharges = 16;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new RemoveStateStatusBattleEvent(typeof(StatChangeState), true, new StringKey("MSG_BUFF_REMOVE"))));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(33);//Gas
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Smoke_White", 2));
                shotAnim.StartDistance = 8;
                shotAnim.Shots = 7;
                shotAnim.BurstTime = 6;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Whirlwind";
            }
            else if (ii == 500)
            {
                skill.Name = new LocalText("=Stored Power");
                skill.Desc = new LocalText("The user attacks the target with stored power. The more the user's stats are raised, the greater the move's power.");
                skill.BaseCharges = 14;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(20));
                HashSet<string> statChanges = new HashSet<string>();
                statChanges.Add("mod_speed");
                statChanges.Add("mod_attack");
                statChanges.Add("mod_defense");
                statChanges.Add("mod_special_attack");
                statChanges.Add("mod_special_defense");
                statChanges.Add("mod_accuracy");
                statChanges.Add("mod_evasion");
                skill.Data.BeforeHits.Add(0, new StatBasePowerEvent(20, false, statChanges));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 1;
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
                skill.HitboxAction.ActionFX.Sound = "DUN_Ice_Beam";
            }
            else if (ii == 501)
            {
                skill.Name = new LocalText("Quick Guard");
                skill.Desc = new LocalText("The user protects itself and its allies from long-range moves for five turns.");
                skill.BaseCharges = 15;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("quick_guard", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Sound = "DUN_Astonish_2";
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 3, -1, -1, 192));
            }
            else if (ii == 502)
            {
                skill.Name = new LocalText("-Ally Switch");
                skill.Desc = new LocalText("The user teleports all allies using a strange power, switching their places.");
                skill.BaseCharges = 22;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new SwitcherEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 5;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Sound = "_UNK_DUN_Flash";
            }
            else if (ii == 503)
            {
                skill.Name = new LocalText("-Scald");
                skill.Desc = new LocalText("The user shoots boiling hot water at its target. This may also leave the target with a burn.");
                skill.BaseCharges = 14;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(55));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("burn", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 2;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Water_Gun_3";
                skill.Data.HitFX.Sound = "DUN_Pokemon_Trap";
            }
            else if (ii == 504)
            {
                skill.Name = new LocalText("-Shell Smash");
                skill.Desc = new LocalText("The user breaks its shell, which lowers Defense and Sp. Def stats but sharply raises its Attack, Sp. Atk, and Movement Speed.");
                skill.BaseCharges = 12;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 2));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, -1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, 2));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_defense", true, false, -1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
            }
            else if (ii == 505)
            {
                skill.Name = new LocalText("Heal Pulse");
                skill.Desc = new LocalText("The user emits a healing pulse which restores the target's HP by up to half of its max HP.");
                skill.BaseCharges = 14;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new PulseState());
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new RestoreHPEvent(1, 2, true));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Wave_Circle_Pink", 3));
                shotAnim.StartDistance = 16;
                shotAnim.Shots = 10;
                shotAnim.BurstTime = 3;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Aura_Sphere";
            }
            else if (ii == 506)
            {
                skill.Name = new LocalText("-Hex");
                skill.Desc = new LocalText("This relentless attack does massive damage to a target affected by major status conditions.");
                skill.BaseCharges = 15;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(45));
                skill.Data.BeforeHits.Add(0, new MajorStatusPowerEvent(true, 2, 1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Night_Shade";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Mean_Look_RSE", 3));
            }
            else if (ii == 507)
            {
                skill.Name = new LocalText("Sky Drop");
                skill.Desc = new LocalText("The user takes the target into the sky, then drops it during the next turn. The target cannot attack while in the sky.");
                skill.BaseCharges = 15;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(60));


                DashAction altAction = new DashAction();
                altAction.CharAnim = 05;//Attack
                altAction.AppearanceMod = DashAction.DashAppearance.Kidnap;
                altAction.Range = 2;
                altAction.StopAtWall = true;
                altAction.StopAtHit = true;
                altAction.HitTiles = true;
                altAction.TargetAlignments = Alignment.Foe | Alignment.Friend;
                BattleFX altPreFX = new BattleFX();
                altPreFX.Sound = "DUN_Thief";
                altAction.ActionFX = altPreFX;

                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = Alignment.Foe | Alignment.Friend;

                BattleData altMoveData = new BattleData();
                altMoveData.Element = DataManager.Instance.DefaultElement;
                altMoveData.Category = BattleData.SkillCategory.None;
                altMoveData.HitRate = -1;
                altMoveData.HitCharAction = new CharAnimProcess(CharAnimProcess.ProcessType.Fly, 04);//Hurt
                BattleFX altPostFX = new BattleFX();
                altPostFX.Delay = 10;
                altMoveData.HitFX = altPostFX;
                altMoveData.BeforeActions.Add(0, new SingleStrikeEvent());
                altMoveData.OnActions.Add(1, new NoPierceEvent(true, false));
                altMoveData.OnHits.Add(0, new SkyDropStatusBattleEvent("sky_drop_target", "sky_drop_user", true, false));

                skill.Data.BeforeTryActions.Add(0, new ChargeCustomEvent(altAction, altExplosion, altMoveData));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimProcess(CharAnimProcess.ProcessType.Drop);
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Deep_Fall";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Foe | Alignment.Friend;
            }
            else if (ii == 508)
            {
                skill.Name = new LocalText("-Shift Gear");
                skill.Desc = new LocalText("The user rotates its gears, raising its Attack and sharply raising its Movement Speed.");
                skill.BaseCharges = 10;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, 2));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Flash_Cannon";
            }
            else if (ii == 509)
            {
                skill.Name = new LocalText("-Circle Throw");
                skill.Desc = new LocalText("The target is thrown into the distance by the user.");
                skill.BaseCharges = 16;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(0, new ThrowBackEvent(4, new DamageFormulaEvent()));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 510)
            {
                skill.Name = new LocalText("Incinerate");
                skill.Desc = new LocalText("The user attacks opposing Pokémon with fire. If a Pokémon has a food item, the item becomes burned up and unusable.");
                skill.BaseCharges = 18;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(45));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                HashSet<FlagType> eligibles = new HashSet<FlagType>();
                eligibles.Add(new FlagType(typeof(EdibleState)));
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new DestroyItemEvent(false, false, "seed_decoy", eligibles)));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 3;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Fire_Column_Ranger", 3));
                emitter.LocHeight = 32;
                skill.HitboxAction.TileEmitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Sacred_Fire";
            }
            else if (ii == 511)
            {
                skill.Name = new LocalText("-Quash");
                skill.Desc = new LocalText("The user suppresses the target and makes it flinch.");
                skill.BaseCharges = 16;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("flinch", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 3;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 512)
            {
                skill.Name = new LocalText("-Acrobatics");
                skill.Desc = new LocalText("The user nimbly strikes the target. If the user is not holding an item, this attack inflicts massive damage.");
                skill.BaseCharges = 14;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.BeforeHits.Add(0, new AcrobaticEvent());
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 513)
            {
                skill.Name = new LocalText("-Reflect Type");
                skill.Desc = new LocalText("The user reflects the target's type, making it the same type as the target.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new ReflectElementEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 4;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
            }
            else if (ii == 514)
            {
                skill.Name = new LocalText("-Retaliate");
                skill.Desc = new LocalText("The user gets ready to retaliate, attacking whenever nearby foes deal damage to its allies.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                SingleEmitter altAnim = new SingleEmitter(new AnimData("Charge_Up", 3));
                skill.Data.BeforeTryActions.Add(0, new WatchOrStrikeEvent("retaliate", altAnim, "DUN_Move_Start"));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(08);//Strike
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.FrontAndCorners;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Revenge";
            }
            else if (ii == 515)
            {
                skill.Name = new LocalText("=Final Gambit");
                skill.Desc = new LocalText("The user risks everything to attack its target. The user's HP drops to 1, and does damage equal to the amount it lost.");
                skill.BaseCharges = 5;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(-1, new UserHPDamageEvent());
                skill.Data.AfterActions.Add(0, new OnHitAnyEvent(true, 100, new HPTo1Event(false)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Flare_Blitz_2";
            }
            else if (ii == 516)
            {
                skill.Name = new LocalText("Bestow");
                skill.Desc = new LocalText("The user passes its held item to the target, replacing the target's held item.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnActions.Add(0, new HeldItemMoveEvent());
                skill.Data.OnHits.Add(0, new BestowItemEvent());
                skill.Data.AfterActions.Add(-1, new LandItemEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                skill.HitboxAction.PreActions.Add(new BattleFX(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Wonder_Tile", 0));
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(42);//Rotate
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Present", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Copycat_2";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2));
                skill.Data.HitFX.Sound = "DUN_Equip";
            }
            else if (ii == 517)
            {
                skill.Name = new LocalText("Inferno");
                skill.Desc = new LocalText("The user attacks by engulfing the target in an intense fire. This leaves the target with a burn.");
                skill.BaseCharges = 12;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 50;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("burn", true, true)));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((OffsetAction)skill.HitboxAction).HitArea = OffsetAction.OffsetArea.Area;
                ((OffsetAction)skill.HitboxAction).Range = 3;
                ((OffsetAction)skill.HitboxAction).Speed = 10;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Fire_Column_Ranger", 3));
                emitter.LocHeight = 32;
                skill.HitboxAction.TileEmitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Open_Chamber";
            }
            else if (ii == 518)
            {
                skill.Name = new LocalText("-Water Pledge");
                skill.Desc = new LocalText("Whenever an enemy is attacked, the user follows up by striking the target with a column of water.");
                skill.BaseCharges = 20;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(30));
                SingleEmitter altAnim = new SingleEmitter(new AnimData("Charge_Up", 3));
                skill.Data.BeforeTryActions.Add(0, new WatchOrStrikeEvent("water_pledge", altAnim, "DUN_Move_Start"));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Water_Column_Ranger", 3));
                emitter.LocHeight = 32;
                skill.HitboxAction.TileEmitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Water_Spout";
            }
            else if (ii == 519)
            {
                skill.Name = new LocalText("-Fire Pledge");
                skill.Desc = new LocalText("Whenever an enemy is attacked, the user follows up by striking the target with a column of fire.");
                skill.BaseCharges = 20;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(30));
                SingleEmitter altAnim = new SingleEmitter(new AnimData("Charge_Up", 3));
                skill.Data.BeforeTryActions.Add(0, new WatchOrStrikeEvent("fire_pledge", altAnim, "DUN_Move_Start"));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Fire_Column_Ranger", 3));
                emitter.LocHeight = 32;
                skill.HitboxAction.TileEmitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Open_Chamber";
            }
            else if (ii == 520)
            {
                skill.Name = new LocalText("-Grass Pledge");
                skill.Desc = new LocalText("Whenever an enemy is attacked, the user follows up by striking the target with a column of grass.");
                skill.BaseCharges = 20;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(30));
                SingleEmitter altAnim = new SingleEmitter(new AnimData("Charge_Up", 3));
                skill.Data.BeforeTryActions.Add(0, new WatchOrStrikeEvent("grass_pledge", altAnim, "DUN_Move_Start"));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                FiniteSprinkleEmitter endEmitter = new FiniteSprinkleEmitter();
                endEmitter.Anims.Add(new ParticleAnim(new AnimData("Leaf_Storm_Shot_Front", 2), 2));
                endEmitter.Anims.Add(new ParticleAnim(new AnimData("Leaf_Storm_Shot_Back", 2), 2));
                endEmitter.HeightSpeed = 320;
                endEmitter.TotalParticles = 6;
                endEmitter.Range = 4;
                endEmitter.Speed = 16;
                skill.HitboxAction.TileEmitter = endEmitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Magical_Leaf";
            }
            else if (ii == 521)
            {
                skill.Name = new LocalText("-Volt Switch");
                skill.Desc = new LocalText("After making its attack, the user rushes back several steps.");
                skill.BaseCharges = 18;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new OnHitAnyEvent(false, 100, new HopEvent(3, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(34);//Shock
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Charge_Beam_2";
            }
            else if (ii == 522)
            {
                skill.Name = new LocalText("-Struggle Bug");
                skill.Desc = new LocalText("While resisting, the user attacks the opposing Pokémon. This lowers the Sp. Atk stat of those hit.");
                skill.BaseCharges = 20;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(25));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_special_attack", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(35);//Emit
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_String_Shot_2";
            }
            else if (ii == 523)
            {
                skill.Name = new LocalText("-Bulldoze");
                skill.Desc = new LocalText("The user strikes everything around it and flattens the ground. This lowers the Movement Speed of those hit.");
                skill.BaseCharges = 18;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_speed", true, true, -1)));
                skill.Data.OnHitTiles.Add(0, new RemoveTrapEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Wall_Break", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", terrainEmitter, new FlagType(typeof(WallTerrainState)), new FlagType(typeof(WaterTerrainState)), new FlagType(typeof(LavaTerrainState)), new FlagType(typeof(AbyssTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(31);//Rumble
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.ActionFX.ScreenMovement = new ScreenMover(0, 8, 30);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Seismic_Toss";
            }
            else if (ii == 524)
            {
                skill.Name = new LocalText("Frost Breath");
                skill.Desc = new LocalText("The user blows its cold breath on the target. This attack always results in a critical hit.");
                skill.BaseCharges = 12;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(4));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Smoke_White", 2));
                shotAnim.StartDistance = 8;
                shotAnim.Shots = 7;
                shotAnim.BurstTime = 6;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Mist";
            }
            else if (ii == 525)
            {
                skill.Name = new LocalText("=Dragon Tail");
                skill.Desc = new LocalText("The target is struck with the violent whip of a tail, knocking it away.");
                skill.BaseCharges = 15;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new KnockBackEvent(4)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Wide;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "_UNK_DUN_Break";
            }
            else if (ii == 526)
            {
                skill.Name = new LocalText("-Work Up");
                skill.Desc = new LocalText("The user is roused, and its Attack and Sp. Atk stats increase.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Sky_Attack";
            }
            else if (ii == 527)
            {
                skill.Name = new LocalText("-Electroweb");
                skill.Desc = new LocalText("The user attacks and captures opposing Pokémon by using an electric net. This lowers their Movement Speed.");
                skill.BaseCharges = 15;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_speed", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TileEmitter = new SingleEmitter(new AnimData("String_Shot_Web", 30));
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Follow_Me";
            }
            else if (ii == 528)
            {
                skill.Name = new LocalText("-Wild Charge");
                skill.Desc = new LocalText("The user shrouds itself in electricity and smashes into its target. This also damages the user a little.");
                skill.BaseCharges = 16;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(85));
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
            }
            else if (ii == 529)
            {
                skill.Name = new LocalText("=Drill Run");
                skill.Desc = new LocalText("The user crashes into its target while rotating its body like a drill. Critical hits land more easily.");
                skill.BaseCharges = 15;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnActions.Add(0, new BoostCriticalEvent(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Wall_Break", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Rollout", terrainEmitter, new FlagType(typeof(WallTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 20;//Jab
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                ((DashAction)skill.HitboxAction).AnimOffset = 12;
                ((DashAction)skill.HitboxAction).Anim = new AnimData("Fury_Attack", 2);
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Fire_Blast";
            }
            else if (ii == 530)
            {
                skill.Name = new LocalText("-Dual Chop");
                skill.Desc = new LocalText("The user attacks its target by hitting it with brutal strikes. The target is hit twice in a row.");
                skill.BaseCharges = 18;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(45));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 2;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(09);//Chop
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Me_First";
            }
            else if (ii == 531)
            {
                skill.Name = new LocalText("-Heart Stamp");
                skill.Desc = new LocalText("The user unleashes a vicious blow after its cute act makes the target less wary. This may also make the target flinch.");
                skill.BaseCharges = 20;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 532)
            {
                skill.Name = new LocalText("-Horn Leech");
                skill.Desc = new LocalText("The user drains the target's energy with its horns. The user's HP is restored by half the damage taken by the target.");
                skill.BaseCharges = 16;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new HPDrainEvent(2));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 20;//Jab
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 533)
            {
                skill.Name = new LocalText("=Sacred Sword");
                skill.Desc = new LocalText("The user attacks by slicing with a sword. The target's stat changes don't affect this attack's damage.");
                skill.BaseCharges = 12;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnActions.Add(0, new AddContextStateEvent(new Infiltrator(new StringKey("MSG_INFILTRATOR_SKILL"))));
                skill.Data.BeforeHits.Add(4, new IgnoreStatsEvent(true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;

                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).CharAnim = 13;//Slice
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Fury_Cutter";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Slash_Blue_RSE", 3));
                skill.Data.HitFX.Sound = "DUN_Metal_Sound";
            }
            else if (ii == 534)
            {
                skill.Name = new LocalText("=Razor Shell");
                skill.Desc = new LocalText("The user cuts its target with sharp shells. This may also lower the target's Defense stat.");
                skill.BaseCharges = 18;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_defense", true, true, -1)));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(13);//Slice
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Wide;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Revenge";
            }
            else if (ii == 535)
            {
                skill.Name = new LocalText("-Heat Crash");
                skill.Desc = new LocalText("The user slams its target with its flame-covered body. The more the user outweighs the target, the greater the move's power.");
                skill.BaseCharges = 16;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState());
                skill.Data.BeforeHits.Add(0, new WeightCrushBasePowerEvent());
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(24);//Stomp
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "_UNK_DUN_Break";
            }
            else if (ii == 536)
            {
                skill.Name = new LocalText("-Leaf Tornado");
                skill.Desc = new LocalText("The user attacks its target by encircling it in sharp leaves. This attack may also lower the target's Accuracy.");
                skill.BaseCharges = 16;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_accuracy", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 537)
            {
                skill.Name = new LocalText("-Steamroller");
                skill.Desc = new LocalText("The user crushes its targets by rolling over them with its rolled-up body. This attack may make the target flinch.");
                skill.BaseCharges = 14;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Gravity";
            }
            else if (ii == 538)
            {
                skill.Name = new LocalText("-Cotton Guard");
                skill.Desc = new LocalText("The user protects itself by wrapping its body in soft cotton, which drastically raises the user's Defense stat.");
                skill.BaseCharges = 14;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, 3));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "_UNK_DUN_Shatter";
            }
            else if (ii == 539)
            {
                skill.Name = new LocalText("-Night Daze");
                skill.Desc = new LocalText("The user lets loose a pitch-black shock wave at its target. This may also lower the target's Accuracy.");
                skill.BaseCharges = 10;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(55));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_accuracy", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 30;
                CircleSquareAreaEmitter areaEmitter = new CircleSquareAreaEmitter(new AnimData("Dark_Pulse_Particle", 3), new AnimData("Dark_Pulse_Ranger", 3));
                areaEmitter.ParticlesPerTile = 1.5;
                areaEmitter.RangeDiff = -12;
                ((AreaAction)skill.HitboxAction).Emitter = areaEmitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Radar_Orb";
            }
            else if (ii == 540)
            {
                skill.Name = new LocalText("-Psystrike");
                skill.Desc = new LocalText("The user materializes an odd psychic wave to attack everything around it. This attack does physical damage.");
                skill.BaseCharges = 10;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnActions.Add(4, new FlipCategoryEvent(true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Miracle_Eye";
            }
            else if (ii == 541)
            {
                skill.Name = new LocalText("Tail Slap");
                skill.Desc = new LocalText("The user attacks by striking the target with its hard tail. It hits the target five times in a row.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 70;
                skill.Data.SkillStates.Set(new BasePowerState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 5;
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
            else if (ii == 542)
            {
                skill.Name = new LocalText("Hurricane");
                skill.Desc = new LocalText("The user attacks by wrapping its opponent in a fierce wind that flies up into the sky. This may also confuse the target.");
                skill.BaseCharges = 10;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 70;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnActions.Add(0, new WeatherNeededEvent("rain", new SetAccuracyEvent(-1)));
                skill.Data.OnActions.Add(0, new WeatherNeededEvent("sunny", new SetAccuracyEvent(50)));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("confuse", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((OffsetAction)skill.HitboxAction).HitArea = OffsetAction.OffsetArea.Area;
                ((OffsetAction)skill.HitboxAction).Range = 2;
                ((OffsetAction)skill.HitboxAction).Speed = 4;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Gust", 1));
                emitter.ParticlesPerTile = 0.7;
                emitter.LocHeight = 24;
                ((OffsetAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.ActionFX.Sound = "DUN_Howling_Wind";
            }
            else if (ii == 543)
            {
                skill.Name = new LocalText("**Head Charge");
                skill.Desc = new LocalText("The user charges its head into its target, using its powerful guard hair. This also damages the user a little.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 544)
            {
                skill.Name = new LocalText("**Gear Grind");
                skill.Desc = new LocalText("The user attacks by throwing steel gears at its target twice.");
                skill.BaseCharges = 15;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 2;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 545)
            {
                skill.Name = new LocalText("**Searing Shot");
                skill.Desc = new LocalText("The user torches everything around it with an inferno of scarlet flames. This may also leave those hit with a burn.");
                skill.BaseCharges = 5;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 546)
            {
                skill.Name = new LocalText("**Techno Blast");
                skill.Desc = new LocalText("The user fires a beam of light at its target. The move's type changes depending on the form the user is in.");
                skill.BaseCharges = 5;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 547)
            {
                skill.Name = new LocalText("**Relic Song");
                skill.Desc = new LocalText("The user sings an ancient song and attacks by appealing to the hearts of the listening opposing Pokémon. This may also induce sleep.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 548)
            {
                skill.Name = new LocalText("-Secret Sword");
                skill.Desc = new LocalText("The user cuts with its long horn. The odd power contained in the horn does physical damage to the target.");
                skill.BaseCharges = 16;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(85));
                skill.Data.OnActions.Add(4, new FlipCategoryEvent(true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(13);//Slice
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Wide;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Slash_Blue_RSE", 3));
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 549)
            {
                skill.Name = new LocalText("-Glaciate");
                skill.Desc = new LocalText("The user attacks by blowing freezing cold air at opposing Pokémon. This lowers their Movement Speed.");
                skill.BaseCharges = 12;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_speed", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 550)
            {
                skill.Name = new LocalText("**Bolt Strike");
                skill.Desc = new LocalText("The user surrounds itself with a great amount of electricity and charges its target. This may also leave the target with paralysis.");
                skill.BaseCharges = 5;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(130));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 551)
            {
                skill.Name = new LocalText("**Blue Flare");
                skill.Desc = new LocalText("The user attacks by engulfing the target in an intense, yet beautiful, blue flame. This may also leave the target with a burn.");
                skill.BaseCharges = 5;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(130));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 552)
            {
                skill.Name = new LocalText("-Fiery Dance");
                skill.Desc = new LocalText("Cloaked in flames, the user dances and flaps its wings. This may also raise the user's Sp. Atk stat.");
                skill.BaseCharges = 12;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new AdditionalEndEvent(new StatusStackBattleEvent("mod_special_attack", false, true, 1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(26);//Dance
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Flame_Wheel";
            }
            else if (ii == 553)
            {
                skill.Name = new LocalText("**Freeze Shock");
                skill.Desc = new LocalText("On the second turn, the user hits the target with electrically charged ice. This may also leave the target with paralysis.");
                skill.BaseCharges = 5;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(140));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 554)
            {
                skill.Name = new LocalText("**Ice Burn");
                skill.Desc = new LocalText("On the second turn, an ultracold, freezing wind surrounds the target. This may leave the target with a burn.");
                skill.BaseCharges = 5;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(140));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 555)
            {
                skill.Name = new LocalText("-Snarl");
                skill.Desc = new LocalText("The user yells as if it's ranting about something, which lowers the Sp. Atk stat of opposing Pokémon.");
                skill.BaseCharges = 14;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(30));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_special_attack", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(30);//Sound
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 4;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Thief";
            }
            else if (ii == 556)
            {
                skill.Name = new LocalText("Icicle Crash");
                skill.Desc = new LocalText("The user attacks by harshly dropping large icicles onto the target. This may also make the target flinch.");
                skill.BaseCharges = 9;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(65));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(31);//Rumble
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                ((ThrowAction)skill.HitboxAction).LagBehindTime = 20;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Ice_Fang";
                SqueezedAreaEmitter emitter = new SqueezedAreaEmitter(new ParticleAnim(new AnimData("Ice_Ball", 3), 6));
                emitter.HeightSpeed = -260;
                emitter.Range = 6;
                emitter.StartHeight = 72;
                emitter.Bursts = 3;
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 1;
                skill.HitboxAction.TileEmitter = emitter;
                FiniteReleaseEmitter endAnim = new FiniteReleaseEmitter(new AnimData("Ice_Pieces", 6, 0, 0), new AnimData("Ice_Pieces", 12, 1, 1), new AnimData("Ice_Pieces", 12, 1, 1));
                endAnim.BurstTime = 2;
                endAnim.ParticlesPerBurst = 4;
                endAnim.Bursts = 4;
                endAnim.StartDistance = 4;
                endAnim.Speed = 60;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Ice_Ball_2";
            }
            else if (ii == 557)
            {
                skill.Name = new LocalText("**V-create");
                skill.Desc = new LocalText("With a hot flame on its forehead, the user hurls itself at its target. This lowers the user's Defense, Sp. Def, and Movement Speed.");
                skill.BaseCharges = 5;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 95;
                skill.Data.SkillStates.Set(new BasePowerState(180));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 558)
            {
                skill.Name = new LocalText("**Fusion Flare");
                skill.Desc = new LocalText("The user brings down a giant flame. This attack is more powerful when influenced by an enormous thunderbolt.");
                skill.BaseCharges = 5;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 559)
            {
                skill.Name = new LocalText("**Fusion Bolt");
                skill.Desc = new LocalText("The user throws down a giant thunderbolt. This move is more powerful when influenced by an enormous flame.");
                skill.BaseCharges = 5;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 560)
            {
                skill.Name = new LocalText("**Flying Press");
                skill.Desc = new LocalText("The user dives down onto the target from the sky. This move is Fighting and Flying type simultaneously.");
                skill.BaseCharges = 10;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 95;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 561)
            {
                skill.Name = new LocalText("**Mat Block");
                skill.Desc = new LocalText("Using a pulled-up mat as a shield, the user protects itself and its allies from damaging moves. This does not stop status moves.");
                skill.BaseCharges = 10;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 562)
            {
                skill.Name = new LocalText("-Belch");
                skill.Desc = new LocalText("The user readies a belch. Eating a food item will unleash the move.");
                skill.BaseCharges = 8;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("belch", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(30);//Sound
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Move_Start";
                preFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 563)
            {
                skill.Name = new LocalText("-Rototiller");
                skill.Desc = new LocalText("The user overturns the soil, destroying obstacles along the way.");
                skill.BaseCharges = 18;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                //skill.Data.OnHits.Add(0, new CharElementNeededEvent("grass", new StatusStackBattleEvent("mod_attack", true, false, 1), new StatusStackBattleEvent("mod_special_attack", true, false, 1)));
                skill.Data.OnHitTiles.Add(0, new RemoveTrapEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Wall_Break", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", terrainEmitter, new FlagType(typeof(WallTerrainState)), new FlagType(typeof(WaterTerrainState)), new FlagType(typeof(LavaTerrainState)), new FlagType(typeof(AbyssTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 4;
                //((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.Wide;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                //skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                //skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "_UNK_DUN_Heave_Up";
            }
            else if (ii == 564)
            {
                skill.Name = new LocalText("Sticky Web");
                skill.Desc = new LocalText("The user shoots a sticky net at the target, causing its item to become sticky.");
                skill.BaseCharges = 14;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new SetItemStickyEvent(true, false, "seed_decoy", true, new HashSet<FlagType>()));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 2;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("String_Shot_Ball", 3, 0, 0));
                shotAnim.StartDistance = 16;
                shotAnim.Shots = 14;
                shotAnim.BurstTime = 2;
                ((ProjectileAction)skill.HitboxAction).StreamEmitter = shotAnim;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Sticky_Trap";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("String_Shot_Web", 6));
            }
            else if (ii == 565)
            {
                skill.Name = new LocalText("-Fell Stinger");
                skill.Desc = new LocalText("When the user knocks out a target with this move, the user's Attack stat rises drastically.");
                skill.BaseCharges = 16;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new KnockOutNeededEvent(new StatusStackBattleEvent("mod_attack", false, true, 3)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 20;//Jab
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 566)
            {
                skill.Name = new LocalText("Phantom Force");
                skill.Desc = new LocalText("The user vanishes somewhere, then strikes the target any time in the next 5 turns. This move hits even if the target protects itself.");
                skill.BaseCharges = 16;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                SelfAction altAction = new SelfAction();
                altAction.CharAnimData = new CharAnimFrameType(41);//Double
                altAction.TargetAlignments |= Alignment.Self;
                altAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Dark_Void_Sparkle", 1));
                BattleFX altPreFX = new BattleFX();
                altPreFX.Sound = "DUN_Curse";
                altAction.PreActions.Add(altPreFX);
                skill.Data.BeforeTryActions.Add(0, new ChargeOrReleaseEvent("phantom_force", altAction));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("wide_guard", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("quick_guard", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("protect", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("all_protect", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("detect", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("kings_shield", true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(08);//Strike
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Dark_Void_Sparkle", 1));
                preFX.Sound = "DUN_Snadow_Sneak_2";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Thief_Hit_Dark", 2));
            }
            else if (ii == 567)
            {
                skill.Name = new LocalText("-Trick-or-Treat");
                skill.Desc = new LocalText("The user takes the target trick-or-treating. This adds Ghost type to the target's type.");
                skill.BaseCharges = 16;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new AddElementEvent("ghost"));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.FrontAndCorners;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                SingleEmitter emitter = new SingleEmitter(new AnimData("Spite", 3));
                emitter.Layer = DrawLayer.Back;
                emitter.LocHeight = 24;
                skill.Data.HitFX.Emitter = emitter;
            }
            else if (ii == 568)
            {
                skill.Name = new LocalText("=Noble Roar");
                skill.Desc = new LocalText("Letting out a noble roar, the user intimidates the target and lowers its Attack and Sp. Atk stats.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, -1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, -1));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Growl", 2), 2);
                emitter.Offset = 12;
                emitter.Layer = DrawLayer.Top;
                ((AreaAction)skill.HitboxAction).ActionFX.Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 569)
            {
                skill.Name = new LocalText("Ion Deluge");
                skill.Desc = new LocalText("The user disperses electrically charged particles, which changes all moves by those affected to the Electric type.");
                skill.BaseCharges = 14;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("electrified", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
            }
            else if (ii == 570)
            {
                skill.Name = new LocalText("**Parabolic Charge");
                skill.Desc = new LocalText("The user attacks everything around it. The user's HP is restored by half the damage taken by those hit.");
                skill.BaseCharges = 1;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = 100;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 571)
            {
                skill.Name = new LocalText("-Forest's Curse");
                skill.Desc = new LocalText("The user puts a forest curse on the target. Afflicted targets are now Grass type as well.");
                skill.BaseCharges = 15;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new AddElementEvent("grass"));
                skill.Strikes = 1;
                skill.HitboxAction = new WaveMotionAction();
                ((WaveMotionAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((WaveMotionAction)skill.HitboxAction).Range = 5;
                ((WaveMotionAction)skill.HitboxAction).Speed = 10;
                ((WaveMotionAction)skill.HitboxAction).Linger = 6;
                ((WaveMotionAction)skill.HitboxAction).Wide = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
            }
            else if (ii == 572)
            {
                skill.Name = new LocalText("Petal Blizzard");
                skill.Desc = new LocalText("The user stirs up a violent petal blizzard and attacks everything around it.");
                skill.BaseCharges = 10;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(32);//FlapAround
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("Petal_Blizzard", 3, -1, -1, 128);
                overlay.TotalTime = 60;
                overlay.Movement = new Loc(10, 0);
                overlay.Layer = DrawLayer.Bottom;
                skill.HitboxAction.ActionFX.Emitter = overlay;
                skill.HitboxAction.ActionFX.Sound = "DUN_Leaf_Storm_3";
                skill.Data.HitFX.Sound = "DUN_Leaf_Storm_2";
            }
            else if (ii == 573)
            {
                skill.Name = new LocalText("-Freeze-Dry");
                skill.Desc = new LocalText("The user rapidly cools the target. This move is super effective on Water types.");
                skill.BaseCharges = 12;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.ElementEffects.Add(0, new TypeSuperEvent("water"));
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((OffsetAction)skill.HitboxAction).HitArea = OffsetAction.OffsetArea.Area;
                ((OffsetAction)skill.HitboxAction).Range = 2;
                ((OffsetAction)skill.HitboxAction).Speed = 10;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 574)
            {
                skill.Name = new LocalText("-Disarming Voice");
                skill.Desc = new LocalText("Letting out a charming cry, the user does emotional damage to opposing Pokémon. This attack never misses.");
                skill.BaseCharges = 15;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(30);//Sound
                ((ProjectileAction)skill.HitboxAction).Range = 3;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Wave_Circle_Pink", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Cotton_Spore";
            }
            else if (ii == 575)
            {
                skill.Name = new LocalText("-Parting Shot");
                skill.Desc = new LocalText("With a parting threat, the user lowers the target's Attack and Sp. Atk stats. Then it warps away.");
                skill.BaseCharges = 14;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, -1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, -1));
                skill.Data.AfterActions.Add(0, new RandomWarpEvent(50, false));
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
            }
            else if (ii == 576)
            {
                skill.Name = new LocalText("**Topsy-Turvy");
                skill.Desc = new LocalText("All stat changes affecting the targets turn topsy-turvy and become the opposite of what they were.");
                skill.BaseCharges = 20;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 577)
            {
                skill.Name = new LocalText("-Draining Kiss");
                skill.Desc = new LocalText("The user steals the target's energy with a kiss. The user's HP is restored by the damage taken by the target.");
                skill.BaseCharges = 18;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new HPDrainEvent(1));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 578)
            {
                skill.Name = new LocalText("Crafty Shield");
                skill.Desc = new LocalText("The user protects itself and its allies from status moves with a mysterious power. This does not stop moves that do damage.");
                skill.BaseCharges = 16;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("crafty_shield", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Sound = "DUN_Aurora_Beam_2";
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Protect_Pink", 2));
            }
            else if (ii == 579)
            {
                skill.Name = new LocalText("Flower Shield");
                skill.Desc = new LocalText("The user raises the Defense stat of all Pokémon in the party with a mysterious power.");
                skill.BaseCharges = 10;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
            }
            else if (ii == 580)
            {
                skill.Name = new LocalText("Grassy Terrain");
                skill.Desc = new LocalText("The user covers the entire floor with grass. This restores the HP of all Pokémon except Flying types.");
                skill.BaseCharges = 10;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new GiveMapStatusEvent("grassy_terrain", 0, new StringKey(), typeof(ExtendWeatherState)));
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
            else if (ii == 581)
            {
                skill.Name = new LocalText("Misty Terrain");
                skill.Desc = new LocalText("The user covers the entire floor with mist. This protects Pokémon from status conditions.");
                skill.BaseCharges = 10;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new GiveMapStatusEvent("misty_terrain", 0, new StringKey(), typeof(ExtendWeatherState)));
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
            else if (ii == 582)
            {
                skill.Name = new LocalText("**Electrify");
                skill.Desc = new LocalText("The target is electrified, converting all of its moves to the Electric type.");
                skill.BaseCharges = 20;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 583)
            {
                skill.Name = new LocalText("-Play Rough");
                skill.Desc = new LocalText("The user plays rough with the target and attacks it. This may also lower the target's Attack stat.");
                skill.BaseCharges = 16;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.SkillStates.Set(new AdditionalEffectState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_attack", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 584)
            {
                skill.Name = new LocalText("-Fairy Wind");
                skill.Desc = new LocalText("The user stirs up a fairy wind and strikes the target with it.");
                skill.BaseCharges = 15;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(30));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Silver_Wind";
            }
            else if (ii == 585)
            {
                skill.Name = new LocalText("=Moonblast");
                skill.Desc = new LocalText("Borrowing the power of the moon, the user attacks the target. This may also lower the target's Sp. Atk stat.");
                skill.BaseCharges = 8;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_special_attack", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Dragon_Pulse_Ball", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                FiniteOverlayEmitter overlay = new FiniteOverlayEmitter();
                overlay.Anim = new BGAnimData("Moonlight", 3);
                overlay.TotalTime = 60;
                overlay.Layer = DrawLayer.Bottom;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = overlay;
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Sound = "DUN_Rest";
            }
            else if (ii == 586)
            {
                skill.Name = new LocalText("-Boomburst");
                skill.Desc = new LocalText("The user attacks everything around it with the destructive power of a terrible, explosive sound.");
                skill.BaseCharges = 5;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(30);//Sound
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 587)
            {
                skill.Name = new LocalText("Fairy Lock");
                skill.Desc = new LocalText("The user locks down the battlefield, causing all other Pokémon in the area to become immobilized after attacking.");
                skill.BaseCharges = 14;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("fairy_lock", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Full;
                ((AreaAction)skill.HitboxAction).Range = 3;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.HitboxAction.ActionFX.Sound = "DUN_Ice_Shard";
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Embargo_Yellow_Front", 2), new AnimData("Embargo_Yellow_Back", 2));
                skill.Data.HitFX.Emitter = emitter;
            }
            else if (ii == 588)
            {
                skill.Name = new LocalText("King's Shield");
                skill.Desc = new LocalText("The user takes a defensive stance while it protects itself from damage. It also lowers the Attack stat of any attacker.");
                skill.BaseCharges = 10;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("kings_shield", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new SelfAction();
                ((SelfAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(37);//Withdraw
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Protect_Yellow", 2), 4);
                skill.HitboxAction.TargetAlignments = Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Protect";
            }
            else if (ii == 589)
            {
                skill.Name = new LocalText("Play Nice");
                skill.Desc = new LocalText("The user and the target become friends, and the target loses its will to fight. This lowers the target's Sp. Atk stat.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, -1));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 4;
                ((ThrowAction)skill.HitboxAction).LagBehindTime = 30;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Emote_Glowing", 2));
                emitter.LocHeight = 4;
                ((ThrowAction)skill.HitboxAction).ActionFX.Emitter = emitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Tail_Whip";
                skill.Data.HitCharAction = new CharAnimFrameType(25);//Appeal
                FiniteSprinkleEmitter endEmitter = new FiniteSprinkleEmitter();
                for (int nn = 0; nn < 44; nn++)
                    endEmitter.Anims.Add(new ParticleAnim(new AnimData("Music_Notes", 30, nn, nn)));
                endEmitter.HeightSpeed = 36;
                endEmitter.SpeedDiff = 36;
                endEmitter.TotalParticles = 4;
                endEmitter.StartHeight = 12;
                endEmitter.Range = 16;
                endEmitter.Speed = 64;
                skill.Data.IntroFX.Add(new BattleFX(endEmitter, "DUN_Tail_Whip", 30));
            }
            else if (ii == 590)
            {
                skill.Name = new LocalText("Confide");
                skill.Desc = new LocalText("The user tells the target a secret, and the target loses its ability to concentrate. This lowers the target's Sp. Atk stat.");
                skill.BaseCharges = 18;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new SoundState());
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, -1));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Around;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Torment", 3));
                emitter.LocHeight = 28;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Nasty_Plot_Start";
                skill.HitboxAction.PreActions.Add(preFX);
                ((AttackAction)skill.HitboxAction).LagBehindTime = 30;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitCharAction = new CharAnimFrameType(04);//Hurt
            }
            else if (ii == 591)
            {
                skill.Name = new LocalText("**Diamond Storm");
                skill.Desc = new LocalText("The user whips up a storm of diamonds to damage opposing Pokémon. This may also sharply raise the user's Defense stat.");
                skill.BaseCharges = 5;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 95;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 592)
            {
                skill.Name = new LocalText("**Steam Eruption");
                skill.Desc = new LocalText("The user immerses the target in superheated steam. This may also leave the target with a burn.");
                skill.BaseCharges = 5;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 95;
                skill.Data.SkillStates.Set(new BasePowerState(110));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 593)
            {
                skill.Name = new LocalText("**Hyperspace Hole");
                skill.Desc = new LocalText("Using a hyperspace hole, the user appears right next to the target and strikes. This also hits a target using Protect or Detect.");
                skill.BaseCharges = 5;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 594)
            {
                skill.Name = new LocalText("**Water Shuriken");
                skill.Desc = new LocalText("The user hits the target with throwing stars three times in a row.");
                skill.BaseCharges = 1;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(20));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 3;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Flame_Wheel", 2);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 595)
            {
                skill.Name = new LocalText("-Mystical Fire");
                skill.Desc = new LocalText("The user attacks by breathing a special, hot fire. This also lowers the target's Sp. Atk stat.");
                skill.BaseCharges = 15;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_special_attack", true, true, -1)));
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Flame_Wheel", 2);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 596)
            {
                skill.Name = new LocalText("**Spiky Shield");
                skill.Desc = new LocalText("In addition to protecting the user from attacks, this move also damages any attacker who makes direct contact.");
                skill.BaseCharges = 10;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 597)
            {
                skill.Name = new LocalText("-Aromatic Mist");
                skill.Desc = new LocalText("The user sharply raises the Sp. Def stat of ally Pokémon with a mysterious aroma.");
                skill.BaseCharges = 12;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_defense", true, false, 2));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(33);//Gas
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                StreamEmitter shotAnim = new StreamEmitter(new AnimData("Smoke_White", 2));
                shotAnim.StartDistance = 8;
                shotAnim.Shots = 7;
                shotAnim.BurstTime = 6;
                skill.HitboxAction.TargetAlignments = Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Friend;
            }
            else if (ii == 598)
            {
                skill.Name = new LocalText("-Eerie Impulse");
                skill.Desc = new LocalText("The user's body generates an eerie impulse. Exposing the target to it harshly lowers the target's Sp. Atk stat.");
                skill.BaseCharges = 16;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, -2));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(35);//Emit
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 599)
            {
                skill.Name = new LocalText("-Venom Drench");
                skill.Desc = new LocalText("Opposing Pokémon are drenched in an odd poisonous liquid. This lowers their Attack, Sp. Atk, and Movement Speed.");
                skill.BaseCharges = 14;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, -1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, -1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, -1));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((AreaAction)skill.HitboxAction).HitArea = Hitbox.AreaLimit.Cone;
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 600)
            {
                skill.Name = new LocalText("Powder");
                skill.Desc = new LocalText("The user covers opponents in a powder that causes an explosion when exposed to fire or electricity.");
                skill.BaseCharges = 12;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusBattleEvent("powder", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(19);//Shake
                ((AreaAction)skill.HitboxAction).Range = 3;
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
                skill.HitboxAction.ActionFX.Sound = "_UNK_DUN_Rustle";
            }
            else if (ii == 601)
            {
                skill.Name = new LocalText("**Geomancy");
                skill.Desc = new LocalText("The user absorbs energy and sharply raises its Sp. Atk, Sp. Def, and Speed stats on the next turn.");
                skill.BaseCharges = 10;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 602)
            {
                skill.Name = new LocalText("-Magnetic Flux");
                skill.Desc = new LocalText("The user manipulates magnetic fields which raises the Defense and Sp. Def stats of ally Pokémon.");
                skill.BaseCharges = 14;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, 1));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_defense", true, false, 1));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(35);//Emit
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Friend;
            }
            else if (ii == 603)
            {
                skill.Name = new LocalText("**Happy Hour");
                skill.Desc = new LocalText("Using Happy Hour doubles the amount of prize money received after battle.");
                skill.BaseCharges = 30;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 604)
            {
                skill.Name = new LocalText("Electric Terrain");
                skill.Desc = new LocalText("The user electrifies the entire floor. Pokémon can no longer fall asleep.");
                skill.BaseCharges = 10;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new GiveMapStatusEvent("electric_terrain", 0, new StringKey(), typeof(ExtendWeatherState)));
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
            else if (ii == 605)
            {
                skill.Name = new LocalText("-Dazzling Gleam");
                skill.Desc = new LocalText("The user damages opposing Pokémon by emitting a powerful flash.");
                skill.BaseCharges = 16;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(65));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(35);//Emit
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Metal_Burst";
            }
            else if (ii == 606)
            {
                skill.Name = new LocalText("**Celebrate");
                skill.Desc = new LocalText("The Pokémon congratulates you on your special day!");
                skill.BaseCharges = 40;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 607)
            {
                skill.Name = new LocalText("**Hold Hands");
                skill.Desc = new LocalText("The user and an ally hold hands. This makes them very happy.");
                skill.BaseCharges = 40;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 608)
            {
                skill.Name = new LocalText("Baby-Doll Eyes");
                skill.Desc = new LocalText("The user stares at the target with its baby-doll eyes, which lowers its Attack stat.");
                skill.BaseCharges = 18;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, -1));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).Range = 8;
                ((ProjectileAction)skill.HitboxAction).Speed = 15;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = new SingleEmitter(new AnimData("Leer", 2));
                preFX.Sound = "DUN_Tickle";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 609)
            {
                skill.Name = new LocalText("Nuzzle");
                skill.Desc = new LocalText("The user attacks by nuzzling its electrified cheeks against the target. This also leaves the target with paralysis.");
                skill.BaseCharges = 18;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("paralyze", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Attack";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.ActionFX.Sound = "DUN_Paralyzed";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Spark", 3));
            }
            else if (ii == 610)
            {
                skill.Name = new LocalText("-Hold Back");
                skill.Desc = new LocalText("The user holds back when it attacks and the target is left with at least 1 HP.");
                skill.BaseCharges = 1;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.BeforeHits.Add(0, new AddContextStateEvent(new AttackEndure()));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(08);//Strike
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 611)
            {
                skill.Name = new LocalText("Infestation");
                skill.Desc = new LocalText("The target is infested and trapped for three turns.");
                skill.BaseCharges = 20;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(30));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new OnHitEvent(true, false, 100, new StatusBattleEvent("infestation", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.FrontAndCorners;
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Attack_Order", 2));
                ((AttackAction)skill.HitboxAction).LagBehindTime = 35;
                skill.HitboxAction.ActionFX.Sound = "DUN_Gunk_Shot_2";
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 612)
            {
                skill.Name = new LocalText("Power-Up Punch");
                skill.Desc = new LocalText("Striking opponents over and over makes the user's fists harder. Hitting a target raises the Attack stat.");
                skill.BaseCharges = 12;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new FistState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new AdditionalEndEvent(new StatusStackBattleEvent("mod_attack", false, true, 1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(11);//Punch
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Feint_2";
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Print_Fist", 12));
            }
            else if (ii == 613)
            {
                skill.Name = new LocalText("**Oblivion Wing");
                skill.Desc = new LocalText("The user absorbs its target's HP. The user's HP is restored by the damage taken by the target.");
                skill.BaseCharges = 10;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 614)
            {
                skill.Name = new LocalText("**Thousand Arrows");
                skill.Desc = new LocalText("This move also hits opposing Pokémon that are in the air. Those Pokémon are knocked down to the ground.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 615)
            {
                skill.Name = new LocalText("**Thousand Waves");
                skill.Desc = new LocalText("The user attacks with a wave that crawls along the ground. Those hit can't flee from battle.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 616)
            {
                skill.Name = new LocalText("**Land's Wrath");
                skill.Desc = new LocalText("The user gathers the energy of the land and focuses that power on opposing Pokémon to damage them.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 617)
            {
                skill.Name = new LocalText("Light of Ruin");
                skill.Desc = new LocalText("Drawing power from the Eternal Flower, the user fires a powerful beam of light. This also damages the user quite a lot.");
                skill.BaseCharges = 5;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new HPRecoilEvent(3, false));
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Wall_Break", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Rollout", terrainEmitter, new FlagType(typeof(WallTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new WaveMotionAction();
                ((WaveMotionAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((WaveMotionAction)skill.HitboxAction).Range = 8;
                ((WaveMotionAction)skill.HitboxAction).Speed = 10;
                ((WaveMotionAction)skill.HitboxAction).Linger = 6;
                ((WaveMotionAction)skill.HitboxAction).Wide = true;
                ((WaveMotionAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.ActionFX.ScreenMovement = new ScreenMover(0, 8, 30);
                ((WaveMotionAction)skill.HitboxAction).Anim = new BeamAnimData("Beam_Pink", 2);
                skill.HitboxAction.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.HitboxAction.ActionFX.Sound = "DUN_Lunar_Dance_2";
            }
            else if (ii == 618)
            {
                skill.Name = new LocalText("**Origin Pulse");
                skill.Desc = new LocalText("The user attacks opposing Pokémon with countless beams of light that glow a deep and brilliant blue.");
                skill.BaseCharges = 10;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(110));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 619)
            {
                skill.Name = new LocalText("**Precipice Blades");
                skill.Desc = new LocalText("The user attacks opposing Pokémon by manifesting the power of the land in fearsome blades of stone.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 620)
            {
                skill.Name = new LocalText("**Dragon Ascent");
                skill.Desc = new LocalText("After soaring upward, the user attacks its target by dropping out of the sky at high speeds, although it lowers its own Defense and Sp. Def in the process.");
                skill.BaseCharges = 5;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new StatusStackBattleEvent("mod_defense", false, true, -1));
                skill.Data.AfterActions.Add(0, new StatusStackBattleEvent("mod_special_defense", false, true, -1));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 621)
            {
                skill.Name = new LocalText("**Hyperspace Fury");
                skill.Desc = new LocalText("Using its many arms, the user unleashes a barrage of attacks that ignore the effects of moves like Protect and Detect. This attack lowers the user's Defense.");
                skill.BaseCharges = 5;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 622)
            {
                skill.Name = new LocalText("**Breakneck Blitz");
                skill.Desc = new LocalText("The user builds up its momentum using its Z-Power and crashes into the target at full speed. The power varies, depending on the original move.");
                skill.BaseCharges = 1;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 623)
            {
                skill.Name = new LocalText("**Breakneck Blitz");
                fileName = ii.ToString();
                skill.Desc = new LocalText("Dummy Data");
                skill.BaseCharges = 1;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 624)
            {
                skill.Name = new LocalText("**All-Out Pummeling");
                skill.Desc = new LocalText("The user rams an energy orb created by its Z-Power into the target with full force. The power varies, depending on the original move.");
                skill.BaseCharges = 1;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 625)
            {
                skill.Name = new LocalText("**All-Out Pummeling");
                fileName = ii.ToString();
                skill.Desc = new LocalText("Dummy Data");
                skill.BaseCharges = 1;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 626)
            {
                skill.Name = new LocalText("**Supersonic Skystrike");
                skill.Desc = new LocalText("The user soars up with its Z-Power and plummets toward the target at full speed. The power varies, depending on the original move.");
                skill.BaseCharges = 1;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 627)
            {
                skill.Name = new LocalText("**Supersonic Skystrike");
                fileName = ii.ToString();
                skill.Desc = new LocalText("Dummy Data");
                skill.BaseCharges = 1;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 628)
            {
                skill.Name = new LocalText("**Acid Downpour");
                skill.Desc = new LocalText("The user creates a poisonous swamp using its Z-Power and sinks the target into it at full force. The power varies, depending on the original move.");
                skill.BaseCharges = 1;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 629)
            {
                skill.Name = new LocalText("**Acid Downpour");
                fileName = ii.ToString();
                skill.Desc = new LocalText("Dummy Data");
                skill.BaseCharges = 1;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 630)
            {
                skill.Name = new LocalText("**Tectonic Rage");
                skill.Desc = new LocalText("The user burrows deep into the ground and slams into the target with the full force of its Z-Power. The power varies, depending on the original move.");
                skill.BaseCharges = 1;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 631)
            {
                skill.Name = new LocalText("**Tectonic Rage");
                fileName = ii.ToString();
                skill.Desc = new LocalText("Dummy Data");
                skill.BaseCharges = 1;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 632)
            {
                skill.Name = new LocalText("**Continental Crush");
                skill.Desc = new LocalText("The user summons a huge rock mountain using its Z-Power and drops it onto the target with full force. The power varies, depending on the original move.");
                skill.BaseCharges = 1;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 633)
            {
                skill.Name = new LocalText("**Continental Crush");
                fileName = ii.ToString();
                skill.Desc = new LocalText("Dummy Data");
                skill.BaseCharges = 1;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 634)
            {
                skill.Name = new LocalText("**Savage Spin-Out");
                skill.Desc = new LocalText("The user binds the target with full force with threads of silk that the user spits using its Z-Power. The power varies, depending on the original move.");
                skill.BaseCharges = 1;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 635)
            {
                skill.Name = new LocalText("**Savage Spin-Out");
                fileName = ii.ToString();
                skill.Desc = new LocalText("Dummy Data");
                skill.BaseCharges = 1;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 636)
            {
                skill.Name = new LocalText("**Never-Ending Nightmare");
                skill.Desc = new LocalText("Deep-seated grudges summoned by the user's Z-Power trap the target. The power varies, depending on the original move.");
                skill.BaseCharges = 1;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 637)
            {
                skill.Name = new LocalText("**Never-Ending Nightmare");
                fileName = ii.ToString();
                skill.Desc = new LocalText("Dummy Data");
                skill.BaseCharges = 1;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 638)
            {
                skill.Name = new LocalText("**Corkscrew Crash");
                skill.Desc = new LocalText("The user spins very fast and rams into the target with the full force of its Z-Power. The power varies, depending on the original move.");
                skill.BaseCharges = 1;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 639)
            {
                skill.Name = new LocalText("**Corkscrew Crash");
                fileName = ii.ToString();
                skill.Desc = new LocalText("Dummy Data");
                skill.BaseCharges = 1;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 640)
            {
                skill.Name = new LocalText("**Inferno Overdrive");
                skill.Desc = new LocalText("The user breathes a stream of intense fire toward the target with the full force of its Z-Power. The power varies depending on the original move.");
                skill.BaseCharges = 1;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 641)
            {
                skill.Name = new LocalText("**Inferno Overdrive");
                fileName = ii.ToString();
                skill.Desc = new LocalText("Dummy Data");
                skill.BaseCharges = 1;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 642)
            {
                skill.Name = new LocalText("**Hydro Vortex");
                skill.Desc = new LocalText("The user creates a huge whirling current using its Z-Power to swallow the target with full force. The power varies, depending on the original move.");
                skill.BaseCharges = 1;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 643)
            {
                skill.Name = new LocalText("**Hydro Vortex");
                fileName = ii.ToString();
                skill.Desc = new LocalText("Dummy Data");
                skill.BaseCharges = 1;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 644)
            {
                skill.Name = new LocalText("**Bloom Doom");
                skill.Desc = new LocalText("The user collects energy from plants using its Z-Power and attacks the target with full force. The power varies, depending on the original move.");
                skill.BaseCharges = 1;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 645)
            {
                skill.Name = new LocalText("**Bloom Doom");
                fileName = ii.ToString();
                skill.Desc = new LocalText("Dummy Data");
                skill.BaseCharges = 1;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 646)
            {
                skill.Name = new LocalText("**Gigavolt Havoc");
                skill.Desc = new LocalText("The user hits the target with a powerful electric current collected by its Z-Power. The power varies, depending on the original move.");
                skill.BaseCharges = 1;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 647)
            {
                skill.Name = new LocalText("**Gigavolt Havoc");
                fileName = ii.ToString();
                skill.Desc = new LocalText("Dummy Data");
                skill.BaseCharges = 1;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 648)
            {
                skill.Name = new LocalText("**Shattered Psyche");
                skill.Desc = new LocalText("The user controls the target with its Z-Power and hurts the target with full force. The power varies, depending on the original move.");
                skill.BaseCharges = 1;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 649)
            {
                skill.Name = new LocalText("**Shattered Psyche");
                fileName = ii.ToString();
                skill.Desc = new LocalText("Dummy Data");
                skill.BaseCharges = 1;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 650)
            {
                skill.Name = new LocalText("**Subzero Slammer");
                skill.Desc = new LocalText("The user dramatically drops the temperature using its Z-Power and freezes the target with full force. The power varies, depending on the original move.");
                skill.BaseCharges = 1;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 651)
            {
                skill.Name = new LocalText("**Subzero Slammer");
                fileName = ii.ToString();
                skill.Desc = new LocalText("Dummy Data");
                skill.BaseCharges = 1;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 652)
            {
                skill.Name = new LocalText("**Devastating Drake");
                skill.Desc = new LocalText("The user materializes its aura using its Z-Power and attacks the target with full force. The power varies, depending on the original move.");
                skill.BaseCharges = 1;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 653)
            {
                skill.Name = new LocalText("**Devastating Drake");
                fileName = ii.ToString();
                skill.Desc = new LocalText("Dummy Data");
                skill.BaseCharges = 1;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 654)
            {
                skill.Name = new LocalText("**Black Hole Eclipse");
                skill.Desc = new LocalText("The user gathers dark energy using its Z-Power and sucks the target into it. The power varies, depending on the original move.");
                skill.BaseCharges = 1;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 655)
            {
                skill.Name = new LocalText("**Black Hole Eclipse");
                fileName = ii.ToString();
                skill.Desc = new LocalText("Dummy Data");
                skill.BaseCharges = 1;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 656)
            {
                skill.Name = new LocalText("**Twinkle Tackle");
                skill.Desc = new LocalText("The user creates a very charming space using its Z-Power and totally toys with the target. The power varies, depending on the original move.");
                skill.BaseCharges = 1;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 657)
            {
                skill.Name = new LocalText("**Twinkle Tackle");
                fileName = ii.ToString();
                skill.Desc = new LocalText("Dummy Data");
                skill.BaseCharges = 1;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 658)
            {
                skill.Name = new LocalText("**Catastropika");
                skill.Desc = new LocalText("The user, Pikachu, surrounds itself with the maximum amount of electricity using its Z-Power and pounces on its target with full force.");
                skill.BaseCharges = 1;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(210));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 659)
            {
                skill.Name = new LocalText("**Shore Up");
                skill.Desc = new LocalText("The user regains up to half of its max HP. It restores more HP in a sandstorm.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 660)
            {
                skill.Name = new LocalText("**First Impression");
                skill.Desc = new LocalText("Although this move has great power, it only works the first turn the user is in battle.");
                skill.BaseCharges = 10;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 661)
            {
                skill.Name = new LocalText("**Baneful Bunker");
                skill.Desc = new LocalText("In addition to protecting the user from attacks, this move also poisons any attacker that makes direct contact.");
                skill.BaseCharges = 10;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 662)
            {
                skill.Name = new LocalText("-Spirit Shackle");
                skill.Desc = new LocalText("The user attacks while simultaneously stitching the target's shadow to the ground to prevent the target from escaping.");
                skill.BaseCharges = 13;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("immobilized", true, false)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Wave_Circle_Blue", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.HitboxAction.ActionFX.Sound = "_UNK_DUN_Water_Echo_2";
            }
            else if (ii == 663)
            {
                skill.Name = new LocalText("**Darkest Lariat");
                skill.Desc = new LocalText("The user swings both arms and hits the target. The target's stat changes don't affect this attack's damage.");
                skill.BaseCharges = 10;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(85));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 664)
            {
                skill.Name = new LocalText("**Sparkling Aria");
                skill.Desc = new LocalText("The user bursts into song, emitting many bubbles. Any Pokémon suffering from a burn will be healed by the touch of these bubbles.");
                skill.BaseCharges = 10;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 665)
            {
                skill.Name = new LocalText("**Ice Hammer");
                skill.Desc = new LocalText("The user swings and hits with its strong, heavy fist. It lowers the user's Speed, however.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 666)
            {
                skill.Name = new LocalText("Floral Healing");
                skill.Desc = new LocalText("The user restores the party's HP. It restores more HP when the terrain is grass.");
                skill.BaseCharges = 13;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = -1;
                Dictionary<string, bool> weather = new Dictionary<string, bool>();
                weather.Add("grassy_terrain", true);
                skill.Data.OnHits.Add(0, new WeatherHPEvent(3, weather));
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((OffsetAction)skill.HitboxAction).HitArea = OffsetAction.OffsetArea.Area;
                ((OffsetAction)skill.HitboxAction).Range = 1;
                ((OffsetAction)skill.HitboxAction).Speed = 6;
                ((OffsetAction)skill.HitboxAction).LagBehindTime = 5;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                BattleFX preFX = new BattleFX();
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Grass_Knot_Grass_Back", 1), new AnimData("Grass_Knot_Grass_Front", 1));
                emitter.HeightBack = 8;
                emitter.HeightFront = 8;
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Harden";
                skill.HitboxAction.PreActions.Add(preFX);
                CircleSquareAreaEmitter flowerEmitter = new CircleSquareAreaEmitter(new AnimData("Petal_Dance_Flower_Pink", 60, 0, 0), new AnimData("Petal_Dance_Flower_Yellow", 60, 0, 0));
                flowerEmitter.ParticlesPerTile = 3;

                CircleSquareSprinkleEmitter sparkleEmitter = new CircleSquareSprinkleEmitter(new AnimData("Natural_Gift_Sparkle", 5));
                sparkleEmitter.ParticlesPerTile = 2;
                sparkleEmitter.StartHeight = 0;
                sparkleEmitter.HeightSpeed = 20;
                sparkleEmitter.SpeedDiff = 15;
                MultiCircleSquareEmitter multiEmitter = new MultiCircleSquareEmitter();
                multiEmitter.Emitters.Add(flowerEmitter);
                multiEmitter.Emitters.Add(sparkleEmitter);
                ((OffsetAction)skill.HitboxAction).Emitter = multiEmitter;
                skill.HitboxAction.TargetAlignments = Alignment.Friend | Alignment.Self;
                skill.Explosion.TargetAlignments = Alignment.Friend | Alignment.Self;
                skill.HitboxAction.ActionFX.Sound = "DUN_Aromatherapy";
            }
            else if (ii == 667)
            {
                skill.Name = new LocalText("**High Horsepower");
                skill.Desc = new LocalText("The user fiercely attacks the target using its entire body.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 95;
                skill.Data.SkillStates.Set(new BasePowerState(95));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 668)
            {
                skill.Name = new LocalText("**Strength Sap");
                skill.Desc = new LocalText("The user restores its HP by the same amount as the target's Attack stat. It also lowers the target's Attack stat.");
                skill.BaseCharges = 10;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = 100;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 669)
            {
                skill.Name = new LocalText("**Solar Blade");
                skill.Desc = new LocalText("In this two-turn attack, the user gathers light and fills a blade with the light's energy, attacking the target on the next turn.");
                skill.BaseCharges = 10;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(125));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 670)
            {
                skill.Name = new LocalText("=Leafage");
                skill.Desc = new LocalText("The user attacks by pelting the target with leaves.");
                skill.BaseCharges = 40;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 2;
                ((ThrowAction)skill.HitboxAction).Speed = 10;
                ((ThrowAction)skill.HitboxAction).Anim = new AnimData("Gastro_Acid_Ball", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Razor_Leaf_Charge", 2));
                endAnim.LocHeight = 20;
                skill.Data.HitFX.Emitter = endAnim;
            }
            else if (ii == 671)
            {
                skill.Name = new LocalText("**Spotlight");
                skill.Desc = new LocalText("The user shines a spotlight on the target so that only the target will be attacked during the turn.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 672)
            {
                skill.Name = new LocalText("**Toxic Thread");
                skill.Desc = new LocalText("The user shoots poisonous threads to poison the target and lower the target's Speed stat.");
                skill.BaseCharges = 20;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 673)
            {
                skill.Name = new LocalText("**Laser Focus");
                skill.Desc = new LocalText("The user concentrates intensely. The attack on the next turn always results in a critical hit.");
                skill.BaseCharges = 30;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 674)
            {
                skill.Name = new LocalText("**Gear Up");
                skill.Desc = new LocalText("The user engages its gears to raise the Attack and Sp. Atk stats of ally Pokémon with the Plus or Minus Ability.");
                skill.BaseCharges = 20;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 675)
            {
                skill.Name = new LocalText("**Throat Chop");
                skill.Desc = new LocalText("The user attacks the target's throat, and the resultant suffering prevents the target from using moves that emit sound for two turns.");
                skill.BaseCharges = 15;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 676)
            {
                skill.Name = new LocalText("-Pollen Puff");
                skill.Desc = new LocalText("The user attacks the enemy with a pollen puff that explodes. If the target is an ally, it gives the ally a pollen puff that restores its HP instead.");
                skill.BaseCharges = 13;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
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
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Pound";
            }
            else if (ii == 677)
            {
                skill.Name = new LocalText("-Anchor Shot");
                skill.Desc = new LocalText("The user entangles the target with its anchor chain while attacking. The target becomes unable to flee.");
                skill.BaseCharges = 16;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("immobilized", true, false)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 4;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Arrow_Shot", 2);
                skill.HitboxAction.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.HitboxAction.ActionFX.Sound = "_UNK_DUN_Shatter_2";
                BetweenEmitter endAnim = new BetweenEmitter(new AnimData("Wrap_White_Back", 3), new AnimData("Wrap_White_Front", 3));
                endAnim.HeightBack = 16;
                endAnim.HeightFront = 16;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "_UNK_DUN_Clank";
            }
            else if (ii == 678)
            {
                skill.Name = new LocalText("Psychic Terrain");
                skill.Desc = new LocalText("This protects Pokémon on the ground from priority moves and powers up Psychic-type moves for five turns.");
                skill.BaseCharges = 10;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new GiveMapStatusEvent("psychic_terrain", 0, new StringKey(), typeof(ExtendWeatherState)));
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
            else if (ii == 679)
            {
                skill.Name = new LocalText("Lunge");
                skill.Desc = new LocalText("The user makes a lunge at the target, attacking with full force. This also lowers the target's Attack stat.");
                skill.BaseCharges = 14;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_attack", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.ActionFX.Sound = "DUN_Thief";
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Power_Whip_Hit_Front", 3));
            }
            else if (ii == 680)
            {
                skill.Name = new LocalText("**Fire Lash");
                skill.Desc = new LocalText("The user strikes the target with a burning lash. This also lowers the target's Defense stat.");
                skill.BaseCharges = 15;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 681)
            {
                skill.Name = new LocalText("**Power Trip");
                skill.Desc = new LocalText("The user boasts its strength and attacks the target. The more the user's stats are raised, the greater the move's power.");
                skill.BaseCharges = 10;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(20));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 682)
            {
                skill.Name = new LocalText("**Burn Up");
                skill.Desc = new LocalText("To inflict massive damage, the user burns itself out. After using this move, the user will no longer be Fire type.");
                skill.BaseCharges = 5;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(130));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 683)
            {
                skill.Name = new LocalText("Speed Swap");
                skill.Desc = new LocalText("The user exchanges Speed stats with the target.");
                skill.BaseCharges = 20;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new SpeedSwapEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 0;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                skill.HitboxAction.PreActions.Add(new BattleFX(new SingleEmitter(new AnimData("Circle_Small_Green_In", 2)), "DUN_Growth_2", 0));
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);

                SwingSwitchEmitter emitter = new SwingSwitchEmitter(new AnimData("Skill_Swap_RSE", 3));
                emitter.AxisRatio = 0.5f;
                emitter.Amount = 1;
                emitter.RotationTime = 20;
                skill.Data.IntroFX.Add(new BattleFX(emitter, "", 20));
                SingleEmitter destAnim = new SingleEmitter(new AnimData("Circle_Small_Green_Out", 2));
                destAnim.UseDest = true;
                skill.Data.IntroFX.Add(new BattleFX(destAnim, "", 0));
                skill.Data.IntroFX.Add(new BattleFX(new SingleEmitter(new AnimData("Circle_Small_Green_Out", 2)), "DUN_Growth", 0));
            }
            else if (ii == 684)
            {
                skill.Name = new LocalText("**Smart Strike");
                skill.Desc = new LocalText("The user stabs the target with a sharp horn. This attack never misses.");
                skill.BaseCharges = 10;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 685)
            {
                skill.Name = new LocalText("**Purify");
                skill.Desc = new LocalText("The user heals the target's status condition. If the move succeeds, it also restores the user's own HP.");
                skill.BaseCharges = 20;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.SkillStates.Set(new HealState());
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 686)
            {
                skill.Name = new LocalText("**Revelation Dance");
                skill.Desc = new LocalText("The user attacks the target by dancing very hard. The user's type determines the type of this move.");
                skill.BaseCharges = 15;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 687)
            {
                skill.Name = new LocalText("**Core Enforcer");
                skill.Desc = new LocalText("If the Pokémon the user has inflicted damage on have already used their moves, this move eliminates the effect of the target's Ability.");
                skill.BaseCharges = 10;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 688)
            {
                skill.Name = new LocalText("**Trop Kick");
                skill.Desc = new LocalText("The user lands an intense kick of tropical origins on the target. This also lowers the target's Attack stat.");
                skill.BaseCharges = 15;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 689)
            {
                skill.Name = new LocalText("**Instruct");
                skill.Desc = new LocalText("The user instructs the target to use the target's last move again.");
                skill.BaseCharges = 15;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 690)
            {
                skill.Name = new LocalText("**Beak Blast");
                skill.Desc = new LocalText("The user first heats up its beak, and then it attacks the target. Making direct contact with the Pokémon while it's heating up its beak results in a burn.");
                skill.BaseCharges = 15;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 691)
            {
                skill.Name = new LocalText("**Clanging Scales");
                skill.Desc = new LocalText("The user rubs the scales on its entire body and makes a huge noise to attack the opposing Pokémon. The user's Defense stat goes down after the attack.");
                skill.BaseCharges = 5;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(110));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 692)
            {
                skill.Name = new LocalText("**Dragon Hammer");
                skill.Desc = new LocalText("The user uses its body like a hammer to attack the target and inflict damage.");
                skill.BaseCharges = 15;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 693)
            {
                skill.Name = new LocalText("Brutal Swing");
                skill.Desc = new LocalText("The user swings its body around violently to inflict damage on everything in its vicinity.");
                skill.BaseCharges = 17;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Around;
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Cut_Dark", 2));
                skill.HitboxAction.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.HitboxAction.ActionFX.Sound = "DUN_Cut";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Shadow_Sneak_Hit", 2));
            }
            else if (ii == 694)
            {
                skill.Name = new LocalText("Aurora Veil");
                skill.Desc = new LocalText("This move reduces damage from physical and special moves for five turns. This can be used only in a hailstorm.");
                skill.BaseCharges = 15;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnActions.Add(-1, new WeatherRequiredEvent(new StringKey("MSG_NOT_HAILING"), "hail", "snow"));
                skill.Data.OnHits.Add(0, new StatusBattleEvent("light_screen", true, false));
                skill.Data.OnHits.Add(0, new StatusBattleEvent("reflect", true, false));
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                ((AreaAction)skill.HitboxAction).Range = 1;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter();
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Blue", 3, -1, -1, 192)));
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Green", 3, -1, -1, 192)));
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Purple", 3, -1, -1, 192)));
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Red", 3, -1, -1, 192)));
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Yellow", 3, -1, -1, 192)));
                emitter.ParticlesPerTile = 0.7;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.HitboxAction.ActionFX.Sound = "DUN_Light_Screen_2";
                skill.HitboxAction.ActionFX.Emitter = new SingleEmitter(new AnimData("Screen_RSE_Yellow", 3, -1, -1, 192));
            }
            else if (ii == 695)
            {
                skill.Name = new LocalText("**Sinister Arrow Raid");
                skill.Desc = new LocalText("The user, Decidueye, creates countless arrows using its Z-Power and shoots the target with full force.");
                skill.BaseCharges = 1;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(180));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 696)
            {
                skill.Name = new LocalText("**Malicious Moonsault");
                skill.Desc = new LocalText("The user, Incineroar, strengthens its body using its Z-Power and crashes into the target with full force.");
                skill.BaseCharges = 1;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(180));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 697)
            {
                skill.Name = new LocalText("**Oceanic Operetta");
                skill.Desc = new LocalText("The user, Primarina, summons a massive amount of water using its Z-Power and attacks the target with full force.");
                skill.BaseCharges = 1;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(195));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 698)
            {
                skill.Name = new LocalText("**Guardian of Alola");
                skill.Desc = new LocalText("The user, the Land Spirit Pokémon, obtains Alola's energy using its Z-Power and attacks the target with full force. This reduces the target's HP greatly.");
                skill.BaseCharges = 1;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 699)
            {
                skill.Name = new LocalText("**Soul-Stealing 7-Star Strike");
                skill.Desc = new LocalText("After obtaining Z-Power, the user, Marshadow, punches and kicks the target consecutively with full force.");
                skill.BaseCharges = 1;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(195));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 700)
            {
                skill.Name = new LocalText("**Stoked Sparksurfer");
                skill.Desc = new LocalText("After obtaining Z-Power, the user, Alolan Raichu, attacks the target with full force. This move leaves the target with paralysis.");
                skill.BaseCharges = 1;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(175));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 701)
            {
                skill.Name = new LocalText("**Pulverizing Pancake");
                skill.Desc = new LocalText("Z-Power brings out the true capabilities of the user, Snorlax. The Pokémon moves its enormous body energetically and attacks the target with full force.");
                skill.BaseCharges = 1;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(210));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 702)
            {
                skill.Name = new LocalText("**Extreme Evoboost");
                skill.Desc = new LocalText("After obtaining Z-Power, the user, Eevee, gets energy from its evolved friends and boosts its stats sharply.");
                skill.BaseCharges = 1;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 703)
            {
                skill.Name = new LocalText("**Genesis Supernova");
                skill.Desc = new LocalText("After obtaining Z-Power, the user, Mew, attacks the target with full force. The terrain will be charged with psychic energy.");
                skill.BaseCharges = 1;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(185));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 704)
            {
                skill.Name = new LocalText("**Shell Trap");
                skill.Desc = new LocalText("The user sets a shell trap. If the user is hit by a physical move, the trap will explode and inflict damage on the opposing Pokémon.");
                skill.BaseCharges = 5;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(150));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 705)
            {
                skill.Name = new LocalText("**Fleur Cannon");
                skill.Desc = new LocalText("The user unleashes a strong beam. The attack's recoil harshly lowers the user's Sp. Atk stat.");
                skill.BaseCharges = 5;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(130));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 706)
            {
                skill.Name = new LocalText("**Psychic Fangs");
                skill.Desc = new LocalText("The user bites the target with its psychic capabilities. This can also destroy Light Screen and Reflect.");
                skill.BaseCharges = 10;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new JawState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(85));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 707)
            {
                skill.Name = new LocalText("**Stomping Tantrum");
                skill.Desc = new LocalText("Driven by frustration, the user attacks the target. If the user's previous move has failed, the power of this move doubles.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 708)
            {
                skill.Name = new LocalText("Shadow Bone");
                skill.Desc = new LocalText("The user attacks by beating the target with a bone that contains a spirit. This may also lower the target's Defense stat.");
                skill.BaseCharges = 15;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(85));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_defense", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(08);//Strike
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Attack";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Thief_Hit_Dark", 2));
                skill.Data.HitFX.Sound = "DUN_Punch";
            }
            else if (ii == 709)
            {
                skill.Name = new LocalText("Accelerock");
                skill.Desc = new LocalText("The user smashes into the target at high speed. This move always goes first.");
                skill.BaseCharges = 20;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = false;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Rock_Wrecker";
                skill.HitboxAction.PreActions.Add(preFX);
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Rock_Smash", 2));
            }
            else if (ii == 710)
            {
                skill.Name = new LocalText("**Liquidation");
                skill.Desc = new LocalText("The user slams into the target using a full-force blast of water. This may also lower the target's Defense stat.");
                skill.BaseCharges = 10;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(85));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 711)
            {
                skill.Name = new LocalText("Prismatic Laser");
                skill.Desc = new LocalText("The user shoots powerful lasers using the power of a prism. The user can't move on the next turn.");
                skill.BaseCharges = 8;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(160));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new StatusBattleEvent("paused", false, true));
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
            else if (ii == 712)
            {
                skill.Name = new LocalText("**Spectral Thief");
                skill.Desc = new LocalText("The user hides in the target's shadow, steals the target's stat boosts, and then attacks.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 713)
            {
                skill.Name = new LocalText("**Sunsteel Strike");
                skill.Desc = new LocalText("The user slams into the target with the force of a meteor. This move can be used on the target regardless of its Abilities.");
                skill.BaseCharges = 5;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 714)
            {
                skill.Name = new LocalText("**Moongeist Beam");
                skill.Desc = new LocalText("The user emits a sinister ray to attack the target. This move can be used on the target regardless of its Abilities.");
                skill.BaseCharges = 5;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 715)
            {
                skill.Name = new LocalText("**Tearful Look");
                skill.Desc = new LocalText("The user gets teary eyed to make the target lose its combative spirit. This lowers the target's Attack and Sp. Atk stats.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 716)
            {
                skill.Name = new LocalText("-Zing Zap");
                skill.Desc = new LocalText("A strong electric blast crashes down on the target, giving it an electric shock. This may also make the target flinch.");
                skill.BaseCharges = 15;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
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
            else if (ii == 717)
            {
                skill.Name = new LocalText("**Nature's Madness");
                skill.Desc = new LocalText("The user hits the target with the force of nature. It halves the target's HP.");
                skill.BaseCharges = 10;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 718)
            {
                skill.Name = new LocalText("**Multi-Attack");
                skill.Desc = new LocalText("Cloaking itself in high energy, the user slams into the target. The move's type changes depending on the form the user is in.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 719)
            {
                skill.Name = new LocalText("**10,000,000 Volt Thunderbolt");
                fileName = "ten_million_volt_thunderbolt";
                skill.Desc = new LocalText("The user, Pikachu wearing a cap, powers up a jolt of electricity using its Z-Power and unleashes it. Critical hits land more easily.");
                skill.BaseCharges = 1;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(195));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 720)
            {
                skill.Name = new LocalText("**Mind Blown");
                skill.Desc = new LocalText("The user attacks everything around it by causing its own head to explode. This also damages the user.");
                skill.BaseCharges = 5;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(150));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 721)
            {
                skill.Name = new LocalText("**Plasma Fists");
                skill.Desc = new LocalText("The user attacks with electrically charged fists. This move changes Normal-type moves to Electric-type moves.");
                skill.BaseCharges = 15;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 722)
            {
                skill.Name = new LocalText("**Photon Geyser");
                skill.Desc = new LocalText("The user attacks a target with a pillar of light. This move inflicts Attack or Sp. Atk damage—whichever stat is higher for the user.");
                skill.BaseCharges = 5;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 723)
            {
                skill.Name = new LocalText("**Light That Burns the Sky");
                skill.Desc = new LocalText("This attack inflicts Attack or Sp. Atk damage— whichever stat is higher for the user, Necrozma. This move ignores the target's Ability.");
                skill.BaseCharges = 1;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(200));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 724)
            {
                skill.Name = new LocalText("**Searing Sunraze Smash");
                skill.Desc = new LocalText("After obtaining Z-Power, the user, Solgaleo, attacks the target with full force. This move can ignore the effect of the target's Ability.");
                skill.BaseCharges = 1;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(200));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 725)
            {
                skill.Name = new LocalText("**Menacing Moonraze Maelstrom");
                skill.Desc = new LocalText("After obtaining Z-Power, the user, Lunala, attacks the target with full force. This move can ignore the effect of the target's Ability.");
                skill.BaseCharges = 1;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(200));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 726)
            {
                skill.Name = new LocalText("**Let's Snuggle Forever");
                skill.Desc = new LocalText("After obtaining Z-Power, the user, Mimikyu, punches the target with full force.");
                skill.BaseCharges = 1;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(190));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 727)
            {
                skill.Name = new LocalText("**Splintered Stormshards");
                skill.Desc = new LocalText("After obtaining Z-Power, the user, Lycanroc, attacks the target with full force. This move negates the effect on the battlefield.");
                skill.BaseCharges = 1;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(190));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 728)
            {
                skill.Name = new LocalText("**Clangorous Soulblaze");
                skill.Desc = new LocalText("After obtaining Z-Power, the user, Kommo-o, attacks the opposing Pokémon with full force. This move boosts the user's stats.");
                skill.BaseCharges = 1;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(185));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 729)
            {
                skill.Name = new LocalText("**Zippy Zap");
                skill.Desc = new LocalText("The user attacks the target with bursts of electricity at high speed. This move always goes first and results in a critical hit.");
                skill.BaseCharges = 10;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 730)
            {
                skill.Name = new LocalText("**Splishy Splash");
                skill.Desc = new LocalText("The user charges a huge wave with electricity and hits the opposing Pokémon with the wave. This may also leave the opposing Pokémon with paralysis.");
                skill.BaseCharges = 15;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 731)
            {
                skill.Name = new LocalText("**Floaty Fall");
                skill.Desc = new LocalText("The user floats in the air, and then dives at a steep angle to attack the target. This may also make the target flinch.");
                skill.BaseCharges = 15;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 95;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 732)
            {
                skill.Name = new LocalText("**Pika Papow");
                skill.Desc = new LocalText("The more Pikachu loves its Trainer, the greater the move’s power. It never misses.");
                skill.BaseCharges = 20;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 733)
            {
                skill.Name = new LocalText("**Bouncy Bubble");
                skill.Desc = new LocalText("The user attacks by shooting water bubbles at the target. It then absorbs water and restores its HP by half the damage taken by the target.");
                skill.BaseCharges = 20;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 734)
            {
                skill.Name = new LocalText("**Buzzy Buzz");
                skill.Desc = new LocalText("The user shoots a jolt of electricity to attack the target. This also leaves the target with paralysis.");
                skill.BaseCharges = 20;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 735)
            {
                skill.Name = new LocalText("**Sizzly Slide");
                skill.Desc = new LocalText("The user cloaks itself in fire and charges at the target. This also leaves the target with a burn.");
                skill.BaseCharges = 20;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 736)
            {
                skill.Name = new LocalText("**Glitzy Glow");
                skill.Desc = new LocalText("The user bombards the target with telekinetic force. A wondrous wall of light is put up to weaken the power of the opposing Pokémon’s special moves.");
                skill.BaseCharges = 15;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 95;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 737)
            {
                skill.Name = new LocalText("**Baddy Bad");
                skill.Desc = new LocalText("The user acts bad and attacks the target. A wondrous wall of light is put up to weaken the power of the opposing Pokémon’s physical moves.");
                skill.BaseCharges = 15;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 95;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 738)
            {
                skill.Name = new LocalText("**Sappy Seed");
                skill.Desc = new LocalText("The user grows a gigantic stalk that scatters seeds to attack the target. The seeds drain the target’s HP every turn.");
                skill.BaseCharges = 10;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 739)
            {
                skill.Name = new LocalText("**Freezy Frost");
                skill.Desc = new LocalText("The user attacks with a crystal made of cold frozen haze. It eliminates every stat change among all the Pokémon engaged in battle.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 740)
            {
                skill.Name = new LocalText("**Sparkly Swirl");
                skill.Desc = new LocalText("The user attacks the target by wrapping it with a whirlwind of an overpowering scent. This also heals all status conditions of the user’s party.");
                skill.BaseCharges = 5;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 741)
            {
                skill.Name = new LocalText("**Veevee Volley");
                skill.Desc = new LocalText("The more Eevee loves its Trainer, the greater the move’s power. It never misses.");
                skill.BaseCharges = 20;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(0));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 742)
            {
                skill.Name = new LocalText("**Double Iron Bash");
                skill.Desc = new LocalText("The user rotates, centering the hex nut in its chest, and then strikes with its arms twice in a row. This may also make the target flinch.");
                skill.BaseCharges = 5;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 743)
            {
                skill.Name = new LocalText("**Max Guard");
                skill.Desc = new LocalText("This move enables the user to protect itself from all attacks. Its chance of failing rises if it is used in succession.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 744)
            {
                skill.Name = new LocalText("**Dynamax Cannon");
                skill.Desc = new LocalText("The user unleashes a strong beam from its core. This move deals twice the damage if the target is Dynamaxed.");
                skill.BaseCharges = 5;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 745)
            {
                skill.Name = new LocalText("**Snipe Shot");
                skill.Desc = new LocalText("The user ignores the effects of opposing Pokémon’s moves and Abilities that draw in moves, allowing this move to hit the chosen target.");
                skill.BaseCharges = 15;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 746)
            {
                skill.Name = new LocalText("**Jaw Lock");
                skill.Desc = new LocalText("This move prevents the user and the target from switching out until either of them faints. The effect goes away if either of the Pokémon leaves the field.");
                skill.BaseCharges = 10;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new JawState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 747)
            {
                skill.Name = new LocalText("**Stuff Cheeks");
                skill.Desc = new LocalText("The user eats its held Berry, then sharply raises its Defense stat.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 748)
            {
                skill.Name = new LocalText("**No Retreat");
                skill.Desc = new LocalText("This move raises all the user’s stats but prevents the user from switching out or fleeing.");
                skill.BaseCharges = 5;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 749)
            {
                skill.Name = new LocalText("**Tar Shot");
                skill.Desc = new LocalText("The user pours sticky tar over the target, lowering the target’s Speed stat. The target becomes weaker to Fire-type moves.");
                skill.BaseCharges = 15;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 750)
            {
                skill.Name = new LocalText("**Magic Powder");
                skill.Desc = new LocalText("The user scatters a cloud of magic powder that changes the target to Psychic type.");
                skill.BaseCharges = 20;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 751)
            {
                skill.Name = new LocalText("**Dragon Darts");
                skill.Desc = new LocalText("The user attacks twice using Dreepy. If there are two targets, this move hits each target once.");
                skill.BaseCharges = 10;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 752)
            {
                skill.Name = new LocalText("=Teatime");
                skill.Desc = new LocalText("The user has teatime with all the Pokémon in the battle. Each Pokémon eats its held Berry.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(38);//RearUp
                ((AreaAction)skill.HitboxAction).Range = 2;
                ((AreaAction)skill.HitboxAction).Speed = 6;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Beat_Up", 3));
                emitter.LocHeight = 24;
                BattleFX preFX = new BattleFX();
                preFX.Emitter = emitter;
                preFX.Sound = "DUN_Rollcall_Orb";
                preFX.Delay = 20;
                skill.HitboxAction.PreActions.Add(preFX);
                skill.HitboxAction.TargetAlignments = Alignment.Self | Alignment.Friend | Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Self | Alignment.Friend | Alignment.Foe;
            }
            else if (ii == 753)
            {
                skill.Name = new LocalText("**Octolock");
                skill.Desc = new LocalText("The user locks the target in and prevents it from fleeing. This move also lowers the target’s Defense and Sp. Def every turn.");
                skill.BaseCharges = 15;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 754)
            {
                skill.Name = new LocalText("**Bolt Beak");
                skill.Desc = new LocalText("The user stabs the target with its electrified beak. If the user attacks before the target, the power of this move is doubled.");
                skill.BaseCharges = 10;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(85));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 755)
            {
                skill.Name = new LocalText("**Fishious Rend");
                skill.Desc = new LocalText("The user rends the target with its hard gills. If the user attacks before the target, the power of this move is doubled.");
                skill.BaseCharges = 10;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new JawState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(85));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 756)
            {
                skill.Name = new LocalText("-Court Change");
                skill.Desc = new LocalText("The user successively switches its position with the positions of other Pokémon.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new SwitcherEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((AreaAction)skill.HitboxAction).Range = 5;
                ((AreaAction)skill.HitboxAction).Speed = 10;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "_UNK_DUN_Flash";
            }
            else if (ii == 757)
            {
                skill.Name = new LocalText("**Max Flare");
                skill.Desc = new LocalText("This is a Fire-type attack Dynamax Pokémon use. The user intensifies the sun for five turns.");
                skill.BaseCharges = 10;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 758)
            {
                skill.Name = new LocalText("**Max Flutterby");
                skill.Desc = new LocalText("This is a Bug-type attack Dynamax Pokémon use. This lowers the target’s Sp. Atk stat.");
                skill.BaseCharges = 10;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(10));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 759)
            {
                skill.Name = new LocalText("**Max Lightning");
                skill.Desc = new LocalText("This is an Electric-type attack Dynamax Pokémon use. The user turns the ground into Electric Terrain for five turns.");
                skill.BaseCharges = 10;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(10));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 760)
            {
                skill.Name = new LocalText("**Max Strike");
                skill.Desc = new LocalText("This is a Normal-type attack Dynamax Pokémon use. This lowers the target’s Speed stat.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(10));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 761)
            {
                skill.Name = new LocalText("**Max Knuckle");
                skill.Desc = new LocalText("This is a Fighting-type attack Dynamax Pokémon use. This raises ally Pokémon’s Attack stats.");
                skill.BaseCharges = 10;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(10));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 762)
            {
                skill.Name = new LocalText("**Max Phantasm");
                skill.Desc = new LocalText("This is a Ghost-type attack Dynamax Pokémon use. This lowers the target’s Defense stat.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(10));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 763)
            {
                skill.Name = new LocalText("**Max Hailstorm");
                skill.Desc = new LocalText("This is an Ice-type attack Dynamax Pokémon use. The user summons a hailstorm lasting five turns.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(10));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 764)
            {
                skill.Name = new LocalText("**Max Ooze");
                skill.Desc = new LocalText("This is a Poison-type attack Dynamax Pokémon use. This raises ally Pokémon’s Sp. Atk stats.");
                skill.BaseCharges = 10;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(10));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 765)
            {
                skill.Name = new LocalText("**Max Geyser");
                skill.Desc = new LocalText("This is a Water-type attack Dynamax Pokémon use. The user summons a heavy rain that falls for five turns.");
                skill.BaseCharges = 10;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(10));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 766)
            {
                skill.Name = new LocalText("**Max Airstream");
                skill.Desc = new LocalText("This is a Flying-type attack Dynamax Pokémon use. This raises ally Pokémon’s Speed stats.");
                skill.BaseCharges = 10;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(10));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 767)
            {
                skill.Name = new LocalText("**Max Starfall");
                skill.Desc = new LocalText("This is a Fairy-type attack Dynamax Pokémon use. The user turns the ground into Misty Terrain for five turns.");
                skill.BaseCharges = 10;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(10));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 768)
            {
                skill.Name = new LocalText("**Max Wyrmwind");
                skill.Desc = new LocalText("This is a Dragon-type attack Dynamax Pokémon use. This lowers the target’s Attack stat.");
                skill.BaseCharges = 10;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(10));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 769)
            {
                skill.Name = new LocalText("**Max Mindstorm");
                skill.Desc = new LocalText("This is a Psychic-type attack Dynamax Pokémon use. The user turns the ground into Psychic Terrain for five turns.");
                skill.BaseCharges = 10;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(10));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 770)
            {
                skill.Name = new LocalText("**Max Rockfall");
                skill.Desc = new LocalText("This is a Rock-type attack Dynamax Pokémon use. The user summons a sandstorm lasting five turns.");
                skill.BaseCharges = 10;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(10));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 771)
            {
                skill.Name = new LocalText("**Max Quake");
                skill.Desc = new LocalText("This is a Ground-type attack Dynamax Pokémon use. This raises ally Pokémon’s Sp. Def stats.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(10));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 772)
            {
                skill.Name = new LocalText("**Max Darkness");
                skill.Desc = new LocalText("This is a Dark-type attack Dynamax Pokémon use. This lowers the target’s Sp. Def stat.");
                skill.BaseCharges = 10;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(10));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 773)
            {
                skill.Name = new LocalText("**Max Overgrowth");
                skill.Desc = new LocalText("This is a Grass-type attack Dynamax Pokémon use. The user turns the ground into Grassy Terrain for five turns.");
                skill.BaseCharges = 10;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(10));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 774)
            {
                skill.Name = new LocalText("**Max Steelspike");
                skill.Desc = new LocalText("This is a Steel-type attack Dynamax Pokémon use. This raises ally Pokémon’s Defense stats.");
                skill.BaseCharges = 10;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(10));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 775)
            {
                skill.Name = new LocalText("**Clangorous Soul");
                skill.Desc = new LocalText("The user raises all its stats by using some of its HP.");
                skill.BaseCharges = 5;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 776)
            {
                skill.Name = new LocalText("**Body Press");
                skill.Desc = new LocalText("The user attacks by slamming its body into the target. The higher the user’s Defense, the more damage it can inflict on the target.");
                skill.BaseCharges = 10;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 777)
            {
                skill.Name = new LocalText("-Decorate");
                skill.Desc = new LocalText("The user sharply raises the target's Attack and Sp. Atk stats by decorating the target.");
                skill.BaseCharges = 12;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 2));
                skill.Data.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, 2));
                skill.Strikes = 1;
                skill.HitboxAction = new ThrowAction();
                ((ThrowAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(25);//Appeal
                ((ThrowAction)skill.HitboxAction).Coverage = ThrowAction.ArcCoverage.WideAngle;
                ((ThrowAction)skill.HitboxAction).Range = 3;
                skill.HitboxAction.TargetAlignments = Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Friend;
                BetweenEmitter endAnim = new BetweenEmitter(new AnimData("Flatter_Back", 2), new AnimData("Flatter_Front", 2));
                endAnim.HeightBack = 56;
                endAnim.HeightFront = 56;
                skill.Data.HitFX.Emitter = endAnim;
                skill.Data.HitFX.Sound = "DUN_Encore_2";
                skill.Data.HitFX.Delay = 30;
            }
            else if (ii == 778)
            {
                skill.Name = new LocalText("**Drum Beating");
                skill.Desc = new LocalText("The user plays its drum, controlling the drum’s roots to attack the target. This also lowers the target’s Speed stat.");
                skill.BaseCharges = 10;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 779)
            {
                skill.Name = new LocalText("**Snap Trap");
                skill.Desc = new LocalText("The user snares the target in a snap trap for four to five turns.");
                skill.BaseCharges = 15;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(35));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 780)
            {
                skill.Name = new LocalText("-Pyro Ball");
                skill.Desc = new LocalText("The user attacks by igniting a small stone and launching it as a fiery ball at the target.");
                skill.BaseCharges = 5;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 10;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Foe | Alignment.Friend;
            }
            else if (ii == 781)
            {
                skill.Name = new LocalText("**Behemoth Blade");
                skill.Desc = new LocalText("The user becomes a gigantic sword and cuts the target. This move deals twice the damage if the target is Dynamaxed.");
                skill.BaseCharges = 5;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 782)
            {
                skill.Name = new LocalText("**Behemoth Bash");
                skill.Desc = new LocalText("The user becomes a gigantic shield and slams into the target. This move deals twice the damage if the target is Dynamaxed.");
                skill.BaseCharges = 5;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 783)
            {
                skill.Name = new LocalText("**Aura Wheel");
                skill.Desc = new LocalText("Morpeko attacks and raises its Speed with the energy stored in its cheeks. This move’s type changes depending on the user’s form.");
                skill.BaseCharges = 10;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(110));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 784)
            {
                skill.Name = new LocalText("**Breaking Swipe");
                skill.Desc = new LocalText("The user swings its tough tail wildly and attacks opposing Pokémon. This also lowers their Attack stats.");
                skill.BaseCharges = 15;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 785)
            {
                skill.Name = new LocalText("**Branch Poke");
                skill.Desc = new LocalText("The user attacks the target by poking it with a sharply pointed branch.");
                skill.BaseCharges = 40;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 786)
            {
                skill.Name = new LocalText("**Overdrive");
                skill.Desc = new LocalText("The user attacks opposing Pokémon by twanging a guitar or bass guitar, causing a huge echo and strong vibration.");
                skill.BaseCharges = 10;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 787)
            {
                skill.Name = new LocalText("-Apple Acid");
                skill.Desc = new LocalText("The user attacks the target with an acidic liquid created from tart apples. This also lowers the target's Sp. Def stat.");
                skill.BaseCharges = 10;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
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
                skill.HitboxAction.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.HitboxAction.ActionFX.Sound = "DUN_Bubble";
                skill.Data.HitFX.Sound = "DUN_Bubble_2";
            }
            else if (ii == 788)
            {
                skill.Name = new LocalText("-Grav Apple");
                skill.Desc = new LocalText("The user inflicts damage by dropping an apple from high above. This also lowers the target's Defense stat.");
                skill.BaseCharges = 10;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_defense", true, true, -1)));
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
            else if (ii == 789)
            {
                skill.Name = new LocalText("**Spirit Break");
                skill.Desc = new LocalText("The user attacks the target with so much force that it could break the target’s spirit. This also lowers the target’s Sp. Atk stat.");
                skill.BaseCharges = 15;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 790)
            {
                skill.Name = new LocalText("**Strange Steam");
                skill.Desc = new LocalText("The user attacks the target by emitting steam. This may also confuse the target.");
                skill.BaseCharges = 10;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 95;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 791)
            {
                skill.Name = new LocalText("-Life Dew");
                skill.Desc = new LocalText("The user scatters mysterious water around and restores the HP of itself and its ally Pokémon in the battle.");
                skill.BaseCharges = 10;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.Data.OnHits.Add(0, new RestoreHPEvent(1, 4, true));
                skill.HitboxAction = new AreaAction();
                ((AreaAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(06);//Charge
                ((AreaAction)skill.HitboxAction).Range = 4;
                ((AreaAction)skill.HitboxAction).Speed = 3;
                ((AreaAction)skill.HitboxAction).HitTiles = true;
                ((AreaAction)skill.HitboxAction).LagBehindTime = 30;
                skill.HitboxAction.TargetAlignments = (Alignment.Self | Alignment.Friend);
                skill.Explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);
                CircleSquareFountainEmitter emitter = new CircleSquareFountainEmitter();
                emitter.Anims.Add(new EmittingAnim(new AnimData("Water_Sport", 1, 0, 0), new StaticAnim(new AnimData("Water_Sport", 1, 0, 3)), DrawLayer.Normal));
                emitter.BurstTime = 4;
                emitter.ParticlesPerBurst = 2;
                emitter.Bursts = 6;
                emitter.RangeDiff = 16;
                emitter.HeightRatio = 0.8f;
                ((AreaAction)skill.HitboxAction).Emitter = emitter;
            }
            else if (ii == 792)
            {
                skill.Name = new LocalText("**Obstruct");
                skill.Desc = new LocalText("This move enables the user to protect itself from all attacks. Its chance of failing rises if it is used in succession. Direct contact harshly lowers the attacker’s Defense stat.");
                skill.BaseCharges = 10;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 793)
            {
                skill.Name = new LocalText("**False Surrender");
                skill.Desc = new LocalText("The user pretends to bow its head, but then it stabs the target with its disheveled hair. This attack never misses.");
                skill.BaseCharges = 10;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 794)
            {
                skill.Name = new LocalText("**Meteor Assault");
                skill.Desc = new LocalText("The user attacks wildly with its thick leek. The user can’t move on the next turn, because the force of this move makes it stagger.");
                skill.BaseCharges = 5;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(150));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 795)
            {
                skill.Name = new LocalText("**Eternabeam");
                skill.Desc = new LocalText("This is Eternatus’s most powerful attack in its original form. The user can’t move on the next turn.");
                skill.BaseCharges = 5;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(160));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 796)
            {
                skill.Name = new LocalText("**Steel Beam");
                skill.Desc = new LocalText("The user fires a beam of steel that it collected from its entire body. This also damages the user.");
                skill.BaseCharges = 5;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 95;
                skill.Data.SkillStates.Set(new BasePowerState(140));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 797)
            {
                skill.Name = new LocalText("**Expanding Force");
                skill.Desc = new LocalText("The user attacks the target with its psychic power. This move’s power goes up and damages all opposing Pokémon on Psychic Terrain.");
                skill.BaseCharges = 10;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 798)
            {
                skill.Name = new LocalText("**Steel Roller");
                skill.Desc = new LocalText("The user attacks while destroying the terrain. This move fails when the ground hasn’t turned into a terrain.");
                skill.BaseCharges = 5;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(130));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 799)
            {
                skill.Name = new LocalText("**Scale Shot");
                skill.Desc = new LocalText("The user attacks by shooting scales two to five times in a row. This move boosts the user’s Speed stat but lowers its Defense stat.");
                skill.BaseCharges = 20;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 800)
            {
                skill.Name = new LocalText("**Meteor Beam");
                skill.Desc = new LocalText("In this two-turn attack, the user gathers space power and boosts its Sp. Atk stat, then attacks the target on the next turn.");
                skill.BaseCharges = 10;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 801)
            {
                skill.Name = new LocalText("**Shell Side Arm");
                skill.Desc = new LocalText("This move inflicts physical or special damage, whichever will be more effective. This may also poison the target.");
                skill.BaseCharges = 10;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 802)
            {
                skill.Name = new LocalText("**Misty Explosion");
                skill.Desc = new LocalText("The user attacks everything around it and faints upon using this move. This move’s power is increased on Misty Terrain.");
                skill.BaseCharges = 5;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 803)
            {
                skill.Name = new LocalText("**Grassy Glide");
                skill.Desc = new LocalText("Gliding on the ground, the user attacks the target. This move always goes first on Grassy Terrain.");
                skill.BaseCharges = 20;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 804)
            {
                skill.Name = new LocalText("**Rising Voltage");
                skill.Desc = new LocalText("The user attacks with electric voltage rising from the ground. This move’s power doubles when the target is on Electric Terrain.");
                skill.BaseCharges = 20;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 805)
            {
                skill.Name = new LocalText("**Terrain Pulse");
                skill.Desc = new LocalText("The user utilizes the power of the terrain to attack. This move’s type and power changes depending on the terrain when it’s used.");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 806)
            {
                skill.Name = new LocalText("Skitter Smack");
                skill.Desc = new LocalText("The user skitters behind the target to attack. This also lowers the target’s Sp. Atk stat.");
                skill.BaseCharges = 16;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_special_attack", true, true, -1)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(40);//Swing
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Grass_Knot";
                skill.HitboxAction.PreActions.Add(preFX);
            }
            else if (ii == 807)
            {
                skill.Name = new LocalText("**Burning Jealousy");
                skill.Desc = new LocalText("The user attacks with energy from jealousy. This leaves all opposing Pokémon that have had their stats boosted during the turn with a burn.");
                skill.BaseCharges = 5;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 808)
            {
                skill.Name = new LocalText("**Lash Out");
                skill.Desc = new LocalText("The user lashes out to vent its frustration toward the target. If the user’s stats were lowered during this turn, the power of this move is doubled.");
                skill.BaseCharges = 5;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(75));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 809)
            {
                skill.Name = new LocalText("**Poltergeist");
                skill.Desc = new LocalText("The user attacks the target by controlling the target’s item. The move fails if the target doesn’t have an item.");
                skill.BaseCharges = 5;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(110));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 810)
            {
                skill.Name = new LocalText("**Corrosive Gas");
                skill.Desc = new LocalText("The user surrounds everything around it with highly acidic gas and melts away items they hold.");
                skill.BaseCharges = 40;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 811)
            {
                skill.Name = new LocalText("**Coaching");
                skill.Desc = new LocalText("The user properly coaches its ally Pokémon, boosting their Attack and Defense stats.");
                skill.BaseCharges = 10;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 812)
            {
                skill.Name = new LocalText("Flip Turn");
                skill.Desc = new LocalText("After making its attack, the user jumps back several steps.");
                skill.BaseCharges = 20;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new OnHitAnyEvent(true, 100, new HopEvent(3, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 813)
            {
                skill.Name = new LocalText("**Triple Axel");
                skill.Desc = new LocalText("A consecutive three-kick attack that becomes more powerful with each successful hit.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(20));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 814)
            {
                skill.Name = new LocalText("**Dual Wingbeat");
                skill.Desc = new LocalText("The user slams the target with its wings. The target is hit twice in a row.");
                skill.BaseCharges = 10;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 815)
            {
                skill.Name = new LocalText("**Scorching Sands");
                skill.Desc = new LocalText("The user throws scorching sand at the target to attack. This may also leave the target with a burn.");
                skill.BaseCharges = 10;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 816)
            {
                skill.Name = new LocalText("**Jungle Healing");
                skill.Desc = new LocalText("The user becomes one with the jungle, restoring HP and healing any status conditions of itself and its ally Pokémon in battle.");
                skill.BaseCharges = 10;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 817)
            {
                skill.Name = new LocalText("**Wicked Blow");
                skill.Desc = new LocalText("The user, having mastered the Dark style, strikes the target with a fierce blow. This attack always results in a critical hit.");
                skill.BaseCharges = 5;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 818)
            {
                skill.Name = new LocalText("**Surging Strikes");
                skill.Desc = new LocalText("The user, having mastered the Water style, strikes the target with a flowing motion three times in a row. This attack always results in a critical hit.");
                skill.BaseCharges = 5;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 819)
            {
                skill.Name = new LocalText("**Thunder Cage");
                skill.Desc = new LocalText("The user traps the target in a cage of sparking electricity for four to five turns.");
                skill.BaseCharges = 15;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 820)
            {
                skill.Name = new LocalText("**Dragon Energy");
                skill.Desc = new LocalText("Converting its life-force into power, the user attacks opposing Pokémon. The lower the user’s HP, the lower the move’s power.");
                skill.BaseCharges = 5;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(150));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 821)
            {
                skill.Name = new LocalText("**Freezing Glare");
                skill.Desc = new LocalText("The user shoots its psychic power from its eyes to attack. This may also leave the target frozen.");
                skill.BaseCharges = 10;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 822)
            {
                skill.Name = new LocalText("**Fiery Wrath");
                skill.Desc = new LocalText("The user transforms its wrath into a fire-like aura to attack. This may also make opposing Pokémon flinch.");
                skill.BaseCharges = 10;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 823)
            {
                skill.Name = new LocalText("**Thunderous Kick");
                skill.Desc = new LocalText("The user overwhelms the target with lightning-like movement before delivering a kick. This also lowers the target’s Defense stat.");
                skill.BaseCharges = 10;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 824)
            {
                skill.Name = new LocalText("**Glacial Lance");
                skill.Desc = new LocalText("The user attacks by hurling a blizzard-cloaked icicle lance at opposing Pokémon.");
                skill.BaseCharges = 5;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(130));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 825)
            {
                skill.Name = new LocalText("**Astral Barrage");
                skill.Desc = new LocalText("The user attacks by sending a frightful amount of small ghosts at opposing Pokémon.");
                skill.BaseCharges = 5;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 826)
            {
                skill.Name = new LocalText("**Eerie Spell");
                skill.Desc = new LocalText("The user attacks with its tremendous psychic power. This also removes 3 PP from the target’s last move.");
                skill.BaseCharges = 5;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 827)
            {
                skill.Name = new LocalText("**Dire Claw");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 15;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 828)
            {
                skill.Name = new LocalText("Psyshield Bash");
                skill.Desc = new LocalText("Cloaking itself in psychic energy, the user slams into the target. This also boosts the user’s Defense stat.");
                skill.BaseCharges = 12;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.SkillStates.Set(new AdditionalEffectState(100));
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new AdditionalEndEvent(new StatusStackBattleEvent("mod_defense", false, true, 1)));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Foe | Alignment.Friend;
            }
            else if (ii == 829)
            {
                skill.Name = new LocalText("**Power Shift");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 830)
            {
                skill.Name = new LocalText("-Stone Axe");
                skill.Desc = new LocalText("The user swings its stone axes at the target. Stone splinters left behind by this attack float around the target.");
                skill.BaseCharges = 15;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Data.OnHitTiles.Add(0, new SetTrapEvent("trap_stealth_rock"));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(13);//Slice
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Wide;
                skill.HitboxAction.ActionFX.Sound = "DUN_Slash";
                SingleEmitter single = new SingleEmitter(new AnimData("Slash_Ranger", 3));
                single.Offset = 16;
                skill.HitboxAction.ActionFX.Emitter = single;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 831)
            {
                skill.Name = new LocalText("**Springtide Storm");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 5;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 832)
            {
                skill.Name = new LocalText("**Mystical Power");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 833)
            {
                skill.Name = new LocalText("**Raging Fury");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 834)
            {
                skill.Name = new LocalText("Wave Crash");
                skill.Desc = new LocalText("The user shrouds itself in water and slams into the target with its whole body to inflict damage.");
                skill.BaseCharges = 10;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new HPRecoilEvent(4, false));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 23;//Slam
                ((DashAction)skill.HitboxAction).Range = 4;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                BattleFX preFX = new BattleFX();
                preFX.Sound = "DUN_Water_Spout";
                skill.HitboxAction.PreActions.Add(preFX);
                SingleEmitter emitter = new SingleEmitter(new AnimData("Cleanse_Blue", 2));
                emitter.LocHeight = 120;
                skill.Data.HitFX.Emitter = emitter;
                skill.Data.HitFX.Sound = "DUN_Surf";
            }
            else if (ii == 835)
            {
                skill.Name = new LocalText("**Chloroblast");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 5;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 95;
                skill.Data.SkillStates.Set(new BasePowerState(150));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 836)
            {
                skill.Name = new LocalText("**Mountain Gale");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 837)
            {
                skill.Name = new LocalText("**Victory Dance");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 838)
            {
                skill.Name = new LocalText("**Headlong Rush");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 5;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 839)
            {
                skill.Name = new LocalText("Barb Barrage");
                skill.Desc = new LocalText("The user launches countless toxic barbs to inflict damage. This may also poison the target. This move’s power is doubled if the target is already poisoned.");
                skill.BaseCharges = 14;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.BeforeHits.Add(0, new StatusPowerEvent("poison", true));
                skill.Data.BeforeHits.Add(0, new StatusPowerEvent("poison_toxic", true));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("poison", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(07);//Shoot
                ((ProjectileAction)skill.HitboxAction).Range = 6;
                ((ProjectileAction)skill.HitboxAction).Speed = 16;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                ((ProjectileAction)skill.HitboxAction).Anim = new AnimData("Spike_Cannon", 3);
                skill.HitboxAction.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.HitboxAction.ActionFX.Sound = "DUN_Throw_Spike";
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
            else if (ii == 840)
            {
                skill.Name = new LocalText("**Esper Wing");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 841)
            {
                skill.Name = new LocalText("Bitter Malice");
                skill.Desc = new LocalText("The user attacks the target with spine-chilling resentment. This may also leave the target frozen.");
                skill.BaseCharges = 12;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(55));
                skill.Data.SkillStates.Set(new AdditionalEffectState(25));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("freeze", true, true)));
                skill.Strikes = 1;
                skill.HitboxAction = new OffsetAction();
                ((OffsetAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(36);//Special
                ((OffsetAction)skill.HitboxAction).HitArea = OffsetAction.OffsetArea.Area;
                ((OffsetAction)skill.HitboxAction).Range = 2;
                ((OffsetAction)skill.HitboxAction).Speed = 10;
                ((OffsetAction)skill.HitboxAction).LagBehindTime = 5;
                ((OffsetAction)skill.HitboxAction).HitTiles = true;
                skill.Explosion.ExplodeFX.Emitter = new BetweenEmitter(new AnimData("Dark_Pulse_Back", 3), new AnimData("Dark_Pulse_Front", 3));
                CircleSquareAreaEmitter areaEmitter = new CircleSquareAreaEmitter(new AnimData("Spite", 3));
                areaEmitter.ParticlesPerTile = 0.8;
                areaEmitter.RangeDiff = -12;
                skill.Explosion.Emitter = areaEmitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Foggy";
            }
            else if (ii == 842)
            {
                skill.Name = new LocalText("**Shelter");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 843)
            {
                skill.Name = new LocalText("**Triple Arrows");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 844)
            {
                skill.Name = new LocalText("-Infernal Parade");
                skill.Desc = new LocalText("The user attacks with myriad fireballs. This may also leave the target with a burn. This move's power is doubled if the target has a status condition.");
                skill.BaseCharges = 15;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.SkillStates.Set(new AdditionalEffectState(35));
                skill.Data.BeforeHits.Add(0, new MajorStatusPowerEvent(true, 2, 1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("burn", true, true)));
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
                skill.HitboxAction.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.HitboxAction.ActionFX.Sound = "DUN_Fire_Spin";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Thief_Hit_Dark", 2));
            }
            else if (ii == 845)
            {
                skill.Name = new LocalText("-Ceaseless Edge");
                skill.Desc = new LocalText("The user slashes its shell blade at the target. Shell splinters left behind by this attack remain scattered under the target as spikes.");
                skill.BaseCharges = 14;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 85;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter cuttingEmitter = new SingleEmitter(new AnimData("Grass_Clear", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Charge_Start", cuttingEmitter, new FlagType(typeof(FoliageTerrainState))));
                skill.Data.OnHitTiles.Add(0, new SetTrapEvent("trap_spikes"));
                skill.Strikes = 1;
                skill.HitboxAction = new ProjectileAction();
                ((ProjectileAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(13);//Slice
                ((ProjectileAction)skill.HitboxAction).Range = 2;
                ((ProjectileAction)skill.HitboxAction).Speed = 8;
                ((ProjectileAction)skill.HitboxAction).Rays = ProjectileAction.RayCount.Three;
                ((ProjectileAction)skill.HitboxAction).StopAtWall = true;
                ((ProjectileAction)skill.HitboxAction).StopAtHit = true;
                ((ProjectileAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 846)
            {
                skill.Name = new LocalText("**Bleakwind Storm");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "flying";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 847)
            {
                skill.Name = new LocalText("**Wildbolt Storm");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 848)
            {
                skill.Name = new LocalText("**Sandsear Storm");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "ground";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 80;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 849)
            {
                skill.Name = new LocalText("**Lunar Blessing");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 5;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 850)
            {
                skill.Name = new LocalText("**Take Heart");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 851)
            {
                skill.Name = new LocalText("**Tera Blast");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 852)
            {
                skill.Name = new LocalText("**Silk Trap");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 853)
            {
                skill.Name = new LocalText("**Axe Kick");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 854)
            {
                skill.Name = new LocalText("**Last Respects");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 855)
            {
                skill.Name = new LocalText("**Lumina Crash");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 856)
            {
                skill.Name = new LocalText("**Order Up");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 857)
            {
                skill.Name = new LocalText("Jet Punch");
                skill.Desc = new LocalText("The user summons a torrent around its fist and punches at blinding speed.");
                skill.BaseCharges = 12;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(60));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Wall_Break", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Rollout", terrainEmitter, new FlagType(typeof(WallTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 11;//Punch
                ((DashAction)skill.HitboxAction).Range = 4;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.Data.HitFX.Sound = "DUN_Punch";
                SingleEmitter frontEmitter = new SingleEmitter(new AnimData("Print_Fist", 12));
                FiniteReleaseEmitter endAnim = new FiniteReleaseEmitter(new AnimData("Hydro_Cannon", 5, 2, -1));
                endAnim.BurstTime = 2;
                endAnim.ParticlesPerBurst = 2;
                endAnim.Bursts = 4;
                endAnim.StartDistance = 4;
                endAnim.Speed = 60;
                endAnim.Layer = DrawLayer.Back;
                ListEmitter multiEmitter = new ListEmitter();
                multiEmitter.Anim.Add(endAnim);
                multiEmitter.Anim.Add(frontEmitter);
                skill.Data.HitFX.Emitter = multiEmitter;
            }
            else if (ii == 858)
            {
                skill.Name = new LocalText("**Spicy Extract");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 15;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 859)
            {
                skill.Name = new LocalText("**Spin Out");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 5;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 860)
            {
                skill.Name = new LocalText("**Population Bomb");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(20));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 861)
            {
                skill.Name = new LocalText("**Ice Spinner");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 15;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 862)
            {
                skill.Name = new LocalText("**Glaive Rush");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 5;
                skill.Data.Element = "dragon";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 863)
            {
                skill.Name = new LocalText("**Revival Blessing");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 1;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 864)
            {
                skill.Name = new LocalText("**Salt Cure");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 15;
                skill.Data.Element = "rock";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 865)
            {
                skill.Name = new LocalText("**Triple Dive");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 95;
                skill.Data.SkillStates.Set(new BasePowerState(30));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 866)
            {
                skill.Name = new LocalText("Mortal Spin");
                skill.Desc = new LocalText("A spin attack that can eliminate moves such as Wrap or Leech Seed. It also destroys traps, and poisons opponents.");
                skill.BaseCharges = 18;
                skill.Data.Element = "poison";
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
                skill.Data.AfterActions.Add(0, new RemoveStatusBattleEvent("magma_storm", false));
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimProcess(CharAnimProcess.ProcessType.Spin);//Spin
                ((AttackAction)skill.HitboxAction).HitTiles = true;
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.Around;
                SingleEmitter shootEmitter = new SingleEmitter(new AnimData("Gust_Purple", 1));
                shootEmitter.LocHeight = 24;
                skill.HitboxAction.ActionFX.Emitter = shootEmitter;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Gunk_Shot_2";
            }
            else if (ii == 867)
            {
                skill.Name = new LocalText("**Doodle");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = 100;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 868)
            {
                skill.Name = new LocalText("**Fillet Away");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 869)
            {
                skill.Name = new LocalText("Kowtow Cleave");
                skill.Desc = new LocalText("The user slashes at the target after kowtowing to make the target let down its guard. This attack never misses.");
                skill.BaseCharges = 17;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(85));
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
                skill.HitboxAction.TileEmitter = new SingleEmitter(new AnimData("Night_Slash", 1));
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
                skill.HitboxAction.ActionFX.Sound = "DUN_Guillotine";
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Psycho_Cut_Cut", 2));
            }
            else if (ii == 870)
            {
                skill.Name = new LocalText("**Flower Trick");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = -1;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 871)
            {
                skill.Name = new LocalText("**Torch Song");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 872)
            {
                skill.Name = new LocalText("**Aqua Step");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 873)
            {
                skill.Name = new LocalText("**Raging Bull");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 874)
            {
                skill.Name = new LocalText("**Make It Rain");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 5;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 875)
            {
                skill.Name = new LocalText("**Psyblade");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 15;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 876)
            {
                skill.Name = new LocalText("**Hydro Steam");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 15;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 877)
            {
                skill.Name = new LocalText("**Ruination");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 90;
                skill.Data.SkillStates.Set(new BasePowerState(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 878)
            {
                skill.Name = new LocalText("**Collision Course");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 5;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 879)
            {
                skill.Name = new LocalText("**Electro Drift");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 5;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 880)
            {
                skill.Name = new LocalText("**Shed Tail");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 881)
            {
                skill.Name = new LocalText("**Chilly Reception");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 882)
            {
                skill.Name = new LocalText("**Tidy Up");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 883)
            {
                skill.Name = new LocalText("**Snowscape");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "ice";
                skill.Data.Category = BattleData.SkillCategory.Status;
                skill.Data.HitRate = -1;
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 884)
            {
                skill.Name = new LocalText("**Pounce");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 20;
                skill.Data.Element = "bug";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 885)
            {
                skill.Name = new LocalText("**Trailblaze");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 20;
                skill.Data.Element = "grass";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 886)
            {
                skill.Name = new LocalText("**Chilling Water");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 20;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(50));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 887)
            {
                skill.Name = new LocalText("Hyper Drill");
                skill.Desc = new LocalText("The user spins the pointed part of its body at high speed to pierce the target. This attack can hit a target using a move such as Protect or Detect.");
                skill.BaseCharges = 13;
                skill.Data.Element = "normal";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("wide_guard", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("quick_guard", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("protect", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("all_protect", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("detect", true));
                skill.Data.BeforeHits.Add(-1, new RemoveStatusBattleEvent("kings_shield", true));
                SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Wall_Break", 2));
                skill.Data.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Rollout", terrainEmitter, new FlagType(typeof(WallTerrainState))));
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 20;//Jab
                ((DashAction)skill.HitboxAction).Range = 3;
                ((DashAction)skill.HitboxAction).WideAngle = LineCoverage.FrontAndCorners;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                ((DashAction)skill.HitboxAction).AnimOffset = 12;
                ((DashAction)skill.HitboxAction).Anim = new AnimData("Fury_Attack", 2);
                skill.HitboxAction.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.Explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                skill.HitboxAction.ActionFX.Sound = "DUN_Fire_Blast";
            }
            else if (ii == 888)
            {
                skill.Name = new LocalText("Twin Beam");
                skill.Desc = new LocalText("The user shoots mystical beams from its eyes to inflict damage. The target is hit twice in a row.");
                skill.BaseCharges = 10;
                skill.Data.Element = "psychic";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
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
            else if (ii == 889)
            {
                skill.Name = new LocalText("Rage Fist");
                skill.Desc = new LocalText("The user converts its rage into energy to attack. The more times the user has been hit before attacking, the greater the move's power.");
                skill.BaseCharges = 14;
                skill.Data.Element = "ghost";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(40));
                skill.Data.SkillStates.Set(new ContactState());
                skill.Data.SkillStates.Set(new FistState());
                skill.Data.BeforeHits.Add(0, new PrevHitBasePowerEvent(40, false, "was_hurt_since_attack", 10));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new DashAction();
                ((DashAction)skill.HitboxAction).CharAnim = 11;//Punch
                ((DashAction)skill.HitboxAction).Range = 2;
                ((DashAction)skill.HitboxAction).StopAtHit = true;
                ((DashAction)skill.HitboxAction).StopAtWall = true;
                ((DashAction)skill.HitboxAction).HitTiles = true;
                skill.HitboxAction.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.Explosion.TargetAlignments = Alignment.Foe | Alignment.Friend;
                skill.Data.HitFX.Emitter = new SingleEmitter(new AnimData("Print_Fist", 12));
            }
            else if (ii == 890)
            {
                skill.Name = new LocalText("**Armor Cannon");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 5;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Magical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 891)
            {
                skill.Name = new LocalText("**Bitter Blade");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(90));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 892)
            {
                skill.Name = new LocalText("**Double Shock");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 5;
                skill.Data.Element = "electric";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(120));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 893)
            {
                skill.Name = new LocalText("Gigaton Hammer");
                skill.Desc = new LocalText("The user swings its whole body around to attack with its huge hammer. This move can't be used twice in a row.");
                skill.BaseCharges = 8;
                skill.Data.Element = "steel";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(140));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Data.AfterActions.Add(0, new CounterDisableBattleEvent("cooldown", new StringKey()));
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
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(23);//Slam
                ((AttackAction)skill.HitboxAction).WideAngle = AttackCoverage.FrontAndCorners;
                ((AttackAction)skill.HitboxAction).BurstTiles = TileAlignment.Any;
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                ((AttackAction)skill.HitboxAction).Emitter = new SingleEmitter(new AnimData("Print_Fist", 12));
                skill.HitboxAction.ActionFX.Sound = "DUN_Punch";
            }
            else if (ii == 894)
            {
                skill.Name = new LocalText("**Comeuppance");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(1));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 895)
            {
                skill.Name = new LocalText("**Aqua Cutter");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 20;
                skill.Data.Element = "water";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.SkillStates.Set(new BladeState());
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(70));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 896)
            {
                skill.Name = new LocalText("**Blazing Torque");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "fire";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 897)
            {
                skill.Name = new LocalText("**Wicked Torque");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "dark";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(80));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 898)
            {
                skill.Name = new LocalText("**Noxious Torque");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "poison";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 899)
            {
                skill.Name = new LocalText("**Combat Torque");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "fighting";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
            else if (ii == 900)
            {
                skill.Name = new LocalText("**Magical Torque");
                skill.Desc = new LocalText("");
                skill.BaseCharges = 10;
                skill.Data.Element = "fairy";
                skill.Data.Category = BattleData.SkillCategory.Physical;
                skill.Data.HitRate = 100;
                skill.Data.SkillStates.Set(new BasePowerState(100));
                skill.Data.OnHits.Add(-1, new DamageFormulaEvent());
                skill.Strikes = 1;
                skill.HitboxAction = new AttackAction();
                ((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack
                skill.HitboxAction.TargetAlignments = Alignment.Foe;
                skill.Explosion.TargetAlignments = Alignment.Foe;
            }
        }
    }
}
