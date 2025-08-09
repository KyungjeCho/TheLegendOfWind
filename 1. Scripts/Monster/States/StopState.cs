using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class StopState : State<EnemyController>
    {
        private float stopTimer = 0;
        private float cooldownTime = 5f;

        public override void Update(float deltaTime)
        {
            if (stopTimer > cooldownTime)
            {
                stateMachine.ChangeState<IdleState>();
            }
            stopTimer += deltaTime;
        }

        public override void OnInitialize()
        {
            
        }
        public override void OnStateEnter()
        {
            context.GetAnimator.speed = 0f;
            context.GetAgent.enabled = false;
            stopTimer = 0;
        }
        public override void OnStateExit()
        {
            context.GetAnimator.speed = 1f;
            context.GetAgent.enabled = true;
        }
    }
}
