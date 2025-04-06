using Jin5eok.Extension;
using UnityEngine;

namespace Jin5eok.Inputs
{
    public abstract class VectorInputHandlerBase : IInputHandler<Vector2>
    {
        public event InputCallback<Vector2> InputValueChanged;
        public Vector2 Value { get; private set; }

        public abstract void UpdateState();

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
        public KeyCode Up { get; set; }
        public KeyCode Down { get; set; }
        public KeyCode Left { get; set; }
        public KeyCode Right { get; set; }

        public VectorInputHandlerKeyCode(KeyCode up, KeyCode down, KeyCode left, KeyCode right)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
        }
        
        public override void UpdateState()
        {
            var isHorizontalPositive = Input.GetKey(Right) == true && Input.GetKeyUp(Right) == false && Input.GetKey(Left) == false;
            var isHorizontalNegative = Input.GetKey(Left) == true  && Input.GetKeyUp(Left) == false && Input.GetKey(Right) == false;
            var isVerticalPositive = Input.GetKey(Up) == true && Input.GetKeyUp(Up) == false  && Input.GetKey(Down) == false;
            var isVerticalNegative = Input.GetKey(Down) == true && Input.GetKeyUp(Down) == false  && Input.GetKey(Up) == false;
            
            float horizontal = isHorizontalPositive == true ? 1.0f : isHorizontalNegative == true ? -1.0f : 0.0f;
            float vertical = isVerticalPositive == true ? 1.0f : isVerticalNegative == true ? -1.0f : 0.0f;
            
            var currentValue = new Vector2(horizontal, vertical);
            UpdateState(currentValue);
        }
    }
  
    public class VectorInputHandlerOldInputSystem : VectorInputHandlerBase
    {
        public string HorizontalAxisName { get; set; }
        public string VerticalAxisName { get; set; }
        public bool IsUsingAxisRaw { get; set; }
        
        public VectorInputHandlerOldInputSystem(string horizontalAxisName, string verticalAxisName, bool isUsingAxisRaw = false)
        {
            HorizontalAxisName = horizontalAxisName;
            VerticalAxisName = verticalAxisName;
            IsUsingAxisRaw = isUsingAxisRaw;
        }
        
        public override void UpdateState()
        {
            var horizontal = IsUsingAxisRaw == true ? Input.GetAxisRaw(HorizontalAxisName) : Input.GetAxis(HorizontalAxisName);
            var vertical = IsUsingAxisRaw == true ? Input.GetAxisRaw(VerticalAxisName) : Input.GetAxis(VerticalAxisName);
            
            var currentValue = new Vector2(horizontal, vertical);
            UpdateState(currentValue);
        }
    }
    

}