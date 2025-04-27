using Jin5eok.Extension;
using UnityEngine;

namespace Jin5eok.Inputs
{
    public abstract class Vector2InputHandlerBase : InputHandler<Vector2>
    {
        public override event InputCallback<Vector2> InputValueChanged;
        public override Vector2 Value { get; protected set; }

        protected void UpdateState(Vector2 currentValue)
        {
            if (Value.ApproximatelyEquals(currentValue) == false)
            {
                Value = currentValue;
                InputValueChanged?.Invoke(currentValue);
            }
        }
    }
    
    public class Vector2InputHandlerKeyCode : Vector2InputHandlerBase
    {
        private readonly AxisInputHandlerKeyCode _axisInputX;
        private readonly AxisInputHandlerKeyCode _axisInputY;
        
        public KeyCode UpKeyCode
        {
            get => _axisInputY.PositiveKeyCode;
            set =>_axisInputY.PositiveKeyCode = value;
        }

        public KeyCode DownKeyCode
        {
            get => _axisInputY.NegativeKeyCode;
            set =>_axisInputY.NegativeKeyCode = value;
        }
        public KeyCode LeftKeyCode
        {
            get => _axisInputX.NegativeKeyCode;
            set =>_axisInputX.NegativeKeyCode = value;
        }
        public KeyCode RightKeyCode
        {
            get => _axisInputX.PositiveKeyCode;
            set =>_axisInputX.PositiveKeyCode = value;
        }
        
        public Vector2InputHandlerKeyCode(KeyCode up, KeyCode down, KeyCode left, KeyCode right)
        {
            _axisInputX = new AxisInputHandlerKeyCode(right, left);
            _axisInputY = new AxisInputHandlerKeyCode(up, down);
        }
        
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
  
    public class Vector2InputHandlerOldInputSystem : Vector2InputHandlerBase
    {
        private readonly AxisInputHandlerOldInputSystem _axisInputX;
        private readonly AxisInputHandlerOldInputSystem _axisInputY;
        
        public string AxisNameX
        {
            get => _axisInputX.AxisName;
            set => _axisInputX.AxisName = value;
        }
        public string AxisNameY
        {
            get => _axisInputY.AxisName;
            set => _axisInputY.AxisName = value;
        }
        public bool IsUsingAxisRaw
        {
            get => _axisInputX.IsUsingAxisRaw;
            set
            {
                _axisInputX.IsUsingAxisRaw = value;
                _axisInputY.IsUsingAxisRaw = value;
            } 
        }
        
        public Vector2InputHandlerOldInputSystem(string axisNameX, string axisNameY, bool isUsingAxisRaw = false)
        {
            _axisInputX = new AxisInputHandlerOldInputSystem(axisNameX, isUsingAxisRaw);
            _axisInputY = new AxisInputHandlerOldInputSystem(axisNameY, isUsingAxisRaw);
        }
        
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