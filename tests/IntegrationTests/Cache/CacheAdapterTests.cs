using Infrastructure.Caching;
using Microsoft.Extensions.Caching.Memory;


namespace IntegrationTests.Cache
{
    public class CacheAdapterTests
    {
        private readonly MemoryCache _memoryCache;
        private readonly CacheAdapter<List<string>> _hashCache;

        public CacheAdapterTests()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _hashCache = new CacheAdapter<List<string>>(_memoryCache);
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
