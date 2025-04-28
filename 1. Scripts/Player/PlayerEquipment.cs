using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace KJ
{
    public class PlayerEquipment : MonoBehaviour
    {
        public InventorySO equipmentSO;

        private EquipmentCombiner combiner;

        private ItemInstances[] itemInstances = new ItemInstances[6];

        public ItemSO[] defaultItemSOs = new ItemSO[6];

        private void Awake()
        {
            combiner = new EquipmentCombiner(gameObject);

            for (int i = 0; i < equipmentSO.Slots.Length; i++)
            {
                equipmentSO.Slots[i].OnPreUpdate += OnRemoveItem;
                equipmentSO.Slots[i].OnPostUpdate += OnEquipItem;
            }
        }

        private void Start()
        {
            foreach(InventorySlot slot in equipmentSO.Slots)
            {
                OnEquipItem(slot);
            }
        }

        private void OnEquipItem(InventorySlot slot)
        {
            ItemSO itemSO = slot.ItemSO;
            if (itemSO == null)
            {
                EquipDefaultItemBy(slot.AllowedItems[0]);
                return;
            }

            int index = (int)slot.AllowedItems[0];

            switch(slot.AllowedItems[0])
            {
                case ItemType.Helmet:
                case ItemType.Chest:
                case ItemType.Pants:
                case ItemType.Boots:
                    itemInstances[index] = EquipSkinnedItem(itemSO);
                    break;
                case ItemType.MeleeWeapon:
                case ItemType.RangeWeapon:
                    itemInstances[index] = EquipMeshItem(itemSO);
                    break;
            }

            if (itemInstances[index] != null)
            {
                itemInstances[index].name = slot.AllowedItems[0].ToString();
            }
        }

        private ItemInstances EquipSkinnedItem(ItemSO itemSO)
        {
            if (itemSO == null)
            {
                return null;
            }
            Transform itemTransform = combiner.AddLimb(itemSO.modelPrefab, itemSO.boneNames);
            ItemInstances instances = itemTransform.gameObject.AddComponent<ItemInstances>();

            if (instances != null)
            {
                instances.items.Add(itemTransform);
            }   
            return instances;
        }
        private ItemInstances EquipMeshItem(ItemSO itemSO)
        {
            if (itemSO == null)
            {
                return null;
            }

            Transform[] itemTransforms = combiner.AddMesh(itemSO.modelPrefab);
            if (itemTransforms.Length > 0)
            {
                ItemInstances instances = new GameObject().AddComponent<ItemInstances>();
                foreach (Transform t in itemTransforms)
                {
                    instances.items.Add(t);
                }

                instances.transform.parent = transform;

                return instances;
            }

            return null;
        }
        private void EquipDefaultItemBy(ItemType type)
        {
            int index = (int)type;

            ItemSO itemSO = defaultItemSOs[index];
            switch (type)
            {
                case ItemType.Helmet:
                case ItemType.Chest:
                case ItemType.Pants:
                case ItemType.Boots:
                    itemInstances[index] = EquipSkinnedItem(itemSO);
                    break;
                case ItemType.MeleeWeapon:
                case ItemType.RangeWeapon:
                    itemInstances[index] = EquipMeshItem(itemSO);
                    break;
            }
        }
        private void RemoveItemBy(ItemType type)
        {
            int index = (int)type;
            if (itemInstances[index] != null)
            {
                Destroy(itemInstances[index].gameObject);
                itemInstances[index] = null;
            }
        }
        private void OnRemoveItem(InventorySlot slot)
        {
            ItemSO itemSO = slot.ItemSO;
            if (itemSO == null)
            {
                RemoveItemBy(slot.AllowedItems[0]);
                return;
            }

            if (slot.ItemSO.modelPrefab != null)
            {
                RemoveItemBy(slot.AllowedItems[0]);
            }
        }
    }
}