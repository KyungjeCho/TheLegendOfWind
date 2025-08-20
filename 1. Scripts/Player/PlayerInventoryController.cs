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

        public bool PickupItem(PickupItem pickupItem)
        {

            Item item = pickupItem.ItemSO.CreateItem();
            inventory.AddItem(item, 1);
            Destroy(pickupItem.gameObject);
            return true;
        }
    }
}