using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class SpawnerController : MonoBehaviour
    {
        public bool isRandomSpawn = false;
        [Range(0.1f, 20f)]
        public float minSpawnTime = 0.1f;
        [Range(0.1f, 20f)]
        public float maxSpawnTime = 1.0f;
        public float spawnDelayTime = 5f;

        public GameObject monsterPrefab;

        private float elapsedTime = 0f;
        private GameObject monsterGameObject;

        private void Start()
        {
            if (isRandomSpawn)
            {
                spawnDelayTime = Random.Range(minSpawnTime, maxSpawnTime);
            }
        }
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