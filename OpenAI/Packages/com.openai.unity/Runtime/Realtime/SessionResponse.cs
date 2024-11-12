// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class SessionResponse : BaseRealtimeEvent, IServerEvent
    {
        [Preserve]
        [JsonConstructor]
        internal SessionResponse(
            [JsonProperty("event_id")] string eventId,
            [JsonProperty("type")] string type,
            [JsonProperty("session")] Options session)
        {
            EventId = eventId;
            Type = type;
            Session = session;
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
        /// The session resource.
        /// </summary>
        [Preserve]
        [JsonProperty("session")]
        public Options Session { get; }
    }
}
