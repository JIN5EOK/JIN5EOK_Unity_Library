using UnityEngine;

namespace Jin5eok
{
    /// <summary>
    /// float 형식의 입력 값을 반환하는 InputHandler의 베이스 클래스입니다.
    /// </summary>
    public abstract class AxisInputHandlerBase : InputHandler<float>
    {
        /// <summary>
        /// 현재 축 입력 값을 반환합니다. 양수 방향이면 양수, 음수 방향이면 음수, 입력이 없으면 0입니다.
        /// </summary>
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
        
        /// <summary>
        /// AxisInputHandlerKeyCode를 생성합니다.
        /// </summary>
        /// <param name="positiveKeyCode">양수 방향에 사용할 KeyCode</param>
        /// <param name="negativeKeyCode">음수 방향에 사용할 KeyCode</param>
        public AxisInputHandlerKeyCode(KeyCode positiveKeyCode, KeyCode negativeKeyCode)
        {
            PositiveKeyCode = positiveKeyCode;
            NegativeKeyCode = negativeKeyCode;
        }

        /// <summary>
        /// KeyCode를 기반으로 축 입력 값을 업데이트합니다.
        /// </summary>
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
        
        /// <summary>
        /// AxisInputHandlerOldInputSystem을 생성합니다.
        /// </summary>
        /// <param name="axisName">키 입력에 사용할 OldInputSystem의 Axis 이름</param>
        /// <param name="isUsingAxisRaw">AxisRaw를 사용할지 여부</param>
        public AxisInputHandlerOldInputSystem(string axisName, bool isUsingAxisRaw = false)
        {
            AxisName = axisName;
            IsUsingAxisRaw = isUsingAxisRaw;
        }

        /// <summary>
        /// OldInputSystem을 기반으로 축 입력 값을 업데이트합니다.
        /// </summary>
        public override void UpdateState()
        {
            var currentValue = IsUsingAxisRaw == true ? Input.GetAxisRaw(AxisName) : Input.GetAxis(AxisName);
            UpdateState(currentValue);
        }
    }

}