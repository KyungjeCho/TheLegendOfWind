using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public abstract class BaseSkill : ScriptableObject
    {
        [SerializeField] private string skillName;
        [SerializeField] private string skillDescription;
        [SerializeField] private Transform playerTranform;

        public virtual void SetPlayerTransform(Transform transform)
        {
            playerTranform = transform;
        }
        public abstract void UseSkill();
    }
}