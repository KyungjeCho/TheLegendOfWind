using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class ControlQuestList : MonoBehaviour
    {
        [SerializeField]
        private GameObject questPanel;

        private bool isOpened = false;
        // Start is called before the first frame update
        void Start()
        {
            questPanel.SetActive(isOpened);
        }

        // Update is called once per frame
        void Update()
        {
            if (InputManager.Instance.QuestButton.IsButtonPressed)
            {
                isOpened = !isOpened;
                questPanel.SetActive(isOpened);

                if (isOpened)
                {
                    EventBusSystem.Publish(EventBusType.STOP);
                }
                else
                {
                    EventBusSystem.Publish(EventBusType.START);
                }
            }
        }
    }
}



