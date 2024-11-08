// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    /// <summary>
    /// Send this event to clear the audio bytes in the buffer.
    /// The server will respond with an input_audio_buffer.cleared event.
    /// </summary>
    [Preserve]
    public sealed class InputAudioBufferClearRequest : BaseRealtimeEvent, IClientEvent
    {
        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public override string EventId { get; internal set; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("type")]
        public override string Type { get; } = "input_audio_buffer.clear";
    }
}
