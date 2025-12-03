using UnityEngine;

namespace Jin5eok
{
    /// <summary>
    /// bool 형식의 버튼 입력 값을 반환하는 InputHandler의 베이스 클래스입니다.
    /// </summary>
    public abstract class ButtonInputHandlerBase : InputHandler<bool>
    {

        public override bool Value { get; protected set; }
        
        protected void UpdateState(bool currentValue)
        {
            if (currentValue != Value)
            {
                Value = currentValue;
                InputDetected(currentValue);
            }
        }
    }
    
    /// <summary>
    /// KeyCode를 기반으로 Button 입력을 수행합니다.
    /// </summary>
    public class ButtonInputHandlerKeyCode : ButtonInputHandlerBase
    {
        /// <summary>
        /// 키 입력에 사용할 KeyCode
        /// </summary>
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
    
    /// <summary>
    /// OldInputSystem을 기반으로 Button 입력을 수행합니다.
    /// </summary>
    public class ButtonInputHandlerOldInputSystem : ButtonInputHandlerBase
    {
        /// <summary>
        /// 키 입력에 사용할 OldInputSystem 버튼 이름
        /// </summary>
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