using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public abstract class EventObject : ScriptableObject
    {
        public abstract void OnEventEnter();
        public abstract void OnEventExit();
    }
}
