using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/ItemSO")]
    public class ItemSO : ScriptableObject
    {
        public ItemType type;
        public bool stackable;

        public Sprite icon;
        public GameObject modelPrefab;

        [TextArea(15, 20)]
        public string description;

    }

}
