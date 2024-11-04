// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class InputAudioBufferStartedResponse : BaseRealtimeEventResponse, IServerEvent
    {
        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; private set; }

        /// <summary>
        /// The event type, must be "input_audio_buffer.started".
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public string Type { get; private set; }

        /// <summary>
        /// Milliseconds since the session started when speech was detected.
        /// </summary>
        [Preserve]
        [JsonProperty("audio_start_ms")]
        public int AudioStartMs { get; private set; }

        /// <summary>
        /// The ID of the user message item that will be created when speech stops.
        /// </summary>
        [Preserve]
        [JsonProperty("item_id")]
        public string ItemId { get; private set; }
    }
}
