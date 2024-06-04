using System.Text.Json.Serialization;

namespace DotNetGenAI.Models.Cohere
{
    internal class CoherePrompt
    {
        public CoherePrompt(string prompt) => Prompt = prompt;

        [JsonPropertyName("temperature")]
        public decimal Temperature { get; set; } = 0.7m;

        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }

        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; } = 100;

        
    }
}
