using System.Text.Json.Serialization;

namespace ResourceChainProject.Models
{
    public class ExchangeRatesResponse
    {
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("base")]
        public required string Base { get; set; }

        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; set; } = new();
    }
} 