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
        static void FillTinyTunnel(ZoneData zone, bool translate)
        {
            #region TINY TUNNEL
            {
                zone.Name = new LocalText("Tiny Tunnel");
                zone.Level = 5;
                zone.LevelCap = true;
                zone.BagRestrict = 8;
                zone.KeepTreasure = true;
                zone.TeamSize = 2;
                zone.Rescues = 2;
                zone.Rogue = RogueStatus.ItemTransfer;

                {
                    int max_floors = 8;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Tiny Tunnel\nB{0}F"), new Priority(-15)));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level + 40, 0);
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;


                    //necessities
                    CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                    necessities.SpawnRates.SetRange(14, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("necessities", necessities);

                    necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 10);
                    necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 5);
                    necessities.Spawns.Add(new InvItem("herb_white"), new IntRange(0, max_floors), 10);

                    //snacks
                    CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                    snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                    snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_sleep"), new IntRange(0, max_floors), 10);
                    snacks.Spawns.Add(new InvItem("seed_hunger"), new IntRange(0, max_floors), 10);
                    //boosters
                    CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                    boosters.SpawnRates.SetRange(5, new IntRange(2, 6));
                    boosters.SpawnRates.SetRange(20, new IntRange(6, max_floors));
                    itemSpawnZoneStep.Spawns.Add("boosters", boosters);


                    boosters.Spawns.Add(new InvItem("gummi_blue"), new IntRange(0, max_floors), 10);
                    boosters.Spawns.Add(new InvItem("gummi_black"), new IntRange(0, max_floors), 10);
                    boosters.Spawns.Add(new InvItem("gummi_clear"), new IntRange(0, max_floors), 10);
                    boosters.Spawns.Add(new InvItem("gummi_grass"), new IntRange(0, max_floors), 10);
                    boosters.Spawns.Add(new InvItem("gummi_green"), new IntRange(0, max_floors), 10);
                    boosters.Spawns.Add(new InvItem("gummi_brown"), new IntRange(0, max_floors), 10);
                    boosters.Spawns.Add(new InvItem("gummi_orange"), new IntRange(0, max_floors), 10);
                    boosters.Spawns.Add(new InvItem("gummi_gold"), new IntRange(0, max_floors), 10);
                    boosters.Spawns.Add(new InvItem("gummi_pink"), new IntRange(0, max_floors), 10);
                    boosters.Spawns.Add(new InvItem("gummi_purple"), new IntRange(0, max_floors), 10);
                    boosters.Spawns.Add(new InvItem("gummi_red"), new IntRange(0, max_floors), 10);
                    boosters.Spawns.Add(new InvItem("gummi_royal"), new IntRange(0, max_floors), 10);
                    boosters.Spawns.Add(new InvItem("gummi_silver"), new IntRange(0, max_floors), 10);
                    boosters.Spawns.Add(new InvItem("gummi_white"), new IntRange(0, max_floors), 10);
                    boosters.Spawns.Add(new InvItem("gummi_yellow"), new IntRange(0, max_floors), 10);
                    boosters.Spawns.Add(new InvItem("gummi_sky"), new IntRange(0, max_floors), 10);
                    boosters.Spawns.Add(new InvItem("gummi_gray"), new IntRange(0, max_floors), 10);
                    boosters.Spawns.Add(new InvItem("gummi_magenta"), new IntRange(0, max_floors), 10);
                    //throwable
                    CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
                    throwable.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("throwable", throwable);


                    throwable.Spawns.Add(new InvItem("ammo_cacnea_spike", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("ammo_corsola_twig", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("ammo_geo_pebble", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("ammo_gravelerock", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, max_floors), 10);
                    throwable.Spawns.Add(new InvItem("wand_fear", false, 3), new IntRange(0, max_floors), 10);

                    //orbs
                    CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                    orbs.SpawnRates.SetRange(8, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                    orbs.Spawns.Add(new InvItem("orb_spurn"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_all_protect"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_slumber"), new IntRange(0, max_floors), 10);
                    orbs.Spawns.Add(new InvItem("orb_totter"), new IntRange(0, max_floors), 10);
                    //tms
                    CategorySpawn<InvItem> tms = new CategorySpawn<InvItem>();
                    tms.SpawnRates.SetRange(8, new IntRange(2, max_floors));
                    itemSpawnZoneStep.Spawns.Add("tms", tms);

                    foreach (string tm in IterateTMs(TMClass.Starter))
                    {
                        tms.Spawns.Add(new InvItem(tm), new IntRange(0, max_floors), 10);
                    }

                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors)), new MapItem("food_apple"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanQuota(new RandRange(1), new IntRange(0, 3)), new MapItem("berry_leppa"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanQuota(new RandRange(1), new IntRange(4, 7)), new MapItem("berry_leppa"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanQuota(new RandRange(1), new IntRange(0, 5)), new MapItem("key", 1));


                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;


                    poolSpawn.Spawns.Add(GetTeamMob("kricketot", "", "growl", "bide", "", "", new RandRange(3)), new IntRange(0, 2), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("wurmple", "", "tackle", "string_shot", "", "", new RandRange(3)), new IntRange(0, 2), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("spearow", "", "leer", "peck", "", "", new RandRange(5)), new IntRange(1, 3), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("purrloin", "unburden", "scratch", "", "", "", new RandRange(5), "wander_normal_itemless"), new IntRange(1, 3), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("wooper", "", "water_gun", "", "", "", new RandRange(6)), new IntRange(2, 4), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("silcoon", "", "harden", "bug_bite", "", "", new RandRange(7), "turret"), new IntRange(2, 4), 5);
                    poolSpawn.Spawns.Add(GetTeamMob("cascoon", "", "harden", "bug_bite", "", "", new RandRange(7), "turret"), new IntRange(2, 4), 5);
                    //sleeping
                    poolSpawn.Spawns.Add(GetTeamMob("whismur", "", "uproar", "", "", "", new RandRange(8), "wander_normal", true), new IntRange(3, 5), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("nickit", "unburden", "beat_up", "tail_whip", "", "", new RandRange(9), "retreater_itemless"), new IntRange(3, 5), 5);
                    poolSpawn.Spawns.Add(GetTeamMob("zigzagoon", "pickup", "headbutt", "", "", "", new RandRange(11)), new IntRange(4, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("venonat", "tinted_lens", "confusion", "", "", "", new RandRange(11)), new IntRange(4, 6), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("rowlet", "", "leafage", "", "", "", new RandRange(11)), new IntRange(5, 7), 5);
                    poolSpawn.Spawns.Add(GetTeamMob("scorbunny", "", "ember", "", "", "", new RandRange(11)), new IntRange(5, 7), 5);
                    poolSpawn.Spawns.Add(GetTeamMob("sewaddle", "", "bug_bite", "", "", "", new RandRange(12)), new IntRange(5, 7), 5);
                    poolSpawn.Spawns.Add(GetTeamMob("beautifly", "", "gust", "string_shot", "", "", new RandRange(13)), new IntRange(6, 8), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("dustox", "", "confusion", "string_shot", "", "", new RandRange(13)), new IntRange(6, 8), 10);
                    poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("rattata", 1, "", Gender.Unknown), "hustle", "focus_energy", "bite", "", "", new RandRange(14)), new IntRange(7, 8), 10);
                    poolSpawn.Spawns.Add(GetTeamMob("kricketune", "technician", "growl", "leech_life", "sing", "", new RandRange(14)), new IntRange(7, 8), 10);

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;

                    tileSpawn.Spawns.Add(new EffectTile("trap_mud", false), new IntRange(0, max_floors), 10);//mud trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_warp", true), new IntRange(0, max_floors), 10);//warp trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_gust", false), new IntRange(0, max_floors), 10);//gust trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, max_floors), 10);//chestnut trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_poison", false), new IntRange(0, max_floors), 10);//poison trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_slumber", false), new IntRange(0, max_floors), 10);//sleep trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_seal", false), new IntRange(0, max_floors), 10);//seal trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_self_destruct", false), new IntRange(0, max_floors), 10);//selfdestruct trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_hunger", true), new IntRange(0, max_floors), 10);//hunger trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_apple", true), new IntRange(0, max_floors), 3);//apple trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_pp_leech", true), new IntRange(0, max_floors), 10);//pp-leech trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_summon", false), new IntRange(0, max_floors), 10);//summon trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_explosion", false), new IntRange(0, max_floors), 10);//explosion trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_slow", false), new IntRange(0, max_floors), 10);//slow trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_spin", false), new IntRange(0, max_floors), 10);//spin trap

                    floorSegment.ZoneSteps.Add(tileSpawn);



                    {
                        SpreadCombinedZoneStep combinedVaultZoneStep = new SpreadCombinedZoneStep();

                        {
                            SpreadVaultZoneStep detourChanceZoneStep = new SpreadVaultZoneStep(PR_SPAWN_ITEMS_EXTRA, PR_SPAWN_TRAPS, PR_SPAWN_MOBS_EXTRA, new SpreadPlanQuota(new RandRange(1), new IntRange(4, max_floors)));

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

                            PopulateVaultItems(detourChanceZoneStep, DungeonStage.Advanced, DungeonAccessibility.Hidden, max_floors, true, true);

                            combinedVaultZoneStep.Steps.Add(detourChanceZoneStep);
                        }

                        floorSegment.ZoneSteps.Add(combinedVaultZoneStep);
                    }


                    //switch vaults
                    {
                        SpreadVaultZoneStep vaultChanceZoneStep = new SpreadVaultZoneStep(PR_SPAWN_ITEMS_EXTRA, PR_SPAWN_TRAPS, PR_SPAWN_MOBS_EXTRA, new SpreadPlanQuota(new RandDecay(1, 4, 40), new IntRange(0, max_floors - 1)));

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

                        PopulateVaultItems(vaultChanceZoneStep, DungeonStage.Beginner, DungeonAccessibility.Hidden, max_floors, false);

                        floorSegment.ZoneSteps.Add(vaultChanceZoneStep);
                    }

                    AddEvoZoneStep(floorSegment, new SpreadPlanQuota(new RandRange(1), new IntRange(5)), EvoRoomType.Small);


                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        GridFloorGen layout = new GridFloorGen();

                        //Floor settings
                        if (ii < 4)
                            AddFloorData(layout, "Demonstration.ogg", 1200, Map.SightRange.Clear, Map.SightRange.Dark);
                        else
                            AddFloorData(layout, "Bramble Woods.ogg", 1200, Map.SightRange.Dark, Map.SightRange.Dark);

                        //Tilesets
                        if (ii < 4)
                            AddSpecificTextureData(layout, "thunderwave_cave_wall", "thunderwave_cave_floor", "thunderwave_cave_secondary", "tall_grass", "bug");
                        else
                            AddSpecificTextureData(layout, "murky_cave_wall", "murky_cave_floor", "murky_cave_secondary", "tall_grass_dark", "bug");

                        if (ii >= 4)
                            AddBlobWaterSteps(layout, "water", new RandRange(7, 12), new IntRange(1, 5), true);


                        SpawnList<PatternPlan> terrainPattern = new SpawnList<PatternPlan>();
                        terrainPattern.Add(new PatternPlan("pattern_blob_small", PatternPlan.PatternExtend.Single), 10);
                        terrainPattern.Add(new PatternPlan("pattern_dither_three_fourth", PatternPlan.PatternExtend.Single), 20);
                        terrainPattern.Add(new PatternPlan("pattern_plus", PatternPlan.PatternExtend.Single), 5);
                        terrainPattern.Add(new PatternPlan("pattern_crosshair", PatternPlan.PatternExtend.Single), 5);
                        if (ii < 2)
                            AddTerrainPatternSteps(layout, "grass", new RandRange(8, 12), terrainPattern, false, ConnectivityRoom.Connectivity.Main, true);
                        else if (ii < 4)
                            AddTerrainPatternSteps(layout, "grass", new RandRange(10, 14), terrainPattern, false, ConnectivityRoom.Connectivity.Main, true);
                        else
                            AddTerrainPatternSteps(layout, "grass", new RandRange(11, 15), terrainPattern, false, ConnectivityRoom.Connectivity.Main, true);

                        //traps
                        AddSingleTrapStep(layout, new RandRange(4, 6), "tile_wonder");//wonder tile
                        if (ii < 2)
                            AddTrapsSteps(layout, new RandRange(6, 9));
                        else
                            AddTrapsSteps(layout, new RandRange(8, 12));

                        //money
                        if (ii < 4)
                            AddMoneyData(layout, new RandRange(2, 4));
                        else
                            AddMoneyData(layout, new RandRange(3, 5));

                        //enemies!
                        AddRespawnData(layout, 6 + ii, 80);

                        //enemies
                        AddEnemySpawnData(layout, 20, new RandRange(4 + ii, 6 + ii));

                        //items
                        if (ii < 4)
                            AddItemData(layout, new RandRange(3, 6), 25);
                        else if (ii < 6)
                            AddItemData(layout, new RandRange(5, 8), 10);
                        else
                            AddItemData(layout, new RandRange(7, 10), 10);

                        {
                            List<MapItem> specificSpawns = new List<MapItem>();
                            if (ii == 0)
                                specificSpawns.Add(new MapItem("apricorn_plain"));//Plain Apricorn

                            RandomRoomSpawnStep<MapGenContext, MapItem> specificItemZoneStep = new RandomRoomSpawnStep<MapGenContext, MapItem>(new PickerSpawner<MapGenContext, MapItem>(new PresetMultiRand<MapItem>(specificSpawns)));
                            specificItemZoneStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            layout.GenSteps.Add(PR_SPAWN_ITEMS, specificItemZoneStep);
                        }

                        //construct paths
                        {
                            if (ii < 2)
                                AddInitGridStep(layout, 4, 4, 9, 9);
                            else if (ii < 4)
                                AddInitGridStep(layout, 5, 4, 9, 9);
                            else
                                AddInitGridStep(layout, 5, 5, 9, 9);

                            GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.RoomRatio = new RandRange(80);
                            path.BranchRatio = new RandRange(25, 50);

                            SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                            //cross
                            genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 10), new RandRange(4, 10), new RandRange(2, 6), new RandRange(2, 6)), 20);
                            //blocked
                            genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile("wall"), new RandRange(5, 8), new RandRange(5, 8), new RandRange(2, 4), new RandRange(2, 4)), 5);
                            path.GenericRooms = genericRooms;

                            if (ii >= 4)
                            {
                                CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(1), GetImmutableFilterList());
                                step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                                step.Filters.Add(new RoomFilterDefaultGen(true));
                                step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                                step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 2), new RoomGenCave<MapGenContext>(new RandRange(19), new RandRange(19))), 10);
                                layout.GenSteps.Add(PR_GRID_GEN, step);
                            }

                            SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(100, new SquareHallBrush(new Loc(2))), 10);
                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_GRID_GEN, path);

                            layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 100));

                        }

                        AddDrawGridSteps(layout);

                        AddStairStep(layout, true);

                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        floorSegment.Floors.Add(layout);
                    }




                    {
                        LoadGen layout = new LoadGen();
                        MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                        startGen.MapID = "end_tiny_tunnel";
                        layout.GenSteps.Add(PR_FILE_LOAD, startGen);

                        MapTimeLimitStep<MapLoadContext> floorData = new MapTimeLimitStep<MapLoadContext>(600);
                        layout.GenSteps.Add(PR_FLOOR_DATA, floorData);

                        AddSpecificTextureData(layout, "murky_cave_wall", "murky_cave_floor", "murky_cave_secondary", "tall_grass_dark", "bug");

                        {
                            HashSet<string> exceptFor = new HashSet<string>();
                            foreach (string legend in IterateLegendaries())
                                exceptFor.Add(legend);
                            SpeciesItemElementSpawner<MapLoadContext> spawn = new SpeciesItemElementSpawner<MapLoadContext>(new IntRange(2), new RandRange(2), "bug", exceptFor);
                            BoxSpawner<MapLoadContext> box = new BoxSpawner<MapLoadContext>("box_heavy", spawn);
                            List<Loc> treasureLocs = new List<Loc>();
                            treasureLocs.Add(new Loc(6, 8));
                            treasureLocs.Add(new Loc(8, 8));
                            layout.GenSteps.Add(PR_SPAWN_ITEMS, new SpecificSpawnStep<MapLoadContext, MapItem>(box, treasureLocs));
                        }

                        SpawnList<InvItem> treasure1 = new SpawnList<InvItem>();

                        treasure1.Add(InvItem.CreateBox("box_dainty", "seed_joy"), 10);
                        treasure1.Add(InvItem.CreateBox("box_dainty", "boost_nectar"), 10);
                        treasure1.Add(InvItem.CreateBox("box_dainty", "food_apple_perfect"), 10);

                        List<(SpawnList<InvItem>, Loc)> items = new List<(SpawnList<InvItem>, Loc)>();
                        items.Add((treasure1, new Loc(7, 8)));
                        AddSpecificSpawnPool(layout, items, PR_SPAWN_ITEMS);

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
            }
            #endregion
        }

        static void FillGeodeCrevice(ZoneData zone, bool translate)
        {
            #region GEODE CREVICE
            {
                zone.Name = new LocalText("Geode Crevice");
                zone.Level = 20;
                zone.LevelCap = true;
                zone.BagRestrict = 8;
                zone.KeepTreasure = true;
                zone.MoneyRestrict = true;
                zone.TeamSize = 2;
                zone.Rescues = 2;
                zone.Rogue = RogueStatus.ItemTransfer;

                {
                    int max_floors = 13;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Geode Crevice\nB{0}F"), new Priority(-15)));

                    //money
                    MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level + 40, 0);
                    moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                    floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                    //items
                    ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                    itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;

                    //necesities
                    CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                    necessities.SpawnRates.SetRange(14, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("necessities", necessities);

                    necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 30);//Leppa
                    necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 40);//Oran
                    necessities.Spawns.Add(new InvItem("berry_sitrus"), new IntRange(0, max_floors), 30);//Sitrus
                    necessities.Spawns.Add(new InvItem("food_apple"), new IntRange(0, max_floors), 40);//Apple
                    necessities.Spawns.Add(new InvItem("food_grimy"), new IntRange(4, max_floors), 30);//Grimy Food
                    necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 50);//Lum berry

                    necessities.Spawns.Add(new InvItem("seed_reviver"), new IntRange(0, max_floors), 30);//reviver seed
                    necessities.Spawns.Add(new InvItem("seed_reviver", true), new IntRange(0, max_floors), 15);//reviver seed
                    necessities.Spawns.Add(new InvItem("machine_recall_box"), new IntRange(0, max_floors), 30);//Link Box


                    //snacks
                    CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                    snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("snacks", snacks);

                    foreach (string key in IteratePinchBerries())
                        snacks.Spawns.Add(new InvItem(key), new IntRange(0, max_floors), 3);
                    snacks.Spawns.Add(new InvItem("berry_enigma"), new IntRange(0, max_floors), 4);//enigma berry

                    snacks.Spawns.Add(new InvItem("berry_jaboca"), new IntRange(0, max_floors), 5);//Jaboca
                    snacks.Spawns.Add(new InvItem("berry_rowap"), new IntRange(0, max_floors), 5);//Rowap

                    foreach (string key in IterateTypeBerries())
                        snacks.Spawns.Add(new InvItem(key), new IntRange(0, max_floors), 1);

                    snacks.Spawns.Add(new InvItem("seed_blast"), new IntRange(0, max_floors), 20);//blast seed
                    snacks.Spawns.Add(new InvItem("seed_warp"), new IntRange(0, max_floors), 10);//warp seed
                    snacks.Spawns.Add(new InvItem("seed_decoy"), new IntRange(0, max_floors), 10);//decoy seed
                    snacks.Spawns.Add(new InvItem("seed_sleep"), new IntRange(0, max_floors), 10);//sleep seed
                    snacks.Spawns.Add(new InvItem("seed_blinker"), new IntRange(0, max_floors), 10);//blinker seed
                    snacks.Spawns.Add(new InvItem("seed_last_chance"), new IntRange(0, max_floors), 5);//last-chance seed
                    snacks.Spawns.Add(new InvItem("seed_doom"), new IntRange(0, max_floors), 5);//doom seed
                    snacks.Spawns.Add(new InvItem("seed_ban"), new IntRange(0, max_floors), 10);//ban seed
                    snacks.Spawns.Add(new InvItem("seed_pure"), new IntRange(0, max_floors), 4);//pure seed
                    snacks.Spawns.Add(new InvItem("seed_pure", true), new IntRange(0, max_floors), 4);//pure seed
                    snacks.Spawns.Add(new InvItem("seed_ice"), new IntRange(0, max_floors), 10);//ice seed
                    snacks.Spawns.Add(new InvItem("seed_vile"), new IntRange(0, max_floors), 10);//vile seed

                    snacks.Spawns.Add(new InvItem("herb_power"), new IntRange(0, max_floors), 10);//power herb
                    snacks.Spawns.Add(new InvItem("herb_mental"), new IntRange(0, max_floors), 5);//mental herb
                    snacks.Spawns.Add(new InvItem("herb_white"), new IntRange(0, max_floors), 50);//white herb


                    //boosters
                    CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
                    boosters.SpawnRates.SetRange(7, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("boosters", boosters);

                    foreach (string key in IterateGummis(false))
                        boosters.Spawns.Add(new InvItem(key), new IntRange(0, max_floors), 1);

                    IntRange range = new IntRange(4, max_floors);

                    boosters.Spawns.Add(new InvItem("boost_protein"), range, 2);//protein
                    boosters.Spawns.Add(new InvItem("boost_iron"), range, 2);//iron
                    boosters.Spawns.Add(new InvItem("boost_calcium"), range, 2);//calcium
                    boosters.Spawns.Add(new InvItem("boost_zinc"), range, 2);//zinc
                    boosters.Spawns.Add(new InvItem("boost_carbos"), range, 2);//carbos
                    boosters.Spawns.Add(new InvItem("boost_hp_up"), range, 2);//hp up

                    //throwable
                    CategorySpawn<InvItem> ammo = new CategorySpawn<InvItem>();
                    ammo.SpawnRates.SetRange(1, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("ammo", ammo);

                    range = new IntRange(0, max_floors);
                    {
                        ammo.Spawns.Add(new InvItem("ammo_rare_fossil", false, 3), range, 10);//Rare fossil
                    }


                    //special items
                    CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
                    special.SpawnRates.SetRange(5, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("special", special);

                    {
                        range = new IntRange(0, max_floors);
                        int rate = 2;

                        special.Spawns.Add(new InvItem("apricorn_plain", true), range, rate);//Plain Apricorn
                        special.Spawns.Add(new InvItem("apricorn_blue"), range, rate);//blue apricorns
                        special.Spawns.Add(new InvItem("apricorn_green"), range, rate);//green apricorns
                        special.Spawns.Add(new InvItem("apricorn_brown"), range, rate);//brown apricorns
                        special.Spawns.Add(new InvItem("apricorn_purple"), range, rate);//purple apricorns
                        special.Spawns.Add(new InvItem("apricorn_red"), range, rate);//red apricorns
                        special.Spawns.Add(new InvItem("apricorn_white"), range, rate);//white apricorns
                        special.Spawns.Add(new InvItem("apricorn_yellow"), range, rate);//yellow apricorns
                        special.Spawns.Add(new InvItem("apricorn_black"), range, rate);//black apricorns

                    }

                    special.Spawns.Add(new InvItem("machine_ability_capsule"), new IntRange(4, max_floors), 5); // Ability Capsule



                    //orbs
                    CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                    orbs.SpawnRates.SetRange(14, new IntRange(0, 30));
                    itemSpawnZoneStep.Spawns.Add("orbs", orbs);

                    {
                        range = new IntRange(0, max_floors);
                        orbs.Spawns.Add(new InvItem("orb_one_room"), range, 7);//One-Room Orb
                        orbs.Spawns.Add(new InvItem("orb_fill_in"), range, 7);//Fill-In Orb
                        orbs.Spawns.Add(new InvItem("orb_one_room", true), range, 3);//One-Room Orb
                        orbs.Spawns.Add(new InvItem("orb_fill_in", true), range, 3);//Fill-In Orb

                        orbs.Spawns.Add(new InvItem("orb_petrify"), range, 10);//Petrify
                        orbs.Spawns.Add(new InvItem("orb_halving"), range, 10);//Halving
                        orbs.Spawns.Add(new InvItem("orb_slumber"), range, 8);//Slumber Orb
                        orbs.Spawns.Add(new InvItem("orb_slow"), range, 8);//Slow
                        orbs.Spawns.Add(new InvItem("orb_totter"), range, 8);//Totter
                        orbs.Spawns.Add(new InvItem("orb_spurn"), range, 5);//Spurn
                        orbs.Spawns.Add(new InvItem("orb_stayaway"), range, 3);//Stayaway
                        orbs.Spawns.Add(new InvItem("orb_pierce"), range, 8);//Pierce
                        orbs.Spawns.Add(new InvItem("orb_freeze"), range, 5);//Freeze
                        orbs.Spawns.Add(new InvItem("orb_devolve"), range, 3);//Devolve
                        orbs.Spawns.Add(new InvItem("orb_slumber", true), range, 3);//Slumber Orb
                        orbs.Spawns.Add(new InvItem("orb_slow", true), range, 3);//Slow
                        orbs.Spawns.Add(new InvItem("orb_totter", true), range, 3);//Totter
                        orbs.Spawns.Add(new InvItem("orb_spurn", true), range, 2);//Spurn
                        orbs.Spawns.Add(new InvItem("orb_stayaway", true), range, 3);//Stayaway
                        orbs.Spawns.Add(new InvItem("orb_pierce", true), range, 3);//Pierce
                        orbs.Spawns.Add(new InvItem("orb_freeze", true), range, 3);//Freeze
                        orbs.Spawns.Add(new InvItem("orb_devolve", true), range, 3);//Devolve

                        orbs.Spawns.Add(new InvItem("orb_all_aim"), range, 10);//All-Aim Orb
                        orbs.Spawns.Add(new InvItem("orb_trap_see"), range, 10);//Trap-See
                        orbs.Spawns.Add(new InvItem("orb_trapbust"), range, 10);//Trapbust
                        orbs.Spawns.Add(new InvItem("orb_foe_hold"), range, 10);//Foe-Hold
                        orbs.Spawns.Add(new InvItem("orb_mobile"), range, 10);//Mobile
                        orbs.Spawns.Add(new InvItem("orb_rollcall"), range, 10);//Roll Call
                        orbs.Spawns.Add(new InvItem("orb_mug"), range, 10);//Mug
                        orbs.Spawns.Add(new InvItem("orb_mirror"), range, 10);//Mirror
                        orbs.Spawns.Add(new InvItem("orb_rebound"), range, 10);//Rebound
                        orbs.Spawns.Add(new InvItem("orb_scanner"), range, 9);//Scanner
                        orbs.Spawns.Add(new InvItem("orb_weather"), range, 10);//Weather Orb
                        orbs.Spawns.Add(new InvItem("orb_foe_seal"), range, 10);//Foe-Seal
                        orbs.Spawns.Add(new InvItem("orb_nullify"), range, 10);//Nullify
                    }


                    //held items
                    CategorySpawn<InvItem> heldItems = new CategorySpawn<InvItem>();
                    heldItems.SpawnRates.SetRange(2, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("held", heldItems);

                    foreach (string key in IterateTypeBoosters())
                        heldItems.Spawns.Add(new InvItem(key), new IntRange(0, max_floors), 1);
                    foreach (string key in IterateTypePlates())
                        heldItems.Spawns.Add(new InvItem(key), new IntRange(0, max_floors), 1);

                    heldItems.Spawns.Add(new InvItem("held_mobile_scarf"), new IntRange(0, max_floors), 2);//Mobile Scarf
                    heldItems.Spawns.Add(new InvItem("held_pass_scarf"), new IntRange(0, max_floors), 2);//Pass Scarf


                    heldItems.Spawns.Add(new InvItem("held_cover_band"), new IntRange(0, max_floors), 2);//Cover Band
                    heldItems.Spawns.Add(new InvItem("held_reunion_cape"), new IntRange(0, max_floors), 1);//Reunion Cape
                    heldItems.Spawns.Add(new InvItem("held_reunion_cape", true), new IntRange(0, max_floors), 1);//Reunion Cape

                    heldItems.Spawns.Add(new InvItem("held_trap_scarf"), new IntRange(0, max_floors), 2);//Trap Scarf
                    heldItems.Spawns.Add(new InvItem("held_trap_scarf", true), new IntRange(0, max_floors), 1);//Trap Scarf

                    heldItems.Spawns.Add(new InvItem("held_grip_claw"), new IntRange(0, max_floors), 2);//Grip Claw

                    range = new IntRange(0, max_floors);
                    heldItems.Spawns.Add(new InvItem("held_twist_band"), range, 2);//Twist Band
                    heldItems.Spawns.Add(new InvItem("held_metronome"), range, 1);//Metronome
                    heldItems.Spawns.Add(new InvItem("held_twist_band", true), range, 1);//Twist Band
                    heldItems.Spawns.Add(new InvItem("held_metronome", true), range, 1);//Metronome
                    heldItems.Spawns.Add(new InvItem("held_shell_bell"), range, 1);//Shell Bell
                    heldItems.Spawns.Add(new InvItem("held_scope_lens"), range, 1);//Scope Lens
                    heldItems.Spawns.Add(new InvItem("held_power_band"), range, 2);//Power Band
                    heldItems.Spawns.Add(new InvItem("held_special_band"), range, 2);//Special Band
                    heldItems.Spawns.Add(new InvItem("held_defense_scarf"), range, 2);//Defense Scarf
                    heldItems.Spawns.Add(new InvItem("held_zinc_band"), range, 2);//Zinc Band
                    heldItems.Spawns.Add(new InvItem("held_wide_lens", true), range, 2);//Wide Lens
                    heldItems.Spawns.Add(new InvItem("held_pierce_band", true), range, 2);//Pierce Band
                    heldItems.Spawns.Add(new InvItem("held_shell_bell", true), range, 1);//Shell Bell
                    heldItems.Spawns.Add(new InvItem("held_scope_lens", true), range, 1);//Scope Lens

                    heldItems.Spawns.Add(new InvItem("held_shed_shell"), new IntRange(0, max_floors), 2);//Shed Shell
                    heldItems.Spawns.Add(new InvItem("held_shed_shell", true), new IntRange(0, max_floors), 1);//Shed Shell

                    heldItems.Spawns.Add(new InvItem("held_x_ray_specs"), new IntRange(0, max_floors), 2);//X-Ray Specs
                    heldItems.Spawns.Add(new InvItem("held_x_ray_specs", true), new IntRange(0, max_floors), 1);//X-Ray Specs

                    heldItems.Spawns.Add(new InvItem("held_goggle_specs"), new IntRange(0, max_floors), 2);//Goggle Specs
                    heldItems.Spawns.Add(new InvItem("held_goggle_specs", true), new IntRange(0, max_floors), 1);//Goggle Specs

                    heldItems.Spawns.Add(new InvItem("held_big_root"), new IntRange(0, max_floors), 2);//Big Root
                    heldItems.Spawns.Add(new InvItem("held_big_root", true), new IntRange(0, max_floors), 1);//Big Root

                    int stickRate = 1;
                    range = new IntRange(0, max_floors);

                    heldItems.Spawns.Add(new InvItem("held_weather_rock"), range, stickRate);//Weather Rock
                    heldItems.Spawns.Add(new InvItem("held_expert_belt"), range, stickRate);//Expert Belt
                    heldItems.Spawns.Add(new InvItem("held_choice_scarf"), range, stickRate);//Choice Scarf
                    heldItems.Spawns.Add(new InvItem("held_choice_specs"), range, stickRate);//Choice Specs
                    heldItems.Spawns.Add(new InvItem("held_choice_band"), range, stickRate);//Choice Band
                    heldItems.Spawns.Add(new InvItem("held_assault_vest"), range, stickRate);//Assault Vest
                    heldItems.Spawns.Add(new InvItem("held_life_orb"), range, stickRate);//Life Orb
                    heldItems.Spawns.Add(new InvItem("held_heal_ribbon"), range, stickRate);//Heal Ribbon


                    heldItems.Spawns.Add(new InvItem("held_warp_scarf"), new IntRange(0, max_floors), 1);//Warp Scarf
                    heldItems.Spawns.Add(new InvItem("held_warp_scarf", true), new IntRange(0, max_floors), 1);//Warp Scarf


                    range = new IntRange(4, max_floors);
                    heldItems.Spawns.Add(new InvItem("held_toxic_orb"), range, 1);//Toxic Orb
                    heldItems.Spawns.Add(new InvItem("held_flame_orb"), range, 1);//Flame Orb
                    heldItems.Spawns.Add(new InvItem("held_sticky_barb"), range, 5);//Sticky Barb
                    heldItems.Spawns.Add(new InvItem("held_ring_target"), range, 1);//Ring Target

                    //machines
                    CategorySpawn<InvItem> machines = new CategorySpawn<InvItem>();
                    machines.SpawnRates.SetRange(7, new IntRange(0, max_floors));
                    itemSpawnZoneStep.Spawns.Add("tms", machines);

                    range = new IntRange(0, max_floors);

                    //tms
                    foreach (string tm_id in IterateTMs(TMClass.Starter | TMClass.Bottom | TMClass.Mid))
                        machines.Spawns.Add(new InvItem(tm_id), range, 2);


                    floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                    //mobs
                    TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                    poolSpawn.Priority = PR_RESPAWN_MOB;

                    poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                    poolSpawn.TeamSizes.Add(2, new IntRange(4, 8), 4);
                    poolSpawn.TeamSizes.Add(2, new IntRange(8, max_floors), 6);
                    floorSegment.ZoneSteps.Add(poolSpawn);

                    TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                    tileSpawn.Priority = PR_RESPAWN_TRAP;
                    tileSpawn.Spawns.Add(new EffectTile("trap_mud", false), new IntRange(0, max_floors), 10);//mud trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_warp", true), new IntRange(0, max_floors), 10);//warp trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_gust", false), new IntRange(0, max_floors), 10);//gust trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, max_floors), 10);//chestnut trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_poison", false), new IntRange(0, max_floors), 10);//poison trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_slumber", false), new IntRange(0, max_floors), 10);//sleep trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_sticky", false), new IntRange(4, max_floors), 10);//sticky trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_seal", false), new IntRange(0, max_floors), 10);//seal trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_self_destruct", false), new IntRange(0, max_floors), 10);//selfdestruct trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_trip", true), new IntRange(0, max_floors), 10);//trip trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_pp_leech", true), new IntRange(4, max_floors), 10);//pp-leech trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_explosion", false), new IntRange(4, max_floors), 10);//explosion trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_slow", false), new IntRange(0, max_floors), 10);//slow trap
                    tileSpawn.Spawns.Add(new EffectTile("trap_spin", false), new IntRange(0, max_floors), 10);//spin trap
                    floorSegment.ZoneSteps.Add(tileSpawn);


                    {
                        //monster houses
                        SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(10, new IntRange(4, 15)));
                        monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHouseStep<ListMapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);

                        monsterChanceZoneStep.Items.Add(new MapItem("evo_fire_stone"), new IntRange(0, max_floors), 4);//Fire Stone
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_thunder_stone"), new IntRange(0, max_floors), 4);//Thunder Stone
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_water_stone"), new IntRange(0, max_floors), 4);//Water Stone
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_moon_stone"), new IntRange(0, max_floors), 4);//Moon Stone
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_dusk_stone"), new IntRange(0, max_floors), 4);//Dusk Stone
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_dawn_stone"), new IntRange(0, max_floors), 4);//Dawn Stone
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_shiny_stone"), new IntRange(0, max_floors), 4);//Shiny Stone
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_leaf_stone"), new IntRange(0, max_floors), 4);//Leaf Stone
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_ice_stone"), new IntRange(0, max_floors), 4);//Ice Stone
                        monsterChanceZoneStep.Items.Add(new MapItem("evo_sun_stone"), new IntRange(0, max_floors), 4);

                        foreach (string key in IterateApricorns(true))
                            monsterChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//apricorns
                        foreach (string key in IterateTMs(TMClass.Natural))
                            monsterChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 2);//TMs

                        PopulateHouseItems(monsterChanceZoneStep, DungeonStage.Rogue, DungeonAccessibility.MainPath, max_floors);

                        monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(1), "loot_star_piece"), new ItemThemeNone(50, new RandRange(2, 4))), new IntRange(0, max_floors), 20);//no theme
                        monsterChanceZoneStep.ItemThemes.Add(new ItemThemeType(ItemData.UseType.Learn, true, true, new RandRange(3, 5)), new IntRange(0, max_floors), 10);//TMs
                        monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(3, 7)), new IntRange(0, max_floors), 30);//gummis
                        monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(RecruitState)), true, true, new RandRange(2, 6)), new IntRange(0, max_floors), 10);//apricorns
                        monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(1), "loot_star_piece"), new ItemStateType(new FlagType(typeof(EvoState)), true, true, new RandRange(2, 4))), new IntRange(0, 10), 30);//evo items
                        monsterChanceZoneStep.MobThemes.Add(new MobThemeNone(0, new RandRange(7, 13)), new IntRange(0, max_floors), 10);
                        floorSegment.ZoneSteps.Add(monsterChanceZoneStep);
                    }

                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(3, 5), new IntRange(0, max_floors)), new MapItem("food_apple"));
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("berry_leppa"));

                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanQuota(new RandRange(1), new IntRange(0, max_floors)), new MapItem("orb_cleanse"));
                    
                    AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(1, max_floors)),
                        new MapItem("apricorn_blue"), new MapItem("apricorn_green"), new MapItem("apricorn_brown"), new MapItem("apricorn_purple"),
                        new MapItem("apricorn_red"), new MapItem("apricorn_white"), new MapItem("apricorn_yellow"), new MapItem("apricorn_black"));

                    AddEvoZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 5), new IntRange(1, max_floors)), EvoRoomType.Diamond);


                    SpawnRangeList<IGenStep> shopZoneSpawns = new SpawnRangeList<IGenStep>();
                    {
                        ShopStep<ListMapGenContext> shop = new ShopStep<ListMapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                        shop.Personality = 2;
                        shop.SecurityStatus = "shop_security";

                        foreach (string tm_id in IterateTMs(TMClass.Top | TMClass.Mid | TMClass.ShopOnly))
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
                    SpreadStepRangeZoneStep shopZoneStep = new SpreadStepRangeZoneStep(new SpreadPlanQuota(new RandRange(1), new IntRange(10, max_floors)), PR_SHOPS, shopZoneSpawns);
                    shopZoneStep.ModStates.Add(new FlagType(typeof(ShopModGenState)));
                    floorSegment.ZoneSteps.Add(shopZoneStep);



                    for (int ii = 0; ii < max_floors; ii++)
                    {
                        RoomFloorGen layout = new RoomFloorGen();

                        //Floor settings
                        if (ii < 4)
                            AddFloorData(layout, "Moonlit Courtyard.ogg", 1200, Map.SightRange.Dark, Map.SightRange.Dark);
                        else if (ii < 8)
                            AddFloorData(layout, "Glacial Path.ogg", 1200, Map.SightRange.Clear, Map.SightRange.Dark);
                        else
                            AddFloorData(layout, "Limestone Cavern.ogg", 1200, Map.SightRange.Dark, Map.SightRange.Dark);


                        //Tilesets
                        //other candidates: crystal_crossing, crystal_cave_2, southern_cavern_2
                        if (ii < 4)
                            AddTextureData(layout, "lapis_cave_wall", "lapis_cave_floor", "lapis_cave_secondary", "rock");
                        else if (ii < 8)
                            AddTextureData(layout, "waterfall_pond_wall", "waterfall_pond_floor", "waterfall_pond_secondary", "ice");
                        else
                            AddTextureData(layout, "crystal_cave_1_wall", "crystal_cave_1_floor", "crystal_cave_1_secondary", "rock");

                        if (ii < 4)
                        {
                            SpawnList<PatternPlan> terrainPattern = new SpawnList<PatternPlan>();
                            terrainPattern.Add(new PatternPlan("pattern_plus", PatternPlan.PatternExtend.Single), 20);
                            AddTerrainPatternSteps(layout, "wall", new RandRange(8, 11), terrainPattern);
                        }
                        else if (ii >= 8)
                        {
                            SpawnList<PatternPlan> terrainPattern = new SpawnList<PatternPlan>();
                            terrainPattern.Add(new PatternPlan("pattern_plus", PatternPlan.PatternExtend.Single), 20);
                            AddTerrainPatternSteps(layout, "water", new RandRange(8, 11), terrainPattern);
                        }
                        if (ii >= 4)
                        {
                            RandRange blob_amt = new RandRange(10, 15);
                            string blob_terrain = "water";
                            IntRange blob_size = new IntRange(2, 6);

                            MultiBlobStencil<ListMapGenContext> multiBlobStencil = new MultiBlobStencil<ListMapGenContext>(false);

                            multiBlobStencil.List.Add(new BlobTilePercentStencil<ListMapGenContext>(40, new MapTerrainStencil<ListMapGenContext>(false, false, true, true)));

                            //not allowed to draw the blob over start or end.
                            multiBlobStencil.List.Add(new StairsStencil<ListMapGenContext>(true));
                            //effect tile checks are also needed since even though they are postproc-shielded, it'll cut off the path to those locations
                            multiBlobStencil.List.Add(new BlobTileStencil<ListMapGenContext>(new TileEffectStencil<ListMapGenContext>(true)));

                            //not allowed to draw the blob such that chokepoints are removed
                            multiBlobStencil.List.Add(new NoChokepointStencil<ListMapGenContext>(new MapTerrainStencil<ListMapGenContext>(false, false, true, true)));

                            //not allowed to draw individual tiles over unbreakable tiles
                            BlobWaterStep<ListMapGenContext> waterStep = new BlobWaterStep<ListMapGenContext>(blob_amt, new Tile(blob_terrain), new MatchTerrainStencil<ListMapGenContext>(true, new Tile("unbreakable")), multiBlobStencil, blob_size, new IntRange(Math.Max(blob_size.Min, 7), Math.Max(blob_size.Max * 3 / 2, 8)));
                            layout.GenSteps.Add(PR_WATER, waterStep);
                            layout.GenSteps.Add(PR_WATER_DIAG, new DropDiagonalBlockStep<ListMapGenContext>(new Tile(blob_terrain)));
                        }

                        //traps
                        AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile

                        AddTrapsSteps(layout, new RandRange(8, 12));
                        if (ii >= 4)
                        {
                            //some patterns
                            SpawnList<PatternPlan> patternList = new SpawnList<PatternPlan>();
                            patternList.Add(new PatternPlan("pattern_teeth", PatternPlan.PatternExtend.Extrapolate), 5);
                            patternList.Add(new PatternPlan("pattern_squiggle", PatternPlan.PatternExtend.Repeat1D), 5);
                            patternList.Add(new PatternPlan("pattern_slash", PatternPlan.PatternExtend.Repeat2D), 5);
                            patternList.Add(new PatternPlan("pattern_crosshair", PatternPlan.PatternExtend.Extrapolate), 5);
                            AddTrapPatternSteps(layout, new RandRange(1, 3), patternList);
                        }

                        //money
                        if (ii < 8)
                            AddMoneyData(layout, new RandRange(4, 9));
                        else
                            AddMoneyData(layout, new RandRange(6, 11));

                        //enemies!
                        AddRespawnData(layout, 20, 100);

                        //enemies
                        if (ii < 4)
                            AddEnemySpawnData(layout, 20, new RandRange(6, 9));
                        else if (ii < 8)
                            AddEnemySpawnData(layout, 20, new RandRange(8, 12));
                        else if (ii < 20)
                            AddEnemySpawnData(layout, 20, new RandRange(10, 15));

                        //items
                        AddItemData(layout, new RandRange(3, 6), 25);

                        if (ii >= 4 && ii <= 10)
                        {
                            SpawnList<MapItem> waterSpawns = new SpawnList<MapItem>();
                            waterSpawns.Add(new MapItem("loot_star_piece"), 50);
                            TerrainSpawnStep<ListMapGenContext, MapItem> waterItemZoneStep = new TerrainSpawnStep<ListMapGenContext, MapItem>(new Tile("water"));
                            waterItemZoneStep.Spawn = new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(waterSpawns, new RandRange(0, 2)));
                            layout.GenSteps.Add(PR_SPAWN_ITEMS, waterItemZoneStep);
                        }

                        //construct paths
                        if (ii < 4)
                        {

                            AddInitListStep(layout, 50, 42);

                            FloorPathBranch<ListMapGenContext> path = new FloorPathBranch<ListMapGenContext>();

                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.FillPercent = new RandRange(35);
                            path.BranchRatio = new RandRange(80);
                            path.HallPercent = 100;

                            SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                            //square
                            genericRooms.Add(new RoomGenSquare<ListMapGenContext>(new RandRange(5), new RandRange(5)), 2);
                            //diamond
                            genericRooms.Add(new RoomGenDiamond<ListMapGenContext>(new RandRange(5), new RandRange(5)), 8);
                            genericRooms.Add(new RoomGenDiamond<ListMapGenContext>(new RandRange(7), new RandRange(7)), 4);
                            genericRooms.Add(new RoomGenDiamond<ListMapGenContext>(new RandRange(9), new RandRange(9)), 2);
                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<ListMapGenContext>(100, new RandRange(1, 6), new RandRange(1, 6)), 10);
                            //genericHalls.Add(new RoomGenAngledHall<ListMapGenContext>(100, new RandRange(1), new RandRange(2, 6)), 10);
                            path.GenericHalls = genericHalls;
                            layout.GenSteps.Add(PR_ROOMS_GEN, path);


                            layout.GenSteps.Add(PR_ROOMS_GEN, CreateGenericListConnect(70, 100));

                            AddTunnelStep<ListMapGenContext> tunneler = new AddTunnelStep<ListMapGenContext>();
                            tunneler.Halls = new RandRange(6, 10);
                            tunneler.TurnLength = new RandRange(4, 9);
                            tunneler.MaxLength = new RandRange(16, 24);
                            layout.GenSteps.Add(PR_TILES_GEN_TUNNEL, tunneler);

                            AddDrawListSteps(layout);
                        }
                        else if (ii < 8)
                        {
                            AddInitListStep(layout, 60, 44);

                            FloorPathBranch<ListMapGenContext> path = new FloorPathBranch<ListMapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallPercent = 50;
                            path.FillPercent = new RandRange(50);
                            path.BranchRatio = new RandRange(25);

                            SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();

                            //oasis cave
                            RoomGenOasis<ListMapGenContext> oasisGen = new RoomGenOasis<ListMapGenContext>(new RandRange(12, 15), new RandRange(12, 15));
                            oasisGen.WaterTerrain = new Tile("water");
                            genericRooms.Add(oasisGen, 4);

                            //cave
                            genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(4, 13), new RandRange(4, 15)), 10);

                            for (int nn = 0; nn < 4; nn++)
                            {
                                bool flipH = nn % 2 == 0;
                                bool flipV = nn / 2 % 2 == 0;
                                genericRooms.Add(new RoomGenTriangle<ListMapGenContext>(new RandRange(3), flipH, flipV), 10);
                                genericRooms.Add(new RoomGenTriangle<ListMapGenContext>(new RandRange(5), flipH, flipV), 8);
                                genericRooms.Add(new RoomGenTriangle<ListMapGenContext>(new RandRange(7), flipH, flipV), 4);
                                genericRooms.Add(new RoomGenTriangle<ListMapGenContext>(new RandRange(9), flipH, flipV), 2);
                            }

                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                            genericHalls.Add(new RoomGenSquare<ListMapGenContext>(new RandRange(1), new RandRange(1)), 10);
                            
                            {
                                RoomGenAngledHall<ListMapGenContext> angledHall = new RoomGenAngledHall<ListMapGenContext>(20, new RandRange(1), new RandRange(1));
                                angledHall.Brush = new SquareHallBrush(new Loc(2));
                                genericHalls.Add(angledHall, 10);
                            }

                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_ROOMS_GEN, path);

                            layout.GenSteps.Add(PR_ROOMS_GEN, CreateGenericListConnect(60, 0));

                            AddDrawListSteps(layout);
                        }
                        else
                        {
                            AddInitListStep(layout, 60, 44, true);

                            FloorPathBranch<ListMapGenContext> path = new FloorPathBranch<ListMapGenContext>();
                            path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            path.HallPercent = 50;
                            path.FillPercent = new RandRange(50);
                            path.BranchRatio = new RandRange(25);

                            SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();

                            //oasis cave
                            RoomGenOasis<ListMapGenContext> oasisGen = new RoomGenOasis<ListMapGenContext>(new RandRange(6, 13), new RandRange(6, 13));
                            oasisGen.WaterTerrain = new Tile("water");
                            genericRooms.Add(oasisGen, 4);

                            //diamond
                            genericRooms.Add(new RoomGenDiamond<ListMapGenContext>(new RandRange(5), new RandRange(5)), 8);
                            genericRooms.Add(new RoomGenDiamond<ListMapGenContext>(new RandRange(7), new RandRange(7)), 4);
                            genericRooms.Add(new RoomGenDiamond<ListMapGenContext>(new RandRange(9), new RandRange(9)), 2);

                            for (int nn = 0; nn < 4; nn++)
                            {
                                bool flipH = nn % 2 == 0;
                                bool flipV = nn / 2 % 2 == 0;
                                genericRooms.Add(new RoomGenTriangle<ListMapGenContext>(new RandRange(3), flipH, flipV), 10);
                                genericRooms.Add(new RoomGenTriangle<ListMapGenContext>(new RandRange(5), flipH, flipV), 8);
                                genericRooms.Add(new RoomGenTriangle<ListMapGenContext>(new RandRange(7), flipH, flipV), 4);
                                genericRooms.Add(new RoomGenTriangle<ListMapGenContext>(new RandRange(9), flipH, flipV), 2);
                            }

                            path.GenericRooms = genericRooms;

                            SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                            genericHalls.Add(new RoomGenAngledHall<ListMapGenContext>(100, new RandRange(1, 6), new RandRange(1, 6)), 10);

                            path.GenericHalls = genericHalls;

                            layout.GenSteps.Add(PR_ROOMS_GEN, path);

                            layout.GenSteps.Add(PR_ROOMS_GEN, CreateGenericListConnect(65, 0));

                            //AddTunnelStep<ListMapGenContext> tunneler = new AddTunnelStep<ListMapGenContext>();
                            //tunneler.Halls = new RandRange(6, 10);
                            //tunneler.TurnLength = new RandRange(4, 9);
                            //tunneler.MaxLength = new RandRange(16, 24);
                            //layout.GenSteps.Add(PR_TILES_GEN_TUNNEL, tunneler);

                            //draw paths
                            layout.GenSteps.Add(PR_TILES_INIT, new DrawFloorToTileStep<ListMapGenContext>());
                        }

                        AddStairStep(layout, true);

                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<ListMapGenContext, MapGenEntrance, MapGenExit>());

                        floorSegment.Floors.Add(layout);
                    }

                    {
                        LoadGen layout = new LoadGen();
                        MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                        startGen.MapID = "end_geode_crevice";
                        layout.GenSteps.Add(PR_FILE_LOAD, startGen);

                        MapTimeLimitStep<MapLoadContext> floorData = new MapTimeLimitStep<MapLoadContext>(600);
                        layout.GenSteps.Add(PR_FLOOR_DATA, floorData);

                        AddTextureData(layout, "waterfall_pond_wall", "waterfall_pond_floor", "waterfall_pond_secondary", "rock");

                        layout.GenSteps.Add(PR_SPAWN_ITEMS, new ScriptGenStep<MapLoadContext>("GeodeCreviceRevisit"));

                        {
                            SpawnList<MapItem> treasure = new SpawnList<MapItem>();
                            treasure.Add(new MapItem("seed_golden"), 5);
                            treasure.Add(new MapItem("ammo_golden_thorn"), 10);
                            treasure.Add(new MapItem("loot_nugget"), 10);
                            treasure.Add(new MapItem("loot_heart_scale"), 10);
                            treasure.Add(new MapItem("apricorn_glittery"), 10);
                            PickerSpawner<MapLoadContext, MapItem> spawn = new PickerSpawner<MapLoadContext, MapItem>(new LoopedRand<MapItem>(treasure, new RandRange(2)));
                            BoxSpawner<MapLoadContext> box = new BoxSpawner<MapLoadContext>("box_glittery", spawn);
                            List<Loc> treasureLocs = new List<Loc>();
                            treasureLocs.Add(new Loc(4, 9));
                            treasureLocs.Add(new Loc(16, 9));
                            layout.GenSteps.Add(PR_SPAWN_ITEMS, new SpecificSpawnStep<MapLoadContext, MapItem>(box, treasureLocs));
                        }

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
            }
            #endregion
        }

        static void FillSicklyHollow(ZoneData zone, bool translate)
        {
            #region SICKLY HOLLOW
            {
                zone.Name = new LocalText("Sickly Hollow");
                zone.Rescues = 2;
                zone.Level = 25;
                zone.LevelCap = true;
                zone.KeepSkills = true;
                zone.BagRestrict = 16;
                zone.KeepTreasure = true;
                zone.TeamSize = 3;
                zone.Rogue = RogueStatus.NoTransfer;

                int max_floors = 16;

                LayeredSegment floorSegment = new LayeredSegment();
                floorSegment.IsRelevant = true;
                floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Sickly Hollow\nB{0}F"), new Priority(-15)));

                //money
                MoneySpawnZoneStep moneySpawnZoneStep = new MoneySpawnZoneStep(PR_RESPAWN_MONEY, new RandRange(30, 40), new RandRange(30, 34));
                moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                //items
                ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;

                //necesities
                CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                necessities.SpawnRates.SetRange(16, new IntRange(0, max_floors));
                itemSpawnZoneStep.Spawns.Add("necessities", necessities);

                necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 25);//Leppa
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
                orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
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
                heldItems.SpawnRates.SetRange(4, new IntRange(0, max_floors));
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


                //mobs
                TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                poolSpawn.Priority = PR_RESPAWN_MOB;

                // 188 Skiploom : Infiltrator : 78 Stun Spore : 235 Synthesis : 584 Fairy Wind
                poolSpawn.Spawns.Add(GetTeamMob("skiploom", "infiltrator", "stun_spore", "synthesis", "fairy_wind", "", new RandRange(24), TeamMemberSpawn.MemberRole.Support), new IntRange(0, 5), 10);
                // 189 Jumpluff : Infiltrator : 79 Sleep Powder : 73 Leech Seed
                poolSpawn.Spawns.Add(GetTeamMob("jumpluff", "infiltrator", "sleep_powder", "leech_seed", "", "", new RandRange(30), TeamMemberSpawn.MemberRole.Support), new IntRange(5, 10), 10);
                // 315 Roselia : 38 Poison Point : 92 Toxic : 73 Leech Seed
                poolSpawn.Spawns.Add(GetTeamMob("roselia", "poison_point", "toxic", "leech_seed", "", "", new RandRange(24)), new IntRange(0, 5), 10);
                // 315 Roselia : 38 Poison Point : 92 Toxic : 73 Leech Seed
                poolSpawn.Spawns.Add(GetTeamMob("roselia", "poison_point", "toxic", "leech_seed", "", "", new RandRange(33), TeamMemberSpawn.MemberRole.Support), new IntRange(10, max_floors), 10);
                // 200 Misdreavus : 109 Confuse Ray : 212 Mean Look : 506 Hex 
                poolSpawn.Spawns.Add(GetTeamMob("misdreavus", "", "confuse_ray", "mean_look", "hex", "", new RandRange(24)), new IntRange(0, 5), 10);
                poolSpawn.Spawns.Add(GetTeamMob("misdreavus", "", "confuse_ray", "mean_look", "hex", "", new RandRange(28)), new IntRange(5, 10), 10);
                // 429 Mismagius : 174 Curse : 220 Pain Split : 595 Mystical Fire
                poolSpawn.Spawns.Add(GetTeamMob("mismagius", "", "curse", "pain_split", "mystical_fire", "", new RandRange(33)), new IntRange(10, max_floors), 10);
                // 53 Persian : 127 Unnerve : 415 Switcheroo : 269 Taunt
                {
                    TeamMemberSpawn mob = GetTeamMob("persian", "unnerve", "switcheroo", "taunt", "", "", new RandRange(28), "thief");
                    mob.Spawn.SpawnFeatures.Add(new MobSpawnItem(true, "held_flame_orb", "held_ring_target"));
                    poolSpawn.Spawns.Add(mob, new IntRange(5, 10), 10);
                }
                // 453 Croagunk : 269 Taunt : 207 Swagger : 279 Revenge
                poolSpawn.Spawns.Add(GetTeamMob("croagunk", "", "taunt", "swagger", "revenge", "", new RandRange(24)), new IntRange(0, 5), 10);
                poolSpawn.Spawns.Add(GetTeamMob("croagunk", "", "taunt", "swagger", "revenge", "", new RandRange(28)), new IntRange(5, 10), 10);
                // 454 Toxicroak : 269 Taunt : 426 Mud Bomb : 279 Revenge
                poolSpawn.Spawns.Add(GetTeamMob("toxicroak", "", "taunt", "mud_bomb", "revenge", "", new RandRange(35)), new IntRange(10, max_floors), 10);
                // 355 Duskull : 50 Disable : 101 Night Shade
                poolSpawn.Spawns.Add(GetTeamMob("duskull", "", "disable", "night_shade", "", "", new RandRange(24)), new IntRange(0, 5), 10);
                poolSpawn.Spawns.Add(GetTeamMob("duskull", "", "disable", "night_shade", "", "", new RandRange(28)), new IntRange(5, 10), 10);
                // 336 Seviper : 151 Infiltrator : 305 Poison Fang : 474 Venoshock
                poolSpawn.Spawns.Add(GetTeamMob("seviper", "infiltrator", "poison_fang", "venoshock", "", "", new RandRange(28)), new IntRange(5, 10), 10);
                // 336 Seviper : 151 Infiltrator : 305 Poison Fang : 380 Gastro Acid : 599 Venom Drench
                poolSpawn.Spawns.Add(GetTeamMob("seviper", "infiltrator", "poison_fang", "gastro_acid", "venom_drench", "", new RandRange(33)), new IntRange(10, max_floors), 10);
                // 41 Zubat : 44 Bite : 141 Leech Life : 48 Supersonic
                poolSpawn.Spawns.Add(GetTeamMob("zubat", "", "bite", "leech_life", "supersonic", "", new RandRange(28)), new IntRange(5, 10), 10);
                // 42 Golbat : 151 Infiltrator : 305 Poison Fang : 109 Confuse Ray : 212 Mean Look
                poolSpawn.Spawns.Add(GetTeamMob("golbat", "infiltrator", "poison_fang", "confuse_ray", "mean_look", "", new RandRange(33)), new IntRange(10, max_floors), 10);
                // 37 Vulpix : 506 Hex : 261 Will-O-Wisp
                poolSpawn.Spawns.Add(GetTeamMob("vulpix", "", "hex", "will_o_wisp", "", "", new RandRange(24)), new IntRange(0, 5), 10);
                poolSpawn.Spawns.Add(GetTeamMob("vulpix", "", "hex", "will_o_wisp", "", "", new RandRange(30)), new IntRange(5, 10), 10);
                // 15 Beedrill : 390 Toxic Spikes : 41 Twineedle
                poolSpawn.Spawns.Add(GetTeamMob("beedrill", "", "toxic_spikes", "twineedle", "", "", new RandRange(34)), new IntRange(10, max_floors), 10);
                // 12 Butterfree : 14 Compound Eyes : 78 Stun Spore : 79 Sleep powder : 77 Poison powder : 093 Confusion
                poolSpawn.Spawns.Add(GetTeamMob("butterfree", "compound_eyes", "stun_spore", "sleep_powder", "poison_powder", "confusion", new RandRange(25)), new IntRange(0, 5), 10);
                // 198 Murkrow : 228 Pursuit : 372 Assurance
                poolSpawn.Spawns.Add(GetTeamMob("murkrow", "", "pursuit", "assurance", "", "", new RandRange(24)), new IntRange(0, 5), 10);
                poolSpawn.Spawns.Add(GetTeamMob("murkrow", "", "pursuit", "assurance", "", "", new RandRange(30)), new IntRange(5, 10), 10);
                // 407 Roserade : 38 Poison Point : 599 Venom Drench : 73 Leech Seed : 202 Giga Drain
                poolSpawn.Spawns.Add(GetTeamMob("roserade", "poison_point", "venom_drench", "leech_seed", "giga_drain", "", new RandRange(34), TeamMemberSpawn.MemberRole.Leader), new IntRange(10, max_floors), 10);
                //457 Lumineon : 114 Storm Drain : 487 Soak : 352 Water Pulse : 445 Captivate
                poolSpawn.Spawns.Add(GetTeamMob("lumineon", "storm_drain", "soak", "water_pulse", "captivate", "", new RandRange(28)), new IntRange(5, 10), 10);

                //206 Dunsparce : 50 Run Away : 99 Rage : 228 Pursuit : 36 Take Down
                poolSpawn.Spawns.Add(GetTeamMob("dunsparce", "run_away", "rage", "pursuit", "take_down", "", new RandRange(28)), new IntRange(5, 10), 10);
                //206 Dunsparce : 32 Serene Grace : 355 Roost : 228 Pursuit : 246 Ancient Power
                poolSpawn.Spawns.Add(GetTeamMob("dunsparce", "serene_grace", "roost", "pursuit", "ancient_power", "", new RandRange(33)), new IntRange(10, max_floors), 10);

                //163 Hoothoot : Growl : Reflect : Confusion
                poolSpawn.Spawns.Add(GetTeamMob("hoothoot", "", "growl", "reflect", "confusion", "", new RandRange(24), TeamMemberSpawn.MemberRole.Support), new IntRange(0, 5), 10);
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


                // required item spreads
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 6), new IntRange(8, max_floors)), new MapItem("machine_assembly_box"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(3, 6), new IntRange(0, max_floors)), new MapItem("food_apple"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("berry_leppa"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanQuota(new RandRange(1), new IntRange(0, 5)), new MapItem("key", 1));


                {
                    //monster houses
                    SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(20, new IntRange(1, 15)));
                    monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHouseStep<ListMapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);

                    foreach (string iter_item in IterateApricorns(true))
                        monsterChanceZoneStep.Items.Add(new MapItem(iter_item), new IntRange(0, max_floors), 4);//apricorns

                    PopulateHouseItems(monsterChanceZoneStep, DungeonStage.Advanced, DungeonAccessibility.Unlockable, max_floors);

                    monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(1, 4), "loot_pearl"), new ItemThemeNone(30, new RandRange(2, 4))), new IntRange(0, max_floors), 20);//no theme

                    monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(3, 7)), new IntRange(0, max_floors), 30);//gummis
                    monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(RecruitState)), true, true, new RandRange(2, 6)), new IntRange(0, 10), 10);//apricorns
                    monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(1, 4), "loot_pearl"), new ItemStateType(new FlagType(typeof(EvoState)), true, true, new RandRange(2, 4))), new IntRange(0, 10), 10);//evo items
                                                                                                                                                                                                                                                                //mobs
                    monsterChanceZoneStep.MobThemes.Add(new MobThemeNone(0, new RandRange(7, 13)), new IntRange(0, max_floors), 10);
                    floorSegment.ZoneSteps.Add(monsterChanceZoneStep);
                }


                {
                    SpreadVaultZoneStep vaultChanceZoneStep = new SpreadVaultZoneStep(PR_SPAWN_ITEMS_EXTRA, PR_SPAWN_TRAPS, PR_SPAWN_MOBS_EXTRA, new SpreadPlanQuota(new RandDecay(1, 4, 50), new IntRange(1, max_floors)));

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
                        vaultChanceZoneStep.Items.Add(new MapItem("evo_reaper_cloth"), new IntRange(0, max_floors), 150);//reaper cloth
                    }

                    PopulateVaultItems(vaultChanceZoneStep, DungeonStage.Advanced, DungeonAccessibility.Unlockable, max_floors, true);

                    floorSegment.ZoneSteps.Add(vaultChanceZoneStep);
                }


                SpawnRangeList<IGenStep> shopZoneSpawns = new SpawnRangeList<IGenStep>();
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

                    shop.Items.Add(new MapItem("evo_fire_stone", 0, 2000), 10);//Fire Stone
                    shop.Items.Add(new MapItem("evo_dusk_stone", 0, 2000), 10);//Dusk Stone
                    shop.Items.Add(new MapItem("evo_shiny_stone", 0, 2000), 10);//Shiny Stone
                    shop.Items.Add(new MapItem("evo_reaper_cloth", 0, 3000), 10);//Reaper Cloth

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

                    shopZoneSpawns.Add(shop, new IntRange(2, max_floors), 10);
                }
                SpreadStepRangeZoneStep shopZoneStep = new SpreadStepRangeZoneStep(new SpreadPlanQuota(new RandDecay(0, 4, 35), new IntRange(2, max_floors)), PR_SHOPS, shopZoneSpawns);
                shopZoneStep.ModStates.Add(new FlagType(typeof(ShopModGenState)));
                floorSegment.ZoneSteps.Add(shopZoneStep);

                AddEvoZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(1, max_floors)), EvoRoomType.Normal);

                AddHiddenStairStep(floorSegment, new SpreadPlanQuota(new RandDecay(1, 6, 30), new IntRange(0, max_floors - 1)), 1);

                AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 2);


                for (int ii = 0; ii < max_floors; ii++)
                {
                    GridFloorGen layout = new GridFloorGen();

                    //Floor settings
                    if (ii < 10)
                        AddFloorData(layout, "Sickly Hollow.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);
                    else
                        AddFloorData(layout, "Sickly Hollow 2.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

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

                    SpawnList<MapItem> wallSpawns = new SpawnList<MapItem>();
                    PopulateWallItems(wallSpawns, DungeonStage.Advanced, DungeonEnvironment.Forest);

                    TerrainSpawnStep<MapGenContext, MapItem> wallItemZoneStep = new TerrainSpawnStep<MapGenContext, MapItem>(new Tile("wall"));
                    wallItemZoneStep.Spawn = new PickerSpawner<MapGenContext, MapItem>(new LoopedRand<MapItem>(wallSpawns, new RandRange(6, 10)));
                    layout.GenSteps.Add(PR_SPAWN_ITEMS, wallItemZoneStep);


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

                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                    floorSegment.Floors.Add(layout);
                }

                {
                    LoadGen layout = new LoadGen();
                    MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                    startGen.MapID = "end_sickly_hollow";
                    layout.GenSteps.Add(PR_FILE_LOAD, startGen);

                    MapTimeLimitStep<MapLoadContext> floorData = new MapTimeLimitStep<MapLoadContext>(600);
                    layout.GenSteps.Add(PR_FLOOR_DATA, floorData);

                    AddTextureData(layout, "mystery_jungle_2_wall", "mystery_jungle_2_floor", "mystery_jungle_2_secondary", "poison");

                    //add 2 chests
                    {
                        HashSet<string> exceptFor = new HashSet<string>();
                        foreach (string legend in IterateLegendaries())
                            exceptFor.Add(legend);
                        SpeciesItemElementSpawner<MapLoadContext> spawn = new SpeciesItemElementSpawner<MapLoadContext>(new IntRange(2), new RandRange(2), "poison", exceptFor);
                        BoxSpawner<MapLoadContext> box = new BoxSpawner<MapLoadContext>("box_heavy", spawn);
                        List<Loc> treasureLocs = new List<Loc>();
                        treasureLocs.Add(new Loc(6, 6));
                        treasureLocs.Add(new Loc(8, 6));
                        layout.GenSteps.Add(PR_SPAWN_ITEMS, new SpecificSpawnStep<MapLoadContext, MapItem>(box, treasureLocs));
                    }

                    List<InvItem> treasure1 = new List<InvItem>();
                    treasure1.Add(InvItem.CreateBox("box_glittery", "seed_golden"));
                    treasure1.Add(InvItem.CreateBox("box_glittery", "ammo_golden_thorn"));//Golden Thorn
                    treasure1.Add(InvItem.CreateBox("box_glittery", "loot_nugget"));//Nugget
                    treasure1.Add(InvItem.CreateBox("box_glittery", "apricorn_glittery"));

                    List<(List<InvItem>, Loc)> items = new List<(List<InvItem>, Loc)>();
                    items.Add((treasure1, new Loc(7, 8)));
                    AddSpecificSpawnPool(layout, items, PR_SPAWN_ITEMS);

                    floorSegment.Floors.Add(layout);
                }

                zone.Segments.Add(floorSegment);
            }

            {
                SingularSegment structure = new SingularSegment(-1);

                SpawnList<TeamMemberSpawn> enemyList = new SpawnList<TeamMemberSpawn>();
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 21, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 22, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 23, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 24, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 25, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 26, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 27, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                structure.BaseFloor = getSecretRoom(translate, "special_gsc_ghost", -1, "mystery_jungle_2_wall", "mystery_jungle_2_floor", "mystery_jungle_2_secondary", "tall_grass_dark", "poison", DungeonStage.Advanced, DungeonAccessibility.Unlockable, enemyList, new Loc(7, 6));

                zone.Segments.Add(structure);
            }

            {
                SingularSegment structure = new SingularSegment(-1);

                ChanceFloorGen multiGen = new ChanceFloorGen();
                string unown = "toxin";
                multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, unown, DungeonStage.Advanced, MysteryRoomType.SmallSquare, -2, false, false), 10);
                multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, unown, DungeonStage.Advanced, MysteryRoomType.TallHall, -2, false, false), 10);
                multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, unown, DungeonStage.Advanced, MysteryRoomType.WideHall, -2, false, false), 10);
                structure.BaseFloor = multiGen;

                zone.Segments.Add(structure);
            }
            #endregion
        }

        static void FillEnergyGarden(ZoneData zone, bool translate)
        {
            #region ENERGY GARDEN
            {
                zone.Name = new LocalText("**Energy Garden");
                zone.Level = 35;
                zone.LevelCap = true;
                zone.Rescues = 2;
                zone.BagRestrict = 8;
                zone.KeepTreasure = true;
                zone.TeamSize = 2;
                zone.Rogue = RogueStatus.NoTransfer;

                {
                    int max_floors = 12;
                    LayeredSegment floorSegment = new LayeredSegment();
                    floorSegment.IsRelevant = true;
                    floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                    floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Energy Garden\nB{0}F"), new Priority(-15)));

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

                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                        floorSegment.Floors.Add(layout);
                    }

                    zone.Segments.Add(floorSegment);
                }
            }
            #endregion
        }

        static void FillRelicTower(ZoneData zone, bool translate)
        {
            #region RELIC TOWER

            zone.Name = new LocalText("Relic Tower");
            zone.Rescues = 2;
            zone.Level = 35;
            zone.LevelCap = true;
            zone.KeepSkills = true;
            zone.Rogue = RogueStatus.ItemTransfer;
            {
                int max_floors = 13;
                LayeredSegment floorSegment = new LayeredSegment();
                floorSegment.IsRelevant = true;
                floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Relic Tower\n{0}F"), new Priority(-15)));

                //money
                MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level + 40, 0);
                moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                floorSegment.ZoneSteps.Add(moneySpawnZoneStep);
                //items
                ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;


                //necessities
                CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                necessities.SpawnRates.SetRange(5, new IntRange(0, max_floors));
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
                evo.SpawnRates.SetRange(3, new IntRange(0, max_floors));
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


                held.Spawns.Add(new InvItem("held_heal_ribbon", true), new IntRange(0, max_floors), 3);
                held.Spawns.Add(new InvItem("held_heal_ribbon"), new IntRange(0, max_floors), 7);
                held.Spawns.Add(new InvItem("held_assault_vest", true), new IntRange(0, max_floors), 3);
                held.Spawns.Add(new InvItem("held_assault_vest"), new IntRange(0, max_floors), 7);
                held.Spawns.Add(new InvItem("held_pass_scarf", true), new IntRange(0, max_floors), 3);
                held.Spawns.Add(new InvItem("held_pass_scarf"), new IntRange(0, max_floors), 7);
                held.Spawns.Add(new InvItem("held_x_ray_specs", true), new IntRange(0, max_floors), 3);
                held.Spawns.Add(new InvItem("held_x_ray_specs"), new IntRange(0, max_floors), 7);
                held.Spawns.Add(new InvItem("held_soft_sand", true), new IntRange(0, max_floors), 3);
                held.Spawns.Add(new InvItem("held_soft_sand"), new IntRange(0, max_floors), 7);
                held.Spawns.Add(new InvItem("held_twisted_spoon", true), new IntRange(0, max_floors), 3);
                held.Spawns.Add(new InvItem("held_twisted_spoon"), new IntRange(0, max_floors), 7);
                held.Spawns.Add(new InvItem("held_life_orb", true), new IntRange(0, max_floors), 10);
                held.Spawns.Add(new InvItem("held_goggle_specs", true), new IntRange(0, max_floors), 3);
                held.Spawns.Add(new InvItem("held_goggle_specs"), new IntRange(0, max_floors), 7);


                floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


                //mobs
                TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
                poolSpawn.Priority = PR_RESPAWN_MOB;


                poolSpawn.Spawns.Add(GetTeamMob("baltoy", "", "cosmic_power", "mud_slap", "heal_block", "", new RandRange(32), "wander_normal"), new IntRange(0, 4), 10);
                poolSpawn.Spawns.Add(GetTeamMob("braixen", "magician", "flame_charge", "lucky_chant", "", "", new RandRange(32), "thief"), new IntRange(0, 4), 10);
                poolSpawn.Spawns.Add(GetTeamMob("meditite", "", "meditate", "force_palm", "", "", new RandRange(32), "wander_normal"), new IntRange(0, 4), 10);
                poolSpawn.Spawns.Add(GetTeamMob("ariados", "", "night_shade", "spider_web", "", "", new RandRange(32), "wander_normal"), new IntRange(0, 6), 10);
                poolSpawn.Spawns.Add(GetTeamMob("larvesta", "", "ember", "string_shot", "leech_life", "", new RandRange(32), "wander_normal"), new IntRange(0, 4), 10);
                poolSpawn.Spawns.Add(GetTeamMob("gothorita", "", "future_sight", "play_nice", "", "", new RandRange(33), "wander_normal"), new IntRange(2, 6), 10);
                poolSpawn.Spawns.Add(GetTeamMob("wobbuffet", "", "mirror_coat", "counter", "encore", "", new RandRange(40), "wander_normal"), new IntRange(6, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("shuppet", "cursed_body", "spite", "curse", "", "", new RandRange(35), "wander_normal"), new IntRange(4, 8), 10);
                poolSpawn.Spawns.Add(GetTeamMob("solrock", "", "fire_spin", "embargo", "rock_slide", "", new RandRange(38), "wander_normal"), new IntRange(4, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("meowstic", "", "psyshock", "charm", "", "", new RandRange(35), "wander_normal"), new IntRange(4, 8), 5);
                poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("meowstic", 1, "", Gender.Unknown), "", "psyshock", "charge_beam", "", "", new RandRange(34), "wander_normal"), new IntRange(4, 8), 5);
                poolSpawn.Spawns.Add(GetTeamMob("bronzong", "", "psywave", "imprison", "", "", new RandRange(42), "wander_normal"), new IntRange(8, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("electrode", "", "rollout", "self_destruct", "light_screen", "", new RandRange(38), "wander_normal"), new IntRange(6, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("claydol", "", "heal_block", "psybeam", "teleport", "", new RandRange(42), "retreater"), new IntRange(8, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("banette", "cursed_body", "grudge", "shadow_ball", "", "", new RandRange(38), "wander_normal"), new IntRange(6, 10), 10);
                poolSpawn.Spawns.Add(GetTeamMob("gothitelle", "", "psychic", "future_sight", "", "", new RandRange(42), "wander_normal"), new IntRange(8, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("delphox", "magician", "switcheroo", "mystical_fire", "psyshock", "", new RandRange(42), "wander_normal"), new IntRange(8, max_floors), 10);

                {

                    MobSpawn mob = GetGuardMob("espeon", "", "psychic", "", "", "", new RandRange(55), "wander_normal", "sleep");
                    MobSpawnItem keySpawn = new MobSpawnItem(true);
                    keySpawn.Items.Add(new InvItem("held_choice_scarf"), 10);
                    mob.SpawnFeatures.Add(keySpawn);

                    SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                    specificTeam.Spawns.Add(mob);
                    LoopedTeamSpawner<ListMapGenContext> spawner = new LoopedTeamSpawner<ListMapGenContext>(specificTeam, new RandRange(1));

                    SpawnRangeList<IGenStep> specialEnemySpawns = new SpawnRangeList<IGenStep>();
                    specialEnemySpawns.Add(new PlaceRandomMobsStep<ListMapGenContext>(spawner), new IntRange(0, max_floors), 10);
                    SpreadStepRangeZoneStep specialEnemyStep = new SpreadStepRangeZoneStep(new SpreadPlanQuota(new RandDecay(0, 1, 50), new IntRange(0, max_floors - 1)), PR_SPAWN_MOBS, specialEnemySpawns);
                    floorSegment.ZoneSteps.Add(specialEnemyStep);
                }

                poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                poolSpawn.TeamSizes.Add(2, new IntRange(0, max_floors), 4);


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


                {
                    //monster houses
                    SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(15, new IntRange(0, max_floors)));
                    monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHouseStep<ListMapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);

                    foreach (string iter_item in IterateApricorns(true))
                        monsterChanceZoneStep.Items.Add(new MapItem(iter_item), new IntRange(0, max_floors), 2);//apricorns
                    foreach (string iter_item in IterateTypePlates())
                        monsterChanceZoneStep.Items.Add(new MapItem(iter_item), new IntRange(0, max_floors), 5);//type plates

                    PopulateHouseItems(monsterChanceZoneStep, DungeonStage.Intermediate, DungeonAccessibility.Hidden, max_floors);

                    monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(1, 4), "loot_pearl"), new ItemThemeNone(40, new RandRange(2, 4))), new IntRange(0, max_floors), 30);//no theme

                    monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(3, 7)), new IntRange(0, max_floors), 20);//gummis
                    monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(RecruitState)), true, true, new RandRange(2, 6)), new IntRange(0, 10), 10);//apricorns
                    monsterChanceZoneStep.MobThemes.Add(new MobThemeNone(40, new RandRange(7, 13)), new IntRange(0, max_floors), 10);
                    floorSegment.ZoneSteps.Add(monsterChanceZoneStep);
                }


                {
                    SpreadHouseZoneStep chestChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanQuota(new RandDecay(1, 8, 60), new IntRange(4, max_floors)));
                    chestChanceZoneStep.ModStates.Add(new FlagType(typeof(ChestModGenState)));
                    chestChanceZoneStep.HouseStepSpawns.Add(new ChestStep<ListMapGenContext>(false, GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);

                    chestChanceZoneStep.Items.Add(new MapItem("evo_link_cable"), new IntRange(0, max_floors), 15);

                    PopulateChestItems(chestChanceZoneStep, DungeonStage.Intermediate, DungeonAccessibility.Hidden, false, max_floors);

                    floorSegment.ZoneSteps.Add(chestChanceZoneStep);
                }


                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 20), new IntRange(0, max_floors)), new MapItem("food_grimy"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("berry_leppa"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(3, max_floors)), new MapItem("machine_assembly_box"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(max_floors / 2, max_floors - 1), new IntRange(0, max_floors)), new MapItem("orb_cleanse"));


                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)),
                    new MapItem("apricorn_brown"), new MapItem("apricorn_purple"), new MapItem("apricorn_red"));


                SpawnRangeList<IGenStep> shopZoneSpawns = new SpawnRangeList<IGenStep>();
                {
                    ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
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

                        PopulateVaultItems(detourChanceZoneStep, DungeonStage.Intermediate, DungeonAccessibility.Hidden, max_floors, true, true);

                        // trap spawnings for the vault
                        {
                            //StepSpawner <- PresetMultiRand
                            MultiStepSpawner<ListMapGenContext, EffectTile> mainSpawner = new MultiStepSpawner<ListMapGenContext, EffectTile>();
                            EffectTile secretTile = new EffectTile("stairs_secret_down", true);
                            secretTile.TileStates.Set(new DestState(new SegLoc(1, 0), true));
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

                        }

                        PopulateVaultItems(vaultChanceZoneStep, DungeonStage.Advanced, DungeonAccessibility.Hidden, max_floors, true);

                        combinedVaultZoneStep.Steps.Add(vaultChanceZoneStep);
                    }

                    floorSegment.ZoneSteps.Add(combinedVaultZoneStep);
                }


                {
                    SpreadBossZoneStep bossChanceZoneStep = new SpreadBossZoneStep(PR_ROOMS_GEN_EXTRA, PR_SPAWN_ITEMS_EXTRA, new SpreadPlanQuota(new RandDecay(1, 10, 60), new IntRange(1, max_floors)));

                    {
                        ResizeFloorStep<ListMapGenContext> addSizeStep = new ResizeFloorStep<ListMapGenContext>(new Loc(16, 16), Dir8.None);
                        bossChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT, addSizeStep));
                        ClampFloorStep<ListMapGenContext> limitStep = new ClampFloorStep<ListMapGenContext>(new Loc(0), new Loc(78, 54));
                        bossChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT, limitStep));
                        ClampFloorStep<ListMapGenContext> clampStep = new ClampFloorStep<ListMapGenContext>();
                        bossChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT_CLAMP, clampStep));
                    }

                    //BOSS TEAMS
                    // no specific items to be used in lv5 dungeons

                    bossChanceZoneStep.BossSteps.Add(getBossRoomStep<ListMapGenContext>("discharge", 0, 31, 2, 3, 20), new IntRange(0, max_floors), 10);
                    bossChanceZoneStep.BossSteps.Add(getBossRoomStep<ListMapGenContext>("eeveelution_2", 0, 31, 2, 3, 20), new IntRange(0, max_floors), 10);
                    bossChanceZoneStep.BossSteps.Add(getBossRoomStep<ListMapGenContext>("eeveelution_1", 0, 31, 2, 3, 20), new IntRange(0, max_floors), 10);
                    bossChanceZoneStep.BossSteps.Add(getBossRoomStep<ListMapGenContext>("sun_altar", 0, 35, 1, 1, 20), new IntRange(0, max_floors), 10);
                    bossChanceZoneStep.BossSteps.Add(getBossRoomStep<ListMapGenContext>("celestial", 0, 35, 1, 1, 20), new IntRange(0, max_floors), 10);
                    bossChanceZoneStep.BossSteps.Add(getBossRoomStep<ListMapGenContext>("charm", 0, 35, 1, 1, 20), new IntRange(0, max_floors), 10);
                    bossChanceZoneStep.BossSteps.Add(getBossRoomStep<ListMapGenContext>("psychic", 0, 35, 1, 1, 20), new IntRange(0, max_floors), 10);
                    bossChanceZoneStep.BossSteps.Add(getBossRoomStep<ListMapGenContext>("trapper", 0, 35, 1, 1, 20), new IntRange(0, max_floors), 10);

                    //sealing the boss room and treasure room
                    {
                        BossSealStep<ListMapGenContext> vaultStep = new BossSealStep<ListMapGenContext>("sealed_block", "tile_boss");
                        vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                        vaultStep.Filters.Add(new RoomFilterIndex(false, 0));
                        vaultStep.BossFilters.Add(new RoomFilterComponent(false, new BossRoom()));
                        vaultStep.BossFilters.Add(new RoomFilterIndex(false, 0));
                        bossChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_TILES_GEN_EXTRA, vaultStep));
                    }

                    PopulateBossItems(bossChanceZoneStep, DungeonStage.Advanced, DungeonAccessibility.Hidden, max_floors);

                    floorSegment.ZoneSteps.Add(bossChanceZoneStep);
                }

                AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 2);


                for (int ii = 0; ii < max_floors; ii++)
                {
                    GridFloorGen layout = new GridFloorGen();

                    //Floor settings
                    if (ii < 4)
                        AddFloorData(layout, "Relic Tower.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);
                    else
                        AddFloorData(layout, "Relic Tower.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                    //Tilesets
                    if (ii < 8)
                        AddTextureData(layout, "temporal_unused_wall", "temporal_unused_floor", "temporal_unused_secondary", "psychic");
                    else
                        AddTextureData(layout, "buried_relic_3_wall", "buried_relic_3_floor", "buried_relic_3_secondary", "psychic");

                    SpawnList<PatternPlan> terrainPattern = new SpawnList<PatternPlan>();
                    terrainPattern.Add(new PatternPlan("pattern_blob", PatternPlan.PatternExtend.Single), 5);
                    terrainPattern.Add(new PatternPlan("pattern_double_colon", PatternPlan.PatternExtend.Single), 10);
                    terrainPattern.Add(new PatternPlan("pattern_plus", PatternPlan.PatternExtend.Single), 20);
                    AddTerrainPatternSteps(layout, "wall", new RandRange(2, 6), terrainPattern);

                    if (ii < 4)
                        AddBlobWaterSteps(layout, "water", new RandRange(1, 4), new IntRange(2, 6), true);

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                    AddTrapsSteps(layout, new RandRange(6, 9));

                    //money - Ballpark 25K
                    AddMoneyData(layout, new RandRange(2, 4));

                    //enemies
                    AddRespawnData(layout, 11, 100);
                    AddEnemySpawnData(layout, 20, new RandRange(8, 11));

                    //items
                    AddItemData(layout, new RandRange(3, 6), 25);

                    SpawnList<MapItem> wallSpawns = new SpawnList<MapItem>();
                    PopulateWallItems(wallSpawns, DungeonStage.Intermediate, DungeonEnvironment.Rock);

                    TerrainSpawnStep<MapGenContext, MapItem> wallItemZoneStep = new TerrainSpawnStep<MapGenContext, MapItem>(new Tile("wall"));
                    wallItemZoneStep.Spawn = new PickerSpawner<MapGenContext, MapItem>(new LoopedRand<MapItem>(wallSpawns, new RandRange(6, 10)));
                    layout.GenSteps.Add(PR_SPAWN_ITEMS, wallItemZoneStep);

                    //construct paths
                    {
                        //100% Square Path

                        AddInitGridStep(layout, 5, 4, 9, 9);

                        GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.RoomRatio = new RandRange(100);
                        path.BranchRatio = new RandRange(0, 25);

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //plus
                        genericRooms.Add(new RoomGenPlus<MapGenContext>(new RandRange(9), new RandRange(9), new RandRange(3)), 5);
                        genericRooms.Add(new RoomGenPlus<MapGenContext>(new RandRange(7), new RandRange(7), new RandRange(2)), 5);
                        //Square
                        genericRooms.Add(new RoomGenSquare<MapGenContext>(new RandRange(5), new RandRange(5)), 5);
                        genericRooms.Add(new RoomGenSquare<MapGenContext>(new RandRange(7), new RandRange(7)), 5);
                        //diamond
                        genericRooms.Add(new RoomGenDiamond<MapGenContext>(new RandRange(7), new RandRange(7)), 5);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(0, new ColumnHallBrush()), 10);
                        if (ii < 8)
                            genericHalls.Add(new RoomGenAngledHall<MapGenContext>(0), 5);

                        path.GenericHalls = genericHalls;
                        layout.GenSteps.Add(PR_GRID_GEN, path);

                        layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 0));

                    }

                    AddDrawGridSteps(layout);

                    AddStairStep(layout, false);


                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                    floorSegment.Floors.Add(layout);
                }


                {
                    LoadGen layout = new LoadGen();
                    MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                    startGen.MapID = "end_relic_tower";
                    layout.GenSteps.Add(PR_FILE_LOAD, startGen);

                    MapTimeLimitStep<MapLoadContext> floorData = new MapTimeLimitStep<MapLoadContext>(600);
                    layout.GenSteps.Add(PR_FLOOR_DATA, floorData);

                    AddTextureData(layout, "buried_relic_3_wall", "buried_relic_3_floor", "buried_relic_3_secondary", "psychic");

                    {
                        HashSet<string> exceptFor = new HashSet<string>();
                        foreach (string legend in IterateLegendaries())
                            exceptFor.Add(legend);
                        SpeciesItemElementSpawner<MapLoadContext> spawn = new SpeciesItemElementSpawner<MapLoadContext>(new IntRange(2), new RandRange(2), "psychic", exceptFor);
                        BoxSpawner<MapLoadContext> box = new BoxSpawner<MapLoadContext>("box_heavy", spawn);
                        List<Loc> treasureLocs = new List<Loc>();
                        treasureLocs.Add(new Loc(6, 8));
                        treasureLocs.Add(new Loc(8, 8));
                        layout.GenSteps.Add(PR_SPAWN_ITEMS, new SpecificSpawnStep<MapLoadContext, MapItem>(box, treasureLocs));
                    }

                    List<InvItem> treasure1 = new List<InvItem>();
                    treasure1.Add(InvItem.CreateBox("box_glittery", "seed_golden"));
                    treasure1.Add(InvItem.CreateBox("box_glittery", "ammo_golden_thorn"));//Golden Thorn
                    treasure1.Add(InvItem.CreateBox("box_glittery", "loot_nugget"));//Nugget
                    treasure1.Add(InvItem.CreateBox("box_glittery", "apricorn_glittery"));

                    List<(List<InvItem>, Loc)> items = new List<(List<InvItem>, Loc)>();
                    items.Add((treasure1, new Loc(7, 11)));
                    AddSpecificSpawnPool(layout, items, PR_SPAWN_ITEMS);

                    floorSegment.Floors.Add(layout);
                }

                zone.Segments.Add(floorSegment);
            }

            {
                SingularSegment structure = new SingularSegment(-1);

                SpawnList<TeamMemberSpawn> enemyList = new SpawnList<TeamMemberSpawn>();
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 7, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 8, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 9, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 10, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 11, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 12, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 13, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                structure.BaseFloor = getSecretRoom(translate, "special_gsc_ghost", -1, "buried_relic_3_wall", "buried_relic_3_floor", "buried_relic_3_secondary", "", "psychic", DungeonStage.Advanced, DungeonAccessibility.Hidden, enemyList, new Loc(7, 6));

                zone.Segments.Add(structure);
            }


            {
                SingularSegment structure = new SingularSegment(-1);

                ChanceFloorGen multiGen = new ChanceFloorGen();
                string unown = "knowledge";
                multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, unown, DungeonStage.Intermediate, MysteryRoomType.SmallSquare, -2, false, false), 10);
                multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, unown, DungeonStage.Intermediate, MysteryRoomType.TallHall, -2, false, false), 10);
                multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, unown, DungeonStage.Intermediate, MysteryRoomType.WideHall, -2, false, false), 10);
                structure.BaseFloor = multiGen;

                zone.Segments.Add(structure);
            }

            #endregion
        }
    }
}
