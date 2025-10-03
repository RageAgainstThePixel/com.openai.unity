// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class NoiseReductionSettings
    {
        [Preserve]
        [JsonConstructor]
        public NoiseReductionSettings([JsonProperty("type")] NoiseReduction type = NoiseReduction.NearField)
        {
            Type = type;
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public NoiseReduction Type { get; private set; }
    }
}
