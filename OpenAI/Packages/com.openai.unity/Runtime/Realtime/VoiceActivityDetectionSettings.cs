// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Realtime
{
    public sealed class VoiceActivityDetectionSettings
    {
        public VoiceActivityDetectionSettings(
            [JsonProperty("type")] TurnDetectionType type = TurnDetectionType.Server_VAD,
            [JsonProperty("threshold")] float? detectionThreshold = null,
            [JsonProperty("prefix_padding_ms")] int? prefixPadding = null,
            [JsonProperty("silence_duration_ms")] int? silenceDuration = null)
        {
            switch (type)
            {
                case TurnDetectionType.Server_VAD:
                    Type = TurnDetectionType.Server_VAD;
                    DetectionThreshold = detectionThreshold;
                    PrefixPadding = prefixPadding;
                    SilenceDuration = silenceDuration;
                    break;
            }
        }

        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TurnDetectionType Type { get; private set; }

        [JsonProperty("threshold")]
        public float? DetectionThreshold { get; private set; }

        [JsonProperty("prefix_padding_ms")]
        public int? PrefixPadding { get; private set; }

        [JsonProperty("silence_duration_ms")]
        public int? SilenceDuration { get; private set; }

        public static VoiceActivityDetectionSettings Disabled() => new(TurnDetectionType.Disabled);
    }
}
