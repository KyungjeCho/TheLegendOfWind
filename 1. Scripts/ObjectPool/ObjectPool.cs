using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KJ
{
    public class ObjectPool
    {
        public GameObject original;
        public Transform root;

        private Stack<GameObject> pool = new Stack<GameObject>();

        public int maxPoolSize;

        public ObjectPool(GameObject original, int maxPoolSize = 10, Transform root = null)
        {
            this.original = original;
            this.maxPoolSize = maxPoolSize;
            if (root != null)
            {
                this.root = root;
            }
            else
            {
                this.root = new GameObject().transform;
                this.root.name = $"{original.name}_Root";
            }

            for (int i = 0; i < maxPoolSize; i++)
            {
                Return(Create());
            }
            Debug.Log(pool.Count);
        }

        private GameObject Create()
        {
            GameObject go = Object.Instantiate<GameObject>(original);
            go.name = original.name;
            return go;
        }

        public void Return(GameObject go)
        {
            go.transform.parent = root;
            go.gameObject.SetActive(false);
            pool.Push(go);
        }

        public GameObject Get(Transform parent = null)
        {
            GameObject go;

            if (pool.Count > 0)
            {
                go = pool.Pop();
            }
            else
            {
                go = Create();
            }

            go.SetActive(true);

            if (parent != null)
            {
                go.transform.parent = parent;
            }

            return go;
        }
    }
}