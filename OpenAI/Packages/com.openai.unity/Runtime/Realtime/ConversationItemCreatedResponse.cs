// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ConversationItemCreatedResponse : BaseRealtimeEventResponse, IServerEvent
    {
        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; private set; }

        /// <summary>
        /// The event type, must be "conversation.item.created".
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public string Type { get; private set; }

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
        public ConversationItem Item { get; private set; }
    }
}
