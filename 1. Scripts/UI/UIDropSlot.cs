using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    public class UIDropSlot : MonoBehaviour
    {
        [SerializeField]
        private Text dropText;

        private void Update()
        {
            
        }

        public void SetText(string text)
        {
            dropText.text = text;
        }
    }
}