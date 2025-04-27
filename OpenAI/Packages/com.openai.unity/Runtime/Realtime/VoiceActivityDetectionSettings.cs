// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public interface IVoiceActivityDetectionSettings
    {
        public TurnDetectionType Type { get; }
        public bool CreateResponse { get; }
        public bool InterruptResponse { get; }
    }

    [Preserve]
    public sealed class DisabledVAD : IVoiceActivityDetectionSettings
    {
        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TurnDetectionType Type { get; } = TurnDetectionType.Disabled;

        [JsonProperty("create_response", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool CreateResponse { get; }

        [JsonProperty("interrupt_response", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool InterruptResponse { get; }
    }

    [Preserve]
    public enum VAD_Eagerness
    {
        [JsonProperty("auto")]
        Auto = 0,
        [JsonProperty("low")]
        Low,
        [JsonProperty("medium")]
        Medium,
        [JsonProperty("high")]
        High,
    }

    [Preserve]
    public sealed class SemanticVAD : IVoiceActivityDetectionSettings
    {
        public SemanticVAD(bool createResponse = true, bool interruptResponse = true, VAD_Eagerness eagerness = VAD_Eagerness.Auto)
        {
            CreateResponse = createResponse;
            InterruptResponse = interruptResponse;
            Eagerness = eagerness;
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TurnDetectionType Type { get; } = TurnDetectionType.Semantic_VAD;

        [JsonProperty("create_response", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool CreateResponse { get; private set; }

        [JsonProperty("interrupt_response", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool InterruptResponse { get; private set; }

        [Preserve]
        [JsonProperty("eagerness", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public VAD_Eagerness Eagerness { get; private set; }
    }

    [Preserve]
    public sealed class ServerVAD : IVoiceActivityDetectionSettings
    {
        public ServerVAD(
            bool createResponse = true,
            bool interruptResponse = true,
            int? prefixPadding = null,
            int? silenceDuration = null,
            float? detectionThreshold = null)
        {
            CreateResponse = createResponse;
            InterruptResponse = interruptResponse;
            PrefixPadding = prefixPadding;
            SilenceDuration = silenceDuration;
            DetectionThreshold = detectionThreshold;
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TurnDetectionType Type { get; } = TurnDetectionType.Server_VAD;

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
    }

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
