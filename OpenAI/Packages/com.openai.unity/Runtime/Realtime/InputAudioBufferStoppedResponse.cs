// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class InputAudioBufferStoppedResponse : BaseRealtimeEventResponse, IServerEvent
    {
        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; private set; }

        /// <summary>
        /// The event type, must be "input_audio_buffer.stopped".
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public string Type { get; private set; }

        /// <summary>
        /// Milliseconds since the session started when speech stopped.
        /// </summary>
        [Preserve]
        [JsonProperty("audio_end_ms")]
        public int AudioEndMs { get; private set; }

        /// <summary>
        /// The ID of the user message item that will be created.
        /// </summary>
        [Preserve]
        [JsonProperty("item_id")]
        public string ItemId { get; private set; }
    }
}
