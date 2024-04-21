using System;
using System.Collections.Generic;
using System.IO;
using RogueElements;
using RogueEssence.Data;
using RogueEssence.Content;
using RogueEssence.Dungeon;
using RogueEssence.Ground;
using RogueEssence.LevelGen;
using System.Text.RegularExpressions;
using System.Linq;
using RogueEssence;
using RogueEssence.Dev;
using PMDC.Data;
using PMDC.Dungeon;
using DataGenerator.Data;


namespace DataGenerator.Dev
{
    public static class Localization
    {
        public static void PrintExclusiveNameStringTable()
        {
            string path = GenPath.TL_PATH + "ExclusiveItemType.txt";


            Dictionary<string, (string, LocalText)> rows = new Dictionary<string, (string, LocalText)>();
            List<string> orderedKeys = new List<string>();
            HashSet<string> languages = new HashSet<string>();

            //names
            foreach (ExclusiveItemType nameType in Enum.GetValues(typeof(ExclusiveItemType)).Cast<ExclusiveItemType>())
            {
                LocalText nameText = AutoItemInfo.GetLocalExpression(nameType, false);
                updateWorkingLists(rows, orderedKeys, languages, typeof(ExclusiveItemType).Name+ "."+nameType, "", nameText);
            }

            printLocalizationRows(path, languages, orderedKeys, rows);
        }

        public static void PrintExclusiveDescStringTable()
        {
            string path = GenPath.TL_PATH + "ExclusiveItemEffect.txt";


            Dictionary<string, (string, LocalText)> rows = new Dictionary<string, (string, LocalText)>();
            List<string> orderedKeys = new List<string>();
            HashSet<string> languages = new HashSet<string>();

            foreach (ExclusiveItemEffect descType in Enum.GetValues(typeof(ExclusiveItemEffect)).Cast<ExclusiveItemEffect>())
            {
                ItemData item = new ItemData();
                item.UseEvent.Element = "none";
                AutoItemInfo.FillExclusiveEffects("", item, new List<LocalText>(), false, descType, new object[0], false);
                updateWorkingLists(rows, orderedKeys, languages, typeof(ExclusiveItemEffect).Name + "." + descType, item.Comment, item.Desc);
            }

            printLocalizationRows(path, languages, orderedKeys, rows);
        }

        //these methods are meant to add to existing tables, which have been preconverted.
        //the asterisk safeguards should be removed when processing these entries
        //initially, there will be an official-word english column.
        //start with highlighting all cells in red
        //you must go through the full spreadsheet and find all cases where the english column does match with the actual description, or is better than what you have.
        //in those cases, un-highlight those rows to denote that they have been approved.
        //for entries with base-blank descriptions, if nothing can be thought up, delete the row
        //*for items, existing names and descriptions must be entered manually*
        //finally, delete the english descriptions
        //when importing, must first run a macro on the spreadsheet that erases all highlighted translations.

        public static void PrintNamedStringTable(DataManager.DataType dataType, GetNamedData getData)
        {
            printNamedDataTable(GenPath.TL_PATH + dataType.ToString() + ".txt", DataManager.Instance.DataIndices[dataType], getData);
        }

