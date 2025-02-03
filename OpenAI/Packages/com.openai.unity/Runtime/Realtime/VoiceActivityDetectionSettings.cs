// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class VoiceActivityDetectionSettings
    {
        [Preserve]
        public VoiceActivityDetectionSettings(
            [JsonProperty("type")] TurnDetectionType type = TurnDetectionType.Server_VAD,
            [JsonProperty("threshold")] float? detectionThreshold = null,
            [JsonProperty("prefix_padding_ms")] int? prefixPadding = null,
            [JsonProperty("silence_duration_ms")] int? silenceDuration = null,
            [JsonProperty("create_response")] bool? createResponse = null)
        {
            switch (type)
            {
                case TurnDetectionType.Server_VAD:
                    Type = TurnDetectionType.Server_VAD;
                    DetectionThreshold = detectionThreshold;
                    PrefixPadding = prefixPadding;
                    SilenceDuration = silenceDuration;
                    CreateResponse = createResponse;
                    break;
            }
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TurnDetectionType Type { get; private set; }

        [Preserve]
        [JsonProperty("threshold")]
        public float? DetectionThreshold { get; private set; }

        [Preserve]
        [JsonProperty("prefix_padding_ms")]
        public int? PrefixPadding { get; private set; }

        [Preserve]
        [JsonProperty("silence_duration_ms")]
        public int? SilenceDuration { get; private set; }

        [Preserve]
        [JsonProperty("create_response")]
        public bool? CreateResponse { get; private set; }

        [Preserve]
        public static VoiceActivityDetectionSettings Disabled() => new(TurnDetectionType.Disabled);
    }
}
