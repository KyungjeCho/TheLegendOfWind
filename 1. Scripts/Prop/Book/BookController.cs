using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class BookController : InteractComponent
    {
        public UnlockList unlockList;

        public override void Interact()
        {
            // ui unlock 
            Debug.Log("Unlock Book");
            DataManager.UnlockData.SetIsUnlocked(unlockList, true);
        }
    }

}
