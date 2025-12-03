using UnityEngine;

namespace Jin5eok
{
    /// <summary>
    /// Vector2 형식의 입력 값을 반환하는 InputHandler의 베이스 클래스입니다.
    /// </summary>
    public abstract class Vector2InputHandlerBase : InputHandler<Vector2>
    {
        /// <summary>
        /// 현재 Vector2 입력 값을 반환합니다. X와 Y 축의 입력을 조합한 값입니다.
        /// </summary>
        public override Vector2 Value { get; protected set; }

        protected void UpdateState(Vector2 currentValue)
        {
            if (Value.ApproximatelyEquals(currentValue) == false)
            {
                Value = currentValue;
                InputDetected(currentValue);
            }
        }
    }
    
    /// <summary>
    /// KeyCode를 기반으로 Vector2 입력을 수행합니다.
    /// </summary>
    public class Vector2InputHandlerKeyCode : Vector2InputHandlerBase
    {
        private readonly AxisInputHandlerKeyCode _axisInputX;
        private readonly AxisInputHandlerKeyCode _axisInputY;
        
        /// <summary>
        /// 상 입력에 사용할 KeyCode
        /// </summary>
        public KeyCode UpKeyCode
        {
            get => _axisInputY.PositiveKeyCode;
            set =>_axisInputY.PositiveKeyCode = value;
        }

        /// <summary>
        /// 하 입력에 사용할 KeyCode
        /// </summary>
        public KeyCode DownKeyCode
        {
            get => _axisInputY.NegativeKeyCode;
            set =>_axisInputY.NegativeKeyCode = value;
        }
        /// <summary>
        /// 좌 입력에 사용할 KeyCode
        /// </summary>
        public KeyCode LeftKeyCode
        {
            get => _axisInputX.NegativeKeyCode;
            set =>_axisInputX.NegativeKeyCode = value;
        }
        /// <summary>
        /// 우 입력에 사용할 KeyCode
        /// </summary>
        public KeyCode RightKeyCode
        {
            get => _axisInputX.PositiveKeyCode;
            set =>_axisInputX.PositiveKeyCode = value;
        }
        
        /// <summary>
        /// Vector2InputHandlerKeyCode를 생성합니다.
        /// </summary>
        /// <param name="up">상 입력에 사용할 KeyCode</param>
        /// <param name="down">하 입력에 사용할 KeyCode</param>
        /// <param name="left">좌 입력에 사용할 KeyCode</param>
        /// <param name="right">우 입력에 사용할 KeyCode</param>
        public Vector2InputHandlerKeyCode(KeyCode up, KeyCode down, KeyCode left, KeyCode right)
        {
            _axisInputX = new AxisInputHandlerKeyCode(right, left);
            _axisInputY = new AxisInputHandlerKeyCode(up, down);
        }
        
        /// <summary>
        /// KeyCode를 기반으로 Vector2 입력 값을 업데이트합니다.
        /// </summary>
        public override void UpdateState()
        {
            _axisInputX.UpdateState();
            _axisInputY.UpdateState();
            
            float horizontal = _axisInputX.Value;
            float vertical = _axisInputY.Value;
            
            var currentValue = new Vector2(horizontal, vertical);
            UpdateState(currentValue);
        }
    }
  
    /// <summary>
    /// OldInputSystem을 기반으로 Vector2 입력을 수행합니다.
    /// </summary>
    public class Vector2InputHandlerOldInputSystem : Vector2InputHandlerBase
    {
        private readonly AxisInputHandlerOldInputSystem _axisInputX;
        private readonly AxisInputHandlerOldInputSystem _axisInputY;
        
        /// <summary>
        /// X 좌표 입력에 사용할 OldInputSystem Axis 이름
        /// </summary>
        public string AxisNameX
        {
            get => _axisInputX.AxisName;
            set => _axisInputX.AxisName = value;
        }
        /// <summary>
        /// Y 좌표 입력에 사용할 OldInputSystem Axis 이름
        /// </summary>
        public string AxisNameY
        {
            get => _axisInputY.AxisName;
            set => _axisInputY.AxisName = value;
        }
        /// <summary>
        /// AxisRaw를 사용할지 Axis를 사용할지 여부
        /// </summary>
        public bool IsUsingAxisRaw
        {
            get => _axisInputX.IsUsingAxisRaw;
            set
            {
                _axisInputX.IsUsingAxisRaw = value;
                _axisInputY.IsUsingAxisRaw = value;
            } 
        }
        
        /// <summary>
        /// Vector2InputHandlerOldInputSystem을 생성합니다.
        /// </summary>
        /// <param name="axisNameX">X 좌표 입력에 사용할 OldInputSystem Axis 이름</param>
        /// <param name="axisNameY">Y 좌표 입력에 사용할 OldInputSystem Axis 이름</param>
        /// <param name="isUsingAxisRaw">AxisRaw를 사용할지 여부</param>
        public Vector2InputHandlerOldInputSystem(string axisNameX, string axisNameY, bool isUsingAxisRaw = false)
        {
            _axisInputX = new AxisInputHandlerOldInputSystem(axisNameX, isUsingAxisRaw);
            _axisInputY = new AxisInputHandlerOldInputSystem(axisNameY, isUsingAxisRaw);
        }
        
        /// <summary>
        /// OldInputSystem을 기반으로 Vector2 입력 값을 업데이트합니다.
        /// </summary>
        public override void UpdateState()
        {
            _axisInputX.UpdateState();
            _axisInputY.UpdateState();
            
            var horizontal = _axisInputX.Value;
            var vertical = _axisInputY.Value;
            
            var currentValue = new Vector2(horizontal, vertical);
            UpdateState(currentValue);
        }
    }
}