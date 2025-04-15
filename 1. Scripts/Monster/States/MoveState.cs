
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KJ
{
    // 타겟 확인
    // if 타겟이 존재하면 -> 위치 설정 움직임
    //                   -> Idle 상태로 전환
    // if 공격 사거리에 들어왔는가? -> 공격 상태로 전환
    // 
    public class MoveState : State<EnemyController>
    {
        private NavMeshAgent agent;
        private int speedFloat;
        private Transform target;
        public override void OnInitialize()
        {
            agent = context.GetAgent;

            speedFloat = Animator.StringToHash(AnimatorKey.Speed);
        }

        public override void OnStateEnter()
        {
            target = context.Target;
            if (target == null)
            {
                context.ChangeState<IdleState>();
            }
            else
            {
                agent.SetDestination(target.position);
                context.GetAnimator.SetFloat(speedFloat, 1.0f);

                agent.stoppingDistance = context.AttackRange;
            }
        }
        public override void Update(float deltaTime)
        {
            if (context.Target)
            {
                agent.SetDestination(target.position);
            }
            else 
            {
                context.ChangeState<IdleState>();
            }
            if (agent.stoppingDistance > agent.remainingDistance)
            {
                context.ChangeState<AttackState>();
            }
        }
    }

}
