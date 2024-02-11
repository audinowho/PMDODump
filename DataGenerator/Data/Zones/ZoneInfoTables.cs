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
        public enum DungeonStage
        {
            Beginner = 0,
            Intermediate = 1,
            Advanced = 2,
            PostGame = 3,
            Rogue = 4
        }

        static MobSpawnStep<MapGenContext> GetUnownSpawns(string unown, int level, bool unrecruitable)
        {
            MobSpawnStep<MapGenContext> spawnStep = new MobSpawnStep<MapGenContext>();
            PoolTeamSpawner poolSpawn = new PoolTeamSpawner();
            for (int ii = 0; ii < unown.Length; ii++)
            {
                if (unown[ii] == '!')
                    poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("unown", 26, "", Gender.Unknown), "", "hidden_power", "", "", "", new RandRange(level), "wander_normal", false, unrecruitable), 10);
                else if (unown[ii] == '?')
                    poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("unown", 27, "", Gender.Unknown), "", "hidden_power", "", "", "", new RandRange(level), "wander_normal", false, unrecruitable), 10);
                else
                {
                    int formNum = unown[ii] - 'a';
                    poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("unown", formNum, "", Gender.Unknown), "", "hidden_power", "", "", "", new RandRange(level), "wander_normal", false, unrecruitable), 10);
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

        static IFloorGen getSecretRoom(bool translate, string map_type, int moveBack, string wall, string floor, string water, string grass, string element, DungeonStage gamePhase, DungeonAccessibility access, SpawnList<TeamMemberSpawn> enemies, params Loc[] locs)
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
                SpawnList<IStepSpawner<MapLoadContext, MapItem>> boxSpawn = new SpawnList<IStepSpawner<MapLoadContext, MapItem>>();

                HashSet<string> exceptFor = new HashSet<string>();
                foreach (string legend in IterateLegendaries())
                    exceptFor.Add(legend);
                SpeciesItemElementSpawner<MapLoadContext> spawn = new SpeciesItemElementSpawner<MapLoadContext>(new IntRange(2), new RandRange(1), element, exceptFor);
                boxSpawn.Add(new BoxSpawner<MapLoadContext>("box_heavy", spawn), 10);

                PopulateSecretRoomItems(boxSpawn, gamePhase, access);

                MultiStepSpawner <MapLoadContext, MapItem> boxPicker = new MultiStepSpawner<MapLoadContext, MapItem>(new LoopedRand<IStepSpawner<MapLoadContext, MapItem>>(boxSpawn, new RandRange(locs.Length)));

                List<Loc> treasureLocs = new List<Loc>();
                treasureLocs.AddRange(locs);
                layout.GenSteps.Add(PR_SPAWN_ITEMS, new SpecificSpawnStep<MapLoadContext, MapItem>(boxPicker, treasureLocs));
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

        static IFloorGen getMysteryRoom(bool translate, int zoneLevel, DungeonStage stage, string map_type, int moveBack, bool unrecruitable, bool noExp)
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
                spawns.Add(GetUnownSpawns("abcde", zoneLevel - 5, unrecruitable), 10);
                spawns.Add(GetUnownSpawns("fghij", zoneLevel - 5, unrecruitable), 10);
                spawns.Add(GetUnownSpawns("klmno", zoneLevel - 5, unrecruitable), 10);
                spawns.Add(GetUnownSpawns("pqrst", zoneLevel - 5, unrecruitable), 10);
                spawns.Add(GetUnownSpawns("uvwxyz", zoneLevel - 5, unrecruitable), 10);
                chanceGenStep.Spawns = new LoopedRand<GenStep<MapGenContext>>(spawns, new RandRange(1));
                layout.GenSteps.Add(PR_RESPAWN_MOB, chanceGenStep);
            }
            // a rare mon appears, based on the difficulty level
            if (!unrecruitable)
            {
                MobSpawnStep<MapGenContext> spawnStep = new MobSpawnStep<MapGenContext>();
                PoolTeamSpawner poolSpawn = new PoolTeamSpawner();
                if (stage == DungeonStage.Beginner || noExp)
                    poolSpawn.Spawns.Add(GetTeamMob("smeargle", "", "sketch", "", "", "", new RandRange(zoneLevel), "wander_smart"), 10);
                if (stage == DungeonStage.Intermediate || noExp)
                    poolSpawn.Spawns.Add(GetTeamMob("porygon", "", "tri_attack", "", "", "", new RandRange(zoneLevel), "wander_smart"), 10);
                if (stage == DungeonStage.Advanced || noExp)
                    poolSpawn.Spawns.Add(GetTeamMob("kecleon", "", "synchronoise", "thief", "", "", new RandRange(zoneLevel), "thief"), 10);
                poolSpawn.TeamSizes.Add(1, 12);
                spawnStep.Spawns.Add(poolSpawn, 20);
                layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);
            }

            AddRespawnData(layout, 7, 20);
            AddEnemySpawnData(layout, 20, new RandRange(7));

            //audino always appears once or thrice
            if (!noExp)
            {
                PoolTeamSpawner subSpawn = new PoolTeamSpawner();
                subSpawn.Spawns.Add(GetTeamMob("audino", "", "secret_power", "", "", "", new RandRange(zoneLevel), "wander_smart"), 10);
                subSpawn.TeamSizes.Add(1, 12);
                LoopedTeamSpawner<MapGenContext> spawner = new LoopedTeamSpawner<MapGenContext>(subSpawn);
                if (unrecruitable)
                    spawner.AmountSpawner = new RandRange(3, 6);
                else
                    spawner.AmountSpawner = new RandRange(1, 4);
                PlaceRandomMobsStep<MapGenContext> mobStep = new PlaceRandomMobsStep<MapGenContext>(spawner);
                mobStep.ClumpFactor = 25;
                layout.GenSteps.Add(PR_SPAWN_MOBS, mobStep);
            }

            //choose two fossils out of the entire selection, spawn two of each
            if (!unrecruitable)
            {
                RandGenStep<MapGenContext> chanceGenStep = new RandGenStep<MapGenContext>();
                SpawnList<GenStep<MapGenContext>> spawns = new SpawnList<GenStep<MapGenContext>>();
                spawns.Add(GetSingleSelectableSpawn(GetTeamMob("omanyte", "", "ancient_power", "brine", "", "", new RandRange(zoneLevel), "wander_smart")), 10);
                spawns.Add(GetSingleSelectableSpawn(GetTeamMob("kabuto", "", "ancient_power", "aqua_jet", "", "", new RandRange(zoneLevel), "wander_smart")), 10);
                //spawns.Add(GetSingleSelectableSpawn(GetTeamMob("anorith", "", "ancient_power", "bug_bite", "", "", new RandRange(zoneLevel), "wander_smart")), 10);
                //spawns.Add(GetSingleSelectableSpawn(GetTeamMob("lileep", "", "ancient_power", "ingrain", "", "", new RandRange(zoneLevel), "wander_smart")), 10);
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
                treasures.Add(new MapItem("loot_pearl", 3));
                if (!unrecruitable)
                {
                    treasures.Add(new MapItem("loot_pearl", 2));
                    treasures.Add(new MapItem("loot_pearl", 2));
                }
                PickerSpawner<MapGenContext, MapItem> treasurePicker = new PickerSpawner<MapGenContext, MapItem>(new PresetMultiRand<MapItem>(treasures));

                SpawnList<MapItem> recruitSpawn = new SpawnList<MapItem>();
                if (unrecruitable)
                    recruitSpawn.Add(new MapItem("loot_nugget"), 10);
                else
                {
                    recruitSpawn.Add(new MapItem("apricorn_brown"), 10);
                    recruitSpawn.Add(new MapItem("apricorn_purple"), 10);
                }
                PickerSpawner<MapGenContext, MapItem> recruitPicker = new PickerSpawner<MapGenContext, MapItem>(new LoopedRand<MapItem>(recruitSpawn, new RandRange(1)));

                SpawnList<IStepSpawner<MapGenContext, MapItem>> boxSpawn = new SpawnList<IStepSpawner<MapGenContext, MapItem>>();

                {
                    HashSet<string> exceptFor = new HashSet<string>();
                    foreach (string legend in IterateLegendaries())
                        exceptFor.Add(legend);
                    boxSpawn.Add(new BoxSpawner<MapGenContext>("box_deluxe", new SpeciesItemActiveTeamSpawner<MapGenContext>(new IntRange(1, 3), new RandRange(1), exceptFor, "unown")), 10);
                }

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

        static RoomGen<T> getBossRoomGen<T>(string id, int baseLv = 3, int scaleNum = 4, int scaleDen = 3) where T : ListMapGenContext
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
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("vespiquen", "", "attack_order", "defend_order", "heal_order", "power_gem", "", new Loc(4, 1), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("combee", "", "sweet_scent", "tailwind", "bug_bite", "endeavor", "", new Loc(4, 2), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("combee", "", "sweet_scent", "tailwind", "bug_bite", "endeavor", "", new Loc(1, 4), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("combee", "", "sweet_scent", "tailwind", "bug_bite", "endeavor", "", new Loc(7, 4), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customWaterCross, new Loc(4, 4), mobSpawns, false);
            }


            if (id == "camerupt" || id == "camerupt-water")
            {
                string[] customLavaLake;
                if (id == "camerupt")
                {
                    customLavaLake = new string[] {                   "#^^.^.^^#",
                                                                      "^^^^.^^^^",
                                                                      "^^^.^.^^^",
                                                                      ".^.....^.",
                                                                      "^.^...^.^",
                                                                      ".^.....^.",
                                                                      "^^^.^.^^^",
                                                                      "^^^^.^^^^",
                                                                      "#^^.^.^^#"};
                }
                else
                {
                    customLavaLake = new string[] {                   "#~~.~.~~#",
                                                                      "~~~~.~~~~",
                                                                      "~~~.~.~~~",
                                                                      ".~.....~.",
                                                                      "~.~...~.~",
                                                                      ".~.....~.",
                                                                      "~~~.~.~~~",
                                                                      "~~~~.~~~~",
                                                                      "#~~.~.~~#"};
                }
                // lava plume synergy
                //323 Camerupt : 284 Eruption : 414 Earth Power : 436 Lava Plume : 281 Yawn
                //229 Houndoom : 53 Flamethrower : 46 Roar : 492 Foul Play : 517 Inferno
                //136 Flareon : 83 Fire Spin : 394 Flare Blitz : 98 Quick Attack : 436 Lava Plume
                //059 Arcanine : 257 Heat Wave : 555 Snarl : 245 Extreme Speed : 126 Fire Blast
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("camerupt", "solid_rock", "eruption", "earth_power", "lava_plume", "yawn", "", new Loc(4, 1), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("houndoom", "flash_fire", "flamethrower", "roar", "foul_play", "inferno", "", new Loc(1, 4), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("flareon", "flash_fire", "fire_spin", "flare_blitz", "quick_attack", "lava_plume", "", new Loc(4, 7), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("arcanine", "flash_fire", "heat_wave", "snarl", "extreme_speed", "fire_blast", "", new Loc(7, 4), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customLavaLake, new Loc(4, 4), mobSpawns, true);
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
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("tyranitar", "", "stone_edge", "crunch", "screech", "taunt", "", new Loc(3, 1), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("steelix", "", "stealth_rock", "iron_tail", "dig", "bind", "", new Loc(7, 1), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("sudowoodo", "", "hammer_arm", "counter", "block", "wood_hammer", "", new Loc(2, 3), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("donphan", "", "heavy_slam", "bulldoze", "knock_off", "rollout", "", new Loc(8, 3), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customJigsaw, new Loc(5, 3), mobSpawns, true);
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
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("dragonite", "", "hyper_beam", "dragon_tail", "outrage", "roost", "", new Loc(0, 0), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("gyarados", "", "earthquake", "waterfall", "dragon_dance", "ice_fang", "", new Loc(6, 0), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("aerodactyl", "", "stealth_rock", "agility", "rock_slide", "wide_guard", "", new Loc(0, 6), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("charizard", "", "fire_pledge", "dragon_rage", "air_cutter", "heat_wave", "", new Loc(6, 6), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customSkyChex, new Loc(3, 3), mobSpawns, true);
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
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("salamence", "", "draco_meteor", "endure", "dragon_rage", "flamethrower", "", new Loc(4, 3), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("feraligatr", "", "superpower", "aqua_tail", "ice_fang", "crunch", "", new Loc(2, 3), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("gallade", "", "close_combat", "leaf_blade", "psycho_cut", "feint", "", new Loc(6, 3), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("honchkrow", "", "haze", "roost", "quash", "dark_pulse", "", new Loc(4, 1), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customPillarHalls, new Loc(4, 5), mobSpawns, true);
            }


            if (id == "claydol" || id == "claydol-water")
            {
                string[] customSkyDiamond;
                if (id == "claydol")
                {
                    customSkyDiamond = new string[] {    "###_._###",
                                                                      "##__.__##",
                                                                      "#___.___#",
                                                                      "____.____",
                                                                      ".........",
                                                                      "____.____",
                                                                      "#___.___#",
                                                                      "##__.__##",
                                                                      "###_._###"};
                }
                else
                {
                    customSkyDiamond = new string[] {                 "###~.~###",
                                                                      "##~~.~~##",
                                                                      "#~~~.~~~#",
                                                                      "~~~~.~~~~",
                                                                      ".........",
                                                                      "~~~~.~~~~",
                                                                      "#~~~.~~~#",
                                                                      "##~~.~~##",
                                                                      "###~.~###"};
                }
                //337 lunatone : 478 Magic Room : 94 Psychic : 322 Cosmic Power : 585 Moonblast
                //338 solrock : 377 Heal Block : 234 Morning Sun : 322 Cosmic Power : 76 Solar Beam
                //344 claydol : 286 Imprison : 471 Power Split : 153 Explosion : 326 Extrasensory
                //437 bronzong : 219 Safeguard : 319 Metal Sound : 248 Future Sight : 430 Flash Cannon
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("lunatone", "", "magic_room", "psychic", "cosmic_power", "moonblast", "", new Loc(2, 2), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("solrock", "", "heal_block", "morning_sun", "cosmic_power", "solar_beam", "", new Loc(6, 2), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("claydol", "", "imprison", "power_split", "explosion", "extrasensory", "", new Loc(2, 6), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("bronzong", "", "safeguard", "metal_sound", "future_sight", "flash_cannon", "", new Loc(6, 6), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customSkyDiamond, new Loc(4, 4), mobSpawns, true);
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
                //   All ditto, impostor
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("ditto", "imposter", "transform", "", "", "", "", new Loc(4, 1), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("ditto", "imposter", "transform", "", "", "", "", new Loc(4, 7), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("ditto", "imposter", "transform", "", "", "", "", new Loc(1, 4), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("ditto", "imposter", "transform", "", "", "", "", new Loc(7, 4), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customPillarCross, new Loc(4, 4), mobSpawns, false);
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
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("clefable", "magic_guard", "follow_me", "minimize", "dream_eater", "metronome", "", new Loc(4, 3), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("clefairy", "friend_guard", "moonlight", "cosmic_power", "lucky_chant", "stored_power", "", new Loc(4, 2), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("jigglypuff", "friend_guard", "sing", "disable", "captivate", "round", "", new Loc(3, 3), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("happiny", "friend_guard", "charm", "refresh", "sweet_kiss", "substitute", "", new Loc(5, 3), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customSideWalls, new Loc(4, 4), mobSpawns, false);
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
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("vaporeon", "", "muddy_water", "aqua_ring", "aurora_beam", "helping_hand", "", new Loc(4, 1), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("jolteon", "", "thunderbolt", "agility", "signal_beam", "stored_power", "", new Loc(3, 1), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("flareon", "", "flare_blitz", "will_o_wisp", "smog", "helping_hand", "", new Loc(5, 1), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("sylveon", "", "moonblast", "safeguard", "shadow_ball", "stored_power", "", new Loc(4, 0), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customSemiCovered, new Loc(4, 3), mobSpawns, false);
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
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("espeon", "", "psyshock", "morning_sun", "dazzling_gleam", "stored_power", "", new Loc(4, 1), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("leafeon", "", "leaf_blade", "grass_whistle", "x_scissor", "helping_hand", "", new Loc(3, 1), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("glaceon", "", "frost_breath", "barrier", "water_pulse", "stored_power", "", new Loc(5, 1), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("umbreon", "", "snarl", "moonlight", "shadow_ball", "helping_hand", "", new Loc(4, 0), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customSemiCovered, new Loc(4, 3), mobSpawns, false);
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
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("jolteon", "volt_absorb", "discharge", "agility", "light_screen", "signal_beam", "", new Loc(4, 1), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("pachirisu", "volt_absorb", "follow_me", "super_fang", "discharge", "ion_deluge", "", new Loc(6, 1), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("raichu", "lightning_rod", "grass_knot", "thunderbolt", "focus_blast", "iron_tail", "", new Loc(2, 3), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("electrode", "aftermath", "discharge", "eerie_impulse", "sonic_boom", "charge", "", new Loc(8, 3), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customTwoPillars, new Loc(5, 3), mobSpawns, false);
            }





            if (id == "hippowdon")
            {
                string[] customJigsaw = new string[] {                "...........",
                                                                      ".#.#.#.#.#.",
                                                                      "...........",
                                                                      "...........",
                                                                      "..#.....#..",
                                                                      "...........",
                                                                      ".#.#.#.#.#.",
                                                                      "..........."};
                //    sand team
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("hippowdon", "sand_stream", "yawn", "sand_tomb", "crunch", "take_down", "", new Loc(5, 2), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("cacturne", "sand_veil", "sand_attack", "ingrain", "needle_arm", "leech_seed", "", new Loc(3, 2), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("sandslash", "sand_veil", "fury_swipes", "crush_claw", "sand_tomb", "dig", "", new Loc(7, 2), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customJigsaw, new Loc(5, 4), mobSpawns, false);
            }

            if (id == "thievul")
            {
                string[] customJigsaw = new string[] {                "###...###",
                                                                      "###...###",
                                                                      "###...###",
                                                                      "###...###",
                                                                      ".........",
                                                                      "###...###",
                                                                      "###...###",
                                                                      "###...###",
                                                                      "###...###"};
                //    thief team
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("thievul", "stakeout", "sucker_punch", "foul_play", "tail_slap", "thief", "", new Loc(4, 2), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("scrafty", "shed_skin", "low_kick", "headbutt", "payback", "chip_away", "", new Loc(3, 2), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("cacturne", "sand_veil", "growth", "destiny_bond", "revenge", "payback", "", new Loc(5, 2), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customJigsaw, new Loc(4, 4), mobSpawns, false);
            }

            if (id == "drapion")
            {
                string[] customJigsaw = new string[] {                ".........",
                                                                      "....#....",
                                                                      "...#.#...",
                                                                      "..#...#..",
                                                                      ".........",
                                                                      ".........",
                                                                      ".........",
                                                                      ".........",
                                                                      "........."};
                //    tank team
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("drapion", "battle_armor", "acupressure", "knock_off", "toxic_spikes", "bite", "", new Loc(4, 3), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("skarmory", "sturdy", "spikes", "steel_wing", "sand_attack", "agility", "", new Loc(2, 5), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("arbok", "shed_skin", "stockpile", "coil", "swallow", "wrap", "", new Loc(6, 5), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customJigsaw, new Loc(4, 5), mobSpawns, false);
            }

            if (id == "lycanroc")
            {
                string[] customJigsaw = new string[] {                "...###...",
                                                                      "...###...",
                                                                      ".........",
                                                                      "##.....##",
                                                                      "##.....##",
                                                                      "##.....##",
                                                                      ".........",
                                                                      ".........",
                                                                      "........."};
                //    tank team
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob(new MonsterID("lycanroc", 2, "", Gender.Unknown), "", "rock_climb", "rock_slide", "thrash", "crunch", "", new Loc(4, 2), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob(new MonsterID("lycanroc", 0, "", Gender.Unknown), "steadfast", "accelerock", "wide_guard", "roar", "stealth_rock", "", new Loc(2, 3), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob(new MonsterID("lycanroc", 1, "", Gender.Unknown), "vital_spirit", "reversal", "counter", "roar", "stealth_rock", "", new Loc(6, 3), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customJigsaw, new Loc(4, 5), mobSpawns, false);
            }

            if (id == "armaldo")
            {
                string[] customJigsaw = new string[] {                ".#.#.#.#.#.",
                                                                      ".#.#...#.#.",
                                                                      ".#.......#.",
                                                                      "...........",
                                                                      ".#.......#.",
                                                                      ".#.#...#.#.",
                                                                      ".#.#.#.#.#."};
                //    hoenn fossils
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("torkoal", "white_smoke", "heat_wave", "iron_defense", "smokescreen", "smog", "", new Loc(5, 1), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("cradily", "", "wring_out", "ingrain", "giga_drain", "gastro_acid", "", new Loc(3, 2), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("armaldo", "", "x_scissor", "metal_claw", "crush_claw", "brine", "", new Loc(7, 2), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customJigsaw, new Loc(5, 4), mobSpawns, false);
            }

            if (id == "bastiodon")
            {
                string[] customJigsaw = new string[] {                "...........",
                                                                      "####...####",
                                                                      "...........",
                                                                      "##.......##",
                                                                      "...........",
                                                                      "####...####",
                                                                      "..........."};
                //    sinnoh fossils
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("bronzong", "levitate", "rain_dance", "psywave", "safeguard", "imprison", "", new Loc(5, 1), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("bastiodon", "", "iron_defense", "metal_burst", "protect", "iron_head", "", new Loc(3, 2), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("rampardos", "", "head_smash", "chip_away", "zen_headbutt", "take_down", "", new Loc(7, 2), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customJigsaw, new Loc(5, 5), mobSpawns, false);
            }

            if (id == "aerodactyl")
            {
                string[] customJigsaw = new string[] {                "##.......##",
                                                                      "#.........#",
                                                                      "..##...##..",
                                                                      "..##...##..",
                                                                      "...........",
                                                                      "#.........#",
                                                                      "##.#.#.#.##",
                                                                      "##.#.#.#.##"};
                //    kanto fossils
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("aerodactyl", "", "sky_drop", "rock_slide", "roar", "crunch", "", new Loc(5, 1), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("omastar", "", "rock_blast", "brine", "mud_shot", "tickle", "", new Loc(1, 4), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("kabutops", "", "night_slash", "feint", "aqua_jet", "endure", "", new Loc(9, 4), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customJigsaw, new Loc(5, 4), mobSpawns, false);
            }

            if (id == "flygon")
            {
                string[] customJigsaw = new string[] {                "###...###",
                                                                      "##.....##",
                                                                      "#..#.#..#",
                                                                      "..#...#..",
                                                                      ".........",
                                                                      ".........",
                                                                      ".........",
                                                                      "..#...#..",
                                                                      "#..#.#..#",
                                                                      "##.....##",
                                                                      "###...###"};
                //    flygon filler
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("flygon", "", "sonic_boom", "sandstorm", "uproar", "earth_power", "", new Loc(4, 3), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("dugtrio", "arena_trap", "magnitude", "night_slash", "growl", "mud_bomb", "", new Loc(2, 4), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("fearow", "", "drill_run", "drill_peck", "pursuit", "mirror_move", "", new Loc(6, 4), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customJigsaw, new Loc(4, 6), mobSpawns, false);
            }

            if (id == "arbok")
            {
                string[] customJigsaw = new string[] {                "#.........#",
                                                                      "...........",
                                                                      ".##.....##.",
                                                                      ".###...###.",
                                                                      "...........",
                                                                      "...........",
                                                                      "#.........#"};
                //    dusclops filler
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                mobSpawns.Add(GetBossMob("arbok", "intimidate", "glare", "stockpile", "spit_up", "acid_spray", "", new Loc(5, 2), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("marowak", "lightning_rod", "retaliate", "bonemerang", "bone_club", "rage", "", new Loc(2, 4), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("drapion", "sniper", "thunder_fang", "pursuit", "night_slash", "poison_fang", "", new Loc(8, 4), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customJigsaw, new Loc(5, 5), mobSpawns, false);
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
                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //227 Skarmory : 005 Sturdy : 319 Metal Sound : 314 Air Cutter : 191 Spikes : 092 Toxic
                //242 Blissey : 032 Serene Grace : 069 Seismic Toss : 135 Soft-Boiled : 287 Refresh : 196 Icy Wind
                mobSpawns.Add(GetBossMob("skarmory", "sturdy", "metal_sound", "air_cutter", "spikes", "toxic", "", new Loc(3, 2), baseLv, scaleNum, scaleDen));
                mobSpawns.Add(GetBossMob("blissey", "serene_grace", "seismic_toss", "soft_boiled", "refresh", "icy_wind", "", new Loc(5, 2), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customShield, new Loc(4, 4), mobSpawns, false);
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

                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //196 Espeon : 156 Magic Bounce : 094 Psychic : 247 Shadow Ball : 115 Reflect : 605 Dazzling Gleam
                mobSpawns.Add(GetBossMob("espeon", "magic_bounce", "psychic", "shadow_ball", "reflect", "dazzling_gleam", "", new Loc(3, 2), baseLv, scaleNum, scaleDen));
                //197 Umbreon : 028 Synchronize : 236 Moonlight : 212 Mean Look : 555 Snarl : 399 Dark Pulse
                mobSpawns.Add(GetBossMob("umbreon", "synchronize", "moonlight", "mean_look", "snarl", "dark_pulse", "", new Loc(5, 2), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customEclipse, new Loc(4, 4), mobSpawns, false);
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

                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //310 Manectric : 058 Minus : 604 Electric Terrain : 435 Discharge : 053 Flamethrower : 598 Eerie Impulse
                mobSpawns.Add(GetBossMob("manectric", "minus", "electric_terrain", "discharge", "flamethrower", "eerie_impulse", "", new Loc(3, 5), baseLv, scaleNum, scaleDen));
                //181 Ampharos : 057 Plus : 192 Zap Cannon : 406 Dragon Pulse : 324 Signal Beam : 602 Magnetic Flux
                mobSpawns.Add(GetBossMob("ampharos", "plus", "zap_cannon", "dragon_pulse", "signal_beam", "magnetic_flux", "", new Loc(5, 5), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customBatteryReverse, new Loc(4, 3), mobSpawns, true);
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

                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //311 Plusle : 057 Plus : 087 Thunder : 129 Swift : 417 Nasty Plot : 447 Grass Knot
                mobSpawns.Add(GetBossMob("plusle", "plus", "thunder", "swift", "nasty_plot", "grass_knot", "", new Loc(3, 5), baseLv, scaleNum, scaleDen));
                //312 Minun : 058 Minus : 240 Rain Dance : 097 Agility : 435 Discharge : 376 Trump Card
                mobSpawns.Add(GetBossMob("minun", "minus", "rain_dance", "agility", "discharge", "trump_card", "", new Loc(5, 5), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customBattery, new Loc(4, 7), mobSpawns, false);
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

                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //128 Tauros : 022 Intimidate : 099 Rage : 037 Thrash : 523 Bulldoze : 371 Payback
                mobSpawns.Add(GetBossMob("tauros", "intimidate", "rage", "thrash", "bulldoze", "payback", "", new Loc(4, 3), baseLv, scaleNum, scaleDen));
                //241 Miltank : 113 Scrappy : 208 Milk Drink : 215 Heal Bell : 034 Body Slam : 045 Growl
                mobSpawns.Add(GetBossMob("miltank", "scrappy", "milk_drink", "heal_bell", "body_slam", "growl", "", new Loc(4, 2), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customRailway, new Loc(4, 5), mobSpawns, true);
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

                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //414 Mothim : 110 Tinted Lens : 318 Silver Wind : 483 Quiver Dance : 403 Air Slash : 094 Psychic
                mobSpawns.Add(GetBossMob("mothim", "tinted_lens", "silver_wind", "quiver_dance", "air_slash", "psychic", "", new Loc(3, 2), baseLv, scaleNum, scaleDen));
                //413 Wormadam : 107 Anticipation : 522 Struggle Bug : 319 Metal Sound : 450 Bug Bite : 527 Electroweb
                mobSpawns.Add(GetBossMob(new MonsterID("wormadam", 2, "", Gender.Unknown), "anticipation", "struggle_bug", "metal_sound", "bug_bite", "electroweb", "", new Loc(5, 2), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customButterfly, new Loc(4, 4), mobSpawns, false);
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

                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //186 Politoed : 002 Drizzle : 054 Mist : 095 Hypnosis : 352 Water Pulse : 058 Ice Beam
                mobSpawns.Add(GetBossMob("politoed", "drizzle", "mist", "hypnosis", "water_pulse", "ice_beam", "", new Loc(2, 2), baseLv, scaleNum, scaleDen));
                //062 Poliwrath : 033 Swift Swim : 187 Belly Drum : 127 Waterfall : 358 Wake-Up Slap : 509 Circle Throw
                mobSpawns.Add(GetBossMob("poliwrath", "swift_swim", "belly_drum", "waterfall", "wake_up_slap", "circle_throw", "", new Loc(6, 2), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customWaterSwirl, new Loc(4, 4), mobSpawns, true);
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

                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //080 Slowbro : 012 Oblivious : 505 Heal Pulse : 244 Psych Up : 352 Water Pulse : 094 Psychic
                mobSpawns.Add(GetBossMob("slowbro", "oblivious", "heal_pulse", "psych_up", "water_pulse", "psychic", "", new Loc(3, 3), baseLv, scaleNum, scaleDen));
                //199 Slowking : 012 Oblivious : 376 Trump Card : 347 Calm Mind : 408 Power Gem : 281 Yawn
                mobSpawns.Add(GetBossMob("slowking", "oblivious", "trump_card", "calm_mind", "power_gem", "yawn", "", new Loc(5, 3), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customCrownWater, new Loc(4, 5), mobSpawns, false);
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

                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //034 Nidoking : 125 Sheer Force : 398 Poison Jab : 224 Megahorn : 116 Focus Energy : 529 Drill Run
                mobSpawns.Add(GetBossMob("nidoking", "sheer_force", "poison_jab", "megahorn", "focus_energy", "drill_run", "", new Loc(3, 3), baseLv, scaleNum, scaleDen));
                //031 Nidoqueen : 079 Rivalry : 270 Helping Hand : 445 Captivate : 414 Earth Power : 482 Sludge Wave
                mobSpawns.Add(GetBossMob("nidoqueen", "rivalry", "helping_hand", "captivate", "earth_power", "sludge_wave", "", new Loc(5, 3), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customCrown, new Loc(4, 5), mobSpawns, true);
            }


            if (id == "gallade")
            {
                string[] customCrown = new string[] {       ".........",
                                                            ".#.....#.",
                                                            ".........",
                                                            ".#.....#.",
                                                            ".........",
                                                            ".#.....#.",
                                                            "........."};

                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //034 Nidoking : 125 Sheer Force : 398 Poison Jab : 224 Megahorn : 116 Focus Energy : 529 Drill Run
                mobSpawns.Add(GetBossMob("gardevoir", "trace", "dazzling_gleam", "wish", "magical_leaf", "heal_pulse", "", new Loc(3, 1), baseLv, scaleNum, scaleDen));
                //031 Nidoqueen : 079 Rivalry : 270 Helping Hand : 445 Captivate : 414 Earth Power : 482 Sludge Wave
                mobSpawns.Add(GetBossMob("gallade", "steadfast", "psycho_cut", "leaf_blade", "night_slash", "wide_guard", "", new Loc(5, 1), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customCrown, new Loc(4, 4), mobSpawns, false);
            }


            if (id == "volbeat")
            {
                string[] customCrown = new string[] {       "####.####",
                                                            "#.~#.#~.#",
                                                            "#~.~.~.~#",
                                                            "##~...~##",
                                                            ".........",
                                                            "##~...~##",
                                                            "#~.~.~.~#",
                                                            "#.~#.#~.#",
                                                            "####.####"};

                List<MobSpawn> mobSpawns = new List<MobSpawn>();
                //034 Nidoking : 125 Sheer Force : 398 Poison Jab : 224 Megahorn : 116 Focus Energy : 529 Drill Run
                mobSpawns.Add(GetBossMob("volbeat", "swarm", "tail_glow", "signal_beam", "thunder", "confuse_ray", "", new Loc(3, 3), baseLv, scaleNum, scaleDen));
                //031 Nidoqueen : 079 Rivalry : 270 Helping Hand : 445 Captivate : 414 Earth Power : 482 Sludge Wave
                mobSpawns.Add(GetBossMob("illumise", "tinted_lens", "moonlight", "encore", "struggle_bug", "bug_buzz", "", new Loc(5, 3), baseLv, scaleNum, scaleDen));
                return CreateRoomGenSpecificBoss<T>(customCrown, new Loc(4, 4), mobSpawns, false);
            }


            if (id == "palafin")
            {
                //3 finizen in front of the player
                //palafin is hiding behind a wall in the back
                //first turn uses jet punch and breaks the walls
            }


            throw new Exception("Invalid boss id");
        }

        static AddBossRoomStep<T> getBossRoomStep<T>(string id, int bossIndex = 0) where T : ListMapGenContext
        {
            SpawnList<RoomGen<T>> bossRooms = new SpawnList<RoomGen<T>>();
            bossRooms.Add(getBossRoomGen<T>(id), 10);
            return CreateGenericBossRoomStep(bossRooms, bossIndex);
        }



        public enum DungeonAccessibility
        {
            MainPath = 0,
            SidePath = 1,
            Unlockable = 2,
            Hidden = 3
        }


        static void PopulateSecretRoomItems(SpawnList<IStepSpawner<MapLoadContext, MapItem>> spawnStep, DungeonStage gamePhase, DungeonAccessibility access)
        {

        }

        static void PopulateVaultItems(SpreadVaultZoneStep vaultChanceZoneStep, DungeonStage gamePhase, DungeonAccessibility access, int max_floors, bool locked, bool secretRoom = false)
        {
            if (!locked)
            {
                // item spawnings for the vault
                {
                    SpawnList<IStepSpawner<ListMapGenContext, MapItem>> boxSpawn = new SpawnList<IStepSpawner<ListMapGenContext, MapItem>>();

                    //444      ***    Light Box - 1* items
                    {
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_light", new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(1), new RandRange(1))), 30);
                    }

                    //445      ***    Heavy Box - 2* items
                    {
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_heavy", new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(2), new RandRange(1))), 10);
                    }

                    MultiStepSpawner<ListMapGenContext, MapItem> boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(1)));

                    //StepSpawner <- PresetMultiRand
                    MultiStepSpawner<ListMapGenContext, MapItem> mainSpawner = new MultiStepSpawner<ListMapGenContext, MapItem>();
                    mainSpawner.Picker = new PresetMultiRand<IStepSpawner<ListMapGenContext, MapItem>>(boxPicker);
                    vaultChanceZoneStep.ItemSpawners.SetRange(mainSpawner, new IntRange(0, max_floors));
                }
                vaultChanceZoneStep.ItemAmount.SetRange(new RandRange(0), new IntRange(0, max_floors));

                // item placements for the vault
                {
                    RandomRoomSpawnStep<ListMapGenContext, MapItem> detourItems = new RandomRoomSpawnStep<ListMapGenContext, MapItem>();
                    detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                    vaultChanceZoneStep.ItemPlacements.SetRange(detourItems, new IntRange(0, max_floors));
                }
            }
            else
            {
                if (secretRoom)
                {
                    // item spawnings for the vault
                    {
                        //add a PickerSpawner <- PresetMultiRand <- coins
                        List<MapItem> treasures = new List<MapItem>();
                        treasures.Add(new MapItem("loot_pearl", 2));
                        PickerSpawner<ListMapGenContext, MapItem> treasurePicker = new PickerSpawner<ListMapGenContext, MapItem>(new PresetMultiRand<MapItem>(treasures));

                        SpawnList<IStepSpawner<ListMapGenContext, MapItem>> boxSpawn = new SpawnList<IStepSpawner<ListMapGenContext, MapItem>>();

                        //444      ***    Light Box - 1* items
                        {
                            boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_light", new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(1), new RandRange(1))), 30);
                        }

                        //445      ***    Heavy Box - 2* items
                        {
                            boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_heavy", new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(2), new RandRange(1))), 10);
                        }

                        MultiStepSpawner<ListMapGenContext, MapItem> boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(1)));

                        //StepSpawner <- PresetMultiRand
                        MultiStepSpawner<ListMapGenContext, MapItem> mainSpawner = new MultiStepSpawner<ListMapGenContext, MapItem>();
                        mainSpawner.Picker = new PresetMultiRand<IStepSpawner<ListMapGenContext, MapItem>>(treasurePicker, boxPicker);
                        vaultChanceZoneStep.ItemSpawners.SetRange(mainSpawner, new IntRange(0, max_floors));
                    }
                    vaultChanceZoneStep.ItemAmount.SetRange(new RandRange(0), new IntRange(0, max_floors));

                    // item placements for the vault
                    {
                        RandomRoomSpawnStep<ListMapGenContext, MapItem> detourItems = new RandomRoomSpawnStep<ListMapGenContext, MapItem>();
                        detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                        vaultChanceZoneStep.ItemPlacements.SetRange(detourItems, new IntRange(0, max_floors));
                    }
                }
                else
                {
                    if (gamePhase == DungeonStage.Advanced)
                    {
                        foreach (string key in IterateTMs(TMClass.Natural))
                            vaultChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 5);//TMs
                        vaultChanceZoneStep.Items.Add(new MapItem("loot_nugget"), new IntRange(0, max_floors), 200);//nugget
                        vaultChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear", 1), new IntRange(0, max_floors), 100);//amber tear
                        vaultChanceZoneStep.Items.Add(new MapItem("seed_reviver"), new IntRange(0, max_floors), 200);//reviver seed
                        vaultChanceZoneStep.Items.Add(new MapItem("seed_joy"), new IntRange(0, max_floors), 100);//joy seed
                        vaultChanceZoneStep.Items.Add(new MapItem("machine_ability_capsule"), new IntRange(0, max_floors), 200);//ability capsule
                    }
                    else
                    {
                        foreach (string key in IterateTMs(TMClass.Starter))
                            vaultChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 5);//TMs
                        vaultChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear", 1), new IntRange(0, max_floors), 100);//amber tear
                        vaultChanceZoneStep.Items.Add(new MapItem("seed_reviver"), new IntRange(0, max_floors), 200);//reviver seed
                        vaultChanceZoneStep.Items.Add(new MapItem("seed_pure"), new IntRange(0, max_floors), 100);//pure seed
                        vaultChanceZoneStep.Items.Add(new MapItem("machine_recall_box"), new IntRange(0, max_floors), 200);//recall box
                    }

                    // item spawnings for the vault
                    {
                        //add a PickerSpawner <- PresetMultiRand <- coins
                        List<MapItem> treasures = new List<MapItem>();
                        treasures.Add(MapItem.CreateMoney(200));
                        treasures.Add(MapItem.CreateMoney(200));
                        treasures.Add(MapItem.CreateMoney(200));
                        treasures.Add(MapItem.CreateMoney(200));
                        treasures.Add(MapItem.CreateMoney(200));
                        PickerSpawner<ListMapGenContext, MapItem> treasurePicker = new PickerSpawner<ListMapGenContext, MapItem>(new PresetMultiRand<MapItem>(treasures));

                        SpawnList<IStepSpawner<ListMapGenContext, MapItem>> boxSpawn = new SpawnList<IStepSpawner<ListMapGenContext, MapItem>>();

                        //444      ***    Light Box - 1* items
                        {
                            boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_light", new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(1), new RandRange(1))), 30);
                        }

                        //445      ***    Heavy Box - 2* items
                        {
                            boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_heavy", new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(2), new RandRange(1))), 10);
                        }

                        if (gamePhase == DungeonStage.Advanced)
                        {
                            //446      ***    Nifty Box - all high tier TMs, ability capsule, heart scale 9, max potion, full heal, max elixir
                            {
                                SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                                boxTreasure.Add(new MapItem("machine_ability_capsule"), 100);//ability capsule
                                boxTreasure.Add(new MapItem("loot_heart_scale"), 100);//heart scale
                                boxTreasure.Add(new MapItem("medicine_max_potion"), 30);//max potion
                                boxTreasure.Add(new MapItem("medicine_full_heal"), 100);//full heal
                                boxTreasure.Add(new MapItem("medicine_max_elixir"), 30);//max elixir
                                boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_nifty", new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 10);
                            }

                            //447      ***    Dainty Box - Stat ups, wonder gummi, nectar, golden apple, golden banana
                            {
                                SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                                boxTreasure.Add(new MapItem("boost_protein"), 2);//protein
                                boxTreasure.Add(new MapItem("boost_iron"), 2);//iron
                                boxTreasure.Add(new MapItem("boost_calcium"), 2);//calcium
                                boxTreasure.Add(new MapItem("boost_zinc"), 2);//zinc
                                boxTreasure.Add(new MapItem("boost_carbos"), 2);//carbos
                                boxTreasure.Add(new MapItem("boost_hp_up"), 2);//hp up
                                boxTreasure.Add(new MapItem("boost_nectar"), 2);//nectar

                                boxTreasure.Add(new MapItem("food_apple_perfect"), 10);//perfect apple
                                boxTreasure.Add(new MapItem("food_banana_big"), 10);//big banana
                                boxTreasure.Add(new MapItem("seed_joy"), 10);//joy seed
                                boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_dainty", new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 3);
                            }

                            //448    Glittery Box - golden apple, amber tear, golden banana, nugget, golden thorn 9
                            {
                                SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();
                                boxTreasure.Add(new MapItem("ammo_golden_thorn"), 10);//golden thorn
                                boxTreasure.Add(new MapItem("medicine_amber_tear"), 10);//Amber Tear
                                boxTreasure.Add(new MapItem("loot_nugget"), 10);//nugget
                                boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_glittery", new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 2);
                            }
                        }

                        MultiStepSpawner<ListMapGenContext, MapItem> boxPicker;
                        if (gamePhase == DungeonStage.Advanced)
                            boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(1, 3)));
                        else
                            boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(1)));

                        //StepSpawner <- PresetMultiRand
                        MultiStepSpawner<ListMapGenContext, MapItem> mainSpawner = new MultiStepSpawner<ListMapGenContext, MapItem>();
                        mainSpawner.Picker = new PresetMultiRand<IStepSpawner<ListMapGenContext, MapItem>>(treasurePicker, boxPicker);
                        vaultChanceZoneStep.ItemSpawners.SetRange(mainSpawner, new IntRange(0, max_floors));
                    }
                    vaultChanceZoneStep.ItemAmount.SetRange(new RandRange(1), new IntRange(0, max_floors));


                    // item placements for the vault
                    {
                        RandomRoomSpawnStep<ListMapGenContext, MapItem> detourItems = new RandomRoomSpawnStep<ListMapGenContext, MapItem>();
                        detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                        vaultChanceZoneStep.ItemPlacements.SetRange(detourItems, new IntRange(0, max_floors));
                    }
                }
            }
        }

        static void PopulateFOEItems(SpreadVaultZoneStep vaultChanceZoneStep, DungeonStage gamePhase, DungeonAccessibility access, int max_floors)
        {
            {
                //items for the vault
                {
                    if (gamePhase == DungeonStage.Rogue)
                    {
                        vaultChanceZoneStep.Items.Add(new MapItem("medicine_elixir"), new IntRange(0, max_floors), 800);//elixir
                        vaultChanceZoneStep.Items.Add(new MapItem("medicine_max_elixir"), new IntRange(0, max_floors), 100);//max elixir
                        vaultChanceZoneStep.Items.Add(new MapItem("medicine_potion"), new IntRange(0, max_floors), 200);//potion
                        vaultChanceZoneStep.Items.Add(new MapItem("medicine_max_potion"), new IntRange(0, max_floors), 100);//max potion
                        vaultChanceZoneStep.Items.Add(new MapItem("medicine_full_heal"), new IntRange(0, max_floors), 200);//full heal
                        foreach (string key in IterateXItems())
                            vaultChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 50);//X-Items
                    }
                    foreach (string key in IterateGummis())
                        vaultChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 200);//gummis
                    vaultChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear", 1), new IntRange(0, max_floors), 2000);//amber tear
                    vaultChanceZoneStep.Items.Add(new MapItem("seed_reviver"), new IntRange(0, max_floors), 200);//reviver seed
                    vaultChanceZoneStep.Items.Add(new MapItem("seed_joy"), new IntRange(0, max_floors), 200);//joy seed
                    if (gamePhase == DungeonStage.Rogue)
                    {
                        vaultChanceZoneStep.Items.Add(new MapItem("machine_ability_capsule"), new IntRange(0, max_floors), 200);//ability capsule
                        vaultChanceZoneStep.Items.Add(new MapItem("machine_assembly_box"), new IntRange(0, max_floors), 500);//assembly box
                        vaultChanceZoneStep.Items.Add(new MapItem("machine_recall_box"), new IntRange(0, max_floors), 500);//link box
                        vaultChanceZoneStep.Items.Add(new MapItem("evo_harmony_scarf"), new IntRange(0, max_floors), 100);//harmony scarf
                        vaultChanceZoneStep.Items.Add(new MapItem("orb_itemizer"), new IntRange(max_floors / 2, max_floors), 50);//itemizer orb
                        vaultChanceZoneStep.Items.Add(new MapItem("wand_transfer", 3), new IntRange(max_floors / 2, max_floors), 50);//transfer wand
                    }
                    else
                    {
                        vaultChanceZoneStep.Items.Add(new MapItem("orb_itemizer"), new IntRange(0, max_floors), 50);//itemizer orb
                        vaultChanceZoneStep.Items.Add(new MapItem("wand_transfer", 3), new IntRange(0, max_floors), 50);//transfer wand
                    }
                    vaultChanceZoneStep.Items.Add(new MapItem("key", 1), new IntRange(0, max_floors), 1000);//key
                }

                // item spawnings for the vault
                {
                    //add a PickerSpawner <- PresetMultiRand <- coins
                    List<MapItem> treasures = new List<MapItem>();
                    treasures.Add(MapItem.CreateMoney(200));
                    treasures.Add(MapItem.CreateMoney(200));
                    treasures.Add(MapItem.CreateMoney(200));
                    treasures.Add(MapItem.CreateMoney(200));
                    treasures.Add(MapItem.CreateMoney(200));
                    treasures.Add(MapItem.CreateMoney(200));
                    PickerSpawner<ListMapGenContext, MapItem> treasurePicker = new PickerSpawner<ListMapGenContext, MapItem>(new PresetMultiRand<MapItem>(treasures));


                    SpawnList<IStepSpawner<ListMapGenContext, MapItem>> boxSpawn = new SpawnList<IStepSpawner<ListMapGenContext, MapItem>>();

                    //444      ***    Light Box - 1* items
                    {
                        if (gamePhase == DungeonStage.Rogue)
                        {
                            SpawnList<MapItem> silks = new SpawnList<MapItem>();
                            foreach (string key in IterateSilks())
                                silks.Add(new MapItem(key), 10);

                            boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_light", new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(silks, new RandRange(1)))), 10);
                        }
                        else
                            boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_light", new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(1), new RandRange(1))), 30);
                    }

                    //445      ***    Heavy Box - 2* items
                    {
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_heavy", new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(2), new RandRange(1))), 10);
                    }

                    if (gamePhase == DungeonStage.Rogue)
                    {
                        ////446      ***    Nifty Box - all high tier TMs, ability capsule, heart scale 9, max potion, full heal, max elixir
                        //    SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                        //    for (int nn = 588; nn < 700; nn++)
                        //        boxTreasure.Add(new MapItem(nn), 1);//TMs
                        //    boxTreasure.Add(new MapItem("machine_ability_capsule"), 100);//ability capsule
                        //    boxTreasure.Add(new MapItem("loot_heart_scale"), 100);//heart scale
                        //    boxTreasure.Add(new MapItem("medicine_potion"), 60);//potion
                        //    boxTreasure.Add(new MapItem("medicine_max_potion"), 30);//max potion
                        //    boxTreasure.Add(new MapItem("medicine_full_heal"), 100);//full heal
                        //    boxTreasure.Add(new MapItem("medicine_elixir"), 60);//elixir
                        //    boxTreasure.Add(new MapItem("medicine_max_elixir"), 30);//max elixir
                        //    boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_nifty", new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 10);

                    }
                    else
                    {
                        //446      ***    Nifty Box - all high tier TMs, ability capsule, heart scale 9, max potion, full heal, max elixir
                        SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();
                        //TMs
                        foreach (string key in IterateTMs(TMClass.Top))
                            boxTreasure.Add(new MapItem(key), 10);

                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_nifty", new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 10);
                    }

                    //447      ***    Dainty Box - Stat ups, wonder gummi, nectar, golden apple, golden banana
                    {
                        SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                        foreach (string key in IterateGummis())
                            boxTreasure.Add(new MapItem(key), 1);

                        boxTreasure.Add(new MapItem("boost_protein"), 2);//protein
                        boxTreasure.Add(new MapItem("boost_iron"), 2);//iron
                        boxTreasure.Add(new MapItem("boost_calcium"), 2);//calcium
                        boxTreasure.Add(new MapItem("boost_zinc"), 2);//zinc
                        boxTreasure.Add(new MapItem("boost_carbos"), 2);//carbos
                        boxTreasure.Add(new MapItem("boost_hp_up"), 2);//hp up
                        boxTreasure.Add(new MapItem("boost_nectar"), 2);//nectar
                        if (gamePhase == DungeonStage.Rogue)
                            boxTreasure.Add(new MapItem("food_apple_perfect"), 10);//perfect apple
                        else
                            boxTreasure.Add(new MapItem("food_apple_huge"), 10);//huge apple
                        boxTreasure.Add(new MapItem("food_banana_big"), 10);//big banana
                        if (gamePhase == DungeonStage.Rogue)
                            boxTreasure.Add(new MapItem("gummi_wonder"), 4);//wonder gummi
                        boxTreasure.Add(new MapItem("seed_joy"), 10);//joy seed
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_dainty", new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 3);
                    }

                    //448    Glittery Box - golden apple, amber tear, golden banana, nugget, golden thorn 9
                    {
                        SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();
                        boxTreasure.Add(new MapItem("ammo_golden_thorn"), 10);//golden thorn
                        boxTreasure.Add(new MapItem("medicine_amber_tear"), 10);//Amber Tear
                        boxTreasure.Add(new MapItem("loot_nugget"), 10);//nugget
                        if (gamePhase == DungeonStage.Rogue)
                            boxTreasure.Add(new MapItem("seed_golden"), 10);//golden seed
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_glittery", new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 2);
                    }

                    //449      ***    Deluxe Box - Legendary exclusive items, harmony scarf, golden items, stat ups, wonder gummi, perfect apricorn, max potion/full heal/max elixir
                    //{
                    //    SpeciesItemListSpawner<ListMapGenContext> legendSpawner = new SpeciesItemListSpawner<ListMapGenContext>(new IntRange(1, 3), new RandRange(1));
                    //    legendSpawner.Species.Add(144);
                    //    legendSpawner.Species.Add(145);
                    //    legendSpawner.Species.Add(146);
                    //    legendSpawner.Species.Add(150);
                    //    boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_deluxe", legendSpawner), 5);
                    //}
                    //{
                    //    SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                    //    for (int nn = 0; nn < 18; nn++)//Gummi
                    //        boxTreasure.Add(new MapItem(76 + nn), 1);

                    //    boxTreasure.Add(new MapItem("food_apple_golden"), 10);//golden apple
                    //    boxTreasure.Add(new MapItem("food_banana_golden"), 10);//gold banana
                    //    boxTreasure.Add(new MapItem("medicine_amber_tear"), 10);//Amber Tear
                    //    boxTreasure.Add(new MapItem("loot_nugget"), 10);//nugget
                    //    boxTreasure.Add(new MapItem("seed_golden"), 10);//golden seed
                    //    boxTreasure.Add(new MapItem("medicine_max_potion"), 30);//max potion
                    //    boxTreasure.Add(new MapItem("medicine_full_heal"), 100);//full heal
                    //    boxTreasure.Add(new MapItem("medicine_max_elixir"), 30);//max elixir
                    //    boxTreasure.Add(new MapItem("gummi_wonder"), 4);//wonder gummi
                    //    boxTreasure.Add(new MapItem("evo_harmony_scarf"), 1);//harmony scarf
                    //    boxTreasure.Add(new MapItem("apricorn_perfect"), 1);//perfect apricorn
                    //    boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_deluxe", new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 10);
                    //}

                    MultiStepSpawner<ListMapGenContext, MapItem> boxPicker;
                    if (gamePhase == DungeonStage.Rogue)
                        boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(1)));
                    else if (gamePhase == DungeonStage.Advanced)
                        boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(3, 5)));
                    else
                        boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(2, 4)));

                    //MultiStepSpawner <- PresetMultiRand
                    MultiStepSpawner<ListMapGenContext, MapItem> mainSpawner = new MultiStepSpawner<ListMapGenContext, MapItem>();
                    mainSpawner.Picker = new PresetMultiRand<IStepSpawner<ListMapGenContext, MapItem>>(treasurePicker, boxPicker);
                    vaultChanceZoneStep.ItemSpawners.SetRange(mainSpawner, new IntRange(0, max_floors));
                }

                if (gamePhase == DungeonStage.Rogue)
                    vaultChanceZoneStep.ItemAmount.SetRange(new RandRange(5, 7), new IntRange(0, max_floors));
                else
                    vaultChanceZoneStep.ItemAmount.SetRange(new RandRange(3, 5), new IntRange(0, max_floors));

                // item placements for the vault
                {
                    RandomRoomSpawnStep<ListMapGenContext, MapItem> detourItems = new RandomRoomSpawnStep<ListMapGenContext, MapItem>();
                    detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                    vaultChanceZoneStep.ItemPlacements.SetRange(detourItems, new IntRange(0, max_floors));
                }
            }
        }

        static void PopulateBossItems(SpreadBossZoneStep bossChanceZoneStep, DungeonStage gamePhase, DungeonAccessibility access, int max_floors)
        {
            //items for the vault
            {
                bossChanceZoneStep.Items.Add(new MapItem("medicine_elixir"), new IntRange(0, max_floors), 800);//elixir
                bossChanceZoneStep.Items.Add(new MapItem("medicine_max_elixir"), new IntRange(0, max_floors), 100);//max elixir
                bossChanceZoneStep.Items.Add(new MapItem("medicine_potion"), new IntRange(0, max_floors), 200);//potion
                bossChanceZoneStep.Items.Add(new MapItem("medicine_max_potion"), new IntRange(0, max_floors), 100);//max potion
                bossChanceZoneStep.Items.Add(new MapItem("medicine_full_heal"), new IntRange(0, max_floors), 200);//full heal
                foreach (string key in IterateXItems())
                    bossChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 50);//X-Items
                foreach (string key in IterateGummis())
                    bossChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 200);//gummis
                bossChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear", 1), new IntRange(0, max_floors), 200);//amber tear
                bossChanceZoneStep.Items.Add(new MapItem("seed_reviver"), new IntRange(0, max_floors), 200);//reviver seed
                bossChanceZoneStep.Items.Add(new MapItem("seed_joy"), new IntRange(0, max_floors), 200);//joy seed
                bossChanceZoneStep.Items.Add(new MapItem("machine_ability_capsule"), new IntRange(0, max_floors), 200);//ability capsule
                bossChanceZoneStep.Items.Add(new MapItem("evo_harmony_scarf"), new IntRange(0, max_floors), 100);//harmony scarf
                bossChanceZoneStep.Items.Add(new MapItem("key", 1), new IntRange(0, 30), 1000);//key

                //tms
                foreach (string tm_id in IterateTMs(TMClass.Top | TMClass.Mid))
                    bossChanceZoneStep.Items.Add(new MapItem(tm_id), new IntRange(0, 30), 5);
            }

            // item spawnings for the vault
            {
                //add a PickerSpawner <- PresetMultiRand <- coins
                List<MapItem> treasures = new List<MapItem>();
                treasures.Add(MapItem.CreateMoney(200));
                treasures.Add(MapItem.CreateMoney(200));
                treasures.Add(MapItem.CreateMoney(200));
                treasures.Add(MapItem.CreateMoney(200));
                treasures.Add(MapItem.CreateMoney(200));
                treasures.Add(MapItem.CreateMoney(200));
                treasures.Add(new MapItem("loot_nugget"));
                PickerSpawner<ListMapGenContext, MapItem> treasurePicker = new PickerSpawner<ListMapGenContext, MapItem>(new PresetMultiRand<MapItem>(treasures));

                SpawnList<IStepSpawner<ListMapGenContext, MapItem>> boxSpawn = new SpawnList<IStepSpawner<ListMapGenContext, MapItem>>();

                //444      ***    Light Box - 1* items
                {
                    SpawnList<MapItem> silks = new SpawnList<MapItem>();
                    foreach (string key in IterateSilks())
                        silks.Add(new MapItem(key), 10);

                    boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_light", new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(silks, new RandRange(1)))), 10);
                }

                //445      ***    Heavy Box - 2* items
                {
                    boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_heavy", new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(2), new RandRange(1))), 10);
                }

                ////446      ***    Nifty Box - all high tier TMs, ability capsule, heart scale 9, max potion, full heal, max elixir
                //{
                //    SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                //    for (int nn = 588; nn < 700; nn++)
                //        boxTreasure.Add(new MapItem(nn), 1);//TMs
                //    boxTreasure.Add(new MapItem("machine_ability_capsule"), 100);//ability capsule
                //    boxTreasure.Add(new MapItem("loot_heart_scale"), 100);//heart scale
                //    boxTreasure.Add(new MapItem("medicine_potion"), 60);//potion
                //    boxTreasure.Add(new MapItem("medicine_max_potion"), 30);//max potion
                //    boxTreasure.Add(new MapItem("medicine_full_heal"), 100);//full heal
                //    boxTreasure.Add(new MapItem("medicine_elixir"), 60);//elixir
                //    boxTreasure.Add(new MapItem("medicine_max_elixir"), 30);//max elixir
                //    boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_nifty", new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 10);
                //}

                //447      ***    Dainty Box - Stat ups, wonder gummi, nectar, golden apple, golden banana
                {
                    SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                    foreach (string key in IterateGummis())
                        boxTreasure.Add(new MapItem(key), 1);

                    boxTreasure.Add(new MapItem("boost_protein"), 2);//protein
                    boxTreasure.Add(new MapItem("boost_iron"), 2);//iron
                    boxTreasure.Add(new MapItem("boost_calcium"), 2);//calcium
                    boxTreasure.Add(new MapItem("boost_zinc"), 2);//zinc
                    boxTreasure.Add(new MapItem("boost_carbos"), 2);//carbos
                    boxTreasure.Add(new MapItem("boost_hp_up"), 2);//hp up
                    boxTreasure.Add(new MapItem("boost_nectar"), 2);//nectar

                    boxTreasure.Add(new MapItem("food_apple_perfect"), 10);//perfect apple
                    boxTreasure.Add(new MapItem("food_banana_big"), 10);//big banana
                    boxTreasure.Add(new MapItem("gummi_wonder"), 4);//wonder gummi
                    boxTreasure.Add(new MapItem("seed_joy"), 10);//joy seed
                    boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_dainty", new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 3);
                }

                //448    Glittery Box - golden apple, amber tear, golden banana, nugget, golden thorn 9
                {
                    SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();
                    boxTreasure.Add(new MapItem("ammo_golden_thorn"), 10);//golden thorn
                    boxTreasure.Add(new MapItem("medicine_amber_tear"), 10);//Amber Tear
                    boxTreasure.Add(new MapItem("loot_nugget"), 10);//nugget
                    boxTreasure.Add(new MapItem("seed_golden"), 10);//golden seed
                    boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_glittery", new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 2);
                }

                //449      ***    Deluxe Box - Legendary exclusive items, harmony scarf, golden items, stat ups, wonder gummi, perfect apricorn, max potion/full heal/max elixir
                //{
                //    SpeciesItemListSpawner<ListMapGenContext> legendSpawner = new SpeciesItemListSpawner<ListMapGenContext>(new IntRange(1, 3), new RandRange(1));
                //    legendSpawner.Species.Add(144);
                //    legendSpawner.Species.Add(145);
                //    legendSpawner.Species.Add(146);
                //    legendSpawner.Species.Add(150);
                //    boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_deluxe", legendSpawner), 5);
                //}
                //{
                //    SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                //    for (int nn = 0; nn < 18; nn++)//Gummi
                //        boxTreasure.Add(new MapItem(76 + nn), 1);

                //    boxTreasure.Add(new MapItem("food_apple_golden"), 10);//golden apple
                //    boxTreasure.Add(new MapItem("food_banana_golden"), 10);//gold banana
                //    boxTreasure.Add(new MapItem("medicine_amber_tear"), 10);//Amber Tear
                //    boxTreasure.Add(new MapItem("loot_nugget"), 10);//nugget
                //    boxTreasure.Add(new MapItem("seed_golden"), 10);//golden seed
                //    boxTreasure.Add(new MapItem("medicine_max_potion"), 30);//max potion
                //    boxTreasure.Add(new MapItem("medicine_full_heal"), 100);//full heal
                //    boxTreasure.Add(new MapItem("medicine_max_elixir"), 30);//max elixir
                //    boxTreasure.Add(new MapItem("gummi_wonder"), 4);//wonder gummi
                //    boxTreasure.Add(new MapItem("evo_harmony_scarf"), 1);//harmony scarf
                //    boxTreasure.Add(new MapItem("apricorn_perfect"), 1);//perfect apricorn
                //    boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_deluxe", new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 10);
                //}

                MultiStepSpawner<ListMapGenContext, MapItem> boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(1)));

                //MultiStepSpawner <- PresetMultiRand
                MultiStepSpawner<ListMapGenContext, MapItem> mainSpawner = new MultiStepSpawner<ListMapGenContext, MapItem>();
                mainSpawner.Picker = new PresetMultiRand<IStepSpawner<ListMapGenContext, MapItem>>(treasurePicker, boxPicker);
                bossChanceZoneStep.ItemSpawners.SetRange(mainSpawner, new IntRange(0, max_floors));
            }
            bossChanceZoneStep.ItemAmount.SetRange(new RandRange(2, 4), new IntRange(0, max_floors));

            // item placements for the vault
            {
                RandomRoomSpawnStep<ListMapGenContext, MapItem> detourItems = new RandomRoomSpawnStep<ListMapGenContext, MapItem>();
                detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                detourItems.Filters.Add(new RoomFilterIndex(false, 0));
                bossChanceZoneStep.ItemPlacements.SetRange(detourItems, new IntRange(0, max_floors));
            }
        }

        static void PopulateChestItems(SpreadHouseZoneStep chestChanceZoneStep, DungeonStage gamePhase, DungeonAccessibility access, bool ambush, int max_floors)
        {
            if (gamePhase == DungeonStage.Rogue)
            {
                foreach (string key in IterateGummis())
                    chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//gummis
                if (ambush)
                    chestChanceZoneStep.Items.Add(new MapItem("apricorn_big"), new IntRange(0, max_floors), 10);//big apricorn
                else
                {
                    foreach (string key in IterateApricorns())
                        chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//apricorns
                }
                chestChanceZoneStep.Items.Add(new MapItem("medicine_elixir"), new IntRange(0, max_floors), 80);//elixir
                chestChanceZoneStep.Items.Add(new MapItem("medicine_max_elixir"), new IntRange(0, max_floors), 20);//max elixir
                chestChanceZoneStep.Items.Add(new MapItem("medicine_potion"), new IntRange(0, max_floors), 20);//potion
                chestChanceZoneStep.Items.Add(new MapItem("medicine_max_potion"), new IntRange(0, max_floors), 20);//max potion
                chestChanceZoneStep.Items.Add(new MapItem("medicine_full_heal"), new IntRange(0, max_floors), 20);//full heal
                foreach (string key in IterateXItems())
                    chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 15);//X-Items

                chestChanceZoneStep.Items.Add(new MapItem("loot_nugget"), new IntRange(0, max_floors), 20);//nugget
                if (ambush)
                    chestChanceZoneStep.Items.Add(new MapItem("loot_pearl", 2), new IntRange(0, max_floors), 5);//pearl
                chestChanceZoneStep.Items.Add(new MapItem("loot_heart_scale", 3), new IntRange(0, max_floors), 10);//heart scale
                if (ambush)
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear", 1), new IntRange(0, max_floors), 200);//amber tear
                chestChanceZoneStep.Items.Add(new MapItem("ammo_rare_fossil", 3), new IntRange(0, max_floors), 20);//rare fossil
                chestChanceZoneStep.Items.Add(new MapItem("seed_reviver"), new IntRange(0, max_floors), 20);//reviver seed
                chestChanceZoneStep.Items.Add(new MapItem("seed_joy"), new IntRange(0, max_floors), 20);//joy seed
                if (ambush)
                    chestChanceZoneStep.Items.Add(new MapItem("seed_golden"), new IntRange(0, max_floors), 20);//golden seed
                chestChanceZoneStep.Items.Add(new MapItem("machine_recall_box"), new IntRange(0, max_floors), 10);//link box
                chestChanceZoneStep.Items.Add(new MapItem("machine_assembly_box"), new IntRange(10, max_floors), 10);//assembly box
                chestChanceZoneStep.Items.Add(new MapItem("machine_ability_capsule"), new IntRange(0, max_floors), 10);//ability capsule
                if (ambush)
                    chestChanceZoneStep.Items.Add(new MapItem("evo_harmony_scarf"), new IntRange(0, max_floors), 10);//harmony scarf

                if (ambush)
                {
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(2, 5), "loot_pearl"), new ItemThemeNone(50, new RandRange(4, 7))), new IntRange(0, max_floors), 30);
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(2, 5), "loot_pearl"), new ItemThemeRange(true, true, new RandRange(3, 5), "seed_reviver")), new IntRange(0, max_floors), 10);//reviver seed
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(2, 5), "loot_pearl"), new ItemThemeRange(true, true, new RandRange(2, 5), "seed_joy")), new IntRange(0, max_floors), 10);//joy seed
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(2, 5), "loot_pearl"), new ItemThemeRange(true, true, new RandRange(1), "seed_golden")), new IntRange(0, max_floors), 10);//golden seed
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(2, 5), "loot_pearl"), new ItemThemeRange(true, true, new RandRange(1), "evo_harmony_scarf")), new IntRange(20, max_floors), 20);//Harmony Scarf
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(2, 5), "loot_pearl"), new ItemThemeRange(true, true, new RandRange(3, 6), ItemArray(IterateManmades()))), new IntRange(0, max_floors), 20);//manmade items
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(2, 5), "loot_pearl"), new ItemStateType(new FlagType(typeof(EquipState)), true, true, new RandRange(3, 6))), new IntRange(0, max_floors), 20);//equip
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(2, 5), "loot_pearl"), new ItemThemeRange(true, true, new RandRange(3, 6), ItemArray(IterateTypePlates()))), new IntRange(0, max_floors), 10);//plates
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(2, 5), "loot_pearl"), new ItemThemeType(ItemData.UseType.Learn, true, true, new RandRange(3, 6))), new IntRange(0, max_floors), 20);//TMs
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(2, 5), "loot_pearl"), new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(4, 9))), new IntRange(0, max_floors), 10);//gummis
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(2, 5), "loot_pearl"), new ItemStateType(new FlagType(typeof(RecruitState)), true, true, new RandRange(3, 7))), new IntRange(0, max_floors), 10);//apricorns
                }
                else
                {
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeNone(50, new RandRange(2, 5)), new IntRange(0, max_floors), 30);
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeRange(true, true, new RandRange(2, 4), "seed_reviver"), new IntRange(0, max_floors), 10);//reviver seed
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeRange(true, true, new RandRange(1, 4), "seed_joy"), new IntRange(0, max_floors), 10);//joy seed
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeRange(true, true, new RandRange(1, 3), ItemArray(IterateManmades())), new IntRange(0, max_floors), 100);//manmade items
                    chestChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(EquipState)), true, true, new RandRange(1, 3)), new IntRange(0, max_floors), 20);//equip
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeRange(true, true, new RandRange(1, 3), ItemArray(IterateTypePlates())), new IntRange(0, max_floors), 10);//plates
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeType(ItemData.UseType.Learn, true, true, new RandRange(1, 3)), new IntRange(0, max_floors), 10);//TMs
                    chestChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(2, 5)), new IntRange(0, max_floors), 20);
                    chestChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(RecruitState)), true, true, new RandRange(1)), new IntRange(0, max_floors), 10);
                }
            }
            else
            {
                if (ambush)
                    chestChanceZoneStep.Items.Add(new MapItem("food_apple_perfect"), new IntRange(0, max_floors), 20);//perfect apple

                if (gamePhase > DungeonStage.Beginner)
                {
                    foreach (string key in IterateVitamins())
                        chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//boosters
                }
                foreach (string key in IterateGummis())
                    chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//gummis
                chestChanceZoneStep.Items.Add(new MapItem("apricorn_big"), new IntRange(0, max_floors), 10);//big apricorn
                chestChanceZoneStep.Items.Add(new MapItem("medicine_elixir"), new IntRange(0, max_floors), 20);//elixir
                chestChanceZoneStep.Items.Add(new MapItem("medicine_potion"), new IntRange(0, max_floors), 20);//potion
                chestChanceZoneStep.Items.Add(new MapItem("medicine_max_elixir"), new IntRange(0, max_floors), 20);//max elixir
                chestChanceZoneStep.Items.Add(new MapItem("medicine_max_potion"), new IntRange(0, max_floors), 20);//max potion
                chestChanceZoneStep.Items.Add(new MapItem("medicine_full_heal"), new IntRange(0, max_floors), 20);//full heal
                foreach (string key in IterateXItems())
                    chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 10);//X-Items

                if (gamePhase < DungeonStage.Advanced)
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear", 1), new IntRange(0, max_floors), 20);//amber tear

                if (gamePhase > DungeonStage.Beginner)
                {
                    chestChanceZoneStep.Items.Add(new MapItem("loot_nugget"), new IntRange(0, max_floors), 20);//nugget
                    chestChanceZoneStep.Items.Add(new MapItem("seed_joy"), new IntRange(0, max_floors), 15);//joy seed
                    chestChanceZoneStep.Items.Add(new MapItem("machine_ability_capsule"), new IntRange(0, max_floors), 15);//ability capsule
                }

                if (ambush)
                {
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(3), "loot_pearl"), new ItemThemeNone(100, new RandRange(3, 5))), new IntRange(0, max_floors), 30);
                    chestChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(5, 10)), new IntRange(0, max_floors), 10);//gummis
                }
                else
                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeNone(100, new RandRange(1, 3)), new IntRange(0, max_floors), 30);
            }
        }

        static void PopulateHouseItems(SpreadHouseZoneStep zoneStep, DungeonStage gamePhase, DungeonAccessibility access)
        {

        }

        static void PopulateHallItems(SpreadHouseZoneStep monsterChanceZoneStep, DungeonStage gamePhase, DungeonAccessibility access, int max_floors)
        {
            foreach (string key in IterateGummis())
                monsterChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//gummis

            monsterChanceZoneStep.Items.Add(new MapItem("food_banana"), new IntRange(0, max_floors), 25);//banana
            monsterChanceZoneStep.Items.Add(new MapItem("loot_nugget"), new IntRange(0, max_floors), 10);//nugget
            monsterChanceZoneStep.Items.Add(new MapItem("loot_pearl", 1), new IntRange(0, max_floors), 10);//pearl
            monsterChanceZoneStep.Items.Add(new MapItem("loot_heart_scale", 2), new IntRange(0, max_floors), 10);//heart scale
            monsterChanceZoneStep.Items.Add(new MapItem("key", 1), new IntRange(0, max_floors), 10);//key
            monsterChanceZoneStep.Items.Add(new MapItem("machine_recall_box"), new IntRange(0, max_floors), 30);//link box
            if (access != DungeonAccessibility.MainPath)
                monsterChanceZoneStep.Items.Add(new MapItem("machine_assembly_box"), new IntRange(10, max_floors), 30);//assembly box
            monsterChanceZoneStep.Items.Add(new MapItem("machine_ability_capsule"), new IntRange(0, max_floors), 10);//ability capsule

            if (gamePhase == DungeonStage.Rogue && access == DungeonAccessibility.MainPath)
            {
                monsterChanceZoneStep.Items.Add(new MapItem("held_friend_bow"), new IntRange(0, max_floors), 10);//friend bow
            }
            else
            {
                monsterChanceZoneStep.Items.Add(new MapItem("held_mobile_scarf"), new IntRange(0, max_floors), 1);//Mobile Scarf
                monsterChanceZoneStep.Items.Add(new MapItem("held_pass_scarf"), new IntRange(0, max_floors), 1);//Pass Scarf
                monsterChanceZoneStep.Items.Add(new MapItem("held_cover_band"), new IntRange(0, max_floors), 1);//Cover Band
                monsterChanceZoneStep.Items.Add(new MapItem("held_reunion_cape"), new IntRange(0, max_floors), 1);//Reunion Cape
                monsterChanceZoneStep.Items.Add(new MapItem("held_trap_scarf"), new IntRange(0, max_floors), 1);//Trap Scarf
                monsterChanceZoneStep.Items.Add(new MapItem("held_grip_claw"), new IntRange(0, max_floors), 1);//Grip Claw
                monsterChanceZoneStep.Items.Add(new MapItem("held_twist_band"), new IntRange(0, max_floors), 1);//Twist Band
                monsterChanceZoneStep.Items.Add(new MapItem("held_metronome"), new IntRange(0, max_floors), 1);//Metronome
                monsterChanceZoneStep.Items.Add(new MapItem("held_shell_bell"), new IntRange(0, max_floors), 1);//Shell Bell
                monsterChanceZoneStep.Items.Add(new MapItem("held_scope_lens"), new IntRange(0, max_floors), 1);//Scope Lens
                monsterChanceZoneStep.Items.Add(new MapItem("held_power_band"), new IntRange(0, max_floors), 1);//Power Band
                monsterChanceZoneStep.Items.Add(new MapItem("held_special_band"), new IntRange(0, max_floors), 1);//Special Band
                monsterChanceZoneStep.Items.Add(new MapItem("held_defense_scarf"), new IntRange(0, max_floors), 1);//Defense Scarf
                monsterChanceZoneStep.Items.Add(new MapItem("held_zinc_band"), new IntRange(0, max_floors), 1);//Zinc Band
                monsterChanceZoneStep.Items.Add(new MapItem("held_wide_lens"), new IntRange(0, max_floors), 1);//Wide Lens
                monsterChanceZoneStep.Items.Add(new MapItem("held_pierce_band"), new IntRange(0, max_floors), 1);//Pierce Band
                monsterChanceZoneStep.Items.Add(new MapItem("held_shed_shell"), new IntRange(0, max_floors), 1);//Shed Shell
                monsterChanceZoneStep.Items.Add(new MapItem("held_x_ray_specs"), new IntRange(0, max_floors), 1);//X-Ray Specs
                monsterChanceZoneStep.Items.Add(new MapItem("held_big_root"), new IntRange(0, max_floors), 1);//Big Root
                monsterChanceZoneStep.Items.Add(new MapItem("held_weather_rock"), new IntRange(0, max_floors), 1);//Weather Rock
                monsterChanceZoneStep.Items.Add(new MapItem("held_expert_belt"), new IntRange(0, max_floors), 1);//Expert Belt
                monsterChanceZoneStep.Items.Add(new MapItem("held_heal_ribbon"), new IntRange(0, max_floors), 1);//Heal Ribbon
                monsterChanceZoneStep.Items.Add(new MapItem("held_goggle_specs"), new IntRange(0, max_floors), 1);//Goggle Specs

                if (gamePhase == DungeonStage.Rogue)
                {
                    monsterChanceZoneStep.Items.Add(new MapItem("held_choice_scarf"), new IntRange(0, max_floors), 1);//Choice Scarf
                    monsterChanceZoneStep.Items.Add(new MapItem("held_choice_specs"), new IntRange(0, max_floors), 1);//Choice Specs
                    monsterChanceZoneStep.Items.Add(new MapItem("held_choice_band"), new IntRange(0, max_floors), 1);//Choice Band
                    monsterChanceZoneStep.Items.Add(new MapItem("held_assault_vest"), new IntRange(0, max_floors), 1);//Assault Vest
                    monsterChanceZoneStep.Items.Add(new MapItem("held_life_orb"), new IntRange(0, max_floors), 1);//Life Orb
                }
            }
        }

        public enum DungeonEnvironment
        {
            Rock,
            Forest
        }

        static void PopulateWallItems(SpawnList<MapItem> wallSpawns, DungeonStage gamePhase, DungeonEnvironment environment)
        {
            if (environment == DungeonEnvironment.Rock)
            {
                //Add:
                // hidden wall items (silver spikes, rare fossils, wands, orbs, plates) - never add anything score-affecting when it comes to roguelockable dungeons
                // No valuable items, no chests, no distinct TMs.  maybe held items...
                wallSpawns.Add(new MapItem("loot_heart_scale", 1), 50);//heart scale
                wallSpawns.Add(new MapItem("ammo_silver_spike", 3), 20);//silver spike
                wallSpawns.Add(new MapItem("ammo_rare_fossil", 3), 20);//rare fossil
                wallSpawns.Add(new MapItem("ammo_corsola_twig", 2), 20);//corsola spike
                wallSpawns.Add(new MapItem("berry_jaboca"), 10);//jaboca berry
                wallSpawns.Add(new MapItem("berry_rowap"), 10);//rowap berry
                wallSpawns.Add(new MapItem("wand_path", 1), 10);//path wand
                wallSpawns.Add(new MapItem("wand_fear", 3), 10);//fear wand
                wallSpawns.Add(new MapItem("wand_switcher", 2), 10);//switcher wand
                wallSpawns.Add(new MapItem("wand_whirlwind", 2), 10);//whirlwind wand

                wallSpawns.Add(new MapItem("orb_slumber"), 10);//Slumber Orb
                wallSpawns.Add(new MapItem("orb_slow"), 10);//Slow
                wallSpawns.Add(new MapItem("orb_totter"), 10);//Totter
                wallSpawns.Add(new MapItem("orb_spurn"), 10);//Spurn
                wallSpawns.Add(new MapItem("orb_stayaway"), 10);//Stayaway
                wallSpawns.Add(new MapItem("orb_pierce"), 10);//Pierce
                foreach (string key in IterateTypePlates())
                    wallSpawns.Add(new MapItem(key), 1);
            }
            else if (environment == DungeonEnvironment.Forest)
            {
                wallSpawns.Add(new MapItem("berry_oran"), 50);//oran berry
                wallSpawns.Add(new MapItem("loot_heart_scale", 1), 50);//heart scale
                wallSpawns.Add(new MapItem("ammo_cacnea_spike", 2), 20);//cacnea spike
                wallSpawns.Add(new MapItem("ammo_stick", 2), 20);//stick
                wallSpawns.Add(new MapItem("berry_jaboca"), 10);//jaboca berry
                wallSpawns.Add(new MapItem("berry_rowap"), 10);//rowap berry
                wallSpawns.Add(new MapItem("wand_path", 1), 10);//path wand
                wallSpawns.Add(new MapItem("wand_fear", 3), 10);//fear wand
                wallSpawns.Add(new MapItem("wand_transfer", 2), 10);//transfer wand
                wallSpawns.Add(new MapItem("wand_vanish", 4), 10);//vanish wand

                wallSpawns.Add(new MapItem("berry_apicot"), 10);//apicot berry
                wallSpawns.Add(new MapItem("berry_liechi"), 10);//liechi berry
                wallSpawns.Add(new MapItem("berry_ganlon"), 10);//ganlon berry
                wallSpawns.Add(new MapItem("berry_salac"), 10);//salac berry
                wallSpawns.Add(new MapItem("berry_petaya"), 10);//petaya berry
                wallSpawns.Add(new MapItem("berry_starf"), 10);//starf berry
                wallSpawns.Add(new MapItem("berry_micle"), 10);//micle berry
                wallSpawns.Add(new MapItem("berry_enigma"), 10);//enigma berry

                foreach (string key in IterateTypeBerries())
                    wallSpawns.Add(new MapItem(key), 1);
            }
        }

        public enum ShopType
        {
            Kecleon = 0,
            Shuckle = 1,
            Cleffa = 2,
            Porygon = 3
        }

        static void PopulateShopItems<T>(ShopStep<T> zoneStep, DungeonStage gamePhase, DungeonAccessibility access, ShopType hop) where T : ListMapGenContext
        {
            //TODO later
        }

    }
}
