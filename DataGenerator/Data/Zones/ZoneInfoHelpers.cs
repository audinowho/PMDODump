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
using System.Linq;

namespace DataGenerator.Data
{
    public partial class ZoneInfo
    {
        //GUIDE TO MAP GENERATION PRIORITY:
        //-7 starting with a preloaded map
        static readonly Priority PR_FILE_LOAD = new Priority(-7);
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
        static readonly Priority PR_ROOMS_GEN = new Priority(-2);
        //-2.1 special rooms, extra rooms added
        //-2 path generation (list version), adding Freeform connections
        static readonly Priority PR_ROOMS_PRE_VAULT = new Priority(-2, 1);
        static readonly Priority PR_ROOMS_GEN_EXTRA = new Priority(-2, 2);
        static readonly Priority PR_ROOMS_PRE_VAULT_CLAMP = new Priority(-2, 5);
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
        //4.1 monster houses
        static readonly Priority PR_HOUSES = new Priority(4, 1);
        //4.2 shops
        static readonly Priority PR_SHOPS = new Priority(4, 2);
        //5 traps
        static readonly Priority PR_SPAWN_TRAPS = new Priority(5);
        //5.1 compass
        static readonly Priority PR_COMPASS = new Priority(5, 1);
        //6 money
        static readonly Priority PR_SPAWN_MONEY = new Priority(6);
        //6.1 items
        static readonly Priority PR_SPAWN_ITEMS = new Priority(6, 1);
        //6.1.1 extra items
        static readonly Priority PR_SPAWN_ITEMS_EXTRA = new Priority(6, 1, 1);
        //6.2 mobs
        static readonly Priority PR_SPAWN_MOBS = new Priority(6, 2);
        //6.2.1 extra mobs
        static readonly Priority PR_SPAWN_MOBS_EXTRA = new Priority(6, 2, 1);
        //7 debug checks
        static readonly Priority PR_DBG_CHECK = new Priority(7);

        public static void AddTitleDrop<T>(MapGen<T> layout) where T : BaseMapGenContext
        {
            MapTitleDropStep<T> fade = new MapTitleDropStep<T>(new Priority(-15));
            layout.GenSteps.Add(PR_FLOOR_DATA, fade);
        }

        public static void AddFloorData<T>(MapGen<T> layout, string music, int timeLimit, Map.SightRange tileSight, Map.SightRange charSight) where T : BaseMapGenContext
        {
            MapDataStep<T> floorData = new MapDataStep<T>(music, timeLimit, tileSight, charSight);
            layout.GenSteps.Add(PR_FLOOR_DATA, floorData);
        }

        public static void AddFloorFakeItems<T>(MapGen<T> layout, Dictionary<ItemFake, MobSpawn> spawnTable) where T : BaseMapGenContext
        {
            MapEffectStep<T> fake = new MapEffectStep<T>();
            fake.Effect.OnPickups.Add(-1, new FakeItemEvent(spawnTable));
            fake.Effect.OnEquips.Add(-1, new FakeItemEvent(spawnTable));
            fake.Effect.BeforeActions.Add(-5, new FakeItemBattleEvent(spawnTable));
            layout.GenSteps.Add(PR_FLOOR_DATA, fake);
        }

        public static void AddItemSpreadZoneStep(ZoneSegmentBase floorSegment, SpreadPlanBase spreadPlan, params MapItem[] items)
        {
            SpawnList<MapItem> zoneSpawns = new SpawnList<MapItem>();
            foreach (MapItem item in items)
                zoneSpawns.Add(item, 10);
            AddItemSpreadZoneStep(floorSegment, spreadPlan, zoneSpawns);
        }

        public static void AddItemSpreadZoneStep(ZoneSegmentBase floorSegment, SpreadPlanBase spreadPlan, SpawnList<MapItem> items)
        {
            SpawnList<IGenStep> zoneSpawns = new SpawnList<IGenStep>();
            foreach (SpawnList<MapItem>.SpawnRate spawnRate in items)
                zoneSpawns.Add(new RandomSpawnStep<ListMapGenContext, MapItem>(new PickerSpawner<ListMapGenContext, MapItem>(new PresetMultiRand<MapItem>(spawnRate.Spawn))), spawnRate.Rate);
            SpreadStepZoneStep zoneStep = new SpreadStepZoneStep(spreadPlan, PR_SPAWN_ITEMS, zoneSpawns);
            floorSegment.ZoneSteps.Add(zoneStep);
        }


