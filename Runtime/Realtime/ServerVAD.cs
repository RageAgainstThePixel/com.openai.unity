using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ServerVAD : IVoiceActivityDetectionSettings
    {
        public ServerVAD(
            bool? createResponse = true,
            bool? interruptResponse = true,
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

        [JsonConstructor]
        internal ServerVAD(
            [JsonProperty("type")] TurnDetectionType type,
            [JsonProperty("create_response")] bool? createResponse,
            [JsonProperty("interrupt_response")] bool? interruptResponse,
            [JsonProperty("prefix_padding_ms")] int? prefixPadding,
            [JsonProperty("silence_duration_ms")] int? silenceDuration,
            [JsonProperty("threshold")] float? detectionThreshold)
        {
            Type = type;
            CreateResponse = createResponse;
            InterruptResponse = interruptResponse;
            PrefixPadding = prefixPadding;
            SilenceDuration = silenceDuration;
            DetectionThreshold = detectionThreshold;
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TurnDetectionType Type { get; private set; } = TurnDetectionType.Server_VAD;

        [Preserve]
        [JsonProperty("create_response", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CreateResponse { get; private set; }

        [Preserve]
        [JsonProperty("interrupt_response", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? InterruptResponse { get; private set; }

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
}
