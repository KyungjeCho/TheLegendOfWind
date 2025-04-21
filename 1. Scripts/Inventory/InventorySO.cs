using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "Inventory SO", menuName = "ScriptableObjects/Inventory SO")]
    public class InventorySO : ScriptableObject
    {
        public ItemDBSO itemDBSO;
        public InterfaceType type;

        [SerializeField]
        private Inventory inventory = new Inventory();

        public Action<ItemSO> OnUseItem;

        public InventorySlot[] Slots => inventory.slots;

        public int EmptySlotCount => Slots.Where(x => x.item.id <= -1).Count();

        public bool AddItem(Item item, int amount)
        {
            InventorySlot slot = FindItemInInventory(item);
            if (!itemDBSO.itemSOs[item.id].stackable || slot == null)
            {
                if (EmptySlotCount <= 0)
                {
                    return false;
                }

                GetEmptySlot().UpdateSlot(item, amount);
            }
            else
            {
                slot.AddAmount(amount);
            }
            return true;
        }

        public InventorySlot FindItemInInventory(Item item) => Slots.FirstOrDefault(i => i.item.id == item.id);
        public bool IsContainItem(ItemSO itemSO) => Slots.FirstOrDefault(i => i.item.id == itemSO.data.id) != null;
        public InventorySlot GetEmptySlot() => Slots.FirstOrDefault(i => i.item.id <= -1);
        public void SwapItems(InventorySlot itemA, InventorySlot itemB)
        {
            if (itemA == itemB)
            {
                return;
            }
            //if (itemB.CanPlaceInSlot(itemA.item))

        }
    }

}
