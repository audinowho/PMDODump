using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using RogueEssence.Dungeon;
using RogueEssence.Content;
using RogueElements;
using RogueEssence;
using RogueEssence.Data;
using System.Linq;
using PMDC;
using PMDC.Data;
using System.Data.SQLite;

namespace DataGenerator.Data
{
    public static class MonsterInfo
    {
        public enum DexColor
        {
            Black,
            Blue,
            Brown,
            Gray,
            Green,
            Pink,
            Purple,
            Red,
            White,
            Yellow
        }

        public enum BodyShape
        {
            Head,
            Serpentine,
            Fins,
            HeadAndArms,
            HeadAndBase,
            BipedWithTail,
            HeadAndLegs,
            Quadruped,
            Wings,
            Tentacles,
            MultiBody,
            Biped,
            MultiWings,
            Insectoid
        }


        const int TOTAL_DEX = 1011;
        const bool GENDER_UNLOCK = true;
        static Dictionary<int, string> langIDs;
        static Dictionary<int, int> langCols;
        static int[] existing = { 1, 3, 5, 6, 7, 8, 9 };
        static int[] straggler = { 11 };
        static int[] added = { 4, 12 };
        const int TOTAL_LANG_COLS = 13;
        const int ADD_VERSION_ID = 25;
        static string MONSTER_PATH { get => GenPath.DATA_GEN_PATH + "Monster/"; }
        static string TL_FILE { get => MONSTER_PATH + "pokedex.9.sqlite"; }

        static List<string> monsterKeys;

        public static void AddMonsterData()
        {
            //DataInfo.DeleteIndexedData(DataManager.DataType.Monster.ToString());

            langIDs = new Dictionary<int, string>();
            langIDs.Add(1, "ja");
            langIDs.Add(3, "ko");
            langIDs.Add(4, "zh-hant");
            langIDs.Add(5, "fr");
            langIDs.Add(6, "de");
            langIDs.Add(7, "es");
            langIDs.Add(8, "it");
            langIDs.Add(11, "ja-jp");
            langIDs.Add(12, "zh-hans");

            //it is assumed that the translation table uses the language convention of
            //[EN], FR, DE, ES, PT, IT, JA-HRKT, JA KO, ZH-HANT, ZH-HANS, etc.
            langCols = new Dictionary<int, int>();
            langCols.Add(9, 2);//EN
            langCols.Add(5, 3);//FR
            langCols.Add(6, 4);//DE
            langCols.Add(7, 5);//ES
            //PT
            langCols.Add(8, 7);//IT
            langCols.Add(3, 8);//KO
            langCols.Add(1, 9);//JA
            langCols.Add(11, 10);//JA-JP
            langCols.Add(12, 11);//ZH-HANS
            langCols.Add(4, 12);//ZH-HANT
            //ETC

            CreateDex();
            //CreateMoves();
            //CreateMoveCodes();
            //CreateAbilities();
            //CreateAbilityCodes();
            //CreateItems();
        }

        public static void CreateDex()
        {
            //SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + DEX_FILE + ";Version=3;");
            //m_dbConnection.Open();
            //SQLiteConnection m_db7Connection = new SQLiteConnection("Data Source=" + DEX_7_FILE + ";Version=3;");
            //m_db7Connection.Open();
            SQLiteConnection m_dbTLConnection = new SQLiteConnection("Data Source="+ TL_FILE + ";Version=3;");
            m_dbTLConnection.Open();

            Directory.CreateDirectory("Dex");

            MonsterData[] totalEntries = new MonsterData[TOTAL_DEX];

            Dictionary<string, List<byte>> personalities = GetPersonalityChecklist();
            monsterKeys = new List<string>();
            for (int ii = 0; ii < TOTAL_DEX; ii++)
            {
                monsterKeys.Add(LoadDexAssetName(m_dbTLConnection, ii));
            }

            for (int ii = 0; ii < TOTAL_DEX; ii++)
            {
                totalEntries[ii] = LoadDex(m_dbTLConnection, ii);
                Console.WriteLine("#" + ii + " " + totalEntries[ii].Name + " Read");
                for (int jj = 0; jj < totalEntries[ii].Forms.Count; jj++)
                {
                    List<byte> personality;
                    int actualForme = jj;
                    while (!personalities.TryGetValue(ii + "-" + actualForme, out personality) && actualForme > 0)
                        actualForme--;
                    if (personality == null)
                        personality = new List<byte>();
                    ((MonsterFormData)totalEntries[ii].Forms[jj]).Personalities = personality;
                }
            }

            m_dbTLConnection.Close();

            WritePersonalityChecklist(personalities, totalEntries);

            CreateLearnables(totalEntries);
            ListAllIncompletes(totalEntries);


            //string outpt = "";
            for (int ii = 0; ii < TOTAL_DEX; ii++)
            {
                totalEntries[ii].JoinRate = MapJoinRate(ii, totalEntries);
                MapFriendshipEvo(ii, totalEntries);
                MapFormEvo(ii, totalEntries);
                //if (TotalEntries[ii].JoinRate > 0)
                //    outpt += (TotalEntries[ii].Name + ": " + TotalEntries[ii].JoinRate + "\n");
            }

            for (int ii = 0; ii < TOTAL_DEX; ii++)
            {
                string fileName = getAssetName(totalEntries[ii].Name.DefaultText);
                DataManager.SaveData(fileName, DataManager.DataType.Monster.ToString(), totalEntries[ii]);
                //TotalEntries[ii].SaveXml("Dex\\" + ii + ".xml");
                Console.WriteLine("#" + ii + " " + totalEntries[ii].Name + " Written");
            }
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }


        public static Dictionary<string, List<byte>> GetPersonalityChecklist()
        {
            Dictionary<string, List<byte>> pairs = new Dictionary<string, List<byte>>();
            int[] personality_map = new int[] {00, 01, 02, 03, 04, 05, 06,
                07, 08, 09, 10, 11, 12, 13, 14, 15, 16, 24, 26, 29, 30,
                31, 32, 33, 34, 35, 36, 37};

            using (StreamReader file = new StreamReader(MONSTER_PATH+"personality_in.txt"))
            {
                while (!file.EndOfStream)
                {
                    string[] checks = file.ReadLine().Trim().Split('\t');
                    List<byte> personalities = new List<byte>();
                    for (int ii = 0; ii < personality_map.Length; ii++)
                    {
                        if (checks.Length > ii + 2 && checks[ii + 2] == "x")
                            personalities.Add((byte)personality_map[ii]);
                    }
                    pairs[checks[0] + "-" + checks[1]] = personalities;
                }
            }
            return pairs;
        }

        public static void WritePersonalityChecklist(Dictionary<string, List<byte>> pairs, MonsterData[] totalEntries)
        {
            byte[] personality_map = new byte[] {00, 01, 02, 03, 04, 05, 06,
                07, 08, 09, 10, 11, 12, 13, 14, 15, 16, 24, 26, 29, 30,
                31, 32, 33, 34, 35, 36, 37};

            List<string> personalities_out = new List<string>();

            for (int ii = 0; ii < TOTAL_DEX; ii++)
            {
                for (int jj = 0; jj < totalEntries[ii].Forms.Count; jj++)
                {
                    bool include = !totalEntries[ii].Forms[jj].Temporary;
                    // forms that are temporary but result in personality change
                    switch (ii)
                    {
                        case 421://cherrim
                        case 746://wishiwashi
                        case 774://minior
                        case 877://morpeko
                        case 964://palafin
                            include = true;
                            break;
                    }
                    //forms that are not temporary but do not result in personality change
                    if (jj > 0)
                    {
                        switch (ii)
                        {
                            case 201://unown
                            case 869://alcremie
                            case 931://squawkabilliy
                                include = false;
                                break;
                        }
                    }
                    if (!include)
                        continue;

                    string[] checks = new string[personality_map.Length];
                    for (int kk = 0; kk < checks.Length; kk++)
                        checks[kk] = "";

                    List<byte> personalities;
                    if (pairs.TryGetValue(ii + "-" + jj, out personalities))
                    {
                        foreach (byte personality in personalities)
                        {
                            int map_idx = Array.FindIndex(personality_map, w => w == personality);
                            checks[map_idx] = "x";
                        }
                    }
                    personalities_out.Add(totalEntries[ii].Forms[jj].FormName.DefaultText + "\t" + ii.ToString() + "\t" + jj.ToString() + "\t" + String.Join("\t", checks));
                }
            }

            using (StreamWriter file = new StreamWriter(MONSTER_PATH+"personality_out.txt"))
            {
                for (int ii = 0; ii < personalities_out.Count; ii++)
                    file.WriteLine(personalities_out[ii]);
            }
        }

        public static MonsterData LoadZero()
        {
            MonsterData entry = new MonsterData();
            entry.IndexNum = 0;
            entry.Name = new LocalText("Missingno.");
            entry.Title = new LocalText("???");
            //entry.Color = DexColor.Gray;
            //entry.BodyStyle = BodyShape.HeadAndBase;
            entry.JoinRate = 0;
            entry.EXPTable = Text.Sanitize(Text.GetMemberTitle(GrowthInfo.GrowthGroup.MediumSlow.ToString())).ToLower();
            entry.SkillGroup1 = Text.Sanitize(Text.GetMemberTitle(SkillGroupInfo.EggGroup.Undiscovered.ToString())).ToLower();
            entry.SkillGroup2 = Text.Sanitize(Text.GetMemberTitle(SkillGroupInfo.EggGroup.Undiscovered.ToString())).ToLower();


            MonsterFormData formEntry = new MonsterFormData();
            formEntry.FormName = new LocalText("Missingno.");
            formEntry.GenderlessWeight = 1;
            formEntry.Height = 1;
            formEntry.Weight = 1;
            formEntry.ExpYield = 1;
            formEntry.BaseHP = 1;
            formEntry.BaseAtk = 1;
            formEntry.BaseDef = 1;
            formEntry.BaseMAtk = 1;
            formEntry.BaseMDef = 1;
            formEntry.BaseSpeed = 1;
            formEntry.Element1 = Text.Sanitize(Text.GetMemberTitle(ElementInfo.Element.None.ToString())).ToLower();
            formEntry.Element2 = Text.Sanitize(Text.GetMemberTitle(ElementInfo.Element.None.ToString())).ToLower();
            formEntry.Intrinsic1 = "none";
            formEntry.Intrinsic2 = "none";
            formEntry.Intrinsic3 = "none";
            formEntry.LevelSkills.Add(new LevelUpSkill("attack", 0));
            entry.Forms.Add(formEntry);

            MonsterFormData formEntry2 = new MonsterFormData();
            formEntry2.FormName = new LocalText("Substitute");
            formEntry2.GenderlessWeight = 1;
            formEntry2.Height = 1;
            formEntry2.Weight = 1;
            formEntry2.ExpYield = 1;
            formEntry2.BaseHP = 1;
            formEntry2.BaseAtk = 1;
            formEntry2.BaseDef = 1;
            formEntry2.BaseMAtk = 1;
            formEntry2.BaseMDef = 1;
            formEntry2.BaseSpeed = 1;
            formEntry2.Element1 = Text.Sanitize(Text.GetMemberTitle(ElementInfo.Element.None.ToString())).ToLower();
            formEntry2.Element2 = Text.Sanitize(Text.GetMemberTitle(ElementInfo.Element.None.ToString())).ToLower();
            formEntry2.Intrinsic1 = "none";
            formEntry2.Intrinsic2 = "none";
            formEntry2.Intrinsic3 = "none";
            formEntry2.LevelSkills.Add(new LevelUpSkill("attack", 0));
            entry.Forms.Add(formEntry2);



            return entry;
        }

        public static string LoadDexAssetName(SQLiteConnection m_dbTLConnection, int index)
        {
            if (index == 0)
                return getAssetName("Missingno.");

            //name
            string name = "";
            string sql = "SELECT * FROM pokemon_v2_pokemonspeciesname WHERE pokemon_species_id = " + index + " AND language_id = 9";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbTLConnection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                int read = 0;
                while (reader.Read())
                {
                    if (read > 0)
                        throw new Exception("#" + index + ": More than 1 Index result!?");
                    name = reader["name"].ToString();
                    if (name.Contains('’'))
                        name = name.Replace('’', '\'');
                    read++;
                }
            }
            return getAssetName(name);
        }

