using UnityEngine;

namespace Jin5eok.Helper
{
    public class GameActionHelper2D
    {
        public static bool IsGrounded(Vector2 boxSize, Vector2 point, LayerMask groundLayer, float castDistance = 0.1f)
        {
            return Physics2D.BoxCast(point, boxSize, 0, Vector2.down, castDistance, groundLayer);
        }
        
        public static bool IsGrounded(Collider2D collider, LayerMask groundLayer, float castDistance = 0.1f)
        {
            Vector2 boxSize = collider.bounds.size;
            Vector2 point = collider.bounds.center;

            return IsGrounded(boxSize, point, groundLayer, castDistance);
        }
        
        public static Vector2 GetDirection(Vector2 from, Vector2 to)
        {
            return (to - from).normalized;
        }
        
        public static Vector2Int GetDirectionInt(Vector2 from, Vector2 to)
        {
            var dir = GetDirection(from, to);

            var dirX = dir.x == 0.0f ? 0 : dir.x > 0.0f ? 1 : -1; 
            var dirY = dir.y == 0.0f ? 0 : dir.y > 0.0f ? 1 : -1;

            return new Vector2Int(dirX, dirY);
        }
    }
}