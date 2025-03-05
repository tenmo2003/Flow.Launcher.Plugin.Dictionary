using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Net.Http;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Flow.Launcher.Plugin.Dictionary
{
    public class QueryService : IDisposable
    {
        private readonly HttpClient httpClient = new();

        private const string iconPath = "icon.png";

        private const string Url = "https://api.dictionaryapi.dev/api/v2/entries/en/{0}";

        public async Task<List<Result>> Query(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return new List<Result> { new() { Title = "Enter the word you wish to conquer! ╰( ◕ ᗜ ◕ )╯", IcoPath = iconPath } };
            }

            var url = string.Format(Url, query);

            var response = await httpClient.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<Result> { new() { Title = "No definitions found (｡•́︿•̀｡)", IcoPath = iconPath } };
            }

            if (!response.IsSuccessStatusCode)
            {
                return new List<Result> { new() { Title = "An error occurred (｡•́︿•̀｡)", SubTitle = response.ReasonPhrase, IcoPath = iconPath } };
            }

            string content = await response.Content.ReadAsStringAsync();

            List<QueryResult> queryResult = System.Text.Json.JsonSerializer.Deserialize<List<QueryResult>>(content);

            QueryResult mainResult = queryResult[0];

            string phonetic = mainResult.Phonetic;
            string audioURL = null;
            for (int i = 0; i < mainResult.Phonetics.Length; i++)
            {
                if (string.IsNullOrEmpty(phonetic) && !string.IsNullOrEmpty(mainResult.Phonetics[i].Text))
                {
                    phonetic = mainResult.Phonetics[i].Text;
                }

                if (string.IsNullOrEmpty(audioURL) && !string.IsNullOrEmpty(mainResult.Phonetics[i].Audio))
                {
                    audioURL = mainResult.Phonetics[i].Audio;
                }

                if (!string.IsNullOrEmpty(phonetic) && !string.IsNullOrEmpty(audioURL))
                {
                    break;
                }
            }

            var results = new List<Result>();
            if (!string.IsNullOrEmpty(phonetic))
            {
                var result = new Result
                {
                    Title = phonetic,
                    SubTitle = "Phonetic (Select to play audio)",
                    IcoPath = iconPath,
                };

                results.Add(result);
            }

            _ = FetchPronunciationAudio(audioURL, results);

            foreach (var meaning in mainResult.Meanings)
            {
                foreach (var definition in meaning.Definitions)
                {
                    var result = new Result
                    {
                        Title = definition.DefinitionText,
                        SubTitle = meaning.PartOfSpeech,
                        IcoPath = iconPath
                    };

                    results.Add(result);
                }
            }

            return results;
        }

        private async Task FetchPronunciationAudio(string audioURL, List<Result> results)
        {
            if (!string.IsNullOrEmpty(audioURL))
            {
                byte[] audioBytes = await httpClient.GetByteArrayAsync(audioURL);

                Stream audioStream = new MemoryStream(audioBytes);

                var reader = new Mp3FileReader(audioStream);
                var waveOut = new WaveOut();
                waveOut.Init(reader);

                if (results.Count > 0)
                {
                    results[0].Action = (c) =>
                    {
                        reader.Seek(0, SeekOrigin.Begin);
                        waveOut.Play();

                        return false;
                    };
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            httpClient.Dispose();
        }
    }
}