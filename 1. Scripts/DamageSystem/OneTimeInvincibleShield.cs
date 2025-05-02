using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class OneTimeInvincibleShield : IDamageHandler
    {
        private bool isUsing = false;

        public float ProcessDamage(float damage)
        {
            if (!isUsing)
            {
                isUsing = true;
                return 0;
            }

            return damage;
        }
    }
}