        public static void AddEvoZoneStep(ZoneSegmentBase floorSegment, SpreadPlanBase spreadPlan, bool small)
        {
            SpreadRoomZoneStep evoZoneStep = new SpreadRoomZoneStep(PR_GRID_GEN_EXTRA, PR_ROOMS_GEN_EXTRA, spreadPlan);
            List<BaseRoomFilter> evoFilters = new List<BaseRoomFilter>();
            evoFilters.Add(new RoomFilterComponent(true, new ImmutableRoom()));
            evoFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
            if (small)
                evoZoneStep.Spawns.Add(new RoomGenOption(new RoomGenEvoSmall<MapGenContext>(), new RoomGenEvoSmall<ListMapGenContext>(), evoFilters), 10);
            else
                evoZoneStep.Spawns.Add(new RoomGenOption(new RoomGenEvo<MapGenContext>(), new RoomGenEvo<ListMapGenContext>(), evoFilters), 10);
            floorSegment.ZoneSteps.Add(evoZoneStep);
        }

        public static void AddHiddenStairStep(ZoneSegmentBase floorSegment, SpreadPlanBase spreadPlan, int segDiff)
        {
            SpawnRangeList<IGenStep> exitZoneSpawns = new SpawnRangeList<IGenStep>();
            EffectTile secretTile = new EffectTile("stairs_secret_down", false);
            secretTile.TileStates.Set(new DestState(new SegLoc(segDiff, 0), true));
            RandomSpawnStep<BaseMapGenContext, EffectTile> trapStep = new RandomSpawnStep<BaseMapGenContext, EffectTile>(new PickerSpawner<BaseMapGenContext, EffectTile>(new PresetMultiRand<EffectTile>(secretTile)));
            exitZoneSpawns.Add(trapStep, spreadPlan.FloorRange, 10);
            SpreadStepRangeZoneStep exitZoneStep = new SpreadStepRangeZoneStep(spreadPlan, PR_SPAWN_TRAPS, exitZoneSpawns);
            exitZoneStep.ModStates.Add(new FlagType(typeof(StairsModGenState)));
            floorSegment.ZoneSteps.Add(exitZoneStep);
        }

        public static void AddMysteriosityZoneStep(ZoneSegmentBase floorSegment, SpreadPlanBase spreadPlan, int baseChance, int segDiff)
        {
            SpawnRangeList<IGenStep> exitZoneSpawns = new SpawnRangeList<IGenStep>();
            exitZoneSpawns.Add(new ScriptGenStep<ListMapGenContext>("Mysteriosity", "{BaseChance="+baseChance+", SegDiff="+segDiff+"}"), spreadPlan.FloorRange, 10);
            SpreadStepRangeZoneStep exitZoneStep = new SpreadStepRangeZoneStep(spreadPlan, PR_SPAWN_TRAPS, exitZoneSpawns);
            floorSegment.ZoneSteps.Add(exitZoneStep);
        }

        public static void AddTutorZoneStep(ZoneSegmentBase floorSegment, SpreadPlanBase spreadPlan, IntRange cost, List<string> tutorElements)
        {
            RandBag<IGenStep> npcZoneSpawns = new RandBag<IGenStep>();
            List<string> quotedElements = new List<string>();
            foreach (string element in tutorElements)
                quotedElements.Add(String.Format("\"{0}\"", element));
            npcZoneSpawns.ToSpawn.Add(new ScriptGenStep<ListMapGenContext>("SpawnRandomTutor", "{ MinCost = "+ cost.Min +", MaxCost = "+ cost.Max+", Elements = {" + String.Join(", ", quotedElements) + "}}"));
            SpreadStepZoneStep exitZoneStep = new SpreadStepZoneStep(spreadPlan, PR_SPAWN_MOBS_EXTRA, npcZoneSpawns);
            floorSegment.ZoneSteps.Add(exitZoneStep);
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

        public static void AddTextureData<T>(MapGen<T> layout, string block, string ground, string water, string element, bool layered = false) where T : BaseMapGenContext
        {
            MapTextureStep<T> textureStep = new MapTextureStep<T>();
            {
                textureStep.GroundElement = element;
                textureStep.GroundTileset = ground;
                textureStep.WaterTileset = water;
                textureStep.BlockTileset = block;
                textureStep.LayeredGround = layered;
            }
            layout.GenSteps.Add(PR_TEXTURES, textureStep);
        }

        public static void AddSpecificTextureData<T>(MapGen<T> layout, string block, string ground, string water, string grass, string element) where T : BaseMapGenContext
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
                textureStep.TextureMap["water_poison"] = water;
                textureStep.TextureMap["grass"] = grass;
            }
            textureStep.GroundElement = element;
            textureStep.LayeredGround = true;
            layout.GenSteps.Add(PR_TEXTURES, textureStep);

        }

