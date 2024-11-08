// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class RealtimeConversationResponse : BaseRealtimeEvent, IServerEvent
    {
        [Preserve]
        [JsonConstructor]
        internal RealtimeConversationResponse(
            [JsonProperty("event_id")] string eventId,
            [JsonProperty("type")] string type,
            [JsonProperty("conversation")] RealtimeConversation conversation)
        {
            EventId = eventId;
            Type = type;
            Conversation = conversation;
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
        /// The conversation resource.
        /// </summary>
        [Preserve]
        [JsonProperty("conversation")]
        public RealtimeConversation Conversation { get; }

        [Preserve]
        public static implicit operator RealtimeConversation(RealtimeConversationResponse response) => response?.Conversation;
    }
}
