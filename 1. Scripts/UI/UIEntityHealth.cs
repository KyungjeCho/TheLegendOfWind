using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    public class UIEntityHealth : MonoBehaviour
    {
        [SerializeField]
        private Slider entityHealthBar;
        [SerializeField]
        private EnemyController enemyController;

        private Transform mainCam;

        private void Start()
        {
            enemyController.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(1f);

            mainCam = Camera.main.transform;
        }

        private void LateUpdate()
        {
            Vector3 dir = transform.position - mainCam.position;
            dir.y = 0f;
            transform.rotation = Quaternion.LookRotation(dir);
        }

        public void UpdateHealthBar(float health)
        {
            entityHealthBar.value = health;
        }
    }

}
