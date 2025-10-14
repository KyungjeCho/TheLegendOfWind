using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class CrawAttack : MonoBehaviour
    {
        public GameObject homingRockPrefab;

        public Transform spawnTr;
        public Transform playerTr;

        public void SpawnRock()
        {
            GameObject go = Instantiate(homingRockPrefab, spawnTr.position, Quaternion.identity);
            HomingRockController controller = go.GetComponent<HomingRockController>();
            controller.Init(transform, playerTr);
        }
    }

}
