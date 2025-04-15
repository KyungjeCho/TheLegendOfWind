using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class IdleState : State<EnemyController>
    {
        private int speedFloat;

        private float elapsedTime;
        public override void OnInitialize()
        {
            speedFloat = Animator.StringToHash(AnimatorKey.Speed);
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();

            context.GetAnimator.SetFloat(speedFloat, 0f);
            elapsedTime = 0f;
        }

        public override void Update(float deltaTime)
        {
            // Search
            Transform target = context.GetFOV.NearestTarget; 

            if (target != null )
            {
                context.ChangeState<MoveState>();
            }
            if (elapsedTime > 5f)
            {
                context.ChangeState<MoveState>();
            }
            
            elapsedTime += deltaTime;
        }
    }

}
