using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class FlameController : MonoBehaviour
    {
        public float damage = 50f;
        public float stopDuration = 5f;

        private bool isWorking = true;
        private ParticleSystem ps;

        // Start is called before the first frame update
        void Start()
        {
            ps = GetComponent<ParticleSystem>();
        }

        public void SetIsWorking(bool isWorking)
        { 
            this.isWorking = isWorking; 
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(TagAndLayer.Player))
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

                if (playerHealth != null && isWorking)
                {
                    playerHealth.OnDamage(gameObject, 50f);
                }
            }
        }

        public void StopObject()
        {
            ps.Stop();
            SetIsWorking(false);
            StartCoroutine(CStopFlameForDuration());
        }

        private IEnumerator CStopFlameForDuration()
        {
            yield return new WaitForSeconds(stopDuration);
            ps.Play();
            SetIsWorking(true);
        }
    }

}
