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
using DataGenerator.Dev;

namespace DataGenerator.Data
{
    public partial class ZoneInfo
    {

        private static Dictionary<string, LocalText> specialRows;

        public static void InitStringsAll()
        {
            specialRows = Localization.readLocalizationRows(GenPath.TL_PATH + "Special.out.txt");
        }


        static IEnumerable<string> IterateGummis(bool wonder)
        {
            if (wonder)
                yield return "gummi_wonder";
            yield return "gummi_blue";
            yield return "gummi_black";
            yield return "gummi_clear";
            yield return "gummi_grass";
            yield return "gummi_green";
            yield return "gummi_brown";
            yield return "gummi_orange";
            yield return "gummi_gold";
            yield return "gummi_pink";
            yield return "gummi_purple";
            yield return "gummi_red";
            yield return "gummi_royal";
            yield return "gummi_silver";
            yield return "gummi_white";
            yield return "gummi_yellow";
            yield return "gummi_sky";
            yield return "gummi_gray";
            yield return "gummi_magenta";
        }

        static IEnumerable<string> IterateXItems()
        {
            yield return "medicine_x_attack";
            yield return "medicine_x_defense";
            yield return "medicine_x_sp_atk";
            yield return "medicine_x_sp_def";
            yield return "medicine_x_speed";
            yield return "medicine_x_accuracy";
            yield return "medicine_dire_hit";
        }

        static IEnumerable<string> IteratePinchBerries()
        {
            yield return "berry_apicot";
            yield return "berry_liechi";
            yield return "berry_ganlon";
            yield return "berry_salac";
            yield return "berry_petaya";
            yield return "berry_starf";
            yield return "berry_micle";
        }

        static IEnumerable<string> IterateVitamins()
        {
            yield return "boost_nectar";
            yield return "boost_hp_up";
            yield return "boost_protein";
            yield return "boost_iron";
            yield return "boost_calcium";
            yield return "boost_zinc";
            yield return "boost_carbos";
        }

        static IEnumerable<string> IterateTypeBerries()
        {
            yield return "berry_tanga";
            yield return "berry_colbur";
            yield return "berry_haban";
            yield return "berry_wacan";
            yield return "berry_chople";
            yield return "berry_occa";
            yield return "berry_coba";
            yield return "berry_kasib";
            yield return "berry_rindo";
            yield return "berry_shuca";
            yield return "berry_yache";
            yield return "berry_chilan";
            yield return "berry_kebia";
            yield return "berry_payapa";
            yield return "berry_charti";
            yield return "berry_babiri";
            yield return "berry_passho";
            yield return "berry_roseli";
        }

        static IEnumerable<string> IterateApricorns(bool plain)
        {
            if (plain)
                yield return "apricorn_plain";
            yield return "apricorn_blue";
            yield return "apricorn_green";
            yield return "apricorn_brown";
            yield return "apricorn_purple";
            yield return "apricorn_red";
            yield return "apricorn_white";
            yield return "apricorn_yellow";
            yield return "apricorn_black";
        }

        static IEnumerable<string> IterateSilks()
        {
            yield return "xcl_element_bug_silk";
            yield return "xcl_element_dark_silk";
            yield return "xcl_element_dragon_silk";
            yield return "xcl_element_electric_silk";
            yield return "xcl_element_fairy_silk";
            yield return "xcl_element_fighting_silk";
            yield return "xcl_element_fire_silk";
            yield return "xcl_element_flying_silk";
            yield return "xcl_element_ghost_silk";
            yield return "xcl_element_grass_silk";
            yield return "xcl_element_ground_silk";
            yield return "xcl_element_ice_silk";
            yield return "xcl_element_normal_silk";
            yield return "xcl_element_poison_silk";
            yield return "xcl_element_psychic_silk";
            yield return "xcl_element_rock_silk";
            yield return "xcl_element_steel_silk";
            yield return "xcl_element_water_silk";
        }

