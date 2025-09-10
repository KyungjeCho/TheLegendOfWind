using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "Quest Event", menuName = "ScriptableObjects/Events/Quest Event")]
    public class QuestEventSO : BaseEventSO
    {
        public List<BaseEventSO> events = new List<BaseEventSO>();

        public override bool DoEvent(BaseTrigger trigger)
        {
            foreach(BaseEventSO e in events)
            {
                e.DoEvent(trigger);
            }
            return true;
        }
    }

}

