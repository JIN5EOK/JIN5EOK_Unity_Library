using System;

namespace Jin5eok.Inputs
{
    public delegate void InputCallback<T>(T input) where T : notnull;

    public interface IInputHandlerBase : IDisposable
    {
        public void UpdateState();
    }
    
    public interface IInputHandler<T> : IInputHandlerBase where T : notnull
    {
        public event InputCallback<T> InputValueChanged;
        public T Value { get; }
    }
    
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