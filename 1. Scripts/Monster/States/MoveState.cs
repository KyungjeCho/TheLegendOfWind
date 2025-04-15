
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KJ
{
    // Ÿ�� Ȯ��
    // if Ÿ���� �����ϸ� -> ��ġ ���� ������
    //                   -> Idle ���·� ��ȯ
    // if ���� ��Ÿ��� ���Դ°�? -> ���� ���·� ��ȯ
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
