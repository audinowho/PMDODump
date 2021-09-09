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
        public const int MAX_ZONES = 50;

        public static void AddZoneData()
        {
            //DataInfo.DeleteIndexedData(DataManager.DataType.Zone.ToString());
            for (int ii = 0; ii < MAX_ZONES; ii++)
            {
                ZoneData zone = GetZoneData(ii);
                DataManager.SaveData(ii, DataManager.DataType.Zone.ToString(), zone);
            }
        }


        public static ZoneData GetZoneData(int index)
        {
            ZoneData zone = new ZoneData();
            if (index == 0)
                FillDebugZone(zone);
            else if (index == 2)
            {
                #region TROPICAL PATH
                {
                    zone.Name = new LocalText("Tropical Path");
                    zone.Rescues = 2;
                    zone.Level = 5;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    int max_floors = 4;

                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Tropical Path\n{0}F")));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = new MoneySpawnZoneStep(PR_RESPAWN_MONEY, new RandRange(10, 21), new RandRange(10));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                    //items!
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;

                    CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                    necessities.SpawnRates.SetRange(14, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("necessities", necessities);

                    necessities.Spawns.Add(new InvItem(11), new IntRange(0, max_floors), 9);//Leppa
                    necessities.Spawns.Add(new InvItem(10), new IntRange(0, max_floors), 12);//Oran
                    necessities.Spawns.Add(new InvItem(1), new IntRange(0, max_floors), 10);//Apple
                    necessities.Spawns.Add(new InvItem(12), new IntRange(0, max_floors), 10);//Lum
                    necessities.Spawns.Add(new InvItem(101), new IntRange(0, max_floors), 5);//reviver seed
                    necessities.Spawns.Add(new InvItem(112), new IntRange(0, max_floors), 9);//blast seed


                    CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                    special.SpawnRates.SetRange(7, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("special", special);

                    int rate = 2;
                    special.Spawns.Add(new InvItem(211), new IntRange(1, max_floors), rate);//blue apricorns
                    special.Spawns.Add(new InvItem(212), new IntRange(1, max_floors), rate);//green apricorns
                    special.Spawns.Add(new InvItem(216), new IntRange(1, max_floors), rate);//white apricorns

                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;
                    //161 Sentret : 10 Scratch
                    poolSpawn.Spawns.Add(GetTeamMob(161, -1, 10, -1, -1, -1, new RandRange(2), 16), new IntRange(0, 2), 10);
                    poolSpawn.Spawns.Add(GetTeamMob(161, -1, 10, -1, -1, -1, new RandRange(5), 16), new IntRange(2, max_floors), 10);
                    //191 Sunkern : 71 Absorb
                    poolSpawn.Spawns.Add(GetTeamMob(191, -1, 71, -1, -1, -1, new RandRange(3), 16), new IntRange(0, 2), 10);
                    poolSpawn.Spawns.Add(GetTeamMob(191, -1, 71, -1, -1, -1, new RandRange(5), 16), new IntRange(1, max_floors), 10);
                    //396 Starly : 33 Tackle
                    poolSpawn.Spawns.Add(GetTeamMob(396, -1, 33, -1, -1, -1, new RandRange(2), 16), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob(396, -1, 33, -1, -1, -1, new RandRange(4), 16), new IntRange(2, max_floors), 10);
                    //10 Caterpie : 33 Tackle : 81 String Shot
                    poolSpawn.Spawns.Add(GetTeamMob(10, -1, 33, 81, -1, -1, new RandRange(4), 16), new IntRange(2, max_floors), 10);
                    //120 Staryu : 55 Water Gun
                    poolSpawn.Spawns.Add(GetTeamMob(120, 30, 55, -1, -1, -1, new RandRange(4), 16), new IntRange(2, max_floors), 10);
                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    floorSegment.ZoneSteps.Add(tileSpawn);

                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        AddFloorData(layout, "B04. Tropical Path.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Clear);

                        AddWaterSteps(layout, 3, new RandRange(30));//water

                        //Tilesets
                        AddTextureData(layout, 150, 151, 152, 13);

                        //money
                        AddMoneyData(layout, new RandRange(1, 3));

                        //items
                        AddItemData(layout, new RandRange(2, 4), 25);

                        //enemies! ~ up to lv5
                        AddRespawnData(layout, 3, 80);

                        //enemies
                        AddEnemySpawnData(layout, 20, new RandRange(2, 4));

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), 27);//wonder tile
                        AddTrapsSteps(layout, new RandRange(6, 9));

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
                            genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 10), new RandRange(5, 10)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));

                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, false);

                        floorSegment.Floors.Add(layout);
                    }
                    zone.Segments.Add(floorSegment);
                }
                #endregion
            }
            else if (index == 3)
            {
                #region FADED TRAIL
                {
                    zone.Name = new LocalText("Faded Trail");
                    zone.Rescues = 2;
                    zone.Level = 10;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Faded Trail\n{0}F")));

                        //money
                        MoneySpawnZoneStep moneySpawnZoneStep = new MoneySpawnZoneStep(PR_RESPAWN_MONEY, new RandRange(18, 24), new RandRange(9, 12));
                        floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                        //items
                        ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                        itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;

                        //necessities
                        CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                        necessities.SpawnRates.SetRange(14, new IntRange(0, 7));
                        itemSpawnZoneStep.Spawns.Add("necessities", necessities);

                        necessities.Spawns.Add(new InvItem(11), new IntRange(0, 7), 9);//Leppa
                        necessities.Spawns.Add(new InvItem(10), new IntRange(0, 7), 12);//Oran
                        necessities.Spawns.Add(new InvItem(1), new IntRange(0, 7), 10);//Apple
                        necessities.Spawns.Add(new InvItem(12), new IntRange(0, 7), 10);//Lum
                        necessities.Spawns.Add(new InvItem(101), new IntRange(0, 7), 5);//reviver seed

                        //snacks
                        CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                        snacks.SpawnRates.SetRange(10, new IntRange(0, 7));
                        itemSpawnZoneStep.Spawns.Add("snacks", snacks);

                        snacks.Spawns.Add(new InvItem(112), new IntRange(0, 7), 20);//blast seed
                        snacks.Spawns.Add(new InvItem(108), new IntRange(0, 7), 10);//warp seed
                        snacks.Spawns.Add(new InvItem(110), new IntRange(0, 7), 10);//sleep seed

                        //wands
                        CategorySpawn<InvItem> ammo = new CategorySpawn<InvItem>();
                        ammo.SpawnRates.SetRange(10, new IntRange(0, 7));
                        itemSpawnZoneStep.Spawns.Add("ammo", ammo);

                        ammo.Spawns.Add(new InvItem(200, false, 3), new IntRange(0, 7), 10);//stick
                        ammo.Spawns.Add(new InvItem(222, false, 2), new IntRange(0, 7), 10);//whirlwind wand
                        ammo.Spawns.Add(new InvItem(221, false, 3), new IntRange(0, 7), 10);//pounce wand
                        ammo.Spawns.Add(new InvItem(232, false, 1), new IntRange(0, 7), 10);//warp wand
                        ammo.Spawns.Add(new InvItem(234, false, 2), new IntRange(0, 7), 10);//lob wand
                        ammo.Spawns.Add(new InvItem(207, false, 2), new IntRange(0, 7), 10);//Geo Pebble

                        //orbs
                        CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                        orbs.SpawnRates.SetRange(10, new IntRange(0, 7));
                        itemSpawnZoneStep.Spawns.Add("orbs", orbs);

                        orbs.Spawns.Add(new InvItem(283), new IntRange(0, 7), 10);//Rebound
                        orbs.Spawns.Add(new InvItem(268), new IntRange(0, 7), 5);//All Protect
                        orbs.Spawns.Add(new InvItem(253), new IntRange(0, 7), 9);//Luminous
                        orbs.Spawns.Add(new InvItem(284), new IntRange(0, 7), 8);//Mirror Orb

                        //held items
                        CategorySpawn<InvItem> heldItems = new CategorySpawn<InvItem>();
                        heldItems.SpawnRates.SetRange(2, new IntRange(0, 7));
                        itemSpawnZoneStep.Spawns.Add("held", heldItems);

                        heldItems.Spawns.Add(new InvItem(400), new IntRange(0, 7), 1);//Power Band
                        heldItems.Spawns.Add(new InvItem(401), new IntRange(0, 7), 1);//Special Band
                        heldItems.Spawns.Add(new InvItem(402), new IntRange(0, 7), 1);//Defense Scarf

                        //special
                        CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                        special.SpawnRates.SetRange(7, new IntRange(0, 7));
                        itemSpawnZoneStep.Spawns.Add("special", special);

                        int rate = 2;
                        special.Spawns.Add(new InvItem(211), new IntRange(0, 7), rate);//blue apricorns
                        special.Spawns.Add(new InvItem(212), new IntRange(0, 7), rate);//green apricorns
                        special.Spawns.Add(new InvItem(216), new IntRange(0, 7), rate);//white apricorns
                        special.Spawns.Add(new InvItem(215), new IntRange(0, 7), rate);//red apricorns
                        special.Spawns.Add(new InvItem(217), new IntRange(0, 7), rate);//yellow apricorns
                        special.Spawns.Add(new InvItem(455, false, 1), new IntRange(2, 7), 10);//Key

                        floorSegment.ZoneSteps.Add(itemSpawnZoneStep);
                        SpawnList<IGenPriority> assemblyZoneSpawns = new SpawnList<IGenPriority>();
                        assemblyZoneSpawns.Add(new GenPriority<GenStep<MapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<MapGenContext, MapItem>(new PickerSpawner<MapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(451))))), 10);//assembly box
                        SpreadStepZoneStep assemblyZoneStep = new SpreadStepZoneStep(new SpreadPlanSpaced(new RandRange(2, 7), new IntRange(0, 7)), assemblyZoneSpawns);
                        floorSegment.ZoneSteps.Add(assemblyZoneStep);

                        //mobs
                        TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                        poolSpawn.Priority = PR_RESPAWN_MOB;

                        //need one super-effective for each possible starter
                        //037 Vulpix : 52 Ember
                        poolSpawn.Spawns.Add(GetTeamMob(37, -1, 52, -1, -1, -1, new RandRange(10), 16), new IntRange(4, 7), 10);
                        //114 Tangela : 022 Vine Whip
                        poolSpawn.Spawns.Add(GetTeamMob(114, -1, -1, 22, -1, -1, new RandRange(10), 16), new IntRange(4, 7), 10);
                        //403 Shinx : 033 Tackle : 43 Leer
                        poolSpawn.Spawns.Add(GetTeamMob(403, -1, 33, 43, -1, -1, new RandRange(5), 16), new IntRange(0, 4), 10);
                        poolSpawn.Spawns.Add(GetTeamMob(403, -1, 33, 43, -1, -1, new RandRange(7), 16), new IntRange(4, 7), 10);
                        //190 Aipom : 010 Scratch : Sand Attack
                        poolSpawn.Spawns.Add(GetTeamMob(190, -1, 10, 28, -1, -1, new RandRange(8), 16), new IntRange(0, 7), 10);
                        //161 Sentret : 10 Scratch : 111 Defense Curl
                        poolSpawn.Spawns.Add(GetTeamMob(161, -1, 10, 111, -1, -1, new RandRange(5), 16), new IntRange(0, 4), 10);
                        //060 Poliwag : 55 Water Gun
                        poolSpawn.Spawns.Add(GetTeamMob(60, -1, 55, -1, -1, -1, new RandRange(8), 16), new IntRange(4, 7), 10);
                        //396 Starly : 33 Tackle : 45 Growl - later pairs
                        poolSpawn.Spawns.Add(GetTeamMob(396, -1, 33, 45, -1, -1, new RandRange(6), 16), new IntRange(0, 4), 10);
                        poolSpawn.SpecificSpawns.Add(new SpecificTeamSpawner(GetGenericMob(396, -1, 33, 45, -1, -1, new RandRange(7), 16), GetGenericMob(396, -1, 33, 45, -1, -1, new RandRange(7), 16)), new IntRange(4, 7), 20);

                        poolSpawn.TeamSizes.Add(1, new IntRange(0, 7), 12);
                        floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    floorSegment.ZoneSteps.Add(tileSpawn);


                        for (int ii = 0; ii < 7; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            AddFloorData(layout, "B05. Faded Trail.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Clear);


                            if (ii > 4)
                                AddWaterSteps(layout, 3, new RandRange(30));//water
                            else
                                AddWaterSteps(layout, 3, new RandRange(40));//water


                            //Tilesets
                            AddTextureData(layout, 373, 374, 375, 13);

                            //money
                            AddMoneyData(layout, new RandRange(1, 4));

                            //items
                            AddItemData(layout, new RandRange(2, 5), 25);

                            //enemies! ~ lv 5 to 10
                            AddRespawnData(layout, 5, 80);
                            AddEnemySpawnData(layout, 20, new RandRange(2, 5));

                            //traps
                            AddSingleTrapStep(layout, new RandRange(5, 7), 27);//wonder tile
                            AddTrapsSteps(layout, new RandRange(6, 9));


                            //construct paths
                            if (ii < 4)
                            {
                                AddInitGridStep(layout, 4, 3, 10, 10);

                                GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                                path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                path.RoomRatio = new RandRange(90);
                                path.BranchRatio = new RandRange(0, 25);

                                SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                                //cross
                                genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 11), new RandRange(4, 11), new RandRange(2, 6), new RandRange(2, 6)), 10);
                                //round
                                genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 10), new RandRange(5, 10)), 10);
                                path.GenericRooms = genericRooms;

                                SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                                genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                                path.GenericHalls = genericHalls;

                                layout.GenSteps.Add(PR_GRID_GEN, path);

                                layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));

                            }
                            else
                            {
                                AddInitGridStep(layout, 4, 3, 10, 10);

                                GridPathBeetle<MapGenContext> path = new GridPathBeetle<MapGenContext>();
                                path.LargeRoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                path.GiantHallGen.Add(new RoomGenBump<MapGenContext>(new RandRange(44, 60), new RandRange(4, 9), new RandRange(0, 101)), 10);
                                path.LegPercent = 80;
                                path.ConnectPercent = 80;
                                path.Vertical = true;

                                SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                                genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 9), new RandRange(4, 9), new RandRange(0, 101)), 10);
                                path.GenericRooms = genericRooms;

                                SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                                genericHalls.Add(new RoomGenAngledHall<MapGenContext>(100), 10);
                                path.GenericHalls = genericHalls;

                                layout.GenSteps.Add(PR_GRID_GEN, path);

                            }

                            AddDrawGridSteps(layout);

                            AddStairStep(layout, false);


                            if (ii == 4)
                            {

                                //vault rooms
                                {
                                    SpawnList<RoomGen<MapGenContext>> detourRooms = new SpawnList<RoomGen<MapGenContext>>();
                                    detourRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
                                    SpawnList<PermissiveRoomGen<MapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                                    detourHalls.Add(new RoomGenAngledHall<MapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                                    AddConnectedRoomsStep<MapGenContext> detours = new AddConnectedRoomsStep<MapGenContext>(detourRooms, detourHalls);
                                    detours.Amount = new RandRange(1);
                                    detours.HallPercent = 100;
                                    detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom()));
                                    detours.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.SwitchVault));
                                    detours.RoomComponents.Set(new NoConnectRoom());
                                    detours.RoomComponents.Set(new NoEventRoom());
                                    detours.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.SwitchVault));
                                    detours.HallComponents.Set(new NoConnectRoom());
                                    detours.RoomComponents.Set(new NoEventRoom());

                                    layout.GenSteps.Add(PR_ROOMS_GEN_EXTRA, detours);
                                }
                                //sealing the vault
                                {
                                    SwitchSealStep<MapGenContext> vaultStep = new SwitchSealStep<MapGenContext>(40, 41, false);
                                    vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                                    vaultStep.SwitchFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                                    vaultStep.SwitchFilters.Add(new RoomFilterComponent(true, new BossRoom()));
                                    layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                                }
                                //vault treasures
                                {
                                    BulkSpawner<MapGenContext, EffectTile> treasures = new BulkSpawner<MapGenContext, EffectTile>();

                                    EffectTile secretStairs = new EffectTile(34, true);
                                    secretStairs.TileStates.Set(new DestState(new SegLoc(1, 0)));
                                    treasures.SpecificSpawns.Add(secretStairs);

                                    RandomRoomSpawnStep<MapGenContext, EffectTile> detourItems = new RandomRoomSpawnStep<MapGenContext, EffectTile>(treasures);
                                    detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                                    layout.GenSteps.Add(PR_SPAWN_ITEMS_EXTRA, detourItems);
                                }
                            }

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }


                    {
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Hidden Trail\nB{0}F")));

                        //money
                        MoneySpawnZoneStep moneySpawnZoneStep = new MoneySpawnZoneStep(PR_RESPAWN_MONEY, new RandRange(72, 96), new RandRange(9, 12));
                        floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                        //items
                        ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                        itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;

                        //necessities
                        CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                        necessities.SpawnRates.SetRange(14, new IntRange(0, 3));
                        itemSpawnZoneStep.Spawns.Add("necessities", necessities);

                        necessities.Spawns.Add(new InvItem(11), new IntRange(0, 3), 9);//Leppa
                        necessities.Spawns.Add(new InvItem(10), new IntRange(0, 3), 12);//Oran
                        necessities.Spawns.Add(new InvItem(1), new IntRange(0, 3), 10);//Apple
                        necessities.Spawns.Add(new InvItem(12), new IntRange(0, 3), 10);//Lum
                        necessities.Spawns.Add(new InvItem(101), new IntRange(0, 3), 5);//reviver seed

                        //snacks
                        CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                        snacks.SpawnRates.SetRange(10, new IntRange(0, 3));
                        itemSpawnZoneStep.Spawns.Add("snacks", snacks);

                        snacks.Spawns.Add(new InvItem(112), new IntRange(0, 3), 20);//blast seed
                        snacks.Spawns.Add(new InvItem(108), new IntRange(0, 3), 10);//warp seed
                        snacks.Spawns.Add(new InvItem(110), new IntRange(0, 3), 10);//sleep seed

                        //wands
                        CategorySpawn<InvItem> ammo = new CategorySpawn<InvItem>();
                        ammo.SpawnRates.SetRange(10, new IntRange(0, 3));
                        itemSpawnZoneStep.Spawns.Add("ammo", ammo);

                        ammo.Spawns.Add(new InvItem(200, false, 3), new IntRange(0, 3), 10);//stick
                        ammo.Spawns.Add(new InvItem(222, false, 2), new IntRange(0, 3), 10);//whirlwind wand
                        ammo.Spawns.Add(new InvItem(221, false, 3), new IntRange(0, 3), 10);//pounce wand
                        ammo.Spawns.Add(new InvItem(232, false, 1), new IntRange(0, 3), 10);//warp wand
                        ammo.Spawns.Add(new InvItem(234, false, 2), new IntRange(0, 3), 10);//lob wand
                        ammo.Spawns.Add(new InvItem(207, false, 2), new IntRange(0, 3), 10);//Geo Pebble

                        //orbs
                        CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                        orbs.SpawnRates.SetRange(10, new IntRange(0, 3));
                        itemSpawnZoneStep.Spawns.Add("orbs", orbs);

                        orbs.Spawns.Add(new InvItem(283), new IntRange(0, 3), 10);//Rebound
                        orbs.Spawns.Add(new InvItem(268), new IntRange(0, 3), 5);//All Protect
                        orbs.Spawns.Add(new InvItem(253), new IntRange(0, 3), 9);//Luminous
                        orbs.Spawns.Add(new InvItem(284), new IntRange(0, 3), 8);//Mirror Orb

                        //held items
                        CategorySpawn<InvItem> heldItems = new CategorySpawn<InvItem>();
                        heldItems.SpawnRates.SetRange(2, new IntRange(0, 3));
                        itemSpawnZoneStep.Spawns.Add("held", heldItems);

                        heldItems.Spawns.Add(new InvItem(400), new IntRange(0, 3), 1);//Power Band
                        heldItems.Spawns.Add(new InvItem(401), new IntRange(0, 3), 1);//Special Band
                        heldItems.Spawns.Add(new InvItem(402), new IntRange(0, 3), 1);//Defense Scarf

                        //special
                        CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                        special.SpawnRates.SetRange(7, new IntRange(0, 3));
                        itemSpawnZoneStep.Spawns.Add("special", special);

                        int rate = 2;
                        special.Spawns.Add(new InvItem(213), new IntRange(0, 3), rate);//brown apricorns
                        special.Spawns.Add(new InvItem(216), new IntRange(0, 3), rate);//white apricorns

                        floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                        //mobs
                        TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                        poolSpawn.Priority = PR_RESPAWN_MOB;

                        //need one super-effective for each possible starter
                        //403 Shinx : 033 Tackle : 43 Leer
                        poolSpawn.SpecificSpawns.Add(new SpecificTeamSpawner(GetGenericMob(403, -1, 33, 43, -1, -1, new RandRange(8), 16), GetGenericMob(403, -1, 33, 43, -1, -1, new RandRange(6), 16)), new IntRange(0, 3), 10);
                        //190 Aipom : 010 Scratch : Sand Attack
                        poolSpawn.SpecificSpawns.Add(new SpecificTeamSpawner(GetGenericMob(190, -1, 10, 28, -1, -1, new RandRange(8), 16), GetGenericMob(190, -1, 10, 28, -1, -1, new RandRange(8), 16)), new IntRange(0, 3), 10);
                        //161 Sentret : 10 Scratch : 111 Defense Curl
                        poolSpawn.Spawns.Add(GetTeamMob(161, -1, 10, 111, -1, -1, new RandRange(10), 16), new IntRange(0, 3), 10);
                        //396 Starly : 33 Tackle : 45 Growl - later pairs
                        poolSpawn.SpecificSpawns.Add(new SpecificTeamSpawner(GetGenericMob(396, -1, 33, 45, -1, -1, new RandRange(7), 16), GetGenericMob(396, -1, 33, 45, -1, -1, new RandRange(7), 16)), new IntRange(0, 3), 20);
                        //438 Bonsly : 88 Rock Throw
                        poolSpawn.Spawns.Add(GetTeamMob(438, -1, 88, -1, -1, -1, new RandRange(9), 10), new IntRange(0, 3), 8);

                        poolSpawn.TeamSizes.Add(1, new IntRange(0, 3), 12);
                        floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    floorSegment.ZoneSteps.Add(tileSpawn);


                        for (int ii = 0; ii < 3; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            AddFloorData(layout, "B02. Demonstration 2.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                            AddWaterSteps(layout, 0, new RandRange(20));//empty


                            //Tilesets
                            AddTextureData(layout, 99, 100, 101, 13);

                            //money
                            AddMoneyData(layout, new RandRange(1, 4));

                            //items
                            AddItemData(layout, new RandRange(2, 5), 25);

                            //enemies! ~ lv 5 to 10
                            AddRespawnData(layout, 4, 80);
                            AddEnemySpawnData(layout, 20, new RandRange(2, 4));

                            //traps
                            AddSingleTrapStep(layout, new RandRange(5, 8), 27);//wonder tile
                            AddTrapsSteps(layout, new RandRange(6, 9));

                            //construct paths
                            {
                                AddInitGridStep(layout, 4, 3, 8, 8);

                                GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                                path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                path.RoomRatio = new RandRange(90);
                                path.BranchRatio = new RandRange(0, 25);

                                SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                                //cross
                                genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(2, 7), new RandRange(2, 7), new RandRange(2, 6), new RandRange(2, 6)), 10);
                                //round
                                genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8)), 10);
                                path.GenericRooms = genericRooms;

                                SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                                genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                                path.GenericHalls = genericHalls;

                                layout.GenSteps.Add(PR_GRID_GEN, path);

                                layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));

                            }

                            AddDrawGridSteps(layout);

                            AddStairStep(layout, false);

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 10)
            {
                #region FAULTED CLIFFS
                {
                    zone.Name = new LocalText("Faulted Cliffs");
                    zone.Rescues = 2;
                    zone.Level = 15;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Faulted Cliffs\n{0}F")));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = new MoneySpawnZoneStep(PR_RESPAWN_MONEY, new RandRange(30, 42), new RandRange(10, 14));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;

                    //necessities
                    CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                    necessities.SpawnRates.SetRange(10, new IntRange(0, 10));
                    itemSpawnZoneStep.Spawns.Add("necessities", necessities);

                    necessities.Spawns.Add(new InvItem(11), new IntRange(0, 10), 9);//Leppa
                    necessities.Spawns.Add(new InvItem(10), new IntRange(0, 10), 12);//Oran
                    necessities.Spawns.Add(new InvItem(1), new IntRange(0, 10), 10);//Apple
                    necessities.Spawns.Add(new InvItem(12), new IntRange(0, 10), 10);//Lum
                    necessities.Spawns.Add(new InvItem(101), new IntRange(0, 10), 5);//reviver seed

                    //snacks
                    CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                    snacks.SpawnRates.SetRange(10, new IntRange(0, 10));
                    itemSpawnZoneStep.Spawns.Add("snacks", snacks);

                    snacks.Spawns.Add(new InvItem(112), new IntRange(0, 10), 20);//blast seed
                    snacks.Spawns.Add(new InvItem(108), new IntRange(0, 10), 10);//warp seed
                    snacks.Spawns.Add(new InvItem(110), new IntRange(0, 10), 10);//sleep seed

                    //wands
                    CategorySpawn<InvItem> ammo = new CategorySpawn<InvItem>();
                    ammo.SpawnRates.SetRange(16, new IntRange(0, 10));
                    itemSpawnZoneStep.Spawns.Add("ammo", ammo);

                    ammo.Spawns.Add(new InvItem(200, false, 3), new IntRange(0, 10), 10);//stick
                    ammo.Spawns.Add(new InvItem(222, false, 2), new IntRange(0, 10), 10);//whirlwind wand
                    ammo.Spawns.Add(new InvItem(221, false, 3), new IntRange(0, 10), 10);//pounce wand
                    ammo.Spawns.Add(new InvItem(232, false, 1), new IntRange(0, 10), 10);//warp wand
                    ammo.Spawns.Add(new InvItem(220, false, 2), new IntRange(0, 10), 16);//path wand
                    ammo.Spawns.Add(new InvItem(228, false, 3), new IntRange(0, 10), 10);//fear wand
                    ammo.Spawns.Add(new InvItem(207, false, 2), new IntRange(0, 10), 16);//Geo Pebble

                    //orbs
                    CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                    orbs.SpawnRates.SetRange(10, new IntRange(0, 10));
                    itemSpawnZoneStep.Spawns.Add("orbs", orbs);

                    orbs.Spawns.Add(new InvItem(283), new IntRange(0, 10), 10);//Rebound
                    orbs.Spawns.Add(new InvItem(268), new IntRange(0, 10), 5);//All Protect
                    orbs.Spawns.Add(new InvItem(258), new IntRange(0, 10), 9);//All-Aim
                    orbs.Spawns.Add(new InvItem(269), new IntRange(0, 10), 8);//Trap-See
                    orbs.Spawns.Add(new InvItem(270), new IntRange(0, 10), 8);//Trapbust

                    //held items
                    CategorySpawn<InvItem> heldItems = new CategorySpawn<InvItem>();
                    heldItems.SpawnRates.SetRange(6, new IntRange(0, 10));
                    itemSpawnZoneStep.Spawns.Add("held", heldItems);

                    heldItems.Spawns.Add(new InvItem(400), new IntRange(0, 10), 1);//Power Band
                    heldItems.Spawns.Add(new InvItem(402), new IntRange(0, 10), 1);//Defense Scarf
                    heldItems.Spawns.Add(new InvItem(336), new IntRange(0, 10), 1);//Black Belt
                    heldItems.Spawns.Add(new InvItem(346), new IntRange(0, 10), 1);//Hard Stone
                    heldItems.Spawns.Add(new InvItem(313), new IntRange(0, 10), 2);//Scope Lens
                    heldItems.Spawns.Add(new InvItem(314), new IntRange(0, 10), 2);//Wide Lens

                    //special
                    CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                    special.SpawnRates.SetRange(7, new IntRange(0, 10));
                    itemSpawnZoneStep.Spawns.Add("special", special);

                    int rate = 2;
                    special.Spawns.Add(new InvItem(213), new IntRange(0, 10), rate);//brown apricorns
                    special.Spawns.Add(new InvItem(216), new IntRange(0, 10), rate);//white apricorns

                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;

                    // 74 Geodude : 222 Magnitude : 479 Smack Down
                    poolSpawn.Spawns.Add(GetTeamMob(74, -1, 222, 479, -1, -1, new RandRange(12), 16), new IntRange(0, 3), 10);
                    poolSpawn.Spawns.Add(GetTeamMob(74, -1, 222, 479, -1, -1, new RandRange(16), 16), new IntRange(3, 10), 10);
                    // 299 Nosepass : 88 Rock Throw : 86 Thunder Wave
                    poolSpawn.Spawns.Add(GetTeamMob(299, -1, 88, 86, -1, -1, new RandRange(15), 16), new IntRange(6, 10), 10);
                    // 231 Phanpy : 205 Rollout : 175 Flail
                    poolSpawn.Spawns.Add(GetTeamMob(231, -1, 205, 175, -1, -1, new RandRange(12), 16), new IntRange(0, 10), 10);
                    // 447 Riolu : 203 Endure : 98 Quick Attack 
                    poolSpawn.Spawns.Add(GetTeamMob(447, -1, 203, 98, -1, -1, new RandRange(14), 16), new IntRange(3, 6), 10);
                    //296  Makuhita : 292 Arm Thrust : 252 Fake Out 
                    poolSpawn.Spawns.Add(GetTeamMob(296, -1, 292, 252, -1, -1, new RandRange(12), 16), new IntRange(0, 6), 10);
                    // 207 Gligar : 98 Quick Attack : 282 Knock Off
                    poolSpawn.Spawns.Add(GetTeamMob(207, -1, 98, 282, -1, -1, new RandRange(14), 16), new IntRange(0, 6), 10);

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, 10), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);


                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    tileSpawn.Spawns.Add(new EffectTile(17, false), new IntRange(0, 10), 10);//chestnut trap
                    tileSpawn.Spawns.Add(new EffectTile(23, false), new IntRange(0, 10), 10);//trip trap
                    floorSegment.ZoneSteps.Add(tileSpawn);


                    for (int ii = 0; ii < 10; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        if (ii < 6)
                            AddFloorData(layout, "B18. Faulted Cliffs.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Clear);
                        else
                            AddFloorData(layout, "B18. Faulted Cliffs.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                        if (ii < 6)
                            AddWaterSteps(layout, 3, new RandRange(30));//water

                        //Tilesets
                        if (ii < 6)
                            AddTextureData(layout, 204, 205, 206, 16);
                        else
                            AddTextureData(layout, 222, 223, 224, 16);

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), 27);//wonder tile
                        if (ii < 6)
                            AddTrapsSteps(layout, new RandRange(12, 16));
                        else
                            AddTrapsSteps(layout, new RandRange(36, 42), true);

                        //money - 750P to 3,150P
                        AddMoneyData(layout, new RandRange(2, 4));

                        //enemies! ~ lv 15
                        if (ii < 6)
                        {
                            AddRespawnData(layout, 5, 80);
                            AddEnemySpawnData(layout, 20, new RandRange(3, 5));
                        }
                        else
                        {
                            AddRespawnData(layout, 6, 80);
                            AddEnemySpawnData(layout, 20, new RandRange(4, 6));
                        }

                        //items
                        if (ii < 6)
                            AddItemData(layout, new RandRange(3, 6), 25);
                        else
                            AddItemData(layout, new RandRange(5, 7), 25, true);


                        //construct paths
                        if (ii < 6)
                        {
                            AddInitGridStep(layout, 5, 3, 13, 8);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(90);
                            path.BranchRatio = new RandRange(0, 25);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(6, 13), new RandRange(3, 7), new RandRange(2, 4), new RandRange(2, 6)), 10);
                            //round
                            genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 9), new RandRange(3, 7)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));

                        }
                        else
                        {
                            AddInitGridStep(layout, 5, 3, 13, 8);

                            GridPathTwoSides<MapGenContext> path = new GridPathTwoSides<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.GapAxis = Axis4.Horiz;

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(6, 13), new RandRange(3, 7), new RandRange(2, 4), new RandRange(2, 6)), 10);
                            //round
                            genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 9), new RandRange(3, 7)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(0, new SquareHallBrush(new Loc(1, 2))), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(100, 50));
                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, false);


                        if (ii >= 6)
                        {
                            //the disconnected rooms
                            {
                                AddDisconnectedRoomsStep<MapGenContext> addDisconnect = new AddDisconnectedRoomsStep<MapGenContext>();
                                addDisconnect.Components.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Disconnected));
                                addDisconnect.Components.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Disconnected));
                                addDisconnect.Amount = new RandRange(1, 4);

                                //Give it some room types to place
                                SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                                //only one tile size
                                genericRooms.Add(new RoomGenDefault<MapGenContext>(), 10);

                                addDisconnect.GenericRooms = genericRooms;

                                layout.GenSteps.Add(PR_ROOMS_GEN_EXTRA, addDisconnect);
                            }

                            //the secret mon
                            {
                                // 213 Shuckle : 522 Struggle Bug : 110 Withdraw : 117 Bide
                                SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                                MobSpawn mob = GetGenericMob(213, -1, 522, 110, 117, -1, new RandRange(12), 16);
                                mob.SpawnFeatures.Add(new MobSpawnItem(true, 10, 10, 72, 80));
                                specificTeam.Spawns.Add(mob);


                                LoopedTeamSpawner<MapGenContext> spawner = new LoopedTeamSpawner<MapGenContext>(specificTeam);
                                {
                                    spawner.AmountSpawner = new RandRange(1, 4);
                                }
                                PlaceDisconnectedMobsStep<MapGenContext> secretMobPlacement = new PlaceDisconnectedMobsStep<MapGenContext>(spawner);
                                secretMobPlacement.AcceptedTiles.Add(new Tile(0));
                                layout.GenSteps.Add(PR_SPAWN_MOBS, secretMobPlacement);
                            }
                        }

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
                #endregion
            }

            //DataStringInfo.LocalizeZones(index, zone);

            if (zone.Name.DefaultText.StartsWith("**"))
                zone.Name.DefaultText = zone.Name.DefaultText.Replace("*", "");
            else if (zone.Name.DefaultText != "")
                zone.Released = true;
            return zone;
        }

    }
}
