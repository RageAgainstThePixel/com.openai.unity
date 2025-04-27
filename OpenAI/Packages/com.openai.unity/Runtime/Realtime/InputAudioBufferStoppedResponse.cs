// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    /// <summary>
    /// Returned in server_vad mode when the server detects the end of speech in the audio buffer.
    /// The server will also send an conversation.item.created event with the user message item that is created from the audio buffer.
    /// </summary>
    [Preserve]
    public sealed class InputAudioBufferStoppedResponse : BaseRealtimeEvent, IServerEvent
    {
        [Preserve]
        [JsonConstructor]
        internal InputAudioBufferStoppedResponse(
            [JsonProperty("event_id")] string eventId,
            [JsonProperty("type")] string type,
            [JsonProperty("audio_end_ms")] int audioEndMs,
            [JsonProperty("item_id")] string itemId)
        {
            EventId = eventId;
            Type = type;
            AudioEndMs = audioEndMs;
            ItemId = itemId;
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
        /// Milliseconds since the session started when speech stopped.
        /// </summary>
        [Preserve]
        [JsonProperty("audio_end_ms")]
        public int AudioEndMs { get; }

        /// <summary>
        /// The ID of the user message item that will be created.
        /// </summary>
        [Preserve]
        [JsonProperty("item_id")]
        public string ItemId { get; }
    }
}
