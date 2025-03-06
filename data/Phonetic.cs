using System.Text.Json.Serialization;

namespace Flow.Launcher.Plugin.FreeDictionary
{
    public class Phonetic
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
        
        [JsonPropertyName("audio")]
        public string Audio { get; set; }
    }
}