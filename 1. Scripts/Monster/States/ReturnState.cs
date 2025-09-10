using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KJ
{
    public class ReturnState : State<EnemyController>
    {
        private NavMeshAgent agent;
        private int speedFloat;

        public override void OnInitialize()
        {
            agent = context.GetAgent;

            speedFloat = Animator.StringToHash(AnimatorKey.Speed);
        }

        public override void OnStateEnter()
        {
            agent.SetDestination(context.originalPos);
            context.GetAnimator.SetFloat(speedFloat, 1.0f);
            agent.stoppingDistance = 0.1f;
        }
        public override void Update(float deltaTime)
        {
            if (agent.stoppingDistance > agent.remainingDistance)
            {
                context.ChangeState<IdleState>();
            }
        }
    }
}