using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public interface IDamageModifier 
    {
        void Apply(ref float damage);
    }

    public class InvincibleModifier : IDamageModifier
    {
        public void Apply(ref float damage)
        {
            damage = 0;
        }
    }
}
