using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class PlayerInventoryController : MonoBehaviour
    {
        [SerializeField]
        private InventorySO equipment;
        [SerializeField]
        private InventorySO inventory;
        private PlayerHealth playerHealth;

        private void Start()
        {
            playerHealth = GetComponent<PlayerHealth>();
        }

        public bool PickupItem(PickupItem pickupItem)
        {
            Item item = pickupItem.ItemSO.CreateItem();
            inventory.AddItem(item, 1);
            DropQueue.Instance.ItemDrop(pickupItem.ItemSO.itemName);
            Destroy(pickupItem.gameObject);
            return true;
        }

        public void OnUseItem(ItemSO itemSO)
        {
            foreach (ItemBuff buff in itemSO.data.buffs)
            {
                if (buff.stat == PlayerAttribute.HP)
                {
                    playerHealth.HealHealthPoint(buff.value);
                }
            }
        }
    }
}