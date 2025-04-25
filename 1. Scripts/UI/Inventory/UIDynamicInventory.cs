using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KJ
{
    public class UIDynamicInventory : UIInventory
    {
        [SerializeField]
        protected GameObject slotPrefab;

        [SerializeField]
        protected Vector2 start;

        [SerializeField]
        protected Vector2 size;

        [SerializeField]
        protected Vector2 space;

        [Min(1), SerializeField]
        protected int numberOfColumn = 4;

        public override void CreateSlots()
        {
            slotUIs = new Dictionary<GameObject, InventorySlot>();

            for (int i = 0;  i < inventorySO.Slots.Length; i++)
            {
                GameObject go = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
                go.GetComponent<RectTransform>().anchoredPosition = CalculatePosition(i);

                AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnter(go); });
                AddEvent(go, EventTriggerType.PointerExit, delegate { OnExit(go); });
                AddEvent(go, EventTriggerType.BeginDrag, delegate { OnStartDrag(go); });
                AddEvent(go, EventTriggerType.EndDrag, delegate { OnEndDrag(go); });
                AddEvent(go, EventTriggerType.Drag, delegate { OnDrag(go); });
                AddEvent(go, EventTriggerType.PointerClick, (data) => { OnClick(go, (PointerEventData)data); });

                inventorySO.Slots[i].slotUI = go;
                slotUIs.Add(go, inventorySO.Slots[i]);
                go.name += ": " + i;
            }
        }
        public Vector3 CalculatePosition(int i)
        {
            float x = start.x + ((space.x + size.x) * (i % numberOfColumn));
            float y = start.y + (-(space.y + size.y) * (i / numberOfColumn));

            return new Vector3(x, y, 0f);
        }
        protected override void OnRightClick(InventorySlot slot)
        {
            inventorySO.UseItem(slot);
        }
    }

}
