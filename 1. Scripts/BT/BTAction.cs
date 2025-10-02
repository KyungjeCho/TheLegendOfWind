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
            Debug.Log(state);
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
    public class BTAoEReady : BTAction<DragonController>
    {
        public BTAoEReady(DragonController context) : base(context)
        {
        }

        public override BTNodeState Evaluate(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
    public class BTAoEAttack : BTAction<DragonController>
    {
        public BTAoEAttack(DragonController context) : base(context)
        {
        }

        public override BTNodeState Evaluate(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}