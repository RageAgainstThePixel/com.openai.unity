// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
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
            [JsonProperty("session")] SessionConfiguration session)
        {
            EventId = eventId;
            Type = type;
            SessionConfiguration = session;
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
        /// The session resource configuration.
        /// </summary>
        [Preserve]
        [JsonProperty("session")]
        public SessionConfiguration SessionConfiguration { get; }

        [JsonIgnore]
        [Obsolete("use SessionResponse.SessionConfiguration")]
        public SessionConfiguration Options => SessionConfiguration;
    }
}
