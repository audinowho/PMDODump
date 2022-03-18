using System;
using System.Threading;
using System.Globalization;
using RogueEssence.Content;
using RogueEssence.Data;
using RogueEssence.Dev;
using RogueEssence.Dungeon;
using RogueEssence.Script;
using RogueEssence;
using PMDC.Dev;
using PMDC.Data;
using DataGenerator.Dev;
using DataGenerator.Data;
using RogueEssence.Ground;
using RogueElements;

namespace DataGenerator
{
    class Program
    {
        static void Main()
        {

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            string[] args = System.Environment.GetCommandLineArgs();
            PathMod.InitExePath(args[0]);
            DiagManager.InitInstance();
            //DiagManager.Instance.CurSettings = DiagManager.Instance.LoadSettings();
            DiagManager.Instance.UpgradeBinder = new UpgradeBinder();

            try
            {
                DiagManager.Instance.LogInfo("=========================================");
                DiagManager.Instance.LogInfo(String.Format("SESSION STARTED: {0}", String.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now)));
                DiagManager.Instance.LogInfo("Version: " + Versioning.GetVersion().ToString());
                DiagManager.Instance.LogInfo(Versioning.GetDotNetInfo());
                DiagManager.Instance.LogInfo("=========================================");

                bool loadStrings = false;
                bool itemPrep = false;
                bool zonePrep = false;
                bool saveStrings = false;
                bool demo = false;
                DataManager.DataType convertIndices = DataManager.DataType.None;
                DataManager.DataType reserializeIndices = DataManager.DataType.None;
                DataManager.DataType dump = DataManager.DataType.None;
                for (int ii = 1; ii < args.Length; ii++)
                {
                    if (args[ii] == "-asset")
                    {
                        PathMod.ASSET_PATH = System.IO.Path.GetFullPath(PathMod.ExePath + args[ii + 1]);
                        ii++;
                    }
                    else if (args[ii] == "-raw")
                    {
                        PathMod.DEV_PATH = System.IO.Path.GetFullPath(PathMod.ExePath + args[ii + 1]);
                        ii++;
                    }
                    else if (args[ii] == "-gen")
                    {
                        GenPath.DATA_GEN_PATH = args[ii + 1];
                        ii++;
                    }
                    else if (args[ii] == "-index")
                    {
                        int jj = 1;
                        while (args.Length > ii + jj)
                        {
                            DataManager.DataType conv = DataManager.DataType.None;
                            foreach (DataManager.DataType type in Enum.GetValues(typeof(DataManager.DataType)))
                            {
                                if (args[ii + jj].ToLower() == type.ToString().ToLower())
                                {
                                    conv = type;
                                    break;
                                }
                            }
                            if (conv != DataManager.DataType.None)
                                convertIndices |= conv;
                            else
                                break;
                            jj++;
                        }
                        ii += jj - 1;
                    }
                    else if (args[ii] == "-strings")
                    {
                        ii++;
                        if (args[ii] == "out")
                            loadStrings = true;
                        else if (args[ii] == "in")
                            saveStrings = true;
                    }
                    else if (args[ii] == "-itemprep")
                    {
                        ii++;
                        itemPrep = true;
                    }
                    else if (args[ii] == "-zoneprep")
                    {
                        ii++;
                        zonePrep = true;
                    }
                    else if (args[ii] == "-reserialize")
                    {
                        int jj = 1;
                        while (args.Length > ii + jj)
                        {
                            DataManager.DataType conv = DataManager.DataType.None;
                            foreach (DataManager.DataType type in Enum.GetValues(typeof(DataManager.DataType)))
                            {
                                if (args[ii + jj].ToLower() == type.ToString().ToLower())
                                {
                                    conv = type;
                                    break;
                                }
                            }
                            if (conv != DataManager.DataType.None)
                                reserializeIndices |= conv;
                            else
                                break;
                            jj++;
                        }
                        ii += jj - 1;
                    }
                    else if (args[ii] == "-dump")
                    {
                        int jj = 1;
                        while (args.Length > ii + jj)
                        {
                            DataManager.DataType conv = DataManager.DataType.None;
                            foreach (DataManager.DataType type in Enum.GetValues(typeof(DataManager.DataType)))
                            {
                                if (args[ii + jj].ToLower() == type.ToString().ToLower())
                                {
                                    conv = type;
                                    break;
                                }
                            }
                            if (conv != DataManager.DataType.None)
                                dump |= conv;
                            else
                                break;
                            jj++;
                        }
                        ii += jj - 1;
                    }
                    else if (args[ii] == "-demo")
                    {
                        demo = true;
                    }
                }


                GraphicsManager.InitParams();

                DiagManager.Instance.DevMode = true;

                Text.Init();
                Text.SetCultureCode("");

                if (itemPrep)
                {
                    LuaEngine.InitInstance();
                    DataManager.InitInstance();
                    DataManager.Instance.InitData();

                    AutoItemInfo.CreateContentLists();
                }

                if (zonePrep)
                {
                    LuaEngine.InitInstance();
                    DataManager.InitInstance();
                    DataManager.Instance.InitData();

                    ZoneInfo.CreateContentLists();
                }

                if (loadStrings)
                {
                    //we need the datamanager for this
                    LuaEngine.InitInstance();
                    DataManager.InitInstance();
                    DataManager.Instance.InitData();

                    Localization.PrintDescribedStringTable(DataManager.DataType.Skill, DataManager.Instance.GetSkill);
                    Localization.PrintDescribedStringTable(DataManager.DataType.Intrinsic, DataManager.Instance.GetIntrinsic);
                    Localization.PrintDescribedStringTable(DataManager.DataType.Status, DataManager.Instance.GetStatus);
                    Localization.PrintDescribedStringTable(DataManager.DataType.MapStatus, DataManager.Instance.GetMapStatus);
                    Localization.PrintDescribedStringTable(DataManager.DataType.Tile, DataManager.Instance.GetTile);
                    Localization.PrintItemStringTable();
                    Localization.PrintTitledDataTable(DataManager.MAP_PATH, DataManager.MAP_EXT, DataManager.Instance.GetMap);
                    Localization.PrintTitledDataTable(DataManager.GROUND_PATH, DataManager.GROUND_EXT, DataManager.Instance.GetGround);
                    Localization.PrintZoneStringTable();

                    Localization.PrintExclusiveNameStringTable();
                    Localization.PrintExclusiveDescStringTable();

                    return;
                }
                if (saveStrings)
                {
                    LuaEngine.InitInstance();
                    //we need the datamanager for this
                    DataManager.InitInstance();
                    DataManager.Instance.InitData();



                    Localization.WriteNamedDataTable(DataManager.DataType.Skin, 2);
                    Localization.CopyNamedData(DataManager.DataType.Skin, 1, 2);
                    Localization.WriteNamedDataTable(DataManager.DataType.Element, 0);
                    Localization.WriteNamedDataTable(DataManager.DataType.Rank, 0);
                    Localization.WriteNamedDataTable(DataManager.DataType.AI, 5);
                    Localization.WriteDescribedDataTable(DataManager.DataType.Skill);
                    Localization.WriteDescribedDataTable(DataManager.DataType.Intrinsic);
                    Localization.WriteDescribedDataTable(DataManager.DataType.Status);
                    Localization.WriteDescribedDataTable(DataManager.DataType.MapStatus);
                    Localization.WriteDescribedDataTable(DataManager.DataType.Tile);
                    Localization.WriteItemStringTable();
                    ItemInfo.AddCalculatedItemData();
                    Localization.WriteTitledDataTable(DataManager.MAP_PATH, DataManager.MAP_EXT, DataManager.Instance.GetMap);
                    Localization.WriteTitledDataTable(DataManager.GROUND_PATH, DataManager.GROUND_EXT, DataManager.Instance.GetGround);
                    Localization.WriteZoneStringTable();

                    DiagManager.Instance.LogInfo("Reserializing indices");
                    RogueEssence.Dev.DevHelper.RunIndexing(DataManager.DataType.All);

                    DataManager.Instance.UniversalData = (TypeDict<BaseData>)RogueEssence.Dev.DevHelper.LoadWithLegacySupport(PathMod.ModPath(DataManager.MISC_PATH + "Index.bin"), typeof(TypeDict<BaseData>));
                    RogueEssence.Dev.DevHelper.RunExtraIndexing(DataManager.DataType.All);
                    return;
                }

                if (reserializeIndices != DataManager.DataType.None)
                {
                    DiagManager.Instance.LogInfo("Beginning Reserialization");
                    //we need the datamanager for this, but only while data is hardcoded
                    //TODO: remove when data is no longer hardcoded
                    LuaEngine.InitInstance();
                    DataManager.InitInstance();

                    DataManager.InitDataDirs(PathMod.ModPath(""));
                    RogueEssence.Dev.DevHelper.ReserializeBase();
                    DiagManager.Instance.LogInfo("Reserializing main data");
                    RogueEssence.Dev.DevHelper.Reserialize(reserializeIndices);
                    DiagManager.Instance.LogInfo("Reserializing map data");
                    if ((reserializeIndices & DataManager.DataType.Zone) != DataManager.DataType.None)
                    {
                        RogueEssence.Dev.DevHelper.ReserializeData<Map>(DataManager.DATA_PATH + "Map/", DataManager.MAP_EXT);
                        RogueEssence.Dev.DevHelper.ReserializeData<GroundMap>(DataManager.DATA_PATH + "Ground/", DataManager.GROUND_EXT);
                    }
                    DiagManager.Instance.LogInfo("Reserializing indices");
                    RogueEssence.Dev.DevHelper.RunIndexing(reserializeIndices);

                    DataManager.Instance.UniversalData = (TypeDict<BaseData>)RogueEssence.Dev.DevHelper.LoadWithLegacySupport(PathMod.ModPath(DataManager.MISC_PATH + "Index.bin"), typeof(TypeDict<BaseData>));
                    RogueEssence.Dev.DevHelper.RunExtraIndexing(reserializeIndices);
                    return;
                }

                if (convertIndices != DataManager.DataType.None)
                {
                    //we need the datamanager for this, but only while data is hardcoded
                    //TODO: remove when data is no longer hardcoded
                    LuaEngine.InitInstance();
                    DataManager.InitInstance();
                    DataManager.InitDataDirs(PathMod.ModPath(""));
                    RogueEssence.Dev.DevHelper.RunIndexing(convertIndices);

                    DataManager.Instance.UniversalData = (TypeDict<BaseData>)RogueEssence.Dev.DevHelper.LoadWithLegacySupport(PathMod.ModPath(DataManager.MISC_PATH + "Index.bin"), typeof(TypeDict<BaseData>));
                    RogueEssence.Dev.DevHelper.RunExtraIndexing(convertIndices);
                    return;
                }

                //For exporting to data
                if (dump > DataManager.DataType.None)
                {
                    LuaEngine.InitInstance();

                    //before reserializing, reserialize skill and monsters, and delete all data
                    dump = addTypeDependency(dump, DataManager.DataType.Element, DataManager.DataType.Item);
                    dump = addTypeDependency(dump, DataManager.DataType.Monster, DataManager.DataType.Item);
                    dump = addTypeDependency(dump, DataManager.DataType.Status, DataManager.DataType.Item);
                    dump = addTypeDependency(dump, DataManager.DataType.MapStatus, DataManager.DataType.Item);

                    dump = addTypeDependency(dump, DataManager.DataType.Rank, DataManager.DataType.Zone);
                    dump = addTypeDependency(dump, DataManager.DataType.Monster, DataManager.DataType.Zone);
                    dump = addTypeDependency(dump, DataManager.DataType.AI, DataManager.DataType.Zone);

                    {
                        DataManager.InitInstance();
                        DataInfo.AddEditorOps();
                        DataInfo.AddSystemFX();
                        DataInfo.AddUniversalEvent();
                        DataInfo.AddUniversalData();

                        if ((dump & DataManager.DataType.Element) != DataManager.DataType.None)
                            ElementInfo.AddElementData();
                        if ((dump & DataManager.DataType.GrowthGroup) != DataManager.DataType.None)
                            GrowthInfo.AddGrowthGroupData();
                        if ((dump & DataManager.DataType.SkillGroup) != DataManager.DataType.None)
                            SkillGroupInfo.AddSkillGroupData();
                        if ((dump & DataManager.DataType.Emote) != DataManager.DataType.None)
                            EmoteInfo.AddEmoteData();
                        if ((dump & DataManager.DataType.AI) != DataManager.DataType.None)
                            AIInfo.AddAIData();
                        if ((dump & DataManager.DataType.Tile) != DataManager.DataType.None)
                            TileInfo.AddTileData();
                        if ((dump & DataManager.DataType.Terrain) != DataManager.DataType.None)
                            TileInfo.AddTerrainData();
                        if ((dump & DataManager.DataType.Rank) != DataManager.DataType.None)
                            RankInfo.AddRankData();
                        if ((dump & DataManager.DataType.Skin) != DataManager.DataType.None)
                            SkinInfo.AddSkinData();

                        if ((dump & DataManager.DataType.Monster) != DataManager.DataType.None)
                            MonsterInfo.AddMonsterData();

                        if ((dump & DataManager.DataType.Skill) != DataManager.DataType.None)
                        {
                            //SkillInfo.AddUnreleasedMoveData();
                            //SkillInfo.AddMoveData(564);
                        }

                        if ((dump & DataManager.DataType.Intrinsic) != DataManager.DataType.None)
                            IntrinsicInfo.AddIntrinsicData();
                        if ((dump & DataManager.DataType.Status) != DataManager.DataType.None)
                            StatusInfo.AddStatusData();
                        if ((dump & DataManager.DataType.MapStatus) != DataManager.DataType.None)
                            MapStatusInfo.AddMapStatusData();

                        DataManager.DataType reserializeType = DataManager.DataType.All;
                        reserializeType ^= DataManager.DataType.Zone;
                        reserializeType ^= DataManager.DataType.Item;
                        RogueEssence.Dev.DevHelper.RunIndexing(reserializeType);
                    }

                    {
                        DataManager.InitInstance();
                        DataManager.Instance.InitData();

                        if ((dump & DataManager.DataType.Item) != DataManager.DataType.None)
                            ItemInfo.AddItemData();

                        if ((dump & DataManager.DataType.Zone) != DataManager.DataType.None)
                        {
                            //GraphicsManager.InitParams();
                            //using (GameBase game = new GameBase())
                            //{
                            //    GraphicsManager.InitSystem(game.GraphicsDevice);
                            //    GraphicsManager.InitStatic();

                                MapInfo.AddGroundData(MapInfo.MapNames[3]);
                                ZoneInfo.AddZoneData();
                            //}
                        }

                        DataManager.DataType reserializeType = DataManager.DataType.None;
                        reserializeType |= DataManager.DataType.Zone;
                        reserializeType |= DataManager.DataType.Item;
                        RogueEssence.Dev.DevHelper.RunIndexing(reserializeType);

                        RogueEssence.Dev.DevHelper.RunExtraIndexing(dump);
                    }
                    return;
                }

                if (demo)
                {
                    LuaEngine.InitInstance();
                    RogueEssence.Dev.DevHelper.DemoData(DataManager.DATA_PATH + "Zone/", DataManager.DATA_EXT, typeof(ZoneData));
                    //RogueEssence.Dev.DevHelper.DemoData(DataManager.DATA_PATH + "Map/", DataManager.MAP_EXT);
                    RogueEssence.Dev.DevHelper.DemoData(DataManager.DATA_PATH + "Ground/", DataManager.GROUND_EXT, typeof(GroundMap));
                    RogueEssence.Dev.DevHelper.RunIndexing(DataManager.DataType.Zone);
                }


            }
            catch (Exception ex)
            {
                DiagManager.Instance.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// If dep1 is being re-dumped, dep2 must be re-dumped too.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="depended"></param>
        /// <param name="dependsOn"></param>
        /// <returns></returns>
        private static DataManager.DataType addTypeDependency(DataManager.DataType type, DataManager.DataType depended, DataManager.DataType dependsOn)
        {
            if ((type & depended) != DataManager.DataType.None)
                type |= dependsOn;
            //if ((type & dependsOn) != DataManager.DataType.None)
            //    type |= depended;
            return type;
        }
    }
}