        public static void AddRespawnData<T>(MapGen<T> layout, int maxFoes, int respawnTime) where T : BaseMapGenContext
        {
            MobSpawnSettingsStep<T> spawnStep = new MobSpawnSettingsStep<T>(new Priority(15), new RespawnFromEligibleEvent(maxFoes, respawnTime));
            layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);
        }

        public static void AddRadiusRespawnData<T>(MapGen<T> layout, int radius, int maxFoes, int respawnTime) where T : BaseMapGenContext
        {
            RespawnFromRandomEvent respawn = new RespawnFromRandomEvent(maxFoes, respawnTime);
            respawn.Radius = radius;
            MobSpawnSettingsStep<T> spawnStep = new MobSpawnSettingsStep<T>(new Priority(15), respawn);
            layout.GenSteps.Add(PR_RESPAWN_MOB, spawnStep);
        }

        public static void AddRadiusDespawnData<T>(MapGen<T> layout, int radius, int despawnTime) where T : BaseMapGenContext
        {
            DespawnRadiusEvent despawn = new DespawnRadiusEvent(radius, despawnTime);
            MapEffectStep<T> spawnStep = new MapEffectStep<T>();
            spawnStep.Effect.OnMapTurnEnds.Add(new Priority(15), despawn);
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

        public static void AddRadiusEnemySpawnData<T>(MapGen<T> layout, int radius, RandRange amount) where T : ListMapGenContext
        {
            PlaceRadiusMobsStep<T> mobStep = new PlaceRadiusMobsStep<T>(new TeamContextSpawner<T>(amount));
            mobStep.Radius = radius;
            layout.GenSteps.Add(PR_SPAWN_MOBS, mobStep);
        }

        public static MoneySpawnZoneStep GetMoneySpawn(int level, int floors_in)
        {
            RandRange addRange = new RandRange(level * 2 / 5, level * 2 / 5 + 4);
            RandRange startRange = new RandRange(level * 5 + 25 + floors_in * addRange.Min, level * 5 + 30 + floors_in * addRange.Max);
            return new MoneySpawnZoneStep(PR_RESPAWN_MONEY, startRange, addRange);
        }

        public static SpeciesItemListSpawner<T> GetLegendaryItemSpawner<T>(bool subLegend, bool boxLegend, bool mythical) where T : BaseMapGenContext
        {
            return GetLegendaryItemSpawner<T>(subLegend, boxLegend, mythical, new RandRange(1));
        }

        public static SpeciesItemListSpawner<T> GetLegendaryItemSpawner<T>(bool subLegend, bool boxLegend, bool mythical, RandRange rand) where T : BaseMapGenContext
        {
            SpeciesItemListSpawner<T> spawn = new SpeciesItemListSpawner<T>(new IntRange(1), rand);

            foreach (string legend in IterateLegendaries(subLegend, false, boxLegend, mythical))
                spawn.Species.Add(legend);

            return spawn;
        }

        public static void AddMoneyData<T>(MapGen<T> layout, RandRange divAmount, bool includeHalls = false, ConnectivityRoom.Connectivity connectivity = ConnectivityRoom.Connectivity.None) where T : ListMapGenContext
        {
            TerminalSpawnStep<T, MoneySpawn> moneyStep = new TerminalSpawnStep<T, MoneySpawn>(new MoneyDivSpawner<T>(divAmount), includeHalls);
            if (connectivity != ConnectivityRoom.Connectivity.None)
                moneyStep.Filters.Add(new RoomFilterConnectivity(connectivity));
            layout.GenSteps.Add(PR_SPAWN_MONEY, moneyStep);
        }

        public static void AddMoneyTrails<T>(MapGen<T> layout, RandRange trailLength, IntRange placementValue, IStepSpawner<T, MapItem> terminalSpawn, ConnectivityRoom.Connectivity connectivity = ConnectivityRoom.Connectivity.None) where T : MapGenContext
        {
            MoneyTrailSpawnStep<T, MapItem> moneyStep = new MoneyTrailSpawnStep<T, MapItem>(terminalSpawn, trailLength, placementValue);
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

        /// <summary>
        /// Adds water based on perlin noise.  Respects walkable tiles.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layout"></param>
        /// <param name="terrain"></param>
        /// <param name="percent"></param>
        /// <param name="eraseIsolated"></param>
        public static void AddWaterSteps<T>(MapGen<T> layout, string terrain, RandRange percent, bool eraseIsolated = true) where T : BaseMapGenContext
        {
            PerlinWaterStep<T> waterStep = new PerlinWaterStep<T>(new RandRange(), 3, new Tile(terrain), new MapTerrainStencil<T>(false, true, false, false), 1);
            waterStep.WaterPercent = percent;
            layout.GenSteps.Add(PR_WATER, waterStep);
            layout.GenSteps.Add(PR_WATER_DIAG, new DropDiagonalBlockStep<T>(new Tile(terrain)));
            if (eraseIsolated)
                layout.GenSteps.Add(PR_WATER_DE_ISOLATE, new EraseIsolatedStep<T>(new Tile(terrain)));
        }

        /// <summary>
        /// Adds water a number of times with blobs.  These blobs can eat into walkable tiles, but respect chokepoints.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layout"></param>
        /// <param name="terrain"></param>
        /// <param name="amount"></param>
        /// <param name="amount"></param>
        /// <param name="eraseIsolated"></param>
        public static void AddBlobWaterSteps<T>(MapGen<T> layout, string terrain, RandRange amount, IntRange size, bool eraseIsolated = true) where T : StairsMapGenContext
        {
            MultiBlobStencil<T> multiBlobStencil = new MultiBlobStencil<T>(false);

            if (eraseIsolated)
                multiBlobStencil.List.Add(new BlobTileStencil<T>(new MapTerrainStencil<T>(false, false, true, true), true));

            //not allowed to draw the blob over start or end.
            multiBlobStencil.List.Add(new StairsStencil<T>(true));
            //effect tile checks are also needed since even though they are postproc-shielded, it'll cut off the path to those locations
            multiBlobStencil.List.Add(new BlobTileStencil<T>(new TileEffectStencil<T>(true)));

            //not allowed to draw the blob such that chokepoints are removed
            multiBlobStencil.List.Add(new NoChokepointStencil<T>(new MapTerrainStencil<T>(false, false, true, true)));

            //not allowed to draw individual tiles over unbreakable tiles
            BlobWaterStep<T> waterStep = new BlobWaterStep<T>(amount, new Tile(terrain), new MatchTerrainStencil<T>(true, new Tile("unbreakable")), multiBlobStencil, size, new IntRange(Math.Max(size.Min, 7), Math.Max(size.Max * 3 / 2, 8)));
            layout.GenSteps.Add(PR_WATER, waterStep);
            layout.GenSteps.Add(PR_WATER_DIAG, new DropDiagonalBlockStep<T>(new Tile(terrain)));
        }

        public static void AddTerrainPatternSteps<T>(MapGen<T> layout, string terrain, RandRange amount, SpawnList<PatternPlan> planSpawns, ConnectivityRoom.Connectivity connectivity = ConnectivityRoom.Connectivity.Main) where T : ListMapGenContext
        {
            PatternTerrainStep<T> trapStep = new PatternTerrainStep<T>(new Tile(terrain));
            trapStep.Amount = amount;
            trapStep.Maps = planSpawns;
            trapStep.AllowTerminal = true;
            if (connectivity != ConnectivityRoom.Connectivity.None)
                trapStep.Filters.Add(new RoomFilterConnectivity(connectivity));

            MapTerrainStencil<T> terrainStencil = new MapTerrainStencil<T>(true, false, false, false);
            NoChokepointTerrainStencil<T> roomStencil = new NoChokepointTerrainStencil<T>(new MapTerrainStencil<T>(true, false, false, false));
            TileEffectStencil<T> noTile = new TileEffectStencil<T>(true);
            trapStep.TerrainStencil = new MultiTerrainStencil<T>(false, terrainStencil, roomStencil, noTile);

            layout.GenSteps.Add(PR_WATER, trapStep);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layout"></param>
        /// <param name="roomBlobCount"></param>
        /// <param name="roomBlobSize"></param>
        /// <param name="hallPercent">Beware, this only works for one-tile halls</param>
        public static void AddGrassSteps<T>(MapGen<T> layout, RandRange roomBlobCount, IntRange roomBlobSize, RandRange hallPercent) where T : BaseMapGenContext
        {
            string coverTerrain = "grass";
            {
                BlobTilePercentStencil<T> terrainPercentStencil = new BlobTilePercentStencil<T>(50, new MapTerrainStencil<T>(false, false, true, true));
                MapTerrainStencil<T> terrainStencil = new MapTerrainStencil<T>(true, false, false, false);
                TileEffectStencil<T> noTile = new TileEffectStencil<T>(true);
                BlobWaterStep<T> coverStep = new BlobWaterStep<T>(roomBlobCount, new Tile(coverTerrain), new MultiTerrainStencil<T>(false, terrainStencil, noTile), terrainPercentStencil, roomBlobSize, new IntRange(Math.Max(roomBlobSize.Min, 6), Math.Max(roomBlobSize.Max * 3 / 2, 15)));
                layout.GenSteps.Add(PR_WATER, coverStep);
            }
            {
                MapTerrainStencil<T> terrainStencil = new MapTerrainStencil<T>(true, false, false, false);
                NoChokepointTerrainStencil<T> roomStencil = new NoChokepointTerrainStencil<T>(new MapTerrainStencil<T>(true, false, false, false));
                roomStencil.Negate = true;
                TileEffectStencil<T> noTile = new TileEffectStencil<T>(true);
                PerlinWaterStep<T> coverStep = new PerlinWaterStep<T>(hallPercent, 4, new Tile(coverTerrain), new MultiTerrainStencil<T>(false, terrainStencil, roomStencil, noTile), 0, false);
                layout.GenSteps.Add(PR_WATER, coverStep);
            }

        }


        public static void AddSingleTrapStep<T>(MapGen<T> layout, RandRange amount, string id, bool revealed = true, bool includeHalls = false, ConnectivityRoom.Connectivity connectivity = ConnectivityRoom.Connectivity.Main) where T : ListMapGenContext
        {
            SpawnList<EffectTile> effectTileSpawns = new SpawnList<EffectTile>();
            effectTileSpawns.Add(new EffectTile(id, revealed), 10);
            SpacedRoomSpawnStep<T, EffectTile> trapStep = new SpacedRoomSpawnStep<T, EffectTile>(new PickerSpawner<T, EffectTile>(new LoopedRand<EffectTile>(effectTileSpawns, amount)), includeHalls);
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

        public static void AddTrapPatternSteps<T>(MapGen<T> layout, RandRange amount, SpawnList<PatternPlan> planSpawns, ConnectivityRoom.Connectivity connectivity = ConnectivityRoom.Connectivity.Main) where T : ListMapGenContext
        {
            PatternSpawnStep<T, EffectTile> trapStep = new PatternSpawnStep<T, EffectTile>();
            trapStep.Amount = amount;
            trapStep.Maps = planSpawns;
            if (connectivity != ConnectivityRoom.Connectivity.None)
                trapStep.Filters.Add(new RoomFilterConnectivity(connectivity));

            MapTerrainStencil<T> terrainStencil = new MapTerrainStencil<T>(true, false, false, false);
            NoChokepointTerrainStencil<T> roomStencil = new NoChokepointTerrainStencil<T>(new MapTerrainStencil<T>(true, false, false, false));

            trapStep.TerrainStencil = new MultiTerrainStencil<T>(false, terrainStencil, roomStencil);

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

        public static AddBossRoomStep<T> CreateGenericBossRoomStep<T>(IRandPicker<RoomGen<T>> bossRooms, int bossIndex = 0) where T : ListMapGenContext
        {
            SpawnList<RoomGen<T>> treasureRooms = new SpawnList<RoomGen<T>>();
            treasureRooms.Add(new RoomGenCross<T>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
            SpawnList<PermissiveRoomGen<T>> detourHalls = new SpawnList<PermissiveRoomGen<T>>();
            detourHalls.Add(new RoomGenAngledHall<T>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
            AddBossRoomStep<T> detours = new AddBossRoomStep<T>(bossRooms, treasureRooms, detourHalls);
            detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom(), new UnVaultableRoom()));
            detours.BossComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
            detours.BossComponents.Set(new NoEventRoom());
            detours.BossComponents.Set(new BossRoom());
            detours.BossComponents.Set(new IndexRoom(bossIndex));
            detours.BossHallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
            detours.VaultComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.BossLocked));
            detours.VaultComponents.Set(new NoConnectRoom());
            detours.VaultComponents.Set(new NoEventRoom());
            detours.VaultComponents.Set(new IndexRoom(bossIndex));
            detours.VaultHallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.BossLocked));
            detours.VaultHallComponents.Set(new NoConnectRoom());
            detours.VaultHallComponents.Set(new IndexRoom(bossIndex));

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


        static TeamMemberSpawn GetBoostedTeamMob(string species, string ability, string move1, string move2, string move3, string move4, RandRange level, int boost,
            string tactic = "wander_normal", bool sleeping = false, bool unrecruitable = false)
        {
            return GetBoostedTeamMob(new MonsterID(species, 0, "", Gender.Unknown), ability, move1, move2, move3, move4, level, boost, TeamMemberSpawn.MemberRole.Normal, tactic, sleeping, unrecruitable);

        }

        static TeamMemberSpawn GetBoostedTeamMob(string species, string ability, string move1, string move2, string move3, string move4, RandRange level, int boost,
            TeamMemberSpawn.MemberRole role, string tactic = "wander_normal", bool sleeping = false, bool unrecruitable = false)
        {
            return GetBoostedTeamMob(new MonsterID(species, 0, "", Gender.Unknown), ability, move1, move2, move3, move4, level, boost, TeamMemberSpawn.MemberRole.Normal, tactic, sleeping, unrecruitable);
        }

        static TeamMemberSpawn GetBoostedTeamMob(MonsterID id, string ability, string move1, string move2, string move3, string move4, RandRange level, int boost,
            TeamMemberSpawn.MemberRole role, string tactic = "wander_normal", bool sleeping = false, bool unrecruitable = false)
        {
            TeamMemberSpawn teamMob = GetTeamMob(id, ability, move1, move2, move3, move4, level, role, tactic, sleeping, unrecruitable);

            MobSpawnBoost spawnBoost = new MobSpawnBoost();
            spawnBoost.MaxHPBonus = boost;
            spawnBoost.AtkBonus = boost;
            spawnBoost.DefBonus = boost;
            spawnBoost.SpAtkBonus = boost;
            spawnBoost.SpDefBonus = boost;
            spawnBoost.SpeedBonus = boost;
            teamMob.Spawn.SpawnFeatures.Add(spawnBoost);

            return teamMob;
        }

        static TeamMemberSpawn GetTeamMob(string species, string ability, string move1, string move2, string move3, string move4, RandRange level,
            string tactic = "wander_normal", bool sleeping = false, bool unrecruitable = false)
        {
            return GetTeamMob(new MonsterID(species, 0, "", Gender.Unknown), ability, move1, move2, move3, move4, level, tactic, sleeping, unrecruitable);
        }
        static TeamMemberSpawn GetTeamMob(MonsterID id, string ability, string move1, string move2, string move3, string move4, RandRange level,
            string tactic = "wander_normal", bool sleeping = false, bool unrecruitable = false)
        {
            return GetTeamMob(id, ability, move1, move2, move3, move4, level, TeamMemberSpawn.MemberRole.Normal, tactic, sleeping, unrecruitable);
        }
        static TeamMemberSpawn GetTeamMob(string species, string ability, string move1, string move2, string move3, string move4, RandRange level,
            TeamMemberSpawn.MemberRole role, string tactic = "wander_normal", bool sleeping = false, bool unrecruitable = false)
        {
            return GetTeamMob(new MonsterID(species, 0, "", Gender.Unknown), ability, move1, move2, move3, move4, level, role, tactic, sleeping, unrecruitable);
        }
        static TeamMemberSpawn GetTeamMob(MonsterID id, string ability, string move1, string move2, string move3, string move4, RandRange level,
            TeamMemberSpawn.MemberRole role, string tactic = "wander_normal", bool sleeping = false, bool unrecruitable = false)
        {
            return new TeamMemberSpawn(GetGenericMob(id, ability, move1, move2, move3, move4, level, tactic, sleeping, unrecruitable), role);
        }

        static MobSpawn GetGenericMob(string species, string ability, string move1, string move2, string move3, string move4, RandRange level,
            string tactic = "wander_normal", bool sleeping = false, bool unrecruitable = false)
        {
            return GetGenericMob(new MonsterID(species, 0, "", Gender.Unknown), ability, move1, move2, move3, move4, level, tactic, sleeping, unrecruitable);
        }

        static MobSpawn GetGenericMob(MonsterID id, string ability, string move1, string move2, string move3, string move4, RandRange level,
            string tactic = "wander_normal", bool sleeping = false, bool unrecruitable = false)
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
            if (unrecruitable)
                post_mob.SpawnFeatures.Add(new MobSpawnUnrecruitable());
            return post_mob;
        }

        static MobSpawn GetGuardMob(string species, string ability, string move1, string move2, string move3, string move4, RandRange level,
            string tactic = "wander_normal", string statusID = "")
        {
            return GetGuardMob(new MonsterID(species, 0, "", Gender.Unknown), ability, move1, move2, move3, move4, level, tactic, statusID);
        }

        static MobSpawn GetGuardMob(MonsterID id, string ability, string move1, string move2, string move3, string move4, RandRange level,
            string tactic = "wander_normal", string statusID = "")
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
            post_mob.SpawnFeatures.Add(new MobSpawnMovesOff(post_mob.SpecifiedSkills.Count));
            if (!String.IsNullOrEmpty(statusID))
            {
                StatusEffect sleep = new StatusEffect(statusID);
                sleep.StatusStates.Set(new CountDownState(-1));
                MobSpawnStatus status = new MobSpawnStatus();
                status.Statuses.Add(sleep, 10);
                post_mob.SpawnFeatures.Add(status);
            }
            return post_mob;
        }

        static MobSpawn GetFOEMob(string species, string ability, string move1, string move2, string move3, string move4, int baseLv, int scaleNum = 5, int scaleDen = 3)
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
            post_mob.SpawnFeatures.Add(new MobSpawnLevelScale(scaleNum, scaleDen));
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

        static MobSpawn GetBossMob(string species, string ability, string move1, string move2, string move3, string move4, string item, Loc loc, int baseLv = 3, int scaleNum = 4, int scaleDen = 3)
        {
            return GetBossMob(new MonsterID(species, 0, "", Gender.Unknown), ability, move1, move2, move3, move4, item, loc, baseLv, scaleNum, scaleDen);
        }

        static MobSpawn GetBossMob(MonsterID id, string ability, string move1, string move2, string move3, string move4, string item, Loc loc, int baseLv = 3, int scaleNum = 4, int scaleDen = 3)
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
            post_mob.Tactic = "boss";
            post_mob.Level = new RandRange(baseLv);
            post_mob.SpawnFeatures.Add(new MobSpawnLoc(loc));
            post_mob.SpawnFeatures.Add(new MobSpawnItem(true, item));
            post_mob.SpawnFeatures.Add(new MobSpawnUnrecruitable());
            post_mob.SpawnFeatures.Add(new MobSpawnLevelScale(scaleNum, scaleDen));
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

        static string[] ItemArray(params IEnumerable<string>[] iters)
        {
            List<string> result = new List<string>();
            foreach(IEnumerable<string> iter in iters)
                foreach (string item in iter)
                    result.Add(item);
            return result.ToArray();
        }


    }
}
