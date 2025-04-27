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
        
        public abstract event InputCallback<T> InputValueChanged;
        public abstract T Value { get; protected set; }
        public bool IsActiveAutoUpdate => InputHandlerUpdater.Contains(this);

        private bool _disposed;
        
        public abstract void UpdateState();
        
        public InputHandler()
        {
            InputHandlerUpdater.Add(this);
        }
        
        public virtual void Dispose()
        {
            if (_disposed == true)
            {
                return;
            }
            _disposed = true;
            
            InputHandlerUpdater.Remove(this);
            GC.SuppressFinalize(this);
        }

        ~InputHandler()
        {
            Dispose();
        }
    }
}