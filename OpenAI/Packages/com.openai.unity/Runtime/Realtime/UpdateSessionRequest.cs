// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class UpdateSessionRequest : BaseRealtimeEventResponse, IClientEvent
    {
        [Preserve]
        public UpdateSessionRequest(SessionResource options)
        {
            Session = options;
        }

        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; } = "session.update";

        /// <summary>
        /// The session resource.
        /// </summary>
        [Preserve]
        [JsonProperty("session")]
        public SessionResource Session { get; }
    }
}
