using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace KJ
{
    public class RockSlide : MonoBehaviour
    {
        [Range(1, 10)]
        public int rockCount = 1;
        [Range(1f, 50f)]
        public float minSpawnRadius =  3f;
        [Range(1f, 50f)]
        public float maxSpawnRadius = 10f;

        public WarningDecalProjectorController warningDecalProjectorPrefab;
        
        public void OnStartGimmick()
        {
            Vector3[] rockPosArray = new Vector3[rockCount];

            for (int i = 0; i < rockCount; i++)
            {
                Vector3 rockPos = GenerateRandomPos();
                bool isPassed = true;

                for (int j = 0; j < i; j++)
                {
                    float distance = Vector3.Distance(rockPos, rockPosArray[j]);

                    if (distance < warningDecalProjectorPrefab.radius)
                    {
                        isPassed = false;
                        break;
                    }
                }
                if (isPassed)
                {
                    rockPosArray[i] = rockPos;
                }
                else
                {
                    i--;
                }
            }

            for (int i = 0; i < rockCount; i++)
            {
                Vector3 finalPos = transform.position + rockPosArray[i];

                Instantiate(warningDecalProjectorPrefab, finalPos, Quaternion.identity);
            }
        }

        public Vector3 GenerateRandomPos()
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float radius = Random.Range(minSpawnRadius, maxSpawnRadius);

            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            return new Vector3(x, 0f, z);
        }

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Vector3.zero, minSpawnRadius);
            Gizmos.DrawWireSphere(Vector3.zero, maxSpawnRadius);
        }
    }
}