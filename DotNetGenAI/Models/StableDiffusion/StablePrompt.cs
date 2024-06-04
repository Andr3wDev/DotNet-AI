using System.Text.Json.Serialization;

namespace DotNetGenAI.Models.StableDiffusion
{
    public class StablePrompt
    {
        public StablePrompt(string prompt)
        {
            var textPrompt = new TextPrompt()
            {
                Text = prompt,
            };
            TextPrompts.Add(textPrompt);
        }

        [JsonPropertyName("text_prompts")]
        public List<StableTextPrompt> TextPrompts { get; set; } = [];

        [JsonPropertyName("cfg_scale")]
        public float CFGScale { get; set; } = 20F;

        [JsonPropertyName("steps")]
        public int Steps { get; set; } = 50;
    }
}
