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
    public static class AIInfo
    {
        public static void AddAIData()
        {
            DataInfo.DeleteIndexedData(DataManager.DataType.AI.ToString());


            List<AITactic> Tactics = new List<AITactic>();
            AITactic tactic = new AITactic();
            tactic.Name = new LocalText("Stick Together");//0
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            AIFlags iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.TeamPartner | AIFlags.PlayerSense;
            tactic.Plans.Add(new PrepareWithLeaderPlan(iq | AIFlags.TrapAvoider, 0, 0, 0, AIPlan.AttackChoice.SmartAttack, 0, false));
            tactic.Plans.Add(new AttackFoesPlan(iq | AIFlags.TrapAvoider, 0, 0, 0, AIPlan.AttackChoice.SmartAttack, AIPlan.PositionChoice.Avoid, 0, false));
            tactic.Plans.Add(new FindItemPlan(iq | AIFlags.TrapAvoider, true));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Go After Foes");//1
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.TeamPartner | AIFlags.PlayerSense;
            tactic.Plans.Add(new AttackFoesPlan(iq | AIFlags.TrapAvoider, 0, 0, 0, AIPlan.AttackChoice.SmartAttack, AIPlan.PositionChoice.Avoid, 0, false));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new FindItemPlan(iq | AIFlags.TrapAvoider, true));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Split Up");//2
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.TeamPartner | AIFlags.PlayerSense;
            tactic.Plans.Add(new AttackFoesPlan(iq | AIFlags.TrapAvoider, 0, 0, 0, AIPlan.AttackChoice.SmartAttack, AIPlan.PositionChoice.Avoid, 0, false));
            //tactic.Plans.Add(new AvoidAlliesPlan(iq | AIFlags.TrapAvoider));//if cornered, don't do anything
            tactic.Plans.Add(new FindItemPlan(iq | AIFlags.TrapAvoider, true));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Avoid Trouble");//3
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.TeamPartner | AIFlags.PlayerSense;
            tactic.Plans.Add(new AvoidFoesPlan(iq | AIFlags.TrapAvoider));//if cornered, don't do anything
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new FindItemPlan(iq | AIFlags.TrapAvoider, true));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Get Away");//4
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.TeamPartner | AIFlags.PlayerSense;
            tactic.Plans.Add(new AvoidAllPlan(iq | AIFlags.TrapAvoider));//if cornered, don't do anything
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Wait There");//5
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.TeamPartner | AIFlags.PlayerSense;
            tactic.Plans.Add(new PreparePlan(iq | AIFlags.TrapAvoider, 0, 0, 0, AIPlan.AttackChoice.SmartAttack, 0, false));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Wait Only");//6
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.None | AIFlags.KnowsMatchups | AIFlags.PlayerSense;
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            // We want the following for enemy AI:
            // Semismart enemies (7) will never approach when they can attack, picking up items if they walk over them but not targeting them, know how to use items
            // Boss enemies (20) will never approach when they can attack
            // Smart enemies (17) will never approach when they can attack, and attempt to line up with the farthest range, know how to use items, and walking towards items specifically
            // Dumb enemies (16) will approach and attack, picking up items if they walk over them

            // All enemies will pick up items by default; things will always be more interesting this way
            // a dungeon must have only one of the three; never mix.
            // level 5 dungeons will have the smart AI (17); the hardest AI that isn't reserved for event mons; roguelocke mode is meant to be the utmost challenge after all!
            // secret mons will always have the smartest AI
            // is there a difference between lategame and dumb enemies?  if not, it makes this much easier... (refer to TODO 3)

            // TODO 1: make AI foes path to items when they see them
            // TODO 2: make AI foes know to throw items when they have them
            // TODO 3: fix the issues with dumb AI and reevaluate the AI's stepping-towards-player system

            tactic = new AITactic();
            tactic.Name = new LocalText("Normal Wander");//7
            tactic.ID = "wander_normal";
            iq = AIFlags.ItemGrabber | AIFlags.AttackToEscape | AIFlags.WontDisturb;
            tactic.Plans.Add(new AttackFoesPlan(iq, 0, 0, 4, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close, 0, false));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Wait Attack");//8
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.None;
            tactic.Plans.Add(new PreparePlan(iq | AIFlags.TrapAvoider, 0, 0, 4, AIPlan.AttackChoice.RandomAttack, 0, false));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Patrol");//9
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.None;
            tactic.Plans.Add(new AttackFoesPlan(iq, 0, 0, 4, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close, 0, false));
            tactic.Plans.Add(new StayInRangePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Weird Tree");//10
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.None;
            tactic.Plans.Add(new ExploreIfSeenPlan(true, iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitUntilAttackedPlan(iq | AIFlags.TrapAvoider, "last_targeted_by"));
            tactic.Plans.Add(new AttackFoesPlan(iq, 0, 0, 4, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close, 0, false));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Thief");//11
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.AttackToEscape;
            tactic.Plans.Add(new ThiefPlan(iq));
            tactic.Plans.Add(new AttackFoesPlan(iq, 0, 0, 4, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close, 0, false));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Itemless Dumb Wander");//12
            tactic.ID = "wander_dumb_itemless";
            iq = AIFlags.AttackToEscape;
            tactic.Plans.Add(new AttackFoesPlan(iq, 4, 4, 3, AIPlan.AttackChoice.DumbAttack, AIPlan.PositionChoice.Approach, 0, false));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Retreater");//13
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.ItemGrabber | AIFlags.WontDisturb;
            tactic.Plans.Add(new RetreaterPlan(AIFlags.AttackToEscape | AIFlags.TrapAvoider, 3));
            tactic.Plans.Add(new AttackFoesPlan(iq, 0, 0, 4, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close, 0, false));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Staged Boss");//14
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.AttackToEscape | AIFlags.WontDisturb | AIFlags.TeamPartner;
            tactic.Plans.Add(new BossPlan(iq | AIFlags.TrapAvoider, 0, 0, 0, AIPlan.AttackChoice.SmartAttack, AIPlan.PositionChoice.Avoid, 0, false));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Item Finder");//15
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.AttackToEscape;
            tactic.Plans.Add(new AttackFoesPlan(iq, 0, 0, 4, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close, 0, false));
            tactic.Plans.Add(new FindItemPlan(iq | AIFlags.TrapAvoider, false));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Dumb Wander");//16
            tactic.ID = "wander_dumb";
            iq = AIFlags.ItemGrabber | AIFlags.AttackToEscape;
            tactic.Plans.Add(new AttackFoesPlan(iq, 4, 4, 3, AIPlan.AttackChoice.DumbAttack, AIPlan.PositionChoice.Approach, 0, false));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            //will pick up items as well as seek them out
            //uses moves all the time
            //
            tactic = new AITactic();
            tactic.Name = new LocalText("Smart Wander");//17
            tactic.ID = "wander_smart";
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.AttackToEscape | AIFlags.WontDisturb;
            tactic.Plans.Add(new PreBuffPlan(iq, 0, 0, 0, "last_used_move_slot", 0, false));
            tactic.Plans.Add(new AttackFoesPlan(iq, 0, 0, 4, AIPlan.AttackChoice.SmartAttack, AIPlan.PositionChoice.Avoid, 0, false));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Tit for Tat");//18
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.AttackToEscape | AIFlags.WontDisturb;
            tactic.Plans.Add(new WaitUntilAttackedPlan(iq | AIFlags.TrapAvoider, "was_hurt_last_turn"));
            tactic.Plans.Add(new AttackFoesPlan(iq, 0, 0, 4, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close, 0, false));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            //uses moves all the time
            //does not pick up or use items
            tactic = new AITactic();
            tactic.Name = new LocalText("Loot Guard");//19
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.AttackToEscape | AIFlags.WontDisturb;
            tactic.Plans.Add(new AttackFoesPlan(iq, 0, 0, 4, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close, 0, false));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Boss");//20
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.AttackToEscape | AIFlags.WontDisturb | AIFlags.TeamPartner;
            tactic.Plans.Add(new AttackFoesPlan(iq | AIFlags.TrapAvoider, 0, 0, 4, AIPlan.AttackChoice.SmartAttack, AIPlan.PositionChoice.Avoid, 0, false));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Slow Wander");//21
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.None;
            tactic.Plans.Add(new WaitPeriodPlan(iq | AIFlags.TrapAvoider, 2));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Slow Patrol");//22
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.None;
            tactic.Plans.Add(new WaitPeriodPlan(iq | AIFlags.TrapAvoider, 2));
            tactic.Plans.Add(new StayInRangePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Shopkeeper");//23
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.TeamPartner;
            tactic.Plans.Add(new WaitUntilMapStatusPlan(iq | AIFlags.TrapAvoider, "thief"));
            tactic.Plans.Add(new PreBuffPlan(iq | AIFlags.TrapAvoider, 0, 0, 0, "last_used_move_slot", 0, false));
            tactic.Plans.Add(new AttackFoesPlan(iq | AIFlags.TrapAvoider, 0, 0, 4, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close, 0, false));
            tactic.Plans.Add(new AvoidAlliesPlan(iq | AIFlags.TrapAvoider));//if cornered, don't do anything
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Shuckle");//24
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.TeamPartner;
            tactic.Plans.Add(new WaitUntilMapStatusPlan(iq | AIFlags.TrapAvoider, "thief"));
            tactic.Plans.Add(new LeadSkillPlan(iq | AIFlags.TrapAvoider, 0, 0, 0, "last_used_move_slot", 0, false));
            tactic.Plans.Add(new AttackFoesPlan(iq | AIFlags.TrapAvoider, 0, 0, 4, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Avoid, 0, false));
            tactic.Plans.Add(new AvoidAlliesPlan(iq | AIFlags.TrapAvoider));//if cornered, don't do anything
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Turret");//25
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.None;
            tactic.Plans.Add(new PreparePlan(iq | AIFlags.TrapAvoider, 0, 0, 4, AIPlan.AttackChoice.RandomAttack, 0, false));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            //attacks on sight
            //moves around if in FOV
            //does not move at all outside of FOV
            tactic = new AITactic();
            tactic.Name = new LocalText("Wait and See");//26
            tactic.ID = "wait_and_see";
            iq = AIFlags.AttackToEscape;
            tactic.Plans.Add(new AttackFoesPlan(iq, 4, 4, 3, AIPlan.AttackChoice.DumbAttack, AIPlan.PositionChoice.Approach, 0, false));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExploreIfSeenPlan(false, iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            //dances around the moon stone until the moon stone is gone or ANYONE in the team is attacked
            //then just becomes normal wander
            tactic = new AITactic();
            tactic.Name = new LocalText("Moon Dance");//27
            tactic.ID = "moon_dance";
            iq = AIFlags.ItemGrabber | AIFlags.AttackToEscape | AIFlags.WontDisturb;
            tactic.Plans.Add(new CultDancePlan(iq | AIFlags.TrapAvoider, "evo_moon_stone", "last_targeted_by"));
            tactic.Plans.Add(new AttackFoesPlan(iq, 0, 0, 4, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close, 0, false));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Lead Boss");//28
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.AttackToEscape | AIFlags.WontDisturb | AIFlags.TeamPartner;
            tactic.Plans.Add(new LeadSkillPlan(iq | AIFlags.TrapAvoider, 0, 0, 0, "last_used_move_slot", 0, false));
            tactic.Plans.Add(new BossPlan(iq | AIFlags.TrapAvoider, 0, 0, 0, AIPlan.AttackChoice.SmartAttack, AIPlan.PositionChoice.Avoid, 0, false));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            //ai for mission target
            //will not wander into secondary terrain or walls
            tactic = new AITactic();
            tactic.Name = new LocalText("Mission Target");//29
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.None;
            tactic.Plans.Add(new WaitPeriodPlan(iq | AIFlags.TrapAvoider, 2, 0, 0, 0, (TerrainData.Mobility) 14, false));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider, 0, 0, 0, (TerrainData.Mobility) 14, false));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider, 0, 0, 0, (TerrainData.Mobility) 14, false));
            Tactics.Add(tactic);

            //ai for fleeing
            //the AI will try to flee for the stairs, while not wandering into secondary terrain or walls
            tactic = new AITactic();
            tactic.Name = new LocalText("Super Flee Stairs");//30
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.None;
            tactic.Plans.Add(new FleeStairsPlan(iq | AIFlags.TrapAvoider, new HashSet<string>() { "stairs_back_down", "stairs_back_up", "stairs_exit_down", "stairs_exit_up", "stairs_go_up", "stairs_go_down" }));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider, 0, 0, 4, (TerrainData.Mobility) 14, false));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Itemless Normal Wander");//31
            tactic.ID = "wander_normal_itemless";
            iq = AIFlags.AttackToEscape | AIFlags.WontDisturb;
            tactic.Plans.Add(new AttackFoesPlan(iq, 0, 0, 4, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close, 0, false));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Itemless Retreater");//32
            tactic.ID = "retreater_itemless";
            iq = AIFlags.WontDisturb;
            tactic.Plans.Add(new RetreaterPlan(AIFlags.AttackToEscape | AIFlags.TrapAvoider, 3));
            tactic.Plans.Add(new AttackFoesPlan(iq, 0, 0, 4, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close, 0, false));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Fleeing Legendary");//33
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.WontDisturb;
            tactic.Plans.Add(new FleeStairsPlan(iq | AIFlags.TrapAvoider, new HashSet<string>() { "stairs_back_down", "stairs_back_up", "stairs_exit_down", "stairs_exit_up", "stairs_go_up", "stairs_go_down" }, true, 2));
            tactic.Plans.Add(new AttackFoesPlan(iq, 0, 0, 4, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close, 0, false));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);


            tactic = new AITactic();
            tactic.Name = new LocalText("Lurker");//34
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.AttackToEscape | AIFlags.WontDisturb;
            tactic.Plans.Add(new AttackFoesPlan(iq, 0, 0, 4, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close, 0, false));
            tactic.Plans.Add(new StalkerPlan(iq | AIFlags.TrapAvoider, "last_targeted_by", 10));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Fleeing");//35
            tactic.ID = Text.Sanitize(tactic.Name.DefaultText).ToLower();
            iq = AIFlags.WontDisturb;
            tactic.Plans.Add(new AvoidFoesCornerPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new AttackFoesPlan(iq, 0, 0, 4, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close, 0, false));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            //make a variant of explore to go for items (For use of team characters, certain thief enemies)
            //always avoid traps when exploring
            //always avoid traps when following a leader
            //team members only: avoid traps when avoiding foes
            //team members only: avoid traps when going for foes
            //team members are cognizant of stepping on traps, but not of attacking them?
            //need a tactic that runs away from enemies when holding an item, goes for stealing moves when not


            for (int ii = 0; ii < Tactics.Count; ii++)
            {
                if (ii < 6)
                    Tactics[ii].Assignable = true;
                Tactics[ii].Released = true;
                DataManager.SaveEntryData(Tactics[ii].ID, DataManager.DataType.AI.ToString(), Tactics[ii]);
            }
        }

    }
}

