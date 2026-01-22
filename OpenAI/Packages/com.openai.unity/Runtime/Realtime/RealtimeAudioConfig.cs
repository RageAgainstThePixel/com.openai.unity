// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class RealtimeAudioConfig
    {
        [Preserve]
        [JsonConstructor]
        public RealtimeAudioConfig(
            [JsonProperty("input")] RealtimeAudioInputConfig input = null,
            [JsonProperty("output")] RealtimeAudioOutputConfig output = null)
        {
            Input = input;
            Output = output;
        }

        /// <summary>
        /// Input audio configuration.
        /// </summary>
        [Preserve]
        [JsonProperty("input", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public RealtimeAudioInputConfig Input { get; }

        /// <summary>
        /// Output audio configuration.
        /// </summary>
        [Preserve]
        [JsonProperty("output", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public RealtimeAudioOutputConfig Output { get; }
    }
}