        static IEnumerable<string> IterateLegendaries()
        {
            yield return "articuno";
            yield return "zapdos";
            yield return "moltres";
            yield return "mewtwo";
            yield return "mew";
            yield return "lugia";
            yield return "ho_oh";
            yield return "celebi";
            yield return "regirock";
            yield return "regice";
            yield return "registeel";
            yield return "latias";
            yield return "latios";
            yield return "kyogre";
            yield return "groudon";
            yield return "rayquaza";
            yield return "jirachi";
            yield return "deoxys";
            yield return "uxie";
            yield return "mesprit";
            yield return "azelf";
            yield return "dialga";
            yield return "palkia";
            yield return "heatran";
            yield return "regigigas";
            yield return "giratina";
            yield return "cresselia";
            yield return "phione";
            yield return "manaphy";
            yield return "darkrai";
            yield return "shaymin";
            yield return "arceus";
            yield return "victini";
            yield return "cobalion";
            yield return "terrakion";
            yield return "virizion";
            yield return "tornadus";
            yield return "thundurus";
            yield return "reshiram";
            yield return "zekrom";
            yield return "landorus";
            yield return "kyurem";
            yield return "keldeo";
            yield return "meloetta";
            yield return "genesect";
            yield return "xerneas";
            yield return "yveltal";
            yield return "zygarde";
            yield return "diancie";
            yield return "hoopa";
            yield return "volcanion";
            yield return "tapu_koko";
            yield return "tapu_lele";
            yield return "tapu_bulu";
            yield return "tapu_fini";
            yield return "cosmog";
            yield return "cosmoem";
            yield return "solgaleo";
            yield return "lunala";
            yield return "nihilego";
            yield return "buzzwole";
            yield return "pheromosa";
            yield return "xurkitree";
            yield return "celesteela";
            yield return "kartana";
            yield return "guzzlord";
            yield return "necrozma";
            yield return "magearna";
            yield return "marshadow";
            yield return "poipole";
            yield return "naganadel";
            yield return "stakataka";
            yield return "blacephalon";
            yield return "zeraora";
            yield return "meltan";
            yield return "melmetal";
            yield return "zacian";
            yield return "zamazenta";
            yield return "eternatus";
            yield return "kubfu";
            yield return "urshifu";
            yield return "zarude";
            yield return "regieleki";
            yield return "regidrago";
            yield return "glastrier";
            yield return "spectrier";
            yield return "calyrex";
            yield return "enamorus";
            yield return "great_tusk";
            yield return "scream_tail";
            yield return "brute_bonnet";
            yield return "flutter_mane";
            yield return "slither_wing";
            yield return "sandy_shocks";
            yield return "iron_treads";
            yield return "iron_bundle";
            yield return "iron_hands";
            yield return "iron_jugulis";
            yield return "iron_moth";
            yield return "iron_thorns";
            yield return "wo_chien";
            yield return "chien_pao";
            yield return "ting_lu";
            yield return "chi_yu";
            yield return "roaring_moon";
            yield return "iron_valiant";
            yield return "koraidon";
            yield return "miraidon";
        }


        [Flags]
        public enum EvoClass
        {
            None = 0,
            Early = 1,
            Mid = 2,
            Late = 4,
            End = 8,
            All = 15
        }

        /// <summary>
        /// This does not include type boosting items that are also evo items
        /// </summary>
        /// <param name="evoClass"></param>
        /// <returns></returns>
        public static IEnumerable<string> IterateEvoItems(EvoClass evoClass)
        {
            if ((evoClass & EvoClass.End) != EvoClass.None)
            {
                yield return "evo_prism_scale";
                yield return "evo_reaper_cloth";
                yield return "evo_magmarizer";
                yield return "evo_electirizer";
                yield return "evo_protector";
            }

            if ((evoClass & EvoClass.Late) != EvoClass.None)
            {
                yield return "evo_link_cable";
                yield return "evo_dawn_stone";
                yield return "evo_dusk_stone";
                yield return "evo_shiny_stone";
            }

            if ((evoClass & EvoClass.Mid) != EvoClass.None)
            {
                yield return "evo_sun_stone";
                yield return "evo_moon_stone";
                yield return "evo_kings_rock";
                yield return "evo_sun_ribbon";
                yield return "evo_moon_ribbon";
            }

            if ((evoClass & EvoClass.Early) != EvoClass.None)
            {
                yield return "evo_thunder_stone";
                yield return "evo_fire_stone";
                yield return "evo_water_stone";
                yield return "evo_leaf_stone";
                yield return "evo_ice_stone";
            }
        }




