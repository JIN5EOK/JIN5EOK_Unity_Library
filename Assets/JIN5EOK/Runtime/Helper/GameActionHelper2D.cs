using UnityEngine;

namespace Jin5eok.Helper
{
    public class GameActionHelper2D
    {
        public static void Move(GameObject target, Vector2 vel)
        {
            target.transform.Translate(vel * Time.deltaTime);
        }
        
        public static void MoveX(GameObject target, float vel)
        {
            Move(target, new Vector2(vel, 0));
        }
        
        public static void MoveY(GameObject target, float vel)
        {
            Move(target, new Vector2(0, vel));
        }

        public static void MoveToPoint(GameObject target, Vector2 to, float speed)
        {
            Move(target, GetDirection(target.transform.position, to) * speed);
        }
        
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
        
        public static void Jump(Rigidbody2D rigidbody2D, float power)
        {
            rigidbody2D.AddForce(Vector2.up * power);
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