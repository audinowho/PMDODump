﻿using System;
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
    public static class GrowthInfo
    {

        public enum GrowthGroup
        {
            None = -1,
            Erratic = 0,
            Fast,
            MediumFast,
            MediumSlow,
            Slow,
            Fluctuating
        }

        static int[,] EXP_CURVES = { { 0, 0, 0, 0, 0, 0 },
                                    { 15, 6, 8, 9, 10, 4 },
                                    { 52, 21, 27, 57, 33, 13 },
                                    { 122, 51, 64, 96, 80, 32 },
                                    { 237, 100, 125, 135, 156, 65 },
                                    { 406, 172, 216, 179, 270, 112 },
                                    { 637, 274, 343, 236, 428, 178 },
                                    { 942, 409, 512, 314, 640, 276 },
                                    { 1326, 583, 729, 419, 911, 393 },
                                    { 1800, 800, 1000, 560, 1250, 540 },
                                    { 2369, 1064, 1331, 742, 1663, 745 },
                                    { 3041, 1382, 1728, 973, 2160, 967 },
                                    { 3822, 1757, 2197, 1261, 2746, 1230 },
                                    { 4719, 2195, 2744, 1612, 3430, 1591 },
                                    { 5737, 2700, 3375, 2035, 4218, 1957 },
                                    { 6881, 3276, 4096, 2535, 5120, 2457 },
                                    { 8155, 3930, 4913, 3120, 6141, 3046 },
                                    { 9564, 4665, 5832, 3798, 7290, 3732 },
                                    { 11111, 5487, 6859, 4575, 8573, 4526 },
                                    { 12800, 6400, 8000, 5460, 10000, 5440 },
                                    { 14632, 7408, 9261, 6458, 11576, 6482 },
                                    { 16610, 8518, 10648, 7577, 13310, 7666 },
                                    { 18737, 9733, 12167, 8825, 15208, 9003 },
                                    { 21012, 11059, 13824, 10208, 17280, 10506 },
                                    { 23437, 12500, 15625, 11735, 19531, 12187 },
                                    { 26012, 14060, 17576, 13411, 21970, 14060 },
                                    { 28737, 15746, 19683, 15244, 24603, 16140 },
                                    { 31610, 17561, 21952, 17242, 27440, 18439 },
                                    { 34632, 19511, 24389, 19411, 30486, 20974 },
                                    { 37800, 21600, 27000, 21760, 33750, 23760 },
                                    { 41111, 23832, 29791, 24294, 37238, 26811 },
                                    { 44564, 26214, 32768, 27021, 40960, 30146 },
                                    { 48155, 28749, 35937, 29949, 44921, 33780 },
                                    { 51881, 31443, 39304, 33084, 49130, 37731 },
                                    { 55737, 34300, 42875, 36435, 53593, 42017 },
                                    { 59719, 37324, 46656, 40007, 58320, 46656 },
                                    { 63822, 40522, 50653, 43808, 63316, 50653 },
                                    { 68041, 43897, 54872, 47846, 68590, 55969 },
                                    { 72369, 47455, 59319, 52127, 74148, 60505 },
                                    { 76800, 51200, 64000, 56660, 80000, 66560 },
                                    { 81326, 55136, 68921, 61450, 86151, 71677 },
                                    { 85942, 59270, 74088, 66505, 92610, 78533 },
                                    { 90637, 63605, 79507, 71833, 99383, 84277 },
                                    { 95406, 68147, 85184, 77440, 106480, 91998 },
                                    { 100237, 72900, 91125, 83335, 113906, 98415 },
                                    { 105122, 77868, 97336, 89523, 121670, 107069 },
                                    { 110052, 83058, 103823, 96012, 129778, 114205 },
                                    { 115015, 88473, 110592, 102810, 138240, 123863 },
                                    { 120001, 94119, 117649, 109923, 147061, 131766 },
                                    { 125000, 100000, 125000, 117360, 156250, 142500 },
                                    { 131324, 106120, 132651, 125126, 165813, 151222 },
                                    { 137795, 112486, 140608, 133229, 175760, 163105 },
                                    { 144410, 119101, 148877, 141677, 186096, 172697 },
                                    { 151165, 125971, 157464, 150476, 196830, 185807 },
                                    { 158056, 133100, 166375, 159635, 207968, 196322 },
                                    { 165079, 140492, 175616, 169159, 219520, 210739 },
                                    { 172229, 148154, 185193, 179056, 231491, 222231 },
                                    { 179503, 156089, 195112, 189334, 243890, 238036 },
                                    { 186894, 164303, 205379, 199999, 256723, 250562 },
                                    { 194400, 172800, 216000, 211060, 270000, 267840 },
                                    { 202013, 181584, 226981, 222522, 283726, 281456 },
                                    { 209728, 190662, 238328, 234393, 297910, 300293 },
                                    { 217540, 200037, 250047, 246681, 312558, 315059 },
                                    { 225443, 209715, 262144, 259392, 327680, 335544 },
                                    { 233431, 219700, 274625, 272535, 343281, 351520 },
                                    { 241496, 229996, 287496, 286115, 359370, 373744 },
                                    { 249633, 240610, 300763, 300140, 375953, 390991 },
                                    { 257834, 251545, 314432, 314618, 393040, 415050 },
                                    { 267406, 262807, 328509, 329555, 410636, 433631 },
                                    { 276458, 274400, 343000, 344960, 428750, 459620 },
                                    { 286328, 286328, 357911, 360838, 447388, 479600 },
                                    { 296358, 298598, 373248, 377197, 466560, 507617 },
                                    { 305767, 311213, 389017, 394045, 486271, 529063 },
                                    { 316074, 324179, 405224, 411388, 506530, 559209 },
                                    { 326531, 337500, 421875, 429235, 527343, 582187 },
                                    { 336255, 351180, 438976, 447591, 548720, 614566 },
                                    { 346965, 365226, 456533, 466464, 570666, 639146 },
                                    { 357812, 379641, 474552, 485862, 593190, 673863 },
                                    { 367807, 394431, 493039, 505791, 616298, 700115 },
                                    { 378880, 409600, 512000, 526260, 640000, 737280 },
                                    { 390077, 425152, 531441, 547274, 664301, 765275 },
                                    { 400293, 441094, 551368, 568841, 689210, 804997 },
                                    { 411686, 457429, 571787, 590969, 714733, 834809 },
                                    { 423190, 474163, 592704, 613664, 740880, 877201 },
                                    { 433572, 491300, 614125, 636935, 767656, 908905 },
                                    { 445239, 508844, 636056, 660787, 795070, 954084 },
                                    { 457001, 526802, 658503, 685228, 823128, 987754 },
                                    { 467489, 545177, 681472, 710266, 851840, 1035837 },
                                    { 479378, 563975, 704969, 735907, 881211, 1071552 },
                                    { 491346, 583200, 729000, 762160, 911250, 1122660 },
                                    { 501878, 602856, 753571, 789030, 941963, 1160499 },
                                    { 513934, 622950, 778688, 816525, 973360, 1214753 },
                                    { 526049, 643485, 804357, 844653, 1005446, 1254796 },
                                    { 536557, 664467, 830584, 873420, 1038230, 1312322 },
                                    { 548720, 685900, 857375, 902835, 1071718, 1354652 },
                                    { 560922, 707788, 884736, 932903, 1105920, 1415577 },
                                    { 571333, 730138, 912673, 963632, 1140841, 1460276 },
                                    { 583539, 752953, 941192, 995030, 1176490, 1524731 },
                                    { 591882, 776239, 970299, 1027103, 1212873, 1571884 },
                                    { 600000, 800000, 1000000, 1059860, 1250000, 1640000 } };


        public const int MAX_GROUPS = 6;
        public static void AddGrowthGroupData()
        {
            DataInfo.DeleteIndexedData(DataManager.DataType.GrowthGroup.ToString());
            for (int ii = 0; ii < MAX_GROUPS; ii++)
            {
                List<int> exp = new List<int>();
                for (int jj = 0; jj < EXP_CURVES.GetLength(0); jj++)
                {
                    exp.Add(EXP_CURVES[jj,ii]);
                }
                GrowthData skillGroup = new GrowthData(new LocalText(Text.GetMemberTitle(((GrowthGroup)ii).ToString())), exp.ToArray());
                DataManager.SaveEntryData(Text.Sanitize(skillGroup.Name.DefaultText).ToLower(), DataManager.DataType.GrowthGroup.ToString(), skillGroup);
            }
        }
        public static void AddMinGrowthGroupData()
        {
            DataInfo.DeleteIndexedData(DataManager.DataType.GrowthGroup.ToString());

            List<int> exp = new List<int>();
            for (int jj = 0; jj < EXP_CURVES.GetLength(0); jj++)
            {
                exp.Add(jj * 100);
            }
            GrowthData skillGroup = new GrowthData(new LocalText(Text.GetMemberTitle(GrowthGroup.Slow.ToString())), exp.ToArray());
            DataManager.SaveEntryData(Text.Sanitize(skillGroup.Name.DefaultText).ToLower(), DataManager.DataType.GrowthGroup.ToString(), skillGroup);
        }

    }
}

