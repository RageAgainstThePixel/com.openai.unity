// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class RateLimitsResponse : BaseRealtimeEvent, IServerEvent
    {
        [Preserve]
        [JsonConstructor]
        internal RateLimitsResponse(
            [JsonProperty("event_id")] string eventId,
            [JsonProperty("type")] string type,
            [JsonProperty("rate_limits")] IReadOnlyList<RateLimit> rateLimits)
        {
            EventId = eventId;
            Type = type;
            RateLimits = rateLimits;
        }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public override string EventId { get; internal set; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("type")]
        public override string Type { get; }

        /// <summary>
        /// List of rate limit information.
        /// </summary>
        [Preserve]
        [JsonProperty("rate_limits")]
        public IReadOnlyList<RateLimit> RateLimits { get; }
    }
}
