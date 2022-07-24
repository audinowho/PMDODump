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
    public static class EmoteInfo
    {
        public const int MAX_EMOTES = 10;
        public static void AddEmoteData()
        {
            DataInfo.DeleteIndexedData(DataManager.DataType.Emote.ToString());
            for (int ii = 1; ii < MAX_EMOTES; ii++)
            {
                EmoteData emote = GetEmoteData(ii);
                DataManager.SaveData(Text.Sanitize(emote.Name.DefaultText).ToLower(), DataManager.DataType.Emote.ToString(), emote);
            }
        }

        public static EmoteData GetEmoteData(int ii)
        {
            if (ii == 0)
            {
                return new EmoteData();
            }
            else if (ii == 1)
            {
                //happylines: 16 height offset, 10 frames
                return new EmoteData(new LocalText("Happy"), new AnimData("Emote_Happy", 10), 16);
            }
            else if (ii == 2)
            {
                //notice: 4 height offset, 1 frame
                return new EmoteData(new LocalText("Notice"), new AnimData("Emote_Notice", 1), 4);
                //sounds: 567
            }
            else if (ii == 3)
            {
                //exclaim: 24 height offset, 1 frame
                return new EmoteData(new LocalText("Exclaim"), new AnimData("Emote_Exclaim", 1), 24);
                //sounds: 567, 569, 570, 581
            }
            else if (ii == 4)
            {
                //glowing: 4 height offset, 2 frames, 1-pixel left
                return new EmoteData(new LocalText("Glowing"), new AnimData("Emote_Glowing", 2), 4);
            }
            else if (ii == 5)
            {
                //cry: 0 height offset, 2 frames, 1-pixel left
                return new EmoteData(new LocalText("Sweating"), new AnimData("Emote_Sweating", 2), 0);
                //sounds: 575
            }
            else if (ii == 6)
            {
                //question: 24 height offset, 4 frames
                return new EmoteData(new LocalText("Question"), new AnimData("Emote_Question", 4), 24);
                //sounds: 565, 566
            }
            else if (ii == 7)
            {
                //cross-vein: 6 height offset, 2 frames
                return new EmoteData(new LocalText("Angry"), new AnimData("Emote_Angry", 2), 6);
            }
            else if (ii == 8)
            {
                //surprise: 12 height offset, 1 frame
                return new EmoteData(new LocalText("Shock"), new AnimData("Emote_Shock", 1), 12);
                //sounds: 571, 572, 576
            }
            else if (ii == 9)
            {
                //sweatdrop: 8 height offset, 7 frames
                return new EmoteData(new LocalText("Sweatdrop"), new AnimData("Emote_Sweatdrop", 7), 8);
                //sounds: 564
            }

            return new EmoteData();
        }
    }
}

