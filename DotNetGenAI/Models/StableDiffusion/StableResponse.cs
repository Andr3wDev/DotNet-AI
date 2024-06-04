using System.Text.Json.Serialization;

namespace DotNetGenAI.Models.StableDiffusion
{
    public class StableResponse
    {
        [JsonPropertyName("result")]
        public string Result { get; set; }

        [JsonPropertyName("artifacts")]
        public List<Artifact> Artifacts { get; set; }
    }
}
