using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class ManualCollision : MonoBehaviour
    {
        public Vector3 boxSize = new Vector3(2, 2, 2);
        public Vector3 position = new Vector3(0, 0, 0);

        public Collider[] CheckOverlapBox(LayerMask layerMask)
        {
            return Physics.OverlapBox(transform.TransformPoint(position), boxSize * 0.5f, Quaternion.identity, layerMask);
        }

        void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(Vector3.zero + position, boxSize * 0.5f);
        }
    }
}
