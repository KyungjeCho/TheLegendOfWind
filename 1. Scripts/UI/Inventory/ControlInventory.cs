using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlInventory : MonoBehaviour
{
    [SerializeField]
    private GameObject InventoryPanel;

    private bool isOpened = false;
    // Start is called before the first frame update
    void Start()
    {
        isOpened = false;
        InventoryPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(ButtonName.Inventory))
        {
            isOpened = !isOpened;
            InventoryPanel.SetActive(isOpened);

            if (isOpened)
            {
                EventBusSystem.Publish(EventBusType.STOP);
                InputManager.Instance.ChangeDialogStrategy();
            }
            else
            {
                EventBusSystem.Publish(EventBusType.START);
                InputManager.Instance.ChangeNormalStrategy();
            }
        }
    }
}
