// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Chat
{
    public sealed class AudioConfig
    {
        [Preserve]
        [JsonConstructor]
        internal AudioConfig(
            [JsonProperty("voice")] string voice,
            [JsonProperty("format")] AudioFormat format)
        {
            Voice = string.IsNullOrWhiteSpace(voice) ? OpenAI.Voice.Alloy : voice;
            Format = format;
        }

        [Preserve]
        public AudioConfig(Voice voice, AudioFormat format = AudioFormat.Pcm16)
            : this(voice?.Id, format)
        {
        }

        [Preserve]
        [JsonProperty("voice")]
        public string Voice { get; }

        [Preserve]
        [JsonProperty("format")]
        public AudioFormat Format { get; }

        [Preserve]
        public static implicit operator AudioConfig(Voice voice) => new(voice);
    }
}
