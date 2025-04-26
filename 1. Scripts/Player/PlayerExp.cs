using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class PlayerExp : MonoBehaviour
    {
        public PlayerData playerData = null;

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

            PlayerStat stat = playerData.GetCopy();
            currentLevel = stat.level;
            currentExp = stat.exp;

            maxExp = DataManager.PlayerLVData.GetCopyFromLevel(currentLevel).exp;

            OnMaxExpChanged?.Invoke(maxExp);
            OnCurrentExpChanged?.Invoke(currentExp);
            OnLevelChanged?.Invoke(currentLevel);
        }

        public void AddExp(float exp)
        {
            currentExp += exp;

            if (IsLevelUp && DataManager.PlayerLVData.IsNextLevelExist(currentLevel))
            {
                // �������� ���� ��� && ���� �ƽ� ������ �ƴϸ�
                currentLevel = currentLevel + 1;
                currentExp = currentExp - maxExp;
                maxExp = DataManager.PlayerLVData.GetCopyFromLevel(currentLevel).exp;
                OnMaxExpChanged?.Invoke(maxExp);
                OnCurrentExpChanged?.Invoke(currentLevel);
                OnLevelChanged?.Invoke(currentLevel);
            }
            else if (IsLevelUp && !DataManager.PlayerLVData.IsNextLevelExist(currentLevel))
            {
                //�������� �ߴµ� �ƽ� �����̾�
                currentExp = maxExp;
                OnCurrentExpChanged?.Invoke(currentExp);
            }
            else
            {
                // �׳� ����ġ�� ����
                OnCurrentExpChanged?.Invoke(currentExp);
            }
        }
    }

}
