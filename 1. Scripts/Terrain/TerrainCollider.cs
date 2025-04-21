using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainCollider : MonoBehaviour
{
    private Transform myTransform;

    private void Awake()
    {
        myTransform = transform;
    }

    private void Start()
    {
        MeshRenderer[] children = myTransform.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer child in children)
        {
            child.AddComponent<MeshCollider>();
        }
    }
}
