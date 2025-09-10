using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    public class Nameplate : MonoBehaviour
    {
        public string characterName;
        public Color nameColor;

        private Text nameplateText;

        // Start is called before the first frame update
        void Start()
        {
            nameplateText = GetComponentInChildren<Text>();
            nameplateText.color = nameColor;
            nameplateText.text = characterName;
        }
    }
}