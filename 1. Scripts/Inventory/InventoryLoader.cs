using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InventoryLoader : MonoBehaviour
{
    public InventorySO inventory;
    public InventorySO equipment;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        inventory.LoadData();
        equipment.LoadData();
    }
}
