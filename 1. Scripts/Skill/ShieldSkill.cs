using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "Shield Skill", menuName = "ScriptableObjects/Shield Skill")]
    public class ShieldSkill : BaseSkill
    {
        //public GameObject shidldEffectPrefab;
        private PlayerHealth playerHealth;

        public override void SetPlayerTransform(Transform transform)
        {
            base.SetPlayerTransform(transform);

            playerHealth = transform.GetComponent<PlayerHealth>();
        }
        public void SetPlayerHealth(PlayerHealth playerHealth) => this.playerHealth = playerHealth;

        public override void UseSkill()
        {
            playerHealth?.AddDamageHandlers(new OneTimeInvincibleShield());
        }
    }
}