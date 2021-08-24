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

                structure.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Debug Dungeon\n{0}F")));

                //First floor: Tests traps, secret stairs, FOV, and has dummies to perform moves on.
                #region DEBUG FLOOR 1
                {
                    StairsFloorGen layout = new StairsFloorGen();

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
                                drawStep.Tiles[ii][jj] = new Tile(0);
                            else
                            {
                                if (level[jj - (height - 25)][ii] == '#')
                                    drawStep.Tiles[ii][jj] = new Tile(2);
                                else
                                    drawStep.Tiles[ii][jj] = new Tile(0);
                            }
                        }
                    }

                    layout.GenSteps.Add(PR_TILES_GEN, drawStep);

                    //add border
                    layout.GenSteps.Add(PR_TILES_BARRIER, new UnbreakableBorderStep<StairsMapGenContext>(1));


                    {
                        List<(MapGenEntrance, Loc)> entrances = new List<(MapGenEntrance, Loc)>();
                        entrances.Add((new MapGenEntrance(Dir8.Down), new Loc(28, 2)));
                        AddSpecificSpawn(layout, entrances, PR_EXITS);
                    }
                    {
                        List<(MapGenExit, Loc)> exits = new List<(MapGenExit, Loc)>();
                        exits.Add((new MapGenExit(new EffectTile(1, true)), new Loc(30, 2)));
                        AddSpecificSpawn(layout, exits, PR_EXITS);
                    }
                    {
                        List<(MapGenExit, Loc)> exits = new List<(MapGenExit, Loc)>();
                        EffectTile secretStairs = new EffectTile(35, true);
                        secretStairs.TileStates.Set(new DestState(new SegLoc(-1, 0)));
                        exits.Add((new MapGenExit(secretStairs), new Loc(31, 2)));
                        AddSpecificSpawn(layout, exits, PR_EXITS);
                    }
                    {
                        List<(MapGenExit, Loc)> exits = new List<(MapGenExit, Loc)>();
                        EffectTile secretStairs = new EffectTile(34, true);
                        secretStairs.TileStates.Set(new DestState(new SegLoc(1, 0), true));
                        exits.Add((new MapGenExit(secretStairs), new Loc(32, 2)));
                        AddSpecificSpawn(layout, exits, PR_EXITS);
                    }
                    {
                        List<(MapGenExit, Loc)> exits = new List<(MapGenExit, Loc)>();
                        EffectTile secretStairs = new EffectTile(34, true);
                        secretStairs.TileStates.Set(new DestState(new SegLoc(2, 0), true));
                        exits.Add((new MapGenExit(secretStairs), new Loc(33, 2)));
                        AddSpecificSpawn(layout, exits, PR_EXITS);
                    }
                    {
                        List<(MapGenExit, Loc)> exits = new List<(MapGenExit, Loc)>();
                        EffectTile secretStairs = new EffectTile(34, true);
                        secretStairs.TileStates.Set(new DestState(new SegLoc(3, 0), true));
                        exits.Add((new MapGenExit(secretStairs), new Loc(34, 2)));
                        AddSpecificSpawn(layout, exits, PR_EXITS);
                    }
                    {
                        List<(MapGenExit, Loc)> exits = new List<(MapGenExit, Loc)>();
                        EffectTile secretStairs = new EffectTile(34, true);
                        secretStairs.TileStates.Set(new DestState(new SegLoc(4, 0), true));
                        exits.Add((new MapGenExit(secretStairs), new Loc(35, 2)));
                        AddSpecificSpawn(layout, exits, PR_EXITS);
                    }

                    layout.GenSteps.Add(PR_EXITS, new StairsStep<StairsMapGenContext, MapGenEntrance, MapGenExit>(new MapGenEntrance(Dir8.Down), new MapGenExit(new EffectTile(1, true))));

                    for (int ii = 0; ii < 20; ii++)
                    {
                        MobSpawn post_mob = new MobSpawn();
                        post_mob.BaseForm = new MonsterID(ii + 1, 0, 0, Gender.Unknown);
                        post_mob.Tactic = 6;
                        post_mob.Level = new RandRange(50);
                        post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc((ii % 5) * 2 + 2, ii / 5 * 2 + 2)));
                        post_mob.SpawnFeatures.Add(new MobSpawnItem(true, 1 + ii));
                        SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);

                        PlaceNoLocMobsStep<StairsMapGenContext> mobStep = new PlaceNoLocMobsStep<StairsMapGenContext>(new PresetMultiTeamSpawner<StairsMapGenContext>(post_team));
                        layout.GenSteps.Add(PR_SPAWN_MOBS, mobStep);
                    }

                    {
                        MobSpawn post_mob = new MobSpawn();
                        post_mob.BaseForm = new MonsterID(132, 0, 0, Gender.Unknown);
                        post_mob.Tactic = 6;
                        post_mob.Level = new RandRange(50);
                        post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc(24, 2)));
                        post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_WAIT_0190"))));
                        SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);

                        PlaceNoLocMobsStep<StairsMapGenContext> mobStep = new PlaceNoLocMobsStep<StairsMapGenContext>(new PresetMultiTeamSpawner<StairsMapGenContext>(post_team));
                        mobStep.Ally = true;
                        layout.GenSteps.Add(PR_SPAWN_MOBS, mobStep);
                    }

                    SpawnList<EffectTile> effectTileSpawns = new SpawnList<EffectTile>();
                    for (int ii = 3; ii <= 31; ii++)
                        effectTileSpawns.Add(new EffectTile(ii, true), 10);

                    RandomSpawnStep<StairsMapGenContext, EffectTile> trapStep = new RandomSpawnStep<StairsMapGenContext, EffectTile>(new PickerSpawner<StairsMapGenContext, EffectTile>(new LoopedRand<EffectTile>(effectTileSpawns, new RandRange(300))));
                    layout.GenSteps.Add(PR_SPAWN_TRAPS, trapStep);

                    List<(InvItem, Loc)> items = new List<(InvItem, Loc)>();
                    items.Add((new InvItem(12), new Loc(7, 32)));//Lum Berry
                    items.Add((new InvItem(12), new Loc(8, 33)));//Lum Berry
                    AddSpecificSpawn(layout, items, PR_SPAWN_ITEMS);

                    //Tilesets
                    AddTextureData(layout, 0, 1, 2, 0);

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
                            drawStep.Tiles[ii][jj] = new Tile(2);
                        else if (level[jj][ii] == '~')
                            drawStep.Tiles[ii][jj] = new Tile(3);
                        else
                            drawStep.Tiles[ii][jj] = new Tile(0);
                    }
                }


                layout.GenSteps.Add(PR_TILES_GEN, drawStep);

                //add border
                layout.GenSteps.Add(PR_TILES_BARRIER, new UnbreakableBorderStep<StairsMapGenContext>(1));

                //TODO: secret rooms can't scale NPC levels this way; figure it out if you ever want to scale level
                layout.GenSteps.Add(PR_FLOOR_DATA, new MapNameIDStep<StairsMapGenContext>(0, new LocalText("Secret Room")));
                
                EffectTile secretStairs = new EffectTile(1, true);
                secretStairs.TileStates.Set(new DestState(new SegLoc(-1, 1), true));
                layout.GenSteps.Add(PR_EXITS, new StairsStep<StairsMapGenContext, MapGenEntrance, MapGenExit>(new MapGenEntrance(Dir8.Down), new MapGenExit(secretStairs)));

                //Tilesets
                AddTextureData(layout, 433, 434, 435, 13);

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
                for (int kk = 0; kk < 153; kk++)
                {
                    string[] level = {
                            "...........................................",
                            "..........#.....................~..........",
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
                            "..........#.....................~..........",
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
                                drawStep.Tiles[ii][jj] = new Tile(2);
                            else if (level[jj][ii] == '~')
                                drawStep.Tiles[ii][jj] = new Tile(3);
                            else
                                drawStep.Tiles[ii][jj] = new Tile(0);
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
                        items.Add((new MapGenExit(new EffectTile(1, true)), new Loc(22, 12)));
                        AddSpecificSpawn(layout, items, PR_EXITS);
                    }

                    MapTextureStep<StairsMapGenContext> textureStep = new MapTextureStep<StairsMapGenContext>();
                    textureStep.BlockTileset = curTileIndex;
                    curTileIndex++;
                    textureStep.GroundTileset = curTileIndex;
                    curTileIndex++;
                    //chasm cave
                    if (curTileIndex != 260 && curTileIndex != 262)
                    {
                        textureStep.WaterTileset = curTileIndex;
                        curTileIndex++;
                    }
                    layout.GenSteps.Add(PR_TEXTURES, textureStep);

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
                            drawStep.Tiles[ii][jj] = new Tile(0);
                    }

                    //monster house
                    {
                        EffectTile effect = new EffectTile(37, true, new Loc(4, 4));
                        effect.Danger = true;
                        effect.TileStates.Set(new BoundsState(new Rect(0, 0, 10, 10)));
                        ItemSpawnState itemSpawn = new ItemSpawnState();
                        for (int ii = 0; ii < 10; ii++)
                            itemSpawn.Spawns.Add(new MapItem(100 + ii));
                        effect.TileStates.Set(itemSpawn);
                        MobSpawnState mobSpawn = new MobSpawnState();
                        for (int ii = 0; ii < 16; ii++)
                            mobSpawn.Spawns.Add(GetGenericMob(260 + ii, -1, -1, -1, -1, -1, new RandRange(10), 7));
                        effect.TileStates.Set(mobSpawn);
                        ((Tile)drawStep.Tiles[4][4]).Effect = effect;

                        drawStep.Tiles[1][7] = new Tile(2);
                        drawStep.Tiles[1][8] = new Tile(2);
                        drawStep.Tiles[1][9] = new Tile(2);
                        drawStep.Tiles[2][7] = new Tile(2);
                        drawStep.Tiles[2][8] = new Tile(2);
                        drawStep.Tiles[2][9] = new Tile(2);
                    }
                    {
                        EffectTile effect = new EffectTile(37, true, new Loc(5, 5));
                        effect.Danger = true;
                        effect.TileStates.Set(new BoundsState(new Rect(3, 3, 13, 13)));
                        ItemSpawnState itemSpawn = new ItemSpawnState();
                        for (int ii = 0; ii < 10; ii++)
                            itemSpawn.Spawns.Add(new MapItem(100 + ii));
                        effect.TileStates.Set(itemSpawn);
                        MobSpawnState mobSpawn = new MobSpawnState();
                        for (int ii = 0; ii < 16; ii++)
                            mobSpawn.Spawns.Add(GetGenericMob(260 + ii, -1, -1, -1, -1, -1, new RandRange(10), 7));
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
                                EffectTile effect = new EffectTile(45, true, new Loc(20+xx, yy));
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
                            state.Security.Add(GetShopMob(352, 168, 264, 425, 510, 168, new int[] { 1984, 1985, 1988 }, -1), 10);
                            StateMapStatusStep<StairsMapGenContext> statusData = new StateMapStatusStep<StairsMapGenContext>(38, state);
                            layout.GenSteps.Add(PR_FLOOR_DATA, statusData);
                        }

                        // place the mob running the shop
                        {
                            MobSpawn post_mob = GetShopMob(352, 16, 485, 20, 103, 86, new int[] { 1984, 1985, 1988 }, 0);
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
                                    drawStep.Tiles[xx][yy] = new Tile(1);
                                }
                            }
                        }

                        EffectTile effect2 = new EffectTile(40, true, lockedTile);
                        ((Tile)drawStep.Tiles[lockedTile.X][lockedTile.Y]).Effect = effect2;

                        //specifically planned enemies
                        MobSpawnState mobSpawnState = new MobSpawnState();
                        {
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID(382, 0, 0, Gender.Unknown);
                            post_mob.Tactic = 6;
                            post_mob.Level = new RandRange(50);
                            post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc(36, 6) + new Loc(1)));
                            post_mob.SpawnFeatures.Add(new MobSpawnItem(true, 1));
                            mobSpawnState.Spawns.Add(post_mob);
                        }
                        {
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID(383, 0, 0, Gender.Unknown);
                            post_mob.Tactic = 6;
                            post_mob.Level = new RandRange(50);
                            post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc(38, 6) + new Loc(1)));
                            post_mob.SpawnFeatures.Add(new MobSpawnItem(true, 1));
                            mobSpawnState.Spawns.Add(post_mob);
                        }
                        {
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID(384, 0, 0, Gender.Unknown);
                            post_mob.Tactic = 6;
                            post_mob.Level = new RandRange(50);
                            post_mob.SpawnFeatures.Add(new MobSpawnLoc(new Loc(40, 6) + new Loc(1)));
                            post_mob.SpawnFeatures.Add(new MobSpawnItem(true, 1));
                            mobSpawnState.Spawns.Add(post_mob);
                        }

                        EffectTile newEffect = new EffectTile(38, true, bossTriggerArea + new Loc(1));
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
                        EffectTile effect2 = new EffectTile(39, true, new Loc(40 + 1, 2 + 1));
                        TileListState state = new TileListState();
                        for (int mm = 1; mm < howMany; mm++)
                            state.Tiles.Add(new Loc(40 + patternDiff * mm + 1, 2 + 1));
                        effect2.TileStates.Set(state);
                        ((Tile)drawStep.Tiles[40][2]).Effect = effect2;
                    }
                    for (int nn = 1; nn < howMany; nn++)
                    {
                        EffectTile effect2 = new EffectTile(40, true, new Loc(40 + patternDiff * nn + 1, 2 + 1));
                        ((Tile)drawStep.Tiles[40 + patternDiff * nn][2]).Effect = effect2;
                    }

                    //tile-activated gates
                    patternDiff = 2;
                    howMany = 3;
                    {
                        EffectTile effect2 = new EffectTile(41, true, new Loc(50, 4));
                        TileListState state = new TileListState();
                        for (int mm = 1; mm < howMany; mm++)
                            state.Tiles.Add(new Loc(50 + patternDiff * mm + 1, 4 + 1));
                        effect2.TileStates.Set(state);
                        ((Tile)drawStep.Tiles[50][4]).Effect = effect2;
                    }
                    for (int nn = 1; nn < howMany; nn++)
                    {
                        EffectTile effect2 = new EffectTile(40, true, new Loc(50 + patternDiff * nn, 4));
                        ((Tile)drawStep.Tiles[50 + patternDiff * nn][4]).Effect = effect2;
                    }


                    layout.GenSteps.Add(PR_TILES_GEN, drawStep);

                    //add border
                    layout.GenSteps.Add(PR_TILES_BARRIER, new UnbreakableBorderStep<StairsMapGenContext>(1));

                    layout.GenSteps.Add(PR_EXITS, new StairsStep<StairsMapGenContext, MapGenEntrance, MapGenExit>(new MapGenEntrance(Dir8.Down), new MapGenExit(new EffectTile(1, true))));

                    MapTextureStep<StairsMapGenContext> textureStep = new MapTextureStep<StairsMapGenContext>();
                    textureStep.BlockTileset = curTileIndex;
                    curTileIndex++;
                    textureStep.GroundTileset = curTileIndex;
                    curTileIndex++;
                    textureStep.WaterTileset = curTileIndex;
                    curTileIndex++;
                    layout.GenSteps.Add(PR_TEXTURES, textureStep);

                    structure.Floors.Add(layout);
                }
                #endregion

                //structure.MainExit = new ZoneLoc(2, 0);
                zone.Segments.Add(structure);
            }

            #region REPLAY TEST ZONE
            {
                LayeredSegment floorSegment = new LayeredSegment();
                floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Replay Test Zone\n{0}F")));
                int total_floors = 10;

                SpawnList<IGenPriority> shopZoneSpawns = new SpawnList<IGenPriority>();
                //kecleon shop
                {
                    ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                    shop.Personality = 0;
                    shop.SecurityStatus = 38;
                    shop.Items.Add(new MapItem(101, 0, 800), 10);//reviver seed
                    shop.Items.Add(new MapItem(112, 0, 500), 10);//blast seed
                    shop.Items.Add(new MapItem(444, 753, 8000), 10);//poison dust
                    shop.Items.Add(new MapItem(234, 9, 180), 10);//Lob Wand
                    shop.Items.Add(new MapItem(352, 0, 2000), 10);//thunder stone
                    shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                    // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 103 screech : 86 thunder wave
                    shop.StartMob = GetShopMob(352, 16, 485, 20, 103, 86, new int[] { 1984, 1985, 1988 }, 0);
                    {
                        // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 103 screech : 86 thunder wave
                        shop.Mobs.Add(GetShopMob(352, 16, 485, 20, 103, 86, new int[] { 1984, 1985, 1988 }, -1), 10);
                        // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 50 disable : 374 fling
                        shop.Mobs.Add(GetShopMob(352, 16, 485, 20, 50, 374, new int[] { 1984, 1985, 1988 }, -1), 10);
                        // 352 Kecleon : 168 protean : 246 ancient power : 425 shadow sneak : 510 incinerate : 168 thief
                        shop.Mobs.Add(GetShopMob(352, 168, 246, 425, 510, 168, new int[] { 1984, 1985, 1988 }, -1), 10);
                        // 352 Kecleon : 168 protean : 332 aerial ace : 421 shadow claw : 60 psybeam : 364 feint
                        shop.Mobs.Add(GetShopMob(352, 168, 332, 421, 60, 364, new int[] { 1984, 1985, 1988 }, -1), 10);
                    }
                    shopZoneSpawns.Add(new GenPriority<GenStep<MapGenContext>>(PR_SHOPS, shop), 10);
                }

                //star treasures
                {
                    ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                    shop.Personality = 1;
                    shop.SecurityStatus = 38;
                    shop.Items.Add(new MapItem(444, 753, 8000), 10);//poison dust
                    shop.Items.Add(new MapItem(352, 0, 2000), 10);//thunder stone
                    shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                    // Cleffa : 98 Magic Guard : 118 Metronome : 47 Sing : 204 Charm : 313 Fake Tears
                    {
                        MobSpawn post_mob = new MobSpawn();
                        post_mob.BaseForm = new MonsterID(173, 0, 0, Gender.Unknown);
                        post_mob.Tactic = 23;
                        post_mob.Level = new RandRange(5);
                        post_mob.Intrinsic = 98;
                        post_mob.SpecifiedSkills.Add(118);
                        post_mob.SpecifiedSkills.Add(47);
                        post_mob.SpecifiedSkills.Add(204);
                        post_mob.SpecifiedSkills.Add(313);
                        post_mob.SpawnFeatures.Add(new MobSpawnDiscriminator(1));
                        post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new BattleScriptEvent("ShopkeeperInteract")));
                        post_mob.SpawnFeatures.Add(new MobSpawnLuaTable("{ Role = \"Shopkeeper\" }"));
                        shop.StartMob = post_mob;
                    }
                    {
                        // 35 Clefairy : 132 Friend Guard : 282 Knock Off : 107 Minimize : 236 Moonlight : 277 Magic Coat
                        shop.Mobs.Add(GetShopMob(35, 132, 282, 107, 236, 277, new int[] { 973, 976 }, -1), 10);
                        // 36 Clefable : 109 Unaware : 118 Metronome : 500 Stored Power : 343 Covet : 271 Trick
                        shop.Mobs.Add(GetShopMob(36, 109, 118, 500, 343, 271, new int[] { 973, 976 }, -1), 10);
                        // 36 Clefable : 98 Magic Guard : 118 Metronome : 213 Attract : 282 Knock Off : 266 Follow Me
                        shop.Mobs.Add(GetShopMob(36, 98, 118, 213, 282, 266, new int[] { 973, 976 }, -1), 10);
                    }
                    shopZoneSpawns.Add(new GenPriority<GenStep<MapGenContext>>(PR_SHOPS, shop), 10);
                }

                //porygon wares
                {
                    ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                    shop.Personality = 2;
                    shop.SecurityStatus = 38;
                    shop.Items.Add(new MapItem(444, 753, 8000), 10);//poison dust
                    shop.Items.Add(new MapItem(352, 0, 2000), 10);//thunder stone
                    shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                    // 137 Porygon : 36 Trace : 97 Agility : 59 Blizzard : 435 Discharge : 94 Psychic
                    shop.StartMob = GetShopMob(137, 36, 97, 59, 435, 94, new int[] { 1322, 1323, 1324, 1325 }, 2);
                    {
                        // 474 Porygon-Z : 91 Adaptability : 417 Nasty Plot : 63 Hyper Beam : 435 Discharge : 373 Embargo
                        shop.Mobs.Add(GetShopMob(474, 91, 417, 63, 435, 373, new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                        // 474 Porygon-Z : 91 Adaptability : 160 Conversion : 59 Blizzard : 435 Discharge : 473 Psyshock
                        shop.Mobs.Add(GetShopMob(474, 91, 160, 59, 435, 473, new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                        // 474 Porygon-Z : 91 Adaptability : 417 Nasty Plot : 63 Hyper Beam : 435 Discharge : 373 Embargo
                        shop.Mobs.Add(GetShopMob(474, 91, 417, 63, 435, 373, new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                        // 474 Porygon-Z : 91 Adaptability : 417 Nasty Plot : 161 Tri Attack : 247 Shadow Ball : 373 Embargo
                        shop.Mobs.Add(GetShopMob(474, 91, 417, 161, 247, 373, new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                        // 474 Porygon-Z : 88 Download : 97 Agility : 473 Psyshock : 324 Signal Beam : 373 Embargo
                        shop.Mobs.Add(GetShopMob(474, 88, 97, 473, 324, 373, new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                        // 233 Porygon2 : 36 Trace : 176 Conversion2 : 105 Recover : 60 Psybeam : 324 Signal Beam
                        shop.Mobs.Add(GetShopMob(233, 36, 176, 105, 60, 324, new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                        // 233 Porygon2 : 36 Trace : 176 Conversion2 : 105 Recover : 60 Psybeam : 435 Discharge
                        shop.Mobs.Add(GetShopMob(233, 36, 176, 105, 60, 435, new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                        // 233 Porygon2 : 36 Trace : 176 Conversion2 : 277 Magic Coat : 161 Tri Attack : 97 Agility
                        shop.Mobs.Add(GetShopMob(233, 36, 176, 277, 161, 97, new int[] { 1322, 1323, 1324, 1325 }, -1), 10);
                    }
                    shopZoneSpawns.Add(new GenPriority<GenStep<MapGenContext>>(PR_SHOPS, shop), 10);
                }

                //bottle shop
                {
                    ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                    shop.Personality = 0;
                    shop.SecurityStatus = 38;
                    shop.Items.Add(new MapItem(444, 753, 8000), 10);//poison dust
                    shop.Items.Add(new MapItem(352, 0, 2000), 10);//thunder stone
                    shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                    // 213 Shuckle : 126 Contrary : 380 Gastro Acid : 564 Sticky Web : 450 Bug Bite : 92 Toxic
                    shop.StartMob = GetShopMob(213, 126, 380, 564, 450, 92, new int[] { 1569, 1570, 1571, 1572 }, 0);
                    {
                        // 213 Shuckle : 126 Contrary : 380 Gastro Acid : 564 Sticky Web : 450 Bug Bite : 92 Toxic
                        shop.Mobs.Add(GetShopMob(213, 126, 380, 564, 450, 92, new int[] { 1569, 1570, 1571, 1572 }, -1), 10);
                        // 213 Shuckle : 126 Contrary : 230 Sweet Scent : 611 Infestation : 189 Mud-Slap : 522 Struggle Bug
                        shop.Mobs.Add(GetShopMob(213, 126, 230, 611, 189, 522, new int[] { 1569, 1570, 1571, 1572 }, -1), 10);
                        // 213 Shuckle : 126 Contrary : 230 Sweet Scent : 219 Safeguard : 446 Stealth Rock : 249 Rock Smash
                        shop.Mobs.Add(GetShopMob(213, 5, 230, 219, 446, 249, new int[] { 1569, 1570, 1571, 1572 }, -1), 10);
                        // 213 Shuckle : 5 Sturdy : 379 Power Trick : 504 Shell Smash : 205 Rollout : 360 Gyro Ball
                        shop.Mobs.Add(GetShopMob(213, 5, 379, 504, 205, 360, new int[] { 1569, 1570, 1571, 1572 }, -1), 10);
                        // 213 Shuckle : 5 Sturdy : 379 Power Trick : 450 Bug Bite : 444 Stone Edge : 91 Dig
                        shop.Mobs.Add(GetShopMob(213, 5, 379, 450, 444, 91, new int[] { 1569, 1570, 1571, 1572 }, -1), 10);
                    }

                    shopZoneSpawns.Add(new GenPriority<GenStep<MapGenContext>>(PR_SHOPS, shop), 10);
                }

                //dunsparce finds
                {
                    ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                    shop.Personality = 0;
                    shop.SecurityStatus = 38;
                    shop.Items.Add(new MapItem(444, 753, 8000), 10);//poison dust
                    shop.Items.Add(new MapItem(352, 0, 2000), 10);//thunder stone
                    shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                    // 206 Dunsparce : 32 Serene Grace : 228 Pursuit : 103 Screech : 44 Bite : Secret Power
                    shop.StartMob = GetShopMob(206, 32, 228, 103, 44, 290, new int[] { 1545, 1546, 1549 }, 0);
                    {
                        // 206 Dunsparce : 32 Serene Grace : 228 Pursuit : 99 Rage : 428 Zen Headbutt : Secret Power
                        shop.Mobs.Add(GetShopMob(206, 32, 228, 99, 428, 290, new int[] { 1545, 1546, 1549 }, -1), 10);
                        // 206 Dunsparce : 32 Serene Grace : 58 Ice Beam : 352 Water Pulse : Secret Power : Ancient Power
                        shop.Mobs.Add(GetShopMob(206, 32, 58, 352, 290, 246, new int[] { 1545, 1546, 1549 }, -1), 10);
                        // 206 Dunsparce : 32 Serene Grace : 228 Pursuit : 44 Bite : Secret Power : Ancient Power
                        shop.Mobs.Add(GetShopMob(206, 32, 228, 44, 290, 246, new int[] { 1545, 1546, 1549 }, -1), 10);
                        // 206 Dunsparce : 32 Serene Grace : 228 Pursuit : 355 Roost : Secret Power : Ancient Power
                        shop.Mobs.Add(GetShopMob(206, 32, 228, 355, 290, 246, new int[] { 1545, 1546, 1549 }, -1), 10);
                        // 206 Dunsparce : 155 Rattled : 228 Pursuit : 180 Spite : 20 Bind : 281 Yawn
                        shop.Mobs.Add(GetShopMob(206, 155, 228, 180, 20, 281, new int[] { 1545, 1546, 1549 }, -1), 10);
                        // 206 Dunsparce : 155 Rattled : 137 Glare : 180 Spite : 20 Bind : 506 Hex
                        shop.Mobs.Add(GetShopMob(206, 155, 137, 180, 20, 506, new int[] { 1545, 1546, 1549 }, -1), 10);
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
                for (int nn = 1; nn < 4/*700*/; nn++)
                    category.Spawns.Add(new InvItem(nn), new IntRange(0, 5), (nn % 5 + 1) * 10);//all items
                floorSegment.ZoneSteps.Add(zoneItemStep);

                //mobs
                TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                poolSpawn.Priority = PR_RESPAWN_MOB;
                for (int xx = 152; xx < 161/*493*/; xx++)
                {
                    int yy = 1;
                    MobSpawn post_mob = new MobSpawn();
                    post_mob.BaseForm = new MonsterID(xx, 0, -1, Gender.Unknown);
                    post_mob.Intrinsic = -1;
                    post_mob.Level = new RandRange(10);
                    post_mob.Tactic = 7;
                    post_mob.SpawnFeatures.Add(new MobSpawnWeak());
                    if (yy == 0)
                    {
                        StatusEffect sleep = new StatusEffect(1);
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
                    post_mob.BaseForm = new MonsterID(200, 0, 0, Gender.Unknown);
                    post_mob.Tactic = 21;
                    post_mob.Level = new RandRange(50);
                    post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_WAIT_0190"))));
                    SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);
                    multiTeamSpawner.Spawns.Add(post_team);
                    PlaceRandomMobsStep<ListMapGenContext> randomSpawn = new PlaceRandomMobsStep<ListMapGenContext>(multiTeamSpawner);
                    randomSpawn.Ally = true;
                    npcZoneSpawns.ToSpawn.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_SPAWN_MOBS_EXTRA, randomSpawn));
                }
                //Scripted Dialogue
                {
                    PresetMultiTeamSpawner<ListMapGenContext> multiTeamSpawner = new PresetMultiTeamSpawner<ListMapGenContext>();
                    MobSpawn post_mob = new MobSpawn();
                    post_mob.BaseForm = new MonsterID(225, 0, 0, Gender.Male);
                    post_mob.Tactic = 21;
                    post_mob.Level = new RandRange(50);
                    post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new BattleScriptEvent("CountTalkTest")));
                    SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);
                    multiTeamSpawner.Spawns.Add(post_team);
                    PlaceRandomMobsStep<ListMapGenContext> randomSpawn = new PlaceRandomMobsStep<ListMapGenContext>(multiTeamSpawner);
                    randomSpawn.Ally = true;
                    npcZoneSpawns.ToSpawn.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_SPAWN_MOBS_EXTRA, randomSpawn));
                }
                //Ally Team
                {
                    PresetMultiTeamSpawner<ListMapGenContext> multiTeamSpawner = new PresetMultiTeamSpawner<ListMapGenContext>();
                    SpecificTeamSpawner post_team = new SpecificTeamSpawner();
                    {
                        MobSpawn post_mob = new MobSpawn();
                        post_mob.BaseForm = new MonsterID(483, 0, 0, Gender.Unknown);
                        post_mob.Tactic = 0;
                        post_mob.Level = new RandRange(10);
                        post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_FULL_0773"))));
                        post_mob.SpawnFeatures.Add(new MobSpawnFoeConflict());
                        post_team.Spawns.Add(post_mob);
                    }
                    {
                        MobSpawn post_mob = new MobSpawn();
                        post_mob.BaseForm = new MonsterID(484, 0, 0, Gender.Unknown);
                        post_mob.Tactic = 0;
                        post_mob.Level = new RandRange(10);
                        post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_FULL_0781"))));
                        post_team.Spawns.Add(post_mob);
                    }
                    {
                        MobSpawn post_mob = new MobSpawn();
                        post_mob.BaseForm = new MonsterID(487, 0, 0, Gender.Unknown);
                        post_mob.Tactic = 0;
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
                for (int jj = 2; jj <= 31; jj++)
                    tileSpawn.Spawns.Add(new EffectTile(jj, true), new IntRange(0, 5), 10);
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

                for (int ii = 0; ii < total_floors; ii++)
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
                    genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile(2), new RandRange(6, 9), new RandRange(6, 9), new RandRange(2, 3), new RandRange(2, 3)), 10);
                    //water ring
                    if (ii < 5)
                    {
                        RoomGenWaterRing<MapGenContext> waterRing = new RoomGenWaterRing<MapGenContext>(new Tile(3), new RandRange(1, 4), new RandRange(1, 4), 3);
                        waterRing.Treasures.Add(new MapItem(2), 10);
                        waterRing.Treasures.Add(new MapItem(3), 10);
                        waterRing.Treasures.Add(new MapItem(4), 10);
                        genericRooms.Add(waterRing, 10);
                    }
                    //Guarded Cave
                    if (ii < 5)
                    {
                        RoomGenGuardedCave<MapGenContext> guarded = new RoomGenGuardedCave<MapGenContext>();
                        //treasure
                        guarded.Treasures.RandomSpawns.Add(new MapItem(101), 10);
                        guarded.Treasures.RandomSpawns.Add(new MapItem(102), 10);
                        guarded.Treasures.RandomSpawns.Add(new MapItem(103), 10);
                        guarded.Treasures.SpawnAmount = 6;
                        //guard
                        MobSpawn spawner = new MobSpawn();
                        spawner.BaseForm = new MonsterID(149, 0, -1, Gender.Unknown);
                        spawner.Level = new RandRange(80);
                        //status sleep
                        StatusEffect sleep = new StatusEffect(1);
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

                    AddWaterSteps(layout, 5, new RandRange(ii * 25), false);//abyss

                    //Tilesets
                    AddTextureData(layout, 0, 1, 2, 09);

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 4), 27, false);//wonder tile
                    AddTrapsSteps(layout, new RandRange(20));

                    //money
                    AddMoneyData(layout, new RandRange(2, 4));

                    //items
                    AddItemData(layout, new RandRange(4, 6), 25);

                    //mobs
                    AddRespawnData(layout, 12, 50);

                    AddEnemySpawnData(layout, 100, new RandRange(5, 8));

                    floorSegment.Floors.Add(layout);
                }
                //floorSegment.MainExit = new ZoneLoc(-1, 0);
                zone.Segments.Add(floorSegment);
            }
            #endregion

        }
    }
}
