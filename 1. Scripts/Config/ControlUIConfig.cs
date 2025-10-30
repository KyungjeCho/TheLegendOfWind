using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlUIConfig : MonoBehaviour
{
    private bool isOpened = false;

    public UIConfig uiConfig;

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.EscapeButton.IsButtonPressed)
        {
            if (isOpened)
            {
                uiConfig.ClosePanel();
                
            }
            else
            {
                uiConfig.OpenPanel();
                
            }
            isOpened = !isOpened;
        }
    }
}