        public static void PrintDescribedStringTable(DataManager.DataType dataType, GetDescribedData getData)
        {
            printDescribedDataTable(GenPath.TL_PATH + dataType.ToString() + ".txt", DataManager.Instance.DataIndices[dataType], getData);
        }

        
        public static void PrintItemStringTable()
        {
            DataManager.DataType dataType = DataManager.DataType.Item;
            string path = GenPath.TL_PATH + dataType.ToString() + ".txt";

            Dictionary<string, string> repeatCheck = new Dictionary<string, string>();

            Dictionary<string, (string, LocalText)> rows = new Dictionary<string, (string, LocalText)>();
            List<string> orderedKeys = new List<string>();
            HashSet<string> languages = new HashSet<string>();

            EntryDataIndex index = DataManager.Instance.DataIndices[dataType];
            List<string> dataKeys = index.GetOrderedKeys(true);
            foreach (string key in dataKeys)
            {
                ItemData data = DataManager.Instance.GetItem(key);

                //skip blank entries
                if (data.Name.DefaultText == "")
                    continue;
                if (repeatCheck.ContainsKey(data.Name.DefaultText))
                    Console.WriteLine("Item name \"{0}\" repeated between {1} and {2}", data.Name.DefaultText, repeatCheck[data.Name.DefaultText], key);
                repeatCheck[data.Name.DefaultText] = key;

                //skip TMs
                if (data.UsageType == ItemData.UseType.Learn)
                    continue;
                //skip autocalculated exclusive item NAMES
                if (data.ItemStates.Contains<MaterialState>() && data.ItemStates.GetWithDefault<ExclusiveState>().ItemType != ExclusiveItemType.None)
                    continue;
                //TODO: get these type names via reflection
                updateWorkingLists(rows, orderedKeys, languages, index.Get(key).SortOrder.ToString("D4") + "-" + key + "-" + 0.ToString("D4") + "|data.Name", data.Comment, data.Name);

                //skip ALL exclusive item DESCRIPTIONS because they are guaranteed autocalculated
                if (data.ItemStates.Contains<MaterialState>())
                    continue;
                updateWorkingLists(rows, orderedKeys, languages, index.Get(key).SortOrder.ToString("D4") + "-" + key + "-" + 1.ToString("D4") + "|data.Desc", "", data.Desc);
            }

            printLocalizationRows(path, languages, orderedKeys, rows);
        }


        public delegate IEntryData GetNamedData(string index);

        public delegate IDescribedData GetDescribedData(string index);


        private static void printNamedDataTable(string path, EntryDataIndex index, GetNamedData method)
        {
            Dictionary<string, (string, LocalText)> rows = new Dictionary<string, (string, LocalText)>();
            List<string> orderedKeys = new List<string>();
            HashSet<string> languages = new HashSet<string>();

            List<string> dataKeys = index.GetOrderedKeys(true);
            foreach (string key in dataKeys)
            {
                IEntryData data = method(key);

                //skip blank entries
                if (data.Name.DefaultText == "")
                    continue;

                //TODO: get these type names via reflection
                updateWorkingLists(rows, orderedKeys, languages, index.Get(key).SortOrder.ToString("D4") + "-" + key + "-" + 0.ToString("D4") + "|data.Name", data.Comment, data.Name);
            }

            printLocalizationRows(path, languages, orderedKeys, rows);
        }

        /// <summary>
        /// Most translatable content here are just types with name and desc, so a common function is used to handle them.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="total"></param>
        /// <param name="method"></param>
        private static void printDescribedDataTable(string path, EntryDataIndex index, GetDescribedData method)
        {
            Dictionary<string, (string, LocalText)> rows = new Dictionary<string, (string, LocalText)>();
            List<string> orderedKeys = new List<string>();
            HashSet<string> languages = new HashSet<string>();

            List<string> dataKeys = index.GetOrderedKeys(true);
            foreach(string key in dataKeys)
            {
                IDescribedData data = method(key);

                //skip blank entries
                if (data.Name.DefaultText == "")
                    continue;

                //TODO: get these type names via reflection
                updateWorkingLists(rows, orderedKeys, languages, index.Get(key).SortOrder.ToString("D4") + "-" + key + "-" + 0.ToString("D4") + "|data.Name", data.Comment, data.Name);
                updateWorkingLists(rows, orderedKeys, languages, index.Get(key).SortOrder.ToString("D4") + "-" + key + "-" + 1.ToString("D4") + "|data.Desc", "", data.Desc);
            }

            printLocalizationRows(path, languages, orderedKeys, rows);
        }


        public delegate IEntryData GetTitledData(string filename);

        /// <summary>
        /// Maps/GroundMaps are types with name and title card, so a common function is used to handle them.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="total"></param>
        /// <param name="method"></param>
        public static void PrintTitledDataTable(string dir, string ext, GetTitledData method)
        {
            string path = GenPath.TL_PATH + (new DirectoryInfo(dir)).Name + ".txt";
            List<string> entryNames = new List<string>();
            foreach (string name in PathMod.GetModFiles(dir, "*"+ ext))
                entryNames.Add(Path.GetFileNameWithoutExtension(name));

            Dictionary<string, (string, LocalText)> rows = new Dictionary<string, (string, LocalText)>();
            List<string> orderedKeys = new List<string>();
            HashSet<string> languages = new HashSet<string>();
            for (int ii = 0; ii < entryNames.Count; ii++)
            {
                IEntryData data = method(entryNames[ii]);

                //TODO: get these type names via reflection
                updateWorkingLists(rows, orderedKeys, languages, entryNames[ii] + "-" + 0.ToString("D4") + "|data.Name", data.Comment, data.Name);
            }

            orderedKeys.Sort(StringComparer.Ordinal);
            printLocalizationRows(path, languages, orderedKeys, rows);
        }

