using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace KJ
{
    public class TestItems : MonoBehaviour
    {
        public InventorySO equipmentObject;
        public InventorySO inventoryObject;
        public ItemDBSO databaseObject;

        public void AddNewItem()
        {
            if (databaseObject.itemSOs.Length > 0)
            {
                ItemSO newItemObject = databaseObject.itemSOs[Random.Range(0, databaseObject.itemSOs.Length - 1)];
                Item newItem = new Item(newItemObject);

                inventoryObject.AddItem(newItem, 1);
            }
        }

        public void ClearInventory()
        {
            equipmentObject?.Clear();
            inventoryObject?.Clear();
        }
    }

}
