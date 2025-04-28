using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    /// <summary>
    /// 경험치 레벨 UI 뷰
    /// 
    /// </summary>
    public class UIPlayerExpLevel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI levelText;
        [SerializeField]
        private Slider expSlider;
        [SerializeField]
        private TextMeshProUGUI expText;

        [SerializeField]
        private PlayerExp playerExp;

        private float maxExpValue;

        private void Awake()
        {
            playerExp.OnCurrentExpChanged += UpdateCurrentExp;
            playerExp.OnMaxExpChanged += UpdateMaxExp;
            playerExp.OnLevelChanged += UpdateLevel;

        }
        public void UpdateLevel(int level)
        {
            levelText.text = "Level : " + level;
        }
        public void UpdateMaxExp(float amount)
        {
            maxExpValue = amount;
            expSlider.maxValue = amount;
        }
        public void UpdateCurrentExp(float amount)
        {
            expSlider.value = amount;
            expText.text = amount.ToString("n0") + " / " + maxExpValue.ToString("n0");
        }
    }

}
