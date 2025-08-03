using UnityEngine;

namespace Jin5eok
{
    public static class VectorExtension
    {
        public static bool ApproximatelyEquals(this Vector2 a, Vector2 b, float tolerance = 0.0001f)
        {
            return Vector2.SqrMagnitude(a - b) < tolerance * tolerance;
        }

        public static bool ApproximatelyEquals(this Vector3 a, Vector3 b, float tolerance = 0.0001f)
        {
            return Vector3.SqrMagnitude(a - b) < tolerance * tolerance;
        }
    }
}