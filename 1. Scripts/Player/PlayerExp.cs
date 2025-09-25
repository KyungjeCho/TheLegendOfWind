using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class PlayerExp : MonoBehaviour
    {
        public PlayerData playerData = null;
        private PlayerStat playerStat = null;

        private float maxExp = 0;
        private float currentExp = 0;
        private int currentLevel = 1;

        public SoundList levelUpSound;
        public EffectList levelUpEffect;

        public event Action<float> OnCurrentExpChanged;
        public event Action<float> OnMaxExpChanged;
        public event Action<int> OnLevelChanged;

        public bool IsLevelUp => currentExp >= maxExp;
        public int CurrentLevel => currentLevel;

        private void Start()
        {
            if (playerData == null)
            {
                playerData = ScriptableObject.CreateInstance<PlayerData>();
            }
            playerData.LoadData();
            
            playerStat = playerData.Stat;
            currentLevel = playerStat.level;
            currentExp = playerStat.exp;

            maxExp = DataManager.PlayerLVData.GetCopyFromLevel(currentLevel).exp;

            OnMaxExpChanged?.Invoke(maxExp);
            OnCurrentExpChanged?.Invoke(currentExp);
            OnLevelChanged?.Invoke(currentLevel);
        }

        public void AddExp(float exp)
        {
            currentExp += exp;

            DropQueue.Instance.AddExp(exp.ToString());
            if (IsLevelUp && DataManager.PlayerLVData.IsNextLevelExist(currentLevel))
            {
                // 레벨업을 했을 경우 && 아직 맥스 레벨이 아니면
                currentLevel = currentLevel + 1;
                currentExp = currentExp - maxExp;
                maxExp = DataManager.PlayerLVData.GetCopyFromLevel(currentLevel).exp;
                OnMaxExpChanged?.Invoke(maxExp);
                OnCurrentExpChanged?.Invoke(currentLevel);
                OnLevelChanged?.Invoke(currentLevel);
            }
            else if (IsLevelUp && !DataManager.PlayerLVData.IsNextLevelExist(currentLevel))
            {
                //레벨업을 했는데 맥스 레벨일 경우
                currentExp = maxExp;
                OnCurrentExpChanged?.Invoke(currentExp);
            }
            else
            {
                // 그냥 경험치만 얻음
                OnCurrentExpChanged?.Invoke(currentExp);
            }

            SaveLevel();
        }

        public void SaveLevel()
        {
            playerData.Stat.exp = currentExp;
            playerData.Stat.level = currentLevel;
            Debug.Log(currentLevel + " " + playerData.Stat.level);
            playerData.SaveData();
        }
    }

}
