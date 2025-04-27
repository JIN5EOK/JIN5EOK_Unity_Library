using Jin5eok.Extension;
using UnityEngine;

namespace Jin5eok.Inputs
{
    public class MousePositionInputHandler : InputHandler<Vector3>
    {
        public override Vector3 Value { get; protected set; }
        
        public override void UpdateState()
        {
            var currentValue = Input.mousePosition;
            
            if (Value.ApproximatelyEquals(currentValue) == false)
            {
                Value = currentValue;
                InputDetected(currentValue);
            }
        }
    }
}