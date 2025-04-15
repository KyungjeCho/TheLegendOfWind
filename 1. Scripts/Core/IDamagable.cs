using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public interface IDamagable 
    {
        public bool IsAlive { get; }
        public abstract void OnDamage(IAttackable enemy);
    }
}

