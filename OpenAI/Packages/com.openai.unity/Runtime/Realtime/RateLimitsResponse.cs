// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class RateLimitsResponse : BaseRealtimeEvent, IServerEvent
    {
        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; private set; }

        /// <summary>
        /// The event type, must be "rate_limits.updated".
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public string Type { get; private set; }

        /// <summary>
        /// List of rate limit information.
        /// </summary>
        [Preserve]
        [JsonProperty("rate_limits")]
        public IReadOnlyList<RateLimit> RateLimits { get; private set; }
    }
}
