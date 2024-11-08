// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class InputAudioBufferClearedResponse : BaseRealtimeEvent, IServerEvent
    {
        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; private set; }

        /// <summary>
        /// The event type, must be "input_audio_buffer.cleared".
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public string Type { get; private set; }
    }
}
