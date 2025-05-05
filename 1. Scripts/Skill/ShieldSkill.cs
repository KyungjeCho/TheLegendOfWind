using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "Shield Skill", menuName = "ScriptableObjects/Shield Skill")]
    public class ShieldSkill : BaseSkill
    {
        public EffectList shieldEffectList;
        public EffectList shieldEffectStartList;
        public EffectList shieldEffectLoopList;
        public EffectList shieldEffectStopList;

        private PlayerHealth playerHealth;
        private OneTimeInvincibleShield oneTimeInvincibleShieldHandler;
        private EffectClip shieldEffectStart;
        private EffectClip shieldEffectLoop;
        private EffectClip shieldEffectStop;

        private GameObject shieldEffectStartGO;
        private GameObject shieldEffectLoopGO;
        private GameObject shieldEffectStopGO;

        [SerializeField]
        private float cooldownTime = 10f;

        public override void SetPlayerTransform(Transform transform)
        {
            base.SetPlayerTransform(transform);

            playerHealth = transform.GetComponent<PlayerHealth>();

            shieldEffectStart = DataManager.EffectData.GetCopy((int)shieldEffectStartList);
            shieldEffectLoop = DataManager.EffectData.GetCopy((int)shieldEffectLoopList);
            shieldEffectStop = DataManager.EffectData.GetCopy((int)shieldEffectStopList);
        }
        public void SetPlayerHealth(PlayerHealth playerHealth) => this.playerHealth = playerHealth;

        public override void UseSkill()
        {
            if (oneTimeInvincibleShieldHandler == null)
            {
                oneTimeInvincibleShieldHandler = new OneTimeInvincibleShield();
            }
            playerHealth?.AddDamageHandler(oneTimeInvincibleShieldHandler);
            TimerManager.Instance.StartTimer(cooldownTime, () => RemoveDamageHandler());

            shieldEffectStartGO = EffectManager.Instance.PlayEffect(shieldEffectStartList, playerTranform.position);
            shieldEffectStartGO.GetComponent<ParticleSystem>().Play();

            TimerManager.Instance.StartTimer(1f, () => PlayLoopShieldEffect());
        }
        private void PlayLoopShieldEffect()
        {
            shieldEffectLoopGO = EffectManager.Instance.PlayEffect(shieldEffectLoopList, playerTranform.position);
            shieldEffectLoopGO.GetComponent<ParticleSystem>().Play();
        }
        private void RemoveDamageHandler()
        {
            playerHealth?.RemoveDamageHandler(oneTimeInvincibleShieldHandler);

            shieldEffectLoopGO.GetComponent<ParticleSystem>().Stop();
            shieldEffectLoopGO.SetActive(false);
            shieldEffectStopGO = EffectManager.Instance.PlayEffect(shieldEffectStopList, playerTranform.position);
            shieldEffectStopGO.GetComponent<ParticleSystem>().Play();
        }
    }
}