// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    /// <summary>
    /// Send this event to truncate a previous assistant message’s audio.
    /// The server will produce audio faster than realtime,
    /// so this event is useful when the user interrupts to truncate audio
    /// that has already been sent to the client but not yet played.
    /// This will synchronize the server's understanding of the audio with the client's playback.
    /// Truncating audio will delete the server-side text transcript to ensure there
    /// is not text in the context that hasn't been heard by the user.
    /// If successful, the server will respond with a conversation.item.truncated event.
    /// </summary>
    [Preserve]
    public sealed class ConversationItemTruncateRequest : BaseRealtimeEvent, IClientEvent
    {
        [Preserve]
        public ConversationItemTruncateRequest(string itemId, int contentIndex, int audioEndMs)
        {
            ItemId = itemId;
            ContentIndex = contentIndex;
            AudioEndMs = audioEndMs;
        }

        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; } = "conversation.item.truncate";

        /// <summary>
        /// The ID of the assistant message item to truncate. Only assistant message items can be truncated.
        /// </summary>
        [Preserve]
        [JsonProperty("item_id")]
        public string ItemId { get; }

        /// <summary>
        /// The index of the content part to truncate. Set this to 0.
        /// </summary>
        [Preserve]
        [JsonProperty("content_index")]
        public int ContentIndex { get; }

        /// <summary>
        /// Inclusive duration up to which audio is truncated, in milliseconds.
        /// If the audio_end_ms is greater than the actual audio duration, the server will respond with an error.
        /// </summary>
        [Preserve]
        [JsonProperty("audio_end_ms")]
        public int AudioEndMs { get; }
    }
}
