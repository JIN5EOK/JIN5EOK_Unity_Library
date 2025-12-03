using System;

namespace Jin5eok
{
    public delegate void InputCallback<T>(T input) where T : notnull;

    /// <summary>
    /// InputHandler들의 일괄적인 Update, Dispose 수행을 위해 상속받는 인터페이스입니다.
    /// </summary>
    public interface IInputHandlerBase : IDisposable
    {
        /// <summary>
        /// InputHandlerUpdater에 의해 매 프레임 호출되며 호출시 입력 값을 반영하는 업데이트를 수행합니다.
        /// 일반적으로는 사용자가 직접 호출할 필요 없습니다.
        /// </summary>
        public void UpdateState();
    }
    
    /// <summary>
    /// InputHandler들에 입력 Value관련 기능을 상속하기 위한 인터페이스입니다.
    /// </summary>
    public interface IInputHandler<T> : IInputHandlerBase where T : notnull
    {
        /// <summary>
        /// 입력값이 변경될 때 실행될 콜백 이벤트입니다.
        /// </summary>
        public event InputCallback<T> InputValueChanged;
        /// <summary>
        /// 현재 입력된 키 입력값을 반환합니다.
        /// </summary>
        public T Value { get; }
    }
    
    /// <summary>
    /// 모든 InputHandler들이 상속받는 베이스 클래스입니다.
    /// IInputHandlerBase, InputHandlerBase를 상속받습니다.
    /// </summary>
    public abstract class InputHandler<T> : IInputHandler<T> where T : notnull
    {
        private static InputHandlerUpdater InputHandlerUpdater => InputHandlerUpdater.Instance;
        
        private InputCallback<T> _inputValueChanged;
        public event InputCallback<T> InputValueChanged
        {
            add
            {
                ThrowIfDisposed();
                _inputValueChanged += value;
            }
            remove
            {
                ThrowIfDisposed();
                _inputValueChanged -= value;  
            } 
        }
        
        public abstract T Value { get; protected set; }
        public bool Disposed { get; private set; }
        
        public abstract void UpdateState();
        
        public InputHandler()
        {
            InputHandlerUpdater.Add(this);
        }
        
        /// <summary>
        /// InputHandlerUpdater에 할당된 콜백을 지워 자동으로 업데이트 되지 않도록 합니다.
        /// </summary>
        public virtual void Dispose()
        {
            if (Disposed == true)
            {
                return;
            }
            Disposed = true;
            
            _inputValueChanged = null;
            InputHandlerUpdater.Remove(this);
        }
        
        protected void InputDetected(T value)
        {
            _inputValueChanged?.Invoke(value);
        }
        
        protected void ThrowIfDisposed()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
    }
}