using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class AttackState : State<EnemyController>
    {
        private int attackBool;
        private AttackStateController attackStateController;

        public override void OnInitialize()
        {
            attackStateController = context.GetComponent<AttackStateController>();
            attackBool = Animator.StringToHash(AnimatorKey.Attack);
        }
        public override void OnStateEnter()
        {
            base.OnStateEnter();

            attackStateController.enterAttackHandler += OnEnterAttackState;
            attackStateController.exitAttackHandler += OnExitAttackState;

            context.GetAnimator.SetBool(attackBool, true);
        }
        public override void OnStateExit()
        {
            base.OnStateExit();

            attackStateController.enterAttackHandler -= OnEnterAttackState;
            attackStateController.exitAttackHandler -= OnExitAttackState;

            context.GetAnimator.SetBool(attackBool, false);
        }
        public override void Update(float deltaTime)
        {
            
        }

        public void OnEnterAttackState() { }

        public void OnExitAttackState() 
        {
            stateMachine.ChangeState<IdleState>();
        }
    }

}

