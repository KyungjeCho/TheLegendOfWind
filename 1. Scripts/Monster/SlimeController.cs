using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KJ
{
    public class SlimeController : EnemyController
    {
        protected override void Start()
        {
            base.Start();

            stateMachine.AddState(new MoveState());
            
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}
