using KJ;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KJ
{
    public class WeaponChangeBehaviour : BaseBehaviour
    {
        // anim hash
        private int weaponInt;
        private int changeWeaponTrigger;

        [SerializeField]
        private InventorySO equipmentSO;

        private void Start()
        {
            weaponInt               = Animator.StringToHash(AnimatorKey.Weapon);
            changeWeaponTrigger     = Animator.StringToHash(AnimatorKey.ChangeWeapon);
        }

        private void Update()
        {
            if (Input.GetButtonDown(ButtonName.Weapon1) && equipmentSO.Slots[(int)EquipmentList.MeleeWeapon].item.id > -1)
            {
                behaviourController.GetAnimator.SetInteger(weaponInt, 1);
                behaviourController.GetAnimator.SetTrigger(changeWeaponTrigger);
            }
            if (Input.GetButtonDown(ButtonName.Weapon2) && equipmentSO.Slots[(int)EquipmentList.RangeWeapon].item.id > -1)
            {
                behaviourController.GetAnimator.SetInteger(weaponInt, 2);
                behaviourController.GetAnimator.SetTrigger(changeWeaponTrigger);
            }
        }
    }

}
