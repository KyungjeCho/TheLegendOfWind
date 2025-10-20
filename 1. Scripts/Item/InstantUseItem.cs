using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class InstantUseItem : MonoBehaviour
    {
        public SoundList useSound;
        public EffectList useEffect;

        [Range(1f, 10f)]
        public float distance = 3.0f;

        public ItemSO itemSO;

        private void Start()
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(TagAndLayer.Player))
            {
                PlayerInventoryController controller = other.GetComponent<PlayerInventoryController>();
                if (controller != null)
                {
                    SoundManager.Instance.PlayOneShotEffect(useSound, transform.position, 1f);
                    EffectManager.Instance.PlayEffect(useEffect, other.transform.position + other.transform.up * 1.0f, other.transform);
                    controller.OnUseItem(itemSO);
                    Destroy(gameObject);
                }
            }
        }
    }

}
