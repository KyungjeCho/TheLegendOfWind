using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "Time Stop Skill", menuName = "ScriptableObjects/Time Stop Skill")]
    public class TimeStopSkill : BaseSkill
    {
        public SoundList failueSound;
        public SoundList successSound;
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
            SoundManager.Instance.PlayOneShotEffect(useSound, playerTranform.position, 1f);
            selectObjectBehaviour.SetIsSelecting(true);
            selectObjectBehaviour.OnTargetObjectSelected += SelectTargetObject;
            StartSkill();
        }

        public void SelectTargetObject(Transform targetTransform)
        {
            selectObjectBehaviour.OnTargetObjectSelected -= SelectTargetObject;

            if (targetTransform == null)
            {
                SoundManager.Instance.PlayOneShotEffect(failueSound, playerTranform.position, 1f);
                return;
            }

            IStopable stopable = targetTransform.GetComponent<IStopable>();

            if (stopable != null)
            {
                stopable.StopObject();
                ObjectSelector os = targetTransform.GetComponent<ObjectSelector>();
                if (os != null)
                {
                    os.TriggerRimLight(5f);
                }
                SoundManager.Instance.PlayOneShotEffect(successSound, playerTranform.position, 1f);
            }
            else
            {
                SoundManager.Instance.PlayOneShotEffect(failueSound, playerTranform.position, 1f);
            }
        }
    }
}