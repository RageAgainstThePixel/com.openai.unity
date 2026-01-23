// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class RealtimeAudioFormatConfig
    {
        [Preserve]
        [JsonConstructor]
        public RealtimeAudioFormatConfig(
            [JsonProperty("type")] RealtimeAudioFormat type = RealtimeAudioFormat.Pcm,
            [JsonProperty("rate")] int? rate = 24000)
        {
            Type = type;
            Rate = rate;
        }

        /// <summary>
        /// Audio encoding format.
        /// </summary>
        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public RealtimeAudioFormat Type { get; }

        /// <summary>
        /// Sample rate in Hz.
        /// </summary>
        [Preserve]
        [JsonProperty("rate", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Rate { get; }
    }
}
