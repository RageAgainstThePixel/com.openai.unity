// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ResponseTextResponse : BaseRealtimeEvent, IServerEvent
    {
        [Preserve]
        [JsonConstructor]
        internal ResponseTextResponse(
            [JsonProperty("event_id")] string eventId,
            [JsonProperty("type")] string type,
            [JsonProperty("response_id")] string responseId,
            [JsonProperty("item_id")] string itemId,
            [JsonProperty("output_index")] string outputIndex,
            [JsonProperty("content_index")] string contentIndex,
            [JsonProperty("delta")] string delta,
            [JsonProperty("text")] string text)
        {
            EventId = eventId;
            Type = type;
            ResponseId = responseId;
            ItemId = itemId;
            OutputIndex = outputIndex;
            ContentIndex = contentIndex;
            Delta = delta;
            Text = text;
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
        /// The ID of the response.
        /// </summary>
        [Preserve]
        [JsonProperty("response_id")]
        public string ResponseId { get; }

        /// <summary>
        /// The ID of the item.
        /// </summary>
        [Preserve]
        [JsonProperty("item_id")]
        public string ItemId { get; }

        /// <summary>
        /// The index of the output item in the response.
        /// </summary>
        [Preserve]
        [JsonProperty("output_index")]
        public string OutputIndex { get; }

        /// <summary>
        /// The index of the content part in the item's content array.
        /// </summary>
        [Preserve]
        [JsonProperty("content_index")]
        public string ContentIndex { get; }

        /// <summary>
        /// The text delta.
        /// </summary>
        [Preserve]
        [JsonProperty("delta")]
        public string Delta { get; }

        /// <summary>
        /// The final text content.
        /// </summary>
        [Preserve]
        [JsonProperty("text")]
        public string Text { get; }

        [Preserve]
        public override string ToString()
            => !string.IsNullOrWhiteSpace(Delta) ? Delta : Text;

        [Preserve]
        public static implicit operator string(ResponseTextResponse response)
            => response?.ToString();
    }
}
