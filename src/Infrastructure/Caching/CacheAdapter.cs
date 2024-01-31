using Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Caching
{
    public class CacheAdapter<T> : IAppCache<T> where T : notnull
    {
        private readonly IMemoryCache _memoryCache;
        
        public CacheAdapter(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T? Get(object key)
        {
            return (T?)_memoryCache.Get(key);
        }

        public void Set(object key, T value)
        {
            _memoryCache.Set(key, value);
        }
    }
}
