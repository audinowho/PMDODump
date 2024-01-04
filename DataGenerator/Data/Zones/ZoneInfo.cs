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
        public const int MAX_ZONES = 55;

        public static void AddZoneData(bool translate, params int[] zonesToAdd)
        {
            if (translate)
                InitStringsAll();

            if (zonesToAdd.Length > 0)
            {
                for (int ii = 0; ii < zonesToAdd.Length; ii++)
                {
                    ZoneData zone = GetZoneData(zonesToAdd[ii], translate);
                    if (zone.Name.DefaultText != "")
                        DataManager.SaveData(Text.Sanitize(zone.Name.DefaultText).ToLower(), DataManager.DataType.Zone.ToString(), zone);
                }
            }
            else
            {
                DataInfo.DeleteIndexedData(DataManager.DataType.Zone.ToString());
                for (int ii = 0; ii < MAX_ZONES; ii++)
                {
                    ZoneData zone = GetZoneData(ii, translate);
                    if (zone.Name.DefaultText != "")
                        DataManager.SaveData(Text.Sanitize(zone.Name.DefaultText).ToLower(), DataManager.DataType.Zone.ToString(), zone);
                }
            }
        }


        public static ZoneData GetZoneData(int index, bool translate)
        {
            ZoneData zone = new ZoneData();
            if (index == 0)
                FillDebugZone(zone);
            else if (index == 1)
                FillHubZone(zone);
            else if (index == 2)
            {
                #region TROPICAL PATH
                {
                    zone.Name = new LocalText("Tropical Path");
                    zone.Rescues = 2;
                    zone.Level = 5;
                    zone.Rogue = RogueStatus.NoTransfer;

                    int max_floors = 4;

                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Tropical Path\n{0}F"), new Priority(-15)));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 0);
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                    //items!
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;

                    CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                    necessities.SpawnRates.SetRange(14, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("necessities", necessities);

                    necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 9);//Leppa
                    necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 12);//Oran
                    necessities.Spawns.Add(new InvItem("food_apple"), new IntRange(0, max_floors), 10);//Apple
                    necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 10);//Lum
                    necessities.Spawns.Add(new InvItem("seed_reviver"), new IntRange(0, max_floors), 5);//reviver seed
                    necessities.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, max_floors), 9);//blast seed


                    CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                    special.SpawnRates.SetRange(7, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("special", special);

                    int rate = 2;
                    special.Spawns.Add(new InvItem("apricorn_blue"), new IntRange(1, max_floors), rate);//blue apricorns
                    special.Spawns.Add(new InvItem("apricorn_green"), new IntRange(1, max_floors), rate);//green apricorns
                    special.Spawns.Add(new InvItem("apricorn_white"), new IntRange(1, max_floors), rate);//white apricorns

                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;
                    //161 Sentret : 10 Scratch
                    poolSpawn.Spawns.Add(GetTeamMob("sentret", "", "scratch", "", "", "", new RandRange(2), "wander_dumb"), new IntRange(0, 2), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("sentret", "", "scratch", "", "", "", new RandRange(5), "wander_dumb"), new IntRange(2, max_floors), 10);
                    //191 Sunkern : 71 Absorb
                    poolSpawn.Spawns.Add(GetTeamMob("sunkern", "", "absorb", "", "", "", new RandRange(3), "wander_dumb"), new IntRange(0, 2), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("sunkern", "", "absorb", "", "", "", new RandRange(5), "wander_dumb"), new IntRange(1, max_floors), 10);
                    //396 Starly : 33 Tackle
                    poolSpawn.Spawns.Add(GetTeamMob("starly", "", "tackle", "", "", "", new RandRange(2), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("starly", "", "tackle", "", "", "", new RandRange(4), "wander_dumb"), new IntRange(2, max_floors), 10);
                    //10 Caterpie : 33 Tackle : 81 String Shot
                    poolSpawn.Spawns.Add(GetTeamMob("caterpie", "", "tackle", "string_shot", "", "", new RandRange(4), "wander_dumb"), new IntRange(2, max_floors), 10);
                    //120 Staryu : 55 Water Gun
                    poolSpawn.Spawns.Add(GetTeamMob("staryu", "natural_cure", "water_gun", "", "", "", new RandRange(4), "wander_dumb"), new IntRange(2, max_floors), 10);
                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);


                    RandBag<IGenStep> npcZoneSpawns = new RandBag<IGenStep>(true, new List<IGenStep>());
                    //Neutral NPCs
                    {
                        PresetMultiTeamSpawner<ListMapGenContext> multiTeamSpawner = new PresetMultiTeamSpawner<ListMapGenContext>();
                        MobSpawn post_mob = new MobSpawn();
                        post_mob.BaseForm = new MonsterID("chansey", 0, "normal", Gender.Female);
                        post_mob.Tactic = "slow_patrol";
                        post_mob.Level = new RandRange(12);
                        post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_ADVICE_NEUTRAL"))));
                        SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);
                        post_team.Explorer = true;
                        multiTeamSpawner.Spawns.Add(post_team);
                        PlaceRandomMobsStep<ListMapGenContext> randomSpawn = new PlaceRandomMobsStep<ListMapGenContext>(multiTeamSpawner);
                        randomSpawn.Ally = true;
                        npcZoneSpawns.ToSpawn.Add(randomSpawn);
                    }
                    //EXP On move use only
                    {
                        PresetMultiTeamSpawner<ListMapGenContext> multiTeamSpawner = new PresetMultiTeamSpawner<ListMapGenContext>();
                        MobSpawn post_mob = new MobSpawn();
                        post_mob.BaseForm = new MonsterID("sandshrew", 0, "normal", Gender.Male);
                        post_mob.Tactic = "slow_wander";
                        post_mob.Level = new RandRange(14);
                        post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_ADVICE_EXP"))));
                        SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);
                        post_team.Explorer = true;
                        multiTeamSpawner.Spawns.Add(post_team);
                        PlaceRandomMobsStep<ListMapGenContext> randomSpawn = new PlaceRandomMobsStep<ListMapGenContext>(multiTeamSpawner);
                        randomSpawn.Ally = true;
                        npcZoneSpawns.ToSpawn.Add(randomSpawn);
                    }
                    SpreadStepZoneStep npcZoneStep = new SpreadStepZoneStep(new SpreadPlanQuota(new RandRange(2), new IntRange(0, max_floors), true), PR_SPAWN_MOBS_EXTRA, npcZoneSpawns);
                    floorSegment.ZoneSteps.Add(npcZoneStep);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    floorSegment.ZoneSteps.Add(tileSpawn);


                    AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 2);

                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        AddFloorData(layout, "B04. Tropical Path.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Clear);

                        AddWaterSteps(layout, "water", new RandRange(30));//water

                        //Tilesets
                        AddTextureData(layout, "howling_forest_1_wall", "howling_forest_1_floor", "howling_forest_1_secondary", "normal");

                        //money
                        AddMoneyData(layout, new RandRange(1, 3));

                        //items
                        AddItemData(layout, new RandRange(2, 4), 25);

                        //enemies! ~ up to lv5
                        AddRespawnData(layout, 3, 80);

                        //enemies
                        AddEnemySpawnData(layout, 20, new RandRange(2, 4));

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
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

                        if (ii == 3)
                        {
                            EffectTile secretTile = new EffectTile("stairs_secret_down", false);
                            secretTile.TileStates.Set(new DestState(new SegLoc(1, 0)));
                            RandomSpawnStep<MapGenContext, EffectTile> trapStep = new RandomSpawnStep<MapGenContext, EffectTile>(new PickerSpawner<MapGenContext, EffectTile>(new PresetMultiRand<EffectTile>(secretTile)));
                            layout.GenSteps.Add(PR_SPAWN_TRAPS, trapStep);
                        }


                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        floorSegment.Floors.Add(layout);
                    }
                    zone.Segments.Add(floorSegment);
                }

                {
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Secret Room"), new Priority(-15)));
                    {
                        LoadGen layout = new LoadGen();
                        MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                        startGen.MapID = "secret_tropical_path";
                        layout.GenSteps.Add(PR_FILE_LOAD, startGen);

                        MapTimeLimitStep<MapLoadContext> floorData = new MapTimeLimitStep<MapLoadContext>(600);
                        layout.GenSteps.Add(PR_FLOOR_DATA, floorData);

                        //add a chest
                        List<InvItem> treasure = new List<InvItem>();
                        treasure.Add(InvItem.CreateBox("box_dainty", "seed_reviver"));//Reviver Seed
                        treasure.Add(InvItem.CreateBox("box_dainty", "seed_reviver"));//Reviver Seed
                        treasure.Add(InvItem.CreateBox("box_dainty", "berry_micle"));//Micle Berry
                        treasure.Add(InvItem.CreateBox("box_dainty", "berry_jaboca"));//Rowap Berry
                        treasure.Add(InvItem.CreateBox("box_dainty", "berry_rowap"));//Jaboca Berry
                        treasure.Add(InvItem.CreateBox("box_dainty", "food_apple_big"));//Big Apple
                        List<(List<InvItem>, Loc)> items = new List<(List<InvItem>, Loc)>();
                        items.Add((treasure, new Loc(4, 5)));
                        AddSpecificSpawnPool(layout, items, PR_SPAWN_ITEMS);

                        floorSegment.Floors.Add(layout);
                    }
                    zone.Segments.Add(floorSegment);
                }


                {
                    SingularSegment structure = new SingularSegment(-1);

                    ChanceFloorGen multiGen = new ChanceFloorGen();
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "small_square", -3, false, false), 10);
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "tall_hall", -3, false, false), 10);
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "wide_hall", -3, false, false), 10);
                    structure.BaseFloor = multiGen;

                    zone.Segments.Add(structure);
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
                    zone.Rogue = RogueStatus.NoTransfer;

                    {
                        int max_floors = 7;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Faded Trail\n{0}F"), new Priority(-15)));

                        //money
                        MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 0);
                        moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                        floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                        //items
                        ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                        itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;

                        //necessities
                        CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                        necessities.SpawnRates.SetRange(14, new IntRange(0, 7));
                        itemSpawnZoneStep.Spawns.Add("necessities", necessities);

                        necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, 7), 9);//Leppa
                        necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, 7), 12);//Oran
                        necessities.Spawns.Add(new InvItem("food_apple"), new IntRange(0, 7), 10);//Apple
                        necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, 7), 10);//Lum
                        necessities.Spawns.Add(new InvItem("seed_reviver"), new IntRange(0, 7), 5);//reviver seed

                        //snacks
                        CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                        snacks.SpawnRates.SetRange(10, new IntRange(0, 7));
                        itemSpawnZoneStep.Spawns.Add("snacks", snacks);

                        snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, 7), 20);//blast seed
                        snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, 7), 10);//warp seed
                        snacks.Spawns.Add(new InvItem("seed_sleep"), new IntRange(0, 7), 10);//sleep seed

                        //wands
                        CategorySpawn<InvItem> ammo = new CategorySpawn<InvItem>();
                        ammo.SpawnRates.SetRange(10, new IntRange(0, 7));
                        itemSpawnZoneStep.Spawns.Add("ammo", ammo);

                        ammo.Spawns.Add(new InvItem("ammo_stick", false, 3), new IntRange(0, 7), 10);//stick
                        ammo.Spawns.Add(new InvItem("wand_whirlwind", false, 2), new IntRange(0, 7), 10);//whirlwind wand
                        ammo.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, 7), 10);//pounce wand
                        ammo.Spawns.Add(new InvItem("wand_warp", false, 1), new IntRange(0, 7), 10);//warp wand
                        ammo.Spawns.Add(new InvItem("wand_lob", false, 2), new IntRange(0, 7), 10);//lob wand
                        ammo.Spawns.Add(new InvItem("ammo_geo_pebble", false, 2), new IntRange(0, 7), 10);//Geo Pebble

                        //orbs
                        CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                        orbs.SpawnRates.SetRange(10, new IntRange(0, 7));
                        itemSpawnZoneStep.Spawns.Add("orbs", orbs);

                        orbs.Spawns.Add(new InvItem("orb_rebound"), new IntRange(0, 7), 10);//Rebound
                        orbs.Spawns.Add(new InvItem("orb_all_protect"), new IntRange(0, 7), 5);//All Protect
                        orbs.Spawns.Add(new InvItem("orb_luminous"), new IntRange(0, 7), 9);//Luminous
                        orbs.Spawns.Add(new InvItem("orb_mirror"), new IntRange(0, 7), 8);//Mirror Orb

                        //held items
                        CategorySpawn<InvItem> heldItems = new CategorySpawn<InvItem>();
                        heldItems.SpawnRates.SetRange(2, new IntRange(0, 7));
                        itemSpawnZoneStep.Spawns.Add("held", heldItems);

                        heldItems.Spawns.Add(new InvItem("held_power_band"), new IntRange(0, 7), 1);//Power Band
                        heldItems.Spawns.Add(new InvItem("held_special_band"), new IntRange(0, 7), 1);//Special Band
                        heldItems.Spawns.Add(new InvItem("held_defense_scarf"), new IntRange(0, 7), 1);//Defense Scarf

                        //special
                        CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                        special.SpawnRates.SetRange(7, new IntRange(0, 7));
                        itemSpawnZoneStep.Spawns.Add("special", special);

                        int rate = 2;
                        special.Spawns.Add(new InvItem("apricorn_blue"), new IntRange(0, 7), rate);//blue apricorns
                        special.Spawns.Add(new InvItem("apricorn_green"), new IntRange(0, 7), rate);//green apricorns
                        special.Spawns.Add(new InvItem("apricorn_white"), new IntRange(0, 7), rate);//white apricorns
                        special.Spawns.Add(new InvItem("apricorn_red"), new IntRange(0, 7), rate);//red apricorns
                        special.Spawns.Add(new InvItem("apricorn_yellow"), new IntRange(0, 7), rate);//yellow apricorns
                        special.Spawns.Add(new InvItem("key", false, 1), new IntRange(2, 7), 10);//Key

                        floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                        //mobs
                        TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                        poolSpawn.Priority = PR_RESPAWN_MOB;

                        //need one super-effective for each possible starter
                        //037 Vulpix : 52 Ember
                        poolSpawn.Spawns.Add(GetTeamMob("vulpix", "", "ember", "", "", "", new RandRange(10), "wander_dumb"), new IntRange(4, 7), 10);

                        poolSpawn.Spawns.Add(GetTeamMob("petilil", "", "sleep_powder", "absorb", "", "", new RandRange(10), "wander_dumb"), new IntRange(4, 7), 10);
                        //403 Shinx : 033 Tackle : 43 Leer
                        poolSpawn.Spawns.Add(GetTeamMob("shinx", "", "tackle", "leer", "", "", new RandRange(5), "wander_dumb"), new IntRange(0, 4), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("shinx", "", "tackle", "leer", "", "", new RandRange(7), "wander_dumb"), new IntRange(4, 7), 10);
                        //190 Aipom : 010 Scratch : Sand Attack
                        poolSpawn.Spawns.Add(GetTeamMob("aipom", "", "scratch", "sand_attack", "", "", new RandRange(8), "wander_dumb"), new IntRange(0, 7), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("kricketot", "", "growl", "bide", "", "", new RandRange(6), "wander_dumb"), new IntRange(0, 4), 10);
                        //060 Poliwag : 55 Water Gun
                        poolSpawn.Spawns.Add(GetTeamMob("poliwag", "", "water_gun", "", "", "", new RandRange(8), "wander_dumb"), new IntRange(4, 7), 10);
                        
                        poolSpawn.Spawns.Add(GetTeamMob("starly", "", "tackle", "quick_attack", "", "", new RandRange(6), "wander_dumb"), new IntRange(0, 4), 10);
                        poolSpawn.SpecificSpawns.Add(new SpecificTeamSpawner(GetGenericMob("starly", "", "tackle", "quick_attack", "", "", new RandRange(7), "wander_dumb"), GetGenericMob("starly", "", "tackle", "quick_attack", "", "", new RandRange(7), "wander_dumb")), new IntRange(4, 7), 20);

                        poolSpawn.TeamSizes.Add(1, new IntRange(0, 7), 12);
                        floorSegment.ZoneSteps.Add(poolSpawn);

                        AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(3, 8), new IntRange(0, max_floors)), new MapItem("food_apple"));
                        AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("berry_leppa"));

                        AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(3, 7), new IntRange(0, max_floors)),
                            new MapItem("apricorn_blue"), new MapItem("apricorn_green"),
                            new MapItem("apricorn_red"), new MapItem("apricorn_white"), new MapItem("apricorn_yellow"));


                        RandBag<IGenStep> npcZoneSpawns = new RandBag<IGenStep>(true, new List<IGenStep>());
                        //Recruitment System
                        {
                            PresetMultiTeamSpawner<ListMapGenContext> multiTeamSpawner = new PresetMultiTeamSpawner<ListMapGenContext>();
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID("bellossom", 0, "normal", Gender.Female);
                            post_mob.Tactic = "slow_patrol";
                            post_mob.Level = new RandRange(21);
                            post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_ADVICE_RECRUIT"))));
                            SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);
                            post_team.Explorer = true;
                            multiTeamSpawner.Spawns.Add(post_team);
                            PlaceRandomMobsStep<ListMapGenContext> randomSpawn = new PlaceRandomMobsStep<ListMapGenContext>(multiTeamSpawner);
                            randomSpawn.Ally = true;
                            npcZoneSpawns.ToSpawn.Add(randomSpawn);
                        }
                        //Song
                        {
                            PresetMultiTeamSpawner<ListMapGenContext> multiTeamSpawner = new PresetMultiTeamSpawner<ListMapGenContext>();
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID("furret", 0, "normal", Gender.Male);
                            post_mob.Tactic = "slow_patrol";
                            post_mob.Level = new RandRange(21);
                            post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_ADVICE_FADED"))));
                            SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);
                            post_team.Explorer = true;
                            multiTeamSpawner.Spawns.Add(post_team);
                            PlaceRandomMobsStep<ListMapGenContext> randomSpawn = new PlaceRandomMobsStep<ListMapGenContext>(multiTeamSpawner);
                            randomSpawn.Ally = true;
                            npcZoneSpawns.ToSpawn.Add(randomSpawn);
                        }
                        //Aipom and wonder tiles
                        {
                            PresetMultiTeamSpawner<ListMapGenContext> multiTeamSpawner = new PresetMultiTeamSpawner<ListMapGenContext>();
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID("swablu", 0, "normal", Gender.Male);
                            post_mob.Tactic = "slow_wander";
                            post_mob.Level = new RandRange(14);
                            post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new BattleScriptEvent("AccuracyTalk")));
                            //post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_ADVICE_STAT_DROP"))));
                            {
                                StatusEffect stack = new StatusEffect("mod_accuracy");
                                stack.StatusStates.Set(new StackState(-1));
                                MobSpawnStatus status = new MobSpawnStatus();
                                status.Statuses.Add(stack, 10);
                                post_mob.SpawnFeatures.Add(status);
                            }
                            SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);
                            post_team.Explorer = true;
                            multiTeamSpawner.Spawns.Add(post_team);
                            PlaceRandomMobsStep<ListMapGenContext> randomSpawn = new PlaceRandomMobsStep<ListMapGenContext>(multiTeamSpawner);
                            randomSpawn.Ally = true;
                            npcZoneSpawns.ToSpawn.Add(randomSpawn);
                        }
                        SpreadStepZoneStep npcZoneStep = new SpreadStepZoneStep(new SpreadPlanQuota(new RandRange(2), new IntRange(1, 5), true), PR_SPAWN_MOBS_EXTRA, npcZoneSpawns);
                        floorSegment.ZoneSteps.Add(npcZoneStep);


                        TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                        tileSpawn.Priority = PR_RESPAWN_TRAP;
                        floorSegment.ZoneSteps.Add(tileSpawn);

                        AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 2);

                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            AddFloorData(layout, "B05. Faded Trail.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Clear);


                            if (ii > 4)
                                AddWaterSteps(layout, "water", new RandRange(30));//water
                            else
                                AddWaterSteps(layout, "water", new RandRange(40));//water


                            //Tilesets
                            AddTextureData(layout, "tiny_meadow_wall", "tiny_meadow_floor", "tiny_meadow_secondary", "normal");

                            //money
                            AddMoneyData(layout, new RandRange(2, 4));

                            //items
                            AddItemData(layout, new RandRange(2, 5), 25);

                            //enemies
                            AddRespawnData(layout, 5, 80);
                            AddEnemySpawnData(layout, 20, new RandRange(2, 5));

                            //traps
                            AddSingleTrapStep(layout, new RandRange(5, 7), "tile_wonder");//wonder tile
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
                                //making room for the vault
                                {
                                    ResizeFloorStep<MapGenContext> addSizeStep = new ResizeFloorStep<MapGenContext>(new Loc(16, 16), Dir8.None);
                                    layout.GenSteps.Add(PR_ROOMS_PRE_VAULT, addSizeStep);
                                    ClampFloorStep<MapGenContext> limitStep = new ClampFloorStep<MapGenContext>(new Loc(0), new Loc(78, 54));
                                    layout.GenSteps.Add(PR_ROOMS_PRE_VAULT, limitStep);
                                    ClampFloorStep<MapGenContext> clampStep = new ClampFloorStep<MapGenContext>();
                                    layout.GenSteps.Add(PR_ROOMS_PRE_VAULT_CLAMP, clampStep);
                                }

                                //vault rooms
                                {
                                    SpawnList<RoomGen<MapGenContext>> detourRooms = new SpawnList<RoomGen<MapGenContext>>();
                                    detourRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
                                    SpawnList<PermissiveRoomGen<MapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                                    detourHalls.Add(new RoomGenAngledHall<MapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                                    AddConnectedRoomsStep<MapGenContext> detours = new AddConnectedRoomsStep<MapGenContext>(detourRooms, detourHalls);
                                    detours.Amount = new RandRange(1);
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
                                    SwitchSealStep<MapGenContext> vaultStep = new SwitchSealStep<MapGenContext>("sealed_block", "tile_switch", 1, true, false);
                                    vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                                    vaultStep.SwitchFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                                    vaultStep.SwitchFilters.Add(new RoomFilterComponent(true, new BossRoom()));
                                    layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                                }

                                //vault treasures
                                {
                                    BulkSpawner<MapGenContext, EffectTile> treasures = new BulkSpawner<MapGenContext, EffectTile>();

                                    EffectTile secretStairs = new EffectTile("stairs_secret_up", true);
                                    secretStairs.TileStates.Set(new DestState(new SegLoc(1, 0)));
                                    treasures.SpecificSpawns.Add(secretStairs);

                                    RandomRoomSpawnStep<MapGenContext, EffectTile> detourItems = new RandomRoomSpawnStep<MapGenContext, EffectTile>(treasures);
                                    detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                                    layout.GenSteps.Add(PR_EXITS_DETOUR, detourItems);
                                }
                            }

                            layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                            if (ii == 4)
                                layout.GenSteps.Add(PR_DBG_CHECK, new DetectTileStep<MapGenContext>("stairs_secret_up"));

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }


                    {
                        int max_floors = 3;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Hidden Trail\nB{0}F"), new Priority(-15)));

                        //money
                        MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 10);
                        moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                        floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                        //items
                        ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                        itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;

                        //necessities
                        CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                        necessities.SpawnRates.SetRange(14, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("necessities", necessities);

                        necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, 3), 9);//Leppa
                        necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, 3), 12);//Oran
                        necessities.Spawns.Add(new InvItem("food_apple"), new IntRange(0, 3), 10);//Apple
                        necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, 3), 10);//Lum
                        necessities.Spawns.Add(new InvItem("seed_reviver"), new IntRange(0, 3), 5);//reviver seed

                        //snacks
                        CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                        snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("snacks", snacks);

                        snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, 3), 20);//blast seed
                        snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, 3), 10);//warp seed
                        snacks.Spawns.Add(new InvItem("seed_sleep"), new IntRange(0, 3), 10);//sleep seed

                        //wands
                        CategorySpawn<InvItem> ammo = new CategorySpawn<InvItem>();
                        ammo.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("ammo", ammo);

                        ammo.Spawns.Add(new InvItem("ammo_stick", false, 3), new IntRange(0, 3), 10);//stick
                        ammo.Spawns.Add(new InvItem("wand_whirlwind", false, 2), new IntRange(0, 3), 10);//whirlwind wand
                        ammo.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, 3), 10);//pounce wand
                        ammo.Spawns.Add(new InvItem("wand_warp", false, 1), new IntRange(0, 3), 10);//warp wand
                        ammo.Spawns.Add(new InvItem("wand_lob", false, 2), new IntRange(0, 3), 10);//lob wand
                        ammo.Spawns.Add(new InvItem("ammo_geo_pebble", false, 2), new IntRange(0, 3), 10);//Geo Pebble

                        //orbs
                        CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                        orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("orbs", orbs);

                        orbs.Spawns.Add(new InvItem("orb_rebound"), new IntRange(0, 3), 10);//Rebound
                        orbs.Spawns.Add(new InvItem("orb_all_protect"), new IntRange(0, 3), 5);//All Protect
                        orbs.Spawns.Add(new InvItem("orb_luminous"), new IntRange(0, 3), 9);//Luminous
                        orbs.Spawns.Add(new InvItem("orb_mirror"), new IntRange(0, 3), 8);//Mirror Orb

                        //held items
                        CategorySpawn<InvItem> heldItems = new CategorySpawn<InvItem>();
                        heldItems.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("held", heldItems);

                        heldItems.Spawns.Add(new InvItem("held_power_band"), new IntRange(0, 3), 1);//Power Band
                        heldItems.Spawns.Add(new InvItem("held_special_band"), new IntRange(0, 3), 1);//Special Band
                        heldItems.Spawns.Add(new InvItem("held_defense_scarf"), new IntRange(0, 3), 1);//Defense Scarf

                        //special
                        CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                        special.SpawnRates.SetRange(7, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("special", special);

                        int rate = 2;
                        special.Spawns.Add(new InvItem("apricorn_brown"), new IntRange(0, 3), rate);//brown apricorns
                        special.Spawns.Add(new InvItem("apricorn_white"), new IntRange(0, 3), rate);//white apricorns

                        floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                        //mobs
                        TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                        poolSpawn.Priority = PR_RESPAWN_MOB;

                        //need one super-effective for each possible starter
                        //403 Shinx : 033 Tackle : 43 Leer
                        poolSpawn.SpecificSpawns.Add(new SpecificTeamSpawner(GetGenericMob("shinx", "", "tackle", "leer", "", "", new RandRange(8), "wander_dumb"), GetGenericMob("shinx", "", "tackle", "leer", "", "", new RandRange(6), "wander_dumb")), new IntRange(0, 3), 10);
                        //190 Aipom : 010 Scratch : Sand Attack
                        poolSpawn.SpecificSpawns.Add(new SpecificTeamSpawner(GetGenericMob("aipom", "", "scratch", "sand_attack", "", "", new RandRange(8), "wander_dumb"), GetGenericMob("aipom", "", "scratch", "sand_attack", "", "", new RandRange(8), "wander_dumb")), new IntRange(0, 3), 10);
                        //114 Tangela : 022 Vine Whip
                        poolSpawn.Spawns.Add(GetTeamMob("tangela", "", "vine_whip", "ingrain", "", "", new RandRange(10), "wander_dumb"), new IntRange(0, 3), 10);
                        //161 Sentret : Charm : Bite
                        poolSpawn.Spawns.Add(GetTeamMob("snubbull", "", "charm", "bite", "", "", new RandRange(10), "wander_dumb"), new IntRange(0, 3), 10);
                        //396 Starly : 33 Tackle : 45 Growl - later pairs
                        poolSpawn.SpecificSpawns.Add(new SpecificTeamSpawner(GetGenericMob("starly", "", "tackle", "quick_attack", "", "", new RandRange(7), "wander_dumb"), GetGenericMob("starly", "", "tackle", "quick_attack", "", "", new RandRange(7), "wander_dumb")), new IntRange(0, 3), 20);
                        //438 Bonsly : 88 Rock Throw
                        poolSpawn.Spawns.Add(GetTeamMob("bonsly", "", "rock_throw", "", "", "", new RandRange(9), "weird_tree"), new IntRange(0, 3), 8);

                        poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                        floorSegment.ZoneSteps.Add(poolSpawn);

                        TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                        tileSpawn.Priority = PR_RESPAWN_TRAP;
                        floorSegment.ZoneSteps.Add(tileSpawn);

                        AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 15, 2);

                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            AddFloorData(layout, "B02. Demonstration 2.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                            AddWaterSteps(layout, "water", new RandRange(20));//empty
                            FloorTerrainStep<MapGenContext> floorPass = new FloorTerrainStep<MapGenContext>(new Tile("floor"));
                            floorPass.TerrainStencil = new MatchTerrainStencil<MapGenContext>(false, new Tile("water"));
                            layout.GenSteps.Add(PR_WATER_DE_ISOLATE, floorPass);

                            //Tilesets
                            AddTextureData(layout, "wyvern_hill_wall", "wyvern_hill_floor", "wyvern_hill_secondary", "normal");

                            //money
                            AddMoneyData(layout, new RandRange(1, 4));

                            //items
                            AddItemData(layout, new RandRange(2, 5), 25);

                            //enemies! ~ lv 5 to 10
                            AddRespawnData(layout, 4, 80);
                            AddEnemySpawnData(layout, 20, new RandRange(2, 4));

                            //traps
                            AddSingleTrapStep(layout, new RandRange(5, 8), "tile_wonder");//wonder tile
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

                            AddStairStep(layout, true);

                            layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }


                    {
                        SingularSegment structure = new SingularSegment(-1);

                        ChanceFloorGen multiGen = new ChanceFloorGen();
                        multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "small_square", -2, false, false), 10);
                        multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "tall_hall", -2, false, false), 10);
                        multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "wide_hall", -2, false, false), 10);
                        structure.BaseFloor = multiGen;

                        zone.Segments.Add(structure);
                    }


                    {
                        SingularSegment structure = new SingularSegment(-1);

                        ChanceFloorGen multiGen = new ChanceFloorGen();
                        multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "small_square", -2, false, false), 10);
                        multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "tall_hall", -2, false, false), 10);
                        multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "wide_hall", -2, false, false), 10);
                        structure.BaseFloor = multiGen;

                        zone.Segments.Add(structure);
                    }
                }
                #endregion
            }
            else if (index == 4)
            {
                #region FLYAWAY CLIFFS
                {
                    zone.Name = new LocalText("Flyaway Cliffs");
                    zone.Rescues = 2;
                    zone.Level = 20;
                    zone.Rogue = RogueStatus.NoTransfer;

                    int max_floors = 10;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Flyaway Cliffs\n{0}F"), new Priority(-15)));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 0);
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);
                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;


                    //necessities
                    CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                    necessities.SpawnRates.SetRange(14, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("necessities", necessities);


                    necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 9);
                    necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 12);
                    necessities.Spawns.Add(new InvItem("food_apple"), new IntRange(0, max_floors), 10);
                    necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 10);
                    necessities.Spawns.Add(new InvItem("seed_reviver"), new IntRange(0, max_floors), 5);
                    necessities.Spawns.Add(new InvItem("food_apple_big"), new IntRange(0, max_floors), 3);
                    //snacks
                    CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                    snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                    snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, max_floors), 20);
                    snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_sleep"), new IntRange(0, max_floors), 10);
                    //boosters
                    CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                    boosters.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("boosters", boosters);


                    boosters.Spawns.Add(new InvItem("gummi_blue"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_black"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_clear"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_grass"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_green"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_brown"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_orange"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_gold"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_red"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_royal"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_silver"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_white"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_yellow"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_sky"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_gray"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_magenta"), new IntRange(0, max_floors), 1);
                    //special
                    CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                    special.SpawnRates.SetRange(5, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("special", special);


                    special.Spawns.Add(new InvItem("machine_recall_box"), new IntRange(0, max_floors), 25);
                    special.Spawns.Add(new InvItem("apricorn_white"), new IntRange(0, max_floors), 5);
                    special.Spawns.Add(new InvItem("apricorn_plain"), new IntRange(0, max_floors), 5);
                    special.Spawns.Add(new InvItem("key", false, 1), new IntRange(0, max_floors), 5);
                    //throwable
                    CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                    throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                    throwable.Spawns.Add(new InvItem("wand_path", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_fear", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_switcher", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_slow", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("ammo_stick", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("ammo_geo_pebble", false, 2), new IntRange(0, max_floors), 10);
                    //orbs
                    CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                    orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                    orbs.Spawns.Add(new InvItem("orb_all_aim"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_rebound"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_all_dodge"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_petrify"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_slumber"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_foe_hold"), new IntRange(0, max_floors), 10);
                    //held
                    CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                    held.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("held", held);


                    held.Spawns.Add(new InvItem("held_scope_lens"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_warp_scarf"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_metronome"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_wide_lens"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_sharp_beak"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_silk_scarf"), new IntRange(0, max_floors), 10);


                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;


                    poolSpawn.Spawns.Add(GetTeamMob("butterfree", "compound_eyes", "gust", "", "", "", new RandRange(16), "wander_dumb"), new IntRange(0, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("togepi", "", "sweet_kiss", "metronome", "", "", new RandRange(14), "wander_dumb"), new IntRange(2, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("chatot", "", "round", "", "", "", new RandRange(16), "wander_dumb"), new IntRange(0, 6), 10);
                    //groups of two
                    poolSpawn.SpecificSpawns.Add(new SpecificTeamSpawner(GetGenericMob("hoppip", "", "tail_whip", "splash", "synthesis", "", new RandRange(16), "wander_dumb"), GetGenericMob("hoppip", "", "tail_whip", "splash", "synthesis", "", new RandRange(16), "wander_dumb")), new IntRange(0, 6), 20);
                    
                    poolSpawn.Spawns.Add(GetTeamMob("lickitung", "", "wrap", "", "", "", new RandRange(17), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("hoothoot", "", "foresight", "peck", "", "", new RandRange(16), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("wingull", "", "growl", "quick_attack", "", "", new RandRange(18), "wander_dumb"), new IntRange(2, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("spinda", "", "dizzy_punch", "", "", "", new RandRange(18), "wander_dumb"), new IntRange(6, max_floors), 10);
                    //sleeping
                    {
                        TeamMemberSpawn mob = GetTeamMob("farfetchd", "defiant", "aerial_ace", "knock_off", "poison_jab", "", new RandRange(25), TeamMemberSpawn.MemberRole.Loner, "wander_dumb", true);
                        MobSpawnItem itemSpawn = new MobSpawnItem(true);
                        itemSpawn.Items.Add(new InvItem("held_power_band"), 10);
                        itemSpawn.Items.Add(new InvItem("held_defense_scarf"), 10);
                        itemSpawn.Items.Add(new InvItem("held_zinc_band"), 10);
                        itemSpawn.Items.Add(new InvItem("held_special_band"), 10);
                        mob.Spawn.SpawnFeatures.Add(itemSpawn);
                        poolSpawn.Spawns.Add(mob, new IntRange(6, max_floors), 5);
                    }
                    poolSpawn.Spawns.Add(GetTeamMob("staravia", "", "double_team", "wing_attack", "", "", new RandRange(19), "wander_dumb"), new IntRange(6, max_floors), 10);
                    
                    poolSpawn.Spawns.Add(GetTeamMob("chatot", "", "round", "", "", "", new RandRange(16), TeamMemberSpawn.MemberRole.Support, "wander_dumb"), new IntRange(6, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("skarmory", "", "metal_claw", "air_cutter", "", "", new RandRange(20), "wander_dumb"), new IntRange(6, max_floors), 5);

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, 10), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);


                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 8), new IntRange(0, max_floors)), new MapItem("food_apple"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("berry_leppa"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("apricorn_white"));


                    RandBag<IGenStep> npcZoneSpawns = new RandBag<IGenStep>(true, new List<IGenStep>());
                    //Speed stat and missing
                    {
                        PresetMultiTeamSpawner<ListMapGenContext> multiTeamSpawner = new PresetMultiTeamSpawner<ListMapGenContext>();
                        MobSpawn post_mob = new MobSpawn();
                        post_mob.BaseForm = new MonsterID("pikachu", 0, "normal", Gender.Male);
                        post_mob.Tactic = "slow_wander";
                        post_mob.Level = new RandRange(28);
                        post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_ADVICE_MISS"))));
                        SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);
                        post_team.Explorer = true;
                        multiTeamSpawner.Spawns.Add(post_team);
                        PlaceRandomMobsStep<ListMapGenContext> randomSpawn = new PlaceRandomMobsStep<ListMapGenContext>(multiTeamSpawner);
                        randomSpawn.Ally = true;
                        npcZoneSpawns.ToSpawn.Add(randomSpawn);
                    }
                    //Team Mode
                    {
                        PresetMultiTeamSpawner<ListMapGenContext> multiTeamSpawner = new PresetMultiTeamSpawner<ListMapGenContext>();
                        SpecificTeamSpawner post_team = new SpecificTeamSpawner();
                        post_team.Explorer = true;
                        {
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID("plusle", 0, "normal", Gender.Male);
                            post_mob.Tactic = "slow_wander";
                            post_mob.Level = new RandRange(20);
                            post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new BattleScriptEvent("PairTalk", "{Pair=0}")));
                            post_team.Spawns.Add(post_mob);
                        }
                        {
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID("minun", 0, "normal", Gender.Male);
                            post_mob.Tactic = "slow_wander";
                            post_mob.Level = new RandRange(20);
                            post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new BattleScriptEvent("PairTalk", "{Pair=1}")));
                            post_team.Spawns.Add(post_mob);
                        }
                        multiTeamSpawner.Spawns.Add(post_team);
                        PlaceRandomMobsStep<ListMapGenContext> randomSpawn = new PlaceRandomMobsStep<ListMapGenContext>(multiTeamSpawner);
                        randomSpawn.Ally = true;
                        npcZoneSpawns.ToSpawn.Add(randomSpawn);
                    }
                    SpreadStepZoneStep npcZoneStep = new SpreadStepZoneStep(new SpreadPlanQuota(new RandRange(2), new IntRange(0, 8), true), PR_SPAWN_MOBS_EXTRA, npcZoneSpawns);
                    floorSegment.ZoneSteps.Add(npcZoneStep);


                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    floorSegment.ZoneSteps.Add(tileSpawn);


                    //a monster house can simply pull random items from the map's spawnlist, or use a theme

                    //so, themes include:
                    //no theme (choose from any in the special list)
                    //all gummis
                    //all apricorns
                    //all seeds
                    //all manmade items
                    //all gold items
                    //all money


                    //a monster house can simply pull random enemies from the map's spawnlist
                    //or, it can be themed: it will only spawn enemies that have a certain attribute
                    //specify the chance of each theme, including the chance of no theme
                    //themes include:
                    //no theme (choose random ratio between the floor spawnlist and the special spawnlist)
                    //evolution family
                    //typing (one type or one of two types) //specify two types vs one type
                    //    specify whether or not to include first evos (evolve && !devolve), final evos (!evolve && devolve), mids (evolve && devolve) or singles (!evolve && !devolve)
                    //stats (atk, def, spatk, spdef, hp, speed) is their highest
                    ////stats (physical, special, offensive, defensive) one of the two prized stats is the highest, the other is in top 3,
                    ////    the lowest stat is one of the opposite, and the other is in bottom 3



                    //switch vaults
                    {
                        SpreadVaultZoneStep vaultChanceZoneStep = new SpreadVaultZoneStep(PR_SPAWN_ITEMS_EXTRA, PR_SPAWN_TRAPS, PR_SPAWN_MOBS_EXTRA, new SpreadPlanQuota(new RandDecay(1, 8, 35), new IntRange(0, max_floors-1)));

                        //making room for the vault
                        {
                            ResizeFloorStep<ListMapGenContext> addSizeStep = new ResizeFloorStep<ListMapGenContext>(new Loc(16, 16), Dir8.None);
                            vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT, addSizeStep));
                            ClampFloorStep<ListMapGenContext> limitStep = new ClampFloorStep<ListMapGenContext>(new Loc(0), new Loc(78, 54));
                            vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT, limitStep));
                            ClampFloorStep<ListMapGenContext> clampStep = new ClampFloorStep<ListMapGenContext>();
                            vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT_CLAMP, clampStep));
                        }

                        // room addition step
                        {
                            SpawnList<RoomGen<ListMapGenContext>> detourRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                            detourRooms.Add(new RoomGenSquare<ListMapGenContext>(new RandRange(2), new RandRange(2)), 10);
                            SpawnList<PermissiveRoomGen<ListMapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                            detourHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                            AddConnectedRoomsRandStep<ListMapGenContext> detours = new AddConnectedRoomsRandStep<ListMapGenContext>(detourRooms, detourHalls);
                            detours.Amount = new RandRange(1);
                            detours.HallPercent = 100;
                            detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom(), new UnVaultableRoom()));
                            detours.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.SwitchVault));
                            detours.RoomComponents.Set(new NoConnectRoom());
                            detours.RoomComponents.Set(new NoEventRoom());
                            detours.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.SwitchVault));
                            detours.HallComponents.Set(new NoConnectRoom());
                            detours.HallComponents.Set(new NoEventRoom());

                            vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_GEN_EXTRA, detours));
                        }

                        //sealing the vault
                        {
                            SwitchSealStep<ListMapGenContext> vaultStep = new SwitchSealStep<ListMapGenContext>("sealed_block", "tile_switch", 1, true, false);
                            vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                            vaultStep.SwitchFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            vaultStep.SwitchFilters.Add(new RoomFilterComponent(true, new BossRoom()));
                            vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_TILES_GEN_EXTRA, vaultStep));
                        }

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


                        floorSegment.ZoneSteps.Add(vaultChanceZoneStep);
                    }


                    AddHiddenStairStep(floorSegment, new SpreadPlanQuota(new RandRange(1), new IntRange(0, max_floors - 1)), 1);

                    AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 2);

                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        if (ii < 2)
                            AddFloorData(layout, "B07. Flyaway Cliffs.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Clear);
                        else
                            AddFloorData(layout, "B07. Flyaway Cliffs.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                        if (ii > 2)
                            AddDefaultMapStatus(layout, "default_weather", "cloudy", "clear", "clear", "clear", "clear");

                        if (ii < 2)
                        {
                            AddWaterSteps(layout, "pit", new RandRange(20));//abyss
                        }
                        else if (ii < 6)
                        {
                            AddWaterSteps(layout, "pit", new RandRange(60));//abyss
                            AddGrassSteps(layout, new RandRange(0), new IntRange(3, 9), new RandRange(30));
                        }
                        else
                        {
                            AddWaterSteps(layout, "pit", new RandRange(40));//abyss
                            AddGrassSteps(layout, new RandRange(4, 8), new IntRange(3, 9), new RandRange(10));
                        }


                        //Tilesets
                        //Mt. Horn -> hidden Land -> hidden land 2 -> mt. horn?
                        if (ii < 6)
                            AddSpecificTextureData(layout, "hidden_land_wall", "hidden_land_floor", "hidden_land_secondary", "tall_grass_dark", "flying");
                        else
                            AddSpecificTextureData(layout, "hidden_highland_wall", "hidden_highland_floor", "hidden_highland_secondary", "tall_grass", "flying");

                        //money
                        AddMoneyData(layout, new RandRange(2, 5));

                        //items
                        AddItemData(layout, new RandRange(2, 6), 25);

                        //enemies!
                        AddRespawnData(layout, 6, 90);

                        //enemies
                        AddEnemySpawnData(layout, 20, new RandRange(3, 6));

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(6, 9));

                        //construct paths
                        {
                            AddInitGridStep(layout, 4, 4, 10, 10);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(90);
                            path.BranchRatio = new RandRange(0, 25);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(50, 0));

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 11), new RandRange(4, 11), new RandRange(2, 6), new RandRange(2, 6)), 10);
                            //cave
                            genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, false);


                        if (ii == max_floors - 1)
                        {
                            //making room for the vault
                            {
                                ResizeFloorStep<MapGenContext> addSizeStep = new ResizeFloorStep<MapGenContext>(new Loc(16, 16), Dir8.None);
                                layout.GenSteps.Add(PR_ROOMS_PRE_VAULT, addSizeStep);
                                ClampFloorStep<MapGenContext> limitStep = new ClampFloorStep<MapGenContext>(new Loc(0), new Loc(78, 54));
                                layout.GenSteps.Add(PR_ROOMS_PRE_VAULT, limitStep);
                                ClampFloorStep<MapGenContext> clampStep = new ClampFloorStep<MapGenContext>();
                                layout.GenSteps.Add(PR_ROOMS_PRE_VAULT_CLAMP, clampStep);
                            }

                            //vault rooms
                            {
                                SpawnList<RoomGen<MapGenContext>> detourRooms = new SpawnList<RoomGen<MapGenContext>>();

                                RoomGenLoadMap<MapGenContext> loadRoom = new RoomGenLoadMap<MapGenContext>();
                                loadRoom.MapID = "room_flying_item";
                                loadRoom.RoomTerrain = new Tile("floor");
                                loadRoom.PreventChanges = PostProcType.Panel | PostProcType.Terrain;
                                detourRooms.Add(loadRoom, 10);
                                SpawnList<PermissiveRoomGen<MapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                                detourHalls.Add(new RoomGenAngledHall<MapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                                AddConnectedRoomsStep<MapGenContext> detours = new AddConnectedRoomsStep<MapGenContext>(detourRooms, detourHalls);
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
                            //sealing the vault
                            {
                                KeySealStep<MapGenContext> vaultStep = new KeySealStep<MapGenContext>("sealed_block", "sealed_door", "key");
                                vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                                layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                            }
                        }

                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        floorSegment.Floors.Add(layout);
                    }
                    zone.Segments.Add(floorSegment);


                    {
                        SingularSegment structure = new SingularSegment(-1);

                        SpawnList<TeamMemberSpawn> enemyList = new SpawnList<TeamMemberSpawn>();
                        enemyList.Add(GetTeamMob(new MonsterID("vivillon", 0, "", Gender.Unknown), "", "poison_powder", "psybeam", "powder", "struggle_bug", new RandRange(zone.Level)), 10);
                        structure.BaseFloor = getSecretRoom(translate, "special_rby_bird", -1, "hidden_land_wall", "hidden_land_floor", "hidden_land_secondary", "tall_grass_dark", "flying", enemyList, new Loc(6, 4));

                        zone.Segments.Add(structure);
                    }


                    {
                        SingularSegment structure = new SingularSegment(-1);

                        ChanceFloorGen multiGen = new ChanceFloorGen();
                        multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "small_square", -2, false, false), 10);
                        multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "tall_hall", -2, false, false), 10);
                        multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "wide_hall", -2, false, false), 10);
                        structure.BaseFloor = multiGen;

                        zone.Segments.Add(structure);
                    }
                }
                #endregion
            }
            else if (index == 5)
            {
                #region THUNDERSTRUCK PASS
                {
                    zone.Name = new LocalText("Thunderstruck Pass");
                    zone.Rescues = 2;
                    zone.Level = 30;
                    zone.Rogue = RogueStatus.NoTransfer;

                    {
                        int max_floors = 14;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Thunderstruck Pass\n{0}F"), new Priority(-15)));

                        //money
                        MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 0);
                        moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                        floorSegment.ZoneSteps.Add(moneySpawnZoneStep);
                        //items
                        ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                        itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;


                        //necessities
                        CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                        necessities.SpawnRates.SetRange(14, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("necessities", necessities);


                        necessities.Spawns.Add(new InvItem("berry_leppa", true), new IntRange(0, max_floors), 3);
                        necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 6);
                        necessities.Spawns.Add(new InvItem("berry_oran", true), new IntRange(0, max_floors), 2);
                        necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 4);
                        necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 10);
                        necessities.Spawns.Add(new InvItem("berry_sitrus"), new IntRange(0, max_floors), 6);
                        //snacks
                        CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                        snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                        snacks.Spawns.Add(new InvItem("berry_rowap"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("herb_power"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_doom"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_blinker"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("berry_wacan"), new IntRange(0, max_floors), 10);
                        //boosters
                        CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                        boosters.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("boosters", boosters);


                        boosters.Spawns.Add(new InvItem("gummi_blue"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_black"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_clear"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_grass"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_green"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_brown"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_orange"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_gold"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_red"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_royal"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_silver"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_white"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_yellow"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_sky"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_gray"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_magenta"), new IntRange(0, max_floors), 1);
                        //special
                        CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                        special.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("special", special);


                        special.Spawns.Add(new InvItem("apricorn_brown", true), new IntRange(0, max_floors), 3);
                        special.Spawns.Add(new InvItem("apricorn_brown"), new IntRange(0, max_floors), 7);
                        special.Spawns.Add(new InvItem("apricorn_yellow", true), new IntRange(0, max_floors), 3);
                        special.Spawns.Add(new InvItem("apricorn_yellow"), new IntRange(0, max_floors), 7);
                        special.Spawns.Add(new InvItem("machine_assembly_box", true), new IntRange(0, max_floors), 3);
                        special.Spawns.Add(new InvItem("machine_assembly_box"), new IntRange(0, max_floors), 7);
                        //evo
                        CategorySpawn<InvItem> evo = new CategorySpawn<InvItem>();
                        evo.SpawnRates.SetRange(1, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("evo", evo);


                        evo.Spawns.Add(new InvItem("evo_thunder_stone"), new IntRange(0, max_floors), 10);
                        //throwable
                        CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                        throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                        throwable.Spawns.Add(new InvItem("ammo_iron_thorn", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("ammo_stick", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_lob", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_fear", false, 2), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_warp", false, 2), new IntRange(0, max_floors), 10);
                        //orbs
                        CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                        orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                        orbs.Spawns.Add(new InvItem("orb_endure", true), new IntRange(0, max_floors), 2);
                        orbs.Spawns.Add(new InvItem("orb_endure"), new IntRange(0, max_floors), 8);
                        orbs.Spawns.Add(new InvItem("orb_cleanse"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_foe_seal", true), new IntRange(0, max_floors), 2);
                        orbs.Spawns.Add(new InvItem("orb_foe_seal"), new IntRange(0, max_floors), 8);
                        orbs.Spawns.Add(new InvItem("orb_totter", true), new IntRange(0, max_floors), 2);
                        orbs.Spawns.Add(new InvItem("orb_totter"), new IntRange(0, max_floors), 8);
                        orbs.Spawns.Add(new InvItem("orb_all_aim", true), new IntRange(0, max_floors), 2);
                        orbs.Spawns.Add(new InvItem("orb_all_aim"), new IntRange(0, max_floors), 8);
                        orbs.Spawns.Add(new InvItem("orb_pierce", true), new IntRange(0, max_floors), 2);
                        orbs.Spawns.Add(new InvItem("orb_pierce"), new IntRange(0, max_floors), 8);
                        //held
                        CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                        held.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("held", held);


                        held.Spawns.Add(new InvItem("held_sticky_barb"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_ring_target"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_weather_rock"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_wide_lens"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_magnet"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_metal_coat"), new IntRange(0, max_floors), 10);
                        //tms
                        CategorySpawn<InvItem> tms = new CategorySpawn<InvItem>();
                        tms.SpawnRates.SetRange(7, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("tms", tms);


                        tms.Spawns.Add(new InvItem("tm_return"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_frustration"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_giga_drain"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_dive"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_poison_jab"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_torment"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_shadow_claw"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_endure"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_echoed_voice"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_gyro_ball"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_recycle"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_false_swipe"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_defog"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_telekinesis"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_double_team"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_thunder_wave"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_attract"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_steel_wing"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_smack_down"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_snarl"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_flame_charge"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_bulldoze"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_substitute"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_iron_tail"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_brine"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_venoshock"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_u_turn"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_aerial_ace"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_hone_claws"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_rock_smash"), new IntRange(0, max_floors), 10);


                        floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                        //mobs
                        TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                        poolSpawn.Priority = PR_RESPAWN_MOB;


                        poolSpawn.Spawns.Add(GetTeamMob("electabuzz", "", "thunder_punch", "light_screen", "", "", new RandRange(29), "wander_dumb"), new IntRange(10, max_floors), 10);
                        {
                            //311 Plusle : 589 Play Nice : 270 Helping Hand : 486 Electro Ball
                            TeamMemberSpawn teamSpawn = GetTeamMob("plusle", "", "play_nice", "helping_hand", "electro_ball", "", new RandRange(30));
                            teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(0, 2));
                            poolSpawn.Spawns.Add(teamSpawn, new IntRange(13, 17), 10);
                        }
                        {
                            //312 Minun : 313 Fake Tears : 270 Helping Hand : 609 Nuzzle
                            TeamMemberSpawn teamSpawn = GetTeamMob("minun", "", "fake_tears", "helping_hand", "nuzzle", "", new RandRange(30));
                            teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(1, 2));
                            poolSpawn.Spawns.Add(teamSpawn, new IntRange(13, 17), 10);
                        }

                        poolSpawn.Spawns.Add(GetTeamMob("magnemite", "", "thunder_shock", "thunder_wave", "", "", new RandRange(26), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("raichu", "", "thunderbolt", "", "", "", new RandRange(28), "wander_dumb"), new IntRange(10, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("voltorb", "", "eerie_impulse", "charge_beam", "", "", new RandRange(26), "wander_dumb"), new IntRange(0, 10), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("hitmonchan", "", "focus_punch", "agility", "", "", new RandRange(26), "wander_dumb"), new IntRange(5, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("geodude", 1, "", Gender.Unknown), "", "spark", "defense_curl", "", "", new RandRange(24), "wander_dumb"), new IntRange(0, 5), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("loudred", "", "screech", "echoed_voice", "", "", new RandRange(26), "wander_dumb"), new IntRange(0, 10), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("hariyama", "", "fake_out", "smelling_salts", "", "", new RandRange(30), "wander_dumb"), new IntRange(5, max_floors), 10);
                        
                        poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                        poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 6);

                        floorSegment.ZoneSteps.Add(poolSpawn);

                        TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                        tileSpawn.Priority = PR_RESPAWN_TRAP;


                        tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, max_floors), 10);//chestnut trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_slow", false), new IntRange(0, max_floors), 10);//slow trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_spin", false), new IntRange(0, max_floors), 10);//spin trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_hunger", true), new IntRange(0, max_floors), 10);//hunger trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_grimy", false), new IntRange(0, max_floors), 10);//grimy trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_sticky", false), new IntRange(0, max_floors), 10);//sticky trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap

                        floorSegment.ZoneSteps.Add(tileSpawn);

                        AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 8), new IntRange(0, max_floors)), new MapItem("food_apple"), new MapItem("food_grimy"));
                        AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("berry_leppa"));
                        AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(3, max_floors)), new MapItem("machine_assembly_box"));
                        AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(max_floors / 2, max_floors - 1), new IntRange(0, max_floors)), new MapItem("orb_cleanse"));


                        AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)),
                            new MapItem("apricorn_brown"), new MapItem("apricorn_white"));

                        {

                            MobSpawn mob = GetGuardMob("jolteon", "", "thunder", "agility", "shadow_ball", "", new RandRange(50), "wander_normal", "sleep");
                            MobSpawnItem keySpawn = new MobSpawnItem(true);
                            keySpawn.Items.Add(new InvItem("held_wide_lens"), 10);
                            mob.SpawnFeatures.Add(keySpawn);

                            SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                            specificTeam.Spawns.Add(mob);
                            LoopedTeamSpawner<ListMapGenContext> spawner = new LoopedTeamSpawner<ListMapGenContext>(specificTeam, new RandRange(1));

                            SpawnRangeList<IGenStep> specialEnemySpawns = new SpawnRangeList<IGenStep>();
                            specialEnemySpawns.Add(new PlaceRandomMobsStep<ListMapGenContext>(spawner), new IntRange(0, max_floors), 10);
                            SpreadStepRangeZoneStep specialEnemyStep = new SpreadStepRangeZoneStep(new SpreadPlanQuota(new RandDecay(0, 1, 50), new IntRange(0, max_floors - 1)), PR_SPAWN_MOBS, specialEnemySpawns);
                            floorSegment.ZoneSteps.Add(specialEnemyStep);
                        }

                        {
                            //monster houses
                            SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(25, new IntRange(0, max_floors)));
                            monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHouseStep<ListMapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                            foreach (string gummi in IterateGummis())
                                monsterChanceZoneStep.Items.Add(new MapItem(gummi), new IntRange(0, max_floors), 4);//gummis
                            foreach (string iter_item in IterateApricorns())
                                monsterChanceZoneStep.Items.Add(new MapItem(iter_item), new IntRange(0, max_floors), 4);//apricorns

                            monsterChanceZoneStep.Items.Add(new MapItem("food_banana"), new IntRange(0, max_floors), 30);//banana
                            monsterChanceZoneStep.Items.Add(new MapItem("food_apple_big"), new IntRange(0, max_floors), 50);//big apple
                            monsterChanceZoneStep.Items.Add(new MapItem("food_banana_big"), new IntRange(0, max_floors), 10);//big banana
                            monsterChanceZoneStep.Items.Add(new MapItem("loot_heart_scale", 2), new IntRange(0, max_floors), 10);//heart scale
                            monsterChanceZoneStep.Items.Add(new MapItem("key", 1), new IntRange(0, max_floors), 10);//key
                            monsterChanceZoneStep.Items.Add(new MapItem("machine_recall_box"), new IntRange(0, max_floors), 10);//link box

                            monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(1, 4), "loot_pearl"), new ItemThemeNone(40, new RandRange(2, 4))), new IntRange(0, max_floors), 30);//no theme

                            monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(3, 7)), new IntRange(0, max_floors), 20);//gummis
                            monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(RecruitState)), true, true, new RandRange(2, 6)), new IntRange(0, 10), 10);//apricorns
                            monsterChanceZoneStep.MobThemes.Add(new MobThemeNone(40, new RandRange(7, 13)), new IntRange(0, max_floors), 10);
                            floorSegment.ZoneSteps.Add(monsterChanceZoneStep);
                        }


                        SpawnRangeList<IGenStep> shopZoneSpawns = new SpawnRangeList<IGenStep>();
                        {
                            ShopStep<ListMapGenContext> shop = new ShopStep<ListMapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                            shop.Personality = 0;
                            shop.SecurityStatus = "shop_security";

                            shop.Items.Add(new MapItem("seed_reviver", 0, 800), 15);//reviver
                            shop.Items.Add(new MapItem("apricorn_big", 0, 1000), 5);//big apricorn
                            shop.Items.Add(new MapItem("seed_joy", 0, 2000), 5);//joy seed
                            shop.Items.Add(new MapItem("held_goggle_specs", 0, 3000), 10);//goggle specs
                            shop.Items.Add(new MapItem("held_shell_bell", 0, 3000), 10);//shell bell
                            shop.Items.Add(new MapItem("held_x_ray_specs", 0, 4000), 10);//x-ray specs
                            shop.Items.Add(new MapItem("held_heal_ribbon", 0, 4000), 10);//heal ribbon
                            shop.Items.Add(new MapItem("held_wide_lens", 0, 4500), 10);//wide lens

                            foreach (string key in IterateTypeBoosters())
                                shop.Items.Add(new MapItem(key, 0, 2000), 5);//type items

                            shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                            // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 103 screech : 86 thunder wave
                            shop.StartMob = GetShopMob("kecleon", "color_change", "synchronoise", "bind", "screech", "thunder_wave", new string[] { "xcl_family_kecleon_00", "xcl_family_kecleon_01", "xcl_family_kecleon_04" }, 0);
                            {
                                // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 103 screech : 86 thunder wave
                                shop.Mobs.Add(GetShopMob("kecleon", "color_change", "synchronoise", "bind", "screech", "thunder_wave", new string[] { "xcl_family_kecleon_00", "xcl_family_kecleon_01", "xcl_family_kecleon_04" }, -1), 10);
                                // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 50 disable : 374 fling
                                shop.Mobs.Add(GetShopMob("kecleon", "color_change", "synchronoise", "bind", "disable", "fling", new string[] { "xcl_family_kecleon_00", "xcl_family_kecleon_01", "xcl_family_kecleon_04" }, -1), 10);
                                // 352 Kecleon : 168 protean : 425 shadow sneak : 246 ancient power : 510 incinerate : 168 thief
                                shop.Mobs.Add(GetShopMob("kecleon", "protean", "shadow_sneak", "ancient_power", "incinerate", "thief", new string[] { "xcl_family_kecleon_00", "xcl_family_kecleon_01", "xcl_family_kecleon_04" }, -1, "shuckle"), 10);
                                // 352 Kecleon : 168 protean : 332 aerial ace : 421 shadow claw : 60 psybeam : 364 feint
                                shop.Mobs.Add(GetShopMob("kecleon", "protean", "aerial_ace", "shadow_claw", "psybeam", "feint", new string[] { "xcl_family_kecleon_00", "xcl_family_kecleon_01", "xcl_family_kecleon_04" }, -1, "shuckle"), 10);
                            }

                            shopZoneSpawns.Add(shop, new IntRange(0, max_floors), 10);
                        }
                        {
                            ShopStep<ListMapGenContext> shop = new ShopStep<ListMapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                            shop.Personality = 2;
                            shop.SecurityStatus = "shop_security";

                            foreach (string tm_id in IterateTMs(TMClass.Top | TMClass.ShopOnly))
                                shop.Items.Add(new MapItem(tm_id, 0, getTMPrice(tm_id)), 2);//TMs

                            shop.ItemThemes.Add(new ItemThemeType(ItemData.UseType.Learn, false, true, new RandRange(3, 6)), 10);//TMs

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

                            shopZoneSpawns.Add(shop, new IntRange(0, max_floors), 10);
                        }
                        SpreadStepRangeZoneStep shopZoneStep = new SpreadStepRangeZoneStep(new SpreadPlanQuota(new RandBinomial(2, 70), new IntRange(2, max_floors)), PR_SHOPS, shopZoneSpawns);
                        shopZoneStep.ModStates.Add(new FlagType(typeof(ShopModGenState)));
                        floorSegment.ZoneSteps.Add(shopZoneStep);



                        //switch vaults
                        {
                            SpreadVaultZoneStep vaultChanceZoneStep = new SpreadVaultZoneStep(PR_SPAWN_ITEMS_EXTRA, PR_SPAWN_TRAPS, PR_SPAWN_MOBS_EXTRA, new SpreadPlanQuota(new RandDecay(1, 2, 50), new IntRange(0, max_floors)));

                            //making room for the vault
                            {
                                ResizeFloorStep<ListMapGenContext> addSizeStep = new ResizeFloorStep<ListMapGenContext>(new Loc(16, 16), Dir8.None);
                                vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT, addSizeStep));
                                ClampFloorStep<ListMapGenContext> limitStep = new ClampFloorStep<ListMapGenContext>(new Loc(0), new Loc(78, 54));
                                vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT, limitStep));
                                ClampFloorStep<ListMapGenContext> clampStep = new ClampFloorStep<ListMapGenContext>();
                                vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT_CLAMP, clampStep));
                            }

                            // room addition step
                            {
                                SpawnList<RoomGen<ListMapGenContext>> detourRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                                detourRooms.Add(new RoomGenCross<ListMapGenContext>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
                                SpawnList<PermissiveRoomGen<ListMapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                                detourHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                                AddConnectedRoomsRandStep<ListMapGenContext> detours = new AddConnectedRoomsRandStep<ListMapGenContext>(detourRooms, detourHalls);
                                detours.Amount = new RandRange(4, 8);
                                detours.HallPercent = 100;
                                detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom(), new UnVaultableRoom()));
                                detours.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.SwitchVault));
                                detours.RoomComponents.Set(new NoConnectRoom());
                                detours.RoomComponents.Set(new NoEventRoom());
                                detours.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.SwitchVault));
                                detours.HallComponents.Set(new NoConnectRoom());
                                detours.HallComponents.Set(new NoEventRoom());

                                vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_GEN_EXTRA, detours));
                            }

                            //sealing the vault
                            {
                                SwitchSealStep<ListMapGenContext> vaultStep = new SwitchSealStep<ListMapGenContext>("sealed_block", "tile_switch", 1, true, true);
                                vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                                vaultStep.SwitchFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                                vaultStep.SwitchFilters.Add(new RoomFilterComponent(true, new BossRoom()));
                                vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_TILES_GEN_EXTRA, vaultStep));
                            }

                            //items for the vault
                            {
                                foreach (string key in IterateGummis())
                                    vaultChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 200);//gummis
                                vaultChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear", 1), new IntRange(0, max_floors), 2000);//amber tear
                                vaultChanceZoneStep.Items.Add(new MapItem("seed_reviver"), new IntRange(0, max_floors), 200);//reviver seed
                                vaultChanceZoneStep.Items.Add(new MapItem("seed_joy"), new IntRange(0, max_floors), 200);//joy seed
                                vaultChanceZoneStep.Items.Add(new MapItem("orb_itemizer"), new IntRange(0, max_floors), 50);//itemizer orb
                                vaultChanceZoneStep.Items.Add(new MapItem("wand_transfer", 3), new IntRange(0, max_floors), 50);//transfer wand
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
                                    boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_light", new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(1), new RandRange(1))), 30);
                                }

                                //445      ***    Heavy Box - 2* items
                                {
                                    boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_heavy", new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(2), new RandRange(1))), 10);
                                }

                                //446      ***    Nifty Box - all high tier TMs, ability capsule, heart scale 9, max potion, full heal, max elixir
                                {
                                    SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                                    //TMs
                                    foreach (string key in IterateTMs(TMClass.Mid | TMClass.Top))
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

                                    boxTreasure.Add(new MapItem("food_apple_huge"), 10);//huge apple
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

                                MultiStepSpawner<ListMapGenContext, MapItem> boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(2, 4)));

                                //MultiStepSpawner <- PresetMultiRand
                                MultiStepSpawner<ListMapGenContext, MapItem> mainSpawner = new MultiStepSpawner<ListMapGenContext, MapItem>();
                                mainSpawner.Picker = new PresetMultiRand<IStepSpawner<ListMapGenContext, MapItem>>(treasurePicker, boxPicker);
                                vaultChanceZoneStep.ItemSpawners.SetRange(mainSpawner, new IntRange(0, max_floors));
                            }
                            vaultChanceZoneStep.ItemAmount.SetRange(new RandRange(3, 5), new IntRange(0, max_floors));

                            // item placements for the vault
                            {
                                RandomRoomSpawnStep<ListMapGenContext, MapItem> detourItems = new RandomRoomSpawnStep<ListMapGenContext, MapItem>();
                                detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                                vaultChanceZoneStep.ItemPlacements.SetRange(detourItems, new IntRange(0, max_floors));
                            }

                            // mobs
                            // Vault FOES
                            {
                                //234 !! Stantler : 43 Leer : 95 Hypnosis : 36 Take Down : 109 Confuse Ray
                                vaultChanceZoneStep.Mobs.Add(GetFOEMob("stantler", "", "leer", "hypnosis", "take_down", "confuse_ray", zone.Level + 5, 1, 2), new IntRange(0, max_floors), 10);

                                //64//479 Rotom : 86 Thunder Wave : 271 Trick : 435 Discharge : 164 Substitute
                                vaultChanceZoneStep.Mobs.Add(GetFOEMob("rotom", "", "thunder_wave", "trick", "discharge", "substitute", zone.Level + 5, 1, 2), new IntRange(0, max_floors), 10);

                                //115 Kangaskhan : 113 Scrappy : 146 Dizzy Punch : 004 Comet Punch : 99 Rage : 203 Endure
                                vaultChanceZoneStep.Mobs.Add(GetFOEMob("kangaskhan", "scrappy", "dizzy_punch", "comet_punch", "rage", "endure", zone.Level + 5, 1, 2), new IntRange(0, max_floors), 5);

                                //24//20 Raticate : 228 Pursuit : 162 Super Fang : 372 Assurance
                                vaultChanceZoneStep.Mobs.Add(GetFOEMob("raticate", "", "pursuit", "super_fang", "assurance", "", zone.Level + 5, 1, 2), new IntRange(0, max_floors), 10);
                            }
                            vaultChanceZoneStep.MobAmount.SetRange(new RandRange(7, 11), new IntRange(0, max_floors));

                            // mob placements
                            {
                                PlaceRandomMobsStep<ListMapGenContext> secretMobPlacement = new PlaceRandomMobsStep<ListMapGenContext>();
                                secretMobPlacement.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                                secretMobPlacement.ClumpFactor = 20;
                                vaultChanceZoneStep.MobPlacements.SetRange(secretMobPlacement, new IntRange(0, max_floors));
                            }

                            floorSegment.ZoneSteps.Add(vaultChanceZoneStep);
                        }



                        AddHiddenStairStep(floorSegment, new SpreadPlanQuota(new RandRange(1), new IntRange(0, max_floors - 1)), 1);

                        AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 2);

                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            RoomFloorGen layout = new RoomFloorGen();

                            //Floor settings
                            AddFloorData(layout, "B10. Thunderstruck Pass.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                            //Tilesets
                            if (ii < 5)
                                AddSpecificTextureData(layout, "mt_bristle_wall", "mt_bristle_floor", "mt_bristle_secondary", "tall_grass_yellow", "electric");
                            else if (ii < 10)
                                AddSpecificTextureData(layout, "mt_thunder_peak_wall", "mt_thunder_peak_floor", "mt_thunder_peak_secondary", "tall_grass_yellow", "electric");
                            else
                                AddSpecificTextureData(layout, "mt_travail_wall", "mt_travail_floor", "mt_travail_secondary", "tall_grass_dark", "electric");

                            if (ii >= 5)
                                AddGrassSteps(layout, new RandRange(3, 7), new IntRange(2, 7), new RandRange(30));

                            //traps
                            AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                            AddTrapsSteps(layout, new RandRange(10, 14));

                            //money
                            AddMoneyData(layout, new RandRange(3, 7));

                            //enemies
                            AddRespawnData(layout, 11, 110);
                            AddEnemySpawnData(layout, 20, new RandRange(6, 9));

                            //items
                            AddItemData(layout, new RandRange(3, 7), 25);

                            //construct paths
                            {
                                AddInitListStep(layout, 50, 40);

                                FloorPathBranch<ListMapGenContext> path = new FloorPathBranch<ListMapGenContext>();

                                path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                path.FillPercent = new RandRange(40);
                                path.BranchRatio = new RandRange(80);
                                path.HallPercent = 100;

                                SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                                //cross
                                if (ii >= 5)
                                    genericRooms.Add(new RoomGenCross<ListMapGenContext>(new RandRange(5, 9), new RandRange(5, 9), new RandRange(2, 5), new RandRange(2, 5)), 4);
                                //diamond
                                genericRooms.Add(new RoomGenDiamond<ListMapGenContext>(new RandRange(5), new RandRange(5)), 8);
                                genericRooms.Add(new RoomGenDiamond<ListMapGenContext>(new RandRange(9), new RandRange(9)), 4);
                                genericRooms.Add(new RoomGenDiamond<ListMapGenContext>(new RandRange(15), new RandRange(15)), 2);
                                path.GenericRooms = genericRooms;

                                SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                                if (ii < 5)
                                    genericHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(1), new RandRange(1)), 10);
                                else
                                {
                                    genericHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(1, 4), new RandRange(1)), 10);
                                    genericHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(1), new RandRange(1, 4)), 10);
                                }
                                path.GenericHalls = genericHalls;

                                layout.GenSteps.Add(PR_ROOMS_GEN, path);


                                AddTunnelStep<ListMapGenContext> tunneler = new AddTunnelStep<ListMapGenContext>();
                                if (ii < 10)
                                    tunneler.Halls = new RandRange(12, 18);
                                else
                                    tunneler.Halls = new RandRange(16, 22);

                                tunneler.TurnLength = new RandRange(2, 4);
                                tunneler.MaxLength = new RandRange(2, 6);
                                tunneler.AllowDeadEnd = true;
                                layout.GenSteps.Add(PR_TILES_GEN_TUNNEL, tunneler);

                            }

                            AddDrawListSteps(layout);

                            AddStairStep(layout, false);

                            layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<ListMapGenContext, MapGenEntrance, MapGenExit>());

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }

                    {
                        SingularSegment structure = new SingularSegment(-1);

                        SpawnList<TeamMemberSpawn> enemyList = new SpawnList<TeamMemberSpawn>();
                        structure.BaseFloor = getSecretRoom(translate, "special_grass_maze", -1, "mt_bristle_wall", "mt_bristle_floor", "mt_bristle_secondary", "tall_grass_yellow", "electric", enemyList, new Loc(5, 11));

                        zone.Segments.Add(structure);
                    }


                    {
                        SingularSegment structure = new SingularSegment(-1);

                        ChanceFloorGen multiGen = new ChanceFloorGen();
                        multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Intermediate, "small_square", -2, false, false), 10);
                        multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Intermediate, "tall_hall", -2, false, false), 10);
                        multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Intermediate, "wide_hall", -2, false, false), 10);
                        structure.BaseFloor = multiGen;

                        zone.Segments.Add(structure);
                    }
                }
                #endregion
            }
            else if (index == 6)
            {
                FillVeiledRidge(zone, translate);
            }
            else if (index == 7)
            {
                #region CHAMPION'S ROAD
                zone.Name = new LocalText("Champion's Road");
                zone.Rescues = 2;
                zone.Level = 50;
                zone.ExpPercent = 0;
                zone.Rogue = RogueStatus.NoTransfer;
                {
                    int max_floors = 23;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Champion's Road\n{0}F"), new Priority(-15)));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 10);
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);
                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;


                    //necessities
                    CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                    necessities.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("necessities", necessities);


                    necessities.Spawns.Add(new InvItem("berry_leppa", true), new IntRange(0, max_floors), 3);
                    necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 6);
                    necessities.Spawns.Add(new InvItem("berry_oran", true), new IntRange(0, max_floors), 2);
                    necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 4);
                    necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 10);
                    necessities.Spawns.Add(new InvItem("berry_sitrus"), new IntRange(0, max_floors), 6);
                    necessities.Spawns.Add(new InvItem("food_grimy"), new IntRange(0, max_floors), 8);

                    //snacks
                    CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                    snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                    snacks.Spawns.Add(new InvItem("herb_power", true), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("herb_power"), new IntRange(0, max_floors), 8);
                    snacks.Spawns.Add(new InvItem("seed_last_chance", true), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("seed_last_chance"), new IntRange(0, max_floors), 8);
                    snacks.Spawns.Add(new InvItem("seed_blast", true), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, max_floors), 8);
                    snacks.Spawns.Add(new InvItem("berry_babiri", true), new IntRange(0, max_floors), 0);
                    snacks.Spawns.Add(new InvItem("berry_babiri"), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("berry_chople", true), new IntRange(0, max_floors), 0);
                    snacks.Spawns.Add(new InvItem("berry_chople"), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("berry_haban", true), new IntRange(0, max_floors), 0);
                    snacks.Spawns.Add(new InvItem("berry_haban"), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("berry_kasib", true), new IntRange(0, max_floors), 0);
                    snacks.Spawns.Add(new InvItem("berry_kasib"), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("berry_occa", true), new IntRange(0, max_floors), 0);
                    snacks.Spawns.Add(new InvItem("berry_occa"), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("berry_roseli", true), new IntRange(0, max_floors), 0);
                    snacks.Spawns.Add(new InvItem("berry_roseli"), new IntRange(0, max_floors), 2);
                    //boosters
                    CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                    boosters.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("boosters", boosters);


                    boosters.Spawns.Add(new InvItem("gummi_blue"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_black"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_clear"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_grass"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_green"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_brown"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_orange"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_gold"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_red"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_royal"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_silver"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_white"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_yellow"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_sky"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_gray"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_magenta"), new IntRange(0, max_floors), 1);
                    //special
                    CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                    special.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("special", special);


                    special.Spawns.Add(new InvItem("machine_ability_capsule", true), new IntRange(0, max_floors), 2);
                    special.Spawns.Add(new InvItem("machine_ability_capsule"), new IntRange(0, max_floors), 4);
                    special.Spawns.Add(new InvItem("machine_recall_box", true), new IntRange(0, max_floors), 3);
                    special.Spawns.Add(new InvItem("machine_recall_box"), new IntRange(0, max_floors), 7);
                    special.Spawns.Add(new InvItem("key", true, 1), new IntRange(0, max_floors), 3);
                    special.Spawns.Add(new InvItem("key", false, 1), new IntRange(0, max_floors), 7);
                    //evo
                    CategorySpawn<InvItem> evo = new CategorySpawn<InvItem>();
                    evo.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("evo", evo);


                    evo.Spawns.Add(new InvItem("evo_link_cable"), new IntRange(0, max_floors), 10);
                    //throwable
                    CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                    throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                    throwable.Spawns.Add(new InvItem("ammo_iron_thorn", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("ammo_rare_fossil", false, 2), new IntRange(0, max_floors), 5);
                    throwable.Spawns.Add(new InvItem("wand_path", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_fear", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_slow", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_warp", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_lob", false, 3), new IntRange(0, max_floors), 10);
                    //orbs
                    CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                    orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                    orbs.Spawns.Add(new InvItem("orb_foe_seal", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_foe_seal"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_devolve", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_devolve"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_mobile", true), new IntRange(0, max_floors), 4);
                    orbs.Spawns.Add(new InvItem("orb_mobile"), new IntRange(0, max_floors), 16);
                    orbs.Spawns.Add(new InvItem("orb_endure", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_endure"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_cleanse"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_spurn", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_spurn"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_trapbust", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_trapbust"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_pierce", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_pierce"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_one_shot", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_one_shot"), new IntRange(0, max_floors), 8);
                    //held
                    CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                    held.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("held", held);


                    held.Spawns.Add(new InvItem("held_choice_band"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_choice_scarf"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_choice_specs"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_life_orb"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_scope_lens", true), new IntRange(0, max_floors), 5);
                    held.Spawns.Add(new InvItem("held_scope_lens"), new IntRange(0, max_floors), 5);
                    held.Spawns.Add(new InvItem("held_wide_lens", true), new IntRange(0, max_floors), 5);
                    held.Spawns.Add(new InvItem("held_wide_lens"), new IntRange(0, max_floors), 5);
                    held.Spawns.Add(new InvItem("held_heal_ribbon", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_heal_ribbon"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_x_ray_specs", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_x_ray_specs"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_mobile_scarf", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_mobile_scarf"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_ring_target", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_ring_target"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_pink_bow", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_pink_bow"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_charcoal", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_charcoal"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_poison_barb", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_poison_barb"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_metal_coat", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_metal_coat"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_hard_stone", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_hard_stone"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_dragon_scale", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_dragon_scale"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_pixie_plate"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_flame_plate"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_iron_plate"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_stone_plate"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_draco_plate"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_toxic_plate"), new IntRange(0, max_floors), 10);
                    //tms
                    CategorySpawn<InvItem> tms = new CategorySpawn<InvItem>();
                    tms.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("tms", tms);


                    tms.Spawns.Add(new InvItem("tm_explosion"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_snatch"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_sunny_day"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_sandstorm"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_x_scissor"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_wild_charge"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_focus_punch"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_safeguard"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_light_screen"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_psyshock"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_dream_eater"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_nature_power"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_facade"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_rock_slide"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_fling"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_thunderbolt"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_water_pulse"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_shock_wave"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_brick_break"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_payback"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_charge_beam"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_flamethrower"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_energy_ball"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_retaliate"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_scald"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_waterfall"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_roost"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_rock_polish"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_acrobatics"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_rock_climb"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_bulk_up"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_pluck"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_psych_up"), new IntRange(0, max_floors), 10);


                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;


                    poolSpawn.Spawns.Add(GetTeamMob("ursaring", "", "hammer_arm", "thrash", "", "", new RandRange(38), "wander_normal", false, true), new IntRange(0, 4), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("skarmory", "", "steel_wing", "spikes", "", "", new RandRange(38), "wander_normal", false, true), new IntRange(0, 4), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("staraptor", "", "close_combat", "brave_bird", "", "", new RandRange(38), TeamMemberSpawn.MemberRole.Leader, "wander_normal", false, true), new IntRange(0, 4), 10);
                    
                    //sleeping
                    {
                        TeamMemberSpawn mob = GetTeamMob("mamoswine", "", "earthquake", "icicle_crash", "", "", new RandRange(45), TeamMemberSpawn.MemberRole.Loner, "wander_normal", true, true);
                        HashSet<string> exceptFor = new HashSet<string>();
                        foreach (string legend in IterateLegendaries())
                            exceptFor.Add(legend);
                        MobSpawnExclAny itemSpawn = new MobSpawnExclAny("box_light", exceptFor, new IntRange(1), true);
                        mob.Spawn.SpawnFeatures.Add(itemSpawn);
                        poolSpawn.Spawns.Add(mob, new IntRange(0, 6), 5);
                    }

                    poolSpawn.Spawns.Add(GetTeamMob("heracross", "", "megahorn", "brick_break", "", "", new RandRange(38), "wander_normal", false, true), new IntRange(0, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("absol", "super_luck", "night_slash", "swords_dance", "", "", new RandRange(38), "wander_normal", false, true), new IntRange(2, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("florges", "", "petal_blizzard", "flower_shield", "", "", new RandRange(38), "wander_normal", false, true), new IntRange(2, 8), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("machamp", "no_guard", "dynamic_punch", "", "", "", new RandRange(39), "wander_normal", false, true), new IntRange(2, 8), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("golem", "", "stone_edge", "rock_blast", "", "", new RandRange(39), "wander_normal", false, true), new IntRange(4, 8), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("swalot", "", "sludge_bomb", "encore", "", "", new RandRange(39), "wander_normal", false, true), new IntRange(4, 8), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("drapion", "", "poison_jab", "sucker_punch", "", "", new RandRange(39), "wander_normal", false, true), new IntRange(6, 12), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("magnezone", "", "discharge", "magnet_bomb", "", "", new RandRange(38), "wander_normal", false, true), new IntRange(4, 10), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("flygon", "", "earthquake", "dragon_tail", "", "", new RandRange(40), TeamMemberSpawn.MemberRole.Loner, "wander_normal", false, true), new IntRange(8, 12), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("mismagius", "", "shadow_ball", "mystical_fire", "", "", new RandRange(40), "wander_normal", false, true), new IntRange(8, 14), 10);

                    //sleeping
                    {
                        TeamMemberSpawn mob = GetTeamMob("exploud", "", "hyper_voice", "sleep_talk", "rest", "boomburst", new RandRange(42), TeamMemberSpawn.MemberRole.Loner, "wander_normal", true, true);
                        HashSet<string> exceptFor = new HashSet<string>();
                        foreach (string legend in IterateLegendaries())
                            exceptFor.Add(legend);
                        MobSpawnExclAny itemSpawn = new MobSpawnExclAny("box_light", exceptFor, new IntRange(1), true);
                        mob.Spawn.SpawnFeatures.Add(itemSpawn);
                        poolSpawn.Spawns.Add(mob, new IntRange(10, 14), 5);
                    }

                    poolSpawn.Spawns.Add(GetTeamMob("ledian", "", "reflect", "light_screen", "silver_wind", "", new RandRange(40), "wander_normal", false, true), new IntRange(10, 16), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("starmie", "", "hydro_pump", "recover", "swift", "", new RandRange(40), "wander_normal", false, true), new IntRange(12, 16), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("gallade", "", "wide_guard", "psycho_cut", "teleport", "swords_dance", new RandRange(39), "retreater", false, true), new IntRange(12, 18), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("gardevoir", "", "psychic", "wish", "moonblast", "", new RandRange(39), "wander_normal", false, true), new IntRange(12, 18), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("steelix", "", "iron_tail", "double_edge", "", "", new RandRange(39), "wander_normal", false, true), new IntRange(12, 18), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("sunflora", "", "solar_beam", "ingrain", "", "", new RandRange(39), "wander_normal", false, true), new IntRange(8, 12), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("azumarill", "", "play_rough", "aqua_ring", "", "", new RandRange(40), "wander_normal", false, true), new IntRange(14, 20), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("electivire", "", "thunderbolt", "low_kick", "", "", new RandRange(40), "wander_normal", false, true), new IntRange(16, 20), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("magmortar", "", "fire_blast", "", "", "", new RandRange(40), "wander_normal", false, true), new IntRange(16, 20), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("raichu", "lightning_rod", "thunder", "", "", "", new RandRange(41), "wander_normal", false, true), new IntRange(16, 22), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("clefable", "", "cosmic_power", "moonblast", "", "", new RandRange(41), "wander_normal", false, true), new IntRange(18, 22), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("eiscue", "", "blizzard", "surf", "", "", new RandRange(42), "wander_normal", false, true), new IntRange(18, 22), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("drifblim", "aftermath", "ominous_wind", "explosion", "", "", new RandRange(41), "wander_normal", false, true), new IntRange(18, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("noivern", "", "boomburst", "dragon_pulse", "", "", new RandRange(41), "wander_normal", false, true), new IntRange(20, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("scizor", "swarm", "focus_energy", "bullet_punch", "x_scissor", "", new RandRange(41), "wander_normal", false, true), new IntRange(20, max_floors), 10);
                    
                    //sleeping
                    {
                        TeamMemberSpawn mob = GetTeamMob("rhyperior", "solid_rock", "drill_run", "rock_wrecker", "", "", new RandRange(45), TeamMemberSpawn.MemberRole.Loner, "wander_normal", true, true);
                        HashSet<string> exceptFor = new HashSet<string>();
                        foreach (string legend in IterateLegendaries())
                            exceptFor.Add(legend);
                        MobSpawnExclAny itemSpawn = new MobSpawnExclAny("box_heavy", exceptFor, new IntRange(2), true);
                        mob.Spawn.SpawnFeatures.Add(itemSpawn);
                        poolSpawn.Spawns.Add(mob, new IntRange(20, max_floors), 5);
                    }

                    //show up in alternating floors
                    poolSpawn.Spawns.Add(GetTeamMob("serperior", "", "grass_pledge", "leaf_blade", "", "", new RandRange(42), "wander_normal", false, true), new IntRange(10), 5);
                    poolSpawn.Spawns.Add(GetTeamMob("serperior", "", "grass_pledge", "leaf_blade", "", "", new RandRange(42), "wander_normal", false, true), new IntRange(13), 5);
                    poolSpawn.Spawns.Add(GetTeamMob("serperior", "", "grass_pledge", "leaf_blade", "", "", new RandRange(42), "wander_normal", false, true), new IntRange(16), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("serperior", "", "grass_pledge", "leaf_blade", "", "", new RandRange(42), "wander_normal", false, true), new IntRange(19, max_floors), 10);
                    //until floor 19+
                    poolSpawn.Spawns.Add(GetTeamMob("emboar", "", "fire_pledge", "heat_crash", "", "", new RandRange(42), "wander_normal", false, true), new IntRange(11), 5);
                    poolSpawn.Spawns.Add(GetTeamMob("emboar", "", "fire_pledge", "heat_crash", "", "", new RandRange(42), "wander_normal", false, true), new IntRange(14), 5);
                    poolSpawn.Spawns.Add(GetTeamMob("emboar", "", "fire_pledge", "heat_crash", "", "", new RandRange(42), "wander_normal", false, true), new IntRange(17), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("emboar", "", "fire_pledge", "heat_crash", "", "", new RandRange(42), "wander_normal", false, true), new IntRange(19, max_floors), 10);
                    //when they all show up
                    poolSpawn.Spawns.Add(GetTeamMob("samurott", "", "water_pledge", "razor_shell", "", "", new RandRange(42), "wander_normal", false, true), new IntRange(12), 5);
                    poolSpawn.Spawns.Add(GetTeamMob("samurott", "", "water_pledge", "razor_shell", "", "", new RandRange(42), "wander_normal", false, true), new IntRange(15), 5);
                    poolSpawn.Spawns.Add(GetTeamMob("samurott", "", "water_pledge", "razor_shell", "", "", new RandRange(42), "wander_normal", false, true), new IntRange(18), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("samurott", "", "water_pledge", "razor_shell", "", "", new RandRange(42), "wander_normal", false, true), new IntRange(19, max_floors), 10);


                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 6);
                    poolSpawn.TeamSizes.Add(3, new IntRange(15, max_floors), 3);


                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;

                    tileSpawn.Spawns.Add(new EffectTile("trap_slumber", false), new IntRange(0, max_floors), 10);//sleep trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_hunger", true), new IntRange(0, max_floors), 10);//hunger trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_sticky", false), new IntRange(0, max_floors), 10);//sticky trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_spin", false), new IntRange(0, max_floors), 10);//spin trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_grimy", false), new IntRange(0, max_floors), 10);//grimy trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_poison", false), new IntRange(0, max_floors), 10);//poison trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_trigger", true), new IntRange(0, max_floors), 20);//trigger trap

                    tileSpawn.Spawns.Add(new EffectTile("trap_mud", false), new IntRange(0, max_floors), 10);//mud trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_self_destruct", false), new IntRange(0, max_floors), 10);//selfdestruct trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_pp_leech", true), new IntRange(0, max_floors), 10);//pp-leech trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_grudge", true), new IntRange(0, max_floors), 10);//grudge trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_summon", true), new IntRange(0, max_floors), 10);//summon trap

                    floorSegment.ZoneSteps.Add(tileSpawn);


                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(6, 10), new IntRange(0, max_floors)), new MapItem("food_apple"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("berry_oran"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(3, 6), new IntRange(0, max_floors)), new MapItem("berry_leppa"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(10, max_floors)), new MapItem("berry_leppa"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(5, max_floors - 1), new IntRange(0, max_floors)), new MapItem("orb_cleanse"));


                    {
                        //monster houses
                        SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(10, new IntRange(0, max_floors)));
                        monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHouseStep<ListMapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                        foreach (string gummi in IterateGummis())
                            monsterChanceZoneStep.Items.Add(new MapItem(gummi), new IntRange(0, max_floors), 4);//gummis
                        foreach (string key in IterateTMs(TMClass.Natural))
                            monsterChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 1);//TMs

                        monsterChanceZoneStep.Items.Add(new MapItem("ammo_silver_spike", 6), new IntRange(0, max_floors), 30);//silver spike
                        monsterChanceZoneStep.Items.Add(new MapItem("loot_nugget"), new IntRange(0, max_floors), 10);//nugget
                        monsterChanceZoneStep.Items.Add(new MapItem("food_banana"), new IntRange(0, max_floors), 50);//banana
                        monsterChanceZoneStep.Items.Add(new MapItem("food_apple_big"), new IntRange(0, max_floors), 50);//big apple
                        monsterChanceZoneStep.Items.Add(new MapItem("food_banana_big"), new IntRange(0, max_floors), 10);//big banana
                        monsterChanceZoneStep.Items.Add(new MapItem("loot_heart_scale", 2), new IntRange(0, max_floors), 10);//heart scale
                        monsterChanceZoneStep.Items.Add(new MapItem("key", 1), new IntRange(0, max_floors), 10);//key
                        monsterChanceZoneStep.Items.Add(new MapItem("machine_recall_box"), new IntRange(0, max_floors), 10);//link box
                        monsterChanceZoneStep.Items.Add(new MapItem("machine_ability_capsule"), new IntRange(0, max_floors), 10);//ability capsule

                        monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(1, 4), "loot_pearl"), new ItemThemeNone(40, new RandRange(2, 4))), new IntRange(0, max_floors), 20);//no theme
                                                                                                                                                                                                                                    //monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMoney(500, new ParamRange(5, 11)), new ParamRange(0, 30));
                        monsterChanceZoneStep.ItemThemes.Add(new ItemThemeType(ItemData.UseType.Learn, true, true, new RandRange(3, 5)), new IntRange(0, max_floors), 15);//TMs
                        monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(3, 7)), new IntRange(0, max_floors), 30);//gummis
                        monsterChanceZoneStep.MobThemes.Add(new MobThemeNone(40, new RandRange(7, 13)), new IntRange(0, max_floors), 10);
                        floorSegment.ZoneSteps.Add(monsterChanceZoneStep);
                    }


                    {
                        SpreadHouseZoneStep chestChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanQuota(new RandDecay(1, 8, 60), new IntRange(4, max_floors)));
                        chestChanceZoneStep.ModStates.Add(new FlagType(typeof(ChestModGenState)));
                        chestChanceZoneStep.HouseStepSpawns.Add(new ChestStep<ListMapGenContext>(false, GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);

                        foreach (string key in IterateVitamins())
                            chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//boosters
                        chestChanceZoneStep.Items.Add(new MapItem("apricorn_big"), new IntRange(0, max_floors), 10);//big apricorn
                        chestChanceZoneStep.Items.Add(new MapItem("medicine_elixir"), new IntRange(0, max_floors), 20);//elixir
                        chestChanceZoneStep.Items.Add(new MapItem("medicine_potion"), new IntRange(0, max_floors), 20);//potion
                        chestChanceZoneStep.Items.Add(new MapItem("medicine_max_elixir"), new IntRange(0, max_floors), 20);//max elixir
                        chestChanceZoneStep.Items.Add(new MapItem("medicine_max_potion"), new IntRange(0, max_floors), 20);//max potion
                        chestChanceZoneStep.Items.Add(new MapItem("medicine_full_heal"), new IntRange(0, max_floors), 20);//full heal
                        foreach (string key in IterateXItems())
                            chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 10);//X-Items
                        chestChanceZoneStep.Items.Add(new MapItem("loot_nugget"), new IntRange(0, max_floors), 20);//nugget
                        chestChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear", 1), new IntRange(0, max_floors), 20);//amber tear
                        chestChanceZoneStep.Items.Add(new MapItem("seed_joy"), new IntRange(0, max_floors), 15);//joy seed
                        chestChanceZoneStep.Items.Add(new MapItem("machine_ability_capsule"), new IntRange(0, max_floors), 15);//ability capsule

                        chestChanceZoneStep.ItemThemes.Add(new ItemThemeNone(100, new RandRange(1, 3)), new IntRange(0, max_floors), 30);

                        floorSegment.ZoneSteps.Add(chestChanceZoneStep);
                    }

                    {
                        SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(10, new IntRange(6, max_floors)));
                        monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHallStep<ListMapGenContext>(new Loc(11, 9), GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                        monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHallStep<ListMapGenContext>(new Loc(15, 13), GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);

                        foreach (string key in IterateGummis())
                            monsterChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//gummis
                        foreach (string key in IterateApricorns())
                            monsterChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//apricorns
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_thunder_stone"), new IntRange(0, 30), 4);//Thunder Stone
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_leaf_stone"), new IntRange(0, 30), 4);//Leaf Stone
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_ice_stone"), new IntRange(0, 30), 4);//Ice Stone
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_fire_stone"), new IntRange(0, max_floors), 4);//Fire Stone
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_water_stone"), new IntRange(0, max_floors), 4);//Water Stone
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_moon_stone"), new IntRange(0, max_floors), 4);//Moon Stone
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_leaf_stone"), new IntRange(0, max_floors), 4);//Leaf Stone
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_kings_rock"), new IntRange(0, max_floors), 4);//King's Rock
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_link_cable"), new IntRange(0, max_floors), 4);//Link Cable
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_sun_stone"), new IntRange(0, max_floors), 4);//Sun Stone
                        monsterChanceZoneStep.Items.Add(new MapItem("food_banana"), new IntRange(0, max_floors), 25);//banana
                        foreach (string tm_id in IterateDistroTMs(TMDistroClass.Ordinary))
                            monsterChanceZoneStep.Items.Add(new MapItem(tm_id), new IntRange(0, max_floors), 2);//TMs
                        monsterChanceZoneStep.Items.Add(new MapItem("loot_nugget"), new IntRange(0, max_floors), 10);//nugget
                        monsterChanceZoneStep.Items.Add(new MapItem("loot_pearl", 1), new IntRange(0, max_floors), 10);//pearl
                        monsterChanceZoneStep.Items.Add(new MapItem("loot_heart_scale", 2), new IntRange(0, max_floors), 10);//heart scale
                        monsterChanceZoneStep.Items.Add(new MapItem("key", 1), new IntRange(0, max_floors), 10);//key
                        monsterChanceZoneStep.Items.Add(new MapItem("machine_recall_box"), new IntRange(0, max_floors), 30);//link box
                        monsterChanceZoneStep.Items.Add(new MapItem("machine_ability_capsule"), new IntRange(0, max_floors), 10);//ability capsule


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
                        monsterChanceZoneStep.Items.Add(new MapItem("held_choice_scarf"), new IntRange(0, max_floors), 1);//Choice Scarf
                        monsterChanceZoneStep.Items.Add(new MapItem("held_choice_specs"), new IntRange(0, max_floors), 1);//Choice Specs
                        monsterChanceZoneStep.Items.Add(new MapItem("held_choice_band"), new IntRange(0, max_floors), 1);//Choice Band
                        monsterChanceZoneStep.Items.Add(new MapItem("held_assault_vest"), new IntRange(0, max_floors), 1);//Assault Vest
                        monsterChanceZoneStep.Items.Add(new MapItem("held_life_orb"), new IntRange(0, max_floors), 1);//Life Orb
                        monsterChanceZoneStep.Items.Add(new MapItem("held_heal_ribbon"), new IntRange(0, max_floors), 1);//Heal Ribbon
                        monsterChanceZoneStep.Items.Add(new MapItem("held_goggle_specs"), new IntRange(0, max_floors), 1);//Goggle Specs

                        //monsterChanceZoneStep.ItemThemes.Add(new ItemThemeNone(100, new RandRange(5, 11)), new ParamRange(0, 30), 20);
                        monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(1, 4), "loot_pearl"), new ItemThemeNone(50, new RandRange(2, 4))), new IntRange(0, 30), 30);//no theme
                        monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeType(ItemData.UseType.Learn, false, true, new RandRange(3, 5)),
                            new ItemThemeRange(true, true, new RandRange(0, 2), ItemArray(IterateMachines()))), new IntRange(0, 30), 10);//TMs + machines

                        monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(3, 7)), new IntRange(0, 30), 30);//gummis
                        monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(1, 4), "loot_pearl"), new ItemStateType(new FlagType(typeof(EvoState)), true, true, new RandRange(2, 4))), new IntRange(0, 10), 20);//evo items
                        monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(1, 4), "loot_pearl"), new ItemStateType(new FlagType(typeof(EvoState)), true, true, new RandRange(2, 4))), new IntRange(10, 20), 10);//evo items
                        monsterChanceZoneStep.MobThemes.Add(new MobThemeNone(0, new RandRange(7, 13)), new IntRange(0, max_floors), 10);

                        floorSegment.ZoneSteps.Add(monsterChanceZoneStep);
                    }




                    SpreadRoomZoneStep caveZoneStep = new SpreadRoomZoneStep(PR_GRID_GEN_EXTRA, PR_ROOMS_GEN_EXTRA, new SpreadPlanQuota(new RandRange(1), new IntRange(19, max_floors - 1)));
                    List<BaseRoomFilter> caveFilters = new List<BaseRoomFilter>();
                    caveFilters.Add(new RoomFilterComponent(true, new ImmutableRoom()));
                    caveFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                    {

                        RoomGenGuardedCave<MapGenContext> guarded = new RoomGenGuardedCave<MapGenContext>();
                        //treasure
                        guarded.Treasures.RandomSpawns.Add(MapItem.CreateBox("box_deluxe", "xcl_family_bird_trio_09"), 10);
                        guarded.Treasures.RandomSpawns.Add(MapItem.CreateBox("box_deluxe", "xcl_family_bird_trio_10"), 10);
                        guarded.Treasures.RandomSpawns.Add(MapItem.CreateBox("box_deluxe", "xcl_family_ho_oh_beasts_08"), 10);
                        guarded.Treasures.RandomSpawns.Add(MapItem.CreateBox("box_deluxe", "xcl_family_ho_oh_beasts_09"), 10);
                        guarded.Treasures.RandomSpawns.Add(MapItem.CreateBox("box_deluxe", "xcl_family_ho_oh_beasts_12"), 10);
                        guarded.Treasures.RandomSpawns.Add(MapItem.CreateBox("box_deluxe", "xcl_family_ho_oh_beasts_13"), 10);
                        guarded.Treasures.RandomSpawns.Add(MapItem.CreateBox("box_deluxe", "xcl_family_ho_oh_beasts_16"), 10);
                        guarded.Treasures.RandomSpawns.Add(MapItem.CreateBox("box_deluxe", "xcl_family_ho_oh_beasts_17"), 10);
                        guarded.Treasures.RandomSpawns.Add(MapItem.CreateBox("box_deluxe", "xcl_family_regi_trio_06"), 10);
                        guarded.Treasures.RandomSpawns.Add(MapItem.CreateBox("box_deluxe", "xcl_family_regi_trio_07"), 10);
                        guarded.Treasures.RandomSpawns.Add(MapItem.CreateBox("box_deluxe", "xcl_family_regi_trio_10"), 10);
                        guarded.Treasures.RandomSpawns.Add(MapItem.CreateBox("box_deluxe", "xcl_family_regi_trio_11"), 10);
                        guarded.Treasures.RandomSpawns.Add(MapItem.CreateBox("box_deluxe", "xcl_family_regi_trio_14"), 10);
                        guarded.Treasures.RandomSpawns.Add(MapItem.CreateBox("box_deluxe", "xcl_family_regi_trio_15"), 10);
                        guarded.Treasures.RandomSpawns.Add(MapItem.CreateBox("box_deluxe", "xcl_family_lake_trio_06"), 10);
                        guarded.Treasures.RandomSpawns.Add(MapItem.CreateBox("box_deluxe", "xcl_family_lake_trio_07"), 10);
                        guarded.Treasures.SpawnAmount = 3;
                        //guard
                        MobSpawn spawner = GetGenericMob("dragonite", "multiscale", "outrage", "roost", "aqua_tail", "hurricane", new RandRange(47), "boss", true, true);
                        guarded.GuardTypes.Add(spawner, 10);

                        caveZoneStep.Spawns.Add(new RoomGenOption(guarded, new RoomGenDefault<ListMapGenContext>(), caveFilters), 10);
                    }
                    floorSegment.ZoneSteps.Add(caveZoneStep);

                    AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 10, 2);

                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        AddFloorData(layout, "B14. Champion Road.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                        //Tilesets
                        if (ii < 8)
                            AddSpecificTextureData(layout, "northwind_field_wall", "northwind_field_floor", "northwind_field_secondary", "tall_grass_dark", "fairy");
                        else if (ii < 16)
                            AddSpecificTextureData(layout, "craggy_peak_wall", "craggy_peak_floor", "craggy_peak_secondary", "tall_grass_dark", "steel");
                        else
                            AddSpecificTextureData(layout, "sky_ruins_area_wall", "sky_ruins_floor", "sky_ruins_secondary", "tall_grass_dark", "flying");


                        if (ii < 4)
                        {
                            AddTunnelStep<MapGenContext> tunneler = new AddTunnelStep<MapGenContext>();
                            tunneler.Halls = new RandRange(3, 7);
                            tunneler.TurnLength = new RandRange(3, 8);
                            tunneler.MaxLength = new RandRange(25);
                            tunneler.Brush = new TerrainHallBrush(Loc.One, new Tile("pit"));
                            layout.GenSteps.Add(PR_WATER, tunneler);
                        }
                        else
                            AddWaterSteps(layout, "pit", new RandRange(20));//pit

                        if (ii >= 8)
                            AddGrassSteps(layout, new RandRange(0, 6), new IntRange(3, 9), new RandRange(30));

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile


                        SpawnList<PatternPlan> patternList = new SpawnList<PatternPlan>();
                        patternList.Add(new PatternPlan("pattern_dither_fourth", PatternPlan.PatternExtend.Repeat2D), 5);
                        patternList.Add(new PatternPlan("pattern_squiggle", PatternPlan.PatternExtend.Repeat1D), 5);
                        patternList.Add(new PatternPlan("pattern_x_repeat", PatternPlan.PatternExtend.Repeat2D), 5);
                        AddTrapPatternSteps(layout, new RandRange(2, 4), patternList);

                        AddTrapsSteps(layout, new RandRange(10, 14));

                        //money
                        AddMoneyData(layout, new RandRange(4, 9));

                        //enemies
                        AddRespawnData(layout, 19, 120);
                        if (ii < 12)
                            AddEnemySpawnData(layout, 20, new RandRange(9, 12));
                        else if (ii < 20)
                            AddEnemySpawnData(layout, 20, new RandRange(11, 14));
                        else
                            AddEnemySpawnData(layout, 20, new RandRange(12, 16));

                        //items
                        AddItemData(layout, new RandRange(4, 7), 25);


                        //construct paths
                        if (ii < 8)
                        {
                            //Two Sides + Horiz Merge
                            AddInitGridStep(layout, 5, 3, 10, 14);

                            GridPathTwoSides<MapGenContext> path = new GridPathTwoSides<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.GapAxis = Axis4.Vert;

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(5, 11), new RandRange(7, 14), new RandRange(2, 5), new RandRange(4, 8)), 10);
                            //round
                            genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 9), new RandRange(7, 14)), 10);
                            //cave
                            genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(5, 11), new RandRange(7, 14)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(100, new SquareHallBrush(Loc.One * 2)), 10);
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(0, new SquareHallBrush(Loc.One * 2)), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(50, 50));

                            {
                                CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(1, 4), GetImmutableFilterList());
                                step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                                step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenDiamond<MapGenContext>(new RandRange(5, 9), new RandRange(20, 27))), 10);
                                layout.GenSteps.Add(PR_GRID_GEN, step);
                            }

                        }
                        else if (ii < 12)
                        {
                            //100% Merged Circle

                            AddInitGridStep(layout, 5, 4, 9, 9);

                            GridPathCircle<MapGenContext> path = new GridPathCircle<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.CircleRoomRatio = new RandRange(100);
                            path.Paths = new RandRange(1);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 10), new RandRange(4, 10), new RandRange(2, 5), new RandRange(2, 5)), 10);
                            //diamond
                            genericRooms.Add(new RoomGenDiamond<MapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                            //cave
                            genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(5, 10), new RandRange(5, 10)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(0), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            {
                                CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(6, 10), GetImmutableFilterList());
                                step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                                step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenDiamond<MapGenContext>(new RandRange(5, 9), new RandRange(10, 18))), 10);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenDiamond<MapGenContext>(new RandRange(10, 18), new RandRange(5, 9))), 10);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenCave<MapGenContext>(new RandRange(5, 10), new RandRange(10, 18))), 10);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenCave<MapGenContext>(new RandRange(10, 18), new RandRange(5, 10))), 10);
                                layout.GenSteps.Add(PR_GRID_GEN, step);
                            }

                        }
                        else if (ii < 20)
                        {
                            //All-One-Way-Merge
                            AddInitGridStep(layout, 5, 5, 9, 9);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(80);
                            path.BranchRatio = new RandRange(0);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 10), new RandRange(4, 10), new RandRange(2, 5), new RandRange(2, 5)), 10);
                            //diamond
                            genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 10), new RandRange(5, 10)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, new SetGridDefaultsStep<MapGenContext>(new RandRange(25), GetImmutableFilterList()));

                            {
                                CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(3, 7), GetImmutableFilterList());
                                step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                                step.Filters.Add(new RoomFilterDefaultGen(true));
                                step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                if (ii % 2 == 0)
                                {
                                    step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenCave<MapGenContext>(new RandRange(9), new RandRange(10, 18))), 10);
                                    step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 3), new RoomGenCave<MapGenContext>(new RandRange(9), new RandRange(20, 27))), 10);
                                    step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenRound<MapGenContext>(new RandRange(9), new RandRange(10, 18))), 10);
                                    step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 3), new RoomGenRound<MapGenContext>(new RandRange(9), new RandRange(20, 27))), 10);
                                }
                                else
                                {
                                    step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenCave<MapGenContext>(new RandRange(10, 18), new RandRange(9))), 10);
                                    step.Combos.Add(new GridCombo<MapGenContext>(new Loc(3, 1), new RoomGenCave<MapGenContext>(new RandRange(20, 27), new RandRange(9))), 10);
                                    step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenRound<MapGenContext>(new RandRange(10, 18), new RandRange(9))), 10);
                                    step.Combos.Add(new GridCombo<MapGenContext>(new Loc(3, 1), new RoomGenRound<MapGenContext>(new RandRange(20, 27), new RandRange(9))), 10);
                                }
                                layout.GenSteps.Add(PR_GRID_GEN, step);
                            }

                            if (ii == 18)
                            {
                                SetGridSpecialRoomStep<MapGenContext> specialStep = new SetGridSpecialRoomStep<MapGenContext>();
                                specialStep.Filters.Add(new RoomFilterComponent(true, new ImmutableRoom()));
                                specialStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));

                                RoomGenLoadMap<MapGenContext> loadRoom = new RoomGenLoadMap<MapGenContext>();
                                loadRoom.MapID = "room_updraft";
                                loadRoom.RoomTerrain = new Tile("floor");
                                loadRoom.PreventChanges = PostProcType.Panel | PostProcType.Terrain;

                                specialStep.Rooms = new PresetPicker<RoomGen<MapGenContext>>(loadRoom);
                                specialStep.RoomComponents.Set(new ImmutableRoom());
                                layout.GenSteps.Add(PR_GRID_GEN_EXTRA, specialStep);
                            }
                        }
                        else
                        {
                            //Large Map 3-Tier Merge

                            AddInitGridStep(layout, 11, 8, 5, 5);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(80);
                            path.BranchRatio = new RandRange(0, 35);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(5), new RandRange(5), new RandRange(2, 5), new RandRange(2, 5)), 10);
                            //round
                            genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5), new RandRange(5)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(100), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            {
                                CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(1, 3), GetImmutableFilterList());
                                step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                                step.Filters.Add(new RoomFilterDefaultGen(true));
                                step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(3), new RoomGenCave<MapGenContext>(new RandRange(10, 16), new RandRange(10, 16))), 10);
                                layout.GenSteps.Add(PR_GRID_GEN, step);
                            }

                            {
                                CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(8, 11), GetImmutableFilterList());
                                step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                                step.Filters.Add(new RoomFilterDefaultGen(true));
                                step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 3), new RoomGenDiamond<MapGenContext>(new RandRange(6, 10), new RandRange(9, 16))), 5);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(3, 2), new RoomGenDiamond<MapGenContext>(new RandRange(9, 16), new RandRange(6, 10))), 5);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 3), new RoomGenCave<MapGenContext>(new RandRange(6, 10), new RandRange(9, 16))), 10);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(3, 2), new RoomGenCave<MapGenContext>(new RandRange(9, 16), new RandRange(6, 10))), 10);
                                layout.GenSteps.Add(PR_GRID_GEN, step);
                            }
                            {
                                CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(8, 11), GetImmutableFilterList());
                                step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                                step.Filters.Add(new RoomFilterDefaultGen(true));
                                step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2), new RoomGenRound<MapGenContext>(new RandRange(10), new RandRange(10))), 5);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2), new RoomGenRound<MapGenContext>(new RandRange(8), new RandRange(8))), 5);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenDiamond<MapGenContext>(new RandRange(5), new RandRange(7, 11))), 5);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenDiamond<MapGenContext>(new RandRange(7, 11), new RandRange(5))), 5);
                                layout.GenSteps.Add(PR_GRID_GEN, step);
                            }
                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, false);



                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        if (ii == 18)
                            layout.GenSteps.Add(PR_DBG_CHECK, new DetectTileStep<MapGenContext>("tile_updraft"));

                        floorSegment.Floors.Add(layout);
                    }


                    {
                        LoadGen layout = new LoadGen();
                        MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                        startGen.MapID = "end_champions_road";
                        layout.GenSteps.Add(PR_FILE_LOAD, startGen);

                        MapTimeLimitStep<MapLoadContext> floorData = new MapTimeLimitStep<MapLoadContext>(600);
                        layout.GenSteps.Add(PR_FLOOR_DATA, floorData);

                        AddTextureData(layout, "sky_ruins_area_wall", "sky_ruins_floor", "sky_ruins_secondary", "flying", true);

                        floorSegment.Floors.Add(layout);
                    }
                    zone.Segments.Add(floorSegment);
                }


                {
                    int max_floors = 4;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Clouded Road\n{0}F"), new Priority(-15)));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 20);
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

                    tileSpawn.Spawns.Add(new EffectTile("trap_slumber", false), new IntRange(0, max_floors), 10);//sleep trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_hunger", true), new IntRange(0, max_floors), 10);//hunger trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_sticky", false), new IntRange(0, max_floors), 10);//sticky trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_spin", false), new IntRange(0, max_floors), 10);//spin trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_grimy", false), new IntRange(0, max_floors), 10);//grimy trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_poison", false), new IntRange(0, max_floors), 10);//poison trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_trigger", true), new IntRange(0, max_floors), 20);//trigger trap

                    tileSpawn.Spawns.Add(new EffectTile("trap_mud", false), new IntRange(0, max_floors), 10);//mud trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_self_destruct", false), new IntRange(0, max_floors), 10);//selfdestruct trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_pp_leech", true), new IntRange(0, max_floors), 10);//pp-leech trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_grudge", true), new IntRange(0, max_floors), 10);//grudge trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_summon", true), new IntRange(0, max_floors), 10);//summon trap


                    floorSegment.ZoneSteps.Add(tileSpawn);


                    {
                        LoadGen layout = new LoadGen();
                        MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                        startGen.MapID = "secret_champions_road";
                        layout.GenSteps.Add(PR_FILE_LOAD, startGen);

                        MapTimeLimitStep<MapLoadContext> floorData = new MapTimeLimitStep<MapLoadContext>(600);
                        layout.GenSteps.Add(PR_FLOOR_DATA, floorData);

                        floorSegment.Floors.Add(layout);
                    }

                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        RoomFloorGen layout = new RoomFloorGen();

                        //Floor settings
                        AddFloorData(layout, "Sky Tower Summit.ogg", 1000, Map.SightRange.Clear, Map.SightRange.Dark);

                        if (ii > 0)
                            AddDefaultMapStatus(layout, "default_weather", "cloudy", "clear", "clear");

                        //Tilesets
                        AddTextureData(layout, "sky_tower_wall", "sky_tower_floor", "sky_tower_secondary", "flying");

                        //Water
                        if (ii < 2)
                            AddWaterSteps(layout, "pit", new RandRange(25));//pit
                        else
                            AddWaterSteps(layout, "pit", new RandRange(80));//pit

                        //add water at the halls
                        {
                            RoomTerrainStep<ListMapGenContext> chasmStep = new RoomTerrainStep<ListMapGenContext>(new Tile("pit"), new RandRange(100), false, true);
                            chasmStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            chasmStep.TerrainStencil = new MatchTerrainStencil<ListMapGenContext>(false, new Tile("floor"));
                            layout.GenSteps.Add(PR_WATER, chasmStep);
                        }

                        //put the walls back in via "water" algorithm
                        if (ii >= 2)
                            AddBlobWaterSteps(layout, "wall", new RandRange(10, 14), new IntRange(1, 9), false);

                        //money
                        AddMoneyData(layout, new RandRange(2, 5));

                        //items
                        AddItemData(layout, new RandRange(2, 5), 25);

                        //enemies
                        AddRespawnData(layout, 7, 90);
                        AddEnemySpawnData(layout, 20, new RandRange(4, 7));

                        //traps
                        AddSingleTrapStep(layout, new RandRange(5, 8), "tile_wonder");//wonder tile

                        SpawnList<PatternPlan> patternList = new SpawnList<PatternPlan>();
                        patternList.Add(new PatternPlan("pattern_dither_fourth", PatternPlan.PatternExtend.Repeat2D), 5);
                        patternList.Add(new PatternPlan("pattern_squiggle", PatternPlan.PatternExtend.Repeat1D), 5);
                        patternList.Add(new PatternPlan("pattern_x_repeat", PatternPlan.PatternExtend.Repeat2D), 5);
                        AddTrapPatternSteps(layout, new RandRange(2, 4), patternList);

                        AddTrapsSteps(layout, new RandRange(16, 19));

                        AddInitListStep(layout, 58, 40, true);

                        //construct paths
                        {
                            //Create a path that is composed of a branching tree
                            FloorPathBranch<ListMapGenContext> path = new FloorPathBranch<ListMapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallPercent = 100;
                            if (ii < 2)
                                path.FillPercent = new RandRange(65);
                            else
                                path.FillPercent = new RandRange(55);
                            path.BranchRatio = new RandRange(30);

                            //Give it some room types to place
                            SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                            //cave
                            genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(6, 18), new RandRange(6, 18)), 10);
                            genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(4, 7), new RandRange(4, 7)), 10);

                            path.GenericRooms = genericRooms;

                            //Give it some hall types to place
                            int minLength;
                            if (ii < 2)
                                minLength = 1;
                            else
                                minLength = 4;
                            SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                            {
                                RoomGenAngledHall<ListMapGenContext> hall = new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(minLength, 8), new RandRange(2));
                                hall.Brush = new SquareHallBrush(Loc.One * 2);
                                genericHalls.Add(hall, 20);
                            }
                            {
                                RoomGenAngledHall<ListMapGenContext> hall = new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(2), new RandRange(minLength, 8));
                                hall.Brush = new SquareHallBrush(Loc.One * 2);
                                genericHalls.Add(hall, 20);
                            }
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_ROOMS_GEN, path);
                        }

                        //draw paths
                        layout.GenSteps.Add(PR_TILES_INIT, new DrawFloorToTileStep<ListMapGenContext>());

                        //Add the stairs up and down
                        AddStairStep(layout, false);

                        //add a stairs that takes the player back to the main path
                        {
                            EffectTile secretTile = new EffectTile("stairs_exit_down", true);
                            secretTile.TileStates.Set(new DestState(new SegLoc(-1, 17), true));
                            NearSpawnableSpawnStep<ListMapGenContext, EffectTile, MapGenEntrance> trapStep = new NearSpawnableSpawnStep<ListMapGenContext, EffectTile, MapGenEntrance>(new PickerSpawner<ListMapGenContext, EffectTile>(new PresetMultiRand<EffectTile>(secretTile)), 100);
                            layout.GenSteps.Add(PR_SPAWN_TRAPS, trapStep);
                        }

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }

                {
                    SingularSegment structure = new SingularSegment(-1);

                    ChanceFloorGen multiGen = new ChanceFloorGen();
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Advanced, "small_square", -2, true, true), 10);
                    structure.BaseFloor = multiGen;

                    zone.Segments.Add(structure);
                }

                #endregion
            }
            else if (index == 8)
                FillCaveOfWhispers(zone);
            else if (index == 9)
            {
                FillMoonlitCourtyard(zone, translate);
            }
            else if (index == 10)
            {
                FillFaultlineRidge(zone, translate);
            }
            else if (index == 11)
            {
                FillSleepingCaldera(zone);
            }
            else if (index == 12)
            {
                FillCastawayCave(zone);
            }
            else if (index == 13)
            {
                FillFertileValley(zone, translate);
            }
            else if (index == 14)
            {
                FillAmbushForest(zone, translate);
            }
            else if (index == 15)
            {
                FillTreacherousMountain(zone, translate);
            }
            else if (index == 16)
            {
                FillForsakenDesert(zone);
            }
            else if (index == 17)
            {
                #region SNOWBOUND PATH
                zone.Name = new LocalText("Snowbound Path");
                zone.Rescues = 2;
                zone.Level = 40;
                zone.ExpPercent = 0;
                zone.Rogue = RogueStatus.NoTransfer;

                {
                    int max_floors = 18;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Snowbound Path\n{0}F"), new Priority(-15)));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 5);
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);
                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;


                    //necessities
                    CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                    necessities.SpawnRates.SetRange(14, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("necessities", necessities);


                    necessities.Spawns.Add(new InvItem("berry_leppa", true), new IntRange(0, max_floors), 3);
                    necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 6);
                    necessities.Spawns.Add(new InvItem("berry_oran", true), new IntRange(0, max_floors), 2);
                    necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 4);
                    necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 10);
                    necessities.Spawns.Add(new InvItem("berry_sitrus"), new IntRange(0, max_floors), 6);
                    necessities.Spawns.Add(new InvItem("food_grimy"), new IntRange(0, max_floors), 10);
                    //snacks
                    CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                    snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                    snacks.Spawns.Add(new InvItem("seed_ice"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("herb_white"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_blinker"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("berry_yache"), new IntRange(0, max_floors), 5);
                    snacks.Spawns.Add(new InvItem("berry_kebia"), new IntRange(0, max_floors), 5);
                    //boosters
                    CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                    boosters.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("boosters", boosters);


                    boosters.Spawns.Add(new InvItem("gummi_blue"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_black"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_clear"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_grass"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_green"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_brown"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_orange"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_gold"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_red"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_royal"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_silver"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_white"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_yellow"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_sky"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_gray"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_magenta"), new IntRange(0, max_floors), 1);
                    //special
                    CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                    special.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("special", special);


                    special.Spawns.Add(new InvItem("apricorn_blue"), new IntRange(0, max_floors), 10);
                    special.Spawns.Add(new InvItem("apricorn_white"), new IntRange(0, max_floors), 10);
                    special.Spawns.Add(new InvItem("machine_assembly_box"), new IntRange(0, max_floors), 10);
                    //throwable
                    CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                    throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                    throwable.Spawns.Add(new InvItem("ammo_gravelerock", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_purge", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_lure", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_warp", false, 2), new IntRange(0, max_floors), 10);
                    //orbs
                    CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                    orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                    orbs.Spawns.Add(new InvItem("orb_scanner", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_scanner"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_freeze", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_freeze"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_spurn", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_spurn"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_petrify", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_petrify"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_totter", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_totter"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_slow", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_slow"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_trawl", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_trawl"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_cleanse"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_weather", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_weather"), new IntRange(0, max_floors), 8);
                    //held
                    CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                    held.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("held", held);


                    held.Spawns.Add(new InvItem("held_trap_scarf", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_trap_scarf"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_weather_rock", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_weather_rock"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_cover_band", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_cover_band"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_shell_bell", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_shell_bell"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_never_melt_ice", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_never_melt_ice"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_metal_coat", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_metal_coat"), new IntRange(0, max_floors), 7);
                    //tms
                    CategorySpawn<InvItem> tms = new CategorySpawn<InvItem>();
                    tms.SpawnRates.SetRange(7, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("tms", tms);


                    tms.Spawns.Add(new InvItem("tm_snatch"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_rain_dance"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_hail"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_taunt"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_focus_punch"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_safeguard"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_light_screen"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_psyshock"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_will_o_wisp"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_dream_eater"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_swagger"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_captivate"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_rock_slide"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_brick_break"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_payback"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_calm_mind"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_reflect"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_energy_ball"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_retaliate"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_scald"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_bulk_up"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_pluck"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_psych_up"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_secret_power"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_natural_gift"), new IntRange(0, max_floors), 10);


                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;


                    //packs
                    poolSpawn.Spawns.Add(GetTeamMob("swinub", "", "powder_snow", "mud_bomb", "", "", new RandRange(30), "wander_dumb"), new IntRange(0, 7), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("piloswine", "", "ice_fang", "earthquake", "", "", new RandRange(37), "wander_dumb"), new IntRange(11, max_floors), 5);
                    poolSpawn.Spawns.Add(GetTeamMob("jynx", "", "lovely_kiss", "draining_kiss", "ice_punch", "", new RandRange(36), "wander_dumb"), new IntRange(11, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("snorunt", "ice_body", "frost_breath", "", "", "", new RandRange(36), "wander_dumb"), new IntRange(0, 11), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("frosmoth", "", "hail", "bug_buzz", "", "", new RandRange(35), "wander_dumb"), new IntRange(11, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("vanillish", "snow_cloak", "icicle_spear", "mist", "", "", new RandRange(35), "wander_dumb"), new IntRange(6, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("furret", "", "foresight", "follow_me", "rest", "", new RandRange(36), "wander_dumb"), new IntRange(2, 15), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("altaria", "", "dragon_breath", "", "", "", new RandRange(36), "wander_dumb"), new IntRange(11, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("snover", "", "ice_shard", "ingrain", "", "", new RandRange(36), "wander_dumb"), new IntRange(0, 11), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("azumarill", "thick_fat", "aqua_tail", "aqua_ring", "", "", new RandRange(36), "wander_dumb"), new IntRange(6, 15), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("vigoroth", "", "uproar", "chip_away", "", "", new RandRange(33), "wander_dumb"), new IntRange(0, 7), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("hitmonlee", "", "wide_guard", "high_jump_kick", "", "", new RandRange(35), "wander_dumb"), new IntRange(11, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("fearow", "", "agility", "mirror_move", "", "", new RandRange(36), "wander_dumb"), new IntRange(6, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("golduck", "", "disable", "aqua_jet", "", "", new RandRange(36), "wander_dumb"), new IntRange(0, 11), 10);

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 6);

                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;


                    tileSpawn.Spawns.Add(new EffectTile("trap_pp_leech", true), new IntRange(0, max_floors), 10);//pp-leech trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_explosion", false), new IntRange(0, max_floors), 10);//explosion trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_slumber", false), new IntRange(0, max_floors), 10);//sleep trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_gust", false), new IntRange(0, max_floors), 10);//gust trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_hunger", true), new IntRange(0, max_floors), 10);//hunger trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_seal", false), new IntRange(0, max_floors), 10);//seal trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_sticky", false), new IntRange(0, max_floors), 10);//sticky trap


                    floorSegment.ZoneSteps.Add(tileSpawn);

                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 8), new IntRange(0, max_floors)), new MapItem("food_grimy"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("berry_leppa"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(3, max_floors)), new MapItem("machine_assembly_box"));

                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)),
                        new MapItem("apricorn_brown"), new MapItem("apricorn_white"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(max_floors / 2, max_floors - 1), new IntRange(0, max_floors)), new MapItem("orb_cleanse"));

                    {
                        //monster houses
                        SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(15, new IntRange(0, max_floors)));
                        monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHouseStep<ListMapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                        foreach (string gummi in IterateGummis())
                            monsterChanceZoneStep.Items.Add(new MapItem(gummi), new IntRange(0, max_floors), 4);//gummis
                        foreach (string iter_item in IterateApricorns())
                            monsterChanceZoneStep.Items.Add(new MapItem(iter_item), new IntRange(0, max_floors), 4);//apricorns
                        foreach (string iter_item in IterateTypePlates())
                            monsterChanceZoneStep.Items.Add(new MapItem(iter_item), new IntRange(0, max_floors), 5);//type plates

                        monsterChanceZoneStep.Items.Add(new MapItem("ammo_silver_spike", 6), new IntRange(0, max_floors), 30);//silver spike
                        monsterChanceZoneStep.Items.Add(new MapItem("loot_nugget"), new IntRange(0, max_floors), 10);//nugget
                        monsterChanceZoneStep.Items.Add(new MapItem("food_banana"), new IntRange(0, max_floors), 50);//banana
                        monsterChanceZoneStep.Items.Add(new MapItem("food_apple_big"), new IntRange(0, max_floors), 50);//big apple
                        monsterChanceZoneStep.Items.Add(new MapItem("food_banana_big"), new IntRange(0, max_floors), 10);//big banana
                        monsterChanceZoneStep.Items.Add(new MapItem("loot_heart_scale", 2), new IntRange(0, max_floors), 10);//heart scale
                        monsterChanceZoneStep.Items.Add(new MapItem("key", 1), new IntRange(0, max_floors), 10);//key
                        monsterChanceZoneStep.Items.Add(new MapItem("machine_recall_box"), new IntRange(0, max_floors), 10);//link box

                        monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(1, 4), "loot_pearl"), new ItemThemeNone(40, new RandRange(2, 4))), new IntRange(0, max_floors), 30);//no theme

                        monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(3, 7)), new IntRange(0, max_floors), 20);//gummis
                        monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(RecruitState)), true, true, new RandRange(2, 6)), new IntRange(0, 10), 10);//apricorns
                        monsterChanceZoneStep.MobThemes.Add(new MobThemeNone(40, new RandRange(7, 13)), new IntRange(0, max_floors), 10);
                        floorSegment.ZoneSteps.Add(monsterChanceZoneStep);
                    }


                    SpawnRangeList<IGenStep> shopZoneSpawns = new SpawnRangeList<IGenStep>();
                    {
                        ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                        shop.Personality = 1;
                        shop.SecurityStatus = "shop_security";
                        shop.Items.Add(new MapItem("orb_mobile", 0, 600), 20);//mobile orb
                        shop.Items.Add(new MapItem("orb_luminous", 0, 600), 20);//luminous orb
                        shop.Items.Add(new MapItem("orb_one_shot", 0, 600), 20);//one-shot orb
                        shop.Items.Add(new MapItem("orb_one_room", 0, 500), 20);//one-room orb
                        shop.Items.Add(new MapItem("orb_all_protect", 0, 600), 20);//all protect orb
                        shop.Items.Add(new MapItem("orb_itemizer", 0, 900), 20);//itemizer orb

                        shop.Items.Add(new MapItem("wand_transfer", 3, 180), 40);//transfer wand
                        shop.Items.Add(new MapItem("wand_warp", 3, 150), 40);//warp wand
                        shop.Items.Add(new MapItem("wand_purge", 3, 120), 40);//purge wand

                        shop.Items.Add(new MapItem("evo_fire_stone", 0, 2500), 50);//Fire Stone
                        shop.Items.Add(new MapItem("evo_thunder_stone", 0, 2500), 50);//Thunder Stone
                        shop.Items.Add(new MapItem("evo_water_stone", 0, 2500), 50);//Water Stone
                        shop.Items.Add(new MapItem("evo_moon_stone", 0, 3500), 50);//Moon Stone
                        shop.Items.Add(new MapItem("evo_sun_stone", 0, 3500), 50);//Sun Stone
                        shop.Items.Add(new MapItem("evo_dusk_stone", 0, 3500), 50);//Dusk Stone
                        shop.Items.Add(new MapItem("evo_dawn_stone", 0, 2500), 50);//Dawn Stone
                        shop.Items.Add(new MapItem("evo_shiny_stone", 0, 3500), 50);//Shiny Stone
                        shop.Items.Add(new MapItem("evo_leaf_stone", 0, 2500), 50);//Leaf Stone
                        shop.Items.Add(new MapItem("evo_ice_stone", 0, 2500), 50);//Ice Stone

                        shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);
                        shop.ItemThemes.Add(new ItemStateType(new FlagType(typeof(EvoState)), false, true, new RandRange(3, 5)), 10);//evo items

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
                            shop.Mobs.Add(GetShopMob("clefairy", "friend_guard", "knock_off", "minimize", "moonlight", "magic_coat", new string[] { "xcl_family_clefairy_00", "xcl_family_clefairy_03" }, -1), 10);
                            // 36 Clefable : 109 Unaware : 118 Metronome : 500 Stored Power : 343 Covet : 271 Trick
                            shop.Mobs.Add(GetShopMob("clefable", "unaware", "metronome", "stored_power", "covet", "trick", new string[] { "xcl_family_clefairy_00", "xcl_family_clefairy_03" }, -1), 5);
                            // 36 Clefable : 98 Magic Guard : 118 Metronome : 213 Attract : 282 Knock Off : 266 Follow Me
                            shop.Mobs.Add(GetShopMob("clefable", "magic_guard", "metronome", "attract", "knock_off", "follow_me", new string[] { "xcl_family_clefairy_00", "xcl_family_clefairy_03" }, -1), 5);
                        }

                        shopZoneSpawns.Add(shop, new IntRange(0, max_floors), 10);
                    }
                    SpreadStepRangeZoneStep shopZoneStep = new SpreadStepRangeZoneStep(new SpreadPlanQuota(new RandRange(1), new IntRange(8, max_floors)), PR_SHOPS, shopZoneSpawns);
                    shopZoneStep.ModStates.Add(new FlagType(typeof(ShopModGenState)));
                    floorSegment.ZoneSteps.Add(shopZoneStep);


                    //switch vaults
                    {
                        SpreadVaultZoneStep vaultChanceZoneStep = new SpreadVaultZoneStep(PR_SPAWN_ITEMS_EXTRA, PR_SPAWN_TRAPS, PR_SPAWN_MOBS_EXTRA, new SpreadPlanQuota(new RandDecay(1, 2, 50), new IntRange(0, max_floors)));

                        //making room for the vault
                        {
                            ResizeFloorStep<ListMapGenContext> addSizeStep = new ResizeFloorStep<ListMapGenContext>(new Loc(16, 16), Dir8.None);
                            vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT, addSizeStep));
                            ClampFloorStep<ListMapGenContext> limitStep = new ClampFloorStep<ListMapGenContext>(new Loc(0), new Loc(78, 54));
                            vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT, limitStep));
                            ClampFloorStep<ListMapGenContext> clampStep = new ClampFloorStep<ListMapGenContext>();
                            vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT_CLAMP, clampStep));
                        }

                        // room addition step
                        {
                            SpawnList<RoomGen<ListMapGenContext>> detourRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                            detourRooms.Add(new RoomGenCross<ListMapGenContext>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
                            SpawnList<PermissiveRoomGen<ListMapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                            detourHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                            AddConnectedRoomsRandStep<ListMapGenContext> detours = new AddConnectedRoomsRandStep<ListMapGenContext>(detourRooms, detourHalls);
                            detours.Amount = new RandRange(4, 8);
                            detours.HallPercent = 100;
                            detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom(), new UnVaultableRoom()));
                            detours.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.SwitchVault));
                            detours.RoomComponents.Set(new NoConnectRoom());
                            detours.RoomComponents.Set(new NoEventRoom());
                            detours.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.SwitchVault));
                            detours.HallComponents.Set(new NoConnectRoom());
                            detours.HallComponents.Set(new NoEventRoom());

                            vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_GEN_EXTRA, detours));
                        }

                        //sealing the vault
                        {
                            SwitchSealStep<ListMapGenContext> vaultStep = new SwitchSealStep<ListMapGenContext>("sealed_block", "tile_switch", 1, true, true);
                            vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                            vaultStep.SwitchFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            vaultStep.SwitchFilters.Add(new RoomFilterComponent(true, new BossRoom()));
                            vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_TILES_GEN_EXTRA, vaultStep));
                        }

                        //items for the vault
                        {
                            foreach (string key in IterateGummis())
                                vaultChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 200);//gummis
                            vaultChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear", 1), new IntRange(0, max_floors), 2000);//amber tear
                            vaultChanceZoneStep.Items.Add(new MapItem("seed_reviver"), new IntRange(0, max_floors), 200);//reviver seed
                            vaultChanceZoneStep.Items.Add(new MapItem("seed_joy"), new IntRange(0, max_floors), 200);//joy seed
                            vaultChanceZoneStep.Items.Add(new MapItem("orb_itemizer"), new IntRange(0, max_floors), 50);//itemizer orb
                            vaultChanceZoneStep.Items.Add(new MapItem("wand_transfer", 3), new IntRange(0, max_floors), 50);//transfer wand
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
                                boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_light", new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(1), new RandRange(1))), 30);
                            }

                            //445      ***    Heavy Box - 2* items
                            {
                                boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_heavy", new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(2), new RandRange(1))), 10);
                            }

                            //446      ***    Nifty Box - all high tier TMs, ability capsule, heart scale 9, max potion, full heal, max elixir
                            {
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

                                boxTreasure.Add(new MapItem("food_apple_huge"), 10);//huge apple
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

                            MultiStepSpawner<ListMapGenContext, MapItem> boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(2, 4)));

                            //MultiStepSpawner <- PresetMultiRand
                            MultiStepSpawner<ListMapGenContext, MapItem> mainSpawner = new MultiStepSpawner<ListMapGenContext, MapItem>();
                            mainSpawner.Picker = new PresetMultiRand<IStepSpawner<ListMapGenContext, MapItem>>(treasurePicker, boxPicker);
                            vaultChanceZoneStep.ItemSpawners.SetRange(mainSpawner, new IntRange(0, max_floors));
                        }
                        vaultChanceZoneStep.ItemAmount.SetRange(new RandRange(3, 5), new IntRange(0, max_floors));

                        // item placements for the vault
                        {
                            RandomRoomSpawnStep<ListMapGenContext, MapItem> detourItems = new RandomRoomSpawnStep<ListMapGenContext, MapItem>();
                            detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                            vaultChanceZoneStep.ItemPlacements.SetRange(detourItems, new IntRange(0, max_floors));
                        }

                        // mobs
                        // Vault FOES
                        {
                            //234 !! Stantler : 43 Leer : 95 Hypnosis : 36 Take Down : 109 Confuse Ray
                            vaultChanceZoneStep.Mobs.Add(GetFOEMob("stantler", "", "leer", "hypnosis", "take_down", "confuse_ray", zone.Level + 5, 1, 2), new IntRange(0, max_floors), 10);

                            //115 Kangaskhan : 113 Scrappy : 146 Dizzy Punch : 004 Comet Punch : 99 Rage : 203 Endure
                            vaultChanceZoneStep.Mobs.Add(GetFOEMob("kangaskhan", "scrappy", "dizzy_punch", "comet_punch", "rage", "endure", zone.Level + 5, 1, 2), new IntRange(0, max_floors), 5);

                            //275 Shiftry : 124 Pickpocket : 018 Whirlwind : 417 Nasty Plot : 536 Leaf Tornado : 542 Hurricane
                            vaultChanceZoneStep.Mobs.Add(GetFOEMob("shiftry", "pickpocket", "whirlwind", "nasty_plot", "leaf_tornado", "hurricane", zone.Level + 5, 1, 2), new IntRange(0, max_floors), 10);

                            //131 Lapras : 011 Water Absorb : 058 Ice Beam : 195 Perish Song : 362 Brine
                            vaultChanceZoneStep.Mobs.Add(GetFOEMob("lapras", "water_absorb", "ice_beam", "perish_song", "brine", "", zone.Level + 5, 1, 2), new IntRange(0, max_floors), 10);

                        }
                        vaultChanceZoneStep.MobAmount.SetRange(new RandRange(7, 11), new IntRange(0, max_floors));

                        // mob placements
                        {
                            PlaceRandomMobsStep<ListMapGenContext> secretMobPlacement = new PlaceRandomMobsStep<ListMapGenContext>();
                            secretMobPlacement.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                            secretMobPlacement.ClumpFactor = 20;
                            vaultChanceZoneStep.MobPlacements.SetRange(secretMobPlacement, new IntRange(0, max_floors));
                        }

                        floorSegment.ZoneSteps.Add(vaultChanceZoneStep);
                    }

                    AddEvoZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(3, 7), new IntRange(1, max_floors)), true);

                    AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 3);

                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        if (ii < 7)
                            AddFloorData(layout, "B33. Snowbound Path.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);
                        else if (ii < 11)
                            AddFloorData(layout, "B33. Snowbound Path.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);
                        else
                            AddFloorData(layout, "B33. Snowbound Path.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                        //Tilesets
                        if (ii < 7)
                            AddSpecificTextureData(layout, "sky_peak_7th_pass_wall", "sky_peak_7th_pass_floor", "sky_peak_7th_pass_secondary", "tall_grass_white", "ice");
                        else if (ii < 11)
                            AddSpecificTextureData(layout, "mt_faraway_2_wall", "mt_faraway_2_floor", "mt_faraway_2_secondary", "tall_grass_white", "ice");
                        else
                            AddSpecificTextureData(layout, "frosty_forest_wall", "frosty_forest_floor", "frosty_forest_secondary", "tall_grass_white", "ice");

                        if (ii >= 7)
                            AddGrassSteps(layout, new RandRange(4, 9), new IntRange(2, 7), new RandRange(25));

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile

                        SpawnList<PatternPlan> patternList = new SpawnList<PatternPlan>();
                        patternList.Add(new PatternPlan("pattern_teeth", PatternPlan.PatternExtend.Extrapolate), 5);
                        patternList.Add(new PatternPlan("pattern_squiggle", PatternPlan.PatternExtend.Repeat1D), 5);
                        patternList.Add(new PatternPlan("pattern_x", PatternPlan.PatternExtend.Extrapolate), 5);
                        patternList.Add(new PatternPlan("pattern_bubble", PatternPlan.PatternExtend.Single), 5);
                        if (ii < 11)
                            AddTrapPatternSteps(layout, new RandRange(0, 4), patternList);
                        else
                            AddTrapPatternSteps(layout, new RandRange(2, 5), patternList);

                        AddTrapsSteps(layout, new RandRange(20, 24), true);

                        //money
                        AddMoneyData(layout, new RandRange(3, 8));

                        //enemies
                        AddRespawnData(layout, 13, 110);
                        AddEnemySpawnData(layout, 20, new RandRange(9, 12));

                        //items
                        AddItemData(layout, new RandRange(3, 7), 25);

                        //construct paths
                        if (ii < 7)
                        {
                            AddInitGridStep(layout, 5, 4, 8, 8);

                            GridPathBeetle<MapGenContext> path = new GridPathBeetle<MapGenContext>();
                            path.LargeRoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.GiantHallGen.Add(new RoomGenCave<MapGenContext>(new RandRange(5, 10), new RandRange(15, 20)), 10);
                            path.LegPercent = 100;
                            path.ConnectPercent = 80;

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 10), new RandRange(4, 10), new RandRange(0, 101)), 10);
                            genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(5, 10), new RandRange(5, 10)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(100, new SquareHallBrush(new Loc(2))), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                        }
                        else if (ii < 11)
                        {
                            AddInitGridStep(layout, 5, 4, 8, 8);

                            GridPathCircle<MapGenContext> path = new GridPathCircle<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.CircleRoomRatio = new RandRange(100);
                            path.Paths = new RandRange(1, 3);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(3, 9), new RandRange(3, 9), new RandRange(3, 7), new RandRange(3, 7)), 10);
                            //cave
                            genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(5, 10), new RandRange(5, 10)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(0, new SquareHallBrush(new Loc(2))), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                        }
                        else
                        {
                            if (ii < 14)
                                AddInitGridStep(layout, 6, 4, 8, 7);
                            else
                                AddInitGridStep(layout, 8, 6, 8, 7);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(55);
                            path.BranchRatio = new RandRange(40);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 9), new RandRange(4, 9), new RandRange(2, 5), new RandRange(2, 5)), 10);
                            //cave
                            genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(0, new SquareHallBrush(new Loc(2))), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, new SetGridDefaultsStep<MapGenContext>(new RandRange(30), GetImmutableFilterList()));

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(30, 0));

                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, false);


                        if (ii == 11)
                        {
                            //vault rooms
                            {
                                SpawnList<RoomGen<MapGenContext>> detourRooms = new SpawnList<RoomGen<MapGenContext>>();
                                detourRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
                                SpawnList<PermissiveRoomGen<MapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                                RoomGenAngledHall<MapGenContext> hall = new RoomGenAngledHall<MapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4));
                                detourHalls.Add(hall, 10);
                                AddConnectedRoomsStep<MapGenContext> detours = new AddConnectedRoomsStep<MapGenContext>(detourRooms, detourHalls);
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
                            //sealing the vault
                            {
                                SpawnList<MobSpawn> guardList = new SpawnList<MobSpawn>();
                                guardList.Add(GetGuardMob(new MonsterID("slaking", 0, "", Gender.Unknown), "", "hammer_arm", "punishment", "slack_off", "chip_away", new RandRange(45), "wander_normal", "freeze"), 10);
                                guardList.Add(GetGuardMob(new MonsterID("slowbro", 0, "", Gender.Unknown), "", "psychic", "water_pulse", "disable", "amnesia", new RandRange(45), "wander_normal", "freeze"), 10);
                                guardList.Add(GetGuardMob(new MonsterID("hitmontop", 0, "", Gender.Unknown), "", "triple_kick", "gyro_ball", "wide_guard", "endeavor", new RandRange(45), "wander_normal", "freeze"), 10);
                                guardList.Add(GetGuardMob(new MonsterID("forretress", 0, "", Gender.Unknown), "", "heavy_slam", "bug_bite", "take_down", "self_destruct", new RandRange(45), "wander_normal", "freeze"), 10);
                                guardList.Add(GetGuardMob(new MonsterID("luxray", 0, "", Gender.Unknown), "", "crunch", "thunder_fang", "leer", "roar", new RandRange(45), "wander_normal", "freeze"), 10);
                                LoopedRand<MobSpawn> guards = new LoopedRand<MobSpawn>(guardList, new RandRange(4));
                                GuardSealStep<MapGenContext> vaultStep = new GuardSealStep<MapGenContext>(guards);
                                vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                                layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                            }
                            // items for the vault
                            {
                                BulkSpawner<MapGenContext, InvItem> treasures = new BulkSpawner<MapGenContext, InvItem>();
                                treasures.RandomSpawns.Add(new InvItem("apricorn_big"), 10);//big apricorn
                                treasures.RandomSpawns.Add(new InvItem("orb_mobile"), 10);//mobile orb
                                treasures.RandomSpawns.Add(new InvItem("seed_reviver"), 10);//reviver seed
                                treasures.SpawnAmount = 1;
                                RandomRoomSpawnStep<MapGenContext, InvItem> detourItems = new RandomRoomSpawnStep<MapGenContext, InvItem>(treasures);
                                detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                                layout.GenSteps.Add(PR_SPAWN_ITEMS_EXTRA, detourItems);
                            }
                            //money for the vault
                            {
                                BulkSpawner<MapGenContext, MoneySpawn> treasures = new BulkSpawner<MapGenContext, MoneySpawn>();
                                treasures.SpecificSpawns.Add(new MoneySpawn(300));
                                treasures.SpecificSpawns.Add(new MoneySpawn(300));
                                treasures.SpecificSpawns.Add(new MoneySpawn(300));
                                RandomRoomSpawnStep<MapGenContext, MoneySpawn> detourItems = new RandomRoomSpawnStep<MapGenContext, MoneySpawn>(treasures);
                                detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                                layout.GenSteps.Add(PR_SPAWN_ITEMS_EXTRA, detourItems);
                            }
                            //vault treasures
                            {
                                BulkSpawner<MapGenContext, EffectTile> treasures = new BulkSpawner<MapGenContext, EffectTile>();

                                EffectTile secretStairs = new EffectTile("stairs_secret_up", true);
                                secretStairs.TileStates.Set(new DestState(new SegLoc(1, 0)));
                                treasures.SpecificSpawns.Add(secretStairs);

                                RandomRoomSpawnStep<MapGenContext, EffectTile> detourItems = new RandomRoomSpawnStep<MapGenContext, EffectTile>(treasures);
                                detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                                layout.GenSteps.Add(PR_EXITS_DETOUR, detourItems);
                            }
                        }

                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        if (ii == 11)
                            layout.GenSteps.Add(PR_DBG_CHECK, new DetectTileStep<MapGenContext>("stairs_secret_up"));

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }


                {
                    int max_floors = 6;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Glacial Path\nB{0}F"), new Priority(-15)));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 15);
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);
                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;


                    //necessities
                    CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                    necessities.SpawnRates.SetRange(14, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("necessities", necessities);


                    necessities.Spawns.Add(new InvItem("berry_leppa", true), new IntRange(0, max_floors), 3);
                    necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 6);
                    necessities.Spawns.Add(new InvItem("berry_oran", true), new IntRange(0, max_floors), 2);
                    necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 4);
                    necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 10);
                    necessities.Spawns.Add(new InvItem("berry_sitrus"), new IntRange(0, max_floors), 6);
                    //snacks
                    CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                    snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                    snacks.Spawns.Add(new InvItem("seed_ice"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("herb_white"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_blinker"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("berry_yache"), new IntRange(0, max_floors), 5);
                    snacks.Spawns.Add(new InvItem("berry_kebia"), new IntRange(0, max_floors), 5);
                    //boosters
                    CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                    boosters.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("boosters", boosters);


                    boosters.Spawns.Add(new InvItem("gummi_blue"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_black"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_clear"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_grass"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_green"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_brown"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_orange"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_gold"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_red"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_royal"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_silver"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_white"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_yellow"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_sky"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_gray"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_magenta"), new IntRange(0, max_floors), 1);
                    //special
                    CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                    special.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("special", special);


                    special.Spawns.Add(new InvItem("apricorn_blue", true), new IntRange(0, max_floors), 3);
                    special.Spawns.Add(new InvItem("apricorn_blue"), new IntRange(0, max_floors), 7);
                    special.Spawns.Add(new InvItem("apricorn_white", true), new IntRange(0, max_floors), 3);
                    special.Spawns.Add(new InvItem("apricorn_white"), new IntRange(0, max_floors), 7);
                    special.Spawns.Add(new InvItem("machine_recall_box", true), new IntRange(0, max_floors), 3);
                    special.Spawns.Add(new InvItem("machine_recall_box"), new IntRange(0, max_floors), 7);
                    //throwable
                    CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                    throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                    throwable.Spawns.Add(new InvItem("ammo_gravelerock", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("ammo_silver_spike", false, 2), new IntRange(0, max_floors), 5);
                    throwable.Spawns.Add(new InvItem("wand_purge", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_lure", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_warp", false, 2), new IntRange(0, max_floors), 10);
                    //orbs
                    CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                    orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                    orbs.Spawns.Add(new InvItem("orb_scanner", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_scanner"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_freeze", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_freeze"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_spurn", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_spurn"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_petrify", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_petrify"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_totter", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_totter"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_slow", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_slow"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_trawl", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_trawl"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_cleanse"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_weather", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_weather"), new IntRange(0, max_floors), 8);
                    //held
                    CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                    held.SpawnRates.SetRange(4, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("held", held);


                    held.Spawns.Add(new InvItem("held_trap_scarf"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_weather_rock"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_cover_band"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_assault_vest"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_never_melt_ice"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_metal_coat"), new IntRange(0, max_floors), 10);
                    //tms
                    CategorySpawn<InvItem> tms = new CategorySpawn<InvItem>();
                    tms.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("tms", tms);


                    tms.Spawns.Add(new InvItem("tm_snatch"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_rain_dance"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_hail"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_taunt"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_focus_punch"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_safeguard"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_light_screen"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_psyshock"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_will_o_wisp"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_dream_eater"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_swagger"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_captivate"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_rock_slide"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_brick_break"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_payback"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_calm_mind"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_reflect"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_energy_ball"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_retaliate"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_scald"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_bulk_up"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_pluck"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_psych_up"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_secret_power"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_natural_gift"), new IntRange(0, max_floors), 10);


                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;


                    poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("ninetales", 1, "", Gender.Unknown), "", "ice_beam", "dazzling_gleam", "", "", new RandRange(40), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("abomasnow", "", "wood_hammer", "ice_shard", "", "", new RandRange(40), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("altaria", "", "dragon_breath", "", "", "", new RandRange(40), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("piloswine", "", "ice_fang", "earthquake", "", "", new RandRange(40), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("eiscue", "", "freeze_dry", "amnesia", "", "", new RandRange(40), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("azumarill", "huge_power", "aqua_tail", "aqua_ring", "", "", new RandRange(40), "wander_dumb"), new IntRange(0, max_floors), 10);
                    
                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 6);

                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;

                    tileSpawn.Spawns.Add(new EffectTile("trap_pp_leech", true), new IntRange(0, max_floors), 10);//pp-leech trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_explosion", false), new IntRange(0, max_floors), 10);//explosion trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_slumber", false), new IntRange(0, max_floors), 10);//sleep trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_gust", false), new IntRange(0, max_floors), 10);//gust trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_hunger", true), new IntRange(0, max_floors), 10);//hunger trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_seal", false), new IntRange(0, max_floors), 10);//seal trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_sticky", false), new IntRange(0, max_floors), 10);//sticky trap

                    floorSegment.ZoneSteps.Add(tileSpawn);

                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 5), new IntRange(0, max_floors)), new MapItem("berry_leppa"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("machine_assembly_box"));

                    {
                        MobSpawn mob = GetGuardMob("glaceon", "", "blizzard", "", "", "", new RandRange(60), "wander_normal", "sleep");
                        MobSpawnItem keySpawn = new MobSpawnItem(true);
                        keySpawn.Items.Add(new InvItem("held_choice_specs"), 10);
                        mob.SpawnFeatures.Add(keySpawn);

                        SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                        specificTeam.Spawns.Add(mob);
                        LoopedTeamSpawner<ListMapGenContext> spawner = new LoopedTeamSpawner<ListMapGenContext>(specificTeam, new RandRange(1));

                        SpawnRangeList<IGenStep> specialEnemySpawns = new SpawnRangeList<IGenStep>();
                        specialEnemySpawns.Add(new PlaceRandomMobsStep<ListMapGenContext>(spawner), new IntRange(0, max_floors), 10);
                        SpreadStepRangeZoneStep specialEnemyStep = new SpreadStepRangeZoneStep(new SpreadPlanQuota(new RandDecay(0, 1, 50), new IntRange(0, max_floors - 1)), PR_SPAWN_MOBS, specialEnemySpawns);
                        floorSegment.ZoneSteps.Add(specialEnemyStep);
                    }

                    {
                        SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(20, new IntRange(0, max_floors)));
                        monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHallStep<ListMapGenContext>(new Loc(11, 9), GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                        monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHallStep<ListMapGenContext>(new Loc(15, 13), GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);

                        foreach (string key in IterateGummis())
                            monsterChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//gummis
                        foreach (string key in IterateApricorns())
                            monsterChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//apricorns
                        monsterChanceZoneStep.Items.Add(new MapItem("food_banana"), new IntRange(0, max_floors), 25);//banana
                        foreach (string tm_id in IterateDistroTMs(TMDistroClass.Ordinary))
                            monsterChanceZoneStep.Items.Add(new MapItem(tm_id), new IntRange(0, max_floors), 2);//TMs
                        monsterChanceZoneStep.Items.Add(new MapItem("loot_nugget"), new IntRange(0, max_floors), 10);//nugget
                        monsterChanceZoneStep.Items.Add(new MapItem("loot_pearl", 1), new IntRange(0, max_floors), 10);//pearl
                        monsterChanceZoneStep.Items.Add(new MapItem("loot_heart_scale", 2), new IntRange(0, max_floors), 10);//heart scale
                        monsterChanceZoneStep.Items.Add(new MapItem("key", 1), new IntRange(0, max_floors), 10);//key
                        monsterChanceZoneStep.Items.Add(new MapItem("machine_recall_box"), new IntRange(0, max_floors), 30);//link box
                        monsterChanceZoneStep.Items.Add(new MapItem("machine_ability_capsule"), new IntRange(0, max_floors), 10);//ability capsule


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

                        monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(1, 4), "loot_pearl"), new ItemThemeNone(50, new RandRange(2, 4))), new IntRange(0, 30), 30);//no theme
                        monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeType(ItemData.UseType.Learn, false, true, new RandRange(3, 5)),
                            new ItemThemeRange(true, true, new RandRange(0, 2), ItemArray(IterateMachines()))), new IntRange(0, 30), 10);//TMs + machines

                        monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(3, 7)), new IntRange(0, 30), 30);//gummis
                        monsterChanceZoneStep.MobThemes.Add(new MobThemeNone(0, new RandRange(7, 13)), new IntRange(0, max_floors), 10);

                        floorSegment.ZoneSteps.Add(monsterChanceZoneStep);
                    }


                    AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 15, 3);

                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        AddFloorData(layout, "B34. Glacial Path.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                        //Tilesets
                        //other candidates: mt_faraway_4
                        AddTextureData(layout, "ice_maze_wall", "ice_maze_floor", "ice_maze_secondary", "ice");

                        AddWaterSteps(layout, "water", new RandRange(20));//water

                        //money
                        AddMoneyData(layout, new RandRange(3, 8));

                        //items
                        AddItemData(layout, new RandRange(3, 7), 25);

                        //enemies
                        AddRespawnData(layout, 15, 110);
                        AddEnemySpawnData(layout, 20, new RandRange(11, 14));

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(10, 14));

                        //construct paths
                        if (ii < 3)
                        {
                            AddInitGridStep(layout, 5, 4, 9, 9);

                            GridPathGrid<MapGenContext> path = new GridPathGrid<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = 100;
                            path.HallRatio = 30;

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //bump
                            genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(5, 10), new RandRange(5, 10), new RandRange(0, 81)), 10);
                            //cave
                            genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(5, 10), new RandRange(5, 10)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(20), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);
                        }
                        else
                        {
                            AddInitGridStep(layout, 4, 3, 13, 13);

                            GridPathGrid<MapGenContext> path = new GridPathGrid<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = 100;
                            path.HallRatio = 50;

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cave
                            genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(8, 14), new RandRange(8, 14)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(20), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);
                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, true);

                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }


                {
                    SingularSegment structure = new SingularSegment(-1);

                    SpawnList<TeamMemberSpawn> enemyList = new SpawnList<TeamMemberSpawn>();
                    //enemyList.Add(GetTeamMob(new MonsterID("vivillon", 6, "", Gender.Unknown), "", "poison_powder", "psybeam", "powder", "struggle_bug", new RandRange(zone.Level)), 10);
                    structure.BaseFloor = getSecretRoom(translate, "special_grass_maze", -2, "mt_faraway_2_wall", "mt_faraway_2_floor", "mt_faraway_2_secondary", "tall_grass_white", "ice", enemyList, new Loc(5, 11));

                    zone.Segments.Add(structure);
                }

                {
                    SingularSegment structure = new SingularSegment(-1);

                    ChanceFloorGen multiGen = new ChanceFloorGen();
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Intermediate, "small_square", -3, false, true), 10);
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Intermediate, "tall_hall", -3, false, true), 10);
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Intermediate, "wide_hall", -3, false, true), 10);
                    structure.BaseFloor = multiGen;

                    zone.Segments.Add(structure);
                }

                {
                    SingularSegment structure = new SingularSegment(-1);

                    ChanceFloorGen multiGen = new ChanceFloorGen();
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Intermediate, "small_square", -3, false, true), 10);
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Intermediate, "tall_hall", -3, false, true), 10);
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Intermediate, "wide_hall", -3, false, true), 10);
                    structure.BaseFloor = multiGen;

                    zone.Segments.Add(structure);
                }
                #endregion
            }
            else if (index == 18)
            {
                FillRelicTower(zone, translate);
            }
            else if (index == 19)
                FillGuildmaster(zone);
            else if (index == 20)
            {
                FillOvergrownWilds(zone, translate);
            }
            else if (index == 21)
            {
                #region WAYWARD WETLANDS
                {
                    zone.Name = new LocalText("**Wayward Wetlands");
                    zone.Rescues = 2;
                    zone.Level = 25;
                    zone.BagRestrict = 0;
                    zone.MoneyRestrict = true;
                    zone.Rogue = RogueStatus.NoTransfer;

                    {
                        int max_floors = 16;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Wayward Wetlands\nB{0}F"), new Priority(-15)));

                        //money
                        MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 10);
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
                            if (ii < 9)
                                AddFloorData(layout, "Mystifying Forest.ogg", 500, Map.SightRange.Dark, Map.SightRange.Dark);//not sure about this one...
                            else
                                AddFloorData(layout, "Mystifying Forest.ogg", 500, Map.SightRange.Dark, Map.SightRange.Dark);

                            //Tilesets
                            if (ii < 5)
                                AddTextureData(layout, "water_maze_wall", "water_maze_floor", "water_maze_secondary", "poison");
                            else if (ii < 9)
                                AddTextureData(layout, "poison_maze_wall", "poison_maze_floor", "poison_maze_secondary", "poison");
                            else
                                AddTextureData(layout, "mystifying_forest_wall", "mystifying_forest_floor", "mystifying_forest_secondary", "poison");

                            //traps
                            AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                            AddTrapsSteps(layout, new RandRange(6, 9));

                            //money - Ballpark 25K
                            AddMoneyData(layout, new RandRange(2, 4));

                            //enemies! ~ lv 20 to 30
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
            else if (index == 22)
            {
                FillTricksterWoods(zone, translate);
            }
            else if (index == 23)
            {
                FillLavaFloeIsland(zone, translate);
            }
            else if (index == 24)
            {
                #region COPPER QUARRY
                {
                    zone.Name = new LocalText("Copper Quarry");
                    zone.Rescues = 2;
                    zone.Level = 25;
                    zone.Rogue = RogueStatus.NoTransfer;

                    {
                        int max_floors = 11;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Copper Quarry\n{0}F"), new Priority(-15)));

                        //money
                        MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 0);
                        moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                        floorSegment.ZoneSteps.Add(moneySpawnZoneStep);
                        //items
                        ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                        itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;


                        //necessities
                        CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                        necessities.SpawnRates.SetRange(14, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("necessities", necessities);


                        necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 9);
                        necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 6);
                        necessities.Spawns.Add(new InvItem("food_apple"), new IntRange(0, max_floors), 10);
                        necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 10);
                        necessities.Spawns.Add(new InvItem("berry_sitrus"), new IntRange(0, max_floors), 6);
                        //snacks
                        CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                        snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                        snacks.Spawns.Add(new InvItem("seed_ban"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_doom"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("berry_shuca"), new IntRange(0, max_floors), 5);
                        snacks.Spawns.Add(new InvItem("berry_wacan"), new IntRange(0, max_floors), 5);
                        //boosters
                        CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                        boosters.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("boosters", boosters);


                        boosters.Spawns.Add(new InvItem("gummi_blue"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_black"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_clear"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_grass"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_green"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_brown"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_orange"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_gold"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_red"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_royal"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_silver"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_white"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_yellow"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_sky"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_gray"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_magenta"), new IntRange(0, max_floors), 5);
                        //special
                        CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                        special.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("special", special);


                        special.Spawns.Add(new InvItem("apricorn_brown"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_yellow"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("machine_assembly_box"), new IntRange(0, max_floors), 10);
                        //throwable
                        CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                        throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                        throwable.Spawns.Add(new InvItem("ammo_geo_pebble", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("ammo_iron_thorn", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("ammo_stick", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_path", false, 3), new IntRange(0, max_floors), 10);
                        //orbs
                        CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                        orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                        orbs.Spawns.Add(new InvItem("orb_trawl"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_weather"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_fill_in"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_endure"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_foe_seal"), new IntRange(0, max_floors), 10);
                        //held
                        CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                        held.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("held", held);


                        held.Spawns.Add(new InvItem("held_expert_belt"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_metronome"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_iron_ball"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_cover_band"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_magnet"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_metal_coat"), new IntRange(0, max_floors), 10);
                        //tms
                        CategorySpawn<InvItem> tms = new CategorySpawn<InvItem>();
                        tms.SpawnRates.SetRange(7, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("tms", tms);


                        tms.Spawns.Add(new InvItem("tm_round"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_rest"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_hidden_power"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_rock_tomb"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_strength"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_thief"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_dig"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_cut"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_power_up_punch"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_infestation"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_work_up"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_incinerate"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_roar"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_flash"), new IntRange(0, max_floors), 10);


                        floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                        //mobs
                        TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                        poolSpawn.Priority = PR_RESPAWN_MOB;


                        poolSpawn.Spawns.Add(GetTeamMob("mawile", "", "iron_head", "taunt", "", "", new RandRange(24), "wander_dumb"), new IntRange(3, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("onix", "", "rock_tomb", "stealth_rock", "", "", new RandRange(22), "wander_dumb"), new IntRange(0, 7), 5);
                        poolSpawn.Spawns.Add(GetTeamMob("aron", "", "metal_claw", "harden", "", "", new RandRange(24), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("nosepass", "", "rock_throw", "rest", "", "", new RandRange(22), "wander_dumb"), new IntRange(0, 3), 10);
                        poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("grimer", 1, "", Gender.Unknown), "", "bite", "poison_fang", "", "", new RandRange(26), "wander_dumb"), new IntRange(7, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("golbat", "", "screech", "leech_life", "", "", new RandRange(24), "wander_dumb"), new IntRange(0, 7), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("rhyhorn", "", "bulldoze", "", "", "", new RandRange(22), "wander_dumb"), new IntRange(0, 7), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("diglett", "", "dig", "", "", "", new RandRange(22), "wander_dumb"), new IntRange(0, 3), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("dugtrio", "", "dig", "sucker_punch", "", "", new RandRange(26), "wander_dumb"), new IntRange(7, max_floors), 10);
                        //sleeping
                        poolSpawn.Spawns.Add(GetTeamMob("lairon", "", "iron_head", "iron_tail", "", "", new RandRange(32), "wander_dumb", true), new IntRange(3, max_floors), 10);

                        poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                        poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 3);

                        floorSegment.ZoneSteps.Add(poolSpawn);

                        TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                        tileSpawn.Priority = PR_RESPAWN_TRAP;

                        tileSpawn.Spawns.Add(new EffectTile("trap_explosion", false), new IntRange(0, max_floors), 10);//explosion trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_sticky", false), new IntRange(0, max_floors), 10);//sticky trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_poison", false), new IntRange(0, max_floors), 10);//poison trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_mud", false), new IntRange(0, max_floors), 10);//mud trap

                        floorSegment.ZoneSteps.Add(tileSpawn);

                        AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 8), new IntRange(0, max_floors)), new MapItem("food_apple"), new MapItem("food_grimy"));
                        AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("berry_leppa"));
                        AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(3, max_floors)), new MapItem("machine_assembly_box"));

                        AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)),
                            new MapItem("apricorn_brown"), new MapItem("apricorn_purple"), new MapItem("apricorn_white"));

                        {
                            //monster houses
                            SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(15, new IntRange(0, max_floors)));
                            monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHouseStep<ListMapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                            foreach (string gummi in IterateGummis())
                                monsterChanceZoneStep.Items.Add(new MapItem(gummi), new IntRange(0, max_floors), 4);//gummis
                            foreach (string iter_item in IterateApricorns())
                                monsterChanceZoneStep.Items.Add(new MapItem(iter_item), new IntRange(0, max_floors), 2);//apricorns
                            foreach (string iter_item in IterateTypePlates())
                                monsterChanceZoneStep.Items.Add(new MapItem(iter_item), new IntRange(0, max_floors), 5);//type plates

                            monsterChanceZoneStep.Items.Add(new MapItem("loot_nugget"), new IntRange(0, max_floors), 10);//nugget
                            monsterChanceZoneStep.Items.Add(new MapItem("loot_heart_scale", 2), new IntRange(0, max_floors), 10);//heart scale
                            monsterChanceZoneStep.Items.Add(new MapItem("key", 1), new IntRange(0, max_floors), 10);//key
                            monsterChanceZoneStep.Items.Add(new MapItem("machine_recall_box"), new IntRange(0, max_floors), 10);//link box

                            monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(1, 4), "loot_pearl"), new ItemThemeNone(40, new RandRange(2, 4))), new IntRange(0, max_floors), 30);//no theme

                            monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(3, 7)), new IntRange(0, max_floors), 20);//gummis
                            monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(RecruitState)), true, true, new RandRange(2, 6)), new IntRange(0, 10), 10);//apricorns
                            monsterChanceZoneStep.MobThemes.Add(new MobThemeNone(40, new RandRange(7, 13)), new IntRange(0, max_floors), 10);
                            floorSegment.ZoneSteps.Add(monsterChanceZoneStep);
                        }

                        //shops
                        SpawnRangeList<IGenStep> shopZoneSpawns = new SpawnRangeList<IGenStep>();
                        {
                            ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                            shop.Personality = 0;
                            shop.SecurityStatus = "shop_security";

                            shop.Items.Add(new MapItem("seed_reviver", 0, 800), 15);//reviver
                            shop.Items.Add(new MapItem("apricorn_big", 0, 1000), 5);//big apricorn
                            shop.Items.Add(new MapItem("seed_joy", 0, 2000), 5);//joy seed
                            shop.Items.Add(new MapItem("held_goggle_specs", 0, 3000), 10);//goggle specs
                            shop.Items.Add(new MapItem("held_shell_bell", 0, 3000), 10);//shell bell
                            shop.Items.Add(new MapItem("held_x_ray_specs", 0, 4000), 10);//x-ray specs
                            shop.Items.Add(new MapItem("held_heal_ribbon", 0, 4000), 10);//heal ribbon
                            shop.Items.Add(new MapItem("held_wide_lens", 0, 4500), 10);//wide lens

                            foreach (string key in IterateTypeBoosters())
                                shop.Items.Add(new MapItem(key, 0, 2000), 5);//type items

                            shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                            // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 103 screech : 86 thunder wave
                            shop.StartMob = GetShopMob("kecleon", "color_change", "synchronoise", "bind", "screech", "thunder_wave", new string[] { "xcl_family_kecleon_00", "xcl_family_kecleon_01", "xcl_family_kecleon_04" }, 0);
                            {
                                // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 103 screech : 86 thunder wave
                                shop.Mobs.Add(GetShopMob("kecleon", "color_change", "synchronoise", "bind", "screech", "thunder_wave", new string[] { "xcl_family_kecleon_00", "xcl_family_kecleon_01", "xcl_family_kecleon_04" }, -1), 10);
                                // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 50 disable : 374 fling
                                shop.Mobs.Add(GetShopMob("kecleon", "color_change", "synchronoise", "bind", "disable", "fling", new string[] { "xcl_family_kecleon_00", "xcl_family_kecleon_01", "xcl_family_kecleon_04" }, -1), 10);
                                // 352 Kecleon : 168 protean : 425 shadow sneak : 246 ancient power : 510 incinerate : 168 thief
                                shop.Mobs.Add(GetShopMob("kecleon", "protean", "shadow_sneak", "ancient_power", "incinerate", "thief", new string[] { "xcl_family_kecleon_00", "xcl_family_kecleon_01", "xcl_family_kecleon_04" }, -1, "shuckle"), 10);
                                // 352 Kecleon : 168 protean : 332 aerial ace : 421 shadow claw : 60 psybeam : 364 feint
                                shop.Mobs.Add(GetShopMob("kecleon", "protean", "aerial_ace", "shadow_claw", "psybeam", "feint", new string[] { "xcl_family_kecleon_00", "xcl_family_kecleon_01", "xcl_family_kecleon_04" }, -1, "shuckle"), 10);
                            }

                            shopZoneSpawns.Add(shop, new IntRange(0, max_floors), 10);
                        }
                        {
                            ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                            shop.Personality = 0;
                            shop.SecurityStatus = "shop_security";
                            shop.Items.Add(new MapItem("berry_sitrus", 0, 100), 10);//sitrus
                            foreach (string key in IteratePinchBerries())
                                shop.Items.Add(new MapItem(key, 0, 600), 3);//pinch berries
                            foreach (string key in IterateTypeBerries())
                                shop.Items.Add(new MapItem(key, 0, 100), 1);//type berries
                            foreach (string key in IterateGummis())
                                shop.Items.Add(new MapItem(key, 0, 800), 1);//gummis
                            shop.Items.Add(new MapItem("food_apple_huge", 0, 1000), 10);//huge apple

                            shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);

                            // 213 Shuckle : 126 Contrary : 380 Gastro Acid : 230 Sweet Scent : 450 Bug Bite : 92 Toxic
                            shop.StartMob = GetShopMob("shuckle", "contrary", "gastro_acid", "sweet_scent", "bug_bite", "toxic", new string[] { "xcl_family_shuckle_00", "xcl_family_shuckle_01", "xcl_family_shuckle_02", "xcl_family_shuckle_03" }, 0);
                            {
                                // 213 Shuckle : 126 Contrary : 380 Gastro Acid : 230 Sweet Scent : 450 Bug Bite : 92 Toxic
                                shop.Mobs.Add(GetShopMob("shuckle", "contrary", "gastro_acid", "sweet_scent", "bug_bite", "toxic", new string[] { "xcl_family_shuckle_00", "xcl_family_shuckle_01", "xcl_family_shuckle_02", "xcl_family_shuckle_03" }, -1), 5);
                                // 213 Shuckle : 126 Contrary : 564 Sticky Web : 611 Infestation : 189 Mud-Slap : 522 Struggle Bug
                                shop.Mobs.Add(GetShopMob("shuckle", "contrary", "sticky_web", "infestation", "mud_slap", "struggle_bug", new string[] { "xcl_family_shuckle_00", "xcl_family_shuckle_01", "xcl_family_shuckle_02", "xcl_family_shuckle_03" }, -1), 5);
                                // 213 Shuckle : 126 Contrary : 201 Sandstorm : 564 Sticky Web : 446 Stealth Rock : 88 Rock Throw
                                shop.Mobs.Add(GetShopMob("shuckle", "sturdy", "sandstorm", "sticky_web", "stealth_rock", "rock_throw", new string[] { "xcl_family_shuckle_00", "xcl_family_shuckle_01", "xcl_family_shuckle_02", "xcl_family_shuckle_03" }, -1, "shuckle"), 10);
                                // 213 Shuckle : 5 Sturdy : 379 Power Trick : 504 Shell Smash : 205 Rollout : 360 Gyro Ball
                                shop.Mobs.Add(GetShopMob("shuckle", "sturdy", "power_trick", "shell_smash", "rollout", "gyro_ball", new string[] { "xcl_family_shuckle_00", "xcl_family_shuckle_01", "xcl_family_shuckle_02", "xcl_family_shuckle_03" }, -1, "shuckle"), 10);
                                // 213 Shuckle : 5 Sturdy : 379 Power Trick : 450 Bug Bite : 444 Stone Edge : 523 Bulldoze
                                shop.Mobs.Add(GetShopMob("shuckle", "sturdy", "power_trick", "bug_bite", "stone_edge", "bulldoze", new string[] { "xcl_family_shuckle_00", "xcl_family_shuckle_01", "xcl_family_shuckle_02", "xcl_family_shuckle_03" }, -1, "shuckle"), 5);
                            }

                            shopZoneSpawns.Add(shop, new IntRange(0, max_floors), 10);
                        }
                        SpreadStepRangeZoneStep shopZoneStep = new SpreadStepRangeZoneStep(new SpreadPlanQuota(new RandBinomial(2, 70), new IntRange(2, max_floors)), PR_SHOPS, shopZoneSpawns);
                        shopZoneStep.ModStates.Add(new FlagType(typeof(ShopModGenState)));
                        floorSegment.ZoneSteps.Add(shopZoneStep);


                        {
                            SpreadCombinedZoneStep combinedVaultZoneStep = new SpreadCombinedZoneStep();

                            {
                                SpreadVaultZoneStep detourChanceZoneStep = new SpreadVaultZoneStep(PR_SPAWN_ITEMS_EXTRA, PR_SPAWN_TRAPS, PR_SPAWN_MOBS_EXTRA, new SpreadPlanQuota(new RandRange(1), new IntRange(1, max_floors - 1)));

                                //making room for the vault
                                {
                                    ResizeFloorStep<ListMapGenContext> addSizeStep = new ResizeFloorStep<ListMapGenContext>(new Loc(16, 16), Dir8.None);
                                    detourChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT, addSizeStep));
                                    ClampFloorStep<ListMapGenContext> limitStep = new ClampFloorStep<ListMapGenContext>(new Loc(0), new Loc(78, 54));
                                    detourChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT, limitStep));
                                    ClampFloorStep<ListMapGenContext> clampStep = new ClampFloorStep<ListMapGenContext>();
                                    detourChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT_CLAMP, clampStep));
                                }

                                // room addition step
                                {
                                    SpawnList<RoomGen<ListMapGenContext>> detourRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                                    detourRooms.Add(new RoomGenCross<ListMapGenContext>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
                                    SpawnList<PermissiveRoomGen<ListMapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                                    detourHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                                    AddConnectedRoomsRandStep<ListMapGenContext> detours = new AddConnectedRoomsRandStep<ListMapGenContext>(detourRooms, detourHalls);
                                    detours.Amount = new RandRange(1);
                                    detours.HallPercent = 100;
                                    detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom(), new UnVaultableRoom()));
                                    detours.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.KeyVault));
                                    detours.RoomComponents.Set(new NoConnectRoom());
                                    detours.RoomComponents.Set(new NoEventRoom());
                                    detours.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.KeyVault));
                                    detours.HallComponents.Set(new NoConnectRoom());
                                    detours.HallComponents.Set(new NoEventRoom());

                                    detourChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_GEN_EXTRA, detours));
                                }

                                //sealing the vault
                                {
                                    KeySealStep<ListMapGenContext> vaultStep = new KeySealStep<ListMapGenContext>("sealed_block", "sealed_door", "key");
                                    vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                                    detourChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_TILES_GEN_EXTRA, vaultStep));
                                }

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
                                    detourChanceZoneStep.ItemSpawners.SetRange(mainSpawner, new IntRange(0, max_floors));
                                }
                                detourChanceZoneStep.ItemAmount.SetRange(new RandRange(0), new IntRange(0, max_floors));

                                // item placements for the vault
                                {
                                    RandomRoomSpawnStep<ListMapGenContext, MapItem> detourItems = new RandomRoomSpawnStep<ListMapGenContext, MapItem>();
                                    detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                                    detourChanceZoneStep.ItemPlacements.SetRange(detourItems, new IntRange(0, max_floors));
                                }

                                // trap spawnings for the vault
                                {
                                    //StepSpawner <- PresetMultiRand
                                    MultiStepSpawner<ListMapGenContext, EffectTile> mainSpawner = new MultiStepSpawner<ListMapGenContext, EffectTile>();
                                    EffectTile secretTile = new EffectTile("stairs_secret_down", true);
                                    secretTile.TileStates.Set(new DestState(new SegLoc(2, 0), true));
                                    mainSpawner.Picker = new PresetMultiRand<IStepSpawner<ListMapGenContext, EffectTile>>(new PickerSpawner<ListMapGenContext, EffectTile>(new PresetMultiRand<EffectTile>(secretTile)));
                                    detourChanceZoneStep.TileSpawners.SetRange(mainSpawner, new IntRange(0, max_floors));
                                }

                                // trap placements for the vault
                                {
                                    RandomRoomSpawnStep<ListMapGenContext, EffectTile> detourItems = new RandomRoomSpawnStep<ListMapGenContext, EffectTile>();
                                    detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                                    detourChanceZoneStep.TilePlacements.SetRange(detourItems, new IntRange(0, max_floors));
                                }

                                combinedVaultZoneStep.Steps.Add(detourChanceZoneStep);
                            }

                            {
                                SpreadVaultZoneStep vaultChanceZoneStep = new SpreadVaultZoneStep(PR_SPAWN_ITEMS_EXTRA, PR_SPAWN_TRAPS, PR_SPAWN_MOBS_EXTRA, new SpreadPlanQuota(new RandDecay(0, 7, 50), new IntRange(1, max_floors)));

                                //making room for the vault
                                {
                                    ResizeFloorStep<ListMapGenContext> addSizeStep = new ResizeFloorStep<ListMapGenContext>(new Loc(16, 16), Dir8.None);
                                    vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT, addSizeStep));
                                    ClampFloorStep<ListMapGenContext> limitStep = new ClampFloorStep<ListMapGenContext>(new Loc(0), new Loc(78, 54));
                                    vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT, limitStep));
                                    ClampFloorStep<ListMapGenContext> clampStep = new ClampFloorStep<ListMapGenContext>();
                                    vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT_CLAMP, clampStep));
                                }

                                // room addition step
                                {
                                    SpawnList<RoomGen<ListMapGenContext>> detourRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                                    detourRooms.Add(new RoomGenCross<ListMapGenContext>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
                                    SpawnList<PermissiveRoomGen<ListMapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                                    detourHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                                    AddConnectedRoomsRandStep<ListMapGenContext> detours = new AddConnectedRoomsRandStep<ListMapGenContext>(detourRooms, detourHalls);
                                    detours.Amount = new RandRange(1);
                                    detours.HallPercent = 100;
                                    detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom(), new UnVaultableRoom()));
                                    detours.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.KeyVault));
                                    detours.RoomComponents.Set(new NoConnectRoom());
                                    detours.RoomComponents.Set(new NoEventRoom());
                                    detours.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.KeyVault));
                                    detours.HallComponents.Set(new NoConnectRoom());
                                    detours.HallComponents.Set(new NoEventRoom());

                                    vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_GEN_EXTRA, detours));
                                }

                                //sealing the vault
                                {
                                    KeySealStep<ListMapGenContext> vaultStep = new KeySealStep<ListMapGenContext>("sealed_block", "sealed_door", "key");
                                    vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                                    vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_TILES_GEN_EXTRA, vaultStep));
                                }

                                //items for the vault
                                {
                                    foreach (string key in IterateTMs(TMClass.Starter))
                                        vaultChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 5);//TMs
                                    vaultChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear"), new IntRange(0, max_floors), 100);//amber tear
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

                                    MultiStepSpawner<ListMapGenContext, MapItem> boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(1)));

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

                                combinedVaultZoneStep.Steps.Add(vaultChanceZoneStep);
                            }

                            floorSegment.ZoneSteps.Add(combinedVaultZoneStep);
                        }

                        AddEvoZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(3, 7), new IntRange(6, max_floors)), false);

                        AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 3);

                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            RoomFloorGen layout = new RoomFloorGen();

                            //Floor settings
                            AddFloorData(layout, "B20. Copper Quarry.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                            //Tilesets
                            //other candidates: mt_steel,silent_chasm,great_canyon
                            if (ii < 7)
                                AddTextureData(layout, "rock_maze_wall", "rock_maze_floor", "rock_maze_secondary", "steel");
                            else
                                AddTextureData(layout, "drenched_bluff_isolated", "copper_quarry_floor", "copper_quarry_secondary", "steel", true);

                            if (ii >= 7)
                                AddBlobWaterSteps(layout, "pit", new RandRange(3, 8), new IntRange(2, 5), true);//pit

                            //traps
                            AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile

                            SpawnList<PatternPlan> patternList = new SpawnList<PatternPlan>();
                            patternList.Add(new PatternPlan("pattern_dither_fourth", PatternPlan.PatternExtend.Repeat2D), 5);
                            patternList.Add(new PatternPlan("pattern_x", PatternPlan.PatternExtend.Extrapolate), 5);
                            AddTrapPatternSteps(layout, new RandRange(1, 4), patternList);

                            AddTrapsSteps(layout, new RandRange(10, 14));

                            //money
                            AddMoneyData(layout, new RandRange(3, 6));

                            //enemies
                            AddRespawnData(layout, 10, 100);
                            AddEnemySpawnData(layout, 20, new RandRange(7, 10));

                            //items
                            AddItemData(layout, new RandRange(3, 6), 25);


                            //construct paths
                            {
                                AddInitListStep(layout, 50, 40);

                                FloorPathBranch<ListMapGenContext> path = new FloorPathBranch<ListMapGenContext>();

                                path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                if (ii < 7)
                                    path.FillPercent = new RandRange(45);
                                else
                                    path.FillPercent = new RandRange(60);

                                if (ii < 7)
                                    path.HallPercent = 100;
                                else
                                    path.HallPercent = 50;

                                path.BranchRatio = new RandRange(0, 25);

                                SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                                //cross
                                genericRooms.Add(new RoomGenCross<ListMapGenContext>(new RandRange(3, 10), new RandRange(3, 10), new RandRange(2, 6), new RandRange(2, 6)), 10);
                                if (ii < 7) //square
                                    genericRooms.Add(new RoomGenSquare<ListMapGenContext>(new RandRange(3, 7), new RandRange(3, 7)), 10);
                                else //blocked
                                    genericRooms.Add(new RoomGenBlocked<ListMapGenContext>(new Tile("pit"), new RandRange(3, 8), new RandRange(3, 8), new RandRange(1, 4), new RandRange(1, 4)), 5);
                                path.GenericRooms = genericRooms;

                                SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                                genericHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(1, 6), new RandRange(1, 6)), 10);
                                path.GenericHalls = genericHalls;

                                layout.GenSteps.Add(PR_ROOMS_GEN, path);

                                layout.GenSteps.Add(PR_ROOMS_GEN, CreateGenericListConnect(25, 0));

                            }

                            AddDrawListSteps(layout);

                            AddStairStep(layout, true);


                            if (ii == 6)
                            {
                                //vault rooms
                                {
                                    SpawnList<RoomGen<ListMapGenContext>> detourRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                                    detourRooms.Add(new RoomGenCross<ListMapGenContext>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
                                    SpawnList<PermissiveRoomGen<ListMapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                                    detourHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                                    AddConnectedRoomsStep<ListMapGenContext> detours = new AddConnectedRoomsStep<ListMapGenContext>(detourRooms, detourHalls);
                                    detours.Amount = new RandRange(1);
                                    detours.HallPercent = 100;
                                    detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom(), new UnVaultableRoom()));
                                    detours.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.BlockVault));
                                    detours.RoomComponents.Set(new NoConnectRoom());
                                    detours.RoomComponents.Set(new NoEventRoom());
                                    detours.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.BlockVault));
                                    detours.HallComponents.Set(new NoConnectRoom());
                                    detours.HallComponents.Set(new NoEventRoom());

                                    layout.GenSteps.Add(PR_ROOMS_GEN_EXTRA, detours);
                                }
                                //sealing the vault
                                {
                                    TerrainSealStep<ListMapGenContext> vaultStep = new TerrainSealStep<ListMapGenContext>("wall", "wall");
                                    vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BlockVault));
                                    layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                                }
                                //money for the vault
                                {
                                    BulkSpawner<ListMapGenContext, MoneySpawn> treasures = new BulkSpawner<ListMapGenContext, MoneySpawn>();
                                    treasures.SpecificSpawns.Add(new MoneySpawn(200));
                                    treasures.SpecificSpawns.Add(new MoneySpawn(200));
                                    treasures.SpecificSpawns.Add(new MoneySpawn(200));
                                    treasures.SpecificSpawns.Add(new MoneySpawn(200));
                                    treasures.SpecificSpawns.Add(new MoneySpawn(200));
                                    RandomRoomSpawnStep<ListMapGenContext, MoneySpawn> detourItems = new RandomRoomSpawnStep<ListMapGenContext, MoneySpawn>(treasures);
                                    detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BlockVault));
                                    layout.GenSteps.Add(PR_SPAWN_ITEMS_EXTRA, detourItems);
                                }
                                //vault treasures
                                {
                                    BulkSpawner<ListMapGenContext, EffectTile> treasures = new BulkSpawner<ListMapGenContext, EffectTile>();

                                    EffectTile secretStairs = new EffectTile("stairs_secret_up", true);
                                    secretStairs.TileStates.Set(new DestState(new SegLoc(1, 0)));
                                    treasures.SpecificSpawns.Add(secretStairs);

                                    RandomRoomSpawnStep<ListMapGenContext, EffectTile> detourItems = new RandomRoomSpawnStep<ListMapGenContext, EffectTile>(treasures);
                                    detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BlockVault));
                                    layout.GenSteps.Add(PR_EXITS_DETOUR, detourItems);
                                }
                            }

                            layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<ListMapGenContext, MapGenEntrance, MapGenExit>());

                            if (ii == 6)
                                layout.GenSteps.Add(PR_DBG_CHECK, new DetectTileStep<ListMapGenContext>("stairs_secret_up"));

                            floorSegment.Floors.Add(layout);
                        }


                        {
                            LoadGen layout = new LoadGen();
                            MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                            startGen.MapID = "end_copper_quarry";
                            layout.GenSteps.Add(PR_FILE_LOAD, startGen);

                            MapTimeLimitStep<MapLoadContext> floorData = new MapTimeLimitStep<MapLoadContext>(600);
                            layout.GenSteps.Add(PR_FLOOR_DATA, floorData);

                            AddTextureData(layout, "drenched_bluff_isolated", "copper_quarry_floor", "copper_quarry_secondary", "steel", true);

                            {
                                HashSet<string> exceptFor = new HashSet<string>();
                                foreach (string legend in IterateLegendaries())
                                    exceptFor.Add(legend);
                                SpeciesItemElementSpawner<MapLoadContext> spawn = new SpeciesItemElementSpawner<MapLoadContext>(new IntRange(2), new RandRange(1), "steel", exceptFor);
                                BoxSpawner<MapLoadContext> box = new BoxSpawner<MapLoadContext>("box_heavy", spawn);
                                List<Loc> treasureLocs = new List<Loc>();
                                treasureLocs.Add(new Loc(7, 9));
                                layout.GenSteps.Add(PR_SPAWN_ITEMS, new SpecificSpawnStep<MapLoadContext, MapItem>(box, treasureLocs));
                            }

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }


                    {
                        int max_floors = 4;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Lodestone Quarry\nB{0}F"), new Priority(-15)));

                        //money
                        MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 10);
                        moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                        floorSegment.ZoneSteps.Add(moneySpawnZoneStep);
                        //items
                        ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                        itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;


                        //necessities
                        CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                        necessities.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("necessities", necessities);


                        necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 9);
                        necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 6);
                        necessities.Spawns.Add(new InvItem("food_grimy"), new IntRange(0, max_floors), 10);
                        necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 10);
                        necessities.Spawns.Add(new InvItem("berry_sitrus"), new IntRange(0, max_floors), 6);
                        //snacks
                        CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                        snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                        snacks.Spawns.Add(new InvItem("berry_jaboca"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_ban"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_doom"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("berry_shuca"), new IntRange(0, max_floors), 5);
                        snacks.Spawns.Add(new InvItem("berry_wacan"), new IntRange(0, max_floors), 5);
                        //boosters
                        CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                        boosters.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("boosters", boosters);


                        boosters.Spawns.Add(new InvItem("gummi_blue"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_black"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_clear"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_grass"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_green"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_brown"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_orange"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_gold"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_red"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_royal"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_silver"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_white"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_yellow"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_sky"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_gray"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_magenta"), new IntRange(0, max_floors), 5);
                        //special
                        CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                        special.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("special", special);


                        special.Spawns.Add(new InvItem("apricorn_brown"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_yellow"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("machine_assembly_box"), new IntRange(0, max_floors), 10);
                        //throwable
                        CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                        throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                        throwable.Spawns.Add(new InvItem("ammo_geo_pebble", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("ammo_iron_thorn", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("ammo_stick", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_path", false, 3), new IntRange(0, max_floors), 10);
                        //orbs
                        CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                        orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                        orbs.Spawns.Add(new InvItem("orb_trawl"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_weather"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_fill_in"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_endure"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_foe_seal"), new IntRange(0, max_floors), 10);
                        //held
                        CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                        held.SpawnRates.SetRange(4, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("held", held);


                        held.Spawns.Add(new InvItem("held_expert_belt"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_metronome"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_iron_ball"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_cover_band"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_magnet"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_metal_coat"), new IntRange(0, max_floors), 20);
                        //tms
                        CategorySpawn<InvItem> tms = new CategorySpawn<InvItem>();
                        tms.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("tms", tms);


                        tms.Spawns.Add(new InvItem("tm_round"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_rest"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_hidden_power"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_rock_tomb"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_strength"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_thief"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_dig"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_cut"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_power_up_punch"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_infestation"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_work_up"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_incinerate"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_roar"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_flash"), new IntRange(0, max_floors), 10);


                        floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                        //mobs
                        TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                        poolSpawn.Priority = PR_RESPAWN_MOB;

                        poolSpawn.Spawns.Add(GetTeamMob("beldum", "", "take_down", "", "", "", new RandRange(18), "wander_dumb"), new IntRange(0, max_floors), 5);
                        poolSpawn.Spawns.Add(GetTeamMob("magnemite", "", "mirror_shot", "sonic_boom", "", "", new RandRange(25), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("probopass", "", "magnet_bomb", "", "", "", new RandRange(25), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("togedemaru", "", "rollout", "spark", "", "", new RandRange(24), "wander_dumb"), new IntRange(0, max_floors), 5);
                        poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("grimer", 1, "", Gender.Unknown), "", "bite", "poison_fang", "", "", new RandRange(26), "wander_dumb"), new IntRange(0, max_floors), 5);
                        poolSpawn.Spawns.Add(GetTeamMob("golbat", "", "screech", "leech_life", "", "", new RandRange(24), "wander_dumb"), new IntRange(0, max_floors), 10);
                        //In Groups
                        poolSpawn.SpecificSpawns.Add(new SpecificTeamSpawner(GetGenericMob("aron", "", "iron_head", "harden", "", "", new RandRange(24), "wander_dumb"), GetGenericMob("aron", "", "iron_head", "harden", "", "", new RandRange(24), "wander_dumb")), new IntRange(0, max_floors), 20);
                        //sleeping
                        poolSpawn.Spawns.Add(GetTeamMob("lairon", "", "iron_head", "iron_tail", "", "", new RandRange(35), "wander_dumb", true), new IntRange(0, max_floors), 10);

                        poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                        poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 3);

                        floorSegment.ZoneSteps.Add(poolSpawn);

                        TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                        tileSpawn.Priority = PR_RESPAWN_TRAP;

                        tileSpawn.Spawns.Add(new EffectTile("trap_explosion", false), new IntRange(0, max_floors), 10);//explosion trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_sticky", false), new IntRange(0, max_floors), 10);//sticky trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_poison", false), new IntRange(0, max_floors), 10);//poison trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_mud", false), new IntRange(0, max_floors), 10);//mud trap

                        floorSegment.ZoneSteps.Add(tileSpawn);


                        {
                            SpreadVaultZoneStep vaultChanceZoneStep = new SpreadVaultZoneStep(PR_SPAWN_ITEMS_EXTRA, PR_SPAWN_TRAPS, PR_SPAWN_MOBS_EXTRA, new SpreadPlanQuota(new RandDecay(0, 8, 50), new IntRange(0, max_floors)));

                            //making room for the vault
                            {
                                ResizeFloorStep<ListMapGenContext> addSizeStep = new ResizeFloorStep<ListMapGenContext>(new Loc(16, 16), Dir8.None);
                                vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT, addSizeStep));
                                ClampFloorStep<ListMapGenContext> limitStep = new ClampFloorStep<ListMapGenContext>(new Loc(0), new Loc(78, 54));
                                vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT, limitStep));
                                ClampFloorStep<ListMapGenContext> clampStep = new ClampFloorStep<ListMapGenContext>();
                                vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT_CLAMP, clampStep));
                            }

                            // room addition step
                            {
                                SpawnList<RoomGen<ListMapGenContext>> detourRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                                detourRooms.Add(new RoomGenCross<ListMapGenContext>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
                                SpawnList<PermissiveRoomGen<ListMapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                                detourHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                                AddConnectedRoomsRandStep<ListMapGenContext> detours = new AddConnectedRoomsRandStep<ListMapGenContext>(detourRooms, detourHalls);
                                detours.Amount = new RandRange(1);
                                detours.HallPercent = 100;
                                detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom(), new UnVaultableRoom()));
                                detours.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.KeyVault));
                                detours.RoomComponents.Set(new NoConnectRoom());
                                detours.RoomComponents.Set(new NoEventRoom());
                                detours.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.KeyVault));
                                detours.HallComponents.Set(new NoConnectRoom());
                                detours.HallComponents.Set(new NoEventRoom());

                                vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_GEN_EXTRA, detours));
                            }

                            //sealing the vault
                            {
                                KeySealStep<ListMapGenContext> vaultStep = new KeySealStep<ListMapGenContext>("sealed_block", "sealed_door", "key");
                                vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                                vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_TILES_GEN_EXTRA, vaultStep));
                            }

                            //items for the vault
                            {
                                foreach (string key in IterateTMs(TMClass.Starter))
                                    vaultChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 5);//TMs
                                vaultChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear"), new IntRange(0, max_floors), 100);//amber tear
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

                                MultiStepSpawner<ListMapGenContext, MapItem> boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(1)));

                                //StepSpawner <- PresetMultiRand
                                MultiStepSpawner<ListMapGenContext, MapItem> mainSpawner = new MultiStepSpawner<ListMapGenContext, MapItem>();
                                mainSpawner.Picker = new PresetMultiRand<IStepSpawner<ListMapGenContext, MapItem>>(treasurePicker, boxPicker);
                                vaultChanceZoneStep.ItemSpawners.SetRange(mainSpawner, new IntRange(0, max_floors));
                            }
                            vaultChanceZoneStep.ItemAmount.SetRange(new RandRange(0, 2), new IntRange(0, max_floors));


                            // item placements for the vault
                            {
                                RandomRoomSpawnStep<ListMapGenContext, MapItem> detourItems = new RandomRoomSpawnStep<ListMapGenContext, MapItem>();
                                detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                                vaultChanceZoneStep.ItemPlacements.SetRange(detourItems, new IntRange(0, max_floors));
                            }

                            floorSegment.ZoneSteps.Add(vaultChanceZoneStep);
                        }

                        AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 3);


                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            RoomFloorGen layout = new RoomFloorGen();

                            //Floor settings
                            AddFloorData(layout, "B21. Magnetic Quarry.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                            //Tilesets
                            AddTextureData(layout, "zero_isle_east_4_wall", "zero_isle_east_4_floor", "zero_isle_east_4_secondary", "steel");

                            //money
                            AddMoneyData(layout, new RandRange(3, 6));

                            //items
                            AddItemData(layout, new RandRange(3, 6), 25);

                            //enemies
                            AddRespawnData(layout, 10, 100);
                            AddEnemySpawnData(layout, 20, new RandRange(7, 10));

                            //traps
                            AddSingleTrapStep(layout, new RandRange(5, 8), "tile_wonder");//wonder tile
                            AddTrapsSteps(layout, new RandRange(14, 18));

                            //construct paths
                            {
                                AddInitListStep(layout, 50, 40);

                                FloorPathBranch<ListMapGenContext> path = new FloorPathBranch<ListMapGenContext>();

                                path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                path.FillPercent = new RandRange(55);

                                path.BranchRatio = new RandRange(80);

                                SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                                //diamond
                                //genericRooms.Add(new RoomGenDiamond<ListMapGenContext>(new RandRange(5), new RandRange(5)), 2);
                                bool flipH = ii % 2 == 0;
                                bool flipV = ii / 2 % 2 == 0;
                                genericRooms.Add(new RoomGenTriangle<ListMapGenContext>(new RandRange(3), flipH, flipV), 10);
                                genericRooms.Add(new RoomGenTriangle<ListMapGenContext>(new RandRange(5), flipH, flipV), 8);
                                genericRooms.Add(new RoomGenTriangle<ListMapGenContext>(new RandRange(9), flipH, flipV), 4);
                                genericRooms.Add(new RoomGenTriangle<ListMapGenContext>(new RandRange(15), flipH, flipV), 2);
                                path.GenericRooms = genericRooms;

                                SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                                genericHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(1), new RandRange(1)), 10);
                                path.GenericHalls = genericHalls;

                                layout.GenSteps.Add(PR_ROOMS_GEN, path);

                                layout.GenSteps.Add(PR_ROOMS_GEN, CreateGenericListConnect(25, 0));

                            }

                            AddDrawListSteps(layout);

                            AddStairStep(layout, true);

                            layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<ListMapGenContext, MapGenEntrance, MapGenExit>());

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }

                    {
                        SingularSegment structure = new SingularSegment(-1);

                        SpawnList<TeamMemberSpawn> enemyList = new SpawnList<TeamMemberSpawn>();
                        enemyList.Add(GetTeamMob(new MonsterID("carbink", 0, "", Gender.Unknown), "", "stealth_rock", "power_gem", "rock_throw", "", new RandRange(zone.Level), "turret"), 10);
                        structure.BaseFloor = getSecretRoom(translate, "special_rby_fossil", -2, "zero_isle_east_4_wall", "zero_isle_east_4_floor", "zero_isle_east_4_secondary", "", "steel", enemyList, new Loc(5, 11));

                        zone.Segments.Add(structure);
                    }


                    {
                        SingularSegment structure = new SingularSegment(-1);

                        ChanceFloorGen multiGen = new ChanceFloorGen();
                        multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Intermediate, "small_square", -3, false, false), 10);
                        multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Intermediate, "tall_hall", -3, false, false), 10);
                        multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Intermediate, "wide_hall", -3, false, false), 10);
                        structure.BaseFloor = multiGen;

                        zone.Segments.Add(structure);
                    }


                    {
                        SingularSegment structure = new SingularSegment(-1);

                        ChanceFloorGen multiGen = new ChanceFloorGen();
                        multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Intermediate, "small_square", -3, false, false), 10);
                        multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Intermediate, "tall_hall", -3, false, false), 10);
                        multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Intermediate, "wide_hall", -3, false, false), 10);
                        structure.BaseFloor = multiGen;

                        zone.Segments.Add(structure);
                    }
                }
                #endregion
            }
            else if (index == 25)
            {
            }
            else if (index == 30)
                FillSecretGarden(zone);
            else if (index == 31)
            {
                FillCaveOfSolace(zone, translate);
            }
            else if (index == 32)
            {
                FillRoyalHalls(zone, translate);
            }
            else if (index == 33)
            {
                FillTheSky(zone, translate);
            }
            else if (index == 34)
            {
                FillTheAbyss(zone, translate);
            }
            else if (index == 35)
                FillTrainingMaze(zone);
            else if (index == 36)
            {
                FillBrambleWoods(zone, translate);
            }
            else if (index == 37)
            {
                FillSicklyHollow(zone, translate);
            }
            else if (index == 38)
            {
                FillEnergyGarden(zone, translate);
            }
            else if (index == 39)
            {
                #region TINY TUNNEL
                {
                    zone.Name = new LocalText("**Tiny Tunnel");
                    zone.Level = 5;
                    zone.LevelCap = true;
                    zone.BagRestrict = 16;
                    zone.TeamSize = 1;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.NoTransfer;

                    {
                        int max_floors = 8;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Tiny Tunnel\nB{0}F"), new Priority(-15)));

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


                        AddEvoZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 5), new IntRange(1, max_floors)), true);


                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            AddFloorData(layout, "Title.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                            //Tilesets
                            AddTextureData(layout, "thunderwave_cave_wall", "thunderwave_cave_floor", "thunderwave_cave_secondary", "bug");

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

                            layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 40)
            {
                #region GEODE UNDERPASS
                {
                    zone.Name = new LocalText("**Geode Underpass");
                    zone.Level = 20;
                    zone.LevelCap = true;
                    zone.BagRestrict = 8;
                    zone.TeamSize = 2;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.NoTransfer;

                    {
                        int max_floors = 12;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Geode Underpass\nB{0}F"), new Priority(-15)));

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


                        AddEvoZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 5), new IntRange(1, max_floors)), true);


                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            AddFloorData(layout, "Title.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                            //Tilesets
                            AddTextureData(layout, "test_dungeon_wall", "test_dungeon_floor", "test_dungeon_secondary", "normal");

                            if (ii % 3 == 2)
                                AddWaterSteps(layout, "water", new RandRange(30));//water

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

                            layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 41)
            {
                #region STARFALL HEIGHTS
                {
                    zone.Name = new LocalText("**Starfall Heights");
                    zone.Level = 35;
                    zone.LevelCap = true;
                    zone.BagRestrict = 8;
                    zone.TeamSize = 3;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.NoTransfer;

                    {
                        int max_floors = 3;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Starfall Heights\n{0}F"), new Priority(-15)));

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
            else if (index == 42)
            {
                #region EON ISLAND
                {
                    zone.Name = new LocalText("**Eon Island");
                    zone.Level = 5;
                    zone.LevelCap = true;
                    zone.BagRestrict = 16;
                    zone.TeamSize = 2;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.NoTransfer;

                    {
                        int max_floors = 3;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Eon Island Coast\n{0}F"), new Priority(-15)));

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
            else if (index == 43)
            {
                #region BARREN TUNDRA
                {
                    zone.Name = new LocalText("**Barren Tundra");
                    zone.Level = 35;
                    zone.BagRestrict = 0;
                    zone.TeamSize = 3;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.NoTransfer;

                    {
                        int max_floors = 30;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Barren Tundra\n{0}F"), new Priority(-15)));

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

                            layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 44)
            {

            }
            else if (index == 45)
            {
                #region BRAVERY ROAD
                {
                    zone.Name = new LocalText("**Bravery Road");
                    zone.Level = 30;
                    zone.LevelCap = true;
                    zone.TeamSize = 2;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.NoTransfer;

                    {
                        int max_floors = 10;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Bravery Road\n{0}F"), new Priority(-15)));

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
            else if (index == 46)
            {
                #region WISDOM ROAD
                {
                    zone.Name = new LocalText("**Wisdom Road");
                    zone.Level = 30;
                    zone.LevelCap = true;
                    zone.TeamSize = 2;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.NoTransfer;

                    {
                        int max_floors = 10;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Wisdom Road\n{0}F"), new Priority(-15)));

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


                            if (ii == 1)
                            {
                                //making room for the vault
                                {
                                    ResizeFloorStep<MapGenContext> addSizeStep = new ResizeFloorStep<MapGenContext>(new Loc(16, 16), Dir8.None);
                                    layout.GenSteps.Add(PR_ROOMS_PRE_VAULT, addSizeStep);
                                    ClampFloorStep<MapGenContext> limitStep = new ClampFloorStep<MapGenContext>(new Loc(0), new Loc(78, 54));
                                    layout.GenSteps.Add(PR_ROOMS_PRE_VAULT, limitStep);
                                    ClampFloorStep<MapGenContext> clampStep = new ClampFloorStep<MapGenContext>();
                                    layout.GenSteps.Add(PR_ROOMS_PRE_VAULT_CLAMP, clampStep);
                                }

                                //vault rooms
                                {
                                    SpawnList<RoomGen<MapGenContext>> detourRooms = new SpawnList<RoomGen<MapGenContext>>();
                                    detourRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
                                    SpawnList<PermissiveRoomGen<MapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                                    detourHalls.Add(new RoomGenAngledHall<MapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                                    AddConnectedRoomsStep<MapGenContext> detours = new AddConnectedRoomsStep<MapGenContext>(detourRooms, detourHalls);
                                    detours.Amount = new RandRange(1);
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
                                    SwitchSealStep<MapGenContext> vaultStep = new SwitchSealStep<MapGenContext>("sealed_block", "tile_switch_sync", 3, true, false);
                                    vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                                    vaultStep.SwitchFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                                    vaultStep.SwitchFilters.Add(new RoomFilterComponent(true, new BossRoom()));
                                    layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                                }

                                //vault treasures
                                {
                                    BulkSpawner<MapGenContext, EffectTile> treasures = new BulkSpawner<MapGenContext, EffectTile>();

                                    EffectTile secretStairs = new EffectTile("stairs_secret_up", true);
                                    secretStairs.TileStates.Set(new DestState(new SegLoc(1, 0)));
                                    treasures.SpecificSpawns.Add(secretStairs);

                                    RandomRoomSpawnStep<MapGenContext, EffectTile> detourItems = new RandomRoomSpawnStep<MapGenContext, EffectTile>(treasures);
                                    detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                                    layout.GenSteps.Add(PR_EXITS_DETOUR, detourItems);
                                }
                            }

                            layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 47)
            {
                #region HOPE ROAD
                {
                    zone.Name = new LocalText("**Hope Road");
                    zone.Level = 40;
                    zone.LevelCap = true;
                    zone.TeamSize = 2;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.NoTransfer;

                    {
                        int max_floors = 12;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Hope Road\n{0}F"), new Priority(-15)));

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
            else if (index == 48)
            {
                #region DESERTED FORTRESS
                {
                    zone.Name = new LocalText("**Deserted Fortress");
                    zone.Level = 25;
                    zone.LevelCap = true;
                    zone.TeamSize = 2;
                    zone.BagRestrict = 16;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.NoTransfer;

                    {
                        int max_floors = 18;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Deserted Fortress\n{0}F"), new Priority(-15)));

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
                            AddFloorData(layout, "Mystifying Forest.ogg", 500, Map.SightRange.Dark, Map.SightRange.Dark);

                            //Tilesets
                            if (ii < 9)
                                AddTextureData(layout, "darknight_relic_wall", "darknight_relic_floor", "darknight_relic_secondary", "normal");
                            else
                                AddTextureData(layout, "rescue_team_maze_wall", "rescue_team_maze_floor", "rescue_team_maze_secondary", "normal");

                            //traps
                            AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                            AddTrapsSteps(layout, new RandRange(6, 9));

                            //money - Ballpark 25K
                            AddMoneyData(layout, new RandRange(2, 4));

                            //enemies! ~ lv 20 to 30
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
            else if (index == 49)
            {
                FillPrismIsles(zone);
            }
            else if (index == 50)
            {
                FillLabyrinthOfTheLost(zone);
            }
            else if (index == 51)
            {
                FillSacredTower(zone, translate);
            }
            else if (index == 52)
            {
                FillLostSeas(zone, translate);
            }
            else if (index == 53)
            {
                FillInscribedCave(zone, translate);
            }
            else if (index == 54)
            {
                FillNeverEndingTale(zone, translate);
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
