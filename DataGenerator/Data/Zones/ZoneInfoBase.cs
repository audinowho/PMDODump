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
            zone.Name = new LocalText("**Debug Zone");

            {
                LayeredSegment structure = new LayeredSegment();

                //First floor: Tests traps, secret stairs, FOV, and has dummies to perform moves on.
                #region DEBUG FLOOR 1
                {
                    StairsFloorGen layout = new StairsFloorGen();

                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<StairsMapGenContext>(new LocalText("Debug Dungeon Main Room")));
                    AddTitleDrop(layout);
                    AddFloorData(layout, "Summit.ogg", -1, Map.SightRange.Dark, Map.SightRange.Dark);

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
                    //    List<(EffectTile, Loc)> exits = new List<(EffectTile, Loc)>();
                    //    EffectTile secretStairs = new EffectTile("tile_cradle", true);
                    //    exits.Add((secretStairs, new Loc(36, 2)));
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
                        post_mob.SpawnFeatures.Add(new MobSpawnItem(true, "food_apple"));
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


                    {
                        List<(EffectTile, Loc)> exits = new List<(EffectTile, Loc)>();
                        exits.Add((new EffectTile("trap_mud", true), new Loc(2, 12)));
                        exits.Add((new EffectTile("trap_warp", true), new Loc(4, 12)));
                        exits.Add((new EffectTile("trap_gust", true), new Loc(6, 12)));
                        exits.Add((new EffectTile("trap_chestnut", true), new Loc(8, 12)));
                        exits.Add((new EffectTile("trap_poison", true), new Loc(10, 12)));
                        exits.Add((new EffectTile("trap_slumber", true), new Loc(12, 12)));
                        exits.Add((new EffectTile("trap_sticky", true), new Loc(14, 12)));
                        exits.Add((new EffectTile("trap_seal", true), new Loc(16, 12)));
                        exits.Add((new EffectTile("trap_self_destruct", true), new Loc(18, 12)));
                        exits.Add((new EffectTile("trap_trip", true), new Loc(20, 12)));
                        exits.Add((new EffectTile("trap_hunger", true), new Loc(2, 14)));
                        exits.Add((new EffectTile("trap_apple", true), new Loc(4, 14)));
                        exits.Add((new EffectTile("trap_pp_leech", true), new Loc(6, 14)));
                        exits.Add((new EffectTile("trap_summon", true), new Loc(8, 14)));
                        exits.Add((new EffectTile("trap_explosion", true), new Loc(10, 14)));
                        exits.Add((new EffectTile("trap_slow", true), new Loc(12, 14)));
                        exits.Add((new EffectTile("trap_spin", true), new Loc(14, 14)));
                        exits.Add((new EffectTile("trap_grimy", true), new Loc(16, 14)));
                        exits.Add((new EffectTile("trap_trigger", true), new Loc(18, 14)));
                        exits.Add((new EffectTile("trap_grudge", true), new Loc(20, 14)));
                        AddSpecificSpawn(layout, exits, PR_EXITS);
                    }

                    List<(InvItem, Loc)> items = new List<(InvItem, Loc)>();
                    items.Add((new InvItem("berry_lum"), new Loc(7, 32)));//Lum Berry
                    items.Add((new InvItem("berry_lum"), new Loc(8, 33)));//Lum Berry
                    AddSpecificSpawn(layout, items, PR_SPAWN_ITEMS);

                    //Tilesets
                    AddTextureData(layout, "test_dungeon_wall", "test_dungeon_floor", "test_dungeon_secondary", "none");

                    structure.Floors.Add(layout);
                }
                #endregion


                //Second floor.  Tests Monster Houses, Locked Passages, and Beetle-shaped rooms
                #region ENEMY STRESS TEST
                {
                    GridFloorGen layout = new GridFloorGen();

                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(new LocalText("Test: Enemies")));
                    AddTitleDrop(layout);
                    AddFloorData(layout, "Summit.ogg", 500, Map.SightRange.Clear, Map.SightRange.Clear);
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
                        AddConnectedRoomsRandStep<MapGenContext> detours = new AddConnectedRoomsRandStep<MapGenContext>(detourRooms, detourHalls);
                        detours.Amount = new RandRange(1);
                        detours.HallPercent = 100;
                        detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom(), new UnVaultableRoom()));
                        detours.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.KeyVault));
                        detours.RoomComponents.Set(new NoConnectRoom());
                        detours.RoomComponents.Set(new NoEventRoom());
                        detours.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.KeyVault));
                        detours.HallComponents.Set(new NoConnectRoom());
                        detours.HallComponents.Set(new NoEventRoom());

                        layout.GenSteps.Add(PR_ROOMS_GEN_EXTRA, detours);
                    }

                    AddDrawGridSteps(layout);

                    //sealing the vault
                    {
                        KeySealStep<MapGenContext> vaultStep = new KeySealStep<MapGenContext>("sealed_block", "sealed_door", "key");
                        vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                        layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                    }

                    AddStairStep(layout, false);

                    //items
                    ItemSpawnStep<MapGenContext> itemSpawnZoneStep = new ItemSpawnStep<MapGenContext>();
                    SpawnList<InvItem> items = new SpawnList<InvItem>();
                    items.Add(new InvItem("berry_leppa"), 12);
                    itemSpawnZoneStep.Spawns.Add("uncategorized", items, 10);
                    layout.GenSteps.Add(PR_RESPAWN_ITEM, itemSpawnZoneStep);

                    //enemies!
                    AddRespawnData(layout, 200, 1);

                    MobSpawnStep<MapGenContext> spawnStep = new MobSpawnStep<MapGenContext>();
                    PoolTeamSpawner poolSpawn = new PoolTeamSpawner();
                    //poolSpawn.Spawns.Add(GetTeamMob("castform", "", "", "", "", "", new RandRange(18), "wander_smart", true), 10);
                    //poolSpawn.TeamSizes.Add(1, 12);
                    //spawnStep.Spawns.Add(poolSpawn, 100);

                    SpecificTeamSpawner teamSpawn = new SpecificTeamSpawner(GetGenericMob("castform", "", "", "", "", "", new RandRange(18), "wander_smart", true, true), GetGenericMob("castform", "", "", "", "", "", new RandRange(18), "wander_smart", true, true));
                    spawnStep.Spawns.Add(teamSpawn, 100);
                    layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);

                    AddEnemySpawnData(layout, 20, new RandRange(200));

                    //Tilesets
                    AddTextureData(layout, "test_dungeon_wall", "test_dungeon_floor", "test_dungeon_secondary", "none");

                    //monsterhouse
                    {
                        MonsterHouseStep<MapGenContext> monsterHouse = new MonsterHouseStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                        monsterHouse.Items.Add(new MapItem("orb_escape"), 10);
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
                        treasures.SpecificSpawns.Add(new InvItem("berry_enigma"));
                        treasures.SpecificSpawns.Add(new InvItem("berry_enigma"));
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

                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(new LocalText("Test: Vault Shop Boss")));
                    AddTitleDrop(layout);
                    AddFloorData(layout, "Summit.ogg", 500, Map.SightRange.Dark, Map.SightRange.Dark);

                    Dictionary<ItemFake, MobSpawn> spawnTable = new Dictionary<ItemFake, MobSpawn>();
                    spawnTable.Add(new ItemFake("food_apple", "applin"), GetGenericMob("applin", "", "astonish", "", "", "", new RandRange(20)));
                    spawnTable.Add(new ItemFake("food_apple", "appletun"), GetGenericMob("appletun", "", "apple_acid", "", "", "", new RandRange(20)));
                    AddFloorFakeItems(layout, spawnTable);

                    AddInitGridStep(layout, 8, 8, 8, 8);

                    //construct paths
                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(50);

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
                    items.Add(InvItem.CreateBox("food_apple", "applin"), 12);
                    items.Add(InvItem.CreateBox("food_apple", "appletun"), 12);
                    itemSpawnZoneStep.Spawns.Add("uncategorized", items, 10);
                    layout.GenSteps.Add(PR_RESPAWN_ITEM, itemSpawnZoneStep);

                    AddItemData(layout, new RandRange(2, 5), 25);

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

                    AddBlobWaterSteps(layout, "water", new RandRange(10), new IntRange(2, 10));

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
                        shop.Items.Add(new MapItem("seed_reviver", 0, 800), 10);//reviver seed
                        shop.Items.Add(new MapItem("seed_blast", 0, 500), 10);//blast seed
                        shop.Items.Add(MapItem.CreateBox("box_light", "xcl_element_poison_dust", 8000), 10);//poison dust
                        shop.Items.Add(new MapItem("wand_lob", 9, 180), 10);//Lob Wand
                        shop.Items.Add(new MapItem("evo_thunder_stone", 0, 2000), 10);//thunder stone
                        shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                        // 137 Porygon : 36 Trace : 97 Agility : 59 Blizzard : 435 Discharge : 94 Psychic
                        shop.StartMob = GetShopMob("porygon", "trace", "agility", "blizzard", "discharge", "psychic", new string[] { "xcl_family_porygon_00", "xcl_family_porygon_01", "xcl_family_porygon_02", "xcl_family_porygon_03" }, 2);
                        {
                            // 474 Porygon-Z : 91 Adaptability : 247 Shadow Ball : 63 Hyper Beam : 435 Discharge : 373 Embargo
                            shop.Mobs.Add(GetShopMob("porygon_z", "adaptability", "shadow_ball", "hyper_beam", "discharge", "embargo", new string[] { "xcl_family_porygon_00", "xcl_family_porygon_01", "xcl_family_porygon_02", "xcl_family_porygon_03" }, -1), 10);
                            // 474 Porygon-Z : 91 Adaptability : 160 Conversion : 59 Blizzard : 435 Discharge : 473 Psyshock
                            shop.Mobs.Add(GetShopMob("porygon_z", "adaptability", "conversion", "blizzard", "discharge", "psyshock", new string[] { "xcl_family_porygon_00", "xcl_family_porygon_01", "xcl_family_porygon_02", "xcl_family_porygon_03" }, -1), 10);
                            // 474 Porygon-Z : 91 Adaptability : 417 Nasty Plot : 63 Hyper Beam : 435 Discharge : 373 Embargo
                            shop.Mobs.Add(GetShopMob("porygon_z", "adaptability", "nasty_plot", "hyper_beam", "discharge", "embargo", new string[] { "xcl_family_porygon_00", "xcl_family_porygon_01", "xcl_family_porygon_02", "xcl_family_porygon_03" }, -1), 10);
                            // 474 Porygon-Z : 91 Adaptability : 417 Nasty Plot : 161 Tri Attack : 247 Shadow Ball : 373 Embargo
                            shop.Mobs.Add(GetShopMob("porygon_z", "adaptability", "nasty_plot", "tri_attack", "shadow_ball", "embargo", new string[] { "xcl_family_porygon_00", "xcl_family_porygon_01", "xcl_family_porygon_02", "xcl_family_porygon_03" }, -1), 10);
                            // 474 Porygon-Z : 88 Download : 97 Agility : 473 Psyshock : 324 Signal Beam : 373 Embargo
                            shop.Mobs.Add(GetShopMob("porygon_z", "download", "agility", "psyshock", "signal_beam", "embargo", new string[] { "xcl_family_porygon_00", "xcl_family_porygon_01", "xcl_family_porygon_02", "xcl_family_porygon_03" }, -1), 10);
                            // 233 Porygon2 : 36 Trace : 176 Conversion2 : 105 Recover : 60 Psybeam : 324 Signal Beam
                            shop.Mobs.Add(GetShopMob("porygon2", "trace", "conversion_2", "recover", "psybeam", "signal_beam", new string[] { "xcl_family_porygon_00", "xcl_family_porygon_01", "xcl_family_porygon_02", "xcl_family_porygon_03" }, -1), 10);
                            // 233 Porygon2 : 36 Trace : 176 Conversion2 : 105 Recover : 60 Psybeam : 435 Discharge
                            shop.Mobs.Add(GetShopMob("porygon2", "trace", "conversion_2", "recover", "psybeam", "discharge", new string[] { "xcl_family_porygon_00", "xcl_family_porygon_01", "xcl_family_porygon_02", "xcl_family_porygon_03" }, -1), 10);
                            // 233 Porygon2 : 36 Trace : 176 Conversion2 : 277 Magic Coat : 161 Tri Attack : 97 Agility
                            shop.Mobs.Add(GetShopMob("porygon2", "trace", "conversion_2", "magic_coat", "tri_attack", "agility", new string[] { "xcl_family_porygon_00", "xcl_family_porygon_01", "xcl_family_porygon_02", "xcl_family_porygon_03" }, -1), 10);
                        }
                        layout.GenSteps.Add(PR_SHOPS, shop);
                    }

                    //chest
                    {
                        ChestStep<MapGenContext> monsterHouse = new ChestStep<MapGenContext>(false, GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                        monsterHouse.Items.Add(new MapItem("orb_escape"), 10);
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
                        AddConnectedRoomsRandStep<MapGenContext> detours = new AddConnectedRoomsRandStep<MapGenContext>(detourRooms, detourHalls);
                        detours.Amount = new RandRange(2, 4);
                        detours.HallPercent = 100;
                        detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom(), new UnVaultableRoom()));
                        detours.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.SwitchVault));
                        detours.RoomComponents.Set(new NoConnectRoom());
                        detours.RoomComponents.Set(new NoEventRoom());
                        detours.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.SwitchVault));
                        detours.HallComponents.Set(new NoConnectRoom());
                        detours.HallComponents.Set(new NoEventRoom());

                        layout.GenSteps.Add(PR_ROOMS_GEN_EXTRA, detours);
                    }
                    //sealing the vault
                    {
                        SwitchSealStep<MapGenContext> vaultStep = new SwitchSealStep<MapGenContext>("sealed_block", "tile_switch", 1, true, true);
                        vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                        vaultStep.SwitchFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        vaultStep.SwitchFilters.Add(new RoomFilterComponent(true, new BossRoom()));
                        layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                    }
                    //vault treasures
                    {
                        BulkSpawner<MapGenContext, InvItem> treasures = new BulkSpawner<MapGenContext, InvItem>();
                        treasures.SpecificSpawns.Add(new InvItem("berry_enigma"));
                        treasures.SpecificSpawns.Add(new InvItem("berry_enigma"));
                        treasures.SpecificSpawns.Add(new InvItem("berry_enigma"));
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
                        int boss_idx = 0;
                        {
                            SpawnList<RoomGen<MapGenContext>> bossRooms = new SpawnList<RoomGen<MapGenContext>>();
                            bossRooms.Add(getBossRoomGen<MapGenContext>("wobbuffet", 23, 0, 1), 10);
                            layout.GenSteps.Add(PR_ROOMS_GEN_EXTRA, CreateGenericBossRoomStep(bossRooms, boss_idx));
                        }
                        //sealing the boss room and treasure room
                        {
                            BossSealStep<MapGenContext> vaultStep = new BossSealStep<MapGenContext>("sealed_block", "tile_boss");
                            vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                            vaultStep.Filters.Add(new RoomFilterIndex(false, boss_idx));
                            vaultStep.BossFilters.Add(new RoomFilterComponent(false, new BossRoom()));
                            vaultStep.BossFilters.Add(new RoomFilterIndex(false, boss_idx));
                            layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                        }
                        //vault treasures
                        {
                            BulkSpawner<MapGenContext, InvItem> treasures = new BulkSpawner<MapGenContext, InvItem>();
                            treasures.SpecificSpawns.Add(new InvItem("gummi_wonder"));
                            treasures.SpecificSpawns.Add(new InvItem("gummi_wonder"));
                            treasures.SpecificSpawns.Add(new InvItem("gummi_wonder"));
                            RandomRoomSpawnStep<MapGenContext, InvItem> detourItems = new RandomRoomSpawnStep<MapGenContext, InvItem>(treasures);
                            detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                            detourItems.Filters.Add(new RoomFilterIndex(false, boss_idx));
                            layout.GenSteps.Add(PR_SPAWN_ITEMS_EXTRA, detourItems);
                        }
                    }

                    {
                        int boss_idx = 1;
                        {
                            SpawnList<RoomGen<MapGenContext>> bossRooms = new SpawnList<RoomGen<MapGenContext>>();
                            bossRooms.Add(getBossRoomGen<MapGenContext>("delphox", 23, 0, 1), 10);
                            layout.GenSteps.Add(PR_ROOMS_GEN_EXTRA, CreateGenericBossRoomStep(bossRooms, boss_idx));
                        }
                        //sealing the boss room and treasure room
                        {
                            BossSealStep<MapGenContext> vaultStep = new BossSealStep<MapGenContext>("sealed_block", "tile_boss");
                            vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                            vaultStep.Filters.Add(new RoomFilterIndex(false, boss_idx));
                            vaultStep.BossFilters.Add(new RoomFilterComponent(false, new BossRoom()));
                            vaultStep.BossFilters.Add(new RoomFilterIndex(false, boss_idx));
                            layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                        }
                        //vault treasures
                        {
                            BulkSpawner<MapGenContext, InvItem> treasures = new BulkSpawner<MapGenContext, InvItem>();
                            treasures.SpecificSpawns.Add(new InvItem("gummi_black"));
                            treasures.SpecificSpawns.Add(new InvItem("gummi_black"));
                            treasures.SpecificSpawns.Add(new InvItem("gummi_black"));
                            RandomRoomSpawnStep<MapGenContext, InvItem> detourItems = new RandomRoomSpawnStep<MapGenContext, InvItem>(treasures);
                            detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                            detourItems.Filters.Add(new RoomFilterIndex(false, boss_idx));
                            layout.GenSteps.Add(PR_SPAWN_ITEMS_EXTRA, detourItems);
                        }
                    }


                    structure.Floors.Add(layout);
                }
                #endregion

                #region Monster Hall
                {
                    GridFloorGen layout = new GridFloorGen();

                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(new LocalText("Test: Monster Hall")));
                    AddTitleDrop(layout);
                    AddFloorData(layout, "Flyaway Cliffs.ogg", 500, Map.SightRange.Dark, Map.SightRange.Dark);

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
                    items.Add(new InvItem("berry_leppa"), 12);
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
                        monsterHouse.Items.Add(new MapItem("orb_escape"), 10);
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

                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(new LocalText("Test: Monster Mansion")));
                    AddTitleDrop(layout);
                    AddFloorData(layout, "Flyaway Cliffs.ogg", 500, Map.SightRange.Dark, Map.SightRange.Dark);

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
                    items.Add(new InvItem("berry_leppa"), 12);
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
                        monsterHouse.Items.Add(new MapItem("orb_escape"), 10);
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

                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(new LocalText("Test: Monster Maze")));
                    AddTitleDrop(layout);
                    AddFloorData(layout, "Flyaway Cliffs.ogg", 500, Map.SightRange.Dark, Map.SightRange.Dark);

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
                    {
                        SetGridInnerComponentStep<MapGenContext> step = new SetGridInnerComponentStep<MapGenContext>();
                        step.Components.Set(new UnVaultableRoom());
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }

                    AddDrawGridSteps(layout);

                    AddStairStep(layout, false);

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 5), "tile_wonder", true);//wonder tile
                    AddSingleTrapStep(layout, new RandRange(16, 19), "trap_poison", false);//poison trap

                    //items
                    ItemSpawnStep<MapGenContext> itemSpawnZoneStep = new ItemSpawnStep<MapGenContext>();
                    SpawnList<InvItem> items = new SpawnList<InvItem>();
                    items.Add(new InvItem("berry_leppa"), 12);
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
                        monsterHouse.Items.Add(new MapItem("orb_escape"), 10);
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
                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(new LocalText("Test: Hedge Maze")));
                    AddTitleDrop(layout);
                    AddFloorData(layout,"Flyaway Cliffs.ogg", 1000, Map.SightRange.Clear, Map.SightRange.Dark);
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
                    items.Add(new InvItem("berry_leppa"), 12);
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
                    {
                        SetGridInnerComponentStep<MapGenContext> step = new SetGridInnerComponentStep<MapGenContext>();
                        step.Components.Set(new UnVaultableRoom());
                        layout.GenSteps.Add(PR_GRID_GEN, step);
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
                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(new LocalText("Test: Hedge Maze 2")));
                    AddTitleDrop(layout);
                    AddFloorData(layout, "Flyaway Cliffs.ogg", 1000, Map.SightRange.Clear, Map.SightRange.Dark);
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
                    items.Add(new InvItem("berry_leppa"), 12);
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
                    {
                        SetGridInnerComponentStep<MapGenContext> step = new SetGridInnerComponentStep<MapGenContext>();
                        step.Components.Set(new UnVaultableRoom());
                        layout.GenSteps.Add(PR_GRID_GEN, step);
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
                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<ListMapGenContext>(new LocalText("Test: Floating Islands")));
                    AddTitleDrop(layout);
                    AddFloorData(layout, "Flyaway Cliffs.ogg", 2000, Map.SightRange.Clear, Map.SightRange.Dark);
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
                    items.Add(new InvItem("berry_leppa"), 12);
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
                        secretMobPlacement.AcceptedTiles.Add(new Tile("pit"));
                        layout.GenSteps.Add(PR_SPAWN_MOBS, secretMobPlacement);
                    }

                    //items
                    AddItemData(layout, new RandRange(3, 6), 25);

                    //secret items
                    SpawnList<InvItem> secretItemSpawns = new SpawnList<InvItem>();
                    secretItemSpawns.Add(new InvItem("seed_reviver"), 10);

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
                        AddDisconnectedRoomsRandStep<ListMapGenContext> addDisconnect = new AddDisconnectedRoomsRandStep<ListMapGenContext>();
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
                    string terrain = "pit";
                    PerlinWaterStep<ListMapGenContext> waterStep = new PerlinWaterStep<ListMapGenContext>(new RandRange(80), 3, new Tile(terrain), new MapTerrainStencil<ListMapGenContext>(false, true, false, false), 2, false);
                    layout.GenSteps.Add(PR_WATER, waterStep);

                    //put the walls back in via "water" algorithm
                    AddBlobWaterSteps(layout, "wall", new RandRange(10), new IntRange(1, 20), false);

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
                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(new LocalText("Test: Grid Room Merging")));
                    AddTitleDrop(layout);
                    AddFloorData(layout, "Flyaway Cliffs.ogg", 1000, Map.SightRange.Dark, Map.SightRange.Dark);

                    //Tilesets
                    //AddTextureData(layout, "sky_peak_4th_pass_wall", "sky_peak_4th_pass_floor", "sky_peak_4th_pass_secondary", "normal");

                    MapDictTextureStep<MapGenContext> textureStep = new MapDictTextureStep<MapGenContext>();
                    {
                        textureStep.BlankBG = "wyvern_hill_wall";
                        textureStep.TextureMap["floor"] = "wyvern_hill_floor";
                        textureStep.TextureMap["unbreakable"] = "wyvern_hill_wall";
                        textureStep.TextureMap["wall"] = "wyvern_hill_wall";
                        textureStep.TextureMap["water"] = "wyvern_hill_secondary";
                        textureStep.TextureMap["water_poison"] = "wyvern_hill_secondary";
                        textureStep.TextureMap["pit"] = "wyvern_hill_secondary";
                        textureStep.TextureMap["water_poison"] = "wyvern_hill_secondary";
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
                    items.Add(new InvItem("berry_leppa"), 12);
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
                        step.Filters.Add(new RoomFilterDefaultGen(true));
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
                    PerlinWaterStep<MapGenContext> coverStep = new PerlinWaterStep<MapGenContext>(new RandRange(20), 3, new Tile(coverTerrain), new MapTerrainStencil<MapGenContext>(true, false, false, false), 1);
                    layout.GenSteps.Add(PR_WATER, coverStep);

                    //TerrainBorderStencil<MapGenContext> stencil = new TerrainBorderStencil<MapGenContext>();
                    //stencil.MatchTiles.Add(new Tile("wall"));
                    //stencil.BorderTiles.Add(new Tile("grass"));
                    //stencil.BorderTiles.Add(new Tile("walkable"));
                    //stencil.FullSide = true;
                    //stencil.Intrude = true;
                    
                    MultiBlobStencil<MapGenContext> combined = new MultiBlobStencil<MapGenContext>();
                    combined.List.Add(new BlobTileStencil<MapGenContext>(new MapTerrainStencil<MapGenContext>(false, false, true, true)));
                    combined.List.Add(new NoChokepointStencil<MapGenContext>(new MapTerrainStencil<MapGenContext>(false, false, true, true)));
                    combined.List.Add(new BlobTileStencil<MapGenContext>(new TileEffectStencil<MapGenContext>(true)));
                    combined.List.Add(new StairsStencil<MapGenContext>(true));

                    LoadBlobStep<MapGenContext> wallStep = new LoadBlobStep<MapGenContext>();
                    wallStep.TerrainStencil = combined;
                    wallStep.PreventChanges = PostProcType.Panel & PostProcType.Terrain;
                    wallStep.Maps.Add("test_blob", 10);
                    wallStep.Amount = new RandRange(10, 15);
                    layout.GenSteps.Add(PR_WATER, wallStep);

                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());


                    structure.Floors.Add(layout);
                }
                #endregion

                #region CHASM CAVE
                {
                    GridFloorGen layout = new GridFloorGen();

                    //Floor settings
                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(new LocalText("Test: Chasm Cave")));
                    AddTitleDrop(layout);
                    AddFloorData(layout, "Flyaway Cliffs.ogg", 2000, Map.SightRange.Clear, Map.SightRange.Dark);

                    //Tilesets
                    MapDictTextureStep<MapGenContext> textureStep = new MapDictTextureStep<MapGenContext>();
                    {
                        textureStep.BlankBG = "chasm_cave_1_wall";
                        textureStep.TextureMap["floor"] = "chasm_cave_1_floor";
                        textureStep.TextureMap["unbreakable"] = "spacial_rift_1_wall";
                        textureStep.TextureMap["wall"] = "spacial_rift_1_wall";
                        textureStep.TextureMap["water"] = "chasm_cave_1_wall";
                        textureStep.TextureMap["water_poison"] = "chasm_cave_1_wall";
                        textureStep.TextureMap["pit"] = "chasm_cave_1_wall";
                        textureStep.TextureMap["water_poison"] = "chasm_cave_1_wall";
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
                    items.Add(new InvItem("berry_leppa"), 12);
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
                    string terrain = "pit";
                    PerlinWaterStep<MapGenContext> waterStep = new PerlinWaterStep<MapGenContext>(new RandRange(100), 3, new Tile(terrain), new MapTerrainStencil<MapGenContext>(false, true, false, false), 2);
                    layout.GenSteps.Add(PR_WATER, waterStep);
                    
                    //Remove walls where diagonals of water exist and replace with water
                    layout.GenSteps.Add(PR_WATER_DIAG, new DropDiagonalBlockStep<MapGenContext>(new Tile(terrain)));



                    //boss rooms
                    {
                        layout.GenSteps.Add(PR_ROOMS_GEN_EXTRA, getBossRoomStep<MapGenContext>("hippowdon", 0));
                    }
                    //sealing the boss room and treasure room
                    {
                        BossSealStep<MapGenContext> vaultStep = new BossSealStep<MapGenContext>("sealed_block", "tile_boss");
                        vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                        vaultStep.Filters.Add(new RoomFilterIndex(false, 0));
                        vaultStep.BossFilters.Add(new RoomFilterComponent(false, new BossRoom()));
                        vaultStep.BossFilters.Add(new RoomFilterIndex(false, 0));
                        layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                    }
                    //vault treasures
                    {
                        BulkSpawner<MapGenContext, InvItem> treasures = new BulkSpawner<MapGenContext, InvItem>();
                        treasures.SpecificSpawns.Add(new InvItem("gummi_wonder"));
                        treasures.SpecificSpawns.Add(new InvItem("gummi_wonder"));
                        treasures.SpecificSpawns.Add(new InvItem("gummi_wonder"));
                        RandomRoomSpawnStep<MapGenContext, InvItem> detourItems = new RandomRoomSpawnStep<MapGenContext, InvItem>(treasures);
                        detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                        detourItems.Filters.Add(new RoomFilterIndex(false, 0));
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
                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(new LocalText("Test: Wrapped Tiered Floor")));
                    AddTitleDrop(layout);
                    AddFloorData(layout, "Flyaway Cliffs.ogg", 1000, Map.SightRange.Dark, Map.SightRange.Dark);

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
                    items.Add(new InvItem("berry_leppa"), 12);
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

                    AddInitGridStep(layout, 8, 5, 7, 6, 1, true);

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
                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(new LocalText("Test: Script Gen Step")));
                    AddTitleDrop(layout);
                    AddFloorData(layout, "Flyaway Cliffs.ogg", 1000, Map.SightRange.Dark, Map.SightRange.Dark);

                    //Tilesets
                    //AddTextureData(layout, "sky_peak_4th_pass_wall", "sky_peak_4th_pass_floor", "sky_peak_4th_pass_secondary", "normal");

                    MapDictTextureStep<MapGenContext> textureStep = new MapDictTextureStep<MapGenContext>();
                    {
                        textureStep.BlankBG = "wyvern_hill_wall";
                        textureStep.TextureMap["floor"] = "wyvern_hill_floor";
                        textureStep.TextureMap["unbreakable"] = "wyvern_hill_wall";
                        textureStep.TextureMap["wall"] = "wyvern_hill_wall";
                        textureStep.TextureMap["water"] = "wyvern_hill_secondary";
                        textureStep.TextureMap["water_poison"] = "mt_blaze_secondary";
                        textureStep.TextureMap["pit"] = "chasm_cave_1_wall";
                        textureStep.TextureMap["water_poison"] = "poison_maze_secondary";
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
                    items.Add(new InvItem("berry_leppa"), 12);
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
                    layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<MapGenContext>(new LocalText("Test: Script Gen Step on Grid and Floor")));
                    AddTitleDrop(layout);
                    AddFloorData(layout, "Flyaway Cliffs.ogg", 1000, Map.SightRange.Dark, Map.SightRange.Dark);

                    //Tilesets
                    //AddTextureData(layout, "sky_peak_4th_pass_wall", "sky_peak_4th_pass_floor", "sky_peak_4th_pass_secondary", "normal");

                    MapDictTextureStep<MapGenContext> textureStep = new MapDictTextureStep<MapGenContext>();
                    {
                        textureStep.BlankBG = "wyvern_hill_wall";
                        textureStep.TextureMap["floor"] = "wyvern_hill_floor";
                        textureStep.TextureMap["unbreakable"] = "wyvern_hill_wall";
                        textureStep.TextureMap["wall"] = "wyvern_hill_wall";
                        textureStep.TextureMap["water"] = "wyvern_hill_secondary";
                        textureStep.TextureMap["water_poison"] = "mt_blaze_secondary";
                        textureStep.TextureMap["pit"] = "chasm_cave_1_wall";
                        textureStep.TextureMap["water_poison"] = "poison_maze_secondary";
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
                    items.Add(new InvItem("berry_leppa"), 12);
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
                
                AddFloorData(layout, "Summit.ogg", -1, Map.SightRange.Dark, Map.SightRange.Dark);

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
                layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<StairsMapGenContext>(new LocalText("Secret Room")));
                AddTitleDrop(layout);

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

            bool repeated = false;
            bool continuous = false;

            if (repeated)
            {
                SingularSegment structure = new SingularSegment(-1);

                LoadGen layout = new LoadGen();
                MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                startGen.MapID = "test_tiles";
                layout.GenSteps.Add(PR_FILE_LOAD, startGen);

                structure.BaseFloor = layout;

                zone.Segments.Add(structure);
            }
            else if (continuous)
            {
                SingularSegment structure = new SingularSegment(100);
                #region TILESET TESTS
                StairsFloorGen layout = new StairsFloorGen();

                AddFloorData(layout, "Demonstration 2.ogg", -1, Map.SightRange.Clear, Map.SightRange.Clear);

                string[] level = {
                            "#######.#######",
                            "#######.#######",
                            "#######.#######",
                            "#######.#######",
                            "#######.#######",
                            "#######.#######",
                            "#######.#######",
                            "#######.#######",
                            "#######.#######",
                            "#######.#######",
                            "#######.#######",
                            "#######.#######",
                            "#######.#######",
                            "........~~~~~~~",
                            "........~~~~~~~",
                            "........~~~~~~~",
                            "........~~~~~~~",
                            "........~~~~~~~",
                            "........~~~~~~~",
                            "........~~~~~~~",
                            "........~~~~~~~",
                            "........~~~~~~~",
                            "........~~~~~~~",
                            "........~~~~~~~",
                            "........~~~~~~~",
                            "........~~~~~~~",
                            "........~~~~~~~",
                            "........~~~~~~~",
                        };

                int total_tiles = 147;
                InitTilesStep<StairsMapGenContext> startStep = new InitTilesStep<StairsMapGenContext>();
                int width = level[0].Length;
                int height = level.Length;
                startStep.Width = width;
                startStep.Height = height * total_tiles;

                layout.GenSteps.Add(PR_TILES_INIT, startStep);

                SpecificTilesStep<StairsMapGenContext> drawStep = new SpecificTilesStep<StairsMapGenContext>();

                drawStep.Tiles = new Tile[width][];
                for (int xx = 0; xx < width; xx++)
                {
                    drawStep.Tiles[xx] = new Tile[height * total_tiles];
                    for (int ii = 0; ii < total_tiles; ii++)
                    {
                        int y_off = ii * height;
                        for (int jj = 0; jj < height; jj++)
                        {
                            Tile tile;
                            if (level[jj][xx] == '#')
                                tile = new Tile("wall");
                            else if (level[jj][xx] == '~')
                                tile = new Tile("water");
                            else
                                tile = new Tile("floor");

                            switch (ii)
                            {
                                case 0: SetTestTileData(tile, "tiny_woods_wall", "tiny_woods_floor", "tiny_woods_secondary"); break;
                                case 1: SetTestTileData(tile, "thunderwave_cave_wall", "thunderwave_cave_floor", "thunderwave_cave_secondary"); break;
                                case 2: SetTestTileData(tile, "mt_steel_1_wall", "mt_steel_1_floor", "mt_steel_1_secondary"); break;
                                case 3: SetTestTileData(tile, "mt_steel_2_wall", "mt_steel_2_floor", "mt_steel_2_secondary"); break;
                                case 4: SetTestTileData(tile, "grass_maze_wall", "grass_maze_floor", "grass_maze_secondary"); break;
                                case 5: SetTestTileData(tile, "uproar_forest_wall", "uproar_forest_floor", "uproar_forest_secondary"); break;
                                case 6: SetTestTileData(tile, "electric_maze_wall", "electric_maze_floor", "electric_maze_secondary"); break;
                                case 7: SetTestTileData(tile, "water_maze_wall", "water_maze_floor", "water_maze_secondary"); break;
                                case 8: SetTestTileData(tile, "poison_maze_wall", "poison_maze_floor", "poison_maze_secondary"); break;
                                case 9: SetTestTileData(tile, "rock_maze_wall", "rock_maze_floor", "rock_maze_secondary"); break;
                                case 10: SetTestTileData(tile, "silent_chasm_wall", "silent_chasm_floor", "silent_chasm_secondary"); break;
                                case 11: SetTestTileData(tile, "mt_thunder_wall", "mt_thunder_floor", "mt_thunder_secondary"); break;
                                case 12: SetTestTileData(tile, "mt_thunder_peak_wall", "mt_thunder_peak_floor", "mt_thunder_peak_secondary"); break;
                                case 13: SetTestTileData(tile, "great_canyon_wall", "great_canyon_floor", "great_canyon_secondary"); break;
                                case 14: SetTestTileData(tile, "lapis_cave_wall", "lapis_cave_floor", "lapis_cave_secondary"); break;
                                case 15: SetTestTileData(tile, "southern_cavern_2_wall", "southern_cavern_2_floor", "southern_cavern_2_secondary"); break;
                                case 16: SetTestTileData(tile, "wish_cave_2_wall", "wish_cave_2_floor", "wish_cave_2_secondary"); break;
                                case 17: SetTestTileData(tile, "rock_path_rb_wall", "rock_path_rb_floor", "rock_path_rb_secondary"); break;
                                case 18: SetTestTileData(tile, "northern_range_1_wall", "northern_range_1_floor", "northern_range_1_secondary"); break;
                                case 19: SetTestTileData(tile, "mt_blaze_wall", "mt_blaze_floor", "mt_blaze_secondary"); break;
                                case 20: SetTestTileData(tile, "snow_path_wall", "snow_path_floor", "snow_path_secondary"); break;
                                case 21: SetTestTileData(tile, "frosty_forest_wall", "frosty_forest_floor", "frosty_forest_secondary"); break;
                                case 22: SetTestTileData(tile, "mt_freeze_wall", "mt_freeze_floor", "mt_freeze_secondary"); break;
                                case 23: SetTestTileData(tile, "ice_maze_wall", "ice_maze_floor", "ice_maze_secondary"); break;
                                case 24: SetTestTileData(tile, "magma_cavern_2_wall", "magma_cavern_2_floor", "magma_cavern_2_secondary"); break;
                                case 25: SetTestTileData(tile, "magma_cavern_3_wall", "magma_cavern_3_floor", "magma_cavern_3_secondary"); break;
                                case 26: SetTestTileData(tile, "howling_forest_2_wall", "howling_forest_2_floor", "howling_forest_2_secondary"); break;
                                case 27: SetTestTileData(tile, "sky_tower_wall", "sky_tower_floor", "sky_tower_secondary"); break;
                                case 28: SetTestTileData(tile, "darknight_relic_wall", "darknight_relic_floor", "darknight_relic_secondary"); break;
                                case 29: SetTestTileData(tile, "desert_region_wall", "desert_region_floor", "desert_region_secondary"); break;
                                case 30: SetTestTileData(tile, "howling_forest_1_wall", "howling_forest_1_floor", "howling_forest_1_secondary"); break;
                                case 31: SetTestTileData(tile, "southern_cavern_1_wall", "southern_cavern_1_floor", "southern_cavern_1_secondary"); break;
                                case 32: SetTestTileData(tile, "wyvern_hill_wall", "wyvern_hill_floor", "wyvern_hill_secondary"); break;
                                case 33: SetTestTileData(tile, "solar_cave_1_wall", "solar_cave_1_floor", "solar_cave_1_secondary"); break;
                                case 34: SetTestTileData(tile, "waterfall_pond_wall", "waterfall_pond_floor", "waterfall_pond_secondary"); break;
                                case 35: SetTestTileData(tile, "stormy_sea_1_wall", "stormy_sea_1_floor", "stormy_sea_1_secondary"); break;
                                case 36: SetTestTileData(tile, "stormy_sea_2_wall", "stormy_sea_2_floor", "stormy_sea_2_secondary"); break;
                                case 37: SetTestTileData(tile, "silver_trench_3_wall", "silver_trench_3_floor", "silver_trench_3_secondary"); break;
                                case 38: SetTestTileData(tile, "buried_relic_1_wall", "buried_relic_1_floor", "buried_relic_1_secondary"); break;
                                case 39: SetTestTileData(tile, "buried_relic_2_wall", "buried_relic_2_floor", "buried_relic_2_secondary"); break;
                                case 40: SetTestTileData(tile, "buried_relic_3_wall", "buried_relic_3_floor", "buried_relic_3_secondary"); break;
                                case 41: SetTestTileData(tile, "lightning_field_wall", "lightning_field_floor", "lightning_field_secondary"); break;
                                case 42: SetTestTileData(tile, "northwind_field_wall", "northwind_field_floor", "northwind_field_secondary"); break;
                                case 43: SetTestTileData(tile, "mt_faraway_2_wall", "mt_faraway_2_floor", "mt_faraway_2_secondary"); break;
                                case 44: SetTestTileData(tile, "mt_faraway_4_wall", "mt_faraway_4_floor", "mt_faraway_4_secondary"); break;
                                case 45: SetTestTileData(tile, "northern_range_2_wall", "northern_range_2_floor", "northern_range_2_secondary"); break;
                                case 46: SetTestTileData(tile, "pitfall_valley_1_wall", "pitfall_valley_1_floor", "pitfall_valley_1_secondary"); break;
                                case 47: SetTestTileData(tile, "wish_cave_1_wall", "wish_cave_1_floor", "wish_cave_1_secondary"); break;
                                case 48: SetTestTileData(tile, "joyous_tower_wall", "joyous_tower_floor", "joyous_tower_secondary"); break;
                                case 49: SetTestTileData(tile, "purity_forest_2_wall", "purity_forest_2_floor", "purity_forest_2_secondary"); break;
                                case 50: SetTestTileData(tile, "purity_forest_4_wall", "purity_forest_4_floor", "purity_forest_4_secondary"); break;
                                case 51: SetTestTileData(tile, "purity_forest_6_wall", "purity_forest_6_floor", "purity_forest_6_secondary"); break;
                                case 52: SetTestTileData(tile, "purity_forest_7_wall", "purity_forest_7_floor", "purity_forest_7_secondary"); break;
                                case 53: SetTestTileData(tile, "purity_forest_8_wall", "purity_forest_8_floor", "purity_forest_8_secondary"); break;
                                case 54: SetTestTileData(tile, "purity_forest_9_wall", "purity_forest_9_floor", "purity_forest_9_secondary"); break;
                                case 55: SetTestTileData(tile, "murky_cave_wall", "murky_cave_floor", "murky_cave_secondary"); break;
                                case 56: SetTestTileData(tile, "western_cave_1_wall", "western_cave_1_floor", "western_cave_1_secondary"); break;
                                case 57: SetTestTileData(tile, "western_cave_2_wall", "western_cave_2_floor", "western_cave_2_secondary"); break;
                                case 58: SetTestTileData(tile, "meteor_cave_wall", "meteor_cave_floor", "meteor_cave_secondary"); break;
                                case 59: SetTestTileData(tile, "rescue_team_maze_wall", "rescue_team_maze_floor", "rescue_team_maze_secondary"); break;
                                case 60: SetTestTileData(tile, "beach_cave_wall", "beach_cave_floor", "beach_cave_secondary"); break;
                                case 61: SetTestTileData(tile, "drenched_bluff_wall", "drenched_bluff_floor", "drenched_bluff_secondary"); break;
                                case 62: SetTestTileData(tile, "mt_bristle_wall", "mt_bristle_floor", "mt_bristle_secondary"); break;
                                case 63: SetTestTileData(tile, "waterfall_cave_wall", "waterfall_cave_floor", "waterfall_cave_secondary"); break;
                                case 64: SetTestTileData(tile, "apple_woods_wall", "apple_woods_floor", "apple_woods_secondary"); break;
                                case 65: SetTestTileData(tile, "craggy_coast_wall", "craggy_coast_floor", "craggy_coast_secondary"); break;
                                case 66: SetTestTileData(tile, "side_path_wall", "side_path_floor", "side_path_secondary"); break;
                                case 67: SetTestTileData(tile, "mt_horn_wall", "mt_horn_floor", "mt_horn_secondary"); break;
                                case 68: SetTestTileData(tile, "rock_path_tds_wall", "rock_path_tds_floor", "rock_path_tds_secondary"); break;
                                case 69: SetTestTileData(tile, "foggy_forest_wall", "foggy_forest_floor", "foggy_forest_secondary"); break;
                                case 70: SetTestTileData(tile, "forest_path_wall", "forest_path_floor", "forest_path_secondary"); break;
                                case 71: SetTestTileData(tile, "steam_cave_wall", "steam_cave_floor", "steam_cave_secondary"); break;
                                case 72: SetTestTileData(tile, "unused_steam_cave_wall", "unused_steam_cave_floor", "unused_steam_cave_secondary"); break;
                                case 73: SetTestTileData(tile, "amp_plains_wall", "amp_plains_floor", "amp_plains_secondary"); break;
                                case 74: SetTestTileData(tile, "far_amp_plains_wall", "far_amp_plains_floor", "far_amp_plains_secondary"); break;
                                case 75: SetTestTileData(tile, "northern_desert_1_wall", "northern_desert_1_floor", "northern_desert_1_secondary"); break;
                                case 76: SetTestTileData(tile, "northern_desert_2_wall", "northern_desert_2_floor", "northern_desert_2_secondary"); break;
                                case 77: SetTestTileData(tile, "quicksand_cave_wall", "quicksand_cave_floor", "quicksand_cave_secondary"); break;
                                case 78: SetTestTileData(tile, "quicksand_pit_wall", "quicksand_pit_floor", "quicksand_pit_secondary"); break;
                                case 79: SetTestTileData(tile, "quicksand_unused_wall", "quicksand_unused_floor", "quicksand_unused_secondary"); break;
                                case 80: SetTestTileData(tile, "crystal_cave_1_wall", "crystal_cave_1_floor", "crystal_cave_1_secondary"); break;
                                case 81: SetTestTileData(tile, "crystal_cave_2_wall", "crystal_cave_2_floor", "crystal_cave_2_secondary"); break;
                                case 82: SetTestTileData(tile, "crystal_crossing_wall", "crystal_crossing_floor", "crystal_crossing_secondary"); break;
                                case 83: SetTestTileData(tile, "chasm_cave_1_wall", "chasm_cave_1_floor", "chasm_cave_1_wall"); break;
                                case 84: SetTestTileData(tile, "chasm_cave_2_wall", "chasm_cave_2_floor", "chasm_cave_2_wall"); break;
                                case 85: SetTestTileData(tile, "dark_hill_1_wall", "dark_hill_1_floor", "dark_hill_1_secondary"); break;
                                case 86: SetTestTileData(tile, "dark_hill_2_wall", "dark_hill_2_floor", "dark_hill_2_secondary"); break;
                                case 87: SetTestTileData(tile, "sealed_ruin_wall", "sealed_ruin_floor", "sealed_ruin_secondary"); break;
                                case 88: SetTestTileData(tile, "deep_sealed_ruin_wall", "deep_sealed_ruin_floor", "deep_sealed_ruin_secondary"); break;
                                case 89: SetTestTileData(tile, "dusk_forest_1_wall", "dusk_forest_1_floor", "dusk_forest_1_secondary"); break;
                                case 90: SetTestTileData(tile, "dusk_forest_2_wall", "dusk_forest_2_floor", "dusk_forest_2_secondary"); break;
                                case 91: SetTestTileData(tile, "deep_dusk_forest_1_wall", "deep_dusk_forest_1_floor", "deep_dusk_forest_1_secondary"); break;
                                case 92: SetTestTileData(tile, "deep_dusk_forest_2_wall", "deep_dusk_forest_2_floor", "deep_dusk_forest_2_secondary"); break;
                                case 93: SetTestTileData(tile, "treeshroud_forest_1_wall", "treeshroud_forest_1_floor", "treeshroud_forest_1_secondary"); break;
                                case 94: SetTestTileData(tile, "treeshroud_forest_2_wall", "treeshroud_forest_2_floor", "treeshroud_forest_2_secondary"); break;
                                case 95: SetTestTileData(tile, "brine_cave_wall", "brine_cave_floor", "brine_cave_secondary"); break;
                                case 96: SetTestTileData(tile, "lower_brine_cave_wall", "lower_brine_cave_floor", "lower_brine_cave_secondary"); break;
                                case 97: SetTestTileData(tile, "unused_brine_cave_wall", "unused_brine_cave_floor", "unused_brine_cave_secondary"); break;
                                case 98: SetTestTileData(tile, "hidden_land_wall", "hidden_land_floor", "hidden_land_secondary"); break;
                                case 99: SetTestTileData(tile, "hidden_highland_wall", "hidden_highland_floor", "hidden_highland_secondary"); break;
                                case 100: SetTestTileData(tile, "temporal_tower_wall", "temporal_tower_floor", "temporal_tower_secondary"); break;
                                case 101: SetTestTileData(tile, "temporal_spire_wall", "temporal_spire_floor", "temporal_spire_secondary"); break;
                                case 102: SetTestTileData(tile, "temporal_unused_wall", "temporal_unused_floor", "temporal_unused_secondary"); break;
                                case 103: SetTestTileData(tile, "mystifying_forest_wall", "mystifying_forest_floor", "mystifying_forest_secondary"); break;
                                case 104: SetTestTileData(tile, "southern_jungle_wall", "southern_jungle_floor", "southern_jungle_secondary"); break;
                                case 105: SetTestTileData(tile, "concealed_ruins_wall", "concealed_ruins_floor", "concealed_ruins_secondary"); break;
                                case 106: SetTestTileData(tile, "surrounded_sea_wall", "surrounded_sea_floor", "surrounded_sea_secondary"); break;
                                case 107: SetTestTileData(tile, "miracle_sea_wall", "miracle_sea_floor", "miracle_sea_secondary"); break;
                                case 108: SetTestTileData(tile, "mt_travail_wall", "mt_travail_floor", "mt_travail_secondary"); break;
                                case 109: SetTestTileData(tile, "the_nightmare_wall", "the_nightmare_floor", "the_nightmare_secondary"); break;
                                case 110: SetTestTileData(tile, "spacial_rift_1_wall", "spacial_rift_1_floor", "spacial_rift_1_secondary"); break;
                                case 111: SetTestTileData(tile, "spacial_rift_2_wall", "spacial_rift_2_floor", "spacial_rift_2_secondary"); break;
                                case 112: SetTestTileData(tile, "dark_crater_wall", "dark_crater_floor", "dark_crater_secondary"); break;
                                case 113: SetTestTileData(tile, "deep_dark_crater_wall", "deep_dark_crater_floor", "deep_dark_crater_secondary"); break;
                                case 114: SetTestTileData(tile, "world_abyss_2_wall", "world_abyss_2_floor", "world_abyss_2_secondary"); break;
                                case 115: SetTestTileData(tile, "golden_chamber_wall", "golden_chamber_floor", "golden_chamber_secondary"); break;
                                case 116: SetTestTileData(tile, "mystery_jungle_2_wall", "mystery_jungle_2_floor", "mystery_jungle_2_secondary"); break;
                                case 117: SetTestTileData(tile, "mystery_jungle_1_wall", "mystery_jungle_1_floor", "mystery_jungle_1_secondary"); break;
                                case 118: SetTestTileData(tile, "zero_isle_east_3_wall", "zero_isle_east_3_floor", "zero_isle_east_3_secondary"); break;
                                case 119: SetTestTileData(tile, "zero_isle_east_4_wall", "zero_isle_east_4_floor", "zero_isle_east_4_secondary"); break;
                                case 120: SetTestTileData(tile, "zero_isle_south_1_wall", "zero_isle_south_1_floor", "zero_isle_south_1_secondary"); break;
                                case 121: SetTestTileData(tile, "zero_isle_south_2_wall", "zero_isle_south_2_floor", "zero_isle_south_2_secondary"); break;
                                case 122: SetTestTileData(tile, "tiny_meadow_wall", "tiny_meadow_floor", "tiny_meadow_secondary"); break;
                                case 123: SetTestTileData(tile, "final_maze_2_wall", "final_maze_2_floor", "final_maze_2_secondary"); break;
                                case 124: SetTestTileData(tile, "unused_waterfall_pond_wall", "unused_waterfall_pond_floor", "unused_waterfall_pond_secondary"); break;
                                case 125: SetTestTileData(tile, "lush_prairie_wall", "lush_prairie_floor", "lush_prairie_secondary"); break;
                                case 126: SetTestTileData(tile, "rock_aegis_cave_wall", "rock_aegis_cave_floor", "rock_aegis_cave_secondary"); break;
                                case 127: SetTestTileData(tile, "ice_aegis_cave_wall", "ice_aegis_cave_floor", "ice_aegis_cave_secondary"); break;
                                case 128: SetTestTileData(tile, "steel_aegis_cave_wall", "steel_aegis_cave_floor", "steel_aegis_cave_secondary"); break;
                                case 129: SetTestTileData(tile, "murky_forest_wall", "murky_forest_floor", "murky_forest_secondary"); break;
                                case 130: SetTestTileData(tile, "deep_boulder_quarry_wall", "deep_boulder_quarry_floor", "deep_boulder_quarry_secondary"); break;
                                case 131: SetTestTileData(tile, "limestone_cavern_wall", "limestone_cavern_floor", "limestone_cavern_secondary"); break;
                                case 132: SetTestTileData(tile, "deep_limestone_cavern_wall", "deep_limestone_cavern_floor", "deep_limestone_cavern_secondary"); break;
                                case 133: SetTestTileData(tile, "barren_valley_wall", "barren_valley_floor", "barren_valley_secondary"); break;
                                case 134: SetTestTileData(tile, "dark_wasteland_wall", "dark_wasteland_floor", "dark_wasteland_secondary"); break;
                                case 135: SetTestTileData(tile, "future_temporal_tower_wall", "future_temporal_tower_floor", "future_temporal_tower_secondary"); break;
                                case 136: SetTestTileData(tile, "future_temporal_spire_wall", "future_temporal_spire_floor", "future_temporal_spire_secondary"); break;
                                case 137: SetTestTileData(tile, "spacial_cliffs_wall", "spacial_cliffs_floor", "spacial_cliffs_secondary"); break;
                                case 138: SetTestTileData(tile, "dark_ice_mountain_wall", "dark_ice_mountain_floor", "dark_ice_mountain_secondary"); break;
                                case 139: SetTestTileData(tile, "dark_ice_mountain_peak_wall", "dark_ice_mountain_peak_floor", "dark_ice_mountain_peak_secondary"); break;
                                case 140: SetTestTileData(tile, "icicle_forest_wall", "icicle_forest_floor", "icicle_forest_secondary"); break;
                                case 141: SetTestTileData(tile, "vast_ice_mountain_wall", "vast_ice_mountain_floor", "vast_ice_mountain_secondary"); break;
                                case 142: SetTestTileData(tile, "vast_ice_mountain_peak_wall", "vast_ice_mountain_peak_floor", "vast_ice_mountain_peak_secondary"); break;
                                case 143: SetTestTileData(tile, "sky_peak_4th_pass_wall", "sky_peak_4th_pass_floor", "sky_peak_4th_pass_secondary"); break;
                                case 144: SetTestTileData(tile, "sky_peak_7th_pass_wall", "sky_peak_7th_pass_floor", "sky_peak_7th_pass_secondary"); break;
                                case 145: SetTestTileData(tile, "sky_peak_summit_pass_wall", "sky_peak_summit_pass_floor", "sky_peak_summit_pass_secondary"); break;
                                case 146: SetTestTileData(tile, "test_dungeon_wall", "test_dungeon_floor", "test_dungeon_secondary"); break;
                            }

                            drawStep.Tiles[xx][y_off + jj] = tile;
                        }
                    }
                }

                layout.GenSteps.Add(PR_TILES_GEN, drawStep);

                {
                    List<(MapGenEntrance, Loc)> items = new List<(MapGenEntrance, Loc)>();
                    items.Add((new MapGenEntrance(Dir8.Down), new Loc(7, 5)));
                    AddSpecificSpawn(layout, items, PR_TILES_INIT);
                }
                {
                    List<(MapGenExit, Loc)> items = new List<(MapGenExit, Loc)>();
                    items.Add((new MapGenExit(new EffectTile("stairs_go_up", true)), new Loc(7, 4110)));
                    AddSpecificSpawn(layout, items, PR_TILES_INIT);
                }

                structure.BaseFloor = layout;
                #endregion

                zone.Segments.Add(structure);
            }
            else
            {
                LayeredSegment structure = new LayeredSegment();
                //Tests Tilesets, and unlockables
                #region TILESET TESTS
                for (int kk = 0; kk < 5/*153*/; kk++)
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

                    AddFloorData(layout, "Summit.ogg", -1, Map.SightRange.Dark, Map.SightRange.Dark);

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

                    switch (kk)
                    {
                        case 0: AddTestTextureData(layout, "tiny_woods_wall", "tiny_woods_floor", "tiny_woods_secondary"); break;
                        case 1: AddTestTextureData(layout, "thunderwave_cave_wall", "thunderwave_cave_floor", "thunderwave_cave_secondary"); break;
                        case 2: AddTestTextureData(layout, "mt_steel_1_wall", "mt_steel_1_floor", "mt_steel_1_secondary"); break;
                        case 3: AddTestTextureData(layout, "mt_steel_2_wall", "mt_steel_2_floor", "mt_steel_2_secondary"); break;
                        case 4: AddTestTextureData(layout, "grass_maze_wall", "grass_maze_floor", "grass_maze_secondary"); break;
                        case 5: AddTestTextureData(layout, "uproar_forest_wall", "uproar_forest_floor", "uproar_forest_secondary"); break;
                        case 6: AddTestTextureData(layout, "electric_maze_wall", "electric_maze_floor", "electric_maze_secondary"); break;
                        case 7: AddTestTextureData(layout, "water_maze_wall", "water_maze_floor", "water_maze_secondary"); break;
                        case 8: AddTestTextureData(layout, "poison_maze_wall", "poison_maze_floor", "poison_maze_secondary"); break;
                        case 9: AddTestTextureData(layout, "rock_maze_wall", "rock_maze_floor", "rock_maze_secondary"); break;
                        case 10: AddTestTextureData(layout, "silent_chasm_wall", "silent_chasm_floor", "silent_chasm_secondary"); break;
                        case 11: AddTestTextureData(layout, "mt_thunder_wall", "mt_thunder_floor", "mt_thunder_secondary"); break;
                        case 12: AddTestTextureData(layout, "mt_thunder_peak_wall", "mt_thunder_peak_floor", "mt_thunder_peak_secondary"); break;
                        case 13: AddTestTextureData(layout, "great_canyon_wall", "great_canyon_floor", "great_canyon_secondary"); break;
                        case 14: AddTestTextureData(layout, "lapis_cave_wall", "lapis_cave_floor", "lapis_cave_secondary"); break;
                        case 15: AddTestTextureData(layout, "southern_cavern_2_wall", "southern_cavern_2_floor", "southern_cavern_2_secondary"); break;
                        case 16: AddTestTextureData(layout, "wish_cave_2_wall", "wish_cave_2_floor", "wish_cave_2_secondary"); break;
                        case 17: AddTestTextureData(layout, "rock_path_rb_wall", "rock_path_rb_floor", "rock_path_rb_secondary"); break;
                        case 18: AddTestTextureData(layout, "northern_range_1_wall", "northern_range_1_floor", "northern_range_1_secondary"); break;
                        case 19: AddTestTextureData(layout, "mt_blaze_wall", "mt_blaze_floor", "mt_blaze_secondary"); break;
                        case 20: AddTestTextureData(layout, "snow_path_wall", "snow_path_floor", "snow_path_secondary"); break;
                        case 21: AddTestTextureData(layout, "frosty_forest_wall", "frosty_forest_floor", "frosty_forest_secondary"); break;
                        case 22: AddTestTextureData(layout, "mt_freeze_wall", "mt_freeze_floor", "mt_freeze_secondary"); break;
                        case 23: AddTestTextureData(layout, "ice_maze_wall", "ice_maze_floor", "ice_maze_secondary"); break;
                        case 24: AddTestTextureData(layout, "magma_cavern_2_wall", "magma_cavern_2_floor", "magma_cavern_2_secondary"); break;
                        case 25: AddTestTextureData(layout, "magma_cavern_3_wall", "magma_cavern_3_floor", "magma_cavern_3_secondary"); break;
                        case 26: AddTestTextureData(layout, "howling_forest_2_wall", "howling_forest_2_floor", "howling_forest_2_secondary"); break;
                        case 27: AddTestTextureData(layout, "sky_tower_wall", "sky_tower_floor", "sky_tower_secondary"); break;
                        case 28: AddTestTextureData(layout, "darknight_relic_wall", "darknight_relic_floor", "darknight_relic_secondary"); break;
                        case 29: AddTestTextureData(layout, "desert_region_wall", "desert_region_floor", "desert_region_secondary"); break;
                        case 30: AddTestTextureData(layout, "howling_forest_1_wall", "howling_forest_1_floor", "howling_forest_1_secondary"); break;
                        case 31: AddTestTextureData(layout, "southern_cavern_1_wall", "southern_cavern_1_floor", "southern_cavern_1_secondary"); break;
                        case 32: AddTestTextureData(layout, "wyvern_hill_wall", "wyvern_hill_floor", "wyvern_hill_secondary"); break;
                        case 33: AddTestTextureData(layout, "solar_cave_1_wall", "solar_cave_1_floor", "solar_cave_1_secondary"); break;
                        case 34: AddTestTextureData(layout, "waterfall_pond_wall", "waterfall_pond_floor", "waterfall_pond_secondary"); break;
                        case 35: AddTestTextureData(layout, "stormy_sea_1_wall", "stormy_sea_1_floor", "stormy_sea_1_secondary"); break;
                        case 36: AddTestTextureData(layout, "stormy_sea_2_wall", "stormy_sea_2_floor", "stormy_sea_2_secondary"); break;
                        case 37: AddTestTextureData(layout, "silver_trench_3_wall", "silver_trench_3_floor", "silver_trench_3_secondary"); break;
                        case 38: AddTestTextureData(layout, "buried_relic_1_wall", "buried_relic_1_floor", "buried_relic_1_secondary"); break;
                        case 39: AddTestTextureData(layout, "buried_relic_2_wall", "buried_relic_2_floor", "buried_relic_2_secondary"); break;
                        case 40: AddTestTextureData(layout, "buried_relic_3_wall", "buried_relic_3_floor", "buried_relic_3_secondary"); break;
                        case 41: AddTestTextureData(layout, "lightning_field_wall", "lightning_field_floor", "lightning_field_secondary"); break;
                        case 42: AddTestTextureData(layout, "northwind_field_wall", "northwind_field_floor", "northwind_field_secondary"); break;
                        case 43: AddTestTextureData(layout, "mt_faraway_2_wall", "mt_faraway_2_floor", "mt_faraway_2_secondary"); break;
                        case 44: AddTestTextureData(layout, "mt_faraway_4_wall", "mt_faraway_4_floor", "mt_faraway_4_secondary"); break;
                        case 45: AddTestTextureData(layout, "northern_range_2_wall", "northern_range_2_floor", "northern_range_2_secondary"); break;
                        case 46: AddTestTextureData(layout, "pitfall_valley_1_wall", "pitfall_valley_1_floor", "pitfall_valley_1_secondary"); break;
                        case 47: AddTestTextureData(layout, "joyous_tower_wall", "joyous_tower_floor", "joyous_tower_secondary"); break;
                        case 48: AddTestTextureData(layout, "purity_forest_2_wall", "purity_forest_2_floor", "purity_forest_2_secondary"); break;
                        case 49: AddTestTextureData(layout, "purity_forest_4_wall", "purity_forest_4_floor", "purity_forest_4_secondary"); break;
                        case 50: AddTestTextureData(layout, "purity_forest_6_wall", "purity_forest_6_floor", "purity_forest_6_secondary"); break;
                        case 51: AddTestTextureData(layout, "purity_forest_7_wall", "purity_forest_7_floor", "purity_forest_7_secondary"); break;
                        case 52: AddTestTextureData(layout, "purity_forest_8_wall", "purity_forest_8_floor", "purity_forest_8_secondary"); break;
                        case 53: AddTestTextureData(layout, "purity_forest_9_wall", "purity_forest_9_floor", "purity_forest_9_secondary"); break;
                        case 54: AddTestTextureData(layout, "wish_cave_1_wall", "wish_cave_1_floor", "wish_cave_1_secondary"); break;
                        case 55: AddTestTextureData(layout, "murky_cave_wall", "murky_cave_floor", "murky_cave_secondary"); break;
                        case 56: AddTestTextureData(layout, "western_cave_1_wall", "western_cave_1_floor", "western_cave_1_secondary"); break;
                        case 57: AddTestTextureData(layout, "western_cave_2_wall", "western_cave_2_floor", "western_cave_2_secondary"); break;
                        case 58: AddTestTextureData(layout, "meteor_cave_wall", "meteor_cave_floor", "meteor_cave_secondary"); break;
                        case 59: AddTestTextureData(layout, "rescue_team_maze_wall", "rescue_team_maze_floor", "rescue_team_maze_secondary"); break;
                        case 60: AddTestTextureData(layout, "beach_cave_wall", "beach_cave_floor", "beach_cave_secondary"); break;
                        case 61: AddTestTextureData(layout, "drenched_bluff_wall", "drenched_bluff_floor", "drenched_bluff_secondary"); break;
                        case 62: AddTestTextureData(layout, "mt_bristle_wall", "mt_bristle_floor", "mt_bristle_secondary"); break;
                        case 63: AddTestTextureData(layout, "waterfall_cave_wall", "waterfall_cave_floor", "waterfall_cave_secondary"); break;
                        case 64: AddTestTextureData(layout, "apple_woods_wall", "apple_woods_floor", "apple_woods_secondary"); break;
                        case 65: AddTestTextureData(layout, "craggy_coast_wall", "craggy_coast_floor", "craggy_coast_secondary"); break;
                        case 66: AddTestTextureData(layout, "side_path_wall", "side_path_floor", "side_path_secondary"); break;
                        case 67: AddTestTextureData(layout, "mt_horn_wall", "mt_horn_floor", "mt_horn_secondary"); break;
                        case 68: AddTestTextureData(layout, "rock_path_tds_wall", "rock_path_tds_floor", "rock_path_tds_secondary"); break;
                        case 69: AddTestTextureData(layout, "foggy_forest_wall", "foggy_forest_floor", "foggy_forest_secondary"); break;
                        case 70: AddTestTextureData(layout, "forest_path_wall", "forest_path_floor", "forest_path_secondary"); break;
                        case 71: AddTestTextureData(layout, "steam_cave_wall", "steam_cave_floor", "steam_cave_secondary"); break;
                        case 72: AddTestTextureData(layout, "unused_steam_cave_wall", "unused_steam_cave_floor", "unused_steam_cave_secondary"); break;
                        case 73: AddTestTextureData(layout, "amp_plains_wall", "amp_plains_floor", "amp_plains_secondary"); break;
                        case 74: AddTestTextureData(layout, "far_amp_plains_wall", "far_amp_plains_floor", "far_amp_plains_secondary"); break;
                        case 75: AddTestTextureData(layout, "northern_desert_1_wall", "northern_desert_1_floor", "northern_desert_1_secondary"); break;
                        case 76: AddTestTextureData(layout, "northern_desert_2_wall", "northern_desert_2_floor", "northern_desert_2_secondary"); break;
                        case 77: AddTestTextureData(layout, "quicksand_cave_wall", "quicksand_cave_floor", "quicksand_cave_secondary"); break;
                        case 78: AddTestTextureData(layout, "quicksand_pit_wall", "quicksand_pit_floor", "quicksand_pit_secondary"); break;
                        case 79: AddTestTextureData(layout, "quicksand_unused_wall", "quicksand_unused_floor", "quicksand_unused_secondary"); break;
                        case 80: AddTestTextureData(layout, "crystal_cave_1_wall", "crystal_cave_1_floor", "crystal_cave_1_secondary"); break;
                        case 81: AddTestTextureData(layout, "crystal_cave_2_wall", "crystal_cave_2_floor", "crystal_cave_2_secondary"); break;
                        case 82: AddTestTextureData(layout, "crystal_crossing_wall", "crystal_crossing_floor", "crystal_crossing_secondary"); break;
                        case 83: AddTestTextureData(layout, "chasm_cave_1_wall", "chasm_cave_1_floor", "chasm_cave_1_wall"); break;
                        case 84: AddTestTextureData(layout, "chasm_cave_2_wall", "chasm_cave_2_floor", "chasm_cave_2_wall"); break;
                        case 85: AddTestTextureData(layout, "dark_hill_1_wall", "dark_hill_1_floor", "dark_hill_1_secondary"); break;
                        case 86: AddTestTextureData(layout, "dark_hill_2_wall", "dark_hill_2_floor", "dark_hill_2_secondary"); break;
                        case 87: AddTestTextureData(layout, "sealed_ruin_wall", "sealed_ruin_floor", "sealed_ruin_secondary"); break;
                        case 88: AddTestTextureData(layout, "deep_sealed_ruin_wall", "deep_sealed_ruin_floor", "deep_sealed_ruin_secondary"); break;
                        case 89: AddTestTextureData(layout, "dusk_forest_1_wall", "dusk_forest_1_floor", "dusk_forest_1_secondary"); break;
                        case 90: AddTestTextureData(layout, "dusk_forest_2_wall", "dusk_forest_2_floor", "dusk_forest_2_secondary"); break;
                        case 91: AddTestTextureData(layout, "deep_dusk_forest_1_wall", "deep_dusk_forest_1_floor", "deep_dusk_forest_1_secondary"); break;
                        case 92: AddTestTextureData(layout, "deep_dusk_forest_2_wall", "deep_dusk_forest_2_floor", "deep_dusk_forest_2_secondary"); break;
                        case 93: AddTestTextureData(layout, "treeshroud_forest_1_wall", "treeshroud_forest_1_floor", "treeshroud_forest_1_secondary"); break;
                        case 94: AddTestTextureData(layout, "treeshroud_forest_2_wall", "treeshroud_forest_2_floor", "treeshroud_forest_2_secondary"); break;
                        case 95: AddTestTextureData(layout, "brine_cave_wall", "brine_cave_floor", "brine_cave_secondary"); break;
                        case 96: AddTestTextureData(layout, "lower_brine_cave_wall", "lower_brine_cave_floor", "lower_brine_cave_secondary"); break;
                        case 97: AddTestTextureData(layout, "unused_brine_cave_wall", "unused_brine_cave_floor", "unused_brine_cave_secondary"); break;
                        case 98: AddTestTextureData(layout, "hidden_land_wall", "hidden_land_floor", "hidden_land_secondary"); break;
                        case 99: AddTestTextureData(layout, "hidden_highland_wall", "hidden_highland_floor", "hidden_highland_secondary"); break;
                        case 100: AddTestTextureData(layout, "southern_jungle_wall", "southern_jungle_floor", "southern_jungle_secondary"); break;
                        case 101: AddTestTextureData(layout, "temporal_tower_wall", "temporal_tower_floor", "temporal_tower_secondary"); break;
                        case 102: AddTestTextureData(layout, "temporal_spire_wall", "temporal_spire_floor", "temporal_spire_secondary"); break;
                        case 103: AddTestTextureData(layout, "temporal_unused_wall", "temporal_unused_floor", "temporal_unused_secondary"); break;
                        case 104: AddTestTextureData(layout, "mystifying_forest_wall", "mystifying_forest_floor", "mystifying_forest_secondary"); break;
                        case 105: AddTestTextureData(layout, "concealed_ruins_wall", "concealed_ruins_floor", "concealed_ruins_secondary"); break;
                        case 106: AddTestTextureData(layout, "surrounded_sea_wall", "surrounded_sea_floor", "surrounded_sea_secondary"); break;
                        case 107: AddTestTextureData(layout, "miracle_sea_wall", "miracle_sea_floor", "miracle_sea_secondary"); break;
                        case 108: AddTestTextureData(layout, "mt_travail_wall", "mt_travail_floor", "mt_travail_secondary"); break;
                        case 109: AddTestTextureData(layout, "the_nightmare_wall", "the_nightmare_floor", "the_nightmare_secondary"); break;
                        case 110: AddTestTextureData(layout, "spacial_rift_1_wall", "spacial_rift_1_floor", "spacial_rift_1_secondary"); break;
                        case 111: AddTestTextureData(layout, "spacial_rift_2_wall", "spacial_rift_2_floor", "spacial_rift_2_secondary"); break;
                        case 112: AddTestTextureData(layout, "dark_crater_wall", "dark_crater_floor", "dark_crater_secondary"); break;
                        case 113: AddTestTextureData(layout, "deep_dark_crater_wall", "deep_dark_crater_floor", "deep_dark_crater_secondary"); break;
                        case 114: AddTestTextureData(layout, "world_abyss_2_wall", "world_abyss_2_floor", "world_abyss_2_secondary"); break;
                        case 115: AddTestTextureData(layout, "golden_chamber_wall", "golden_chamber_floor", "golden_chamber_secondary"); break;
                        case 116: AddTestTextureData(layout, "mystery_jungle_2_wall", "mystery_jungle_2_floor", "mystery_jungle_2_secondary"); break;
                        case 117: AddTestTextureData(layout, "mystery_jungle_1_wall", "mystery_jungle_1_floor", "mystery_jungle_1_secondary"); break;
                        case 118: AddTestTextureData(layout, "zero_isle_east_3_wall", "zero_isle_east_3_floor", "zero_isle_east_3_secondary"); break;
                        case 119: AddTestTextureData(layout, "zero_isle_east_4_wall", "zero_isle_east_4_floor", "zero_isle_east_4_secondary"); break;
                        case 120: AddTestTextureData(layout, "zero_isle_south_1_wall", "zero_isle_south_1_floor", "zero_isle_south_1_secondary"); break;
                        case 121: AddTestTextureData(layout, "zero_isle_south_2_wall", "zero_isle_south_2_floor", "zero_isle_south_2_secondary"); break;
                        case 122: AddTestTextureData(layout, "tiny_meadow_wall", "tiny_meadow_floor", "tiny_meadow_secondary"); break;
                        case 123: AddTestTextureData(layout, "final_maze_2_wall", "final_maze_2_floor", "final_maze_2_secondary"); break;
                        case 124: AddTestTextureData(layout, "unused_waterfall_pond_wall", "unused_waterfall_pond_floor", "unused_waterfall_pond_secondary"); break;
                        case 125: AddTestTextureData(layout, "lush_prairie_wall", "lush_prairie_floor", "lush_prairie_secondary"); break;
                        case 126: AddTestTextureData(layout, "rock_aegis_cave_wall", "rock_aegis_cave_floor", "rock_aegis_cave_secondary"); break;
                        case 127: AddTestTextureData(layout, "ice_aegis_cave_wall", "ice_aegis_cave_floor", "ice_aegis_cave_secondary"); break;
                        case 128: AddTestTextureData(layout, "steel_aegis_cave_wall", "steel_aegis_cave_floor", "steel_aegis_cave_secondary"); break;
                        case 129: AddTestTextureData(layout, "murky_forest_wall", "murky_forest_floor", "murky_forest_secondary"); break;
                        case 130: AddTestTextureData(layout, "deep_boulder_quarry_wall", "deep_boulder_quarry_floor", "deep_boulder_quarry_secondary"); break;
                        case 131: AddTestTextureData(layout, "limestone_cavern_wall", "limestone_cavern_floor", "limestone_cavern_secondary"); break;
                        case 132: AddTestTextureData(layout, "deep_limestone_cavern_wall", "deep_limestone_cavern_floor", "deep_limestone_cavern_secondary"); break;
                        case 133: AddTestTextureData(layout, "barren_valley_wall", "barren_valley_floor", "barren_valley_secondary"); break;
                        case 134: AddTestTextureData(layout, "dark_wasteland_wall", "dark_wasteland_floor", "dark_wasteland_secondary"); break;
                        case 135: AddTestTextureData(layout, "future_temporal_tower_wall", "future_temporal_tower_floor", "future_temporal_tower_secondary"); break;
                        case 136: AddTestTextureData(layout, "future_temporal_spire_wall", "future_temporal_spire_floor", "future_temporal_spire_secondary"); break;
                        case 137: AddTestTextureData(layout, "spacial_cliffs_wall", "spacial_cliffs_floor", "spacial_cliffs_secondary"); break;
                        case 138: AddTestTextureData(layout, "dark_ice_mountain_wall", "dark_ice_mountain_floor", "dark_ice_mountain_secondary"); break;
                        case 139: AddTestTextureData(layout, "dark_ice_mountain_peak_wall", "dark_ice_mountain_peak_floor", "dark_ice_mountain_peak_secondary"); break;
                        case 140: AddTestTextureData(layout, "icicle_forest_wall", "icicle_forest_floor", "icicle_forest_secondary"); break;
                        case 141: AddTestTextureData(layout, "vast_ice_mountain_wall", "vast_ice_mountain_floor", "vast_ice_mountain_secondary"); break;
                        case 142: AddTestTextureData(layout, "vast_ice_mountain_peak_wall", "vast_ice_mountain_peak_floor", "vast_ice_mountain_peak_secondary"); break;
                        case 143: AddTestTextureData(layout, "sky_peak_4th_pass_wall", "sky_peak_4th_pass_floor", "sky_peak_4th_pass_secondary"); break;
                        case 144: AddTestTextureData(layout, "sky_peak_7th_pass_wall", "sky_peak_7th_pass_floor", "sky_peak_7th_pass_secondary"); break;
                        case 145: AddTestTextureData(layout, "sky_peak_summit_pass_wall", "sky_peak_summit_pass_floor", "sky_peak_summit_pass_secondary"); break;
                        case 146: AddTestTextureData(layout, "test_dungeon_wall", "test_dungeon_floor", "test_dungeon_secondary"); break;
                        case 147: AddTestTextureData(layout, "forest_area_wall", "forest_area_floor", "forest_area_secondary"); break;
                        case 148: AddTestTextureData(layout, "high_cave_area_wall", "high_cave_area_floor", "high_cave_area_secondary"); break;
                        case 149: AddTestTextureData(layout, "sky_ruins_area_wall", "sky_ruins_area_floor", "sky_ruins_area_secondary"); break;
                        case 150: AddTestTextureData(layout, "craggy_peak_wall", "craggy_peak_floor", "craggy_peak_secondary"); break;
                        case 151: AddTestTextureData(layout, "sky_ruins_wall", "sky_ruins_floor", "sky_ruins_secondary"); break;
                        case 152: AddTestTextureData(layout, "buried_relic_2_sky_wall", "buried_relic_2_sky_floor", "buried_relic_2_sky_secondary"); break;
                    }


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

                    AddFloorData(layout, "Summit.ogg", -1, Map.SightRange.Dark, Map.SightRange.Dark);

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
                        effect.TileStates.Set(new DangerState(true));
                        effect.TileStates.Set(new UnlockState("key"));
                        effect.TileStates.Set(new BoundsState(new Rect(0, 0, 10, 10)));
                        ItemSpawnState itemSpawn = new ItemSpawnState();
                        foreach(string key in IterateGummis(true))
                            itemSpawn.Spawns.Add(new MapItem(key));
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
                        effect.TileStates.Set(new DangerState(true));
                        effect.TileStates.Set(new UnlockState("key"));
                        effect.TileStates.Set(new BoundsState(new Rect(3, 3, 13, 13)));
                        ItemSpawnState itemSpawn = new ItemSpawnState();
                        foreach (string key in IterateGummis(true))
                            itemSpawn.Spawns.Add(new MapItem(key));
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
                        treasure1.Add(new InvItem("food_apple", false, 0, 50));//Apple
                        List<InvItem> treasure2 = new List<InvItem>();
                        treasure2.Add(InvItem.CreateBox("box_light", "xcl_element_poison_dust", 8000));//Poison Dust
                        List<(List<InvItem>, Loc)> items = new List<(List<InvItem>, Loc)>();
                        List<InvItem> treasure3 = new List<InvItem>();
                        treasure3.Add(new InvItem("wand_lob", false, 9, 180));//Lob Wand
                        items.Add((treasure1, new Loc(22, 1)));
                        items.Add((treasure2, new Loc(22, 2)));
                        items.Add((treasure3, new Loc(23, 2)));
                        AddSpecificSpawnPool(layout, items, PR_SPAWN_ITEMS);

                        // place the map status
                        {
                            ShopSecurityState state = new ShopSecurityState();
                            state.Security.Add(GetShopMob("kecleon", "protean", "focus_punch", "shadow_sneak", "incinerate", "thief", new string[] { "xcl_family_kecleon_00", "xcl_family_kecleon_01", "xcl_family_kecleon_04" }, -1), 10);
                            StateMapStatusStep<StairsMapGenContext> statusData = new StateMapStatusStep<StairsMapGenContext>("shop_security", state);
                            layout.GenSteps.Add(PR_FLOOR_DATA, statusData);
                        }

                        // place the mob running the shop
                        {
                            MobSpawn post_mob = GetShopMob("kecleon", "color_change", "synchronoise", "bind", "screech", "thunder_wave", new string[] { "xcl_family_kecleon_00", "xcl_family_kecleon_01", "xcl_family_kecleon_04" }, 0);
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
                            post_mob.SpawnFeatures.Add(new MobSpawnItem(true, "food_apple"));
                            post_mob.SpawnFeatures.Add(new MobSpawnUnrecruitable());
                            mobSpawnState.Spawns.Add(post_mob);
                        }
                        {
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID("groudon", 0, "normal", Gender.Unknown);
                            post_mob.Tactic = "wait_only";
                            post_mob.Level = new RandRange(50);
                            post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc(38, 6) + new Loc(1)));
                            post_mob.SpawnFeatures.Add(new MobSpawnItem(true, "food_apple"));
                            post_mob.SpawnFeatures.Add(new MobSpawnUnrecruitable());
                            mobSpawnState.Spawns.Add(post_mob);
                        }
                        {
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID("rayquaza", 0, "normal", Gender.Unknown);
                            post_mob.Tactic = "wait_only";
                            post_mob.Level = new RandRange(50);
                            post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc(40, 6) + new Loc(1)));
                            post_mob.SpawnFeatures.Add(new MobSpawnItem(true, "food_apple"));
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
                        effect2.TileStates.Set(new UnlockState("key"));
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
                floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Replay Test Zone\n{0}F"), new Priority(-15)));
                int total_floors = 10;

                SpawnList<IGenStep> shopZoneSpawns = new SpawnList<IGenStep>();
                //kecleon shop
                {
                    ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                    shop.Personality = 0;
                    shop.SecurityStatus = "shop_security";
                    shop.Items.Add(new MapItem("seed_reviver", 0, 800), 10);//reviver seed
                    shop.Items.Add(new MapItem("seed_blast", 0, 500), 10);//blast seed
                    shop.Items.Add(MapItem.CreateBox("box_light", "xcl_element_poison_dust", 8000), 10);//poison dust
                    shop.Items.Add(new MapItem("wand_lob", 9, 180), 10);//Lob Wand
                    shop.Items.Add(new MapItem("evo_thunder_stone", 0, 2000), 10);//thunder stone
                    shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                    // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 103 screech : 86 thunder wave
                    shop.StartMob = GetShopMob("kecleon", "color_change", "synchronoise", "bind", "screech", "thunder_wave", new string[] { "xcl_family_kecleon_00", "xcl_family_kecleon_01", "xcl_family_kecleon_04" }, 0);
                    {
                        // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 103 screech : 86 thunder wave
                        shop.Mobs.Add(GetShopMob("kecleon", "color_change", "synchronoise", "bind", "screech", "thunder_wave", new string[] { "xcl_family_kecleon_00", "xcl_family_kecleon_01", "xcl_family_kecleon_04" }, -1), 10);
                        // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 50 disable : 374 fling
                        shop.Mobs.Add(GetShopMob("kecleon", "color_change", "synchronoise", "bind", "disable", "fling", new string[] { "xcl_family_kecleon_00", "xcl_family_kecleon_01", "xcl_family_kecleon_04" }, -1), 10);
                        // 352 Kecleon : 168 protean : 246 ancient power : 425 shadow sneak : 510 incinerate : 168 thief
                        shop.Mobs.Add(GetShopMob("kecleon", "protean", "ancient_power", "shadow_sneak", "incinerate", "thief", new string[] { "xcl_family_kecleon_00", "xcl_family_kecleon_01", "xcl_family_kecleon_04" }, -1), 10);
                        // 352 Kecleon : 168 protean : 332 aerial ace : 421 shadow claw : 60 psybeam : 364 feint
                        shop.Mobs.Add(GetShopMob("kecleon", "protean", "aerial_ace", "shadow_claw", "psybeam", "feint", new string[] { "xcl_family_kecleon_00", "xcl_family_kecleon_01", "xcl_family_kecleon_04" }, -1), 10);
                    }
                    shopZoneSpawns.Add(shop, 10);
                }

                //star treasures
                {
                    ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                    shop.Personality = 1;
                    shop.SecurityStatus = "shop_security";
                    shop.Items.Add(MapItem.CreateBox("box_light", "xcl_element_poison_dust", 8000), 10);//poison dust
                    shop.Items.Add(new MapItem("evo_thunder_stone", 0, 2000), 10);//thunder stone
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
                        shop.Mobs.Add(GetShopMob("clefairy", "friend_guard", "knock_off", "minimize", "moonlight", "magic_coat", new string[] { "xcl_family_clefairy_00", "xcl_family_clefairy_02" }, -1), 10);
                        // 36 Clefable : 109 Unaware : 118 Metronome : 500 Stored Power : 343 Covet : 271 Trick
                        shop.Mobs.Add(GetShopMob("clefable", "unaware", "metronome", "stored_power", "covet", "trick", new string[] { "xcl_family_clefairy_00", "xcl_family_clefairy_02" }, -1), 10);
                        // 36 Clefable : 98 Magic Guard : 118 Metronome : 213 Attract : 282 Knock Off : 266 Follow Me
                        shop.Mobs.Add(GetShopMob("clefable", "magic_guard", "metronome", "attract", "knock_off", "follow_me", new string[] { "xcl_family_clefairy_00", "xcl_family_clefairy_02" }, -1), 10);
                    }
                    shopZoneSpawns.Add(shop, 10);
                }

                //porygon wares
                {
                    ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                    shop.Personality = 2;
                    shop.SecurityStatus = "shop_security";
                    shop.Items.Add(MapItem.CreateBox("box_light", "xcl_element_poison_dust", 8000), 10);//poison dust
                    shop.Items.Add(new MapItem("evo_thunder_stone", 0, 2000), 10);//thunder stone
                    shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                    // 137 Porygon : 36 Trace : 97 Agility : 59 Blizzard : 435 Discharge : 94 Psychic
                    shop.StartMob = GetShopMob("porygon", "trace", "agility", "blizzard", "discharge", "psychic", new string[] { "xcl_family_porygon_00", "xcl_family_porygon_01", "xcl_family_porygon_02", "xcl_family_porygon_03" }, 2);
                    {
                        // 474 Porygon-Z : 91 Adaptability : 417 Nasty Plot : 63 Hyper Beam : 435 Discharge : 373 Embargo
                        shop.Mobs.Add(GetShopMob("porygon_z", "adaptability", "nasty_plot", "hyper_beam", "discharge", "embargo", new string[] { "xcl_family_porygon_00", "xcl_family_porygon_01", "xcl_family_porygon_02", "xcl_family_porygon_03" }, -1), 10);
                        // 474 Porygon-Z : 91 Adaptability : 160 Conversion : 59 Blizzard : 435 Discharge : 473 Psyshock
                        shop.Mobs.Add(GetShopMob("porygon_z", "adaptability", "conversion", "blizzard", "discharge", "psyshock", new string[] { "xcl_family_porygon_00", "xcl_family_porygon_01", "xcl_family_porygon_02", "xcl_family_porygon_03" }, -1), 10);
                        // 474 Porygon-Z : 91 Adaptability : 417 Nasty Plot : 63 Hyper Beam : 435 Discharge : 373 Embargo
                        shop.Mobs.Add(GetShopMob("porygon_z", "adaptability", "nasty_plot", "hyper_beam", "discharge", "embargo", new string[] { "xcl_family_porygon_00", "xcl_family_porygon_01", "xcl_family_porygon_02", "xcl_family_porygon_03" }, -1), 10);
                        // 474 Porygon-Z : 91 Adaptability : 417 Nasty Plot : 161 Tri Attack : 247 Shadow Ball : 373 Embargo
                        shop.Mobs.Add(GetShopMob("porygon_z", "adaptability", "nasty_plot", "tri_attack", "shadow_ball", "embargo", new string[] { "xcl_family_porygon_00", "xcl_family_porygon_01", "xcl_family_porygon_02", "xcl_family_porygon_03" }, -1), 10);
                        // 474 Porygon-Z : 88 Download : 97 Agility : 473 Psyshock : 324 Signal Beam : 373 Embargo
                        shop.Mobs.Add(GetShopMob("porygon_z", "download", "agility", "psyshock", "signal_beam", "embargo", new string[] { "xcl_family_porygon_00", "xcl_family_porygon_01", "xcl_family_porygon_02", "xcl_family_porygon_03" }, -1), 10);
                        // 233 Porygon2 : 36 Trace : 176 Conversion2 : 105 Recover : 60 Psybeam : 324 Signal Beam
                        shop.Mobs.Add(GetShopMob("porygon2", "trace", "conversion_2", "recover", "psybeam", "signal_beam", new string[] { "xcl_family_porygon_00", "xcl_family_porygon_01", "xcl_family_porygon_02", "xcl_family_porygon_03" }, -1), 10);
                        // 233 Porygon2 : 36 Trace : 176 Conversion2 : 105 Recover : 60 Psybeam : 435 Discharge
                        shop.Mobs.Add(GetShopMob("porygon2", "trace", "conversion_2", "recover", "psybeam", "discharge", new string[] { "xcl_family_porygon_00", "xcl_family_porygon_01", "xcl_family_porygon_02", "xcl_family_porygon_03" }, -1), 10);
                        // 233 Porygon2 : 36 Trace : 176 Conversion2 : 277 Magic Coat : 161 Tri Attack : 97 Agility
                        shop.Mobs.Add(GetShopMob("porygon2", "trace", "conversion_2", "magic_coat", "tri_attack", "agility", new string[] { "xcl_family_porygon_00", "xcl_family_porygon_01", "xcl_family_porygon_02", "xcl_family_porygon_03" }, -1), 10);
                    }
                    shopZoneSpawns.Add(shop, 10);
                }

                //bottle shop
                {
                    ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                    shop.Personality = 0;
                    shop.SecurityStatus = "shop_security";
                    shop.Items.Add(MapItem.CreateBox("box_light", "xcl_element_poison_dust", 8000), 10);//poison dust
                    shop.Items.Add(new MapItem("evo_thunder_stone", 0, 2000), 10);//thunder stone
                    shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                    // 213 Shuckle : 126 Contrary : 380 Gastro Acid : 564 Sticky Web : 450 Bug Bite : 92 Toxic
                    shop.StartMob = GetShopMob("shuckle", "contrary", "gastro_acid", "sticky_web", "bug_bite", "toxic", new string[] { "xcl_family_shuckle_00", "xcl_family_shuckle_01", "xcl_family_shuckle_02", "xcl_family_shuckle_03" }, 0);
                    {
                        // 213 Shuckle : 126 Contrary : 380 Gastro Acid : 564 Sticky Web : 450 Bug Bite : 92 Toxic
                        shop.Mobs.Add(GetShopMob("shuckle", "contrary", "gastro_acid", "sticky_web", "bug_bite", "toxic", new string[] { "xcl_family_shuckle_00", "xcl_family_shuckle_01", "xcl_family_shuckle_02", "xcl_family_shuckle_03" }, -1), 10);
                        // 213 Shuckle : 126 Contrary : 230 Sweet Scent : 611 Infestation : 189 Mud-Slap : 522 Struggle Bug
                        shop.Mobs.Add(GetShopMob("shuckle", "contrary", "sweet_scent", "infestation", "mud_slap", "struggle_bug", new string[] { "xcl_family_shuckle_00", "xcl_family_shuckle_01", "xcl_family_shuckle_02", "xcl_family_shuckle_03" }, -1), 10);
                        // 213 Shuckle : 126 Contrary : 230 Sweet Scent : 219 Safeguard : 446 Stealth Rock : 249 Rock Smash
                        shop.Mobs.Add(GetShopMob("shuckle", "sturdy", "sweet_scent", "safeguard", "stealth_rock", "rock_smash", new string[] { "xcl_family_shuckle_00", "xcl_family_shuckle_01", "xcl_family_shuckle_02", "xcl_family_shuckle_03" }, -1), 10);
                        // 213 Shuckle : 5 Sturdy : 379 Power Trick : 504 Shell Smash : 205 Rollout : 360 Gyro Ball
                        shop.Mobs.Add(GetShopMob("shuckle", "sturdy", "power_trick", "shell_smash", "rollout", "gyro_ball", new string[] { "xcl_family_shuckle_00", "xcl_family_shuckle_01", "xcl_family_shuckle_02", "xcl_family_shuckle_03" }, -1), 10);
                        // 213 Shuckle : 5 Sturdy : 379 Power Trick : 450 Bug Bite : 444 Stone Edge : 91 Dig
                        shop.Mobs.Add(GetShopMob("shuckle", "sturdy", "power_trick", "bug_bite", "stone_edge", "dig", new string[] { "xcl_family_shuckle_00", "xcl_family_shuckle_01", "xcl_family_shuckle_02", "xcl_family_shuckle_03" }, -1), 10);
                    }

                    shopZoneSpawns.Add(shop, 10);
                }

                //dunsparce finds
                {
                    ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                    shop.Personality = 0;
                    shop.SecurityStatus = "shop_security";
                    shop.Items.Add(MapItem.CreateBox("box_light", "xcl_element_poison_dust", 8000), 10);//poison dust
                    shop.Items.Add(new MapItem("evo_thunder_stone", 0, 2000), 10);//thunder stone
                    shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                    // 206 Dunsparce : 32 Serene Grace : 228 Pursuit : 103 Screech : 44 Bite : Secret Power
                    shop.StartMob = GetShopMob("dunsparce", "serene_grace", "pursuit", "screech", "bite", "secret_power", new string[] { "xcl_family_dunsparce_00", "xcl_family_dunsparce_01", "xcl_family_dunsparce_04" }, 0);
                    {
                        // 206 Dunsparce : 32 Serene Grace : 228 Pursuit : 99 Rage : 428 Zen Headbutt : Secret Power
                        shop.Mobs.Add(GetShopMob("dunsparce", "serene_grace", "pursuit", "rage", "zen_headbutt", "secret_power", new string[] { "xcl_family_dunsparce_00", "xcl_family_dunsparce_01", "xcl_family_dunsparce_04" }, -1), 10);
                        // 206 Dunsparce : 32 Serene Grace : 58 Ice Beam : 352 Water Pulse : Secret Power : Ancient Power
                        shop.Mobs.Add(GetShopMob("dunsparce", "serene_grace", "ice_beam", "water_pulse", "secret_power", "ancient_power", new string[] { "xcl_family_dunsparce_00", "xcl_family_dunsparce_01", "xcl_family_dunsparce_04" }, -1), 10);
                        // 206 Dunsparce : 32 Serene Grace : 228 Pursuit : 44 Bite : Secret Power : Ancient Power
                        shop.Mobs.Add(GetShopMob("dunsparce", "serene_grace", "pursuit", "bite", "secret_power", "ancient_power", new string[] { "xcl_family_dunsparce_00", "xcl_family_dunsparce_01", "xcl_family_dunsparce_04" }, -1), 10);
                        // 206 Dunsparce : 32 Serene Grace : 228 Pursuit : 355 Roost : Secret Power : Ancient Power
                        shop.Mobs.Add(GetShopMob("dunsparce", "serene_grace", "pursuit", "roost", "secret_power", "ancient_power", new string[] { "xcl_family_dunsparce_00", "xcl_family_dunsparce_01", "xcl_family_dunsparce_04" }, -1), 10);
                        // 206 Dunsparce : 155 Rattled : 228 Pursuit : 180 Spite : 20 Bind : 281 Yawn
                        shop.Mobs.Add(GetShopMob("dunsparce", "rattled", "pursuit", "spite", "bind", "yawn", new string[] { "xcl_family_dunsparce_00", "xcl_family_dunsparce_01", "xcl_family_dunsparce_04" }, -1), 10);
                        // 206 Dunsparce : 155 Rattled : 137 Glare : 180 Spite : 20 Bind : 506 Hex
                        shop.Mobs.Add(GetShopMob("dunsparce", "rattled", "glare", "spite", "bind", "hex", new string[] { "xcl_family_dunsparce_00", "xcl_family_dunsparce_01", "xcl_family_dunsparce_04" }, -1), 10);
                    }
                    shopZoneSpawns.Add(shop, 10);
                }

                SpreadStepZoneStep shopZoneStep = new SpreadStepZoneStep(new SpreadPlanSpaced(new RandRange(1, 3), new IntRange(2, 10)), PR_SHOPS, shopZoneSpawns);
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
                category.Spawns.Add(new InvItem("food_apple"), new IntRange(0, 5), 10);
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


                RandBag<IGenStep> npcZoneSpawns = new RandBag<IGenStep>(true, new List<IGenStep>());
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
                    npcZoneSpawns.ToSpawn.Add(randomSpawn);
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
                    npcZoneSpawns.ToSpawn.Add(randomSpawn);
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
                    npcZoneSpawns.ToSpawn.Add(randomSpawn);
                }
                SpreadStepZoneStep npcZoneStep = new SpreadStepZoneStep(new SpreadPlanQuota(new RandRange(4, 5), new IntRange(7, total_floors), true), PR_SPAWN_MOBS_EXTRA, npcZoneSpawns);
                floorSegment.ZoneSteps.Add(npcZoneStep);

                TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                tileSpawn.Priority = PR_RESPAWN_TRAP;
                tileSpawn.Spawns.Add(new EffectTile("trap_explosion", true), new IntRange(0, 5), 10);
                floorSegment.ZoneSteps.Add(tileSpawn);

                SpawnList<IGenStep> apricornZoneSpawns = new SpawnList<IGenStep>();
                apricornZoneSpawns.Add(new RandomSpawnStep<MapGenContext, MapItem>(new PickerSpawner<MapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem("apricorn_plain")))), 10);//plain
                SpreadStepZoneStep apricornStep = new SpreadStepZoneStep(new SpreadPlanSpaced(new RandRange(2, 5), new IntRange(3, 25)), PR_SPAWN_ITEMS, apricornZoneSpawns);//apricorn (variety)
                floorSegment.ZoneSteps.Add(apricornStep);

                SpreadRoomZoneStep evoStep = new SpreadRoomZoneStep(PR_GRID_GEN_EXTRA, PR_ROOMS_GEN_EXTRA, new SpreadPlanSpaced(new RandRange(1, 1), new IntRange(0, 5)));
                List<BaseRoomFilter> evoFilters = new List<BaseRoomFilter>();
                evoFilters.Add(new RoomFilterComponent(true, new ImmutableRoom()));
                evoFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                evoStep.Spawns.Add(new RoomGenOption(new RoomGenEvo<MapGenContext>(), new RoomGenEvo<ListMapGenContext>(), evoFilters), 10);
                floorSegment.ZoneSteps.Add(evoStep);

                for (int ii = 0; ii < total_floors; ii++)
                {

                    GridFloorGen layout = new GridFloorGen();

                    AddFloorData(layout, "Base Town.ogg", -1, Map.SightRange.Dark, Map.SightRange.Dark);

                    if (ii == 0)
                        AddInitGridStep(layout, 1, 1, 11, 11);
                    else if (ii == 1)
                        AddInitGridStep(layout, 2, 1, 11, 11);
                    else
                        AddInitGridStep(layout, 5, 4, 11, 11);

                    //construct paths
                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    if (ii == 0)
                        path.RoomRatio = new RandRange(100);
                    else if (ii == 1)
                        path.RoomRatio = new RandRange(100);
                    else
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
                        waterRing.Treasures.Add(new MapItem("food_apple_big"), 10);
                        waterRing.Treasures.Add(new MapItem("food_apple_huge"), 10);
                        waterRing.Treasures.Add(new MapItem("food_banana"), 10);
                        genericRooms.Add(waterRing, 10);
                    }
                    //Guarded Cave
                    if (ii < 5)
                    {
                        RoomGenGuardedCave<MapGenContext> guarded = new RoomGenGuardedCave<MapGenContext>();
                        //treasure
                        guarded.Treasures.RandomSpawns.Add(new MapItem("seed_plain"), 10);
                        guarded.Treasures.RandomSpawns.Add(new MapItem("seed_reviver"), 10);
                        guarded.Treasures.RandomSpawns.Add(new MapItem("seed_pure"), 10);
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

                    AddWaterSteps(layout, "pit", new RandRange(ii * 25), false);

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
                layout.GenSteps.Add(PR_FILE_LOAD, startGen);

                MapTimeLimitStep<MapLoadContext> floorData = new MapTimeLimitStep<MapLoadContext>(600);
                layout.GenSteps.Add(PR_FLOOR_DATA, floorData);

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
                layout.GenSteps.Add(PR_FILE_LOAD, startGen);
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
            structure.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Training Maze\nLv. {0}"), new Priority(-15)));

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
                
                AddFloorData(layout, "Demonstration 2.ogg", -1, Map.SightRange.Dark, Map.SightRange.Dark);

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

                items.Add((new InvItem("apricorn_plain"), new Loc(13, 10)));//Plain Apricorn
                items.Add((new InvItem("apricorn_plain"), new Loc(14, 10)));//Plain Apricorn
                items.Add((new InvItem("apricorn_plain"), new Loc(15, 10)));//Plain Apricorn
                items.Add((new InvItem("apricorn_plain"), new Loc(16, 10)));//Plain Apricorn
                items.Add((new InvItem("apricorn_plain"), new Loc(17, 10)));//Plain Apricorn

                items.Add((new InvItem("apricorn_white"), new Loc(24, 11)));//White Apricorn
                items.Add((new InvItem("apricorn_red"), new Loc(22, 10)));//Red Apricorn
                items.Add((new InvItem("apricorn_blue"), new Loc(23, 10)));//Blue Apricorn
                items.Add((new InvItem("apricorn_green"), new Loc(24, 10)));//Green Apricorn
                items.Add((new InvItem("apricorn_yellow"), new Loc(25, 10)));//Yellow Apricorn
                items.Add((new InvItem("apricorn_brown"), new Loc(26, 10)));//Brown Apricorn

                items.Add((new InvItem("machine_assembly_box"), new Loc(31, 10)));//Assembly Box
                items.Add((new InvItem("machine_assembly_box"), new Loc(32, 10)));//Assembly Box
                items.Add((new InvItem("machine_assembly_box"), new Loc(33, 10)));//Assembly Box
                items.Add((new InvItem("machine_assembly_box"), new Loc(34, 10)));//Assembly Box
                items.Add((new InvItem("machine_assembly_box"), new Loc(35, 10)));//Assembly Box
                items.Add((new InvItem("machine_recall_box"), new Loc(31, 11)));//Link Box
                items.Add((new InvItem("machine_recall_box"), new Loc(32, 11)));//Link Box
                items.Add((new InvItem("machine_recall_box"), new Loc(33, 11)));//Link Box
                items.Add((new InvItem("machine_recall_box"), new Loc(34, 11)));//Link Box
                items.Add((new InvItem("machine_recall_box"), new Loc(35, 11)));//Link Box



                items.Add((new InvItem("medicine_x_attack"), new Loc(45, 10)));//X Attack
                items.Add((new InvItem("medicine_x_attack"), new Loc(46, 10)));//X Attack
                items.Add((new InvItem("medicine_x_attack"), new Loc(45, 11)));//X Attack
                items.Add((new InvItem("medicine_x_attack"), new Loc(46, 11)));//X Attack

                items.Add((new InvItem("medicine_x_defense"), new Loc(47, 10)));//X Defense
                items.Add((new InvItem("medicine_x_defense"), new Loc(48, 10)));//X Defense
                items.Add((new InvItem("medicine_x_defense"), new Loc(47, 11)));//X Defense
                items.Add((new InvItem("medicine_x_defense"), new Loc(48, 11)));//X Defense

                items.Add((new InvItem("medicine_x_sp_atk"), new Loc(50, 10)));//X Sp. Atk
                items.Add((new InvItem("medicine_x_sp_atk"), new Loc(51, 10)));//X Sp. Atk
                items.Add((new InvItem("medicine_x_sp_atk"), new Loc(50, 11)));//X Sp. Atk
                items.Add((new InvItem("medicine_x_sp_atk"), new Loc(51, 11)));//X Sp. Atk

                items.Add((new InvItem("medicine_x_sp_def"), new Loc(52, 10)));//X Sp. Def
                items.Add((new InvItem("medicine_x_sp_def"), new Loc(53, 10)));//X Sp. Def
                items.Add((new InvItem("medicine_x_sp_def"), new Loc(52, 11)));//X Sp. Def
                items.Add((new InvItem("medicine_x_sp_def"), new Loc(53, 11)));//X Sp. Def

                items.Add((new InvItem("medicine_x_accuracy"), new Loc(56, 10)));//X Accuracy
                items.Add((new InvItem("medicine_x_accuracy"), new Loc(57, 10)));//X Accuracy
                items.Add((new InvItem("medicine_x_accuracy"), new Loc(56, 11)));//X Accuracy
                items.Add((new InvItem("medicine_x_accuracy"), new Loc(57, 11)));//X Accuracy

                items.Add((new InvItem("orb_all_dodge"), new Loc(59, 10)));//All-Dodge Orb
                items.Add((new InvItem("orb_all_dodge"), new Loc(60, 10)));//All-Dodge Orb
                items.Add((new InvItem("orb_all_dodge"), new Loc(59, 11)));//All-Dodge Orb
                items.Add((new InvItem("orb_all_dodge"), new Loc(60, 11)));//All-Dodge Orb

                items.Add((new InvItem("medicine_x_speed"), new Loc(66, 10)));//X Speed
                items.Add((new InvItem("medicine_x_speed"), new Loc(67, 10)));//X Speed
                items.Add((new InvItem("medicine_x_speed"), new Loc(68, 10)));//X Speed
                items.Add((new InvItem("medicine_x_speed"), new Loc(66, 11)));//X Speed
                items.Add((new InvItem("medicine_x_speed"), new Loc(67, 11)));//X Speed
                items.Add((new InvItem("medicine_x_speed"), new Loc(68, 11)));//X Speed

                items.Add((new InvItem("berry_lum"), new Loc(80, 10)));//Lum Berry
                items.Add((new InvItem("berry_lum"), new Loc(83, 10)));//Lum Berry
                items.Add((new InvItem("berry_lum"), new Loc(86, 10)));//Lum Berry
                items.Add((new InvItem("berry_lum"), new Loc(89, 10)));//Lum Berry
                items.Add((new InvItem("berry_lum"), new Loc(92, 10)));//Lum Berry
                items.Add((new InvItem("berry_lum"), new Loc(95, 10)));//Lum Berry
                items.Add((new InvItem("berry_lum"), new Loc(98, 10)));//Lum Berry
                items.Add((new InvItem("berry_lum"), new Loc(101, 10)));//Lum Berry
                items.Add((new InvItem("berry_lum"), new Loc(104, 10)));//Lum Berry

                items.Add((new InvItem("berry_oran"), new Loc(114, 10)));//Oran Berry
                items.Add((new InvItem("berry_oran"), new Loc(116, 10)));//Oran Berry
                items.Add((new InvItem("berry_leppa"), new Loc(118, 10)));//Leppa Berry
                items.Add((new InvItem("berry_leppa"), new Loc(120, 10)));//Leppa Berry
                items.Add((new InvItem("orb_cleanse"), new Loc(122, 10)));//Cleanse Orb
                items.Add((new InvItem("orb_cleanse"), new Loc(124, 10)));//Cleanse Orb
                items.Add((new InvItem("orb_revival"), new Loc(126, 11)));//Revival Orb
                items.Add((new InvItem("orb_cleanse"), new Loc(128, 10)));//Cleanse Orb
                items.Add((new InvItem("orb_cleanse"), new Loc(130, 10)));//Cleanse Orb
                items.Add((new InvItem("berry_leppa"), new Loc(132, 10)));//Leppa Berry
                items.Add((new InvItem("berry_leppa"), new Loc(134, 10)));//Leppa Berry
                items.Add((new InvItem("berry_oran"), new Loc(136, 10)));//Oran Berry
                items.Add((new InvItem("berry_oran"), new Loc(138, 10)));//Oran Berry

                items.Add((new InvItem("held_mobile_scarf"), new Loc(166, 11)));//Mobile Scarf
                items.Add((new InvItem("held_shell_bell"), new Loc(167, 11)));//Shell Bell
                items.Add((new InvItem("held_choice_band"), new Loc(168, 11)));//Choice Band

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

            }
            //structure.MainExit = ZoneLoc.Invalid;
            zone.Segments.Add(structure);
        }

        #endregion



        public static void AddTestTextureData<T>(MapGen<T> layout, string block, string ground, string water, bool independent = false) where T : BaseMapGenContext
        {
            layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<T>(new LocalText(block)));
            AddTitleDrop(layout);

            MapTextureStep<T> textureStep = new MapTextureStep<T>();
            {
                textureStep.GroundTileset = ground;
                textureStep.WaterTileset = water;
                textureStep.BlockTileset = block;
                textureStep.IndependentGround = independent;
            }
            layout.GenSteps.Add(PR_TEXTURES, textureStep);
        }

        public static void SetTestTileData(Tile tile, string block, string ground, string water)
        {
            if (tile.ID == "wall")
                tile.Data.TileTex = new AutoTile(block);
            else if (tile.ID == "water")
                tile.Data.TileTex = new AutoTile(water);
            else
                tile.Data.TileTex = new AutoTile(ground);
        }
    }
}
