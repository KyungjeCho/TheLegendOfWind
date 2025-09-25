using KJ;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    private static SoundData soundData = null;
    private static MonsterData monsterData = null;
    private static EffectData effectData = null;
    private static PlayerLVData playerLVData = null;
    public ItemDBSO itemDBSO = null;
    private static UnlockData unlockData = null;
    private static RespawnData respawnData = null;

    private void Start()
    {
        if (soundData == null)
        {
            soundData = ScriptableObject.CreateInstance<SoundData>();
            soundData.LoadData();
        }
        if (monsterData == null)
        {
            monsterData = ScriptableObject.CreateInstance<MonsterData>();
            monsterData.LoadData();
        }
        if (effectData == null)
        {
            effectData = ScriptableObject.CreateInstance<EffectData>();
            effectData.LoadData();
        }
        if (playerLVData == null)
        {
            playerLVData = ScriptableObject.CreateInstance<PlayerLVData>();
            playerLVData.LoadData();
        }
        if (unlockData == null)
        {
            unlockData = ScriptableObject.CreateInstance<UnlockData>();
            unlockData.LoadData();
        }
        if (respawnData == null)
        {
            respawnData = ScriptableObject.CreateInstance<RespawnData>();
            respawnData.LoadData();
        }
    }

    public static SoundData SoundData
    {
        get
        {
            if (soundData == null) 
            {
                soundData = ScriptableObject.CreateInstance<SoundData>();
                soundData.LoadData();
            }
            return soundData;
        }
    }
    public static MonsterData MonsterData
    {
        get
        {
            if (monsterData == null)
            {
                monsterData = ScriptableObject.CreateInstance<MonsterData>();
                monsterData.LoadData();
            }
            return monsterData;
        }
    }
    public static EffectData EffectData
    {
        get
        {
            if (effectData == null)
            {
                effectData = ScriptableObject.CreateInstance<EffectData>();
                effectData.LoadData();
            }
            return effectData;
        }
    }
    public static PlayerLVData PlayerLVData
    {
        get
        {
            if (playerLVData == null)
            {
                playerLVData = ScriptableObject.CreateInstance<PlayerLVData>();
                playerLVData.LoadData();
            }
            return playerLVData;
        }
    }
    public ItemDBSO ItemDBSO => itemDBSO;

    public static UnlockData UnlockData
    {
        get
        {
            if (unlockData == null)
            {
                unlockData = ScriptableObject.CreateInstance<UnlockData>();
                unlockData.LoadData();
            }
            return unlockData;
        }
    }

    public static RespawnData RespawnData
    {
        get
        {
            if (respawnData == null)
            {
                respawnData = ScriptableObject.CreateInstance<RespawnData>();
                respawnData.LoadData();
            }
            return respawnData;
        }
    }
}
