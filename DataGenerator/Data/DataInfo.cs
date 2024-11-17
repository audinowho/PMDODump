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
using System.IO;
using PMDC.Dev;
using RogueEssence.LevelGen;

namespace DataGenerator.Data
{
    public static class DataInfo
    {
        public const int N_E = PreTypeEvent.N_E;
        public const int NVE = PreTypeEvent.NVE;
        public const int NRM = PreTypeEvent.NRM;
        public const int S_E = PreTypeEvent.S_E;

        public static void AddUniversalEvent()
        {
            File.Delete(PathMod.ModPath(DataManager.DATA_PATH + "Universal" + DataManager.DATA_EXT));
            UniversalActiveEffect universalEvent = new UniversalActiveEffect();

            universalEvent.OnHits.Add(5, new HitPostEvent("was_hurt_last_turn", "was_hurt_since_attack", "last_move_hit_by_other", "last_targeted_by", "crits_landed"));
            universalEvent.OnHitTiles.Add(5, new TilePostEvent());
            universalEvent.OnActions.Add(-10, new PreActionEvent("last_used_move_slot", "last_used_move", "times_move_used"));
            universalEvent.AfterActions.Add(5, new UsePostEvent("times_move_used", "last_ally_move", "missed_all_last_turn"));
            ElementMobilityEvent mobility = new ElementMobilityEvent();
            mobility.ElementPair["water"] = TerrainData.Mobility.Water;
            mobility.ElementPair["fire"] = TerrainData.Mobility.Lava;
            mobility.ElementPair["dragon"] = TerrainData.Mobility.Water | TerrainData.Mobility.Lava;
            mobility.ElementPair["flying"] = TerrainData.Mobility.Water | TerrainData.Mobility.Lava | TerrainData.Mobility.Abyss;
            mobility.ElementPair["ghost"] = TerrainData.Mobility.Block;
            universalEvent.OnRefresh.Add(-5, mobility);
            universalEvent.InitActionData.Add(-10, new PreSkillEvent());
            universalEvent.InitActionData.Add(-10, new PreItemEvent());
            universalEvent.InitActionData.Add(-10, new PreThrowEvent());
            universalEvent.OnEquips.Add(0, new CurseWarningEvent());
            universalEvent.BeforeHits.Add(-10, new PreHitEvent());
            universalEvent.BeforeHits.Add(10, new AttemptHitEvent());
            universalEvent.ElementEffects.Add(-10, new PreTypeEvent());
            universalEvent.OnDeaths.Add(-10, new PreDeathEvent());
            universalEvent.OnDeaths.Add(-10, new SetDeathEvent());
            universalEvent.OnDeaths.Add(0, new ImpostorReviveEvent("imposter", "transformed"));

            universalEvent.OnDeaths.Add(10, new HandoutRelativeExpEvent(1, 7, 5, 2));
            universalEvent.OnMapStarts.Add(-10, new SingleCharScriptEvent("UpdateEscort"));
            universalEvent.OnMapStarts.Add(-10, new StealthEvoEvent(35, "tandemaus"));
            universalEvent.OnMapStarts.Add(-10, new FadeInEvent());
            universalEvent.OnMapStarts.Add(-5, new SpecialIntroEvent());
            universalEvent.OnMapStarts.Add(-5, new ReactivateItemsEvent());
            universalEvent.ZoneSteps.Add(new ScriptZoneStep("SpawnRescueNote"));
            universalEvent.ZoneSteps.Add(new ScriptZoneStep("SpawnMissionNpcFromSV"));
            //UniversalEvent.OnWalks.Add(-5, new RevealFrontTrapEvent());


            HitRateLevelTableState hitRateTable = new HitRateLevelTableState(-6, 6, -6, 6);
            hitRateTable.AccuracyLevels = new int[13] { 105, 120, 140, 168, 210, 280, 420, 630, 840, 1050, 1260, 1470, 1680 };
            hitRateTable.EvasionLevels = new int[13] { 1680, 1470, 1260, 1050, 840, 630, 420, 280, 210, 168, 140, 120, 105 };
            universalEvent.UniversalStates.Set(hitRateTable);
            CritRateLevelTableState critRateTable = new CritRateLevelTableState();
            critRateTable.CritLevels = new int[5] { 0, 3, 4, 6, 12 };
            universalEvent.UniversalStates.Set(critRateTable);
            AtkDefLevelTableState dmgModTable = new AtkDefLevelTableState(-6, 6, -6, 6, 4, 4);
            universalEvent.UniversalStates.Set(dmgModTable);
            ElementTableState elementTable = new ElementTableState();

            elementTable.TypeMatchup = new int[19][];
            elementTable.TypeMatchup[00] = new int[19] { NRM,NRM,NRM,NRM,NRM,NRM,NRM,NRM,NRM,NRM,NRM,NRM,NRM,NRM,NRM,NRM,NRM,NRM,NRM};
            elementTable.TypeMatchup[01] = new int[19] { NRM,NRM,S_E,NRM,NRM,NVE,NVE,NVE,NVE,NVE,S_E,NRM,NRM,NRM,NVE,S_E,NRM,NVE,NRM};
            elementTable.TypeMatchup[02] = new int[19] { NRM,NRM,NVE,NRM,NRM,NVE,NVE,NRM,NRM,S_E,NRM,NRM,NRM,NRM,NRM,S_E,NRM,NRM,NRM};
            elementTable.TypeMatchup[03] = new int[19] { NRM,NRM,NRM,S_E,NRM,N_E,NRM,NRM,NRM,NRM,NRM,NRM,NRM,NRM,NRM,NRM,NRM,NVE,NRM};
            elementTable.TypeMatchup[04] = new int[19] { NRM,NRM,NRM,NVE,NVE,NRM,NRM,NRM,S_E,NRM,NVE,N_E,NRM,NRM,NRM,NRM,NRM,NRM,S_E};
            elementTable.TypeMatchup[05] = new int[19] { NRM,NRM,S_E,S_E,NRM,NRM,S_E,NVE,NRM,NRM,NRM,NRM,NRM,NRM,NVE,NRM,NRM,NVE,NRM};
            elementTable.TypeMatchup[06] = new int[19] { NRM,NVE,S_E,NRM,NRM,NVE,NRM,NRM,NVE,N_E,NRM,NRM,S_E,S_E,NVE,NVE,S_E,S_E,NRM};
            elementTable.TypeMatchup[07] = new int[19] { NRM,S_E,NRM,NVE,NRM,NRM,NRM,NVE,NRM,NRM,S_E,NRM,S_E,NRM,NRM,NRM,NVE,S_E,NVE};
            elementTable.TypeMatchup[08] = new int[19] { NRM,S_E,NRM,NRM,NVE,NRM,S_E,NRM,NRM,NRM,S_E,NRM,NRM,NRM,NRM,NRM,NVE,NVE,NRM};
            elementTable.TypeMatchup[09] = new int[19] { NRM,NRM,NVE,NRM,NRM,NRM,NRM,NRM,NRM,S_E,NRM,NRM,NRM,N_E,NRM,S_E,NRM,NRM,NRM};
            elementTable.TypeMatchup[10] = new int[19] { NRM,NVE,NRM,NVE,NRM,NRM,NRM,NVE,NVE,NRM,NVE,S_E,NRM,NRM,NVE,NRM,S_E,NVE,S_E};
            elementTable.TypeMatchup[11] = new int[19] { NRM,NVE,NRM,NRM,S_E,NRM,NRM,S_E,N_E,NRM,NVE,NRM,NRM,NRM,S_E,NRM,S_E,S_E,NRM};
            elementTable.TypeMatchup[12] = new int[19] { NRM,NRM,NRM,S_E,NRM,NRM,NRM,NVE,S_E,NRM,S_E,S_E,NVE,NRM,NRM,NRM,NRM,NVE,NVE};
            elementTable.TypeMatchup[13] = new int[19] { NRM,NRM,NRM,NRM,NRM,NRM,NRM,NRM,NRM,N_E,NRM,NRM,NRM,NRM,NRM,NRM,NVE,NVE,NRM};
            elementTable.TypeMatchup[14] = new int[19] { NRM,NRM,NRM,NRM,NRM,S_E,NRM,NRM,NRM,NVE,S_E,NVE,NRM,NRM,NVE,NRM,NVE,N_E,NRM};
            elementTable.TypeMatchup[15] = new int[19] { NRM,NRM,N_E,NRM,NRM,NRM,S_E,NRM,NRM,NRM,NRM,NRM,NRM,NRM,S_E,NVE,NRM,NVE,NRM};
            elementTable.TypeMatchup[16] = new int[19] { NRM,S_E,NRM,NRM,NRM,NRM,NVE,S_E,S_E,NRM,NRM,NVE,S_E,NRM,NRM,NRM,NRM,NVE,NRM};
            elementTable.TypeMatchup[17] = new int[19] { NRM,NRM,NRM,NRM,NVE,S_E,NRM,NVE,NRM,NRM,NRM,NRM,S_E,NRM,NRM,NRM,S_E,NVE,NVE};
            elementTable.TypeMatchup[18] = new int[19] { NRM,NRM,NRM,NVE,NRM,NRM,NRM,S_E,NRM,NRM,NVE,S_E,NRM,NRM,NRM,NRM,S_E,NRM,NVE};

            elementTable.Effectiveness = new int[11] { 0, 0, 0, 0, 0, 0, 1, 2, 4, 6, 9 };

            foreach (ElementInfo.Element type in Enum.GetValues(typeof(ElementInfo.Element)))
                elementTable.TypeMap[Text.Sanitize(type.ToString()).ToLower()] = (int)type;

            universalEvent.UniversalStates.Set(elementTable);
            universalEvent.UniversalStates.Set(new SkinTableState(1024, "shiny", "shiny_square"));


            DataManager.SaveData(universalEvent, DataManager.DATA_PATH, "Universal", DataManager.DATA_EXT);
        }

