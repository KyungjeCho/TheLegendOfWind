using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class SkillBehaviour : BaseBehaviour
    {
        [SerializeField] private BaseSkill shieldSkill;

        private void Start()
        {
            if (shieldSkill == null)
            {
                shieldSkill = ScriptableObject.CreateInstance<BaseSkill>();
            }
            shieldSkill.SetPlayerTransform(transform);
        }

        private void Update()
        {
            if (Input.GetButtonDown(ButtonName.Skill1))
            {

            }
            if (Input.GetButtonDown(ButtonName.Skill2))
            {
                shieldSkill.UseSkill();
            }
            if (Input.GetButtonDown(ButtonName.Skill3))
            {

            }

            shieldSkill.UpdateSkill(Time.deltaTime);
        }
    }
}
