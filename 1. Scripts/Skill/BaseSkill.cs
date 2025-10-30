using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public abstract class BaseSkill : ScriptableObject
    {
        public SoundList useSound;
        public UnlockList unlockList;
        [SerializeField] private string skillName;
        [SerializeField] private string skillDescription;
        [SerializeField] protected Transform playerTranform;
        [SerializeField] private float cooldownTime = 60f;

        private float timer = 60f;

        public event Action OnSkillExecuted;

        public string SkillName => skillName;
        public string SkillDescription => skillDescription;
        public float CooldownTime => cooldownTime;
        public float Timer => timer;

        private void OnValidate()
        {
            timer = cooldownTime;
            DataManager.UnlockData.OnUnlock += UpdateSkill;
        }
        public virtual void SetPlayerTransform(Transform transform)
        {
            playerTranform = transform;
        }
        public virtual void UpdateSkill(float deltaTime)
        {
            timer -= deltaTime;
        }
        public abstract void UseSkill();

        public void StartSkill()
        {
            timer = cooldownTime;
            OnSkillExecuted?.Invoke();
        }
        public void UpdateSkill(UnlockList unlockList, bool isUnlocked)
        {
            if (this.unlockList != unlockList)
            {
                return;
            }

            if (isUnlocked)
            {

            }
            else
            {

            }
        }
    }
}