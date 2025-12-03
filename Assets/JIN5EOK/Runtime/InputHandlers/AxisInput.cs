using UnityEngine;

namespace Jin5eok
{
    /// <summary>
    /// float 형식의 입력 값을 반환하는 InputHandler의 베이스 클래스입니다.
    /// </summary>
    public abstract class AxisInputHandlerBase : InputHandler<float>
    {
        public override float Value { get; protected set; }
        
        protected void UpdateState(float currentValue)
        {
            if (Mathf.Approximately(Value, currentValue) == false)
            {
                Value = currentValue;
                InputDetected(currentValue);   
            }
        }
    }
    
    /// <summary>
    /// KeyCode를 기반으로 AxisRaw 키 입력을 수행합니다.
    /// </summary>
    public class AxisInputHandlerKeyCode : AxisInputHandlerBase
    {
        /// <summary>
        /// 키 입력에 사용할 양수에 해당하는 KeyCode
        /// </summary>
        public KeyCode PositiveKeyCode { get; set; }
        /// <summary>
        /// 키 입력에 사용할 음수에 해당하는 KeyCode
        /// </summary>
        public KeyCode NegativeKeyCode { get; set; }
        
        public AxisInputHandlerKeyCode(KeyCode positiveKeyCode, KeyCode negativeKeyCode)
        {
            PositiveKeyCode = positiveKeyCode;
            NegativeKeyCode = negativeKeyCode;
        }

        public override void UpdateState()
        {
            var isNegative = Input.GetKey(NegativeKeyCode) == true && Input.GetKeyUp(NegativeKeyCode) == false && Input.GetKey(PositiveKeyCode) == false;
            var isPositive = Input.GetKey(PositiveKeyCode) == true && Input.GetKeyUp(PositiveKeyCode) == false && Input.GetKey(NegativeKeyCode) == false;
            var currentValue = isNegative ? -1 : isPositive ? 1 : 0;
            UpdateState(currentValue);
        }
    }
    
    /// <summary>
    /// OldInputSystem의 Axis 입력을 기반으로 키 입력을 수행합니다.
    /// </summary>
    public class AxisInputHandlerOldInputSystem : AxisInputHandlerBase
    {
        /// <summary>
        /// 키 입력에 사용할 OldInputSystem의 Axis 이름
        /// </summary>
        public string AxisName { get; set; }
        /// <summary>
        /// Input을 AxisRaw 형태로 받을지 Axis형태로 받을지 여부
        /// </summary>
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