        [Flags]
        public enum TMClass
        {
            None = 0,
            Top = 1,
            Mid = 2,
            Bottom = 4,
            Starter = 8,
            Natural = 15,
            ShopOnly = 16,
            All = 31
        }

        public static IEnumerable<string> IterateTMs(TMClass tmClass)
        {
            if ((tmClass & TMClass.Top) != TMClass.None)
            {
                yield return "tm_earthquake";
                yield return "tm_hyper_beam";
                yield return "tm_overheat";
                yield return "tm_blizzard";
                yield return "tm_swords_dance";
                yield return "tm_surf";
                yield return "tm_dark_pulse";
                yield return "tm_psychic";
                yield return "tm_thunder";
                yield return "tm_shadow_ball";
                yield return "tm_ice_beam";
                yield return "tm_giga_impact";
                yield return "tm_fire_blast";
                yield return "tm_dazzling_gleam";
                yield return "tm_flash_cannon";
                yield return "tm_stone_edge";
                yield return "tm_sludge_bomb";
                yield return "tm_focus_blast";
            }

            if ((tmClass & TMClass.Mid) != TMClass.None)
            {
                yield return "tm_explosion";
                yield return "tm_snatch";
                yield return "tm_sunny_day";
                yield return "tm_rain_dance";
                yield return "tm_sandstorm";
                yield return "tm_hail";
                yield return "tm_x_scissor";
                yield return "tm_wild_charge";
                yield return "tm_taunt";
                yield return "tm_focus_punch";
                yield return "tm_safeguard";
                yield return "tm_light_screen";
                yield return "tm_psyshock";
                yield return "tm_will_o_wisp";
                yield return "tm_dream_eater";
                yield return "tm_nature_power";
                yield return "tm_facade";
                yield return "tm_swagger";
                yield return "tm_captivate";
                yield return "tm_rock_slide";
                yield return "tm_fling";
                yield return "tm_thunderbolt";
                yield return "tm_water_pulse";
                yield return "tm_shock_wave";
                yield return "tm_brick_break";
                yield return "tm_payback";
                yield return "tm_calm_mind";
                yield return "tm_reflect";
                yield return "tm_charge_beam";
                yield return "tm_flamethrower";
                yield return "tm_energy_ball";
                yield return "tm_retaliate";
                yield return "tm_scald";
                yield return "tm_waterfall";
                yield return "tm_roost";
                yield return "tm_rock_polish";
                yield return "tm_acrobatics";
                yield return "tm_rock_climb";
                yield return "tm_bulk_up";
                yield return "tm_pluck";
                yield return "tm_psych_up";
                yield return "tm_secret_power";
                yield return "tm_natural_gift";
            }

            if ((tmClass & TMClass.Bottom) != TMClass.None)
            {
                yield return "tm_return";
                yield return "tm_frustration";
                yield return "tm_giga_drain";
                yield return "tm_dive";
                yield return "tm_poison_jab";
                yield return "tm_torment";
                yield return "tm_shadow_claw";
                yield return "tm_endure";
                yield return "tm_echoed_voice";
                yield return "tm_gyro_ball";
                yield return "tm_recycle";
                yield return "tm_false_swipe";
                yield return "tm_defog";
                yield return "tm_telekinesis";
                yield return "tm_double_team";
                yield return "tm_thunder_wave";
                yield return "tm_attract";
                yield return "tm_steel_wing";
                yield return "tm_smack_down";
                yield return "tm_snarl";
                yield return "tm_flame_charge";
                yield return "tm_bulldoze";
                yield return "tm_substitute";
                yield return "tm_iron_tail";
                yield return "tm_brine";
                yield return "tm_venoshock";
                yield return "tm_u_turn";
                yield return "tm_aerial_ace";
                yield return "tm_hone_claws";
                yield return "tm_rock_smash";
            }

            if ((tmClass & TMClass.Starter) != TMClass.None)
            {
                yield return "tm_protect";
                yield return "tm_round";
                yield return "tm_rest";
                yield return "tm_hidden_power";
                yield return "tm_rock_tomb";
                yield return "tm_strength";
                yield return "tm_thief";
                yield return "tm_dig";
                yield return "tm_cut";
                yield return "tm_whirlpool";
                yield return "tm_grass_knot";
                yield return "tm_fly";
                yield return "tm_power_up_punch";
                yield return "tm_infestation";
                yield return "tm_work_up";
                yield return "tm_incinerate";
                yield return "tm_roar";
                yield return "tm_flash";
                yield return "tm_bullet_seed";
            }

            if ((tmClass & TMClass.ShopOnly) != TMClass.None)
            {
                yield return "tm_embargo";
                yield return "tm_dragon_claw";
                yield return "tm_low_sweep";
                yield return "tm_volt_switch";
                yield return "tm_dragon_pulse";
                yield return "tm_sludge_wave";
                yield return "tm_struggle_bug";
                yield return "tm_avalanche";
                yield return "tm_drain_punch";
                yield return "tm_dragon_tail";
                yield return "tm_silver_wind";
                yield return "tm_frost_breath";
                yield return "tm_sky_drop";
                yield return "tm_quash";
            }
        }



