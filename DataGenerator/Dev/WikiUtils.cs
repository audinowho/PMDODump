using DynamicData;
using PMDC.Data;
using PMDC.Dev;
using PMDC.Dungeon;
using PMDC.LevelGen;
using RogueElements;
using RogueEssence;
using RogueEssence.Data;
using RogueEssence.Dungeon;
using RogueEssence.LevelGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataGenerator.Dev
{
    public static class WikiUtils
    {
        private const int TOTAL_CHUNKS = 60;


        public static void DeleteWiki()
        {
            if (Directory.Exists(Path.Join(GenPath.WIKI_PATH, "Data")))
                Directory.Delete(Path.Join(GenPath.WIKI_PATH, "Data"), true);
        }

        private static bool WriteToWiki(string name, string content)
        {
            if (!Directory.Exists(Path.Join(GenPath.WIKI_PATH, "Data")))
                Directory.CreateDirectory(Path.Join(GenPath.WIKI_PATH, "Data"));

            string endPath = Path.Join(GenPath.WIKI_PATH, "Data", name + ".txt");
            string endDirectory = Path.GetDirectoryName(endPath);

            if (!Directory.Exists(endDirectory))
                Directory.CreateDirectory(endDirectory);

            if (File.Exists(endPath))
            {
                Console.WriteLine("Path conflict: " + name);
                return false;
            }

            using (var fstream = File.CreateText(endPath))
            {
                fstream.WriteLine(content);

                fstream.Flush();
                fstream.Close();
            }
            return true;
        }

        private static bool hasUnown(string input)
        {
            bool hasUnown = false;
            foreach (char c in input)
            {
                if (c > '\uE000')
                {
                    hasUnown = true;
                    break;
                }
            }
            return hasUnown;
        }

        private static string substituteUnown(string input)
        {
            string output = "";
            foreach (char c in input)
            {
                if (c > '\uE000')
                {
                    output += (char)(c - '\uE000');
                }
                else
                    output += c;
            }
            return output;
        }

        public static void PrintItemWiki()
        {
            List<string> itemKeys = DataManager.Instance.DataIndices[DataManager.DataType.Item].GetOrderedKeys(true);
            Console.WriteLine("Creating item pages...");
            for (int ii = 0; ii < itemKeys.Count; ii++)
            {
                string key = itemKeys[ii];
                ItemData entry = DataManager.Instance.GetItem(key);
                if (entry.Released)
                {
                    string localName = entry.Name.ToLocal();
                    if (hasUnown(localName))
                        localName = substituteUnown(localName);
                    string fileContent = "{{{{{1|ItemData}}}" +
                        "\r\n|item_name=" + localName +
                        "\r\n|sprite=" + entry.Sprite + ".png" +
                        "\r\n|item_id=" + key +
                        "\r\n|is_edible=" + (entry.ItemStates.Contains<EdibleState>() ? "Yes" : "No") +
                        "\r\n|stack_size=" + Math.Max(1, entry.MaxStack) +
                        "\r\n|value=" + entry.Price +
                        "\r\n}}";

                    bool completed = WriteToWiki(localName + "/Data", fileContent);
                    if (!completed)
                        completed = WriteToWiki(localName + " (Item)/Data", fileContent);
                }
            }
        }
        public static void PrintAbilityWiki()
        {
            List<string> abilityKeys = DataManager.Instance.DataIndices[DataManager.DataType.Intrinsic].GetOrderedKeys(true);
            Console.WriteLine("Creating ability pages...");
            for (int ii = 0; ii < abilityKeys.Count; ii++)
            {
                string key = abilityKeys[ii];
                IntrinsicData entry = DataManager.Instance.GetIntrinsic(key);
                if (entry.Released)
                {
                    string localName = entry.Name.ToLocal();
                    string fileContent = "{{{{{1|AbilityData}}}" +
                        "\r\n|ability_name=" + localName +
                        "\r\n|ability_id=" + key +
                        "\r\n|description=" + entry.Desc.ToLocal() +
                        "\r\n}}";

                    bool completed = WriteToWiki(localName + "/Data", fileContent);
                    if (!completed)
                        completed = WriteToWiki(localName + " (Ability)/Data", fileContent);
                }
            }
        }

        public static void PrintMonsterWiki()
        {
            Dictionary<string, string> encounterDict = PrintEncounterWiki();

            List<string> itemKeys = DataManager.Instance.DataIndices[DataManager.DataType.Monster].GetOrderedKeys(true);
            for (int ii = 0; ii < itemKeys.Count; ii++)
            {
                string key = itemKeys[ii];
                MonsterData entry = DataManager.Instance.GetMonster(key);
                if (entry.Released && entry.IndexNum > 0)
                {
                    // Get the Pokemon name
                    string localName = entry.Name.ToLocal();
                    int lastValidForm = 0;
                    
                    for (int form = 0; form < entry.Forms.Count; form++)
                    {
                        MonsterFormData formData = (MonsterFormData)entry.Forms[form];
                        // Check if this is a cosmetic form
                        bool formIsCosmetic = false;
                        if (form > 0)
                        {
                            formIsCosmetic = StrategyGuide.EvaluateCosmeticForm(entry, form, lastValidForm);
                        }
                        // Console.WriteLine(localName + "_" + form + ": " + formIsCosmetic);

                        if (!formIsCosmetic && formData.Released)
                        {
                            // Set the last form used for comparison to cosmetic formes
                            lastValidForm = form;

                            string formName = formData.FormName.DefaultText;
                            string strippedName = formName.Replace(".", "").Replace(":", "").Replace("?", "Question Mark").Replace("!", "Exclamation Mark").Replace("%", " Percent").Replace(" ", "_");


                            // Get type names
                            ElementData element1 = DataManager.Instance.GetElement(formData.Element1);
                            ElementData element2 = DataManager.Instance.GetElement(formData.Element2);

                            // Get ability names
                            IntrinsicData intrinsic1 = DataManager.Instance.GetIntrinsic(formData.Intrinsic1);
                            IntrinsicData intrinsic2 = DataManager.Instance.GetIntrinsic(formData.Intrinsic2);
                            IntrinsicData intrinsic3 = DataManager.Instance.GetIntrinsic(formData.Intrinsic3);

                            // Create main Pokemon data entry
                            string dataFileContent = "{{{{{1|PokemonData}}}";
                            dataFileContent += "\r\n|pokemon_name=" + formName;
                            dataFileContent += "\r\n|pokemon_id=" + key;
                            if (entry.Forms.Count > 1)
                            {
                                dataFileContent += "\r\n|form_id=" + form;
                            }
                            dataFileContent += "\r\n|type1=" + element1.Name.DefaultText;
                            if (element2.Name.DefaultText != "None")
                            {
                                dataFileContent += "\r\n|type2=" + element2.Name.DefaultText;
                            }
                            dataFileContent += "\r\n|ability1=" + intrinsic1.Name.DefaultText;
                            if (intrinsic2.Name.DefaultText != "None")
                            {
                                dataFileContent += "\r\n|ability2=" + intrinsic2.Name.DefaultText;
                            }
                            if (intrinsic3.Name.DefaultText != "None")
                            {
                                dataFileContent += "\r\n|ability3=" + intrinsic3.Name.DefaultText;
                            }
                            dataFileContent += "\r\n|recruit=" + entry.JoinRate;
                            dataFileContent += "\r\n|portrait=Portrait_" + strippedName + ".png";
                            dataFileContent += "\r\n}}";

                            // Write main Pokemon data entry
                            bool completed = WriteToWiki(strippedName + "/Data", dataFileContent);
                            if (!completed) // Check for duplicate form name and append form number as a fallback
                                completed = WriteToWiki(strippedName + "_" + form + "/Data", dataFileContent);


                            // Write learnset data

                            // Level-up learnset
                            string learnsetFileContent = "<h6>By level up</h6>\n{|- class=\"wikitable\"\n{{LearnsetHeader}}\r\n";
                            for (int skill_index = 0; skill_index < formData.LevelSkills.Count; skill_index++)
                            {
                                LevelUpSkill level_up_skill = formData.LevelSkills[skill_index];
                                SkillData current_skill = DataManager.Instance.GetSkill(level_up_skill.Skill);
                                learnsetFileContent += ("|  " + level_up_skill.Level + " {{:" + current_skill.Name.DefaultText + "/Data|LearnsetRow}}\r\n");
                            }
                            // TM learnset
                            learnsetFileContent += "|}\r\n\r\n<h6>By TM</h6>\r\n{|- class=\"wikitable\"\r\n{{LearnsetHeader}}\r\n";
                            for (int skill_index = 0; skill_index < formData.TeachSkills.Count; skill_index++)
                            {
                                LearnableSkill learnable_skill = formData.TeachSkills[skill_index];
                                SkillData current_skill = DataManager.Instance.GetSkill(learnable_skill.Skill);
                                learnsetFileContent += ("| {{:" + current_skill.Name.DefaultText + "/Data|LearnsetRow}}\r\n");
                            }
                            // Tutor learnset
                            learnsetFileContent += "|}\r\n\r\n<h6>By Move Tutor</h6>\r\n{|- class=\"wikitable\"\r\n{{LearnsetHeader}}\r\n";
                            for (int skill_index = 0; skill_index < formData.SecretSkills.Count; skill_index++)
                            {
                                LearnableSkill learnable_skill = formData.SecretSkills[skill_index];
                                SkillData current_skill = DataManager.Instance.GetSkill(learnable_skill.Skill);
                                learnsetFileContent += ("| {{:" + current_skill.Name.DefaultText + "/Data|LearnsetRow}}\r\n");
                            }
                            // Tutor learnset
                            learnsetFileContent += "|}\r\n\r\n<h6>Egg Moves</h6>\r\n{|- class=\"wikitable\"\r\n{{LearnsetHeader}}\r\n";
                            for (int skill_index = 0; skill_index < formData.SharedSkills.Count; skill_index++)
                            {
                                LearnableSkill learnable_skill = formData.SharedSkills[skill_index];
                                SkillData current_skill = DataManager.Instance.GetSkill(learnable_skill.Skill);
                                learnsetFileContent += ("| {{:" + current_skill.Name.DefaultText + "/Data|LearnsetRow}}\r\n");
                            }
                            learnsetFileContent += "|}\r\n\r\n<noinclude>[[Category: Learnsets]]</noinclude>";

                            // Write main Pokemon data entry
                            bool learnset_completed = WriteToWiki(strippedName + "/Learnset", learnsetFileContent);
                            if (!learnset_completed) // Check for duplicate form name and append form number as a fallback
                                learnset_completed = WriteToWiki(strippedName + "_" + form + "/Learnset", learnsetFileContent);


                            // Write stats entry
                            string statsFileContent = "{{StatBars|" +
                                "\r\n|hp=" + formData.BaseHP +
                                "\r\n|atk=" + formData.BaseAtk +
                                "\r\n|def=" + formData.BaseDef +
                                "\r\n|spa=" + formData.BaseMAtk +
                                "\r\n|spd=" + formData.BaseMDef +
                                "\r\n|spe=" + formData.BaseSpeed +
                                "\r\n}}\n<noinclude>[[Category: Pokémon stat pages]]</noinclude>";

                            bool stats_completed = WriteToWiki(strippedName + "/Stats", statsFileContent);
                            if (!stats_completed) // Check for duplicate form name and append form number as a fallback
                                stats_completed = WriteToWiki(strippedName + "_" + form + "/Stats", statsFileContent);

                            // Write locations entry
                            if (encounterDict.ContainsKey(formName))
                            { 
                                string locationFileContent = encounterDict[formName];

                                bool location_completed = WriteToWiki(strippedName + "/Location", locationFileContent);
                                if (!location_completed) // Check for duplicate form name and append form number as a fallback
                                { 
                                    if (encounterDict.ContainsKey(formName + "_" + form))
                                    {
                                        locationFileContent = encounterDict[formName + "_" + form];
                                        location_completed = WriteToWiki(strippedName + "_" + form + "/Location", locationFileContent);
                                    }
                                    else
                                    {
                                        locationFileContent = "N/A";
                                        location_completed = WriteToWiki(strippedName + "_" + form + "/Location", locationFileContent);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static Dictionary<string, string> PrintEncounterWiki()
        {
            Dictionary<string, string> encounterDict = new Dictionary<string, string>();

            List<string> monsterKeys = DataManager.Instance.DataIndices[DataManager.DataType.Monster].GetOrderedKeys(true);

            Dictionary<MonsterID, HashSet<(string tag, ZoneLoc encounter)>> foundSpecies = DevHelper.GetAllAppearingMonsters(true);

            foreach (StartChar startchar in DataManager.Instance.Start.Chars)
                DevHelper.AddWithEvos(foundSpecies, new MonsterID(startchar.ID.Species, startchar.ID.Form, "", Gender.Unknown), "STARTER", ZoneLoc.Invalid);

            Console.WriteLine("Creating encounter pages...");
            for (int ii = 0; ii < monsterKeys.Count; ii++)
            {
                string key = monsterKeys[ii];
                MonsterEntrySummary summary = (MonsterEntrySummary)DataManager.Instance.DataIndices[DataManager.DataType.Monster].Get(key);
                MonsterData data = DataManager.Instance.GetMonster(key);
                int formIndexNumber = 0;
                for (int jj = 0; jj < summary.Forms.Count; jj++)
                {
                    MonsterFormData formData = (MonsterFormData)data.Forms[jj];
                    if (formData.Temporary)
                        continue;

                    string encounterStr = "UNKNOWN";
                    if (summary.Released && formData.Released)
                    {
                        MonsterID monId = new MonsterID(key, jj, "", Gender.Unknown);
                        if (foundSpecies.ContainsKey(monId))
                        {
                            bool evolve = false;
                            bool starter = false;

                            Dictionary<string, (Dictionary<string, HashSet<int>> specialDict, Dictionary<string, Dictionary<int, HashSet<int>>> floorDict)> foundDict = new Dictionary<string, (Dictionary<string, HashSet<int>> specialDict, Dictionary<string, Dictionary<int, HashSet<int>>> floorDict)>();

                            foreach ((string tag, ZoneLoc encounter) in foundSpecies[monId])
                            {
                                if (!foundDict.ContainsKey(tag))
                                    foundDict[tag] = (new Dictionary<string, HashSet<int>>(), new Dictionary<string, Dictionary<int, HashSet<int>>>());
                                Dictionary<string, HashSet<int>> specialDict = foundDict[tag].specialDict;
                                Dictionary<string, Dictionary<int, HashSet<int>>> floorDict = foundDict[tag].floorDict;

                                if (tag == "STARTER")
                                    starter = true;
                                else if (tag == "EVOLVE")
                                    evolve = true;
                                else if (encounter.StructID.ID == -1)
                                {
                                    if (!specialDict.ContainsKey(encounter.ID))
                                        specialDict[encounter.ID] = new HashSet<int>();
                                    specialDict[encounter.ID].Add(encounter.StructID.Segment);
                                }
                                else
                                {
                                    if (!floorDict.ContainsKey(encounter.ID))
                                        floorDict[encounter.ID] = new Dictionary<int, HashSet<int>>();
                                    if (!floorDict[encounter.ID].ContainsKey(encounter.StructID.Segment))
                                        floorDict[encounter.ID][encounter.StructID.Segment] = new HashSet<int>();
                                    floorDict[encounter.ID][encounter.StructID.Segment].Add(encounter.StructID.ID);
                                }
                            }

                            List<string> encounterMsg = new List<string>();

                            foreach (string tag in foundDict.Keys)
                            {
                                Dictionary<string, HashSet<int>> specialDict = foundDict[tag].specialDict;
                                Dictionary<string, Dictionary<int, HashSet<int>>> floorDict = foundDict[tag].floorDict;

                                foreach (string zz in DataManager.Instance.DataIndices[DataManager.DataType.Zone].GetOrderedKeys(true))
                                {
                                    ZoneData mainZone = DataManager.Instance.GetZone(zz);
                                    for (int yy = 0; yy < mainZone.Segments.Count; yy++)
                                    {
                                        if (specialDict.ContainsKey(zz) && specialDict[zz].Contains(yy))
                                        {
                                            string locString = String.Format("{0} {1}S", mainZone.Name.ToLocal(), yy + 1);
                                            string formattedZoneName = mainZone.Name.ToLocal();
                                            foreach (var step in mainZone.Segments[yy].ZoneSteps)
                                            {
                                                var startStep = step as FloorNameIDZoneStep;
                                                if (startStep != null)
                                                {
                                                    locString = LocalText.FormatLocalText(startStep.Name, "?").ToLocal().Replace('\n', ' ');
                                                    break;
                                                }
                                            }
                                            locString = locString.Replace(mainZone.Name.ToLocal(), formattedZoneName);
                                            if (tag != "")
                                                locString = String.Format("[{0}] [[{1}", tag, locString);
                                            else
                                                locString = "[[" + locString;
                                            int place = locString.LastIndexOf(" ");
                                            locString = locString.Remove(place, 1).Insert(place, "]] ");
                                            encounterMsg.Add(locString);
                                        }

                                        if (floorDict.ContainsKey(zz) && floorDict[zz].ContainsKey(yy))
                                        {
                                            List<string> ranges = StrategyGuide.combineFloorRanges(floorDict[zz][yy]);
                                            string rangeString = String.Join(",", ranges.ToArray());
                                            string formattedZoneName = mainZone.Name.ToLocal();
                                            string locString = String.Format("{0} {1}S {2}F", formattedZoneName, yy + 1, rangeString);
                                            foreach (var step in mainZone.Segments[yy].ZoneSteps)
                                            {
                                                var startStep = step as FloorNameIDZoneStep;
                                                if (startStep != null)
                                                {
                                                    locString = LocalText.FormatLocalText(startStep.Name, rangeString).ToLocal().Replace('\n', ' ');
                                                    break;
                                                }
                                            }
                                            locString = locString.Replace(mainZone.Name.ToLocal(), formattedZoneName);
                                            if (tag != "")
                                                locString = String.Format("[{0}] [[{1}", tag, locString);
                                            else
                                                locString = "[[" + locString;

                                            int place = locString.LastIndexOf(" ");
                                            locString = locString.Remove(place, 1).Insert(place, "]] ");

                                            encounterMsg.Add(locString);
                                        }
                                    }
                                }
                            }

                            if (evolve && encounterMsg.Count == 0)
                                encounterMsg.Add("Evolve");
                            else if (starter && encounterMsg.Count == 0)
                                encounterMsg.Add("Starter");

                            if (encounterMsg.Count > 0)
                                encounterStr = String.Join("\n", encounterMsg.ToArray());
                        }
                    }
                    else
                    {
                        encounterStr = "NO DATA";
                    }
                    string monsterName = formData.FormName.ToLocal();
                    if (encounterDict.ContainsKey(monsterName))
                    {
                        formIndexNumber = formIndexNumber + 1;
                        monsterName = monsterName + "_" + formIndexNumber.ToString();
                    }
                    else
                    {
                        formIndexNumber = 0;
                    }
                    //Console.WriteLine(monsterName + "\n " + encounterStr + "\n\n");
                    encounterDict.Add(monsterName, encounterStr);
                }
            }

            return encounterDict;
        }


        public static void PrintMonsterFamilyWiki()
        {
            // Get a list of first-form Pokemon to serve as the evolution tree's roots
            List<string> itemKeys = DataManager.Instance.DataIndices[DataManager.DataType.Monster].GetOrderedKeys(true);
            List<MonsterData> firstFormMonsters = new List<MonsterData>();
            for (int ii = 0; ii < itemKeys.Count; ii++)
            {
                string key = itemKeys[ii];
                MonsterData entry = DataManager.Instance.GetMonster(key);
                if (entry.Released)
                {
                    if (entry.PromoteFrom == "")
                    {
                        if (entry.IndexNum > 0)
                        {
                            firstFormMonsters.Add(entry);
                        }
                    }
                }
            }

            Console.WriteLine("Begin printing Pokemon families");
            // For each Pokemon, create lists containing each form's evolution tree
            Console.WriteLine("Creating monster family pages...");
            for (int ii = 0; ii < firstFormMonsters.Count; ii++)
            {
                List<List<MonsterFormData>> monsterFamilyData = new List<List<MonsterFormData>>();

                // Get the base form
                MonsterData startingMonster = firstFormMonsters[ii];

                bool singleStageFamily = true;
                int lastValidForm = 0;

                for (int form = 0; form < startingMonster.Forms.Count; form++)
                {
                    bool formIsCosmetic = false;
                    if (form > 0)
                    {
                        formIsCosmetic = StrategyGuide.EvaluateCosmeticForm(startingMonster, form, lastValidForm);
                    }
                    if (!formIsCosmetic)
                    {
                        // Set this form as the one to compare stats to
                        lastValidForm = form;

                        List<MonsterFormData> currentEvolutionBranch = new List<MonsterFormData>();
                        currentEvolutionBranch.Add((MonsterFormData)startingMonster.Forms[form]);

                        // Get list of valid evolutions
                        List<MonsterFormData> validEvolutions = StrategyGuide.EvaluateMonsterEvolution(startingMonster, form, startingMonster.Promotions);
                        for (int validEvolutionIndex = 0; validEvolutionIndex < validEvolutions.Count; validEvolutionIndex++)
                        {
                            singleStageFamily = false;
                            MonsterFormData currentEvolution = validEvolutions[validEvolutionIndex];
                            if (currentEvolution.Released)
                            {
                                if (currentEvolutionBranch.IndexOf(currentEvolution) == -1)
                                {
                                    currentEvolutionBranch.Add(currentEvolution);
                                }
                            }
                        }

                        // Add this branch of the family tree to the family list
                        monsterFamilyData.Add(currentEvolutionBranch);
                    }
                }

                // Keep track of names that have already been used in the data structure
                List<String> namesAlreadyUsed = new List<string>();
                List<String> redirectNames = new List<string>();
                int currentFormNumber = 0;

                // Print the Pokemon family page
                string fileContent = "__NOTOC__";

                for (int evolutionBranchIndex = 0; evolutionBranchIndex < monsterFamilyData.Count; evolutionBranchIndex++)
                {
                    // Create tabs for each evolution branch
                    fileContent += "\r\n\r\n<tabs>";

                    // For each Pokemon in the branch, add a tab for it
                    for(int familyMemberIndex = 0; familyMemberIndex < monsterFamilyData[evolutionBranchIndex].Count; familyMemberIndex++)
                    {
                        MonsterFormData currentMonsterForm = monsterFamilyData[evolutionBranchIndex][familyMemberIndex];

                        string formName = currentMonsterForm.FormName.DefaultText;
                        string strippedName = formName.Replace(".", "").Replace(":", "").Replace("?", "Question Mark").Replace("!", "Exclamation Mark").Replace("%", " Percent").Replace(" ", "_");

                        if (namesAlreadyUsed.Contains(strippedName))
                        {
                            currentFormNumber += 1;
                            strippedName = strippedName + "_" + currentFormNumber.ToString();
                        }
                        else
                        {
                            currentFormNumber = 0;
                        }

                        fileContent += ("\r\n<tab name=\"" + formName + "\">{{:" + strippedName + "/Data|PokemonInfobox}}</tab>");

                        namesAlreadyUsed.Add(strippedName);
                        redirectNames.Add(formName);
                    }

                    // End the tab
                    fileContent += "\r\n</tabs>";
                }
                /*
                foreach (string nameUsed in namesAlreadyUsed)
                {
                    Console.WriteLine(nameUsed);
                }
                */
                fileContent += "\r\n";

                // Write to file
                string firstFormStrippedName = startingMonster.Name.DefaultText;
                firstFormStrippedName = firstFormStrippedName.Replace(".", "").Replace(":", "").Replace("?", "Question Mark").Replace("!", "Exclamation Mark").Replace("%", " Percent").Replace(" ", "_");
                if (!singleStageFamily)
                {
                    firstFormStrippedName += "_family";
                }

                bool completed = WriteToWiki(firstFormStrippedName, fileContent);
                if (!completed) // Check for duplicate form name and append form number as a fallback
                    completed = WriteToWiki(firstFormStrippedName + " (Pokemon)", fileContent);

                // Create redirect pages
                if (!singleStageFamily)
                {
                    for (int redirectNameIndex = 0; redirectNameIndex < namesAlreadyUsed.Count; redirectNameIndex++)
                    {
                        if (redirectNameIndex == 0)
                        {
                            WriteToWiki(namesAlreadyUsed[redirectNameIndex], "#REDIRECT [[" + firstFormStrippedName.Replace("_", " ") + "]]");
                        }
                        else
                        {
                            WriteToWiki(namesAlreadyUsed[redirectNameIndex], "#REDIRECT [[" + firstFormStrippedName.Replace("_", " ") + "#" + redirectNames[redirectNameIndex] + "]]");
                        }
                    }
                }
            }
        }


        public static void PrintMoveWiki()
        {
            Console.WriteLine("Creating skill pages...");
            List<string> itemKeys = DataManager.Instance.DataIndices[DataManager.DataType.Skill].GetOrderedKeys(true);
            for (int ii = 0; ii < itemKeys.Count; ii++)
            {
                string key = itemKeys[ii];
                SkillData entry = DataManager.Instance.GetSkill(key);
                if (entry.Released)
                {
                    string localName = entry.Name.ToLocal();
                    string localDesc = entry.Desc.ToLocal();
                    ElementData elementEntry = DataManager.Instance.GetElement(entry.Data.Element);
                    BasePowerState powerState = entry.Data.SkillStates.GetWithDefault<BasePowerState>();
                    string target_string = entry.HitboxAction.GetTargetsString(false);
                    string target_string_plural = entry.HitboxAction.GetTargetsString(true);
                    string true_target_string = "";
                    string range_string = entry.HitboxAction.GetDescription();
                    if (range_string.StartsWith(target_string_plural + " in "))
                        true_target_string = target_string_plural;
                    else if (range_string.StartsWith(target_string + " in "))
                        true_target_string = target_string;
                    if (true_target_string != "")
                        range_string = range_string.Substring(true_target_string.Length + 4, range_string.Length - true_target_string.Length - 4);
                    string power_string = (powerState != null ? powerState.Power.ToString() : "--");
                    if (entry.Strikes > 1)
                        power_string += "x" + entry.Strikes;
                    string hit_string = (entry.Data.HitRate > 0 ? entry.Data.HitRate.ToString() : "--");
                    List<string> removals = new List<string>();
                    foreach (BattleEvent battleEvent in entry.Data.OnHitTiles.EnumerateInOrder())
                    {
                        if (battleEvent is RemoveItemEvent)
                            removals.Add("Destroys Items");
                        else if (battleEvent is RemoveTrapEvent)
                            removals.Add("Destroys Traps");
                        else if (battleEvent is RemoveTerrainStateEvent)
                        {
                            RemoveTerrainStateEvent removeTerrain = (RemoveTerrainStateEvent)battleEvent;
                            foreach (FlagType state in removeTerrain.States)
                            {
                                if (state.FullType == typeof(WallTerrainState))
                                    removals.Add("Breaks Walls");
                                else if (state.FullType == typeof(WaterTerrainState))
                                    removals.Add("Removes Water");
                                else if (state.FullType == typeof(LavaTerrainState))
                                    removals.Add("Removes Lava");
                                else if (state.FullType == typeof(AbyssTerrainState))
                                    removals.Add("Removes Pits");
                                else if (state.FullType == typeof(FoliageTerrainState))
                                    removals.Add("Removes Grass");
                            }
                        }
                        else if (battleEvent is ShatterTerrainEvent)
                        {
                            ShatterTerrainEvent removeTerrain = (ShatterTerrainEvent)battleEvent;
                            foreach (string state in removeTerrain.TileTypes)
                            {
                                if (state == "wall")
                                    removals.Add("Breaks Walls + Adjacents");
                            }
                        }
                    }

                    string terrain_string = "None";
                    if (removals.Count > 0)
                    {
                        terrain_string = String.Join("\n", removals);
                    }

                    string fileContent = "{{{{{1|MoveData}}}" +
                        "\r\n|move_name=" + localName +
                        "\r\n|move_id=" + key +
                        "\r\n|type=" + elementEntry.Name.ToLocal() +
                        "\r\n|category=" + entry.Data.Category.ToLocal() +
                        "\r\n|power=" + power_string +
                        "\r\n|accuracy=" + hit_string +
                        "\r\n|pp=" + entry.BaseCharges +
                        "\r\n|range=" + range_string +
                        "\r\n|target=" + true_target_string +
                        "\r\n|terrain_effects=" + terrain_string +
                        "\r\n|description=" + "[TMP] " + localDesc +
                        "\r\n}}";

                    bool completed = WriteToWiki(localName + "/Data", fileContent);
                    if (!completed)
                        completed = WriteToWiki(localName + " (Move)/Data", fileContent);
                }
            }
        }


        public static void PrintDungeonEncounterWiki()
        {
            List<string> dungeonList = new List<string>();
            dungeonList = DataManager.Instance.DataIndices[DataManager.DataType.Zone].GetOrderedKeys(true);

            for (int dungeonIndex = 0; dungeonIndex < dungeonList.Count; dungeonIndex++)
            {
                // Create list of dungeon spawns
                List<string[]> dungeonSpawnList = new List<string[]>();
                int conflictSegment = 0;

                // Get the dungeon's main zone
                ZoneData mainZone = DataManager.Instance.GetZone(dungeonList[dungeonIndex]);
                string mainZoneName = mainZone.Name.DefaultText;
                //Console.WriteLine(mainZone.Name.ToLocal());

                List<ZoneSegmentBase> segmentList = mainZone.Segments;
                for (int zoneIndex = 0; zoneIndex < segmentList.Count; zoneIndex++)
                {
                    // Create lists of Pokemon spawns
                    List<DungeonSpawnData> segmentSpawnList = new List<DungeonSpawnData>();
                    List<DungeonSpawnData> specialSpawnList = new List<DungeonSpawnData>();
                    List<DungeonSpawnData> vaultSpawnList = new List<DungeonSpawnData>();
                    List<StaticSpawnData> staticSpawnList = new List<StaticSpawnData>();

                    // Get the current dungeon segment
                    ZoneSegmentBase currentSegment = segmentList[zoneIndex];

                    // Variable to check basement floors
                    bool isBasementFloor = false;

                    // Look through current segment's global steps to find Pokemon spawning step
                    List<ZoneStep> zoneStepList = currentSegment.ZoneSteps;
                    string zoneName = "";
                    string trimmedZoneName = "";
                    foreach (ZoneStep step in zoneStepList)
                    {
                        Type zoneStepType = step.GetType();
                        if (zoneStepType == typeof(FloorNameDropZoneStep))
                        {
                            FloorNameDropZoneStep castZoneStep = (FloorNameDropZoneStep)step;
                            zoneName = castZoneStep.Name.ToLocal();
                            trimmedZoneName = zoneName.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Replace("B{0}F", "").Replace("{0}F", "").TrimEnd().Replace(" ", "_");
                            //Console.WriteLine(trimmedZoneName);

                            if (zoneName.Contains("B{0}"))
                            {
                                isBasementFloor = true;
                            }
                        }
                        if (zoneStepType == typeof(TeamSpawnZoneStep))
                        {
                            TeamSpawnZoneStep castZoneStep = (TeamSpawnZoneStep)step;
                            // Get list of regular dungeon enemies
                            SpawnRangeList<TeamMemberSpawn> spawnList = castZoneStep.Spawns;
                            // Make a data entry for each enemy
                            for (int spawnIndex = 0; spawnIndex < spawnList.Count; spawnIndex++)
                            {
                                // Get current enemy data
                                TeamMemberSpawn currentSpawn = spawnList.GetSpawn(spawnIndex);
                                MobSpawn currentMob = currentSpawn.Spawn;

                                IntRange floorRange = spawnList.GetSpawnRange(spawnIndex);
                                DungeonSpawnData encounterData = GetDungeonEncounterData(currentMob, currentSpawn, floorRange.Min, floorRange.Max, null, isBasementFloor);
                                segmentSpawnList.Add(encounterData);
                            }

                            // Get list of specific dungeon enemies
                            SpawnRangeList<SpecificTeamSpawner> specificSpawnList = castZoneStep.SpecificSpawns;
                            for (int specificSpawnIndex = 0; specificSpawnIndex < specificSpawnList.Count; specificSpawnIndex++)
                            {
                                // Get current enemy data
                                SpecificTeamSpawner currentSpecificSpawn = specificSpawnList.GetSpawn(specificSpawnIndex);
                                IntRange currentSpecificSpawnFloorRange = specificSpawnList.GetSpawnRange(specificSpawnIndex);

                                foreach (MobSpawn currentMob in currentSpecificSpawn.Spawns)
                                {
                                    DungeonSpawnData encounterData = GetDungeonEncounterData(currentMob, null, currentSpecificSpawnFloorRange.Min, currentSpecificSpawnFloorRange.Max, null, isBasementFloor);
                                    specialSpawnList.Add(encounterData);
                                }
                            }
                        }
                        if (zoneStepType == typeof(SpreadStepRangeZoneStep))
                        {
                            SpreadStepRangeZoneStep castZoneStep = (SpreadStepRangeZoneStep)step;
                            SpawnRangeList<IGenStep> spreadSteps = castZoneStep.Spawns;
                            for (int stepIndex = 0; stepIndex < spreadSteps.Count; stepIndex++)
                            {
                                // Get random mob placement through the dungeon
                                if (spreadSteps.GetSpawn(stepIndex) is IPlaceMobsStep)
                                {
                                    List<DungeonSpawnData> spawnList = EvaluateMobSpawnStep((IPlaceMobsStep)spreadSteps.GetSpawn(stepIndex));
                                    for (int i = 0; i < spawnList.Count; i++)
                                    {
                                        DungeonSpawnData spawnData = spawnList[i];
                                        spawnData.startFloor = castZoneStep.SpreadPlan.FloorRange.Min + 1;
                                        spawnData.endFloor = castZoneStep.SpreadPlan.FloorRange.Max + 1;
                                        specialSpawnList.Add(spawnData);
                                    }
                                }
                            }
                        }
                        if (zoneStepType == typeof(SpreadVaultZoneStep))
                        {
                            SpreadVaultZoneStep castZoneStep = (SpreadVaultZoneStep)step;
                            SpawnRangeList<MobSpawn> spawnList = castZoneStep.Mobs;
                            // Make a data entry for each enemy
                            for (int spawnIndex = 0; spawnIndex < spawnList.Count; spawnIndex++)
                            {
                                // Get current enemy data
                                MobSpawn currentMob = spawnList.GetSpawn(spawnIndex);

                                IntRange floorRange = spawnList.GetSpawnRange(spawnIndex);
                                DungeonSpawnData encounterData = GetDungeonEncounterData(currentMob, null, floorRange.Min, floorRange.Max, null, isBasementFloor);
                                vaultSpawnList.Add(encounterData);
                            }
                        }
                    }

                    // Look through floor gen steps
                    List<IFloorGen> floorGenList = new List<IFloorGen>();
                    if (currentSegment is LayeredSegment)
                    {
                        LayeredSegment currentLayeredSegment = (LayeredSegment)currentSegment;
                        floorGenList = currentLayeredSegment.Floors;
                    }
                    else if (currentSegment is SingularSegment)
                    {
                        SingularSegment currentSingularSegment = (SingularSegment)currentSegment;
                        floorGenList.Add(currentSingularSegment.BaseFloor);
                    }

                    for (int floorGenIndex = 0; floorGenIndex < floorGenList.Count; floorGenIndex++)
                    {
                        PriorityList<IGenStep> genStepList = new PriorityList<IGenStep>();
                        IFloorGen currentFloorGen = floorGenList[floorGenIndex];
                        if (floorGenList[floorGenIndex] is GridFloorGen)
                        {
                            GridFloorGen currentGen = (GridFloorGen)currentFloorGen;
                            RetrieveGenSteps<MapGenContext>(genStepList, currentGen);
                        }
                        if (floorGenList[floorGenIndex] is RoomFloorGen)
                        {
                            RoomFloorGen currentGen = (RoomFloorGen)currentFloorGen;
                            RetrieveGenSteps<ListMapGenContext>(genStepList, currentGen);
                        }
                        if (floorGenList[floorGenIndex] is LoadGen)
                        {
                            LoadGen currentGen = (LoadGen)currentFloorGen;
                            RetrieveGenSteps<MapLoadContext>(genStepList, currentGen);
                        }

                        IEnumerable<Priority> genStepListOfPriorities = genStepList.GetPriorities();
                        foreach (Priority currentPriority in genStepListOfPriorities)
                        {
                            IEnumerable<IGenStep> genStepsAtCurrentPriority = genStepList.GetItems(currentPriority);
                            foreach (IGenStep currentGenStep in genStepsAtCurrentPriority)
                            {
                                // Get special per-floor spawns
                                if (currentGenStep is IPlaceMobsStep)
                                {
                                    //Console.WriteLine(currentGenStep);
                                    List<DungeonSpawnData> spawnList = EvaluateMobSpawnStep((IPlaceMobsStep)currentGenStep);
                                    for (int i = 0; i < spawnList.Count; i++)
                                    {
                                        DungeonSpawnData spawnData = spawnList[i];
                                        spawnData.startFloor = floorGenIndex + 1;
                                        spawnData.endFloor = floorGenIndex + 1;
                                        spawnData.isBasement = isBasementFloor;
                                        //Console.WriteLine(spawnData);
                                        specialSpawnList.Add(spawnData);
                                    }
                                }

                                if (currentGenStep is GuardSealStep<MapGenContext>)
                                {
                                    GuardSealStep<MapGenContext> currentGuardSealGenStep = (GuardSealStep<MapGenContext>)currentGenStep;
                                    LoopedRand<MobSpawn> guardRand = (LoopedRand<MobSpawn>)currentGuardSealGenStep.Guards;
                                    SpawnList<MobSpawn> guardSpawns = (SpawnList<MobSpawn>)guardRand.Spawner;

                                    for (int guardSpawnIndex = 0; guardSpawnIndex < guardSpawns.Count; guardSpawnIndex++)
                                    {
                                        MobSpawn currentSpawn = guardSpawns.GetSpawn(guardSpawnIndex);
                                        DungeonSpawnData spawnData = GetDungeonEncounterData(currentSpawn);
                                        spawnData.startFloor = floorGenIndex + 1;
                                        spawnData.endFloor = floorGenIndex + 1;
                                        spawnData.isBasement = isBasementFloor;
                                        spawnData.extraFeatures.Add("Spawns once, guarding secret stairs");
                                        specialSpawnList.Add(spawnData);
                                    }
                                }

                                if (currentGenStep is MapNameIDStep<MapLoadContext>)
                                {
                                    MapNameIDStep<MapLoadContext> currentMapNameIDGenStep = (MapNameIDStep<MapLoadContext>)currentGenStep;
                                    zoneName = mainZoneName + " " + currentMapNameIDGenStep.Name.DefaultText;
                                    trimmedZoneName = zoneName.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Replace("B{0}F", "").Replace("{0}F", "").TrimEnd().Replace(" ", "_");
                                    //Console.WriteLine(trimmedZoneName);
                                }

                                // Get spawns in loaded map
                                if (currentGenStep is MappedRoomStep<MapLoadContext>)
                                {
                                    MappedRoomStep<MapLoadContext> currentMappedRoomGenStep = (MappedRoomStep<MapLoadContext>)currentGenStep;
                                    string mapID = currentMappedRoomGenStep.MapID;
                                    Map currentMap = DataManager.Instance.GetMap(mapID);

                                    if (zoneName == "")
                                    {
                                        zoneName = currentMap.Name.ToLocal();
                                        //Console.WriteLine(zoneName);
                                        trimmedZoneName = zoneName.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Replace("B{0}F", "").Replace("{0}F", "").TrimEnd().Replace(" ", "_");
                                        //Console.WriteLine(trimmedZoneName);
                                    }

                                    if (currentMap.MapTeams.Count > 0)
                                    {
                                        Team mapMobs = currentMap.MapTeams[0];
                                        foreach (Character currentMob in mapMobs.Players)
                                        {
                                            //Console.WriteLine(currentMob.Name);

                                            StaticSpawnData currentStaticSpawn = new StaticSpawnData();
                                            currentStaticSpawn.spawnName = currentMob.Name;
                                            currentStaticSpawn.level = currentMob.Level;
                                            currentStaticSpawn.gender = (int)currentMob.CurrentForm.Gender;

                                            foreach (SlotSkill currentSkill in currentMob.BaseSkills)
                                            {
                                                SkillData learnedSkill = DataManager.Instance.GetSkill(currentSkill.SkillNum);
                                                currentStaticSpawn.specifiedSkillsList.Add("[[" + learnedSkill.Name.ToLocal() + "]]");
                                            }

                                            currentStaticSpawn.spawnIntrinsic = DataManager.Instance.GetIntrinsic(currentMob.BaseIntrinsics[0]).Name.ToLocal();

                                            currentStaticSpawn.extraFeatures.Add("Max HP: " + currentMob.MaxHP.ToString());
                                            currentStaticSpawn.extraFeatures.Add("Attack: " + currentMob.Atk.ToString());
                                            currentStaticSpawn.extraFeatures.Add("Defense: " + currentMob.Def.ToString());
                                            currentStaticSpawn.extraFeatures.Add("Sp. Atk: " + currentMob.MAtk.ToString());
                                            currentStaticSpawn.extraFeatures.Add("Sp. Def: " + currentMob.MDef.ToString());
                                            currentStaticSpawn.extraFeatures.Add("Speed: " + currentMob.Speed.ToString());

                                            if (currentMob.EquippedItem.ID != "")
                                            {
                                                ItemData mobHeldItem = DataManager.Instance.GetItem(currentMob.EquippedItem.ID);
                                                currentStaticSpawn.extraFeatures.Add("Held: [[" + mobHeldItem.Name.ToLocal() + "]]");
                                            }


                                            staticSpawnList.Add(currentStaticSpawn);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Remove duplicate entries in special spawn list
                    for (int i = 0; i < specialSpawnList.Count; i++)
                    {
                        DungeonSpawnData currentEntry = specialSpawnList[i];
                        if (!currentEntry.isDuplicate)
                        {
                            // Check through the list to find duplicates
                            for (int j = i + 1; j < specialSpawnList.Count; j++)
                            {
                                DungeonSpawnData evaluatedEntry = specialSpawnList[j];

                                if (currentEntry.Equals(evaluatedEntry))
                                {
                                    currentEntry.endFloor = evaluatedEntry.endFloor;
                                    evaluatedEntry.isDuplicate = true;

                                    specialSpawnList.RemoveAt(i);
                                    specialSpawnList.Insert(i, currentEntry);

                                    specialSpawnList.RemoveAt(j);
                                    specialSpawnList.Insert(j, evaluatedEntry);
                                }
                            }
                        }
                    }
                    for(int i = specialSpawnList.Count - 1; i >= 0; i--)
                    {
                        if (specialSpawnList[i].isDuplicate)
                        {
                            specialSpawnList.RemoveAt(i);
                        }
                    }

                    // Sort spawn lists by start and end floor, then min and max level
                    segmentSpawnList.Sort(delegate (DungeonSpawnData x, DungeonSpawnData y)
                    {
                        if (x.startFloor != y.startFloor)
                        {
                            return x.startFloor.CompareTo(y.startFloor);
                        }
                        else if (x.endFloor != y.endFloor)
                        {
                            return x.endFloor.CompareTo(y.endFloor);
                        }
                        else if (x.minLevel != y.minLevel)
                        {
                            return x.minLevel.CompareTo(y.minLevel);
                        }
                        else
                        {
                            return x.maxLevel.CompareTo(y.maxLevel);
                        }
                    }
                    );
                    specialSpawnList.Sort(delegate (DungeonSpawnData x, DungeonSpawnData y)
                    {
                        if (x.startFloor != y.startFloor)
                        {
                            return x.startFloor.CompareTo(y.startFloor);
                        }
                        else if (x.endFloor != y.endFloor)
                        {
                            return x.endFloor.CompareTo(y.endFloor);
                        }
                        else if (x.minLevel != y.minLevel)
                        {
                            return x.minLevel.CompareTo(y.minLevel);
                        }
                        else
                        {
                            return x.maxLevel.CompareTo(y.maxLevel);
                        }
                    }
                    );

                    string fileContent = "";

                    // Output the spawn list
                    if (segmentSpawnList.Count > 0)
                    {
                        if (specialSpawnList.Count > 0 || vaultSpawnList.Count > 0)
                        {
                            fileContent += "=== Regular spawns ===\r\n\r\n{| class=\"wikitable\"\r\n{{EncounterHeader}}\r\n";
                        }
                        else
                        {
                            fileContent += "{| class=\"wikitable\"\r\n{{EncounterHeader}}\r\n";
                        }
                        foreach (DungeonSpawnData encounterData in segmentSpawnList)
                        {
                            fileContent += encounterData.ToString();
                        }
                        fileContent += "|}\r\n";
                    }

                    // Output the special spawn list
                    if (specialSpawnList.Count > 0)
                    {
                        fileContent += "\r\n=== Special spawns ===\r\n\r\n{| class=\"wikitable\"\r\n{{EncounterHeader}}\r\n";
                        foreach (DungeonSpawnData encounterData in specialSpawnList)
                        {
                            fileContent += encounterData.ToString();
                        }
                        fileContent += "|}\r\n";
                    }

                    // Output the vault spawn list
                    if (vaultSpawnList.Count > 0)
                    {
                        fileContent += "\r\n=== Vault spawns ===\r\n\r\n{| class=\"wikitable\"\r\n{{EncounterHeader}}\r\n";
                        foreach (DungeonSpawnData encounterData in vaultSpawnList)
                        {
                            fileContent += encounterData.ToString();
                        }
                        fileContent += "|}\r\n";
                    }

                    // Output the static spawn list
                    if (staticSpawnList.Count > 0)
                    {
                        fileContent += "\r\n=== Static spawns ===\r\n\r\n{| class=\"wikitable\"\r\n{{EncounterHeader}}\r\n";
                        foreach (StaticSpawnData encounterData in staticSpawnList)
                        {
                            fileContent += encounterData.ToString();
                        }
                        fileContent += "|}\r\n";
                    }

                    if (fileContent.Length > 0)
                    { 
                        string fileName = zoneName.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Replace("B{0}F", "").Replace("{0}F", "").TrimEnd().Replace(" ", "_");
                        bool completed = WriteToWiki(fileName + "/Encounters", fileContent);
                        if (!completed)
                        {
                            conflictSegment++;
                            completed = WriteToWiki(fileName + "_" + conflictSegment.ToString() + "/Encounters", fileContent);
                        }
                    }
                }
            }
        }

        public struct DungeonSpawnData()
        {
            public string spawnName;

            public int minLevel;
            public int maxLevel;
            public bool levelCanBeIncreased;

            public int startFloor;
            public int endFloor;
            public bool isBasement;

            public string spawnIntrinsic;

            public List<string> specifiedSkillsList = new List<string>();

            public List<string> extraFeatures = new List<string>();

            public bool isDuplicate = false;

            public void setDuplicate(bool isDupe)
            {
                isDuplicate = isDupe;
            }

            public bool Equals(DungeonSpawnData otherEntry)
            {
                bool answer = false;

                if (spawnName.Equals(otherEntry.spawnName))
                {
                    if (minLevel == otherEntry.minLevel && maxLevel == otherEntry.maxLevel)
                    {
                        if (spawnIntrinsic.Equals(otherEntry.spawnIntrinsic))
                        {
                            if (specifiedSkillsList.Except(otherEntry.specifiedSkillsList).Count() == 0)
                            {
                                if (extraFeatures.Except(otherEntry.extraFeatures).Count() == 0)
                                {
                                    answer = true;
                                }
                                answer = true;
                            }
                        }
                    }
                }

                return answer;
            }

            public override string ToString()
            {
                string encounterRow = "{{EncounterRow";

                // Spawn name step
                encounterRow += "\r\n|pokemon=" + spawnName;

                // Level range step
                string levelRangeString;
                if (minLevel == maxLevel)
                {
                    levelRangeString = minLevel.ToString();
                }
                else
                {
                    levelRangeString = minLevel.ToString() + "-" + maxLevel.ToString();
                }
                encounterRow += "\r\n|level=" + levelRangeString;
                if (levelCanBeIncreased)
                {
                    encounterRow += "+";
                }

                // Floor range step
                encounterRow += "\r\n|start_floor=" + startFloor;
                encounterRow += "\r\n|end_floor=" + endFloor;
                if (isBasement)
                {
                    encounterRow += "\r\n|is_basement=true";
                }

                // Ability step
                if (spawnIntrinsic != "")
                {
                    encounterRow += "\r\n|ability=" + spawnIntrinsic;
                }

                // Moves step
                if (specifiedSkillsList.Count > 0)
                {
                    string specifiedSkillsString = "";
                    for(int i = 0; i < specifiedSkillsList.Count; i++)
                    {
                        specifiedSkillsString += specifiedSkillsList[i];
                        if (i <  specifiedSkillsList.Count - 1)
                        {
                            specifiedSkillsString += "<br>";
                        }
                    }
                    encounterRow += "\r\n|moves=" + specifiedSkillsString;
                }

                // Notes step
                if (extraFeatures.Count > 0)
                {
                    string notes = "\r\n|notes=";

                    for (int i = 0; i < extraFeatures.Count; i++)
                    {
                        notes += extraFeatures[i];
                        if (i < extraFeatures.Count - 1)
                        {
                            notes += "<br>";
                        }
                    }

                    encounterRow += notes;
                }

                // Footer step
                encounterRow += "\r\n}}\r\n";

                return encounterRow;
            }
        }

        public struct StaticSpawnData()
        {
            public string spawnName;

            public int level;

            public int gender = -1;
            private static string[] genderStrings = ["Genderless", "Male", "Female"];

            public string spawnIntrinsic;

            public List<string> specifiedSkillsList = new List<string>();

            public List<string> extraFeatures = new List<string>();

            public bool isDuplicate = false;

            public void setDuplicate(bool isDupe)
            {
                isDuplicate = isDupe;
            }

            public bool Equals(StaticSpawnData otherEntry)
            {
                bool answer = false;

                if (spawnName.Equals(otherEntry.spawnName))
                {
                    if (level == otherEntry.level)
                    {
                        if (gender == otherEntry.gender)
                        {
                            if (spawnIntrinsic.Equals(otherEntry.spawnIntrinsic))
                            {
                                if (specifiedSkillsList.Except(otherEntry.specifiedSkillsList).Count() == 0)
                                {
                                    if (extraFeatures.Except(otherEntry.extraFeatures).Count() == 0)
                                    {
                                        answer = true;
                                    }
                                    answer = true;
                                }
                            }
                        }
                    }
                }

                return answer;
            }

            public override string ToString()
            {
                string encounterRow = "{{EncounterRow";

                // Spawn name step
                encounterRow += "\r\n|pokemon=" + spawnName;

                // Level range step
                encounterRow += "\r\n|level=" + level.ToString();

                // Gender step
                if (gender != -1)
                {
                    encounterRow += "\r\n|gender=" + genderStrings[gender];
                }

                // Ability step
                if (spawnIntrinsic != "")
                {
                    encounterRow += "\r\n|ability=" + spawnIntrinsic;
                }

                // Moves step
                if (specifiedSkillsList.Count > 0)
                {
                    string specifiedSkillsString = "";
                    for (int i = 0; i < specifiedSkillsList.Count; i++)
                    {
                        specifiedSkillsString += specifiedSkillsList[i];
                        if (i < specifiedSkillsList.Count - 1)
                        {
                            specifiedSkillsString += "<br>";
                        }
                    }
                    encounterRow += "\r\n|moves=" + specifiedSkillsString;
                }

                // Notes step
                if (extraFeatures.Count > 0)
                {
                    string notes = "\r\n|notes=";

                    for (int i = 0; i < extraFeatures.Count; i++)
                    {
                        notes += extraFeatures[i];
                        if (i < extraFeatures.Count - 1)
                        {
                            notes += "<br>";
                        }
                    }

                    encounterRow += notes;
                }

                // Footer step
                encounterRow += "\r\n}}\r\n";

                return encounterRow;
            }
        }

        public static void RetrieveGenSteps<T>(PriorityList<IGenStep> genStepList, MapGen<T> currentGen) where T : BaseMapGenContext
        {
            PriorityList<GenStep<T>> uncastGenStepList = currentGen.GenSteps;
            foreach (Priority currentPriority in uncastGenStepList.GetPriorities())
            {
                IEnumerable<IGenStep> genStepsAtCurrentPriority = uncastGenStepList.GetItems(currentPriority);
                foreach (IGenStep currentGenStep in genStepsAtCurrentPriority)
                {
                    genStepList.Add(currentPriority, currentGenStep);
                }
            }
        }

        public static List<DungeonSpawnData> EvaluateMobSpawnStep(IPlaceMobsStep evaluatedStep)
        {
            List<DungeonSpawnData> currentSpecialSpawns = new List<DungeonSpawnData>();
            //Console.WriteLine(evaluatedStep.Spawn.GetType());

            // Check for two spawner types
            ILoopedTeamSpawner loopedTeamSpawner = null;
            IPresetMultiTeamSpawner presetMultiTeamSpawner = null;
            if (evaluatedStep.Spawn is ILoopedTeamSpawner)
            {
                loopedTeamSpawner = (ILoopedTeamSpawner)evaluatedStep.Spawn;
            }
            if (evaluatedStep.Spawn is IPresetMultiTeamSpawner)
            {
                presetMultiTeamSpawner = (IPresetMultiTeamSpawner)evaluatedStep.Spawn;
            }

            bool isTerrainMobStep = false;
            string addedTerrainString = "";
            if (evaluatedStep is IPlaceTerrainMobsStep || evaluatedStep is IPlaceDisconnectedMobsStep)
            {
                isTerrainMobStep = true;
                List<ITile> acceptedTileList = new List<ITile>();

                if (evaluatedStep is IPlaceTerrainMobsStep)
                {
                    IPlaceTerrainMobsStep castStep = (IPlaceTerrainMobsStep)evaluatedStep;
                    acceptedTileList = castStep.AcceptedTiles;
                }
                else if (evaluatedStep is IPlaceDisconnectedMobsStep)
                {
                    IPlaceDisconnectedMobsStep castStep = (IPlaceDisconnectedMobsStep)evaluatedStep;
                    acceptedTileList = castStep.AcceptedTiles;
                }

                foreach (ITile currentTile in acceptedTileList)
                {
                    string currentTileString = currentTile.ToString();
                    if (currentTileString.Contains("Foliage"))
                    {
                        addedTerrainString = "Spawns in tall grass";
                    }
                    else if (currentTileString.Contains("Abyss"))
                    {
                        addedTerrainString = "Spawns on abyss tiles";
                    }
                    else if (currentTileString.Contains("Blocked"))
                    {
                        addedTerrainString = "Spawns in walls";
                    }
                    else if (currentTileString.Contains("Water"))
                    {
                        addedTerrainString = "Spawns on water tiles";
                    }
                    else if (currentTileString.Contains("Lava"))
                    {
                        addedTerrainString = "Spawns on lava tiles";
                    }
                }
            }

            if (loopedTeamSpawner != null)
            {
                List<MobSpawn> specificSpawns = new List<MobSpawn>();
                if (loopedTeamSpawner.Picker is SpecificTeamSpawner)
                {
                    SpecificTeamSpawner specificSpawner = (SpecificTeamSpawner)loopedTeamSpawner.Picker;
                    specificSpawns = specificSpawner.Spawns;
                }
                else if (loopedTeamSpawner.Picker is PoolTeamSpawner)
                {
                    PoolTeamSpawner specificSpawner = (PoolTeamSpawner)loopedTeamSpawner.Picker;
                    SpawnList<MobSpawn> poolSpawnList = specificSpawner.GetPossibleSpawns();
                    for(int currentSpawn = 0; currentSpawn < poolSpawnList.Count; currentSpawn++)
                    {
                        specificSpawns.Add(poolSpawnList.GetSpawn(currentSpawn));
                    }
                }
                

                // If there's a RandDecay for specific spawns per floor, keep track of it
                RandDecay currentRandDecaySpawner = new RandDecay(-1);
                if (loopedTeamSpawner.AmountSpawner.GetType() == typeof(RandDecay))
                {
                    currentRandDecaySpawner = (RandDecay)loopedTeamSpawner.AmountSpawner;
                }

                foreach (MobSpawn mobSpawn in specificSpawns)
                {
                    DungeonSpawnData currentSpawnData = GetDungeonEncounterData(mobSpawn);
                    // Only add this tag if there's a RandDecay
                    if (currentRandDecaySpawner.Min != -1)
                    {
                        currentSpawnData.extraFeatures.Add(String.Format("Spawns {0}-{1} times per floor; {2}% chance<br>Does not respawn", currentRandDecaySpawner.Min, currentRandDecaySpawner.Max, currentRandDecaySpawner.Rate));
                    }
                    // Check for terrain the mob spawns on
                    if (isTerrainMobStep && addedTerrainString.Length > 0)
                    {
                        currentSpawnData.extraFeatures.Add(addedTerrainString);
                    }
                    currentSpecialSpawns.Add(currentSpawnData);
                }
            }
            if (presetMultiTeamSpawner != null)
            {
                List<SpecificTeamSpawner> spawnerList = presetMultiTeamSpawner.Spawns;
                foreach(SpecificTeamSpawner currentSpawner in spawnerList)
                {
                    List<MobSpawn> specificSpawns = currentSpawner.Spawns;
                    foreach (MobSpawn mobSpawn in specificSpawns)
                    {
                        DungeonSpawnData currentSpawnData = GetDungeonEncounterData(mobSpawn);
                        // Check for terrain the mob spawns on
                        if (isTerrainMobStep && addedTerrainString.Length > 0)
                        {
                            currentSpawnData.extraFeatures.Add(addedTerrainString);
                        }
                        currentSpecialSpawns.Add(currentSpawnData);
                    }
                }
                
            }
            return currentSpecialSpawns;
        }


        public static DungeonSpawnData GetDungeonEncounterData(MobSpawn currentMob, TeamMemberSpawn currentSpawn = null, int minFloor = 0, int maxFloor = 0, string[] addedFeatures = null, bool isBasementFloor = false)
        {
            DungeonSpawnData currentSpawnData = new DungeonSpawnData();

            // Form name step
            MonsterData currentEnemyData = DataManager.Instance.GetMonster(currentMob.BaseForm.Species);
            currentSpawnData.spawnName = currentEnemyData.Forms[currentMob.BaseForm.Form].FormName.ToLocal();

            // Level range step
            RandRange levelRange = currentMob.Level;
            currentSpawnData.minLevel = levelRange.Min;
            currentSpawnData.maxLevel = levelRange.Max;

            // Floor range step
            IntRange floorRange = new IntRange(minFloor, maxFloor);
            currentSpawnData.startFloor = floorRange.Min + 1;
            currentSpawnData.endFloor = floorRange.Max;
            currentSpawnData.isBasement = isBasementFloor;

            // Intrinsic step
            if (currentMob.Intrinsic != "")
            {
                IntrinsicData currentIntrinsicData = DataManager.Instance.GetIntrinsic(currentMob.Intrinsic);
                currentSpawnData.spawnIntrinsic = currentIntrinsicData.Name.ToLocal();
            }
            else
            {
                currentSpawnData.spawnIntrinsic = "";
            }

            // Specified skills list
            List<string> specifiedSkillsList = currentMob.SpecifiedSkills;
            for (int specifiedSkillIndex = 0; specifiedSkillIndex < specifiedSkillsList.Count; specifiedSkillIndex++)
            {
                string skill = specifiedSkillsList[specifiedSkillIndex];
                SkillData currentSkillData = DataManager.Instance.GetSkill(skill);
                currentSpawnData.specifiedSkillsList.Add("[[" + currentSkillData.Name.ToLocal() + "]]");
            }

            // Other qualities list
            if (addedFeatures != null)
            {
                for (int i = 0; i < addedFeatures.Length; i++)
                {
                    currentSpawnData.extraFeatures.Add(addedFeatures[i]);
                }
            }

            if (currentMob.Tactic == "wait_attack" || currentMob.Tactic == "turret")
            {
                currentSpawnData.extraFeatures.Add("Doesn't move");
            }
            for (int spawnFeatureIndex = 0; spawnFeatureIndex < currentMob.SpawnFeatures.Count; spawnFeatureIndex++)
            {
                MobSpawnExtra spawnFeature = currentMob.SpawnFeatures[spawnFeatureIndex];
                if (spawnFeature is MobSpawnItem)
                {
                    MobSpawnItem castFeature = (MobSpawnItem)spawnFeature;
                    ItemData heldItem = DataManager.Instance.GetItem(castFeature.Items.GetSpawn(0).ID);
                    currentSpawnData.extraFeatures.Add("Held: [[" + heldItem.Name.ToLocal() + "]]");
                }
                if (spawnFeature is MobSpawnStatus)
                {
                    MobSpawnStatus castFeature = (MobSpawnStatus)spawnFeature;
                    SpawnList<StatusEffect> statusList = castFeature.Statuses;
                    for(int statusIndex = 0; statusIndex < statusList.Count; statusIndex++)
                    {
                        StatusEffect currentStatus = statusList.GetSpawn(statusIndex);
                        if (currentStatus.ID == "sleep")
                        {
                            currentSpawnData.extraFeatures.Add("Spawns asleep");
                        }
                        if (currentStatus.ID == "freeze")
                        {
                            currentSpawnData.extraFeatures.Add("Spawns frozen");
                        }
                    }
                }
                if (spawnFeature is MobSpawnLevelScale)
                {
                    MobSpawnLevelScale castFeature = (MobSpawnLevelScale)spawnFeature;
                    string levelScaleString = "";
                    if (castFeature.StartFromID == 0)
                    {
                        levelScaleString = String.Format("Gains {0}/{1} levels every floor", castFeature.AddNumerator, castFeature.AddDenominator);
                    }
                    else
                    {
                        levelScaleString = String.Format("Starting at floor {0}, gains {1}/{2} levels every floor", castFeature.StartFromID + 1, castFeature.AddNumerator, castFeature.AddDenominator);
                    }
                    currentSpawnData.extraFeatures.Add(levelScaleString);
                    currentSpawnData.minLevel = castFeature.MinLevel;
                    currentSpawnData.maxLevel = castFeature.MinLevel;
                    currentSpawnData.levelCanBeIncreased = true;
                }
                /*
                if (spawnFeature is MobSpawnWeak)
                {
                    notes += "Half PP and 35% belly<br>";
                    noteCount++;
                }
                */
            }
            if (currentSpawn != null)
            {
                TeamMemberSpawn.MemberRole memberRole = currentSpawn.Role;
                if (memberRole == TeamMemberSpawn.MemberRole.Support)
                {
                    currentSpawnData.extraFeatures.Add("Spawns as team support");
                }
                if (memberRole == TeamMemberSpawn.MemberRole.Leader)
                {
                    currentSpawnData.extraFeatures.Add("Spawns as team leader");
                }
                if (memberRole == TeamMemberSpawn.MemberRole.Loner)
                {
                    currentSpawnData.extraFeatures.Add("Spawns alone");
                }
            }

            return currentSpawnData;
        }

    }
}
