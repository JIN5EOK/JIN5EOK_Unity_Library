using UnityEngine;

namespace Jin5eok
{
    /// <summary>
    /// Vector 관련 확장 메서드를 제공하는 클래스입니다.
    /// </summary>
    public static class VectorExtension
    {
        /// <summary>
        /// 두 Vector2가 거의 같은지 확인합니다. (부동소수점 오차 허용)
        /// </summary>
        /// <param name="a">첫 번째 벡터</param>
        /// <param name="b">두 번째 벡터</param>
        /// <param name="tolerance">허용 오차</param>
        /// <returns>거의 같으면 true, 그렇지 않으면 false</returns>
        public static bool ApproximatelyEquals(this Vector2 a, Vector2 b, float tolerance = 0.0001f)
        {
            return Vector2.SqrMagnitude(a - b) < tolerance * tolerance;
        }

        /// <summary>
        /// 두 Vector3가 거의 같은지 확인합니다. (부동소수점 오차 허용)
        /// </summary>
        /// <param name="a">첫 번째 벡터</param>
        /// <param name="b">두 번째 벡터</param>
        /// <param name="tolerance">허용 오차</param>
        /// <returns>거의 같으면 true, 그렇지 않으면 false</returns>
        public static bool ApproximatelyEquals(this Vector3 a, Vector3 b, float tolerance = 0.0001f)
        {
            return Vector3.SqrMagnitude(a - b) < tolerance * tolerance;
        }
    }
}