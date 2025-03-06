using System.Text.Json.Serialization;

namespace Flow.Launcher.Plugin.FreeDictionary
{
    public class Definition
    {
        [JsonPropertyName("definition")]
        public string DefinitionText { get; set; }
    }
}