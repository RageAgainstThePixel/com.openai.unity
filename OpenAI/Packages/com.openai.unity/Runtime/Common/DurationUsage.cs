// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class DurationUsage
    {
        [Preserve]
        [JsonConstructor]
        internal DurationUsage(
            [JsonProperty("seconds")] float seconds,
            [JsonProperty("type")] string type)
        {
        }

        [Preserve]
        [JsonProperty("seconds")]
        public float Seconds { get; }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; }

        [Preserve]
        public override string ToString()
            => JsonConvert.SerializeObject(this, Formatting.Indented, OpenAIClient.JsonSerializationOptions);
    }
}
