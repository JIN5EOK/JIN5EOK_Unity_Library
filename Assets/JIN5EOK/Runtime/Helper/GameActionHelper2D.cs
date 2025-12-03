using UnityEngine;

namespace Jin5eok
{
    /// <summary>
    /// 2D 게임 로직 구현을 위한 헬퍼 클래스입니다.
    /// </summary>
    public class GameActionHelper2D
    {
        /// <summary>
        /// 지정된 박스 크기와 위치를 사용하여 지면에 닿아있는지 확인합니다, 사이드 스크롤러 게임에 사용합니다.
        /// </summary>
        /// <param name="boxSize">박스 캐스트의 크기</param>
        /// <param name="point">박스 캐스트의 중심점</param>
        /// <param name="groundLayer">지면으로 인식할 레이어 마스크</param>
        /// <param name="castDistance">캐스트 거리</param>
        /// <returns>지면에 닿아있으면 true, 그렇지 않으면 false</returns>
        public static bool IsGrounded(Vector2 boxSize, Vector2 point, LayerMask groundLayer, float castDistance = 0.1f)
        {
            return Physics2D.BoxCast(point, boxSize, 0, Vector2.down, castDistance, groundLayer);
        }
        
        /// <summary>
        /// Collider2D를 사용하여 지면에 닿아있는지 확인합니다, 사이드 스크롤러 게임에 사용합니다
        /// </summary>
        /// <param name="collider">지면 확인에 사용할 Collider2D</param>
        /// <param name="groundLayer">지면으로 인식할 레이어 마스크</param>
        /// <param name="castDistance">캐스트 거리</param>
        /// <returns>지면에 닿아있으면 true, 그렇지 않으면 false</returns>
        public static bool IsGrounded(Collider2D collider, LayerMask groundLayer, float castDistance = 0.1f)
        {
            Vector2 boxSize = collider.bounds.size;
            Vector2 point = collider.bounds.center;

            return IsGrounded(boxSize, point, groundLayer, castDistance);
        }
        
        /// <summary>
        /// 두 지점 사이의 정규화된 방향 벡터를 반환합니다.
        /// </summary>
        /// <param name="from">시작 지점</param>
        /// <param name="to">목표 지점</param>
        /// <returns>정규화된 방향 벡터</returns>
        public static Vector2 GetDirection(Vector2 from, Vector2 to)
        {
            return (to - from).normalized;
        }
        
        /// <summary>
        /// 두 지점 사이의 정수 방향 벡터를 반환합니다. (-1, 0, 1 값만 사용)
        /// </summary>
        /// <param name="from">시작 지점</param>
        /// <param name="to">목표 지점</param>
        /// <returns>정수 방향 벡터 (각 축은 -1, 0, 1 중 하나)</returns>
        public static Vector2Int GetDirectionInt(Vector2 from, Vector2 to)
        {
            var dir = GetDirection(from, to);

            var dirX = dir.x == 0.0f ? 0 : dir.x > 0.0f ? 1 : -1; 
            var dirY = dir.y == 0.0f ? 0 : dir.y > 0.0f ? 1 : -1;

            return new Vector2Int(dirX, dirY);
        }
    }
}