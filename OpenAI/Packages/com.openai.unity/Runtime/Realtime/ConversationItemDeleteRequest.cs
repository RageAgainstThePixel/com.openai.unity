// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    /// <summary>
    /// Send this event when you want to remove any item from the conversation history.
    /// The server will respond with a conversation.item.deleted event,
    /// unless the item does not exist in the conversation history,
    /// in which case the server will respond with an error.
    /// </summary>
    [Preserve]
    public sealed class ConversationItemDeleteRequest : BaseRealtimeEvent, IClientEvent
    {
        [Preserve]
        public ConversationItemDeleteRequest(string itemId)
        {
            ItemId = itemId;
        }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public override string EventId { get; internal set; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("type")]
        public override string Type { get; } = "conversation.item.delete";

        /// <summary>
        /// The ID of the item to delete.
        /// </summary>
        [Preserve]
        [JsonProperty("item_id")]
        public string ItemId { get; }
    }
}
