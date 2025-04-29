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

        [SerializeField]
        private Transform leftWeaponSlot;
        [SerializeField]
        private Transform rightWeaponSlot;
        [SerializeField]
        private Transform defaultMeleeWeaponSlot;
        [SerializeField]
        private Transform defaultRangeWeaponSlot;

        private WeaponChangeBehaviour weaponChangeBehaviour;

        private bool canTakeMeleeWeapon;
        private bool canTakeRangeWeapon;
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

            weaponChangeBehaviour = GetComponent<WeaponChangeBehaviour>();
            weaponChangeBehaviour.OnWeaponChanged += ChangeWeapon;

            canTakeMeleeWeapon = true;
            canTakeRangeWeapon = true;
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
                    itemInstances[index] = EquipMeleeWeapon(itemSO);
                    break;
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
        private ItemInstances EquipMeleeWeapon(ItemSO itemSO)
        {
            if (itemSO == null)
            {
                return null;
            }
            Transform weaponTransform = Instantiate(itemSO.modelPrefab, defaultMeleeWeaponSlot).transform;
            ItemInstances instances = new GameObject().AddComponent<ItemInstances>();
            instances.items.Add(weaponTransform);
            instances.transform.parent = transform;

            return instances;
        }
        public void TakeMeleeWeapon(bool takeWeapon)
        {
            if (takeWeapon && canTakeMeleeWeapon && itemInstances[0].items[0] != null)
            {
                Transform weaponTransform = itemInstances[0].items[0];
                weaponTransform.parent = rightWeaponSlot;
                weaponTransform.localPosition = Vector3.zero;
                weaponTransform.localRotation = Quaternion.identity;
                itemInstances[0].items[0] = weaponTransform;
                canTakeMeleeWeapon = false;
            }
            if (!takeWeapon && !canTakeMeleeWeapon && itemInstances[0].items[0] != null)
            {
                Transform weaponTransform = itemInstances[0].items[0];
                weaponTransform.parent = defaultMeleeWeaponSlot;
                weaponTransform.localPosition = Vector3.zero;
                weaponTransform.localRotation = Quaternion.identity;
                itemInstances[0].items[0] = weaponTransform;
                canTakeMeleeWeapon = true;
            }
        }
        public void ChangeWeapon(int weapon) // 1 : Melee, 2 : Range
        {
            switch (weapon)
            {
                case 1:
                    TakeMeleeWeapon(true);
                    break;
                case 2:
                    TakeMeleeWeapon(false);
                    break;
            }

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