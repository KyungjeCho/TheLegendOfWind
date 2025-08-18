using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class SkillBehaviour : BaseBehaviour
    {
        [SerializeField] private BaseSkill shieldSkill;
        [SerializeField] private BaseSkill timeStopSkill;
        [SerializeField] private BaseSkill rewindSkill;

        private void Start()
        {
            if (shieldSkill == null)
            {
                shieldSkill = ScriptableObject.CreateInstance<BaseSkill>();
            }
            shieldSkill.SetPlayerTransform(transform);

            if (timeStopSkill == null)
            {
                timeStopSkill = ScriptableObject.CreateInstance<BaseSkill>();
            }
            timeStopSkill.SetPlayerTransform(transform);

            if (rewindSkill == null)
            {
                rewindSkill = ScriptableObject.CreateInstance<BaseSkill>();
            }
            rewindSkill.SetPlayerTransform(transform);
        }

        private void Update()
        {
            if (InputManager.Instance.SkillQButton.IsButtonPressed)
            {
                timeStopSkill.UseSkill();
            }
            if (InputManager.Instance.SkillEButton.IsButtonPressed)
            {
                shieldSkill.UseSkill();
            }
            if (InputManager.Instance.SkillRButton.IsButtonPressed)
            {
                rewindSkill.UseSkill();
            }

            shieldSkill.UpdateSkill(Time.deltaTime);
            timeStopSkill.UpdateSkill(Time.deltaTime);
            rewindSkill.UpdateSkill(Time.deltaTime);
        }
    }
}
