using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    /// <summary>
    /// 
    /// </summary>
    public class PickupItem : MonoBehaviour, IInteract
    {
        public LayerMask groundLayer;
        public SoundList dropSound;

        [SerializeField]
        private float distance = 3.0f;
        [SerializeField]
        private Vector3 offset;

        [SerializeField]
        private ItemSO itemSO;

        public ItemSO ItemSO => itemSO;

        private void OnEnable()
        {
            RaycastHit hit;

            Vector3 ray = transform.position + Vector3.up * 100f;

            if (Physics.Raycast(ray, Vector3.down, out hit, Mathf.Infinity, groundLayer))
            {
                
                Vector3 groundPos = hit.point;
                Vector3 finalPos = groundPos + offset;

                transform.position = finalPos;
            }
        }
        public void Interact(GameObject other)
        {
            float currentDistance = Vector3.Distance(transform.position, other.transform.position);
            if (currentDistance > distance)
            {
                return;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(TagAndLayer.Player))
            {
                other.gameObject.GetComponent<PlayerInventoryController>().PickupItem(this);
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, distance);
        }
    }

}