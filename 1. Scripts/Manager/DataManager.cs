using KJ;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static SoundData soundData = null;

    private void Start()
    {
        if (soundData == null)
        {
            soundData = ScriptableObject.CreateInstance<SoundData>();
            soundData.LoadData();
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

}