        /// <summary>
        /// Zone data is an exceptional case where the names and the floor naming patterns must be translated
        /// </summary>
        /// <param name="path"></param>
        /// <param name="entryNames"></param>
        /// <param name="method"></param>
        public static void PrintZoneStringTable()
        {
            string path = GenPath.TL_PATH + "Zone.txt";
            Dictionary<string, (string, LocalText)> rows = new Dictionary<string, (string, LocalText)>();
            List<string> orderedKeys = new List<string>();
            HashSet<string> languages = new HashSet<string>();

            EntryDataIndex index = DataManager.Instance.DataIndices[DataManager.DataType.Zone];
            List<string> dataKeys = index.GetOrderedKeys(true);
            foreach (string key in dataKeys)
            {
                ZoneData data = DataManager.Instance.GetZone(key);

                int nn = 0;
                updateWorkingLists(rows, orderedKeys, languages, index.Get(key).SortOrder.ToString("D4") + "-" + key + "-" + nn.ToString("D4") + "|data.Name", data.Comment, data.Name);
                for (int jj = 0; jj < data.Segments.Count; jj++)
                {
                    ZoneSegmentBase structure = data.Segments[jj];

                    for (int kk = 0; kk < structure.ZoneSteps.Count; kk++)
                    {
                        FloorNameIDZoneStep postProc = structure.ZoneSteps[kk] as FloorNameIDZoneStep;
                        if (postProc != null)
                        {
                            //TODO: get these type names via reflection
                            nn++;
                            updateWorkingLists(rows, orderedKeys, languages,
                                index.Get(key).SortOrder.ToString("D4") + "-" + key + "-" + nn.ToString("D4") + "|((FloorNameIDZoneStep)data.Segments[" + jj.ToString("D4") + "].ZoneSteps[" + kk.ToString("D4") + "]).Name", "", postProc.Name);
                        }
                    }
                }
            }

            printLocalizationRows(path, languages, orderedKeys, rows);
        }

        private static void updateWorkingLists(Dictionary<string, (string, LocalText)> rows, List<string> orderedKeys, HashSet<string> languages, string key, string comment, LocalText val)
        {
            foreach (string language in val.LocalTexts.Keys)
                languages.Add(language);
            rows.Add(key, (/*comment*/"", val));
            orderedKeys.Add(key);
        }

        private static void printLocalizationRows(string path, HashSet<string> languages, List<string> orderedKeys, Dictionary<string, (string, LocalText)> rows)
        {
            using (StreamWriter file = new StreamWriter(path))
            {
                file.Write("Key\t\tEN");
                foreach (string language in languages)
                    file.Write("\t"+language.ToUpper());
                file.WriteLine();
                foreach(string key in orderedKeys)
                {
                    LocalText text = rows[key].Item2;
                    file.Write(key+"\t"+ rows[key] .Item1+ "\t"+text.DefaultText.Replace("\n", "\\n"));
                    foreach (string language in languages)
                    {
                        file.Write("\t");
                        if (text.LocalTexts.ContainsKey(language))
                            file.Write(text.LocalTexts[language].Replace("\n", "\\n"));
                    }
                    file.WriteLine();
                }
            }
        }
















        //write backs

        public static void WriteTitledDataTable(string path, string ext, GetTitledData method)
        {
            string filename = (new DirectoryInfo(path)).Name;
            List<string> entryNames = new List<string>();
            foreach (string name in PathMod.GetModFiles(path, "*" + ext))
                entryNames.Add(Path.GetFileNameWithoutExtension(name));

            Dictionary<string, LocalText> rows = readLocalizationRows(GenPath.TL_PATH + filename + ".out.txt");
            for (int ii = 0; ii < entryNames.Count; ii++)
            {
                IEntryData data = method(entryNames[ii]);

                data.Name = rows[entryNames[ii] + "-" + 0.ToString("D4") + "|data.Name"];

                DataManager.SaveData(PathMod.ModPath(path + entryNames[ii] + ext), data);
            }
        }

