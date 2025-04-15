using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KJ
{
    public class SlimeController : EnemyController, IDamagable
    {
        
        protected override void Start()
        {
            base.Start();

            stateMachine.AddState(new MoveState());
            stateMachine.AddState(new AttackState());
            stateMachine.AddState(new DeadState());
        }

        protected override void Update()
        {
            base.Update();
        }

        public bool IsAlive
        {
            get
            {
                if (CurrentHP > 0f)
                {
                    return true;
                }
                return false;
            }
        }

        public void OnDamage(IAttackable enemy)
        {
            // Can Get hit ?

            // hit effect

            // hit sound

            // hit animation


            // Damage Calc

            currentHP -= 1.0f;
            Debug.Log(currentHP);
            if (currentHP <= 0f)
            {
                stateMachine.ChangeState<DeadState>();
            }
        }

    }
}
