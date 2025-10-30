using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class PlayerBuff : MonoBehaviour
    {
        public float maxHP = 0;
        public float attack = 0;
        public float defense = 0;
        public float mana = 0;
        public float stemina = 0;
        public float speed = 0;

        public event Action<PlayerAttribute, float> OnValueChanged;

        public List<ItemSO> itemSOs;

        // Start is called before the first frame update
        void Start()
        {
            itemSOs = new List<ItemSO>();
        }

        public void Add(ItemSO itemSO)
        {
            if (itemSO != null && !itemSOs.Contains(itemSO) && itemSO.data != null && itemSO.data.buffs != null)
            {
                foreach (ItemBuff buff in itemSO.data.buffs)
                {
                    switch (buff.stat)
                    {
                        case PlayerAttribute.HP:
                            maxHP += buff.value;
                            OnValueChanged?.Invoke(PlayerAttribute.HP, maxHP);
                            break;
                        case PlayerAttribute.ATTACK:
                            attack += buff.value;
                            OnValueChanged?.Invoke(PlayerAttribute.ATTACK, attack);
                            break;
                        case PlayerAttribute.DEFENSE:
                            defense += buff.value;
                            OnValueChanged?.Invoke(PlayerAttribute.DEFENSE, defense);
                            break;
                        case PlayerAttribute.MANA:
                            mana += buff.value;
                            OnValueChanged?.Invoke(PlayerAttribute.MANA, mana);
                            break;
                        case PlayerAttribute.STEMINA:
                            stemina += buff.value;
                            OnValueChanged?.Invoke(PlayerAttribute.STEMINA, stemina);
                            break;
                        case PlayerAttribute.SPEED:
                            speed += buff.value;
                            OnValueChanged?.Invoke(PlayerAttribute.SPEED, speed);
                            break;
                    }
                }
                itemSOs.Add(itemSO);
            }
        }

        public void Remove(ItemSO itemSO)
        {
            if (itemSO != null && itemSOs.Contains(itemSO))
            {
                foreach (ItemBuff buff in itemSO.data.buffs)
                {
                    switch (buff.stat)
                    {
                        case PlayerAttribute.HP:
                            maxHP -= buff.value;
                            OnValueChanged?.Invoke(PlayerAttribute.HP, maxHP);
                            break;
                        case PlayerAttribute.ATTACK:
                            attack -= buff.value;
                            OnValueChanged?.Invoke(PlayerAttribute.ATTACK, attack);
                            break;
                        case PlayerAttribute.DEFENSE:
                            defense -= buff.value;
                            OnValueChanged?.Invoke(PlayerAttribute.DEFENSE, defense);
                            break;
                        case PlayerAttribute.MANA:
                            mana -= buff.value;
                            OnValueChanged?.Invoke(PlayerAttribute.MANA, mana);
                            break;
                        case PlayerAttribute.STEMINA:
                            stemina -= buff.value;
                            OnValueChanged?.Invoke(PlayerAttribute.STEMINA, stemina);
                            break;
                        case PlayerAttribute.SPEED:
                            speed -= buff.value;
                            OnValueChanged?.Invoke(PlayerAttribute.SPEED, speed);
                            break;
                    }
                }
                itemSOs.Remove(itemSO);
            }
        }
    }

}
