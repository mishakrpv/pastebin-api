using Ardalis.Specification;
using Core.Entities;
using Core.Specifications;

namespace UnitTests.Core.Specifications
{
    public class ObjectDetailsSpecificationTests
    {
        private string _testObjectDetailsKey = "test";

        [Fact]
        public void MatchesObjectDetailsWithGivenKey()
        {
            var specification = new ObjectDetailsSpecification(_testObjectDetailsKey);

            var result = specification.Evaluate(GetTestObjectDetailsCollection()).FirstOrDefault();

            Assert.NotNull(result);
            Assert.Equal(_testObjectDetailsKey, result.Key);
        }

        [Fact]
        public void MatchesNoObjectDetailsIfKeyNotPresent()
        {
            string badKey = "badKey";
            var specification = new ObjectDetailsSpecification(badKey);

            var result = specification.Evaluate(GetTestObjectDetailsCollection()).Any();

            Assert.False(result);
        }

        public List<ObjectDetails> GetTestObjectDetailsCollection()
        {
            return new List<ObjectDetails>()
            {
                new ObjectDetails("1", It.IsAny<DateTime>()),
                new ObjectDetails("2", It.IsAny<DateTime>()),
                new ObjectDetails(_testObjectDetailsKey, It.IsAny<DateTime>())
            };
        }
    }
}
