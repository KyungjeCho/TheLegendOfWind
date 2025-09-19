using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class ControlDescriptionPanel : SingletonMonoBehaviour<ControlDescriptionPanel>
    {
        public UIDescriptionPanel descriptionPanel;
        private bool isOpened = false;
        private ItemSO itemSO = null;

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
        }
        public void ClosePanel()
        {
            if (!isOpened || descriptionPanel == null) { return; }

            isOpened = false;
            descriptionPanel.gameObject.SetActive(isOpened);
            if (itemSO == null) { return; }
            DropQueue.Instance.ItemDrop(itemSO.itemName);
            itemSO = null;
        }
    }
}