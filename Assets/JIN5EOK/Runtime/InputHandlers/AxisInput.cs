using UnityEngine;

namespace Jin5eok.Inputs
{
    public abstract class AxisInputHandlerBase : InputHandler<float>
    {
        public override event InputCallback<float> InputValueChanged;
        public override float Value { get; protected set; }
        
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
        public KeyCode PositiveKeyCode { get; set; }
        public KeyCode NegativeKeyCode { get; set; }
        
        public AxisInputHandlerKeyCode(KeyCode positiveKeyCode, KeyCode negativeKeyCode)
        {
            PositiveKeyCode = positiveKeyCode;
            NegativeKeyCode = negativeKeyCode;
            SetActiveAutoUpdate(true);
        }

        public override void UpdateState()
        {
            var isNegative = Input.GetKey(NegativeKeyCode) == true && Input.GetKeyUp(NegativeKeyCode) == false && Input.GetKey(PositiveKeyCode) == false;
            var isPositive = Input.GetKey(PositiveKeyCode) == true && Input.GetKeyUp(PositiveKeyCode) == false && Input.GetKey(NegativeKeyCode) == false;
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
            SetActiveAutoUpdate(true);
        }

        public override void UpdateState()
        {
            var currentValue = IsUsingAxisRaw == true ? Input.GetAxisRaw(AxisName) : Input.GetAxis(AxisName);
            UpdateState(currentValue);
        }
    }

}