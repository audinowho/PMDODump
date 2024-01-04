﻿using System;
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
    public static class IntrinsicInfo
    {
        public const int MAX_INTRINSICS = 299;

        public static void AddIntrinsicData()
        {
            DataInfo.DeleteIndexedData(DataManager.DataType.Intrinsic.ToString());
            for (int ii = 0; ii < MAX_INTRINSICS; ii++)
            {
                (string, IntrinsicData) ability = GetIntrinsicData(ii);
                if (ability.Item1 != "")
                    DataManager.SaveData(ability.Item1, DataManager.DataType.Intrinsic.ToString(), ability.Item2);
            }
        }
        public static (string, IntrinsicData) GetIntrinsicData(int ii)
        {
            string fileName = "";
            IntrinsicData ability = new IntrinsicData();
            ability.IndexNum = ii;
            if (ii == 0)
            {
                ability.Name = new LocalText("None");
            }
            else if (ii == 1)
            {
                ability.Name = new LocalText("Stench");
                ability.Desc = new LocalText("Contact with the Pokémon may cause flinching.");
                FiniteReleaseEmitter emitter = new FiniteReleaseEmitter(new AnimData("Puff_Green", 3));
                emitter.BurstTime = 4;
                emitter.ParticlesPerBurst = 1;
                emitter.Bursts = 3;
                emitter.Speed = 48;
                emitter.StartDistance = 4;
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 35, new StatusBattleEvent("flinch", false, true, false, new StringKey("MSG_STENCH"),
                    new BattleAnimEvent(emitter, "DUN_Smokescreen", false, 30))));
            }
            else if (ii == 2)
            {
                ability.Name = new LocalText("Drizzle");
                ability.Desc = new LocalText("The Pokémon makes it rain when it is battling.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent("rain", 10, new StringKey("MSG_DRIZZLE"), typeof(ExtendWeatherState))));
            }
            else if (ii == 3)
            {
                ability.Name = new LocalText("Speed Boost");
                ability.Desc = new LocalText("The Pokémon's Movement Speed is increased.");
                ability.OnRefresh.Add(0, new AddSpeedEvent(1));
            }
            else if (ii == 4)
            {
                ability.Name = new LocalText("Battle Armor");
                ability.Desc = new LocalText("Hard armor protects the Pokémon from critical hits.");
                ability.BeforeBeingHits.Add(0, new BlockCriticalEvent());
            }
            else if (ii == 5)
            {
                ability.Name = new LocalText("Sturdy");
                ability.Desc = new LocalText("The Pokémon cannot be knocked out with one hit.");
                ability.BeforeBeingHits.Add(0, new FullEndureEvent());
            }
            else if (ii == 6)
            {
                ability.Name = new LocalText("Damp");
                ability.Desc = new LocalText("Prevents explosions and other forms of splash damage.");
                ability.ProximityEvent.Radius = 5;
                ability.ProximityEvent.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                ability.ProximityEvent.BeforeExplosions.Add(0, new DampEvent(2, new StringKey("MSG_DAMP")));
            }
            else if (ii == 7)
            {
                ability.Name = new LocalText("Limber");
                ability.Desc = new LocalText("Its limber body protects the Pokémon from paralysis.");
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("paralyze", new StringKey("MSG_LIMBER")));
            }
            else if (ii == 8)
            {
                ability.Name = new LocalText("Sand Veil");
                ability.Desc = new LocalText("Avoids attacks from a distance when in a sandstorm.");
                ability.OnRefresh.Add(0, new MiscEvent(new SandState()));
                ability.BeforeBeingHits.Add(0, new WeatherNeededEvent("sandstorm", new EvadeDistanceEvent()));
            }
            else if (ii == 9)
            {
                ability.Name = new LocalText("Static");
                ability.Desc = new LocalText("The Pokémon is charged with static electricity, so contact with it may cause paralysis.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 35, new StatusBattleEvent("paralyze", false, true, false, new StringKey("MSG_STATIC"),
                    new BattleAnimEvent(new SingleEmitter(new AnimData("Spark", 3)), "DUN_Paralyzed", false, 0))));
            }
            else if (ii == 10)
            {
                ability.Name = new LocalText("Volt Absorb");
                ability.Desc = new LocalText("Restores HP if hit by an Electric-type move.");
                ability.ProximityEvent.Radius = 0;
                ability.ProximityEvent.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                ability.ProximityEvent.BeforeExplosions.Add(0, new IsolateElementEvent("electric"));
                ability.BeforeBeingHits.Add(5, new AbsorbElementEvent("electric", false, true,
                    new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Drink",
                    new RestoreHPEvent(1, 4, true)));
            }
            else if (ii == 11)
            {
                ability.Name = new LocalText("Water Absorb");
                ability.Desc = new LocalText("Restores HP if hit by a Water-type move.");
                ability.ProximityEvent.Radius = 0;
                ability.ProximityEvent.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                ability.ProximityEvent.BeforeExplosions.Add(0, new IsolateElementEvent("water"));
                ability.BeforeBeingHits.Add(5, new AbsorbElementEvent("water", false, true,
                    new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Drink",
                    new RestoreHPEvent(1, 4, true)));
            }
            else if (ii == 12)
            {
                ability.Name = new LocalText("Oblivious");
                ability.Desc = new LocalText("Protects the Pokémon from Attract or Rage Powder.");
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("in_love", new StringKey("MSG_OBLIVIOUS")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("rage_powder", new StringKey("MSG_OBLIVIOUS")));
            }
            else if (ii == 13)
            {
                ability.Name = new LocalText("Cloud Nine");
                ability.Desc = new LocalText("The Pokémon eliminates weather when it is battling.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent("clear", 10, new StringKey("MSG_CLOUD_NINE"), typeof(ExtendWeatherState))));
            }
            else if (ii == 14)
            {
                ability.Name = new LocalText("Compound Eyes");
                ability.Desc = new LocalText("Boosts the Pokémon's Attack Range.");
                ability.OnActions.Add(-1, new AddRangeEvent(1));
            }
            else if (ii == 15)
            {
                ability.Name = new LocalText("Insomnia");
                ability.Desc = new LocalText("The Pokémon is suffering from insomnia and cannot fall asleep.");
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("sleep", new StringKey("MSG_INSOMNIA")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("yawning", new StringKey("MSG_INSOMNIA_DROWSY")));
            }
            else if (ii == 16)
            {
                ability.Name = new LocalText("Color Change");
                ability.Desc = new LocalText("The Pokémon's type becomes the type of the move used on it.");
                ability.AfterBeingHits.Add(0, new ConversionEvent(true));
            }
            else if (ii == 17)
            {
                ability.Name = new LocalText("Immunity");
                ability.Desc = new LocalText("The immune system of the Pokémon prevents it from getting poisoned.");
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("poison", new StringKey("MSG_IMMUNITY")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("poison_toxic", new StringKey("MSG_IMMUNITY")));
            }
            else if (ii == 18)
            {
                ability.Name = new LocalText("Flash Fire");
                ability.Desc = new LocalText("Powers up the Pokémon's Fire-type moves if it's hit by one.");
                ability.ProximityEvent.Radius = 0;
                ability.ProximityEvent.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                ability.ProximityEvent.BeforeExplosions.Add(0, new IsolateElementEvent("fire"));
                ability.BeforeBeingHits.Add(5, new AbsorbElementEvent("fire", false, true,
                    new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Drink",
                    new StatusElementBattleEvent("type_boosted", true, false, "fire")));
            }
            else if (ii == 19)
            {
                ability.Name = new LocalText("Shield Dust");
                ability.Desc = new LocalText("This Pokémon's dust blocks the additional effects of attacks taken.");
                ability.BeforeBeingHits.Add(0, new ExceptInfiltratorEvent(false, new BlockAdditionalEvent()));
            }
            else if (ii == 20)
            {
                ability.Name = new LocalText("Own Tempo");
                ability.Desc = new LocalText("This Pokémon has its own tempo, and that prevents it from becoming confused.");
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("confuse", new StringKey("MSG_OWN_TEMPO")));
            }
            else if (ii == 21)
            {
                ability.Name = new LocalText("Suction Cups");
                ability.Desc = new LocalText("Prevents the Pokémon from being forced off its location.");
                ability.OnRefresh.Add(0, new MiscEvent(new AnchorState()));
            }
            else if (ii == 22)
            {
                ability.Name = new LocalText("Intimidate");
                ability.Desc = new LocalText("Lowers the Attack stat of opposing Pokémon nearby.");
                ability.ProximityEvent.Radius = 1;
                ability.ProximityEvent.TargetAlignments = Alignment.Foe;
                ability.ProximityEvent.OnActions.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Physical, 2, 3));
            }
            else if (ii == 23)
            {
                ability.Name = new LocalText("Shadow Tag");
                ability.Desc = new LocalText("This Pokémon steps on the opposing Pokémon's shadow to prevent it from escaping.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent(Alignment.Foe, false, false, false, 100, new StatusBattleEvent("immobilized", false, true, false, new StringKey("MSG_SHADOW_TAG"),
                    new BattleAnimEvent(new SingleEmitter(new AnimData("Dark_Pulse_Ranger", 3)), "DUN_Night_Shade", false, 0))));
                //ability.AfterHittings.Add(0, new OnHitEvent(false, false, 100, new StatusBattleEvent("immobilized", true, true, false, new StringKey("MSG_SHADOW_TAG"),
                //    new BattleAnimEvent(new SingleEmitter(new AnimData("Dark_Pulse_Ranger", 3)), "DUN_Night_Shade", true, 0))));
            }
            else if (ii == 24)
            {
                ability.Name = new LocalText("Rough Skin");
                ability.Desc = new LocalText("This Pokémon inflicts damage with its rough skin to the attacker on contact.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 100, new ChipDamageEvent(8, new StringKey("MSG_HURT_BY_OTHER"), true, true)));
            }
            else if (ii == 25)
            {
                ability.Name = new LocalText("Wonder Guard");
                ability.Desc = new LocalText("Its mysterious power only lets super effective moves hit the Pokémon.");
                ability.BeforeBeingHits.Add(0, new WonderGuardEvent(new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", true, 10)));
                ability.BeforeBeingHits.Add(6, new ElementNeededEvent("none", new RegularAttackNeededEvent(new SetDamageEvent(new SpecificDamageEvent(1), new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", true, 10)))));
            }
            else if (ii == 26)
            {
                ability.Name = new LocalText("Levitate");
                ability.Desc = new LocalText("By floating in the air, the Pokémon receives full immunity to all Ground-type moves.");
                ability.TargetElementEffects.Add(0, new TypeImmuneEvent("ground"));
                ability.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Water));
                ability.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Abyss));
                ability.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Lava));
            }
            else if (ii == 27)
            {
                ability.Name = new LocalText("Effect Spore");
                ability.Desc = new LocalText("Contact with the Pokémon may inflict poison, sleep, or paralysis on its attacker.");
                FiniteReleaseEmitter emitter1 = new FiniteReleaseEmitter(new AnimData("Puff_Pink", 3), new AnimData("Puff_Brown", 3), new AnimData("Puff_Purple", 3));
                emitter1.BurstTime = 4;
                emitter1.ParticlesPerBurst = 2;
                emitter1.Bursts = 2;
                emitter1.Speed = 48;
                emitter1.StartDistance = 4;
                FiniteReleaseEmitter emitter2 = new FiniteReleaseEmitter(new AnimData("Puff_Pink", 3), new AnimData("Puff_Brown", 3), new AnimData("Puff_Purple", 3));
                emitter2.BurstTime = 4;
                emitter2.ParticlesPerBurst = 2;
                emitter2.Bursts = 2;
                emitter2.Speed = 48;
                emitter2.StartDistance = 4;
                FiniteReleaseEmitter emitter3 = new FiniteReleaseEmitter(new AnimData("Puff_Pink", 3), new AnimData("Puff_Brown", 3), new AnimData("Puff_Purple", 3));
                emitter3.BurstTime = 4;
                emitter3.ParticlesPerBurst = 2;
                emitter3.Bursts = 2;
                emitter3.Speed = 48;
                emitter3.StartDistance = 4;
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 100,
                    new StatusBattleEvent("sleep", false, true, false, new StringKey("MSG_EFFECT_SPORE"), new BattleAnimEvent(emitter1, "DUN_Substitute", false, 20)),
                    new StatusBattleEvent("paralyze", false, true, false, new StringKey("MSG_EFFECT_SPORE"), new BattleAnimEvent(emitter2, "DUN_Substitute", false, 20)),
                    new StatusBattleEvent("poison", false, true, false, new StringKey("MSG_EFFECT_SPORE"), new BattleAnimEvent(emitter3, "DUN_Substitute", false, 20))));
            }
            else if (ii == 28)
            {
                ability.Name = new LocalText("Synchronize");
                ability.Desc = new LocalText("Passes major status problems to opposing Pokémon.");
                ability.OnStatusAdds.Add(0, new StateStatusShareEvent(typeof(MajorStatusState), 3, new StringKey("MSG_SYNCHRONIZE"), new StatusAnimEvent(new SingleEmitter(new AnimData("Power_Trick", 2)), "DUN_Destiny_Bond", 30)));
            }
            else if (ii == 29)
            {
                ability.Name = new LocalText("Clear Body");
                ability.Desc = new LocalText("Prevents other Pokémon from lowering its stats.");
                ability.BeforeStatusAdds.Add(0, new StatChangeCheck(new List<Stat>(), new StringKey("MSG_STAT_DROP_PROTECT"), true, false, false));
            }
            else if (ii == 30)
            {
                ability.Name = new LocalText("Natural Cure");
                ability.Desc = new LocalText("Cures all status problems on the Pokémon when it is at full HP.");
                ability.OnTurnStarts.Add(0, new MaxHPNeededEvent(new CureAllEvent(new StringKey("MSG_CURE_SELF"), new AnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Wonder_Tile", 10))));
            }
            else if (ii == 31)
            {
                ability.Name = new LocalText("Lightning Rod");
                ability.Desc = new LocalText("The Pokémon draws in all Electric-type moves. Instead of being hit by Electric-type moves, it boosts its Sp. Atk.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = Alignment.Foe;
                ability.ProximityEvent.BeforeExplosions.Add(0, new DrawAttackEvent(Alignment.Friend, "electric", new StringKey("MSG_DRAW_ATTACK")));
                ability.BeforeBeingHits.Add(5, new AbsorbElementEvent("electric", true, new StatusStackBattleEvent("mod_special_attack", true, false, 1)));
                ability.BeforeBeingHits.Add(5, new AddContextStateEvent(new SingleDrawAbsorb(), true));
            }
            else if (ii == 32)
            {
                ability.Name = new LocalText("Serene Grace");
                ability.Desc = new LocalText("Boosts the likelihood of additional effects occurring when attacking.");
                ability.OnActions.Add(0, new BoostAdditionalEvent());
            }
            else if (ii == 33)
            {
                ability.Name = new LocalText("Swift Swim");
                ability.Desc = new LocalText("Boosts the Pokémon's Movement Speed in rain.");
                ability.OnRefresh.Add(0, new WeatherSpeedEvent("rain"));
            }
            else if (ii == 34)
            {
                ability.Name = new LocalText("Chlorophyll");
                ability.Desc = new LocalText("Boosts the Pokémon's Movement Speed in sunshine.");
                ability.OnRefresh.Add(0, new WeatherSpeedEvent("sunny"));
            }
            else if (ii == 35)
            {
                ability.Name = new LocalText("Illuminate");
                ability.Desc = new LocalText("It may warp an ally to the Pokémon when it is hurt.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent(Alignment.Foe, true, false, true, 35, new WarpAlliesInEvent(80, 1, true, new StringKey("MSG_ILLUMINATE"), true)));
            }
            else if (ii == 36)
            {
                ability.Name = new LocalText("Trace");
                ability.Desc = new LocalText("The Pokémon copies the Ability of the Pokémon that attacks it.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), false, false, true, 100, new ReflectAbilityEvent(true, new StringKey("MSG_TRACE"))));
            }
            else if (ii == 37)
            {
                ability.Name = new LocalText("Huge Power");
                ability.Desc = new LocalText("Boosts the Pokémon's Attack stat.");
                ability.OnActions.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Physical, 3, 2));
            }
            else if (ii == 38)
            {
                ability.Name = new LocalText("Poison Point");
                ability.Desc = new LocalText("Contact with the Pokémon may poison the attacker.");
                SqueezedAreaEmitter emitter = new SqueezedAreaEmitter(new AnimData("Bubbles_Purple", 3));
                emitter.BurstTime = 3;
                emitter.Bursts = 4;
                emitter.ParticlesPerBurst = 1;
                emitter.Range = 12;
                emitter.StartHeight = -4;
                emitter.HeightSpeed = 12;
                emitter.SpeedDiff = 4;
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 35, new StatusBattleEvent("poison", false, true, false, new StringKey("MSG_POISON_POINT"),
                    new BattleAnimEvent(emitter, "DUN_Toxic", false, 30))));
            }
            else if (ii == 39)
            {
                ability.Name = new LocalText("Inner Focus");
                ability.Desc = new LocalText("The Pokémon's intensely focused, and that protects the Pokémon from flinching.");
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("flinch", new StringKey("MSG_INNER_FOCUS")));
            }
            else if (ii == 40)
            {
                ability.Name = new LocalText("Magma Armor");
                ability.Desc = new LocalText("The Pokémon is covered with hot magma, which prevents the Pokémon from becoming frozen. Thrown items will also burn up on contact.");
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("freeze", new StringKey("MSG_MAGMA_ARMOR")));
                ability.ProximityEvent.Radius = 0;
                ability.ProximityEvent.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                ability.ProximityEvent.BeforeExplosions.Add(0, new DampItemEvent());

                BattleData catchData = new BattleData();
                catchData.Element = "none";
                SingleEmitter emitter = new SingleEmitter(new AnimData("Ember", 3));
                emitter.LocHeight = 8;
                catchData.HitFX.Emitter = emitter;
                catchData.HitFX.Sound = "DUN_Ember_2";
                ability.BeforeBeingHits.Add(0, new ThrowItemDestroyEvent(catchData));
                ability.OnRefresh.Add(0, new ThrownItemBarrierEvent());
            }
            else if (ii == 41)
            {
                ability.Name = new LocalText("Water Veil");
                ability.Desc = new LocalText("The Pokémon is covered with a water veil, which prevents the Pokémon from getting a burn.");
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("burn", new StringKey("MSG_WATER_VEIL")));
            }
            else if (ii == 42)
            {
                ability.Name = new LocalText("Magnet Pull");
                ability.Desc = new LocalText("Pulls Steel-type targets to this Pokémon.");
                //ability.AfterBeingHits.Add(0, new HitCounterEvent(Alignment.Foe | Alignment.Friend, false, false, true, 100, new CharElementNeededEvent("steel", new WarpHereEvent(new StringKey("MSG_MAGNET_PULL"), false))));
                ability.AfterHittings.Add(5, new OnHitEvent(false, false, 100, new CharElementNeededEvent("steel", new WarpHereEvent(new StringKey("MSG_MAGNET_PULL"), true))));
            }
            else if (ii == 43)
            {
                ability.Name = new LocalText("Soundproof");
                ability.Desc = new LocalText("Soundproofing of the Pokémon itself gives full immunity to all sound-based moves.");
                ability.BeforeBeingHits.Add(0, new EvadeMoveStateEvent(typeof(SoundState), new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 44)
            {
                ability.Name = new LocalText("Rain Dish");
                ability.Desc = new LocalText("The Pokémon gradually regains HP when battling in rain.");
                ability.AfterActions.Add(0, new WeatherNeededEvent("rain", new OnMoveUseEvent(new RestoreHPEvent(1, 8, false))));
            }
            else if (ii == 45)
            {
                ability.Name = new LocalText("Sand Stream");
                ability.Desc = new LocalText("The Pokémon summons a sandstorm when it is battling.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent("sandstorm", 10, new StringKey("MSG_SAND_STREAM"), typeof(ExtendWeatherState))));
            }
            else if (ii == 46)
            {
                ability.Name = new LocalText("Pressure");
                ability.Desc = new LocalText("By putting pressure on the opposing Pokémon, it raises their PP usage.");
                ability.AfterBeingHits.Add(0, new PressureEvent(1));
            }
            else if (ii == 47)
            {
                ability.Name = new LocalText("Thick Fat");
                ability.Desc = new LocalText("Boosts resistance to Fire- and Ice-type moves.");
                SingleEmitter emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));
                ability.BeforeBeingHits.Add(0, new MultiplyElementEvent("fire", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
                emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));
                ability.BeforeBeingHits.Add(0, new MultiplyElementEvent("ice", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 48)
            {
                ability.Name = new LocalText("Early Bird");
                ability.Desc = new LocalText("The Pokémon awakens quickly from sleep.");
                ability.OnTurnEnds.Add(0, new EarlyBirdEvent("sleep"));
            }
            else if (ii == 49)
            {
                ability.Name = new LocalText("Flame Body");
                ability.Desc = new LocalText("Contact with the Pokémon may burn the attacker. Thrown items will also burn up on contact.");
                SingleEmitter endEmitter = new SingleEmitter(new AnimData("Burned", 3));
                endEmitter.LocHeight = 8;
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 35, new StatusBattleEvent("burn", false, true, false, new StringKey("MSG_FLAME_BODY"),
                    new BattleAnimEvent(endEmitter, "DUN_Flamethrower_3", false, 0))));
                ability.ProximityEvent.Radius = 0;
                ability.ProximityEvent.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                ability.ProximityEvent.BeforeExplosions.Add(0, new DampItemEvent());

                BattleData catchData = new BattleData();
                catchData.Element = "none";
                SingleEmitter emitter = new SingleEmitter(new AnimData("Ember", 3));
                emitter.LocHeight = 8;
                catchData.HitFX.Emitter = emitter;
                catchData.HitFX.Sound = "DUN_Ember_2";
                ability.BeforeBeingHits.Add(0, new ThrowItemDestroyEvent(catchData));
                ability.OnRefresh.Add(0, new ThrownItemBarrierEvent());
            }
            else if (ii == 50)
            {
                ability.Name = new LocalText("Run Away");
                ability.Desc = new LocalText("Prevents the Pokémon from being Immobilized.");
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("immobilized", new StringKey("MSG_RUN_AWAY")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("wrap", new StringKey("MSG_RUN_AWAY")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("bind", new StringKey("MSG_RUN_AWAY")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("fire_spin", new StringKey("MSG_RUN_AWAY")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("whirlpool", new StringKey("MSG_RUN_AWAY")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("sand_tomb", new StringKey("MSG_RUN_AWAY")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("telekinesis", new StringKey("MSG_RUN_AWAY")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("clamp", new StringKey("MSG_RUN_AWAY")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("infestation", new StringKey("MSG_RUN_AWAY")));
            }
            else if (ii == 51)
            {
                ability.Name = new LocalText("Keen Eye");
                ability.Desc = new LocalText("Prevents other Pokémon from lowering its Accuracy.");
                List<Stat> drops = new List<Stat>();
                drops.Add(Stat.HitRate);
                ability.BeforeStatusAdds.Add(0, new StatChangeCheck(drops, new StringKey("MSG_STAT_DROP_PROTECT"), true, false, false));
                ability.BeforeHittings.Add(0, new IgnoreHaxEvent());
            }
            else if (ii == 52)
            {
                ability.Name = new LocalText("Hyper Cutter");
                ability.Desc = new LocalText("The Pokémon's proud of its powerful pincers. They prevent other Pokémon from lowering its Attack stat.");
                List<Stat> drops = new List<Stat>();
                drops.Add(Stat.Attack);
                ability.BeforeStatusAdds.Add(0, new StatChangeCheck(drops, new StringKey("MSG_STAT_DROP_PROTECT"), true, false, false));
            }
            else if (ii == 53)
            {
                ability.Name = new LocalText("Pickup");
                ability.Desc = new LocalText("The Pokémon may find an item when it enters a new floor.");
                ability.OnMapStarts.Add(0, new PickupEvent(50, new AnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 2)), "")));
            }
            else if (ii == 54)
            {
                ability.Name = new LocalText("Truant");
                ability.Desc = new LocalText("The Pokémon can't attack on consecutive turns.");
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(2));
                ability.AfterActions.Add(0, new OnMoveUseEvent(new StatusStateBattleEvent("paused", false, true, statusStates)));
            }
            else if (ii == 55)
            {
                ability.Name = new LocalText("Hustle");
                ability.Desc = new LocalText("Boosts the Attack stat, but lowers Attack Range.");
                ability.OnActions.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Physical, 4, 3));
                ability.OnActions.Add(-1, new AddRangeEvent(-1));
            }
            else if (ii == 56)
            {
                ability.Name = new LocalText("Cute Charm");
                ability.Desc = new LocalText("Contact with the Pokémon may cause infatuation.");
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Charm", 1));
                endAnim.LocHeight = 16;
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 35, new StatusBattleEvent("in_love", false, true, false, new StringKey("MSG_CUTE_CHARM"),
                    new BattleAnimEvent(endAnim, "DUN_Charm", false, 20))));
            }
            else if (ii == 57)
            {
                ability.Name = new LocalText("Plus");
                ability.Desc = new LocalText("Boosts the Sp. Atk stat if another Pokémon has Minus.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = Alignment.Friend;
                ability.ProximityEvent.OnActions.Add(0, new SupportAbilityEvent("minus"));
            }
            else if (ii == 58)
            {
                ability.Name = new LocalText("Minus");
                ability.Desc = new LocalText("Boosts the Sp. Atk stat if another Pokémon has Plus.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = Alignment.Friend;
                ability.ProximityEvent.OnActions.Add(0, new SupportAbilityEvent("plus"));
            }
            else if (ii == 59)
            {
                ability.Name = new LocalText("Forecast");
                ability.Desc = new LocalText("The Pokémon transforms with the weather to change its type to Water, Fire, or Ice.");
                {
                    Dictionary<string, int> weather = new Dictionary<string, int>();
                    weather.Add("rain", 2);
                    weather.Add("sunny", 1);
                    weather.Add("hail", 3);
                    ability.OnMapStatusAdds.Add(0, new WeatherFormeChangeEvent("castform", 0, weather));
                }
                {
                    Dictionary<string, int> weather = new Dictionary<string, int>();
                    weather.Add("rain", 2);
                    weather.Add("sunny", 1);
                    weather.Add("hail", 3);
                    ability.OnMapStatusRemoves.Add(0, new WeatherFormeChangeEvent("castform", 0, weather));
                }
                {
                    Dictionary<string, int> weather = new Dictionary<string, int>();
                    weather.Add("rain", 2);
                    weather.Add("sunny", 1);
                    weather.Add("hail", 3);
                    ability.OnMapStarts.Add(-11, new WeatherFormeEvent("castform", 0, weather));
                }
            }
            else if (ii == 60)
            {
                ability.Name = new LocalText("Sticky Hold");
                ability.Desc = new LocalText("Protects the Pokémon from item theft. Also allows the Pokémon to handle Sticky items.");
                ability.OnRefresh.Add(0, new NoStickItemEvent());
                ability.OnRefresh.Add(0, new MiscEvent(new StickyHoldState()));
            }
            else if (ii == 61)
            {
                ability.Name = new LocalText("Shed Skin");
                ability.Desc = new LocalText("The Pokémon may heal its own status conditions by shedding its skin.");
                ability.OnTurnEnds.Add(0, new ChanceEvent(35, new CureAllEvent(new StringKey("MSG_CURE_SELF"), new AnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Wonder_Tile", 10))));
            }
            else if (ii == 62)
            {
                ability.Name = new LocalText("Guts");
                ability.Desc = new LocalText("It's so gutsy that having a major status condition boosts the Pokémon's Attack stat.");
                ability.OnActions.Add(0, new MultiplyCategoryInMajorStatusEvent(BattleData.SkillCategory.Physical, 3, 2, false));
                ability.OnActions.Add(0, new MultiplyCategoryInStatusEvent("burn", BattleData.SkillCategory.Physical, 3, 2, false));
            }
            else if (ii == 63)
            {
                ability.Name = new LocalText("Marvel Scale");
                ability.Desc = new LocalText("The Pokémon's marvelous scales boost the Defense stat if it has a major status condition.");
                ability.BeforeBeingHits.Add(0, new MultiplyCategoryInMajorStatusEvent(BattleData.SkillCategory.Physical, 1, 2, true));
            }
            else if (ii == 64)
            {
                ability.Name = new LocalText("Liquid Ooze");
                ability.Desc = new LocalText("Oozed liquid has strong stench, which damages attackers using any draining move.");
                ability.AfterBeingHits.Add(0, new AddContextStateEvent(new TaintedDrain(4), true));
                ability.OnRefresh.Add(0, new MiscEvent(new DrainDamageState(4)));
            }
            else if (ii == 65)
            {
                ability.Name = new LocalText("Overgrow");
                ability.Desc = new LocalText("Powers up Grass-type moves when the Pokémon's HP is low.");
                ability.OnActions.Add(0, new PinchEvent("grass"));
            }
            else if (ii == 66)
            {
                ability.Name = new LocalText("Blaze");
                ability.Desc = new LocalText("Powers up Fire-type moves when the Pokémon's HP is low.");
                ability.OnActions.Add(0, new PinchEvent("fire"));
            }
            else if (ii == 67)
            {
                ability.Name = new LocalText("Torrent");
                ability.Desc = new LocalText("Powers up Water-type moves when the Pokémon's HP is low.");
                ability.OnActions.Add(0, new PinchEvent("water"));
            }
            else if (ii == 68)
            {
                ability.Name = new LocalText("Swarm");
                ability.Desc = new LocalText("Powers up Bug-type moves when the Pokémon's HP is low.");
                ability.OnActions.Add(0, new PinchEvent("bug"));
            }
            else if (ii == 69)
            {
                ability.Name = new LocalText("Rock Head");
                ability.Desc = new LocalText("Protects the Pokémon from recoil damage.");
                ability.OnRefresh.Add(0, new MiscEvent(new NoRecoilState()));
            }
            else if (ii == 70)
            {
                ability.Name = new LocalText("Drought");
                ability.Desc = new LocalText("Turns the sunlight harsh when the Pokémon is battling.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent("sunny", 10, new StringKey("MSG_DROUGHT"), typeof(ExtendWeatherState))));
            }
            else if (ii == 71)
            {
                ability.Name = new LocalText("Arena Trap");
                ability.Desc = new LocalText("Prevents grounded opposing Pokémon from fleeing.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent(Alignment.Foe, false, false, false, 100, new CheckImmunityBattleEvent("ground", false, new StatusBattleEvent("immobilized", false, true, false, new StringKey("MSG_ARENA_TRAP")))));
                //ability.AfterHittings.Add(0, new OnHitEvent(false, false, 100, new CheckImmunityBattleEvent("ground", true, new StatusBattleEvent("immobilized", true, true, false, new StringKey("MSG_ARENA_TRAP")))));
            }
            else if (ii == 72)
            {
                ability.Name = new LocalText("Vital Spirit");
                ability.Desc = new LocalText("The Pokémon is full of vitality, and that prevents it from falling asleep.");
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("sleep", new StringKey("MSG_VITAL_SPIRIT")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck("yawning", new StringKey("MSG_VITAL_SPIRIT_DROWSY")));
            }
            else if (ii == 73)
            {
                ability.Name = new LocalText("White Smoke");
                ability.Desc = new LocalText("The Pokémon is protected by its white smoke, which prevents other Pokémon from lowering its stats.");
                ability.BeforeStatusAdds.Add(0, new StatChangeCheck(new List<Stat>(), new StringKey("MSG_STAT_DROP_PROTECT"), true, false, false));
            }
            else if (ii == 74)
            {
                ability.Name = new LocalText("Pure Power");
                ability.Desc = new LocalText("Boosts the Pokémon's Attack stat.");
                ability.OnActions.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Physical, 3, 2));
            }
            else if (ii == 75)
            {
                ability.Name = new LocalText("Shell Armor");
                ability.Desc = new LocalText("A hard shell protects the Pokémon from critical hits.");
                ability.BeforeBeingHits.Add(0, new BlockCriticalEvent());
            }
            else if (ii == 76)
            {
                ability.Name = new LocalText("Air Lock");
                ability.Desc = new LocalText("Eliminates the effects of weather when the Pokémon is battling.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent("clear", 10, new StringKey("MSG_AIR_LOCK"), typeof(ExtendWeatherState))));
            }
            else if (ii == 77)
            {
                ability.Name = new LocalText("Tangled Feet");
                ability.Desc = new LocalText("If the Pokémon is confused, it will avoid attacks from a distance.");
                ability.BeforeBeingHits.Add(0, new EvadeInStatusEvent("confuse"));
            }
            else if (ii == 78)
            {
                ability.Name = new LocalText("Motor Drive");
                ability.Desc = new LocalText("Boosts the Pokémon's Movement Speed when it's hit by an Electric-type move.");
                ability.ProximityEvent.Radius = 0;
                ability.ProximityEvent.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                ability.ProximityEvent.BeforeExplosions.Add(0, new IsolateElementEvent("electric"));
                ability.BeforeBeingHits.Add(5, new AbsorbElementEvent("electric", false, true,
                    new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Drink",
                    new StatusStackBattleEvent("mod_speed", true, false, 1)));
            }
            else if (ii == 79)
            {
                ability.Name = new LocalText("Rivalry");
                //do more damage if an ally is the same gender?
                //do more damage per enemy of the same gender?
                ability.Desc = new LocalText("Deals more damage to Pokémon of the same gender.");
                ability.BeforeHittings.Add(0, new RivalryEvent());
            }
            else if (ii == 80)
            {
                ability.Name = new LocalText("Steadfast");
                ability.Desc = new LocalText("Boosts the Pokémon's Movement Speed each time the it flinches.");
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new StackState(1));
                ability.OnStatusAdds.Add(0, new StatusResponseEvent("flinch", new GiveStatusEvent("mod_speed", statusStates, false, new StringKey("MSG_STEADFAST"))));
            }
            else if (ii == 81)
            {
                ability.Name = new LocalText("Snow Cloak");
                ability.Desc = new LocalText("Avoids attacks from a distance when in a hailstorm.");
                ability.OnRefresh.Add(0, new MiscEvent(new HailState()));
                ability.BeforeBeingHits.Add(0, new WeatherNeededEvent("hail", new EvadeDistanceEvent()));
            }
            else if (ii == 82)
            {
                ability.Name = new LocalText("Gluttony");
                ability.Desc = new LocalText("Steals and eats a food item from an attacker that made direct contact.");
                HashSet<FlagType> eligibles = new HashSet<FlagType>();
                eligibles.Add(new FlagType(typeof(EdibleState)));
                ability.AfterBeingHits.Add(0, new HitCounterEvent(Alignment.Foe, true, true, true, 100, new UseFoeItemEvent(true, false, "seed_decoy", eligibles, false, true)));
            }
            else if (ii == 83)
            {
                ability.Name = new LocalText("Anger Point");
                ability.Desc = new LocalText("The Pokémon is angered when it takes a critical hit, and that maxes its Attack stat.");
                ability.AfterBeingHits.Add(0, new CritNeededEvent(new StatusStackBattleEvent("mod_attack", true, true, false, 12, new StringKey("MSG_ANGER_POINT"))));
            }
            else if (ii == 84)
            {
                ability.Name = new LocalText("Unburden");
                ability.Desc = new LocalText("Boosts Movement Speed if the Pokémon has no held item.");
                ability.OnRefresh.Add(0, new UnburdenEvent());
            }
            else if (ii == 85)
            {
                ability.Name = new LocalText("Heatproof");
                ability.Desc = new LocalText("Weakens the power of Fire-type moves.");
                SingleEmitter emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));
                ability.BeforeBeingHits.Add(0, new MultiplyElementEvent("fire", 1, 4, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
                ability.OnRefresh.Add(0, new MiscEvent(new HeatproofState()));
            }
            else if (ii == 86)
            {
                ability.Name = new LocalText("Simple");
                ability.Desc = new LocalText("The stat changes the Pokémon receives are doubled.");
                ability.BeforeStatusAdds.Add(-1, new StatusStackMod(2));
            }
            else if (ii == 87)
            {
                ability.Name = new LocalText("Dry Skin");
                ability.Desc = new LocalText("Restores HP in rain or when hit by Water-type moves. Reduces HP in harsh sunlight, and increases the damage received from Fire-type moves.");
                ability.ProximityEvent.Radius = 0;
                ability.ProximityEvent.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                ability.ProximityEvent.BeforeExplosions.Add(0, new IsolateElementEvent("water"));
                ability.BeforeBeingHits.Add(5, new AbsorbElementEvent("water", false, true,
                    new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Drink", new RestoreHPEvent(1, 4, true)));
                ability.BeforeBeingHits.Add(0, new MultiplyElementEvent("fire", 3, 2, false));
                ability.OnMapTurnEnds.Add(0, new WeatherAlignedEvent("sunny", "rain"));
            }
            else if (ii == 88)
            {
                ability.Name = new LocalText("Download");
                ability.Desc = new LocalText("Compares an opposing Pokémon's Defense and Sp. Def stats before raising its own Attack or Sp. Atk stat- whichever will be more effective.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent(Alignment.Foe, true, false, true, 100, new DownloadEvent("mod_attack", "mod_special_attack")));
            }
            else if (ii == 89)
            {
                ability.Name = new LocalText("Iron Fist");
                ability.Desc = new LocalText("Powers up punching moves.");
                ability.OnActions.Add(0, new MultiplyMoveStateEvent(typeof(FistState), 5, 4));
            }
            else if (ii == 90)
            {
                ability.Name = new LocalText("Poison Heal");
                ability.Desc = new LocalText("Restores HP if the Pokémon is poisoned, instead of losing HP.");
                ability.OnRefresh.Add(0, new MiscEvent(new PoisonHealState()));
            }
            else if (ii == 91)
            {
                ability.Name = new LocalText("Adaptability");
                ability.Desc = new LocalText("Powers up moves of the same type as the Pokémon.");
                ability.OnActions.Add(0, new AdaptabilityEvent());
            }
            else if (ii == 92)
            {
                ability.Name = new LocalText("Skill Link");
                ability.Desc = new LocalText("Ensures that all multi-strike moves hit.");
                ability.OnActions.Add(0, new SkillLinkEvent());
            }
            else if (ii == 93)
            {
                ability.Name = new LocalText("Hydration");
                ability.Desc = new LocalText("Heals status conditions if it's raining.");
                ability.OnTurnEnds.Add(0, new WeatherNeededSingleEvent("rain", new CureAllEvent(new StringKey("MSG_CURE_SELF"), new AnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Wonder_Tile", 10))));
            }
            else if (ii == 94)
            {
                ability.Name = new LocalText("Solar Power");
                ability.Desc = new LocalText("Boosts the Sp. Atk stat in sunny weather, but HP decreases.");
                ability.OnActions.Add(0, new WeatherNeededEvent("sunny", new MultiplyCategoryEvent(BattleData.SkillCategory.Magical, 3, 2)));
                ability.AfterActions.Add(0, new WeatherNeededEvent("sunny", new OnAggressionEvent(new ChipDamageEvent(12, new StringKey(), true, true))));
            }
            else if (ii == 95)
            {
                ability.Name = new LocalText("Quick Feet");
                ability.Desc = new LocalText("Boosts Movement Speed if the Pokémon has a status condition.");
                ability.OnRefresh.Add(5, new StatusSpeedEvent());
            }
            else if (ii == 96)
            {
                ability.Name = new LocalText("Normalize");
                ability.Desc = new LocalText("All the Pokémon’s moves become Normal type.");
                ability.OnActions.Add(-1, new ChangeMoveElementEvent("none", "normal"));
            }
            else if (ii == 97)
            {
                ability.Name = new LocalText("Sniper");
                ability.Desc = new LocalText("Powers up moves if they become critical hits when attacking.");
                ability.OnRefresh.Add(0, new MiscEvent(new SnipeState()));
            }
            else if (ii == 98)
            {
                ability.Name = new LocalText("Magic Guard");
                ability.Desc = new LocalText("The Pokémon only takes damage from attacks.");
                ability.OnRefresh.Add(0, new MiscEvent(new MagicGuardState()));
            }
            else if (ii == 99)
            {
                ability.Name = new LocalText("No Guard");
                ability.Desc = new LocalText("The Pokémon employs no-guard tactics to ensure incoming and outgoing attacks always land.");
                ability.BeforeHittings.Add(-1, new SureShotEvent());
                ability.BeforeBeingHits.Add(-1, new SureShotEvent());
            }
            else if (ii == 100)
            {
                ability.Name = new LocalText("Stall");
                ability.Desc = new LocalText("Boosts move power if the Pokémon was last hit by the target.");
                ability.BeforeHittings.Add(0, new RevengeEvent("last_targeted_by", 5, 4, false, false));
            }
            else if (ii == 101)
            {
                ability.Name = new LocalText("Technician");
                ability.Desc = new LocalText("Powers up the Pokémon's weaker moves.");
                ability.OnActions.Add(0, new TechnicianEvent());
                //ability.OnActions.Add(0, new OnMoveUseEvent(new TechnicianEvent()));
            }
            else if (ii == 102)
            {
                ability.Name = new LocalText("Leaf Guard");
                ability.Desc = new LocalText("Avoids enemy status moves in sunny weather.");
                ability.BeforeBeingHits.Add(0, new WeatherNeededEvent("sunny", new EvadeCategoryEvent(Alignment.Foe, BattleData.SkillCategory.Status)));
            }
            else if (ii == 103)
            {
                ability.Name = new LocalText("Klutz");
                ability.Desc = new LocalText("Items held by the Pokémon have no effect. They also can't stick to the Pokémon.");
                ability.OnRefresh.Add(0, new NoStickItemEvent());
                ability.OnRefresh.Add(-5, new NoHeldItemEvent());
            }
            else if (ii == 104)
            {
                ability.Name = new LocalText("Mold Breaker");
                ability.Desc = new LocalText("Damaging moves will remove the abilities of opposing Pokémon.");
                ability.BeforeHittings.Add(0, new AttackingMoveNeededEvent(new TargetNeededEvent(Alignment.Friend | Alignment.Foe, new ChangeToAbilityEvent("none", true, true))));
            }
            else if (ii == 105)
            {
                ability.Name = new LocalText("Super Luck");
                ability.Desc = new LocalText("The Pokémon is so lucky that the critical-hit ratios of its moves are boosted.");
                ability.OnActions.Add(0, new BoostCriticalEvent(1));
            }
            else if (ii == 106)
            {
                ability.Name = new LocalText("Aftermath");
                ability.Desc = new LocalText("The Pokémon explodes when fainted.");

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
                emitter.ParticlesPerTile = 0.7;
                altExplosion.Emitter = emitter;
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHits.Add(-1, new MaxHPDamageEvent(4));
                newData.OnHitTiles.Add(0, new RemoveTrapEvent());
                newData.OnHitTiles.Add(0, new RemoveItemEvent(true));
                newData.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState)), new FlagType(typeof(FoliageTerrainState))));
                ability.OnDeaths.Add(-1, new InvokeAttackEvent(altAction, altExplosion, newData, new StringKey("MSG_EXPLODE")));
            }
            else if (ii == 107)
            {
                ability.Name = new LocalText("Anticipation");
                ability.Desc = new LocalText("Avoids the strongest super-effective move that the opposing Pokémon has.");
                ability.BeforeBeingHits.Add(0, new EvadeStrongestEffectiveEvent());
            }
            else if (ii == 108)
            {
                ability.Name = new LocalText("Forewarn");
                ability.Desc = new LocalText("Avoids the strongest move that the opposing Pokémon has.");
                ability.BeforeBeingHits.Add(0, new EvadeStrongestEvent());
            }
            else if (ii == 109)
            {
                ability.Name = new LocalText("Unaware");
                ability.Desc = new LocalText("Ignores the opposing Pokémon's stat changes.");
                ability.BeforeHittings.Add(4, new IgnoreStatsEvent(true));
            }
            else if (ii == 110)
            {
                ability.Name = new LocalText("Tinted Lens");
                ability.Desc = new LocalText("Powers up \"not very effective\" moves.");
                ability.BeforeHittings.Add(0, new MultiplyEffectiveEvent(true, 3, 2));
            }
            else if (ii == 111)
            {
                ability.Name = new LocalText("Filter");
                ability.Desc = new LocalText("Reduces damage from super-effective attacks.");
                ability.BeforeBeingHits.Add(0, new MultiplyEffectiveEvent(false, 2, 3, new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 112)
            {
                ability.Name = new LocalText("Slow Start");
                ability.Desc = new LocalText("Temporarily lowers the Pokémon's Movement Speed.");
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new StackState(-1));
                ability.OnMapStarts.Add(0, new GiveStatusEvent("mod_speed", statusStates, false, new StringKey("MSG_SLOW_START")));
            }
            else if (ii == 113)
            {
                ability.Name = new LocalText("Scrappy");
                ability.Desc = new LocalText("The Pokémon can hit Ghost-type Pokémon with Normal- and Fighting-type moves.");
                ability.UserElementEffects.Add(0, new ScrappyEvent("normal", "fighting"));
            }
            else if (ii == 114)
            {
                ability.Name = new LocalText("Storm Drain");
                ability.Desc = new LocalText("Draws in all Water-type moves. Instead of being hit by Water-type moves, it boosts its Sp. Atk.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = Alignment.Foe;
                ability.ProximityEvent.BeforeExplosions.Add(0, new DrawAttackEvent(Alignment.Friend, "water", new StringKey("MSG_DRAW_ATTACK")));
                ability.BeforeBeingHits.Add(5, new AbsorbElementEvent("water", true, new StatusStackBattleEvent("mod_special_attack", true, false, 1)));
                ability.BeforeBeingHits.Add(5, new AddContextStateEvent(new SingleDrawAbsorb(), true));
            }
            else if (ii == 115)
            {
                ability.Name = new LocalText("Ice Body");
                ability.Desc = new LocalText("The Pokémon gradually regains HP when battling in a hailstorm.");
                ability.OnRefresh.Add(0, new MiscEvent(new HailState()));
                ability.AfterActions.Add(0, new WeatherNeededEvent("hail", new OnMoveUseEvent(new RestoreHPEvent(1, 8, false))));
            }
            else if (ii == 116)
            {
                ability.Name = new LocalText("Solid Rock");
                ability.Desc = new LocalText("Reduces damage from super-effective attacks.");
                ability.BeforeBeingHits.Add(0, new MultiplyEffectiveEvent(false, 2, 3, new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 117)
            {
                ability.Name = new LocalText("Snow Warning");
                ability.Desc = new LocalText("The Pokémon summons a hailstorm when it is battling.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent("hail", 10, new StringKey("MSG_SNOW_WARNING"), typeof(ExtendWeatherState))));
            }
            else if (ii == 118)
            {
                ability.Name = new LocalText("Honey Gather");
                ability.Desc = new LocalText("The Pokémon may gather Nectar when it enters a new floor.");
                ability.OnMapStarts.Add(0, new GatherEvent(new List<string> { "boost_nectar" }, 50, new AnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 2)), "")));
            }
            else if (ii == 119)
            {
                ability.Name = new LocalText("Frisk");
                ability.Desc = new LocalText("Attacking an opposing Pokémon removes its held item.");
                ability.AfterHittings.Add(0, new OnHitEvent(true, false, 100, new DropItemEvent(false, true, "", new HashSet<FlagType>(), new StringKey("MSG_FRISK"), true)));
            }
            else if (ii == 120)
            {
                ability.Name = new LocalText("Reckless");
                ability.Desc = new LocalText("Powers up moves that have recoil damage.");
                ability.OnActions.Add(0, new MultiplyRecklessEvent(5, 4));
            }
            else if (ii == 121)
            {
                ability.Name = new LocalText("Multitype");
                ability.Desc = new LocalText("Changes the Pokémon's type to match its held Plate. The Pokémon can also utilize the protection of Plates in its Bag.");
                //ability.OnActions.Add(0, new ConversionEffect(false));
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
                ability.OnTurnStarts.Add(0, new PlateElementEvent(plate));
                Dictionary<string, string> plate2 = new Dictionary<string, string>();
                foreach (string key in plate.Keys)
                    plate2[key] = plate[key];
                ability.OnTurnEnds.Add(0, new PlateElementEvent(plate2));
                Dictionary<string, string> element = new Dictionary<string, string>();
                foreach (string key in plate.Keys)
                    element[plate[key]] = key;
                ability.BeforeBeingHits.Add(-1, new PlateProtectEvent(element));
            }
            else if (ii == 122)
            {
                ability.Name = new LocalText("Flower Gift");
                ability.Desc = new LocalText("Boosts the Attack and Sp. Def stats of itself and allies in harsh sunlight.");
                {
                    Dictionary<string, int> weather = new Dictionary<string, int>();
                    weather.Add("sunny", 1);
                    ability.OnMapStatusAdds.Add(0, new WeatherFormeChangeEvent("cherrim", 0, weather));
                }
                {
                    Dictionary<string, int> weather = new Dictionary<string, int>();
                    weather.Add("sunny", 1);
                    ability.OnMapStatusRemoves.Add(0, new WeatherFormeChangeEvent("cherrim", 0, weather));
                }
                {
                    Dictionary<string, int> weather = new Dictionary<string, int>();
                    weather.Add("sunny", 1);
                    ability.OnMapStarts.Add(-11, new WeatherFormeEvent("cherrim", 0, weather));
                }
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = (Alignment.Self | Alignment.Friend);
                ability.ProximityEvent.OnActions.Add(0, new MultiplyCategoryInWeatherEvent("sunny", BattleData.SkillCategory.Physical, 5, 4));
                ability.ProximityEvent.BeforeBeingHits.Add(0, new MultiplyCategoryInWeatherEvent("sunny", BattleData.SkillCategory.Magical, 4, 5));
            }
            else if (ii == 123)
            {
                ability.Name = new LocalText("Bad Dreams");
                ability.Desc = new LocalText("Reduces the HP of opposing Pokémon that are asleep.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = Alignment.Foe;
                ability.ProximityEvent.OnTurnEnds.Add(0, new NightmareEvent("sleep", 8, new StringKey("MSG_HURT_BY_OTHER"), new AnimEvent(new SingleEmitter(new AnimData("Dark_Pulse_Ranger", 3)), "DUN_Night_Shade", 0)));
            }
            else if (ii == 124)
            {
                ability.Name = new LocalText("Pickpocket");
                ability.Desc = new LocalText("Steals an item from an attacker that made direct contact.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent(Alignment.Foe, true, true, true, 100, new StealItemEvent(true, false, "seed_decoy", new HashSet<FlagType>(), new StringKey("MSG_STEAL_WITH"), false, true)));
            }
            else if (ii == 125)
            {
                ability.Name = new LocalText("Sheer Force");
                ability.Desc = new LocalText("Removes additional effects to increase the power of moves when attacking.");
                ability.OnActions.Add(0, new SheerForceEvent());
            }
            else if (ii == 126)
            {
                ability.Name = new LocalText("Contrary");
                ability.Desc = new LocalText("Makes stat changes have an opposite effect.");
                ability.BeforeStatusAdds.Add(-1, new StatusStackMod(-1));
            }
            else if (ii == 127)
            {
                ability.Name = new LocalText("Unnerve");
                ability.Desc = new LocalText("Unnerves opposing Pokémon and makes them unable to eat food items.");
                ability.ProximityEvent.Radius = 5;
                ability.ProximityEvent.TargetAlignments = Alignment.Foe;
                ability.ProximityEvent.BeforeTryActions.Add(0, new PreventItemUseEvent(new StringKey("MSG_UNNERVE"), new FlagType(typeof(EdibleState))));
                ability.ProximityEvent.BeforeActions.Add(0, new PreventItemUseEvent(new StringKey("MSG_UNNERVE"), new FlagType(typeof(EdibleState))));
                ability.ProximityEvent.BeforeBeingHits.Add(0, new DodgeFoodEvent(new StringKey("MSG_UNNERVE")));
            }
            else if (ii == 128)
            {
                ability.Name = new LocalText("Defiant");
                ability.Desc = new LocalText("Boosts the Pokémon's Attack stat sharply when its stats are lowered.");
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new StackState(2));
                ability.OnStatusAdds.Add(0, new StatDropResponseEvent(new GiveStatusEvent("mod_attack", statusStates, false)));
            }
            else if (ii == 129)
            {
                ability.Name = new LocalText("Defeatist");
                ability.Desc = new LocalText("Halves the Pokémon's Attack and Sp. Atk stats when its HP becomes half or less.");
                ability.OnActions.Add(0, new PinchNeededEvent(2, new MultiplyCategoryEvent(BattleData.SkillCategory.Physical, 1, 2)));
                ability.OnActions.Add(0, new PinchNeededEvent(2, new MultiplyCategoryEvent(BattleData.SkillCategory.Magical, 1, 2)));
            }
            else if (ii == 130)
            {
                ability.Name = new LocalText("Cursed Body");
                ability.Desc = new LocalText("May disable a move used on the Pokémon.");
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Spite", 3));
                endAnim.Layer = DrawLayer.Back;
                endAnim.LocHeight = 24;
                CounterDisableBattleEvent counterDisable = new CounterDisableBattleEvent("disable", new StringKey("MSG_CURSED_BODY"));
                counterDisable.Anims.Add(new BattleAnimEvent(endAnim, "DUN_Spite", false, 30));
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), true, false, false, 35, counterDisable));
            }
            else if (ii == 131)
            {
                ability.Name = new LocalText("Healer");
                ability.Desc = new LocalText("Heals the status conditions of nearby Pokémon when battling.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new HealSurroundingsEvent(new StringKey("MSG_HEALER"), new AnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Wonder_Tile", 10))));
            }
            else if (ii == 132)
            {
                ability.Name = new LocalText("Friend Guard");
                ability.Desc = new LocalText("Reduces damage done to nearby allies.");
                ability.ProximityEvent.Radius = 1;
                ability.ProximityEvent.TargetAlignments = Alignment.Friend;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));
                ability.ProximityEvent.BeforeBeingHits.Add(0, new MultiplyDamageEvent(3, 4, new MessageOnceEvent(new FriendGuardProcEvent(), false, true, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10), new StringKey())));
            }
            else if (ii == 133)
            {
                ability.Name = new LocalText("Weak Armor");
                ability.Desc = new LocalText("Physical attacks lower its Defense stat and raise its Movement Speed.");
                CategoryNeededEvent reqEffect = new CategoryNeededEvent(BattleData.SkillCategory.Physical);
                reqEffect.BaseEvents.Add(new RaiseOneLowerOneEvent("mod_speed", "mod_defense", new StringKey("MSG_WEAK_ARMOR")));
                ability.AfterBeingHits.Add(0, new HitCounterEvent(Alignment.Foe | Alignment.Friend, true, false, true, 100, reqEffect));
            }
            else if (ii == 134)
            {
                ability.Name = new LocalText("Heavy Metal");
                ability.Desc = new LocalText("Doubles the Pokémon's weight.");
                ability.OnRefresh.Add(0, new MiscEvent(new HeavyWeightState()));
            }
            else if (ii == 135)
            {
                ability.Name = new LocalText("Light Metal");
                ability.Desc = new LocalText("Halves the Pokémon's weight.");
                ability.OnRefresh.Add(0, new MiscEvent(new LightWeightState()));
            }
            else if (ii == 136)
            {
                ability.Name = new LocalText("Multiscale");
                ability.Desc = new LocalText("Reduces damage the Pokémon takes when its HP is full.");
                ability.BeforeBeingHits.Add(0, new MultiScaleEvent(new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 137)
            {
                ability.Name = new LocalText("Toxic Boost");
                ability.Desc = new LocalText("Powers up physical attacks when the Pokémon is poisoned.");
                ability.OnActions.Add(0, new MultiplyCategoryInStatusEvent("poison", BattleData.SkillCategory.Physical, 3, 2, false));
                ability.OnActions.Add(0, new MultiplyCategoryInStatusEvent("poison_toxic", BattleData.SkillCategory.Physical, 3, 2, false));
            }
            else if (ii == 138)
            {
                ability.Name = new LocalText("Flare Boost");
                ability.Desc = new LocalText("Powers up special attacks when the Pokémon is burned.");
                ability.OnActions.Add(0, new MultiplyCategoryInStatusEvent("burn", BattleData.SkillCategory.Magical, 3, 2, false));
            }
            else if (ii == 139)
            {
                ability.Name = new LocalText("Harvest");
                ability.Desc = new LocalText("Passes the effect of Berries to nearby allies.");
                RepeatEmitter emitter = new RepeatEmitter(new AnimData("Circle_Green_Out", 2));
                emitter.Bursts = 3;
                emitter.BurstTime = 8;
                ability.OnActions.Add(-3, new BerryAoEEvent(new StringKey("MSG_HARVEST"), emitter, "DUN_Me_First_2"));
            }
            else if (ii == 140)
            {
                ability.Name = new LocalText("Telepathy");
                ability.Desc = new LocalText("Anticipates an ally's attack and dodges it.");
                ability.BeforeBeingHits.Add(0, new TelepathyEvent());
            }
            else if (ii == 141)
            {
                ability.Name = new LocalText("Moody");
                ability.Desc = new LocalText("Raises one stat and lowers another.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new MoodyEvent("mod_attack", "mod_special_attack")));
                ability.AfterBeingHits.Add(0, new HitCounterEvent(Alignment.Foe | Alignment.Friend, true, false, true, 100, new MoodyEvent("mod_defense", "mod_special_defense")));
            }
            else if (ii == 142)
            {
                ability.Name = new LocalText("Overcoat");
                ability.Desc = new LocalText("Protects the Pokémon from the weather and splash damage.");
                ability.OnRefresh.Add(0, new MiscEvent(new SandState()));
                ability.OnRefresh.Add(0, new MiscEvent(new HailState()));

                SingleEmitter emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));
                ability.BeforeBeingHits.Add(0, new BlastProofEvent(1, -1, 1, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 143)
            {
                ability.Name = new LocalText("Poison Touch");
                ability.Desc = new LocalText("May poison a target when the Pokémon makes contact.");
                SqueezedAreaEmitter emitter = new SqueezedAreaEmitter(new AnimData("Bubbles_Purple", 3));
                emitter.BurstTime = 3;
                emitter.Bursts = 4;
                emitter.ParticlesPerBurst = 1;
                emitter.Range = 12;
                emitter.StartHeight = -4;
                emitter.HeightSpeed = 12;
                emitter.SpeedDiff = 4;
                ability.AfterHittings.Add(0, new OnHitEvent(true, true, 25, new StatusBattleEvent("poison", true, true, false, new StringKey("MSG_POISON_TOUCH"),
                    new BattleAnimEvent(emitter, "DUN_Toxic", true, 30))));
            }
            else if (ii == 144)
            {
                ability.Name = new LocalText("Regenerator");
                ability.Desc = new LocalText("Restores HP when there are no threats nearby.");
                ability.OnTurnEnds.Add(0, new RegeneratorEvent(7, 8));
            }
            else if (ii == 145)
            {
                ability.Name = new LocalText("Big Pecks");
                ability.Desc = new LocalText("Protects the Pokémon from Defense-lowering effects.");
                List<Stat> drops = new List<Stat>();
                drops.Add(Stat.Defense);
                ability.BeforeStatusAdds.Add(0, new StatChangeCheck(drops, new StringKey("MSG_STAT_DROP_PROTECT"), true, false, false));
            }
            else if (ii == 146)
            {
                ability.Name = new LocalText("Sand Rush");
                ability.Desc = new LocalText("Boosts the Pokémon's Movement Speed in a sandstorm.");
                ability.OnRefresh.Add(0, new MiscEvent(new SandState()));
                ability.OnRefresh.Add(0, new WeatherSpeedEvent("sandstorm"));
            }
            else if (ii == 147)
            {
                ability.Name = new LocalText("Wonder Skin");
                ability.Desc = new LocalText("Avoids all status moves targeting the Pokémon.");
                ability.BeforeBeingHits.Add(0, new EvadeCategoryEvent(Alignment.Friend | Alignment.Foe, BattleData.SkillCategory.Status, new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 148)
            {
                ability.Name = new LocalText("Analytic");
                ability.Desc = new LocalText("Boosts move power if the Pokémon was last hit by the target.");
                ability.BeforeHittings.Add(0, new RevengeEvent("last_targeted_by", 5, 4, false, false));
            }
            else if (ii == 149)
            {
                ability.Name = new LocalText("Illusion");
                ability.Desc = new LocalText("Disguises itself as another Pokémon, fooling wild Pokémon of the same species.");
                ability.OnMapStarts.Add(0, new GiveIllusionEvent("illusion"));
            }
            else if (ii == 150)
            {
                ability.Name = new LocalText("Imposter");
                ability.Desc = new LocalText("If defeated while transformed, the Pokémon reverts to its original form instead of fainting.");
                //effect is in Universal Effects
            }
            else if (ii == 151)
            {
                ability.Name = new LocalText("Infiltrator");
                ability.Desc = new LocalText("Passes through the opposing Pokémon's barrier and strikes.");
                ability.OnActions.Add(0, new AddContextStateEvent(new Infiltrator(new StringKey("MSG_INFILTRATOR"))));
                ability.BeforeStatusAddings.Add(-1, new AddStatusContextStateEvent(new Infiltrator(new StringKey("MSG_INFILTRATOR"))));
            }
            else if (ii == 152)
            {
                ability.Name = new LocalText("Mummy");
                ability.Desc = new LocalText("Contact with the Pokémon spreads this Ability.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 100, new ChangeToAbilityEvent("mummy", false, true)));
                ability.AfterHittings.Add(0, new OnHitEvent(true, true, 100, new ChangeToAbilityEvent("mummy", true, true)));
            }
            else if (ii == 153)
            {
                ability.Name = new LocalText("Moxie");
                ability.Desc = new LocalText("The Pokémon shows moxie, which may boost the Attack stat after knocking out any Pokémon with a move.");
                ability.AfterActions.Add(0, new KnockOutNeededEvent(new OnMoveUseEvent(new OnHitAnyEvent(true, 50, new StatusStackBattleEvent("mod_attack", false, true, false, 1, new StringKey("MSG_MOXIE"))))));
            }
            else if (ii == 154)
            {
                ability.Name = new LocalText("Justified");
                ability.Desc = new LocalText("Being hit by a Dark-type move boosts the Attack stat of the Pokémon, for justice.");
                ability.AfterBeingHits.Add(0, new OnMoveUseEvent(new ElementNeededEvent("dark", new StatusStackBattleEvent("mod_attack", true, true, false, 1, new StringKey("MSG_JUSTIFIED")))));
            }
            else if (ii == 155)
            {
                ability.Name = new LocalText("Rattled");
                ability.Desc = new LocalText("Dark-, Ghost-, and Bug-type moves scare the Pokémon and boost its Movement Speed.");
                ability.AfterBeingHits.Add(0, new ElementNeededEvent("dark", new StatusStackBattleEvent("mod_speed", true, true, false, 1, new StringKey("MSG_RATTLED"))));
                ability.AfterBeingHits.Add(0, new ElementNeededEvent("ghost", new StatusStackBattleEvent("mod_speed", true, true, false, 1, new StringKey("MSG_RATTLED"))));
                ability.AfterBeingHits.Add(0, new ElementNeededEvent("bug", new StatusStackBattleEvent("mod_speed", true, true, false, 1, new StringKey("MSG_RATTLED"))));
            }
            else if (ii == 156)
            {
                ability.Name = new LocalText("Magic Bounce");
                ability.Desc = new LocalText("Reflects moves that cause status conditions.");
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Green", 2, -1, -1, 192), 3);
                ability.BeforeBeingHits.Add(-3, new ExceptInfiltratorEvent(false, new BounceStatusEvent(new StringKey("MSG_MAGIC_BOUNCE"), new BattleAnimEvent(emitter, "DUN_Light_Screen", true, 30))));
            }
            else if (ii == 157)
            {
                ability.Name = new LocalText("Sap Sipper");
                ability.Desc = new LocalText("Boosts the Attack stat if hit by a Grass-type move, instead of taking damage.");
                ability.ProximityEvent.Radius = 0;
                ability.ProximityEvent.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                ability.ProximityEvent.BeforeExplosions.Add(0, new IsolateElementEvent("grass"));
                ability.BeforeBeingHits.Add(5, new AbsorbElementEvent("grass", false, true,
                    new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Drink", new StatusStackBattleEvent("mod_attack", true, false, 1)));
            }
            else if (ii == 158)
            {
                ability.Name = new LocalText("Prankster");
                ability.Desc = new LocalText("Increases the Attack Range of status moves.");
                ability.OnActions.Add(-1, new CategoryAddRangeEvent(BattleData.SkillCategory.Status, 2));
            }
            else if (ii == 159)
            {
                ability.Name = new LocalText("Sand Force");
                ability.Desc = new LocalText("Boosts the power of Rock-, Ground-, and Steel-type moves in a sandstorm.");
                ability.OnRefresh.Add(0, new MiscEvent(new SandState()));
                ability.OnActions.Add(0, new WeatherNeededEvent("sandstorm", new MultiplyElementEvent("rock", 4, 3, false),
                    new MultiplyElementEvent("ground", 4, 3, false), new MultiplyElementEvent("steel", 4, 3, false)));
            }
            else if (ii == 160)
            {
                ability.Name = new LocalText("Iron Barbs");
                ability.Desc = new LocalText("Inflicts damage to the attacker on contact with iron barbs.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 100, new ChipDamageEvent(8, new StringKey("MSG_HURT_BY_OTHER"), true, true)));
            }
            else if (ii == 161)
            {
                ability.Name = new LocalText("**Zen Mode");
                ability.Desc = new LocalText("Changes the Pokémon's shape when HP is half or less.");
            }
            else if (ii == 162)
            {
                ability.Name = new LocalText("Victory Star");
                ability.Desc = new LocalText("Boosts the Attack Range of party Pokémon.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = (Alignment.Self | Alignment.Friend);
                ability.ProximityEvent.OnActions.Add(-1, new AddRangeEvent(1));
            }
            else if (ii == 163)
            {
                ability.Name = new LocalText("**Turboblaze");
                ability.Desc = new LocalText("Damaging moves will remove the abilities of opposing Pokémon.");
            }
            else if (ii == 164)
            {
                ability.Name = new LocalText("**Teravolt");
                ability.Desc = new LocalText("Damaging moves will remove the abilities of opposing Pokémon.");
            }
            else if (ii == 165)
            {
                ability.Name = new LocalText("Aroma Veil");
                ability.Desc = new LocalText("Protects itself and its allies from attacks that limit their move choices.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = (Alignment.Self | Alignment.Friend);
                ability.ProximityEvent.BeforeStatusAdds.Add(0, new PreventStatusCheck("taunted", new StringKey("MSG_PROTECT_STATUS"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
                ability.ProximityEvent.BeforeStatusAdds.Add(0, new PreventStatusCheck("encore", new StringKey("MSG_PROTECT_STATUS"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
                ability.ProximityEvent.BeforeStatusAdds.Add(0, new PreventStatusCheck("torment", new StringKey("MSG_PROTECT_STATUS"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
                ability.ProximityEvent.BeforeStatusAdds.Add(0, new PreventStatusCheck("disable", new StringKey("MSG_PROTECT_STATUS"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
            }
            else if (ii == 166)
            {
                ability.Name = new LocalText("Flower Veil");
                ability.Desc = new LocalText("Protects ally Pokémon from major status conditions.");
                ability.ProximityEvent.Radius = 1;
                ability.ProximityEvent.TargetAlignments = Alignment.Friend;
                ability.ProximityEvent.BeforeStatusAdds.Add(0, new StateStatusCheck(typeof(MajorStatusState), new StringKey("MSG_PROTECT_STATUS"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
                //ability.ProximityEvent.BeforeStatusAdds.Add(0, new StatChangeCheck(new List<Stat>(), new StringKey("MSG_FLOWER_VEIL_STATS"), true, false, false, new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
            }
            else if (ii == 167)
            {
                ability.Name = new LocalText("Cheek Pouch");
                ability.Desc = new LocalText("Restores HP when the Pokémon eats a food item.");
                ability.AfterBeingHits.Add(0, new FoodNeededEvent(new RestoreHPEvent(1, 3, true)));
            }
            else if (ii == 168)
            {
                ability.Name = new LocalText("Protean");
                ability.Desc = new LocalText("Changes the Pokémon's type to the type of the move it last used.");
                ability.AfterActions.Add(-1, new ConversionEvent(false));
            }
            else if (ii == 169)
            {
                ability.Name = new LocalText("Fur Coat");
                ability.Desc = new LocalText("Halves damage from physical moves.");
                ability.BeforeBeingHits.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Physical, 1, 2, new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 170)
            {
                ability.Name = new LocalText("Magician");
                ability.Desc = new LocalText("The Pokémon steals the item of a Pokémon it hits with a move.");
                ability.AfterHittings.Add(0, new OnHitEvent(true, false, 100, new StealItemEvent(true, false, "seed_decoy", new HashSet<FlagType>(), new StringKey("MSG_STEAL_WITH"), true, true)));
            }
            else if (ii == 171)
            {
                ability.Name = new LocalText("Bulletproof");
                ability.Desc = new LocalText("Protects the Pokémon from explosions.");
                ability.ProximityEvent.Radius = 0;
                ability.ProximityEvent.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                ability.ProximityEvent.BeforeExplosions.Add(0, new DampEvent(0, new StringKey("MSG_NO_SPLASH_DAMAGE")));

                SingleEmitter emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));
                ability.BeforeBeingHits.Add(0, new BlastProofEvent(0, -1, 1, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 172)
            {
                ability.Name = new LocalText("Competitive");
                ability.Desc = new LocalText("Boosts the Sp. Atk stat sharply when a stat is lowered.");
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new StackState(2));
                ability.OnStatusAdds.Add(0, new StatDropResponseEvent(new GiveStatusEvent("mod_special_attack", statusStates, false)));
            }
            else if (ii == 173)
            {
                ability.Name = new LocalText("Strong Jaw");
                ability.Desc = new LocalText("The Pokémon's strong jaw boosts the power of its biting moves.");
                ability.OnActions.Add(0, new MultiplyMoveStateEvent(typeof(JawState), 5, 4));
            }
            else if (ii == 174)
            {
                ability.Name = new LocalText("Refrigerate");
                ability.Desc = new LocalText("Normal-type moves become Ice-type moves.");
                ability.OnActions.Add(-1, new ChangeMoveElementEvent("normal", "ice"));
            }
            else if (ii == 175)
            {
                ability.Name = new LocalText("Sweet Veil");
                ability.Desc = new LocalText("Prevents party Pokémon from falling asleep.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = (Alignment.Self | Alignment.Friend);
                ability.ProximityEvent.BeforeStatusAdds.Add(0, new PreventStatusCheck("sleep", new StringKey("MSG_SWEET_VEIL"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
                ability.ProximityEvent.BeforeStatusAdds.Add(0, new PreventStatusCheck("yawning", new StringKey("MSG_SWEET_VEIL_DROWSY"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
            }
            else if (ii == 176)
            {
                ability.Name = new LocalText("Stance Change");
                ability.Desc = new LocalText("The Pokémon changes its form to Blade Forme when it uses an attack move, and changes to Shield Forme when it uses King's Shield.");
                ability.AfterActions.Add(-1, new StanceChangeEvent("aegislash", "kings_shield", 0, 1));
            }
            else if (ii == 177)
            {
                ability.Name = new LocalText("**Gale Wings");
                ability.Desc = new LocalText("Increases the Attack Range of Flying-type moves.");
            }
            else if (ii == 178)
            {
                ability.Name = new LocalText("Mega Launcher");
                ability.Desc = new LocalText("Powers up aura and pulse moves.");
                ability.OnActions.Add(0, new MultiplyMoveStateEvent(typeof(PulseState), 5, 4));
            }
            else if (ii == 179)
            {
                ability.Name = new LocalText("Grass Pelt");
                ability.Desc = new LocalText("Boosts the Defense stat in Grassy Terrain.");
                ability.BeforeBeingHits.Add(0, new WeatherNeededEvent("grassy_terrain", new MultiplyCategoryEvent(BattleData.SkillCategory.Physical, 1, 2, new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", true, 10))));
            }
            else if (ii == 180)
            {
                ability.Name = new LocalText("Symbiosis");
                ability.Desc = new LocalText("Passes the effect of certain held items to ally Pokémon. It won't use the item itself.");
                ability.ProximityEvent.Radius = 1;
                ability.ProximityEvent.TargetAlignments = Alignment.Friend;
                ability.ProximityEvent.AfterActions.Add(0, new ShareAfterActionsEvent());
                ability.ProximityEvent.AfterBeingHits.Add(0, new ShareAfterBeingHitsEvent());
                ability.ProximityEvent.AfterHittings.Add(0, new ShareAfterHittingsEvent());
                ability.ProximityEvent.BeforeActions.Add(0, new ShareBeforeActionsEvent());
                ability.ProximityEvent.BeforeBeingHits.Add(0, new ShareBeforeBeingHitsEvent());
                ability.ProximityEvent.BeforeHittings.Add(0, new ShareBeforeHittingsEvent());
                ability.ProximityEvent.BeforeTryActions.Add(0, new ShareBeforeTryActionsEvent());
                ability.ProximityEvent.OnActions.Add(0, new ShareOnActionsEvent());
                ability.ProximityEvent.OnHitTiles.Add(0, new ShareOnHitTilesEvent());
                ability.ProximityEvent.OnTurnEnds.Add(0, new ShareOnTurnEndsEvent());
                ability.ProximityEvent.OnTurnStarts.Add(0, new ShareOnTurnStartsEvent());
                ability.ProximityEvent.OnDeaths.Add(0, new ShareOnDeathsEvent());
                ability.ProximityEvent.OnWalks.Add(0, new ShareOnWalksEvent());
                ability.ProximityEvent.OnMapTurnEnds.Add(0, new ShareOnMapTurnEndsEvent());
                ability.ProximityEvent.OnMapStarts.Add(0, new ShareOnMapStartsEvent());
                ability.ProximityEvent.BeforeStatusAdds.Add(0, new ShareBeforeStatusAddsEvent());
                ability.ProximityEvent.BeforeStatusAddings.Add(0, new ShareBeforeStatusAddingsEvent());
                ability.ProximityEvent.OnStatusAdds.Add(0, new ShareOnStatusAddsEvent());
                ability.ProximityEvent.OnStatusRemoves.Add(0, new ShareOnStatusRemovesEvent());
                ability.ProximityEvent.OnMapStatusAdds.Add(0, new ShareOnMapStatusAddsEvent());
                ability.ProximityEvent.OnMapStatusRemoves.Add(0, new ShareOnMapStatusRemovesEvent());
                ability.ProximityEvent.ModifyHPs.Add(0, new ShareModifyHPsEvent());
                ability.ProximityEvent.RestoreHPs.Add(0, new ShareRestoreHPsEvent());
                ability.ProximityEvent.TargetElementEffects.Add(0, new ShareTargetElementEvent());
                ability.ProximityEvent.UserElementEffects.Add(0, new ShareUserElementEvent());
                //ability.ProximityEvent.OnPickups.Add(0, new ShareEquipItemEvent());
                //ability.ProximityEvent.OnEquips.Add(0, new ShareEquipItemEvent());
                ////ability.ProximityEvent.OnRefresh.Add(0, new ShareEquipRefreshEvent());
                ability.OnEquips.Add(0, new CheckEquipPassValidityEvent());
                ability.OnRefresh.Add(-5, new NoHeldItemEvent());
            }
            else if (ii == 181)
            {
                ability.Name = new LocalText("Tough Claws");
                ability.Desc = new LocalText("Powers up moves that make direct contact.");
                ability.OnActions.Add(0, new MultiplyMoveStateEvent(typeof(ContactState), 5, 4));
            }
            else if (ii == 182)
            {
                ability.Name = new LocalText("Pixilate");
                ability.Desc = new LocalText("Normal-type moves become Fairy-type moves.");
                ability.OnActions.Add(-1, new ChangeMoveElementEvent("normal", "fairy"));
            }
            else if (ii == 183)
            {
                ability.Name = new LocalText("Gooey");
                ability.Desc = new LocalText("Contact with the Pokémon lowers the attacker's Movement Speed.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 50, new StatusStackBattleEvent("mod_speed", false, true, -1)));
            }
            else if (ii == 184)
            {
                ability.Name = new LocalText("Aerilate");
                ability.Desc = new LocalText("Normal-type moves become Flying-type moves.");
                ability.OnActions.Add(-1, new ChangeMoveElementEvent("normal", "flying"));
            }
            else if (ii == 185)
            {
                ability.Name = new LocalText("**Parental Bond");
                ability.Desc = new LocalText("Parent and child attack together.");
            }
            else if (ii == 186)
            {
                ability.Name = new LocalText("Dark Aura");
                ability.Desc = new LocalText("Powers up all Dark-type moves.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = (Alignment.Self | Alignment.Friend);
                ability.ProximityEvent.OnActions.Add(0, new MultiplyElementEvent("dark", 6, 5, false));
            }
            else if (ii == 187)
            {
                ability.Name = new LocalText("Fairy Aura");
                ability.Desc = new LocalText("Powers up all Fairy-type moves.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = (Alignment.Self | Alignment.Friend);
                ability.ProximityEvent.OnActions.Add(0, new MultiplyElementEvent("fairy", 6, 5, false));
            }
            else if (ii == 188)
            {
                ability.Name = new LocalText("**Aura Break");
                ability.Desc = new LocalText("The effects of aura abilities are reversed to lower the power of affected moves.");
            }
            else if (ii == 189)
            {
                ability.Name = new LocalText("Primordial Sea");
                ability.Desc = new LocalText("Affects weather and nullifies any Fire-type attacks.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent("heavy_rain")));
            }
            else if (ii == 190)
            {
                ability.Name = new LocalText("Desolate Land");
                ability.Desc = new LocalText("Affects weather and nullifies any Water-type attacks.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent("harsh_sun")));
            }
            else if (ii == 191)
            {
                ability.Name = new LocalText("Delta Stream");
                ability.Desc = new LocalText("Affects weather and eliminates all of the Flying type's weaknesses.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent("wind")));
            }
            else if (ii == 192)
            {
                ability.Name = new LocalText("Stamina");
                ability.Desc = new LocalText("The Pokémon may boost its Defense stat when hit by a move.");
                ability.AfterBeingHits.Add(0, new OnMoveUseEvent(new HitCounterEvent((Alignment.Friend | Alignment.Foe), true, false, true, 25, new StatusStackBattleEvent("mod_defense", true, true, false, 1, new StringKey("MSG_STAMINA")))));
            }
            else if (ii == 193)
            {
                ability.Name = new LocalText("**Wimp Out");
                ability.Desc = new LocalText("The Pokémon cowardly switches out when its HP becomes half or less.");
            }
            else if (ii == 194)
            {
                ability.Name = new LocalText("**Emergency Exit");
                ability.Desc = new LocalText("The Pokémon, sensing danger, switches out when its HP becomes half or less.");
            }
            else if (ii == 195)
            {
                ability.Name = new LocalText("**Water Compaction");
                ability.Desc = new LocalText("Boosts the Pokémon's Defense stat sharply when hit by a Water-type move.");
            }
            else if (ii == 196)
            {
                ability.Name = new LocalText("**Merciless");
                ability.Desc = new LocalText("The Pokémon's attacks become critical hits if the target is poisoned.");
            }
            else if (ii == 197)
            {
                ability.Name = new LocalText("Shields Down");
                ability.Desc = new LocalText("When it takes a hit that causes its HP to become half or less, the Pokémon's shell breaks and it becomes aggressive.");
                SingleEmitter emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));
                ability.BeforeStatusAdds.Add(0, new FormeNeededStatusEvent(new StateStatusCheck(typeof(BadStatusState), new StringKey("MSG_PROTECT_STATUS"), new StatusAnimEvent(emitter, "DUN_Screen_Hit", 10)), 0, 1, 2, 3, 4, 5, 6));
                ability.OnMapStarts.Add(0, new MeteorFormeEvent("minior", 0, 7, 50, false));
                ability.AfterBeingHits.Add(6, new BattlelessEvent(true, new MeteorFormeEvent("minior", 1, 7, 50, true)));
            }
            else if (ii == 198)
            {
                ability.Name = new LocalText("Stakeout");
                ability.Desc = new LocalText("The Pokémon's first attack deals increased damage.");
                ability.OnActions.Add(-11, new MultiplyCategoryWithoutStatusEvent("last_used_move", BattleData.SkillCategory.None, 2, 1, false));
            }
            else if (ii == 199)
            {
                ability.Name = new LocalText("**Water Bubble");
                ability.Desc = new LocalText("Lowers the power of Fire-type moves done to the Pokémon and prevents the Pokémon from getting a burn.");
            }
            else if (ii == 200)
            {
                ability.Name = new LocalText("Steelworker");
                ability.Desc = new LocalText("Powers up Steel-type moves.");
                ability.OnActions.Add(0, new MultiplyElementEvent("steel", 4, 3, false));
            }
            else if (ii == 201)
            {
                ability.Name = new LocalText("Berserk");
                ability.Desc = new LocalText("Boosts the Pokémon's Sp. Atk stat when its HP is half or less.");
                ability.OnActions.Add(0, new PinchNeededEvent(2, new MultiplyCategoryEvent(BattleData.SkillCategory.Magical, 4, 3)));
            }
            else if (ii == 202)
            {
                ability.Name = new LocalText("Slush Rush");
                ability.Desc = new LocalText("Boosts the Pokémon's Speed stat in a hailstorm.");
                ability.OnRefresh.Add(0, new MiscEvent(new HailState()));
                ability.OnRefresh.Add(0, new WeatherSpeedEvent("hail"));
            }
            else if (ii == 203)
            {
                ability.Name = new LocalText("Long Reach");
                ability.Desc = new LocalText("The Pokémon uses its moves without making contact with the target.");
                ability.OnActions.Add(-1, new RemoveMoveStateEvent(typeof(ContactState)));
                ability.OnActions.Add(-1, new SnapDashBackEvent());
            }
            else if (ii == 204)
            {
                ability.Name = new LocalText("Liquid Voice");
                ability.Desc = new LocalText("All sound-based moves become Water-type moves.");
                ability.OnActions.Add(-1, new MoveStateNeededEvent(typeof(SoundState), new ChangeMoveElementEvent("none", "water")));
            }
            else if (ii == 205)
            {
                ability.Name = new LocalText("Triage");
                ability.Desc = new LocalText("Boosts the Attack Range of healing moves.");
                ability.OnActions.Add(-1, new MoveStateNeededEvent(typeof(HealState), new AddRangeEvent(1)));
            }
            else if (ii == 206)
            {
                ability.Name = new LocalText("Galvanize");
                ability.Desc = new LocalText("Normal-type moves become Electric-type moves.");
                ability.OnActions.Add(-1, new ChangeMoveElementEvent("normal", "electric"));
            }
            else if (ii == 207)
            {
                ability.Name = new LocalText("Surge Surfer");
                ability.Desc = new LocalText("Boosts the Pokémon's Movement Speed on Electric Terrain.");
                ability.OnRefresh.Add(0, new WeatherSpeedEvent("electric_terrain"));
            }
            else if (ii == 208)
            {
                ability.Name = new LocalText("**Schooling");
                ability.Desc = new LocalText("When it has a lot of HP, the Pokémon forms a powerful school. It stops schooling when its HP is low.");
            }
            else if (ii == 209)
            {
                ability.Name = new LocalText("Disguise");
                ability.Desc = new LocalText("Once per floor, the shroud that covers the Pokémon can protect it from an attack.");
                ability.BeforeBeingHits.Add(0, new AttackingMoveNeededEvent(new BustFormEvent("mimikyu", 0, 1, new StringKey("MSG_DISGUISE_DECOY"), new BattleAnimEvent(new SingleEmitter(new AnimData("Puff_White", 3)), "_UNK_EVT_012", true, 10))));
            }
            else if (ii == 210)
            {
                ability.Name = new LocalText("**Battle Bond");
                ability.Desc = new LocalText("Defeating an opposing Pokémon strengthens the Pokémon's bond with its Trainer, and it becomes Ash-Greninja. Water Shuriken gets more powerful.");
            }
            else if (ii == 211)
            {
                ability.Name = new LocalText("**Power Construct");
                ability.Desc = new LocalText("Other Cells gather to aid when its HP becomes half or less. Then the Pokémon changes its form to Complete Forme.");
            }
            else if (ii == 212)
            {
                ability.Name = new LocalText("Corrosion");
                ability.Desc = new LocalText("The Pokémon can poison the target even if it's a Steel or Poison type.");
                ability.OnActions.Add(0, new AddContextStateEvent(new Corrosion()));
                ability.BeforeStatusAddings.Add(-1, new AddStatusContextStateEvent(new Corrosion()));
            }
            else if (ii == 213)
            {
                ability.Name = new LocalText("**Comatose");
                ability.Desc = new LocalText("It's always drowsing and will never wake up. It can attack without waking up.");
            }
            else if (ii == 214)
            {
                ability.Name = new LocalText("**Queenly Majesty");
                ability.Desc = new LocalText("Its majesty pressures the opposing Pokémon, making it unable to attack using priority moves.");
            }
            else if (ii == 215)
            {
                ability.Name = new LocalText("**Innards Out");
                ability.Desc = new LocalText("Damages the attacker landing the finishing hit by the amount equal to its last HP.");
            }
            else if (ii == 216)
            {
                ability.Name = new LocalText("**Dancer");
                ability.Desc = new LocalText("When another Pokémon uses a dance move, it can use a dance move following it regardless of its Speed.");
            }
            else if (ii == 217)
            {
                ability.Name = new LocalText("**Battery");
                ability.Desc = new LocalText("Powers up ally Pokémon's special moves.");
            }
            else if (ii == 218)
            {
                ability.Name = new LocalText("**Fluffy");
                ability.Desc = new LocalText("Halves the damage taken from moves that make direct contact, but doubles that of Fire-type moves.");
            }
            else if (ii == 219)
            {
                ability.Name = new LocalText("**Dazzling");
                ability.Desc = new LocalText("Surprises the opposing Pokémon, making it unable to attack using priority moves.");
            }
            else if (ii == 220)
            {
                ability.Name = new LocalText("**Soul-Heart");
                ability.Desc = new LocalText("Boosts its Sp. Atk stat every time a Pokémon faints.");
            }
            else if (ii == 221)
            {
                ability.Name = new LocalText("Tangling Hair");
                ability.Desc = new LocalText("Contact with the Pokémon lowers the attacker's Speed stat.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 50, new StatusStackBattleEvent("mod_speed", false, true, -1)));
            }
            else if (ii == 222)
            {
                ability.Name = new LocalText("Receiver");
                ability.Desc = new LocalText("The Pokémon copies the Ability of an ally.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend), false, false, true, 100, new ReflectAbilityEvent(true, new StringKey("MSG_TRACE"))));
            }
            else if (ii == 223)
            {
                ability.Name = new LocalText("Power of Alchemy");
                ability.Desc = new LocalText("The Pokémon copies the Ability of an ally.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend), false, false, true, 100, new ReflectAbilityEvent(true, new StringKey("MSG_TRACE"))));
            }
            else if (ii == 224)
            {
                ability.Name = new LocalText("Beast Boost");
                ability.Desc = new LocalText("The Pokémon may boost its most proficient stat each time it knocks out a Pokémon with a move.");
                ability.AfterActions.Add(0, new KnockOutNeededEvent(new OnMoveUseEvent(new OnHitAnyEvent(true, 50, new AffectHighestStatBattleEvent(false, "mod_attack", "mod_defense", "mod_special_attack", "mod_special_defense", false, 1)))));
            }
            else if (ii == 225)
            {
                ability.Name = new LocalText("**RKS System");
                ability.Desc = new LocalText("Changes the Pokémon's type to match the memory disc it holds.");
            }
            else if (ii == 226)
            {
                ability.Name = new LocalText("Electric Surge");
                ability.Desc = new LocalText("Turns the ground into Electric Terrain when the Pokémon is battling.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent("electric_terrain", 10, new StringKey(), typeof(ExtendWeatherState))));
            }
            else if (ii == 227)
            {
                ability.Name = new LocalText("Psychic Surge");
                ability.Desc = new LocalText("Turns the ground into Psychic Terrain when the Pokémon is battling.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent("psychic_terrain", 10, new StringKey(), typeof(ExtendWeatherState))));
            }
            else if (ii == 228)
            {
                ability.Name = new LocalText("Misty Surge");
                ability.Desc = new LocalText("Turns the ground into Misty Terrain when the Pokémon is battling.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent("misty_terrain", 10, new StringKey(), typeof(ExtendWeatherState))));
            }
            else if (ii == 229)
            {
                ability.Name = new LocalText("Grassy Surge");
                ability.Desc = new LocalText("Turns the ground into Grassy Terrain when the Pokémon is battling.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent("grassy_terrain", 10, new StringKey(), typeof(ExtendWeatherState))));
            }
            else if (ii == 230)
            {
                ability.Name = new LocalText("Full Metal Body");
                ability.Desc = new LocalText("Prevents other Pokémon's moves or Abilities from lowering the Pokémon's stats.");
                ability.BeforeStatusAdds.Add(0, new StatChangeCheck(new List<Stat>(), new StringKey("MSG_STAT_DROP_PROTECT"), true, false, false));
            }
            else if (ii == 231)
            {
                ability.Name = new LocalText("Shadow Shield");
                ability.Desc = new LocalText("Reduces the amount of damage the Pokémon takes while its HP is full.");
                ability.BeforeBeingHits.Add(0, new MultiScaleEvent(new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 232)
            {
                ability.Name = new LocalText("Prism Armor");
                ability.Desc = new LocalText("Reduces the power of supereffective attacks taken.");
                ability.BeforeBeingHits.Add(0, new MultiplyEffectiveEvent(false, 2, 3, new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 233)
            {
                ability.Name = new LocalText("Neuroforce");
                ability.Desc = new LocalText("Powers up moves that are super effective.");
                ability.BeforeHittings.Add(1, new MultiplyEffectiveEvent(false, 5, 4));
            }
            else if (ii == 234)
            {
                ability.Name = new LocalText("**Intrepid Sword");
                ability.Desc = new LocalText("Boosts the Pokémon's Attack stat when the Pokémon enters a battle.");
            }
            else if (ii == 235)
            {
                ability.Name = new LocalText("**Dauntless Shield");
                ability.Desc = new LocalText("Boosts the Pokémon's Defense stat when the Pokémon enters a battle.");
            }
            else if (ii == 236)
            {
                ability.Name = new LocalText("Libero");
                ability.Desc = new LocalText("Changes the Pokémon's type to the type of the move it last used.");
                ability.AfterActions.Add(-1, new ConversionEvent(false));
            }
            else if (ii == 237)
            {
                ability.Name = new LocalText("Ball Fetch");
                ability.Desc = new LocalText("The Pokémon will fetch any Apricorn that fails to recruit wild Pokémon.");
                ability.ProximityEvent.Radius = 5;
                ability.ProximityEvent.TargetAlignments = Alignment.Friend | Alignment.Foe;
                ability.ProximityEvent.AfterBeingHits.Add(0, new FetchEvent(new StringKey("MSG_BALL_FETCH")));
            }
            else if (ii == 238)
            {
                ability.Name = new LocalText("**Cotton Down");
                ability.Desc = new LocalText("When the Pokémon is hit by an attack, it scatters cotton fluff around and lowers the Speed stat of all Pokémon except itself.");
            }
            else if (ii == 239)
            {
                ability.Name = new LocalText("**Propeller Tail");
                ability.Desc = new LocalText("Ignores the effects of opposing Pokémon's Abilities and moves that draw in moves.");
            }
            else if (ii == 240)
            {
                ability.Name = new LocalText("Mirror Armor");
                ability.Desc = new LocalText("Bounces back stat-lowering effects from other Pokémon.");
                StatChangeReflect reflect = new StatChangeReflect(new List<Stat>(), new StringKey("MSG_STAT_DROP_REFLECT"), true, false, false);
                SingleEmitter emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));
                reflect.Anims.Add(new StatusAnimEvent(emitter, "DUN_Screen_Hit", 10));
                ability.BeforeStatusAdds.Add(0, reflect);
            }
            else if (ii == 241)
            {
                ability.Name = new LocalText("**Gulp Missile");
                ability.Desc = new LocalText("When the Pokémon uses Surf or Dive, it will come back with prey. When it takes damage, it will spit out the prey to attack.");
            }
            else if (ii == 242)
            {
                ability.Name = new LocalText("**Stalwart");
                ability.Desc = new LocalText("Ignores the effects of opposing Pokémon's Abilities and moves that draw in moves.");
            }
            else if (ii == 243)
            {
                ability.Name = new LocalText("**Steam Engine");
                ability.Desc = new LocalText("Boosts the Pokémon's Speed stat drastically if hit by a Fire- or Water-type move.");
            }
            else if (ii == 244)
            {
                ability.Name = new LocalText("**Punk Rock");
                ability.Desc = new LocalText("Boosts the power of sound-based moves. The Pokémon also takes half the damage from these kinds of moves.");
            }
            else if (ii == 245)
            {
                ability.Name = new LocalText("**Sand Spit");
                ability.Desc = new LocalText("The Pokémon creates a sandstorm when it's hit by an attack.");
            }
            else if (ii == 246)
            {
                ability.Name = new LocalText("Ice Scales");
                ability.Desc = new LocalText("The Pokémon is protected by ice scales, which halve the damage taken from special moves.");
                ability.BeforeBeingHits.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Magical, 1, 2, new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 247)
            {
                ability.Name = new LocalText("Ripen");
                ability.Desc = new LocalText("Raises a stat when it eats a berry.");
                ability.AfterBeingHits.Add(0, new BerryBoostEvent("mod_speed", "mod_attack", "mod_defense", "mod_special_attack", "mod_special_defense", "mod_accuracy", "mod_evasion"));
            }
            else if (ii == 248)
            {
                ability.Name = new LocalText("Ice Face");
                ability.Desc = new LocalText("The Pokémon's ice head can take a physical attack as a substitute, breaking in the process. The ice is restored in hail.");
                FiniteReleaseEmitter endAnim = new FiniteReleaseEmitter(new AnimData("Ice_Pieces", 6, 0, 0), new AnimData("Ice_Pieces", 12, 1, 1), new AnimData("Ice_Pieces", 12, 1, 1));
                endAnim.BurstTime = 2;
                endAnim.ParticlesPerBurst = 4;
                endAnim.Bursts = 4;
                endAnim.StartDistance = 8;
                endAnim.Speed = 60;
                endAnim.Layer = DrawLayer.Front;
                ability.BeforeBeingHits.Add(0, new CategoryNeededEvent(BattleData.SkillCategory.Physical, new BustFormEvent("eiscue", 0, 1, new StringKey("MSG_DISGUISE_DECOY"), new BattleAnimEvent(endAnim, "DUN_Ice_Ball_2", true, 10))));
                
                {
                    Dictionary<string, int> weather = new Dictionary<string, int>();
                    weather.Add("hail", 0);
                    ability.OnMapTurnEnds.Add(0, new WeatherFormeSingleEvent("eiscue", -1, weather, new AnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Wonder_Tile", 10)));
                }
            }
            else if (ii == 249)
            {
                ability.Name = new LocalText("**Power Spot");
                ability.Desc = new LocalText("Just being next to the Pokémon powers up moves.");
            }
            else if (ii == 250)
            {
                ability.Name = new LocalText("**Mimicry");
                ability.Desc = new LocalText("Changes the Pokémon's type depending on the terrain.");
            }
            else if (ii == 251)
            {
                ability.Name = new LocalText("**Screen Cleaner");
                ability.Desc = new LocalText("When the Pokémon enters a battle, the effects of Light Screen, Reflect, and Aurora Veil are nullified for both opposing and ally Pokémon.");
            }
            else if (ii == 252)
            {
                ability.Name = new LocalText("**Steely Spirit");
                ability.Desc = new LocalText("Powers up ally Pokémon's Steel-type moves.");
            }
            else if (ii == 253)
            {
                ability.Name = new LocalText("Perish Body");
                ability.Desc = new LocalText("When hit by a move that makes direct contact, the attacker will receive the Perish Count status.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 100, new StatusBattleEvent("perish_song", false, true, false, new StringKey("MSG_HAS_ABILITY"),
                    new BattleAnimEvent(new SingleEmitter(new AnimData("Dark_Pulse_Ranger", 3)), "DUN_Night_Shade", false, 30))));
            }
            else if (ii == 254)
            {
                ability.Name = new LocalText("**Wandering Spirit");
                ability.Desc = new LocalText("The Pokémon exchanges Abilities with a Pokémon that hits it with a move that makes direct contact.");
            }
            else if (ii == 255)
            {
                ability.Name = new LocalText("**Gorilla Tactics");
                ability.Desc = new LocalText("Boosts the Pokémon's Attack stat but only allows the use of the first selected move.");
            }
            else if (ii == 256)
            {
                ability.Name = new LocalText("Neutralizing Gas");
                ability.Desc = new LocalText("The Pokémon neutralizes abilities when attacked.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 100, new ChangeToAbilityEvent("none", true, true)));
            }
            else if (ii == 257)
            {
                ability.Name = new LocalText("Pastel Veil");
                ability.Desc = new LocalText("Protects the Pokémon and its ally Pokémon from being poisoned.");
                ability.ProximityEvent.TargetAlignments = (Alignment.Self | Alignment.Friend);
                ability.ProximityEvent.BeforeStatusAdds.Add(0, new PreventStatusCheck("poison", new StringKey("MSG_PASTEL_VEIL"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
                ability.ProximityEvent.BeforeStatusAdds.Add(0, new PreventStatusCheck("poison_toxic", new StringKey("MSG_PASTEL_VEIL"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
            }
            else if (ii == 258)
            {
                ability.Name = new LocalText("**Hunger Switch");
                ability.Desc = new LocalText("The Pokémon changes its form, alternating between its Full Belly Mode and Hangry Mode after the end of each turn.");
            }
            else if (ii == 259)
            {
                ability.Name = new LocalText("**Quick Draw");
                ability.Desc = new LocalText("Enables the Pokémon to move first occasionally.");
            }
            else if (ii == 260)
            {
                ability.Name = new LocalText("**Unseen Fist");
                ability.Desc = new LocalText("If the Pokémon uses moves that make direct contact, it can attack the target even if the target protects itself.");
            }
            else if (ii == 261)
            {
                ability.Name = new LocalText("**Curious Medicine");
                ability.Desc = new LocalText("When the Pokémon enters a battle, it scatters medicine from its shell, which removes all stat changes from allies.");
            }
            else if (ii == 262)
            {
                ability.Name = new LocalText("Transistor");
                ability.Desc = new LocalText("Powers up Electric-type moves.");
                ability.OnActions.Add(0, new MultiplyElementEvent("electric", 11, 10, false));
            }
            else if (ii == 263)
            {
                ability.Name = new LocalText("Dragon's Maw");
                ability.Desc = new LocalText("Powers up Dragon-type moves.");
                ability.OnActions.Add(0, new MultiplyElementEvent("dragon", 11, 10, false));
            }
            else if (ii == 264)
            {
                ability.Name = new LocalText("**Chilling Neigh");
                ability.Desc = new LocalText("When the Pokémon knocks out a target, it utters a chilling neigh, which boosts its Attack stat.");
            }
            else if (ii == 265)
            {
                ability.Name = new LocalText("**Grim Neigh");
                ability.Desc = new LocalText("When the Pokémon knocks out a target, it utters a terrifying neigh, which boosts its Sp. Atk stat.");
            }
            else if (ii == 266)
            {
                ability.Name = new LocalText("**As One");
                fileName = "as_one_chill";
                ability.Desc = new LocalText("This Ability combines the effects of both Calyrex's Unnerve Ability and Glastrier's Chilling Neigh Ability.");
            }
            else if (ii == 267)
            {
                ability.Name = new LocalText("**As One");
                fileName = "as_one_grim";
                ability.Desc = new LocalText("This Ability combines the effects of both Calyrex's Unnerve Ability and Spectrier's Grim Neigh Ability.");
            }
            else if (ii == 268)
            {
                ability.Name = new LocalText("**Lingering Aroma");
                ability.Desc = new LocalText("");
            }
            else if (ii == 269)
            {
                ability.Name = new LocalText("**Seed Sower");
                ability.Desc = new LocalText("");
            }
            else if (ii == 270)
            {
                ability.Name = new LocalText("**Thermal Exchange");
                ability.Desc = new LocalText("");
            }
            else if (ii == 271)
            {
                ability.Name = new LocalText("**Anger Shell");
                ability.Desc = new LocalText("");
            }
            else if (ii == 272)
            {
                ability.Name = new LocalText("**Purifying Salt");
                ability.Desc = new LocalText("");
            }
            else if (ii == 273)
            {
                ability.Name = new LocalText("**Well-Baked Body");
                ability.Desc = new LocalText("");
            }
            else if (ii == 274)
            {
                ability.Name = new LocalText("**Wind Rider");
                ability.Desc = new LocalText("");
            }
            else if (ii == 275)
            {
                ability.Name = new LocalText("**Guard Dog");
                ability.Desc = new LocalText("");
            }
            else if (ii == 276)
            {
                ability.Name = new LocalText("**Rocky Payload");
                ability.Desc = new LocalText("");
            }
            else if (ii == 277)
            {
                ability.Name = new LocalText("**Wind Power");
                ability.Desc = new LocalText("");
            }
            else if (ii == 278)
            {
                ability.Name = new LocalText("**Zero to Hero");
                ability.Desc = new LocalText("");
            }
            else if (ii == 279)
            {
                ability.Name = new LocalText("**Commander");
                ability.Desc = new LocalText("");
            }
            else if (ii == 280)
            {
                ability.Name = new LocalText("**Electromorphosis");
                ability.Desc = new LocalText("");
            }
            else if (ii == 281)
            {
                ability.Name = new LocalText("**Protosynthesis");
                ability.Desc = new LocalText("");
            }
            else if (ii == 282)
            {
                ability.Name = new LocalText("**Quark Drive");
                ability.Desc = new LocalText("");
            }
            else if (ii == 283)
            {
                ability.Name = new LocalText("**Good as Gold");
                ability.Desc = new LocalText("");
            }
            else if (ii == 284)
            {
                ability.Name = new LocalText("Vessel of Ruin");
                ability.Desc = new LocalText("The power of the Pokémon's ruinous vessel lowers the Sp. Atk stats of all Pokémon except itself.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = Alignment.Friend | Alignment.Foe;
                ability.ProximityEvent.OnActions.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Magical, 3, 4));
            }
            else if (ii == 285)
            {
                ability.Name = new LocalText("Sword of Ruin");
                ability.Desc = new LocalText("The power of the Pokémon's ruinous sword lowers the Defense stats of all Pokémon except itself.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = Alignment.Friend | Alignment.Foe;
                ability.ProximityEvent.BeforeBeingHits.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Physical, 4, 3));
            }
            else if (ii == 286)
            {
                ability.Name = new LocalText("Tablets of Ruin");
                ability.Desc = new LocalText("The power of the Pokémon's ruinous wooden tablets lowers the Attack stats of all Pokémon except itself.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = Alignment.Friend | Alignment.Foe;
                ability.ProximityEvent.OnActions.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Physical, 3, 4));
            }
            else if (ii == 287)
            {
                ability.Name = new LocalText("Beads of Ruin");
                ability.Desc = new LocalText("The power of the Pokémon's ruinous beads lowers the Sp. Def stats of all Pokémon except itself.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = Alignment.Friend | Alignment.Foe;
                ability.ProximityEvent.BeforeBeingHits.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Magical, 4, 3));
            }
            else if (ii == 288)
            {
                ability.Name = new LocalText("**Orichalcum Pulse");
                ability.Desc = new LocalText("");
            }
            else if (ii == 289)
            {
                ability.Name = new LocalText("**Hadron Engine");
                ability.Desc = new LocalText("");
            }
            else if (ii == 290)
            {
                ability.Name = new LocalText("**Opportunist");
                ability.Desc = new LocalText("");
            }
            else if (ii == 291)
            {
                ability.Name = new LocalText("**Cud Chew");
                ability.Desc = new LocalText("");
            }
            else if (ii == 292)
            {
                ability.Name = new LocalText("Sharpness");
                ability.Desc = new LocalText("Powers up slicing moves.");
                ability.OnActions.Add(0, new MultiplyMoveStateEvent(typeof(BladeState), 5, 4));
            }
            else if (ii == 293)
            {
                ability.Name = new LocalText("Supreme Overlord");
                ability.Desc = new LocalText("The Pokémon's Attack and Sp. Atk stats are boosted for every fainted teammate.");
                ability.OnActions.Add(0, new MultiplyFromFallenEvent(10));
            }
            else if (ii == 294)
            {
                ability.Name = new LocalText("**Costar");
                ability.Desc = new LocalText("");
            }
            else if (ii == 295)
            {
                ability.Name = new LocalText("**Toxic Debris");
                ability.Desc = new LocalText("");
            }
            else if (ii == 296)
            {
                ability.Name = new LocalText("**Armor Tail");
                ability.Desc = new LocalText("");
            }
            else if (ii == 297)
            {
                ability.Name = new LocalText("**Earth Eater");
                ability.Desc = new LocalText("");
            }
            else if (ii == 298)
            {
                ability.Name = new LocalText("**Mycelium Might");
                ability.Desc = new LocalText("");
            }

            if (ability.Name.DefaultText.StartsWith("**"))
                ability.Name.DefaultText = ability.Name.DefaultText.Replace("*", "");
            else if (ability.Name.DefaultText != "")
                ability.Released = true;

            if (fileName == "")
                fileName = Text.Sanitize(ability.Name.DefaultText).ToLower();

            return (fileName, ability);
        }



    }
}
