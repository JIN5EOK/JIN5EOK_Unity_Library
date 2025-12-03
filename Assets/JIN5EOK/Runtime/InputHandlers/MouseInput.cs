using UnityEngine;

namespace Jin5eok
{
    /// <summary>
    /// 마우스 입력 InputHandler입니다.
    /// Vector3 형식의 마우스 위치 값을 입력 값으로 반환합니다.
    /// </summary>
    public class MousePositionInputHandler : InputHandler<Vector3>
    {
        /// <summary>
        /// 현재 마우스 위치를 반환합니다. 화면 좌표계 기준입니다.
        /// </summary>
        public override Vector3 Value { get; protected set; }
        
        /// <summary>
        /// 마우스 위치를 업데이트합니다.
        /// </summary>
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