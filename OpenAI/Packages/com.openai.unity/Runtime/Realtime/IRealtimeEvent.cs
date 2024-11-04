// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public interface IRealtimeEvent
    {
        /// <summary>
        /// The unique ID of the server event.
        /// </summary>
        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; }

        public string ToJsonString();
    }
}
