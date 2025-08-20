using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class LichController : EnemyController, IAttackable, IDamagable
    {
        private Vector3 originalPos;

        private int getHitTrigger;
        private int isAliveBool;

        public GameObject projectilePrefab;
        public Transform projectileCreatePosition;

        private DropSystem dropSystem;

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

            dropSystem = GetComponent<DropSystem>();
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

        public void OnAttack()
        {
            if (projectilePrefab == null)
            {
                Debug.Log("Projectile 이 설정되어 있지 않습니다.");
                return;
            }

            GameObject projectileGO = Instantiate(projectilePrefab);
            ProjectileController projectileController = projectileGO.GetComponent<ProjectileController>();
            if (projectileController != null)
            {
                projectileController.transform.position = projectileCreatePosition.position;
                projectileController.target = Target;
                projectileController.owner = transform;

            }
        }

        public void OnDamage(GameObject target, float damage)
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

                currentHP -= damage;
                HealthChanged(currentHP / maxHP);

                if (currentHP <= 0f)
                {
                    GetAnimator.SetBool(isAliveBool, false);
                    dropSystem?.Drop(target);
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
