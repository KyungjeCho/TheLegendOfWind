using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    /// <summary>
    /// Player Health View
    /// 옵저버 패턴과 MVC에서 View 파트
    /// Observer 로 PlayerHealth
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
