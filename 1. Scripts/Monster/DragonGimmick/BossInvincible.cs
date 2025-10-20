using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KJ
{
    public class BossInvincible : MonoBehaviour
    {
        public GameObject shieldPrefab;
        public int shieldCount = 5;

        public EffectList impactEffect;
        public Vector3 impactOffset = Vector3.zero;
        private EffectClip impactEffectClip;

        private bool isShieldSpawned = false;
        private List<GameObject> shieldList;
        private BossController context;
        
        private InvincibleModifier invincibleModifier;

        public InvincibleModifier InvincibleModifier
        {
            get
            {
                if (invincibleModifier == null)
                {
                    invincibleModifier = new InvincibleModifier(); 
                }
                return invincibleModifier;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            context = GetComponent<BossController>();
            impactEffectClip = DataManager.EffectData.effectClips[(int)impactEffect];
            impactEffectClip.PreLoad();
        }

        public void SpawnShield()
        {
            if (isShieldSpawned)
            {
                return;
            }
            shieldList = new List<GameObject>();
            for (int i = 0; i < shieldCount; i++)
            {
                GameObject go = Instantiate(shieldPrefab);
                ShieldOrbit shieldOrbit = go.GetComponent<ShieldOrbit>();
                shieldOrbit.target = transform;
                shieldOrbit.startAngle = i * (360f / shieldCount);

                shieldList.Add(go);
            }
            isShieldSpawned = true;
        }

        public void DeleteShield()
        {
            foreach(GameObject go in shieldList)
            {
                go.SetActive(false);
                Destroy(go, 1f);
            }
            shieldList.Clear();
            isShieldSpawned = false;
        }
        public void AddInvincibleModifier()
        {
            context.damagePipeline.AddModifier(InvincibleModifier);
            SpawnShield();
        }
        public void RemoveInvincibleModifier()
        {
            EffectManager.Instance.PlayEffect(impactEffect, transform.position + impactOffset);
            context.damagePipeline.RemoveModifier(InvincibleModifier);
            DeleteShield();
        }
    }
}