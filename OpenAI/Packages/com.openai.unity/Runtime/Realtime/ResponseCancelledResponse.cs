// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ResponseCancelledResponse : BaseRealtimeEvent, IServerEvent
    {
        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; } = "response.cancelled";
    }
}
