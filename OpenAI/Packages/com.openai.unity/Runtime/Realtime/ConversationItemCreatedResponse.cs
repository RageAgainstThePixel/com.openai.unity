// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ConversationItemCreatedResponse : BaseRealtimeEventResponse, IRealtimeEvent
    {
        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; }

        /// <summary>
        /// The event type, must be "conversation.item.created".
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Type { get; }

        /// <summary>
        /// The ID of the preceding item.
        /// </summary>
        [Preserve]
        [JsonProperty("previous_item_id")]
        public string PreviousItemId { get; private set; }

        /// <summary>
        /// The item that was created.
        /// </summary>
        [Preserve]
        [JsonProperty("item")]
        public ConversationItem Item { get; }
    }
}
