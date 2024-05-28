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
    public class MapInfo
    {

        public static string[] MapNames = { "test_grounds", "base_camp", "base_camp_2", "forest_camp", "cliff_camp",
            "canyon_camp", "rest_stop", "final_stop", "guildmaster_summit", "guild_path", "guild_hut", "luminous_spring", "post_office" };

        public static void AddMapData()
        {
            //DataInfo.DeleteData(PathMod.ModPath(DataManager.MAP_PATH));
            for (int ii = 0; ii < MapNames.Length; ii++)
            {
                Map data = GetMapData(MapNames[ii]);
                if (data != null)
                    DataManager.SaveData(data, DataManager.MAP_PATH, MapNames[ii], DataManager.MAP_EXT);
            }
        }
        public static void AddMapData(string name)
        {
            Map data = GetMapData(name);
            if (data != null)
                DataManager.SaveData(data, DataManager.MAP_PATH, name, DataManager.MAP_EXT);
        }


        private static void SetObstacle(Map map, int xx, int yy, string val)
        {
            map.Tiles[xx][yy] = new Tile(val, new Loc(xx, yy));
        }
        private static void SetObstacle(GroundMap map, int xx, int yy, uint val)
        {
            map.SetObstacle(xx, yy, val);
        }

        public static Map GetMapData(string name)
        {
            Map map = new Map();

            if (name == MapNames[3])
            {
                int width = 21;
                int height = 23;
                map.CreateNew(width, height);
                map.Name = new LocalText("Forest Camp");
                map.Music = "C01. Boss Battle.ogg";
                map.EdgeView = Map.ScrollEdge.Clamp;

                for (int xx = 0; xx < width; xx++)
                {
                    for (int yy = 0; yy < height; yy++)
                    {
                        if (yy <= 2)
                            SetObstacle(map, xx, yy, DataManager.Instance.GenUnbreakable);
                        else if (yy == 3)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 10 || xx >= 11 && xx < 21) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 4)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 9 || xx >= 12 && xx < 21) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 5)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 6 || xx >= 15 && xx < 21) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 6)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 5 || xx >= 16 && xx < 21) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 7)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 6 || xx >= 15 && xx < 21) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 8)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 6 || xx >= 15 && xx < 21) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 9)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 5 || xx >= 16 && xx < 21) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 10)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 6 || xx >= 15 && xx < 21) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 11)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 6 || xx >= 15 && xx < 21) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 12)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 5 || xx >= 16 && xx < 21) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 13)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 5 || xx >= 16 && xx < 21) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 14)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 6 || xx >= 15 && xx < 21) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 15)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 6 || xx >= 15 && xx < 21) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 16)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 5 || xx >= 16 && xx < 21) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 17)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 6 || xx >= 15 && xx < 21) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 18)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 9 || xx >= 12 && xx < 21) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 19)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 10 || xx >= 11 && xx < 21) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy >= 20)
                            SetObstacle(map, xx, yy, DataManager.Instance.GenUnbreakable);
                        map.Layers[0].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "ForestCamp"));
                    }
                }
                map.EntryPoints.Add(new LocRay8(new Loc(10, 5), Dir8.Up));


                ExplorerTeam team = new ExplorerTeam();
                team.SetRank("normal");
                {
                    //Snorlax
                    CharData mobData = new CharData();
                    mobData.BaseForm = new MonsterID("snorlax", 0, "normal", Gender.Male);
                    mobData.Level = 20;
                    mobData.BaseSkills[0] = new SlotSkill("tackle");
                    mobData.BaseSkills[1] = new SlotSkill("bite");
                    mobData.BaseSkills[2] = new SlotSkill("body_slam");
                    mobData.BaseSkills[3] = new SlotSkill("rest");
                    mobData.BaseIntrinsics[0] = "gluttony";
                    Character newMob = new Character(mobData);
                    team.Players.Add(newMob);
                    AITactic tactic = DataManager.Instance.GetAITactic("staged_boss");
                    newMob.Tactic = new AITactic(tactic);
                    newMob.CharLoc = new Loc(10, 4);
                    newMob.CharDir = Dir8.Down;
                    //newMob.MaxHPBonus = MonsterFormData.MAX_STAT_BOOST;
                    //newMob.DefBonus = MonsterFormData.MAX_STAT_BOOST;
                    //newMob.SpDefBonus = MonsterFormData.MAX_STAT_BOOST;
                    //newMob.SpeedBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.HP = newMob.MaxHP;
                    //newMob.HeldItem = new InvItem("seed_reviver");
                    //newMob.Skills[3].Element.Allowed = false;
                }
                map.MapTeams.Add(team);

                map.MapEffect.OnMapStarts.Add(-15, new BattlePositionEvent(new LocRay8(0, 0, Dir8.Up), new LocRay8(0, 1, Dir8.Up), new LocRay8(-1, 1, Dir8.Up), new LocRay8(-1, 1, Dir8.Up)));
                map.MapEffect.OnMapStarts.Add(-5, new BeginBattleEvent("map_clear_check"));
            }
            else if (name == MapNames[6])
            {
                map.CreateNew(10, 10);
                map.EntryPoints.Add(new LocRay8(new Loc(), Dir8.Down));
                map.Name = new LocalText("Rest Stop");
                map.Music = "A05. Cave Camp.ogg";
            }
            else if (name == MapNames[7])
            {
                int width = 25;
                int height = 27;
                map.CreateNew(width, height);
                map.Name = new LocalText("Blizzard Camp");
                map.Music = "C06. Final Battle.ogg";
                map.EdgeView = Map.ScrollEdge.Clamp;

                map.AddLayer("Cliff");
                map.Layers[1].Layer = DrawLayer.Top;

                for (int xx = 0; xx < width; xx++)
                {
                    for (int yy = 0; yy < height; yy++)
                    {
                        map.Layers[0].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "SnowCamp"));
                        if (yy >= 20 && (xx < 8 || xx >= 17))
                            map.Layers[1].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "SnowCampCliffs"));
                        else if (yy >= 17 && (xx < 7 || xx >= 18))
                            map.Layers[1].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "SnowCampCliffs"));
                        else if (yy >= 14 && (xx < 6 || xx >= 19))
                            map.Layers[1].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "SnowCampCliffs"));
                        else if (yy >= 10 && (xx < 5 || xx >= 20))
                            map.Layers[1].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "SnowCampCliffs"));
                        else if (yy >= 8 && (xx < 3 || xx >= 22))
                            map.Layers[1].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "SnowCampCliffs"));
                    }
                }

                for (int xx = 0; xx < width; xx++)
                {
                    for (int yy = 0; yy < height; yy++)
                    {
                        if (yy == 0)
                            SetObstacle(map, xx, yy, DataManager.Instance.GenUnbreakable);
                        else if (yy <= 4)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 8 || xx >= 17 && xx < 25) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 5)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 6 || xx >= 19 && xx < 25) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 6)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 4 || xx >= 21 && xx < 25) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy <= 8)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 1 || xx >= 24 && xx < 25) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 9)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 2 || xx >= 23 && xx < 25) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 10)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 3 || xx >= 22 && xx < 25) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy <= 13)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 4 || xx >= 21 && xx < 25) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy <= 17)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 5 || xx >= 20 && xx < 25) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy <= 20)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 6 || xx >= 19 && xx < 25) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy < 26)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 7 || xx >= 18 && xx < 25) ? DataManager.Instance.GenUnbreakable : DataManager.Instance.GenFloor);
                        else if (yy == 26)
                            SetObstacle(map, xx, yy, DataManager.Instance.GenUnbreakable);
                    }
                }
                SetObstacle(map, 3, 7, DataManager.Instance.GenUnbreakable);
                SetObstacle(map, 4, 7, DataManager.Instance.GenUnbreakable);
                SetObstacle(map, 5, 6, DataManager.Instance.GenUnbreakable);
                SetObstacle(map, 7, 10, DataManager.Instance.GenUnbreakable);
                SetObstacle(map, 5, 12, DataManager.Instance.GenUnbreakable);
                SetObstacle(map, 18, 11, DataManager.Instance.GenUnbreakable);
                SetObstacle(map, 19, 11, DataManager.Instance.GenUnbreakable);
                SetObstacle(map, 17, 14, DataManager.Instance.GenUnbreakable);

                map.EntryPoints.Add(new LocRay8(new Loc(12, 10), Dir8.Up));


                ExplorerTeam team = new ExplorerTeam();
                team.SetRank("normal");
                {
                    //Charizard
                    CharData mobData = new CharData();
                    mobData.BaseForm = new MonsterID("charizard", 0, "normal", Gender.Male);
                    mobData.Level = 62;
                    mobData.BaseSkills[0] = new SlotSkill("ancient_power");
                    mobData.BaseSkills[1] = new SlotSkill("air_slash");
                    mobData.BaseSkills[2] = new SlotSkill("dragon_pulse");
                    mobData.BaseSkills[3] = new SlotSkill("inferno");
                    mobData.BaseIntrinsics[0] = "blaze";
                    Character newMob = new Character(mobData);
                    team.Players.Add(newMob);
                    AITactic tactic = DataManager.Instance.GetAITactic("staged_boss");
                    newMob.Tactic = new AITactic(tactic);
                    newMob.CharLoc = new Loc(12, 8);
                    newMob.CharDir = Dir8.Down;
                    newMob.MaxHPBonus = MonsterFormData.MAX_STAT_BOOST;
                    newMob.DefBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.MDefBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.SpeedBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.HP = newMob.MaxHP;
                    newMob.Skills[3].Element.Enabled = false;
                    //newMob.HeldItem = new InvItem("seed_reviver");
                }
                {
                    //Aerodactyl
                    CharData mobData = new CharData();
                    mobData.BaseForm = new MonsterID("aerodactyl", 0, "normal", Gender.Male);
                    mobData.Level = 62;
                    mobData.BaseSkills[0] = new SlotSkill("agility");
                    mobData.BaseSkills[1] = new SlotSkill("sky_drop");
                    mobData.BaseSkills[2] = new SlotSkill("wide_guard");
                    mobData.BaseSkills[3] = new SlotSkill("stone_edge");
                    mobData.BaseIntrinsics[0] = "pressure";
                    Character newMob = new Character(mobData);
                    team.Players.Add(newMob);
                    AITactic tactic = DataManager.Instance.GetAITactic("staged_boss");
                    newMob.Tactic = new AITactic(tactic);
                    newMob.CharLoc = new Loc(11, 8);
                    newMob.CharDir = Dir8.Down;
                    newMob.MaxHPBonus = MonsterFormData.MAX_STAT_BOOST;
                    newMob.DefBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.MDefBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.SpeedBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.HP = newMob.MaxHP;
                    //newMob.HeldItem = new InvItem("seed_reviver");
                }
                {
                    //Gyarados
                    CharData mobData = new CharData();
                    mobData.BaseForm = new MonsterID("gyarados", 0, "normal", Gender.Male);
                    mobData.Level = 62;
                    mobData.BaseSkills[0] = new SlotSkill("waterfall");
                    mobData.BaseSkills[1] = new SlotSkill("dragon_rage");
                    mobData.BaseSkills[2] = new SlotSkill("ice_fang");
                    mobData.BaseSkills[3] = new SlotSkill("giga_impact");
                    mobData.BaseIntrinsics[0] = "intimidate";
                    Character newMob = new Character(mobData);
                    team.Players.Add(newMob);
                    AITactic tactic = DataManager.Instance.GetAITactic("staged_boss");
                    newMob.Tactic = new AITactic(tactic);
                    newMob.CharLoc = new Loc(13, 8);
                    newMob.CharDir = Dir8.Down;
                    newMob.MaxHPBonus = MonsterFormData.MAX_STAT_BOOST;
                    newMob.DefBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.MDefBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.SpeedBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.HP = newMob.MaxHP;
                    //newMob.HeldItem = new InvItem("seed_reviver");
                }
                {
                    //Flygon
                    CharData mobData = new CharData();
                    mobData.BaseForm = new MonsterID("flygon", 0, "normal", Gender.Male);
                    mobData.Level = 63;
                    mobData.BaseSkills[0] = new SlotSkill("draco_meteor");
                    mobData.BaseSkills[1] = new SlotSkill("dragon_rush");
                    mobData.BaseSkills[2] = new SlotSkill("rock_slide");
                    mobData.BaseSkills[3] = new SlotSkill("earthquake");
                    mobData.BaseIntrinsics[0] = "levitate";
                    Character newMob = new Character(mobData);
                    team.Players.Add(newMob);
                    AITactic tactic = DataManager.Instance.GetAITactic("lead_boss");
                    newMob.Tactic = new AITactic(tactic);
                    newMob.CharLoc = new Loc(12, 7);
                    newMob.CharDir = Dir8.Down;
                    newMob.MaxHPBonus = MonsterFormData.MAX_STAT_BOOST;
                    newMob.DefBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.MDefBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.SpeedBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.HP = newMob.MaxHP;
                    //newMob.Skills[3].Element.Enabled = false;
                    //newMob.HeldItem = new InvItem("seed_reviver");
                }
                map.MapTeams.Add(team);

                map.MapEffect.OnMapStarts.Add(-15, new BattlePositionEvent(new LocRay8(0, 0, Dir8.Up), new LocRay8(0, 1, Dir8.Up), new LocRay8(-1, 0, Dir8.Up), new LocRay8(1, 0, Dir8.Up)));
                map.MapEffect.OnMapStarts.Add(-5, new BeginBattleEvent("map_clear_check"));

                {
                    MapStatus status = new MapStatus("mysterious_force");
                    status.LoadFromData();
                    map.Status.Add(status.ID, status);
                }
                {
                    MapStatus status = new MapStatus("default_mapstatus");
                    status.StatusStates.Set(new MapIDState("diamond_dust"));
                    map.Status.Add(status.ID, status);
                }
            }
            else if (name == MapNames[8])
            {
                int width = 17;
                int height = 14;
                map.CreateNew(width, height);
                map.Name = new LocalText("**Guildmaster Summit");
                map.Music = "C06. Final Battle.ogg";
                map.EdgeView = Map.ScrollEdge.Clamp;

                for (int xx = 0; xx < width; xx++)
                {
                    for (int yy = 0; yy < height; yy++)
                    {
                        if (yy == 5 && xx >= 6 && xx <= 10 ||
                            (yy == 6 || yy == 10) && xx >= 5 && xx <= 11 ||
                            yy > 6 && yy < 10 && xx >= 4 && xx <= 12)
                        {
                            map.Tiles[xx][yy] = new Tile(DataManager.Instance.GenFloor, new Loc(xx, yy));
                        }
                        else
                            map.Tiles[xx][yy] = new Tile(DataManager.Instance.GenUnbreakable, new Loc(xx, yy));
                        map.Layers[0].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "Summit"));
                    }
                }
                map.EntryPoints.Add(new LocRay8(new Loc(8, 8), Dir8.Up));


                ExplorerTeam team = new ExplorerTeam();
                team.SetRank("master");
                //Summit
                {
                    //Xatu : 156 Magic Bounce : 248 Future Sight : 273 Wish : 403 Air Slash : 466 Ominous Wind
                    CharData mobData = new CharData();
                    mobData.BaseForm = new MonsterID("xatu", 0, "normal", Gender.Male);
                    mobData.Level = 50;
                    mobData.BaseSkills[0] = new SlotSkill("future_sight");
                    mobData.BaseSkills[1] = new SlotSkill("wish");
                    mobData.BaseSkills[2] = new SlotSkill("air_slash");
                    mobData.BaseSkills[3] = new SlotSkill("ominous_wind");
                    mobData.BaseIntrinsics[0] = "magic_bounce";
                    Character newMob = new Character(mobData);
                    team.Players.Add(newMob);
                    AITactic tactic = DataManager.Instance.GetAITactic("staged_boss");
                    newMob.Tactic = new AITactic(tactic);
                    newMob.CharLoc = new Loc(8, 5);
                    newMob.CharDir = Dir8.Down;
                    newMob.MaxHPBonus = MonsterFormData.MAX_STAT_BOOST;
                    newMob.DefBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.MDefBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.SpeedBonus = MonsterFormData.MAX_STAT_BOOST / 8;
                    newMob.HP = newMob.MaxHP;
                    newMob.EquippedItem = new InvItem("seed_reviver");
                    newMob.Skills[3].Element.Enabled = false;
                }
                {
                    //Lucario : 80 Steadfast : 396 Aura Sphere : 406 Dragon Pulse : 198 Bone Rush : 409 Drain Punch
                    CharData mobData = new CharData();
                    mobData.BaseForm = new MonsterID("lucario", 0, "normal", Gender.Male);
                    mobData.Level = 50;
                    mobData.BaseSkills[0] = new SlotSkill("aura_sphere");
                    mobData.BaseSkills[1] = new SlotSkill("dragon_pulse");
                    mobData.BaseSkills[2] = new SlotSkill("bone_rush");
                    mobData.BaseSkills[3] = new SlotSkill("drain_punch");
                    mobData.BaseIntrinsics[0] = "steadfast";
                    Character newMob = new Character(mobData);
                    team.Players.Add(newMob);
                    AITactic tactic = DataManager.Instance.GetAITactic("staged_boss");
                    newMob.Tactic = new AITactic(tactic);
                    newMob.CharLoc = new Loc(6, 6);
                    newMob.CharDir = Dir8.Down;
                    newMob.MaxHPBonus = MonsterFormData.MAX_STAT_BOOST;
                    newMob.DefBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.MDefBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.SpeedBonus = MonsterFormData.MAX_STAT_BOOST / 8;
                    newMob.HP = newMob.MaxHP;
                    newMob.EquippedItem = new InvItem("held_assault_vest");
                    newMob.Skills[3].Element.Enabled = false;
                }
                {
                    //Wigglytuff : 172 Competitive : 47 Sing : 605 Dazzling Gleam : 304 Hyper Voice : 50 Disable
                    CharData mobData = new CharData();
                    mobData.BaseForm = new MonsterID("wigglytuff", 0, "normal", Gender.Male);
                    mobData.Level = 50;
                    mobData.BaseSkills[0] = new SlotSkill("sing");
                    mobData.BaseSkills[1] = new SlotSkill("dazzling_gleam");
                    mobData.BaseSkills[2] = new SlotSkill("disable");
                    mobData.BaseSkills[3] = new SlotSkill("hyper_voice");
                    mobData.BaseIntrinsics[0] = "competitive";
                    Character newMob = new Character(mobData);
                    team.Players.Add(newMob);
                    AITactic tactic = DataManager.Instance.GetAITactic("staged_boss");
                    newMob.Tactic = new AITactic(tactic);
                    newMob.CharLoc = new Loc(10, 6);
                    newMob.CharDir = Dir8.Down;
                    newMob.MaxHPBonus = MonsterFormData.MAX_STAT_BOOST;
                    newMob.DefBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.MDefBonus = MonsterFormData.MAX_STAT_BOOST / 4;
                    newMob.SpeedBonus = MonsterFormData.MAX_STAT_BOOST / 8;
                    newMob.HP = newMob.MaxHP;
                    newMob.EquippedItem = new InvItem("berry_lum");
                    newMob.Skills[3].Element.Enabled = false;
                }
                map.MapTeams.Add(team);

                map.MapEffect.OnMapStarts.Add(-15, new BattlePositionEvent(new LocRay8(0, -1, Dir8.Up), new LocRay8(-2, 0, Dir8.Up), new LocRay8(2, 0, Dir8.Up), new LocRay8(0, 1, Dir8.Up)));
                map.MapEffect.OnMapStarts.Add(-15, new PrepareCameraEvent(new Loc(17 * GraphicsManager.TileSize / 2, GraphicsManager.ScreenHeight / 2 + 104)));
                map.MapEffect.OnMapStarts.Add(-5, new BeginBattleEvent("map_clear_check"));

                {
                    MapStatus status = new MapStatus("mysterious_force");
                    status.LoadFromData();
                    map.Status.Add(status.ID, status);
                }
            }
            else
            {
                map.CreateNew(10, 10);
                map.EntryPoints.Add(new LocRay8(new Loc(), Dir8.Down));
            }

            if (map.Name.DefaultText.StartsWith("**"))
                map.Name.DefaultText = map.Name.DefaultText.Replace("*", "");
            else if (map.Name.DefaultText != "")
                map.Released = true;

            return map;
        }

        public static void AddGroundData()
        {
            DataInfo.DeleteData(PathMod.ModPath(DataManager.GROUND_PATH));
            for (int ii = 0; ii < MapNames.Length; ii++)
                AddGroundData(MapNames[ii]);
        }
        public static void AddGroundData(string name)
        {
            GroundMap data = GetGroundData(name);
            if (data != null)
                DataManager.SaveData(data, DataManager.GROUND_PATH, name, DataManager.GROUND_EXT);
        }

        public static GroundMap GetGroundData(string name)
        {
            GroundMap map = new GroundMap();

            if (name == MapNames[0])
            {
                int width = 19;
                int height = 19;
                int texSize = 3;
                map.CreateNew(width, height, texSize);
                map.Name = new LocalText("Test Grounds");
                map.EdgeView = Map.ScrollEdge.Clamp;

                for (int xx = 0; xx < width; xx++)
                {
                    for (int yy = 0; yy < height; yy++)
                        map.Layers[0].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "CaveStop"));
                }
                for (int xx = 0; xx < width * texSize; xx++)
                {
                    for (int yy = 0; yy < height * texSize; yy++)
                    {
                        if (yy < 15)
                            SetObstacle(map, xx, yy, 1);
                        else if (yy < 16)
                            SetObstacle(map, xx, yy, (xx < 23 || width * texSize - xx <= 23) ? 1u : 0u);
                        else if (yy < 17)
                            SetObstacle(map, xx, yy, (xx < 19 || width * texSize - xx <= 19) ? 1u : 0u);
                        else if (yy < 18)
                            SetObstacle(map, xx, yy, (xx < 17 || width * texSize - xx <= 17) ? 1u : 0u);
                        else if (yy < 19)
                            SetObstacle(map, xx, yy, (xx < 15 || width * texSize - xx <= 15) ? 1u : 0u);
                        else if (yy < 20)
                            SetObstacle(map, xx, yy, (xx < 13 || width * texSize - xx <= 13) ? 1u : 0u);
                        else if (yy < 21)
                            SetObstacle(map, xx, yy, (xx < 12 || width * texSize - xx <= 12) ? 1u : 0u);
                        else if (yy < 22)
                            SetObstacle(map, xx, yy, (xx < 11 || width * texSize - xx <= 11) ? 1u : 0u);
                        else if (yy < 24)
                            SetObstacle(map, xx, yy, (xx < 10 || width * texSize - xx <= 10) ? 1u : 0u);
                        else if (yy < 25)
                            SetObstacle(map, xx, yy, (xx < 9 || width * texSize - xx <= 9) ? 1u : 0u);
                        else if (yy < 28)
                            SetObstacle(map, xx, yy, (xx < 8 || width * texSize - xx <= 8) ? 1u : 0u);
                        else if (yy < 43)
                            SetObstacle(map, xx, yy, (xx < 6 || width * texSize - xx <= 6) ? 1u : 0u);
                        else if (yy < 48)
                            SetObstacle(map, xx, yy, (xx < 7 || width * texSize - xx <= 7) ? 1u : 0u);
                        else if (yy < 55)
                            SetObstacle(map, xx, yy, (xx < 6 || width * texSize - xx <= 6) ? 1u : 0u);
                        else if (yy < 56)
                            SetObstacle(map, xx, yy, (xx < 7 || width * texSize - xx <= 7) ? 1u : 0u);
                        else if (yy < 57)
                            SetObstacle(map, xx, yy, (xx < 8 || width * texSize - xx <= 8) ? 1u : 0u);
                    }
                }

                //add Entrypoint
                map.AddMarker("entrance", new Loc(240, 160), Dir8.Down);

                //add a few flowers
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_2", 13, Dir8.Left), new Loc(180, 180)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_2", 13, Dir8.Right), new Loc(204, 160)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_2", 13, Dir8.Left), new Loc(260, 120)));
                //add signs
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Sign_Crossroads", 1), new Rect(224, 120, 24, 24), new Loc(16, 40), false, "Sign1");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Sign", 1), new Rect(224, 180, 24, 24), new Loc(8, 0), false, "Sign2");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Sign", 1), new Rect(248, 120, 24, 24), new Loc(8, 0), false, "Sign3");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Sign", 1), new Rect(200, 120, 24, 24), new Loc(8, 0), false, "Sign4");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Sign", 1), new Rect(168, 120, 24, 24), new Loc(8, 0), false, "Sign5");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(0, 452, 456, 4), true, "SouthExit");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(232, 152, 16, 16), true, "Entrance");
                    groundObject.SetTriggerType(GroundEntity.EEntityTriggerTypes.TouchOnce);
                    groundObject.Passable = true;
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(248, 168, 16, 16), true, "Activation");
                    groundObject.Passable = true;
                    groundObject.SetTriggerType(GroundEntity.EEntityTriggerTypes.TouchOnce);
                    groundObject.EntEnabled = false;
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Assembly", 30), new Rect(48, 224, 24, 24), new Loc(0, 8), false, "Assembly");
                    map.AddObject(groundObject);
                }

                //add NPCs
                {
                    GroundChar groundChar = new GroundChar(new MonsterID("mew", 0, "normal", Gender.Genderless), new Loc(180, 360), Dir8.DownLeft, "Mew");
                    map.AddMapChar(groundChar);
                }
                {
                    GroundChar groundChar = new GroundChar(new MonsterID("caterpie", 0, "normal", Gender.Female), new Loc(220, 360), Dir8.DownRight, "Caterpie");
                    map.AddMapChar(groundChar);
                }
                {
                    GroundChar groundChar = new GroundChar(new MonsterID("butterfree", 0, "normal", Gender.Female), new Loc(240, 360), Dir8.DownRight, "Butterfree");
                    map.AddMapChar(groundChar);
                }
                {
                    GroundChar groundChar = new GroundChar(new MonsterID("illumise", 0, "normal", Gender.Female), new Loc(220, 390), Dir8.DownRight, "Illumise");
                    map.AddMapChar(groundChar);
                }
                {
                    GroundChar groundChar = new GroundChar(new MonsterID("volbeat", 0, "normal", Gender.Male), new Loc(240, 390), Dir8.DownRight, "Volbeat");
                    map.AddMapChar(groundChar);
                }
                {
                    GroundChar groundChar = new GroundChar(new MonsterID("magnezone", 0, "normal", Gender.Genderless), new Loc(240, 420), Dir8.DownRight, "Magnezone");
                    map.AddMapChar(groundChar);
                }
                {
                    GroundChar groundChar = new GroundChar(new MonsterID("jigglypuff", 0, "normal", Gender.Male), new Loc(280, 360), Dir8.DownRight, "Hungrybox", "Hungrybox");
                    map.AddMapChar(groundChar);
                }
                {
                    GroundChar groundChar = new GroundChar(new MonsterID("poochyena", 0, "normal", Gender.Male), new Loc(285, 208), Dir8.DownLeft, "Poochy", "Poochy");
                    map.AddMapChar(groundChar);
                }

                
                GroundSpawner teamSpawner = new GroundSpawner("PARTNER_SPAWN", "Partner", new CharData());
                teamSpawner.Position = new Loc(260, 152);
                teamSpawner.Direction = Dir8.Down;
                teamSpawner.EntityCallbacks.Add(LuaEngine.EEntLuaEventTypes.Action);
                map.AddSpawner(teamSpawner);

                //map.AddSpawner(CreateTeamSpawner(1, new Loc(300, 192), Dir8.Down));
                //map.AddSpawner(CreateTeamSpawner(2, new Loc(300, 224), Dir8.Down));
                //map.AddSpawner(CreateTeamSpawner(3, new Loc(300, 256), Dir8.Down));

                {
                    ExplorerTeam team = new ExplorerTeam();
                    team.SetRank("normal");
                    CharData mobData = new CharData();
                    mobData.BaseForm = new MonsterID("xatu", 0, "normal", Gender.Male);
                    mobData.Level = 60;
                    mobData.BaseSkills[0] = new SlotSkill("future_sight");
                    mobData.BaseSkills[1] = new SlotSkill("wish");
                    mobData.BaseSkills[2] = new SlotSkill("air_slash");
                    mobData.BaseSkills[3] = new SlotSkill("ominous_wind");
                    mobData.BaseIntrinsics[0] = "magic_bounce";

                    GroundSpawner merchspawner = new GroundSpawner("MerchantSpawner", "Merchant", mobData);
                    merchspawner.Position = new Loc(285, 238);
                    merchspawner.Direction = Dir8.Left;
                    merchspawner.EntityCallbacks.Add(LuaEngine.EEntLuaEventTypes.Action);
                    map.AddSpawner(merchspawner);


                    GroundSpawner merchspawner2 = new GroundSpawner("MerchantSpawner2", "Merchant2", new CharData());
                    merchspawner2.Position = new Loc(245, 238);
                    merchspawner2.Direction = Dir8.Left;
                    merchspawner2.EntityCallbacks.Add(LuaEngine.EEntLuaEventTypes.Action);
                    map.AddSpawner(merchspawner2);
                }

                map.Music = "B09. Relic Tower.ogg";
            }
            else if (name == MapNames[1])
            {
                int width = 21;
                int height = 24;
                int texSize = 3;
                map.CreateNew(width, height, texSize);
                map.Name = new LocalText("Base Camp");
                map.EdgeView = Map.ScrollEdge.Clamp;

                map.Layers[0].Name = "Water";
                map.AddLayer("Land");
                for (int xx = 0; xx < width; xx++)
                {
                    for (int yy = 0; yy < height; yy++)
                    {
                        if (yy < 18)
                            map.Layers[1].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "BaseCamp"));
                        else
                        {
                            TileLayer water = new TileLayer(10);
                            for (int ii = 0; ii < 10; ii++)
                                water.Frames.Add(new TileFrame(new Loc(xx, yy - 18 + 6 * ii), "BeachWaves"));
                            map.Layers[0].Tiles[xx][yy] = new AutoTile(water);

                            if (yy < 19)
                                map.Layers[1].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "BaseCamp"));
                            else if (yy < 20 && xx > 8 && xx < 12)
                                map.Layers[1].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "BaseCamp"));
                        }
                    }
                }
                for (int xx = 0; xx < width * texSize; xx++)
                {
                    for (int yy = 0; yy < height * texSize; yy++)
                    {
                        if (yy < 2)
                            SetObstacle(map, xx, yy, (xx < 28 || width * texSize - xx <= 28) ? 1u : 0u);
                        else if (yy < 4)
                            SetObstacle(map, xx, yy, (xx < 27 || width * texSize - xx <= 27) ? 1u : 0u);
                        else if (yy < 25)
                            SetObstacle(map, xx, yy, (xx < 25 || width * texSize - xx <= 25) ? 1u : 0u);
                        else if (yy < 26)
                            SetObstacle(map, xx, yy, (xx < 24 || width * texSize - xx <= 24) ? 1u : 0u);
                        else if (yy < 27)
                            SetObstacle(map, xx, yy, (xx < 21 || width * texSize - xx <= 21) ? 1u : 0u);
                        else if (yy < 28)
                            SetObstacle(map, xx, yy, (xx < 19 || width * texSize - xx <= 19) ? 1u : 0u);
                        else if (yy < 29)
                            SetObstacle(map, xx, yy, (xx < 18 || width * texSize - xx <= 18) ? 1u : 0u);
                        else if (yy < 30)
                            SetObstacle(map, xx, yy, (xx < 15 || width * texSize - xx <= 15) ? 1u : 0u);
                        else if (yy < 31)
                            SetObstacle(map, xx, yy, (xx < 13 || width * texSize - xx <= 13) ? 1u : 0u);
                        else if (yy < 35)
                            SetObstacle(map, xx, yy, 0);
                        else if (yy < 36)
                            SetObstacle(map, xx, yy, (xx < 8 || width * texSize - xx <= 8) ? 1u : 0u);
                        else if (yy < 37)
                            SetObstacle(map, xx, yy, (xx < 9 || width * texSize - xx <= 9) ? 1u : 0u);
                        else if (yy < 38)
                            SetObstacle(map, xx, yy, (xx < 10 || width * texSize - xx <= 10) ? 1u : 0u);
                        else if (yy < 43)
                            SetObstacle(map, xx, yy, (xx < 12 || width * texSize - xx <= 12) ? 1u : 0u);
                        else if (yy < 46)
                            SetObstacle(map, xx, yy, (xx < 11 || width * texSize - xx <= 11) ? 1u : 0u);
                        else if (yy < 52)
                            SetObstacle(map, xx, yy, (xx < 12 || width * texSize - xx <= 12) ? 1u : 0u);
                        else if (yy < 54)
                            SetObstacle(map, xx, yy, (xx < 11 || width * texSize - xx <= 11) ? 1u : 0u);
                        else if (yy < 59)
                            SetObstacle(map, xx, yy, (xx < 29 || width * texSize - xx <= 29) ? 1u : 0u);
                        else
                            SetObstacle(map, xx, yy, 1);
                    }
                }

                map.AddMarker("entrance_center", new Loc(244, 250), Dir8.Down);
                map.AddMarker("entrance_east", new Loc(484, 250), Dir8.Left);
                map.AddMarker("entrance_west", new Loc(4, 250), Dir8.Right);
                map.AddMarker("entrance_north", new Loc(244, 4), Dir8.Down);

                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_5", 13, Dir8.Left), new Loc(344, 224)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_5", 13, Dir8.Left), new Loc(328, 392)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_5", 13, Dir8.Right), new Loc(88, 344)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_5", 13, Dir8.Right), new Loc(272, 72)));

                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_4", 13, Dir8.Left), new Loc(104, 224)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_4", 13, Dir8.Right), new Loc(376, 328)));

                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_2", 13, Dir8.Left), new Loc(160, 16)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_2", 13, Dir8.Left), new Loc(352, 64)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_2", 13, Dir8.Left), new Loc(408, 128)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_2", 13, Dir8.Left), new Loc(72, 192)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_2", 13, Dir8.Left), new Loc(384, 200)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_2", 13, Dir8.Left), new Loc(48, 360)));

                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_1", 13, Dir8.Left), new Loc(136, 96)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_1", 13, Dir8.Left), new Loc(448, 176)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_1", 13, Dir8.Left), new Loc(40, 288)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_1", 13, Dir8.Left), new Loc(432, 384)));

                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_3", 13, Dir8.Left), new Loc(312, 0)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_3", 13, Dir8.Left), new Loc(8, 192)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_3", 13, Dir8.Left), new Loc(440, 288)));


                //Reload the map script
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(0, 0, 456, 4), true, "North_Exit");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(0, 0, 456, 4), true, "First_North_Exit");
                    groundObject.EntEnabled = false;
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(500, 0, 4, 576), true, "East_Exit");
                    map.AddObject(groundObject);
                }
                {
                    //this is a debug entrance; it'll be blocked by the tutorial giver and never be accessed in actual gameplay
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(0, 0, 4, 576), true, "West_Exit");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Sign_Crossroads", 1), new Rect(312, 208, 24, 24), new Loc(16, 40), false, "Sign");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Assembly", 1, 0, 0), new Rect(168, 200, 24, 24), new Loc(0, 8), false, "Assembly");
                    //groundObject.Events.Add(new AssemblyEvent());
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Storage", 1), new Rect(144, 216, 24, 24), new Loc(4, 8), false, "Storage");
                    //groundObject.Events.Add(new StorageEvent());
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Logs_Large", 1), new Rect(8, 240, 28, 32), new Loc(6, 0), false, "West_LogPile");
                    groundObject.EntEnabled = false;
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Logs_Large", 1), new Rect(470, 240, 28, 32), new Loc(6, 0), false, "East_LogPile");
                    groundObject.EntEnabled = false;
                    map.AddObject(groundObject);
                }


                map.AddSpawner(CreateTeamSpawner(1, new Loc(178, 326), Dir8.Down));
                map.AddSpawner(CreateTeamSpawner(2, new Loc(310, 326), Dir8.Down));
                map.AddSpawner(CreateTeamSpawner(3, new Loc(244, 382), Dir8.Down));

                {
                    GroundChar groundChar = new GroundChar(new MonsterID("noctowl", 0, "normal", Gender.Male), new Loc(8, 252), Dir8.Down, "Noctowl");
                    groundChar.CharDir = Dir8.Right;
                    map.AddMapChar(groundChar);
                }


                map.Music = "A01. Title.ogg";
            }
            else if (name == MapNames[2])
            {
                int width = 114;
                int height = 96;
                int texSize = 1;
                map.CreateNew(width, height, texSize);
                map.Name = new LocalText("Base Camp");
                map.EdgeView = Map.ScrollEdge.Clamp;

                for (int xx = 0; xx < width; xx++)
                {
                    for (int yy = 0; yy < height; yy++)
                        map.Layers[0].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "TownBase"));
                }


                for (int xx = 0; xx < width * texSize; xx++)
                {
                    for (int yy = 0; yy < height * texSize; yy++)
                    {
                        if (yy == 0)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 14 || xx >= 26 && xx < 114) ? 1u : 0u);
                        else if (yy == 1)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 15 || xx >= 25 && xx < 114) ? 1u : 0u);
                        else if (yy == 2)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 16 || xx >= 24 && xx < 114) ? 1u : 0u);
                        else if (yy == 3)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 16 || xx >= 24 && xx < 114) ? 1u : 0u);
                        else if (yy == 4)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 16 || xx >= 24 && xx < 114) ? 1u : 0u);
                        else if (yy == 5)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 16 || xx >= 24 && xx < 114) ? 1u : 0u);
                        else if (yy == 6)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 16 || xx >= 22 && xx < 114) ? 1u : 0u);
                        else if (yy == 7)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 15 || xx >= 22 && xx < 114) ? 1u : 0u);
                        else if (yy == 8)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 13 || xx >= 22 && xx < 114) ? 1u : 0u);
                        else if (yy == 9)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 12 || xx >= 22 && xx < 24 || xx >= 79 && xx < 81 || xx >= 92 && xx < 114) ? 1u : 0u);
                        else if (yy == 10)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 12 || xx >= 22 && xx < 24 || xx >= 79 && xx < 81 || xx >= 93 && xx < 114) ? 1u : 0u);
                        else if (yy == 11)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 11 || xx >= 22 && xx < 24 || xx >= 75 && xx < 77 || xx >= 79 && xx < 81 || xx >= 93 && xx < 99 || xx >= 101 && xx < 114) ? 1u : 0u);
                        else if (yy == 12)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 9 || xx >= 22 && xx < 24 || xx >= 75 && xx < 77 || xx >= 95 && xx < 97 || xx >= 103 && xx < 114) ? 1u : 0u);
                        else if (yy == 13)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 24 || xx >= 74 && xx < 76 || xx >= 78 && xx < 80 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 14)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 24 || xx >= 74 && xx < 76 || xx >= 78 && xx < 80 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 15)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 7 || xx >= 22 && xx < 24 || xx >= 76 && xx < 78 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 16)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 6 || xx >= 12 && xx < 14 || xx >= 22 && xx < 24 || xx >= 76 && xx < 78 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 17)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 7 || xx >= 12 && xx < 14 || xx >= 22 && xx < 24 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 18)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 12 && xx < 14 || xx >= 22 && xx < 79 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 19)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 51 || xx >= 53 && xx < 79 || xx >= 101 && xx < 103 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 20)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 24 || xx >= 38 && xx < 51 || xx >= 53 && xx < 66 || xx >= 101 && xx < 103 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 21)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 24 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 22)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 23)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 7 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 24)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 6 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 25)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 7 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 26)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 39 || xx >= 63 && xx < 80 || xx >= 86 && xx < 88 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 27)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 39 || xx >= 63 && xx < 80 || xx >= 86 && xx < 88 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 28)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 39 || xx >= 63 && xx < 82 || xx >= 86 && xx < 88 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 29)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 24 || xx >= 55 && xx < 57 || xx >= 80 && xx < 82 || xx >= 86 && xx < 88 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 30)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 24 || xx >= 55 && xx < 57 || xx >= 80 && xx < 82 || xx >= 86 && xx < 88 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 31)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 7 || xx >= 22 && xx < 24 || xx >= 36 && xx < 40 || xx >= 55 && xx < 57 || xx >= 80 && xx < 82 || xx >= 86 && xx < 88 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 32)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 6 || xx >= 22 && xx < 24 || xx >= 35 && xx < 41 || xx >= 80 && xx < 82 || xx >= 86 && xx < 88 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 33)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 7 || xx >= 22 && xx < 24 || xx >= 34 && xx < 42 || xx >= 80 && xx < 82 || xx >= 86 && xx < 88 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 34)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 24 || xx >= 34 && xx < 42 || xx >= 80 && xx < 82 || xx >= 86 && xx < 88 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 35)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 24 || xx >= 34 && xx < 42 || xx >= 80 && xx < 82 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 36)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 24 || xx >= 34 && xx < 42 || xx >= 80 && xx < 82 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 37)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 24 || xx >= 34 && xx < 42 || xx >= 80 && xx < 82 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 38)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 24 || xx >= 35 && xx < 41 || xx >= 80 && xx < 82 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 39)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 7 || xx >= 22 && xx < 24 || xx >= 37 && xx < 39 || xx >= 66 && xx < 68 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 40)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 6 || xx >= 66 && xx < 68 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 41)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 7 || xx >= 66 && xx < 68 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 42)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 43)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 12 && xx < 14 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 44)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 12 && xx < 14 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 45)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 12 && xx < 14 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 46)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 47)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 7 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 48)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 6 || xx >= 27 && xx < 42 || xx >= 61 && xx < 80 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 49)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 7 || xx >= 27 && xx < 42 || xx >= 61 && xx < 80 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 50)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 26 && xx < 42 || xx >= 61 && xx < 80 || xx >= 86 && xx < 88 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 51)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 24 && xx < 28 || xx >= 40 && xx < 42 || xx >= 61 && xx < 63 || xx >= 80 && xx < 82 || xx >= 86 && xx < 88 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 52)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 28 || xx >= 40 && xx < 42 || xx >= 61 && xx < 63 || xx >= 80 && xx < 82 || xx >= 86 && xx < 88 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 53)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 28 || xx >= 40 && xx < 42 || xx >= 61 && xx < 63 || xx >= 80 && xx < 82 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 54)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 24 || xx >= 40 && xx < 42 || xx >= 59 && xx < 63 || xx >= 80 && xx < 82 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 55)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 7 || xx >= 22 && xx < 24 || xx >= 40 && xx < 63 || xx >= 80 && xx < 82 || xx >= 95 && xx < 97 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 56)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 6 || xx >= 22 && xx < 24 || xx >= 40 && xx < 63 || xx >= 80 && xx < 82 || xx >= 90 && xx < 94 || xx >= 95 && xx < 97 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 57)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 7 || xx >= 22 && xx < 24 || xx >= 40 && xx < 58 || xx >= 80 && xx < 82 || xx >= 89 && xx < 97 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 58)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 24 || xx >= 80 && xx < 82 || xx >= 88 && xx < 96 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 59)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 24 || xx >= 80 && xx < 82 || xx >= 88 && xx < 96 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 60)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 24 || xx >= 80 && xx < 82 || xx >= 88 && xx < 98 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 61)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 24 || xx >= 80 && xx < 82 || xx >= 88 && xx < 99 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 62)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 8 || xx >= 22 && xx < 24 || xx >= 78 && xx < 82 || xx >= 88 && xx < 100 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 63)
                            map.SetObstacle(xx, yy, (xx >= 1 && xx < 7 || xx >= 22 && xx < 24 || xx >= 78 && xx < 82 || xx >= 89 && xx < 100 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 64)
                            map.SetObstacle(xx, yy, (xx >= 3 && xx < 5 || xx >= 22 && xx < 30 || xx >= 32 && xx < 41 || xx >= 67 && xx < 72 || xx >= 75 && xx < 82 || xx >= 91 && xx < 100 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 65)
                            map.SetObstacle(xx, yy, (xx >= 22 && xx < 30 || xx >= 32 && xx < 41 || xx >= 67 && xx < 72 || xx >= 75 && xx < 78 || xx >= 80 && xx < 82 || xx >= 92 && xx < 100 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 66)
                            map.SetObstacle(xx, yy, (xx >= 22 && xx < 26 || xx >= 36 && xx < 44 || xx >= 46 && xx < 58 || xx >= 60 && xx < 68 || xx >= 80 && xx < 82 || xx >= 92 && xx < 100 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 67)
                            map.SetObstacle(xx, yy, (xx >= 22 && xx < 26 || xx >= 36 && xx < 44 || xx >= 46 && xx < 58 || xx >= 60 && xx < 68 || xx >= 80 && xx < 82 || xx >= 93 && xx < 99 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 68)
                            map.SetObstacle(xx, yy, (xx >= 22 && xx < 29 || xx >= 33 && xx < 37 || xx >= 63 && xx < 65 || xx >= 95 && xx < 97 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 69)
                            map.SetObstacle(xx, yy, (xx >= 25 && xx < 29 || xx >= 33 && xx < 37 || xx >= 63 && xx < 65 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 70)
                            map.SetObstacle(xx, yy, (xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 71)
                            map.SetObstacle(xx, yy, (xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 72)
                            map.SetObstacle(xx, yy, (xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 73)
                            map.SetObstacle(xx, yy, (xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 74)
                            map.SetObstacle(xx, yy, (xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 75)
                            map.SetObstacle(xx, yy, (xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 76)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 2 || xx >= 6 && xx < 10 || xx >= 42 && xx < 44 || xx >= 99 && xx < 101 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 77)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 3 || xx >= 5 && xx < 11 || xx >= 42 && xx < 44 || xx >= 99 && xx < 101 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 78)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 12 || xx >= 40 && xx < 44 || xx >= 99 && xx < 101 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 79)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 12 || xx >= 40 && xx < 42 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 80)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 14 || xx >= 18 && xx < 22 || xx >= 26 && xx < 30 || xx >= 34 && xx < 38 || xx >= 40 && xx < 46 || xx >= 50 && xx < 54 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 81)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 15 || xx >= 17 && xx < 23 || xx >= 25 && xx < 31 || xx >= 33 && xx < 39 || xx >= 40 && xx < 47 || xx >= 49 && xx < 55 || xx >= 105 && xx < 114) ? 1u : 0u);
                        else if (yy == 82)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 56 || xx >= 104 && xx < 114) ? 1u : 0u);
                        else if (yy == 83)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 56 || xx >= 103 && xx < 114) ? 1u : 0u);
                        else if (yy == 84)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 58 || xx >= 62 && xx < 66 || xx >= 70 && xx < 74 || xx >= 78 && xx < 82 || xx >= 86 && xx < 90 || xx >= 94 && xx < 98 || xx >= 102 && xx < 114) ? 1u : 0u);
                        else if (yy == 85)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 59 || xx >= 61 && xx < 67 || xx >= 69 && xx < 75 || xx >= 77 && xx < 83 || xx >= 85 && xx < 91 || xx >= 93 && xx < 99 || xx >= 101 && xx < 114) ? 1u : 0u);
                        else if (yy == 86)
                            map.SetObstacle(xx, yy, 1);
                    }
                }

                map.AddMarker("entrance_west", new Loc(4, 568), Dir8.Right);
                map.AddMarker("entrance_north", new Loc(152, 4), Dir8.Down);
                map.AddMarker("entrance_post_office", new Loc(408, 168), Dir8.Down);


                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(192, 80)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(112, 104)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(72, 144)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(80, 192)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(72, 248)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(216, 248)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(112, 296)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(96, 368)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(176, 376)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(112, 408)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(64, 432)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(184, 584)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(128, 600)));

                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(272, 88)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(504, 96)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(312, 112)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(256, 224)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(344, 248)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(464, 272)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(288, 400)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(512, 400)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(320, 464)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(400, 472)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(496, 472)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(456, 584)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(368, 608)));

                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(560, 72)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(640, 128)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(576, 224)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(792, 264)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(568, 272)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(688, 288)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(616, 312)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(736, 312)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(792, 384)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(696, 544)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(648, 584)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(536, 640)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(688, 640)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(768, 632)));



                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Stump_Chair", 0, Dir8.Left), new Loc(688, 144)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Stump_Chair", 0, Dir8.Left), new Loc(688, 82)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Stump_Chair", 0, Dir8.Left), new Loc(720, 112)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Stump_Chair", 0, Dir8.Left), new Loc(656, 112)));

                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Stump_Chair", 0, Dir8.Left), new Loc(760, 224)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Stump_Chair", 0, Dir8.Left), new Loc(760, 162)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Stump_Chair", 0, Dir8.Left), new Loc(728, 192)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Stump_Chair", 0, Dir8.Left), new Loc(792, 192)));


                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Stump_Table", 0, Dir8.Left), new Rect(672, 104, 48, 40), new Loc(0, 8), false, "Stump_Table");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Stump_Table", 0, Dir8.Left), new Rect(744, 184, 48, 40), new Loc(0, 8), false, "Stump_Table");
                    map.AddObject(groundObject);
                }

                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0, Dir8.Left), new Rect(0, 0, 4, 768), true, "West_Exit");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0, Dir8.Left), new Rect(0, 0, 912, 4), true, "North_Exit");
                    map.AddObject(groundObject);
                }


                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Mission_Board", 0, Dir8.Left), new Rect(88, 544, 40, 24), new Loc(16, 32), false, "Mission_Board");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0, Dir8.Left), new Rect(240, 512, 16, 16), false, "Shop");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0, Dir8.Left), new Rect(576, 512, 24, 16), false, "Appraisal");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0, Dir8.Left), new Rect(464, 528, 16, 16), false, "Swap");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0, Dir8.Left), new Rect(352, 528, 16, 16), false, "Tutor");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0, Dir8.Left), new Rect(240, 144, 16, 16), false, "Locator");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0, Dir8.Left), new Rect(576, 144, 16, 16), false, "Juice");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0, Dir8.Left), new Rect(240, 144, 16, 16), false, "Music");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0, Dir8.Left), new Rect(408, 152, 16, 16), true, "Post_Office_Entrance");
                    map.AddObject(groundObject);
                }

                {
                    GroundChar groundChar = new GroundChar(new MonsterID("kecleon", 0, "normal", Gender.Male), new Loc(240, 496), Dir8.Down, "Shop_Owner");
                    groundChar.CharDir = Dir8.Down;
                    map.AddMapChar(groundChar);
                }
                {
                    GroundChar groundChar = new GroundChar(new MonsterID("blaziken", 0, "normal", Gender.Genderless), new Loc(352, 512), Dir8.Down, "Tutor_Owner");
                    groundChar.CharDir = Dir8.Down;
                    map.AddMapChar(groundChar);
                }
                {
                    GroundChar groundChar = new GroundChar(new MonsterID("sableye", 0, "normal", Gender.Male), new Loc(464, 512), Dir8.Down, "Swap_Owner");
                    groundChar.CharDir = Dir8.Down;
                    map.AddMapChar(groundChar);
                }
                {
                    GroundChar groundChar = new GroundChar(new MonsterID("voltorb", 0, "normal", Gender.Male), new Loc(580, 496), Dir8.Down, "Appraisal_Owner");
                    groundChar.CharDir = Dir8.Down;
                    map.AddMapChar(groundChar);
                }
                {
                    GroundChar groundChar = new GroundChar(new MonsterID("nosepass", 0, "normal", Gender.Male), new Loc(240, 128), Dir8.Down, "Locator_Owner");
                    groundChar.CharDir = Dir8.Down;
                    map.AddMapChar(groundChar);
                }
                {
                    GroundChar groundChar = new GroundChar(new MonsterID("kricketune", 0, "normal", Gender.Male), new Loc(240, 128), Dir8.Down, "Music_Owner");
                    groundChar.CharDir = Dir8.Down;
                    map.AddMapChar(groundChar);
                }
                {
                    GroundChar groundChar = new GroundChar(new MonsterID("shuckle", 0, "normal", Gender.Male), new Loc(576, 126), Dir8.Down, "Juice_Owner");
                    groundChar.CharDir = Dir8.Down;
                    map.AddMapChar(groundChar);
                }


                map.AddSpawner(CreateAssemblySpawner(1, new Loc(352, 384), Dir8.UpRight));
                map.AddSpawner(CreateAssemblySpawner(2, new Loc(448, 408), Dir8.UpLeft));
                map.AddSpawner(CreateAssemblySpawner(3, new Loc(200, 240), Dir8.Down));
                map.AddSpawner(CreateAssemblySpawner(4, new Loc(600, 288), Dir8.Down));
                map.AddSpawner(CreateAssemblySpawner(5, new Loc(352, 232), Dir8.Down));
                map.AddSpawner(CreateAssemblySpawner(6, new Loc(296, 592), Dir8.UpLeft));
                map.AddSpawner(CreateAssemblySpawner(7, new Loc(512, 280), Dir8.DownLeft));
                map.AddSpawner(CreateAssemblySpawner(8, new Loc(80, 400), Dir8.DownRight));
                map.AddSpawner(CreateAssemblySpawner(9, new Loc(96, 424), Dir8.UpLeft));
                map.AddSpawner(CreateAssemblySpawner(10, new Loc(120, 208), Dir8.UpRight));
                map.AddSpawner(CreateAssemblySpawner(11, new Loc(480, 240), Dir8.UpLeft));
                map.AddSpawner(CreateAssemblySpawner(12, new Loc(232, 280), Dir8.Down));
                map.AddSpawner(CreateAssemblySpawner(13, new Loc(688, 80), Dir8.Down));
                map.AddSpawner(CreateAssemblySpawner(14, new Loc(96, 280), Dir8.DownRight));
                map.AddSpawner(CreateAssemblySpawner(15, new Loc(560, 248), Dir8.Down));
                map.AddSpawner(CreateAssemblySpawner(16, new Loc(72, 128), Dir8.Down));
                map.AddSpawner(CreateAssemblySpawner(17, new Loc(792, 416), Dir8.Down));
                map.AddSpawner(CreateAssemblySpawner(18, new Loc(760, 160), Dir8.Down));
                map.AddSpawner(CreateAssemblySpawner(19, new Loc(424, 600), Dir8.DownRight));
                map.AddSpawner(CreateAssemblySpawner(20, new Loc(792, 320), Dir8.Left));
                map.AddSpawner(CreateAssemblySpawner(21, new Loc(752, 294), Dir8.DownRight));
                map.AddSpawner(CreateAssemblySpawner(22, new Loc(760, 368), Dir8.Down));
                map.AddSpawner(CreateAssemblySpawner(23, new Loc(608, 624), Dir8.Down));
                map.AddSpawner(CreateAssemblySpawner(24, new Loc(776, 584), Dir8.Down));
                map.AddSpawner(CreateAssemblySpawner(25, new Loc(728, 624), Dir8.Right));
                map.AddSpawner(CreateAssemblySpawner(26, new Loc(792, 184), Dir8.Left));


                map.Music = "A02. Base Town.ogg";
            }
            else if (name == MapNames[3])
            {
                int width = 21;
                int height = 23;
                int texSize = 3;
                map.CreateNew(width, height, texSize);
                map.Name = new LocalText("Forest Camp");
                map.EdgeView = Map.ScrollEdge.Clamp;

                for (int xx = 0; xx < width; xx++)
                {
                    for (int yy = 0; yy < height; yy++)
                        map.Layers[0].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "ForestCamp"));
                }
                for (int xx = 0; xx < width * texSize; xx++)
                {
                    for (int yy = 0; yy < height * texSize; yy++)
                    {
                        if (yy <= 6)
                            SetObstacle(map, xx, yy, 1);
                        else if (yy == 7)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 28 || xx >= 35 && xx < 63) ? 1u : 0u);
                        else if (yy == 8)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 28 || xx >= 35 && xx < 63) ? 1u : 0u);
                        else if (yy == 9)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 28 || xx >= 35 && xx < 63) ? 1u : 0u);
                        else if (yy == 10)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 28 || xx >= 35 && xx < 63) ? 1u : 0u);
                        else if (yy == 11)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 27 || xx >= 36 && xx < 63) ? 1u : 0u);
                        else if (yy == 12)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 18 || xx >= 21 && xx < 26 || xx >= 37 && xx < 42 || xx >= 45 && xx < 63) ? 1u : 0u);
                        else if (yy == 13)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 17 || xx >= 22 && xx < 25 || xx >= 38 && xx < 41 || xx >= 46 && xx < 63) ? 1u : 0u);
                        else if (yy == 14)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 47 && xx < 63) ? 1u : 0u);
                        else if (yy == 15)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 47 && xx < 63) ? 1u : 0u);
                        else if (yy == 16)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 15 || xx >= 48 && xx < 63) ? 1u : 0u);
                        else if (yy == 17)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 14 || xx >= 49 && xx < 63) ? 1u : 0u);
                        else if (yy == 18)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 13 || xx >= 50 && xx < 63) ? 1u : 0u);
                        else if (yy == 19)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 13 || xx >= 50 && xx < 63) ? 1u : 0u);
                        else if (yy == 20)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 14 || xx >= 49 && xx < 63) ? 1u : 0u);
                        else if (yy == 21)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 15 || xx >= 48 && xx < 63) ? 1u : 0u);
                        else if (yy == 22)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 47 && xx < 63) ? 1u : 0u);
                        else if (yy == 23)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 47 && xx < 63) ? 1u : 0u);
                        else if (yy == 24)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 47 && xx < 63) ? 1u : 0u);
                        else if (yy == 25)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 47 && xx < 63) ? 1u : 0u);
                        else if (yy == 26)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 15 || xx >= 48 && xx < 63) ? 1u : 0u);
                        else if (yy == 27)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 14 || xx >= 49 && xx < 63) ? 1u : 0u);
                        else if (yy == 28)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 13 || xx >= 50 && xx < 63) ? 1u : 0u);
                        else if (yy == 29)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 13 || xx >= 50 && xx < 63) ? 1u : 0u);
                        else if (yy == 30)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 14 || xx >= 49 && xx < 63) ? 1u : 0u);
                        else if (yy == 31)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 15 || xx >= 48 && xx < 63) ? 1u : 0u);
                        else if (yy == 32)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 47 && xx < 63) ? 1u : 0u);
                        else if (yy == 33)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 47 && xx < 63) ? 1u : 0u);
                        else if (yy == 34)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 47 && xx < 63) ? 1u : 0u);
                        else if (yy == 35)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 47 && xx < 63) ? 1u : 0u);
                        else if (yy == 36)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 15 || xx >= 48 && xx < 63) ? 1u : 0u);
                        else if (yy == 37)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 14 || xx >= 49 && xx < 63) ? 1u : 0u);
                        else if (yy == 38)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 13 || xx >= 50 && xx < 63) ? 1u : 0u);
                        else if (yy == 39)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 13 || xx >= 50 && xx < 63) ? 1u : 0u);
                        else if (yy == 40)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 14 || xx >= 49 && xx < 63) ? 1u : 0u);
                        else if (yy == 41)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 15 || xx >= 48 && xx < 63) ? 1u : 0u);
                        else if (yy == 42)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 47 && xx < 63) ? 1u : 0u);
                        else if (yy == 43)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 47 && xx < 63) ? 1u : 0u);
                        else if (yy == 44)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 47 && xx < 63) ? 1u : 0u);
                        else if (yy == 45)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 47 && xx < 63) ? 1u : 0u);
                        else if (yy == 46)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 15 || xx >= 48 && xx < 63) ? 1u : 0u);
                        else if (yy == 47)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 14 || xx >= 49 && xx < 63) ? 1u : 0u);
                        else if (yy == 48)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 13 || xx >= 50 && xx < 63) ? 1u : 0u);
                        else if (yy == 49)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 13 || xx >= 50 && xx < 63) ? 1u : 0u);
                        else if (yy == 50)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 14 || xx >= 49 && xx < 63) ? 1u : 0u);
                        else if (yy == 51)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 15 || xx >= 48 && xx < 63) ? 1u : 0u);
                        else if (yy == 52)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 47 && xx < 63) ? 1u : 0u);
                        else if (yy == 53)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 47 && xx < 63) ? 1u : 0u);
                        else if (yy == 54)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 17 || xx >= 46 && xx < 63) ? 1u : 0u);
                        else if (yy == 55)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 18 || xx >= 21 && xx < 26 || xx >= 37 && xx < 42 || xx >= 45 && xx < 63) ? 1u : 0u);
                        else if (yy == 56)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 27 || xx >= 36 && xx < 63) ? 1u : 0u);
                        else if (yy == 57)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 28 || xx >= 35 && xx < 63) ? 1u : 0u);
                        else if (yy == 58)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 28 || xx >= 35 && xx < 63) ? 1u : 0u);
                        else if (yy == 59)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 29 || xx >= 34 && xx < 63) ? 1u : 0u);
                        else if (yy == 60)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 30 || xx >= 33 && xx < 63) ? 1u : 0u);
                        else if (yy >= 61)
                            SetObstacle(map, xx, yy, 1);
                    }
                }

                map.AddMarker("entrance_south", new Loc(244, 460), Dir8.Up);
                map.AddMarker("entrance_center", new Loc(244, 270), Dir8.Up);
                map.AddMarker("entrance_north", new Loc(244, 80), Dir8.Down);

                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(0, 76, 504, 4), true, "North_Exit");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(0, 480, 504, 4), true, "South_Exit");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Assembly", 1, 0, 0), new Rect(176, 112, 24, 24), new Loc(0, 8), false, "Assembly");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Storage", 1), new Rect(304, 112, 24, 24), new Loc(4, 8), false, "Storage");
                    map.AddObject(groundObject);
                }
                {
                    GroundChar groundChar = new GroundChar(new MonsterID("snorlax", 0, "normal", Gender.Male), new Loc(244, 100), Dir8.Down, "Snorlax");
                    groundChar.CharDir = Dir8.Down;
                    map.AddMapChar(groundChar);
                }

                map.AddSpawner(CreateTeamSpawner(1, new Loc(144, 96), Dir8.DownRight));
                map.AddSpawner(CreateTeamSpawner(2, new Loc(120, 128), Dir8.DownRight));
                map.AddSpawner(CreateTeamSpawner(3, new Loc(120, 160), Dir8.Right));

                map.Music = "A02. Base Town.ogg";
            }
            else if (name == MapNames[4])
            {
                int width = 33;
                int height = 22;
                int texSize = 3;
                map.CreateNew(width, height, texSize);
                map.Name = new LocalText("Cliff Camp");
                map.EdgeView = Map.ScrollEdge.Clamp;
                map.Background = new MapBG(new BGAnimData("Cloudy_Sky", 1), new Loc(-5, 0));
                int offset = 4;
                for (int xx = 0; xx < width; xx++)
                {
                    for (int yy = offset; yy < height; yy++)
                    {
                        if (yy == offset && xx > 0)
                            continue;
                        if (yy == offset + 1 && (xx > 1 && xx < 10 || xx > 14))
                            continue;
                        if (yy == offset + 2 && (xx > 3 && xx < 9 || xx > 15 && xx < 23 || xx > 27))
                            continue;
                        map.Layers[0].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy - offset), "CliffCamp"));
                    }
                }
                for (int xx = 0; xx < width * texSize; xx++)
                {
                    for (int yy = 0; yy < height * texSize; yy++)
                    {
                        if (yy < 17)
                            SetObstacle(map, xx, yy, 1);
                        else if (yy == 17)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 33 || xx >= 42 && xx < 99) ? 1u : 0u);
                        else if (yy == 18)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 33 || xx >= 42 && xx < 99) ? 1u : 0u);
                        else if (yy == 19)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 33 || xx >= 43 && xx < 99) ? 1u : 0u);
                        else if (yy == 20)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 33 || xx >= 43 && xx < 72 || xx >= 81 && xx < 99) ? 1u : 0u);
                        else if (yy == 21)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 33 || xx >= 44 && xx < 72 || xx >= 81 && xx < 99) ? 1u : 0u);
                        else if (yy == 22)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 18 || xx >= 81 && xx < 99) ? 1u : 0u);
                        else if (yy == 23)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 18 || xx >= 83 && xx < 99) ? 1u : 0u);
                        else if (yy == 24)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 18 || xx >= 83 && xx < 99) ? 1u : 0u);
                        else if (yy == 25)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 7) ? 1u : 0u);
                        else if (yy == 26)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 6) ? 1u : 0u);
                        else if (yy == 27)
                            SetObstacle(map, xx, yy, (xx >= 2 && xx < 4) ? 1u : 0u);
                        else if (yy == 44)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 4) ? 1u : 0u);
                        else if (yy == 45)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 5) ? 1u : 0u);
                        else if (yy == 46)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 6) ? 1u : 0u);
                        else if (yy == 47)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 7 || xx >= 9 && xx < 13) ? 1u : 0u);
                        else if (yy == 48)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 7 || xx >= 8 && xx < 14) ? 1u : 0u);
                        else if (yy == 49)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 15) ? 1u : 0u);
                        else if (yy == 50)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 25 && xx < 34 || xx >= 49 && xx < 52 || xx >= 55 && xx < 68) ? 1u : 0u);
                        else if (yy == 51)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 25 && xx < 34 || xx >= 39 && xx < 52 || xx >= 55 && xx < 68) ? 1u : 0u);
                        else if (yy == 52)
                            SetObstacle(map, xx, yy, (xx >= 1 && xx < 3 || xx >= 6 && xx < 16 || xx >= 25 && xx < 34 || xx >= 39 && xx < 52 || xx >= 55 && xx < 68) ? 1u : 0u);
                        else if (yy == 53)
                            SetObstacle(map, xx, yy, (xx >= 7 && xx < 15 || xx >= 19 && xx < 22 || xx >= 25 && xx < 70 || xx >= 73 && xx < 76) ? 1u : 0u);
                        else if (yy == 54)
                            SetObstacle(map, xx, yy, (xx >= 8 && xx < 14 || xx >= 19 && xx < 22 || xx >= 25 && xx < 70 || xx >= 73 && xx < 76) ? 1u : 0u);
                        else if (yy == 55)
                            SetObstacle(map, xx, yy, (xx >= 10 && xx < 12 || xx >= 19 && xx < 22 || xx >= 25 && xx < 70 || xx >= 73 && xx < 76) ? 1u : 0u);
                        else if (yy == 56)
                            SetObstacle(map, xx, yy, (xx >= 20 && xx < 81) ? 1u : 0u);
                        else if (yy == 57)
                            SetObstacle(map, xx, yy, (xx >= 20 && xx < 81) ? 1u : 0u);
                        else if (yy == 58)
                            SetObstacle(map, xx, yy, (xx >= 20 && xx < 81) ? 1u : 0u);
                        else if (yy == 59)
                            SetObstacle(map, xx, yy, 1);
                        else if (yy > 59)
                            SetObstacle(map, xx, yy, 1);

                    }
                }


                map.AddMarker("entrance_west", new Loc(4, 268), Dir8.Right);
                map.AddMarker("entrance_center", new Loc(388, 268), Dir8.Right);
                map.AddMarker("entrance_east", new Loc(772, 268), Dir8.Left);


                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(56, 216)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(600, 192)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(32, 448)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(160, 400)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(472, 376)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(552, 400)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(712, 392)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_2", 7, Dir8.Left), new Loc(744, 448)));

                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(144, 184)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(184, 176)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(120, 440)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(320, 384)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_1", 7, Dir8.Left), new Loc(656, 440)));

                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(788, 0, 4, 528), true, "East_Exit");
                    //BeginEntranceEvent entrance = new BeginEntranceEvent();
                    //entrance.AddDungeonEntrances(4, 11, 15, 17);
                    //entrance.AddGroundEntrances(new UnlockableGroundDest(5, 1, 5, 0), new UnlockableGroundDest(6, 1, 6, 0), new UnlockableGroundDest(7, 1, 7, 0));
                    //groundObject.Events.Add(entrance);
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(0, 0, 4, 528), true, "West_Exit");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Assembly", 1, 0, 0), new Rect(696, 216, 24, 24), new Loc(0, 8), false, "Assembly");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Storage", 1), new Rect(728, 216, 24, 24), new Loc(4, 8), false, "Storage");
                    map.AddObject(groundObject);
                }


                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Trunk_Small", 1), new Rect(632, 184, 24, 24), new Loc(4, 0), false, "Small_Stump_0");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Trunk_Small", 1), new Rect(112, 200, 24, 24), new Loc(4, 0), false, "Small_Stump_1");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Trunk_Small", 1), new Rect(672, 200, 24, 24), new Loc(4, 0), false, "Small_Stump_2");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Trunk_Small", 1), new Rect(88, 208, 24, 24), new Loc(4, 0), false, "Small_Stump_3");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Trunk_Small", 1), new Rect(744, 384, 24, 24), new Loc(4, 0), false, "Small_Stump_4");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Trunk_Small", 1), new Rect(640, 400, 24, 24), new Loc(4, 0), false, "Small_Stump_5");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Trunk_Large", 1), new Rect(760, 208, 32, 32), new Loc(4, 0), false, "Stump_0");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Trunk_Large", 1), new Rect(520, 344, 32, 32), new Loc(4, 0), false, "Stump_1");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Trunk_Large", 1), new Rect(288, 376, 32, 32), new Loc(4, 0), false, "Stump_2");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Trunk_Large", 1), new Rect(704, 424, 32, 32), new Loc(4, 0), false, "Stump_3");
                    map.AddObject(groundObject);
                }


                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Tent", 1), Dir8.Right, new Rect(168, 352, 40, 32), new Loc(16, 40), false, "Tent_0");
                    map.AddObject(groundObject);
                }

                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Tent", 1), Dir8.Left, new Rect(448, 184, 40, 32), new Loc(16, 40), false, "Tent_1");
                    map.AddObject(groundObject);
                }



                map.AddSpawner(CreateTeamSpawner(1, new Loc(744, 296), Dir8.Up));
                map.AddSpawner(CreateTeamSpawner(2, new Loc(712, 296), Dir8.Up));
                map.AddSpawner(CreateTeamSpawner(3, new Loc(680, 296), Dir8.Up));

                map.Music = "A03. Cliff Camp.ogg";
            }
            else if (name == MapNames[5])
            {
                int width = 144;
                int height = 78;
                int texSize = 1;
                map.CreateNew(width, height, texSize);
                map.Name = new LocalText("**Ravine Camp");
                map.EdgeView = Map.ScrollEdge.Clamp;

                List<Loc> sparkleLocs = new List<Loc>();
                sparkleLocs.Add(new Loc(3, 40));
                sparkleLocs.Add(new Loc(12, 40));
                sparkleLocs.Add(new Loc(21, 40));
                sparkleLocs.Add(new Loc(30, 40));
                sparkleLocs.Add(new Loc(39, 40));
                sparkleLocs.Add(new Loc(58, 36));
                sparkleLocs.Add(new Loc(67, 36));
                sparkleLocs.Add(new Loc(79, 37));
                sparkleLocs.Add(new Loc(83, 43));
                sparkleLocs.Add(new Loc(83, 55));
                sparkleLocs.Add(new Loc(88, 60));
                sparkleLocs.Add(new Loc(97, 60));
                sparkleLocs.Add(new Loc(106, 60));
                sparkleLocs.Add(new Loc(124, 60));
                sparkleLocs.Add(new Loc(124, 60));
                sparkleLocs.Add(new Loc(133, 60));

                map.AddLayer("Sparkles");

                for (int xx = 0; xx < width; xx++)
                {
                    for (int yy = 0; yy < height; yy++)
                    {
                        map.Layers[0].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "CanyonCamp"));

                        foreach(Loc loc in sparkleLocs)
                        {
                            if (xx >= loc.X && xx < loc.X + 2 && yy >= loc.Y && yy < loc.Y + 2)
                            {
                                Loc diff = new Loc(xx, yy) - loc;
                                TileLayer waterSparkle = new TileLayer(5);
                                waterSparkle.Frames.Add(new TileFrame(new Loc(0, 79) + diff, "CanyonCamp"));
                                waterSparkle.Frames.Add(new TileFrame(new Loc(3, 79) + diff, "CanyonCamp"));
                                waterSparkle.Frames.Add(new TileFrame(new Loc(6, 79) + diff, "CanyonCamp"));
                                waterSparkle.Frames.Add(new TileFrame(new Loc(9, 79) + diff, "CanyonCamp"));
                                map.Layers[1].Tiles[xx][yy] = new AutoTile(waterSparkle);
                                break;
                            }
                        }
                    }
                }

                for (int xx = 0; xx < width * texSize; xx++)
                {
                    for (int yy = 0; yy < height * texSize; yy++)
                    {
                        if (yy <= 17)
                            map.SetObstacle(xx, yy, 1);
                        else if (yy == 18)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 44 || xx >= 60 && xx < 88 || xx >= 91 && xx < 93 || xx >= 104 && xx < 144) ? 1u : 0u);
                        else if (yy == 19)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 44 || xx >= 57 && xx < 88 || xx >= 91 && xx < 93 || xx >= 104 && xx < 144) ? 1u : 0u);
                        else if (yy == 20)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 44 || xx >= 57 && xx < 88 || xx >= 91 && xx < 93 || xx >= 104 && xx < 144) ? 1u : 0u);
                        else if (yy == 21)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 43 || xx >= 50 && xx < 54 || xx >= 60 && xx < 87 || xx >= 91 && xx < 93 || xx >= 104 && xx < 144) ? 1u : 0u);
                        else if (yy == 22)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 36 || xx >= 39 && xx < 41 || xx >= 50 && xx < 54 || xx >= 60 && xx < 64 || xx >= 104 && xx < 108 || xx >= 112 && xx < 144) ? 1u : 0u);
                        else if (yy == 23)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 36 || xx >= 39 && xx < 41 || xx >= 50 && xx < 54 || xx >= 65 && xx < 69 || xx >= 112 && xx < 144) ? 1u : 0u);
                        else if (yy == 24)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 36 || xx >= 39 && xx < 41 || xx >= 65 && xx < 69 || xx >= 80 && xx < 82 || xx >= 112 && xx < 144) ? 1u : 0u);
                        else if (yy == 25)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 36 || xx >= 65 && xx < 69 || xx >= 79 && xx < 90 || xx >= 107 && xx < 111 || xx >= 113 && xx < 144) ? 1u : 0u);
                        else if (yy == 26)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 28 || xx >= 32 && xx < 36 || xx >= 79 && xx < 81 || xx >= 82 && xx < 90 || xx >= 107 && xx < 111 || xx >= 120 && xx < 144) ? 1u : 0u);
                        else if (yy == 27)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 28 || xx >= 107 && xx < 111 || xx >= 120 && xx < 144) ? 1u : 0u);
                        else if (yy == 28)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 28 || xx >= 120 && xx < 144) ? 1u : 0u);
                        else if (yy == 29)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 27 || xx >= 28 && xx < 38 || xx >= 120 && xx < 144) ? 1u : 0u);
                        else if (yy == 30)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 28 && xx < 38 || xx >= 112 && xx < 115 || xx >= 120 && xx < 121 || xx >= 124 && xx < 144) ? 1u : 0u);
                        else if (yy == 31)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 29 && xx < 36 || xx >= 66 && xx < 72 || xx >= 112 && xx < 115 || xx >= 119 && xx < 121 || xx >= 124 && xx < 144) ? 1u : 0u);
                        else if (yy == 32)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 25 && xx < 28 || xx >= 30 && xx < 36 || xx >= 66 && xx < 72 || xx >= 119 && xx < 121 || xx >= 124 && xx < 144) ? 1u : 0u);
                        else if (yy == 33)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 25 && xx < 28 || xx >= 49 && xx < 81 || xx >= 124 && xx < 144) ? 1u : 0u);
                        else if (yy == 34)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 48 && xx < 82 || xx >= 88 && xx < 90 || xx >= 124 && xx < 128 || xx >= 132 && xx < 144) ? 1u : 0u);
                        else if (yy == 35)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 29 && xx < 35 || xx >= 47 && xx < 83 || xx >= 88 && xx < 90 || xx >= 132 && xx < 144) ? 1u : 0u);
                        else if (yy == 36)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 29 && xx < 35 || xx >= 46 && xx < 84 || xx >= 132 && xx < 144) ? 1u : 0u);
                        else if (yy == 37)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 85 || xx >= 132 && xx < 144) ? 1u : 0u);
                        else if (yy == 38)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 86 || xx >= 118 && xx < 122 || xx >= 136 && xx < 144) ? 1u : 0u);
                        else if (yy == 39)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 87 || xx >= 92 && xx < 102 || xx >= 118 && xx < 122 || xx >= 130 && xx < 132 || xx >= 136 && xx < 144) ? 1u : 0u);
                        else if (yy == 40)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 55 || xx >= 78 && xx < 88 || xx >= 92 && xx < 102 || xx >= 110 && xx < 112 || xx >= 118 && xx < 122 || xx >= 124 && xx < 126 || xx >= 130 && xx < 132 || xx >= 136 && xx < 144) ? 1u : 0u);
                        else if (yy == 41)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 54 || xx >= 55 && xx < 58 || xx >= 79 && xx < 88 || xx >= 91 && xx < 100 || xx >= 110 && xx < 112 || xx >= 124 && xx < 126 || xx >= 137 && xx < 144) ? 1u : 0u);
                        else if (yy == 42)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 53 || xx >= 55 && xx < 58 || xx >= 72 && xx < 75 || xx >= 80 && xx < 88 || xx >= 90 && xx < 100) ? 1u : 0u);
                        else if (yy == 43)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 53 || xx >= 62 && xx < 66 || xx >= 72 && xx < 75 || xx >= 80 && xx < 88 || xx >= 90 && xx < 94 || xx >= 114 && xx < 116) ? 1u : 0u);
                        else if (yy == 44)
                            map.SetObstacle(xx, yy, (xx >= 12 && xx < 15 || xx >= 51 && xx < 53 || xx >= 62 && xx < 66 || xx >= 80 && xx < 88 || xx >= 114 && xx < 116) ? 1u : 0u);
                        else if (yy == 45)
                            map.SetObstacle(xx, yy, (xx >= 12 && xx < 15 || xx >= 62 && xx < 66 || xx >= 80 && xx < 88) ? 1u : 0u);
                        else if (yy == 46)
                            map.SetObstacle(xx, yy, (xx >= 80 && xx < 88) ? 1u : 0u);
                        else if (yy == 52)
                            map.SetObstacle(xx, yy, (xx >= 117 && xx < 123 || xx >= 133 && xx < 139) ? 1u : 0u);
                        else if (yy == 53)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 6 || xx >= 7 && xx < 13 || xx >= 14 && xx < 20 || xx >= 80 && xx < 88 || xx >= 117 && xx < 123 || xx >= 124 && xx < 127 || xx >= 133 && xx < 141) ? 1u : 0u);
                        else if (yy == 54)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 6 || xx >= 7 && xx < 13 || xx >= 14 && xx < 20 || xx >= 80 && xx < 88 || xx >= 111 && xx < 117 || xx >= 124 && xx < 127 || xx >= 139 && xx < 141) ? 1u : 0u);
                        else if (yy == 55)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 28 || xx >= 80 && xx < 89 || xx >= 111 && xx < 117) ? 1u : 0u);
                        else if (yy == 56)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 28 || xx >= 74 && xx < 90 || xx >= 111 && xx < 115) ? 1u : 0u);
                        else if (yy == 57)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 28 || xx >= 33 && xx < 35 || xx >= 74 && xx < 144) ? 1u : 0u);
                        else if (yy == 58)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 29 || xx >= 33 && xx < 35 || xx >= 64 && xx < 144) ? 1u : 0u);
                        else if (yy == 59)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 32 || xx >= 64 && xx < 144) ? 1u : 0u);
                        else if (yy == 60)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 32 || xx >= 38 && xx < 42 || xx >= 66 && xx < 144) ? 1u : 0u);
                        else if (yy == 61)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 33 || xx >= 38 && xx < 42 || xx >= 66 && xx < 144) ? 1u : 0u);
                        else if (yy == 62)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 33 || xx >= 38 && xx < 42 || xx >= 65 && xx < 144) ? 1u : 0u);
                        else if (yy == 63)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 33 || xx >= 64 && xx < 144) ? 1u : 0u);
                        else if (yy == 64)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 38 || xx >= 42 && xx < 44 || xx >= 45 && xx < 51 || xx >= 54 && xx < 63 || xx >= 64 && xx < 144) ? 1u : 0u);
                        else if (yy == 65)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 38 || xx >= 42 && xx < 44 || xx >= 45 && xx < 63 || xx >= 64 && xx < 144) ? 1u : 0u);
                        else if (yy >= 66)
                            map.SetObstacle(xx, yy, 1);
                    }
                }


                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Left), new Loc(40, 440)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Left), new Loc(120, 360)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Left), new Loc(192, 272)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Right), new Loc(216, 424)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Left), new Loc(304, 512)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Right), new Loc(312, 216)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Left), new Loc(312, 280)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Right), new Loc(320, 352)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Left), new Loc(336, 40)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Left), new Loc(424, 152)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Right), new Loc(464, 240)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Left), new Loc(472, 320)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Right), new Loc(544, 72)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Right), new Loc(664, 176)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Left), new Loc(720, 432)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Left), new Loc(728, 512)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Left), new Loc(760, 440)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Right), new Loc(768, 200)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Left), new Loc(808, 144)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Right), new Loc(808, 288)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Left), new Loc(952, 328)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Left), new Loc(976, 128)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Left), new Loc(1008, 512)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Left), new Loc(1032, 432)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_3", 11, Dir8.Left), new Loc(1128, 344)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Left), new Loc(184, 416)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Right), new Loc(272, 352)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Left), new Loc(320, 64)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Right), new Loc(376, 192)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Left), new Loc(376, 256)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Left), new Loc(480, 528)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Left), new Loc(560, 176)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Left), new Loc(600, 240)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Left), new Loc(608, 344)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Right), new Loc(640, 496)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Left), new Loc(672, 40)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Left), new Loc(680, 512)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Right), new Loc(744, 352)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Left), new Loc(744, 424)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Left), new Loc(752, 176)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Right), new Loc(792, 248)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Left), new Loc(832, 192)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Left), new Loc(864, 248)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Right), new Loc(896, 72)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Left), new Loc(904, 512)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Left), new Loc(936, 280)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Right), new Loc(1016, 440)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_4", 11, Dir8.Left), new Loc(1064, 336)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(207, 360)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(242, 104)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Right), new Loc(250, 514)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(263, 480)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(367, 528)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(370, 358)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Right), new Loc(416, 238)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(456, 190)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(480, 200)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(488, 240)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(544, 320)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Right), new Loc(562, 496)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(576, 416)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(578, 192)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(624, 184)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(648, 70)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(650, 518)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Right), new Loc(664, 246)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(696, 296)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(744, 440)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(760, 238)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(792, 432)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(794, 272)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Right), new Loc(800, 166)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(840, 48)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(840, 518)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Right), new Loc(872, 440)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(880, 182)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(904, 208)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(920, 326)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Right), new Loc(970, 432)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(992, 306)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Right), new Loc(1008, 160)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(1040, 528)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Right), new Loc(1058, 360)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Town_5", 11, Dir8.Left), new Loc(1088, 440)));

                map.AddMarker("entrance_west", new Loc(4, 384), Dir8.Right);
                map.AddMarker("entrance_center", new Loc(836, 384), Dir8.Right);
                map.AddMarker("entrance_east", new Loc(1132, 384), Dir8.Left);

                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(1148, 0, 4, 624), true, "East_Exit");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(0, 0, 4, 624), true, "West_Exit");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Assembly", 1, 0, 0), new Rect(984, 352, 24, 24), new Loc(0, 8), false, "Assembly");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Storage", 1), new Rect(932, 352, 24, 24), new Loc(4, 8), false, "Storage");
                    map.AddObject(groundObject);
                }

                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Pot", 1), Dir8.Left, new Rect(512, 504, 16, 16), new Loc(0, 0), false, "Pot_0");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Pot", 1), Dir8.Left, new Rect(528, 504, 16, 16), new Loc(0, 0), false, "Pot_1");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Pot", 1), Dir8.Left, new Rect(632, 200, 16, 16), new Loc(0, 0), false, "Pot_2");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Pot", 1), Dir8.Left, new Rect(656, 200, 16, 16), new Loc(0, 0), false, "Pot_3");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Pot", 1), Dir8.Left, new Rect(720, 336, 16, 16), new Loc(0, 0), false, "Pot_4");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Pot", 1), Dir8.Left, new Rect(736, 336, 16, 16), new Loc(0, 0), false, "Pot_5");
                    map.AddObject(groundObject);
                }

                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Pot", 1), Dir8.Left, new Rect(520, 496, 16, 16), new Loc(0, 0), false, "Pot_6");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Pot", 1), Dir8.Left, new Rect(640, 192, 16, 16), new Loc(0, 0), false, "Pot_7");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Pot", 1), Dir8.Left, new Rect(728, 328, 16, 16), new Loc(0, 0), false, "Pot_8");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Pot", 1), Dir8.Left, new Rect(912, 344, 16, 16), new Loc(0, 0), false, "Pot_9");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Tree_Town", 1), Dir8.Left, new Rect(312, 184, 16, 16), new Loc(32, 56), false, "Tree_Town_8");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Tree_Town", 1), Dir8.Left, new Rect(264, 456, 16, 16), new Loc(32, 56), false, "Tree_Town_0");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Tree_Town", 1), Dir8.Left, new Rect(408, 344, 16, 16), new Loc(32, 56), false, "Tree_Town_1");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Tree_Town", 1), Dir8.Left, new Rect(704, 272, 16, 16), new Loc(32, 56), false, "Tree_Town_2");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Tree_Town", 1), Dir8.Left, new Rect(728, 160, 16, 16), new Loc(32, 56), false, "Tree_Town_3");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Tree_Town", 1), Dir8.Left, new Rect(880, 320, 16, 16), new Loc(32, 56), false, "Tree_Town_4");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Tree_Town", 1), Dir8.Left, new Rect(944, 504, 16, 16), new Loc(32, 56), false, "Tree_Town_5");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Tree_Town", 1), Dir8.Left, new Rect(952, 248, 16, 16), new Loc(32, 56), false, "Tree_Town_6");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Tree_Town", 1), Dir8.Left, new Rect(1040, 312, 16, 16), new Loc(32, 56), false, "Tree_Town_7");
                    map.AddObject(groundObject);
                }

                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Campfire", 1), Dir8.Left, new Rect(304, 480, 32, 24), new Loc(0, 8), false, "Campfire_0");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Campfire", 1), Dir8.Left, new Rect(400, 168, 32, 24), new Loc(0, 8), false, "Campfire_1");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Campfire", 1), Dir8.Left, new Rect(496, 344, 32, 24), new Loc(0, 8), false, "Campfire_2");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Campfire", 1), Dir8.Left, new Rect(856, 200, 32, 24), new Loc(0, 8), false, "Campfire_3");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Fence", 1), Dir8.Left, new Rect(232, 280, 48, 16), new Loc(0, 32), false, "Fence_9");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Fence", 1), Dir8.Left, new Rect(0, 424, 48, 16), new Loc(0, 32), false, "Fence_0");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Fence", 1), Dir8.Left, new Rect(56, 424, 48, 16), new Loc(0, 32), false, "Fence_1");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Fence", 1), Dir8.Left, new Rect(112, 424, 48, 16), new Loc(0, 32), false, "Fence_2");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Fence", 1), Dir8.Left, new Rect(360, 512, 48, 16), new Loc(0, 32), false, "Fence_3");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Fence", 1), Dir8.Left, new Rect(456, 512, 48, 16), new Loc(0, 32), false, "Fence_4");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Fence", 1), Dir8.Left, new Rect(528, 248, 48, 16), new Loc(0, 32), false, "Fence_5");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Fence", 1), Dir8.Left, new Rect(672, 200, 48, 16), new Loc(0, 32), false, "Fence_6");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Fence", 1), Dir8.Left, new Rect(936, 416, 48, 16), new Loc(0, 32), false, "Fence_7");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Fence", 1), Dir8.Left, new Rect(1064, 416, 48, 16), new Loc(0, 32), false, "Fence_8");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Logs_Small", 1), Dir8.Left, new Rect(96, 352, 24, 16), new Loc(0, 8), false, "Logs_Small_0");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Logs_Small", 1), Dir8.Right, new Rect(168, 440, 24, 16), new Loc(0, 8), false, "Logs_Small_1");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Logs_Small", 1), Dir8.Left, new Rect(200, 256, 24, 16), new Loc(0, 8), false, "Logs_Small_2");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Logs_Small", 1), Dir8.Right, new Rect(232, 472, 24, 16), new Loc(0, 8), false, "Logs_Small_3");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Logs_Small", 1), Dir8.Right, new Rect(432, 512, 24, 16), new Loc(0, 8), false, "Logs_Small_4");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Logs_Small", 1), Dir8.Left, new Rect(440, 328, 24, 16), new Loc(0, 8), false, "Logs_Small_5");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Logs_Small", 1), Dir8.Left, new Rect(456, 152, 24, 16), new Loc(0, 8), false, "Logs_Small_6");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Logs_Small", 1), Dir8.Left, new Rect(576, 336, 24, 16), new Loc(0, 8), false, "Logs_Small_7");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Logs_Small", 1), Dir8.Left, new Rect(896, 240, 24, 16), new Loc(0, 8), false, "Logs_Small_8");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Logs_Small", 1), Dir8.Left, new Rect(992, 424, 24, 16), new Loc(0, 8), false, "Logs_Small_9");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Logs_Stacked", 1), Dir8.Left, new Rect(592, 440, 48, 32), new Loc(0, 16), false, "Logs_Stacked_0");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Tent", 1), Dir8.Left, new Rect(240, 224, 48, 40), new Loc(16, 40), false, "Tent_2");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Tent", 1), Dir8.Right, new Rect(528, 456, 48, 40), new Loc(16, 40), false, "Tent_0");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Tent", 1), Dir8.Left, new Rect(752, 304, 48, 40), new Loc(16, 40), false, "Tent_1");
                    map.AddObject(groundObject);
                }

                map.AddSpawner(CreateTeamSpawner(1, new Loc(1040, 356), Dir8.Down));
                map.AddSpawner(CreateTeamSpawner(2, new Loc(1076, 356), Dir8.Down));
                map.AddSpawner(CreateTeamSpawner(3, new Loc(1112, 356), Dir8.Down));

                map.Music = "A04. Canyon Camp.ogg";
            }
            else if (name == MapNames[6])
            {
                int width = 19;
                int height = 19;
                int texSize = 3;
                map.CreateNew(width, height, texSize);
                map.Name = new LocalText("**Cave Shelter");
                map.EdgeView = Map.ScrollEdge.Clamp;

                for (int xx = 0; xx < width; xx++)
                {
                    for (int yy = 0; yy < height; yy++)
                        map.Layers[0].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "CaveStop"));
                }
                for (int xx = 0; xx < width * texSize; xx++)
                {
                    for (int yy = 0; yy < height * texSize; yy++)
                    {
                        if (yy < 15)
                            SetObstacle(map, xx, yy, 1);
                        else if (yy < 16)
                            SetObstacle(map, xx, yy, (xx < 23 || width * texSize - xx <= 23) ? 1u : 0u);
                        else if (yy < 17)
                            SetObstacle(map, xx, yy, (xx < 19 || width * texSize - xx <= 19) ? 1u : 0u);
                        else if (yy < 18)
                            SetObstacle(map, xx, yy, (xx < 17 || width * texSize - xx <= 17) ? 1u : 0u);
                        else if (yy < 19)
                            SetObstacle(map, xx, yy, (xx < 15 || width * texSize - xx <= 15) ? 1u : 0u);
                        else if (yy < 20)
                            SetObstacle(map, xx, yy, (xx < 13 || width * texSize - xx <= 13) ? 1u : 0u);
                        else if (yy < 21)
                            SetObstacle(map, xx, yy, (xx < 12 || width * texSize - xx <= 12) ? 1u : 0u);
                        else if (yy < 22)
                            SetObstacle(map, xx, yy, (xx < 11 || width * texSize - xx <= 11) ? 1u : 0u);
                        else if (yy < 24)
                            SetObstacle(map, xx, yy, (xx < 10 || width * texSize - xx <= 10) ? 1u : 0u);
                        else if (yy < 25)
                            SetObstacle(map, xx, yy, (xx < 9 || width * texSize - xx <= 9) ? 1u : 0u);
                        else if (yy < 28)
                            SetObstacle(map, xx, yy, (xx < 8 || width * texSize - xx <= 8) ? 1u : 0u);
                        else if (yy < 43)
                            SetObstacle(map, xx, yy, (xx < 6 || width * texSize - xx <= 6) ? 1u : 0u);
                        else if (yy < 48)
                            SetObstacle(map, xx, yy, (xx < 7 || width * texSize - xx <= 7) ? 1u : 0u);
                        else if (yy < 55)
                            SetObstacle(map, xx, yy, (xx < 6 || width * texSize - xx <= 6) ? 1u : 0u);
                        else if (yy < 56)
                            SetObstacle(map, xx, yy, (xx < 7 || width * texSize - xx <= 7) ? 1u : 0u);
                        else if (yy < 57)
                            SetObstacle(map, xx, yy, (xx < 8 || width * texSize - xx <= 8) ? 1u : 0u);
                    }
                }

                map.AddMarker("entrance_south", new Loc(220, 436), Dir8.Up);
                map.AddMarker("entrance_center", new Loc(220, 308), Dir8.Up);
                map.AddMarker("entrance_north", new Loc(220, 124), Dir8.Down);


                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Assembly", 1, 0, 0), new Rect(288, 144, 24, 24), new Loc(0, 8), false, "Assembly");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Storage", 1), new Rect(312, 168, 24, 24), new Loc(4, 8), false, "Storage");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(0, 120, 456, 4), true, "North_Exit");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(0, 452, 456, 4), true, "South_Exit");
                    map.AddObject(groundObject);
                }

                map.AddSpawner(CreateTeamSpawner(1, new Loc(152, 152), Dir8.DownRight));
                map.AddSpawner(CreateTeamSpawner(2, new Loc(120, 168), Dir8.DownRight));
                map.AddSpawner(CreateTeamSpawner(3, new Loc(96, 192), Dir8.DownRight));

                map.Music = "A05. Cave Camp.ogg";
            }
            else if (name == MapNames[7])
            {
                int width = 25;
                int height = 27;
                int texSize = 3;
                map.CreateNew(width, height, texSize);
                map.Name = new LocalText("**Path to the Summit");
                map.EdgeView = Map.ScrollEdge.Clamp;

                map.AddLayer("Cliff");
                map.Layers[1].Layer = DrawLayer.Top;

                for (int xx = 0; xx < width; xx++)
                {
                    for (int yy = 0; yy < height; yy++)
                    {
                        map.Layers[0].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "SnowCamp"));
                        if (yy >= 20 && (xx < 8 || xx >= 17))
                            map.Layers[1].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "SnowCampCliffs"));
                        else if (yy >= 17 && (xx < 7 || xx >= 18))
                            map.Layers[1].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "SnowCampCliffs"));
                        else if (yy >= 14 && (xx < 6 || xx >= 19))
                            map.Layers[1].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "SnowCampCliffs"));
                        else if (yy >= 10 && (xx < 5 || xx >= 20))
                            map.Layers[1].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "SnowCampCliffs"));
                        else if (yy >= 8 && (xx < 3 || xx >= 22))
                            map.Layers[1].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "SnowCampCliffs"));
                    }
                }

                for (int xx = 0; xx < width * texSize; xx++)
                {
                    for (int yy = 0; yy < height * texSize; yy++)
                    {
                        if (yy == 0)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 1)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 2)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 3)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 4)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 5)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 6)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 7)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 8)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 9)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 10)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 11)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 12)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 13)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 22 || xx >= 53 && xx < 75) ? 1u : 0u);
                        else if (yy == 14)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 22 || xx >= 53 && xx < 75) ? 1u : 0u);
                        else if (yy == 15)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 19 || xx >= 56 && xx < 75) ? 1u : 0u);
                        else if (yy == 16)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 16 || xx >= 57 && xx < 75) ? 1u : 0u);
                        else if (yy == 17)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 12 || xx >= 57 && xx < 59 || xx >= 63 && xx < 75) ? 1u : 0u);
                        else if (yy == 18)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 10 || xx >= 15 && xx < 17 || xx >= 65 && xx < 75) ? 1u : 0u);
                        else if (yy == 19)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 6 || xx >= 15 && xx < 17 || xx >= 69 && xx < 75) ? 1u : 0u);
                        else if (yy == 20)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 6 || xx >= 69 && xx < 75) ? 1u : 0u);
                        else if (yy == 21)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 6 || xx >= 10 && xx < 14 || xx >= 69 && xx < 75) ? 1u : 0u);
                        else if (yy == 22)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 2 || xx >= 10 && xx < 14 || xx >= 73 && xx < 75) ? 1u : 0u);
                        else if (yy == 23)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 2 || xx >= 10 && xx < 14 || xx >= 73 && xx < 75) ? 1u : 0u);
                        else if (yy == 24)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 2 || xx >= 73 && xx < 75) ? 1u : 0u);
                        else if (yy == 25)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 2 || xx >= 73 && xx < 75) ? 1u : 0u);
                        else if (yy == 26)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 2 || xx >= 73 && xx < 75) ? 1u : 0u);
                        else if (yy == 27)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 2 || xx >= 73 && xx < 75) ? 1u : 0u);
                        else if (yy == 28)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 7 || xx >= 68 && xx < 75) ? 1u : 0u);
                        else if (yy == 29)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 7 || xx >= 68 && xx < 75) ? 1u : 0u);
                        else if (yy == 30)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 7 || xx >= 21 && xx < 23 || xx >= 68 && xx < 75) ? 1u : 0u);
                        else if (yy == 31)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 7 || xx >= 21 && xx < 23 || xx >= 68 && xx < 75) ? 1u : 0u);
                        else if (yy == 32)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 11 || xx >= 64 && xx < 75) ? 1u : 0u);
                        else if (yy == 33)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 12 || xx >= 54 && xx < 58 || xx >= 63 && xx < 75) ? 1u : 0u);
                        else if (yy == 34)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 12 || xx >= 54 && xx < 60 || xx >= 63 && xx < 75) ? 1u : 0u);
                        else if (yy == 35)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 12 || xx >= 54 && xx < 60 || xx >= 63 && xx < 75) ? 1u : 0u);
                        else if (yy == 36)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 12 || xx >= 15 && xx < 17 || xx >= 63 && xx < 75) ? 1u : 0u);
                        else if (yy == 37)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 12 || xx >= 15 && xx < 17 || xx >= 63 && xx < 75) ? 1u : 0u);
                        else if (yy == 38)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 12 || xx >= 63 && xx < 75) ? 1u : 0u);
                        else if (yy == 39)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 12 || xx >= 63 && xx < 75) ? 1u : 0u);
                        else if (yy == 40)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 12 || xx >= 63 && xx < 75) ? 1u : 0u);
                        else if (yy == 41)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 12 || xx >= 63 && xx < 75) ? 1u : 0u);
                        else if (yy == 42)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 12 || xx >= 51 && xx < 53 || xx >= 63 && xx < 75) ? 1u : 0u);
                        else if (yy == 43)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 13 || xx >= 51 && xx < 53 || xx >= 62 && xx < 75) ? 1u : 0u);
                        else if (yy == 44)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 14 || xx >= 61 && xx < 75) ? 1u : 0u);
                        else if (yy == 45)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 14 || xx >= 61 && xx < 75) ? 1u : 0u);
                        else if (yy == 46)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 14 || xx >= 61 && xx < 75) ? 1u : 0u);
                        else if (yy == 47)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 14 || xx >= 61 && xx < 75) ? 1u : 0u);
                        else if (yy == 48)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 14 || xx >= 61 && xx < 75) ? 1u : 0u);
                        else if (yy == 49)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 14 || xx >= 61 && xx < 75) ? 1u : 0u);
                        else if (yy == 50)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 14 || xx >= 61 && xx < 75) ? 1u : 0u);
                        else if (yy == 51)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 14 || xx >= 61 && xx < 75) ? 1u : 0u);
                        else if (yy == 52)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 14 || xx >= 61 && xx < 75) ? 1u : 0u);
                        else if (yy == 53)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 16 || xx >= 59 && xx < 75) ? 1u : 0u);
                        else if (yy == 54)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 17 || xx >= 58 && xx < 75) ? 1u : 0u);
                        else if (yy == 55)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 17 || xx >= 58 && xx < 75) ? 1u : 0u);
                        else if (yy == 56)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 17 || xx >= 58 && xx < 75) ? 1u : 0u);
                        else if (yy == 57)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 17 || xx >= 58 && xx < 75) ? 1u : 0u);
                        else if (yy == 58)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 17 || xx >= 58 && xx < 75) ? 1u : 0u);
                        else if (yy == 59)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 17 || xx >= 58 && xx < 75) ? 1u : 0u);
                        else if (yy == 60)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 17 || xx >= 58 && xx < 75) ? 1u : 0u);
                        else if (yy == 61)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 17 || xx >= 58 && xx < 75) ? 1u : 0u);
                        else if (yy == 62)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 19 || xx >= 56 && xx < 75) ? 1u : 0u);
                        else if (yy == 63)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 20 || xx >= 55 && xx < 75) ? 1u : 0u);
                        else if (yy == 64)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 20 || xx >= 55 && xx < 75) ? 1u : 0u);
                        else if (yy == 65)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 20 || xx >= 55 && xx < 75) ? 1u : 0u);
                        else if (yy == 66)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 20 || xx >= 55 && xx < 75) ? 1u : 0u);
                        else if (yy == 67)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 20 || xx >= 55 && xx < 75) ? 1u : 0u);
                        else if (yy == 68)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 20 || xx >= 55 && xx < 75) ? 1u : 0u);
                        else if (yy == 69)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 20 || xx >= 55 && xx < 75) ? 1u : 0u);
                        else if (yy == 70)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 20 || xx >= 55 && xx < 75) ? 1u : 0u);
                        else if (yy == 71)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 20 || xx >= 55 && xx < 75) ? 1u : 0u);
                        else if (yy == 72)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 20 || xx >= 55 && xx < 75) ? 1u : 0u);
                        else if (yy == 73)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 20 || xx >= 55 && xx < 75) ? 1u : 0u);
                        else if (yy == 74)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 20 || xx >= 55 && xx < 75) ? 1u : 0u);
                        else if (yy == 75)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 20 || xx >= 55 && xx < 75) ? 1u : 0u);
                        else if (yy == 76)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 20 || xx >= 55 && xx < 75) ? 1u : 0u);
                        else if (yy == 77)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 20 || xx >= 55 && xx < 75) ? 1u : 0u);
                        else if (yy == 78)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 20 || xx >= 55 && xx < 75) ? 1u : 0u);
                        else if (yy == 79)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 20 || xx >= 55 && xx < 75) ? 1u : 0u);
                        else if (yy == 80)
                            map.SetObstacle(xx, yy, (xx >= 0 && xx < 20 || xx >= 55 && xx < 75) ? 1u : 0u);
                    }
                }

                map.AddMarker("entrance_south", new Loc(292, 628), Dir8.Up);
                map.AddMarker("entrance_center", new Loc(292, 212), Dir8.Up);
                map.AddMarker("entrance_north", new Loc(292, 4), Dir8.Down);

                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Assembly", 1, 0, 0), new Rect(424, 120, 24, 24), new Loc(0, 8), false, "Assembly");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Storage", 1), new Rect(480, 136, 24, 24), new Loc(4, 8), false, "Storage");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(0, 0, 600, 4), true, "North_Exit");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(0, 644, 600, 4), true, "South_Exit");
                    map.AddObject(groundObject);
                }

                map.AddSpawner(CreateTeamSpawner(1, new Loc(176, 112), Dir8.DownRight));
                map.AddSpawner(CreateTeamSpawner(2, new Loc(136, 144), Dir8.DownRight));
                map.AddSpawner(CreateTeamSpawner(3, new Loc(104, 192), Dir8.DownRight));

                map.Music = "A06. Snow Camp.ogg";
            }
            else if (name == MapNames[8])
            {
                int width = 17;
                int height = 14;
                int texSize = 3;
                map.CreateNew(width, height, texSize);
                map.Name = new LocalText("**Guildmaster Summit");
                map.EdgeView = Map.ScrollEdge.Clamp;

                for (int xx = 0; xx < width; xx++)
                {
                    for (int yy = 0; yy < height; yy++)
                        map.Layers[0].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "Summit"));
                }
                for (int xx = 0; xx < width * texSize; xx++)
                {
                    for (int yy = 0; yy < height * texSize; yy++)
                    {
                        if (yy <= 15)
                            SetObstacle(map, xx, yy, 1);
                        else if (yy == 16)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 18 || xx >= 33 && xx < 51) ? 1u : 0u);
                        else if (yy == 17)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 17 || xx >= 34 && xx < 51) ? 1u : 0u);
                        else if (yy == 18)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 35 && xx < 51) ? 1u : 0u);
                        else if (yy == 19)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 15 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 20)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 13 || xx >= 38 && xx < 51) ? 1u : 0u);
                        else if (yy == 21)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 12 || xx >= 39 && xx < 51) ? 1u : 0u);
                        else if (yy == 22)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 12 || xx >= 39 && xx < 51) ? 1u : 0u);
                        else if (yy == 23)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 12 || xx >= 39 && xx < 51) ? 1u : 0u);
                        else if (yy == 24)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 12 || xx >= 39 && xx < 51) ? 1u : 0u);
                        else if (yy == 25)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 12 || xx >= 39 && xx < 51) ? 1u : 0u);
                        else if (yy == 26)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 12 || xx >= 39 && xx < 51) ? 1u : 0u);
                        else if (yy == 27)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 12 || xx >= 39 && xx < 51) ? 1u : 0u);
                        else if (yy == 28)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 12 || xx >= 39 && xx < 51) ? 1u : 0u);
                        else if (yy == 29)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 12 || xx >= 39 && xx < 51) ? 1u : 0u);
                        else if (yy == 30)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 13 || xx >= 38 && xx < 51) ? 1u : 0u);
                        else if (yy == 31)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 14 || xx >= 37 && xx < 51) ? 1u : 0u);
                        else if (yy == 32)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 15 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 33)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 35 && xx < 51) ? 1u : 0u);
                        else if (yy >= 34)
                            SetObstacle(map, xx, yy, 1);
                    }
                }

                map.AddMarker("entrance_south", new Loc(196, 252), Dir8.Up);

                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(0, 268, 408, 4), true, "South_Exit");
                    map.AddObject(groundObject);
                }

                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(192, 112, 24, 16), false, "Summit");
                    map.AddObject(groundObject);
                }


                {
                    GroundChar groundChar = new GroundChar(new MonsterID("xatu", 0, "normal", Gender.Male), new Loc(196, 120), Dir8.Down, "Xatu");
                    groundChar.CharDir = Dir8.Up;
                    map.AddMapChar(groundChar);
                }
                {
                    GroundChar groundChar = new GroundChar(new MonsterID("lucario", 0, "normal", Gender.Male), new Loc(172, 120), Dir8.Up, "Lucario");
                    groundChar.CharDir = Dir8.Up;
                    map.AddMapChar(groundChar);
                }
                {
                    GroundChar groundChar = new GroundChar(new MonsterID("wigglytuff", 0, "normal", Gender.Male), new Loc(220, 120), Dir8.Up, "Wigglytuff");
                    groundChar.CharDir = Dir8.Up;
                    map.AddMapChar(groundChar);
                }

                map.AddSpawner(CreateTeamSpawner(1, new Loc(160, 128), Dir8.Up));
                map.AddSpawner(CreateTeamSpawner(2, new Loc(232, 128), Dir8.Up));
                map.AddSpawner(CreateTeamSpawner(3, new Loc(196, 160), Dir8.Up));

                map.Music = "A07. Summit.ogg";
            }
            else if (name == MapNames[9])
            {
                int width = 17;
                int height = 31;
                int texSize = 3;
                map.CreateNew(width, height, texSize);
                map.Name = new LocalText("**Guildmaster Path");
                map.EdgeView = Map.ScrollEdge.Clamp;

                for (int xx = 0; xx < width; xx++)
                {
                    for (int yy = 0; yy < height; yy++)
                        map.Layers[0].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "GuildPath"));
                }

                for (int xx = 0; xx < width * texSize; xx++)
                {
                    for (int yy = 0; yy < height * texSize; yy++)
                    {
                        if (yy <= 11)
                            SetObstacle(map, xx, yy, 1);
                        else if (yy == 12)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 28 || xx >= 34 && xx < 51) ? 1u : 0u);
                        else if (yy == 13)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 14)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 23 || xx >= 39 && xx < 51) ? 1u : 0u);
                        else if (yy == 15)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 22 || xx >= 40 && xx < 51) ? 1u : 0u);
                        else if (yy == 16)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 21 || xx >= 41 && xx < 51) ? 1u : 0u);
                        else if (yy == 17)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20 || xx >= 42 && xx < 51) ? 1u : 0u);
                        else if (yy == 18)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20 || xx >= 29 && xx < 33 || xx >= 42 && xx < 51) ? 1u : 0u);
                        else if (yy == 19)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20 || xx >= 27 && xx < 35 || xx >= 42 && xx < 51) ? 1u : 0u);
                        else if (yy == 20)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20 || xx >= 26 && xx < 36 || xx >= 42 && xx < 51) ? 1u : 0u);
                        else if (yy == 21)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20 || xx >= 25 && xx < 37 || xx >= 42 && xx < 51) ? 1u : 0u);
                        else if (yy == 22)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20 || xx >= 25 && xx < 37 || xx >= 42 && xx < 51) ? 1u : 0u);
                        else if (yy == 23)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20 || xx >= 25 && xx < 29 || xx >= 33 && xx < 37 || xx >= 42 && xx < 51) ? 1u : 0u);
                        else if (yy == 24)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20 || xx >= 26 && xx < 29 || xx >= 33 && xx < 36 || xx >= 42 && xx < 51) ? 1u : 0u);
                        else if (yy == 25)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20 || xx >= 27 && xx < 29 || xx >= 33 && xx < 35 || xx >= 42 && xx < 51) ? 1u : 0u);
                        else if (yy == 26)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20 || xx >= 42 && xx < 51) ? 1u : 0u);
                        else if (yy == 27)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20 || xx >= 42 && xx < 51) ? 1u : 0u);
                        else if (yy == 28)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 22 || xx >= 40 && xx < 51) ? 1u : 0u);
                        else if (yy == 29)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 24 || xx >= 38 && xx < 51) ? 1u : 0u);
                        else if (yy == 30)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 25 || xx >= 37 && xx < 51) ? 1u : 0u);
                        else if (yy == 31)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 32)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 33)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 34)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 35)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 36)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 37)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 38)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 39)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 40)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 41)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 42)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 43)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 44)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 45)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 46)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 47)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 48)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 49)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 50)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 51)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 52)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 53)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 54)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 55)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 56)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else if (yy == 57)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 25 || xx >= 37 && xx < 51) ? 1u : 0u);
                        else if (yy == 58)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 24 || xx >= 38 && xx < 51) ? 1u : 0u);
                        else if (yy == 59)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 22 || xx >= 40 && xx < 51) ? 1u : 0u);
                        else if (yy == 60)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 21 || xx >= 41 && xx < 51) ? 1u : 0u);
                        else if (yy == 61)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20 || xx >= 42 && xx < 51) ? 1u : 0u);
                        else if (yy == 62)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20 || xx >= 43 && xx < 51) ? 1u : 0u);
                        else if (yy == 63)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20) ? 1u : 0u);
                        else if (yy == 64)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20) ? 1u : 0u);
                        else if (yy == 65)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20) ? 1u : 0u);
                        else if (yy == 66)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20) ? 1u : 0u);
                        else if (yy == 67)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20 || xx >= 42 && xx < 51) ? 1u : 0u);
                        else if (yy == 68)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 21 || xx >= 41 && xx < 51) ? 1u : 0u);
                        else if (yy == 69)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 22 || xx >= 40 && xx < 51) ? 1u : 0u);
                        else if (yy == 70)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 23 || xx >= 39 && xx < 51) ? 1u : 0u);
                        else if (yy == 71)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 26 || xx >= 36 && xx < 51) ? 1u : 0u);
                        else
                            SetObstacle(map, xx, yy, 1);
                    }
                }

                map.AddMarker("entrance_east", new Loc(388, 506), Dir8.Left);
                map.AddMarker("entrance_hut", new Loc(240, 200), Dir8.Down);


                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_5", 13, Dir8.Left), new Loc(168, 200)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_5", 13, Dir8.Right), new Loc(296, 200)));

                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_4", 13, Dir8.Left), new Loc(168, 464)));

                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_2", 13, Dir8.Left), new Loc(72, 64)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_2", 13, Dir8.Left), new Loc(120, 200)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_2", 13, Dir8.Right), new Loc(312, 352)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_2", 13, Dir8.Left), new Loc(144, 376)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_2", 13, Dir8.Left), new Loc(296, 592)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_2", 13, Dir8.Left), new Loc(104, 672)));

                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_1", 13, Dir8.Right), new Loc(368, 448)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_1", 13, Dir8.Left), new Loc(360, 224)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_1", 13, Dir8.Right), new Loc(40, 232)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_1", 13, Dir8.Left), new Loc(40, 480)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_1", 13, Dir8.Left), new Loc(368, 560)));

                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_3", 13, Dir8.Left), new Loc(312, 80)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_3", 13, Dir8.Left), new Loc(24, 344)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_3", 13, Dir8.Left), new Loc(312, 408)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_3", 13, Dir8.Left), new Loc(184, 600)));
                map.Decorations[0].Anims.Add(new GroundAnim(new ObjAnimData("Flowers_Tropical_3", 13, Dir8.Right), new Loc(352, 696)));


                //Reload the map script
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("House_Normal", 0), new Rect(232, 184, 32, 16), new Loc(40, 88), true, "Hut_Entrance");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(404, 0, 4, 744), true, "East_Exit");
                    map.AddObject(groundObject);
                }

                map.Music = "A01. Title.ogg";
            }
            else if (name == MapNames[10])
            {
                int width = 16;
                int height = 13;
                int texSize = 3;
                map.CreateNew(width, height, texSize);
                map.Name = new LocalText("**Guildmaster Hut");
                map.EdgeView = Map.ScrollEdge.Clamp;

                for (int xx = 0; xx < width; xx++)
                {
                    for (int yy = 0; yy < height; yy++)
                        map.Layers[0].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "InsideHut"));
                }


                for (int xx = 0; xx < width * texSize; xx++)
                {
                    for (int yy = 0; yy < height * texSize; yy++)
                    {
                        if (yy <= 14)
                            SetObstacle(map, xx, yy, 1);
                        else if (yy == 15)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20 || xx >= 28 && xx < 48) ? 1u : 0u);
                        else if (yy == 16)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 15 || xx >= 33 && xx < 48) ? 1u : 0u);
                        else if (yy == 17)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 13 || xx >= 35 && xx < 48) ? 1u : 0u);
                        else if (yy == 18)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 12 || xx >= 36 && xx < 48) ? 1u : 0u);
                        else if (yy == 19)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 12 || xx >= 36 && xx < 48) ? 1u : 0u);
                        else if (yy == 20)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 11 || xx >= 37 && xx < 48) ? 1u : 0u);
                        else if (yy == 21)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 11 || xx >= 37 && xx < 48) ? 1u : 0u);
                        else if (yy == 22)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 10 || xx >= 38 && xx < 48) ? 1u : 0u);
                        else if (yy == 23)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 10 || xx >= 38 && xx < 48) ? 1u : 0u);
                        else if (yy == 24)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 10 || xx >= 38 && xx < 48) ? 1u : 0u);
                        else if (yy == 25)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 10 || xx >= 38 && xx < 48) ? 1u : 0u);
                        else if (yy == 26)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 10 || xx >= 38 && xx < 48) ? 1u : 0u);
                        else if (yy == 27)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 10 || xx >= 38 && xx < 48) ? 1u : 0u);
                        else if (yy == 28)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 10 || xx >= 38 && xx < 48) ? 1u : 0u);
                        else if (yy == 29)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 11 || xx >= 37 && xx < 48) ? 1u : 0u);
                        else if (yy == 30)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 11 || xx >= 37 && xx < 48) ? 1u : 0u);
                        else if (yy == 31)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 12 || xx >= 36 && xx < 48) ? 1u : 0u);
                        else if (yy == 32)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 12 || xx >= 36 && xx < 48) ? 1u : 0u);
                        else if (yy == 33)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 13 || xx >= 35 && xx < 48) ? 1u : 0u);
                        else if (yy == 34)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 15 || xx >= 33 && xx < 48) ? 1u : 0u);
                        else if (yy == 35)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 17 || xx >= 31 && xx < 48) ? 1u : 0u);
                        else if (yy == 36)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 21 || xx >= 27 && xx < 48) ? 1u : 0u);
                        else if (yy == 37)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 21 || xx >= 27 && xx < 48) ? 1u : 0u);
                        else if (yy == 38)
                            SetObstacle(map, xx, yy, 1);
                    }
                }

                map.AddMarker("entrance_south", new Loc(184, 284), Dir8.Up);
                map.AddMarker("entrance_portal", new Loc(184, 152), Dir8.Up);

                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(0, 300, 384, 4), true, "South_Exit");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Portal", 4), new Rect(176, 120, 32, 24), new Loc(44, 78), true, "Card_Portal");
                    map.AddObject(groundObject);
                }

                map.Music = "A09. Guildmaster.ogg";
            }
            else if (name == MapNames[11])
            {
                int width = 25;
                int height = 25;
                int texSize = 3;
                map.CreateNew(width, height, texSize);
                map.Name = new LocalText("Luminous Spring");
                map.EdgeView = Map.ScrollEdge.Clamp;

                map.AddLayer("Streams");

                for (int xx = 0; xx < width; xx++)
                {
                    for (int yy = 0; yy < height; yy++)
                        map.Layers[0].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "LuminousSpring"));
                }

                for (int xx = 0; xx < 3; xx++)
                {
                    for (int yy = 0; yy < 6; yy++)
                    {
                        TileLayer anim = new TileLayer(10);
                        for (int ii = 0; ii < 3; ii++)
                            anim.Frames.Add(new TileFrame(new Loc(ii * 3 + xx, yy), "LuminousSpringAnim"));
                        map.Layers[1].Tiles[11 + xx][yy] = new AutoTile(anim);
                    }
                }

                for (int xx = 0; xx < 5; xx++)
                {
                    for (int yy = 0; yy < 2; yy++)
                    {
                        if (yy == 0 && (xx == 0 || xx == 4))
                            continue;
                        TileLayer anim = new TileLayer(10);
                        for (int ii = 0; ii < 13; ii++)
                            anim.Frames.Add(new TileFrame(new Loc(xx, 20 + 2 * ii + yy), "LuminousSpringAnim"));
                        map.Layers[1].Tiles[10 + xx][11 + yy] = new AutoTile(anim);
                    }
                }

                for (int xx = 0; xx < 2; xx++)
                {
                    for (int yy = 0; yy < 4; yy++)
                    {
                        if (!(yy == 0 && xx == 1))
                        {
                            TileLayer anim_left = new TileLayer(10);
                            for (int ii = 0; ii < 3; ii++)
                                anim_left.Frames.Add(new TileFrame(new Loc(ii * 3 + xx, 6 + yy), "LuminousSpringAnim"));
                            map.Layers[1].Tiles[8 + xx][1 + yy] = new AutoTile(anim_left);
                        }

                        if (!(yy == 0 && xx == 0))
                        {
                            TileLayer anim_right = new TileLayer(10);
                            for (int ii = 0; ii < 3; ii++)
                                anim_right.Frames.Add(new TileFrame(new Loc(ii * 3 + xx, 6 + 7 + yy), "LuminousSpringAnim"));
                            map.Layers[1].Tiles[15 + xx][1 + yy] = new AutoTile(anim_right);
                        }
                    }
                }

                for (int yy = 0; yy < 2; yy++)
                {
                    TileLayer anim_left = new TileLayer(10);
                    for (int ii = 0; ii < 3; ii++)
                        anim_left.Frames.Add(new TileFrame(new Loc(ii * 3, 10 + yy), "LuminousSpringAnim"));
                    map.Layers[1].Tiles[6][9 + yy] = new AutoTile(anim_left);

                    TileLayer anim_right = new TileLayer(10);
                    for (int ii = 0; ii < 3; ii++)
                        anim_right.Frames.Add(new TileFrame(new Loc(ii * 3, 10 + 7 + yy), "LuminousSpringAnim"));
                    map.Layers[1].Tiles[18][9 + yy] = new AutoTile(anim_right);
                }

                {
                    TileLayer anim_left = new TileLayer(10);
                    for (int ii = 0; ii < 3; ii++)
                        anim_left.Frames.Add(new TileFrame(new Loc(ii * 3, 12), "LuminousSpringAnim"));
                    map.Layers[1].Tiles[7][12] = new AutoTile(anim_left);

                    TileLayer anim_right = new TileLayer(10);
                    for (int ii = 0; ii < 3; ii++)
                        anim_right.Frames.Add(new TileFrame(new Loc(ii * 3, 12 + 7), "LuminousSpringAnim"));
                    map.Layers[1].Tiles[17][12] = new AutoTile(anim_right);
                }


                for (int xx = 0; xx < width * texSize; xx++)
                {
                    for (int yy = 0; yy < height * texSize; yy++)
                    {
                        if (yy == 37)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 23 || xx >= 52 && xx < 75) ? 1u : 0u);
                        else if (yy == 38)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 23 || xx >= 52 && xx < 75) ? 1u : 0u);
                        else if (yy == 39)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 23 || xx >= 52 && xx < 75) ? 1u : 0u);
                        else if (yy == 40)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 23 || xx >= 52 && xx < 75) ? 1u : 0u);
                        else if (yy == 41)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 23 || xx >= 52 && xx < 75) ? 1u : 0u);
                        else if (yy == 42)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 23 || xx >= 52 && xx < 75) ? 1u : 0u);
                        else if (yy == 43)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 44)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 45)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 46)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 47)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 48)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 49)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 50)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 51)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 23 || xx >= 52 && xx < 75) ? 1u : 0u);
                        else if (yy == 52)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 23 || xx >= 52 && xx < 75) ? 1u : 0u);
                        else if (yy == 53)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 23 || xx >= 52 && xx < 75) ? 1u : 0u);
                        else if (yy == 54)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 23 || xx >= 52 && xx < 75) ? 1u : 0u);
                        else if (yy == 55)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 56)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 57)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 58)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 59)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 60)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 61)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 62)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 24 || xx >= 51 && xx < 75) ? 1u : 0u);
                        else if (yy == 63)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 25 || xx >= 50 && xx < 75) ? 1u : 0u);
                        else if (yy == 64)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 28 || xx >= 47 && xx < 75) ? 1u : 0u);
                        else if (yy == 65)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 29 || xx >= 46 && xx < 75) ? 1u : 0u);
                        else if (yy == 66)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 30 || xx >= 45 && xx < 75) ? 1u : 0u);
                        else if (yy == 67)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 30 || xx >= 45 && xx < 75) ? 1u : 0u);
                        else if (yy == 68)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 30 || xx >= 45 && xx < 75) ? 1u : 0u);
                        else if (yy == 69)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 30 || xx >= 45 && xx < 75) ? 1u : 0u);
                        else if (yy == 70)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 31 || xx >= 44 && xx < 75) ? 1u : 0u);
                        else if (yy == 71)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 32 || xx >= 43 && xx < 75) ? 1u : 0u);
                        else if (yy == 72)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 32 || xx >= 43 && xx < 75) ? 1u : 0u);
                        else if (yy == 73)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 32 || xx >= 43 && xx < 75) ? 1u : 0u);
                        else if (yy == 74)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 32 || xx >= 43 && xx < 75) ? 1u : 0u);
                    }
                }

                map.AddMarker("entrance_south", new Loc(292, 580), Dir8.Up);


                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(0, 596, 600, 4), true, "South_Exit");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(184, 296, 232, 16), true, "Spring");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Assembly", 1, 0, 0), new Rect(384, 384, 24, 24), new Loc(0, 8), false, "Assembly");
                    //groundObject.Events.Add(new AssemblyEvent());
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("Storage", 1), new Rect(384, 480, 24, 24), new Loc(4, 8), false, "Storage");
                    //groundObject.Events.Add(new StorageEvent());
                    map.AddObject(groundObject);
                }

                map.AddSpawner(CreateTeamSpawner(1, new Loc(216, 472), Dir8.DownRight));
                map.AddSpawner(CreateTeamSpawner(2, new Loc(216, 432), Dir8.DownRight));
                map.AddSpawner(CreateTeamSpawner(3, new Loc(216, 392), Dir8.DownRight));

                {
                    GroundSpawner evoSpawner = new GroundSpawner("EVO_SUBJECT", "EvoSubject", new CharData());
                    evoSpawner.Position = new Loc(292, 312);
                    evoSpawner.Direction = Dir8.Up;
                    evoSpawner.EntityCallbacks.Add(LuaEngine.EEntLuaEventTypes.Action);
                    map.AddSpawner(evoSpawner);
                }

                map.Music = "A10. Luminous Spring.ogg";
            }
            else if (name == MapNames[12])
            {
                int width = 16;
                int height = 13;
                int texSize = 3;
                map.CreateNew(width, height, texSize);
                map.Name = new LocalText("Pelipper Post Office");
                map.EdgeView = Map.ScrollEdge.Clamp;

                for (int xx = 0; xx < width; xx++)
                {
                    for (int yy = 0; yy < height; yy++)
                        map.Layers[0].Tiles[xx][yy] = new AutoTile(new TileLayer(new Loc(xx, yy), "PostOffice"));
                }

                for (int xx = 0; xx < width * texSize; xx++)
                {
                    for (int yy = 0; yy < height * texSize; yy++)
                    {
                        if (yy <= 15)
                            SetObstacle(map, xx, yy, 1);
                        else if (yy == 16)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 19 || xx >= 34 && xx < 48) ? 1u : 0u);
                        else if (yy == 17)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 18 || xx >= 34 && xx < 48) ? 1u : 0u);
                        else if (yy == 18)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 17 || xx >= 34 && xx < 48) ? 1u : 0u);
                        else if (yy == 19)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 34 && xx < 48) ? 1u : 0u);
                        else if (yy == 20)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 34 && xx < 48) ? 1u : 0u);
                        else if (yy == 21)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 34 && xx < 48) ? 1u : 0u);
                        else if (yy == 22)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20 || xx >= 24 && xx < 27 || xx >= 31 && xx < 48) ? 1u : 0u);
                        else if (yy == 23)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20 || xx >= 24 && xx < 27 || xx >= 31 && xx < 48) ? 1u : 0u);
                        else if (yy == 24)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 20 || xx >= 24 && xx < 27 || xx >= 31 && xx < 48) ? 1u : 0u);
                        else if (yy == 25)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 12 || xx >= 36 && xx < 48) ? 1u : 0u);
                        else if (yy == 26)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 12 || xx >= 36 && xx < 48) ? 1u : 0u);
                        else if (yy == 27)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 12 || xx >= 36 && xx < 48) ? 1u : 0u);
                        else if (yy == 28)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 13 || xx >= 36 && xx < 48) ? 1u : 0u);
                        else if (yy == 29)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 13 || xx >= 36 && xx < 48) ? 1u : 0u);
                        else if (yy == 30)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 14 || xx >= 36 && xx < 48) ? 1u : 0u);
                        else if (yy == 31)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 15 || xx >= 36 && xx < 48) ? 1u : 0u);
                        else if (yy == 32)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 36 && xx < 48) ? 1u : 0u);
                        else if (yy == 33)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 35 && xx < 48) ? 1u : 0u);
                        else if (yy == 34)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 16 || xx >= 33 && xx < 48) ? 1u : 0u);
                        else if (yy == 35)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 17 || xx >= 31 && xx < 48) ? 1u : 0u);
                        else if (yy == 36)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 21 || xx >= 27 && xx < 48) ? 1u : 0u);
                        else if (yy == 37)
                            SetObstacle(map, xx, yy, (xx >= 0 && xx < 21 || xx >= 27 && xx < 48) ? 1u : 0u);
                        else if (yy == 38)
                            SetObstacle(map, xx, yy, 1);
                    }
                }

                map.AddMarker("entrance_south", new Loc(184, 284), Dir8.Up);
                map.AddMarker("entrance_counter", new Loc(224, 200), Dir8.Up);

                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(0, 300, 384, 4), true, "South_Exit");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(160, 176, 32, 24), false, "Main_Desk");
                    map.AddObject(groundObject);
                }
                {
                    GroundObject groundObject = new GroundObject(new ObjAnimData("", 0), new Rect(216, 176, 32, 24), false, "Side_Desk");
                    map.AddObject(groundObject);
                }

                {
                    GroundChar groundChar = new GroundChar(new MonsterID("pelipper", 0, "normal", Gender.Male), new Loc(168, 158), Dir8.Down, "Connect_Owner");
                    groundChar.CharDir = Dir8.Down;
                    map.AddMapChar(groundChar);
                }

                {
                    GroundChar groundChar = new GroundChar(new MonsterID("pelipper", 0, "normal", Gender.Male), new Loc(224, 158), Dir8.Down, "Rescue_Owner");
                    groundChar.CharDir = Dir8.Down;
                    map.AddMapChar(groundChar);
                }

                map.Music = "A02. Base Town.ogg";
            }
            else
            {
                return null;
            }

            if (map.Name.DefaultText.StartsWith("**"))
                map.Name.DefaultText = map.Name.DefaultText.Replace("*", "");
            else if (map.Name.DefaultText != "")
                map.Released = true;

            return map;
        }


        static GroundSpawner CreateAssemblySpawner(int index, Loc loc, Dir8 dir)
        {
            GroundSpawner teamSpawner = new GroundSpawner("ASSEMBLY_" + index, "Assembly" + index, new CharData());
            teamSpawner.Position = loc;
            teamSpawner.Direction = dir;
            teamSpawner.EntityCallbacks.Add(LuaEngine.EEntLuaEventTypes.Action);
            return teamSpawner;
        }
        static GroundSpawner CreateTeamSpawner(int index, Loc loc, Dir8 dir)
        {
            GroundSpawner teamSpawner = new GroundSpawner("TEAMMATE_" + index, "Teammate" + index, new CharData());
            teamSpawner.Position = loc;
            teamSpawner.Direction = dir;
            teamSpawner.EntityCallbacks.Add(LuaEngine.EEntLuaEventTypes.Action);
            return teamSpawner;
        }
    }

}
