using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    public class UIBossHealth : MonoBehaviour
    {
        public Slider hpSlider;
        public Text monsterNameText;
        public BossController controller;
        public MonsterList monsterList;

        // Start is called before the first frame update
        void Start()
        {
            controller.OnHealthChanged += UpdateUI;
            monsterNameText.text = DataManager.MonsterData.names[(int)monsterList];
            controller.onDragonDie += UpdateUnactiveUI;
        }

        public void UpdateUI(float health)
        {
            hpSlider.value = health;
        }

        public void UpdateUnactiveUI()
        {
            gameObject.SetActive(false);
        }
    }

}
