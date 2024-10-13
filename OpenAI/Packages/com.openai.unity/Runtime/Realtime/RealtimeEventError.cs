// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class RealtimeEventError : BaseRealtimeEventResponse, IRealtimeEvent
    {
        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; private set; }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; private set; }

        [Preserve]
        [JsonProperty("error")]
        public Error Error { get; private set; }
    }
}
