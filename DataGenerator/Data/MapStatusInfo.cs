using System.Collections.Generic;
using RogueEssence.Dungeon;
using RogueEssence;
using RogueEssence.Data;
using RogueEssence.Content;
using PMDC.Dungeon;
using PMDC;
using PMDC.Data;
using Microsoft.Xna.Framework;

namespace DataGenerator.Data
{
    public static class MapStatusInfo
    {
        public const int MAX_MAP_STATUSES = 50;

        public static void AddMapStatusData()
        {
            DataInfo.DeleteIndexedData(DataManager.DataType.MapStatus.ToString());
            for (int ii = 0; ii < MAX_MAP_STATUSES; ii++)
            {
                MapStatusData status = GetMapStatusData(ii);
                if (status.Name.DefaultText != "")
                    DataManager.SaveData(Text.Sanitize(status.Name.DefaultText).ToLower(), DataManager.DataType.MapStatus.ToString(), status);
            }
        }
        public static MapStatusData GetMapStatusData(int ii)
        {
            MapStatusData status = new MapStatusData();
            if (ii == 0)
            {
                status.Name = new LocalText("Clear");
                status.Desc = new LocalText("The weather is clear.");
                status.DefaultHidden = true;
                status.RepeatMethod = new MapStatusRefreshEvent();
                status.StatusStates.Set(new MapWeatherState());
                status.OnMapStatusAdds.Add(-5, new MapStatusVisibleIfCountdownEvent());
                status.OnMapStatusAdds.Add(-5, new ReplaceStatusGroupEvent(typeof(MapWeatherState), true));
                status.StatusStates.Set(new MapCountDownState(50));
                status.StatusStates.Set(new MapTickState(0));
                status.OnMapTurnEnds.Add(5, new MapTickEvent());
                status.OnMapTurnEnds.Add(5, new MapStatusCountDownEvent());
            }
            else if (ii == 1)
            {
                status.Name = new LocalText("Rain");
                status.Desc = new LocalText("Water-type attacks are boosted and Fire-type attacks are weakened.");
                ScreenRainEmitter rain = new ScreenRainEmitter(new AnimData("Rain", 1, 0, 0));
                rain.HeightSpeed = -360;
                rain.SpeedDiff = 120;
                rain.BurstTime = 3;
                rain.ParticlesPerBurst = 2;
                rain.ResultAnim = new AnimData("Rain", 1, 1, -1);
                rain.Layer = DrawLayer.Top;
                status.Emitter = rain;
                status.RepeatMethod = new MapStatusRefreshEvent();
                status.StatusStates.Set(new MapWeatherState());
                status.OnMapStatusAdds.Add(0, new MapStatusSoundEvent("DUN_Rain_Dance"));
                status.OnMapStatusAdds.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_RAIN_START"), true));
                status.OnMapStatusAdds.Add(-5, new ReplaceStatusGroupEvent(typeof(MapWeatherState)));
                status.OnMapStatusRemoves.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_RAIN_END"), true));
                status.OnActions.Add(0, new MultiplyElementEvent("water", 3, 2, false));
                status.OnActions.Add(0, new MultiplyElementEvent("fire", 1, 2, false));
                status.StatusStates.Set(new MapCountDownState(50));
                status.StatusStates.Set(new MapTickState(0));
                status.OnMapTurnEnds.Add(5, new MapTickEvent());
                status.OnMapTurnEnds.Add(5, new MapStatusCountDownEvent());
            }
            else if (ii == 2)
            {
                status.Name = new LocalText("Sunny");
                status.Desc = new LocalText("Fire-type attacks are boosted and Water-type attacks are weakened.");
                OverlayEmitter overlay = new OverlayEmitter();
                overlay.Anim = new BGAnimData("White", 3);
                overlay.Color = Color.LightYellow * 0.2f;
                overlay.Layer = DrawLayer.Top;
                status.Emitter = overlay;
                status.RepeatMethod = new MapStatusRefreshEvent();
                status.StatusStates.Set(new MapWeatherState());
                status.OnMapStatusAdds.Add(0, new MapStatusSoundEvent("DUN_Sunny_Day"));
                status.OnMapStatusAdds.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_SUN_START"), true));
                status.OnMapStatusAdds.Add(-5, new ReplaceStatusGroupEvent(typeof(MapWeatherState)));
                status.OnMapStatusRemoves.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_SUN_END"), true));
                status.OnActions.Add(0, new MultiplyElementEvent("fire", 3, 2, false));
                status.OnActions.Add(0, new MultiplyElementEvent("water", 1, 2, false));
                status.OnTurnStarts.Add(0, new RemoveStatusEvent("freeze"));//insta-freeze thaw
                status.StatusStates.Set(new MapCountDownState(50));
                status.StatusStates.Set(new MapTickState(0));
                status.OnMapTurnEnds.Add(5, new MapTickEvent());
                status.OnMapTurnEnds.Add(5, new MapStatusCountDownEvent());
            }
            else if (ii == 3)
            {
                status.Name = new LocalText("Sandstorm");
                status.Desc = new LocalText("All Pokémon on the floor will take damage except Rock, Ground, and Steel-types. The Sp. Def of Rock-types is boosted.");
                OverlayEmitter overlay = new OverlayEmitter();
                overlay.Anim = new BGAnimData("Sandstorm", 3, 0, 0, 96);
                overlay.Layer = DrawLayer.Top;
                overlay.Movement = new RogueElements.Loc(240, 0);
                status.Emitter = overlay;
                status.RepeatMethod = new MapStatusRefreshEvent();
                status.StatusStates.Set(new MapWeatherState());
                status.OnMapStatusAdds.Add(0, new MapStatusSoundEvent("DUN_Sandstorm"));
                status.OnMapStatusAdds.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_SAND_START"), true));
                status.OnMapStatusAdds.Add(-5, new ReplaceStatusGroupEvent(typeof(MapWeatherState)));
                status.OnMapStatusRemoves.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_SAND_END"), true));
                status.BeforeBeingHits.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Magical, 2, 3));
                status.OnMapTurnEnds.Add(0, new WeatherDamageEvent(new System.Type[] { typeof(MagicGuardState), typeof(SandState) }, "rock", "ground", "steel"));
                status.StatusStates.Set(new MapCountDownState(50));
                status.StatusStates.Set(new MapTickState(0));
                status.OnMapTurnEnds.Add(5, new MapTickEvent());
                status.OnMapTurnEnds.Add(5, new MapStatusCountDownEvent());
            }
            else if (ii == 4)
            {
                status.Name = new LocalText("Hail");
                status.Desc = new LocalText("All Pokémon on the floor will take damage except Ice-types.");
                ScreenRainEmitter rain = new ScreenRainEmitter(new AnimData("Hail", 1, 0, 1));
                rain.HeightSpeed = -240;
                rain.SpeedDiff = 120;
                rain.BurstTime = 5;
                rain.ParticlesPerBurst = 2;
                rain.ResultAnim = new AnimData("Hail", 1, 2, -1);
                rain.Layer = DrawLayer.Top;
                status.Emitter = rain;
                status.RepeatMethod = new MapStatusRefreshEvent();
                status.StatusStates.Set(new MapWeatherState());
                status.OnMapStatusAdds.Add(0, new MapStatusSoundEvent("DUN_Hail"));
                status.OnMapStatusAdds.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_HAIL_START"), true));
                status.OnMapStatusAdds.Add(-5, new ReplaceStatusGroupEvent(typeof(MapWeatherState)));
                status.OnMapStatusRemoves.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_HAIL_END"), true));
                status.OnMapTurnEnds.Add(0, new WeatherDamageEvent(new System.Type[] { typeof(MagicGuardState), typeof(HailState) }, "ice"));
                status.StatusStates.Set(new MapCountDownState(50));
                status.StatusStates.Set(new MapTickState(0));
                status.OnMapTurnEnds.Add(5, new MapTickEvent());
                status.OnMapTurnEnds.Add(5, new MapStatusCountDownEvent());
            }
            else if (ii == 5)
            {
                status.Name = new LocalText("**Fog");
                OverlayEmitter overlay = new OverlayEmitter();
                overlay.Anim = new BGAnimData("White", 3);
                overlay.Color = Color.Gray * 0.4f;
                overlay.Layer = DrawLayer.Top;
                status.Emitter = overlay;
                status.RepeatMethod = new MapStatusRefreshEvent();
                status.StatusStates.Set(new MapWeatherState());
                status.OnMapStatusAdds.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_FOG_START"), true));
                status.OnMapStatusAdds.Add(-5, new ReplaceStatusGroupEvent(typeof(MapWeatherState)));
                status.OnMapStatusRemoves.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_FOG_END"), true));
                status.StatusStates.Set(new MapCountDownState(50));
                status.OnMapTurnEnds.Add(5, new MapStatusCountDownEvent());
            }
            else if (ii == 6)
            {
                status.Name = new LocalText("Cloudy");
                OverlayEmitter overlay = new OverlayEmitter();
                overlay.Anim = new BGAnimData("Clouds_Overhead", 1);
                overlay.Color = Color.White * 0.234f;
                overlay.Movement = new RogueElements.Loc(-20, 20);
                overlay.Layer = DrawLayer.Top;
                status.Emitter = overlay;
                status.RepeatMethod = new MapStatusRefreshEvent();
            }
            else if (ii == 7)
            {
                status.Name = new LocalText("Snow");
                OverlayEmitter overlay = new OverlayEmitter();
                overlay.Anim = new BGAnimData("Snow", 4);
                overlay.Layer = DrawLayer.Top;
                status.Emitter = overlay;
                status.RepeatMethod = new MapStatusRefreshEvent();
            }
            else if (ii == 8)
            {
                status.Name = new LocalText("Blizzard");
                OverlayEmitter overlay = new OverlayEmitter();
                overlay.Anim = new BGAnimData("Blizzard", 3);
                overlay.Layer = DrawLayer.Top;
                overlay.Movement = new RogueElements.Loc(-240, 180);
                status.Emitter = overlay;
                status.RepeatMethod = new MapStatusRefreshEvent();
            }
            else if (ii == 9)
            {
                status.Name = new LocalText("Trick Room");
                status.Desc = new LocalText("Effects on all Pokémon's Movement Speed are reversed.");
                OverlayEmitter overlay = new OverlayEmitter();
                overlay.Anim = new BGAnimData("Trick_Room", 3, 0, 0, 64);
                overlay.Layer = DrawLayer.Top;
                overlay.Movement = new RogueElements.Loc(-60, -60);
                status.Emitter = overlay;
                status.RepeatMethod = new MapStatusToggleEvent();
                status.OnMapStatusAdds.Add(0, new MapStatusSoundEvent("DUN_Focus_Blast"));
                status.OnMapStatusAdds.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_TRICK_ROOM_START"), true));
                status.OnMapStatusRemoves.Add(0, new MapStatusSoundEvent("DUN_Focus_Blast"));
                status.OnMapStatusRemoves.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_TRICK_ROOM_END"), true));
                status.OnRefresh.Add(5, new SpeedReverseEvent());
                status.StatusStates.Set(new MapCountDownState(50));
                status.StatusStates.Set(new MapTickState(0));
                status.OnMapTurnEnds.Add(5, new MapTickEvent());
                status.OnMapTurnEnds.Add(5, new MapStatusCountDownEvent());
            }
            else if (ii == 10)
            {
                status.Name = new LocalText("Gravity");
                status.Desc = new LocalText("All Pokémon on the floor become less evasive and are forced to drop to the ground.");
                OverlayEmitter overlay = new OverlayEmitter();
                overlay.Anim = new BGAnimData("White", 3);
                overlay.Color = Color.Black * 0.2f;
                overlay.Layer = DrawLayer.Top;
                status.Emitter = overlay;
                status.RepeatMethod = new MapStatusRefreshEvent();
                status.OnMapStatusAdds.Add(0, new MapStatusSoundEvent("DUN_Cloudy"));
                status.OnMapStatusAdds.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_GRAVITY_START"), true));
                //remove statuses when gravity intensifies
                status.OnMapStatusAdds.Add(0, new MapStatusCharEvent(new RemoveStatusEvent("flying")));
                status.OnMapStatusAdds.Add(0, new MapStatusCharEvent(new RemoveStatusEvent("bouncing")));
                status.OnMapStatusAdds.Add(0, new MapStatusCharEvent(new RemoveStatusEvent("telekinesis")));
                status.OnMapStatusAdds.Add(0, new MapStatusCharEvent(new RemoveStatusEvent("magnet_rise")));
                status.OnMapStatusRemoves.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_GRAVITY_END"), true));
                status.TargetElementEffects.Add(1, new TypeVulnerableEvent("ground"));
                //prevent telekinesis, magnet rise, bounce and fly status effects
                status.BeforeStatusAdds.Add(0, new PreventStatusCheck("flying", new StringKey("MSG_GRAVITY_NO_FLY")));
                status.BeforeStatusAdds.Add(0, new PreventStatusCheck("bouncing", new StringKey("MSG_GRAVITY_NO_BOUNCE")));
                status.BeforeStatusAdds.Add(0, new PreventStatusCheck("telekinesis", new StringKey("MSG_GRAVITY_NO_LEVITATE")));
                status.BeforeStatusAdds.Add(0, new PreventStatusCheck("magnet_rise", new StringKey("MSG_GRAVITY_NO_LEVITATE")));
                //prevent the specific moves banned by gravity... or not
                //drop everyone's evasion by 2 stages
                status.BeforeBeingHits.Add(-5, new TargetStatAddEvent(Stat.DodgeRate, -2));
                status.StatusStates.Set(new MapCountDownState(50));
                status.StatusStates.Set(new MapTickState(0));
                status.OnMapTurnEnds.Add(5, new MapTickEvent());
                status.OnMapTurnEnds.Add(5, new MapStatusCountDownEvent());
            }
            else if (ii == 11)
            {
                status.Name = new LocalText("Wonder Room");
                status.Desc = new LocalText("Physical attacks become Special attacks, and vice versa.");
                OverlayEmitter overlay = new OverlayEmitter();
                overlay.Anim = new BGAnimData("Wonder_Room", 3, 0, 0, 64);
                overlay.Layer = DrawLayer.Top;
                overlay.Movement = new RogueElements.Loc(60, 60);
                status.Emitter = overlay;
                status.RepeatMethod = new MapStatusToggleEvent();
                status.OnMapStatusAdds.Add(0, new MapStatusSoundEvent("DUN_Focus_Blast"));
                status.OnMapStatusAdds.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_WONDER_ROOM_START"), true));
                status.OnMapStatusRemoves.Add(0, new MapStatusSoundEvent("DUN_Focus_Blast"));
                status.OnMapStatusRemoves.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_WONDER_ROOM_END"), true));
                status.BeforeActions.Add(-5, new FlipCategoryEvent(false));
                status.StatusStates.Set(new MapCountDownState(50));
                status.StatusStates.Set(new MapTickState(0));
                status.OnMapTurnEnds.Add(5, new MapTickEvent());
                status.OnMapTurnEnds.Add(5, new MapStatusCountDownEvent());
            }
            else if (ii == 12)
            {
                status.Name = new LocalText("Magic Room");
                status.Desc = new LocalText("All Pokémon on the floor have their held items and abilities disabled.");
                OverlayEmitter overlay = new OverlayEmitter();
                overlay.Anim = new BGAnimData("Magic_Room", 3, 0, 0, 64);
                overlay.Layer = DrawLayer.Top;
                overlay.Movement = new RogueElements.Loc(60, -60);
                status.Emitter = overlay;
                status.RepeatMethod = new MapStatusToggleEvent();
                status.OnMapStatusAdds.Add(0, new MapStatusSoundEvent("DUN_Focus_Blast"));
                status.OnMapStatusAdds.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_MAGIC_ROOM_START"), true));
                status.OnMapStatusRemoves.Add(0, new MapStatusSoundEvent("DUN_Focus_Blast"));
                status.OnMapStatusRemoves.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_MAGIC_ROOM_END"), true));
                status.OnRefresh.Add(-5, new NoHeldItemEvent());
                status.OnRefresh.Add(-5, new NoAbilityEvent());
                status.StatusStates.Set(new MapCountDownState(50));
                status.StatusStates.Set(new MapTickState(0));
                status.OnMapTurnEnds.Add(5, new MapTickEvent());
                status.OnMapTurnEnds.Add(5, new MapStatusCountDownEvent());
            }
            else if (ii == 13)
            {
                status.Name = new LocalText("Electric Terrain");
                status.Desc = new LocalText("Electric-type attacks are boosted, and all Pokémon on the floor cannot sleep.");
                OverlayEmitter overlay = new OverlayEmitter();
                overlay.Anim = new BGAnimData("White", 3);
                overlay.Color = Color.Yellow * 0.3f;
                overlay.Layer = DrawLayer.Top;
                status.Emitter = overlay;
                status.RepeatMethod = new MapStatusRefreshEvent();
                status.StatusStates.Set(new MapWeatherState());
                status.OnMapStatusAdds.Add(0, new MapStatusSoundEvent("DUN_Discharge"));
                status.OnMapStatusAdds.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_ELECTRIC_TERRAIN_START"), true));
                status.OnMapStatusAdds.Add(-5, new ReplaceStatusGroupEvent(typeof(MapWeatherState)));
                //remove statuses when electric terrain occurs
                status.OnMapStatusAdds.Add(0, new MapStatusCharEvent(new RemoveStatusEvent("sleep")));
                status.OnMapStatusAdds.Add(0, new MapStatusCharEvent(new RemoveStatusEvent("yawning")));
                status.OnMapStatusRemoves.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_ELECTRIC_TERRAIN_END"), true));
                status.BeforeStatusAdds.Add(0, new PreventStatusCheck("sleep", new StringKey("MSG_ELECTRIC_TERRAIN_NO_SLEEP")));
                status.BeforeStatusAdds.Add(0, new PreventStatusCheck("yawning", new StringKey("MSG_ELECTRIC_TERRAIN_NO_DROWSY")));
                status.OnActions.Add(0, new MultiplyElementEvent("electric", 3, 2, false));
                status.StatusStates.Set(new MapCountDownState(50));
                status.StatusStates.Set(new MapTickState(0));
                status.OnMapTurnEnds.Add(5, new MapTickEvent());
                status.OnMapTurnEnds.Add(5, new MapStatusCountDownEvent());
            }
            else if (ii == 14)
            {
                status.Name = new LocalText("Grassy Terrain");
                status.Desc = new LocalText("Grass-type attacks are boosted, Ground-type attacks are weakened, and all Pokémon on the floor slowly gain HP except Flying types.");
                OverlayEmitter overlay = new OverlayEmitter();
                overlay.Anim = new BGAnimData("White", 3);
                overlay.Color = Color.LightGreen * 0.3f;
                overlay.Layer = DrawLayer.Top;
                status.Emitter = overlay;
                status.RepeatMethod = new MapStatusRefreshEvent();
                status.StatusStates.Set(new MapWeatherState());
                status.OnMapStatusAdds.Add(0, new MapStatusSoundEvent("DUN_Natural_Gift"));
                status.OnMapStatusAdds.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_GRASSY_TERRAIN_START"), true));
                status.OnMapStatusAdds.Add(-5, new ReplaceStatusGroupEvent(typeof(MapWeatherState)));
                status.OnMapStatusRemoves.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_GRASSY_TERRAIN_END"), true));
                status.OnActions.Add(0, new MultiplyElementEvent("grass", 3, 2, false));
                status.OnActions.Add(0, new MultiplyElementEvent("ground", 1, 2, false));
                status.OnMapTurnEnds.Add(0, new WeatherHealEvent("flying"));
                status.StatusStates.Set(new MapCountDownState(50));
                status.StatusStates.Set(new MapTickState(0));
                status.OnMapTurnEnds.Add(5, new MapTickEvent());
                status.OnMapTurnEnds.Add(5, new MapStatusCountDownEvent());
            }
            else if (ii == 15)
            {
                status.Name = new LocalText("Misty Terrain");
                status.Desc = new LocalText("Dragon-type attacks are weakened, and all Pokémon are protected from major status problems.");
                OverlayEmitter overlay = new OverlayEmitter();
                overlay.Anim = new BGAnimData("White", 3);
                overlay.Color = Color.LightPink * 0.3f;
                overlay.Layer = DrawLayer.Top;
                status.Emitter = overlay;
                status.RepeatMethod = new MapStatusRefreshEvent();
                status.StatusStates.Set(new MapWeatherState());
                status.OnMapStatusAdds.Add(0, new MapStatusSoundEvent("DUN_Foggy"));
                status.OnMapStatusAdds.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_MISTY_TERRAIN_START"), true));
                status.OnMapStatusAdds.Add(-5, new ReplaceStatusGroupEvent(typeof(MapWeatherState)));
                status.OnMapStatusRemoves.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_MISTY_TERRAIN_END"), true));
                status.BeforeStatusAdds.Add(0, new StateStatusCheck(typeof(MajorStatusState), new StringKey("MSG_MISTY_TERRAIN_NO_STATUS")));
                status.OnActions.Add(0, new MultiplyElementEvent("dragon", 1, 2, false));
                status.StatusStates.Set(new MapCountDownState(50));
                status.StatusStates.Set(new MapTickState(0));
                status.OnMapTurnEnds.Add(5, new MapTickEvent());
                status.OnMapTurnEnds.Add(5, new MapStatusCountDownEvent());
            }
            else if (ii == 16)
            {
                status.Name = new LocalText("Inverse");
                status.Desc = new LocalText("All type matchups are reversed.");
                OverlayEmitter overlay = new OverlayEmitter();
                overlay.Anim = new BGAnimData("Inverse", 3);
                overlay.Color = Color.White * 0.3f;
                overlay.Layer = DrawLayer.Top;
                status.Emitter = overlay;
                status.RepeatMethod = new MapStatusToggleEvent();
                status.OnMapStatusAdds.Add(0, new MapStatusSoundEvent("DUN_Cloudy"));
                status.OnMapStatusAdds.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_INVERSE_START"), true));
                status.OnMapStatusRemoves.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_INVERSE_END"), true));
                status.TargetElementEffects.Add(2, new InverseEvent());
                status.StatusStates.Set(new MapCountDownState(50));
                status.StatusStates.Set(new MapTickState(0));
                status.OnMapTurnEnds.Add(5, new MapTickEvent());
                status.OnMapTurnEnds.Add(5, new MapStatusCountDownEvent());
            }
            else if (ii == 17)
            {
                status.Name = new LocalText("**Heavy Rain");
                status.RepeatMethod = new MapStatusRefreshEvent();
                status.StatusStates.Set(new MapWeatherState());
                //status.OnMapStatusAdds.Add(0, new MapStatusBattleLogEffect(, true));
                status.OnMapStatusAdds.Add(-5, new ReplaceStatusGroupEvent(typeof(MapWeatherState)));
                //status.OnMapStatusRemoves.Add(0, new MapStatusBattleLogEffect(, true));
                status.StatusStates.Set(new MapTickState(0));
                status.OnMapTurnEnds.Add(5, new MapTickEvent());
            }
            else if (ii == 18)
            {
                status.Name = new LocalText("**Harsh Sun");
                status.RepeatMethod = new MapStatusRefreshEvent();
                status.StatusStates.Set(new MapWeatherState());
                //status.OnMapStatusAdds.Add(0, new MapStatusBattleLogEffect(, true));
                status.OnMapStatusAdds.Add(-5, new ReplaceStatusGroupEvent(typeof(MapWeatherState)));
                //status.OnMapStatusRemoves.Add(0, new MapStatusBattleLogEffect(, true));
                status.StatusStates.Set(new MapTickState(0));
                status.OnMapTurnEnds.Add(5, new MapTickEvent());
            }
            else if (ii == 19)
            {
                status.Name = new LocalText("Wind");
                status.Desc = new LocalText("Flying-type Pokémon are protected from their weaknesses.");
                OverlayEmitter overlay = new OverlayEmitter();
                overlay.Anim = new BGAnimData("White", 3);
                overlay.Color = Color.LightGray * 0.3f;
                overlay.Layer = DrawLayer.Top;
                status.Emitter = overlay;
                status.RepeatMethod = new MapStatusRefreshEvent();
                status.StatusStates.Set(new MapWeatherState());
                status.OnMapStatusAdds.Add(0, new MapStatusSoundEvent("DUN_Cloudy"));
                status.OnMapStatusAdds.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_WIND_START"), true));
                status.OnMapStatusAdds.Add(-5, new ReplaceStatusGroupEvent(typeof(MapWeatherState)));
                status.OnMapStatusRemoves.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_WIND_END"), true));
                status.TargetElementEffects.Add(0, new RemoveTypeWeaknessEvent("flying"));
                status.StatusStates.Set(new MapCountDownState(50));
                status.StatusStates.Set(new MapTickState(0));
                status.OnMapTurnEnds.Add(5, new MapTickEvent());
                status.OnMapTurnEnds.Add(5, new MapStatusCountDownEvent());
            }
            else if (ii == 20)
            {
                status.Name = new LocalText("Move Ban");
                status.Desc = new LocalText("A move has been banned and can no longer be used.");
                status.CarryOver = true;
                status.RepeatMethod = new MapStatusReplaceEvent();
                status.OnMapStatusAdds.Add(0, new MapStatusSoundEvent("DUN_Invisible_2"));
                status.OnMapStatusAdds.Add(0, new MapStatusMoveLogEvent(new StringKey("MSG_MOVE_BAN_START")));
                status.OnMapStatusRemoves.Add(0, new MapStatusMoveLogEvent(new StringKey("MSG_MOVE_BAN_END")));
                status.StatusStates.Set(new MapIndexState());
                status.OnRefresh.Add(0, new MoveBanEvent());
            }
            else if (ii == 21)
            {
                status.Name = new LocalText("Mysterious Force");
                status.Desc = new LocalText("A mysterious force prevents Orbs, Machines, and special items from working.");
                status.RepeatMethod = new MapStatusReplaceEvent();
                status.BeforeTryActions.Add(0, new PreventItemUseEvent(new StringKey("MSG_ITEM_BAN"), new FlagType(typeof(UtilityState)), new FlagType(typeof(OrbState)), new FlagType(typeof(MachineState))));
                status.BeforeActions.Add(0, new PreventItemUseEvent(new StringKey("MSG_ITEM_BAN"), new FlagType(typeof(UtilityState)), new FlagType(typeof(OrbState)), new FlagType(typeof(MachineState))));
            }
            else if (ii == 22)
            {
                status.Name = new LocalText("Something's stirring...");
                status.Desc = new LocalText("Something's approaching. The exploration team must leave the floor before it arrives, or they will be forced out of the dungeon.");
                status.DefaultHidden = true;
                status.RepeatMethod = new MapStatusReplaceEvent();
                status.StatusStates.Set(new MapCountDownState(1000));
                WindEmitter overlay = new WindEmitter(new AnimData("Wind_Leaves", 4), new AnimData("Wind_Leaves_Small", 3));
                overlay.Bursts = 4;
                overlay.BurstTime = 20;
                overlay.ParticlesPerBurst = 4;
                overlay.Speed = -420;
                overlay.SpeedDiff = 300;
                overlay.StartDistance = 32;
                overlay.Layer = DrawLayer.Front;
                TimeLimitEvent timeLimit = new TimeLimitEvent();
                timeLimit.Emitter = overlay;
                timeLimit.Warning1 = new StringKey("MSG_TIME_WARNING_1");
                timeLimit.Warning2 = new StringKey("MSG_TIME_WARNING_2");
                timeLimit.Warning3 = new StringKey("MSG_TIME_WARNING_3");
                timeLimit.TimeOut = new StringKey("MSG_TIME_UP");
                timeLimit.WarningSE1 = "DUN_Wind";
                timeLimit.WarningSE2 = "DUN_Wind";
                timeLimit.WarningSE3 = "DUN_Wind";
                timeLimit.TimeOutSE = "DUN_Wind_2";
                timeLimit.BGM = "C04. Wind.ogg";
                status.OnMapTurnEnds.Add(7, timeLimit);
            }
            else if (ii == 23)
            {
                status.Name = new LocalText("Items Sniffed");
                status.Desc = new LocalText("");
                status.DefaultHidden = true;
            }
            else if (ii == 24)
            {
                status.Name = new LocalText("Default Weather");
                status.Desc = new LocalText("");
                status.DefaultHidden = true;
                status.RepeatMethod = new MapStatusReplaceEvent();
                status.StatusStates.Set(new MapIDState());
                status.OnMapStarts.Add(0, new WeatherFillEvent());
                status.OnMapTurnEnds.Add(6, new WeatherFillEvent());
            }
            else if (ii == 25)
            {
                status.Name = new LocalText("Default MapStatus");
                status.Desc = new LocalText("");
                status.DefaultHidden = true;
                status.RepeatMethod = new MapStatusReplaceEvent();
                status.StatusStates.Set(new MapIDState());
                status.OnMapStarts.Add(0, new MapStatusFillEvent());
                status.OnMapTurnEnds.Add(6, new MapStatusFillEvent());
            }
            else if (ii == 27)
            {
                status.Name = new LocalText("Mercy Revive");
                status.Desc = new LocalText("");
                status.DefaultHidden = true;
                status.RepeatMethod = new MapStatusReplaceEvent();
                status.OnDeaths.Add(6, new MercyReviveEvent(true, true, false));
            }
            else if (ii == 28)
            {
                status.Name = new LocalText("Stairs Sensed");
                status.Desc = new LocalText("");
                status.DefaultHidden = true;
            }
            else if (ii == 29)
            {
                status.Name = new LocalText("Map Surveyed");
                status.Desc = new LocalText("");
                status.DefaultHidden = true;
            }
            else if (ii == 30)
            {
                status.Name = new LocalText("Monster Mansion");
                status.Desc = new LocalText("");
                status.OnRefresh.Add(0, new SetSightEvent(true, Map.SightRange.Clear));
                status.OnRefresh.Add(0, new SeeCharsEvent());
                status.DefaultHidden = true;
            }
            else if (ii == 31)
            {
                status.Name = new LocalText("Thief");
                status.Desc = new LocalText("");
                status.DefaultHidden = true;
                status.RepeatMethod = new MapStatusIgnoreEvent();
                status.StatusStates.Set(new ShopSecurityState());
                status.StatusStates.Set(new MapTickState(0));
                //prevent escape orbs
                status.BeforeTryActions.Add(0, new PreventItemIndexEvent(new StringKey("MSG_ITEM_BAN"), 250));
                //prevent leaderswitch
                status.OnMapRefresh.Add(0, new MapNoSwitchEvent());
                //prevent rescue
                status.OnMapRefresh.Add(0, new MapNoRescueEvent());
                //constantly spawn a mon near the exit
                status.OnMapStatusAdds.Add(0, new MapStatusBGMEvent("C07. Outlaw.ogg"));
                status.OnMapStatusAdds.Add(0, new MapStatusScriptEvent("SetShopkeeperHostile"));
                status.OnMapStatusAdds.Add(0, new MapStatusSpawnStartGuardsEvent("shopkeeper"));
                status.OnMapTurnEnds.Add(10, new PeriodicSpawnEntranceGuards(10, 40, "shopkeeper"));
            }
            else if (ii == 32)
            {
                status.Name = new LocalText("Dark");
                OverlayEmitter overlay = new OverlayEmitter();
                overlay.Anim = new BGAnimData("White", 3);
                overlay.Color = Color.Black * 0.4f;
                overlay.Layer = DrawLayer.Top;
                status.Emitter = overlay;
                status.RepeatMethod = new MapStatusRefreshEvent();
            }
            else if (ii == 33)
            {
                status.Name = new LocalText("Intrusion Check");
                status.Desc = new LocalText("");
                status.DefaultHidden = true;
                status.RepeatMethod = new MapStatusCombineCheckEvent();
                status.StatusStates.Set(new MapCheckState());
                status.OnMapStarts.Add(-3, new CheckTriggersEvent());
                status.OnMapTurnEnds.Add(10, new CheckTriggersEvent());
            }
            else if (ii == 34)
            {
                status.Name = new LocalText("Map Clear Check");
                status.Desc = new LocalText("");
                status.DefaultHidden = true;
                status.RepeatMethod = new MapStatusCombineCheckEvent();
                status.StatusStates.Set(new MapCheckState());
                status.OnTurnEnds.Add(0, new CheckTriggersEvent());
                status.OnMapTurnEnds.Add(0, new NullCharEvent(new CheckTriggersEvent()));
            }
            else if (ii == 35)
            {
                status.Name = new LocalText("Outlaw Clear Check");
                status.Desc = new LocalText("");
                status.DefaultHidden = true;
                status.RepeatMethod = new MapStatusReplaceEvent();
                status.OnTurnEnds.Add(0, new SingleCharScriptEvent("OutlawClearCheck"));
                status.OnMapTurnEnds.Add(0, new NullCharEvent(new SingleCharScriptEvent("OutlawClearCheck")));
            }
            else if (ii == 37)
            {
                status.Name = new LocalText("Shopping");
                status.Desc = new LocalText("");
                status.DefaultHidden = true;
                status.RepeatMethod = new MapStatusIgnoreEvent();
                //prevent leaderswitch
                status.OnMapRefresh.Add(0, new MapNoSwitchEvent());
                status.OnMapStatusAdds.Add(0, new MapStatusBGMEvent("A11. Shop.ogg"));
                status.OnMapStatusAdds.Add(0, new MapStatusScriptEvent("ShopGreeting"));
                status.OnTurnEnds.Add(0, new EndShopEvent("area_shop"));
                status.OnTurnEnds.Add(0, new SingleCharScriptEvent("ShopCheckout"));
                status.OnMapTurnEnds.Add(0, new NullCharEvent(new EndShopEvent("area_shop")));
                status.OnMapTurnEnds.Add(0, new NullCharEvent(new SingleCharScriptEvent("ShopCheckout")));
            }
            else if (ii == 38)
            {
                status.Name = new LocalText("Shop Security");
                status.Desc = new LocalText("");
                status.DefaultHidden = true;
                status.RepeatMethod = new MapStatusIgnoreEvent();
                status.StatusStates.Set(new ShopPriceState());
                status.StatusStates.Set(new ShopSecurityState());
                status.StatusStates.Set(new MapIndexState());
                status.OnMapStarts.Add(-10, new InitShopPriceEvent("area_shop"));
                status.OnTurnEnds.Add(10, new SingleCharScriptEvent("ThiefCheck"));
                status.OnMapTurnEnds.Add(10, new NullCharEvent(new SingleCharScriptEvent("ThiefCheck")));
            }
            else if (ii == 39)
            {
                status.Name = new LocalText("Rescued");
                status.Desc = new LocalText("");
                status.DefaultHidden = true;
                status.RepeatMethod = new MapStatusIgnoreEvent();
                //prevent rescue
                status.OnMapRefresh.Add(0, new MapNoRescueEvent());
            }
            else if (ii == 40)
            {
                status.Name = new LocalText("Foe FOV");
                status.Desc = new LocalText("");
                status.DefaultHidden = true;
                status.OnRefresh.Add(-1, new FactionRefreshEvent(Faction.Foe, new SetSightEvent(true, Map.SightRange.Dark)));
            }
            else if (ii == 41)
            {
                status.Name = new LocalText("Scanner");
                status.Desc = new LocalText("");
                status.DefaultHidden = true;
                status.RepeatMethod = new MapStatusReplaceEvent();
                status.OnMapStatusAdds.Add(0, new MapStatusBattleLogEvent(new StringKey("MSG_FLOOR_SCAN"), true));
                status.OnRefresh.Add(0, new FactionRefreshEvent(Faction.Player, new SetSightEvent(true, Map.SightRange.Clear)));
                status.OnRefresh.Add(0, new FactionRefreshEvent(Faction.Player, new SeeCharsEvent()));
                status.OnRefresh.Add(0, new FactionRefreshEvent(Faction.Player, new SeeItemsEvent(false)));
            }


            if (status.Name.DefaultText.StartsWith("**"))
                status.Name.DefaultText = status.Name.DefaultText.Replace("*", "");
            else if (status.Name.DefaultText != "")
                status.Released = true;

            return status;
        }

    }
}

