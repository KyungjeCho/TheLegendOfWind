using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class SaveNonPlayerControlelr : InteractComponent
    {
        private Respawn respawn;

        public Vector3 respawnPos;
        public Vector3 respawnDir;
        public PlayerData playerData;

        // Start is called before the first frame update
        protected override void Start()
        {
            respawn = new Respawn();
        }

        public override void Interact()
        {
            Debug.Log("Save Pos Rot");
            respawn.SaveRespawnPosition(respawnPos);
            respawn.SaveRespawnRotation(respawnDir);
            // �÷��̾� LV ������ ����
            playerData.SaveData();
            // todo: ���� UI
        }
    }
}