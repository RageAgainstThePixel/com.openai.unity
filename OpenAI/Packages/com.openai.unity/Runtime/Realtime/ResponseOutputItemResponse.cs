// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ResponseOutputItemResponse : BaseRealtimeEventResponse, IServerEvent
    {
        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; private set; }

        /// <summary>
        /// The event type, "response.output_item.added" or "response.output_item.done".
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public string Type { get; private set; }

        /// <summary>
        /// The ID of the response to which the item belongs.
        /// </summary>
        [Preserve]
        [JsonProperty("response_id")]
        public string ResponseId { get; private set; }

        /// <summary>
        /// The index of the output item in the response.
        /// </summary>
        [Preserve]
        [JsonProperty("output_index")]
        public string OutputIndex { get; private set; }

        /// <summary>
        /// The item that was added.
        /// </summary>
        [Preserve]
        [JsonProperty("item")]
        public ConversationItem Item { get; private set; }
    }
}