        public static void AddUniversalData()
        {
            File.Delete(PathMod.ModPath(DataManager.MISC_PATH + "Index" + DataManager.DATA_EXT));
            TypeDict<BaseData> baseData = new TypeDict<BaseData>();
            baseData.Set(new RarityData());
            baseData.Set(new MonsterFeatureData());
            DataManager.SaveData(baseData, DataManager.MISC_PATH, "Index", DataManager.DATA_EXT);
        }

        public static void AddEditorOps()
        {
            DeleteData(Path.Combine(PathMod.RESOURCE_PATH, "Extensions"));
            {
                CharSheetGenAnimOp op = new CharSheetGenAnimOp();
                DataManager.SaveObject(op, Path.Combine(PathMod.RESOURCE_PATH, "Extensions", "GenAnim" + ".op"));
            }
            {
                CharSheetAlignOp op = new CharSheetAlignOp();
                DataManager.SaveObject(op, Path.Combine(PathMod.RESOURCE_PATH, "Extensions", "Align" + ".op"));
            }
            {
                CharSheetMirrorOp op = new CharSheetMirrorOp();
                DataManager.SaveObject(op, Path.Combine(PathMod.RESOURCE_PATH, "Extensions", "MirrorLR" + ".op"));
            }
            {
                CharSheetMirrorOp op = new CharSheetMirrorOp();
                op.StartRight = true;
                DataManager.SaveObject(op, Path.Combine(PathMod.RESOURCE_PATH, "Extensions", "MirrorRL" + ".op"));
            }
            {
                CharSheetCollapseOffsetsOp op = new CharSheetCollapseOffsetsOp();
                DataManager.SaveObject(op, Path.Combine(PathMod.RESOURCE_PATH, "Extensions", "CollapseOffsets" + ".op"));
            }
        }

