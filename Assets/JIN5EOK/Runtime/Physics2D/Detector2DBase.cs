using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jin5eok
{
    /// <summary>
    /// 2D 감지(Detector) 공통 베이스 클래스입니다.
    /// - 감지 타겟은 SetTargetType으로 주입합니다. (DIP)
    /// - 감지 결과는 이벤트 콜백(TargetEntered/Exited/Stayed)으로 노출합니다.
    /// </summary>
    public abstract class Detector2DBase : MonoBehaviour
    {
        public event Action<Component> OnTargetEntered;
        public event Action<Component> OnTargetExited;
        public event Action<Component> OnTargetStayed;
        
        [Header("컴포넌트를 찾을때 부모까지 찾을지 여부")]
        [SerializeField] private bool _includeParent = true;

        public bool IncludeParent
        {
            get => _includeParent;
            set => _includeParent = value;
        }
        
        [Header("Stay 사용 여부")]
        [SerializeField] private bool _useStay = false;
        
        public bool UseStay
        {
            get => _useStay;
            set => _useStay = value;
        }

        public Type TargetType { get; set; }
        
        private Component FindTarget(Component other)
        {
            if (other == null || TargetType == null)
            {
                return null;
            }

            var target = other.GetComponent(TargetType);
            if (target != null)
            {
                return target;
            }

            if (_includeParent)
            {
                return other.GetComponentInParent(TargetType);
            }

            return null;
        }

        protected void RaiseEnter(Component other)
        {
            var target = FindTarget(other);
            if (target != null)
            {
                OnTargetEntered?.Invoke(target);
            }
        }

        protected void RaiseExit(Component other)
        {
            var target = FindTarget(other);
            if (target != null)
            {
                OnTargetExited?.Invoke(target);
            }
        }

        protected void RaiseStay(Component other)
        {
            if (!_useStay)
                return;

            var target = FindTarget(other);
            
            if (target == null)
                return;

            OnTargetStayed?.Invoke(target);
        }
    }
}

