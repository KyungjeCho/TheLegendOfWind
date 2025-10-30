using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class TutorialController : MonoBehaviour
    {
        public TutorialList tutorialList;

        public UITutorialPanel tutorialPanel;

        public void OpenPanel()
        {
            tutorialPanel.OpenPanel(tutorialList);
        }
    }

}
