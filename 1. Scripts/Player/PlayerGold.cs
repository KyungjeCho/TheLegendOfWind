using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class PlayerGold : MonoBehaviour
    {
        public PlayerData playerData = null;

        private int gold = 0;

        public event Action<int> OnCurrentGoldChanged;

        public int Gold => gold;

        private void Start()
        {
            if (playerData == null)
            {
                playerData = ScriptableObject.CreateInstance<PlayerData>();
            }
            playerData.LoadData();

            PlayerStat stat = playerData.GetCopy();
            gold = stat.gold;

            OnCurrentGoldChanged += SaveGold;
            OnCurrentGoldChanged?.Invoke(gold);
        }

        public void AddGold(int gold)
        {
            this.gold += gold;
            DropQueue.Instance.AddGold(gold.ToString());
            OnCurrentGoldChanged?.Invoke(this.gold);
        }
        public bool RemoveGold(int gold)
        {
            if (this.gold - gold < 0)
            {
                return false;
            }

            this.gold -= gold;
            OnCurrentGoldChanged?.Invoke(this.gold);
            return true;
        }
        public void PublishGold()
        {
            OnCurrentGoldChanged?.Invoke(gold);
        }
        public void SaveGold(int gold)
        {
            playerData.Stat.gold = gold;
            playerData.SaveData();
        }
    }
}