        [Flags]
        public enum TMDistroClass
        {
            None = 0,
            Universal = 1,
            High = 2,
            Medium = 4,
            Low = 8,
            Ordinary = 14,
            Natural = 15,
            ShopOnly = 16,
            NonUniversal = 30,
        }

        static IEnumerable<string> IterateDistroTMs(TMDistroClass tmClass)
        {
            if ((tmClass & TMDistroClass.Universal) != TMDistroClass.None)
            {
                yield return "tm_substitute";
                yield return "tm_protect";
                yield return "tm_facade";
                yield return "tm_round";
                yield return "tm_rest";
                yield return "tm_hidden_power";
                yield return "tm_return";
                yield return "tm_frustration";
                yield return "tm_double_team";
                yield return "tm_swagger";
                yield return "tm_secret_power";
                yield return "tm_attract";
                yield return "tm_endure";
                yield return "tm_natural_gift";
                yield return "tm_sunny_day";
                yield return "tm_rain_dance";
                yield return "tm_captivate";
            }


            if ((tmClass & TMDistroClass.High) != TMDistroClass.None)
            {
                yield return "tm_rock_smash";
                yield return "tm_thief";
                yield return "tm_flash";
                yield return "tm_shadow_ball";
                yield return "tm_psych_up";
                yield return "tm_rock_tomb";
                yield return "tm_strength";
                yield return "tm_rock_slide";
                yield return "tm_aerial_ace";
                yield return "tm_fling";
                yield return "tm_ice_beam";
                yield return "tm_dig";
                yield return "tm_safeguard";
                yield return "tm_thunder_wave";
                yield return "tm_grass_knot";
                yield return "tm_light_screen";
                yield return "tm_thunderbolt";
            }


            if ((tmClass & TMDistroClass.Medium) != TMDistroClass.None)
            {
                yield return "tm_blizzard";
                yield return "tm_water_pulse";
                yield return "tm_shock_wave";
                yield return "tm_bulldoze";
                yield return "tm_cut";
                yield return "tm_thunder";
                yield return "tm_psychic";
                yield return "tm_iron_tail";
                yield return "tm_taunt";
                yield return "tm_brick_break";
                yield return "tm_giga_impact";
                yield return "tm_echoed_voice";
                yield return "tm_payback";
                yield return "tm_earthquake";
                yield return "tm_hyper_beam";
                yield return "tm_sandstorm";
                yield return "tm_calm_mind";
                yield return "tm_reflect";
                yield return "tm_charge_beam";
                yield return "tm_dream_eater";
                yield return "tm_flamethrower";
                yield return "tm_swords_dance";
                yield return "tm_surf";
                yield return "tm_fire_blast";
                yield return "tm_energy_ball";
                yield return "tm_work_up";
                yield return "tm_incinerate";
                yield return "tm_hail";
                yield return "tm_retaliate";
                yield return "tm_power_up_punch";
                yield return "tm_roar";
                yield return "tm_torment";
                yield return "tm_shadow_claw";
                yield return "tm_u_turn";
                yield return "tm_whirlpool";
                yield return "tm_hone_claws";
                yield return "tm_dark_pulse";
                yield return "tm_stone_edge";
                yield return "tm_focus_punch";
                yield return "tm_sludge_bomb";
                yield return "tm_poison_jab";
                yield return "tm_giga_drain";
            }

            if ((tmClass & TMDistroClass.Low) != TMDistroClass.None)
            {
                yield return "tm_nature_power";
                yield return "tm_dive";
                yield return "tm_dazzling_gleam";
                yield return "tm_scald";
                yield return "tm_psyshock";
                yield return "tm_waterfall";
                yield return "tm_will_o_wisp";
                yield return "tm_roost";
                yield return "tm_telekinesis";
                yield return "tm_smack_down";
                yield return "tm_focus_blast";
                yield return "tm_wild_charge";
                yield return "tm_rock_polish";
                yield return "tm_fly";
                yield return "tm_steel_wing";
                yield return "tm_explosion";
                yield return "tm_acrobatics";
                yield return "tm_brine";
                yield return "tm_infestation";
                yield return "tm_gyro_ball";
                yield return "tm_recycle";
                yield return "tm_snatch";
                yield return "tm_false_swipe";
                yield return "tm_venoshock";
                yield return "tm_x_scissor";
                yield return "tm_rock_climb";
                yield return "tm_overheat";
                yield return "tm_defog";
                yield return "tm_bulk_up";
                yield return "tm_snarl";
                yield return "tm_flame_charge";
                yield return "tm_flash_cannon";
                yield return "tm_pluck";
                yield return "tm_bullet_seed";
            }

            if ((tmClass & TMDistroClass.ShopOnly) != TMDistroClass.None)
            {
                yield return "tm_embargo";
                yield return "tm_dragon_claw";
                yield return "tm_low_sweep";
                yield return "tm_volt_switch";
                yield return "tm_dragon_pulse";
                yield return "tm_sludge_wave";
                yield return "tm_struggle_bug";
                yield return "tm_avalanche";
                yield return "tm_drain_punch";
                yield return "tm_dragon_tail";
                yield return "tm_silver_wind";
                yield return "tm_frost_breath";
                yield return "tm_sky_drop";
                yield return "tm_quash";
            }
        }

