using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class AttackState : State<EnemyController>
    {
        private int attackBool;

        public override void OnInitialize()
        {
            attackBool = Animator.StringToHash(AnimatorKey.Attack);
        }
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            
            context.GetAnimator.SetBool(attackBool, true);
        }
        public override void OnStateExit()
        {
            base.OnStateExit();

            context.GetAnimator.SetBool(attackBool, false);
        }
        public override void Update(float deltaTime)
        {
            
        }
    }

}

