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
            DataInfo.DeleteIndexedData(DataManager.DataType.Zone.ToString());

            for (int ii = 0; ii < MAX_ZONES; ii++)
            {
                ZoneData zone = GetZoneData(ii);
                if (zone.Name.DefaultText != "")
                    DataManager.SaveData(Text.Sanitize(zone.Name.DefaultText).ToLower(), DataManager.DataType.Zone.ToString(), zone);
            }
        }


        public static ZoneData GetZoneData(int index)
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
                    zone.Rogue = RogueStatus.ItemTransfer;

                    int max_floors = 4;

                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Tropical Path\n{0}F")));

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


                    RandBag<IGenPriority> npcZoneSpawns = new RandBag<IGenPriority>();
                    npcZoneSpawns.RemoveOnRoll = true;
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
                        npcZoneSpawns.ToSpawn.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_SPAWN_MOBS_EXTRA, randomSpawn));
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
                        npcZoneSpawns.ToSpawn.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_SPAWN_MOBS_EXTRA, randomSpawn));
                    }
                    SpreadStepZoneStep npcZoneStep = new SpreadStepZoneStep(new SpreadPlanQuota(new RandRange(2), new IntRange(0, max_floors), true), npcZoneSpawns);
                    floorSegment.ZoneSteps.Add(npcZoneStep);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    floorSegment.ZoneSteps.Add(tileSpawn);

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
                        floorSegment.Floors.Add(layout);
                    }
                    zone.Segments.Add(floorSegment);

                }
                {
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Secret Room")));
                    {
                        LoadGen layout = new LoadGen();
                        MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                        startGen.MapID = "zone_02_end";
                        layout.GenSteps.Add(PR_TILES_INIT, startGen);
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
                        int max_floors = 7;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Faded Trail\n{0}F")));

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
                        //114 Tangela : 022 Vine Whip
                        poolSpawn.Spawns.Add(GetTeamMob("tangela", "", "", "vine_whip", "", "", new RandRange(10), "wander_dumb"), new IntRange(4, 7), 10);
                        //403 Shinx : 033 Tackle : 43 Leer
                        poolSpawn.Spawns.Add(GetTeamMob("shinx", "", "tackle", "leer", "", "", new RandRange(5), "wander_dumb"), new IntRange(0, 4), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("shinx", "", "tackle", "leer", "", "", new RandRange(7), "wander_dumb"), new IntRange(4, 7), 10);
                        //190 Aipom : 010 Scratch : Sand Attack
                        poolSpawn.Spawns.Add(GetTeamMob("aipom", "", "scratch", "sand_attack", "", "", new RandRange(8), "wander_dumb"), new IntRange(0, 7), 10);
                        //161 Sentret : 10 Scratch : 111 Defense Curl
                        poolSpawn.Spawns.Add(GetTeamMob("sentret", "", "scratch", "defense_curl", "", "", new RandRange(5), "wander_dumb"), new IntRange(0, 4), 10);
                        //060 Poliwag : 55 Water Gun
                        poolSpawn.Spawns.Add(GetTeamMob("poliwag", "", "water_gun", "", "", "", new RandRange(8), "wander_dumb"), new IntRange(4, 7), 10);
                        //396 Starly : 33 Tackle : 45 Growl - later pairs
                        poolSpawn.Spawns.Add(GetTeamMob("starly", "", "tackle", "growl", "", "", new RandRange(6), "wander_dumb"), new IntRange(0, 4), 10);
                        poolSpawn.SpecificSpawns.Add(new SpecificTeamSpawner(GetGenericMob("starly", "", "tackle", "growl", "", "", new RandRange(7), "wander_dumb"), GetGenericMob("starly", "", "tackle", "growl", "", "", new RandRange(7), "wander_dumb")), new IntRange(4, 7), 20);

                        poolSpawn.TeamSizes.Add(1, new IntRange(0, 7), 12);
                        floorSegment.ZoneSteps.Add(poolSpawn);


                        RandBag<IGenPriority> npcZoneSpawns = new RandBag<IGenPriority>();
                        npcZoneSpawns.RemoveOnRoll = true;
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
                            npcZoneSpawns.ToSpawn.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_SPAWN_MOBS_EXTRA, randomSpawn));
                        }
                        //Song
                        {
                            PresetMultiTeamSpawner<ListMapGenContext> multiTeamSpawner = new PresetMultiTeamSpawner<ListMapGenContext>();
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID("loudred", 0, "normal", Gender.Male);
                            post_mob.Tactic = "slow_patrol";
                            post_mob.Level = new RandRange(21);
                            post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_ADVICE_FADED"))));
                            SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);
                            post_team.Explorer = true;
                            multiTeamSpawner.Spawns.Add(post_team);
                            PlaceRandomMobsStep<ListMapGenContext> randomSpawn = new PlaceRandomMobsStep<ListMapGenContext>(multiTeamSpawner);
                            randomSpawn.Ally = true;
                            npcZoneSpawns.ToSpawn.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_SPAWN_MOBS_EXTRA, randomSpawn));
                        }
                        //Aipom and wonder tiles
                        {
                            PresetMultiTeamSpawner<ListMapGenContext> multiTeamSpawner = new PresetMultiTeamSpawner<ListMapGenContext>();
                            MobSpawn post_mob = new MobSpawn();
                            post_mob.BaseForm = new MonsterID("machop", 0, "normal", Gender.Male);
                            post_mob.Tactic = "slow_wander";
                            post_mob.Level = new RandRange(14);
                            post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_ADVICE_STAT_DROP"))));
                            SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);
                            post_team.Explorer = true;
                            multiTeamSpawner.Spawns.Add(post_team);
                            PlaceRandomMobsStep<ListMapGenContext> randomSpawn = new PlaceRandomMobsStep<ListMapGenContext>(multiTeamSpawner);
                            randomSpawn.Ally = true;
                            npcZoneSpawns.ToSpawn.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_SPAWN_MOBS_EXTRA, randomSpawn));
                        }
                        SpreadStepZoneStep npcZoneStep = new SpreadStepZoneStep(new SpreadPlanQuota(new RandRange(2), new IntRange(1, 5), true), npcZoneSpawns);
                        floorSegment.ZoneSteps.Add(npcZoneStep);


                        TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                        tileSpawn.Priority = PR_RESPAWN_TRAP;
                        floorSegment.ZoneSteps.Add(tileSpawn);


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
                            AddMoneyData(layout, new RandRange(1, 4));

                            //items
                            AddItemData(layout, new RandRange(2, 5), 25);

                            //enemies! ~ lv 5 to 10
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
                                    SwitchSealStep<MapGenContext> vaultStep = new SwitchSealStep<MapGenContext>("sealed_block", "tile_switch", false);
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
                        MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 10);
                        moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                        floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                        //items
                        ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                        itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;

                        //necessities
                        CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                        necessities.SpawnRates.SetRange(14, new IntRange(0, 3));
                        itemSpawnZoneStep.Spawns.Add("necessities", necessities);

                        necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, 3), 9);//Leppa
                        necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, 3), 12);//Oran
                        necessities.Spawns.Add(new InvItem("food_apple"), new IntRange(0, 3), 10);//Apple
                        necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, 3), 10);//Lum
                        necessities.Spawns.Add(new InvItem("seed_reviver"), new IntRange(0, 3), 5);//reviver seed

                        //snacks
                        CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                        snacks.SpawnRates.SetRange(10, new IntRange(0, 3));
                        itemSpawnZoneStep.Spawns.Add("snacks", snacks);

                        snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, 3), 20);//blast seed
                        snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, 3), 10);//warp seed
                        snacks.Spawns.Add(new InvItem("seed_sleep"), new IntRange(0, 3), 10);//sleep seed

                        //wands
                        CategorySpawn<InvItem> ammo = new CategorySpawn<InvItem>();
                        ammo.SpawnRates.SetRange(10, new IntRange(0, 3));
                        itemSpawnZoneStep.Spawns.Add("ammo", ammo);

                        ammo.Spawns.Add(new InvItem("ammo_stick", false, 3), new IntRange(0, 3), 10);//stick
                        ammo.Spawns.Add(new InvItem("wand_whirlwind", false, 2), new IntRange(0, 3), 10);//whirlwind wand
                        ammo.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, 3), 10);//pounce wand
                        ammo.Spawns.Add(new InvItem("wand_warp", false, 1), new IntRange(0, 3), 10);//warp wand
                        ammo.Spawns.Add(new InvItem("wand_lob", false, 2), new IntRange(0, 3), 10);//lob wand
                        ammo.Spawns.Add(new InvItem("ammo_geo_pebble", false, 2), new IntRange(0, 3), 10);//Geo Pebble

                        //orbs
                        CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                        orbs.SpawnRates.SetRange(10, new IntRange(0, 3));
                        itemSpawnZoneStep.Spawns.Add("orbs", orbs);

                        orbs.Spawns.Add(new InvItem("orb_rebound"), new IntRange(0, 3), 10);//Rebound
                        orbs.Spawns.Add(new InvItem("orb_all_protect"), new IntRange(0, 3), 5);//All Protect
                        orbs.Spawns.Add(new InvItem("orb_luminous"), new IntRange(0, 3), 9);//Luminous
                        orbs.Spawns.Add(new InvItem("orb_mirror"), new IntRange(0, 3), 8);//Mirror Orb

                        //held items
                        CategorySpawn<InvItem> heldItems = new CategorySpawn<InvItem>();
                        heldItems.SpawnRates.SetRange(2, new IntRange(0, 3));
                        itemSpawnZoneStep.Spawns.Add("held", heldItems);

                        heldItems.Spawns.Add(new InvItem("held_power_band"), new IntRange(0, 3), 1);//Power Band
                        heldItems.Spawns.Add(new InvItem("held_special_band"), new IntRange(0, 3), 1);//Special Band
                        heldItems.Spawns.Add(new InvItem("held_defense_scarf"), new IntRange(0, 3), 1);//Defense Scarf

                        //special
                        CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                        special.SpawnRates.SetRange(7, new IntRange(0, 3));
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
                        //161 Sentret : 10 Scratch : 111 Defense Curl
                        poolSpawn.Spawns.Add(GetTeamMob("sentret", "", "scratch", "defense_curl", "", "", new RandRange(10), "wander_dumb"), new IntRange(0, 3), 10);
                        //161 Sentret : Charm : Bite
                        poolSpawn.Spawns.Add(GetTeamMob("snubbull", "", "charm", "bite", "", "", new RandRange(10), "wander_dumb"), new IntRange(0, 3), 10);
                        //396 Starly : 33 Tackle : 45 Growl - later pairs
                        poolSpawn.SpecificSpawns.Add(new SpecificTeamSpawner(GetGenericMob("starly", "", "tackle", "growl", "", "", new RandRange(7), "wander_dumb"), GetGenericMob("starly", "", "tackle", "growl", "", "", new RandRange(7), "wander_dumb")), new IntRange(0, 3), 20);
                        //438 Bonsly : 88 Rock Throw
                        poolSpawn.Spawns.Add(GetTeamMob("bonsly", "", "rock_throw", "", "", "", new RandRange(9), "weird_tree"), new IntRange(0, 3), 8);

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

                            AddWaterSteps(layout, "floor", new RandRange(20));//empty


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

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
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
                    zone.Rogue = RogueStatus.ItemTransfer;

                    int max_floors = 10;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Flyaway Cliffs\n{0}F")));

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
                    poolSpawn.Spawns.Add(GetTeamMob("hoppip", "", "tail_whip", "splash", "synthesis", "", new RandRange(16), TeamMemberSpawn.MemberRole.Support, "wander_dumb"), new IntRange(2, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("lickitung", "", "wrap", "", "", "", new RandRange(17), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("hoothoot", "", "foresight", "peck", "", "", new RandRange(16), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("wingull", "", "growl", "quick_attack", "", "", new RandRange(18), "wander_dumb"), new IntRange(2, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("spinda", "", "dizzy_punch", "", "", "", new RandRange(18), "wander_dumb"), new IntRange(6, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("farfetchd", "defiant", "aerial_ace", "knock_off", "", "", new RandRange(20), TeamMemberSpawn.MemberRole.Loner, "wander_dumb"), new IntRange(6, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("staravia", "", "double_team", "wing_attack", "", "", new RandRange(19), "wander_dumb"), new IntRange(6, max_floors), 10);
                    //Increase the team size to 2
                    poolSpawn.Spawns.Add(GetTeamMob("chatot", "", "round", "", "", "", new RandRange(16), TeamMemberSpawn.MemberRole.Support, "wander_dumb"), new IntRange(6, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("skarmory", "", "metal_claw", "air_cutter", "", "", new RandRange(20), "wander_dumb"), new IntRange(6, max_floors), 5);

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, 10), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);

                    RandBag<IGenPriority> npcZoneSpawns = new RandBag<IGenPriority>();
                    npcZoneSpawns.RemoveOnRoll = true;
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
                        npcZoneSpawns.ToSpawn.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_SPAWN_MOBS_EXTRA, randomSpawn));
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
                        npcZoneSpawns.ToSpawn.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_SPAWN_MOBS_EXTRA, randomSpawn));
                    }
                    SpreadStepZoneStep npcZoneStep = new SpreadStepZoneStep(new SpreadPlanQuota(new RandRange(2), new IntRange(0, 8), true), npcZoneSpawns);
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


                    //HouseChanceZoneStep chestChanceZoneStep = new HouseChanceZoneStep(20, new RangeSpawn(5, 29));
                    //floorSegment.ZoneSteps.Add(chestChanceZoneStep);

                    //HouseChanceZoneStep monsterChanceZoneStep = new HouseChanceZoneStep(20, new RangeSpawn(5, 29));
                    //floorSegment.ZoneSteps.Add(monsterChanceZoneStep);

                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        if (ii < 2)
                            AddFloorData(layout, "B07. Flyaway Cliffs.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Clear);
                        else
                            AddFloorData(layout, "B07. Flyaway Cliffs.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

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
                            AddTextureData(layout, "hidden_land_wall", "hidden_land_floor", "hidden_land_secondary", "flying");
                        else
                            AddSpecificTextureData(layout, "hidden_highland_wall", "hidden_highland_floor", "hidden_highland_secondary", "tall_grass", "flying");

                        //money - 765P to 3,315P
                        AddMoneyData(layout, new RandRange(1, 4));

                        //items
                        AddItemData(layout, new RandRange(3, 5), 25);

                        //enemies! ~ lv 10 to 20
                        AddRespawnData(layout, 3, 80);

                        //enemies
                        AddEnemySpawnData(layout, 20, new RandRange(2, 4));

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

                        floorSegment.Floors.Add(layout);
                    }
                    zone.Segments.Add(floorSegment);
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
                    zone.Rogue = RogueStatus.ItemTransfer;

                    int max_floors = 14;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Thunderstruck Pass\n{0}F")));

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
                    poolSpawn.Spawns.Add(GetTeamMob("plusle", "", "electro_ball", "play_nice", "", "", new RandRange(30), "wander_dumb"), new IntRange(0, 10), 10);
                    //Version Excl
                    poolSpawn.Spawns.Add(GetTeamMob("minun", "", "electro_ball", "play_nice", "", "", new RandRange(30), "wander_dumb"), new IntRange(0, 10), 10);
                    //Version Excl
                    poolSpawn.Spawns.Add(GetTeamMob("magnemite", "", "thunder_shock", "thunder_wave", "", "", new RandRange(26), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("raichu", "", "thunderbolt", "", "", "", new RandRange(28), "wander_dumb"), new IntRange(10, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("voltorb", "", "eerie_impulse", "charge_beam", "", "", new RandRange(26), "wander_dumb"), new IntRange(0, 10), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("hitmonchan", "", "focus_punch", "agility", "", "", new RandRange(26), "wander_dumb"), new IntRange(5, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("geodude", 1, "", Gender.Unknown), "", "spark", "defense_curl", "", "", new RandRange(24), "wander_dumb"), new IntRange(0, 5), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("loudred", "", "screech", "echoed_voice", "", "", new RandRange(26), "wander_dumb"), new IntRange(0, 10), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("hariyama", "", "fake_out", "smelling_salts", "", "", new RandRange(30), "wander_dumb"), new IntRange(5, max_floors), 10);
                    //Sleeping, holding wide lens
                    poolSpawn.Spawns.Add(GetTeamMob("jolteon", "", "thunder", "agility", "shadow_ball", "", new RandRange(55), TeamMemberSpawn.MemberRole.Loner, "wander_normal"), new IntRange(0, max_floors), 10);

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

                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

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
                        AddTrapsSteps(layout, new RandRange(6, 9));

                        //money - 3,036P to 6,210P
                        AddMoneyData(layout, new RandRange(2, 4));

                        //enemies! ~ lv 20 to 32
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

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
                #endregion
            }
            else if (index == 6)
            {
                #region VEILED RIDGE
                {
                    zone.Name = new LocalText("Veiled Ridge");
                    zone.Rescues = 2;
                    zone.Level = 35;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 16;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Veiled Ridge\n{0}F")));

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
                        necessities.Spawns.Add(new InvItem("food_grimy"), new IntRange(0, max_floors), 10);
                        //snacks
                        CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                        snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                        snacks.Spawns.Add(new InvItem("berry_jaboca"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_ice"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("herb_white"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_last_chance"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_decoy"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("berry_colbur"), new IntRange(0, max_floors), 5);
                        snacks.Spawns.Add(new InvItem("berry_chople"), new IntRange(0, max_floors), 5);
                        //boosters
                        CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                        boosters.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("boosters", boosters);


                        boosters.Spawns.Add(new InvItem("gummi_blue"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_black"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_clear"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_grass"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_green"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_brown"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_orange"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_gold"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 5);
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


                        special.Spawns.Add(new InvItem("apricorn_big", true), new IntRange(0, max_floors), 3);
                        special.Spawns.Add(new InvItem("apricorn_big"), new IntRange(0, max_floors), 7);
                        special.Spawns.Add(new InvItem("apricorn_black", true), new IntRange(0, max_floors), 3);
                        special.Spawns.Add(new InvItem("apricorn_black"), new IntRange(0, max_floors), 7);
                        special.Spawns.Add(new InvItem("apricorn_purple", true), new IntRange(0, max_floors), 3);
                        special.Spawns.Add(new InvItem("apricorn_purple"), new IntRange(0, max_floors), 7);
                        special.Spawns.Add(new InvItem("machine_assembly_box", true), new IntRange(0, max_floors), 3);
                        special.Spawns.Add(new InvItem("machine_assembly_box"), new IntRange(0, max_floors), 7);
                        //throwable
                        CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                        throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                        throwable.Spawns.Add(new InvItem("ammo_gravelerock", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("ammo_silver_spike", false, 3), new IntRange(0, max_floors), 5);
                        throwable.Spawns.Add(new InvItem("wand_vanish", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_fear", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_slow", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_lure", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, max_floors), 10);
                        //orbs
                        CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                        orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                        orbs.Spawns.Add(new InvItem("orb_mug", true), new IntRange(0, max_floors), 2);
                        orbs.Spawns.Add(new InvItem("orb_mug"), new IntRange(0, max_floors), 8);
                        orbs.Spawns.Add(new InvItem("orb_rollcall", true), new IntRange(0, max_floors), 2);
                        orbs.Spawns.Add(new InvItem("orb_rollcall"), new IntRange(0, max_floors), 8);
                        orbs.Spawns.Add(new InvItem("orb_nullify", true), new IntRange(0, max_floors), 2);
                        orbs.Spawns.Add(new InvItem("orb_nullify"), new IntRange(0, max_floors), 8);
                        orbs.Spawns.Add(new InvItem("orb_luminous", true), new IntRange(0, max_floors), 2);
                        orbs.Spawns.Add(new InvItem("orb_luminous"), new IntRange(0, max_floors), 8);
                        orbs.Spawns.Add(new InvItem("orb_cleanse"), new IntRange(0, max_floors), 10);
                        //held
                        CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                        held.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("held", held);


                        held.Spawns.Add(new InvItem("held_grip_claw", true), new IntRange(0, max_floors), 3);
                        held.Spawns.Add(new InvItem("held_grip_claw"), new IntRange(0, max_floors), 7);
                        held.Spawns.Add(new InvItem("held_warp_scarf", true), new IntRange(0, max_floors), 3);
                        held.Spawns.Add(new InvItem("held_warp_scarf"), new IntRange(0, max_floors), 7);
                        held.Spawns.Add(new InvItem("held_cover_band", true), new IntRange(0, max_floors), 3);
                        held.Spawns.Add(new InvItem("held_cover_band"), new IntRange(0, max_floors), 7);
                        held.Spawns.Add(new InvItem("held_expert_belt", true), new IntRange(0, max_floors), 3);
                        held.Spawns.Add(new InvItem("held_expert_belt"), new IntRange(0, max_floors), 7);
                        held.Spawns.Add(new InvItem("held_never_melt_ice", true), new IntRange(0, max_floors), 3);
                        held.Spawns.Add(new InvItem("held_never_melt_ice"), new IntRange(0, max_floors), 7);
                        held.Spawns.Add(new InvItem("held_black_glasses", true), new IntRange(0, max_floors), 3);
                        held.Spawns.Add(new InvItem("held_black_glasses"), new IntRange(0, max_floors), 7);
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


                        poolSpawn.Spawns.Add(GetTeamMob("zorua", "", "foul_play", "", "", "", new RandRange(25), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("koffing", "levitate", "self_destruct", "haze", "", "", new RandRange(28), "wander_dumb"), new IntRange(0, 8), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("pawniard", "", "torment", "slash", "", "", new RandRange(30), "wander_dumb"), new IntRange(0, 8), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("absol", "", "future_sight", "night_slash", "", "", new RandRange(33), "wander_dumb"), new IntRange(8, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("scrafty", "", "beat_up", "feint_attack", "", "", new RandRange(30), "wander_dumb"), new IntRange(8, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("mightyena", "", "thief", "scary_face", "", "", new RandRange(31), "thief"), new IntRange(4, 12), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("ninetales", "", "imprison", "flamethrower", "", "", new RandRange(30), "wander_dumb"), new IntRange(4, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("sableye", "", "knock_off", "detect", "", "", new RandRange(30), "wander_dumb"), new IntRange(8, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("masquerain", "", "ominous_wind", "stun_spore", "", "", new RandRange(30), "wander_dumb"), new IntRange(0, 8), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("graveler", "", "rock_blast", "", "", "", new RandRange(32), "wander_dumb"), new IntRange(0, 12), 10);
                        //spawns asleep with pearl, if initial
                        poolSpawn.Spawns.Add(GetTeamMob("grumpig", "", "magic_coat", "zen_headbutt", "", "", new RandRange(30), "wander_normal"), new IntRange(4, max_floors), 10);

                        poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                        poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 6);

                        floorSegment.ZoneSteps.Add(poolSpawn);

                        TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                        tileSpawn.Priority = PR_RESPAWN_TRAP;


                        tileSpawn.Spawns.Add(new EffectTile("trap_pp_leech", true), new IntRange(0, max_floors), 10);//pp-leech trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, max_floors), 10);//chestnut trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_spin", false), new IntRange(0, max_floors), 10);//spin trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_grimy", false), new IntRange(0, max_floors), 10);//grimy trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_sticky", false), new IntRange(0, max_floors), 10);//sticky trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap


                        floorSegment.ZoneSteps.Add(tileSpawn);

                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            if (ii < 12)
                                AddFloorData(layout, "B32. Veiled Ridge.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);
                            else
                                AddFloorData(layout, "B32. Veiled Ridge.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                            //Tilesets
                            // other candidates: dark_hill_2,steel_aegis_cave,sealed_ruin_wall
                            if (ii < 4)
                                AddSpecificTextureData(layout, "rock_path_tds_wall", "rock_path_tds_floor", "rock_path_tds_secondary", "tall_grass_dark", "dark");
                            else if (ii < 8)
                                AddSpecificTextureData(layout, "dark_hill_2_wall", "dark_hill_2_floor", "dark_hill_2_secondary", "tall_grass_dark", "dark");
                            else
                                AddSpecificTextureData(layout, "dark_hill_1_wall", "dark_hill_1_floor", "dark_hill_1_secondary", "tall_grass_dark", "dark");

                            if (ii >= 10)
                                AddWaterSteps(layout, "water", new RandRange(20));//water

                            AddGrassSteps(layout, new RandRange(2, 5), new IntRange(2, 7), new RandRange(30));

                            //traps
                            AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                            AddTrapsSteps(layout, new RandRange(6, 9));

                            //money - 5,278P to 10,353P
                            AddMoneyData(layout, new RandRange(2, 4));

                            //enemies! ~ lv 32 to 45
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


                            floorSegment.Floors.Add(layout);
                        }
                        zone.Segments.Add(floorSegment);
                    }


                    {
                        int max_floors = 6;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Illusion Ridge\nB{0}F")));

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


                        snacks.Spawns.Add(new InvItem("berry_jaboca"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_ice"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("herb_white"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_last_chance"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_decoy"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("berry_colbur"), new IntRange(0, max_floors), 5);
                        snacks.Spawns.Add(new InvItem("berry_chople"), new IntRange(0, max_floors), 5);
                        //boosters
                        CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                        boosters.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("boosters", boosters);


                        boosters.Spawns.Add(new InvItem("gummi_blue"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_black"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_clear"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_grass"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_green"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_brown"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_orange"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_gold"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 5);
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


                        special.Spawns.Add(new InvItem("apricorn_big", true), new IntRange(0, max_floors), 3);
                        special.Spawns.Add(new InvItem("apricorn_big"), new IntRange(0, max_floors), 7);
                        special.Spawns.Add(new InvItem("apricorn_black", true), new IntRange(0, max_floors), 3);
                        special.Spawns.Add(new InvItem("apricorn_black"), new IntRange(0, max_floors), 7);
                        special.Spawns.Add(new InvItem("apricorn_purple", true), new IntRange(0, max_floors), 3);
                        special.Spawns.Add(new InvItem("apricorn_purple"), new IntRange(0, max_floors), 7);
                        special.Spawns.Add(new InvItem("machine_assembly_box", true), new IntRange(0, max_floors), 3);
                        special.Spawns.Add(new InvItem("machine_assembly_box"), new IntRange(0, max_floors), 7);
                        //throwable
                        CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                        throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                        throwable.Spawns.Add(new InvItem("ammo_gravelerock", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("ammo_silver_spike", false, 3), new IntRange(0, max_floors), 5);
                        throwable.Spawns.Add(new InvItem("wand_vanish", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_fear", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_slow", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_lure", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, max_floors), 10);
                        //orbs
                        CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                        orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                        orbs.Spawns.Add(new InvItem("orb_mug", true), new IntRange(0, max_floors), 2);
                        orbs.Spawns.Add(new InvItem("orb_mug"), new IntRange(0, max_floors), 8);
                        orbs.Spawns.Add(new InvItem("orb_rollcall", true), new IntRange(0, max_floors), 2);
                        orbs.Spawns.Add(new InvItem("orb_rollcall"), new IntRange(0, max_floors), 8);
                        orbs.Spawns.Add(new InvItem("orb_nullify", true), new IntRange(0, max_floors), 2);
                        orbs.Spawns.Add(new InvItem("orb_nullify"), new IntRange(0, max_floors), 8);
                        orbs.Spawns.Add(new InvItem("orb_luminous", true), new IntRange(0, max_floors), 2);
                        orbs.Spawns.Add(new InvItem("orb_luminous"), new IntRange(0, max_floors), 8);
                        orbs.Spawns.Add(new InvItem("orb_cleanse"), new IntRange(0, max_floors), 10);
                        //held
                        CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                        held.SpawnRates.SetRange(4, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("held", held);


                        held.Spawns.Add(new InvItem("held_choice_scarf", true), new IntRange(0, max_floors), 3);
                        held.Spawns.Add(new InvItem("held_choice_scarf"), new IntRange(0, max_floors), 7);
                        held.Spawns.Add(new InvItem("held_warp_scarf", true), new IntRange(0, max_floors), 3);
                        held.Spawns.Add(new InvItem("held_warp_scarf"), new IntRange(0, max_floors), 7);
                        held.Spawns.Add(new InvItem("held_cover_band", true), new IntRange(0, max_floors), 3);
                        held.Spawns.Add(new InvItem("held_cover_band"), new IntRange(0, max_floors), 7);
                        held.Spawns.Add(new InvItem("held_expert_belt", true), new IntRange(0, max_floors), 3);
                        held.Spawns.Add(new InvItem("held_expert_belt"), new IntRange(0, max_floors), 7);
                        held.Spawns.Add(new InvItem("held_never_melt_ice", true), new IntRange(0, max_floors), 3);
                        held.Spawns.Add(new InvItem("held_never_melt_ice"), new IntRange(0, max_floors), 7);
                        held.Spawns.Add(new InvItem("held_black_glasses", true), new IntRange(0, max_floors), 3);
                        held.Spawns.Add(new InvItem("held_black_glasses"), new IntRange(0, max_floors), 7);
                        //tms
                        CategorySpawn<InvItem> tms = new CategorySpawn<InvItem>();
                        tms.SpawnRates.SetRange(10, new IntRange(0, max_floors));
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


                        poolSpawn.Spawns.Add(GetTeamMob("absol", "", "future_sight", "night_slash", "", "", new RandRange(33), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("mightyena", "", "thief", "scary_face", "", "", new RandRange(30), "thief"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("pawniard", "", "torment", "slash", "", "", new RandRange(30), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("masquerain", "", "ominous_wind", "stun_spore", "", "", new RandRange(30), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("sneasel", "pickpocket", "agility", "quick_attack", "", "", new RandRange(33), "thief"), new IntRange(0, max_floors), 10);
                        //spawns with pearl, if initial spawn, sleeping
                        poolSpawn.Spawns.Add(GetTeamMob("grumpig", "", "magic_coat", "zen_headbutt", "", "", new RandRange(30), "wander_dumb"), new IntRange(0, max_floors), 10);
                        //spawns with sticky item
                        poolSpawn.Spawns.Add(GetTeamMob("persian", "", "switcheroo", "feint_attack", "swift", "", new RandRange(33), "thief"), new IntRange(0, max_floors), 10);
                        //sleeping, disguised as grumpig
                        poolSpawn.Spawns.Add(GetTeamMob("zoroark", "", "night_daze", "u_turn", "agility", "", new RandRange(57), "wander_normal"), new IntRange(0, max_floors), 10);


                        poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                        poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 6);


                        floorSegment.ZoneSteps.Add(poolSpawn);

                        TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                        tileSpawn.Priority = PR_RESPAWN_TRAP;

                        tileSpawn.Spawns.Add(new EffectTile("trap_pp_leech", true), new IntRange(0, max_floors), 10);//pp-leech trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, max_floors), 10);//chestnut trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_spin", false), new IntRange(0, max_floors), 10);//spin trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_grimy", false), new IntRange(0, max_floors), 10);//grimy trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_sticky", false), new IntRange(0, max_floors), 10);//sticky trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap

                        floorSegment.ZoneSteps.Add(tileSpawn);


                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            AddFloorData(layout, "B12. Sickly Hollow.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                            //Tilesets
                            //other options: craggy_peak, sky_peak_summit_pass
                            AddTextureData(layout, "spacial_rift_2_wall", "spacial_rift_2_floor", "spacial_rift_2_secondary", "dark");

                            if (ii >= 6)
                                AddWaterSteps(layout, "lava", new RandRange(25));//lava

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

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 7)
            {
                #region CHAMPION'S ROAD
                {
                    zone.Name = new LocalText("Champion's Road");
                    zone.Rescues = 2;
                    zone.Level = 45;
                    zone.NoEXP = true;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    int max_floors = 22;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Champion's Road\n{0}F")));

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
                    snacks.Spawns.Add(new InvItem("berry_charti", true), new IntRange(0, max_floors), 0);
                    snacks.Spawns.Add(new InvItem("berry_charti"), new IntRange(0, max_floors), 2);
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
                    throwable.Spawns.Add(new InvItem("wand_purge", false, 3), new IntRange(0, max_floors), 10);
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
                    orbs.Spawns.Add(new InvItem("orb_mobile", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_mobile"), new IntRange(0, max_floors), 8);
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

                    poolSpawn.Spawns.Add(GetTeamMob("ursaring", "", "hammer_arm", "thrash", "", "", new RandRange(38), "wander_normal"), new IntRange(0, 3), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("skarmory", "", "steel_wing", "spikes", "", "", new RandRange(38), "wander_normal"), new IntRange(0, 3), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("staraptor", "", "close_combat", "brave_bird", "", "", new RandRange(38), "wander_normal"), new IntRange(0, 3), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("mamoswine", "", "ancient_power", "thrash", "", "", new RandRange(38), "wander_normal"), new IntRange(0, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("heracross", "", "megahorn", "brick_break", "", "", new RandRange(38), "wander_normal"), new IntRange(0, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("florges", "", "petal_blizzard", "flower_shield", "", "", new RandRange(39), "wander_normal"), new IntRange(3, 9), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("machamp", "no_guard", "dynamic_punch", "", "", "", new RandRange(39), "wander_normal"), new IntRange(3, 9), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("golem", "", "stone_edge", "rock_blast", "", "", new RandRange(39), "wander_normal"), new IntRange(3, 9), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("sunflora", "", "solar_beam", "ingrain", "", "", new RandRange(39), "wander_normal"), new IntRange(3, 9), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("gallade", "", "psycho_cut", "teleport", "", "", new RandRange(39), "wander_normal"), new IntRange(6, 12), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("gardevoir", "", "psychic", "wish", "", "", new RandRange(39), "wander_normal"), new IntRange(6, 12), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("magnezone", "", "discharge", "magnet_bomb", "", "", new RandRange(39), "wander_normal"), new IntRange(6, 12), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("drapion", "", "poison_jab", "sucker_punch", "", "", new RandRange(39), "wander_normal"), new IntRange(6, 12), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("flygon", "", "earthquake", "dragon_tail", "", "", new RandRange(40), TeamMemberSpawn.MemberRole.Loner, "wander_normal"), new IntRange(9, 15), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("ledian", "", "reflect", "light_screen", "silver_wind", "", new RandRange(40), "wander_normal"), new IntRange(9, 15), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("exploud", "", "hyper_voice", "", "", "", new RandRange(40), "wander_normal"), new IntRange(9, 15), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("starmie", "", "hydro_pump", "recover", "swift", "", new RandRange(40), "wander_normal"), new IntRange(9, 15), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("mismagius", "", "shadow_ball", "mystical_fire", "", "", new RandRange(40), "wander_normal"), new IntRange(12, 18), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("steelix", "", "iron_tail", "double_edge", "", "", new RandRange(40), "wander_normal"), new IntRange(12, 18), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("azumarill", "", "play_rough", "aqua_ring", "", "", new RandRange(40), "wander_normal"), new IntRange(12, 18), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("swalot", "", "sludge_bomb", "encore", "", "", new RandRange(40), "wander_normal"), new IntRange(12, 18), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("manectric", "lightning_rod", "thunder", "", "", "", new RandRange(41), "wander_normal"), new IntRange(15, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("electivire", "", "thunderbolt", "", "", "", new RandRange(41), "wander_normal"), new IntRange(15, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("magmortar", "", "fire_blast", "", "", "", new RandRange(41), "wander_normal"), new IntRange(15, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("clefable", "", "cosmic_power", "moonblast", "", "", new RandRange(41), "wander_normal"), new IntRange(15, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("eiscue", "", "blizzard", "surf", "", "", new RandRange(42), "wander_normal"), new IntRange(18, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("salamence", "", "dragon_claw", "fly", "", "", new RandRange(42), "wander_normal"), new IntRange(18, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("drifblim", "aftermath", "ominous_wind", "explosion", "", "", new RandRange(42), "wander_normal"), new IntRange(18, max_floors), 10);
                    //show up in alternating floors
                    poolSpawn.Spawns.Add(GetTeamMob("emboar", "", "fire_pledge", "heat_crash", "", "", new RandRange(43), "wander_normal"), new IntRange(9, max_floors), 10);
                    //until floor 19+
                    poolSpawn.Spawns.Add(GetTeamMob("samurott", "", "water_pledge", "razor_shell", "", "", new RandRange(43), "wander_normal"), new IntRange(9, max_floors), 10);
                    //when they all show up
                    poolSpawn.Spawns.Add(GetTeamMob("serperior", "", "grass_pledge", "leaf_blade", "", "", new RandRange(43), "wander_normal"), new IntRange(9, max_floors), 10);


                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 6);
                    poolSpawn.TeamSizes.Add(3, new IntRange(15, max_floors), 3);


                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    floorSegment.ZoneSteps.Add(tileSpawn);


                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        AddFloorData(layout, "B14. Champion Road.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                        //Tilesets
                        if (ii < 12)
                            AddSpecificTextureData(layout, "northwind_field_wall", "northwind_field_floor", "northwind_field_secondary", "tall_grass_dark", "fairy");
                        else if (ii < 18)
                            AddSpecificTextureData(layout, "craggy_peak_wall", "craggy_peak_floor", "craggy_peak_secondary", "tall_grass_dark", "steel");
                        else
                            AddSpecificTextureData(layout, "sky_ruins_area_wall", "sky_ruins_floor", "sky_ruins_secondary", "tall_grass_dark", "flying", true);

                        if (ii >= 6)
                            AddWaterSteps(layout, "pit", new RandRange(25));//pit

                        if (ii >= 12)
                            AddGrassSteps(layout, new RandRange(0, 6), new IntRange(3, 9), new RandRange(30));

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(6, 9));

                        //money - 7,650P to 19,380P
                        AddMoneyData(layout, new RandRange(2, 5));

                        //enemies! ~ lv 45 to 55
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


                        floorSegment.Floors.Add(layout);
                    }
                    zone.Segments.Add(floorSegment);
                }
                #endregion
            }
            else if (index == 8)
                FillCaveOfWhispers(zone);
            else if (index == 9)
            {
                #region MOONLIT COURTYARD
                {
                    zone.Name = new LocalText("Moonlit Courtyard");
                    zone.Rescues = 4;
                    zone.Level = 25;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 14;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Moonlit Courtyard\n{0}F")));

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
                        //snacks
                        CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                        snacks.SpawnRates.SetRange(5, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                        snacks.Spawns.Add(new InvItem("berry_roseli"), new IntRange(0, max_floors), 3);
                        snacks.Spawns.Add(new InvItem("berry_chilan"), new IntRange(0, max_floors), 3);
                        snacks.Spawns.Add(new InvItem("berry_rindo"), new IntRange(0, max_floors), 3);
                        //boosters
                        CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                        boosters.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("boosters", boosters);


                        boosters.Spawns.Add(new InvItem("gummi_blue"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_black"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_clear"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_grass"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_green"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_brown"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_orange"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_gold"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_red"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_royal"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_silver"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_white"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_yellow"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_sky"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_gray"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_magenta"), new IntRange(0, max_floors), 5);
                        //special
                        CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                        special.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("special", special);


                        special.Spawns.Add(new InvItem("apricorn_plain"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_green"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_purple"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_white"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("machine_assembly_box"), new IntRange(0, max_floors), 10);
                        //evo
                        CategorySpawn<InvItem> evo = new CategorySpawn<InvItem>();
                        evo.SpawnRates.SetRange(1, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("evo", evo);


                        evo.Spawns.Add(new InvItem("evo_moon_stone"), new IntRange(0, max_floors), 10);
                        //throwable
                        CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                        throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                        throwable.Spawns.Add(new InvItem("ammo_cacnea_spike", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("ammo_corsola_twig", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_fear", false, 2), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_lure", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_warp", false, 3), new IntRange(0, max_floors), 10);
                        //orbs
                        CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                        orbs.SpawnRates.SetRange(20, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                        orbs.Spawns.Add(new InvItem("orb_trapbust"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_foe_hold"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_all_dodge"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_spurn"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_petrify"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_totter"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_scanner"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_freeze"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_all_protect"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_slow"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_halving"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_rollcall"), new IntRange(0, max_floors), 10);
                        //held
                        CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                        held.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("held", held);


                        held.Spawns.Add(new InvItem("held_heal_ribbon"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_warp_scarf"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_twist_band"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_shell_bell"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_pink_bow"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_twisted_spoon"), new IntRange(0, max_floors), 10);
                        //tms
                        CategorySpawn<InvItem> tms = new CategorySpawn<InvItem>();
                        tms.SpawnRates.SetRange(7, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("tms", tms);


                        tms.Spawns.Add(new InvItem("tm_protect"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_round"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_rest"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_hidden_power"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_thief"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_dig"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_cut"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_grass_knot"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_fly"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_infestation"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_work_up"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_roar"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_flash"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_bullet_seed"), new IntRange(0, max_floors), 10);


                        floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                        //mobs
                        TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                        poolSpawn.Priority = PR_RESPAWN_MOB;


                        poolSpawn.Spawns.Add(GetTeamMob("clefairy", "cute_charm", "follow_me", "disarming_voice", "", "", new RandRange(23), "wander_dumb"), new IntRange(0, 10), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("gloom", "", "mega_drain", "moonlight", "", "", new RandRange(24), "wander_dumb"), new IntRange(5, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("cutiefly", "", "draining_kiss", "struggle_bug", "", "", new RandRange(22), "wander_dumb"), new IntRange(0, 5), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("ralts", "", "teleport", "growl", "", "", new RandRange(18), "wander_dumb"), new IntRange(0, 10), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("mime_jr", "", "copycat", "encore", "", "", new RandRange(23), "wander_dumb"), new IntRange(0, 5), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("roselia", "", "magical_leaf", "", "", "", new RandRange(22), "wander_dumb"), new IntRange(0, 6), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("bayleef", "", "magical_leaf", "synthesis", "", "", new RandRange(22), "wander_dumb"), new IntRange(5, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("lunatone", "", "embargo", "moonblast", "", "", new RandRange(23), "wander_dumb"), new IntRange(5, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("floette", "", "grassy_terrain", "wish", "", "", new RandRange(24), "wander_dumb"), new IntRange(5, max_floors), 10);
                        //groups.  maybe group turret?
                        poolSpawn.Spawns.Add(GetTeamMob("flabebe", "", "fairy_wind", "razor_leaf", "", "", new RandRange(16), "turret"), new IntRange(0, 10), 10);
                        poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("flabebe", 1, "", Gender.Unknown), "", "fairy_wind", "razor_leaf", "", "", new RandRange(16), "turret"), new IntRange(0, 10), 10);
                        poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("flabebe", 2, "", Gender.Unknown), "", "fairy_wind", "razor_leaf", "", "", new RandRange(16), "turret"), new IntRange(0, 10), 10);
                        poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("flabebe", 3, "", Gender.Unknown), "", "fairy_wind", "razor_leaf", "", "", new RandRange(16), "turret"), new IntRange(0, 10), 10);
                        poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("flabebe", 4, "", Gender.Unknown), "", "fairy_wind", "razor_leaf", "", "", new RandRange(16), "turret"), new IntRange(0, 10), 10);
                        //sleeping, with pass scarf
                        poolSpawn.Spawns.Add(GetTeamMob("umbreon", "", "moonlight", "confuse_ray", "assurance", "toxic", new RandRange(5), "wander_normal"), new IntRange(0, max_floors), 10);
                        //version exclusives
                        poolSpawn.Spawns.Add(GetTeamMob("volbeat", "", "flash", "struggle_bug", "", "", new RandRange(24), "wander_dumb"), new IntRange(5, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("illumise", "", "wish", "struggle_bug", "", "", new RandRange(24), "wander_dumb"), new IntRange(5, max_floors), 10);

                        poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                        poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 3);

                        floorSegment.ZoneSteps.Add(poolSpawn);

                        TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                        tileSpawn.Priority = PR_RESPAWN_TRAP;


                        tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_slumber", false), new IntRange(0, max_floors), 10);//sleep trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_seal", false), new IntRange(0, max_floors), 10);//seal trap


                        floorSegment.ZoneSteps.Add(tileSpawn);


                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            AddFloorData(layout, "B23. Shimmer Bay.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                            //Tilesets
                            if (ii < 5)
                                AddSpecificTextureData(layout, "moonlit_courtyard_wall", "moonlit_courtyard_floor", "moonlit_courtyard_secondary", "tall_grass_blue", "fairy");
                            else if (ii < 10)
                                AddSpecificTextureData(layout, "sky_ruins_area_wall", "moonlit_courtyard_floor", "moonlit_courtyard_secondary", "tall_grass_blue", "fairy", true);
                            else
                                AddSpecificTextureData(layout, "moonlit_courtyard_2_wall", "moonlit_courtyard_floor", "moonlit_courtyard_secondary", "tall_grass_blue", "fairy", true);

                            AddGrassSteps(layout, new RandRange(1, 6), new IntRange(3, 9), new RandRange(0));

                            //traps
                            AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                            AddTrapsSteps(layout, new RandRange(6, 9));

                            //money - 4,416P to 7,866P
                            AddMoneyData(layout, new RandRange(2, 4));

                            //enemies! ~ lv 20 to 45
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

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }


                    {
                        int max_floors = 6;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Moonlit Temple\nB{0}F")));

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


                        necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 9);
                        necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 12);
                        necessities.Spawns.Add(new InvItem("food_apple"), new IntRange(0, max_floors), 10);
                        necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 10);
                        //snacks
                        CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                        snacks.SpawnRates.SetRange(5, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                        snacks.Spawns.Add(new InvItem("berry_roseli"), new IntRange(0, max_floors), 3);
                        snacks.Spawns.Add(new InvItem("berry_chilan"), new IntRange(0, max_floors), 3);
                        snacks.Spawns.Add(new InvItem("berry_rindo"), new IntRange(0, max_floors), 3);
                        //boosters
                        CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                        boosters.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("boosters", boosters);


                        boosters.Spawns.Add(new InvItem("gummi_blue"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_black"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_clear"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_grass"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_green"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_brown"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_orange"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_gold"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_red"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_royal"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_silver"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_white"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_yellow"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_sky"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_gray"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_magenta"), new IntRange(0, max_floors), 5);
                        //special
                        CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                        special.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("special", special);


                        special.Spawns.Add(new InvItem("apricorn_plain"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_green"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_purple"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_white"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("machine_assembly_box"), new IntRange(0, max_floors), 10);
                        //evo
                        CategorySpawn<InvItem> evo = new CategorySpawn<InvItem>();
                        evo.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("evo", evo);


                        evo.Spawns.Add(new InvItem("evo_moon_stone"), new IntRange(0, max_floors), 10);
                        //throwable
                        CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                        throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                        throwable.Spawns.Add(new InvItem("ammo_cacnea_spike", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("ammo_corsola_twig", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_fear", false, 2), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_lure", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_warp", false, 3), new IntRange(0, max_floors), 10);
                        //orbs
                        CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                        orbs.SpawnRates.SetRange(20, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                        orbs.Spawns.Add(new InvItem("orb_trapbust"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_foe_hold"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_all_dodge"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_spurn"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_petrify"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_totter"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_scanner"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_freeze"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_all_protect"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_slow"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_halving"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_rollcall"), new IntRange(0, max_floors), 10);
                        //held
                        CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                        held.SpawnRates.SetRange(4, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("held", held);


                        held.Spawns.Add(new InvItem("held_heal_ribbon"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_warp_scarf"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_twist_band"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_shell_bell"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_pink_bow"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_twisted_spoon"), new IntRange(0, max_floors), 10);
                        //tms
                        CategorySpawn<InvItem> tms = new CategorySpawn<InvItem>();
                        tms.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("tms", tms);


                        tms.Spawns.Add(new InvItem("tm_protect"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_round"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_rest"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_hidden_power"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_thief"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_dig"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_cut"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_grass_knot"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_fly"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_infestation"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_work_up"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_roar"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_flash"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_bullet_seed"), new IntRange(0, max_floors), 10);


                        floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                        //mobs
                        TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                        poolSpawn.Priority = PR_RESPAWN_MOB;


                        poolSpawn.Spawns.Add(GetTeamMob("mr_mime", "", "quick_guard", "psybeam", "", "", new RandRange(26), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("kirlia", "", "heal_pulse", "disarming_voice", "", "", new RandRange(26), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("glameow", "", "fake_out", "fury_swipes", "", "", new RandRange(26), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("drowzee", "", "hypnosis", "meditate", "", "", new RandRange(24), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("wobbuffet", "", "counter", "safeguard", "", "", new RandRange(26), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("lunatone", "", "embargo", "moonblast", "", "", new RandRange(23), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("floette", "", "grassy_terrain", "wish", "", "", new RandRange(24), "wander_dumb"), new IntRange(0, max_floors), 10);
                        //sleeping, with assault vest
                        poolSpawn.Spawns.Add(GetTeamMob("sylveon", "pixilate", "moonblast", "shadow_ball", "swift", "", new RandRange(50), "wander_normal"), new IntRange(0, max_floors), 10);

                        poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                        poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 3);

                        floorSegment.ZoneSteps.Add(poolSpawn);

                        TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                        tileSpawn.Priority = PR_RESPAWN_TRAP;

                        tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_slumber", false), new IntRange(0, max_floors), 10);//sleep trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_seal", false), new IntRange(0, max_floors), 10);//seal trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_warp", true), new IntRange(0, max_floors), 10);//warp trap


                        floorSegment.ZoneSteps.Add(tileSpawn);


                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            AddFloorData(layout, "B09. Relic Tower.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                            //Tilesets
                            AddTextureData(layout, "wish_cave_1_wall", "wish_cave_1_floor", "wish_cave_1_secondary", "fairy");

                            //TODO: add water at the borders

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

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 10)
            {
                #region FAULTLINE RIDGE
                {
                    zone.Name = new LocalText("Faultline Ridge");
                    zone.Rescues = 2;
                    zone.Level = 15;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Faultline Ridge\n{0}F")));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 0);
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;

                    //necessities
                    CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                    necessities.SpawnRates.SetRange(10, new IntRange(0, 10));
                    itemSpawnZoneStep.Spawns.Add("necessities", necessities);

                    necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, 10), 9);//Leppa
                    necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, 10), 12);//Oran
                    necessities.Spawns.Add(new InvItem("food_apple"), new IntRange(0, 10), 10);//Apple
                    necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, 10), 10);//Lum
                    necessities.Spawns.Add(new InvItem("seed_reviver"), new IntRange(0, 10), 5);//reviver seed

                    //snacks
                    CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                    snacks.SpawnRates.SetRange(10, new IntRange(0, 10));
                    itemSpawnZoneStep.Spawns.Add("snacks", snacks);

                    snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, 10), 20);//blast seed
                    snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, 10), 10);//warp seed
                    snacks.Spawns.Add(new InvItem("seed_sleep"), new IntRange(0, 10), 10);//sleep seed

                    //wands
                    CategorySpawn<InvItem> ammo = new CategorySpawn<InvItem>();
                    ammo.SpawnRates.SetRange(16, new IntRange(0, 10));
                    itemSpawnZoneStep.Spawns.Add("ammo", ammo);

                    ammo.Spawns.Add(new InvItem("ammo_stick", false, 3), new IntRange(0, 10), 10);//stick
                    ammo.Spawns.Add(new InvItem("wand_whirlwind", false, 2), new IntRange(0, 10), 10);//whirlwind wand
                    ammo.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, 10), 10);//pounce wand
                    ammo.Spawns.Add(new InvItem("wand_warp", false, 1), new IntRange(0, 10), 10);//warp wand
                    ammo.Spawns.Add(new InvItem("wand_path", false, 2), new IntRange(0, 10), 16);//path wand
                    ammo.Spawns.Add(new InvItem("wand_fear", false, 3), new IntRange(0, 10), 10);//fear wand
                    ammo.Spawns.Add(new InvItem("ammo_geo_pebble", false, 2), new IntRange(0, 10), 16);//Geo Pebble

                    //orbs
                    CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                    orbs.SpawnRates.SetRange(10, new IntRange(0, 10));
                    itemSpawnZoneStep.Spawns.Add("orbs", orbs);

                    orbs.Spawns.Add(new InvItem("orb_rebound"), new IntRange(0, 10), 10);//Rebound
                    orbs.Spawns.Add(new InvItem("orb_all_protect"), new IntRange(0, 10), 5);//All Protect
                    orbs.Spawns.Add(new InvItem("orb_all_aim"), new IntRange(0, 10), 9);//All-Aim
                    orbs.Spawns.Add(new InvItem("orb_trap_see"), new IntRange(0, 10), 8);//Trap-See
                    orbs.Spawns.Add(new InvItem("orb_trapbust"), new IntRange(0, 10), 8);//Trapbust

                    //held items
                    CategorySpawn<InvItem> heldItems = new CategorySpawn<InvItem>();
                    heldItems.SpawnRates.SetRange(6, new IntRange(0, 10));
                    itemSpawnZoneStep.Spawns.Add("held", heldItems);

                    heldItems.Spawns.Add(new InvItem("held_power_band"), new IntRange(0, 10), 1);//Power Band
                    heldItems.Spawns.Add(new InvItem("held_defense_scarf"), new IntRange(0, 10), 1);//Defense Scarf
                    heldItems.Spawns.Add(new InvItem("held_black_belt"), new IntRange(0, 10), 1);//Black Belt
                    heldItems.Spawns.Add(new InvItem("held_hard_stone"), new IntRange(0, 10), 1);//Hard Stone
                    heldItems.Spawns.Add(new InvItem("held_binding_band"), new IntRange(0, 10), 2);//Binding Band
                    heldItems.Spawns.Add(new InvItem("held_metronome"), new IntRange(0, 10), 2);//Metronome

                    //special
                    CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                    special.SpawnRates.SetRange(7, new IntRange(0, 10));
                    itemSpawnZoneStep.Spawns.Add("special", special);

                    int rate = 2;
                    special.Spawns.Add(new InvItem("apricorn_brown"), new IntRange(0, 10), rate);//brown apricorns
                    special.Spawns.Add(new InvItem("apricorn_white"), new IntRange(0, 10), rate);//white apricorns

                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;

                    // 74 Geodude : 222 Magnitude : 479 Smack Down
                    poolSpawn.Spawns.Add(GetTeamMob("geodude", "", "magnitude", "smack_down", "", "", new RandRange(12), "wander_dumb"), new IntRange(0, 3), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("geodude", "", "magnitude", "smack_down", "", "", new RandRange(16), "wander_dumb"), new IntRange(3, 10), 10);
                    // 299 Nosepass : 88 Rock Throw : 86 Thunder Wave
                    poolSpawn.Spawns.Add(GetTeamMob("nosepass", "", "rock_throw", "thunder_wave", "", "", new RandRange(15), "wander_dumb"), new IntRange(6, 10), 10);
                    // 231 Phanpy : 205 Rollout : 175 Flail
                    poolSpawn.Spawns.Add(GetTeamMob("phanpy", "", "rollout", "flail", "", "", new RandRange(12), "wander_dumb"), new IntRange(0, 10), 10);
                    // 447 Riolu : 203 Endure : 98 Quick Attack 
                    poolSpawn.Spawns.Add(GetTeamMob("riolu", "", "endure", "quick_attack", "", "", new RandRange(14), "wander_dumb"), new IntRange(3, 6), 10);
                    //296  Makuhita : 292 Arm Thrust : 252 Fake Out 
                    poolSpawn.Spawns.Add(GetTeamMob("makuhita", "", "arm_thrust", "fake_out", "", "", new RandRange(12), "wander_dumb"), new IntRange(0, 6), 10);
                    // 207 Gligar : 98 Quick Attack : 282 Knock Off
                    poolSpawn.Spawns.Add(GetTeamMob("gligar", "", "quick_attack", "knock_off", "", "", new RandRange(14), "wander_dumb"), new IntRange(0, 6), 10);

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, 10), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);


                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, 10), 10);//chestnut trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_gust", false), new IntRange(0, 10), 10);//trip trap
                    floorSegment.ZoneSteps.Add(tileSpawn);


                    for (int ii = 0; ii < 10; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        if (ii < 6)
                            AddFloorData(layout, "B18. Faultline Ridge.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Clear);
                        else
                            AddFloorData(layout, "B18. Faultline Ridge.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                        if (ii < 6)
                            AddWaterSteps(layout, "water", new RandRange(30));//water

                        //Tilesets
                        if (ii < 6)
                            AddTextureData(layout, "mt_horn_wall", "mt_horn_floor", "mt_horn_secondary", "rock");
                        else
                            AddTextureData(layout, "amp_plains_wall", "amp_plains_floor", "amp_plains_secondary", "rock");

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
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
                                MobSpawn mob = GetGenericMob("shuckle", "", "struggle_bug", "withdraw", "bide", "", new RandRange(12), "wander_dumb");
                                mob.SpawnFeatures.Add(new MobSpawnItem(true, "berry_oran", "berry_oran", "berry_sitrus", "gummi_green"));
                                specificTeam.Spawns.Add(mob);


                                LoopedTeamSpawner<MapGenContext> spawner = new LoopedTeamSpawner<MapGenContext>(specificTeam);
                                {
                                    spawner.AmountSpawner = new RandRange(1, 4);
                                }
                                PlaceDisconnectedMobsStep<MapGenContext> secretMobPlacement = new PlaceDisconnectedMobsStep<MapGenContext>(spawner);
                                secretMobPlacement.AcceptedTiles.Add(new Tile("floor"));
                                layout.GenSteps.Add(PR_SPAWN_MOBS, secretMobPlacement);
                            }
                        }

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
                #endregion
            }
            else if (index == 11)
            {
                #region SLEEPING CALDERA
                {
                    zone.Name = new LocalText("Sleeping Caldera");
                    zone.Rescues = 2;
                    zone.Level = 35;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    int max_floors = 12;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Sleeping Caldera\nB{0}F")));

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
                    necessities.Spawns.Add(new InvItem("food_banana"), new IntRange(0, max_floors), 2);
                    necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 10);
                    necessities.Spawns.Add(new InvItem("berry_sitrus"), new IntRange(0, max_floors), 6);
                    necessities.Spawns.Add(new InvItem("food_grimy"), new IntRange(0, max_floors), 6);
                    //snacks
                    CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                    snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                    snacks.Spawns.Add(new InvItem("seed_blinker"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_ban"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_decoy"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("berry_occa"), new IntRange(0, max_floors), 3);
                    snacks.Spawns.Add(new InvItem("berry_coba"), new IntRange(0, max_floors), 3);
                    snacks.Spawns.Add(new InvItem("berry_passho"), new IntRange(0, max_floors), 3);
                    //boosters
                    CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                    boosters.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("boosters", boosters);


                    boosters.Spawns.Add(new InvItem("gummi_blue"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_black"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_clear"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_grass"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_green"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_brown"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_orange"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_gold"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_red"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_royal"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_silver"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_white"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_yellow"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_sky"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_gray"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_magenta"), new IntRange(0, max_floors), 5);
                    //special
                    CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                    special.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("special", special);


                    special.Spawns.Add(new InvItem("apricorn_purple", true), new IntRange(0, max_floors), 3);
                    special.Spawns.Add(new InvItem("apricorn_purple"), new IntRange(0, max_floors), 7);
                    special.Spawns.Add(new InvItem("apricorn_red", true), new IntRange(0, max_floors), 3);
                    special.Spawns.Add(new InvItem("apricorn_red"), new IntRange(0, max_floors), 7);
                    special.Spawns.Add(new InvItem("apricorn_white", true), new IntRange(0, max_floors), 3);
                    special.Spawns.Add(new InvItem("apricorn_white"), new IntRange(0, max_floors), 7);
                    special.Spawns.Add(new InvItem("machine_assembly_box", true), new IntRange(0, max_floors), 3);
                    special.Spawns.Add(new InvItem("machine_assembly_box"), new IntRange(0, max_floors), 7);
                    //evo
                    CategorySpawn<InvItem> evo = new CategorySpawn<InvItem>();
                    evo.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("evo", evo);


                    evo.Spawns.Add(new InvItem("evo_fire_stone"), new IntRange(0, max_floors), 10);
                    //throwable
                    CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                    throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                    throwable.Spawns.Add(new InvItem("ammo_gravelerock", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("ammo_iron_thorn", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_whirlwind", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_warp", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_lob", false, 3), new IntRange(0, max_floors), 10);
                    //orbs
                    CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                    orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                    orbs.Spawns.Add(new InvItem("orb_weather", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_weather"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_mobile", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_mobile"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_fill_in", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_fill_in"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_endure", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_endure"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_mirror", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_mirror"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_cleanse"), new IntRange(0, max_floors), 10);
                    //held
                    CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                    held.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("held", held);


                    held.Spawns.Add(new InvItem("held_binding_band", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_binding_band"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_weather_rock", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_weather_rock"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_cover_band", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_cover_band"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_shell_bell", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_shell_bell"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_soft_sand", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_soft_sand"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_twisted_spoon", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_twisted_spoon"), new IntRange(0, max_floors), 7);
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

                    poolSpawn.Spawns.Add(GetTeamMob("croagunk", "dry_skin", "feint_attack", "revenge", "", "", new RandRange(29), "wander_dumb"), new IntRange(0, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("barboach", "", "amnesia", "water_gun", "", "", new RandRange(29), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("nidorina", "", "helping_hand", "bite", "", "", new RandRange(29), "wander_dumb"), new IntRange(6, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("nidorino", "", "horn_attack", "fury_attack", "", "", new RandRange(29), "wander_dumb"), new IntRange(0, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("grimer", "", "sludge_bomb", "disable", "", "", new RandRange(29), "wander_dumb"), new IntRange(6, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("stunky", "", "poison_gas", "fury_swipes", "", "", new RandRange(29), "wander_dumb"), new IntRange(0, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("poliwhirl", "", "hypnosis", "water_sport", "double_slap", "", new RandRange(29), "wander_dumb"), new IntRange(0, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("snorlax", "", "rest", "body_slam", "", "", new RandRange(29), "wander_dumb"), new IntRange(6, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("dunsparce", "", "yawn", "ancient_power", "", "", new RandRange(29), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("quilava", "", "flame_wheel", "", "", "", new RandRange(29), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("camerupt", "", "rock_slide", "lava_plume", "", "", new RandRange(33), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("magmar", "", "fire_punch", "", "", "", new RandRange(33), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("pignite", "", "heat_crash", "rollout", "", "", new RandRange(31), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("slugma", "", "incinerate", "harden", "", "", new RandRange(33), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("marowak", 1, "", Gender.Unknown), "", "shadow_bone", "will_o_wisp", "", "", new RandRange(33), "wander_dumb"), new IntRange(0, max_floors), 10);
                    //sleeping, with choice band
                    poolSpawn.Spawns.Add(GetTeamMob("flareon", "", "flare_blitz", "", "", "", new RandRange(50), "wander_normal"), new IntRange(0, max_floors), 10);
                    //only one
                    poolSpawn.Spawns.Add(GetTeamMob("heatran", "", "magma_storm", "earth_power", "", "", new RandRange(40), "wander_normal"), new IntRange(0, max_floors), 10);

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 4);

                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;


                    tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_poison", false), new IntRange(0, max_floors), 10);//poison trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_self_destruct", false), new IntRange(0, max_floors), 10);//selfdestruct trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_hunger", true), new IntRange(0, max_floors), 10);//hunger trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_grimy", false), new IntRange(0, max_floors), 10);//grimy trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_slumber", false), new IntRange(0, max_floors), 10);//sleep trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_gust", false), new IntRange(0, max_floors), 10);//gust trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_warp", true), new IntRange(0, max_floors), 10);//warp trap


                    floorSegment.ZoneSteps.Add(tileSpawn);



                    SpawnList<IGenPriority> assemblyZoneSpawns = new SpawnList<IGenPriority>();
                    assemblyZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem("food_apple"))))), 10);
                    SpreadStepZoneStep appleZoneStep = new SpreadStepZoneStep(new SpreadPlanSpaced(new RandRange(1, 1), new IntRange(0, 13)), assemblyZoneSpawns);//apple
                    floorSegment.ZoneSteps.Add(appleZoneStep);


                    SpreadRoomZoneStep evoZoneStep = new SpreadRoomZoneStep(PR_GRID_GEN_EXTRA, PR_ROOMS_GEN_EXTRA, new SpreadPlanSpaced(new RandRange(1, 1), new IntRange(0, 13)));
                    List<BaseRoomFilter> evoFilters = new List<BaseRoomFilter>();
                    evoFilters.Add(new RoomFilterComponent(true, new ImmutableRoom()));
                    evoFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                    evoZoneStep.Spawns.Add(new RoomGenOption(new RoomGenEvo<MapGenContext>(), new RoomGenEvo<ListMapGenContext>(), evoFilters), 10);
                    floorSegment.ZoneSteps.Add(evoZoneStep);


                    SpreadHouseZoneStep chestChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanQuota(new RandDecay(3, 100, 50), new IntRange(5, 13)));
                    chestChanceZoneStep.ModStates.Add(new FlagType(typeof(ChestModGenState)));
                    chestChanceZoneStep.HouseStepSpawns.Add(new ChestStep<ListMapGenContext>(true, GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                    foreach (string gummi in IterateGummis())
                        chestChanceZoneStep.Items.Add(new MapItem(gummi), new IntRange(0, 30), 4);//gummis
                    chestChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(4, 9)), new IntRange(0, 30), 10);
                    chestChanceZoneStep.MobThemes.Add(new MobThemeTypingSeeded(EvoFlag.FinalEvo | EvoFlag.NoEvo | EvoFlag.MidEvo, new RandRange(7, 13)), new IntRange(0, 13), 10);
                    floorSegment.ZoneSteps.Add(chestChanceZoneStep);


                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        RoomFloorGen layout = new RoomFloorGen();


                        //Floor settings
                        AddFloorData(layout, "B07. Flyaway Cliffs.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                        //Tilesets
                        //other candidates: steam_cave
                        //solar_cave_1/deep_dark_crater,waterfall_cave/dark_crater,unused_steam_cave/magma_cavern_3
                        if (ii < 6)
                            AddTextureData(layout, "rock_path_rb_wall", "rock_path_rb_floor", "rock_path_rb_secondary", "fire");
                        else
                            AddTextureData(layout, "waterfall_cave_wall", "waterfall_cave_floor", "waterfall_cave_secondary", "fire");

                        AddWaterSteps(layout, "water", new RandRange(30));//water

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(6, 9));

                        //money - 1,560P to 5,850P
                        AddMoneyData(layout, new RandRange(2, 4));

                        //enemies! ~ lv 15 to 35
                        AddRespawnData(layout, 3, 80);

                        //enemies
                        AddEnemySpawnData(layout, 20, new RandRange(2, 4));


                        //items
                        AddItemData(layout, new RandRange(3, 6), 25);


                        AddInitListStep(layout, 54, 40);

                        //construct paths
                        {
                            FloorPathBranch<ListMapGenContext> path = new FloorPathBranch<ListMapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallPercent = 70;
                            path.FillPercent = new RandRange(45);
                            path.BranchRatio = new RandRange(0, 25);

                            SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<ListMapGenContext>(new RandRange(4, 11), new RandRange(4, 11), new RandRange(2, 6), new RandRange(2, 6)), 10);
                            //round
                            genericRooms.Add(new RoomGenRound<ListMapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(3), new RandRange(3, 7)), 10);
                            genericHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(3, 7), new RandRange(3)), 10);
                            genericHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(3, 7), new RandRange(3, 7)), 5);
                            genericHalls.Add(new RoomGenSquare<ListMapGenContext>(new RandRange(1), new RandRange(1)), 20);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_ROOMS_GEN, path);
                        }

                        layout.GenSteps.Add(PR_ROOMS_GEN, CreateGenericListConnect(100, 100));

                        AddDrawListSteps(layout);

                        AddStairStep(layout, false);

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
                #endregion
            }
            else if (index == 12)
            {
                #region SHIMMER BAY
                {
                    zone.Name = new LocalText("Shimmer Bay");
                    zone.Rescues = 2;
                    zone.Level = 25;
                    zone.Rogue = RogueStatus.ItemTransfer;
                    //zone.Persistent = true;

                    int max_floors = 12;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Shimmer Bay\n{0}F")));

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


                    necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 9);
                    necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 12);
                    necessities.Spawns.Add(new InvItem("food_apple"), new IntRange(0, max_floors), 10);
                    necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 10);
                    necessities.Spawns.Add(new InvItem("seed_reviver"), new IntRange(0, max_floors), 5);
                    //snacks
                    CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                    snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                    snacks.Spawns.Add(new InvItem("seed_sleep"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_hunger"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_blinker"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("berry_rowap"), new IntRange(0, max_floors), 10);
                    //boosters
                    CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                    boosters.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("boosters", boosters);


                    boosters.Spawns.Add(new InvItem("gummi_blue"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_black"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_clear"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_grass"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_green"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_brown"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_orange"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_gold"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_red"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_royal"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_silver"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_white"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_yellow"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_sky"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_gray"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_magenta"), new IntRange(0, max_floors), 1);
                    //special
                    CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                    special.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("special", special);


                    special.Spawns.Add(new InvItem("apricorn_plain"), new IntRange(0, max_floors), 10);
                    special.Spawns.Add(new InvItem("apricorn_blue"), new IntRange(0, max_floors), 10);
                    special.Spawns.Add(new InvItem("machine_assembly_box"), new IntRange(0, max_floors), 10);
                    //evo
                    CategorySpawn<InvItem> evo = new CategorySpawn<InvItem>();
                    evo.SpawnRates.SetRange(1, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("evo", evo);


                    evo.Spawns.Add(new InvItem("evo_water_stone"), new IntRange(0, max_floors), 10);
                    //throwable
                    CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                    throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                    throwable.Spawns.Add(new InvItem("ammo_rare_fossil", false, 1), new IntRange(0, max_floors), 5);
                    throwable.Spawns.Add(new InvItem("ammo_corsola_twig", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_switcher", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_fear", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_slow", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_lob", false, 3), new IntRange(0, max_floors), 10);
                    //orbs
                    CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                    orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                    orbs.Spawns.Add(new InvItem("orb_scanner"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_freeze"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_spurn"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_petrify"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_luminous"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_rollcall"), new IntRange(0, max_floors), 10);
                    //held
                    CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                    held.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("held", held);


                    held.Spawns.Add(new InvItem("held_reunion_cape"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_shed_shell"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_shell_bell"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_expert_belt"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_pink_bow"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_mystic_water"), new IntRange(0, max_floors), 10);
                    //tms
                    CategorySpawn<InvItem> tms = new CategorySpawn<InvItem>();
                    tms.SpawnRates.SetRange(7, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("tms", tms);


                    tms.Spawns.Add(new InvItem("tm_protect"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_round"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_rest"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_hidden_power"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_rock_tomb"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_strength"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_thief"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_cut"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_whirlpool"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_fly"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_power_up_punch"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_infestation"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_work_up"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_roar"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_flash"), new IntRange(0, max_floors), 10);


                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;


                    poolSpawn.Spawns.Add(GetTeamMob("seel", "", "icy_wind", "encore", "", "", new RandRange(22), "wander_dumb"), new IntRange(0, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("clamperl", "", "clamp", "whirlpool", "iron_defense", "", new RandRange(23), "turret"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("goldeen", "", "horn_attack", "water_pulse", "", "", new RandRange(22), "wander_dumb"), new IntRange(0, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("mantyke", "", "wing_attack", "bubble_beam", "", "", new RandRange(22), "wander_dumb"), new IntRange(0, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("horsea", "", "smokescreen", "twister", "", "", new RandRange(24), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("carvanha", "", "bite", "screech", "", "", new RandRange(24), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("wailmer", "", "whirlpool", "astonish", "", "", new RandRange(25), "wander_dumb"), new IntRange(6, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("tentacool", "", "acid_spray", "wrap", "", "", new RandRange(25), "wander_dumb"), new IntRange(6, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("corsola", "", "spike_cannon", "lucky_chant", "", "", new RandRange(25), "wander_dumb"), new IntRange(6, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("chinchou", "", "confuse_ray", "spark", "", "", new RandRange(24), "wander_dumb"), new IntRange(6, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("mantine", "", "wide_guard", "bubble_beam", "", "", new RandRange(28), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("phione", "", "dive", "aqua_ring", "acid_armor", "", new RandRange(36), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("huntail", "", "ice_fang", "sucker_punch", "dive", "", new RandRange(28), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("gorebyss", "", "amnesia", "draining_kiss", "dive", "", new RandRange(28), "wander_dumb"), new IntRange(0, max_floors), 10);
                    //spawn in the walls
                    poolSpawn.Spawns.Add(GetTeamMob("dratini", "", "twister", "dragon_rage", "", "", new RandRange(25), "wander_dumb"), new IntRange(0, max_floors), 10);
                    //asleep, with shell bell
                    poolSpawn.Spawns.Add(GetTeamMob("vaporeon", "", "aqua_ring", "acid_armor", "muddy_water", "", new RandRange(50), "wander_normal"), new IntRange(0, max_floors), 10);

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 3);

                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;


                    tileSpawn.Spawns.Add(new EffectTile("trap_mud", false), new IntRange(0, max_floors), 10);//mud trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_slow", false), new IntRange(0, max_floors), 10);//slow trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, max_floors), 10);//chestnut trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_gust", false), new IntRange(0, max_floors), 10);//gust trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_grimy", false), new IntRange(0, max_floors), 10);//grimy trap


                    floorSegment.ZoneSteps.Add(tileSpawn);



                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        AddFloorData(layout, "B23. Shimmer Bay.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                        //Tilesets
                        if (ii < 6)
                            AddTextureData(layout, "miracle_sea_wall", "miracle_sea_floor", "miracle_sea_secondary", "water");
                        else
                            AddTextureData(layout, "stormy_sea_1_wall", "stormy_sea_1_floor", "stormy_sea_1_secondary", "water");

                        if (ii < 6)
                            AddWaterSteps(layout, "water", new RandRange(30));//water
                        else if (ii < 10)
                            AddWaterSteps(layout, "water", new RandRange(25));//water
                        else
                            AddWaterSteps(layout, "water", new RandRange(35));//water

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(6, 9));

                        //money - 630P to 1,197P
                        AddMoneyData(layout, new RandRange(2, 4));

                        //enemies! ~ up to lv 8
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

                        {
                            EffectTile exitTile = new EffectTile("stairs_back_down", true);
                            exitTile.TileStates.Set(new DestState(new SegLoc(0, -1), true));
                            var step = new FloorStairsStep<MapGenContext, MapGenEntrance, MapGenExit>(new MapGenEntrance(Dir8.Down), new MapGenExit(exitTile));
                            step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            step.Filters.Add(new RoomFilterComponent(true, new BossRoom()));
                            layout.GenSteps.Add(PR_EXITS, step);
                        }
                        if (ii < max_floors - 1)
                        {
                            EffectTile secretTile = new EffectTile("stairs_go_down", true);
                            RandomSpawnStep<MapGenContext, EffectTile> trapStep = new RandomSpawnStep<MapGenContext, EffectTile>(new PickerSpawner<MapGenContext, EffectTile>(new PresetMultiRand<EffectTile>(secretTile)));
                            layout.GenSteps.Add(PR_SPAWN_TRAPS, trapStep);
                        }


                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
                #endregion
            }
            else if (index == 13)
            {
                #region FERTILE VALLEY
                {
                    zone.Name = new LocalText("Fertile Valley");
                    zone.Rescues = 2;
                    zone.Level = 15;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 8;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Fertile Valley\nB{0}F")));

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


                        snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_sleep"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_hunger"), new IntRange(0, max_floors), 10);
                        //special
                        CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                        special.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("special", special);


                        special.Spawns.Add(new InvItem("apricorn_plain"), new IntRange(0, max_floors), 5);
                        special.Spawns.Add(new InvItem("apricorn_green"), new IntRange(0, max_floors), 5);
                        special.Spawns.Add(new InvItem("apricorn_purple"), new IntRange(0, max_floors), 5);
                        special.Spawns.Add(new InvItem("apricorn_brown"), new IntRange(0, max_floors), 5);
                        special.Spawns.Add(new InvItem("apricorn_black"), new IntRange(0, max_floors), 5);
                        //throwable
                        CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                        throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                        throwable.Spawns.Add(new InvItem("ammo_cacnea_spike", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_whirlwind", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_switcher", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_slow", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("ammo_geo_pebble", false, 3), new IntRange(0, max_floors), 10);
                        //orbs
                        CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                        orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                        orbs.Spawns.Add(new InvItem("orb_all_aim"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_all_dodge"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_mirror"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_petrify"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_slumber"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_foe_hold"), new IntRange(0, max_floors), 10);
                        //held
                        CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                        held.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("held", held);


                        held.Spawns.Add(new InvItem("held_power_band"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_special_band"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_defense_scarf"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_zinc_band"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_twist_band"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_soft_sand"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_poison_barb"), new IntRange(0, max_floors), 10);


                        floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                        //mobs
                        TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                        poolSpawn.Priority = PR_RESPAWN_MOB;

                        poolSpawn.Spawns.Add(GetTeamMob("ekans", "", "wrap", "bite", "", "", new RandRange(12), "wander_dumb"), new IntRange(0, 6), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("doduo", "", "growl", "fury_attack", "", "", new RandRange(12), "wander_dumb"), new IntRange(0, 6), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("bonsly", "", "rock_throw", "", "", "", new RandRange(14), "weird_tree"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("lotad", "", "bubble", "natural_gift", "", "", new RandRange(13), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("surskit", "", "sweet_scent", "quick_attack", "", "", new RandRange(13), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("beedrill", "", "fury_attack", "focus_energy", "", "", new RandRange(13), "wander_dumb"), new IntRange(3, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("spinarak", "", "string_shot", "leech_life", "", "", new RandRange(13), "wander_dumb"), new IntRange(3, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("poochyena", "", "bite", "howl", "", "", new RandRange(13), "wander_dumb"), new IntRange(3, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("corphish", "", "leer", "vice_grip", "", "", new RandRange(13), "wander_dumb"), new IntRange(3, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("azurill", "", "charm", "splash", "", "", new RandRange(10), "wander_dumb"), new IntRange(3, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("tauros", "", "", "", "", "", new RandRange(5)), new IntRange(0, max_floors), 10);

                        poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                        floorSegment.ZoneSteps.Add(poolSpawn);

                        TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                        tileSpawn.Priority = PR_RESPAWN_TRAP;


                        tileSpawn.Spawns.Add(new EffectTile("trap_mud", false), new IntRange(0, max_floors), 10);//mud trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_slow", false), new IntRange(0, max_floors), 10);//slow trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, max_floors), 10);//chestnut trap

                        floorSegment.ZoneSteps.Add(tileSpawn);



                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            AddFloorData(layout, "B16. Fertile Valley.ogg", 500, Map.SightRange.Clear, Map.SightRange.Clear);

                            //Tilesets
                            if (ii < 6)
                                AddTextureData(layout, "purity_forest_7_wall", "purity_forest_7_floor", "purity_forest_7_secondary", "ground");
                            else
                                AddTextureData(layout, "purity_forest_6_wall", "purity_forest_6_floor", "purity_forest_6_secondary", "ground");

                            AddWaterSteps(layout, "water", new RandRange(20));//water

                            //traps
                            AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                            AddTrapsSteps(layout, new RandRange(6, 9));

                            //money - 1,020P to 4,284P
                            AddMoneyData(layout, new RandRange(2, 4));

                            //enemies! ~ lv 6 to 20
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

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }

                    {
                        int max_floors = 5;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Muddy Valley\nB{0}F")));

                        //money
                        MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 10);
                        moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                        floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                        //items
                        ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                        itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;


                        //necessities
                        CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                        necessities.SpawnRates.SetRange(5, new IntRange(0, max_floors));
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


                        snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_sleep"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_hunger"), new IntRange(0, max_floors), 10);
                        //special
                        CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                        special.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("special", special);


                        special.Spawns.Add(new InvItem("apricorn_plain"), new IntRange(0, max_floors), 5);
                        special.Spawns.Add(new InvItem("apricorn_green"), new IntRange(0, max_floors), 5);
                        special.Spawns.Add(new InvItem("apricorn_purple"), new IntRange(0, max_floors), 5);
                        special.Spawns.Add(new InvItem("apricorn_brown"), new IntRange(0, max_floors), 5);
                        special.Spawns.Add(new InvItem("apricorn_black"), new IntRange(0, max_floors), 5);
                        //throwable
                        CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                        throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                        throwable.Spawns.Add(new InvItem("ammo_cacnea_spike", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_whirlwind", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_switcher", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_slow", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("ammo_geo_pebble", false, 3), new IntRange(0, max_floors), 10);
                        //orbs
                        CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                        orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                        orbs.Spawns.Add(new InvItem("orb_all_aim"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_all_dodge"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_mirror"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_petrify"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_slumber"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_foe_hold"), new IntRange(0, max_floors), 10);
                        //held
                        CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                        held.SpawnRates.SetRange(4, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("held", held);


                        held.Spawns.Add(new InvItem("held_power_band"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_special_band"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_defense_scarf"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_zinc_band"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_twist_band"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_soft_sand"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_poison_barb"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_miracle_seed"), new IntRange(0, max_floors), 10);


                        floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                        //mobs
                        TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                        poolSpawn.Priority = PR_RESPAWN_MOB;

                        poolSpawn.Spawns.Add(GetTeamMob("oddish", "", "acid", "absorb", "", "", new RandRange(15), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("pikachu", "", "play_nice", "thunder_shock", "", "", new RandRange(15), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("teddiursa", "", "fake_tears", "lick", "", "", new RandRange(15), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("surskit", "", "sweet_scent", "quick_attack", "", "", new RandRange(13), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("wooper", "", "mud_bomb", "", "", "", new RandRange(15), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("gulpin", "", "sludge", "", "", "", new RandRange(15), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("shroomish", "effect_spore", "mega_drain", "leech_seed", "", "", new RandRange(15), "wander_dumb"), new IntRange(0, max_floors), 10);

                        poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                        floorSegment.ZoneSteps.Add(poolSpawn);

                        TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                        tileSpawn.Priority = PR_RESPAWN_TRAP;

                        tileSpawn.Spawns.Add(new EffectTile("trap_mud", false), new IntRange(0, max_floors), 10);//mud trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_slow", false), new IntRange(0, max_floors), 10);//slow trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, max_floors), 10);//chestnut trap

                        floorSegment.ZoneSteps.Add(tileSpawn);


                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            AddFloorData(layout, "B17. Muddy Valley.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                            AddWaterSteps(layout, "water", new RandRange(20));//water

                            //Tilesets
                            AddTextureData(layout, "quicksand_pit_wall", "quicksand_pit_floor", "quicksand_pit_secondary", "ground");

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

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 14)
            {
                #region AMBUSH FOREST
                {
                    zone.Name = new LocalText("**Ambush Forest");
                    zone.Rescues = 2;
                    zone.Level = 30;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    int max_floors = 25;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Ambush Forest\nB{0}F")));

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


                    necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 9);
                    necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 6);
                    necessities.Spawns.Add(new InvItem("food_apple"), new IntRange(0, max_floors), 10);
                    necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 10);
                    necessities.Spawns.Add(new InvItem("berry_sitrus"), new IntRange(0, max_floors), 6);
                    //snacks
                    CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                    snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                    snacks.Spawns.Add(new InvItem("berry_payapa"), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("berry_chilan"), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("berry_kebia"), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("berry_rindo"), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("berry_tanga"), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("berry_colbur"), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("berry_jaboca"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("berry_rowap"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_doom"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_blinker"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_decoy"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_vile"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("herb_white"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("herb_mental"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_pure"), new IntRange(0, max_floors), 10);
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


                    special.Spawns.Add(new InvItem("machine_recall_box"), new IntRange(0, max_floors), 10);
                    special.Spawns.Add(new InvItem("key", false, 1), new IntRange(0, max_floors), 10);
                    //evo
                    CategorySpawn<InvItem> evo = new CategorySpawn<InvItem>();
                    evo.SpawnRates.SetRange(1, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("evo", evo);


                    evo.Spawns.Add(new InvItem("evo_dusk_stone"), new IntRange(0, max_floors), 10);
                    //throwable
                    CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                    throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                    throwable.Spawns.Add(new InvItem("ammo_gravelerock", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("ammo_rare_fossil", false, 2), new IntRange(0, max_floors), 5);
                    throwable.Spawns.Add(new InvItem("wand_purge", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_fear", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_slow", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_warp", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_lure", false, 3), new IntRange(0, max_floors), 10);
                    //orbs
                    CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                    orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                    orbs.Spawns.Add(new InvItem("orb_mug"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_foe_hold"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_foe_seal"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_mobile"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_pierce"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_nullify"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_scanner"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_trap_see"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_rebound"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_halving"), new IntRange(0, max_floors), 10);
                    //held
                    CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                    held.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("held", held);


                    held.Spawns.Add(new InvItem("held_shed_shell"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_reunion_cape"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_cover_band"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_life_orb"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_spell_tag"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_twisted_spoon"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_silk_scarf"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_miracle_seed"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_black_glasses"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_silver_powder"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_mind_plate"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_blank_plate"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_spooky_plate"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_meadow_plate"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_insect_plate"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_dread_plate"), new IntRange(0, max_floors), 10);
                    //tms
                    CategorySpawn<InvItem> tms = new CategorySpawn<InvItem>();
                    tms.SpawnRates.SetRange(10, new IntRange(0, max_floors));
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


                    poolSpawn.Spawns.Add(GetTeamMob("pachirisu", "run_away", "quick_attack", "charm", "nuzzle", "", new RandRange(25), "wander_normal"), new IntRange(0, 4), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("fearow", "", "mirror_move", "leer", "", "", new RandRange(26), "wander_normal"), new IntRange(0, 4), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("corphish", "", "bubble_beam", "night_slash", "", "", new RandRange(25), "wander_normal"), new IntRange(0, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("shedinja", "", "sand_attack", "shadow_sneak", "", "", new RandRange(25), "wander_normal"), new IntRange(0, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("gastly", "", "night_shade", "sucker_punch", "", "", new RandRange(23), "wander_normal"), new IntRange(0, 8), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("braixen", "magician", "howl", "flame_charge", "", "", new RandRange(27), "thief"), new IntRange(4, 10), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("scyther", "technician", "quick_attack", "false_swipe", "", "", new RandRange(27), "wander_normal"), new IntRange(4, 10), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("swablu", "", "round", "sing", "", "", new RandRange(27), "wander_normal"), new IntRange(4, 12), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("zangoose", "", "pursuit", "fury_cutter", "", "", new RandRange(27), "wander_normal"), new IntRange(6, 12), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("murkrow", "insomnia", "pursuit", "wing_attack", "", "", new RandRange(27), "wander_normal"), new IntRange(6, 14), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("swellow", "", "quick_guard", "aerial_ace", "", "", new RandRange(27), "wander_normal"), new IntRange(8, 14), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("sneasel", "", "beat_up", "icy_wind", "", "", new RandRange(27), "wander_normal"), new IntRange(8, 16), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("dartrix", "", "foresight", "pluck", "", "", new RandRange(27), "wander_normal"), new IntRange(10, 16), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("hattrem", "", "dazzling_gleam", "life_dew", "", "", new RandRange(32), TeamMemberSpawn.MemberRole.Support, "wander_normal"), new IntRange(10, 18), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("growlithe", "", "retaliate", "flame_wheel", "", "", new RandRange(32), TeamMemberSpawn.MemberRole.Support, "wander_normal"), new IntRange(10, 18), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("ambipom", "technician", "agility", "swift", "", "", new RandRange(27), "wander_normal"), new IntRange(12, 18), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("furret", "", "defense_curl", "follow_me", "", "", new RandRange(26), TeamMemberSpawn.MemberRole.Support, "wander_normal"), new IntRange(12, 20), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("haunter", "", "dream_eater", "payback", "", "", new RandRange(30), "wander_normal"), new IntRange(14, 20), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("teddiursa", "", "covet", "sweet_scent", "fury_swipes", "", new RandRange(29), "thief"), new IntRange(14, 20), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("hypno", "", "nightmare", "confusion", "", "", new RandRange(30), "wander_normal"), new IntRange(16, 22), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("mr_mime", "", "wide_guard", "psybeam", "", "", new RandRange(30), "wander_normal"), new IntRange(16, 22), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("ariados", "", "spider_web", "sucker_punch", "", "", new RandRange(28), "wander_normal"), new IntRange(18, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("parasect", "", "spore", "growth", "leech_life", "", new RandRange(30), "wander_normal"), new IntRange(20, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("delphox", "magician", "howl", "fire_spin", "", "", new RandRange(36), "thief"), new IntRange(20, max_floors), 5);
                    poolSpawn.Spawns.Add(GetTeamMob("spinda", "tangled_feet", "teeter_dance", "copycat", "", "", new RandRange(32), "wander_normal"), new IntRange(22, max_floors), 5);
                    poolSpawn.Spawns.Add(GetTeamMob("raticate", "", "pursuit", "super_fang", "crunch", "", new RandRange(30), "wander_normal"), new IntRange(22, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("crawdaunt", "", "razor_shell", "night_slash", "", "", new RandRange(31), "wander_normal"), new IntRange(22, max_floors), 10);
                    //MH Only
                    poolSpawn.Spawns.Add(GetTeamMob("decidueye", "long_reach", "spirit_shackle", "sucker_punch", "", "", new RandRange(37), "wander_normal"), new IntRange(14, max_floors), 5);
                    //MH Only
                    poolSpawn.Spawns.Add(GetTeamMob("spiritomb", "", "hypnosis", "dream_eater", "", "", new RandRange(31), "wander_normal"), new IntRange(0, max_floors), 10);

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    poolSpawn.TeamSizes.Add(2, new IntRange(0, 10), 3);
                    poolSpawn.TeamSizes.Add(2, new IntRange(10, max_floors), 4);
                    poolSpawn.TeamSizes.Add(3, new IntRange(15, max_floors), 3);

                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;


                    tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, max_floors), 10);//chestnut trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_slumber", false), new IntRange(0, max_floors), 10);//sleep trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_gust", false), new IntRange(0, max_floors), 10);//gust trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_hunger", true), new IntRange(0, max_floors), 10);//hunger trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_seal", false), new IntRange(0, max_floors), 10);//seal trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_sticky", false), new IntRange(0, max_floors), 10);//sticky trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_warp", true), new IntRange(0, max_floors), 10);//warp trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_spin", false), new IntRange(0, max_floors), 10);//spin trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_grimy", false), new IntRange(0, max_floors), 10);//grimy trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_poison", false), new IntRange(0, max_floors), 10);//poison trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_trigger", true), new IntRange(0, max_floors), 20);//trigger trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_slow", false), new IntRange(0, max_floors), 10);//slow trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_explosion", false), new IntRange(0, max_floors), 10);//explosion trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_trip", false), new IntRange(0, max_floors), 10);//trip trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_summon", true), new IntRange(0, max_floors), 10);//summon trap


                    floorSegment.ZoneSteps.Add(tileSpawn);


                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        if (ii < 12)
                            AddFloorData(layout, "B26. Ambush Forest.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);
                        else if (ii < 20)
                            AddFloorData(layout, "B27. Ambush Forest 2.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);
                        else
                            AddFloorData(layout, "B28. Ambush Forest 3.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                        //Tilesets
                        if (ii < 8)
                            AddSpecificTextureData(layout, "deep_dusk_forest_1_wall", "deep_dusk_forest_1_floor", "deep_dusk_forest_1_secondary", "tall_grass_dark", "bug");
                        else if (ii < 12)
                            AddSpecificTextureData(layout, "dusk_forest_1_wall", "dusk_forest_1_floor", "dusk_forest_1_secondary", "tall_grass_dark", "bug");
                        else if (ii < 20)
                            AddSpecificTextureData(layout, "dusk_forest_2_wall", "dusk_forest_2_floor", "dusk_forest_2_secondary", "tall_grass_blue", "bug");
                        else
                            AddSpecificTextureData(layout, "murky_forest_wall", "murky_forest_floor", "murky_forest_secondary", "tall_grass_blue", "bug");

                        if (ii >= 12)
                            AddBlobWaterSteps(layout, "water", new RandRange(1, 4), new IntRange(2, 9), true);

                        AddGrassSteps(layout, new RandRange(8, 12), new IntRange(2, 6), new RandRange(25));

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(6, 9));

                        //money - Ballpark 35K
                        AddMoneyData(layout, new RandRange(2, 4));

                        //enemies! ~ lv 7 to 30
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


                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
                #endregion
            }
            else if (index == 15)
            {
                #region TREACHEROUS MOUNTAIN
                {
                    zone.Name = new LocalText("**Treacherous Mountain");
                    zone.Rescues = 2;
                    zone.Level = 40;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    int max_floors = 25;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Treacherous Mountain\n{0}F")));

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


                    necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 9);
                    necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 6);
                    necessities.Spawns.Add(new InvItem("food_apple"), new IntRange(0, max_floors), 10);
                    necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 10);
                    necessities.Spawns.Add(new InvItem("berry_sitrus"), new IntRange(0, max_floors), 6);
                    //snacks
                    CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                    snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                    snacks.Spawns.Add(new InvItem("berry_chople"), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("berry_wacan"), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("berry_passho"), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("berry_shuca"), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("berry_coba"), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("berry_yache"), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("berry_jaboca"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("berry_rowap"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_ice"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_last_chance"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_doom"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_blinker"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("herb_power"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("herb_white"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("herb_mental"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_pure"), new IntRange(0, max_floors), 10);
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


                    special.Spawns.Add(new InvItem("machine_recall_box"), new IntRange(0, max_floors), 10);
                    special.Spawns.Add(new InvItem("key", false, 1), new IntRange(0, max_floors), 10);
                    //evo
                    CategorySpawn<InvItem> evo = new CategorySpawn<InvItem>();
                    evo.SpawnRates.SetRange(1, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("evo", evo);


                    evo.Spawns.Add(new InvItem("evo_dawn_stone"), new IntRange(0, max_floors), 10);
                    //throwable
                    CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                    throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                    throwable.Spawns.Add(new InvItem("ammo_gravelerock", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("ammo_silver_spike", false, 2), new IntRange(0, max_floors), 5);
                    throwable.Spawns.Add(new InvItem("wand_purge", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_fear", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_slow", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_warp", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_lob", false, 3), new IntRange(0, max_floors), 10);
                    //orbs
                    CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                    orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                    orbs.Spawns.Add(new InvItem("orb_weather"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_freeze"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_trapbust"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_all_dodge"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_rebound"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_endure"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_devolve"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_one_shot"), new IntRange(0, max_floors), 10);
                    //held
                    CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                    held.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("held", held);


                    held.Spawns.Add(new InvItem("held_trap_scarf"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_mobile_scarf"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_choice_band"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_sticky_barb"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_black_belt"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_magnet"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_mystic_water"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_soft_sand"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_never_melt_ice"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_sharp_beak"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_fist_plate"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_zap_plate"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_splash_plate"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_earth_plate"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_icicle_plate"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_sky_plate"), new IntRange(0, max_floors), 10);
                    //tms
                    CategorySpawn<InvItem> tms = new CategorySpawn<InvItem>();
                    tms.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("tms", tms);


                    tms.Spawns.Add(new InvItem("tm_explosion"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_snatch"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_sunny_day"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_rain_dance"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_sandstorm"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_hail"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_x_scissor"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_wild_charge"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_taunt"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_focus_punch"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_safeguard"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_light_screen"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_psyshock"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_will_o_wisp"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_dream_eater"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_nature_power"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_facade"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_swagger"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_captivate"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_rock_slide"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_fling"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_thunderbolt"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_water_pulse"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_shock_wave"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_brick_break"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_payback"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_calm_mind"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_reflect"), new IntRange(0, max_floors), 10);
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
                    tms.Spawns.Add(new InvItem("tm_secret_power"), new IntRange(0, max_floors), 10);
                    tms.Spawns.Add(new InvItem("tm_natural_gift"), new IntRange(0, max_floors), 10);


                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;


                    poolSpawn.Spawns.Add(GetTeamMob("marshtomp", "", "mud_bomb", "water_gun", "", "", new RandRange(33), "wander_dumb"), new IntRange(0, 4), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("snorunt", "ice_body", "powder_snow", "double_team", "", "", new RandRange(33), "wander_dumb"), new IntRange(0, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("pupitar", "", "screech", "rock_slide", "", "", new RandRange(33), "wander_dumb"), new IntRange(0, 8), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("piloswine", "", "endure", "take_down", "", "", new RandRange(33), "wander_dumb"), new IntRange(0, 8), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("duskull", "", "night_shade", "disable", "", "", new RandRange(34), "wander_dumb"), new IntRange(4, 10), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("yanmega", "", "sonic_boom", "", "", "", new RandRange(34), "wander_dumb"), new IntRange(6, 12), 10);
                    poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("graveler", 1, "", Gender.Unknown), "galvanize", "self_destruct", "stealth_rock", "", "", new RandRange(34), "wander_dumb"), new IntRange(6, 12), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("drampa", "sap_sipper", "dragon_rage", "twister", "", "", new RandRange(35), "wander_dumb"), new IntRange(8, 16), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("nuzleaf", "", "torment", "razor_leaf", "", "", new RandRange(35), "wander_dumb"), new IntRange(10, 16), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("bronzong", "", "imprison", "extrasensory", "", "", new RandRange(35), "wander_dumb"), new IntRange(10, 16), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("shelgon", "overcoat", "dragon_breath", "protect", "", "", new RandRange(35), "wander_dumb"), new IntRange(10, 16), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("dunsparce", "", "spite", "ancient_power", "screech", "", new RandRange(36), "wander_dumb"), new IntRange(12, 18), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("weavile", "", "embargo", "night_slash", "", "", new RandRange(36), "wander_dumb"), new IntRange(12, 18), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("dusclops", "pressure", "will_o_wisp", "bind", "", "", new RandRange(36), "wander_dumb"), new IntRange(12, 18), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("ninjask", "", "double_team", "bug_bite", "", "", new RandRange(36), "retreater"), new IntRange(14, 20), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("swampert", "", "muddy_water", "", "", "", new RandRange(37), "wander_dumb"), new IntRange(14, 20), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("dugtrio", "sand_veil", "earth_power", "bulldoze", "", "", new RandRange(37), "wander_dumb"), new IntRange(14, 20), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("sandslash", "sand_veil", "magnitude", "crush_claw", "", "", new RandRange(37), "wander_dumb"), new IntRange(16, 22), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("froslass", "snow_cloak", "captivate", "draining_kiss", "", "", new RandRange(37), "wander_dumb"), new IntRange(16, 22), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("glalie", "", "frost_breath", "", "", "", new RandRange(38), "wander_dumb"), new IntRange(18, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("shiftry", "", "razor_wind", "leaf_tornado", "", "", new RandRange(38), "wander_dumb"), new IntRange(20, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("scizor", "swarm", "focus_energy", "bullet_punch", "x_scissor", "", new RandRange(38), "wander_dumb"), new IntRange(20, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("absol", "super_luck", "night_slash", "swords_dance", "", "", new RandRange(38), "wander_dumb"), new IntRange(22, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("noivern", "", "boomburst", "dragon_pulse", "", "", new RandRange(38), "wander_dumb"), new IntRange(22, max_floors), 10);

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    poolSpawn.TeamSizes.Add(2, new IntRange(0, 10), 3);
                    poolSpawn.TeamSizes.Add(2, new IntRange(10, max_floors), 4);


                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;

                    tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, max_floors), 10);//chestnut trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_slumber", false), new IntRange(0, max_floors), 10);//sleep trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_gust", false), new IntRange(0, max_floors), 10);//gust trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_hunger", true), new IntRange(0, max_floors), 10);//hunger trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_seal", false), new IntRange(0, max_floors), 10);//seal trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_sticky", false), new IntRange(0, max_floors), 10);//sticky trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_warp", true), new IntRange(0, max_floors), 10);//warp trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_spin", false), new IntRange(0, max_floors), 10);//spin trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_grimy", false), new IntRange(0, max_floors), 10);//grimy trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_poison", false), new IntRange(0, max_floors), 10);//poison trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_trigger", true), new IntRange(0, max_floors), 20);//trigger trap

                    tileSpawn.Spawns.Add(new EffectTile("trap_mud", false), new IntRange(0, max_floors), 10);//mud trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_self_destruct", false), new IntRange(0, max_floors), 10);//selfdestruct trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_pp_leech", true), new IntRange(0, max_floors), 10);//pp-leech trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_grudge", true), new IntRange(0, max_floors), 10);//grudge trap

                    floorSegment.ZoneSteps.Add(tileSpawn);


                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();


                        //Floor settings
                        if (ii < 12)
                            AddFloorData(layout, "B29. Treacherous Mountain.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);
                        else if (ii < 20)
                            AddFloorData(layout, "B30. Treacherous Mountain 2.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);
                        else
                            AddFloorData(layout, "B31. Treacherous Mountain 3.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);


                        //Tilesets
                        if (ii < 8)
                            AddSpecificTextureData(layout, "zero_isle_south_2_wall", "zero_isle_south_2_floor", "zero_isle_south_2_secondary", "tall_grass_dark", "dragon");
                        else if (ii < 12)
                            AddSpecificTextureData(layout, "mt_horn_wall", "mt_horn_floor", "mt_horn_secondary", "tall_grass_dark", "dragon");
                        else if (ii < 16)
                            AddSpecificTextureData(layout, "mt_thunder_wall", "mt_thunder_floor", "mt_thunder_secondary", "tall_grass_yellow", "dragon");
                        else if (ii < 20)
                            AddSpecificTextureData(layout, "southern_cavern_1_wall", "southern_cavern_1_floor", "southern_cavern_1_secondary", "tall_grass_gray", "dragon");
                        else
                            AddSpecificTextureData(layout, "zero_isle_east_3_wall", "zero_isle_east_3_floor", "zero_isle_east_3_secondary", "tall_grass_dark", "dragon");

                        if (ii >= 10)
                            AddWaterSteps(layout, "water", new RandRange(25));//water

                        if (ii < 12)
                            AddGrassSteps(layout, new RandRange(4, 8), new IntRange(3, 9), new RandRange(30));
                        else if (ii < 20)
                            AddGrassSteps(layout, new RandRange(3, 7), new IntRange(2, 7), new RandRange(20));


                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(6, 9));

                        //money - Ballpark 30K
                        AddMoneyData(layout, new RandRange(2, 4));

                        //enemies! ~ lv 10 to 33
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


                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
                #endregion
            }
            else if (index == 16)
            {
                #region FORSAKEN DESERT
                {
                    zone.Name = new LocalText("Forsaken Desert");
                    zone.Rescues = 2;
                    zone.Level = 25;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    int max_floors = 4;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Forsaken Desert\n{0}F")));


                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 0);
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;


                    //snacks
                    CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                    snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                    snacks.Spawns.Add(new InvItem("seed_hunger"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_decoy"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_vile"), new IntRange(0, max_floors), 10);
                    //special
                    CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                    special.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("special", special);


                    special.Spawns.Add(new InvItem("apricorn_brown"), new IntRange(0, max_floors), 10);
                    special.Spawns.Add(new InvItem("apricorn_red"), new IntRange(0, max_floors), 10);
                    special.Spawns.Add(new InvItem("apricorn_green"), new IntRange(0, max_floors), 10);
                    //throwable
                    CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                    throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                    throwable.Spawns.Add(new InvItem("ammo_gravelerock", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("ammo_geo_pebble", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_lure", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_topsy_turvy", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, max_floors), 10);
                    //orbs
                    CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                    orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                    orbs.Spawns.Add(new InvItem("orb_weather"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_rebound"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_mirror"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_cleanse"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_trapbust"), new IntRange(0, max_floors), 10);
                    //held
                    CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                    held.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("held", held);


                    held.Spawns.Add(new InvItem("held_power_band", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_power_band"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_defense_scarf", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_defense_scarf"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_scope_lens", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_scope_lens"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_shell_bell", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_shell_bell"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_soft_sand", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_soft_sand"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_poison_barb", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_poison_barb"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_grip_claw", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_grip_claw"), new IntRange(0, max_floors), 7);
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
                    //120 Staryu : 55 Water Gun
                    poolSpawn.Spawns.Add(GetTeamMob("cubone", "", "bone_club", "growl", "", "", new RandRange(23), "wander_normal"), new IntRange(0, 3), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("marowak", "", "bonemerang", "", "", "", new RandRange(28), "wander_normal"), new IntRange(3, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("hippopotas", "", "sand_tomb", "dig", "", "", new RandRange(26), "wander_normal"), new IntRange(1, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("fearow", "", "drill_run", "pluck", "", "", new RandRange(24), "wander_normal"), new IntRange(0, 2), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("sandslash", "", "magnitude", "sand_attack", "", "", new RandRange(25), "wander_normal"), new IntRange(1, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("cacnea", "", "leech_seed", "needle_arm", "", "", new RandRange(24), "wander_normal"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("skorupi", "", "acupressure", "bug_bite", "", "", new RandRange(24), TeamMemberSpawn.MemberRole.Support, "wander_normal"), new IntRange(2, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("torkoal", "", "smokescreen", "lava_plume", "", "", new RandRange(25), "wander_normal"), new IntRange(0, 2), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("arbok", "", "screech", "glare", "crunch", "", new RandRange(25), "wander_normal"), new IntRange(2, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("thievul", "", "snarl", "thief", "", "", new RandRange(25), "wander_normal"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("trapinch", "", "mud_slap", "bide", "", "", new RandRange(24), "wander_normal"), new IntRange(1, max_floors), 5);

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 3);

                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;


                    tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_mud", false), new IntRange(0, max_floors), 10);//mud trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_seal", false), new IntRange(0, max_floors), 10);//seal trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_sticky", false), new IntRange(0, max_floors), 10);//sticky trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_self_destruct", false), new IntRange(0, max_floors), 10);//selfdestruct trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_hunger", true), new IntRange(0, max_floors), 10);//hunger trap


                    floorSegment.ZoneSteps.Add(tileSpawn);


                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();


                        //Floor settings
                        AddFloorData(layout, "B08. Forsaken Desert.ogg", 30000, Map.SightRange.Clear, Map.SightRange.Dark);

                        //Tilesets
                        if (ii < 1)
                            AddTextureData(layout, "desert_region_wall", "desert_region_floor", "desert_region_secondary", "ground");
                        else if (ii < 2)
                            AddTextureData(layout, "northern_desert_1_wall", "northern_desert_1_floor", "northern_desert_1_secondary", "ground");
                        else
                            AddTextureData(layout, "northern_desert_2_wall", "northern_desert_2_floor", "northern_desert_2_secondary", "ground");

                        NoChokepointStencil<MapGenContext> roomStencil = new NoChokepointStencil<MapGenContext>(new MapTerrainStencil<MapGenContext>(false, false, true, true));
                        BlobWaterStep<MapGenContext> waterStep = new BlobWaterStep<MapGenContext>(new RandRange(0, 3), new Tile("water"), new DefaultTerrainStencil<MapGenContext>(), roomStencil, new IntRange(1, 20), new IntRange(1, 20));
                        layout.GenSteps.Add(PR_WATER, waterStep);

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(6, 9));


                        if (ii == 0)
                            AddSingleTrapStep(layout, new RandRange(35), "tile_compass");//compass tile
                        else
                            AddSingleTrapStep(layout, new RandRange(60), "tile_compass");//compass tile

                        if (ii < 2)
                        {
                            EffectTile secretTile = new EffectTile("tile_compass", true);
                            NearSpawnableSpawnStep<MapGenContext, EffectTile, MapGenEntrance> trapStep = new NearSpawnableSpawnStep<MapGenContext, EffectTile, MapGenEntrance>(new PickerSpawner<MapGenContext, EffectTile>(new PresetMultiRand<EffectTile>(secretTile)), 100);
                            layout.GenSteps.Add(PR_SPAWN_TRAPS, trapStep);
                        }

                        //money - Ballpark 25K
                        AddMoneyData(layout, new RandRange(250, 250));

                        //enemies! ~ lv 18 to 32
                        if (ii == 0)
                        {
                            AddRadiusDespawnData(layout, 120, 160);
                            AddRadiusRespawnData(layout, 80, 100, 80);
                            AddRadiusEnemySpawnData(layout, 120, new RandRange(100));
                        }
                        else
                        {
                            AddRadiusDespawnData(layout, 120, 120);
                            AddRadiusRespawnData(layout, 80, 120, 60);
                            AddRadiusEnemySpawnData(layout, 120, new RandRange(120));
                        }

                        //enemies


                        //items
                        AddItemData(layout, new RandRange(250, 250), 25);


                        //construct paths
                        {
                            if (ii == 0)
                                AddInitGridStep(layout, 32, 32, 8, 8);
                            else
                                AddInitGridStep(layout, 50, 50, 8, 8);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(90);
                            path.BranchRatio = new RandRange(0, 35);

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

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(85, 50));

                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, false);


                        //chest
                        int chest_amount = 0;
                        if (ii == 1)
                            chest_amount = 1;
                        else if (ii == 2)
                            chest_amount = 2;
                        else if (ii == 3)
                            chest_amount = 6;

                        for (int kk = 0; kk < chest_amount; kk++)
                        {
                            ChestStep<MapGenContext> chestStep = new ChestStep<MapGenContext>(false, GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                            chestStep.Items.Add(new MapItem("orb_escape"), 10);
                            chestStep.ItemThemes.Add(new ItemThemeNone(50, new RandRange(5, 10)), 10);
                            layout.GenSteps.Add(PR_HOUSES, chestStep);
                        }

                        {
                            SetCompassStep<MapGenContext> trapStep = new SetCompassStep<MapGenContext>("tile_compass");
                            layout.GenSteps.Add(new Priority(6, 1), trapStep);
                        }

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
                #endregion
            }
            else if (index == 17)
            {
                #region SNOWBOUND PATH
                {
                    zone.Name = new LocalText("Snowbound Path");
                    zone.Rescues = 2;
                    zone.Level = 40;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 16;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Snowbound Path\n{0}F")));

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

                        poolSpawn.Spawns.Add(GetTeamMob("swinub", "", "powder_snow", "mud_bomb", "", "", new RandRange(30), "wander_normal"), new IntRange(0, 7), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("piloswine", "", "ice_fang", "earthquake", "", "", new RandRange(38), "wander_normal"), new IntRange(11, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("jynx", "", "lovely_kiss", "draining_kiss", "ice_punch", "", new RandRange(36), "wander_normal"), new IntRange(11, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("snorunt", "", "frost_breath", "", "", "", new RandRange(36), "wander_normal"), new IntRange(6, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("vanillish", "", "icy_wind", "icicle_spear", "", "", new RandRange(35), "wander_normal"), new IntRange(0, 11), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("furret", "", "foresight", "follow_me", "rest", "", new RandRange(36), "wander_normal"), new IntRange(2, 15), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("altaria", "", "dragon_breath", "", "", "", new RandRange(36), "wander_normal"), new IntRange(11, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("snover", "", "ice_shard", "ingrain", "", "", new RandRange(36), "wander_normal"), new IntRange(0, 11), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("azumarill", "thick_fat", "aqua_tail", "aqua_ring", "", "", new RandRange(36), "wander_normal"), new IntRange(6, 15), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("vigoroth", "", "uproar", "chip_away", "", "", new RandRange(33), "wander_normal"), new IntRange(0, 7), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("hitmonlee", "", "wide_guard", "high_jump_kick", "", "", new RandRange(36), "wander_normal"), new IntRange(11, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("fearow", "", "agility", "mirror_move", "", "", new RandRange(36), "wander_normal"), new IntRange(6, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("golduck", "", "disable", "aqua_jet", "", "", new RandRange(36), "wander_normal"), new IntRange(0, 11), 10);

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
                                AddSpecificTextureData(layout, "frosty_forest_wall", "frosty_forest_floor", "frosty_forest_secondary", "tall_grass_white", "ice");
                            else if (ii < 11)
                                AddSpecificTextureData(layout, "mt_faraway_2_wall", "mt_faraway_2_floor", "mt_faraway_2_secondary", "tall_grass_white", "ice");
                            else
                                AddSpecificTextureData(layout, "sky_peak_7th_pass_wall", "sky_peak_7th_pass_floor", "sky_peak_7th_pass_secondary", "tall_grass_white", "ice");

                            if (ii < 11)
                                AddGrassSteps(layout, new RandRange(2, 5), new IntRange(2, 7), new RandRange(25));

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

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }


                    {
                        int max_floors = 6;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Glacial Path\nB{0}F")));

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


                        poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("ninetales", 1, "", Gender.Unknown), "", "ice_beam", "dazzling_gleam", "", "", new RandRange(40), "wander_normal"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("abomasnow", "", "wood_hammer", "ice_shard", "", "", new RandRange(40), "wander_normal"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("altaria", "", "dragon_breath", "", "", "", new RandRange(40), "wander_normal"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("piloswine", "", "ice_fang", "earthquake", "", "", new RandRange(40), "wander_normal"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("eiscue", "", "freeze_dry", "amnesia", "", "", new RandRange(40), "wander_normal"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("azumarill", "huge_power", "aqua_tail", "aqua_ring", "", "", new RandRange(40), "wander_normal"), new IntRange(0, max_floors), 10);
                        //spawns asleep, with choice specs
                        poolSpawn.Spawns.Add(GetTeamMob("glaceon", "", "blizzard", "", "", "", new RandRange(60), TeamMemberSpawn.MemberRole.Loner, "wander_normal"), new IntRange(0, max_floors), 10);

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

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 18)
            {
                #region RELIC TOWER
                {
                    zone.Name = new LocalText("Relic Tower");
                    zone.Rescues = 2;
                    zone.Level = 30;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    int max_floors = 13;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Relic Tower\n{0}F")));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level + 40, 0);
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;


                    //necessities
                    CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                    necessities.SpawnRates.SetRange(8, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("necessities", necessities);


                    necessities.Spawns.Add(new InvItem("berry_leppa", true), new IntRange(0, max_floors), 3);
                    necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 6);
                    //snacks
                    CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                    snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                    snacks.Spawns.Add(new InvItem("seed_ban", true), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("seed_ban"), new IntRange(0, max_floors), 8);
                    snacks.Spawns.Add(new InvItem("herb_power", true), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("herb_power"), new IntRange(0, max_floors), 8);
                    snacks.Spawns.Add(new InvItem("seed_warp", true), new IntRange(0, max_floors), 2);
                    snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, max_floors), 8);
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
                    boosters.Spawns.Add(new InvItem("gummi_orange"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_gold"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_red"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_royal"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_silver"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_white"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_yellow"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_sky"), new IntRange(0, max_floors), 1);
                    boosters.Spawns.Add(new InvItem("gummi_gray"), new IntRange(0, max_floors), 5);
                    boosters.Spawns.Add(new InvItem("gummi_magenta"), new IntRange(0, max_floors), 1);
                    //special
                    CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                    special.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("special", special);


                    special.Spawns.Add(new InvItem("apricorn_purple"), new IntRange(0, max_floors), 10);
                    special.Spawns.Add(new InvItem("machine_assembly_box"), new IntRange(0, max_floors), 10);
                    special.Spawns.Add(new InvItem("machine_ability_capsule"), new IntRange(0, max_floors), 5);
                    //evo
                    CategorySpawn<InvItem> evo = new CategorySpawn<InvItem>();
                    evo.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("evo", evo);


                    evo.Spawns.Add(new InvItem("evo_sun_ribbon"), new IntRange(0, max_floors), 10);
                    evo.Spawns.Add(new InvItem("evo_lunar_ribbon"), new IntRange(0, max_floors), 10);
                    //throwable
                    CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                    throwable.SpawnRates.SetRange(20, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                    throwable.Spawns.Add(new InvItem("wand_lure", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_slow", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_vanish", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_topsy_turvy", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_whirlwind", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_warp", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_lob", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_path", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_switcher", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_fear", false, 2), new IntRange(0, max_floors), 10);
                    //orbs
                    CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                    orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                    orbs.Spawns.Add(new InvItem("orb_pierce", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_pierce"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_mobile", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_mobile"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_mug", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_mug"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_nullify", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_nullify"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_trawl", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_trawl"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_weather", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_weather"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_fill_in", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_fill_in"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_endure", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_endure"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_cleanse", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_cleanse"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_slumber", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_slumber"), new IntRange(0, max_floors), 8);
                    orbs.Spawns.Add(new InvItem("orb_halving", true), new IntRange(0, max_floors), 2);
                    orbs.Spawns.Add(new InvItem("orb_halving"), new IntRange(0, max_floors), 8);
                    //held
                    CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                    held.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("held", held);


                    held.Spawns.Add(new InvItem("held_special_band", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_special_band"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_zinc_band", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_zinc_band"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_pass_scarf", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_pass_scarf"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_x_ray_specs", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_x_ray_specs"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_soft_sand", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_soft_sand"), new IntRange(0, max_floors), 7);
                    held.Spawns.Add(new InvItem("held_twisted_spoon", true), new IntRange(0, max_floors), 3);
                    held.Spawns.Add(new InvItem("held_twisted_spoon"), new IntRange(0, max_floors), 7);


                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;


                    poolSpawn.Spawns.Add(GetTeamMob("solrock", "", "confusion", "rock_polish", "", "", new RandRange(29), "wander_dumb"), new IntRange(4, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("meowstic", "", "psyshock", "charm", "", "", new RandRange(31), "wander_dumb"), new IntRange(8, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("meowstic", 1, "", Gender.Unknown), "", "psyshock", "charge_beam", "", "", new RandRange(31), "wander_dumb"), new IntRange(8, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("baltoy", "", "heal_block", "rapid_spin", "", "", new RandRange(26), "wander_dumb"), new IntRange(0, 8), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("shuppet", "", "spite", "curse", "", "", new RandRange(27), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("meditite", "", "meditate", "force_palm", "", "", new RandRange(27), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("natu", "", "teleport", "future_sight", "", "", new RandRange(22), "wander_dumb"), new IntRange(0, 4), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("ariados", "", "fell_stinger", "venom_drench", "", "", new RandRange(28), "wander_dumb"), new IntRange(4, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("bronzor", "", "psywave", "imprison", "", "", new RandRange(26), "wander_dumb"), new IntRange(0, 8), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("wobbuffet", "", "mirror_coat", "safeguard", "", "", new RandRange(31), "wander_dumb"), new IntRange(8, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("braixen", "", "fire_spin", "lucky_chant", "", "", new RandRange(28), "wander_dumb"), new IntRange(4, max_floors), 10);
                    //sleeping, holding choice scarf
                    poolSpawn.Spawns.Add(GetTeamMob("espeon", "magic_bounce", "psychic", "", "", "", new RandRange(5), "wander_normal"), new IntRange(0, max_floors), 10);


                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 3);


                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;

                    tileSpawn.Spawns.Add(new EffectTile("trap_apple", true), new IntRange(0, max_floors), 3);//apple trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_spin", false), new IntRange(0, max_floors), 10);//spin trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_seal", false), new IntRange(0, max_floors), 10);//seal trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_slow", false), new IntRange(0, max_floors), 10);//slow trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_warp", true), new IntRange(0, max_floors), 10);//warp trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_sticky", false), new IntRange(0, max_floors), 10);//sticky trap

                    floorSegment.ZoneSteps.Add(tileSpawn);


                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        if (ii < 4)
                            AddFloorData(layout, "B09. Relic Tower.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);
                        else
                            AddFloorData(layout, "B09. Relic Tower.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                        //Tilesets
                        if (ii < 8)
                            AddTextureData(layout, "temporal_unused_wall", "temporal_unused_floor", "temporal_unused_secondary", "psychic");
                        else
                            AddTextureData(layout, "buried_relic_3_wall", "buried_relic_3_floor", "buried_relic_3_secondary", "psychic");

                        if (ii < 4)
                            AddBlobWaterSteps(layout, "water", new RandRange(1, 4), new IntRange(2, 6), true);

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(6, 9));

                        //money - Ballpark 25K
                        AddMoneyData(layout, new RandRange(2, 4));

                        //enemies! ~ lv 22 to 32
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



                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
                #endregion
            }
            else if (index == 19)
                FillGuildmaster(zone);
            else if (index == 20)
            {
                #region OVERGROWN WILDS
                {
                    zone.Name = new LocalText("Overgrown Wilds");
                    zone.Rescues = 2;
                    zone.Level = 20;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 12;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Overgrown Wilds\n{0}F")));

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
                        snacks.SpawnRates.SetRange(15, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                        snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_sleep"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_blinker"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_hunger"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_vile"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("herb_white"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("herb_mental"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_ban"), new IntRange(0, max_floors), 5);
                        snacks.Spawns.Add(new InvItem("seed_decoy"), new IntRange(0, max_floors), 10);
                        //evo
                        CategorySpawn<InvItem> evo = new CategorySpawn<InvItem>();
                        evo.SpawnRates.SetRange(1, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("evo", evo);


                        evo.Spawns.Add(new InvItem("evo_leaf_stone"), new IntRange(0, max_floors), 10);
                        //boosters
                        CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                        boosters.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("boosters", boosters);


                        boosters.Spawns.Add(new InvItem("gummi_blue"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_black"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_clear"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_grass"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_green"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_brown"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_orange"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_gold"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_red"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_royal"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_silver"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_white"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_yellow"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_sky"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_gray"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_magenta"), new IntRange(0, max_floors), 1);
                        //special
                        CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                        special.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("special", special);


                        special.Spawns.Add(new InvItem("apricorn_plain"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_green"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_purple"), new IntRange(0, max_floors), 10);
                        //throwable
                        CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                        throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                        throwable.Spawns.Add(new InvItem("ammo_stick", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("ammo_cacnea_spike", false, 2), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_lob", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_switcher", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_fear", false, 3), new IntRange(0, max_floors), 10);
                        //held
                        CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                        held.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("held", held);


                        held.Spawns.Add(new InvItem("held_heal_ribbon"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_warp_scarf"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_metronome"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_big_root"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_miracle_seed"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_silver_powder"), new IntRange(0, max_floors), 10);
                        //tms
                        CategorySpawn<InvItem> tms = new CategorySpawn<InvItem>();
                        tms.SpawnRates.SetRange(7, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("tms", tms);


                        tms.Spawns.Add(new InvItem("tm_protect"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_round"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_rest"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_hidden_power"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_thief"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_dig"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_cut"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_grass_knot"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_fly"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_infestation"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_work_up"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_roar"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_flash"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_bullet_seed"), new IntRange(0, max_floors), 10);


                        floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                        //mobs
                        TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                        poolSpawn.Priority = PR_RESPAWN_MOB;


                        poolSpawn.Spawns.Add(GetTeamMob("taillow", "", "focus_energy", "quick_attack", "", "", new RandRange(17), "wander_dumb"), new IntRange(0, 8), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("grovyle", "", "pursuit", "fury_cutter", "", "", new RandRange(18), TeamMemberSpawn.MemberRole.Loner, "wander_dumb"), new IntRange(8, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("seedot", "", "harden", "nature_power", "", "", new RandRange(17), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("cherubi", "", "leech_seed", "tackle", "", "", new RandRange(17), "wander_dumb"), new IntRange(0, 4), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("bellsprout", "", "growth", "vine_whip", "", "", new RandRange(17), "wander_dumb"), new IntRange(4, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("exeggcute", "", "barrage", "reflect", "", "", new RandRange(17), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("flabebe", "", "fairy_wind", "", "", "", new RandRange(17), "turret"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("venonat", "", "leech_life", "poison_powder", "", "", new RandRange(17), "wander_dumb"), new IntRange(4, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("pineco", "", "bug_bite", "self_destruct", "", "", new RandRange(17), "turret"), new IntRange(8, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("girafarig", "", "stomp", "confusion", "", "", new RandRange(17), "wander_dumb"), new IntRange(4, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("snivy", "", "growth", "vine_whip", "", "", new RandRange(14), "wander_dumb"), new IntRange(0, 4), 10);
                        //MH Only
                        poolSpawn.Spawns.Add(GetTeamMob("heracross", "", "horn_attack", "arm_thrust", "", "", new RandRange(20), "wander_dumb"), new IntRange(0, max_floors), 10);

                        poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                        floorSegment.ZoneSteps.Add(poolSpawn);

                        TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                        tileSpawn.Priority = PR_RESPAWN_TRAP;


                        tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_poison", false), new IntRange(0, max_floors), 10);//poison trap

                        floorSegment.ZoneSteps.Add(tileSpawn);


                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            if ((ii / 2) % 2 == 0)
                                AddFloorData(layout, "B22. Overgrown Wilds.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);
                            else
                                AddFloorData(layout, "B22. Overgrown Wilds.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                            //Tilesets
                            if (ii < 4)
                                AddSpecificTextureData(layout, "lush_prairie_wall", "lush_prairie_floor", "lush_prairie_secondary", "tall_grass", "grass");
                            else if (ii < 8)
                                AddSpecificTextureData(layout, "purity_forest_9_wall", "purity_forest_9_floor", "purity_forest_9_secondary", "tall_grass", "grass");
                            else
                                AddSpecificTextureData(layout, "southern_jungle_wall", "southern_jungle_floor", "southern_jungle_secondary", "tall_grass_dark", "grass");

                            if (ii < 8)
                                AddWaterSteps(layout, "water", new RandRange(20));//water

                            if (ii < 4)
                                AddGrassSteps(layout, new RandRange(3, 7), new IntRange(2, 6), new RandRange(20));
                            else
                                AddGrassSteps(layout, new RandRange(4, 8), new IntRange(4, 11), new RandRange(40));

                            //traps
                            AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                            AddTrapsSteps(layout, new RandRange(6, 9));

                            //money - 750P to 3,150P
                            AddMoneyData(layout, new RandRange(2, 4));

                            //enemies! ~ up to lv 15
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

                            floorSegment.Floors.Add(layout);
                        }
                        //floorSegment.MainExit = new ZoneLoc(1, new SegLoc(-1, 4));
                        zone.Segments.Add(floorSegment);
                    }


                    {
                        int max_floors = 4;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Exotic Wilds\nB{0}F")));

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


                        necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 9);
                        necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 12);
                        necessities.Spawns.Add(new InvItem("food_apple"), new IntRange(0, max_floors), 10);
                        necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 10);
                        necessities.Spawns.Add(new InvItem("seed_reviver"), new IntRange(0, max_floors), 5);
                        necessities.Spawns.Add(new InvItem("food_apple_big"), new IntRange(0, max_floors), 3);
                        //snacks
                        CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                        snacks.SpawnRates.SetRange(20, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                        snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_sleep"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_blinker"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_hunger"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_vile"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("herb_white"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("herb_mental"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_pure"), new IntRange(0, max_floors), 5);
                        snacks.Spawns.Add(new InvItem("seed_decoy"), new IntRange(0, max_floors), 10);
                        //evo
                        CategorySpawn<InvItem> evo = new CategorySpawn<InvItem>();
                        evo.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("evo", evo);


                        evo.Spawns.Add(new InvItem("evo_leaf_stone"), new IntRange(0, max_floors), 10);
                        //boosters
                        CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                        boosters.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("boosters", boosters);


                        boosters.Spawns.Add(new InvItem("gummi_blue"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_black"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_clear"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_grass"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_green"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_brown"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_orange"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_gold"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_red"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_royal"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_silver"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_white"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_yellow"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_sky"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_gray"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_magenta"), new IntRange(0, max_floors), 1);
                        //special
                        CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                        special.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("special", special);


                        special.Spawns.Add(new InvItem("apricorn_plain"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_green"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_purple"), new IntRange(0, max_floors), 10);
                        //throwable
                        CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                        throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                        throwable.Spawns.Add(new InvItem("ammo_stick", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("ammo_cacnea_spike", false, 2), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_lob", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_switcher", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_fear", false, 3), new IntRange(0, max_floors), 10);
                        //held
                        CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                        held.SpawnRates.SetRange(4, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("held", held);


                        held.Spawns.Add(new InvItem("held_heal_ribbon"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_warp_scarf"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_metronome"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_big_root"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_miracle_seed"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_silver_powder"), new IntRange(0, max_floors), 10);
                        //tms
                        CategorySpawn<InvItem> tms = new CategorySpawn<InvItem>();
                        tms.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("tms", tms);


                        tms.Spawns.Add(new InvItem("tm_protect"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_round"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_rest"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_hidden_power"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_thief"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_dig"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_cut"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_grass_knot"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_fly"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_infestation"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_work_up"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_roar"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_flash"), new IntRange(0, max_floors), 10);
                        tms.Spawns.Add(new InvItem("tm_bullet_seed"), new IntRange(0, max_floors), 10);


                        floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                        //mobs
                        TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                        poolSpawn.Priority = PR_RESPAWN_MOB;


                        poolSpawn.Spawns.Add(GetTeamMob("tropius", "", "whirlwind", "magical_leaf", "", "", new RandRange(21), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("leavanny", "", "razor_leaf", "bug_bite", "", "", new RandRange(21), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("weepinbell", "", "vine_whip", "poison_powder", "", "", new RandRange(21), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("yanma", "speed_boost", "sonic_boom", "", "", "", new RandRange(21), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("duskull", "", "shadow_sneak", "astonish", "", "", new RandRange(21), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("servine", "", "leaf_tornado", "", "", "", new RandRange(21), "wander_dumb"), new IntRange(0, max_floors), 10);
                        //sleeping, holding scope lens
                        poolSpawn.Spawns.Add(GetTeamMob("leafeon", "", "leaf_blade", "aerial_ace", "x_scissor", "", new RandRange(50), "wander_normal"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("pineco", "", "bug_bite", "take_down", "", "", new RandRange(21), "turret"), new IntRange(0, max_floors), 10);
                        //MH Only
                        poolSpawn.Spawns.Add(GetTeamMob("heracross", "", "horn_attack", "arm_thrust", "", "", new RandRange(20), "wander_dumb"), new IntRange(0, max_floors), 10);

                        poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                        floorSegment.ZoneSteps.Add(poolSpawn);

                        TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                        tileSpawn.Priority = PR_RESPAWN_TRAP;

                        tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_poison", false), new IntRange(0, max_floors), 10);//poison trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_apple", true), new IntRange(0, max_floors), 3);//apple trap

                        floorSegment.ZoneSteps.Add(tileSpawn);


                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            AddFloorData(layout, "B22. Overgrown Wilds.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                            //Tilesets
                            AddSpecificTextureData(layout, "mystery_jungle_1_wall", "mystery_jungle_1_floor", "mystery_jungle_1_secondary", "tall_grass_dark", "grass");

                            AddWaterSteps(layout, "water", new RandRange(20));//water

                            AddGrassSteps(layout, new RandRange(4, 8), new IntRange(4, 11), new RandRange(20));

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

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 21)
            {
                #region WAYWARD WETLANDS
                {
                    zone.Name = new LocalText("**Wayward Wetlands");
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Wayward Wetlands\nB{0}F")));

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

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, 15), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    floorSegment.ZoneSteps.Add(tileSpawn);



                    for (int ii = 0; ii < 15; ii++)
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


                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
                #endregion
            }
            else if (index == 22)
            {
                #region TRICKSTER WOODS
                {
                    zone.Name = new LocalText("Trickster Woods");
                    zone.Rescues = 2;
                    zone.Level = 20;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 10;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Trickster Woods\n{0}F")));

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
                        //snacks
                        CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                        snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                        snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_sleep"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_blinker"), new IntRange(0, max_floors), 10);
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
                        boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_red"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_royal"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_silver"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_white"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_yellow"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_sky"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_gray"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_magenta"), new IntRange(0, max_floors), 1);
                        //special
                        CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                        special.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("special", special);


                        special.Spawns.Add(new InvItem("apricorn_plain"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_brown"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_purple"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_black"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_white"), new IntRange(0, max_floors), 10);
                        //throwable
                        CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                        throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                        throwable.Spawns.Add(new InvItem("ammo_stick", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_whirlwind", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_slow", false, 2), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("ammo_gravelerock", false, 2), new IntRange(0, max_floors), 10);
                        //orbs
                        CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                        orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                        orbs.Spawns.Add(new InvItem("orb_trap_see"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_foe_hold"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_all_dodge"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_slow"), new IntRange(0, max_floors), 10);
                        //held
                        CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                        held.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("held", held);


                        held.Spawns.Add(new InvItem("held_power_band"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_defense_scarf"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_special_band"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_zinc_band"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_spell_tag"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_silk_scarf"), new IntRange(0, max_floors), 10);


                        floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                        //mobs
                        TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                        poolSpawn.Priority = PR_RESPAWN_MOB;


                        poolSpawn.Spawns.Add(GetTeamMob("fennekin", "", "ember", "", "", "", new RandRange(14), "wander_dumb"), new IntRange(0, 5), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("paras", "", "leech_life", "", "", "", new RandRange(16), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("luxio", "", "spark", "charge", "", "", new RandRange(18), "wander_dumb"), new IntRange(5, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("slowpoke", "", "curse", "tackle", "", "", new RandRange(19), "wander_dumb"), new IntRange(0, 5), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("rattata", "", "hyper_fang", "", "", "", new RandRange(18), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("mankey", "", "seismic_toss", "", "", "", new RandRange(17), "wander_dumb"), new IntRange(5, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("sandshrew", "", "rapid_spin", "", "", "", new RandRange(17), "wander_dumb"), new IntRange(5, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("bidoof", "simple", "defense_curl", "tackle", "", "", new RandRange(16), "wander_dumb"), new IntRange(0, 5), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("poochyena", "", "howl", "bite", "", "", new RandRange(14), "wander_dumb"), new IntRange(5, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("abra", "", "teleport", "", "", "", new RandRange(12), "wander_normal"), new IntRange(5, max_floors), 10);

                        poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                        floorSegment.ZoneSteps.Add(poolSpawn);

                        TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                        tileSpawn.Priority = PR_RESPAWN_TRAP;


                        tileSpawn.Spawns.Add(new EffectTile("trap_mud", false), new IntRange(0, max_floors), 10);//mud trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_slow", false), new IntRange(0, max_floors), 10);//slow trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, max_floors), 10);//chestnut trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_gust", false), new IntRange(0, max_floors), 10);//gust trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_poison", false), new IntRange(0, max_floors), 10);//poison trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_self_destruct", false), new IntRange(0, max_floors), 10);//selfdestruct trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_slumber", false), new IntRange(0, max_floors), 10);//sleep trap


                        floorSegment.ZoneSteps.Add(tileSpawn);


                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            AddFloorData(layout, "B25. Trickster Woods.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Clear);

                            //Tilesets
                            //other candidates: purity_forest_6_wall,uproar_forest
                            if (ii < 5)
                                AddTextureData(layout, "pitfall_valley_1_wall", "pitfall_valley_1_floor", "pitfall_valley_1_secondary", "normal");
                            else
                                AddTextureData(layout, "grass_maze_wall", "grass_maze_floor", "grass_maze_secondary", "normal");

                            //traps
                            AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile

                            SpawnList<PatternPlan> patternList = new SpawnList<PatternPlan>();
                            patternList.Add(new PatternPlan("pattern_crosshair", PatternPlan.PatternExtend.Single), 10);
                            AddTrapPatternSteps(layout, new RandRange(5), patternList);

                            AddTrapsSteps(layout, new RandRange(4, 7));

                            //money - Ballpark 25K
                            AddMoneyData(layout, new RandRange(2, 4));

                            //enemies! ~ lv 22 to 32
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


                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }


                    {
                        int max_floors = 4;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Trickster Maze\nB{0}F")));

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


                        poolSpawn.Spawns.Add(GetTeamMob("buizel", "", "aqua_jet", "", "", "", new RandRange(20), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("woobat", "", "heart_stamp", "", "", "", new RandRange(22), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("hatenna", "", "life_dew", "disarming_voice", "", "", new RandRange(20), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("combee", "", "gust", "", "", "", new RandRange(18), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("houndour", "", "roar", "smog", "", "", new RandRange(20), "wander_dumb"), new IntRange(0, max_floors), 10);
                        //form depends on version
                        poolSpawn.Spawns.Add(GetTeamMob("deerling", "", "take_down", "leech_seed", "", "", new RandRange(19), "wander_dumb"), new IntRange(0, max_floors), 10);

                        poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                        floorSegment.ZoneSteps.Add(poolSpawn);

                        TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                        tileSpawn.Priority = PR_RESPAWN_TRAP;

                        tileSpawn.Spawns.Add(new EffectTile("trap_mud", false), new IntRange(0, max_floors), 10);//mud trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_slow", false), new IntRange(0, max_floors), 10);//slow trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, max_floors), 10);//chestnut trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_gust", false), new IntRange(0, max_floors), 10);//gust trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_poison", false), new IntRange(0, max_floors), 10);//poison trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_self_destruct", false), new IntRange(0, max_floors), 10);//selfdestruct trap
                        tileSpawn.Spawns.Add(new EffectTile("trap_slumber", false), new IntRange(0, max_floors), 10);//sleep trap

                        tileSpawn.Spawns.Add(new EffectTile("trap_trigger", true), new IntRange(0, max_floors), 20);//trigger trap


                        floorSegment.ZoneSteps.Add(tileSpawn);


                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            AddFloorData(layout, "B01. Demonstration.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                            //Tilesets
                            AddTextureData(layout, "purity_forest_8_wall", "purity_forest_8_floor", "purity_forest_8_secondary", "normal");

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

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 23)
            {
                #region LAVA FLOE ISLAND
                {
                    zone.Name = new LocalText("Lava Floe Island");
                    zone.Rescues = 2;
                    zone.Level = 20;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 16;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Lava Floe\nIsland\n{0}F")));

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
                        //snacks
                        CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                        snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                        snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, max_floors), 10);
                        snacks.Spawns.Add(new InvItem("seed_sleep"), new IntRange(0, max_floors), 10);
                        //boosters
                        CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                        boosters.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("boosters", boosters);


                        boosters.Spawns.Add(new InvItem("gummi_blue"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_black"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_clear"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_grass"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_green"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_brown"), new IntRange(0, max_floors), 5);
                        boosters.Spawns.Add(new InvItem("gummi_orange"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_gold"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 1);
                        boosters.Spawns.Add(new InvItem("gummi_red"), new IntRange(0, max_floors), 5);
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


                        special.Spawns.Add(new InvItem("apricorn_plain"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_blue"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_red"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("apricorn_brown"), new IntRange(0, max_floors), 10);
                        special.Spawns.Add(new InvItem("machine_assembly_box"), new IntRange(0, max_floors), 10);
                        //throwable
                        CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                        throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                        throwable.Spawns.Add(new InvItem("ammo_geo_pebble", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("ammo_corsola_twig", false, 2), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_pounce", false, 2), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_whirlwind", false, 2), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_slow", false, 2), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_path", false, 3), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_switcher", false, 2), new IntRange(0, max_floors), 10);
                        throwable.Spawns.Add(new InvItem("wand_fear", false, 2), new IntRange(0, max_floors), 10);
                        //orbs
                        CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                        orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                        orbs.Spawns.Add(new InvItem("orb_rebound"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_luminous"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_trapbust"), new IntRange(0, max_floors), 10);
                        orbs.Spawns.Add(new InvItem("orb_foe_hold"), new IntRange(0, max_floors), 10);
                        //held
                        CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
                        held.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                        itemSpawnZoneStep.Spawns.Add("held", held);


                        held.Spawns.Add(new InvItem("held_scope_lens"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_warp_scarf"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_metronome"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_wide_lens"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_charcoal"), new IntRange(0, max_floors), 10);
                        held.Spawns.Add(new InvItem("held_mystic_water"), new IntRange(0, max_floors), 10);


                        floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                        //mobs
                        TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                        poolSpawn.Priority = PR_RESPAWN_MOB;


                        poolSpawn.Spawns.Add(GetTeamMob("finneon", "", "attract", "water_gun", "", "", new RandRange(17), "wander_dumb"), new IntRange(0, 8), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("krabby", "", "harden", "vice_grip", "", "", new RandRange(17), "wander_dumb"), new IntRange(0, 8), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("spheal", "", "ice_ball", "brine", "", "", new RandRange(17), "wander_dumb"), new IntRange(8, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("shellos", "", "hidden_power", "mud_slap", "", "", new RandRange(17), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("magikarp", "", "splash", "", "", "", new RandRange(10), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("remoraid", "", "lock_on", "bubble_beam", "psybeam", "", new RandRange(19), "wander_dumb"), new IntRange(8, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("slowpoke", "", "yawn", "curse", "tackle", "", new RandRange(20), "wander_dumb"), new IntRange(8, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("growlithe", "", "roar", "flame_wheel", "", "", new RandRange(18), "wander_dumb"), new IntRange(0, 8), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("magby", "", "fire_spin", "smokescreen", "", "", new RandRange(17), "wander_dumb"), new IntRange(0, 8), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("salandit", "", "sweet_scent", "ember", "", "", new RandRange(17), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("numel", "", "flame_burst", "", "", "", new RandRange(20), "wander_dumb"), new IntRange(8, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("slugma", "", "incinerate", "rock_throw", "", "", new RandRange(20), "wander_dumb"), new IntRange(8, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("sandshrew", "", "rapid_spin", "rollout", "", "", new RandRange(20), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("castform", "", "headbutt", "sunny_day", "rain_dance", "", new RandRange(20), "wander_dumb"), new IntRange(8, max_floors), 5);
                        poolSpawn.Spawns.Add(GetTeamMob("machop", "", "seismic_toss", "", "", "", new RandRange(18), "wander_dumb"), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("drifloon", "", "minimize", "constrict", "astonish", "", new RandRange(18), "wander_dumb"), new IntRange(8, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("tepig", "", "defense_curl", "flame_charge", "", "", new RandRange(14), "wander_dumb"), new IntRange(0, 8), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("oshawott", "", "focus_energy", "water_gun", "", "", new RandRange(14), "wander_dumb"), new IntRange(0, 8), 10);

                        poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                        floorSegment.ZoneSteps.Add(poolSpawn);

                        TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                        tileSpawn.Priority = PR_RESPAWN_TRAP;
                        floorSegment.ZoneSteps.Add(tileSpawn);


                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            if (ii < 8)
                                AddFloorData(layout, "B18. Faultline Ridge.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Clear);
                            else
                                AddFloorData(layout, "B18. Faultline Ridge.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                            //Tilesets
                            //other caniddates: side_path,lower_brine_cave
                            if (ii < 8)
                            {
                                if (ii % 2 == 0)
                                    AddTextureData(layout, "side_path_wall", "side_path_floor", "side_path_secondary", "water");
                                else
                                    AddTextureData(layout, "mt_blaze_wall", "mt_blaze_floor", "mt_blaze_secondary", "fire");
                            }
                            else
                            {
                                if (ii % 2 == 0)
                                    AddTextureData(layout, "craggy_coast_wall", "craggy_coast_floor", "craggy_coast_secondary", "water");
                                else
                                    AddTextureData(layout, "magma_cavern_2_wall", "magma_cavern_2_floor", "magma_cavern_2_secondary", "fire");
                            }

                            if (ii % 2 == 0)
                                AddWaterSteps(layout, "water", new RandRange(100));//water
                            else
                                AddWaterSteps(layout, "lava", new RandRange(100));//lava


                            //traps
                            AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                            AddTrapsSteps(layout, new RandRange(6, 9));

                            //money - Ballpark 25K
                            AddMoneyData(layout, new RandRange(2, 4));

                            //enemies! ~ lv 22 to 32
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



                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }


                    {
                        int max_floors = 10;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Abyssal Island\nB{0}F")));

                        //money
                        MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 0);
                        moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                        floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                        //items
                        ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                        itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;

                        floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                        //mobs
                        TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                        poolSpawn.Priority = PR_RESPAWN_MOB;

                        poolSpawn.Spawns.Add(GetTeamMob("gyarados", "", "splash", "dragon_dance", "aqua_tail", "", new RandRange(62)), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("drifblim", "", "ominous_wind", "minimize", "baton_pass", "", new RandRange(62)), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("camerupt", "", "eruption", "earthquake", "", "", new RandRange(62)), new IntRange(4, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("machamp", "", "seismic_toss", "dynamic_punch", "", "", new RandRange(62)), new IntRange(0, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("octillery", "", "hyper_beam", "gunk_shot", "", "", new RandRange(62)), new IntRange(0, 5), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("salazzle", "", "toxic", "flame_burst", "", "", new RandRange(62)), new IntRange(0, 5), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("castform", "", "weather_ball", "sunny_day", "rain_dance", "", new RandRange(62)), new IntRange(4, max_floors), 5);
                        poolSpawn.Spawns.Add(GetTeamMob("gastrodon", "", "muddy_water", "recover", "", "", new RandRange(62)), new IntRange(4, max_floors), 10);

                        poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                        floorSegment.ZoneSteps.Add(poolSpawn);

                        TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                        tileSpawn.Priority = PR_RESPAWN_TRAP;
                        floorSegment.ZoneSteps.Add(tileSpawn);


                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            AddFloorData(layout, "B02. Demonstration 2.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                            AddWaterSteps(layout, "floor", new RandRange(20));//empty


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

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 24)
            {
                #region FISSURE QUARRY
                {
                    zone.Name = new LocalText("Fissure Quarry");
                    zone.Rescues = 2;
                    zone.Level = 25;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 11;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Fissure Quarry\n{0}F")));

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
                        snacks.Spawns.Add(new InvItem("berry_babiri"), new IntRange(0, max_floors), 5);
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
                        poolSpawn.Spawns.Add(GetTeamMob("togedemaru", "", "rollout", "spark", "", "", new RandRange(24), "wander_dumb"), new IntRange(3, 7), 5);
                        poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("grimer", 1, "", Gender.Unknown), "", "bite", "poison_fang", "", "", new RandRange(26), "wander_dumb"), new IntRange(7, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("golbat", "", "screech", "leech_life", "", "", new RandRange(24), "wander_dumb"), new IntRange(0, 7), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("rhyhorn", "", "bulldoze", "", "", "", new RandRange(22), "wander_dumb"), new IntRange(0, 7), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("diglett", "", "dig", "", "", "", new RandRange(22), "wander_dumb"), new IntRange(0, 3), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("dugtrio", "", "dig", "sucker_punch", "", "", new RandRange(26), "wander_dumb"), new IntRange(7, max_floors), 10);
                        poolSpawn.Spawns.Add(GetTeamMob("steelix", "", "dragon_breath", "iron_tail", "", "", new RandRange(28), "wander_dumb"), new IntRange(7, max_floors), 5);

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


                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            AddFloorData(layout, "B20. Fissure Quarry.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                            //Tilesets
                            //other candidates: mt_steel,silent_chasm,great_canyon
                            if (ii < 7)
                                AddTextureData(layout, "solar_cave_1_wall", "solar_cave_1_floor", "solar_cave_1_secondary", "steel");
                            else
                                AddTextureData(layout, "rock_maze_wall", "rock_maze_floor", "rock_maze_secondary", "steel");

                            //traps
                            AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                            AddTrapsSteps(layout, new RandRange(6, 9));

                            //money - Ballpark 25K
                            AddMoneyData(layout, new RandRange(2, 4));

                            //enemies! ~ lv 22 to 32
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


                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }


                    {
                        int max_floors = 4;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Magnetic Quarry\nB{0}F")));

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


                        necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 9);
                        necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 6);
                        necessities.Spawns.Add(new InvItem("food_apple"), new IntRange(0, max_floors), 10);
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
                        snacks.Spawns.Add(new InvItem("berry_babiri"), new IntRange(0, max_floors), 5);
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
                        poolSpawn.Spawns.Add(GetTeamMob("steelix", "", "dragon_breath", "iron_tail", "", "", new RandRange(28), "wander_dumb"), new IntRange(0, max_floors), 5);
                        poolSpawn.Spawns.Add(GetTeamMob("golbat", "", "screech", "leech_life", "", "", new RandRange(24), "wander_dumb"), new IntRange(0, max_floors), 10);
                        //In Groups
                        poolSpawn.Spawns.Add(GetTeamMob("aron", "", "iron_head", "harden", "", "", new RandRange(24), "wander_dumb"), new IntRange(0, max_floors), 10);

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


                        for (int ii = 0; ii < max_floors; ii++)
                        {
                            GridFloorGen layout = new GridFloorGen();

                            //Floor settings
                            AddFloorData(layout, "B21. Magnetic Quarry.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                            //Tilesets
                            AddTextureData(layout, "zero_isle_east_4_wall", "zero_isle_east_4_floor", "zero_isle_east_4_secondary", "steel");

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

                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
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
                #region CAVE OF SOLACE
                {
                    zone.Name = new LocalText("**Cave of Solace");
                    zone.BagRestrict = 0;
                    zone.MoneyRestrict = true;
                    zone.TeamSize = 1;
                    zone.Level = 80;
                    zone.NoEXP = true;
                    zone.Rescues = 4;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Cave of Solace\nB{0}F")));

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


                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
                #endregion
            }
            else if (index == 32)
            {
                #region ROYAL HALLS
                {
                    zone.Name = new LocalText("**Royal Halls");
                    zone.Rescues = 4;
                    zone.Level = 60;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Royal Halls\nB{0}F")));

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


                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
                #endregion
            }
            else if (index == 33)
            {
                #region THE SKY
                {
                    zone.Name = new LocalText("**The Sky");
                    zone.TeamSize = 3;
                    zone.Rescues = 4;
                    zone.Level = 60;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("The Sky\n{0}F")));

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

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, 20), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    floorSegment.ZoneSteps.Add(tileSpawn);


                    for (int ii = 0; ii < 20; ii++)
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


                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
                #endregion
            }
            else if (index == 34)
            {
                #region THE ABYSS
                {
                    zone.Name = new LocalText("**The Abyss");
                    zone.Level = 90;
                    zone.LevelCap = true;
                    zone.BagRestrict = 0;
                    zone.MoneyRestrict = true;
                    zone.TeamSize = 2;
                    zone.TeamRestrict = true;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("The Abyss\nB{0}F")));

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


                    SpawnList<IGenPriority> evoZoneSpawns = new SpawnList<IGenPriority>();
                    SpreadStepZoneStep evoItemZoneStep = new SpreadStepZoneStep(new SpreadPlanQuota(new RandRange(2, 4), new IntRange(0, 15)), evoZoneSpawns);//evo items
                    floorSegment.ZoneSteps.Add(evoItemZoneStep);


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
                            AddFloorData(layout, "B29. Treacherous Mountain.ogg", 3000, Map.SightRange.Dark, Map.SightRange.Dark);
                        else if (ii < 20)
                            AddFloorData(layout, "B26. Ambush Forest.ogg", 3000, Map.SightRange.Dark, Map.SightRange.Dark);
                        else if (ii < 30)
                            AddFloorData(layout, "B30. Treacherous Mountain 2.ogg", 3000, Map.SightRange.Dark, Map.SightRange.Dark);
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


                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
                #endregion
            }
            else if (index == 35)
                FillTrainingMaze(zone);
            else if (index == 36)
            {
                zone.Name = new LocalText("Bramble Woods");
                zone.Rescues = 2;
                zone.Level = 10;
                zone.Rogue = RogueStatus.ItemTransfer;
                #region BRAMBLE WOODS
                {
                    int max_floors = 7;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Bramble Woods\nB{0}F")));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 0);
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;

                    //necessities
                    CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                    necessities.SpawnRates.SetRange(50, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("necessities", necessities);

                    necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 9);//Leppa
                    necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 12);//Oran
                    necessities.Spawns.Add(new InvItem("food_apple"), new IntRange(0, max_floors), 10);//Apple
                    necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 80);//Lum
                    necessities.Spawns.Add(new InvItem("seed_reviver"), new IntRange(0, max_floors), 5);//reviver seed

                    //snacks
                    CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                    snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("snacks", snacks);

                    snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, max_floors), 20);//blast seed
                    snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, max_floors), 10);//warp seed
                    snacks.Spawns.Add(new InvItem("seed_sleep"), new IntRange(0, max_floors), 10);//sleep seed

                    //wands
                    CategorySpawn<InvItem> ammo = new CategorySpawn<InvItem>();
                    ammo.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("ammo", ammo);

                    ammo.Spawns.Add(new InvItem("ammo_stick", false, 3), new IntRange(0, max_floors), 10);//stick
                    ammo.Spawns.Add(new InvItem("wand_whirlwind", false, 2), new IntRange(0, max_floors), 10);//whirlwind wand
                    ammo.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, max_floors), 10);//pounce wand
                    ammo.Spawns.Add(new InvItem("wand_warp", false, 1), new IntRange(0, max_floors), 10);//warp wand
                    ammo.Spawns.Add(new InvItem("wand_lob", false, 2), new IntRange(0, max_floors), 10);//lob wand
                    ammo.Spawns.Add(new InvItem("ammo_geo_pebble", false, 2), new IntRange(0, max_floors), 10);//Geo Pebble

                    //orbs
                    CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                    orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("orbs", orbs);

                    orbs.Spawns.Add(new InvItem("orb_rebound"), new IntRange(0, max_floors), 10);//Rebound
                    orbs.Spawns.Add(new InvItem("orb_all_protect"), new IntRange(0, max_floors), 5);//All Protect
                    orbs.Spawns.Add(new InvItem("orb_luminous"), new IntRange(0, max_floors), 9);//Luminous
                    orbs.Spawns.Add(new InvItem("orb_petrify"), new IntRange(0, max_floors), 10);//Petrify
                    orbs.Spawns.Add(new InvItem("orb_slumber"), new IntRange(0, max_floors), 8);//Slumber Orb
                    orbs.Spawns.Add(new InvItem("orb_mirror"), new IntRange(0, max_floors), 8);//Mirror Orb

                    //special
                    CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                    special.SpawnRates.SetRange(4, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("special", special);

                    int rate = 2;
                    special.Spawns.Add(new InvItem("apricorn_blue"), new IntRange(0, max_floors), rate);//blue apricorns
                    special.Spawns.Add(new InvItem("apricorn_green"), new IntRange(0, max_floors), rate);//green apricorns
                    special.Spawns.Add(new InvItem("apricorn_white"), new IntRange(0, max_floors), rate);//white apricorns
                    special.Spawns.Add(new InvItem("apricorn_red"), new IntRange(0, max_floors), rate);//red apricorns
                    special.Spawns.Add(new InvItem("apricorn_yellow"), new IntRange(0, max_floors), rate);//yellow apricorns

                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;

                    {
                        //032 Nidoran♂ : 079 Rivalry : 043 Leer : 064 Peck
                        TeamMemberSpawn teamSpawn = GetTeamMob("nidoran_m", "poison_point", "leer", "peck", "", "", new RandRange(7));
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(0, 2));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 10);
                    }
                    {
                        //029 Nidoran♀ : 079 Rivalry : 045 Growl : 010 Scratch
                        TeamMemberSpawn teamSpawn = GetTeamMob("nidoran_f", "poison_point", "growl", "scratch", "", "", new RandRange(7));
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(1, 2));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 10);
                    }
                    // 13 Weedle : 40 Poison Sting
                    poolSpawn.Spawns.Add(GetTeamMob("weedle", "", "poison_sting", "", "", "", new RandRange(5), "wander_dumb"), new IntRange(0, max_floors), 10);
                    // 10 Caterpie : 81 String Shot : 33 Tackle
                    poolSpawn.Spawns.Add(GetTeamMob("caterpie", "", "string_shot", "tackle", "", "", new RandRange(5), "wander_dumb"), new IntRange(0, max_floors), 10);
                    // 406 Budew : 30 Natural Cure : 71 Absorb : 78 Stun Spore
                    poolSpawn.Spawns.Add(GetTeamMob("budew", "poison_point", "absorb", "stun_spore", "", "", new RandRange(7), "wander_dumb"), new IntRange(0, 3), 10);
                    // 285 Shroomish : 90 Poison Heal : 73 Leech Seed : 33 Tackle
                    poolSpawn.Spawns.Add(GetTeamMob("shroomish", "poison_heal", "leech_seed", "tackle", "", "", new RandRange(8), "wander_dumb"), new IntRange(3, max_floors), 10);
                    // 165 Ledyba : 48 Supersonic : 4 Comet Punch
                    poolSpawn.Spawns.Add(GetTeamMob("ledyba", "", "supersonic", "comet_punch", "", "", new RandRange(7), "wander_dumb"), new IntRange(3, max_floors), 10);

                    // 14 Kakuna : 106 Harden
                    poolSpawn.Spawns.Add(GetTeamMob("kakuna", "", "harden", "", "", "", new RandRange(8), "wait_attack"), new IntRange(3, max_floors), 10);
                    // 11 Metapod : 106 Harden
                    poolSpawn.Spawns.Add(GetTeamMob("metapod", "", "harden", "", "", "", new RandRange(8), "wait_attack"), new IntRange(3, max_floors), 10);

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);


                    RandBag<IGenPriority> npcZoneSpawns = new RandBag<IGenPriority>();
                    npcZoneSpawns.RemoveOnRoll = true;
                    //Supersonic's range
                    {
                        PresetMultiTeamSpawner<ListMapGenContext> multiTeamSpawner = new PresetMultiTeamSpawner<ListMapGenContext>();
                        MobSpawn post_mob = new MobSpawn();
                        post_mob.BaseForm = new MonsterID("sandshrew", 0, "normal", Gender.Male);
                        post_mob.Tactic = "slow_wander";
                        post_mob.Level = new RandRange(14);
                        post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_ADVICE_RANGE"))));
                        SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);
                        post_team.Explorer = true;
                        multiTeamSpawner.Spawns.Add(post_team);
                        PlaceRandomMobsStep<ListMapGenContext> randomSpawn = new PlaceRandomMobsStep<ListMapGenContext>(multiTeamSpawner);
                        randomSpawn.Ally = true;
                        npcZoneSpawns.ToSpawn.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_SPAWN_MOBS_EXTRA, randomSpawn));
                    }
                    SpreadStepZoneStep npcZoneStep = new SpreadStepZoneStep(new SpreadPlanQuota(new RandRange(2), new IntRange(2, 6), true), npcZoneSpawns);
                    floorSegment.ZoneSteps.Add(npcZoneStep);


                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    floorSegment.ZoneSteps.Add(tileSpawn);


                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        AddFloorData(layout, "B06. Bramble Woods.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                        if (ii < 3)
                        {

                        }
                        else
                            AddGrassSteps(layout, new RandRange(2, 5), new IntRange(2, 6), new RandRange(50));

                        //Tilesets
                        AddSpecificTextureData(layout, "mystifying_forest_wall", "mystifying_forest_floor", "mystifying_forest_secondary", "tall_grass_dark", "bug");

                        //traps
                        AddSingleTrapStep(layout, new RandRange(5, 7), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(5, 7));

                        //money - 315P to 1,260P
                        AddMoneyData(layout, new RandRange(1, 4));

                        //items
                        AddItemData(layout, new RandRange(3, 7), 25);

                        //enemies! ~ lv 5 to 10
                        AddRespawnData(layout, 6, 80);

                        //enemies
                        AddEnemySpawnData(layout, 20, new RandRange(3, 6));


                        //construct paths
                        if (ii < 3)
                        {
                            AddInitGridStep(layout, 4, 3, 10, 10);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(90);
                            path.BranchRatio = new RandRange(0, 25);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cave
                            genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(5, 10), new RandRange(5, 10)), 10);
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
                            AddInitGridStep(layout, 4, 4, 9, 9);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(90);
                            path.BranchRatio = new RandRange(0, 35);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cave
                            genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(4, 9), new RandRange(4, 9)), 10);
                            //round
                            genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(0), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));
                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, false);

                        if (ii > 4)
                            AddWaterSteps(layout, "water", new RandRange(30));//water



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
                                detours.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.KeyVault));
                                detours.RoomComponents.Set(new NoConnectRoom());
                                detours.RoomComponents.Set(new NoEventRoom());
                                detours.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.KeyVault));
                                detours.HallComponents.Set(new NoConnectRoom());
                                detours.RoomComponents.Set(new NoEventRoom());

                                layout.GenSteps.Add(PR_ROOMS_GEN_EXTRA, detours);
                            }
                            //sealing the vault
                            {
                                KeySealStep<MapGenContext> vaultStep = new KeySealStep<MapGenContext>("sealed_block", "sealed_door", "key");
                                vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                                layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                            }
                            // items for the vault
                            {
                                BulkSpawner<MapGenContext, InvItem> treasures = new BulkSpawner<MapGenContext, InvItem>();
                                treasures.RandomSpawns.Add(new InvItem("apricorn_purple"), 10);//purple apricorn
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
                                treasures.SpecificSpawns.Add(new MoneySpawn(100));
                                treasures.SpecificSpawns.Add(new MoneySpawn(200));
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
                                layout.GenSteps.Add(PR_SPAWN_ITEMS_EXTRA, detourItems);
                            }
                        }

                        //layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        floorSegment.Floors.Add(layout);
                    }
                    {
                        LoadGen layout = new LoadGen();
                        MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                        startGen.MapID = "zone_36_end";
                        layout.GenSteps.Add(PR_TILES_INIT, startGen);
                        //add a chest

                        //List<(InvItem, Loc)> items = new List<(InvItem, Loc)>();
                        //items.Add((new InvItem("apricorn_plain"), new Loc(13, 10)));//Plain Apricorn
                        //layout.GenSteps.Add(PR_SPAWN_ITEMS, new SpecificSpawnStep<MapLoadContext, InvItem>(items));

                        List<InvItem> treasure = new List<InvItem>();
                        treasure.Add(InvItem.CreateBox("box_light", "xcl_family_bulbasaur_02"));//Bulbasaur
                        treasure.Add(InvItem.CreateBox("box_light", "xcl_family_charmander_02"));//Charmander
                        treasure.Add(InvItem.CreateBox("box_light", "xcl_family_squirtle_02"));//Squirtle
                        treasure.Add(InvItem.CreateBox("box_light", "xcl_family_pikachu_02"));//Pikachu
                        List<(List<InvItem>, Loc)> items = new List<(List<InvItem>, Loc)>();
                        items.Add((treasure, new Loc(4, 4)));
                        AddSpecificSpawnPool(layout, items, PR_SPAWN_ITEMS);

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }

                {
                    int max_floors = 3;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Bramble Thicket\nB{0}F")));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 10);
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;

                    //necessities
                    CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                    necessities.SpawnRates.SetRange(15, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("necessities", necessities);

                    necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 9);//Leppa
                    necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 12);//Oran
                    necessities.Spawns.Add(new InvItem("food_apple"), new IntRange(0, max_floors), 10);//Apple
                    necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 35);//Lum
                    necessities.Spawns.Add(new InvItem("seed_reviver"), new IntRange(0, max_floors), 5);//reviver seed

                    //snacks
                    CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                    snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("snacks", snacks);

                    snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, max_floors), 20);//blast seed
                    snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, max_floors), 10);//warp seed
                    snacks.Spawns.Add(new InvItem("seed_sleep"), new IntRange(0, max_floors), 10);//sleep seed
                    snacks.Spawns.Add(new InvItem("seed_blinker"), new IntRange(0, max_floors), 10);//blinker seed

                    //wands
                    CategorySpawn<InvItem> ammo = new CategorySpawn<InvItem>();
                    ammo.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("ammo", ammo);

                    ammo.Spawns.Add(new InvItem("ammo_stick", false, 3), new IntRange(0, max_floors), 10);//stick
                    ammo.Spawns.Add(new InvItem("wand_whirlwind", false, 2), new IntRange(0, max_floors), 10);//whirlwind wand
                    ammo.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, max_floors), 10);//pounce wand
                    ammo.Spawns.Add(new InvItem("wand_warp", false, 1), new IntRange(0, max_floors), 10);//warp wand
                    ammo.Spawns.Add(new InvItem("wand_lob", false, 2), new IntRange(0, max_floors), 10);//lob wand
                    ammo.Spawns.Add(new InvItem("ammo_geo_pebble", false, 2), new IntRange(0, max_floors), 10);//Geo Pebble

                    //orbs
                    CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                    orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("orbs", orbs);

                    orbs.Spawns.Add(new InvItem("orb_rebound"), new IntRange(0, max_floors), 10);//Rebound
                    orbs.Spawns.Add(new InvItem("orb_all_protect"), new IntRange(0, max_floors), 5);//All Protect
                    orbs.Spawns.Add(new InvItem("orb_luminous"), new IntRange(0, max_floors), 9);//Luminous
                    orbs.Spawns.Add(new InvItem("orb_petrify"), new IntRange(0, max_floors), 10);//Petrify
                    orbs.Spawns.Add(new InvItem("orb_slumber"), new IntRange(0, max_floors), 8);//Slumber Orb
                    orbs.Spawns.Add(new InvItem("orb_mirror"), new IntRange(0, max_floors), 8);//Mirror Orb

                    //held items
                    CategorySpawn<InvItem> heldItems = new CategorySpawn<InvItem>();
                    heldItems.SpawnRates.SetRange(1, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("held", heldItems);

                    heldItems.Spawns.Add(new InvItem("held_black_belt"), new IntRange(0, max_floors), 1);//Silver Powder
                    heldItems.Spawns.Add(new InvItem("held_toxic_plate"), new IntRange(0, max_floors), 1);//Toxic Plate

                    //special
                    CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                    special.SpawnRates.SetRange(8, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("special", special);

                    int rate = 2;
                    special.Spawns.Add(new InvItem("apricorn_blue"), new IntRange(0, max_floors), rate);//blue apricorns
                    special.Spawns.Add(new InvItem("apricorn_green"), new IntRange(0, max_floors), rate);//green apricorns
                    special.Spawns.Add(new InvItem("apricorn_white"), new IntRange(0, max_floors), rate);//white apricorns
                    special.Spawns.Add(new InvItem("apricorn_purple"), new IntRange(0, max_floors), rate);//purple apricorns

                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;

                    {
                        //032 Nidoran♂ : 079 Rivalry : 043 Leer : 064 Peck
                        TeamMemberSpawn teamSpawn = GetTeamMob("nidoran_m", "poison_point", "leer", "peck", "", "", new RandRange(7));
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(0, 2));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 10);
                    }
                    {
                        //029 Nidoran♀ : 079 Rivalry : 045 Growl : 010 Scratch
                        TeamMemberSpawn teamSpawn = GetTeamMob("nidoran_f", "poison_point", "growl", "scratch", "", "", new RandRange(7));
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(1, 2));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 10);
                    }
                    // 406 Budew : 30 Natural Cure : 71 Absorb : 78 Stun Spore
                    poolSpawn.Spawns.Add(GetTeamMob("budew", "natural_cure", "absorb", "poison_powder", "", "", new RandRange(9), "wander_dumb"), new IntRange(0, max_floors), 10);
                    // 285 Shroomish : 90 Poison Heal : 73 Leech Seed : 33 Tackle
                    poolSpawn.Spawns.Add(GetTeamMob("shroomish", "poison_heal", "leech_seed", "tackle", "", "", new RandRange(10), "wander_dumb"), new IntRange(0, max_floors), 10);
                    // 165 Ledyba : 48 Supersonic : 4 Comet Punch
                    poolSpawn.SpecificSpawns.Add(new SpecificTeamSpawner(GetGenericMob("ledyba", "", "supersonic", "comet_punch", "", "", new RandRange(10), "wander_dumb"), GetGenericMob("ledyba", "", "supersonic", "comet_punch", "", "", new RandRange(10), "wander_dumb")), new IntRange(0, max_floors), 10);

                    // 14 Kakuna : 106 Harden
                    poolSpawn.Spawns.Add(GetTeamMob("kakuna", "", "harden", "", "", "", new RandRange(9), "wait_attack"), new IntRange(0, max_floors), 10);
                    // 11 Metapod : 106 Harden
                    poolSpawn.Spawns.Add(GetTeamMob("metapod", "", "harden", "", "", "", new RandRange(9), "wait_attack"), new IntRange(0, max_floors), 10);

                    // 15 Beedrill : 41 Twineedle
                    poolSpawn.Spawns.Add(GetTeamMob("beedrill", "", "twineedle", "", "", "", new RandRange(14), "wander_dumb"), new IntRange(0, max_floors), 10);
                    // 12 Butterfree : 14 Compound Eyes : 78 Stun Spore : 79 Sleep powder : 77 Poison powder : 93 Confusion
                    poolSpawn.Spawns.Add(GetTeamMob("butterfree", "compound_eyes", "stun_spore", "sleep_powder", "poison_powder", "confusion", new RandRange(14), "wander_dumb"), new IntRange(0, max_floors), 10);

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    floorSegment.ZoneSteps.Add(tileSpawn);


                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        AddFloorData(layout, "B19. Bramble Thicket.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                        AddGrassSteps(layout, new RandRange(3, 7), new IntRange(2, 6), new RandRange(50));

                        //Tilesets
                        AddSpecificTextureData(layout, "mystifying_forest_wall", "mystifying_forest_floor", "mystifying_forest_secondary", "tall_grass_dark", "bug");

                        //money - 315P to 1,260P
                        AddMoneyData(layout, new RandRange(1, 4));

                        //items
                        AddItemData(layout, new RandRange(2, 5), 25);

                        //enemies! ~ lv 5 to 10
                        AddRespawnData(layout, 4, 60);
                        AddEnemySpawnData(layout, 20, new RandRange(2, 4));

                        //traps
                        AddSingleTrapStep(layout, new RandRange(5, 7), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(5, 7));


                        //construct paths
                        {
                            AddInitGridStep(layout, 4, 4, 6, 6);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(80);
                            path.BranchRatio = new RandRange(0, 25);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //blocked
                            genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(3, 6), new RandRange(3, 6), new RandRange(1, 3), new RandRange(1, 3)), 5);
                            //round
                            genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(3, 6), new RandRange(3, 6)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(100), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(50, 100));

                            layout.GenSteps.Add(PR_GRID_GEN, new SetGridDefaultsStep<MapGenContext>(new RandRange(20), GetImmutableFilterList()));

                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, false);

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
                #endregion
            }
            else if (index == 37)
            {
                #region SICKLY HOLLOW
                {
                    zone.Name = new LocalText("Sickly Hollow");
                    zone.Rescues = 2;
                    zone.Level = 25;
                    zone.LevelCap = true;
                    zone.BagRestrict = 16;
                    zone.TeamSize = 3;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    int max_floors = 16;

                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Sickly Hollow\nB{0}F")));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = new MoneySpawnZoneStep(PR_RESPAWN_MONEY, new RandRange(30, 40), new RandRange(30, 34));
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;

                    //necesities
                    CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                    necessities.SpawnRates.SetRange(24, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("necessities", necessities);

                    necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 30);//Leppa
                    necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 40);//Oran
                    necessities.Spawns.Add(new InvItem("berry_sitrus"), new IntRange(0, max_floors), 30);//Sitrus
                    necessities.Spawns.Add(new InvItem("food_apple"), new IntRange(0, max_floors), 10);//Apple
                    necessities.Spawns.Add(new InvItem("food_grimy"), new IntRange(5, max_floors), 30);//Grimy Food
                    necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(2, max_floors), 50);//Lum berry

                    necessities.Spawns.Add(new InvItem("seed_reviver"), new IntRange(0, max_floors), 20);//reviver seed
                    necessities.Spawns.Add(new InvItem("seed_reviver", true), new IntRange(0, max_floors), 10);//reviver seed
                    necessities.Spawns.Add(new InvItem("machine_recall_box"), new IntRange(4, max_floors), 30);//Link Box


                    //snacks
                    CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                    snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("snacks", snacks);

                    snacks.Spawns.Add(new InvItem("berry_ganlon"), new IntRange(0, max_floors), 4);//ganlon berry
                    snacks.Spawns.Add(new InvItem("berry_petaya"), new IntRange(0, max_floors), 4);//apicot berry

                    snacks.Spawns.Add(new InvItem("berry_wacan"), new IntRange(6, max_floors), 8);//Wacan berry
                    snacks.Spawns.Add(new InvItem("berry_rindo"), new IntRange(6, max_floors), 8);//rindo berry
                    snacks.Spawns.Add(new InvItem("berry_kebia"), new IntRange(6, max_floors), 8);//kebia berry
                    snacks.Spawns.Add(new InvItem("berry_babiri"), new IntRange(6, max_floors), 8);//Babiri berry

                    snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, max_floors), 20);//blast seed
                    snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, max_floors), 10);//warp seed
                    snacks.Spawns.Add(new InvItem("seed_decoy"), new IntRange(0, max_floors), 10);//decoy seed
                    snacks.Spawns.Add(new InvItem("seed_sleep"), new IntRange(0, max_floors), 10);//sleep seed
                    snacks.Spawns.Add(new InvItem("seed_blinker"), new IntRange(0, max_floors), 10);//blinker seed
                    snacks.Spawns.Add(new InvItem("seed_last_chance"), new IntRange(0, max_floors), 5);//last-chance seed
                    snacks.Spawns.Add(new InvItem("seed_doom"), new IntRange(0, max_floors), 5);//doom seed
                    snacks.Spawns.Add(new InvItem("seed_ban"), new IntRange(0, max_floors), 10);//ban seed
                    snacks.Spawns.Add(new InvItem("seed_ice"), new IntRange(0, max_floors), 10);//ice seed
                    snacks.Spawns.Add(new InvItem("seed_vile"), new IntRange(0, max_floors), 10);//vile seed

                    snacks.Spawns.Add(new InvItem("herb_mental"), new IntRange(0, max_floors), 5);//mental herb
                    snacks.Spawns.Add(new InvItem("herb_white"), new IntRange(0, max_floors), 50);//white herb


                    //boosters
                    CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                    boosters.SpawnRates.SetRange(5, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("boosters", boosters);

                    boosters.Spawns.Add(new InvItem("gummi_grass"), new IntRange(0, max_floors), 2);//grass gummi
                    boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 2);//pink gummi
                    boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 2);//purple gummi
                    boosters.Spawns.Add(new InvItem("gummi_red"), new IntRange(0, max_floors), 2);//red gummi
                    boosters.Spawns.Add(new InvItem("gummi_sky"), new IntRange(0, max_floors), 2);//sky gummi

                    IntRange range = new IntRange(10, max_floors);

                    boosters.Spawns.Add(new InvItem("boost_protein"), range, 1);//protein
                    boosters.Spawns.Add(new InvItem("boost_iron"), range, 1);//iron
                    boosters.Spawns.Add(new InvItem("boost_calcium"), range, 1);//calcium
                    boosters.Spawns.Add(new InvItem("boost_zinc"), range, 1);//zinc
                    boosters.Spawns.Add(new InvItem("boost_carbos"), range, 1);//carbos
                    boosters.Spawns.Add(new InvItem("boost_hp_up"), range, 1);//hp up

                    //throwable
                    CategorySpawn<InvItem> ammo = new CategorySpawn<InvItem>();
                    ammo.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("ammo", ammo);

                    range = new IntRange(0, max_floors);
                    {
                        ammo.Spawns.Add(new InvItem("ammo_stick", false, 4), range, 10);//stick
                        ammo.Spawns.Add(new InvItem("ammo_cacnea_spike", false, 3), range, 10);//cacnea spike
                        ammo.Spawns.Add(new InvItem("wand_path", false, 2), range, 50);//path wand
                        ammo.Spawns.Add(new InvItem("wand_fear", false, 4), range, 10);//fear wand
                        ammo.Spawns.Add(new InvItem("wand_switcher", false, 4), range, 10);//switcher wand
                        ammo.Spawns.Add(new InvItem("wand_whirlwind", false, 4), range, 10);//whirlwind wand
                        ammo.Spawns.Add(new InvItem("wand_lure", false, 4), range, 10);//lure wand
                        ammo.Spawns.Add(new InvItem("wand_slow", false, 4), range, 10);//slow wand
                        ammo.Spawns.Add(new InvItem("wand_pounce", false, 4), range, 10);//pounce wand
                        ammo.Spawns.Add(new InvItem("wand_warp", false, 2), range, 10);//warp wand
                        ammo.Spawns.Add(new InvItem("wand_topsy_turvy", false, 4), range, 10);//topsy-turvy wand
                        ammo.Spawns.Add(new InvItem("wand_lob", false, 4), range, 10);//lob wand
                        ammo.Spawns.Add(new InvItem("wand_purge", false, 4), range, 10);//purge wand
                        ammo.Spawns.Add(new InvItem("wand_vanish", false, 3), range, 10);//vanish wand

                        ammo.Spawns.Add(new InvItem("ammo_gravelerock", false, 3), range, 10);//Gravelerock

                        ammo.Spawns.Add(new InvItem("ammo_geo_pebble", false, 3), range, 10);//Geo Pebble
                    }


                    //special items
                    CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                    special.SpawnRates.SetRange(7, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("special", special);

                    {
                        range = new IntRange(0, max_floors);
                        int rate = 2;

                        special.Spawns.Add(new InvItem("apricorn_blue"), range, rate);//blue apricorns
                        special.Spawns.Add(new InvItem("apricorn_green"), range, rate);//green apricorns
                        special.Spawns.Add(new InvItem("apricorn_brown"), range, rate);//brown apricorns
                        special.Spawns.Add(new InvItem("apricorn_red"), range, rate);//red apricorns
                        special.Spawns.Add(new InvItem("apricorn_white"), range, rate);//white apricorns
                        special.Spawns.Add(new InvItem("apricorn_yellow"), range, rate);//yellow apricorns
                        special.Spawns.Add(new InvItem("apricorn_black"), range, rate);//black apricorns

                    }

                    special.Spawns.Add(new InvItem("key", false, 1), new IntRange(0, max_floors), 25);//Key


                    //orbs
                    CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                    orbs.SpawnRates.SetRange(10, new IntRange(0, 30));
                    itemSpawnZoneStep.Spawns.Add("orbs", orbs);

                    {
                        range = new IntRange(10, max_floors);
                        orbs.Spawns.Add(new InvItem("orb_one_room"), range, 7);//One-Room Orb
                        orbs.Spawns.Add(new InvItem("orb_fill_in"), range, 7);//Fill-In Orb
                        orbs.Spawns.Add(new InvItem("orb_one_room", true), range, 3);//One-Room Orb
                        orbs.Spawns.Add(new InvItem("orb_fill_in", true), range, 3);//Fill-In Orb
                    }

                    {
                        range = new IntRange(0, max_floors);
                        orbs.Spawns.Add(new InvItem("orb_petrify"), range, 10);//Petrify
                        orbs.Spawns.Add(new InvItem("orb_halving"), range, 10);//Halving
                        orbs.Spawns.Add(new InvItem("orb_slumber"), range, 8);//Slumber Orb
                        orbs.Spawns.Add(new InvItem("orb_slow"), range, 8);//Slow
                        orbs.Spawns.Add(new InvItem("orb_totter"), range, 8);//Totter
                        orbs.Spawns.Add(new InvItem("orb_stayaway"), range, 8);//Stayaway
                        orbs.Spawns.Add(new InvItem("orb_pierce"), range, 8);//Pierce
                        orbs.Spawns.Add(new InvItem("orb_invisify"), range, 8);//Invisify
                        orbs.Spawns.Add(new InvItem("orb_slumber", true), range, 3);//Slumber Orb
                        orbs.Spawns.Add(new InvItem("orb_slow", true), range, 3);//Slow
                        orbs.Spawns.Add(new InvItem("orb_totter", true), range, 3);//Totter
                        orbs.Spawns.Add(new InvItem("orb_stayaway", true), range, 3);//Stayaway
                        orbs.Spawns.Add(new InvItem("orb_pierce", true), range, 3);//Pierce
                        orbs.Spawns.Add(new InvItem("orb_invisify", true), range, 3);//Invisify
                    }

                    orbs.Spawns.Add(new InvItem("orb_cleanse"), new IntRange(2, max_floors), 7);//Cleanse

                    {
                        range = new IntRange(5, max_floors);
                        orbs.Spawns.Add(new InvItem("orb_all_aim"), range, 10);//All-Aim Orb
                        orbs.Spawns.Add(new InvItem("orb_trap_see"), range, 10);//Trap-See
                        orbs.Spawns.Add(new InvItem("orb_rollcall"), range, 10);//Roll Call
                        orbs.Spawns.Add(new InvItem("orb_mug"), range, 10);//Mug
                        orbs.Spawns.Add(new InvItem("orb_mirror"), range, 10);//Mirror
                    }

                    {
                        range = new IntRange(5, max_floors);
                        orbs.Spawns.Add(new InvItem("orb_weather"), range, 10);//Weather Orb
                        orbs.Spawns.Add(new InvItem("orb_foe_seal"), range, 10);//Foe-Seal
                        orbs.Spawns.Add(new InvItem("orb_freeze"), range, 10);//Freeze
                        orbs.Spawns.Add(new InvItem("orb_devolve"), range, 10);//Devolve
                        orbs.Spawns.Add(new InvItem("orb_nullify"), range, 10);//Nullify
                    }

                    {
                        range = new IntRange(0, 10);
                        orbs.Spawns.Add(new InvItem("orb_rebound"), range, 10);//Rebound
                        orbs.Spawns.Add(new InvItem("orb_all_protect"), range, 5);//All Protect
                        orbs.Spawns.Add(new InvItem("orb_all_protect", true), range, 5);//All Protect
                    }

                    //held items
                    CategorySpawn<InvItem> heldItems = new CategorySpawn<InvItem>();
                    heldItems.SpawnRates.SetRange(4, new IntRange(0, 30));
                    itemSpawnZoneStep.Spawns.Add("held", heldItems);

                    heldItems.Spawns.Add(new InvItem("held_poison_barb"), new IntRange(0, 10), 2);//Poison Barb
                    heldItems.Spawns.Add(new InvItem("held_twisted_spoon"), new IntRange(10, max_floors), 2);//Twisted Spoon
                    heldItems.Spawns.Add(new InvItem("held_toxic_plate"), new IntRange(0, max_floors), 1);//Toxic Plate

                    heldItems.Spawns.Add(new InvItem("held_cover_band"), new IntRange(0, max_floors), 2);//Cover Band
                    heldItems.Spawns.Add(new InvItem("held_reunion_cape"), new IntRange(0, max_floors), 1);//Reunion Cape
                    heldItems.Spawns.Add(new InvItem("held_reunion_cape", true), new IntRange(0, max_floors), 1);//Reunion Cape

                    heldItems.Spawns.Add(new InvItem("held_trap_scarf"), new IntRange(0, max_floors), 2);//Trap Scarf
                    heldItems.Spawns.Add(new InvItem("held_trap_scarf", true), new IntRange(0, max_floors), 1);//Trap Scarf

                    heldItems.Spawns.Add(new InvItem("held_grip_claw"), new IntRange(0, max_floors), 2);//Grip Claw

                    range = new IntRange(0, 20);
                    heldItems.Spawns.Add(new InvItem("held_twist_band"), range, 2);//Twist Band
                    heldItems.Spawns.Add(new InvItem("held_metronome"), range, 1);//Metronome
                    heldItems.Spawns.Add(new InvItem("held_twist_band", true), range, 1);//Twist Band
                    heldItems.Spawns.Add(new InvItem("held_metronome", true), range, 1);//Metronome
                    heldItems.Spawns.Add(new InvItem("held_scope_lens"), range, 1);//Scope Lens
                    heldItems.Spawns.Add(new InvItem("held_power_band"), range, 2);//Power Band
                    heldItems.Spawns.Add(new InvItem("held_special_band"), range, 2);//Special Band
                    heldItems.Spawns.Add(new InvItem("held_defense_scarf"), range, 2);//Defense Scarf
                    heldItems.Spawns.Add(new InvItem("held_zinc_band"), range, 2);//Zinc Band

                    heldItems.Spawns.Add(new InvItem("held_shed_shell"), new IntRange(0, max_floors), 2);//Shed Shell
                    heldItems.Spawns.Add(new InvItem("held_shed_shell", true), new IntRange(0, max_floors), 1);//Shed Shell

                    heldItems.Spawns.Add(new InvItem("held_big_root"), new IntRange(0, max_floors), 2);//Big Root
                    heldItems.Spawns.Add(new InvItem("held_big_root", true), new IntRange(0, max_floors), 1);//Big Root

                    int stickRate = 2;
                    range = new IntRange(0, 15);

                    heldItems.Spawns.Add(new InvItem("held_life_orb"), range, stickRate);//Life Orb
                    heldItems.Spawns.Add(new InvItem("held_heal_ribbon"), range, stickRate);//Heal Ribbon

                    stickRate = 1;
                    range = new IntRange(15, 30);

                    heldItems.Spawns.Add(new InvItem("held_life_orb"), range, stickRate);//Life Orb
                    heldItems.Spawns.Add(new InvItem("held_heal_ribbon"), range, stickRate);//Heal Ribbon


                    heldItems.Spawns.Add(new InvItem("held_warp_scarf"), new IntRange(0, max_floors), 1);//Warp Scarf
                    heldItems.Spawns.Add(new InvItem("held_warp_scarf", true), new IntRange(0, max_floors), 1);//Warp Scarf

                    //machines
                    CategorySpawn<InvItem> machines = new CategorySpawn<InvItem>();
                    machines.SpawnRates.SetRange(7, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("tms", machines);

                    range = new IntRange(0, max_floors);
                    machines.Spawns.Add(new InvItem("tm_double_team"), range, 2);//TM Double Team
                    machines.Spawns.Add(new InvItem("tm_will_o_wisp"), range, 2);//TM Will-o-Wisp
                    machines.Spawns.Add(new InvItem("tm_protect"), range, 2);//TM Protect
                    machines.Spawns.Add(new InvItem("tm_defog"), range, 2);//TM Defog
                    machines.Spawns.Add(new InvItem("tm_swagger"), range, 2);//TM Swagger
                    machines.Spawns.Add(new InvItem("tm_facade"), range, 2);//TM Facade
                    machines.Spawns.Add(new InvItem("tm_safeguard"), range, 2);//TM Safeguard
                    machines.Spawns.Add(new InvItem("tm_venoshock"), range, 2);//TM Venoshock
                    machines.Spawns.Add(new InvItem("tm_scald"), range, 2);//TM Scald
                    machines.Spawns.Add(new InvItem("tm_thunder_wave"), range, 2);//TM Thunder Wave
                    machines.Spawns.Add(new InvItem("tm_infestation"), range, 2);//TM Infestation
                    machines.Spawns.Add(new InvItem("tm_dream_eater"), range, 2);//TM Dream Eater
                    machines.Spawns.Add(new InvItem("tm_quash"), range, 2);//TM Quash
                    machines.Spawns.Add(new InvItem("tm_taunt"), range, 2);//TM Taunt
                    machines.Spawns.Add(new InvItem("tm_torment"), range, 2);//TM Torment

                    range = new IntRange(10, max_floors);

                    machines.Spawns.Add(new InvItem("tm_sludge_bomb"), range, 1);//TM Sludge Bomb
                    machines.Spawns.Add(new InvItem("tm_sludge_bomb", true), range, 1);//TM Sludge Bomb
                    machines.Spawns.Add(new InvItem("tm_sludge_wave"), range, 1);//TM Sludge Wave
                    machines.Spawns.Add(new InvItem("tm_sludge_wave", true), range, 1);//TM Sludge Wave
                    machines.Spawns.Add(new InvItem("tm_fire_blast"), range, 1);//TM Fire Blast
                    machines.Spawns.Add(new InvItem("tm_fire_blast", true), range, 1);//TM Fire Blast

                    //evo items
                    CategorySpawn<InvItem> evoItems = new CategorySpawn<InvItem>();
                    evoItems.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("evo", evoItems);

                    range = new IntRange(5, max_floors);
                    evoItems.Spawns.Add(new InvItem("evo_fire_stone"), range, 10);//Fire Stone
                    evoItems.Spawns.Add(new InvItem("evo_dusk_stone"), range, 10);//Dusk Stone
                    evoItems.Spawns.Add(new InvItem("evo_shiny_stone"), range, 10);//Shiny Stone
                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);

                    SpawnList<IGenPriority> assemblyZoneSpawns = new SpawnList<IGenPriority>();
                    assemblyZoneSpawns.Add(new GenPriority<GenStep<MapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<MapGenContext, MapItem>(new PickerSpawner<MapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem("machine_assembly_box"))))), 10);//assembly box
                    SpreadStepZoneStep assemblyZoneStep = new SpreadStepZoneStep(new SpreadPlanSpaced(new RandRange(2, 6), new IntRange(8, max_floors)), assemblyZoneSpawns);
                    floorSegment.ZoneSteps.Add(assemblyZoneStep);

                    SpawnList<IGenPriority> appleZoneSpawns = new SpawnList<IGenPriority>();
                    appleZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem("food_apple"))))), 10);
                    SpreadStepZoneStep appleZoneStep = new SpreadStepZoneStep(new SpreadPlanSpaced(new RandRange(3, 5), new IntRange(0, max_floors)), appleZoneSpawns);//apple
                    floorSegment.ZoneSteps.Add(appleZoneStep);

                    SpawnList<IGenPriority> keyZoneSpawns = new SpawnList<IGenPriority>();
                    keyZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem("key", 1))))), 10);
                    SpreadStepZoneStep keyZoneStep = new SpreadStepZoneStep(new SpreadPlanQuota(new RandRange(1), new IntRange(0, 5)), keyZoneSpawns);//key
                    floorSegment.ZoneSteps.Add(keyZoneStep);


                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;

                    // 188 Skiploom : Infiltrator : 78 Stun Spore : 235 Synthesis : 584 Fairy Wind
                    poolSpawn.Spawns.Add(GetTeamMob("skiploom", "infiltrator", "stun_spore", "synthesis", "fairy_wind", "", new RandRange(25), TeamMemberSpawn.MemberRole.Support), new IntRange(0, 5), 10);
                    // 189 Jumpluff : Infiltrator : 79 Sleep Powder : 73 Leech Seed
                    poolSpawn.Spawns.Add(GetTeamMob("jumpluff", "infiltrator", "sleep_powder", "leech_seed", "", "", new RandRange(30), TeamMemberSpawn.MemberRole.Support), new IntRange(5, 10), 10);
                    // 315 Roselia : 38 Poison Point : 92 Toxic : 73 Leech Seed
                    poolSpawn.Spawns.Add(GetTeamMob("roselia", "poison_point", "toxic", "leech_seed", "", "", new RandRange(25)), new IntRange(0, 5), 10);
                    // 315 Roselia : 38 Poison Point : 92 Toxic : 73 Leech Seed
                    poolSpawn.Spawns.Add(GetTeamMob("roselia", "poison_point", "toxic", "leech_seed", "", "", new RandRange(35), TeamMemberSpawn.MemberRole.Support), new IntRange(10, max_floors), 10);
                    // 200 Misdreavus : 109 Confuse Ray : 212 Mean Look : 506 Hex 
                    poolSpawn.Spawns.Add(GetTeamMob("misdreavus", "", "confuse_ray", "mean_look", "hex", "", new RandRange(25)), new IntRange(0, 5), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("misdreavus", "", "confuse_ray", "mean_look", "hex", "", new RandRange(30)), new IntRange(5, 10), 10);
                    // 429 Mismagius : 174 Curse : 220 Pain Split : 595 Mystical Fire
                    poolSpawn.Spawns.Add(GetTeamMob("mismagius", "", "curse", "pain_split", "mystical_fire", "", new RandRange(35)), new IntRange(10, max_floors), 10);
                    // 53 Persian : 127 Unnerve : 415 Switcheroo : 269 Taunt
                    {
                        TeamMemberSpawn mob = GetTeamMob("persian", "unnerve", "switcheroo", "taunt", "", "", new RandRange(30), "thief");
                        mob.Spawn.SpawnFeatures.Add(new MobSpawnItem(true, "held_flame_orb", "held_ring_target"));
                        poolSpawn.Spawns.Add(mob, new IntRange(5, 10), 10);
                    }
                    // 453 Croagunk : 269 Taunt : 207 Swagger : 279 Revenge
                    poolSpawn.Spawns.Add(GetTeamMob("croagunk", "", "taunt", "swagger", "revenge", "", new RandRange(25)), new IntRange(0, 5), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("croagunk", "", "taunt", "swagger", "revenge", "", new RandRange(30)), new IntRange(5, 10), 10);
                    // 454 Toxicroak : 269 Taunt : 426 Mud Bomb : 279 Revenge
                    poolSpawn.Spawns.Add(GetTeamMob("toxicroak", "", "taunt", "mud_bomb", "revenge", "", new RandRange(35)), new IntRange(10, max_floors), 10);
                    // 355 Duskull : 50 Disable : 101 Night Shade
                    poolSpawn.Spawns.Add(GetTeamMob("duskull", "", "disable", "night_shade", "", "", new RandRange(25)), new IntRange(0, 5), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("duskull", "", "disable", "night_shade", "", "", new RandRange(30)), new IntRange(5, 10), 10);
                    // 336 Seviper : 151 Infiltrator : 305 Poison Fang : 474 Venoshock
                    poolSpawn.Spawns.Add(GetTeamMob("seviper", "infiltrator", "poison_fang", "venoshock", "", "", new RandRange(30)), new IntRange(5, 10), 10);
                    // 336 Seviper : 151 Infiltrator : 305 Poison Fang : 380 Gastro Acid : 599 Venom Drench
                    poolSpawn.Spawns.Add(GetTeamMob("seviper", "infiltrator", "poison_fang", "gastro_acid", "venom_drench", "", new RandRange(35)), new IntRange(10, max_floors), 10);
                    // 41 Zubat : 44 Bite : 141 Leech Life : 48 Supersonic
                    poolSpawn.Spawns.Add(GetTeamMob("zubat", "", "bite", "leech_life", "supersonic", "", new RandRange(30)), new IntRange(5, 10), 10);
                    // 42 Golbat : 151 Infiltrator : 305 Poison Fang : 109 Confuse Ray : 212 Mean Look
                    poolSpawn.Spawns.Add(GetTeamMob("golbat", "infiltrator", "poison_fang", "confuse_ray", "mean_look", "", new RandRange(35)), new IntRange(10, max_floors), 10);
                    // 37 Vulpix : 506 Hex : 261 Will-O-Wisp
                    poolSpawn.Spawns.Add(GetTeamMob("vulpix", "", "hex", "will_o_wisp", "", "", new RandRange(25)), new IntRange(0, 5), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("vulpix", "", "hex", "will_o_wisp", "", "", new RandRange(30)), new IntRange(5, 10), 10);
                    // 15 Beedrill : 390 Toxic Spikes : 41 Twineedle
                    poolSpawn.Spawns.Add(GetTeamMob("beedrill", "", "toxic_spikes", "twineedle", "", "", new RandRange(35)), new IntRange(10, max_floors), 10);
                    // 12 Butterfree : 14 Compound Eyes : 78 Stun Spore : 79 Sleep powder : 77 Poison powder : 093 Confusion
                    poolSpawn.Spawns.Add(GetTeamMob("butterfree", "compound_eyes", "stun_spore", "sleep_powder", "poison_powder", "confusion", new RandRange(25)), new IntRange(0, 5), 10);
                    // 198 Murkrow : 228 Pursuit : 372 Assurance
                    poolSpawn.Spawns.Add(GetTeamMob("murkrow", "", "pursuit", "assurance", "", "", new RandRange(25)), new IntRange(0, 5), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("murkrow", "", "pursuit", "assurance", "", "", new RandRange(30)), new IntRange(5, 10), 10);
                    // 407 Roserade : 38 Poison Point : 599 Venom Drench : 73 Leech Seed : 202 Giga Drain
                    poolSpawn.Spawns.Add(GetTeamMob("roserade", "poison_point", "venom_drench", "leech_seed", "giga_drain", "", new RandRange(35), TeamMemberSpawn.MemberRole.Leader), new IntRange(10, max_floors), 10);
                    //457 Lumineon : 114 Storm Drain : 487 Soak : 352 Water Pulse : 445 Captivate
                    poolSpawn.Spawns.Add(GetTeamMob("lumineon", "storm_drain", "soak", "water_pulse", "captivate", "", new RandRange(30)), new IntRange(5, 10), 10);

                    //206 Dunsparce : 50 Run Away : 99 Rage : 228 Pursuit : 36 Take Down
                    poolSpawn.Spawns.Add(GetTeamMob("dunsparce", "run_away", "rage", "pursuit", "take_down", "", new RandRange(30)), new IntRange(5, 10), 10);
                    //206 Dunsparce : 32 Serene Grace : 355 Roost : 228 Pursuit : 246 Ancient Power
                    poolSpawn.Spawns.Add(GetTeamMob("dunsparce", "serene_grace", "roost", "pursuit", "ancient_power", "", new RandRange(35)), new IntRange(10, max_floors), 10);

                    //344 Claydol : 322 Cosmic Power : 377 Heal Block : Mud-Slap
                    poolSpawn.Spawns.Add(GetTeamMob("claydol", "", "cosmic_power", "heal_block", "mud_slap", "", new RandRange(35), TeamMemberSpawn.MemberRole.Support), new IntRange(10, max_floors), 10);

                    //163 Hoothoot : Growl : Reflect : Confusion
                    poolSpawn.Spawns.Add(GetTeamMob("hoothoot", "", "growl", "reflect", "confusion", "", new RandRange(25), TeamMemberSpawn.MemberRole.Support), new IntRange(0, 5), 10);
                    //164 Noctowl : Growl : Reflect : Dream Eater
                    poolSpawn.Spawns.Add(GetTeamMob("noctowl", "", "growl", "reflect", "dream_eater", "", new RandRange(30), TeamMemberSpawn.MemberRole.Support), new IntRange(5, 10), 10);

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 6);
                    poolSpawn.TeamSizes.Add(3, new IntRange(10, max_floors), 4);
                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    tileSpawn.Spawns.Add(new EffectTile("trap_mud", false), new IntRange(0, max_floors), 10);//mud trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_poison", true), new IntRange(0, max_floors), 10);//poison trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_slumber", true), new IntRange(0, max_floors), 10);//sleep trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_sticky", false), new IntRange(0, max_floors), 10);//sticky trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_seal", true), new IntRange(0, max_floors), 10);//seal trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_summon", true), new IntRange(0, max_floors), 10);//summon trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_slow", true), new IntRange(0, max_floors), 10);//slow trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_spin", true), new IntRange(0, max_floors), 10);//spin trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_grimy", true), new IntRange(0, max_floors), 10);//grimy trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_trigger", true), new IntRange(0, max_floors), 20);//trigger trap
                    floorSegment.ZoneSteps.Add(tileSpawn);


                    SpreadRoomZoneStep evoZoneStep = new SpreadRoomZoneStep(PR_GRID_GEN_EXTRA, PR_ROOMS_GEN_EXTRA, new SpreadPlanSpaced(new RandRange(2, 5), new IntRange(1, max_floors)));
                    List<BaseRoomFilter> evoFilters = new List<BaseRoomFilter>();
                    evoFilters.Add(new RoomFilterComponent(true, new ImmutableRoom()));
                    evoFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                    evoZoneStep.Spawns.Add(new RoomGenOption(new RoomGenEvo<MapGenContext>(), new RoomGenEvo<ListMapGenContext>(), evoFilters), 10);
                    floorSegment.ZoneSteps.Add(evoZoneStep);


                    {
                        //monster houses
                        SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(20, new IntRange(1, 15)));
                        monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHouseStep<ListMapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                        foreach (string gummi in IterateGummis())
                            monsterChanceZoneStep.Items.Add(new MapItem(gummi), new IntRange(0, max_floors), 4);//gummis
                        foreach (string iter_item in IterateApricorns())
                            monsterChanceZoneStep.Items.Add(new MapItem(iter_item), new IntRange(0, max_floors), 4);//apricorns
                        monsterChanceZoneStep.Items.Add(new MapItem("food_banana"), new IntRange(0, max_floors), 50);//banana
                        monsterChanceZoneStep.Items.Add(new MapItem("loot_nugget"), new IntRange(0, max_floors), 10);//nugget
                        monsterChanceZoneStep.Items.Add(new MapItem("loot_pearl", 1), new IntRange(0, max_floors), 10);//pearl
                        monsterChanceZoneStep.Items.Add(new MapItem("loot_heart_scale", 2), new IntRange(0, max_floors), 10);//heart scale
                        monsterChanceZoneStep.Items.Add(new MapItem("key", 1), new IntRange(0, max_floors), 10);//key
                        monsterChanceZoneStep.Items.Add(new MapItem("machine_recall_box"), new IntRange(0, max_floors), 10);//link box
                        monsterChanceZoneStep.Items.Add(new MapItem("machine_assembly_box"), new IntRange(8, max_floors), 10);//assembly box
                        monsterChanceZoneStep.Items.Add(new MapItem("machine_ability_capsule"), new IntRange(0, max_floors), 10);//ability capsule

                        monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(1, 4), "loot_pearl"), new ItemThemeNone(0, new RandRange(2, 4))), new IntRange(0, max_floors), 20);//no theme

                        monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(3, 7)), new IntRange(0, max_floors), 30);//gummis
                        monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(RecruitState)), true, true, new RandRange(2, 6)), new IntRange(0, 10), 10);//apricorns
                        monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(1, 4), "loot_pearl"), new ItemStateType(new FlagType(typeof(EvoState)), true, true, new RandRange(2, 4))), new IntRange(0, 10), 10);//evo items
                        //mobs
                        monsterChanceZoneStep.MobThemes.Add(new MobThemeNone(0, new RandRange(7, 13)), new IntRange(0, max_floors), 10);
                        floorSegment.ZoneSteps.Add(monsterChanceZoneStep);
                    }


                    {
                        SpreadVaultZoneStep vaultChanceZoneStep = new SpreadVaultZoneStep(PR_SPAWN_ITEMS_EXTRA, PR_SPAWN_MOBS_EXTRA, new SpreadPlanQuota(new RandBinomial(4, 50, 1), new IntRange(1, max_floors)));

                        // room addition step
                        {
                            SpawnList<RoomGen<ListMapGenContext>> detourRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                            detourRooms.Add(new RoomGenCross<ListMapGenContext>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
                            SpawnList<PermissiveRoomGen<ListMapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                            detourHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                            AddConnectedRoomsStep<ListMapGenContext> detours = new AddConnectedRoomsStep<ListMapGenContext>(detourRooms, detourHalls);
                            detours.Amount = new RandRange(1);
                            detours.HallPercent = 100;
                            detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom()));
                            detours.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.KeyVault));
                            detours.RoomComponents.Set(new NoConnectRoom());
                            detours.RoomComponents.Set(new NoEventRoom());
                            detours.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.KeyVault));
                            detours.HallComponents.Set(new NoConnectRoom());
                            detours.RoomComponents.Set(new NoEventRoom());

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
                            vaultChanceZoneStep.Items.Add(new MapItem("medicine_elixir"), new IntRange(0, 30), 200);//elixir
                            vaultChanceZoneStep.Items.Add(new MapItem("medicine_max_elixir"), new IntRange(0, 30), 100);//max elixir
                            vaultChanceZoneStep.Items.Add(new MapItem("medicine_potion"), new IntRange(0, 30), 200);//potion
                            vaultChanceZoneStep.Items.Add(new MapItem("medicine_max_potion"), new IntRange(0, 30), 100);//max potion
                            vaultChanceZoneStep.Items.Add(new MapItem("medicine_full_heal"), new IntRange(0, 30), 300);//full heal
                            foreach (string key in IterateXItems())
                                vaultChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, 30), 50);//X-Items
                            foreach (string key in IterateTMs(TMClass.Natural))
                                vaultChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, 30), 5);//TMs
                            vaultChanceZoneStep.Items.Add(new MapItem("loot_nugget"), new IntRange(0, 30), 200);//nugget
                            vaultChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear"), new IntRange(0, 30), 100);//amber tear
                            vaultChanceZoneStep.Items.Add(new MapItem("seed_reviver"), new IntRange(0, 30), 200);//reviver seed
                            vaultChanceZoneStep.Items.Add(new MapItem("seed_joy"), new IntRange(0, 30), 100);//joy seed
                            vaultChanceZoneStep.Items.Add(new MapItem("machine_ability_capsule"), new IntRange(0, 30), 200);//ability capsule
                        }

                        // item spawnings for the vault
                        for (int ii = 0; ii < 30; ii++)
                        {
                            //add a PickerSpawner <- PresetMultiRand <- coins
                            List<MapItem> treasures = new List<MapItem>();
                            treasures.Add(MapItem.CreateMoney(150));
                            treasures.Add(MapItem.CreateMoney(150));
                            treasures.Add(MapItem.CreateMoney(150));
                            treasures.Add(MapItem.CreateMoney(150));
                            treasures.Add(MapItem.CreateMoney(150));
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
                                boxTreasure.Add(new MapItem("tm_will_o_wisp"), 8);//TM Will-o-Wisp
                                boxTreasure.Add(new MapItem("tm_protect"), 8);//TM Protect
                                boxTreasure.Add(new MapItem("tm_facade"), 8);//TM Facade
                                boxTreasure.Add(new MapItem("tm_safeguard"), 8);//TM Safeguard

                                boxTreasure.Add(new MapItem("machine_ability_capsule"), 100);//ability capsule
                                boxTreasure.Add(new MapItem("loot_heart_scale"), 100);//heart scale
                                boxTreasure.Add(new MapItem("medicine_potion"), 60);//potion
                                boxTreasure.Add(new MapItem("medicine_max_potion"), 30);//max potion
                                boxTreasure.Add(new MapItem("medicine_full_heal"), 100);//full heal
                                boxTreasure.Add(new MapItem("medicine_elixir"), 60);//elixir
                                boxTreasure.Add(new MapItem("medicine_max_elixir"), 30);//max elixir
                                boxTreasure.Add(new MapItem("evo_reaper_cloth"), 80);//Reaper Cloth
                                boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_nifty", new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 10);
                            }

                            //447      ***    Dainty Box - Stat ups, wonder gummi, nectar, golden apple, golden banana
                            {
                                SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                                //Stat-up
                                boxTreasure.Add(new MapItem("gummi_grass"), 2);//grass gummi
                                boxTreasure.Add(new MapItem("gummi_pink"), 2);//pink gummi
                                boxTreasure.Add(new MapItem("gummi_purple"), 2);//purple gummi
                                boxTreasure.Add(new MapItem("gummi_red"), 2);//red gummi
                                boxTreasure.Add(new MapItem("gummi_sky"), 2);//sky gummi

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

                            MultiStepSpawner<ListMapGenContext, MapItem> boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(1)));

                            //StepSpawner <- PresetMultiRand
                            MultiStepSpawner<ListMapGenContext, MapItem> mainSpawner = new MultiStepSpawner<ListMapGenContext, MapItem>();
                            mainSpawner.Picker = new PresetMultiRand<IStepSpawner<ListMapGenContext, MapItem>>(treasurePicker, boxPicker);
                            vaultChanceZoneStep.ItemSpawners.SetRange(mainSpawner, new IntRange(0, max_floors));
                        }
                        vaultChanceZoneStep.ItemAmount.SetRange(new RandRange(1, 3), new IntRange(0, max_floors));


                        // item placements for the vault
                        {
                            RandomRoomSpawnStep<ListMapGenContext, MapItem> detourItems = new RandomRoomSpawnStep<ListMapGenContext, MapItem>();
                            detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                            vaultChanceZoneStep.ItemPlacements.SetRange(detourItems, new IntRange(0, max_floors));
                        }


                        floorSegment.ZoneSteps.Add(vaultChanceZoneStep);
                    }


                    SpawnRangeList<IGenPriority> shopZoneSpawns = new SpawnRangeList<IGenPriority>();
                    {
                        ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                        shop.Personality = 0;
                        shop.SecurityStatus = "shop_security";
                        shop.Items.Add(new MapItem("berry_oran", 0, 100), 20);//oran
                        shop.Items.Add(new MapItem("berry_leppa", 0, 750), 20);//leppa
                        shop.Items.Add(new MapItem("berry_lum", 0, 500), 20);//lum
                        shop.Items.Add(new MapItem("berry_sitrus", 0, 100), 20);//sitrus
                        shop.Items.Add(new MapItem("seed_reviver", 0, 1200), 15);//reviver
                        shop.Items.Add(new MapItem("seed_ban", 0, 1000), 15);//ban

                        shop.Items.Add(new MapItem("seed_blast", 0, 900), 20);//blast seed

                        shop.Items.Add(new MapItem("orb_all_protect", 0, 600), 2);//all protect orb
                        shop.Items.Add(new MapItem("orb_cleanse", 0, 300), 2);//cleanse orb
                        shop.Items.Add(new MapItem("orb_one_shot", 0, 600), 2);//one-shot orb
                        shop.Items.Add(new MapItem("orb_nullify", 0, 400), 2);//nullify orb
                        shop.Items.Add(new MapItem("orb_mobile", 0, 600), 2);//mobile orb
                        shop.Items.Add(new MapItem("orb_luminous", 0, 600), 2);//luminous orb
                        shop.Items.Add(new MapItem("orb_fill_in", 0, 400), 2);//fill-in orb
                        shop.Items.Add(new MapItem("orb_one_room", 0, 500), 2);//one-room orb
                        shop.Items.Add(new MapItem("orb_rebound", 0, 400), 2);//rebound orb
                        shop.Items.Add(new MapItem("orb_mirror", 0, 400), 2);//mirror orb

                        shop.Items.Add(new MapItem("tm_will_o_wisp", 0, 1000), 2);//TM Will-o-Wisp
                        shop.Items.Add(new MapItem("tm_protect", 0, 1000), 2);//TM Protect
                        shop.Items.Add(new MapItem("tm_facade", 0, 1000), 2);//TM Facade
                        shop.Items.Add(new MapItem("tm_safeguard", 0, 1000), 2);//TM Safeguard
                        shop.Items.Add(new MapItem("tm_venoshock", 0, 1000), 2);//TM Venoshock
                        shop.Items.Add(new MapItem("tm_scald", 0, 1000), 2);//TM Scald
                        shop.Items.Add(new MapItem("tm_strength", 0, 1000), 2);//TM Strength
                        shop.Items.Add(new MapItem("tm_cut", 0, 1000), 2);//TM Cut
                        shop.Items.Add(new MapItem("tm_rock_smash", 0, 1000), 2);//TM Rock Smash
                        shop.Items.Add(new MapItem("tm_sludge_bomb", 0, 1000), 1);//TM Sludge Bomb
                        shop.Items.Add(new MapItem("tm_sludge_wave", 0, 1000), 1);//TM Sludge Wave
                        shop.Items.Add(new MapItem("tm_fire_blast", 0, 1000), 1);//TM Fire Blast

                        shop.Items.Add(new MapItem("held_poison_barb", 0, 1000), 2);//Poison Barb
                        shop.Items.Add(new MapItem("held_twisted_spoon", 0, 1000), 2);//Twisted Spoon
                        shop.Items.Add(new MapItem("held_toxic_plate", 0, 1000), 1);//Toxic Plate
                        shop.Items.Add(new MapItem("held_cover_band", 0, 1000), 2);//Cover Band
                        shop.Items.Add(new MapItem("held_reunion_cape", 0, 1000), 1);//Reunion Cape
                        shop.Items.Add(new MapItem("held_trap_scarf", 0, 1000), 2);//Trap Scarf
                        shop.Items.Add(new MapItem("held_grip_claw", 0, 1000), 2);//Grip Claw

                        shop.Items.Add(new MapItem("held_twist_band", 0, 1000), 2);//Twist Band
                        shop.Items.Add(new MapItem("held_metronome", 0, 1000), 1);//Metronome
                        shop.Items.Add(new MapItem("held_scope_lens", 0, 1000), 1);//Scope Lens
                        shop.Items.Add(new MapItem("held_power_band", 0, 1000), 2);//Power Band
                        shop.Items.Add(new MapItem("held_special_band", 0, 1000), 2);//Special Band
                        shop.Items.Add(new MapItem("held_defense_scarf", 0, 1000), 2);//Defense Scarf
                        shop.Items.Add(new MapItem("held_zinc_band", 0, 1000), 2);//Zinc Band

                        shop.Items.Add(new MapItem("held_shed_shell", 0, 1000), 2);//Shed Shell
                        shop.Items.Add(new MapItem("held_big_root", 0, 1000), 2);//Big Root

                        shop.Items.Add(new MapItem("machine_ability_capsule", 0, 1000), 20);//Ability Capsule

                        shop.Items.Add(new MapItem("evo_fire_stone", 0, 1500), 10);//Fire Stone
                        shop.Items.Add(new MapItem("evo_dusk_stone", 0, 1500), 10);//Dusk Stone
                        shop.Items.Add(new MapItem("evo_shiny_stone", 0, 1500), 10);//Shiny Stone
                        shop.Items.Add(new MapItem("evo_reaper_cloth", 0, 2000), 10);//Reaper Cloth

                        shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 6)), 10);

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

                        shopZoneSpawns.Add(new GenPriority<GenStep<MapGenContext>>(PR_SHOPS, shop), new IntRange(2, max_floors), 10);
                    }
                    SpreadStepRangeZoneStep shopZoneStep = new SpreadStepRangeZoneStep(new SpreadPlanQuota(new RandDecay(0, 4, 35), new IntRange(2, max_floors)), shopZoneSpawns);
                    shopZoneStep.ModStates.Add(new FlagType(typeof(ShopModGenState)));
                    floorSegment.ZoneSteps.Add(shopZoneStep);


                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        if (ii < 10)
                            AddFloorData(layout, "B12. Sickly Hollow.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);
                        else
                            AddFloorData(layout, "B13. Sickly Hollow 2.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                        //Tilesets
                        if (ii < 10)
                            AddTextureData(layout, "howling_forest_2_wall", "howling_forest_2_floor", "howling_forest_2_secondary", "poison");
                        else
                            AddTextureData(layout, "mystery_jungle_2_wall", "mystery_jungle_2_floor", "mystery_jungle_2_secondary", "poison");

                        if (ii < 3)
                        {

                        }
                        else if (ii < 10)
                            AddWaterSteps(layout, "water_poison", new RandRange(20));//poison
                        else
                            AddWaterSteps(layout, "pit", new RandRange(20), false);//abyss

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder", false);//wonder tile
                        AddTrapsSteps(layout, new RandRange(12, 16));

                        //money
                        AddMoneyData(layout, new RandRange(3, 5));

                        //enemies! ~ lv 5 to 10
                        AddRespawnData(layout, 15, 50);

                        //enemies
                        if (ii < 10)
                            AddEnemySpawnData(layout, 20, new RandRange(4, 8));
                        else
                            AddEnemySpawnData(layout, 20, new RandRange(6, 10));

                        //items
                        AddItemData(layout, new RandRange(4, 7), 25);


                        //construct paths
                        if (ii < 5)
                        {
                            AddInitGridStep(layout, 4, 4, 10, 10);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(90);
                            path.BranchRatio = new RandRange(0, 25);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //blocked
                            genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(5, 9), new RandRange(5, 9), new RandRange(1, 3), new RandRange(1, 3)), 10);
                            //cave
                            genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(4, 9), new RandRange(4, 9)), 50);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(0), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 0));
                        }
                        else if (ii < 10)
                        {
                            AddInitGridStep(layout, 5, 4, 9, 10);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(90);
                            path.BranchRatio = new RandRange(0, 25);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //blocked
                            genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(5, 9), new RandRange(5, 9), new RandRange(2, 5), new RandRange(2, 5)), 10);
                            //cave
                            genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(4, 9), new RandRange(4, 9)), 50);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(100), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(25, 100));
                        }
                        else
                        {
                            AddInitGridStep(layout, 5, 5, 9, 9, 1);

                            GridPathCircle<MapGenContext> path = new GridPathCircle<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.CircleRoomRatio = new RandRange(70);
                            path.Paths = new RandRange(2, 5);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //blocked
                            genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(5, 9), new RandRange(5, 9), new RandRange(2, 5), new RandRange(2, 5)), 10);
                            //cave
                            genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(4, 9), new RandRange(4, 9)), 50);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(100), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, false);

                        //layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        floorSegment.Floors.Add(layout);
                    }

                    {
                        LoadGen layout = new LoadGen();
                        MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                        startGen.MapID = "zone_37_end";
                        layout.GenSteps.Add(PR_TILES_INIT, startGen);
                        //add a chest

                        //List<(InvItem, Loc)> items = new List<(InvItem, Loc)>();
                        //items.Add((new InvItem("apricorn_plain"), new Loc(13, 10)));//Plain Apricorn
                        //layout.GenSteps.Add(PR_SPAWN_ITEMS, new SpecificSpawnStep<MapLoadContext, InvItem>(items));

                        List<InvItem> treasure1 = new List<InvItem>();
                        //TODO: a specific item from anyone in the dex
                        treasure1.Add(InvItem.CreateBox("box_glittery", "medicine_amber_tear"));//Amber Tear
                        treasure1.Add(InvItem.CreateBox("box_glittery", "ammo_golden_thorn"));//Golden Thorn
                        treasure1.Add(InvItem.CreateBox("box_glittery", "loot_nugget"));//Nugget

                        List<InvItem> treasure2 = new List<InvItem>();
                        treasure2.Add(InvItem.CreateBox("box_light", "xcl_element_poison_dust"));//Poison Dust
                        List<(List<InvItem>, Loc)> items = new List<(List<InvItem>, Loc)>();
                        items.Add((treasure1, new Loc(7, 7)));
                        items.Add((treasure2, new Loc(7, 13)));
                        AddSpecificSpawnPool(layout, items, PR_SPAWN_ITEMS);

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
                #endregion
            }
            else if (index == 38)
            {
                #region ENERGY GARDEN
                {
                    zone.Name = new LocalText("**Energy Garden");
                    zone.Level = 35;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 12;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Energy Garden\nB{0}F")));

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
                            //lightning_field
                            if (ii < 9)
                                AddTextureData(layout, "western_cave_1_wall", "western_cave_1_floor", "western_cave_1_secondary", "normal");
                            else
                                AddTextureData(layout, "western_cave_2_wall", "western_cave_2_floor", "western_cave_2_secondary", "normal");

                            AddWaterSteps(layout, "water", new RandRange(30));//water

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


                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 39)
            {
                #region TINY TUNNEL
                {
                    zone.Name = new LocalText("**Tiny Tunnel");
                    zone.Level = 5;
                    zone.LevelCap = true;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 8;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Tiny Tunnel\nB{0}F")));

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


                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 40)
            {
                #region Geode Underpass
                {
                    zone.Name = new LocalText("**Geode Underpass");
                    zone.Level = 10;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 12;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Geode Underpass\nB{0}F")));

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
                    zone.Level = 10;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 3;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Starfall Heights\n{0}F")));

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
                    zone.Level = 10;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 3;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Eon Island Coast\n{0}F")));

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
                    zone.Level = 10;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 3;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Barren Tundra\n{0}F")));

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


                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 44)
            {
                #region WAYWARD WETLANDS
                {
                    zone.Name = new LocalText("**Wayward Wetlands");
                    zone.Level = 10;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 3;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Wayward Wetlands\n{0}F")));

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


                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 45)
            {
                #region BRAVERY ROAD
                {
                    zone.Name = new LocalText("**Bravery Road");
                    zone.Level = 10;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 3;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Bravery Road\n{0}F")));

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
                    zone.Level = 10;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 3;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Wisdom Road\n{0}F")));

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
                    zone.Level = 10;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 3;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Hope Road\n{0}F")));

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
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 18;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Deserted Fortress\n{0}F")));

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


                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 49)
            {
                #region PRISM ISLES
                {
                    zone.Name = new LocalText("**Prism Isles");
                    zone.Level = 10;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 3;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("White Isle\n{0}F")));

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


                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 50)
            {
                #region LABYRINTH OF THE LOST
                {
                    zone.Name = new LocalText("**LABYRINTH OF THE LOST");
                    zone.Level = 10;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 3;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Labyrinth of the Lost\nB{0}F")));

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


                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 51)
            {
                #region SACRED TOWER
                {
                    zone.Name = new LocalText("**Sacred Tower");
                    zone.Level = 10;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 3;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Sacred Tower\n{0}F")));

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
                            // candidates: buried_relic_2,temporal_spire,temporal_tower,joyous_tower,electric maze
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


                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 52)
            {
                #region LOST SEAS
                {
                    zone.Name = new LocalText("**Lost Seas");
                    zone.Level = 10;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 3;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Lost Seas\nB{0}F")));

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


                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
                }
                #endregion
            }
            else if (index == 53)
            {
                #region INSCRIBED CAVE
                {
                    zone.Name = new LocalText("**Inscribed Cave");
                    zone.Level = 10;
                    zone.Rescues = 2;
                    zone.Rogue = RogueStatus.ItemTransfer;

                    {
                        int max_floors = 3;
                        LayeredSegment floorSegment = new LayeredSegment();
                        floorSegment.IsRelevant = true;
                        floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                        floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Inscribed Cave\nB{0}F")));

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


                            floorSegment.Floors.Add(layout);
                        }

                        zone.Segments.Add(floorSegment);
                    }
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
