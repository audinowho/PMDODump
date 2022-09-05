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
        //GUIDE TO MAP GENERATION PRIORITY:
        //-6 name, ID, floor data
        static readonly Priority PR_FLOOR_DATA = new Priority(-6);
        //-5 Grid Creation
        static readonly Priority PR_GRID_INIT = new Priority(-5);
        //-4 Path Generation (grid version), Adding Grid Connections, defaults
        static readonly Priority PR_GRID_GEN = new Priority(-4);
        //-4.1 special rooms in the room pool
        static readonly Priority PR_GRID_GEN_EXTRA = new Priority(-4, 1);
        //-3 writing grids to freeform floorplans
        static readonly Priority PR_ROOMS_INIT = new Priority(-3);
        //-2 path generation (list version), adding Freeform connections
        static readonly Priority PR_ROOMS_PRE_VAULT = new Priority(-2, -5);
        static readonly Priority PR_ROOMS_PRE_VAULT_CLAMP = new Priority(-2, -1);
        static readonly Priority PR_ROOMS_GEN = new Priority(-2);
        //-2.1 special rooms, extra rooms added
        static readonly Priority PR_ROOMS_GEN_EXTRA = new Priority(-2, 1);
        //-1 init map size
        static readonly Priority PR_TILES_INIT = new Priority(-1);
        //0 draw floor
        static readonly Priority PR_TILES_GEN = new Priority(0);
        //0.1 create unbreakable barriers
        static readonly Priority PR_TILES_BARRIER = new Priority(0, 1);
        //0.2 add extra floor changes ex vault barriers
        static readonly Priority PR_TILES_GEN_EXTRA = new Priority(0, 2);
        //0.3 add extra floor changes ex tunnels
        static readonly Priority PR_TILES_GEN_TUNNEL = new Priority(0, 3);
        //1 money respawn
        static readonly Priority PR_RESPAWN_MONEY = new Priority(1);
        //1.1 item respawn
        static readonly Priority PR_RESPAWN_ITEM = new Priority(1, 1);
        //1.2 mob respawn
        static readonly Priority PR_RESPAWN_MOB = new Priority(1, 2);
        //1.3 trap respawn
        static readonly Priority PR_RESPAWN_TRAP = new Priority(1, 3);
        //2 stairs
        static readonly Priority PR_EXITS = new Priority(2);
        //2.1 rescue point (save variables)
        static readonly Priority PR_EXITS_RESCUE = new Priority(2, 1);
        //2.2 sealed detours
        static readonly Priority PR_EXITS_DETOUR = new Priority(2, 2);
        //3 add water
        static readonly Priority PR_WATER = new Priority(3);
        //3.1 drop diagonal
        static readonly Priority PR_WATER_DIAG = new Priority(3, 1);
        //3.2 erase isolated
        static readonly Priority PR_WATER_DE_ISOLATE = new Priority(3, 2);
        //4 textures
        static readonly Priority PR_TEXTURES = new Priority(4);
        //4.1 shops
        static readonly Priority PR_SHOPS = new Priority(4, 1);
        //5 money
        static readonly Priority PR_SPAWN_MONEY = new Priority(5);
        //5.1 items
        static readonly Priority PR_SPAWN_ITEMS = new Priority(5, 1);
        //5.1.1 extra items
        static readonly Priority PR_SPAWN_ITEMS_EXTRA = new Priority(5, 1, 1);
        //5.2 mobs
        static readonly Priority PR_SPAWN_MOBS = new Priority(5, 2);
        //5.2.1 extra mobs
        static readonly Priority PR_SPAWN_MOBS_EXTRA = new Priority(5, 2, 1);
        //5.3 traps
        static readonly Priority PR_SPAWN_TRAPS = new Priority(5, 3);
        //6 monster houses
        static readonly Priority PR_HOUSES = new Priority(6);
        //7 debug checks
        static readonly Priority PR_DBG_CHECK = new Priority(7);


        public static void AddFloorData<T>(MapGen<T> layout, string music, int timeLimit, Map.SightRange tileSight, Map.SightRange charSight) where T : BaseMapGenContext
        {
            MapDataStep<T> floorData = new MapDataStep<T>(music, timeLimit, tileSight, charSight);
            layout.GenSteps.Add(PR_FLOOR_DATA, floorData);
        }

        /// <summary>
        /// Sets a default status for the map, one which even when changed, snaps back to normal after enough time.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layout"></param>
        /// <param name="setterID">Different families of mapstatus have different setter ID</param>
        /// <param name="defaultStatus">Must all be in the same family (weather, nonweather)</param>
        public static void AddDefaultMapStatus<T>(MapGen<T> layout, string setterID, params string[] defaultStatus) where T : BaseMapGenContext
        {
            DefaultMapStatusStep<T> statusData = new DefaultMapStatusStep<T>(setterID, defaultStatus);
            layout.GenSteps.Add(PR_FLOOR_DATA, statusData);
        }

        public static void AddTextureData<T>(MapGen<T> layout, string block, string ground, string water, string element, bool independent = false) where T : BaseMapGenContext
        {
            MapTextureStep<T> textureStep = new MapTextureStep<T>();
            {
                textureStep.GroundElement = element;
                textureStep.GroundTileset = ground;
                textureStep.WaterTileset = water;
                textureStep.BlockTileset = block;
                textureStep.IndependentGround = independent;
            }
            layout.GenSteps.Add(PR_TEXTURES, textureStep);
        }

        public static void AddSpecificTextureData<T>(MapGen<T> layout, string block, string ground, string water, string grass, string element, bool independent = false) where T : BaseMapGenContext
        {
            MapDictTextureStep<T> textureStep = new MapDictTextureStep<T>();
            {
                textureStep.BlankBG = block;
                textureStep.TextureMap["floor"] = ground;
                textureStep.TextureMap["unbreakable"] = block;
                textureStep.TextureMap["wall"] = block;
                textureStep.TextureMap["water"] = water;
                textureStep.TextureMap["lava"] = water;
                textureStep.TextureMap["pit"] = water;
                textureStep.TextureMap["poison_water"] = water;
                textureStep.TextureMap["grass"] = grass;
            }
            textureStep.GroundElement = element;
            textureStep.LayeredGround = true;
            layout.GenSteps.Add(PR_TEXTURES, textureStep);

        }

        public static void AddRespawnData<T>(MapGen<T> layout, int maxFoes, int respawnTime) where T : BaseMapGenContext
        {
            MobSpawnSettingsStep<T> spawnStep = new MobSpawnSettingsStep<T>(maxFoes, respawnTime);
            layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);
        }

        public static void AddEnemySpawnData<T>(MapGen<T> layout, int clumpFactor, RandRange amount, ConnectivityRoom.Connectivity connectivity = ConnectivityRoom.Connectivity.Main) where T : ListMapGenContext
        {
            PlaceRandomMobsStep<T> mobStep = new PlaceRandomMobsStep<T>(new TeamContextSpawner<T>(amount));
            if (connectivity != ConnectivityRoom.Connectivity.None)
                mobStep.Filters.Add(new RoomFilterConnectivity(connectivity));
            mobStep.ClumpFactor = clumpFactor;
            layout.GenSteps.Add(PR_SPAWN_MOBS, mobStep);
        }

        public static void AddMoneyData<T>(MapGen<T> layout, RandRange divAmount, bool includeHalls = false, ConnectivityRoom.Connectivity connectivity = ConnectivityRoom.Connectivity.None) where T : ListMapGenContext
        {
            TerminalSpawnStep<T, MoneySpawn> moneyStep = new TerminalSpawnStep<T, MoneySpawn>(new MoneyDivSpawner<T>(divAmount), includeHalls);
            if (connectivity != ConnectivityRoom.Connectivity.None)
                moneyStep.Filters.Add(new RoomFilterConnectivity(connectivity));
            layout.GenSteps.Add(PR_SPAWN_MONEY, moneyStep);
        }

        public static void AddItemData<T>(MapGen<T> layout, RandRange amount, int successPercent, bool includeHalls = false, ConnectivityRoom.Connectivity connectivity = ConnectivityRoom.Connectivity.Main) where T : ListMapGenContext
        {
            DueSpawnStep<T, InvItem, MapGenEntrance> itemStep = new DueSpawnStep<T, InvItem, MapGenEntrance>(new ContextSpawner<T, InvItem>(amount), successPercent, includeHalls);
            if (connectivity != ConnectivityRoom.Connectivity.None)
                itemStep.Filters.Add(new RoomFilterConnectivity(connectivity));
            layout.GenSteps.Add(PR_SPAWN_ITEMS, itemStep);
        }


        public static void AddInitGridStep<T>(MapGen<T> layout, int cellX, int cellY, int cellWidth, int cellHeight, int thickness = 1, bool wrap = false) where T : MapGenContext
        {
            InitGridPlanStep<T> startGen = new InitGridPlanStep<T>(thickness);
            {
                startGen.CellX = cellX;
                startGen.CellY = cellY;

                startGen.CellWidth = cellWidth;
                startGen.CellHeight = cellHeight;
                startGen.Wrap = wrap;
            }
            layout.GenSteps.Add(PR_GRID_INIT, startGen);
        }

        public static void AddInitListStep<T>(MapGen<T> layout, int width, int height, bool wrap = false) where T : ListMapGenContext
        {
            InitFloorPlanStep<T> startGen = new InitFloorPlanStep<T>();
            startGen.Width = width;
            startGen.Height = height;
            startGen.Wrap = wrap;
            layout.GenSteps.Add(PR_ROOMS_INIT, startGen);
        }

        public static void AddDrawGridSteps<T>(MapGen<T> layout) where T : MapGenContext
        {
            //init from floor plan
            layout.GenSteps.Add(PR_ROOMS_INIT, new DrawGridToFloorStep<T>());

            AddDrawListSteps(layout);
        }

        public static void AddDrawListSteps<T>(MapGen<T> layout) where T : ListMapGenContext
        {
            //draw paths
            layout.GenSteps.Add(PR_TILES_INIT, new DrawFloorToTileStep<T>(1));

            //add border
            layout.GenSteps.Add(PR_TILES_BARRIER, new UnbreakableBorderStep<T>(1));
        }

        public static void AddWaterSteps<T>(MapGen<T> layout, string terrain, RandRange percent, bool eraseIsolated = true) where T : BaseMapGenContext
        {
            PerlinWaterStep<T> waterStep = new PerlinWaterStep<T>(new RandRange(), 3, new Tile(terrain), new MapTerrainStencil<T>(false, true, false), 1);
            waterStep.WaterPercent = percent;
            layout.GenSteps.Add(PR_WATER, waterStep);
            layout.GenSteps.Add(PR_WATER_DIAG, new DropDiagonalBlockStep<T>(new Tile(terrain)));
            if (eraseIsolated)
                layout.GenSteps.Add(PR_WATER_DE_ISOLATE, new EraseIsolatedStep<T>(new Tile(terrain)));
        }
        public static void AddGrassSteps<T>(MapGen<T> layout, RandRange roomBlobCount, IntRange roomBlobArea, RandRange hallPercent) where T : BaseMapGenContext
        {
            string coverTerrain = "grass";
            {
                MapTerrainStencil<T> terrainStencil = new MapTerrainStencil<T>(true, false, false);
                BlobTilePercentStencil<T> terrainPercentStencil = new BlobTilePercentStencil<T>(50, terrainStencil);

                MatchTerrainStencil<T> matchStencil = new MatchTerrainStencil<T>();
                matchStencil.MatchTiles.Add(new Tile("floor"));
                matchStencil.MatchTiles.Add(new Tile("grass"));
                NoChokepointStencil<T> roomStencil = new NoChokepointStencil<T>(matchStencil);
                BlobWaterStep<T> coverStep = new BlobWaterStep<T>(roomBlobCount, new Tile(coverTerrain), new MapTerrainStencil<T>(true, false, false), new MultiBlobStencil<T>(false, terrainPercentStencil, roomStencil), roomBlobArea, new IntRange(10, 30));
                layout.GenSteps.Add(PR_WATER, coverStep);
            }
            {
                MapTerrainStencil<T> terrainStencil = new MapTerrainStencil<T>(true, false, false);
                MatchTerrainStencil<T> matchStencil = new MatchTerrainStencil<T>();
                matchStencil.MatchTiles.Add(new Tile("floor"));
                matchStencil.MatchTiles.Add(new Tile("grass"));
                NoChokepointTerrainStencil<T> roomStencil = new NoChokepointTerrainStencil<T>(matchStencil);
                roomStencil.Negate = true;
                PerlinWaterStep<T> coverStep = new PerlinWaterStep<T>(hallPercent, 4, new Tile(coverTerrain), new MultiTerrainStencil<T>(false, terrainStencil, roomStencil), 0, false);
                layout.GenSteps.Add(PR_WATER, coverStep);
            }

        }


        public static void AddSingleTrapStep<T>(MapGen<T> layout, RandRange amount, string id, bool revealed = true, bool includeHalls = false, ConnectivityRoom.Connectivity connectivity = ConnectivityRoom.Connectivity.Main) where T : ListMapGenContext
        {
            SpawnList<EffectTile> effectTileSpawns = new SpawnList<EffectTile>();
            effectTileSpawns.Add(new EffectTile(id, revealed), 10);
            RandomRoomSpawnStep<T, EffectTile> trapStep = new RandomRoomSpawnStep<T, EffectTile>(new PickerSpawner<T, EffectTile>(new LoopedRand<EffectTile>(effectTileSpawns, amount)), includeHalls);
            if (connectivity != ConnectivityRoom.Connectivity.None)
                trapStep.Filters.Add(new RoomFilterConnectivity(connectivity));
            layout.GenSteps.Add(PR_SPAWN_TRAPS, trapStep);
        }

        public static void AddTrapsSteps<T>(MapGen<T> layout, RandRange amount, bool includeHalls = false, ConnectivityRoom.Connectivity connectivity = ConnectivityRoom.Connectivity.Main) where T : ListMapGenContext
        {
            RandomRoomSpawnStep<T, EffectTile> trapStep = new RandomRoomSpawnStep<T, EffectTile>(new ContextSpawner<T, EffectTile>(amount), includeHalls);
            if (connectivity != ConnectivityRoom.Connectivity.None)
                trapStep.Filters.Add(new RoomFilterConnectivity(connectivity));
            layout.GenSteps.Add(PR_SPAWN_TRAPS, trapStep);
        }

        public static void AddTrapListStep<T>(MapGen<T> layout, RandRange amount, SpawnList<EffectTile> effectTileSpawns, bool includeHalls = false, ConnectivityRoom.Connectivity connectivity = ConnectivityRoom.Connectivity.Main) where T : ListMapGenContext
        {
            RandomRoomSpawnStep<T, EffectTile> trapStep = new RandomRoomSpawnStep<T, EffectTile>(new PickerSpawner<T, EffectTile>(new LoopedRand<EffectTile>(effectTileSpawns, amount)), includeHalls);
            if (connectivity != ConnectivityRoom.Connectivity.None)
                trapStep.Filters.Add(new RoomFilterConnectivity(connectivity));
            layout.GenSteps.Add(PR_SPAWN_TRAPS, trapStep);
        }

        static void AddStairStep<T>(MapGen<T> layout, bool goDown)
            where T : class, IFloorPlanGenContext, IPlaceableGenContext<MapGenEntrance>, IPlaceableGenContext<MapGenExit>
        {
            var step = new FloorStairsStep<T, MapGenEntrance, MapGenExit>(new MapGenEntrance(Dir8.Down), new MapGenExit(new EffectTile(goDown ? "stairs_go_down" : "stairs_go_up", true)));
            step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
            step.Filters.Add(new RoomFilterComponent(true, new BossRoom()));
            layout.GenSteps.Add(PR_EXITS, step);
        }

        static void AddSpecificSpawn<TGenContext, TSpawnable>(MapGen<TGenContext> layout, List<(TSpawnable item, Loc loc)> items, Priority priority)
            where TGenContext : class, IPlaceableGenContext<TSpawnable>
            where TSpawnable : ISpawnable
        {
            PresetMultiRand<TSpawnable> picker = new PresetMultiRand<TSpawnable>();
            List<Loc> spawnLocs = new List<Loc>();
            for (int ii = 0; ii < items.Count; ii++)
            {
                picker.ToSpawn.Add(new PresetPicker<TSpawnable>(items[ii].item));
                spawnLocs.Add(items[ii].loc);
            }
            PickerSpawner<TGenContext, TSpawnable> spawn = new PickerSpawner<TGenContext, TSpawnable>(picker);
            layout.GenSteps.Add(priority, new SpecificSpawnStep<TGenContext, TSpawnable>(spawn, spawnLocs));
        }


        static void AddSpecificSpawnPool<TGenContext, TSpawnable>(MapGen<TGenContext> layout, List<(List<TSpawnable> items, Loc loc)> items, Priority priority)
            where TGenContext : class, IPlaceableGenContext<TSpawnable>
            where TSpawnable : ISpawnable
        {
            PresetMultiRand<TSpawnable> picker = new PresetMultiRand<TSpawnable>();
            List<Loc> spawnLocs = new List<Loc>();
            for (int ii = 0; ii < items.Count; ii++)
            {
                picker.ToSpawn.Add(new RandBag<TSpawnable>(items[ii].items));
                spawnLocs.Add(items[ii].loc);
            }
            PickerSpawner<TGenContext, TSpawnable> spawn = new PickerSpawner<TGenContext, TSpawnable>(picker);
            layout.GenSteps.Add(priority, new SpecificSpawnStep<TGenContext, TSpawnable>(spawn, spawnLocs));
        }


        public static RoomGenSpecific<T> CreateRoomGenSpecific<T>(string[] level) where T : class, ITiledGenContext
        {
            RoomGenSpecific<T> roomGen = new RoomGenSpecific<T>(level[0].Length, level.Length, new Tile(DataManager.Instance.GenFloor));
            roomGen.Tiles = new Tile[level[0].Length][];
            for (int xx = 0; xx < level[0].Length; xx++)
            {
                roomGen.Tiles[xx] = new Tile[level.Length];
                for (int yy = 0; yy < level.Length; yy++)
                {
                    if (level[yy][xx] == 'X')
                        roomGen.Tiles[xx][yy] = new Tile("unbreakable");
                    else if (level[yy][xx] == '#')
                        roomGen.Tiles[xx][yy] = new Tile("wall");
                    else if (level[yy][xx] == '~')
                        roomGen.Tiles[xx][yy] = new Tile("water");
                    else if (level[yy][xx] == '^')
                        roomGen.Tiles[xx][yy] = new Tile("lava");
                    else if (level[yy][xx] == '_')
                        roomGen.Tiles[xx][yy] = new Tile("pit");
                    else
                        roomGen.Tiles[xx][yy] = new Tile(DataManager.Instance.GenFloor);
                }
            }
            return roomGen;
        }

        public static AddBossRoomStep<ListMapGenContext> CreateGenericBossRoomStep(IRandPicker<RoomGen<ListMapGenContext>> bossRooms)
        {
            SpawnList<RoomGen<ListMapGenContext>> treasureRooms = new SpawnList<RoomGen<ListMapGenContext>>();
            treasureRooms.Add(new RoomGenCross<ListMapGenContext>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
            SpawnList<PermissiveRoomGen<ListMapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
            detourHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
            AddBossRoomStep<ListMapGenContext> detours = new AddBossRoomStep<ListMapGenContext>(bossRooms, treasureRooms, detourHalls);
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

            return detours;
        }

        public static RoomGenSpecific<T> CreateRoomGenSpecificBoss<T>(string[] level, Loc trigger, List<MobSpawn> mobs, bool severe) where T : ListMapGenContext
        {
            RoomGenSpecificBoss<T> roomGen = new RoomGenSpecificBoss<T>(level[0].Length, level.Length, new Tile(DataManager.Instance.GenFloor), "tile_boss", trigger, severe ? "C02. Boss Battle 2.ogg" : "C01. Boss Battle.ogg");
            roomGen.Bosses = mobs;
            roomGen.Tiles = new Tile[level[0].Length][];
            for (int xx = 0; xx < level[0].Length; xx++)
            {
                roomGen.Tiles[xx] = new Tile[level.Length];
                for (int yy = 0; yy < level.Length; yy++)
                {
                    if (level[yy][xx] == 'X')
                        roomGen.Tiles[xx][yy] = new Tile("unbreakable");
                    else if (level[yy][xx] == '#')
                        roomGen.Tiles[xx][yy] = new Tile("wall");
                    else if (level[yy][xx] == '~')
                        roomGen.Tiles[xx][yy] = new Tile("water");
                    else if (level[yy][xx] == '^')
                        roomGen.Tiles[xx][yy] = new Tile("lava");
                    else if (level[yy][xx] == '_')
                        roomGen.Tiles[xx][yy] = new Tile("pit");
                    else
                        roomGen.Tiles[xx][yy] = new Tile(DataManager.Instance.GenFloor);
                }
            }
            return roomGen;
        }

        public static RoomGenPostProcSpecific<MapGenContext> CreateRoomGenPostProcSpecific(string[] level)
        {
            RoomGenPostProcSpecific<MapGenContext> roomGen = new RoomGenPostProcSpecific<MapGenContext>(level[0].Length, level.Length, new Tile(DataManager.Instance.GenFloor));
            roomGen.Tiles = new Tile[level[0].Length][];
            roomGen.PostProcMask = new PostProcTile[level[0].Length][];
            for (int xx = 0; xx < level[0].Length; xx++)
            {
                roomGen.Tiles[xx] = new Tile[level.Length];
                roomGen.PostProcMask[xx] = new PostProcTile[level.Length];
                for (int yy = 0; yy < level.Length; yy++)
                {
                    if (level[yy][xx] == 'X')
                        roomGen.Tiles[xx][yy] = new Tile("unbreakable");
                    else if (level[yy][xx] == '#')
                        roomGen.Tiles[xx][yy] = new Tile("wall");
                    else if (level[yy][xx] == '~')
                        roomGen.Tiles[xx][yy] = new Tile("water");
                    else if (level[yy][xx] == '^')
                        roomGen.Tiles[xx][yy] = new Tile("lava");
                    else if (level[yy][xx] == '_')
                        roomGen.Tiles[xx][yy] = new Tile("pit");
                    else
                        roomGen.Tiles[xx][yy] = new Tile(DataManager.Instance.GenFloor);
                    roomGen.PostProcMask[xx][yy] = new PostProcTile();
                }
            }
            return roomGen;
        }

        static void AddSpecificRoom(GridPathSpecific<MapGenContext> path, Loc start, Loc size, RoomGen<MapGenContext> roomGen, bool immutable = true)
        {
            SpecificGridRoomPlan<MapGenContext> specificPlan = new SpecificGridRoomPlan<MapGenContext>(new Rect(start, size), roomGen);
            if (immutable)
                specificPlan.Components.Set(new ImmutableRoom());
            path.SpecificRooms.Add(specificPlan);
        }

        static SpecificTeamSpawner CreateSetMobTeam(Loc loc, string species, string ability, string move1, string move2, string move3, string move4, int level, string tactic = "tit_for_tat")
        {
            MobSpawn post_mob = new MobSpawn();
            post_mob.BaseForm = new MonsterID(species, 0, DataManager.Instance.DefaultSkin, Gender.Unknown);
            post_mob.Intrinsic = ability;
            if (!String.IsNullOrEmpty(move1))
                post_mob.SpecifiedSkills.Add(move1);
            if (!String.IsNullOrEmpty(move2))
                post_mob.SpecifiedSkills.Add(move2);
            if (!String.IsNullOrEmpty(move3))
                post_mob.SpecifiedSkills.Add(move3);
            if (!String.IsNullOrEmpty(move4))
                post_mob.SpecifiedSkills.Add(move4);
            post_mob.Level = new RandRange(level);
            post_mob.Tactic = tactic;
            post_mob.SpawnFeatures.Add(new MobSpawnMovesOff(post_mob.SpecifiedSkills.Count));
            post_mob.SpawnFeatures.Add(new MobSpawnLoc(loc));
            SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);
            return post_team;
        }

        static List<BaseRoomFilter> GetImmutableFilterList()
        {
            return new List<BaseRoomFilter>() { new RoomFilterComponent(true, new ImmutableRoom()) };
        }
        static List<BaseRoomFilter> GetAntiFilterList(params RoomComponent[] components)
        {
            return new List<BaseRoomFilter>() { new RoomFilterComponent(true, components) };
        }



        static TeamMemberSpawn GetTeamMob(string species, string ability, string move1, string move2, string move3, string move4, RandRange level,
            string tactic = "wander_normal", bool sleeping = false)
        {
            return GetTeamMob(new MonsterID(species, 0, "", Gender.Unknown), ability, move1, move2, move3, move4, level, tactic, sleeping);
        }
        static TeamMemberSpawn GetTeamMob(MonsterID id, string ability, string move1, string move2, string move3, string move4, RandRange level,
            string tactic = "wander_normal", bool sleeping = false)
        {
            return GetTeamMob(id, ability, move1, move2, move3, move4, level, TeamMemberSpawn.MemberRole.Normal, tactic, sleeping);
        }
        static TeamMemberSpawn GetTeamMob(string species, string ability, string move1, string move2, string move3, string move4, RandRange level,
            TeamMemberSpawn.MemberRole role, string tactic = "wander_normal", bool sleeping = false)
        {
            return GetTeamMob(new MonsterID(species, 0, "", Gender.Unknown), ability, move1, move2, move3, move4, level, role, tactic, sleeping);
        }
        static TeamMemberSpawn GetTeamMob(MonsterID id, string ability, string move1, string move2, string move3, string move4, RandRange level,
            TeamMemberSpawn.MemberRole role, string tactic = "wander_normal", bool sleeping = false)
        {
            return new TeamMemberSpawn(GetGenericMob(id, ability, move1, move2, move3, move4, level, tactic, sleeping), role);
        }

        static MobSpawn GetGenericMob(string species, string ability, string move1, string move2, string move3, string move4, RandRange level,
            string tactic = "wander_normal", bool sleeping = false)
        {
            return GetGenericMob(new MonsterID(species, 0, "", Gender.Unknown), ability, move1, move2, move3, move4, level, tactic, sleeping);
        }
        static MobSpawn GetGenericMob(MonsterID id, string ability, string move1, string move2, string move3, string move4, RandRange level,
            string tactic = "wander_normal", bool sleeping = false)
        {
            MobSpawn post_mob = new MobSpawn();
            post_mob.BaseForm = id;
            post_mob.Intrinsic = ability;
            if (!String.IsNullOrEmpty(move1))
                post_mob.SpecifiedSkills.Add(move1);
            if (!String.IsNullOrEmpty(move2))
                post_mob.SpecifiedSkills.Add(move2);
            if (!String.IsNullOrEmpty(move3))
                post_mob.SpecifiedSkills.Add(move3);
            if (!String.IsNullOrEmpty(move4))
                post_mob.SpecifiedSkills.Add(move4);
            post_mob.Level = level;
            post_mob.Tactic = tactic;
            post_mob.SpawnFeatures.Add(new MobSpawnWeak());
            post_mob.SpawnFeatures.Add(new MobSpawnMovesOff(post_mob.SpecifiedSkills.Count));
            if (sleeping)
            {
                StatusEffect sleep = new StatusEffect("sleep");
                sleep.StatusStates.Set(new CountDownState(-1));
                MobSpawnStatus status = new MobSpawnStatus();
                status.Statuses.Add(sleep, 10);
                post_mob.SpawnFeatures.Add(status);
            }
            return post_mob;
        }

        static MobSpawn GetFOEMob(string species, string ability, string move1, string move2, string move3, string move4, int baseLv)
        {
            MobSpawn post_mob = new MobSpawn();
            post_mob.BaseForm = new MonsterID(species, 0, "", Gender.Unknown);
            post_mob.Intrinsic = ability;
            if (!String.IsNullOrEmpty(move1))
                post_mob.SpecifiedSkills.Add(move1);
            if (!String.IsNullOrEmpty(move2))
                post_mob.SpecifiedSkills.Add(move2);
            if (!String.IsNullOrEmpty(move3))
                post_mob.SpecifiedSkills.Add(move3);
            if (!String.IsNullOrEmpty(move4))
                post_mob.SpecifiedSkills.Add(move4);
            post_mob.Tactic = "loot_guard";
            post_mob.Level = new RandRange(baseLv);
            post_mob.SpawnFeatures.Add(new MobSpawnLevelScale(5, 3));
            post_mob.SpawnFeatures.Add(new MobSpawnMovesOff(post_mob.SpecifiedSkills.Count));
            return post_mob;
        }

        static MobSpawn GetHouseMob(string species, string tactic)
        {
            MobSpawn post_mob = new MobSpawn();
            post_mob.BaseForm = new MonsterID(species, 0, "", Gender.Unknown);
            post_mob.Level = new RandRange(1);
            post_mob.Tactic = tactic;
            post_mob.Level = new RandRange(1);
            post_mob.SpawnFeatures.Add(new MobSpawnWeak());
            post_mob.SpawnFeatures.Add(new MobSpawnLevelScale(4, 3));
            return post_mob;
        }

        static MobSpawn GetBossMob(string species, string ability, string move1, string move2, string move3, string move4, string item, Loc loc, string tactic = "boss")
        {
            return GetBossMob(new MonsterID(species, 0, "", Gender.Unknown), ability, move1, move2, move3, move4, item, loc, tactic);
        }

        static MobSpawn GetBossMob(MonsterID id, string ability, string move1, string move2, string move3, string move4, string item, Loc loc, string tactic = "boss")
        {
            MobSpawn post_mob = new MobSpawn();
            post_mob.BaseForm = id;
            post_mob.Intrinsic = ability;
            if (!String.IsNullOrEmpty(move1))
                post_mob.SpecifiedSkills.Add(move1);
            if (!String.IsNullOrEmpty(move2))
                post_mob.SpecifiedSkills.Add(move2);
            if (!String.IsNullOrEmpty(move3))
                post_mob.SpecifiedSkills.Add(move3);
            if (!String.IsNullOrEmpty(move4))
                post_mob.SpecifiedSkills.Add(move4);
            post_mob.Tactic = tactic;
            post_mob.Level = new RandRange(3);
            post_mob.SpawnFeatures.Add(new MobSpawnLoc(loc));
            post_mob.SpawnFeatures.Add(new MobSpawnItem(true, item));
            post_mob.SpawnFeatures.Add(new MobSpawnUnrecruitable());
            post_mob.SpawnFeatures.Add(new MobSpawnLevelScale(4, 3));
            MobSpawnScaledBoost boost = new MobSpawnScaledBoost(new IntRange(1, 50));
            boost.MaxHPBonus = new IntRange(15, MonsterFormData.MAX_STAT_BOOST);
            post_mob.SpawnFeatures.Add(boost);
            return post_mob;
        }


        static MobSpawn GetShopMob(string species, string ability, string move1, string move2, string move3, string move4, string[] items, int keeperId, string tactic = "shopkeeper")
        {
            MobSpawn post_mob = new MobSpawn();
            post_mob.BaseForm = new MonsterID(species, 0, DataManager.Instance.DefaultSkin, Gender.Unknown);
            post_mob.Tactic = tactic;
            post_mob.Level = new RandRange(100);
            post_mob.Intrinsic = ability;
            post_mob.SpecifiedSkills.Add(move1);
            post_mob.SpecifiedSkills.Add(move2);
            post_mob.SpecifiedSkills.Add(move3);
            post_mob.SpecifiedSkills.Add(move4);
            MobSpawnBoost spawnBoost = new MobSpawnBoost();
            spawnBoost.AtkBonus = MonsterFormData.MAX_STAT_BOOST;
            spawnBoost.DefBonus = MonsterFormData.MAX_STAT_BOOST;
            spawnBoost.SpAtkBonus = MonsterFormData.MAX_STAT_BOOST;
            spawnBoost.SpDefBonus = MonsterFormData.MAX_STAT_BOOST;
            spawnBoost.SpeedBonus = MonsterFormData.MAX_STAT_BOOST;
            spawnBoost.MaxHPBonus = MonsterFormData.MAX_STAT_BOOST;
            post_mob.SpawnFeatures.Add(new MobSpawnInv(false, items));
            post_mob.SpawnFeatures.Add(spawnBoost);
            if (keeperId > -1)
            {
                post_mob.SpawnFeatures.Add(new MobSpawnDiscriminator(keeperId));
                post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new BattleScriptEvent("ShopkeeperInteract")));
                post_mob.SpawnFeatures.Add(new MobSpawnLuaTable("{ Role = \"Shopkeeper\" }"));
            }
            return post_mob;
        }

        static ConnectGridBranchStep<MapGenContext> CreateGenericConnect(int rate, int turnBias)
        {
            ConnectGridBranchStep<MapGenContext> step = new ConnectGridBranchStep<MapGenContext>(rate);
            step.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
            step.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom()));
            PresetPicker<PermissiveRoomGen<MapGenContext>> picker = new PresetPicker<PermissiveRoomGen<MapGenContext>>();
            picker.ToSpawn = new RoomGenAngledHall<MapGenContext>(turnBias);
            step.GenericHalls = picker;
            return step;
        }

        static ConnectBranchStep<ListMapGenContext> CreateGenericListConnect(int rate, int turnBias)
        {
            ConnectBranchStep<ListMapGenContext> step = new ConnectBranchStep<ListMapGenContext>();
            step.Components.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
            step.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom()));
            step.ConnectPercent = rate;
            PresetPicker<PermissiveRoomGen<ListMapGenContext>> picker = new PresetPicker<PermissiveRoomGen<ListMapGenContext>>();
            picker.ToSpawn = new RoomGenAngledHall<ListMapGenContext>(turnBias);
            step.GenericHalls = picker;
            return step;
        }

        static string[] ItemArray(IEnumerable<string> iter)
        {
            List<string> result = new List<string>();
            foreach (string item in iter)
                result.Add(item);
            return result.ToArray();
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

        static IEnumerable<string> IterateTMs()
        {
            yield return "tm_secret_power";
            yield return "tm_embargo";
            yield return "tm_echoed_voice";
            yield return "tm_double_team";
            yield return "tm_psychic";
            yield return "tm_toxic";
            yield return "tm_will_o_wisp";
            yield return "tm_dragon_tail";
            yield return "tm_flamethrower";
            yield return "tm_protect";
            yield return "tm_defog";
            yield return "tm_roar";
            yield return "tm_hyper_beam";
            yield return "tm_swagger";
            yield return "tm_dive";
            yield return "tm_fly";
            yield return "tm_facade";
            yield return "tm_rock_climb";
            yield return "tm_waterfall";
            yield return "tm_smack_down";
            yield return "tm_flame_charge";
            yield return "tm_low_sweep";
            yield return "tm_charge_beam";
            yield return "tm_payback";
            yield return "tm_blizzard";
            yield return "tm_wild_charge";
            yield return "tm_hone_claws";
            yield return "tm_telekinesis";
            yield return "tm_giga_impact";
            yield return "tm_rock_polish";
            yield return "tm_dig";
            yield return "tm_gyro_ball";
            yield return "tm_rock_slide";
            yield return "tm_sludge_wave";
            yield return "tm_substitute";
            yield return "tm_trick_room";
            yield return "tm_safeguard";
            yield return "tm_swords_dance";
            yield return "tm_venoshock";
            yield return "tm_work_up";
            yield return "tm_scald";
            yield return "tm_energy_ball";
            yield return "tm_explosion";
            yield return "tm_u_turn";
            yield return "tm_thunder_wave";
            yield return "tm_return";
            yield return "tm_pluck";
            yield return "tm_frustration";
            yield return "tm_fire_blast";
            yield return "tm_ally_switch";
            yield return "tm_flash";
            yield return "tm_thief";
            yield return "tm_acrobatics";
            yield return "tm_thunderbolt";
            yield return "tm_shadow_ball";
            yield return "tm_false_swipe";
            yield return "tm_fling";
            yield return "tm_captivate";
            yield return "tm_roost";
            yield return "tm_infestation";
            yield return "tm_drain_punch";
            yield return "tm_water_pulse";
            yield return "tm_dark_pulse";
            yield return "tm_giga_drain";
            yield return "tm_shock_wave";
            yield return "tm_volt_switch";
            yield return "tm_steel_wing";
            yield return "tm_earthquake";
            yield return "tm_psyshock";
            yield return "tm_bulldoze";
            yield return "tm_poison_jab";
            yield return "tm_frost_breath";
            yield return "tm_dream_eater";
            yield return "tm_thunder";
            yield return "tm_x_scissor";
            yield return "tm_dazzling_gleam";
            yield return "tm_retaliate";
            yield return "tm_reflect";
            yield return "tm_quash";
            yield return "tm_snarl";
            yield return "tm_round";
            yield return "tm_aerial_ace";
            yield return "tm_focus_blast";
            yield return "tm_incinerate";
            yield return "tm_struggle_bug";
            yield return "tm_overheat";
            yield return "tm_dragon_claw";
            yield return "tm_rain_dance";
            yield return "tm_sunny_day";
            yield return "tm_sludge_bomb";
            yield return "tm_sandstorm";
            yield return "tm_hail";
            yield return "tm_rock_tomb";
            yield return "tm_attract";
            yield return "tm_hidden_power";
            yield return "tm_taunt";
            yield return "tm_recycle";
            yield return "tm_ice_beam";
            yield return "tm_flash_cannon";
            yield return "tm_light_screen";
            yield return "tm_stone_edge";
            yield return "tm_shadow_claw";
            yield return "tm_grass_knot";
            yield return "tm_brick_break";
            yield return "tm_calm_mind";
            yield return "tm_torment";
            yield return "tm_strength";
            yield return "tm_cut";
            yield return "tm_rock_smash";
            yield return "tm_bulk_up";
            yield return "tm_surf";
            yield return "tm_rest";
            yield return "tm_psych_up";
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

        static readonly (string, int)[] specific_tms = { ("tm_embargo", 2500), //TM Embargo
                                                ("tm_psychic", 5000), //TM Psychic
                                                ("tm_will_o_wisp", 3500), //TM Will-O-Wisp
                                                ("tm_dragon_tail", 5000), //TM Dragon Tail
                                                ("tm_flamethrower", 5000), //TM Flamethrower
                                                ("tm_dive", 5000), //TM Dive
                                                ("tm_waterfall", 5000), //TM Waterfall
                                                ("tm_smack_down", 3500), //TM Smack Down
                                                ("tm_flame_charge", 5000), //TM Flame Charge
                                                ("tm_low_sweep", 2500), //TM Low Sweep
                                                ("tm_charge_beam", 5000), //TM Charge Beam
                                                ("tm_blizzard", 5000), //TM Blizzard
                                                ("tm_wild_charge", 5000), //TM Wild Charge
                                                ("tm_hone_claws", 5000), //TM Hone Claws
                                                ("tm_telekinesis", 3500), //TM Telekinesis
                                                ("tm_rock_polish", 2500), //TM Rock Polish
                                                ("tm_gyro_ball", 3500), //TM Gyro Ball
                                                ("tm_rock_slide", 5000), //TM Rock Slide
                                                ("tm_sludge_wave", 5000), //TM Sludge Wave
                                                ("tm_trick_room", 3500), //TM Trick Room
                                                ("tm_venoshock", 3500), //TM Venoshock
                                                ("tm_scald", 5000), //TM Scald
                                                ("tm_energy_ball", 5000), //TM Energy Ball
                                                ("tm_explosion", 5000), //TM Explosion
                                                ("tm_u_turn", 3500), //TM U-turn
                                                ("tm_thunder_wave", 3500), //TM Thunder Wave
                                                ("tm_pluck", 5000), //TM Pluck
                                                ("tm_fire_blast", 5000), //TM Fire Blast
                                                ("tm_ally_switch", 2500), //TM Ally Switch
                                                ("tm_acrobatics", 5000), //TM Acrobatics
                                                ("tm_thunderbolt", 5000), //TM Thunderbolt
                                                ("tm_shadow_ball", 5000), //TM Shadow Ball
                                                ("tm_false_swipe", 5000), //TM False Swipe
                                                ("tm_roost", 5000), //TM Roost
                                                ("tm_infestation", 2500), //TM Infestation
                                                ("tm_drain_punch", 5000), //TM Drain Punch
                                                ("tm_water_pulse", 5000), //TM Water Pulse
                                                ("tm_dark_pulse", 5000), //TM Dark Pulse
                                                ("tm_giga_drain", 3500), //TM Giga Drain
                                                ("tm_shock_wave", 5000), //TM Shock Wave
                                                ("tm_volt_switch", 5000), //TM Volt Switch
                                                ("tm_steel_wing", 5000), //TM Steel Wing
                                                ("tm_psyshock", 5000), //TM Psyshock
                                                ("tm_bulldoze", 3500), //TM Bulldoze
                                                ("tm_poison_jab", 5000), //TM Poison Jab
                                                ("tm_frost_breath", 5000), //TM Frost Breath
                                                ("tm_dream_eater", 3500), //TM Dream Eater
                                                ("tm_thunder", 5000), //TM Thunder
                                                ("tm_x_scissor", 5000), //TM X-Scissor
                                                ("tm_dazzling_gleam", 5000), //TM Dazzling Gleam
                                                ("tm_retaliate", 3500), //TM Retaliate
                                                ("tm_quash", 2500), //TM Quash
                                                ("tm_snarl", 2500), //TM Snarl
                                                ("tm_aerial_ace", 2500), //TM Aerial Ace
                                                ("tm_focus_blast", 5000), //TM Focus Blast
                                                ("tm_incinerate", 2500), //TM Incinerate
                                                ("tm_struggle_bug", 2500), //TM Struggle Bug
                                                ("tm_overheat", 5000), //TM Overheat
                                                ("tm_dragon_claw", 3500), //TM Dragon Claw
                                                ("tm_sludge_bomb", 5000), //TM Sludge Bomb
                                                ("tm_rock_tomb", 5000), //TM Rock Tomb
                                                ("tm_recycle", 3500), //TM Recycle
                                                ("tm_ice_beam", 5000), //TM Ice Beam
                                                ("tm_flash_cannon", 5000), //TM Flash Cannon
                                                ("tm_stone_edge", 5000), //TM Stone Edge
                                                ("tm_shadow_claw", 5000), //TM Shadow Claw
                                                ("tm_brick_break", 5000), //TM Brick Break
                                                ("tm_calm_mind", 5000), //TM Calm Mind
                                                ("tm_torment", 2500), //TM Torment
                                                ("tm_bulk_up", 5000) //TM Bulk Up
                                              };
    }
}
