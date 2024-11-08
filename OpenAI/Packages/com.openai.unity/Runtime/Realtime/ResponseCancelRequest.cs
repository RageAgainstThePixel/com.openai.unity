// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    /// <summary>
    /// Send this event to cancel an in-progress response.
    /// The server will respond with a response.cancelled event or an error if there is no response to cancel.
    /// </summary>
    [Preserve]
    public sealed class ResponseCancelRequest : BaseRealtimeEvent, IClientEvent
    {
        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; } = "response.cancel";
    }
}
