using Ardalis.GuardClauses;
using Core.Interfaces;

namespace Core.Entities
{
    public class TextObject : IObjectItem
    {
        public TextObject(string contentBody)
        {
            Guard.Against.NullOrEmpty(contentBody, nameof(contentBody));
            ContentBody = contentBody;
        }

        public string ContentBody { get; private set; }
    }
}
