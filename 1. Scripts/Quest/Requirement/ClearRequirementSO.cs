using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "Clear Requirement", menuName = "ScriptableObjects/Requirements/Clear Requirement")]
    public class ClearRequirementSO : BaseRequirementSO
    {
        public UnlockList unlockList;

        public override Requirement CreateRequirement(Quest q)
        {
            return new ClearRequirement(this, q);
        }
    }
}