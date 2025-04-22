using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    /// <summary>
    /// Player Health View
    /// ������ ���ϰ� MVC���� View ��Ʈ
    /// Observer �� PlayerHealth
    /// </summary>
    public class UIPlayerHealth : MonoBehaviour
    {
        [SerializeField]
        private Slider playerHpBar;
        [SerializeField]
        private PlayerHealth playerHealth;

        private void Awake()
        {
            playerHealth.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(1f);
        }
        public void UpdateHealthBar(float health)
        {
            playerHpBar.value = health;
        }
    }
}
