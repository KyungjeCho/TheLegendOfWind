using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class ControlDescriptionPanel : SingletonMonoBehaviour<ControlDescriptionPanel>
    {
        public UIDescriptionPanel descriptionPanel;
        public SoundList openSound;
        private bool isOpened = false;
        private ItemSO itemSO = null;

        public Action<ItemSO> OnPanelClose;

        void Update()
        {
            if (InputManager.Instance.InteractButton.IsButtonPressed)
            {
                ClosePanel();
            }
        }
        public void OpenPanel(ItemSO itemSO)
        { 
            if (isOpened || descriptionPanel == null || itemSO == null) { return; }
            isOpened = true;
            this.itemSO = itemSO;
            descriptionPanel.gameObject.SetActive(isOpened);
            descriptionPanel.UpdatePanel(itemSO);
            InputManager.Instance.InteractButton.ConsumeButtonPressed();
            InputManager.Instance.ChangeDialogStrategy();
            SoundManager.Instance.PlayOneShotEffect(openSound, Camera.main.transform.position + Camera.main.transform.forward, 0.7f);
        }
        public void ClosePanel()
        {
            if (!isOpened || descriptionPanel == null) { return; }

            isOpened = false;
            descriptionPanel.gameObject.SetActive(isOpened);
            if (itemSO == null) { return; }
            DropQueue.Instance.ItemDrop(itemSO.itemName);
            OnPanelClose?.Invoke(itemSO);
            itemSO = null;
            InputManager.Instance.ChangeNormalStrategy();
        }
    }
}