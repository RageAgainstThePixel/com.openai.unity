// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    /// <summary>
    /// Send this event to update the session’s default configuration.
    /// The client may send this event at any time to update the session configuration,
    /// and any field may be updated at any time, except for "voice".
    /// The server will respond with a session.updated event that shows the full effective configuration.
    /// Only fields that are present are updated, thus the correct way to clear a field like "instructions" is to pass an empty string.
    /// </summary>
    [Preserve]
    public sealed class UpdateSessionRequest : BaseRealtimeEvent, IClientEvent
    {
        [Preserve]
        public UpdateSessionRequest(SessionConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public override string EventId { get; internal set; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("type")]
        public override string Type { get; } = "session.update";

        /// <summary>
        /// The session configuration configuration.
        /// </summary>
        [Preserve]
        [JsonProperty("session")]
        public SessionConfiguration Configuration { get; }

        [JsonIgnore]
        [Obsolete("use UpdateSessionRequest.Configuration")]
        public SessionConfiguration Session => Configuration;
    }
}
