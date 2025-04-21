using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "New Item DB", menuName = "ScriptableObjects/ItemDBSO")]
    public class ItemDBSO : ScriptableObject
    {
        public ItemSO[] itemSOs = new ItemSO[0];

        public void OnValidate()
        {
            for (int i = 0; i < itemSOs.Length; i++)
            {
                itemSOs[i].data.id = i;
            }
        }
    }

}