        public static int getTMPrice(string tm_id)
        {
            switch (tm_id)
            {
                case "tm_earthquake": return 5000;
                case "tm_hyper_beam": return 6000;
                case "tm_overheat": return 5000;
                case "tm_blizzard": return 5000;
                case "tm_swords_dance": return 5000;
                case "tm_surf": return 5000;
                case "tm_dark_pulse": return 5000;
                case "tm_psychic": return 5000;
                case "tm_thunder": return 5000;
                case "tm_shadow_ball": return 5000;
                case "tm_ice_beam": return 5000;
                case "tm_giga_impact": return 6000;
                case "tm_fire_blast": return 5000;
                case "tm_dazzling_gleam": return 5000;
                case "tm_flash_cannon": return 5000;
                case "tm_stone_edge": return 5000;
                case "tm_sludge_bomb": return 5000;
                case "tm_focus_blast": return 5000;
                case "tm_explosion": return 3500;
                case "tm_snatch": return 3500;
                case "tm_sunny_day": return 3500;
                case "tm_rain_dance": return 3500;
                case "tm_sandstorm": return 3500;
                case "tm_hail": return 3500;
                case "tm_x_scissor": return 3500;
                case "tm_wild_charge": return 3500;
                case "tm_taunt": return 3500;
                case "tm_focus_punch": return 3500;
                case "tm_safeguard": return 3500;
                case "tm_light_screen": return 3500;
                case "tm_psyshock": return 3500;
                case "tm_will_o_wisp": return 3500;
                case "tm_dream_eater": return 3500;
                case "tm_nature_power": return 3500;
                case "tm_facade": return 3500;
                case "tm_swagger": return 3500;
                case "tm_captivate": return 3500;
                case "tm_rock_slide": return 3500;
                case "tm_fling": return 3500;
                case "tm_thunderbolt": return 3500;
                case "tm_water_pulse": return 3500;
                case "tm_shock_wave": return 3500;
                case "tm_brick_break": return 3500;
                case "tm_payback": return 3500;
                case "tm_calm_mind": return 3500;
                case "tm_reflect": return 3500;
                case "tm_charge_beam": return 3500;
                case "tm_flamethrower": return 3500;
                case "tm_energy_ball": return 3500;
                case "tm_retaliate": return 3500;
                case "tm_scald": return 3500;
                case "tm_waterfall": return 3500;
                case "tm_roost": return 3500;
                case "tm_rock_polish": return 3500;
                case "tm_acrobatics": return 3500;
                case "tm_rock_climb": return 3500;
                case "tm_bulk_up": return 3500;
                case "tm_pluck": return 3500;
                case "tm_psych_up": return 3500;
                case "tm_secret_power": return 3500;
                case "tm_natural_gift": return 3500;
                case "tm_return": return 2500;
                case "tm_frustration": return 2500;
                case "tm_giga_drain": return 2500;
                case "tm_dive": return 2500;
                case "tm_poison_jab": return 2500;
                case "tm_torment": return 2500;
                case "tm_shadow_claw": return 2500;
                case "tm_endure": return 2500;
                case "tm_echoed_voice": return 2500;
                case "tm_gyro_ball": return 2500;
                case "tm_recycle": return 2500;
                case "tm_false_swipe": return 2500;
                case "tm_defog": return 2500;
                case "tm_telekinesis": return 2500;
                case "tm_double_team": return 2500;
                case "tm_thunder_wave": return 2500;
                case "tm_attract": return 2500;
                case "tm_steel_wing": return 2500;
                case "tm_smack_down": return 2500;
                case "tm_snarl": return 2500;
                case "tm_flame_charge": return 2500;
                case "tm_bulldoze": return 2500;
                case "tm_substitute": return 2500;
                case "tm_iron_tail": return 2500;
                case "tm_brine": return 2500;
                case "tm_venoshock": return 2500;
                case "tm_u_turn": return 2500;
                case "tm_aerial_ace": return 2500;
                case "tm_hone_claws": return 2500;
                case "tm_rock_smash": return 2500;
                case "tm_protect": return 2500;
                case "tm_round": return 2500;
                case "tm_rest": return 2500;
                case "tm_hidden_power": return 2500;
                case "tm_rock_tomb": return 2500;
                case "tm_strength": return 2500;
                case "tm_thief": return 2500;
                case "tm_dig": return 3500;
                case "tm_cut": return 2500;
                case "tm_whirlpool": return 2500;
                case "tm_grass_knot": return 2500;
                case "tm_fly": return 3500;
                case "tm_power_up_punch": return 3500;
                case "tm_infestation": return 2500;
                case "tm_work_up": return 3500;
                case "tm_incinerate": return 2500;
                case "tm_roar": return 2500;
                case "tm_flash": return 2500;
                case "tm_bullet_seed": return 3500;
                case "tm_embargo": return 2500;
                case "tm_dragon_claw": return 3500;
                case "tm_low_sweep": return 2500;
                case "tm_volt_switch": return 3500;
                case "tm_dragon_pulse": return 5000;
                case "tm_sludge_wave": return 5000;
                case "tm_struggle_bug": return 2500;
                case "tm_avalanche": return 2500;
                case "tm_drain_punch": return 3500;
                case "tm_dragon_tail": return 3500;
                case "tm_silver_wind": return 5000;
                case "tm_frost_breath": return 3500;
                case "tm_sky_drop": return 2500;
                case "tm_quash": return 2500;
            }
            throw new Exception("Unknown TM ID");
        }

