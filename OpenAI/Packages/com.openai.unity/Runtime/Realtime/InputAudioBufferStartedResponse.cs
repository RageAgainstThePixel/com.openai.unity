// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    /// <summary>
    /// Sent by the server when in server_vad mode to indicate that speech has been detected in the audio buffer.
    /// This can happen any time audio is added to the buffer (unless speech is already detected).
    /// The client may want to use this event to interrupt audio playback or provide visual feedback to the user.
    /// The client should expect to receive a input_audio_buffer.speech_stopped event when speech stops.
    /// The item_id property is the ID of the user message item that will be created when speech stops and
    /// will also be included in the input_audio_buffer.speech_stopped event (unless the client manually commits the audio buffer during VAD activation).
    /// </summary>
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
