using UnityEngine;

namespace Jin5eok
{
    /// <summary>
    /// 2D Collision 기반 감지 컴포넌트입니다.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class CollisionDetector2D : Detector2DBase
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            RaiseEnter(collision.collider);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            RaiseExit(collision.collider);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            RaiseStay(collision.collider);
        }
    }
}