        public static void WriteNamedDataTable(DataManager.DataType dataType)
        {

            Dictionary<string, LocalText> rows = readLocalizationRows(GenPath.TL_PATH + dataType.ToString() + ".out.txt");
            foreach (string key in DataManager.Instance.DataIndices[dataType].GetOrderedKeys(true))
            {
                string tlKey = dataType.ToString() + "-" + key + "|data.Name";
                if (rows.ContainsKey(tlKey))
                {
                    string dir = PathMod.ModPath(DataManager.DATA_PATH + dataType.ToString() + "/" + key + DataManager.DATA_EXT);

                    IEntryData describedData = DataManager.LoadData<IEntryData>(dir);

                    if (describedData.Name.DefaultText != "")
                    {
                        describedData.Name = rows[tlKey];
                        DataManager.SaveData(dir, describedData);
                    }
                }
            }
        }

        public static void CopyNamedData(DataManager.DataType dataType, string from, string to)
        {
            string fromDir = PathMod.ModPath(DataManager.DATA_PATH + dataType.ToString() + "/" + from + DataManager.DATA_EXT);
            string toDir = PathMod.ModPath(DataManager.DATA_PATH + dataType.ToString() + "/" + to + DataManager.DATA_EXT);

            IEntryData fromData = DataManager.LoadData<IEntryData>(fromDir);
            IEntryData toData = DataManager.LoadData<IEntryData>(toDir);

            toData.Name = fromData.Name;
            DataManager.SaveData(toDir, toData);
        }

        public static void WriteDescribedDataTable(DataManager.DataType dataType)
        {

            Dictionary<string, LocalText> rows = readLocalizationRows(GenPath.TL_PATH + dataType.ToString() + ".out.txt");
            foreach(string key in DataManager.Instance.DataIndices[dataType].GetOrderedKeys(true))
            {
                string dir = PathMod.ModPath(DataManager.DATA_PATH + dataType.ToString() + "/" + key + DataManager.DATA_EXT);

                IDescribedData describedData = DataManager.LoadData<IDescribedData>(dir);

                if (describedData.Name.DefaultText != "")
                {
                    int sort = DataManager.Instance.DataIndices[dataType].Get(key).SortOrder;
                    describedData.Name = rows[sort.ToString("D4") + "-" + key + "-" + 0.ToString("D4") + "|data.Name"];
                    describedData.Desc = rows[sort.ToString("D4") + "-" + key + "-" + 1.ToString("D4") + "|data.Desc"];

                    DataManager.SaveData(dir, describedData);
                }
            }
        }

