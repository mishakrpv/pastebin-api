namespace Core.Interfaces
{
    public interface IAppCache<T> where T : notnull
    {
        T? Get(object key);

        void Set(object key, T value);
    }
}
