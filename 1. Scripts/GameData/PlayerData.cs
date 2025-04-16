using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [Serializable]
    public class PlayerStat
    {
        public int level            = 1;
        public float hp             = 0f;
        public float mana           = 0f;
        public float exp            = 0f;
        public int gold             = 0;
        public int remainSkillPoint = 0;
    }

    public class PlayerData : ScriptableObject
    {
        public const string dataDirectory = "Assets/9. Resources/Resources/Data";

        public PlayerStat stat;

        public void SaveData()
        {

        }
        public void LoadData()
        {

        }
    }

}

