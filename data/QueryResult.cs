using System.Text.Json.Serialization;

namespace Flow.Launcher.Plugin.Dictionary
{
    public class QueryResult
    {
        [JsonPropertyName("word")]
        public string Word { get; set; }

        [JsonPropertyName("phonetic")]
        public string Phonetic { get; set; }

        [JsonPropertyName("phonetics")]
        public Phonetic[] Phonetics { get; set; }

        [JsonPropertyName("meanings")]
        public Meaning[] Meanings { get; set; }
     }
}