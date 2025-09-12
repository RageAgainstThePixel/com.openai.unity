using Newtonsoft.Json;
using System;
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

        private NoiseReductionSettings(NoiseReduction type)
        {
            Type = type;
        }

        public NoiseReductionSettings(
            [JsonProperty("type")] NoiseReduction type = NoiseReduction.far_field)
        {
            switch (type)
            {
                default:
                case NoiseReduction.far_field:
                    Type = type;
                    break;
            }
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public NoiseReduction Type { get; private set; }
    }
}
