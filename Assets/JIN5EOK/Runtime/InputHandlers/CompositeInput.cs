using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jin5eok
{
    /// <summary>
    /// 같은 반환형을 가진 입력 핸들러 여러개가 합성된 복합 입력 핸들러를 구성합니다.
    /// 한가지 입력에 여러가지 입력수단을 할당하고 싶을 때 사용합니다.
    /// 예) 공격 명령에 KeyCode 입력값 Space Bar 키, Old InputSystem의 Attack 입력을 할당합니다.
    /// 주의사항: CompositeInputHandler에 포함된 개별 InputHandler를 포함시키면 해당 InputHandler는 CompositeInputHandler와 생명주기를 함께합니다.
    /// 따라서 요소로 넣은 핸들러를 독립적으로 다룰수는 있으나 권장하지는 않습니다.
    /// </summary>
    public abstract class CompositeInputHandlerBase<T> : InputHandler<T> where T : notnull
    {
        /// <summary>
        /// 현재 입력된 키 입력값을 반환합니다. 내부에 포함된 InputHandler들 중 하나라도 입력이 있으면 해당 값을 반환합니다.
        /// </summary>
        public override T Value { get; protected set; }

        private List<IInputHandler<T>> _childInputs = new();
        private IInputHandler<T>[] _cachedChildInputs = Array.Empty<IInputHandler<T>>();
        private bool _isDirty = true;

        private List<IInputHandler<T>> ChildInputs
        {
            get
            {
                ThrowIfDisposed();
                return _childInputs;
            }
            set
            {
                ThrowIfDisposed();
                _childInputs = value;
            }
        }
        
        
        public CompositeInputHandlerBase(params IInputHandler<T>[] inputGroup)
        {
            foreach (var input in inputGroup)
            {
                AddInput(input);
            }
        }

        /// <summary>
        /// CompositeInput 내부에 포함된 InputHandler들을 반환합니다.
        /// </summary>
        /// <returns>포함된 InputHandler들의 배열</returns>
        public IInputHandler<T>[] GetInputs()
        {
            return ChildInputs.ToArray();
        }
        
        /// <summary>
        /// InputHandler를 추가합니다.
        /// </summary>
        /// <param name="inputHandler">추가할 InputHandler</param>
        public void AddInput(IInputHandler<T> inputHandler)
        {
            if (ChildInputs.Contains(inputHandler) == false)
            {
                ChildInputs.Add(inputHandler);
                _isDirty = true;
            }
        }
        
        /// <summary>
        /// InputHandler를 제거합니다.
        /// </summary>
        /// <param name="inputHandler">제거할 InputHandler</param>
        public void RemoveInput(IInputHandler<T> inputHandler)
        {
            if (ChildInputs.Contains(inputHandler) == true)
            {
                ChildInputs.Remove(inputHandler);
                _isDirty = true;
            }
        }

        /// <summary>
        /// InputHandler를 모두 제거합니다.
        /// </summary>
        public void RemoveAllInputs()
        {
            ChildInputs.Clear();
            _isDirty = true;
        }

        /// <summary>
        /// 내부에 포함된 모든 InputHandler들을 업데이트하고, 입력값이 변경되면 이벤트를 발생시킵니다.
        /// </summary>
        public override void UpdateState()
        {
            if (ChildInputs == null || ChildInputs.Count == 0)
            {
                return;
            }
            
            // 컬렉션이 변경되었을 때만 캐시 갱신 (GC 할당 최소화)
            if (_isDirty)
            {
                _cachedChildInputs = ChildInputs.ToArray();
                _isDirty = false;
            }
            
            T changedValue = default;
            foreach (var input in _cachedChildInputs)
            {
                if (input == null || input.Disposed)
                {
                    continue;
                }
                
                input.UpdateState();
                if (IsEquals(input.Value, default) == false)
                {
                    changedValue = input.Value;
                }
            }

            var isInputBefore = Value.Equals(null) == false;
            var isInputCurrent = changedValue != null && changedValue.Equals(null) == false;
            var isInputChanged = changedValue != null && changedValue.Equals(Value) == false;
            
            if (isInputBefore == false && isInputCurrent == false)
            {
                return;
            }
            
            if (isInputBefore == true && isInputCurrent == false)
            {
                Value = default;
                InputDetected(Value);
                return;
            }
            
            if (isInputChanged)
            {
                Value = changedValue;
                InputDetected(changedValue);
                return;
            }
        }
        
        protected abstract bool IsEquals(T a, T b);

        public override void Dispose()
        {
            if (Disposed == true)
                return;
            
            foreach (var child in ChildInputs)
            {
                child.Dispose();
            }
            RemoveAllInputs();
            base.Dispose();
        }
    }
    
    /// <summary>
    /// int 타입 CompositeInputHandler
    /// </summary>
    public class IntCompositeInputHandler : CompositeInputHandlerBase<int>
    {
        /// <summary>
        /// IntCompositeInputHandler를 생성합니다.
        /// </summary>
        /// <param name="inputGroup">합성할 InputHandler들</param>
        public IntCompositeInputHandler(params IInputHandler<int>[] inputGroup) : base(inputGroup) { }

        protected override bool IsEquals(int a, int b)
        {
            return a == b;
        }
    }
    
    /// <summary>
    /// float 타입 CompositeInputHandler
    /// </summary>
    public class FloatCompositeInputHandler : CompositeInputHandlerBase<float>
    {
        /// <summary>
        /// FloatCompositeInputHandler를 생성합니다.
        /// </summary>
        /// <param name="inputGroup">합성할 InputHandler들</param>
        public FloatCompositeInputHandler(params IInputHandler<float>[] inputGroup) : base(inputGroup) { }
        
        protected override bool IsEquals(float a, float b)
        {
            return Mathf.Approximately(a, b);
        }
    }
    
    /// <summary>
    /// bool 타입 CompositeInputHandler
    /// </summary>
    public class BoolCompositeInputHandler : CompositeInputHandlerBase<bool>
    {
        /// <summary>
        /// BoolCompositeInputHandler를 생성합니다.
        /// </summary>
        /// <param name="inputGroup">합성할 InputHandler들</param>
        public BoolCompositeInputHandler(params IInputHandler<bool>[] inputGroup) : base(inputGroup) { }

        protected override bool IsEquals(bool a, bool b)
        {
            return a == b;
        }
    }
    
    /// <summary>
    /// Vector2 타입 CompositeInputHandler
    /// </summary>
    public class Vector2CompositeInputHandler : CompositeInputHandlerBase<Vector2>
    {
        /// <summary>
        /// Vector2CompositeInputHandler를 생성합니다.
        /// </summary>
        /// <param name="inputGroup">합성할 InputHandler들</param>
        public Vector2CompositeInputHandler(params IInputHandler<Vector2>[] inputGroup) : base(inputGroup) { }
        
        protected override bool IsEquals(Vector2 a, Vector2 b)
        {
            return a.ApproximatelyEquals(b);
        }
    }
    
    /// <summary>
    /// Vector3 타입 CompositeInputHandler
    /// </summary>
    public class Vector3CompositeInputHandler : CompositeInputHandlerBase<Vector3>
    {
        /// <summary>
        /// Vector3CompositeInputHandler를 생성합니다.
        /// </summary>
        /// <param name="inputGroup">합성할 InputHandler들</param>
        public Vector3CompositeInputHandler(params IInputHandler<Vector3>[] inputGroup) : base(inputGroup) { }
        
        protected override bool IsEquals(Vector3 a, Vector3 b)
        {
            return a.ApproximatelyEquals(b);
        }
    }
}