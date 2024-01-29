using Ardalis.Specification;
using Core.Entities;

namespace Core.Specifications
{
    public class ObjectDetailsSpecification : Specification<ObjectDetails>, ISingleResultSpecification<ObjectDetails>
    {
        public ObjectDetailsSpecification(string key)
        {
            Query
                .Where(o => o.Key == key);
        }
    }
}
