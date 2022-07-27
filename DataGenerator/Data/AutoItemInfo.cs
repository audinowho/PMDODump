using System;
using System.Collections.Generic;
using RogueEssence.Dungeon;
using RogueEssence.Ground;
using RogueEssence.LevelGen;
using RogueEssence;
using RogueEssence.Data;
using PMDC.Dungeon;
using PMDC;
using PMDC.Data;
using System.Text;
using RogueEssence.Content;
using DataGenerator.Dev;
using System.Linq;
using System.IO;
using RogueEssence.Script;

namespace DataGenerator.Data
{
    public enum ExclusiveItemEffect
    {
        None,
        TypeStatBonus,//1*
        TypeSpeedBoost,//3*
        TypeGroupWeaknessReduce,//5*
        TypeBulldozer,//4*
        SuperCrit,//3*
        NVECrit,//2*
        Nontraitor,//1*
        WaterTerrain,//2*
        LavaTerrain,//2*
        AllTerrain,//4*
        Wallbreaker,//5* breaks walls with MOVES
        GapFiller,//5* fills gaps with MOVES
        PPBoost,//2*
        DeepBreather,//1*
        PracticeSwinger,//1*
        MisfortuneMirror,//1*
        CounterBasher,//3* reflects all normal attacks and projectiles
        DoubleAttacker,//2*
        MasterHurler,//2* longtoss+pierce
        AllyReviver,//5* occurs on floor start
        AllyReviverBattle,//5* occurs on defeating a foe
        PressurePlus,//3*
        StatusOnAttack,//3* can inflict status with normal attack (and projectile?): self and foe
        KnockbackOnAttack,//3*
        ThrowOnAttack,//3*
        SureHitAttacker,//1*
        SpecialAttacker,//1*
        StatusImmune,//3*
        StatDropImmune,//2*
        SleepWalker,//2* can walk while asleep!
        ChargeWalker,//3* can walk while charging!
        StairSensor,//4* diagonals are better approximated...
        AcuteSniffer,//2*
        MapSurveyor,//5* nerfed in some way- reveals layout within 20 tiles away
        XRay,//5*
        WeaknessPayback,//4* inflict status
        WarpPayback,//4* warp
        LungeAttack,//3*
        WideAttack,//2*
        ExplosiveAttack,//5*
        TrapBuster,//4* destroy traps when attacking
        ExplosionGuard,//3*
        WandMaster,//4* wands become arc throw
        WandSpread,//5* wands spread out
        MultiRayShot,//5* turns linear projectiles into multi-ray projectiles at the cost of destroying them
        RoyalVeil,//4* restores others' HP when at full HP
                  //more effects that work on allies...
        Celebrate,//4* extra turn after defeating an enemy
        Absorption,//3* restore HP after defeating an enemy
        ExcessiveForce,//3* deal AOE damage after defeating an enemy
        Anchor,//1*
        BarrageGuard,//2*
        BetterOdds,//3* cannot miss and will crit if the move has 1 PP left
        ClutchPerformer,//4* dodges moves in a pinch
        DistanceDodge,//4* dodges faraway moves
        CloseDodge,//4* dodges close-up moves
        FastHealer,//5*
        SelfCurer,//3*
        StatusMirror,//3* gives status (not stats) to the target in front.  only curable status and transferrable status
        StatMirror,//2*
        ErraticAttacker,//2*
        ErraticDefender,//2*
        FastFriend,//2* meta-effect; make exceptions for this
        CoinWatcher,//3* meta-effect; make exceptions for this
        HiddenStairFinder,//3* meta-effect; make exceptions for this
        ChestFinder,//3* meta-effect; make exceptions for this
        ShopFinder,//3* meta-effect; make exceptions for this
        SecondSTAB,//5* increase the power of X-type moves
        TypedAttack,//4* changes regular attack to user's type
        GapProber,//1*
        PassThroughAttacker,//1*
        WeatherProtection,
        RemoveAbilityAttack,
        CheekPouch,
        HealInWater,
        HealInLava,
        HealOnNewFloor,
        EndureCategory,
        EndureType,
        SpikeDropper,
        NoStatusInWeather,
        DeepBreatherPlus,//3*
        WeaknessReduce,//5*
        Gratitude,//2*
        HitAndRun,//2*
        StatusOnCategoryHit,//3*
        StatusOnCategoryUse,//5*
        MapStatusOnCategoryUse,//5*
        DoubleDash,//4*
        StatusSplash,//5*
        DevolveOnAttack,//3* can inflict devolve with normal attack (and projectile?): self and foe
        ProjectileAttack,//4*
        MetronomePlus,//5*
        AttackRangeInWeather,//4*
        PPSaver,
        CelebrateCure,
        TypeBodyguard,
        StatOnCategoryUse,
        StatusOnTypeHit,
        SweetDreams,
        ChanceStatusOnTypeHit,
        ChanceStatOnTypeHit,
        ChanceStatusOnCategoryHit,
    }
    public class AutoItemInfo
    {

        private static Dictionary<string, Dictionary<string, string>> stringsAll;
        private static Dictionary<string, LocalText> specialRows;
        private static Dictionary<string, LocalText> specificItemRows;
        private static Dictionary<string, LocalText> itemTypeRows;
        private static Dictionary<string, LocalText> itemEffectRows;

        public static void InitStringsAll()
        {
            stringsAll = new Dictionary<string, Dictionary<string, string>>();
            stringsAll[""] = Text.LoadStringResx(PathMod.ModPath("Strings/strings.resx"));
            foreach (string code in Text.SupportedLangs)
            {
                if (code != "en")
                    stringsAll[code] = Text.LoadStringResx(PathMod.ModPath("Strings/strings." + code + ".resx"));
            }

            specialRows = Localization.readLocalizationRows(GenPath.TL_PATH  + "Special.out.txt");
            specificItemRows = Localization.readLocalizationRows(GenPath.TL_PATH + "Item.out.txt");
            itemTypeRows = Localization.readLocalizationRows(GenPath.TL_PATH + "ExclusiveItemType.out.txt");
            itemEffectRows = Localization.readLocalizationRows(GenPath.TL_PATH + "ExclusiveItemEffect.out.txt");
        }
        //this class will not be auto-generated
        //it will only contain english translations
        //it will ask datastringinfo for translations
        //when scraping translations, the algorithm will iterate the local expressions and effect list names separately
        //it will return with localtexts and then send them over to translation
        //when read back it will update datastringinfo for those enums
        //in this case, we can send arguments for string formatting too
        //since the callers of getLocalExpression for the purpose of string scraping can just use {0}, {1}, etc?


        //when it comes to custom specific item names,
        //it will go through all item names in the specific item range and print to list only if the exclusive item type is none


        public static LocalText GetLocalExpression(ExclusiveItemType type, bool translate)
        {
            if (!translate)
            {
                LocalText formatName = new LocalText("{0} " + type.ToString());
                return formatName;
            }
            else
            {
                LocalText formatName = itemTypeRows[typeof(ExclusiveItemType).Name + "." + type];
                return formatName;
            }
        }

