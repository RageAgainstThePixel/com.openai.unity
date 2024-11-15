// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ConversationItemCreatedResponse : BaseRealtimeEvent, IServerEvent
    {
        [Preserve]
        [JsonConstructor]
        internal ConversationItemCreatedResponse(
            [JsonProperty("event_id")] string eventId,
            [JsonProperty("type")] string type,
            [JsonProperty("previous_item_id")] string previousItemId,
            [JsonProperty("item")] ConversationItem item)
        {
            EventId = eventId;
            Type = type;
            PreviousItemId = previousItemId;
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
        /// The ID of the preceding item.
        /// </summary>
        [Preserve]
        [JsonProperty("previous_item_id")]
        public string PreviousItemId { get; }

        /// <summary>
        /// The item that was created.
        /// </summary>
        [Preserve]
        [JsonProperty("item")]
        public ConversationItem Item { get; }
    }
}
