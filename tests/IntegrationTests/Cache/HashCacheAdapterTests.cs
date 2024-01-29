using Infrastructure.Caching;
using Microsoft.Extensions.Caching.Memory;


namespace IntegrationTests.Cache
{
    public class HashCacheAdapterTests
    {
        private readonly MemoryCache _memoryCache;
        private readonly HashCacheAdapter _hashCache;

        public HashCacheAdapterTests()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _hashCache = new HashCacheAdapter(_memoryCache);
        }

        [Fact]
        public void GetsSettedHashes()
        {
            var key = "test_key";
            var expected = new List<string>();
            _hashCache.Set(key, expected);

            var actual = _hashCache.Get(key);

            Assert.Equal(expected, actual);
        }
    }
}
