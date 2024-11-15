// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class RateLimit
    {
        [Preserve]
        [JsonProperty("name")]
        public string Name { get; private set; }

        [Preserve]
        [JsonProperty("limit")]
        public int Limit { get; private set; }

        [Preserve]
        [JsonProperty("remaining")]
        public int Remaining { get; private set; }

        [Preserve]
        [JsonProperty("reset_seconds")]
        public int ResetSeconds { get; private set; }
    }
}
