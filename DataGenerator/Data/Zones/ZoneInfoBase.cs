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

        static void FillDebugZone(ZoneData zone)
        {
            zone.Name = new LocalText("Debug");

            {
                LayeredSegment structure = new LayeredSegment();

                //First floor: Tests traps, secret stairs, FOV, and has dummies to perform moves on.
                #region DEBUG FLOOR 1
                {
                    StairsFloorGen layout = new StairsFloorGen();

                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<StairsMapGenContext>(structure.Floors.Count, new LocalText("Debug Dungeon Main Room")));
                    AddFloorData(layout, "A07. Summit.ogg", -1, Map.SightRange.Dark, Map.SightRange.Dark);

                    InitTilesStep<StairsMapGenContext> startStep = new InitTilesStep<StairsMapGenContext>();
                    int width = 70;
                    int height = 50;
                    startStep.Width = width + 2;
                    startStep.Height = height + 2;

                    layout.GenSteps.Add(PR_TILES_INIT, startStep);

                    SpecificTilesStep<StairsMapGenContext> drawStep = new SpecificTilesStep<StairsMapGenContext>();
                    drawStep.Offset = new Loc(1);
                    //square
                    string[] level = {
                            "#.#...#...#....#....####################..............................",
                            "#.#...#...#....#....#..#......#.......##..............................",
                            "#.#...#...#....#....#..#.####.#.......##..............................",
                            "#..........#........##.#.#.......#######..............#...............",
                            "#.....#........#..###..#.#.##.##.#######..............#...............",
                            "####...........##..###.#.#.#...#.#######..............#...............",
                            "####.#######.#####..###................#..............#...............",
                            "####.########.#.#.#..#####.#...#.#####.#........#############.........",
                            "####.#########.#.#.#..##########.#####.#..............#...............",
                            "####.##########.#.###..#######...#####.#..............#...............",
                            "####.###########.#####..##############.#..............#...............",
                            "##.....................................#..............#...............",
                            "########################################..............................",
                            "......................................................................",
                            "......................................................................",
                            ".........................................................#.#..........",
                            ".........................................................#.#..........",
                            "...........................#############.................#.#..........",
                            "............#............................................#.#..........",
                            "...........................#############.................#.#..........",
                            "......................................................................",
                            "..................................................##..................",
                            "..................................................##..................",
                            "......................................................................",
                            "......................................................................",
                        };

                    drawStep.Tiles = new Tile[width][];
                    for (int ii = 0; ii < width; ii++)
                    {
                        drawStep.Tiles[ii] = new Tile[height];
                        for (int jj = 0; jj < height; jj++)
                        {
                            if (jj < height - 25)
                                drawStep.Tiles[ii][jj] = new Tile("floor");
                            else
                            {
                                if (level[jj - (height - 25)][ii] == '#')
                                    drawStep.Tiles[ii][jj] = new Tile("wall");
                                else
                                    drawStep.Tiles[ii][jj] = new Tile("floor");
                            }
                        }
                    }

                    layout.GenSteps.Add(PR_TILES_GEN, drawStep);

                    //add border
                    layout.GenSteps.Add(PR_TILES_BARRIER, new UnbreakableBorderStep<StairsMapGenContext>(1));

                    //static exits
                    {
                        List<(MapGenEntrance, Loc)> entrances = new List<(MapGenEntrance, Loc)>();
                        entrances.Add((new MapGenEntrance(Dir8.Down), new Loc(28, 2)));
                        AddSpecificSpawn(layout, entrances, PR_EXITS);
                    }
                    {
                        List<(MapGenExit, Loc)> exits = new List<(MapGenExit, Loc)>();
                        exits.Add((new MapGenExit(new EffectTile("stairs_go_up", true)), new Loc(30, 2)));
                        AddSpecificSpawn(layout, exits, PR_EXITS);
                    }
                    {
                        List<(MapGenExit, Loc)> exits = new List<(MapGenExit, Loc)>();
                        EffectTile secretStairs = new EffectTile("stairs_exit_up", true);
                        secretStairs.TileStates.Set(new DestState(SegLoc.Invalid));
                        exits.Add((new MapGenExit(secretStairs), new Loc(31, 2)));
                        AddSpecificSpawn(layout, exits, PR_EXITS);
                    }
                    {
                        List<(MapGenExit, Loc)> exits = new List<(MapGenExit, Loc)>();
                        EffectTile secretStairs = new EffectTile("stairs_secret_up", true);
                        secretStairs.TileStates.Set(new DestState(new SegLoc(1, 0), true));
                        exits.Add((new MapGenExit(secretStairs), new Loc(32, 2)));
                        AddSpecificSpawn(layout, exits, PR_EXITS);
                    }
                    {
                        List<(MapGenExit, Loc)> exits = new List<(MapGenExit, Loc)>();
                        EffectTile secretStairs = new EffectTile("stairs_secret_up", true);
                        secretStairs.TileStates.Set(new DestState(new SegLoc(2, 0), true));
                        exits.Add((new MapGenExit(secretStairs), new Loc(33, 2)));
                        AddSpecificSpawn(layout, exits, PR_EXITS);
                    }
                    {
                        List<(MapGenExit, Loc)> exits = new List<(MapGenExit, Loc)>();
                        EffectTile secretStairs = new EffectTile("stairs_secret_up", true);
                        secretStairs.TileStates.Set(new DestState(new SegLoc(3, 0), true));
                        exits.Add((new MapGenExit(secretStairs), new Loc(34, 2)));
                        AddSpecificSpawn(layout, exits, PR_EXITS);
                    }
                    {
                        List<(MapGenExit, Loc)> exits = new List<(MapGenExit, Loc)>();
                        EffectTile secretStairs = new EffectTile("stairs_secret_up", true);
                        secretStairs.TileStates.Set(new DestState(new SegLoc(4, 0), true));
                        exits.Add((new MapGenExit(secretStairs), new Loc(35, 2)));
                        AddSpecificSpawn(layout, exits, PR_EXITS);
                    }
                    //{
                    //    List<(MapGenExit, Loc)> exits = new List<(MapGenExit, Loc)>();
                    //    EffectTile secretStairs = new EffectTile("area_script", true);
                    //    secretStairs.TileStates.Set(new TileScriptState("Test", "{}"));
                    //    exits.Add((new MapGenExit(secretStairs), new Loc(36, 2)));
                    //    AddSpecificSpawn(layout, exits, PR_EXITS);
                    //}

                    layout.GenSteps.Add(PR_EXITS, new StairsStep<StairsMapGenContext, MapGenEntrance, MapGenExit>(new MapGenEntrance(Dir8.Down), new MapGenExit(new EffectTile("stairs_go_up", true))));

                    List<string> monsterKeys = DataManager.Instance.DataIndices[DataManager.DataType.Monster].GetOrderedKeys(true);
                    for (int ii = 0; ii < 20; ii++)
                    {
                        MobSpawn post_mob = new MobSpawn();
                        post_mob.BaseForm = new MonsterID(monsterKeys[ii + 1], 0, "normal", Gender.Unknown);
                        post_mob.Tactic = "wait_only";
                        post_mob.Level = new RandRange(50);
                        post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc((ii % 5) * 2 + 2, ii / 5 * 2 + 2)));
                        post_mob.SpawnFeatures.Add(new MobSpawnItem(true, 1));
                        SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);

                        PlaceNoLocMobsStep<StairsMapGenContext> mobStep = new PlaceNoLocMobsStep<StairsMapGenContext>(new PresetMultiTeamSpawner<StairsMapGenContext>(post_team));
                        layout.GenSteps.Add(PR_SPAWN_MOBS, mobStep);
                    }

                    {
                        MobSpawn post_mob = new MobSpawn();
                        post_mob.BaseForm = new MonsterID("ditto", 0, "normal", Gender.Unknown);
                        post_mob.Tactic = "wait_only";
                        post_mob.Level = new RandRange(50);
                        post_mob.SpawnFeatures.Add(new MobSpawnUnrecruitable());
                        post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc(24, 2)));
                        post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_WAIT_0190"))));
                        SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);
                        post_team.Explorer = true;

                        PlaceNoLocMobsStep<StairsMapGenContext> mobStep = new PlaceNoLocMobsStep<StairsMapGenContext>(new PresetMultiTeamSpawner<StairsMapGenContext>(post_team));
                        mobStep.Ally = true;
                        layout.GenSteps.Add(PR_SPAWN_MOBS, mobStep);
                    }

                    SpawnList<EffectTile> effectTileSpawns = new SpawnList<EffectTile>();
                    effectTileSpawns.Add(new EffectTile("trap_mud", true), 10);//mud trap
                    effectTileSpawns.Add(new EffectTile("trap_warp", true), 10);//warp trap
                    effectTileSpawns.Add(new EffectTile("trap_gust", true), 10);//gust trap
                    effectTileSpawns.Add(new EffectTile("trap_chestnut", true), 10);//chestnut trap
                    effectTileSpawns.Add(new EffectTile("trap_poison", true), 10);//poison trap
                    effectTileSpawns.Add(new EffectTile("trap_slumber", true), 10);//sleep trap
                    effectTileSpawns.Add(new EffectTile("trap_sticky", true), 10);//sticky trap
                    effectTileSpawns.Add(new EffectTile("trap_seal", true), 10);//seal trap
                    effectTileSpawns.Add(new EffectTile("trap_self_destruct", true), 10);//selfdestruct trap
                    effectTileSpawns.Add(new EffectTile("trap_trip", true), 10);//trip trap
                    effectTileSpawns.Add(new EffectTile("trap_trip", true), 10);//trip trap
                    effectTileSpawns.Add(new EffectTile("trap_hunger", true), 10);//hunger trap
                    effectTileSpawns.Add(new EffectTile("trap_apple", true), 3);//apple trap
                    effectTileSpawns.Add(new EffectTile("trap_apple", true), 3);//apple trap
                    effectTileSpawns.Add(new EffectTile("trap_pp_leech", true), 10);//pp-leech trap
                    effectTileSpawns.Add(new EffectTile("trap_summon", true), 10);//summon trap
                    effectTileSpawns.Add(new EffectTile("trap_explosion", true), 10);//explosion trap
                    effectTileSpawns.Add(new EffectTile("trap_slow", true), 10);//slow trap
                    effectTileSpawns.Add(new EffectTile("trap_spin", true), 10);//spin trap
                    effectTileSpawns.Add(new EffectTile("trap_grimy", true), 10);//grimy trap
                    effectTileSpawns.Add(new EffectTile("trap_trigger", true), 20);//trigger trap
                    effectTileSpawns.Add(new EffectTile("trap_grudge", true), 10);//grudge trap

                    RandomSpawnStep<StairsMapGenContext, EffectTile> trapStep = new RandomSpawnStep<StairsMapGenContext, EffectTile>(new PickerSpawner<StairsMapGenContext, EffectTile>(new LoopedRand<EffectTile>(effectTileSpawns, new RandRange(300))));
                    layout.GenSteps.Add(PR_SPAWN_TRAPS, trapStep);

                    List<(InvItem, Loc)> items = new List<(InvItem, Loc)>();
                    items.Add((new InvItem(12), new Loc(7, 32)));//Lum Berry
                    items.Add((new InvItem(12), new Loc(8, 33)));//Lum Berry
                    AddSpecificSpawn(layout, items, PR_SPAWN_ITEMS);

                    //Tilesets
                    AddTextureData(layout, "test_dungeon_wall", "test_dungeon_floor", "test_dungeon_secondary", "none");

                    structure.Floors.Add(layout);
                }
                #endregion


                //Second floor.  Tests Monster Houses, Locked Passages, and Beetle-shaped rooms
                #region DEBUG FLOOR 2
                {
                    GridFloorGen layout = new GridFloorGen();

                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(structure.Floors.Count, new LocalText("Test: Enemies")));
                    AddFloorData(layout, "A07. Summit.ogg", 500, Map.SightRange.Clear, Map.SightRange.Clear);
                    AddDefaultMapStatus(layout, "default_weather", "rain");

                    AddInitGridStep(layout, 4, 4, 9, 9);

                    //construct paths
                    GridPathBeetle<MapGenContext> path = new GridPathBeetle<MapGenContext>();
                    path.LargeRoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.GiantHallGen.Add(new RoomGenBump<MapGenContext>(new RandRange(44, 60), new RandRange(4, 9), new RandRange(0, 101)), 10);
                    path.LegPercent = 80;
                    path.ConnectPercent = 80;
                    path.Vertical = true;

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8), new RandRange(0, 101)), 10);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(100), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    //vault rooms
                    {
                        SpawnList<RoomGen<MapGenContext>> detourRooms = new SpawnList<RoomGen<MapGenContext>>();
                        detourRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
                        SpawnList<PermissiveRoomGen<MapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        detourHalls.Add(new RoomGenAngledHall<MapGenContext>(0, new RandRange(2, 6), new RandRange(2, 6)), 10);
                        AddConnectedRoomsStep<MapGenContext> detours = new AddConnectedRoomsStep<MapGenContext>(detourRooms, detourHalls);
                        detours.Amount = new RandRange(1);
                        detours.HallPercent = 100;
                        detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom()));
                        detours.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.KeyVault));
                        detours.RoomComponents.Set(new NoConnectRoom());
                        detours.RoomComponents.Set(new NoEventRoom());
                        detours.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.KeyVault));
                        detours.HallComponents.Set(new NoConnectRoom());
                        detours.RoomComponents.Set(new NoEventRoom());

                        layout.GenSteps.Add(PR_ROOMS_GEN_EXTRA, detours);
                    }

                    AddDrawGridSteps(layout);

                    //sealing the vault
                    {
                        KeySealStep<MapGenContext> vaultStep = new KeySealStep<MapGenContext>("sealed_block", "sealed_door", 455);
                        vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                        layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                    }

                    AddStairStep(layout, false);

                    //items
                    ItemSpawnStep<MapGenContext> itemSpawnZoneStep = new ItemSpawnStep<MapGenContext>();
                    SpawnList<InvItem> items = new SpawnList<InvItem>();
                    items.Add(new InvItem(11), 12);
                    itemSpawnZoneStep.Spawns.Add("uncategorized", items, 10);
                    layout.GenSteps.Add(PR_RESPAWN_ITEM, itemSpawnZoneStep);

                    //enemies!
                    AddRespawnData(layout, 200, 1);

                    MobSpawnStep<MapGenContext> spawnStep = new MobSpawnStep<MapGenContext>();
                    PoolTeamSpawner poolSpawn = new PoolTeamSpawner();
                    poolSpawn.Spawns.Add(GetTeamMob("castform", "", "", "", "", "", new RandRange(18), "wander_normal", true), 10);
                    poolSpawn.TeamSizes.Add(1, 12);
                    spawnStep.Spawns.Add(poolSpawn, 100);
                    layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);

                    AddEnemySpawnData(layout, 20, new RandRange(200));

                    //Tilesets
                    AddTextureData(layout, "test_dungeon_wall", "test_dungeon_floor", "test_dungeon_secondary", "none");

                    //monsterhouse
                    {
                        MonsterHouseStep<MapGenContext> monsterHouse = new MonsterHouseStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                        for (int ii = 250; ii < 290; ii++)
                            monsterHouse.Items.Add(new MapItem(ii), 10);
                        monsterHouse.ItemThemes.Add(new ItemThemeNone(50, new RandRange(5, 10)), 10);
                        List<string> monsterKeys = DataManager.Instance.DataIndices[DataManager.DataType.Monster].GetOrderedKeys(true);
                        for (int ii = 387; ii < 397; ii++)
                            monsterHouse.Mobs.Add(GetGenericMob(monsterKeys[ii], "", "", "", "", "", new RandRange(10, 20)), 10);
                        monsterHouse.MobThemes.Add(new MobThemeNone(50, new RandRange(6, 11)), 10);
                        layout.GenSteps.Add(PR_HOUSES, monsterHouse);
                    }

                    // items for the vault
                    {
                        BulkSpawner<MapGenContext, InvItem> treasures = new BulkSpawner<MapGenContext, InvItem>();
                        treasures.SpecificSpawns.Add(new InvItem(51));
                        treasures.SpecificSpawns.Add(new InvItem(51));
                        RandomRoomSpawnStep<MapGenContext, InvItem> detourItems = new RandomRoomSpawnStep<MapGenContext, InvItem>(treasures);
                        detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                        layout.GenSteps.Add(PR_SPAWN_ITEMS_EXTRA, detourItems);
                    }

                    //money for the vault
                    {
                        BulkSpawner<MapGenContext, MoneySpawn> treasures = new BulkSpawner<MapGenContext, MoneySpawn>();
                        treasures.SpecificSpawns.Add(new MoneySpawn(500));
                        treasures.SpecificSpawns.Add(new MoneySpawn(400));
                        treasures.SpecificSpawns.Add(new MoneySpawn(300));
                        RandomRoomSpawnStep<MapGenContext, MoneySpawn> detourItems = new RandomRoomSpawnStep<MapGenContext, MoneySpawn>(treasures);
                        detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                        layout.GenSteps.Add(PR_SPAWN_ITEMS_EXTRA, detourItems);
                    }

                    {
                        BulkSpawner<MapGenContext, EffectTile> treasures = new BulkSpawner<MapGenContext, EffectTile>();
                        EffectTile secretStairs = new EffectTile("stairs_secret_up", true);
                        secretStairs.TileStates.Set(new DestState(new SegLoc(0, 0)));
                        treasures.SpecificSpawns.Add(secretStairs);
                        RandomRoomSpawnStep<MapGenContext, EffectTile> detourTiles = new RandomRoomSpawnStep<MapGenContext, EffectTile>(treasures);
                        detourTiles.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                        layout.GenSteps.Add(PR_SPAWN_ITEMS_EXTRA, detourTiles);
                    }

                    structure.Floors.Add(layout);
                }
                #endregion

                //Contains a switch-activated vault, chest, and a boss vault
                #region DEBUG FLOOR 3
                {
                    GridFloorGen layout = new GridFloorGen();

                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(structure.Floors.Count, new LocalText("Test: Vault Shop Boss")));
                    AddFloorData(layout, "A07. Summit.ogg", 500, Map.SightRange.Dark, Map.SightRange.Dark);

                    AddInitGridStep(layout, 8, 8, 8, 8);

                    //construct paths
                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(15);

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //round
                    genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(6, 8), new RandRange(6, 8)), 10);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(0), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);


                    AddDrawGridSteps(layout);

                    AddStairStep(layout, false);

                    //items
                    ItemSpawnStep<MapGenContext> itemSpawnZoneStep = new ItemSpawnStep<MapGenContext>();
                    SpawnList<InvItem> items = new SpawnList<InvItem>();
                    items.Add(new InvItem(11), 12);
                    itemSpawnZoneStep.Spawns.Add("uncategorized", items, 10);
                    layout.GenSteps.Add(PR_RESPAWN_ITEM, itemSpawnZoneStep);

                    //enemies!
                    AddRespawnData(layout, 3, 80);

                    MobSpawnStep<MapGenContext> spawnStep = new MobSpawnStep<MapGenContext>();
                    PoolTeamSpawner poolSpawn = new PoolTeamSpawner();
                    poolSpawn.Spawns.Add(GetTeamMob("chatot", "", "echoed_voice", "feather_dance", "", "", new RandRange(18)), 10);
                    poolSpawn.TeamSizes.Add(1, 12);
                    spawnStep.Spawns.Add(poolSpawn, 100);
                    layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);

                    //Tilesets
                    AddTextureData(layout, "sky_peak_4th_pass_wall", "sky_peak_4th_pass_floor", "sky_peak_4th_pass_secondary", "normal");

                    // neutral NPC
                    {
                        SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                        specificTeam.Explorer = true;
                        MobSpawn post_mob = new MobSpawn();
                        post_mob.BaseForm = new MonsterID("smeargle", 0, "normal", Gender.Unknown);
                        post_mob.Tactic = "slow_wander";
                        post_mob.Level = new RandRange(50);
                        post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_WAIT_0190"))));
                        specificTeam.Spawns.Add(post_mob);

                        PlaceRandomMobsStep<MapGenContext> secretMobPlacement = new PlaceRandomMobsStep<MapGenContext>(new PresetMultiTeamSpawner<MapGenContext>(specificTeam));
                        secretMobPlacement.Ally = true;
                        secretMobPlacement.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        secretMobPlacement.ClumpFactor = 20;
                        layout.GenSteps.Add(PR_SPAWN_MOBS_EXTRA, secretMobPlacement);
                    }


                    //shop
                    {
                        ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                        shop.Personality = 2;
                        shop.SecurityStatus = "shop_security";
                        shop.Items.Add(new MapItem(101, 0, 800), 10);//reviver seed
                        shop.Items.Add(new MapItem(112, 0, 500), 10);//blast seed
                        shop.Items.Add(new MapItem(444, 753, 8000), 10);//poison dust
                        shop.Items.Add(new MapItem(234, 9, 180), 10);//Lob Wand
                        shop.Items.Add(new MapItem(352, 0, 2000), 10);//thunder stone
                        shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                        // 137 Porygon : 36 Trace : 97 Agility : 59 Blizzard : 435 Discharge : 94 Psychic
                        shop.StartMob = GetShopMob("porygon", "trace", "agility", "blizzard", "discharge", "psychic", new int[] { 1322, 1323, 1324, 1325 }, 2);
                        {
                            // 474 Porygon-Z : 91 Adaptability : 247 Shadow Ball : 63 Hyper Beam : 435 Discharge : 373 Embargo
                            shop.Mobs.Add(GetShopMob("porygon_z", "adaptability", "shadow_ball", "hyper_beam", "discharge", "embargo", new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                            // 474 Porygon-Z : 91 Adaptability : 160 Conversion : 59 Blizzard : 435 Discharge : 473 Psyshock
                            shop.Mobs.Add(GetShopMob("porygon_z", "adaptability", "conversion", "blizzard", "discharge", "psyshock", new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                            // 474 Porygon-Z : 91 Adaptability : 417 Nasty Plot : 63 Hyper Beam : 435 Discharge : 373 Embargo
                            shop.Mobs.Add(GetShopMob("porygon_z", "adaptability", "nasty_plot", "hyper_beam", "discharge", "embargo", new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                            // 474 Porygon-Z : 91 Adaptability : 417 Nasty Plot : 161 Tri Attack : 247 Shadow Ball : 373 Embargo
                            shop.Mobs.Add(GetShopMob("porygon_z", "adaptability", "nasty_plot", "tri_attack", "shadow_ball", "embargo", new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                            // 474 Porygon-Z : 88 Download : 97 Agility : 473 Psyshock : 324 Signal Beam : 373 Embargo
                            shop.Mobs.Add(GetShopMob("porygon_z", "download", "agility", "psyshock", "signal_beam", "embargo", new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                            // 233 Porygon2 : 36 Trace : 176 Conversion2 : 105 Recover : 60 Psybeam : 324 Signal Beam
                            shop.Mobs.Add(GetShopMob("porygon2", "trace", "conversion_2", "recover", "psybeam", "signal_beam", new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                            // 233 Porygon2 : 36 Trace : 176 Conversion2 : 105 Recover : 60 Psybeam : 435 Discharge
                            shop.Mobs.Add(GetShopMob("porygon2", "trace", "conversion_2", "recover", "psybeam", "discharge", new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                            // 233 Porygon2 : 36 Trace : 176 Conversion2 : 277 Magic Coat : 161 Tri Attack : 97 Agility
                            shop.Mobs.Add(GetShopMob("porygon2", "trace", "conversion_2", "magic_coat", "tri_attack", "agility", new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                        }
                        layout.GenSteps.Add(PR_SHOPS, shop);
                    }

                    //chest
                    {
                        ChestStep<MapGenContext> monsterHouse = new ChestStep<MapGenContext>(false, GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                        for (int ii = 250; ii < 251; ii++)
                            monsterHouse.Items.Add(new MapItem(ii), 10);
                        monsterHouse.ItemThemes.Add(new ItemThemeNone(50, new RandRange(5, 10)), 10);
                        List<string> monsterKeys = DataManager.Instance.DataIndices[DataManager.DataType.Monster].GetOrderedKeys(true);
                        for (int ii = 387; ii < 397; ii++)
                            monsterHouse.Mobs.Add(GetGenericMob(monsterKeys[ii], "", "", "", "", "", new RandRange(10, 20)), 10);
                        monsterHouse.MobThemes.Add(new MobThemeNone(50, new RandRange(6, 11)), 10);
                        layout.GenSteps.Add(PR_HOUSES, monsterHouse);
                    }

                    //vault rooms
                    {
                        SpawnList<RoomGen<MapGenContext>> detourRooms = new SpawnList<RoomGen<MapGenContext>>();
                        detourRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
                        SpawnList<PermissiveRoomGen<MapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        detourHalls.Add(new RoomGenAngledHall<MapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                        AddConnectedRoomsStep<MapGenContext> detours = new AddConnectedRoomsStep<MapGenContext>(detourRooms, detourHalls);
                        detours.Amount = new RandRange(2, 4);
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
                        SwitchSealStep<MapGenContext> vaultStep = new SwitchSealStep<MapGenContext>("sealed_block", "tile_switch", true);
                        vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                        vaultStep.SwitchFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        vaultStep.SwitchFilters.Add(new RoomFilterComponent(true, new BossRoom()));
                        layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                    }
                    //vault treasures
                    {
                        BulkSpawner<MapGenContext, InvItem> treasures = new BulkSpawner<MapGenContext, InvItem>();
                        treasures.SpecificSpawns.Add(new InvItem(51));
                        treasures.SpecificSpawns.Add(new InvItem(51));
                        treasures.SpecificSpawns.Add(new InvItem(51));
                        RandomRoomSpawnStep<MapGenContext, InvItem> detourItems = new RandomRoomSpawnStep<MapGenContext, InvItem>(treasures);
                        detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                        layout.GenSteps.Add(PR_SPAWN_ITEMS_EXTRA, detourItems);
                    }
                    //vault guardians
                    {
                        //secret enemies
                        SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                        specificTeam.Spawns.Add(GetGenericMob("stantler", "", "", "", "", "", new RandRange(23), "loot_guard"));

                        PlaceRandomMobsStep<MapGenContext> secretMobPlacement = new PlaceRandomMobsStep<MapGenContext>(new LoopedTeamSpawner<MapGenContext>(specificTeam, new RandRange(12, 18)));
                        secretMobPlacement.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                        secretMobPlacement.ClumpFactor = 20;
                        layout.GenSteps.Add(PR_SPAWN_MOBS_EXTRA, secretMobPlacement);
                    }


                    //boss rooms
                    {
                        SpawnList<RoomGen<MapGenContext>> bossRooms = new SpawnList<RoomGen<MapGenContext>>();
                        string[] custom = new string[] {  "~~~...~~~",
                                                              "~~~...~~~",
                                                              "~~X...X~~",
                                                              ".........",
                                                              ".........",
                                                              ".........",
                                                              "~~X...X~~",
                                                              "~~~...~~~",
                                                              "~~~...~~~"};
                        List<MobSpawn> mobSpawns = new List<MobSpawn>();
                        {
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID("kyogre", 0, "normal", Gender.Unknown);
                            post_mob.Tactic = "wait_only";
                            post_mob.Level = new RandRange(50);
                            post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc(1, 4)));
                            post_mob.SpawnFeatures.Add(new MobSpawnItem(true, 1));
                            post_mob.SpawnFeatures.Add(new MobSpawnUnrecruitable());
                            mobSpawns.Add(post_mob);
                        }
                        {
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID("groudon", 0, "normal", Gender.Unknown);
                            post_mob.Tactic = "wait_only";
                            post_mob.Level = new RandRange(50);
                            post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc(7, 4)));
                            post_mob.SpawnFeatures.Add(new MobSpawnItem(true, 1));
                            post_mob.SpawnFeatures.Add(new MobSpawnUnrecruitable());
                            mobSpawns.Add(post_mob);
                        }
                        {
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID("rayquaza", 0, "normal", Gender.Unknown);
                            post_mob.Tactic = "wait_only";
                            post_mob.Level = new RandRange(50);
                            post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc(4, 1)));
                            post_mob.SpawnFeatures.Add(new MobSpawnItem(true, 1));
                            post_mob.SpawnFeatures.Add(new MobSpawnUnrecruitable());
                            mobSpawns.Add(post_mob);
                        }
                        bossRooms.Add(CreateRoomGenSpecificBoss<MapGenContext>(custom, new Loc(4, 4), mobSpawns, true), 10);
                        SpawnList<RoomGen<MapGenContext>> treasureRooms = new SpawnList<RoomGen<MapGenContext>>();
                        treasureRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
                        SpawnList<PermissiveRoomGen<MapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        detourHalls.Add(new RoomGenAngledHall<MapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                        AddBossRoomStep<MapGenContext> detours = new AddBossRoomStep<MapGenContext>(bossRooms, treasureRooms, detourHalls);
                        detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom()));
                        detours.BossComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        detours.BossComponents.Set(new NoEventRoom());
                        detours.BossComponents.Set(new BossRoom());
                        detours.BossHallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        detours.VaultComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.BossLocked));
                        detours.VaultComponents.Set(new NoConnectRoom());
                        detours.VaultComponents.Set(new NoEventRoom());
                        detours.VaultHallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.BossLocked));
                        detours.VaultHallComponents.Set(new NoConnectRoom());

                        layout.GenSteps.Add(PR_ROOMS_GEN_EXTRA, detours);
                    }
                    //sealing the boss room and treasure room
                    {
                        BossSealStep<MapGenContext> vaultStep = new BossSealStep<MapGenContext>("sealed_block", "tile_boss");
                        vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                        vaultStep.BossFilters.Add(new RoomFilterComponent(false, new BossRoom()));
                        layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                    }
                    //vault treasures
                    {
                        BulkSpawner<MapGenContext, InvItem> treasures = new BulkSpawner<MapGenContext, InvItem>();
                        treasures.SpecificSpawns.Add(new InvItem(75));
                        treasures.SpecificSpawns.Add(new InvItem(75));
                        treasures.SpecificSpawns.Add(new InvItem(75));
                        RandomRoomSpawnStep<MapGenContext, InvItem> detourItems = new RandomRoomSpawnStep<MapGenContext, InvItem>(treasures);
                        detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                        layout.GenSteps.Add(PR_SPAWN_ITEMS_EXTRA, detourItems);
                    }


                    structure.Floors.Add(layout);
                }
                #endregion

                #region Monster Hall
                {
                    GridFloorGen layout = new GridFloorGen();

                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(structure.Floors.Count, new LocalText("Test: Monster Hall")));
                    AddFloorData(layout, "B07. Flyaway Cliffs.ogg", 500, Map.SightRange.Dark, Map.SightRange.Dark);

                    AddInitGridStep(layout, 5, 3, 10, 10);

                    //construct paths
                    GridPathTwoSides<MapGenContext> path = new GridPathTwoSides<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.GapAxis = Axis4.Horiz;

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //cross
                    genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 11), new RandRange(4, 11), new RandRange(2, 6), new RandRange(2, 6)), 10);
                    //round
                    genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                    path.GenericRooms = genericRooms;
                    //blocked
                    genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(6, 10), new RandRange(6, 10), new RandRange(2, 4), new RandRange(2, 4)), 10);

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(0), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(100, 50));


                    AddDrawGridSteps(layout);

                    AddTunnelStep<MapGenContext> tunneler = new AddTunnelStep<MapGenContext>();
                    tunneler.AllowDeadEnd = true;
                    tunneler.Halls = new RandRange(30);
                    tunneler.TurnLength = new RandRange(2, 10);
                    tunneler.MaxLength = new RandRange(25);
                    layout.GenSteps.Add(PR_TILES_GEN_TUNNEL, tunneler);

                    AddStairStep(layout, false);

                    //items
                    ItemSpawnStep<MapGenContext> itemSpawnZoneStep = new ItemSpawnStep<MapGenContext>();
                    SpawnList<InvItem> items = new SpawnList<InvItem>();
                    items.Add(new InvItem(11), 12);
                    itemSpawnZoneStep.Spawns.Add("uncategorized", items, 10);
                    layout.GenSteps.Add(PR_RESPAWN_ITEM, itemSpawnZoneStep);

                    //enemies!
                    AddRespawnData(layout, 3, 80);


                    MobSpawnStep<MapGenContext> spawnStep = new MobSpawnStep<MapGenContext>();
                    PoolTeamSpawner poolSpawn = new PoolTeamSpawner();
                    poolSpawn.Spawns.Add(GetTeamMob("chatot", "", "echoed_voice", "feather_dance", "", "", new RandRange(18)), 10);
                    poolSpawn.TeamSizes.Add(1, 12);
                    spawnStep.Spawns.Add(poolSpawn, 100);
                    layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);

                    //Tilesets
                    AddTextureData(layout, "mt_travail_wall", "mt_travail_floor", "mt_travail_secondary", "ground");

                    {
                        MonsterHallStep<MapGenContext> monsterHouse = new MonsterHallStep<MapGenContext>(new Loc(12, 9), GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                        for (int ii = 250; ii < 290; ii++)
                            monsterHouse.Items.Add(new MapItem(ii), 10);
                        monsterHouse.ItemThemes.Add(new ItemThemeNone(50, new RandRange(5, 10)), 10);
                        List<string> monsterKeys = DataManager.Instance.DataIndices[DataManager.DataType.Monster].GetOrderedKeys(true);
                        for (int ii = 387; ii < 397; ii++)
                            monsterHouse.Mobs.Add(GetGenericMob(monsterKeys[ii], "", "", "", "", "", new RandRange(10, 20)), 10);
                        monsterHouse.MobThemes.Add(new MobThemeNone(50, new RandRange(18, 24)), 10);
                        layout.GenSteps.Add(PR_HOUSES, monsterHouse);
                    }

                    structure.Floors.Add(layout);
                }
                #endregion

                #region Monster Mansion
                {
                    GridFloorGen layout = new GridFloorGen();

                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(structure.Floors.Count, new LocalText("Test: Monster Mansion")));
                    AddFloorData(layout, "B07. Flyaway Cliffs.ogg", 500, Map.SightRange.Dark, Map.SightRange.Dark);

                    AddInitGridStep(layout, 1, 1, 50, 35);

                    //construct paths
                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //round
                    genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(40, 50), new RandRange(30, 35)), 10);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(0), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);


                    AddDrawGridSteps(layout);

                    AddStairStep(layout, false);


                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 5), "tile_wonder", true);//wonder tile
                    AddSingleTrapStep(layout, new RandRange(16, 19), "trap_poison", false);//poison trap

                    //items
                    ItemSpawnStep<MapGenContext> itemSpawnZoneStep = new ItemSpawnStep<MapGenContext>();
                    SpawnList<InvItem> items = new SpawnList<InvItem>();
                    items.Add(new InvItem(11), 12);
                    itemSpawnZoneStep.Spawns.Add("uncategorized", items, 10);
                    layout.GenSteps.Add(PR_RESPAWN_ITEM, itemSpawnZoneStep);

                    //enemies!
                    AddRespawnData(layout, 3, 80);

                    MobSpawnStep<MapGenContext> spawnStep = new MobSpawnStep<MapGenContext>();
                    PoolTeamSpawner poolSpawn = new PoolTeamSpawner();
                    poolSpawn.Spawns.Add(GetTeamMob("chatot", "", "echoed_voice", "feather_dance", "", "", new RandRange(18)), 10);
                    poolSpawn.TeamSizes.Add(1, 12);
                    spawnStep.Spawns.Add(poolSpawn, 100);
                    layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);

                    //Tilesets
                    AddTextureData(layout, "magma_cavern_2_wall", "magma_cavern_2_floor", "magma_cavern_2_secondary", "fire");

                    {
                        MonsterMansionStep<MapGenContext> monsterHouse = new MonsterMansionStep<MapGenContext>();
                        for (int ii = 250; ii < 290; ii++)
                            monsterHouse.Items.Add(new MapItem(ii), 10);
                        monsterHouse.ItemThemes.Add(new ItemThemeNone(50, new RandRange(12, 16)), 10);
                        List<string> monsterKeys = DataManager.Instance.DataIndices[DataManager.DataType.Monster].GetOrderedKeys(true);
                        for (int ii = 387; ii < 397; ii++)
                            monsterHouse.Mobs.Add(GetGenericMob(monsterKeys[ii], "", "", "", "", "", new RandRange(10, 20)), 10);
                        monsterHouse.MobThemes.Add(new MobThemeNone(50, new RandRange(25, 32)), 10);
                        layout.GenSteps.Add(PR_HOUSES, monsterHouse);
                    }

                    structure.Floors.Add(layout);
                }
                #endregion

                #region Monster Maze
                {
                    GridFloorGen layout = new GridFloorGen();

                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(structure.Floors.Count, new LocalText("Test: Monster Maze")));
                    AddFloorData(layout, "B07. Flyaway Cliffs.ogg", 500, Map.SightRange.Dark, Map.SightRange.Dark);

                    AddInitGridStep(layout, 9, 7, 3, 3);


                    //Create a path that is composed of a branching tree
                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(100);
                    //MODDABLE: can change branching
                    path.BranchRatio = new RandRange(50);

                    //Give it some room types to place
                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    ////square
                    genericRooms.Add(new RoomGenSquare<MapGenContext>(new RandRange(3), new RandRange(3)), 3);
                    path.GenericRooms = genericRooms;

                    //Give it some hall types to place
                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenSquare<MapGenContext>(new RandRange(1), new RandRange(1)), 20);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    //MODDABLE: can change connectivity
                    {
                        ConnectGridBranchStep<MapGenContext> step = new ConnectGridBranchStep<MapGenContext>(100);
                        step.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        step.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom()));
                        PresetPicker<PermissiveRoomGen<MapGenContext>> picker = new PresetPicker<PermissiveRoomGen<MapGenContext>>();
                        picker.ToSpawn = new RoomGenSquare<MapGenContext>(new RandRange(1), new RandRange(1));
                        step.GenericHalls = picker;
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }
                    {
                        SetGridPlanComponentStep<MapGenContext> step = new SetGridPlanComponentStep<MapGenContext>();
                        step.Components.Set(new NoEventRoom());
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                        //layout.GenSteps.Add(PR_GRID_GEN, new MarkAsHallStep<MapGenContext>());
                    }

                    AddDrawGridSteps(layout);

                    AddStairStep(layout, false);

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 5), "tile_wonder", true);//wonder tile
                    AddSingleTrapStep(layout, new RandRange(16, 19), "trap_poison", false);//poison trap

                    //items
                    ItemSpawnStep<MapGenContext> itemSpawnZoneStep = new ItemSpawnStep<MapGenContext>();
                    SpawnList<InvItem> items = new SpawnList<InvItem>();
                    items.Add(new InvItem(11), 12);
                    itemSpawnZoneStep.Spawns.Add("uncategorized", items, 10);
                    layout.GenSteps.Add(PR_RESPAWN_ITEM, itemSpawnZoneStep);

                    //enemies!
                    AddRespawnData(layout, 3, 80);

                    MobSpawnStep<MapGenContext> spawnStep = new MobSpawnStep<MapGenContext>();
                    PoolTeamSpawner poolSpawn = new PoolTeamSpawner();
                    poolSpawn.Spawns.Add(GetTeamMob("chatot", "", "echoed_voice", "feather_dance", "", "", new RandRange(18)), 10);
                    poolSpawn.TeamSizes.Add(1, 12);
                    spawnStep.Spawns.Add(poolSpawn, 100);
                    layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);


                    //Tilesets
                    AddTextureData(layout, "crystal_cave_2_wall", "crystal_cave_2_floor", "crystal_cave_2_secondary", "rock");

                    {
                        MonsterMansionStep<MapGenContext> monsterHouse = new MonsterMansionStep<MapGenContext>();
                        for (int ii = 250; ii < 290; ii++)
                            monsterHouse.Items.Add(new MapItem(ii), 10);
                        monsterHouse.ItemThemes.Add(new ItemThemeNone(50, new RandRange(12, 16)), 10);
                        List<string> monsterKeys = DataManager.Instance.DataIndices[DataManager.DataType.Monster].GetOrderedKeys(true);
                        for (int ii = 387; ii < 397; ii++)
                            monsterHouse.Mobs.Add(GetGenericMob(monsterKeys[ii], "", "", "", "", "", new RandRange(10, 20)), 10);
                        monsterHouse.MobThemes.Add(new MobThemeNone(50, new RandRange(25, 32)), 10);
                        layout.GenSteps.Add(PR_HOUSES, monsterHouse);
                    }

                    structure.Floors.Add(layout);
                }
                #endregion

                #region HEDGE MAZE 1
                {
                    GridFloorGen layout = new GridFloorGen();

                    //Floor settings
                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(structure.Floors.Count, new LocalText("Test: Hedge Maze")));
                    AddFloorData(layout,"B07. Flyaway Cliffs.ogg", 1000, Map.SightRange.Clear, Map.SightRange.Dark);
                    AddDefaultMapStatus(layout, "default_weather", "sandstorm");

                    //Tilesets
                    AddTextureData(layout, "sky_peak_4th_pass_wall", "sky_peak_4th_pass_floor", "sky_peak_4th_pass_secondary", "normal");

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 5), "tile_wonder", true);//wonder tile
                    AddSingleTrapStep(layout, new RandRange(16, 19), "trap_poison", false);//poison trap

                    SpawnList<EffectTile> trapTileSpawns = new SpawnList<EffectTile>();
                    trapTileSpawns.Add(new EffectTile("trap_poison", false), 10);
                    {
                        EffectTile secretStairs = new EffectTile("stairs_secret_up", true);
                        secretStairs.TileStates.Set(new DestState(new SegLoc(1, 0), true));
                        trapTileSpawns.Add(secretStairs, 10);
                    }
                    AddTrapListStep(layout, new RandRange(1), trapTileSpawns);

                    //money
                    MoneySpawnStep<MapGenContext> moneySpawnZoneStep = new MoneySpawnStep<MapGenContext>(new RandRange(90, 130));
                    layout.GenSteps.Add(PR_RESPAWN_MONEY, moneySpawnZoneStep);
                    AddMoneyData(layout, new RandRange(10, 14), true);

                    //items
                    ItemSpawnStep<MapGenContext> itemSpawnZoneStep = new ItemSpawnStep<MapGenContext>();
                    SpawnList<InvItem> items = new SpawnList<InvItem>();
                    items.Add(new InvItem(11), 12);
                    itemSpawnZoneStep.Spawns.Add("uncategorized", items, 10);
                    layout.GenSteps.Add(PR_RESPAWN_ITEM, itemSpawnZoneStep);

                    //enemies
                    AddRespawnData(layout, 3, 80);

                    //enemies
                    AddEnemySpawnData(layout, 20, new RandRange(2, 4));


                    MobSpawnStep<MapGenContext> spawnStep = new MobSpawnStep<MapGenContext>();
                    PoolTeamSpawner poolSpawn = new PoolTeamSpawner();
                    poolSpawn.Spawns.Add(GetTeamMob("sentret", "", "scratch", "", "", "", new RandRange(2), "wander_dumb"), 10);
                    poolSpawn.TeamSizes.Add(1, 12);
                    spawnStep.Spawns.Add(poolSpawn, 100);
                    layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);

                    //items
                    RandomSpawnStep<MapGenContext, InvItem> freeItemStep = new RandomSpawnStep<MapGenContext, InvItem>(new ContextSpawner<MapGenContext, InvItem>(new RandRange(2, 4)));
                    layout.GenSteps.Add(PR_SPAWN_ITEMS, freeItemStep);
                    AddItemData(layout, new RandRange(3, 7), 25);

                    //Initialize a grid of cells.
                    AddInitGridStep(layout, 18, 15, 2, 2);

                    //Create a path that is composed of a branching tree
                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(100);
                    //MODDABLE: can change branching
                    path.BranchRatio = new RandRange(30);

                    //Give it some room types to place
                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //square
                    genericRooms.Add(new RoomGenSquare<MapGenContext>(new RandRange(2), new RandRange(2)), 3);
                    path.GenericRooms = genericRooms;

                    //Give it some hall types to place
                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenSquare<MapGenContext>(new RandRange(1), new RandRange(1)), 20);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    //MODDABLE: can change connectivity
                    {
                        ConnectGridBranchStep<MapGenContext> step = new ConnectGridBranchStep<MapGenContext>(80);
                        step.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        step.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom()));
                        PresetPicker<PermissiveRoomGen<MapGenContext>> picker = new PresetPicker<PermissiveRoomGen<MapGenContext>>();
                        picker.ToSpawn = new RoomGenSquare<MapGenContext>(new RandRange(1), new RandRange(1));
                        step.GenericHalls = picker;
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }
                    {
                        SetGridPlanComponentStep<MapGenContext> step = new SetGridPlanComponentStep<MapGenContext>();
                        step.Components.Set(new NoEventRoom());
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                        //layout.GenSteps.Add(PR_GRID_GEN, new MarkAsHallStep<MapGenContext>());
                    }

                    //MODDABLE: use different special rooms
                    //TODO: come up with different special rooms
                    {
                        AddLargeRoomStep<MapGenContext> step = new AddLargeRoomStep<MapGenContext>(new RandRange(4, 7), GetImmutableFilterList());
                        step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        {
                            LargeRoom<MapGenContext> largeRoom = new LargeRoom<MapGenContext>(new RoomGenSquare<MapGenContext>(new RandRange(8), new RandRange(8)), new Loc(3), 9);
                            largeRoom.OpenBorders[(int)Dir4.Down][1] = true;
                            largeRoom.OpenBorders[(int)Dir4.Left][1] = true;
                            largeRoom.OpenBorders[(int)Dir4.Up][1] = true;
                            largeRoom.OpenBorders[(int)Dir4.Right][1] = true;
                            step.GiantRooms.Add(largeRoom, 10);
                        }
                        {
                            string[] custom = new string[] {  "~~~..~~~",
                                                                  "~~~..~~~",
                                                                  "~~#..#~~",
                                                                  "........",
                                                                  "........",
                                                                  "~~#..#~~",
                                                                  "~~~..~~~",
                                                                  "~~~..~~~"};
                            LargeRoom<MapGenContext> largeRoom = new LargeRoom<MapGenContext>(CreateRoomGenSpecific<MapGenContext>(custom), new Loc(3), 2);
                            largeRoom.OpenBorders[(int)Dir4.Down][1] = true;
                            largeRoom.OpenBorders[(int)Dir4.Left][1] = true;
                            largeRoom.OpenBorders[(int)Dir4.Up][1] = true;
                            largeRoom.OpenBorders[(int)Dir4.Right][1] = true;
                            step.GiantRooms.Add(largeRoom, 10);
                        }
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }
                    //MODDABLE: can turn on or off, change percentage; use different special rooms
                    {
                        CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(3), GetImmutableFilterList());
                        step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        step.RoomComponents.Set(new NoEventRoom());
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2), new RoomGenSquare<MapGenContext>(new RandRange(5), new RandRange(5))), 10);
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }

                    //Output the rooms into a FloorPlan
                    layout.GenSteps.Add(PR_ROOMS_INIT, new DrawGridToFloorStep<MapGenContext>());


                    AddDrawGridSteps(layout);

                    //Add the stairs up and down
                    AddStairStep(layout, false);

                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                    structure.Floors.Add(layout);
                }
                #endregion

                #region HEDGE MAZE 2
                {
                    GridFloorGen layout = new GridFloorGen();

                    //Floor settings
                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(structure.Floors.Count, new LocalText("Test: Hedge Maze 2")));
                    AddFloorData(layout, "B07. Flyaway Cliffs.ogg", 1000, Map.SightRange.Clear, Map.SightRange.Dark);
                    AddDefaultMapStatus(layout, "default_mapstatus", "gravity");

                    //Tilesets
                    AddTextureData(layout, "sky_peak_4th_pass_wall", "sky_peak_4th_pass_floor", "sky_peak_4th_pass_secondary", "normal");

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 5), "tile_wonder", true);//wonder tile
                    AddSingleTrapStep(layout, new RandRange(16, 19), "trap_poison", false);//poison trap

                    //money
                    MoneySpawnStep<MapGenContext> moneySpawnStep = new MoneySpawnStep<MapGenContext>(new RandRange(90, 130));
                    layout.GenSteps.Add(PR_RESPAWN_MONEY, moneySpawnStep);
                    AddMoneyData(layout, new RandRange(10, 14), true);

                    //items
                    ItemSpawnStep<MapGenContext> itemSpawnStep = new ItemSpawnStep<MapGenContext>();
                    SpawnList<InvItem> items = new SpawnList<InvItem>();
                    items.Add(new InvItem(11), 12);
                    itemSpawnStep.Spawns.Add("uncategorized", items, 10);
                    layout.GenSteps.Add(PR_RESPAWN_ITEM, itemSpawnStep);

                    //enemies
                    AddRespawnData(layout, 3, 80);

                    //enemies
                    AddEnemySpawnData(layout, 20, new RandRange(2, 4));

                    MobSpawnStep<MapGenContext> spawnStep = new MobSpawnStep<MapGenContext>();
                    PoolTeamSpawner poolSpawn = new PoolTeamSpawner();
                    //sentret
                    poolSpawn.Spawns.Add(GetTeamMob("sentret", "", "scratch", "", "", "", new RandRange(2), "wander_dumb"), 10);
                    poolSpawn.TeamSizes.Add(1, 12);
                    spawnStep.Spawns.Add(poolSpawn, 100);
                    layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);


                    //items
                    RandomSpawnStep<MapGenContext, InvItem> freeItemStep = new RandomSpawnStep<MapGenContext, InvItem>(new ContextSpawner<MapGenContext, InvItem>(new RandRange(2, 4)));
                    layout.GenSteps.Add(PR_SPAWN_ITEMS, freeItemStep);

                    AddItemData(layout, new RandRange(3, 7), 25);


                    //Initialize a 6x4 grid of 10x10 cells.
                    AddInitGridStep(layout, 13, 11, 2, 2);


                    //Create a path that is composed of a branching tree
                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(100);
                    //MODDABLE: can change branching
                    path.BranchRatio = new RandRange(30);

                    //Give it some room types to place
                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    ////square
                    genericRooms.Add(new RoomGenSquare<MapGenContext>(new RandRange(2), new RandRange(2)), 3);
                    path.GenericRooms = genericRooms;

                    //Give it some hall types to place
                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenSquare<MapGenContext>(new RandRange(1), new RandRange(1)), 20);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    //MODDABLE: can change connectivity
                    {
                        ConnectGridBranchStep<MapGenContext> step = new ConnectGridBranchStep<MapGenContext>(80);
                        step.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        step.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom()));
                        PresetPicker<PermissiveRoomGen<MapGenContext>> picker = new PresetPicker<PermissiveRoomGen<MapGenContext>>();
                        picker.ToSpawn = new RoomGenSquare<MapGenContext>(new RandRange(1), new RandRange(1));
                        step.GenericHalls = picker;
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }
                    {
                        SetGridPlanComponentStep<MapGenContext> step = new SetGridPlanComponentStep<MapGenContext>();
                        step.Components.Set(new NoEventRoom());
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                        //layout.GenSteps.Add(PR_GRID_GEN, new MarkAsHallStep<MapGenContext>());
                    }

                    //MODDABLE: use different special rooms
                    //TODO: come up with different special rooms
                    {
                        AddLargeRoomStep<MapGenContext> step = new AddLargeRoomStep<MapGenContext>(new RandRange(4, 6), GetImmutableFilterList());
                        step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        {
                            LargeRoom<MapGenContext> largeRoom = new LargeRoom<MapGenContext>(new RoomGenSquare<MapGenContext>(new RandRange(10), new RandRange(10)), new Loc(3), 3);
                            largeRoom.OpenBorders[(int)Dir4.Down][1] = true;
                            largeRoom.OpenBorders[(int)Dir4.Left][1] = true;
                            largeRoom.OpenBorders[(int)Dir4.Up][1] = true;
                            largeRoom.OpenBorders[(int)Dir4.Right][1] = true;
                            step.GiantRooms.Add(largeRoom, 10);
                        }
                        {
                            string[] custom = new string[] {  "~~~~..~~~~",
                                                                  "~~~~..~~~~",
                                                                  "~~~~..~~~~",
                                                                  "~~~#..#~~~",
                                                                  "..........",
                                                                  "..........",
                                                                  "~~~#..#~~~",
                                                                  "~~~~..~~~~",
                                                                  "~~~~..~~~~",
                                                                  "~~~~..~~~~"};
                            LargeRoom<MapGenContext> largeRoom = new LargeRoom<MapGenContext>(CreateRoomGenSpecific<MapGenContext>(custom), new Loc(3), 2);
                            largeRoom.OpenBorders[(int)Dir4.Down][1] = true;
                            largeRoom.OpenBorders[(int)Dir4.Left][1] = true;
                            largeRoom.OpenBorders[(int)Dir4.Up][1] = true;
                            largeRoom.OpenBorders[(int)Dir4.Right][1] = true;
                            step.GiantRooms.Add(largeRoom, 10);
                        }
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }
                    //MODDABLE: can turn on or off, change percentage; use different special rooms
                    {
                        //can dissuade monster houses by turning on IMMUTABLE (kind of a hack)
                        CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(100), GetImmutableFilterList());
                        step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        step.RoomComponents.Set(new NoEventRoom());
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2), new RoomGenSquare<MapGenContext>(new RandRange(5), new RandRange(5))), 10);
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }

                    AddDrawGridSteps(layout);


                    //Add the stairs up and down
                    AddStairStep(layout, false);




                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());


                    structure.Floors.Add(layout);
                }
                #endregion

                #region FLOATING ISLANDS
                {
                    RoomFloorGen layout = new RoomFloorGen();

                    //Floor settings
                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<ListMapGenContext>(structure.Floors.Count, new LocalText("Test: Floating Islands")));
                    AddFloorData(layout, "B07. Flyaway Cliffs.ogg", 2000, Map.SightRange.Clear, Map.SightRange.Dark);
                    AddDefaultMapStatus(layout, "default_mapstatus", "inverse");

                    //Tilesets
                    AddTextureData(layout, "sky_tower_wall", "sky_tower_floor", "sky_tower_secondary", "flying");

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 5), "tile_wonder", true);//wonder tile
                    AddSingleTrapStep(layout, new RandRange(16, 19), "trap_poison", false);//poison trap

                    //money - 1,000P
                    MoneySpawnStep<ListMapGenContext> moneySpawnStep = new MoneySpawnStep<ListMapGenContext>(new RandRange(1000));
                    layout.GenSteps.Add(PR_RESPAWN_MONEY, moneySpawnStep);
                    AddMoneyData(layout, new RandRange(2, 4));

                    //items
                    ItemSpawnStep<ListMapGenContext> itemSpawnStep = new ItemSpawnStep<ListMapGenContext>();
                    SpawnList<InvItem> items = new SpawnList<InvItem>();
                    items.Add(new InvItem(11), 12);
                    itemSpawnStep.Spawns.Add("uncategorized", items, 10);
                    layout.GenSteps.Add(PR_RESPAWN_ITEM, itemSpawnStep);

                    //enemies!
                    AddRespawnData(layout, 3, 80);

                    //enemies
                    AddEnemySpawnData(layout, 20, new RandRange(2, 4));

                    MobSpawnStep<ListMapGenContext> spawnStep = new MobSpawnStep<ListMapGenContext>();
                    PoolTeamSpawner poolSpawn = new PoolTeamSpawner();
                    poolSpawn.Spawns.Add(GetTeamMob("chatot", "", "echoed_voice", "feather_dance", "", "", new RandRange(18)), 10);
                    poolSpawn.TeamSizes.Add(1, 12);
                    spawnStep.Spawns.Add(poolSpawn, 100);
                    layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);

                    //sky-only mobs
                    {
                        SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                        specificTeam.Spawns.Add(GetGenericMob("dragonite", "", "wrap", "leer", "", "", new RandRange(15), "patrol"));

                        LoopedTeamSpawner<ListMapGenContext> spawner = new LoopedTeamSpawner<ListMapGenContext>(specificTeam);
                        {
                            spawner.AmountSpawner = new RandRange(10, 13);
                        }
                        PlaceTerrainMobsStep<ListMapGenContext> secretMobPlacement = new PlaceTerrainMobsStep<ListMapGenContext>(spawner);
                        secretMobPlacement.AcceptedTiles.Add(new Tile("abyss"));
                        layout.GenSteps.Add(PR_SPAWN_MOBS, secretMobPlacement);
                    }

                    //items
                    AddItemData(layout, new RandRange(3, 6), 25);

                    //secret items
                    SpawnList<InvItem> secretItemSpawns = new SpawnList<InvItem>();
                    secretItemSpawns.Add(new InvItem(101), 10);

                    //DisconnectedSpawnStep<ListMapGenContext, InvItem, MapGenEntrance> secretPlacement = new DisconnectedSpawnStep<ListMapGenContext, InvItem, MapGenEntrance>(new PickerSpawner<ListMapGenContext, InvItem>(new LoopedRand<InvItem>(secretItemSpawns, new RandRange(1, 2))));
                    RandomRoomSpawnStep<ListMapGenContext, InvItem> secretPlacement = new RandomRoomSpawnStep<ListMapGenContext, InvItem>(new PickerSpawner<ListMapGenContext, InvItem>(new LoopedRand<InvItem>(secretItemSpawns, new RandRange(1, 2))));
                    secretPlacement.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Disconnected));
                    layout.GenSteps.Add(PR_SPAWN_ITEMS_EXTRA, secretPlacement);

                    {
                        //secret enemies
                        SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                        specificTeam.Spawns.Add(GetGenericMob("spiritomb", "", "echoed_voice", "feather_dance", "", "", new RandRange(18)));


                        //secret enemies
                        //PlaceDisconnectedMobsStep<ListMapGenContext> secretMobPlacement = new PlaceDisconnectedMobsStep<ListMapGenContext>(new TeamPickerSpawner<ListMapGenContext>(specificTeam), new RandRange(2,5));
                        PlaceRandomMobsStep<ListMapGenContext> secretMobPlacement = new PlaceRandomMobsStep<ListMapGenContext>(new LoopedTeamSpawner<ListMapGenContext>(specificTeam, new RandRange(2, 4)));
                        secretMobPlacement.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Disconnected));
                        {
                            secretMobPlacement.ClumpFactor = 20;
                        }
                        layout.GenSteps.Add(PR_SPAWN_ITEMS, secretMobPlacement);
                    }


                    //Initialize a 54x40 floorplan with which to populate with rectangular floor and halls.
                    AddInitListStep(layout, 58, 40, true);

                    {
                        //Create a path that is composed of a branching tree
                        FloorPathBranch<ListMapGenContext> path = new FloorPathBranch<ListMapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallPercent = 0;
                        path.FillPercent = new RandRange(65);
                        //MODDABLE: can change branching
                        path.BranchRatio = new RandRange(30);

                        //Give it some room types to place
                        SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                        //cave
                        genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(6, 18), new RandRange(6, 18)), 10);

                        path.GenericRooms = genericRooms;

                        //Give it some hall types to place
                        SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                        genericHalls.Add(new RoomGenSquare<ListMapGenContext>(new RandRange(1), new RandRange(1)), 20);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_ROOMS_GEN, path);
                    }

                    {
                        //Add some disconnected rooms
                        AddDisconnectedRoomsStep<ListMapGenContext> addDisconnect = new AddDisconnectedRoomsStep<ListMapGenContext>();
                        addDisconnect.Components.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Disconnected));
                        addDisconnect.Amount = new RandRange(2, 4);

                        //Give it some room types to place
                        SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                        //cave
                        genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(5, 10), new RandRange(5, 10)), 10);
                        //circle
                        genericRooms.Add(new RoomGenRound<ListMapGenContext>(new RandRange(4, 6), new RandRange(4, 6)), 5);

                        addDisconnect.GenericRooms = genericRooms;


                        layout.GenSteps.Add(PR_ROOMS_GEN, addDisconnect);
                    }

                    //AddDrawListSteps(layout);

                    //draw paths
                    layout.GenSteps.Add(PR_TILES_INIT, new DrawFloorToTileStep<ListMapGenContext>());


                    //Add the stairs up and down
                    AddStairStep(layout, false);


                    //Generate water (specified by user as Terrain 2) with a frequency of 99%, using Perlin Noise in an order of 2.
                    string terrain = "abyss";
                    PerlinWaterStep<ListMapGenContext> waterStep = new PerlinWaterStep<ListMapGenContext>(new RandRange(80), 3, new Tile(terrain), new MapTerrainStencil<ListMapGenContext>(false, true, false), 2, false);
                    layout.GenSteps.Add(PR_WATER, waterStep);

                    //lay down more floor
                    //commented out and using disconnected rooms instead...
                    //int floorTerrain = 0;
                    //IntrudingBlobWaterStep<ListMapGenContext> floorStep = new IntrudingBlobWaterStep<ListMapGenContext>(new RandRange(8), new Tile(floorTerrain), 10, new RandRange(20));
                    //layout.GenSteps.Add(PR_WATER, floorStep);

                    //put the walls back in via "water" algorithm
                    string wallTerrain = "wall";
                    IntrudingBlobWaterStep<ListMapGenContext> wallStep = new IntrudingBlobWaterStep<ListMapGenContext>(new RandRange(10), new Tile(wallTerrain), new DefaultTerrainStencil<ListMapGenContext>(), 0, new RandRange(20));
                    layout.GenSteps.Add(PR_WATER, wallStep);

                    //Remove walls where diagonals of water exist and replace with water
                    layout.GenSteps.Add(PR_WATER_DIAG, new DropDiagonalBlockStep<ListMapGenContext>(new Tile(terrain)));


                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<ListMapGenContext, MapGenEntrance, MapGenExit>());


                    structure.Floors.Add(layout);
                }
                #endregion

                #region Merged Maze
                {
                    GridFloorGen layout = new GridFloorGen();

                    //Floor settings
                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(structure.Floors.Count, new LocalText("Test: Grid Room Merging")));
                    AddFloorData(layout, "B07. Flyaway Cliffs.ogg", 1000, Map.SightRange.Dark, Map.SightRange.Dark);

                    //Tilesets
                    //AddTextureData(layout, "sky_peak_4th_pass_wall", "sky_peak_4th_pass_floor", "sky_peak_4th_pass_secondary", "normal");

                    MapDictTextureStep<MapGenContext> textureStep = new MapDictTextureStep<MapGenContext>();
                    {
                        textureStep.BlankBG = "wyvern_hill_wall";
                        textureStep.TextureMap["floor"] = "wyvern_hill_floor";
                        textureStep.TextureMap["unbreakable"] = "wyvern_hill_wall";
                        textureStep.TextureMap["wall"] = "wyvern_hill_wall";
                        textureStep.TextureMap["water"] = "wyvern_hill_secondary";
                        textureStep.TextureMap["poison_water"] = "wyvern_hill_secondary";
                        textureStep.TextureMap["abyss"] = "wyvern_hill_secondary";
                        textureStep.TextureMap["poison_water"] = "wyvern_hill_secondary";
                        textureStep.TextureMap["grass"] = "tall_grass";
                    }
                    textureStep.GroundElement = "normal";
                    textureStep.LayeredGround = true;
                    layout.GenSteps.Add(PR_TEXTURES, textureStep);

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 5), "tile_wonder", true);//wonder tile
                    AddSingleTrapStep(layout, new RandRange(16, 19), "trap_poison", false);//poison trap

                    //money
                    MoneySpawnStep<MapGenContext> moneySpawnStep = new MoneySpawnStep<MapGenContext>(new RandRange(90, 130));
                    layout.GenSteps.Add(PR_RESPAWN_MONEY, moneySpawnStep);
                    AddMoneyData(layout, new RandRange(10, 14), true);

                    //items
                    ItemSpawnStep<MapGenContext> itemSpawnStep = new ItemSpawnStep<MapGenContext>();
                    SpawnList<InvItem> items = new SpawnList<InvItem>();
                    items.Add(new InvItem(11), 12);
                    itemSpawnStep.Spawns.Add("uncategorized", items, 10);
                    layout.GenSteps.Add(PR_RESPAWN_ITEM, itemSpawnStep);

                    //enemies
                    AddRespawnData(layout, 3, 5);

                    //enemies
                    AddEnemySpawnData(layout, 20, new RandRange(8, 12));

                    MobSpawnStep<MapGenContext> spawnStep = new MobSpawnStep<MapGenContext>();
                    PoolTeamSpawner poolSpawn = new PoolTeamSpawner();
                    //sentret
                    poolSpawn.Spawns.Add(GetTeamMob("sentret", "", "scratch", "", "", "", new RandRange(25), "wander_dumb"), 10);
                    poolSpawn.TeamSizes.Add(1, 12);
                    spawnStep.Spawns.Add(poolSpawn, 100);
                    layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);

                    {
                        //UB Stalks the grounds
                        MobSpawn mob = new MobSpawn();
                        mob.BaseForm = new MonsterID("nihilego", 0, "", Gender.Unknown);
                        mob.SpecifiedSkills.Add("acid_spray");
                        mob.SpecifiedSkills.Add("toxic_spikes");
                        mob.SpecifiedSkills.Add("power_gem");
                        mob.Intrinsic = "";
                        mob.Level = new RandRange(40);
                        mob.Tactic = "wander_normal";
                        mob.SpawnFeatures.Add(new MobSpawnMovesOff(mob.SpecifiedSkills.Count));
                        MobSpawnStatus keySpawn = new MobSpawnStatus();
                        keySpawn.Statuses.Add(new StatusEffect("veiled"), 10);
                        mob.SpawnFeatures.Add(keySpawn);
                        keySpawn = new MobSpawnStatus();
                        keySpawn.Statuses.Add(new StatusEffect("friendly_fire"), 10);
                        mob.SpawnFeatures.Add(keySpawn);
                        SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                        specificTeam.Spawns.Add(mob);

                        LoopedTeamSpawner<MapGenContext> spawner = new LoopedTeamSpawner<MapGenContext>(specificTeam);
                        {
                            spawner.AmountSpawner = new RandRange(1);
                        }
                        PlaceRandomMobsStep<MapGenContext> secretMobPlacement = new PlaceRandomMobsStep<MapGenContext>(spawner);
                        layout.GenSteps.Add(PR_SPAWN_MOBS, secretMobPlacement);
                    }

                    //items
                    RandomSpawnStep<MapGenContext, InvItem> freeItemStep = new RandomSpawnStep<MapGenContext, InvItem>(new ContextSpawner<MapGenContext, InvItem>(new RandRange(2, 4)));
                    layout.GenSteps.Add(PR_SPAWN_ITEMS, freeItemStep);

                    AddItemData(layout, new RandRange(3, 7), 25);


                    //Initialize a 13x11 grid of 2x2 cells.
                    //AddInitGridStep(layout, 13, 11, 2, 2);

                    AddInitGridStep(layout, 8, 6, 8, 8, 1, true);

                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(80);
                    path.BranchRatio = new RandRange(0, 25);

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //square
                    genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8), new RandRange(0, 30)), 10);
                    //blocked
                    genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(5, 9), new RandRange(5, 9), new RandRange(8, 9), new RandRange(1, 2)), 4);
                    genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(5, 9), new RandRange(5, 9), new RandRange(1, 2), new RandRange(8, 9)), 4);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));

                    {
                        CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(8), GetImmutableFilterList());
                        step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        step.RoomComponents.Set(new NoEventRoom());
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2), new RoomGenRound<MapGenContext>(new RandRange(13), new RandRange(13))), 10);
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenRound<MapGenContext>(new RandRange(7), new RandRange(13))), 10);
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenRound<MapGenContext>(new RandRange(13), new RandRange(7))), 10);
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }

                    //special room gen
                    {
                        RoomGenLoadMap<MapGenContext> loadRoom = new RoomGenLoadMap<MapGenContext>();
                        loadRoom.MapID = "test_room";
                        loadRoom.RoomTerrain = new Tile("floor");
                        loadRoom.PreventChanges = PostProcType.Terrain;
                        SetGridSpecialRoomStep<MapGenContext> specialStep = new SetGridSpecialRoomStep<MapGenContext>();
                        specialStep.Rooms = new PresetPicker<RoomGen<MapGenContext>>(loadRoom);
                        specialStep.RoomComponents.Set(new ImmutableRoom());
                        layout.GenSteps.Add(PR_GRID_GEN_EXTRA, specialStep);
                    }
                    
                    //AddDrawGridSteps(layout);

                    layout.GenSteps.Add(PR_ROOMS_INIT, new DrawGridToFloorStep<MapGenContext>());
                    layout.GenSteps.Add(PR_TILES_INIT, new DrawFloorToTileStep<MapGenContext>());


                    //Add the stairs up and down
                    AddStairStep(layout, false);


                    //grass
                    string coverTerrain = "grass";
                    PerlinWaterStep<MapGenContext> coverStep = new PerlinWaterStep<MapGenContext>(new RandRange(20), 3, new Tile(coverTerrain), new MapTerrainStencil<MapGenContext>(true, false, false), 1);
                    layout.GenSteps.Add(PR_WATER, coverStep);


                    //layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());


                    structure.Floors.Add(layout);
                }
                #endregion

                #region CHASM CAVE
                {
                    GridFloorGen layout = new GridFloorGen();

                    //Floor settings
                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(structure.Floors.Count, new LocalText("Test: Chasm Cave")));
                    AddFloorData(layout, "B07. Flyaway Cliffs.ogg", 2000, Map.SightRange.Clear, Map.SightRange.Dark);

                    //Tilesets
                    MapDictTextureStep<MapGenContext> textureStep = new MapDictTextureStep<MapGenContext>();
                    {
                        textureStep.BlankBG = "chasm_cave_1_wall";
                        textureStep.TextureMap["floor"] = "chasm_cave_1_floor";
                        textureStep.TextureMap["unbreakable"] = "spacial_rift_1_wall";
                        textureStep.TextureMap["wall"] = "spacial_rift_1_wall";
                        textureStep.TextureMap["water"] = "chasm_cave_1_wall";
                        textureStep.TextureMap["poison_water"] = "chasm_cave_1_wall";
                        textureStep.TextureMap["abyss"] = "chasm_cave_1_wall";
                        textureStep.TextureMap["poison_water"] = "chasm_cave_1_wall";
                    }
                    textureStep.GroundElement = "normal";
                    textureStep.LayeredGround = true;
                    layout.GenSteps.Add(PR_TEXTURES, textureStep);

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 5), "tile_wonder", true);//wonder tile
                    AddSingleTrapStep(layout, new RandRange(16, 19), "trap_poison", false);//poison trap

                    //money - 1,000P
                    MoneySpawnStep<MapGenContext> moneySpawnStep = new MoneySpawnStep<MapGenContext>(new RandRange(1000));
                    layout.GenSteps.Add(PR_RESPAWN_MONEY, moneySpawnStep);
                    AddMoneyData(layout, new RandRange(2, 4));

                    //items
                    ItemSpawnStep<MapGenContext> itemSpawnStep = new ItemSpawnStep<MapGenContext>();
                    SpawnList<InvItem> items = new SpawnList<InvItem>();
                    items.Add(new InvItem(11), 12);
                    itemSpawnStep.Spawns.Add("uncategorized", items, 10);
                    layout.GenSteps.Add(PR_RESPAWN_ITEM, itemSpawnStep);

                    //enemies!
                    AddRespawnData(layout, 3, 80);

                    //enemies
                    AddEnemySpawnData(layout, 20, new RandRange(2, 4));

                    MobSpawnStep<MapGenContext> spawnStep = new MobSpawnStep<MapGenContext>();
                    PoolTeamSpawner poolSpawn = new PoolTeamSpawner();
                    poolSpawn.Spawns.Add(GetTeamMob("chatot", "", "echoed_voice", "feather_dance", "", "", new RandRange(18)), 10);
                    poolSpawn.TeamSizes.Add(1, 12);
                    spawnStep.Spawns.Add(poolSpawn, 100);
                    layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);

                    //items
                    AddItemData(layout, new RandRange(3, 6), 25);

                    AddInitGridStep(layout, 6, 4, 10, 10);

                    {
                        GridPathGrid<MapGenContext> path = new GridPathGrid<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.RoomRatio = 80;
                        path.HallRatio = 30;

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //square
                        genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 9), new RandRange(4, 9), new RandRange(0, 81)), 10);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(20), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_GRID_GEN, path);
                    }

                    //init from floor plan
                    layout.GenSteps.Add(PR_ROOMS_INIT, new DrawGridToFloorStep<MapGenContext>());
                    //draw paths
                    layout.GenSteps.Add(PR_TILES_INIT, new DrawFloorToTileStep<MapGenContext>(2));

                    //add invisible unbreakable border
                    Tile invisBarrier = new Tile("unbreakable", true);
                    invisBarrier.Data.TileTex = new AutoTile("chasm_cave_1_wall");
                    layout.GenSteps.Add(PR_TILES_BARRIER, new TileBorderStep<MapGenContext>(1, invisBarrier));

                    AddStairStep(layout, false);


                    //Generate water (specified by user as Terrain 2) with a frequency of 100%, using Perlin Noise in an order of 2.
                    string terrain = "abyss";
                    PerlinWaterStep<MapGenContext> waterStep = new PerlinWaterStep<MapGenContext>(new RandRange(100), 3, new Tile(terrain), new MapTerrainStencil<MapGenContext>(false, true, false), 2);
                    layout.GenSteps.Add(PR_WATER, waterStep);
                    
                    //Remove walls where diagonals of water exist and replace with water
                    layout.GenSteps.Add(PR_WATER_DIAG, new DropDiagonalBlockStep<MapGenContext>(new Tile(terrain)));



                    //boss rooms
                    {
                        SpawnList<RoomGen<MapGenContext>> bossRooms = new SpawnList<RoomGen<MapGenContext>>();
                        string[] custom = new string[] {  "~~~...~~~",
                                                              "~~~...~~~",
                                                              "~~X...X~~",
                                                              ".........",
                                                              ".........",
                                                              ".........",
                                                              "~~X...X~~",
                                                              "~~~...~~~",
                                                              "~~~...~~~"};
                        List<MobSpawn> mobSpawns = new List<MobSpawn>();
                        {
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID("kyogre", 0, "normal", Gender.Unknown);
                            post_mob.Tactic = "wait_only";
                            post_mob.Level = new RandRange(50);
                            post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc(1, 4)));
                            post_mob.SpawnFeatures.Add(new MobSpawnItem(true, 1));
                            post_mob.SpawnFeatures.Add(new MobSpawnUnrecruitable());
                            mobSpawns.Add(post_mob);
                        }
                        {
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID("groudon", 0, "normal", Gender.Unknown);
                            post_mob.Tactic = "wait_only";
                            post_mob.Level = new RandRange(50);
                            post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc(7, 4)));
                            post_mob.SpawnFeatures.Add(new MobSpawnItem(true, 1));
                            post_mob.SpawnFeatures.Add(new MobSpawnUnrecruitable());
                            mobSpawns.Add(post_mob);
                        }
                        {
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID("rayquaza", 0, "normal", Gender.Unknown);
                            post_mob.Tactic = "wait_only";
                            post_mob.Level = new RandRange(50);
                            post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc(4, 1)));
                            post_mob.SpawnFeatures.Add(new MobSpawnItem(true, 1));
                            post_mob.SpawnFeatures.Add(new MobSpawnUnrecruitable());
                            mobSpawns.Add(post_mob);
                        }
                        bossRooms.Add(CreateRoomGenSpecificBoss<MapGenContext>(custom, new Loc(4, 4), mobSpawns, true), 10);
                        SpawnList<RoomGen<MapGenContext>> treasureRooms = new SpawnList<RoomGen<MapGenContext>>();
                        treasureRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
                        SpawnList<PermissiveRoomGen<MapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        detourHalls.Add(new RoomGenAngledHall<MapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                        AddBossRoomStep<MapGenContext> detours = new AddBossRoomStep<MapGenContext>(bossRooms, treasureRooms, detourHalls);
                        detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom()));
                        detours.BossComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        detours.BossComponents.Set(new NoEventRoom());
                        detours.BossComponents.Set(new BossRoom());
                        detours.BossHallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        detours.VaultComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.BossLocked));
                        detours.VaultComponents.Set(new NoConnectRoom());
                        detours.VaultComponents.Set(new NoEventRoom());
                        detours.VaultHallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.BossLocked));
                        detours.VaultHallComponents.Set(new NoConnectRoom());

                        layout.GenSteps.Add(PR_ROOMS_GEN_EXTRA, detours);
                    }
                    //sealing the boss room and treasure room
                    {
                        BossSealStep<MapGenContext> vaultStep = new BossSealStep<MapGenContext>("sealed_block", "tile_boss");
                        vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                        vaultStep.BossFilters.Add(new RoomFilterComponent(false, new BossRoom()));
                        layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                    }
                    //vault treasures
                    {
                        BulkSpawner<MapGenContext, InvItem> treasures = new BulkSpawner<MapGenContext, InvItem>();
                        treasures.SpecificSpawns.Add(new InvItem(75));
                        treasures.SpecificSpawns.Add(new InvItem(75));
                        treasures.SpecificSpawns.Add(new InvItem(75));
                        RandomRoomSpawnStep<MapGenContext, InvItem> detourItems = new RandomRoomSpawnStep<MapGenContext, InvItem>(treasures);
                        detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                        layout.GenSteps.Add(PR_SPAWN_ITEMS_EXTRA, detourItems);
                    }


                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());


                    structure.Floors.Add(layout);
                }
                #endregion

                #region Wrapped Tiered Floor
                {
                    GridFloorGen layout = new GridFloorGen();

                    //Floor settings
                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(structure.Floors.Count, new LocalText("Test: Wrapped Tiered Floor")));
                    AddFloorData(layout, "B07. Flyaway Cliffs.ogg", 1000, Map.SightRange.Dark, Map.SightRange.Dark);

                    //Tilesets
                    AddTextureData(layout, "sky_peak_4th_pass_wall", "sky_peak_4th_pass_floor", "sky_peak_4th_pass_secondary", "normal");

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 5), "tile_wonder", true);//wonder tile
                    AddSingleTrapStep(layout, new RandRange(16, 19), "trap_poison", false);//poison trap

                    //money
                    MoneySpawnStep<MapGenContext> moneySpawnStep = new MoneySpawnStep<MapGenContext>(new RandRange(90, 130));
                    layout.GenSteps.Add(PR_RESPAWN_MONEY, moneySpawnStep);
                    AddMoneyData(layout, new RandRange(10, 14), true);

                    //items
                    ItemSpawnStep<MapGenContext> itemSpawnStep = new ItemSpawnStep<MapGenContext>();
                    SpawnList<InvItem> items = new SpawnList<InvItem>();
                    items.Add(new InvItem(11), 12);
                    itemSpawnStep.Spawns.Add("uncategorized", items, 10);
                    layout.GenSteps.Add(PR_RESPAWN_ITEM, itemSpawnStep);

                    //enemies
                    AddRespawnData(layout, 3, 5);

                    //enemies
                    AddEnemySpawnData(layout, 20, new RandRange(8, 12));

                    MobSpawnStep<MapGenContext> spawnStep = new MobSpawnStep<MapGenContext>();
                    PoolTeamSpawner poolSpawn = new PoolTeamSpawner();
                    //sentret
                    poolSpawn.Spawns.Add(GetTeamMob("sentret", "", "scratch", "", "", "", new RandRange(25), "wander_dumb"), 10);
                    poolSpawn.TeamSizes.Add(1, 12);
                    spawnStep.Spawns.Add(poolSpawn, 100);
                    layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);

                    //items
                    RandomSpawnStep<MapGenContext, InvItem> freeItemStep = new RandomSpawnStep<MapGenContext, InvItem>(new ContextSpawner<MapGenContext, InvItem>(new RandRange(2, 4)));
                    layout.GenSteps.Add(PR_SPAWN_ITEMS, freeItemStep);

                    AddItemData(layout, new RandRange(3, 7), 25);


                    //Initialize a 13x11 grid of 2x2 cells.
                    //AddInitGridStep(layout, 13, 11, 2, 2);

                    AddInitGridStep(layout, 8, 5, 6, 7, 1, true);

                    GridPathTiered<MapGenContext> path = new GridPathTiered<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.TierAxis = Axis4.Horiz;
                    path.TierConnections = new RandRange(1, 3);

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //square
                    genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(3, 7), new RandRange(3, 7), new RandRange(0, 30)), 10);
                    //blocked
                    genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(4, 7), new RandRange(4, 7), new RandRange(8, 9), new RandRange(1, 2)), 4);
                    genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(4, 7), new RandRange(4, 7), new RandRange(1, 2), new RandRange(8, 9)), 4);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    layout.GenSteps.Add(PR_GRID_GEN, new SetGridDefaultsStep<MapGenContext>(new RandRange(30), GetImmutableFilterList()));
                    {
                        CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(8), GetImmutableFilterList());
                        step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        step.RoomComponents.Set(new NoEventRoom());
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenRound<MapGenContext>(new RandRange(13), new RandRange(7))), 10);
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(3, 1), new RoomGenRound<MapGenContext>(new RandRange(19), new RandRange(5))), 10);
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }

                    //AddDrawGridSteps(layout);

                    layout.GenSteps.Add(PR_ROOMS_INIT, new DrawGridToFloorStep<MapGenContext>());
                    layout.GenSteps.Add(PR_TILES_INIT, new DrawFloorToTileStep<MapGenContext>());


                    //Add the stairs up and down
                    AddStairStep(layout, false);

                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                    structure.Floors.Add(layout);
                }
                #endregion

                #region Scripted Floor 1
                {
                    GridFloorGen layout = new GridFloorGen();

                    //Floor settings
                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(structure.Floors.Count, new LocalText("Test: Script Gen Step")));
                    AddFloorData(layout, "B07. Flyaway Cliffs.ogg", 1000, Map.SightRange.Dark, Map.SightRange.Dark);

                    //Tilesets
                    //AddTextureData(layout, "sky_peak_4th_pass_wall", "sky_peak_4th_pass_floor", "sky_peak_4th_pass_secondary", "normal");

                    MapDictTextureStep<MapGenContext> textureStep = new MapDictTextureStep<MapGenContext>();
                    {
                        textureStep.BlankBG = "wyvern_hill_wall";
                        textureStep.TextureMap["floor"] = "wyvern_hill_floor";
                        textureStep.TextureMap["unbreakable"] = "wyvern_hill_wall";
                        textureStep.TextureMap["wall"] = "wyvern_hill_wall";
                        textureStep.TextureMap["water"] = "wyvern_hill_secondary";
                        textureStep.TextureMap["poison_water"] = "mt_blaze_secondary";
                        textureStep.TextureMap["abyss"] = "chasm_cave_1_wall";
                        textureStep.TextureMap["poison_water"] = "poison_maze_secondary";
                        textureStep.TextureMap["grass"] = "tall_grass";
                    }
                    textureStep.GroundElement = "normal";
                    textureStep.LayeredGround = true;
                    layout.GenSteps.Add(PR_TEXTURES, textureStep);

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 5), "tile_wonder", true);//wonder tile
                    AddSingleTrapStep(layout, new RandRange(16, 19), "trap_poison", false);//poison trap

                    //money
                    MoneySpawnStep<MapGenContext> moneySpawnStep = new MoneySpawnStep<MapGenContext>(new RandRange(90, 130));
                    layout.GenSteps.Add(PR_RESPAWN_MONEY, moneySpawnStep);
                    AddMoneyData(layout, new RandRange(10, 14), true);

                    //items
                    ItemSpawnStep<MapGenContext> itemSpawnStep = new ItemSpawnStep<MapGenContext>();
                    SpawnList<InvItem> items = new SpawnList<InvItem>();
                    items.Add(new InvItem(11), 12);
                    itemSpawnStep.Spawns.Add("uncategorized", items, 10);
                    layout.GenSteps.Add(PR_RESPAWN_ITEM, itemSpawnStep);

                    //enemies
                    AddRespawnData(layout, 3, 80);

                    //enemies
                    AddEnemySpawnData(layout, 20, new RandRange(2, 4));

                    MobSpawnStep<MapGenContext> spawnStep = new MobSpawnStep<MapGenContext>();
                    PoolTeamSpawner poolSpawn = new PoolTeamSpawner();
                    //sentret
                    poolSpawn.Spawns.Add(GetTeamMob("sentret", "", "scratch", "", "", "", new RandRange(2), "wander_dumb"), 10);
                    poolSpawn.TeamSizes.Add(1, 12);
                    spawnStep.Spawns.Add(poolSpawn, 100);
                    layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);


                    //items
                    RandomSpawnStep<MapGenContext, InvItem> freeItemStep = new RandomSpawnStep<MapGenContext, InvItem>(new ContextSpawner<MapGenContext, InvItem>(new RandRange(2, 4)));
                    layout.GenSteps.Add(PR_SPAWN_ITEMS, freeItemStep);

                    AddItemData(layout, new RandRange(3, 7), 25);


                    //Initialize a 6x4 grid of 10x10 cells.
                    AddInitGridStep(layout, 13, 11, 2, 2);


                    AddInitGridStep(layout, 8, 6, 8, 8);

                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(80);
                    path.BranchRatio = new RandRange(0, 25);

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //square
                    genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8), new RandRange(0, 30)), 10);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    AddDrawGridSteps(layout);


                    //Add the stairs up and down
                    AddStairStep(layout, false);


                    layout.GenSteps.Add(PR_DBG_CHECK, new ScriptGenStep<MapGenContext>("Test"));


                    structure.Floors.Add(layout);
                }
                #endregion

                #region Scripted Floor 2
                {
                    GridFloorGen layout = new GridFloorGen();

                    //Floor settings
                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(structure.Floors.Count, new LocalText("Test: Script Gen Step on Grid and Floor")));
                    AddFloorData(layout, "B07. Flyaway Cliffs.ogg", 1000, Map.SightRange.Dark, Map.SightRange.Dark);

                    //Tilesets
                    //AddTextureData(layout, "sky_peak_4th_pass_wall", "sky_peak_4th_pass_floor", "sky_peak_4th_pass_secondary", "normal");

                    MapDictTextureStep<MapGenContext> textureStep = new MapDictTextureStep<MapGenContext>();
                    {
                        textureStep.BlankBG = "wyvern_hill_wall";
                        textureStep.TextureMap["floor"] = "wyvern_hill_floor";
                        textureStep.TextureMap["unbreakable"] = "wyvern_hill_wall";
                        textureStep.TextureMap["wall"] = "wyvern_hill_wall";
                        textureStep.TextureMap["water"] = "wyvern_hill_secondary";
                        textureStep.TextureMap["poison_water"] = "mt_blaze_secondary";
                        textureStep.TextureMap["abyss"] = "chasm_cave_1_wall";
                        textureStep.TextureMap["poison_water"] = "poison_maze_secondary";
                        textureStep.TextureMap["grass"] = "tall_grass";
                    }
                    textureStep.GroundElement = "normal";
                    textureStep.LayeredGround = true;
                    layout.GenSteps.Add(PR_TEXTURES, textureStep);

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 5), "tile_wonder", true);//wonder tile
                    AddSingleTrapStep(layout, new RandRange(16, 19), "trap_poison", false);//poison trap

                    //money
                    MoneySpawnStep<MapGenContext> moneySpawnStep = new MoneySpawnStep<MapGenContext>(new RandRange(90, 130));
                    layout.GenSteps.Add(PR_RESPAWN_MONEY, moneySpawnStep);
                    AddMoneyData(layout, new RandRange(10, 14), true);

                    //items
                    ItemSpawnStep<MapGenContext> itemSpawnStep = new ItemSpawnStep<MapGenContext>();
                    SpawnList<InvItem> items = new SpawnList<InvItem>();
                    items.Add(new InvItem(11), 12);
                    itemSpawnStep.Spawns.Add("uncategorized", items, 10);
                    layout.GenSteps.Add(PR_RESPAWN_ITEM, itemSpawnStep);

                    //enemies
                    AddRespawnData(layout, 3, 80);

                    //enemies
                    AddEnemySpawnData(layout, 20, new RandRange(2, 4));

                    MobSpawnStep<MapGenContext> spawnStep = new MobSpawnStep<MapGenContext>();
                    PoolTeamSpawner poolSpawn = new PoolTeamSpawner();
                    //sentret
                    poolSpawn.Spawns.Add(GetTeamMob("sentret", "", "scratch", "", "", "", new RandRange(2), "wander_dumb"), 10);
                    poolSpawn.TeamSizes.Add(1, 12);
                    spawnStep.Spawns.Add(poolSpawn, 100);
                    layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);


                    //items
                    RandomSpawnStep<MapGenContext, InvItem> freeItemStep = new RandomSpawnStep<MapGenContext, InvItem>(new ContextSpawner<MapGenContext, InvItem>(new RandRange(2, 4)));
                    layout.GenSteps.Add(PR_SPAWN_ITEMS, freeItemStep);

                    AddItemData(layout, new RandRange(3, 7), 25);


                    //Initialize a 6x4 grid of 10x10 cells.
                    AddInitGridStep(layout, 13, 11, 2, 2);


                    AddInitGridStep(layout, 8, 6, 8, 8);

                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(80);
                    path.BranchRatio = new RandRange(0, 25);

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //square
                    genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8), new RandRange(0, 30)), 10);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    AddDrawGridSteps(layout);


                    //Add the stairs up and down
                    AddStairStep(layout, false);


                    layout.GenSteps.Add(PR_GRID_GEN_EXTRA, new ScriptGenStep<MapGenContext>("TestGrid"));
                    layout.GenSteps.Add(PR_DBG_CHECK, new ScriptGenStep<MapGenContext>("TestRooms"));


                    structure.Floors.Add(layout);
                }
                #endregion

                //structure.MainExit = new ZoneLoc(1, 0);
                zone.Segments.Add(structure);
            }

            {
                SingularSegment structure = new SingularSegment(100);
                #region SECRET ROOMS

                StairsFloorGen layout = new StairsFloorGen();
                
                AddFloorData(layout, "A07. Summit.ogg", -1, Map.SightRange.Dark, Map.SightRange.Dark);

                //square
                string[] level = {
                            "##..........##",
                            "##..........##",
                            "..............",
                            "..............",
                            "....~~~~~~....",
                            "....~~~~~~....",
                            "....~~##~~....",
                            "....~~##~~....",
                            "....~~~~~~....",
                            "....~~~~~~....",
                            "..............",
                            "..............",
                            "##..........##",
                            "##..........##"
                        };

                InitTilesStep<StairsMapGenContext> startStep = new InitTilesStep<StairsMapGenContext>();
                int width = level[0].Length;
                int height = level.Length;
                startStep.Width = width + 2;
                startStep.Height = height + 2;

                layout.GenSteps.Add(PR_TILES_INIT, startStep);

                SpecificTilesStep<StairsMapGenContext> drawStep = new SpecificTilesStep<StairsMapGenContext>();
                drawStep.Offset = new Loc(1);

                drawStep.Tiles = new Tile[width][];
                for (int ii = 0; ii < width; ii++)
                {
                    drawStep.Tiles[ii] = new Tile[height];
                    for (int jj = 0; jj < height; jj++)
                    {
                        if (level[jj][ii] == '#')
                            drawStep.Tiles[ii][jj] = new Tile("wall");
                        else if (level[jj][ii] == '~')
                            drawStep.Tiles[ii][jj] = new Tile("water");
                        else
                            drawStep.Tiles[ii][jj] = new Tile("floor");
                    }
                }


                layout.GenSteps.Add(PR_TILES_GEN, drawStep);

                //add border
                layout.GenSteps.Add(PR_TILES_BARRIER, new UnbreakableBorderStep<StairsMapGenContext>(1));

                //TODO: secret rooms can't scale NPC levels this way; figure it out if you ever want to scale level
                layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<StairsMapGenContext>(0, new LocalText("Secret Room")));
                
                EffectTile secretStairs = new EffectTile("stairs_go_up", true);
                secretStairs.TileStates.Set(new DestState(new SegLoc(-1, 1), true));
                layout.GenSteps.Add(PR_EXITS, new StairsStep<StairsMapGenContext, MapGenEntrance, MapGenExit>(new MapGenEntrance(Dir8.Down), new MapGenExit(secretStairs)));

                //Tilesets
                AddTextureData(layout, "sky_peak_4th_pass_wall", "sky_peak_4th_pass_floor", "sky_peak_4th_pass_secondary", "normal");

                structure.BaseFloor = layout;

                #endregion
                //structure.MainExit = new ZoneLoc(1, 0);
                zone.Segments.Add(structure);
            }
            {
                LayeredSegment structure = new LayeredSegment();
                //Tests Tilesets, and unlockables
                #region TILESET TESTS
                int curTileIndex = 0;
                for (int kk = 0; kk < 5/*151*/; kk++)
                {
                    string[] level = {
                            "...........................................",
                            ".#........#........#...~........~........~.",
                            "...###...###...###.......~~~...~~~...~~~...",
                            "..#.#.....#.....#.#.....~.~.....~.....~.~..",
                            "..####...###...####.....~~~~...~~~...~~~~..",
                            "..#.#############.#.....~.~~~~~~~~~~~~~.~..",
                            ".....##.......##...........~~.......~~.....",
                            ".....#..#####..#...........~..~~~~~..~.....",
                            ".....#.#######.#...........~.~~~~~~~.~.....",
                            "..#.##.#######.##.#.....~.~~.~~~~~~~.~~.~..",
                            ".#####.###.###.#####...~~~~~.~~~.~~~.~~~~~.",
                            "..#.##.#######.##.#.....~.~~.~~~~~~~.~~.~..",
                            ".....#.#######.#...........~.~~~~~~~.~.....",
                            ".....#..#####..#...........~..~~~~~..~.....",
                            ".....##.......##...........~~.......~~.....",
                            "..#.#############.#.....~.~~~~~~~~~~~~~.~..",
                            "..####...###...####.....~~~~...~~~...~~~~..",
                            "..#.#.....#.....#.#.....~.~.....~.....~.~..",
                            "...###...###...###.......~~~...~~~...~~~...",
                            ".#........#........#...~........~........~.",
                            "...........................................",
                        };

                    StairsFloorGen layout = new StairsFloorGen();

                    AddFloorData(layout, "A07. Summit.ogg", -1, Map.SightRange.Dark, Map.SightRange.Dark);

                    InitTilesStep<StairsMapGenContext> startStep = new InitTilesStep<StairsMapGenContext>();
                    int width = level[0].Length;
                    int height = level.Length;
                    startStep.Width = width + 2;
                    startStep.Height = height + 2;

                    layout.GenSteps.Add(PR_TILES_INIT, startStep);

                    SpecificTilesStep<StairsMapGenContext> drawStep = new SpecificTilesStep<StairsMapGenContext>();
                    drawStep.Offset = new Loc(1);

                    drawStep.Tiles = new Tile[width][];
                    for (int ii = 0; ii < width; ii++)
                    {
                        drawStep.Tiles[ii] = new Tile[height];
                        for (int jj = 0; jj < height; jj++)
                        {
                            if (level[jj][ii] == '#')
                                drawStep.Tiles[ii][jj] = new Tile("wall");
                            else if (level[jj][ii] == '~')
                                drawStep.Tiles[ii][jj] = new Tile("water");
                            else
                                drawStep.Tiles[ii][jj] = new Tile("floor");
                        }
                    }

                    layout.GenSteps.Add(PR_TILES_GEN, drawStep);

                    //add border
                    layout.GenSteps.Add(PR_TILES_BARRIER, new UnbreakableBorderStep<StairsMapGenContext>(1));

                    {
                        List<(MapGenEntrance, Loc)> items = new List<(MapGenEntrance, Loc)>();
                        items.Add((new MapGenEntrance(Dir8.Down), new Loc(22, 11)));
                        AddSpecificSpawn(layout, items, PR_EXITS);
                    }
                    {
                        List<(MapGenExit, Loc)> items = new List<(MapGenExit, Loc)>();
                        items.Add((new MapGenExit(new EffectTile("stairs_go_up", true)), new Loc(22, 12)));
                        AddSpecificSpawn(layout, items, PR_EXITS);
                    }

                    AddTextureData(layout, "test_dungeon_wall", "test_dungeon_floor", "test_dungeon_secondary", "normal");
                    ////chasm cave
                    //if (curTileIndex != 258 && curTileIndex != 260)
                    //    curTileIndex += 3;
                    //else
                    //    curTileIndex += 2;

                    structure.Floors.Add(layout);
                }
                #endregion

                //structure.MainExit = new ZoneLoc(2, 0);
                zone.Segments.Add(structure);
            }

            {
                LayeredSegment structure = new LayeredSegment();
                //Tests Tilesets, and unlockables
                #region FLOOR MACHINE TESTS
                int curTileIndex = 0;
                for (int kk = 0; kk < 3; kk++)
                {
                    StairsFloorGen layout = new StairsFloorGen();

                    AddFloorData(layout, "A07. Summit.ogg", -1, Map.SightRange.Dark, Map.SightRange.Dark);

                    InitTilesStep<StairsMapGenContext> startStep = new InitTilesStep<StairsMapGenContext>();
                    int width = 70;
                    int height = 25;
                    startStep.Width = width + 2;
                    startStep.Height = height + 2;

                    layout.GenSteps.Add(PR_TILES_INIT, startStep);

                    SpecificTilesStep<StairsMapGenContext> drawStep = new SpecificTilesStep<StairsMapGenContext>();
                    drawStep.Offset = new Loc(1);

                    drawStep.Tiles = new Tile[width][];
                    for (int ii = 0; ii < width; ii++)
                    {
                        drawStep.Tiles[ii] = new Tile[height];
                        for (int jj = 0; jj < height; jj++)
                            drawStep.Tiles[ii][jj] = new Tile("floor");
                    }

                    //monster house
                    {
                        EffectTile effect = new EffectTile("chest_full", true, new Loc(4, 4));
                        effect.Danger = true;
                        effect.TileStates.Set(new UnlockState(455));
                        effect.TileStates.Set(new BoundsState(new Rect(0, 0, 10, 10)));
                        ItemSpawnState itemSpawn = new ItemSpawnState();
                        for (int ii = 0; ii < 10; ii++)
                            itemSpawn.Spawns.Add(new MapItem(100 + ii));
                        effect.TileStates.Set(itemSpawn);
                        MobSpawnState mobSpawn = new MobSpawnState();
                        List<string> monsterKeys = DataManager.Instance.DataIndices[DataManager.DataType.Monster].GetOrderedKeys(true);
                        for (int ii = 0; ii < 16; ii++)
                            mobSpawn.Spawns.Add(GetGenericMob(monsterKeys[260 + ii], "", "", "", "", "", new RandRange(10)));
                        effect.TileStates.Set(mobSpawn);
                        ((Tile)drawStep.Tiles[4][4]).Effect = effect;

                        drawStep.Tiles[1][7] = new Tile("wall");
                        drawStep.Tiles[1][8] = new Tile("wall");
                        drawStep.Tiles[1][9] = new Tile("wall");
                        drawStep.Tiles[2][7] = new Tile("wall");
                        drawStep.Tiles[2][8] = new Tile("wall");
                        drawStep.Tiles[2][9] = new Tile("wall");
                    }
                    {
                        EffectTile effect = new EffectTile("chest_full", true, new Loc(5, 5));
                        effect.Danger = true;
                        effect.TileStates.Set(new UnlockState(455));
                        effect.TileStates.Set(new BoundsState(new Rect(3, 3, 13, 13)));
                        ItemSpawnState itemSpawn = new ItemSpawnState();
                        for (int ii = 0; ii < 10; ii++)
                            itemSpawn.Spawns.Add(new MapItem(100 + ii));
                        effect.TileStates.Set(itemSpawn);
                        MobSpawnState mobSpawn = new MobSpawnState();
                        List<string> monsterKeys = DataManager.Instance.DataIndices[DataManager.DataType.Monster].GetOrderedKeys(true);
                        for (int ii = 0; ii < 16; ii++)
                            mobSpawn.Spawns.Add(GetGenericMob(monsterKeys[260 + ii], "", "", "", "", "", new RandRange(10)));
                        effect.TileStates.Set(mobSpawn);
                        ((Tile)drawStep.Tiles[5][5]).Effect = effect;
                    }

                    // shop
                    {
                        // place the mat of the shop
                        for (int xx = 0; xx < 3; xx++)
                        {
                            for (int yy = 0; yy < 3; yy++)
                            {
                                EffectTile effect = new EffectTile("area_shop", true, new Loc(20+xx, yy));
                                ((Tile)drawStep.Tiles[20+xx][yy]).Effect = effect;
                            }
                        }

                        // place the items of the shop
                        List<InvItem> treasure1 = new List<InvItem>();
                        treasure1.Add(new InvItem(1, false, 0, 50));//Apple
                        List<InvItem> treasure2 = new List<InvItem>();
                        treasure2.Add(new InvItem(444, false, 753, 8000));//Poison Dust
                        List<(List<InvItem>, Loc)> items = new List<(List<InvItem>, Loc)>();
                        List<InvItem> treasure3 = new List<InvItem>();
                        treasure3.Add(new InvItem(234, false, 9, 180));//Lob Wand
                        items.Add((treasure1, new Loc(22, 1)));
                        items.Add((treasure2, new Loc(22, 2)));
                        items.Add((treasure3, new Loc(23, 2)));
                        AddSpecificSpawnPool(layout, items, PR_SPAWN_ITEMS);

                        // place the map status
                        {
                            ShopSecurityState state = new ShopSecurityState();
                            state.Security.Add(GetShopMob("kecleon", "protean", "focus_punch", "shadow_sneak", "incinerate", "thief", new int[] { 1984, 1985, 1988 }, -1), 10);
                            StateMapStatusStep<StairsMapGenContext> statusData = new StateMapStatusStep<StairsMapGenContext>("shop_security", state);
                            layout.GenSteps.Add(PR_FLOOR_DATA, statusData);
                        }

                        // place the mob running the shop
                        {
                            MobSpawn post_mob = GetShopMob("kecleon", "color_change", "synchronoise", "bind", "screech", "thunder_wave", new int[] { 1984, 1985, 1988 }, 0);
                            post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc(21, 1)));
                            SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);

                            PlaceNoLocMobsStep<StairsMapGenContext> mobStep = new PlaceNoLocMobsStep<StairsMapGenContext>(new PresetMultiTeamSpawner<StairsMapGenContext>(post_team));
                            mobStep.Ally = true;
                            layout.GenSteps.Add(PR_SPAWN_MOBS, mobStep);
                        }
                    }

                    //boss area
                    {
                        Rect bossArea = new Rect(34, 4, 10, 10);
                        Loc bossTriggerArea = new Loc(39, 9);
                        Loc lockedTile = new Loc(35, 4);
                        for (int xx = bossArea.X; xx < bossArea.End.X; xx++)
                        {
                            for (int yy = bossArea.Y; yy < bossArea.End.Y; yy++)
                            {
                                if (yy == bossArea.End.Y - 1 && xx < bossArea.X + 5)
                                {

                                }
                                else if (xx == bossArea.X || xx == bossArea.End.X - 1 ||
                                    yy == bossArea.Y || yy == bossArea.End.Y - 1)
                                {
                                    drawStep.Tiles[xx][yy] = new Tile("unbreakable");
                                }
                            }
                        }

                        EffectTile effect2 = new EffectTile("sealed_block", true, lockedTile);
                        ((Tile)drawStep.Tiles[lockedTile.X][lockedTile.Y]).Effect = effect2;

                        //specifically planned enemies
                        MobSpawnState mobSpawnState = new MobSpawnState();
                        {
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID("kyogre", 0, "normal", Gender.Unknown);
                            post_mob.Tactic = "wait_only";
                            post_mob.Level = new RandRange(50);
                            post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc(36, 6) + new Loc(1)));
                            post_mob.SpawnFeatures.Add(new MobSpawnItem(true, 1));
                            post_mob.SpawnFeatures.Add(new MobSpawnUnrecruitable());
                            mobSpawnState.Spawns.Add(post_mob);
                        }
                        {
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID("groudon", 0, "normal", Gender.Unknown);
                            post_mob.Tactic = "wait_only";
                            post_mob.Level = new RandRange(50);
                            post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc(38, 6) + new Loc(1)));
                            post_mob.SpawnFeatures.Add(new MobSpawnItem(true, 1));
                            post_mob.SpawnFeatures.Add(new MobSpawnUnrecruitable());
                            mobSpawnState.Spawns.Add(post_mob);
                        }
                        {
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID("rayquaza", 0, "normal", Gender.Unknown);
                            post_mob.Tactic = "wait_only";
                            post_mob.Level = new RandRange(50);
                            post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc(40, 6) + new Loc(1)));
                            post_mob.SpawnFeatures.Add(new MobSpawnItem(true, 1));
                            post_mob.SpawnFeatures.Add(new MobSpawnUnrecruitable());
                            mobSpawnState.Spawns.Add(post_mob);
                        }

                        EffectTile newEffect = new EffectTile("tile_boss", true, bossTriggerArea + new Loc(1));
                        newEffect.TileStates.Set(mobSpawnState);
                        newEffect.TileStates.Set(new BoundsState(new Rect(bossArea.Start + new Loc(1), bossArea.Size)));
                        ((Tile)drawStep.Tiles[bossTriggerArea.X][bossTriggerArea.Y]).Effect = newEffect;

                        ResultEventState resultEvent = new ResultEventState();
                        resultEvent.ResultEvents.Add(new OpenVaultEvent((new List<Loc>() { lockedTile + new Loc(1) })));
                        newEffect.TileStates.Set(resultEvent);
                    }


                    int patternDiff = 2;
                    int howMany = 5;
                    {
                        EffectTile effect2 = new EffectTile("sealed_door", true, new Loc(40 + 1, 2 + 1));
                        TileListState state = new TileListState();
                        for (int mm = 1; mm < howMany; mm++)
                            state.Tiles.Add(new Loc(40 + patternDiff * mm + 1, 2 + 1));
                        effect2.TileStates.Set(state);
                        effect2.TileStates.Set(new UnlockState(455));
                        ((Tile)drawStep.Tiles[40][2]).Effect = effect2;
                    }
                    for (int nn = 1; nn < howMany; nn++)
                    {
                        EffectTile effect2 = new EffectTile("sealed_block", true, new Loc(40 + patternDiff * nn + 1, 2 + 1));
                        ((Tile)drawStep.Tiles[40 + patternDiff * nn][2]).Effect = effect2;
                    }

                    //tile-activated gates
                    patternDiff = 2;
                    howMany = 3;
                    {
                        EffectTile effect2 = new EffectTile("tile_switch", true, new Loc(50, 4));
                        TileListState state = new TileListState();
                        for (int mm = 1; mm < howMany; mm++)
                            state.Tiles.Add(new Loc(50 + patternDiff * mm + 1, 4 + 1));
                        effect2.TileStates.Set(state);
                        ((Tile)drawStep.Tiles[50][4]).Effect = effect2;
                    }
                    for (int nn = 1; nn < howMany; nn++)
                    {
                        EffectTile effect2 = new EffectTile("sealed_block", true, new Loc(50 + patternDiff * nn, 4));
                        ((Tile)drawStep.Tiles[50 + patternDiff * nn][4]).Effect = effect2;
                    }


                    layout.GenSteps.Add(PR_TILES_GEN, drawStep);

                    //add border
                    layout.GenSteps.Add(PR_TILES_BARRIER, new UnbreakableBorderStep<StairsMapGenContext>(1));

                    layout.GenSteps.Add(PR_EXITS, new StairsStep<StairsMapGenContext, MapGenEntrance, MapGenExit>(new MapGenEntrance(Dir8.Down), new MapGenExit(new EffectTile("stairs_go_up", true))));

                    AddTextureData(layout, "test_dungeon_wall", "test_dungeon_floor", "test_dungeon_secondary", "normal");
                    curTileIndex += 3;

                    structure.Floors.Add(layout);
                }
                #endregion

                //structure.MainExit = new ZoneLoc(2, 0);
                zone.Segments.Add(structure);
            }

            #region REPLAY TEST ZONE
            {
                RangeDictSegment floorSegment = new RangeDictSegment();
                floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Replay Test Zone\n{0}F")));
                int total_floors = 10;

                SpawnList<IGenPriority> shopZoneSpawns = new SpawnList<IGenPriority>();
                //kecleon shop
                {
                    ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                    shop.Personality = 0;
                    shop.SecurityStatus = "shop_security";
                    shop.Items.Add(new MapItem(101, 0, 800), 10);//reviver seed
                    shop.Items.Add(new MapItem(112, 0, 500), 10);//blast seed
                    shop.Items.Add(new MapItem(444, 753, 8000), 10);//poison dust
                    shop.Items.Add(new MapItem(234, 9, 180), 10);//Lob Wand
                    shop.Items.Add(new MapItem(352, 0, 2000), 10);//thunder stone
                    shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                    // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 103 screech : 86 thunder wave
                    shop.StartMob = GetShopMob("kecleon", "color_change", "synchronoise", "bind", "screech", "thunder_wave", new int[] { 1984, 1985, 1988 }, 0);
                    {
                        // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 103 screech : 86 thunder wave
                        shop.Mobs.Add(GetShopMob("kecleon", "color_change", "synchronoise", "bind", "screech", "thunder_wave", new int[] { 1984, 1985, 1988 }, -1), 10);
                        // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 50 disable : 374 fling
                        shop.Mobs.Add(GetShopMob("kecleon", "color_change", "synchronoise", "bind", "disable", "fling", new int[] { 1984, 1985, 1988 }, -1), 10);
                        // 352 Kecleon : 168 protean : 246 ancient power : 425 shadow sneak : 510 incinerate : 168 thief
                        shop.Mobs.Add(GetShopMob("kecleon", "protean", "ancient_power", "shadow_sneak", "incinerate", "thief", new int[] { 1984, 1985, 1988 }, -1), 10);
                        // 352 Kecleon : 168 protean : 332 aerial ace : 421 shadow claw : 60 psybeam : 364 feint
                        shop.Mobs.Add(GetShopMob("kecleon", "protean", "aerial_ace", "shadow_claw", "psybeam", "feint", new int[] { 1984, 1985, 1988 }, -1), 10);
                    }
                    shopZoneSpawns.Add(new GenPriority<GenStep<MapGenContext>>(PR_SHOPS, shop), 10);
                }

                //star treasures
                {
                    ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                    shop.Personality = 1;
                    shop.SecurityStatus = "shop_security";
                    shop.Items.Add(new MapItem(444, 753, 8000), 10);//poison dust
                    shop.Items.Add(new MapItem(352, 0, 2000), 10);//thunder stone
                    shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                    // Cleffa : 98 Magic Guard : 118 Metronome : 47 Sing : 204 Charm : 313 Fake Tears
                    {
                        MobSpawn post_mob = new MobSpawn();
                        post_mob.BaseForm = new MonsterID("cleffa", 0, "normal", Gender.Unknown);
                        post_mob.Tactic = "shopkeeper";
                        post_mob.Level = new RandRange(5);
                        post_mob.Intrinsic = "magic_guard";
                        post_mob.SpecifiedSkills.Add("metronome");
                        post_mob.SpecifiedSkills.Add("sing");
                        post_mob.SpecifiedSkills.Add("charm");
                        post_mob.SpecifiedSkills.Add("fake_tears");
                        post_mob.SpawnFeatures.Add(new MobSpawnDiscriminator(1));
                        post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new BattleScriptEvent("ShopkeeperInteract")));
                        post_mob.SpawnFeatures.Add(new MobSpawnLuaTable("{ Role = \"Shopkeeper\" }"));
                        shop.StartMob = post_mob;
                    }
                    {
                        // 35 Clefairy : 132 Friend Guard : 282 Knock Off : 107 Minimize : 236 Moonlight : 277 Magic Coat
                        shop.Mobs.Add(GetShopMob("clefairy", "friend_guard", "knock_off", "minimize", "moonlight", "magic_coat", new int[] { 973, 976 }, -1), 10);
                        // 36 Clefable : 109 Unaware : 118 Metronome : 500 Stored Power : 343 Covet : 271 Trick
                        shop.Mobs.Add(GetShopMob("clefable", "unaware", "metronome", "stored_power", "covet", "trick", new int[] { 973, 976 }, -1), 10);
                        // 36 Clefable : 98 Magic Guard : 118 Metronome : 213 Attract : 282 Knock Off : 266 Follow Me
                        shop.Mobs.Add(GetShopMob("clefable", "magic_guard", "metronome", "attract", "knock_off", "follow_me", new int[] { 973, 976 }, -1), 10);
                    }
                    shopZoneSpawns.Add(new GenPriority<GenStep<MapGenContext>>(PR_SHOPS, shop), 10);
                }

                //porygon wares
                {
                    ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                    shop.Personality = 2;
                    shop.SecurityStatus = "shop_security";
                    shop.Items.Add(new MapItem(444, 753, 8000), 10);//poison dust
                    shop.Items.Add(new MapItem(352, 0, 2000), 10);//thunder stone
                    shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                    // 137 Porygon : 36 Trace : 97 Agility : 59 Blizzard : 435 Discharge : 94 Psychic
                    shop.StartMob = GetShopMob("porygon", "trace", "agility", "blizzard", "discharge", "psychic", new int[] { 1322, 1323, 1324, 1325 }, 2);
                    {
                        // 474 Porygon-Z : 91 Adaptability : 417 Nasty Plot : 63 Hyper Beam : 435 Discharge : 373 Embargo
                        shop.Mobs.Add(GetShopMob("porygon_z", "adaptability", "nasty_plot", "hyper_beam", "discharge", "embargo", new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                        // 474 Porygon-Z : 91 Adaptability : 160 Conversion : 59 Blizzard : 435 Discharge : 473 Psyshock
                        shop.Mobs.Add(GetShopMob("porygon_z", "adaptability", "conversion", "blizzard", "discharge", "psyshock", new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                        // 474 Porygon-Z : 91 Adaptability : 417 Nasty Plot : 63 Hyper Beam : 435 Discharge : 373 Embargo
                        shop.Mobs.Add(GetShopMob("porygon_z", "adaptability", "nasty_plot", "hyper_beam", "discharge", "embargo", new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                        // 474 Porygon-Z : 91 Adaptability : 417 Nasty Plot : 161 Tri Attack : 247 Shadow Ball : 373 Embargo
                        shop.Mobs.Add(GetShopMob("porygon_z", "adaptability", "nasty_plot", "tri_attack", "shadow_ball", "embargo", new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                        // 474 Porygon-Z : 88 Download : 97 Agility : 473 Psyshock : 324 Signal Beam : 373 Embargo
                        shop.Mobs.Add(GetShopMob("porygon_z", "download", "agility", "psyshock", "signal_beam", "embargo", new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                        // 233 Porygon2 : 36 Trace : 176 Conversion2 : 105 Recover : 60 Psybeam : 324 Signal Beam
                        shop.Mobs.Add(GetShopMob("porygon2", "trace", "conversion_2", "recover", "psybeam", "signal_beam", new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                        // 233 Porygon2 : 36 Trace : 176 Conversion2 : 105 Recover : 60 Psybeam : 435 Discharge
                        shop.Mobs.Add(GetShopMob("porygon2", "trace", "conversion_2", "recover", "psybeam", "discharge", new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                        // 233 Porygon2 : 36 Trace : 176 Conversion2 : 277 Magic Coat : 161 Tri Attack : 97 Agility
                        shop.Mobs.Add(GetShopMob("porygon2", "trace", "conversion_2", "magic_coat", "tri_attack", "agility", new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                    }
                    shopZoneSpawns.Add(new GenPriority<GenStep<MapGenContext>>(PR_SHOPS, shop), 10);
                }

                //bottle shop
                {
                    ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                    shop.Personality = 0;
                    shop.SecurityStatus = "shop_security";
                    shop.Items.Add(new MapItem(444, 753, 8000), 10);//poison dust
                    shop.Items.Add(new MapItem(352, 0, 2000), 10);//thunder stone
                    shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                    // 213 Shuckle : 126 Contrary : 380 Gastro Acid : 564 Sticky Web : 450 Bug Bite : 92 Toxic
                    shop.StartMob = GetShopMob("shuckle", "contrary", "gastro_acid", "sticky_web", "bug_bite", "toxic", new int[] { 1569, 1570, 1571, 1572 }, 0);
                    {
                        // 213 Shuckle : 126 Contrary : 380 Gastro Acid : 564 Sticky Web : 450 Bug Bite : 92 Toxic
                        shop.Mobs.Add(GetShopMob("shuckle", "contrary", "gastro_acid", "sticky_web", "bug_bite", "toxic", new int[] { 1569, 1570, 1571, 1572 }, -1), 10);
                        // 213 Shuckle : 126 Contrary : 230 Sweet Scent : 611 Infestation : 189 Mud-Slap : 522 Struggle Bug
                        shop.Mobs.Add(GetShopMob("shuckle", "contrary", "sweet_scent", "infestation", "mud_slap", "struggle_bug", new int[] { 1569, 1570, 1571, 1572 }, -1), 10);
                        // 213 Shuckle : 126 Contrary : 230 Sweet Scent : 219 Safeguard : 446 Stealth Rock : 249 Rock Smash
                        shop.Mobs.Add(GetShopMob("shuckle", "sturdy", "sweet_scent", "safeguard", "stealth_rock", "rock_smash", new int[] { 1569, 1570, 1571, 1572 }, -1), 10);
                        // 213 Shuckle : 5 Sturdy : 379 Power Trick : 504 Shell Smash : 205 Rollout : 360 Gyro Ball
                        shop.Mobs.Add(GetShopMob("shuckle", "sturdy", "power_trick", "shell_smash", "rollout", "gyro_ball", new int[] { 1569, 1570, 1571, 1572 }, -1), 10);
                        // 213 Shuckle : 5 Sturdy : 379 Power Trick : 450 Bug Bite : 444 Stone Edge : 91 Dig
                        shop.Mobs.Add(GetShopMob("shuckle", "sturdy", "power_trick", "bug_bite", "stone_edge", "dig", new int[] { 1569, 1570, 1571, 1572 }, -1), 10);
                    }

                    shopZoneSpawns.Add(new GenPriority<GenStep<MapGenContext>>(PR_SHOPS, shop), 10);
                }

                //dunsparce finds
                {
                    ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                    shop.Personality = 0;
                    shop.SecurityStatus = "shop_security";
                    shop.Items.Add(new MapItem(444, 753, 8000), 10);//poison dust
                    shop.Items.Add(new MapItem(352, 0, 2000), 10);//thunder stone
                    shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                    // 206 Dunsparce : 32 Serene Grace : 228 Pursuit : 103 Screech : 44 Bite : Secret Power
                    shop.StartMob = GetShopMob("dunsparce", "serene_grace", "pursuit", "screech", "bite", "secret_power", new int[] { 1545, 1546, 1549 }, 0);
                    {
                        // 206 Dunsparce : 32 Serene Grace : 228 Pursuit : 99 Rage : 428 Zen Headbutt : Secret Power
                        shop.Mobs.Add(GetShopMob("dunsparce", "serene_grace", "pursuit", "rage", "zen_headbutt", "secret_power", new int[] { 1545, 1546, 1549 }, -1), 10);
                        // 206 Dunsparce : 32 Serene Grace : 58 Ice Beam : 352 Water Pulse : Secret Power : Ancient Power
                        shop.Mobs.Add(GetShopMob("dunsparce", "serene_grace", "ice_beam", "water_pulse", "secret_power", "ancient_power", new int[] { 1545, 1546, 1549 }, -1), 10);
                        // 206 Dunsparce : 32 Serene Grace : 228 Pursuit : 44 Bite : Secret Power : Ancient Power
                        shop.Mobs.Add(GetShopMob("dunsparce", "serene_grace", "pursuit", "bite", "secret_power", "ancient_power", new int[] { 1545, 1546, 1549 }, -1), 10);
                        // 206 Dunsparce : 32 Serene Grace : 228 Pursuit : 355 Roost : Secret Power : Ancient Power
                        shop.Mobs.Add(GetShopMob("dunsparce", "serene_grace", "pursuit", "roost", "secret_power", "ancient_power", new int[] { 1545, 1546, 1549 }, -1), 10);
                        // 206 Dunsparce : 155 Rattled : 228 Pursuit : 180 Spite : 20 Bind : 281 Yawn
                        shop.Mobs.Add(GetShopMob("dunsparce", "rattled", "pursuit", "spite", "bind", "yawn", new int[] { 1545, 1546, 1549 }, -1), 10);
                        // 206 Dunsparce : 155 Rattled : 137 Glare : 180 Spite : 20 Bind : 506 Hex
                        shop.Mobs.Add(GetShopMob("dunsparce", "rattled", "glare", "spite", "bind", "hex", new int[] { 1545, 1546, 1549 }, -1), 10);
                    }
                    shopZoneSpawns.Add(new GenPriority<GenStep<MapGenContext>>(PR_SHOPS, shop), 10);
                }

                SpreadStepZoneStep shopZoneStep = new SpreadStepZoneStep(new SpreadPlanSpaced(new RandRange(1, 3), new IntRange(2, 10)), shopZoneSpawns);
                floorSegment.ZoneSteps.Add(shopZoneStep);


                //money
                MoneySpawnZoneStep moneySpawnStep = new MoneySpawnZoneStep(PR_RESPAWN_MONEY, new RandRange(50, 100), new RandRange(20));
                floorSegment.ZoneSteps.Add(moneySpawnStep);

                //items
                ItemSpawnZoneStep zoneItemStep = new ItemSpawnZoneStep();
                zoneItemStep.Priority = PR_RESPAWN_ITEM;
                CategorySpawn<InvItem> category = new CategorySpawn<InvItem>();
                category.SpawnRates.SetRange(10, new IntRange(0, 5));
                zoneItemStep.Spawns.Add("uncategorized", category);
                for (int nn = 1; nn < 4; nn++)
                    category.Spawns.Add(new InvItem(nn), new IntRange(0, 5), (nn % 5 + 1) * 10);//all items
                floorSegment.ZoneSteps.Add(zoneItemStep);

                //mobs
                TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                poolSpawn.Priority = PR_RESPAWN_MOB;
                List<string> monsterKeys = DataManager.Instance.DataIndices[DataManager.DataType.Monster].GetOrderedKeys(true);
                for (int xx = 152; xx < 161; xx++)
                {
                    int yy = 1;
                    MobSpawn post_mob = new MobSpawn();
                    post_mob.BaseForm = new MonsterID(monsterKeys[xx], 0, "", Gender.Unknown);
                    post_mob.Intrinsic = "";
                    post_mob.Level = new RandRange(10);
                    post_mob.Tactic = "wander_normal";
                    post_mob.SpawnFeatures.Add(new MobSpawnWeak());
                    if (yy == 0)
                    {
                        StatusEffect sleep = new StatusEffect("sleep");
                        sleep.StatusStates.Set(new CountDownState(-1));
                        MobSpawnStatus status = new MobSpawnStatus();
                        status.Statuses.Add(sleep, 10);
                        post_mob.SpawnFeatures.Add(status);
                    }
                    poolSpawn.Spawns.Add(new TeamMemberSpawn(post_mob, TeamMemberSpawn.MemberRole.Normal), new IntRange(0, total_floors), 10);
                }

                poolSpawn.TeamSizes.Add(1, new IntRange(0, total_floors), 6);
                poolSpawn.TeamSizes.Add(2, new IntRange(0, total_floors), 2);
                poolSpawn.TeamSizes.Add(3, new IntRange(0, 5), 1);
                poolSpawn.TeamSizes.Add(4, new IntRange(0, 5), 1);
                floorSegment.ZoneSteps.Add(poolSpawn);

                ScriptZoneStep scriptZoneStep = new ScriptZoneStep("SpawnMissionNpcFromSV");
                floorSegment.ZoneSteps.Add(scriptZoneStep);


                RandBag<IGenPriority> npcZoneSpawns = new RandBag<IGenPriority>();
                npcZoneSpawns.RemoveOnRoll = true;
                //Generic Dialogue
                {
                    PresetMultiTeamSpawner<ListMapGenContext> multiTeamSpawner = new PresetMultiTeamSpawner<ListMapGenContext>();
                    MobSpawn post_mob = new MobSpawn();
                    post_mob.BaseForm = new MonsterID("misdreavus", 0, "normal", Gender.Unknown);
                    post_mob.Tactic = "slow_wander";
                    post_mob.Level = new RandRange(50);
                    post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_WAIT_0190"))));
                    SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);
                    post_team.Explorer = true;
                    multiTeamSpawner.Spawns.Add(post_team);
                    PlaceRandomMobsStep<ListMapGenContext> randomSpawn = new PlaceRandomMobsStep<ListMapGenContext>(multiTeamSpawner);
                    randomSpawn.Ally = true;
                    npcZoneSpawns.ToSpawn.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_SPAWN_MOBS_EXTRA, randomSpawn));
                }
                //Scripted Dialogue
                {
                    PresetMultiTeamSpawner<ListMapGenContext> multiTeamSpawner = new PresetMultiTeamSpawner<ListMapGenContext>();
                    MobSpawn post_mob = new MobSpawn();
                    post_mob.BaseForm = new MonsterID("delibird", 0, "normal", Gender.Male);
                    post_mob.Tactic = "slow_wander";
                    post_mob.Level = new RandRange(50);
                    post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new BattleScriptEvent("CountTalkTest")));
                    SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);
                    post_team.Explorer = true;
                    multiTeamSpawner.Spawns.Add(post_team);
                    PlaceRandomMobsStep<ListMapGenContext> randomSpawn = new PlaceRandomMobsStep<ListMapGenContext>(multiTeamSpawner);
                    randomSpawn.Ally = true;
                    npcZoneSpawns.ToSpawn.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_SPAWN_MOBS_EXTRA, randomSpawn));
                }
                //Ally Team
                {
                    PresetMultiTeamSpawner<ListMapGenContext> multiTeamSpawner = new PresetMultiTeamSpawner<ListMapGenContext>();
                    SpecificTeamSpawner post_team = new SpecificTeamSpawner();
                    post_team.Explorer = true;
                    {
                        MobSpawn post_mob = new MobSpawn();
                        post_mob.BaseForm = new MonsterID("dialga", 0, "normal", Gender.Unknown);
                        post_mob.Tactic = "stick_together";
                        post_mob.Level = new RandRange(10);
                        post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_FULL_0773"))));
                        post_mob.SpawnFeatures.Add(new MobSpawnFoeConflict());
                        post_team.Spawns.Add(post_mob);
                    }
                    {
                        MobSpawn post_mob = new MobSpawn();
                        post_mob.BaseForm = new MonsterID("palkia", 0, "normal", Gender.Unknown);
                        post_mob.Tactic = "stick_together";
                        post_mob.Level = new RandRange(10);
                        post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_FULL_0781"))));
                        post_team.Spawns.Add(post_mob);
                    }
                    {
                        MobSpawn post_mob = new MobSpawn();
                        post_mob.BaseForm = new MonsterID("giratina", 0, "normal", Gender.Unknown);
                        post_mob.Tactic = "stick_together";
                        post_mob.Level = new RandRange(10);
                        post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_FULL_0783"))));
                        post_team.Spawns.Add(post_mob);
                    }
                    multiTeamSpawner.Spawns.Add(post_team);
                    PlaceRandomMobsStep<ListMapGenContext> randomSpawn = new PlaceRandomMobsStep<ListMapGenContext>(multiTeamSpawner);
                    randomSpawn.Ally = true;
                    npcZoneSpawns.ToSpawn.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_SPAWN_MOBS_EXTRA, randomSpawn));
                }
                SpreadStepZoneStep npcZoneStep = new SpreadStepZoneStep(new SpreadPlanQuota(new RandRange(4, 5), new IntRange(7, total_floors), true), npcZoneSpawns);
                floorSegment.ZoneSteps.Add(npcZoneStep);

                TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                tileSpawn.Priority = PR_RESPAWN_TRAP;
                tileSpawn.Spawns.Add(new EffectTile("trap_explosion", true), new IntRange(0, 5), 10);
                floorSegment.ZoneSteps.Add(tileSpawn);

                SpawnList<IGenPriority> apricornZoneSpawns = new SpawnList<IGenPriority>();
                apricornZoneSpawns.Add(new GenPriority<GenStep<MapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<MapGenContext, MapItem>(new PickerSpawner<MapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(210))))), 10);//plain
                SpreadStepZoneStep apricornStep = new SpreadStepZoneStep(new SpreadPlanSpaced(new RandRange(2, 5), new IntRange(3, 25)), apricornZoneSpawns);//apricorn (variety)
                floorSegment.ZoneSteps.Add(apricornStep);

                SpreadRoomZoneStep evoStep = new SpreadRoomZoneStep(PR_GRID_GEN_EXTRA, PR_ROOMS_GEN_EXTRA, new SpreadPlanSpaced(new RandRange(1, 1), new IntRange(0, 5)));
                List<BaseRoomFilter> evoFilters = new List<BaseRoomFilter>();
                evoFilters.Add(new RoomFilterComponent(true, new ImmutableRoom()));
                evoFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                evoStep.Spawns.Add(new RoomGenOption(new RoomGenEvo<MapGenContext>(), new RoomGenEvo<ListMapGenContext>(), evoFilters), 10);
                floorSegment.ZoneSteps.Add(evoStep);

                for (int ii = 0; ii < total_floors; ii += 5)
                {

                    GridFloorGen layout = new GridFloorGen();

                    AddFloorData(layout, "A02. Base Town.ogg", -1, Map.SightRange.Dark, Map.SightRange.Dark);

                    AddInitGridStep(layout, 5, 4, 11, 11);

                    //construct paths
                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(80);
                    path.BranchRatio = new RandRange(0, 25);

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //cross
                    genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 10), new RandRange(4, 10), new RandRange(2, 6), new RandRange(2, 5)), 10);
                    //round
                    genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                    //square
                    genericRooms.Add(new RoomGenSquare<MapGenContext>(new RandRange(4, 7), new RandRange(4, 7)), 10);
                    //bump
                    genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 7), new RandRange(4, 7), new RandRange(20, 30)), 10);
                    //evo
                    genericRooms.Add(new RoomGenEvo<MapGenContext>(), 10);
                    //blocked
                    genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(6, 9), new RandRange(6, 9), new RandRange(2, 3), new RandRange(2, 3)), 10);
                    //water ring
                    if (ii < 5)
                    {
                        RoomGenWaterRing<MapGenContext> waterRing = new RoomGenWaterRing<MapGenContext>(new Tile("water"), new RandRange(1, 4), new RandRange(1, 4), 3);
                        waterRing.Treasures.Add(new MapItem(2), 10);
                        waterRing.Treasures.Add(new MapItem(3), 10);
                        waterRing.Treasures.Add(new MapItem(6), 10);
                        genericRooms.Add(waterRing, 10);
                    }
                    //Guarded Cave
                    if (ii < 5)
                    {
                        RoomGenGuardedCave<MapGenContext> guarded = new RoomGenGuardedCave<MapGenContext>();
                        //treasure
                        guarded.Treasures.RandomSpawns.Add(new MapItem(100), 10);
                        guarded.Treasures.RandomSpawns.Add(new MapItem(101), 10);
                        guarded.Treasures.RandomSpawns.Add(new MapItem(114), 10);
                        guarded.Treasures.SpawnAmount = 6;
                        //guard
                        MobSpawn spawner = new MobSpawn();
                        spawner.BaseForm = new MonsterID("dragonite", 0, "", Gender.Unknown);
                        spawner.Level = new RandRange(80);
                        spawner.Tactic = "go_after_foes";
                        //status sleep
                        StatusEffect sleep = new StatusEffect("sleep");
                        sleep.StatusStates.Set(new CountDownState(-1));
                        MobSpawnStatus status = new MobSpawnStatus();
                        status.Statuses.Add(sleep, 10);
                        spawner.SpawnFeatures.Add(status);
                        guarded.GuardTypes.Add(spawner, 10);
                        genericRooms.Add(guarded, 3);
                    }
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(100), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(30, 100));

                    AddDrawGridSteps(layout);

                    AddStairStep(layout, false);

                    AddWaterSteps(layout, "abyss", new RandRange(ii * 25), false);

                    //Tilesets
                    AddTextureData(layout, "test_dungeon_wall", "test_dungeon_floor", "test_dungeon_secondary", "ghost");

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder", false);//wonder tile
                    AddTrapsSteps(layout, new RandRange(20));

                    //money
                    AddMoneyData(layout, new RandRange(2, 4));

                    //items
                    AddItemData(layout, new RandRange(4, 6), 25);

                    //mobs
                    AddRespawnData(layout, 12, 50);

                    AddEnemySpawnData(layout, 100, new RandRange(5, 8));

                    floorSegment.Floors.SetRange(layout, new IntRange(ii, ii+5));
                }
                //floorSegment.MainExit = new ZoneLoc(-1, 0);
                zone.Segments.Add(floorSegment);
            }
            #endregion

            {
                LayeredSegment staticSegment = new LayeredSegment();
                LoadGen layout = new LoadGen();
                MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                startGen.MapID = "guildmaster_summit";
                layout.GenSteps.Add(PR_TILES_INIT, startGen);
                staticSegment.Floors.Add(layout);
                zone.Segments.Add(staticSegment);
            }

            for (int jj = 0; jj < MapInfo.MapNames.Length; jj++)
                zone.GroundMaps.Add(MapInfo.MapNames[jj]);
        }

        #region GUILDMASTER ISLAND
        static void FillHubZone(ZoneData zone)
        {
            zone.Name = new LocalText("Guildmaster Island");

            //flavour: explorer badges prevent team members from being separated in mystery dungeons
            //saving is at a statue; disabled in roguelocke
            //drifblim handle gondolas (price to ride = difference in restore price (only upwards)); disabled in roguelocke
            //bank is only at base camp
            //adventures in normal mode are each individual leg
            //normal mode has no reserve member EXP
            //first attempt will ALWAYS be in roguelocke mode
            //in roguelocke mode only, an apricorn is given at the start.

            //Legendary birds: summonable
            //Legendary beasts: obtainable by a game of tag
            //Legendary golems + gigas: regigigas challenge
            //Legendary lake trio: rescued from thieves? summonable?
            //Mewtwo: cave of solace
            //Ho-oh/Lugia: the sky?
            //Groudon: summoned by thieves?
            //Kyogre: summoned by thieves?
            //Rayquaza: the sky
            //latis: secret garden
            //mew: happy new year's
            //celebi: set the clock back
            //jirachi: leave the game on for 7 days and 7 nights
            //deoxys:
            //manaphy:
            //phione:
            //shaymin: needed for reaching the sky...
            //cresselia: dream dungeon?
            //darkrai: dream dungeon?
            //heatran: cave stop?
            //giratina: at the bottom of the abyss
            //dialga: dimensional dungeon
            //palkia: dimensional dungeon
            //arceus: dimensional dungeon?

            //-----------------Home Base
            //Guildmaster Trail
            //Tropical Path
            //Shimmering Bay
            //Trainee's Fault
            //Overgrown Wilds
            //Cave of Whispers
            //-----------------Clearing Camp (storage, assembly)
            //exit is blocked by a snorlax; no one can figure out how to wake it; you have 3 rounds of 3 choices
            //NPCs nearby provide hints
            //talk about how no one's made it to the top
            //allude to the Secret Garden
            //talk about apricorns and recruitment
            //4-5 NPCs in total
            //Faded Trail - Watch out for Shinx's intimidate
            //Windy Valley
            //Ambush Forest - meet outlaws and this is their base of operations
            //Secret Garden
            //-----------------Cliff Camp (storage, assembly)
            //relic camp is alluded to; "some teams have gone even farther... but we haven't had any contact with them"
            //monster in the cave is alluded to
            //Talk about the flying pokemon and their weaknesses
            //And by the way, watch out for the (notably dangerous pokemon here)
            //talk about alternate routes; an explorer is angry that a rival explorer knows a shortcut but won't share; change this when overgrown wilds is unlocked
            //5-7 npcs in total
            //talk about item uses
            //Flyaway Cliffs - watch out for sky attack
            //Igneous Tunnel
            //Treacherous Mountain - meet outlaws and this is where they stash their treasures
            //Wayward Snow Path
            //-----------------Canyon Camp (storage, assembly, summon altar?)
            //"long ago a tower was built on the side of the mountain.  ages passed, and the tower collapsed.  now the remains cover the path up to the summit"
            //And by the way, watch out for the (notably dangerous pokemon here)
            //Dispute occurs here, intro to outlaws?  creates a reason to go after them in the forsaken desert?
            //talk about traps and disarming them
            //talk about dark dungeons; degrees of darkness
            //rhydon offers to create a statue for you if you can defeat them
            //Thunderstruck Pass - watch out for weather tactics
            //Forsaken Desert - meet thieves (cacturne, hippowdon, skarmory)
            //Lunar Range
            //Relic Tower
            //-----------------Cave Shelter (assembly, storage) - 1 - Cave Camp; boss battle upon entering (Dyed in Blood) (natural terror)
            //And by the way, watch out for the (notably dangerous pokemon here)
            //"you don't think this can get any worse, do you?"
            //ambushed by a crapload of angered pokemon
            //Illusion Ridge
            //Cave of Solace
            //Royal Halls
            //-----------------Path to the Summit (assembly, storage) - contact from the guildmasters; very few others are here
            //"going to Champion's Road?" [Yes/No]
            //"You came all the way here... just to turn back?"
            //and by the way, watch out for the EVERYTHING
            //Champion's Road
            //The Sky
            //-----------------Summit


            //Outrage/Rest + heal bell

            //someone to abuse moves:
            //elemental punches
            //drill peck
            //smog
            //poison gas
            //lovely kiss
            //sweet kiss
            //sweet scent
            //ancient power
            //shadow ball
            //charge
            //signal beam
            //dragon dance
            //swagger/flatter
            //refresh
            //meteor mash
            //fake tears
            //bullet seed/rock blast
            //power trick
            //heart stamp


            //bide
            //leech seed
            //string shot
            //wrap/bind
            //rock tomb
            //beat up
            //safeguard
            //spite
            //curse
            //incinerate
            //perish song
            //swords dance
            //yawn
            //knock off
            //echoed voice
            //shadow sneak
            //psychic


            //disable
            //thunder wave
            //toxic
            //mirror move
            //egg bomb
            //barrage
            //transform
            //psywave
            //Tri Attack
            //triple kick
            //mind reader
            //protect
            //belly drum
            //destiny bond
            //mean look
            //mirror coat
            //extreme speed
            //torment
            //will-o-wisp
            //brick break
            //imprison
            //grudge
            //snatch
            //teeter dance
            //poison fang
            //air cutter
            //metal sound
            //grass whistle
            //water pulse
            //hammer arm
            //gyro ball
            //brine
            //feint
            //pluck/bug bite
            //close combat
            //embargo
            //heal block
            //gastro acid
            //seed bomb
            //air slash
            //drain punch
            //vacuum wave
            //focus blast/ice shard
            //elemental fangs
            //rock climb
            //iron head
            //stone edge
            //captivate
            //grass knot
            //chatter
            //charge beam
            //wood hammer
            //wonder room
            //psyshock
            //telekinesis
            //magic room
            //flame charge
            //coil
            //clear smog
            //scald
            //shell smash
            //hex
            //acrobatics
            //struggle bug
            //bulldoze
            //drill run
            //sticky web
            //phantom force
            //freeze-dry
            //disarming voice
            //draining kiss
            //terrain moves
            //confide
            //venom drench
            //dazzling gleam
            //power-up punch

            //revenge
            //payback
            //punishment
            //avalanche
            //flail (tank/endure)
            //smelling salts/wake-up-slap (wth status inducer)
            //assurance (groups?)
            //stored power (while adding buffs)
            //fell stinger (aim for allies!)

            //vice grip (rooms)
            //razor wind (rooms)
            //take down (stantler?)
            //leer (early-game)
            //roar (good with tackle)
            //sing (early-mid-game; other status moves need to be present)
            //supersonic (halls)
            //petal dance (tank)
            //dragon rage (tank; rooms)
            //agility (in groups)
            //rage (tank)
            //night shade/seismic toss
            //screech (halls)
            //confuse ray (distance)
            //light screen (in groups)
            //haze (proper use)
            //reflect (in groups)
            //focus energy (projectiles)
            //clamp (in groups)
            //skull bash (rooms)
            //glare (distance)
            //sky attack (open area)
            //splash (floor dragons)
            //rest (tank)
            //substitute (rooms)
            //spider web (groups)
            //nightmare (rooms)
            //cotton spore (group)
            //outrage (tank; rooms)
            //heal bell (monster houses)
            //present (groups)
            //magnitude (groups)
            //dragon breath (halls)
            //encore (groups)
            //pursuit (in groups)
            //iron tail (halls; showcase terrain destruction)
            //morning sun (groups)
            //synthesis (pair with leech seed?)
            //moonlight (groups)
            //twister (groups)
            //future sight (as training before xatu)
            //uproar (with sleeping foes)
            //memento (late-game)
            //taunt (mid-late game)
            //helping hand (mid-game)
            //wish (groups)
            //magic coat (later game)
            //metal burst (rooms)
            //hyper voice (training before xatu)
            //crush claw (early-mid game)
            //astonish (earlygame)
            //cosmic power (groups)
            //howl (groups)
            //u-turn (with backup ally)
            //accupressure (with an ally)

            //lucky chant (groups)
            //discharge (+ground type allies later)
            //lava plume (+fire type allies later)
            //wide guard (groups)
            //foul play (with buffs?/tank)

            //after you (groups)
            //quash (groups)
            //frost breath (halls)
            //dragon tail (rooms)
            //snarl (rooms)
            //eerie impulse (rooms)
            //magnetic flux (groups)
            //draco meteor (groups)
            //magnet rise (groups)
            //rage powder (groups)
            //round (groups)
            //quick guard (groups)
            //ally switch (groups)

            //super fang
            //healing wish (properly in groups; someone else cover it)
            //aqua ring (groups)

            //self-destruct/explosion (properly)
            //teleport (metal slimes)
            //retaliate (rooms; properly)

            //wring out (properly; at high HP)
            //dream eater (with a sleeper)
            //rock smash (tunnel ants)
            //psycho shift (properly; on tanks; in groups)
            //pain split (properly)
            //final gambit
            //baton pass (properly)
            //psych up (properly; with something that buffs)
            //focus punch (properly; charge up when far away)
            //endeavor

            //thief (needs a good means of escape; needs proper use)
            //covet (needs proper use; run away when having it, or keep begging if there exists no path)
            //trick (do once, then run away)
            //skill swap (only once)
            //entrainment

            //sleep talk (with a natural sleeper)
            //nature power (properly)
            //me first (properly)
            //copycat (properly)
            //stockpile/spit up/swallow (properly)

            //fake out
            //sucker punch

            //switcheroo (properly in groups)
            //heal pulse (friendly)
            //bestow (friendly; just leave afterwards// later on, have it bestow bad items)
            //spikes/toxic spikes/stealth rock (properly)
            //super fang + brine

            //someone to abuse abilities:
            //defiant
            //toxic boost/flare boost
            //justified
            //rattled
            //suction cups
            //hustle
            //guts/quick feet
            //liquid ooze
            //poison heal
            //magic guard

            //color change
            //wonder guard
            //levitate
            //magnet pull
            //pressure
            //pickup/etc
            //truant
            //speed boost
            //sand stream/snow warning

            //stench
            //sturdy
            //damp
            //sand veil/snow cloak (in weather)
            //volt absorb/water absorb/flash fire
            //compound eyes
            //static/etc
            //intimidate
            //shadow tag
            //rough skin
            //synchronize
            //clear body
            //natural cure
            //serene grace
            //swift swim/chlorophyll
            //illuminate
            //trace
            //huge power/pure power
            //soundproof
            //rain dish/ice body
            //thick fat
            //forecast
            //sticky hold
            //shed skin
            //overgrow/etc
            //rock head
            //tangled feet
            //gluttony
            //unburden (with acrobatics?)
            //simple
            //dry skin
            //download
            //skill link
            //solar power
            //normalize
            //no guard
            //technician
            //leaf guard
            //klutz
            //mold breaker
            //aftermath
            //anticipation
            //forewarn
            //unaware
            //tinted lens
            //filter/solid rock
            //scrappy
            //storm drain/lightning rod
            //frisk
            //flower gift
            //pickpocket
            //contrary
            //unnerve
            //cursed body
            //healer
            //friend guard
            //weak armor
            //multiscale
            //harvest
            //telepathy
            //overcoat
            //regenerator
            //analytic
            //infiltrator
            //moxie
            //magic bounce
            //sap sipper
            //prankster
            //competitive


            for (int jj = 0; jj < MapInfo.MapNames.Length; jj++)
                zone.GroundMaps.Add(MapInfo.MapNames[jj]);

            LayeredSegment staticSegment = new LayeredSegment();
            for (int jj = 0; jj < MapInfo.MapNames.Length; jj++)
            {
                LoadGen layout = new LoadGen();
                MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                startGen.MapID = MapInfo.MapNames[jj];
                layout.GenSteps.Add(PR_TILES_INIT, startGen);
                MapEffectStep<MapLoadContext> noRescue = new MapEffectStep<MapLoadContext>();
                noRescue.Effect.OnMapRefresh.Add(0, new MapNoRescueEvent());
                layout.GenSteps.Add(PR_FLOOR_DATA, noRescue);
                staticSegment.Floors.Add(layout);
            }

            zone.Segments.Add(staticSegment);
        }
        #endregion

        #region TRAINING MAZE
        static void FillTrainingMaze(ZoneData zone)
        {
            zone.Name = new LocalText("Training Maze");
            zone.Level = 10;
            zone.LevelCap = true;
            zone.BagRestrict = 0;
            zone.MoneyRestrict = true;






            //What can be placed in info pages?
            //put a "Help" option in Others
            //Controls
            //[Z]+Direction Run
            //[Z] Attack
            //[Z]+[X] Skip Turn
            //[S]+Direction Change Direction
            //[D]+Direction Move Diagonally
            //[A]+[S]/[D]/[Z]/[X] Use a Move
            //[C] Team Mode
            //[1][2][3][4] Change Leader
            //[Backspace] Toggle Minimap
            //Menu Hotkeys
            //[ESC] Main Menu
            //[TAB] Message Log
            //[Q] Moves Menu
            //[W] Items Menu
            //[S] Sort Items (in the Items menu)
            //[E] Tactics Menu
            //[R] Team Menu
            //Type Chart
            //Type Immunities
            //\u8226Fire-types are immune to being Burned.
            //\u8226Electric-types are immune to being Paralyzed.
            //\u8226Ice-types are immune to being Frozen.
            //\u8226Grass-types are immune to being Seeded.
            //\u8226Ghost-types are immune to being Immobilized.
            //\u8226Steel-types are immune to being Poisoned.

            DictionarySegment structure = new DictionarySegment();
            structure.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Training Maze\nLv. {0}")));

            for (int nn = 0; nn < 4; nn++)
            {
                int floor_level = 5;
                if (nn == 1)
                    floor_level = 10;
                else if (nn == 2)
                    floor_level = 25;
                else if (nn == 3)
                    floor_level = 50;

                StairsFloorGen layout = new StairsFloorGen();
                
                AddFloorData(layout, "B02. Demonstration 2.ogg", -1, Map.SightRange.Dark, Map.SightRange.Dark);

                string[] level = new string[]  {"#######################################################################################################################################################################################",
                                                    "#########=............................=####=............................=####=............................=####=............................=####=............................=########",
                                                    "#########.....a..a...a..a..............####...a+a+a+a....+a+......+a+....####....a...a....a...a............####....(...)....?.`.?....!...!...####...........a.a..a.a...........########",
                                                    "#########..............................####..............................####..............................####..............$.$......a.a....####..............................########",
                                                    "#A..B####..............................####..............................####..............................####..../.|.!....`.?.`....!.:.!...####...##....................##...########",
                                                    "#0..1####....a..a..a..a..a....a..a..a..####...a+a+a+a....+a+......+a+....####....a...a....a...a....a.a.a...####..............$.$......a.a....####...##.....##########.....##...########",
                                                    "#....####..............................####..............................####..............................####....%...&....?.`.?....!...!...####..............................########",
                                                    "#....####......E........F........G.....####......H........I........J.....####......K........L........M.....####......N........O........P.....####....Q......R......S......T....####..U#",
                                                    "#..@.................................................................................................................................................................................4#",
                                                    "#....####..............................####..............................####..............................####..............................####....~.~.~.~......~~~~~~~~~~~..####...#",
                                                    "#C..D####....*****....*****....*****...####..****.****..**.**.....***....####...*..*..*..*..*..*..*..*..*..####...*.*.*.*.*.*.+.*.*.*.*.*.*..####...~.~.~.~.~.....~~~~~~~~~~~..########",
                                                    "#2..3####=..............*......*****..=####=.****.****..**.**.....***...=####=............................=####=............................=####=...~.~.~.~......~~..***..~~.=########",
                                                    "#######################################################################################################################################################################################"};

                InitTilesStep<StairsMapGenContext> startStep = new InitTilesStep<StairsMapGenContext>();
                int width = level[0].Length;
                int height = level.Length;
                startStep.Width = width;
                startStep.Height = height;

                layout.GenSteps.Add(PR_TILES_INIT, startStep);

                SpecificTilesStep<StairsMapGenContext> drawStep = new SpecificTilesStep<StairsMapGenContext>();
                drawStep.Offset = new Loc(0);

                drawStep.Tiles = new Tile[width][];
                for (int ii = 0; ii < width; ii++)
                {
                    drawStep.Tiles[ii] = new Tile[height];
                    for (int jj = 0; jj < height; jj++)
                    {
                        if (level[jj][ii] == '#')
                            drawStep.Tiles[ii][jj] = new Tile("wall");
                        else if (level[jj][ii] == '~')
                            drawStep.Tiles[ii][jj] = new Tile("water");
                        else
                        {
                            EffectTile effect = new EffectTile();
                            if (level[jj][ii] == '=') //Reset Tile
                                effect = new EffectTile("tile_reset", true);
                            else if (level[jj][ii] == '+') //Wonder Tile
                                effect = new EffectTile("tile_wonder", true);
                            else if (level[jj][ii] == '%') //Chestnut
                                effect = new EffectTile("trap_chestnut", true);
                            else if (level[jj][ii] == '&') //Poison
                                effect = new EffectTile("trap_poison", true);
                            else if (level[jj][ii] == '/') //Mud
                                effect = new EffectTile("trap_mud", true);
                            else if (level[jj][ii] == '(') //Sticky Trap
                                effect = new EffectTile("trap_sticky", true);
                            else if (level[jj][ii] == ')') //PP-Leech Trap
                                effect = new EffectTile("trap_pp_leech", true);
                            else if (level[jj][ii] == '|') //Gust Trap
                                effect = new EffectTile("trap_gust", true);
                            else if (level[jj][ii] == '!') //Self Destruct
                                effect = new EffectTile("trap_self_destruct", true);
                            else if (level[jj][ii] == '?') //Hidden Sticky
                                effect = new EffectTile("trap_sticky", false);
                            else if (level[jj][ii] == '$') //Hidden Mud
                                effect = new EffectTile("trap_mud", false);
                            else if (level[jj][ii] == '`') //Hidden Chestnut
                                effect = new EffectTile("trap_chestnut", false);
                            else if (level[jj][ii] == ':') //Trigger
                                effect = new EffectTile("trap_trigger", true);
                            else if (level[jj][ii] == '0') //Lv5
                            {
                                effect = new EffectTile("stairs_go_down", true);
                                effect.TileStates.Set(new DestState(new SegLoc(0, 5 - 1), false));
                            }
                            else if (level[jj][ii] == '1') //Lv10
                            {
                                effect = new EffectTile("stairs_go_down", true);
                                effect.TileStates.Set(new DestState(new SegLoc(0, 10 - 1), false));
                            }
                            else if (level[jj][ii] == '2') //Lv25
                            {
                                effect = new EffectTile("stairs_go_down", true);
                                effect.TileStates.Set(new DestState(new SegLoc(0, 25 - 1), false));
                            }
                            else if (level[jj][ii] == '3') //Lv50
                            {
                                effect = new EffectTile("stairs_go_down", true);
                                effect.TileStates.Set(new DestState(new SegLoc(0, 50 - 1), false));
                            }
                            else if (level[jj][ii] == '4') //Exit
                            {
                                effect = new EffectTile("stairs_go_up", true);
                                effect.TileStates.Set(new DestState(SegLoc.Invalid, false));
                            }
                            else if (level[jj][ii] == 'A') //Sign: Level 5
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_LV_005")));
                            }
                            else if (level[jj][ii] == 'B') //Sign: Level 10
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_LV_010")));
                            }
                            else if (level[jj][ii] == 'C') //Sign: Level 25
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_LV_025")));
                            }
                            else if (level[jj][ii] == 'D') //Sign: Level 50
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_LV_050")));
                            }
                            else if (level[jj][ii] == 'E') //Sign: 1.1
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_MOVES"), new LocalFormatControls("SIGN_TUTORIAL_1_1", FrameInput.InputType.Skills)));
                            }
                            else if (level[jj][ii] == 'F') //Sign: 1.2
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_TYPES"), new LocalFormatControls("SIGN_TUTORIAL_1_2")));
                            }
                            else if (level[jj][ii] == 'G') //Sign: 1.3
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_TYPES"), new LocalFormatControls("SIGN_TUTORIAL_1_3")));
                            }
                            else if (level[jj][ii] == 'H') //Sign: 2.1
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_CATEGORY"), new LocalFormatControls("SIGN_TUTORIAL_2_1")));
                            }
                            else if (level[jj][ii] == 'I') //Sign: 2.2
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_HIT_RATE"), new LocalFormatControls("SIGN_TUTORIAL_2_2")));
                            }
                            else if (level[jj][ii] == 'J') //Sign: 2.3
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_MOVEMENT_SPEED"), new LocalFormatControls("SIGN_TUTORIAL_2_3")));
                            }
                            else if (level[jj][ii] == 'K') //Sign: 3.1
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_STATUS"), new LocalFormatControls("SIGN_TUTORIAL_3_1")));
                            }
                            else if (level[jj][ii] == 'L') //Sign: 3.2
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_STATUS"), new LocalFormatControls("SIGN_TUTORIAL_3_2")));
                            }
                            else if (level[jj][ii] == 'M') //Sign: 3.3
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_ABILITY"), new LocalFormatControls("SIGN_TUTORIAL_3_3")));
                            }
                            else if (level[jj][ii] == 'N') //Sign: 4.1
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_TRAP"), new LocalFormatControls("SIGN_TUTORIAL_4_1")));
                            }
                            else if (level[jj][ii] == 'O') //Sign: 4.2
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_TRAP"), new LocalFormatControls("SIGN_TUTORIAL_4_2", FrameInput.InputType.Attack)));
                            }
                            else if (level[jj][ii] == 'P') //Sign: 4.3
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_TRAP"), new LocalFormatControls("SIGN_TUTORIAL_4_3")));
                            }
                            else if (level[jj][ii] == 'Q') //Sign: 5.1
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_MOVEMENT"), new LocalFormatControls("SIGN_TUTORIAL_5_1", FrameInput.InputType.Turn, FrameInput.InputType.Diagonal)));
                            }
                            else if (level[jj][ii] == 'R') //Sign: 5.2
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_TURNS"), new LocalFormatControls("SIGN_TUTORIAL_5_2", FrameInput.InputType.Run, FrameInput.InputType.Attack)));
                            }
                            else if (level[jj][ii] == 'S') //Sign: 5.3
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_TEAM"), new LocalFormatControls("SIGN_TUTORIAL_5_3", FrameInput.InputType.LeaderSwap1, FrameInput.InputType.LeaderSwap2, FrameInput.InputType.LeaderSwap3, FrameInput.InputType.LeaderSwap4)));
                            }
                            else if (level[jj][ii] == 'T') //Sign: 5.4
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_TEAM"), new LocalFormatControls("SIGN_TUTORIAL_5_4", FrameInput.InputType.TeamMode)));
                            }
                            else if (level[jj][ii] == 'U') //Sign: Exit
                            {
                                effect = new EffectTile("sign", true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_EXIT")));
                            }
                            Tile tile = new Tile("floor");
                            tile.Effect = effect;
                            drawStep.Tiles[ii][jj] = tile;
                        }
                    }
                }


                layout.GenSteps.Add(PR_TILES_GEN, drawStep);

                //add border
                layout.GenSteps.Add(PR_TILES_BARRIER, new UnbreakableBorderStep<StairsMapGenContext>(1));

                layout.GenSteps.Add(PR_FLOOR_DATA, new MapExtraStatusStep<StairsMapGenContext>("mercy_revive"));

                ActiveEffect activeEffect = new ActiveEffect();
                activeEffect.OnMapStarts.Add(-15, new PrepareLevelEvent(floor_level));
                MapEffectStep<StairsMapGenContext> mapEvents = new MapEffectStep<StairsMapGenContext>(activeEffect);
                layout.GenSteps.Add(PR_FLOOR_DATA, mapEvents);


                //Entrance
                List<(MapGenEntrance, Loc)> entrances = new List<(MapGenEntrance, Loc)>();
                entrances.Add((new MapGenEntrance(Dir8.Down), new Loc(3, 8)));
                AddSpecificSpawn(layout, entrances, PR_EXITS);



                //items
                List<(InvItem, Loc)> items = new List<(InvItem, Loc)>();

                items.Add((new InvItem(210), new Loc(13, 10)));//Plain Apricorn
                items.Add((new InvItem(210), new Loc(14, 10)));//Plain Apricorn
                items.Add((new InvItem(210), new Loc(15, 10)));//Plain Apricorn
                items.Add((new InvItem(210), new Loc(16, 10)));//Plain Apricorn
                items.Add((new InvItem(210), new Loc(17, 10)));//Plain Apricorn

                items.Add((new InvItem(216), new Loc(24, 11)));//White Apricorn
                items.Add((new InvItem(215), new Loc(22, 10)));//Red Apricorn
                items.Add((new InvItem(211), new Loc(23, 10)));//Blue Apricorn
                items.Add((new InvItem(212), new Loc(24, 10)));//Green Apricorn
                items.Add((new InvItem(217), new Loc(25, 10)));//Yellow Apricorn
                items.Add((new InvItem(213), new Loc(26, 10)));//Brown Apricorn

                items.Add((new InvItem(451), new Loc(31, 10)));//Assembly Box
                items.Add((new InvItem(451), new Loc(32, 10)));//Assembly Box
                items.Add((new InvItem(451), new Loc(33, 10)));//Assembly Box
                items.Add((new InvItem(451), new Loc(34, 10)));//Assembly Box
                items.Add((new InvItem(451), new Loc(35, 10)));//Assembly Box
                items.Add((new InvItem(450), new Loc(31, 11)));//Link Box
                items.Add((new InvItem(450), new Loc(32, 11)));//Link Box
                items.Add((new InvItem(450), new Loc(33, 11)));//Link Box
                items.Add((new InvItem(450), new Loc(34, 11)));//Link Box
                items.Add((new InvItem(450), new Loc(35, 11)));//Link Box



                items.Add((new InvItem(175), new Loc(45, 10)));//X Attack
                items.Add((new InvItem(175), new Loc(46, 10)));//X Attack
                items.Add((new InvItem(175), new Loc(45, 11)));//X Attack
                items.Add((new InvItem(175), new Loc(46, 11)));//X Attack

                items.Add((new InvItem(176), new Loc(47, 10)));//X Defense
                items.Add((new InvItem(176), new Loc(48, 10)));//X Defense
                items.Add((new InvItem(176), new Loc(47, 11)));//X Defense
                items.Add((new InvItem(176), new Loc(48, 11)));//X Defense

                items.Add((new InvItem(177), new Loc(50, 10)));//X Sp. Atk
                items.Add((new InvItem(177), new Loc(51, 10)));//X Sp. Atk
                items.Add((new InvItem(177), new Loc(50, 11)));//X Sp. Atk
                items.Add((new InvItem(177), new Loc(51, 11)));//X Sp. Atk

                items.Add((new InvItem(178), new Loc(52, 10)));//X Sp. Def
                items.Add((new InvItem(178), new Loc(53, 10)));//X Sp. Def
                items.Add((new InvItem(178), new Loc(52, 11)));//X Sp. Def
                items.Add((new InvItem(178), new Loc(53, 11)));//X Sp. Def

                items.Add((new InvItem(180), new Loc(56, 10)));//X Accuracy
                items.Add((new InvItem(180), new Loc(57, 10)));//X Accuracy
                items.Add((new InvItem(180), new Loc(56, 11)));//X Accuracy
                items.Add((new InvItem(180), new Loc(57, 11)));//X Accuracy

                items.Add((new InvItem(278), new Loc(59, 10)));//All-Dodge Orb
                items.Add((new InvItem(278), new Loc(60, 10)));//All-Dodge Orb
                items.Add((new InvItem(278), new Loc(59, 11)));//All-Dodge Orb
                items.Add((new InvItem(278), new Loc(60, 11)));//All-Dodge Orb

                items.Add((new InvItem(179), new Loc(66, 10)));//X Speed
                items.Add((new InvItem(179), new Loc(67, 10)));//X Speed
                items.Add((new InvItem(179), new Loc(68, 10)));//X Speed
                items.Add((new InvItem(179), new Loc(66, 11)));//X Speed
                items.Add((new InvItem(179), new Loc(67, 11)));//X Speed
                items.Add((new InvItem(179), new Loc(68, 11)));//X Speed

                items.Add((new InvItem(12), new Loc(80, 10)));//Lum Berry
                items.Add((new InvItem(12), new Loc(83, 10)));//Lum Berry
                items.Add((new InvItem(12), new Loc(86, 10)));//Lum Berry
                items.Add((new InvItem(12), new Loc(89, 10)));//Lum Berry
                items.Add((new InvItem(12), new Loc(92, 10)));//Lum Berry
                items.Add((new InvItem(12), new Loc(95, 10)));//Lum Berry
                items.Add((new InvItem(12), new Loc(98, 10)));//Lum Berry
                items.Add((new InvItem(12), new Loc(101, 10)));//Lum Berry
                items.Add((new InvItem(12), new Loc(104, 10)));//Lum Berry

                items.Add((new InvItem(10), new Loc(114, 10)));//Oran Berry
                items.Add((new InvItem(10), new Loc(116, 10)));//Oran Berry
                items.Add((new InvItem(11), new Loc(118, 10)));//Leppa Berry
                items.Add((new InvItem(11), new Loc(120, 10)));//Leppa Berry
                items.Add((new InvItem(263), new Loc(122, 10)));//Cleanse Orb
                items.Add((new InvItem(263), new Loc(124, 10)));//Cleanse Orb
                items.Add((new InvItem(260), new Loc(126, 11)));//Revival Orb
                items.Add((new InvItem(263), new Loc(128, 10)));//Cleanse Orb
                items.Add((new InvItem(263), new Loc(130, 10)));//Cleanse Orb
                items.Add((new InvItem(11), new Loc(132, 10)));//Leppa Berry
                items.Add((new InvItem(11), new Loc(134, 10)));//Leppa Berry
                items.Add((new InvItem(10), new Loc(136, 10)));//Oran Berry
                items.Add((new InvItem(10), new Loc(138, 10)));//Oran Berry

                items.Add((new InvItem(303), new Loc(166, 11)));//Mobile Scarf
                items.Add((new InvItem(312), new Loc(167, 11)));//Shell Bell
                items.Add((new InvItem(318), new Loc(168, 11)));//Choice Band

                AddSpecificSpawn(layout, items, PR_SPAWN_ITEMS);


                //enemies

                PresetMultiTeamSpawner<StairsMapGenContext> presetMultiSpawner = new PresetMultiTeamSpawner<StairsMapGenContext>();
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(13, 5), "sentret", "keen_eye", "tackle", "", "", "", floor_level));// Sentret : Tackle
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(16, 5), "chikorita", "", "razor_leaf", "", "", "", floor_level));// Chikorita : Razor Leaf
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(19, 5), "cyndaquil", "", "ember", "", "", "", floor_level));// Cyndaquil : Ember
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(22, 5), "totodile", "", "water_gun", "", "", "", floor_level));// Totodile : Water Gun
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(25, 5), "pikachu", "run_away", "thunder_shock", "", "", "", floor_level));// Pachirisu : Thunder Shock
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(14, 2), "sandshrew", "", "sand_tomb", "", "", "", floor_level));// Sandshrew : Sand Tomb
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(17, 2), "bonsly", "rock_head", "rock_throw", "", "", "", floor_level));// Bonsly : Rock Throw
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(21, 2), "taillow", "", "wing_attack", "", "", "", floor_level));// Taillow : Wing Attack
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(24, 2), "ekans", "shed_skin", "poison_sting", "", "", "", floor_level));// Ekans : Poison Sting

                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(30, 5), "skiploom", "chlorophyll", "absorb", "", "", "", floor_level));// Skiploom : Absorb
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(33, 5), "marshtomp", "", "water_gun", "", "", "", floor_level));// Marshtomp : Water Gun
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(36, 5), "graveler", "rock_head", "rock_throw", "", "", "", floor_level));// Graveler : Rock Throw


                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(46, 2), "togepi", "serene_grace", "charm", "", "", "", floor_level));// Togepi : Charm
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(48, 2), "chatot", "keen_eye", "confide", "", "", "", floor_level));// Chatot : Confide
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(50, 2), "shellder", "", "iron_defense", "", "", "", floor_level));// Shellder : Iron Defense
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(52, 2), "munchlax", "", "amnesia", "", "", "", floor_level));// Munchlax : Amnesia
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(46, 5), "meowth", "technician", "growl", "", "", "", floor_level));// Meowth : Growl
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(48, 5), "kricketot", "", "struggle_bug", "", "", "", floor_level));// Kricketot : Struggle Bug
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(50, 5), "metapod", "", "harden", "", "", "", floor_level));// Metapod : Harden
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(52, 5), "clefairy", "", "cosmic_power", "", "", "", floor_level));// Clefairy : Cosmic Power

                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(58, 2), "ralts", "synchronize", "double_team", "", "", "", floor_level));// Ralts : Double Team
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(58, 5), "sandshrew", "", "sand_attack", "", "", "", floor_level));// Sandshrew : Sand Attack

                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(67, 2), "totodile", "", "scary_face", "", "", "", floor_level));// Totodile : Scary Face
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(67, 5), "caterpie", "", "string_shot", "", "", "", floor_level));// Caterpie : String Shot


                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(81, 2), "vulpix", "", "will_o_wisp", "", "", "", floor_level));// Vulpix : Will-o-Wisp
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(85, 2), "hoothoot", "", "hypnosis", "", "", "", floor_level));// Hoothoot : Hypnosis
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(81, 5), "koffing", "", "poison_gas", "", "", "", floor_level));// Koffing : Poison Gas
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(85, 5), "pikachu", "run_away", "thunder_wave", "", "", "", floor_level));// Pachirisu : Thunder Wave

                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(90, 2), "golbat", "", "mean_look", "", "", "", floor_level));// Golbat : Mean Look
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(94, 2), "glameow", "", "attract", "", "", "", floor_level));// Glameow : Attract
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(90, 5), "volbeat", "", "confuse_ray", "", "", "", floor_level));// Volbeat : Confuse Ray
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(94, 5), "hoppip", "", "leech_seed", "", "", "", floor_level));// Hoppip : Leech Seed

                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(99, 5), "slugma", "flame_body", "", "", "", "", floor_level));// Slugma : Flame Body
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(101, 5), "stunky", "aftermath", "", "", "", "", floor_level));// Stunky : Aftermath
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(103, 5), "lombre", "rain_dish", "rain_dance", "", "", "", floor_level));// Lombre : Rain Dish : Rain Dance


                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(134, 3), "sentret", "keen_eye", "", "", "", "", floor_level));// Sentret
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(136, 3), "sentret", "keen_eye", "", "", "", "", floor_level));// Sentret
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(134, 5), "sentret", "keen_eye", "", "", "", "", floor_level));// Sentret
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(136, 5), "sentret", "keen_eye", "", "", "", "", floor_level));// Sentret


                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(156, 2), "ditto", "", "transform", "", "", "", floor_level));// Ditto : Transform
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(158, 2), "ditto", "", "transform", "", "", "", floor_level));// ''
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(161, 2), "ditto", "", "transform", "", "", "", floor_level));// ''
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(163, 2), "ditto", "", "transform", "", "", "", floor_level));// ''
                PlaceNoLocMobsStep<StairsMapGenContext> mobStep = new PlaceNoLocMobsStep<StairsMapGenContext>(presetMultiSpawner);
                layout.GenSteps.Add(PR_SPAWN_MOBS, mobStep);

                //Tilesets
                AddTextureData(layout, "darknight_relic_wall", "darknight_relic_floor", "darknight_relic_secondary", "normal");

                structure.Floors[floor_level - 1] = layout;

                //structure.MainExit = ZoneLoc.Invalid;
                zone.Segments.Add(structure);
            }
        }

        #endregion
    }
}
