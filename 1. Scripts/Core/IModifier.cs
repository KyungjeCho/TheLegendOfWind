using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public interface IModifier 
    {
        public void AddValue(ref float baseValue);
    }

}
