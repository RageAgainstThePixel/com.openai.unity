using Newtonsoft.Json;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class NoiseReductionSettings
    {
        public enum NoiseReduction
        {
            [EnumMember(Value = "near_field")]
            near_field,
            [EnumMember(Value = "far_field")]
            far_field,
        }

        [Preserve]
        [JsonConstructor]
        public NoiseReductionSettings(NoiseReduction type = NoiseReduction.near_field)
        {
            Type = type;
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public NoiseReduction Type { get; private set; }
    }
}
