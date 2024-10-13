// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class SessionResponse : BaseRealtimeEventResponse, IRealtimeEvent
    {
        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; private set; }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; private set; }

        /// <summary>
        /// The session resource.
        /// </summary>
        [Preserve]
        [JsonProperty("session")]
        public SessionResource Session { get; private set; }
    }
}
