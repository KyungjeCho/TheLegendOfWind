using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public enum PlayerAttribute
    {
        HP = 0,
        MANA = 1,
        STEMINA = 2,
        ATTACK = 3,
        DEFENSE = 4,
        SPEED = 5,
    }
    [Serializable]
    public class ItemBuff : IModifier
    {
        public PlayerAttribute stat;
        public float value;

        public ItemBuff(float value)
        {
            this.value = value;
        }

        public void AddValue(ref float baseValue)
        {
            baseValue += value;
        }
    }

}
