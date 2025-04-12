using System;
using System.Collections.Generic;
using RogueEssence.Dungeon;
using RogueEssence.Content;
using RogueElements;
using RogueEssence;
using RogueEssence.Data;
using PMDC;
using PMDC.Data;

namespace DataGenerator.Data
{
    public static class RankInfo
    {

        public enum TeamRank
        {
            Unknown = -1,
            None,
            Normal,
            Bronze,
            Silver,
            Gold,
            Platinum,
            Diamond,
            Super,
            Ultra,
            Hyper,
            Master,
            Grandmaster
        }

        public static int[] RANK_NEXT = new int[] { 1,
                                                    100,
                                                    300,
                                                    1200,
                                                    1600,
                                                    3200,
                                                    6400,
                                                    12000,
                                                    20000,
                                                    50000,
                                                    100000,
                                                    0};

        public const int MAX_GROUPS = 12;
        public static void AddRankData()
        {
            DataInfo.DeleteIndexedData(DataManager.DataType.Rank.ToString());
            for (int ii = 0; ii < MAX_GROUPS; ii++)
            {
                string next = "";
                if (ii < MAX_GROUPS - 1)
                    next = Text.Sanitize(Text.GetMemberTitle(((TeamRank)ii + 1).ToString())).ToLower();
                RankData data = new RankData(new LocalText(Text.GetMemberTitle(((TeamRank)ii).ToString())), 24, RANK_NEXT[ii], next);
                if (ii == (int)TeamRank.None)
                    data.BagSize = 24;
                else if (ii == (int)TeamRank.Normal)
                    data.BagSize = 24;
                else if (ii == (int)TeamRank.Bronze)
                    data.BagSize = 32;
                else if (ii == (int)TeamRank.Silver)
                    data.BagSize = 40;
                else
                    data.BagSize = 48;
                DataManager.SaveEntryData(Text.Sanitize(data.Name.DefaultText).ToLower(), DataManager.DataType.Rank.ToString(), data);
            }
        }

        public static void AddMinRankData()
        {
            DataInfo.DeleteIndexedData(DataManager.DataType.Rank.ToString());
            for (int ii = 0; ii < 1; ii++)
            {
                string next = "";
                RankData data = new RankData(new LocalText(Text.GetMemberTitle(((TeamRank)ii).ToString())), 24, RANK_NEXT[ii], next);
                data.BagSize = 24;
                DataManager.SaveEntryData(Text.Sanitize(data.Name.DefaultText).ToLower(), DataManager.DataType.Rank.ToString(), data);
            }
        }
    }
}

