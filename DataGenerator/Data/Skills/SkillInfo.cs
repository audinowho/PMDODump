using System;
using System.Collections.Generic;
using RogueEssence.Dungeon;
using RogueEssence.Content;
using RogueElements;
using RogueEssence;
using RogueEssence.Data;
using PMDC.Dungeon;
using PMDC;
using PMDC.Data;

namespace DataGenerator.Data
{
    public partial class SkillInfo
    {
        public const int MAX_SKILLS = 901;


        public static void AddUnreleasedMoveData()
        {
            for (int ii = 0; ii < MAX_SKILLS; ii++)
            {

                (string, SkillData) move = GetSkillData(ii);
                if (!move.Item2.Released)
                    DataManager.SaveData(move.Item1, DataManager.DataType.Skill.ToString(), move.Item2);
            }
        }

        public static void AddMoveDataToAnims(params int[] movesToAdd)
        {
            if (movesToAdd.Length > 0)
            {
                for (int ii = 0; ii < movesToAdd.Length; ii++)
                {

                    (string, SkillData) move = GetSkillData(movesToAdd[ii]);
                    SkillData oldMove = DataManager.LoadData<SkillData>(move.Item1, DataManager.DataType.Skill.ToString(), ".json");
                    if (oldMove != null)
                    {
                        oldMove.Data.BeforeHits = move.Item2.Data.BeforeHits;
                        DataManager.SaveData(move.Item1, DataManager.DataType.Skill.ToString(), oldMove);
                    }
                }
            }
            else
            {
                for (int ii = 0; ii < MAX_SKILLS; ii++)
                {
                    (string, SkillData) move = GetSkillData(ii);
                    SkillData oldMove = DataManager.LoadData<SkillData>(move.Item1, DataManager.DataType.Skill.ToString(), ".json");
                    if (oldMove != null)
                    {
                        oldMove.Data.OnActions = move.Item2.Data.OnActions;
                        DataManager.SaveData(move.Item1, DataManager.DataType.Skill.ToString(), oldMove);
                    }
                }
            }
        }

        public static void AddMoveData(params int[] movesToAdd)
        {
            if (movesToAdd.Length > 0)
            {
                for (int ii = 0; ii < movesToAdd.Length; ii++)
                {

                    (string, SkillData) move = GetSkillData(movesToAdd[ii]);
                    DataManager.SaveData(move.Item1, DataManager.DataType.Skill.ToString(), move.Item2);
                }
            }
            else
            {
                for (int ii = 0; ii < MAX_SKILLS; ii++)
                {
                    (string, SkillData) move = GetSkillData(ii);
                    //System.Diagnostics.Debug.WriteLine(String.Format("{0}\t{1}", ii, move.Item1));
                    DataManager.SaveData(move.Item1, DataManager.DataType.Skill.ToString(), move.Item2);
                }
            }
        }


        public static (string, SkillData) GetSkillData(int ii)
        {
            SkillData skill = new SkillData();
            string fileName = "";
            skill.IndexNum = ii;

            if (ii < 468)
                FillSkillsPMD(skill, ii, ref fileName);
            else
                FillSkillsGen5Plus(skill, ii, ref fileName);

            if (skill.Name.DefaultText.StartsWith("**"))
                skill.Name.DefaultText = skill.Name.DefaultText.Replace("*", "");
            else if (skill.Name.DefaultText != "")
                skill.Released = true;

            if (skill.Name.DefaultText.StartsWith("-"))
            {
                skill.Name.DefaultText = skill.Name.DefaultText.Substring(1);
                skill.Comment = "No Anim";
                skill.Released = true;
            }
            else if (skill.Name.DefaultText.StartsWith("="))
            {
                skill.Name.DefaultText = skill.Name.DefaultText.Substring(1);
                skill.Comment = "No Sound";
                skill.Released = true;
            }

            if (fileName == "")
                fileName = Text.Sanitize(skill.Name.DefaultText).ToLower();

            return (fileName, skill);
        }
        
    }
}