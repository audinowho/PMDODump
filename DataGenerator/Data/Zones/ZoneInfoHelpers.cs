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
        public static void AddDefaultMapStatus<T>(MapGen<T> layout, int setterID, params int[] defaultStatus) where T : BaseMapGenContext
        {
            DefaultMapStatusStep<T> statusData = new DefaultMapStatusStep<T>(setterID, defaultStatus);
            layout.GenSteps.Add(PR_FLOOR_DATA, statusData);
        }

        public static void AddTextureData<T>(MapGen<T> layout, int block, int ground, int water, int element, bool independent = false) where T : BaseMapGenContext
        {
            MapTextureStep<T> textureStep = new MapTextureStep<T>();
            {
                textureStep.GroundTileset = ground;
                textureStep.GroundElement = element;
                textureStep.WaterTileset = water;
                textureStep.BlockTileset = block;
                textureStep.IndependentGround = independent;
            }
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


        public static void AddInitGridStep<T>(MapGen<T> layout, int cellX, int cellY, int cellWidth, int cellHeight, int thickness = 1) where T : MapGenContext
        {
            InitGridPlanStep<T> startGen = new InitGridPlanStep<T>(thickness);
            {
                startGen.CellX = cellX;
                startGen.CellY = cellY;

                startGen.CellWidth = cellWidth;
                startGen.CellHeight = cellHeight;
            }
            layout.GenSteps.Add(PR_GRID_INIT, startGen);
        }

        public static void AddInitListStep<T>(MapGen<T> layout, int width, int height) where T : ListMapGenContext
        {
            InitFloorPlanStep<T> startGen = new InitFloorPlanStep<T>();
            startGen.Width = width;
            startGen.Height = height;
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

        public static void AddWaterSteps<T>(MapGen<T> layout, int terrain, RandRange percent, bool eraseIsolated = true) where T : BaseMapGenContext
        {
            PerlinWaterStep<T> waterStep = new PerlinWaterStep<T>(new RandRange(), 3, new Tile(terrain), 1, true);
            waterStep.WaterPercent = percent;
            layout.GenSteps.Add(PR_WATER, waterStep);
            layout.GenSteps.Add(PR_WATER_DIAG, new DropDiagonalBlockStep<T>(new Tile(terrain)));
            if (eraseIsolated)
                layout.GenSteps.Add(PR_WATER_DE_ISOLATE, new EraseIsolatedStep<T>(new Tile(terrain)));
        }


        public static void AddSingleTrapStep<T>(MapGen<T> layout, RandRange amount, int id, bool revealed = true, bool includeHalls = false, ConnectivityRoom.Connectivity connectivity = ConnectivityRoom.Connectivity.Main) where T : ListMapGenContext
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
            var step = new FloorStairsStep<T, MapGenEntrance, MapGenExit>(new MapGenEntrance(Dir8.Down), new MapGenExit(new EffectTile(goDown ? 2 : 1, true)));
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
            RoomGenSpecific<T> roomGen = new RoomGenSpecific<T>(level[0].Length, level.Length, new Tile(0), false);
            roomGen.Tiles = new Tile[level[0].Length][];
            for (int xx = 0; xx < level[0].Length; xx++)
            {
                roomGen.Tiles[xx] = new Tile[level.Length];
                for (int yy = 0; yy < level.Length; yy++)
                {
                    if (level[yy][xx] == '#')
                        roomGen.Tiles[xx][yy] = new Tile(2);
                    else if (level[yy][xx] == '~')
                        roomGen.Tiles[xx][yy] = new Tile(3);
                    else if (level[yy][xx] == '^')
                        roomGen.Tiles[xx][yy] = new Tile(4);
                    else if (level[yy][xx] == '_')
                        roomGen.Tiles[xx][yy] = new Tile(5);
                    else
                        roomGen.Tiles[xx][yy] = new Tile(0);
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
            RoomGenSpecificBoss<T> roomGen = new RoomGenSpecificBoss<T>(level[0].Length, level.Length, new Tile(0), false, 38, trigger, severe ? "C02. Boss Battle 2.ogg" : "C01. Boss Battle.ogg");
            roomGen.Bosses = mobs;
            roomGen.Tiles = new Tile[level[0].Length][];
            for (int xx = 0; xx < level[0].Length; xx++)
            {
                roomGen.Tiles[xx] = new Tile[level.Length];
                for (int yy = 0; yy < level.Length; yy++)
                {
                    if (level[yy][xx] == 'X')
                        roomGen.Tiles[xx][yy] = new Tile(1);
                    else if (level[yy][xx] == '#')
                        roomGen.Tiles[xx][yy] = new Tile(2);
                    else if (level[yy][xx] == '~')
                        roomGen.Tiles[xx][yy] = new Tile(3);
                    else if (level[yy][xx] == '^')
                        roomGen.Tiles[xx][yy] = new Tile(4);
                    else if (level[yy][xx] == '_')
                        roomGen.Tiles[xx][yy] = new Tile(5);
                    else
                        roomGen.Tiles[xx][yy] = new Tile(0);
                }
            }
            return roomGen;
        }

        public static RoomGenPostProcSpecific<MapGenContext> CreateRoomGenPostProcSpecific(string[] level)
        {
            RoomGenPostProcSpecific<MapGenContext> roomGen = new RoomGenPostProcSpecific<MapGenContext>(level[0].Length, level.Length, new Tile(0), false);
            roomGen.Tiles = new Tile[level[0].Length][];
            roomGen.PostProcMask = new PostProcTile[level[0].Length][];
            for (int xx = 0; xx < level[0].Length; xx++)
            {
                roomGen.Tiles[xx] = new Tile[level.Length];
                roomGen.PostProcMask[xx] = new PostProcTile[level.Length];
                for (int yy = 0; yy < level.Length; yy++)
                {
                    if (level[yy][xx] == 'X')
                        roomGen.Tiles[xx][yy] = new Tile(1);
                    else if (level[yy][xx] == '#')
                        roomGen.Tiles[xx][yy] = new Tile(2);
                    else if (level[yy][xx] == '~')
                        roomGen.Tiles[xx][yy] = new Tile(3);
                    else if (level[yy][xx] == '^')
                        roomGen.Tiles[xx][yy] = new Tile(4);
                    else if (level[yy][xx] == '_')
                        roomGen.Tiles[xx][yy] = new Tile(5);
                    else
                        roomGen.Tiles[xx][yy] = new Tile(0);
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

        static SpecificTeamSpawner CreateSetMobTeam(Loc loc, int species, int ability, int move1, int move2, int move3, int move4, int level, int tactic = 18)
        {
            MobSpawn post_mob = new MobSpawn();
            post_mob.BaseForm = new MonsterID(species, 0, 0, Gender.Unknown);
            post_mob.Intrinsic = ability;
            if (move1 > 0)
                post_mob.SpecifiedSkills.Add(move1);
            if (move2 > 0)
                post_mob.SpecifiedSkills.Add(move2);
            if (move3 > 0)
                post_mob.SpecifiedSkills.Add(move3);
            if (move4 > 0)
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



        static TeamMemberSpawn GetTeamMob(int species, int ability, int move1, int move2, int move3, int move4, RandRange level,
            int tactic = 7, bool sleeping = false)
        {
            return GetTeamMob(new MonsterID(species, 0, -1, Gender.Unknown), ability, move1, move2, move3, move4, level, tactic, sleeping);
        }
        static TeamMemberSpawn GetTeamMob(MonsterID id, int ability, int move1, int move2, int move3, int move4, RandRange level,
            int tactic = 7, bool sleeping = false)
        {
            return GetTeamMob(id, ability, move1, move2, move3, move4, level, TeamMemberSpawn.MemberRole.Normal, tactic, sleeping);
        }
        static TeamMemberSpawn GetTeamMob(int species, int ability, int move1, int move2, int move3, int move4, RandRange level,
            TeamMemberSpawn.MemberRole role, int tactic = 7, bool sleeping = false)
        {
            return GetTeamMob(new MonsterID(species, 0, -1, Gender.Unknown), ability, move1, move2, move3, move4, level, role, tactic, sleeping);
        }
        static TeamMemberSpawn GetTeamMob(MonsterID id, int ability, int move1, int move2, int move3, int move4, RandRange level,
            TeamMemberSpawn.MemberRole role, int tactic = 7, bool sleeping = false)
        {
            return new TeamMemberSpawn(GetGenericMob(id, ability, move1, move2, move3, move4, level, tactic, sleeping), role);
        }

        static MobSpawn GetGenericMob(int species, int ability, int move1, int move2, int move3, int move4, RandRange level,
            int tactic = 7, bool sleeping = false)
        {
            return GetGenericMob(new MonsterID(species, 0, -1, Gender.Unknown), ability, move1, move2, move3, move4, level, tactic, sleeping);
        }
        static MobSpawn GetGenericMob(MonsterID id, int ability, int move1, int move2, int move3, int move4, RandRange level,
            int tactic = 7, bool sleeping = false)
        {
            MobSpawn post_mob = new MobSpawn();
            post_mob.BaseForm = id;
            post_mob.Intrinsic = ability;
            if (move1 > 0)
                post_mob.SpecifiedSkills.Add(move1);
            if (move2 > 0)
                post_mob.SpecifiedSkills.Add(move2);
            if (move3 > 0)
                post_mob.SpecifiedSkills.Add(move3);
            if (move4 > 0)
                post_mob.SpecifiedSkills.Add(move4);
            post_mob.Level = level;
            post_mob.Tactic = tactic;
            post_mob.SpawnFeatures.Add(new MobSpawnWeak());
            post_mob.SpawnFeatures.Add(new MobSpawnMovesOff(post_mob.SpecifiedSkills.Count));
            if (sleeping)
            {
                StatusEffect sleep = new StatusEffect(1);
                sleep.StatusStates.Set(new CountDownState(-1));
                MobSpawnStatus status = new MobSpawnStatus();
                status.Statuses.Add(sleep, 10);
                post_mob.SpawnFeatures.Add(status);
            }
            return post_mob;
        }

        static MobSpawn GetFOEMob(int species, int ability, int move1, int move2, int move3, int move4, int baseLv)
        {
            MobSpawn post_mob = new MobSpawn();
            post_mob.BaseForm = new MonsterID(species, 0, -1, Gender.Unknown);
            post_mob.Intrinsic = ability;
            if (move1 > 0)
                post_mob.SpecifiedSkills.Add(move1);
            if (move2 > 0)
                post_mob.SpecifiedSkills.Add(move2);
            if (move3 > 0)
                post_mob.SpecifiedSkills.Add(move3);
            if (move4 > 0)
                post_mob.SpecifiedSkills.Add(move4);
            post_mob.Tactic = 19;
            post_mob.Level = new RandRange(baseLv);
            post_mob.SpawnFeatures.Add(new MobSpawnLevelScale(5, 3));
            post_mob.SpawnFeatures.Add(new MobSpawnMovesOff(post_mob.SpecifiedSkills.Count));
            return post_mob;
        }

        static MobSpawn GetHouseMob(int species, int tactic)
        {
            MobSpawn post_mob = new MobSpawn();
            post_mob.BaseForm = new MonsterID(species, 0, -1, Gender.Unknown);
            post_mob.Level = new RandRange(1);
            post_mob.Tactic = tactic;
            post_mob.Level = new RandRange(1);
            post_mob.SpawnFeatures.Add(new MobSpawnWeak());
            post_mob.SpawnFeatures.Add(new MobSpawnLevelScale(4, 3));
            return post_mob;
        }

        static MobSpawn GetBossMob(int species, int ability, int move1, int move2, int move3, int move4, int item, Loc loc, int tactic = 20)
        {
            return GetBossMob(new MonsterID(species, 0, -1, Gender.Unknown), ability, move1, move2, move3, move4, item, loc, tactic);
        }

        static MobSpawn GetBossMob(MonsterID id, int ability, int move1, int move2, int move3, int move4, int item, Loc loc, int tactic = 20)
        {
            MobSpawn post_mob = new MobSpawn();
            post_mob.BaseForm = id;
            post_mob.Intrinsic = ability;
            if (move1 > 0)
                post_mob.SpecifiedSkills.Add(move1);
            if (move2 > 0)
                post_mob.SpecifiedSkills.Add(move2);
            if (move3 > 0)
                post_mob.SpecifiedSkills.Add(move3);
            if (move4 > 0)
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


        static MobSpawn GetShopMob(int species, int ability, int move1, int move2, int move3, int move4, int[] items, int keeperId, int tactic = 23)
        {
            MobSpawn post_mob = new MobSpawn();
            post_mob.BaseForm = new MonsterID(species, 0, 0, Gender.Unknown);
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


        public static void CreateContentLists()
        {
            List<string> monsters = new List<string>();
            List<string> skills = new List<string>();
            List<string> intrinsics = new List<string>();
            List<string> statuses = new List<string>();
            List<string> ai = new List<string>();
            List<string> items = new List<string>();
            List<string> roles = new List<string>();



            for (int ii = 0; ii < DataManager.Instance.DataIndices[DataManager.DataType.Monster].Count; ii++)
                monsters.Add((ii).ToString("D3") + ": " + DataManager.Instance.DataIndices[DataManager.DataType.Monster].Entries[ii].Name.DefaultText);

            for (int ii = 0; ii < DataManager.Instance.DataIndices[DataManager.DataType.Skill].Count; ii++)
                skills.Add((ii).ToString("D3") + ": " + DataManager.Instance.DataIndices[DataManager.DataType.Skill].Entries[ii].Name.DefaultText);

            for (int ii = 0; ii < DataManager.Instance.DataIndices[DataManager.DataType.Intrinsic].Count; ii++)
                intrinsics.Add((ii).ToString("D3") + ": " + DataManager.Instance.DataIndices[DataManager.DataType.Intrinsic].Entries[ii].Name.DefaultText);

            for (int ii = 0; ii < DataManager.Instance.DataIndices[DataManager.DataType.Status].Count; ii++)
            {
                StatusData data = DataManager.Instance.GetStatus(ii);
                if (data.MenuName && data.Name.DefaultText != "" && data.StatusStates.Contains<TransferStatusState>())
                    statuses.Add((ii).ToString("D3") + ": " + data.Name.DefaultText);
            }

            for (int ii = 0; ii < DataManager.Instance.DataIndices[DataManager.DataType.AI].Count; ii++)
                ai.Add((ii).ToString("D2") + ": " + DataManager.Instance.DataIndices[DataManager.DataType.AI].Entries[ii].Name.DefaultText);

            for (int ii = 0; ii < DataManager.Instance.DataIndices[DataManager.DataType.Item].Count; ii++)
                items.Add((ii).ToString("D4") + ": " + DataManager.Instance.DataIndices[DataManager.DataType.Item].Entries[ii].Name.DefaultText);

            for (int ii = 0; ii <= (int)TeamMemberSpawn.MemberRole.Loner; ii++)
                roles.Add(((TeamMemberSpawn.MemberRole)ii).ToString());


            string path = GenPath.ZONE_PATH + "PreZone.txt";
            using (StreamWriter file = new StreamWriter(path))
            {
                file.WriteLine("Monster\tSkill\tIntrinsic\tStatus\tAI\tItem\tRole");
                file.WriteLine("---: NULL\t---: NULL\t---: NULL\t---: NULL\t--: NULL\t----: NULL\t---");
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

        static readonly int[][] specific_tms = { new int[] { 0588, 2500 }, //TM Embargo
                                                new int[] { 0591, 5000 }, //TM Psychic
                                                new int[] { 0593, 3500 }, //TM Will-O-Wisp
                                                new int[] { 0594, 5000 }, //TM Dragon Tail
                                                new int[] { 0595, 5000 }, //TM Flamethrower
                                                new int[] { 0601, 5000 }, //TM Dive
                                                new int[] { 0605, 5000 }, //TM Waterfall
                                                new int[] { 0606, 3500 }, //TM Smack Down
                                                new int[] { 0607, 5000 }, //TM Flame Charge
                                                new int[] { 0608, 2500 }, //TM Low Sweep
                                                new int[] { 0609, 5000 }, //TM Charge Beam
                                                new int[] { 0611, 5000 }, //TM Blizzard
                                                new int[] { 0612, 5000 }, //TM Wild Charge
                                                new int[] { 0613, 5000 }, //TM Hone Claws
                                                new int[] { 0614, 3500 }, //TM Telekinesis
                                                new int[] { 0616, 2500 }, //TM Rock Polish
                                                new int[] { 0618, 3500 }, //TM Gyro Ball
                                                new int[] { 0619, 5000 }, //TM Rock Slide
                                                new int[] { 0620, 5000 }, //TM Sludge Wave
                                                new int[] { 0622, 3500 }, //TM Trick Room
                                                new int[] { 0625, 3500 }, //TM Venoshock
                                                new int[] { 0627, 5000 }, //TM Scald
                                                new int[] { 0628, 5000 }, //TM Energy Ball
                                                new int[] { 0629, 5000 }, //TM Explosion
                                                new int[] { 0630, 3500 }, //TM U-turn
                                                new int[] { 0631, 3500 }, //TM Thunder Wave
                                                new int[] { 0633, 5000 }, //TM Pluck
                                                new int[] { 0635, 5000 }, //TM Fire Blast
                                                new int[] { 0636, 2500 }, //TM Ally Switch
                                                new int[] { 0639, 5000 }, //TM Acrobatics
                                                new int[] { 0640, 5000 }, //TM Thunderbolt
                                                new int[] { 0641, 5000 }, //TM Shadow Ball
                                                new int[] { 0642, 5000 }, //TM False Swipe
                                                new int[] { 0645, 5000 }, //TM Roost
                                                new int[] { 0646, 2500 }, //TM Infestation
                                                new int[] { 0647, 5000 }, //TM Drain Punch
                                                new int[] { 0648, 5000 }, //TM Water Pulse
                                                new int[] { 0649, 5000 }, //TM Dark Pulse
                                                new int[] { 0650, 3500 }, //TM Giga Drain
                                                new int[] { 0651, 5000 }, //TM Shock Wave
                                                new int[] { 0652, 5000 }, //TM Volt Switch
                                                new int[] { 0653, 5000 }, //TM Steel Wing
                                                new int[] { 0655, 5000 }, //TM Psyshock
                                                new int[] { 0656, 3500 }, //TM Bulldoze
                                                new int[] { 0657, 5000 }, //TM Poison Jab
                                                new int[] { 0658, 5000 }, //TM Frost Breath
                                                new int[] { 0659, 3500 }, //TM Dream Eater
                                                new int[] { 0660, 5000 }, //TM Thunder
                                                new int[] { 0661, 5000 }, //TM X-Scissor
                                                new int[] { 0662, 5000 }, //TM Dazzling Gleam
                                                new int[] { 0663, 3500 }, //TM Retaliate
                                                new int[] { 0665, 2500 }, //TM Quash
                                                new int[] { 0666, 2500 }, //TM Snarl
                                                new int[] { 0668, 2500 }, //TM Aerial Ace
                                                new int[] { 0669, 5000 }, //TM Focus Blast
                                                new int[] { 0670, 2500 }, //TM Incinerate
                                                new int[] { 0671, 2500 }, //TM Struggle Bug
                                                new int[] { 0672, 5000 }, //TM Overheat
                                                new int[] { 0673, 3500 }, //TM Dragon Claw
                                                new int[] { 0676, 5000 }, //TM Sludge Bomb
                                                new int[] { 0679, 5000 }, //TM Rock Tomb
                                                new int[] { 0683, 3500 }, //TM Recycle
                                                new int[] { 0684, 5000 }, //TM Ice Beam
                                                new int[] { 0685, 5000 }, //TM Flash Cannon
                                                new int[] { 0687, 5000 }, //TM Stone Edge
                                                new int[] { 0688, 5000 }, //TM Shadow Claw
                                                new int[] { 0690, 5000 }, //TM Brick Break
                                                new int[] { 0691, 5000 }, //TM Calm Mind
                                                new int[] { 0692, 2500 }, //TM Torment
                                                new int[] { 0696, 5000 } //TM Bulk Up
                                              };
    }
}
