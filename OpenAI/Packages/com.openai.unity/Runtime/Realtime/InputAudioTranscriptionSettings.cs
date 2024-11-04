// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Models;

namespace OpenAI.Realtime
{
    public sealed class InputAudioTranscriptionSettings
    {
        [JsonConstructor]
        public InputAudioTranscriptionSettings([JsonProperty("model")] Model model)
        {
            Model = string.IsNullOrWhiteSpace(model.Id) ? "whisper-1" : model;
        }

        [JsonProperty("model")]
        public string Model { get; }
    }
}
