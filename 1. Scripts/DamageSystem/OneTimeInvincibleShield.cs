using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class OneTimeInvincibleShield : IDamageHandler
    {
        private bool isUsing = true;
        private ShieldSkill parent;

        public bool IsUsing { get => isUsing; set => isUsing = value; }
        public OneTimeInvincibleShield(ShieldSkill shieldSkill)
        {
            parent = shieldSkill;
        }

        public float ProcessDamage(float damage)
        {
            if (!isUsing)
            {
                isUsing = true;
                parent.StopAllShieldEffect();
                return 0;
            }

            return damage;
        }
    }
}