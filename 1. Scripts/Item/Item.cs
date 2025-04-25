using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [Serializable]
    public class Item 
    {
        public int id = -1;
        public string name = string.Empty;

        public ItemBuff[] buffs;

        public Item()
        {
            id = -1;
            name = string.Empty;
        }

        public Item(ItemSO itemSO)
        {
            name = itemSO.name;
            id = itemSO.data.id;

            buffs = new ItemBuff[itemSO.data.buffs.Length];

            for (int i = 0; i < buffs.Length; i++)
            {
                buffs[i] = new ItemBuff(itemSO.data.buffs[i].Value) { stat = itemSO.data.buffs[i].stat };
            }
        }
    }

}
