using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class SpawnerController : MonoBehaviour
    {
        public float spawnDelayTime = 5f;

        public GameObject monsterPrefab;

        private float elapsedTime = 0f;
        private GameObject monsterGameObject;

        // Update is called once per frame
        void Update()
        {
            if (monsterGameObject == null)
            {
                elapsedTime += Time.deltaTime;
            }

            if (elapsedTime > spawnDelayTime)
            {
                elapsedTime = 0f;

                monsterGameObject = Instantiate(monsterPrefab, transform.position, transform.rotation);
            }
        }
    }
}