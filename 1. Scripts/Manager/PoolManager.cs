using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class PoolManager : SingletonMonoBehaviour<PoolManager>
    {
        public Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();
        public Transform root;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        public void Initialize()
        {
            if (root == null)
            {
                root = new GameObject { name = "@Pool_Root" }.transform;
            }
        }

        public void CreatePool(GameObject original, int poolSize = 10)
        {
            ObjectPool pool = new ObjectPool(original, poolSize);
            pool.root.parent = this.root;

            pools.Add(original.name, pool);
        }

        public void Return(GameObject prefab)
        {
            string name = prefab.name;

            if (pools.ContainsKey(name) == false)
            {
                Destroy(prefab);
                return;
            }
            pools[name].Return(prefab);
        }

        public GameObject Get(GameObject original, Transform parent = null)
        {
            if (pools.ContainsKey(original.name) == false)
            {
                CreatePool(original);
            }
            return pools[original.name].Get(parent);
        }
        public GameObject Get(GameObject original, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (pools.ContainsKey(original.name) == false)
            {
                CreatePool(original);
            }
            GameObject go = pools[original.name].Get(parent);
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go;
        }
        public bool IsContain(GameObject original)
        {
            return pools.ContainsKey(original.name);
        }

        public bool IsContain(string originalName)
        {
            return pools.ContainsKey(originalName);
        }

        public GameObject GetOriginal(string name)
        {
            if (pools.ContainsKey(name) == false)
            {
                return null;
            }

            return pools[name].original;
        }

        public void Clear()
        {
            foreach (Transform child in root)
                Destroy(child.gameObject);

            pools.Clear();
        }
    }
}