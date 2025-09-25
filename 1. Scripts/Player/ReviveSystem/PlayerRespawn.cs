using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class PlayerRespawn : MonoBehaviour
    {
        private PlayerHealth playerHealth;
        private Respawn respawn;
        // Start is called before the first frame update
        void Start()
        {
            playerHealth = GetComponent<PlayerHealth>();
            respawn = new Respawn();

            respawn.LoadRespawn(transform);
        }

        public bool RespawnPlayer()
        {
            respawn = new Respawn();

            respawn.LoadRespawn(transform);

            return true;
        }

        public bool RevivePlayer()
        {
            RespawnPlayer();
            return true;
        }
    }

}
