using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [Serializable]
    public class NPCClip
    {
        public GameObject npcPrefab = null;
        public string clipName = string.Empty;
        public string clipPath = string.Empty;

        public NPCClip() { }

        public void PreLoad()
        {
            if (npcPrefab == null)
            {
                npcPrefab = Resources.Load<GameObject>(clipPath + clipName);
            }
        }
    }
}
