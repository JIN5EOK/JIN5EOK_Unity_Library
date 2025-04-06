using Jin5eok.Extension;
using UnityEngine;

namespace Jin5eok.Inputs
{
    public class MousePositionInputHandler : IInputHandler<Vector3>
    {
        public event InputCallback<Vector3> InputValueChanged;
        public Vector3 Value { get; private set; }
        
        public void UpdateState()
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