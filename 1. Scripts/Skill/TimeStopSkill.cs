using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "Time Stop Skill", menuName = "ScriptableObjects/Time Stop Skill")]
    public class TimeStopSkill : BaseSkill
    {
        private Transform targetTransform = null;

        private SelectObjectBehaviour selectObjectBehaviour;

        public override void SetPlayerTransform(Transform transform)
        {
            base.SetPlayerTransform(transform);

            selectObjectBehaviour = transform.GetComponent<SelectObjectBehaviour>();

        }
        public override void UseSkill()
        {
            if (Timer > 0)
            {
                return;
            }

            selectObjectBehaviour.SetIsSelecting(true);
            selectObjectBehaviour.OnTargetObjectSelected += SelectTargetObject;
            StartSkill();
        }

        public void SelectTargetObject(Transform targetTransform)
        {
            if (targetTransform == null)
                return;

            Debug.Log(targetTransform);
            IStopable stopable = targetTransform.GetComponent<IStopable>();

            if (stopable != null)
            {
                stopable.StopObject();
            }
            selectObjectBehaviour.OnTargetObjectSelected -= SelectTargetObject;
        }
        public void StopObject()
        {

        }
    }
}