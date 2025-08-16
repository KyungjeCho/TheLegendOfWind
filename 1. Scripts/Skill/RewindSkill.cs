using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "Rewind Skill", menuName = "ScriptableObjects/Rewind Skill")]
    public class RewindSkill : BaseSkill
    {
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
            if (selectObjectBehaviour.GetIsSelecting() == true)
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

            Rewinder rewinder = targetTransform.GetComponent<Rewinder>();

            if (rewinder != null)
            {
                rewinder.StartRewind();
            }
        }
    }
}
