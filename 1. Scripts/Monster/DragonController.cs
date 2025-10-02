using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KJ
{
    public class DragonController : BossController, IDamagable
    {
        private BTIdleAction            idleAction;
        private BTMoveToTargetAction    moveAction;
        private BTMeleeAttackAction     meleeAttackAction;

        private BTConditional notCombatConditional;
        private BTConditional underHalfHPConditional;
        private BTConditional underQuarterHPConditional;

        private BTSequence meleeAttackSequencer;

        public Action onCombatStart;
        public Action onHalfHPGimmickStart;

        public bool isCombatting = false;
        public bool isFirstHalfHPGimmick = true;

        public Vector3 centerOfMap = Vector3.zero;

        public bool IsUnderHalfHP => CurrentHP < MaxHP * 0.5f;
        public bool IsUnderQuarterHP => CurrentHP < MaxHP * 0.25f;
        public bool IsAlive => CurrentHP > 0f ? true : false;

        public void OnDamage(GameObject target, float damage)
        {
            //// Can Get hit ?
            //if (!IsAlive)
            //{
            //    GetAnimator.SetBool(isAliveBool, false);
            //    return;
            //}
            //else
            //{
            //    // hit effect
            //    EffectManager.Instance.PlayEffect(EffectList.HitBlood, transform.position + transform.up + transform.forward);
            //    // hit sound
            //    SoundManager.Instance.PlayEffectSound(HitSoundClip, transform.position, 1f);

            //    // Damage Calc
            //    currentHP -= damage;
            //    HealthChanged(currentHP / maxHP);

            //    if (currentHP <= 0f)
            //    {
            //        GetAnimator.SetBool(isAliveBool, false);
            //        dropSystem?.Drop(target);
            //        stateMachine.ChangeState<DeadState>();
            //    }
            //    else
            //    {
            //        transform.rotation = Quaternion.LookRotation((target.transform.position - transform.position));
            //        // hit animation
            //        GetAnimator.SetTrigger(getHitTrigger);
            //    }

            //}
        }

        protected override void Start()
        {
            base.Start();

            onCombatStart += StartCombat;

            idleAction = new BTIdleAction(this);
            moveAction = new BTMoveToTargetAction(this);
            meleeAttackAction = new BTMeleeAttackAction(this);

            notCombatConditional = new BTConditional(idleAction, () => isCombatting == false);
            
            meleeAttackSequencer = new BTSequence(new List<BTNode>()
            {
                moveAction, meleeAttackAction
            });

            root = new BTSelector(new List<BTNode>()
            {
                notCombatConditional,
                meleeAttackSequencer
            }) ;
        }
        protected override void Update()
        {
            base.Update();
        }

        public void StartCombat()
        {
            isCombatting = true;
        }

        public void OnMeleeAttackEvent()
        {
            // 밀리 어택 이밴트 실행시
        }
    }
}
