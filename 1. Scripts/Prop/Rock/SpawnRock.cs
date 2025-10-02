using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class SpawnRock : MonoBehaviour
    {
        public GameObject rockPrefab;

        public void Spawn()
        {
            Vector3 spawnPos = transform.position + Vector3.up * 30f;
            Instantiate(rockPrefab, spawnPos, Quaternion.identity);
        }
    }

}
