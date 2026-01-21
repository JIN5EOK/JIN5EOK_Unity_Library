using UnityEngine;

namespace Jin5eok
{
    /// <summary>
    /// 2D Trigger 기반 감지 컴포넌트입니다.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class TriggerDetector2D : Detector2DBase
    {
        private Collider2D _triggerCollider;
        
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _triggerCollider = GetComponent<Collider2D>();
            if (_triggerCollider != null)
            {
                _triggerCollider.isTrigger = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            RaiseEnter(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            RaiseExit(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            RaiseStay(other);
        }
    }
}