        public static void WriteItemStringTable()
        {
            DataManager.DataType dataType = DataManager.DataType.Item;
            Dictionary<string, LocalText> rows = readLocalizationRows(GenPath.TL_PATH + dataType.ToString() + ".out.txt");
            Dictionary<string, LocalText> skillRows = readLocalizationRows(GenPath.TL_PATH + DataManager.DataType.Skill.ToString() + ".out.txt");
            Dictionary<string, LocalText> specialRows = readLocalizationRows(GenPath.TL_PATH + "Special.out.txt");

            EntryDataIndex itemIndex = DataManager.Instance.DataIndices[dataType];
            EntryDataIndex skillIndex = DataManager.Instance.DataIndices[DataManager.DataType.Skill];
            foreach (string key in itemIndex.GetOrderedKeys(true))
            {
                string dir = PathMod.ModPath(DataManager.DATA_PATH + dataType.ToString() + "/" + key + DataManager.DATA_EXT);

                ItemData data = DataManager.LoadData<ItemData>(dir);

                //skip blank entries
                if (data.Name.DefaultText == "")
                    continue;

                if (data.UsageType == ItemData.UseType.Learn)
                {
                    //autocalculate TM name and descriptions
                    LocalText tmFormatName = specialRows["tmFormatName"];
                    LocalText tmFormatDesc = specialRows["tmFormatDesc"];
                    string moveIndex = data.ItemStates.GetWithDefault<ItemIDState>().ID;
                    LocalText moveName = skillRows[skillIndex.Get(moveIndex).SortOrder.ToString("D4") + "-" + moveIndex + "-" + 0.ToString("D4") + "|data.Name"];
                    data.Name = LocalText.FormatLocalText(tmFormatName, moveName);
                    data.Desc = LocalText.FormatLocalText(tmFormatDesc, moveName);
                }
                else if (data.ItemStates.Contains<MaterialState>())
                {
                    //skip this; exclusive items will be added in AddCalculatedItemData
                    //if (data.ItemStates.Get<ExclusiveState>().ItemType != ExclusiveItemType.None)
                    //{
                    //    //autocalculate exclusive item NAMES
                    //    LocalText exclFormatName = itemTypeRows[typeof(ExclusiveItemType).Name + "." + data.ItemStates.Get<ExclusiveState>().ItemType];
                    //    MonsterData monsterData = DataManager.Instance.GetMonster();
                    //    data.Name = LocalText.FormatLocalText(exclFormatName, monsterData.Name);
                    //}
                    //else
                    //{
                    //    data.Name = rows[ii.ToString("D4") + "-" + 0.ToString("D4") + "|data.Name"];
                    //}
                    ////generate autocalculated item descriptions
                    //LocalText qualityText;
                    //if (/*Species-based*/)
                    //{
                    //    if (/*Allow And-Family*/)
                    //    {

                    //    }
                    //    else//Just list out the names
                    //    {

                    //    }
                    //}
                    //else if (/*Type-based*/)
                    //{

                    //}
                    //data.Desc = rows[ii.ToString("D4") + "-" + 1.ToString("D4") + "|data.Desc"];
                }
                else
                {
                    //TODO: get these type names via reflection
                    data.Name = rows[itemIndex.Get(key).SortOrder.ToString("D4") + "-" + key + "-" + 0.ToString("D4") + "|data.Name"];
                    data.Desc = rows[itemIndex.Get(key).SortOrder.ToString("D4") + "-" + key + "-" + 1.ToString("D4") + "|data.Desc"];
                }

                DataManager.SaveData(dir, data);
            }
        }

        public static void WriteZoneStringTable()
        {
            DataManager.DataType dataType = DataManager.DataType.Zone;
            Dictionary<string, LocalText> rows = readLocalizationRows(GenPath.TL_PATH + dataType.ToString() + ".out.txt");
            foreach (string key in DataManager.Instance.DataIndices[dataType].GetOrderedKeys(false))
            {
                string dir = PathMod.ModPath(DataManager.DATA_PATH + dataType.ToString() + "/" + key + DataManager.DATA_EXT);

                ZoneData data = DataManager.LoadData<ZoneData>(dir);

                int nn = 0;
                int sort = DataManager.Instance.DataIndices[dataType].Get(key).SortOrder;
                data.Name = rows[sort.ToString("D4") + "-" + key + "-" + 0.ToString("D4") + "|data.Name"];
                for (int jj = 0; jj < data.Segments.Count; jj++)
                {
                    ZoneSegmentBase structure = data.Segments[jj];
                    for (int kk = 0; kk < structure.ZoneSteps.Count; kk++)
                    {
                        FloorNameIDZoneStep postProc = structure.ZoneSteps[kk] as FloorNameIDZoneStep;
                        if (postProc != null)
                        {
                            //TODO: get these type names via reflection
                            nn++;
                            postProc.Name = rows[sort.ToString("D4") + "-" + key + "-" + nn.ToString("D4") + "|((FloorNameIDZoneStep)data.Segments[" + jj.ToString("D4") + "].ZoneSteps[" + kk.ToString("D4") + "]).Name"];
                        }
                    }
                }

                DataManager.SaveData(dir, data);
            }
        }

        public static Dictionary<string, LocalText> readLocalizationRows(string path)
        {
            Dictionary<string, LocalText> rows = new Dictionary<string, LocalText>();
            using (StreamReader inStream = new StreamReader(path))
            {
                string[] langs = inStream.ReadLine().Split('\t');
                while (!inStream.EndOfStream)
                {
                    string[] cols = inStream.ReadLine().Split('\t');
                    string key = cols[0];
                    LocalText text = new LocalText(cols[2]);
                    for (int ii = 3; ii < langs.Length; ii++)
                    {
                        if (ii < cols.Length && !String.IsNullOrWhiteSpace(cols[ii]))
                            text.LocalTexts.Add(langs[ii].ToLower(), cols[ii]);
                    }
                    rows[key] = text;
                }
            }
            return rows;
        }

    }
}
