using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace KJ
{
    public class Respawn
    {
        public bool SaveRespawnPosition(Vector3 position)
        {
            RespawnClip respawnClip = DataManager.RespawnData.GetCopyByName(SceneManager.GetActiveScene().name);
            if (respawnClip == null)
            {
                return false;
            }

            respawnClip.posX = position.x;
            respawnClip.posY = position.y;
            respawnClip.posZ = position.z;

            DataManager.RespawnData.SetData(SceneManager.GetActiveScene().name, respawnClip);
            DataManager.RespawnData.SaveData();
            return true;
        }
        public bool SaveRespawnRotation(Vector3 rotation)
        {
            RespawnClip respawnClip = DataManager.RespawnData.GetCopyByName(SceneManager.GetActiveScene().name);
            if (respawnClip == null)
            {
                return false;
            }
            respawnClip.rotX = rotation.x;
            respawnClip.rotY = rotation.y;
            respawnClip.rotZ = rotation.z;

            DataManager.RespawnData.SetData(SceneManager.GetActiveScene().name, respawnClip);
            DataManager.RespawnData.SaveData();
            return true;
        }
        public bool SaveRespawn(Transform tr)
        {
            if(SaveRespawnPosition(tr.position) && SaveRespawnRotation(tr.eulerAngles))
            {
                return true;
            }
            return false;
        }

        public bool LoadRespawn(ref Vector3 position, ref Vector3 rotation)
        {
            RespawnClip respawnClip = DataManager.RespawnData.GetCopyByName(SceneManager.GetActiveScene().name);

            if (respawnClip == null)
            {
                return false;
            }
            position.x = respawnClip.posX;
            position.y = respawnClip.posY;
            position.z = respawnClip.posZ;
            rotation.x = respawnClip.rotX;
            rotation.y = respawnClip.rotY;
            rotation.z = respawnClip.rotZ;

            return true;
        }
        public bool LoadRespawn(Transform tr)
        {
            RespawnClip respawnClip = DataManager.RespawnData.GetCopyByName(SceneManager.GetActiveScene().name);

            if (respawnClip == null)
            {
                return false;
            }

            tr.position = new Vector3(respawnClip.posX, respawnClip.posY, respawnClip.posZ);
            tr.eulerAngles = new Vector3(respawnClip.rotX, respawnClip.rotY, respawnClip.rotZ);

            return true;
        }
    }
}