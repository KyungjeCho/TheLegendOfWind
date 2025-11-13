using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KJ
{
    public class TitleBtn : MonoBehaviour
    {
        private Color origColor;
        public Color enterColor;

        private UnityEngine.UI.Button btn;
        private Text text;
        // Start is called before the first frame update
        void Start()
        {
            btn = GetComponent<UnityEngine.UI.Button>();

            text = btn.transform.GetChild(0).GetComponent<Text>();
            origColor = text.color;

            AddEvent(EventTriggerType.PointerEnter, delegate { OnPointerEnter(); });
            AddEvent(EventTriggerType.PointerExit, delegate { OnPointerExit(); });
        }

        protected void AddEvent(EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = GetComponent<EventTrigger>();
            if (!trigger)
            {
                trigger = gameObject.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry eventTriger = new EventTrigger.Entry { eventID = type };
            eventTriger.callback.AddListener(action);
            trigger.triggers.Add(eventTriger);
        }

        private void OnPointerEnter()
        {
            text.color = enterColor;
        }
        private void OnPointerExit()
        {
            text.color = origColor;
        }
    }

}
