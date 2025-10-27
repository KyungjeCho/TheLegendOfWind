using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    public class UIDescriptionPanel : MonoBehaviour
    {
        public Image icon;
        public Text itemName;
        public Text itemDescription;

        public void UpdatePanel(ItemSO itemSO)
        {
            if (icon == null || itemName == null || itemDescription == null)
            {
                return;
            }    
            icon.sprite = itemSO.icon;
            itemName.text = itemSO.itemName;
            itemDescription.text = itemSO.description;
        }
    }

}
