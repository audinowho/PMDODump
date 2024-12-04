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

namespace DataGenerator.Data
{
    public partial class ZoneInfo
    {
        static void FillSacredTower(ZoneData zone, bool translate)
        {
            #region SACRED TOWER
            {
                zone.Name = new LocalText("**Sacred Tower");
                zone.Level = 50;
                zone.LevelCap = true;
                zone.KeepSkills = true;
                zone.BagRestrict = 0;
                zone.KeepTreasure = true;
                zone.MoneyRestrict = true;
                zone.TeamSize = 3;
                zone.Rescues = 2;
                zone.Rogue = RogueStatus.NoTransfer;

                {
                    int max_floors = 70;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Sacred Tower\n{0}F"), new Priority(-15)));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = new MoneySpawnZoneStep(PR_RESPAWN_MONEY, new RandRange(1), new RandRange(1));
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;
                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    floorSegment.ZoneSteps.Add(tileSpawn);



                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        AddFloorData(layout, "Sacred Tower.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                        //Tilesets
                        // candidates: buried_relic_2,temporal_spire,temporal_tower,joyous_tower,electric maze
                        if (ii < 10)
                            AddTextureData(layout, "buried_relic_2_wall", "buried_relic_2_floor", "buried_relic_2_secondary", "normal");
                        else if (ii < 20)
                            AddTextureData(layout, "temporal_tower_wall", "temporal_tower_floor", "temporal_tower_secondary", "normal");
                        else if (ii < 30)
                            AddTextureData(layout, "temporal_spire_wall", "temporal_spire_floor", "temporal_spire_secondary", "normal");
                        else if (ii < 40)
                            AddTextureData(layout, "joyous_tower_wall", "joyous_tower_floor", "joyous_tower_secondary", "normal");
                        else if (ii < 50)
                            AddTextureData(layout, "electric_maze_wall", "electric_maze_floor", "electric_maze_secondary", "normal");
                        else if (ii < 60)
                            AddTextureData(layout, "temporal_tower_wall", "temporal_tower_floor", "temporal_tower_secondary", "normal");
                        else
                            AddTextureData(layout, "temporal_tower_wall", "temporal_tower_floor", "temporal_tower_secondary", "normal");

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(6, 9));

                        //money
                        AddMoneyData(layout, new RandRange(2, 4));

                        //enemies!
                        AddRespawnData(layout, 3, 80);

                        //enemies
                        AddEnemySpawnData(layout, 20, new RandRange(2, 4));

                        //items
                        AddItemData(layout, new RandRange(3, 6), 25);


                        //construct paths
                        {
                            AddInitGridStep(layout, 3, 2, 10, 10);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(90);
                            path.BranchRatio = new RandRange(0, 25);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 11), new RandRange(4, 11), new RandRange(2, 6), new RandRange(2, 6)), 10);
                            //round
                            genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));

                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, false);

                        AddWaterSteps(layout, "water", new RandRange(30));//water

                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
            }
            #endregion
        }

        static void FillLostSeas(ZoneData zone, bool translate)
        {
            #region LOST SEAS
            {
                zone.Name = new LocalText("**Lost Seas");
                zone.Level = 50;
                zone.MoneyRestrict = true;
                zone.Rescues = 2;
                zone.Rogue = RogueStatus.NoTransfer;

                {
                    int max_floors = 3;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Lost Seas\nB{0}F"), new Priority(-15)));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = new MoneySpawnZoneStep(PR_RESPAWN_MONEY, new RandRange(1), new RandRange(1));
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;
                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    floorSegment.ZoneSteps.Add(tileSpawn);



                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        AddFloorData(layout, "Title.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                        //Tilesets
                        AddTextureData(layout, "test_dungeon_wall", "test_dungeon_floor", "test_dungeon_secondary", "normal");

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(6, 9));

                        //money
                        AddMoneyData(layout, new RandRange(2, 4));

                        //enemies!
                        AddRespawnData(layout, 3, 80);

                        //enemies
                        AddEnemySpawnData(layout, 20, new RandRange(2, 4));

                        //items
                        AddItemData(layout, new RandRange(3, 6), 25);


                        //construct paths
                        {
                            AddInitGridStep(layout, 4, 4, 10, 10);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(90);
                            path.BranchRatio = new RandRange(0, 25);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 11), new RandRange(4, 11), new RandRange(2, 6), new RandRange(2, 6)), 10);
                            //round
                            genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));

                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, false);

                        AddWaterSteps(layout, "water", new RandRange(30));//water

                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
            }
            #endregion
        }

        static void FillInscribedCave(ZoneData zone, bool translate)
        {
            #region INSCRIBED CAVE
            {
                zone.Name = new LocalText("**Inscribed Cave");
                zone.Level = 60;
                zone.BagRestrict = 0;
                zone.KeepTreasure = true;
                zone.MoneyRestrict = true;
                zone.TeamSize = 2;
                zone.Rescues = 2;
                zone.Rogue = RogueStatus.NoTransfer;

                {
                    int max_floors = 3;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Inscribed Cave\nB{0}F"), new Priority(-15)));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = new MoneySpawnZoneStep(PR_RESPAWN_MONEY, new RandRange(1), new RandRange(1));
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;
                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    floorSegment.ZoneSteps.Add(tileSpawn);



                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        AddFloorData(layout, "Title.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                        //Tilesets
                        AddTextureData(layout, "test_dungeon_wall", "test_dungeon_floor", "test_dungeon_secondary", "normal");

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(6, 9));

                        //money
                        AddMoneyData(layout, new RandRange(2, 4));

                        //enemies!
                        AddRespawnData(layout, 3, 80);

                        //enemies
                        AddEnemySpawnData(layout, 20, new RandRange(2, 4));

                        //items
                        AddItemData(layout, new RandRange(3, 6), 25);


                        //construct paths
                        {
                            AddInitGridStep(layout, 4, 4, 10, 10);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(90);
                            path.BranchRatio = new RandRange(0, 25);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 11), new RandRange(4, 11), new RandRange(2, 6), new RandRange(2, 6)), 10);
                            //round
                            genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));

                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, false);

                        AddWaterSteps(layout, "water", new RandRange(30));//water

                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
            }
            #endregion
        }

        static void FillRoyalHalls(ZoneData zone, bool translate)
        {
            #region ROYAL HALLS
            {
                zone.Name = new LocalText("**Royal Halls");
                zone.Rescues = 4;
                zone.Level = 80;
                zone.ExpPercent = 0;
                zone.Rogue = RogueStatus.NoTransfer;

                LayeredSegment floorSegment = new LayeredSegment();
                floorSegment.IsRelevant = true;
                floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Royal Halls\nB{0}F"), new Priority(-15)));

                //money
                MoneySpawnZoneStep moneySpawnZoneStep = new MoneySpawnZoneStep(PR_RESPAWN_MONEY, new RandRange(1), new RandRange(1));
                moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                //items
                ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;
                floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                //mobs
                TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                poolSpawn.Priority = PR_RESPAWN_MOB;

                poolSpawn.TeamSizes.Add(1, new IntRange(0, 25), 12);
                floorSegment.ZoneSteps.Add(poolSpawn);

                TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                tileSpawn.Priority = PR_RESPAWN_TRAP;
                floorSegment.ZoneSteps.Add(tileSpawn);


                for (int ii = 0; ii < 25; ii++)
                {
                    GridFloorGen layout = new GridFloorGen();

                    //Floor settings
                    AddFloorData(layout, "Fortune Ravine.ogg", 1000, Map.SightRange.Clear, Map.SightRange.Dark);

                    //Tilesets
                    AddTextureData(layout, "golden_chamber_wall", "golden_chamber_floor", "golden_chamber_secondary", "normal");

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                    AddTrapsSteps(layout, new RandRange(6, 9));

                    //money - Ballpark 40K
                    AddMoneyData(layout, new RandRange(2, 4));

                    //enemies! ~ lv 35 to 50
                    AddRespawnData(layout, 3, 80);

                    //enemies
                    AddEnemySpawnData(layout, 20, new RandRange(2, 4));


                    //items
                    AddItemData(layout, new RandRange(3, 6), 25);


                    //construct paths
                    {
                        AddInitGridStep(layout, 4, 4, 10, 10);

                        GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.RoomRatio = new RandRange(90);
                        path.BranchRatio = new RandRange(0, 25);

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //cross
                        genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 11), new RandRange(4, 11), new RandRange(2, 6), new RandRange(2, 6)), 10);
                        //round
                        genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_GRID_GEN, path);

                        layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));

                    }

                    AddDrawGridSteps(layout);

                    AddStairStep(layout, false);

                    AddWaterSteps(layout, "water", new RandRange(30));//water

                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                    floorSegment.Floors.Add(layout);
                }

                zone.Segments.Add(floorSegment);
            }
            #endregion
        }

        static void FillCaveOfSolace(ZoneData zone, bool translate)
        {
            #region CAVE OF SOLACE
            {
                zone.Name = new LocalText("**Cave of Solace");
                zone.BagRestrict = 0;
                zone.KeepTreasure = true;
                zone.MoneyRestrict = true;
                zone.TeamSize = 1;
                zone.Level = 80;
                zone.ExpPercent = 0;
                zone.Rescues = 4;
                zone.Rogue = RogueStatus.NoTransfer;

                LayeredSegment floorSegment = new LayeredSegment();
                floorSegment.IsRelevant = true;
                floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Cave of Solace\nB{0}F"), new Priority(-15)));

                //money
                MoneySpawnZoneStep moneySpawnZoneStep = new MoneySpawnZoneStep(PR_RESPAWN_MONEY, new RandRange(1), new RandRange(1));
                moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                //items
                ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;
                floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                //mobs
                TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                poolSpawn.Priority = PR_RESPAWN_MOB;

                poolSpawn.TeamSizes.Add(1, new IntRange(0, 25), 12);
                floorSegment.ZoneSteps.Add(poolSpawn);

                TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                tileSpawn.Priority = PR_RESPAWN_TRAP;
                floorSegment.ZoneSteps.Add(tileSpawn);



                for (int ii = 0; ii < 25; ii++)
                {
                    GridFloorGen layout = new GridFloorGen();

                    //Floor settings
                    if (ii < 16)
                        AddFloorData(layout, "Limestone Cavern.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);
                    else
                        AddFloorData(layout, "Deep Limestone Cavern.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                    //Tilesets
                    //northern_range_2, meteor_cave
                    if (ii < 8)
                        AddTextureData(layout, "howling_forest_2_wall", "howling_forest_2_floor", "howling_forest_2_secondary", "psychic");
                    else if (ii < 16)
                        AddTextureData(layout, "spacial_rift_1_wall", "spacial_rift_1_floor", "spacial_rift_1_secondary", "psychic");
                    else
                        AddTextureData(layout, "spacial_rift_2_wall", "spacial_rift_2_floor", "spacial_rift_2_secondary", "psychic");

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                    AddTrapsSteps(layout, new RandRange(6, 9));

                    //money - Ballpark 20K
                    AddMoneyData(layout, new RandRange(2, 4));

                    //enemies!~lv 40 to 50; recruitables must be 40
                    AddRespawnData(layout, 3, 80);

                    //enemies
                    AddEnemySpawnData(layout, 20, new RandRange(2, 4));

                    //items
                    AddItemData(layout, new RandRange(3, 6), 25);


                    //construct paths
                    {
                        AddInitGridStep(layout, 4, 4, 10, 10);

                        GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.RoomRatio = new RandRange(90);
                        path.BranchRatio = new RandRange(0, 25);

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //cross
                        genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 11), new RandRange(4, 11), new RandRange(2, 6), new RandRange(2, 6)), 10);
                        //round
                        genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_GRID_GEN, path);

                        layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));

                    }

                    AddDrawGridSteps(layout);

                    AddStairStep(layout, false);

                    AddWaterSteps(layout, "water", new RandRange(30));//water

                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                    floorSegment.Floors.Add(layout);
                }

                zone.Segments.Add(floorSegment);
            }
            #endregion
        }

        static void FillTheSky(ZoneData zone, bool translate)
        {
            #region THE SKY
            {
                zone.Name = new LocalText("**The Sky");
                zone.TeamSize = 3;
                zone.Rescues = 4;
                zone.Level = 70;
                zone.MoneyRestrict = true;
                zone.Rogue = RogueStatus.NoTransfer;

                {
                    int max_floors = 30;

                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("The Sky\n{0}F"), new Priority(-15)));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = new MoneySpawnZoneStep(PR_RESPAWN_MONEY, new RandRange(1), new RandRange(1));
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;
                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    floorSegment.ZoneSteps.Add(tileSpawn);


                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();


                        //Floor settings
                        AddFloorData(layout, "Sky Tower.ogg", 1000, Map.SightRange.Clear, Map.SightRange.Dark);


                        //Tilesets
                        AddTextureData(layout, "sky_tower_wall", "sky_tower_floor", "sky_tower_secondary", "flying");

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(6, 9));

                        //money - Ballpark 30K
                        AddMoneyData(layout, new RandRange(2, 4));

                        //enemies! ~ lv 40 to 55
                        AddRespawnData(layout, 3, 80);

                        //enemies
                        AddEnemySpawnData(layout, 20, new RandRange(2, 4));

                        //items
                        AddItemData(layout, new RandRange(3, 6), 25);


                        //construct paths
                        {
                            AddInitGridStep(layout, 4, 4, 10, 10);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(90);
                            path.BranchRatio = new RandRange(0, 25);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 11), new RandRange(4, 11), new RandRange(2, 6), new RandRange(2, 6)), 10);
                            //round
                            genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));

                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, false);

                        AddWaterSteps(layout, "water", new RandRange(30));//water

                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
            }
            #endregion
        }

        static void FillTheAbyss(ZoneData zone, bool translate)
        {
            #region THE ABYSS
            {
                zone.Name = new LocalText("**The Abyss");
                zone.Level = 100;
                zone.LevelCap = true;
                zone.BagRestrict = 0;
                zone.KeepTreasure = true;
                zone.MoneyRestrict = true;
                zone.TeamSize = 1;
                zone.Rogue = RogueStatus.NoTransfer;

                LayeredSegment floorSegment = new LayeredSegment();
                floorSegment.IsRelevant = true;
                floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("The Abyss\nB{0}F"), new Priority(-15)));

                //money
                MoneySpawnZoneStep moneySpawnZoneStep = new MoneySpawnZoneStep(PR_RESPAWN_MONEY, new RandRange(1), new RandRange(1));
                moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                //items
                ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;
                floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                //mobs
                TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                poolSpawn.Priority = PR_RESPAWN_MOB;

                poolSpawn.TeamSizes.Add(1, new IntRange(0, 90), 12);
                floorSegment.ZoneSteps.Add(poolSpawn);

                TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                tileSpawn.Priority = PR_RESPAWN_TRAP;
                floorSegment.ZoneSteps.Add(tileSpawn);



                SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(10, new IntRange(4, 20)));
                monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHouseStep<ListMapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                floorSegment.ZoneSteps.Add(monsterChanceZoneStep);

                SpreadHouseZoneStep chestChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanQuota(new RandRange(2, 5), new IntRange(6, 30)));
                chestChanceZoneStep.ModStates.Add(new FlagType(typeof(ChestModGenState)));
                chestChanceZoneStep.HouseStepSpawns.Add(new ChestStep<ListMapGenContext>(false, GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                floorSegment.ZoneSteps.Add(chestChanceZoneStep);

                SpreadHouseZoneStep monsterChestChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanQuota(new RandDecay(1, 5, 50), new IntRange(6, 30)));
                monsterChestChanceZoneStep.HouseStepSpawns.Add(new ChestStep<ListMapGenContext>(true, GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                floorSegment.ZoneSteps.Add(monsterChestChanceZoneStep);

                //Spawn a golden apple on B1F

                for (int ii = 0; ii < 90; ii++)
                {
                    GridFloorGen layout = new GridFloorGen();

                    //Floor settings
                    if (ii < 10)
                        AddFloorData(layout, "Treacherous Mountain.ogg", 3000, Map.SightRange.Dark, Map.SightRange.Dark);
                    else if (ii < 20)
                        AddFloorData(layout, "Ambush Forest.ogg", 3000, Map.SightRange.Dark, Map.SightRange.Dark);
                    else if (ii < 30)
                        AddFloorData(layout, "Treacherous Mountain 2.ogg", 3000, Map.SightRange.Dark, Map.SightRange.Dark);
                    else if (ii < 40)
                        AddFloorData(layout, "Deep Dark Crater.ogg", 3000, Map.SightRange.Dark, Map.SightRange.Dark);
                    else if (ii < 50)
                        AddFloorData(layout, "Spring Cave.ogg", 3000, Map.SightRange.Dark, Map.SightRange.Dark);
                    else if (ii < 60)
                        AddFloorData(layout, "Lower Spring Cave.ogg", 3000, Map.SightRange.Dark, Map.SightRange.Dark);
                    else if (ii < 70)
                        AddFloorData(layout, "Spring Cave Depths.ogg", 3000, Map.SightRange.Dark, Map.SightRange.Dark);
                    else if (ii < 80)
                        AddFloorData(layout, "Fortune Ravine Depths.ogg", 3000, Map.SightRange.Dark, Map.SightRange.Dark);
                    else if (ii < 90)
                        AddFloorData(layout, "Limestone Cavern.ogg", 3000, Map.SightRange.Dark, Map.SightRange.Dark);
                    else
                        AddFloorData(layout, "Icicle Forest.ogg", 3000, Map.SightRange.Dark, Map.SightRange.Dark);


                    //Tilesets
                    if (ii % 10 == 9)
                        AddTextureData(layout, "world_abyss_2_wall", "world_abyss_2_floor", "world_abyss_2_secondary", "ghost");
                    else if (ii < 10)
                        AddTextureData(layout, "purity_forest_2_wall", "purity_forest_2_floor", "purity_forest_2_secondary", "normal");
                    else if (ii < 20)
                        AddTextureData(layout, "purity_forest_2_wall", "purity_forest_2_floor", "purity_forest_2_secondary", "normal");
                    else if (ii < 30)
                        AddTextureData(layout, "purity_forest_2_wall", "purity_forest_2_floor", "purity_forest_2_secondary", "normal");
                    else if (ii < 40)
                        AddTextureData(layout, "purity_forest_2_wall", "purity_forest_2_floor", "purity_forest_2_secondary", "normal");
                    else if (ii < 50)
                        AddTextureData(layout, "purity_forest_2_wall", "purity_forest_2_floor", "purity_forest_2_secondary", "normal");
                    else if (ii < 60)
                        AddTextureData(layout, "purity_forest_2_wall", "purity_forest_2_floor", "purity_forest_2_secondary", "normal");
                    else if (ii < 70)
                        AddTextureData(layout, "purity_forest_2_wall", "purity_forest_2_floor", "purity_forest_2_secondary", "normal");
                    else if (ii < 80)
                        AddTextureData(layout, "purity_forest_2_wall", "purity_forest_2_floor", "purity_forest_2_secondary", "normal");
                    else if (ii < 90)
                        AddTextureData(layout, "purity_forest_2_wall", "purity_forest_2_floor", "purity_forest_2_secondary", "normal");
                    else
                        AddTextureData(layout, "mt_faraway_4_wall", "mt_faraway_4_floor", "mt_faraway_4_secondary", "normal");


                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                    AddTrapsSteps(layout, new RandRange(6, 9));

                    //money - Ballpark 90K
                    AddMoneyData(layout, new RandRange(2, 4));

                    //enemies! ~ up to lv 70
                    AddRespawnData(layout, 3, 80);

                    //enemies
                    AddEnemySpawnData(layout, 20, new RandRange(2, 4));

                    //items
                    AddItemData(layout, new RandRange(3, 6), 25);

                    //construct paths
                    {
                        AddInitGridStep(layout, 4, 4, 10, 10);

                        GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.RoomRatio = new RandRange(90);
                        path.BranchRatio = new RandRange(0, 25);

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //cross
                        genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 11), new RandRange(4, 11), new RandRange(2, 6), new RandRange(2, 6)), 10);
                        //round
                        genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_GRID_GEN, path);

                        layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));

                    }

                    AddDrawGridSteps(layout);

                    AddStairStep(layout, false);

                    AddWaterSteps(layout, "water", new RandRange(30));//water

                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                    floorSegment.Floors.Add(layout);
                }

                zone.Segments.Add(floorSegment);
            }
            #endregion
        }

        static void FillNeverEndingTale(ZoneData zone, bool translate)
        {
            #region NEVERENDING TALE
            {
                zone.Name = new LocalText("The NeverEnding Tale");
                zone.Level = 5;
                zone.BagRestrict = 0;
                zone.MoneyRestrict = true;
                zone.Rogue = RogueStatus.NoTransfer;

                {
                    int max_floors = 3;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("The NeverEnding Tale\nCh. {0}"), new Priority(-15)));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = new MoneySpawnZoneStep(PR_RESPAWN_MONEY, new RandRange(1), new RandRange(1));
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;
                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    floorSegment.ZoneSteps.Add(tileSpawn);



                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        AddFloorData(layout, "Hidden Land.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                        //Tilesets
                        AddTextureData(layout, "test_dungeon_wall", "test_dungeon_floor", "test_dungeon_secondary", "normal");

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(6, 9));

                        //money
                        AddMoneyData(layout, new RandRange(2, 4));

                        //enemies!
                        AddRespawnData(layout, 3, 80);

                        //enemies
                        AddEnemySpawnData(layout, 20, new RandRange(2, 4));

                        //items
                        AddItemData(layout, new RandRange(3, 6), 25);


                        //construct paths
                        {
                            AddInitGridStep(layout, 4, 4, 10, 10);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(90);
                            path.BranchRatio = new RandRange(0, 25);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 11), new RandRange(4, 11), new RandRange(2, 6), new RandRange(2, 6)), 10);
                            //round
                            genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));

                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, false);

                        AddWaterSteps(layout, "water", new RandRange(30));//water

                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }

                zone.GroundMaps.Add("dev_room");
            }
            #endregion
        }
    }
}
