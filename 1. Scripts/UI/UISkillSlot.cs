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
            DataManager.UnlockData.OnUnlock += UpdateSkillActive;

            SetActive(DataManager.UnlockData.GetIsUnlocked(skillSO.unlockList));
        }
        private void Update()
        {
            if (DataManager.UnlockData.GetIsUnlocked(skillSO.unlockList))
            //if ((skillSO.SkillName == "TimeStopSkill" && DataManager.UnlockData.data[(int)UnlockList.SkillQUnlock].isUnlocked == true) ||
            //    (skillSO.SkillName == "쉴드 스킬" && DataManager.UnlockData.data[(int)UnlockList.SkillEUnlock].isUnlocked == true) ||
            //    (skillSO.SkillName == "되돌리기" && DataManager.UnlockData.data[(int)UnlockList.SkillRUnlock].isUnlocked == true)
            //    )
            {
                if (timer > Mathf.Epsilon)
                {
                    timer -= Time.deltaTime;
                    UpdateCooldownTime(timer);
                }
            }
        }

        public void UpdateSkillActive(UnlockList unlockList, bool isUnlocked)
        {
            if (skillSO.unlockList != unlockList)
            {
                return;
            }

            SetActive(isUnlocked);
        }

        public void SetActive(bool isUnlocked)
        {
            gameObject.SetActive(isUnlocked);
            //if (isUnlocked)
            //{
            //    gameObject.SetActive(isUnlocked);
            //}
            //else
            //{
                
            //}
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
