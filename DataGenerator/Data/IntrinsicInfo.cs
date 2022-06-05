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
    public static class IntrinsicInfo
    {
        public const int MAX_INTRINSICS = 268;

        public static void AddIntrinsicData()
        {
            DataInfo.DeleteIndexedData(DataManager.DataType.Intrinsic.ToString());
            for (int ii = 0; ii < MAX_INTRINSICS; ii++)
            {
                IntrinsicData ability = GetIntrinsicData(ii);
                DataManager.SaveData(ii, DataManager.DataType.Intrinsic.ToString(), ability);
            }
        }
        public static IntrinsicData GetIntrinsicData(int ii)
        {
            IntrinsicData ability = new IntrinsicData();
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
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 35, new StatusBattleEvent(8, false, true, false, new StringKey("MSG_STENCH"),
                    new BattleAnimEvent(emitter, "DUN_Smokescreen", false, 30))));
            }
            else if (ii == 2)
            {
                ability.Name = new LocalText("Drizzle");
                ability.Desc = new LocalText("The Pokémon makes it rain when it is battling.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent(1, 10, new StringKey("MSG_DRIZZLE"), typeof(ExtendWeatherState))));
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
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(4, new StringKey("MSG_LIMBER")));
            }
            else if (ii == 8)
            {
                ability.Name = new LocalText("Sand Veil");
                ability.Desc = new LocalText("Avoids attacks from a distance when in a sandstorm.");
                ability.OnRefresh.Add(0, new MiscEvent(new SandState()));
                ability.BeforeBeingHits.Add(0, new WeatherNeededEvent(3, new EvadeDistanceEvent()));
            }
            else if (ii == 9)
            {
                ability.Name = new LocalText("Static");
                ability.Desc = new LocalText("The Pokémon is charged with static electricity, so contact with it may cause paralysis.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 35, new StatusBattleEvent(4, false, true, false, new StringKey("MSG_STATIC"),
                    new BattleAnimEvent(new SingleEmitter(new AnimData("Spark", 3)), "DUN_Paralyzed", false, 0))));
            }
            else if (ii == 10)
            {
                ability.Name = new LocalText("Volt Absorb");
                ability.Desc = new LocalText("Restores HP if hit by an Electric-type move.");
                ability.ProximityEvent.Radius = 0;
                ability.ProximityEvent.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                ability.ProximityEvent.BeforeExplosions.Add(0, new IsolateElementEvent(04));
                ability.BeforeBeingHits.Add(5, new AbsorbElementEvent(04, false, true,
                    new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Drink",
                    new RestoreHPEvent(1, 4, true)));
            }
            else if (ii == 11)
            {
                ability.Name = new LocalText("Water Absorb");
                ability.Desc = new LocalText("Restores HP if hit by a Water-type move.");
                ability.ProximityEvent.Radius = 0;
                ability.ProximityEvent.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                ability.ProximityEvent.BeforeExplosions.Add(0, new IsolateElementEvent(18));
                ability.BeforeBeingHits.Add(5, new AbsorbElementEvent(18, false, true,
                    new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Drink",
                    new RestoreHPEvent(1, 4, true)));
            }
            else if (ii == 12)
            {
                ability.Name = new LocalText("Oblivious");
                ability.Desc = new LocalText("Protects the Pokémon from Attract or Rage Powder.");
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(21, new StringKey("MSG_OBLIVIOUS")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(22, new StringKey("MSG_OBLIVIOUS")));
            }
            else if (ii == 13)
            {
                ability.Name = new LocalText("Cloud Nine");
                ability.Desc = new LocalText("The Pokémon eliminates weather when it is battling.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent(0, 10, new StringKey("MSG_CLOUD_NINE"), typeof(ExtendWeatherState))));
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
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(1, new StringKey("MSG_INSOMNIA")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(61, new StringKey("MSG_INSOMNIA_DROWSY")));
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
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(5, new StringKey("MSG_IMMUNITY")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(6, new StringKey("MSG_IMMUNITY")));
            }
            else if (ii == 18)
            {
                ability.Name = new LocalText("Flash Fire");
                ability.Desc = new LocalText("Powers up the Pokémon's Fire-type moves if it's hit by one.");
                ability.ProximityEvent.Radius = 0;
                ability.ProximityEvent.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                ability.ProximityEvent.BeforeExplosions.Add(0, new IsolateElementEvent(07));
                ability.BeforeBeingHits.Add(5, new AbsorbElementEvent(07, false, true,
                    new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Drink",
                    new StatusElementBattleEvent(84, true, false, 07)));
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
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(7, new StringKey("MSG_OWN_TEMPO")));
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
                ability.AfterBeingHits.Add(0, new HitCounterEvent(Alignment.Foe, false, false, false, 100, new StatusBattleEvent(90, false, true, false, new StringKey("MSG_SHADOW_TAG"),
                    new BattleAnimEvent(new SingleEmitter(new AnimData("Dark_Pulse_Ranger", 3)), "DUN_Night_Shade", false, 0))));
                //ability.AfterHittings.Add(0, new OnHitEvent(false, false, 100, new StatusBattleEvent(90, true, true, false, new StringKey("MSG_SHADOW_TAG"),
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
                ability.BeforeBeingHits.Add(6, new ElementNeededEvent(0, new RegularAttackNeededEvent(new SetDamageEvent(new SpecificDamageEvent(1), new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", true, 10)))));
            }
            else if (ii == 26)
            {
                ability.Name = new LocalText("Levitate");
                ability.Desc = new LocalText("By floating in the air, the Pokémon receives full immunity to all Ground-type moves.");
                ability.TargetElementEffects.Add(0, new TypeImmuneEvent(11));
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
                    new StatusBattleEvent(1, false, true, false, new StringKey("MSG_EFFECT_SPORE"), new BattleAnimEvent(emitter1, "DUN_Substitute", false, 20)),
                    new StatusBattleEvent(4, false, true, false, new StringKey("MSG_EFFECT_SPORE"), new BattleAnimEvent(emitter2, "DUN_Substitute", false, 20)),
                    new StatusBattleEvent(5, false, true, false, new StringKey("MSG_EFFECT_SPORE"), new BattleAnimEvent(emitter3, "DUN_Substitute", false, 20))));
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
                ability.ProximityEvent.BeforeExplosions.Add(0, new DrawAttackEvent(Alignment.Friend, 04, new StringKey("MSG_DRAW_ATTACK")));
                ability.BeforeBeingHits.Add(5, new AbsorbElementEvent(04, true, new StatusStackBattleEvent(12, true, false, 1)));
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
                ability.OnRefresh.Add(0, new WeatherSpeedEvent(1));
            }
            else if (ii == 34)
            {
                ability.Name = new LocalText("Chlorophyll");
                ability.Desc = new LocalText("Boosts the Pokémon's Movement Speed in sunshine.");
                ability.OnRefresh.Add(0, new WeatherSpeedEvent(2));
            }
            else if (ii == 35)
            {
                ability.Name = new LocalText("Illuminate");
                ability.Desc = new LocalText("It may warp an ally to the Pokémon when it is hurt.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent(Alignment.Foe, true, false, true, 35, new WarpAlliesInEvent(1, true, new StringKey("MSG_ILLUMINATE"), true)));
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
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 35, new StatusBattleEvent(5, false, true, false, new StringKey("MSG_POISON_POINT"),
                    new BattleAnimEvent(emitter, "DUN_Toxic", false, 30))));
            }
            else if (ii == 39)
            {
                ability.Name = new LocalText("Inner Focus");
                ability.Desc = new LocalText("The Pokémon's intensely focused, and that protects the Pokémon from flinching.");
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(8, new StringKey("MSG_INNER_FOCUS")));
            }
            else if (ii == 40)
            {
                ability.Name = new LocalText("Magma Armor");
                ability.Desc = new LocalText("The Pokémon is covered with hot magma, which prevents the Pokémon from becoming frozen. Thrown items will also burn up on contact.");
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(3, new StringKey("MSG_MAGMA_ARMOR")));
                ability.ProximityEvent.Radius = 0;
                ability.ProximityEvent.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                ability.ProximityEvent.BeforeExplosions.Add(0, new DampItemEvent());

                BattleData catchData = new BattleData();
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
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(2, new StringKey("MSG_WATER_VEIL")));
            }
            else if (ii == 42)
            {
                ability.Name = new LocalText("Magnet Pull");
                ability.Desc = new LocalText("Pulls Steel-type targets to this Pokémon.");
                //ability.AfterBeingHits.Add(0, new HitCounterEvent(Alignment.Foe | Alignment.Friend, false, false, true, 100, new CharElementNeededEvent(17, new WarpHereEvent(new StringKey("MSG_MAGNET_PULL"), false))));
                ability.AfterHittings.Add(5, new OnHitEvent(false, false, 100, new CharElementNeededEvent(17, new WarpHereEvent(new StringKey("MSG_MAGNET_PULL"), true))));
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
                ability.AfterActions.Add(0, new WeatherNeededEvent(1, new OnMoveUseEvent(new RestoreHPEvent(1, 8, false))));
            }
            else if (ii == 45)
            {
                ability.Name = new LocalText("Sand Stream");
                ability.Desc = new LocalText("The Pokémon summons a sandstorm when it is battling.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent(3, 10, new StringKey("MSG_SAND_STREAM"), typeof(ExtendWeatherState))));
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
                ability.BeforeBeingHits.Add(0, new MultiplyElementEvent(07, 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
                emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));
                ability.BeforeBeingHits.Add(0, new MultiplyElementEvent(12, 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 48)
            {
                ability.Name = new LocalText("Early Bird");
                ability.Desc = new LocalText("The Pokémon awakens quickly from sleep.");
                ability.OnTurnEnds.Add(0, new EarlyBirdEvent(1));
            }
            else if (ii == 49)
            {
                ability.Name = new LocalText("Flame Body");
                ability.Desc = new LocalText("Contact with the Pokémon may burn the attacker. Thrown items will also burn up on contact.");
                SingleEmitter endEmitter = new SingleEmitter(new AnimData("Burned", 3));
                endEmitter.LocHeight = 8;
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 35, new StatusBattleEvent(2, false, true, false, new StringKey("MSG_FLAME_BODY"),
                    new BattleAnimEvent(endEmitter, "DUN_Flamethrower_3", false, 0))));
                ability.ProximityEvent.Radius = 0;
                ability.ProximityEvent.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                ability.ProximityEvent.BeforeExplosions.Add(0, new DampItemEvent());

                BattleData catchData = new BattleData();
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
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(90, new StringKey("MSG_RUN_AWAY")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(19, new StringKey("MSG_RUN_AWAY")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(20, new StringKey("MSG_RUN_AWAY")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(44, new StringKey("MSG_RUN_AWAY")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(45, new StringKey("MSG_RUN_AWAY")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(46, new StringKey("MSG_RUN_AWAY")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(58, new StringKey("MSG_RUN_AWAY")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(103, new StringKey("MSG_RUN_AWAY")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(104, new StringKey("MSG_RUN_AWAY")));
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
                ability.OnMapStarts.Add(0, new PickupEvent(new AnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 2)), "")));
            }
            else if (ii == 54)
            {
                ability.Name = new LocalText("Truant");
                ability.Desc = new LocalText("The Pokémon can't attack on consecutive turns.");
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(2));
                ability.AfterActions.Add(0, new OnMoveUseEvent(new StatusStateBattleEvent(55, false, true, statusStates)));
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
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 35, new StatusBattleEvent(21, false, true, false, new StringKey("MSG_CUTE_CHARM"),
                    new BattleAnimEvent(endAnim, "DUN_Charm", false, 20))));
            }
            else if (ii == 57)
            {
                ability.Name = new LocalText("Plus");
                ability.Desc = new LocalText("Boosts the Sp. Atk stat if another Pokémon has Minus.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = Alignment.Friend;
                ability.ProximityEvent.OnActions.Add(0, new SupportAbilityEvent(58));
            }
            else if (ii == 58)
            {
                ability.Name = new LocalText("Minus");
                ability.Desc = new LocalText("Boosts the Sp. Atk stat if another Pokémon has Plus.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = Alignment.Friend;
                ability.ProximityEvent.OnActions.Add(0, new SupportAbilityEvent(57));
            }
            else if (ii == 59)
            {
                ability.Name = new LocalText("Forecast");
                ability.Desc = new LocalText("The Pokémon transforms with the weather to change its type to Water, Fire, or Ice.");
                {
                    Dictionary<int, int> weather = new Dictionary<int, int>();
                    weather.Add(1, 2);
                    weather.Add(2, 1);
                    weather.Add(4, 3);
                    ability.OnMapStatusAdds.Add(0, new WeatherFormeChangeEvent(351, 0, weather));
                }
                {
                    Dictionary<int, int> weather = new Dictionary<int, int>();
                    weather.Add(1, 2);
                    weather.Add(2, 1);
                    weather.Add(4, 3);
                    ability.OnMapStatusRemoves.Add(0, new WeatherFormeChangeEvent(351, 0, weather));
                }
                {
                    Dictionary<int, int> weather = new Dictionary<int, int>();
                    weather.Add(1, 2);
                    weather.Add(2, 1);
                    weather.Add(4, 3);
                    ability.OnMapStarts.Add(-11, new WeatherFormeEvent(351, 0, weather));
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
                ability.OnActions.Add(0, new MultiplyCategoryInStatusEvent(2, BattleData.SkillCategory.Physical, 3, 2, false));
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
                ability.AfterBeingHits.Add(0, new AddContextStateEvent(new TaintedDrain(), true));
                ability.OnRefresh.Add(0, new MiscEvent(new DrainDamageState()));
            }
            else if (ii == 65)
            {
                ability.Name = new LocalText("Overgrow");
                ability.Desc = new LocalText("Powers up Grass-type moves when the Pokémon's HP is low.");
                ability.OnActions.Add(0, new PinchEvent(10));
            }
            else if (ii == 66)
            {
                ability.Name = new LocalText("Blaze");
                ability.Desc = new LocalText("Powers up Fire-type moves when the Pokémon's HP is low.");
                ability.OnActions.Add(0, new PinchEvent(07));
            }
            else if (ii == 67)
            {
                ability.Name = new LocalText("Torrent");
                ability.Desc = new LocalText("Powers up Water-type moves when the Pokémon's HP is low.");
                ability.OnActions.Add(0, new PinchEvent(18));
            }
            else if (ii == 68)
            {
                ability.Name = new LocalText("Swarm");
                ability.Desc = new LocalText("Powers up Bug-type moves when the Pokémon's HP is low.");
                ability.OnActions.Add(0, new PinchEvent(01));
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
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent(2, 10, new StringKey("MSG_DROUGHT"), typeof(ExtendWeatherState))));
            }
            else if (ii == 71)
            {
                ability.Name = new LocalText("Arena Trap");
                ability.Desc = new LocalText("Prevents grounded opposing Pokémon from fleeing.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent(Alignment.Foe, false, false, false, 100, new CheckImmunityBattleEvent(11, false, new StatusBattleEvent(90, false, true, false, new StringKey("MSG_ARENA_TRAP")))));
                //ability.AfterHittings.Add(0, new OnHitEvent(false, false, 100, new CheckImmunityBattleEvent(11, true, new StatusBattleEvent(90, true, true, false, new StringKey("MSG_ARENA_TRAP")))));
            }
            else if (ii == 72)
            {
                ability.Name = new LocalText("Vital Spirit");
                ability.Desc = new LocalText("The Pokémon is full of vitality, and that prevents it from falling asleep.");
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(1, new StringKey("MSG_VITAL_SPIRIT")));
                ability.BeforeStatusAdds.Add(0, new PreventStatusCheck(61, new StringKey("MSG_VITAL_SPIRIT_DROWSY")));
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
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent(0, 10, new StringKey("MSG_AIR_LOCK"), typeof(ExtendWeatherState))));
            }
            else if (ii == 77)
            {
                ability.Name = new LocalText("Tangled Feet");
                ability.Desc = new LocalText("If the Pokémon is confused, it will avoid attacks from a distance.");
                ability.BeforeBeingHits.Add(0, new EvadeInStatusEvent(7));
            }
            else if (ii == 78)
            {
                ability.Name = new LocalText("Motor Drive");
                ability.Desc = new LocalText("Boosts the Pokémon's Movement Speed when it's hit by an Electric-type move.");
                ability.ProximityEvent.Radius = 0;
                ability.ProximityEvent.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                ability.ProximityEvent.BeforeExplosions.Add(0, new IsolateElementEvent(04));
                ability.BeforeBeingHits.Add(5, new AbsorbElementEvent(04, false, true,
                    new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Drink",
                    new StatusStackBattleEvent(9, true, false, 1)));
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
                ability.OnStatusAdds.Add(0, new StatusResponseEvent(8, new GiveStatusEvent(9, statusStates, false, new StringKey("MSG_STEADFAST"))));
            }
            else if (ii == 81)
            {
                ability.Name = new LocalText("Snow Cloak");
                ability.Desc = new LocalText("Avoids attacks from a distance when in a hailstorm.");
                ability.OnRefresh.Add(0, new MiscEvent(new HailState()));
                ability.BeforeBeingHits.Add(0, new WeatherNeededEvent(4, new EvadeDistanceEvent()));
            }
            else if (ii == 82)
            {
                ability.Name = new LocalText("Gluttony");
                ability.Desc = new LocalText("Steals and eats a food item from an attacker that made direct contact.");
                HashSet<FlagType> eligibles = new HashSet<FlagType>();
                eligibles.Add(new FlagType(typeof(EdibleState)));
                ability.AfterBeingHits.Add(0, new HitCounterEvent(Alignment.Foe, true, true, true, 100, new UseFoeItemEvent(true, false, 116, eligibles, false, true)));
            }
            else if (ii == 83)
            {
                ability.Name = new LocalText("Anger Point");
                ability.Desc = new LocalText("The Pokémon is angered when it takes a critical hit, and that maxes its Attack stat.");
                ability.AfterBeingHits.Add(0, new CritNeededEvent(new StatusStackBattleEvent(10, true, true, false, 12, new StringKey("MSG_ANGER_POINT"))));
            }
            else if (ii == 84)
            {
                ability.Name = new LocalText("Unburden");
                ability.Desc = new LocalText("Boosts Movement Speed the Pokémon has no held item.");
                ability.OnRefresh.Add(0, new UnburdenEvent());
            }
            else if (ii == 85)
            {
                ability.Name = new LocalText("Heatproof");
                ability.Desc = new LocalText("Weakens the power of Fire-type moves.");
                SingleEmitter emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));
                ability.BeforeBeingHits.Add(0, new MultiplyElementEvent(07, 1, 4, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
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
                ability.ProximityEvent.BeforeExplosions.Add(0, new IsolateElementEvent(18));
                ability.BeforeBeingHits.Add(5, new AbsorbElementEvent(18, false, true,
                    new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Drink", new RestoreHPEvent(1, 4, true)));
                ability.BeforeBeingHits.Add(0, new MultiplyElementEvent(07, 3, 2, false));
                ability.OnMapTurnEnds.Add(0, new WeatherAlignedEvent(2, 1));
            }
            else if (ii == 88)
            {
                ability.Name = new LocalText("Download");
                ability.Desc = new LocalText("Compares an opposing Pokémon's Defense and Sp. Def stats before raising its own Attack or Sp. Atk stat— whichever will be more effective.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent(Alignment.Foe, true, false, true, 100, new DownloadEvent(10, 12)));
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
                ability.OnTurnEnds.Add(0, new WeatherNeededSingleEvent(1, new CureAllEvent(new StringKey("MSG_CURE_SELF"), new AnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Wonder_Tile", 10))));
            }
            else if (ii == 94)
            {
                ability.Name = new LocalText("Solar Power");
                ability.Desc = new LocalText("Boosts the Sp. Atk stat in sunny weather, but HP decreases.");
                ability.OnActions.Add(0, new WeatherNeededEvent(2, new MultiplyCategoryEvent(BattleData.SkillCategory.Magical, 3, 2)));
                ability.AfterActions.Add(0, new WeatherNeededEvent(2, new OnAggressionEvent(new ChipDamageEvent(12, new StringKey(), true, true))));
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
                ability.OnActions.Add(-1, new ChangeMoveElementEvent(0, 13));
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
                ability.BeforeHittings.Add(0, new RevengeEvent(25, 5, 4, false, false));
            }
            else if (ii == 101)
            {
                ability.Name = new LocalText("Technician");
                ability.Desc = new LocalText("Powers up the Pokémon's weaker moves.");
                ability.OnActions.Add(0, new TechnicianEvent());
            }
            else if (ii == 102)
            {
                ability.Name = new LocalText("Leaf Guard");
                ability.Desc = new LocalText("Avoids enemy status moves in sunny weather.");
                ability.BeforeBeingHits.Add(0, new WeatherNeededEvent(2, new EvadeCategoryEvent(Alignment.Foe, BattleData.SkillCategory.Status)));
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
                ability.BeforeHittings.Add(0, new AttackingMoveNeededEvent(new TargetNeededEvent(Alignment.Friend | Alignment.Foe, new ChangeToAbilityEvent(0, true, true))));
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
                newData.HitRate = -1;
                newData.OnHits.Add(-1, new MaxHPDamageEvent(4));
                newData.OnHitTiles.Add(0, new RemoveTrapEvent());
                newData.OnHitTiles.Add(0, new RemoveItemEvent(true));
                newData.OnHitTiles.Add(0, new RemoveTerrainEvent("", new EmptyFiniteEmitter(), 2));
                newData.OnHitTiles.Add(0, new RemoveTerrainEvent("", new EmptyFiniteEmitter(), 7));
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
                ability.OnMapStarts.Add(0, new GiveStatusEvent(9, statusStates, false, new StringKey("MSG_SLOW_START")));
            }
            else if (ii == 113)
            {
                ability.Name = new LocalText("Scrappy");
                ability.Desc = new LocalText("The Pokémon can hit Ghost-type Pokémon with Normal- and Fighting-type moves.");
                ability.UserElementEffects.Add(0, new ScrappyEvent(13, 06));
            }
            else if (ii == 114)
            {
                ability.Name = new LocalText("Storm Drain");
                ability.Desc = new LocalText("Draws in all Water-type moves. Instead of being hit by Water-type moves, it boosts its Sp. Atk.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = Alignment.Foe;
                ability.ProximityEvent.BeforeExplosions.Add(0, new DrawAttackEvent(Alignment.Friend, 18, new StringKey("MSG_DRAW_ATTACK")));
                ability.BeforeBeingHits.Add(5, new AbsorbElementEvent(18, true, new StatusStackBattleEvent(12, true, false, 1)));
                ability.BeforeBeingHits.Add(5, new AddContextStateEvent(new SingleDrawAbsorb(), true));
            }
            else if (ii == 115)
            {
                ability.Name = new LocalText("Ice Body");
                ability.Desc = new LocalText("The Pokémon gradually regains HP when battling in a hailstorm.");
                ability.OnRefresh.Add(0, new MiscEvent(new HailState()));
                ability.AfterActions.Add(0, new WeatherNeededEvent(4, new OnMoveUseEvent(new RestoreHPEvent(1, 8, false))));
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
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent(4, 10, new StringKey("MSG_SNOW_WARNING"), typeof(ExtendWeatherState))));
            }
            else if (ii == 118)
            {
                ability.Name = new LocalText("Honey Gather");
                ability.Desc = new LocalText("The Pokémon may gather Nectar when it enters a new floor.");
                ability.OnMapStarts.Add(0, new GatherEvent(150));
            }
            else if (ii == 119)
            {
                ability.Name = new LocalText("Frisk");
                ability.Desc = new LocalText("Attacking an opposing Pokémon removes its held item.");
                ability.AfterHittings.Add(0, new OnHitEvent(true, false, 100, new DropItemEvent(false, true, -1, new HashSet<FlagType>(), new StringKey("MSG_FRISK"), true)));
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
                Dictionary<int, int> plate = new Dictionary<int, int>();
                for (int nn = 0; nn < 18; nn++)
                    plate[380 + nn] = nn + 1;
                ability.OnTurnStarts.Add(0, new PlateElementEvent(plate));
                plate = new Dictionary<int, int>();
                for (int nn = 0; nn < 18; nn++)
                    plate[380 + nn] = nn + 1;
                ability.OnTurnEnds.Add(0, new PlateElementEvent(plate));
                Dictionary<int, int> element = new Dictionary<int, int>();
                for (int nn = 0; nn < 18; nn++)
                    element[nn + 1] = 380 + nn;
                ability.BeforeBeingHits.Add(-1, new PlateProtectEvent(element));
            }
            else if (ii == 122)
            {
                ability.Name = new LocalText("Flower Gift");
                ability.Desc = new LocalText("Boosts the Attack and Sp. Def stats of itself and allies in harsh sunlight.");
                {
                    Dictionary<int, int> weather = new Dictionary<int, int>();
                    weather.Add(2, 1);
                    ability.OnMapStatusAdds.Add(0, new WeatherFormeChangeEvent(421, 0, weather));
                }
                {
                    Dictionary<int, int> weather = new Dictionary<int, int>();
                    weather.Add(2, 1);
                    ability.OnMapStatusRemoves.Add(0, new WeatherFormeChangeEvent(421, 0, weather));
                }
                {
                    Dictionary<int, int> weather = new Dictionary<int, int>();
                    weather.Add(2, 1);
                    ability.OnMapStarts.Add(-11, new WeatherFormeEvent(421, 0, weather));
                }
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = (Alignment.Self | Alignment.Friend);
                ability.ProximityEvent.OnActions.Add(0, new MultiplyCategoryInWeatherEvent(2, BattleData.SkillCategory.Physical, 5, 4));
                ability.ProximityEvent.BeforeBeingHits.Add(0, new MultiplyCategoryInWeatherEvent(2, BattleData.SkillCategory.Magical, 4, 5));
            }
            else if (ii == 123)
            {
                ability.Name = new LocalText("Bad Dreams");
                ability.Desc = new LocalText("Reduces the HP of opposing Pokémon that are asleep.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = Alignment.Foe;
                ability.ProximityEvent.OnTurnEnds.Add(0, new NightmareEvent(1, 8, new StringKey("MSG_HURT_BY_OTHER"), new AnimEvent(new SingleEmitter(new AnimData("Dark_Pulse_Ranger", 3)), "DUN_Night_Shade", 0)));
            }
            else if (ii == 124)
            {
                ability.Name = new LocalText("Pickpocket");
                ability.Desc = new LocalText("Steals an item from an attacker that made direct contact.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent(Alignment.Foe, true, true, true, 100, new StealItemEvent(true, false, 116, new HashSet<FlagType>(), new StringKey("MSG_STEAL_WITH"), false, true)));
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
                HashSet<FlagType> foods = new HashSet<FlagType>();
                foods.Add(new FlagType(typeof(EdibleState)));
                ability.ProximityEvent.BeforeTryActions.Add(0, new PreventItemUseEvent(new StringKey("MSG_UNNERVE"), foods));
                foods = new HashSet<FlagType>();
                foods.Add(new FlagType(typeof(EdibleState)));
                ability.ProximityEvent.BeforeActions.Add(0, new PreventItemUseEvent(new StringKey("MSG_UNNERVE"), foods));
                ability.ProximityEvent.BeforeBeingHits.Add(0, new DodgeFoodEvent(new StringKey("MSG_UNNERVE")));
            }
            else if (ii == 128)
            {
                ability.Name = new LocalText("Defiant");
                ability.Desc = new LocalText("Boosts the Pokémon's Attack stat sharply when its stats are lowered.");
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new StackState(2));
                ability.OnStatusAdds.Add(0, new StatDropResponseEvent(new GiveStatusEvent(10, statusStates, false)));
            }
            else if (ii == 129)
            {
                ability.Name = new LocalText("**Defeatist");
                ability.Desc = new LocalText("Halves the Pokémon's Attack and Sp. Atk stats when its HP becomes half or less.");
            }
            else if (ii == 130)
            {
                ability.Name = new LocalText("Cursed Body");
                ability.Desc = new LocalText("May disable a move used on the Pokémon.");
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Spite", 3));
                endAnim.Layer = DrawLayer.Back;
                endAnim.LocHeight = 24;
                CounterDisableBattleEvent counterDisable = new CounterDisableBattleEvent(60, new StringKey("MSG_CURSED_BODY"));
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
                reqEffect.BaseEvents.Add(new RaiseOneLowerOneEvent(9, 11, new StringKey("MSG_WEAK_ARMOR")));
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
                ability.OnActions.Add(0, new MultiplyCategoryInStatusEvent(5, BattleData.SkillCategory.Physical, 3, 2, false));
                ability.OnActions.Add(0, new MultiplyCategoryInStatusEvent(6, BattleData.SkillCategory.Physical, 3, 2, false));
            }
            else if (ii == 138)
            {
                ability.Name = new LocalText("Flare Boost");
                ability.Desc = new LocalText("Powers up special attacks when the Pokémon is burned.");
                ability.OnActions.Add(0, new MultiplyCategoryInStatusEvent(2, BattleData.SkillCategory.Magical, 3, 2, false));
            }
            else if (ii == 139)
            {
                ability.Name = new LocalText("Harvest");
                ability.Desc = new LocalText("Passes the effect of Berries to nearby allies.");
                RepeatEmitter emitter = new RepeatEmitter(new AnimData("Circle_Green_Out", 2));
                emitter.Bursts = 3;
                emitter.BurstTime = 8;
                ability.OnActions.Add(-3, new BerryAoEEvent(new StringKey("MSG_HARVEST"),emitter, "DUN_Me_First_2"));
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
                ability.AfterActions.Add(0, new OnMoveUseEvent(new MoodyEvent(10, 12)));
                ability.AfterBeingHits.Add(0, new HitCounterEvent(Alignment.Foe | Alignment.Friend, true, false, true, 100, new MoodyEvent(11, 13)));
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
                ability.AfterHittings.Add(0, new OnHitEvent(true, true, 25, new StatusBattleEvent(5, true, true, false, new StringKey("MSG_POISON_TOUCH"),
                    new BattleAnimEvent(emitter, "DUN_Toxic", true, 30))));
            }
            else if (ii == 144)
            {
                ability.Name = new LocalText("Regenerator");
                ability.Desc = new LocalText("Restores HP when there are no threats nearby.");
                ability.OnTurnEnds.Add(0, new RegeneratorEvent(8));
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
                ability.OnRefresh.Add(0, new WeatherSpeedEvent(3));
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
                ability.BeforeHittings.Add(0, new RevengeEvent(25, 5, 4, false, false));
            }
            else if (ii == 149)
            {
                ability.Name = new LocalText("Illusion");
                ability.Desc = new LocalText("Disguises itself as another Pokémon, fooling wild Pokémon of the same species.");
                ability.OnMapStarts.Add(0, new GiveIllusionEvent(111));
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
            }
            else if (ii == 152)
            {
                ability.Name = new LocalText("Mummy");
                ability.Desc = new LocalText("Contact with the Pokémon spreads this Ability.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 100, new ChangeToAbilityEvent(152, false, true)));
                ability.AfterHittings.Add(0, new OnHitEvent(true, true, 100, new ChangeToAbilityEvent(152, true, true)));
            }
            else if (ii == 153)
            {
                ability.Name = new LocalText("Moxie");
                ability.Desc = new LocalText("The Pokémon shows moxie, and that boosts the Attack stat after knocking out any Pokémon.");
                ability.AfterActions.Add(0, new KnockOutNeededEvent(new StatusStackBattleEvent(10, false, true, false, 1, new StringKey("MSG_MOXIE"))));
            }
            else if (ii == 154)
            {
                ability.Name = new LocalText("Justified");
                ability.Desc = new LocalText("Being hit by a Dark-type move boosts the Attack stat of the Pokémon, for justice.");
                ability.AfterBeingHits.Add(0, new ElementNeededEvent(02, new StatusStackBattleEvent(10, true, true, false, 1, new StringKey("MSG_JUSTIFIED"))));
            }
            else if (ii == 155)
            {
                ability.Name = new LocalText("Rattled");
                ability.Desc = new LocalText("Dark-, Ghost-, and Bug-type moves scare the Pokémon and boost its Movement Speed.");
                ability.AfterBeingHits.Add(0, new ElementNeededEvent(02, new StatusStackBattleEvent(9, true, true, false, 1, new StringKey("MSG_RATTLED"))));
                ability.AfterBeingHits.Add(0, new ElementNeededEvent(09, new StatusStackBattleEvent(9, true, true, false, 1, new StringKey("MSG_RATTLED"))));
                ability.AfterBeingHits.Add(0, new ElementNeededEvent(01, new StatusStackBattleEvent(9, true, true, false, 1, new StringKey("MSG_RATTLED"))));
            }
            else if (ii == 156)
            {
                ability.Name = new LocalText("Magic Bounce");
                ability.Desc = new LocalText("Reflects moves that cause status conditions.");
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Green", 2, -1, -1, 192), 3);
                ability.BeforeBeingHits.Add(-3, new ExceptInfiltratorEvent(true, new BounceStatusEvent(new StringKey("MSG_MAGIC_BOUNCE"), new BattleAnimEvent(emitter, "DUN_Light_Screen", true, 30))));
            }
            else if (ii == 157)
            {
                ability.Name = new LocalText("Sap Sipper");
                ability.Desc = new LocalText("Boosts the Attack stat if hit by a Grass-type move, instead of taking damage.");
                ability.ProximityEvent.Radius = 0;
                ability.ProximityEvent.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                ability.ProximityEvent.BeforeExplosions.Add(0, new IsolateElementEvent(10));
                ability.BeforeBeingHits.Add(5, new AbsorbElementEvent(10, false, true,
                    new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Drink", new StatusStackBattleEvent(10, true, false, 1)));
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
                ability.OnActions.Add(0, new WeatherNeededEvent(3, new MultiplyElementEvent(16, 4, 3, false),
                    new MultiplyElementEvent(11, 4, 3, false), new MultiplyElementEvent(17, 4, 3, false)));
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
                ability.ProximityEvent.BeforeStatusAdds.Add(0, new PreventStatusCheck(72, new StringKey("MSG_PROTECT_STATUS"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
                ability.ProximityEvent.BeforeStatusAdds.Add(0, new PreventStatusCheck(73, new StringKey("MSG_PROTECT_STATUS"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
                ability.ProximityEvent.BeforeStatusAdds.Add(0, new PreventStatusCheck(74, new StringKey("MSG_PROTECT_STATUS"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
                ability.ProximityEvent.BeforeStatusAdds.Add(0, new PreventStatusCheck(60, new StringKey("MSG_PROTECT_STATUS"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
            }
            else if (ii == 166)
            {
                ability.Name = new LocalText("Flower Veil");
                ability.Desc = new LocalText("Ally Grass-type Pokémon are protected from status conditions and the lowering of their stats.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = (Alignment.Self | Alignment.Friend);
                ability.ProximityEvent.BeforeStatusAdds.Add(0, new StateStatusCheck(typeof(BadStatusState), new StringKey("MSG_PROTECT_STATUS"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
                ability.ProximityEvent.BeforeStatusAdds.Add(0, new StatChangeCheck(new List<Stat>(), new StringKey("MSG_FLOWER_VEIL_STATS"), true, false, false, new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
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
                ability.AfterHittings.Add(0, new OnHitEvent(true, false, 100, new StealItemEvent(true, false, 116, new HashSet<FlagType>(), new StringKey("MSG_STEAL_WITH"), true, true)));
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
                ability.OnStatusAdds.Add(0, new StatDropResponseEvent(new GiveStatusEvent(12, statusStates, false)));
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
                ability.OnActions.Add(-1, new ChangeMoveElementEvent(13, 12));
            }
            else if (ii == 175)
            {
                ability.Name = new LocalText("Sweet Veil");
                ability.Desc = new LocalText("Prevents party Pokémon from falling asleep.");
                ability.ProximityEvent.Radius = 3;
                ability.ProximityEvent.TargetAlignments = (Alignment.Self | Alignment.Friend);
                ability.ProximityEvent.BeforeStatusAdds.Add(0, new PreventStatusCheck(1, new StringKey("MSG_SWEET_VEIL"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
                ability.ProximityEvent.BeforeStatusAdds.Add(0, new PreventStatusCheck(61, new StringKey("MSG_SWEET_VEIL_DROWSY"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", 10)));
            }
            else if (ii == 176)
            {
                ability.Name = new LocalText("**Stance Change");
                ability.Desc = new LocalText("The Pokémon changes its form to Blade Forme when it uses an attack move, and changes to Shield Forme when it uses King's Shield.");
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
                ability.BeforeBeingHits.Add(0, new WeatherNeededEvent(14, new MultiplyCategoryEvent(BattleData.SkillCategory.Physical, 1, 2, new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Screen_Hit", true, 10))));
            }
            else if (ii == 180)
            {
                ability.Name = new LocalText("**Symbiosis");
                ability.Desc = new LocalText("");
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
                ability.OnActions.Add(-1, new ChangeMoveElementEvent(13, 05));
            }
            else if (ii == 183)
            {
                ability.Name = new LocalText("Gooey");
                ability.Desc = new LocalText("Contact with the Pokémon lowers the attacker's Movement Speed.");
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 100, new StatusStackBattleEvent(9, false, true, -1)));
            }
            else if (ii == 184)
            {
                ability.Name = new LocalText("Aerilate");
                ability.Desc = new LocalText("Normal-type moves become Flying-type moves.");
                ability.OnActions.Add(-1, new ChangeMoveElementEvent(13, 08));
            }
            else if (ii == 185)
            {
                ability.Name = new LocalText("**Parental Bond");
                ability.Desc = new LocalText("Parent and child attack together.");
            }
            else if (ii == 186)
            {
                ability.Name = new LocalText("**Dark Aura");
                ability.Desc = new LocalText("Powers up all Dark-type moves.");
            }
            else if (ii == 187)
            {
                ability.Name = new LocalText("**Fairy Aura");
                ability.Desc = new LocalText("Powers up all Fairy-type moves.");
            }
            else if (ii == 188)
            {
                ability.Name = new LocalText("**Aura Break");
                ability.Desc = new LocalText("The effects of “Aura” Abilities are reversed to lower the power of affected moves.");
            }
            else if (ii == 189)
            {
                ability.Name = new LocalText("Primordial Sea");
                ability.Desc = new LocalText("Affects weather and nullifies any Fire-type attacks.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent(17)));
            }
            else if (ii == 190)
            {
                ability.Name = new LocalText("Desolate Land");
                ability.Desc = new LocalText("Affects weather and nullifies any Water-type attacks.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent(18)));
            }
            else if (ii == 191)
            {
                ability.Name = new LocalText("Delta Stream");
                ability.Desc = new LocalText("Affects weather and eliminates all of the Flying type's weaknesses.");
                ability.AfterActions.Add(0, new OnMoveUseEvent(new GiveMapStatusEvent(19)));
            }
            else if (ii == 192)
            {
                ability.Name = new LocalText("**Stamina");
                ability.Desc = new LocalText("Boosts the Defense stat when hit by an attack.");
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
                ability.Name = new LocalText("**Shields Down");
                ability.Desc = new LocalText("When its HP becomes half or less, the Pokémon's shell breaks and it becomes aggressive.");
            }
            else if (ii == 198)
            {
                ability.Name = new LocalText("**Stakeout");
                ability.Desc = new LocalText("Doubles the damage dealt to the target's replacement if the target switches out.");
            }
            else if (ii == 199)
            {
                ability.Name = new LocalText("**Water Bubble");
                ability.Desc = new LocalText("Lowers the power of Fire-type moves done to the Pokémon and prevents the Pokémon from getting a burn.");
            }
            else if (ii == 200)
            {
                ability.Name = new LocalText("**Steelworker");
                ability.Desc = new LocalText("Powers up Steel-type moves.");
            }
            else if (ii == 201)
            {
                ability.Name = new LocalText("**Berserk");
                ability.Desc = new LocalText("Boosts the Pokémon's Sp. Atk stat when it takes a hit that causes its HP to become half or less.");
            }
            else if (ii == 202)
            {
                ability.Name = new LocalText("**Slush Rush");
                ability.Desc = new LocalText("Boosts the Pokémon's Speed stat in a hailstorm.");
            }
            else if (ii == 203)
            {
                ability.Name = new LocalText("**Long Reach");
                ability.Desc = new LocalText("The Pokémon uses its moves without making contact with the target.");
            }
            else if (ii == 204)
            {
                ability.Name = new LocalText("**Liquid Voice");
                ability.Desc = new LocalText("All sound-based moves become Water-type moves.");
            }
            else if (ii == 205)
            {
                ability.Name = new LocalText("**Triage");
                ability.Desc = new LocalText("Gives priority to a healing move.");
            }
            else if (ii == 206)
            {
                ability.Name = new LocalText("Galvanize");
                ability.Desc = new LocalText("Normal-type moves become Electric-type moves.");
                ability.OnActions.Add(-1, new ChangeMoveElementEvent(13, 04));
            }
            else if (ii == 207)
            {
                ability.Name = new LocalText("**Surge Surfer");
                ability.Desc = new LocalText("Doubles the Pokémon's Speed stat on Electric Terrain.");
            }
            else if (ii == 208)
            {
                ability.Name = new LocalText("**Schooling");
                ability.Desc = new LocalText("When it has a lot of HP, the Pokémon forms a powerful school. It stops schooling when its HP is low.");
            }
            else if (ii == 209)
            {
                ability.Name = new LocalText("**Disguise");
                ability.Desc = new LocalText("Once per battle, the shroud that covers the Pokémon can protect it from an attack.");
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
                ability.Name = new LocalText("**Corrosion");
                ability.Desc = new LocalText("The Pokémon can poison the target even if it's a Steel or Poison type.");
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
                ability.Name = new LocalText("**Tangling Hair");
                ability.Desc = new LocalText("Contact with the Pokémon lowers the attacker's Speed stat.");
            }
            else if (ii == 222)
            {
                ability.Name = new LocalText("**Receiver");
                ability.Desc = new LocalText("The Pokémon copies the Ability of a defeated ally.");
            }
            else if (ii == 223)
            {
                ability.Name = new LocalText("**Power of Alchemy");
                ability.Desc = new LocalText("The Pokémon copies the Ability of a defeated ally.");
            }
            else if (ii == 224)
            {
                ability.Name = new LocalText("**Beast Boost");
                ability.Desc = new LocalText("The Pokémon boosts its most proficient stat each time it knocks out a Pokémon.");
            }
            else if (ii == 225)
            {
                ability.Name = new LocalText("**RKS System");
                ability.Desc = new LocalText("Changes the Pokémon's type to match the memory disc it holds.");
            }
            else if (ii == 226)
            {
                ability.Name = new LocalText("**Electric Surge");
                ability.Desc = new LocalText("Turns the ground into Electric Terrain when the Pokémon enters a battle.");
            }
            else if (ii == 227)
            {
                ability.Name = new LocalText("**Psychic Surge");
                ability.Desc = new LocalText("Turns the ground into Psychic Terrain when the Pokémon enters a battle.");
            }
            else if (ii == 228)
            {
                ability.Name = new LocalText("**Misty Surge");
                ability.Desc = new LocalText("Turns the ground into Misty Terrain when the Pokémon enters a battle.");
            }
            else if (ii == 229)
            {
                ability.Name = new LocalText("**Grassy Surge");
                ability.Desc = new LocalText("Turns the ground into Grassy Terrain when the Pokémon enters a battle.");
            }
            else if (ii == 230)
            {
                ability.Name = new LocalText("**Full Metal Body");
                ability.Desc = new LocalText("Prevents other Pokémon's moves or Abilities from lowering the Pokémon's stats.");
            }
            else if (ii == 231)
            {
                ability.Name = new LocalText("**Shadow Shield");
                ability.Desc = new LocalText("Reduces the amount of damage the Pokémon takes while its HP is full.");
            }
            else if (ii == 232)
            {
                ability.Name = new LocalText("**Prism Armor");
                ability.Desc = new LocalText("Reduces the power of supereffective attacks taken.");
            }
            else if (ii == 233)
            {
                ability.Name = new LocalText("**Neuroforce");
                ability.Desc = new LocalText("Powers up moves that are super effective.");
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
                ability.Name = new LocalText("**Libero");
                ability.Desc = new LocalText("Changes the Pokémon's type to the type of the move it's about to use.");
            }
            else if (ii == 237)
            {
                ability.Name = new LocalText("**Ball Fetch");
                ability.Desc = new LocalText("If the Pokémon is not holding an item, it will fetch the Poké Ball from the first failed throw of the battle.");
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
                ability.Name = new LocalText("**Mirror Armor");
                ability.Desc = new LocalText("Bounces back only the stat-lowering effects that the Pokémon receives.");
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
                ability.Name = new LocalText("**Ice Scales");
                ability.Desc = new LocalText("The Pokémon is protected by ice scales, which halve the damage taken from special moves.");
            }
            else if (ii == 247)
            {
                ability.Name = new LocalText("**Ripen");
                ability.Desc = new LocalText("Ripens Berries and doubles their effect.");
            }
            else if (ii == 248)
            {
                ability.Name = new LocalText("**Ice Face");
                ability.Desc = new LocalText("The Pokémon's ice head can take a physical attack as a substitute, but the attack also changes the Pokémon's appearance. The ice will be restored when it hails.");
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
                ability.Name = new LocalText("**Perish Body");
                ability.Desc = new LocalText("When hit by a move that makes direct contact, the Pokémon and the attacker will faint after three turns unless they switch out of battle.");
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
                ability.AfterBeingHits.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), 100, new ChangeToAbilityEvent(0, true, true)));
            }
            else if (ii == 257)
            {
                ability.Name = new LocalText("**Pastel Veil");
                ability.Desc = new LocalText("Protects the Pokémon and its ally Pokémon from being poisoned.");
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
                ability.Name = new LocalText("**Transistor");
                ability.Desc = new LocalText("Powers up Electric-type moves.");
            }
            else if (ii == 263)
            {
                ability.Name = new LocalText("**Dragon's Maw");
                ability.Desc = new LocalText("Powers up Dragon-type moves.");
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
                ability.Desc = new LocalText("This Ability combines the effects of both Calyrex's Unnerve Ability and Glastrier's Chilling Neigh Ability.");
            }
            else if (ii == 267)
            {
                ability.Name = new LocalText("**As One");
                ability.Desc = new LocalText("This Ability combines the effects of both Calyrex's Unnerve Ability and Spectrier's Grim Neigh Ability.");
            }

            if (ability.Name.DefaultText.StartsWith("**"))
                ability.Name.DefaultText = ability.Name.DefaultText.Replace("*", "");
            else if (ability.Name.DefaultText != "")
                ability.Released = true;

            return ability;
        }



    }
}
