using System;
using System.Collections.Generic;
using RogueEssence.Dungeon;
using RogueEssence.Content;
using RogueElements;
using RogueEssence;
using RogueEssence.Data;
using PMDC;
using PMDC.Data;
using Microsoft.Xna.Framework;

namespace DataGenerator.Data
{
    public static class SkinInfo
    {


        public enum Coloration
        {
            Unknown = -1,
            Normal = 0,
            Shiny = 1,
            SquareShiny = 2,
        }

        public const int MAX_GROUPS = 3;
        public static void AddSkinData()
        {
            DataInfo.DeleteIndexedData(DataManager.DataType.Skin.ToString());

            for (int ii = 0; ii < MAX_GROUPS; ii++)
            {
                SkinData data = new SkinData(new LocalText(ii > 0 ? "Shiny" : "Normal"), ii > 0 ? '\uE10C' : '\0');
                data.IndexNum = ii;
                string fileName = "";
                switch (ii)
                {
                    case 0:
                        {
                            fileName = "normal";
                            data.MinimapColor = Color.Red;
                        }
                        break;
                    case 1:
                        {
                            fileName = "shiny";
                            FiniteAreaEmitter emitter = new FiniteAreaEmitter(new AnimData("Screen_Sparkle_RSE", 5));
                            emitter.Range = GraphicsManager.TileSize;
                            emitter.Speed = GraphicsManager.TileSize * 2;
                            emitter.TotalParticles = 12;
                            emitter.Layer = DrawLayer.Front;
                            data.LeaderFX.Emitter = emitter;
                            data.LeaderFX.Sound = "EVT_CH14_Eye_Glint";
                            data.LeaderFX.Delay = 20;
                            data.Display = true;
                            data.MinimapColor = new Color(255, 0, 255);
                        }
                        break;
                    case 2:
                        {
                            fileName = "shiny_square";
                            FiniteAreaEmitter emitter = new FiniteAreaEmitter(new AnimData("Captivate_Sparkle", 2));
                            emitter.Range = GraphicsManager.TileSize;
                            emitter.Speed = GraphicsManager.TileSize * 2;
                            emitter.TotalParticles = 10;
                            emitter.Layer = DrawLayer.Front;
                            data.Comment = "Square";
                            data.LeaderFX.Emitter = emitter;
                            data.LeaderFX.Sound = "EVT_CH14_Eye_Glint";
                            data.LeaderFX.Delay = 20;
                            data.Challenge = true;
                            data.Display = true;
                            data.MinimapColor = new Color(255, 0, 255);
                        }
                        break;
                }
                DataManager.SaveData(fileName, DataManager.DataType.Skin.ToString(), data);
            }
        }

    }
}

