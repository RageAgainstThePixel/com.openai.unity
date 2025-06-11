// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    [Obsolete("Use new IVoiceActivityDetectionSettings classes: SemanticVAD, ServerVAD, and DisabledVAD")]
    public sealed class VoiceActivityDetectionSettings : IVoiceActivityDetectionSettings
    {
        private VoiceActivityDetectionSettings(TurnDetectionType type)
        {
            Type = type;
            DetectionThreshold = null;
            PrefixPadding = null;
            SilenceDuration = null;
            CreateResponse = false;
        }

        public VoiceActivityDetectionSettings(
            [JsonProperty("type")] TurnDetectionType type = TurnDetectionType.Server_VAD,
            [JsonProperty("threshold")] float? detectionThreshold = null,
            [JsonProperty("prefix_padding_ms")] int? prefixPadding = null,
            [JsonProperty("silence_duration_ms")] int? silenceDuration = null,
            [JsonProperty("create_response")] bool createResponse = true)
        {
            switch (type)
            {
                default:
                case TurnDetectionType.Server_VAD:
                    Type = TurnDetectionType.Server_VAD;
                    DetectionThreshold = detectionThreshold;
                    PrefixPadding = prefixPadding;
                    SilenceDuration = silenceDuration;
                    CreateResponse = createResponse;
                    break;
                case TurnDetectionType.Disabled:
                    Type = TurnDetectionType.Disabled;
                    DetectionThreshold = null;
                    PrefixPadding = null;
                    SilenceDuration = null;
                    CreateResponse = false;
                    break;
            }
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TurnDetectionType Type { get; private set; }

        [Preserve]
        [JsonProperty("create_response", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool CreateResponse { get; private set; }

        [JsonProperty("interrupt_response", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool InterruptResponse { get; private set; }

        [Preserve]
        [JsonProperty("prefix_padding_ms", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? PrefixPadding { get; private set; }

        [Preserve]
        [JsonProperty("silence_duration_ms", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? SilenceDuration { get; private set; }

        [Preserve]
        [JsonProperty("threshold", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public float? DetectionThreshold { get; private set; }

        [Preserve]
        public static VoiceActivityDetectionSettings Disabled() => new(TurnDetectionType.Disabled);
    }
}
