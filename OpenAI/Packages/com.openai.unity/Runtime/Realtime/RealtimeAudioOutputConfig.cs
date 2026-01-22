// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class RealtimeAudioOutputConfig
    {
        [Preserve]
        [JsonConstructor]
        public RealtimeAudioOutputConfig(
            [JsonProperty("format")] RealtimeAudioFormatConfig format = null,
            [JsonProperty("voice")] string voice = null,
            [JsonProperty("speed")] float? speed = null)
        {
            Format = format ?? new RealtimeAudioFormatConfig();
            Voice = voice;
            Speed = speed;
        }

        /// <summary>
        /// Output audio format configuration.
        /// </summary>
        [Preserve]
        [JsonProperty("format", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public RealtimeAudioFormatConfig Format { get; }

        /// <summary>
        /// Voice the model uses for audio output.
        /// </summary>
        [Preserve]
        [JsonProperty("voice", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Voice { get; }

        /// <summary>
        /// Optional output speed for the voice.
        /// </summary>
        [Preserve]
        [JsonProperty("speed", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public float? Speed { get; }
    }
}
