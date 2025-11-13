using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    public class UIItemInfo : MonoBehaviour
    {
        public Text itemNameText;
        public Text itemDescriptionText;

        // Start is called before the first frame update
        void Start()
        {
            if (itemNameText == null)
            {
                itemNameText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
            }
            if (itemDescriptionText == null)
            {
                itemDescriptionText = transform.GetChild(0).GetChild(1).GetComponent<Text>();
            }
        }

        public void SetPanelContext(ItemSO itemSO)
        {
            if (itemSO ==  null)
            {
                return;
            }
            itemNameText.text = itemSO.itemName;
            string context = "";
            foreach (ItemBuff buff in itemSO.data.buffs)
            {
                switch (buff.stat)
                {
                    case PlayerAttribute.HP:
                        context = "체력 ";
                        break;
                    case PlayerAttribute.ATTACK:
                        context = "공격력 ";
                        break;
                    case PlayerAttribute.DEFENSE:
                        context = "방어력 ";
                        break;
                    case PlayerAttribute.STEMINA:
                        context = "스테미나 ";
                        break;
                    case PlayerAttribute.MANA:
                        context = "마나 ";
                        break;
                    case PlayerAttribute.SPEED:
                        context = "스피드 ";
                        break;
                }
                context += $"+{buff.value}\n";
            }
            context += itemSO.description;
            itemDescriptionText.text = context;
        }

        public void OpenPanel()
        {
            gameObject.SetActive(true);
        }
        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }
    }

}
