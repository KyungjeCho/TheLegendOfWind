using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "Collecting Requirement", menuName = "ScriptableObjects/Requirements/Collecting Requirement")]
    public class CollectingRequirementSO : BaseRequirementSO
    {
        public ItemSO targetItemSO;
        public int requireCount;

        public override Requirement CreateRequirement(Quest q)
        {
            return new CollectingRequirement(this, q);
        }
    }
}
