using Ardalis.GuardClauses;
using Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class ObjectDetails : IAggregateRoot
    {
        public ObjectDetails(string key, DateTime expiration)
        {
            Guard.Against.NullOrEmpty(key, nameof(key));
            Key = key;
            Expiration = expiration;
        }

        [Key]
        public string Key { get; private set; }
        public DateTime Expiration { get; private set; }
    }
}
