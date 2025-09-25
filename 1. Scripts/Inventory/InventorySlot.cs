using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    /// <summary>
    /// 각 인벤토리 슬롯 클래스
    /// </summary>
    [Serializable]
    public class InventorySlot
    {
        public ItemType[] AllowedItems = new ItemType[0];

        [NonSerialized]
        public InventorySO parent;
        [NonSerialized]
        public GameObject slotUI;

        [NonSerialized]
        public Action<InventorySlot> OnPreUpdate;
        [NonSerialized]
        public Action<InventorySlot> OnPostUpdate;

        public Item item;
        public int amount;

        public ItemSO ItemSO
        {
            get
            {
                return item.id >= 0 ? DataManager.Instance.ItemDBSO.itemSOs[item.id] : null;
            }
        }
        public InventorySlot() => UpdateSlot(new Item(), 0);
        public InventorySlot(Item item, int amount) => UpdateSlot(item, amount);
        public void RemoveItem() => UpdateSlot(new Item(), 0);
        public void AddAmount(int value) => UpdateSlot(item, amount += value);

        public void UpdateSlot(Item item, int amount)
        {
            if (amount <= 0)
            {
                item = new Item();
            }

            if (OnPreUpdate != null)
            {
                OnPreUpdate.Invoke(this);
            }
            this.item = item;
            this.amount = amount;
            if (OnPostUpdate != null)
            {
                OnPostUpdate.Invoke(this);
            }
            if (parent != null)
            {
                parent.SaveData();
            }
        }

        public bool CanPlaceInSlot(ItemSO itemSO)
        {
            if (AllowedItems.Length <= 0 || itemSO == null || itemSO.data.id < 0)
            {
                return true;
            }

            foreach(ItemType itemType in AllowedItems)
            {
                if (itemSO.type == itemType)
                {
                    return true;
                }
            }

            return false;
        }
    }

}
