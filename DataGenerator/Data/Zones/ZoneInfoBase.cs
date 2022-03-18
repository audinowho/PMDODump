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

        static void FillDebugZone(ZoneData zone)
        {
            zone.Name = new LocalText("Debug");
            zone.Rogue = RogueStatus.ItemTransfer;

            {
                LayeredSegment structure = new LayeredSegment();

                    //{
                    //    List<(MapGenExit, Loc)> exits = new List<(MapGenExit, Loc)>();
                    //    EffectTile secretStairs = new EffectTile(48, true);
                    //    secretStairs.TileStates.Set(new TileScriptState("Test", "{}"));
                    //    exits.Add((new MapGenExit(secretStairs), new Loc(36, 2)));
                    //    AddSpecificSpawn(layout, exits, PR_EXITS);
                    //}
                //Tests Tilesets, and unlockables
                #region TILESET TESTS

                {
                    string[] level = {
                            ".........................",
                            "............9............",
                            ".........................",
                            "............#............",
                            ".....###...###...###.....",
                            "....#.#.....#.....#.#....",
                            "....####...###...####....",
                            "....#.#############.#....",
                            ".......##.......##.......",
                            ".......#..#####..#.......",
                            ".......#.#######.#.......",
                            "....#.##.#######.##.#....",
                            "...#####.###.###.#####...",
                            "....#.##.#######.##.#....",
                            ".......#.#######.#.......",
                            ".......#..#####..#.......",
                            ".......##.......##.......",
                            "....#.#############.#....",
                            "....####...###...####....",
                            "....#.#.....#.....#.#....",
                            ".....###...###...###.....",
                            "............#............",
                            ".........................",
                            "...........8.>...........",
                            ".........................",
                            "...###.............###...",
                            "...###.............###...",
                            "...###.............###...",
                            ".........................",
                            ".........6.....7.........",
                            ".........................",
                            "##.....................##",
                            "##....#...........#....##",
                            "##....#.....5.....#....##",
                            "##....#...........#....##",
                            "##.....................##",
                            "##....#...........#....##",
                            "##....#..3.....4..#....##",
                            "##....#...........#....##",
                            "##.....................##",
                            "##.....................##",
                            "#########.......#########",
                            "######.............######",
                            "######.............######",
                            "######......2......######",
                            "######.............######",
                            "######.............######",
                            "######.............######",
                            "######.............######",
                            "######.............######",
                            "######.............######",
                            "######...1.........######",
                            "######.............######",
                            "#######.#################",
                            "#######...........#######",
                            "#################.#######",
                            "#######...........#######",
                            "#######.#################",
                            "#######...........#######",
                            "#######..0........#######",
                            "#######...........#######",
                            "#######.....@.....#######",
                            "#######...........#######",
                        };

                    StairsFloorGen layout = new StairsFloorGen();
                    
                    AddFloorData(layout, "", -1, Map.SightRange.Dark, Map.SightRange.Dark);

                    InitTilesStep<StairsMapGenContext> startStep = new InitTilesStep<StairsMapGenContext>();
                    int width = level[0].Length;
                    int height = level.Length;
                    startStep.Width = width + 2;
                    startStep.Height = height + 2;

                    layout.GenSteps.Add(PR_TILES_INIT, startStep);

                    SpecificTilesStep<StairsMapGenContext> drawStep = new SpecificTilesStep<StairsMapGenContext>();
                    drawStep.Offset = new Loc(1);

                    drawStep.Tiles = new Tile[width][];
                    for (int ii = 0; ii < width; ii++)
                    {
                        drawStep.Tiles[ii] = new Tile[height];
                        for (int jj = 0; jj < height; jj++)
                        {
                            if (level[jj][ii] == '#')
                                drawStep.Tiles[ii][jj] = new Tile(2);
                            else if (level[jj][ii] == '~')
                                drawStep.Tiles[ii][jj] = new Tile(3);
                            else
                            {
                                EffectTile effect = new EffectTile();
                                if (level[jj][ii] == '0')
                                {
                                    effect = new EffectTile(3, true);
                                    effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE"), new LocalFormatSimple("SIGN_SAMPLE_000")));
                                }
                                else if (level[jj][ii] == '1')
                                {
                                    effect = new EffectTile(3, true);
                                    effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE"), new LocalFormatSimple("SIGN_SAMPLE_001")));
                                }
                                else if (level[jj][ii] == '2')
                                {
                                    effect = new EffectTile(3, true);
                                    effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE"), new LocalFormatSimple("SIGN_SAMPLE_002")));
                                }
                                else if (level[jj][ii] == '3')
                                {
                                    effect = new EffectTile(3, true);
                                    effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE"), new LocalFormatSimple("SIGN_SAMPLE_003")));
                                }
                                else if (level[jj][ii] == '4')
                                {
                                    effect = new EffectTile(3, true);
                                    effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE"), new LocalFormatSimple("SIGN_SAMPLE_004")));
                                }
                                else if (level[jj][ii] == '5')
                                {
                                    effect = new EffectTile(3, true);
                                    effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE"), new LocalFormatSimple("SIGN_SAMPLE_005")));
                                }
                                else if (level[jj][ii] == '6')
                                {
                                    effect = new EffectTile(3, true);
                                    effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE"), new LocalFormatSimple("SIGN_SAMPLE_006")));
                                }
                                else if (level[jj][ii] == '7')
                                {
                                    effect = new EffectTile(3, true);
                                    effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE"), new LocalFormatSimple("SIGN_SAMPLE_007")));
                                }
                                else if (level[jj][ii] == '8')
                                {
                                    effect = new EffectTile(3, true);
                                    effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE"), new LocalFormatSimple("SIGN_SAMPLE_008")));
                                }
                                else if (level[jj][ii] == '9')
                                {
                                    effect = new EffectTile(3, true);
                                    effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE"), new LocalFormatSimple("SIGN_SAMPLE_009")));
                                }
                                Tile tile = new Tile(0);
                                tile.Effect = effect;
                                drawStep.Tiles[ii][jj] = tile;
                            }
                        }
                    }

                    layout.GenSteps.Add(PR_TILES_GEN, drawStep);

                    //add border
                    layout.GenSteps.Add(PR_TILES_BARRIER, new UnbreakableBorderStep<StairsMapGenContext>(1));

                    {
                        List<(MapGenEntrance, Loc)> items = new List<(MapGenEntrance, Loc)>();
                        items.Add((new MapGenEntrance(Dir8.Down), new Loc(12, 61)));
                        AddSpecificSpawn(layout, items, PR_EXITS);
                    }
                    {
                        List<(MapGenExit, Loc)> items = new List<(MapGenExit, Loc)>();
                        items.Add((new MapGenExit(new EffectTile(1, true)), new Loc(14, 24)));
                        AddSpecificSpawn(layout, items, PR_EXITS);
                    }

                    MapEffectStep<StairsMapGenContext> checkPostProc = new MapEffectStep<StairsMapGenContext>();
                    checkPostProc.Effect.OnMapStarts.Add(0, new NullCharEvent(new BattleLogEvent(new StringKey("MSG_GREETING"))));
                    layout.GenSteps.Add(PR_HOUSES, checkPostProc);

                    MapTextureStep<StairsMapGenContext> texturePostProc = new MapTextureStep<StairsMapGenContext>();
                    texturePostProc.BlockTileset = 0;
                    texturePostProc.GroundTileset = 1;
                    texturePostProc.WaterTileset = 2;
                    layout.GenSteps.Add(PR_TEXTURES, texturePostProc);

                    structure.Floors.Add(layout);
                }
                #endregion


                zone.Segments.Add(structure);
            }

            zone.GroundMaps.Add(MapInfo.MapNames[3]);
        }

        #region GUILDMASTER ISLAND
        static void FillHubZone(ZoneData zone)
        {
            zone.Name = new LocalText("Guildmaster Island");

            //flavour: explorer badges prevent team members from being separated in mystery dungeons
            //saving is at a statue; disabled in roguelocke
            //drifblim handle gondolas (price to ride = difference in restore price (only upwards)); disabled in roguelocke
            //bank is only at base camp
            //adventures in normal mode are each individual leg
            //normal mode has no reserve member EXP
            //first attempt will ALWAYS be in roguelocke mode
            //in roguelocke mode only, an apricorn is given at the start.

            //Legendary birds: summonable
            //Legendary beasts: obtainable by a game of tag
            //Legendary golems + gigas: regigigas challenge
            //Legendary lake trio: rescued from thieves? summonable?
            //Mewtwo: cave of solace
            //Ho-oh/Lugia: the sky?
            //Groudon: summoned by thieves?
            //Kyogre: summoned by thieves?
            //Rayquaza: the sky
            //latis: secret garden
            //mew: happy new year's
            //celebi: set the clock back
            //jirachi: leave the game on for 7 days and 7 nights
            //deoxys:
            //manaphy:
            //phione:
            //shaymin: needed for reaching the sky...
            //cresselia: dream dungeon?
            //darkrai: dream dungeon?
            //heatran: cave stop?
            //giratina: at the bottom of the abyss
            //dialga: dimensional dungeon
            //palkia: dimensional dungeon
            //arceus: dimensional dungeon?

            //-----------------Home Base
            //Guildmaster Trail
            //Tropical Path
            //Shimmering Bay
            //Trainee's Fault
            //Overgrown Wilds
            //Cave of Whispers
            //-----------------Clearing Camp (storage, assembly)
            //exit is blocked by a snorlax; no one can figure out how to wake it; you have 3 rounds of 3 choices
            //NPCs nearby provide hints
            //talk about how no one's made it to the top
            //allude to the Secret Garden
            //talk about apricorns and recruitment
            //4-5 NPCs in total
            //Faded Trail - Watch out for Shinx's intimidate
            //Windy Valley
            //Ambush Forest - meet outlaws and this is their base of operations
            //Secret Garden
            //-----------------Cliff Camp (storage, assembly)
            //relic camp is alluded to; "some teams have gone even farther... but we haven't had any contact with them"
            //monster in the cave is alluded to
            //Talk about the flying pokemon and their weaknesses
            //And by the way, watch out for the (notably dangerous pokemon here)
            //talk about alternate routes; an explorer is angry that a rival explorer knows a shortcut but won't share; change this when overgrown wilds is unlocked
            //5-7 npcs in total
            //talk about item uses
            //Flyaway Cliffs - watch out for sky attack
            //Igneous Tunnel
            //Treacherous Mountain - meet outlaws and this is where they stash their treasures
            //Wayward Snow Path
            //-----------------Canyon Camp (storage, assembly, summon altar?)
            //"long ago a tower was built on the side of the mountain.  ages passed, and the tower collapsed.  now the remains cover the path up to the summit"
            //And by the way, watch out for the (notably dangerous pokemon here)
            //Dispute occurs here, intro to outlaws?  creates a reason to go after them in the forsaken desert?
            //talk about traps and disarming them
            //talk about dark dungeons; degrees of darkness
            //rhydon offers to create a statue for you if you can defeat them
            //Thunderstruck Pass - watch out for weather tactics
            //Forsaken Desert - meet thieves (cacturne, hippowdon, skarmory)
            //Lunar Range
            //Relic Tower
            //-----------------Cave Shelter (assembly, storage) - 1 - Cave Camp; boss battle upon entering (Dyed in Blood) (natural terror)
            //And by the way, watch out for the (notably dangerous pokemon here)
            //"you don't think this can get any worse, do you?"
            //ambushed by a crapload of angered pokemon
            //Illusion Ridge
            //Cave of Solace
            //Royal Halls
            //-----------------Path to the Summit (assembly, storage) - contact from the guildmasters; very few others are here
            //"going to Champion's Road?" [Yes/No]
            //"You came all the way here... just to turn back?"
            //and by the way, watch out for the EVERYTHING
            //Champion's Road
            //The Sky
            //-----------------Summit


            //Outrage/Rest + heal bell

            //someone to abuse moves:
            //elemental punches
            //drill peck
            //smog
            //poison gas
            //lovely kiss
            //sweet kiss
            //sweet scent
            //ancient power
            //shadow ball
            //charge
            //signal beam
            //dragon dance
            //swagger/flatter
            //refresh
            //meteor mash
            //fake tears
            //bullet seed/rock blast
            //power trick
            //heart stamp


            //bide
            //leech seed
            //string shot
            //wrap/bind
            //rock tomb
            //beat up
            //safeguard
            //spite
            //curse
            //incinerate
            //perish song
            //swords dance
            //yawn
            //knock off
            //echoed voice
            //shadow sneak
            //psychic


            //disable
            //thunder wave
            //toxic
            //mirror move
            //egg bomb
            //barrage
            //transform
            //psywave
            //Tri Attack
            //triple kick
            //mind reader
            //protect
            //belly drum
            //destiny bond
            //mean look
            //mirror coat
            //extreme speed
            //torment
            //will-o-wisp
            //brick break
            //imprison
            //grudge
            //snatch
            //teeter dance
            //poison fang
            //air cutter
            //metal sound
            //grass whistle
            //water pulse
            //hammer arm
            //gyro ball
            //brine
            //feint
            //pluck/bug bite
            //close combat
            //embargo
            //heal block
            //gastro acid
            //seed bomb
            //air slash
            //drain punch
            //vacuum wave
            //focus blast/ice shard
            //elemental fangs
            //rock climb
            //iron head
            //stone edge
            //captivate
            //grass knot
            //chatter
            //charge beam
            //wood hammer
            //wonder room
            //psyshock
            //telekinesis
            //magic room
            //flame charge
            //coil
            //clear smog
            //scald
            //shell smash
            //hex
            //acrobatics
            //struggle bug
            //bulldoze
            //drill run
            //sticky web
            //phantom force
            //freeze-dry
            //disarming voice
            //draining kiss
            //terrain moves
            //confide
            //venom drench
            //dazzling gleam
            //power-up punch

            //revenge
            //payback
            //punishment
            //avalanche
            //flail (tank/endure)
            //smelling salts/wake-up-slap (wth status inducer)
            //assurance (groups?)
            //stored power (while adding buffs)
            //fell stinger (aim for allies!)

            //vice grip (rooms)
            //razor wind (rooms)
            //take down (stantler?)
            //leer (early-game)
            //roar (good with tackle)
            //sing (early-mid-game; other status moves need to be present)
            //supersonic (halls)
            //petal dance (tank)
            //dragon rage (tank; rooms)
            //agility (in groups)
            //rage (tank)
            //night shade/seismic toss
            //screech (halls)
            //confuse ray (distance)
            //light screen (in groups)
            //haze (proper use)
            //reflect (in groups)
            //focus energy (projectiles)
            //clamp (in groups)
            //skull bash (rooms)
            //glare (distance)
            //sky attack (open area)
            //splash (floor dragons)
            //rest (tank)
            //substitute (rooms)
            //spider web (groups)
            //nightmare (rooms)
            //cotton spore (group)
            //outrage (tank; rooms)
            //heal bell (monster houses)
            //present (groups)
            //magnitude (groups)
            //dragon breath (halls)
            //encore (groups)
            //pursuit (in groups)
            //iron tail (halls; showcase terrain destruction)
            //morning sun (groups)
            //synthesis (pair with leech seed?)
            //moonlight (groups)
            //twister (groups)
            //future sight (as training before xatu)
            //uproar (with sleeping foes)
            //memento (late-game)
            //taunt (mid-late game)
            //helping hand (mid-game)
            //wish (groups)
            //magic coat (later game)
            //metal burst (rooms)
            //hyper voice (training before xatu)
            //crush claw (early-mid game)
            //astonish (earlygame)
            //cosmic power (groups)
            //howl (groups)
            //u-turn (with backup ally)
            //accupressure (with an ally)

            //lucky chant (groups)
            //discharge (+ground type allies later)
            //lava plume (+fire type allies later)
            //wide guard (groups)
            //foul play (with buffs?/tank)

            //after you (groups)
            //quash (groups)
            //frost breath (halls)
            //dragon tail (rooms)
            //snarl (rooms)
            //eerie impulse (rooms)
            //magnetic flux (groups)
            //draco meteor (groups)
            //magnet rise (groups)
            //rage powder (groups)
            //round (groups)
            //quick guard (groups)
            //ally switch (groups)

            //super fang
            //healing wish (properly in groups; someone else cover it)
            //aqua ring (groups)

            //self-destruct/explosion (properly)
            //teleport (metal slimes)
            //retaliate (rooms; properly)

            //wring out (properly; at high HP)
            //dream eater (with a sleeper)
            //rock smash (tunnel ants)
            //psycho shift (properly; on tanks; in groups)
            //pain split (properly)
            //final gambit
            //baton pass (properly)
            //psych up (properly; with something that buffs)
            //focus punch (properly; charge up when far away)
            //endeavor

            //thief (needs a good means of escape; needs proper use)
            //covet (needs proper use; run away when having it, or keep begging if there exists no path)
            //trick (do once, then run away)
            //skill swap (only once)
            //entrainment

            //sleep talk (with a natural sleeper)
            //nature power (properly)
            //me first (properly)
            //copycat (properly)
            //stockpile/spit up/swallow (properly)

            //fake out
            //sucker punch

            //switcheroo (properly in groups)
            //heal pulse (friendly)
            //bestow (friendly; just leave afterwards// later on, have it bestow bad items)
            //spikes/toxic spikes/stealth rock (properly)
            //super fang + brine

            //someone to abuse abilities:
            //defiant
            //toxic boost/flare boost
            //justified
            //rattled
            //suction cups
            //hustle
            //guts/quick feet
            //liquid ooze
            //poison heal
            //magic guard

            //color change
            //wonder guard
            //levitate
            //magnet pull
            //pressure
            //pickup/etc
            //truant
            //speed boost
            //sand stream/snow warning

            //stench
            //sturdy
            //damp
            //sand veil/snow cloak (in weather)
            //volt absorb/water absorb/flash fire
            //compound eyes
            //static/etc
            //intimidate
            //shadow tag
            //rough skin
            //synchronize
            //clear body
            //natural cure
            //serene grace
            //swift swim/chlorophyll
            //illuminate
            //trace
            //huge power/pure power
            //soundproof
            //rain dish/ice body
            //thick fat
            //forecast
            //sticky hold
            //shed skin
            //overgrow/etc
            //rock head
            //tangled feet
            //gluttony
            //unburden (with acrobatics?)
            //simple
            //dry skin
            //download
            //skill link
            //solar power
            //normalize
            //no guard
            //technician
            //leaf guard
            //klutz
            //mold breaker
            //aftermath
            //anticipation
            //forewarn
            //unaware
            //tinted lens
            //filter/solid rock
            //scrappy
            //storm drain/lightning rod
            //frisk
            //flower gift
            //pickpocket
            //contrary
            //unnerve
            //cursed body
            //healer
            //friend guard
            //weak armor
            //multiscale
            //harvest
            //telepathy
            //overcoat
            //regenerator
            //analytic
            //infiltrator
            //moxie
            //magic bounce
            //sap sipper
            //prankster
            //competitive


            for (int jj = 0; jj < MapInfo.MapNames.Length; jj++)
                zone.GroundMaps.Add(MapInfo.MapNames[jj]);

            LayeredSegment staticSegment = new LayeredSegment();
            for (int jj = 0; jj < MapInfo.MapNames.Length; jj++)
            {
                LoadGen layout = new LoadGen();
                MappedRoomStep<MapLoadContext> startGen = new MappedRoomStep<MapLoadContext>();
                startGen.MapID = MapInfo.MapNames[jj];
                layout.GenSteps.Add(PR_TILES_INIT, startGen);
                MapEffectStep<MapLoadContext> noRescue = new MapEffectStep<MapLoadContext>();
                noRescue.Effect.OnMapRefresh.Add(0, new MapNoRescueEvent());
                layout.GenSteps.Add(PR_FLOOR_DATA, noRescue);
                staticSegment.Floors.Add(layout);
            }

            zone.Segments.Add(staticSegment);
        }
        #endregion

        #region TRAINING MAZE
        static void FillTrainingMaze(ZoneData zone)
        {
            zone.Name = new LocalText("Training Maze");
            zone.Level = 10;
            zone.LevelCap = true;
            zone.BagRestrict = 0;
            zone.MoneyRestrict = true;






            //What can be placed in info pages?
            //put a "Help" option in Others
            //Controls
            //[Z]+Direction Run
            //[Z] Attack
            //[Z]+[X] Skip Turn
            //[S]+Direction Change Direction
            //[D]+Direction Move Diagonally
            //[A]+[S]/[D]/[Z]/[X] Use a Move
            //[C] Team Mode
            //[1][2][3][4] Change Leader
            //[Backspace] Toggle Minimap
            //Menu Hotkeys
            //[ESC] Main Menu
            //[TAB] Message Log
            //[Q] Moves Menu
            //[W] Items Menu
            //[S] Sort Items (in the Items menu)
            //[E] Tactics Menu
            //[R] Team Menu
            //Type Chart
            //Type Immunities
            //\u8226Fire-types are immune to being Burned.
            //\u8226Electric-types are immune to being Paralyzed.
            //\u8226Ice-types are immune to being Frozen.
            //\u8226Grass-types are immune to being Seeded.
            //\u8226Ghost-types are immune to being Immobilized.
            //\u8226Steel-types are immune to being Poisoned.

            DictionarySegment structure = new DictionarySegment();
            structure.ZoneSteps.Add(new FloorNameIDZoneStep(PR_FLOOR_DATA, new LocalText("Training Maze\nLv. {0}")));

            for (int nn = 0; nn < 4; nn++)
            {
                int floor_level = 5;
                if (nn == 1)
                    floor_level = 10;
                else if (nn == 2)
                    floor_level = 25;
                else if (nn == 3)
                    floor_level = 50;

                StairsFloorGen layout = new StairsFloorGen();

                AddFloorData(layout, "B02. Demonstration 2.ogg", -1, Map.SightRange.Dark, Map.SightRange.Dark);

                string[] level = new string[]  {"#######################################################################################################################################################################################",
                                                    "#########=............................=####=............................=####=............................=####=............................=####=............................=########",
                                                    "#########.....a..a...a..a..............####...a+a+a+a....+a+......+a+....####....a...a....a...a............####....(...)....?.`.?....!...!...####...........a.a..a.a...........########",
                                                    "#########..............................####..............................####..............................####..............$.$......a.a....####..............................########",
                                                    "#A..B####..............................####..............................####..............................####..../.|.!....`.?.`....!.:.!...####...##....................##...########",
                                                    "#0..1####....a..a..a..a..a....a..a..a..####...a+a+a+a....+a+......+a+....####....a...a....a...a....a.a.a...####..............$.$......a.a....####...##.....##########.....##...########",
                                                    "#....####..............................####..............................####..............................####....%...&....?.`.?....!...!...####..............................########",
                                                    "#....####......E........F........G.....####......H........I........J.....####......K........L........M.....####......N........O........P.....####....Q......R......S......T....####..U#",
                                                    "#..@.................................................................................................................................................................................4#",
                                                    "#....####..............................####..............................####..............................####..............................####....~.~.~.~......~~~~~~~~~~~..####...#",
                                                    "#C..D####....*****....*****....*****...####..****.****..**.**.....***....####...*..*..*..*..*..*..*..*..*..####...*.*.*.*.*.*.+.*.*.*.*.*.*..####...~.~.~.~.~.....~~~~~~~~~~~..########",
                                                    "#2..3####=..............*......*****..=####=.****.****..**.**.....***...=####=............................=####=............................=####=...~.~.~.~......~~..***..~~.=########",
                                                    "#######################################################################################################################################################################################"};

                InitTilesStep<StairsMapGenContext> startStep = new InitTilesStep<StairsMapGenContext>();
                int width = level[0].Length;
                int height = level.Length;
                startStep.Width = width;
                startStep.Height = height;

                layout.GenSteps.Add(PR_TILES_INIT, startStep);

                SpecificTilesStep<StairsMapGenContext> drawStep = new SpecificTilesStep<StairsMapGenContext>();
                drawStep.Offset = new Loc(0);

                drawStep.Tiles = new Tile[width][];
                for (int ii = 0; ii < width; ii++)
                {
                    drawStep.Tiles[ii] = new Tile[height];
                    for (int jj = 0; jj < height; jj++)
                    {
                        if (level[jj][ii] == '#')
                            drawStep.Tiles[ii][jj] = new Tile(2);
                        else if (level[jj][ii] == '~')
                            drawStep.Tiles[ii][jj] = new Tile(3);
                        else
                        {
                            EffectTile effect = new EffectTile();
                            if (level[jj][ii] == '=') //Reset Tile
                                effect = new EffectTile(42, true);
                            else if (level[jj][ii] == '+') //Wonder Tile
                                effect = new EffectTile(27, true);
                            else if (level[jj][ii] == '%') //Chestnut
                                effect = new EffectTile(17, true);
                            else if (level[jj][ii] == '&') //Poison
                                effect = new EffectTile(3, true);
                            else if (level[jj][ii] == '/') //Mud
                                effect = new EffectTile(7, true);
                            else if (level[jj][ii] == '(') //Sticky Trap
                                effect = new EffectTile(11, true);
                            else if (level[jj][ii] == ')') //PP-Leech Trap
                                effect = new EffectTile(9, true);
                            else if (level[jj][ii] == '|') //Gust Trap
                                effect = new EffectTile(14, true);
                            else if (level[jj][ii] == '!') //Self Destruct
                                effect = new EffectTile(18, true);
                            else if (level[jj][ii] == '?') //Hidden Sticky
                                effect = new EffectTile(11, false);
                            else if (level[jj][ii] == '$') //Hidden Mud
                                effect = new EffectTile(7, false);
                            else if (level[jj][ii] == '`') //Hidden Chestnut
                                effect = new EffectTile(17, false);
                            else if (level[jj][ii] == ':') //Trigger
                                effect = new EffectTile(28, true);
                            else if (level[jj][ii] == '0') //Lv5
                            {
                                effect = new EffectTile(2, true);
                                effect.TileStates.Set(new DestState(new SegLoc(0, 5 - 1), false));
                            }
                            else if (level[jj][ii] == '1') //Lv10
                            {
                                effect = new EffectTile(2, true);
                                effect.TileStates.Set(new DestState(new SegLoc(0, 10 - 1), false));
                            }
                            else if (level[jj][ii] == '2') //Lv25
                            {
                                effect = new EffectTile(2, true);
                                effect.TileStates.Set(new DestState(new SegLoc(0, 25 - 1), false));
                            }
                            else if (level[jj][ii] == '3') //Lv50
                            {
                                effect = new EffectTile(2, true);
                                effect.TileStates.Set(new DestState(new SegLoc(0, 50 - 1), false));
                            }
                            else if (level[jj][ii] == '4') //Exit
                            {
                                effect = new EffectTile(1, true);
                                effect.TileStates.Set(new DestState(SegLoc.Invalid, false));
                            }
                            else if (level[jj][ii] == 'A') //Sign: Level 5
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_LV_005")));
                            }
                            else if (level[jj][ii] == 'B') //Sign: Level 10
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_LV_010")));
                            }
                            else if (level[jj][ii] == 'C') //Sign: Level 25
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_LV_025")));
                            }
                            else if (level[jj][ii] == 'D') //Sign: Level 50
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_LV_050")));
                            }
                            else if (level[jj][ii] == 'E') //Sign: 1.1
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_MOVES"), new LocalFormatControls("SIGN_TUTORIAL_1_1", FrameInput.InputType.Skills)));
                            }
                            else if (level[jj][ii] == 'F') //Sign: 1.2
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_TYPES"), new LocalFormatControls("SIGN_TUTORIAL_1_2")));
                            }
                            else if (level[jj][ii] == 'G') //Sign: 1.3
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_TYPES"), new LocalFormatControls("SIGN_TUTORIAL_1_3")));
                            }
                            else if (level[jj][ii] == 'H') //Sign: 2.1
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_CATEGORY"), new LocalFormatControls("SIGN_TUTORIAL_2_1")));
                            }
                            else if (level[jj][ii] == 'I') //Sign: 2.2
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_HIT_RATE"), new LocalFormatControls("SIGN_TUTORIAL_2_2")));
                            }
                            else if (level[jj][ii] == 'J') //Sign: 2.3
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_MOVEMENT_SPEED"), new LocalFormatControls("SIGN_TUTORIAL_2_3")));
                            }
                            else if (level[jj][ii] == 'K') //Sign: 3.1
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_STATUS"), new LocalFormatControls("SIGN_TUTORIAL_3_1")));
                            }
                            else if (level[jj][ii] == 'L') //Sign: 3.2
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_STATUS"), new LocalFormatControls("SIGN_TUTORIAL_3_2")));
                            }
                            else if (level[jj][ii] == 'M') //Sign: 3.3
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_ABILITY"), new LocalFormatControls("SIGN_TUTORIAL_3_3")));
                            }
                            else if (level[jj][ii] == 'N') //Sign: 4.1
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_TRAP"), new LocalFormatControls("SIGN_TUTORIAL_4_1")));
                            }
                            else if (level[jj][ii] == 'O') //Sign: 4.2
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_TRAP"), new LocalFormatControls("SIGN_TUTORIAL_4_2", FrameInput.InputType.Attack)));
                            }
                            else if (level[jj][ii] == 'P') //Sign: 4.3
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_TRAP"), new LocalFormatControls("SIGN_TUTORIAL_4_3")));
                            }
                            else if (level[jj][ii] == 'Q') //Sign: 5.1
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_MOVEMENT"), new LocalFormatControls("SIGN_TUTORIAL_5_1", FrameInput.InputType.Turn, FrameInput.InputType.Diagonal)));
                            }
                            else if (level[jj][ii] == 'R') //Sign: 5.2
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_TURNS"), new LocalFormatControls("SIGN_TUTORIAL_5_2", FrameInput.InputType.Run, FrameInput.InputType.Attack)));
                            }
                            else if (level[jj][ii] == 'S') //Sign: 5.3
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_TEAM"), new LocalFormatControls("SIGN_TUTORIAL_5_3", FrameInput.InputType.LeaderSwap1, FrameInput.InputType.LeaderSwap2, FrameInput.InputType.LeaderSwap3, FrameInput.InputType.LeaderSwap4)));
                            }
                            else if (level[jj][ii] == 'T') //Sign: 5.4
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_TITLE_TEAM"), new LocalFormatControls("SIGN_TUTORIAL_5_4", FrameInput.InputType.TeamMode)));
                            }
                            else if (level[jj][ii] == 'U') //Sign: Exit
                            {
                                effect = new EffectTile(43, true);
                                effect.TileStates.Set(new NoticeState(new LocalFormatSimple("SIGN_EXIT")));
                            }
                            Tile tile = new Tile(0);
                            tile.Effect = effect;
                            drawStep.Tiles[ii][jj] = tile;
                        }
                    }
                }


                layout.GenSteps.Add(PR_TILES_GEN, drawStep);

                //add border
                layout.GenSteps.Add(PR_TILES_BARRIER, new UnbreakableBorderStep<StairsMapGenContext>(1));

                layout.GenSteps.Add(PR_FLOOR_DATA, new MapExtraStatusStep<StairsMapGenContext>(27));

                ActiveEffect activeEffect = new ActiveEffect();
                activeEffect.OnMapStarts.Add(-15, new PrepareLevelEvent(floor_level));
                MapEffectStep<StairsMapGenContext> mapEvents = new MapEffectStep<StairsMapGenContext>(activeEffect);
                layout.GenSteps.Add(PR_FLOOR_DATA, mapEvents);


                //Entrance
                List<(MapGenEntrance, Loc)> entrances = new List<(MapGenEntrance, Loc)>();
                entrances.Add((new MapGenEntrance(Dir8.Down), new Loc(3, 8)));
                AddSpecificSpawn(layout, entrances, PR_EXITS);



                //items
                List<(InvItem, Loc)> items = new List<(InvItem, Loc)>();

                items.Add((new InvItem(210), new Loc(13, 10)));//Plain Apricorn
                items.Add((new InvItem(210), new Loc(14, 10)));//Plain Apricorn
                items.Add((new InvItem(210), new Loc(15, 10)));//Plain Apricorn
                items.Add((new InvItem(210), new Loc(16, 10)));//Plain Apricorn
                items.Add((new InvItem(210), new Loc(17, 10)));//Plain Apricorn

                items.Add((new InvItem(216), new Loc(24, 11)));//White Apricorn
                items.Add((new InvItem(215), new Loc(22, 10)));//Red Apricorn
                items.Add((new InvItem(211), new Loc(23, 10)));//Blue Apricorn
                items.Add((new InvItem(212), new Loc(24, 10)));//Green Apricorn
                items.Add((new InvItem(217), new Loc(25, 10)));//Yellow Apricorn
                items.Add((new InvItem(213), new Loc(26, 10)));//Brown Apricorn

                items.Add((new InvItem(451), new Loc(31, 10)));//Assembly Box
                items.Add((new InvItem(451), new Loc(32, 10)));//Assembly Box
                items.Add((new InvItem(451), new Loc(33, 10)));//Assembly Box
                items.Add((new InvItem(451), new Loc(34, 10)));//Assembly Box
                items.Add((new InvItem(451), new Loc(35, 10)));//Assembly Box
                items.Add((new InvItem(450), new Loc(31, 11)));//Link Box
                items.Add((new InvItem(450), new Loc(32, 11)));//Link Box
                items.Add((new InvItem(450), new Loc(33, 11)));//Link Box
                items.Add((new InvItem(450), new Loc(34, 11)));//Link Box
                items.Add((new InvItem(450), new Loc(35, 11)));//Link Box



                items.Add((new InvItem(175), new Loc(45, 10)));//X Attack
                items.Add((new InvItem(175), new Loc(46, 10)));//X Attack
                items.Add((new InvItem(175), new Loc(45, 11)));//X Attack
                items.Add((new InvItem(175), new Loc(46, 11)));//X Attack

                items.Add((new InvItem(176), new Loc(47, 10)));//X Defense
                items.Add((new InvItem(176), new Loc(48, 10)));//X Defense
                items.Add((new InvItem(176), new Loc(47, 11)));//X Defense
                items.Add((new InvItem(176), new Loc(48, 11)));//X Defense

                items.Add((new InvItem(177), new Loc(50, 10)));//X Sp. Atk
                items.Add((new InvItem(177), new Loc(51, 10)));//X Sp. Atk
                items.Add((new InvItem(177), new Loc(50, 11)));//X Sp. Atk
                items.Add((new InvItem(177), new Loc(51, 11)));//X Sp. Atk

                items.Add((new InvItem(178), new Loc(52, 10)));//X Sp. Def
                items.Add((new InvItem(178), new Loc(53, 10)));//X Sp. Def
                items.Add((new InvItem(178), new Loc(52, 11)));//X Sp. Def
                items.Add((new InvItem(178), new Loc(53, 11)));//X Sp. Def

                items.Add((new InvItem(180), new Loc(56, 10)));//X Accuracy
                items.Add((new InvItem(180), new Loc(57, 10)));//X Accuracy
                items.Add((new InvItem(180), new Loc(56, 11)));//X Accuracy
                items.Add((new InvItem(180), new Loc(57, 11)));//X Accuracy

                items.Add((new InvItem(278), new Loc(59, 10)));//All-Dodge Orb
                items.Add((new InvItem(278), new Loc(60, 10)));//All-Dodge Orb
                items.Add((new InvItem(278), new Loc(59, 11)));//All-Dodge Orb
                items.Add((new InvItem(278), new Loc(60, 11)));//All-Dodge Orb

                items.Add((new InvItem(179), new Loc(66, 10)));//X Speed
                items.Add((new InvItem(179), new Loc(67, 10)));//X Speed
                items.Add((new InvItem(179), new Loc(68, 10)));//X Speed
                items.Add((new InvItem(179), new Loc(66, 11)));//X Speed
                items.Add((new InvItem(179), new Loc(67, 11)));//X Speed
                items.Add((new InvItem(179), new Loc(68, 11)));//X Speed

                items.Add((new InvItem(12), new Loc(80, 10)));//Lum Berry
                items.Add((new InvItem(12), new Loc(83, 10)));//Lum Berry
                items.Add((new InvItem(12), new Loc(86, 10)));//Lum Berry
                items.Add((new InvItem(12), new Loc(89, 10)));//Lum Berry
                items.Add((new InvItem(12), new Loc(92, 10)));//Lum Berry
                items.Add((new InvItem(12), new Loc(95, 10)));//Lum Berry
                items.Add((new InvItem(12), new Loc(98, 10)));//Lum Berry
                items.Add((new InvItem(12), new Loc(101, 10)));//Lum Berry
                items.Add((new InvItem(12), new Loc(104, 10)));//Lum Berry

                items.Add((new InvItem(10), new Loc(114, 10)));//Oran Berry
                items.Add((new InvItem(10), new Loc(116, 10)));//Oran Berry
                items.Add((new InvItem(11), new Loc(118, 10)));//Leppa Berry
                items.Add((new InvItem(11), new Loc(120, 10)));//Leppa Berry
                items.Add((new InvItem(263), new Loc(122, 10)));//Cleanse Orb
                items.Add((new InvItem(263), new Loc(124, 10)));//Cleanse Orb
                items.Add((new InvItem(260), new Loc(126, 11)));//Revival Orb
                items.Add((new InvItem(263), new Loc(128, 10)));//Cleanse Orb
                items.Add((new InvItem(263), new Loc(130, 10)));//Cleanse Orb
                items.Add((new InvItem(11), new Loc(132, 10)));//Leppa Berry
                items.Add((new InvItem(11), new Loc(134, 10)));//Leppa Berry
                items.Add((new InvItem(10), new Loc(136, 10)));//Oran Berry
                items.Add((new InvItem(10), new Loc(138, 10)));//Oran Berry

                items.Add((new InvItem(303), new Loc(166, 11)));//Mobile Scarf
                items.Add((new InvItem(312), new Loc(167, 11)));//Shell Bell
                items.Add((new InvItem(318), new Loc(168, 11)));//Choice Band

                AddSpecificSpawn(layout, items, PR_SPAWN_ITEMS);


                //enemies

                PresetMultiTeamSpawner<StairsMapGenContext> presetMultiSpawner = new PresetMultiTeamSpawner<StairsMapGenContext>();
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(13, 5), 161, 51, 33, -1, -1, -1, floor_level));// Sentret : Tackle
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(16, 5), 152, -1, 75, -1, -1, -1, floor_level));// Chikorita : Razor Leaf
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(19, 5), 155, -1, 52, -1, -1, -1, floor_level));// Cyndaquil : Ember
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(22, 5), 158, -1, 55, -1, -1, -1, floor_level));// Totodile : Water Gun
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(25, 5), 25, 50, 84, -1, -1, -1, floor_level));// Pachirisu : Thunder Shock
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(14, 2), 27, -1, 328, -1, -1, -1, floor_level));// Sandshrew : Sand Tomb
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(17, 2), 438, 69, 88, -1, -1, -1, floor_level));// Bonsly : Rock Throw
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(21, 2), 276, -1, 17, -1, -1, -1, floor_level));// Taillow : Wing Attack
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(24, 2), 23, 61, 40, -1, -1, -1, floor_level));// Ekans : Poison Sting

                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(30, 5), 188, 34, 71, -1, -1, -1, floor_level));// Skiploom : Absorb
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(33, 5), 259, -1, 55, -1, -1, -1, floor_level));// Marshtomp : Water Gun
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(36, 5), 75, 69, 88, -1, -1, -1, floor_level));// Graveler : Rock Throw


                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(46, 2), 175, 32, 204, -1, -1, -1, floor_level));// Togepi : Charm
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(48, 2), 441, 51, 590, -1, -1, -1, floor_level));// Chatot : Confide
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(50, 2), 90, -1, 334, -1, -1, -1, floor_level));// Shellder : Iron Defense
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(52, 2), 446, -1, 133, -1, -1, -1, floor_level));// Munchlax : Amnesia
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(46, 5), 52, 101, 45, -1, -1, -1, floor_level));// Meowth : Growl
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(48, 5), 401, -1, 522, -1, -1, -1, floor_level));// Kricketot : Struggle Bug
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(50, 5), 11, -1, 106, -1, -1, -1, floor_level));// Metapod : Harden
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(52, 5), 35, -1, 322, -1, -1, -1, floor_level));// Clefairy : Cosmic Power

                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(58, 2), 280, 28, 104, -1, -1, -1, floor_level));// Ralts : Double Team
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(58, 5), 27, -1, 28, -1, -1, -1, floor_level));// Sandshrew : Sand Attack

                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(67, 2), 158, -1, 184, -1, -1, -1, floor_level));// Totodile : Scary Face
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(67, 5), 10, -1, 81, -1, -1, -1, floor_level));// Caterpie : String Shot


                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(81, 2), 37, -1, 261, -1, -1, -1, floor_level));// Vulpix : Will-o-Wisp
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(85, 2), 163, -1, 95, -1, -1, -1, floor_level));// Hoothoot : Hypnosis
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(81, 5), 109, -1, 139, -1, -1, -1, floor_level));// Koffing : Poison Gas
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(85, 5), 25, 50, 86, -1, -1, -1, floor_level));// Pachirisu : Thunder Wave

                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(90, 2), 42, -1, 212, -1, -1, -1, floor_level));// Golbat : Mean Look
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(94, 2), 431, -1, 213, -1, -1, -1, floor_level));// Glameow : Attract
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(90, 5), 313, -1, 109, -1, -1, -1, floor_level));// Volbeat : Confuse Ray
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(94, 5), 187, -1, 73, -1, -1, -1, floor_level));// Hoppip : Leech Seed

                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(99, 5), 218, 49, -1, -1, -1, -1, floor_level));// Slugma : Flame Body
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(101, 5), 434, 106, -1, -1, -1, -1, floor_level));// Stunky : Aftermath
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(103, 5), 271, 44, 240, -1, -1, -1, floor_level));// Lombre : Rain Dish : Rain Dance


                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(134, 3), 161, 51, -1, -1, -1, -1, floor_level));// Sentret
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(136, 3), 161, 51, -1, -1, -1, -1, floor_level));// Sentret
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(134, 5), 161, 51, -1, -1, -1, -1, floor_level));// Sentret
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(136, 5), 161, 51, -1, -1, -1, -1, floor_level));// Sentret


                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(156, 2), 132, -1, 144, -1, -1, -1, floor_level));// Ditto : Transform
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(158, 2), 132, -1, 144, -1, -1, -1, floor_level));// ''
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(161, 2), 132, -1, 144, -1, -1, -1, floor_level));// ''
                presetMultiSpawner.Spawns.Add(CreateSetMobTeam(new Loc(163, 2), 132, -1, 144, -1, -1, -1, floor_level));// ''
                PlaceNoLocMobsStep<StairsMapGenContext> mobStep = new PlaceNoLocMobsStep<StairsMapGenContext>(presetMultiSpawner);
                layout.GenSteps.Add(PR_SPAWN_MOBS, mobStep);

                //Tilesets
                AddTextureData(layout, 87, 88, 89, 13);

                structure.Floors[floor_level - 1] = layout;

                //structure.MainExit = ZoneLoc.Invalid;
                zone.Segments.Add(structure);
            }
        }

        #endregion
    }
}
