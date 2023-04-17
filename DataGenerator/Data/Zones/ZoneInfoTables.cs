using RogueEssence.Content;
using RogueElements;
using RogueEssence.LevelGen;
using RogueEssence.Dungeon;
using RogueEssence.Ground;
using RogueEssence.Script;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using RogueEssence;
using RogueEssence.Data;
using PMDC.Dungeon;
using PMDC.LevelGen;
using PMDC;
using PMDC.Data;
using DataGenerator.Dev;

namespace DataGenerator.Data
{
    public partial class ZoneInfo
    {
        [Flags]
        public enum DungeonStage
        {
            Beginner = 0,
            Intermediate = 1,
            Advanced = 2
        }

        private static Dictionary<string, LocalText> specialRows;

        public static void InitStringsAll()
        {
            specialRows = Localization.readLocalizationRows(GenPath.TL_PATH + "Special.out.txt");
        }

        static MobSpawnStep<MapGenContext> GetUnownSpawns(string unown, int level)
        {
            MobSpawnStep<MapGenContext> spawnStep = new MobSpawnStep<MapGenContext>();
            PoolTeamSpawner poolSpawn = new PoolTeamSpawner();
            for (int ii = 0; ii < unown.Length; ii++)
            {
                if (unown[ii] == '!')
                    poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("unown", 26, "", Gender.Unknown), "", "hidden_power", "", "", "", new RandRange(level), "wander_normal"), 10);
                else if (unown[ii] == '?')
                    poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("unown", 27, "", Gender.Unknown), "", "hidden_power", "", "", "", new RandRange(level), "wander_normal"), 10);
                else
                {
                    int formNum = unown[ii] - 'a';
                    poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("unown", formNum, "", Gender.Unknown), "", "hidden_power", "", "", "", new RandRange(level), "wander_normal"), 10);
                }
            }
            poolSpawn.TeamSizes.Add(1, 12);
            poolSpawn.TeamSizes.Add(2, 6);
            spawnStep.Spawns.Add(poolSpawn, 100);
            return spawnStep;
        }

        static PlaceRandomMobsStep<MapGenContext> GetSingleSelectableSpawn(TeamMemberSpawn teamSpawn)
        {
            PoolTeamSpawner subSpawn = new PoolTeamSpawner();
            subSpawn.Spawns.Add(teamSpawn, 10);
            subSpawn.TeamSizes.Add(1, 12);

            LoopedTeamSpawner<MapGenContext> spawner = new LoopedTeamSpawner<MapGenContext>(subSpawn);
            spawner.AmountSpawner = new RandRange(2);
            PlaceRandomMobsStep<MapGenContext> mobStep = new PlaceRandomMobsStep<MapGenContext>(spawner);
            mobStep.ClumpFactor = 25;
            return mobStep;
        }

        static IFloorGen getSecretRoom(bool translate, string map_type, int moveBack, string wall, string floor, string water, string grass, string element, SpawnList<TeamMemberSpawn> enemies, params Loc[] locs)
        {
            LoadGen layout = new LoadGen();
            MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
            startGen.MapID = map_type;
            layout.GenSteps.Add(PR_FILE_LOAD, startGen);

            if (translate)
                layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapLoadContext>(specialRows["secretRoom"]));
            else
                layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapLoadContext>(new LocalText("Secret Room")));
            AddTitleDrop(layout);

            MapTimeLimitStep <MapLoadContext> floorData = new MapTimeLimitStep<MapLoadContext>(600);
            layout.GenSteps.Add(PR_FLOOR_DATA, floorData);

            //Tilesets
            if (String.IsNullOrEmpty(grass))
                AddTextureData(layout, wall, floor, water, element);
            else
                AddSpecificTextureData(layout, wall, floor, water, grass, element);

            {
                PoolTeamSpawner subSpawn = new PoolTeamSpawner();
                subSpawn.Spawns = enemies;
                subSpawn.TeamSizes.Add(1, 12);
                LoopedTeamSpawner<MapLoadContext> spawner = new LoopedTeamSpawner<MapLoadContext>(subSpawn);
                spawner.AmountSpawner = new RandRange(1, 3);
                PlaceTerrainMobsStep<MapLoadContext> mobStep = new PlaceTerrainMobsStep<MapLoadContext>(spawner);
                mobStep.AcceptedTiles.Add(new Tile("floor"));
                layout.GenSteps.Add(PR_SPAWN_MOBS, mobStep);
            }

            {
                HashSet<string> exceptFor = new HashSet<string>();
                foreach (string legend in IterateLegendaries())
                    exceptFor.Add(legend);
                SpeciesItemElementSpawner<MapLoadContext> spawn = new SpeciesItemElementSpawner<MapLoadContext>(new IntRange(2), new RandRange(locs.Length), element, exceptFor);
                BoxSpawner<MapLoadContext> box = new BoxSpawner<MapLoadContext>("box_heavy", spawn);
                List<Loc> treasureLocs = new List<Loc>();
                treasureLocs.AddRange(locs);
                layout.GenSteps.Add(PR_SPAWN_ITEMS, new SpecificSpawnStep<MapLoadContext, MapItem>(box, treasureLocs));
            }

            {
                EffectTile exitTile = new EffectTile("stairs_go_up", true);
                exitTile.TileStates.Set(new DestState(new SegLoc(moveBack, 1), true));
                MapTileStep<MapLoadContext> trapStep = new MapTileStep<MapLoadContext>(exitTile);

                trapStep.TerrainStencil = new MatchTileEffectStencil<MapLoadContext>("stairs_go_up");
                layout.GenSteps.Add(PR_EXITS, trapStep);
            }

            return layout;
        }

        static IFloorGen getMysteryRoom(bool translate, int zoneLevel, DungeonStage stage, string map_type, int moveBack)
        {
            GridFloorGen layout = new GridFloorGen();

            if (translate)
                layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(specialRows["mysteryPass"]));
            else
                layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(new LocalText("Mysterious Passage")));
            AddTitleDrop(layout);

            //Floor settings
            AddFloorData(layout, "B35. Mysterious Passage.ogg", 800, Map.SightRange.Dark, Map.SightRange.Dark);

            //Tilesets
            AddTextureData(layout, "the_nightmare_wall", "the_nightmare_floor", "the_nightmare_secondary", "normal");

            AddWaterSteps(layout, "pit", new RandRange(20));//water

            //enemies
            {
                RandGenStep<MapGenContext> chanceGenStep = new RandGenStep<MapGenContext>();
                SpawnList<GenStep<MapGenContext>> spawns = new SpawnList<GenStep<MapGenContext>>();
                spawns.Add(GetUnownSpawns("abcde", zoneLevel - 5), 10);
                spawns.Add(GetUnownSpawns("fghij", zoneLevel - 5), 10);
                spawns.Add(GetUnownSpawns("klmno", zoneLevel - 5), 10);
                spawns.Add(GetUnownSpawns("pqrst", zoneLevel - 5), 10);
                spawns.Add(GetUnownSpawns("uvwxyz", zoneLevel - 5), 10);
                chanceGenStep.Spawns = new LoopedRand<GenStep<MapGenContext>>(spawns, new RandRange(1));
                layout.GenSteps.Add(PR_RESPAWN_MOB, chanceGenStep);
            }
            // a rare mon appears, based on the difficulty level
            {
                MobSpawnStep<MapGenContext> spawnStep = new MobSpawnStep<MapGenContext>();
                PoolTeamSpawner poolSpawn = new PoolTeamSpawner();
                if (stage == DungeonStage.Beginner)
                    poolSpawn.Spawns.Add(GetTeamMob("smeargle", "", "sketch", "", "", "", new RandRange(zoneLevel), "wander_smart"), 10);
                else if (stage == DungeonStage.Intermediate)
                    poolSpawn.Spawns.Add(GetTeamMob("porygon", "", "tri_attack", "", "", "", new RandRange(zoneLevel), "wander_smart"), 10);
                else
                    poolSpawn.Spawns.Add(GetTeamMob("kecleon", "", "synchronoise", "thief", "", "", new RandRange(zoneLevel), "thief"), 10);
                poolSpawn.TeamSizes.Add(1, 12);
                spawnStep.Spawns.Add(poolSpawn, 20);
                layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);
            }

            AddRespawnData(layout, 7, 20);
            AddEnemySpawnData(layout, 20, new RandRange(7));

            //audino always appears once or thrice
            {
                PoolTeamSpawner subSpawn = new PoolTeamSpawner();
                subSpawn.Spawns.Add(GetTeamMob("audino", "", "secret_power", "", "", "", new RandRange(zoneLevel), "wander_smart"), 10);
                subSpawn.TeamSizes.Add(1, 12);
                LoopedTeamSpawner<MapGenContext> spawner = new LoopedTeamSpawner<MapGenContext>(subSpawn);
                spawner.AmountSpawner = new RandRange(1, 4);
                PlaceRandomMobsStep<MapGenContext> mobStep = new PlaceRandomMobsStep<MapGenContext>(spawner);
                mobStep.ClumpFactor = 25;
                layout.GenSteps.Add(PR_SPAWN_MOBS, mobStep);
            }

            //choose two fossils out of the entire selection, spawn two of each
            {
                RandGenStep<MapGenContext> chanceGenStep = new RandGenStep<MapGenContext>();
                SpawnList<GenStep<MapGenContext>> spawns = new SpawnList<GenStep<MapGenContext>>();
                spawns.Add(GetSingleSelectableSpawn(GetTeamMob("omanyte", "", "ancient_power", "brine", "", "", new RandRange(zoneLevel), "wander_smart")), 10);
                spawns.Add(GetSingleSelectableSpawn(GetTeamMob("kabuto", "", "ancient_power", "aqua_jet", "", "", new RandRange(zoneLevel), "wander_smart")), 10);
                spawns.Add(GetSingleSelectableSpawn(GetTeamMob("anorith", "", "ancient_power", "bug_bite", "", "", new RandRange(zoneLevel), "wander_smart")), 10);
                spawns.Add(GetSingleSelectableSpawn(GetTeamMob("lileep", "", "ancient_power", "ingrain", "", "", new RandRange(zoneLevel), "wander_smart")), 10);
                spawns.Add(GetSingleSelectableSpawn(GetTeamMob("cranidos", "", "ancient_power", "take_down", "", "", new RandRange(zoneLevel), "wander_smart")), 10);
                spawns.Add(GetSingleSelectableSpawn(GetTeamMob("shieldon", "", "ancient_power", "iron_defense", "", "", new RandRange(zoneLevel), "wander_smart")), 10);
                spawns.Add(GetSingleSelectableSpawn(GetTeamMob("amaura", "", "ancient_power", "aurora_beam", "", "", new RandRange(zoneLevel), "wander_smart")), 10);
                chanceGenStep.Spawns = new LoopedRand<GenStep<MapGenContext>>(spawns, new RandRange(2));
                layout.GenSteps.Add(PR_RESPAWN_MOB, chanceGenStep);
            }

            //items
            ItemSpawnStep<MapGenContext> itemSpawnZoneStep = new ItemSpawnStep<MapGenContext>();
            SpawnList<InvItem> items = new SpawnList<InvItem>();
            items.Add(new InvItem("berry_enigma"), 12);
            items.Add(new InvItem("orb_invert"), 4);
            itemSpawnZoneStep.Spawns.Add("uncategorized", items, 10);
            layout.GenSteps.Add(PR_RESPAWN_ITEM, itemSpawnZoneStep);
            AddItemData(layout, new RandRange(0, 3), 25);

            {
                List<MapItem> treasures = new List<MapItem>();
                treasures.Add(new MapItem("loot_pearl", 3));
                treasures.Add(new MapItem("loot_pearl", 2));
                treasures.Add(new MapItem("loot_pearl", 3));
                treasures.Add(new MapItem("loot_pearl", 2));
                PickerSpawner<MapGenContext, MapItem> treasurePicker = new PickerSpawner<MapGenContext, MapItem>(new PresetMultiRand<MapItem>(treasures));

                SpawnList<MapItem> recruitSpawn = new SpawnList<MapItem>();
                recruitSpawn.Add(new MapItem("apricorn_brown"), 10);
                recruitSpawn.Add(new MapItem("apricorn_purple"), 10);
                PickerSpawner<MapGenContext, MapItem> recruitPicker = new PickerSpawner<MapGenContext, MapItem>(new LoopedRand<MapItem>(recruitSpawn, new RandRange(1)));

                SpawnList<IStepSpawner<MapGenContext, MapItem>> boxSpawn = new SpawnList<IStepSpawner<MapGenContext, MapItem>>();

                //445      ***    Deluxe Box - 5* items
                boxSpawn.Add(new BoxSpawner<MapGenContext>("box_deluxe", new SpeciesItemContextSpawner<MapGenContext>(new IntRange(5), new RandRange(1))), 10);

                MultiStepSpawner<MapGenContext, MapItem> boxPicker = new MultiStepSpawner<MapGenContext, MapItem>(new LoopedRand<IStepSpawner<MapGenContext, MapItem>>(boxSpawn, new RandRange(1)));

                MultiStepSpawner<MapGenContext, MapItem> mainSpawner = new MultiStepSpawner<MapGenContext, MapItem>();
                mainSpawner.Picker = new PresetMultiRand<IStepSpawner<MapGenContext, MapItem>>(treasurePicker, recruitPicker, boxPicker);

                RandomRoomSpawnStep<MapGenContext, MapItem> specificItemZoneStep = new RandomRoomSpawnStep<MapGenContext, MapItem>(mainSpawner);
                specificItemZoneStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                layout.GenSteps.Add(PR_SPAWN_ITEMS, specificItemZoneStep);
            }

            //construct paths
            switch (map_type)
            {
                case "small_square":
                    {
                        AddInitGridStep(layout, 5, 5, 6, 6, 1, true);

                        GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.RoomRatio = new RandRange(80);
                        path.BranchRatio = new RandRange(30);

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //square
                        genericRooms.Add(new RoomGenSquare<MapGenContext>(new RandRange(3, 7), new RandRange(3, 7)), 10);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(20), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_GRID_GEN, path);

                        layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(40, 0));

                        layout.GenSteps.Add(PR_GRID_GEN, new SetGridDefaultsStep<MapGenContext>(new RandRange(25), GetImmutableFilterList()));
                        {
                            CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(3, 6), GetImmutableFilterList());
                            step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenCave<MapGenContext>(new RandRange(9, 13), new RandRange(5, 7))), 10);
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenCave<MapGenContext>(new RandRange(5, 7), new RandRange(9, 13))), 10);
                            layout.GenSteps.Add(PR_GRID_GEN, step);
                        }
                    }
                    break;
                case "tall_hall":
                    {
                        AddInitGridStep(layout, 3, 10, 3, 3, 1, true);

                        GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.RoomRatio = new RandRange(100);
                        path.BranchRatio = new RandRange(30);

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //cross
                        genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(2, 4), new RandRange(2, 4), new RandRange(1, 3), new RandRange(1, 3)), 10);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(20), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_GRID_GEN, path);

                        layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(40, 0));

                        layout.GenSteps.Add(PR_GRID_GEN, new SetGridDefaultsStep<MapGenContext>(new RandRange(25), GetImmutableFilterList()));
                        {
                            CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(6, 9), GetImmutableFilterList());
                            step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenBump<MapGenContext>(new RandRange(7), new RandRange(3), new RandRange(20, 60))), 10);
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenBump<MapGenContext>(new RandRange(3), new RandRange(7), new RandRange(20, 60))), 10);
                            layout.GenSteps.Add(PR_GRID_GEN, step);
                        }
                    }
                    break;
                case "wide_hall":
                    {
                        AddInitGridStep(layout, 12, 2, 4, 4, 1, true);

                        GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.RoomRatio = new RandRange(100);
                        path.BranchRatio = new RandRange(30);

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //cross
                        genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(2, 5), new RandRange(2, 5), new RandRange(1, 3), new RandRange(1, 3)), 10);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(20), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_GRID_GEN, path);

                        layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(40, 0));

                        layout.GenSteps.Add(PR_GRID_GEN, new SetGridDefaultsStep<MapGenContext>(new RandRange(25), GetImmutableFilterList()));
                        {
                            CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(6, 9), GetImmutableFilterList());
                            step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenBump<MapGenContext>(new RandRange(9), new RandRange(4), new RandRange(20, 60))), 10);
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenBump<MapGenContext>(new RandRange(4), new RandRange(9), new RandRange(20, 60))), 10);
                            layout.GenSteps.Add(PR_GRID_GEN, step);
                        }
                    }
                    break;
            }


            layout.GenSteps.Add(PR_ROOMS_INIT, new DrawGridToFloorStep<MapGenContext>());
            layout.GenSteps.Add(PR_TILES_INIT, new DrawFloorToTileStep<MapGenContext>());

            {
                EffectTile exitTile = new EffectTile("stairs_go_down", true);
                exitTile.TileStates.Set(new DestState(new SegLoc(moveBack, 1), true));
                var step = new FloorStairsStep<MapGenContext, MapGenEntrance, MapGenExit>(new MapGenEntrance(Dir8.Down), new MapGenExit(exitTile));
                step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                step.Filters.Add(new RoomFilterComponent(true, new BossRoom()));
                layout.GenSteps.Add(PR_EXITS, step);
            }

            return layout;
        }

        static AddBossRoomStep<ListMapGenContext> getBossRoom(string id)
        {
            if (id == "vespiquen")
            {
                string[] customWaterCross = new string[] {    "~~~...~~~",
                                                                      "~~~...~~~",
                                                                      "~~#...#~~",
                                                                      ".........",
                                                                      ".........",
                                                                      ".........",
                                                                      "~~#...#~~",
                                                                      "~~~...~~~",
                                                                      "~~~...~~~"};
                // vespiquen and its underlings
                //416 Vespiquen : 454 Attack Order : 455 Heal Order : 456 Defend Order : 408 Power Gem
                //415 Combee    : 230 Sweet Scent : 366 Tailwind : 450 Bug Bite : 283 Endeavor
                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("vespiquen", "", "attack_order", "defend_order", "heal_order", "power_gem", "", new Loc(4, 1)));
                mobSpawns.Add(GetBossMob("combee", "", "sweet_scent", "tailwind", "bug_bite", "endeavor", "", new Loc(4, 2)));
                mobSpawns.Add(GetBossMob("combee", "", "sweet_scent", "tailwind", "bug_bite", "endeavor", "", new Loc(1, 4)));
                mobSpawns.Add(GetBossMob("combee", "", "sweet_scent", "tailwind", "bug_bite", "endeavor", "", new Loc(7, 4)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customWaterCross, new Loc(4, 4), mobSpawns, false), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }


            if (id == "camerupt")
            {
                string[] customLavaLake = new string[] {      "#^^.^.^^#",
                                                                      "^^^^.^^^^",
                                                                      "^^^.^.^^^",
                                                                      ".^.....^.",
                                                                      "^.^...^.^",
                                                                      ".^.....^.",
                                                                      "^^^.^.^^^",
                                                                      "^^^^.^^^^",
                                                                      "#^^.^.^^#"};
                // lava plume synergy
                //323 Camerupt : 284 Eruption : 414 Earth Power : 436 Lava Plume : 281 Yawn
                //229 Houndoom : 53 Flamethrower : 46 Roar : 492 Foul Play : 517 Inferno
                //136 Flareon : 83 Fire Spin : 394 Flare Blitz : 98 Quick Attack : 436 Lava Plume
                //059 Arcanine : 257 Heat Wave : 555 Snarl : 245 Extreme Speed : 126 Fire Blast
                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("camerupt", "solid_rock", "eruption", "earth_power", "lava_plume", "yawn", "", new Loc(4, 2)));
                mobSpawns.Add(GetBossMob("houndoom", "flash_fire", "flamethrower", "roar", "foul_play", "inferno", "", new Loc(2, 4)));
                mobSpawns.Add(GetBossMob("flareon", "flash_fire", "fire_spin", "flare_blitz", "quick_attack", "lava_plume", "", new Loc(4, 6)));
                mobSpawns.Add(GetBossMob("arcanine", "flash_fire", "heat_wave", "snarl", "extreme_speed", "fire_blast", "", new Loc(6, 4)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customLavaLake, new Loc(4, 4), mobSpawns, true), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }



            if (id == "tyranitar")
            {
                string[] customJigsaw = new string[] {        "....###....",
                                                                      "##..###..##",
                                                                      "##.......##",
                                                                      "...........",
                                                                      "...........",
                                                                      "##.......##",
                                                                      "##..###..##",
                                                                      "....###...."};
                //    sand team
                //248 Tyranitar : 444 Stone Edge : 242 Crunch : 103 Screech : 269 Taunt
                //208 Steelix : 446 Stealth Rock : 231 Iron Tail : 91 Dig : 20 Bind
                //185 Sudowoodo : 359 Hammer Arm : 68 Counter : 335 Block : 452 Wood Hammer
                //232 Donphan : 484 Heavy Slam : 523 Bulldoze : 282 Knock Off : 205 Rollout
                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("tyranitar", "", "stone_edge", "crunch", "screech", "taunt", "", new Loc(3, 1)));
                mobSpawns.Add(GetBossMob("steelix", "", "stealth_rock", "iron_tail", "dig", "bind", "", new Loc(7, 1)));
                mobSpawns.Add(GetBossMob("sudowoodo", "", "hammer_arm", "counter", "block", "wood_hammer", "", new Loc(2, 3)));
                mobSpawns.Add(GetBossMob("donphan", "", "heavy_slam", "bulldoze", "knock_off", "rollout", "", new Loc(8, 3)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customJigsaw, new Loc(5, 3), mobSpawns, true), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }


            if (id == "dragonite")
            {
                string[] customSkyChex = new string[] {       "..___..",
                                                                      "..___..",
                                                                      "__...__",
                                                                      "__...__",
                                                                      "__...__",
                                                                      "..___..",
                                                                      "..___.."};
                //    dragon team
                //149 Dragonite : 63 Hyper Beam : 200 Outrage : 525 Dragon Tail : 355 Roost
                //130 Gyarados : 89 Earthquake : 127 Waterfall : 349 Dragon Dance : 423 Ice Fang
                //142 Aerodactyl : 446 Stealth Rock : 97 Agility : 157 Rock Slide : 469 Wide Guard
                //006 Charizard : 519 Fire Pledge : 82 Dragon Rage : 314 Air Cutter : 257 Heat Wave
                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("dragonite", "", "hyper_beam", "dragon_tail", "outrage", "roost", "", new Loc(0, 0)));
                mobSpawns.Add(GetBossMob("gyarados", "", "earthquake", "waterfall", "dragon_dance", "ice_fang", "", new Loc(6, 0)));
                mobSpawns.Add(GetBossMob("aerodactyl", "", "stealth_rock", "agility", "rock_slide", "wide_guard", "", new Loc(0, 6)));
                mobSpawns.Add(GetBossMob("charizard", "", "fire_pledge", "dragon_rage", "air_cutter", "heat_wave", "", new Loc(6, 6)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customSkyChex, new Loc(3, 3), mobSpawns, true), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }

            if (id == "salamence")
            {
                string[] customPillarHalls = new string[] {   ".........",
                                                                      "..#...#..",
                                                                      ".........",
                                                                      ".........",
                                                                      "..#...#..",
                                                                      ".........",
                                                                      ".........",
                                                                      "..#...#..",
                                                                      "........."};
                // dragon? team 2
                //373 Salamence : 434 Draco Meteor : 203 Endure : 82 Dragon Rage : 53 Flamethrower
                //160 Feraligatr : 276 Superpower : 401 Aqua Tail : 423 Ice Fang : 242 Crunch
                //475 Gallade : 370 Close Combat : 348 Leaf Blade : 427 Psycho Cut : 364 Feint
                //430 Honchkrow : 114 Haze : 355 Roost : 511 Quash : 399 Dark Pulse
                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("salamence", "", "draco_meteor", "endure", "dragon_rage", "flamethrower", "", new Loc(4, 3)));
                mobSpawns.Add(GetBossMob("feraligatr", "", "superpower", "aqua_tail", "ice_fang", "crunch", "", new Loc(2, 3)));
                mobSpawns.Add(GetBossMob("gallade", "", "close_combat", "leaf_blade", "psycho_cut", "feint", "", new Loc(6, 3)));
                mobSpawns.Add(GetBossMob("honchkrow", "", "haze", "roost", "quash", "dark_pulse", "", new Loc(4, 1)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customPillarHalls, new Loc(4, 5), mobSpawns, true), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }


            if (id == "claydol")
            {
                string[] customSkyDiamond = new string[] {    "###_._###",
                                                                      "##__.__##",
                                                                      "#___.___#",
                                                                      "____.____",
                                                                      ".........",
                                                                      "____.____",
                                                                      "#___.___#",
                                                                      "##__.__##",
                                                                      "###_._###"};
                //337 lunatone : 478 Magic Room : 94 Psychic : 322 Cosmic Power : 585 Moonblast
                //338 solrock : 377 Heal Block : 234 Morning Sun : 322 Cosmic Power : 76 Solar Beam
                //344 claydol : 286 Imprison : 471 Power Split : 153 Explosion : 326 Extrasensory
                //437 bronzong : 219 Safeguard : 319 Metal Sound : 248 Future Sight : 430 Flash Cannon
                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("lunatone", "", "magic_room", "psychic", "cosmic_power", "moonblast", "", new Loc(2, 2)));
                mobSpawns.Add(GetBossMob("solrock", "", "heal_block", "morning_sun", "cosmic_power", "solar_beam", "", new Loc(6, 2)));
                mobSpawns.Add(GetBossMob("claydol", "", "imprison", "power_split", "explosion", "extrasensory", "", new Loc(2, 6)));
                mobSpawns.Add(GetBossMob("bronzong", "", "safeguard", "metal_sound", "future_sight", "flash_cannon", "", new Loc(6, 6)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customSkyDiamond, new Loc(4, 4), mobSpawns, true), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }



            if (id == "ditto")
            {
                string[] customPillarCross = new string[] {   ".........",
                                                                      ".##...##.",
                                                                      ".##...##.",
                                                                      ".........",
                                                                      ".........",
                                                                      ".........",
                                                                      ".##...##.",
                                                                      ".##...##.",
                                                                      "........."};
                //   All ditto, no impostor
                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("ditto", "", "", "", "", "", "", new Loc(4, 1)));
                mobSpawns.Add(GetBossMob("ditto", "", "", "", "", "", "", new Loc(4, 7)));
                mobSpawns.Add(GetBossMob("ditto", "", "", "", "", "", "", new Loc(1, 4)));
                mobSpawns.Add(GetBossMob("ditto", "", "", "", "", "", "", new Loc(7, 4)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customPillarCross, new Loc(4, 4), mobSpawns, false), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }


            if (id == "clefable")
            {
                string[] customSideWalls = new string[] {     ".........",
                                                                      ".........",
                                                                      "..#...#..",
                                                                      "..#...#..",
                                                                      ".........",
                                                                      "..#...#..",
                                                                      "..#...#..",
                                                                      "........."};
                //   pink wall
                //36 Clefable : 98 Magic Guard : 266 Follow Me : 107 Minimize : 138 Dream Eater : 118 Metronome
                //35 Clefairy : 132 Friend Guard : 322 Cosmic Power : 381 Lucky Chant : 500 Stored Power : 236 Moonlight
                //39 Jigglypuff : 132 Friend Guard : 47 Sing : 50 Disable : 445 Captivate : 496 Round
                //440 Happiny : 132 Friend Guard : 204 Charm : 287 Refresh : 186 Sweet Kiss : 164 Substitute
                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("clefable", "magic_guard", "follow_me", "minimize", "dream_eater", "metronome", "", new Loc(4, 3)));
                mobSpawns.Add(GetBossMob("clefairy", "friend_guard", "moonlight", "cosmic_power", "lucky_chant", "stored_power", "", new Loc(4, 2)));
                mobSpawns.Add(GetBossMob("jigglypuff", "friend_guard", "sing", "disable", "captivate", "round", "", new Loc(3, 3)));
                mobSpawns.Add(GetBossMob("happiny", "friend_guard", "charm", "refresh", "sweet_kiss", "substitute", "", new Loc(5, 3)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customSideWalls, new Loc(4, 4), mobSpawns, false), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }


            if (id == "vaporeon")
            {
                string[] customSemiCovered = new string[] {   ".........",
                                                                      ".........",
                                                                      ".##...##.",
                                                                      ".........",
                                                                      ".........",
                                                                      "........."};
                //  Eeveelution 1
                // Vaporeon : Muddy Water : Aqua Ring : Aurora Beam : Helping Hand
                // Jolteon : Thunderbolt : Agility : Signal Beam : Stored Power
                // Flareon : Flare Blitz : Will-O-Wisp : Smog : Helping Hand
                // Sylveon : 585 Moonblast : 219 Safeguard : 247 Shadow Ball : Stored Power
                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("vaporeon", "", "muddy_water", "aqua_ring", "aurora_beam", "helping_hand", "", new Loc(4, 1)));
                mobSpawns.Add(GetBossMob("jolteon", "", "thunderbolt", "agility", "signal_beam", "stored_power", "", new Loc(3, 1)));
                mobSpawns.Add(GetBossMob("flareon", "", "flare_blitz", "will_o_wisp", "smog", "helping_hand", "", new Loc(5, 1)));
                mobSpawns.Add(GetBossMob("sylveon", "", "moonblast", "safeguard", "shadow_ball", "stored_power", "", new Loc(4, 0)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customSemiCovered, new Loc(4, 3), mobSpawns, false), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }

            if (id == "espeon")
            {
                string[] customSemiCovered = new string[] {   ".........",
                                                                      ".........",
                                                                      ".##...##.",
                                                                      ".........",
                                                                      ".........",
                                                                      "........."};
                //  Eeveelution 2
                // Espeon : Psyshock : Morning Sun : Dazzling Gleam : Stored Power
                // Leafeon : Leaf Blade : Grass Whistle : #-Scissor : Helping Hand
                // Glaceon : Frost Breath : Barrier : Water Pulse : Stored Power
                // Umbreon : Snarl : Moonlight : Shadow Ball : Helping Hand
                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("espeon", "", "psyshock", "morning_sun", "dazzling_gleam", "stored_power", "", new Loc(4, 1)));
                mobSpawns.Add(GetBossMob("leafeon", "", "leaf_blade", "grass_whistle", "x_scissor", "helping_hand", "", new Loc(3, 1)));
                mobSpawns.Add(GetBossMob("glaceon", "", "frost_breath", "barrier", "water_pulse", "stored_power", "", new Loc(5, 1)));
                mobSpawns.Add(GetBossMob("umbreon", "", "snarl", "moonlight", "shadow_ball", "helping_hand", "", new Loc(4, 0)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customSemiCovered, new Loc(4, 3), mobSpawns, false), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }


            if (id == "raichu")
            {
                string[] customTwoPillars = new string[] {    "...........",
                                                                      "..##...##..",
                                                                      "..##...##..",
                                                                      "...........",
                                                                      "..........."};
                //   Discharge + volt absorb
                //135 Jolteon : 10 Volt Absorb : 435 Discharge : 97 Agility : 113 Light Screen : 324 Signal Beam
                //417 Pachirisu : 10 Volt Absorb : 266 Follow Me : 162 Super Fang : 435 Discharge : 569 Ion Deluge
                //26 Raichu : 31 Lightning Rod : 447 Grass Knot : 85 Thunderbolt : 411 Focus Blast : 231 Iron Tail
                //101 Electrode : 106 Aftermath : 435 Discharge : 598 Eerie Impulse : 49 Sonic Boom : 268 Charge
                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("jolteon", "volt_absorb", "discharge", "agility", "light_screen", "signal_beam", "", new Loc(4, 1)));
                mobSpawns.Add(GetBossMob("pachirisu", "volt_absorb", "follow_me", "super_fang", "discharge", "ion_deluge", "", new Loc(6, 1)));
                mobSpawns.Add(GetBossMob("raichu", "lightning_rod", "grass_knot", "thunderbolt", "focus_blast", "iron_tail", "", new Loc(2, 3)));
                mobSpawns.Add(GetBossMob("electrode", "aftermath", "discharge", "eerie_impulse", "sonic_boom", "charge", "", new Loc(8, 3)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customTwoPillars, new Loc(5, 3), mobSpawns, false), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }

            //   Fossil team with ancient power; omastar has shell smash

            //entry hazards/gradual grind
            //forretress

            // redirection synergy; gyarados+lightningrod?

            //   PP stall team

            //   something with draco meteor - altaria? - also has haze support
            //   maybe an overheat team with haze support



            if (id == "skarmory")
            {
                //boss rooms
                string[] customShield = new string[] {          ".#######.",
                                                                ".........",
                                                                ".........",
                                                                ".........",
                                                                ".........",
                                                                "#.......#",
                                                                "#.......#",
                                                                "#.......#",
                                                                "##.....##"};
                //   skarmbliss
                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //227 Skarmory : 005 Sturdy : 319 Metal Sound : 314 Air Cutter : 191 Spikes : 092 Toxic
                //242 Blissey : 032 Serene Grace : 069 Seismic Toss : 135 Soft-Boiled : 287 Refresh : 196 Icy Wind
                mobSpawns.Add(GetBossMob("skarmory", "sturdy", "metal_sound", "air_cutter", "spikes", "toxic", "", new Loc(3, 2)));
                mobSpawns.Add(GetBossMob("blissey", "serene_grace", "seismic_toss", "soft_boiled", "refresh", "icy_wind", "", new Loc(5, 2)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customShield, new Loc(4, 4), mobSpawns, false), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }

            if (id == "umbreon")
            {
                string[] customEclipse = new string[] {         "##.....##",
                                                                "#.......#",
                                                                ".........",
                                                                ".........",
                                                                ".~.....~.",
                                                                ".~~...~~.",
                                                                "..~~~~~..",
                                                                "#..~~~..#",
                                                                "##.....##"};

                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //196 Espeon : 156 Magic Bounce : 094 Psychic : 247 Shadow Ball : 115 Reflect : 605 Dazzling Gleam
                mobSpawns.Add(GetBossMob("espeon", "magic_bounce", "psychic", "shadow_ball", "reflect", "dazzling_gleam", "", new Loc(3, 2)));
                //197 Umbreon : 028 Synchronize : 236 Moonlight : 212 Mean Look : 555 Snarl : 399 Dark Pulse
                mobSpawns.Add(GetBossMob("umbreon", "synchronize", "moonlight", "mean_look", "snarl", "dark_pulse", "", new Loc(5, 2)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customEclipse, new Loc(4, 4), mobSpawns, false), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }

            if (id == "ampharos")
            {
                string[] customBatteryReverse = new string[] {  ".........",
                                                                "..#####..",
                                                                ".........",
                                                                ".........",
                                                                ".........",
                                                                ".........",
                                                                "....#....",
                                                                "....#....",
                                                                "..#####..",
                                                                "....#....",
                                                                "....#...."};

                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //310 Manectric : 058 Minus : 604 Electric Terrain : 435 Discharge : 053 Flamethrower : 598 Eerie Impulse
                mobSpawns.Add(GetBossMob("manectric", "minus", "electric_terrain", "discharge", "flamethrower", "eerie_impulse", "", new Loc(3, 5)));
                //181 Ampharos : 057 Plus : 192 Zap Cannon : 406 Dragon Pulse : 324 Signal Beam : 602 Magnetic Flux
                mobSpawns.Add(GetBossMob("ampharos", "plus", "zap_cannon", "dragon_pulse", "signal_beam", "magnetic_flux", "", new Loc(5, 5)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customBatteryReverse, new Loc(4, 3), mobSpawns, true), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }

            if (id == "plusle")
            {
                string[] customBattery = new string[] {         "....#....",
                                                                "....#....",
                                                                "..#####..",
                                                                "....#....",
                                                                "....#....",
                                                                ".........",
                                                                ".........",
                                                                ".........",
                                                                ".........",
                                                                "..#####..",
                                                                "........."};

                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //311 Plusle : 057 Plus : 087 Thunder : 129 Swift : 417 Nasty Plot : 447 Grass Knot
                mobSpawns.Add(GetBossMob("plusle", "plus", "thunder", "swift", "nasty_plot", "grass_knot", "", new Loc(3, 5)));
                //312 Minun : 058 Minus : 240 Rain Dance : 097 Agility : 435 Discharge : 376 Trump Card
                mobSpawns.Add(GetBossMob("minun", "minus", "rain_dance", "agility", "discharge", "trump_card", "", new Loc(5, 5)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customBattery, new Loc(4, 7), mobSpawns, false), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }


            if (id == "tauros")
            {
                string[] customRailway = new string[] {         ".........",
                                                                ".........",
                                                                "..#...#..",
                                                                "..#...#..",
                                                                "..#...#..",
                                                                "..#...#..",
                                                                "..#...#..",
                                                                ".........",
                                                                "........."};

                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //128 Tauros : 022 Intimidate : 099 Rage : 037 Thrash : 523 Bulldoze : 371 Payback
                mobSpawns.Add(GetBossMob("tauros", "intimidate", "rage", "thrash", "bulldoze", "payback", "", new Loc(4, 3)));
                //241 Miltank : 113 Scrappy : 208 Milk Drink : 215 Heal Bell : 034 Body Slam : 045 Growl
                mobSpawns.Add(GetBossMob("miltank", "scrappy", "milk_drink", "heal_bell", "body_slam", "growl", "", new Loc(4, 2)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customRailway, new Loc(4, 5), mobSpawns, true), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }


            if (id == "mothim")
            {
                string[] customButterfly = new string[] {       "#.##.##.#",
                                                                "...#.#...",
                                                                ".........",
                                                                ".........",
                                                                "#.......#",
                                                                "##.....##",
                                                                ".........",
                                                                "...#.#...",
                                                                "#..#.#..#"};

                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //414 Mothim : 110 Tinted Lens : 318 Silver Wind : 483 Quiver Dance : 403 Air Slash : 094 Psychic
                mobSpawns.Add(GetBossMob("mothim", "tinted_lens", "silver_wind", "quiver_dance", "air_slash", "psychic", "", new Loc(3, 2)));
                //413 Wormadam : 107 Anticipation : 522 Struggle Bug : 319 Metal Sound : 450 Bug Bite : 527 Electroweb
                mobSpawns.Add(GetBossMob(new MonsterID("wormadam", 2, "", Gender.Unknown), "anticipation", "struggle_bug", "metal_sound", "bug_bite", "electroweb", "", new Loc(5, 2)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customButterfly, new Loc(4, 4), mobSpawns, false), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }


            if (id == "politoed")
            {
                string[] customWaterSwirl = new string[] {      "..~~~~~..",
                                                                ".~.....~.",
                                                                "~..~~~..~",
                                                                "~.~...~.~",
                                                                "~.~.....~",
                                                                "~.~....~.",
                                                                "~..~~~~..",
                                                                ".~......~",
                                                                "..~~~~~~."};
                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //186 Politoed : 002 Drizzle : 054 Mist : 095 Hypnosis : 352 Water Pulse : 058 Ice Beam
                mobSpawns.Add(GetBossMob("politoed", "drizzle", "mist", "hypnosis", "water_pulse", "ice_beam", "", new Loc(2, 2)));
                //062 Poliwrath : 033 Swift Swim : 187 Belly Drum : 127 Waterfall : 358 Wake-Up Slap : 509 Circle Throw
                mobSpawns.Add(GetBossMob("poliwrath", "swift_swim", "belly_drum", "waterfall", "wake_up_slap", "circle_throw", "", new Loc(6, 2)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customWaterSwirl, new Loc(4, 4), mobSpawns, true), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }

            if (id == "slowbro")
            {
                string[] customCrownWater = new string[] {  ".~~~.~~~.",
                                                            ".~~~.~~~.",
                                                            "..~...~..",
                                                            "..~...~..",
                                                            ".........",
                                                            ".........",
                                                            ".........",
                                                            "........."};

                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //080 Slowbro : 012 Oblivious : 505 Heal Pulse : 244 Psych Up : 352 Water Pulse : 094 Psychic
                mobSpawns.Add(GetBossMob("slowbro", "oblivious", "heal_pulse", "psych_up", "water_pulse", "psychic", "", new Loc(3, 3)));
                //199 Slowking : 012 Oblivious : 376 Trump Card : 347 Calm Mind : 408 Power Gem : 281 Yawn
                mobSpawns.Add(GetBossMob("slowking", "oblivious", "trump_card", "calm_mind", "power_gem", "yawn", "", new Loc(5, 3)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customCrownWater, new Loc(4, 5), mobSpawns, false), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }

            if (id == "nidoking")
            {
                string[] customCrown = new string[] {       ".###.###.",
                                                            ".###.###.",
                                                            "..#...#..",
                                                            "..#...#..",
                                                            ".........",
                                                            ".........",
                                                            ".........",
                                                            "........."};
                SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //034 Nidoking : 125 Sheer Force : 398 Poison Jab : 224 Megahorn : 116 Focus Energy : 529 Drill Run
                mobSpawns.Add(GetBossMob("nidoking", "sheer_force", "poison_jab", "megahorn", "focus_energy", "drill_run", "", new Loc(3, 3)));
                //031 Nidoqueen : 079 Rivalry : 270 Helping Hand : 445 Captivate : 414 Earth Power : 482 Sludge Wave
                mobSpawns.Add(GetBossMob("nidoqueen", "rivalry", "helping_hand", "captivate", "earth_power", "sludge_wave", "", new Loc(5, 3)));
                bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customCrown, new Loc(4, 5), mobSpawns, true), 10);
                return CreateGenericBossRoomStep(bossRooms);
            }


            throw new Exception("Invalid boss id");
        }


        static IEnumerable<string> IterateGummis()
        {
            yield return "gummi_wonder";
            yield return "gummi_blue";
            yield return "gummi_black";
            yield return "gummi_clear";
            yield return "gummi_grass";
            yield return "gummi_green";
            yield return "gummi_brown";
            yield return "gummi_orange";
            yield return "gummi_gold";
            yield return "gummi_pink";
            yield return "gummi_purple";
            yield return "gummi_red";
            yield return "gummi_royal";
            yield return "gummi_silver";
            yield return "gummi_white";
            yield return "gummi_yellow";
            yield return "gummi_sky";
            yield return "gummi_gray";
            yield return "gummi_magenta";
        }

        static IEnumerable<string> IterateXItems()
        {
            yield return "medicine_x_attack";
            yield return "medicine_x_defense";
            yield return "medicine_x_sp_atk";
            yield return "medicine_x_sp_def";
            yield return "medicine_x_speed";
            yield return "medicine_x_accuracy";
            yield return "medicine_dire_hit";
        }

        static IEnumerable<string> IteratePinchBerries()
        {
            yield return "berry_apicot";
            yield return "berry_liechi";
            yield return "berry_ganlon";
            yield return "berry_salac";
            yield return "berry_petaya";
            yield return "berry_starf";
            yield return "berry_micle";
        }

        static IEnumerable<string> IterateVitamins()
        {
            yield return "boost_nectar";
            yield return "boost_hp_up";
            yield return "boost_protein";
            yield return "boost_iron";
            yield return "boost_calcium";
            yield return "boost_zinc";
            yield return "boost_carbos";
        }

        static IEnumerable<string> IterateTypeBerries()
        {
            yield return "berry_tanga";
            yield return "berry_colbur";
            yield return "berry_haban";
            yield return "berry_wacan";
            yield return "berry_chople";
            yield return "berry_occa";
            yield return "berry_coba";
            yield return "berry_kasib";
            yield return "berry_rindo";
            yield return "berry_shuca";
            yield return "berry_yache";
            yield return "berry_chilan";
            yield return "berry_kebia";
            yield return "berry_payapa";
            yield return "berry_charti";
            yield return "berry_babiri";
            yield return "berry_passho";
            yield return "berry_roseli";
        }

        static IEnumerable<string> IterateApricorns()
        {
            yield return "apricorn_plain";
            yield return "apricorn_blue";
            yield return "apricorn_green";
            yield return "apricorn_brown";
            yield return "apricorn_purple";
            yield return "apricorn_red";
            yield return "apricorn_white";
            yield return "apricorn_yellow";
            yield return "apricorn_black";
        }

        static IEnumerable<string> IterateSilks()
        {
            yield return "xcl_element_bug_silk";
            yield return "xcl_element_dark_silk";
            yield return "xcl_element_dragon_silk";
            yield return "xcl_element_electric_silk";
            yield return "xcl_element_fairy_silk";
            yield return "xcl_element_fighting_silk";
            yield return "xcl_element_fire_silk";
            yield return "xcl_element_flying_silk";
            yield return "xcl_element_ghost_silk";
            yield return "xcl_element_grass_silk";
            yield return "xcl_element_ground_silk";
            yield return "xcl_element_ice_silk";
            yield return "xcl_element_normal_silk";
            yield return "xcl_element_poison_silk";
            yield return "xcl_element_psychic_silk";
            yield return "xcl_element_rock_silk";
            yield return "xcl_element_steel_silk";
            yield return "xcl_element_water_silk";
        }

        static IEnumerable<string> IterateLegendaries()
        {
            yield return "articuno";
            yield return "zapdos";
            yield return "moltres";
            yield return "mewtwo";
            yield return "mew";
            yield return "lugia";
            yield return "ho_oh";
            yield return "celebi";
            yield return "regirock";
            yield return "regice";
            yield return "registeel";
            yield return "latias";
            yield return "latios";
            yield return "kyogre";
            yield return "groudon";
            yield return "rayquaza";
            yield return "jirachi";
            yield return "deoxys";
            yield return "uxie";
            yield return "mesprit";
            yield return "azelf";
            yield return "dialga";
            yield return "palkia";
            yield return "heatran";
            yield return "regigigas";
            yield return "giratina";
            yield return "cresselia";
            yield return "phione";
            yield return "manaphy";
            yield return "darkrai";
            yield return "shaymin";
            yield return "arceus";
            yield return "victini";
            yield return "cobalion";
            yield return "terrakion";
            yield return "virizion";
            yield return "tornadus";
            yield return "thundurus";
            yield return "reshiram";
            yield return "zekrom";
            yield return "landorus";
            yield return "kyurem";
            yield return "keldeo";
            yield return "meloetta";
            yield return "genesect";
            yield return "xerneas";
            yield return "yveltal";
            yield return "zygarde";
            yield return "diancie";
            yield return "hoopa";
            yield return "volcanion";
            yield return "tapu_koko";
            yield return "tapu_lele";
            yield return "tapu_bulu";
            yield return "tapu_fini";
            yield return "cosmog";
            yield return "cosmoem";
            yield return "solgaleo";
            yield return "lunala";
            yield return "nihilego";
            yield return "buzzwole";
            yield return "pheromosa";
            yield return "xurkitree";
            yield return "celesteela";
            yield return "kartana";
            yield return "guzzlord";
            yield return "necrozma";
            yield return "magearna";
            yield return "marshadow";
            yield return "poipole";
            yield return "naganadel";
            yield return "stakataka";
            yield return "blacephalon";
            yield return "zeraora";
            yield return "meltan";
            yield return "melmetal";
            yield return "zacian";
            yield return "zamazenta";
            yield return "eternatus";
            yield return "kubfu";
            yield return "urshifu";
            yield return "zarude";
            yield return "regieleki";
            yield return "regidrago";
            yield return "glastrier";
            yield return "spectrier";
            yield return "calyrex";
        }

        [Flags]
        public enum TMClass
        {
            None = 0,
            Top = 1,
            Mid = 2,
            Bottom = 4,
            Starter = 8,
            Natural = 15,
            ShopOnly = 16
        }

        static IEnumerable<string> IterateTMs(TMClass tmClass)
        {
            if ((tmClass & TMClass.Top) != TMClass.None)
            {
                yield return "tm_earthquake";
                yield return "tm_hyper_beam";
                yield return "tm_overheat";
                yield return "tm_blizzard";
                yield return "tm_swords_dance";
                yield return "tm_surf";
                yield return "tm_dark_pulse";
                yield return "tm_psychic";
                yield return "tm_thunder";
                yield return "tm_shadow_ball";
                yield return "tm_ice_beam";
                yield return "tm_giga_impact";
                yield return "tm_fire_blast";
                yield return "tm_dazzling_gleam";
                yield return "tm_flash_cannon";
                yield return "tm_stone_edge";
                yield return "tm_sludge_bomb";
                yield return "tm_focus_blast";
            }

            if ((tmClass & TMClass.Mid) != TMClass.None)
            {
                yield return "tm_explosion";
                yield return "tm_snatch";
                yield return "tm_sunny_day";
                yield return "tm_rain_dance";
                yield return "tm_sandstorm";
                yield return "tm_hail";
                yield return "tm_x_scissor";
                yield return "tm_wild_charge";
                yield return "tm_taunt";
                yield return "tm_focus_punch";
                yield return "tm_safeguard";
                yield return "tm_light_screen";
                yield return "tm_psyshock";
                yield return "tm_will_o_wisp";
                yield return "tm_dream_eater";
                yield return "tm_nature_power";
                yield return "tm_facade";
                yield return "tm_swagger";
                yield return "tm_captivate";
                yield return "tm_rock_slide";
                yield return "tm_fling";
                yield return "tm_thunderbolt";
                yield return "tm_water_pulse";
                yield return "tm_shock_wave";
                yield return "tm_brick_break";
                yield return "tm_payback";
                yield return "tm_calm_mind";
                yield return "tm_reflect";
                yield return "tm_charge_beam";
                yield return "tm_flamethrower";
                yield return "tm_energy_ball";
                yield return "tm_retaliate";
                yield return "tm_scald";
                yield return "tm_waterfall";
                yield return "tm_roost";
                yield return "tm_rock_polish";
                yield return "tm_acrobatics";
                yield return "tm_rock_climb";
                yield return "tm_bulk_up";
                yield return "tm_pluck";
                yield return "tm_psych_up";
                yield return "tm_secret_power";
                yield return "tm_natural_gift";
            }

            if ((tmClass & TMClass.Bottom) != TMClass.None)
            {
                yield return "tm_return";
                yield return "tm_frustration";
                yield return "tm_giga_drain";
                yield return "tm_dive";
                yield return "tm_poison_jab";
                yield return "tm_torment";
                yield return "tm_shadow_claw";
                yield return "tm_endure";
                yield return "tm_echoed_voice";
                yield return "tm_gyro_ball";
                yield return "tm_recycle";
                yield return "tm_false_swipe";
                yield return "tm_defog";
                yield return "tm_telekinesis";
                yield return "tm_double_team";
                yield return "tm_thunder_wave";
                yield return "tm_attract";
                yield return "tm_steel_wing";
                yield return "tm_smack_down";
                yield return "tm_snarl";
                yield return "tm_flame_charge";
                yield return "tm_bulldoze";
                yield return "tm_substitute";
                yield return "tm_iron_tail";
                yield return "tm_brine";
                yield return "tm_venoshock";
                yield return "tm_u_turn";
                yield return "tm_aerial_ace";
                yield return "tm_hone_claws";
                yield return "tm_rock_smash";
            }

            if ((tmClass & TMClass.Starter) != TMClass.None)
            {
                yield return "tm_protect";
                yield return "tm_round";
                yield return "tm_rest";
                yield return "tm_hidden_power";
                yield return "tm_rock_tomb";
                yield return "tm_strength";
                yield return "tm_thief";
                yield return "tm_dig";
                yield return "tm_cut";
                yield return "tm_whirlpool";
                yield return "tm_grass_knot";
                yield return "tm_fly";
                yield return "tm_power_up_punch";
                yield return "tm_infestation";
                yield return "tm_work_up";
                yield return "tm_incinerate";
                yield return "tm_roar";
                yield return "tm_flash";
                yield return "tm_bullet_seed";
            }

            if ((tmClass & TMClass.ShopOnly) != TMClass.None)
            {
                yield return "tm_embargo";
                yield return "tm_dragon_claw";
                yield return "tm_low_sweep";
                yield return "tm_volt_switch";
                yield return "tm_dragon_pulse";
                yield return "tm_sludge_wave";
                yield return "tm_struggle_bug";
                yield return "tm_avalanche";
                yield return "tm_drain_punch";
                yield return "tm_dragon_tail";
                yield return "tm_silver_wind";
                yield return "tm_frost_breath";
                yield return "tm_sky_drop";
                yield return "tm_quash";
            }
        }



        [Flags]
        public enum TMDistroClass
        {
            None = 0,
            Universal = 1,
            High = 2,
            Medium = 4,
            Low = 8,
            Ordinary = 14,
            Natural = 15,
            ShopOnly = 16,
            NonUniversal = 30,
        }

        static IEnumerable<string> IterateDistroTMs(TMDistroClass tmClass)
        {
            if ((tmClass & TMDistroClass.Universal) != TMDistroClass.None)
            {
                yield return "tm_substitute";
                yield return "tm_protect";
                yield return "tm_facade";
                yield return "tm_round";
                yield return "tm_rest";
                yield return "tm_hidden_power";
                yield return "tm_return";
                yield return "tm_frustration";
                yield return "tm_double_team";
                yield return "tm_swagger";
                yield return "tm_secret_power";
                yield return "tm_attract";
                yield return "tm_endure";
                yield return "tm_natural_gift";
                yield return "tm_sunny_day";
                yield return "tm_rain_dance";
                yield return "tm_captivate";
            }


            if ((tmClass & TMDistroClass.High) != TMDistroClass.None)
            {
                yield return "tm_rock_smash";
                yield return "tm_thief";
                yield return "tm_flash";
                yield return "tm_shadow_ball";
                yield return "tm_psych_up";
                yield return "tm_rock_tomb";
                yield return "tm_strength";
                yield return "tm_rock_slide";
                yield return "tm_aerial_ace";
                yield return "tm_fling";
                yield return "tm_ice_beam";
                yield return "tm_dig";
                yield return "tm_safeguard";
                yield return "tm_thunder_wave";
                yield return "tm_grass_knot";
                yield return "tm_light_screen";
                yield return "tm_thunderbolt";
            }


            if ((tmClass & TMDistroClass.Medium) != TMDistroClass.None)
            {
                yield return "tm_blizzard";
                yield return "tm_water_pulse";
                yield return "tm_shock_wave";
                yield return "tm_bulldoze";
                yield return "tm_cut";
                yield return "tm_thunder";
                yield return "tm_psychic";
                yield return "tm_iron_tail";
                yield return "tm_taunt";
                yield return "tm_brick_break";
                yield return "tm_giga_impact";
                yield return "tm_echoed_voice";
                yield return "tm_payback";
                yield return "tm_earthquake";
                yield return "tm_hyper_beam";
                yield return "tm_sandstorm";
                yield return "tm_calm_mind";
                yield return "tm_reflect";
                yield return "tm_charge_beam";
                yield return "tm_dream_eater";
                yield return "tm_flamethrower";
                yield return "tm_swords_dance";
                yield return "tm_surf";
                yield return "tm_fire_blast";
                yield return "tm_energy_ball";
                yield return "tm_work_up";
                yield return "tm_incinerate";
                yield return "tm_hail";
                yield return "tm_retaliate";
                yield return "tm_power_up_punch";
                yield return "tm_roar";
                yield return "tm_torment";
                yield return "tm_shadow_claw";
                yield return "tm_u_turn";
                yield return "tm_whirlpool";
                yield return "tm_hone_claws";
                yield return "tm_dark_pulse";
                yield return "tm_stone_edge";
                yield return "tm_focus_punch";
                yield return "tm_sludge_bomb";
                yield return "tm_poison_jab";
                yield return "tm_giga_drain";
            }

            if ((tmClass & TMDistroClass.Low) != TMDistroClass.None)
            {
                yield return "tm_nature_power";
                yield return "tm_dive";
                yield return "tm_dazzling_gleam";
                yield return "tm_scald";
                yield return "tm_psyshock";
                yield return "tm_waterfall";
                yield return "tm_will_o_wisp";
                yield return "tm_roost";
                yield return "tm_telekinesis";
                yield return "tm_smack_down";
                yield return "tm_focus_blast";
                yield return "tm_wild_charge";
                yield return "tm_rock_polish";
                yield return "tm_fly";
                yield return "tm_steel_wing";
                yield return "tm_explosion";
                yield return "tm_acrobatics";
                yield return "tm_brine";
                yield return "tm_infestation";
                yield return "tm_gyro_ball";
                yield return "tm_recycle";
                yield return "tm_snatch";
                yield return "tm_false_swipe";
                yield return "tm_venoshock";
                yield return "tm_x_scissor";
                yield return "tm_rock_climb";
                yield return "tm_overheat";
                yield return "tm_defog";
                yield return "tm_bulk_up";
                yield return "tm_snarl";
                yield return "tm_flame_charge";
                yield return "tm_flash_cannon";
                yield return "tm_pluck";
                yield return "tm_bullet_seed";
            }

            if ((tmClass & TMDistroClass.ShopOnly) != TMDistroClass.None)
            {
                yield return "tm_embargo";
                yield return "tm_dragon_claw";
                yield return "tm_low_sweep";
                yield return "tm_volt_switch";
                yield return "tm_dragon_pulse";
                yield return "tm_sludge_wave";
                yield return "tm_struggle_bug";
                yield return "tm_avalanche";
                yield return "tm_drain_punch";
                yield return "tm_dragon_tail";
                yield return "tm_silver_wind";
                yield return "tm_frost_breath";
                yield return "tm_sky_drop";
                yield return "tm_quash";
            }
        }

        public static int getTMPrice(string tm_id)
        {
            switch (tm_id)
            {
                case "tm_earthquake": return 5000;
                case "tm_hyper_beam": return 6000;
                case "tm_overheat": return 5000;
                case "tm_blizzard": return 5000;
                case "tm_swords_dance": return 5000;
                case "tm_surf": return 5000;
                case "tm_dark_pulse": return 5000;
                case "tm_psychic": return 5000;
                case "tm_thunder": return 5000;
                case "tm_shadow_ball": return 5000;
                case "tm_ice_beam": return 5000;
                case "tm_giga_impact": return 6000;
                case "tm_fire_blast": return 5000;
                case "tm_dazzling_gleam": return 5000;
                case "tm_flash_cannon": return 5000;
                case "tm_stone_edge": return 5000;
                case "tm_sludge_bomb": return 5000;
                case "tm_focus_blast": return 5000;
                case "tm_explosion": return 3500;
                case "tm_snatch": return 3500;
                case "tm_sunny_day": return 3500;
                case "tm_rain_dance": return 3500;
                case "tm_sandstorm": return 3500;
                case "tm_hail": return 3500;
                case "tm_x_scissor": return 3500;
                case "tm_wild_charge": return 3500;
                case "tm_taunt": return 3500;
                case "tm_focus_punch": return 3500;
                case "tm_safeguard": return 3500;
                case "tm_light_screen": return 3500;
                case "tm_psyshock": return 3500;
                case "tm_will_o_wisp": return 3500;
                case "tm_dream_eater": return 3500;
                case "tm_nature_power": return 3500;
                case "tm_facade": return 3500;
                case "tm_swagger": return 3500;
                case "tm_captivate": return 3500;
                case "tm_rock_slide": return 3500;
                case "tm_fling": return 3500;
                case "tm_thunderbolt": return 3500;
                case "tm_water_pulse": return 3500;
                case "tm_shock_wave": return 3500;
                case "tm_brick_break": return 3500;
                case "tm_payback": return 3500;
                case "tm_calm_mind": return 3500;
                case "tm_reflect": return 3500;
                case "tm_charge_beam": return 3500;
                case "tm_flamethrower": return 3500;
                case "tm_energy_ball": return 3500;
                case "tm_retaliate": return 3500;
                case "tm_scald": return 3500;
                case "tm_waterfall": return 3500;
                case "tm_roost": return 3500;
                case "tm_rock_polish": return 3500;
                case "tm_acrobatics": return 3500;
                case "tm_rock_climb": return 3500;
                case "tm_bulk_up": return 3500;
                case "tm_pluck": return 3500;
                case "tm_psych_up": return 3500;
                case "tm_secret_power": return 3500;
                case "tm_natural_gift": return 3500;
                case "tm_return": return 2500;
                case "tm_frustration": return 2500;
                case "tm_giga_drain": return 2500;
                case "tm_dive": return 2500;
                case "tm_poison_jab": return 2500;
                case "tm_torment": return 2500;
                case "tm_shadow_claw": return 2500;
                case "tm_endure": return 2500;
                case "tm_echoed_voice": return 2500;
                case "tm_gyro_ball": return 2500;
                case "tm_recycle": return 2500;
                case "tm_false_swipe": return 2500;
                case "tm_defog": return 2500;
                case "tm_telekinesis": return 2500;
                case "tm_double_team": return 2500;
                case "tm_thunder_wave": return 2500;
                case "tm_attract": return 2500;
                case "tm_steel_wing": return 2500;
                case "tm_smack_down": return 2500;
                case "tm_snarl": return 2500;
                case "tm_flame_charge": return 2500;
                case "tm_bulldoze": return 2500;
                case "tm_substitute": return 2500;
                case "tm_iron_tail": return 2500;
                case "tm_brine": return 2500;
                case "tm_venoshock": return 2500;
                case "tm_u_turn": return 2500;
                case "tm_aerial_ace": return 2500;
                case "tm_hone_claws": return 2500;
                case "tm_rock_smash": return 2500;
                case "tm_protect": return 2500;
                case "tm_round": return 2500;
                case "tm_rest": return 2500;
                case "tm_hidden_power": return 2500;
                case "tm_rock_tomb": return 2500;
                case "tm_strength": return 2500;
                case "tm_thief": return 2500;
                case "tm_dig": return 3500;
                case "tm_cut": return 2500;
                case "tm_whirlpool": return 2500;
                case "tm_grass_knot": return 2500;
                case "tm_fly": return 3500;
                case "tm_power_up_punch": return 3500;
                case "tm_infestation": return 2500;
                case "tm_work_up": return 3500;
                case "tm_incinerate": return 2500;
                case "tm_roar": return 2500;
                case "tm_flash": return 2500;
                case "tm_bullet_seed": return 3500;
                case "tm_embargo": return 2500;
                case "tm_dragon_claw": return 3500;
                case "tm_low_sweep": return 2500;
                case "tm_volt_switch": return 3500;
                case "tm_dragon_pulse": return 5000;
                case "tm_sludge_wave": return 5000;
                case "tm_struggle_bug": return 2500;
                case "tm_avalanche": return 2500;
                case "tm_drain_punch": return 3500;
                case "tm_dragon_tail": return 3500;
                case "tm_silver_wind": return 5000;
                case "tm_frost_breath": return 3500;
                case "tm_sky_drop": return 2500;
                case "tm_quash": return 2500;
            }
            throw new Exception("Unknown TM ID");
        }

        static IEnumerable<string> IterateTypeBoosters()
        {
            yield return "held_silver_powder";
            yield return "held_black_glasses";
            yield return "held_dragon_scale";
            yield return "held_magnet";
            yield return "held_pink_bow";
            yield return "held_black_belt";
            yield return "held_charcoal";
            yield return "held_sharp_beak";
            yield return "held_spell_tag";
            yield return "held_miracle_seed";
            yield return "held_soft_sand";
            yield return "held_never_melt_ice";
            yield return "held_silk_scarf";
            yield return "held_poison_barb";
            yield return "held_twisted_spoon";
            yield return "held_hard_stone";
            yield return "held_metal_coat";
            yield return "held_mystic_water";
        }

        static IEnumerable<string> IterateTypePlates()
        {
            yield return "held_insect_plate";
            yield return "held_dread_plate";
            yield return "held_draco_plate";
            yield return "held_zap_plate";
            yield return "held_pixie_plate";
            yield return "held_fist_plate";
            yield return "held_flame_plate";
            yield return "held_sky_plate";
            yield return "held_spooky_plate";
            yield return "held_meadow_plate";
            yield return "held_earth_plate";
            yield return "held_icicle_plate";
            yield return "held_blank_plate";
            yield return "held_toxic_plate";
            yield return "held_mind_plate";
            yield return "held_stone_plate";
            yield return "held_iron_plate";
            yield return "held_splash_plate";
        }

        static IEnumerable<string> IterateManmades()
        {
            yield return "medicine_amber_tear";
            yield return "medicine_potion";
            yield return "medicine_max_potion";
            yield return "medicine_elixir";
            yield return "medicine_max_elixir";
            yield return "medicine_full_heal";
            yield return "medicine_full_restore";
            yield return "medicine_x_attack";
            yield return "medicine_x_defense";
            yield return "medicine_x_sp_atk";
            yield return "medicine_x_sp_def";
            yield return "medicine_x_speed";
            yield return "medicine_x_accuracy";
            yield return "medicine_dire_hit";
        }

        static IEnumerable<string> IterateMachines()
        {
            yield return "machine_recall_box";
            yield return "machine_assembly_box";
            yield return "machine_storage_box";
            yield return "machine_ability_capsule";
        }


        public static void CreateContentLists()
        {
            List<string> monsters = new List<string>();
            List<string> skills = new List<string>();
            List<string> intrinsics = new List<string>();
            List<string> statuses = new List<string>();
            List<string> ai = new List<string>();
            List<string> items = new List<string>();
            List<string> roles = new List<string>();



            foreach(string key in DataManager.Instance.DataIndices[DataManager.DataType.Monster].GetOrderedKeys(false))
                monsters.Add(key);
            
            foreach(string key in DataManager.Instance.DataIndices[DataManager.DataType.Skill].GetOrderedKeys(false))
                skills.Add(key);
            
            foreach(string key in DataManager.Instance.DataIndices[DataManager.DataType.Intrinsic].GetOrderedKeys(false))
                intrinsics.Add(key);

            foreach (string key in DataManager.Instance.DataIndices[DataManager.DataType.Status].GetOrderedKeys(false))
                statuses.Add(key);
            
            foreach(string key in DataManager.Instance.DataIndices[DataManager.DataType.AI].GetOrderedKeys(false))
                ai.Add(key);

            foreach (string key in DataManager.Instance.DataIndices[DataManager.DataType.Item].GetOrderedKeys(false))
                items.Add(key);

            for (int ii = 0; ii <= (int)TeamMemberSpawn.MemberRole.Loner; ii++)
                roles.Add(((TeamMemberSpawn.MemberRole)ii).ToString());


            string path = GenPath.ZONE_PATH + "PreZone.txt";
            using (StreamWriter file = new StreamWriter(path))
            {
                file.WriteLine("Monster\tSkill\tIntrinsic\tStatus\tAI\tItem\tRole");
                file.WriteLine("---\t---\t---\t---\t--\t----\t---");
                int ii = 0;
                bool completed = false;
                while (!completed)
                {
                    completed = true;

                    List<string> row = new List<string>();

                    if (ii < monsters.Count)
                    {
                        row.Add(monsters[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    if (ii < skills.Count)
                    {
                        row.Add(skills[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    if (ii < intrinsics.Count)
                    {
                        row.Add(intrinsics[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    if (ii < statuses.Count)
                    {
                        row.Add(statuses[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    if (ii < ai.Count)
                    {
                        row.Add(ai[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    if (ii < items.Count)
                    {
                        row.Add(items[ii]);
                        completed = ii >= 700;
                    }
                    else
                        row.Add("");
                    if (ii < roles.Count)
                    {
                        row.Add(roles[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");

                    file.WriteLine(String.Join('\t', row.ToArray()));
                    ii++;
                }
            }
        }

    }
}
