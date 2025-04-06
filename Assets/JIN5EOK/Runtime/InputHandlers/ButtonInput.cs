using UnityEngine;

namespace Jin5eok.Inputs
{
    public abstract class ButtonInputHandlerBase : IInputHandler<bool>
    {
        public abstract void UpdateState();

        public event InputCallback<bool> InputValueChanged;
        public bool Value { get; private set; }
        
        protected void UpdateState(bool currentValue)
        {
            if (currentValue != Value)
            {
                Value = currentValue;
                InputValueChanged?.Invoke(currentValue);
            }
        }
    }
    
    public class ButtonInputHandlerKeyCode : ButtonInputHandlerBase
    {
        public KeyCode KeyCode { get; set; }
        
        public ButtonInputHandlerKeyCode(KeyCode keyCode)
        {
            KeyCode = keyCode;
        }

        public override void UpdateState()
        {
            var currentInput = Input.GetKey(KeyCode) == true && Input.GetKeyUp(KeyCode) == false;
            UpdateState(currentInput);
        }
    }
    
    public class ButtonInputHandlerOldInputSystem : ButtonInputHandlerBase
    {
        public string ButtonName { get; set; }
        
        public ButtonInputHandlerOldInputSystem(string buttonName)
        {
            ButtonName = buttonName;
        }

        public override void UpdateState()
        {
            var currentInput = Input.GetButton(ButtonName) == true && Input.GetButtonUp(ButtonName) == false;
            UpdateState(currentInput);
        }
    }
}