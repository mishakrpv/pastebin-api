using Core.Entities;

namespace IntegrationTests.Builders
{
    public class ObjectDetailsBuilder
    {
        private ObjectDetails _objectDetails;
        public string TestKey => "123";
        public DateTime TestExpiration => default;

        public ObjectDetailsBuilder()
        {
            _objectDetails = WithDefaultValues();
        }

        public ObjectDetails Build()
        {
            return _objectDetails;
        }

        public ObjectDetails WithDefaultValues()
        {
            _objectDetails = new ObjectDetails(TestKey, TestExpiration);
            return _objectDetails;
        }
    }
}
