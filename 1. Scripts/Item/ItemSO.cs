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

        public Item data = new Item();

        public List<string> boneNames = new List<string>();

        [TextArea(15, 20)]
        public string description;

        public void OnValidate()
        {
            boneNames.Clear();
            if (modelPrefab == null || modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>() == null)
            {
                return;
            }

            SkinnedMeshRenderer renderer = modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
            Transform[] bones = renderer.bones;

            foreach (Transform t in bones)
            {
                boneNames.Add(t.name);
            }
        }

        public Item CreateItem()
        {
            Item newItem = new Item(this);
            return newItem;
        }
    }

}