        public static MonsterData LoadDex(SQLiteConnection m_dbTLConnection, int index)
        {
            if (index == 0)
                return LoadZero();
            MonsterData entry = new MonsterData();
            entry.IndexNum = index; 

            //name
            //species name
            string sql = "SELECT * FROM pokemon_v2_pokemonspeciesname WHERE pokemon_species_id = " + index + " AND language_id = 9";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbTLConnection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                int read = 0;
                while (reader.Read())
                {
                    if (read > 0)
                        throw new Exception("#" + index + ": More than 1 Index result!?");
                    entry.Name = new LocalText(reader["name"].ToString());
                    if (entry.Name.DefaultText.Contains('’'))
                    {
                        Console.WriteLine(entry.Name.DefaultText + " has a ’.  Replacing with '");
                        //Console.ReadLine();
                        entry.Name.DefaultText = entry.Name.DefaultText.Replace('’', '\'');
                    }
                    entry.Title = new LocalText(reader["genus"].ToString());
                    read++;
                }
            }

            foreach (int key in langIDs.Keys)
            {
                sql = "SELECT * FROM pokemon_v2_pokemonspeciesname WHERE pokemon_species_id = " + index + " AND language_id = " + key;
                command = new SQLiteCommand(sql, m_dbTLConnection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    int read = 0;
                    while (reader.Read())
                    {
                        if (read > 0)
                            throw new Exception("#" + index + ": More than 1 Index result!?");
                        entry.Name.LocalTexts[langIDs[key]] = reader["name"].ToString();
                        entry.Title.LocalTexts[langIDs[key]] = reader["genus"].ToString();
                        read++;
                    }
                }
            }

            //color
            //shape
            //gender - transferred
            int Ratio = -1;
            //catch rate
            //growth
            sql = "SELECT * FROM pokemon_v2_pokemonspecies WHERE id = " + index;
            command = new SQLiteCommand(sql, m_dbTLConnection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                int read = 0;
                while (reader.Read())
                {
                    if (read > 0)
                        throw new Exception(entry.Name + ": More than 1 Species result!?");
                    //entry.Color = MapColorStyle(Convert.ToInt32(reader["color_id"].ToString()));
                    //entry.BodyStyle = MapBodyStyle(Convert.ToInt32(reader["shape_id"].ToString()));
                    Ratio = Convert.ToInt32(reader["gender_rate"].ToString());
                    entry.JoinRate = Convert.ToInt32(reader["capture_rate"].ToString());
                    entry.EXPTable = Text.Sanitize(Text.GetMemberTitle(MapGrowthGroup(Convert.ToInt32(reader["growth_rate_id"].ToString())).ToString())).ToLower();
                    string prevo = reader["evolves_from_species_id"].ToString();
                    if (!String.IsNullOrEmpty(prevo))
                        entry.PromoteFrom = monsterKeys[Convert.ToInt32(prevo)];
                    read++;
                }
            }

            //egg groups
            sql = "SELECT * FROM pokemon_v2_pokemonegggroup WHERE pokemon_species_id = " + index;
            command = new SQLiteCommand(sql, m_dbTLConnection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                int read = 0;
                while (reader.Read())
                {
                    if (read > 1)
                        throw new Exception(entry.Name + ": More than 2 Egg Group results!?");
                    else if (read == 0)
                        entry.SkillGroup1 = Text.Sanitize(Text.GetMemberTitle(MapEggGroup(Convert.ToInt32(reader["egg_group_id"].ToString())).ToString())).ToLower();
                    else if (read == 1)
                        entry.SkillGroup2 = Text.Sanitize(Text.GetMemberTitle(MapEggGroup(Convert.ToInt32(reader["egg_group_id"].ToString())).ToString())).ToLower();
                    read++;
                }
            }

