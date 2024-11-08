// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    /// <summary>
    /// Add a new Item to the Conversation's context, including messages, function calls, and function call responses.
    /// This event can be used both to populate a "history" of the conversation and to add new items mid-stream,
    /// but has the current limitation that it cannot populate assistant audio messages.
    /// If successful, the server will respond with a conversation.item.created event, otherwise an error event will be sent.
    /// </summary>
    [Preserve]
    public sealed class ConversationItemCreateRequest : BaseRealtimeEvent, IClientEvent
    {
        [Preserve]
        public ConversationItemCreateRequest(ConversationItem item, string previousItemId = null)
        {
            PreviousItemId = previousItemId;
            Item = item;
        }

        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; } = "conversation.item.create";

        /// <summary>
        /// The ID of the preceding item after which the new item will be inserted.
        /// If not set, the new item will be appended to the end of the conversation.
        /// If set, it allows an item to be inserted mid-conversation.
        /// If the ID cannot be found, an error will be returned and the item will not be added.
        /// </summary>
        [Preserve]
        [JsonProperty("previous_item_id")]
        public string PreviousItemId { get; }

        /// <summary>
        /// The item to add to the conversation.
        /// </summary>
        [Preserve]
        [JsonProperty("item")]
        public ConversationItem Item { get; }
    }
}
