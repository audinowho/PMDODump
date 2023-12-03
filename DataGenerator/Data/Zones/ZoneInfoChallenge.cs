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
        static void FillCastawayCave(ZoneData zone)
        {
            #region CASTAWAY CAVE
            {
                zone.Name = new LocalText("Castaway Cave");
                zone.Rescues = 2;
                zone.Level = 30;
                zone.Rogue = RogueStatus.ItemTransfer;
                zone.Persistent = true;

                int max_floors = 12;
                LayeredSegment floorSegment = new LayeredSegment();
                floorSegment.IsRelevant = true;
                floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Castaway Cave\nB{0}F"), new Priority(-15)));

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

                poolSpawn.Spawns.Add(GetTeamMob("woobat", "", "heart_stamp", "confusion", "", "", new RandRange(23), "wander_dumb_itemless"), new IntRange(0, 4), 10);
                poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("qwilfish", 1, "", Gender.Unknown), "", "pin_missile", "spikes", "", "", new RandRange(24), "wander_dumb_itemless"), new IntRange(4, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("makuhita", "", "fake_out", "whirlwind", "", "", new RandRange(25), "wander_dumb_itemless"), new IntRange(4, 10), 10);
                //poolSpawn.Spawns.Add(GetTeamMob("anorith", "", "ancient_power", "bug_bite", "", "", new RandRange(29), "wander_dumb_itemless"), new IntRange(8, max_floors), 10);
                //poolSpawn.Spawns.Add(GetTeamMob("lileep", "", "acid", "ingrain", "", "", new RandRange(29), "wander_dumb_itemless"), new IntRange(8, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("honedge", "", "autotomize", "shadow_sneak", "", "", new RandRange(28), "wander_dumb_itemless"), new IntRange(6, max_floors), 5);
                poolSpawn.Spawns.Add(GetTeamMob("bronzor", "", "psywave", "imprison", "", "", new RandRange(27), "wander_dumb_itemless"), new IntRange(6, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob(new MonsterID("shellos", 1, "", Gender.Unknown), "", "mud_bomb", "hidden_power", "", "", new RandRange(24), "wander_dumb_itemless"), new IntRange(2, 8), 10);
                poolSpawn.Spawns.Add(GetTeamMob("goldeen", "", "horn_attack", "water_pulse", "", "", new RandRange(23), "wander_dumb_itemless"), new IntRange(0, 6), 10);
                poolSpawn.Spawns.Add(GetTeamMob("seel", "", "icy_wind", "encore", "", "", new RandRange(22), "wander_dumb_itemless"), new IntRange(0, 6), 10);
                poolSpawn.Spawns.Add(GetTeamMob("mantyke", "", "wing_attack", "bubble_beam", "", "", new RandRange(23), "wander_dumb_itemless"), new IntRange(2, 8), 10);
                poolSpawn.Spawns.Add(GetTeamMob("horsea", "", "smokescreen", "twister", "", "", new RandRange(24), "wander_dumb_itemless"), new IntRange(2, 8), 10);
                poolSpawn.Spawns.Add(GetTeamMob("carvanha", "", "bite", "screech", "", "", new RandRange(24), "wander_dumb_itemless"), new IntRange(0, 8), 10);
                poolSpawn.Spawns.Add(GetTeamMob("corphish", "", "knock_off", "bubble", "", "", new RandRange(27), "wander_dumb_itemless"), new IntRange(6, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("tentacool", "", "acid_spray", "wrap", "", "", new RandRange(28), "wander_dumb_itemless"), new IntRange(6, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("corsola", "", "spike_cannon", "lucky_chant", "ancient_power", "", new RandRange(29), "wander_dumb_itemless"), new IntRange(8, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("chinchou", "", "confuse_ray", "spark", "", "", new RandRange(26), "wander_dumb_itemless"), new IntRange(4, 10), 10);

                {
                    TeamMemberSpawn teamSpawn = GetTeamMob("mantine", "", "wide_guard", "bubble_beam", "", "", new RandRange(30), "wander_normal");
                    teamSpawn.Spawn.SpawnConditions.Add(new MobCheckSaveVar("castaway_cave.TookTreasure", true));
                    poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 100);
                }
                {
                    TeamMemberSpawn teamSpawn = GetTeamMob("huntail", "", "ice_fang", "sucker_punch", "dive", "", new RandRange(30), "wander_normal");
                    teamSpawn.Spawn.SpawnConditions.Add(new MobCheckSaveVar("castaway_cave.TookTreasure", true));
                    poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 100);
                }
                {
                    TeamMemberSpawn teamSpawn = GetTeamMob("gorebyss", "", "amnesia", "draining_kiss", "dive", "", new RandRange(30), "wander_normal");
                    teamSpawn.Spawn.SpawnConditions.Add(new MobCheckSaveVar("castaway_cave.TookTreasure", true));
                    poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 100);
                }
                {
                    TeamMemberSpawn teamSpawn = GetTeamMob("dhelmise", "", "anchor_shot", "giga_drain", "metal_sound", "", new RandRange(32), "wander_normal");
                    teamSpawn.Spawn.SpawnConditions.Add(new MobCheckSaveVar("castaway_cave.TookTreasure", true));
                    poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 50);
                }

                poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
                poolSpawn.TeamSizes.Add(2, new IntRange(8, max_floors), 3);

                floorSegment.ZoneSteps.Add(poolSpawn);

                TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
                tileSpawn.Priority = PR_RESPAWN_TRAP;


                tileSpawn.Spawns.Add(new EffectTile("trap_trip", false), new IntRange(0, max_floors), 10);//trip trap
                tileSpawn.Spawns.Add(new EffectTile("trap_slow", false), new IntRange(0, max_floors), 10);//slow trap
                tileSpawn.Spawns.Add(new EffectTile("trap_chestnut", false), new IntRange(0, max_floors), 10);//chestnut trap
                tileSpawn.Spawns.Add(new EffectTile("trap_gust", false), new IntRange(0, max_floors), 10);//gust trap
                tileSpawn.Spawns.Add(new EffectTile("trap_grimy", false), new IntRange(0, max_floors), 10);//grimy trap

                floorSegment.ZoneSteps.Add(tileSpawn);



                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 8), new IntRange(0, max_floors)), new MapItem("food_apple"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("berry_leppa"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanQuota(new RandDecay(2, 8, 50), new IntRange(0, max_floors)), new MapItem("key", 1));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(3, max_floors)), new MapItem("machine_assembly_box"));

                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(3, 6), new IntRange(0, max_floors)),
                    new MapItem("apricorn_brown"), new MapItem("apricorn_white"), new MapItem("apricorn_yellow"), new MapItem("apricorn_black"));

                {

                    MobSpawn mob = GetGuardMob("vaporeon", "", "aqua_ring", "acid_armor", "muddy_water", "", new RandRange(40), "wander_normal", "sleep");
                    MobSpawnItem keySpawn = new MobSpawnItem(true);
                    keySpawn.Items.Add(new InvItem("held_shell_bell"), 10);
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
                    SpreadHouseZoneStep chestChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanQuota(new RandBinomial(8, 60), new IntRange(4, max_floors - 1)));
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

                AddEvoZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(3, 6), new IntRange(1, max_floors - 1)), false);

                for (int ii = 0; ii < max_floors; ii++)
                {
                    GridFloorGen layout = new GridFloorGen();

                    //Floor settings
                    AddFloorData(layout, "B23. Castaway Cave.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                    MapEffectStep<MapGenContext> takeTreasure = new MapEffectStep<MapGenContext>();
                    takeTreasure.Effect.OnMapStarts.Add(-20, new SingleCharScriptEvent("CastawayCaveAltMusic"));
                    takeTreasure.Effect.OnMapStarts.Add(-15, new SingleCharScriptEvent("CastawayCaveAltEnemies"));
                    if (ii == max_floors - 1)
                    {
                        takeTreasure.Effect.OnPickups.Add(0, new ItemScriptEvent("CastawayCaveShift"));
                        takeTreasure.Effect.OnEquips.Add(0, new ItemScriptEvent("CastawayCaveShift"));
                    }
                    layout.GenSteps.Add(PR_FLOOR_DATA, takeTreasure);

                    //Tilesets
                    if (ii < 6)
                        AddTextureData(layout, "miracle_sea_wall", "miracle_sea_floor", "miracle_sea_secondary", "water");
                    else
                        AddTextureData(layout, "stormy_sea_1_wall", "stormy_sea_1_floor", "stormy_sea_1_secondary", "water");

                    if (ii < 6)
                        AddWaterSteps(layout, "water", new RandRange(30), false);//water
                    else if (ii < 10)
                        AddWaterSteps(layout, "water", new RandRange(25), false);//water
                    else
                        AddWaterSteps(layout, "water", new RandRange(35), false);//water

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                    AddTrapsSteps(layout, new RandRange(8, 12));

                    //money
                    AddMoneyData(layout, new RandRange(3, 6));

                    //all floors
                    {
                        //063 Abra : 100 Teleport
                        //always holds a TM
                        MobSpawn mob = GetGenericMob("clamperl", "", "clamp", "whirlpool", "iron_defense", "", new RandRange(26), "turret");
                        MobSpawnItem keySpawn = new MobSpawnItem(true);
                        keySpawn.Items.Add(new InvItem("loot_pearl", false, 1), 10);
                        mob.SpawnFeatures.Add(keySpawn);
                        SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                        specificTeam.Spawns.Add(mob);

                        LoopedTeamSpawner<MapGenContext> spawner = new LoopedTeamSpawner<MapGenContext>(specificTeam);
                        {
                            spawner.AmountSpawner = new RandDecay(0, 2, 50);
                        }
                        PlaceRandomMobsStep<MapGenContext> secretMobPlacement = new PlaceRandomMobsStep<MapGenContext>(spawner);
                        layout.GenSteps.Add(PR_SPAWN_MOBS, secretMobPlacement);
                    }

                    //enemies
                    if (ii < 3)
                    {
                        AddRespawnData(layout, 5, 50);
                        AddEnemySpawnData(layout, 20, new RandRange(3, 6));
                    }
                    else if (ii < 6)
                    {
                        AddRespawnData(layout, 6, 50);
                        AddEnemySpawnData(layout, 20, new RandRange(3, 7));
                    }
                    else
                    {
                        AddRespawnData(layout, 7, 50);
                        AddEnemySpawnData(layout, 20, new RandRange(4, 8));
                    }



                    if (ii >= 10)
                    {
                        SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                        specificTeam.Spawns.Add(GetGenericMob("dratini", "", "twister", "dragon_rage", "", "", new RandRange(25), "wander_normal"));

                        LoopedTeamSpawner<MapGenContext> spawner = new LoopedTeamSpawner<MapGenContext>(specificTeam);
                        {
                            spawner.AmountSpawner = new RandRange(1, 3);
                        }
                        PlaceDisconnectedMobsStep<MapGenContext> secretMobPlacement = new PlaceDisconnectedMobsStep<MapGenContext>(spawner);
                        secretMobPlacement.AcceptedTiles.Add(new Tile("water"));
                        layout.GenSteps.Add(PR_SPAWN_MOBS, secretMobPlacement);
                    }

                    layout.GenSteps.Add(PR_SPAWN_ITEMS, new ScriptGenStep<MapGenContext>("CastawayCaveRevisit"));

                    //items
                    AddItemData(layout, new RandRange(3, 6), 25);

                    List<MapItem> specificSpawns = new List<MapItem>();
                    if (ii == 0)
                        specificSpawns.Add(new MapItem("berry_leppa"));//leppa berry

                    RandomRoomSpawnStep<MapGenContext, MapItem> specificItemZoneStep = new RandomRoomSpawnStep<MapGenContext, MapItem>(new PickerSpawner<MapGenContext, MapItem>(new PresetMultiRand<MapItem>(specificSpawns)));
                    specificItemZoneStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                    layout.GenSteps.Add(PR_SPAWN_ITEMS, specificItemZoneStep);


                    //construct paths
                    {
                        //3-tier Merged Maze

                        if (ii < 3)
                            AddInitGridStep(layout, 9, 7, 4, 4);
                        else if (ii < 6)
                            AddInitGridStep(layout, 10, 7, 4, 4);
                        else if (ii < 10)
                            AddInitGridStep(layout, 10, 8, 4, 4);
                        else
                            AddInitGridStep(layout, 11, 8, 4, 4);

                        GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.RoomRatio = new RandRange(80);
                        path.BranchRatio = new RandRange(0, 35);

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //round
                        genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(4), new RandRange(4)), 10);
                        //cave
                        genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(5, 7), new RandRange(5, 7)), 10);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(100), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_GRID_GEN, path);

                        if (ii == max_floors - 1)
                        {
                            CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(1), GetImmutableFilterList());
                            step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));

                            RoomGenLoadMap<MapGenContext> loadRoom = new RoomGenLoadMap<MapGenContext>();
                            loadRoom.MapID = "room_castaway_cave_altar";
                            loadRoom.RoomTerrain = new Tile("floor");
                            loadRoom.PreventChanges = PostProcType.Terrain;
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 3), loadRoom), 10);
                            layout.GenSteps.Add(PR_GRID_GEN, step);
                        }

                        if (ii >= 6)
                        {
                            CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(1, 3), GetImmutableFilterList());
                            step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            step.Filters.Add(new RoomFilterDefaultGen(true));
                            step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(3), new RoomGenCave<MapGenContext>(new RandRange(9, 15), new RandRange(9, 15))), 10);
                            layout.GenSteps.Add(PR_GRID_GEN, step);
                        }

                        {
                            CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(2, 5), GetImmutableFilterList());
                            if (ii >= 6)
                                step.MergeRate = new RandRange(8, 11);
                            step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            step.Filters.Add(new RoomFilterDefaultGen(true));
                            step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 3), new RoomGenBump<MapGenContext>(new RandRange(6, 9), new RandRange(9, 15), new RandRange(30, 70))), 10);
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(3, 2), new RoomGenBump<MapGenContext>(new RandRange(9, 15), new RandRange(6, 9), new RandRange(30, 70))), 10);
                            layout.GenSteps.Add(PR_GRID_GEN, step);
                        }
                        {
                            CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(8, 11), GetImmutableFilterList());
                            step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            step.Filters.Add(new RoomFilterDefaultGen(true));
                            step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2), new RoomGenBump<MapGenContext>(new RandRange(6, 9), new RandRange(6, 9), new RandRange(30, 70))), 10);
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenRound<MapGenContext>(new RandRange(4), new RandRange(6, 9))), 5);
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenRound<MapGenContext>(new RandRange(6, 9), new RandRange(4))), 5);
                            layout.GenSteps.Add(PR_GRID_GEN, step);
                        }

                    }

                    AddDrawGridSteps(layout);

                    {
                        var step = new FloorStairsStep<MapGenContext, MapGenEntrance, MapGenExit>();
                        step.Entrances.Add(new MapGenEntrance(Dir8.Down));

                        EffectTile returnTile = new EffectTile("stairs_back_up", true);
                        DestState returnDest = new DestState(new SegLoc(0, -1), true);
                        returnDest.PreserveMusic = true;
                        returnTile.TileStates.Set(returnDest);
                        step.Exits.Add(new MapGenExit(returnTile));

                        if (ii < max_floors - 1)
                        {
                            EffectTile exitTile = new EffectTile("stairs_go_down", true);
                            DestState exitDest = new DestState(new SegLoc(0, 1), true);
                            exitDest.PreserveMusic = true;
                            exitTile.TileStates.Set(exitDest);
                            step.Exits.Add(new MapGenExit(exitTile));
                        }

                        step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        step.Filters.Add(new RoomFilterComponent(true, new BossRoom()));
                        layout.GenSteps.Add(PR_EXITS, step);
                    }



                    //layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());
                    //if (ii == max_floors - 1)
                    //    layout.GenSteps.Add(PR_DBG_CHECK, new DetectItemStep<MapGenContext>("egg_mystery"));


                    floorSegment.Floors.Add(layout);
                }

                zone.Segments.Add(floorSegment);
            }
            #endregion
        }

        static void FillSleepingCaldera(ZoneData zone)
        {
            #region SLEEPING CALDERA
            {
                zone.Name = new LocalText("Sleeping Caldera");
                zone.Rescues = 2;
                zone.Level = 35;
                zone.BagRestrict = 0;
                zone.Rogue = RogueStatus.ItemTransfer;
                zone.Persistent = true;

                int max_floors = 12;
                LayeredSegment floorSegment = new LayeredSegment();
                floorSegment.IsRelevant = true;
                floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Sleeping Caldera\nB{0}F"), new Priority(-15)));

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


                poolSpawn.Spawns.Add(GetTeamMob("croagunk", "dry_skin", "feint_attack", "revenge", "", "", new RandRange(29), "wander_dumb_itemless"), new IntRange(0, 6), 10);
                poolSpawn.Spawns.Add(GetTeamMob("barboach", "", "amnesia", "water_gun", "", "", new RandRange(29), "wander_dumb_itemless"), new IntRange(0, max_floors), 10);

                {
                    TeamMemberSpawn teamSpawn = GetTeamMob("nidorino", "", "horn_attack", "fury_attack", "", "", new RandRange(29), "wander_dumb_itemless");
                    teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(0, 2));
                    poolSpawn.Spawns.Add(teamSpawn, new IntRange(6, max_floors), 10);
                }
                {
                    TeamMemberSpawn teamSpawn = GetTeamMob("nidorina", "", "helping_hand", "bite", "", "", new RandRange(29), "wander_dumb_itemless");
                    teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(1, 2));
                    poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, 6), 10);
                }

                poolSpawn.Spawns.Add(GetTeamMob("grimer", "", "sludge_bomb", "disable", "", "", new RandRange(29), "wander_dumb_itemless"), new IntRange(6, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("stunky", "", "poison_gas", "fury_swipes", "", "", new RandRange(29), "wander_dumb_itemless"), new IntRange(0, 6), 10);
                poolSpawn.Spawns.Add(GetTeamMob("poliwhirl", "", "hypnosis", "water_sport", "double_slap", "", new RandRange(29), "wander_dumb_itemless"), new IntRange(0, 6), 10);
                poolSpawn.Spawns.Add(GetTeamMob("dunsparce", "", "yawn", "ancient_power", "", "", new RandRange(29), "wander_dumb_itemless"), new IntRange(0, max_floors), 10);
                poolSpawn.Spawns.Add(GetTeamMob("quilava", "", "flame_wheel", "", "", "", new RandRange(29), "wander_dumb_itemless"), new IntRange(0, max_floors), 10);

                {
                    TeamMemberSpawn teamSpawn = GetTeamMob("camerupt", "", "rock_slide", "lava_plume", "", "", new RandRange(33), "wander_normal");
                    teamSpawn.Spawn.SpawnConditions.Add(new MobCheckSaveVar("sleeping_caldera.TookTreasure", true));
                    poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 100);
                }
                {
                    TeamMemberSpawn teamSpawn = GetTeamMob("magmar", "", "fire_punch", "", "", "", new RandRange(33), "wander_normal");
                    teamSpawn.Spawn.SpawnConditions.Add(new MobCheckSaveVar("sleeping_caldera.TookTreasure", true));
                    poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 100);
                }
                {
                    TeamMemberSpawn teamSpawn = GetTeamMob("pignite", "", "heat_crash", "rollout", "", "", new RandRange(31), "wander_normal");
                    teamSpawn.Spawn.SpawnConditions.Add(new MobCheckSaveVar("sleeping_caldera.TookTreasure", true));
                    poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 100);
                }
                {
                    TeamMemberSpawn teamSpawn = GetTeamMob("slugma", "", "incinerate", "harden", "", "", new RandRange(33), "wander_normal");
                    teamSpawn.Spawn.SpawnConditions.Add(new MobCheckSaveVar("sleeping_caldera.TookTreasure", true));
                    poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 100);
                }
                {
                    TeamMemberSpawn teamSpawn = GetTeamMob(new MonsterID("marowak", 1, "", Gender.Unknown), "", "shadow_bone", "will_o_wisp", "", "", new RandRange(33), "wander_normal");
                    teamSpawn.Spawn.SpawnConditions.Add(new MobCheckSaveVar("sleeping_caldera.TookTreasure", true));
                    poolSpawn.Spawns.Add(teamSpawn, new IntRange(0, max_floors), 100);
                }

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

                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 8), new IntRange(0, max_floors)), new MapItem("food_apple"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("berry_leppa"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanQuota(new RandDecay(2, 8, 50), new IntRange(0, max_floors)), new MapItem("key", 1));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(3, max_floors)), new MapItem("machine_assembly_box"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(max_floors / 2, max_floors - 1), new IntRange(0, max_floors)), new MapItem("orb_cleanse"));

                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)),
                    new MapItem("apricorn_blue"), new MapItem("apricorn_white"), new MapItem("apricorn_purple"), new MapItem("apricorn_red"));

                {

                    MobSpawn mob = GetGuardMob("flareon", "", "flare_blitz", "", "", "", new RandRange(50), "wander_normal", "sleep");
                    MobSpawnItem keySpawn = new MobSpawnItem(true);
                    keySpawn.Items.Add(new InvItem("held_choice_band"), 10);
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


                {
                    SpreadVaultZoneStep vaultChanceZoneStep = new SpreadVaultZoneStep(PR_SPAWN_ITEMS_EXTRA, PR_SPAWN_TRAPS, PR_SPAWN_MOBS_EXTRA, new SpreadPlanQuota(new RandDecay(1, 8, 50), new IntRange(1, max_floors - 1)));

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
                        vaultChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear", 1), new IntRange(0, max_floors), 100);//amber tear
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

                    floorSegment.ZoneSteps.Add(vaultChanceZoneStep);
                }

                for (int ii = 0; ii < max_floors; ii++)
                {
                    RoomFloorGen layout = new RoomFloorGen();


                    //Floor settings
                    AddFloorData(layout, "B07. Flyaway Cliffs.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                    if (ii < 8)
                        AddDefaultMapStatus(layout, "default_weather", "rain", "clear", "clear", "clear", "clear");

                    MapEffectStep<ListMapGenContext> takeTreasure = new MapEffectStep<ListMapGenContext>();
                    takeTreasure.Effect.OnMapStarts.Add(-20, new SingleCharScriptEvent("SleepingCalderaAltData"));
                    takeTreasure.Effect.OnMapStarts.Add(-15, new SingleCharScriptEvent("SleepingCalderaAltTiles"));
                    takeTreasure.Effect.OnMapStarts.Add(-15, new SingleCharScriptEvent("SleepingCalderaAltEnemies"));
                    takeTreasure.Effect.OnMapStarts.Add(5, new SingleCharScriptEvent("SleepingCalderaSummonHeatran"));
                    takeTreasure.Effect.AfterActions.Add(5, new BattleScriptEvent("LegendRecruitCheck"));
                    if (ii == max_floors - 1)
                    {
                        takeTreasure.Effect.OnPickups.Add(0, new ItemScriptEvent("SleepingCalderaShift"));
                        takeTreasure.Effect.OnEquips.Add(0, new ItemScriptEvent("SleepingCalderaShift"));
                    }
                    layout.GenSteps.Add(PR_FLOOR_DATA, takeTreasure);

                    //Tilesets
                    //other candidates: steam_cave
                    //solar_cave_1/deep_dark_crater,waterfall_cave/dark_crater,unused_steam_cave/magma_cavern_3
                    string block, ground, water, lava, element;
                    if (ii < 6)
                    {
                        block = "rock_path_rb_wall";
                        ground = "rock_path_rb_floor";
                        water = "rock_path_rb_secondary";
                        lava = "dark_crater_secondary";
                        element = "fire";
                    }
                    else
                    {
                        block = "waterfall_cave_wall";
                        ground = "waterfall_cave_floor";
                        water = "waterfall_cave_secondary";
                        lava = "deep_dark_crater_secondary";
                        element = "fire";
                    }


                    MapDictTextureStep<ListMapGenContext> textureStep = new MapDictTextureStep<ListMapGenContext>();
                    {
                        textureStep.BlankBG = block;
                        textureStep.TextureMap["floor"] = ground;
                        textureStep.TextureMap["unbreakable"] = block;
                        textureStep.TextureMap["wall"] = block;
                        textureStep.TextureMap["water"] = water;
                        textureStep.TextureMap["lava"] = lava;
                    }
                    textureStep.GroundElement = element;
                    textureStep.LayeredGround = true;
                    layout.GenSteps.Add(PR_TEXTURES, textureStep);

                    //add water cracks
                    AddTunnelStep<ListMapGenContext> tunneler = new AddTunnelStep<ListMapGenContext>();
                    tunneler.Halls = new RandRange(2, 5);
                    tunneler.TurnLength = new RandRange(3, 8);
                    tunneler.MaxLength = new RandRange(25);
                    tunneler.Brush = new TerrainHallBrush(Loc.One, new Tile("water"));
                    layout.GenSteps.Add(PR_WATER, tunneler);

                    AddBlobWaterSteps(layout, "water", new RandRange(2, 5), new IntRange(2, 8), true);//water

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile
                    AddTrapsSteps(layout, new RandRange(8, 12));

                    //money
                    AddMoneyData(layout, new RandRange(3, 7));

                    if (ii >= 3 && ii < 9)
                    {
                        SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                        MobSpawn mob = GetGenericMob("snorlax", "", "rest", "body_slam", "amnesia", "", new RandRange(38), "wander_normal", true);

                        HashSet<string> exceptFor = new HashSet<string>();
                        foreach (string legend in IterateLegendaries())
                            exceptFor.Add(legend);
                        MobSpawnExclElement itemSpawn = new MobSpawnExclElement("box_light", exceptFor, new IntRange(1), true);
                        mob.SpawnFeatures.Add(itemSpawn);

                        specificTeam.Spawns.Add(mob);

                        LoopedTeamSpawner<ListMapGenContext> spawner = new LoopedTeamSpawner<ListMapGenContext>(specificTeam);
                        {
                            spawner.AmountSpawner = new RandRange(0, 2);
                        }
                        PlaceRandomMobsStep<ListMapGenContext> secretMobPlacement = new PlaceRandomMobsStep<ListMapGenContext>(spawner);
                        secretMobPlacement.Filters.Add(new RoomFilterHall(false));
                        secretMobPlacement.ClumpFactor = 20;
                        secretMobPlacement.IncludeHalls = true;
                        layout.GenSteps.Add(PR_SPAWN_MOBS, secretMobPlacement);
                    }

                    AddRespawnData(layout, 14, 160);
                    if (ii < 3)
                        AddEnemySpawnData(layout, 20, new RandRange(5, 9));
                    else if (ii < 6)
                        AddEnemySpawnData(layout, 20, new RandRange(6, 10));
                    else
                        AddEnemySpawnData(layout, 20, new RandRange(7, 11));

                    //items
                    AddItemData(layout, new RandRange(3, 7), 25);

                    //construct paths
                    {
                        if (ii < 6)
                            AddInitListStep(layout, 52, 40);
                        else if (ii < max_floors - 1)
                            AddInitListStep(layout, 58, 46);
                        else
                            AddInitListStep(layout, 52, 38);

                        FloorPathBranch<ListMapGenContext> path = new FloorPathBranch<ListMapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallPercent = 50;
                        if (ii < 6)
                            path.FillPercent = new RandRange(50);
                        else if (ii < max_floors - 1)
                            path.FillPercent = new RandRange(55);
                        else
                            path.FillPercent = new RandRange(20);

                        if (ii < max_floors - 1)
                            path.BranchRatio = new RandRange(0, 25);
                        else
                            path.BranchRatio = new RandRange(0);

                        SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                        if (ii >= 6)
                            genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(12, 15), new RandRange(12, 15)), 10);
                        //cave
                        genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(4, 15), new RandRange(4, 15)), 10);
                        //round
                        genericRooms.Add(new RoomGenRound<ListMapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 5);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                        genericHalls.Add(new RoomGenSquare<ListMapGenContext>(new RandRange(1), new RandRange(1)), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_ROOMS_GEN, path);

                        layout.GenSteps.Add(PR_ROOMS_GEN, CreateGenericListConnect(50, 100));

                    }

                    AddDrawListSteps(layout);

                    {
                        var step = new FloorStairsStep<ListMapGenContext, MapGenEntrance, MapGenExit>();
                        step.Entrances.Add(new MapGenEntrance(Dir8.Down));

                        EffectTile returnTile = new EffectTile("stairs_back_up", true);
                        DestState returnDest = new DestState(new SegLoc(0, -1), true);
                        returnDest.PreserveMusic = true;
                        returnTile.TileStates.Set(returnDest);
                        step.Exits.Add(new MapGenExit(returnTile));

                        if (ii < max_floors - 1)
                        {
                            EffectTile exitTile = new EffectTile("stairs_go_down", true);
                            DestState exitDest = new DestState(new SegLoc(0, 1), true);
                            exitDest.PreserveMusic = true;
                            exitTile.TileStates.Set(exitDest);
                            step.Exits.Add(new MapGenExit(exitTile));
                        }

                        step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        step.Filters.Add(new RoomFilterComponent(true, new BossRoom()));
                        layout.GenSteps.Add(PR_EXITS, step);
                    }



                    if (ii == max_floors - 1)
                    {
                        //making room for the vault
                        {
                            ResizeFloorStep<ListMapGenContext> addSizeStep = new ResizeFloorStep<ListMapGenContext>(new Loc(20, 16), Dir8.None);
                            layout.GenSteps.Add(PR_ROOMS_PRE_VAULT, addSizeStep);
                            ClampFloorStep<ListMapGenContext> limitStep = new ClampFloorStep<ListMapGenContext>(new Loc(0), new Loc(78, 54));
                            layout.GenSteps.Add(PR_ROOMS_PRE_VAULT, limitStep);
                            ClampFloorStep<ListMapGenContext> clampStep = new ClampFloorStep<ListMapGenContext>();
                            layout.GenSteps.Add(PR_ROOMS_PRE_VAULT_CLAMP, clampStep);
                        }

                        //vault rooms
                        {
                            SpawnList<RoomGen<ListMapGenContext>> detourRooms = new SpawnList<RoomGen<ListMapGenContext>>();

                            RoomGenLoadMap<ListMapGenContext> loadRoom = new RoomGenLoadMap<ListMapGenContext>();
                            loadRoom.MapID = "room_sleeping_caldera_altar";
                            loadRoom.RoomTerrain = new Tile("floor");
                            loadRoom.PreventChanges = PostProcType.Terrain;
                            detourRooms.Add(loadRoom, 10);
                            SpawnList<PermissiveRoomGen<ListMapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                            detourHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                            AddConnectedRoomsStep<ListMapGenContext> detours = new AddConnectedRoomsStep<ListMapGenContext>(detourRooms, detourHalls);
                            detours.Amount = new RandRange(1);
                            detours.HallPercent = 100;
                            detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom(), new UnVaultableRoom()));
                            detours.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.SwitchVault));
                            detours.RoomComponents.Set(new NoEventRoom());

                            layout.GenSteps.Add(PR_ROOMS_GEN_EXTRA, detours);
                        }

                        {
                            SpawnList<RoomGen<ListMapGenContext>> detourRooms = new SpawnList<RoomGen<ListMapGenContext>>();

                            detourRooms.Add(new RoomGenRound<ListMapGenContext>(new RandRange(4, 8), new RandRange(4, 8)), 10);
                            detourRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(4, 9), new RandRange(4, 9)), 10);
                            SpawnList<PermissiveRoomGen<ListMapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                            detourHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                            AddConnectedRoomsStep<ListMapGenContext> detours = new AddConnectedRoomsStep<ListMapGenContext>(detourRooms, detourHalls);
                            detours.Amount = new RandRange(20);
                            detours.HallPercent = 100;
                            detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom()));
                            detours.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            detours.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            layout.GenSteps.Add(PR_ROOMS_GEN_EXTRA, detours);

                            // try to add more halls connecting
                            layout.GenSteps.Add(PR_ROOMS_GEN_EXTRA, CreateGenericListConnect(60, 0));
                        }


                        //sealing the vault
                        {
                            SwitchSealStep<ListMapGenContext> vaultStep = new SwitchSealStep<ListMapGenContext>("sealed_block", "tile_switch", 3, true, false);
                            vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                            vaultStep.SwitchFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            vaultStep.SwitchFilters.Add(new RoomFilterComponent(true, new BossRoom()));
                            layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
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

                            {
                                HashSet<string> exceptFor = new HashSet<string>();
                                foreach (string legend in IterateLegendaries())
                                    exceptFor.Add(legend);
                                SpeciesItemElementSpawner<ListMapGenContext> spawn = new SpeciesItemElementSpawner<ListMapGenContext>(new IntRange(2), new RandRange(1), "fire", exceptFor);
                                boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_deluxe", spawn), 10);
                            }
                            {
                                HashSet<string> exceptFor = new HashSet<string>();
                                foreach (string legend in IterateLegendaries())
                                    exceptFor.Add(legend);
                                SpeciesItemElementSpawner<ListMapGenContext> spawn = new SpeciesItemElementSpawner<ListMapGenContext>(new IntRange(2), new RandRange(1), "water", exceptFor);
                                boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_deluxe", spawn), 10);
                            }
                            {
                                HashSet<string> exceptFor = new HashSet<string>();
                                foreach (string legend in IterateLegendaries())
                                    exceptFor.Add(legend);
                                SpeciesItemElementSpawner<ListMapGenContext> spawn = new SpeciesItemElementSpawner<ListMapGenContext>(new IntRange(2), new RandRange(1), "grass", exceptFor);
                                boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_deluxe", spawn), 10);
                            }
                            {
                                HashSet<string> exceptFor = new HashSet<string>();
                                foreach (string legend in IterateLegendaries())
                                    exceptFor.Add(legend);
                                SpeciesItemElementSpawner<ListMapGenContext> spawn = new SpeciesItemElementSpawner<ListMapGenContext>(new IntRange(2), new RandRange(1), "electric", exceptFor);
                                boxSpawn.Add(new BoxSpawner<ListMapGenContext>("box_deluxe", spawn), 10);
                            }

                            MultiStepSpawner<ListMapGenContext, MapItem> boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(3)));

                            //StepSpawner <- PresetMultiRand
                            MultiStepSpawner<ListMapGenContext, MapItem> mainSpawner = new MultiStepSpawner<ListMapGenContext, MapItem>();
                            mainSpawner.Picker = new PresetMultiRand<IStepSpawner<ListMapGenContext, MapItem>>(treasurePicker, boxPicker);

                            RandomRoomSpawnStep<ListMapGenContext, MapItem> detourItems = new RandomRoomSpawnStep<ListMapGenContext, MapItem>(mainSpawner);
                            detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                            layout.GenSteps.Add(PR_SPAWN_ITEMS_EXTRA, detourItems);
                        }
                    }


                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<ListMapGenContext, MapGenEntrance, MapGenExit>());
                    if (ii == max_floors - 1)
                        layout.GenSteps.Add(PR_DBG_CHECK, new DetectItemStep<ListMapGenContext>("box_deluxe"));


                    floorSegment.Floors.Add(layout);
                }

                zone.Segments.Add(floorSegment);
            }
            #endregion
        }

        static void FillAmbushForest(ZoneData zone, bool translate)
        {
            #region AMBUSH FOREST
            {
                zone.Name = new LocalText("Ambush Forest");
                zone.Rescues = 2;
                zone.Level = 30;
                zone.Rogue = RogueStatus.NoTransfer;

                int max_floors = 20;
                LayeredSegment floorSegment = new LayeredSegment();
                floorSegment.IsRelevant = true;
                floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Ambush Forest\nB{0}F"), new Priority(-15)));

                //money
                MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 10);
                moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

                //items
                ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;

                //fakes
                CategorySpawn<InvItem> fakes = new CategorySpawn<InvItem>();
                fakes.SpawnRates.SetRange(5, new IntRange(0, max_floors));
                itemSpawnZoneStep.Spawns.Add("fakes", fakes);

                fakes.Spawns.Add(InvItem.CreateBox("food_apple_huge", "appletun"), new IntRange(10, max_floors), 10);//Big Apple (fake)

                //necessities
                CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                necessities.SpawnRates.SetRange(14, new IntRange(0, max_floors));
                itemSpawnZoneStep.Spawns.Add("necessities", necessities);


                necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 18);
                necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 12);
                necessities.Spawns.Add(new InvItem("food_apple_huge"), new IntRange(0, max_floors), 1);
                necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 20);
                necessities.Spawns.Add(new InvItem("berry_sitrus"), new IntRange(0, max_floors), 12);

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


                poolSpawn.Spawns.Add(GetBoostedTeamMob("murkrow", "insomnia", "pursuit", "wing_attack", "", "", new RandRange(23), 4, "wander_normal", false, true), new IntRange(0, 6), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("spinarak", "", "spider_web", "night_shade", "", "", new RandRange(23), 4, "wander_normal", false, true), new IntRange(0, 4), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("gastly", "", "sucker_punch", "hypnosis", "", "", new RandRange(23), 4, "wander_normal", false, true), new IntRange(0, 4), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("dartrix", "", "foresight", "razor_leaf", "", "", new RandRange(23), 4, "wander_normal", false, true), new IntRange(0, 6), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("corphish", "", "bubble", "knock_off", "harden", "", new RandRange(23), 4, "wander_normal", false, true), new IntRange(0, 6), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("teddiursa", "quick_feet", "covet", "sweet_scent", "fury_swipes", "", new RandRange(25), 4, "thief", false, true), new IntRange(2, 8), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("scyther", "", "vacuum_wave", "false_swipe", "", "", new RandRange(25), 4, "wander_normal", false, true), new IntRange(2, 8), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("loudred", "", "round", "sing", "", "", new RandRange(25), 4, TeamMemberSpawn.MemberRole.Support, "wander_normal", false, true), new IntRange(4, 8), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("zangoose", "", "pursuit", "crush_claw", "", "", new RandRange(25), 4, "wander_normal", false, true), new IntRange(6, 10), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("shedinja", "", "sand_attack", "shadow_sneak", "", "", new RandRange(23), 4, "wander_normal", false, true), new IntRange(6, 10), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("sneasel", "", "beat_up", "icy_wind", "", "", new RandRange(24), 4, "wander_normal", false, true), new IntRange(8, 12), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("raticate", "", "pursuit", "super_fang", "crunch", "", new RandRange(26), 4, "wander_normal", false, true), new IntRange(8, 12), 10);

                //Sleeping, holds item
                {
                    TeamMemberSpawn mob = GetBoostedTeamMob("spinda", "tangled_feet", "teeter_dance", "copycat", "", "", new RandRange(30), 4, TeamMemberSpawn.MemberRole.Support, "wander_normal", true);
                    MobSpawnItem itemSpawn = new MobSpawnItem(true);
                    itemSpawn.Items.Add(new InvItem("held_trap_scarf"), 10);
                    itemSpawn.Items.Add(new InvItem("held_twist_band"), 10);
                    itemSpawn.Items.Add(new InvItem("held_metronome"), 10);
                    mob.Spawn.SpawnFeatures.Add(itemSpawn);
                    poolSpawn.Spawns.Add(mob, new IntRange(8, 14), 5);
                }

                poolSpawn.Spawns.Add(GetBoostedTeamMob("swellow", "", "quick_guard", "aerial_ace", "", "", new RandRange(24), 4, "wander_normal", false, true), new IntRange(8, 14), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("hattrem", "", "dazzling_gleam", "life_dew", "", "", new RandRange(25), 4, TeamMemberSpawn.MemberRole.Support, "wander_normal", false, true), new IntRange(8, 14), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("ambipom", "technician", "agility", "swift", "", "", new RandRange(24), 4, "wander_normal", false, true), new IntRange(12, 16), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("furret", "", "defense_curl", "follow_me", "", "", new RandRange(24), 4, TeamMemberSpawn.MemberRole.Support, "wander_normal", false, true), new IntRange(12, 16), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("growlithe", "", "retaliate", "flame_wheel", "", "", new RandRange(27), 4, TeamMemberSpawn.MemberRole.Support, "wander_normal", false, true), new IntRange(14, 18), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("braixen", "magician", "howl", "fire_spin", "", "", new RandRange(25), 4, "thief", false, true), new IntRange(14, 18), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("haunter", "", "hypnosis", "night_shade", "", "", new RandRange(27), 4, "wander_normal", false, true), new IntRange(14, 18), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("hypno", "insomnia", "nightmare", "confusion", "", "", new RandRange(27), 4, "wander_normal", false, true), new IntRange(16, 20), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("crawdaunt", "", "razor_shell", "night_slash", "harden", "", new RandRange(28), 4, "wander_normal", false, true), new IntRange(16, 20), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("parasect", "", "spore", "growth", "leech_life", "", new RandRange(27), 4, "wander_normal", false, true), new IntRange(18, 20), 10);

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


                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(5, 8), new IntRange(0, max_floors)), new MapItem("food_apple"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(8, 15), new IntRange(0, max_floors)), new MapItem("berry_leppa"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(max_floors / 2, max_floors - 1), new IntRange(0, max_floors)), new MapItem("orb_cleanse"));


                {
                    //monster houses
                    SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(20, new IntRange(1, 10)));
                    monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHouseStep<ListMapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                    foreach (string gummi in IterateGummis())
                        monsterChanceZoneStep.Items.Add(new MapItem(gummi), new IntRange(0, max_floors), 4);//gummis
                    foreach (string key in IterateTMs(TMClass.Starter | TMClass.Bottom | TMClass.Mid))
                        monsterChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 1);//TMs

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
                    monsterChanceZoneStep.Mobs.Add(GetGenericMob("spiritomb", "", "hypnosis", "dream_eater", "", "", new RandRange(32), "wander_normal", false, true), new IntRange(6, max_floors), 10);
                    monsterChanceZoneStep.Mobs.Add(GetGenericMob("decidueye", "long_reach", "spirit_shackle", "sucker_punch", "", "", new RandRange(34), "wander_normal", false, true), new IntRange(14, max_floors), 10);
                    monsterChanceZoneStep.MobThemes.Add(new MobThemeNone(20, new RandRange(7, 13)), new IntRange(0, max_floors), 10);
                    floorSegment.ZoneSteps.Add(monsterChanceZoneStep);
                }


                {
                    SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(25, new IntRange(10, max_floors)));
                    monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHallStep<ListMapGenContext>(new Loc(11, 9), GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                    monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHallStep<ListMapGenContext>(new Loc(15, 13), GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);

                    foreach (string key in IterateGummis())
                        monsterChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//gummis
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
                    monsterChanceZoneStep.Mobs.Add(GetGenericMob("spiritomb", "", "hypnosis", "dream_eater", "", "", new RandRange(31), "wander_normal", false, true), new IntRange(0, max_floors), 10);
                    monsterChanceZoneStep.Mobs.Add(GetGenericMob("decidueye", "long_reach", "spirit_shackle", "sucker_punch", "", "", new RandRange(37), "wander_normal", false, true), new IntRange(14, max_floors), 10);
                    monsterChanceZoneStep.MobThemes.Add(new MobThemeNone(20, new RandRange(7, 13)), new IntRange(0, max_floors), 10);

                    floorSegment.ZoneSteps.Add(monsterChanceZoneStep);
                }


                {
                    SpreadHouseZoneStep chestChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanQuota(new RandDecay(1, 8, 50), new IntRange(4, max_floors)));
                    chestChanceZoneStep.ModStates.Add(new FlagType(typeof(ChestModGenState)));
                    chestChanceZoneStep.HouseStepSpawns.Add(new ChestStep<ListMapGenContext>(false, GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);

                    foreach (string key in IterateVitamins())
                        chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//boosters
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
                    SpreadHouseZoneStep chestChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanQuota(new RandDecay(1, 8, 80), new IntRange(4, max_floors)));
                    chestChanceZoneStep.ModStates.Add(new FlagType(typeof(ChestModGenState)));
                    chestChanceZoneStep.HouseStepSpawns.Add(new ChestStep<ListMapGenContext>(true, GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);

                    foreach (string key in IterateVitamins())
                        chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//boosters
                    foreach (string key in IterateGummis())
                        chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//gummis
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_elixir"), new IntRange(0, max_floors), 20);//elixir
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_potion"), new IntRange(0, max_floors), 20);//potion
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_max_elixir"), new IntRange(0, max_floors), 20);//max elixir
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_max_potion"), new IntRange(0, max_floors), 20);//max potion
                    chestChanceZoneStep.Items.Add(new MapItem("food_apple_perfect"), new IntRange(0, max_floors), 20);//perfect apple
                    foreach (string key in IterateXItems())
                        chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 10);//X-Items
                    chestChanceZoneStep.Items.Add(new MapItem("loot_nugget"), new IntRange(0, max_floors), 20);//nugget
                    chestChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear", 1), new IntRange(0, max_floors), 20);//amber tear
                    chestChanceZoneStep.Items.Add(new MapItem("seed_joy"), new IntRange(0, max_floors), 15);//joy seed
                    chestChanceZoneStep.Items.Add(new MapItem("machine_ability_capsule"), new IntRange(0, max_floors), 15);//ability capsule

                    chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(true, true, new RandRange(3), "loot_pearl"), new ItemThemeNone(100, new RandRange(3, 5))), new IntRange(0, max_floors), 30);
                    chestChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(5, 10)), new IntRange(0, max_floors), 10);//gummis

                    chestChanceZoneStep.Mobs.Add(GetGenericMob("spiritomb", "", "hypnosis", "dream_eater", "", "", new RandRange(31), "wander_normal", false, true), new IntRange(0, max_floors), 10);
                    chestChanceZoneStep.Mobs.Add(GetGenericMob("decidueye", "long_reach", "spirit_shackle", "sucker_punch", "", "", new RandRange(37), "wander_normal", false, true), new IntRange(14, max_floors), 10);
                    chestChanceZoneStep.MobThemes.Add(new MobThemeNone(20, new RandRange(7, 13)), new IntRange(0, max_floors), 10);

                    floorSegment.ZoneSteps.Add(chestChanceZoneStep);
                }

                AddHiddenStairStep(floorSegment, new SpreadPlanQuota(new RandRange(1), new IntRange(0, max_floors - 1)), 1);

                AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 10, 2);

                for (int ii = 0; ii < max_floors; ii++)
                {
                    GridFloorGen layout = new GridFloorGen();

                    //Floor settings
                    if (ii < 8)
                        AddFloorData(layout, "B26. Ambush Forest.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);
                    else if (ii < 16)
                        AddFloorData(layout, "B27. Ambush Forest 2.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);
                    else
                        AddFloorData(layout, "B28. Ambush Forest 3.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);

                    if (ii >= 8)
                    {
                        Dictionary<ItemFake, MobSpawn> spawnTable = new Dictionary<ItemFake, MobSpawn>();
                        spawnTable.Add(new ItemFake("food_apple_huge", "appletun"), GetGenericMob("appletun", "gluttony", "apple_acid", "body_slam", "", "", new RandRange(32)));
                        AddFloorFakeItems(layout, spawnTable);
                    }

                    //Tilesets
                    if (ii < 4)
                        AddSpecificTextureData(layout, "deep_dusk_forest_1_wall", "deep_dusk_forest_1_floor", "deep_dusk_forest_1_secondary", "tall_grass_dark", "bug");
                    else if (ii < 8)
                        AddSpecificTextureData(layout, "dusk_forest_1_wall", "dusk_forest_1_floor", "dusk_forest_1_secondary", "tall_grass_dark", "bug");
                    else if (ii < 16)
                        AddSpecificTextureData(layout, "dusk_forest_2_wall", "dusk_forest_2_floor", "dusk_forest_2_secondary", "tall_grass_blue", "bug");
                    else
                        AddSpecificTextureData(layout, "murky_forest_wall", "murky_forest_floor", "murky_forest_secondary", "tall_grass_blue", "bug");

                    //add water cracks
                    if (ii >= 4)
                    {
                        AddTunnelStep<MapGenContext> tunneler = new AddTunnelStep<MapGenContext>();
                        tunneler.Halls = new RandRange(4, 8);
                        tunneler.TurnLength = new RandRange(3, 8);
                        tunneler.MaxLength = new RandRange(25);
                        tunneler.Brush = new TerrainHallBrush(Loc.One, new Tile("water"));
                        layout.GenSteps.Add(PR_WATER, tunneler);
                    }

                    if (ii >= 8)
                        AddBlobWaterSteps(layout, "water", new RandRange(1, 4), new IntRange(2, 9), true);

                    AddGrassSteps(layout, new RandRange(8, 12), new IntRange(2, 6), new RandRange(25));

                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile

                    if (ii >= 8)
                    {
                        SpawnList<PatternPlan> patternList = new SpawnList<PatternPlan>();
                        patternList.Add(new PatternPlan("pattern_checker_large", PatternPlan.PatternExtend.Repeat2D), 5);
                        patternList.Add(new PatternPlan("pattern_crosshair", PatternPlan.PatternExtend.Extrapolate), 5);
                        patternList.Add(new PatternPlan("pattern_squiggle", PatternPlan.PatternExtend.Repeat1D), 5);
                        AddTrapPatternSteps(layout, new RandRange(1, 3), patternList);
                    }

                    AddTrapsSteps(layout, new RandRange(10, 14));

                    //money
                    AddMoneyData(layout, new RandRange(4, 9));

                    //enemies
                    AddRespawnData(layout, 12, 100);
                    if (ii < 4)
                        AddEnemySpawnData(layout, 20, new RandRange(6, 10));
                    else if (ii < 8)
                        AddEnemySpawnData(layout, 20, new RandRange(7, 10));
                    else if (ii < 16)
                        AddEnemySpawnData(layout, 20, new RandRange(8, 11));
                    else
                        AddEnemySpawnData(layout, 20, new RandRange(9, 12));

                    //items
                    if (ii < 8)
                        AddItemData(layout, new RandRange(3, 6), 25);
                    else
                        AddItemData(layout, new RandRange(4, 7), 25);

                    //construct paths
                    if (ii < 4)
                    {
                        //Large Rooms with Tunnels
                        if (ii < 2)
                            AddInitGridStep(layout, 3, 3, 14, 14);
                        else
                            AddInitGridStep(layout, 4, 3, 14, 14);

                        GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.RoomRatio = new RandRange(90, 101);
                        path.BranchRatio = new RandRange(0);

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //cross
                        genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(9, 15), new RandRange(9, 15), new RandRange(4, 8), new RandRange(4, 8)), 10);
                        //cave
                        genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(9, 15), new RandRange(9, 15)), 10);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(0), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_GRID_GEN, path);

                        layout.GenSteps.Add(PR_GRID_GEN, new SetGridDefaultsStep<MapGenContext>(new RandRange(70), GetImmutableFilterList()));


                        {
                            CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(1, 4), GetImmutableFilterList());
                            step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenBump<MapGenContext>(new RandRange(20, 29), new RandRange(10, 13), new RandRange(3, 91))), 10);
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenBump<MapGenContext>(new RandRange(10, 13), new RandRange(20, 29), new RandRange(3, 91))), 10);
                            layout.GenSteps.Add(PR_GRID_GEN, step);
                        }


                        AddTunnelStep<MapGenContext> tunnelerEnd = new AddTunnelStep<MapGenContext>();
                        tunnelerEnd.Halls = new RandRange(8, 12);
                        tunnelerEnd.TurnLength = new RandRange(3, 7);
                        tunnelerEnd.MaxLength = new RandRange(8);
                        tunnelerEnd.AllowDeadEnd = true;
                        layout.GenSteps.Add(PR_TILES_GEN_TUNNEL, tunnelerEnd);
                    }
                    else if (ii < 8)
                    {
                        //Min Spanning Tree with Tunnels.  Short Dead-Ends
                        AddInitGridStep(layout, 8, 6, 7, 7);

                        GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.RoomRatio = new RandRange(45);
                        path.BranchRatio = new RandRange(10);

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //cross
                        genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8), new RandRange(2, 5), new RandRange(2, 5)), 10);
                        //cave
                        genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(5, 8), new RandRange(5, 8)), 10);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_GRID_GEN, path);

                        layout.GenSteps.Add(PR_GRID_GEN, new SetGridDefaultsStep<MapGenContext>(new RandRange(25), GetImmutableFilterList()));

                        {
                            CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(2, 5), GetImmutableFilterList());
                            step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            step.Filters.Add(new RoomFilterDefaultGen(true));
                            step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenCave<MapGenContext>(new RandRange(5, 8), new RandRange(10, 15))), 10);
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenCave<MapGenContext>(new RandRange(10, 15), new RandRange(5, 8))), 10);
                            layout.GenSteps.Add(PR_GRID_GEN, step);
                        }

                        AddTunnelStep<MapGenContext> tunnelerEnd = new AddTunnelStep<MapGenContext>();
                        tunnelerEnd.Halls = new RandRange(8, 12);
                        tunnelerEnd.TurnLength = new RandRange(2, 5);
                        tunnelerEnd.MaxLength = new RandRange(4);
                        tunnelerEnd.AllowDeadEnd = true;
                        layout.GenSteps.Add(PR_TILES_GEN_TUNNEL, tunnelerEnd);
                    }
                    else if (ii < 12)
                    {
                        //Merged Circle Path

                        AddInitGridStep(layout, 5, 4, 9, 9);

                        GridPathCircle<MapGenContext> path = new GridPathCircle<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.CircleRoomRatio = new RandRange(80);
                        path.Paths = new RandRange(1, 5);

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //cross
                        genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(5, 10), new RandRange(5, 10), new RandRange(2, 5), new RandRange(2, 5)), 10);
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
                            step.Filters.Add(new RoomFilterDefaultGen(true));
                            step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenCave<MapGenContext>(new RandRange(5, 8), new RandRange(10, 18))), 10);
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenCave<MapGenContext>(new RandRange(10, 18), new RandRange(5, 8))), 10);
                            layout.GenSteps.Add(PR_GRID_GEN, step);
                        }
                    }
                    else if (ii < 16)
                    {
                        //100% Merged Grid

                        AddInitGridStep(layout, 5, 4, 9, 9);

                        GridPathGrid<MapGenContext> path = new GridPathGrid<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.RoomRatio = 100;
                        path.HallRatio = 60;

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //cross
                        genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(5, 10), new RandRange(5, 10), new RandRange(2, 5), new RandRange(2, 5)), 10);
                        //cave
                        genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(5, 10), new RandRange(5, 10)), 10);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(20), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_GRID_GEN, path);

                        {
                            CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(6, 10), GetImmutableFilterList());
                            step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                            step.Filters.Add(new RoomFilterDefaultGen(true));
                            step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenCave<MapGenContext>(new RandRange(5, 8), new RandRange(10, 18))), 10);
                            step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenCave<MapGenContext>(new RandRange(10, 18), new RandRange(5, 8))), 10);
                            layout.GenSteps.Add(PR_GRID_GEN, step);
                        }
                    }
                    else
                    {
                        //many tunnel maze

                        AddInitGridStep(layout, 5, 4, 11, 11);

                        GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.RoomRatio = new RandRange(80);
                        path.BranchRatio = new RandRange(20);

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //cross
                        genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(5, 11), new RandRange(5, 11), new RandRange(2, 5), new RandRange(2, 5)), 10);
                        //cave
                        genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(5, 11), new RandRange(5, 11)), 10);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(0), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_GRID_GEN, path);

                        AddTunnelStep<MapGenContext> tunneler = new AddTunnelStep<MapGenContext>();
                        tunneler.Halls = new RandRange(20, 28);
                        tunneler.TurnLength = new RandRange(3, 8);
                        tunneler.MaxLength = new RandRange(25);
                        layout.GenSteps.Add(PR_TILES_GEN_TUNNEL, tunneler);
                    }

                    AddDrawGridSteps(layout);

                    AddStairStep(layout, false);


                    if (ii == max_floors / 2)
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
                            loadRoom.MapID = "room_ambush_item";
                            loadRoom.RoomTerrain = new Tile("grass");
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
            }

            {
                SingularSegment structure = new SingularSegment(-1);

                SpawnList<TeamMemberSpawn> enemyList = new SpawnList<TeamMemberSpawn>();
                structure.BaseFloor = getSecretRoom(translate, "special_grass_maze", -1, "murky_forest_wall", "murky_forest_floor", "murky_forest_secondary", "tall_grass_blue", "bug", enemyList, new Loc(5, 11));

                zone.Segments.Add(structure);
            }

            {
                SingularSegment structure = new SingularSegment(-1);

                ChanceFloorGen multiGen = new ChanceFloorGen();
                multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Advanced, "small_square", -2, true, false), 10);
                structure.BaseFloor = multiGen;

                zone.Segments.Add(structure);
            }

            {
                LayeredSegment staticSegment = new LayeredSegment();
                LoadGen layout = new LoadGen();
                MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                startGen.MapID = "end_ambush_forest";
                layout.GenSteps.Add(PR_FILE_LOAD, startGen);
                MapEffectStep<MapLoadContext> noRescue = new MapEffectStep<MapLoadContext>();
                noRescue.Effect.OnMapRefresh.Add(0, new MapNoRescueEvent());
                layout.GenSteps.Add(PR_FLOOR_DATA, noRescue);
                staticSegment.Floors.Add(layout);
                zone.Segments.Add(staticSegment);
            }


            zone.GroundMaps.Add("end_ambush_forest");
            #endregion
        }

        static void FillTreacherousMountain(ZoneData zone, bool translate)
        {
            #region TREACHEROUS MOUNTAIN
            {
                zone.Name = new LocalText("Treacherous Mountain");
                zone.Rescues = 2;
                zone.Level = 40;
                zone.Rogue = RogueStatus.NoTransfer;

                int max_floors = 20;
                LayeredSegment floorSegment = new LayeredSegment();
                floorSegment.IsRelevant = true;
                floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
                floorSegment.ZoneSteps.Add(new FloorNameDropZoneStep(PR_FLOOR_DATA, new LocalText("Treacherous Mountain\n{0}F"), new Priority(-15)));

                //money
                MoneySpawnZoneStep moneySpawnZoneStep = GetMoneySpawn(zone.Level, 10);
                moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
                floorSegment.ZoneSteps.Add(moneySpawnZoneStep);
                //items
                ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
                itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;


                //necessities
                CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
                necessities.SpawnRates.SetRange(16, new IntRange(0, max_floors));
                itemSpawnZoneStep.Spawns.Add("necessities", necessities);


                necessities.Spawns.Add(new InvItem("berry_leppa"), new IntRange(0, max_floors), 10);
                necessities.Spawns.Add(new InvItem("berry_oran"), new IntRange(0, max_floors), 10);
                necessities.Spawns.Add(new InvItem("food_grimy"), new IntRange(0, max_floors), 10);
                necessities.Spawns.Add(new InvItem("berry_lum"), new IntRange(0, max_floors), 10);
                necessities.Spawns.Add(new InvItem("berry_sitrus"), new IntRange(0, max_floors), 10);

                //snacks
                CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
                snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                itemSpawnZoneStep.Spawns.Add("snacks", snacks);


                snacks.Spawns.Add(new InvItem("berry_charti"), new IntRange(0, max_floors), 5);
                snacks.Spawns.Add(new InvItem("berry_babiri"), new IntRange(0, max_floors), 2);
                snacks.Spawns.Add(new InvItem("berry_passho"), new IntRange(0, max_floors), 2);
                snacks.Spawns.Add(new InvItem("berry_shuca"), new IntRange(0, max_floors), 2);
                snacks.Spawns.Add(new InvItem("berry_coba"), new IntRange(0, max_floors), 2);
                snacks.Spawns.Add(new InvItem("berry_yache"), new IntRange(0, max_floors), 5);
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
                throwable.Spawns.Add(new InvItem("wand_path", false, 2), new IntRange(0, max_floors), 10);
                throwable.Spawns.Add(new InvItem("wand_fear", false, 2), new IntRange(0, max_floors), 10);
                throwable.Spawns.Add(new InvItem("wand_slow", false, 2), new IntRange(0, max_floors), 10);
                throwable.Spawns.Add(new InvItem("wand_pounce", false, 3), new IntRange(0, max_floors), 10);
                throwable.Spawns.Add(new InvItem("wand_warp", false, 2), new IntRange(0, max_floors), 10);
                throwable.Spawns.Add(new InvItem("wand_lob", false, 3), new IntRange(0, max_floors), 10);
                //orbs
                CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
                orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
                itemSpawnZoneStep.Spawns.Add("orbs", orbs);


                orbs.Spawns.Add(new InvItem("orb_weather"), new IntRange(0, max_floors), 15);
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


                poolSpawn.Spawns.Add(GetBoostedTeamMob("marshtomp", "", "mud_bomb", "water_gun", "", "", new RandRange(32), 4, "wander_normal", false, true), new IntRange(0, 4), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("snorunt", "ice_body", "powder_snow", "double_team", "", "", new RandRange(32), 4, "wander_normal", false, true), new IntRange(0, 4), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("pupitar", "", "screech", "rock_slide", "", "", new RandRange(32), 4, "wander_normal", false, true), new IntRange(0, 6), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("piloswine", "", "endure", "take_down", "ice_shard", "", new RandRange(32), 4, "wander_normal", false, true), new IntRange(0, 6), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob(new MonsterID("graveler", 1, "", Gender.Unknown), "galvanize", "self_destruct", "stealth_rock", "", "", new RandRange(33), 4, TeamMemberSpawn.MemberRole.Normal, "wander_normal", false, true), new IntRange(2, 8), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("yanmega", "", "sonic_boom", "", "", "", new RandRange(33), 4, "wander_normal", false, true), new IntRange(4, 8), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("duskull", "", "night_shade", "disable", "", "", new RandRange(33), 3, "wander_normal", false, true), new IntRange(4, 10), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("drampa", "sap_sipper", "dragon_rage", "twister", "", "", new RandRange(34), 4, "wander_normal", false, true), new IntRange(4, 10), 5);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("shelgon", "overcoat", "dragon_breath", "protect", "", "", new RandRange(34), 4, "wander_normal", false, true), new IntRange(6, 11), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("nuzleaf", "", "torment", "razor_leaf", "", "", new RandRange(34), 4, "wander_normal", false, true), new IntRange(8, 13), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("bronzong", "", "imprison", "extrasensory", "", "", new RandRange(34), 4, "wander_normal", false, true), new IntRange(8, 13), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("ninjask", "", "swords_dance", "slash", "", "", new RandRange(35), 4, "retreater", false, true), new IntRange(10, 14), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("weavile", "pickpocket", "embargo", "night_slash", "", "", new RandRange(35), 4, "thief", false, true), new IntRange(10, 16), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("dunsparce", "", "spite", "ancient_power", "screech", "", new RandRange(35), 4, "wander_normal", false, true), new IntRange(12, 16), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("dusclops", "pressure", "will_o_wisp", "bind", "", "", new RandRange(35), 4, "wander_normal", false, true), new IntRange(12, 16), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("shiftry", "", "razor_wind", "leaf_tornado", "", "", new RandRange(35), 4, "wander_normal", false, true), new IntRange(14, 18), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("swampert", "", "muddy_water", "", "", "", new RandRange(36), 4, "wander_normal", false, true), new IntRange(14, max_floors), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("dugtrio", "sand_veil", "earth_power", "bulldoze", "", "", new RandRange(36), 4, "wander_normal", false, true), new IntRange(14, max_floors), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("sandslash", "sand_veil", "magnitude", "crush_claw", "", "", new RandRange(36), 4, "wander_normal", false, true), new IntRange(16, max_floors), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("froslass", "snow_cloak", "captivate", "draining_kiss", "", "", new RandRange(36), 4, "wander_normal", false, true), new IntRange(16, max_floors), 10);
                poolSpawn.Spawns.Add(GetBoostedTeamMob("glalie", "ice_body", "frost_breath", "double_team", "", "", new RandRange(37), 4, "wander_normal", false, true), new IntRange(18, max_floors), 10);


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

                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(8, 15), new IntRange(0, max_floors)), new MapItem("food_apple"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(1, 4), new IntRange(0, max_floors)), new MapItem("berry_sitrus"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(3, 6), new IntRange(0, max_floors)), new MapItem("berry_leppa"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(max_floors / 2, max_floors - 1), new IntRange(0, max_floors)), new MapItem("orb_cleanse"));


                {
                    //monster houses
                    SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(15, new IntRange(2, max_floors)));
                    monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHouseStep<ListMapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                    foreach (string gummi in IterateGummis())
                        monsterChanceZoneStep.Items.Add(new MapItem(gummi), new IntRange(0, max_floors), 4);//gummis
                    foreach (string key in IterateTMs(TMClass.Starter | TMClass.Bottom | TMClass.Mid))
                        monsterChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 1);//TMs

                    monsterChanceZoneStep.Items.Add(new MapItem("ammo_silver_spike", 6), new IntRange(0, max_floors), 30);//silver spike
                    monsterChanceZoneStep.Items.Add(new MapItem("loot_nugget"), new IntRange(0, max_floors), 10);//nugget
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
                    SpreadHouseZoneStep chestChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanQuota(new RandDecay(1, 8, 80), new IntRange(4, max_floors)));
                    chestChanceZoneStep.ModStates.Add(new FlagType(typeof(ChestModGenState)));
                    chestChanceZoneStep.HouseStepSpawns.Add(new ChestStep<ListMapGenContext>(false, GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);

                    foreach (string key in IterateVitamins())
                        chestChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 4);//boosters
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

                        MultiStepSpawner<ListMapGenContext, MapItem> boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(3, 5)));

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

                        //426 Drifblim : 466 Ominous Wind : 226 Baton Pass : 254 Stockpile : 107 Minimize
                        vaultChanceZoneStep.Mobs.Add(GetFOEMob("drifblim", "", "ominous_wind", "baton_pass", "stockpile", "minimize", zone.Level + 5, 1, 2), new IntRange(0, max_floors), 10);

                        //50//233 Porygon2 : 176 Conversion 2 : 105 Recover : 161 Tri Attack : 111 Defense Curl
                        vaultChanceZoneStep.Mobs.Add(GetFOEMob("porygon2", "", "conversion_2", "recover", "tri_attack", "defense_curl", zone.Level + 5, 1, 2), new IntRange(0, max_floors), 5);

                        //53//452 Drapion : 367 Acupressure : 398 Poison Jab : 424 Fire Fang : 565 Fell Stinger
                        vaultChanceZoneStep.Mobs.Add(GetFOEMob("drapion", "", "acupressure", "poison_jab", "fire_fang", "fell_stinger", zone.Level + 5, 1, 2), new IntRange(0, max_floors), 10);

                        //131 Lapras : 011 Water Absorb : 058 Ice Beam : 195 Perish Song : 362 Brine
                        vaultChanceZoneStep.Mobs.Add(GetFOEMob("lapras", "water_absorb", "ice_beam", "perish_song", "brine", "", zone.Level + 5, 1, 2), new IntRange(0, max_floors), 10);

                        //275 Shiftry : 124 Pickpocket : 018 Whirlwind : 417 Nasty Plot : 536 Leaf Tornado : 542 Hurricane
                        vaultChanceZoneStep.Mobs.Add(GetFOEMob("shiftry", "pickpocket", "whirlwind", "nasty_plot", "leaf_tornado", "hurricane", zone.Level + 5, 1, 2), new IntRange(0, max_floors), 10);

                        //130 !! Gyarados : 153 Moxie : 242 Crunch : 82 Dragon Rage : 240 Rain Dance : 401 Aqua Tail
                        vaultChanceZoneStep.Mobs.Add(GetFOEMob("gyarados", "moxie", "crunch", "dragon_rage", "rain_dance", "aqua_tail", zone.Level + 5, 1, 2), new IntRange(0, max_floors), 10);

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

                AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 10, 2);

                for (int ii = 0; ii < max_floors; ii++)
                {
                    RoomFloorGen layout = new RoomFloorGen();

                    //Floor settings
                    if (ii < 8)
                        AddFloorData(layout, "B29. Treacherous Mountain.ogg", 1500, Map.SightRange.Dark, Map.SightRange.Dark);
                    else if (ii < 16)
                        AddFloorData(layout, "B30. Treacherous Mountain 2.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);
                    else
                        AddFloorData(layout, "B31. Treacherous Mountain 3.ogg", 1500, Map.SightRange.Clear, Map.SightRange.Dark);

                    if (ii == 0)
                        AddDefaultMapStatus(layout, "default_weather", "sandstorm", "hail");
                    else if (ii < 16)
                    {
                        if (ii % 2 == 0)
                            AddDefaultMapStatus(layout, "default_weather", "sandstorm", "hail", "clear", "clear", "clear");
                    }
                    else
                        AddDefaultMapStatus(layout, "default_weather", "sandstorm", "hail", "clear", "clear", "clear", "clear");


                    //Tilesets
                    if (ii < 4)
                        AddSpecificTextureData(layout, "zero_isle_south_2_wall", "zero_isle_south_2_floor", "zero_isle_south_2_secondary", "tall_grass_dark", "dragon");
                    else if (ii < 8)
                        AddSpecificTextureData(layout, "mt_horn_wall", "mt_horn_floor", "mt_horn_secondary", "tall_grass_dark", "dragon");
                    else if (ii < 12)
                        AddSpecificTextureData(layout, "mt_thunder_wall", "mt_thunder_floor", "mt_thunder_secondary", "tall_grass_yellow", "dragon");
                    else if (ii < 16)
                        AddSpecificTextureData(layout, "southern_cavern_1_wall", "southern_cavern_1_floor", "southern_cavern_1_secondary", "tall_grass_white", "dragon");
                    else
                        AddSpecificTextureData(layout, "zero_isle_east_3_wall", "zero_isle_east_3_floor", "zero_isle_east_3_secondary", "tall_grass_dark", "dragon");

                    if (ii >= 6)
                        AddBlobWaterSteps(layout, "water", new RandRange(2, 5), new IntRange(2, 6), true);

                    //put the walls back in via "water" algorithm
                    if (ii >= 16)
                        AddBlobWaterSteps(layout, "wall", new RandRange(4, 10), new IntRange(2, 9), false);

                    if (ii < 8)
                        AddGrassSteps(layout, new RandRange(3, 7), new IntRange(2, 7), new RandRange(30));
                    else if (ii < 16)
                        AddGrassSteps(layout, new RandRange(3, 7), new IntRange(3, 9), new RandRange(20));


                    //traps
                    AddSingleTrapStep(layout, new RandRange(2, 4), "tile_wonder");//wonder tile

                    if (ii >= 8)
                    {
                        SpawnList<PatternPlan> patternList = new SpawnList<PatternPlan>();
                        patternList.Add(new PatternPlan("pattern_checker", PatternPlan.PatternExtend.Repeat2D), 5);
                        patternList.Add(new PatternPlan("pattern_crosshair", PatternPlan.PatternExtend.Extrapolate), 5);
                        patternList.Add(new PatternPlan("pattern_x_repeat", PatternPlan.PatternExtend.Repeat2D), 5);
                        AddTrapPatternSteps(layout, new RandRange(1, 3), patternList);
                    }

                    AddTrapsSteps(layout, new RandRange(20, 24), true);

                    //money
                    AddMoneyData(layout, new RandRange(4, 9));

                    //enemies
                    AddRespawnData(layout, 11, 130);
                    AddEnemySpawnData(layout, 20, new RandRange(9, 11));

                    //items
                    if (ii < 4)
                        AddItemData(layout, new RandRange(3, 6), 25);
                    else
                        AddItemData(layout, new RandRange(4, 7), 25);

                    //construct paths
                    if (ii < 4)
                    {
                        //Branch w/ Large Halls
                        AddInitListStep(layout, 40, 40);

                        FloorPathBranch<ListMapGenContext> path = new FloorPathBranch<ListMapGenContext>();

                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.FillPercent = new RandRange(50);
                        path.HallPercent = 100;
                        path.BranchRatio = new RandRange(20);

                        SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                        //cross
                        genericRooms.Add(new RoomGenCross<ListMapGenContext>(new RandRange(4, 8), new RandRange(4, 8), new RandRange(2, 5), new RandRange(2, 5)), 10);
                        //cave
                        if (ii < 4)
                            genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(10, 15), new RandRange(10, 15)), 10);
                        else
                            genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(5, 8), new RandRange(5, 8)), 10);
                        //small square
                        genericRooms.Add(new RoomGenSquare<ListMapGenContext>(new RandRange(2), new RandRange(2)), 20);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                        RoomGenAngledHall<ListMapGenContext> hall = new RoomGenAngledHall<ListMapGenContext>(50, new SquareHallBrush(new Loc(2)));
                        hall.Width = new RandRange(4, 8);
                        hall.Height = new RandRange(4, 8);
                        genericHalls.Add(hall, 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_ROOMS_GEN, path);

                        {
                            ConnectBranchStep<ListMapGenContext> step = new ConnectBranchStep<ListMapGenContext>();
                            step.Components.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            step.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom()));
                            step.ConnectPercent = 75;
                            PresetPicker<PermissiveRoomGen<ListMapGenContext>> picker = new PresetPicker<PermissiveRoomGen<ListMapGenContext>>();
                            picker.ToSpawn = new RoomGenAngledHall<ListMapGenContext>(50, new SquareHallBrush(new Loc(2)));
                            step.GenericHalls = picker;
                            layout.GenSteps.Add(PR_ROOMS_GEN, step);
                        }

                    }
                    else if (ii < 8)
                    {
                        //Freestyle with Large Halls Small Rooms
                        AddInitListStep(layout, 45, 40);

                        FloorPathBranch<ListMapGenContext> path = new FloorPathBranch<ListMapGenContext>();

                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.FillPercent = new RandRange(70);
                        path.HallPercent = 100;
                        path.BranchRatio = new RandRange(50);

                        SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                        //cave
                        genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(4, 8), new RandRange(4, 8)), 10);
                        //small square
                        genericRooms.Add(new RoomGenSquare<ListMapGenContext>(new RandRange(3), new RandRange(3)), 30);
                        path.GenericRooms = genericRooms;

                        {
                            SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                            RoomGenAngledHall<ListMapGenContext> hall = new RoomGenAngledHall<ListMapGenContext>(50, new SquareHallBrush(new Loc(2)));
                            hall.Width = new RandRange(4, 8);
                            hall.Height = new RandRange(4, 8);
                            genericHalls.Add(hall, 10);
                            path.GenericHalls = genericHalls;
                        }

                        layout.GenSteps.Add(PR_ROOMS_GEN, path);

                        {
                            ConnectBranchStep<ListMapGenContext> step = new ConnectBranchStep<ListMapGenContext>();
                            step.Components.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                            step.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom()));
                            step.ConnectPercent = 50;
                            PresetPicker<PermissiveRoomGen<ListMapGenContext>> picker = new PresetPicker<PermissiveRoomGen<ListMapGenContext>>();
                            picker.ToSpawn = new RoomGenAngledHall<ListMapGenContext>(50, new SquareHallBrush(new Loc(2)));
                            step.GenericHalls = picker;
                            layout.GenSteps.Add(PR_ROOMS_GEN, step);
                        }
                    }
                    else if (ii < 12)
                    {
                        //Freestyle short halls Cave/Cross
                        AddInitListStep(layout, 50, 44);

                        FloorPathBranch<ListMapGenContext> path = new FloorPathBranch<ListMapGenContext>();

                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.FillPercent = new RandRange(45);
                        path.HallPercent = 100;
                        path.BranchRatio = new RandRange(40);

                        SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                        //cross
                        genericRooms.Add(new RoomGenCross<ListMapGenContext>(new RandRange(5, 9), new RandRange(5, 9), new RandRange(2, 5), new RandRange(2, 5)), 10);
                        //cave
                        genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(6, 16), new RandRange(6, 16)), 10);
                        path.GenericRooms = genericRooms;

                        {
                            SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                            RoomGenAngledHall<ListMapGenContext> hall = new RoomGenAngledHall<ListMapGenContext>(50, new SquareHallBrush(new Loc(2)));
                            hall.Width = new RandRange(1, 3);
                            hall.Height = new RandRange(1, 3);
                            genericHalls.Add(hall, 10);
                            path.GenericHalls = genericHalls;
                        }

                        layout.GenSteps.Add(PR_ROOMS_GEN, path);
                    }
                    else if (ii < 16)
                    {
                        //Few Large Rooms
                        AddInitListStep(layout, 56, 44);

                        FloorPathBranch<ListMapGenContext> path = new FloorPathBranch<ListMapGenContext>();

                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.FillPercent = new RandRange(50);
                        path.HallPercent = 100;
                        path.BranchRatio = new RandRange(40);

                        SpawnList<RoomGen<ListMapGenContext>> genericRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                        //cross
                        genericRooms.Add(new RoomGenCross<ListMapGenContext>(new RandRange(11, 16), new RandRange(11, 16), new RandRange(4, 7), new RandRange(4, 7)), 10);
                        //cave
                        genericRooms.Add(new RoomGenCave<ListMapGenContext>(new RandRange(11, 16), new RandRange(11, 16)), 10);
                        path.GenericRooms = genericRooms;

                        {
                            SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                            RoomGenAngledHall<ListMapGenContext> hall = new RoomGenAngledHall<ListMapGenContext>(50, new SquareHallBrush(new Loc(2)));
                            hall.Width = new RandRange(1, 3);
                            hall.Height = new RandRange(1, 3);
                            genericHalls.Add(hall, 10);
                            path.GenericHalls = genericHalls;
                        }

                        layout.GenSteps.Add(PR_ROOMS_GEN, path);


                        AddTunnelStep<ListMapGenContext> tunneler = new AddTunnelStep<ListMapGenContext>();
                        tunneler.Halls = new RandRange(8, 15);
                        tunneler.TurnLength = new RandRange(3, 8);
                        tunneler.MaxLength = new RandRange(4, 9);
                        tunneler.AllowDeadEnd = true;
                        tunneler.Brush = new SquareHallBrush(new Loc(2));
                        layout.GenSteps.Add(PR_WATER, tunneler);
                    }
                    else
                    {
                        //Freestyle Large with Blocks Re-Added
                        AddInitListStep(layout, 56, 44);

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

                        {
                            SpawnList<PermissiveRoomGen<ListMapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                            RoomGenAngledHall<ListMapGenContext> hall = new RoomGenAngledHall<ListMapGenContext>(50, new SquareHallBrush(new Loc(2)));
                            hall.Width = new RandRange(4, 8);
                            hall.Height = new RandRange(4, 8);
                            genericHalls.Add(hall, 10);
                            path.GenericHalls = genericHalls;
                        }

                        layout.GenSteps.Add(PR_ROOMS_GEN, path);
                    }

                    AddDrawListSteps(layout);

                    AddStairStep(layout, false);


                    if (ii == max_floors / 2)
                    {
                        //making room for the vault
                        {
                            ResizeFloorStep<ListMapGenContext> addSizeStep = new ResizeFloorStep<ListMapGenContext>(new Loc(16, 16), Dir8.None);
                            layout.GenSteps.Add(PR_ROOMS_PRE_VAULT, addSizeStep);
                            ClampFloorStep<ListMapGenContext> limitStep = new ClampFloorStep<ListMapGenContext>(new Loc(0), new Loc(78, 54));
                            layout.GenSteps.Add(PR_ROOMS_PRE_VAULT, limitStep);
                            ClampFloorStep<ListMapGenContext> clampStep = new ClampFloorStep<ListMapGenContext>();
                            layout.GenSteps.Add(PR_ROOMS_PRE_VAULT_CLAMP, clampStep);
                        }

                        //vault rooms
                        {
                            SpawnList<RoomGen<ListMapGenContext>> detourRooms = new SpawnList<RoomGen<ListMapGenContext>>();

                            RoomGenLoadMap<ListMapGenContext> loadRoom = new RoomGenLoadMap<ListMapGenContext>();
                            loadRoom.MapID = "room_treacherous_item";
                            loadRoom.RoomTerrain = new Tile("floor");
                            loadRoom.PreventChanges = PostProcType.Panel | PostProcType.Terrain;
                            detourRooms.Add(loadRoom, 10);
                            SpawnList<PermissiveRoomGen<ListMapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                            detourHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                            AddConnectedRoomsStep<ListMapGenContext> detours = new AddConnectedRoomsStep<ListMapGenContext>(detourRooms, detourHalls);
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
                            KeySealStep<ListMapGenContext> vaultStep = new KeySealStep<ListMapGenContext>("sealed_block", "sealed_door", "key");
                            vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.KeyVault));
                            layout.GenSteps.Add(PR_TILES_GEN_EXTRA, vaultStep);
                        }
                    }

                    layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<ListMapGenContext, MapGenEntrance, MapGenExit>());

                    floorSegment.Floors.Add(layout);
                }

                zone.Segments.Add(floorSegment);
            }

            {
                SingularSegment structure = new SingularSegment(-1);

                SpawnList<TeamMemberSpawn> enemyList = new SpawnList<TeamMemberSpawn>();
                structure.BaseFloor = getSecretRoom(translate, "special_grass_maze", -1, "southern_cavern_1_wall", "southern_cavern_1_floor", "southern_cavern_1_secondary", "tall_grass_white", "rock", enemyList, new Loc(5, 11));

                zone.Segments.Add(structure);
            }

            {
                SingularSegment structure = new SingularSegment(-1);

                ChanceFloorGen multiGen = new ChanceFloorGen();
                multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Advanced, "small_square", -2, true, false), 10);
                structure.BaseFloor = multiGen;

                zone.Segments.Add(structure);
            }

            {
                LayeredSegment staticSegment = new LayeredSegment();
                LoadGen layout = new LoadGen();
                MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                startGen.MapID = "end_treacherous_mountain";
                layout.GenSteps.Add(PR_FILE_LOAD, startGen);
                MapEffectStep<MapLoadContext> noRescue = new MapEffectStep<MapLoadContext>();
                noRescue.Effect.OnMapRefresh.Add(0, new MapNoRescueEvent());
                layout.GenSteps.Add(PR_FLOOR_DATA, noRescue);
                staticSegment.Floors.Add(layout);
                zone.Segments.Add(staticSegment);
            }

            zone.GroundMaps.Add("end_treacherous_mountain");
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
                zone.BagRestrict = 16;
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


                // required item spreads
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 6), new IntRange(8, max_floors)), new MapItem("machine_assembly_box"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(3, 6), new IntRange(0, max_floors)), new MapItem("food_apple"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, max_floors)), new MapItem("berry_leppa"));
                AddItemSpreadZoneStep(floorSegment, new SpreadPlanQuota(new RandRange(1), new IntRange(0, 5)), new MapItem("key", 1));


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
                    monsterChanceZoneStep.Items.Add(new MapItem("loot_heart_scale", 2), new IntRange(0, max_floors), 10);//heart scale
                    monsterChanceZoneStep.Items.Add(new MapItem("key", 1), new IntRange(0, max_floors), 10);//key
                    monsterChanceZoneStep.Items.Add(new MapItem("machine_recall_box"), new IntRange(0, max_floors), 10);//link box
                    monsterChanceZoneStep.Items.Add(new MapItem("machine_assembly_box"), new IntRange(8, max_floors), 10);//assembly box
                    monsterChanceZoneStep.Items.Add(new MapItem("machine_ability_capsule"), new IntRange(0, max_floors), 10);//ability capsule

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
                        foreach (string key in IterateTMs(TMClass.Natural))
                            vaultChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 5);//TMs
                        vaultChanceZoneStep.Items.Add(new MapItem("loot_nugget"), new IntRange(0, max_floors), 200);//nugget
                        vaultChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear", 1), new IntRange(0, max_floors), 100);//amber tear
                        vaultChanceZoneStep.Items.Add(new MapItem("seed_reviver"), new IntRange(0, max_floors), 200);//reviver seed
                        vaultChanceZoneStep.Items.Add(new MapItem("seed_joy"), new IntRange(0, max_floors), 100);//joy seed
                        vaultChanceZoneStep.Items.Add(new MapItem("machine_ability_capsule"), new IntRange(0, max_floors), 200);//ability capsule
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
                            boxTreasure.Add(new MapItem("medicine_max_potion"), 30);//max potion
                            boxTreasure.Add(new MapItem("medicine_full_heal"), 100);//full heal
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

                        MultiStepSpawner<ListMapGenContext, MapItem> boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(1, 3)));

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

                    shopZoneSpawns.Add(shop, new IntRange(2, max_floors), 10);
                }
                SpreadStepRangeZoneStep shopZoneStep = new SpreadStepRangeZoneStep(new SpreadPlanQuota(new RandDecay(0, 4, 35), new IntRange(2, max_floors)), PR_SHOPS, shopZoneSpawns);
                shopZoneStep.ModStates.Add(new FlagType(typeof(ShopModGenState)));
                floorSegment.ZoneSteps.Add(shopZoneStep);

                AddEvoZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(1, max_floors)), false);

                AddHiddenStairStep(floorSegment, new SpreadPlanQuota(new RandRange(1), new IntRange(0, max_floors - 1)), 1);

                AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 2);


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
                        SpeciesItemElementSpawner<MapLoadContext> spawn = new SpeciesItemElementSpawner<MapLoadContext>(new IntRange(4), new RandRange(1), "poison", exceptFor);
                        BoxSpawner<MapLoadContext> box = new BoxSpawner<MapLoadContext>("box_deluxe", spawn);
                        List<Loc> treasureLocs = new List<Loc>();
                        treasureLocs.Add(new Loc(7, 6));
                        layout.GenSteps.Add(PR_SPAWN_ITEMS, new SpecificSpawnStep<MapLoadContext, MapItem>(box, treasureLocs));
                    }


                    //List<(InvItem, Loc)> items = new List<(InvItem, Loc)>();
                    //items.Add((new InvItem("apricorn_plain"), new Loc(13, 10)));//Plain Apricorn
                    //layout.GenSteps.Add(PR_SPAWN_ITEMS, new SpecificSpawnStep<MapLoadContext, InvItem>(items));

                    List<InvItem> treasure1 = new List<InvItem>();
                    // a specific item from poison types in the dex
                    treasure1.Add(InvItem.CreateBox("box_glittery", "medicine_amber_tear"));//Amber Tear
                    treasure1.Add(InvItem.CreateBox("box_glittery", "ammo_golden_thorn"));//Golden Thorn
                    treasure1.Add(InvItem.CreateBox("box_glittery", "loot_nugget"));//Nugget

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
                structure.BaseFloor = getSecretRoom(translate, "special_gsc_ghost", -2, "mystery_jungle_2_wall", "mystery_jungle_2_floor", "mystery_jungle_2_secondary", "tall_grass_dark", "poison", enemyList, new Loc(7, 6));

                zone.Segments.Add(structure);
            }

            {
                SingularSegment structure = new SingularSegment(-1);

                ChanceFloorGen multiGen = new ChanceFloorGen();
                multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Advanced, "small_square", -2, false, false), 10);
                multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Advanced, "tall_hall", -2, false, false), 10);
                multiGen.Spawns.Add(getMysteryRoom(translate, zone.Level, DungeonStage.Advanced, "wide_hall", -2, false, false), 10);
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
                poolSpawn.Spawns.Add(GetTeamMob("larvesta", "", "ember", "string_shot", "leech_life", "", new RandRange(31), "wander_dumb"), new IntRange(4, max_floors), 10);

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

                {

                    MobSpawn mob = GetGuardMob("espeon", "", "psychic", "", "", "", new RandRange(50), "wander_normal", "sleep");
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
                    monsterChanceZoneStep.Items.Add(new MapItem("machine_ability_capsule"), new IntRange(0, max_floors), 10);//ability capsule

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
                            foreach (string key in IterateTMs(TMClass.Starter))
                                vaultChanceZoneStep.Items.Add(new MapItem(key), new IntRange(0, max_floors), 5);//TMs
                            vaultChanceZoneStep.Items.Add(new MapItem("medicine_amber_tear", 1), new IntRange(0, max_floors), 100);//amber tear
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

                        combinedVaultZoneStep.Steps.Add(vaultChanceZoneStep);
                    }

                    floorSegment.ZoneSteps.Add(combinedVaultZoneStep);
                }


                AddMysteriosityZoneStep(floorSegment, new SpreadPlanSpaced(new RandRange(2, 4), new IntRange(0, max_floors - 1)), 5, 2);


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

                    List<InvItem> treasure1 = new List<InvItem>();
                    treasure1.Add(InvItem.CreateBox("box_glittery", "loot_nugget"));//Nugget

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
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 7, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 8, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 9, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 10, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 11, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 12, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                enemyList.Add(GetTeamMob(new MonsterID("alcremie", 13, "", Gender.Unknown), "", "sweet_kiss", "sweet_scent", "draining_kiss", "decorate", new RandRange(zone.Level)), 10);
                structure.BaseFloor = getSecretRoom(translate, "special_gsc_ghost", -1, "buried_relic_3_wall", "buried_relic_3_floor", "buried_relic_3_secondary", "", "psychic", enemyList, new Loc(7, 6));

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

            #endregion
        }
    }
}
