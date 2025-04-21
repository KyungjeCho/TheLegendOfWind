using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KJ
{
    /// <summary>
    /// ������ �ִ� 24�� ������ �κ��丮 Ŭ����
    /// </summary>
    [Serializable]
    public class Inventory 
    {
        public InventorySlot[] slots = new InventorySlot[24];

        public void Clear()
        {
            foreach (InventorySlot slot in slots)
            {
                slot.UpdateSlot(new Item(), 0);
            }
        }

        public bool IsContain(ItemSO itemSO)
        {
            return Array.Find(slots, i => i.item.id == itemSO.data.id) != null;
        }

        public bool IsContain(int id)
        {
            return slots.FirstOrDefault(i => i.item.id == id) != null;
        }
    }
}