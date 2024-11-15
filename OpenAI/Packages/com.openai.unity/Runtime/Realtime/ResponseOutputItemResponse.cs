// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ResponseOutputItemResponse : BaseRealtimeEvent, IServerEvent
    {
        [Preserve]
        [JsonConstructor]
        internal ResponseOutputItemResponse(
            [JsonProperty("event_id")] string eventId,
            [JsonProperty("type")] string type,
            [JsonProperty("response_id")] string responseId,
            [JsonProperty("output_index")] int outputIndex,
            [JsonProperty("item")] ConversationItem item)
        {
            EventId = eventId;
            Type = type;
            ResponseId = responseId;
            OutputIndex = outputIndex;
            Item = item;
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
        /// The ID of the response to which the item belongs.
        /// </summary>
        [Preserve]
        [JsonProperty("response_id")]
        public string ResponseId { get; }

        /// <summary>
        /// The index of the output item in the response.
        /// </summary>
        [Preserve]
        [JsonProperty("output_index")]
        public int OutputIndex { get; }

        /// <summary>
        /// The item that was added.
        /// </summary>
        [Preserve]
        [JsonProperty("item")]
        public ConversationItem Item { get; }
    }
}