        static IEnumerable<string> IterateTypeBoosters()
        {
            yield return "held_silver_powder";
            yield return "held_black_glasses";
            yield return "held_dragon_scale";
            yield return "held_magnet";
            yield return "held_pink_bow";
            yield return "held_black_belt";
            yield return "held_charcoal";
            yield return "held_sharp_beak";
            yield return "held_spell_tag";
            yield return "held_miracle_seed";
            yield return "held_soft_sand";
            yield return "held_never_melt_ice";
            yield return "held_silk_scarf";
            yield return "held_poison_barb";
            yield return "held_twisted_spoon";
            yield return "held_hard_stone";
            yield return "held_metal_coat";
            yield return "held_mystic_water";
        }

        static IEnumerable<string> IterateTypePlates()
        {
            yield return "held_insect_plate";
            yield return "held_dread_plate";
            yield return "held_draco_plate";
            yield return "held_zap_plate";
            yield return "held_pixie_plate";
            yield return "held_fist_plate";
            yield return "held_flame_plate";
            yield return "held_sky_plate";
            yield return "held_spooky_plate";
            yield return "held_meadow_plate";
            yield return "held_earth_plate";
            yield return "held_icicle_plate";
            yield return "held_blank_plate";
            yield return "held_toxic_plate";
            yield return "held_mind_plate";
            yield return "held_stone_plate";
            yield return "held_iron_plate";
            yield return "held_splash_plate";
        }

