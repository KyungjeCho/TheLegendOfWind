using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "Get Item EventSO", menuName = "ScriptableObjects/Get Item EventSO")]
    public class GetItemEventSO : BaseEventSO
    {
        public ItemSO itemSO;
        public InventorySO inventory;

        public override bool DoEvent(BaseTrigger trigger)
        {
            if (inventory == null || itemSO == null)
                return false;
            inventory.AddItem(itemSO.CreateItem(), 1);
            ControlDescriptionPanel.Instance.OpenPanel(itemSO);

            return true;
        }
    }
}
