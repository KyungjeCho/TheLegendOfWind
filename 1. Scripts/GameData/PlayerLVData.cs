using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace KJ
{
    public class PlayerLVData : BaseData<PlayerLVStat>
    {
        public override int AddData(string name)
        {
            if (names == null)
            {
                names = new string[] { name };
                data = new PlayerLVStat[] { new PlayerLVStat() };

            }
            else
            {
                names = ArrayHelper.Add(name, names);
                data = ArrayHelper.Add(new PlayerLVStat(), data);
            }
            return GetDataCount();
        }
        public override void RemoveData(int index)
        {
            if (names.Length == 0)
            {
                names = null;
                data = null;
                return;
            }

            names = ArrayHelper.Remove(index, names);
            data = ArrayHelper.Remove(index, data);
        }

        public PlayerLVStat GetCopy(int index)
        {
            if (index < 0 || index >= data.Length)
                return null;

            PlayerLVStat copy = new PlayerLVStat();
            PlayerLVStat original = data[index];
            copy.level = original.level;
            copy.maxHp = original.maxHp;
            copy.maxMana = original.maxMana;   
            copy.maxStemina = original.maxStemina;  
            copy.attack = original.attack;
            copy.defense = original.defense;
            copy.exp = original.exp;
            return copy;
        }

        public PlayerLVStat GetCopyFromLevel(int level)
        {
            if (level < 0 || level - 1 >= data.Length)
            {
                return null;
            }

            PlayerLVStat copy = new PlayerLVStat();
            PlayerLVStat original = data[level - 1];
            copy.level = original.level;
            copy.maxHp = original.maxHp;
            copy.maxMana = original.maxMana;
            copy.maxStemina = original.maxStemina;
            copy.attack = original.attack;
            copy.defense = original.defense;
            copy.exp = original.exp;
            return copy;
        }

        /// <summary>
        /// 다음 레벨이 존재하는지 확인하는 메소드
        /// </summary>
        /// <param name="level">확인하고자 하는 레벨</param>
        /// <returns></returns>
        public bool IsNextLevelExist(int level) 
        {
            return level < data.Length;
        }
    }
}