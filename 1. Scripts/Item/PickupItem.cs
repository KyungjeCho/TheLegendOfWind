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
        [SerializeField]
        private float distance = 3.0f;

        [SerializeField]
        private ItemSO itemSO;

        public ItemSO ItemSO => itemSO;
        public void Interact(GameObject other)
        {
            float currentDistance = Vector3.Distance(transform.position, other.transform.position);
            if (currentDistance > distance)
            {
                return;
            }

            //other.GetComponent<>
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