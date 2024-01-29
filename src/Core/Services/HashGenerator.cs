using Core.Configuration;
using Core.Interfaces;
using Microsoft.Extensions.Options;

namespace Core.Services
{
    public class HashGenerator : IHashGenerator
    {
        private int _hashLength;
        private int _requestReserve;
        private int _generationCapacity;
        
        private bool isRestorationInProgress = false;
        private readonly Queue<string> _hashQueue = new();

        private readonly IAppCache<List<string>> _cache;

        public HashGenerator(
            IOptions<HashGeneratorSettings> options,
            IAppCache<List<string>> cache)
        {
            var settings = options.Value;
            _hashLength = settings.HashLength;
            _requestReserve = settings.RequestReserve;
            _generationCapacity = settings.GenerationCapacity;

            _cache = cache;
        }
        
        public async Task<string> GetHashAsync()
        {
            if (_hashQueue.Count <= _requestReserve && !isRestorationInProgress)
            {
                isRestorationInProgress = true;
                await RestoreAsync().ConfigureAwait(false);
            }

            return _hashQueue.Dequeue();
        }

        public async Task RestoreAsync()
        {
            var reserve = GetReserve();
            await Task.Run(() => SetNewReserve());
            ReplenishQueue(reserve);
            isRestorationInProgress = false;
        }

        public List<string> GetReserve()
        {
            var reserve = _cache.Get(Constants.HASH_GENERATOR_KEY);

            if (reserve == null)
            {
                reserve = new List<string>();

                for (int i = 0; i < _generationCapacity; i++)
                {
                    reserve.Add(Generate());
                }
            }

            return reserve;
        }

        public void SetNewReserve()
        {
            var reserve = new List<string>();

            for (int i = 0; i < _generationCapacity; i++)
            {
                reserve.Add(Generate());
            }

            _cache.Set(Constants.HASH_GENERATOR_KEY, reserve);
        }

        public void ReplenishQueue(List<string> hashes)
        {
            foreach (var hash in hashes)
            {
                _hashQueue.Enqueue(hash);
            }
        }

        public string Generate()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("/", "").Replace("+", "")
                .Substring(0, _hashLength);
        }
    }
}
