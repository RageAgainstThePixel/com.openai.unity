// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Models;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class InputAudioTranscriptionSettings
    {
        [Preserve]
        public InputAudioTranscriptionSettings(Model model, string prompt = null, string language = null)
        {
            Model = string.IsNullOrWhiteSpace(model?.Id) ? Models.Model.Whisper1 : model;
            Prompt = prompt;
            Language = language;
        }

        [Preserve]
        [JsonConstructor]
        internal InputAudioTranscriptionSettings(
            [JsonProperty("model")] string model,
            [JsonProperty("prompt")] string prompt = null,
            [JsonProperty("language")] string language = null)
        {
            Model = model;
            Prompt = prompt;
            Language = language;
        }

        [Preserve]
        [JsonProperty("model")]
        public string Model { get; }

        [Preserve]
        [JsonProperty("prompt")]
        public string Prompt { get; }

        [Preserve]
        [JsonProperty("language")]
        public string Language { get; }
    }
}
