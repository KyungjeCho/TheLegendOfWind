using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public abstract class BaseRequirementSO : ScriptableObject
    {
        public abstract Requirement CreateRequirement();
    }
}