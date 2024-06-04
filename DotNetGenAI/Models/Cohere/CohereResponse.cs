using System.Text.Json.Serialization;

namespace DotNetGenAI.Models.Cohere
{
    public class CohereResponse
    {
        [JsonPropertyName("generations")]
        public Generation[]? Generations { get; set; }
    }
}
