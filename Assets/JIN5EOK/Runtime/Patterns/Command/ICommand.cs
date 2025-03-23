namespace Jin5eok.Patterns
{
    public interface ICommand<T>
    {
        public void Execute(T target);
    }
}