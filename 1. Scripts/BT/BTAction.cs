using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KJ
{
    public abstract class BTAction<T> : BTNode
    {
        protected T context;

        public T Context => context;

        public BTAction(T context)
        {
            this.context = context;
        }
    }

    public class BTIdleAction : BTAction<DragonController>
    {
        private Animator myAnimator;
        private int speedFloat;

        public BTIdleAction(DragonController context) : base(context)
        {
            myAnimator = context.GetAnimator;

            speedFloat = Animator.StringToHash(AnimatorKey.Speed);
        }

        public override BTNodeState Evaluate(float deltaTime)
        {
            myAnimator.SetFloat(speedFloat, 0f);

            if (context.Target != null)
            {
                context.onCombatStart?.Invoke();
            }
            state = BTNodeState.Success;
            return state;
        }
    }

    public class BTMoveToTargetAction : BTAction<DragonController>
    {
        private Animator myAnimator;
        private NavMeshAgent myAgent;
        private int speedFloat;

        private Transform targetTr;

        public BTMoveToTargetAction(DragonController context) : base(context)
        {
            myAnimator = context.GetAnimator;
            myAgent = context.GetAgent;

            speedFloat = Animator.StringToHash(AnimatorKey.Speed);
            myAgent.speed = 2f;
        }

        public override BTNodeState Evaluate(float deltaTime)
        {
            targetTr = context.Target;

            if (targetTr == null)
            {
                state = BTNodeState.Failure;
                return state;
            }

            myAgent.SetDestination(targetTr.position);
            myAnimator.SetFloat(speedFloat, 1.0f);

            myAgent.stoppingDistance = context.AttackRange;

            if (myAgent.stoppingDistance > myAgent.remainingDistance)
            {
                state = BTNodeState.Success;
                return state;
            }
            else
            {
                state = BTNodeState.Running;
                return state;
            }
        }
    }

    public class BTMeleeAttackAction : BTAction<DragonController>
    {
        private Animator myAnimator;
        private NavMeshAgent myAgent;

        private int meleeAttackBool;

        private Transform targetTr;

        public BTMeleeAttackAction(DragonController context) : base(context)
        {
            myAgent = context.GetAgent;
            myAnimator = context.GetAnimator;

            meleeAttackBool = Animator.StringToHash(AnimatorKey.MeleeAttack);
        }

        public override BTNodeState Evaluate(float deltaTime)
        {
            //targetTr = context.Target.transform;
            //if (targetTr == null)
            //{
            //    state = BTNodeState.Failure;
            //    return state;
            //}

            if (myAgent.stoppingDistance > myAgent.remainingDistance)
            {
                myAnimator.SetBool(meleeAttackBool, true);
                state = BTNodeState.Success;
                return state;
            }
            else
            {
                myAnimator.SetBool(meleeAttackBool, false);
                state = BTNodeState.Failure;
                return state;
            }

        }
    }

    #region Dragon 50% HP Gimmick
    public class BTReturnToCenterOfMap : BTAction<DragonController>
    {
        private Animator myAnimator;
        private NavMeshAgent myAgent;
        private int speedFloat;

        public BTReturnToCenterOfMap(DragonController context) : base(context)
        {
            myAgent = context.GetAgent;
            myAnimator = context.GetAnimator;

            speedFloat = Animator.StringToHash(AnimatorKey.Speed);
        }

        public override BTNodeState Evaluate(float deltaTime)
        {
            myAgent.SetDestination(context.centerOfMap);
            myAnimator.SetFloat(speedFloat, 1.0f);
            myAgent.stoppingDistance = 0.1f;
            myAgent.speed = 10f;
            
            if (!myAgent.pathPending && myAgent.stoppingDistance > myAgent.remainingDistance)
            {
                state = BTNodeState.Success;
            }
            else
            {
                state = BTNodeState.Running;
            }
            return state;
        }
    }
    public class BTScreamForGimmick : BTAction<DragonController>
    {
        public SoundList screamSound = SoundList.CrateBreak;

        private Animator myAnimator;
        private int speedFloat;
        private int screamTrigger;

        private bool isScreamed = false;

        public BTScreamForGimmick(DragonController context) : base(context)
        {
            myAnimator = context.GetAnimator;
            speedFloat = Animator.StringToHash(AnimatorKey.Speed);
            screamTrigger = Animator.StringToHash(AnimatorKey.Scream);
            myAnimator.SetFloat(speedFloat, 0f);
            
        }

        public override BTNodeState Evaluate(float deltaTime)
        {
            myAnimator.SetFloat(speedFloat, 0f);
            
            if (isScreamed)
            {
                state = BTNodeState.Success;
                return state;
            }

            myAnimator.SetTrigger(screamTrigger);
            state = BTNodeState.Success;
            context.RockSlide.OnStartGimmick();

            isScreamed = true;
            return state;
        }
    }
    public class BTAoECharging: BTAction<DragonController>
    {
        private bool isNodeStarted = false;
        private bool isNodeEnd = false;
        private float delayTime = 5f;
        private float elapsedTime = 0f;

        // Charging Anim
        private Animator myAnimator;
        private int readyBool;
        // Charging Effect
        private GlobalAoEAttack globalAoEAttack;

        // Warning Decal Projector
        private WarningDecalProjectorController warningDecalProjectorController;
        private ParticleSystem imposionVFX;

        // ���� ��� ������ ���� ���� �� ���� ��� -> return Sueccess
        public BTAoECharging(DragonController context) : base(context)
        {
            myAnimator = context.GetAnimator;
            readyBool = Animator.StringToHash(AnimatorKey.Ready);
            globalAoEAttack = context.GetGlobalAoEAttack;
            warningDecalProjectorController = globalAoEAttack.aoeWarningDecalProjectorController;

            imposionVFX = context.imposionVFX;
        }
        public void OnNodeStart(float deltaTime)
        {
            if (elapsedTime < delayTime)
            {
                elapsedTime += deltaTime;
                return;
            }
            if (isNodeStarted)
            {
                return;
            }
            // Anim Change
            myAnimator.SetBool(readyBool, true);

            // Global AoE Decal Start
            globalAoEAttack.OnAoEWarningStart();
            if (imposionVFX != null)
            {
                imposionVFX.Play();
            }
           
            // Decal End Event Callback 
            warningDecalProjectorController.onDecalProjectorEnd += OnWarningEnd;

            isNodeStarted = true;
        }

        public void OnWarningEnd()
        {
            myAnimator.SetBool(readyBool, false);

            isNodeEnd = true;
        }
        public override BTNodeState Evaluate(float deltaTime)
        {
            if (isNodeEnd)
            {
                state = BTNodeState.Success;
                return state;
            }
            OnNodeStart(deltaTime);

            state = BTNodeState.Running;
            return state;
        }
    }
    public class BTAoEAttack : BTAction<DragonController>
    {
        private bool isNodeStarted = false;
        private Animator myAnimator;
        private int screamTrigger;

        private SoundList shockwaveSound = SoundList.None;
        private ParticleSystem shockwaveVFX;

        private GlobalAoEAttack globalAoEAttack;
        // ��ũ�� �ѹ���
        // shock wave vfx play

        // ��ü sphere -> Player �ִ��� Ȯ�� 
        // �÷��̾�� �巡�� ���̿� ���� �ִ��� Ȯ��
        // ���� ���ٸ� 999 ������  
        // ���� �ִٸ� 0 ������
        // �׳� ��ü �� 3��~4�� ���� �ı� (���� ����Ʈ�� �迭�� ��������)
        // ���� ������ ������ context�� isFirst... ���� false ����

        public BTAoEAttack(DragonController context) : base(context)
        {
            myAnimator = context.GetAnimator;
            screamTrigger = Animator.StringToHash(AnimatorKey.Scream);

            shockwaveVFX = context.shockwaveVFX;
            globalAoEAttack = context.GetGlobalAoEAttack;
        }

        public override BTNodeState Evaluate(float deltaTime)
        {
            if (isNodeStarted)
            {
                state = BTNodeState.Success;
                return state;
            }

            myAnimator.SetTrigger(screamTrigger);
            shockwaveVFX.Play();
            globalAoEAttack.OnAoEAttackStart();
            context.isFirstHalfHPGimmick = false;
            isNodeStarted = true;
            state = BTNodeState.Success;
            return state;
        }
    }
    #endregion

    #region Dragon HP 25% under gimmick
    public class BTInvincible : BTAction<DragonController>
    {
        private BossInvincible bossInvincible;
        private bool isInvincible = false;
        private float elapsedTime = 0f;
        private float durationTime = 5f;

        public BTInvincible(DragonController context) : base(context)
        {
            bossInvincible = context.GetBossInvincible;
            context.onInvincibleBreak += OnInvicibleBreak;
        }

        public void OnInvicibleBreak()
        {
            isInvincible = false;
            context.SetHighlight(false);
            bossInvincible.RemoveInvincibleModifier();
        }
        public override BTNodeState Evaluate(float deltaTime)
        {
            if (!isInvincible)
            {
                elapsedTime += deltaTime;
            }
            if (elapsedTime > durationTime)
            {
                elapsedTime = 0f;
                isInvincible = true;
                context.SetHighlight(true);
                bossInvincible.AddInvincibleModifier();

                state = BTNodeState.Success;
                return state;
            }
            
            if (isInvincible)
            {
                state = BTNodeState.Success;
                return state;
            }
            else
            {
                state = BTNodeState.Failure;
                return state;
            }
        }
    }

    public class BTRockAttack : BTAction<DragonController>
    {
        private Animator myAnimator;

        private int rangeAttackTrigger;

        private bool isAttacking = false;
        // Dragon Craw Animation ->
        public BTRockAttack(DragonController context) : base(context)
        {
            myAnimator = context.GetAnimator;

            rangeAttackTrigger = Animator.StringToHash(AnimatorKey.RangeAttack);

            context.onRockAttackExit += OnRockAttackExit;
        }
        public void OnRockAttackExit()
        {
            isAttacking = false;
        }
        public override BTNodeState Evaluate(float deltaTime)
        {
            if (!isAttacking)
            {
                myAnimator.SetTrigger(rangeAttackTrigger);
                isAttacking=true;
            }

            state = BTNodeState.Success;
            return state;
        }
    }

    #endregion
}