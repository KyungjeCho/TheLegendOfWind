using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryLoader : MonoBehaviour
{
    public InventorySO inventory;
    public InventorySO equipment;

    // Start is called before the first frame update
    void Start()
    {
        inventory.LoadData();
        equipment.LoadData();
    }
}
