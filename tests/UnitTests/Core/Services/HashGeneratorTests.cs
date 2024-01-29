using Core;
using Core.Configuration;
using Core.Interfaces;
using Core.Services;
using Microsoft.Extensions.Options;

namespace UnitTests.Core.Services
{
    public class HashGeneratorTests
    {
        private int _defaultHashLength = 8;
        private int _defaultGenerationCapacity = 50;

        private readonly Mock<IOptions<HashGeneratorSettings>> _mockOptions = new();
        private readonly Mock<IAppCache<List<string>>> _mockCache = new();

        [Fact]    
        public async Task GetHashAsync_ReturnsListOfUniqueStrings()
        {
            int requestCount = 100;
            _mockOptions.SetupGet(x => x.Value).Returns(new HashGeneratorSettings()
            {
                HashLength = _defaultHashLength,
                RequestReserve = It.IsAny<int>(),
                GenerationCapacity = _defaultGenerationCapacity
            });
            var hashGenerator = new HashGenerator(_mockOptions.Object, _mockCache.Object);
            
            var hashes = new List<string>();
            for (int i = 0; i < requestCount; i++)
            {
                hashes.Add(await hashGenerator.GetHashAsync());
            }

            Assert.True(hashes.TrueForAll(h => h.Length == _defaultHashLength));
            Assert.True(hashes.Distinct().Count() == requestCount);
        }

        [Fact]
        public void GetReserve_ReturnsCachedListOfStrings()
        {
            _mockOptions.SetupGet(x => x.Value).Returns(new HashGeneratorSettings()
            {
                HashLength = It.IsAny<int>(),
                RequestReserve = It.IsAny<int>(),
                GenerationCapacity = It.IsAny<int>()
            });
            var expected = new List<string>()
            {
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            };
            _mockCache.Setup(x => x.Get(Constants.HASH_GENERATOR_KEY)).Returns(expected);
            var hashGenerator = new HashGenerator(_mockOptions.Object, _mockCache.Object);

            var actual = hashGenerator.GetReserve();

            Assert.Equal(expected, actual);
            _mockCache.Verify(x => x.Get(Constants.HASH_GENERATOR_KEY), Times.Once);
        }

        [Fact]
        public void GetReserve_ReturnsListOfGeneratedHashes()
        {
            _mockOptions.SetupGet(x => x.Value).Returns(new HashGeneratorSettings()
            {
                HashLength = _defaultHashLength,
                RequestReserve = It.IsAny<int>(),
                GenerationCapacity = _defaultGenerationCapacity
            });
            var hashGenerator = new HashGenerator(_mockOptions.Object, _mockCache.Object);

            var hashes = hashGenerator.GetReserve();

            Assert.True(hashes.TrueForAll(h => h.Length == _defaultHashLength));
            Assert.True(hashes.Count() == _defaultGenerationCapacity);
            Assert.True(hashes.Distinct().Count() == _defaultGenerationCapacity);
        }
    }
}
