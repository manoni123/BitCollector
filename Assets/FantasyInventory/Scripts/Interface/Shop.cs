using System.Collections.Generic;
using System.Linq;
using Assets.FantasyInventory.Scripts.Data;
using Assets.FantasyInventory.Scripts.Enums;
using Assets.FantasyInventory.Scripts.GameData;
using Assets.FantasyInventory.Scripts.Interface.Elements;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FantasyInventory.Scripts.Interface
{
    /// <summary>
    /// High-level shop inverface.
    /// </summary>
    public class Shop : ItemWorkspace
    {
        public ScrollInventory Trader;
        public ScrollInventory Bag;
        public Button BuyButton;
        public Button SellButton;
        public AudioSource AudioSource;
        public AudioClip TradeSound;
        public AudioClip NoMoney;
        public PlayerStats pStats;

        public const int SellRatio = 2;

        /// <summary>
        /// Initialize owned items and trader items (just for example).
        /// </summary>
        protected void Awake()
        {
            var inventory = new List<Item>();
            for (int i = 0; i < saveManager.inventoryItems.Count; i++)
            {
                inventory.Add(new Item(saveManager.inventoryItems[i].Id, saveManager.inventoryItems[i].Count));
                Debug.Log("addded item to inventory from shop");
            };

            var shop = new List<Item>
            {
                new Item(ItemId.FireballScroll, 10),
                new Item(ItemId.HealthPotion, 10),
                new Item(ItemId.IronSword, 1),
                new Item(ItemId.IvyBow, 1),
                new Item(ItemId.LeatherArmor, 1),
                new Item(ItemId.LeatherHelmet, 1),
                new Item(ItemId.ManaPotion, 10),
                new Item(ItemId.RoundShield, 1),
                new Item(ItemId.SilverRing, 2),
                new Item(ItemId.Spear, 1),
                new Item(ItemId.StoneAmulet, 1),
                new Item(ItemId.TwoHandedSword, 1),
                new Item(ItemId.WoodcutterAxe, 10)
            };

            Trader.Initialize(ref shop);
            Bag.Initialize(ref inventory);
        }

        protected void Start()
        {
            Reset();
            BuyButton.interactable = SellButton.interactable = true;

            // TODO: Assigning static callbacks. Don't forget to set null values when UI will be closed. You can also use events instead.
            InventoryItem.OnItemSelected = SelectItem;
            InventoryItem.OnDragStarted = SelectItem;
   //         InventoryItem.OnDragCompleted = InventoryItem.OnDoubleClick = item => { if (Trader.Items.Contains(item)) Buy(); else Sell(); };
        }

        public void SelectItem(Item item)
        {
            SelectItem(item.Id);
        }

        public void SelectItem(ItemId itemId)
        {
            SelectedItem = itemId;
            SelectedItemParams = Items.Params[itemId];
            ItemInfo.Initialize(SelectedItem, SelectedItemParams, true);
            Refresh();
        }

        public void Buy()
        {
            Debug.Log("pressed buy");
            if (pStats.pGold < SelectedItemParams.Price)
            {
                Debug.Log("clicked button");
                AudioSource.PlayOneShot(NoMoney);
                ItemInfo.Description.text = "You dont have enough gold!";
                return;
            } 
            else if (pStats.pGold >= SelectedItemParams.Price)
            {
                //TODO add to inventory the bought item
                AddMoney(-SelectedItemParams.Price);
                MoveItem(SelectedItem, Trader, Bag);
                AudioSource.PlayOneShot(TradeSound);
            }
        }

        public void Sell()
        {
            AddMoney(SelectedItemParams.Price / SellRatio);
            SellItems(SelectedItem, Bag);
            AudioSource.PlayOneShot(TradeSound);
        }

        public override void Refresh()
        {
            if (SelectedItem == ItemId.Undefined)
            {
                ItemInfo.Reset();
                BuyButton.interactable = SellButton.interactable = false;
                Debug.Log("buttons go disable4");
            }
            else
            {
                var item = Items.Params[SelectedItem];
                Debug.Log("buttons go disable");
                if (!item.Tags.Contains(ItemTag.NotForSale))
                {
                    BuyButton.interactable = Trader.Items.Any(i => i.Id == SelectedItem) && pStats.pGold >= item.Price;
                    SellButton.interactable = Bag.Items.Any(i => i.Id == SelectedItem) && pStats.pGold >= item.Price / SellRatio;
                    Debug.Log("buttons go disable2");
                }
                else
                {
                    ItemInfo.Price.text = null;
                    BuyButton.interactable = SellButton.interactable = false;
                    Debug.Log("buttons go disable3");
                }
            }
        }

        private static long GetCurrency(ItemContainer bag, ItemId currencyId)
        {
            var currency = bag.Items.SingleOrDefault(i => i.Id == currencyId);

            return currency == null ? 0 : currency.Count;
        }

        private void AddMoney(int value)
        {
            pStats.AddGold(value);
        }
    }
}