            //evo/devo data
            sql = "SELECT * FROM pokemon_v2_pokemonspecies a, pokemon_v2_pokemonevolution b WHERE b.evolved_species_id = a.id AND a.evolves_from_species_id = " + index;
            command = new SQLiteCommand(sql, m_dbTLConnection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                int read = 0;
                while (reader.Read())
                {
                    int evoSpecies = Convert.ToInt32(reader["id"].ToString());

                    if (evoSpecies == 292)//manually account for shedinja
                        continue;

                    bool hasEvo = false;
                    foreach (PromoteBranch currentBranch in entry.Promotions)
                    {
                        if (currentBranch.Result == monsterKeys[evoSpecies])
                            hasEvo = true;
                    }
                    if (hasEvo)
                        continue;

                    PromoteBranch branch = new PromoteBranch();
                    branch.Result = monsterKeys[evoSpecies];
                    int evoMethod = Convert.ToInt32(reader["evolution_trigger_id"].ToString());//level/trade/useitem/shed

                    string itemNum = reader["evolution_item_id"].ToString();
                    string level = reader["min_level"].ToString();
                    string gender = reader["gender_id"].ToString();
                    string location = reader["location_id"].ToString();
                    string heldItem = reader["held_item_id"].ToString();
                    string time = reader["time_of_day"].ToString();
                    string move = reader["known_move_id"].ToString();
                    string moveType = reader["known_move_type_id"].ToString();
                    string friendship = reader["min_happiness"].ToString();
                    string beauty = reader["min_beauty"].ToString();
                    string affection = reader["min_affection"].ToString();
                    string stats = reader["relative_physical_stats"].ToString();
                    string allySpecies = reader["party_species_id"].ToString();
                    string allyType = reader["party_type_id"].ToString();
                    string tradeSpecies = reader["trade_species_id"].ToString();
                    bool rain = (bool)reader["needs_overworld_rain"];
                    bool flip = (bool)reader["turn_upside_down"];

                    try
                    {
                        if (evoSpecies == 367)//huntail
                        {
                            EvoStatBoost evoDetail = new EvoStatBoost();
                            evoDetail.StatBoostStatus = "mod_special_attack";
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 368)//gorebyss
                        {
                            EvoStatBoost evoDetail = new EvoStatBoost();
                            evoDetail.StatBoostStatus = "mod_special_defense";
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 738)//vikavolt
                        {
                            //FIXME
                            EvoLocation evoDetail = new EvoLocation();
                            evoDetail.TileElement = Text.Sanitize(ElementInfo.Element.Steel.ToString()).ToLower();
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 740)//crabominable
                        {
                            //FIXME
                            EvoItem evoDetail = new EvoItem();
                            evoDetail.ItemNum = "evo_ice_stone";//Ice Stone
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 745)//lycanroc
                        {
                            EvoLevel evoDetail = new EvoLevel();
                            evoDetail.Level = Convert.ToInt32(level);
                            branch.Details.Add(evoDetail);

                            EvoFormDusk dusk = new EvoFormDusk();
                            dusk.DefaultForm = 2;
                            dusk.ItemMap["evo_sun_ribbon"] = 0;
                            dusk.ItemMap["evo_lunar_ribbon"] = 1;
                            branch.Details.Add(dusk);
                        }
                        else if (evoSpecies == 841)//flapple
                        {
                            EvoItem evoDetail = new EvoItem();
                            evoDetail.ItemNum = "food_apple_big";//Apple
                            branch.Details.Add(evoDetail);

                            EvoHunger hungerDetail = new EvoHunger();
                            branch.Details.Add(hungerDetail);
                        }
                        else if (evoSpecies == 842)//appletun
                        {
                            EvoItem evoDetail = new EvoItem();
                            evoDetail.ItemNum = "food_apple_huge";//Apple
                            branch.Details.Add(evoDetail);

                            EvoHunger hungerDetail = new EvoHunger();
                            hungerDetail.Hungry = true;
                            branch.Details.Add(hungerDetail);
                        }
                        else if (evoSpecies == 853)//grapploct
                        {
                            //FIXME
                            EvoMove evoDetail = new EvoMove();
                            evoDetail.MoveNum = "taunt";//taunt
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 855)//polteageist
                        {
                            EvoItem evoDetail = new EvoItem();
                            evoDetail.ItemNum = "evo_cracked_pot";//Cracked Pot
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 862)//obstagoon
                        {
                            //FIXME
                            EvoLevel evoDetail = new EvoLevel();
                            evoDetail.Level = Convert.ToInt32(35);
                            branch.Details.Add(evoDetail);

                            EvoItem evoDetail2 = new EvoItem();
                            evoDetail2.ItemNum = "evo_lunar_ribbon";//moon ribbon
                            branch.Details.Add(evoDetail2);
                        }
                        else if (evoSpecies == 865)//sirfetch'd
                        {
                            //FIXME
                            EvoCrits evoDetail = new EvoCrits();
                            evoDetail.CritStatus = "crits_landed";
                            evoDetail.Stack = 10;
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 867)//runerigus
                        {
                            //FIXME
                            EvoWeather evoDetail = new EvoWeather();
                            evoDetail.Weather = "sandstorm";//sandstorm
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 899)//wyrdeer
                        {
                            //FIXME
                            EvoMoveUse evoDetail = new EvoMoveUse();
                            evoDetail.LastMoveStatusID = "last_used_move";
                            evoDetail.MoveRepeatStatusID = "times_move_used";
                            evoDetail.MoveNum = "psyshield_bash";
                            evoDetail.Amount = 10;
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 900)//kleavor
                        {
                            //FIXME
                            EvoItem evoDetail = new EvoItem();
                            evoDetail.ItemNum = "held_hard_stone";
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 901)//ursaluna
                        {
                            //FIXME
                            EvoItem evoDetail = new EvoItem();
                            evoDetail.ItemNum = "held_soft_sand";
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 902)//basculegion
                        {
                            //FIXME
                            EvoTookDamage evoDetail = new EvoTookDamage();
                            evoDetail.Amount = 200;
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 904)//overqwil
                        {
                            //FIXME
                            EvoMoveUse evoDetail = new EvoMoveUse();
                            evoDetail.LastMoveStatusID = "last_used_move";
                            evoDetail.MoveRepeatStatusID = "times_move_used";
                            evoDetail.MoveNum = "barb_barrage";
                            evoDetail.Amount = 10;
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 923)//pawmot
                        {
                            //FIXME
                            EvoWalk evoDetail = new EvoWalk();
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 925)//maushold
                        {
                            //FIXME
                            EvoLevel evoDetail = new EvoLevel();
                            evoDetail.Level = Convert.ToInt32(25);
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 936)//armarouge
                        {
                            //FIXME
                            EvoItem evoDetail = new EvoItem();
                            evoDetail.ItemNum = "empty";
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 937)//ceruledge
                        {
                            //FIXME
                            EvoItem evoDetail = new EvoItem();
                            evoDetail.ItemNum = "empty";
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 947)//brambleghast
                        {
                            //FIXME
                            EvoWalk evoDetail = new EvoWalk();
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 954)//rabsca
                        {
                            //FIXME
                            EvoWalk evoDetail = new EvoWalk();
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 964)//palafin
                        {
                            //FIXME
                            EvoRescue evoDetail = new EvoRescue();
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 979)//annihilape
                        {
                            //FIXME
                            EvoMoveUse evoDetail = new EvoMoveUse();
                            evoDetail.LastMoveStatusID = "last_used_move";
                            evoDetail.MoveRepeatStatusID = "times_move_used";
                            evoDetail.MoveNum = "rage_fist";
                            evoDetail.Amount = 10;
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 983)//kingambit
                        {
                            //FIXME
                            EvoKillCount evoDetail = new EvoKillCount();
                            evoDetail.Amount = 10;
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 1000)//gholdengo
                        {
                            //FIXME
                            EvoMoney evoDetail = new EvoMoney();
                            evoDetail.Amount = 1000000;
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 869)//alcremie
                            branch.Details.Add(new EvoFormCream());
                        else if (evoSpecies == 892)//urshifu
                            branch.Details.Add(new EvoFormScroll());
                        else if (evoMethod == 1)
                        {
                            //all evolution classes need to be made, however they don't all need to be mapped to
                            if (CheckEvoConditions(reader, "min_level"))
                            {
                                EvoLevel evoDetail = new EvoLevel();
                                evoDetail.Level = Convert.ToInt32(level);
                                branch.Details.Add(evoDetail);
                            }
                            else if (CheckEvoConditions(reader, "min_level", "gender_id"))
                            {
                                EvoLevel evoDetail = new EvoLevel();
                                evoDetail.Level = Convert.ToInt32(level);
                                branch.Details.Add(evoDetail);

                                EvoGender evoDetail2 = new EvoGender();
                                evoDetail2.ReqGender = MapGender(Convert.ToInt32(gender));
                                branch.Details.Add(evoDetail2);
                            }
                            else if (CheckEvoConditions(reader, "location_id"))
                            {
                                if (Convert.ToInt32(location) == 10 || Convert.ToInt32(location) == 379 || Convert.ToInt32(location) == 629)
                                {
                                    EvoLocation evoDetail = new EvoLocation();
                                    evoDetail.TileElement = Text.Sanitize(ElementInfo.Element.Steel.ToString()).ToLower();
                                    branch.Details.Add(evoDetail);
                                }
                                else if (Convert.ToInt32(location) == 8 || Convert.ToInt32(location) == 375 || Convert.ToInt32(location) == 650)
                                {
                                    EvoItem evoDetail = new EvoItem();
                                    evoDetail.ItemNum = "evo_leaf_stone";//Leaf Stone
                                    branch.Details.Add(evoDetail);
                                }
                                else if (Convert.ToInt32(location) == 48 || Convert.ToInt32(location) == 380 || Convert.ToInt32(location) == 640)
                                {
                                    EvoItem evoDetail = new EvoItem();
                                    evoDetail.ItemNum = "evo_ice_stone";//Ice Stone
                                    branch.Details.Add(evoDetail);
                                }
                                else
                                    throw new Exception(entry.Name + ": Unknown Loc Evo method");

                            }
                            else if (CheckEvoConditions(reader, "min_happiness"))
                            {
                                EvoFriendship evoDetail = new EvoFriendship();
                                branch.Details.Add(evoDetail);
                            }
                            else if (CheckEvoConditions(reader, "min_level", "time_of_day"))
                            {
                                EvoLevel evoDetail = new EvoLevel();
                                evoDetail.Level = Convert.ToInt32(level);
                                branch.Details.Add(evoDetail);

                                EvoItem evoDetail2 = new EvoItem();
                                evoDetail2.ItemNum = (time == "night") ? "evo_lunar_ribbon" : "evo_sun_ribbon";
                                //EvoTime evoDetail2 = new EvoTime();
                                //evoDetail2.Time = (time == "night") ? Maps.ZoneManager.TimeOfDay.Night : Maps.ZoneManager.TimeOfDay.Day;
                                branch.Details.Add(evoDetail2);
                            }
                            else if (CheckEvoConditions(reader, "min_happiness", "time_of_day"))
                            {
                                EvoFriendship evoDetail = new EvoFriendship();
                                branch.Details.Add(evoDetail);

                                if (index < monsterKeys.IndexOf(branch.Result)) //pre-evos are excluded from time of day req
                                {
                                    EvoItem evoDetail2 = new EvoItem();
                                    evoDetail2.ItemNum = (time == "night") ? "evo_lunar_ribbon" : "evo_sun_ribbon";
                                    //EvoTime evoDetail2 = new EvoTime();
                                    //evoDetail2.Time = (time == "night") ? Maps.ZoneManager.TimeOfDay.Night : Maps.ZoneManager.TimeOfDay.Day;
                                    branch.Details.Add(evoDetail2);
                                }
                            }
                            else if (CheckEvoConditions(reader, "held_item_id", "time_of_day"))
                            {
                                //EvoItem evoDetail = new EvoItem();
                                //evoDetail.ItemNum = MapItem(Convert.ToInt32(heldItem));
                                //branch.Details.Add(evoDetail);

                                //EvoTime evoDetail2 = new EvoTime();
                                //evoDetail2.Time = (time == "night") ? Maps.ZoneManager.TimeOfDay.Night : Maps.ZoneManager.TimeOfDay.Day;
                                EvoItem evoDetail2 = new EvoItem();
                                evoDetail2.ItemNum = (time == "night") ? "evo_lunar_ribbon" : "evo_sun_ribbon";
                                branch.Details.Add(evoDetail2);
                            }
                            else if (CheckEvoConditions(reader, "known_move_id"))
                            {
                                EvoMove evoDetail = new EvoMove();
                                (string, SkillData) skill = SkillInfo.GetSkillData(Convert.ToInt32(move));
                                evoDetail.MoveNum = skill.Item1;
                                branch.Details.Add(evoDetail);
                            }
                            else if (CheckEvoConditions(reader, "min_affection", "known_move_type_id"))
                            {
                                //EvoItem evoDetail = new EvoItem();
                                //evoDetail.ItemNum = "held_pink_bow";//Pink Bow
                                //branch.Details.Add(evoDetail);

                                EvoMoveElement evoDetail2 = new EvoMoveElement();
                                evoDetail2.MoveElement = MapElement(Convert.ToInt32(moveType));
                                branch.Details.Add(evoDetail2);
                            }
                            else if (CheckEvoConditions(reader, "min_beauty"))
                            {
                                EvoItem evoDetail = new EvoItem();
                                evoDetail.ItemNum = "evo_prism_scale";//prism scale
                                branch.Details.Add(evoDetail);
                            }
                            else if (CheckEvoConditions(reader, "min_level", "relative_physical_stats"))
                            {
                                EvoLevel evoDetail = new EvoLevel();
                                evoDetail.Level = Convert.ToInt32(level);
                                branch.Details.Add(evoDetail);

                                EvoStats evoDetail2 = new EvoStats();
                                evoDetail2.AtkDefComparison = Convert.ToInt32(stats);
                                branch.Details.Add(evoDetail2);
                            }
                            else if (CheckEvoConditions(reader, "party_species_id"))
                            {
                                EvoPartner evoDetail = new EvoPartner();
                                evoDetail.Species = monsterKeys[Convert.ToInt32(allySpecies)];
                                branch.Details.Add(evoDetail);
                            }
                            else if (CheckEvoConditions(reader, "min_level", "party_type_id"))
                            {
                                EvoPartnerElement evoDetail = new EvoPartnerElement();
                                evoDetail.PartnerElement = MapElement(Convert.ToInt32(allyType));
                                branch.Details.Add(evoDetail);
                            }
                            else if (CheckEvoConditions(reader, "min_level", "needs_overworld_rain"))
                            {
                                EvoWeather evoDetail = new EvoWeather();
                                evoDetail.Weather = "rain";//rain
                                branch.Details.Add(evoDetail);
                            }
                            else if (CheckEvoConditions(reader, "min_level", "turn_upside_down"))
                            {
                                EvoWeather evoDetail = new EvoWeather();
                                evoDetail.Weather = "trick_room";//Trick Room
                                branch.Details.Add(evoDetail);
                            }
                            else
                                throw new Exception(entry.Name + ": Unknown Level Evo method");
                        }
                        else if (evoMethod == 2)
                        {
                            if (CheckEvoConditions(reader))
                            {
                                EvoItem evoDetail = new EvoItem();
                                evoDetail.ItemNum = "evo_link_cable";//Link Cable
                                branch.Details.Add(evoDetail);
                            }
                            else if (CheckEvoConditions(reader, "held_item_id"))
                            {
                                EvoItem evoDetail = new EvoItem();
                                evoDetail.ItemNum = MapItem(Convert.ToInt32(heldItem));
                                if (String.IsNullOrEmpty(evoDetail.ItemNum))
                                {
                                    evoDetail = new EvoItem();
                                    evoDetail.ItemNum = "evo_link_cable";//Link Cable
                                    branch.Details.Add(evoDetail);
                                }
                                else
                                    branch.Details.Add(evoDetail);
                            }
                            else if (CheckEvoConditions(reader, "trade_species_id"))
                            {
                                EvoPartner evoDetail = new EvoPartner();
                                evoDetail.Species = monsterKeys[Convert.ToInt32(tradeSpecies)];
                                branch.Details.Add(evoDetail);
                            }
                            else
                                throw new Exception(entry.Name + ": Unknown Trade Evo method");
                        }
                        else if (evoMethod == 3)
                        {
                            if (CheckEvoConditions(reader, "evolution_item_id"))
                            {
                                EvoItem evoDetail = new EvoItem();
                                evoDetail.ItemNum = MapItem(Convert.ToInt32(itemNum));
                                if (String.IsNullOrEmpty(evoDetail.ItemNum))
                                    throw new Exception(entry.Name + ": Unknown Item Evo");
                                branch.Details.Add(evoDetail);
                            }
                            else if (CheckEvoConditions(reader, "evolution_item_id", "gender_id"))
                            {
                                EvoItem evoDetail = new EvoItem();
                                evoDetail.ItemNum = MapItem(Convert.ToInt32(itemNum));
                                if (String.IsNullOrEmpty(evoDetail.ItemNum))
                                    throw new Exception(entry.Name + ": Unknown Item Evo");
                                branch.Details.Add(evoDetail);

                                EvoGender evoDetail2 = new EvoGender();
                                evoDetail2.ReqGender = MapGender(Convert.ToInt32(gender));
                                branch.Details.Add(evoDetail2);
                            }
                            else
                                throw new Exception(entry.Name + ": Unknown Item Evo method");
                        }
                        else
                        {
                            throw new Exception(entry.Name + ": Unknown Evo method");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    //manually account for evo formes
                    if (evoSpecies == 291)//ninjask
                    {
                        EvoShed evoDetail = new EvoShed();
                        evoDetail.ShedSpecies = monsterKeys[292];
                        branch.Details.Add(evoDetail);
                    }
                    else if (evoSpecies == 414)//mothim
                        branch.Details.Add(new EvoSetForm(0));
                    else if (evoSpecies == 666)//vivillon
                        branch.Details.Add(new EvoFormLocOrigin());
                    else if (evoSpecies == 678)//meowstic
                    {
                        EvoSetForm evoDetail = new EvoSetForm(1);
                        evoDetail.Conditions.Add(new EvoGender(Gender.Female));
                        branch.Details.Add(evoDetail);
                    }
                    else if (evoSpecies == 849)//toxtricity
                    {
                        EvoSetForm evoDetail = new EvoSetForm(1);
                        EvoPersonality personality = new EvoPersonality();
                        personality.Divisor = 2;
                        evoDetail.Conditions.Add(personality);
                        branch.Details.Add(evoDetail);
                    }

                    //FIXME
                    //manually account for regional requirements (regional -> new)
                    switch (evoSpecies)
                    {
                        case 863://perrserker
                            {
                                //second alt form required
                                branch.Details.Insert(0, new EvoForm(2));
                                branch.Details.Add(new EvoSetForm(0));
                            }
                            break;
                        case 862://obstagoon
                        case 864://cursola
                        case 865://sirfetch'd
                        case 866://Mr. Rime
                        case 867://Runerigus
                            {
                                //first alt form required
                                branch.Details.Insert(0, new EvoForm(1));
                                branch.Details.Add(new EvoSetForm(0));
                            }
                            break;
                        case 563://cofagrigus
                            {
                                //original form required due to having a regional -> new that requires a different form
                                branch.Details.Insert(0, new EvoForm(0));
                            }
                            break;
                        case 903://sneasler
                            {
                                //first alt form required
                                branch.Details.Insert(0, new EvoForm(1));
                                branch.Details.Add(new EvoSetForm(0));
                            }
                            break;
                        case 902://basculegion
                            {
                                //second alt form required
                                branch.Details.Insert(0, new EvoForm(2));
                                {
                                    EvoSetForm evoDetail = new EvoSetForm(0);
                                    evoDetail.Conditions.Add(new EvoGender(Gender.Male));
                                    branch.Details.Add(evoDetail);
                                }
                                {
                                    EvoSetForm evoDetail = new EvoSetForm(1);
                                    evoDetail.Conditions.Add(new EvoGender(Gender.Female));
                                    branch.Details.Add(evoDetail);
                                }
                            }
                            break;
                        case 461://weavile
                            {
                                //original form required due to having a regional -> new that requires a different form
                                branch.Details.Insert(0, new EvoForm(0));
                            }
                            break;
                        case 904://overqwil
                            {
                                //first alt form required
                                branch.Details.Insert(0, new EvoForm(1));
                                branch.Details.Add(new EvoSetForm(0));
                            }
                            break;
                    }


                    //FIXME
                    //manually add regional forms (normal -> regional)
                    switch (evoSpecies)
                    {
                        //alola
                        case 26://raichu
                        case 103://exeggutor
                        case 105://marowak
                            {
                                EvoSetForm evoDetail = new EvoSetForm(1);
                                evoDetail.Conditions.Add(new EvoLocation(Text.Sanitize(ElementInfo.Element.Water.ToString()).ToLower()));
                                branch.Details.Add(evoDetail);
                            }
                            break;
                        //galar
                        case 110://weezing
                        case 122://mr. mime
                            {
                                EvoSetForm evoDetail = new EvoSetForm(1);
                                evoDetail.Conditions.Add(new EvoLocation(Text.Sanitize(ElementInfo.Element.Fairy.ToString()).ToLower()));
                                branch.Details.Add(evoDetail);
                            }
                            break;
                        //hisui
                        case 157://typhlosion
                        case 503://samurott
                        case 628://braviary
                        case 705://sliggoo
                        case 713://avalugg
                        case 724://decidueye
                            {
                                EvoSetForm evoDetail = new EvoSetForm(1);
                                evoDetail.Conditions.Add(new EvoLocation(Text.Sanitize(ElementInfo.Element.Ice.ToString()).ToLower()));
                                branch.Details.Add(evoDetail);
                            }
                            break;
                    }

                    entry.Promotions.Add(branch);

                    //FIXME
                    //manually add regional forms (regional -> regional)
                    if (evoSpecies == 20)//raticate
                    {
                        branch.Details.Insert(0, new EvoForm(0));
                        PromoteBranch altBranch = new PromoteBranch();
                        altBranch.Result = branch.Result;
                        altBranch.Details.Add(new EvoForm(1));

                        EvoLevel evoDetail = new EvoLevel();
                        evoDetail.Level = Convert.ToInt32(20);
                        altBranch.Details.Add(evoDetail);

                        EvoItem evoDetail2 = new EvoItem();
                        evoDetail2.ItemNum = "evo_lunar_ribbon";//moon ribbon
                        altBranch.Details.Add(evoDetail2);
                        entry.Promotions.Add(altBranch);
                    }
                    else if (evoSpecies == 28)//sandslash
                    {
                        branch.Details.Insert(0, new EvoForm(0));
                        PromoteBranch altBranch = new PromoteBranch();
                        altBranch.Result = branch.Result;
                        altBranch.Details.Add(new EvoForm(1));

                        EvoItem evoDetail = new EvoItem();
                        evoDetail.ItemNum = "evo_ice_stone";//ice stone
                        altBranch.Details.Add(evoDetail);
                        entry.Promotions.Add(altBranch);
                    }
                    else if (evoSpecies == 38)//ninetales
                    {
                        branch.Details.Insert(0, new EvoForm(0));
                        PromoteBranch altBranch = new PromoteBranch();
                        altBranch.Result = branch.Result;
                        altBranch.Details.Add(new EvoForm(1));

                        EvoItem evoDetail = new EvoItem();
                        evoDetail.ItemNum = "evo_ice_stone";//ice stone
                        altBranch.Details.Add(evoDetail);
                        entry.Promotions.Add(altBranch);
                    }
                    else if (evoSpecies == 51)//dugtrio
                    {
                        branch.Details.Insert(0, new EvoForm(0));
                        PromoteBranch altBranch = new PromoteBranch();
                        altBranch.Result = branch.Result;
                        altBranch.Details.Add(new EvoForm(1));

                        EvoLevel evoDetail = new EvoLevel();
                        evoDetail.Level = Convert.ToInt32(26);
                        altBranch.Details.Add(evoDetail);

                        entry.Promotions.Add(altBranch);
                    }
                    else if (evoSpecies == 53)//persian
                    {
                        branch.Details.Insert(0, new EvoForm(0));
                        PromoteBranch altBranch = new PromoteBranch();
                        altBranch.Result = branch.Result;
                        altBranch.Details.Add(new EvoForm(1));

                        EvoFriendship evoDetail = new EvoFriendship();
                        altBranch.Details.Add(evoDetail);

                        entry.Promotions.Add(altBranch);
                    }
                    else if (evoSpecies == 75)//graveler
                    {
                        branch.Details.Insert(0, new EvoForm(0));
                        PromoteBranch altBranch = new PromoteBranch();
                        altBranch.Result = branch.Result;
                        altBranch.Details.Add(new EvoForm(1));

                        EvoLevel evoDetail = new EvoLevel();
                        evoDetail.Level = Convert.ToInt32(25);
                        altBranch.Details.Add(evoDetail);

                        entry.Promotions.Add(altBranch);
                    }
                    else if (evoSpecies == 76)//golem
                    {
                        branch.Details.Insert(0, new EvoForm(0));
                        PromoteBranch altBranch = new PromoteBranch();
                        altBranch.Result = branch.Result;
                        altBranch.Details.Add(new EvoForm(1));

                        EvoItem evoDetail = new EvoItem();
                        evoDetail.ItemNum = "evo_link_cable";//Link Cable
                        altBranch.Details.Add(evoDetail);

                        entry.Promotions.Add(altBranch);
                    }
                    else if (evoSpecies == 78)//rapidash
                    {
                        branch.Details.Insert(0, new EvoForm(0));
                        PromoteBranch altBranch = new PromoteBranch();
                        altBranch.Result = branch.Result;
                        altBranch.Details.Add(new EvoForm(1));

                        EvoLevel evoDetail = new EvoLevel();
                        evoDetail.Level = Convert.ToInt32(40);
                        altBranch.Details.Add(evoDetail);

                        entry.Promotions.Add(altBranch);
                    }
                    else if (evoSpecies == 80)//slowbro
                    {
                        branch.Details.Insert(0, new EvoForm(0));
                        PromoteBranch altBranch = new PromoteBranch();
                        altBranch.Result = branch.Result;
                        altBranch.Details.Add(new EvoForm(1));

                        EvoLevel evoDetail = new EvoLevel();
                        evoDetail.Level = Convert.ToInt32(38);
                        altBranch.Details.Add(evoDetail);

                        entry.Promotions.Add(altBranch);
                    }
                    else if (evoSpecies == 89)//muk
                    {
                        branch.Details.Insert(0, new EvoForm(0));
                        PromoteBranch altBranch = new PromoteBranch();
                        altBranch.Result = branch.Result;
                        altBranch.Details.Add(new EvoForm(1));

                        EvoLevel evoDetail = new EvoLevel();
                        evoDetail.Level = Convert.ToInt32(38);
                        altBranch.Details.Add(evoDetail);

                        entry.Promotions.Add(altBranch);
                    }
                    else if (evoSpecies == 199)//slowking
                    {
                        branch.Details.Insert(0, new EvoForm(0));
                        PromoteBranch altBranch = new PromoteBranch();
                        altBranch.Result = branch.Result;
                        altBranch.Details.Add(new EvoForm(1));

                        EvoItem evoDetail = new EvoItem();
                        evoDetail.ItemNum = "evo_kings_rock";//king's rock
                        altBranch.Details.Add(evoDetail);

                        entry.Promotions.Add(altBranch);
                    }
                    else if (evoSpecies == 264)//linoone
                    {
                        branch.Details.Insert(0, new EvoForm(0));
                        PromoteBranch altBranch = new PromoteBranch();
                        altBranch.Result = branch.Result;
                        altBranch.Details.Add(new EvoForm(1));

                        EvoLevel evoDetail = new EvoLevel();
                        evoDetail.Level = Convert.ToInt32(20);
                        altBranch.Details.Add(evoDetail);

                        entry.Promotions.Add(altBranch);
                    }
                    else if (evoSpecies == 555)//darmanitan
                    {
                        branch.Details.Insert(0, new EvoForm(0));
                        PromoteBranch altBranch = new PromoteBranch();
                        altBranch.Result = branch.Result;
                        altBranch.Details.Add(new EvoForm(1));
                        altBranch.Details.Add(new EvoSetForm(2));

                        EvoLevel evoDetail = new EvoLevel();
                        evoDetail.Level = Convert.ToInt32(35);
                        altBranch.Details.Add(evoDetail);

                        entry.Promotions.Add(altBranch);
                    }

                    //and polteageist...
                    if (evoSpecies == 855)
                    {
                        branch.Details.Insert(0, new EvoForm(0));
                        PromoteBranch altBranch = new PromoteBranch();
                        altBranch.Result = branch.Result;
                        altBranch.Details.Add(new EvoForm(1));

                        EvoItem evoDetail = new EvoItem();
                        evoDetail.ItemNum = "evo_chipped_pot";//Chipped Pot
                        altBranch.Details.Add(evoDetail);

                        entry.Promotions.Add(altBranch);
                    }

                    read++;
                }
            }


            if (index == 808)//meltan -> melmetal
            {
                //FIXME
                PromoteBranch branch = new PromoteBranch();
                branch.Result = monsterKeys[809];

                EvoItem evoDetail = new EvoItem();
                evoDetail.ItemNum = "evo_link_cable";//Link Cable
                branch.Details.Add(evoDetail);
                entry.Promotions.Add(branch);
            }

            //form data
            sql = "SELECT a.id AS formId, b.id AS dexId, a.version_group_id, a.is_battle_only FROM pokemon_v2_pokemonform a, pokemon_v2_pokemon b WHERE a.pokemon_id = b.id AND b.pokemon_species_id = " + index + " ORDER BY a.pokemon_id, a.form_order, a.id";
            command = new SQLiteCommand(sql, m_dbTLConnection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                int read = 0;
                while (reader.Read())
                {
                    read++;
                    if (index == 20 && read > 2)//only 2 raticate form
                        break;
                    if (index == 25 && read > 1 && read < 17)//only 2 pikachu forms: normal and gigantamax
                        continue;
                    if (index == 105 && read > 2)//only 2 marowak form - somehow the db has a duplicate marowak after alolan????
                        break;
                    if (index == 133 && read > 1)//only 1 eevee form
                        break;
                    if (index == 172 && read > 1)//only 1 pichu form
                        break;
                    if (index == 414 && read > 1)//only 1 mothim form
                        break;
                    if (index == 658 && read > 1)//skip battle bond greninja/ash greninja
                        break;
                    if ((index == 664 || index == 665) && read > 1)//only 1 scatterbug/spewpa form
                        break;
                    if (index == 735 && read > 1)//only 1 gumshoos form
                        break;
                    if (index == 738 && read > 1)//only 1 vikavolt form
                        break;
                    if (index == 743 && read > 1)//only 1 ribombee form
                        break;
                    if (index == 744 && read > 1)//only 1 rockruff form
                        break;
                    if (index == 752 && read > 1)//only 1 araquanid form
                        break;
                    if (index == 754 && read > 1)//only 1 lurantis form
                        break;
                    if (index == 758 && read > 1)//only 1 salazzle form
                        break;
                    if (index == 777 && read > 1)//only 1 togedemaru form
                        break;
                    if (index == 778 && read > 2)//only 2 mimikyu forms
                        break;
                    if (index == 784 && read > 1)//only 1 kommo-o form
                        break;
                    if (index == 893 && read > 1)//only 1 zarude form
                        break;
                    if (index == 1007 && read > 1)//only 1 koraidon form
                        break;
                    if (index == 1008 && read > 1)//only 1 miraidon form
                        break;


                    int dexId = Convert.ToInt32(reader["dexId"].ToString());
                    int formId = Convert.ToInt32(reader["formId"].ToString());
                    bool battle_only = (bool)reader["is_battle_only"];
                    int version = Convert.ToInt32(reader["version_group_id"].ToString());
                    if (version < 16)
                        version = 16;
                    //These mons are from LEGENDS ARCEUS.  NOT SWORD AND SHIELD.
                    if (index >= 899 && index < 906)
                        version = 24;
                    // hisui forms don't have info for their original game...
                    if (version == 24)
                        version = 25;
                    MonsterFormData formEntry = LoadForme(m_dbTLConnection, version, index, dexId, formId, entry.Name);
                    formEntry.Generation = genVersion(version);
                    if (Ratio == -1)
                    {
                        formEntry.GenderlessWeight = 8;
                        if (GENDER_UNLOCK)
                        {
                            formEntry.MaleWeight = 1;
                            formEntry.FemaleWeight = 1;
                        }
                    }
                    else
                    {
                        formEntry.MaleWeight = 8 - Ratio;
                        formEntry.FemaleWeight = Ratio;
                        if (GENDER_UNLOCK)
                            formEntry.GenderlessWeight = 2;
                    }
                    formEntry.Temporary = battle_only;

                    if (index == 678)//meowstic; forme has fixed gender ratio
                    {
                        if (entry.Forms.Count == 0)//male
                        {
                            formEntry.MaleWeight = 8;
                            formEntry.FemaleWeight = 0;
                            formEntry.GenderlessWeight = 0;
                        }
                        else
                        {
                            formEntry.MaleWeight = 0;
                            formEntry.FemaleWeight = 8;
                            formEntry.GenderlessWeight = 0;
                        }
                    }
                    //special-case in-battle only formes
                    //arceus
                    if (index == 493 && entry.Forms.Count > 0)
                        formEntry.Temporary = true;
                    //genesect
                    if (index == 649 && entry.Forms.Count > 0)
                        formEntry.Temporary = true;
                    //zygarde
                    if (index == 718 && entry.Forms.Count > 1)
                        formEntry.Temporary = true;
                    //silvally
                    if (index == 773 && entry.Forms.Count > 0)
                        formEntry.Temporary = true;
                    //cramorant
                    if (index == 845 && entry.Forms.Count > 0)
                        formEntry.Temporary = true;
                    //noice eiscue
                    if (index == 875 && entry.Forms.Count > 0)
                        formEntry.Temporary = true;

                    formEntry.Released = hasFormeGraphics(index, entry.Forms.Count);

                    //vivillon meadow form should be considered standard
                    if (index == 666)
                    {
                        if (entry.Forms.Count == 6)
                            entry.Forms.Insert(0, formEntry);
                        else
                            entry.Forms.Add(formEntry);
                    }
                    else
                        entry.Forms.Add(formEntry);

                    //alcremie needs more forms to represent ALL appearances
                    if (index == 869)
                    {
                        for (int ii = 0; ii < 6; ii++)
                        {
                            MonsterFormData formCopy = formEntry.Copy();
                            entry.Forms.Add(formCopy);
                        }
                    }

                    if (index == 902)//basculegion; forme has fixed gender ratio
                    {
                        if (entry.Forms.Count == 0)//male
                        {
                            formEntry.MaleWeight = 8;
                            formEntry.FemaleWeight = 0;
                            formEntry.GenderlessWeight = 0;
                        }
                        else
                        {
                            formEntry.MaleWeight = 0;
                            formEntry.FemaleWeight = 8;
                            formEntry.GenderlessWeight = 0;
                        }
                    }

                    if (formEntry.Released)
                        entry.Released = true;

                }
            }

            return entry;
        }


        public static MonsterFormData LoadForme(SQLiteConnection m_dbTLConnection, int version, int index, int dexId, int formId, LocalText dexName)
        {
            MonsterFormData entry = new MonsterFormData();

            string sql = "SELECT * FROM pokemon_v2_pokemonformname WHERE pokemon_form_id = " + formId + " AND language_id = 9";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbTLConnection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                int read = 0;
                while (reader.Read())
                {
                    if (read > 0)
                        throw new Exception("#" + index + ": More than 1 Form result!?");
                    entry.FormName = new LocalText(reader["pokemon_name"].ToString());
                    read++;
                }
            }

            sql = "SELECT * FROM pokemon_v2_pokemonformname WHERE pokemon_form_id = " + formId + " AND language_id = 9";
            command = new SQLiteCommand(sql, m_dbTLConnection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                int read = 0;
                while (reader.Read())
                {
                    if (read > 0)
                        throw new Exception("#" + index + ": More than 1 Form result!?");
                    if (entry.FormName.DefaultText != reader["pokemon_name"].ToString())
                        throw new Exception("#" + index + ": " + entry.FormName.DefaultText + " != " + reader["pokemon_name"].ToString());
                    read++;
                }
            }

            foreach (int key in langIDs.Keys)
            {
                sql = "SELECT * FROM pokemon_v2_pokemonformname WHERE pokemon_form_id = " + formId + " AND language_id = " + key;
                command = new SQLiteCommand(sql, m_dbTLConnection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    int read = 0;
                    while (reader.Read())
                    {
                        if (read > 0)
                            throw new Exception("#" + index + ": More than 1 Form result!?");
                        entry.FormName.LocalTexts[langIDs[key]] = reader["pokemon_name"].ToString();
                        read++;
                    }
                }
            }

            if (entry.FormName.DefaultText == "")
                entry.FormName = new LocalText(dexName);

            sql = "SELECT * FROM pokemon_v2_pokemontype WHERE pokemon_id = " + dexId;
            command = new SQLiteCommand(sql, m_dbTLConnection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                entry.Element1 = Text.Sanitize(Text.GetMemberTitle(ElementInfo.Element.None.ToString())).ToLower();
                entry.Element2 = Text.Sanitize(Text.GetMemberTitle(ElementInfo.Element.None.ToString())).ToLower();
                int read = 0;
                while (reader.Read())
                {
                    if (read > 1)
                        throw new Exception(entry.FormName + ": More than 2 type results!?");
                    int slot = Convert.ToInt32(reader["slot"].ToString());
                    if (slot == 1)
                        entry.Element1 = MapElement(Convert.ToInt32(reader["type_id"].ToString()));
                    else if (slot == 2)
                        entry.Element2 = MapElement(Convert.ToInt32(reader["type_id"].ToString()));
                    read++;
                }
            }

            sql = "SELECT * FROM pokemon_v2_pokemonability WHERE pokemon_id = " + dexId;
            command = new SQLiteCommand(sql, m_dbTLConnection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                entry.Intrinsic1 = "none";
                entry.Intrinsic2 = "none";
                entry.Intrinsic3 = "none";

                int read = 0;
                while (reader.Read())
                {
                    if (read > 2)
                        throw new Exception(entry.FormName + ": More than 3 ability results!?");
                    int slot = Convert.ToInt32(reader["slot"].ToString());
                    int ability_idx = Convert.ToInt32(reader["ability_id"].ToString());
                    (string, IntrinsicData) intrinsic = IntrinsicInfo.GetIntrinsicData(ability_idx);
                    if (slot == 1)
                        entry.Intrinsic1 = intrinsic.Item1;
                    else if (slot == 2)
                        entry.Intrinsic2 = intrinsic.Item1;
                    else if (slot == 3)
                        entry.Intrinsic3 = intrinsic.Item1;
                    read++;
                }
            }

            //stats
            sql = "SELECT * FROM pokemon_v2_pokemonstat WHERE pokemon_id = " + dexId;
            command = new SQLiteCommand(sql, m_dbTLConnection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                int read = 0;
                while (reader.Read())
                {
                    if (read > 5)
                        throw new Exception(entry.FormName + ": More than 6 stat results!?");
                    int slot = Convert.ToInt32(reader["stat_id"].ToString());
                    if (slot == 1)
                        entry.BaseHP = Convert.ToInt32(reader["base_stat"].ToString());
                    else if (slot == 2)
                        entry.BaseAtk = Convert.ToInt32(reader["base_stat"].ToString());
                    else if (slot == 3)
                        entry.BaseDef = Convert.ToInt32(reader["base_stat"].ToString());
                    else if (slot == 4)
                        entry.BaseMAtk = Convert.ToInt32(reader["base_stat"].ToString());
                    else if (slot == 5)
                        entry.BaseMDef = Convert.ToInt32(reader["base_stat"].ToString());
                    else if (slot == 6)
                        entry.BaseSpeed = Convert.ToInt32(reader["base_stat"].ToString());
                    read++;
                }
            }

            //height
            //weight
            //exp
            sql = "SELECT * FROM pokemon_v2_pokemon WHERE id = " + dexId;
            command = new SQLiteCommand(sql, m_dbTLConnection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                int read = 0;
                while (reader.Read())
                {
                    if (read > 0)
                        throw new Exception(entry.FormName + ": More than 1 statistic result!?");
                    entry.Height = Convert.ToDouble(reader["height"].ToString()) / 10;
                    entry.Weight = Convert.ToDouble(reader["weight"].ToString()) / 10;
                    string base_exp_str = reader["base_experience"].ToString();
                    if (base_exp_str == "")
                    {
                        Console.WriteLine("WARNING: STUBBED EXP YIELD");
                        entry.ExpYield = (entry.BaseHP + entry.BaseAtk + entry.BaseDef + entry.BaseMAtk + entry.BaseMDef + entry.BaseSpeed) / 4;
                    }
                    else
                        entry.ExpYield = Convert.ToInt32(base_exp_str);

                    //special case
                    if (index == 113) //chansey
                        entry.ExpYield = 250;
                    else if (index == 242) // blissey
                        entry.ExpYield = 255;
                    else if (index == 531) // audino
                        entry.ExpYield = 500;
                    read++;
                }
            }

            //level moves
            sql = "SELECT * FROM pokemon_v2_pokemonmove WHERE pokemon_id = " + dexId + " AND version_group_id = "+ version + " AND move_learn_method_id = 1 ORDER BY [level], [order]";
            command = new SQLiteCommand(sql, m_dbTLConnection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    LevelUpSkill levelMove = new LevelUpSkill();
                    levelMove.Level = Convert.ToInt32(reader["level"].ToString());
                    
                    (string, SkillData) skill = SkillInfo.GetSkillData(Convert.ToInt32(reader["move_id"].ToString()));
                    levelMove.Skill = skill.Item1;

                    entry.LevelSkills.Add(levelMove);
                }
            }


