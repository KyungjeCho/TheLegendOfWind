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

        private bool isSelecting = false;

        public void SetIsAttacking(bool isAttacking)
        {
            this.isAttacking = isAttacking;
        }
        public bool GetIsAttacking() => this.isAttacking;

        public void UpdateIsSelecting(bool isSelecting) { this.isSelecting = isSelecting; }

        public void Start()
        {
            meleeAttackComboInt = Animator.StringToHash(AnimatorKey.MeleeAttackCombo);
            meleeAttackTrigger = Animator.StringToHash(AnimatorKey.MeleeAttack);
            weaponInt = Animator.StringToHash(AnimatorKey.Weapon);

            GetComponent<SelectObjectBehaviour>().OnSelect += UpdateIsSelecting;
            behaviourController.SubscribeBehaviour(this);
        }

        private void Update()
        {
            if (!isSelecting && !isAttacking && Input.GetButtonDown(ButtonName.Attack) && behaviourController.IsGrounded() && behaviourController.GetAnimator.GetInteger(weaponInt) == 1)
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
