using System.Globalization;
using System.Linq;
using Assets.FantasyInventory.Scripts.Data;
using Assets.FantasyInventory.Scripts.Enums;
using UnityEngine;

namespace Assets.FantasyInventory.Scripts.Interface.Elements
{
    /// <summary>
    /// Abstract item workspace. It can be shop or player inventory. Items can be managed here (selected, moved and so on).
    /// </summary>
    public abstract class ShopItemWorkspace : MonoBehaviour
    {
        public ItemInfo ItemInfo;
        public GameSaveManager saveManager;


        protected ItemId SelectedItem;
        protected ItemParams SelectedItemParams;

        private void Awake()
        {
            //      inventory = GameObject.Find("Inventory").GetComponent<Inventory>(); 
        }

        public void Start()
        {
            saveManager = FindObjectOfType<GameSaveManager>();
        }

        public abstract void Refresh();

        protected void Reset()
        {
            SelectedItem = ItemId.Undefined;
            ItemInfo.Reset();
        }

        protected void MoveItem(Item item, ShopItemContainer from, ShopItemContainer to)
        {
            MoveItem(item.Id, from, to);
        }
        protected void EquipItem(Item item, ShopItemContainer from, ShopItemContainer to)
        {
            EquipItem(item.Id, from, to);
        }

        protected void SellItems(ItemId id, ShopItemContainer from)
        {
            var target = from.Items.SingleOrDefault(i => i.Id == id);
            if (target.Count > 1)
            {
                target.Count--;
                for (int i = 0; i < saveManager.inventoryItems.Count; i++)
                {
                    if (target.Id == saveManager.inventoryItems[i].Id)
                    {
                        saveManager.inventoryItems[i].Count--;
                        Debug.Log("Deceased item - 1 from MoveItem Func");
                    }
                }
            }
            else
            {
                from.Items.Remove(target);
                Debug.Log("removed item from bag");
                for (int i = 0; i < saveManager.inventoryItems.Count; i++)
                {
                    if (target.Id == saveManager.inventoryItems[i].Id)
                    {
                        saveManager.inventoryItems.Remove(saveManager.inventoryItems[i]);
                        Debug.Log("removed item from save list");
                    }
                }
            }
            Refresh();
            from.Refresh();
        }

        protected void MoveItem(ItemId id, ShopItemContainer from, ShopItemContainer to)
        {
            if (to.Expanded)
            {
                to.Items.Add(new Item(id, 1));
                Debug.Log("Created new item to list");
            }
            else
            {
                var target = to.Items.SingleOrDefault(i => i.Id == id);

                if (target == null)
                {
                    to.Items.Add(new Item(id, 1));
                    Debug.Log("Created new item to list");
                    if (from.isShop)
                    {
                        saveManager.inventoryItems.Add(new Item(id, 1));
                        Debug.Log("added item once and list size is: " + saveManager.inventoryItems.Count);
                    }
                }
                else
                {
                    target.Count++;
                    Debug.Log("Created new item to list");
                    for (int i = 0; i < saveManager.inventoryItems.Count; i++)
                    {
                        if (target.Id == saveManager.inventoryItems[i].Id)
                        {
                            saveManager.inventoryItems[i].Count++;
                            Debug.Log("item add + 1 from moveItem Func");
                        }
                    }
                    //update player inventory
                }
            }

            if (from.Expanded)
            {
                from.Items.Remove(from.Items.Last(i => i.Id == id));
            }
            else
            {
                var target = from.Items.Single(i => i.Id == id);

                if (target.Count > 1)
                {
                    target.Count--;
                    for (int i = 0; i < saveManager.inventoryItems.Count; i++)
                    {
                        if (target.Id == saveManager.inventoryItems[i].Id)
                        {
                            saveManager.inventoryItems[i].Count--;
                            Debug.Log("decrese item count");
                            if (saveManager.inventoryItems[i].Count == 0)
                            {
                                saveManager.inventoryItems.Remove(saveManager.inventoryItems[i]);
                            }
                        }
                    }
                }
                else
                {
                    from.Items.Remove(target);
                }
            }
            Refresh();
            from.Refresh();
            to.Refresh();
        }

        protected void EquipItem(ItemId id, ShopItemContainer from, ShopItemContainer to)
        {
            Debug.Log("Enter EquipItem funciton");
            if (to.Expanded)
            {
                to.Items.Add(new Item(id, 1));
            }
            else
            {
                var target = to.Items.SingleOrDefault(i => i.Id == id);

                if (target == null)
                {
                    to.Items.Add(new Item(id, 1));
                }
                else
                {
                    target.Count++;
                }
            }

            if (from.Expanded)
            {
                from.Items.Remove(from.Items.Last(i => i.Id == id));
            }
            else
            {
                var target = from.Items.Single(i => i.Id == id);

                if (target.Count >= 1)
                {
                    target.Count--;
                }
                else
                {
                    from.Items.Remove(target);
                }
            }
            Refresh();
            from.Refresh();
            to.Refresh();
        }
    }
}