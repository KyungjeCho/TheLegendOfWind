using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreNonPlayerController : InteractComponent
{
    public GameObject storeUI;

    private bool isOpened = false;

    public override void Interact()
    {
        if (isOpened)
        {
            CloseStore();
        }
        else
        {
            OpenStore();
        }
    }
    public void OpenStore()
    {
        if (isOpened) return;

        if (storeUI != null)
        {
            isOpened = true;
            storeUI.SetActive(isOpened);
            InputManager.Instance.ChangeDialogStrategy();
            EventBusSystem.Publish(EventBusType.STOP);
        }
    }

    public void CloseStore()
    {
        if (!isOpened) return;

        if (storeUI != null)
        {
            isOpened = false;
            storeUI.SetActive(isOpened);
            InputManager.Instance.ChangeNormalStrategy();
            EventBusSystem.Publish(EventBusType.START);
        }
    }
}