        public static void AddSystemFX()
        {
            DeleteData(PathMod.ModPath(DataManager.FX_PATH));
            {
                BattleFX fx = new BattleFX();
                fx.Sound = "DUN_Heal";

                RepeatEmitter lowEmit = new RepeatEmitter(new AnimData("Stat_White_Ring", 3));
                lowEmit.Bursts = 4;
                lowEmit.BurstTime = 9;
                lowEmit.LocHeight = -6;
                lowEmit.Layer = DrawLayer.Bottom;

                FiniteSprinkleEmitter emitter = new FiniteSprinkleEmitter(new AnimData("Event_Gather_Sparkle", 8));
                emitter.Range = 18;
                emitter.Speed = 36;
                emitter.TotalParticles = 8;
                emitter.StartHeight = 4;
                emitter.HeightSpeed = 36;
                emitter.SpeedDiff = 12;

                ListEmitter listEmitter = new ListEmitter();
                listEmitter.Layer = DrawLayer.NoDraw;
                listEmitter.Anim.Add(lowEmit);
                listEmitter.Anim.Add(emitter);

                fx.Emitter = listEmitter;

                fx.Delay = 20;

                DataManager.SaveData(fx, DataManager.FX_PATH, "Heal", DataManager.DATA_EXT);
            }
            {
                BattleFX fx = new BattleFX();
                fx.Sound = "DUN_PP_Up";

                SingleEmitter lowEmit = new SingleEmitter(new AnimData("Stat_Red_Ring", 3));
                lowEmit.LocHeight = -6;
                lowEmit.Layer = DrawLayer.Bottom;

                SqueezedAreaEmitter emitter = new SqueezedAreaEmitter(new AnimData("Stat_Red_Line", 2, Dir8.Up));
                emitter.Bursts = 3;
                emitter.ParticlesPerBurst = 2;
                emitter.BurstTime = 6;
                emitter.Range = GraphicsManager.TileSize;
                emitter.HeightSpeed = 6;

                ListEmitter listEmitter = new ListEmitter();
                listEmitter.Layer = DrawLayer.NoDraw;
                listEmitter.Anim.Add(lowEmit);
                listEmitter.Anim.Add(emitter);

                fx.Emitter = listEmitter;

                fx.Delay = 30;

                DataManager.SaveData(fx, DataManager.FX_PATH, "RestoreCharge", DataManager.DATA_EXT);
            }
            {
                BattleFX fx = new BattleFX();
                fx.Sound = "DUN_PP_Down";

                SqueezedAreaEmitter emitter = new SqueezedAreaEmitter(new AnimData("Stat_Red_Line", 2, Dir8.Down));
                emitter.Bursts = 3;
                emitter.ParticlesPerBurst = 2;
                emitter.BurstTime = 6;
                emitter.Range = GraphicsManager.TileSize;
                emitter.StartHeight = 0;
                emitter.HeightSpeed = 6;
                fx.Emitter = emitter;

                DataManager.SaveData(fx, DataManager.FX_PATH, "LoseCharge", DataManager.DATA_EXT);
            }
            {
                EmoteFX fx = new EmoteFX();
                fx.Sound = "EVT_Emote_Sweating";
                fx.Anim = new AnimData("Emote_Sweating", 2);
                DataManager.SaveData(fx, DataManager.FX_PATH, "NoCharge", DataManager.DATA_EXT);
            }
            {
                BattleFX fx = new BattleFX();
                fx.Sound = "DUN_Trace";

                FiniteReleaseEmitter emitter = new FiniteReleaseEmitter(new AnimData("Puff_Green", 3), new AnimData("Puff_Yellow", 3), new AnimData("Puff_Blue", 3), new AnimData("Puff_Red", 3));
                emitter.BurstTime = 4;
                emitter.ParticlesPerBurst = 1;
                emitter.Bursts = 4;
                emitter.Speed = 48;
                emitter.StartDistance = 4;
                fx.Emitter = emitter;
                fx.Delay = 20;
                DataManager.SaveData(fx, DataManager.FX_PATH, "Element", DataManager.DATA_EXT);
            }
            {
                BattleFX fx = new BattleFX();
                fx.Sound = "DUN_Trace";

                FiniteReleaseEmitter emitter = new FiniteReleaseEmitter(new AnimData("Puff_Green", 3), new AnimData("Puff_Yellow", 3), new AnimData("Puff_Blue", 3), new AnimData("Puff_Red", 3));
                emitter.BurstTime = 4;
                emitter.ParticlesPerBurst = 1;
                emitter.Bursts = 4;
                emitter.Speed = 48;
                emitter.StartDistance = 4;
                fx.Emitter = emitter;
                fx.Delay = 20;
                DataManager.SaveData(fx, DataManager.FX_PATH, "Intrinsic", DataManager.DATA_EXT);
            }
            {
                BattleFX fx = new BattleFX();
                fx.Sound = "DUN_Send_Home";

                SingleEmitter emitter = new SingleEmitter(new BeamAnimData("Column_Yellow", 3));
                emitter.Layer = DrawLayer.Front;
                fx.Emitter = emitter;

                DataManager.SaveData(fx, DataManager.FX_PATH, "SendHome", DataManager.DATA_EXT);
            }
            {
                BattleFX fx = new BattleFX();
                fx.Sound = "DUN_Ember";

                fx.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2));
                fx.Delay = 20;

