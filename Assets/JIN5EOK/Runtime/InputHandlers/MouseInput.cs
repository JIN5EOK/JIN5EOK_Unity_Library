using Jin5eok.Extension;
using UnityEngine;

namespace Jin5eok.Inputs
{
    public class MousePositionInputHandler : InputHandler<Vector3>
    {
        public override event InputCallback<Vector3> InputValueChanged;
        public override Vector3 Value { get; protected set; }

        public MousePositionInputHandler()
        {
            SetActiveAutoUpdate(true);
        }
        
        public override void UpdateState()
        {
            var currentValue = Input.mousePosition;
            
            if (Value.ApproximatelyEquals(currentValue) == false)
            {
                Value = currentValue;
                InputValueChanged?.Invoke(currentValue);
            }
        }
    }
}