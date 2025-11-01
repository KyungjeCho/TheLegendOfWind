using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class BarrelController : MonoBehaviour, IDamagable
    {
        public EffectList brokenEffect;
        public SoundList brokenSound;
        public GameObject spawnItemPrefab;

        public bool IsAlive => throw new System.NotImplementedException();

        public void BreakBarrel(Transform targetTr = null)
        {
            EffectManager.Instance.PlayEffect(brokenEffect, transform.position + transform.up * 1.0f);
            SoundManager.Instance.PlayOneShotEffect(brokenSound, transform.position, 1f);

            Instantiate(spawnItemPrefab, transform.position + transform.up * 1.0f, Quaternion.identity);

            gameObject.SetActive(false);
            Destroy(gameObject, 3f);
        }

        public void OnDamage(GameObject target, float damage)
        {
            BreakBarrel(target.transform);
        }
    }
}

