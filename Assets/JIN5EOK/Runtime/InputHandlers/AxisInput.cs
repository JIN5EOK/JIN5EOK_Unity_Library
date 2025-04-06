using UnityEngine;

namespace Jin5eok.Inputs
{
    public abstract class AxisInputHandlerBase : IInputHandler<float>
    {
        public event InputCallback<float> InputValueChanged;
        public float Value { get; private set; }
        
        public abstract void UpdateState();
        
        protected void UpdateState(float currentValue)
        {
            if (Mathf.Approximately(Value, currentValue) == false)
            {
                Value = currentValue;
                InputValueChanged?.Invoke(currentValue);   
            }
        }
    }
    
    public class AxisInputHandlerKeyCode : AxisInputHandlerBase
    {
        public KeyCode PositiveKey { get; set; }
        public KeyCode NegativeKey { get; set; }
        
        public AxisInputHandlerKeyCode(KeyCode positiveKey, KeyCode negativeKey)
        {
            PositiveKey = positiveKey;
            NegativeKey = negativeKey;
        }

        public override void UpdateState()
        {
            var isNegative = Input.GetKey(NegativeKey) == true && Input.GetKeyUp(NegativeKey) == false && Input.GetKey(PositiveKey) == false;
            var isPositive = Input.GetKey(PositiveKey) == true && Input.GetKeyUp(PositiveKey) == false && Input.GetKey(NegativeKey) == false;
            var currentValue = isNegative ? -1 : isPositive ? 1 : 0;
            UpdateState(currentValue);
        }
    }
    
    public class AxisInputHandlerOldInputSystem : AxisInputHandlerBase
    {
        public string AxisName { get; set; }
        public bool IsUsingAxisRaw { get; set; }
        public AxisInputHandlerOldInputSystem(string axisName, bool isUsingAxisRaw = false)
        {
            AxisName = axisName;
            IsUsingAxisRaw = isUsingAxisRaw;
        }

        public override void UpdateState()
        {
            var currentValue = IsUsingAxisRaw == true ? Input.GetAxisRaw(AxisName) : Input.GetAxis(AxisName);
            UpdateState(currentValue);
        }
    }

}