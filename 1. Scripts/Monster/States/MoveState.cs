
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KJ
{
    public class MoveState : State<EnemyController>
    {
        private NavMeshAgent agent;
        private int speedFloat;
        public override void OnInitialize()
        {
            

        }

        public override void OnStateEnter()
        {
            agent = context.GetAgent;

            speedFloat = Animator.StringToHash(AnimatorKey.Speed);
            agent.SetDestination(context.Target.position);
            context.GetAnimator.SetFloat(speedFloat, 1.0f);

        }
        public override void Update(float deltaTime)
        {
            if (context.Target)
            {
                agent.SetDestination(context.Target.position);
            }
            
        }
    }

}
