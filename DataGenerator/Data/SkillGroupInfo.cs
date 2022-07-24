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
    public static class SkillGroupInfo
    {

        public enum EggGroup
        {
            Undiscovered = 0,
            Ditto = 1,
            Monster = 2,
            Water1 = 3,
            Bug = 4,
            Flying = 5,
            Field = 6,
            Fairy = 7,
            Grass = 8,
            Humanlike = 9,
            Water3 = 10,
            Mineral = 11,
            Amorphous = 12,
            Water2 = 13,
            Dragon = 14
        }

        public const int MAX_GROUPS = 15;
        public static void AddSkillGroupData()
        {
            DataInfo.DeleteIndexedData(DataManager.DataType.SkillGroup.ToString());
            for (int ii = 0; ii < MAX_GROUPS; ii++)
            {
                SkillGroupData skillGroup = new SkillGroupData(new LocalText(Text.GetMemberTitle(((EggGroup)ii).ToString())));
                DataManager.SaveData(Text.Sanitize(skillGroup.Name.DefaultText).ToLower(), DataManager.DataType.SkillGroup.ToString(), skillGroup);
            }
        }

    }
}

