using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class DamagePipeline 
    {
        public List<IDamageModifier> modifiers = new List<IDamageModifier>();

        public void AddModifier(IDamageModifier modifier)
        {
            modifiers.Add(modifier);
        }
        public void RemoveModifier(IDamageModifier modifier)
        {
            modifiers.Remove(modifier);
        }
        public float Calculate(float damage)
        {
            foreach(IDamageModifier modifier in modifiers)
            {
                modifier.Apply(ref damage);
            }

            return damage;
        }
    }
}

