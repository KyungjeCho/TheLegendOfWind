using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class GlobalAoEAttack : MonoBehaviour
    {
        public float radius = 100f;
        public LayerMask targetMask;
        public LayerMask obstacleMask;

        public Transform playerTr;
        // 1. AoE Warning Decal Projector + Dragon AoE Charging
        // 2. AoE Charging End -> Dragon AoE Attack
        // 3. Attack -> BT Gimmick True

        public WarningDecalProjectorController aoeWarningDecalProjectorController;

        public void OnAoEWarningStart()
        {
            aoeWarningDecalProjectorController.gameObject.SetActive(true);
        }

        public void OnAoEAttackStart()
        {
            // 전체 광역기 플레이어 & Rock 데미지 계산만 수행

            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetMask);
            Debug.Log(colliders.Length);
            foreach (Collider collider in colliders)
            {
                Vector3 playerPos = collider.transform.position + Vector3.up * 1.0f;
                Vector3 direction = (playerPos - transform.position).normalized;
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                if (!Physics.Raycast(transform.position, direction, distance + 0.1f, obstacleMask))
                {
                    // 플레이어 발견
                    PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.OnDamage(gameObject, 999f);
                    }
                }
                else
                {
                    
                }
            }

            colliders = Physics.OverlapSphere(transform.position, radius, obstacleMask);
            foreach (Collider collider in colliders)
            {
                RockController rockController = collider.GetComponent<RockController>();

                if (rockController != null)
                {
                    rockController.CrushRock();
                }
            }
        }

        public void OnAoEAttackEnd()
        {

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);

            if (playerTr != null)
            {
                Vector3 playerPos = playerTr.transform.position + Vector3.up * 1.0f;
                Vector3 direction = (playerPos - transform.position).normalized;
                float distance = Vector3.Distance(transform.position, playerTr.transform.position);

                if (!Physics.Raycast(transform.position, direction, distance + 0.1f, obstacleMask))
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(transform.position, playerPos);
                }
                else
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(transform.position, playerPos);
                }
            }

        }
    }
}