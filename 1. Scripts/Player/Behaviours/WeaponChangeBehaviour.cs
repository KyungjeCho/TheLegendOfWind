using System;
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

        public event Action<int> OnWeaponChanged;

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
                //OnWeaponChanged?.Invoke(1);
            }
            if (Input.GetButtonDown(ButtonName.Weapon2) && equipmentSO.Slots[(int)EquipmentList.RangeWeapon].item.id > -1)
            {
                behaviourController.GetAnimator.SetInteger(weaponInt, 2);
                behaviourController.GetAnimator.SetTrigger(changeWeaponTrigger);
                //OnWeaponChanged?.Invoke(2);
            }
        }
        public void ChangeWeapon(int weapon)
        {
            OnWeaponChanged?.Invoke(weapon);
        }
    }

}