        public static void CreateContentLists()
        {
            List<string> itemTypes = new List<string>();
            List<string> effectTypes = new List<string>();
            Dictionary<string, string> effectDescriptions = new Dictionary<string, string>();
            List<string> stats = new List<string>();
            List<string> monsters = new List<string>();
            List<string> elements = new List<string>();
            List<string> statuses = new List<string>();
            List<string> mapStatuses = new List<string>();
            List<string> categories = new List<string>();


            foreach (ExclusiveItemType nameType in Enum.GetValues(typeof(ExclusiveItemType)).Cast<ExclusiveItemType>())
            {
                int rarity = 0;
                if (nameType >= ExclusiveItemType.Claw)
                    rarity++;
                if (nameType >= ExclusiveItemType.Tag)
                    rarity++;
                if (nameType >= ExclusiveItemType.Blade)
                    rarity++;
                itemTypes.Add(rarity + "* " + nameType.ToString());
            }
            itemTypes.Sort();

            foreach (ExclusiveItemEffect descType in Enum.GetValues(typeof(ExclusiveItemEffect)).Cast<ExclusiveItemEffect>())
            {


                ItemData item = new ItemData();
                item.UseEvent.Element = "none";
                AutoItemInfo.FillExclusiveEffects(item, new List<LocalText>(), false, descType, new object[0], false);


                effectTypes.Add(item.Rarity + "* "+ ((int)descType).ToString("D3") + ": " + descType.ToString());
                effectDescriptions.Add(item.Rarity + "* " + ((int)descType).ToString("D3") + ": " + descType.ToString(), item.Desc.DefaultText);
            }
            effectTypes.Sort();

            foreach (Stat statType in Enum.GetValues(typeof(Stat)).Cast<Stat>())
                stats.Add(((int)statType).ToString("D3") + ": " + statType.ToString());


            for (int ii = 0; ii < DataManager.Instance.DataIndices[DataManager.DataType.Monster].Count; ii++)
                monsters.Add((ii).ToString("D3") + ": " + DataManager.Instance.DataIndices[DataManager.DataType.Monster].Entries[ii.ToString()].Name.DefaultText);

            foreach(string key in DataManager.Instance.DataIndices[DataManager.DataType.Element].Entries.Keys)
                elements.Add(key + ": " + DataManager.Instance.DataIndices[DataManager.DataType.Element].Entries[key].Name.DefaultText);

            foreach (string key in DataManager.Instance.DataIndices[DataManager.DataType.Status].Entries.Keys)
            {
                StatusData data = DataManager.Instance.GetStatus(key);
                if (data.MenuName && data.Name.DefaultText != "" && data.StatusStates.Contains<TransferStatusState>())
                    statuses.Add(key + ": " + data.Name.DefaultText);
            }

            foreach (string key in DataManager.Instance.DataIndices[DataManager.DataType.MapStatus].Entries.Keys)
            {
                MapStatusData data = DataManager.Instance.GetMapStatus(key);
                if (!data.DefaultHidden && data.Name.DefaultText != "")
                    mapStatuses.Add(key + ": " + data.Name.DefaultText);
            }

            foreach (BattleData.SkillCategory statType in Enum.GetValues(typeof(BattleData.SkillCategory)).Cast<BattleData.SkillCategory>())
                categories.Add(((int)statType).ToString("D3") + ": " + statType.ToString());


            string path = GenPath.ITEM_PATH  + "PreAutoItem.txt";
            using (StreamWriter file = new StreamWriter(path))
            {
                file.WriteLine("ItemType\tEffectType\tStat\tMonster\tElement\tStatus\tMapStatus\tCategory");
                int ii = 0;
                bool completed = false;
                while (!completed)
                {
                    completed = true;

                    List<string> row = new List<string>();

                    if (ii < itemTypes.Count)
                    {
                        row.Add(itemTypes[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    if (ii < effectTypes.Count){
                        row.Add(effectTypes[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    if (ii < stats.Count){
                        row.Add(stats[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    if (ii < monsters.Count){
                        row.Add(monsters[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    if (ii < elements.Count){
                        row.Add(elements[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    if (ii < statuses.Count){
                        row.Add(statuses[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    if (ii < mapStatuses.Count)
                    {
                        row.Add(mapStatuses[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    if (ii < categories.Count)
                    {
                        row.Add(categories[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    file.WriteLine(String.Join('\t', row.ToArray()));
                    ii++;
                }
            }
            path = GenPath.ITEM_PATH  + "AutoItemRef.txt";
            using (StreamWriter file = new StreamWriter(path))
            {
                file.WriteLine("EffectType\tDescription");
                for(int ii = 0; ii < effectTypes.Count; ii++)
                {
                    file.WriteLine(effectTypes[ii] + '\t' + effectDescriptions[effectTypes[ii]]);
                }
            }

            List<List<int>> evoTrees = new List<List<int>>();
            List<bool> branchedEvo = new List<bool>();
            HashSet<int> traversed = new HashSet<int>();
            for (int ii = 0; ii < DataManager.Instance.DataIndices[DataManager.DataType.Monster].Count; ii++)
            {
                if (traversed.Contains(ii))
                    continue;

                int firstStage = ii;
                MonsterData data = DataManager.Instance.GetMonster(firstStage);
                int prevo = data.PromoteFrom;
                while (prevo > -1)
                {
                    firstStage = prevo;
                    data = DataManager.Instance.GetMonster(firstStage);
                    prevo = data.PromoteFrom;
                }
                List<string> dexNums = new List<string>();
                dexNums.Add(firstStage.ToString());
                bool branched = FindEvos(dexNums, data);
                List<int> dexNumInt = new List<int>();
                foreach (string dexNum in dexNums)
                    dexNumInt.Add(Int32.Parse(dexNum));
                evoTrees.Add(dexNumInt);
                branchedEvo.Add(branched);
                foreach (string dexNum in dexNums)
                    traversed.Add(Int32.Parse(dexNum));
            }

            path = GenPath.ITEM_PATH  + "EvoTreeRef.txt";
            using (StreamWriter file = new StreamWriter(path))
            {
                file.WriteLine("User\tEvo\tIndex\tRarity");
                for (int ii = 0; ii < evoTrees.Count; ii++)
                {
                    List<int> evoTree = evoTrees[ii];
                    if (branchedEvo[ii])
                    {
                        for (int jj = evoTree.Count - 1; jj >= 0; jj--)
                            file.WriteLine(monsters[evoTree[jj]] + "\tFALSE\t"+(char)('A'+ (evoTree.Count - 1 - jj)) +"\t5*");
                    }
                    else if (evoTree.Count == 3)
                    {
                        file.WriteLine(monsters[evoTree[2]] + "\tTRUE\tA\t5*");
                        file.WriteLine(monsters[evoTree[1]] + "\tTRUE\tB\t4*");
                        file.WriteLine(monsters[evoTree[0]] + "\tTRUE\tC\t1-2*");
                        file.WriteLine(monsters[evoTree[0]] + "\tTRUE\tD\t1-2*");
                        file.WriteLine(monsters[evoTree[0]] + "\tTRUE\tE\t2-3*");
                        file.WriteLine(monsters[evoTree[1]] + "\tTRUE\tF\t2-3*");
                    }
                    else if (evoTree.Count == 2)
                    {
                        file.WriteLine(monsters[evoTree[1]] + "\tTRUE\tA\t5*");
                        file.WriteLine(monsters[evoTree[1]] + "\tTRUE\tB\t4*");
                        file.WriteLine(monsters[evoTree[0]] + "\tTRUE\tC\t1-2*");
                        file.WriteLine(monsters[evoTree[0]] + "\tTRUE\tD\t1-2*");
                        file.WriteLine(monsters[evoTree[0]] + "\tTRUE\tE\t2-3*");
                        file.WriteLine(monsters[evoTree[1]] + "\tTRUE\tF\t2-3*");
                    }
                    else if (evoTree.Count == 1)
                    {
                        file.WriteLine(monsters[evoTree[0]] + "\tFALSE\tA\t5*");
                        file.WriteLine(monsters[evoTree[0]] + "\tFALSE\tB\t4*");
                        file.WriteLine(monsters[evoTree[0]] + "\tFALSE\tC\t1-2*");
                        file.WriteLine(monsters[evoTree[0]] + "\tFALSE\tD\t1-2*");
                        file.WriteLine(monsters[evoTree[0]] + "\tFALSE\tE\t2-3*");
                        file.WriteLine(monsters[evoTree[0]] + "\tFALSE\tF\t2-3*");
                    }
                }
            }
        }


        public static List<string[]> LoadItemRows(string path)
        {
            List<string[]> rows = new List<string[]>();
            using (StreamReader inStream = new StreamReader(path))
            {
                string[] header = inStream.ReadLine().Split('\t');
                while (!inStream.EndOfStream)
                {
                    string[] row = inStream.ReadLine().Split('\t');
                    rows.Add(row);
                }
            }
            return rows;
        }

        public static void WriteExclusiveItems(int init_idx, bool translate)
        {
            int incompleteLeft = 0;
            List<(int item, int[] reqItem)> specific_tradeables = new List<(int, int[])>();
            List<(int item, int dex, int reqCount)> random_tradeables = new List<(int, int, int)>();
            Dictionary<int, int> dex_map = new Dictionary<int, int>();

            int prev_start = init_idx;
            List<string> running_tradeables = new List<string>();

            //load from generated csv
            if (File.Exists(GenPath.ITEM_PATH  + "ExclusiveItem.out.txt"))
            {
                List<string[]> rows = AutoItemInfo.LoadItemRows(GenPath.ITEM_PATH  + "ExclusiveItem.out.txt");
                for (int ii = 0; ii < rows.Count; ii++)
                {
                    string[] row = rows[ii];
                    int item_idx = ii + init_idx;


                    string customName = row[1].Trim();
                    ExclusiveItemType exclType = (ExclusiveItemType)Enum.Parse(typeof(ExclusiveItemType), row[0].Substring(2));
                    ExclusiveItemEffect exclEffect = (ExclusiveItemEffect)Int32.Parse(row[10].Substring(3, 3));
                    int primaryDex = Int32.Parse(row[2].Substring(0, 3));
                    dex_map[item_idx] = primaryDex;

                    string[] rarityStr = row[9].Substring(0, row[9].Length-1).Split('-');
                    int minRarity = Int32.Parse(rarityStr[0]);
                    int maxRarity = minRarity;
                    if (rarityStr.Length > 1)
                        maxRarity = Int32.Parse(rarityStr[1]);


                    List<object> args = new List<object>();
                    if (row[11] != "")
                        args.Add(Int32.Parse(row[11].Substring(0, 3)));
                    if (row[12] != "")
                        args.Add(Int32.Parse(row[12].Substring(0, 3)));
                    if (row[13] != "")
                        args.Add(new Stat[] { (Stat)Int32.Parse(row[13].Substring(0, 3)) });
                    if (row[14] != "")
                        args.Add(Int32.Parse(row[14].Substring(0, 3)));
                    if (row[15] != "")
                        args.Add((BattleData.SkillCategory)Int32.Parse(row[15].Substring(0, 3)));
                    if (row[16] != "")
                        args.Add(Int32.Parse(row[16]));
                    if (row[17] != "")
                        args.Add(Int32.Parse(row[17].Substring(0, 3)));

                    List<int> familyStarts = new List<int>();
                    if (row[4] != "")
                    {
                        string[] startDexes = row[4].Split(',');
                        foreach (string startDex in startDexes)
                        {
                            string cutoff = startDex.Trim().Substring(0, 3);
                            familyStarts.Add(Int32.Parse(cutoff));
                        }
                    }
                    else
                        familyStarts.Add(primaryDex);

                    bool includeFamily = row[3] == "True";
                    List<string> dexNums = new List<string>();
                    foreach (int dex in familyStarts)
                    {
                        if (includeFamily)
                        {
                            string firstStage = dex.ToString();
                            MonsterData data = DataManager.Instance.GetMonster(firstStage);
                            int prevo = data.PromoteFrom;
                            while (prevo > -1)
                            {
                                firstStage = prevo.ToString();
                                data = DataManager.Instance.GetMonster(firstStage);
                                prevo = data.PromoteFrom;
                            }
                            dexNums.Add(firstStage);
                            AutoItemInfo.FindEvos(dexNums, data);
                        }
                        else
                            dexNums.Add(dex.ToString());
                    }

                    ItemData item = new ItemData();
                    item.UseEvent.Element = "none";

                    if (exclType != ExclusiveItemType.None && customName != "")
                        Console.WriteLine(String.Format("Item {0} found with both name \"{1}\" and type {2}.", item_idx, customName, exclType));

                    AutoItemInfo.FillExclusiveData(item_idx, item, "", customName, exclType, exclEffect, args.ToArray(), primaryDex.ToString(), dexNums, translate, includeFamily);

                    item.Rarity = Math.Clamp(item.Rarity, minRarity, maxRarity);
                    item.Sprite = "Box_Yellow";
                    item.Icon = 10;
                    item.Price = 800 * item.Rarity;
                    item.UsageType = ItemData.UseType.None;
                    item.ItemStates.Set(new MaterialState());
                    item.BagEffect = true;
                    item.CannotDrop = true;


                    if (item.Name.DefaultText.StartsWith("**"))
                        item.Name.DefaultText = item.Name.DefaultText.Replace("*", "");
                    else if (item.Name.DefaultText != "")
                        item.Released = true;

                    if (item.Name.DefaultText != "" && !item.Released)
                        incompleteLeft++;

                    DataManager.SaveData(item_idx.ToString(), DataManager.DataType.Item.ToString(), item);

                    if (item.Released)
                    {
                        string trade_in = row[8];
                        running_tradeables.Add(trade_in);
                    }

                    bool reload = false;
                    if (ii == rows.Count - 1)
                        reload = true;
                    else if (rows[ii + 1][7] == "A")
                        reload = true;
                    else if (rows[ii + 1][7][0] != rows[ii][7][0] + 1)
                        throw new Exception("Out of order labeling!");

                    if (reload)
                    {
                        int prev_tradeable_count = specific_tradeables.Count;
                        if (running_tradeables.Count == 0)
                        {

                        }
                        else if (running_tradeables[0] == "*")
                        {
                            //autocalculate normal case
                            if (running_tradeables.Count == 4)
                            {
                                //C+D+B=A
                                addSpecificTradeable(specific_tradeables, prev_start, 'A', 'C', 'D', 'B');
                                //C+D=B
                                addSpecificTradeable(specific_tradeables, prev_start, 'B', 'C', 'D');
                            }
                            else if (running_tradeables.Count == 5)
                            {
                                //E+B=A
                                addSpecificTradeable(specific_tradeables, prev_start, 'A', 'E', 'B');
                                //C+D=E
                                addSpecificTradeable(specific_tradeables, prev_start, 'E', 'C', 'D');

                            }
                            else if (running_tradeables.Count == 6)
                            {
                                //E+F+B=A
                                addSpecificTradeable(specific_tradeables, prev_start, 'A', 'E', 'F', 'B');
                                //E+F=B
                                addSpecificTradeable(specific_tradeables, prev_start, 'B', 'E', 'F');
                                //C+D=E
                                addSpecificTradeable(specific_tradeables, prev_start, 'E', 'C', 'D');
                            }
                        }
                        else if (running_tradeables[0] == "@")
                        {
                            //autocalculate branch evo case
                            if (running_tradeables.Count == 5)
                            {
                                //C+D+E=A/B
                                addSpecificTradeable(specific_tradeables, prev_start, 'A', 'C', 'D', 'E');
                                addSpecificTradeable(specific_tradeables, prev_start, 'B', 'C', 'D', 'E');
                                //C+D=E
                                addSpecificTradeable(specific_tradeables, prev_start, 'E', 'C', 'D');
                            }
                            else if (running_tradeables.Count == 6)
                            {
                                //C+D+E=A
                                addSpecificTradeable(specific_tradeables, prev_start, 'A', 'C', 'D', 'E');
                                //C+D+F=B
                                addSpecificTradeable(specific_tradeables, prev_start, 'B', 'C', 'D', 'F');
                                //C+D=E/F
                                addSpecificTradeable(specific_tradeables, prev_start, 'E', 'C', 'D');
                                addSpecificTradeable(specific_tradeables, prev_start, 'F', 'C', 'D');
                            }
                        }
                        else if (running_tradeables[0] == "/")
                        {
                            //autocalculate counterpart case
                            if (running_tradeables.Count == 6)
                            {
                                //C+D+E=A
                                addSpecificTradeable(specific_tradeables, prev_start, 'A', 'C', 'D', 'E');
                                //C+D+F=B
                                addSpecificTradeable(specific_tradeables, prev_start, 'B', 'C', 'D', 'F');
                                //C+D=E/F
                                addSpecificTradeable(specific_tradeables, prev_start, 'E', 'C', 'D');
                                addSpecificTradeable(specific_tradeables, prev_start, 'F', 'C', 'D');
                            }
                            else if (running_tradeables.Count == 7)
                            {
                                //G+E=A
                                addSpecificTradeable(specific_tradeables, prev_start, 'A', 'G', 'E');
                                //G+F=B
                                addSpecificTradeable(specific_tradeables, prev_start, 'B', 'G', 'F');
                                //C+D=G
                                addSpecificTradeable(specific_tradeables, prev_start, 'G', 'C', 'D');
                            }
                        }
                        else
                        {
                            for (int nn = 0; nn < running_tradeables.Count; nn++)
                            {
                                List<int> trade_ins = new List<int>();
                                for (int kk = 0; kk < running_tradeables[nn].Length; kk++)
                                    trade_ins.Add(running_tradeables[nn][kk] - 'A' + prev_start);

                                //only add if they have trade-ins
                                if (trade_ins.Count > 0)
                                    specific_tradeables.Add((prev_start + nn, trade_ins.ToArray()));
                            }
                        }

                        //add to random tradeable pool if...
                        for (int nn = 0; nn < running_tradeables.Count; nn++)
                        {
                            int old_idx = prev_start + nn;
                            bool has_tradeables = false;
                            for (int kk = prev_tradeable_count; kk < specific_tradeables.Count; kk++)
                            {
                                if (specific_tradeables[kk].item == old_idx)
                                {
                                    has_tradeables = true;
                                    break;
                                }
                            }
                            //they are at the bottom of their trade chain
                            if (!has_tradeables)
                            {
                                ItemData old_item = DataManager.LoadData<ItemData>(old_idx.ToString(), DataManager.DataType.Item.ToString());
                                //has a rarity of 3 or lower
                                if (old_item.Rarity <= 3)
                                {
                                    int old_dex = dex_map[old_idx];

                                    switch (primaryDex)
                                    {
                                        case 144://legendary birds
                                        case 145:
                                        case 146:
                                        case 150://mew/two
                                        case 151:
                                        case 201://unown
                                        case 243://legendary beasts
                                        case 244:
                                        case 245:
                                        case 249://bird duo
                                        case 250:
                                        case 251://celebi
                                        case 486://regis
                                        case 377:
                                        case 378:
                                        case 379:
                                        case 380://lati
                                        case 381:
                                        case 382://weather trio
                                        case 383:
                                        case 384:
                                        case 385://jirachi
                                        case 386://deoxys
                                        case 480://spirit trio
                                        case 481:
                                        case 482:
                                        case 483://creation trio
                                        case 484:
                                        case 487:
                                        case 485://heatran
                                        case 488://dream duo
                                        case 491:
                                        case 492://shaymin
                                        case 493://arceus
                                            break;
                                        default:
                                            random_tradeables.Add((old_idx, old_dex, old_item.Rarity));
                                            break;
                                    }
                                }
                            }
                        }

                        running_tradeables.Clear();
                        prev_start = item_idx + 1;
                    }
                }
            }

            //output trade tables to common_gen.lua
            string path = PathMod.ModPath(LuaEngine.SCRIPT_PATH + "common_gen.lua");
            using (StreamWriter file = new StreamWriter(path))
            {
                file.Write("--[[\n" +
                "    common_gen.lua\n" +
                "    A generated collection of values\n" +
                "]]--\n" +
                "COMMON_GEN = {}\n" +
                "\n" +
                "COMMON_GEN.TRADES = {\n");
                foreach ((int item, int[] reqItem) trade in specific_tradeables)
                {
                    file.Write("{ Item=" + trade.item + ", ReqItem={" + String.Join(',',trade.reqItem) + "}},\n");
                }
                file.Write("}\n\n");
                file.Write("COMMON_GEN.TRADES_RANDOM = {\n");
                foreach ((int item, int dex, int reqCount) trade in random_tradeables)
                {
                    List<int> wildcards = new List<int>();
                    for (int nn = 0; nn < trade.reqCount + 1; nn++)
                        wildcards.Add(-1);
                    file.Write("{ Item=" + trade.item + ", Dex="+trade.dex + ", ReqItem={" + String.Join(',', wildcards.ToArray()) + "}},\n");
                }
                file.Write("}\n\n");
            }

            Console.WriteLine(String.Format("Incomplete Left: {0}", incompleteLeft));
        }

        private static void addSpecificTradeable(List<(int, int[])> specific_tradeables, int start_item, char result, params char[] trades)
        {
            List<int> trade_items = new List<int>();
            foreach (char trade in trades)
                trade_items.Add(start_item + (trade - 'A'));
            specific_tradeables.Add((start_item + (result - 'A'), trade_items.ToArray()));
        }


        /// <summary>
        /// The purpose of this function is:
        /// - To fill in the item effects based on the type and args.
        /// - To fill in the string description of the item effects, to be ready for local format.
        /// - To return the raw un-formatted string expressions to be ready for translation.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="localArgs"></param>
        /// <param name="type"></param>
        /// <param name="args"></param>
        public static void FillExclusiveEffects(ItemData item, List<LocalText> localArgs, bool includeEffects, ExclusiveItemEffect type, object[] args, bool translate)
        {
            if (type == ExclusiveItemEffect.TypeStatBonus)
            {
                item.Rarity = 1;
                item.Desc = new LocalText("When kept in the bag, it slightly boosts the {1} of {0}-type members.");
                if (includeEffects)
                {
                    localArgs.Add(DataManager.Instance.GetElement((string)args[0]).Name);
                    Stat[] boostedStats = (Stat[])args[1];
                    List<LocalText> nameList = new List<LocalText>();
                    foreach (Stat stat in boostedStats)
                        nameList.Add(ToLocalText(stat, item.Desc, translate));

                    localArgs.Add(BuildLocalTextList(nameList, translate));

                    foreach (Stat stat in boostedStats)
                    {
                        if (stat == Stat.Attack)
                            item.OnActions.Add(0, new TypeSpecificMultCategoryEvent((string)args[0], new DustState(), BattleData.SkillCategory.Physical, 20, 1));
                        else if (stat == Stat.MAtk)
                            item.OnActions.Add(0, new TypeSpecificMultCategoryEvent((string)args[0], new DustState(), BattleData.SkillCategory.Magical, 20, 1));
                        else if (stat == Stat.Defense)
                            item.BeforeBeingHits.Add(0, new TypeSpecificMultCategoryEvent((string)args[0], new SilkState(), BattleData.SkillCategory.Physical, 20, 1));
                        else if (stat == Stat.MDef)
                            item.BeforeBeingHits.Add(0, new TypeSpecificMultCategoryEvent((string)args[0], new SilkState(), BattleData.SkillCategory.Magical, 20, 1));
                    }
                }
            }
            else if (type == ExclusiveItemEffect.TypeSpeedBoost)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, it boosts the Movement Speed of {0}-type members that have no status conditions.");
                if (includeEffects)
                {
                    localArgs.Add(DataManager.Instance.GetElement((string)args[0]).Name);

                    item.OnRefresh.Add(0, new AddTypeSpeedEvent((string)args[0], 1, new GemBoostState()));
                }
            }
            else if (type == ExclusiveItemEffect.TypeGroupWeaknessReduce)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, it reduces damage done to {0}-type members by {1}-type attacks, based on how many are on the team.");
                if (includeEffects)
                {
                    string defendType = (string)args[1];
                    string holderType = (string)args[0];
                    localArgs.Add(DataManager.Instance.GetElement(holderType).Name);
                    localArgs.Add(DataManager.Instance.GetElement(defendType).Name);

                    SingleEmitter emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));
                    BattleEvent reduceEvent1 = new MultiplyElementEvent(defendType, 2, 3, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10));
                    emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));
                    BattleEvent reduceEvent2 = new MultiplyElementEvent(defendType, 1, 3, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10));
                    emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));
                    BattleEvent immuneEvent = new MultiplyElementEvent(defendType, -1, 1, true, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10));
                    item.BeforeBeingHits.Add(0, new TeamReduceEvent(holderType, reduceEvent1, reduceEvent2, immuneEvent, null));


                    //item.ProximityEvent.Radius = 0;
                    //item.ProximityEvent.TargetAlignments = (Alignment.Friend | Alignment.Foe);
                    //BattleEvent isolateEvent = new IsolateElementEvent(defendType);
                    BattleEvent absorbEvent = new AbsorbElementEvent(defendType, false, true,
                        new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Drink",
                        new RestoreHPEvent(1, 4, true));

                    //item.ProximityEvent.BeforeExplosions.Add(0, new TeamReduceEvent(holderType, null, null, null, isolateEvent));
                    item.BeforeBeingHits.Add(5, new TeamReduceEvent(holderType, null, null, null, absorbEvent));
                }
            }
            else if (type == ExclusiveItemEffect.TypeBulldozer)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, it allows {0}-type moves to hit {1}-type Pokémon.");
                if (includeEffects)
                {
                    string attackType = (string)args[0];
                    string defendType = (string)args[1];
                    localArgs.Add(DataManager.Instance.GetElement(attackType).Name);
                    localArgs.Add(DataManager.Instance.GetElement(defendType).Name);

                    item.UserElementEffects.Add(0, new FamilyMatchupEvent(new LessImmunityEvent(attackType, defendType)));
                }
            }
            else if (type == ExclusiveItemEffect.SuperCrit)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, it raises the critical-hit ratio of super-effective hits.");
                if (includeEffects)
                {
                    item.BeforeHittings.Add(0, new FamilyBattleEvent(new CritEffectiveEvent(false, 1)));
                }
            }
            else if (type == ExclusiveItemEffect.NVECrit)
            {
                item.Rarity = 1;
                item.Desc = new LocalText("When kept in the bag, it raises the critical-hit ratio of not-very-effective hits.");
                if (includeEffects)
                {
                    item.BeforeHittings.Add(0, new FamilyBattleEvent(new CritEffectiveEvent(true, 1)));
                }
            }
            else if (type == ExclusiveItemEffect.GapProber)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokemon's moves and items that hit in a straight line cannot damage allies.");
                if (includeEffects)
                {
                    item.OnActions.Add(1, new FamilyBattleEvent(new GapProberEvent()));
                }
            }
            else if (type == ExclusiveItemEffect.PassThroughAttacker)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, the Pokemon's moves and items that hit in a straight line will pass through walls.");
                if (includeEffects)
                {
                    item.OnActions.Add(1, new FamilyBattleEvent(new PierceEvent(true, true, false, true)));
                }
            }
            else if (type == ExclusiveItemEffect.Nontraitor)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, the Pokémon cannot damage allies with its moves.");
                if (includeEffects)
                {
                    item.OnActions.Add(1, new FamilyBattleEvent(new OnMoveUseEvent(new NontraitorEvent())));
                }
            }
            else if (type == ExclusiveItemEffect.WaterTerrain)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, the Pokémon can traverse water.");
                if (includeEffects)
                {
                    item.OnRefresh.Add(0, new FamilyRefreshEvent(new AddMobilityEvent(TerrainData.Mobility.Water)));
                }
            }
            else if (type == ExclusiveItemEffect.LavaTerrain)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, the Pokémon can traverse lava without being burned.");
                if (includeEffects)
                {
                    item.OnRefresh.Add(0, new FamilyRefreshEvent(new AddMobilityEvent(TerrainData.Mobility.Lava)));
                    item.OnRefresh.Add(0, new FamilyRefreshEvent(new MiscEvent(new LavaState())));
                }
            }
            else if (type == ExclusiveItemEffect.AllTerrain)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, the Pokémon can traverse water, lava, and pits.");
                if (includeEffects)
                {
                    item.OnRefresh.Add(0, new FamilyRefreshEvent(new AddMobilityEvent(TerrainData.Mobility.Water)));
                    item.OnRefresh.Add(0, new FamilyRefreshEvent(new AddMobilityEvent(TerrainData.Mobility.Abyss)));
                    item.OnRefresh.Add(0, new FamilyRefreshEvent(new AddMobilityEvent(TerrainData.Mobility.Lava)));
                }
            }
            else if (type == ExclusiveItemEffect.Wallbreaker)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, some of the Pokémon's moves will break walls.");
                if (includeEffects)
                {
                    item.OnActions.Add(1, new FamilyBattleEvent(new MeleeHitTilesEvent(TileAlignment.Wall)));
                    SingleEmitter terrainEmitter = new SingleEmitter(new AnimData("Wall_Break", 2));
                    item.OnHitTiles.Add(0, new FamilyBattleEvent(new OnMoveUseEvent(new OnMeleeActionEvent(false, new RemoveTerrainEvent("DUN_Rollout", terrainEmitter, "wall")))));
                }
            }
            else if (type == ExclusiveItemEffect.GapFiller)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, some of the Pokémon's moves will fill water, lava, and pits.");
                if (includeEffects)
                {
                    item.OnHitTiles.Add(0, new FamilyBattleEvent(new OnMoveUseEvent(new OnMeleeActionEvent(true, new RemoveTerrainEvent("", new EmptyFiniteEmitter(), "water", "lava", "abyss")))));
                }
            }
            else if (type == ExclusiveItemEffect.PPBoost)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, the Pokémon gets an extra {0}PP to all of its moves.");
                if (includeEffects)
                {
                    localArgs.Add(new LocalText(((int)args[0]).ToString()));

                    item.OnRefresh.Add(0, new FamilyRefreshEvent(new AddChargeEvent((int)args[0])));
                }
            }
            else if (type == ExclusiveItemEffect.PPSaver)
            {
                item.Rarity = 1;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's PP cannot be lowered by the moves and abilities of opposing Pokémon.");
                if (includeEffects)
                {
                    item.OnRefresh.Add(0, new FamilyRefreshEvent(new PPSaverEvent()));
                }
            }
            else if (type == ExclusiveItemEffect.DeepBreather)
            {
                item.Rarity = 1;
                item.Desc = new LocalText("When kept in the bag, it restores the PP of a move when the Pokémon reaches a new floor.");
                if (includeEffects)
                {
                    item.OnMapStarts.Add(0, new FamilySingleEvent(new DeepBreathEvent(false)));
                }
            }
            else if (type == ExclusiveItemEffect.PracticeSwinger)
            {
                item.Rarity = 1;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will do increased damage if its previous move missed.");
                if (includeEffects)
                {
                    item.BeforeHittings.Add(0, new FamilyBattleEvent(new MultWhenMissEvent("missed_all_last_turn", 4, 3)));
                }
            }
            else if (type == ExclusiveItemEffect.MisfortuneMirror)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will more easily dodge attacks if its previous move missed.");
                if (includeEffects)
                {
                    item.BeforeBeingHits.Add(0, new FamilyBattleEvent(new EvasiveWhenMissEvent("missed_all_last_turn")));
                }
            }
            else if (type == ExclusiveItemEffect.CounterBasher)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will counter damage from regular attacks and thrown items.");
                if (includeEffects)
                {
                    SingleEmitter emitter = new SingleEmitter(new BeamAnimData("Column_Blue", 3, -1, -1, 192));
                    SingleEmitter targetEmitter = new SingleEmitter(new AnimData("Hit_Neutral", 3));
                    item.AfterBeingHits.Add(0, new FamilyBattleEvent(new CounterNonSkillEvent(1, 2, new BattleAnimEvent(emitter, "DUN_Light_Screen", true, 10), new BattleAnimEvent(targetEmitter, "DUN_Hit_Neutral", false))));
                }
            }
            else if (type == ExclusiveItemEffect.Gratitude)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, the Pokémon gives back recovered HP when it is healed by another Pokémon.");
                if (includeEffects)
                {
                    SingleEmitter emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2));
                    item.AfterBeingHits.Add(0, new FamilyBattleEvent(new CounterHealEvent(1, 2, new BattleAnimEvent(emitter, "DUN_Growth", true, 10))));
                }
            }
            else if (type == ExclusiveItemEffect.DoubleAttacker)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, it allows the Pokémon's regular attack to strike twice.");
                if (includeEffects)
                {
                    item.BeforeActions.Add(-1, new FamilyBattleEvent(new RegularAttackNeededEvent(new MultiStrikeEvent(2, false))));
                }
            }
            else if (type == ExclusiveItemEffect.DoubleDash)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, it changes the Pokémon's lunging moves into two weaker strikes.");
                if (includeEffects)
                {
                    item.BeforeActions.Add(-1, new FamilyBattleEvent(new OnDashActionEvent(new MultiStrikeEvent(2, true))));
                }
            }
            else if (type == ExclusiveItemEffect.StatusSplash)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's stat-changing moves are spread to allies.");
                if (includeEffects)
                {
                    ExplosionData explosion = new ExplosionData();
                    explosion.Range = 1;
                    explosion.Speed = 10;
                    explosion.TargetAlignments = (Alignment.Self | Alignment.Friend);

                    explosion.ExplodeFX.Emitter = new SingleEmitter(new AnimData("Circle_White_Out", 3));

                    //CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Event_Gather_Sparkle", 5));
                    //emitter.BurstTime = 6;
                    //emitter.ParticlesPerBurst = 4;
                    //emitter.Bursts = 5;
                    //emitter.StartDistance = 4;
                    //explosion.Emitter = emitter;

                    string[] statuses = { "mod_speed", "mod_attack", "mod_defense", "mod_special_attack", "mod_special_defense", "mod_accuracy", "mod_evasion" };

                    item.BeforeActions.Add(-3, new FamilyBattleEvent(new OnSelfActionEvent(new GiveStatusNeededEvent(statuses, new ChangeExplosionEvent(explosion)))));
                }
            }
            else if (type == ExclusiveItemEffect.MasterHurler)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, items thrown by the Pokémon will pierce through enemies.");
                if (includeEffects)
                {
                    item.OnActions.Add(0, new FamilyBattleEvent(new PierceEvent(false, true, true, false)));
                }
            }
            else if (type == ExclusiveItemEffect.AllyReviver)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will revive a fallen ally when it reaches a new floor.");
                if (includeEffects)
                {
                    item.OnMapStarts.Add(0, new FamilySingleEvent(new AllyReviverEvent()));
                }
            }
            else if (type == ExclusiveItemEffect.AllyReviverBattle)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will revive a fallen ally when it defeats an opponent.");
                if (includeEffects)
                {
                    item.AfterActions.Add(0, new FamilyBattleEvent(new KnockOutNeededEvent(new BattlelessEvent(false, new AllyReviverEvent()))));
                }
            }
            else if (type == ExclusiveItemEffect.PressurePlus)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will raise the PP usage of opponents that attack it.");
                if (includeEffects)
                {
                    item.BeforeBeingHits.Add(0, new FamilyBattleEvent(new AddContextStateEvent(new PressurePlus())));
                    item.AfterBeingHits.Add(0, new FamilyBattleEvent(new PressureEvent(0)));
                }
            }
            else if (type == ExclusiveItemEffect.StatusOnAttack)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's regular attacks and thrown items have a chance to inflict the {0} status.");
                if (includeEffects)
                {
                    localArgs.Add(DataManager.Instance.GetStatus((string)args[0]).Name);

                    item.AfterHittings.Add(0, new FamilyBattleEvent(new RegularAttackNeededEvent(new OnHitEvent(true, false, 35, new StatusBattleEvent((string)args[0], true, true, false, new StringKey("MSG_EXCL_ITEM_TYPE"))))));
                    item.AfterHittings.Add(0, new FamilyBattleEvent(new ThrownItemNeededEvent(new OnHitEvent(true, false, 35, new StatusBattleEvent((string)args[0], true, true, false, new StringKey("MSG_EXCL_ITEM_TYPE"))))));
                }
            }
            else if (type == ExclusiveItemEffect.DevolveOnAttack)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's regular attacks and thrown items have a chance to devolve the target.");
                if (includeEffects)
                {
                    item.AfterHittings.Add(0, new FamilyBattleEvent(new RegularAttackNeededEvent(new OnHitEvent(true, false, 35, new DevolveEvent(true, new BattleAnimEvent(new SingleEmitter(new AnimData("Puff_Green", 3)), "DUN_Transform", true))))));
                    item.AfterHittings.Add(0, new FamilyBattleEvent(new ThrownItemNeededEvent(new OnHitEvent(true, false, 35, new DevolveEvent(true, new BattleAnimEvent(new SingleEmitter(new AnimData("Puff_Green", 3)), "DUN_Transform", true))))));
                }
            }
            else if (type == ExclusiveItemEffect.KnockbackOnAttack)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's regular attack will knock opponents back.");
                if (includeEffects)
                {
                    item.AfterHittings.Add(0, new FamilyBattleEvent(new RegularAttackNeededEvent(new OnHitEvent(true, false, 100, new KnockBackEvent(1)))));
                }
            }
            else if (type == ExclusiveItemEffect.ThrowOnAttack)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's regular attack will throw opponents back.");
                if (includeEffects)
                {
                    BattleData altData = new BattleData();
                    altData.Element = "none";
                    altData.Category = BattleData.SkillCategory.Physical;
                    altData.SkillStates.Set(new ContactState());
                    altData.HitRate = 90;
                    altData.SkillStates.Set(new BasePowerState(30));
                    altData.OnHits.Add(0, new ThrowBackEvent(2, new DamageFormulaEvent()));

                    item.OnActions.Add(-3, new FamilyBattleEvent(new RegularAttackNeededEvent(new ChangeDataEvent(altData))));
                }
            }
            else if (type == ExclusiveItemEffect.SureHitAttacker)
            {
                item.Rarity = 1;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's regular attack cannot miss.");
                if (includeEffects)
                {
                    item.OnActions.Add(0, new FamilyBattleEvent(new RegularAttackNeededEvent(new SetAccuracyEvent(-1))));
                }
            }
            else if (type == ExclusiveItemEffect.SpecialAttacker)
            {
                item.Rarity = 1;
                item.Desc = new LocalText("When kept in the bag, the category of the Pokémon's regular attack is changed from physical to special.");
                if (includeEffects)
                {
                    item.OnActions.Add(-5, new FamilyBattleEvent(new RegularAttackNeededEvent(new FlipCategoryEvent(false))));
                }
            }
            else if (type == ExclusiveItemEffect.StatusOnCategoryHit)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's {1} moves inflict the {0} status.");
                if (includeEffects)
                {
                    localArgs.Add(DataManager.Instance.GetStatus((string)args[0]).Name);
                    BattleData.SkillCategory category = (BattleData.SkillCategory)args[1];
                    localArgs.Add(ToLocalText(category, item.Desc, translate));

                    item.AfterHittings.Add(0, new FamilyBattleEvent(new CategoryNeededEvent(category, new OnHitEvent(false, false, 100, new StatusBattleEvent((string)args[0], true, true, false, new StringKey("MSG_EXCL_ITEM_TYPE"))))));
                }
            }
            else if (type == ExclusiveItemEffect.ChanceStatusOnCategoryHit)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's {1} moves may inflict the {0} status.");
                if (includeEffects)
                {
                    localArgs.Add(DataManager.Instance.GetStatus((string)args[0]).Name);
                    BattleData.SkillCategory category = (BattleData.SkillCategory)args[1];
                    localArgs.Add(ToLocalText(category, item.Desc, translate));
                    int chance = (int)args[2];

                    item.AfterHittings.Add(0, new FamilyBattleEvent(new CategoryNeededEvent(category, new OnHitEvent(false, false, chance, new StatusBattleEvent((string)args[0], true, true, false, new StringKey("MSG_EXCL_ITEM_TYPE"))))));
                }
            }
            else if (type == ExclusiveItemEffect.StatusOnTypeHit)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's {0}-type moves inflict the {1} status.");
                if (includeEffects)
                {
                    string element = (string)args[0];
                    localArgs.Add(DataManager.Instance.GetElement(element).Name);
                    localArgs.Add(DataManager.Instance.GetStatus((string)args[1]).Name);

                    item.AfterHittings.Add(0, new FamilyBattleEvent(new ElementNeededEvent(element, new OnHitEvent(false, false, 100, new StatusBattleEvent((string)args[1], true, true, false, new StringKey("MSG_EXCL_ITEM_TYPE"))))));
                }
            }
            else if (type == ExclusiveItemEffect.ChanceStatusOnTypeHit)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's {0}-type moves have a chance to inflict the {1} status.");
                if (includeEffects)
                {
                    string element = (string)args[0];
                    localArgs.Add(DataManager.Instance.GetElement(element).Name);
                    localArgs.Add(DataManager.Instance.GetStatus((string)args[1]).Name);
                    int chance = (int)args[2];

                    item.AfterHittings.Add(0, new FamilyBattleEvent(new ElementNeededEvent(element, new OnHitEvent(false, false, chance, new StatusBattleEvent((string)args[1], true, true, false, new StringKey("MSG_EXCL_ITEM_TYPE"))))));
                }
            }
            else if (type == ExclusiveItemEffect.ChanceStatOnTypeHit)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's {0}-type moves may lower the target's {1}.");
                if (includeEffects)
                {
                    string element = (string)args[0];
                    localArgs.Add(DataManager.Instance.GetElement(element).Name);
                    localArgs.Add(DataManager.Instance.GetStatus((string)args[1]).Name);
                    int chance = (int)args[2];

                    item.AfterHittings.Add(0, new FamilyBattleEvent(new ElementNeededEvent(element, new OnHitEvent(false, false, chance, new StatusStackBattleEvent((string)args[1], true, true, false, -1, new StringKey("MSG_EXCL_ITEM_TYPE"))))));
                }
            }
            else if (type == ExclusiveItemEffect.StatusOnCategoryUse)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, the Pokémon gains the {0} status after using a {1} move.");
                if (includeEffects)
                {
                    localArgs.Add(DataManager.Instance.GetStatus((string)args[0]).Name);
                    BattleData.SkillCategory category = (BattleData.SkillCategory)args[1];
                    localArgs.Add(ToLocalText(category, item.Desc, translate));

                    item.AfterActions.Add(0, new FamilyBattleEvent(new CategoryNeededEvent(category, new StatusBattleEvent((string)args[0], false, true, false, new StringKey("MSG_EXCL_ITEM_TYPE")))));
                }
            }
            else if (type == ExclusiveItemEffect.StatOnCategoryUse)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's {0} is raised slightly when it uses a {1} move.");
                if (includeEffects)
                {
                    localArgs.Add(DataManager.Instance.GetStatus((string)args[0]).Name);
                    BattleData.SkillCategory category = (BattleData.SkillCategory)args[1];
                    localArgs.Add(ToLocalText(category, item.Desc, translate));

                    item.AfterActions.Add(0, new FamilyBattleEvent(new CategoryNeededEvent(category, new StatusStackBattleEvent((string)args[0], false, true, false, 1, new StringKey("MSG_EXCL_ITEM_TYPE")))));
                }
            }
            else if (type == ExclusiveItemEffect.MapStatusOnCategoryUse)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, the Pokémon changes the floor to {1} after using a {0} move.");
                if (includeEffects)
                {
                    BattleData.SkillCategory category = (BattleData.SkillCategory)args[0];
                    localArgs.Add(ToLocalText(category, item.Desc, translate));
                    MapStatusData mapStatus = DataManager.Instance.GetMapStatus((string)args[1]);
                    localArgs.Add(mapStatus.Name);

                    if (mapStatus.StatusStates.Contains<MapWeatherState>())
                        item.AfterActions.Add(0, new FamilyBattleEvent(new CategoryNeededEvent(category, new GiveMapStatusEvent((string)args[1], 10, new StringKey(), typeof(ExtendWeatherState)))));
                    else
                        item.AfterActions.Add(0, new FamilyBattleEvent(new CategoryNeededEvent(category, new GiveMapStatusEvent((string)args[1], 10))));
                }
            }
            else if (type == ExclusiveItemEffect.StatusImmune)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon becomes immune to the {0} status.");
                if (includeEffects)
                {
                    string status = (string)args[0];
                    localArgs.Add(DataManager.Instance.GetStatus(status).Name);

                    item.BeforeStatusAdds.Add(0, new FamilyStatusEvent(new PreventStatusCheck(status, new StringKey("MSG_PROTECT_WITH"))));
                    if (status == "poison")//poison and bad poison
                        item.BeforeStatusAdds.Add(0, new FamilyStatusEvent(new PreventStatusCheck("poison_toxic", new StringKey("MSG_PROTECT_WITH"))));
                    if (status == "sleep")//asleep and yawn
                        item.BeforeStatusAdds.Add(0, new FamilyStatusEvent(new PreventStatusCheck("yawning", new StringKey("MSG_PROTECT_WITH"))));
                }
            }
            else if (type == ExclusiveItemEffect.StatDropImmune)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, the Pokémon cannot have its {0} lowered.");
                if (includeEffects)
                {
                    Stat[] boostedStats = (Stat[])args[0];
                    List<LocalText> nameList = new List<LocalText>();
                    foreach (Stat stat in boostedStats)
                        nameList.Add(ToLocalText(stat, item.Desc, translate));

                    localArgs.Add(BuildLocalTextList(nameList, translate));

                    List<Stat> protectedStats = new List<Stat>();
                    protectedStats.AddRange(boostedStats);
                    item.BeforeStatusAdds.Add(0, new FamilyStatusEvent(new StatChangeCheck(protectedStats, new StringKey("MSG_STAT_DROP_PROTECT"), true, false, false)));
                }
            }
            else if (type == ExclusiveItemEffect.SleepWalker)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will be able to move when asleep.");
                if (includeEffects)
                {
                    item.OnRefresh.Add(-1, new FamilyRefreshEvent(new MiscEvent(new SleepWalkerState())));
                }
            }
            else if (type == ExclusiveItemEffect.ChargeWalker)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will be able to move when charging attacks.");
                if (includeEffects)
                {
                    item.OnRefresh.Add(-1, new FamilyRefreshEvent(new MiscEvent(new ChargeWalkerState())));
                }
            }
            else if (type == ExclusiveItemEffect.HitAndRun)
            {
                item.Rarity = 1;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will take reduced damage from counter attacks.");
                if (includeEffects)
                {
                    item.OnRefresh.Add(-1, new FamilyRefreshEvent(new MiscEvent(new HitAndRunState())));
                }
            }
            else if (type == ExclusiveItemEffect.StairSensor)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, it reveals the direction of the staircase whenever the Pokémon reaches a new floor.");
                if (includeEffects)
                {
                    SingleEmitter emitter = new SingleEmitter(new AnimData("Stair_Sensor_Arrow", 6), 6);
                    emitter.Layer = DrawLayer.Top;
                    item.OnMapStarts.Add(0, new FamilySingleEvent(new StairSensorEvent("stairs_sensed", emitter)));
                }
            }
            else if (type == ExclusiveItemEffect.AcuteSniffer)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, it reveals the number of items laying on the ground the Pokémon reaches a new floor.");
                if (includeEffects)
                {
                    item.OnMapStarts.Add(0, new FamilySingleEvent(new AcuteSnifferEvent("items_sniffed", new AnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 2)), ""))));
                }
            }
            else if (type == ExclusiveItemEffect.MapSurveyor)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, it partially reveals the floor's layout the Pokémon reaches a new floor.");
                if (includeEffects)
                {
                    item.OnMapStarts.Add(0, new FamilySingleEvent(new MapSurveyorEvent("map_surveyed", 22)));
                }
            }
            else if (type == ExclusiveItemEffect.XRay)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, it allows the Pokémon to see foes and items in heavy darkness.");
                if (includeEffects)
                {
                    item.OnRefresh.Add(0, new FamilyRefreshEvent(new SetSightEvent(true, Map.SightRange.Clear)));
                    item.OnRefresh.Add(0, new FamilyRefreshEvent(new SeeItemsEvent(true)));
                }
            }
            else if (type == ExclusiveItemEffect.WeaknessPayback)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, it inflicts the {1} status on the attacker when the Pokémon is hit with a {0}-type move.");
                if (includeEffects)
                {
                    string counterElement = (string)args[0];
                    string counterStatus = (string)args[1];
                    localArgs.Add(DataManager.Instance.GetElement(counterElement).Name);
                    localArgs.Add(DataManager.Instance.GetStatus(counterStatus).Name);

                    item.AfterBeingHits.Add(0, new FamilyBattleEvent(new ElementNeededEvent(counterElement, new HitCounterEvent(Alignment.Foe, false, false, false, 100, new StatusBattleEvent(counterStatus, false, true, false, new StringKey("MSG_EXCL_ITEM_TYPE"),
                        new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2)), "DUN_Switcher", false, 0))))));
                }
            }
            else if (type == ExclusiveItemEffect.WeaknessReduce)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, it reduces damage done by {0}-type attacks.");
                if (includeEffects)
                {
                    string counterElement = (string)args[0];
                    localArgs.Add(DataManager.Instance.GetElement(counterElement).Name);

                    SingleEmitter emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));
                    item.BeforeBeingHits.Add(0, new FamilyBattleEvent(new MultiplyElementEvent(counterElement, 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10))));
                }
            }
            else if (type == ExclusiveItemEffect.WarpPayback)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, it warps the attackers away when the Pokémon is hit with a {0}-type move.");
                if (includeEffects)
                {
                    string counterElement = (string)args[0];
                    localArgs.Add(DataManager.Instance.GetElement(counterElement).Name);

                    item.AfterBeingHits.Add(0, new FamilyBattleEvent(new ElementNeededEvent(counterElement, new HitCounterEvent(Alignment.Foe, false, false, false, 100, new RandomWarpEvent(50, false, new StringKey("MSG_EXCL_ITEM_TYPE"))))));
                }
            }
            else if (type == ExclusiveItemEffect.LungeAttack)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, it changes the Pokémon's regular attack into a dash.");
                if (includeEffects)
                {
                    DashAction altAction = new DashAction();
                    altAction.CharAnim = 05;//Attack
                    altAction.Range = 2;
                    altAction.StopAtWall = true;
                    altAction.StopAtHit = true;
                    altAction.HitTiles = true;
                    altAction.TargetAlignments = Alignment.Foe;

                    BattleFX preFX = new BattleFX();
                    preFX.Sound = "DUN_Attack";
                    altAction.PreActions.Add(preFX);

                    item.OnActions.Add(-3, new FamilyBattleEvent(new RegularAttackNeededEvent(new ChangeActionEvent(altAction))));
                    item.OnActions.Add(-3, new FamilyBattleEvent(new RegularAttackNeededEvent(new TraitorEvent())));
                }
            }
            else if (type == ExclusiveItemEffect.ProjectileAttack)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, it changes the Pokémon's regular attack into a short projectile.");
                if (includeEffects)
                {
                    ProjectileAction altAction = new ProjectileAction();
                    altAction.CharAnimData = new CharAnimFrameType(07);//shoot
                    altAction.Range = 2;
                    altAction.Speed = 10;
                    altAction.StopAtWall = true;
                    altAction.StopAtHit = true;
                    altAction.HitTiles = true;
                    altAction.Anim = new AnimData("Confuse_Ray", 2);
                    altAction.TargetAlignments = Alignment.Foe;

                    BattleFX preFX = new BattleFX();
                    preFX.Sound = "DUN_Attack";
                    altAction.PreActions.Add(preFX);

                    item.OnActions.Add(-3, new FamilyBattleEvent(new RegularAttackNeededEvent(new ChangeActionEvent(altAction))));
                    item.OnActions.Add(-3, new FamilyBattleEvent(new RegularAttackNeededEvent(new TraitorEvent())));
                }
            }
            else if (type == ExclusiveItemEffect.WideAttack)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's regular attack will hit the front and sides.");
                if (includeEffects)
                {
                    AttackAction altAction = new AttackAction();
                    altAction.CharAnimData = new CharAnimFrameType(40);//Swing
                    altAction.HitTiles = true;
                    altAction.TargetAlignments = Alignment.Foe;

                    BattleFX preFX = new BattleFX();
                    preFX.Sound = "DUN_Attack";
                    altAction.PreActions.Add(preFX);

                    item.OnActions.Add(-3, new FamilyBattleEvent(new RegularAttackNeededEvent(new ChangeActionEvent(altAction))));
                }
            }
            else if (type == ExclusiveItemEffect.ExplosiveAttack)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's regular attack explodes outwards with splash damage.");
                if (includeEffects)
                {
                    ExplosionData explosion = new ExplosionData();
                    explosion.Range = 1;
                    explosion.HitTiles = true;
                    explosion.Speed = 10;
                    explosion.ExplodeFX.Sound = "DUN_Blast_Seed";
                    explosion.TargetAlignments = (Alignment.Friend | Alignment.Foe);

                    CircleSquareAreaEmitter explosionEmitter = new CircleSquareAreaEmitter(new AnimData("Blast_Seed", 3));
                    explosionEmitter.ParticlesPerTile = 0.8;
                    explosion.Emitter = explosionEmitter;

                    item.OnActions.Add(-3, new FamilyBattleEvent(new RegularAttackNeededEvent(new ChangeExplosionEvent(explosion))));
                }
            }
            else if (type == ExclusiveItemEffect.TrapBuster)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's moves will destroy traps.");
                if (includeEffects)
                {
                    item.OnHitTiles.Add(0, new FamilyBattleEvent(new OnMoveUseEvent(new RemoveTrapEvent())));
                }
            }
            else if (type == ExclusiveItemEffect.ExplosionGuard)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will take reduced damage from explosions and splash damage.");
                if (includeEffects)
                {
                    SingleEmitter emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));
                    item.BeforeBeingHits.Add(0, new FamilyBattleEvent(new BlastProofEvent(0, 1, 2, true, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10))));
                }
            }
            else if (type == ExclusiveItemEffect.WandMaster)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the effects of wands will fly in an arc when waved by the Pokémon.");
                if (includeEffects)
                {
                    ThrowAction altAction = new ThrowAction();
                    altAction.CharAnimData = new CharAnimFrameType(42);//Rotate
                    altAction.Coverage = ThrowAction.ArcCoverage.WideAngle;
                    altAction.Range = 10;
                    altAction.Speed = 12;
                    altAction.Anim = new AnimData("Confuse_Ray", 2);
                    altAction.TargetAlignments = Alignment.Foe;
                    BattleFX itemFX = new BattleFX();
                    itemFX.Sound = "DUN_Throw_Start";
                    altAction.PreActions.Add(itemFX);
                    altAction.ActionFX.Sound = "DUN_Blowback_Orb";

                    item.OnActions.Add(-3, new FamilyBattleEvent(new WandAttackNeededEvent(new List<int> { 220, 221 }, new ChangeActionEvent(altAction))));
                }
            }
            else if (type == ExclusiveItemEffect.WandSpread)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, the effects of wands will spread out like a fan when waved by the Pokémon.");
                if (includeEffects)
                {
                    item.OnActions.Add(-3, new FamilyBattleEvent(new WandAttackNeededEvent(new List<int> { 221 }, new SpreadProjectileEvent(ProjectileAction.RayCount.Three))));
                }
            }
            else if (type == ExclusiveItemEffect.MultiRayShot)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, items thrown by the Pokémon will spread out like a fan.");
                if (includeEffects)
                {
                    item.OnActions.Add(-2, new FamilyBattleEvent(new ThrowItemPreventDropEvent()));
                    item.OnActions.Add(-2, new FamilyBattleEvent(new ThrownItemNeededEvent(new SpreadProjectileEvent(ProjectileAction.RayCount.Three))));
                }
            }
            else if (type == ExclusiveItemEffect.RoyalVeil)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will gradually restore the HP of its allies when its own HP is full.");
                if (includeEffects)
                {
                    item.OnTurnEnds.Add(-3, new FamilySingleEvent(new RoyalVeilEvent(4)));
                }
            }
            else if (type == ExclusiveItemEffect.Celebrate)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will be able to move again after defeating an enemy.");
                if (includeEffects)
                {
                    SingleEmitter emitter = new SingleEmitter(new AnimData("Circle_White_In", 1));
                    item.AfterActions.Add(1, new FamilyBattleEvent(new KnockOutNeededEvent(new PreserveTurnEvent(new StringKey("MSG_EXTRA_TURN"), new BattleAnimEvent(emitter, "DUN_Attack_Speed_Up", false, 20)))));
                }
            }
            else if (type == ExclusiveItemEffect.Absorption)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will regain HP after defeating an enemy.");
                if (includeEffects)
                {
                    item.AfterActions.Add(1, new FamilyBattleEvent(new KnockOutNeededEvent(new RestoreHPEvent(1, 8, false))));
                }
            }
            else if (type == ExclusiveItemEffect.ExcessiveForce)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will damage nearby enemies after defeating an enemy.");
                if (includeEffects)
                {
                    AreaAction altAction = new AreaAction();
                    altAction.HitTiles = true;
                    altAction.BurstTiles = TileAlignment.Any;
                    altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                    ExplosionData altExplosion = new ExplosionData();
                    altExplosion.TargetAlignments = Alignment.Friend;
                    altExplosion.Range = 1;
                    altExplosion.Speed = 10;
                    altExplosion.ExplodeFX.Sound = "DUN_Blast_Seed";
                    CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Blast_Seed", 3));
                    emitter.ParticlesPerTile = 0.8;
                    altExplosion.Emitter = emitter;
                    BattleData newData = new BattleData();
                    newData.Element = "none";
                    newData.HitRate = -1;
                    newData.OnHits.Add(-1, new MaxHPDamageEvent(6));

                    item.AfterHittings.Add(0, new FamilyBattleEvent(new TargetDeadNeededEvent(new InvokeCustomBattleEvent(altAction, altExplosion, newData, new StringKey("MSG_EXCESSIVE_FORCE")))));
                }
            }
            else if (type == ExclusiveItemEffect.CelebrateCure)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will recover from status effects after defeating an enemy.");
                if (includeEffects)
                {
                    BattleAnimEvent battleAnim = new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1)), "DUN_Wonder_Tile", false, 10);
                    item.AfterActions.Add(1, new FamilyBattleEvent(new KnockOutNeededEvent(new RemoveStateStatusBattleEvent(typeof(BadStatusState), false, new StringKey("MSG_CURE_SELF"), battleAnim))));
                }
            }
            else if (type == ExclusiveItemEffect.Anchor)
            {
                item.Rarity = 1;
                item.Desc = new LocalText("When kept in the bag, the Pokémon is prevented from being forced off its location.");
                if (includeEffects)
                {
                    item.OnRefresh.Add(0, new FamilyRefreshEvent(new MiscEvent(new AnchorState())));
                }
            }
            else if (type == ExclusiveItemEffect.BarrageGuard)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, the Pokémon takes reduced damage from multiple attacks in a turn.");
                if (includeEffects)
                {
                    SingleEmitter emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));
                    item.BeforeBeingHits.Add(0, new FamilyBattleEvent(new BarrageGuardEvent("was_hurt_last_turn", 2, 3, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10))));
                }
            }
            else if (type == ExclusiveItemEffect.BetterOdds)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's attacks never miss, and always land a critical hit if the move is on its last PP.");
                if (includeEffects)
                {
                    item.BeforeHittings.Add(0, new FamilyBattleEvent(new BetterOddsEvent()));
                }
            }
            else if (type == ExclusiveItemEffect.ClutchPerformer)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, the Pokémon is more likely to evade attacks when at low HP.");
                if (includeEffects)
                {
                    item.BeforeBeingHits.Add(0, new FamilyBattleEvent(new EvasiveInPinchEvent()));
                }
            }
            else if (type == ExclusiveItemEffect.DistanceDodge)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon is more likely to evade attacks from far away.");
                if (includeEffects)
                {
                    item.BeforeBeingHits.Add(0, new FamilyBattleEvent(new EvasiveInDistanceEvent()));
                }
            }
            else if (type == ExclusiveItemEffect.CloseDodge)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon is more likely to evade attacks from close up.");
                if (includeEffects)
                {
                    item.BeforeBeingHits.Add(0, new FamilyBattleEvent(new EvasiveCloseUpEvent()));
                }
            }
            else if (type == ExclusiveItemEffect.SweetDreams)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, the Pokémon gradually restores the HP of its teammates while they are asleep.");
                if (includeEffects)
                {
                    item.ProximityEvent.Radius = 3;
                    item.ProximityEvent.TargetAlignments = Alignment.Friend | Alignment.Self;
                    item.ProximityEvent.OnTurnEnds.Add(0, new FamilySingleEvent(new NightmareEvent("sleep", -4, new StringKey("MSG_HEAL_WITH_OTHER"), new AnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2)), "DUN_Healing_Wish_2", 0))));
                }
            }
            else if (type == ExclusiveItemEffect.FastHealer)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's natural HP-recovery speed is boosted.");
                if (includeEffects)
                {
                    item.ModifyHPs.Add(1, new FamilyHPEvent(new HealMultEvent(3, 2)));
                }
            }
            else if (type == ExclusiveItemEffect.SelfCurer)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon recovers faster from status problems.");
                if (includeEffects)
                {
                    item.BeforeStatusAdds.Add(0, new FamilyStatusEvent(new SelfCurerEvent(1, 2)));
                }
            }
            else if (type == ExclusiveItemEffect.StatusMirror)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, status problems inflicted on the Pokémon are passed to the Pokémon that caused it.");
                if (includeEffects)
                {
                    item.OnStatusAdds.Add(0, new FamilyStatusEvent(new StateStatusSyncEvent(typeof(BadStatusState), new StringKey("MSG_SYNCHRONIZE"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2)), "DUN_Switcher", 30))));
                }
            }
            else if (type == ExclusiveItemEffect.StatMirror)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, stat changes inflicted on the Pokémon are passed to the Pokémon that caused it.");
                if (includeEffects)
                {
                    item.OnStatusAdds.Add(0, new FamilyStatusEvent(new StatDropSyncEvent(new StringKey("MSG_STAT_DROP_TRIGGER"), new StatusAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2)), "DUN_Switcher", 30))));
                }
            }
            else if (type == ExclusiveItemEffect.ErraticAttacker)
            {
                item.Rarity = 2;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's moves are more intensely affected by type match-ups.");
                if (includeEffects)
                {
                    item.BeforeHittings.Add(0, new FamilyBattleEvent(new MultiplyEffectiveEvent(false, 6, 5)));
                    item.BeforeHittings.Add(0, new FamilyBattleEvent(new MultiplyEffectiveEvent(true, 4, 5)));
                }
            }
            else if (type == ExclusiveItemEffect.ErraticDefender)
            {
                item.Rarity = 1;
                item.Desc = new LocalText("When kept in the bag, moves used on the Pokémon are more intensely affected by type match-ups.");
                if (includeEffects)
                {
                    item.BeforeBeingHits.Add(0, new FamilyBattleEvent(new MultiplyEffectiveEvent(false, 6, 5)));
                    item.BeforeBeingHits.Add(0, new FamilyBattleEvent(new MultiplyEffectiveEvent(true, 4, 5)));
                }
            }
            else if (type == ExclusiveItemEffect.FastFriend)
            {
                item.Rarity = 1;
                item.Desc = new LocalText("When kept in the bag, the Pokémon has a higher chance of recruiting wild Pokémon.");
                if (includeEffects)
                {
                    item.OnActions.Add(0, new FamilyBattleEvent(new FlatRecruitmentEvent(10)));
                }
            }
            else if (type == ExclusiveItemEffect.CoinWatcher)
            {
                item.Rarity = 3;
                item.Name.DefaultText = item.Name.DefaultText;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will find more \\uE024 when exploring in dungeons.");
                item.OnRefresh.Add(0, new FamilyRefreshEvent(new MiscEvent(new CoinModGenState(50))));
            }
            else if (type == ExclusiveItemEffect.HiddenStairFinder)
            {
                item.Rarity = 3;
                item.Name.DefaultText = "**" + item.Name.DefaultText;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will find more Hidden Stairs when exploring in dungeons.");
                item.OnRefresh.Add(0, new FamilyRefreshEvent(new MiscEvent(new StairsModGenState(10))));
            }
            else if (type == ExclusiveItemEffect.ChestFinder)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will find more Treasure Chests when exploring in dungeons.");
                item.OnRefresh.Add(0, new FamilyRefreshEvent(new MiscEvent(new ChestModGenState(10))));
            }
            else if (type == ExclusiveItemEffect.ShopFinder)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will find more shops when exploring in dungeons.");
                item.OnRefresh.Add(0, new FamilyRefreshEvent(new MiscEvent(new ShopModGenState(10))));
            }
            else if (type == ExclusiveItemEffect.SecondSTAB)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's {0}-type moves are boosted.");
                if (includeEffects)
                {
                    string element = (string)args[0];
                    localArgs.Add(DataManager.Instance.GetElement(element).Name);

                    item.OnActions.Add(0, new FamilyBattleEvent(new MultiplyElementEvent(element, 4, 3, false)));
                }
            }
            else if (type == ExclusiveItemEffect.TypedAttack)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's regular attacks change to match its type.");
                if (includeEffects)
                {
                    item.OnActions.Add(-1, new FamilyBattleEvent(new RegularAttackNeededEvent(new MatchAttackToTypeEvent())));
                }
            }
            else if (type == ExclusiveItemEffect.WeatherProtection)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon is protected from weather damage.");
                if (includeEffects)
                {
                    item.OnRefresh.Add(0, new FamilyRefreshEvent(new MiscEvent(new SandState())));
                    item.OnRefresh.Add(0, new FamilyRefreshEvent(new MiscEvent(new HailState())));
                }
            }
            else if (type == ExclusiveItemEffect.RemoveAbilityAttack)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, it causes the Pokémon's regular attack to remove the target's ability.");
                if (includeEffects)
                {
                    item.AfterHittings.Add(0, new FamilyBattleEvent(new RegularAttackNeededEvent(new ChangeToAbilityEvent(DataManager.Instance.DefaultIntrinsic, true, true))));
                }
            }
            else if (type == ExclusiveItemEffect.CheekPouch)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's HP is restored when it eats a food item.");
                if (includeEffects)
                {
                    item.AfterBeingHits.Add(0, new FamilyBattleEvent(new FoodNeededEvent(new RestoreHPEvent(1, 3, true))));
                }
            }
            else if (type == ExclusiveItemEffect.HealInWater)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, the Pokémon recovers HP when in water.");
                if (includeEffects)
                {
                    item.OnTurnEnds.Add(0, new FamilySingleEvent(new TerrainNeededEvent("water", new FractionHealEvent(8, new StringKey("MSG_HEAL_WITH")))));
                }
            }
            else if (type == ExclusiveItemEffect.HealInLava)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon recovers HP when in lava.");
                if (includeEffects)
                {
                    item.OnTurnEnds.Add(0, new FamilySingleEvent(new TerrainNeededEvent("lava", new FractionHealEvent(8, new StringKey("MSG_HEAL_WITH")))));
                }
            }
            else if (type == ExclusiveItemEffect.HealOnNewFloor)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon is fully healed upon entering a new floor.");
                if (includeEffects)
                {
                    item.OnMapStarts.Add(0, new FamilySingleEvent(new FractionHealEvent(1, new StringKey("MSG_HEAL_WITH"))));
                }
            }
            else if (type == ExclusiveItemEffect.EndureCategory)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, prevents the Pokémon from fainting to {0} moves, leaving it with 1 HP.");
                if (includeEffects)
                {
                    BattleData.SkillCategory category = (BattleData.SkillCategory)args[0];
                    localArgs.Add(ToLocalText(category, item.Desc, translate));

                    item.BeforeBeingHits.Add(0, new FamilyBattleEvent(new OnMoveUseEvent(new EndureCategoryEvent(category))));
                }
            }
            else if (type == ExclusiveItemEffect.EndureType)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, prevents the Pokémon from fainting to {0}-type moves, leaving it with 1 HP.");
                if (includeEffects)
                {
                    string element = (string)args[0];
                    localArgs.Add(DataManager.Instance.GetElement(element).Name);

                    item.BeforeBeingHits.Add(0, new FamilyBattleEvent(new OnMoveUseEvent(new EndureElementEvent(element))));
                }
            }
            else if (type == ExclusiveItemEffect.SpikeDropper)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, the Pokémon scatters Spikes when hit by a {0} attack.");
                if (includeEffects)
                {
                    BattleData.SkillCategory category = (BattleData.SkillCategory)args[0];
                    localArgs.Add(ToLocalText(category, item.Desc, translate));

                    item.AfterBeingHits.Add(0, new FamilyBattleEvent(new CategoryNeededEvent(category, new CounterTrapEvent("trap_spikes"))));
                }
            }
            else if (type == ExclusiveItemEffect.NoStatusInWeather)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, the Pokémon is protected from status conditions when the floor has the {0} status.");
                if (includeEffects)
                {
                    string mapStatus = (string)args[0];
                    localArgs.Add(DataManager.Instance.GetMapStatus(mapStatus).Name);

                    SingleEmitter emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));
                    item.BeforeStatusAdds.Add(0, new FamilyStatusEvent(new WeatherNeededStatusEvent(mapStatus, new StateStatusCheck(typeof(BadStatusState), new StringKey("MSG_PROTECT_STATUS"), new StatusAnimEvent(emitter, "DUN_Screen_Hit", 10)))));
                }
            }
            else if (type == ExclusiveItemEffect.AttackRangeInWeather)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, the Pokémon's Attack Range is increased when the floor has the {0} status.");
                if (includeEffects)
                {
                    string mapStatus = (string)args[0];
                    localArgs.Add(DataManager.Instance.GetMapStatus(mapStatus).Name);

                    item.OnActions.Add(-1, new FamilyBattleEvent(new WeatherAddRangeEvent(mapStatus, 1)));
                }
            }
            else if (type == ExclusiveItemEffect.DeepBreatherPlus)
            {
                item.Rarity = 4;
                item.Desc = new LocalText("When kept in the bag, it restores the PP of all moves when the Pokémon reaches a new floor.");
                if (includeEffects)
                {
                    item.OnMapStarts.Add(0, new FamilySingleEvent(new DeepBreathEvent(true)));
                }
            }
            else if (type == ExclusiveItemEffect.MetronomePlus)
            {
                item.Rarity = 5;
                item.Desc = new LocalText("When kept in the bag, it increases the Pokémon's luck when using Metronome.");
                if (includeEffects)
                {
                    BattleData altData = new BattleData();
                    altData.Element = "normal";
                    altData.Category = BattleData.SkillCategory.Status;
                    altData.HitRate = -1;
                    altData.OnHits.Add(0, new NeededMoveEvent());

                    item.OnActions.Add(-3, new FamilyBattleEvent(new SpecificSkillNeededEvent(new ChangeDataEvent(altData), 118)));
                }
            }
            else if (type == ExclusiveItemEffect.TypeBodyguard)
            {
                item.Rarity = 3;
                item.Desc = new LocalText("When kept in the bag, the Pokémon will step in to take {0}-type attacks for nearby allies.");
                if (includeEffects)
                {
                    string element = (string)args[0];
                    localArgs.Add(DataManager.Instance.GetElement(element).Name);

                    item.ProximityEvent.Radius = 1;
                    item.ProximityEvent.TargetAlignments = Alignment.Foe;
                    item.ProximityEvent.BeforeExplosions.Add(0, new DrawAttackEvent(Alignment.Friend, element, new StringKey("MSG_DRAW_ATTACK")));
                }
            }
            else
            {
                item.Name.DefaultText = "**" + item.Name.DefaultText;
            }

           if (translate)
            {
                item.Desc = itemEffectRows[typeof(ExclusiveItemEffect).Name + "." + type];
            }
        }


        public static void FillExclusiveData(int item_idx, ItemData item, string sprite, string name, ExclusiveItemType type, ExclusiveItemEffect effect, object[] effectArgs, string dexFamily, bool translate)
        {
            string firstStage = dexFamily;
            MonsterData data = DataManager.Instance.GetMonster(firstStage);
            int prevo = data.PromoteFrom;
            while (prevo > -1)
            {
                firstStage = prevo.ToString();
                data = DataManager.Instance.GetMonster(firstStage);
                prevo = data.PromoteFrom;
            }
            List<string> dexNums = new List<string>();
            dexNums.Add(firstStage);
            FindEvos(dexNums, data);


            FillExclusiveData(item_idx, item, sprite, name, type, effect, effectArgs, dexFamily, dexNums, translate, true);
        }

        public static bool FindEvos(List<string> dexNums, MonsterData data)
        {
            bool branched = data.Promotions.Count > 1;
            foreach (PromoteBranch evo in data.Promotions)
            {
                dexNums.Add(evo.Result.ToString());
                MonsterData evoData = DataManager.Instance.GetMonster(evo.Result);
                branched |= FindEvos(dexNums, evoData);
            }
            return branched;
        }

        public static void FillExclusiveData(int item_idx, ItemData item, string sprite, string name, ExclusiveItemType type, ExclusiveItemEffect effect, object[] effectArgs, string primaryDexNum, List<string> dexNums, bool translate, bool family)
        {
            item.Sprite = sprite;

            LocalText localName = null;
            if (type != ExclusiveItemType.None)
            {
                if (name != "")
                {
                    localName = GetLocalExpression(type, translate);
                    localName = LocalText.FormatLocalText(localName, new LocalText(name));
                }
                else
                {
                    MonsterData data = DataManager.Instance.GetMonster(primaryDexNum);
                    localName = GetLocalExpression(type, translate);
                    localName = LocalText.FormatLocalText(localName, data.Name);
                }
            }
            else
            {
                //custom name; leave as is.
                //when printing out to string table, the custom names are detected using exclusive item type checking

                //if translating, draw from translation list (should contain the original name as the default text)
                if (translate && name != "")
                    localName = specificItemRows[item_idx.ToString("D4") + "-" + 0.ToString("D4") + "|data.Name"];
                else
                    localName = new LocalText(name);
            }
            item.Name = localName;
            item.ItemStates.Set(new ExclusiveState(type));
            item.ItemStates.Set(new FamilyState(dexNums.ToArray()));

            LocalText monsterNames = null;
            if (family)
                monsterNames = DataManager.Instance.GetMonster(primaryDexNum).Name;
            else
            {
                List<LocalText> nameList = new List<LocalText>();
                foreach (string dexNum in dexNums)
                {
                    MonsterData data = DataManager.Instance.GetMonster(dexNum);
                    nameList.Add(data.Name);
                }
                monsterNames = BuildLocalTextList(nameList, translate);
            }

            LocalText reqStatement = GetExclusiveDescription(family, translate);

            List<LocalText> localArgs = new List<LocalText>();
            FillExclusiveEffects(item, localArgs, true, effect, effectArgs, translate);
            item.Desc = LocalText.FormatLocalText(item.Desc, localArgs.ToArray());

            item.Desc = LocalText.FormatLocalText(reqStatement, monsterNames, item.Desc);
        }


        public static void FillExclusiveTypeData(int item_idx, ItemData item, string name, ExclusiveItemEffect effect, object[] effectArgs, string element, bool translate)
        {
            //set name
            if (translate)
                item.Name = specificItemRows[item_idx.ToString("D4") + "-" + 0.ToString("D4") + "|data.Name"];
            else
                item.Name = new LocalText(name);
            
            item.ItemStates.Set(new ExclusiveState());

            //desc: Item for a type
            //An exclusive item for {0}-type Pokémon.
            LocalText reqStatement = GetExclusiveTypeDescription(translate);
            ElementData elementData = DataManager.Instance.GetElement(element);

            List<object> newArgs = new List<object>();
            newArgs.Add(element);
            newArgs.AddRange(effectArgs);

            List<LocalText> localArgs = new List<LocalText>();
            FillExclusiveEffects(item, localArgs, true, effect, newArgs.ToArray(), translate);
            item.Desc = LocalText.FormatLocalText(item.Desc, localArgs.ToArray());

            item.Desc = LocalText.FormatLocalText(reqStatement, elementData.Name, item.Desc);
        }


        public static LocalText GetExclusiveDescription(bool family, bool translate)
        {
            if (translate)
            {
                if (family)
                    return specialRows["exclFormatDescFamily"];
                else
                    return specialRows["exclFormatDesc"];
            }
            else
            {
                if (family)
                    return new LocalText("An exclusive item for the {0} family. {1}");
                else
                    return new LocalText("An exclusive item for {0}. {1}");
            }
        }

        public static LocalText GetExclusiveTypeDescription(bool translate)
        {
            if (translate)
                return specialRows["exclFormatDescType"];
            else
                return new LocalText("An exclusive item for {0}-type Pokémon. {1}");
        }



        public static LocalText BuildLocalTextList(List<LocalText> localTexts, bool translate)
        {
            List<string> defaultList = new List<string>();
            Dictionary<string, List<string>> langList = new Dictionary<string, List<string>>();

            foreach (LocalText name in localTexts)
            {
                defaultList.Add(name.DefaultText);
                foreach (string key in name.LocalTexts.Keys)
                {
                    if (!langList.ContainsKey(key))
                        langList[key] = new List<string>();
                    langList[key].Add(name.LocalTexts[key]);
                }
            }

            if (translate)
            {
                LocalText localList = new LocalText(BuildList(defaultList.ToArray(), ""));
                foreach (string key in langList.Keys)
                    localList.LocalTexts[key] = BuildList(langList[key].ToArray(), key);
                return localList;
            }
            else
                return new LocalText(Text.BuildList(defaultList.ToArray()));
        }

        public static LocalText ToLocalText<T>(T value, LocalText parent, bool translate) where T : Enum
        {
            if (translate)
            {
                LocalText typeNames = new LocalText(ToCulture(value, ""));
                foreach (string lang in parent.LocalTexts.Keys)
                    typeNames.LocalTexts[lang] = ToCulture(value, lang);
                return typeNames;
            }
            else
                return new LocalText(value.ToLocal());
        }


        private static string ToCulture<T>(T value, string code) where T : Enum
        {
            string key = "_ENUM_" + typeof(T).Name + "_" + value;
            string text;
            if (!stringsAll[code].TryGetValue(key, out text))
                text = stringsAll[""][key];
            if (!String.IsNullOrEmpty(text))
                return text;
            return value.ToString();
        }


        private static string BuildList(string[] input, string code)
        {
            StringBuilder totalString = new StringBuilder();
            for (int ii = 0; ii < input.Length; ii++)
            {
                if (ii > 0)
                {
                    if (input.Length > 2)
                        totalString.Append(", ");
                    else if (ii == input.Length - 1)
                    {
                        string key = "ADD_END";
                        string text;
                        if (!stringsAll[code].TryGetValue(key, out text))
                            text = stringsAll[""][key];
                        totalString.Append(text + " ");
                    }
                }
                totalString.Append(input[ii]);
            }
            return totalString.ToString();
        }
    }
}
