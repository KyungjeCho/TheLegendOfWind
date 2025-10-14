using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    public class UIBossHealth : MonoBehaviour
    {
        public Slider hpSlider;
        public BossController controller;

        // Start is called before the first frame update
        void Start()
        {
            controller.OnHealthChanged += UpdateUI;
        }

        public void UpdateUI(float health)
        {
            hpSlider.value = health;
        }
    }

}
