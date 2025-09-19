using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KJ
{
    public class BoxController : InteractComponent
    {
        public SoundList openSound;
        public string objectName;

        private BaseTrigger trigger;
        private Animator myAnimator;
        private int openInt;
        [SerializeField]
        private bool isOpened;
        private string filePath = "Assets/9. Resources/Resources/Data";

        protected override void Start()
        {
            base.Start();
            trigger = GetComponent<BaseTrigger>();
            myAnimator = GetComponent<Animator>();

            openInt = Animator.StringToHash(AnimatorKey.Open);

            isOpened = false;

            interactStrategy = new TriggerInteract(trigger);

            LoadText();
            
            if (isOpened)
            {
                myAnimator.enabled = false;
                Transform boxTopTr = transform.GetChild(0);
                boxTopTr.Rotate(new Vector3(180f, 0f, 0f));
            }
        }
        public override void Interact()
        {
            if (isOpened) { return; }
            myAnimator.SetTrigger(openInt);
            SoundManager.Instance.PlayOneShotEffect(openSound, transform.position, 1.0f);
            isOpened = true;
            SaveText();
        }
        public void OpenBox()
        {
            trigger.OnTrigger();
        }

        public bool SaveText()
        {
            if (objectName == string.Empty || objectName == "")
            {
                return false;
            }
            string totalPath = Path.Combine(filePath, objectName + ".txt");
            string content = isOpened ? "1" : "0";
            File.WriteAllText(totalPath, content);
            return true;
        }
        public bool LoadText()
        {
            if (objectName == string.Empty || objectName == "")
            {
                return false;
            }
            string totalPath = Path.Combine(filePath, objectName + ".txt");
            try
            {
                string content = File.ReadAllText(totalPath);
                isOpened = content == "1" ? true : false;
            }
            catch (Exception e1)
            {
                Debug.Log("Load File Error: " + e1.Message);
                return false;
            }
            return true;
        }
    }
}