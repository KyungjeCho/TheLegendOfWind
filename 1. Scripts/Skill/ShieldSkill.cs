using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "Shield Skill", menuName = "ScriptableObjects/Shield Skill")]
    public class ShieldSkill : BaseSkill
    {
        public EffectList shieldEffectStartList;
        public EffectList shieldEffectLoopList;
        public EffectList shieldEffectStopList;

        public SoundList shieldStartSoundList;
        public SoundList shieldBreakSoundList;
        private PlayerHealth playerHealth;
        private OneTimeInvincibleShield oneTimeInvincibleShieldHandler;

        private GameObject shieldEffectStartGO;
        private GameObject shieldEffectLoopGO;
        private GameObject shieldEffectStopGO;

        public override void SetPlayerTransform(Transform transform)
        {
            base.SetPlayerTransform(transform);

            playerHealth = transform.GetComponent<PlayerHealth>();
            oneTimeInvincibleShieldHandler = new OneTimeInvincibleShield(this);
            playerHealth?.AddDamageHandler(oneTimeInvincibleShieldHandler);
        }
        public void SetPlayerHealth(PlayerHealth playerHealth) => this.playerHealth = playerHealth;

        public override void UseSkill()
        {
            if (Timer > 0)
            {
                return;
            }
            if (!oneTimeInvincibleShieldHandler.IsUsing)
            {
                return;
            }
            oneTimeInvincibleShieldHandler.IsUsing = false;

            shieldEffectLoopGO = EffectManager.Instance.PlayEffect(shieldEffectLoopList, playerTranform.position, playerTranform);
            shieldEffectLoopGO.GetComponent<ParticleSystem>().Play();

            StartSkill();
        }
        public void StopAllShieldEffect()
        {
            oneTimeInvincibleShieldHandler.IsUsing = true;

            if (shieldEffectLoopGO != null)
            {
                Destroy(shieldEffectLoopGO);
            }
            if (shieldBreakSoundList != SoundList.None)
            {
                SoundManager.Instance.PlayOneShotEffect(shieldBreakSoundList, playerTranform.position, 1f);
            }
        }
    }
}