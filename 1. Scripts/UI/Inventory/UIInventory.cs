using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KJ
{
    public abstract class UIInventory : MonoBehaviour
    {
        public InventorySO inventorySO;
        private InventorySO previousInventorySO;

        public Dictionary<GameObject, InventorySlot> slotUIs = new Dictionary<GameObject, InventorySlot>();


        private void Awake()
        {
            CreateSlots();

            for (int i = 0; i < inventorySO.Slots.Length; i++)
            {
                inventorySO.Slots[i].parent = inventorySO;
                inventorySO.Slots[i].OnPostUpdate += OnPostUpdate;
            }

        }
        private void Start()
        {
            for (int i = 0; i < inventorySO.Slots.Length; i++)
            {
                inventorySO.Slots[i].UpdateSlot(inventorySO.Slots[i].item, inventorySO.Slots[i].amount);
            }
        }

        public abstract void CreateSlots();

        public void OnPostUpdate(InventorySlot slot)
        {
            slot.slotUI.transform.GetChild(0).GetComponent<Image>().sprite = slot.item.id < 0 ? null : slot.ItemSO.icon;
            slot.slotUI.transform.GetChild(0).GetComponent<Image>().color = slot.item.id < 0 ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
            slot.slotUI.GetComponentInChildren<TextMeshProUGUI>().text = slot.item.id < 0 ? string.Empty : (slot.amount == 1 ? string.Empty : slot.amount.ToString("n0"));
        }

        protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = go.GetComponent<EventTrigger>();
            if (!trigger)
            {
                Debug.LogWarning("No EventTrigger component found!");
                return;
            }

            EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }
        public void OnEnterInterface(GameObject go)
        {
            MouseData.interfaceMouseIsOver = go.GetComponent<UIInventory>();
        }
        public void OnExitInterface(GameObject go)
        {
            MouseData.interfaceMouseIsOver = null;
        }
        public void OnEnter(GameObject go)
        {
            MouseData.slotHoveredOver = go;
            MouseData.interfaceMouseIsOver = go.GetComponentInParent<UIInventory>();
        }
        public void OnExit(GameObject go)
        {
            MouseData.slotHoveredOver = null;
        }
        public void OnStartDrag(GameObject go)
        {
            MouseData.tempItemBeingDragged = CreateDragImage(go);
        }
        private GameObject CreateDragImage(GameObject go)
        {
            if (slotUIs[go].item.id < 0)
            {
                return null;
            }
            GameObject dragImage = new GameObject();

            RectTransform rectTransform = dragImage.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(30, 30);
            dragImage.transform.SetParent(transform.parent);
            Image image = dragImage.AddComponent<Image>();
            image.sprite = slotUIs[go].ItemSO.icon;
            image.raycastTarget = false;

            dragImage.name = "DragImage";
            return dragImage;
        }
        public void OnDrag(GameObject go)
        {
            if (MouseData.tempItemBeingDragged == null)
            {
                return;
            }

            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
        public void OnEndDrag(GameObject go)
        {
            Destroy(MouseData.tempItemBeingDragged);

            if (MouseData.interfaceMouseIsOver == null)
            {
                slotUIs[go].RemoveItem();
            }
            else if (MouseData.slotHoveredOver)
            {
                InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotUIs[MouseData.slotHoveredOver];
                inventorySO.SwapItems(slotUIs[go], mouseHoverSlotData);
            }
        }
        public void OnClick(GameObject go, PointerEventData data)
        {
            InventorySlot slot = slotUIs[go];
            if (slot == null)
            {
                return;
            }
            if (data.button == PointerEventData.InputButton.Left)
            {
                OnLeftClick(slot);
            }
            else if (data.button == PointerEventData.InputButton.Right)
            {
                OnRightClick(slot);
            }
        }

        protected virtual void OnLeftClick(InventorySlot slot) { }
        protected virtual void OnRightClick(InventorySlot slot) { }
    }
}
