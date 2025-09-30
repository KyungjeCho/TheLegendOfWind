using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class ClearDungeon : MonoBehaviour
    {
        public void Clear()
        {
            GameEvent.PublishUnlocked(UnlockList.ClearDungeon);
            DataManager.UnlockData.data[(int)UnlockList.ClearDungeon].isUnlocked = true;
        }
    }

}
