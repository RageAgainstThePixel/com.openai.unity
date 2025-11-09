using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class SemanticVAD : IVoiceActivityDetectionSettings
    {
        public SemanticVAD(bool createResponse = true, bool interruptResponse = true, VAD_Eagerness eagerness = VAD_Eagerness.Auto)
        {
            CreateResponse = createResponse;
            InterruptResponse = interruptResponse;
            Eagerness = eagerness;
        }

        [JsonConstructor]
        internal SemanticVAD(
            [JsonProperty("type")] TurnDetectionType type,
            [JsonProperty("create_response")] bool? createResponse,
            [JsonProperty("interrupt_response")] bool? interruptResponse,
            [JsonProperty("eagerness")] VAD_Eagerness eagerness)
        {
            Type = type;
            CreateResponse = createResponse;
            InterruptResponse = interruptResponse;
            Eagerness = eagerness;
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TurnDetectionType Type { get; private set; } = TurnDetectionType.Semantic_VAD;

        [Preserve]
        [JsonProperty("create_response", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CreateResponse { get; private set; }

        [Preserve]
        [JsonProperty("interrupt_response", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? InterruptResponse { get; private set; }

        [Preserve]
        [JsonProperty("eagerness", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public VAD_Eagerness Eagerness { get; private set; }
    }
}
