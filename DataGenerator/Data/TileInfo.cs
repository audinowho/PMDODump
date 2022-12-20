using System;
using System.Collections.Generic;
using RogueEssence.Dungeon;
using RogueEssence.Content;
using RogueElements;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using RogueEssence;
using RogueEssence.Data;
using PMDC.Dungeon;
using PMDC;
using PMDC.Data;
using Microsoft.Xna.Framework;

namespace DataGenerator.Data
{
    public static class TileInfo
    {
        public const int MAX_TILES = 80;

        public static void AddTileData()
        {
            DataInfo.DeleteIndexedData(DataManager.DataType.Tile.ToString());
            for (int ii = 0; ii < MAX_TILES; ii++)
            {
                (string, TileData) tile = GetTileData(ii);
                if (tile.Item1 != "")
                    DataManager.SaveData(tile.Item1, DataManager.DataType.Tile.ToString(), tile.Item2);
            }
        }




        public static (string, TileData) GetTileData(int ii)
        {
            string fileName = "";
            TileData tile = new TileData();
            if (ii == 0)
            {
                tile.Name = new LocalText("None");
                tile.Desc = new LocalText("This tile has no effect.");
                tile.Anim = new ObjAnimData("Trap_Hidden", 1);
                tile.MinimapIcon = new Loc(2, 0);
                tile.MinimapColor = Color.Red;
            }
            else if (ii == 1)
            {
                tile.Name = new LocalText("Stairs");
                fileName = "stairs_go_up";
                tile.Desc = new LocalText("Stairs leading to the next floor. If you are on the final floor, you will escape from the dungeon.");
                tile.Comment = "Up";
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Passage;
                tile.Anim = new ObjAnimData("Stairs_Up", 1);
                tile.MinimapIcon = new Loc(4, 0);
                tile.MinimapColor = Color.Cyan;
                tile.LandedOnTiles.Add(0, new RevealSecretEvent());
                tile.LandedOnTiles.Add(0, new AskLeaderEvent());
                tile.InteractWithTiles.Add(0, new NextFloorEvent());
                tile.InteractWithTiles.Add(0, new SwitchMapEvent());
            }
            else if (ii == 2)
            {
                tile.Name = new LocalText("Stairs");
                fileName = "stairs_go_down";
                tile.Desc = new LocalText("Stairs leading to the next floor. If you are on the final floor, you will escape from the dungeon.");
                tile.Comment = "Down";
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Passage;
                tile.Anim = new ObjAnimData("Stairs_Down", 1);
                tile.MinimapIcon = new Loc(4, 0);
                tile.MinimapColor = Color.Cyan;
                tile.LandedOnTiles.Add(0, new RevealSecretEvent());
                tile.LandedOnTiles.Add(0, new AskLeaderEvent());
                tile.InteractWithTiles.Add(0, new NextFloorEvent());
                tile.InteractWithTiles.Add(0, new SwitchMapEvent());
            }
            else if (ii == 3)
            {
                tile.Name = new LocalText("Poison Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap will shoot poison spikes in all directions. Pokémon hit will become poisoned.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Poison", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                //SE 121

                ProjectileAction altAction = new ProjectileAction();
                altAction.Range = 8;
                altAction.Speed = 10;
                altAction.Rays = ProjectileAction.RayCount.FourCross;
                altAction.StopAtWall = true;
                altAction.StopAtHit = true;
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                altAction.Anim = new AnimData("Pin_Missile", 3);

                altAction.ActionFX.Sound = "DUN_Throw_Spike";
                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHits.Add(0, new StatusBattleEvent("poison", true, false, true));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, false));
            }
            else if (ii == 4)
            {
                tile.Name = new LocalText("Slumber Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap releases a sleeping gas that puts all nearby Pokémon to sleep.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Slumber", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.Range = 1;
                altAction.Speed = 8;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Puff_Pink", 3));
                emitter.ParticlesPerTile = 3;
                altAction.Emitter = emitter;
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                altAction.LagBehindTime = 40;

                altAction.ActionFX.Sound = "DUN_Summon_Trap_2";
                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHits.Add(0, new StatusBattleEvent("sleep", true, false, true));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, true));
            }
            else if (ii == 5)
            {
                tile.Name = new LocalText("Spin Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap spins all nearby Pokémon around dizzyingly, making them confused.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Spin", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                //SE 198

                AreaAction altAction = new AreaAction();
                altAction.Range = 1;
                altAction.Speed = 10;
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter();
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Blue", 3, -1, -1, 192)));
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Red", 3, -1, -1, 192)));
                emitter.ParticlesPerTile = 0.8;
                altAction.Emitter = emitter;
                altAction.LagBehindTime = 40;
                altAction.ActionFX.Sound = "DUN_Light_Screen";

                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHits.Add(0, new StatusBattleEvent("confuse", true, false, true));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, false));
            }
            else if (ii == 6)
            {
                tile.Name = new LocalText("Slow Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap lowers the Movement Speed of nearby Pokémon by one level.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Slow", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.Range = 1;
                altAction.Speed = 10;
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter();
                emitter.Anims.Add(new SingleEmitter(new BeamAnimData("Column_Pink", 3, -1, -1, 192)));
                emitter.ParticlesPerTile = 0.8;
                altAction.Emitter = emitter;
                altAction.LagBehindTime = 40;
                altAction.ActionFX.Sound = "DUN_Light_Screen";

                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, true, -1));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, false));
            }
            else if (ii == 7)
            {
                tile.Name = new LocalText("Mud Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap makes it blow mud that sharply lowers the highest base stat of all nearby Pokémon.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Mud", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.Range = 1;
                altAction.Speed = 10;
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Sticky_Brown_Back", 3), new AnimData("Sticky_Brown_Front", 3));
                emitter.HeightBack = 36;
                emitter.HeightFront = 20;
                altAction.ActionFX.Emitter = emitter;
                altAction.LagBehindTime = 40;

                altAction.ActionFX.Sound = "DUN_Mud_Trap";
                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHits.Add(0, new AffectHighestStatBattleEvent(true, "mod_attack", "mod_defense", "mod_special_attack", "mod_special_defense", true, -2));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, false));
            }
            else if (ii == 8)
            {
                tile.Name = new LocalText("Seal Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap disables the last-used move of all nearby Pokémon.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Seal", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.Range = 1;
                altAction.Speed = 10;
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                altAction.LagBehindTime = 40;
                BattleFX altFX = new BattleFX();
                altFX.Sound = "DUN_Sealed";
                altAction.PreActions.Add(altFX);
                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitFX.Emitter = new SingleEmitter(new AnimData("X_RSE", 15), 3);
                newData.HitRate = -1;
                newData.OnHits.Add(0, new DisableBattleEvent("disable", "last_used_move_slot", true));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, true));
            }
            else if (ii == 9)
            {
                tile.Name = new LocalText("PP-Leech Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap completely drains the PP of the Pokémon's last-used move.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_PP_Zero", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                //SE 79

                SelfAction altAction = new SelfAction();
                altAction.TargetAlignments = Alignment.Self;

                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = Alignment.Self;
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHits.Add(0, new SpiteEvent("last_used_move_slot", 100));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, true));
            }
            else if (ii == 10)
            {
                tile.Name = new LocalText("Grimy Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap makes it spew a muck that gums up food and removes abilities.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Grimy", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.Range = 1;
                altAction.Speed = 10;
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Sticky_Purple_Back", 3), new AnimData("Sticky_Purple_Front", 3));
                emitter.HeightBack = 36;
                emitter.HeightFront = 20;
                altAction.ActionFX.Emitter = emitter;
                altAction.LagBehindTime = 40;

                altAction.ActionFX.Sound = "DUN_Grimy_Trap";
                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                HashSet<FlagType> eligibles = new HashSet<FlagType>();
                eligibles.Add(new FlagType(typeof(FoodState)));
                eligibles.Add(new FlagType(typeof(GummiState)));
                newData.OnHits.Add(0, new TransformItemEvent(true, false, "seed_decoy", "food_grimy", eligibles));
                newData.OnHits.Add(0, new ChangeToAbilityEvent("none", true));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, false));
            }
            else if (ii == 11)
            {
                tile.Name = new LocalText("Sticky Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap makes all nearby Pokémon immobilized. It will also cause items to become sticky.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Sticky", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.Range = 1;
                altAction.Speed = 10;
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BetweenEmitter emitter = new BetweenEmitter(new AnimData("Sticky_Green_Back", 3), new AnimData("Sticky_Green_Front", 3));
                emitter.HeightBack = 36;
                emitter.HeightFront = 20;
                altAction.ActionFX.Emitter = emitter;
                altAction.LagBehindTime = 40;

                altAction.ActionFX.Sound = "DUN_Sticky_Trap";
                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHits.Add(0, new SetItemStickyEvent(true, false, "seed_decoy", true, new HashSet<FlagType>()));
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(5));
                newData.OnHits.Add(0, new StatusStateBattleEvent("immobilized", true, false, true, statusStates));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, true));
            }
            else if (ii == 12)
            {
                tile.Name = new LocalText("Apple Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap turns a held-effect item into an apple.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Apple", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.TileEmitter = new SingleEmitter(new AnimData("Puff_Pink", 3));
                altAction.ActionFX.Sound = "DUN_Trace";
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);

                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);

                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;

                HashSet<FlagType> eligibles = new HashSet<FlagType>();
                eligibles.Add(new FlagType(typeof(HeldState)));
                newData.OnHits.Add(0, new TransformItemEvent(true, false, "seed_decoy", "food_apple", eligibles));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, true));
            }
            else if (ii == 13)
            {
                tile.Name = new LocalText("Warp Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap instantly warps the Pokémon to another spot on the floor.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Warp", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                //SE 156

                SelfAction altAction = new SelfAction();
                altAction.TargetAlignments = Alignment.Self;

                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = Alignment.Self;
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitFX.Sound = "DUN_Eyedrop";

                newData.HitRate = -1;
                newData.OnHits.Add(0, new RandomWarpEvent(50, true));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, false));
            }
            else if (ii == 14)
            {
                tile.Name = new LocalText("Gust Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Activating this trap triggers a gust of wind that sends nearby Pokémon flying.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Gust", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                FiniteAreaEmitter emitter = new FiniteAreaEmitter(new AnimData("Gust", 1));
                emitter.Speed = 96;
                emitter.Range = 24;
                emitter.TotalParticles = 3;
                emitter.LocHeight = 24;
                altAction.ActionFX.Emitter = emitter;
                altAction.ActionFX.Sound = "DUN_Gust_Trap";
                altAction.LagBehindTime = 40;
                altAction.HitTiles = true;

                ExplosionData altExplosion = new ExplosionData();
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHitTiles.Add(0, new LaunchAllEvent(8));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, false));
            }
            else if (ii == 15)
            {
                tile.Name = new LocalText("Summon Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap releases a sweet aroma that attracts Pokémon from afar.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Summon", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.ActionFX.Sound = "DUN_Pitfall_Trap";
                altAction.ActionFX.Delay = 20;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Puff_Pink", 3));
                emitter.ParticlesPerTile = 3;
                emitter.RangeDiff = 8;
                altAction.Emitter = emitter;
                altAction.HitTiles = true;

                ExplosionData altExplosion = new ExplosionData();

                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHitTiles.Add(0, new WarpFoesToTileEvent(80, 4));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, true));
            }
            else if (ii == 16)
            {
                tile.Name = new LocalText("**Pokémon Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap transforms all items in the area into hostile Pokémon.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Pokemon", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.Speed = 10;
                altAction.HitTiles = true;

                ExplosionData altExplosion = new ExplosionData();
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, true));
            }
            else if (ii == 17)
            {
                tile.Name = new LocalText("Chestnut Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap makes spiky unshelled Chestnuts drop, inflicting damage on nearby Pokémon and causing them to flinch.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Chestnut", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.Range = 1;
                altAction.Speed = 40;
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                SingleEmitter emitter = new SingleEmitter(new AnimData("Chestnut_Trap", 1));
                emitter.LocHeight = 112;
                altAction.TileEmitter = emitter;
                altAction.LagBehindTime = 35;

                altAction.ActionFX.Sound = "DUN_Chestnut_Trap";
                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.HitCharAction = new CharAnimFrameType(04);//Hurt
                newData.OnHits.Add(0, new IndirectDamageEvent(6));
                newData.OnHits.Add(0, new StatusBattleEvent("flinch", true, true));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, false));
            }
            else if (ii == 18)
            {
                tile.Name = new LocalText("Self-Destruct Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap makes it explode. Its damage extends to everything near the trap.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Self_Destruct", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.HitTiles = true;
                altAction.BurstTiles = TileAlignment.Any;
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);

                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                altExplosion.Range = 1;
                altExplosion.HitTiles = true;
                altExplosion.Speed = 7;
                altExplosion.ExplodeFX.Sound = "DUN_Self-Destruct";
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Explosion", 3));
                emitter.ParticlesPerTile = 0.7;
                altExplosion.Emitter = emitter;
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHits.Add(-1, new CutHPDamageEvent());
                newData.OnHitTiles.Add(0, new RemoveTrapEvent());
                newData.OnHitTiles.Add(0, new RemoveItemEvent(true));
                newData.OnHitTiles.Add(0, new RemoveTerrainEvent("", new EmptyFiniteEmitter(), "wall"));
                newData.OnHitTiles.Add(0, new RemoveTerrainEvent("", new EmptyFiniteEmitter(), "grass"));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, true));
            }
            else if (ii == 19)
            {
                tile.Name = new LocalText("Explosion Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Activating this trap triggers a huge explosion. Its damage extends to everything up to 2 steps away from it.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Explosion", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.HitTiles = true;
                altAction.BurstTiles = TileAlignment.Any;
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);

                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                altExplosion.Range = 2;
                altExplosion.HitTiles = true;
                altExplosion.Speed = 7;
                altExplosion.ExplodeFX.Sound = "DUN_Self-Destruct";
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Explosion", 3));
                emitter.ParticlesPerTile = 0.5;
                altExplosion.Emitter = emitter;
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHits.Add(-1, new CutHPDamageEvent());
                newData.OnHitTiles.Add(0, new RemoveTrapEvent());
                newData.OnHitTiles.Add(0, new RemoveItemEvent(true));
                newData.OnHitTiles.Add(0, new RemoveTerrainEvent("", new EmptyFiniteEmitter(), "wall"));
                newData.OnHitTiles.Add(0, new RemoveTerrainEvent("", new EmptyFiniteEmitter(), "grass"));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, false));
            }
            else if (ii == 20)
            {
                tile.Name = new LocalText("Spikes");
                fileName = "trap_spikes";
                tile.Desc = new LocalText("Stepping on this trap damages the Pokémon standing on it.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Spikes", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);

                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHits.Add(0, new IndirectDamageEvent(8));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, true));
            }
            else if (ii == 21)
            {
                tile.Name = new LocalText("Toxic Spikes");
                fileName = "trap_toxic_spikes";
                tile.Desc = new LocalText("Stepping on this trap badly poisons the Pokémon standing on it.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Toxic_Spikes", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);

                altAction.ActionFX.Sound = "DUN_Poison_Trap";
                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHits.Add(0, new StatusBattleEvent("poison_toxic", true, false, true));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, true));
            }
            else if (ii == 22)
            {
                tile.Name = new LocalText("Stealth Rock");
                fileName = "trap_stealth_rock";
                tile.Desc = new LocalText("Stepping on this trap damages the Pokémon standing on it with a Rock-type attack.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Stealth_Rock", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);

                altAction.ActionFX.Sound = "DUN_Hit_Neutral";
                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHits.Add(-1, new IndirectElementDamageEvent("rock", 8));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, true));
            }
            else if (ii == 23)
            {
                tile.Name = new LocalText("Trip Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap causes nearby Pokémon to trip and drop their items.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Trip", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                //SE 72

                AreaAction altAction = new AreaAction();
                altAction.Range = 1;
                altAction.Speed = 10;
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);

                altAction.ActionFX.Sound = "DUN_Trapbust_Orb";
                altAction.ActionFX.ScreenMovement = new ScreenMover(0, 6, 30);
                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                //newData.SkillStates.Add(typeof(BasePowerState).FullName, new BasePowerState());
                //newData.BeforeHits.Add(0, new WeightBasePowerEffect());
                //newData.HitEffects.Add(-1, new DamageFormulaEffect());
                newData.HitCharAction = new CharAnimFrameType(04);//Hurt
                newData.OnHits.Add(0, new DropItemEvent(false, false, "seed_decoy", new HashSet<FlagType>(), new StringKey(), false));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, false));
            }
            else if (ii == 24)
            {
                tile.Name = new LocalText("Grudge Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap gives the Grudge status to all opposing Pokémon in the area.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Grudge", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.Range = 5;
                altAction.Speed = 10;
                altAction.TargetAlignments = Alignment.Foe;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Grudge_Lights", 3));
                emitter.ParticlesPerTile = 1;
                altAction.Emitter = emitter;

                altAction.ActionFX.Sound = "DUN_Stayaway_Orb";
                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = Alignment.Foe;
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHits.Add(0, new StatusBattleEvent("grudge", true, false, true));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, false));
            }
            else if (ii == 25)
            {
                tile.Name = new LocalText("Hunger Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap causes the Pokémon to become hungry.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Hunger", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                SelfAction altAction = new SelfAction();
                altAction.TargetAlignments = Alignment.Self;

                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = Alignment.Self;
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHits.Add(0, new RestoreBellyEvent(-20, true));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, false));
            }
            else if (ii == 26)
            {
                tile.Name = new LocalText("**Training Switch");
                fileName = "tile_training";
                tile.Desc = new LocalText("Stepping on this tile gives the Trained status to the party.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Switch;
                tile.Anim = new ObjAnimData("Tile_Wonder", 1);
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.Speed = 10;
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);

                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, true));
            }
            else if (ii == 27)
            {
                tile.Name = new LocalText("Wonder Tile");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("An odd floor tile seen in many dungeons. Triggering it resets nearby Pokémon's stats if they are boosted or reduced.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Tile_Wonder", 1);
                tile.MinimapIcon = new Loc(2, 0);
                tile.MinimapColor = Color.Cyan;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.Range = 1;
                altAction.Speed = 10;
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);

                CircleSquareSprinkleEmitter emitter = new CircleSquareSprinkleEmitter(new AnimData("Event_Gather_Sparkle", 10));
                emitter.ParticlesPerTile = 2.5;
                emitter.StartHeight = 0;
                emitter.HeightSpeed = 20;
                emitter.SpeedDiff = 15;
                altAction.Emitter = emitter;
                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.HitFX.Sound = "DUN_Wonder_Tile";
                newData.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 1));

                newData.OnHits.Add(0, new RemoveStateStatusBattleEvent(typeof(StatChangeState), true, new StringKey("MSG_BUFF_REMOVE")));
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, false));
            }
            else if (ii == 28)
            {
                tile.Name = new LocalText("Trigger Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap sets off all other traps in the area.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Random", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.Range = 5;
                altAction.Speed = 10;
                altAction.HitTiles = true;

                altAction.ActionFX.Sound = "DUN_Trapbust_Orb";
                altAction.ActionFX.ScreenMovement = new ScreenMover(0, 6, 30);
                ExplosionData altExplosion = new ExplosionData();
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHitTiles.Add(0, new TriggerTrapEvent());
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, true));
            }
            else if (ii == 29)
            {
                tile.Name = new LocalText("**Pitfall Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Stepping on this trap drops the Pokémon to the floor below, inflicting damage.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Trap_Pitfall", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);

                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, false));
            }
            else if (ii == 30)
            {
                tile.Name = new LocalText("**Discord Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap creates a discord among the party.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Tile_Wonder", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.Speed = 10;
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);

                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, false));
            }
            else if (ii == 31)
            {
                tile.Name = new LocalText("**Emera-Swap Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap swaps an emera on a looplet for another emera.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Tile_Wonder", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.Speed = 10;
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);

                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, false));
            }
            else if (ii == 32)
            {
                tile.Name = new LocalText("**Emera-Crush Trap");
                fileName = Text.Sanitize(RogueEssence.Dev.DevHelper.ReverseEndian(tile.Name.DefaultText).Replace("*", "")).ToLower();
                tile.Desc = new LocalText("Triggering this trap causes an emera on a looplet to come off and disappear.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Trap;
                tile.Anim = new ObjAnimData("Tile_Wonder", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Red;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());

                AreaAction altAction = new AreaAction();
                altAction.Speed = 10;
                altAction.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);

                ExplosionData altExplosion = new ExplosionData();
                altExplosion.TargetAlignments = (Alignment.Self | Alignment.Friend | Alignment.Foe);
                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                tile.InteractWithTiles.Add(0, new InvokeTrapEvent(altAction, altExplosion, newData, false));
            }
            else if (ii == 33)
            {
                tile.Name = new LocalText("Luminous Area");
                fileName = "tile_evo";
                tile.Desc = new LocalText("A mysterious area that allows Pokémon to evolve.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Site;
                tile.Anim = new ObjAnimData("Evolution", 1);
                tile.MinimapIcon = new Loc(4, 1);
                tile.MinimapColor = Color.Cyan;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());
                tile.InteractWithTiles.Add(0, new AskEvoEvent("evo_harmony_scarf"));
            }
            else if (ii == 34)
            {
                tile.Name = new LocalText("Secret Stairs");
                fileName = "stairs_secret_up";
                tile.Desc = new LocalText("Stairs leading to an unknown location.");
                tile.Comment = "Up";
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Passage;
                tile.Anim = new ObjAnimData("Stairs_Up", 1);
                tile.MinimapIcon = new Loc(4, 0);
                tile.MinimapColor = Color.Orange;
                tile.LandedOnTiles.Add(0, new RevealSecretEvent());
                tile.LandedOnTiles.Add(0, new AskLeaderEvent());
                tile.InteractWithTiles.Add(0, new NextFloorEvent());
                tile.InteractWithTiles.Add(0, new SwitchMapEvent());
            }
            else if (ii == 35)
            {
                tile.Name = new LocalText("Secret Stairs");
                fileName = "stairs_secret_down";
                tile.Desc = new LocalText("Stairs leading to an unknown location.");
                tile.Comment = "Down";
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Passage;
                tile.Anim = new ObjAnimData("Stairs_Down", 1);
                tile.MinimapIcon = new Loc(4, 0);
                tile.MinimapColor = Color.Orange;
                tile.LandedOnTiles.Add(0, new RevealSecretEvent());
                tile.LandedOnTiles.Add(0, new AskLeaderEvent());
                tile.InteractWithTiles.Add(0, new NextFloorEvent());
                tile.InteractWithTiles.Add(0, new SwitchMapEvent());
            }
            else if (ii == 36)
            {
                tile.Name = new LocalText("Empty Chest");
                fileName = "chest_empty";
                tile.ObjectLayer = true;
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Blocker;
                tile.Anim = new ObjAnimData("Chest_Open", 1);
                tile.MinimapIcon = new Loc(4, 1);
                tile.MinimapColor = Color.Red;
            }
            else if (ii == 37)
            {
                tile.Name = new LocalText("Treasure Chest");
                fileName = "chest_full";
                tile.ObjectLayer = true;
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Unlockable;
                tile.Anim = new ObjAnimData("Chest", 1);
                tile.MinimapIcon = new Loc(4, 1);
                tile.MinimapColor = Color.Cyan;
                tile.LandedOnTiles.Add(0, new AskUnlockEvent());//for doors/chests, this will be triggered when "talked to"
                tile.InteractWithTiles.Add(0, new ChestEvent());//for doors/chests, this will be triggered when the key is used
            }
            else if (ii == 38)
            {
                tile.Name = new LocalText("Signal Area");
                fileName = "tile_boss";
                tile.Desc = new LocalText("A mysterious signal that calls Pokémon to the area.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Site;
                tile.Anim = new ObjAnimData("Tile_Signal", 12);
                tile.MinimapIcon = new Loc(4, 1);
                tile.MinimapColor = Color.Cyan;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());
                tile.InteractWithTiles.Add(0, new LockdownTileEvent("map_clear_check"));
                tile.InteractWithTiles.Add(0, new BossSpawnEvent());
            }
            else if (ii == 39)
            {
                tile.Name = new LocalText("Sealed Door");
                fileName = "sealed_door";
                tile.ObjectLayer = true;
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Unlockable;
                tile.Anim = new ObjAnimData("Block_Vault", 1);
                tile.MinimapIcon = new Loc(4, 1);
                tile.MinimapColor = Color.Cyan;
                //tile.BlockLight = true;
                tile.LandedOnTiles.Add(0, new AskUnlockEvent());//for doors/chests, this will be triggered when "talked to"
                tile.InteractWithTiles.Add(0, new OpenSelfEvent());

                WindEmitter overlay = new WindEmitter(new AnimData("Wind_Leaves", 4), new AnimData("Wind_Leaves_Small", 3));
                overlay.Bursts = 4;
                overlay.BurstTime = 20;
                overlay.ParticlesPerBurst = 4;
                overlay.Speed = -420;
                overlay.SpeedDiff = 300;
                overlay.StartDistance = 32;
                overlay.Layer = DrawLayer.Front;

                OpenOtherPassageEvent openEvent = new OpenOtherPassageEvent();
                openEvent.TimeLimitStatus = "somethings_stirring";
                openEvent.Emitter = overlay;
                openEvent.Warning = new StringKey("MSG_TIME_WARNING_1");
                openEvent.WarningSE = "DUN_Wind";
                openEvent.WarningBGM = "C04. Wind.ogg";
                tile.InteractWithTiles.Add(0, openEvent);
            }
            else if (ii == 40)
            {
                tile.Name = new LocalText("Sealed Block");
                fileName = "sealed_block";
                tile.ObjectLayer = true;
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Blocker;
                tile.Anim = new ObjAnimData("Block_Blank", 1);
                tile.MinimapIcon = new Loc(4, 1);
                tile.MinimapColor = Color.Red;
                //tile.BlockLight = true;
            }
            else if (ii == 41)
            {
                tile.Name = new LocalText("Switch Tile");
                fileName = "tile_switch";
                tile.Desc = new LocalText("A switch that opens various blocked passageways found on the floor.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Switch;
                tile.Anim = new ObjAnimData("Tile_Reset", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Cyan;
                tile.LandedOnTiles.Add(0, new AskLeaderEvent());
                tile.InteractWithTiles.Add(0, new TriggerSwitchEvent(true));


                WindEmitter overlay = new WindEmitter(new AnimData("Wind_Leaves", 4), new AnimData("Wind_Leaves_Small", 3));
                overlay.Bursts = 4;
                overlay.BurstTime = 20;
                overlay.ParticlesPerBurst = 4;
                overlay.Speed = -420;
                overlay.SpeedDiff = 300;
                overlay.StartDistance = 32;
                overlay.Layer = DrawLayer.Front;

                OpenOtherPassageEvent openEvent = new OpenOtherPassageEvent();
                openEvent.TimeLimitStatus = "somethings_stirring";
                openEvent.Emitter = overlay;
                openEvent.Warning = new StringKey("MSG_TIME_WARNING_1");
                openEvent.WarningSE = "DUN_Wind";
                openEvent.WarningBGM = "C04. Wind.ogg";
                tile.InteractWithTiles.Add(0, openEvent);
            }
            else if (ii == 42)
            {
                tile.Name = new LocalText("Reset Tile");
                fileName = "tile_reset";
                tile.Desc = new LocalText("A switch that resets the floor, returning all items, traps, and wild Pokémon to their original positions.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Switch;
                tile.Anim = new ObjAnimData("Tile_Reset", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Cyan;
                tile.LandedOnTiles.Add(0, new AskLeaderEvent());
                tile.InteractWithTiles.Add(0, new ResetFloorEvent());
            }
            else if (ii == 43)
            {
                tile.Name = new LocalText("Sign");
                fileName = "sign";
                tile.ObjectLayer = true;
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Blocker;
                tile.Anim = new ObjAnimData("Sign", 1);
                tile.MinimapIcon = new Loc(4, 1);
                tile.MinimapColor = Color.Yellow;
                tile.LandedOnTiles.Add(0, new NoticeEvent());
            }
            else if (ii == 44)
            {
                tile.Name = new LocalText("Rescue Point");
                fileName = "tile_rescue";
                tile.Desc = new LocalText("It's the rescue spot where your friend's team went down! Send an A-OK mail to rescue the defeated team.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Passage;
                tile.Anim = new ObjAnimData("Tile_Rescue", 1);
                tile.MinimapIcon = new Loc(4, 0);
                tile.MinimapColor = Color.Orange;
                tile.LandedOnTiles.Add(0, new AskLeaderEvent());
                tile.InteractWithTiles.Add(0, new RescueEvent());
            }
            else if (ii == 45)
            {
                tile.Name = new LocalText("Shop Tile");
                fileName = "area_shop";
                tile.Desc = new LocalText("");
                tile.StepType = TileData.TriggerType.None;
                tile.Anim = new ObjAnimData("Tile_Shop", 1);
            }
            else if (ii == 46)
            {
                tile.Name = new LocalText("Exit Stairs");
                fileName = "stairs_exit_up";
                tile.Desc = new LocalText("Stairs leading to the dungeon's exit.");
                tile.Comment = "Up";
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Passage;
                tile.Anim = new ObjAnimData("Stairs_Up", 1);
                tile.MinimapIcon = new Loc(4, 0);
                tile.MinimapColor = Color.Cyan;
                tile.LandedOnTiles.Add(0, new RevealSecretEvent());
                tile.LandedOnTiles.Add(0, new AskLeaderEvent());
                tile.InteractWithTiles.Add(0, new NextFloorEvent());
                tile.InteractWithTiles.Add(0, new SwitchMapEvent());
            }
            else if (ii == 47)
            {
                tile.Name = new LocalText("Exit Stairs");
                fileName = "stairs_exit_down";
                tile.Desc = new LocalText("Stairs leading to the dungeon's exit.");
                tile.Comment = "Down";
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Passage;
                tile.Anim = new ObjAnimData("Stairs_Down", 1);
                tile.MinimapIcon = new Loc(4, 0);
                tile.MinimapColor = Color.Cyan;
                tile.LandedOnTiles.Add(0, new RevealSecretEvent());
                tile.LandedOnTiles.Add(0, new AskLeaderEvent());
                tile.InteractWithTiles.Add(0, new NextFloorEvent());
                tile.InteractWithTiles.Add(0, new SwitchMapEvent());
            }
            else if (ii == 48)
            {
                tile.Name = new LocalText("Script Trigger");
                fileName = "area_script";
                tile.Desc = new LocalText("");
                tile.StepType = TileData.TriggerType.None;
                tile.Anim = new ObjAnimData();
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());
                tile.InteractWithTiles.Add(0, new SingleCharStateScriptEvent());
            }
            else if (ii == 49)
            {
                tile.Name = new LocalText("Stairs");
                fileName = "stairs_back_down";
                tile.Desc = new LocalText("Stairs leading to the previous floor. If you are on the first floor, you will escape from the dungeon.");
                tile.Comment = "Back Up";
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Passage;
                tile.Anim = new ObjAnimData("Stairs_Up", 1);
                tile.MinimapIcon = new Loc(4, 0);
                tile.MinimapColor = Color.Cyan;
                tile.LandedOnTiles.Add(0, new RevealSecretEvent());
                tile.LandedOnTiles.Add(0, new AskLeaderEvent());
                tile.InteractWithTiles.Add(0, new NextFloorEvent());
                tile.InteractWithTiles.Add(0, new SwitchMapEvent());
            }
            else if (ii == 50)
            {
                tile.Name = new LocalText("Stairs");
                fileName = "stairs_back_up";
                tile.Desc = new LocalText("Stairs leading to the previous floor. If you are on the first floor, you will escape from the dungeon.");
                tile.Comment = "Back Down";
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Passage;
                tile.Anim = new ObjAnimData("Stairs_Down", 1);
                tile.MinimapIcon = new Loc(4, 0);
                tile.MinimapColor = Color.Cyan;
                tile.LandedOnTiles.Add(0, new RevealSecretEvent());
                tile.LandedOnTiles.Add(0, new AskLeaderEvent());
                tile.InteractWithTiles.Add(0, new NextFloorEvent());
                tile.InteractWithTiles.Add(0, new SwitchMapEvent());
            }
            else if (ii == 51)
            {
                tile.Name = new LocalText("Compass Tile");
                fileName = "tile_compass";
                tile.Desc = new LocalText("A tile that shows the way to important locations on the floor.");
                tile.BlockItem = true;
                tile.StepType = TileData.TriggerType.Switch;
                tile.Anim = new ObjAnimData("Tile_Compass", 1);
                tile.MinimapIcon = new Loc(2, 1);
                tile.MinimapColor = Color.Cyan;
                tile.LandedOnTiles.Add(0, new TriggerUnderfootEvent());
                SingleEmitter emitter = new SingleEmitter(new AnimData("Stair_Sensor_Arrow", 6), 6);
                emitter.Layer = DrawLayer.Top;
                tile.InteractWithTiles.Add(0, new CompassEvent(emitter, "stairs_go_up", "stairs_go_down", "stairs_secret_up", "stairs_secret_down", "rescue_point",
                    "chest_treasure", "tile_boss", "sealed_door", "stairs_exit_up", "stairs_exit_down", "stairs_back_up", "stairs_back_down"));
            }

            if (tile.Name.DefaultText.StartsWith("**"))
                tile.Name.DefaultText = tile.Name.DefaultText.Replace("*", "");
            else if (tile.Name.DefaultText != "")
                tile.Released = true;
            if (fileName == "")
                fileName = Text.Sanitize(tile.Name.DefaultText).ToLower();

            return (fileName, tile);
        }

        public const int MAX_TERRAIN = 8;

        public static void AddTerrainData()
        {
            DataInfo.DeleteIndexedData(DataManager.DataType.Terrain.ToString());
            for (int ii = 0; ii < MAX_TERRAIN; ii++)
            {
                (string, TerrainData) entry = GetTerrainData(ii);
                DataManager.SaveData(entry.Item1, DataManager.DataType.Terrain.ToString(), entry.Item2);
            }
        }

        //TODO: we need one class specifically for ground, and one class specifically for wall
        //Also make the ground and wall instance classes hold the textures, and make those modifiable on runtime (for generation)
        public static (string, TerrainData) GetTerrainData(int ii)
        {
            string asset = "";
            TerrainData tile = new TerrainData();
            if (ii == 0)
            {
                tile.Name = new LocalText("Variable-Texture Normal");
                asset = "floor";
            }
            else if (ii == 1)
            {
                tile.Name = new LocalText("Variable-Texture Impassable");
                asset = "unbreakable";
                tile.BlockType = TerrainData.Mobility.Impassable;
                tile.MinimapColor = Color.White;
                tile.BlockDiagonal = true;
                tile.BlockLight = true;
            }
            else if (ii == 2)
            {
                tile.Name = new LocalText("Variable-Texture Blocked");
                asset = "wall";
                tile.BlockType = TerrainData.Mobility.Block;
                tile.MinimapColor = Color.White;
                tile.BlockDiagonal = true;
                tile.BlockLight = true;
                tile.LandedOnTiles.Add(0, new TeamHungerEvent(800));
            }
            else if (ii == 3)
            {
                tile.Name = new LocalText("Variable-Texture Water");
                asset = "water";
                tile.BlockType = TerrainData.Mobility.Water;
                tile.MinimapColor = Color.Blue;
                tile.ShadowType = 3;
                tile.LandedOnTiles.Add(0, new RemoveStatusEvent("burn"));
            }
            else if (ii == 4)
            {
                tile.Name = new LocalText("Variable-Texture Lava");
                asset = "lava";
                tile.BlockType = TerrainData.Mobility.Lava;
                tile.MinimapColor = Color.DarkOrange;
                tile.ShadowType = 4;
                SingleEmitter endAnim = new SingleEmitter(new AnimData("Burned", 3));
                endAnim.LocHeight = 18;
                tile.LandedOnTiles.Add(0, new SingleExceptEvent(typeof(LavaState), new GiveStatusEvent("burn", new StateCollection<StatusState>(), true, new StringKey(), "DUN_Flamethrower_3", endAnim)));
            }
            else if (ii == 5)
            {
                tile.Name = new LocalText("Variable-Texture Abyss");
                asset = "pit";
                tile.BlockType = TerrainData.Mobility.Abyss;
                tile.MinimapColor = Color.Gray;
                tile.ShadowType = 4;
            }
            else if (ii == 6)
            {
                tile.Name = new LocalText("Variable-Texture Poison");
                asset = "water_poison";
                tile.BlockType = TerrainData.Mobility.Water;
                tile.MinimapColor = Color.Blue;
                tile.ShadowType = 3;
                SqueezedAreaEmitter emitter = new SqueezedAreaEmitter(new AnimData("Bubbles_Purple", 3));
                emitter.BurstTime = 3;
                emitter.Bursts = 4;
                emitter.ParticlesPerBurst = 1;
                emitter.Range = 12;
                emitter.StartHeight = -4;
                emitter.HeightSpeed = 12;
                emitter.SpeedDiff = 4;
                tile.LandedOnTiles.Add(0, new SingleExceptEvent(typeof(PoisonState), new GiveStatusEvent("poison", new StateCollection<StatusState>(), true, new StringKey(), "DUN_Toxic", emitter)));
            }
            else if (ii == 7)
            {
                tile.Name = new LocalText("Variable-Texture Foliage");
                asset = "grass";
                tile.MinimapColor = Color.Teal;
                tile.BlockLight = true;
            }

            return (asset, tile);
        }


        
    }
}

