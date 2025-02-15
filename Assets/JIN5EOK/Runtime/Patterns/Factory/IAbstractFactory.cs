namespace Jin5eok.Patterns.Factory
{
    public interface IAbstractFactory<T>
    {
        public T Create();
    }
}