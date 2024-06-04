using System.Text.Json.Serialization;

namespace DotNetGenAI.Models.StableDiffusion
{
    public class StableTextPrompt
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("weight")]
        public float Weight { get; set; } = 1F;
    }
}
