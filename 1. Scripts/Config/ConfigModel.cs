using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    // todo : Mono -> Scripable
    [CreateAssetMenu(fileName = "Config Model", menuName = "ScriptableObjects/Config/Config Model")]
    public class ConfigModel : ScriptableObject
    {
        public const string SCREEN_MODE = "ScreenMode";
        public const string IS_WINDOW = "IsWindow";
        public const string RESOLUTION_IDX = "Resolution Index";
        public const string MOUSE_SENSITIVITY = "MouseSensitivity";

        public ResolutionDBSO resolutionDB;
        public int resolutionIdx = 0;
        public FullScreenMode screenMode = FullScreenMode.ExclusiveFullScreen;
        public bool isWindow = false;
        public ResolutionSO resolutionSO;
        
        public float masterVolume;
        public float bgmVolume;
        public float sfxVolume;
        public float uiVolume;

        public float mouseSensitivity = 1.0f;

        public event Action<float> OnMouseSensitivityChanged;

        private bool isLoaded = false;

        public void OnEnable()
        {
            if (!isLoaded)
            {
                Init();
            }
        }

        public void Init()
        {
            masterVolume = SoundManager.GetOrCreateInstance().GetMasterVolume();
            bgmVolume = SoundManager.GetOrCreateInstance().GetBGMVolume();
            sfxVolume = SoundManager.GetOrCreateInstance().GetSFXVolume();

            screenMode = LoadScreenMode();
            resolutionIdx = LoadResolutionIdx();
            mouseSensitivity = LoadMouseSensitivity();

            OnMouseSensitivityChanged?.Invoke(mouseSensitivity);

            if (resolutionDB.container != null && resolutionDB.container.Count > resolutionIdx)
            {
                resolutionSO = resolutionDB.container[resolutionIdx];
            }
            else
            {
                Debug.Log("Resolution DB container is not Exist. OR Resolution IDX is over a size of container.");
            }
        }

        public FullScreenMode LoadScreenMode()
        {
            int mode = PlayerPrefs.GetInt(SCREEN_MODE, (int)FullScreenMode.ExclusiveFullScreen);

            isWindow = (FullScreenMode) mode == FullScreenMode.Windowed ? true : false;

            return (FullScreenMode) mode;
        }
        public void SaveScreenMode()
        {
            int mode = isWindow ? (int)FullScreenMode.Windowed : (int)FullScreenMode.ExclusiveFullScreen;

            PlayerPrefs.SetInt(SCREEN_MODE, mode);
        }

        public int LoadResolutionIdx()
        {
            return PlayerPrefs.GetInt(RESOLUTION_IDX, 0);
        }
        public void SaveResolutionIdx()
        {
            PlayerPrefs.SetInt(RESOLUTION_IDX, resolutionIdx);
        }

        public float LoadMouseSensitivity()
        {
            return PlayerPrefs.GetFloat(MOUSE_SENSITIVITY, 1f);
        }
        public void SaveMouseSensitivity()
        {
            PlayerPrefs.SetFloat(MOUSE_SENSITIVITY, mouseSensitivity);
        }

        public void SetScreenMode(bool isWindow)
        {
            this.isWindow = isWindow;
            this.screenMode = isWindow ? FullScreenMode.Windowed : FullScreenMode.ExclusiveFullScreen;

            // Screen Manager 호출 -> 변경

            ScreenManager.GetOrCreateInstance().SetScreen(resolutionSO, screenMode);
            SaveScreenMode();
        }

        public void SetResolutionIdx(int idx)
        {
            this.resolutionIdx = idx;

            if (resolutionDB.container != null && resolutionDB.container.Count > resolutionIdx)
            {
                resolutionSO = resolutionDB.container[resolutionIdx];
            }
            else
            {
                Debug.Log("Resolution DB container is not Exist. OR Resolution IDX is over a size of container.");
            }

            ScreenManager.GetOrCreateInstance().SetScreen(resolutionSO, screenMode);
            SaveResolutionIdx();
        }
        public void SetMasterVolume(float volume)
        {
            masterVolume = volume;

            SoundManager.GetOrCreateInstance().SetMasterVolume(masterVolume);
        }
        public void SetBGMVolume(float volume)
        {
            bgmVolume = volume;

            SoundManager.GetOrCreateInstance().SetMasterVolume(bgmVolume);
        }
        public void SetSFXVolume(float volume)
        {
            sfxVolume = volume;

            SoundManager.GetOrCreateInstance().SetSFXVolume(sfxVolume);
        }

        public void SetMouseSensitivity(float sensitivity)
        {
            mouseSensitivity = sensitivity;

            // Invoke -> ThirdOrbitCam sens change
            OnMouseSensitivityChanged?.Invoke(mouseSensitivity);
            SaveMouseSensitivity();
        }


    }
}