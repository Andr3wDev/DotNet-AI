using System.Text.Json.Serialization;

namespace DotNetGenAI.Models.StableDiffusion
{
    public class Artifact
    {
        [JsonPropertyName("base64")]
        public string Base64 { get; set; }

        [JsonPropertyName("finishReason")]
        public string FinishReason { get; set; }
    }
}
