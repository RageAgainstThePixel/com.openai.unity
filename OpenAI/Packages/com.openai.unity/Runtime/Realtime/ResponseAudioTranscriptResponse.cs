// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ResponseAudioTranscriptResponse : BaseRealtimeEvent, IServerEvent
    {
        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; private set; }

        /// <summary>
        /// "response.audio_transcript.delta" or "response.audio_transcript.done"
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public string Type { get; private set; }

        /// <summary>
        /// The ID of the response.
        /// </summary>
        [Preserve]
        [JsonProperty("response_id")]
        public string ResponseId { get; private set; }

        /// <summary>
        /// The ID of the item.
        /// </summary>
        [Preserve]
        [JsonProperty("item_id")]
        public string ItemId { get; private set; }

        /// <summary>
        /// The index of the output item in the response.
        /// </summary>
        [Preserve]
        [JsonProperty("output_index")]
        public string OutputIndex { get; private set; }

        /// <summary>
        /// The index of the content part in the item's content array.
        /// </summary>
        [Preserve]
        [JsonProperty("content_index")]
        public string ContentIndex { get; private set; }

        /// <summary>
        /// The transcript delta.
        /// </summary>
        [Preserve]
        [JsonProperty("delta")]
        public string Delta { get; private set; }

        /// <summary>
        /// The final transcript of the audio.
        /// </summary>
        [Preserve]
        [JsonProperty("transcript")]
        public string Transcript { get; private set; }

        [Preserve]
        public override string ToString()
            => !string.IsNullOrWhiteSpace(Delta) ? Delta : Transcript;

        [Preserve]
        public static implicit operator string(ResponseAudioTranscriptResponse response)
            => response?.ToString();
    }
}
