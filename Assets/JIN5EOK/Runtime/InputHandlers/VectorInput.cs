using Jin5eok.Extension;
using UnityEngine;

namespace Jin5eok.Inputs
{
    public abstract class VectorInputHandlerBase : InputHandler<Vector2>
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
    
    public class VectorInputHandlerKeyCode : VectorInputHandlerBase
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
        
        public VectorInputHandlerKeyCode(KeyCode up, KeyCode down, KeyCode left, KeyCode right)
        {
            _axisInputX = new AxisInputHandlerKeyCode(right, left);
            _axisInputX.SetActiveAutoUpdate(false);
            _axisInputY = new AxisInputHandlerKeyCode(up, down);
            _axisInputY.SetActiveAutoUpdate(false);
            SetActiveAutoUpdate(true);
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
  
    public class VectorInputHandlerOldInputSystem : VectorInputHandlerBase
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
        
        public VectorInputHandlerOldInputSystem(string axisNameX, string axisNameY, bool isUsingAxisRaw = false)
        {
            _axisInputX = new AxisInputHandlerOldInputSystem(axisNameX, isUsingAxisRaw);
            _axisInputX.SetActiveAutoUpdate(false);
            _axisInputY = new AxisInputHandlerOldInputSystem(axisNameY, isUsingAxisRaw);
            _axisInputY.SetActiveAutoUpdate(false);
            SetActiveAutoUpdate(true);
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