using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private void Start()
    {
        EventBusSystem.Subscribe(EventBusType.START, () => Cursor.visible = false);
        EventBusSystem.Subscribe(EventBusType.START, () => Time.timeScale = 1f);
        EventBusSystem.Subscribe(EventBusType.STOP, () => Cursor.visible = true);
        EventBusSystem.Subscribe(EventBusType.STOP, () => Time.timeScale = 0f);
        
        EventBusSystem.Publish(EventBusType.START);
    }
}
