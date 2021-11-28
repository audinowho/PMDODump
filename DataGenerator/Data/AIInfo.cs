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
            tactic.ID = 0;
            AIFlags iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.TeamPartner | AIFlags.PlayerSense;
            tactic.Plans.Add(new PreparePlan(iq | AIFlags.TrapAvoider, AIPlan.AttackChoice.SmartAttack));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new AttackFoesPlan(iq | AIFlags.TrapAvoider, AIPlan.AttackChoice.SmartAttack, AIPlan.PositionChoice.Avoid));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Go After Foes");//1
            tactic.ID = 1;
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.TeamPartner | AIFlags.PlayerSense;
            tactic.Plans.Add(new AttackFoesPlan(iq | AIFlags.TrapAvoider, AIPlan.AttackChoice.SmartAttack, AIPlan.PositionChoice.Avoid));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Split Up");//2
            tactic.ID = 2;
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.TeamPartner | AIFlags.PlayerSense;
            tactic.Plans.Add(new AttackFoesPlan(iq | AIFlags.TrapAvoider, AIPlan.AttackChoice.SmartAttack, AIPlan.PositionChoice.Avoid));
            tactic.Plans.Add(new AvoidAlliesPlan(iq | AIFlags.TrapAvoider));//if cornered, don't do anything
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Avoid Trouble");//3
            tactic.ID = 3;
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.TeamPartner | AIFlags.PlayerSense;
            tactic.Plans.Add(new AvoidFoesPlan(iq | AIFlags.TrapAvoider));//if cornered, don't do anything
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Get Away");//4
            tactic.ID = 4;
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.TeamPartner | AIFlags.PlayerSense;
            tactic.Plans.Add(new AvoidAllPlan(iq | AIFlags.TrapAvoider));//if cornered, don't do anything
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Wait There");//5
            tactic.ID = 5;
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.TeamPartner | AIFlags.PlayerSense;
            tactic.Plans.Add(new PreparePlan(iq | AIFlags.TrapAvoider, AIPlan.AttackChoice.SmartAttack));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Wait Only");//6
            tactic.ID = 6;
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
            tactic.ID = 7;
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.AttackToEscape | AIFlags.WontDisturb;
            tactic.Plans.Add(new AttackFoesPlan(iq, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Wait Attack");//8
            tactic.ID = 8;
            iq = AIFlags.None;
            tactic.Plans.Add(new PreparePlan(iq | AIFlags.TrapAvoider, AIPlan.AttackChoice.RandomAttack));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Patrol");//9
            tactic.ID = 9;
            iq = AIFlags.None;
            tactic.Plans.Add(new AttackFoesPlan(iq, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close));
            tactic.Plans.Add(new StayInRangePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Weird Tree");//10
            tactic.ID = 10;
            iq = AIFlags.None;
            tactic.Plans.Add(new ExploreIfUnseenPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitUntilAttackedPlan(iq | AIFlags.TrapAvoider, 25));
            tactic.Plans.Add(new AttackFoesPlan(iq, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Thief");//11
            tactic.ID = 11;
            iq = AIFlags.AttackToEscape;
            tactic.Plans.Add(new ThiefPlan(iq));
            tactic.Plans.Add(new AttackFoesPlan(iq, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Cannibal");//12
            tactic.ID = 12;
            iq = AIFlags.Cannibal | AIFlags.AttackToEscape;
            tactic.Plans.Add(new AttackFoesPlan(iq, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Retreater");//13
            tactic.ID = 13;
            iq = AIFlags.AttackToEscape;
            tactic.Plans.Add(new RetreaterPlan(iq | AIFlags.TrapAvoider, 3));
            tactic.Plans.Add(new AttackFoesPlan(iq, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Staged Boss");//14
            tactic.ID = 14;
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.AttackToEscape | AIFlags.WontDisturb | AIFlags.TeamPartner;
            tactic.Plans.Add(new BossPlan(iq | AIFlags.TrapAvoider, AIPlan.AttackChoice.SmartAttack, AIPlan.PositionChoice.Avoid));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Item Finder");//15
            tactic.ID = 15;
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.AttackToEscape;
            tactic.Plans.Add(new AttackFoesPlan(iq, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Dumb Wander");//16
            tactic.ID = 16;
            iq = AIFlags.ItemGrabber | AIFlags.AttackToEscape;
            tactic.Plans.Add(new AttackFoesPlan(iq, AIPlan.AttackChoice.DumbAttack, AIPlan.PositionChoice.Close));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            //will pick up items as well as seek them out
            //uses moves all the time
            //
            tactic = new AITactic();
            tactic.Name = new LocalText("Smart Wander");//17
            tactic.ID = 17;
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.AttackToEscape | AIFlags.WontDisturb;
            tactic.Plans.Add(new PreBuffPlan(iq, 26));
            tactic.Plans.Add(new AttackFoesPlan(iq, AIPlan.AttackChoice.SmartAttack, AIPlan.PositionChoice.Avoid));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Tit for Tat");//18
            tactic.ID = 18;
            iq = AIFlags.AttackToEscape | AIFlags.WontDisturb;
            tactic.Plans.Add(new WaitUntilAttackedPlan(iq | AIFlags.TrapAvoider, 31));
            tactic.Plans.Add(new AttackFoesPlan(iq, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            //uses moves all the time
            //does not pick up or use items
            tactic = new AITactic();
            tactic.Name = new LocalText("Loot Guard");//19
            tactic.ID = 19;
            iq = AIFlags.AttackToEscape | AIFlags.WontDisturb;
            tactic.Plans.Add(new AttackFoesPlan(iq, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Boss");//20
            tactic.ID = 20;
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.AttackToEscape | AIFlags.WontDisturb | AIFlags.TeamPartner;
            tactic.Plans.Add(new AttackFoesPlan(iq | AIFlags.TrapAvoider, AIPlan.AttackChoice.SmartAttack, AIPlan.PositionChoice.Avoid));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Slow Wander");//21
            tactic.ID = 21;
            iq = AIFlags.None;
            tactic.Plans.Add(new WaitPeriodPlan(iq | AIFlags.TrapAvoider, 2));
            tactic.Plans.Add(new FollowLeaderPlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Slow Patrol");//22
            tactic.ID = 22;
            iq = AIFlags.None;
            tactic.Plans.Add(new WaitPeriodPlan(iq | AIFlags.TrapAvoider, 2));
            tactic.Plans.Add(new StayInRangePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Shopkeeper");//23
            tactic.ID = 23;
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.TeamPartner;
            tactic.Plans.Add(new WaitUntilMapStatusPlan(iq | AIFlags.TrapAvoider, 31));
            tactic.Plans.Add(new PreBuffPlan(iq | AIFlags.TrapAvoider, 26));
            tactic.Plans.Add(new AttackFoesPlan(iq | AIFlags.TrapAvoider, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Close));
            tactic.Plans.Add(new AvoidAlliesPlan(iq | AIFlags.TrapAvoider));//if cornered, don't do anything
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Shuckle");//24
            tactic.ID = 24;
            iq = AIFlags.ItemGrabber | AIFlags.ItemMaster | AIFlags.KnowsMatchups | AIFlags.TeamPartner;
            tactic.Plans.Add(new WaitUntilMapStatusPlan(iq | AIFlags.TrapAvoider, 31));
            tactic.Plans.Add(new LeadSkillPlan(iq | AIFlags.TrapAvoider, 26));
            tactic.Plans.Add(new AttackFoesPlan(iq | AIFlags.TrapAvoider, AIPlan.AttackChoice.RandomAttack, AIPlan.PositionChoice.Avoid));
            tactic.Plans.Add(new AvoidAlliesPlan(iq | AIFlags.TrapAvoider));//if cornered, don't do anything
            tactic.Plans.Add(new ExplorePlan(iq | AIFlags.TrapAvoider));
            tactic.Plans.Add(new WaitPlan(iq | AIFlags.TrapAvoider));
            Tactics.Add(tactic);

            tactic = new AITactic();
            tactic.Name = new LocalText("Turret");//25
            tactic.ID = 25;
            iq = AIFlags.None;
            tactic.Plans.Add(new PreparePlan(iq | AIFlags.TrapAvoider, AIPlan.AttackChoice.RandomAttack));
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
                DataManager.SaveData(ii, DataManager.DataType.AI.ToString(), Tactics[ii]);
            }
        }

    }
}

