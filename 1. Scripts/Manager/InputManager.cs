using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class ConsumeButton
    {
        private bool isButtonPressed = false;
        public bool IsButtonPressed 
        { 
            get { return isButtonPressed;} 
            set { isButtonPressed = value;} 
        }

        public bool ConsumeButtonPressed()
        {
            if (isButtonPressed == true)
            {
                isButtonPressed = false;
                return true;
            }
            return false;
        }
    }
    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        public ConsumeButton InteractButton;

        private void Start()
        {
            InteractButton = new ConsumeButton();
        }
        private void Update()
        {
            InteractButton.IsButtonPressed = Input.GetButtonDown(ButtonName.Interact);
        }
    }
}