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
                RankData data = new RankData(new LocalText(((TeamRank)ii).ToString()), 24, RANK_NEXT[ii]);
                DataManager.SaveData(ii, DataManager.DataType.Rank.ToString(), data);
            }
        }

    }
}