                DataManager.SaveData(fx, DataManager.FX_PATH, "ItemLost", DataManager.DATA_EXT);
            }
            {
                BattleFX fx = new BattleFX();
                fx.Sound = "DUN_Warp";

                SingleEmitter emitter = new SingleEmitter(new AnimData("Circle_Red_Out", 3));
                emitter.Layer = DrawLayer.Front;
                fx.Emitter = emitter;

                DataManager.SaveData(fx, DataManager.FX_PATH, "Warp", DataManager.DATA_EXT);
            }
            {
                BattleFX fx = new BattleFX();
                fx.Sound = "DUN_Throw_Spike";

                DataManager.SaveData(fx, DataManager.FX_PATH, "Knockback", DataManager.DATA_EXT);
            }
            {
                BattleFX fx = new BattleFX();
                fx.Sound = "DUN_Pound";

                DataManager.SaveData(fx, DataManager.FX_PATH, "Jump", DataManager.DATA_EXT);
            }
            {
                BattleFX fx = new BattleFX();
                fx.Sound = "DUN_Throw_Spike";

                DataManager.SaveData(fx, DataManager.FX_PATH, "Throw", DataManager.DATA_EXT);
            }
        }

        public static void DeleteIndexedData(string subPath)
        {
            DeleteData(PathMod.ModPath(DataManager.DATA_PATH + subPath));
        }
        public static void DeleteData(string path)
        {
            string[] filePaths = Directory.GetFiles(path);
            foreach (string filePath in filePaths)
                File.Delete(filePath);
        }
    }
}

