using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace KJ
{
    public class UIStoreInventory : UIInventory
    {
        public InventorySO inventory;
        private ItemSO selectionItemSO;
        [SerializeField]
        protected GameObject slotPrefab;
        [SerializeField]
        private ItemSO[] items;
        [SerializeField]
        protected Vector2 start;

        [SerializeField]
        protected Vector2 size;

        [SerializeField]
        protected Vector2 space;

        [Min(1), SerializeField]
        protected int numberOfColumn = 4;


        [SerializeField]
        private GameObject buyItemSlotPrefab;
        private InventorySlot buyItemSlot;
        [SerializeField]
        private Text itemNameText;
        [SerializeField]
        private Text totalPriceText;
        [SerializeField]
        private Text priceText;
        [SerializeField]
        private InputField countInputField;

        [SerializeField]
        private Text goldText;

        public PlayerGold playerGold;

        public override void CreateSlots()
        {
            slotUIs = new Dictionary<GameObject, InventorySlot>();

            for (int i = 0; i < inventorySO.Slots.Length; i++)
            {
                GameObject go = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
                go.GetComponent<RectTransform>().anchoredPosition = CalculatePosition(i);
                AddEvent(go, EventTriggerType.PointerClick, (data) => { OnClick(go, (PointerEventData)data); });

                inventorySO.Slots[i].slotUI = go;
                slotUIs.Add(go, inventorySO.Slots[i]);
                go.name += ": " + i;
            }
            buyItemSlot = new InventorySlot();
            buyItemSlot.slotUI = buyItemSlotPrefab;
            buyItemSlot.parent = inventorySO;
            buyItemSlot.OnPostUpdate += OnPostUpdate;
            slotUIs.Add(buyItemSlotPrefab, buyItemSlot);
            if (inventorySO.Slots[0].ItemSO == null) 
            {
                foreach (ItemSO item in items)
                {
                    inventorySO.AddItem(item.CreateItem(), 1);
                }
            }

            UpdateSellPanel();
            playerGold.OnCurrentGoldChanged += UpdateGold;
            playerGold.PublishGold();
        }
        public Vector3 CalculatePosition(int i)
        {
            float x = start.x + ((space.x + size.x) * (i % numberOfColumn));
            float y = start.y + (-(space.y + size.y) * (i / numberOfColumn));

            return new Vector3(x, y, 0f);
        }
        protected override void OnLeftClick(InventorySlot slot)
        {
            if (slot != null)
            {
                selectionItemSO = slot.ItemSO;
                UpdateSellPanel();
            }
        }
        protected override void OnRightClick(InventorySlot slot)
        {
                
        }

        public void UpdateSellPanel()
        {
            if (buyItemSlotPrefab == null || itemNameText == null || priceText == null)
            {
                return;
            }

            if (selectionItemSO == null)
            {
                buyItemSlot.UpdateSlot(null, 0);
                itemNameText.text = string.Empty;
                priceText.text = "陛問: 0";
            }
            else
            {
                Debug.Log("Update Slot");
                buyItemSlot.UpdateSlot(selectionItemSO.CreateItem(), 1);
                itemNameText.text = selectionItemSO.itemName;
                priceText.text = "陛問: " + selectionItemSO.price;

            }
            UpdateSellPrice();
        }

        public void UpdateSellPrice()
        {
            string originalText = countInputField.text;
            string filtered = System.Text.RegularExpressions.Regex.Replace(originalText, @"\D", "");
            filtered = filtered.TrimStart('0');
            countInputField.text = filtered;

            if (selectionItemSO == null || countInputField.text == "")
            {
                countInputField.text = string.Empty;
                totalPriceText.text = "識 陛問: 0";
            }
            else
            {
                totalPriceText.text = "識 陛問: " + int.Parse(countInputField.text) * selectionItemSO.price;
            }
        }

        public void BuyItem()
        {
            if (selectionItemSO == null || countInputField.text == "")
            {
                return;
            }

            int totalPrice = int.Parse(countInputField.text) * selectionItemSO.price;
            if (totalPrice < 0 || totalPrice > playerGold.Gold)
            {
                return;
            }
            else
            {
                playerGold.RemoveGold(totalPrice);
                inventory.AddItem(selectionItemSO.CreateItem(), int.Parse(countInputField.text));
            }
        }

        public void UpdateGold(int gold)
        {
            goldText.text = gold.ToString();
        }
    }
}