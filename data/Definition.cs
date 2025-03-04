using System.Text.Json.Serialization;

namespace Flow.Launcher.Plugin.Dictionary
{
    public class Definition
    {
        [JsonPropertyName("definition")]
        public string DefinitionText { get; set; }
    }
}