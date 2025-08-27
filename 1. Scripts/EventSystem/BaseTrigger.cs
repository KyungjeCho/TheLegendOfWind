using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class BaseTrigger : MonoBehaviour
    {
        [SerializeField]
        private BaseEventSO eventSO;

        public virtual void OnTrigger()
        {
            eventSO.DoEvent(this);
        }
    }

}
