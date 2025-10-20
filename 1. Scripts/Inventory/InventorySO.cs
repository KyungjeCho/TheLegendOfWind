using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace KJ
{
    [Serializable]
    public class WrapInventoryData
    {
        public int[] itemId;
        public int[] amount;
    }
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


        private string jsonFilePath = "Assets/9. Resources/Resources/Data";
        [SerializeField]
        private string jsonFileName = "inventorySO.json";

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
            if (itemB.CanPlaceInSlot(itemA.ItemSO) && itemA.CanPlaceInSlot(itemB.ItemSO))
            {
                InventorySlot temp = new InventorySlot(itemB.item, itemB.amount);
                itemB.UpdateSlot(itemA.item, itemA.amount);
                itemA.UpdateSlot(temp.item, temp.amount);
            }
            
        }
        public WrapInventoryData WrapData()
        {
            WrapInventoryData data = new WrapInventoryData();
            data.itemId = new int[Slots.Length];
            data.amount = new int[Slots.Length];
            for(int i = 0; i < Slots.Length; i++)
            {
                data.itemId[i] = Slots[i].item.id;
                data.amount[i] = Slots[i].amount;
            }

            return data;
        }
        public void UnwrapData(WrapInventoryData data)
        {
            if (data == null)
            {
                return;
            }
            for (int i = 0; i < Slots.Length; i++)
            {
                if (data.itemId[i] <= -1)
                {
                    Slots[i].UpdateSlot(new Item(), 0);
                }
                else
                {
                    DataManager manager = DataManager.GetOrCreateInstance();
                    ItemSO itemSO = manager.ItemDBSO.itemSOs[data.itemId[i]];
                    int amount = data.amount[i];
                    Slots[i].UpdateSlot(itemSO.CreateItem(), amount);
                }
            }
        }
        public void SaveData()
        {
            WrapInventoryData wrapData = WrapData();
            string path = Path.Combine(jsonFilePath, jsonFileName);
            string json = JsonUtility.ToJson(wrapData, true);
            File.WriteAllText(path, json);
        }
        public void LoadData()
        {
            //Clear();
            string path = Path.Combine(jsonFilePath, jsonFileName);

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                WrapInventoryData wrapData = JsonUtility.FromJson<WrapInventoryData>(json);
                UnwrapData(wrapData);
            }
        }
        public void Clear()
        {
            inventory.Clear();
        }
        public void UseItem(InventorySlot slotToUse)
        {
            if (slotToUse.ItemSO == null || slotToUse.item.id < 0 || slotToUse.amount <= 0)
            {
                return;
            }

            ItemSO itemSO = slotToUse.ItemSO;
            slotToUse.UpdateSlot(slotToUse.item, slotToUse.amount - 1);

            OnUseItem.Invoke(itemSO);
        }
    }

}
