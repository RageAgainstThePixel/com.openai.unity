// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class InputAudioBufferStartedResponse : BaseRealtimeEvent, IServerEvent
    {
        [Preserve]
        [JsonConstructor]
        internal InputAudioBufferStartedResponse(
            [JsonProperty("event_id")] string eventId,
            [JsonProperty("type")] string type,
            [JsonProperty("audio_start_ms")] int audioStartMs,
            [JsonProperty("item_id")] string itemId)
        {
            EventId = eventId;
            Type = type;
            AudioStartMs = audioStartMs;
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
        /// Milliseconds since the session started when speech was detected.
        /// </summary>
        [Preserve]
        [JsonProperty("audio_start_ms")]
        public int AudioStartMs { get; }

        /// <summary>
        /// The ID of the user message item that will be created when speech stops.
        /// </summary>
        [Preserve]
        [JsonProperty("item_id")]
        public string ItemId { get; }
    }
}
