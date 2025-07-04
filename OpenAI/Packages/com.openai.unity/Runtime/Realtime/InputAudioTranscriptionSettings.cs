// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Models;

namespace OpenAI.Realtime
{
    public sealed class InputAudioTranscriptionSettings
    {
        [JsonConstructor]
        public InputAudioTranscriptionSettings(
            [JsonProperty("model")] Model model,
            [JsonProperty("prompt")] string prompt = null,
            [JsonProperty("language")] string language = null)
        {
            Model = string.IsNullOrWhiteSpace(model.Id) ? "whisper-1" : model;
            Prompt = prompt;
            Language = language;
        }

        [JsonProperty("model")]
        public string Model { get; }

        [JsonProperty("prompt")]
        public string Prompt { get; }

        [JsonProperty("language")]
        public string Language { get; }
    }
}
