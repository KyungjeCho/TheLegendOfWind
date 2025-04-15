using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KJ
{
    public class SlimeController : EnemyController, IDamagable
    {
        private Vector3 originalPos;
        
        
        private int getHitTrigger;
        private int isAliveBool;


        protected override void Start()
        {
            base.Start();
            originalPos = transform.position;

            stateMachine.AddState(new MoveState());
            stateMachine.AddState(new AttackState());
            stateMachine.AddState(new DeadState());

            getHitTrigger = Animator.StringToHash(AnimatorKey.GetHit);
            isAliveBool = Animator.StringToHash(AnimatorKey.IsAlive);

            GetAnimator.SetBool(isAliveBool, true);
        }

        protected override void Update()
        {
            base.Update();
        }

        public bool IsAlive
        {
            get
            {
                if (CurrentHP > 0f)
                {
                    return true;
                }
                return false;
            }
        }

        public void OnDamage(IAttackable enemy)
        {
            // Can Get hit ?
            if (!IsAlive)
            {
                GetAnimator.SetBool(isAliveBool, false);
                return;
            }
            else
            {
                // hit effect
                EffectManager.Instance.PlayEffect(EffectList.HitBlood, transform.position + transform.up + transform.forward);
                // hit sound
                SoundManager.Instance.PlayEffectSound(HitSoundClip, transform.position, 1f);

                // Damage Calc

                currentHP -= 1.0f;

                if (currentHP <= 0f)
                {
                    GetAnimator.SetBool(isAliveBool, false);
                    stateMachine.ChangeState<DeadState>();
                }
                else
                {
                    // hit animation
                    GetAnimator.SetTrigger(getHitTrigger);
                }
                
            }
        }
    }
}
