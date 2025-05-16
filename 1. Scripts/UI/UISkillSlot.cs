using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    public class UISkillSlot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI cooldownText;
        [SerializeField] private Image cooldownImage;
        [SerializeField] private BaseSkill skillSO;

        private float timer = 60;
        private float cooldownTime = 60;
        public float CooldownTime => cooldownTime;

        private void Start()
        {
            UpdateCooldownTime(0f);

            cooldownTime = skillSO.CooldownTime;
            timer = cooldownTime;
                
            skillSO.OnSkillExecuted += UpdateTimer;
        }
        private void Update()
        {
            if (timer > Mathf.Epsilon)
            {
                timer -= Time.deltaTime;
                UpdateCooldownTime(timer);
            }
        }
        // Event 
        public void UpdateCooldownTime(float time)
        {
            if (time < Mathf.Epsilon)
            {
                cooldownText.text = string.Empty;
            }
            else
            {
                cooldownText.text = time.ToString("n0");
            }
            cooldownImage.fillAmount = time / cooldownTime;
        }
        public void UpdateTimer()
        {
            timer = cooldownTime;
        }
    }
}
