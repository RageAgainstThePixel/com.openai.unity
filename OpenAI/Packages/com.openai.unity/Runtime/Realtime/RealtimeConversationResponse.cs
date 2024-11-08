// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class RealtimeConversationResponse : BaseRealtimeEvent, IServerEvent
    {
        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; private set; }

        /// <summary>
        /// The event type, must be "conversation.created".
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public string Type { get; private set; }

        /// <summary>
        /// The conversation resource.
        /// </summary>
        [Preserve]
        [JsonProperty("conversation")]
        public RealtimeConversation Conversation { get; private set; }

        [Preserve]
        public static implicit operator RealtimeConversation(RealtimeConversationResponse response) => response?.Conversation;
    }
}
