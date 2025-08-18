using KJ.CameraControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class NPCDialogCamera : MonoBehaviour, IGetTargetPos, IGetTargetRot
    {
        public Transform npcTr;

        [SerializeField]
        private Vector3 camOffset;
        [SerializeField]
        private float height; // todo: change NPC Object

        public void SetNPCTransform(Transform npcTr)
        {
            this.npcTr = npcTr;
        }

        public Vector3 GetPos()
        {
            if (npcTr == null)
            {
                return Vector3.zero;
            }

            return npcTr.position + camOffset;
        }

        public Quaternion GetRot()
        {
            if (npcTr == null)
            {
                return Quaternion.identity;
            }
            Vector3 pos = npcTr.position;
            pos.y = height;
            Quaternion rot = Quaternion.LookRotation(pos - (npcTr.position + camOffset));
            return rot;
        }
    }
}