using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public abstract class BaseSkill : ScriptableObject
    {
        [SerializeField] private string skillName;
        [SerializeField] private string skillDescription;

        public abstract void UseSkill();
    }
}