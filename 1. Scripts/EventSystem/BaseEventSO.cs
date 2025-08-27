using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "Base Event SO", menuName = "ScriptableObjects/Base Event SO")]
    public abstract class BaseEventSO : ScriptableObject
    {
        public string eventName = string.Empty;

        public abstract bool DoEvent(BaseTrigger trigger);
    }

}
