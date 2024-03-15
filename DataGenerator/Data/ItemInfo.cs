using System;
using System.Collections.Generic;
using RogueEssence.Dungeon;
using RogueEssence.Content;
using RogueEssence;
using RogueEssence.Data;
using PMDC.Dungeon;
using PMDC;
using PMDC.Data;
using RogueEssence.Ground;

namespace DataGenerator.Data
{
    public class ItemInfo
    {
        public const int MAX_NORMAL_ITEMS = 700;
        public const int MAX_INIT_EXCL_ITEMS = 900;
        public const int MAX_ITEMS = 2500;

        /// <summary>
        /// Computes all items with just english translation.
        /// </summary>
        public static void AddItemData()
        {
            DataInfo.DeleteIndexedData(DataManager.DataType.Item.ToString());

            for (int ii = 0; ii < MAX_NORMAL_ITEMS; ii++)
            {
                (string, ItemData) item = GetItemData(ii);
                if (item.Item1 != "")
                    DataManager.SaveData(item.Item1, DataManager.DataType.Item.ToString(), item.Item2);
            }
            AddExclItemData(false);
        }
        /// <summary>
        /// Recomputes all exclusive items with full string translations
        /// </summary>
        public static void AddCalculatedItemData()
        {
            AutoItemInfo.InitStringsAll();
            AddExclItemData(true);
        }

