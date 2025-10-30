using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KJ
{
    public class SlimeController : EnemyController, IAttackable, IDamagable, IStopable
    {
        private int getHitTrigger;
        private int isAliveBool;

        private ManualCollision manualCollision;
        public LayerMask targetMask;

        private DropSystem dropSystem;

        private ObjectSelector objectSelector;

        private MaterialPropertyBlock propBlock;
        private SkinnedMeshRenderer meshRenderer;

        private readonly int intensityId = Shader.PropertyToID("_Highlight_Intensity");

        protected override void Start()
        {
            base.Start();
            originalPos = transform.position;

            stateMachine.AddState(new MoveState());
            stateMachine.AddState(new AttackState());
            stateMachine.AddState(new DeadState());
            stateMachine.AddState(new StopState());
            stateMachine.AddState(new ReturnState());

            getHitTrigger   = Animator.StringToHash(AnimatorKey.GetHit);
            isAliveBool     = Animator.StringToHash(AnimatorKey.IsAlive);

            GetAnimator.SetBool(isAliveBool, true);

            manualCollision = GetComponent<ManualCollision>();
            dropSystem = GetComponent<DropSystem>();

            propBlock = new MaterialPropertyBlock();
            meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

            originalPos = transform.position;

            objectSelector = GetComponentInChildren<ObjectSelector>();
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
                    transform.rotation = Quaternion.LookRotation((target.transform.position - transform.position));
                    // hit animation
                    GetAnimator.SetTrigger(getHitTrigger);
                }
                
            }
        }

        public void OnAttack()
        {
            Collider[] targetsInManualCollision = manualCollision.CheckOverlapBox(targetMask);

            foreach(Collider collider in targetsInManualCollision)
            {
                if (collider.GetComponent<IDamagable>() != null)
                {
            
                    collider.GetComponent<IDamagable>().OnDamage(gameObject, monsterStat.attack);
                }
            }
        }

        public void StopObject()
        {
            stateMachine.ChangeState<StopState>();
        }

        public void SetHighlight(bool isEnabled)
        {
            meshRenderer.GetPropertyBlock(propBlock);

            propBlock.SetFloat(intensityId, isEnabled ? 1f : 0f);

            meshRenderer.SetPropertyBlock(propBlock);
        }

        public void TriggerHighlight(float duration)
        {
            StartCoroutine(IHighlightRoutine(duration));
        }

        public IEnumerator IHighlightRoutine(float duration)
        {
            SetHighlight(true);
            yield return new WaitForSeconds(duration);
            SetHighlight(false);
        }
    }
}
