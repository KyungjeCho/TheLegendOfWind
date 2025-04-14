using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class IdleState : State<EnemyController>
    {
        private int speedFloat;


        public override void OnInitialize()
        {
            speedFloat = Animator.StringToHash(AnimatorKey.Speed);
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();

            context.GetAnimator.SetFloat(speedFloat, 0f);
        }

        public override void Update(float deltaTime)
        {
            // Search
            Transform target = context.GetFOV.NearestTarget; 

            if (target != null )
            {
                context.ChangeState<MoveState>();
            }
        }
    }

}
