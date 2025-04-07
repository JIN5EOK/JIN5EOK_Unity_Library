using System;

namespace Jin5eok.Inputs
{
    public delegate void InputCallback<T>(T input) where T : notnull;

    public interface IInputHandlerBase
    {
        public bool IsActiveAutoUpdate { get; }
        public void SetActiveAutoUpdate(bool isActive);
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
        public bool IsActiveAutoUpdate => InputHandlerUpdater.ContainsInputHandler(this);
        
        public abstract void UpdateState();
        
        public void SetActiveAutoUpdate(bool isActive)
        {
            if (isActive == true)
            {
                InputHandlerUpdater.AddInputHandler(this);
            }
            else
            {
                InputHandlerUpdater.RemoveInputHandler(this);
            }
        }

        public virtual void Dispose()
        {
            SetActiveAutoUpdate(false);
            GC.SuppressFinalize(this);
        }

        ~InputHandler()
        {
            Dispose();
        }
    }
}