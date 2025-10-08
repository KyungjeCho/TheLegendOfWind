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
            foreach (Collider collider in colliders)
            {
                Vector3 direction = (collider.transform.position - transform.position).normalized;
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                if (!Physics.Raycast(transform.position, direction, distance + 0.1f, obstacleMask))
                {
                    // 플레이어 발견
                    Debug.Log("todo : 플레이어 데미지 설정");
                }
                else
                {
                    // 장애물
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
        }
    }
}