        static IEnumerable<string> IterateManmades()
        {
            yield return "medicine_amber_tear";
            yield return "medicine_potion";
            yield return "medicine_max_potion";
            yield return "medicine_elixir";
            yield return "medicine_max_elixir";
            yield return "medicine_full_heal";
            yield return "medicine_full_restore";
            yield return "medicine_x_attack";
            yield return "medicine_x_defense";
            yield return "medicine_x_sp_atk";
            yield return "medicine_x_sp_def";
            yield return "medicine_x_speed";
            yield return "medicine_x_accuracy";
            yield return "medicine_dire_hit";
        }

        static IEnumerable<string> IterateMachines()
        {
            yield return "machine_recall_box";
            yield return "machine_assembly_box";
            yield return "machine_storage_box";
            yield return "machine_ability_capsule";
        }


        public static void CreateContentLists()
        {
            List<string> monsters = new List<string>();
            List<string> skills = new List<string>();
            List<string> intrinsics = new List<string>();
            List<string> statuses = new List<string>();
            List<string> ai = new List<string>();
            List<string> items = new List<string>();
            List<string> roles = new List<string>();



            foreach(string key in DataManager.Instance.DataIndices[DataManager.DataType.Monster].GetOrderedKeys(false))
                monsters.Add(key);
            
            foreach(string key in DataManager.Instance.DataIndices[DataManager.DataType.Skill].GetOrderedKeys(false))
                skills.Add(key);
            
            foreach(string key in DataManager.Instance.DataIndices[DataManager.DataType.Intrinsic].GetOrderedKeys(false))
                intrinsics.Add(key);

            foreach (string key in DataManager.Instance.DataIndices[DataManager.DataType.Status].GetOrderedKeys(false))
                statuses.Add(key);
            
            foreach(string key in DataManager.Instance.DataIndices[DataManager.DataType.AI].GetOrderedKeys(false))
                ai.Add(key);

            foreach (string key in DataManager.Instance.DataIndices[DataManager.DataType.Item].GetOrderedKeys(false))
            {
                if (!key.StartsWith("xcl_"))
                    items.Add(key);
            }

            for (int ii = 0; ii <= (int)TeamMemberSpawn.MemberRole.Loner; ii++)
                roles.Add(((TeamMemberSpawn.MemberRole)ii).ToString());


            string path = GenPath.ZONE_PATH + "PreZone.txt";
            using (StreamWriter file = new StreamWriter(path))
            {
                file.WriteLine("Monster\tSkill\tIntrinsic\tStatus\tAI\tItem\tRole");
                file.WriteLine("---\t---\t---\t---\t--\t----\t---");
                int ii = 0;
                bool completed = false;
                while (!completed)
                {
                    completed = true;

                    List<string> row = new List<string>();

                    if (ii < monsters.Count)
                    {
                        row.Add(monsters[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    if (ii < skills.Count)
                    {
                        row.Add(skills[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    if (ii < intrinsics.Count)
                    {
                        row.Add(intrinsics[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    if (ii < statuses.Count)
                    {
                        row.Add(statuses[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    if (ii < ai.Count)
                    {
                        row.Add(ai[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    if (ii < items.Count)
                    {
                        row.Add(items[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");
                    if (ii < roles.Count)
                    {
                        row.Add(roles[ii]);
                        completed = false;
                    }
                    else
                        row.Add("");

                    file.WriteLine(String.Join('\t', row.ToArray()));
                    ii++;
                }
            }
        }

    }
}
