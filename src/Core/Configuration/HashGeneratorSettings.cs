using System.ComponentModel.DataAnnotations;

namespace Core.Configuration
{
    public class HashGeneratorSettings
    {
        public int HashLength { get; set; }
        public int RequestReserve { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Generation capacity must be greater than 0")]
        public int GenerationCapacity { get; set; }
    }
}