        public static (string, ItemData) GetItemData(int ii)
        {
            string fileName = "";
            ItemData item = new ItemData();
            item.UseEvent.Element = "none";

            if (ii == 0)
            {
                item.Name = new LocalText("Empty");
                fileName = "empty";
            }
            else if (ii == 1)
            {
                item.Name = new LocalText("Apple");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly.");
                item.Sprite = "Apple_Red";
                item.Price = 10;
                item.UseEvent.OnHits.Add(0, new RestoreBellyEvent(50, true, 3, true, new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Green_In", 2)), "DUN_Growth", true)));
            }
            else if (ii == 2)
            {
                item.Name = new LocalText("Big Apple");
                item.Desc = new LocalText("A food item that amply fills the Pokémon's belly.");
                item.Sprite = "Apple_Red";
                item.Price = 30;
                item.UseEvent.OnHits.Add(0, new RestoreBellyEvent(100, true, 6, true, new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Green_In", 2)), "DUN_Growth", true)));
            }
            else if (ii == 3)
            {
                item.Name = new LocalText("Huge Apple");
                item.Desc = new LocalText("A food item that completely fills and slightly enlarges the Pokémon's belly.");
                item.Sprite = "Apple_Red";
                item.Price = 80;
                item.UseEvent.OnHits.Add(0, new RestoreBellyEvent(200, true, 10, false, new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Green_In", 2)), "DUN_Growth", true)));
            }
            else if (ii == 4)
            {
                item.Name = new LocalText("Golden Apple");
                item.Desc = new LocalText("A miraculous apple that glows with an alluring golden aura. It's far too precious and beautiful to even consider eating! If eaten, however, it would completely fill and greatly enlarge the Pokémon's belly.");
                item.Sprite = "Apple_Gold";
                item.Price = 3000;
                item.UseEvent.OnHits.Add(0, new RestoreBellyEvent(200, true, 50, false, new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Green_In", 2)), "DUN_Growth", true)));
            }
            else if (ii == 5)
            {
                item.Name = new LocalText("Perfect Apple");
                item.Desc = new LocalText("A food item that completely fills and somewhat enlarges the Pokémon's belly.");
                item.Sprite = "Apple_Red";
                item.Price = 300;
                item.UseEvent.OnHits.Add(0, new RestoreBellyEvent(200, true, 20, false, new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Green_In", 2)), "DUN_Growth", true)));
            }
            else if (ii == 6)
            {
                item.Name = new LocalText("Banana");
                item.Desc = new LocalText("A rare food item that somewhat fills the entire team's belly.");
                item.Sprite = "Banana_Yellow";
                item.Price = 100;
                item.UseEvent.OnHits.Add(0, new RestoreBellyEvent(35, true, 2, true, new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Green_In", 2)), "DUN_Growth", true)));
            }
            else if (ii == 7)
            {
                item.Name = new LocalText("Big Banana");
                item.Desc = new LocalText("A rare food item that amply fills the entire team's belly.");
                item.Sprite = "Banana_Yellow";
                item.Price = 300;
                item.UseEvent.OnHits.Add(0, new RestoreBellyEvent(70, true, 5, true, new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Green_In", 2)), "DUN_Growth", true)));
            }
            else if (ii == 8)
            {
                item.Name = new LocalText("Golden Banana");
                item.Desc = new LocalText("An alluring banana that gives off a golden glow. It's valued far more as a treasure than as a food. If eaten, however, it would completely fill and enlarge the entire team's belly.");
                item.Sprite = "Banana_Yellow";
                item.Price = 4000;
                item.UseEvent.OnHits.Add(0, new RestoreBellyEvent(100, true, 20, false, new BattleAnimEvent(new SingleEmitter(new AnimData("Circle_Small_Green_In", 2)), "DUN_Growth", true)));
            }
            else if (ii == 9)
            {
                item.Name = new LocalText("**Chestnut");
            }
            else if (ii == 10)
            {
                item.Name = new LocalText("Oran Berry");
                item.Desc = new LocalText("A peculiar berry with a mix of flavors. It fully restores HP.");
                item.Sprite = "Berry_Oran";
                item.UseEvent.OnHits.Add(0, new RestoreHPEvent(1, 1, true));
            }
            else if (ii == 11)
            {
                item.Name = new LocalText("Leppa Berry");
                item.Desc = new LocalText("A berry that takes a long time to grow. It fully restores PP.");
                item.Sprite = "Berry_Leppa";
                item.UseEvent.OnHits.Add(0, new RestorePPEvent(100));
            }
            else if (ii == 12)
            {
                item.Name = new LocalText("Lum Berry");
                item.Desc = new LocalText("A berry that matures slowly, storing nutrients beneficial to Pokémon health. It cures many status problems.");
                item.Sprite = "Berry_Lum";
                item.ItemStates.Set(new CurerState());
                item.UseEvent.BeforeActions.Add(-1, new AddContextStateEvent(new CureAttack()));
                item.UseEvent.OnHits.Add(0, new RemoveStateStatusBattleEvent(typeof(BadStatusState), true, new StringKey("MSG_CURE_ALL")));
            }
            else if (ii == 13)
            {
                item.Name = new LocalText("**Cheri Berry");
            }
            else if (ii == 14)
            {
                item.Name = new LocalText("**Chesto Berry");
            }
            else if (ii == 15)
            {
                item.Name = new LocalText("**Pecha Berry");
            }
            else if (ii == 16)
            {
                item.Name = new LocalText("**Aspear Berry");
            }
            else if (ii == 17)
            {
                item.Name = new LocalText("**Rawst Berry");
            }
            else if (ii == 18)
            {
                item.Name = new LocalText("**Persim Berry");
            }
            else if (ii == 19)
            {
                item.Name = new LocalText("Tanga Berry");
                item.Desc = new LocalText("The flower grows at the tip of this berry. It attracts Bug Pokémon by letting its stringy petals stream out. It changes the Pokémon to the Bug-type.");
                item.Sprite = "Berry_Tanga";
                item.UseEvent.OnHits.Add(0, new ChangeToElementEvent("bug"));
            }
            else if (ii == 20)
            {
                item.Name = new LocalText("Colbur Berry");
                item.Desc = new LocalText("Tiny hooks grow on the surface of this berry. It latches on to Pokémon so it can be carried to far-off places. It changes the Pokémon to the Dark-type.");
                item.Sprite = "Berry_Colbur";
                item.UseEvent.OnHits.Add(0, new ChangeToElementEvent("dark"));
            }
            else if (ii == 21)
            {
                item.Name = new LocalText("Haban Berry");
                item.Desc = new LocalText("If a large enough volume of this berry is boiled down, its bitterness fades away. It makes a good jam. It changes the Pokémon to the Dragon-type.");
                item.Sprite = "Berry_Haban";
                item.UseEvent.OnHits.Add(0, new ChangeToElementEvent("dragon"));
            }
            else if (ii == 22)
            {
                item.Name = new LocalText("Wacan Berry");
                item.Desc = new LocalText("Energy from lightning strikes is drawn into the plant, making the berries grow big and rich. It changes the Pokémon to the Electric-type.");
                item.Sprite = "Berry_Wacan";
                item.UseEvent.OnHits.Add(0, new ChangeToElementEvent("electric"));
            }
            else if (ii == 23)
            {
                item.Name = new LocalText("Chople Berry");
                item.Desc = new LocalText("This berry contains a substance that generates heat. It can even heat up a chilly heart. It changes the Pokémon to the Fighting-type.");
                item.Sprite = "Berry_Chople";
                item.UseEvent.OnHits.Add(0, new ChangeToElementEvent("fighting"));
            }
            else if (ii == 24)
            {
                item.Name = new LocalText("Occa Berry");
                item.Desc = new LocalText("This berry is said to have grown plentiful in the tropics of the past. It boasts an intensely hot spiciness. It changes the Pokémon to the Fire-type.");
                item.Sprite = "Berry_Occa";
                item.UseEvent.OnHits.Add(0, new ChangeToElementEvent("fire"));
            }
            else if (ii == 25)
            {
                item.Name = new LocalText("Coba Berry");
                item.Desc = new LocalText("This berry is said to be a new kind that is a cross of two berries brought together by winds from far away. It changes the Pokémon to the Flying-type.");
                item.Sprite = "Berry_Coba";
                item.UseEvent.OnHits.Add(0, new ChangeToElementEvent("flying"));
            }
            else if (ii == 26)
            {
                item.Name = new LocalText("Kasib Berry");
                item.Desc = new LocalText("Considered to have a special power from the olden days, this berry is sometimes dried and used as a good-luck charm. It changes the Pokémon to the Ghost-type.");
                item.Sprite = "Berry_Kasib";
                item.UseEvent.OnHits.Add(0, new ChangeToElementEvent("ghost"));
            }
            else if (ii == 27)
            {
                item.Name = new LocalText("Rindo Berry");
                item.Desc = new LocalText("This berry has a disagreeable \"green\" flavor and scent typical of vegetables. It is rich in health-promoting fiber. It changes the Pokémon to the Grass-type.");
                item.Sprite = "Berry_Rindo";
                item.UseEvent.OnHits.Add(0, new ChangeToElementEvent("grass"));
            }
            else if (ii == 28)
            {
                item.Name = new LocalText("Shuca Berry");
                item.Desc = new LocalText("The sweetness-laden pulp has just the hint of a hard-edged and fragrant bite to it. It changes the Pokémon to the Ground-type.");
                item.Sprite = "Berry_Shuca";
                item.UseEvent.OnHits.Add(0, new ChangeToElementEvent("ground"));
            }
            else if (ii == 29)
            {
                item.Name = new LocalText("Yache Berry");
                item.Desc = new LocalText("This berry has a refreshing flavor that strikes a good balance of dryness and sourness. It tastes better chilled. It changes the Pokémon to the Ice-type.");
                item.Sprite = "Berry_Yache";
                item.UseEvent.OnHits.Add(0, new ChangeToElementEvent("ice"));
            }
            else if (ii == 30)
            {
                item.Name = new LocalText("Chilan Berry");
                item.Desc = new LocalText("This berry can be cored out and dried to make a whistle. Blowing through its hole makes an indescribable sound. It changes the Pokémon to the Normal-type.");
                item.Sprite = "Berry_Chilan";
                item.UseEvent.OnHits.Add(0, new ChangeToElementEvent("normal"));
            }
            else if (ii == 31)
            {
                item.Name = new LocalText("Kebia Berry");
                item.Desc = new LocalText("This berry is a brilliant green on the outside. Inside, it is packed with a dry-flavored, black-colored flesh. It changes the Pokémon to the Poison-type.");
                item.Sprite = "Berry_Kebia";
                item.UseEvent.OnHits.Add(0, new ChangeToElementEvent("poison"));
            }
            else if (ii == 32)
            {
                item.Name = new LocalText("Payapa Berry");
                item.Desc = new LocalText("This berry is said to sense emotions for the way it swells roundly when anything approaches. It changes the Pokémon to the Psychic-type.");
                item.Sprite = "Berry_Payapa";
                item.UseEvent.OnHits.Add(0, new ChangeToElementEvent("psychic"));
            }
            else if (ii == 33)
            {
                item.Name = new LocalText("Charti Berry");
                item.Desc = new LocalText("It is often used for pickles because of its very dry flavor. It can also be eaten raw for its provocative taste. It changes the Pokémon to the Rock-type.");
                item.Sprite = "Berry_Charti";
                item.UseEvent.OnHits.Add(0, new ChangeToElementEvent("rock"));
            }
            else if (ii == 34)
            {
                item.Name = new LocalText("Babiri Berry");
                item.Desc = new LocalText("This berry is very tough with a strong flavor. It was used to make medicine in the past. It changes the Pokémon to the Steel-type.");
                item.Sprite = "Berry_Babiri";
                item.UseEvent.OnHits.Add(0, new ChangeToElementEvent("steel"));
            }
            else if (ii == 35)
            {
                item.Name = new LocalText("Passho Berry");
                item.Desc = new LocalText("This berry's flesh is dotted with tiny bubbles of air that keep it afloat in water. It changes the Pokémon to the Water-type.");
                item.Sprite = "Berry_Passho";
                item.UseEvent.OnHits.Add(0, new ChangeToElementEvent("water"));
            }
            else if (ii == 36)
            {
                item.Name = new LocalText("Roseli Berry");
                item.Desc = new LocalText("The slight bitterness that cuts through the rich sweetness makes this Berry perfect for adding an accent.  It changes the Pokémon to the Fairy-type.");
                item.Sprite = "Berry_Roseli";
                item.UseEvent.OnHits.Add(0, new ChangeToElementEvent("fairy"));
            }
            else if (ii == 37)
            {
                item.Name = new LocalText("Jaboca Berry");
                item.Desc = new LocalText("The cluster of drupelets that make up this berry pop rhythmically if the berry is handled roughly. If a physical attack hits the Pokémon, the damage will be reflected to all enemies nearby.");
                item.Sprite = "Berry_Jaboca";
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CategoryState(BattleData.SkillCategory.Physical));
                item.UseEvent.OnHits.Add(0, new StatusStateBattleEvent("area_counter", true, false, statusStates));
            }
            else if (ii == 38)
            {
                item.Name = new LocalText("Rowap Berry");
                item.Desc = new LocalText("In days of old, people worked the top-shaped pieces of this berry free and used them as toys. If a special attack hits the Pokémon, the damage will be reflected to all enemies nearby.");
                item.Sprite = "Berry_Rowap";
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CategoryState(BattleData.SkillCategory.Magical));
                item.UseEvent.OnHits.Add(0, new StatusStateBattleEvent("area_counter", true, false, statusStates));
            }
            else if (ii == 39)
            {
                item.Name = new LocalText("**Kelpsy Berry");
            }
            else if (ii == 40)
            {
                item.Name = new LocalText("**Qualot Berry");
            }
            else if (ii == 41)
            {
                item.Name = new LocalText("**Tamato Berry");
            }
            else if (ii == 42)
            {
                item.Name = new LocalText("**Hondew Berry");
            }
            else if (ii == 43)
            {
                item.Name = new LocalText("Apicot Berry");
                item.Desc = new LocalText("A very mystifying Berry. There is no telling how it can be used, or what may happen if it is used. It maximizes Special Defense.");
                item.Sprite = "Berry_Apicot";
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_special_defense", true, false, 6));
            }
            else if (ii == 44)
            {
                item.Name = new LocalText("Liechi Berry");
                item.Desc = new LocalText("A mysterious Berry, rumored to contain the power of the sea. It maximizes Attack.");
                item.Sprite = "Berry_Liechi";
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 6));
            }
            else if (ii == 45)
            {
                item.Name = new LocalText("Ganlon Berry");
                item.Desc = new LocalText("A Berry surrounded by mystery, rumored to be imbued with the power of the land. It maximizes Defense.");
                item.Sprite = "Berry_Ganlon";
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, 6));
            }
            else if (ii == 46)
            {
                item.Name = new LocalText("Salac Berry");
                item.Desc = new LocalText("A Berry surrounded by mystery, rumored to be imbued with the power of the sky. It maximizes Movement Speed.");
                item.Sprite = "Berry_Salac";
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, 3));
            }
            else if (ii == 47)
            {
                item.Name = new LocalText("Petaya Berry");
                item.Desc = new LocalText("A mysterious Berry rumored to contain the power of all living things. It maximizes Special Attack.");
                item.Sprite = "Berry_Petaya";
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, 6));
            }
            else if (ii == 48)
            {
                item.Name = new LocalText("Starf Berry");
                item.Desc = new LocalText("So strong, it was abandoned at the world's edge. It is still considered a mirage to this very day. It sharply raises all stats.");
                item.Sprite = "Berry_Starf";
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, 2));
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 2));
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, 2));
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, 2));
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_special_defense", true, false, 2));
            }
            else if (ii == 49)
            {
                item.Name = new LocalText("Micle Berry");
                item.Desc = new LocalText("This berry has a very dry flavor. It has the effect of making other food eaten at the same time taste sweet. It maximizes Accuracy.");
                item.Sprite = "Berry_Micle";
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_accuracy", true, false, 6));
            }
            else if (ii == 50)
            {
                item.Name = new LocalText("**Custap Berry");
            }
            else if (ii == 51)
            {
                item.Name = new LocalText("Enigma Berry");
                item.Desc = new LocalText("A completely enigmatic Berry that appears to have the power of stars. It restores HP when hit by a super-effective move.");
                item.Sprite = "Berry_Enigma";
                item.UseEvent.OnHits.Add(0, new StatusBattleEvent("weakness_drain", true, false));
            }
            else if (ii == 52)
            {
                item.Name = new LocalText("**Kee Berry");
            }
            else if (ii == 53)
            {
                item.Name = new LocalText("**Maranga Berry");
            }
            else if (ii == 72)
            {
                item.Name = new LocalText("Sitrus Berry");
                item.Desc = new LocalText("A large berry with a well-rounded flavor. It gradually restores HP for a few turns.");
                item.Sprite = "Berry_Sitrus";
                item.UseEvent.OnHits.Add(0, new StatusBattleEvent("regeneration", true, false));
            }
            else if (ii == 73)
            {
            }
            else if (ii == 74)
            {
            }
            else if (ii == 75)
            {
                item.Name = new LocalText("Wonder Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats.");
                item.Sprite = "Gummi_White";
                item.UseEvent.OnHits.Add(0, new VitaminEvent(Stat.None, 2));
                item.UseEvent.OnHits.Add(0, new RestoreBellyEvent(20, true));
            }
            else if (ii == 76)
            {
                item.Name = new LocalText("Blue Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats. It usually boosts Maximum HP, and Water-type Pokémon like it the most.");
                item.Sprite = "Gummi_Blue";
                item.UseEvent.OnHits.Add(0, new VitaGummiEvent("water", true, Stat.HP));
            }
            else if (ii == 77)
            {
                item.Name = new LocalText("Black Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats. It usually boosts Maximum HP, and Dark-type Pokémon like it the most.");
                item.Sprite = "Gummi_Black";
                item.UseEvent.OnHits.Add(0, new VitaGummiEvent("dark", true, Stat.HP));
            }
            else if (ii == 78)
            {
                item.Name = new LocalText("Clear Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats. It usually boosts Special Defense, and Ice-type Pokémon like it the most.");
                item.Sprite = "Gummi_Clear";
                item.UseEvent.OnHits.Add(0, new VitaGummiEvent("ice", true, Stat.MDef));
            }
            else if (ii == 79)
            {
                item.Name = new LocalText("Grass Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats. It usually boosts Special Defense, and Grass-type Pokémon like it the most.");
                item.Sprite = "Gummi_Grass";
                item.UseEvent.OnHits.Add(0, new VitaGummiEvent("grass", true, Stat.MDef));
            }
            else if (ii == 80)
            {
                item.Name = new LocalText("Green Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats. It usually boosts Speed, and Bug-type Pokémon like it the most.");
                item.Sprite = "Gummi_Green";
                item.UseEvent.OnHits.Add(0, new VitaGummiEvent("bug", true, Stat.Speed));
            }
            else if (ii == 81)
            {
                item.Name = new LocalText("Brown Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats. It usually boosts Attack, and Ground-type Pokémon like it the most.");
                item.Sprite = "Gummi_Brown";
                item.UseEvent.OnHits.Add(0, new VitaGummiEvent("ground", true, Stat.Attack));
            }
            else if (ii == 82)
            {
                item.Name = new LocalText("Orange Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats. It usually boosts Attack, and Fighting-type Pokémon like it the most.");
                item.Sprite = "Gummi_Orange";
                item.UseEvent.OnHits.Add(0, new VitaGummiEvent("fighting", true, Stat.Attack));
            }
            else if (ii == 83)
            {
                item.Name = new LocalText("Gold Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats. It usually boosts Special Attack, and Psychic-type Pokémon like it the most.");
                item.Sprite = "Gummi_Gold";
                item.UseEvent.OnHits.Add(0, new VitaGummiEvent("psychic", true, Stat.MAtk));
            }
            else if (ii == 84)
            {
                item.Name = new LocalText("Pink Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats. It usually boosts Defense, and Poison-type Pokémon like it the most.");
                item.Sprite = "Gummi_Pink";
                item.UseEvent.OnHits.Add(0, new VitaGummiEvent("poison", true, Stat.Defense));
            }
            else if (ii == 85)
            {
                item.Name = new LocalText("Purple Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats. It usually boosts Special Attack, and Ghost-type Pokémon like it the most.");
                item.Sprite = "Gummi_Purple";
                item.UseEvent.OnHits.Add(0, new VitaGummiEvent("ghost", true, Stat.MAtk));
            }
            else if (ii == 86)
            {
                item.Name = new LocalText("Red Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats. It usually boosts Special Attack, and Fire-type Pokémon like it the most.");
                item.Sprite = "Gummi_Red";
                item.UseEvent.OnHits.Add(0, new VitaGummiEvent("fire", true, Stat.MAtk));
            }
            else if (ii == 87)
            {
                item.Name = new LocalText("Royal Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats. It usually boosts Attack, and Dragon-type Pokémon like it the most.");
                item.Sprite = "Gummi_Royal";
                item.UseEvent.OnHits.Add(0, new VitaGummiEvent("dragon", true, Stat.Attack));
            }
            else if (ii == 88)
            {
                item.Name = new LocalText("Silver Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats. It usually boosts Defense, and Steel-type Pokémon like it the most.");
                item.Sprite = "Gummi_Silver";
                item.UseEvent.OnHits.Add(0, new VitaGummiEvent("steel", true, Stat.Defense));
            }
            else if (ii == 89)
            {
                item.Name = new LocalText("White Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats. It usually boosts Maximum HP, and Normal-type Pokémon like it the most.");
                item.Sprite = "Gummi_White";
                item.UseEvent.OnHits.Add(0, new VitaGummiEvent("normal", true, Stat.HP));
            }
            else if (ii == 90)
            {
                item.Name = new LocalText("Yellow Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats. It usually boosts Speed, and Electric-type Pokémon like it the most.");
                item.Sprite = "Gummi_Yellow";
                item.UseEvent.OnHits.Add(0, new VitaGummiEvent("electric", true, Stat.Speed));
            }
            else if (ii == 91)
            {
                item.Name = new LocalText("Sky Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats. It usually boosts Speed, and Flying-type Pokémon like it the most.");
                item.Sprite = "Gummi_Sky";
                item.UseEvent.OnHits.Add(0, new VitaGummiEvent("flying", true, Stat.Speed));
            }
            else if (ii == 92)
            {
                item.Name = new LocalText("Gray Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats. It usually boosts Defense, and Rock-type Pokémon like it the most.");
                item.Sprite = "Gummi_Gray";
                item.UseEvent.OnHits.Add(0, new VitaGummiEvent("rock", true, Stat.Defense));
            }
            else if (ii == 93)
            {
                item.Name = new LocalText("Magenta Gummi");
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly, and permanently boosts its stats. It usually boosts Special Defense, and Fairy-type Pokémon like it the most.");
                item.Sprite = "Gummi_Magenta";
                item.UseEvent.OnHits.Add(0, new VitaGummiEvent("fairy", true, Stat.MDef));
            }
            else if (ii == 94)
            {

            }
            else if (ii == 100)
            {
                item.Name = new LocalText("Plain Seed");
                item.Desc = new LocalText("A seed that has no special effect.");
                item.Sprite = "Seed_White";
            }
            else if (ii == 101)
            {
                item.Name = new LocalText("Reviver Seed");
                item.Desc = new LocalText("A seed that revives a fainted Pokémon, then becomes a Plain Seed after use.");
                item.Sprite = "Seed_Yellow";
                item.BagEffect = true;
                item.OnDeaths.Add(5, new AutoReviveEvent(true, "seed_plain"));
            }
            else if (ii == 102)
            {
                item.Name = new LocalText("Joy Seed");
                item.Desc = new LocalText("A seed that raises the Pokémon's level by 1.");
                item.Sprite = "Seed_White";
                item.UseEvent.OnHits.Add(0, new LevelChangeEvent(1));
            }
            else if (ii == 103)
            {
                item.Name = new LocalText("Golden Seed");
                item.Desc = new LocalText("A seed that raises the Pokémon's level by 3.");
                item.Sprite = "Seed_Yellow";
                item.UseEvent.OnHits.Add(0, new LevelChangeEvent(3));
            }
            else if (ii == 104)
            {
                item.Name = new LocalText("Doom Seed");
                item.Desc = new LocalText("A seed that reduces the Pokémon's level by 5.");
                item.Sprite = "Seed_DarkBlue";
                item.UseEvent.OnHits.Add(0, new LevelChangeEvent(-5));
            }
            else if (ii == 105)
            {
                item.Name = new LocalText("**Spreader Seed");
                item.Desc = new LocalText("A seed that causes the Pokémon to share the effects of moves used on it.");
                item.Sprite = "Seed_Red";
                //instant splash damage
            }
            else if (ii == 106)
            {
                item.Name = new LocalText("**Training Seed");
            }
            else if (ii == 107)
            {
                item.Name = new LocalText("Hunger Seed");
                item.Desc = new LocalText("A seed that makes the Pokémon's belly somewhat emptier.");
                item.Sprite = "Seed_Brown";
                item.UseEvent.OnHits.Add(0, new RestoreBellyEvent(-35, true));
            }
            else if (ii == 108)
            {
                item.Name = new LocalText("Warp Seed");
                item.Desc = new LocalText("A seed that warps the Pokémon and its nearby allies to a different place on the floor.");
                item.Sprite = "Seed_Green";
                item.UseEvent.OnHits.Add(0, new RandomGroupWarpEvent(80, true));
            }
            else if (ii == 109)
            {
                item.Name = new LocalText("**Vanish Seed");
            }
            else if (ii == 110)
            {
                item.Name = new LocalText("Sleep Seed");
                item.Desc = new LocalText("A seed that puts the Pokémon to sleep for many turns.");
                item.Sprite = "Seed_DarkBlue";
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(20));
                item.UseEvent.OnHits.Add(0, new StatusStateBattleEvent("sleep", true, false, statusStates));
            }
            else if (ii == 111)
            {
                item.Name = new LocalText("Vile Seed");
                item.Desc = new LocalText("A seed that drastically lowers the Pokémon's Defense and Special Defense.");
                item.Sprite = "Seed_DarkBlue";
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, -3));
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_special_defense", true, false, -3));
            }
            else if (ii == 112)
            {
                item.Name = new LocalText("Blast Seed");
                item.Desc = new LocalText("A seed that causes the Pokémon to release a blast on the tile in front of it, causing damage.");
                item.Sprite = "Seed_Red";
                AttackAction altAction = new AttackAction();
                altAction.CharAnimData = new CharAnimFrameType(07);//Shoot
                altAction.WideAngle = AttackCoverage.FrontAndCorners;
                altAction.HitTiles = true;
                altAction.BurstTiles = TileAlignment.Any;
                altAction.TargetAlignments |= Alignment.Foe;

                ExplosionData altExplosion = new ExplosionData();
                altExplosion.Range = 1;
                altExplosion.HitTiles = true;
                altExplosion.Speed = 10;
                altExplosion.ExplodeFX.Sound = "DUN_Self-Destruct";
                altExplosion.TargetAlignments |= Alignment.Foe;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Blast_Seed", 3));
                emitter.ParticlesPerTile = 0.8;
                altExplosion.Emitter = emitter;

                BattleData newData = new BattleData();
                newData.Element = "none";
                newData.HitRate = -1;
                newData.OnHits.Add(-1, new LevelDamageEvent(false, 2, 1));
                newData.OnHitTiles.Add(0, new RemoveItemEvent(true));
                newData.OnHitTiles.Add(0, new RemoveTrapEvent());
                newData.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new EmptyFiniteEmitter(), new FlagType(typeof(WallTerrainState)), new FlagType(typeof(FoliageTerrainState))));
                item.UseEvent.OnHits.Add(0, new InvokeCustomBattleEvent(altAction, altExplosion, newData, new StringKey(), true));
            }
            else if (ii == 113)
            {
                item.Name = new LocalText("Blinker Seed");
                item.Desc = new LocalText("A seed that gives the Blinker status, reducing the Pokémon's Attack Range to its lowest level.");
                item.Sprite = "Seed_DarkBlue";
                item.UseEvent.OnHits.Add(0, new StatusBattleEvent("blinker", true, false));
            }
            else if (ii == 114)
            {
                item.Name = new LocalText("Pure Seed");
                item.Desc = new LocalText("A seed that warps the Pokémon close to the stairs.");
                item.Sprite = "Seed_White";
                item.UseEvent.OnHits.Add(0, new WarpToEndEvent(80, 4, true));
            }
            else if (ii == 115)
            {
                item.Name = new LocalText("Ice Seed");
                item.Desc = new LocalText("A seed that freezes the Pokémon for many turns.");
                item.Sprite = "Seed_Blue";
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(30));
                item.UseEvent.OnHits.Add(0, new StatusStateBattleEvent("freeze", true, false, statusStates));
            }
            else if (ii == 116)
            {
                item.Name = new LocalText("Decoy Seed");
                item.Desc = new LocalText("A seed that gives the Pokémon the Decoy status, making it a target of everyone's attacks. Moves that target the team's inventory will always pick this item first.");
                item.Sprite = "Seed_Yellow";
                item.UseEvent.OnHits.Add(0, new StatusBattleEvent("decoy", true, false));
            }
            else if (ii == 117)
            {
                item.Name = new LocalText("Last-Chance Seed");
                item.Desc = new LocalText("A seed that sets the PP of the Pokémon's moves to 1, and drastically raises its Attack, Special Attack, and Speed.");
                item.Sprite = "Seed_Red";
                item.UseEvent.OnHits.Add(0, new PPTo1Event(true));
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, 3));
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 3));
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, 3));
            }
            else if (ii == 118)
            {
                item.Name = new LocalText("Ban Seed");
                item.Desc = new LocalText("A seed that bans the last move used by the Pokémon, preventing anyone in the dungeon from using it.");
                item.Sprite = "Seed_Blue";
                item.UseEvent.OnHits.Add(0, new BanMoveEvent("move_ban", "last_used_move"));
            }
            else if (ii == 119)
            {

            }
            else if (ii == 150)
            {
                item.Name = new LocalText("Nectar");
                item.Desc = new LocalText("A drink that permanently raises the Pokémon's stats.");
                item.Sprite = "Bottle_Gold";
                item.UseEvent.OnHits.Add(0, new VitaminEvent(Stat.None, 1));
            }
            else if (ii == 151)
            {
                item.Name = new LocalText("Protein");
                item.Desc = new LocalText("A drink that permanently raises the Pokémon's Attack.");
                item.Sprite = "Medicine_Red";
                item.UseEvent.OnHits.Add(0, new VitaminEvent(Stat.Attack, 4));
            }
            else if (ii == 152)
            {
                item.Name = new LocalText("Iron");
                item.Desc = new LocalText("A drink that permanently raises the Pokémon's Defense.");
                item.Sprite = "Medicine_Yellow";
                item.UseEvent.OnHits.Add(0, new VitaminEvent(Stat.Defense, 4));
            }
            else if (ii == 153)
            {
                item.Name = new LocalText("Calcium");
                item.Desc = new LocalText("A drink that permanently raises the Pokémon's Special Attack.");
                item.Sprite = "Medicine_Red";
                item.UseEvent.OnHits.Add(0, new VitaminEvent(Stat.MAtk, 4));
            }
            else if (ii == 154)
            {
                item.Name = new LocalText("Zinc");
                item.Desc = new LocalText("A drink that permanently raises the Pokémon's Special Defense.");
                item.Sprite = "Medicine_Yellow";
                item.UseEvent.OnHits.Add(0, new VitaminEvent(Stat.MDef, 4));
            }
            else if (ii == 155)
            {
                item.Name = new LocalText("Carbos");
                item.Desc = new LocalText("A drink that permanently raises the Pokémon's Speed.");
                item.Sprite = "Medicine_Orange";
                item.UseEvent.OnHits.Add(0, new VitaminEvent(Stat.Speed, 4));
            }
            else if (ii == 156)
            {
                item.Name = new LocalText("HP Up");
                item.Desc = new LocalText("A drink that permanently raises the Pokémon's Maximum HP.");
                item.Sprite = "Medicine_Orange";
                item.UseEvent.OnHits.Add(0, new VitaminEvent(Stat.HP, 4));
            }
            else if (ii == 157)
            {

            }
            else if (ii == 158)
            {
                item.Name = new LocalText("Amber Tear");
                fileName = "medicine_" + Text.Sanitize(item.Name.DefaultText).ToLower();
                item.Desc = new LocalText("An amber liquid that sparkles like crystal-clear tears, rumored to be the most precious of even the rarest treasures. It raises chances of recruiting other Pokémon to the team on the floor it's used.");
                item.Sprite = "Bottle_Gold";
                item.Price = 2500;
                item.MaxStack = 3;
                item.UseEvent.OnHits.Add(0, new StatusBattleEvent("recruit_boost", true, false));
            }
            else if (ii == 159)
            {

            }
            else if (ii == 160)
            {
                item.Name = new LocalText("Potion");
                item.Desc = new LocalText("A spray-type medicine that somewhat restores HP. It affects all team members up to 5 tiles away.");
                item.Sprite = "Medicine_Green";
                item.UseEvent.OnHits.Add(0, new RestoreHPEvent(1, 2, true));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 6;
                ((AreaAction)item.UseAction).LagBehindTime = 24;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Event_Gather_Sparkle", 5));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 4;
                emitter.Bursts = 5;
                emitter.StartDistance = 4;
                ((AreaAction)item.UseAction).Emitter = emitter;
                item.UseAction.ActionFX.Sound = "DUN_Aromatherapy";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
                item.Price = 300;
            }
            else if (ii == 161)
            {
                item.Name = new LocalText("Max Potion");
                item.Desc = new LocalText("A spray-type medicine that fully restores HP. It affects all team members up to 3 tiles away.");
                item.Sprite = "Medicine_Purple";
                item.UseEvent.OnHits.Add(0, new RestoreHPEvent(1, 1, true));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 3;
                ((AreaAction)item.UseAction).Speed = 6;
                ((AreaAction)item.UseAction).LagBehindTime = 24;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Event_Gather_Sparkle", 5));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 4;
                emitter.Bursts = 5;
                emitter.StartDistance = 4;
                ((AreaAction)item.UseAction).Emitter = emitter;
                item.UseAction.ActionFX.Sound = "DUN_Aromatherapy";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
                item.Price = 700;
            }
            else if (ii == 162)
            {

            }
            else if (ii == 163)
            {

            }
            else if (ii == 164)
            {
                item.Name = new LocalText("Elixir");
                item.Desc = new LocalText("A spray-type medicine that somewhat restores PP. It affects all team members up to 5 tiles away.");
                item.Sprite = "Medicine_Green";
                item.UseEvent.OnHits.Add(0, new RestorePPEvent(10));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 6;
                ((AreaAction)item.UseAction).LagBehindTime = 24;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Event_Gather_Sparkle", 5));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 4;
                emitter.Bursts = 5;
                emitter.StartDistance = 4;
                ((AreaAction)item.UseAction).Emitter = emitter;
                item.UseAction.ActionFX.Sound = "DUN_Aromatherapy";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
                item.Price = 300;
            }
            else if (ii == 165)
            {

            }
            else if (ii == 166)
            {
                item.Name = new LocalText("Max Elixir");
                item.Desc = new LocalText("A spray-type medicine that completely restores PP. It affects all team members up to 3 tiles away.");
                item.Sprite = "Medicine_Purple";
                item.UseEvent.OnHits.Add(0, new RestorePPEvent(100));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 3;
                ((AreaAction)item.UseAction).Speed = 6;
                ((AreaAction)item.UseAction).LagBehindTime = 24;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Event_Gather_Sparkle", 5));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 4;
                emitter.Bursts = 5;
                emitter.StartDistance = 4;
                ((AreaAction)item.UseAction).Emitter = emitter;
                item.UseAction.ActionFX.Sound = "DUN_Aromatherapy";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
                item.Price = 700;
            }
            else if (ii == 167)
            {
            }
            else if (ii == 168)
            {

            }
            else if (ii == 169)
            {

            }
            else if (ii == 170)
            {

            }
            else if (ii == 171)
            {

            }
            else if (ii == 172)
            {

            }
            else if (ii == 173)
            {
                item.Name = new LocalText("Full Heal");
                item.Desc = new LocalText("A spray-type medicine that cures status problems. It affects all team members up to 5 tiles away.");
                item.Sprite = "Medicine_Green";
                item.ItemStates.Set(new CurerState());
                item.UseEvent.BeforeActions.Add(-1, new AddContextStateEvent(new CureAttack()));
                item.UseEvent.OnHits.Add(0, new RemoveStateStatusBattleEvent(typeof(BadStatusState), true, new StringKey("MSG_CURE_ALL")));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 6;
                ((AreaAction)item.UseAction).LagBehindTime = 24;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Event_Gather_Sparkle", 5));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 4;
                emitter.Bursts = 5;
                emitter.StartDistance = 4;
                ((AreaAction)item.UseAction).Emitter = emitter;
                item.UseAction.ActionFX.Sound = "DUN_Aromatherapy";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
                item.Price = 500;
            }
            else if (ii == 174)
            {
                item.Name = new LocalText("**Full Restore");
            }
            else if (ii == 175)
            {
                item.Name = new LocalText("X Attack");
                item.Desc = new LocalText("An item that sharply raises Attack. It affects all team members up to 5 tiles away.");
                item.Sprite = "Medicine_Red";
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_attack", true, false, 2));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 6;
                ((AreaAction)item.UseAction).LagBehindTime = 24;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Event_Gather_Sparkle", 5));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 4;
                emitter.Bursts = 5;
                emitter.StartDistance = 4;
                ((AreaAction)item.UseAction).Emitter = emitter;
                item.UseAction.ActionFX.Sound = "DUN_Aromatherapy";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
                item.Price = 300;
            }
            else if (ii == 176)
            {
                item.Name = new LocalText("X Defense");
                item.Desc = new LocalText("An item that sharply raises Defense. It affects all team members up to 5 tiles away.");
                item.Sprite = "Medicine_Yellow";
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_defense", true, false, 2));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 6;
                ((AreaAction)item.UseAction).LagBehindTime = 24;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Event_Gather_Sparkle", 5));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 4;
                emitter.Bursts = 5;
                emitter.StartDistance = 4;
                ((AreaAction)item.UseAction).Emitter = emitter;
                item.UseAction.ActionFX.Sound = "DUN_Aromatherapy";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
                item.Price = 300;
            }
            else if (ii == 177)
            {
                item.Name = new LocalText("X Sp. Atk");
                item.Desc = new LocalText("An item that sharply raises Special Attack. It affects all team members up to 5 tiles away.");
                item.Sprite = "Medicine_Red";
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_special_attack", true, false, 2));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 6;
                ((AreaAction)item.UseAction).LagBehindTime = 24;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Event_Gather_Sparkle", 5));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 4;
                emitter.Bursts = 5;
                emitter.StartDistance = 4;
                ((AreaAction)item.UseAction).Emitter = emitter;
                item.UseAction.ActionFX.Sound = "DUN_Aromatherapy";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
                item.Price = 300;
            }
            else if (ii == 178)
            {
                item.Name = new LocalText("X Sp. Def");
                item.Desc = new LocalText("An item that sharply raises Special Defense. It affects all team members up to 5 tiles away.");
                item.Sprite = "Medicine_Yellow";
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_special_defense", true, false, 2));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 6;
                ((AreaAction)item.UseAction).LagBehindTime = 24;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Event_Gather_Sparkle", 5));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 4;
                emitter.Bursts = 5;
                emitter.StartDistance = 4;
                ((AreaAction)item.UseAction).Emitter = emitter;
                item.UseAction.ActionFX.Sound = "DUN_Aromatherapy";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
                item.Price = 300;
            }
            else if (ii == 179)
            {
                item.Name = new LocalText("X Speed");
                item.Desc = new LocalText("An item that boosts Movement Speed by 1 stage. It affects all team members up to 5 tiles away.");
                item.Sprite = "Medicine_Orange";
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, 2));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 6;
                ((AreaAction)item.UseAction).LagBehindTime = 24;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Event_Gather_Sparkle", 5));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 4;
                emitter.Bursts = 5;
                emitter.StartDistance = 4;
                ((AreaAction)item.UseAction).Emitter = emitter;
                item.UseAction.ActionFX.Sound = "DUN_Aromatherapy";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
                item.Price = 300;
            }
            else if (ii == 180)
            {
                item.Name = new LocalText("X Accuracy");
                item.Desc = new LocalText("An item that sharply raises Accuracy. It affects all team members up to 5 tiles away.");
                item.Sprite = "Medicine_Orange";
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_accuracy", true, false, 2));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 6;
                ((AreaAction)item.UseAction).LagBehindTime = 24;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Event_Gather_Sparkle", 5));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 4;
                emitter.Bursts = 5;
                emitter.StartDistance = 4;
                ((AreaAction)item.UseAction).Emitter = emitter;
                item.UseAction.ActionFX.Sound = "DUN_Aromatherapy";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
                item.Price = 300;
            }
            else if (ii == 181)
            {
                item.Name = new LocalText("Dire Hit");
                item.Desc = new LocalText("An item that raises critical-hit ratio. It affects all team members up to 5 tiles away.");
                item.Sprite = "Medicine_Orange";
                item.UseEvent.OnHits.Add(0, new StatusBattleEvent("focus_energy", true, false));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 6;
                ((AreaAction)item.UseAction).LagBehindTime = 24;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Event_Gather_Sparkle", 5));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 4;
                emitter.Bursts = 5;
                emitter.StartDistance = 4;
                ((AreaAction)item.UseAction).Emitter = emitter;
                item.UseAction.ActionFX.Sound = "DUN_Aromatherapy";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
                item.Price = 300;
            }
            else if (ii == 182)
            {
                item.Name = new LocalText("**Guard Spec.");
            }
            else if (ii == 183)
            {
                item.Name = new LocalText("Mental Herb");
                item.Desc = new LocalText("An herb that removes all move-binding effects, and prevents moves from being sealed.");
                item.Sprite = "Herb_Blue";
                item.Price = 100;
                item.UseEvent.OnHits.Add(0, new RemoveStatusBattleEvent("taunted", true));
                item.UseEvent.OnHits.Add(0, new RemoveStatusBattleEvent("encore", true));
                item.UseEvent.OnHits.Add(0, new RemoveStatusBattleEvent("torment", true));
                item.UseEvent.OnHits.Add(0, new RemoveStatusBattleEvent("disable", true));
                item.UseEvent.OnHits.Add(0, new StatusBattleEvent("mental_charged", true, false));
            }
            else if (ii == 184)
            {
                item.Name = new LocalText("Power Herb");
                item.Desc = new LocalText("An herb that allows the use of two-turn attacks in one turn, and protects against PP-draining effects.");
                item.Sprite = "Herb_Red";
                item.Price = 100;
                item.UseEvent.OnHits.Add(0, new StatusBattleEvent("power_charged", true, false));
            }
            else if (ii == 185)
            {
                item.Name = new LocalText("White Herb");
                item.Desc = new LocalText("An herb that removes all stat changes, and prevents them from changing.");
                item.Sprite = "Herb_White";
                item.Price = 50;
                item.UseEvent.OnHits.Add(0, new RemoveStatusStackBattleEvent("mod_speed", true, true, true));
                item.UseEvent.OnHits.Add(0, new RemoveStatusStackBattleEvent("mod_attack", true, true, true));
                item.UseEvent.OnHits.Add(0, new RemoveStatusStackBattleEvent("mod_defense", true, true, true));
                item.UseEvent.OnHits.Add(0, new RemoveStatusStackBattleEvent("mod_special_attack", true, true, true));
                item.UseEvent.OnHits.Add(0, new RemoveStatusStackBattleEvent("mod_special_defense", true, true, true));
                item.UseEvent.OnHits.Add(0, new RemoveStatusStackBattleEvent("mod_accuracy", true, true, true));
                item.UseEvent.OnHits.Add(0, new RemoveStatusStackBattleEvent("mod_evasion", true, true, true));
                item.UseEvent.OnHits.Add(0, new StatusBattleEvent("stat_charged", true, false));
            }
            else if (ii == 186)
            {
            }
            else if (ii == 200)
            {
                item.Name = new LocalText("Stick");
                item.Desc = new LocalText("A weapon to be hurled. It flies in a straight line to inflict damage on any Pokémon it hits. It causes the target to flinch.");
                item.Sprite = "Stick_Brown";
                item.Icon = 1;
                item.ThrowAnim = new AnimData("Thorn_Brown", 60);
                item.Price = 1;
                item.UseEvent.Category = BattleData.SkillCategory.Physical;
                item.UseEvent.SkillStates.Set(new BasePowerState(50));
                item.UseEvent.SkillStates.Set(new AdditionalEffectState(100));
                item.UseEvent.OnHits.Add(-1, new DamageFormulaEvent());
                item.UseEvent.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
            }
            else if (ii == 201)
            {
                item.Name = new LocalText("Cacnea Spike");
                item.Desc = new LocalText("A weapon to be hurled. It flies in a straight line to inflict damage and sharply lowers the Attack of any Pokémon it hits.");
                item.Sprite = "Stick_Green";
                item.Icon = 1;
                item.ThrowAnim = new AnimData("Thorn_Green", 60);
                item.Price = 2;
                item.UseEvent.Category = BattleData.SkillCategory.Physical;
                item.UseEvent.SkillStates.Set(new BasePowerState(50));
                item.UseEvent.SkillStates.Set(new AdditionalEffectState(100));
                item.UseEvent.OnHits.Add(-1, new DamageFormulaEvent());
                item.UseEvent.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_attack", true, true, -2)));
            }
            else if (ii == 202)
            {
                item.Name = new LocalText("Corsola Twig");
                item.Desc = new LocalText("A weapon to be hurled. It flies in a straight line to inflict damage and sharply lowers the Sp.Atk of any Pokémon it hits.");
                item.Sprite = "Stick_Pink";
                item.Icon = 1;
                item.ThrowAnim = new AnimData("Thorn_Pink", 60);
                item.Price = 2;
                item.UseEvent.Category = BattleData.SkillCategory.Physical;
                item.UseEvent.SkillStates.Set(new BasePowerState(50));
                item.UseEvent.SkillStates.Set(new AdditionalEffectState(100));
                item.UseEvent.OnHits.Add(-1, new DamageFormulaEvent());
                item.UseEvent.OnHits.Add(0, new AdditionalEvent(new StatusStackBattleEvent("mod_special_attack", true, true, -2)));
            }
            else if (ii == 203)
            {
                item.Name = new LocalText("Iron Thorn");
                item.Desc = new LocalText("A weapon to be hurled. It flies in a straight line to inflict damage on any Pokémon it hits. Critical hits land more easily.");
                item.Sprite = "Stick_DarkBlue";
                item.Icon = 1;
                item.ThrowAnim = new AnimData("Thorn_Gray", 60);
                item.Price = 2;
                item.UseEvent.Category = BattleData.SkillCategory.Physical;
                item.UseEvent.SkillStates.Set(new BasePowerState(70));
                item.UseEvent.OnActions.Add(0, new BoostCriticalEvent(2));
                item.UseEvent.OnHits.Add(-1, new DamageFormulaEvent());
            }
            else if (ii == 204)
            {
                item.Name = new LocalText("Silver Spike");
                item.Desc = new LocalText("A weapon to be hurled. It flies in a straight line to inflict damage to any Pokémon it hits, passing through walls.");
                item.Sprite = "Stick_White";
                item.Icon = 1;
                item.ThrowAnim = new AnimData("Thorn_White", 60);
                item.Price = 11;
                item.UseEvent.Category = BattleData.SkillCategory.Physical;
                item.UseEvent.SkillStates.Set(new BasePowerState(50));
                item.UseEvent.OnActions.Add(0, new PierceEvent(false, true, true, true));
                item.UseEvent.OnHits.Add(-1, new DamageFormulaEvent());
            }
            else if (ii == 205)
            {
                item.Name = new LocalText("Golden Thorn");
                item.Desc = new LocalText("A brilliantly shining thorn crafted from solid gold. It is a fantastically precious item that gives its owner serious bragging rights. When hurled, it flies in a straight line to inflict damage on any Pokémon it hits, scattering coins everywhere.");
                item.Sprite = "Thorn_Yellow";
                item.Icon = 1;
                item.ThrowAnim = new AnimData("Thorn_Gold", 60);
                item.Price = 111;
                item.UseEvent.Category = BattleData.SkillCategory.Physical;
                item.UseEvent.SkillStates.Set(new BasePowerState(50));
                item.UseEvent.OnActions.Add(0, new PierceEvent(false, true, true, false));
                item.UseEvent.OnHits.Add(-1, new DamageFormulaEvent());
                item.UseEvent.AfterActions.Add(0, new DamageMoneyEvent(10));
            }
            else if (ii == 206)
            {
                item.Name = new LocalText("Rare Fossil");
                item.Desc = new LocalText("A weapon to be thrown. It flies high in an arc to clear obstacles and strike the target's area. It deals Special damage.");
                item.Sprite = "Stone_Brown";
                item.Icon = 0;
                item.Price = 50;
                item.ArcThrow = true;
                item.BreakOnThrow = true;
                item.UseEvent.Category = BattleData.SkillCategory.Magical;
                item.UseEvent.SkillStates.Set(new BasePowerState(50));
                item.UseEvent.OnHits.Add(-1, new DamageFormulaEvent());

                item.Explosion = new ExplosionData();
                item.Explosion.Range = 1;
                item.Explosion.HitTiles = true;
                item.Explosion.Speed = 10;
                item.Explosion.ExplodeFX.Sound = "DUN_Rollout";
                item.Explosion.TargetAlignments |= Alignment.Foe;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Rock_Pieces", 3, 0, 0));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 2;
                emitter.Bursts = 4;
                emitter.StartDistance = 8;
                item.Explosion.Emitter = emitter;
            }
            else if (ii == 207)
            {
                item.Name = new LocalText("Geo Pebble");
                item.Desc = new LocalText("A weapon to be thrown. It flies high in an arc to clear obstacles and strike the target. It causes the target to flinch.");
                item.Sprite = "Rock_Gray";
                item.Icon = 0;
                item.Price = 1;
                item.ArcThrow = true;
                item.UseEvent.Category = BattleData.SkillCategory.Physical;
                item.UseEvent.SkillStates.Set(new BasePowerState(50));
                item.UseEvent.SkillStates.Set(new AdditionalEffectState(100));
                item.UseEvent.OnHits.Add(-1, new DamageFormulaEvent());
                item.UseEvent.OnHits.Add(0, new AdditionalEvent(new StatusBattleEvent("flinch", true, true)));
                item.Explosion = new ExplosionData();
                item.Explosion.TargetAlignments |= Alignment.Foe;
            }
            else if (ii == 208)
            {
                item.Name = new LocalText("Gravelerock");
                item.Desc = new LocalText("A weapon to be thrown. It flies high in an arc to clear obstacles and strike the target's area.");
                item.Sprite = "Rock_Brown";
                item.Icon = 0;
                item.Price = 3;
                item.ArcThrow = true;
                item.BreakOnThrow = true;
                item.UseEvent.Category = BattleData.SkillCategory.Physical;
                item.UseEvent.SkillStates.Set(new BasePowerState(50));
                item.UseEvent.OnHits.Add(-1, new DamageFormulaEvent());

                item.Explosion = new ExplosionData();
                item.Explosion.Range = 1;
                item.Explosion.HitTiles = true;
                item.Explosion.Speed = 10;
                item.Explosion.ExplodeFX.Sound = "DUN_Giga_Impact";
                item.Explosion.TargetAlignments |= Alignment.Foe;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Rock_Pieces", 3, 0, 0));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 2;
                emitter.Bursts = 4;
                emitter.StartDistance = 8;
                item.Explosion.Emitter = emitter;
            }
            else if (ii == 209)
            {
                item.Name = new LocalText("Big Apricorn");
                item.Desc = new LocalText("An apricorn with a pleasant scent. It can tossed at wild Pokémon as a gift for a high chance to recruit them into the team.");
                item.Sprite = "Apricorn_White";
                item.Price = 400;
                item.UseEvent.OnHits.Add(0, new FlatRecruitmentEvent(30));
                item.UseEvent.OnHits.Add(0, new RecruitmentEvent(new BattleScriptEvent("AllyInteract")));
            }
            else if (ii == 210)
            {
                item.Name = new LocalText("Plain Apricorn");
                item.Desc = new LocalText("An apricorn with a pleasant scent. It can tossed at wild Pokémon of any type, as a gift to recruit them into the team.");
                item.Sprite = "Apricorn_White";
                item.Price = 250;
                item.UseEvent.OnHits.Add(0, new FlatRecruitmentEvent(0));
                item.UseEvent.OnHits.Add(0, new RecruitmentEvent(new BattleScriptEvent("AllyInteract")));
            }
            else if (ii == 211)
            {
                item.Name = new LocalText("Blue Apricorn");
                item.Desc = new LocalText("An apricorn that can be tossed at Water- or Ice-type Pokémon as a gift for a high chance to recruit them into the team.");
                item.Sprite = "Apricorn_Blue";
                item.Price = 250;
                HashSet<string> elements = new HashSet<string>();
                elements.Add("water");
                elements.Add("ice");
                item.UseEvent.OnHits.Add(0, new TypeRecruitmentEvent(elements));
                item.UseEvent.OnHits.Add(0, new RecruitmentEvent(new BattleScriptEvent("AllyInteract")));
            }
            else if (ii == 212)
            {
                item.Name = new LocalText("Green Apricorn");
                item.Desc = new LocalText("An apricorn that can be tossed at Grass- or Bug-type Pokémon as a gift for a high chance to recruit them into the team.");
                item.Sprite = "Apricorn_Green";
                item.Price = 250;
                HashSet<string> elements = new HashSet<string>();
                elements.Add("grass");
                elements.Add("bug");
                item.UseEvent.OnHits.Add(0, new TypeRecruitmentEvent(elements));
                item.UseEvent.OnHits.Add(0, new RecruitmentEvent(new BattleScriptEvent("AllyInteract")));
            }
            else if (ii == 213)
            {
                item.Name = new LocalText("Brown Apricorn");
                item.Desc = new LocalText("An apricorn that can be tossed at Fighting-, Rock-, or Ground-type Pokémon as a gift for a high chance to recruit them into the team.");
                item.Sprite = "Apricorn_Brown";
                item.Price = 250;
                HashSet<string> elements = new HashSet<string>();
                elements.Add("fighting");
                elements.Add("rock");
                elements.Add("ground");
                item.UseEvent.OnHits.Add(0, new TypeRecruitmentEvent(elements));
                item.UseEvent.OnHits.Add(0, new RecruitmentEvent(new BattleScriptEvent("AllyInteract")));
            }
            else if (ii == 214)
            {
                item.Name = new LocalText("Purple Apricorn");
                item.Desc = new LocalText("An apricorn that can be tossed at Poison- or Psychic-type Pokémon as a gift for a high chance to recruit them into the team.");
                item.Sprite = "Apricorn_Purple";
                item.Price = 250;
                HashSet<string> elements = new HashSet<string>();
                elements.Add("poison");
                elements.Add("psychic");
                item.UseEvent.OnHits.Add(0, new TypeRecruitmentEvent(elements));
                item.UseEvent.OnHits.Add(0, new RecruitmentEvent(new BattleScriptEvent("AllyInteract")));
            }
            else if (ii == 215)
            {
                item.Name = new LocalText("Red Apricorn");
                item.Desc = new LocalText("An apricorn that can be tossed at Fire- or Dragon-type Pokémon as a gift for a high chance to recruit them into the team.");
                item.Sprite = "Apricorn_Red";
                item.Price = 250;
                HashSet<string> elements = new HashSet<string>();
                elements.Add("fire");
                elements.Add("dragon");
                item.UseEvent.OnHits.Add(0, new TypeRecruitmentEvent(elements));
                item.UseEvent.OnHits.Add(0, new RecruitmentEvent(new BattleScriptEvent("AllyInteract")));
            }
            else if (ii == 216)
            {
                item.Name = new LocalText("White Apricorn");
                item.Desc = new LocalText("An apricorn that can be tossed at Normal-, Flying-, or Fairy-type Pokémon as a gift for a high chance to recruit them into the team.");
                item.Sprite = "Apricorn_White";
                item.Price = 250;
                HashSet<string> elements = new HashSet<string>();
                elements.Add("normal");
                elements.Add("flying");
                elements.Add("fairy");
                item.UseEvent.OnHits.Add(0, new TypeRecruitmentEvent(elements));
                item.UseEvent.OnHits.Add(0, new RecruitmentEvent(new BattleScriptEvent("AllyInteract")));
            }
            else if (ii == 217)
            {
                item.Name = new LocalText("Yellow Apricorn");
                item.Desc = new LocalText("An apricorn that can be tossed at Electric- or Steel-type Pokémon as a gift for a high chance to recruit them into the team.");
                item.Sprite = "Apricorn_Yellow";
                item.Price = 250;
                HashSet<string> elements = new HashSet<string>();
                elements.Add("electric");
                elements.Add("steel");
                item.UseEvent.OnHits.Add(0, new TypeRecruitmentEvent(elements));
                item.UseEvent.OnHits.Add(0, new RecruitmentEvent(new BattleScriptEvent("AllyInteract")));
            }
            else if (ii == 218)
            {
                item.Name = new LocalText("Black Apricorn");
                item.Desc = new LocalText("An apricorn that can be tossed at Ghost- or Dark-type Pokémon as a gift for a high chance to recruit them into the team.");
                item.Sprite = "Apricorn_Black";
                item.Price = 250;
                HashSet<string> elements = new HashSet<string>();
                elements.Add("ghost");
                elements.Add("dark");
                item.UseEvent.OnHits.Add(0, new TypeRecruitmentEvent(elements));
                item.UseEvent.OnHits.Add(0, new RecruitmentEvent(new BattleScriptEvent("AllyInteract")));
            }
            else if (ii == 219)
            {
                item.Name = new LocalText("Perfect Apricorn");
                item.Desc = new LocalText("A beautiful apricorn with an indescribable scent. It can be tossed at wild Pokémon as a gift to recruit them into the team without any hesitation.");
                item.Sprite = "Apricorn_White";
                item.Price = 50000;
                item.UseEvent.OnHits.Add(0, new FlatRecruitmentEvent(1000));
                item.UseEvent.OnHits.Add(0, new RecruitmentEvent(new BattleScriptEvent("AllyInteract")));
            }
            else if (ii == 220)
            {
                item.Name = new LocalText("Glittery Apricorn");
                item.Desc = new LocalText("An apricorn that can be tossed at shiny Pokémon as a gift for a high chance to recruit them into the team.");
                item.Sprite = "Apricorn_Black";
                item.Price = 500;
                item.UseEvent.OnHits.Add(0, new SkinRecruitmentEvent());
                item.UseEvent.OnHits.Add(0, new RecruitmentEvent(new BattleScriptEvent("AllyInteract")));
            }
            else if (ii == 221)
            {
                item.Name = new LocalText("Pounce Wand");
                item.Desc = new LocalText("A wand to be waved at a Pokémon or wall. It causes the user to jump to that location.");
                item.Sprite = "Wand_Green";
                item.Price = 5;
                item.UseAction = new ProjectileAction();
                ((ProjectileAction)item.UseAction).CharAnimData = new CharAnimFrameType(42);//Rotate
                ((ProjectileAction)item.UseAction).Range = CharAction.MAX_RANGE;
                ((ProjectileAction)item.UseAction).Speed = 12;
                ((ProjectileAction)item.UseAction).StopAtHit = true;
                ((ProjectileAction)item.UseAction).StopAtWall = true;
                ((ProjectileAction)item.UseAction).Anim = new AnimData("Confuse_Ray", 2);
                item.UseAction.TargetAlignments = (Alignment.Foe | Alignment.Friend);
                item.Explosion.TargetAlignments = (Alignment.Foe | Alignment.Friend);
                item.Explosion.HitTiles = true;
                BattleFX itemFX = new BattleFX();
                itemFX.Sound = "DUN_Throw_Start";
                item.UseAction.PreActions.Add(itemFX);
                item.UseAction.ActionFX.Sound = "DUN_Blowback_Orb";
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2));
                item.UseEvent.OnHitTiles.Add(0, new PounceEvent(1));
            }
            else if (ii == 222)
            {
                item.Name = new LocalText("Whirlwind Wand");
                item.Desc = new LocalText("A wand to be waved at a Pokémon. It knocks the target back.");
                item.Sprite = "Wand_White";
                item.Price = 5;
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2));
                item.UseEvent.OnHits.Add(0, new KnockBackEvent(8));
            }
            else if (ii == 223)
            {
                item.Name = new LocalText("Switcher Wand");
                item.Desc = new LocalText("A wand to be waved at a Pokémon. It causes the user to switch places with the target.");
                item.Sprite = "Wand_Red";
                item.Price = 5;
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2));
                item.UseEvent.HitFX.Sound = "DUN_Switcher";
                item.UseEvent.OnHits.Add(0, new SwitcherEvent());
            }
            else if (ii == 224)
            {
                item.Name = new LocalText("**Surround Wand");
                item.Desc = new LocalText("A wand to be waved at a Pokémon. It causes the user's allies to warp to the target.");
            }
            else if (ii == 225)
            {
                item.Name = new LocalText("Lure Wand");
                item.Desc = new LocalText("A wand to be waved at a Pokémon. It causes the target to warp in front of the user.");
                item.Sprite = "Wand_Pink";
                item.Price = 5;
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2));
                item.UseEvent.OnHits.Add(0, new LureEvent());
            }
            else if (ii == 226)
            {
                item.Name = new LocalText("Slow Wand");
                item.Desc = new LocalText("A wand to be waved at a Pokémon. It reduces the Movement Speed of the target by 1 level.");
                item.Sprite = "Wand_Blue";
                item.Price = 5;
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2));
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, -1));
            }
            else if (ii == 227)
            {
                item.Name = new LocalText("**Stayaway Wand");
                item.Desc = new LocalText("A wand to be waved at enemy Pokémon. It warps the target to the nearest stairway, and puts it into a deep sleep.");
            }
            else if (ii == 228)
            {
                item.Name = new LocalText("Fear Wand");
                item.Desc = new LocalText("A wand to be waved at a Pokémon. It causes the target to flinch.");
                item.Sprite = "Wand_Orange";
                item.Price = 5;
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2));
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(4));
                item.UseEvent.OnHits.Add(0, new StatusStateBattleEvent("flinch", true, false, statusStates));
            }
            else if (ii == 229)
            {
                item.Name = new LocalText("**Totter Wand");
                item.Desc = new LocalText("A wand to be waved at a Pokémon. It causes the target to become briefly confused.");
            }
            else if (ii == 230)
            {
                item.Name = new LocalText("**Infatuation Wand");
                item.Desc = new LocalText("A wand to be waved at Pokémon of the opposite gender. It causes the target to become attracted to the user.");
            }
            else if (ii == 230)
            {
                item.Name = new LocalText("**Exchange Wand");
                item.Desc = new LocalText("A wand to be waved at a Pokémon. It swaps all minor status effects between the user and the target.");
            }
            else if (ii == 231)
            {
                item.Name = new LocalText("Topsy-Turvy Wand");
                item.Desc = new LocalText("A wand to be waved at a Pokémon. It reverses all stat changes on the target.");
                item.Sprite = "Wand_Blue";
                item.Price = 5;
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 2));
                item.UseEvent.HitFX.Sound = "DUN_U-Turn_2";
                item.UseEvent.OnHits.Add(0, new ReverseStateStatusBattleEvent(typeof(StatChangeState), true, new StringKey("MSG_BUFF_REVERSE")));
            }
            else if (ii == 232)
            {
                item.Name = new LocalText("Warp Wand");
                item.Desc = new LocalText("A wand to be waved at a Pokémon. It warps the target to a different place on the floor.");
                item.Sprite = "Wand_Pink";
                item.Price = 5;
                item.UseEvent.OnHits.Add(0, new RandomWarpEvent(50, true));
            }
            else if (ii == 233)
            {
                item.Name = new LocalText("Purge Wand");
                item.Desc = new LocalText("A wand to be waved at a Pokémon. It removes all stat boosts and damages the target for each stat boosted. Stat drops will heal the target instead.");
                item.Sprite = "Wand_Blue";
                item.Price = 5;
                item.Explosion.Range = 1;
                item.Explosion.Speed = 8;
                CircleSquareAreaEmitter emitter = new CircleSquareAreaEmitter(new AnimData("Circle_Small_Green_Out", 2));
                emitter.ParticlesPerTile = 1;
                item.Explosion.Emitter = emitter;
                item.Explosion.ExplodeFX.Sound = "DUN_Ice_Beam";
                item.UseEvent.OnHits.Add(0, new AdNihiloEvent(typeof(StatChangeState), 12, true));
                item.UseEvent.OnHits.Add(0, new RemoveStateStatusBattleEvent(typeof(StatChangeState), true, new StringKey("MSG_BUFF_REMOVE")));
            }
            else if (ii == 234)
            {
                item.Name = new LocalText("Lob Wand");
                item.Desc = new LocalText("A wand to be waved at a Pokémon. It inflicts damage on any Pokémon in a straight line, passing through walls.");
                item.Sprite = "Wand_White";
                item.Price = 5;
                item.UseEvent.Category = BattleData.SkillCategory.Magical;
                item.UseEvent.SkillStates.Set(new BasePowerState(70));
                item.UseEvent.OnActions.Add(0, new PierceEvent(true, true, false, true));
                item.UseEvent.OnHits.Add(-1, new DamageFormulaEvent());
            }
            else if (ii == 235)
            {
                item.Name = new LocalText("Transfer Wand");
                item.Desc = new LocalText("A wand to be waved at a Pokémon. It transforms the target into the same Pokémon as the user.");
                item.Sprite = "Wand_Pink";
                item.Price = 10;
                item.UseEvent.OnHits.Add(0, new TransformEvent(true, "transformed", 5));
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Puff_Green", 3));
                item.UseEvent.HitFX.Sound = "DUN_Transform";
            }
            else if (ii == 236)
            {
                item.Name = new LocalText("Vanish Wand");
                item.Desc = new LocalText("A wand to be waved at a Pokémon. It temporarily makes the target invisible.");
                item.Sprite = "Wand_Purple";
                item.Price = 5;
                item.UseEvent.OnHits.Add(0, new StatusBattleEvent("invisible", true, false));
                item.UseEvent.HitFX.Sound = "DUN_Invisible";
            }
            else if (ii == 249)
            {
                item.Name = new LocalText("Path Wand");
                item.Desc = new LocalText("A wand to be waved at terrain features. It clears obstacles to form a path directly in front of the user.");
                item.Sprite = "Wand_Purple";
                item.Price = 5;
                item.UseAction = new ProjectileAction();
                ((ProjectileAction)item.UseAction).CharAnimData = new CharAnimFrameType(42);//Rotate
                ((ProjectileAction)item.UseAction).Range = 8;
                ((ProjectileAction)item.UseAction).Speed = 12;
                ((ProjectileAction)item.UseAction).HitTiles = true;
                ((ProjectileAction)item.UseAction).Anim = new AnimData("Confuse_Ray", 2);
                item.UseAction.TargetAlignments = (Alignment.Foe | Alignment.Friend);
                item.Explosion.TargetAlignments = (Alignment.Foe | Alignment.Friend);
                BattleFX itemFX = new BattleFX();
                itemFX.Sound = "DUN_Throw_Start";
                item.UseAction.PreActions.Add(itemFX);
                item.UseAction.ActionFX.Sound = "DUN_Blowback_Orb";
                item.UseEvent.OnHitTiles.Add(0, new RemoveTrapEvent());
                item.UseEvent.OnHitTiles.Add(0, new RemoveTerrainStateEvent("DUN_Transform", new SingleEmitter(new AnimData("Puff_Brown", 3)),
                    new FlagType(typeof(WallTerrainState)), new FlagType(typeof(WaterTerrainState)), new FlagType(typeof(LavaTerrainState)), new FlagType(typeof(AbyssTerrainState)), new FlagType(typeof(FoliageTerrainState))));
            }
            else if (ii == 250)
            {
                item.Name = new LocalText("Escape Orb");
                item.Desc = new LocalText("An orb that allows the team to instantly warp out of the dungeon.");
                item.Sprite = "Orb_Blue";
                item.UseEvent.OnHits.Add(0, new ExitDungeonEvent());
                item.UseAction = new SelfAction();
                item.UseAction.ActionFX.Sound = "DUN_Natural_Gift";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
            }
            else if (ii == 251)
            {
                item.Name = new LocalText("Weather Orb");
                item.Desc = new LocalText("A climate-control orb that changes the dungeon floor's condition depending on the user's type.");
                item.Sprite = "Orb_LightBlue";

                Dictionary<string, string> weatherPair = new Dictionary<string, string>();
                weatherPair.Add("water", "rain");
                weatherPair.Add("fire", "sunny");
                weatherPair.Add("grass", "grassy_terrain");
                weatherPair.Add("electric", "electric_terrain");
                weatherPair.Add("fairy", "misty_terrain");
                weatherPair.Add("psychic", "psychic_terrain");
                weatherPair.Add("ice", "hail");
                weatherPair.Add("rock", "sandstorm");
                weatherPair.Add("ground", "sandstorm");
                weatherPair.Add("steel", "sandstorm");
                weatherPair.Add("flying", "wind");
                item.UseEvent.OnHits.Add(0, new TypeWeatherEvent(weatherPair));
                item.UseAction = new SelfAction();
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
            }
            else if (ii == 252)
            {
                item.Name = new LocalText("Mobile Orb");
                item.Desc = new LocalText("An orb that allows the user's team to move through walls.");
                item.Sprite = "Orb_White";
                item.UseEvent.OnHits.Add(0, new StatusBattleEvent("super_mobile", true, false));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                item.UseAction.ActionFX.Sound = "DUN_Invisible";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
            }
            else if (ii == 253)
            {
                item.Name = new LocalText("Luminous Orb");
                item.Desc = new LocalText("A mapping orb that reveals the entire floor.");
                item.Sprite = "Orb_Tan";
                //need ground effects
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = CharAction.MAX_RANGE;
                ((AreaAction)item.UseAction).Speed = 36;
                ((AreaAction)item.UseAction).HitTiles = true;
                item.UseAction.ActionFX.Sound = "DUN_Luminous_Orb";
                item.UseEvent.OnHitTiles.Add(0, new MapOutEvent());
                item.UseEvent.AfterActions.Add(0, new LuminousEvent());
                item.UseEvent.AfterActions.Add(0, new BattleLogBattleEvent(new StringKey("MSG_FLOOR_MAP")));
            }
            else if (ii == 254)
            {
                item.Name = new LocalText("Invert Orb");
                item.Desc = new LocalText("An orb that temporarily reverses all type matchups.");
                item.Sprite = "Orb_White";

                item.UseEvent.OnHits.Add(0, new GiveMapStatusEvent("inverse"));
                item.UseAction = new SelfAction();
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
            }
            else if (ii == 255)
            {
                item.Name = new LocalText("Invisify Orb");
                item.Desc = new LocalText("An orb that temporarily makes the user's team invisible.");
                item.Sprite = "Orb_Purple";
                item.UseEvent.OnHits.Add(0, new StatusBattleEvent("invisible", true, false));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                item.UseAction.ActionFX.Sound = "DUN_Invisible";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
            }
            else if (ii == 256)
            {
                item.Name = new LocalText("Fill-In Orb");
                item.Desc = new LocalText("An orb that fills in all holes on the floor, removing all gullies, water, or lava.");
                item.Sprite = "Orb_Brown";
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = CharAction.MAX_RANGE;
                ((AreaAction)item.UseAction).Speed = 36;
                ((AreaAction)item.UseAction).HitTiles = true;
                item.UseAction.ActionFX.Sound = "_UNK_DUN_Seismic";
                item.UseEvent.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new SingleEmitter(new AnimData("Puff_Brown", 3)),
                    new FlagType(typeof(WaterTerrainState)), new FlagType(typeof(LavaTerrainState)), new FlagType(typeof(AbyssTerrainState))));
                item.UseEvent.AfterActions.Add(0, new BattleLogBattleEvent(new StringKey("MSG_FLOOR_FILL")));
            }
            else if (ii == 257)
            {
                item.Name = new LocalText("Devolve Orb");
                item.Desc = new LocalText("An orb that devolves all enemies up to 5 tiles away.");
                item.Sprite = "Orb_Pink";
                item.UseEvent.OnHits.Add(0, new DevolveEvent(false, "transformed", 5));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                item.UseAction.ActionFX.Sound = "DUN_Stat_Down_2";
                item.UseAction.TargetAlignments |= Alignment.Foe;
                item.Explosion.TargetAlignments |= Alignment.Foe;
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Puff_Green", 3));
                item.UseEvent.HitFX.Sound = "DUN_Transform";
            }
            else if (ii == 258)
            {
                item.Name = new LocalText("All-Aim Orb");
                item.Desc = new LocalText("An orb that ensures that all of the team's attacks hit.");
                item.Sprite = "Orb_Red";
                item.UseEvent.OnHits.Add(0, new StatusBattleEvent("sure_shot", true, false));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                item.UseAction.ActionFX.Sound = "DUN_Eyedrop";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
            }
            else if (ii == 259)
            {
                item.Name = new LocalText("Trawl Orb");
                item.Desc = new LocalText("An orb that pulls unclaimed items on the floor to the user.");
                item.Sprite = "Orb_Yellow";
                item.UseEvent.OnHits.Add(0, new TrawlEvent());
                item.UseAction = new SelfAction();
                item.UseAction.ActionFX.Sound = "DUN_Petrify_Orb";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
            }
            else if (ii == 260)
            {
                item.Name = new LocalText("Revival Orb");
                item.Desc = new LocalText("An orb that revives all team members that have fainted.");
                item.Sprite = "Orb_Yellow";
                item.UseEvent.OnHits.Add(0, new ReviveAllEvent());
                item.UseAction = new SelfAction();
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
            }
            else if (ii == 261)
            {
                item.Name = new LocalText("Scanner Orb");
                item.Desc = new LocalText("An orb that reveals the location of all items and enemies on the floor.");
                item.Sprite = "Orb_DarkBlue";

                item.UseEvent.OnHits.Add(0, new GiveMapStatusEvent("scanner"));
                item.UseAction = new SelfAction();
                item.UseAction.ActionFX.Sound = "DUN_Identify_2";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
            }
            else if (ii == 262)
            {
                item.Name = new LocalText("**All-Hit Orb");
            }
            else if (ii == 263)
            {
                item.Name = new LocalText("Cleanse Orb");
                item.Desc = new LocalText("An orb that un-sticks the team's items, allowing them to be removed.");
                item.Sprite = "Orb_Blue";
                item.UseEvent.OnHits.Add(0, new CleanseTeamEvent());
                item.UseEvent.AfterActions.Add(0, new BattleLogBattleEvent(new StringKey("MSG_CLEANSE_ALL")));
                item.UseAction = new SelfAction();
                item.UseAction.ActionFX.Sound = "DUN_Surf";
                SingleEmitter emitter = new SingleEmitter(new AnimData("Cleanse_Blue", 2));
                emitter.LocHeight = 120;
                item.UseAction.ActionFX.Emitter = emitter;
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
            }
            else if (ii == 264)
            {
                item.Name = new LocalText("One-Shot Orb");
                item.Desc = new LocalText("An orb that causes the target to instantly faint, if it hits. It affects all enemies up to 5 tiles away.");
                item.UseEvent.HitRate = 100;
                item.Sprite = "Orb_Purple";
                item.UseEvent.BeforeHits.Add(0, new CustomHitRateEvent(1, 2));
                item.UseEvent.OnHits.Add(-1, new OHKODamageEvent());
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                item.UseAction.TargetAlignments |= Alignment.Foe;
                item.Explosion.TargetAlignments |= Alignment.Foe;
            }
            else if (ii == 265)
            {
                item.Name = new LocalText("Endure Orb");
                item.Desc = new LocalText("An orb that allows the team to endure any attack with at least 1 HP.");
                item.Sprite = "Orb_Green";
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(11));
                item.UseEvent.OnHits.Add(0, new StatusStateBattleEvent("endure", true, false, statusStates));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                item.UseAction.ActionFX.Sound = "DUN_Eyedrop";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
            }
            else if (ii == 266)
            {
                item.Name = new LocalText("Pierce Orb");
                item.Desc = new LocalText("An orb that allows the team's attacks and items to pierce through multiple targets.");
                item.Sprite = "Orb_White";
                item.UseEvent.OnHits.Add(0, new StatusBattleEvent("pierce", true, false));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                item.UseAction.ActionFX.Sound = "DUN_Focus_Energy";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
            }
            else if (ii == 267)
            {
                item.Name = new LocalText("Stayaway Orb");
                item.Desc = new LocalText("An orb that warps enemies to the nearest stairway, and puts them all into a deep sleep. It affects all enemies up to 5 tiles away.");
                item.Sprite = "Orb_Purple";
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(-1));
                item.UseEvent.OnHits.Add(0, new StatusStateBattleEvent("sleep", true, false, statusStates));
                item.UseEvent.OnHits.Add(0, new WarpToEndEvent(80, 0, true));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                item.UseAction.ActionFX.Sound = "DUN_Disable";
                item.UseAction.TargetAlignments |= Alignment.Foe;
                item.Explosion.TargetAlignments |= Alignment.Foe;
            }
            else if (ii == 268)
            {
                item.Name = new LocalText("All Protect Orb");
                item.Desc = new LocalText("An orb that protects the team from all attacks for several turns.");
                item.Sprite = "Orb_Green";
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(4));
                item.UseEvent.OnHits.Add(0, new StatusStateBattleEvent("all_protect", true, false, statusStates));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                item.UseAction.ActionFX.Sound = "DUN_Protect";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Protect", 3));
            }
            else if (ii == 269)
            {
                item.Name = new LocalText("Trap-See Orb");
                item.Desc = new LocalText("An orb that reveals all traps on the floor.");
                item.Sprite = "Orb_Brown";
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = CharAction.MAX_RANGE;
                ((AreaAction)item.UseAction).HitTiles = true;
                ((AreaAction)item.UseAction).ActionFX.Emitter = new SingleEmitter(new AnimData("Circle_Red_Out", 3));
                item.UseAction.ActionFX.Sound = "DUN_Identify_2";
                item.UseEvent.OnHitTiles.Add(0, new RevealTrapEvent());
                item.UseEvent.AfterActions.Add(0, new BattleLogBattleEvent(new StringKey("MSG_FLOOR_REVEAL")));
            }
            else if (ii == 270)
            {
                item.Name = new LocalText("Trapbust Orb");
                item.Desc = new LocalText("An orb that destroys all traps on the floor.");
                item.Sprite = "Orb_Brown";
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = CharAction.MAX_RANGE;
                ((AreaAction)item.UseAction).HitTiles = true;
                ((AreaAction)item.UseAction).ActionFX.ScreenMovement = new ScreenMover(0, 12, 30);
                item.UseAction.ActionFX.Sound = "DUN_Trapbust_Orb";
                item.UseEvent.OnHitTiles.Add(0, new RemoveTrapEvent());
                item.UseEvent.AfterActions.Add(0, new BattleLogBattleEvent(new StringKey("MSG_FLOOR_TRAPBUST")));
            }
            else if (ii == 271)
            {
                item.Name = new LocalText("Slumber Orb");
                item.Desc = new LocalText("An orb that puts all enemies to sleep. It affects all enemies up to 5 tiles away.");
                item.Sprite = "Orb_Pink";
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(15));
                item.UseEvent.OnHits.Add(0, new StatusStateBattleEvent("sleep", true, false, statusStates));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Puff_Pink", 3));
                emitter.BurstTime = 6;
                emitter.ParticlesPerBurst = 4;
                emitter.Bursts = 5;
                emitter.StartDistance = 4;
                ((AreaAction)item.UseAction).Emitter = emitter;
                item.UseAction.ActionFX.Sound = "DUN_Sweet_Scent";
                item.UseAction.TargetAlignments |= Alignment.Foe;
                item.Explosion.TargetAlignments |= Alignment.Foe;
            }
            else if (ii == 272)
            {
                item.Name = new LocalText("Totter Orb");
                item.Desc = new LocalText("An orb that confuses all enemies. It affects all enemies up to 5 tiles away.");
                item.Sprite = "Orb_Tan";
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(15));
                item.UseEvent.OnHits.Add(0, new StatusStateBattleEvent("confuse", true, false, statusStates));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                CircleSquareReleaseEmitter emitter = new CircleSquareReleaseEmitter(new AnimData("Swift_RSE", 1, 0, 0),
                    new AnimData("Swift_RSE", 1, 1, 1), new AnimData("Swift_RSE", 1, 2, 2), new AnimData("Swift_RSE", 1, 2, 2));
                emitter.BurstTime = 3;
                emitter.ParticlesPerBurst = 3;
                emitter.Bursts = 8;
                emitter.StartDistance = 4;
                ((AreaAction)item.UseAction).Emitter = emitter;
                item.UseAction.ActionFX.Sound = "DUN_Disable";
                item.UseAction.TargetAlignments |= Alignment.Foe;
                item.Explosion.TargetAlignments |= Alignment.Foe;
            }
            else if (ii == 273)
            {
                item.Name = new LocalText("Petrify Orb");
                item.Desc = new LocalText("An orb that paralyzes all enemies. It affects all enemies up to 5 tiles away.");
                item.Sprite = "Orb_Tan";
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(11));
                item.UseEvent.OnHits.Add(0, new StatusStateBattleEvent("paralyze", true, false, statusStates));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                ((AreaAction)item.UseAction).ActionFX.Emitter = new SingleEmitter(new AnimData("Circle_Yellow_Out", 3));
                item.UseAction.ActionFX.Sound = "DUN_Disable";
                item.UseAction.TargetAlignments |= Alignment.Foe;
                item.Explosion.TargetAlignments |= Alignment.Foe;
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Spark", 3));
                item.UseEvent.HitFX.Sound = "DUN_Paralyzed";
            }
            else if (ii == 274)
            {
                item.Name = new LocalText("Freeze Orb");
                item.Desc = new LocalText("An orb that freezes all enemies. It affects all enemies up to 5 tiles away.");
                item.Sprite = "Orb_LightBlue";
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(20));
                item.UseEvent.OnHits.Add(0, new StatusStateBattleEvent("freeze", true, false, statusStates));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                item.UseAction.ActionFX.Sound = "DUN_Ice_Shard";
                item.UseAction.TargetAlignments |= Alignment.Foe;
                item.Explosion.TargetAlignments |= Alignment.Foe;
            }
            else if (ii == 275)
            {
                item.Name = new LocalText("Spurn Orb");
                item.Desc = new LocalText("An orb that warps all enemies to random locations on the floor. It affects all enemies up to 5 tiles away.");
                item.Sprite = "Orb_Blue";
                item.UseEvent.OnHits.Add(0, new RandomWarpEvent(50, true));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                ((AreaAction)item.UseAction).ActionFX.Emitter = new SingleEmitter(new AnimData("Circle_Red_Out", 3));
                item.UseAction.ActionFX.Sound = "DUN_Disable";
                item.UseAction.TargetAlignments |= Alignment.Foe;
                item.Explosion.TargetAlignments |= Alignment.Foe;
            }
            else if (ii == 276)
            {
                item.Name = new LocalText("Foe-Hold Orb");
                item.Desc = new LocalText("An orb that immobilizes all enemies for many turns. It affects all enemies up to 5 tiles away.");
                item.Sprite = "Orb_DarkBlue";

                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(20));
                item.UseEvent.OnHits.Add(0, new StatusStateBattleEvent("immobilized", true, false, statusStates));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                ((AreaAction)item.UseAction).ActionFX.Emitter = new SingleEmitter(new AnimData("Circle_Yellow_Out", 3));
                item.UseAction.ActionFX.Sound = "DUN_Disable";
                item.UseAction.TargetAlignments |= Alignment.Foe;
                item.Explosion.TargetAlignments |= Alignment.Foe;
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Spark", 3));
                item.UseEvent.HitFX.Sound = "DUN_Paralyzed";
            }
            else if (ii == 277)
            {
                item.Name = new LocalText("Nullify Orb");
                item.Desc = new LocalText("An orb that nullifies the Abilities of all enemies on the floor.");
                item.Sprite = "Orb_Green";
                item.UseEvent.OnHits.Add(0, new ChangeToAbilityEvent(DataManager.Instance.DefaultIntrinsic, true));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = CharAction.MAX_RANGE;
                ((AreaAction)item.UseAction).Speed = 72;
                item.UseAction.ActionFX.Sound = "DUN_Disable";
                ((AreaAction)item.UseAction).ActionFX.Emitter = new SingleEmitter(new AnimData("Circle_Red_Out", 3));
                item.UseAction.TargetAlignments |= Alignment.Foe;
                item.Explosion.TargetAlignments |= Alignment.Foe;
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 3));
                item.UseEvent.HitFX.Sound = "DUN_Switcher";
            }
            else if (ii == 278)
            {
                item.Name = new LocalText("All-Dodge Orb");
                item.Desc = new LocalText("An orb that drastically boosts Evasion. It affects team members up to 5 tiles away.");
                item.Sprite = "Orb_Yellow";
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_evasion", true, false, 6));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
            }
            else if (ii == 279)
            {
                item.Name = new LocalText("**Health Orb");
            }
            else if (ii == 280)
            {
                item.Name = new LocalText("**All Power-Up Orb");
            }
            else if (ii == 281)
            {
                item.Name = new LocalText("One-Room Orb");
                item.Desc = new LocalText("A destructive orb that tears down all walls on the floor, turning the floor into one vast, open room.");
                item.Sprite = "Orb_Brown";
                //need ground effects
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = CharAction.MAX_RANGE;
                ((AreaAction)item.UseAction).Speed = 36;
                item.UseAction.ActionFX.Sound = "DUN_One_Room_Orb";
                ((AreaAction)item.UseAction).HitTiles = true;
                item.UseEvent.OnHitTiles.Add(0, new RemoveTerrainStateEvent("", new SingleEmitter(new AnimData("Wall_Break", 2)), new FlagType(typeof(WallTerrainState)), new FlagType(typeof(FoliageTerrainState))));
                item.UseEvent.OnHitTiles.Add(0, new MapOutEvent());
                item.UseEvent.AfterActions.Add(0, new BattleLogBattleEvent(new StringKey("MSG_FLOOR_ROOM")));
            }
            else if (ii == 282)
            {
                item.Name = new LocalText("Slow Orb");
                item.Desc = new LocalText("An orb that reduces the Movement Speed of all enemies by 1 level. It affects all enemies up to 5 tiles away.");
                item.Sprite = "Orb_Purple";
                item.UseEvent.OnHits.Add(0, new StatusStackBattleEvent("mod_speed", true, false, -1));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                ((AreaAction)item.UseAction).ActionFX.Emitter = new SingleEmitter(new AnimData("Circle_Red_Out", 3));
                item.UseAction.ActionFX.Sound = "DUN_Warp";
                item.UseAction.TargetAlignments |= Alignment.Foe;
                item.Explosion.TargetAlignments |= Alignment.Foe;
            }
            else if (ii == 283)
            {
                item.Name = new LocalText("Rebound Orb");
                item.Desc = new LocalText("An orb that gives the team the Mini-Counter status. If an attack hits any member of the team, the damage will be reflected back at the user.");
                item.Sprite = "Orb_Red";
                item.UseEvent.OnHits.Add(0, new StatusBattleEvent("mini_counter", true, false));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                item.UseAction.ActionFX.Sound = "DUN_Light_Screen";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new BeamAnimData("Column_White", 3, -1, -1, 192));
            }
            else if (ii == 284)
            {
                item.Name = new LocalText("Mirror Orb");
                item.Desc = new LocalText("An orb that protects the team with a barrier that reflects status moves.");
                item.Sprite = "Orb_Green";
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(30));
                item.UseEvent.OnHits.Add(0, new StatusStateBattleEvent("magic_coat", true, false, statusStates));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                item.UseAction.ActionFX.Sound = "DUN_Light_Screen";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Screen_RSE_Green", 3, -1, -1, 192));
            }
            else if (ii == 285)
            {
                item.Name = new LocalText("Itemizer Orb");
                item.Desc = new LocalText("An orb that transforms the target into an item. It affects the enemy in front.");
                item.Sprite = "Orb_Yellow";
                item.UseEvent.OnHits.Add(0, new ItemizerEvent());
                item.UseAction = new ProjectileAction();
                ((ProjectileAction)item.UseAction).CharAnimData = new CharAnimFrameType(42);//Rotate
                ((ProjectileAction)item.UseAction).Range = 1;
                ((ProjectileAction)item.UseAction).Speed = 12;
                ((ProjectileAction)item.UseAction).StopAtHit = true;
                ((ProjectileAction)item.UseAction).StopAtWall = true;
                ((ProjectileAction)item.UseAction).Anim = new AnimData("Confuse_Ray", 2);
                ((ProjectileAction)item.UseAction).LagBehindTime = 20;
                item.UseAction.TargetAlignments |= Alignment.Foe;
                item.Explosion.TargetAlignments |= Alignment.Foe;
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 3));
                item.UseEvent.HitFX.Sound = "DUN_Switcher";
                item.UseAction.ActionFX.Sound = "DUN_Blowback_Orb";
            }
            else if (ii == 286)
            {
                item.Name = new LocalText("Foe-Seal Orb");
                item.Desc = new LocalText("An orb that seals the last-used move of all enemies. It affects all enemies up to 5 tiles away.");
                item.Sprite = "Orb_DarkBlue";
                item.UseEvent.OnHits.Add(0, new DisableBattleEvent("disable", "last_used_move_slot", false, true));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                item.UseAction.TargetAlignments |= Alignment.Foe;
                item.Explosion.TargetAlignments |= Alignment.Foe;
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 3));
                item.UseEvent.HitFX.Sound = "DUN_Switcher";
            }
            else if (ii == 287)
            {
                item.Name = new LocalText("Halving Orb");
                item.Desc = new LocalText("An orb that halves the HP of all enemies. It affects all enemies up to 5 tiles away.");
                item.Sprite = "Orb_Red";
                item.UseEvent.OnHits.Add(-1, new CutHPDamageEvent());
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = 5;
                ((AreaAction)item.UseAction).Speed = 10;
                item.UseAction.TargetAlignments |= Alignment.Foe;
                item.Explosion.TargetAlignments |= Alignment.Foe;
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 3));
                item.UseEvent.HitFX.Sound = "DUN_Two-Edge";
            }
            else if (ii == 288)
            {
                item.Name = new LocalText("Rollcall Orb");
                item.Desc = new LocalText("A warping orb that draws all teammates to the user.");
                item.Sprite = "Orb_Blue";
                item.UseEvent.OnHits.Add(0, new WarpHereEvent(new StringKey(), true));
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = CharAction.MAX_RANGE;
                item.UseAction.TargetAlignments |= Alignment.Friend;
                item.Explosion.TargetAlignments |= Alignment.Friend;
            }
            else if (ii == 289)
            {
                item.Name = new LocalText("Mug Orb");
                item.Desc = new LocalText("An orb that pulls all items held by enemy Pokémon to the user. It affects all enemies on the floor.");
                item.Sprite = "Orb_Yellow";
                item.UseEvent.OnHits.Add(0, new MugItemEvent());
                item.UseAction = new AreaAction();
                ((AreaAction)item.UseAction).Range = CharAction.MAX_RANGE;
                item.UseAction.ActionFX.Sound = "DUN_Petrify_Orb";
                item.UseAction.TargetAlignments |= Alignment.Foe;
                item.Explosion.TargetAlignments |= Alignment.Foe;
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_Out", 3));
                item.UseEvent.HitFX.Sound = "DUN_Throw_Spike";
                item.UseEvent.AfterActions.Add(0, new BattleLogBattleEvent(new StringKey("MSG_MUG")));
            }
            else if (ii == 290)
            {
                item.Name = new LocalText("Monster Orb");
                item.Desc = new LocalText("An orb that summons a monster house near the user.");
                item.Sprite = "Orb_Green";
                item.UseEvent.OnHits.Add(0, new BattlelessEvent(true, new MonsterHouseOwnerEvent(new RogueElements.RandRange(7, 13), 50)));
                item.UseAction = new SelfAction();
                item.UseAction.ActionFX.Sound = "DUN_Petrify_Orb";
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
            }
            else if (ii == 291)
            {

            }
            else if (ii == 300)
            {
                item.Name = new LocalText("Friend Bow");
                item.Desc = new LocalText("A held item that makes it easier for team members to recruit wild Pokémon.");
                item.Sprite = "Bow_Pink";
                item.Price = 2000;
                item.OnActions.Add(0, new FlatRecruitmentEvent(30));
            }
            else if (ii == 301)
            {
                item.Name = new LocalText("Pierce Band");
                item.Desc = new LocalText("A headband that allows the Pokémon's moves and items to hit through multiple targets.");
                item.Sprite = "Band_Gray";
                item.Price = 500;
                item.OnActions.Add(0, new PierceEvent(true, true, true, false));
            }
            else if (ii == 302)
            {
                item.Name = new LocalText("Golden Mask");
                item.Desc = new LocalText("A held item that makes it easier for team members to recruit lower-leveled wild Pokémon.");
                item.Sprite = "Mask_Gold";
                item.Price = 4000;
                item.OnActions.Add(0, new LevelRecruitmentEvent());
            }
            else if (ii == 303)
            {
                item.Name = new LocalText("Mobile Scarf");
                item.Desc = new LocalText("A held item that enables the Pokémon to move through walls.");
                item.Sprite = "Scarf_2_White";
                item.Price = 200;
                item.OnRefresh.Add(0, new AddMobilityEvent(TerrainData.Mobility.Block));
            }
            else if (ii == 304)
            {
                item.Name = new LocalText("Pass Scarf");
                item.Desc = new LocalText("A held item that causes the Pokémon to shrug off damaging moves and pass them on to an adjacent Pokémon.");
                item.Sprite = "Scarf_Pink";
                item.Price = 200;
                item.ProximityEvent.Radius = 0;
                item.ProximityEvent.TargetAlignments = Alignment.Foe;
                item.ProximityEvent.BeforeExplosions.Add(0, new PassAttackEvent(5));
            }
            else if (ii == 305)
            {
                item.Name = new LocalText("Warp Scarf");
                item.Desc = new LocalText("A held item that warps the Pokémon to another place on the floor after being attacked.");
                item.Sprite = "Scarf_Blue";
                item.Price = 200;
                item.AfterBeingHits.Add(1, new HitCounterEvent(Alignment.Foe, true, false, true, 100, new RandomWarpEvent(50, true)));
            }
            else if (ii == 306)
            {
                item.Name = new LocalText("Trap Scarf");
                item.Desc = new LocalText("A held item that prevents the Pokémon from setting off traps.");
                item.Sprite = "Scarf_Brown";
                item.Price = 200;
                item.OnRefresh.Add(0, new MiscEvent(new TrapState()));
            }
            else if (ii == 307)
            {
                item.Name = new LocalText("Grip Claw");
                item.Desc = new LocalText("An item that increases the duration of binding moves used by the holder.");
                item.Sprite = "Claw_Gray";
                item.Price = 200;
                item.OnRefresh.Add(0, new MiscEvent(new GripState()));
            }
            else if (ii == 308)
            {
                item.Name = new LocalText("Binding Band");
                item.Desc = new LocalText("An item to be held by a Pokémon. When the holder successfully inflicts damage, the target may also be immobilized.");
                item.Sprite = "Band_Tan";
                item.Price = 200;
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(3));
                item.AfterHittings.Add(0, new OnMoveUseEvent(new OnHitEvent(true, false, 25, new StatusStateBattleEvent("immobilized", true, true, statusStates))));
            }
            else if (ii == 309)
            {
                item.Name = new LocalText("Twist Band");
                item.Desc = new LocalText("A held item that prevents the Pokémon's Attack and Special Attack from being reduced.");
                item.Sprite = "Scarf_Red";
                item.Price = 200;
                List<Stat> drops = new List<Stat>();
                drops.Add(Stat.Attack);
                drops.Add(Stat.MAtk);
                item.BeforeStatusAdds.Add(0, new StatChangeCheck(drops, new StringKey("MSG_STAT_DROP_PROTECT"), true, false, false));
            }
            else if (ii == 310)
            {
                item.Name = new LocalText("Metronome");
                item.Desc = new LocalText("An item to be held by a Pokémon. It boosts moves used consecutively, but only until a different move is used.");
                item.Sprite = "Box_White";
                item.Price = 200;
                item.BeforeHittings.Add(0, new RepeatHitEvent("last_used_move", "times_move_used", 20, 10, false));
            }
            else if (ii == 311)
            {
                item.Name = new LocalText("Shed Shell");
                item.Desc = new LocalText("A tough, discarded carapace that prevents the holder from being immobilized.");
                item.Sprite = "Box_Tan";
                item.Price = 200;
                item.BeforeStatusAdds.Add(0, new PreventStatusCheck("immobilized", new StringKey("MSG_SHED_SHELL")));
                item.BeforeStatusAdds.Add(0, new PreventStatusCheck("wrap", new StringKey("MSG_SHED_SHELL")));
                item.BeforeStatusAdds.Add(0, new PreventStatusCheck("bind", new StringKey("MSG_SHED_SHELL")));
                item.BeforeStatusAdds.Add(0, new PreventStatusCheck("fire_spin", new StringKey("MSG_SHED_SHELL")));
                item.BeforeStatusAdds.Add(0, new PreventStatusCheck("whirlpool", new StringKey("MSG_SHED_SHELL")));
                item.BeforeStatusAdds.Add(0, new PreventStatusCheck("sand_tomb", new StringKey("MSG_SHED_SHELL")));
                item.BeforeStatusAdds.Add(0, new PreventStatusCheck("telekinesis", new StringKey("MSG_SHED_SHELL")));
                item.BeforeStatusAdds.Add(0, new PreventStatusCheck("clamp", new StringKey("MSG_SHED_SHELL")));
                item.BeforeStatusAdds.Add(0, new PreventStatusCheck("infestation", new StringKey("MSG_SHED_SHELL")));
                item.BeforeStatusAdds.Add(0, new PreventStatusCheck("magma_storm", new StringKey("MSG_SHED_SHELL")));
            }
            else if (ii == 312)
            {
                item.Name = new LocalText("Shell Bell");
                item.Desc = new LocalText("A held item that restores a little HP to the holder every time it inflicts damage on others.");
                item.Sprite = "Box_White";
                item.Price = 200;
                item.AfterActions.Add(0, new HPDrainEvent(8));
            }
            else if (ii == 313)
            {
                item.Name = new LocalText("Scope Lens");
                item.Desc = new LocalText("A lens used for scoping out weak points. It boosts the holder’s critical-hit ratio.");
                item.Sprite = "Specs_DarkBlue";
                item.Price = 200;
                item.OnActions.Add(0, new BoostCriticalEvent(1));
            }
            else if (ii == 314)
            {
                item.Name = new LocalText("Wide Lens");
                item.Desc = new LocalText("A magnifying lens that boosts the Pokémon's Attack Range.");
                item.Sprite = "Specs_LightBlue";
                item.Price = 200;
                item.OnActions.Add(-1, new OnMoveUseEvent(new AddRangeEvent(1)));
            }
            else if (ii == 315)
            {
                item.Name = new LocalText("Heal Ribbon");
                item.Desc = new LocalText("A held item that speeds up the Pokémon's natural HP recovery. This item sticks when held.");
                item.Sprite = "Bow_Minty";
                item.Price = 250;
                item.ModifyHPs.Add(1, new HealMultEvent(2, 1));
            }
            else if (ii == 316)
            {
                //Stamina Band?
                //There must be a way to guarantee that this item does not disrupt the balance of the game.
                //Being given more time to act, grind, etc. is extremely powerful as a perk, especially in set-level dungeons.
            }
            else if (ii == 317)
            {
                item.Name = new LocalText("Sticky Barb");
                item.Desc = new LocalText("A held item that damages the holder and sticks when held. It may latch on to Pokémon that make contact with the holder.");
                item.Sprite = "Sticky_Barb";
                item.Price = 300;
                item.AfterHittings.Add(0, new HitCounterEvent((Alignment.Friend | Alignment.Foe), true, true, false, 100, new SwitchHeldItemEvent()));
                item.AfterActions.Add(0, new OnAggressionEvent(new ChipDamageEvent(12, new StringKey(), true, true)));
            }
            else if (ii == 318)
            {
                item.Name = new LocalText("Choice Band");
                item.Desc = new LocalText("A curious headband that boosts Attack, but only allows the use of one move. This item sticks when held.");
                item.Sprite = "Band_Gray";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Physical, 3, 2));
                item.OnRefresh.Add(0, new MoveLockEvent("last_used_move_slot", true));
            }
            else if (ii == 319)
            {
                item.Name = new LocalText("Choice Specs");
                item.Desc = new LocalText("Distinctive glasses that boost Special Attack, but only allows the use of one move. This item sticks when held.");
                item.Sprite = "Specs_Yellow";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Magical, 3, 2));
                item.OnRefresh.Add(0, new MoveLockEvent("last_used_move_slot", true));
            }
            else if (ii == 320)
            {
                item.Name = new LocalText("Choice Scarf");
                item.Desc = new LocalText("A curious scarf that boosts Movement Speed, but only allows the use of one move. This item sticks when held.");
                item.Sprite = "Scarf_2_LightBlue";
                item.Price = 500;
                item.OnRefresh.Add(0, new AddSpeedEvent(1));
                item.OnRefresh.Add(0, new MoveLockEvent("last_used_move_slot", true));
            }
            else if (ii == 321)
            {
                item.Name = new LocalText("Assault Vest");
                item.Desc = new LocalText("An offensive vest that raises Special Defense, but prevents the use of status moves. This item sticks when held.");
                item.Sprite = "Silk_Red";
                item.Price = 300;
                item.BeforeBeingHits.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Magical, 2, 3));
                item.OnRefresh.Add(0, new TauntEvent());
            }
            else if (ii == 322)
            {
                item.Name = new LocalText("Life Orb");
                item.Desc = new LocalText("A held item that boosts the power of moves, but at the cost of some HP on each use. This item sticks when held.");
                item.Sprite = "Life_Orb";
                item.Price = 300;
                item.OnActions.Add(0, new MultiplyDamageEvent(4, 3));
                item.AfterActions.Add(0, new OnAggressionEvent(new ChipDamageEvent(12, new StringKey(), true, true)));
            }
            else if (ii == 323)
            {
                item.Name = new LocalText("Toxic Orb");
                item.Desc = new LocalText("A bizarre orb that will badly poison the holder. This item sticks when held.");
                item.Sprite = "Toxic_Orb";
                item.Price = 300;
                item.AfterActions.Add(0, new OnAggressionEvent(new StatusBattleEvent("poison_toxic", false, true)));
            }
            else if (ii == 324)
            {
                item.Name = new LocalText("Flame Orb");
                item.Desc = new LocalText("A bizarre orb that will afflict the holder with a burn. This item sticks when held.");
                item.Sprite = "Flame_Orb";
                item.Price = 300;
                item.AfterActions.Add(0, new OnAggressionEvent(new StatusBattleEvent("burn", false, true)));
            }
            else if (ii == 325)
            {
                item.Name = new LocalText("Iron Ball");
                item.Desc = new LocalText("A held item that weighs the Pokémon down, and allows Ground-type moves to hit Flying-type and levitating holders. This item sticks when held.");
                item.Sprite = "Iron_Ball";
                item.Price = 300;
                StateCollection<StatusState> statusStates = new StateCollection<StatusState>();
                statusStates.Set(new CountDownState(2));
                item.AfterActions.Add(0, new OnMoveUseEvent(new StatusStateBattleEvent("paused", false, true, statusStates)));
                item.OnRefresh.Add(0, new MiscEvent(new AnchorState()));
                item.TargetElementEffects.Add(1, new TypeVulnerableEvent("ground"));
            }
            else if (ii == 326)
            {
                item.Name = new LocalText("Ring Target");
                item.Desc = new LocalText("A held item that removes the Pokémon's type immunities. This item sticks when held.");
                item.Sprite = "Ring_Target";
                item.Price = 250;
                item.TargetElementEffects.Add(1, new NoImmunityEvent());
                item.TargetElementEffects.Add(1, new NoResistanceEvent());
            }
            else if (ii == 327)
            {
                item.Name = new LocalText("**Black Sludge");
                item.Desc = new LocalText("A held item that increases the HP regeneration of Poison-types, but prevents it for any other type. This item sticks when held.");
                item.Sprite = "Rock_Purple";
                item.Price = 1;
            }
            else if (ii == 328)
            {
                item.Name = new LocalText("X-Ray Specs");
                item.Desc = new LocalText("A held item that enables the Pokémon to see enemies through heavy darkness.");
                item.Sprite = "Specs_Red";
                item.Price = 200;
                item.OnRefresh.Add(0, new SetSightEvent(true, Map.SightRange.Clear));
                item.OnRefresh.Add(0, new SeeCharsEvent());
            }
            else if (ii == 329)
            {
                item.Name = new LocalText("Reunion Cape");
                item.Desc = new LocalText("A held item that warps the Pokémon to its teammate or leader when attacked.");
                item.Sprite = "Cape_Gray";
                item.Price = 200;
                item.AfterBeingHits.Add(1, new HitCounterEvent(Alignment.Foe, true, false, true, 100, new WarpToAllyEvent()));
            }
            else if (ii == 330)
            {
                item.Name = new LocalText("Cover Band");
                item.Desc = new LocalText("An item to be held by a Pokémon. While the holder's HP is at half or above, it will take attacks for nearby allies.");
                item.Sprite = "Band_Green";
                item.Price = 200;
                item.ProximityEvent.Radius = 1;
                item.ProximityEvent.TargetAlignments = Alignment.Foe;
                item.ProximityEvent.BeforeExplosions.Add(0, new CoverAttackEvent());
            }
            else if (ii == 331)
            {
                item.Name = new LocalText("Silver Powder");
                item.Desc = new LocalText("An item to be held by a Pokémon. It is a shiny, silver powder that will boost the power of Bug-type moves.");
                item.Sprite = "Sack_Gray";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyElementEvent("bug", 6, 5, false));
            }
            else if (ii == 332)
            {
                item.Name = new LocalText("Black Glasses");
                item.Desc = new LocalText("An item to be held by a Pokémon. A pair of shady-looking glasses that boost the power of Dark-type moves.");
                item.Sprite = "Specs_DarkBlue";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyElementEvent("dark", 6, 5, false));
            }
            else if (ii == 333)
            {
                item.Name = new LocalText("Dragon Scale");
                item.Desc = new LocalText("An item to be held by a Pokémon. This thick, tough scale boosts the power of Dragon-type moves.");
                item.Sprite = "Scale_Green";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyElementEvent("dragon", 6, 5, false));
            }
            else if (ii == 334)
            {
                item.Name = new LocalText("Magnet");
                item.Desc = new LocalText("An item to be held by a Pokémon. It is a powerful magnet that boosts the power of Electric-type moves.");
                item.Sprite = "Box_Yellow";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyElementEvent("electric", 6, 5, false));
            }
            else if (ii == 335)
            {
                item.Name = new LocalText("Pink Bow");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a cute bow that boosts the power of Fairy-type moves.");
                item.Sprite = "Bow_Pink";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyElementEvent("fairy", 6, 5, false));
            }
            else if (ii == 336)
            {
                item.Name = new LocalText("Black Belt");
                item.Desc = new LocalText("An item to be held by a Pokémon. This belt helps the wearer to focus and boosts the power of Fighting-type moves.");
                item.Sprite = "Belt_Black";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyElementEvent("fighting", 6, 5, false));
            }
            else if (ii == 337)
            {
                item.Name = new LocalText("Charcoal");
                item.Desc = new LocalText("An item to be held by a Pokémon. It is a combustible fuel that boosts the power of Fire-type moves.");
                item.Sprite = "Box_DarkBlue";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyElementEvent("fire", 6, 5, false));
            }
            else if (ii == 338)
            {
                item.Name = new LocalText("Sharp Beak");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a long, sharp beak that boosts the power of Flying-type moves.");
                item.Sprite = "Stick_Yellow";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyElementEvent("flying", 6, 5, false));
            }
            else if (ii == 339)
            {
                item.Name = new LocalText("Spell Tag");
                item.Desc = new LocalText("An item to be held by a Pokémon. It is a sinister, eerie tag that boosts the power of Ghost-type moves.");
                item.Sprite = "Tag_Purple";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyElementEvent("ghost", 6, 5, false));
            }
            else if (ii == 340)
            {
                item.Name = new LocalText("Miracle Seed");
                item.Desc = new LocalText("An item to be held by a Pokémon. It is a seed imbued with life force that boosts the power of Grass-type moves.");
                item.Sprite = "Seed_Yellow";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyElementEvent("grass", 6, 5, false));
            }
            else if (ii == 341)
            {
                item.Name = new LocalText("Soft Sand");
                item.Desc = new LocalText("An item to be held by a Pokémon. It is a loose, silky sand that boosts the power of Ground-type moves.");
                item.Sprite = "Sack_LightBlue";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyElementEvent("ground", 6, 5, false));
            }
            else if (ii == 342)
            {
                item.Name = new LocalText("Never-Melt Ice");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a piece of ice that repels heat effects and boosts Ice-type moves.");
                item.Sprite = "Ore_Blue";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyElementEvent("ice", 6, 5, false));
            }
            else if (ii == 343)
            {
                item.Name = new LocalText("Silk Scarf");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a sumptuous scarf that boosts the power of Normal-type moves.");
                item.Sprite = "Scarf_2_White";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyElementEvent("normal", 6, 5, false));
            }
            else if (ii == 344)
            {
                item.Name = new LocalText("Poison Barb");
                item.Desc = new LocalText("An item to be held by a Pokémon. This small, poisonous barb boosts the power of Poison-type moves.");
                item.Sprite = "Stick_Pink";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyElementEvent("poison", 6, 5, false));
            }
            else if (ii == 345)
            {
                item.Name = new LocalText("Twisted Spoon");
                item.Desc = new LocalText("An item to be held by a Pokémon. This spoon is imbued with telekinetic power and boosts Psychic-type moves.");
                item.Sprite = "Box_Pink";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyElementEvent("psychic", 6, 5, false));
            }
            else if (ii == 346)
            {
                item.Name = new LocalText("Hard Stone");
                item.Desc = new LocalText("An item to be held by a Pokémon. It is a durable stone that boosts the power of Rock-type moves.");
                item.Sprite = "Stone_Black";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyElementEvent("rock", 6, 5, false));
            }
            else if (ii == 347)
            {
                item.Name = new LocalText("Metal Coat");
                item.Desc = new LocalText("An item to be held by a Pokémon. It is a special metallic film that can boost the power of Steel-type moves.");
                item.Sprite = "Box_White";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyElementEvent("steel", 6, 5, false));
            }
            else if (ii == 348)
            {
                item.Name = new LocalText("Mystic Water");
                item.Desc = new LocalText("An item to be held by a Pokémon. This teardrop-shaped gem boosts the power of Water-type moves.");
                item.Sprite = "Bottle_LightBlue";
                item.Price = 500;
                item.OnActions.Add(0, new MultiplyElementEvent("water", 6, 5, false));
            }
            else if (ii == 349)
            {
                item.Name = new LocalText("Harmony Scarf");
                item.Desc = new LocalText("A scarf woven from the fibers of a legendary tree. It allows the Pokémon to evolve, regardless of its requirements.");
                item.Sprite = "Scarf_Blue";
                item.Price = 2500;
            }
            else if (ii == 350)
            {
                item.Name = new LocalText("**Everstone");
                //item.Desc = new LocalText("A held item that prevents the Pokémon from gaining EXP. This item sticks when held.");
                //item.Sticky = true;
                item.Sprite = "Rock_Gray";
                item.Price = 250;
            }
            else if (ii == 351)
            {
                item.Name = new LocalText("Fire Stone");
                item.Desc = new LocalText("A peculiar orange stone that radiates warmth like a flame. It allows certain kinds of Pokémon to evolve. If held, it changes the Pokémon's regular attacks to Fire-type.");
                item.Sprite = "Stone_Red";
                item.OnActions.Add(-1, new RegularAttackNeededEvent(new ChangeMoveElementEvent("none", "fire")));
            }
            else if (ii == 352)
            {
                item.Name = new LocalText("Thunder Stone");
                item.Desc = new LocalText("A peculiar stone inscribed with a thunderbolt pattern. It allows certain kinds of Pokémon to evolve. If held, it changes the Pokémon's regular attacks to Electric-type.");
                item.Sprite = "Stone_Gold";
                item.OnActions.Add(-1, new RegularAttackNeededEvent(new ChangeMoveElementEvent("none", "electric")));
            }
            else if (ii == 353)
            {
                item.Name = new LocalText("Water Stone");
                item.Desc = new LocalText("A peculiar stone that holds the color of clearest blue. It allows certain kinds of Pokémon to evolve. If held, it changes the Pokémon's regular attacks to Water-type.");
                item.Sprite = "Stone_Blue";
                item.OnActions.Add(-1, new RegularAttackNeededEvent(new ChangeMoveElementEvent("none", "water")));
            }
            else if (ii == 354)
            {
                item.Name = new LocalText("Leaf Stone");
                item.Desc = new LocalText("A peculiar stone inscribed with a leafy pattern. It allows certain kinds of Pokémon to evolve. If held, it changes the Pokémon's regular attacks to Grass-type.");
                item.Sprite = "Stone_Green";
                item.OnActions.Add(-1, new RegularAttackNeededEvent(new ChangeMoveElementEvent("none", "grass")));
            }
            else if (ii == 355)
            {
                item.Name = new LocalText("Moon Stone");
                item.Desc = new LocalText("An odd stone that gleams like the moon in the evening sky. It allows certain kinds of Pokémon to evolve. If held, it changes the Pokémon's regular attacks to Fairy-type.");
                item.Sprite = "Stone_Black";
                item.OnActions.Add(-1, new RegularAttackNeededEvent(new ChangeMoveElementEvent("none", "fairy")));
            }
            else if (ii == 356)
            {
                item.Name = new LocalText("Sun Stone");
                item.Desc = new LocalText("An odd stone that glows with sunny warmth. It allows certain kinds of Pokémon to evolve. If held, it changes the Pokémon's regular attacks to Psychic-type.");
                item.Sprite = "Stone_Orange";
                item.OnActions.Add(-1, new RegularAttackNeededEvent(new ChangeMoveElementEvent("none", "psychic")));
            }
            else if (ii == 357)
            {
                item.Name = new LocalText("Magmarizer");
                item.Desc = new LocalText("A box brimming with a huge amount of magma energy. It allows a certain kind of Pokémon to evolve.");
                item.Sprite = "Machine_Pink";
            }
            else if (ii == 358)
            {
                item.Name = new LocalText("Electirizer");
                item.Desc = new LocalText("A box full of a massive amount of electric energy. It allows a certain kind of Pokémon to evolve.");
                item.Sprite = "Machine_Yellow";
            }
            else if (ii == 359)
            {
                item.Name = new LocalText("Reaper Cloth");
                item.Desc = new LocalText("An eerie cloth imbued with horrifyingly strong spiritual energy. It allows a certain kind of Pokémon to evolve.");
                item.Sprite = "Silk_Purple";
            }
            else if (ii == 360)
            {
                item.Name = new LocalText("Cracked Pot");
                item.Desc = new LocalText("A peculiar teapot that can make a certain species of Pokémon evolve. It may be cracked, but tea poured from it is delicious.");
                item.Sprite = "Box_LightBlue";
            }
            else if (ii == 361)
            {
                item.Name = new LocalText("Chipped Pot");
                item.Desc = new LocalText("A peculiar teapot that can make a certain species of Pokémon evolve. It may be chipped, but tea poured from it is delicious.");
                item.Sprite = "Box_LightBlue";
            }
            else if (ii == 362)
            {
                item.Name = new LocalText("Shiny Stone");
                item.Desc = new LocalText("An odd stone that shines with dazzling light. It allows certain kinds of Pokémon to evolve.");
                item.Sprite = "Stone_Yellow";
            }
            else if (ii == 363)
            {
                item.Name = new LocalText("Dusk Stone");
                item.Desc = new LocalText("An odd stone with a mesmerizing darkness. It allows certain kinds of Pokémon to evolve.");
                item.Sprite = "Stone_Purple";
            }
            else if (ii == 364)
            {
                item.Name = new LocalText("Dawn Stone");
                item.Desc = new LocalText("An odd stone that glints like an eye. It allows certain kinds of Pokémon to evolve.");
                item.Sprite = "Stone_White";
            }
            else if (ii == 365)
            {
                item.Name = new LocalText("Link Cable");
                item.Desc = new LocalText("An intriguing cable used for linking unknown devices. It allows a certain kind of Pokémon to evolve.");
                item.Sprite = "Box_Green";
            }
            else if (ii == 366)
            {
                item.Name = new LocalText("Up-Grade");
                item.Desc = new LocalText("A mysterious device filled with all sorts of data. It allows a certain kind of Pokémon to evolve.");
                item.Sprite = "Box_LightPurple";
            }
            else if (ii == 367)
            {
                item.Name = new LocalText("Dubious Disc");
                item.Desc = new LocalText("An enigmatic disc that overflows with dubious data. It allows a certain kind of Pokémon to evolve.");
                item.Sprite = "Box_Purple";
            }
            else if (ii == 368)
            {
                item.Name = new LocalText("Razor Fang");
                item.Desc = new LocalText("A savagely sharp, piercing fang. When the holder successfully inflicts damage, the target may also flinch.");
                item.Sprite = "Fang_White";
                item.AfterHittings.Add(0, new OnMoveUseEvent(new OnHitEvent(true, false, 25, new StatusBattleEvent("flinch", true, true))));
            }
            else if (ii == 369)
            {
                item.Name = new LocalText("Razor Claw");
                item.Desc = new LocalText("A wickedly sharp claw perfect for raking enemies. It increases the holder’s critical-hit ratio.");
                item.Sprite = "Claw_White";
                item.OnActions.Add(0, new BoostCriticalEvent(1));
            }
            else if (ii == 370)
            {

            }
            else if (ii == 371)
            {
                item.Name = new LocalText("Protector");
                item.Desc = new LocalText("A heavy, durable piece of protective equipment. It allows a certain kind of Pokémon to evolve.");
                item.Sprite = "Box_Brown";
            }
            else if (ii == 372)
            {
                item.Name = new LocalText("**Oval Stone");
                item.Desc = new LocalText("A surprisingly smooth, rounded stone. It allows a certain kind of Pokémon to evolve.");
                item.Sprite = "Stone_LightBlue";
            }
            else if (ii == 373)
            {
                item.Name = new LocalText("Prism Scale");
                item.Desc = new LocalText("A mysterious scale that shines in rainbow colors. It allows a certain kind of Pokémon to evolve.");
                item.Sprite = "Scale_LightBlue";
            }
            else if (ii == 374)
            {
                item.Name = new LocalText("King's Rock");
                item.Desc = new LocalText("An impressive icon that conveys a kingly nobility. When the holder successfully inflicts damage, the target may also flinch.");
                item.Sprite = "Crown_Yellow";
                //item.AfterHittings.Add(0, new OnHitEvent(true, false, 25, new StatusBattleEvent("flinch", true, true)));
                item.AfterHittings.Add(0, new OnMoveUseEvent(new OnHitEvent(true, false, 25, new StatusBattleEvent("flinch", true, true))));
            }
            else if (ii == 375)
            {
                item.Name = new LocalText("Deep Sea Tooth");
                item.Desc = new LocalText("A supersharp fang with a subtle gleam. It allows a certain kind of Pokémon to evolve.");
                item.Sprite = "Fang_LightBlue";
            }
            else if (ii == 376)
            {
                item.Name = new LocalText("Deep Sea Scale");
                item.Desc = new LocalText("A pretty scale that shines faintly. It allows a certain kind of Pokémon to evolve.");
                item.Sprite = "Scale_Pink";
            }
            else if (ii == 377)
            {
                item.Name = new LocalText("Sun Ribbon");
                item.Desc = new LocalText("A ribbon infused with sunshine. It allows a certain kind of Pokémon to evolve.");
                item.Sprite = "Scarf_Yellow";
            }
            else if (ii == 378)
            {
                item.Name = new LocalText("Lunar Ribbon");
                item.Desc = new LocalText("A ribbon infused with moonlight. It allows a certain kind of Pokémon to evolve.");
                item.Sprite = "Scarf_Blue";
            }
            else if (ii == 379)
            {
                item.Name = new LocalText("Ice Stone");
                item.Desc = new LocalText("A peculiar stone that can make certain species of Pokémon evolve. It has an unmistakable snowflake pattern. If held, it changes the Pokémon's regular attacks to Ice-type.");
                item.Sprite = "Stone_LightBlue";
                item.OnActions.Add(-1, new RegularAttackNeededEvent(new ChangeMoveElementEvent("none", "ice")));
            }
            else if (ii == 380)
            {
                item.Name = new LocalText("Insect Plate");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a stone tablet that halves the damage of Bug-type moves.");
                item.Sprite = "Slab_Gray";
                item.Price = 250;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                item.BeforeBeingHits.Add(0, new MultiplyElementEvent("bug", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 381)
            {
                item.Name = new LocalText("Dread Plate");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a stone tablet that halves the damage of Dark-type moves.");
                item.Sprite = "Slab_Gray";
                item.Price = 250;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                item.BeforeBeingHits.Add(0, new MultiplyElementEvent("dark", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 382)
            {
                item.Name = new LocalText("Draco Plate");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a stone tablet that halves the damage of Dragon-type moves.");
                item.Sprite = "Slab_Gray";
                item.Price = 250;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                item.BeforeBeingHits.Add(0, new MultiplyElementEvent("dragon", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 383)
            {
                item.Name = new LocalText("Zap Plate");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a stone tablet that halves the damage of Electric-type moves.");
                item.Sprite = "Slab_Gray";
                item.Price = 250;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                item.BeforeBeingHits.Add(0, new MultiplyElementEvent("electric", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 384)
            {
                item.Name = new LocalText("Pixie Plate");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a stone tablet that halves the damage of Fairy-type moves.");
                item.Sprite = "Slab_Gray";
                item.Price = 250;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                item.BeforeBeingHits.Add(0, new MultiplyElementEvent("fairy", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 385)
            {
                item.Name = new LocalText("Fist Plate");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a stone tablet that halves the damage of Fighting-type moves.");
                item.Sprite = "Slab_Gray";
                item.Price = 250;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                item.BeforeBeingHits.Add(0, new MultiplyElementEvent("fighting", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 386)
            {
                item.Name = new LocalText("Flame Plate");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a stone tablet that halves the damage of Fire-type moves.");
                item.Sprite = "Slab_Gray";
                item.Price = 250;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                item.BeforeBeingHits.Add(0, new MultiplyElementEvent("fire", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 387)
            {
                item.Name = new LocalText("Sky Plate");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a stone tablet that halves the damage of Flying-type moves.");
                item.Sprite = "Slab_Gray";
                item.Price = 250;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                item.BeforeBeingHits.Add(0, new MultiplyElementEvent("flying", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 388)
            {
                item.Name = new LocalText("Spooky Plate");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a stone tablet that halves the damage of Ghost-type moves.");
                item.Sprite = "Slab_Gray";
                item.Price = 250;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                item.BeforeBeingHits.Add(0, new MultiplyElementEvent("ghost", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 389)
            {
                item.Name = new LocalText("Meadow Plate");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a stone tablet that halves the damage of Grass-type moves.");
                item.Sprite = "Slab_Gray";
                item.Price = 250;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                item.BeforeBeingHits.Add(0, new MultiplyElementEvent("grass", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 390)
            {
                item.Name = new LocalText("Earth Plate");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a stone tablet that halves the damage of Ground-type moves.");
                item.Sprite = "Slab_Gray";
                item.Price = 250;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                item.BeforeBeingHits.Add(0, new MultiplyElementEvent("ground", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 391)
            {
                item.Name = new LocalText("Icicle Plate");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a stone tablet that halves the damage of Ice-type moves.");
                item.Sprite = "Slab_Gray";
                item.Price = 250;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                item.BeforeBeingHits.Add(0, new MultiplyElementEvent("ice", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 392)
            {
                item.Name = new LocalText("Blank Plate");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a stone tablet that halves the damage of Normal-type moves.");
                item.Sprite = "Slab_Gray";
                item.Price = 250;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                item.BeforeBeingHits.Add(0, new MultiplyElementEvent("normal", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 393)
            {
                item.Name = new LocalText("Toxic Plate");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a stone tablet that halves the damage of Poison-type moves.");
                item.Sprite = "Slab_Gray";
                item.Price = 250;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                item.BeforeBeingHits.Add(0, new MultiplyElementEvent("poison", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 394)
            {
                item.Name = new LocalText("Mind Plate");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a stone tablet that halves the damage of Psychic-type moves.");
                item.Sprite = "Slab_Gray";
                item.Price = 250;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                item.BeforeBeingHits.Add(0, new MultiplyElementEvent("psychic", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 395)
            {
                item.Name = new LocalText("Stone Plate");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a stone tablet that halves the damage of Rock-type moves.");
                item.Sprite = "Slab_Gray";
                item.Price = 250;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                item.BeforeBeingHits.Add(0, new MultiplyElementEvent("rock", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 396)
            {
                item.Name = new LocalText("Iron Plate");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a stone tablet that halves the damage of Steel-type moves.");
                item.Sprite = "Slab_Gray";
                item.Price = 250;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                item.BeforeBeingHits.Add(0, new MultiplyElementEvent("steel", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 397)
            {
                item.Name = new LocalText("Splash Plate");
                item.Desc = new LocalText("An item to be held by a Pokémon. It's a stone tablet that halves the damage of Water-type moves.");
                item.Sprite = "Slab_Gray";
                item.Price = 250;
                SingleEmitter emitter = new SingleEmitter(new AnimData("Screen_RSE_Gray", 1, -1, -1, 192));
                item.BeforeBeingHits.Add(0, new MultiplyElementEvent("water", 1, 2, false, new BattleAnimEvent(emitter, "DUN_Screen_Hit", true, 10)));
            }
            else if (ii == 400)
            {
                item.Name = new LocalText("Power Band");
                item.Desc = new LocalText("A hold item that slightly boosts the Pokémon's Attack.");
                item.Sprite = "Band_Tan";
                item.Price = 120;
                item.OnActions.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Physical, 11, 10));
            }
            else if (ii == 401)
            {
                item.Name = new LocalText("Special Band");
                item.Desc = new LocalText("A hold item that slightly boosts the Pokémon's Special Attack.");
                item.Sprite = "Band_Pink";
                item.Price = 120;
                item.OnActions.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Magical, 11, 10));
            }
            else if (ii == 402)
            {
                item.Name = new LocalText("Defense Scarf");
                item.Desc = new LocalText("A hold item that slightly boosts the Pokémon's Defense.");
                item.Sprite = "Scarf_Green";
                item.Price = 120;
                item.BeforeBeingHits.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Physical, 9, 10));
            }
            else if (ii == 403)
            {
                item.Name = new LocalText("Zinc Band");
                item.Desc = new LocalText("A hold item that slightly boosts the Pokémon's Special Defense.");
                item.Sprite = "Band_Gray";
                item.Price = 120;
                item.BeforeBeingHits.Add(0, new MultiplyCategoryEvent(BattleData.SkillCategory.Magical, 9, 10));
            }
            else if (ii == 404)
            {
                item.Name = new LocalText("Big Root");
                item.Desc = new LocalText("A held item that increases HP recovery.");
                item.Sprite = "Thorn_Brown";
                item.Price = 200;
                item.RestoreHPs.Add(0, new HealMultEvent(3, 2));
            }
            else if (ii == 405)
            {
                item.Name = new LocalText("Expert Belt");
                item.Desc = new LocalText("A well-worn belt that boosts the power of super-effective moves.");
                item.Sprite = "Belt_Black";
                item.Price = 200;
                item.BeforeHittings.Add(1, new MultiplyEffectiveEvent(false, 5, 4));
            }
            else if (ii == 406)
            {
                item.Name = new LocalText("Weather Rock");
                item.Desc = new LocalText("An item that increases the duration of weather moves used by the holder.");
                item.Sprite = "Ore_White";
                item.Price = 200;
                item.OnRefresh.Add(0, new MiscEvent(new ExtendWeatherState()));
            }
            else if (ii == 407)
            {
                item.Name = new LocalText("Goggle Specs");
                item.Desc = new LocalText("A held item that enables the Pokémon to identify traps on the floor.");
                item.Sprite = "Specs_Red";
                item.Price = 200;
                item.OnRefresh.Add(0, new SeeTrapsEvent());
            }
            else if (ii == 408)
            {
                //item.Name = new LocalText("Lure Band");
                //item.Desc = new LocalText("A band that may pull the target to the holder when it successfully inflicts damage.");
                //item.Sprite = "Band_Tan";
                //item.Price = 200;
                //item.AfterHittings.Add(0, new OnMoveUseEvent(new OnHitEvent(true, false, 25, new LureEvent())));
            }
            else if (ii == 444)
            {
                //444      ***    Light Box - 1* items
                item.Name = new LocalText("Light Box");
                item.Desc = new LocalText("A box that is somewhat light. Specific Pokémon adore the items inside.");
                item.Sprite = "Chest_Blue";
            }
            else if (ii == 445)
            {
                //445      ***    Heavy Box - 2* items
                item.Name = new LocalText("Heavy Box");
                item.Desc = new LocalText("A box that is unbelievably heavy. Specific Pokémon adore the items inside.");
                item.Sprite = "Chest_Blue";
            }
            else if (ii == 446)
            {
                //446      ***    Nifty Box - all high tier TMs, ability capsule, heart scale 9, max potion, full heal, max elixir
                item.Name = new LocalText("Nifty Box");
                item.Desc = new LocalText("A box that is completely impressive. There is something mysterious inside.");
                item.Sprite = "Chest_Red";
            }
            else if (ii == 447)
            {
                //447      ***    Dainty Box - Stat ups, wonder gummi, nectar, golden apple, golden banana
                item.Name = new LocalText("Dainty Box");
                item.Desc = new LocalText("A box that is dainty and fashionable. There is something tasty inside!");
                item.Sprite = "Chest_Red";
            }
            else if (ii == 448)
            {
                //448      ***    Glittery Box - golden apple, amber tear, golden banana, nugget, golden thorn 9
                item.Name = new LocalText("Glittery Box");
                item.Desc = new LocalText("A box that glitters with dazzling light. Whatever's inside is sure to sell for a high price!");
                item.Sprite = "Chest_Gold";
            }
            else if (ii == 449)
            {
                //449      ***    Deluxe Box - Legendary exclusive items, harmony scarf, golden items, stat ups, wonder gummi, perfect apricorn, max potion/full heal/max elixir
                item.Name = new LocalText("Deluxe Box");
                item.Desc = new LocalText("A box that is extravagant and opulent. It looks like something is inside.");
                item.Sprite = "Chest_Gold";
            }
            else if (ii == 450)
            {
                item.Name = new LocalText("Recall Box");
                item.Desc = new LocalText("A marvelous box that lets the user recall any move they've learned in the past.");
                item.Sprite = "Machine_Blue";
                item.Icon = 12;
                item.UsageType = ItemData.UseType.UseOther;
                item.ItemStates.Set(new MachineState());
                RecallBoxEvent action = new RecallBoxEvent(true);
                item.GroundUseActions.Add(action);
                item.GroundUseActions[0].Selection = SelectionType.Others;
                item.GroundUseActions[0].GroundUsageType = ItemData.UseType.UseOther;
                item.Price = 800;
                item.UseEvent.BeforeTryActions.Add(1, new LinkBoxEvent());
                item.UseEvent.OnHits.Add(0, new MoveLearnEvent());
                item.UseEvent.OnHits.Add(0, new MoveDeleteEvent());
            }
            else if (ii == 451)
            {
                item.Name = new LocalText("Assembly Box");
                item.Desc = new LocalText("A marvelous box that lets the user add a Pokémon from the assembly to the team.");
                item.Sprite = "Machine_Green";
                item.Icon = 12;
                item.UsageType = ItemData.UseType.Use;
                item.ItemStates.Set(new UtilityState());
                item.Price = 800;
                item.UseEvent.BeforeTryActions.Add(1, new AssemblyBoxEvent());
                item.UseEvent.OnHits.Add(0, new WithdrawRecruitEvent());
                item.UseAction = new SelfAction();
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
            }
            else if (ii == 452)
            {
                item.Name = new LocalText("Storage Box");
                item.Desc = new LocalText("A marvelous box that lets the user add any item from storage into the Treasure Bag.");
                item.Sprite = "Machine_Gray";
                item.Icon = 12;
                item.UsageType = ItemData.UseType.Use;
                item.ItemStates.Set(new UtilityState());
                item.Price = 800;
                item.UseEvent.BeforeTryActions.Add(1, new StorageBoxEvent());
                item.UseEvent.OnHits.Add(0, new WithdrawItemEvent());
                item.UseAction = new SelfAction();
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
            }
            else if (ii == 453)
            {
                item.Name = new LocalText("Ability Capsule");
                item.Desc = new LocalText("A capsule that allows a Pokémon to change its Abilities when used.");
                item.Sprite = "Ability_Capsule";
                item.Icon = 12;
                item.UsageType = ItemData.UseType.UseOther;
                item.ItemStates.Set(new MachineState());
                item.GroundUseActions.Add(new AbilityCapsuleItemEvent());
                item.GroundUseActions[0].Selection = SelectionType.Others;
                item.GroundUseActions[0].GroundUsageType = ItemData.UseType.UseOther;
                item.Price = 800;
                item.UseEvent.BeforeTryActions.Add(1, new AbilityCapsuleEvent());
                item.UseEvent.OnHits.Add(0, new AbilityLearnEvent());
                //item.UseEffect.HitEffects.Add(0, new AbilityDeleteEffect());
            }
            else if (ii == 454)
            {
                item.Name = new LocalText("Grimy Food");
                fileName = "food_grimy";
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly. However, it will also inflict a variety of status problems because it's covered in filthy grime. Be careful of what you eat!");
                item.Sprite = "Rock_Purple";
                item.SortCategory = 4;
                item.Icon = 16;
                item.UsageType = ItemData.UseType.Eat;
                item.ItemStates.Set(new EdibleState());
                item.ItemStates.Set(new FoodState());
                item.Price = 1;
                item.UseEvent.OnHits.Add(0, new RestoreBellyEvent(30, false));
                item.UseEvent.OnHits.Add(0, new ChooseOneEvent(new StatusBattleEvent("sleep", true, false), new MultiBattleEvent(new StatusStackBattleEvent("mod_defense", true, false, -3), new StatusStackBattleEvent("mod_special_defense", true, false, -3)), new StatusBattleEvent("burn", true, false), new StatusBattleEvent("poison_toxic", true, false)));
                item.UseAction = new SelfAction();
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
            }
            else if (ii == 455)
            {
                item.Name = new LocalText("Key");
                fileName = "key";
                item.Desc = new LocalText("A key that unlocks a chest or door inside a dungeon.");
                item.Sprite = "Key_White";
                item.Icon = 18;
                item.UsageType = ItemData.UseType.Use;
                item.ItemStates.Set(new UtilityState());
                item.SortCategory = 13;
                item.Price = 10;
                item.MaxStack = 3;
                item.UseEvent.BeforeTryActions.Add(1, new KeyCheckEvent());
                item.UseEvent.OnHitTiles.Add(0, new KeyUnlockEvent());
                item.UseAction = new AttackAction();
                ((AttackAction)item.UseAction).CharAnimData = new CharAnimFrameType(05);//Attack
                ((AttackAction)item.UseAction).HitTiles = true;
                ((AttackAction)item.UseAction).WideAngle = AttackCoverage.FrontAndCorners;
                ((AttackAction)item.UseAction).Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 2));
                item.UseAction.ActionFX.Sound = "EVT_Chest_Click";//or 653
            }
            else if (ii == 456)
            {
                item.Name = new LocalText("**Ancient Key");
                fileName = "key_ancient";
                item.Desc = new LocalText("A key that unlocks an ancient door inside a dungeon.");
                item.Sprite = "Key_Brown";
                item.Icon = 18;
                item.UsageType = ItemData.UseType.Use;
                item.ItemStates.Set(new UtilityState());
                item.Price = 1000;
            }
            else if (ii == 458)
            {
                item.Name = new LocalText("Foul Gummi");
                fileName = "gummi_foul";
                item.Desc = new LocalText("A food item that somewhat fills the Pokémon's belly. However, it will also reduce the Pokémon's level by 1.");
                item.Sprite = "Gummi_Black";
                item.SortCategory = 4;
                item.Icon = 16;
                item.UsageType = ItemData.UseType.Eat;
                item.ItemStates.Set(new EdibleState());
                item.ItemStates.Set(new FoodState());
                item.Price = 1000;
                item.UseEvent.OnHits.Add(0, new RestoreBellyEvent(25, false));
                item.UseEvent.OnHits.Add(0, new LevelChangeEvent(-1));
                item.UseAction = new SelfAction();
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
            }
            else if (ii == 477)
            {
                item.Name = new LocalText("Nugget");
                item.Desc = new LocalText("A nugget of pure gold. It sells for a very high price.");
                item.Sprite = "Rock_Yellow";
                item.Price = 5000;
            }
            else if (ii == 478)
            {
                item.Name = new LocalText("Prison Bottle");
                item.Desc = new LocalText("A bottle believed to have been used to seal away the power of a certain Pokémon long, long ago.");
                item.Sprite = "Bottle_Purple";
                item.UsageType = ItemData.UseType.UseOther;
                item.Price = 800000;
                item.MaxStack = -1;
                item.UseEvent.BeforeTryActions.Add(0, new CheckItemActiveEvent());
                item.UseEvent.BeforeTryActions.Add(1, new FormChoiceEvent("hoopa"));
                item.UseEvent.OnHits.Add(0, new SwitchFormEvent());
                item.UseEvent.OnHits.Add(0, new DeactivateItemEvent());
                item.UseAction = new SelfAction();
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;

                BattleFX itemFX = new BattleFX();
                itemFX.Sound = "DUN_Wonder_Tile";
                itemFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 2));
                itemFX.Delay = 20;
                item.UseAction.PreActions.Add(itemFX);

                item.UseEvent.HitFX.Delay = 20;
                item.UseEvent.HitFX.Sound = "_UNK_EVT_043";
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Materialize", 2));
            }
            else if (ii == 479)
            {
                item.Name = new LocalText("**Berserk Gene");
                item.Sprite = "Rock_Yellow";
                item.Price = 999999;
            }
            else if (ii == 480)
            {
                item.Name = new LocalText("Pearl");
                item.Desc = new LocalText("A rather small pearl that has a very nice silvery sheen to it. It sells for a moderately high price.");
                item.Sprite = "Pearl";
                item.Price = 500;
                item.MaxStack = 9;
            }
            else if (ii == 481)
            {
                item.Name = new LocalText("Heart Scale");
                item.Desc = new LocalText("A pretty, heart-shaped scale that is extremely rare. It glows faintly with all of the colors of the rainbow.");
                item.Sprite = "Heart_Pink";
                item.Price = 50;
                item.MaxStack = 9;
            }
            else if (ii == 482)
            {
                item.Name = new LocalText("**Red Orb");
                item.Price = 700000;
            }
            else if (ii == 483)
            {
                item.Name = new LocalText("**Blue Orb");
                item.Price = 700000;
            }
            else if (ii == 484)
            {
                item.Name = new LocalText("**Green Orb");
                item.Price = 700000;
            }
            else if (ii == 485)
            {
                item.Name = new LocalText("Comet Shard");
                item.Desc = new LocalText("A shard that fell to the ground when a comet passed nearby. It sells for an extremely high price.");
                item.Sprite = "Crystal_Blue";
                item.Price = 60000;
            }
            else if (ii == 486)
            {
                item.Name = new LocalText("Meteorite");
                item.Desc = new LocalText("A meteorite that fell from space long ago. It can be used on Deoxys once per floor to cause a Forme change.");
                item.Sprite = "Rock_Blue";
                item.UsageType = ItemData.UseType.UseOther;
                item.Price = 400000;
                item.MaxStack = -1;
                item.UseEvent.BeforeTryActions.Add(0, new CheckItemActiveEvent());
                item.UseEvent.BeforeTryActions.Add(1, new FormChoiceEvent("deoxys"));
                item.UseEvent.OnHits.Add(0, new SwitchFormEvent());
                item.UseEvent.OnHits.Add(0, new DeactivateItemEvent());
                item.UseAction = new SelfAction();
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;

                BattleFX itemFX = new BattleFX();
                itemFX.Sound = "DUN_Wonder_Tile";
                itemFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 2));
                itemFX.Delay = 20;
                item.UseAction.PreActions.Add(itemFX);

                item.UseEvent.HitFX.Delay = 20;
                item.UseEvent.HitFX.Sound = "_UNK_EVT_043";
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Materialize", 2));
            }
            else if (ii == 487)
            {
                item.Name = new LocalText("**Adamant Orb");
                item.Price = 800000;
            }
            else if (ii == 488)
            {
                item.Name = new LocalText("**Lustrous Orb");
                item.Price = 800000;
            }
            else if (ii == 489)
            {
                item.Name = new LocalText("Griseous Orb");
                item.Desc = new LocalText("A glowing orb can be used on Giratina once per floor to cause a Forme change.");
                item.Sprite = "Orb_Tan";
                item.UsageType = ItemData.UseType.UseOther;
                item.Price = 800000;
                item.MaxStack = -1;
                item.UseEvent.BeforeTryActions.Add(0, new CheckItemActiveEvent());
                item.UseEvent.BeforeTryActions.Add(1, new FormChoiceEvent("giratina"));
                item.UseEvent.OnHits.Add(0, new SwitchFormEvent());
                item.UseEvent.OnHits.Add(0, new DeactivateItemEvent());
                item.UseAction = new SelfAction();
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;

                BattleFX itemFX = new BattleFX();
                itemFX.Sound = "DUN_Wonder_Tile";
                itemFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 2));
                itemFX.Delay = 20;
                item.UseAction.PreActions.Add(itemFX);

                item.UseEvent.HitFX.Delay = 20;
                item.UseEvent.HitFX.Sound = "_UNK_EVT_043";
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Materialize", 2));
            }
            else if (ii == 490)
            {
                item.Name = new LocalText("**Spacetime Globe");
                item.Price = 1000000;
            }
            else if (ii == 491)
            {
                item.Name = new LocalText("Gracidea");
                item.Desc = new LocalText("A beautiful flower that blooms in the Secret Garden. It can be used on Shaymin once per floor to cause a Forme change.");
                item.Sprite = "Gracidea";
                item.UsageType = ItemData.UseType.UseOther;
                item.Price = 50000;
                item.MaxStack = -1;
                item.UseEvent.BeforeTryActions.Add(0, new CheckItemActiveEvent());
                item.UseEvent.BeforeTryActions.Add(1, new FormChoiceEvent("shaymin"));
                item.UseEvent.OnHits.Add(0, new SwitchFormEvent());
                item.UseEvent.OnHits.Add(0, new DeactivateItemEvent());
                item.UseAction = new SelfAction();
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;

                BattleFX itemFX = new BattleFX();
                itemFX.Sound = "DUN_Wonder_Tile";
                itemFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 2));
                itemFX.Delay = 20;
                item.UseAction.PreActions.Add(itemFX);

                item.UseEvent.HitFX.Delay = 20;
                item.UseEvent.HitFX.Sound = "_UNK_EVT_043";
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Materialize", 2));
            }
            else if (ii == 492)
            {
                item.Name = new LocalText("Mystery Egg");
                fileName = "egg_mystery";
                item.Sprite = "Egg_Sea";
                item.Desc = new LocalText("An egg with bizarre colors that have never been seen before. What could this egg be?");
            }
            else if (ii == 493)
            {
                item.Name = new LocalText("**Secret Slab");
                item.Desc = new LocalText("An ancient stone slab inscribed with what seems to be prehistoric legend, rumored to hold an incredible secret. Its writings change depending on the dungeon it's used in.");
                // Lua script: Will list out all restrictions the player needs to abide by to get to the dungeon's golden chamber, also keep track of which restrictions are met?
                // If the dungeon doesn't have a golden chamber, just say it's blank.
                item.Price = 80000;
            }
            else if (ii == 494)
            {
                item.Name = new LocalText("Music Box");
                item.Desc = new LocalText("An enchanting music box that plays a beautiful melody. It is said to draw something special to it when it is kept close by.");
                // This will trigger legendaries to appear.
                // when they do appear, the music will change to a music box tune.
                // they will never spawn at the start of the dungeon, or when the wind timer is visible.
                // they have a chance to spawn at 100 turns into the floor and no more
                item.Price = 80000;
            }
            else if (ii == 495)
            {
                item.Name = new LocalText("**Mystery Part");
                item.Desc = new LocalText("A mysterious mechanical part that has lain undisturbed for eons, veiled by legendary mystery. It is said to have an effect on mysterious distortions.");
                // This will prevent mysterious distortions from occurring, and cause them to flood out when taken away
                item.Sprite = "Piece_Yellow";
                item.Price = 40000;
            }
            else if (ii == 545)
            {
                item.Name = new LocalText("Special Ingredient");
                item.Comment = "Do not Translate";
                fileName = "lost_item_bug";
                item.Sprite = "Flower_Blue";
                item.Desc = new LocalText("A mission item.");
            }
            else if (ii == 546)
            {
                item.Name = new LocalText("Lost Satchel");
                item.Comment = "Do not Translate";
                fileName = "lost_item_dark";
                item.Sprite = "Flower_Blue";
                item.Desc = new LocalText("A mission item.");
            }
            else if (ii == 547)
            {
                item.Name = new LocalText("Lost Scarf");
                item.Comment = "Do not Translate";
                fileName = "lost_item_ground";
                item.Sprite = "Flower_Blue";
                item.Desc = new LocalText("A mission item.");
            }
            else if (ii == 548)
            {
                item.Name = new LocalText("Herba Mystica");
                item.Comment = "Do not Translate";
                fileName = "lost_item_grass";
                item.Sprite = "Flower_Blue";
                item.Desc = new LocalText("A mission item.");
            }
            else if (ii == 549)
            {
                item.Name = new LocalText("Not-So-Normal Gem");
                item.Comment = "Do not Translate";
                fileName = "lost_item_normal";
                item.Sprite = "Flower_Blue";
                item.Desc = new LocalText("A mission item.");
            }
            else if (ii == 576)
                FillTMData(item, "earthquake");
            else if (ii == 577)
                FillTMData(item, "hyper_beam");
            else if (ii == 578)
                FillTMData(item, "overheat");
            else if (ii == 579)
                FillTMData(item, "blizzard");
            else if (ii == 580)
                FillTMData(item, "swords_dance");
            else if (ii == 581)
                FillTMData(item, "surf");
            else if (ii == 582)
                FillTMData(item, "dark_pulse");
            else if (ii == 583)
                FillTMData(item, "psychic");
            else if (ii == 584)
                FillTMData(item, "thunder");
            else if (ii == 585)
                FillTMData(item, "shadow_ball");
            else if (ii == 586)
                FillTMData(item, "ice_beam");
            else if (ii == 587)
                FillTMData(item, "giga_impact");
            else if (ii == 588)
                FillTMData(item, "fire_blast");
            else if (ii == 589)
                FillTMData(item, "dazzling_gleam");
            else if (ii == 590)
                FillTMData(item, "flash_cannon");
            else if (ii == 591)
                FillTMData(item, "stone_edge");
            else if (ii == 592)
                FillTMData(item, "sludge_bomb");
            else if (ii == 593)
                FillTMData(item, "focus_blast");
            else if (ii == 594)
                FillTMData(item, "explosion");
            else if (ii == 595)
                FillTMData(item, "snatch");
            else if (ii == 596)
                FillTMData(item, "sunny_day");
            else if (ii == 597)
                FillTMData(item, "rain_dance");
            else if (ii == 598)
                FillTMData(item, "sandstorm");
            else if (ii == 599)
                FillTMData(item, "hail");
            else if (ii == 600)
                FillTMData(item, "x_scissor");
            else if (ii == 601)
                FillTMData(item, "wild_charge");
            else if (ii == 602)
                FillTMData(item, "taunt");
            else if (ii == 603)
                FillTMData(item, "focus_punch");
            else if (ii == 604)
                FillTMData(item, "safeguard");
            else if (ii == 605)
                FillTMData(item, "light_screen");
            else if (ii == 606)
                FillTMData(item, "psyshock");
            else if (ii == 607)
                FillTMData(item, "will_o_wisp");
            else if (ii == 608)
                FillTMData(item, "dream_eater");
            else if (ii == 609)
                FillTMData(item, "nature_power");
            else if (ii == 610)
                FillTMData(item, "facade");
            else if (ii == 611)
                FillTMData(item, "swagger");
            else if (ii == 612)
                FillTMData(item, "captivate");
            else if (ii == 613)
                FillTMData(item, "rock_slide");
            else if (ii == 614)
                FillTMData(item, "fling");
            else if (ii == 615)
                FillTMData(item, "thunderbolt");
            else if (ii == 616)
                FillTMData(item, "water_pulse");
            else if (ii == 617)
                FillTMData(item, "shock_wave");
            else if (ii == 618)
                FillTMData(item, "brick_break");
            else if (ii == 619)
                FillTMData(item, "payback");
            else if (ii == 620)
                FillTMData(item, "calm_mind");
            else if (ii == 621)
                FillTMData(item, "reflect");
            else if (ii == 622)
                FillTMData(item, "charge_beam");
            else if (ii == 623)
                FillTMData(item, "flamethrower");
            else if (ii == 624)
                FillTMData(item, "energy_ball");
            else if (ii == 625)
                FillTMData(item, "retaliate");
            else if (ii == 626)
                FillTMData(item, "scald");
            else if (ii == 627)
                FillTMData(item, "waterfall");
            else if (ii == 628)
                FillTMData(item, "roost");
            else if (ii == 629)
                FillTMData(item, "rock_polish");
            else if (ii == 630)
                FillTMData(item, "acrobatics");
            else if (ii == 631)
                FillTMData(item, "rock_climb");
            else if (ii == 632)
                FillTMData(item, "bulk_up");
            else if (ii == 633)
                FillTMData(item, "pluck");
            else if (ii == 634)
                FillTMData(item, "psych_up");
            else if (ii == 635)
                FillTMData(item, "secret_power");
            else if (ii == 636)
                FillTMData(item, "natural_gift");
            else if (ii == 637)
                FillTMData(item, "return");
            else if (ii == 638)
                FillTMData(item, "frustration");
            else if (ii == 639)
                FillTMData(item, "giga_drain");
            else if (ii == 640)
                FillTMData(item, "dive");
            else if (ii == 641)
                FillTMData(item, "poison_jab");
            else if (ii == 642)
                FillTMData(item, "torment");
            else if (ii == 643)
                FillTMData(item, "shadow_claw");
            else if (ii == 644)
                FillTMData(item, "endure");
            else if (ii == 645)
                FillTMData(item, "echoed_voice");
            else if (ii == 646)
                FillTMData(item, "gyro_ball");
            else if (ii == 647)
                FillTMData(item, "recycle");
            else if (ii == 648)
                FillTMData(item, "false_swipe");
            else if (ii == 649)
                FillTMData(item, "defog");
            else if (ii == 650)
                FillTMData(item, "telekinesis");
            else if (ii == 651)
                FillTMData(item, "double_team");
            else if (ii == 652)
                FillTMData(item, "thunder_wave");
            else if (ii == 653)
                FillTMData(item, "attract");
            else if (ii == 654)
                FillTMData(item, "steel_wing");
            else if (ii == 655)
                FillTMData(item, "smack_down");
            else if (ii == 656)
                FillTMData(item, "snarl");
            else if (ii == 657)
                FillTMData(item, "flame_charge");
            else if (ii == 658)
                FillTMData(item, "bulldoze");
            else if (ii == 659)
                FillTMData(item, "substitute");
            else if (ii == 660)
                FillTMData(item, "iron_tail");
            else if (ii == 661)
                FillTMData(item, "brine");
            else if (ii == 662)
                FillTMData(item, "venoshock");
            else if (ii == 663)
                FillTMData(item, "u_turn");
            else if (ii == 664)
                FillTMData(item, "aerial_ace");
            else if (ii == 665)
                FillTMData(item, "hone_claws");
            else if (ii == 666)
                FillTMData(item, "rock_smash");
            else if (ii == 667)
                FillTMData(item, "protect");
            else if (ii == 668)
                FillTMData(item, "round");
            else if (ii == 669)
                FillTMData(item, "rest");
            else if (ii == 670)
                FillTMData(item, "hidden_power");
            else if (ii == 671)
                FillTMData(item, "rock_tomb");
            else if (ii == 672)
                FillTMData(item, "strength");
            else if (ii == 673)
                FillTMData(item, "thief");
            else if (ii == 674)
                FillTMData(item, "dig");
            else if (ii == 675)
                FillTMData(item, "cut");
            else if (ii == 676)
                FillTMData(item, "whirlpool");
            else if (ii == 677)
                FillTMData(item, "grass_knot");
            else if (ii == 678)
                FillTMData(item, "fly");
            else if (ii == 679)
                FillTMData(item, "power_up_punch");
            else if (ii == 680)
                FillTMData(item, "infestation");
            else if (ii == 681)
                FillTMData(item, "work_up");
            else if (ii == 682)
                FillTMData(item, "incinerate");
            else if (ii == 683)
                FillTMData(item, "roar");
            else if (ii == 684)
                FillTMData(item, "flash");
            else if (ii == 685)
                FillTMData(item, "bullet_seed");
            else if (ii == 686)
                FillTMData(item, "embargo");
            else if (ii == 687)
                FillTMData(item, "dragon_claw");
            else if (ii == 688)
                FillTMData(item, "low_sweep");
            else if (ii == 689)
                FillTMData(item, "volt_switch");
            else if (ii == 690)
                FillTMData(item, "dragon_pulse");
            else if (ii == 691)
                FillTMData(item, "sludge_wave");
            else if (ii == 692)
                FillTMData(item, "struggle_bug");
            else if (ii == 693)
                FillTMData(item, "avalanche");
            else if (ii == 694)
                FillTMData(item, "drain_punch");
            else if (ii == 695)
                FillTMData(item, "dragon_tail");
            else if (ii == 696)
                FillTMData(item, "silver_wind");
            else if (ii == 697)
                FillTMData(item, "frost_breath");
            else if (ii == 698)
                FillTMData(item, "sky_drop");
            else if (ii == 699)
                FillTMData(item, "quash");


            if (item.Name.DefaultText.StartsWith("**"))
                item.Name.DefaultText = item.Name.DefaultText.Replace("*", "");
            else if (item.Name.DefaultText != "")
                item.Released = true;

            if (!String.IsNullOrWhiteSpace(item.Name.DefaultText))
            {
                if (ii < 1)
                    item.UsageType = ItemData.UseType.None;
                if (ii < 10)
                {
                    if (fileName == "")
                        fileName = "food_" + Text.Sanitize(ReverseWords(item.Name.DefaultText)).ToLower();
                    item.SortCategory = 4;
                    item.UsageType = ItemData.UseType.Eat;
                    item.ItemStates.Set(new EdibleState());
                    item.ItemStates.Set(new FoodState());
                    item.Icon = 2;
                }
                else if (ii < 75)
                {
                    if (fileName == "")
                        fileName = Text.Sanitize(ShiftNameWords(item.Name.DefaultText)).ToLower();
                    item.SortCategory = 6;
                    item.UsageType = ItemData.UseType.Eat;
                    item.ItemStates.Set(new EdibleState());
                    item.ItemStates.Set(new BerryState());
                    item.Icon = 3;
                    item.Price = 50;
                    item.UseEvent.OnHits.Add(0, new RestoreBellyEvent(3, false));
                }
                else if (ii < 100)
                {
                    if (fileName == "")
                        fileName = Text.Sanitize(ShiftNameWords(item.Name.DefaultText)).ToLower();
                    item.SortCategory = 5;
                    item.UsageType = ItemData.UseType.Eat;
                    item.ItemStates.Set(new EdibleState());
                    item.ItemStates.Set(new GummiState());
                    item.Icon = 6;
                    item.Price = 80;
                }
                else if (ii < 150)
                {
                    if (fileName == "")
                        fileName = Text.Sanitize(ShiftNameWords(item.Name.DefaultText)).ToLower();
                    item.SortCategory = 7;
                    item.UsageType = ItemData.UseType.Eat;
                    item.ItemStates.Set(new EdibleState());
                    item.ItemStates.Set(new SeedState());
                    item.Icon = 4;
                    item.Price = 50;
                    if (ii != 107)
                        item.UseEvent.OnHits.Add(0, new RestoreBellyEvent(3, false));
                }
                else if (ii < 200)
                {
                    if (ii >= 150 && ii < 160)//drinkables
                    {
                        if (fileName == "")
                            fileName = "boost_" + Text.Sanitize(item.Name.DefaultText).ToLower();
                        item.SortCategory = 9;
                        item.UsageType = ItemData.UseType.Drink;
                        item.ItemStates.Set(new EdibleState());
                        item.ItemStates.Set(new DrinkState());
                        item.Icon = 7;
                        item.Price = 750;
                        item.UseEvent.OnHits.Add(0, new RestoreBellyEvent(3, false));
                    }
                    else if (ii >= 160 && ii < 183)//manmade items
                    {
                        if (fileName == "")
                            fileName = "medicine_" + Text.Sanitize(item.Name.DefaultText).ToLower();
                        item.SortCategory = 10;
                        item.UsageType = ItemData.UseType.Use;
                        item.ItemStates.Set(new UtilityState());
                        item.Icon = 7;
                    }
                    else if (ii >= 183 && ii <= 185)//herbs
                    {
                        if (fileName == "")
                            fileName = Text.Sanitize(ShiftNameWords(item.Name.DefaultText)).ToLower();
                        item.SortCategory = 8;
                        item.UsageType = ItemData.UseType.Eat;
                        item.ItemStates.Set(new EdibleState());
                        item.ItemStates.Set(new HerbState());
                        item.Icon = 5;
                        item.UseEvent.OnHits.Add(0, new RestoreBellyEvent(3, false));
                    }
                }
                else if (ii < 209)//throwables
                {
                    if (fileName == "")
                        fileName = "ammo_" + Text.Sanitize(item.Name.DefaultText).ToLower();
                    item.SortCategory = 2;
                    item.UsageType = ItemData.UseType.Throw;
                    item.ItemStates.Set(new AmmoState());
                    item.MaxStack = 9;
                }
                else if (ii < 221)//apricorn
                {
                    if (fileName == "")
                        fileName = Text.Sanitize(ShiftNameWords(item.Name.DefaultText)).ToLower();
                    item.SortCategory = 8;
                    item.UsageType = ItemData.UseType.Throw;
                    item.ItemStates.Set(new RecruitState());
                    item.Icon = 11;
                    item.ArcThrow = true;
                    item.Explosion = new ExplosionData();
                    item.Explosion.TargetAlignments |= Alignment.Foe;
                }
                else if (ii < 250)//wands
                {
                    if (fileName == "")
                        fileName = Text.Sanitize(ShiftNameWords(item.Name.DefaultText)).ToLower();
                    item.SortCategory = 3;
                    item.UsageType = ItemData.UseType.Use;
                    item.ItemStates.Set(new WandState());
                    item.Icon = 8;
                    item.MaxStack = 9;
                }
                else if (ii < 300)//orbs
                {
                    if (fileName == "")
                        fileName = Text.Sanitize(ShiftNameWords(item.Name.DefaultText)).ToLower();
                    item.SortCategory = 11;
                    item.UsageType = ItemData.UseType.Use;
                    item.ItemStates.Set(new OrbState());
                    item.Icon = 9;
                    item.Price = 150;
                }
                else if (ii < 440)//held items
                {
                    //if (ii >= 331 && ii <= 348)
                    //{
                    //    //type boost
                    //    if (fileName == "")
                    //        fileName = "held_boost_" + Text.Sanitize(((ElementInfo.Element)ii-330).ToString()).ToLower();
                    //}
                    //else 
                    if (ii >= 349 && ii < 380)
                    {
                        //evo
                        if (fileName == "")
                            fileName = "evo_" + Text.Sanitize(item.Name.DefaultText).ToLower();
                        item.ItemStates.Set(new EvoState());
                        item.SortCategory = 14;
                    }
                    //else if (ii >= 380 && ii <= 397)
                    //{
                    //    if (fileName == "")
                    //        fileName = "held_plate_" + Text.Sanitize(((ElementInfo.Element)ii - 379).ToString()).ToLower();
                    //}
                    else
                    {
                        //ordinary held
                        if (fileName == "")
                            fileName = "held_" + Text.Sanitize(item.Name.DefaultText).ToLower();
                        item.ItemStates.Set(new EquipState());
                        item.SortCategory = 1;
                    }

                    item.UsageType = ItemData.UseType.None;
                    item.ItemStates.Set(new HeldState());
                    if (ii >= 315 && ii <= 327)//negative effect
                    {
                        item.OnEquips.Add(1, new AutoCurseItemEvent());
                    }
                    else if (ii > 350)
                        item.Price = 1000;
                }
                else if (ii < 450)//box
                {
                    if (fileName == "")
                        fileName = Text.Sanitize(ShiftNameWords(item.Name.DefaultText)).ToLower();
                    item.SortCategory = 16;
                    item.Icon = 17;
                    item.UsageType = ItemData.UseType.Box;
                    item.Price = 1000;
                }
                else if (ii < 540)//special
                {
                    if (ii < 454)
                    {
                        if (fileName == "")
                            fileName = "machine_" + Text.Sanitize(item.Name.DefaultText).ToLower();
                        item.SortCategory = 12;
                    }
                    else if (ii >= 477)
                    {
                        if (fileName == "")
                            fileName = "loot_" + Text.Sanitize(item.Name.DefaultText).ToLower();
                        item.SortCategory = 15;
                    }
                }
                else if (ii < 550)//mission items
                {
                    item.SortCategory = 18;
                }
                else if (ii < 700)//TM
                {
                    if (fileName == "")
                        fileName = Text.Sanitize(item.Name.DefaultText).ToLower();
                    item.SortCategory = 13;
                    item.Sprite = "Disc_Blue";
                    item.Icon = 13;
                    item.Price = 500;
                    item.UsageType = ItemData.UseType.Learn;
                }
            }

            if (item.ItemStates.Contains<FoodState>())
            {
                if (ii < 6)
                {
                    item.UseAction = new SelfAction();
                    item.UseAction.TargetAlignments |= Alignment.Self;
                    item.Explosion.TargetAlignments |= Alignment.Self;
                }
                else if (ii < 9)
                {
                    item.UseAction = new AreaAction();
                    ((AreaAction)item.UseAction).Range = 5;
                    ((AreaAction)item.UseAction).Speed = 10;
                    item.UseAction.TargetAlignments |= Alignment.Self;
                    item.Explosion.TargetAlignments |= Alignment.Self;
                    item.UseAction.TargetAlignments |= Alignment.Friend;
                    item.Explosion.TargetAlignments |= Alignment.Friend;
                }
                //sound and animation go here
                item.UseEvent.HitFX.Sound = "DUN_Seed";
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 2));
                item.UseEvent.HitFX.Delay = 15;

            }
            else if (item.ItemStates.Contains<BerryState>() || item.ItemStates.Contains<SeedState>() || item.ItemStates.Contains<HerbState>())
            {
                item.UseAction = new SelfAction();
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseEvent.HitFX.Delay = 15;
                item.UseEvent.HitFX.Sound = "DUN_Berry";
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 2));
            }
            else if (item.ItemStates.Contains<GummiState>())
            {
                item.UseAction = new SelfAction();
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseEvent.HitFX.Delay = 15;
                item.UseEvent.HitFX.Sound = "DUN_Gummi";
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 2));
            }
            else if (item.ItemStates.Contains<DrinkState>())
            {
                item.UseAction = new SelfAction();
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                item.UseEvent.HitFX.Delay = 15;
                item.UseEvent.HitFX.Sound = "DUN_Drink";
                item.UseEvent.HitFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 2));
            }
            else if (item.ItemStates.Contains<WandState>())
            {
                if (ii < 249)
                {
                    item.UseAction = new ProjectileAction();
                    ((ProjectileAction)item.UseAction).CharAnimData = new CharAnimFrameType(42);//Rotate
                    ((ProjectileAction)item.UseAction).Range = CharAction.MAX_RANGE;
                    ((ProjectileAction)item.UseAction).Speed = 12;
                    ((ProjectileAction)item.UseAction).StopAtHit = true;
                    if (ii != 234)
                        ((ProjectileAction)item.UseAction).StopAtWall = true;
                    ((ProjectileAction)item.UseAction).Anim = new AnimData("Confuse_Ray", 2);
                    item.UseAction.TargetAlignments = (Alignment.Foe | Alignment.Friend | Alignment.Self);
                    item.Explosion.TargetAlignments = (Alignment.Foe | Alignment.Friend | Alignment.Self);
                    BattleFX itemFX = new BattleFX();
                    itemFX.Sound = "DUN_Throw_Start";
                    item.UseAction.PreActions.Add(itemFX);
                    item.UseAction.ActionFX.Sound = "DUN_Blowback_Orb";
                }
            }
            else if (item.ItemStates.Contains<OrbState>())
            {
                //for orbs, TMs, etc,
                //can't have the spin around motion + animation for now.
                //need a charge step or something else to sort it all out.
                BattleFX itemFX = new BattleFX();
                itemFX.Sound = "DUN_Move_Start";
                itemFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                itemFX.Delay = 10;
                item.UseAction.PreActions.Add(itemFX);
            }
            else if (item.UsageType == ItemData.UseType.Learn || item.ItemStates.Contains<MachineState>())
            {
                item.UseAction = new SelfAction();
                item.UseAction.TargetAlignments |= Alignment.Self;
                item.Explosion.TargetAlignments |= Alignment.Self;
                BattleFX itemFX = new BattleFX();
                itemFX.Sound = "DUN_TM";
                itemFX.Emitter = new SingleEmitter(new AnimData("Charge_Up", 3));
                item.UseAction.PreActions.Add(itemFX);
            }
            else if (item.ItemStates.Contains<UtilityState>())
            {
                if (ii < 455)
                {
                    BattleFX itemFX = new BattleFX();
                    itemFX.Sound = "DUN_Wonder_Tile";
                    itemFX.Emitter = new SingleEmitter(new AnimData("Circle_Small_Blue_In", 2));
                    item.UseAction.PreActions.Add(itemFX);
                }
            }

            return (fileName, item);
        }

        private static string ShiftNameWords(string name)
        {
            string[] args = name.Split();
            List<string> result = new List<string>();
            result.Add(args[args.Length - 1]);
            for (int ii = 0; ii < args.Length - 1; ii++)
                result.Add(args[ii]);

            return String.Join(' ', result.ToArray());
        }

        private static string ReverseWords(string name)
        {
            string[] args = name.Split();
            Array.Reverse(args);
            return String.Join(' ', args);
        }

        public static void FillTMData(ItemData item, string moveIndex)
        {
            SkillData move = DataManager.Instance.GetSkill(moveIndex);
            LocalText tmFormatName = new LocalText("TM {0}");
            LocalText tmFormatDesc = new LocalText("Teaches the move {0} to a Pokémon.");
            item.Name = LocalText.FormatLocalText(tmFormatName, move.Name);
            item.Desc = LocalText.FormatLocalText(tmFormatDesc, move.Name);
            item.GroundUseActions.Add(new LearnItemEvent(moveIndex));
            item.GroundUseActions[0].Selection = SelectionType.Self;
            item.GroundUseActions[0].GroundUsageType = ItemData.UseType.Learn;
            item.ItemStates.Set(new ItemIDState(moveIndex));
            item.UseEvent.BeforeTryActions.Add(1, new TMEvent());
            item.UseEvent.OnHits.Add(0, new MoveLearnEvent());
        }


        public static void AddExclItemData(bool translate)
        {
            for (int ii = MAX_NORMAL_ITEMS; ii < MAX_INIT_EXCL_ITEMS; ii++)
            {
                string exclElement = Text.Sanitize(((ElementInfo.Element)((ii - 700) / 4 + 1)).ToString()).ToLower();
                ItemData item = new ItemData();
                item.SortCategory = 16;
                item.UseEvent.Element = "none";


                string fileName = "";

                if (ii >= 700 && ii <= 771)
                {
                    string itemType = "globe";
                    if ((ii - 700) % 4 == 0)
                        itemType = "silk";
                    else if ((ii - 700) % 4 == 1)
                        itemType = "dust";
                    else if ((ii - 700) % 4 == 2)
                        itemType = "gem";
                    fileName = String.Format("xcl_element_{0}_{1}", exclElement, itemType);
                }

                if (ii == 700)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Green Silk", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Defense, Stat.MDef } }, exclElement, translate);
                else if (ii == 701)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Wonder Dust", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Attack, Stat.MAtk } }, exclElement, translate);
                else if (ii == 702)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Guard Gem", ExclusiveItemEffect.TypeSpeedBoost,
                        new object[] { }, exclElement, translate);
                else if (ii == 703)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Defend Globe", ExclusiveItemEffect.TypeGroupWeaknessReduce,
                        new object[] { "flying" }, exclElement, translate);
                else if (ii == 704)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Black Silk", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Defense, Stat.MDef } }, exclElement, translate);
                else if (ii == 705)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Dark Dust", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Attack, Stat.MAtk } }, exclElement, translate);
                else if (ii == 706)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Dark Gem", ExclusiveItemEffect.TypeSpeedBoost,
                        new object[] { }, exclElement, translate);
                else if (ii == 707)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Dusk Globe", ExclusiveItemEffect.TypeGroupWeaknessReduce,
                        new object[] { "bug" }, exclElement, translate);
                else if (ii == 708)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Royal Silk", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Defense, Stat.MDef } }, exclElement, translate);
                else if (ii == 709)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Dragon Dust", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Attack, Stat.MAtk } }, exclElement, translate);
                else if (ii == 710)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Dragon Gem", ExclusiveItemEffect.TypeSpeedBoost,
                        new object[] { }, exclElement, translate);
                else if (ii == 711)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Dragon Globe", ExclusiveItemEffect.TypeGroupWeaknessReduce,
                        new object[] { "dragon" }, exclElement, translate);
                else if (ii == 712)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Yellow Silk", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Defense, Stat.MDef } }, exclElement, translate);
                else if (ii == 713)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Thunder Dust", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Attack, Stat.MAtk } }, exclElement, translate);
                else if (ii == 714)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Thunder Gem", ExclusiveItemEffect.TypeSpeedBoost,
                        new object[] { }, exclElement, translate);
                else if (ii == 715)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Volt Globe", ExclusiveItemEffect.TypeGroupWeaknessReduce,
                        new object[] { "ground" }, exclElement, translate);
                else if (ii == 716)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Magenta Silk", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Defense, Stat.MDef } }, exclElement, translate);
                else if (ii == 717)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Fairy Dust", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Attack, Stat.MAtk } }, exclElement, translate);
                else if (ii == 718)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Fairy Gem", ExclusiveItemEffect.TypeSpeedBoost,
                        new object[] { }, exclElement, translate);
                else if (ii == 719)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Fairy Globe", ExclusiveItemEffect.TypeGroupWeaknessReduce,
                        new object[] { "poison" }, exclElement, translate);
                else if (ii == 720)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Orange Silk", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Defense, Stat.MDef } }, exclElement, translate);
                else if (ii == 721)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Courage Dust", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Attack, Stat.MAtk } }, exclElement, translate);
                else if (ii == 722)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Fight Gem", ExclusiveItemEffect.TypeSpeedBoost,
                        new object[] { }, exclElement, translate);
                else if (ii == 723)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Power Globe", ExclusiveItemEffect.TypeGroupWeaknessReduce,
                        new object[] { "fairy" }, exclElement, translate);
                else if (ii == 724)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Red Silk", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Defense, Stat.MDef } }, exclElement, translate);
                else if (ii == 725)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Fire Dust", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Attack, Stat.MAtk } }, exclElement, translate);
                else if (ii == 726)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Fiery Gem", ExclusiveItemEffect.TypeSpeedBoost,
                        new object[] { }, exclElement, translate);
                else if (ii == 727)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Fiery Globe", ExclusiveItemEffect.TypeGroupWeaknessReduce,
                        new object[] { "water" }, exclElement, translate);
                else if (ii == 728)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Sky Silk", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Defense, Stat.MDef } }, exclElement, translate);
                else if (ii == 729)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Sky Dust", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Attack, Stat.MAtk } }, exclElement, translate);
                else if (ii == 730)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Sky Gem", ExclusiveItemEffect.TypeSpeedBoost,
                        new object[] { }, exclElement, translate);
                else if (ii == 731)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Sky Globe", ExclusiveItemEffect.TypeGroupWeaknessReduce,
                        new object[] { "electric" }, exclElement, translate);
                else if (ii == 732)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Purple Silk", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Defense, Stat.MDef } }, exclElement, translate);
                else if (ii == 733)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Shady Dust", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Attack, Stat.MAtk } }, exclElement, translate);
                else if (ii == 734)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Shadow Gem", ExclusiveItemEffect.TypeSpeedBoost,
                        new object[] { }, exclElement, translate);
                else if (ii == 735)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Nether Globe", ExclusiveItemEffect.TypeGroupWeaknessReduce,
                        new object[] { "dark" }, exclElement, translate);
                else if (ii == 736)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Grass Silk", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Defense, Stat.MDef } }, exclElement, translate);
                else if (ii == 737)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Grass Dust", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Attack, Stat.MAtk } }, exclElement, translate);
                else if (ii == 738)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Grass Gem", ExclusiveItemEffect.TypeSpeedBoost,
                        new object[] { }, exclElement, translate);
                else if (ii == 739)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Soothe Globe", ExclusiveItemEffect.TypeGroupWeaknessReduce,
                        new object[] { "fire" }, exclElement, translate);
                else if (ii == 740)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Brown Silk", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Defense, Stat.MDef } }, exclElement, translate);
                else if (ii == 741)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Ground Dust", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Attack, Stat.MAtk } }, exclElement, translate);
                else if (ii == 742)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Earth Gem", ExclusiveItemEffect.TypeSpeedBoost,
                        new object[] { }, exclElement, translate);
                else if (ii == 743)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Terra Globe", ExclusiveItemEffect.TypeGroupWeaknessReduce,
                        new object[] { "ice" }, exclElement, translate);
                else if (ii == 744)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Clear Silk", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Defense, Stat.MDef } }, exclElement, translate);
                else if (ii == 745)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Icy Dust", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Attack, Stat.MAtk } }, exclElement, translate);
                else if (ii == 746)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Icy Gem", ExclusiveItemEffect.TypeSpeedBoost,
                        new object[] { }, exclElement, translate);
                else if (ii == 747)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Icy Globe", ExclusiveItemEffect.TypeGroupWeaknessReduce,
                        new object[] { "rock" }, exclElement, translate);
                else if (ii == 748)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "White Silk", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Defense, Stat.MDef } }, exclElement, translate);
                else if (ii == 749)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Normal Dust", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Attack, Stat.MAtk } }, exclElement, translate);
                else if (ii == 750)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "White Gem", ExclusiveItemEffect.TypeSpeedBoost,
                        new object[] { }, exclElement, translate);
                else if (ii == 751)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Joy Globe", ExclusiveItemEffect.TypeGroupWeaknessReduce,
                        new object[] { "fighting" }, exclElement, translate);
                else if (ii == 752)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Pink Silk", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Defense, Stat.MDef } }, exclElement, translate);
                else if (ii == 753)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Poison Dust", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Attack, Stat.MAtk } }, exclElement, translate);
                else if (ii == 754)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Poison Gem", ExclusiveItemEffect.TypeSpeedBoost,
                        new object[] { }, exclElement, translate);
                else if (ii == 755)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Poison Globe", ExclusiveItemEffect.TypeGroupWeaknessReduce,
                        new object[] { "psychic" }, exclElement, translate);
                else if (ii == 756)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Gold Silk", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Defense, Stat.MDef } }, exclElement, translate);
                else if (ii == 757)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Psyche Dust", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Attack, Stat.MAtk } }, exclElement, translate);
                else if (ii == 758)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Psyche Gem", ExclusiveItemEffect.TypeSpeedBoost,
                        new object[] { }, exclElement, translate);
                else if (ii == 759)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Psyche Globe", ExclusiveItemEffect.TypeGroupWeaknessReduce,
                        new object[] { "ghost" }, exclElement, translate);
                else if (ii == 760)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Gray Silk", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Defense, Stat.MDef } }, exclElement, translate);
                else if (ii == 761)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Rock Dust", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Attack, Stat.MAtk } }, exclElement, translate);
                else if (ii == 762)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Stone Gem", ExclusiveItemEffect.TypeSpeedBoost,
                        new object[] { }, exclElement, translate);
                else if (ii == 763)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Rock Globe", ExclusiveItemEffect.TypeGroupWeaknessReduce,
                        new object[] { "steel" }, exclElement, translate);
                else if (ii == 764)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Iron Silk", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Defense, Stat.MDef } }, exclElement, translate);
                else if (ii == 765)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Steel Dust", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Attack, Stat.MAtk } }, exclElement, translate);
                else if (ii == 766)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Metal Gem", ExclusiveItemEffect.TypeSpeedBoost,
                        new object[] { }, exclElement, translate);
                else if (ii == 767)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Steel Globe", ExclusiveItemEffect.TypeGroupWeaknessReduce,
                        new object[] { "fighting" }, exclElement, translate);
                else if (ii == 768)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Blue Silk", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Defense, Stat.MDef } }, exclElement, translate);
                else if (ii == 769)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Water Dust", ExclusiveItemEffect.TypeStatBonus,
                        new object[] { new Stat[] { Stat.Attack, Stat.MAtk } }, exclElement, translate);
                else if (ii == 770)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Aqua Gem", ExclusiveItemEffect.TypeSpeedBoost,
                        new object[] { }, exclElement, translate);
                else if (ii == 771)
                    AutoItemInfo.FillExclusiveTypeData(fileName, item, "Aqua Globe", ExclusiveItemEffect.TypeGroupWeaknessReduce,
                        new object[] { "grass" }, exclElement, translate);
                else if (ii == 804)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.TypeBulldozer, new object[] { "ground", "flying" }, translate);
                else if (ii == 805)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.SuperCrit, new object[] { }, translate);
                else if (ii == 806)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.NVECrit, new object[] { }, translate);
                else if (ii == 807)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.Nontraitor, new object[] { }, translate);
                else if (ii == 808)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.WaterTerrain, new object[] { }, translate);
                else if (ii == 809)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.LavaTerrain, new object[] { }, translate);
                else if (ii == 810)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.AllTerrain, new object[] { }, translate);
                else if (ii == 811)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.Wallbreaker, new object[] { }, translate);
                else if (ii == 812)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.GapFiller, new object[] { }, translate);
                else if (ii == 813)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.PPBoost, new object[] { 2 }, translate);
                else if (ii == 814)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.DeepBreather, new object[] { }, translate);
                else if (ii == 815)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.PracticeSwinger, new object[] { }, translate);
                else if (ii == 816)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.MisfortuneMirror, new object[] { }, translate);
                else if (ii == 817)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.CounterBasher, new object[] { }, translate);
                else if (ii == 818)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.DoubleAttacker, new object[] { }, translate);
                else if (ii == 819)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.MasterHurler, new object[] { }, translate);
                else if (ii == 820)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.AllyReviver, new object[] { }, translate);
                else if (ii == 821)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.AllyReviverBattle, new object[] { }, translate);
                else if (ii == 822)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.PressurePlus, new object[] { }, translate);
                else if (ii == 823)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.StatusOnAttack, new object[] { "poison" }, translate);
                else if (ii == 824)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.KnockbackOnAttack, new object[] { }, translate);
                else if (ii == 825)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.ThrowOnAttack, new object[] { }, translate);
                else if (ii == 826)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.SureHitAttacker, new object[] { }, translate);
                else if (ii == 827)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.SpecialAttacker, new object[] { }, translate);
                else if (ii == 828)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.StatusImmune, new object[] { "sleep" }, translate);
                else if (ii == 829)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.StatDropImmune, new object[] { new Stat[] { Stat.Attack } }, translate);
                else if (ii == 830)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.SleepWalker, new object[] { }, translate);
                else if (ii == 831)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.ChargeWalker, new object[] { }, translate);
                else if (ii == 832)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.StairSensor, new object[] { }, translate);
                else if (ii == 833)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.AcuteSniffer, new object[] { }, translate);
                else if (ii == 834)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.MapSurveyor, new object[] { }, translate);
                else if (ii == 835)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.XRay, new object[] { }, translate);
                else if (ii == 836)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.WeaknessPayback, new object[] { "fire", "poison" }, translate);
                else if (ii == 837)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.WarpPayback, new object[] { "flying" }, translate);
                else if (ii == 838)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.LungeAttack, new object[] { }, translate);
                else if (ii == 839)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.WideAttack, new object[] { }, translate);
                else if (ii == 840)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.ExplosiveAttack, new object[] { }, translate);
                else if (ii == 841)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.TrapBuster, new object[] { }, translate);
                else if (ii == 842)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.ExplosionGuard, new object[] { }, translate);
                else if (ii == 843)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.WandMaster, new object[] { }, translate);
                else if (ii == 844)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.WandSpread, new object[] { }, translate);
                else if (ii == 845)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.MultiRayShot, new object[] { }, translate);
                else if (ii == 846)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.RoyalVeil, new object[] { }, translate);
                else if (ii == 847)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.Celebrate, new object[] { }, translate);
                else if (ii == 848)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.Absorption, new object[] { }, translate);
                else if (ii == 849)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.ExcessiveForce, new object[] { }, translate);
                else if (ii == 850)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.Anchor, new object[] { }, translate);
                else if (ii == 851)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.BarrageGuard, new object[] { }, translate);
                else if (ii == 852)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.BetterOdds, new object[] { }, translate);
                else if (ii == 853)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.ClutchPerformer, new object[] { }, translate);
                else if (ii == 854)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.DistanceDodge, new object[] { }, translate);
                else if (ii == 855)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.CloseDodge, new object[] { }, translate);
                else if (ii == 856)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.FastHealer, new object[] { }, translate);
                else if (ii == 857)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.SelfCurer, new object[] { }, translate);
                else if (ii == 858)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.StatusMirror, new object[] { }, translate);
                else if (ii == 859)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.StatMirror, new object[] { }, translate);
                else if (ii == 860)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.ErraticAttacker, new object[] { }, translate);
                else if (ii == 861)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.ErraticDefender, new object[] { }, translate);
                else if (ii == 862)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.FastFriend, new object[] { }, translate);
                else if (ii == 863)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.CoinWatcher, new object[] { }, translate);
                else if (ii == 864)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.HiddenStairFinder, new object[] { }, translate);
                else if (ii == 865)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.ChestFinder, new object[] { }, translate);
                else if (ii == 866)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.ShopFinder, new object[] { }, translate);
                else if (ii == 867)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.SecondSTAB, new object[] { "fire" }, translate);
                else if (ii == 868)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.TypedAttack, new object[] { }, translate);
                else if (ii == 869)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.GapProber, new object[] { }, translate);
                else if (ii == 870)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.PassThroughAttacker, new object[] { }, translate);
                else if (ii == 871)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.WeatherProtection, new object[] { }, translate);
                else if (ii == 872)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.RemoveAbilityAttack, new object[] { }, translate);
                else if (ii == 873)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.CheekPouch, new object[] { }, translate);
                else if (ii == 874)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.HealInWater, new object[] { }, translate);
                else if (ii == 875)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.HealInLava, new object[] { }, translate);
                else if (ii == 876)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.HealOnNewFloor, new object[] { }, translate);
                else if (ii == 877)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.EndureCategory, new object[] { BattleData.SkillCategory.Physical }, translate);
                else if (ii == 878)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.EndureType, new object[] { "ice" }, translate);
                else if (ii == 879)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.SpikeDropper, new object[] { BattleData.SkillCategory.Physical }, translate);
                else if (ii == 880)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.NoStatusInWeather, new object[] { "sunny" }, translate);
                else if (ii == 881)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.DeepBreatherPlus, new object[] { }, translate);
                else if (ii == 882)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.WeaknessReduce, new object[] { "psychic" }, translate);
                else if (ii == 883)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.Gratitude, new object[] { }, translate);
                else if (ii == 884)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.HitAndRun, new object[] { }, translate);
                else if (ii == 885)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.StatusOnCategoryHit, new object[] { "whirlpool", BattleData.SkillCategory.Status }, translate);
                else if (ii == 886)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.StatusOnCategoryUse, new object[] { "electrified", BattleData.SkillCategory.Status }, translate);
                else if (ii == 887)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.MapStatusOnCategoryUse, new object[] { BattleData.SkillCategory.Status, "grassy_terrain" }, translate);
                else if (ii == 888)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.DoubleDash, new object[] { }, translate);
                else if (ii == 889)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.StatusSplash, new object[] { }, translate);
                else if (ii == 890)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.TypeBodyguard, new object[] { "bug" }, translate);
                else if (ii == 891)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.TypeBecomesCategory, new object[] { "grass", BattleData.SkillCategory.Physical }, translate);
                else if (ii == 892)
                    fileName = AutoItemInfo.FillExclusiveTestData(item, "", ExclusiveItemEffect.WeaknessDodge, new object[] { "fire" }, translate);

                if (ii >= 800 && ii < 900)
                    item.Comment = "Test item, do not translate";


                item.Sprite = "Box_Yellow";
                item.Icon = 10;
                item.Price = 800 * item.Rarity;
                item.UsageType = ItemData.UseType.Treasure;
                item.ItemStates.Set(new MaterialState());
                item.BagEffect = true;
                item.CannotDrop = ii <= 771;


                if (item.Name.DefaultText.StartsWith("**"))
                    item.Name.DefaultText = item.Name.DefaultText.Replace("*", "");
                else if (item.Name.DefaultText != "")
                    item.Released = true;

                if (item.Name.DefaultText != "")
                    DataManager.SaveData(fileName, DataManager.DataType.Item.ToString(), item);
            }

            AutoItemInfo.WriteExclusiveItems(MAX_INIT_EXCL_ITEMS, translate);
        }


    }
}
