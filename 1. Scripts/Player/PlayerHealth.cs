using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace KJ
{
    public class PlayerHealth : MonoBehaviour, IDamagable
    {
        public PlayerData playerData;
        public float hp;
        public float defense;
        private float maxHp;
        private Animator myAnimator;

        public SoundList hitSound;
        public SoundList dieSound;
        public EffectList hitEffect;
        private int getHitTrigger;
        private int dieTrigger;

        public bool IsAlive => hp > 0f;

        public event Action<float> OnHealthChanged;

        // Start is called before the first frame update
        void Start()
        {
            if (playerData != null)
            {
                playerData = ScriptableObject.CreateInstance<PlayerData>();
            }
            playerData.LoadData();

            maxHp = playerData.GetCopy().hp;
            hp = playerData.GetCopy().hp;
            defense = DataManager.PlayerLVData.GetCopyFromLevel(playerData.GetCopy().level).defense;

            myAnimator = GetComponent<Animator>();

            getHitTrigger = Animator.StringToHash(AnimatorKey.GetHit);
            dieTrigger = Animator.StringToHash(AnimatorKey.Die);
        }

        
        private void OnDisable()
        {
            
        }

        public void Kill()
        {

        }

        public void OnDamage(float damage)
        {
            // OnDamage

            // DamageCalc
            float totalDamage = (defense - damage) > 0f ? defense - damage : 1f;
            hp -= totalDamage;

            OnHealthChanged?.Invoke(hp / maxHp);

            if (hp < Mathf.Epsilon)
            {
                Kill();
            }
            else
            {
                myAnimator.SetTrigger(getHitTrigger);
                // hit SFX Play
                SoundManager.Instance.PlayOneShotEffect(hitSound, transform.position, 1f);
                // hit VFX Play
                EffectManager.Instance.PlayEffect(hitEffect, transform.position + Vector3.up * 1.7f);
            }
        }
    }

}
