using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "Hunting Requirement", menuName = "ScriptableObjects/Requirements/Hunting Requirement")]
    public class HuntingRequirementSO : BaseRequirementSO
    {
        public MonsterList targetMonster;
        public int requireCount;

        public override Requirement CreateRequirement(Quest q)
        {
            return new HuntingRequirement(this, q);
        }
    }
}