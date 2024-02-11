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

        static void FillBrambleWoods(ZoneData zone, bool translate)
        {
            #region BRAMBLE WOODS
            zone.Name = new LocalText("Bramble Woods");
            zone.Rescues = 2;
            zone.Level = 10;
            zone.Rogue = RogueStatus.NoTransfer;

            {
                int max_floors = 7;
                LayeredSegment floorSegment = new LayeredSegment();
                floorSegment.IsRelevant = true;
                floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Bramble Woods\nB{0}F"), new Priority(-15)));

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
                    TeamMemberSpawn teamSpawn = GetTeamMob("nidoran_m", "poison_point", "leer", "peck", "", "", new RandRange(7), "wander_dumb");
                    teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(0, 2));
                    poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 10);
                }
                {
                    //029 Nidoran♀ : 079 Rivalry : 045 Growl : 010 Scratch
                    TeamMemberSpawn teamSpawn = GetTeamMob("nidoran_f", "poison_point", "growl", "scratch", "", "", new RandRange(7), "wander_dumb");
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

                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("food_apple"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(3, 7), new IntRange(0, max_floors)), new MapItem("berry_leppa"));

                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(3, 7), new IntRange(0, max_floors)),
                    new MapItem("apricorn_green"), new MapItem("apricorn_purple"));

                RandBag<IGenStep> npcZoneSpawns = new RandBag<IGenStep>(true, new List<IGenStep>());
                //poison protection
                {
                    PresetMultiTeamSpawner<ListMapGenContext> multiTeamSpawner = new PresetMultiTeamSpawner<ListMapGenContext>();
                    MobSpawn post_mob = new MobSpawn();
                    post_mob.BaseForm = new MonsterID("sandshrew", 0, "normal", Gender.Male);
                    post_mob.Tactic = "slow_wander";
                    post_mob.Level = new RandRange(14);
                    post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_ADVICE_POISON"))));
                    SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);
                    post_team.Explorer = true;
                    multiTeamSpawner.Spawns.Add(post_team);
                    PlaceRandomMobsStep<ListMapGenContext> randomSpawn = new PlaceRandomMobsStep<ListMapGenContext>(multiTeamSpawner);
                    randomSpawn.Ally = true;
                    npcZoneSpawns.ToSpawn.Add(randomSpawn);
                }
                SpreadStepZoneStep npcZoneStep = new SpreadStepZoneStep(new SpreadPlanQuota(new RandRange(2), new IntRange(2, 6), true), PR_SPAWN_MOBS_EXTRA, npcZoneSpawns);
                floorSegment.ZoneSteps.Add(npcZoneStep);


                TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                tileSpawn.Priority = PR_RESPAWN_TRAP;
                floorSegment.ZoneSteps.Add(tileSpawn);

                AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 2);


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

                    //money
                    AddMoneyData(layout, new RandRange(2, 4));

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

                    AddStairStep(layout, true);

                    if (ii > 4)
                        AddWaterSteps(layout, "water", new RandRange(30));//water



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
                            guardList.Add(GetGuardMob(new MonsterID("roselia", 0, "", Gender.Unknown), "poison_point", "growth", "leech_seed", "mega_drain", "", new RandRange(20), "wander_normal", "sleep"), 10);
                            LoopedRand<MobSpawn> guards = new LoopedRand<MobSpawn>(guardList, new RandRange(1));
                            GuardSealStep<MapGenContext> vaultStep = new GuardSealStep<MapGenContext>(guards);
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
                            treasures.SpecificSpawns.Add(new MoneySpawn(100));
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

                    if (ii == 4)
                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectTileStep<MapGenContext>("stairs_secret_up"));

                    floorSegment.Floors.Add(layout);
                }
                {
                    LoadGen layout = new LoadGen();
                    MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                    startGen.MapID = "end_bramble_woods";
                    layout.GenSteps.Add(PR_FILE_LOAD, startGen);

                    MapTimeLimitStep<MapLoadContext> floorData = new MapTimeLimitStep<MapLoadContext>(600);
                    layout.GenSteps.Add(PR_FLOOR_DATA, floorData);

                    AddTextureData(layout, "mystifying_forest_wall", "mystifying_forest_floor", "mystifying_forest_secondary", "bug");

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
                floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Bramble Thicket\nB{0}F"), new Priority(-15)));

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
                    TeamMemberSpawn teamSpawn = GetTeamMob("nidoran_m", "poison_point", "leer", "peck", "", "", new RandRange(7), "wander_dumb");
                    teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(0, 2));
                    poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 10);
                }
                {
                    //029 Nidoran♀ : 079 Rivalry : 045 Growl : 010 Scratch
                    TeamMemberSpawn teamSpawn = GetTeamMob("nidoran_f", "poison_point", "growl", "scratch", "", "", new RandRange(7), "wander_dumb");
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

                AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 3);

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

                    AddStairStep(layout, true);

                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

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


        static void FillFaultlineRidge(ZoneData zone, bool translate)
        {
            #region FAULTLINE RIDGE
            zone.Name = new LocalText("Faultline Ridge");
            zone.Rescues = 2;
            zone.Level = 15;
            zone.Rogue = RogueStatus.NoTransfer;

            {
                int max_floors = 10;
                LayeredSegment floorSegment = new LayeredSegment();
                floorSegment.IsRelevant = true;
                floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Faultline Ridge\n{0}F"), new Priority(-15)));

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
                poolSpawn.Spawns.Add(GetTeamMob("geodude", "", "magnitude", "smack_down", "", "", new RandRange(16), "wander_dumb"), new IntRange(3, max_floors), 10);
                // 299 Nosepass : 88 Rock Throw : 86 Thunder Wave
                poolSpawn.Spawns.Add(GetTeamMob("nosepass", "", "rock_throw", "thunder_wave", "", "", new RandRange(15), "wander_dumb"), new IntRange(6, max_floors), 10);
                // 231 Phanpy : 205 Rollout : 175 Flail
                poolSpawn.Spawns.Add(GetTeamMob("phanpy", "", "rollout", "flail", "", "", new RandRange(12), "wander_dumb"), new IntRange(0, max_floors), 10);
                // 447 Riolu : 203 Endure : 98 Quick Attack 
                poolSpawn.Spawns.Add(GetTeamMob("riolu", "", "endure", "quick_attack", "", "", new RandRange(14), "wander_dumb"), new IntRange(3, 6), 10);
                //296  Makuhita : 292 Arm Thrust : 252 Fake Out 
                poolSpawn.Spawns.Add(GetTeamMob("makuhita", "", "arm_thrust", "fake_out", "", "", new RandRange(12), "wander_dumb"), new IntRange(0, 6), 10);
                // 207 Gligar : 98 Quick Attack : 282 Knock Off
                poolSpawn.Spawns.Add(GetTeamMob("gligar", "", "quick_attack", "knock_off", "", "", new RandRange(14), "wander_dumb"), new IntRange(0, 6), 10);

                poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                floorSegment.ZoneSteps.Add(poolSpawn);

                List<string> tutorElements = new List<string>() { "grass", "water", "fire", "normal" };
                AddTutorZoneStep(floorSegment, new SpreadPlanQuota(new RandDecay(1, 2, 30), new IntRange(0, max_floors), true), new IntRange(0, 5), tutorElements);

                TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                tileSpawn.Priority = PR_RESPAWN_TRAP;
                tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, max_floors), 10);//chestnut trap
                tileSpawn.Spawns.Add(new EffectTile("trap_gust", false), new IntRange(0, max_floors), 10);//trip trap
                floorSegment.ZoneSteps.Add(tileSpawn);


                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 8), new IntRange(0, max_floors)), new MapItem("food_apple"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("berry_leppa"));

                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)),
                    new MapItem("apricorn_brown"), new MapItem("apricorn_white"));

                {
                    SpreadHouseZoneStep chestChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanQuota(new RandDecay(1, 8, 60), new IntRange(4, max_floors)));
                    chestChanceZoneStep.ModStates.Add(new FlagType(typeof(ChestModGenState)));
                    chestChanceZoneStep.HouseStepSpawns.Add(new ChestStep<ListMapGenContext>(false, GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);

                    foreach (string key in IterateGummis())
                        chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//gummis
                    foreach (string key in IterateEvoItems(EvoClass.Early))
                        chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 8);
                    chestChanceZoneStep.Items.Add(new MapItem("apricorn_big"), new IntRange(0, max_floors), 20);//big apricorn
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_elixir"), new IntRange(0, max_floors), 40);//elixir
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_potion"), new IntRange(0, max_floors), 30);//potion
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_max_elixir"), new IntRange(0, max_floors), 10);//max elixir
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_max_potion"), new IntRange(0, max_floors), 10);//max potion
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_full_heal"), new IntRange(0, max_floors), 20);//full heal
                    foreach (string key in IterateXItems())
                        chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 15);//X-Items
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear", 1), new IntRange(0, max_floors), 20);//amber tear

                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeNone(100, new RandRange(1, 3)), new IntRange(0, max_floors), 30);

                    floorSegment.ZoneSteps.Add(chestChanceZoneStep);
                }

                AddHiddenStairStep(floorSegment, new SpreadPlanQuota(new RandRange(1), new IntRange(0, max_floors - 1)), 1);

                AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 2);

                for (int ii = 0; ii < max_floors; ii++)
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
                    if (ii < 6)
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                    else
                        AddSingleTrapStep(layout, new RandRange(1, 3), "tile_wonder");//wonder tile

                    if (ii < 6)
                        AddTrapsSteps(layout, new RandRange(12, 16));
                    else
                        AddTrapsSteps(layout, new RandRange(36, 42), true);

                    //money
                    AddMoneyData(layout, new RandRange(2, 5));

                    //enemies
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
                        AddItemData(layout, new RandRange(4, 6), 25, true);


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

                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                    floorSegment.Floors.Add(layout);
                }

                zone.Segments.Add(floorSegment);
            }

            {
                SingularSegment structure = new SingularSegment(-1);

                SpawnList<TeamMemberSpawn> enemyList = new SpawnList<TeamMemberSpawn>();
                structure.BaseFloor = getSecretRoom(translate, "special_rby_fossil", -1, "amp_plains_wall", "amp_plains_floor", "amp_plains_secondary", "", "ground", DungeonStage.Beginner, DungeonAccessibility.Unlockable, enemyList, new Loc(5, 11));

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

            #endregion
        }


        static void FillFertileValley(ZoneData zone, bool translate)
        {
            #region FERTILE VALLEY
            {
                zone.Name = new LocalText("Fertile Valley");
                zone.Rescues = 2;
                zone.Level = 15;
                zone.ExpPercent = 80;
                zone.Rogue = RogueStatus.NoTransfer;

                {
                    int max_floors = 8;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Fertile Valley\nB{0}F"), new Priority(-15)));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 0);
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);
                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;


                    //necessities
                    CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                    necessities.SpawnRates.SetRange(20, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("necessities", necessities);


                    necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 9);
                    necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 12);
                    necessities.Spawns.Add(new InvItem("food_apple"), new IntRange(0, max_floors), 10);
                    necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 10);
                    necessities.Spawns.Add(new InvItem("seed_reviver"), new IntRange(0, max_floors), 5);
                    necessities.Spawns.Add(new InvItem("food_apple_big"), new IntRange(0, max_floors), 10);
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
                    {
                        TeamMemberSpawn mob = GetTeamMob("tauros", "", "rage", "horn_attack", "", "", new RandRange(20), "wander_normal", true);
                        MobSpawnItem itemSpawn = new MobSpawnItem(true);
                        itemSpawn.Items.Add(new InvItem("herb_white"), 10);
                        itemSpawn.Items.Add(new InvItem("herb_mental"), 10);
                        mob.Spawn.SpawnFeatures.Add(itemSpawn);
                        poolSpawn.Spawns.Add(mob, new IntRange(3, max_floors), 5);
                    }

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;


                    tileSpawn.Spawns.Add(new EffectTile("trap_mud", false), new IntRange(0, max_floors), 10);//mud trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_slow", false), new IntRange(0, max_floors), 10);//slow trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, max_floors), 10);//chestnut trap

                    floorSegment.ZoneSteps.Add(tileSpawn);

                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors)), new MapItem("food_apple"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(3, 5), new IntRange(0, max_floors)), new MapItem("berry_leppa"));

                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)),
                        new MapItem("apricorn_blue"), new MapItem("apricorn_green"), new MapItem("apricorn_brown"), new MapItem("apricorn_purple"),
                        new MapItem("apricorn_white"), new MapItem("apricorn_black"));

                    AddHiddenStairStep(floorSegment, new SpreadPlanQuota(new RandRange(1), new IntRange(0, max_floors - 1)), 2);

                    AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 3);

                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        AddFloorData(layout, "B16. Fertile Valley.ogg", 800, Map.SightRange.Clear, Map.SightRange.Clear);

                        //Tilesets
                        if (ii < 6)
                            AddTextureData(layout, "purity_forest_7_wall", "purity_forest_7_floor", "purity_forest_7_secondary", "ground");
                        else
                            AddTextureData(layout, "purity_forest_6_wall", "purity_forest_6_floor", "purity_forest_6_secondary", "ground");

                        //cave in
                        if (ii != 2)
                        {
                            RandRange amount = new RandRange(3, 8);
                            IntRange size = new IntRange(3, 9);
                            MultiBlobStencil<MapGenContext> multiBlobStencil = new MultiBlobStencil<MapGenContext>(false);
                            multiBlobStencil.List.Add(new BlobTileStencil<MapGenContext>(new MapTerrainStencil<MapGenContext>(false, false, true, true), true));

                            //not allowed to draw the blob over start or end.
                            multiBlobStencil.List.Add(new StairsStencil<MapGenContext>(true));
                            //effect tile checks are also needed since even though they are postproc-shielded, it'll cut off the path to those locations
                            multiBlobStencil.List.Add(new BlobTileStencil<MapGenContext>(new TileEffectStencil<MapGenContext>(true)));

                            //not allowed to draw individual tiles over unbreakable tiles
                            BlobWaterStep<MapGenContext> waterStep = new BlobWaterStep<MapGenContext>(amount, new Tile("floor"), new MatchTerrainStencil<MapGenContext>(true, new Tile("unbreakable")), multiBlobStencil, size, new IntRange(Math.Max(size.Min, 7), Math.Max(size.Max * 3 / 2, 8)));
                            layout.GenSteps.Add(PR_WATER, waterStep);
                        }

                        AddWaterSteps(layout, "water", new RandRange(12));//water

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(6, 9));

                        //money
                        AddMoneyData(layout, new RandRange(2, 5));

                        //enemies
                        AddRespawnData(layout, 6, 90);
                        AddEnemySpawnData(layout, 20, new RandRange(3, 6));

                        //items
                        AddItemData(layout, new RandRange(2, 5), 25);


                        //construct paths
                        {
                            AddInitGridStep(layout, 9, 6, 7, 6);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(35);
                            path.BranchRatio = new RandRange(0);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //bump
                            genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8), new RandRange(30, 70)), 10);
                            //round
                            genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(100), 10);
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(0, new SquareHallBrush(new Loc(2))), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(30, 50));

                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, true);

                        if (ii == 2)
                        {
                            //vault rooms
                            {
                                SpawnList<RoomGen<MapGenContext>> detourRooms = new SpawnList<RoomGen<MapGenContext>>();
                                RoomGenLoadMap<MapGenContext> loadRoom = new RoomGenLoadMap<MapGenContext>();
                                loadRoom.MapID = "room_muddy_valley_entrance";
                                loadRoom.RoomTerrain = new Tile("floor");
                                loadRoom.PreventChanges = PostProcType.Panel | PostProcType.Terrain;
                                detourRooms.Add(loadRoom, 10);
                                SpawnList<PermissiveRoomGen<MapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                                RoomGenAngledHall<MapGenContext> hall = new RoomGenAngledHall<MapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4));
                                hall.Brush = new TerrainHallBrush(Loc.One, new Tile("water"));
                                detourHalls.Add(hall, 10);
                                AddConnectedRoomsStep<MapGenContext> detours = new AddConnectedRoomsStep<MapGenContext>(detourRooms, detourHalls);
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
                                TerrainSealStep<MapGenContext> vaultStep = new TerrainSealStep<MapGenContext>("water", "water");
                                vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BlockVault));
                                layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                            }
                        }

                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        if (ii == 2)
                            layout.GenSteps.Add(PR_DBG_CHECK, new DetectTileStep<MapGenContext>("stairs_secret_down"));

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }

                {
                    int max_floors = 5;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Muddy Valley\nB{0}F"), new Priority(-15)));

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


                    List<string> tutorElements = new List<string>() { "ice", "psychic", "flying", "normal" };
                    AddTutorZoneStep(floorSegment, new SpreadPlanQuota(new RandDecay(0, 1, 60), new IntRange(0, max_floors), true), new IntRange(0, 5), tutorElements);


                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;

                    tileSpawn.Spawns.Add(new EffectTile("trap_mud", false), new IntRange(0, max_floors), 10);//mud trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_slow", false), new IntRange(0, max_floors), 10);//slow trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, max_floors), 10);//chestnut trap

                    floorSegment.ZoneSteps.Add(tileSpawn);

                    AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 15, 3);

                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        AddFloorData(layout, "B17. Muddy Valley.ogg", 1000, Map.SightRange.Clear, Map.SightRange.Dark);

                        //Tilesets
                        AddTextureData(layout, "quicksand_pit_wall", "quicksand_pit_floor", "quicksand_pit_secondary", "ground");

                        //cave in
                        {
                            RandRange amount = new RandRange(5, 10);
                            IntRange size = new IntRange(3, 9);
                            MultiBlobStencil<MapGenContext> multiBlobStencil = new MultiBlobStencil<MapGenContext>(false);
                            multiBlobStencil.List.Add(new BlobTileStencil<MapGenContext>(new MapTerrainStencil<MapGenContext>(false, false, true, true), true));

                            //not allowed to draw the blob over start or end.
                            multiBlobStencil.List.Add(new StairsStencil<MapGenContext>(true));
                            //effect tile checks are also needed since even though they are postproc-shielded, it'll cut off the path to those locations
                            multiBlobStencil.List.Add(new BlobTileStencil<MapGenContext>(new TileEffectStencil<MapGenContext>(true)));

                            //not allowed to draw individual tiles over unbreakable tiles
                            BlobWaterStep<MapGenContext> waterStep = new BlobWaterStep<MapGenContext>(amount, new Tile("floor"), new MatchTerrainStencil<MapGenContext>(true, new Tile("unbreakable")), multiBlobStencil, size, new IntRange(Math.Max(size.Min, 7), Math.Max(size.Max * 3 / 2, 8)));
                            layout.GenSteps.Add(PR_WATER, waterStep);
                        }

                        AddWaterSteps(layout, "water", new RandRange(20));//water

                        //money
                        AddMoneyData(layout, new RandRange(2, 5));

                        //items
                        AddItemData(layout, new RandRange(2, 5), 25);

                        //enemies
                        AddRespawnData(layout, 6, 90);
                        AddEnemySpawnData(layout, 20, new RandRange(3, 6));

                        //traps
                        AddSingleTrapStep(layout, new RandRange(5, 8), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(6, 9));

                        //construct paths
                        {
                            AddInitGridStep(layout, 9, 6, 7, 6);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(40);
                            path.BranchRatio = new RandRange(0);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //bump
                            genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8), new RandRange(30, 70)), 10);
                            //round
                            genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(30, 50));
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
                    //enemyList.Add(GetTeamMob(new MonsterID("vivillon", 5, "", Gender.Unknown), "", "poison_powder", "psybeam", "powder", "struggle_bug", new RandRange(zone.Level)), 10);
                    structure.BaseFloor = getSecretRoom(translate, "special_gsc_plant", -2, "purity_forest_7_wall", "purity_forest_7_floor", "purity_forest_7_secondary", "tall_grass", "water", DungeonStage.Beginner, DungeonAccessibility.Unlockable, enemyList, new Loc(9, 5));

                    zone.Segments.Add(structure);
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


                {
                    SingularSegment structure = new SingularSegment(-1);

                    ChanceFloorGen multiGen = new ChanceFloorGen();
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "small_square", -3, false, false), 10);
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "tall_hall", -3, false, false), 10);
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "wide_hall", -3, false, false), 10);
                    structure.BaseFloor = multiGen;

                    zone.Segments.Add(structure);
                }
            }
            #endregion
        }


        static void FillLavaFloeIsland(ZoneData zone, bool translate)
        {
            #region LAVA FLOE ISLAND
            {
                zone.Name = new LocalText("Lava Floe Island");
                zone.Rescues = 2;
                zone.Level = 20;
                zone.ExpPercent = 75;
                zone.TeamSize = 2;
                zone.Rogue = RogueStatus.NoTransfer;

                {
                    int max_floors = 16;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Lava Floe\nIsland\nB{0}F"), new Priority(-15)));

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


                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(3, 7), new IntRange(0, max_floors)), new MapItem("food_apple"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("berry_leppa"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(3, max_floors)), new MapItem("machine_assembly_box"));

                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)),
                        new MapItem("apricorn_blue"), new MapItem("apricorn_brown"), new MapItem("apricorn_red"), new MapItem("apricorn_white"));

                    {
                        SpreadHouseZoneStep chestChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanQuota(new RandDecay(1, 8, 60), new IntRange(4, max_floors)));
                        chestChanceZoneStep.ModStates.Add(new FlagType(typeof(ChestModGenState)));
                        chestChanceZoneStep.HouseStepSpawns.Add(new ChestStep<ListMapGenContext>(false, GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);

                        foreach (string key in IterateGummis())
                            chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//gummis
                        chestChanceZoneStep.Items.Add(new MapItem("apricorn_big"), new IntRange(0, max_floors), 20);//big apricorn
                        chestChanceZoneStep.Items.Add(new MapItem("medicine_elixir"), new IntRange(0, max_floors), 80);//elixir
                        chestChanceZoneStep.Items.Add(new MapItem("medicine_potion"), new IntRange(0, max_floors), 40);//potion
                        chestChanceZoneStep.Items.Add(new MapItem("medicine_max_elixir"), new IntRange(0, max_floors), 10);//max elixir
                        chestChanceZoneStep.Items.Add(new MapItem("medicine_max_potion"), new IntRange(0, max_floors), 10);//max potion
                        chestChanceZoneStep.Items.Add(new MapItem("medicine_full_heal"), new IntRange(0, max_floors), 20);//full heal
                        foreach (string key in IterateXItems())
                            chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 15);//X-Items
                        chestChanceZoneStep.Items.Add(new MapItem("loot_nugget"), new IntRange(0, max_floors), 20);//nugget
                        chestChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear", 1), new IntRange(0, max_floors), 20);//amber tear

                        chestChanceZoneStep.ItemThemes.Add(new ItemThemeNone(100, new RandRange(1, 3)), new IntRange(0, max_floors), 30);

                        floorSegment.ZoneSteps.Add(chestChanceZoneStep);
                    }

                    AddEvoZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(3, 7), new IntRange(1, max_floors)), false);

                    AddHiddenStairStep(floorSegment, new SpreadPlanQuota(new RandRange(1), new IntRange(0, max_floors - 1)), 2);

                    AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 3);


                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        RoomFloorGen layout = new RoomFloorGen();

                        //Floor settings
                        string song = "B37. Lava Floe Island Fire.ogg";
                        if (ii % 2 == 0)
                            song = "B37. Lava Floe Island Water.ogg";

                        if (ii < 8)
                            AddFloorData(layout, song, 1500, Map.SightRange.Clear, Map.SightRange.Clear);
                        else
                            AddFloorData(layout, song, 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                        //Tilesets
                        //other caniddates: side_path,lower_brine_cave
                        string wallTex;
                        string floorTex;
                        string secondaryTex;
                        string element;
                        if (ii < 8)
                        {
                            if (ii % 2 == 0)
                            {
                                wallTex = "side_path_wall";
                                floorTex = "side_path_floor";
                                secondaryTex = "side_path_secondary";
                                element = "water";
                            }
                            else
                            {
                                wallTex = "mt_blaze_wall";
                                floorTex = "mt_blaze_floor";
                                secondaryTex = "mt_blaze_secondary";
                                element = "fire";
                            }
                        }
                        else
                        {
                            if (ii % 2 == 0)
                            {
                                wallTex = "craggy_coast_wall";
                                floorTex = "craggy_coast_floor";
                                secondaryTex = "craggy_coast_secondary";
                                element = "water";
                            }
                            else
                            {
                                wallTex = "magma_cavern_2_wall";
                                floorTex = "magma_cavern_2_floor";
                                secondaryTex = "magma_cavern_2_secondary";
                                element = "fire";
                            }
                        }

                        MapDictTextureStep<ListMapGenContext> textureStep = new MapDictTextureStep<ListMapGenContext>();
                        {
                            textureStep.BlankBG = secondaryTex;
                            textureStep.TextureMap["floor"] = floorTex;
                            textureStep.TextureMap["unbreakable"] = wallTex;
                            textureStep.TextureMap["wall"] = wallTex;
                            textureStep.TextureMap["water"] = secondaryTex;
                            textureStep.TextureMap["lava"] = secondaryTex;
                            textureStep.TextureMap["water_poison"] = secondaryTex;
                            textureStep.TextureMap["pit"] = secondaryTex;
                        }
                        textureStep.GroundElement = element;
                        textureStep.LayeredGround = true;
                        layout.GenSteps.Add(PR_TEXTURES, textureStep);

                        if (ii % 2 == 0)
                            AddWaterSteps(layout, "water", new RandRange(100));//water
                        else
                            AddWaterSteps(layout, "lava", new RandRange(100));//lava

                        //put the walls back in via "water" algorithm
                        AddBlobWaterSteps(layout, "wall", new RandRange(1, 5), new IntRange(1, 9), false);


                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(6, 9));

                        //money
                        AddMoneyData(layout, new RandRange(1, 4));

                        //enemies
                        AddRespawnData(layout, 7, 90);
                        AddEnemySpawnData(layout, 20, new RandRange(4, 7));

                        //items
                        AddItemData(layout, new RandRange(2, 6), 25);

                        if (ii >= 10)
                        {
                            //secret items
                            SpawnList<InvItem> secretItemSpawns = new SpawnList<InvItem>();
                            secretItemSpawns.Add(new InvItem("held_weather_rock"), 3);//Weather Rock
                            secretItemSpawns.Add(new InvItem("loot_pearl", false, 2), 10);//Pearl
                            secretItemSpawns.Add(new InvItem("key", false, 1), 10);//Key
                            foreach (string key in IterateGummis())
                                secretItemSpawns.Add(new InvItem(key), 2);

                            RandRange spawnRange = (ii < 12) ? new RandRange(0, 2) : new RandRange(1, 3);

                            RandomRoomSpawnStep<ListMapGenContext, InvItem> secretPlacement = new RandomRoomSpawnStep<ListMapGenContext, InvItem>(new PickerSpawner<ListMapGenContext, InvItem>(new LoopedRand<InvItem>(secretItemSpawns, spawnRange)));
                            secretPlacement.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Disconnected));
                            layout.GenSteps.Add(PR_SPAWN_ITEMS, secretPlacement);
                        }

                        if (ii >= 8)
                        {
                            //secret money
                            List<MapItem> secretItemSpawns = new List<MapItem>();
                            secretItemSpawns.Add(MapItem.CreateMoney(150));
                            if (ii < 12)
                                secretItemSpawns.Add(MapItem.CreateMoney(150));
                            RandomRoomSpawnStep<ListMapGenContext, MapItem> secretPlacement = new RandomRoomSpawnStep<ListMapGenContext, MapItem>(new PickerSpawner<ListMapGenContext, MapItem>(new PresetMultiRand<MapItem>(secretItemSpawns)));
                            secretPlacement.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Disconnected));
                            layout.GenSteps.Add(PR_SPAWN_ITEMS, secretPlacement);
                        }

                        //construct paths
                        {
                            //Initialize a 54x40 floorplan with which to populate with rectangular floor and halls.
                            AddInitListStep(layout, 44, 34);

                            {
                                //Create a path that is composed of a branching tree
                                FloorPathBranch<ListMapGenContext> path = new FloorPathBranch<ListMapGenContext>();
                                path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                path.HallPercent = 30;
                                path.FillPercent = new RandRange(45);
                                path.BranchRatio = new RandRange(30);

                                //Give it some room types to place
                                SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                                //cave
                                genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(4, 10), new RandRange(4, 10)), 10);
                                //round
                                genericRooms.Add(new RoomGenRound<ListMapGenContext>(new RandRange(4, 8), new RandRange(4, 8)), 5);

                                path.GenericRooms = genericRooms;

                                //Give it some hall types to place
                                SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                                genericHalls.Add(new RoomGenSquare<ListMapGenContext>(new RandRange(1), new RandRange(1)), 20);
                                path.GenericHalls = genericHalls;

                                layout.GenSteps.Add(PR_ROOMS_GEN, path);
                            }


                            if (ii == 5)
                            {
                                AddDisconnectedRoomsStep<ListMapGenContext> addDisconnect = new AddDisconnectedRoomsStep<ListMapGenContext>();
                                addDisconnect.Components.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Disconnected));
                                addDisconnect.Components.Set(new ImmutableRoom());
                                addDisconnect.Amount = new RandRange(1);

                                SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();

                                RoomGenLoadMap<ListMapGenContext> loadRoom = new RoomGenLoadMap<ListMapGenContext>();
                                loadRoom.MapID = "room_inscribed_entrance";
                                loadRoom.RoomTerrain = new Tile("floor");
                                loadRoom.PreventChanges = PostProcType.Panel | PostProcType.Terrain;
                                genericRooms.Add(loadRoom, 10);

                                addDisconnect.GenericRooms = genericRooms;
                                layout.GenSteps.Add(PR_ROOMS_GEN, addDisconnect);
                            }

                            if (ii >= 8)
                            {
                                //Add some disconnected rooms
                                AddDisconnectedRoomsStep<ListMapGenContext> addDisconnect = new AddDisconnectedRoomsStep<ListMapGenContext>();
                                addDisconnect.Components.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Disconnected));
                                addDisconnect.Amount = new RandRange(1, 4);

                                //Give it some room types to place
                                SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                                //cave
                                genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(5, 10), new RandRange(5, 10)), 10);
                                //circle
                                genericRooms.Add(new RoomGenRound<ListMapGenContext>(new RandRange(4, 6), new RandRange(4, 6)), 5);

                                addDisconnect.GenericRooms = genericRooms;


                                layout.GenSteps.Add(PR_ROOMS_GEN, addDisconnect);
                            }
                        }

                        //draw paths
                        layout.GenSteps.Add(PR_TILES_INIT, new DrawFloorToTileStep<ListMapGenContext>(5));

                        //add invisible unbreakable border
                        Tile invisBarrier = new Tile("unbreakable", true);
                        invisBarrier.Data.TileTex = new AutoTile(secondaryTex);
                        layout.GenSteps.Add(PR_TILES_BARRIER, new TileBorderStep<ListMapGenContext>(1, invisBarrier));

                        AddStairStep(layout, true);


                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<ListMapGenContext, MapGenEntrance, MapGenExit>());

                        if (ii == 5)
                            layout.GenSteps.Add(PR_DBG_CHECK, new DetectTileStep<ListMapGenContext>("stairs_secret_down"));

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }


                {
                    int max_floors = 10;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Abyssal Island\nB{0}F"), new Priority(-15)));

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
                        RoomFloorGen layout = new RoomFloorGen();

                        //Floor settings
                        AddFloorData(layout, "Limestone Cavern.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                        AddWaterSteps(layout, "floor", new RandRange(20));//empty


                        //Tilesets
                        string wallTex = "spacial_rift_1_wall";
                        string floorTex = "chasm_cave_1_floor";
                        string secondaryTex = "chasm_cave_1_wall";
                        string element = "flying";

                        AddTextureData(layout, wallTex, floorTex, secondaryTex, element);

                        AddWaterSteps(layout, "pit", new RandRange(50));//pit
                                                                        //put the walls back in via "water" algorithm
                        AddBlobWaterSteps(layout, "wall", new RandRange(3, 7), new IntRange(1, 9), false);

                        //money
                        AddMoneyData(layout, new RandRange(1, 4));

                        //items
                        AddItemData(layout, new RandRange(2, 6), 25);

                        //enemies
                        AddRespawnData(layout, 7, 40);
                        AddEnemySpawnData(layout, 20, new RandRange(4, 7));

                        //traps
                        AddSingleTrapStep(layout, new RandRange(5, 8), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(20, 24));



                        //construct paths
                        {
                            //Initialize a 54x40 floorplan with which to populate with rectangular floor and halls.
                            AddInitListStep(layout, 60, 48);

                            {
                                //Create a path that is composed of a branching tree
                                FloorPathBranch<ListMapGenContext> path = new FloorPathBranch<ListMapGenContext>();
                                path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                path.HallPercent = 30;
                                path.FillPercent = new RandRange(50);
                                path.BranchRatio = new RandRange(30);

                                //Give it some room types to place
                                SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                                //cave
                                genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(4, 10), new RandRange(4, 10)), 10);
                                //cross
                                genericRooms.Add(new RoomGenCross<ListMapGenContext>(new RandRange(3, 9), new RandRange(3, 9), new RandRange(2, 5), new RandRange(2, 5)), 5);

                                path.GenericRooms = genericRooms;

                                //Give it some hall types to place
                                SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                                genericHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(1), new RandRange(1)), 20);
                                path.GenericHalls = genericHalls;

                                layout.GenSteps.Add(PR_ROOMS_GEN, path);

                                layout.GenSteps.Add(PR_ROOMS_GEN, CreateGenericListConnect(25, 0));

                            }

                            {
                                //Add some disconnected rooms
                                AddDisconnectedRoomsRandStep<ListMapGenContext> addDisconnect = new AddDisconnectedRoomsRandStep<ListMapGenContext>();
                                addDisconnect.Components.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Disconnected));
                                addDisconnect.Amount = new RandRange(1, 4);

                                //Give it some room types to place
                                SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                                //cave
                                genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(5, 10), new RandRange(5, 10)), 10);
                                //circle
                                genericRooms.Add(new RoomGenRound<ListMapGenContext>(new RandRange(4, 6), new RandRange(4, 6)), 5);

                                addDisconnect.GenericRooms = genericRooms;


                                layout.GenSteps.Add(PR_ROOMS_GEN, addDisconnect);
                            }

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
                    //enemyList.Add(GetTeamMob(new MonsterID("vivillon", 9, "", Gender.Unknown), "", "poison_powder", "psybeam", "powder", "struggle_bug", new RandRange(zone.Level)), 10);
                    structure.BaseFloor = getSecretRoom(translate, "special_rby_fossil", -2, "craggy_coast_wall", "craggy_coast_floor", "craggy_coast_secondary", "", "fire", DungeonStage.Beginner, DungeonAccessibility.Unlockable, enemyList, new Loc(5, 11));

                    zone.Segments.Add(structure);
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
            }
            #endregion
        }


        static void FillVeiledRidge(ZoneData zone, bool translate)
        {
            #region VEILED RIDGE

            zone.Name = new LocalText("Veiled Ridge");
            zone.Rescues = 2;
            zone.Level = 35;
            zone.ExpPercent = 80;
            zone.Rogue = RogueStatus.NoTransfer;

            {
                int max_floors = 16;
                LayeredSegment floorSegment = new LayeredSegment();
                floorSegment.IsRelevant = true;
                floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Veiled Ridge\n{0}F"), new Priority(-15)));

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
                necessities.Spawns.Add(new InvItem("seed_reviver"), new IntRange(0, max_floors), 3);
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


                //limited spawn, spawns separately
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
                {

                    TeamMemberSpawn mob = GetTeamMob("grumpig", "thick_fat", "magic_coat", "psywave", "confuse_ray", "", new RandRange(30), "wander_normal", true);
                    MobSpawnItem keySpawn = new MobSpawnItem(true);
                    keySpawn.Items.Add(new InvItem("loot_pearl", false, 2), 10);
                    mob.Spawn.SpawnFeatures.Add(keySpawn);
                    poolSpawn.Spawns.Add(mob, new IntRange(4, max_floors), 10);
                }

                poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 6);

                floorSegment.ZoneSteps.Add(poolSpawn);


                List<string> tutorElements = new List<string>() { "fighting", "bug", "fairy" };
                AddTutorZoneStep(floorSegment, new SpreadPlanQuota(new RandDecay(2, 3, 30), new IntRange(0, max_floors), true), new IntRange(5, 13), tutorElements);


                TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                tileSpawn.Priority = PR_RESPAWN_TRAP;


                tileSpawn.Spawns.Add(new EffectTile("trap_pp_leech", true), new IntRange(0, max_floors), 10);//pp-leech trap
                tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, max_floors), 10);//chestnut trap
                tileSpawn.Spawns.Add(new EffectTile("trap_spin", false), new IntRange(0, max_floors), 10);//spin trap
                tileSpawn.Spawns.Add(new EffectTile("trap_grimy", false), new IntRange(0, max_floors), 10);//grimy trap
                tileSpawn.Spawns.Add(new EffectTile("trap_sticky", false), new IntRange(0, max_floors), 10);//sticky trap
                tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap

                floorSegment.ZoneSteps.Add(tileSpawn);



                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 8), new IntRange(0, max_floors)), new MapItem("food_grimy"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("berry_leppa"), new MapItem(new InvItem("berry_leppa", true)));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)),
                    new MapItem("apricorn_brown"), new MapItem("apricorn_purple"), new MapItem("apricorn_red"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(3, max_floors)), new MapItem("machine_assembly_box"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(max_floors / 2, max_floors - 1), new IntRange(0, max_floors)), new MapItem("orb_cleanse"));

                {
                    //monster houses
                    SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(20, new IntRange(0, max_floors)));
                    monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHouseStep<ListMapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                    foreach (string gummi in IterateGummis())
                        monsterChanceZoneStep.Items.Add(new MapItem(gummi), new IntRange(0, max_floors), 4);//gummis
                    foreach (string iter_item in IterateApricorns())
                        monsterChanceZoneStep.Items.Add(new MapItem(iter_item), new IntRange(0, max_floors), 4);//apricorns
                    foreach (string iter_item in IterateTypePlates())
                        monsterChanceZoneStep.Items.Add(new MapItem(iter_item), new IntRange(0, max_floors), 5);//type plates

                    monsterChanceZoneStep.Items.Add(new MapItem("ammo_silver_spike", 6), new IntRange(0, max_floors), 30);//silver spike
                    monsterChanceZoneStep.Items.Add(new MapItem("loot_nugget"), new IntRange(0, max_floors), 10);//nugget
                    monsterChanceZoneStep.Items.Add(new MapItem("food_banana"), new IntRange(0, max_floors), 30);//banana
                    monsterChanceZoneStep.Items.Add(new MapItem("food_apple_big"), new IntRange(0, max_floors), 50);//big apple
                    monsterChanceZoneStep.Items.Add(new MapItem("food_banana_big"), new IntRange(0, max_floors), 10);//big banana
                    monsterChanceZoneStep.Items.Add(new MapItem("loot_heart_scale", 2), new IntRange(0, max_floors), 10);//heart scale
                    monsterChanceZoneStep.Items.Add(new MapItem("key", 1), new IntRange(0, max_floors), 10);//key
                    monsterChanceZoneStep.Items.Add(new MapItem("machine_recall_box"), new IntRange(0, max_floors), 10);//link box

                    monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(1, 4), "loot_pearl"), new ItemThemeNone(40, new RandRange(2, 4))), new IntRange(0, max_floors), 20);//no theme

                    monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(3, 7)), new IntRange(0, max_floors), 30);//gummis
                    monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(RecruitState)), true, true, new RandRange(2, 6)), new IntRange(0, 10), 10);//apricorns
                    monsterChanceZoneStep.MobThemes.Add(new MobThemeNone(40, new RandRange(7, 13)), new IntRange(0, max_floors), 10);
                    floorSegment.ZoneSteps.Add(monsterChanceZoneStep);
                }



                {
                    SpreadHouseZoneStep chestChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanQuota(new RandDecay(0, 8, 70), new IntRange(4, max_floors)));
                    chestChanceZoneStep.ModStates.Add(new FlagType(typeof(ChestModGenState)));
                    chestChanceZoneStep.HouseStepSpawns.Add(new ChestStep<ListMapGenContext>(false, GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);

                    foreach (string key in IterateVitamins())
                        chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//boosters
                    foreach (string key in IterateGummis())
                        chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//gummis
                    chestChanceZoneStep.Items.Add(new MapItem("apricorn_big"), new IntRange(0, max_floors), 20);//big apricorn
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_elixir"), new IntRange(0, max_floors), 80);//elixir
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_potion"), new IntRange(0, max_floors), 40);//potion
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_max_elixir"), new IntRange(0, max_floors), 20);//max elixir
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_max_potion"), new IntRange(0, max_floors), 20);//max potion
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_full_heal"), new IntRange(0, max_floors), 20);//full heal
                    foreach (string key in IterateXItems())
                        chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 15);//X-Items
                    chestChanceZoneStep.Items.Add(new MapItem("loot_nugget"), new IntRange(0, max_floors), 20);//nugget
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear", 1), new IntRange(0, max_floors), 20);//amber tear
                    chestChanceZoneStep.Items.Add(new MapItem("seed_joy"), new IntRange(0, max_floors), 15);//joy seed
                    chestChanceZoneStep.Items.Add(new MapItem("machine_ability_capsule"), new IntRange(0, max_floors), 15);//ability capsule

                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeNone(100, new RandRange(1, 3)), new IntRange(0, max_floors), 30);

                    floorSegment.ZoneSteps.Add(chestChanceZoneStep);
                }

                AddHiddenStairStep(floorSegment, new SpreadPlanQuota(new RandRange(1), new IntRange(0, max_floors - 1)), 2);

                AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 3);

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

                    SpawnList<PatternPlan> patternList = new SpawnList<PatternPlan>();
                    patternList.Add(new PatternPlan("pattern_teeth", PatternPlan.PatternExtend.Extrapolate), 5);
                    patternList.Add(new PatternPlan("pattern_squiggle", PatternPlan.PatternExtend.Repeat1D), 5);
                    patternList.Add(new PatternPlan("pattern_slash", PatternPlan.PatternExtend.Repeat2D), 5);
                    patternList.Add(new PatternPlan("pattern_crosshair", PatternPlan.PatternExtend.Extrapolate), 5);
                    AddTrapPatternSteps(layout, new RandRange(1, 4), patternList);

                    AddTrapsSteps(layout, new RandRange(10, 14));

                    //money
                    AddMoneyData(layout, new RandRange(3, 7));

                    //enemies
                    AddRespawnData(layout, 15, 110);
                    AddEnemySpawnData(layout, 20, new RandRange(9, 12));

                    //items
                    AddItemData(layout, new RandRange(3, 7), 25);

                    //construct paths
                    if (ii < 4)
                        AddInitGridStep(layout, 6, 4, 7, 6, 1, true);
                    else if (ii >= 8 && ii < 12)
                        AddInitGridStep(layout, 7, 5, 6, 7, 1, true);
                    else
                        AddInitGridStep(layout, 7, 5, 7, 6, 1, true);

                    GridPathTiered<MapGenContext> path = new GridPathTiered<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.TierAxis = Axis4.Horiz;
                    path.TierConnections = new RandRange(1, 3);

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //bump
                    genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(3, 7), new RandRange(3, 7), new RandRange(40, 70)), 10);
                    //blocked
                    if (ii >= 8 && ii < 12)
                    {
                        genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 7), new RandRange(4, 6), new RandRange(2, 5), new RandRange(2, 5)), 3);
                        genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 7), new RandRange(4, 6), new RandRange(2, 5), new RandRange(2, 5)), 3);
                    }
                    else
                    {
                        genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile("water"), new RandRange(4, 7), new RandRange(4, 7), new RandRange(8, 9), new RandRange(1, 3)), 3);
                        genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile("water"), new RandRange(4, 7), new RandRange(4, 7), new RandRange(1, 3), new RandRange(8, 9)), 3);
                    }
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(20), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    if (ii >= 8 && ii < 12)
                        layout.GenSteps.Add(PR_GRID_GEN, new SetGridDefaultsStep<MapGenContext>(new RandRange(50), GetImmutableFilterList()));
                    else
                        layout.GenSteps.Add(PR_GRID_GEN, new SetGridDefaultsStep<MapGenContext>(new RandRange(25), GetImmutableFilterList()));

                    {
                        CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(6, 8), GetImmutableFilterList());
                        step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenCave<MapGenContext>(new RandRange(9, 13), new RandRange(5))), 10);
                        if (ii < 8)
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenCave<MapGenContext>(new RandRange(5, 7), new RandRange(9, 13))), 10);
                        else if (ii >= 8 && ii < 12)
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(3, 1), new RoomGenCave<MapGenContext>(new RandRange(15, 19), new RandRange(4, 6))), 10);
                        else
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(3, 1), new RoomGenCave<MapGenContext>(new RandRange(15, 19), new RandRange(5, 7))), 10);
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }

                    layout.GenSteps.Add(PR_ROOMS_INIT, new DrawGridToFloorStep<MapGenContext>());
                    layout.GenSteps.Add(PR_TILES_INIT, new DrawFloorToTileStep<MapGenContext>());

                    //Add the stairs up and down
                    AddStairStep(layout, false);


                    if (ii == 9)
                    {
                        //vault rooms
                        {
                            SpawnList<RoomGen<MapGenContext>> detourRooms = new SpawnList<RoomGen<MapGenContext>>();
                            detourRooms.Add(new RoomGenSquare<MapGenContext>(new RandRange(1), new RandRange(1)), 10);
                            SpawnList<PermissiveRoomGen<MapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            detourHalls.Add(new RoomGenAngledHall<MapGenContext>(0, new RandRange(1), new RandRange(1)), 10);
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
                            SwitchSealStep<MapGenContext> vaultStep = new SwitchSealStep<MapGenContext>("sealed_block", "tile_switch_sync", 2, true, false);
                            vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                            vaultStep.SwitchFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            vaultStep.SwitchFilters.Add(new RoomFilterComponent(true, new BossRoom()));
                            layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                        }

                        //vault treasures
                        {
                            BulkSpawner<MapGenContext, EffectTile> treasures = new BulkSpawner<MapGenContext, EffectTile>();

                            EffectTile secretStairs = new EffectTile("stairs_secret_down", false);
                            secretStairs.TileStates.Set(new DestState(new SegLoc(1, 0)));
                            treasures.SpecificSpawns.Add(secretStairs);

                            RandomRoomSpawnStep<MapGenContext, EffectTile> detourItems = new RandomRoomSpawnStep<MapGenContext, EffectTile>(treasures);
                            detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                            layout.GenSteps.Add(PR_EXITS_DETOUR, detourItems);
                        }
                    }

                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                    if (ii == 9)
                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectTileStep<MapGenContext>("stairs_secret_down"));

                    floorSegment.Floors.Add(layout);
                }


                {
                    LoadGen layout = new LoadGen();
                    MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                    startGen.MapID = "end_veiled_ridge";
                    layout.GenSteps.Add(PR_FILE_LOAD, startGen);

                    MapTimeLimitStep<MapLoadContext> floorData = new MapTimeLimitStep<MapLoadContext>(600);
                    layout.GenSteps.Add(PR_FLOOR_DATA, floorData);

                    AddTextureData(layout, "rock_path_tds_wall", "rock_path_tds_floor", "rock_path_tds_secondary", "dark");

                    {
                        HashSet<string> exceptFor = new HashSet<string>();
                        foreach (string legend in IterateLegendaries())
                            exceptFor.Add(legend);
                        SpeciesItemElementSpawner<MapLoadContext> spawn = new SpeciesItemElementSpawner<MapLoadContext>(new IntRange(2), new RandRange(1), "dark", exceptFor);
                        BoxSpawner<MapLoadContext> box = new BoxSpawner<MapLoadContext>("box_heavy", spawn);
                        List<Loc> treasureLocs = new List<Loc>();
                        treasureLocs.Add(new Loc(7, 2));
                        layout.GenSteps.Add(PR_SPAWN_ITEMS, new SpecificSpawnStep<MapLoadContext, MapItem>(box, treasureLocs));
                    }

                    floorSegment.Floors.Add(layout);
                }

                zone.Segments.Add(floorSegment);
            }


            {
                int max_floors = 6;
                LayeredSegment floorSegment = new LayeredSegment();
                floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Illusion Ridge\nB{0}F"), new Priority(-15)));

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
                {
                    TeamMemberSpawn mob = GetTeamMob("absol", "", "future_sight", "night_slash", "", "", new RandRange(33), "wander_dumb", true);
                    MobSpawnItem keySpawn = new MobSpawnItem(true);
                    keySpawn.Items.Add(new InvItem("gummi_black", false, 1), 10);
                    mob.Spawn.SpawnFeatures.Add(keySpawn);
                    poolSpawn.Spawns.Add(mob, new IntRange(0, max_floors), 5);
                }
                poolSpawn.Spawns.Add(GetTeamMob("mightyena", "", "thief", "scary_face", "", "", new RandRange(30), "thief"), new IntRange(0, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("pawniard", "", "torment", "slash", "", "", new RandRange(30), "wander_dumb"), new IntRange(0, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("masquerain", "", "ominous_wind", "stun_spore", "", "", new RandRange(30), "wander_dumb"), new IntRange(0, max_floors), 10);
                {
                    TeamMemberSpawn mob = GetTeamMob("masquerain", "", "ominous_wind", "stun_spore", "", "", new RandRange(30), "wander_dumb", true);
                    MobSpawnItem keySpawn = new MobSpawnItem(true);
                    keySpawn.Items.Add(new InvItem("gummi_green", false, 1), 10);
                    mob.Spawn.SpawnFeatures.Add(keySpawn);
                    poolSpawn.Spawns.Add(mob, new IntRange(0, max_floors), 5);
                }
                poolSpawn.Spawns.Add(GetTeamMob("sneasel", "pickpocket", "agility", "quick_attack", "", "", new RandRange(33), "thief"), new IntRange(0, max_floors), 10);
                //spawns with pearl, if initial spawn, sleeping
                {
                    TeamMemberSpawn mob = GetTeamMob("grumpig", "", "magic_coat", "zen_headbutt", "", "", new RandRange(30), "wander_dumb", true);
                    MobSpawnItem keySpawn = new MobSpawnItem(true);
                    keySpawn.Items.Add(new InvItem("loot_pearl", false, 3), 10);
                    mob.Spawn.SpawnFeatures.Add(keySpawn);
                    poolSpawn.Spawns.Add(mob, new IntRange(0, max_floors), 5);
                }
                //spawns with sticky item
                {
                    TeamMemberSpawn mob = GetTeamMob("persian", "", "switcheroo", "feint_attack", "swift", "", new RandRange(33), "thief");
                    mob.Spawn.SpawnFeatures.Add(new MobSpawnItem(true, "held_toxic_orb", "held_flame_orb", "held_iron_ball", "held_ring_target", "held_choice_scarf"));
                    poolSpawn.Spawns.Add(mob, new IntRange(0, max_floors), 5);
                }
                //sleeping
                poolSpawn.Spawns.Add(GetTeamMob("zoroark", "", "night_daze", "u_turn", "agility", "night_slash", new RandRange(55), TeamMemberSpawn.MemberRole.Loner, "wander_normal", true), new IntRange(0, max_floors), 10);


                poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 6);

                floorSegment.ZoneSteps.Add(poolSpawn);


                List<string> tutorElements = new List<string>() { "fighting", "bug", "fairy" };
                AddTutorZoneStep(floorSegment, new SpreadPlanQuota(new RandDecay(0, 1, 70), new IntRange(0, max_floors), true), new IntRange(5, 13), tutorElements);


                TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                tileSpawn.Priority = PR_RESPAWN_TRAP;

                tileSpawn.Spawns.Add(new EffectTile("trap_pp_leech", true), new IntRange(0, max_floors), 10);//pp-leech trap
                tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, max_floors), 10);//chestnut trap
                tileSpawn.Spawns.Add(new EffectTile("trap_spin", false), new IntRange(0, max_floors), 10);//spin trap
                tileSpawn.Spawns.Add(new EffectTile("trap_grimy", false), new IntRange(0, max_floors), 10);//grimy trap
                tileSpawn.Spawns.Add(new EffectTile("trap_sticky", false), new IntRange(0, max_floors), 10);//sticky trap
                tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap

                floorSegment.ZoneSteps.Add(tileSpawn);

                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("berry_leppa"));

                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("machine_assembly_box"));

                AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 15, 3);

                for (int ii = 0; ii < max_floors; ii++)
                {
                    RoomFloorGen layout = new RoomFloorGen();

                    //Floor settings
                    AddFloorData(layout, "B12. Sickly Hollow.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                    //Tilesets
                    //other options: craggy_peak, sky_peak_summit_pass
                    AddTextureData(layout, "spacial_rift_2_wall", "spacial_rift_2_floor", "spacial_rift_2_secondary", "dark");

                    SpawnList<PatternPlan> terrainPattern = new SpawnList<PatternPlan>();
                    terrainPattern.Add(new PatternPlan("pattern_blob", PatternPlan.PatternExtend.Single), 5);
                    terrainPattern.Add(new PatternPlan("pattern_checker_large", PatternPlan.PatternExtend.Single), 5);
                    terrainPattern.Add(new PatternPlan("pattern_plus", PatternPlan.PatternExtend.Single), 5);
                    AddTerrainPatternSteps(layout, "wall", new RandRange(4, 8), terrainPattern);

                    AddWaterSteps(layout, "pit", new RandRange(10));//lava

                    //money
                    AddMoneyData(layout, new RandRange(3, 7));

                    //items
                    AddItemData(layout, new RandRange(3, 7), 25);

                    //enemies
                    AddRespawnData(layout, 15, 110);
                    AddEnemySpawnData(layout, 20, new RandRange(9, 12));

                    //traps
                    AddSingleTrapStep(layout, new RandRange(5, 8), "tile_wonder");//wonder tile

                    SpawnList<PatternPlan> patternList = new SpawnList<PatternPlan>();
                    patternList.Add(new PatternPlan("pattern_checker", PatternPlan.PatternExtend.Repeat1D), 5);
                    patternList.Add(new PatternPlan("pattern_x_repeat", PatternPlan.PatternExtend.Repeat2D), 5);
                    patternList.Add(new PatternPlan("pattern_crosshair", PatternPlan.PatternExtend.Extrapolate), 5);
                    AddTrapPatternSteps(layout, new RandRange(1, 4), patternList);

                    AddTrapsSteps(layout, new RandRange(6, 9));

                    //construct paths
                    AddInitListStep(layout, 58, 40, true);

                    {
                        //Create a path that is composed of a branching tree
                        FloorPathBranch<ListMapGenContext> path = new FloorPathBranch<ListMapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallPercent = 50;
                        path.FillPercent = new RandRange(65);
                        path.BranchRatio = new RandRange(30);

                        //Give it some room types to place
                        SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                        //cave
                        genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(6, 18), new RandRange(6, 18)), 10);

                        path.GenericRooms = genericRooms;

                        //Give it some hall types to place
                        SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(1), new RandRange(1)), 20);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_ROOMS_GEN, path);
                    }

                    //draw paths
                    layout.GenSteps.Add(PR_TILES_INIT, new DrawFloorToTileStep<ListMapGenContext>());


                    //Add the stairs up and down
                    AddStairStep(layout, true);

                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<ListMapGenContext, MapGenEntrance, MapGenExit>());

                    floorSegment.Floors.Add(layout);
                }

                zone.Segments.Add(floorSegment);
            }


            {
                SingularSegment structure = new SingularSegment(-1);

                SpawnList<TeamMemberSpawn> enemyList = new SpawnList<TeamMemberSpawn>();
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 14, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 15, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 16, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 17, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 18, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 19, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 20, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                structure.BaseFloor = getSecretRoom(translate, "special_gsc_ghost", -2, "dark_hill_2_wall", "dark_hill_2_floor", "dark_hill_2_secondary", "tall_grass_dark", "dark", DungeonStage.Intermediate, DungeonAccessibility.Unlockable, enemyList, new Loc(7, 6));

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
            #endregion
        }


        static void FillOvergrownWilds(ZoneData zone, bool translate)
        {
            #region OVERGROWN WILDS

            zone.Name = new LocalText("Overgrown Wilds");
            zone.Rescues = 2;
            zone.Level = 20;
            zone.ExpPercent = 80;
            zone.Rogue = RogueStatus.NoTransfer;

            {
                int max_floors = 12;
                LayeredSegment floorSegment = new LayeredSegment();
                floorSegment.IsRelevant = true;
                floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Overgrown Wilds\n{0}F"), new Priority(-15)));

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
                //sleeping
                {
                    TeamMemberSpawn mob = GetTeamMob("heracross", "", "horn_attack", "arm_thrust", "", "", new RandRange(22), "wander_normal", true);
                    MobSpawnItem itemSpawn = new MobSpawnItem(true);
                    foreach (string item_name in IterateGummis())
                        itemSpawn.Items.Add(new InvItem(item_name), 10);
                    mob.Spawn.SpawnFeatures.Add(itemSpawn);
                    poolSpawn.Spawns.Add(mob, new IntRange(4, max_floors), 5);
                }

                poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                floorSegment.ZoneSteps.Add(poolSpawn);


                List<string> tutorElements = new List<string>() { "fire", "poison", "bug" };
                AddTutorZoneStep(floorSegment, new SpreadPlanQuota(new RandDecay(1, 3, 60), new IntRange(0, max_floors), true), new IntRange(0, 5), tutorElements);


                TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                tileSpawn.Priority = PR_RESPAWN_TRAP;

                tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap
                tileSpawn.Spawns.Add(new EffectTile("trap_poison", false), new IntRange(0, max_floors), 10);//poison trap

                floorSegment.ZoneSteps.Add(tileSpawn);


                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 7), new IntRange(0, max_floors)), new MapItem("food_apple"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("berry_leppa"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(3, max_floors)), new MapItem("machine_assembly_box"));

                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)),
                    new MapItem("apricorn_green"), new MapItem("apricorn_purple"), new MapItem("apricorn_white"));

                //shops
                SpawnRangeList<IGenStep> shopZoneSpawns = new SpawnRangeList<IGenStep>();
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
                    foreach (string key in IterateEvoItems(EvoClass.Early))
                        shop.Items.Add(new MapItem(key, 0, 2500), 2);
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


                //switch vaults
                {
                    SpreadVaultZoneStep vaultChanceZoneStep = new SpreadVaultZoneStep(PR_SPAWN_ITEMS_EXTRA, PR_SPAWN_TRAPS, PR_SPAWN_MOBS_EXTRA, new SpreadPlanQuota(new RandDecay(1, 8, 50), new IntRange(0, max_floors)));

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
                        SwitchSealStep<ListMapGenContext> vaultStep = new SwitchSealStep<ListMapGenContext>("sealed_block", "tile_switch", 2, true, false);
                        vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                        vaultStep.SwitchFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        vaultStep.SwitchFilters.Add(new RoomFilterComponent(true, new BossRoom()));
                        vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_TILES_GEN_EXTRA, vaultStep));
                    }

                    PopulateVaultItems(vaultChanceZoneStep, DungeonStage.Beginner, DungeonAccessibility.Unlockable, max_floors, false);


                    floorSegment.ZoneSteps.Add(vaultChanceZoneStep);
                }


                AddHiddenStairStep(floorSegment, new SpreadPlanQuota(new RandRange(1), new IntRange(0, max_floors - 1)), 2);

                AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 3);


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
                        AddWaterSteps(layout, "water", new RandRange(12));//water
                    else if (ii < 8)
                        AddWaterSteps(layout, "water", new RandRange(20));//water

                    if (ii < 4)
                        AddGrassSteps(layout, new RandRange(3, 7), new IntRange(2, 6), new RandRange(20));
                    else
                        AddGrassSteps(layout, new RandRange(4, 8), new IntRange(4, 11), new RandRange(40));

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                    AddTrapsSteps(layout, new RandRange(6, 9));

                    //money
                    AddMoneyData(layout, new RandRange(2, 5));

                    //enemies
                    AddRespawnData(layout, 9, 90);
                    AddEnemySpawnData(layout, 20, new RandRange(6, 8));

                    //items
                    AddItemData(layout, new RandRange(2, 6), 25);

                    //construct paths
                    {
                        //prim maze with caves
                        AddInitGridStep(layout, 6, 6, 6, 6);

                        GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.RoomRatio = new RandRange(80);
                        path.BranchRatio = new RandRange(0, 35);

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //bump
                        genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(3, 7), new RandRange(3, 7), new RandRange(50)), 10);
                        //cave
                        genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(4, 7), new RandRange(4, 7)), 10);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_GRID_GEN, path);

                        {
                            AddTunnelStep<MapGenContext> tunneler = new AddTunnelStep<MapGenContext>();
                            tunneler.Halls = new RandRange(14, 20);
                            tunneler.TurnLength = new RandRange(3, 8);
                            tunneler.MaxLength = new RandRange(25);
                            layout.GenSteps.Add(PR_TILES_GEN_TUNNEL, tunneler);
                        }

                        {
                            CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(3, 7), GetImmutableFilterList());
                            step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            step.Filters.Add(new RoomFilterDefaultGen(true));
                            step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2), new RoomGenCave<MapGenContext>(new RandRange(10, 13), new RandRange(10, 13))), 10);
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenCave<MapGenContext>(new RandRange(7), new RandRange(10, 13))), 10);
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenCave<MapGenContext>(new RandRange(10, 13), new RandRange(7))), 10);
                            layout.GenSteps.Add(PR_GRID_GEN, step);
                        }

                    }

                    AddDrawGridSteps(layout);

                    AddStairStep(layout, false);

                    if (ii == 7)
                    {
                        //vault rooms
                        {
                            SpawnList<RoomGen<MapGenContext>> detourRooms = new SpawnList<RoomGen<MapGenContext>>();
                            RoomGenLoadMap<MapGenContext> loadRoom = new RoomGenLoadMap<MapGenContext>();
                            loadRoom.MapID = "room_exotic_wilds_entrance";
                            loadRoom.RoomTerrain = new Tile("grass");
                            loadRoom.PreventChanges = PostProcType.Panel | PostProcType.Terrain;
                            detourRooms.Add(loadRoom, 10);
                            SpawnList<PermissiveRoomGen<MapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            RoomGenAngledHall<MapGenContext> hall = new RoomGenAngledHall<MapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4));
                            hall.Brush = new TerrainHallBrush(Loc.One, new Tile("grass"));
                            detourHalls.Add(hall, 10);
                            AddConnectedRoomsStep<MapGenContext> detours = new AddConnectedRoomsStep<MapGenContext>(detourRooms, detourHalls);
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
                            TerrainSealStep<MapGenContext> vaultStep = new TerrainSealStep<MapGenContext>("grass", "wall");
                            vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BlockVault));
                            layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                        }
                    }

                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                    if (ii == 7)
                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectTileStep<MapGenContext>("stairs_secret_down"));

                    floorSegment.Floors.Add(layout);
                }


                {
                    LoadGen layout = new LoadGen();
                    MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                    startGen.MapID = "end_overgrown_wilds";
                    layout.GenSteps.Add(PR_FILE_LOAD, startGen);

                    MapTimeLimitStep<MapLoadContext> floorData = new MapTimeLimitStep<MapLoadContext>(600);
                    layout.GenSteps.Add(PR_FLOOR_DATA, floorData);

                    AddSpecificTextureData(layout, "southern_jungle_wall", "southern_jungle_floor", "southern_jungle_secondary", "tall_grass_dark", "grass");

                    {
                        HashSet<string> exceptFor = new HashSet<string>();
                        foreach (string legend in IterateLegendaries())
                            exceptFor.Add(legend);
                        SpeciesItemElementSpawner<MapLoadContext> spawn = new SpeciesItemElementSpawner<MapLoadContext>(new IntRange(2), new RandRange(1), "grass", exceptFor);
                        BoxSpawner<MapLoadContext> box = new BoxSpawner<MapLoadContext>("box_heavy", spawn);
                        List<Loc> treasureLocs = new List<Loc>();
                        treasureLocs.Add(new Loc(10, 5));
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
                floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Exotic Wilds\nB{0}F"), new Priority(-15)));

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
                necessities.Spawns.Add(new InvItem("food_banana"), new IntRange(0, max_floors), 3);
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
                poolSpawn.Spawns.Add(GetTeamMob("seedot", "", "harden", "nature_power", "", "", new RandRange(17), "wander_dumb"), new IntRange(0, max_floors), 10);

                poolSpawn.Spawns.Add(GetTeamMob("pineco", "", "bug_bite", "take_down", "", "", new RandRange(21), "turret"), new IntRange(0, max_floors), 10);

                poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                floorSegment.ZoneSteps.Add(poolSpawn);

                List<string> tutorElements = new List<string>() { "fire", "poison", "bug" };
                AddTutorZoneStep(floorSegment, new SpreadPlanQuota(new RandDecay(0, 1, 60), new IntRange(0, max_floors), true), new IntRange(0, 5), tutorElements);


                TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                tileSpawn.Priority = PR_RESPAWN_TRAP;

                tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap
                tileSpawn.Spawns.Add(new EffectTile("trap_poison", false), new IntRange(0, max_floors), 10);//poison trap
                tileSpawn.Spawns.Add(new EffectTile("trap_apple", true), new IntRange(0, max_floors), 3);//apple trap

                floorSegment.ZoneSteps.Add(tileSpawn);


                {
                    MobSpawn mob = GetGuardMob("leafeon", "", "leaf_blade", "aerial_ace", "x_scissor", "synthesis", new RandRange(40), "wander_normal", "sleep");
                    MobSpawnItem keySpawn = new MobSpawnItem(true);
                    keySpawn.Items.Add(new InvItem("held_scope_lens"), 10);
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
                    SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(20, new IntRange(0, max_floors)));
                    monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHouseStep<ListMapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                    foreach (string gummi in IterateGummis())
                        monsterChanceZoneStep.Items.Add(new MapItem(gummi), new IntRange(0, max_floors), 4);//gummis
                    foreach (string iter_item in IterateApricorns())
                        monsterChanceZoneStep.Items.Add(new MapItem(iter_item), new IntRange(0, max_floors), 4);//apricorns
                    monsterChanceZoneStep.Items.Add(new MapItem("evo_sun_ribbon"), new IntRange(0, max_floors), 50);
                    monsterChanceZoneStep.Items.Add(new MapItem("evo_kings_rock"), new IntRange(0, max_floors), 50);
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

                AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 15, 3);


                for (int ii = 0; ii < max_floors; ii++)
                {
                    RoomFloorGen layout = new RoomFloorGen();

                    //Floor settings
                    AddFloorData(layout, "B22. Overgrown Wilds.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                    //Tilesets
                    AddSpecificTextureData(layout, "mystery_jungle_1_wall", "mystery_jungle_1_floor", "mystery_jungle_1_secondary", "tall_grass_dark", "bug");

                    AddWaterSteps(layout, "water", new RandRange(20));//water

                    AddGrassSteps(layout, new RandRange(4, 8), new IntRange(4, 11), new RandRange(20));

                    //money
                    AddMoneyData(layout, new RandRange(1, 4));

                    //items
                    AddItemData(layout, new RandRange(2, 6), 25);

                    //enemies
                    AddRespawnData(layout, 9, 90);
                    AddEnemySpawnData(layout, 20, new RandRange(6, 8));

                    //traps
                    AddSingleTrapStep(layout, new RandRange(5, 8), "tile_wonder");//wonder tile
                    AddTrapsSteps(layout, new RandRange(6, 9));


                    //construct paths
                    {
                        AddInitListStep(layout, 72, 54);

                        FloorPathBranch<ListMapGenContext> path = new FloorPathBranch<ListMapGenContext>();

                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.FillPercent = new RandRange(45);
                        path.HallPercent = 100;
                        path.BranchRatio = new RandRange(0, 25);

                        SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                        //bump
                        genericRooms.Add(new RoomGenBump<ListMapGenContext>(new RandRange(4, 8), new RandRange(4, 8), new RandRange(50)), 10);
                        //cave
                        genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(4, 8), new RandRange(4, 8)), 10);
                        genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(8, 14), new RandRange(8, 14)), 4);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<ListMapGenContext>(100, new RandRange(4, 8), new RandRange(4, 8)), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_ROOMS_GEN, path);


                        AddTunnelStep<ListMapGenContext> tunneler = new AddTunnelStep<ListMapGenContext>();
                        tunneler.Halls = new RandRange(14, 20);
                        tunneler.TurnLength = new RandRange(3, 8);
                        tunneler.MaxLength = new RandRange(25);
                        layout.GenSteps.Add(PR_TILES_GEN_TUNNEL, tunneler);
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
                //enemyList.Add(GetTeamMob(new MonsterID("vivillon", 4, "", Gender.Unknown), "", "poison_powder", "psybeam", "powder", "struggle_bug", new RandRange(zone.Level)), 10);
                structure.BaseFloor = getSecretRoom(translate, "special_gsc_plant", -2, "mystery_jungle_1_wall", "mystery_jungle_1_floor", "mystery_jungle_1_secondary", "tall_grass_dark", "grass", DungeonStage.Beginner, DungeonAccessibility.Unlockable, enemyList, new Loc(9, 5));

                zone.Segments.Add(structure);
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


        static void FillTricksterWoods(ZoneData zone, bool translate)
        {
            #region TRICKSTER WOODS
            {
                zone.Name = new LocalText("Trickster Woods");
                zone.Rescues = 2;
                zone.Level = 20;
                zone.TeamSize = 1;
                zone.Rogue = RogueStatus.NoTransfer;

                {
                    int max_floors = 10;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Trickster Woods\n{0}F"), new Priority(-15)));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 0);
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;


                    //fakes
                    CategorySpawn<InvItem> fakes = new CategorySpawn<InvItem>();
                    fakes.SpawnRates.SetRange(6, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("fakes", fakes);

                    fakes.Spawns.Add(InvItem.CreateBox("food_apple", "applin"), new IntRange(4, max_floors), 3);


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
                    poolSpawn.Spawns.Add(GetTeamMob("paras", "", "leech_life", "", "", "", new RandRange(15), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("luxio", "", "spark", "charge", "", "", new RandRange(18), "wander_dumb"), new IntRange(5, max_floors), 5);
                    poolSpawn.Spawns.Add(GetTeamMob("slowpoke", "", "curse", "tackle", "", "", new RandRange(19), "wander_dumb"), new IntRange(0, 5), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("rattata", "", "hyper_fang", "", "", "", new RandRange(16), "wander_dumb"), new IntRange(0, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("mankey", "", "seismic_toss", "", "", "", new RandRange(17), "wander_dumb"), new IntRange(5, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("sandshrew", "", "rapid_spin", "", "", "", new RandRange(17), "wander_dumb"), new IntRange(5, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("bidoof", "unaware", "defense_curl", "tackle", "", "", new RandRange(16), "wander_dumb"), new IntRange(0, 5), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("poochyena", "", "howl", "bite", "", "", new RandRange(14), "wander_dumb"), new IntRange(5, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("abra", "", "teleport", "", "", "", new RandRange(12), "wander_normal"), new IntRange(5, max_floors), 10);

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);


                    List<string> tutorElements = new List<string>() { "fighting", "flying", "electric" };
                    AddTutorZoneStep(floorSegment, new SpreadPlanQuota(new RandDecay(1, 2, 30), new IntRange(0, max_floors), true), new IntRange(0, 5), tutorElements);


                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;

                    tileSpawn.Spawns.Add(new EffectTile("trap_mud", true), new IntRange(0, max_floors), 10);//mud trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", true), new IntRange(0, max_floors), 10);//chestnut trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_poison", true), new IntRange(0, max_floors), 10);//poison trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_self_destruct", true), new IntRange(0, max_floors), 10);//selfdestruct trap

                    tileSpawn.Spawns.Add(new EffectTile("trap_mud", false), new IntRange(5, max_floors), 10);//mud trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(5, max_floors), 10);//chestnut trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_poison", false), new IntRange(5, max_floors), 10);//poison trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_self_destruct", false), new IntRange(5, max_floors), 10);//selfdestruct trap


                    floorSegment.ZoneSteps.Add(tileSpawn);

                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("food_apple"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(3, 6), new IntRange(0, max_floors)), new MapItem("berry_leppa"));

                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)),
                        new MapItem("apricorn_green"), new MapItem("apricorn_brown"), new MapItem("apricorn_purple"),
                        new MapItem("apricorn_red"), new MapItem("apricorn_white"), new MapItem("apricorn_yellow"), new MapItem("apricorn_black"));

                    //switch vaults
                    {
                        SpreadVaultZoneStep vaultChanceZoneStep = new SpreadVaultZoneStep(PR_SPAWN_ITEMS_EXTRA, PR_SPAWN_TRAPS, PR_SPAWN_MOBS_EXTRA, new SpreadPlanQuota(new RandDecay(1, 8, 35), new IntRange(0, max_floors)));

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

                        PopulateVaultItems(vaultChanceZoneStep, DungeonStage.Beginner, DungeonAccessibility.Unlockable, max_floors, false);

                        floorSegment.ZoneSteps.Add(vaultChanceZoneStep);
                    }


                    AddHiddenStairStep(floorSegment, new SpreadPlanQuota(new RandRange(1), new IntRange(0, max_floors - 1)), 2);

                    AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 3);


                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        AddFloorData(layout, "B25. Trickster Woods.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Clear);

                        Dictionary<ItemFake, MobSpawn> spawnTable = new Dictionary<ItemFake, MobSpawn>();
                        spawnTable.Add(new ItemFake("food_apple", "applin"), GetGenericMob("applin", "", "astonish", "withdraw", "", "", new RandRange(20)));
                        AddFloorFakeItems(layout, spawnTable);

                        //Tilesets
                        //other candidates: purity_forest_6_wall,uproar_forest
                        if (ii < 5)
                            AddTextureData(layout, "pitfall_valley_1_wall", "pitfall_valley_1_floor", "pitfall_valley_1_secondary", "normal");
                        else
                            AddTextureData(layout, "grass_maze_wall", "grass_maze_floor", "grass_maze_secondary", "normal");

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile

                        SpawnList<PatternPlan> patternList = new SpawnList<PatternPlan>();
                        if (ii < 5)
                            patternList.Add(new PatternPlan("pattern_slash", PatternPlan.PatternExtend.Extrapolate), 10);
                        patternList.Add(new PatternPlan("pattern_squiggle", PatternPlan.PatternExtend.Repeat1D), 10);
                        if (ii >= 5)
                        {
                            patternList.Add(new PatternPlan("pattern_crosshair", PatternPlan.PatternExtend.Extrapolate), 5);
                            patternList.Add(new PatternPlan("pattern_x_repeat", PatternPlan.PatternExtend.Repeat2D), 5);
                        }
                        AddTrapPatternSteps(layout, new RandRange(3, 6), patternList);

                        AddTrapsSteps(layout, new RandRange(4, 7));

                        //money
                        AddMoneyData(layout, new RandRange(2, 5));

                        //enemies

                        if (ii >= 5)
                        {
                            //063 Abra : 100 Teleport
                            //always holds a TM
                            MobSpawn mob = GetGenericMob("abra", "", "teleport", "", "", "", new RandRange(12));
                            MobSpawnItem keySpawn = new MobSpawnItem(true);
                            keySpawn.Items.Add(new InvItem("tm_secret_power"), 10);//TM Secret Power
                            keySpawn.Items.Add(new InvItem("tm_hidden_power"), 10);//TM Hidden Power
                            keySpawn.Items.Add(new InvItem("tm_protect"), 10);//TM Protect
                            keySpawn.Items.Add(new InvItem("tm_double_team"), 10);//TM Double Team
                            keySpawn.Items.Add(new InvItem("tm_attract"), 10);//TM Attract
                            keySpawn.Items.Add(new InvItem("tm_captivate"), 10);//TM Captivate
                            keySpawn.Items.Add(new InvItem("tm_fling"), 10);//TM Fling
                            keySpawn.Items.Add(new InvItem("tm_taunt"), 10);//TM Taunt
                            mob.SpawnFeatures.Add(keySpawn);
                            SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                            specificTeam.Spawns.Add(mob);

                            LoopedTeamSpawner<MapGenContext> spawner = new LoopedTeamSpawner<MapGenContext>(specificTeam);
                            {
                                spawner.AmountSpawner = new RandDecay(0, 2, 30);
                            }
                            PlaceRandomMobsStep<MapGenContext> secretMobPlacement = new PlaceRandomMobsStep<MapGenContext>(spawner);
                            layout.GenSteps.Add(PR_SPAWN_MOBS, secretMobPlacement);
                        }

                        AddRespawnData(layout, 7, 90);
                        AddEnemySpawnData(layout, 20, new RandRange(4, 7));


                        //items
                        AddItemData(layout, new RandRange(2, 5), 25);


                        //construct paths
                        if (ii < 5)
                        {
                            //prim maze with caves
                            AddInitGridStep(layout, 4, 3, 12, 12);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(100);
                            path.BranchRatio = new RandRange(0, 35);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(5, 12), new RandRange(5, 12), new RandRange(4, 6), new RandRange(4, 6)), 10);
                            //round
                            genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(6, 11), new RandRange(6, 11)), 10);
                            //blocked
                            genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(6, 9), new RandRange(6, 9), new RandRange(1, 5), new RandRange(1, 5)), 5);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(50, 50));

                            {
                                CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(1, 4), GetImmutableFilterList());
                                step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                                step.Filters.Add(new RoomFilterDefaultGen(true));
                                step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenRound<MapGenContext>(new RandRange(7), new RandRange(10, 15))), 10);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(5, 7), new RandRange(10, 15), new RandRange(1, 3), new RandRange(3, 7))), 10);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenRound<MapGenContext>(new RandRange(10, 15), new RandRange(7))), 10);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(10, 15), new RandRange(5, 7), new RandRange(3, 7), new RandRange(1, 3))), 10);
                                layout.GenSteps.Add(PR_GRID_GEN, step);
                            }
                        }
                        else
                        {
                            //prim maze with caves
                            AddInitGridStep(layout, 5, 5, 7, 7);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(80);
                            path.BranchRatio = new RandRange(0, 35);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(5, 7), new RandRange(5, 7), new RandRange(2, 5), new RandRange(2, 5)), 10);
                            //blocked
                            genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(4, 7), new RandRange(4, 7), new RandRange(1, 3), new RandRange(1, 3)), 5);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));

                            {
                                CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(8, 12), GetImmutableFilterList());
                                step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                                step.Filters.Add(new RoomFilterDefaultGen(true));
                                step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2), new RoomGenRound<MapGenContext>(new RandRange(10, 15), new RandRange(10, 15))), 10);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(10, 15), new RandRange(10, 15), new RandRange(3, 7), new RandRange(3, 7))), 10);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(5, 7), new RandRange(10, 15), new RandRange(1, 3), new RandRange(3, 7))), 10);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenRound<MapGenContext>(new RandRange(10, 15), new RandRange(7))), 10);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(10, 15), new RandRange(5, 7), new RandRange(3, 7), new RandRange(1, 3))), 10);
                                layout.GenSteps.Add(PR_GRID_GEN, step);
                            }

                            if (ii == 5)
                            {
                                SetGridSpecialRoomStep<MapGenContext> specialStep = new SetGridSpecialRoomStep<MapGenContext>();
                                specialStep.Filters.Add(new RoomFilterComponent(true, new ImmutableRoom()));
                                specialStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));

                                RoomGenLoadMap<MapGenContext> loadRoom = new RoomGenLoadMap<MapGenContext>();
                                loadRoom.MapID = "room_fairy_ring";
                                loadRoom.RoomTerrain = new Tile("floor");
                                loadRoom.PreventChanges = PostProcType.Panel | PostProcType.Terrain;

                                specialStep.Rooms = new PresetPicker<RoomGen<MapGenContext>>(loadRoom);
                                specialStep.RoomComponents.Set(new ImmutableRoom());
                                layout.GenSteps.Add(PR_GRID_GEN_EXTRA, specialStep);
                            }
                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, false);

                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        if (ii == 5)
                            layout.GenSteps.Add(PR_DBG_CHECK, new DetectTileStep<MapGenContext>("tile_fairy_ring"));

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }


                {
                    int max_floors = 4;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Trickster Maze\nB{0}F"), new Priority(-15)));

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
                    necessities.Spawns.Add(InvItem.CreateBox("food_apple", "applin"), new IntRange(0, max_floors), 5);//Apple (fake)

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
                    throwable.Spawns.Add(new InvItem("wand_vanish", false, 2), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_whirlwind", false, 2), new IntRange(0, max_floors), 10);
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
                    held.SpawnRates.SetRange(4, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("held", held);


                    held.Spawns.Add(new InvItem("held_power_band"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_defense_scarf"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_special_band"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_zinc_band"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_spell_tag"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_silk_scarf"), new IntRange(0, max_floors), 10);
                    held.Spawns.Add(new InvItem("held_goggle_specs"), new IntRange(0, max_floors), 10);


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
                    {
                        TeamMemberSpawn teamSpawn = GetTeamMob(new MonsterID("deerling", 0, "", Gender.Unknown), "", "take_down", "leech_seed", "", "", new RandRange(19), "wander_dumb");
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(0, 4));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 10);
                    }
                    {
                        TeamMemberSpawn teamSpawn = GetTeamMob(new MonsterID("deerling", 1, "", Gender.Unknown), "", "take_down", "leech_seed", "", "", new RandRange(19), "wander_dumb");
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(1, 4));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 10);
                    }
                    {
                        TeamMemberSpawn teamSpawn = GetTeamMob(new MonsterID("deerling", 2, "", Gender.Unknown), "", "take_down", "leech_seed", "", "", new RandRange(19), "wander_dumb");
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(2, 4));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 10);
                    }
                    {
                        TeamMemberSpawn teamSpawn = GetTeamMob(new MonsterID("deerling", 3, "", Gender.Unknown), "", "take_down", "leech_seed", "", "", new RandRange(19), "wander_dumb");
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(3, 4));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 10);
                    }

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);


                    List<string> tutorElements = new List<string>() { "fighting", "flying", "electric" };
                    AddTutorZoneStep(floorSegment, new SpreadPlanQuota(new RandDecay(0, 1, 60), new IntRange(0, max_floors), true), new IntRange(0, 5), tutorElements);


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


                    AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 15, 3);


                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        AddFloorData(layout, "B01. Demonstration.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                        Dictionary<ItemFake, MobSpawn> spawnTable = new Dictionary<ItemFake, MobSpawn>();
                        spawnTable.Add(new ItemFake("food_apple", "applin"), GetGenericMob("applin", "", "astonish", "withdraw", "", "", new RandRange(20)));
                        AddFloorFakeItems(layout, spawnTable);

                        //Tilesets
                        AddTextureData(layout, "purity_forest_8_wall", "purity_forest_8_floor", "purity_forest_8_secondary", "normal");

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
                        patternList.Add(new PatternPlan("pattern_checker", PatternPlan.PatternExtend.Repeat1D), 10);
                        patternList.Add(new PatternPlan("pattern_crosshair", PatternPlan.PatternExtend.Extrapolate), 5);
                        patternList.Add(new PatternPlan("pattern_x_repeat", PatternPlan.PatternExtend.Repeat2D), 5);
                        AddTrapPatternSteps(layout, new RandRange(2, 5), patternList);

                        AddTrapsSteps(layout, new RandRange(16, 19));

                        //construct paths
                        {
                            //prim maze with caves
                            AddInitGridStep(layout, 5, 5, 7, 7, 1, true);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(100);
                            path.BranchRatio = new RandRange(0, 35);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(5, 7), new RandRange(5, 7), new RandRange(2, 5), new RandRange(2, 5)), 10);
                            //blocked
                            genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(4, 7), new RandRange(4, 7), new RandRange(1, 3), new RandRange(1, 3)), 5);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));

                            {
                                CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(5, 9), GetImmutableFilterList());
                                step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                                step.Filters.Add(new RoomFilterDefaultGen(true));
                                step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2), new RoomGenRound<MapGenContext>(new RandRange(10, 15), new RandRange(10, 15))), 10);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(10, 15), new RandRange(10, 15), new RandRange(3, 7), new RandRange(3, 7))), 10);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(5, 7), new RandRange(10, 15), new RandRange(1, 3), new RandRange(3, 7))), 10);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenRound<MapGenContext>(new RandRange(10, 15), new RandRange(7))), 10);
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(10, 15), new RandRange(5, 7), new RandRange(3, 7), new RandRange(1, 3))), 10);
                                layout.GenSteps.Add(PR_GRID_GEN, step);
                            }
                        }

                        layout.GenSteps.Add(PR_ROOMS_INIT, new DrawGridToFloorStep<MapGenContext>());
                        layout.GenSteps.Add(PR_TILES_INIT, new DrawFloorToTileStep<MapGenContext>());

                        //Add the stairs up and down
                        AddStairStep(layout, true);


                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }

                {
                    SingularSegment structure = new SingularSegment(-1);

                    SpawnList<TeamMemberSpawn> enemyList = new SpawnList<TeamMemberSpawn>();
                    structure.BaseFloor = getSecretRoom(translate, "special_gsc_plant", -2, "purity_forest_8_wall", "purity_forest_8_floor", "purity_forest_8_secondary", "tall_grass", "normal", DungeonStage.Beginner, DungeonAccessibility.Unlockable, enemyList, new Loc(9, 5));

                    zone.Segments.Add(structure);
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


                {
                    SingularSegment structure = new SingularSegment(-1);

                    ChanceFloorGen multiGen = new ChanceFloorGen();
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "small_square", -3, false, false), 10);
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "tall_hall", -3, false, false), 10);
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "wide_hall", -3, false, false), 10);
                    structure.BaseFloor = multiGen;

                    zone.Segments.Add(structure);
                }
            }
            #endregion
        }


        static void FillMoonlitCourtyard(ZoneData zone, bool translate)
        {
            #region MOONLIT COURTYARD
            {
                zone.Name = new LocalText("Moonlit Courtyard");
                zone.Rescues = 4;
                zone.Level = 25;
                zone.ExpPercent = 55;
                zone.BagRestrict = 16;
                zone.Rogue = RogueStatus.NoTransfer;

                {
                    int max_floors = 14;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Moonlit Courtyard\n{0}F"), new Priority(-15)));

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


                    poolSpawn.Spawns.Add(GetTeamMob("clefairy", "cute_charm", "follow_me", "disarming_voice", "", "", new RandRange(23), TeamMemberSpawn.MemberRole.Support, "wander_dumb"), new IntRange(0, 10), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("gloom", "", "mega_drain", "moonlight", "", "", new RandRange(24), "wander_dumb"), new IntRange(5, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("cutiefly", "", "draining_kiss", "struggle_bug", "", "", new RandRange(22), "wander_dumb"), new IntRange(0, 5), 10);

                    poolSpawn.Spawns.Add(GetTeamMob("mime_jr", "", "copycat", "encore", "", "", new RandRange(23), "wander_dumb"), new IntRange(0, 5), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("roselia", "", "magical_leaf", "", "", "", new RandRange(22), "wander_dumb"), new IntRange(0, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("bayleef", "", "magical_leaf", "synthesis", "", "", new RandRange(22), "wander_dumb"), new IntRange(5, max_floors), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("lunatone", "", "embargo", "moonblast", "", "", new RandRange(23), "wander_dumb"), new IntRange(5, max_floors), 10);

                    {
                        TeamMemberSpawn teamSpawn = GetTeamMob(new MonsterID("floette", 0, "", Gender.Unknown), "", "grassy_terrain", "wish", "", "", new RandRange(19), TeamMemberSpawn.MemberRole.Support, "wander_dumb");
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(0, 5));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(5, max_floors), 10);
                    }
                    {
                        TeamMemberSpawn teamSpawn = GetTeamMob(new MonsterID("floette", 1, "", Gender.Unknown), "", "grassy_terrain", "wish", "", "", new RandRange(19), TeamMemberSpawn.MemberRole.Support, "wander_dumb");
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(1, 5));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(5, max_floors), 10);
                    }
                    {
                        TeamMemberSpawn teamSpawn = GetTeamMob(new MonsterID("floette", 2, "", Gender.Unknown), "", "grassy_terrain", "wish", "", "", new RandRange(19), TeamMemberSpawn.MemberRole.Support, "wander_dumb");
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(2, 5));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(5, max_floors), 10);
                    }
                    {
                        TeamMemberSpawn teamSpawn = GetTeamMob(new MonsterID("floette", 3, "", Gender.Unknown), "", "grassy_terrain", "wish", "", "", new RandRange(19), TeamMemberSpawn.MemberRole.Support, "wander_dumb");
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(3, 5));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(5, max_floors), 10);
                    }
                    {
                        TeamMemberSpawn teamSpawn = GetTeamMob(new MonsterID("floette", 4, "", Gender.Unknown), "", "grassy_terrain", "wish", "", "", new RandRange(19), TeamMemberSpawn.MemberRole.Support, "wander_dumb");
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(4, 5));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(5, max_floors), 10);
                    }

                    poolSpawn.SpecificSpawns.Add(new SpecificTeamSpawner(GetGenericMob("flabebe", "", "fairy_wind", "razor_leaf", "", "", new RandRange(18), "turret"), GetGenericMob("flabebe", "", "fairy_wind", "razor_leaf", "", "", new RandRange(18), "turret"), GetGenericMob("flabebe", "", "fairy_wind", "razor_leaf", "", "", new RandRange(18), "turret")), new IntRange(0, 10), 10);
                    poolSpawn.SpecificSpawns.Add(new SpecificTeamSpawner(GetGenericMob(new MonsterID("flabebe", 1, "", Gender.Unknown), "", "fairy_wind", "razor_leaf", "", "", new RandRange(18), "turret"), GetGenericMob(new MonsterID("flabebe", 1, "", Gender.Unknown), "", "fairy_wind", "razor_leaf", "", "", new RandRange(18), "turret"), GetGenericMob(new MonsterID("flabebe", 1, "", Gender.Unknown), "", "fairy_wind", "razor_leaf", "", "", new RandRange(18), "turret")), new IntRange(0, 10), 10);
                    poolSpawn.SpecificSpawns.Add(new SpecificTeamSpawner(GetGenericMob(new MonsterID("flabebe", 2, "", Gender.Unknown), "", "fairy_wind", "razor_leaf", "", "", new RandRange(18), "turret"), GetGenericMob(new MonsterID("flabebe", 2, "", Gender.Unknown), "", "fairy_wind", "razor_leaf", "", "", new RandRange(18), "turret"), GetGenericMob(new MonsterID("flabebe", 2, "", Gender.Unknown), "", "fairy_wind", "razor_leaf", "", "", new RandRange(18), "turret")), new IntRange(0, 10), 10);
                    poolSpawn.SpecificSpawns.Add(new SpecificTeamSpawner(GetGenericMob(new MonsterID("flabebe", 3, "", Gender.Unknown), "", "fairy_wind", "razor_leaf", "", "", new RandRange(18), "turret"), GetGenericMob(new MonsterID("flabebe", 3, "", Gender.Unknown), "", "fairy_wind", "razor_leaf", "", "", new RandRange(18), "turret"), GetGenericMob(new MonsterID("flabebe", 3, "", Gender.Unknown), "", "fairy_wind", "razor_leaf", "", "", new RandRange(18), "turret")), new IntRange(0, 10), 10);
                    poolSpawn.SpecificSpawns.Add(new SpecificTeamSpawner(GetGenericMob(new MonsterID("flabebe", 4, "", Gender.Unknown), "", "fairy_wind", "razor_leaf", "", "", new RandRange(18), "turret"), GetGenericMob(new MonsterID("flabebe", 4, "", Gender.Unknown), "", "fairy_wind", "razor_leaf", "", "", new RandRange(18), "turret"), GetGenericMob(new MonsterID("flabebe", 4, "", Gender.Unknown), "", "fairy_wind", "razor_leaf", "", "", new RandRange(18), "turret")), new IntRange(0, 10), 10);

                    {

                        MobSpawn mob = GetGuardMob("umbreon", "", "moonlight", "confuse_ray", "assurance", "toxic", new RandRange(40), "wander_normal", "sleep");
                        MobSpawnItem keySpawn = new MobSpawnItem(true);
                        keySpawn.Items.Add(new InvItem("held_pass_scarf"), 10);
                        mob.SpawnFeatures.Add(keySpawn);

                        SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                        specificTeam.Spawns.Add(mob);
                        LoopedTeamSpawner<ListMapGenContext> spawner = new LoopedTeamSpawner<ListMapGenContext>(specificTeam, new RandRange(1));

                        SpawnRangeList<IGenStep> specialEnemySpawns = new SpawnRangeList<IGenStep>();
                        specialEnemySpawns.Add(new PlaceRandomMobsStep<ListMapGenContext>(spawner), new IntRange(0, max_floors), 10);
                        SpreadStepRangeZoneStep specialEnemyStep = new SpreadStepRangeZoneStep(new SpreadPlanQuota(new RandDecay(0, 1, 50), new IntRange(0, max_floors - 1)), PR_SPAWN_MOBS, specialEnemySpawns);
                        floorSegment.ZoneSteps.Add(specialEnemyStep);
                    }

                    //version exclusives
                    {
                        TeamMemberSpawn teamSpawn = GetTeamMob("volbeat", "", "flash", "struggle_bug", "", "", new RandRange(24), "wander_dumb");
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(0, 2));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(5, max_floors), 10);
                    }
                    {
                        TeamMemberSpawn teamSpawn = GetTeamMob("illumise", "", "wish", "struggle_bug", "", "", new RandRange(24), "wander_dumb");
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(1, 2));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(5, max_floors), 10);
                    }

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 3);

                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;


                    tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_slumber", false), new IntRange(0, max_floors), 10);//sleep trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_seal", false), new IntRange(0, max_floors), 10);//seal trap

                    floorSegment.ZoneSteps.Add(tileSpawn);


                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(3, 8), new IntRange(0, max_floors)), new MapItem("food_apple"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("berry_leppa"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(3, max_floors)), new MapItem("machine_assembly_box"));

                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)),
                        new MapItem("apricorn_green"), new MapItem("apricorn_purple"), new MapItem("apricorn_white"));

                    SpawnRangeList<IGenStep> shopZoneSpawns = new SpawnRangeList<IGenStep>();
                    {
                        ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                        shop.Personality = 2;
                        shop.SecurityStatus = "shop_security";

                        foreach (string tm_id in IterateTMs(TMClass.Mid | TMClass.ShopOnly))
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


                    List<string> tutorElements = new List<string>() { "fire", "steel", "poison" };
                    AddTutorZoneStep(floorSegment, new SpreadPlanQuota(new RandDecay(1, 3, 60), new IntRange(0, max_floors), true), new IntRange(0, 5), tutorElements);


                    //key vault
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

                            PopulateVaultItems(detourChanceZoneStep, DungeonStage.Beginner, DungeonAccessibility.Hidden, max_floors, true, true);

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
                                vaultChanceZoneStep.Items.Add(new MapItem("evo_lunar_ribbon"), new IntRange(0, max_floors), 50);
                                vaultChanceZoneStep.Items.Add(new MapItem("evo_kings_rock"), new IntRange(0, max_floors), 50);
                                vaultChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear", 1), new IntRange(0, max_floors), 100);//amber tear
                                vaultChanceZoneStep.Items.Add(new MapItem("seed_reviver"), new IntRange(0, max_floors), 200);//reviver seed
                                vaultChanceZoneStep.Items.Add(new MapItem("seed_pure"), new IntRange(0, max_floors), 100);//pure seed
                                vaultChanceZoneStep.Items.Add(new MapItem("machine_recall_box"), new IntRange(0, max_floors), 200);//recall box
                            }

                            // item spawnings for the vault
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

                            combinedVaultZoneStep.Steps.Add(vaultChanceZoneStep);
                        }

                        floorSegment.ZoneSteps.Add(combinedVaultZoneStep);
                    }

                    {
                        SpreadRoomZoneStep danceZoneStep = new SpreadRoomZoneStep(PR_GRID_GEN_EXTRA, PR_ROOMS_GEN_EXTRA, new SpreadPlanQuota(new RandRange(0, 2), new IntRange(10, max_floors)));
                        List<BaseRoomFilter> danceFilters = new List<BaseRoomFilter>();
                        danceFilters.Add(new RoomFilterComponent(true, new ImmutableRoom()));
                        danceFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        RoomGenLoadMap<MapGenContext> loadMap = new RoomGenLoadMap<MapGenContext>();
                        loadMap.MapID = "room_moon_dance";
                        loadMap.RoomTerrain = new Tile("floor");
                        loadMap.PreventChanges = PostProcType.Panel | PostProcType.Terrain;
                        danceZoneStep.Spawns.Add(new RoomGenOption(loadMap, new RoomGenDefault<ListMapGenContext>(), danceFilters), 10);
                        floorSegment.ZoneSteps.Add(danceZoneStep);
                    }

                    AddEvoZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 5), new IntRange(1, max_floors)), true);

                    AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 3);

                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        if (ii < 5)
                            AddFloorData(layout, "Star Cave.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);
                        else
                            AddFloorData(layout, "Star Cave.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                        if (ii == 7)
                            AddDefaultMapStatus(layout, "default_weather", "misty_terrain");
                        else if (ii > 7)
                            AddDefaultMapStatus(layout, "default_weather", "misty_terrain", "clear", "clear", "clear");

                        //Tilesets
                        if (ii < 5)
                            AddSpecificTextureData(layout, "moonlit_courtyard_wall", "moonlit_courtyard_floor", "moonlit_courtyard_secondary", "tall_grass_blue", "fairy");
                        else if (ii < 10)
                            AddSpecificTextureData(layout, "moonlit_courtyard_2_wall", "moonlit_courtyard_2_floor", "moonlit_courtyard_secondary", "tall_grass_blue", "fairy");
                        else
                            AddSpecificTextureData(layout, "moonlit_courtyard_3_wall", "moonlit_courtyard_floor", "moonlit_courtyard_secondary", "tall_grass_blue", "fairy");

                        //grass
                        BlobWaterStep<MapGenContext> coverStep = new BlobWaterStep<MapGenContext>(new RandRange(2, 6), new Tile("grass"), new MapTerrainStencil<MapGenContext>(true, false, false, false), new DefaultBlobStencil<MapGenContext>(), new IntRange(4, 9), new IntRange(6, 15));
                        layout.GenSteps.Add(PR_WATER, coverStep);

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(8, 12));

                        //money
                        AddMoneyData(layout, new RandRange(2, 5));

                        //enemies
                        AddRespawnData(layout, 7, 90);
                        AddEnemySpawnData(layout, 20, new RandRange(5, 7));


                        if (ii >= 1 && ii < 6)
                        {
                            //280 Ralts : 100 Teleport
                            //always holds a key
                            MobSpawn mob = GetGenericMob("ralts", "", "teleport", "growl", "", "", new RandRange(18));
                            MobSpawnItem keySpawn = new MobSpawnItem(true);
                            keySpawn.Items.Add(new InvItem("key", false, 1), 10);
                            mob.SpawnFeatures.Add(keySpawn);

                            SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                            specificTeam.Spawns.Add(mob);

                            LoopedTeamSpawner<MapGenContext> spawner = new LoopedTeamSpawner<MapGenContext>(specificTeam);
                            {
                                spawner.AmountSpawner = new RandDecay(0, 3, 70);
                            }
                            PlaceRandomMobsStep<MapGenContext> secretMobPlacement = new PlaceRandomMobsStep<MapGenContext>(spawner);
                            layout.GenSteps.Add(PR_SPAWN_MOBS, secretMobPlacement);
                        }

                        //items
                        AddItemData(layout, new RandRange(3, 6), 25);

                        //construct paths
                        if (ii < 5 || ii >= 10)
                        {
                            //Initialize a grid of cells.
                            AddInitGridStep(layout, 10, 9, 2, 2, 2);

                            //Create a path that is composed of a branching tree
                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(100);
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
                                ConnectGridBranchStep<MapGenContext> step = new ConnectGridBranchStep<MapGenContext>(70);
                                if (ii >= 10)
                                    step.ConnectPercent = 50;
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

                            //Combine some rooms for large rooms
                            {
                                CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(3), GetImmutableFilterList());
                                step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                                step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2), new RoomGenSquare<MapGenContext>(new RandRange(6), new RandRange(6))), 10);
                                layout.GenSteps.Add(PR_GRID_GEN, step);
                            }
                        }
                        else
                        {

                            //Initialize a grid of cells.
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
                                ConnectGridBranchStep<MapGenContext> step = new ConnectGridBranchStep<MapGenContext>(70);
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

                            {
                                AddLargeRoomStep<MapGenContext> step = new AddLargeRoomStep<MapGenContext>(new RandRange(2, 7), GetImmutableFilterList());
                                step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                                step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                {
                                    RoomGenLoadMap<MapGenContext> loadRoom = new RoomGenLoadMap<MapGenContext>();
                                    loadRoom.MapID = "room_garden_cross";
                                    loadRoom.RoomTerrain = new Tile("floor");
                                    LargeRoom<MapGenContext> largeRoom = new LargeRoom<MapGenContext>(loadRoom, new Loc(3), 2);
                                    largeRoom.OpenBorders[(int)Dir4.Down][1] = true;
                                    largeRoom.OpenBorders[(int)Dir4.Left][1] = true;
                                    largeRoom.OpenBorders[(int)Dir4.Up][1] = true;
                                    largeRoom.OpenBorders[(int)Dir4.Right][1] = true;
                                    step.GiantRooms.Add(largeRoom, 10);
                                }
                                {
                                    RoomGenLoadMap<MapGenContext> loadRoom = new RoomGenLoadMap<MapGenContext>();
                                    loadRoom.MapID = "room_garden_center_cross";
                                    loadRoom.RoomTerrain = new Tile("floor");
                                    LargeRoom<MapGenContext> largeRoom = new LargeRoom<MapGenContext>(loadRoom, new Loc(3), 2);
                                    largeRoom.OpenBorders[(int)Dir4.Down][1] = true;
                                    largeRoom.OpenBorders[(int)Dir4.Left][1] = true;
                                    largeRoom.OpenBorders[(int)Dir4.Up][1] = true;
                                    largeRoom.OpenBorders[(int)Dir4.Right][1] = true;
                                    step.GiantRooms.Add(largeRoom, 10);
                                }
                                layout.GenSteps.Add(PR_GRID_GEN, step);
                            }
                            //Combine some rooms for large rooms
                            {
                                CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(3), GetImmutableFilterList());
                                step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                                step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2), new RoomGenSquare<MapGenContext>(new RandRange(5), new RandRange(5))), 10);
                                layout.GenSteps.Add(PR_GRID_GEN, step);
                            }
                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, false);


                        if (ii == 7)
                        {
                            EffectTile secretTile = new EffectTile("stairs_secret_up", false);
                            secretTile.TileStates.Set(new DestState(new SegLoc(1, 0)));
                            RandomSpawnStep<MapGenContext, EffectTile> trapStep = new RandomSpawnStep<MapGenContext, EffectTile>(new PickerSpawner<MapGenContext, EffectTile>(new PresetMultiRand<EffectTile>(secretTile)));
                            layout.GenSteps.Add(PR_SPAWN_TRAPS, trapStep);
                        }

                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        if (ii == 7)
                            //if (false)
                            layout.GenSteps.Add(PR_DBG_CHECK, new DetectTileStep<MapGenContext>("stairs_secret_up"));

                        floorSegment.Floors.Add(layout);
                    }


                    {
                        LoadGen layout = new LoadGen();
                        MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                        startGen.MapID = "end_moonlit_courtyard";
                        layout.GenSteps.Add(PR_FILE_LOAD, startGen);

                        MapTimeLimitStep<MapLoadContext> floorData = new MapTimeLimitStep<MapLoadContext>(600);
                        layout.GenSteps.Add(PR_FLOOR_DATA, floorData);

                        AddSpecificTextureData(layout, "moonlit_courtyard_2_wall", "moonlit_courtyard_floor", "moonlit_courtyard_secondary", "tall_grass_blue", "fairy");

                        {
                            HashSet<string> exceptFor = new HashSet<string>();
                            foreach (string legend in IterateLegendaries())
                                exceptFor.Add(legend);
                            SpeciesItemElementSpawner<MapLoadContext> spawn = new SpeciesItemElementSpawner<MapLoadContext>(new IntRange(2), new RandRange(1), "fairy", exceptFor);
                            BoxSpawner<MapLoadContext> box = new BoxSpawner<MapLoadContext>("box_heavy", spawn);
                            List<Loc> treasureLocs = new List<Loc>();
                            treasureLocs.Add(new Loc(7, 8));
                            layout.GenSteps.Add(PR_SPAWN_ITEMS, new SpecificSpawnStep<MapLoadContext, MapItem>(box, treasureLocs));
                        }

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }


                {
                    int max_floors = 6;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Moonlit Temple\n{0}F"), new Priority(-15)));

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
                    {
                        TeamMemberSpawn teamSpawn = GetTeamMob(new MonsterID("floette", 0, "", Gender.Unknown), "", "grassy_terrain", "wish", "", "", new RandRange(24), TeamMemberSpawn.MemberRole.Support, "wander_dumb");
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(0, 5));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 10);
                    }
                    {
                        TeamMemberSpawn teamSpawn = GetTeamMob(new MonsterID("floette", 1, "", Gender.Unknown), "", "grassy_terrain", "wish", "", "", new RandRange(24), TeamMemberSpawn.MemberRole.Support, "wander_dumb");
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(1, 5));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 10);
                    }
                    {
                        TeamMemberSpawn teamSpawn = GetTeamMob(new MonsterID("floette", 2, "", Gender.Unknown), "", "grassy_terrain", "wish", "", "", new RandRange(24), TeamMemberSpawn.MemberRole.Support, "wander_dumb");
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(2, 5));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 10);
                    }
                    {
                        TeamMemberSpawn teamSpawn = GetTeamMob(new MonsterID("floette", 3, "", Gender.Unknown), "", "grassy_terrain", "wish", "", "", new RandRange(24), TeamMemberSpawn.MemberRole.Support, "wander_dumb");
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(3, 5));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 10);
                    }
                    {
                        TeamMemberSpawn teamSpawn = GetTeamMob(new MonsterID("floette", 4, "", Gender.Unknown), "", "grassy_terrain", "wish", "", "", new RandRange(19), TeamMemberSpawn.MemberRole.Support, "wander_dumb");
                        teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(4, 5));
                        poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 10);
                    }

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 3);

                    floorSegment.ZoneSteps.Add(poolSpawn);


                    List<string> tutorElements = new List<string>() { "fire", "steel", "poison" };
                    AddTutorZoneStep(floorSegment, new SpreadPlanQuota(new RandDecay(0, 1, 60), new IntRange(0, max_floors), true), new IntRange(0, 5), tutorElements);


                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;

                    tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_slumber", false), new IntRange(0, max_floors), 10);//sleep trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_seal", false), new IntRange(0, max_floors), 10);//seal trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_warp", true), new IntRange(0, max_floors), 10);//warp trap


                    floorSegment.ZoneSteps.Add(tileSpawn);

                    {
                        MobSpawn mob = GetGuardMob("sylveon", "pixilate", "moonblast", "shadow_ball", "swift", "", new RandRange(50), "wander_normal", "sleep");
                        MobSpawnItem keySpawn = new MobSpawnItem(true);
                        keySpawn.Items.Add(new InvItem("held_assault_vest"), 10);
                        mob.SpawnFeatures.Add(keySpawn);

                        SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                        specificTeam.Spawns.Add(mob);
                        LoopedTeamSpawner<ListMapGenContext> spawner = new LoopedTeamSpawner<ListMapGenContext>(specificTeam, new RandRange(1));

                        SpawnRangeList<IGenStep> specialEnemySpawns = new SpawnRangeList<IGenStep>();
                        specialEnemySpawns.Add(new PlaceRandomMobsStep<ListMapGenContext>(spawner), new IntRange(0, max_floors), 10);
                        SpreadStepRangeZoneStep specialEnemyStep = new SpreadStepRangeZoneStep(new SpreadPlanQuota(new RandDecay(0, 1, 50), new IntRange(0, max_floors - 1)), PR_SPAWN_MOBS, specialEnemySpawns);
                        floorSegment.ZoneSteps.Add(specialEnemyStep);
                    }

                    //switch vaults
                    {
                        SpreadVaultZoneStep vaultChanceZoneStep = new SpreadVaultZoneStep(PR_SPAWN_ITEMS_EXTRA, PR_SPAWN_TRAPS, PR_SPAWN_MOBS_EXTRA, new SpreadPlanQuota(new RandDecay(2, 4, 50), new IntRange(0, max_floors)));

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
                            SwitchSealStep<ListMapGenContext> vaultStep = new SwitchSealStep<ListMapGenContext>("sealed_block", "tile_switch", 2, true, false);
                            vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                            vaultStep.SwitchFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            vaultStep.SwitchFilters.Add(new RoomFilterComponent(true, new BossRoom()));
                            vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_TILES_GEN_EXTRA, vaultStep));
                        }

                        PopulateVaultItems(vaultChanceZoneStep, DungeonStage.Beginner, DungeonAccessibility.Hidden, max_floors, false);

                        floorSegment.ZoneSteps.Add(vaultChanceZoneStep);
                    }

                    AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 15, 3);

                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        AddFloorData(layout, "B09. Relic Tower.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                        //Tilesets
                        AddTextureData(layout, "wish_cave_1_wall", "wish_cave_1_floor", "wish_cave_1_secondary", "fairy");

                        //add water cracks
                        AddTunnelStep<MapGenContext> tunneler = new AddTunnelStep<MapGenContext>();
                        tunneler.Halls = new RandRange(3, 7);
                        tunneler.TurnLength = new RandRange(3, 8);
                        tunneler.MaxLength = new RandRange(25);
                        tunneler.Brush = new TerrainHallBrush(Loc.One, new Tile("water"));
                        layout.GenSteps.Add(PR_WATER, tunneler);


                        //add water at the borders
                        RoomTerrainStep<MapGenContext> trapStep = new RoomTerrainStep<MapGenContext>(new Tile("water"), new RandRange(2, 5), true, true);
                        trapStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));

                        MatchTerrainStencil<MapGenContext> terrainStencil = new MatchTerrainStencil<MapGenContext>(false, new Tile("wall"));
                        BorderTerrainStencil<MapGenContext> borderStencil = new BorderTerrainStencil<MapGenContext>(false, new Tile("floor"));
                        trapStep.TerrainStencil = new MultiTerrainStencil<MapGenContext>(false, terrainStencil, borderStencil);
                        layout.GenSteps.Add(PR_WATER, trapStep);

                        //money
                        AddMoneyData(layout, new RandRange(2, 4));

                        //items
                        AddItemData(layout, new RandRange(3, 5), 25);

                        //enemies
                        AddRespawnData(layout, 7, 90);
                        AddEnemySpawnData(layout, 20, new RandRange(5, 7));

                        //traps
                        AddSingleTrapStep(layout, new RandRange(5, 8), "tile_wonder");//wonder tile
                        AddTrapsSteps(layout, new RandRange(6, 9));

                        //construct paths
                        {
                            AddInitGridStep(layout, 5, 5, 7, 7);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(100);
                            path.BranchRatio = new RandRange(0, 25);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //square
                            genericRooms.Add(new RoomGenSquare<MapGenContext>(new RandRange(4, 7), new RandRange(4, 7)), 10);
                            //round
                            genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8)), 10);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(50, 50));

                            {
                                CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(1), GetImmutableFilterList());
                                step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                                step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));

                                RoomGenLoadMap<MapGenContext> loadRoom = new RoomGenLoadMap<MapGenContext>();
                                switch (ii)
                                {
                                    case 1:
                                        loadRoom.MapID = "room_moon_crescent_left";
                                        break;
                                    case 2:
                                        loadRoom.MapID = "room_moon_half_left";
                                        break;
                                    case 3:
                                        loadRoom.MapID = "room_moon_full";
                                        break;
                                    case 4:
                                        loadRoom.MapID = "room_moon_half_right";
                                        break;
                                    case 5:
                                        loadRoom.MapID = "room_moon_crescent_right";
                                        break;
                                    default:
                                        loadRoom.MapID = "room_moon_new";
                                        break;
                                }
                                loadRoom.RoomTerrain = new Tile("floor");

                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2), loadRoom), 10);
                                layout.GenSteps.Add(PR_GRID_GEN, step);
                            }
                        }

                        AddDrawGridSteps(layout);

                        {
                            EffectTile exitTile = new EffectTile("stairs_exit_down", true);
                            exitTile.TileStates.Set(new DestState(SegLoc.Invalid));
                            var step = new FloorStairsStep<MapGenContext, MapGenEntrance, MapGenExit>(new MapGenEntrance(Dir8.Down), new MapGenExit(exitTile));
                            step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            step.Filters.Add(new RoomFilterComponent(true, new BossRoom()));
                            layout.GenSteps.Add(PR_EXITS, step);
                        }

                        if (ii == 0)
                        {
                            EffectTile secretTile = new EffectTile("stairs_go_up", true);
                            RandomRoomSpawnStep<MapGenContext, MapGenExit> secretStep = new RandomRoomSpawnStep<MapGenContext, MapGenExit>(new PickerSpawner<MapGenContext, MapGenExit>(new PresetMultiRand<MapGenExit>(new MapGenExit(secretTile))));
                            secretStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            secretStep.Filters.Add(new RoomFilterComponent(true, new BossRoom()));
                            layout.GenSteps.Add(PR_EXITS, secretStep);
                        }
                        else if (ii < 5)
                        {
                            EffectTile secretTile = new EffectTile("stairs_go_up", false);
                            RandomRoomSpawnStep<MapGenContext, MapGenExit> secretStep = new RandomRoomSpawnStep<MapGenContext, MapGenExit>(new PickerSpawner<MapGenContext, MapGenExit>(new PresetMultiRand<MapGenExit>(new MapGenExit(secretTile))));
                            secretStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            secretStep.Filters.Add(new RoomFilterComponent(true, new BossRoom()));
                            layout.GenSteps.Add(PR_EXITS, secretStep);
                        }
                        else
                        {
                            {
                                SpawnList<RoomGen<MapGenContext>> bossRooms = new SpawnList<RoomGen<MapGenContext>>();
                                bossRooms.Add(getBossRoomGen<MapGenContext>("clefable", 24, 0, 1), 10);
                                bossRooms.Add(getBossRoomGen<MapGenContext>("gallade", 30, 0, 1), 10);
                                bossRooms.Add(getBossRoomGen<MapGenContext>("umbreon", 30, 0, 1), 10);
                                bossRooms.Add(getBossRoomGen<MapGenContext>("volbeat", 30, 0, 1), 10);
                                layout.GenSteps.Add(PR_ROOMS_GEN_EXTRA, CreateGenericBossRoomStep(bossRooms, 0));
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
                            {
                                BulkSpawner<MapGenContext, EffectTile> treasures = new BulkSpawner<MapGenContext, EffectTile>();

                                EffectTile secretStairs = new EffectTile("stairs_go_up", true);
                                treasures.SpecificSpawns.Add(secretStairs);

                                RandomRoomSpawnStep<MapGenContext, EffectTile> detourItems = new RandomRoomSpawnStep<MapGenContext, EffectTile>(treasures);
                                detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                                detourItems.Filters.Add(new RoomFilterIndex(false, 0));
                                layout.GenSteps.Add(PR_EXITS_DETOUR, detourItems);
                            }
                        }


                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }

                {
                    SingularSegment structure = new SingularSegment(-1);

                    SpawnList<TeamMemberSpawn> enemyList = new SpawnList<TeamMemberSpawn>();
                    enemyList.Add(GetTeamMob(new MonsterID("alcremie", 0, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                    enemyList.Add(GetTeamMob(new MonsterID("alcremie", 1, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                    enemyList.Add(GetTeamMob(new MonsterID("alcremie", 2, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                    enemyList.Add(GetTeamMob(new MonsterID("alcremie", 3, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                    enemyList.Add(GetTeamMob(new MonsterID("alcremie", 4, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                    enemyList.Add(GetTeamMob(new MonsterID("alcremie", 5, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                    enemyList.Add(GetTeamMob(new MonsterID("alcremie", 6, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                    structure.BaseFloor = getSecretRoom(translate, "special_rby_fairy", -2, "sky_peak_4th_pass_wall", "sky_peak_4th_pass_floor", "sky_peak_4th_pass_secondary", "tall_grass", "fairy", DungeonStage.Beginner, DungeonAccessibility.Hidden, enemyList, new Loc(13, 5));

                    zone.Segments.Add(structure);
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


                {
                    SingularSegment structure = new SingularSegment(-1);

                    ChanceFloorGen multiGen = new ChanceFloorGen();
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "small_square", -3, false, false), 10);
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "tall_hall", -3, false, false), 10);
                    multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Beginner, "wide_hall", -3, false, false), 10);
                    structure.BaseFloor = multiGen;

                    zone.Segments.Add(structure);
                }

                zone.GroundMaps.Add("end_moonlit_temple");
            }
            #endregion
        }


        static void FillForsakenDesert(ZoneData zone)
        {
            #region FORSAKEN DESERT
            {
                zone.Name = new LocalText("Forsaken Desert");
                zone.Rescues = 2;
                zone.Level = 30;
                zone.ExpPercent = 70;
                zone.Rogue = RogueStatus.NoTransfer;

                int max_floors = 4;
                LayeredSegment floorSegment = new LayeredSegment();
                floorSegment.IsRelevant = true;
                floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Forsaken Desert\n{0}F"), new Priority(-15)));


                //money
                MoneySpawnZoneStep moneySpawnZoneStep = new MoneySpawnZoneStep(PR_RESPAWN_MONEY, new RandRange(3200, 3300), new RandRange(1500, 1600));
                moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                //items
                ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;


                //necessities
                CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                necessities.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                itemSpawnZoneStep.Spawns.Add("necessities", necessities);

                necessities.Spawns.Add(new InvItem("food_grimy"), new IntRange(0, max_floors), 10);

                //snacks
                CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                snacks.Spawns.Add(new InvItem("seed_hunger"), new IntRange(0, max_floors), 10);
                snacks.Spawns.Add(new InvItem("seed_blinker"), new IntRange(0, max_floors), 10);
                snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, max_floors), 10);
                snacks.Spawns.Add(new InvItem("seed_plain"), new IntRange(0, max_floors), 10);
                snacks.Spawns.Add(new InvItem("seed_vile"), new IntRange(0, max_floors), 10);
                snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, max_floors), 10);
                snacks.Spawns.Add(new InvItem("seed_last_chance"), new IntRange(0, max_floors), 10);
                snacks.Spawns.Add(new InvItem("seed_decoy"), new IntRange(0, max_floors), 5);
                //special
                CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                special.SpawnRates.SetRange(3, new IntRange(0, max_floors));
                itemSpawnZoneStep.Spawns.Add("special", special);


                special.Spawns.Add(new InvItem("apricorn_brown"), new IntRange(0, max_floors), 10);
                special.Spawns.Add(new InvItem("apricorn_red"), new IntRange(0, max_floors), 10);
                special.Spawns.Add(new InvItem("apricorn_green"), new IntRange(0, max_floors), 10);
                special.Spawns.Add(new InvItem("machine_recall_box"), new IntRange(0, max_floors), 10);
                special.Spawns.Add(new InvItem("key", false, 1), new IntRange(0, max_floors), 5);
                //throwable
                CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
                itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                throwable.Spawns.Add(new InvItem("ammo_gravelerock", false, 3), new IntRange(0, max_floors), 10);
                throwable.Spawns.Add(new InvItem("ammo_geo_pebble", false, 3), new IntRange(0, max_floors), 10);
                throwable.Spawns.Add(new InvItem("ammo_cacnea_spike", false, 2), new IntRange(0, max_floors), 3);
                throwable.Spawns.Add(new InvItem("ammo_iron_thorn", false, 2), new IntRange(0, max_floors), 3);
                throwable.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, max_floors), 3);
                throwable.Spawns.Add(new InvItem("wand_lure", false, 3), new IntRange(0, max_floors), 3);
                throwable.Spawns.Add(new InvItem("wand_topsy_turvy", false, 2), new IntRange(0, max_floors), 3);
                throwable.Spawns.Add(new InvItem("wand_path", false, 1), new IntRange(0, max_floors), 3);

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
                held.Spawns.Add(new InvItem("held_twisted_spoon", true), new IntRange(0, max_floors), 3);
                held.Spawns.Add(new InvItem("held_twisted_spoon"), new IntRange(0, max_floors), 7);
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

                poolSpawn.Spawns.Add(GetTeamMob("cubone", "", "bone_club", "growl", "", "", new RandRange(23), "wait_and_see"), new IntRange(0, 3), 10);
                poolSpawn.Spawns.Add(GetTeamMob("rockruff", "", "odor_sleuth", "rock_throw", "", "", new RandRange(22), TeamMemberSpawn.MemberRole.Support, "wait_and_see"), new IntRange(0, 2), 10);
                poolSpawn.Spawns.Add(GetTeamMob("marowak", "", "bonemerang", "", "", "", new RandRange(28), "wait_and_see"), new IntRange(3, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("hippopotas", "", "sand_tomb", "dig", "", "", new RandRange(26), "wait_and_see"), new IntRange(0, 2), 10);
                poolSpawn.Spawns.Add(GetTeamMob("fearow", "", "drill_run", "pluck", "", "", new RandRange(24), "wait_and_see"), new IntRange(1, 3), 10);
                poolSpawn.Spawns.Add(GetTeamMob("sandslash", "", "magnitude", "sand_attack", "", "", new RandRange(25), "wait_and_see"), new IntRange(1, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("cacnea", "", "leech_seed", "needle_arm", "", "", new RandRange(24), "wait_and_see"), new IntRange(0, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("skorupi", "", "acupressure", "bug_bite", "", "", new RandRange(24), TeamMemberSpawn.MemberRole.Support, "wait_and_see"), new IntRange(2, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("torkoal", "drought", "smokescreen", "lava_plume", "", "", new RandRange(25), "wait_and_see"), new IntRange(0, 2), 10);
                poolSpawn.Spawns.Add(GetTeamMob("arbok", "", "screech", "glare", "crunch", "", new RandRange(25), "wait_and_see"), new IntRange(2, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("thievul", "run_away", "snarl", "assurance", "", "", new RandRange(25), "wait_and_see"), new IntRange(2, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("trapinch", "", "mud_slap", "bide", "", "", new RandRange(24), "wait_and_see"), new IntRange(1, max_floors), 5);
                poolSpawn.Spawns.Add(GetTeamMob("scraggy", "", "leer", "low_kick", "", "", new RandRange(24), "wait_and_see"), new IntRange(0, 2), 5);
                poolSpawn.Spawns.Add(GetTeamMob("gible", "", "sandstorm", "dragon_rage", "", "", new RandRange(25), "wait_and_see"), new IntRange(2, max_floors), 10);

                poolSpawn.Spawns.Add(GetTeamMob("rockruff", "", "odor_sleuth", "rock_throw", "", "", new RandRange(22), TeamMemberSpawn.MemberRole.Support, "wait_and_see"), new IntRange(0, 2), 10);

                {
                    TeamMemberSpawn teamSpawn = GetTeamMob(new MonsterID("lycanroc", 0, "", Gender.Unknown), "", "accelerock", "rock_throw", "", "", new RandRange(26), "wait_and_see");
                    teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(0, 2));
                    poolSpawn.Spawns.Add(teamSpawn, new IntRange(2, max_floors), 10);
                }
                {
                    TeamMemberSpawn teamSpawn = GetTeamMob(new MonsterID("lycanroc", 1, "", Gender.Unknown), "", "counter", "rock_throw", "", "", new RandRange(26), "wait_and_see");
                    teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(1, 2));
                    poolSpawn.Spawns.Add(teamSpawn, new IntRange(2, max_floors), 10);
                }

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

                    if (ii % 2 == 1)
                        AddDefaultMapStatus(layout, "default_weather", "sunny");

                    //Tilesets
                    if (ii < 2)
                        AddTextureData(layout, "desert_region_wall", "desert_region_floor", "desert_region_secondary", "ground");
                    else if (ii < 3)
                        AddTextureData(layout, "northern_desert_1_wall", "northern_desert_1_floor", "northern_desert_1_secondary", "ground");
                    else
                        AddTextureData(layout, "northern_desert_2_wall", "northern_desert_2_floor", "northern_desert_2_secondary", "ground");


                    //traps

                    //wonder tile
                    AddSingleTrapStep(layout, new RandRange(100 + ii * 30, 120 + ii * 30), "tile_wonder");//wonder tile

                    AddTrapsSteps(layout, new RandRange(300, 400));


                    {
                        SpawnList<EffectTile> effectTileSpawns = new SpawnList<EffectTile>();
                        effectTileSpawns.Add(new EffectTile("tile_compass", true), 10);
                        RandRange compassAmount;
                        if (ii == 0)
                            compassAmount = new RandRange(35);
                        else
                            compassAmount = new RandRange(80);
                        SpacedRoomSpawnStep<MapGenContext, EffectTile> trapStep = new SpacedRoomSpawnStep<MapGenContext, EffectTile>(new PickerSpawner<MapGenContext, EffectTile>(new LoopedRand<EffectTile>(effectTileSpawns, compassAmount)), false);
                        trapStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        layout.GenSteps.Add(PR_SPAWN_TRAPS, trapStep);
                    }

                    if (ii < 2)
                    {
                        EffectTile secretTile = new EffectTile("tile_compass", true);
                        NearSpawnableSpawnStep<MapGenContext, EffectTile, MapGenEntrance> trapStep = new NearSpawnableSpawnStep<MapGenContext, EffectTile, MapGenEntrance>(new PickerSpawner<MapGenContext, EffectTile>(new PresetMultiRand<EffectTile>(secretTile)), 100);
                        layout.GenSteps.Add(PR_SPAWN_TRAPS, trapStep);
                    }

                    //money - Ballpark 25K
                    if (ii == 0)
                        AddMoneyTrails(layout, new RandRange(12, 22), new IntRange(20, 120), new BoxSpawner<MapGenContext>("box_light", new SpeciesItemContextSpawner<MapGenContext>(new IntRange(1), new RandRange(3))));
                    else if (ii == 1)
                        AddMoneyTrails(layout, new RandRange(16, 30), new IntRange(20, 120), new BoxSpawner<MapGenContext>("box_light", new SpeciesItemContextSpawner<MapGenContext>(new IntRange(1), new RandRange(4))));
                    else if (ii == 2)
                        AddMoneyTrails(layout, new RandRange(16, 30), new IntRange(20, 120), new BoxSpawner<MapGenContext>("box_light", new SpeciesItemContextSpawner<MapGenContext>(new IntRange(1), new RandRange(5))));
                    else
                        AddMoneyTrails(layout, new RandRange(16, 30), new IntRange(20, 120), new BoxSpawner<MapGenContext>("box_light", new SpeciesItemContextSpawner<MapGenContext>(new IntRange(1), new RandRange(6))));

                    //enemies! ~ lv 18 to 32
                    if (ii == 0)
                    {
                        AddRadiusDespawnData(layout, 100, 90);
                        AddRadiusRespawnData(layout, 70, 100, 30);
                        AddRadiusEnemySpawnData(layout, 80, new RandRange(90));
                    }
                    else
                    {
                        AddRadiusDespawnData(layout, 90, 90);
                        AddRadiusRespawnData(layout, 60, 100, 30);
                        AddRadiusEnemySpawnData(layout, 80, new RandRange(90));
                    }

                    if (ii == 2)
                    {
                        PresetMultiTeamSpawner<MapGenContext> multiTeamSpawner = new PresetMultiTeamSpawner<MapGenContext>();
                        MobSpawn post_mob = new MobSpawn();
                        post_mob.BaseForm = new MonsterID("baltoy", 0, "normal", Gender.Genderless);
                        post_mob.Tactic = "wait_only";
                        post_mob.Level = new RandRange(25);
                        post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new NpcDialogueBattleEvent(new StringKey("TALK_ADVICE_PYRAMID"))));
                        SpecificTeamSpawner post_team = new SpecificTeamSpawner(post_mob);
                        post_team.Explorer = true;
                        multiTeamSpawner.Spawns.Add(post_team);
                        PlaceNearSpawnableMobsStep<MapGenContext, MapGenExit> randomSpawn = new PlaceNearSpawnableMobsStep<MapGenContext, MapGenExit>(multiTeamSpawner);
                        randomSpawn.Ally = true;
                        layout.GenSteps.Add(PR_SPAWN_MOBS_EXTRA, randomSpawn);
                    }

                    //items
                    {
                        RandRange itemRange = new RandRange(100 + ii * 50, 120 + ii * 50);
                        RandomRoomSpawnStep<MapGenContext, InvItem> itemStep = new RandomRoomSpawnStep<MapGenContext, InvItem>(new ContextSpawner<MapGenContext, InvItem>(itemRange), false);
                        itemStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        layout.GenSteps.Add(PR_SPAWN_ITEMS, itemStep);
                    }

                    //construct paths
                    {
                        if (ii == 0)
                            AddInitGridStep(layout, 32, 32, 8, 8);
                        else if (ii == 1)
                            AddInitGridStep(layout, 40, 40, 8, 8);
                        else
                            AddInitGridStep(layout, 50, 50, 8, 8);

                        //on the third floor, create a 200x200 tile structure made of a 23x23 cell megaroom.
                        //there are no entrances into the room except for the center, which connects straight down to the room below it
                        //then, 4 switches are placed at the exact corners of this structure, bulldozing out in their directions until they hit another walkable
                        //the main path must connect to this room

                        GridPathStartStepGeneric<MapGenContext> path;
                        if (ii != 2)
                        {
                            GridPathBranchSpread<MapGenContext> spreadPath = new GridPathBranchSpread<MapGenContext>();
                            spreadPath.RoomRatio = new RandRange(90);
                            spreadPath.BranchRatio = new RandRange(0, 35);
                            path = spreadPath;
                        }
                        else
                        {
                            GridPathPyramid<MapGenContext> spreadPath = new GridPathPyramid<MapGenContext>();

                            spreadPath.GiantHallSize = new Loc(20, 20);
                            spreadPath.LargeRoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.SwitchVault));
                            spreadPath.LargeRoomComponents.Set(new NoConnectRoom());
                            spreadPath.LargeRoomComponents.Set(new NoEventRoom());
                            RoomGenLoadMapBordered<MapGenContext> pyramid = new RoomGenLoadMapBordered<MapGenContext>();
                            pyramid.MapID = "room_pyramid";
                            pyramid.PreventChanges = PostProcType.Terrain | PostProcType.Panel;

                            int room_width = 179;
                            int room_height = 179;
                            pyramid.Borders[Dir4.Down] = new bool[room_width];
                            pyramid.Borders[Dir4.Up] = new bool[room_width];
                            pyramid.Borders[Dir4.Left] = new bool[room_width];
                            pyramid.Borders[Dir4.Right] = new bool[room_width];

                            pyramid.Borders[Dir4.Down][room_width / 2] = true;
                            pyramid.Borders[Dir4.Up][room_width / 2] = true;
                            pyramid.Borders[Dir4.Left][room_height / 2] = true;
                            pyramid.Borders[Dir4.Right][room_height / 2] = true;

                            spreadPath.GiantHallGen.Add(pyramid, 10);

                            spreadPath.CornerRoomComponents.Set(new CornerRoom());
                            spreadPath.CornerRoomComponents.Set(new ImmutableRoom());
                            spreadPath.CornerRoomComponents.Set(new NoEventRoom());

                            spreadPath.RoomRatio = new RandRange(90);
                            spreadPath.BranchRatio = new RandRange(0, 35);

                            path = spreadPath;


                            //sealing the vault
                            {
                                SwitchSealStep<MapGenContext> vaultStep = new SwitchSealStep<MapGenContext>("sealed_block", "tile_switch", 4, true, false);
                                vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                                vaultStep.SwitchFilters.Add(new RoomFilterComponent(false, new CornerRoom()));
                                vaultStep.SwitchFilters.Add(new RoomFilterComponent(true, new BossRoom()));
                                layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                            }
                        }

                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //cross
                        genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 11), new RandRange(4, 11), new RandRange(2, 6), new RandRange(2, 6)), 10);
                        //round
                        genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(4, 9), new RandRange(4, 9)), 10);
                        //blocked
                        genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(4, 9), new RandRange(4, 9), new RandRange(1, 3), new RandRange(1, 3)), 10);
                        //bump
                        genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 9), new RandRange(4, 9), new RandRange(30, 81)), 10);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(30), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_GRID_GEN, path);

                        layout.GenSteps.Add(PR_GRID_GEN, new SetGridDefaultsStep<MapGenContext>(new RandRange(20), GetImmutableFilterList()));

                        if (ii == 0)
                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(80, 30));
                        else
                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(150, 30));



                        {
                            //oases: 1, 1-2, 2-3, 2-3
                            RandRange amount;
                            if (ii < 1)
                                amount = new RandRange(1);
                            else if (ii < 2)
                                amount = new RandRange(1, 3);
                            else
                                amount = new RandRange(2, 4);
                            CombineGridRoomRandStep<MapGenContext> step = new CombineGridRoomRandStep<MapGenContext>(amount, GetImmutableFilterList());
                            step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            RoomGenOasis<MapGenContext> oasisGen = new RoomGenOasis<MapGenContext>(new RandRange(13, 18), new RandRange(13, 18));
                            oasisGen.WaterTerrain = new Tile("water");
                            oasisGen.ItemAmount = 1;
                            oasisGen.Treasures.Add(new MapItem("seed_joy"), 10);
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2), oasisGen), 5);
                            layout.GenSteps.Add(PR_GRID_GEN, step);
                        }

                        {
                            RandRange combines = new RandRange(300 + 150 * ii);
                            CombineGridRoomRandStep<MapGenContext> step = new CombineGridRoomRandStep<MapGenContext>(combines, GetImmutableFilterList());
                            step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenCave<MapGenContext>(new RandRange(6, 9), new RandRange(13, 18))), 10);
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenCave<MapGenContext>(new RandRange(13, 18), new RandRange(6, 9))), 10);
                            //step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 2), new RoomGenCave<MapGenContext>(new RandRange(13, 18), new RandRange(13, 18))), 10);
                            layout.GenSteps.Add(PR_GRID_GEN, step);
                        }

                    }

                    AddDrawGridSteps(layout);
                    {
                        IntRange range;
                        if (ii == 0)
                            range = new IntRange(72, 600);
                        else
                            range = new IntRange(120, 1000);
                        var step = new FloorStairsDistanceStep<MapGenContext, MapGenEntrance, MapGenExit>(range, new MapGenEntrance(Dir8.Down), new MapGenExit(new EffectTile("stairs_go_up", true)));
                        step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        step.Filters.Add(new RoomFilterComponent(true, new BossRoom()));
                        layout.GenSteps.Add(PR_EXITS, step);
                    }


                    //chest
                    int boss_amount = 0;
                    int chest_amount = 0;
                    if (ii == 1)
                        boss_amount = 1;
                    else if (ii == 2)
                        boss_amount = 2;
                    else if (ii == 3)
                    {
                        boss_amount = 3;
                        chest_amount = 1;
                    }

                    for (int kk = 0; kk < boss_amount; kk++)
                    {
                        {
                            SpawnList<RoomGen<MapGenContext>> bossRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //boss that only shows on second or final floor
                            if (ii == 1 || ii == 3 && kk == 0)
                            {
                                bossRooms.Add(getBossRoomGen<MapGenContext>("hippowdon", 23, 0, 1), 10);
                                bossRooms.Add(getBossRoomGen<MapGenContext>("thievul", 23, 0, 1), 10);
                                bossRooms.Add(getBossRoomGen<MapGenContext>("drapion", 23, 0, 1), 10);
                            }
                            //boss that only shows on third or final floor
                            if (ii == 2 || ii == 3 && kk > 0 && kk < 2)
                            {
                                bossRooms.Add(getBossRoomGen<MapGenContext>("armaldo", 23, 0, 1), 10);
                                bossRooms.Add(getBossRoomGen<MapGenContext>("lycanroc", 23, 0, 1), 10);
                                bossRooms.Add(getBossRoomGen<MapGenContext>("bastiodon", 23, 0, 1), 10);
                                bossRooms.Add(getBossRoomGen<MapGenContext>("aerodactyl", 23, 0, 1), 10);
                                bossRooms.Add(getBossRoomGen<MapGenContext>("flygon", 23, 0, 1), 10);
                                bossRooms.Add(getBossRoomGen<MapGenContext>("arbok", 23, 0, 1), 10);
                                bossRooms.Add(getBossRoomGen<MapGenContext>("ditto", 23, 0, 1), 10);
                            }
                            //special boss that only shows up on the final floor.
                            //guaranteed to be one of these, guaranteed only one of these
                            if (ii == 3 && kk == 2)
                            {
                                bossRooms.Add(getBossRoomGen<MapGenContext>("camerupt-water", 23, 0, 1), 10);
                                bossRooms.Add(getBossRoomGen<MapGenContext>("claydol-water", 23, 0, 1), 10);
                            }
                            layout.GenSteps.Add(PR_ROOMS_GEN_EXTRA, CreateGenericBossRoomStep(bossRooms, kk));
                        }
                        //sealing the boss room and treasure room
                        {
                            BossSealStep<MapGenContext> vaultStep = new BossSealStep<MapGenContext>("sealed_block", "tile_boss");
                            vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                            vaultStep.Filters.Add(new RoomFilterIndex(false, kk));
                            vaultStep.BossFilters.Add(new RoomFilterComponent(false, new BossRoom()));
                            vaultStep.BossFilters.Add(new RoomFilterIndex(false, kk));
                            layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                        }
                        //vault treasures
                        {
                            BulkSpawner<MapGenContext, MapItem> treasures = new BulkSpawner<MapGenContext, MapItem>();
                            if (kk == 2)
                            {
                                //joy seed guaranteed; it is an oasis
                                treasures.SpecificSpawns.Add(new MapItem("seed_joy"));
                            }
                            if (ii == 1)
                            {
                                treasures.SpecificSpawns.Add(new MapItem("food_apple", 1));
                            }
                            else if (ii == 2)
                            {
                                treasures.SpecificSpawns.Add(new MapItem("orb_luminous", 1));
                            }
                            if (ii == 3 && kk == 0)
                            {
                                //key guaranteed; go check the chest.
                                treasures.SpecificSpawns.Add(new MapItem("key", 1));
                            }

                            treasures.SpawnAmount = 3;
                            foreach (string key in IterateVitamins())
                                treasures.RandomSpawns.Add(new MapItem(key), 4);//boosters
                            foreach (string key in IterateGummis())
                                treasures.RandomSpawns.Add(new MapItem(key), 4);//gummis
                            treasures.RandomSpawns.Add(new MapItem("held_pierce_band"), 5);//pierce band
                            treasures.RandomSpawns.Add(new MapItem("held_friend_bow"), 5);//friend bow
                            treasures.RandomSpawns.Add(new MapItem("held_goggle_specs"), 5);//goggle specs
                            treasures.RandomSpawns.Add(new MapItem("evo_sun_ribbon"), 10);
                            treasures.RandomSpawns.Add(new MapItem("evo_lunar_ribbon"), 10);
                            treasures.RandomSpawns.Add(new MapItem("medicine_max_elixir"), 20);//max elixir
                            treasures.RandomSpawns.Add(new MapItem("medicine_max_potion"), 20);//max potion
                            treasures.RandomSpawns.Add(new MapItem("medicine_full_heal"), 20);//full heal
                            treasures.RandomSpawns.Add(new MapItem("medicine_amber_tear", 2), 20);//amber tear

                            SpawnList<IStepSpawner<MapGenContext, MapItem>> boxSpawn = new SpawnList<IStepSpawner<MapGenContext, MapItem>>();
                            boxSpawn.Add(new BoxSpawner<MapGenContext>("box_heavy", new SpeciesItemContextSpawner<MapGenContext>(new IntRange(2), new RandRange(1))), 10);
                            MultiStepSpawner<MapGenContext, MapItem> boxPicker = new MultiStepSpawner<MapGenContext, MapItem>(new LoopedRand<IStepSpawner<MapGenContext, MapItem>>(boxSpawn, new RandRange(1)));

                            MultiStepSpawner<MapGenContext, MapItem> mainSpawner = new MultiStepSpawner<MapGenContext, MapItem>();
                            mainSpawner.Picker = new PresetMultiRand<IStepSpawner<MapGenContext, MapItem>>(treasures, boxPicker);
                            RandomRoomSpawnStep<MapGenContext, MapItem> detourItems = new RandomRoomSpawnStep<MapGenContext, MapItem>(mainSpawner);
                            detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                            detourItems.Filters.Add(new RoomFilterIndex(false, kk));
                            layout.GenSteps.Add(PR_SPAWN_ITEMS_EXTRA, detourItems);
                        }
                    }

                    for (int kk = 0; kk < chest_amount; kk++)
                    {
                        ChestStep<MapGenContext> chestStep = new ChestStep<MapGenContext>(false, GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));

                        foreach (string key in IterateVitamins())
                            chestStep.Items.Add(new MapItem(key), 4);//boosters
                        foreach (string key in IterateGummis())
                            chestStep.Items.Add(new MapItem(key), 4);//gummis
                        chestStep.Items.Add(new MapItem("held_pierce_band"), 10);//pierce band
                        chestStep.Items.Add(new MapItem("held_friend_bow"), 10);//friend bow
                        chestStep.Items.Add(new MapItem("held_goggle_specs"), 10);//goggle specs
                        chestStep.Items.Add(new MapItem("evo_harmony_scarf"), 10);//harmony scarf

                        chestStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(HeldState)), false, true, new RandRange(1)), 10);//held
                        chestStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(DrinkState)), false, true, new RandRange(3, 6)), 10);//vitamins
                        chestStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), false, true, new RandRange(5, 10)), 10);//gummis
                        layout.GenSteps.Add(PR_HOUSES, chestStep);
                    }

                    {
                        SetCompassStep<MapGenContext> trapStep = new SetCompassStep<MapGenContext>("tile_compass");
                        layout.GenSteps.Add(PR_COMPASS, trapStep);
                    }

                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                    floorSegment.Floors.Add(layout);
                }

                {
                    LoadGen layout = new LoadGen();
                    MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                    startGen.MapID = "end_forsaken_desert";
                    layout.GenSteps.Add(PR_FILE_LOAD, startGen);

                    MapTimeLimitStep<MapLoadContext> floorData = new MapTimeLimitStep<MapLoadContext>(600);
                    layout.GenSteps.Add(PR_FLOOR_DATA, floorData);

                    AddTextureData(layout, "northern_desert_2_wall", "northern_desert_2_floor", "northern_desert_2_secondary", "ground");

                    {
                        HashSet<string> exceptFor = new HashSet<string>();
                        foreach (string legend in IterateLegendaries())
                            exceptFor.Add(legend);
                        SpeciesItemElementSpawner<MapLoadContext> spawn = new SpeciesItemElementSpawner<MapLoadContext>(new IntRange(2), new RandRange(1), "ground", exceptFor);
                        BoxSpawner<MapLoadContext> box = new BoxSpawner<MapLoadContext>("box_heavy", spawn);
                        List<Loc> treasureLocs = new List<Loc>();
                        treasureLocs.Add(new Loc(7, 6));
                        layout.GenSteps.Add(PR_SPAWN_ITEMS, new SpecificSpawnStep<MapLoadContext, MapItem>(box, treasureLocs));
                    }

                    List<InvItem> treasure1 = new List<InvItem>();
                    treasure1.Add(InvItem.CreateBox("box_glittery", "loot_nugget"));//Nugget

                    List<(List<InvItem>, Loc)> items = new List<(List<InvItem>, Loc)>();
                    items.Add((treasure1, new Loc(7, 12)));
                    AddSpecificSpawnPool(layout, items, PR_SPAWN_ITEMS);

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
                    startGen.MapID = "secret_forsaken_desert";
                    layout.GenSteps.Add(PR_FILE_LOAD, startGen);

                    MapTimeLimitStep<MapLoadContext> floorData = new MapTimeLimitStep<MapLoadContext>(600);
                    layout.GenSteps.Add(PR_FLOOR_DATA, floorData);

                    //add a chest
                    List<(List<InvItem>, Loc)> items = new List<(List<InvItem>, Loc)>();
                    {
                        List<InvItem> treasure = new List<InvItem>();
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_eevee_02"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_eevee_05"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_eevee_08"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_eevee_11"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_eevee_14"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_eevee_17"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_eevee_20"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_eevee_23"));
                        items.Add((treasure, new Loc(6, 8)));
                    }
                    //TODO: do we want other things in this pool?
                    {
                        List<InvItem> treasure = new List<InvItem>();
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_bulbasaur_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_charmander_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_squirtle_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_pikachu_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_chikorita_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_cyndaquil_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_totodile_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_treecko_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_torchic_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_mudkip_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_turtwig_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_chimchar_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_piplup_01"));
                        items.Add((treasure, new Loc(2, 6)));
                    }
                    {
                        List<InvItem> treasure = new List<InvItem>();
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_bulbasaur_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_charmander_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_squirtle_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_pikachu_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_chikorita_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_cyndaquil_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_totodile_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_treecko_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_torchic_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_mudkip_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_turtwig_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_chimchar_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_piplup_01"));
                        items.Add((treasure, new Loc(10, 6)));
                    }
                    {
                        List<InvItem> treasure = new List<InvItem>();
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_bulbasaur_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_charmander_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_squirtle_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_pikachu_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_chikorita_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_cyndaquil_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_totodile_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_treecko_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_torchic_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_mudkip_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_turtwig_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_chimchar_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_piplup_01"));
                        items.Add((treasure, new Loc(2, 11)));
                    }
                    {
                        List<InvItem> treasure = new List<InvItem>();
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_bulbasaur_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_charmander_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_squirtle_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_pikachu_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_chikorita_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_cyndaquil_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_totodile_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_treecko_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_torchic_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_mudkip_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_turtwig_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_chimchar_01"));
                        treasure.Add(InvItem.CreateBox("box_deluxe", "xcl_family_piplup_01"));
                        items.Add((treasure, new Loc(10, 11)));
                    }
                    AddSpecificSpawnPool(layout, items, PR_SPAWN_ITEMS);

                    floorSegment.Floors.Add(layout);

                }
                zone.Segments.Add(floorSegment);
            }
            #endregion
        }
    }
}
