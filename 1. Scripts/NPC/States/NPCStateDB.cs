using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "NPC State Database", menuName = "ScriptableObjects/NPC/States/NPC State Database")]
    public class NPCStateDB : ScriptableObject
    {
        public NPCState[] container;

        public NPCState GetNPCState(string name)
        {
            foreach(NPCState n in container)
            {
                if (n.name == name) return n;
            }
            return null;
        }
    }
}
