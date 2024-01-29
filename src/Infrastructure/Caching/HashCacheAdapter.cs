using Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Caching
{
    public class HashCacheAdapter : IAppCache<List<string>>
    {
        private readonly IMemoryCache _memoryCache;
        
        public HashCacheAdapter(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public List<string>? Get(object key)
        {
            return (List<string>?)_memoryCache.Get(key);
        }

        public void Set(object key, List<string> value)
        {
            _memoryCache.Set(key, value);
        }
    }
}
