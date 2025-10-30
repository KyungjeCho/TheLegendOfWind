using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static UnityEngine.Rendering.DebugUI;

namespace KJ
{
    public class UIConfig : MonoBehaviour
    {
        public ConfigModel model;

        public GameObject windowSlot;
        private Toggle windowToggle;

        public GameObject resolutionSlot;
        private Dropdown resolutionDropdown;

        public GameObject masterVolumeSlot;
        private Slider masterVolumeSlider;

        public GameObject bgmVolumeSlot;
        private Slider bgmVolumeSlider;

        public GameObject sfxVolumeSlot;
        private Slider sfxVolumeSlider;

        public GameObject uiVolumeSlot;
        private Slider uiVolumeSlider;

        public UnityEngine.UI.Button moveTitleBtn;
        public UnityEngine.UI.Button backToGameBtn;

        public GameObject mouseSensitivitySlot;
        private Slider mouseSensitivitySlider;
        private InputField mouseSensitivityInputField;

        public const float MIN_VOLUME = 0f;
        public const float MAX_VOLUME = 100f;

        public float minSensitivity = 0.01f;
        public float maxSensitivity = 10f;
        private float sensitivity;

        private bool isOpened = false;
        public GameObject diableUI;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            if (model == null)
            {
                model = ScriptableObject.CreateInstance<ConfigModel>();
            }

            windowToggle                = windowSlot.transform.GetChild(1).GetComponent<Toggle>();
            resolutionDropdown          = resolutionSlot.transform.GetChild(1).GetComponent<Dropdown>();
            masterVolumeSlider          = masterVolumeSlot.transform.GetChild(1).GetComponent<Slider>();
            bgmVolumeSlider             = bgmVolumeSlot.transform.GetChild(1).GetComponent<Slider>();
            sfxVolumeSlider             = sfxVolumeSlot.transform.GetChild(1).GetComponent<Slider>();
            mouseSensitivitySlider      = mouseSensitivitySlot.transform.GetChild(1).GetComponent<Slider>();
            mouseSensitivityInputField  = mouseSensitivitySlot.transform.GetChild(2).GetComponent<InputField>();

            windowToggle.isOn = model.isWindow;

            resolutionDropdown.interactable = model.isWindow;
            windowToggle.onValueChanged.AddListener(isOn => resolutionDropdown.interactable = isOn);
            windowToggle.onValueChanged.AddListener(isOn => ChangeWindowMode(isOn));

            foreach (ResolutionSO resolutionSO in model.resolutionDB.container)
            {
                resolutionDropdown.options.Add(new Dropdown.OptionData(resolutionSO.ToString()));
            }

            resolutionDropdown.value = model.resolutionIdx;
            resolutionDropdown.onValueChanged.AddListener(value => ChangeResolution(value));

            masterVolumeSlider.minValue = MIN_VOLUME;
            masterVolumeSlider.maxValue = MAX_VOLUME;
            masterVolumeSlider.value = model.masterVolume * 100f;

            bgmVolumeSlider.minValue = MIN_VOLUME;
            bgmVolumeSlider.maxValue = MAX_VOLUME;
            bgmVolumeSlider.value = model.bgmVolume * 100f;

            sfxVolumeSlider.minValue = MIN_VOLUME;
            sfxVolumeSlider.maxValue = MAX_VOLUME;
            sfxVolumeSlider.value = model.sfxVolume * 100f;

            masterVolumeSlider.onValueChanged.AddListener(value => model.SetMasterVolume(value / 100f));
            bgmVolumeSlider.onValueChanged.AddListener(value => model.SetBGMVolume(value / 100f));
            sfxVolumeSlider.onValueChanged.AddListener(value => model.SetSFXVolume(value / 100f));

            // mouse Sensitivity
            mouseSensitivitySlider.minValue = minSensitivity;
            mouseSensitivitySlider.maxValue = maxSensitivity;
            sensitivity = Mathf.Clamp(model.mouseSensitivity, minSensitivity, maxSensitivity);
            
            mouseSensitivitySlider.value = sensitivity;
            mouseSensitivityInputField.text = sensitivity.ToString("F2");

            mouseSensitivitySlider.onValueChanged.AddListener(value => ChangeMouseSensitivity(value));
            mouseSensitivitySlider.onValueChanged.AddListener(value => mouseSensitivityInputField.text = value.ToString("F2"));

            mouseSensitivityInputField.onValueChanged.AddListener(text => ChangeMouseSensitivity(text));
            mouseSensitivityInputField.onEndEdit.AddListener(text => ChangeMouseSensitivity(text));

            moveTitleBtn.onClick.AddListener(() => MoveTitleScene());
            backToGameBtn.onClick.AddListener(() => ClosePanel());
        }

        public void ChangeWindowMode(bool isWindow)
        {
            model.SetScreenMode(isWindow);
        }
        public void ChangeResolution(int index)
        {
            model.SetResolutionIdx(index);
        }
        
        public void ChangeMouseSensitivity(float sensitivity)
        {
            model.SetMouseSensitivity(sensitivity);
        }

        public void ChangeMouseSensitivity(string sensitivity)
        {
            string valid = Regex.Replace(sensitivity, @"[^0-9.]", "");
            int dotIndex = valid.IndexOf('.');

            if (dotIndex != -1)
            {
                int secondDot = valid.IndexOf('.', dotIndex + 1);
                if (secondDot != -1)
                    valid = valid.Remove(secondDot, 1);
            }

            float value;

            if (float.TryParse(valid, out value))
            {
                mouseSensitivitySlider.value = Mathf.Clamp(value, minSensitivity, maxSensitivity);

                model.SetMouseSensitivity(value);
            }
            else
            {
                Debug.Log("String To Float Parse Error!");
            }
            mouseSensitivityInputField.text = valid;
        }

        public void OpenPanel()
        {
            isOpened = true;
            gameObject.SetActive(isOpened);
            diableUI.SetActive(!isOpened);
            InputManager.Instance.ChangeDialogStrategy();
            EventBusSystem.Publish(EventBusType.STOP);
        }
        public void ClosePanel()
        {
            isOpened = false;
            gameObject.SetActive(isOpened);
            diableUI.SetActive(!isOpened);
            InputManager.Instance.ChangeNormalStrategy();
            EventBusSystem.Publish(EventBusType.START);
        }

        public void MoveTitleScene()
        {
            ClosePanel();
            LoadScene.Instance.LoadAsync((int)SceneList.TitleScene);
        }
    }
}