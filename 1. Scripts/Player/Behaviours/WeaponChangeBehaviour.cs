using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class WeaponChangeBehaviour : BaseBehaviour
    {
        // anim hash
        private int weaponInt;
        private int changeWeaponTrigger;

        private void Start()
        {
            weaponInt               = Animator.StringToHash(AnimatorKey.Weapon);
            changeWeaponTrigger     = Animator.StringToHash(AnimatorKey.ChangeWeapon);
        }

        private void Update()
        {
            if (Input.GetButtonDown(ButtonName.Weapon1))
            {
                behaviourController.GetAnimator.SetInteger(weaponInt, 1);
                behaviourController.GetAnimator.SetTrigger(changeWeaponTrigger);
            }
            if (Input.GetButtonDown(ButtonName.Weapon2))
            {
                behaviourController.GetAnimator.SetInteger(weaponInt, 2);
                behaviourController.GetAnimator.SetTrigger(changeWeaponTrigger);
            }
        }
    }

}
