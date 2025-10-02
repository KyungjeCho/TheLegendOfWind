using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class GlobalAoEAttack : MonoBehaviour
    {
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
            // ��ü ������ �÷��̾� & Rock ������ ��길 ����
        }

        public void OnAoEAttackEnd()
        {

        }
    }
}