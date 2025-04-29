using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class AttackBehaviour : BaseBehaviour
    {
        private bool isAttacking = false;

        private int meleeAttackComboInt;
        private int meleeAttackTrigger;
        private int weaponInt;

        public void SetIsAttacking(bool isAttacking)
        {
            this.isAttacking = isAttacking;
        }
        public bool GetIsAttacking() => this.isAttacking;
        public void Start()
        {
            meleeAttackComboInt = Animator.StringToHash(AnimatorKey.MeleeAttackCombo);
            meleeAttackTrigger = Animator.StringToHash(AnimatorKey.MeleeAttack);
            weaponInt = Animator.StringToHash(AnimatorKey.Weapon);

            behaviourController.SubscribeBehaviour(this);
        }

        private void Update()
        {
            if (!isAttacking && Input.GetButtonDown(ButtonName.Attack) && behaviourController.IsGrounded() && behaviourController.GetAnimator.GetInteger(weaponInt) == 1)
            {
                behaviourController.GetAnimator.SetInteger(meleeAttackComboInt, 1);
                behaviourController.GetAnimator.SetTrigger(meleeAttackTrigger);
            }
            AttackManagement();
        }

        public override void LocalLateUpdate()
        {
            
        }

        private void AttackManagement()
        {
            if (isAttacking)
            {
                behaviourController.OverrideWithBehaviour(this);
                //behaviourController.LockTempBehaviour(behaviourCode);
                
            }
            else
            {
                behaviourController.RevokeOverridingBehaviour(this);
                //behaviourController.UnLockTempBehaviour(behaviourCode);
            }
        }
    }

}
