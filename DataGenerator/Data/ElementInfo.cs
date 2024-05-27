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
    public static class ElementInfo
    {


        public enum Element
        {
            None = 0,
            Bug = 1,
            Dark = 2,
            Dragon = 3,
            Electric = 4,
            Fairy = 5,
            Fighting = 6,
            Fire = 7,
            Flying = 8,
            Ghost = 9,
            Grass = 10,
            Ground = 11,
            Ice = 12,
            Normal = 13,
            Poison = 14,
            Psychic = 15,
            Rock = 16,
            Steel = 17,
            Water = 18
        }

        public const int MAX_ELEMENTS = 19;
        public static void AddElementData()
        {
            DataInfo.DeleteIndexedData(DataManager.DataType.Element.ToString());
            for (int ii = 0; ii < MAX_ELEMENTS; ii++)
            {
                ElementData element = new ElementData(new LocalText(((Element)ii).ToString()), (char)(ii + 0xE080));
                DataManager.SaveEntryData(Text.Sanitize(element.Name.DefaultText).ToLower(), DataManager.DataType.Element.ToString(), element);
            }
        }

    }
}

