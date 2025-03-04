using System.Text.Json.Serialization;

namespace Flow.Launcher.Plugin.Dictionary
{
    public class Meaning
    {
        [JsonPropertyName("partOfSpeech")]
        public string PartOfSpeech { get; set; }
        
        [JsonPropertyName("definitions")]
        public Definition[] Definitions { get; set; }
        
        [JsonPropertyName("synonyms")]
        public string[] Synonyms { get; set; }
    }
}