            //other moves
            sql = "SELECT * FROM pokemon_v2_pokemonmove WHERE pokemon_id = " + dexId + " AND version_group_id > 4 AND version_group_id <= "+ version + " AND move_learn_method_id > 1";
            command = new SQLiteCommand(sql, m_dbTLConnection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int method = Convert.ToInt32(reader["move_learn_method_id"].ToString());
                    int move = Convert.ToInt32(reader["move_id"].ToString());
                    (string, SkillData) skill = SkillInfo.GetSkillData(move);
                    if (method == 2)//egg
                    {
                        if (!entry.SharedSkills.Contains(new LearnableSkill(skill.Item1)))
                            entry.SharedSkills.Add(new LearnableSkill(skill.Item1));
                    }
                    else if (method == 3)//tutor
                    {
                        if (!entry.SecretSkills.Contains(new LearnableSkill(skill.Item1)))
                            entry.SecretSkills.Add(new LearnableSkill(skill.Item1));
                    }
                    else if (method == 4)//TM
                    {
                        if (!entry.TeachSkills.Contains(new LearnableSkill(skill.Item1)))
                            entry.TeachSkills.Add(new LearnableSkill(skill.Item1));
                    }
                    else//event
                    {
                        if (!entry.SecretSkills.Contains(new LearnableSkill(skill.Item1)))
                            entry.SecretSkills.Add(new LearnableSkill(skill.Item1));
                    }
                }
            }


            return entry;
        }

        public static bool CheckEvoConditions(SQLiteDataReader reader, params string[] args)
        {
            if (!CheckEvoCondition(reader, "evolution_item_id", false, args)) return false;
            if (!CheckEvoCondition(reader, "min_level", false, args)) return false;
            if (!CheckEvoCondition(reader, "gender_id", false, args)) return false;
            if (!CheckEvoCondition(reader, "location_id", false, args)) return false;
            if (!CheckEvoCondition(reader, "held_item_id", false, args)) return false;
            if (!CheckEvoCondition(reader, "time_of_day", false, args)) return false;
            if (!CheckEvoCondition(reader, "known_move_id", false, args)) return false;
            if (!CheckEvoCondition(reader, "known_move_type_id", false, args)) return false;
            if (!CheckEvoCondition(reader, "min_happiness", false, args)) return false;
            if (!CheckEvoCondition(reader, "min_beauty", false, args)) return false;
            if (!CheckEvoCondition(reader, "min_affection", false, args)) return false;
            if (!CheckEvoCondition(reader, "relative_physical_stats", false, args)) return false;
            if (!CheckEvoCondition(reader, "party_species_id", false, args)) return false;
            if (!CheckEvoCondition(reader, "party_type_id", false, args)) return false;
            if (!CheckEvoCondition(reader, "trade_species_id", false, args)) return false;
            if (!CheckEvoCondition(reader, "needs_overworld_rain", true, args)) return false;
            if (!CheckEvoCondition(reader, "turn_upside_down", true, args)) return false;

            return true;
        }

        public static bool CheckEvoCondition(SQLiteDataReader reader, string col, bool intCheck, params string[] args)
        {
            if (intCheck)
                return ((bool)reader[col] == args.Contains(col));
            else
                return (String.IsNullOrEmpty(reader[col].ToString()) != args.Contains(col));
        }

        public static string MapItem(int itemNum)
        {
            switch (itemNum)
            {
                case 80: return "evo_sun_stone";//sun-stone
                case 81: return "evo_moon_stone";//moon-stone
                case 82: return "evo_fire_stone";//fire-stone
                case 83: return "evo_thunder_stone";//thunder-stone
                case 84: return "evo_water_stone";//water-stone
                case 85: return "evo_leaf_stone";//leaf-stone
                case 107: return "evo_shiny_stone";//shiny-stone
                case 108: return "evo_dusk_stone";//dusk-stone
                case 109: return "evo_dawn_stone";//dawn-stone
                case 198: return "evo_kings_rock";//kings-rock
                case 210: return "held_metal_coat";//metal-coat
                case 212: return "held_dragon_scale";//dragon-scale
                case 298: return "evo_protector";//protector
                case 299: return "evo_electirizer";//electirizer
                case 300: return "evo_magmarizer";//magmarizer
                case 302: return "evo_reaper_cloth";//reaper-cloth
                case 580: return "evo_prism_scale";//prism-scale
                case 885: return "evo_ice_stone";//ice stone
                case 301: //dubious-disc
                case 110: //oval-stone
                case 203: //deep-sea-tooth
                case 204: //deep-sea-scale
                case 229: //up-grade
                case 303: //razor-claw
                case 304: //razor-fang
                case 686: //whipped-dream
                case 687: //sachet
                case 2045: //auspicious armor
                case 2046: //malicious armor
                    return "";
            }
            throw new Exception("No item mappable");
        }

        public static Gender MapGender(int gender)
        {
            switch (gender)
            {
                case 1: return Gender.Female;
                case 2: return Gender.Male;
                case 3: return Gender.Genderless;
            }
            throw new Exception("No gender mappable");
        }

        public static DexColor MapColorStyle(int color)
        {
            if (color > 0 && color < 11)
            {
                return (DexColor)(color - 1);
            }

            throw new Exception("No color style mappable");
        }

        public static BodyShape MapBodyStyle(int body)
        {
            if (body > 0 && body < 15)
            {
                return (BodyShape)(body - 1);
            }

            throw new Exception("No body style mappable");
        }


        public static GrowthInfo.GrowthGroup MapGrowthGroup(int growth)
        {
            switch (growth)
            {
                case 1: return GrowthInfo.GrowthGroup.Slow;
                case 2: return GrowthInfo.GrowthGroup.MediumFast;
                case 3: return GrowthInfo.GrowthGroup.Fast;
                case 4: return GrowthInfo.GrowthGroup.MediumSlow;
                case 5: return GrowthInfo.GrowthGroup.Erratic;
                case 6: return GrowthInfo.GrowthGroup.Fluctuating;
            }
            throw new Exception("No growth group mappable");
        }

        public static SkillGroupInfo.EggGroup MapEggGroup(int group)
        {
            switch (group)
            {
                case 1: return SkillGroupInfo.EggGroup.Monster;
                case 2: return SkillGroupInfo.EggGroup.Water1;
                case 3: return SkillGroupInfo.EggGroup.Bug;
                case 4: return SkillGroupInfo.EggGroup.Flying;
                case 5: return SkillGroupInfo.EggGroup.Field;
                case 6: return SkillGroupInfo.EggGroup.Fairy;
                case 7: return SkillGroupInfo.EggGroup.Grass;
                case 8: return SkillGroupInfo.EggGroup.Humanlike;
                case 9: return SkillGroupInfo.EggGroup.Water3;
                case 10: return SkillGroupInfo.EggGroup.Mineral;
                case 11: return SkillGroupInfo.EggGroup.Amorphous;
                case 12: return SkillGroupInfo.EggGroup.Water2;
                case 13: return SkillGroupInfo.EggGroup.Ditto;
                case 14: return SkillGroupInfo.EggGroup.Dragon;
                case 15: return SkillGroupInfo.EggGroup.Undiscovered;
            }
            throw new Exception("No egg group mappable");
        }

        public static BattleData.SkillCategory MapCategory(int category)
        {
            switch (category)
            {
                case 1: return BattleData.SkillCategory.Status;
                case 2: return BattleData.SkillCategory.Physical;
                case 3: return BattleData.SkillCategory.Magical;
            }
            throw new Exception("No Type mappable");
        }

        public static string MapElement(int element)
        {
            ElementInfo.Element elementEnum;
            switch (element)
            {
                case 1: elementEnum = ElementInfo.Element.Normal; break;
                case 2: elementEnum = ElementInfo.Element.Fighting; break;
                case 3: elementEnum = ElementInfo.Element.Flying; break;
                case 4: elementEnum = ElementInfo.Element.Poison; break;
                case 5: elementEnum = ElementInfo.Element.Ground; break;
                case 6: elementEnum = ElementInfo.Element.Rock; break;
                case 7: elementEnum = ElementInfo.Element.Bug; break;
                case 8: elementEnum = ElementInfo.Element.Ghost; break;
                case 9: elementEnum = ElementInfo.Element.Steel; break;
                case 10: elementEnum = ElementInfo.Element.Fire; break;
                case 11: elementEnum = ElementInfo.Element.Water; break;
                case 12: elementEnum = ElementInfo.Element.Grass; break;
                case 13: elementEnum = ElementInfo.Element.Electric; break;
                case 14: elementEnum = ElementInfo.Element.Psychic; break;
                case 15: elementEnum = ElementInfo.Element.Ice; break;
                case 16: elementEnum = ElementInfo.Element.Dragon; break;
                case 17: elementEnum = ElementInfo.Element.Dark; break;
                case 18: elementEnum = ElementInfo.Element.Fairy; break;
                default:
                    throw new Exception("No Type mappable");
            }

            return Text.Sanitize(elementEnum.ToString()).ToLower();
        }

        public static int MapJoinRate(int dexIndex, MonsterData[] totalEntries)
        {
            //special cases
            switch (dexIndex)
            {
                //eeveelutions
                case 134:
                case 135:
                case 136:
                case 196:
                case 197:
                case 470:
                case 471:
                case 700:
                    return -60;
                //pseudolegend stage 1
                case 147:
                case 246:
                case 371:
                case 374:
                case 443:
                case 633:
                case 704:
                case 782:
                case 885:
                    return -5;
                //pseudolegend stage 2
                case 148:
                case 247:
                case 372:
                case 375:
                case 444:
                case 634:
                case 705:
                case 783:
                case 886:
                    return -40;
                //pseudolegend final
                case 149:
                case 248:
                case 373:
                case 376:
                case 445:
                case 635:
                case 706:
                case 784:
                case 887:
                    return -100;
                //wandering legendaries
                case 243:
                case 244:
                case 245:
                    return -40;
                //wandering unobtainables
                case 251://celebi
                case 647://keldeo
                case 719://diancie
                    return -50;
                //UBs
                case 793:
                case 794:
                case 795:
                case 796:
                case 797:
                case 798:
                case 799:
                case 804:
                case 805:
                case 806:
                    return -120;
                case 803:
                    return -80;
                //heatran
                case 485:
                    return -60;

            }

            MonsterData entry = totalEntries[dexIndex];
            int evoCount = 0;
            foreach (PromoteBranch evo in entry.Promotions)
            {
                if (monsterKeys.IndexOf(evo.Result) < GetGenBoundary(dexIndex))
                    evoCount++;
            }
            int newRate = 0;
            if (evoCount == 0)//...if it doesn't have any evos (don't count future gens)
            {
                //cannot evolve further
                if (String.IsNullOrEmpty(entry.PromoteFrom))//single stager -25% to 65%
                    newRate = entry.JoinRate * 90 / 255 - 25;
                else if (String.IsNullOrEmpty(totalEntries[monsterKeys.IndexOf(entry.PromoteFrom)].PromoteFrom))//second stage -40% to 20%
                    newRate = entry.JoinRate * 60 / 255 - 40;
                else//third stage: -65% to -15%
                    newRate = entry.JoinRate * 50 / 255 - 65;
            }
            else if (String.IsNullOrEmpty(entry.PromoteFrom))//first stage evolvable: 10% to 80%
            {
                //newRate = entry.JoinRate * 70 / 255 + 10;
                if (entry.JoinRate <= 200)//0-200 -> 10% to 50%
                    newRate = entry.JoinRate * 40 / 200 + 10;
                else//200-255 -> 50% to 80%
                    newRate = (entry.JoinRate - 200) * 30 / 55 + 50;
            }
            else //mid evos: -25% to 55%
                newRate = entry.JoinRate * 80 / 255 - 25;

            //leave a gap at -50--40%, -25--15%, 0-10%, 25-35%
            if (newRate < 50)
            {
                newRate += 100;
                if ((newRate / 5) % 5 == 0)//round down
                    newRate = newRate / 5 * 5;
                else if ((newRate / 5) % 5 == 1)//round up
                    newRate = (newRate / 5 + 1) * 5;
                newRate -= 100;
            }

            return newRate;
        }

        public static void MapFriendshipEvo(int preSpecies, MonsterData[] totalEntries)
        {
            MonsterData entry = totalEntries[preSpecies];

            foreach (PromoteBranch branch in entry.Promotions)
            {
                int evoSpecies = monsterKeys.IndexOf(branch.Result);
                EvoFriendship friendship = null;
                //bool hasTime = false;
                foreach (PromoteDetail detail in branch.Details)
                {
                    if (detail is EvoFriendship)
                        friendship = detail as EvoFriendship;
                    //if (detail is EvoTime)
                    //    hasTime = true;
                }

                if (friendship == null)
                    continue;

                if (!String.IsNullOrEmpty(entry.PromoteFrom))
                {
                    //this is an evo to the final stage of 3
                    friendship.Allies = 3;
                }
                else
                {
                    MonsterData evoEntry = totalEntries[evoSpecies];
                    if (evoEntry.Promotions.Count > 0)
                    {
                        //this is the first evo from the first stage of 3
                        friendship.Allies = 1;
                    }
                    else
                    {
                        //this is an evo from first to final stage
                        friendship.Allies = 2;
                    }
                }
            }

            if (entry.Promotions.Count == 0)//...if it doesn't have any evos (don't count future gens)
            {
                //cannot evolve further
                if (String.IsNullOrEmpty(entry.PromoteFrom))
                {
                    //impossible
                }
                else if (String.IsNullOrEmpty(totalEntries[monsterKeys.IndexOf(entry.PromoteFrom)].PromoteFrom))//second stage
                {

                }
                else//third stage
                {

                }
            }
            else if (String.IsNullOrEmpty(entry.PromoteFrom))//first stage evolvable: 10% to 80%
            {

            }
            else //mid evos: -25% to 55%
            {

            }

        }

        public static void MapFormEvo(int prevoSpecies, MonsterData[] totalEntries)
        {
            MonsterData prevoData = totalEntries[prevoSpecies];
            foreach (PromoteBranch branch in prevoData.Promotions)
            {
                MonsterData evoData = totalEntries[monsterKeys.IndexOf(branch.Result)];

                //by default, the evo evolves from the same form of the prevo, as long as they're both there.
                for(int ii = 0; ii < evoData.Forms.Count && ii < prevoData.Forms.Count; ii++)
                {
                    BaseMonsterForm form = evoData.Forms[ii];
                    form.PromoteForm = ii;
                }

                int reqForm = 0;
                foreach (PromoteDetail detail in branch.Details)
                {
                    if (detail is EvoForm)
                        reqForm = ((EvoForm)detail).ReqForm;
                }
                int resultForm = reqForm;
                foreach (PromoteDetail detail in branch.Details)
                {
                    if (detail is EvoSetForm)
                    {
                        EvoSetForm setForm = ((EvoSetForm)detail);
                        int innerForm = setForm.Form;
                        int innerReqForm = reqForm;
                        foreach (PromoteDetail innerReq in setForm.Conditions)
                        {
                            if (innerReq is EvoForm)
                                innerReqForm = ((EvoForm)innerReq).ReqForm;
                        }
                        BaseMonsterForm form = evoData.Forms[innerForm];
                        form.PromoteForm = innerReqForm;
                    }
                }
            }
        }

        public static int GetGenBoundary(int index)
        {
            if (index < 151)
                return 151;
            else if (index < 251)
                return 251;
            else if (index < 386)
                return 386;
            else if (index < 493)
                return 493;
            else if (index < 649)
                return 649;
            else
                return 721;
        }

        private static int genVersion(int version)
        {
            if (version <= 16)
                return 6;
            else if (version < 20)
                return 7;
            else if (version < 25)
                return 8;
            else
                return 9;
        }

        private static bool hasFormeGraphics(int species, int form)
        {
            CharaIndexNode charaIndex = GraphicsManager.LoadCharaIndices(GraphicsManager.CONTENT_PATH + "Chara/");

            if (form == 0)
                form = -1;
            CharID id = new CharID(species, form, -1, -1);
            return GraphicsManager.GetFallbackForm(charaIndex, id) == id;
        }

        private static string getAssetName(string fileName)
        {
            //nido special case
            fileName = fileName.Replace("♂", "_m").Replace("♀", "_f");
            fileName = Text.Sanitize(fileName).ToLower();
            return fileName;
        }

        const int TOTAL_MOVES = 901;

        public static int compareOldNew(string oldStr, string newStr)
        {
            if (oldStr == newStr)
                return 0;
            Console.WriteLine(oldStr);
            Console.WriteLine("vs.");
            Console.WriteLine(newStr);
            Console.WriteLine("Is new worse, equal, or better? 1/2/3");
            ConsoleKey key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.D1)
                return -1;
            if (key == ConsoleKey.D3)
                return 1;
            return 0;
        }

        public static string getTLStr(SQLiteConnection m_dbConnection, string sql, int langNum, string nameCol)
        {
            string oldStr = "";
            SQLiteCommand command = new SQLiteCommand(sql + langNum.ToString(), m_dbConnection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                int read = 0;
                while (reader.Read())
                {
                    if (read > 0)
                        throw new Exception(sql + ": More than 1 Index result!?");
                    oldStr = SingleSpace(reader[nameCol].ToString());
                    read++;
                }
            }
            return oldStr;
        }


        public static void assignRowElements(string[] nameRow, int ii, SQLiteConnection m_dbTLConnection, string newSql, string nameCol)
        {
            foreach (int key in existing)
                nameRow[langCols[key]] = getTLStr(m_dbTLConnection, newSql, key, nameCol);
            foreach (int key in straggler)
                nameRow[langCols[key]] = getTLStr(m_dbTLConnection, newSql, key, nameCol);
            foreach (int key in added)
                nameRow[langCols[key]] = getTLStr(m_dbTLConnection, newSql, key, nameCol);
        }


        public static void assignRowElements(string[] nameRow, int ii, SQLiteConnection m_dbConnection, SQLiteConnection m_dbTLConnection, string oldSql, string newSql, string nameCol)
        {
            string oldStr = getTLStr(m_dbConnection, oldSql, 9, nameCol);
            string newStr = getTLStr(m_dbTLConnection, newSql, 9, nameCol);

            int cmpOldNew = compareOldNew(oldStr, newStr);
            if (cmpOldNew == 1)
            {
                foreach (int key in existing)
                    nameRow[langCols[key]] = getTLStr(m_dbTLConnection, newSql, key, nameCol);
                foreach (int key in straggler)
                    nameRow[langCols[key]] = getTLStr(m_dbTLConnection, newSql, key, nameCol);
                foreach (int key in added)
                    nameRow[langCols[key]] = getTLStr(m_dbTLConnection, newSql, key, nameCol);
            }
            else if (cmpOldNew == -1)
            {
                foreach (int key in straggler)
                    nameRow[langCols[key]] = getTLStr(m_dbConnection, oldSql, key, nameCol);
                foreach (int key in added)
                    nameRow[langCols[key]] = getTLStr(m_dbTLConnection, newSql, key, nameCol) + "*";
            }
            else
            {
                foreach (int key in straggler)
                    nameRow[langCols[key]] = getTLStr(m_dbConnection, oldSql, key, nameCol);
                foreach (int key in added)
                    nameRow[langCols[key]] = getTLStr(m_dbTLConnection, newSql, key, nameCol);
            }
        }

        public static string SingleSpace(string inStr)
        {
            return inStr.Replace("\n", " ").Replace("  ", " ");
        }



        public static void CreateMoveCodes()
        {
            //load names and descriptions to the translation table
            //include the english column of the actual description to see if there's a difference.
            //just take it out after the initial setup
            //this is meant to be a one-time fill-in job.  all translations will go to their respective places, and then this should never be visited again
            //even when new blank descs are filled in, those blanks will not have a corresponding translation in the table, so running this script again will give useless translations
            //so when adding the new blank descs, the initial table generation will be run on the final translation table from the spreadsheet,
            //and that table will simply be put through the converter for code
            //alternatively, have this be the start of the stream
            //the downside is, if there are any english messages that sound better than the originals, they must be moved in manually.
            SQLiteConnection m_dbTLConnection = new SQLiteConnection("Data Source=" + TL_FILE + ";Version=3;");
            m_dbTLConnection.Open();

            using (StreamWriter file = new StreamWriter("skill.txt"))
            {
                for (int ii = 0; ii < TOTAL_MOVES; ii++)
                {
                    string name = getTLStr(m_dbTLConnection, "SELECT * FROM pokemon_v2_movename WHERE move_id = " + ii + " AND language_id = ", 9, "name");
                    string description = getTLStr(m_dbTLConnection, "SELECT * FROM pokemon_v2_moveflavortext WHERE move_id = " + ii + " AND version_group_id = "+ADD_VERSION_ID+" AND language_id = ", 9, "flavor_text");

                    int power = 0;
                    int pp = 0;
                    int accuracy = -1;
                    string typing = "none";
                    BattleData.SkillCategory category = BattleData.SkillCategory.None;

                    string sql = "SELECT * FROM pokemon_v2_move WHERE id = " + ii;
                    SQLiteCommand command = new SQLiteCommand(sql, m_dbTLConnection);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        int read = 0;
                        while (reader.Read())
                        {
                            if (read > 0)
                                throw new Exception(name + ": More than 1 statistic result!?");

                            if (!Int32.TryParse(reader["power"].ToString(), out power))
                                power = 0;
                            if (!Int32.TryParse(reader["pp"].ToString(), out pp))
                                pp = 0;
                            if (!Int32.TryParse(reader["accuracy"].ToString(), out accuracy))
                                accuracy = -1;
                            typing = MapElement(Convert.ToInt32(reader["type_id"].ToString()));
                            int category_int = 0;
                            if (Int32.TryParse(reader["move_damage_class_id"].ToString(), out category_int))
                                category = MapCategory(category_int);
                            read++;
                        }
                    }

                    file.WriteLine("else if (ii == " + ii + ")");
                    file.WriteLine("{");
                    file.WriteLine("	skill.Name = new LocalText(\"**" + name.Replace('’', '\'') + "\");");
                    file.WriteLine("	skill.Desc = new LocalText(\"" + description + "\");");
                    file.WriteLine("	skill.BaseCharges = " + pp + ";");
                    file.WriteLine("	skill.Data.Element = \"" + typing + "\";");
                    file.WriteLine("	skill.Data.Category = BattleData.SkillCategory." + category.ToString() + ";");
                    file.WriteLine("	skill.Data.HitRate = " + accuracy + ";");
                    if (category != BattleData.SkillCategory.Status)
                    {
                        file.WriteLine("	skill.Data.SkillStates.Set(new BasePowerState(" + power + "));");
                        file.WriteLine("	skill.Data.OnHits.Add(-1, new DamageFormulaEvent());");
                    }
                    file.WriteLine("	skill.Strikes = 1;");
                    file.WriteLine("	skill.HitboxAction = new AttackAction();");
                    file.WriteLine("	((AttackAction)skill.HitboxAction).CharAnimData = new CharAnimFrameType(05);//Attack");
                    file.WriteLine("	skill.HitboxAction.TargetAlignments = Alignment.Foe;");
                    file.WriteLine("	skill.Explosion.TargetAlignments = Alignment.Foe;");
                    file.WriteLine("}");

                    Console.WriteLine("#" + ii + " " + name + " Read");
                }
            }

            m_dbTLConnection.Close();
        }

        public static void CreateMoves()
        {
            //load names and descriptions to the translation table
            //include the english column of the actual description to see if there's a difference.
            //just take it out after the initial setup
            //this is meant to be a one-time fill-in job.  all translations will go to their respective places, and then this should never be visited again
            //even when new blank descs are filled in, those blanks will not have a corresponding translation in the table, so running this script again will give useless translations
            //so when adding the new blank descs, the initial table generation will be run on the final translation table from the spreadsheet,
            //and that table will simply be put through the converter for code
            //alternatively, have this be the start of the stream
            //the downside is, if there are any english messages that sound better than the originals, they must be moved in manually.
            SQLiteConnection m_dbTLConnection = new SQLiteConnection("Data Source="+ TL_FILE + ";Version=3;");
            m_dbTLConnection.Open();

            string[][] totalEntries = new string[TOTAL_MOVES * 2][];

            for (int ii = 0; ii < TOTAL_MOVES; ii++)
            {

                string[] nameRow = new string[TOTAL_LANG_COLS];
                for (int jj = 0; jj < nameRow.Length; jj++)
                    nameRow[jj] = "";

                assignRowElements(nameRow, ii, m_dbTLConnection, "SELECT * FROM pokemon_v2_movename WHERE move_id = " + ii + " AND language_id = ", "name");

                totalEntries[ii * 2] = nameRow;

                string[] descRow = new string[TOTAL_LANG_COLS];
                for (int jj = 0; jj < descRow.Length; jj++)
                    descRow[jj] = "";

                assignRowElements(descRow, ii, m_dbTLConnection,
                    "SELECT * FROM pokemon_v2_moveflavortext WHERE move_id = " + ii + " AND version_group_id = "+ADD_VERSION_ID+" AND language_id = ", "flavor_text");

                totalEntries[ii * 2 + 1] = descRow;

                Console.WriteLine("#" + ii + " " + totalEntries[ii * 2][2] + " Read");
            }

            m_dbTLConnection.Close();

            using (StreamWriter file = new StreamWriter("skill.txt"))
            {
                for (int ii = 0; ii < totalEntries.Length; ii++)
                {
                    for (int jj = 0; jj < totalEntries[ii].Length; jj++)
                    {
                        if (jj > 0)
                            file.Write("\t");
                        file.Write(totalEntries[ii][jj]);
                    }
                    file.WriteLine();
                }
            }
        }



        const int TOTAL_ABILITIES = 299;



        public static void CreateAbilityCodes()
        {
            //load names and descriptions to the translation table
            //include the english column of the actual description to see if there's a difference.
            //just take it out after the initial setup
            //this is meant to be a one-time fill-in job.  all translations will go to their respective places, and then this should never be visited again
            //even when new blank descs are filled in, those blanks will not have a corresponding translation in the table, so running this script again will give useless translations
            //so when adding the new blank descs, the initial table generation will be run on the final translation table from the spreadsheet,
            //and that table will simply be put through the converter for code
            //alternatively, have this be the start of the stream
            //the downside is, if there are any english messages that sound better than the originals, they must be moved in manually.
            SQLiteConnection m_dbTLConnection = new SQLiteConnection("Data Source=" + TL_FILE + ";Version=3;");
            m_dbTLConnection.Open();

            using (StreamWriter file = new StreamWriter("intrinsic.txt"))
            {
                for (int ii = 0; ii < TOTAL_ABILITIES; ii++)
                {
                    string name = getTLStr(m_dbTLConnection, "SELECT * FROM pokemon_v2_abilityname WHERE ability_id = " + ii + " AND language_id = ", 9, "name");
                    string description = getTLStr(m_dbTLConnection, "SELECT * FROM pokemon_v2_abilityflavortext WHERE ability_id = " + ii + " AND version_group_id = "+ADD_VERSION_ID+" AND language_id = ", 9, "flavor_text");

                    file.WriteLine("else if (ii == " + ii + ")");
                    file.WriteLine("{");
                    file.WriteLine("	ability.Name = new LocalText(\"**" + name.Replace('’', '\'') + "\");");
                    file.WriteLine("	ability.Desc = new LocalText(\"" + description + "\");");
                    file.WriteLine("}");

                    Console.WriteLine("#" + ii + " " + name + " Read");
                }
            }

            m_dbTLConnection.Close();
        }

        public static void CreateAbilities()
        {
            //load names and descriptions to the translation table
            //include the english column of the actual description to see if there's a difference.
            //just take it out after the initial setup
            //this is meant to be a one-time fill-in job.  all translations will go to their respective places, and then this should never be visited again
            //even when new blank descs are filled in, those blanks will not have a corresponding translation in the table, so running this script again will give useless translations
            //so when adding the new blank descs, the initial table generation will be run on the final translation table from the spreadsheet,
            //and that table will simply be put through the converter for code
            //alternatively, have this be the start of the stream
            //the downside is, if there are any english messages that sound better than the originals, they must be moved in manually.
            SQLiteConnection m_dbTLConnection = new SQLiteConnection("Data Source="+ TL_FILE + ";Version=3;");
            m_dbTLConnection.Open();

            string[][] totalEntries = new string[TOTAL_ABILITIES * 2][];

            for (int ii = 0; ii < TOTAL_ABILITIES; ii++)
            {
                string[] nameRow = new string[TOTAL_LANG_COLS];
                for (int jj = 0; jj < nameRow.Length; jj++)
                    nameRow[jj] = "";

                assignRowElements(nameRow, ii, m_dbTLConnection, "SELECT * FROM pokemon_v2_abilityname WHERE ability_id = " + ii + " AND language_id = ", "name");

                totalEntries[ii * 2] = nameRow;


                string[] descRow = new string[TOTAL_LANG_COLS];
                for (int jj = 0; jj < descRow.Length; jj++)
                    descRow[jj] = "";

                assignRowElements(descRow, ii, m_dbTLConnection, 
                    "SELECT * FROM pokemon_v2_abilityflavortext WHERE ability_id = " + ii + " AND version_group_id = "+ADD_VERSION_ID+" AND language_id = ", "flavor_text");

                totalEntries[ii * 2 + 1] = descRow;

                Console.WriteLine("#" + ii + " " + totalEntries[ii * 2][2] + " Read");
            }

            m_dbTLConnection.Close();

            using (StreamWriter file = new StreamWriter("intrinsic.txt"))
            {
                for (int ii = 0; ii < totalEntries.Length; ii++)
                {
                    for (int jj = 0; jj < totalEntries[ii].Length; jj++)
                    {
                        if (jj > 0)
                            file.Write("\t");
                        file.Write(totalEntries[ii][jj]);
                    }
                    file.WriteLine();
                }
            }
        }



        const int TOTAL_ITEMS = 1006;


        public static void CreateItems()
        {
            //load names and descriptions to the translation table
            //include the english column of the actual description to see if there's a difference.
            //just take it out after the initial setup
            //this is meant to be a one-time fill-in job.  all translations will go to their respective places, and then this should never be visited again
            //even when new blank descs are filled in, those blanks will not have a corresponding translation in the table, so running this script again will give useless translations
            //so when adding the new blank descs, the initial table generation will be run on the final translation table from the spreadsheet,
            //and that table will simply be put through the converter for code
            //alternatively, have this be the start of the stream
            //the downside is, if there are any english messages that sound better than the originals, they must be moved in manually.

            SQLiteConnection m_dbTLConnection = new SQLiteConnection("Data Source=" + TL_FILE + ";Version=3;");
            m_dbTLConnection.Open();

            string[][] totalEntries = new string[TOTAL_ITEMS * 2][];

            for (int ii = 0; ii < TOTAL_ITEMS; ii++)
            {
                string[] nameRow = new string[TOTAL_LANG_COLS];
                for (int jj = 0; jj < nameRow.Length; jj++)
                    nameRow[jj] = "";

                assignRowElements(nameRow, ii, m_dbTLConnection, 
                    "SELECT * FROM pokemon_v2_itemname WHERE item_id = " + ii + " AND language_id = ", "name");

                totalEntries[ii * 2] = nameRow;


                string[] descRow = new string[TOTAL_LANG_COLS];
                for (int jj = 0; jj < descRow.Length; jj++)
                    descRow[jj] = "";

                assignRowElements(descRow, ii, m_dbTLConnection,
                    "SELECT * FROM pokemon_v2_itemflavortext WHERE item_id = " + ii + " AND version_group_id = 25 AND language_id = ", "flavor_text");

                totalEntries[ii * 2 + 1] = descRow;

                Console.WriteLine("#" + ii + " " + totalEntries[ii * 2][0] + " Read");
            }

            m_dbTLConnection.Close();

            using (StreamWriter file = new StreamWriter("item.txt"))
            {
                for (int ii = 0; ii < totalEntries.Length; ii++)
                {
                    for (int jj = 0; jj < totalEntries[ii].Length; jj++)
                    {
                        if (jj > 0)
                            file.Write("\t");
                        file.Write(totalEntries[ii][jj]);
                    }
                    file.WriteLine();
                }
            }
        }

        public static void CreateLearnables(MonsterData[] totalEntries)
        {
            Dictionary<string, int> dexToId = new Dictionary<string, int>();
            for (int ii = 0; ii < TOTAL_DEX; ii++)
            {
                string fileName = getAssetName(totalEntries[ii].Name.DefaultText);
                dexToId[fileName] = ii;
            }

            Dictionary<string, int> learnMoves = new Dictionary<string, int>();
            Dictionary<string, int> eggMoves = new Dictionary<string, int>();
            for (int ii = 0; ii < TOTAL_DEX; ii++)
            {
                MonsterData mon = totalEntries[ii];
                if (!mon.Released)
                    continue;
                for (int jj = 0; jj < mon.Forms.Count; jj++)
                {
                    MonsterFormData form = (MonsterFormData)mon.Forms[jj];
                    if (!form.Released)
                        continue;

                    foreach (LearnableSkill skill in form.SecretSkills)
                    {
                        bool newSkill = true;
                        string prevData = mon.PromoteFrom;
                        int prevForm = form.PromoteForm;
                        if (!String.IsNullOrEmpty(prevData))
                        {
                            MonsterData preMon = totalEntries[dexToId[prevData]];
                            MonsterFormData preForm = (MonsterFormData)preMon.Forms[prevForm];
                            foreach (LearnableSkill preSkill in preForm.SecretSkills)
                            {
                                if (preSkill.Skill == skill.Skill)
                                {
                                    newSkill = false;
                                    break;
                                }
                            }
                        }

                        if (newSkill)
                        {
                            if (!learnMoves.ContainsKey(skill.Skill))
                                learnMoves[skill.Skill] = 0;
                            learnMoves[skill.Skill]++;
                        }
                    }

                    
                    foreach (LearnableSkill skill in form.SharedSkills)
                    {
                        bool newSkill = true;
                        string prevData = mon.PromoteFrom;
                        int prevForm = form.PromoteForm;
                        if (!String.IsNullOrEmpty(prevData))
                        {
                            MonsterData preMon = totalEntries[dexToId[prevData]];
                            MonsterFormData preForm = (MonsterFormData)preMon.Forms[prevForm];
                            foreach (LearnableSkill preSkill in preForm.SharedSkills)
                            {
                                if (preSkill.Skill == skill.Skill)
                                {
                                    newSkill = false;
                                    break;
                                }
                            }
                        }

                        if (newSkill)
                        {
                            if (!eggMoves.ContainsKey(skill.Skill))
                                eggMoves[skill.Skill] = 0;
                            eggMoves[skill.Skill]++;
                        }
                    }
                }
            }

            Dictionary<string, (int, int)> totalMoves = new Dictionary<string, (int, int)>();
            foreach (string key in eggMoves.Keys)
            {
                int tutorCount;
                learnMoves.TryGetValue(key, out tutorCount);
                totalMoves[key] = (eggMoves[key], tutorCount);
            }
            foreach (string key in learnMoves.Keys)
            {
                if (!eggMoves.ContainsKey(key))
                    totalMoves[key] = (0, learnMoves[key]);
            }
            using (StreamWriter file = new StreamWriter("tutor.txt"))
            {
                foreach (string key in totalMoves.Keys)
                {
                    SkillData skillData = DataManager.LoadData<SkillData>(key, DataManager.DataType.Skill.ToString());
                    int tutorCost;
                    if (skillData.BaseCharges < 10)
                        tutorCost = (10 - skillData.BaseCharges) + 7;
                    else if (skillData.BaseCharges < 20)
                        tutorCost = (20 - skillData.BaseCharges) / 2 + 2;
                    else if (skillData.BaseCharges < 23)
                        tutorCost = 2;
                    else
                        tutorCost = 1;
                    file.WriteLine(key + "\t" + totalMoves[key].Item1 + "\t" + totalMoves[key].Item2 + "\t" + tutorCost);
                }
            }
        }

        private static string getBasePrevo(MonsterData[] mons, Dictionary<string, int> filenames, int id)
        {
            MonsterData monData = mons[id];
            while (!String.IsNullOrEmpty(monData.PromoteFrom))
            {
                monData = mons[filenames[monData.PromoteFrom]];
            }
            return getAssetName(monData.Name.DefaultText);
        }

        private static void ListAllIncompletes(MonsterData[] mons)
        {
            Dictionary<string, int> filenames = new Dictionary<string, int>();
            for (int ii = 0; ii < mons.Length; ii++)
            {
                string filename = getAssetName(mons[ii].Name.DefaultText);
                filenames[filename] = ii;
            }
            Dictionary<string, List<string>> missingMons = new Dictionary<string, List<string>>();
            HashSet<string> hasData = new HashSet<string>();
            for (int ii = 0; ii < mons.Length; ii++)
            {
                MonsterData entry = mons[ii];
                for (int jj = 0; jj < entry.Forms.Count; jj++)
                {
                    MonsterFormData form = (MonsterFormData)entry.Forms[jj];
                    if (!form.FormName.DefaultText.Contains("Mega ") && !form.FormName.DefaultText.Contains("Gigantamax "))
                    {
                        string prevo = getBasePrevo(mons, filenames, ii);
                        if ((!entry.Released || !form.Released))
                        {
                            if (!missingMons.ContainsKey(prevo))
                                missingMons[prevo] = new List<string>();
                            missingMons[prevo].Add(form.FormName.DefaultText);
                        }
                        else
                        {
                            hasData.Add(prevo);
                        }
                    }
                }
            }
            Console.WriteLine("INCOMPLETE LINES");
            foreach (string name in missingMons.Keys)
            {
                if (hasData.Contains(name))
                {
                    Console.WriteLine(name);
                    List<string> missing = missingMons[name];
                    foreach (string miss in missing)
                        Console.WriteLine("  " + miss);
                }
            }
        }
    }
}

