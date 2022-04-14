using RogueEssence.Content;
using RogueElements;
using RogueEssence.LevelGen;
using RogueEssence.Dungeon;
using RogueEssence.Ground;
using RogueEssence.Script;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using RogueEssence;
using RogueEssence.Data;
using PMDC.Dungeon;
using PMDC.LevelGen;
using PMDC;
using PMDC.Data;

namespace DataGenerator.Data
{
    public partial class ZoneInfo
    {

        #region GUILDMASTER TRAIL
        static void FillGuildmaster(ZoneData zone)
        {
            zone.Name = new LocalText("Guildmaster Trail");
            zone.Level = 5;
            zone.LevelCap = true;
            zone.BagRestrict = 4;
            zone.Rescues = 2;
            zone.Rogue = RogueStatus.AllTransfer;

            int max_floors = 30;

            LayeredSegment floorSegment = new LayeredSegment();
            floorSegment.IsRelevant = true;
            floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
            floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Guildmaster Trail\n{0}F")));

            //money
            MoneySpawnZoneStep moneySpawnZoneStep = new MoneySpawnZoneStep(PR_RESPAWN_MONEY, new RandRange(63, 72), new RandRange(21, 24));
            moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
            floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

            //items
            ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
            itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;

            //necesities
            CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
            necessities.SpawnRates.SetRange(14, new IntRange(0, 30));
            itemSpawnZoneStep.Spawns.Add("necessities", necessities);

            necessities.Spawns.Add(new InvItem(11), new IntRange(0, 30), 30);//Leppa
            necessities.Spawns.Add(new InvItem(10), new IntRange(0, 10), 70);//Oran
            necessities.Spawns.Add(new InvItem(10), new IntRange(10, 30), 40);//Oran
            necessities.Spawns.Add(new InvItem(72), new IntRange(10, 30), 30);//Sitrus
            necessities.Spawns.Add(new InvItem(1), new IntRange(0, 15), 40);//Apple
            necessities.Spawns.Add(new InvItem(454), new IntRange(5, 30), 30);//Grimy Food
            necessities.Spawns.Add(new InvItem(12), new IntRange(2, 30), 50);//Lum berry

            necessities.Spawns.Add(new InvItem(101), new IntRange(0, 30), 30);//reviver seed
            necessities.Spawns.Add(new InvItem(101, true), new IntRange(0, 30), 15);//reviver seed
            necessities.Spawns.Add(new InvItem(450), new IntRange(4, 30), 30);//Link Box


            //snacks
            CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
            snacks.SpawnRates.SetRange(10, new IntRange(0, 30));
            itemSpawnZoneStep.Spawns.Add("snacks", snacks);

            for (int nn = 0; nn < 7; nn++)//Pinch Berries
                snacks.Spawns.Add(new InvItem(43 + nn), new IntRange(0, 30), 3);
            snacks.Spawns.Add(new InvItem(51), new IntRange(0, 30), 4);//enigma berry

            snacks.Spawns.Add(new InvItem(37), new IntRange(0, 30), 5);//Jaboca
            snacks.Spawns.Add(new InvItem(38), new IntRange(0, 30), 5);//Rowap

            for (int nn = 0; nn < 18; nn++)//Type berry
                snacks.Spawns.Add(new InvItem(19 + nn), new IntRange(6, 30), 1);

            snacks.Spawns.Add(new InvItem(112), new IntRange(0, 30), 20);//blast seed
            snacks.Spawns.Add(new InvItem(108), new IntRange(0, 30), 10);//warp seed
            snacks.Spawns.Add(new InvItem(116), new IntRange(0, 30), 10);//decoy seed
            snacks.Spawns.Add(new InvItem(110), new IntRange(0, 30), 10);//sleep seed
            snacks.Spawns.Add(new InvItem(113), new IntRange(0, 30), 10);//blinker seed
            snacks.Spawns.Add(new InvItem(117), new IntRange(0, 30), 5);//last-chance seed
            snacks.Spawns.Add(new InvItem(104), new IntRange(0, 30), 5);//doom seed
            snacks.Spawns.Add(new InvItem(118), new IntRange(0, 30), 10);//ban seed
            snacks.Spawns.Add(new InvItem(114), new IntRange(0, 30), 4);//pure seed
            snacks.Spawns.Add(new InvItem(114, true), new IntRange(0, 30), 4);//pure seed
            snacks.Spawns.Add(new InvItem(115), new IntRange(0, 30), 10);//ice seed
            snacks.Spawns.Add(new InvItem(111), new IntRange(0, 30), 10);//vile seed

            snacks.Spawns.Add(new InvItem(184), new IntRange(0, 30), 5);//power herb
            snacks.Spawns.Add(new InvItem(184, true), new IntRange(0, 30), 5);//power herb
            snacks.Spawns.Add(new InvItem(183), new IntRange(0, 30), 5);//mental herb
            snacks.Spawns.Add(new InvItem(185), new IntRange(0, 30), 50);//white herb


            //boosters
            CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
            boosters.SpawnRates.SetRange(7, new IntRange(0, 30));
            itemSpawnZoneStep.Spawns.Add("boosters", boosters);

            for (int nn = 0; nn < 18; nn++)//Gummi
                boosters.Spawns.Add(new InvItem(76 + nn), new IntRange(0, 30), 1);

            IntRange range = new IntRange(3, 30);

            boosters.Spawns.Add(new InvItem(151), range, 2);//protein
            boosters.Spawns.Add(new InvItem(152), range, 2);//iron
            boosters.Spawns.Add(new InvItem(153), range, 2);//calcium
            boosters.Spawns.Add(new InvItem(154), range, 2);//zinc
            boosters.Spawns.Add(new InvItem(155), range, 2);//carbos
            boosters.Spawns.Add(new InvItem(156), range, 2);//hp up

            //throwable
            CategorySpawn<InvItem> ammo = new CategorySpawn<InvItem>();
            ammo.SpawnRates.SetRange(12, new IntRange(0, 30));
            itemSpawnZoneStep.Spawns.Add("ammo", ammo);

            range = new IntRange(0, 30);
            {
                ammo.Spawns.Add(new InvItem(200, false, 4), range, 10);//stick
                ammo.Spawns.Add(new InvItem(201, false, 3), range, 10);//cacnea spike
                ammo.Spawns.Add(new InvItem(220, false, 2), range, 10);//path wand
                ammo.Spawns.Add(new InvItem(228, false, 4), range, 10);//fear wand
                ammo.Spawns.Add(new InvItem(223, false, 4), range, 10);//switcher wand
                ammo.Spawns.Add(new InvItem(222, false, 4), range, 10);//whirlwind wand
                ammo.Spawns.Add(new InvItem(225, false, 4), range, 10);//lure wand
                ammo.Spawns.Add(new InvItem(226, false, 4), range, 10);//slow wand
                ammo.Spawns.Add(new InvItem(221, false, 4), range, 10);//pounce wand
                ammo.Spawns.Add(new InvItem(232, false, 2), range, 10);//warp wand
                ammo.Spawns.Add(new InvItem(231, false, 4), range, 10);//topsy-turvy wand
                ammo.Spawns.Add(new InvItem(234, false, 4), range, 10);//lob wand
                ammo.Spawns.Add(new InvItem(233, false, 4), range, 10);//purge wand

                ammo.Spawns.Add(new InvItem(203, false, 3), range, 10);//iron thorn
                ammo.Spawns.Add(new InvItem(204, false, 3), range, 10);//silver spike
                ammo.Spawns.Add(new InvItem(208, false, 3), range, 10);//Gravelerock

                ammo.Spawns.Add(new InvItem(202, false, 3), range, 10);//corsola spike
                ammo.Spawns.Add(new InvItem(206, false, 3), range, 10);//Rare fossil

                ammo.Spawns.Add(new InvItem(207, false, 3), range, 10);//Geo Pebble
            }


            //special items
            CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
            special.SpawnRates.SetRange(7, new IntRange(0, 30));
            itemSpawnZoneStep.Spawns.Add("special", special);

            {
                range = new IntRange(0, 15);
                int rate = 2;

                special.Spawns.Add(new InvItem(211), range, rate);//blue apricorns
                special.Spawns.Add(new InvItem(212), range, rate);//green apricorns
                special.Spawns.Add(new InvItem(213), range, rate);//brown apricorns
                special.Spawns.Add(new InvItem(214), range, rate);//purple apricorns
                special.Spawns.Add(new InvItem(215), range, rate);//red apricorns
                special.Spawns.Add(new InvItem(216), range, rate);//white apricorns
                special.Spawns.Add(new InvItem(217), range, rate);//yellow apricorns
                special.Spawns.Add(new InvItem(218), range, rate);//black apricorns

                range = new IntRange(15, 25);
                rate = 1;

                special.Spawns.Add(new InvItem(210, true), range, rate);//Plain Apricorn
                special.Spawns.Add(new InvItem(211, true), range, rate);//blue apricorns
                special.Spawns.Add(new InvItem(212, true), range, rate);//green apricorns
                special.Spawns.Add(new InvItem(213, true), range, rate);//brown apricorns
                special.Spawns.Add(new InvItem(214, true), range, rate);//purple apricorns
                special.Spawns.Add(new InvItem(215, true), range, rate);//red apricorns
                special.Spawns.Add(new InvItem(216, true), range, rate);//white apricorns
                special.Spawns.Add(new InvItem(217, true), range, rate);//yellow apricorns
                special.Spawns.Add(new InvItem(218, true), range, rate);//black apricorns

            }

            special.Spawns.Add(new InvItem(455, false, 1), new IntRange(0, 25), 40);//Key
            special.Spawns.Add(new InvItem(451), new IntRange(9, 30), 35);//Assembly Box
                                                                          //special.Spawns.Add(new InvItem(452), 10);//Storage Box
            special.Spawns.Add(new InvItem(453), new IntRange(13, 30), 15); // Ability Capsule



            //orbs
            CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
            orbs.SpawnRates.SetRange(10, new IntRange(0, 30));
            itemSpawnZoneStep.Spawns.Add("orbs", orbs);

            {
                range = new IntRange(3, 30);
                orbs.Spawns.Add(new InvItem(281), range, 7);//One-Room Orb
                orbs.Spawns.Add(new InvItem(256), range, 7);//Fill-In Orb
                orbs.Spawns.Add(new InvItem(281, true), range, 3);//One-Room Orb
                orbs.Spawns.Add(new InvItem(256, true), range, 3);//Fill-In Orb
            }

            {
                range = new IntRange(0, 25);
                orbs.Spawns.Add(new InvItem(273), range, 10);//Petrify
                orbs.Spawns.Add(new InvItem(287), range, 10);//Halving
                orbs.Spawns.Add(new InvItem(271), range, 8);//Slumber Orb
                orbs.Spawns.Add(new InvItem(282), range, 8);//Slow
                orbs.Spawns.Add(new InvItem(272), range, 8);//Totter
                orbs.Spawns.Add(new InvItem(275), range, 5);//Spurn
                orbs.Spawns.Add(new InvItem(267), range, 3);//Stayaway
                orbs.Spawns.Add(new InvItem(266), range, 8);//Pierce
                orbs.Spawns.Add(new InvItem(271, true), range, 3);//Slumber Orb
                orbs.Spawns.Add(new InvItem(282, true), range, 3);//Slow
                orbs.Spawns.Add(new InvItem(272, true), range, 3);//Totter
                orbs.Spawns.Add(new InvItem(275, true), range, 2);//Spurn
                orbs.Spawns.Add(new InvItem(267, true), range, 3);//Stayaway
                orbs.Spawns.Add(new InvItem(266, true), range, 3);//Pierce
            }

            orbs.Spawns.Add(new InvItem(263), new IntRange(2, 25), 7);//Cleanse

            {
                range = new IntRange(6, 25);
                orbs.Spawns.Add(new InvItem(258), range, 10);//All-Aim Orb
                orbs.Spawns.Add(new InvItem(269), range, 10);//Trap-See
                orbs.Spawns.Add(new InvItem(270), range, 10);//Trapbust
                orbs.Spawns.Add(new InvItem(276), range, 10);//Foe-Hold
                orbs.Spawns.Add(new InvItem(252), range, 10);//Mobile
                orbs.Spawns.Add(new InvItem(288), range, 10);//Roll Call
                orbs.Spawns.Add(new InvItem(289), range, 10);//Mug
                orbs.Spawns.Add(new InvItem(284), range, 10);//Mirror
            }

            {
                range = new IntRange(10, 25);
                orbs.Spawns.Add(new InvItem(251), range, 10);//Weather Orb
                orbs.Spawns.Add(new InvItem(286), range, 10);//Foe-Seal
                orbs.Spawns.Add(new InvItem(274), range, 10);//Freeze
                orbs.Spawns.Add(new InvItem(257), range, 10);//Devolve
                orbs.Spawns.Add(new InvItem(277), range, 10);//Nullify
            }

            {
                range = new IntRange(0, 20);
                orbs.Spawns.Add(new InvItem(283), range, 10);//Rebound
                orbs.Spawns.Add(new InvItem(268), range, 5);//All Protect
                orbs.Spawns.Add(new InvItem(253), range, 9);//Luminous
                orbs.Spawns.Add(new InvItem(259), range, 9);//Trawl
                orbs.Spawns.Add(new InvItem(261), range, 9);//Scanner
                orbs.Spawns.Add(new InvItem(268, true), range, 5);//All Protect
                orbs.Spawns.Add(new InvItem(253, true), range, 2);//Luminous
                orbs.Spawns.Add(new InvItem(259, true), range, 2);//Trawl
            }

            //held items
            CategorySpawn<InvItem> heldItems = new CategorySpawn<InvItem>();
            heldItems.SpawnRates.SetRange(2, new IntRange(0, 30));
            itemSpawnZoneStep.Spawns.Add("held", heldItems);

            for (int nn = 0; nn < 18; nn++)//Type booster
                heldItems.Spawns.Add(new InvItem(331 + nn), new IntRange(0, 30), 1);
            for (int nn = 0; nn < 18; nn++)//Type Plate
                heldItems.Spawns.Add(new InvItem(380 + nn), new IntRange(0, 30), 1);

            heldItems.Spawns.Add(new InvItem(303), new IntRange(0, 20), 2);//Mobile Scarf
            heldItems.Spawns.Add(new InvItem(304), new IntRange(0, 20), 2);//Pass Scarf


            heldItems.Spawns.Add(new InvItem(330), new IntRange(0, 25), 2);//Cover Band
            heldItems.Spawns.Add(new InvItem(329), new IntRange(0, 25), 1);//Reunion Cape
            heldItems.Spawns.Add(new InvItem(329, true), new IntRange(0, 25), 1);//Reunion Cape

            heldItems.Spawns.Add(new InvItem(306), new IntRange(0, 15), 2);//Trap Scarf
            heldItems.Spawns.Add(new InvItem(306, true), new IntRange(0, 15), 1);//Trap Scarf

            heldItems.Spawns.Add(new InvItem(307), new IntRange(0, 30), 2);//Grip Claw

            range = new IntRange(0, 20);
            heldItems.Spawns.Add(new InvItem(309), range, 2);//Twist Band
            heldItems.Spawns.Add(new InvItem(310), range, 1);//Metronome
            heldItems.Spawns.Add(new InvItem(309, true), range, 1);//Twist Band
            heldItems.Spawns.Add(new InvItem(310, true), range, 1);//Metronome
            heldItems.Spawns.Add(new InvItem(312), range, 1);//Shell Bell
            heldItems.Spawns.Add(new InvItem(313), range, 1);//Scope Lens
            heldItems.Spawns.Add(new InvItem(400), range, 2);//Power Band
            heldItems.Spawns.Add(new InvItem(401), range, 2);//Special Band
            heldItems.Spawns.Add(new InvItem(402), range, 2);//Defense Scarf
            heldItems.Spawns.Add(new InvItem(403), range, 2);//Zinc Band
            heldItems.Spawns.Add(new InvItem(314, true), range, 2);//Wide Lens
            heldItems.Spawns.Add(new InvItem(301, true), range, 2);//Pierce Band
            heldItems.Spawns.Add(new InvItem(312, true), range, 1);//Shell Bell
            heldItems.Spawns.Add(new InvItem(313, true), range, 1);//Scope Lens

            heldItems.Spawns.Add(new InvItem(311), new IntRange(0, 20), 2);//Shed Shell
            heldItems.Spawns.Add(new InvItem(311, true), new IntRange(0, 20), 1);//Shed Shell

            heldItems.Spawns.Add(new InvItem(328), new IntRange(0, 30), 2);//X-Ray Specs
            heldItems.Spawns.Add(new InvItem(328, true), new IntRange(0, 30), 1);//X-Ray Specs

            heldItems.Spawns.Add(new InvItem(407), new IntRange(0, 30), 2);//Goggle Specs
            heldItems.Spawns.Add(new InvItem(407, true), new IntRange(0, 30), 1);//Goggle Specs

            heldItems.Spawns.Add(new InvItem(404), new IntRange(0, 15), 2);//Big Root
            heldItems.Spawns.Add(new InvItem(404, true), new IntRange(0, 15), 1);//Big Root

            int stickRate = 2;
            range = new IntRange(0, 15);

            heldItems.Spawns.Add(new InvItem(406), range, stickRate);//Weather Rock
            heldItems.Spawns.Add(new InvItem(405), range, stickRate);//Expert Belt
            heldItems.Spawns.Add(new InvItem(320), range, stickRate);//Choice Scarf
            heldItems.Spawns.Add(new InvItem(319), range, stickRate);//Choice Specs
            heldItems.Spawns.Add(new InvItem(318), range, stickRate);//Choice Band
            heldItems.Spawns.Add(new InvItem(321), range, stickRate);//Assault Vest
            heldItems.Spawns.Add(new InvItem(322), range, stickRate);//Life Orb
            heldItems.Spawns.Add(new InvItem(315), range, stickRate);//Heal Ribbon

            stickRate = 1;
            range = new IntRange(15, 30);

            heldItems.Spawns.Add(new InvItem(406), range, stickRate);//Weather Rock
            heldItems.Spawns.Add(new InvItem(405), range, stickRate);//Expert Belt
            heldItems.Spawns.Add(new InvItem(320), range, stickRate);//Choice Scarf
            heldItems.Spawns.Add(new InvItem(319), range, stickRate);//Choice Specs
            heldItems.Spawns.Add(new InvItem(318), range, stickRate);//Choice Band
            heldItems.Spawns.Add(new InvItem(321), range, stickRate);//Assault Vest
            heldItems.Spawns.Add(new InvItem(322), range, stickRate);//Life Orb
            heldItems.Spawns.Add(new InvItem(315), range, stickRate);//Heal Ribbon


            heldItems.Spawns.Add(new InvItem(305), new IntRange(0, 30), 1);//Warp Scarf
            heldItems.Spawns.Add(new InvItem(305, true), new IntRange(0, 30), 1);//Warp Scarf


            range = new IntRange(8, 30);
            heldItems.Spawns.Add(new InvItem(323), range, 1);//Toxic Orb
            heldItems.Spawns.Add(new InvItem(324), range, 1);//Flame Orb
            heldItems.Spawns.Add(new InvItem(317), range, 10);//Sticky Barb
            heldItems.Spawns.Add(new InvItem(326), range, 1);//Ring Target

            //machines
            CategorySpawn<InvItem> machines = new CategorySpawn<InvItem>();
            machines.SpawnRates.SetRange(7, new IntRange(0, 30));
            itemSpawnZoneStep.Spawns.Add("tms", machines);

            range = new IntRange(0, 30);
            machines.Spawns.Add(new InvItem(589), range, 2);//TM Echoed Voice
            machines.Spawns.Add(new InvItem(590), range, 2);//TM Double Team
            machines.Spawns.Add(new InvItem(592), range, 2);//TM Toxic
            machines.Spawns.Add(new InvItem(593), range, 2);//TM Will-o-Wisp
            machines.Spawns.Add(new InvItem(594), range, 2);//TM Dragon Tail
            machines.Spawns.Add(new InvItem(596), range, 2);//TM Protect
            machines.Spawns.Add(new InvItem(597), range, 2);//TM Defog
            machines.Spawns.Add(new InvItem(598), range, 2);//TM Roar
            machines.Spawns.Add(new InvItem(600), range, 2);//TM Swagger
            machines.Spawns.Add(new InvItem(601), range, 2);//TM Dive
            machines.Spawns.Add(new InvItem(602), range, 2);//TM Fly
            machines.Spawns.Add(new InvItem(603), range, 2);//TM Facade
            machines.Spawns.Add(new InvItem(604), range, 2);//TM Rock Climb
            machines.Spawns.Add(new InvItem(605), range, 2);//TM Waterfall
            machines.Spawns.Add(new InvItem(606), range, 2);//TM Smack Down
            machines.Spawns.Add(new InvItem(607), range, 2);//TM Flame Charge
            machines.Spawns.Add(new InvItem(608), range, 2);//TM Low Sweep
            machines.Spawns.Add(new InvItem(609), range, 2);//TM Charge Beam
            machines.Spawns.Add(new InvItem(610), range, 2);//TM Payback
            machines.Spawns.Add(new InvItem(612), range, 2);//TM Wild Charge
            machines.Spawns.Add(new InvItem(613), range, 2);//TM Hone Claws
            machines.Spawns.Add(new InvItem(614), range, 2);//TM Telekinesis
            machines.Spawns.Add(new InvItem(615), range, 2);//TM Giga Impact
            machines.Spawns.Add(new InvItem(616), range, 2);//TM Rock Polish
            machines.Spawns.Add(new InvItem(617), range, 2);//TM Dig
            machines.Spawns.Add(new InvItem(618), range, 2);//TM Gyro Ball
            machines.Spawns.Add(new InvItem(621), range, 2);//TM Substitute
            machines.Spawns.Add(new InvItem(622), range, 2);//TM Trick Room
            machines.Spawns.Add(new InvItem(623), range, 2);//TM Safeguard
            machines.Spawns.Add(new InvItem(625), range, 2);//TM Venoshock
            machines.Spawns.Add(new InvItem(626), range, 2);//TM Work Up
            machines.Spawns.Add(new InvItem(627), range, 2);//TM Scald
            machines.Spawns.Add(new InvItem(629), range, 2);//TM Explosion
            machines.Spawns.Add(new InvItem(630), range, 2);//TM U-turn
            machines.Spawns.Add(new InvItem(631), range, 2);//TM Thunder Wave
            machines.Spawns.Add(new InvItem(632), range, 2);//TM Return
            machines.Spawns.Add(new InvItem(633), range, 2);//TM Pluck
            machines.Spawns.Add(new InvItem(634), range, 2);//TM Frustration
            machines.Spawns.Add(new InvItem(638), range, 2);//TM Thief
            machines.Spawns.Add(new InvItem(639), range, 2);//TM Acrobatics
            machines.Spawns.Add(new InvItem(642), range, 2);//TM False Swipe
            machines.Spawns.Add(new InvItem(643), range, 2);//TM Fling
            machines.Spawns.Add(new InvItem(644), range, 2);//TM Captivate
            machines.Spawns.Add(new InvItem(645), range, 2);//TM Roost
            machines.Spawns.Add(new InvItem(646), range, 2);//TM Infestation
            machines.Spawns.Add(new InvItem(647), range, 2);//TM Drain Punch
            machines.Spawns.Add(new InvItem(648), range, 2);//TM Water Pulse
            machines.Spawns.Add(new InvItem(650), range, 2);//TM Giga Drain
            machines.Spawns.Add(new InvItem(651), range, 2);//TM Shock Wave
            machines.Spawns.Add(new InvItem(652), range, 2);//TM Volt Switch
            machines.Spawns.Add(new InvItem(653), range, 2);//TM Steel Wing
            machines.Spawns.Add(new InvItem(655), range, 2);//TM Psyshock
            machines.Spawns.Add(new InvItem(656), range, 2);//TM Bulldoze
            machines.Spawns.Add(new InvItem(657), range, 2);//TM Poison Jab
            machines.Spawns.Add(new InvItem(659), range, 2);//TM Dream Eater
            machines.Spawns.Add(new InvItem(660), range, 2);//TM Thunder
            machines.Spawns.Add(new InvItem(661), range, 2);//TM X-Scissor
            machines.Spawns.Add(new InvItem(663), range, 2);//TM Retaliate
            machines.Spawns.Add(new InvItem(664), range, 2);//TM Reflect
            machines.Spawns.Add(new InvItem(665), range, 2);//TM Quash
            machines.Spawns.Add(new InvItem(667), range, 2);//TM Round
            machines.Spawns.Add(new InvItem(668), range, 2);//TM Aerial Ace
            machines.Spawns.Add(new InvItem(670), range, 2);//TM Incinerate
            machines.Spawns.Add(new InvItem(671), range, 2);//TM Struggle Bug
            machines.Spawns.Add(new InvItem(673), range, 2);//TM Dragon Claw
            machines.Spawns.Add(new InvItem(674), range, 2);//TM Rain Dance
            machines.Spawns.Add(new InvItem(675), range, 2);//TM Sunny Day
            machines.Spawns.Add(new InvItem(677), range, 2);//TM Sandstorm
            machines.Spawns.Add(new InvItem(678), range, 2);//TM Hail
            machines.Spawns.Add(new InvItem(679), range, 2);//TM Rock Tomb
            machines.Spawns.Add(new InvItem(680), range, 2);//TM Attract
            machines.Spawns.Add(new InvItem(681), range, 2);//TM Hidden Power
            machines.Spawns.Add(new InvItem(682), range, 2);//TM Taunt
            machines.Spawns.Add(new InvItem(684), range, 2);//TM Ice Beam
            machines.Spawns.Add(new InvItem(686), range, 2);//TM Light Screen
            machines.Spawns.Add(new InvItem(687), range, 2);//TM Stone Edge
            machines.Spawns.Add(new InvItem(688), range, 2);//TM Shadow Claw
            machines.Spawns.Add(new InvItem(689), range, 2);//TM Grass Knot
            machines.Spawns.Add(new InvItem(690), range, 2);//TM Brick Break
            machines.Spawns.Add(new InvItem(691), range, 2);//TM Calm Mind
            machines.Spawns.Add(new InvItem(692), range, 2);//TM Torment
            machines.Spawns.Add(new InvItem(693), range, 2);//TM Strength
            machines.Spawns.Add(new InvItem(694), range, 2);//TM Cut
            machines.Spawns.Add(new InvItem(695), range, 2);//TM Rock Smash
            machines.Spawns.Add(new InvItem(696), range, 2);//TM Bulk Up
            machines.Spawns.Add(new InvItem(698), range, 2);//TM Rest
            machines.Spawns.Add(new InvItem(699), range, 2);//TM Psych Up

            range = new IntRange(10, 30);

            machines.Spawns.Add(new InvItem(591), range, 1);//TM Psychic
            machines.Spawns.Add(new InvItem(595), range, 1);//TM Flamethrower
            machines.Spawns.Add(new InvItem(599), range, 1);//TM Hyper Beam
            machines.Spawns.Add(new InvItem(611), range, 1);//TM Blizzard
            machines.Spawns.Add(new InvItem(619), range, 1);//TM Rock Slide
            machines.Spawns.Add(new InvItem(620), range, 1);//TM Sludge Wave
            machines.Spawns.Add(new InvItem(624), range, 1);//TM Swords Dance
            machines.Spawns.Add(new InvItem(628), range, 1);//TM Energy Ball
            machines.Spawns.Add(new InvItem(635), range, 1);//TM Fire Blast
            machines.Spawns.Add(new InvItem(640), range, 1);//TM Thunderbolt
            machines.Spawns.Add(new InvItem(641), range, 1);//TM Shadow Ball
            machines.Spawns.Add(new InvItem(649), range, 1);//TM Dark Pulse
            machines.Spawns.Add(new InvItem(654), range, 1);//TM Earthquake
            machines.Spawns.Add(new InvItem(658), range, 1);//TM Frost Breath
            machines.Spawns.Add(new InvItem(662), range, 1);//TM Dazzling Gleam
            machines.Spawns.Add(new InvItem(666), range, 1);//TM Snarl
            machines.Spawns.Add(new InvItem(669), range, 1);//TM Focus Blast
            machines.Spawns.Add(new InvItem(672), range, 1);//TM Overheat
            machines.Spawns.Add(new InvItem(676), range, 1);//TM Sludge Bomb
            machines.Spawns.Add(new InvItem(683), range, 1);//TM Solar Beam
            machines.Spawns.Add(new InvItem(685), range, 1);//TM Flash Cannon
            machines.Spawns.Add(new InvItem(697), range, 1);//TM Surf

            machines.Spawns.Add(new InvItem(591, true), range, 1);//TM Psychic
            machines.Spawns.Add(new InvItem(595, true), range, 1);//TM Flamethrower
            machines.Spawns.Add(new InvItem(599, true), range, 1);//TM Hyper Beam
            machines.Spawns.Add(new InvItem(611, true), range, 1);//TM Blizzard
            machines.Spawns.Add(new InvItem(619, true), range, 1);//TM Rock Slide
            machines.Spawns.Add(new InvItem(620, true), range, 1);//TM Sludge Wave
            machines.Spawns.Add(new InvItem(624, true), range, 1);//TM Swords Dance
            machines.Spawns.Add(new InvItem(628, true), range, 1);//TM Energy Ball
            machines.Spawns.Add(new InvItem(635, true), range, 1);//TM Fire Blast
            machines.Spawns.Add(new InvItem(640, true), range, 1);//TM Thunderbolt
            machines.Spawns.Add(new InvItem(641, true), range, 1);//TM Shadow Ball
            machines.Spawns.Add(new InvItem(649, true), range, 1);//TM Dark Pulse
            machines.Spawns.Add(new InvItem(654, true), range, 1);//TM Earthquake
            machines.Spawns.Add(new InvItem(658, true), range, 1);//TM Frost Breath
            machines.Spawns.Add(new InvItem(662, true), range, 1);//TM Dazzling Gleam
            machines.Spawns.Add(new InvItem(666, true), range, 1);//TM Snarl
            machines.Spawns.Add(new InvItem(669, true), range, 1);//TM Focus Blast
            machines.Spawns.Add(new InvItem(672, true), range, 1);//TM Overheat
            machines.Spawns.Add(new InvItem(676, true), range, 1);//TM Sludge Bomb
            machines.Spawns.Add(new InvItem(683, true), range, 1);//TM Solar Beam
            machines.Spawns.Add(new InvItem(685, true), range, 1);//TM Flash Cannon
            machines.Spawns.Add(new InvItem(697, true), range, 1);//TM Surf

            //evo items
            CategorySpawn<InvItem> evoItems = new CategorySpawn<InvItem>();
            evoItems.SpawnRates.SetRange(3, new IntRange(0, 30));
            itemSpawnZoneStep.Spawns.Add("evo", evoItems);

            range = new IntRange(0, 25);
            evoItems.Spawns.Add(new InvItem(351), range, 10);//Fire Stone
            evoItems.Spawns.Add(new InvItem(352), range, 10);//Thunder Stone
            evoItems.Spawns.Add(new InvItem(353), range, 10);//Water Stone
            evoItems.Spawns.Add(new InvItem(355), range, 10);//Moon Stone
            evoItems.Spawns.Add(new InvItem(363), range, 10);//Dusk Stone
            evoItems.Spawns.Add(new InvItem(364), range, 10);//Dawn Stone
            evoItems.Spawns.Add(new InvItem(362), range, 10);//Shiny Stone
            evoItems.Spawns.Add(new InvItem(354), range, 10);//Leaf Stone
            evoItems.Spawns.Add(new InvItem(379), range, 10);//Ice Stone
            evoItems.Spawns.Add(new InvItem(377), range, 10);//Sun Ribbon
            evoItems.Spawns.Add(new InvItem(378), range, 10);//Moon Ribbon
            evoItems.Spawns.Add(new InvItem(347), range, 10);//Metal Coat
            floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


            //mobs
            TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
            poolSpawn.Priority = PR_RESPAWN_MOB;

            //19 Rattata : 33 Tackle
            poolSpawn.Spawns.Add(GetTeamMob(19, -1, 33, -1, -1, -1, new RandRange(3)), new IntRange(0, 2), 10);

            //173 Cleffa : 98 Magic Guard : 383 Pound
            poolSpawn.Spawns.Add(GetTeamMob(173, 98, 1, -1, -1, -1, new RandRange(3)), new IntRange(0, 3), 10);

            //427 Buneary : 50 Run Away : 1 Pound : 150 Splash
            poolSpawn.Spawns.Add(GetTeamMob(427, 50, 1, 150, -1, -1, new RandRange(3)), new IntRange(0, 3), 10);

            //016 Pidgey : 16 Gust : 28 Sand Attack
            poolSpawn.Spawns.Add(GetTeamMob(16, -1, 16, 28, -1, -1, new RandRange(3)), new IntRange(1, 3), 10);

            //1//175 Togepi : 204 Charm
            poolSpawn.Spawns.Add(GetTeamMob(175, 32, 204, -1, -1, -1, new RandRange(5)), new IntRange(1, 3), 10);

            //287 Slakoth : Truant : 010 Scratch
            poolSpawn.Spawns.Add(GetTeamMob(287, -1, 10, -1, -1, -1, new RandRange(6)), new IntRange(1, 4), 10);

            //265 Wurmple : 081 String Shot : 033 Tackle
            poolSpawn.Spawns.Add(GetTeamMob(265, -1, 81, 33, -1, -1, new RandRange(5)), new IntRange(1, 3), 10);


            //1//403 Shinx : 043 Leer : 033 Tackle
            poolSpawn.Spawns.Add(GetTeamMob(403, -1, 43, 33, -1, -1, new RandRange(4)), new IntRange(1, 3), 10);

            //420 Cherubi : 073 Leech Seed : 033 Tackle
            poolSpawn.Spawns.Add(GetTeamMob(420, -1, 73, 33, -1, -1, new RandRange(6)), new IntRange(2, 4), 10);

            //290 Nincada : 117 Bide : 010 Scratch : 106 Harden
            poolSpawn.Spawns.Add(GetTeamMob(290, -1, 117, 10, 106, -1, new RandRange(9)), new IntRange(2, 4), 10);

            //5//406 Budew : 071 Absorb : 078 Stun Spore : 346 Water Sport
            poolSpawn.Spawns.Add(GetTeamMob(406, -1, 71, 78, 346, -1, new RandRange(6)), new IntRange(2, 4), 10);

            //7//433 Chingling : 310 Astonish : 035 Wrap
            poolSpawn.Spawns.Add(GetTeamMob(433, -1, 310, 35, -1, -1, new RandRange(8)), new IntRange(2, 4), 10);

            //133 Eevee : 270 Helping Hand : 098 Quick Attack
            poolSpawn.Spawns.Add(GetTeamMob(133, -1, 270, 98, -1, -1, new RandRange(8)), new IntRange(3, 5), 10);

            //228 Houndour : 052 Ember : 123 Smog : 046 Roar
            poolSpawn.Spawns.Add(GetTeamMob(228, -1, 52, 123, 46, -1, new RandRange(7)), new IntRange(3, 5), 10);

            //172 Pichu : 084 Thundershock : 204 Charm
            poolSpawn.Spawns.Add(GetTeamMob(172, -1, 84, 204, -1, -1, new RandRange(5)), new IntRange(4, 5), 10);


            {
                //155 Cyndaquil : 43 Leer : 52 Ember
                poolSpawn.Spawns.Add(GetTeamMob(155, -1, 43, 52, -1, -1, new RandRange(10), 7, true), new IntRange(3, 5), 10);

                //152 Chikorita : 77 Poison Powder : 075 Razor Leaf
                poolSpawn.Spawns.Add(GetTeamMob(152, -1, 77, 22, -1, -1, new RandRange(10), 7, true), new IntRange(3, 5), 10);

                //158 Totodile : 44 Bite : 55 Water Gun
                poolSpawn.Spawns.Add(GetTeamMob(158, -1, 44, 55, -1, -1, new RandRange(10), 7, true), new IntRange(3, 5), 10);
            }


            {
                //266 Silcoon : 106 Harden : 450 Bug Bite
                poolSpawn.Spawns.Add(GetTeamMob(266, -1, 106, 450, -1, -1, new RandRange(8), TeamMemberSpawn.MemberRole.Leader, 8), new IntRange(4, 6), 10);
                //268 Cascoon : 106 Harden : 450 Bug Bite
                poolSpawn.Spawns.Add(GetTeamMob(268, -1, 106, 450, -1, -1, new RandRange(8), TeamMemberSpawn.MemberRole.Leader, 8), new IntRange(4, 6), 10);
            }

            //if (ii >= 9 && ii < 10)
            //{
            //    //174 * Igglybuff : 383 Copycat
            //    //appears in a special situation
            //    GetGenericMob(mobZoneStep, 174, -1, 383, -1, -1, -1, new RangeSpawn(15));
            //}


            //447 Riolu : 068 Counter : 098 Quick Attack
            poolSpawn.Spawns.Add(GetTeamMob(447, -1, 68, 98, -1, -1, new RandRange(11), TeamMemberSpawn.MemberRole.Leader), new IntRange(4, 6), 10);

            //090 Shellder : 062 Aurora Beam : 055 Water Gun
            poolSpawn.Spawns.Add(GetTeamMob(90, -1, 62, 55, -1, -1, new RandRange(10)), new IntRange(4, 5), 10);

            //102 Exeggcute : 140 Barrage : 115 Reflect
            poolSpawn.Spawns.Add(GetTeamMob(102, -1, 140, 115, -1, -1, new RandRange(10), TeamMemberSpawn.MemberRole.Support), new IntRange(5, 7), 100);

            //15//190 Aipom : 310 Astonish : 321 Tickle : 010 Scratch
            poolSpawn.Spawns.Add(GetTeamMob(190, -1, 310, 321, 10, -1, new RandRange(10), TeamMemberSpawn.MemberRole.Leader), new IntRange(5, 7), 10);


            //046 Paras : 78 Stun Spore : 141 Leech Life
            poolSpawn.Spawns.Add(GetTeamMob(46, -1, 78, 141, -1, -1, new RandRange(9)), new IntRange(5, 7), 10);

            //456 Finneon : 114 Storm Drain : 55 Water Gun : 16 Gust
            poolSpawn.Spawns.Add(GetTeamMob(456, 114, 55, 16, -1, -1, new RandRange(13), TeamMemberSpawn.MemberRole.Loner), new IntRange(5, 7), 10);

            //first individual, then in groups
            //4//261 Poochyena : 336 Howl : 44 Bite
            poolSpawn.Spawns.Add(GetTeamMob(261, -1, 336, 44, -1, -1, new RandRange(10), TeamMemberSpawn.MemberRole.Support), new IntRange(5, 7), 100);


            //353 Shuppet : 174 Curse : 101 Night Shade
            poolSpawn.Spawns.Add(GetTeamMob(353, -1, 174, 101, -1, -1, new RandRange(11)), new IntRange(6, 8), 10);


            //220 Swinub : 426 Mud Bomb
            poolSpawn.Spawns.Add(GetTeamMob(220, -1, 426, -1, -1, -1, new RandRange(13), TeamMemberSpawn.MemberRole.Leader), new IntRange(6, 8), 10);

            //108 Lickitung : 35 Wrap : 122 Lick
            poolSpawn.Spawns.Add(GetTeamMob(108, -1, 35, 122, -1, -1, new RandRange(14), TeamMemberSpawn.MemberRole.Leader), new IntRange(6, 8), 10);

            //352 Kecleon : 168 Thief
            poolSpawn.Spawns.Add(GetTeamMob(352, -1, 168, -1, -1, -1, new RandRange(11), TeamMemberSpawn.MemberRole.Loner, 11), new IntRange(6, 8), 10);

            //417 Pachirisu : 609 Nuzzle : 098 Quick Attack
            poolSpawn.Spawns.Add(GetTeamMob(417, -1, 609, 98, -1, -1, new RandRange(13)), new IntRange(6, 8), 10);

            //1//056 Mankey : 128 Defiant : 083 Anger Point : 043 Leer : 067 Low Kick
            poolSpawn.Spawns.Add(GetTeamMob(56, -1, 43, 67, -1, -1, new RandRange(14)), new IntRange(7, 9), 10);

            //052 Meowth : 6 Pay Day : 10 Scratch : Taunt
            poolSpawn.Spawns.Add(GetTeamMob(52, -1, 6, 10, -1, -1, new RandRange(14)), new IntRange(7, 9), 10);

            //316 Gulpin : 124 Sludge : 281 Yawn
            poolSpawn.Spawns.Add(GetTeamMob(316, -1, 124, 281, -1, -1, new RandRange(14)), new IntRange(7, 9), 10);

            //035 Clefairy : 56 Cute Charm : 516 Bestow : 1 Pound
            poolSpawn.Spawns.Add(GetTeamMob(35, 56, 516, 1, -1, -1, new RandRange(15), 15), new IntRange(7, 9), 10);

            //455 Carnivine : 275 Ingrain : 230 Sweet Scent : 44 Bite
            poolSpawn.Spawns.Add(GetTeamMob(455, -1, 275, 230, 44, -1, new RandRange(16)), new IntRange(7, 9), 10);

            //1//123 Scyther : 410 Vacuum Wave : 206 False Swipe
            poolSpawn.Spawns.Add(GetTeamMob(123, -1, 410, 206, -1, -1, new RandRange(11), TeamMemberSpawn.MemberRole.Loner), new IntRange(7, 9), 10);

            //39//286 Imprison?
            //037 Vulpix : 52 Ember
            poolSpawn.Spawns.Add(GetTeamMob(37, -1, 52, -1, -1, -1, new RandRange(18)), new IntRange(7, 9), 10);

            //8//127 Pinsir : 11 Vice Grip : 069 Seismic Toss
            poolSpawn.Spawns.Add(GetTeamMob(127, -1, 11, 69, -1, -1, new RandRange(15), TeamMemberSpawn.MemberRole.Loner), new IntRange(8, 10), 10);

            //21//096 Drowzee : 095 Hypnosis : 050 Disable : 096 Meditate
            poolSpawn.Spawns.Add(GetTeamMob(96, -1, 95, 50, 96, -1, new RandRange(16)), new IntRange(8, 10), 10);

            //438 Bonsly : 102 Mimic? : 67 Low Kick : 88 Rock Throw
            poolSpawn.Spawns.Add(GetTeamMob(438, -1, 102, 67, 88, -1, new RandRange(15), 10), new IntRange(8, 10), 10);

            //441 Chatot : 497 Echoed Voice : 297 Feather Dance
            poolSpawn.Spawns.Add(GetTeamMob(441, -1, 497, 297, -1, -1, new RandRange(18), TeamMemberSpawn.MemberRole.Loner), new IntRange(8, 10), 10);

            //29//439 Mime Jr. : 383 Copycat : 164 Substitute
            poolSpawn.Spawns.Add(GetTeamMob(439, -1, 383, 164, -1, -1, new RandRange(18)), new IntRange(8, 10), 10);

            //236 Tyrogue : 33 Tackle : 252 Fake Out
            poolSpawn.Spawns.Add(GetTeamMob(236, -1, 33, 252, -1, -1, new RandRange(16)), new IntRange(8, 10), 10);

            //288 Vigoroth : 281 Yawn : 303 Slack Off : 163 Slash
            poolSpawn.Spawns.Add(GetTeamMob(288, -1, 281, 303, 163, -1, new RandRange(18), TeamMemberSpawn.MemberRole.Loner), new IntRange(8, 10), 10);

            //328 Trapinch : 71 Arena Trap : 328 Sand Tomb : 91 Dig
            poolSpawn.Spawns.Add(GetTeamMob(328, 71, 328, 91, -1, -1, new RandRange(18), TeamMemberSpawn.MemberRole.Leader), new IntRange(9, 11), 10);

            //17//446 Munchlax : 033 Tackle : 133 Amnesia : 498 Chip Away
            poolSpawn.Spawns.Add(GetTeamMob(446, -1, 33, 133, 498, -1, new RandRange(20), 7, true), new IntRange(9, 11), 10);

            //246 Larvitar : 103 Screech : 157 Rock Slide
            poolSpawn.Spawns.Add(GetTeamMob(246, -1, 103, 157, -1, -1, new RandRange(18)), new IntRange(9, 11), 10);

            //100 Voltorb : 043 Soundproof : 451 Charge Beam : 120 Self-Destruct
            poolSpawn.Spawns.Add(GetTeamMob(100, 43, 451, 120, -1, -1, new RandRange(20)), new IntRange(9, 11), 10);

            //15//436 Bronzor : 149 Psywave : 286 Imprison
            poolSpawn.Spawns.Add(GetTeamMob(436, -1, 149, 286, -1, -1, new RandRange(20)), new IntRange(9, 11), 10);

            //296 Makuhita : 282 Knock Off : 292 Arm Thrust
            poolSpawn.Spawns.Add(GetTeamMob(296, -1, 282, 292, -1, -1, new RandRange(18)), new IntRange(10, 12), 10);

            //37//304 Aron : 232 Metal Claw : 334 Iron Defense

            //23//104 Cubone : 099 Rage : 125 Bone Club
            poolSpawn.Spawns.Add(GetTeamMob(104, -1, 99, 125, -1, -1, new RandRange(23)), new IntRange(10, 12), 10);

            //198 Murkrow : 228 Pursuit : 17 Wing Attack : 372 Assurance
            poolSpawn.Spawns.Add(GetTeamMob(198, -1, 228, 17, 372, -1, new RandRange(18)), new IntRange(10, 12), 10);

            //343 Baltoy : 322 Cosmic Power : 377 Heal Block : 189 Mud Slap
            poolSpawn.Spawns.Add(GetTeamMob(343, -1, 322, 377, 189, -1, new RandRange(19), TeamMemberSpawn.MemberRole.Support), new IntRange(11, 13), 10);

            //129 Magikarp : 150 Splash : 33 Tackle
            poolSpawn.Spawns.Add(GetTeamMob(129, -1, 150, 33, -1, -1, new RandRange(14)), new IntRange(11, 13), 10);

            //17//371 Bagon : 116 Focus Energy : 099 Rage : 029 Headbutt
            poolSpawn.Spawns.Add(GetTeamMob(371, -1, 116, 99, 29, -1, new RandRange(20)), new IntRange(11, 13), 10);

            //24//231 Phanpy : 205 Rollout : 021 Slam
            poolSpawn.Spawns.Add(GetTeamMob(231, -1, 205, 21, -1, -1, new RandRange(22)), new IntRange(11, 13), 10);

            //1//359 Absol : 013 Razor Wind
            poolSpawn.Spawns.Add(GetTeamMob(359, -1, 13, -1, -1, -1, new RandRange(20), TeamMemberSpawn.MemberRole.Loner), new IntRange(11, 13), 10);

            //035 Clefairy : 56 Cute Charm : 266 Follow Me : 3 Double Slap : 107 Minimize
            poolSpawn.Spawns.Add(GetTeamMob(35, 56, 266, 1, 107, -1, new RandRange(22), TeamMemberSpawn.MemberRole.Support), new IntRange(11, 13), 10);

            //206 Dunsparce : 180 Spite : 103 Screech : 246 Ancient Power
            poolSpawn.Spawns.Add(GetTeamMob(206, -1, 180, 103, 246, -1, new RandRange(22)), new IntRange(11, 13), 10);

            //194 Wooper : 341 Mud Shot : 21 Slam : 133 Amnesia
            poolSpawn.Spawns.Add(GetTeamMob(194, -1, 341, 21, 133, -1, new RandRange(24)), new IntRange(11, 13), 10);

            //185 Sudowoodo : 335 Block : 157 Rock Slide : 102 Mimic
            poolSpawn.Spawns.Add(GetTeamMob(185, -1, 335, 157, 102, -1, new RandRange(24), 10), new IntRange(11, 13), 10);

            //29//167 Spinarak : 169 Spider Web : 101 Night Shade
            poolSpawn.Spawns.Add(GetTeamMob(167, -1, 101, 169, -1, -1, new RandRange(25), TeamMemberSpawn.MemberRole.Support), new IntRange(12, 14), 10);

            //33//404 Luxio : 209 Spark : 268 Charge
            poolSpawn.Spawns.Add(GetTeamMob(404, -1, 209, 268, -1, -1, new RandRange(22)), new IntRange(12, 14), 10);

            //177 Natu : 100 Teleport : 248 Future Sight
            poolSpawn.Spawns.Add(GetTeamMob(177, -1, 100, 248, -1, -1, new RandRange(25), TeamMemberSpawn.MemberRole.Loner), new IntRange(12, 14), 10);

            //322 Numel : 481 Flame Burst
            poolSpawn.Spawns.Add(GetTeamMob(322, -1, 481, -1, -1, -1, new RandRange(22)), new IntRange(12, 14), 10);

            //095 Onix : 317 Rock Tomb : 157 Rock Slide
            poolSpawn.Spawns.Add(GetTeamMob(95, -1, 317, 157, -1, -1, new RandRange(24), TeamMemberSpawn.MemberRole.Loner), new IntRange(12, 14), 10);

            //8//238 Smoochum : 186 Sweet Kiss : 181 Powder Snow
            poolSpawn.Spawns.Add(GetTeamMob(238, -1, 186, 181, -1, -1, new RandRange(23)), new IntRange(12, 14), 10);

            //352 Kecleon : 168 Thief : 425 Shadow Sneak
            poolSpawn.Spawns.Add(GetTeamMob(352, -1, 168, 425, -1, -1, new RandRange(25), TeamMemberSpawn.MemberRole.Support, 11), new IntRange(13, 15), 10);

            //1//132 Ditto : 144 Transform
            poolSpawn.Spawns.Add(GetTeamMob(132, -1, 144, -1, -1, -1, new RandRange(20), TeamMemberSpawn.MemberRole.Loner), new IntRange(13, 15), 10);

            //200 Misdreavus : 180 Spite : 310 Astonish : 220 Pain Split
            poolSpawn.Spawns.Add(GetTeamMob(200, -1, 180, 310, 220, -1, new RandRange(25)), new IntRange(13, 15), 10);

            //107 Hitmonchan : 264 Focus Punch : 193 Foresight
            poolSpawn.Spawns.Add(GetTeamMob(107, -1, 264, 193, -1, -1, new RandRange(23), TeamMemberSpawn.MemberRole.Support), new IntRange(12, 14), 10);

            //037-1 Vulpix : 196 Icy Wind
            poolSpawn.Spawns.Add(GetTeamMob(new MonsterID(37, 1, -1, Gender.Unknown), -1, 196, -1, -1, -1, new RandRange(25)), new IntRange(12, 14), 10);

            //037 Vulpix : 52 Ember : 506 Hex
            poolSpawn.Spawns.Add(GetTeamMob(37, -1, 52, 506, -1, -1, new RandRange(28)), new IntRange(14, 16), 10);

            //299 Nosepass : 408 Power Gem : 435 Discharge
            poolSpawn.Spawns.Add(GetTeamMob(299, -1, 408, 435, -1, -1, new RandRange(21), TeamMemberSpawn.MemberRole.Loner), new IntRange(14, 16), 10);

            //218 Slugma : 510 Incinerate : 106 Harden
            poolSpawn.Spawns.Add(GetTeamMob(218, -1, 510, 106, -1, -1, new RandRange(23), TeamMemberSpawn.MemberRole.Loner), new IntRange(14, 16), 10);

            //136 Flareon : 424 Fire Fang
            poolSpawn.Spawns.Add(GetTeamMob(136, -1, 424, -1, -1, -1, new RandRange(23)), new IntRange(14, 16), 10);

            //463 Lickilicky : 205 Rollout : 438 Power Whip : 378 Wring Out : 35 Wrap
            poolSpawn.Spawns.Add(GetTeamMob(463, -1, 205, 438, 378, 35, new RandRange(24)), new IntRange(15, 17), 10);

            //476 Probopass : 443 Magnet Bomb : 393 Magnet Rise : 414 Earth Power
            poolSpawn.Spawns.Add(GetTeamMob(476, -1, 443, 393, 414, -1, new RandRange(26), TeamMemberSpawn.MemberRole.Loner), new IntRange(15, 17), 10);

            //353 Shuppet : 271 Trick : 180 Spite
            poolSpawn.Spawns.Add(GetTeamMob(353, -1, 271, 180, -1, -1, new RandRange(25), 11), new IntRange(15, 17), 10);

            //136 Flareon : 83 Fire Spin : 387 Last Resort
            poolSpawn.Spawns.Add(GetTeamMob(136, -1, 83, 387, -1, -1, new RandRange(26)), new IntRange(15, 17), 10);

            //23//294 Loudred : 103 Screech : 023 Stomp

            //34//020 Raticate : 162 Super Fang : 389 Sucker Punch : 158 Hyper Fang
            //34//019 Rattata : 283 Endeavor : 098 Quick Attack

            //34//075 Graveler : 446 Stealth Rock : 350 Rock Blast

            //26//223 Remoraid : 055 Hustle : 116 Focus Energy : 199 Lock-On : 352 Water Pulse

            //31//459 Snover : 420 Ice Shard : 320 Grass Whistle : 275 Ingrain

            //44//143 Snorlax : 187 Belly Drum : 156 Rest : 034 Body Slam

            //29//176 Togetic : 273 Wish : 227 Encore : 584 Fairy Wind

            //344 Claydol : 60 Psybeam : 246 Ancient Power : 100 Teleport
            poolSpawn.Spawns.Add(GetTeamMob(344, -1, 60, 246, 100, -1, new RandRange(28), 13), new IntRange(15, 17), 10);

            //23//226 Mantine : 469 Wide Guard : 017 Wing Attack

            //29//125 Electabuzz : 009 Thunder Punch : 129 Swift : 113 Light Screen
            //29//499 Magmar : 007 Fire Punch : 499 Clear Smog : 109 Confuse Ray
            //18//124 Jynx : 008 Ice Punch : 577 Draining Kiss : 531 Heart Stamp

            //42//326 Grumpig : 277 Magic Coat : 473 Psyshock : 316 Odor Sleuth

            //106 Hitmonlee : 469 Wide Guard : 136 High Jump Kick
            poolSpawn.Spawns.Add(GetTeamMob(106, -1, 469, 136, -1, -1, new RandRange(27), TeamMemberSpawn.MemberRole.Loner), new IntRange(16, 18), 10);

            //114 Haze?
            //4//109 Koffing : 499 Clear Smog : 120 Self-Destruct
            poolSpawn.Spawns.Add(GetTeamMob(109, 26, 499, 120, -1, -1, new RandRange(27)), new IntRange(16, 18), 10);

            //219 Magcargo : 436 Lava Plume : 414 Earth Power
            poolSpawn.Spawns.Add(GetTeamMob(219, -1, 436, 414, -1, -1, new RandRange(25), TeamMemberSpawn.MemberRole.Loner), new IntRange(16, 18), 10);

            //46//599 Muk : 151 Acid Armor : 188 Sludge Bomb

            //69//189 Jumpluff : 476 Rage Powder : 262 Memento : 073 Leech Seed

            //47//185 Sudowoodo : 335 Block : 359 Hammer Arm : 452 Wood Hammer : Rock Slide

            //50//162 Furret : 382 Me First : 270 Helping Hand : 226 Baton Pass

            //50//229 Houndoom : 373 Embargo : 053 Flamethrower : 492 Foul Play : 251 Beat Up

            //49//082 Magneton : 604 Electric Terrain : 199 Lock-On : 192 Zap Cannon

            //359 Absol : 14 Swords Dance : 400 Night Slash : 427 Psycho Cut  (3 tiles in front, 1 tile away)
            poolSpawn.Spawns.Add(GetTeamMob(359, -1, 14, 400, 427, -1, new RandRange(28)), new IntRange(17, 19), 10);

            //303 Mawile : 242 Crunch : 442 Iron Head : 313 Fake Tears?
            poolSpawn.Spawns.Add(GetTeamMob(303, -1, 242, 442, 313, -1, new RandRange(27)), new IntRange(17, 19), 10);

            //006 Charizard : 126 Fire Blast : 403 Air Slash
            {
                TeamMemberSpawn mob = GetTeamMob(6, -1, 126, 17, -1, -1, new RandRange(45), TeamMemberSpawn.MemberRole.Loner, 7, true);
                mob.Spawn.SpawnFeatures.Add(new MobSpawnItem(true, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93));
                poolSpawn.Spawns.Add(mob, new IntRange(17, 19), 10);
            }

            //36//153 Bayleef : 113 Light Screen : 115 Reflect : 235 Synthesis : 075 Razor Leaf : 363 Natural Gift

            //40//286 Breloom : 090 Poison Heal : 073 Leech Seed : 147 Spore

            //32//332 Cacturne : 596 Spiky Shield : Needle Arm

            //358 Wake-Up Slap, 265 Smelling Salts?
            //297 Hariyama : 395 Force Palm : 362 Brine
            poolSpawn.Spawns.Add(GetTeamMob(297, -1, 395, 362, -1, -1, new RandRange(30)), new IntRange(17, 19), 10);

            //25//315 Roselia : 230 Sweet Scent : 320 Grass Whistle : 202 Giga Drain

            //491 Acid Spray? 380 Gastro Acid?
            //317 Swalot : 60 Sticky Hold : 188 Sludge Bomb : 151 Acid Armor
            poolSpawn.Spawns.Add(GetTeamMob(317, 60, 188, 151, -1, -1, new RandRange(32), TeamMemberSpawn.MemberRole.Loner), new IntRange(18, 20), 10);

            //50//428 Lopunny : 415 Switcheroo : 494 Entrainment : 98 Quick Attack

            //27//358 Chimecho : 215 Heal Bell : 281 Yawn
            poolSpawn.Spawns.Add(GetTeamMob(358, -1, 215, 281, 93, -1, new RandRange(30), TeamMemberSpawn.MemberRole.Support), new IntRange(19, 21), 10);

            //20//171 Lanturn : 035 Illuminate : 145 Bubble Beam : 486 Electro Ball : 254 Stockpile

            //47//171 Lanturn : 035 Illuminate : 435 Discharge : 598 Eerie Impulse : 392 Aqua Ring

            //46//334 Altaria : 054 Mist : 143 Sky Attack : 195 Perish Song : 219 Safeguard : 287 Refresh

            //37//417 Pachirisu : 162 Super Fang : 609 Nuzzle : 186 Sweet Kiss

            //28//028 Sandslash : 008 Sand Veil : 306 Crush Claw : 163 Slash

            //288 Grudge?
            //292 Shedinja : 180 Spite : 210 Fury Cutter : 566 Phantom Force
            poolSpawn.Spawns.Add(GetTeamMob(292, -1, 180, 210, 566, -1, new RandRange(35)), new IntRange(19, 21), 10);

            //53//465 Tangrowth : 144 Regenerator : 438 Power Whip

            //43//442 Spiritomb : 109 Confuse Ray : 262 Memento : 095 Hypnosis

            //46//214 Heracross : 068 Swarm : 153 Moxie : 280 Brick Break : 224 Megahorn : 203 Endure : 179 Reversal

            //35//329 Vibrava : 225 Dragon Breath : 048 Supersonic

            //44//076 Golem : 153 Explosion : 205 Rollout

            //38//324 Torkoal : 257 Heat Wave : 334 Iron Defense : 175 Flail

            //39 Jigglypuff : 47 Sing : 156 Rest : 574 Disarming Voice
            poolSpawn.Spawns.Add(GetTeamMob(39, -1, 47, 156, -1, -1, new RandRange(35)), new IntRange(19, 21), 10);

            //373 Embargo? 289 Snatch?
            //354 Banette : 130 Cursed Body : 288 Grudge : 425 Shadow Sneak
            poolSpawn.Spawns.Add(GetTeamMob(354, 130, 288, 425, -1, -1, new RandRange(32)), new IntRange(19, 21), 10);

            //33//321 Wailord : 323 Water Spout : 156 Rest : 250 Whirlpool

            //31//308 Medicham : 364 Feint : 244 Psych Up : 136 High Jump Kick

            //1//097 Hypno : 171 Nightmare : 093 Confusion : 415 Switcheroo
            poolSpawn.Spawns.Add(GetTeamMob(97, -1, 171, 93, 415, -1, new RandRange(33), TeamMemberSpawn.MemberRole.Support, 11), new IntRange(19, 24), 10);

            //43//411 Bastiodon : 368 Metal Burst : 269 Taunt

            //27//264 Linoone : 343 Covet : 415 Switcheroo : 516 Bestow // to allies?

            //37//327 Spinda : 298 Teeter Dance : 253 Uproar
            //21//441 Chatot : 448 Chatter : 590 Confide : 119 Mirror Move

            //21//212 Scizor : 418 Bullet Punch : 232 Metal Claw

            //48//157 Typhlosion : 436 Lava Plume : 053 Flamethrower

            //43//442 Spiritomb : 095 Hypnosis : 138 Dream Eater
            poolSpawn.Spawns.Add(GetTeamMob(442, -1, 95, 138, -1, -1, new RandRange(32)), new IntRange(19, 22), 10);

            //37//311 Plusle : 097 Agility : 270 Helping Hand
            //40//312 Minun : 486 Electro Ball : 376 Trump Card
            //crowd with one

            //51//435 Skuntank : 053 Flamethrower : 262 Memento

            //51//319 Sharpedo : 130 Skull Bash : 242 Crunch

            //32//199 Slowking : 144 Regenerator : 303 Slack Off : 505 Heal Pulse : 428 Zen Headbutt

            //1//121 Starmie : 035 Illuminate : 105 Recover : 129 Swift

            //017 Pidgeotto : 119 Mirror Move : 17 Wing Attack
            poolSpawn.Spawns.Add(GetTeamMob(17, -1, 119, 17, -1, -1, new RandRange(34)), new IntRange(20, 22), 10);

            //361 Snorunt : 196 Icy Wind : 181 Powder Snow
            poolSpawn.Spawns.Add(GetTeamMob(361, -1, 196, 181, -1, -1, new RandRange(33)), new IntRange(20, 22), 10);

            //215 Sneasel : 124 Pickpocket : 097 Agility : 196 Icy Wind : 185 Feint Attack
            poolSpawn.Spawns.Add(GetTeamMob(215, 124, 97, 196, 185, -1, new RandRange(35), TeamMemberSpawn.MemberRole.Support), new IntRange(20, 22), 10);

            //15//369 Relicanth : 069 Rock Head : 457 Head Smash : 317 Rock Tomb

            //15//466 Electivire : 486 Electro Ball : 009 Thunder Punch : 569 Ion Deluge

            //45//405 Luxray : 528 Wild Charge : 268 Charge

            //15//335 Zangoose : 279 Revenge : 468 Hone Claws : 098 Quick Attack

            //23//110 Weezing : 114 Haze : 120 Self-Destruct

            //128 Tauros : 037 Thrash : 371 Payback : 036 Take Down : 099 Rage

            //103 Exeggutor : 140 Barrage : 121 Egg Bomb : 402 Seed Bomb


            //472 Gliscor : 512 Acrobatics

            //40//262 Mightyena : 046 Roar : 036 Take Down : 555 Snarl

            //429 Mismagius : 595 Mystical Fire : 381 Lucky Chant
            poolSpawn.Spawns.Add(GetTeamMob(429, -1, 595, 381, -1, -1, new RandRange(33), TeamMemberSpawn.MemberRole.Support), new IntRange(19, 21), 10);

            //352 Kecleon : 168 Thief : 425 Shadow Sneak : 485 Synchronoise
            poolSpawn.Spawns.Add(GetTeamMob(352, -1, 168, 425, 485, -1, new RandRange(33), TeamMemberSpawn.MemberRole.Support, 11), new IntRange(19, 21), 10);


            //29//122 Mr. Mime : 164 Substitute : 112 Barrier : 501 Quick Guard

            //35//042 Golbat : 109 Confuse Ray : 103 Screech : 305 Poison Fang : 512 Acrobatics

            //25//142 Aerodactyl : 367 Pressure : 246 Ancient Power : 017 Wing attack

            //053 Persian : 415 Switcheroo : 185 Feint Attack : 129 Swift
            {
                TeamMemberSpawn mob = GetTeamMob(53, -1, 415, 185, 129, -1, new RandRange(33), 11);
                mob.Spawn.SpawnFeatures.Add(new MobSpawnItem(true, 323, 324, 325, 326));
                poolSpawn.Spawns.Add(mob, new IntRange(20, 22), 10);
            }

            //46//162 Furret : 133 Amnesia : 266 Follow Me : 226 Baton Pass

            //58//168 Ariados : 564 Sticky Web : 599 Venom Drench : 398 Poison Jab

            //340 Whiscash : 107 Anticipation : 248 Future Sight : 209 Spark : 089 Earthquake : 401 Aqua Tail

            //17//024 Arbok : 137 Glare : 035 Wrap : 103 Screech

            //42//362 Glalie : 573 Freeze-Dry : 196 Icy Wind

            //135 Jolteon : 86 Thunder Wave : 97 Agility : 42 Pin Missile
            poolSpawn.Spawns.Add(GetTeamMob(135, -1, 86, 97, 42, -1, new RandRange(35)), new IntRange(21, 23), 10);

            //448 Lucario : 370 Close Combat

            //47//308 Medicham : 105 Recover : 203 Endure : 179 Reversal

            //45//222 Corsola : 203 Endure : 243 Mirror Coat : 105 Recover

            //269 Dustox : 92 Toxic : 236 Moonlight : 405 Bug Buzz
            poolSpawn.Spawns.Add(GetTeamMob(269, -1, 92, 236, 405, -1, new RandRange(34)), new IntRange(21, 23), 10);
            //267 Beautifly : 213 Attract : 483 Quiver Dance : 202 Giga Drain : 318 Silver Wind
            poolSpawn.Spawns.Add(GetTeamMob(267, -1, 213, 483, 202, 318, new RandRange(34)), new IntRange(21, 23), 10);

            //48//186 Politoed : 195 Perish Song : 304 Hyper Voice

            //34//Sunflora : Solar Power : Sunny Day : Solar Beam

            //170 Mind Reader, 154 Fury Swipes, 405 Bug Buzz
            //291 Ninjask : 14 Swords Dance : 210 Fury Cutter
            poolSpawn.Spawns.Add(GetTeamMob(291, -1, 14, 210, -1, -1, new RandRange(38), 13), new IntRange(21, 23), 10);

            //197 Umbreon : 212 Mean Look : 185 Feint Attack
            poolSpawn.Spawns.Add(GetTeamMob(197, -1, 212, 185, -1, -1, new RandRange(36), TeamMemberSpawn.MemberRole.Support), new IntRange(22, 24), 10);

            //32//262 Mightyena : 046 Roar : 168 Thief : 373 Embargo //run away when low on HP
            poolSpawn.Spawns.Add(GetTeamMob(262, -1, 46, 168, 373, -1, new RandRange(37), TeamMemberSpawn.MemberRole.Support, 13), new IntRange(22, 24), 10);

            //221 Piloswine : 423 Ice Fang : 31 Fury Attack : 36 Take Down
            poolSpawn.Spawns.Add(GetTeamMob(221, -1, 423, 31, 36, -1, new RandRange(38)), new IntRange(22, 24), 10);

            //101 Electrode : 205 Rollout : 153 Explosion : 113 Light Screen
            poolSpawn.Spawns.Add(GetTeamMob(101, -1, 205, 153, -1, -1, new RandRange(38), TeamMemberSpawn.MemberRole.Support), new IntRange(22, 24), 10);

            //1//225 Delibird : 217 Present

            poolSpawn.Spawns.Add(GetTeamMob(225, -1, 217, -1, -1, -1, new RandRange(39), TeamMemberSpawn.MemberRole.Support), new IntRange(22, 25), 10);

            //29//368 Gorebyss : 504 Shell Smash : 226 Baton Pass

            //59//105 Marowak : 155 Bonemarang : 198 Bone Rush : 514 Retaliate
            poolSpawn.Spawns.Add(GetTeamMob(105, -1, 155, 198, 514, -1, new RandRange(37), TeamMemberSpawn.MemberRole.Support), new IntRange(22, 24), 10);

            //53//462 Magnezone : 393 Magnet Rise : 435 Discharge

            //50//315 Roselia : 312 Aromatherapy : 235 Synthesis : 080 Petal Dance

            //36//460 Abomasnow : 420 Ice Shard : 320 Grass Whistle : 452 Wood Hammer

            //52//466 Drifblim : 138 Flare Boost : 254 Stockpile : 466 Ominous Wind : 226 Baton Pass

            //91 Cloyster : 92 Skill Link : 182 Protect : 131 Spike Cannon : 191 Spikes
            poolSpawn.Spawns.Add(GetTeamMob(91, 92, 182, 131, 191, -1, new RandRange(36)), new IntRange(23, 25), 10);

            //478 Froslass : 130 Cursed Body : 194 Destiny Bond : 58 Ice Beam
            poolSpawn.Spawns.Add(GetTeamMob(478, 130, 194, 58, -1, -1, new RandRange(36)), new IntRange(23, 25), 10);

            //110 Weezing : 194 Destiny Bond : 153 Explosion : 188 Sludge Bomb
            {
                TeamMemberSpawn mob = GetTeamMob(110, 256, 194, 153, -1, -1, new RandRange(50), TeamMemberSpawn.MemberRole.Loner, 17, true);
                mob.Spawn.SpawnFeatures.Add(new MobSpawnItem(true, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93));
                poolSpawn.Spawns.Add(mob, new IntRange(22, 25), 10);
            }

            //67//139 Omastar : 504 Shell Smash : 131 Spike Cannon : 362 Brine

            //047 Parasect : 27 Effect Spore : 476 Rage Powder : 147 Spore
            poolSpawn.Spawns.Add(GetTeamMob(47, 27, 476, 147, -1, -1, new RandRange(38), TeamMemberSpawn.MemberRole.Support), new IntRange(23, 26), 10);

            //457 Lumineon : 33 Swift Swim : 318 Silver Wind : 369 U-Turn : 445 Captivate
            poolSpawn.Spawns.Add(GetTeamMob(457, 33, 318, 369, 445, -1, new RandRange(38)), new IntRange(24, 26), 10);

            //196 Espeon : 094 Psychic : 234 Morning Sun
            poolSpawn.Spawns.Add(GetTeamMob(196, -1, 94, 234, -1, -1, new RandRange(35)), new IntRange(24, 26), 10);

            //36//Quagsire : 133 Amnesia : 089 Earthquake

            //48//Bibarel : 133 Amnesia : 158 Hyper Fang : 276 Superpower

            //32//131 Lapras : 047 Sing : 057 Surf : 195 Perish Song : 058 Ice Beam

            //1//132 Ditto : 144 Transform

            //41//085 Dodrio : 161 Tri-Attack : 065 Drill Peck

            //1//164 Noctowl : 143 Sky Attack : 095 Hypnosis

            //130 Skull Bash?
            //009 Blastoise : 056 Hydro Pump
            poolSpawn.Spawns.Add(GetTeamMob(9, -1, 56, -1, -1, -1, new RandRange(40), TeamMemberSpawn.MemberRole.Loner), new IntRange(24, 27), 10);

            //31//475 Gallade : 469 Wide Guard : 427 Psycho Cut : 100 Teleport : 14 Swords Dance
            poolSpawn.Spawns.Add(GetTeamMob(475, -1, 469, 427, 100, 14, new RandRange(40), TeamMemberSpawn.MemberRole.Leader, 13), new IntRange(24, 27), 10);
            //282 Gardevoir : 94 Psychic : Wish : Moonblast
            poolSpawn.Spawns.Add(GetTeamMob(282, -1, 94, 273, 585, -1, new RandRange(40), TeamMemberSpawn.MemberRole.Support), new IntRange(24, 27), 10);

            //036 Clefable : 98 Magic Guard : 309 Meteor Mash : 236 Moonlight : 227 Encore

            //9//437 Bronzong : 241 Sunny Day : 240 Rain Dance : 286 Imprison
            poolSpawn.Spawns.Add(GetTeamMob(437, -1, 241, 240, 286, -1, new RandRange(42), 13), new IntRange(24, 28), 10);

            //13//358 Chimecho : 361 Healing Wish : 281 Yawn : 35 Wrap
            poolSpawn.Spawns.Add(GetTeamMob(358, -1, 361, 281, 35, -1, new RandRange(40), TeamMemberSpawn.MemberRole.Support), new IntRange(24, 27), 10);

            //386 Punishment?
            //289 Slaking : 359 Hammer Arm : 227 Encore : 303 Slack Off
            poolSpawn.Spawns.Add(GetTeamMob(289, -1, 359, 227, 303, -1, new RandRange(45), TeamMemberSpawn.MemberRole.Loner), new IntRange(24, 26), 10);

            //1//468 Togekiss : 032 Serene Grace : 055 Hustle : 403 Air Slash : 396 Aura Sphere : 245 Extreme Speed

            //461 Weavile : 251 Beat Up : 289 Snatch : 400 Night Slash : 008 Ice Punch
            poolSpawn.Spawns.Add(GetTeamMob(461, -1, 251, 289, 400, 8, new RandRange(38), TeamMemberSpawn.MemberRole.Loner), new IntRange(24, 27), 10);

            //63//428 Lopunny : 361 Healing Wish : 203 Endure


            //29//437 Bronzong : 248 Future Sight : 377 Heal Block : 286 Imprison

            //026 Raichu : 031 Lightning Rod : 087 Thunder
            poolSpawn.Spawns.Add(GetTeamMob(26, 31, 87, -1, -1, -1, new RandRange(41)), new IntRange(26, 30), 10);

            //134 Vaporeon : 392 Aqua Ring : 055 Water Gun : 240 Rain Dance
            poolSpawn.Spawns.Add(GetTeamMob(134, -1, 392, 55, 240, -1, new RandRange(39), TeamMemberSpawn.MemberRole.Support), new IntRange(26, 30), 10);

            //43//315 Roserade : 311 Weather Ball : 312 Aromatherapy : 345 Magical Leaf
            poolSpawn.Spawns.Add(GetTeamMob(407, -1, 311, 312, 345, -1, new RandRange(43)), new IntRange(26, 30), 10);

            //33//464 Rhyperior : 116 Solid Rock : 439 Rock Wrecker : 529 Drill Run


            //018 Pidgeot : 051 Keen Eye : 403 Air Slash : 542 Hurricane
            poolSpawn.Spawns.Add(GetTeamMob(18, 51, 143, 403, 542, -1, new RandRange(42)), new IntRange(27, 30), 10);

            //Lucario: Endure : Final Gambit : Reversal

            //60//230 Kingdra : 116 Focus Energy : 434 Draco Meteor : 056 Hydro Pump

            //have something with Haze support a Draco Meteor user, or Baton Pass?  Or Power Swap

            //038 Ninetales : 219 Safeguard : 53 Flamethrower : 286 Imprison
            poolSpawn.Spawns.Add(GetTeamMob(38, -1, 219, 53, 286, -1, new RandRange(46), TeamMemberSpawn.MemberRole.Support), new IntRange(27, 30), 10);

            //003 Venusaur : 76 Solarbeam : 79 Sleep Powder
            poolSpawn.Spawns.Add(GetTeamMob(3, -1, 76, 79, -1, -1, new RandRange(48)), new IntRange(28, 30), 10);

            //421 Cherrim : 80 Petal Dance, 270 Helping Hand : 241 Sunny Day
            poolSpawn.Spawns.Add(GetTeamMob(421, -1, 80, 270, 241, -1, new RandRange(47), TeamMemberSpawn.MemberRole.Support), new IntRange(28, 30), 10);

            //330 Flygon : 89 Earthquake : 525 Dragon Tail
            poolSpawn.Spawns.Add(GetTeamMob(330, -1, 89, 525, -1, -1, new RandRange(50), TeamMemberSpawn.MemberRole.Loner), new IntRange(28, 30), 10);


            poolSpawn.TeamSizes.Add(1, new IntRange(0, 30), 12);
            poolSpawn.TeamSizes.Add(2, new IntRange(5, 10), 3);
            poolSpawn.TeamSizes.Add(2, new IntRange(10, 15), 4);
            poolSpawn.TeamSizes.Add(2, new IntRange(15, 30), 6);

            poolSpawn.TeamSizes.Add(3, new IntRange(15, 30), 3);
            poolSpawn.TeamSizes.Add(3, new IntRange(24, 30), 4);

            floorSegment.ZoneSteps.Add(poolSpawn);


            TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
            tileSpawn.Priority = PR_RESPAWN_TRAP;
            tileSpawn.Spawns.Add(new EffectTile(7, false), new IntRange(0, max_floors), 10);//mud trap
            tileSpawn.Spawns.Add(new EffectTile(13, true), new IntRange(0, max_floors), 10);//warp trap
            tileSpawn.Spawns.Add(new EffectTile(14, false), new IntRange(0, max_floors), 10);//gust trap
            tileSpawn.Spawns.Add(new EffectTile(17, false), new IntRange(0, max_floors), 10);//chestnut trap
            tileSpawn.Spawns.Add(new EffectTile(3, false), new IntRange(0, max_floors), 10);//poison trap
            tileSpawn.Spawns.Add(new EffectTile(4, false), new IntRange(0, max_floors), 10);//sleep trap
            tileSpawn.Spawns.Add(new EffectTile(11, false), new IntRange(0, max_floors), 10);//sticky trap
            tileSpawn.Spawns.Add(new EffectTile(8, false), new IntRange(0, max_floors), 10);//seal trap
            tileSpawn.Spawns.Add(new EffectTile(18, false), new IntRange(0, max_floors), 10);//selfdestruct trap
            tileSpawn.Spawns.Add(new EffectTile(23, true), new IntRange(0, 15), 10);//trip trap
            tileSpawn.Spawns.Add(new EffectTile(23, false), new IntRange(15, max_floors), 10);//trip trap
            tileSpawn.Spawns.Add(new EffectTile(25, true), new IntRange(0, max_floors), 10);//hunger trap
            tileSpawn.Spawns.Add(new EffectTile(12, true), new IntRange(0, 15), 3);//apple trap
            tileSpawn.Spawns.Add(new EffectTile(12, false), new IntRange(15, max_floors), 3);//apple trap
            tileSpawn.Spawns.Add(new EffectTile(9, true), new IntRange(0, max_floors), 10);//pp-leech trap
            tileSpawn.Spawns.Add(new EffectTile(15, false), new IntRange(0, max_floors), 10);//summon trap
            tileSpawn.Spawns.Add(new EffectTile(19, false), new IntRange(0, max_floors), 10);//explosion trap
            tileSpawn.Spawns.Add(new EffectTile(6, false), new IntRange(0, max_floors), 10);//slow trap
            tileSpawn.Spawns.Add(new EffectTile(5, false), new IntRange(0, max_floors), 10);//spin trap
            tileSpawn.Spawns.Add(new EffectTile(10, false), new IntRange(0, max_floors), 10);//grimy trap
            tileSpawn.Spawns.Add(new EffectTile(28, true), new IntRange(0, max_floors), 20);//trigger trap
                                                                                            //pokemon trap
            tileSpawn.Spawns.Add(new EffectTile(24, true), new IntRange(15, max_floors), 10);//grudge trap
                                                                                             //training switch
            floorSegment.ZoneSteps.Add(tileSpawn);


            SpawnList<IGenPriority> appleZoneSpawns = new SpawnList<IGenPriority>();
            appleZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(1))))), 10);
            SpreadStepZoneStep appleZoneStep = new SpreadStepZoneStep(new SpreadPlanSpaced(new RandRange(3, 5), new IntRange(0, 30)), appleZoneSpawns);//apple
            floorSegment.ZoneSteps.Add(appleZoneStep);
            SpawnList<IGenPriority> bigAppleZoneSpawns = new SpawnList<IGenPriority>();
            bigAppleZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(2))))), 10);
            SpreadStepZoneStep bigAppleZoneStep = new SpreadStepZoneStep(new SpreadPlanSpaced(new RandRange(13, 15), new IntRange(0, 30)), bigAppleZoneSpawns);//big apple
            floorSegment.ZoneSteps.Add(bigAppleZoneStep);
            SpawnList<IGenPriority> leppaZoneSpawns = new SpawnList<IGenPriority>();
            leppaZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(11))))), 10);
            SpreadStepZoneStep leppaZoneStep = new SpreadStepZoneStep(new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, 30)), leppaZoneSpawns);//leppa
            floorSegment.ZoneSteps.Add(leppaZoneStep);
            SpawnList<IGenPriority> joySeedZoneSpawns = new SpawnList<IGenPriority>();
            joySeedZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(102))))), 10);
            SpreadStepZoneStep joySeedZoneStep = new SpreadStepZoneStep(new SpreadPlanSpaced(new RandRange(12, 30), new IntRange(0, 30)), joySeedZoneSpawns);//joy seed
            floorSegment.ZoneSteps.Add(joySeedZoneStep);
            SpawnList<IGenPriority> keyZoneSpawns = new SpawnList<IGenPriority>();
            keyZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(455, 1))))), 10);
            SpreadStepZoneStep keyZoneStep = new SpreadStepZoneStep(new SpreadPlanQuota(new RandRange(1), new IntRange(0, 5)), keyZoneSpawns);//key
            floorSegment.ZoneSteps.Add(keyZoneStep);
            SpawnList<IGenPriority> assemblyZoneSpawns = new SpawnList<IGenPriority>();
            assemblyZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(451))))), 10);
            SpreadStepZoneStep assemblyZoneStep = new SpreadStepZoneStep(new SpreadPlanSpaced(new RandRange(3, 7), new IntRange(4, 30)), assemblyZoneSpawns);//assembly box
            floorSegment.ZoneSteps.Add(assemblyZoneStep);
            SpawnList<IGenPriority> apricornZoneSpawns = new SpawnList<IGenPriority>();
            apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(211))))), 10);//blue apricorns
            apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(212))))), 10);//green apricorns
            apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(213))))), 10);//brown apricorns
            apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(214))))), 10);//purple apricorns
            apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(215))))), 10);//red apricorns
            apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(216))))), 10);//white apricorns
            apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(217))))), 10);//yellow apricorns
            apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(218))))), 10);//black apricorns
            SpreadStepZoneStep apricornZoneStep = new SpreadStepZoneStep(new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(1, 21)), apricornZoneSpawns);//apricorn (variety)
            floorSegment.ZoneSteps.Add(apricornZoneStep);

            SpawnList<IGenPriority> cleanseZoneSpawns = new SpawnList<IGenPriority>();
            cleanseZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(263))))), 10);
            SpreadStepZoneStep cleanseZoneStep = new SpreadStepZoneStep(new SpreadPlanQuota(new RandDecay(1, 10, 50), new IntRange(3, 30)), cleanseZoneSpawns);//cleanse orb
            floorSegment.ZoneSteps.Add(cleanseZoneStep);

            SpawnList<IGenPriority> evoZoneSpawns = new SpawnList<IGenPriority>();
            evoZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(351))))), 10);//Fire Stone
            evoZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(352))))), 10);//Thunder Stone
            evoZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(353))))), 10);//Water Stone
            evoZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(355))))), 10);//Moon Stone
            evoZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(363))))), 10);//Dusk Stone
            evoZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(364))))), 10);//Dawn Stone
            evoZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(362))))), 10);//Shiny Stone
            evoZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(354))))), 10);//Leaf Stone
            evoZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(379))))), 10);//Ice Stone
            evoZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(377))))), 10);//Sun Ribbon
            evoZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(378))))), 10);//Moon Ribbon
            SpreadStepZoneStep evoItemZoneStep = new SpreadStepZoneStep(new SpreadPlanQuota(new RandRange(2, 4), new IntRange(0, 15)), evoZoneSpawns);//evo items
            floorSegment.ZoneSteps.Add(evoItemZoneStep);


            SpreadRoomZoneStep evoZoneStep = new SpreadRoomZoneStep(PR_GRID_GEN_EXTRA, PR_ROOMS_GEN_EXTRA, new SpreadPlanSpaced(new RandRange(2, 5), new IntRange(3, 30)));
            List<BaseRoomFilter> evoFilters = new List<BaseRoomFilter>();
            evoFilters.Add(new RoomFilterComponent(true, new ImmutableRoom()));
            evoFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
            evoZoneStep.Spawns.Add(new RoomGenOption(new RoomGenEvo<MapGenContext>(), new RoomGenEvo<ListMapGenContext>(), evoFilters), 10);
            floorSegment.ZoneSteps.Add(evoZoneStep);


            int[] dexMap = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 152, 153, 154, 155, 156, 157, 158, 159, 160,
                                      172, 25, 26, 161, 162, 427, 428,
                                      16, 17, 18, 173, 35, 36, 265, 266, 267, 268, 269, 175, 176,
                                      468, 287, 288, 289, 403, 404, 405, 420, 421, 290, 291, 292,
                                      406, 315, 407, 433, 358, 133, 134, 135, 136, 196, 197, 470,
                                      471, 700, 228, 229, 447, 448, 90, 91, 102, 103, 190, 424,
                                      46, 47, 456, 457, 280, 281, 282, 475, 353, 354, 261,
                                      262, 220, 221, 473, 108, 463, 352, 417, 56, 57, 52, 53,
                                      316, 317, 455, 123, 212, 37, 38, 127, 96, 97, 438, 185,
                                      441, 439, 122, 236, 106, 107, 237, 328, 329, 330,
                                      446, 143, 246, 247, 248, 100, 101, 436, 437, 296, 297,
                                      104, 105, 198, 430, 343, 344, 129, 130, 371, 372, 373,
                                      231, 232, 359, 167, 168, 177, 178, 206, 322, 323,
                                      194, 195, 95, 208, 238, 124, 132, 200, 429, 225, 360, 202,
                                      299, 476, 218, 219, 109, 110, 303, 174, 39, 40 };

            {
                //monster houses
                SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(10, new IntRange(4, 15)));
                monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHouseStep<ListMapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                for (int ii = 0; ii < 18; ii++)
                    monsterChanceZoneStep.Items.Add(new MapItem(76 + ii), new IntRange(0, 30), 4);//gummis
                for (int ii = 0; ii < 8; ii++)
                    monsterChanceZoneStep.Items.Add(new MapItem(210 + ii), new IntRange(0, 30), 4);//apricorns
                monsterChanceZoneStep.Items.Add(new MapItem(351), new IntRange(0, 30), 4);//Fire Stone
                monsterChanceZoneStep.Items.Add(new MapItem(352), new IntRange(0, 30), 4);//Thunder Stone
                monsterChanceZoneStep.Items.Add(new MapItem(353), new IntRange(0, 30), 4);//Water Stone
                monsterChanceZoneStep.Items.Add(new MapItem(355), new IntRange(0, 30), 4);//Moon Stone
                monsterChanceZoneStep.Items.Add(new MapItem(363), new IntRange(0, 30), 4);//Dusk Stone
                monsterChanceZoneStep.Items.Add(new MapItem(364), new IntRange(0, 30), 4);//Dawn Stone
                monsterChanceZoneStep.Items.Add(new MapItem(362), new IntRange(0, 30), 4);//Shiny Stone
                monsterChanceZoneStep.Items.Add(new MapItem(354), new IntRange(0, 30), 4);//Leaf Stone
                monsterChanceZoneStep.Items.Add(new MapItem(379), new IntRange(0, 30), 4);//Ice Stone
                monsterChanceZoneStep.Items.Add(new MapItem(377), new IntRange(0, 30), 4);//Sun Ribbon
                monsterChanceZoneStep.Items.Add(new MapItem(378), new IntRange(0, 30), 4);//Moon Ribbon
                monsterChanceZoneStep.Items.Add(new MapItem(6), new IntRange(0, 30), 25);//banana
                for (int ii = 587; ii < 700; ii++)
                    monsterChanceZoneStep.Items.Add(new MapItem(ii), new IntRange(0, 30), 2);//TMs
                monsterChanceZoneStep.Items.Add(new MapItem(477), new IntRange(0, 30), 10);//nugget
                monsterChanceZoneStep.Items.Add(new MapItem(480, 1), new IntRange(0, 30), 10);//pearl
                monsterChanceZoneStep.Items.Add(new MapItem(481, 2), new IntRange(0, 30), 10);//heart scale
                monsterChanceZoneStep.Items.Add(new MapItem(455, 1), new IntRange(0, 30), 10);//key
                monsterChanceZoneStep.Items.Add(new MapItem(450), new IntRange(0, 30), 10);//link box
                monsterChanceZoneStep.Items.Add(new MapItem(451), new IntRange(0, 30), 10);//assembly box
                monsterChanceZoneStep.Items.Add(new MapItem(453), new IntRange(0, 30), 10);//ability capsule
                monsterChanceZoneStep.Items.Add(new MapItem(300), new IntRange(0, 30), 10);//friend bow

                //monsterChanceZoneStep.ItemThemes.Add(new ItemThemeNone(0, new RandRange(5, 11)), new ParamRange(0, 30), 20);
                monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(1, 4)), new ItemThemeNone(50, new RandRange(2, 4))), new IntRange(0, 30), 20);//no theme
                                                                                                                                                                                                                         //monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMoney(500, new ParamRange(5, 11)), new ParamRange(0, 30));
                monsterChanceZoneStep.ItemThemes.Add(new ItemThemeType(ItemData.UseType.Learn, true, true, new RandRange(3, 5)), new IntRange(0, 30), 10);//TMs
                monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(3, 7)), new IntRange(0, 30), 30);//gummis
                monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(RecruitState)), true, true, new RandRange(2, 6)), new IntRange(0, 30), 10);//apricorns
                monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(1, 4)), new ItemThemeRange(new IntRange(351, 380), true, true, new RandRange(2, 4))), new IntRange(0, 10), 40);//evo items
                monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(1, 4)), new ItemThemeRange(new IntRange(351, 380), true, true, new RandRange(2, 4))), new IntRange(10, 20), 20);//evo items
                for (int ii = 0; ii < dexMap.Length; ii++)
                    monsterChanceZoneStep.Mobs.Add(GetHouseMob(dexMap[ii], 17), new IntRange(0, 30), 10);//all monsters in the game
                monsterChanceZoneStep.MobThemes.Add(new MobThemeNone(50, new RandRange(7, 13)), new IntRange(19, 30), 10);
                monsterChanceZoneStep.MobThemes.Add(new MobThemeTypingSeeded(EvoFlag.FirstEvo | EvoFlag.NoEvo, new RandRange(7, 13)), new IntRange(0, 10), 10);
                monsterChanceZoneStep.MobThemes.Add(new MobThemeTypingSeeded(EvoFlag.FirstEvo | EvoFlag.NoEvo | EvoFlag.MidEvo, new RandRange(7, 13)), new IntRange(10, 20), 10);
                monsterChanceZoneStep.MobThemes.Add(new MobThemeTypingSeeded(EvoFlag.All, new RandRange(7, 13)), new IntRange(12, 22), 10);
                monsterChanceZoneStep.MobThemes.Add(new MobThemeTypingSeeded(EvoFlag.FinalEvo | EvoFlag.NoEvo | EvoFlag.MidEvo, new RandRange(7, 13)), new IntRange(20, 30), 10);
                floorSegment.ZoneSteps.Add(monsterChanceZoneStep);
            }

            {
                //monster halls
                SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(10, new IntRange(15, 30)));
                monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHallStep<ListMapGenContext>(new Loc(11, 9), GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                for (int ii = 0; ii < 18; ii++)
                    monsterChanceZoneStep.Items.Add(new MapItem(76 + ii), new IntRange(0, 30), 4);//gummis
                for (int ii = 0; ii < 8; ii++)
                    monsterChanceZoneStep.Items.Add(new MapItem(210 + ii), new IntRange(0, 30), 4);//apricorns
                monsterChanceZoneStep.Items.Add(new MapItem(351), new IntRange(0, 30), 4);//Fire Stone
                monsterChanceZoneStep.Items.Add(new MapItem(352), new IntRange(0, 30), 4);//Thunder Stone
                monsterChanceZoneStep.Items.Add(new MapItem(353), new IntRange(0, 30), 4);//Water Stone
                monsterChanceZoneStep.Items.Add(new MapItem(355), new IntRange(0, 30), 4);//Moon Stone
                monsterChanceZoneStep.Items.Add(new MapItem(363), new IntRange(0, 30), 4);//Dusk Stone
                monsterChanceZoneStep.Items.Add(new MapItem(364), new IntRange(0, 30), 4);//Dawn Stone
                monsterChanceZoneStep.Items.Add(new MapItem(362), new IntRange(0, 30), 4);//Shiny Stone
                monsterChanceZoneStep.Items.Add(new MapItem(354), new IntRange(0, 30), 4);//Leaf Stone
                monsterChanceZoneStep.Items.Add(new MapItem(379), new IntRange(0, 30), 4);//Ice Stone
                monsterChanceZoneStep.Items.Add(new MapItem(377), new IntRange(0, 30), 4);//Sun Ribbon
                monsterChanceZoneStep.Items.Add(new MapItem(378), new IntRange(0, 30), 4);//Moon Ribbon
                monsterChanceZoneStep.Items.Add(new MapItem(6), new IntRange(0, 30), 25);//banana
                for (int ii = 587; ii < 700; ii++)
                    monsterChanceZoneStep.Items.Add(new MapItem(ii), new IntRange(0, 30), 2);//TMs
                monsterChanceZoneStep.Items.Add(new MapItem(477), new IntRange(0, 30), 10);//nugget
                monsterChanceZoneStep.Items.Add(new MapItem(480, 1), new IntRange(0, 30), 10);//pearl
                monsterChanceZoneStep.Items.Add(new MapItem(481, 2), new IntRange(0, 30), 10);//heart scale
                monsterChanceZoneStep.Items.Add(new MapItem(455, 1), new IntRange(0, 30), 10);//key
                monsterChanceZoneStep.Items.Add(new MapItem(450), new IntRange(0, 30), 10);//link box
                monsterChanceZoneStep.Items.Add(new MapItem(451), new IntRange(0, 30), 10);//assembly box
                monsterChanceZoneStep.Items.Add(new MapItem(453), new IntRange(0, 30), 10);//ability capsule
                monsterChanceZoneStep.Items.Add(new MapItem(300), new IntRange(0, 30), 10);//friend bow

                //monsterChanceZoneStep.ItemThemes.Add(new ItemThemeNone(0, new RandRange(5, 11)), new ParamRange(0, 30), 20);
                monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(1, 4)), new ItemThemeNone(50, new RandRange(2, 4))), new IntRange(0, 30), 20);//no theme
                                                                                                                                                                                                                         //monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMoney(500, new ParamRange(5, 11)), new ParamRange(0, 30));
                monsterChanceZoneStep.ItemThemes.Add(new ItemThemeType(ItemData.UseType.Learn, true, true, new RandRange(3, 5)), new IntRange(0, 30), 10);//TMs
                monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(3, 7)), new IntRange(0, 30), 30);//gummis
                monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(RecruitState)), true, true, new RandRange(2, 6)), new IntRange(0, 30), 10);//apricorns
                monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(1, 4)), new ItemThemeRange(new IntRange(351, 380), true, true, new RandRange(2, 4))), new IntRange(0, 10), 40);//evo items
                monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(1, 4)), new ItemThemeRange(new IntRange(351, 380), true, true, new RandRange(2, 4))), new IntRange(10, 20), 20);//evo items
                for (int ii = 0; ii < dexMap.Length; ii++)
                    monsterChanceZoneStep.Mobs.Add(GetHouseMob(dexMap[ii], 17), new IntRange(0, 30), 10);//all monsters in the game
                monsterChanceZoneStep.MobThemes.Add(new MobThemeNone(50, new RandRange(7, 13)), new IntRange(19, 30), 10);
                monsterChanceZoneStep.MobThemes.Add(new MobThemeTypingSeeded(EvoFlag.FirstEvo | EvoFlag.NoEvo, new RandRange(7, 13)), new IntRange(0, 10), 10);
                monsterChanceZoneStep.MobThemes.Add(new MobThemeTypingSeeded(EvoFlag.FirstEvo | EvoFlag.NoEvo | EvoFlag.MidEvo, new RandRange(7, 13)), new IntRange(10, 20), 10);
                monsterChanceZoneStep.MobThemes.Add(new MobThemeTypingSeeded(EvoFlag.All, new RandRange(7, 13)), new IntRange(12, 22), 10);
                monsterChanceZoneStep.MobThemes.Add(new MobThemeTypingSeeded(EvoFlag.FinalEvo | EvoFlag.NoEvo | EvoFlag.MidEvo, new RandRange(7, 13)), new IntRange(20, 30), 10);
                floorSegment.ZoneSteps.Add(monsterChanceZoneStep);
            }

            {
                SpreadHouseZoneStep chestChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanQuota(new RandRange(2, 5), new IntRange(6, 30)));
                chestChanceZoneStep.ModStates.Add(new FlagType(typeof(ChestModGenState)));
                chestChanceZoneStep.HouseStepSpawns.Add(new ChestStep<ListMapGenContext>(false, GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                for (int ii = 0; ii < 18; ii++)
                    chestChanceZoneStep.Items.Add(new MapItem(76 + ii), new IntRange(0, 30), 4);//gummis
                chestChanceZoneStep.Items.Add(new MapItem(209), new IntRange(0, 30), 20);//big apricorn
                chestChanceZoneStep.Items.Add(new MapItem(164), new IntRange(0, 30), 80);//elixir
                chestChanceZoneStep.Items.Add(new MapItem(166), new IntRange(0, 30), 20);//max elixir
                chestChanceZoneStep.Items.Add(new MapItem(160), new IntRange(0, 30), 20);//potion
                chestChanceZoneStep.Items.Add(new MapItem(161), new IntRange(0, 30), 20);//max potion
                chestChanceZoneStep.Items.Add(new MapItem(173), new IntRange(0, 30), 20);//full heal
                for (int ii = 175; ii <= 181; ii++)
                    chestChanceZoneStep.Items.Add(new MapItem(ii), new IntRange(0, 30), 15);//X-Items
                for (int ii = 587; ii < 700; ii++)
                    chestChanceZoneStep.Items.Add(new MapItem(ii), new IntRange(0, 30), 2);//TMs
                chestChanceZoneStep.Items.Add(new MapItem(477), new IntRange(0, 30), 20);//nugget
                chestChanceZoneStep.Items.Add(new MapItem(481, 3), new IntRange(0, 30), 10);//heart scale
                chestChanceZoneStep.Items.Add(new MapItem(158, 1), new IntRange(0, 30), 20);//amber tear
                chestChanceZoneStep.Items.Add(new MapItem(206, 3), new IntRange(0, 30), 20);//rare fossil
                chestChanceZoneStep.Items.Add(new MapItem(101), new IntRange(0, 30), 20);//reviver seed
                chestChanceZoneStep.Items.Add(new MapItem(102), new IntRange(0, 30), 20);//joy seed
                chestChanceZoneStep.Items.Add(new MapItem(450), new IntRange(0, 30), 10);//link box
                chestChanceZoneStep.Items.Add(new MapItem(451), new IntRange(0, 30), 10);//assembly box
                chestChanceZoneStep.Items.Add(new MapItem(453), new IntRange(0, 30), 10);//ability capsule
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeNone(0, new RandRange(2, 5)), new IntRange(0, 30), 30);
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeRange(new IntRange(101), true, true, new RandRange(2, 4)), new IntRange(0, 30), 10);//reviver seed
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeRange(new IntRange(102), true, true, new RandRange(1, 4)), new IntRange(0, 30), 10);//joy seed
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeRange(new IntRange(158, 182), true, true, new RandRange(1, 3)), new IntRange(0, 30), 100);//manmade items
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeRange(new IntRange(300, 349), true, true, new RandRange(1, 3)), new IntRange(0, 30), 20);//equip
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeRange(new IntRange(380, 398), true, true, new RandRange(1, 3)), new IntRange(0, 30), 10);//plates
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeType(ItemData.UseType.Learn, true, true, new RandRange(1, 3)), new IntRange(0, 30), 10);//TMs
                chestChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(2, 5)), new IntRange(0, 30), 20);
                chestChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(RecruitState)), true, true, new RandRange(1)), new IntRange(0, 30), 10);

                floorSegment.ZoneSteps.Add(chestChanceZoneStep);
            }


            {
                SpreadHouseZoneStep chestChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanQuota(new RandDecay(1, 5, 50), new IntRange(6, 30)));
                chestChanceZoneStep.ModStates.Add(new FlagType(typeof(ChestModGenState)));
                chestChanceZoneStep.HouseStepSpawns.Add(new ChestStep<ListMapGenContext>(true, GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                for (int ii = 0; ii < 18; ii++)
                    chestChanceZoneStep.Items.Add(new MapItem(76 + ii), new IntRange(0, 30), 4);//gummis
                for (int ii = 0; ii < 8; ii++)
                    chestChanceZoneStep.Items.Add(new MapItem(210 + ii), new IntRange(0, 30), 4);//apricorns
                chestChanceZoneStep.Items.Add(new MapItem(209), new IntRange(0, 30), 10);//big apricorn
                chestChanceZoneStep.Items.Add(new MapItem(164), new IntRange(0, 30), 80);//elixir
                chestChanceZoneStep.Items.Add(new MapItem(166), new IntRange(0, 30), 20);//max elixir
                chestChanceZoneStep.Items.Add(new MapItem(160), new IntRange(0, 30), 20);//potion
                chestChanceZoneStep.Items.Add(new MapItem(161), new IntRange(0, 30), 20);//max potion
                chestChanceZoneStep.Items.Add(new MapItem(173), new IntRange(0, 30), 20);//full heal
                for (int ii = 175; ii <= 181; ii++)
                    chestChanceZoneStep.Items.Add(new MapItem(ii), new IntRange(0, 30), 15);//X-Items
                for (int ii = 587; ii < 700; ii++)
                    chestChanceZoneStep.Items.Add(new MapItem(ii), new IntRange(0, 30), 2);//TMs
                chestChanceZoneStep.Items.Add(new MapItem(477), new IntRange(0, 30), 20);//nugget
                chestChanceZoneStep.Items.Add(new MapItem(480, 3), new IntRange(0, 30), 5);//pearl
                chestChanceZoneStep.Items.Add(new MapItem(481, 3), new IntRange(0, 30), 10);//heart scale
                chestChanceZoneStep.Items.Add(new MapItem(158, 1), new IntRange(0, 30), 200);//amber tear
                chestChanceZoneStep.Items.Add(new MapItem(206, 3), new IntRange(0, 30), 20);//rare fossil
                chestChanceZoneStep.Items.Add(new MapItem(101), new IntRange(0, 30), 20);//reviver seed
                chestChanceZoneStep.Items.Add(new MapItem(102), new IntRange(0, 30), 20);//joy seed
                chestChanceZoneStep.Items.Add(new MapItem(103), new IntRange(0, 30), 20);//golden seed
                chestChanceZoneStep.Items.Add(new MapItem(450), new IntRange(0, 30), 10);//link box
                chestChanceZoneStep.Items.Add(new MapItem(451), new IntRange(0, 30), 10);//assembly box
                chestChanceZoneStep.Items.Add(new MapItem(453), new IntRange(0, 30), 10);//ability capsule
                chestChanceZoneStep.Items.Add(new MapItem(349), new IntRange(0, 30), 10);//harmony scarf
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(2, 5)), new ItemThemeNone(50, new RandRange(4, 7))), new IntRange(0, 30), 30);
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(2, 5)), new ItemThemeRange(new IntRange(101), true, true, new RandRange(3, 5))), new IntRange(0, 30), 10);//reviver seed
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(2, 5)), new ItemThemeRange(new IntRange(102), true, true, new RandRange(2, 5))), new IntRange(0, 30), 10);//joy seed
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(2, 5)), new ItemThemeRange(new IntRange(103), true, true, new RandRange(1))), new IntRange(0, 30), 10);//golden seed
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(2, 5)), new ItemThemeRange(new IntRange(349), true, true, new RandRange(1))), new IntRange(20, 30), 20);//Harmony Scarf
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(2, 5)), new ItemThemeRange(new IntRange(158, 182), true, true, new RandRange(3, 6))), new IntRange(0, 30), 20);//manmade items
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(2, 5)), new ItemThemeRange(new IntRange(300, 349), true, true, new RandRange(3, 6))), new IntRange(0, 30), 20);//equip
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(2, 5)), new ItemThemeRange(new IntRange(380, 398), true, true, new RandRange(3, 6))), new IntRange(0, 30), 10);//plates
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(2, 5)), new ItemThemeType(ItemData.UseType.Learn, true, true, new RandRange(3, 6))), new IntRange(0, 30), 20);//TMs
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(2, 5)), new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(4, 9))), new IntRange(0, 30), 10);//gummis
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(2, 5)), new ItemStateType(new FlagType(typeof(RecruitState)), true, true, new RandRange(3, 7))), new IntRange(0, 30), 10);//apricorns

                for (int ii = 0; ii < dexMap.Length; ii++)
                    chestChanceZoneStep.Mobs.Add(GetHouseMob(dexMap[ii], 17), new IntRange(0, 30), 10);//all monsters in the game
                chestChanceZoneStep.MobThemes.Add(new MobThemeTypingSeeded(EvoFlag.FirstEvo | EvoFlag.NoEvo, new RandRange(7, 13)), new IntRange(0, 10), 10);
                chestChanceZoneStep.MobThemes.Add(new MobThemeTypingSeeded(EvoFlag.FirstEvo | EvoFlag.NoEvo | EvoFlag.MidEvo, new RandRange(7, 13)), new IntRange(10, 20), 10);
                chestChanceZoneStep.MobThemes.Add(new MobThemeTypingSeeded(EvoFlag.All, new RandRange(7, 13)), new IntRange(12, 22), 10);
                chestChanceZoneStep.MobThemes.Add(new MobThemeTypingSeeded(EvoFlag.FinalEvo | EvoFlag.NoEvo | EvoFlag.MidEvo, new RandRange(7, 13)), new IntRange(20, 30), 10);

                floorSegment.ZoneSteps.Add(chestChanceZoneStep);
            }


            //we need two types of vaults:
            //locked by key
            //locked by switch
            //can they overlap? only the types can overlap but we may need to make do with just that - need to add a new connection type
            //each has its own table of items and foes

            //key vaults
            //removed...

            //switch vaults
            {
                SpreadVaultZoneStep vaultChanceZoneStep = new SpreadVaultZoneStep(PR_SPAWN_ITEMS_EXTRA, PR_SPAWN_MOBS_EXTRA, new SpreadPlanQuota(new RandRange(1, 4), new IntRange(0, 30)));

                //making room for the vault
                {
                    ResizeFloorStep<ListMapGenContext> vaultStep = new ResizeFloorStep<ListMapGenContext>(new Loc(8, 8), Dir8.DownRight, Dir8.UpLeft);
                    vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT, vaultStep));
                }
                {
                    ClampFloorStep<ListMapGenContext> vaultStep = new ClampFloorStep<ListMapGenContext>(new Loc(0), new Loc(78, 54));
                    vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT_CLAMP, vaultStep));
                }

                // room addition step
                {
                    SpawnList<RoomGen<ListMapGenContext>> detourRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    detourRooms.Add(new RoomGenCross<ListMapGenContext>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
                    SpawnList<PermissiveRoomGen<ListMapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                    detourHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                    AddConnectedRoomsStep<ListMapGenContext> detours = new AddConnectedRoomsStep<ListMapGenContext>(detourRooms, detourHalls);
                    detours.Amount = new RandRange(8, 10);
                    detours.HallPercent = 100;
                    detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom()));
                    detours.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.SwitchVault));
                    detours.RoomComponents.Set(new NoConnectRoom());
                    detours.RoomComponents.Set(new NoEventRoom());
                    detours.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.SwitchVault));
                    detours.HallComponents.Set(new NoConnectRoom());
                    detours.RoomComponents.Set(new NoEventRoom());

                    vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_GEN_EXTRA, detours));
                }

                //sealing the vault
                {
                    SwitchSealStep<ListMapGenContext> vaultStep = new SwitchSealStep<ListMapGenContext>(40, 41, true);
                    vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                    vaultStep.SwitchFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                    vaultStep.SwitchFilters.Add(new RoomFilterComponent(true, new BossRoom()));
                    vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_TILES_GEN_EXTRA, vaultStep));
                }

                //items for the vault
                {
                    vaultChanceZoneStep.Items.Add(new MapItem(164), new IntRange(0, 30), 800);//elixir
                    vaultChanceZoneStep.Items.Add(new MapItem(166), new IntRange(0, 30), 100);//max elixir
                    vaultChanceZoneStep.Items.Add(new MapItem(160), new IntRange(0, 30), 200);//potion
                    vaultChanceZoneStep.Items.Add(new MapItem(161), new IntRange(0, 30), 100);//max potion
                    vaultChanceZoneStep.Items.Add(new MapItem(173), new IntRange(0, 30), 200);//full heal
                    for (int ii = 175; ii <= 181; ii++)
                        vaultChanceZoneStep.Items.Add(new MapItem(ii), new IntRange(0, 30), 50);//X-Items
                    for (int ii = 0; ii < 18; ii++)
                        vaultChanceZoneStep.Items.Add(new MapItem(76 + ii), new IntRange(0, 30), 200);//gummis
                    vaultChanceZoneStep.Items.Add(new MapItem(158, 1), new IntRange(0, 30), 2000);//amber tear
                    vaultChanceZoneStep.Items.Add(new MapItem(101), new IntRange(0, 30), 200);//reviver seed
                    vaultChanceZoneStep.Items.Add(new MapItem(102), new IntRange(0, 30), 200);//joy seed
                    vaultChanceZoneStep.Items.Add(new MapItem(453), new IntRange(0, 30), 200);//ability capsule
                    vaultChanceZoneStep.Items.Add(new MapItem(349), new IntRange(0, 30), 100);//harmony scarf
                    vaultChanceZoneStep.Items.Add(new MapItem(285), new IntRange(15, 30), 50);//itemizer orb
                    vaultChanceZoneStep.Items.Add(new MapItem(235, 3), new IntRange(15, 30), 50);//transfer wand
                    vaultChanceZoneStep.Items.Add(new MapItem(455, 1), new IntRange(0, 30), 1000);//key
                }

                // item spawnings for the vault
                for (int ii = 0; ii < 30; ii++)
                {
                    //add a PickerSpawner <- PresetMultiRand <- coins
                    List<MapItem> treasures = new List<MapItem>();
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(false, 152));
                    PickerSpawner<ListMapGenContext, MapItem> treasurePicker = new PickerSpawner<ListMapGenContext, MapItem>(new PresetMultiRand<MapItem>(treasures));


                    SpawnList<IStepSpawner<ListMapGenContext, MapItem>> boxSpawn = new SpawnList<IStepSpawner<ListMapGenContext, MapItem>>();

                    //444      ***    Light Box - 1* items
                    {
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>(444, new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(1), new RandRange(1))), 10);
                    }

                    //445      ***    Heavy Box - 2* items
                    {
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>(445, new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(2), new RandRange(1))), 10);
                    }

                    ////446      ***    Nifty Box - all high tier TMs, ability capsule, heart scale 9, max potion, full heal, max elixir
                    //{
                    //    SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                    //    for (int nn = 588; nn < 700; nn++)
                    //        boxTreasure.Add(new MapItem(nn), 1);//TMs
                    //    boxTreasure.Add(new MapItem(453), 100);//ability capsule
                    //    boxTreasure.Add(new MapItem(481), 100);//heart scale
                    //    boxTreasure.Add(new MapItem(160), 60);//potion
                    //    boxTreasure.Add(new MapItem(161), 30);//max potion
                    //    boxTreasure.Add(new MapItem(173), 100);//full heal
                    //    boxTreasure.Add(new MapItem(164), 60);//elixir
                    //    boxTreasure.Add(new MapItem(166), 30);//max elixir
                    //    boxSpawn.Add(new BoxSpawner<ListMapGenContext>(446, new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 10);
                    //}

                    //447      ***    Dainty Box - Stat ups, wonder gummi, nectar, golden apple, golden banana
                    {
                        SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                        for (int nn = 0; nn < 18; nn++)//Gummi
                            boxTreasure.Add(new MapItem(76 + nn), 1);

                        boxTreasure.Add(new MapItem(151), 2);//protein
                        boxTreasure.Add(new MapItem(152), 2);//iron
                        boxTreasure.Add(new MapItem(153), 2);//calcium
                        boxTreasure.Add(new MapItem(154), 2);//zinc
                        boxTreasure.Add(new MapItem(155), 2);//carbos
                        boxTreasure.Add(new MapItem(156), 2);//hp up
                        boxTreasure.Add(new MapItem(150), 2);//nectar

                        boxTreasure.Add(new MapItem(5), 10);//perfect apple
                        boxTreasure.Add(new MapItem(7), 10);//big banana
                        boxTreasure.Add(new MapItem(75), 4);//wonder gummi
                        boxTreasure.Add(new MapItem(102), 10);//joy seed
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>(447, new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 3);
                    }

                    //448    Glittery Box - golden apple, amber tear, golden banana, nugget, golden thorn 9
                    {
                        SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();
                        boxTreasure.Add(new MapItem(205), 10);//golden thorn
                        boxTreasure.Add(new MapItem(158), 10);//Amber Tear
                        boxTreasure.Add(new MapItem(477), 10);//nugget
                        boxTreasure.Add(new MapItem(103), 10);//golden seed
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>(448, new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 2);
                    }

                    //449      ***    Deluxe Box - Legendary exclusive items, harmony scarf, golden items, stat ups, wonder gummi, perfect apricorn, max potion/full heal/max elixir
                    //{
                    //    SpeciesItemListSpawner<ListMapGenContext> legendSpawner = new SpeciesItemListSpawner<ListMapGenContext>(new IntRange(1, 3), new RandRange(1));
                    //    legendSpawner.Species.Add(144);
                    //    legendSpawner.Species.Add(145);
                    //    legendSpawner.Species.Add(146);
                    //    legendSpawner.Species.Add(150);
                    //    boxSpawn.Add(new BoxSpawner<ListMapGenContext>(449, legendSpawner), 5);
                    //}
                    //{
                    //    SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                    //    for (int nn = 0; nn < 18; nn++)//Gummi
                    //        boxTreasure.Add(new MapItem(76 + nn), 1);

                    //    boxTreasure.Add(new MapItem(4), 10);//golden apple
                    //    boxTreasure.Add(new MapItem(8), 10);//gold banana
                    //    boxTreasure.Add(new MapItem(158), 10);//Amber Tear
                    //    boxTreasure.Add(new MapItem(477), 10);//nugget
                    //    boxTreasure.Add(new MapItem(103), 10);//golden seed
                    //    boxTreasure.Add(new MapItem(161), 30);//max potion
                    //    boxTreasure.Add(new MapItem(173), 100);//full heal
                    //    boxTreasure.Add(new MapItem(166), 30);//max elixir
                    //    boxTreasure.Add(new MapItem(75), 4);//wonder gummi
                    //    boxTreasure.Add(new MapItem(349), 1);//harmony scarf
                    //    boxTreasure.Add(new MapItem(219), 1);//perfect apricorn
                    //    boxSpawn.Add(new BoxSpawner<ListMapGenContext>(449, new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 10);
                    //}

                    MultiStepSpawner<ListMapGenContext, MapItem> boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(1)));

                    //MultiStepSpawner <- PresetMultiRand
                    MultiStepSpawner<ListMapGenContext, MapItem> mainSpawner = new MultiStepSpawner<ListMapGenContext, MapItem>();
                    mainSpawner.Picker = new PresetMultiRand<IStepSpawner<ListMapGenContext, MapItem>>(treasurePicker, boxPicker);
                    vaultChanceZoneStep.ItemSpawners.SetRange(mainSpawner, new IntRange(0, 30));
                }
                vaultChanceZoneStep.ItemAmount.SetRange(new RandRange(5, 7), new IntRange(0, 30));

                // item placements for the vault
                {
                    RandomRoomSpawnStep<ListMapGenContext, MapItem> detourItems = new RandomRoomSpawnStep<ListMapGenContext, MapItem>();
                    detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                    vaultChanceZoneStep.ItemPlacements.SetRange(detourItems, new IntRange(0, 30));
                }

                // mobs
                // Vault FOES
                {
                    //37//470 Leafeon : 320 Grass Whistle : 076 Solar Beam* : 235 Synthesis : 241 Sunny Day
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(470, -1, 320, 76, 235, 241, 4), new IntRange(0, 10), 10);

                    //43//471 Glaceon : 59 Blizzard : 270 Helping Hand : 258 Hail
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(471, -1, 59, 270, 258, -1, 4), new IntRange(20, 30), 10);

                    //64//479 Rotom : 86 Thunder Wave : 271 Trick : 435 Discharge : 164 Substitute
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(479, -1, 86, 271, 435, 164, 4), new IntRange(0, 30), 10);

                    //234 !! Stantler : 43 Leer : 95 Hypnosis : 36 Take Down : 109 Confuse Ray
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(234, -1, 43, 95, 36, 109, 4), new IntRange(0, 10), 10);

                    //130 !! Gyarados : 153 Moxie : 242 Crunch : 82 Dragon Rage : 240 Rain Dance : 401 Aqua Tail
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(130, 153, 242, 82, 240, 401, 4), new IntRange(8, 15), 10);

                    //53//452 Drapion : 367 Acupressure : 398 Poison Jab : 424 Fire Fang : 565 Fell Stinger
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(452, -1, 367, 398, 424, 565, 4), new IntRange(5, 30), 10);

                    //24//262 Mightyena : 372 Assurance : 336 Howl
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(262, -1, 372, 336, -1, -1, 4), new IntRange(0, 30), 10);

                    //24//20 Raticate : 228 Pursuit : 162 Super Fang : 372 Assurance
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(20, -1, 228, 162, 372, -1, 4), new IntRange(0, 30), 10);

                    //29//137 Porygon : 160 Conversion : 060 Psybeam : 324 Signal Beam : 033 Tackle
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(137, -1, 160, 60, 324, 33, 4), new IntRange(5, 25), 10);

                    //50//233 Porygon2 : 176 Conversion 2 : 105 Recover : 161 Tri Attack : 111 Defense Curl
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(233, -1, 176, 105, 161, 111, 4), new IntRange(10, 30), 5);

                    //40//474 Porygon-Z : 097 Agility : 433 Trick Room : 435 Discharge
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(474, -1, 97, 433, 435, -1, 4), new IntRange(10, 30), 5);

                    //67//474 Porygon-Z : 373 Embargo : 199 Lock-On : 063 Hyper Beam
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(474, -1, 373, 199, 63, -1, 4), new IntRange(10, 30), 5);

                    //115 Kangaskhan : 113 Scrappy : 146 Dizzy Punch : 004 Comet Punch : 99 Rage : 203 Endure
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(115, 113, 146, 4, 99, 203, 4), new IntRange(0, 30), 5);

                    //40//224 Octillery : 021 Suction Cups : 097 Sniper : 058 Ice Beam : 060 Psybeam : 190 Octazooka
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(224, 97, 58, 60, 190, -1, 4), new IntRange(0, 15), 5);

                    //40//208 Steelix : 174 Curse : 360 Gyro Ball : 231 Iron Tail : Dig
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(208, -1, 174, 360, 231, 43, 4), new IntRange(10, 30), 5);

                }
                vaultChanceZoneStep.MobAmount.SetRange(new RandRange(7, 11), new IntRange(0, 30));

                // mob placements
                {
                    PlaceRandomMobsStep<ListMapGenContext> secretMobPlacement = new PlaceRandomMobsStep<ListMapGenContext>();
                    secretMobPlacement.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                    secretMobPlacement.ClumpFactor = 20;
                    vaultChanceZoneStep.MobPlacements.SetRange(secretMobPlacement, new IntRange(0, 30));
                }

                floorSegment.ZoneSteps.Add(vaultChanceZoneStep);
            }

            {
                SpreadBossZoneStep bossChanceZoneStep = new SpreadBossZoneStep(PR_ROOMS_GEN_EXTRA, PR_SPAWN_ITEMS_EXTRA, new SpreadPlanQuota(new RandDecay(0, 4, 50), new IntRange(2, 30)));

                {
                    ResizeFloorStep<ListMapGenContext> resizeStep = new ResizeFloorStep<ListMapGenContext>(new Loc(8, 8), Dir8.DownRight, Dir8.UpLeft);
                    bossChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT, resizeStep));
                }
                {
                    ClampFloorStep<ListMapGenContext> vaultStep = new ClampFloorStep<ListMapGenContext>(new Loc(0), new Loc(78, 54));
                    bossChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT_CLAMP, vaultStep));
                }

                //BOSS TEAMS
                // no specific items to be used in lv5 dungeons

                string[] customWaterCross = new string[] {    "~~~...~~~",
                                                                      "~~~...~~~",
                                                                      "~~X...X~~",
                                                                      ".........",
                                                                      ".........",
                                                                      ".........",
                                                                      "~~X...X~~",
                                                                      "~~~...~~~",
                                                                      "~~~...~~~"};
                // vespiquen and its underlings
                //416 Vespiquen : 454 Attack Order : 455 Heal Order : 456 Defend Order : 408 Power Gem
                //415 Combee    : 230 Sweet Scent : 366 Tailwind : 450 Bug Bite : 283 Endeavor
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    mobSpawns.Add(GetBossMob(416, -1, 454, 455, 456, 408, -1, new Loc(4, 1)));
                    mobSpawns.Add(GetBossMob(415, -1, 230, 366, 450, 283, -1, new Loc(4, 2)));
                    mobSpawns.Add(GetBossMob(415, -1, 230, 366, 450, 283, -1, new Loc(1, 4)));
                    mobSpawns.Add(GetBossMob(415, -1, 230, 366, 450, 283, -1, new Loc(7, 4)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customWaterCross, new Loc(4, 4), mobSpawns, false), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);

                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(0, 14), 10);
                }


                string[] customLavaLake = new string[] {      "X^^.^.^^X",
                                                                      "^^^^.^^^^",
                                                                      "^^^.^.^^^",
                                                                      ".^.....^.",
                                                                      "^.^...^.^",
                                                                      ".^.....^.",
                                                                      "^^^.^.^^^",
                                                                      "^^^^.^^^^",
                                                                      "X^^.^.^^X"};
                // lava plume synergy
                //323 Camerupt : 284 Eruption : 414 Earth Power : 436 Lava Plume : 281 Yawn
                //229 Houndoom : 53 Flamethrower : 46 Roar : 492 Foul Play : 517 Inferno
                //136 Flareon : 83 Fire Spin : 394 Flare Blitz : 98 Quick Attack : 436 Lava Plume
                //059 Arcanine : 257 Heat Wave : 555 Snarl : 245 Extreme Speed : 126 Fire Blast
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    mobSpawns.Add(GetBossMob(323, 116, 284, 414, 436, 281, -1, new Loc(4, 2)));
                    mobSpawns.Add(GetBossMob(229, 18, 53, 46, 492, 517, -1, new Loc(2, 4)));
                    mobSpawns.Add(GetBossMob(136, 18, 83, 394, 98, 436, -1, new Loc(4, 6)));
                    mobSpawns.Add(GetBossMob(59, 18, 257, 555, 245, 126, -1, new Loc(6, 4)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customLavaLake, new Loc(4, 4), mobSpawns, true), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);

                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(14, 19), 10);
                }



                string[] customJigsaw = new string[] {        "....XXX....",
                                                                      "XX..XXX..XX",
                                                                      "XX.......XX",
                                                                      "...........",
                                                                      "...........",
                                                                      "XX.......XX",
                                                                      "XX..XXX..XX",
                                                                      "....XXX...."};
                //    sand team
                //248 Tyranitar : 444 Stone Edge : 242 Crunch : 103 Screech : 269 Taunt
                //208 Steelix : 446 Stealth Rock : 231 Iron Tail : 91 Dig : 20 Bind
                //185 Sudowoodo : 359 Hammer Arm : 68 Counter : 335 Block : 452 Wood Hammer
                //232 Donphan : 484 Heavy Slam : 523 Bulldoze : 282 Knock Off : 205 Rollout
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    mobSpawns.Add(GetBossMob(248, -1, 444, 242, 103, 269, -1, new Loc(3, 1)));
                    mobSpawns.Add(GetBossMob(208, -1, 446, 231, 91, 20, -1, new Loc(7, 1)));
                    mobSpawns.Add(GetBossMob(185, -1, 359, 68, 335, 452, -1, new Loc(2, 3)));
                    mobSpawns.Add(GetBossMob(232, -1, 484, 523, 282, 205, -1, new Loc(8, 3)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customJigsaw, new Loc(5, 3), mobSpawns, true), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);

                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(12, 30), 10);
                }


                string[] customSkyChex = new string[] {       "..___..",
                                                                      "..___..",
                                                                      "__...__",
                                                                      "__...__",
                                                                      "__...__",
                                                                      "..___..",
                                                                      "..___.."};
                //    dragon team
                //149 Dragonite : 63 Hyper Beam : 200 Outrage : 525 Dragon Tail : 355 Roost
                //130 Gyarados : 89 Earthquake : 127 Waterfall : 349 Dragon Dance : 423 Ice Fang
                //142 Aerodactyl : 446 Stealth Rock : 97 Agility : 157 Rock Slide : 469 Wide Guard
                //006 Charizard : 519 Fire Pledge : 82 Dragon Rage : 314 Air Cutter : 257 Heat Wave
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    mobSpawns.Add(GetBossMob(149, -1, 63, 525, 200, 355, -1, new Loc(0, 0)));
                    mobSpawns.Add(GetBossMob(130, -1, 89, 127, 349, 423, -1, new Loc(6, 0)));
                    mobSpawns.Add(GetBossMob(142, -1, 446, 97, 157, 469, -1, new Loc(0, 6)));
                    mobSpawns.Add(GetBossMob(006, -1, 519, 82, 314, 257, -1, new Loc(6, 6)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customSkyChex, new Loc(3, 3), mobSpawns, true), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);

                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(23, 30), 10);
                }

                string[] customPillarHalls = new string[] {   ".........",
                                                                      "..X...X..",
                                                                      ".........",
                                                                      ".........",
                                                                      "..X...X..",
                                                                      ".........",
                                                                      ".........",
                                                                      "..X...X..",
                                                                      "........."};
                // dragon? team 2
                //373 Salamence : 434 Draco Meteor : 203 Endure : 82 Dragon Rage : 53 Flamethrower
                //160 Feraligatr : 276 Superpower : 401 Aqua Tail : 423 Ice Fang : 242 Crunch
                //475 Gallade : 370 Close Combat : 348 Leaf Blade : 427 Psycho Cut : 364 Feint
                //430 Honchkrow : 114 Haze : 355 Roost : 511 Quash : 399 Dark Pulse
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    mobSpawns.Add(GetBossMob(373, -1, 434, 203, 82, 53, -1, new Loc(4, 3)));
                    mobSpawns.Add(GetBossMob(160, -1, 276, 401, 423, 242, -1, new Loc(2, 3)));
                    mobSpawns.Add(GetBossMob(475, -1, 370, 348, 427, 364, -1, new Loc(6, 3)));
                    mobSpawns.Add(GetBossMob(430, -1, 114, 355, 511, 399, -1, new Loc(4, 1)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customPillarHalls, new Loc(4, 5), mobSpawns, true), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);

                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(19, 30), 10);
                }


                string[] customSkyDiamond = new string[] {    "XXX_._XXX",
                                                                      "XX__.__XX",
                                                                      "X___.___X",
                                                                      "____.____",
                                                                      ".........",
                                                                      "____.____",
                                                                      "X___.___X",
                                                                      "XX__.__XX",
                                                                      "XXX_._XXX"};
                //337 lunatone : 478 Magic Room : 94 Psychic : 322 Cosmic Power : 585 Moonblast
                //338 solrock : 377 Heal Block : 234 Morning Sun : 322 Cosmic Power : 76 Solar Beam
                //344 claydol : 286 Imprison : 471 Power Split : 153 Explosion : 326 Extrasensory
                //437 bronzong : 219 Safeguard : 319 Metal Sound : 248 Future Sight : 430 Flash Cannon
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    mobSpawns.Add(GetBossMob(337, -1, 478, 94, 322, 585, -1, new Loc(2, 2)));
                    mobSpawns.Add(GetBossMob(338, -1, 377, 234, 322, 76, -1, new Loc(6, 2)));
                    mobSpawns.Add(GetBossMob(344, -1, 286, 471, 153, 326, -1, new Loc(2, 6)));
                    mobSpawns.Add(GetBossMob(437, -1, 219, 319, 248, 430, -1, new Loc(6, 6)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customSkyDiamond, new Loc(4, 4), mobSpawns, true), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);

                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(23, 30), 10);
                }



                string[] customPillarCross = new string[] {   ".........",
                                                                      ".XX...XX.",
                                                                      ".XX...XX.",
                                                                      ".........",
                                                                      ".........",
                                                                      ".........",
                                                                      ".XX...XX.",
                                                                      ".XX...XX.",
                                                                      "........."};
                //   All ditto impostor
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    mobSpawns.Add(GetBossMob(132, 150, -1, -1, -1, -1, -1, new Loc(4, 1)));
                    mobSpawns.Add(GetBossMob(132, 150, -1, -1, -1, -1, -1, new Loc(4, 7)));
                    mobSpawns.Add(GetBossMob(132, 150, -1, -1, -1, -1, -1, new Loc(1, 4)));
                    mobSpawns.Add(GetBossMob(132, 150, -1, -1, -1, -1, -1, new Loc(7, 4)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customPillarCross, new Loc(4, 4), mobSpawns, false), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);

                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(0, 30), 10);
                }


                string[] customSideWalls = new string[] {     ".........",
                                                                      ".........",
                                                                      "..X...X..",
                                                                      "..X...X..",
                                                                      ".........",
                                                                      "..X...X..",
                                                                      "..X...X..",
                                                                      "........."};
                //   pink wall
                //36 Clefable : 98 Magic Guard : 266 Follow Me : 107 Minimize : 138 Dream Eater : 118 Metronome
                //35 Clefairy : 132 Friend Guard : 322 Cosmic Power : 381 Lucky Chant : 500 Stored Power : 236 Moonlight
                //39 Jigglypuff : 132 Friend Guard : 47 Sing : 50 Disable : 445 Captivate : 496 Round
                //440 Happiny : 132 Friend Guard : 204 Charm : 287 Refresh : 186 Sweet Kiss : 164 Substitute
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    mobSpawns.Add(GetBossMob(36, 98, 266, 107, 138, 118, -1, new Loc(4, 3)));
                    mobSpawns.Add(GetBossMob(35, 132, 236, 322, 381, 500, -1, new Loc(4, 2)));
                    mobSpawns.Add(GetBossMob(39, 132, 47, 50, 445, 496, -1, new Loc(3, 3)));
                    mobSpawns.Add(GetBossMob(440, 132, 204, 287, 186, 164, -1, new Loc(5, 3)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customSideWalls, new Loc(4, 4), mobSpawns, false), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);

                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(0, 14), 10);
                }


                string[] customSemiCovered = new string[] {   ".........",
                                                                      ".........",
                                                                      ".XX...XX.",
                                                                      ".........",
                                                                      ".........",
                                                                      "........."};
                //  Eeveelution 1
                // Vaporeon : Muddy Water : Aqua Ring : Aurora Beam : Helping Hand
                // Jolteon : Thunderbolt : Agility : Signal Beam : Stored Power
                // Flareon : Flare Blitz : Will-O-Wisp : Smog : Helping Hand
                // Sylveon : 585 Moonblast : 219 Safeguard : 247 Shadow Ball : Stored Power
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    mobSpawns.Add(GetBossMob(134, -1, 330, 392, 62, 270, -1, new Loc(4, 1)));
                    mobSpawns.Add(GetBossMob(135, -1, 85, 97, 324, 500, -1, new Loc(3, 1)));
                    mobSpawns.Add(GetBossMob(136, -1, 394, 261, 123, 270, -1, new Loc(5, 1)));
                    mobSpawns.Add(GetBossMob(700, -1, 585, 219, 247, 500, -1, new Loc(4, 0)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customSemiCovered, new Loc(4, 3), mobSpawns, false), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);

                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(0, 30), 10);
                }
                //  Eeveelution 2
                // Espeon : Psyshock : Morning Sun : Dazzling Gleam : Stored Power
                // Leafeon : Leaf Blade : Grass Whistle : X-Scissor : Helping Hand
                // Glaceon : Frost Breath : Barrier : Water Pulse : Stored Power
                // Umbreon : Snarl : Moonlight : Shadow Ball : Helping Hand
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    mobSpawns.Add(GetBossMob(196, -1, 473, 234, 605, 500, -1, new Loc(4, 1)));
                    mobSpawns.Add(GetBossMob(470, -1, 348, 320, 404, 270, -1, new Loc(3, 1)));
                    mobSpawns.Add(GetBossMob(471, -1, 524, 112, 352, 500, -1, new Loc(5, 1)));
                    mobSpawns.Add(GetBossMob(197, -1, 555, 236, 247, 270, -1, new Loc(4, 0)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customSemiCovered, new Loc(4, 3), mobSpawns, false), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);

                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(0, 30), 10);
                }


                string[] customTwoPillars = new string[] {    "...........",
                                                                      "..XX...XX..",
                                                                      "..XX...XX..",
                                                                      "...........",
                                                                      "..........."};
                //   Discharge + volt absorb
                //135 Jolteon : 10 Volt Absorb : 435 Discharge : 97 Agility : 113 Light Screen : 324 Signal Beam
                //417 Pachirisu : 10 Volt Absorb : 266 Follow Me : 162 Super Fang : 435 Discharge : 569 Ion Deluge
                //26 Raichu : 31 Lightning Rod : 447 Grass Knot : 85 Thunderbolt : 411 Focus Blast : 231 Iron Tail
                //101 Electrode : 106 Aftermath : 435 Discharge : 598 Eerie Impulse : 49 Sonic Boom : 268 Charge
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    mobSpawns.Add(GetBossMob(135, 10, 435, 97, 113, 324, -1, new Loc(4, 1)));
                    mobSpawns.Add(GetBossMob(417, 10, 266, 162, 435, 569, -1, new Loc(6, 1)));
                    mobSpawns.Add(GetBossMob(26, 31, 447, 85, 411, 231, -1, new Loc(2, 3)));
                    mobSpawns.Add(GetBossMob(101, 106, 435, 598, 49, 268, -1, new Loc(8, 3)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customTwoPillars, new Loc(5, 3), mobSpawns, false), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);

                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(0, 30), 10);
                }

                //   Fossil team with ancient power; omastar has shell smash

                //entry hazards/gradual grind
                //forretress

                // redirection synergy; gyarados+lightningrod?

                //   PP stall team

                //   something with draco meteor - altaria? - also has haze support
                //   maybe an overheat team with haze support


                //sealing the boss room and treasure room
                {
                    BossSealStep<ListMapGenContext> vaultStep = new BossSealStep<ListMapGenContext>(40, 38);
                    vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                    vaultStep.BossFilters.Add(new RoomFilterComponent(false, new BossRoom()));
                    bossChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_TILES_GEN_EXTRA, vaultStep));
                }


                //items for the vault
                {
                    bossChanceZoneStep.Items.Add(new MapItem(164), new IntRange(0, 30), 800);//elixir
                    bossChanceZoneStep.Items.Add(new MapItem(166), new IntRange(0, 30), 100);//max elixir
                    bossChanceZoneStep.Items.Add(new MapItem(160), new IntRange(0, 30), 200);//potion
                    bossChanceZoneStep.Items.Add(new MapItem(161), new IntRange(0, 30), 100);//max potion
                    bossChanceZoneStep.Items.Add(new MapItem(173), new IntRange(0, 30), 200);//full heal
                    for (int ii = 175; ii <= 181; ii++)
                        bossChanceZoneStep.Items.Add(new MapItem(ii), new IntRange(0, 30), 50);//X-Items
                    for (int ii = 0; ii < 18; ii++)
                        bossChanceZoneStep.Items.Add(new MapItem(76 + ii), new IntRange(0, 30), 200);//gummis
                    bossChanceZoneStep.Items.Add(new MapItem(158, 1), new IntRange(0, 30), 200);//amber tear
                    bossChanceZoneStep.Items.Add(new MapItem(101), new IntRange(0, 30), 200);//reviver seed
                    bossChanceZoneStep.Items.Add(new MapItem(102), new IntRange(0, 30), 200);//joy seed
                    bossChanceZoneStep.Items.Add(new MapItem(453), new IntRange(0, 30), 200);//ability capsule
                    bossChanceZoneStep.Items.Add(new MapItem(349), new IntRange(0, 30), 100);//harmony scarf
                    bossChanceZoneStep.Items.Add(new MapItem(455, 1), new IntRange(0, 30), 1000);//key

                    bossChanceZoneStep.Items.Add(new MapItem(592), new IntRange(0, 30), 5);//TM Toxic
                    bossChanceZoneStep.Items.Add(new MapItem(604), new IntRange(0, 30), 5);//TM Rock Climb
                    bossChanceZoneStep.Items.Add(new MapItem(605), new IntRange(0, 30), 5);//TM Waterfall
                    bossChanceZoneStep.Items.Add(new MapItem(609), new IntRange(0, 30), 5);//TM Charge Beam
                    bossChanceZoneStep.Items.Add(new MapItem(613), new IntRange(0, 30), 5);//TM Hone Claws
                    bossChanceZoneStep.Items.Add(new MapItem(615), new IntRange(0, 30), 5);//TM Giga Impact
                    bossChanceZoneStep.Items.Add(new MapItem(617), new IntRange(0, 30), 5);//TM Dig
                    bossChanceZoneStep.Items.Add(new MapItem(623), new IntRange(0, 30), 5);//TM Safeguard
                    bossChanceZoneStep.Items.Add(new MapItem(626), new IntRange(0, 30), 5);//TM Work Up
                    bossChanceZoneStep.Items.Add(new MapItem(627), new IntRange(0, 30), 5);//TM Scald
                    bossChanceZoneStep.Items.Add(new MapItem(630), new IntRange(0, 30), 5);//TM U-turn
                    bossChanceZoneStep.Items.Add(new MapItem(631), new IntRange(0, 30), 5);//TM Thunder Wave
                    bossChanceZoneStep.Items.Add(new MapItem(645), new IntRange(0, 30), 5);//TM Roost
                    bossChanceZoneStep.Items.Add(new MapItem(655), new IntRange(0, 30), 5);//TM Psyshock
                    bossChanceZoneStep.Items.Add(new MapItem(660), new IntRange(0, 30), 5);//TM Thunder
                    bossChanceZoneStep.Items.Add(new MapItem(661), new IntRange(0, 30), 5);//TM X-Scissor
                    bossChanceZoneStep.Items.Add(new MapItem(682), new IntRange(0, 30), 5);//TM Taunt
                    bossChanceZoneStep.Items.Add(new MapItem(684), new IntRange(0, 30), 5);//TM Ice Beam
                    bossChanceZoneStep.Items.Add(new MapItem(686), new IntRange(0, 30), 5);//TM Light Screen
                    bossChanceZoneStep.Items.Add(new MapItem(691), new IntRange(0, 30), 5);//TM Calm Mind
                    bossChanceZoneStep.Items.Add(new MapItem(692), new IntRange(0, 30), 5);//TM Torment
                    bossChanceZoneStep.Items.Add(new MapItem(693), new IntRange(0, 30), 5);//TM Strength
                    bossChanceZoneStep.Items.Add(new MapItem(694), new IntRange(0, 30), 5);//TM Cut
                    bossChanceZoneStep.Items.Add(new MapItem(695), new IntRange(0, 30), 5);//TM Rock Smash
                    bossChanceZoneStep.Items.Add(new MapItem(696), new IntRange(0, 30), 5);//TM Bulk Up
                    bossChanceZoneStep.Items.Add(new MapItem(698), new IntRange(0, 30), 5);//TM Rest

                    bossChanceZoneStep.Items.Add(new MapItem(591), new IntRange(0, 30), 5);//TM Psychic
                    bossChanceZoneStep.Items.Add(new MapItem(595), new IntRange(0, 30), 5);//TM Flamethrower
                    bossChanceZoneStep.Items.Add(new MapItem(599), new IntRange(0, 30), 5);//TM Hyper Beam
                    bossChanceZoneStep.Items.Add(new MapItem(611), new IntRange(0, 30), 5);//TM Blizzard
                    bossChanceZoneStep.Items.Add(new MapItem(619), new IntRange(0, 30), 5);//TM Rock Slide
                    bossChanceZoneStep.Items.Add(new MapItem(620), new IntRange(0, 30), 5);//TM Sludge Wave
                    bossChanceZoneStep.Items.Add(new MapItem(624), new IntRange(0, 30), 5);//TM Swords Dance
                    bossChanceZoneStep.Items.Add(new MapItem(628), new IntRange(0, 30), 5);//TM Energy Ball
                    bossChanceZoneStep.Items.Add(new MapItem(635), new IntRange(0, 30), 5);//TM Fire Blast
                    bossChanceZoneStep.Items.Add(new MapItem(640), new IntRange(0, 30), 5);//TM Thunderbolt
                    bossChanceZoneStep.Items.Add(new MapItem(641), new IntRange(0, 30), 5);//TM Shadow Ball
                    bossChanceZoneStep.Items.Add(new MapItem(649), new IntRange(0, 30), 5);//TM Dark Pulse
                    bossChanceZoneStep.Items.Add(new MapItem(654), new IntRange(0, 30), 5);//TM Earthquake
                    bossChanceZoneStep.Items.Add(new MapItem(658), new IntRange(0, 30), 5);//TM Frost Breath
                    bossChanceZoneStep.Items.Add(new MapItem(662), new IntRange(0, 30), 5);//TM Dazzling Gleam
                    bossChanceZoneStep.Items.Add(new MapItem(666), new IntRange(0, 30), 5);//TM Snarl
                    bossChanceZoneStep.Items.Add(new MapItem(669), new IntRange(0, 30), 5);//TM Focus Blast
                    bossChanceZoneStep.Items.Add(new MapItem(672), new IntRange(0, 30), 5);//TM Overheat
                    bossChanceZoneStep.Items.Add(new MapItem(676), new IntRange(0, 30), 5);//TM Sludge Bomb
                    bossChanceZoneStep.Items.Add(new MapItem(683), new IntRange(0, 30), 5);//TM Solar Beam
                    bossChanceZoneStep.Items.Add(new MapItem(685), new IntRange(0, 30), 5);//TM Flash Cannon
                    bossChanceZoneStep.Items.Add(new MapItem(697), new IntRange(0, 30), 5);//TM Surf

                }

                // item spawnings for the vault
                for (int ii = 0; ii < 30; ii++)
                {
                    //add a PickerSpawner <- PresetMultiRand <- coins
                    List<MapItem> treasures = new List<MapItem>();
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(false, 477));
                    PickerSpawner<ListMapGenContext, MapItem> treasurePicker = new PickerSpawner<ListMapGenContext, MapItem>(new PresetMultiRand<MapItem>(treasures));

                    SpawnList<IStepSpawner<ListMapGenContext, MapItem>> boxSpawn = new SpawnList<IStepSpawner<ListMapGenContext, MapItem>>();

                    //444      ***    Light Box - 1* items
                    {
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>(444, new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(1), new RandRange(1))), 10);
                    }

                    //445      ***    Heavy Box - 2* items
                    {
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>(445, new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(2), new RandRange(1))), 10);
                    }

                    ////446      ***    Nifty Box - all high tier TMs, ability capsule, heart scale 9, max potion, full heal, max elixir
                    //{
                    //    SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                    //    for (int nn = 588; nn < 700; nn++)
                    //        boxTreasure.Add(new MapItem(nn), 1);//TMs
                    //    boxTreasure.Add(new MapItem(453), 100);//ability capsule
                    //    boxTreasure.Add(new MapItem(481), 100);//heart scale
                    //    boxTreasure.Add(new MapItem(160), 60);//potion
                    //    boxTreasure.Add(new MapItem(161), 30);//max potion
                    //    boxTreasure.Add(new MapItem(173), 100);//full heal
                    //    boxTreasure.Add(new MapItem(164), 60);//elixir
                    //    boxTreasure.Add(new MapItem(166), 30);//max elixir
                    //    boxSpawn.Add(new BoxSpawner<ListMapGenContext>(446, new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 10);
                    //}

                    //447      ***    Dainty Box - Stat ups, wonder gummi, nectar, golden apple, golden banana
                    {
                        SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                        for (int nn = 0; nn < 18; nn++)//Gummi
                            boxTreasure.Add(new MapItem(76 + nn), 1);

                        boxTreasure.Add(new MapItem(151), 2);//protein
                        boxTreasure.Add(new MapItem(152), 2);//iron
                        boxTreasure.Add(new MapItem(153), 2);//calcium
                        boxTreasure.Add(new MapItem(154), 2);//zinc
                        boxTreasure.Add(new MapItem(155), 2);//carbos
                        boxTreasure.Add(new MapItem(156), 2);//hp up
                        boxTreasure.Add(new MapItem(150), 2);//nectar

                        boxTreasure.Add(new MapItem(5), 10);//perfect apple
                        boxTreasure.Add(new MapItem(7), 10);//big banana
                        boxTreasure.Add(new MapItem(75), 4);//wonder gummi
                        boxTreasure.Add(new MapItem(102), 10);//joy seed
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>(447, new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 3);
                    }

                    //448    Glittery Box - golden apple, amber tear, golden banana, nugget, golden thorn 9
                    {
                        SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();
                        boxTreasure.Add(new MapItem(205), 10);//golden thorn
                        boxTreasure.Add(new MapItem(158), 10);//Amber Tear
                        boxTreasure.Add(new MapItem(477), 10);//nugget
                        boxTreasure.Add(new MapItem(103), 10);//golden seed
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>(448, new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 2);
                    }

                    //449      ***    Deluxe Box - Legendary exclusive items, harmony scarf, golden items, stat ups, wonder gummi, perfect apricorn, max potion/full heal/max elixir
                    //{
                    //    SpeciesItemListSpawner<ListMapGenContext> legendSpawner = new SpeciesItemListSpawner<ListMapGenContext>(new IntRange(1, 3), new RandRange(1));
                    //    legendSpawner.Species.Add(144);
                    //    legendSpawner.Species.Add(145);
                    //    legendSpawner.Species.Add(146);
                    //    legendSpawner.Species.Add(150);
                    //    boxSpawn.Add(new BoxSpawner<ListMapGenContext>(449, legendSpawner), 5);
                    //}
                    //{
                    //    SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                    //    for (int nn = 0; nn < 18; nn++)//Gummi
                    //        boxTreasure.Add(new MapItem(76 + nn), 1);

                    //    boxTreasure.Add(new MapItem(4), 10);//golden apple
                    //    boxTreasure.Add(new MapItem(8), 10);//gold banana
                    //    boxTreasure.Add(new MapItem(158), 10);//Amber Tear
                    //    boxTreasure.Add(new MapItem(477), 10);//nugget
                    //    boxTreasure.Add(new MapItem(103), 10);//golden seed
                    //    boxTreasure.Add(new MapItem(161), 30);//max potion
                    //    boxTreasure.Add(new MapItem(173), 100);//full heal
                    //    boxTreasure.Add(new MapItem(166), 30);//max elixir
                    //    boxTreasure.Add(new MapItem(75), 4);//wonder gummi
                    //    boxTreasure.Add(new MapItem(349), 1);//harmony scarf
                    //    boxTreasure.Add(new MapItem(219), 1);//perfect apricorn
                    //    boxSpawn.Add(new BoxSpawner<ListMapGenContext>(449, new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 10);
                    //}

                    MultiStepSpawner<ListMapGenContext, MapItem> boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(1)));

                    //MultiStepSpawner <- PresetMultiRand
                    MultiStepSpawner<ListMapGenContext, MapItem> mainSpawner = new MultiStepSpawner<ListMapGenContext, MapItem>();
                    mainSpawner.Picker = new PresetMultiRand<IStepSpawner<ListMapGenContext, MapItem>>(treasurePicker, boxPicker);
                    bossChanceZoneStep.ItemSpawners.SetRange(mainSpawner, new IntRange(0, 30));
                }
                bossChanceZoneStep.ItemAmount.SetRange(new RandRange(2, 4), new IntRange(0, 30));

                // item placements for the vault
                {
                    RandomRoomSpawnStep<ListMapGenContext, MapItem> detourItems = new RandomRoomSpawnStep<ListMapGenContext, MapItem>();
                    detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                    bossChanceZoneStep.ItemPlacements.SetRange(detourItems, new IntRange(0, 30));
                }

                floorSegment.ZoneSteps.Add(bossChanceZoneStep);
            }

            SpawnRangeList<IGenPriority> shopZoneSpawns = new SpawnRangeList<IGenPriority>();
            {
                ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                shop.Personality = 0;
                shop.SecurityStatus = 38;
                shop.Items.Add(new MapItem(10, 0, 100), 20);//oran
                shop.Items.Add(new MapItem(11, 0, 150), 20);//leppa
                shop.Items.Add(new MapItem(12, 0, 100), 20);//lum
                shop.Items.Add(new MapItem(72, 0, 100), 20);//sitrus
                shop.Items.Add(new MapItem(101, 0, 800), 15);//reviver
                shop.Items.Add(new MapItem(118, 0, 500), 15);//ban
                for (int nn = 211; nn <= 218; nn++)
                    shop.Items.Add(new MapItem(nn, 0, 600), 2);//apricorns
                shop.Items.Add(new MapItem(210, 0, 800), 5);//plain apricorn
                for (int nn = 0; nn < 7; nn++)
                    shop.Items.Add(new MapItem(43 + nn, 0, 600), 3);//pinch berries
                for (int nn = 0; nn <= 18; nn++)
                    shop.Items.Add(new MapItem(19 + nn, 0, 100), 1);//type berries
                shop.Items.Add(new MapItem(112, 0, 350), 20);//blast seed
                shop.Items.Add(new MapItem(102, 0, 2000), 5);//joy seed

                shop.Items.Add(new MapItem(351, 0, 2500), 10);//Fire Stone
                shop.Items.Add(new MapItem(352, 0, 2500), 10);//Thunder Stone
                shop.Items.Add(new MapItem(353, 0, 2500), 10);//Water Stone
                shop.Items.Add(new MapItem(355, 0, 3500), 10);//Moon Stone
                shop.Items.Add(new MapItem(363, 0, 3500), 10);//Dusk Stone
                shop.Items.Add(new MapItem(364, 0, 2500), 10);//Dawn Stone
                shop.Items.Add(new MapItem(362, 0, 3500), 10);//Shiny Stone
                shop.Items.Add(new MapItem(354, 0, 2500), 10);//Leaf Stone
                shop.Items.Add(new MapItem(379, 0, 2500), 10);//Ice Stone

                shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);
                shop.ItemThemes.Add(new ItemThemeRange(new IntRange(351, 380), false, true, new RandRange(3, 5)), 10);//evo items

                // 213 Shuckle : 126 Contrary : 380 Gastro Acid : 230 Sweet Scent : 450 Bug Bite : 92 Toxic
                shop.StartMob = GetShopMob(213, 126, 380, 230, 450, 92, new int[] { 1569, 1570, 1571, 1572 }, 0);
                {
                    // 213 Shuckle : 126 Contrary : 380 Gastro Acid : 230 Sweet Scent : 450 Bug Bite : 92 Toxic
                    shop.Mobs.Add(GetShopMob(213, 126, 380, 230, 450, 92, new int[] { 1569, 1570, 1571, 1572 }, -1), 5);
                    // 213 Shuckle : 126 Contrary : 564 Sticky Web : 611 Infestation : 189 Mud-Slap : 522 Struggle Bug
                    shop.Mobs.Add(GetShopMob(213, 126, 564, 611, 189, 522, new int[] { 1569, 1570, 1571, 1572 }, -1), 5);
                    // 213 Shuckle : 126 Contrary : 201 Sandstorm : 564 Sticky Web : 446 Stealth Rock : 88 Rock Throw
                    shop.Mobs.Add(GetShopMob(213, 5, 201, 564, 446, 88, new int[] { 1569, 1570, 1571, 1572 }, -1, 24), 10);
                    // 213 Shuckle : 5 Sturdy : 379 Power Trick : 504 Shell Smash : 205 Rollout : 360 Gyro Ball
                    shop.Mobs.Add(GetShopMob(213, 5, 379, 504, 205, 360, new int[] { 1569, 1570, 1571, 1572 }, -1, 24), 10);
                    // 213 Shuckle : 5 Sturdy : 379 Power Trick : 450 Bug Bite : 444 Stone Edge : 523 Bulldoze
                    shop.Mobs.Add(GetShopMob(213, 5, 379, 450, 444, 523, new int[] { 1569, 1570, 1571, 1572 }, -1, 24), 5);
                }

                shopZoneSpawns.Add(new GenPriority<GenStep<MapGenContext>>(PR_SHOPS, shop), new IntRange(2, 25), 10);
            }
            {
                ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                shop.Personality = 0;
                shop.SecurityStatus = 38;
                shop.Items.Add(new MapItem(10, 0, 100), 20);//oran
                shop.Items.Add(new MapItem(11, 0, 150), 20);//leppa
                shop.Items.Add(new MapItem(12, 0, 100), 20);//lum
                shop.Items.Add(new MapItem(72, 0, 100), 20);//sitrus
                shop.Items.Add(new MapItem(101, 0, 800), 15);//reviver
                shop.Items.Add(new MapItem(118, 0, 500), 15);//ban
                for (int nn = 211; nn <= 218; nn++)
                    shop.Items.Add(new MapItem(nn, 0, 600), 2);//apricorns
                shop.Items.Add(new MapItem(210, 0, 800), 5);//plain apricorn
                for (int nn = 0; nn < 7; nn++)
                    shop.Items.Add(new MapItem(43 + nn, 0, 600), 3);//pinch berries
                for (int nn = 0; nn <= 18; nn++)
                    shop.Items.Add(new MapItem(19 + nn, 0, 100), 1);//type berries
                shop.Items.Add(new MapItem(112, 0, 350), 20);//blast seed
                shop.Items.Add(new MapItem(102, 0, 2000), 5);//joy seed


                shop.Items.Add(new MapItem(591, 0, 5000), 2);//TM Psychic
                shop.Items.Add(new MapItem(595, 0, 5000), 2);//TM Flamethrower
                shop.Items.Add(new MapItem(599, 0, 5000), 2);//TM Hyper Beam
                shop.Items.Add(new MapItem(611, 0, 5000), 2);//TM Blizzard
                shop.Items.Add(new MapItem(619, 0, 5000), 2);//TM Rock Slide
                shop.Items.Add(new MapItem(620, 0, 5000), 2);//TM Sludge Wave
                shop.Items.Add(new MapItem(624, 0, 4000), 2);//TM Swords Dance
                shop.Items.Add(new MapItem(628, 0, 5000), 2);//TM Energy Ball
                shop.Items.Add(new MapItem(635, 0, 5000), 2);//TM Fire Blast
                shop.Items.Add(new MapItem(640, 0, 5000), 2);//TM Thunderbolt
                shop.Items.Add(new MapItem(641, 0, 5000), 2);//TM Shadow Ball
                shop.Items.Add(new MapItem(649, 0, 5000), 2);//TM Dark Pulse
                shop.Items.Add(new MapItem(654, 0, 4000), 2);//TM Earthquake
                shop.Items.Add(new MapItem(658, 0, 4000), 2);//TM Frost Breath
                shop.Items.Add(new MapItem(662, 0, 5000), 2);//TM Dazzling Gleam
                shop.Items.Add(new MapItem(666, 0, 4000), 2);//TM Snarl
                shop.Items.Add(new MapItem(669, 0, 5000), 2);//TM Focus Blast
                shop.Items.Add(new MapItem(672, 0, 5000), 2);//TM Overheat
                shop.Items.Add(new MapItem(676, 0, 5000), 2);//TM Sludge Bomb
                shop.Items.Add(new MapItem(683, 0, 5000), 2);//TM Solar Beam
                shop.Items.Add(new MapItem(685, 0, 5000), 2);//TM Flash Cannon
                shop.Items.Add(new MapItem(697, 0, 5000), 2);//TM Surf

                for (int ii = 0; ii < 18; ii++)
                    shop.Items.Add(new MapItem(331 + ii, 0, 2000), 5);//type items

                shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);
                shop.ItemThemes.Add(new ItemThemeRange(new IntRange(331, 349), false, true, new RandRange(3, 5)), 10);//type items
                shop.ItemThemes.Add(new ItemThemeType(ItemData.UseType.Learn, false, true, new RandRange(3, 6)), 10);//TMs

                // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 103 screech : 86 thunder wave
                shop.StartMob = GetShopMob(352, 16, 485, 20, 103, 86, new int[] { 1984, 1985, 1988 }, 0);
                {
                    // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 103 screech : 86 thunder wave
                    shop.Mobs.Add(GetShopMob(352, 16, 485, 20, 103, 86, new int[] { 1984, 1985, 1988 }, -1), 10);
                    // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 50 disable : 374 fling
                    shop.Mobs.Add(GetShopMob(352, 16, 485, 20, 50, 374, new int[] { 1984, 1985, 1988 }, -1), 10);
                    // 352 Kecleon : 168 protean : 425 shadow sneak : 246 ancient power : 510 incinerate : 168 thief
                    shop.Mobs.Add(GetShopMob(352, 168, 425, 246, 510, 168, new int[] { 1984, 1985, 1988 }, -1, 24), 10);
                    // 352 Kecleon : 168 protean : 332 aerial ace : 421 shadow claw : 60 psybeam : 364 feint
                    shop.Mobs.Add(GetShopMob(352, 168, 332, 421, 60, 364, new int[] { 1984, 1985, 1988 }, -1, 24), 10);
                }

                shopZoneSpawns.Add(new GenPriority<GenStep<MapGenContext>>(PR_SHOPS, shop), new IntRange(25, 30), 10);
            }
            {
                ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                shop.Personality = 1;
                shop.SecurityStatus = 38;
                shop.Items.Add(new MapItem(251, 0, 300), 20);//weather orb
                shop.Items.Add(new MapItem(252, 0, 600), 20);//mobile orb
                shop.Items.Add(new MapItem(253, 0, 600), 20);//luminous orb
                shop.Items.Add(new MapItem(256, 0, 400), 20);//fill-in orb
                shop.Items.Add(new MapItem(258, 0, 300), 20);//all-aim orb
                shop.Items.Add(new MapItem(263, 0, 300), 20);//cleanse orb
                shop.Items.Add(new MapItem(264, 0, 600), 20);//one-shot orb
                shop.Items.Add(new MapItem(265, 0, 500), 20);//endure orb
                shop.Items.Add(new MapItem(266, 0, 400), 20);//pierce orb
                shop.Items.Add(new MapItem(267, 0, 600), 20);//stayaway orb
                shop.Items.Add(new MapItem(268, 0, 600), 20);//all protect orb
                shop.Items.Add(new MapItem(269, 0, 300), 20);//trap-see orb
                shop.Items.Add(new MapItem(270, 0, 300), 20);//trapbust orb
                shop.Items.Add(new MapItem(271, 0, 500), 20);//slumber orb
                shop.Items.Add(new MapItem(272, 0, 400), 20);//totter orb
                shop.Items.Add(new MapItem(273, 0, 400), 20);//petrify orb
                shop.Items.Add(new MapItem(274, 0, 400), 20);//freeze orb
                shop.Items.Add(new MapItem(275, 0, 500), 20);//spurn orb
                shop.Items.Add(new MapItem(276, 0, 500), 20);//foe-hold orb
                shop.Items.Add(new MapItem(277, 0, 400), 20);//nullify orb
                shop.Items.Add(new MapItem(278, 0, 300), 20);//all-dodge orb
                shop.Items.Add(new MapItem(281, 0, 500), 20);//one-room orb
                shop.Items.Add(new MapItem(282, 0, 400), 20);//slow orb
                shop.Items.Add(new MapItem(283, 0, 400), 20);//rebound orb
                shop.Items.Add(new MapItem(284, 0, 400), 20);//mirror orb
                shop.Items.Add(new MapItem(286, 0, 400), 20);//foe-seal orb
                shop.Items.Add(new MapItem(287, 0, 600), 20);//halving orb
                shop.Items.Add(new MapItem(288, 0, 300), 20);//rollcall orb
                shop.Items.Add(new MapItem(289, 0, 300), 20);//mug orb

                shop.Items.Add(new MapItem(220, 3, 180), 40);//path wand
                shop.Items.Add(new MapItem(221, 3, 150), 40);//pounce wand
                shop.Items.Add(new MapItem(222, 3, 150), 40);//whirlwind wand
                shop.Items.Add(new MapItem(223, 3, 150), 40);//switcher wand
                shop.Items.Add(new MapItem(225, 3, 120), 40);//lure wand
                shop.Items.Add(new MapItem(228, 3, 150), 40);//slow wand
                shop.Items.Add(new MapItem(231, 3, 150), 40);//topsy-turvy wand
                shop.Items.Add(new MapItem(232, 3, 150), 40);//warp wand
                shop.Items.Add(new MapItem(233, 3, 120), 40);//purge wand
                shop.Items.Add(new MapItem(234, 3, 120), 40);//lob wand

                shop.Items.Add(new MapItem(351, 0, 2500), 50);//Fire Stone
                shop.Items.Add(new MapItem(352, 0, 2500), 50);//Thunder Stone
                shop.Items.Add(new MapItem(353, 0, 2500), 50);//Water Stone
                shop.Items.Add(new MapItem(355, 0, 3500), 50);//Moon Stone
                shop.Items.Add(new MapItem(363, 0, 3500), 50);//Dusk Stone
                shop.Items.Add(new MapItem(364, 0, 2500), 50);//Dawn Stone
                shop.Items.Add(new MapItem(362, 0, 3500), 50);//Shiny Stone
                shop.Items.Add(new MapItem(354, 0, 2500), 50);//Leaf Stone
                shop.Items.Add(new MapItem(379, 0, 2500), 50);//Ice Stone
                shop.Items.Add(new MapItem(377, 0, 3500), 50);//Sun Ribbon
                shop.Items.Add(new MapItem(378, 0, 3500), 50);//Moon Ribbon

                shop.Items.Add(new MapItem(347, 0, 3500), 40);//Metal Coat
                for (int ii = 0; ii < 18; ii++)
                {
                    shop.Items.Add(new MapItem(331 + ii, 0, 2000), 10);//type items
                }


                shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);
                shop.ItemThemes.Add(new ItemThemeRange(new IntRange(351, 380), false, true, new RandRange(3, 5)), 10);//evo items
                shop.ItemThemes.Add(new ItemThemeRange(new IntRange(331, 349), false, true, new RandRange(3, 5)), 10);//type items

                // Cleffa : 98 Magic Guard : 118 Metronome : 47 Sing : 204 Charm : 313 Fake Tears
                {
                    MobSpawn post_mob = new MobSpawn();
                    post_mob.BaseForm = new MonsterID(173, 0, 0, Gender.Unknown);
                    post_mob.Tactic = 23;
                    post_mob.Level = new RandRange(5);
                    post_mob.Intrinsic = 98;
                    post_mob.SpecifiedSkills.Add(118);
                    post_mob.SpecifiedSkills.Add(47);
                    post_mob.SpecifiedSkills.Add(204);
                    post_mob.SpecifiedSkills.Add(313);
                    post_mob.SpawnFeatures.Add(new MobSpawnDiscriminator(1));
                    post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new BattleScriptEvent("ShopkeeperInteract")));
                    post_mob.SpawnFeatures.Add(new MobSpawnLuaTable("{ Role = \"Shopkeeper\" }"));
                    shop.StartMob = post_mob;
                }
                {
                    // 35 Clefairy : 132 Friend Guard : 282 Knock Off : 107 Minimize : 236 Moonlight : 277 Magic Coat
                    shop.Mobs.Add(GetShopMob(35, 132, 282, 107, 236, 277, new int[] { 973, 976 }, -1), 10);
                    // 36 Clefable : 109 Unaware : 118 Metronome : 500 Stored Power : 343 Covet : 271 Trick
                    shop.Mobs.Add(GetShopMob(36, 109, 118, 500, 343, 271, new int[] { 973, 976 }, -1), 5);
                    // 36 Clefable : 98 Magic Guard : 118 Metronome : 213 Attract : 282 Knock Off : 266 Follow Me
                    shop.Mobs.Add(GetShopMob(36, 98, 118, 213, 282, 266, new int[] { 973, 976 }, -1), 5);
                }

                shopZoneSpawns.Add(new GenPriority<GenStep<MapGenContext>>(PR_SHOPS, shop), new IntRange(15, 30), 10);
            }
            SpreadStepRangeZoneStep shopZoneStep = new SpreadStepRangeZoneStep(new SpreadPlanSpaced(new RandRange(4, 14), new IntRange(2, 28)), shopZoneSpawns);
            shopZoneStep.ModStates.Add(new FlagType(typeof(ShopModGenState)));
            floorSegment.ZoneSteps.Add(shopZoneStep);


            for (int ii = 0; ii < 30; ii++)
            {
                GridFloorGen layout = new GridFloorGen();

                //Floor settings
                MapDataStep<MapGenContext> floorData = new MapDataStep<MapGenContext>();
                floorData.TimeLimit = 3000;
                if (ii < 5)
                    floorData.Music = "B01. Demonstration.ogg";
                else if (ii < 9)
                    floorData.Music = "B18. Faulted Cliffs.ogg";
                else if (ii < 14)
                    floorData.Music = "B07. Flyaway Cliffs.ogg";
                else if (ii < 19)
                    floorData.Music = "B11. Igneous Tunnel.ogg";
                else if (ii < 24)
                    floorData.Music = "Fortune Ravine.ogg";
                else
                    floorData.Music = "B14. Champion Road.ogg";

                floorData.CharSight = Map.SightRange.Dark;

                if (ii < 11)
                    floorData.TileSight = Map.SightRange.Clear;
                else if (ii < 26)
                    floorData.TileSight = Map.SightRange.Dark;

                layout.GenSteps.Add(PR_FLOOR_DATA, floorData);

                //Tilesets
                if (ii < 5)
                    AddTextureData(layout, 147, 148, 149, 13);
                else if (ii < 9)
                    AddTextureData(layout, 174, 175, 176, 18);
                else if (ii < 11)
                    AddTextureData(layout, 337, 338, 339, 11);
                else if (ii < 14)
                    AddTextureData(layout, 243, 244, 245, 05);
                else if (ii < 17)
                    AddTextureData(layout, 60, 61, 62, 07);
                else if (ii < 19)
                    AddTextureData(layout, 75, 76, 77, 07);
                else if (ii < 21)
                    AddTextureData(layout, 442, 443, 444, 17);
                else if (ii < 24)
                {
                    if (ii < 23)
                        AddTextureData(layout, 129, 443, 444, 12);
                    else
                        AddTextureData(layout, 129, 443, 444, 12);
                }
                else
                    AddTextureData(layout, 454, 455, 456, 17, true);

                //wonder tiles
                RandRange wonderTileRange;
                if (ii < 10)
                    wonderTileRange = new RandRange(2, 4);
                else if (ii < 18)
                    wonderTileRange = new RandRange(1, 3);
                else
                    wonderTileRange = new RandRange(4);

                if (ii < 18)
                    AddSingleTrapStep(layout, wonderTileRange, 27, true);
                else
                    AddSingleTrapStep(layout, wonderTileRange, 27, false);

                //traps
                RandRange trapRange = new RandRange(0);
                if (ii < 10)
                    trapRange = new RandRange(6, 9);
                else if (ii < 15)
                    trapRange = new RandRange(8, 12);
                else if (ii < 20)
                    trapRange = new RandRange(10, 14);
                else if (ii < 25)
                    trapRange = new RandRange(12, 16);
                else
                    trapRange = new RandRange(10, 14);
                AddTrapsSteps(layout, trapRange);


                //money - 25,725P to 54,600P
                if (ii < 5)
                    AddMoneyData(layout, new RandRange(3, 7));
                else if (ii < 10)
                    AddMoneyData(layout, new RandRange(3, 8));
                else if (ii < 20)
                    AddMoneyData(layout, new RandRange(4, 9));
                else
                    AddMoneyData(layout, new RandRange(6, 10));

                //items
                if (ii < 5)
                    AddItemData(layout, new RandRange(3, 5), 25);
                else if (ii < 10)
                    AddItemData(layout, new RandRange(3, 6), 25);
                else
                    AddItemData(layout, new RandRange(4, 7), 25);

                List<MapItem> specificSpawns = new List<MapItem>();
                if (ii == 0)
                    specificSpawns.Add(new MapItem(210));//Apricorn
                if (ii == 29)
                    specificSpawns.Add(new MapItem(101));//Reviver Seed

                RandomSpawnStep<MapGenContext, MapItem> specificItemZoneStep = new RandomSpawnStep<MapGenContext, MapItem>(new PickerSpawner<MapGenContext, MapItem>(new PresetMultiRand<MapItem>(specificSpawns)));
                layout.GenSteps.Add(PR_SPAWN_ITEMS, specificItemZoneStep);



                //Add:
                // hidden wall items (silver spikes, rare fossils, wands, orbs, plates) - never add anything score-affecting when it comes to roguelockable dungeons
                // No valuable items, no chests, no distinct TMs.  maybe held items...

                SpawnList<MapItem> wallSpawns = new SpawnList<MapItem>();
                wallSpawns.Add(new MapItem(481, 1), 50);//heart scale
                wallSpawns.Add(new MapItem(204, 3), 20);//silver spike
                wallSpawns.Add(new MapItem(206, 3), 20);//rare fossil
                wallSpawns.Add(new MapItem(202, 2), 20);//corsola spike
                wallSpawns.Add(new MapItem(37), 10);//jaboca berry
                wallSpawns.Add(new MapItem(38), 10);//rowap berry
                wallSpawns.Add(new MapItem(220, 1), 10);//path wand
                wallSpawns.Add(new MapItem(228, 3), 10);//fear wand
                wallSpawns.Add(new MapItem(223, 2), 10);//switcher wand
                wallSpawns.Add(new MapItem(222, 2), 10);//whirlwind wand

                wallSpawns.Add(new MapItem(271), 10);//Slumber Orb
                wallSpawns.Add(new MapItem(282), 10);//Slow
                wallSpawns.Add(new MapItem(272), 10);//Totter
                wallSpawns.Add(new MapItem(275), 10);//Spurn
                wallSpawns.Add(new MapItem(267), 10);//Stayaway
                wallSpawns.Add(new MapItem(266), 10);//Pierce
                for (int nn = 0; nn < 18; nn++)//Type Plate
                    wallSpawns.Add(new MapItem(380 + nn), 1);

                TerrainSpawnStep<MapGenContext, MapItem> wallItemZoneStep = new TerrainSpawnStep<MapGenContext, MapItem>(new Tile(2));
                wallItemZoneStep.Spawn = new PickerSpawner<MapGenContext, MapItem>(new LoopedRand<MapItem>(wallSpawns, new RandRange(6, 10)));
                layout.GenSteps.Add(PR_SPAWN_ITEMS, wallItemZoneStep);

                if (ii >= 5 && ii < 15)
                {
                    //pearls in the water
                    SpawnList<MapItem> waterSpawns = new SpawnList<MapItem>();
                    waterSpawns.Add(new MapItem(480, 1), 50);//pearl
                    TerrainSpawnStep<MapGenContext, MapItem> waterItemZoneStep = new TerrainSpawnStep<MapGenContext, MapItem>(new Tile(3));
                    waterItemZoneStep.Spawn = new PickerSpawner<MapGenContext, MapItem>(new LoopedRand<MapItem>(waterSpawns, new RandRange(0, 4)));
                    layout.GenSteps.Add(PR_SPAWN_ITEMS, waterItemZoneStep);
                }

                if (ii >= 16 && ii < 19)
                {

                    {
                        AddDisconnectedRoomsStep<MapGenContext> addDisconnect = new AddDisconnectedRoomsStep<MapGenContext>();
                        addDisconnect.Components.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Disconnected));
                        addDisconnect.Amount = new RandRange(1, 4);

                        //Give it some room types to place
                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //cave
                        genericRooms.Add(new RoomGenCave<MapGenContext>(new RandRange(3, 7), new RandRange(3, 7)), 10);


                        addDisconnect.GenericRooms = genericRooms;

                        layout.GenSteps.Add(PR_ROOMS_GEN_EXTRA, addDisconnect);
                    }

                    {
                        //secret items
                        SpawnList<InvItem> secretItemSpawns = new SpawnList<InvItem>();
                        secretItemSpawns.Add(new InvItem(406), 3);//Weather Rock
                        secretItemSpawns.Add(new InvItem(405), 3);//Expert Belt
                        secretItemSpawns.Add(new InvItem(320), 3);//Choice Scarf
                        secretItemSpawns.Add(new InvItem(319), 3);//Choice Specs
                        secretItemSpawns.Add(new InvItem(318), 3);//Choice Band
                        secretItemSpawns.Add(new InvItem(321), 3);//Assault Vest
                        secretItemSpawns.Add(new InvItem(322), 3);//Life Orb
                        secretItemSpawns.Add(new InvItem(315), 3);//Heal Ribbon
                        secretItemSpawns.Add(new InvItem(455, false, 1), 35);//Key
                        for (int nn = 0; nn < 18; nn++)//Gummi
                            secretItemSpawns.Add(new InvItem(76 + nn), 2);

                        RandomRoomSpawnStep<MapGenContext, InvItem> secretPlacement = new RandomRoomSpawnStep<MapGenContext, InvItem>(new PickerSpawner<MapGenContext, InvItem>(new LoopedRand<InvItem>(secretItemSpawns, new RandRange(1, 3))));
                        secretPlacement.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Disconnected));
                        layout.GenSteps.Add(PR_SPAWN_ITEMS, secretPlacement);
                    }
                    {
                        //secret money
                        List<MapItem> secretItemSpawns = new List<MapItem>();
                        secretItemSpawns.Add(new MapItem(true, 200));
                        secretItemSpawns.Add(new MapItem(true, 200));
                        secretItemSpawns.Add(new MapItem(true, 200));
                        RandomRoomSpawnStep<MapGenContext, MapItem> secretPlacement = new RandomRoomSpawnStep<MapGenContext, MapItem>(new PickerSpawner<MapGenContext, MapItem>(new PresetMultiRand<MapItem>(secretItemSpawns)));
                        secretPlacement.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Disconnected));
                        layout.GenSteps.Add(PR_SPAWN_ITEMS, secretPlacement);
                    }

                    {
                        SpawnList<IStepSpawner<MapGenContext, MapItem>> boxSpawn = new SpawnList<IStepSpawner<MapGenContext, MapItem>>();

                        //444      ***    Light Box - 1* items
                        {
                            boxSpawn.Add(new BoxSpawner<MapGenContext>(444, new SpeciesItemContextSpawner<MapGenContext>(new IntRange(1), new RandRange(1))), 10);
                        }

                        //445      ***    Heavy Box - 2* items
                        {
                            boxSpawn.Add(new BoxSpawner<MapGenContext>(445, new SpeciesItemContextSpawner<MapGenContext>(new IntRange(2), new RandRange(1))), 10);
                        }

                        MultiStepSpawner<MapGenContext, MapItem> boxPicker = new MultiStepSpawner<MapGenContext, MapItem>(new LoopedRand<IStepSpawner<MapGenContext, MapItem>>(boxSpawn, new RandRange(0, 2)));

                        RandomRoomSpawnStep<MapGenContext, MapItem> secretPlacement = new RandomRoomSpawnStep<MapGenContext, MapItem>(boxPicker);
                        secretPlacement.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Disconnected));
                        layout.GenSteps.Add(PR_SPAWN_ITEMS, secretPlacement);
                    }

                    //secret enemies
                    SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                    // 218 Slugma : 510 Incinerate : 106 Harden
                    specificTeam.Spawns.Add(GetGenericMob(218, -1, 510, 106, -1, -1, new RandRange(23), 9));

                    //secret enemies
                    PlaceRandomMobsStep<MapGenContext> secretMobPlacement = new PlaceRandomMobsStep<MapGenContext>(new LoopedTeamSpawner<MapGenContext>(specificTeam, new RandRange(3, 6)));
                    secretMobPlacement.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Disconnected));
                    {
                        secretMobPlacement.ClumpFactor = 20;
                    }
                    layout.GenSteps.Add(PR_SPAWN_MOBS, secretMobPlacement);

                }

                if (ii >= 6 && ii < 8)
                {
                    //280 Ralts : 100 Teleport
                    //always holds a key
                    MobSpawn mob = GetGenericMob(280, -1, 100, 45, -1, -1, new RandRange(10));
                    MobSpawnItem keySpawn = new MobSpawnItem(true);
                    keySpawn.Items.Add(new InvItem(455, false, 1), 10);
                    mob.SpawnFeatures.Add(keySpawn);

                    SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                    specificTeam.Spawns.Add(mob);

                    LoopedTeamSpawner<MapGenContext> spawner = new LoopedTeamSpawner<MapGenContext>(specificTeam);
                    {
                        spawner.AmountSpawner = new RandDecay(0, 4, 80);
                    }
                    PlaceRandomMobsStep<MapGenContext> secretMobPlacement = new PlaceRandomMobsStep<MapGenContext>(spawner);
                    layout.GenSteps.Add(PR_SPAWN_MOBS, secretMobPlacement);
                }

                if (ii >= 11 && ii < 14)
                {
                    //147 Dratini : 35 Wrap : 43 Leer
                    SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                    specificTeam.Spawns.Add(GetGenericMob(147, -1, 35, 43, -1, -1, new RandRange(15)));

                    LoopedTeamSpawner<MapGenContext> spawner = new LoopedTeamSpawner<MapGenContext>(specificTeam);
                    {
                        spawner.AmountSpawner = new RandRange(1, 3);
                    }
                    PlaceDisconnectedMobsStep<MapGenContext> secretMobPlacement = new PlaceDisconnectedMobsStep<MapGenContext>(spawner);
                    secretMobPlacement.AcceptedTiles.Add(new Tile(3));
                    layout.GenSteps.Add(PR_SPAWN_MOBS, secretMobPlacement);
                }

                if (ii >= 21 && ii < 24)
                {
                    //142 Aerodactyl : 17 Wing Attack : 246 Ancient Power : 48 Supersonic : 97 Agility
                    SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                    MobSpawn mob = GetGenericMob(142, -1, 17, 246, 48, 97, new RandRange(48), 17);
                    mob.SpawnFeatures.Add(new MobSpawnItem(true, 477));
                    specificTeam.Spawns.Add(mob);

                    LoopedTeamSpawner<MapGenContext> spawner = new LoopedTeamSpawner<MapGenContext>(specificTeam);
                    {
                        spawner.AmountSpawner = new RandRange(1);
                    }
                    PlaceDisconnectedMobsStep<MapGenContext> secretMobPlacement = new PlaceDisconnectedMobsStep<MapGenContext>(spawner);
                    secretMobPlacement.AcceptedTiles.Add(new Tile(5));
                    layout.GenSteps.Add(PR_SPAWN_MOBS, secretMobPlacement);
                }



                //enemies
                if (ii < 8)
                    AddRespawnData(layout, 10, 80);
                else if (ii < 16)
                    AddRespawnData(layout, 15, 90);
                else if (ii < 20)
                    AddRespawnData(layout, 20, 100);
                else if (ii < 27)
                    AddRespawnData(layout, 30, 100);
                else
                    AddRespawnData(layout, 25, 120);

                //enemies
                if (ii < 8)
                    AddEnemySpawnData(layout, 30, new RandRange(5, 7));
                else if (ii < 16)
                    AddEnemySpawnData(layout, 30, new RandRange(9, 14));
                else if (ii < 20)
                    AddEnemySpawnData(layout, 30, new RandRange(13, 18));
                else if (ii < 27)
                    AddEnemySpawnData(layout, 20, new RandRange(18, 25));
                else
                    AddEnemySpawnData(layout, 20, new RandRange(15, 24));


                //construct paths
                if (ii < 5)
                {
                    AddInitGridStep(layout, 4, 4, 10, 10);

                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(90);
                    path.BranchRatio = new RandRange(0, 25);

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //cross
                    genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 11), new RandRange(4, 11), new RandRange(2, 6), new RandRange(2, 6)), 10);
                    //round
                    genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));

                }
                else if (ii < 9)
                {
                    AddInitGridStep(layout, 5, 4, 9, 9, 2);

                    GridPathCircle<MapGenContext> path = new GridPathCircle<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.CircleRoomRatio = new RandRange(70);
                    path.Paths = new RandRange(2, 5);

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //cross
                    genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(3, 9), new RandRange(3, 9), new RandRange(3, 7), new RandRange(3, 7)), 10);
                    //round
                    genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 8), new RandRange(5, 8)), 10);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(0), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                }
                else if (ii < 13)
                {
                    AddInitGridStep(layout, 5, 3, 10, 10);

                    GridPathTwoSides<MapGenContext> path = new GridPathTwoSides<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.GapAxis = Axis4.Horiz;

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //cross
                    genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 11), new RandRange(4, 11), new RandRange(2, 6), new RandRange(2, 6)), 10);
                    //round
                    genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                    //blocked
                    genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile(2), new RandRange(6, 10), new RandRange(6, 10), new RandRange(2, 4), new RandRange(2, 4)), 10);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(0), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(100, 50));

                }
                else if (ii < 19)
                {
                    AddInitGridStep(layout, 6, 4, 10, 10);

                    if (ii < 16)
                    {
                        GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.RoomRatio = new RandRange(70);
                        path.BranchRatio = new RandRange(0, 25);

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //cross
                        genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 11), new RandRange(4, 11), new RandRange(2, 6), new RandRange(2, 6)), 10);
                        //round
                        genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                        //blocked
                        genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile(2), new RandRange(6, 10), new RandRange(6, 10), new RandRange(2, 4), new RandRange(2, 4)), 10);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_GRID_GEN, path);

                        layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));
                        layout.GenSteps.Add(PR_GRID_GEN, new SetGridDefaultsStep<MapGenContext>(new RandRange(20), GetImmutableFilterList()));

                    }
                    else
                    {
                        GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.RoomRatio = new RandRange(60);
                        path.BranchRatio = new RandRange(0, 25);

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //cross
                        genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 11), new RandRange(4, 11), new RandRange(2, 6), new RandRange(2, 6)), 10);
                        //round
                        genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                        //blocked
                        genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile(2), new RandRange(6, 10), new RandRange(6, 10), new RandRange(2, 4), new RandRange(2, 4)), 10);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_GRID_GEN, path);

                        layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));
                        layout.GenSteps.Add(PR_GRID_GEN, new SetGridDefaultsStep<MapGenContext>(new RandRange(10), GetImmutableFilterList()));

                    }
                }
                else if (ii < 22)
                {
                    AddInitGridStep(layout, 6, 4, 10, 10);

                    if (ii < 21)
                    {
                        GridPathGrid<MapGenContext> path = new GridPathGrid<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.RoomRatio = 80;
                        path.HallRatio = 30;

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //square
                        genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 9), new RandRange(4, 9), new RandRange(0, 81)), 10);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(20), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_GRID_GEN, path);
                    }
                    else
                    {
                        GridPathGrid<MapGenContext> path = new GridPathGrid<MapGenContext>();
                        path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        path.RoomRatio = 100;
                        path.HallRatio = 0;

                        SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                        //square
                        genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 9), new RandRange(4, 9), new RandRange(0, 81)), 10);
                        path.GenericRooms = genericRooms;

                        SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                        genericHalls.Add(new RoomGenAngledHall<MapGenContext>(20), 10);
                        path.GenericHalls = genericHalls;

                        layout.GenSteps.Add(PR_GRID_GEN, path);
                    }
                }
                else if (ii < 24)
                {
                    AddInitGridStep(layout, 5, 4, 9, 9);

                    GridPathBeetle<MapGenContext> path = new GridPathBeetle<MapGenContext>();
                    path.LargeRoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.GiantHallGen.Add(new RoomGenBump<MapGenContext>(new RandRange(44, 61), new RandRange(4, 9), new RandRange(0, 101)), 10);
                    path.LegPercent = 80;
                    path.ConnectPercent = 80;
                    path.Vertical = true;

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8), new RandRange(0, 101)), 10);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(100), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                }
                else if (ii < 27)
                {
                    AddInitGridStep(layout, 7, 5, 7, 7);

                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(100);
                    path.BranchRatio = new RandRange(0, 25);

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    if (ii < 26)
                    {
                        //cross
                        genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(5, 15), new RandRange(5, 15), new RandRange(3, 6), new RandRange(3, 6)), 10);
                        //square
                        genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(8, 15), new RandRange(8, 15), new RandRange(0, 91)), 10);
                    }
                    else
                    {
                        //square
                        genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 7), new RandRange(4, 7), new RandRange(0, 81)), 10);
                    }
                    //blocked
                    genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile(2), new RandRange(5, 9), new RandRange(5, 9), new RandRange(8, 9), new RandRange(1, 3)), 4);
                    genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile(2), new RandRange(5, 9), new RandRange(5, 9), new RandRange(1, 3), new RandRange(8, 9)), 4);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(100), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(65, 100));
                    layout.GenSteps.Add(PR_GRID_GEN, new SetGridDefaultsStep<MapGenContext>(new RandRange(10), GetImmutableFilterList()));

                }
                else
                {
                    AddInitGridStep(layout, 8, 6, 8, 8);

                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(80);
                    path.BranchRatio = new RandRange(0, 25);

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //square
                    genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8), new RandRange(0, 30)), 10);
                    //blocked
                    genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile(2), new RandRange(5, 9), new RandRange(5, 9), new RandRange(8, 9), new RandRange(1, 2)), 4);
                    genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile(2), new RandRange(5, 9), new RandRange(5, 9), new RandRange(1, 2), new RandRange(8, 9)), 4);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));
                }

                AddDrawGridSteps(layout);

                AddStairStep(layout, false);

                int terrain = 0;
                if (ii < 14)
                    terrain = 3;//water
                else if (ii < 19)
                    terrain = 4;//lava
                else if (ii < 23)
                    terrain = 5;//abyss
                else
                    terrain = 5;//abyss

                //water
                PerlinWaterStep<MapGenContext> waterZoneStep = new PerlinWaterStep<MapGenContext>(new RandRange(), 3, new Tile(terrain), 1, true);
                if (ii < 4)
                    AddWaterSteps(layout, 3, new RandRange(35));//water
                else if (ii < 8)
                    AddWaterSteps(layout, 3, new RandRange(25));//water
                else if (ii < 10)
                    AddWaterSteps(layout, 3, new RandRange(22));//water
                else if (ii < 12)
                    AddWaterSteps(layout, 3, new RandRange(30), false);//water
                else if (ii < 14)
                    AddWaterSteps(layout, 3, new RandRange(15), false);//water
                else if (ii < 15)
                    AddWaterSteps(layout, 4, new RandRange(22));//lava
                else if (ii < 16)
                    AddWaterSteps(layout, 4, new RandRange(30));//lava
                else if (ii < 18)
                    AddWaterSteps(layout, 4, new RandRange(50));//lava
                else if (ii < 19)
                    AddWaterSteps(layout, 4, new RandRange(20));//lava
                else if (ii < 20)
                { }
                else if (ii < 23)
                    AddWaterSteps(layout, 5, new RandRange(30), false);//abyss
                else if (ii < 24)
                { }
                else if (ii < 26)
                    AddWaterSteps(layout, 5, new RandRange(35));//abyss
                else if (ii < 27)
                    AddWaterSteps(layout, 5, new RandRange(55));//abyss
                else if (ii < 28)
                    AddWaterSteps(layout, 5, new RandRange(75));//abyss
                else if (ii < 29)
                    AddWaterSteps(layout, 5, new RandRange(45));//abyss
                else
                    AddWaterSteps(layout, 5, new RandRange(25));//abyss

                layout.GenSteps.Add(PR_DBG_CHECK, new DetectIsolatedStairsStep<MapGenContext, MapGenEntrance, MapGenExit>());

                floorSegment.Floors.Add(layout);
            }

            zone.Segments.Add(floorSegment);

            LayeredSegment staticSegment = new LayeredSegment();
            {
                LoadGen layout = new LoadGen();
                MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                startGen.MapID = "guildmaster_summit";
                layout.GenSteps.Add(PR_TILES_INIT, startGen);
                MapEffectStep<MapLoadContext> noRescue = new MapEffectStep<MapLoadContext>();
                noRescue.Effect.OnMapRefresh.Add(0, new MapNoRescueEvent());
                layout.GenSteps.Add(PR_FLOOR_DATA, noRescue);
                staticSegment.Floors.Add(layout);
            }

            zone.Segments.Add(staticSegment);
        }
        #endregion

        #region SECRET GARDEN
        static void FillSecretGarden(ZoneData zone)
        {
            zone.Name = new LocalText("Secret Garden");
            zone.Level = 5;
            zone.LevelCap = true;
            zone.BagRestrict = 8;
            zone.TeamSize = 2;
            zone.Rescues = 2;
            zone.Rogue = RogueStatus.AllTransfer;
            int max_floors = 40;

            LayeredSegment floorSegment = new LayeredSegment();
            floorSegment.IsRelevant = true;
            floorSegment.ZoneSteps.Add(new SaveVarsZoneStep(PR_EXITS_RESCUE));
            floorSegment.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Secret Garden\nB{0}F")));

            //money
            MoneySpawnZoneStep moneySpawnZoneStep = new MoneySpawnZoneStep(PR_RESPAWN_MONEY, new RandRange(63, 72), new RandRange(21, 24));
            moneySpawnZoneStep.ModStates.Add(new FlagType(typeof(CoinModGenState)));
            floorSegment.ZoneSteps.Add(moneySpawnZoneStep);

            //items
            ItemSpawnZoneStep itemSpawnZoneStep = new ItemSpawnZoneStep();
            itemSpawnZoneStep.Priority = PR_RESPAWN_ITEM;


            //necessities
            CategorySpawn<InvItem> necessities = new CategorySpawn<InvItem>();
            necessities.SpawnRates.SetRange(14, new IntRange(0, max_floors));
            itemSpawnZoneStep.Spawns.Add("necessities", necessities);


            necessities.Spawns.Add(new InvItem(0072), new IntRange(0, max_floors), 45);//Sitrus Berry
            necessities.Spawns.Add(new InvItem(0011), new IntRange(0, 31), 70);//Leppa Berry
            necessities.Spawns.Add(new InvItem(0001), new IntRange(0, 31), 15);//Apple
            necessities.Spawns.Add(new InvItem(0012), new IntRange(0, max_floors), 50);//Lum Berry
            necessities.Spawns.Add(new InvItem(0101), new IntRange(0, 27), 10);//Reviver Seed
                                                                               //special
            CategorySpawn<InvItem> special = new CategorySpawn<InvItem>();
            special.SpawnRates.SetRange(4, new IntRange(0, 27));
            itemSpawnZoneStep.Spawns.Add("special", special);


            special.Spawns.Add(new InvItem(0450), new IntRange(0, 27), 25);//Link Box
            special.Spawns.Add(new InvItem(0451), new IntRange(4, 27), 5);//Assembly Box
            special.Spawns.Add(new InvItem(0455, false, 1), new IntRange(0, 24), 10);//Key
                                                                                     //snacks
            CategorySpawn<InvItem> snacks = new CategorySpawn<InvItem>();
            snacks.SpawnRates.SetRange(10, new IntRange(0, max_floors));
            itemSpawnZoneStep.Spawns.Add("snacks", snacks);


            snacks.Spawns.Add(new InvItem(0043), new IntRange(0, 27), 3);//Apicot Berry
            snacks.Spawns.Add(new InvItem(0044), new IntRange(0, 27), 3);//Liechi Berry
            snacks.Spawns.Add(new InvItem(0045), new IntRange(0, 27), 3);//Ganlon Berry
            snacks.Spawns.Add(new InvItem(0046), new IntRange(0, 27), 3);//Salac Berry
            snacks.Spawns.Add(new InvItem(0047), new IntRange(0, 27), 3);//Petaya Berry
            snacks.Spawns.Add(new InvItem(0048), new IntRange(0, 27), 3);//Starf Berry
            snacks.Spawns.Add(new InvItem(0049), new IntRange(0, 27), 3);//Micle Berry
            snacks.Spawns.Add(new InvItem(0051), new IntRange(0, 27), 4);//Enigma Berry
            snacks.Spawns.Add(new InvItem(0037), new IntRange(0, max_floors), 1);//Jaboca Berry
            snacks.Spawns.Add(new InvItem(0038), new IntRange(0, max_floors), 1);//Rowap Berry
            snacks.Spawns.Add(new InvItem(0019), new IntRange(0, 27), 1);//Tanga Berry
            snacks.Spawns.Add(new InvItem(0020), new IntRange(0, 27), 1);//Colbur Berry
            snacks.Spawns.Add(new InvItem(0021), new IntRange(0, 27), 1);//Haban Berry
            snacks.Spawns.Add(new InvItem(0022), new IntRange(0, 27), 1);//Wacan Berry
            snacks.Spawns.Add(new InvItem(0023), new IntRange(0, 27), 1);//Chople Berry
            snacks.Spawns.Add(new InvItem(0024), new IntRange(0, 27), 1);//Occa Berry
            snacks.Spawns.Add(new InvItem(0025), new IntRange(0, 27), 1);//Coba Berry
            snacks.Spawns.Add(new InvItem(0026), new IntRange(0, 27), 1);//Kasib Berry
            snacks.Spawns.Add(new InvItem(0027), new IntRange(0, 27), 1);//Rindo Berry
            snacks.Spawns.Add(new InvItem(0028), new IntRange(0, 27), 1);//Shuca Berry
            snacks.Spawns.Add(new InvItem(0029), new IntRange(0, 27), 1);//Yache Berry
            snacks.Spawns.Add(new InvItem(0030), new IntRange(0, 27), 1);//Chilan Berry
            snacks.Spawns.Add(new InvItem(0031), new IntRange(0, 27), 1);//Kebia Berry
            snacks.Spawns.Add(new InvItem(0032), new IntRange(0, 27), 1);//Payapa Berry
            snacks.Spawns.Add(new InvItem(0033), new IntRange(0, 27), 1);//Charti Berry
            snacks.Spawns.Add(new InvItem(0034), new IntRange(0, 27), 1);//Babiri Berry
            snacks.Spawns.Add(new InvItem(0035), new IntRange(0, 27), 1);//Passho Berry
            snacks.Spawns.Add(new InvItem(0036), new IntRange(0, 27), 1);//Roseli Berry
            snacks.Spawns.Add(new InvItem(0112), new IntRange(0, max_floors), 20);//Blast Seed
            snacks.Spawns.Add(new InvItem(0108), new IntRange(0, max_floors), 10);//Warp Seed
            snacks.Spawns.Add(new InvItem(0116), new IntRange(0, max_floors), 10);//Decoy Seed
            snacks.Spawns.Add(new InvItem(0110), new IntRange(0, max_floors), 10);//Sleep Seed
            snacks.Spawns.Add(new InvItem(0113), new IntRange(0, max_floors), 10);//Blinker Seed
            snacks.Spawns.Add(new InvItem(0117), new IntRange(0, max_floors), 5);//Last-Chance Seed
            snacks.Spawns.Add(new InvItem(0104), new IntRange(0, max_floors), 5);//Doom Seed
            snacks.Spawns.Add(new InvItem(0118), new IntRange(0, max_floors), 10);//Ban Seed
            snacks.Spawns.Add(new InvItem(0114, true), new IntRange(0, max_floors), 3);//Pure Seed
            snacks.Spawns.Add(new InvItem(0114), new IntRange(0, max_floors), 3);//Pure Seed
            snacks.Spawns.Add(new InvItem(0115), new IntRange(0, max_floors), 10);//Ice Seed
            snacks.Spawns.Add(new InvItem(0111), new IntRange(0, max_floors), 10);//Vile Seed
            snacks.Spawns.Add(new InvItem(0184, true), new IntRange(0, max_floors), 5);//Power Herb
            snacks.Spawns.Add(new InvItem(0184), new IntRange(0, max_floors), 5);//Power Herb
            snacks.Spawns.Add(new InvItem(0183), new IntRange(0, max_floors), 10);//Mental Herb
            snacks.Spawns.Add(new InvItem(0185), new IntRange(0, max_floors), 50);//White Herb
                                                                                  //boosters
            CategorySpawn<InvItem> boosters = new CategorySpawn<InvItem>();
            boosters.SpawnRates.SetRange(7, new IntRange(0, 27));
            itemSpawnZoneStep.Spawns.Add("boosters", boosters);


            boosters.Spawns.Add(new InvItem(0151, true), new IntRange(0, max_floors), 4);//Protein
            boosters.Spawns.Add(new InvItem(0151), new IntRange(0, max_floors), 6);//Protein
            boosters.Spawns.Add(new InvItem(0152, true), new IntRange(0, max_floors), 4);//Iron
            boosters.Spawns.Add(new InvItem(0152), new IntRange(0, max_floors), 6);//Iron
            boosters.Spawns.Add(new InvItem(0153, true), new IntRange(0, max_floors), 4);//Calcium
            boosters.Spawns.Add(new InvItem(0153), new IntRange(0, max_floors), 6);//Calcium
            boosters.Spawns.Add(new InvItem(0154, true), new IntRange(0, max_floors), 4);//Zinc
            boosters.Spawns.Add(new InvItem(0154), new IntRange(0, max_floors), 6);//Zinc
            boosters.Spawns.Add(new InvItem(0155, true), new IntRange(0, max_floors), 4);//Carbos
            boosters.Spawns.Add(new InvItem(0155), new IntRange(0, max_floors), 6);//Carbos
            boosters.Spawns.Add(new InvItem(0156, true), new IntRange(0, max_floors), 4);//HP Up
            boosters.Spawns.Add(new InvItem(0156), new IntRange(0, max_floors), 6);//HP Up
                                                                                   //throwable
            CategorySpawn<InvItem> throwable = new CategorySpawn<InvItem>();
            throwable.SpawnRates.SetRange(12, new IntRange(0, max_floors));
            itemSpawnZoneStep.Spawns.Add("throwable", throwable);


            throwable.Spawns.Add(new InvItem(0200, false, 4), new IntRange(0, max_floors), 10);//Stick
            throwable.Spawns.Add(new InvItem(0201, false, 3), new IntRange(0, max_floors), 10);//Cacnea Spike
            throwable.Spawns.Add(new InvItem(0220, false, 2), new IntRange(0, max_floors), 10);//Path Wand
            throwable.Spawns.Add(new InvItem(0228, false, 4), new IntRange(0, max_floors), 10);//Fear Wand
            throwable.Spawns.Add(new InvItem(0223, false, 4), new IntRange(0, max_floors), 10);//Switcher Wand
            throwable.Spawns.Add(new InvItem(0222, false, 4), new IntRange(0, max_floors), 10);//Whirlwind Wand
            throwable.Spawns.Add(new InvItem(0225, false, 4), new IntRange(0, max_floors), 10);//Lure Wand
            throwable.Spawns.Add(new InvItem(0226, false, 4), new IntRange(0, max_floors), 10);//Slow Wand
            throwable.Spawns.Add(new InvItem(0221, false, 4), new IntRange(0, max_floors), 10);//Pounce Wand
            throwable.Spawns.Add(new InvItem(0232, false, 2), new IntRange(0, max_floors), 10);//Warp Wand
            throwable.Spawns.Add(new InvItem(0231, false, 3), new IntRange(0, max_floors), 10);//Topsy-Turvy Wand
            throwable.Spawns.Add(new InvItem(0234, false, 4), new IntRange(0, max_floors), 10);//Lob Wand
            throwable.Spawns.Add(new InvItem(0233, false, 4), new IntRange(0, max_floors), 10);//Purge Wand
            throwable.Spawns.Add(new InvItem(0203, false, 3), new IntRange(0, max_floors), 10);//Iron Thorn
            throwable.Spawns.Add(new InvItem(0204, false, 3), new IntRange(0, max_floors), 10);//Silver Spike
            throwable.Spawns.Add(new InvItem(0208, false, 3), new IntRange(0, max_floors), 10);//Gravelerock
            throwable.Spawns.Add(new InvItem(0202, false, 3), new IntRange(0, max_floors), 10);//Corsola Twig
            throwable.Spawns.Add(new InvItem(0206, false, 3), new IntRange(0, max_floors), 10);//Rare Fossil
            throwable.Spawns.Add(new InvItem(0207, false, 3), new IntRange(0, max_floors), 10);//Geo Pebble
                                                                                               //orbs
            CategorySpawn<InvItem> orbs = new CategorySpawn<InvItem>();
            orbs.SpawnRates.SetRange(10, new IntRange(0, max_floors));
            itemSpawnZoneStep.Spawns.Add("orbs", orbs);


            orbs.Spawns.Add(new InvItem(0281, true), new IntRange(0, max_floors), 3);//One-Room Orb
            orbs.Spawns.Add(new InvItem(0281), new IntRange(0, max_floors), 7);//One-Room Orb
            orbs.Spawns.Add(new InvItem(0256, true), new IntRange(0, max_floors), 3);//Fill-In Orb
            orbs.Spawns.Add(new InvItem(0256), new IntRange(0, max_floors), 7);//Fill-In Orb
            orbs.Spawns.Add(new InvItem(0273, true), new IntRange(0, max_floors), 3);//Petrify Orb
            orbs.Spawns.Add(new InvItem(0273), new IntRange(0, max_floors), 7);//Petrify Orb
            orbs.Spawns.Add(new InvItem(0287, true), new IntRange(0, max_floors), 3);//Halving Orb
            orbs.Spawns.Add(new InvItem(0287), new IntRange(0, max_floors), 7);//Halving Orb
            orbs.Spawns.Add(new InvItem(0271, true), new IntRange(0, max_floors), 2);//Slumber Orb
            orbs.Spawns.Add(new InvItem(0271), new IntRange(0, max_floors), 6);//Slumber Orb
            orbs.Spawns.Add(new InvItem(0282, true), new IntRange(0, max_floors), 2);//Slow Orb
            orbs.Spawns.Add(new InvItem(0282), new IntRange(0, max_floors), 6);//Slow Orb
            orbs.Spawns.Add(new InvItem(0272, true), new IntRange(0, max_floors), 2);//Totter Orb
            orbs.Spawns.Add(new InvItem(0272), new IntRange(0, max_floors), 6);//Totter Orb
            orbs.Spawns.Add(new InvItem(0275, true), new IntRange(0, max_floors), 2);//Spurn Orb
            orbs.Spawns.Add(new InvItem(0275), new IntRange(0, max_floors), 6);//Spurn Orb
            orbs.Spawns.Add(new InvItem(0267, true), new IntRange(0, max_floors), 2);//Stayaway Orb
            orbs.Spawns.Add(new InvItem(0267), new IntRange(0, max_floors), 2);//Stayaway Orb
            orbs.Spawns.Add(new InvItem(0266, true), new IntRange(0, max_floors), 3);//Pierce Orb
            orbs.Spawns.Add(new InvItem(0266), new IntRange(0, max_floors), 7);//Pierce Orb
            orbs.Spawns.Add(new InvItem(0263), new IntRange(0, max_floors), 7);//Cleanse Orb
            orbs.Spawns.Add(new InvItem(0258), new IntRange(0, max_floors), 10);//All-Aim Orb
            orbs.Spawns.Add(new InvItem(0269), new IntRange(0, max_floors), 10);//Trap-See Orb
            orbs.Spawns.Add(new InvItem(0270), new IntRange(0, max_floors), 10);//Trapbust Orb
            orbs.Spawns.Add(new InvItem(0276), new IntRange(0, max_floors), 10);//Foe-Hold Orb
            orbs.Spawns.Add(new InvItem(0252), new IntRange(0, max_floors), 10);//Mobile Orb
            orbs.Spawns.Add(new InvItem(0288), new IntRange(0, max_floors), 10);//Rollcall Orb
            orbs.Spawns.Add(new InvItem(0289), new IntRange(0, max_floors), 10);//Mug Orb
            orbs.Spawns.Add(new InvItem(0284), new IntRange(0, max_floors), 10);//Mirror Orb
            orbs.Spawns.Add(new InvItem(0251), new IntRange(0, max_floors), 10);//Weather Orb
            orbs.Spawns.Add(new InvItem(0286), new IntRange(0, max_floors), 10);//Foe-Seal Orb
            orbs.Spawns.Add(new InvItem(0274), new IntRange(0, max_floors), 10);//Freeze Orb
            orbs.Spawns.Add(new InvItem(0257), new IntRange(0, max_floors), 10);//Devolve Orb
            orbs.Spawns.Add(new InvItem(0277), new IntRange(0, max_floors), 10);//Nullify Orb
            orbs.Spawns.Add(new InvItem(0283), new IntRange(0, max_floors), 10);//Rebound Orb
            orbs.Spawns.Add(new InvItem(0268, true), new IntRange(0, max_floors), 3);//All Protect Orb
            orbs.Spawns.Add(new InvItem(0268), new IntRange(0, max_floors), 7);//All Protect Orb
            orbs.Spawns.Add(new InvItem(0253, true), new IntRange(0, max_floors), 3);//Luminous Orb
            orbs.Spawns.Add(new InvItem(0253), new IntRange(0, max_floors), 7);//Luminous Orb
            orbs.Spawns.Add(new InvItem(0259, true), new IntRange(0, max_floors), 3);//Trawl Orb
            orbs.Spawns.Add(new InvItem(0259), new IntRange(0, max_floors), 7);//Trawl Orb
            orbs.Spawns.Add(new InvItem(0261), new IntRange(0, max_floors), 10);//Scanner Orb
                                                                                //held
            CategorySpawn<InvItem> held = new CategorySpawn<InvItem>();
            held.SpawnRates.SetRange(2, new IntRange(0, 27));
            itemSpawnZoneStep.Spawns.Add("held", held);


            held.Spawns.Add(new InvItem(0331), new IntRange(0, max_floors), 5);//Silver Powder
            held.Spawns.Add(new InvItem(0332), new IntRange(0, max_floors), 5);//Black Glasses
            held.Spawns.Add(new InvItem(0333), new IntRange(0, max_floors), 5);//Dragon Scale
            held.Spawns.Add(new InvItem(0334), new IntRange(0, max_floors), 5);//Magnet
            held.Spawns.Add(new InvItem(0335), new IntRange(0, max_floors), 5);//Pink Bow
            held.Spawns.Add(new InvItem(0336), new IntRange(0, max_floors), 5);//Black Belt
            held.Spawns.Add(new InvItem(0337), new IntRange(0, max_floors), 5);//Charcoal
            held.Spawns.Add(new InvItem(0338), new IntRange(0, max_floors), 5);//Sharp Beak
            held.Spawns.Add(new InvItem(0339), new IntRange(0, max_floors), 5);//Spell Tag
            held.Spawns.Add(new InvItem(0340), new IntRange(0, max_floors), 5);//Miracle Seed
            held.Spawns.Add(new InvItem(0341), new IntRange(0, max_floors), 5);//Soft Sand
            held.Spawns.Add(new InvItem(0342), new IntRange(0, max_floors), 5);//Never-Melt Ice
            held.Spawns.Add(new InvItem(0343), new IntRange(0, max_floors), 5);//Silk Scarf
            held.Spawns.Add(new InvItem(0344), new IntRange(0, max_floors), 5);//Poison Barb
            held.Spawns.Add(new InvItem(0345), new IntRange(0, max_floors), 5);//Twisted Spoon
            held.Spawns.Add(new InvItem(0346), new IntRange(0, max_floors), 5);//Hard Stone
            held.Spawns.Add(new InvItem(0347), new IntRange(0, max_floors), 5);//Metal Coat
            held.Spawns.Add(new InvItem(0348), new IntRange(0, max_floors), 5);//Mystic Water
            held.Spawns.Add(new InvItem(0380), new IntRange(0, max_floors), 5);//Insect Plate
            held.Spawns.Add(new InvItem(0381), new IntRange(0, max_floors), 5);//Dread Plate
            held.Spawns.Add(new InvItem(0382), new IntRange(0, max_floors), 5);//Draco Plate
            held.Spawns.Add(new InvItem(0383), new IntRange(0, max_floors), 5);//Zap Plate
            held.Spawns.Add(new InvItem(0384), new IntRange(0, max_floors), 5);//Pixie Plate
            held.Spawns.Add(new InvItem(0385), new IntRange(0, max_floors), 5);//Fist Plate
            held.Spawns.Add(new InvItem(0386), new IntRange(0, max_floors), 5);//Flame Plate
            held.Spawns.Add(new InvItem(0387), new IntRange(0, max_floors), 5);//Sky Plate
            held.Spawns.Add(new InvItem(0388), new IntRange(0, max_floors), 5);//Spooky Plate
            held.Spawns.Add(new InvItem(0389), new IntRange(0, max_floors), 5);//Meadow Plate
            held.Spawns.Add(new InvItem(0390), new IntRange(0, max_floors), 5);//Earth Plate
            held.Spawns.Add(new InvItem(0391), new IntRange(0, max_floors), 5);//Icicle Plate
            held.Spawns.Add(new InvItem(0393), new IntRange(0, max_floors), 5);//Toxic Plate
            held.Spawns.Add(new InvItem(0394), new IntRange(0, max_floors), 5);//Mind Plate
            held.Spawns.Add(new InvItem(0395), new IntRange(0, max_floors), 5);//Stone Plate
            held.Spawns.Add(new InvItem(0396), new IntRange(0, max_floors), 5);//Iron Plate
            held.Spawns.Add(new InvItem(0397), new IntRange(0, max_floors), 5);//Splash Plate
            held.Spawns.Add(new InvItem(0303), new IntRange(0, max_floors), 10);//Mobile Scarf
            held.Spawns.Add(new InvItem(0304), new IntRange(0, max_floors), 10);//Pass Scarf
            held.Spawns.Add(new InvItem(0330), new IntRange(0, max_floors), 10);//Cover Band
            held.Spawns.Add(new InvItem(0329, true), new IntRange(0, max_floors), 5);//Reunion Cape
            held.Spawns.Add(new InvItem(0329), new IntRange(0, max_floors), 5);//Reunion Cape
            held.Spawns.Add(new InvItem(0306, true), new IntRange(0, max_floors), 5);//Trap Scarf
            held.Spawns.Add(new InvItem(0306), new IntRange(0, max_floors), 5);//Trap Scarf
            held.Spawns.Add(new InvItem(0307), new IntRange(0, max_floors), 10);//Grip Claw
            held.Spawns.Add(new InvItem(0309, true), new IntRange(0, max_floors), 5);//Twist Band
            held.Spawns.Add(new InvItem(0309), new IntRange(0, max_floors), 5);//Twist Band
            held.Spawns.Add(new InvItem(0310, true), new IntRange(0, max_floors), 5);//Metronome
            held.Spawns.Add(new InvItem(0310), new IntRange(0, max_floors), 5);//Metronome
            held.Spawns.Add(new InvItem(0312, true), new IntRange(0, max_floors), 5);//Shell Bell
            held.Spawns.Add(new InvItem(0312), new IntRange(0, max_floors), 5);//Shell Bell
            held.Spawns.Add(new InvItem(0313, true), new IntRange(0, max_floors), 5);//Scope Lens
            held.Spawns.Add(new InvItem(0313), new IntRange(0, max_floors), 5);//Scope Lens
            held.Spawns.Add(new InvItem(0400), new IntRange(0, max_floors), 10);//Power Band
            held.Spawns.Add(new InvItem(0401), new IntRange(0, max_floors), 10);//Special Band
            held.Spawns.Add(new InvItem(0402), new IntRange(0, max_floors), 10);//Defense Scarf
            held.Spawns.Add(new InvItem(0403), new IntRange(0, max_floors), 10);//Zinc Band
            held.Spawns.Add(new InvItem(0314, true), new IntRange(0, max_floors), 10);//Wide Lens
            held.Spawns.Add(new InvItem(0301, true), new IntRange(0, max_floors), 5);//Pierce Band
            held.Spawns.Add(new InvItem(0301), new IntRange(0, max_floors), 5);//Pierce Band
            held.Spawns.Add(new InvItem(0311, true), new IntRange(0, max_floors), 5);//Shed Shell
            held.Spawns.Add(new InvItem(0311), new IntRange(0, max_floors), 5);//Shed Shell
            held.Spawns.Add(new InvItem(0328, true), new IntRange(0, max_floors), 5);//X-Ray Specs
            held.Spawns.Add(new InvItem(0328), new IntRange(0, max_floors), 5);//X-Ray Specs
            held.Spawns.Add(new InvItem(0404, true), new IntRange(0, max_floors), 5);//Big Root
            held.Spawns.Add(new InvItem(0404), new IntRange(0, max_floors), 5);//Big Root
            held.Spawns.Add(new InvItem(0406, true), new IntRange(0, max_floors), 5);//Weather Rock
            held.Spawns.Add(new InvItem(0406), new IntRange(0, max_floors), 5);//Weather Rock
            held.Spawns.Add(new InvItem(0405, true), new IntRange(0, max_floors), 5);//Expert Belt
            held.Spawns.Add(new InvItem(0405), new IntRange(0, max_floors), 5);//Expert Belt
            held.Spawns.Add(new InvItem(0320), new IntRange(0, max_floors), 10);//Choice Scarf
            held.Spawns.Add(new InvItem(0319), new IntRange(0, max_floors), 10);//Choice Specs
            held.Spawns.Add(new InvItem(0318), new IntRange(0, max_floors), 10);//Choice Band
            held.Spawns.Add(new InvItem(0321), new IntRange(0, max_floors), 10);//Assault Vest
            held.Spawns.Add(new InvItem(0322), new IntRange(0, max_floors), 10);//Life Orb
            held.Spawns.Add(new InvItem(0315, true), new IntRange(0, max_floors), 5);//Heal Ribbon
            held.Spawns.Add(new InvItem(0315), new IntRange(0, max_floors), 5);//Heal Ribbon
            held.Spawns.Add(new InvItem(0305, true), new IntRange(0, max_floors), 5);//Warp Scarf
            held.Spawns.Add(new InvItem(0305), new IntRange(0, max_floors), 5);//Warp Scarf
            held.Spawns.Add(new InvItem(0323), new IntRange(0, max_floors), 10);//Toxic Orb
            held.Spawns.Add(new InvItem(0324), new IntRange(0, max_floors), 10);//Flame Orb
            held.Spawns.Add(new InvItem(0317), new IntRange(0, max_floors), 10);//Sticky Barb
            held.Spawns.Add(new InvItem(0326), new IntRange(0, max_floors), 10);//Ring Target
                                                                                //evo
            CategorySpawn<InvItem> evo = new CategorySpawn<InvItem>();
            evo.SpawnRates.SetRange(1, new IntRange(0, 27));
            itemSpawnZoneStep.Spawns.Add("evo", evo);


            evo.Spawns.Add(new InvItem(0354), new IntRange(0, max_floors), 10);//Leaf Stone
            evo.Spawns.Add(new InvItem(0356), new IntRange(0, max_floors), 10);//Sun Stone
            evo.Spawns.Add(new InvItem(0351), new IntRange(0, max_floors), 10);//Fire Stone
            evo.Spawns.Add(new InvItem(0374), new IntRange(0, max_floors), 10);//King's Rock
            evo.Spawns.Add(new InvItem(0365), new IntRange(0, max_floors), 10);//Link Cable
            evo.Spawns.Add(new InvItem(0355), new IntRange(0, max_floors), 10);//Moon Stone
            evo.Spawns.Add(new InvItem(0353), new IntRange(0, max_floors), 10);//Water Stone
                                                                               //tms
            CategorySpawn<InvItem> tms = new CategorySpawn<InvItem>();
            tms.SpawnRates.SetRange(5, new IntRange(0, 27));
            itemSpawnZoneStep.Spawns.Add("tms", tms);


            tms.Spawns.Add(new InvItem(0587), new IntRange(0, max_floors), 10);//TM Secret Power
            tms.Spawns.Add(new InvItem(0589), new IntRange(0, max_floors), 10);//TM Echoed Voice
            tms.Spawns.Add(new InvItem(0590), new IntRange(0, max_floors), 10);//TM Double Team
            tms.Spawns.Add(new InvItem(0592), new IntRange(0, max_floors), 10);//TM Toxic
            tms.Spawns.Add(new InvItem(0596), new IntRange(0, max_floors), 10);//TM Protect
            tms.Spawns.Add(new InvItem(0597), new IntRange(0, max_floors), 10);//TM Defog
            tms.Spawns.Add(new InvItem(0598), new IntRange(0, max_floors), 10);//TM Roar
            tms.Spawns.Add(new InvItem(0599), new IntRange(0, max_floors), 5);//TM Hyper Beam
            tms.Spawns.Add(new InvItem(0600), new IntRange(0, max_floors), 10);//TM Swagger
            tms.Spawns.Add(new InvItem(0602), new IntRange(0, max_floors), 10);//TM Fly
            tms.Spawns.Add(new InvItem(0603), new IntRange(0, max_floors), 10);//TM Façade
            tms.Spawns.Add(new InvItem(0604), new IntRange(0, max_floors), 10);//TM Rock Climb
            tms.Spawns.Add(new InvItem(0610), new IntRange(0, max_floors), 10);//TM Payback
            tms.Spawns.Add(new InvItem(0615), new IntRange(0, max_floors), 5);//TM Giga Impact
            tms.Spawns.Add(new InvItem(0617), new IntRange(0, max_floors), 10);//TM Dig
            tms.Spawns.Add(new InvItem(0621), new IntRange(0, max_floors), 10);//TM Substitute
            tms.Spawns.Add(new InvItem(0623), new IntRange(0, max_floors), 10);//TM Safeguard
            tms.Spawns.Add(new InvItem(0624), new IntRange(0, max_floors), 5);//TM Swords Dance
            tms.Spawns.Add(new InvItem(0626), new IntRange(0, max_floors), 10);//TM Work Up
            tms.Spawns.Add(new InvItem(0632), new IntRange(0, max_floors), 10);//TM Return
            tms.Spawns.Add(new InvItem(0634), new IntRange(0, max_floors), 10);//TM Frustration
            tms.Spawns.Add(new InvItem(0637), new IntRange(0, max_floors), 10);//TM Flash
            tms.Spawns.Add(new InvItem(0638), new IntRange(0, max_floors), 10);//TM Thief
            tms.Spawns.Add(new InvItem(0643), new IntRange(0, max_floors), 10);//TM Fling
            tms.Spawns.Add(new InvItem(0644), new IntRange(0, max_floors), 10);//TM Captivate
            tms.Spawns.Add(new InvItem(0654), new IntRange(0, max_floors), 10);//TM Earthquake
            tms.Spawns.Add(new InvItem(0664), new IntRange(0, max_floors), 10);//TM Reflect
            tms.Spawns.Add(new InvItem(0667), new IntRange(0, max_floors), 10);//TM Round
            tms.Spawns.Add(new InvItem(0674), new IntRange(0, max_floors), 10);//TM Rain Dance
            tms.Spawns.Add(new InvItem(0675), new IntRange(0, max_floors), 10);//TM Sunny Day
            tms.Spawns.Add(new InvItem(0677), new IntRange(0, max_floors), 10);//TM Sandstorm
            tms.Spawns.Add(new InvItem(0678), new IntRange(0, max_floors), 10);//TM Hail
            tms.Spawns.Add(new InvItem(0680), new IntRange(0, max_floors), 10);//TM Attract
            tms.Spawns.Add(new InvItem(0681), new IntRange(0, max_floors), 10);//TM Hidden Power
            tms.Spawns.Add(new InvItem(0682), new IntRange(0, max_floors), 10);//TM Taunt
            tms.Spawns.Add(new InvItem(0686), new IntRange(0, max_floors), 10);//TM Light Screen
            tms.Spawns.Add(new InvItem(0689), new IntRange(0, max_floors), 10);//TM Grass Knot
            tms.Spawns.Add(new InvItem(0693), new IntRange(0, max_floors), 10);//TM Strength
            tms.Spawns.Add(new InvItem(0694), new IntRange(0, max_floors), 10);//TM Cut
            tms.Spawns.Add(new InvItem(0695), new IntRange(0, max_floors), 10);//TM Rock Smash
            tms.Spawns.Add(new InvItem(0697), new IntRange(0, max_floors), 10);//TM Surf
            tms.Spawns.Add(new InvItem(0698), new IntRange(0, max_floors), 10);//TM Rest
            tms.Spawns.Add(new InvItem(0699), new IntRange(0, max_floors), 10);//TM Psych Up


            floorSegment.ZoneSteps.Add(itemSpawnZoneStep);


            //mobs
            TeamSpawnZoneStep poolSpawn = new TeamSpawnZoneStep();
            poolSpawn.Priority = PR_RESPAWN_MOB;


            //010 Caterpie : 033 Tackle : 081 String Shot
            poolSpawn.Spawns.Add(GetTeamMob(010, -1, 033, 081, -1, -1, new RandRange(3)), new IntRange(0, 3), 10);
            //396 Starly : 033 Tackle
            poolSpawn.Spawns.Add(GetTeamMob(396, -1, 033, -1, -1, -1, new RandRange(3)), new IntRange(0, 3), 10);
            //191 Sunkern : 071 Absorb
            poolSpawn.Spawns.Add(GetTeamMob(191, -1, 071, -1, -1, -1, new RandRange(5)), new IntRange(0, 3), 10);
            //273 Seedot : 117 Bide
            poolSpawn.Spawns.Add(GetTeamMob(273, -1, 117, -1, -1, -1, new RandRange(5)), new IntRange(0, 3), 10);
            //161 Sentret : 010 Scratch : 111 Defense Curl
            poolSpawn.Spawns.Add(GetTeamMob(161, -1, 010, 111, -1, -1, new RandRange(5)), new IntRange(1, 5), 10);
            //013 Weedle : 040 Poison Sting : 081 String Shot
            poolSpawn.Spawns.Add(GetTeamMob(013, -1, 040, 081, -1, -1, new RandRange(5)), new IntRange(1, 5), 10);
            //333 Swablu : 030 Natural Cure : 047 Sing : 064 Peck
            poolSpawn.Spawns.Add(GetTeamMob(333, 030, 047, 064, -1, -1, new RandRange(6)), new IntRange(1, 5), 10);
            //309 Electrike : 336 Howl : 033 Tackle
            poolSpawn.Spawns.Add(GetTeamMob(309, -1, 336, 033, -1, -1, new RandRange(6)), new IntRange(1, 5), 10);
            //412 Burmy : 182 Protect
            poolSpawn.Spawns.Add(GetTeamMob(412, -1, 182, -1, -1, -1, new RandRange(7), 25), new IntRange(1, 5), 5);
            //412 Burmy : 182 Protect
            poolSpawn.Spawns.Add(GetTeamMob(new MonsterID(412, 1, -1, Gender.Unknown), -1, 182, -1, -1, -1, new RandRange(7), 25), new IntRange(1, 5), 5);
            //412 Burmy : 182 Protect
            poolSpawn.Spawns.Add(GetTeamMob(new MonsterID(412, 2, -1, Gender.Unknown), -1, 182, -1, -1, -1, new RandRange(7), 25), new IntRange(1, 5), 5);
            //066 Machop : 067 Low Kick : 116 Focus Energy
            poolSpawn.Spawns.Add(GetTeamMob(066, -1, 067, 116, -1, -1, new RandRange(8)), new IntRange(3, 7), 10);
            //043 Oddish : 230 Sweet Scent : 051 Acid
            poolSpawn.Spawns.Add(GetTeamMob(043, -1, 230, 051, -1, -1, new RandRange(9)), new IntRange(3, 7), 10);
            //300 Skitty : 056 Cute Charm : 252 Fake Out
            poolSpawn.Spawns.Add(GetTeamMob(300, 056, 252, -1, -1, -1, new RandRange(10)), new IntRange(3, 7), 10);
            //209 Snubbull : 044 Bite : 204 Charm
            poolSpawn.Spawns.Add(GetTeamMob(209, -1, 044, 204, -1, -1, new RandRange(9)), new IntRange(3, 7), 10);
            //050 Diglett : 071 Arena Trap : 189 Mud-Slap : 010 Scratch
            poolSpawn.Spawns.Add(GetTeamMob(050, 071, 189, 010, -1, -1, new RandRange(10)), new IntRange(3, 7), 10);
            //179 Mareep : 178 Cotton Spore : 084 Thunder Shock
            poolSpawn.Spawns.Add(GetTeamMob(179, -1, 178, 084, -1, -1, new RandRange(10)), new IntRange(3, 7), 10);
            //187 Hoppip : 235 Synthesis : 033 Tackle
            poolSpawn.Spawns.Add(GetTeamMob(187, -1, 235, 033, -1, -1, new RandRange(10)), new IntRange(3, 7), 10);
            //023 Ekans : 040 Poison Sting : 043 Leer
            poolSpawn.Spawns.Add(GetTeamMob(023, -1, 040, 043, -1, -1, new RandRange(10)), new IntRange(5, 9), 10);
            //393 Piplup : 128 Defiant : 064 Peck
            poolSpawn.Spawns.Add(GetTeamMob(393, 128, 064, -1, -1, -1, new RandRange(12), TeamMemberSpawn.MemberRole.Loner), new IntRange(5, 9), 10);
            //390 Chimchar : 269 Taunt : 052 Ember
            poolSpawn.Spawns.Add(GetTeamMob(390, -1, 269, 052, -1, -1, new RandRange(12), TeamMemberSpawn.MemberRole.Loner), new IntRange(5, 9), 10);
            //387 Turtwig : 110 Withdraw : 071 Absorb
            poolSpawn.Spawns.Add(GetTeamMob(387, -1, 110, 071, -1, -1, new RandRange(12), TeamMemberSpawn.MemberRole.Loner), new IntRange(5, 9), 10);
            //133 Eevee : 608 Baby-Doll Eyes : 129 Swift
            poolSpawn.Spawns.Add(GetTeamMob(133, -1, 608, 129, -1, -1, new RandRange(11)), new IntRange(5, 9), 10);
            //325 Spoink : 149 Psywave
            poolSpawn.Spawns.Add(GetTeamMob(325, -1, 149, -1, -1, -1, new RandRange(14)), new IntRange(5, 9), 10);
            //438 Bonsly : 313 Fake Tears : 067 Low Kick
            poolSpawn.Spawns.Add(GetTeamMob(438, -1, 313, 067, -1, -1, new RandRange(14), TeamMemberSpawn.MemberRole.Loner, 10), new IntRange(5, 9), 10);
            //077 Ponyta : 172 Flame Wheel : 039 Tail Whip
            poolSpawn.Spawns.Add(GetTeamMob(077, -1, 172, 039, -1, -1, new RandRange(13)), new IntRange(5, 9), 10);
            //060 Poliwag : 346 Water Sport : 055 Water Gun
            poolSpawn.Spawns.Add(GetTeamMob(060, -1, 346, 055, -1, -1, new RandRange(12)), new IntRange(5, 9), 10);
            //193 Yanma : 003 Speed Boost : 049 Sonic Boom
            poolSpawn.Spawns.Add(GetTeamMob(193, 003, 049, -1, -1, -1, new RandRange(16)), new IntRange(7, 11), 10);
            //440 Happiny : 383 Copycat : 204 Charm
            poolSpawn.Spawns.Add(GetTeamMob(440, -1, 383, 204, -1, -1, new RandRange(16), TeamMemberSpawn.MemberRole.Support), new IntRange(7, 11), 5);
            //215 Sneasel : 196 Icy Wind : 269 Taunt
            poolSpawn.Spawns.Add(GetTeamMob(215, -1, 196, 269, -1, -1, new RandRange(16)), new IntRange(7, 11), 10);
            //274 Nuzleaf : 259 Torment : 075 Razor Leaf
            poolSpawn.Spawns.Add(GetTeamMob(274, -1, 259, 075, -1, -1, new RandRange(16)), new IntRange(7, 11), 10);
            //360 Wynaut : 227 Encore : 068 Counter : 243 Mirror Coat
            poolSpawn.Spawns.Add(GetTeamMob(360, -1, 227, 068, 243, -1, new RandRange(17)), new IntRange(7, 11), 10);
            //163 Hoothoot : 115 Reflect : 064 Peck
            poolSpawn.Spawns.Add(GetTeamMob(163, -1, 115, 064, -1, -1, new RandRange(16)), new IntRange(7, 11), 10);
            //074 Geodude : 088 Rock Throw : 111 Defense Curl
            poolSpawn.Spawns.Add(GetTeamMob(074, -1, 088, 111, -1, -1, new RandRange(16)), new IntRange(7, 11), 10);
            //079 Slowpoke : 050 Disable : 055 Water Gun : 093 Confusion
            poolSpawn.Spawns.Add(GetTeamMob(079, -1, 050, 055, 093, -1, new RandRange(17)), new IntRange(7, 11), 10);
            //092 Gastly : 101 Night Shade : 095 Hypnosis
            poolSpawn.Spawns.Add(GetTeamMob(092, -1, 101, 095, -1, -1, new RandRange(19)), new IntRange(9, 13), 10);
            //058 Growlithe : 052 Ember : 046 Roar : 316 Odor Sleuth
            poolSpawn.Spawns.Add(GetTeamMob(058, -1, 052, 046, 316, -1, new RandRange(20), TeamMemberSpawn.MemberRole.Support), new IntRange(9, 13), 10);
            //012 Butterfree : 014 Compound Eyes : 018 Whirlwind : 093 Confusion
            poolSpawn.Spawns.Add(GetTeamMob(012, 014, 018, 093, -1, -1, new RandRange(22)), new IntRange(11, 15), 10);
            //056 Mankey : 069 Seismic Toss : 103 Screech
            poolSpawn.Spawns.Add(GetTeamMob(056, -1, 069, 103, -1, -1, new RandRange(23)), new IntRange(11, 15), 10);
            //397 Staravia : 104 Double Team : 018 Whirlwind : 017 Wing Attack
            poolSpawn.Spawns.Add(GetTeamMob(397, -1, 104, 018, 017, -1, new RandRange(23)), new IntRange(11, 15), 10);
            //216 Teddiursa : 230 Sweet Scent : 343 Covet : 154 Fury Swipes
            poolSpawn.Spawns.Add(GetTeamMob(216, -1, 230, 343, 154, -1, new RandRange(22)), new IntRange(11, 15), 10);
            //083 Farfetch'd : 128 Defiant : 332 Aerial Ace : 282 Knock Off
            poolSpawn.Spawns.Add(GetTeamMob(083, 128, 332, 282, -1, -1, new RandRange(23)), new IntRange(11, 15), 10);
            //180 Flaaffy : 178 Cotton Spore : 268 Charge : 486 Electro Ball
            poolSpawn.Spawns.Add(GetTeamMob(180, -1, 178, 268, 486, -1, new RandRange(25)), new IntRange(13, 17), 10);
            //227 Skarmory : 191 Spikes : 232 Metal Claw
            poolSpawn.Spawns.Add(GetTeamMob(227, -1, 191, 232, -1, -1, new RandRange(28), TeamMemberSpawn.MemberRole.Leader), new IntRange(13, 17), 10);
            //210 Granbull : 155 Rattled : 424 Fire Fang : 422 Thunder Fang : 423 Ice Fang : 184 Scary Face
            poolSpawn.Spawns.Add(GetTeamMob(210, 155, 424, 422, 423, 184, new RandRange(26), TeamMemberSpawn.MemberRole.Leader), new IntRange(13, 17), 10);
            //075 Graveler : 222 Magnitude : 205 Rollout
            poolSpawn.Spawns.Add(GetTeamMob(075, -1, 222, 205, -1, -1, new RandRange(25), TeamMemberSpawn.MemberRole.Loner), new IntRange(13, 17), 10);
            //188 Skiploom : 073 Leech Seed : 072 Mega Drain : 235 Synthesis
            poolSpawn.Spawns.Add(GetTeamMob(188, -1, 073, 072, 235, -1, new RandRange(26)), new IntRange(13, 17), 10);
            //310 Manectric : 604 Electric Terrain : 209 Spark
            poolSpawn.Spawns.Add(GetTeamMob(310, -1, 604, 209, -1, -1, new RandRange(26)), new IntRange(13, 17), 10);
            //015 Beedrill : 390 Toxic Spikes : 228 Pursuit : 041 Twineedle
            poolSpawn.Spawns.Add(GetTeamMob(015, -1, 390, 228, 041, -1, new RandRange(25)), new IntRange(13, 17), 10);
            //413 Wormadam : 107 Anticipation : 075 Razor Leaf : 074 Growth
            poolSpawn.Spawns.Add(GetTeamMob(413, 107, 075, 074, -1, -1, new RandRange(29), TeamMemberSpawn.MemberRole.Leader, 25), new IntRange(15, 19), 5);
            //413 Wormadam : 107 Anticipation : 350 Rock Blast : 106 Harden
            poolSpawn.Spawns.Add(GetTeamMob(new MonsterID(413, 1, -1, Gender.Unknown), 107, 350, 106, -1, -1, new RandRange(29), TeamMemberSpawn.MemberRole.Leader, 25), new IntRange(15, 19), 5);
            //413 Wormadam : 107 Anticipation : 429 Mirror Shot : 319 Metal Sound
            poolSpawn.Spawns.Add(GetTeamMob(new MonsterID(413, 2, -1, Gender.Unknown), 107, 429, 319, -1, -1, new RandRange(29), TeamMemberSpawn.MemberRole.Leader, 25), new IntRange(15, 19), 5);
            //391 Monferno : 172 Flame Wheel : 259 Torment
            poolSpawn.Spawns.Add(GetTeamMob(391, -1, 172, 259, -1, -1, new RandRange(29)), new IntRange(15, 19), 10);
            //213 Shuckle : 227 Encore : 522 Struggle Bug
            poolSpawn.Spawns.Add(GetTeamMob(213, -1, 227, 522, -1, -1, new RandRange(29)), new IntRange(15, 19), 10);
            //067 Machoke : 116 Focus Energy : 490 Low Sweep
            poolSpawn.Spawns.Add(GetTeamMob(067, -1, 116, 490, -1, -1, new RandRange(29)), new IntRange(15, 19), 10);
            //093 Haunter : 212 Mean Look : 101 Night Shade
            poolSpawn.Spawns.Add(GetTeamMob(093, -1, 212, 101, -1, -1, new RandRange(29)), new IntRange(15, 19), 10);
            //051 Dugtrio : 071 Arena Trap : 389 Sucker Punch : 523 Bulldoze
            poolSpawn.Spawns.Add(GetTeamMob(051, 071, 389, 523, -1, -1, new RandRange(29)), new IntRange(15, 19), 10);
            //207 Gligar : 282 Knock Off : 512 Acrobatics
            poolSpawn.Spawns.Add(GetTeamMob(207, -1, 282, 512, -1, -1, new RandRange(29)), new IntRange(15, 19), 10);
            //064 Kadabra : 477 Telekinesis : 060 Psybeam
            poolSpawn.Spawns.Add(GetTeamMob(064, -1, 477, 060, -1, -1, new RandRange(33)), new IntRange(17, 21), 10);
            //113 Chansey : 516 Bestow : 505 Heal Pulse
            poolSpawn.Spawns.Add(GetTeamMob(113, -1, 516, 505, -1, -1, new RandRange(33), TeamMemberSpawn.MemberRole.Support), new IntRange(17, 21), 5);
            //326 Grumpig : 277 Magic Coat : 149 Psywave : 109 Confuse Ray
            poolSpawn.Spawns.Add(GetTeamMob(326, -1, 277, 149, 109, -1, new RandRange(33)), new IntRange(17, 21), 10);
            //181 Ampharos : 602 Magnetic Flux : 406 Dragon Pulse : 486 Electro Ball
            poolSpawn.Spawns.Add(GetTeamMob(181, -1, 602, 406, 486, -1, new RandRange(33)), new IntRange(17, 21), 10);
            //059 Arcanine : 424 Fire Fang : 245 Extreme Speed : 514 Retaliate
            poolSpawn.Spawns.Add(GetTeamMob(059, -1, 424, 245, 514, -1, new RandRange(33)), new IntRange(17, 21), 10);
            //162 Furret : 193 Foresight : 266 Follow Me : 156 Rest
            poolSpawn.Spawns.Add(GetTeamMob(162, -1, 193, 266, 156, -1, new RandRange(36), TeamMemberSpawn.MemberRole.Support), new IntRange(17, 21), 10);
            //189 Jumpluff : 476 Rage Powder : 369 U-turn : 235 Synthesis
            poolSpawn.Spawns.Add(GetTeamMob(189, -1, 476, 369, 235, -1, new RandRange(37), TeamMemberSpawn.MemberRole.Support), new IntRange(19, 23), 10);
            //044 Gloom : 236 Moonlight : 381 Lucky Chant : 072 Mega Drain
            poolSpawn.Spawns.Add(GetTeamMob(044, -1, 236, 381, 072, -1, new RandRange(36), TeamMemberSpawn.MemberRole.Support), new IntRange(19, 23), 10);
            //192 Sunflora : 074 Growth : 076 Solar Beam
            poolSpawn.Spawns.Add(GetTeamMob(192, -1, 074, 076, -1, -1, new RandRange(36)), new IntRange(19, 23), 15);
            //182 Bellossom : 241 Sunny Day : 345 Magical Leaf
            poolSpawn.Spawns.Add(GetTeamMob(182, -1, 241, 345, -1, -1, new RandRange(36)), new IntRange(19, 23), 15);
            //301 Delcatty : 096 Normalize : 274 Assist : 047 Sing
            poolSpawn.Spawns.Add(GetTeamMob(301, 096, 274, 047, -1, -1, new RandRange(36), TeamMemberSpawn.MemberRole.Support), new IntRange(19, 23), 10);
            //078 Rapidash : 083 Fire Spin : 517 Inferno
            poolSpawn.Spawns.Add(GetTeamMob(078, -1, 083, 517, -1, -1, new RandRange(40), TeamMemberSpawn.MemberRole.Loner), new IntRange(19, 23), 10);
            //389 Torterra : 452 Wood Hammer : 089 Earthquake : 235 Synthesis
            poolSpawn.Spawns.Add(GetTeamMob(389, -1, 452, 089, 235, -1, new RandRange(36)), new IntRange(19, 23), 10);
            //398 Staraptor : 097 Agility : 515 Final Gambit : 370 Close Combat
            poolSpawn.Spawns.Add(GetTeamMob(398, -1, 097, 515, 370, -1, new RandRange(39)), new IntRange(21, 25), 10);
            //062 Poliwrath : 358 Wake-Up Slap : 095 Hypnosis
            poolSpawn.Spawns.Add(GetTeamMob(062, -1, 358, 095, -1, -1, new RandRange(39)), new IntRange(21, 25), 10);
            //337 Lunatone : 478 Magic Room : 585 Moonblast : 157 Rock Slide
            poolSpawn.Spawns.Add(GetTeamMob(337, -1, 478, 585, 157, -1, new RandRange(39)), new IntRange(21, 25), 10);
            //164 Noctowl : 115 Reflect : 138 Dream Eater : 355 Roost
            poolSpawn.Spawns.Add(GetTeamMob(164, -1, 115, 138, 355, -1, new RandRange(39), TeamMemberSpawn.MemberRole.Support), new IntRange(21, 25), 10);
            //094 Gengar : 247 Shadow Ball : 180 Spite : 138 Dream Eater
            poolSpawn.Spawns.Add(GetTeamMob(094, -1, 247, 180, 138, -1, new RandRange(39)), new IntRange(21, 25), 10);
            //057 Primeape : 386 Punishment : 238 Cross Chop
            poolSpawn.Spawns.Add(GetTeamMob(057, -1, 386, 238, -1, -1, new RandRange(42)), new IntRange(21, 25), 10);
            //302 Sableye : 212 Mean Look : 282 Knock Off
            poolSpawn.Spawns.Add(GetTeamMob(302, -1, 212, 282, -1, -1, new RandRange(42)), new IntRange(23, 28), 10);
            //186 Politoed : 195 Perish Song : 207 Swagger
            poolSpawn.Spawns.Add(GetTeamMob(186, -1, 195, 207, -1, -1, new RandRange(42), TeamMemberSpawn.MemberRole.Loner), new IntRange(23, 28), 20);
            //065 Alakazam : 105 Recover : 094 Psychic
            poolSpawn.Spawns.Add(GetTeamMob(065, -1, 105, 094, -1, -1, new RandRange(42)), new IntRange(23, 28), 10);
            //080 Slowbro : 505 Heal Pulse : 133 Amnesia : 352 Water Pulse
            poolSpawn.Spawns.Add(GetTeamMob(080, -1, 505, 133, 352, -1, new RandRange(45)), new IntRange(25, 28), 10);
            //068 Machamp : 099 No Guard : 223 Dynamic Punch : 530 Dual Chop
            poolSpawn.Spawns.Add(GetTeamMob(068, 099, 223, 530, -1, -1, new RandRange(45)), new IntRange(25, 28), 10);
            //136 Flareon : 083 Fire Spin : 436 Lava Plume
            poolSpawn.Spawns.Add(GetTeamMob(136, -1, 083, 436, -1, -1, new RandRange(45)), new IntRange(25, 28), 10);
            //202 Wobbuffet : 219 Safeguard : 068 Counter : 243 Mirror Coat : 227 Encore
            poolSpawn.Spawns.Add(GetTeamMob(202, -1, 219, 068, 243, 227, new RandRange(45), TeamMemberSpawn.MemberRole.Loner), new IntRange(25, 28), 10);
            //213 Shuckle : 379 Power Trick : 205 Rollout
            poolSpawn.Spawns.Add(GetTeamMob(213, -1, 379, 205, -1, -1, new RandRange(45), TeamMemberSpawn.MemberRole.Loner), new IntRange(25, 31), 10);
            //338 Solrock : 472 Wonder Room : 083 Fire Spin : 397 Rock Polish
            poolSpawn.Spawns.Add(GetTeamMob(338, -1, 472, 083, 397, -1, new RandRange(45)), new IntRange(25, 31), 15);
            //461 Weavile : 251 Beat Up : 386 Punishment : 196 Icy Wind
            poolSpawn.Spawns.Add(GetTeamMob(461, -1, 251, 386, 196, -1, new RandRange(47), TeamMemberSpawn.MemberRole.Loner), new IntRange(25, 31), 10);
            //392 Infernape : 370 Close Combat : 394 Flare Blitz
            poolSpawn.Spawns.Add(GetTeamMob(392, -1, 370, 394, -1, -1, new RandRange(47)), new IntRange(26, 31), 10);
            //413 Wormadam : 107 Anticipation : 319 Metal Sound : 445 Captivate : 213 Attract
            poolSpawn.Spawns.Add(GetTeamMob(new MonsterID(413, 2, -1, Gender.Unknown), 107, 319, 445, 213, -1, new RandRange(48), TeamMemberSpawn.MemberRole.Loner, 25), new IntRange(28, 35), 5);
            //413 Wormadam : 107 Anticipation : 437 Leaf Storm : 445 Captivate : 213 Attract
            poolSpawn.Spawns.Add(GetTeamMob(413, 107, 437, 445, 213, -1, new RandRange(48), TeamMemberSpawn.MemberRole.Loner, 25), new IntRange(28, 35), 5);
            //217 Ursaring : 095 Quick Feet : 359 Hammer Arm : 230 Sweet Scent
            poolSpawn.Spawns.Add(GetTeamMob(217, 095, 359, 230, -1, -1, new RandRange(48)), new IntRange(26, 31), 10);
            //024 Arbok : 114 Haze : 380 Gastro Acid : 254 Stockpile : 242 Crunch
            poolSpawn.Spawns.Add(GetTeamMob(024, -1, 114, 380, 254, 242, new RandRange(48), TeamMemberSpawn.MemberRole.Support), new IntRange(28, 31), 20);
            //185 Sudowoodo : 068 Counter : 452 Wood Hammer
            //make this spawn at doorsteps
            poolSpawn.Spawns.Add(GetTeamMob(185, -1, 068, 452, -1, -1, new RandRange(48), TeamMemberSpawn.MemberRole.Loner, 10), new IntRange(28, 31), 10);
            //469 Yanmega : 003 Speed Boost : 048 Supersonic : 246 Ancient Power
            poolSpawn.Spawns.Add(GetTeamMob(469, 003, 048, 246, -1, -1, new RandRange(48)), new IntRange(31, 35), 10);
            //334 Altaria : 434 Draco Meteor : 219 Safeguard : 363 Natural Gift
            //seek berries on the map
            poolSpawn.Spawns.Add(GetTeamMob(334, -1, 434, 219, 363, -1, new RandRange(48), TeamMemberSpawn.MemberRole.Leader), new IntRange(26, 31), 10);
            //357 Tropius : 139 Harvest : 363 Natural Gift : 437 Leaf Storm
            //seek berries on the map
            poolSpawn.Spawns.Add(GetTeamMob(357, 139, 363, 437, -1, -1, new RandRange(48), TeamMemberSpawn.MemberRole.Leader), new IntRange(26, 31), 10);
            //414 Mothim : 110 Tinted Lens : 483 Quiver Dance : 318 Silver Wind
            poolSpawn.Spawns.Add(GetTeamMob(414, 110, 483, 318, -1, -1, new RandRange(51)), new IntRange(31, 35), 10);
            //395 Empoleon : 128 Defiant : 065 Drill Peck : 453 Aqua Jet : 014 Swords Dance
            poolSpawn.Spawns.Add(GetTeamMob(395, 128, 065, 453, 014, -1, new RandRange(51)), new IntRange(31, 35), 10);
            //076 Golem : 089 Earthquake : 300 Mud Sport : 484 Heavy Slam : 205 Rollout
            poolSpawn.Spawns.Add(GetTeamMob(076, -1, 089, 300, 484, 205, new RandRange(51)), new IntRange(31, 35), 10);
            //162 Furret : 226 Baton Pass : 133 Amnesia : 156 Rest : 266 Follow Me
            poolSpawn.Spawns.Add(GetTeamMob(162, -1, 226, 133, 156, 266, new RandRange(51), TeamMemberSpawn.MemberRole.Support), new IntRange(31, 35), 10);
            //045 Vileplume : 580 Grassy Terrain : 572 Petal Blizzard : 078 Stun Spore
            poolSpawn.Spawns.Add(GetTeamMob(045, -1, 580, 572, 078, -1, new RandRange(51)), new IntRange(31, 35), 10);
            //181 Ampharos : 406 Dragon Pulse : 192 Zap Cannon : 324 Signal Beam : 178 Cotton Spore
            poolSpawn.Spawns.Add(GetTeamMob(181, -1, 406, 192, 324, 178, new RandRange(54)), new IntRange(31, 40), 10);
            //472 Gliscor : 103 Screech : 512 Acrobatics : 423 Ice Fang
            poolSpawn.Spawns.Add(GetTeamMob(472, -1, 103, 512, 423, -1, new RandRange(54)), new IntRange(31, 40), 10);
            //189 Jumpluff : 079 Sleep Powder : 262 Memento : 235 Synthesis
            poolSpawn.Spawns.Add(GetTeamMob(189, -1, 079, 262, 235, -1, new RandRange(57), TeamMemberSpawn.MemberRole.Support), new IntRange(35, 40), 10);
            //302 Sableye : 158 Prankster : 511 Quash : 109 Confuse Ray : 492 Foul Play : 193 Foresight
            poolSpawn.Spawns.Add(GetTeamMob(302, 158, 511, 109, 492, 193, new RandRange(57)), new IntRange(35, 40), 10);
            //062 Poliwrath : 187 Belly Drum : 358 Wake-Up Slap : 095 Hypnosis
            poolSpawn.Spawns.Add(GetTeamMob(062, -1, 187, 358, 095, -1, new RandRange(57)), new IntRange(35, 40), 10);
            //310 Manectric : 604 Electric Terrain : 435 Discharge : 424 Fire Fang
            poolSpawn.Spawns.Add(GetTeamMob(310, -1, 604, 435, 424, -1, new RandRange(57)), new IntRange(35, 40), 10);


            //extra spawns here


            {
                //032 Nidoran♂ : 079 Rivalry : 043 Leer : 064 Peck
                TeamMemberSpawn teamSpawn = GetTeamMob(032, 079, 043, 064, -1, -1, new RandRange(6));
                teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(0, 2));
                poolSpawn.Spawns.Add(teamSpawn, new IntRange(1, 5), 10);
            }
            {
                //029 Nidoran♀ : 079 Rivalry : 045 Growl : 010 Scratch
                TeamMemberSpawn teamSpawn = GetTeamMob(029, 079, 045, 010, -1, -1, new RandRange(6));
                teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(1, 2));
                poolSpawn.Spawns.Add(teamSpawn, new IntRange(1, 5), 10);
            }
            {
                //311 Plusle : 589 Play Nice : 270 Helping Hand : 486 Electro Ball
                TeamMemberSpawn teamSpawn = GetTeamMob(311, -1, 589, 270, 486, -1, new RandRange(26));
                teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(0, 2));
                poolSpawn.Spawns.Add(teamSpawn, new IntRange(13, 17), 5);
            }
            {
                //312 Minun : 313 Fake Tears : 270 Helping Hand : 609 Nuzzle
                TeamMemberSpawn teamSpawn = GetTeamMob(312, -1, 313, 270, 609, -1, new RandRange(26));
                teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(1, 2));
                poolSpawn.Spawns.Add(teamSpawn, new IntRange(13, 17), 5);
            }
            {
                //033 Nidorino : 038 Poison Point : 270 Helping Hand : 024 Double Kick : 040 Poison Sting
                TeamMemberSpawn teamSpawn = GetTeamMob(033, 038, 270, 024, 040, -1, new RandRange(29), TeamMemberSpawn.MemberRole.Leader);
                teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(0, 2));
                poolSpawn.Spawns.Add(teamSpawn, new IntRange(15, 19), 5);
            }
            {
                //030 Nidorina : 038 Poison Point : 270 Helping Hand : 044 Bite : 040 Poison Sting
                TeamMemberSpawn teamSpawn = GetTeamMob(030, 038, 270, 044, 040, -1, new RandRange(29), TeamMemberSpawn.MemberRole.Leader);
                teamSpawn.Spawn.SpawnConditions.Add(new MobCheckVersionDiff(1, 2));
                poolSpawn.Spawns.Add(teamSpawn, new IntRange(15, 19), 5);
            }



            poolSpawn.TeamSizes.Add(1, new IntRange(0, max_floors), 12);
            poolSpawn.TeamSizes.Add(2, new IntRange(8, 12), 3);
            poolSpawn.TeamSizes.Add(2, new IntRange(12, 20), 6);
            poolSpawn.TeamSizes.Add(2, new IntRange(20, max_floors), 8);

            poolSpawn.TeamSizes.Add(3, new IntRange(24, max_floors), 3);
            poolSpawn.TeamSizes.Add(3, new IntRange(27, max_floors), 4);
            floorSegment.ZoneSteps.Add(poolSpawn);


            TileSpawnZoneStep tileSpawn = new TileSpawnZoneStep();
            tileSpawn.Priority = PR_RESPAWN_TRAP;
            tileSpawn.Spawns.Add(new EffectTile(7, false), new IntRange(0, max_floors), 10);//mud trap
            tileSpawn.Spawns.Add(new EffectTile(13, true), new IntRange(0, max_floors), 10);//warp trap
            tileSpawn.Spawns.Add(new EffectTile(14, false), new IntRange(0, max_floors), 10);//gust trap
            tileSpawn.Spawns.Add(new EffectTile(17, false), new IntRange(0, max_floors), 10);//chestnut trap
            tileSpawn.Spawns.Add(new EffectTile(3, false), new IntRange(0, max_floors), 10);//poison trap
            tileSpawn.Spawns.Add(new EffectTile(4, false), new IntRange(0, max_floors), 10);//sleep trap
            tileSpawn.Spawns.Add(new EffectTile(11, false), new IntRange(0, max_floors), 10);//sticky trap
            tileSpawn.Spawns.Add(new EffectTile(8, false), new IntRange(0, max_floors), 10);//seal trap
            tileSpawn.Spawns.Add(new EffectTile(18, false), new IntRange(0, max_floors), 10);//selfdestruct trap
            tileSpawn.Spawns.Add(new EffectTile(23, true), new IntRange(0, 15), 10);//trip trap
            tileSpawn.Spawns.Add(new EffectTile(23, false), new IntRange(15, max_floors), 10);//trip trap
            tileSpawn.Spawns.Add(new EffectTile(25, true), new IntRange(0, max_floors), 10);//hunger trap
            tileSpawn.Spawns.Add(new EffectTile(12, true), new IntRange(0, 15), 3);//apple trap
            tileSpawn.Spawns.Add(new EffectTile(12, false), new IntRange(15, max_floors), 3);//apple trap
            tileSpawn.Spawns.Add(new EffectTile(9, true), new IntRange(0, max_floors), 10);//pp-leech trap
            tileSpawn.Spawns.Add(new EffectTile(15, false), new IntRange(0, max_floors), 10);//summon trap
            tileSpawn.Spawns.Add(new EffectTile(19, false), new IntRange(0, max_floors), 10);//explosion trap
            tileSpawn.Spawns.Add(new EffectTile(6, false), new IntRange(0, max_floors), 10);//slow trap
            tileSpawn.Spawns.Add(new EffectTile(5, false), new IntRange(0, max_floors), 10);//spin trap
            tileSpawn.Spawns.Add(new EffectTile(10, false), new IntRange(0, max_floors), 10);//grimy trap
            tileSpawn.Spawns.Add(new EffectTile(28, true), new IntRange(0, max_floors), 20);//trigger trap
                                                                                      //pokemon trap
            tileSpawn.Spawns.Add(new EffectTile(24, true), new IntRange(15, max_floors), 10);//grudge trap
                                                                                      //training switch
            floorSegment.ZoneSteps.Add(tileSpawn);


            SpawnList<IGenPriority> appleZoneSpawns = new SpawnList<IGenPriority>();
            appleZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(1))))), 10);
            SpreadStepZoneStep appleZoneStep = new SpreadStepZoneStep(new SpreadPlanSpaced(new RandRange(3, 5), new IntRange(0, max_floors)), appleZoneSpawns);//apple
            floorSegment.ZoneSteps.Add(appleZoneStep);

            SpawnList<IGenPriority> leppaZoneSpawns = new SpawnList<IGenPriority>();
            leppaZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(11))))), 10);
            SpreadStepZoneStep leppaZoneStep = new SpreadStepZoneStep(new SpreadPlanSpaced(new RandRange(3, 6), new IntRange(0, max_floors)), appleZoneSpawns);//leppa
            floorSegment.ZoneSteps.Add(leppaZoneStep);

            SpawnList<IGenPriority> assemblyZoneSpawns = new SpawnList<IGenPriority>();
            assemblyZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(451))))), 10);
            SpreadStepZoneStep assemblyZoneStep = new SpreadStepZoneStep(new SpreadPlanSpaced(new RandRange(3, 7), new IntRange(4, max_floors)), appleZoneSpawns);//assembly box
            floorSegment.ZoneSteps.Add(assemblyZoneStep);

            {
                SpawnList<IGenPriority> apricornZoneSpawns = new SpawnList<IGenPriority>();
                apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(211))))), 10);//blue apricorns
                apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(212))))), 10);//green apricorns
                apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(213))))), 10);//brown apricorns
                apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(214))))), 10);//purple apricorns
                apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(215))))), 10);//red apricorns
                apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(216))))), 10);//white apricorns
                apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(217))))), 10);//yellow apricorns
                apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(218))))), 10);//black apricorns
                SpreadStepZoneStep apricornZoneStep = new SpreadStepZoneStep(new SpreadPlanSpaced(new RandRange(4, 7), new IntRange(0, 21)), apricornZoneSpawns);//apricorn (variety)
                floorSegment.ZoneSteps.Add(apricornZoneStep);
            }

            int max_apricorn = 20;
            for (int jj = 0; jj < 4; jj++)
            {
                SpawnList<IGenPriority> apricornZoneSpawns = new SpawnList<IGenPriority>();
                apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(211))))), 10);//blue apricorns
                apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(212))))), 10);//green apricorns
                apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(213))))), 10);//brown apricorns
                apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(214))))), 10);//purple apricorns
                apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(215))))), 10);//red apricorns
                apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(216))))), 10);//white apricorns
                apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(217))))), 10);//yellow apricorns
                apricornZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(218))))), 10);//black apricorns
                SpreadStepZoneStep apricornZoneStep = new SpreadStepZoneStep(new SpreadPlanQuota(new RandRange(1), new IntRange(0, max_apricorn)), apricornZoneSpawns);//apricorn (variety)
                floorSegment.ZoneSteps.Add(apricornZoneStep);
                max_apricorn /= 2;
            }

            SpawnList<IGenPriority> cleanseZoneSpawns = new SpawnList<IGenPriority>();
            cleanseZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(263))))), 10);
            SpreadStepZoneStep cleanseZoneStep = new SpreadStepZoneStep(new SpreadPlanQuota(new RandDecay(2, 10, 60), new IntRange(3, max_floors)), cleanseZoneSpawns);//cleanse orb
            floorSegment.ZoneSteps.Add(cleanseZoneStep);

            SpawnList<IGenPriority> evoZoneSpawns = new SpawnList<IGenPriority>();
            SpreadStepZoneStep evoItemZoneStep = new SpreadStepZoneStep(new SpreadPlanQuota(new RandRange(1, 4), new IntRange(0, 15)), evoZoneSpawns);//evo items
            evoZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(351))))), 10);//Fire Stone
            evoZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(354))))), 10);//Leaf Stone
            evoZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(353))))), 10);//Water Stone
            evoZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(355))))), 10);//Moon Stone
            evoZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(356))))), 10);//Sun Stone
            evoZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(374))))), 10);//King's Rock
            evoZoneSpawns.Add(new GenPriority<GenStep<BaseMapGenContext>>(PR_SPAWN_ITEMS, new RandomSpawnStep<BaseMapGenContext, MapItem>(new PickerSpawner<BaseMapGenContext, MapItem>(new PresetMultiRand<MapItem>(new MapItem(365))))), 10);//Link Cable
            floorSegment.ZoneSteps.Add(evoItemZoneStep);


            SpreadRoomZoneStep evoZoneStep = new SpreadRoomZoneStep(PR_GRID_GEN_EXTRA, PR_ROOMS_GEN_EXTRA, new SpreadPlanSpaced(new RandRange(2, 5), new IntRange(3, max_floors)));
            List<BaseRoomFilter> evoFilters = new List<BaseRoomFilter>();
            evoFilters.Add(new RoomFilterComponent(true, new ImmutableRoom()));
            evoFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
            evoZoneStep.Spawns.Add(new RoomGenOption(new RoomGenEvoSmall<MapGenContext>(), new RoomGenEvoSmall<ListMapGenContext>(), evoFilters), 10);
            floorSegment.ZoneSteps.Add(evoZoneStep);


            {
                //monster houses
                SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(10, new IntRange(3, 30)));
                monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHouseStep<ListMapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                for (int ii = 0; ii < 18; ii++)
                    monsterChanceZoneStep.Items.Add(new MapItem(76 + ii), new IntRange(0, max_floors), 4);//gummis
                for (int ii = 0; ii < 8; ii++)
                    monsterChanceZoneStep.Items.Add(new MapItem(210 + ii), new IntRange(0, max_floors), 4);//apricorns
                monsterChanceZoneStep.Items.Add(new MapItem(351), new IntRange(0, max_floors), 4);//Fire Stone
                monsterChanceZoneStep.Items.Add(new MapItem(353), new IntRange(0, max_floors), 4);//Water Stone
                monsterChanceZoneStep.Items.Add(new MapItem(355), new IntRange(0, max_floors), 4);//Moon Stone
                monsterChanceZoneStep.Items.Add(new MapItem(354), new IntRange(0, max_floors), 4);//Leaf Stone
                monsterChanceZoneStep.Items.Add(new MapItem(374), new IntRange(0, max_floors), 4);//King's Rock
                monsterChanceZoneStep.Items.Add(new MapItem(365), new IntRange(0, max_floors), 4);//Link Cable
                monsterChanceZoneStep.Items.Add(new MapItem(356), new IntRange(0, max_floors), 4);//Sun Stone
                monsterChanceZoneStep.Items.Add(new MapItem(6), new IntRange(0, max_floors), 25);//banana
                for (int ii = 0; ii < specific_tms.Length; ii++)
                    monsterChanceZoneStep.Items.Add(new MapItem(specific_tms[ii][0]), new IntRange(0, max_floors), 2);//TMs
                monsterChanceZoneStep.Items.Add(new MapItem(477), new IntRange(0, max_floors), 10);//nugget
                monsterChanceZoneStep.Items.Add(new MapItem(480, 1), new IntRange(0, max_floors), 10);//pearl
                monsterChanceZoneStep.Items.Add(new MapItem(481, 2), new IntRange(0, max_floors), 10);//heart scale
                monsterChanceZoneStep.Items.Add(new MapItem(455, 1), new IntRange(0, max_floors), 10);//key
                monsterChanceZoneStep.Items.Add(new MapItem(450), new IntRange(0, max_floors), 30);//link box
                monsterChanceZoneStep.Items.Add(new MapItem(451), new IntRange(0, max_floors), 30);//assembly box
                monsterChanceZoneStep.Items.Add(new MapItem(453), new IntRange(0, max_floors), 10);//ability capsule


                monsterChanceZoneStep.Items.Add(new MapItem(0303), new IntRange(0, max_floors), 1);//Mobile Scarf
                monsterChanceZoneStep.Items.Add(new MapItem(0304), new IntRange(0, max_floors), 1);//Pass Scarf
                monsterChanceZoneStep.Items.Add(new MapItem(0330), new IntRange(0, max_floors), 1);//Cover Band
                monsterChanceZoneStep.Items.Add(new MapItem(0329), new IntRange(0, max_floors), 1);//Reunion Cape
                monsterChanceZoneStep.Items.Add(new MapItem(0306), new IntRange(0, max_floors), 1);//Trap Scarf
                monsterChanceZoneStep.Items.Add(new MapItem(0307), new IntRange(0, max_floors), 1);//Grip Claw
                monsterChanceZoneStep.Items.Add(new MapItem(0309), new IntRange(0, max_floors), 1);//Twist Band
                monsterChanceZoneStep.Items.Add(new MapItem(0310), new IntRange(0, max_floors), 1);//Metronome
                monsterChanceZoneStep.Items.Add(new MapItem(0312), new IntRange(0, max_floors), 1);//Shell Bell
                monsterChanceZoneStep.Items.Add(new MapItem(0313), new IntRange(0, max_floors), 1);//Scope Lens
                monsterChanceZoneStep.Items.Add(new MapItem(0400), new IntRange(0, max_floors), 1);//Power Band
                monsterChanceZoneStep.Items.Add(new MapItem(0401), new IntRange(0, max_floors), 1);//Special Band
                monsterChanceZoneStep.Items.Add(new MapItem(0402), new IntRange(0, max_floors), 1);//Defense Scarf
                monsterChanceZoneStep.Items.Add(new MapItem(0403), new IntRange(0, max_floors), 1);//Zinc Band
                monsterChanceZoneStep.Items.Add(new MapItem(0314), new IntRange(0, max_floors), 1);//Wide Lens
                monsterChanceZoneStep.Items.Add(new MapItem(0301), new IntRange(0, max_floors), 1);//Pierce Band
                monsterChanceZoneStep.Items.Add(new MapItem(0311), new IntRange(0, max_floors), 1);//Shed Shell
                monsterChanceZoneStep.Items.Add(new MapItem(0328), new IntRange(0, max_floors), 1);//X-Ray Specs
                monsterChanceZoneStep.Items.Add(new MapItem(0404), new IntRange(0, max_floors), 1);//Big Root
                monsterChanceZoneStep.Items.Add(new MapItem(0406), new IntRange(0, max_floors), 1);//Weather Rock
                monsterChanceZoneStep.Items.Add(new MapItem(0405), new IntRange(0, max_floors), 1);//Expert Belt
                monsterChanceZoneStep.Items.Add(new MapItem(0320), new IntRange(0, max_floors), 1);//Choice Scarf
                monsterChanceZoneStep.Items.Add(new MapItem(0319), new IntRange(0, max_floors), 1);//Choice Specs
                monsterChanceZoneStep.Items.Add(new MapItem(0318), new IntRange(0, max_floors), 1);//Choice Band
                monsterChanceZoneStep.Items.Add(new MapItem(0321), new IntRange(0, max_floors), 1);//Assault Vest
                monsterChanceZoneStep.Items.Add(new MapItem(0322), new IntRange(0, max_floors), 1);//Life Orb
                monsterChanceZoneStep.Items.Add(new MapItem(0315), new IntRange(0, max_floors), 1);//Heal Ribbon

                //monsterChanceZoneStep.ItemThemes.Add(new ItemThemeNone(0, new RandRange(5, 11)), new ParamRange(0, max_floors), 20);
                monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(1, 4)), new ItemThemeNone(80, new RandRange(2, 4))), new IntRange(0, max_floors), 30);//no theme
                monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeType(ItemData.UseType.Learn, false, true, new RandRange(3, 5)),
                    new ItemThemeRange(new IntRange(450, 454), true, true, new RandRange(0, 2))), new IntRange(0, 30), 10);//TMs + machines
                monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(3, 7)), new IntRange(0, max_floors), 30);//gummis
                monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(1, 4)), new ItemThemeRange(new IntRange(351, 380), true, true, new RandRange(2, 4))), new IntRange(0, 10), 20);//evo items
                monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(1, 4)), new ItemThemeRange(new IntRange(351, 380), true, true, new RandRange(2, 4))), new IntRange(10, 20), 10);//evo items
                monsterChanceZoneStep.MobThemes.Add(new MobThemeNone(0, new RandRange(7, 13)), new IntRange(0, max_floors), 10);
                floorSegment.ZoneSteps.Add(monsterChanceZoneStep);
            }

            {
                SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(10, new IntRange(5, 20)));
                monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHallStep<ListMapGenContext>(new Loc(11, 9), GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHallStep<ListMapGenContext>(new Loc(15, 13), GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);

                for (int ii = 0; ii < 18; ii++)
                    monsterChanceZoneStep.Items.Add(new MapItem(76 + ii), new IntRange(0, max_floors), 4);//gummis
                for (int ii = 0; ii < 8; ii++)
                    monsterChanceZoneStep.Items.Add(new MapItem(210 + ii), new IntRange(0, max_floors), 4);//apricorns
                monsterChanceZoneStep.Items.Add(new MapItem(351), new IntRange(0, max_floors), 4);//Fire Stone
                monsterChanceZoneStep.Items.Add(new MapItem(353), new IntRange(0, max_floors), 4);//Water Stone
                monsterChanceZoneStep.Items.Add(new MapItem(355), new IntRange(0, max_floors), 4);//Moon Stone
                monsterChanceZoneStep.Items.Add(new MapItem(354), new IntRange(0, max_floors), 4);//Leaf Stone
                monsterChanceZoneStep.Items.Add(new MapItem(374), new IntRange(0, max_floors), 4);//King's Rock
                monsterChanceZoneStep.Items.Add(new MapItem(365), new IntRange(0, max_floors), 4);//Link Cable
                monsterChanceZoneStep.Items.Add(new MapItem(356), new IntRange(0, max_floors), 4);//Sun Stone
                monsterChanceZoneStep.Items.Add(new MapItem(6), new IntRange(0, max_floors), 25);//banana
                for (int ii = 0; ii < specific_tms.Length; ii++)
                    monsterChanceZoneStep.Items.Add(new MapItem(specific_tms[ii][0]), new IntRange(0, max_floors), 2);//TMs
                monsterChanceZoneStep.Items.Add(new MapItem(477), new IntRange(0, max_floors), 10);//nugget
                monsterChanceZoneStep.Items.Add(new MapItem(480, 1), new IntRange(0, max_floors), 10);//pearl
                monsterChanceZoneStep.Items.Add(new MapItem(481, 2), new IntRange(0, max_floors), 10);//heart scale
                monsterChanceZoneStep.Items.Add(new MapItem(455, 1), new IntRange(0, max_floors), 10);//key
                monsterChanceZoneStep.Items.Add(new MapItem(450), new IntRange(0, max_floors), 30);//link box
                monsterChanceZoneStep.Items.Add(new MapItem(451), new IntRange(0, max_floors), 30);//assembly box
                monsterChanceZoneStep.Items.Add(new MapItem(453), new IntRange(0, max_floors), 10);//ability capsule


                monsterChanceZoneStep.Items.Add(new MapItem(0303), new IntRange(0, max_floors), 1);//Mobile Scarf
                monsterChanceZoneStep.Items.Add(new MapItem(0304), new IntRange(0, max_floors), 1);//Pass Scarf
                monsterChanceZoneStep.Items.Add(new MapItem(0330), new IntRange(0, max_floors), 1);//Cover Band
                monsterChanceZoneStep.Items.Add(new MapItem(0329), new IntRange(0, max_floors), 1);//Reunion Cape
                monsterChanceZoneStep.Items.Add(new MapItem(0306), new IntRange(0, max_floors), 1);//Trap Scarf
                monsterChanceZoneStep.Items.Add(new MapItem(0307), new IntRange(0, max_floors), 1);//Grip Claw
                monsterChanceZoneStep.Items.Add(new MapItem(0309), new IntRange(0, max_floors), 1);//Twist Band
                monsterChanceZoneStep.Items.Add(new MapItem(0310), new IntRange(0, max_floors), 1);//Metronome
                monsterChanceZoneStep.Items.Add(new MapItem(0312), new IntRange(0, max_floors), 1);//Shell Bell
                monsterChanceZoneStep.Items.Add(new MapItem(0313), new IntRange(0, max_floors), 1);//Scope Lens
                monsterChanceZoneStep.Items.Add(new MapItem(0400), new IntRange(0, max_floors), 1);//Power Band
                monsterChanceZoneStep.Items.Add(new MapItem(0401), new IntRange(0, max_floors), 1);//Special Band
                monsterChanceZoneStep.Items.Add(new MapItem(0402), new IntRange(0, max_floors), 1);//Defense Scarf
                monsterChanceZoneStep.Items.Add(new MapItem(0403), new IntRange(0, max_floors), 1);//Zinc Band
                monsterChanceZoneStep.Items.Add(new MapItem(0314), new IntRange(0, max_floors), 1);//Wide Lens
                monsterChanceZoneStep.Items.Add(new MapItem(0301), new IntRange(0, max_floors), 1);//Pierce Band
                monsterChanceZoneStep.Items.Add(new MapItem(0311), new IntRange(0, max_floors), 1);//Shed Shell
                monsterChanceZoneStep.Items.Add(new MapItem(0328), new IntRange(0, max_floors), 1);//X-Ray Specs
                monsterChanceZoneStep.Items.Add(new MapItem(0404), new IntRange(0, max_floors), 1);//Big Root
                monsterChanceZoneStep.Items.Add(new MapItem(0406), new IntRange(0, max_floors), 1);//Weather Rock
                monsterChanceZoneStep.Items.Add(new MapItem(0405), new IntRange(0, max_floors), 1);//Expert Belt
                monsterChanceZoneStep.Items.Add(new MapItem(0320), new IntRange(0, max_floors), 1);//Choice Scarf
                monsterChanceZoneStep.Items.Add(new MapItem(0319), new IntRange(0, max_floors), 1);//Choice Specs
                monsterChanceZoneStep.Items.Add(new MapItem(0318), new IntRange(0, max_floors), 1);//Choice Band
                monsterChanceZoneStep.Items.Add(new MapItem(0321), new IntRange(0, max_floors), 1);//Assault Vest
                monsterChanceZoneStep.Items.Add(new MapItem(0322), new IntRange(0, max_floors), 1);//Life Orb
                monsterChanceZoneStep.Items.Add(new MapItem(0315), new IntRange(0, max_floors), 1);//Heal Ribbon

                //monsterChanceZoneStep.ItemThemes.Add(new ItemThemeNone(0, new RandRange(5, 11)), new ParamRange(0, 30), 20);
                monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(1, 4)), new ItemThemeNone(80, new RandRange(2, 4))), new IntRange(0, 30), 30);//no theme
                monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeType(ItemData.UseType.Learn, false, true, new RandRange(3, 5)),
                    new ItemThemeRange(new IntRange(450, 454), true, true, new RandRange(0, 2))), new IntRange(0, 30), 10);//TMs + machines

                monsterChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(3, 7)), new IntRange(0, 30), 30);//gummis
                monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(1, 4)), new ItemThemeRange(new IntRange(351, 380), true, true, new RandRange(2, 4))), new IntRange(0, 10), 20);//evo items
                monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(1, 4)), new ItemThemeRange(new IntRange(351, 380), true, true, new RandRange(2, 4))), new IntRange(10, 20), 10);//evo items
                monsterChanceZoneStep.MobThemes.Add(new MobThemeNone(0, new RandRange(7, 13)), new IntRange(0, max_floors), 10);

                floorSegment.ZoneSteps.Add(monsterChanceZoneStep);
            }

            {
                SpreadHouseZoneStep monsterChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanChance(20, new IntRange(28, max_floors)));
                monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHallStep<ListMapGenContext>(new Loc(11, 9), GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);
                monsterChanceZoneStep.HouseStepSpawns.Add(new MonsterHallStep<ListMapGenContext>(new Loc(15, 13), GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);

                for (int ii = 0; ii < 18; ii++)
                    monsterChanceZoneStep.Items.Add(new MapItem(76 + ii), new IntRange(0, max_floors), 4);//gummis
                monsterChanceZoneStep.Items.Add(new MapItem(6), new IntRange(0, max_floors), 25);//banana
                for (int ii = 0; ii < specific_tms.Length; ii++)
                    monsterChanceZoneStep.Items.Add(new MapItem(specific_tms[ii][0]), new IntRange(0, max_floors), 2);//TMs
                monsterChanceZoneStep.Items.Add(new MapItem(480, 1), new IntRange(0, max_floors), 10);//pearl
                monsterChanceZoneStep.Items.Add(new MapItem(481, 2), new IntRange(0, max_floors), 10);//heart scale
                monsterChanceZoneStep.Items.Add(new MapItem(450), new IntRange(0, max_floors), 30);//link box
                monsterChanceZoneStep.Items.Add(new MapItem(451), new IntRange(0, max_floors), 30);//assembly box
                monsterChanceZoneStep.Items.Add(new MapItem(453), new IntRange(0, max_floors), 10);//ability capsule


                monsterChanceZoneStep.Items.Add(new MapItem(0303), new IntRange(0, max_floors), 1);//Mobile Scarf
                monsterChanceZoneStep.Items.Add(new MapItem(0304), new IntRange(0, max_floors), 1);//Pass Scarf
                monsterChanceZoneStep.Items.Add(new MapItem(0330), new IntRange(0, max_floors), 1);//Cover Band
                monsterChanceZoneStep.Items.Add(new MapItem(0329), new IntRange(0, max_floors), 1);//Reunion Cape
                monsterChanceZoneStep.Items.Add(new MapItem(0306), new IntRange(0, max_floors), 1);//Trap Scarf
                monsterChanceZoneStep.Items.Add(new MapItem(0307), new IntRange(0, max_floors), 1);//Grip Claw
                monsterChanceZoneStep.Items.Add(new MapItem(0309), new IntRange(0, max_floors), 1);//Twist Band
                monsterChanceZoneStep.Items.Add(new MapItem(0310), new IntRange(0, max_floors), 1);//Metronome
                monsterChanceZoneStep.Items.Add(new MapItem(0312), new IntRange(0, max_floors), 1);//Shell Bell
                monsterChanceZoneStep.Items.Add(new MapItem(0313), new IntRange(0, max_floors), 1);//Scope Lens
                monsterChanceZoneStep.Items.Add(new MapItem(0400), new IntRange(0, max_floors), 1);//Power Band
                monsterChanceZoneStep.Items.Add(new MapItem(0401), new IntRange(0, max_floors), 1);//Special Band
                monsterChanceZoneStep.Items.Add(new MapItem(0402), new IntRange(0, max_floors), 1);//Defense Scarf
                monsterChanceZoneStep.Items.Add(new MapItem(0403), new IntRange(0, max_floors), 1);//Zinc Band
                monsterChanceZoneStep.Items.Add(new MapItem(0314), new IntRange(0, max_floors), 1);//Wide Lens
                monsterChanceZoneStep.Items.Add(new MapItem(0301), new IntRange(0, max_floors), 1);//Pierce Band
                monsterChanceZoneStep.Items.Add(new MapItem(0311), new IntRange(0, max_floors), 1);//Shed Shell
                monsterChanceZoneStep.Items.Add(new MapItem(0328), new IntRange(0, max_floors), 1);//X-Ray Specs
                monsterChanceZoneStep.Items.Add(new MapItem(0404), new IntRange(0, max_floors), 1);//Big Root
                monsterChanceZoneStep.Items.Add(new MapItem(0406), new IntRange(0, max_floors), 1);//Weather Rock
                monsterChanceZoneStep.Items.Add(new MapItem(0405), new IntRange(0, max_floors), 1);//Expert Belt
                monsterChanceZoneStep.Items.Add(new MapItem(0320), new IntRange(0, max_floors), 1);//Choice Scarf
                monsterChanceZoneStep.Items.Add(new MapItem(0319), new IntRange(0, max_floors), 1);//Choice Specs
                monsterChanceZoneStep.Items.Add(new MapItem(0318), new IntRange(0, max_floors), 1);//Choice Band
                monsterChanceZoneStep.Items.Add(new MapItem(0321), new IntRange(0, max_floors), 1);//Assault Vest
                monsterChanceZoneStep.Items.Add(new MapItem(0322), new IntRange(0, max_floors), 1);//Life Orb
                monsterChanceZoneStep.Items.Add(new MapItem(0315), new IntRange(0, max_floors), 1);//Heal Ribbon

                //monsterChanceZoneStep.ItemThemes.Add(new ItemThemeNone(0, new RandRange(5, 11)), new ParamRange(0, 30), 20);
                monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeRange(new IntRange(480), true, true, new RandRange(1, 4)), new ItemThemeNone(50, new RandRange(2, 4))), new IntRange(0, 30), 20);//no theme
                monsterChanceZoneStep.ItemThemes.Add(new ItemThemeMoney(200, new RandRange(4, 7)), new IntRange(0, max_floors), 20);
                monsterChanceZoneStep.MobThemes.Add(new MobThemeNone(0, new RandRange(7, 13)), new IntRange(0, max_floors), 10);

                floorSegment.ZoneSteps.Add(monsterChanceZoneStep);
            }

            {
                SpreadHouseZoneStep chestChanceZoneStep = new SpreadHouseZoneStep(PR_HOUSES, new SpreadPlanQuota(new RandRange(2, 5), new IntRange(6, max_floors)));
                chestChanceZoneStep.ModStates.Add(new FlagType(typeof(ChestModGenState)));
                chestChanceZoneStep.HouseStepSpawns.Add(new ChestStep<ListMapGenContext>(false, GetAntiFilterList(new ImmutableRoom(), new NoEventRoom())), 10);

                for (int ii = 0; ii < 18; ii++)
                    chestChanceZoneStep.Items.Add(new MapItem(76 + ii), new IntRange(0, max_floors), 4);//gummis
                chestChanceZoneStep.Items.Add(new MapItem(209), new IntRange(0, max_floors), 20);//big apricorn
                chestChanceZoneStep.Items.Add(new MapItem(164), new IntRange(0, max_floors), 80);//elixir
                chestChanceZoneStep.Items.Add(new MapItem(166), new IntRange(0, max_floors), 20);//max elixir
                chestChanceZoneStep.Items.Add(new MapItem(160), new IntRange(0, max_floors), 20);//potion
                chestChanceZoneStep.Items.Add(new MapItem(161), new IntRange(0, max_floors), 20);//max potion
                chestChanceZoneStep.Items.Add(new MapItem(173), new IntRange(0, max_floors), 20);//full heal
                for (int ii = 175; ii <= 181; ii++)
                    chestChanceZoneStep.Items.Add(new MapItem(ii), new IntRange(0, max_floors), 15);//X-Items
                for (int ii = 0; ii < specific_tms.Length; ii++)
                    chestChanceZoneStep.Items.Add(new MapItem(specific_tms[ii][0]), new IntRange(0, max_floors), 2);//TMs
                chestChanceZoneStep.Items.Add(new MapItem(477), new IntRange(0, max_floors), 20);//nugget
                chestChanceZoneStep.Items.Add(new MapItem(481, 3), new IntRange(0, max_floors), 10);//heart scale
                chestChanceZoneStep.Items.Add(new MapItem(158, 1), new IntRange(0, max_floors), 20);//amber tear
                chestChanceZoneStep.Items.Add(new MapItem(206, 3), new IntRange(0, max_floors), 20);//rare fossil
                chestChanceZoneStep.Items.Add(new MapItem(101), new IntRange(0, max_floors), 20);//reviver seed
                chestChanceZoneStep.Items.Add(new MapItem(102), new IntRange(0, max_floors), 20);//joy seed
                chestChanceZoneStep.Items.Add(new MapItem(450), new IntRange(0, max_floors), 30);//link box
                chestChanceZoneStep.Items.Add(new MapItem(451), new IntRange(0, max_floors), 30);//assembly box
                chestChanceZoneStep.Items.Add(new MapItem(453), new IntRange(0, max_floors), 10);//ability capsule
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeNone(0, new RandRange(2, 5)), new IntRange(0, max_floors), 30);
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeRange(new IntRange(101), true, true, new RandRange(2, 4)), new IntRange(0, max_floors), 10);//reviver seed
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeRange(new IntRange(102), true, true, new RandRange(1, 4)), new IntRange(0, max_floors), 10);//joy seed
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeRange(new IntRange(158, 182), true, true, new RandRange(1, 3)), new IntRange(0, max_floors), 100);//manmade items
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeRange(new IntRange(300, 349), true, true, new RandRange(1, 3)), new IntRange(0, max_floors), 20);//equip
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeRange(new IntRange(380, 398), true, true, new RandRange(1, 3)), new IntRange(0, max_floors), 10);//plates
                chestChanceZoneStep.ItemThemes.Add(new ItemThemeType(ItemData.UseType.Learn, true, true, new RandRange(1, 3)), new IntRange(0, max_floors), 10);//TMs
                chestChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(GummiState)), true, true, new RandRange(2, 5)), new IntRange(0, max_floors), 20);
                chestChanceZoneStep.ItemThemes.Add(new ItemStateType(new FlagType(typeof(RecruitState)), true, true, new RandRange(1)), new IntRange(0, max_floors), 10);

                floorSegment.ZoneSteps.Add(chestChanceZoneStep);
            }

            //switch vaults
            {
                SpreadVaultZoneStep vaultChanceZoneStep = new SpreadVaultZoneStep(PR_SPAWN_ITEMS_EXTRA, PR_SPAWN_MOBS_EXTRA, new SpreadPlanQuota(new RandRange(1, 4), new IntRange(0, 30)));

                //making room for the vault
                {
                    ResizeFloorStep<ListMapGenContext> vaultStep = new ResizeFloorStep<ListMapGenContext>(new Loc(8, 8), Dir8.DownRight, Dir8.UpLeft);
                    vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT, vaultStep));
                }
                {
                    ClampFloorStep<ListMapGenContext> vaultStep = new ClampFloorStep<ListMapGenContext>(new Loc(0), new Loc(78, 54));
                    vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT_CLAMP, vaultStep));
                }

                // room addition step
                {
                    SpawnList<RoomGen<ListMapGenContext>> detourRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    detourRooms.Add(new RoomGenCross<ListMapGenContext>(new RandRange(4), new RandRange(4), new RandRange(3), new RandRange(3)), 10);
                    SpawnList<PermissiveRoomGen<ListMapGenContext>> detourHalls = new SpawnList<PermissiveRoomGen<ListMapGenContext>>();
                    detourHalls.Add(new RoomGenAngledHall<ListMapGenContext>(0, new RandRange(2, 4), new RandRange(2, 4)), 10);
                    AddConnectedRoomsStep<ListMapGenContext> detours = new AddConnectedRoomsStep<ListMapGenContext>(detourRooms, detourHalls);
                    detours.Amount = new RandRange(8, 10);
                    detours.HallPercent = 100;
                    detours.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom()));
                    detours.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.SwitchVault));
                    detours.RoomComponents.Set(new NoConnectRoom());
                    detours.RoomComponents.Set(new NoEventRoom());
                    detours.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.SwitchVault));
                    detours.HallComponents.Set(new NoConnectRoom());
                    detours.RoomComponents.Set(new NoEventRoom());

                    vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_GEN_EXTRA, detours));
                }

                //sealing the vault
                {
                    SwitchSealStep<ListMapGenContext> vaultStep = new SwitchSealStep<ListMapGenContext>(40, 41, true);
                    vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                    vaultStep.SwitchFilters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                    vaultStep.SwitchFilters.Add(new RoomFilterComponent(true, new BossRoom()));
                    vaultChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_TILES_GEN_EXTRA, vaultStep));
                }

                //items for the vault
                {
                    vaultChanceZoneStep.Items.Add(new MapItem(164), new IntRange(0, 30), 800);//elixir
                    vaultChanceZoneStep.Items.Add(new MapItem(166), new IntRange(0, 30), 100);//max elixir
                    vaultChanceZoneStep.Items.Add(new MapItem(160), new IntRange(0, 30), 200);//potion
                    vaultChanceZoneStep.Items.Add(new MapItem(161), new IntRange(0, 30), 100);//max potion
                    vaultChanceZoneStep.Items.Add(new MapItem(173), new IntRange(0, 30), 200);//full heal
                    for (int ii = 175; ii <= 181; ii++)
                        vaultChanceZoneStep.Items.Add(new MapItem(ii), new IntRange(0, 30), 50);//X-Items
                    for (int ii = 0; ii < 18; ii++)
                        vaultChanceZoneStep.Items.Add(new MapItem(76 + ii), new IntRange(0, 30), 200);//gummis
                    vaultChanceZoneStep.Items.Add(new MapItem(158, 1), new IntRange(0, 30), 2000);//amber tear
                    vaultChanceZoneStep.Items.Add(new MapItem(101), new IntRange(0, 30), 200);//reviver seed
                    vaultChanceZoneStep.Items.Add(new MapItem(102), new IntRange(0, 30), 200);//joy seed
                    vaultChanceZoneStep.Items.Add(new MapItem(453), new IntRange(0, 30), 200);//ability capsule
                    vaultChanceZoneStep.Items.Add(new MapItem(451), new IntRange(0, 30), 500);//link box
                    vaultChanceZoneStep.Items.Add(new MapItem(450), new IntRange(0, 30), 500);//assembly box
                    vaultChanceZoneStep.Items.Add(new MapItem(349), new IntRange(0, 30), 100);//harmony scarf
                    vaultChanceZoneStep.Items.Add(new MapItem(285), new IntRange(15, 30), 50);//itemizer orb
                    vaultChanceZoneStep.Items.Add(new MapItem(235, 3), new IntRange(15, 30), 50);//transfer wand
                    vaultChanceZoneStep.Items.Add(new MapItem(455, 1), new IntRange(0, 30), 1000);//key
                }

                // item spawnings for the vault
                {
                    //add a PickerSpawner <- PresetMultiRand <- coins
                    List<MapItem> treasures = new List<MapItem>();
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(false, 152));
                    PickerSpawner<ListMapGenContext, MapItem> treasurePicker = new PickerSpawner<ListMapGenContext, MapItem>(new PresetMultiRand<MapItem>(treasures));


                    SpawnList<IStepSpawner<ListMapGenContext, MapItem>> boxSpawn = new SpawnList<IStepSpawner<ListMapGenContext, MapItem>>();

                    //444      ***    Light Box - 1* items
                    {
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>(444, new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(1), new RandRange(1))), 10);
                    }

                    //445      ***    Heavy Box - 2* items
                    {
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>(445, new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(2), new RandRange(1))), 10);
                    }

                    //447      ***    Dainty Box - Stat ups, wonder gummi, nectar, golden apple, golden banana
                    {
                        SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                        for (int nn = 0; nn < 18; nn++)//Gummi
                            boxTreasure.Add(new MapItem(76 + nn), 1);

                        boxTreasure.Add(new MapItem(151), 2);//protein
                        boxTreasure.Add(new MapItem(152), 2);//iron
                        boxTreasure.Add(new MapItem(153), 2);//calcium
                        boxTreasure.Add(new MapItem(154), 2);//zinc
                        boxTreasure.Add(new MapItem(155), 2);//carbos
                        boxTreasure.Add(new MapItem(156), 2);//hp up
                        boxTreasure.Add(new MapItem(150), 2);//nectar

                        boxTreasure.Add(new MapItem(5), 10);//perfect apple
                        boxTreasure.Add(new MapItem(7), 10);//big banana
                        boxTreasure.Add(new MapItem(75), 4);//wonder gummi
                        boxTreasure.Add(new MapItem(102), 10);//joy seed
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>(447, new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 3);
                    }

                    //448    Glittery Box - golden apple, amber tear, golden banana, nugget, golden thorn 9
                    {
                        SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();
                        boxTreasure.Add(new MapItem(205), 10);//golden thorn
                        boxTreasure.Add(new MapItem(158), 10);//Amber Tear
                        boxTreasure.Add(new MapItem(477), 10);//nugget
                        boxTreasure.Add(new MapItem(103), 10);//golden seed
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>(448, new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 2);
                    }

                    MultiStepSpawner<ListMapGenContext, MapItem> boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(1)));

                    //MultiStepSpawner <- PresetMultiRand
                    MultiStepSpawner<ListMapGenContext, MapItem> mainSpawner = new MultiStepSpawner<ListMapGenContext, MapItem>();
                    mainSpawner.Picker = new PresetMultiRand<IStepSpawner<ListMapGenContext, MapItem>>(treasurePicker, boxPicker);
                    vaultChanceZoneStep.ItemSpawners.SetRange(mainSpawner, new IntRange(0, 30));
                }
                vaultChanceZoneStep.ItemAmount.SetRange(new RandRange(5, 7), new IntRange(0, 30));

                // item placements for the vault
                {
                    RandomRoomSpawnStep<ListMapGenContext, MapItem> detourItems = new RandomRoomSpawnStep<ListMapGenContext, MapItem>();
                    detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                    vaultChanceZoneStep.ItemPlacements.SetRange(detourItems, new IntRange(0, 30));
                }

                // mobs
                // Vault FOES
                {
                    //37//470 Leafeon : 320 Grass Whistle : 348 Leaf Blade : 235 Synthesis : 241 Sunny Day
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(470, -1, 320, 348, 235, 241, 3), new IntRange(0, 30), 10);

                    //234 !! Stantler : 43 Leer : 95 Hypnosis : 36 Take Down : 109 Confuse Ray
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(234, -1, 43, 95, 36, 109, 3), new IntRange(0, 10), 10);

                    //275 Shiftry : 124 Pickpocket : 018 Whirlwind : 417 Nasty Plot : 536 Leaf Tornado : 542 Hurricane
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(275, 124, 018, 417, 536, 542, 3), new IntRange(10, 30), 10);
                    //131 Lapras : 011 Water Absorb : 058 Ice Beam : 195 Perish Song : 362 Brine
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(131, 011, 058, 195, 362, -1, 3), new IntRange(10, 30), 10);
                    //53//452 Drapion : 367 Acupressure : 398 Poison Jab : 424 Fire Fang : 565 Fell Stinger
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(452, -1, 367, 398, 424, 565, 4), new IntRange(5, 30), 10);
                    //148 Dragonair : 097 Agility : 239 Twister : 082 Dragon Rage
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(148, -1, 097, 239, 082, -1, 3), new IntRange(0, 20), 10);
                    //149 Dragonite : 136 Multiscale : 349 Dragon Dance : 355 Roost : 407 Dragon Rush : 401 Aqua Tail
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(149, 136, 349, 355, 407, 401, 3), new IntRange(15, 30), 10);
                    //327 Spinda : 077 Tangled Feet : 298 Teeter Dance : 037 Thrash
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(327, 077, 298, 037, -1, -1, 3), new IntRange(0, 30), 10);
                    //425 Drifloon : 466 Ominous Wind : 116 Focus Energy : 132 Constrict
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(425, -1, 466, 116, 132, -1, 3), new IntRange(0, 20), 10);
                    //426 Drifblim : 466 Ominous Wind : 226 Baton Pass : 254 Stockpile : 107 Minimize
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(426, -1, 466, 226, 254, 107, 3), new IntRange(15, 30), 10);
                    //045 Vileplume : 077 Poison Powder : 080 Petal Dance : 051 Acid
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(045, -1, 077, 080, 051, -1, 3), new IntRange(0, 30), 10);
                    //414 Mothim : 110 Tinted Lens : 483 Quiver Dance : 318 Silver Wind : 094 Psychic
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(414, 110, 483, 318, 094, -1, 3), new IntRange(10, 30), 10);
                    //348 Armaldo : 306 Crush Claw : 404 X-Scissor : 479 Smack Down
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(348, -1, 306, 404, 479, -1, 3), new IntRange(0, 30), 10);
                    //346 Cradily : 275 Ingrain : 051 Acid : 378 Wring Out : 362 Brine
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(346, -1, 275, 051, 378, 362, 3), new IntRange(0, 30), 10);
                    //279 Pelipper : 254 Stockpile : 255 Spit Up : 256 Swallow
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(279, -1, 254, 255, 256, -1, 3), new IntRange(0, 30), 10);
                    //700 Sylveon : 182 Pixilate : 129 Swift : 113 Light Screen : 581 Misty Terrain : 585 Moonblast
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(700, 182, 129, 113, 581, 585, 3), new IntRange(0, 30), 10);
                    //134 Vaporeon : 270 Helping Hand : 392 Aqua Ring : 330 Muddy Water
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(134, -1, 270, 392, 330, -1, 3), new IntRange(0, 30), 10);
                    //196 Espeon : 270 Helping Hand : 234 Morning Sun : 248 Future Sight
                    vaultChanceZoneStep.Mobs.Add(GetFOEMob(196, -1, 270, 234, 248, -1, 3), new IntRange(0, 30), 10);
                }
                vaultChanceZoneStep.MobAmount.SetRange(new RandRange(7, 11), new IntRange(0, 30));

                // mob placements
                {
                    PlaceRandomMobsStep<ListMapGenContext> secretMobPlacement = new PlaceRandomMobsStep<ListMapGenContext>();
                    secretMobPlacement.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.SwitchVault));
                    secretMobPlacement.ClumpFactor = 20;
                    vaultChanceZoneStep.MobPlacements.SetRange(secretMobPlacement, new IntRange(0, 30));
                }

                floorSegment.ZoneSteps.Add(vaultChanceZoneStep);
            }



            {
                SpreadBossZoneStep bossChanceZoneStep = new SpreadBossZoneStep(PR_ROOMS_GEN_EXTRA, PR_SPAWN_ITEMS_EXTRA, new SpreadPlanQuota(new RandDecay(0, 8, 55), new IntRange(2, 30)));

                {
                    ResizeFloorStep<ListMapGenContext> resizeStep = new ResizeFloorStep<ListMapGenContext>(new Loc(8, 8), Dir8.DownRight, Dir8.UpLeft);
                    bossChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT, resizeStep));
                }
                {
                    ClampFloorStep<ListMapGenContext> vaultStep = new ClampFloorStep<ListMapGenContext>(new Loc(0), new Loc(78, 54));
                    bossChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_ROOMS_PRE_VAULT_CLAMP, vaultStep));
                }

                //boss rooms
                string[] customShield = new string[] {          ".XXXXXXX.",
                                                                ".........",
                                                                ".........",
                                                                ".........",
                                                                ".........",
                                                                "X.......X",
                                                                "X.......X",
                                                                "X.......X",
                                                                "XX.....XX"};
                //   skarmbliss
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    //227 Skarmory : 005 Sturdy : 319 Metal Sound : 314 Air Cutter : 191 Spikes : 092 Toxic
                    //242 Blissey : 032 Serene Grace : 069 Seismic Toss : 135 Soft-Boiled : 287 Refresh : 196 Icy Wind
                    mobSpawns.Add(GetBossMob(227, 005, 319, 314, 191, 092, -1, new Loc(3, 2)));
                    mobSpawns.Add(GetBossMob(242, 032, 069, 135, 287, 196, -1, new Loc(5, 2)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customShield, new Loc(4, 4), mobSpawns, false), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);
                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(0, 30), 10);
                }

                string[] customEclipse = new string[] {         "XX.....XX",
                                                                "X.......X",
                                                                ".........",
                                                                ".........",
                                                                ".~.....~.",
                                                                ".~~...~~.",
                                                                "..~~~~~..",
                                                                "X..~~~..X",
                                                                "XX.....XX"};
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    //196 Espeon : 156 Magic Bounce : 094 Psychic : 247 Shadow Ball : 115 Reflect : 605 Dazzling Gleam
                    mobSpawns.Add(GetBossMob(196, 156, 094, 247, 115, 605, -1, new Loc(3, 2)));
                    //197 Umbreon : 028 Synchronize : 236 Moonlight : 212 Mean Look : 555 Snarl : 399 Dark Pulse
                    mobSpawns.Add(GetBossMob(197, 028, 236, 212, 555, 399, -1, new Loc(5, 2)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customEclipse, new Loc(4, 4), mobSpawns, false), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);
                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(0, 30), 10);
                }

                string[] customBatteryReverse = new string[] {  ".........",
                                                                "..XXXXX..",
                                                                ".........",
                                                                ".........",
                                                                ".........",
                                                                ".........",
                                                                "....X....",
                                                                "....X....",
                                                                "..XXXXX..",
                                                                "....X....",
                                                                "....X...."};
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    //310 Manectric : 058 Minus : 604 Electric Terrain : 435 Discharge : 053 Flamethrower : 598 Eerie Impulse
                    mobSpawns.Add(GetBossMob(310, 058, 604, 435, 053, 598, -1, new Loc(3, 5)));
                    //181 Ampharos : 057 Plus : 192 Zap Cannon : 406 Dragon Pulse : 324 Signal Beam : 602 Magnetic Flux
                    mobSpawns.Add(GetBossMob(181, 057, 192, 406, 324, 602, -1, new Loc(5, 5)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customBatteryReverse, new Loc(4, 3), mobSpawns, true), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);
                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(0, 30), 10);
                }

                string[] customBattery = new string[] {         "....X....",
                                                                "....X....",
                                                                "..XXXXX..",
                                                                "....X....",
                                                                "....X....",
                                                                ".........",
                                                                ".........",
                                                                ".........",
                                                                ".........",
                                                                "..XXXXX..",
                                                                "........."};
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    //311 Plusle : 057 Plus : 087 Thunder : 129 Swift : 417 Nasty Plot : 447 Grass Knot
                    mobSpawns.Add(GetBossMob(311, 057, 087, 129, 417, 447, -1, new Loc(3, 5)));
                    //312 Minun : 058 Minus : 240 Rain Dance : 097 Agility : 435 Discharge : 376 Trump Card
                    mobSpawns.Add(GetBossMob(312, 058, 240, 097, 435, 376, -1, new Loc(5, 5)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customBattery, new Loc(4, 7), mobSpawns, false), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);
                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(0, 30), 10);
                }


                string[] customRailway = new string[] {         ".........",
                                                                ".........",
                                                                "..X...X..",
                                                                "..X...X..",
                                                                "..X...X..",
                                                                "..X...X..",
                                                                "..X...X..",
                                                                ".........",
                                                                "........."};
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    //128 Tauros : 022 Intimidate : 099 Rage : 037 Thrash : 523 Bulldoze : 371 Payback
                    mobSpawns.Add(GetBossMob(128, 022, 099, 037, 523, 371, -1, new Loc(4, 3)));
                    //241 Miltank : 113 Scrappy : 208 Milk Drink : 215 Heal Bell : 034 Body Slam : 045 Growl
                    mobSpawns.Add(GetBossMob(241, 113, 208, 215, 034, 045, -1, new Loc(4, 2)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customRailway, new Loc(4, 5), mobSpawns, true), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);
                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(0, 30), 10);
                }


                string[] customButterfly = new string[] {       "X.XX.XX.X",
                                                                "...X.X...",
                                                                ".........",
                                                                ".........",
                                                                "X.......X",
                                                                "XX.....XX",
                                                                ".........",
                                                                "...X.X...",
                                                                "X..X.X..X"};
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    //414 Mothim : 110 Tinted Lens : 318 Silver Wind : 483 Quiver Dance : 403 Air Slash : 094 Psychic
                    mobSpawns.Add(GetBossMob(414, 110, 318, 483, 403, 094, -1, new Loc(3, 2)));
                    //413 Wormadam : 107 Anticipation : 522 Struggle Bug : 319 Metal Sound : 450 Bug Bite : 527 Electroweb
                    mobSpawns.Add(GetBossMob(new MonsterID(413, 2, -1, Gender.Unknown), 107, 522, 319, 450, 527, -1, new Loc(5, 2)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customButterfly, new Loc(4, 4), mobSpawns, false), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);
                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(0, 30), 10);
                }


                string[] customWaterSwirl = new string[] {      "..~~~~~..",
                                                                ".~.....~.",
                                                                "~..~~~..~",
                                                                "~.~...~.~",
                                                                "~.~.....~",
                                                                "~.~....~.",
                                                                "~..~~~~..",
                                                                ".~......~",
                                                                "..~~~~~~."};
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    //186 Politoed : 002 Drizzle : 054 Mist : 095 Hypnosis : 352 Water Pulse : 058 Ice Beam
                    mobSpawns.Add(GetBossMob(186, 002, 054, 095, 352, 058, -1, new Loc(2, 2)));
                    //062 Poliwrath : 033 Swift Swim : 187 Belly Drum : 127 Waterfall : 358 Wake-Up Slap : 509 Circle Throw
                    mobSpawns.Add(GetBossMob(062, 033, 187, 127, 358, 509, -1, new Loc(6, 2)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customWaterSwirl, new Loc(4, 4), mobSpawns, true), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);
                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(0, 30), 10);
                }

                string[] customCrownWater = new string[] {  ".~~~.~~~.",
                                                            ".~~~.~~~.",
                                                            "..~...~..",
                                                            "..~...~..",
                                                            ".........",
                                                            ".........",
                                                            ".........",
                                                            "........."};
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    //080 Slowbro : 012 Oblivious : 505 Heal Pulse : 244 Psych Up : 352 Water Pulse : 094 Psychic
                    mobSpawns.Add(GetBossMob(080, 012, 505, 244, 352, 094, -1, new Loc(3, 3)));
                    //199 Slowking : 012 Oblivious : 376 Trump Card : 347 Calm Mind : 408 Power Gem : 281 Yawn
                    mobSpawns.Add(GetBossMob(199, 012, 376, 347, 408, 281, -1, new Loc(5, 3)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customCrownWater, new Loc(4, 5), mobSpawns, false), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);
                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(0, 30), 10);
                }

                string[] customCrown = new string[] {       ".XXX.XXX.",
                                                            ".XXX.XXX.",
                                                            "..X...X..",
                                                            "..X...X..",
                                                            ".........",
                                                            ".........",
                                                            ".........",
                                                            "........."};
                {
                    SpawnList<RoomGen<ListMapGenContext>> bossRooms = new SpawnList<RoomGen<ListMapGenContext>>();
                    List<MobSpawn> mobSpawns = new List<MobSpawn>();
                    //034 Nidoking : 125 Sheer Force : 398 Poison Jab : 224 Megahorn : 116 Focus Energy : 529 Drill Run
                    mobSpawns.Add(GetBossMob(034, 125, 398, 224, 116, 529, -1, new Loc(3, 3)));
                    //031 Nidoqueen : 079 Rivalry : 270 Helping Hand : 445 Captivate : 414 Earth Power : 482 Sludge Wave
                    mobSpawns.Add(GetBossMob(031, 079, 270, 445, 414, 482, -1, new Loc(5, 3)));
                    bossRooms.Add(CreateRoomGenSpecificBoss<ListMapGenContext>(customCrown, new Loc(4, 5), mobSpawns, true), 10);
                    AddBossRoomStep<ListMapGenContext> detours = CreateGenericBossRoomStep(bossRooms);
                    bossChanceZoneStep.BossSteps.Add(detours, new IntRange(0, 30), 10);
                }

                //sealing the boss room and treasure room
                {
                    BossSealStep<ListMapGenContext> vaultStep = new BossSealStep<ListMapGenContext>(40, 38);
                    vaultStep.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                    vaultStep.BossFilters.Add(new RoomFilterComponent(false, new BossRoom()));
                    bossChanceZoneStep.VaultSteps.Add(new GenPriority<GenStep<ListMapGenContext>>(PR_TILES_GEN_EXTRA, vaultStep));
                }


                //items for the vault
                {
                    bossChanceZoneStep.Items.Add(new MapItem(164), new IntRange(0, max_floors), 800);//elixir
                    bossChanceZoneStep.Items.Add(new MapItem(166), new IntRange(0, max_floors), 100);//max elixir
                    bossChanceZoneStep.Items.Add(new MapItem(160), new IntRange(0, max_floors), 200);//potion
                    bossChanceZoneStep.Items.Add(new MapItem(161), new IntRange(0, max_floors), 100);//max potion
                    bossChanceZoneStep.Items.Add(new MapItem(173), new IntRange(0, max_floors), 200);//full heal
                    for (int ii = 175; ii <= 181; ii++)
                        bossChanceZoneStep.Items.Add(new MapItem(ii), new IntRange(0, max_floors), 50);//X-Items
                    for (int ii = 0; ii < 18; ii++)
                        bossChanceZoneStep.Items.Add(new MapItem(76 + ii), new IntRange(0, max_floors), 200);//gummis
                    bossChanceZoneStep.Items.Add(new MapItem(158, 1), new IntRange(0, max_floors), 200);//amber tear
                    bossChanceZoneStep.Items.Add(new MapItem(101), new IntRange(0, max_floors), 200);//reviver seed
                    bossChanceZoneStep.Items.Add(new MapItem(102), new IntRange(0, max_floors), 200);//joy seed
                    bossChanceZoneStep.Items.Add(new MapItem(453), new IntRange(0, max_floors), 200);//ability capsule
                    bossChanceZoneStep.Items.Add(new MapItem(349), new IntRange(0, max_floors), 100);//harmony scarf
                    bossChanceZoneStep.Items.Add(new MapItem(455, 1), new IntRange(0, 30), 1000);//key

                    bossChanceZoneStep.Items.Add(new MapItem(592), new IntRange(0, 30), 5);//TM Toxic
                    bossChanceZoneStep.Items.Add(new MapItem(604), new IntRange(0, 30), 5);//TM Rock Climb
                    bossChanceZoneStep.Items.Add(new MapItem(605), new IntRange(0, 30), 5);//TM Waterfall
                    bossChanceZoneStep.Items.Add(new MapItem(609), new IntRange(0, 30), 5);//TM Charge Beam
                    bossChanceZoneStep.Items.Add(new MapItem(613), new IntRange(0, 30), 5);//TM Hone Claws
                    bossChanceZoneStep.Items.Add(new MapItem(615), new IntRange(0, 30), 5);//TM Giga Impact
                    bossChanceZoneStep.Items.Add(new MapItem(617), new IntRange(0, 30), 5);//TM Dig
                    bossChanceZoneStep.Items.Add(new MapItem(623), new IntRange(0, 30), 5);//TM Safeguard
                    bossChanceZoneStep.Items.Add(new MapItem(626), new IntRange(0, 30), 5);//TM Work Up
                    bossChanceZoneStep.Items.Add(new MapItem(627), new IntRange(0, 30), 5);//TM Scald
                    bossChanceZoneStep.Items.Add(new MapItem(630), new IntRange(0, 30), 5);//TM U-turn
                    bossChanceZoneStep.Items.Add(new MapItem(631), new IntRange(0, 30), 5);//TM Thunder Wave
                    bossChanceZoneStep.Items.Add(new MapItem(645), new IntRange(0, 30), 5);//TM Roost
                    bossChanceZoneStep.Items.Add(new MapItem(655), new IntRange(0, 30), 5);//TM Psyshock
                    bossChanceZoneStep.Items.Add(new MapItem(660), new IntRange(0, 30), 5);//TM Thunder
                    bossChanceZoneStep.Items.Add(new MapItem(661), new IntRange(0, 30), 5);//TM X-Scissor
                    bossChanceZoneStep.Items.Add(new MapItem(682), new IntRange(0, 30), 5);//TM Taunt
                    bossChanceZoneStep.Items.Add(new MapItem(684), new IntRange(0, 30), 5);//TM Ice Beam
                    bossChanceZoneStep.Items.Add(new MapItem(686), new IntRange(0, 30), 5);//TM Light Screen
                    bossChanceZoneStep.Items.Add(new MapItem(691), new IntRange(0, 30), 5);//TM Calm Mind
                    bossChanceZoneStep.Items.Add(new MapItem(692), new IntRange(0, 30), 5);//TM Torment
                    bossChanceZoneStep.Items.Add(new MapItem(693), new IntRange(0, 30), 5);//TM Strength
                    bossChanceZoneStep.Items.Add(new MapItem(694), new IntRange(0, 30), 5);//TM Cut
                    bossChanceZoneStep.Items.Add(new MapItem(695), new IntRange(0, 30), 5);//TM Rock Smash
                    bossChanceZoneStep.Items.Add(new MapItem(696), new IntRange(0, 30), 5);//TM Bulk Up
                    bossChanceZoneStep.Items.Add(new MapItem(698), new IntRange(0, 30), 5);//TM Rest

                    bossChanceZoneStep.Items.Add(new MapItem(591), new IntRange(0, 30), 5);//TM Psychic
                    bossChanceZoneStep.Items.Add(new MapItem(595), new IntRange(0, 30), 5);//TM Flamethrower
                    bossChanceZoneStep.Items.Add(new MapItem(599), new IntRange(0, 30), 5);//TM Hyper Beam
                    bossChanceZoneStep.Items.Add(new MapItem(611), new IntRange(0, 30), 5);//TM Blizzard
                    bossChanceZoneStep.Items.Add(new MapItem(619), new IntRange(0, 30), 5);//TM Rock Slide
                    bossChanceZoneStep.Items.Add(new MapItem(620), new IntRange(0, 30), 5);//TM Sludge Wave
                    bossChanceZoneStep.Items.Add(new MapItem(624), new IntRange(0, 30), 5);//TM Swords Dance
                    bossChanceZoneStep.Items.Add(new MapItem(628), new IntRange(0, 30), 5);//TM Energy Ball
                    bossChanceZoneStep.Items.Add(new MapItem(635), new IntRange(0, 30), 5);//TM Fire Blast
                    bossChanceZoneStep.Items.Add(new MapItem(640), new IntRange(0, 30), 5);//TM Thunderbolt
                    bossChanceZoneStep.Items.Add(new MapItem(641), new IntRange(0, 30), 5);//TM Shadow Ball
                    bossChanceZoneStep.Items.Add(new MapItem(649), new IntRange(0, 30), 5);//TM Dark Pulse
                    bossChanceZoneStep.Items.Add(new MapItem(654), new IntRange(0, 30), 5);//TM Earthquake
                    bossChanceZoneStep.Items.Add(new MapItem(658), new IntRange(0, 30), 5);//TM Frost Breath
                    bossChanceZoneStep.Items.Add(new MapItem(662), new IntRange(0, 30), 5);//TM Dazzling Gleam
                    bossChanceZoneStep.Items.Add(new MapItem(666), new IntRange(0, 30), 5);//TM Snarl
                    bossChanceZoneStep.Items.Add(new MapItem(669), new IntRange(0, 30), 5);//TM Focus Blast
                    bossChanceZoneStep.Items.Add(new MapItem(672), new IntRange(0, 30), 5);//TM Overheat
                    bossChanceZoneStep.Items.Add(new MapItem(676), new IntRange(0, 30), 5);//TM Sludge Bomb
                    bossChanceZoneStep.Items.Add(new MapItem(683), new IntRange(0, 30), 5);//TM Solar Beam
                    bossChanceZoneStep.Items.Add(new MapItem(685), new IntRange(0, 30), 5);//TM Flash Cannon
                    bossChanceZoneStep.Items.Add(new MapItem(697), new IntRange(0, 30), 5);//TM Surf

                }

                // item spawnings for the vault
                {
                    //add a PickerSpawner <- PresetMultiRand <- coins
                    List<MapItem> treasures = new List<MapItem>();
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(true, 200));
                    treasures.Add(new MapItem(false, 477));
                    PickerSpawner<ListMapGenContext, MapItem> treasurePicker = new PickerSpawner<ListMapGenContext, MapItem>(new PresetMultiRand<MapItem>(treasures));

                    SpawnList<IStepSpawner<ListMapGenContext, MapItem>> boxSpawn = new SpawnList<IStepSpawner<ListMapGenContext, MapItem>>();

                    //444      ***    Light Box - 1* items
                    {
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>(444, new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(1), new RandRange(1))), 10);
                    }

                    //445      ***    Heavy Box - 2* items
                    {
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>(445, new SpeciesItemContextSpawner<ListMapGenContext>(new IntRange(2), new RandRange(1))), 10);
                    }

                    ////446      ***    Nifty Box - all high tier TMs, ability capsule, heart scale 9, max potion, full heal, max elixir
                    //{
                    //    SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                    //    for (int nn = 588; nn < 700; nn++)
                    //        boxTreasure.Add(new MapItem(nn), 1);//TMs
                    //    boxTreasure.Add(new MapItem(453), 100);//ability capsule
                    //    boxTreasure.Add(new MapItem(481), 100);//heart scale
                    //    boxTreasure.Add(new MapItem(160), 60);//potion
                    //    boxTreasure.Add(new MapItem(161), 30);//max potion
                    //    boxTreasure.Add(new MapItem(173), 100);//full heal
                    //    boxTreasure.Add(new MapItem(164), 60);//elixir
                    //    boxTreasure.Add(new MapItem(166), 30);//max elixir
                    //    boxSpawn.Add(new BoxSpawner<ListMapGenContext>(446, new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 10);
                    //}

                    //447      ***    Dainty Box - Stat ups, wonder gummi, nectar, golden apple, golden banana
                    {
                        SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                        for (int nn = 0; nn < 18; nn++)//Gummi
                            boxTreasure.Add(new MapItem(76 + nn), 1);

                        boxTreasure.Add(new MapItem(151), 2);//protein
                        boxTreasure.Add(new MapItem(152), 2);//iron
                        boxTreasure.Add(new MapItem(153), 2);//calcium
                        boxTreasure.Add(new MapItem(154), 2);//zinc
                        boxTreasure.Add(new MapItem(155), 2);//carbos
                        boxTreasure.Add(new MapItem(156), 2);//hp up
                        boxTreasure.Add(new MapItem(150), 2);//nectar

                        boxTreasure.Add(new MapItem(5), 10);//perfect apple
                        boxTreasure.Add(new MapItem(7), 10);//big banana
                        boxTreasure.Add(new MapItem(75), 4);//wonder gummi
                        boxTreasure.Add(new MapItem(102), 10);//joy seed
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>(447, new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 3);
                    }

                    //448    Glittery Box - golden apple, amber tear, golden banana, nugget, golden thorn 9
                    {
                        SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();
                        boxTreasure.Add(new MapItem(205), 10);//golden thorn
                        boxTreasure.Add(new MapItem(158), 10);//Amber Tear
                        boxTreasure.Add(new MapItem(477), 10);//nugget
                        boxTreasure.Add(new MapItem(103), 10);//golden seed
                        boxSpawn.Add(new BoxSpawner<ListMapGenContext>(448, new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 2);
                    }

                    //449      ***    Deluxe Box - Legendary exclusive items, harmony scarf, golden items, stat ups, wonder gummi, perfect apricorn, max potion/full heal/max elixir
                    //{
                    //    SpeciesItemListSpawner<ListMapGenContext> legendSpawner = new SpeciesItemListSpawner<ListMapGenContext>(new IntRange(1, 3), new RandRange(1));
                    //    legendSpawner.Species.Add(144);
                    //    legendSpawner.Species.Add(145);
                    //    legendSpawner.Species.Add(146);
                    //    legendSpawner.Species.Add(150);
                    //    boxSpawn.Add(new BoxSpawner<ListMapGenContext>(449, legendSpawner), 5);
                    //}
                    //{
                    //    SpawnList<MapItem> boxTreasure = new SpawnList<MapItem>();

                    //    for (int nn = 0; nn < 18; nn++)//Gummi
                    //        boxTreasure.Add(new MapItem(76 + nn), 1);

                    //    boxTreasure.Add(new MapItem(4), 10);//golden apple
                    //    boxTreasure.Add(new MapItem(8), 10);//gold banana
                    //    boxTreasure.Add(new MapItem(158), 10);//Amber Tear
                    //    boxTreasure.Add(new MapItem(477), 10);//nugget
                    //    boxTreasure.Add(new MapItem(103), 10);//golden seed
                    //    boxTreasure.Add(new MapItem(161), 30);//max potion
                    //    boxTreasure.Add(new MapItem(173), 100);//full heal
                    //    boxTreasure.Add(new MapItem(166), 30);//max elixir
                    //    boxTreasure.Add(new MapItem(75), 4);//wonder gummi
                    //    boxTreasure.Add(new MapItem(349), 1);//harmony scarf
                    //    boxTreasure.Add(new MapItem(219), 1);//perfect apricorn
                    //    boxSpawn.Add(new BoxSpawner<ListMapGenContext>(449, new PickerSpawner<ListMapGenContext, MapItem>(new LoopedRand<MapItem>(boxTreasure, new RandRange(1)))), 10);
                    //}

                    MultiStepSpawner<ListMapGenContext, MapItem> boxPicker = new MultiStepSpawner<ListMapGenContext, MapItem>(new LoopedRand<IStepSpawner<ListMapGenContext, MapItem>>(boxSpawn, new RandRange(1)));

                    //MultiStepSpawner <- PresetMultiRand
                    MultiStepSpawner<ListMapGenContext, MapItem> mainSpawner = new MultiStepSpawner<ListMapGenContext, MapItem>();
                    mainSpawner.Picker = new PresetMultiRand<IStepSpawner<ListMapGenContext, MapItem>>(treasurePicker, boxPicker);
                    bossChanceZoneStep.ItemSpawners.SetRange(mainSpawner, new IntRange(0, max_floors));
                }
                bossChanceZoneStep.ItemAmount.SetRange(new RandRange(2, 4), new IntRange(0, max_floors));

                // item placements for the vault
                {
                    RandomRoomSpawnStep<ListMapGenContext, MapItem> detourItems = new RandomRoomSpawnStep<ListMapGenContext, MapItem>();
                    detourItems.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.BossLocked));
                    bossChanceZoneStep.ItemPlacements.SetRange(detourItems, new IntRange(0, max_floors));
                }

                floorSegment.ZoneSteps.Add(bossChanceZoneStep);
            }


            SpawnRangeList<IGenPriority> shopZoneSpawns = new SpawnRangeList<IGenPriority>();
            {
                ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                shop.Personality = 0;
                shop.SecurityStatus = 38;
                shop.Items.Add(new MapItem(10, 0, 100), 20);//oran
                shop.Items.Add(new MapItem(11, 0, 150), 20);//leppa
                shop.Items.Add(new MapItem(12, 0, 100), 20);//lum
                shop.Items.Add(new MapItem(72, 0, 100), 20);//sitrus
                shop.Items.Add(new MapItem(101, 0, 800), 15);//reviver
                shop.Items.Add(new MapItem(118, 0, 500), 15);//ban
                shop.Items.Add(new MapItem(450, 0, 1200), 20);//Link Box
                shop.Items.Add(new MapItem(451, 0, 1200), 20);//Assembly Box
                for (int nn = 211; nn <= 218; nn++)
                    shop.Items.Add(new MapItem(nn, 0, 600), 2);//apricorns
                shop.Items.Add(new MapItem(210, 0, 800), 5);//plain apricorn
                for (int nn = 0; nn < 7; nn++)
                    shop.Items.Add(new MapItem(43 + nn, 0, 600), 3);//pinch berries
                for (int nn = 0; nn <= 18; nn++)
                    shop.Items.Add(new MapItem(19 + nn, 0, 100), 1);//type berries
                shop.Items.Add(new MapItem(112, 0, 350), 20);//blast seed
                shop.Items.Add(new MapItem(102, 0, 2000), 5);//joy seed

                for(int nn = 0; nn < specific_tms.Length; nn++)
                    shop.Items.Add(new MapItem(specific_tms[nn][0], 0, specific_tms[nn][1]), 2);//TMs

                for (int ii = 0; ii < 18; ii++)
                    shop.Items.Add(new MapItem(331 + ii, 0, 2000), 5);//type items

                shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);
                shop.ItemThemes.Add(new ItemThemeRange(new IntRange(331, 349), false, true, new RandRange(3, 5)), 10);//type items
                shop.ItemThemes.Add(new ItemThemeMultiple(new ItemThemeType(ItemData.UseType.Learn, false, true, new RandRange(3, 5)),
                    new ItemThemeRange(new IntRange(450, 454), false, true, new RandRange(0, 3))), 10);//TMs + machines

                // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 103 screech : 86 thunder wave
                shop.StartMob = GetShopMob(352, 16, 485, 20, 103, 86, new int[] { 1984, 1985, 1988 }, 0);
                {
                    // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 103 screech : 86 thunder wave
                    shop.Mobs.Add(GetShopMob(352, 16, 485, 20, 103, 86, new int[] { 1984, 1985, 1988 }, -1), 10);
                    // 352 Kecleon : 16 color change : 485 synchronoise : 20 bind : 50 disable : 374 fling
                    shop.Mobs.Add(GetShopMob(352, 16, 485, 20, 50, 374, new int[] { 1984, 1985, 1988 }, -1), 10);
                    // 352 Kecleon : 168 protean : 425 shadow sneak : 246 ancient power : 510 incinerate : 168 thief
                    shop.Mobs.Add(GetShopMob(352, 168, 425, 246, 510, 168, new int[] { 1984, 1985, 1988 }, -1, 24), 10);
                    // 352 Kecleon : 168 protean : 332 aerial ace : 421 shadow claw : 60 psybeam : 364 feint
                    shop.Mobs.Add(GetShopMob(352, 168, 332, 421, 60, 364, new int[] { 1984, 1985, 1988 }, -1, 24), 10);
                }

                shopZoneSpawns.Add(new GenPriority<GenStep<MapGenContext>>(PR_SHOPS, shop), new IntRange(5, max_floors), 10);
            }
            {
                ShopStep<MapGenContext> shop = new ShopStep<MapGenContext>(GetAntiFilterList(new ImmutableRoom(), new NoEventRoom()));
                shop.Personality = 1;
                shop.SecurityStatus = 38;
                shop.Items.Add(new MapItem(251, 0, 300), 20);//weather orb
                shop.Items.Add(new MapItem(252, 0, 600), 20);//mobile orb
                shop.Items.Add(new MapItem(253, 0, 600), 20);//luminous orb
                shop.Items.Add(new MapItem(256, 0, 400), 20);//fill-in orb
                shop.Items.Add(new MapItem(258, 0, 300), 20);//all-aim orb
                shop.Items.Add(new MapItem(263, 0, 300), 20);//cleanse orb
                shop.Items.Add(new MapItem(264, 0, 600), 20);//one-shot orb
                shop.Items.Add(new MapItem(265, 0, 500), 20);//endure orb
                shop.Items.Add(new MapItem(266, 0, 400), 20);//pierce orb
                shop.Items.Add(new MapItem(267, 0, 600), 20);//stayaway orb
                shop.Items.Add(new MapItem(268, 0, 600), 20);//all protect orb
                shop.Items.Add(new MapItem(269, 0, 300), 20);//trap-see orb
                shop.Items.Add(new MapItem(270, 0, 300), 20);//trapbust orb
                shop.Items.Add(new MapItem(271, 0, 500), 20);//slumber orb
                shop.Items.Add(new MapItem(272, 0, 400), 20);//totter orb
                shop.Items.Add(new MapItem(273, 0, 400), 20);//petrify orb
                shop.Items.Add(new MapItem(274, 0, 400), 20);//freeze orb
                shop.Items.Add(new MapItem(275, 0, 500), 20);//spurn orb
                shop.Items.Add(new MapItem(276, 0, 500), 20);//foe-hold orb
                shop.Items.Add(new MapItem(277, 0, 400), 20);//nullify orb
                shop.Items.Add(new MapItem(278, 0, 300), 20);//all-dodge orb
                shop.Items.Add(new MapItem(281, 0, 500), 20);//one-room orb
                shop.Items.Add(new MapItem(282, 0, 400), 20);//slow orb
                shop.Items.Add(new MapItem(283, 0, 400), 20);//rebound orb
                shop.Items.Add(new MapItem(284, 0, 400), 20);//mirror orb
                shop.Items.Add(new MapItem(286, 0, 400), 20);//foe-seal orb
                shop.Items.Add(new MapItem(287, 0, 600), 20);//halving orb
                shop.Items.Add(new MapItem(288, 0, 300), 20);//rollcall orb
                shop.Items.Add(new MapItem(289, 0, 300), 20);//mug orb

                shop.Items.Add(new MapItem(220, 3, 180), 40);//path wand
                shop.Items.Add(new MapItem(221, 3, 150), 40);//pounce wand
                shop.Items.Add(new MapItem(222, 3, 150), 40);//whirlwind wand
                shop.Items.Add(new MapItem(223, 3, 150), 40);//switcher wand
                shop.Items.Add(new MapItem(225, 3, 120), 40);//lure wand
                shop.Items.Add(new MapItem(228, 3, 150), 40);//slow wand
                shop.Items.Add(new MapItem(231, 3, 150), 40);//topsy-turvy wand
                shop.Items.Add(new MapItem(232, 3, 150), 40);//warp wand
                shop.Items.Add(new MapItem(233, 3, 120), 40);//purge wand
                shop.Items.Add(new MapItem(234, 3, 120), 40);//lob wand

                shop.Items.Add(new MapItem(351, 0, 2500), 50);//Fire Stone
                shop.Items.Add(new MapItem(353, 0, 2500), 50);//Water Stone
                shop.Items.Add(new MapItem(354, 0, 2500), 50);//Leaf Stone
                shop.Items.Add(new MapItem(355, 0, 3500), 50);//Moon Stone
                shop.Items.Add(new MapItem(356, 0, 3500), 50);//Sun Stone
                shop.Items.Add(new MapItem(374, 0, 3500), 50);//King's Rock
                shop.Items.Add(new MapItem(365, 0, 3500), 50);//Link Cable

                for (int ii = 0; ii < 18; ii++)
                    shop.Items.Add(new MapItem(331 + ii, 0, 2000), 10);//type items


                shop.ItemThemes.Add(new ItemThemeNone(100, new RandRange(3, 9)), 10);
                shop.ItemThemes.Add(new ItemThemeRange(new IntRange(351, 380), false, true, new RandRange(3, 5)), 10);//evo items
                shop.ItemThemes.Add(new ItemThemeRange(new IntRange(331, 349), false, true, new RandRange(3, 5)), 10);//type items

                // Cleffa : 98 Magic Guard : 118 Metronome : 47 Sing : 204 Charm : 313 Fake Tears
                {
                    MobSpawn post_mob = new MobSpawn();
                    post_mob.BaseForm = new MonsterID(173, 0, 0, Gender.Unknown);
                    post_mob.Tactic = 23;
                    post_mob.Level = new RandRange(5);
                    post_mob.Intrinsic = 98;
                    post_mob.SpecifiedSkills.Add(118);
                    post_mob.SpecifiedSkills.Add(47);
                    post_mob.SpecifiedSkills.Add(204);
                    post_mob.SpecifiedSkills.Add(313);
                    post_mob.SpawnFeatures.Add(new MobSpawnDiscriminator(1));
                    post_mob.SpawnFeatures.Add(new MobSpawnInteractable(new BattleScriptEvent("ShopkeeperInteract")));
                    post_mob.SpawnFeatures.Add(new MobSpawnLuaTable("{ Role = \"Shopkeeper\" }"));
                    shop.StartMob = post_mob;
                }
                {
                    // 35 Clefairy : 132 Friend Guard : 282 Knock Off : 107 Minimize : 236 Moonlight : 277 Magic Coat
                    shop.Mobs.Add(GetShopMob(35, 132, 282, 107, 236, 277, new int[] { 973, 976 }, -1), 10);
                    // 36 Clefable : 109 Unaware : 118 Metronome : 500 Stored Power : 343 Covet : 271 Trick
                    shop.Mobs.Add(GetShopMob(36, 109, 118, 500, 343, 271, new int[] { 973, 976 }, -1), 5);
                    // 36 Clefable : 98 Magic Guard : 118 Metronome : 213 Attract : 282 Knock Off : 266 Follow Me
                    shop.Mobs.Add(GetShopMob(36, 98, 118, 213, 282, 266, new int[] { 973, 976 }, -1), 5);
                }

                shopZoneSpawns.Add(new GenPriority<GenStep<MapGenContext>>(PR_SHOPS, shop), new IntRange(5, max_floors), 10);
            }
            SpreadStepRangeZoneStep shopZoneStep = new SpreadStepRangeZoneStep(new SpreadPlanQuota(new RandRange(4, 14), new IntRange(2, 38)), shopZoneSpawns);
            shopZoneStep.ModStates.Add(new FlagType(typeof(ShopModGenState)));
            floorSegment.ZoneSteps.Add(shopZoneStep);

            for (int ii = 0; ii < max_floors; ii++)
            {
                GridFloorGen layout = new GridFloorGen();

                //Floor settings
                MapDataStep<MapGenContext> floorData = new MapDataStep<MapGenContext>();
                floorData.TimeLimit = 1500;
                if (ii <= 4)
                    floorData.Music = "B02. Demonstration 2.ogg";
                else if (ii <= 12)
                    floorData.Music = "Sky Peak Forest.ogg";
                else if (ii <= 16)
                    floorData.Music = "Random Dungeon Theme 3.ogg";
                else if (ii <= 20)
                    floorData.Music = "Miracle Sea.ogg";
                else if (ii <= 27)
                    floorData.Music = "B03. Demonstration 3.ogg";
                else if (ii <= 34)
                    floorData.Music = "Hidden Land.ogg";
                else
                    floorData.Music = "Hidden Highland.ogg";

                if (ii <= 8)
                    floorData.CharSight = Map.SightRange.Dark;
                else if (ii <= 16)
                    floorData.CharSight = Map.SightRange.Clear;
                else
                    floorData.CharSight = Map.SightRange.Dark;

                if (ii <= 4)
                    floorData.TileSight = Map.SightRange.Dark;
                else if (ii <= 24)
                    floorData.TileSight = Map.SightRange.Clear;
                else
                    floorData.TileSight = Map.SightRange.Dark;

                layout.GenSteps.Add(PR_FLOOR_DATA, floorData);

                //Tilesets
                if (ii <= 4)
                    AddTextureData(layout, 162, 163, 164, 13);
                else if (ii <= 12)
                    AddTextureData(layout, 433, 434, 435, 01);
                else if (ii <= 16)
                    AddTextureData(layout, 439, 440, 441, 08);
                else if (ii <= 20)
                    AddTextureData(layout, 213, 214, 215, 10);
                else if (ii <= 24)
                    AddTextureData(layout, 171, 172, 173, 16);
                else if (ii <= 27)
                    AddTextureData(layout, 174, 175, 176, 15);
                else if (ii <= 34)
                    AddTextureData(layout, 292, 293, 294, 13);
                else
                    AddTextureData(layout, 210, 211, 212, 05);

                //wonder tiles
                RandRange wonderTileRange;
                if (ii <= 12)
                    wonderTileRange = new RandRange(3, 6);
                else if (ii <= 20)
                    wonderTileRange = new RandRange(4, 7);
                else if (ii <= 34)
                    wonderTileRange = new RandRange(5, 8);
                else
                    wonderTileRange = new RandRange(0);

                if (ii <= 16)
                    AddSingleTrapStep(layout, wonderTileRange, 27, true);
                else
                    AddSingleTrapStep(layout, wonderTileRange, 27, false);

                //traps
                RandRange trapRange;
                if (ii <= 8)
                    trapRange = new RandRange(6, 9);
                else if (ii <= 16)
                    trapRange = new RandRange(8, 12);
                else if (ii <= 20)
                    trapRange = new RandRange(10, 14);
                else if (ii <= 24)
                    trapRange = new RandRange(12, 16);
                else
                    trapRange = new RandRange(10, 14);
                AddTrapsSteps(layout, trapRange);


                //money - Ballpark 40K
                if (ii <= 4)
                    AddMoneyData(layout, new RandRange(2, 5));
                else if (ii <= 12)
                    AddMoneyData(layout, new RandRange(4, 8));
                else if (ii <= 20)
                    AddMoneyData(layout, new RandRange(3, 7));
                else
                    AddMoneyData(layout, new RandRange(7, 11));

                if (ii >= 3 && ii < 7)
                {
                    //063 Abra : 100 Teleport
                    //always holds a TM
                    MobSpawn mob = GetGenericMob(063, -1, 100, -1, -1, -1, new RandRange(10));
                    MobSpawnItem keySpawn = new MobSpawnItem(true);
                    keySpawn.Items.Add(new InvItem(587), 10);//TM Secret Power
                    keySpawn.Items.Add(new InvItem(681), 10);//TM Hidden Power
                    keySpawn.Items.Add(new InvItem(596), 10);//TM Protect
                    keySpawn.Items.Add(new InvItem(590), 10);//TM Double Team
                    keySpawn.Items.Add(new InvItem(680), 10);//TM Attract
                    keySpawn.Items.Add(new InvItem(644), 10);//TM Captivate
                    keySpawn.Items.Add(new InvItem(643), 10);//TM Fling
                    keySpawn.Items.Add(new InvItem(682), 10);//TM Taunt
                    mob.SpawnFeatures.Add(keySpawn);
                    SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                    specificTeam.Spawns.Add(mob);

                    LoopedTeamSpawner<MapGenContext> spawner = new LoopedTeamSpawner<MapGenContext>(specificTeam);
                    {
                        spawner.AmountSpawner = new RandDecay(0, 4, 70);
                    }
                    PlaceRandomMobsStep<MapGenContext> secretMobPlacement = new PlaceRandomMobsStep<MapGenContext>(spawner);
                    layout.GenSteps.Add(PR_SPAWN_MOBS, secretMobPlacement);
                }

                if (ii >= 6 && ii < 9)
                {
                    //147 Dratini : 35 Wrap : 43 Leer
                    SpecificTeamSpawner specificTeam = new SpecificTeamSpawner();
                    specificTeam.Spawns.Add(GetGenericMob(147, -1, 35, 43, -1, -1, new RandRange(15)));

                    LoopedTeamSpawner<MapGenContext> spawner = new LoopedTeamSpawner<MapGenContext>(specificTeam);
                    {
                        spawner.AmountSpawner = new RandRange(1, 3);
                    }
                    PlaceDisconnectedMobsStep<MapGenContext> secretMobPlacement = new PlaceDisconnectedMobsStep<MapGenContext>(spawner);
                    secretMobPlacement.AcceptedTiles.Add(new Tile(3));
                    layout.GenSteps.Add(PR_SPAWN_MOBS, secretMobPlacement);
                }

                //enemies
                if (ii <= 8)
                    AddRespawnData(layout, 10, 85);
                else if (ii <= 16)
                    AddRespawnData(layout, 14, 90);
                else if (ii <= 20)
                    AddRespawnData(layout, 18, 100);
                else if (ii <= 27)
                    AddRespawnData(layout, 26, 110);
                else
                    AddRespawnData(layout, 22, 130);

                //enemies
                if (ii <= 8)
                    AddEnemySpawnData(layout, 30, new RandRange(5, 7));
                else if (ii <= 16)
                    AddEnemySpawnData(layout, 30, new RandRange(7, 12));
                else if (ii <= 20)
                    AddEnemySpawnData(layout, 30, new RandRange(12, 16));
                else if (ii <= 27)
                    AddEnemySpawnData(layout, 20, new RandRange(17, 22));
                else
                    AddEnemySpawnData(layout, 20, new RandRange(13, 18));

                //items
                if (ii <= 8)
                    AddItemData(layout, new RandRange(3, 6), 25);
                else
                    AddItemData(layout, new RandRange(3, 5), 25);

                List<MapItem> specificSpawns = new List<MapItem>();
                if (ii > 20 && ii <= 27)
                {
                    //2 pearls scattered in the maze, 3 pearls in the later floors
                    if (ii > 24)
                        specificSpawns.Add(new MapItem(480, 1));//Pearl
                    specificSpawns.Add(new MapItem(480, 1));//Pearl
                    specificSpawns.Add(new MapItem(480, 1));//Pearl
                }
                if (ii == 29)
                    specificSpawns.Add(new MapItem(11));//Leppa Berry
                if (ii == 39)
                    specificSpawns.Add(new MapItem(491));//Gracidea

                RandomSpawnStep<MapGenContext, MapItem> specificItemZoneStep = new RandomSpawnStep<MapGenContext, MapItem>(new PickerSpawner<MapGenContext, MapItem>(new PresetMultiRand<MapItem>(specificSpawns)));
                layout.GenSteps.Add(PR_SPAWN_ITEMS, specificItemZoneStep);


                SpawnList<MapItem> wallSpawns = new SpawnList<MapItem>();
                wallSpawns.Add(new MapItem(10), 50);//oran berry
                wallSpawns.Add(new MapItem(481, 1), 50);//heart scale
                wallSpawns.Add(new MapItem(201, 2), 20);//cacnea spike
                wallSpawns.Add(new MapItem(200, 2), 20);//stick
                wallSpawns.Add(new MapItem(37), 10);//jaboca berry
                wallSpawns.Add(new MapItem(38), 10);//rowap berry
                wallSpawns.Add(new MapItem(220, 1), 10);//path wand
                wallSpawns.Add(new MapItem(228, 3), 10);//fear wand
                wallSpawns.Add(new MapItem(235, 2), 10);//transfer wand
                wallSpawns.Add(new MapItem(236, 4), 10);//vanish wand

                wallSpawns.Add(new MapItem(43), 10);//apicot berry
                wallSpawns.Add(new MapItem(44), 10);//liechi berry
                wallSpawns.Add(new MapItem(45), 10);//ganlon berry
                wallSpawns.Add(new MapItem(46), 10);//salac berry
                wallSpawns.Add(new MapItem(47), 10);//petaya berry
                wallSpawns.Add(new MapItem(48), 10);//starf berry
                wallSpawns.Add(new MapItem(49), 10);//micle berry
                wallSpawns.Add(new MapItem(51), 10);//enigma berry

                for (int nn = 0; nn < 18; nn++)//Type Berry
                    wallSpawns.Add(new MapItem(19 + nn), 1);

                TerrainSpawnStep<MapGenContext, MapItem> wallItemZoneStep = new TerrainSpawnStep<MapGenContext, MapItem>(new Tile(2));
                wallItemZoneStep.Spawn = new PickerSpawner<MapGenContext, MapItem>(new LoopedRand<MapItem>(wallSpawns, new RandRange(6, 10)));
                layout.GenSteps.Add(PR_SPAWN_ITEMS, wallItemZoneStep);

                //construct paths
                if (ii <= 4)
                {
                    //free form rooms with short halls
                    AddInitGridStep(layout, 4, 4, 8, 8);

                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(90);
                    path.BranchRatio = new RandRange(0, 25);

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //blocked
                    genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile(2), new RandRange(4, 8), new RandRange(4, 8), new RandRange(2), new RandRange(2)), 5);
                    //bump
                    genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8), new RandRange(50)), 10);
                    //round
                    genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8)), 10);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(50, 50));

                    AddTunnelStep<MapGenContext> tunneler = new AddTunnelStep<MapGenContext>();
                    tunneler.Halls = new RandRange(2, 5);
                    tunneler.TurnLength = new RandRange(3, 8);
                    tunneler.MaxLength = new RandRange(25);
                    layout.GenSteps.Add(PR_TILES_GEN_TUNNEL, tunneler);

                }
                else if (ii <= 8)
                {
                    //free form rooms with long halls, some of which are 2-wide
                    AddInitGridStep(layout, 4, 4, 10, 10);

                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(80);
                    path.BranchRatio = new RandRange(0, 25);

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //blocked
                    genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile(2), new RandRange(4, 8), new RandRange(4, 8), new RandRange(2), new RandRange(2)), 5);
                    //bump
                    genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8), new RandRange(50)), 10);
                    //round
                    genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8)), 10);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(50, 50));

                    {
                        CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(2, 5), GetImmutableFilterList());
                        step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenCave<MapGenContext>(new RandRange(10), new RandRange(18))), 10);
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenCave<MapGenContext>(new RandRange(18), new RandRange(10))), 10);
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }

                    AddTunnelStep<MapGenContext> tunneler = new AddTunnelStep<MapGenContext>();
                    tunneler.Halls = new RandRange(2, 5);
                    tunneler.TurnLength = new RandRange(3, 8);
                    tunneler.MaxLength = new RandRange(25);
                    layout.GenSteps.Add(PR_TILES_GEN_TUNNEL, tunneler);

                }
                else if (ii <= 12)
                {
                    //wide open clearings with chokepoints
                    AddInitGridStep(layout, 5, 5, 8, 8);

                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(90);
                    path.BranchRatio = new RandRange(0, 25);

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //blocked
                    genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile(2), new RandRange(4, 8), new RandRange(3, 7), new RandRange(2), new RandRange(2)), 5);
                    //bump
                    genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8), new RandRange(50)), 10);
                    //round
                    genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8)), 10);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(50, 50));

                    {
                        CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(2, 6), GetImmutableFilterList());
                        step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 2), new RoomGenCave<MapGenContext>(new RandRange(16), new RandRange(16))), 10);
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenCave<MapGenContext>(new RandRange(8), new RandRange(16))), 10);
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenCave<MapGenContext>(new RandRange(16), new RandRange(8))), 10);
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }
                }
                else if (ii <= 16)
                {
                    //semi-open layout
                    AddInitGridStep(layout, 5, 5, 8, 8);

                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(90);
                    path.BranchRatio = new RandRange(0, 25);

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //blocked
                    genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile(2), new RandRange(4, 8), new RandRange(4, 8), new RandRange(2), new RandRange(2)), 5);
                    //bump
                    genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8), new RandRange(50)), 10);
                    //round
                    genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8)), 10);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(50, 50));

                    {
                        CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(2, 4), GetImmutableFilterList());
                        step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 2), new RoomGenCave<MapGenContext>(new RandRange(16), new RandRange(16))), 10);
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }

                    AddTunnelStep<MapGenContext> tunneler = new AddTunnelStep<MapGenContext>();
                    tunneler.Halls = new RandRange(2, 5);
                    tunneler.TurnLength = new RandRange(3, 8);
                    tunneler.MaxLength = new RandRange(25);
                    layout.GenSteps.Add(PR_TILES_GEN_TUNNEL, tunneler);

                }
                else if (ii <= 20)
                {
                    //add rectangular rooms
                    AddInitGridStep(layout, 5, 5, 8, 8);

                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(90);
                    path.BranchRatio = new RandRange(0, 25);

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //blocked
                    genericRooms.Add(new RoomGenBlocked<MapGenContext>(new Tile(2), new RandRange(4, 8), new RandRange(4, 8), new RandRange(2), new RandRange(2)), 5);
                    //bump
                    genericRooms.Add(new RoomGenBump<MapGenContext>(new RandRange(4, 8), new RandRange(4, 8), new RandRange(50)), 20);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    //layout.GenSteps.Add(PR_GRID_GEN, CreateGenericConnect(75, 50));

                    {
                        CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(2, 5), GetImmutableFilterList());
                        step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 2), new RoomGenCross<MapGenContext>(new RandRange(4, 15), new RandRange(4, 15), new RandRange(2, 4), new RandRange(2, 4))), 10);
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(1, 2), new RoomGenCross<MapGenContext>(new RandRange(4, 15), new RandRange(4, 15), new RandRange(2, 4), new RandRange(2, 4))), 10);
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2, 1), new RoomGenCross<MapGenContext>(new RandRange(4, 15), new RandRange(4, 15), new RandRange(2, 4), new RandRange(2, 4))), 10);
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }

                    AddTunnelStep<MapGenContext> tunneler = new AddTunnelStep<MapGenContext>();
                    tunneler.Halls = new RandRange(10, 18);
                    tunneler.TurnLength = new RandRange(3, 8);
                    tunneler.MaxLength = new RandRange(25);
                    layout.GenSteps.Add(PR_TILES_GEN_TUNNEL, tunneler);

                }
                else if (ii <= 24)
                {
                    //Initialize a grid of cells.
                    AddInitGridStep(layout, 13, 10, 2, 2, 2);

                    //Create a path that is composed of a branching tree
                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(100);
                    //MODDABLE: can change branching
                    path.BranchRatio = new RandRange(30);

                    //Give it some room types to place
                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //square
                    genericRooms.Add(new RoomGenSquare<MapGenContext>(new RandRange(2), new RandRange(2)), 3);
                    path.GenericRooms = genericRooms;

                    //Give it some hall types to place
                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenSquare<MapGenContext>(new RandRange(1), new RandRange(1)), 20);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    //MODDABLE: can change connectivity
                    {
                        ConnectGridBranchStep<MapGenContext> step = new ConnectGridBranchStep<MapGenContext>(80);
                        step.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        step.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom()));
                        PresetPicker<PermissiveRoomGen<MapGenContext>> picker = new PresetPicker<PermissiveRoomGen<MapGenContext>>();
                        picker.ToSpawn = new RoomGenSquare<MapGenContext>(new RandRange(1), new RandRange(1));
                        step.GenericHalls = picker;
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }
                    {
                        SetGridPlanComponentStep<MapGenContext> step = new SetGridPlanComponentStep<MapGenContext>();
                        step.Components.Set(new NoEventRoom());
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                        //layout.GenSteps.Add(PR_GRID_GEN, new MarkAsHallStep<MapGenContext>());
                    }

                    //Combine some rooms for large rooms
                    {
                        CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(3), GetImmutableFilterList());
                        step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2), new RoomGenSquare<MapGenContext>(new RandRange(5), new RandRange(5))), 10);
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }
                }
                else if (ii <= 27)
                {
                    //Initialize a grid of cells.
                    AddInitGridStep(layout, 16, 13, 2, 2);

                    //Create a path that is composed of a branching tree
                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(100);
                    //MODDABLE: can change branching
                    path.BranchRatio = new RandRange(30);

                    //Give it some room types to place
                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //square
                    genericRooms.Add(new RoomGenSquare<MapGenContext>(new RandRange(2), new RandRange(2)), 3);
                    path.GenericRooms = genericRooms;

                    //Give it some hall types to place
                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenSquare<MapGenContext>(new RandRange(1), new RandRange(1)), 20);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    //MODDABLE: can change connectivity
                    {
                        ConnectGridBranchStep<MapGenContext> step = new ConnectGridBranchStep<MapGenContext>(80);
                        step.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        step.Filters.Add(new RoomFilterComponent(true, new NoConnectRoom()));
                        PresetPicker<PermissiveRoomGen<MapGenContext>> picker = new PresetPicker<PermissiveRoomGen<MapGenContext>>();
                        picker.ToSpawn = new RoomGenSquare<MapGenContext>(new RandRange(1), new RandRange(1));
                        step.GenericHalls = picker;
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }
                    {
                        SetGridPlanComponentStep<MapGenContext> step = new SetGridPlanComponentStep<MapGenContext>();
                        step.Components.Set(new NoEventRoom());
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                        //layout.GenSteps.Add(PR_GRID_GEN, new MarkAsHallStep<MapGenContext>());
                    }

                    //Special rooms
                    {
                        AddLargeRoomStep<MapGenContext> step = new AddLargeRoomStep<MapGenContext>(new RandRange(3, 7), GetImmutableFilterList());
                        step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        {
                            LargeRoom<MapGenContext> largeRoom = new LargeRoom<MapGenContext>(new RoomGenSquare<MapGenContext>(new RandRange(8), new RandRange(8)), new Loc(3), 9);
                            largeRoom.OpenBorders[(int)Dir4.Down][1] = true;
                            largeRoom.OpenBorders[(int)Dir4.Left][1] = true;
                            largeRoom.OpenBorders[(int)Dir4.Up][1] = true;
                            largeRoom.OpenBorders[(int)Dir4.Right][1] = true;
                            step.GiantRooms.Add(largeRoom, 10);
                        }
                        {
                            string[] custom = new string[] {  "~~~..~~~",
                                              "~~~..~~~",
                                              "~~#..#~~",
                                              "........",
                                              "........",
                                              "~~#..#~~",
                                              "~~~..~~~",
                                              "~~~..~~~"};
                            LargeRoom<MapGenContext> largeRoom = new LargeRoom<MapGenContext>(CreateRoomGenSpecific<MapGenContext>(custom), new Loc(3), 2);
                            largeRoom.OpenBorders[(int)Dir4.Down][1] = true;
                            largeRoom.OpenBorders[(int)Dir4.Left][1] = true;
                            largeRoom.OpenBorders[(int)Dir4.Up][1] = true;
                            largeRoom.OpenBorders[(int)Dir4.Right][1] = true;
                            step.GiantRooms.Add(largeRoom, 10);
                        }
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }
                    //Combine some rooms for large rooms
                    {
                        CombineGridRoomStep<MapGenContext> step = new CombineGridRoomStep<MapGenContext>(new RandRange(3), GetImmutableFilterList());
                        step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                        step.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                        step.Combos.Add(new GridCombo<MapGenContext>(new Loc(2), new RoomGenSquare<MapGenContext>(new RandRange(5), new RandRange(5))), 10);
                        layout.GenSteps.Add(PR_GRID_GEN, step);
                    }
                }
                else
                {
                    //prim maze with caves
                    AddInitGridStep(layout, 5, 4, 12, 12);

                    GridPathBranch<MapGenContext> path = new GridPathBranch<MapGenContext>();
                    path.RoomComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.HallComponents.Set(new ConnectivityRoom(ConnectivityRoom.Connectivity.Main));
                    path.RoomRatio = new RandRange(80);
                    path.BranchRatio = new RandRange(0, 25);

                    SpawnList<RoomGen<MapGenContext>> genericRooms = new SpawnList<RoomGen<MapGenContext>>();
                    //cross
                    genericRooms.Add(new RoomGenCross<MapGenContext>(new RandRange(4, 11), new RandRange(4, 11), new RandRange(2, 6), new RandRange(2, 6)), 10);
                    //round
                    genericRooms.Add(new RoomGenRound<MapGenContext>(new RandRange(5, 9), new RandRange(5, 9)), 10);
                    path.GenericRooms = genericRooms;

                    SpawnList<PermissiveRoomGen<MapGenContext>> genericHalls = new SpawnList<PermissiveRoomGen<MapGenContext>>();
                    genericHalls.Add(new RoomGenAngledHall<MapGenContext>(50), 10);
                    path.GenericHalls = genericHalls;

                    layout.GenSteps.Add(PR_GRID_GEN, path);

                    AddTunnelStep<MapGenContext> tunneler = new AddTunnelStep<MapGenContext>();
                    tunneler.Halls = new RandRange(20, 28);
                    tunneler.TurnLength = new RandRange(3, 8);
                    tunneler.MaxLength = new RandRange(25);
                    layout.GenSteps.Add(PR_TILES_GEN_TUNNEL, tunneler);
                }

                AddDrawGridSteps(layout);

                if (ii <= 27)
                    AddStairStep(layout, false);
                else
                {
                    //the main stairs becomes the exit stairs
                    EffectTile exitTile = new EffectTile(46, true);
                    exitTile.TileStates.Set(new DestState(SegLoc.Invalid));
                    var step = new FloorStairsStep<MapGenContext, MapGenEntrance, MapGenExit>(new MapGenEntrance(Dir8.Down), new MapGenExit(exitTile));
                    step.Filters.Add(new RoomFilterConnectivity(ConnectivityRoom.Connectivity.Main));
                    step.Filters.Add(new RoomFilterComponent(true, new BossRoom()));
                    layout.GenSteps.Add(PR_EXITS, step);

                    if (ii == 28)
                    {
                        //the next floor is all in visible tiles, as a secret stairs.
                        //It will always be in the same room as the exit stairs if possible
                        EffectTile secretTile = new EffectTile(2, true);
                        NearSpawnableSpawnStep<MapGenContext, EffectTile, MapGenExit> trapStep = new NearSpawnableSpawnStep<MapGenContext, EffectTile, MapGenExit>(new PickerSpawner<MapGenContext, EffectTile>(new PresetMultiRand<EffectTile>(secretTile)), 100);
                        layout.GenSteps.Add(PR_SPAWN_TRAPS, trapStep);
                    }
                    else if (ii <= 34)
                    {
                        //the next floor is in whatever room, but exposed
                        EffectTile secretTile = new EffectTile(2, true);
                        RandomSpawnStep<MapGenContext, EffectTile> trapStep = new RandomSpawnStep<MapGenContext, EffectTile>(new PickerSpawner<MapGenContext, EffectTile>(new PresetMultiRand<EffectTile>(secretTile)));
                        layout.GenSteps.Add(PR_SPAWN_TRAPS, trapStep);
                    }
                    else if (ii < 39)
                    {
                        //the next floor will be in whatever room, hidden
                        EffectTile secretTile = new EffectTile(2, false);
                        RandomSpawnStep<MapGenContext, EffectTile> trapStep = new RandomSpawnStep<MapGenContext, EffectTile>(new PickerSpawner<MapGenContext, EffectTile>(new PresetMultiRand<EffectTile>(secretTile)));
                        layout.GenSteps.Add(PR_SPAWN_TRAPS, trapStep);
                    }
                }

                if (ii <= 4)
                {
                    //nothing
                }
                else if (ii < 9)
                    AddWaterSteps(layout, 3, new RandRange(30), false);//water
                else if (ii <= 12)
                    AddWaterSteps(layout, 3, new RandRange(30));//water
                else if (ii <= 16)
                    AddWaterSteps(layout, 3, new RandRange(25));//water
                else if (ii <= 20)
                    AddWaterSteps(layout, 3, new RandRange(15));//water
                else
                    AddWaterSteps(layout, 3, new RandRange(22));//water



                floorSegment.Floors.Add(layout);
            }

            zone.Segments.Add(floorSegment);

            zone.GroundMaps.Add("garden_end");
        }
        #endregion

    }
}
