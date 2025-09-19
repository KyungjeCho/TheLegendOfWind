using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KJ
{
    public class UIStaticInventory : UIInventory
    {
        public GameObject[] staticSlots = null;


        public override void CreateSlots()
        {
            slotUIs = new Dictionary<GameObject, InventorySlot>();
            for (int i = 0; i < inventorySO.Slots.Length; i++)
            {
                GameObject slotGO = staticSlots[i];

                AddEvent(slotGO, EventTriggerType.PointerEnter, delegate { OnEnter(slotGO); });
                AddEvent(slotGO, EventTriggerType.PointerExit, delegate { OnExit(slotGO); });
                AddEvent(slotGO, EventTriggerType.BeginDrag, delegate { OnStartDrag(slotGO); });
                AddEvent(slotGO, EventTriggerType.EndDrag, delegate { OnEndDrag(slotGO); });
                AddEvent(slotGO, EventTriggerType.Drag, delegate { OnDrag(slotGO); });

                inventorySO.Slots[i].slotUI = slotGO;
                slotUIs.Add(slotGO, inventorySO.Slots[i]);
            }
        }
    }

}
