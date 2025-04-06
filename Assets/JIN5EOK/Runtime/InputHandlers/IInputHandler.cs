namespace Jin5eok.Inputs
{
    public delegate void InputCallback<T>(T input) where T : notnull;

    public interface IInputHandlerBase
    {
        public void UpdateState();
    }
    
    public interface IInputHandler<T> : IInputHandlerBase where T : notnull
    {
        public event InputCallback<T> InputValueChanged;
        public T Value { get; }
    }
}