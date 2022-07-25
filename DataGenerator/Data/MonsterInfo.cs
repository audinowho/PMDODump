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


        const int TOTAL_DEX = 899;
        static Dictionary<int, string> langIDs;
        static Dictionary<int, int> langCols;
        static int[] existing = { 1, 3, 5, 6, 7, 8, 9 };
        static int[] straggler = { 11 };
        static int[] added = { 4, 12 };
        const int TOTAL_LANG_COLS = 13;

        static string MONSTER_PATH { get => GenPath.DATA_GEN_PATH + "Monster/"; }
        static string DEX_FILE { get => MONSTER_PATH + "pokedex.6.sqlite"; }
        static string DEX_7_FILE { get => MONSTER_PATH + "pokedex.7.sqlite"; }
        static string TL_FILE { get => MONSTER_PATH + "pokedex.8.sqlite"; }

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

            for (int ii = 0; ii < TOTAL_DEX; ii++)
            {
                totalEntries[ii] = LoadDex(m_dbTLConnection, ii);
                Console.WriteLine("#" + ii + " " + totalEntries[ii].Name + " Read");
                for (int jj = 0; jj < totalEntries[ii].Forms.Count; jj++)
                {
                    List<byte> personality;
                    int actualForme = jj;
                    while (!personalities.TryGetValue(ii + "-" + actualForme, out personality))
                    {
                        actualForme--;
                    }
                    ((MonsterFormData)totalEntries[ii].Forms[jj]).Personalities = personality;
                }
            }
            //WritePersonalityChecklist(personalities, totalEntries);

            //m_dbConnection.Close();
            //m_db7Connection.Close();
            m_dbTLConnection.Close();

            //string outpt = "";
            for (int ii = 0; ii < TOTAL_DEX; ii++)
            {
                totalEntries[ii].JoinRate = MapJoinRate(ii, totalEntries);
                MapFriendshipEvo(ii, totalEntries);
                //if (TotalEntries[ii].JoinRate > 0)
                //    outpt += (TotalEntries[ii].Name + ": " + TotalEntries[ii].JoinRate + "\n");
            }

            for (int ii = 0; ii < TOTAL_DEX; ii++)
            {
                //TODO: String Assets
                DataManager.SaveData(ii.ToString(), DataManager.DataType.Monster.ToString(), totalEntries[ii]);
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
                    switch (ii)
                    {
                        case 421://cherrim
                        case 746://wishiwashi
                        case 774://minior
                        case 877://morpeko
                            include = true;
                            break;
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
            formEntry.LevelSkills.Add(new LevelUpSkill());
            entry.Forms.Add(formEntry);

            MonsterFormData formEntry2 = new MonsterFormData();
            formEntry2.FormName = new LocalText("Substitute");
            formEntry.GenderlessWeight = 1;
            formEntry2.Height = 1;
            formEntry2.Weight = 1;
            formEntry2.ExpYield = 1;
            formEntry2.BaseHP = 1;
            formEntry2.BaseAtk = 1;
            formEntry2.BaseDef = 1;
            formEntry2.BaseMAtk = 1;
            formEntry2.BaseMDef = 1;
            formEntry2.BaseSpeed = 1;
            formEntry.Element1 = Text.Sanitize(Text.GetMemberTitle(ElementInfo.Element.None.ToString())).ToLower();
            formEntry.Element2 = Text.Sanitize(Text.GetMemberTitle(ElementInfo.Element.None.ToString())).ToLower();
            formEntry.LevelSkills.Add(new LevelUpSkill());
            entry.Forms.Add(formEntry2);



            return entry;
        }

        public static MonsterData LoadDex(SQLiteConnection m_dbTLConnection, int index)
        {
            if (index == 0)
                return LoadZero();
            MonsterData entry = new MonsterData();

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
                        entry.PromoteFrom = Convert.ToInt32(prevo);
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
                        if (currentBranch.Result == evoSpecies)
                            hasEvo = true;
                    }
                    if (hasEvo)
                        continue;

                    PromoteBranch branch = new PromoteBranch();
                    branch.Result = evoSpecies;
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
                            evoDetail.StatBoostStatus = 12;
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 368)//gorebyss
                        {
                            EvoStatBoost evoDetail = new EvoStatBoost();
                            evoDetail.StatBoostStatus = 13;
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 738)//vikavolt
                        {
                            //FIXME
                            EvoLocation evoDetail = new EvoLocation();
                            evoDetail.TileElement = Text.Sanitize(ElementInfo.Element.Electric.ToString()).ToLower();
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 740)//crabominable
                        {
                            //FIXME
                            EvoLocation evoDetail = new EvoLocation();
                            evoDetail.TileElement = Text.Sanitize(ElementInfo.Element.Ice.ToString()).ToLower();
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 745)//lycanroc
                        {
                            EvoLevel evoDetail = new EvoLevel();
                            evoDetail.Level = Convert.ToInt32(level);
                            branch.Details.Add(evoDetail);

                            branch.Details.Add(new EvoFormDusk());
                        }
                        else if (evoSpecies == 841)//flapple
                        {
                            EvoItem evoDetail = new EvoItem();
                            evoDetail.ItemNum = 2;//Big Apple
                            branch.Details.Add(evoDetail);

                            EvoHunger hungerDetail = new EvoHunger();
                            hungerDetail.Hungry = true;
                            branch.Details.Add(hungerDetail);
                        }
                        else if (evoSpecies == 842)//appletun
                        {
                            EvoItem evoDetail = new EvoItem();
                            evoDetail.ItemNum = 1;//Apple
                            branch.Details.Add(evoDetail);

                            EvoHunger hungerDetail = new EvoHunger();
                            branch.Details.Add(hungerDetail);
                        }
                        else if (evoSpecies == 853)//grapploct
                        {
                            //FIXME
                            EvoMove evoDetail = new EvoMove();
                            evoDetail.MoveNum = 269;//taunt
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 855)//polteageist
                        {
                            EvoItem evoDetail = new EvoItem();
                            evoDetail.ItemNum = 360;//Cracked Pot
                            branch.Details.Add(evoDetail);
                        }
                        else if (evoSpecies == 862)//obstagoon
                        {
                            //FIXME
                            EvoLevel evoDetail = new EvoLevel();
                            evoDetail.Level = Convert.ToInt32(35);
                            branch.Details.Add(evoDetail);

                            EvoItem evoDetail2 = new EvoItem();
                            evoDetail2.ItemNum = 378;//moon ribbon
                            branch.Details.Add(evoDetail2);
                        }
                        else if (evoSpecies == 865)//sirfetch'd
                        {
                            //FIXME
                            EvoCrits evoDetail = new EvoCrits();
                            evoDetail.CritStatus = 127;
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
                                    evoDetail.ItemNum = 354;//Leaf Stone
                                    branch.Details.Add(evoDetail);
                                }
                                else if (Convert.ToInt32(location) == 48 || Convert.ToInt32(location) == 380 || Convert.ToInt32(location) == 640)
                                {
                                    EvoItem evoDetail = new EvoItem();
                                    evoDetail.ItemNum = 379;//Ice Stone
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
                                evoDetail2.ItemNum = (time == "night") ? 378 : 377;
                                //EvoTime evoDetail2 = new EvoTime();
                                //evoDetail2.Time = (time == "night") ? Maps.ZoneManager.TimeOfDay.Night : Maps.ZoneManager.TimeOfDay.Day;
                                branch.Details.Add(evoDetail2);
                            }
                            else if (CheckEvoConditions(reader, "min_happiness", "time_of_day"))
                            {
                                EvoFriendship evoDetail = new EvoFriendship();
                                branch.Details.Add(evoDetail);

                                if (index < branch.Result) //pre-evos are excluded from time of day req
                                {
                                    EvoItem evoDetail2 = new EvoItem();
                                    evoDetail2.ItemNum = (time == "night") ? 378 : 377;
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
                                evoDetail2.ItemNum = (time == "night") ? 378 : 377;
                                branch.Details.Add(evoDetail2);
                            }
                            else if (CheckEvoConditions(reader, "known_move_id"))
                            {
                                EvoMove evoDetail = new EvoMove();
                                evoDetail.MoveNum = Convert.ToInt32(move);
                                branch.Details.Add(evoDetail);
                            }
                            else if (CheckEvoConditions(reader, "min_affection", "known_move_type_id"))
                            {
                                //EvoItem evoDetail = new EvoItem();
                                //evoDetail.ItemNum = 335;//Pink Bow
                                //branch.Details.Add(evoDetail);

                                EvoMoveElement evoDetail2 = new EvoMoveElement();
                                evoDetail2.MoveElement = MapElement(Convert.ToInt32(moveType));
                                branch.Details.Add(evoDetail2);
                            }
                            else if (CheckEvoConditions(reader, "min_beauty"))
                            {
                                EvoItem evoDetail = new EvoItem();
                                evoDetail.ItemNum = 373;//prism scale
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
                                evoDetail.Species = Convert.ToInt32(allySpecies);
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
                                evoDetail.ItemNum = 365;//Link Cable
                                branch.Details.Add(evoDetail);
                            }
                            else if (CheckEvoConditions(reader, "held_item_id"))
                            {
                                EvoItem evoDetail = new EvoItem();
                                evoDetail.ItemNum = MapItem(Convert.ToInt32(heldItem));
                                if (evoDetail.ItemNum == 0)
                                {
                                    evoDetail = new EvoItem();
                                    evoDetail.ItemNum = 365;//Link Cable
                                    branch.Details.Add(evoDetail);
                                }
                                else
                                    branch.Details.Add(evoDetail);
                            }
                            else if (CheckEvoConditions(reader, "trade_species_id"))
                            {
                                EvoPartner evoDetail = new EvoPartner();
                                evoDetail.Species = Convert.ToInt32(tradeSpecies);
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
                                if (evoDetail.ItemNum == 0)
                                    throw new Exception(entry.Name + ": Unknown Item Evo");
                                branch.Details.Add(evoDetail);
                            }
                            else if (CheckEvoConditions(reader, "evolution_item_id", "gender_id"))
                            {
                                EvoItem evoDetail = new EvoItem();
                                evoDetail.ItemNum = MapItem(Convert.ToInt32(itemNum));
                                if (evoDetail.ItemNum == 0)
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
                        evoDetail.ShedSpecies = 292;
                        branch.Details.Add(evoDetail);
                    }
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
                            }
                            break;
                        case 563://cofagrigus
                            {
                                //original form required due to having a regional -> new that requires a different form
                                branch.Details.Insert(0, new EvoForm(0));
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
                        evoDetail2.ItemNum = 378;//moon ribbon
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
                        evoDetail.ItemNum = 379;//ice stone
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
                        evoDetail.ItemNum = 379;//ice stone
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
                        evoDetail.ItemNum = 365;//Link Cable
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
                        evoDetail.ItemNum = 374;//king's rock
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
                        evoDetail.ItemNum = 361;//Chipped Pot
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
                branch.Result = 809;

                EvoItem evoDetail = new EvoItem();
                evoDetail.ItemNum = 365;//Link Cable
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


                    int dexId = Convert.ToInt32(reader["dexId"].ToString());
                    int formId = Convert.ToInt32(reader["formId"].ToString());
                    bool battle_only = (bool)reader["is_battle_only"];
                    int version = Convert.ToInt32(reader["version_group_id"].ToString());
                    if (version < 16)
                        version = 16;
                    MonsterFormData formEntry = LoadForme(m_dbTLConnection, version, index, dexId, formId, entry.Name);
                    formEntry.Generation = genVersion(version);
                    if (Ratio == -1)
                        formEntry.GenderlessWeight = 1;
                    else
                    {
                        formEntry.MaleWeight = 8 - Ratio;
                        formEntry.FemaleWeight = Ratio;
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

                    formEntry.Released = hasFormeGraphics(index, entry.Forms.Count);

                    entry.Forms.Add(formEntry);
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
                    entry.ExpYield = Convert.ToInt32(reader["base_experience"].ToString());
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

            //level moves
            sql = "SELECT * FROM pokemon_v2_pokemonmove WHERE pokemon_id = " + dexId + " AND version_group_id = "+ version + " AND move_learn_method_id = 1 ORDER BY [level], [order]";
            command = new SQLiteCommand(sql, m_dbTLConnection);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    LevelUpSkill levelMove = new LevelUpSkill();
                    levelMove.Level = Convert.ToInt32(reader["level"].ToString());
                    levelMove.Skill = Convert.ToInt32(reader["move_id"].ToString());
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
                    if (method == 2)//egg
                    {
                        if (!entry.SharedSkills.Contains(new LearnableSkill(move)))
                            entry.SharedSkills.Add(new LearnableSkill(move));
                    }
                    else if (method == 3)//tutor
                    {
                        if (!entry.SecretSkills.Contains(new LearnableSkill(move)))
                            entry.SecretSkills.Add(new LearnableSkill(move));
                    }
                    else if (method == 4)//TM
                    {
                        if (!entry.TeachSkills.Contains(new LearnableSkill(move)))
                            entry.TeachSkills.Add(new LearnableSkill(move));
                    }
                    else//event
                    {
                        if (!entry.SecretSkills.Contains(new LearnableSkill(move)))
                            entry.SecretSkills.Add(new LearnableSkill(move));
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

        public static int MapItem(int itemNum)
        {
            switch (itemNum)
            {
                case 80: return 356;//sun-stone
                case 81: return 355;//moon-stone
                case 82: return 351;//fire-stone
                case 83: return 352;//thunder-stone
                case 84: return 353;//water-stone
                case 85: return 354;//leaf-stone
                case 107: return 362;//shiny-stone
                case 108: return 363;//dusk-stone
                case 109: return 364;//dawn-stone
                case 198: return 374;//kings-rock
                case 210: return 347;//metal-coat
                case 212: return 333;//dragon-scale
                case 298: return 371;//protector
                case 299: return 358;//electirizer
                case 300: return 357;//magmarizer
                case 302: return 359;//reaper-cloth
                case 580: return 373;//prism-scale
                case 885: return 379;//ice stone
                case 301: //dubious-disc
                case 110: //oval-stone
                case 203: //deep-sea-tooth
                case 204: //deep-sea-scale
                case 229: //up-grade
                case 303: //razor-claw
                case 304: //razor-fang
                case 686: //whipped-dream
                case 687: //sachet
                    return 0;
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
            MonsterData entry = totalEntries[dexIndex];
            int evoCount = 0;
            foreach (PromoteBranch evo in entry.Promotions)
            {
                if (evo.Result < GetGenBoundary(dexIndex))
                    evoCount++;
            }
            int newRate = 0;
            if (evoCount == 0)//...if it doesn't have any evos (don't count future gens)
            {
                //cannot evolve further
                if (entry.PromoteFrom == -1)//single stager -25% to 65%
                    newRate = entry.JoinRate * 90 / 255 - 25;
                else if (totalEntries[entry.PromoteFrom].PromoteFrom == -1)//second stage -40% to 20%
                    newRate = entry.JoinRate * 60 / 255 - 40;
                else//third stage: -65% to -15%
                    newRate = entry.JoinRate * 50 / 255 - 65;
            }
            else if (entry.PromoteFrom == -1)//first stage evolvable: 10% to 80%
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
                int evoSpecies = branch.Result;
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

                if (entry.PromoteFrom != -1)
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
                if (entry.PromoteFrom == -1)
                {
                    //impossible
                }
                else if (totalEntries[entry.PromoteFrom].PromoteFrom == -1)//second stage
                {

                }
                else//third stage
                {

                }
            }
            else if (entry.PromoteFrom == -1)//first stage evolvable: 10% to 80%
            {

            }
            else //mid evos: -25% to 55%
            {

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
            else
                return 8;
        }

        private static bool hasFormeGraphics(int species, int form)
        {
            CharaIndexNode charaIndex = GraphicsManager.LoadCharaIndices(GraphicsManager.CONTENT_PATH + "Chara/");

            if (form == 0)
                form = -1;
            MonsterID id = new MonsterID(species, form, -1, Gender.Unknown);
            return GraphicsManager.GetFallbackForm(charaIndex, id) == id;
        }

        const int TOTAL_MOVES = 826;

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
            SQLiteConnection m_dbTLConnection = new SQLiteConnection("Data Source=" + DEX_7_FILE + ";Version=3;");
            m_dbTLConnection.Open();

            using (StreamWriter file = new StreamWriter("skill.txt"))
            {
                for (int ii = 0; ii < TOTAL_MOVES; ii++)
                {
                    string name = getTLStr(m_dbTLConnection, "SELECT * FROM pokemon_v2_movename WHERE move_id = " + ii + " AND language_id = ", 9, "name");
                    string description = getTLStr(m_dbTLConnection, "SELECT * FROM pokemon_v2_moveflavortext WHERE move_id = " + ii + " AND version_group_id = 18 AND language_id = ", 9, "flavor_text");

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
                    "SELECT * FROM pokemon_v2_moveflavortext WHERE move_id = " + ii + " AND version_group_id = 20 AND language_id = ", "flavor_text");

                totalEntries[ii * 2 + 1] = descRow;

                Console.WriteLine("#" + ii + " " + totalEntries[ii * 2][0] + " Read");
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



        const int TOTAL_ABILITIES = 268;


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
                    "SELECT * FROM pokemon_v2_abilityflavortext WHERE ability_id = " + ii + " AND version_group_id = 18 AND language_id = ", "flavor_text");

                totalEntries[ii * 2 + 1] = descRow;

                Console.WriteLine("#" + ii + " " + totalEntries[ii * 2][0] + " Read");
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
                    "SELECT * FROM pokemon_v2_itemflavortext WHERE item_id = " + ii + " AND version_group_id = 18 AND language_id = ", "flavor_text");

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
    }
}

