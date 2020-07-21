using System.Collections.Generic;
using System.Linq;
using Assets.FantasyInventory.Scripts.Data;
using Assets.FantasyInventory.Scripts.Enums;
using Assets.FantasyInventory.Scripts.GameData;
using Assets.FantasyInventory.Scripts.Interface.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FantasyInventory.Scripts.Interface
{
    /// <summary>
    /// High-level inventory inverface.
    /// </summary>
    public class Inventory : ItemWorkspace
    {
        public Equipment Equipment;
        public ScrollInventory Bag;
        public Button EquipButton;
        public Button RemoveButton;
        public AudioSource AudioSource;
        public AudioClip EquipSound;
        public AudioClip RemoveSound;
        public PlayerStats pStats;
        public bool toInitialize;
        public List<Item> playerInventory = new List<Item>();
        public List<Item> playerEquipment = new List<Item>();

        /// <summary>
        /// Initialize owned items (just for example).
        /// </summary>
        public void Awake()
        {
            for (int i = 0; i < saveManager.inventoryItems.Count; i++)
            {
                playerInventory.Add(new Item(saveManager.inventoryItems[i].Id, saveManager.inventoryItems[i].Count));
            }

            Bag.Initialize(ref playerInventory);
            Equipment.Initialize(ref saveManager.equipmentItems);
        }

        protected void Start()
        {
            Reset();
            EquipButton.interactable = RemoveButton.interactable = false;

            // TODO: Assigning static callbacks. Don't forget to set null values when UI will be closed. You can also use events instead.
            InventoryItem.OnItemSelected = SelectItem;
            InventoryItem.OnDragStarted = SelectItem;
      //    InventoryItem.OnDragCompleted = InventoryItem.OnDoubleClick = item => { if (Bag.Items.Contains(item)) Equip(); else Remove(); };
        }

        private void Update()
        {
            if (toInitialize)
            {
                InitiatePlayerData(saveManager.inventoryItems, playerInventory, Bag);
                toInitialize = false;
            }
        }

        public void InitiatePlayerData(List<Item> listFrom, List<Item> listTo, ScrollInventory type)
        {
            for (int i = 0; i < listFrom.Count; i++)
            {
                if (listTo.Any(j => j.Id == listFrom[i].Id))
                {
                    listTo[i].Count = listFrom[i].Count;
                }
                else
                {
                    listTo.Add(new Item(listFrom[i].Id, 1));
                }
            }
            for (int i = 0; i < listTo.Count; i++)
            {
                if (!listFrom.Any(j => j.Id == listTo[i].Id))
                {
                    listTo.Remove(listTo[i]);
                }
            }
            type.Initialize(ref listTo);
        }

        public void SelectItem(Item item)
        {
            SelectItem(item.Id);
        }

        public void SelectItem(ItemId itemId)
        {
            SelectedItem = itemId;
            SelectedItemParams = Items.Params[itemId];
            ItemInfo.Initialize(SelectedItem, SelectedItemParams);
            Refresh();
        }

        public void Equip()
        {
            var equipped = Equipment.Items.LastOrDefault(i => i.Params.Type == SelectedItemParams.Type);

            if (equipped != null)
            {
                AutoRemove(SelectedItemParams.Type, Equipment.Slots.Count(i => i.ItemType == SelectedItemParams.Type));
            }

            if (SelectedItemParams.Tags.Contains(ItemTag.TwoHanded))
            {
                var shield = Equipment.Items.SingleOrDefault(i => i.Params.Type == ItemType.Shield);

                if (shield != null)
                {
                    MoveItem(shield, Equipment, Bag);
                }
            }
            else if (SelectedItemParams.Type == ItemType.Shield)
            {
                var weapon2H = Equipment.Items.SingleOrDefault(i => i.Params.Tags.Contains(ItemTag.TwoHanded));

                if (weapon2H != null)
                {
                    MoveItem(weapon2H, Equipment, Bag);
                }
            }

            MoveItem(SelectedItem, Bag, Equipment);
            AudioSource.PlayOneShot(EquipSound);
        }

        public void Remove()
        {
            MoveItem(SelectedItem, Equipment, Bag);
            SelectItem(Equipment.Items.FirstOrDefault(i => i.Id == SelectedItem) ?? Bag.Items.Single(i => i.Id == SelectedItem));
            AudioSource.PlayOneShot(RemoveSound);
        }

        public void Use()
        {
            if (SelectedItemParams.Type == ItemType.Potion)
            {

            }
            AudioSource.PlayOneShot(RemoveSound);
        }

        public override void Refresh()
        {
            if (SelectedItem == ItemId.Undefined)
            {
                ItemInfo.Reset();
                EquipButton.interactable = RemoveButton.interactable = false;
            }
            else
            {
                if (CanEquip())
                {
                    EquipButton.interactable = Bag.Items.Any(i => i.Id == SelectedItem)
                        && Equipment.Slots.Count(i => i.ItemType == SelectedItemParams.Type) > Equipment.Items.Count(i => i.Id == SelectedItem);
                    RemoveButton.interactable = Equipment.Items.Any(i => i.Id == SelectedItem);
                    ItemInfo.Price.enabled = !SelectedItemParams.Tags.Contains(ItemTag.NotForSale);
                }
                else
                {
                    EquipButton.interactable = RemoveButton.interactable = false;
                }
            }
        }

        private bool CanEquip()
        {
            return Equipment.Slots.Any(i => i.ItemType == SelectedItemParams.Type && i.ItemTags.All(j => SelectedItemParams.Tags.Contains(j)));
        }

        /// <summary>
        /// Automatically removes items if target slot is busy.
        /// </summary>
        private void AutoRemove(ItemType itemType, int max)
        {
            var items = Equipment.Items.Where(i => i.Params.Type == itemType).ToList();
            long sum = 0;

            foreach (var p in items)
            {
                sum += p.Count;
            }

            if (sum == max)
            {
                MoveItem(items.LastOrDefault(i => i.Id != SelectedItem) ?? items.Last(), Equipment, Bag);
            }
        }
    }
}