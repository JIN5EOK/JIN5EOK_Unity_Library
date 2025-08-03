namespace Jin5eok
{
    public interface ICommand<T>
    {
        public void Execute(T target);
    }
}