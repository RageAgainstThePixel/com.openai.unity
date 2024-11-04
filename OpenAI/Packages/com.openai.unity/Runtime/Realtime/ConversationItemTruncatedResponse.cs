// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ConversationItemTruncatedResponse : BaseRealtimeEventResponse, IServerEvent
    {
        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; private set; }

        /// <summary>
        /// The event type, must be "conversation.item.truncated".
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public string Type { get; private set; }

        /// <summary>
        /// The ID of the assistant message item that was truncated.
        /// </summary>
        [Preserve]
        [JsonProperty("item_id")]
        public string ItemId { get; private set; }

        /// <summary>
        /// The index of the content part that was truncated.
        /// </summary>
        [Preserve]
        [JsonProperty("content_index")]
        public int ContentIndex { get; private set; }

        /// <summary>
        /// The duration up to which the audio was truncated, in milliseconds.
        /// </summary>
        [Preserve]
        [JsonProperty("audio_end_ms")]
        public int AudioEndMs { get; private set; }
    }
}
