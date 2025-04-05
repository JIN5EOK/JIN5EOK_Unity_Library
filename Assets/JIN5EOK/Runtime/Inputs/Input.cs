using UnityEngine;

namespace Jin5eok.Inputs
{
    public delegate void InputCallback<T>(T input) where T : notnull;
    public interface IInput<T> where T : notnull
    {
        public event InputCallback<T> InputValueChanged;
        public T InputState { get; }
    }

    public abstract class InputBase
    {
        public abstract void UpdateState();
    }
    
    public class ButtonInput : InputBase, IInput<bool>
    {
        public KeyCode KeyCode { get; set; }
        public event InputCallback<bool> InputValueChanged;
        public bool InputState { get; private set; }

        public ButtonInput(KeyCode keyCode)
        {
            KeyCode = keyCode;
        }
        
        public override void UpdateState()
        {
            bool currentInput = Input.GetKeyDown(KeyCode) == true || Input.GetKey(KeyCode);
            
            if (InputState != currentInput)
            {
                InputState = currentInput;
                InputValueChanged?.Invoke(currentInput);    
            }
        }
    }
    
    public class AxisInput : InputBase, IInput<float>
    {
        public string AxisName { get; set; }
        public event InputCallback<float> InputValueChanged;
        public float InputState { get; private set; }

        public AxisInput(string axisName)
        {
            AxisName = axisName;
        }

        public override void UpdateState()
        {
            var currentInput = Input.GetAxisRaw(AxisName);
            if (Mathf.Approximately(InputState, currentInput) == false)
            {
                InputState = currentInput;
                InputValueChanged?.Invoke(currentInput);    
            }
        }
    }
}