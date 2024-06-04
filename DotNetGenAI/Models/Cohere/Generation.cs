using System.Text.Json.Serialization;

namespace DotNetGenAI.Models.Cohere
{
    public class Generation
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
