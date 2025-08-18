using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public class Button
    {
        private float buttonValue = 0f;
        public float ButtonValue
        {
            get { return buttonValue; }
            set { buttonValue = value; }
        }
    }
    public class PressedButton
    {
        private bool isPressedDown = false;
        private bool isPressed = false;
        private bool isPressedUp = false;

        public bool IsPressedDown { get { return isPressedDown; } set { isPressedDown = value; } }
        public bool IsPressed { get { return isPressed; } set { isPressed = value; } }
        public bool IsPressedUp { get {  return isPressedUp; } set { isPressedUp = value; } }
    }
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

    public interface InputStrategy
    {
        void OnInput();
    }

    public class NormalInput : InputStrategy
    {
        public void OnInput()
        {
            InputManager.Instance.HorizontalButton.ButtonValue = Input.GetAxis(ButtonName.Horizontal);
            InputManager.Instance.VerticalButton.ButtonValue = Input.GetAxis(ButtonName.Vertical);
            InputManager.Instance.SprintButton.IsButtonPressed = Input.GetButton(ButtonName.Sprint);
            InputManager.Instance.JumpButton.IsButtonPressed = Input.GetButtonDown(ButtonName.Jump);
            InputManager.Instance.CrouchButton.IsButtonPressed = Input.GetButton(ButtonName.Crouch);
            InputManager.Instance.AimButton.ButtonValue = Input.GetAxisRaw(ButtonName.Aim);
            InputManager.Instance.AttackButton.IsPressedDown = Input.GetButtonDown(ButtonName.Attack);
            InputManager.Instance.AttackButton.IsPressed = Input.GetButton(ButtonName.Attack);
            InputManager.Instance.AttackButton.IsPressedUp = Input.GetButtonUp(ButtonName.Attack);

            InputManager.Instance.MeleeWeaponButton.IsButtonPressed = Input.GetButtonDown(ButtonName.Weapon1);
            InputManager.Instance.RangeWeaponButton.IsButtonPressed = Input.GetButtonDown(ButtonName.Weapon2);
            InputManager.Instance.SkillQButton.IsButtonPressed = Input.GetButtonDown(ButtonName.Skill1);
            InputManager.Instance.SkillEButton.IsButtonPressed = Input.GetButtonDown(ButtonName.Skill2);
            InputManager.Instance.SkillRButton.IsButtonPressed = Input.GetButtonDown(ButtonName.Skill3);
            InputManager.Instance.InventoryButton.IsButtonPressed = Input.GetButtonDown(ButtonName.Inventory);
            InputManager.Instance.QuestButton.IsButtonPressed = Input.GetButtonDown(ButtonName.Quest);
            InputManager.Instance.InteractButton.IsButtonPressed = Input.GetButtonDown(ButtonName.Interact);
            InputManager.Instance.DenyButton.IsButtonPressed = Input.GetButtonDown(ButtonName.Deny);
        }
    }
    public class DialogInput : InputStrategy
    {
        public void OnInput()
        {
            InputManager.Instance.HorizontalButton.ButtonValue = 0f;
            InputManager.Instance.VerticalButton.ButtonValue = 0f;

            InputManager.Instance.InteractButton.IsButtonPressed = Input.GetButtonDown(ButtonName.Interact);
            InputManager.Instance.DenyButton.IsButtonPressed = Input.GetButtonDown(ButtonName.Deny);
        }
    }
    public class StopInput : InputStrategy
    {
        public void OnInput()
        {
            InputManager.Instance.InteractButton.IsButtonPressed = Input.GetButtonDown(ButtonName.Interact);
        }
    }
    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        public Button HorizontalButton;
        public Button VerticalButton;
        public Button AimButton;
        public ConsumeButton SkillQButton;
        public ConsumeButton SkillEButton;
        public ConsumeButton SkillRButton;
        public ConsumeButton MeleeWeaponButton;
        public ConsumeButton RangeWeaponButton;
        public ConsumeButton SprintButton;
        public ConsumeButton JumpButton;
        public ConsumeButton CrouchButton;
        public PressedButton AttackButton;
        public ConsumeButton QuestButton;
        public ConsumeButton DenyButton;
        public ConsumeButton InteractButton;
        public ConsumeButton InventoryButton;

        private InputStrategy inputStrategy;

        private void Start()
        {
            Init();
        }
        private void Update()
        {
            inputStrategy.OnInput();
        }

        private void Init()
        {
            HorizontalButton = new Button();
            VerticalButton = new Button();
            SkillQButton = new ConsumeButton();
            SkillEButton = new ConsumeButton();
            SkillRButton = new ConsumeButton();
            MeleeWeaponButton = new ConsumeButton();
            RangeWeaponButton = new ConsumeButton();
            SprintButton = new ConsumeButton();
            JumpButton = new ConsumeButton();
            CrouchButton = new ConsumeButton();
            AimButton = new Button();
            AttackButton = new PressedButton();
            QuestButton = new ConsumeButton();
            DenyButton = new ConsumeButton();
            InteractButton = new ConsumeButton();
            InventoryButton = new ConsumeButton();
            inputStrategy = new NormalInput();
            EventBusSystem.Subscribe(EventBusType.START, ChangeNormalStrategy);
            EventBusSystem.Subscribe(EventBusType.DIALOG, ChangeDialogStrategy);
        }
        public void ChangeStrategy(InputStrategy strategy)
        {
            inputStrategy = strategy;
        }

        private void ChangeNormalStrategy()
        {
            ChangeStrategy(new NormalInput());
        }
        private void ChangeDialogStrategy()
        {
            ChangeStrategy(new DialogInput());
        }
    }
}