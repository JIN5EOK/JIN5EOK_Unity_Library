using UnityEngine;

namespace Jin5eok
{
    /// <summary>
    /// 마우스 입력 InputHandler입니다.
    /// Vector3 형식의 마우스 위치 값을 입력 값으로 반환합니다.
    /// </summary>
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