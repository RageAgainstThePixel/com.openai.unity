// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ConversationItemInputAudioTranscriptionResponse : BaseRealtimeEvent, IServerEvent
    {
        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; private set; }

        /// <summary>
        /// "conversation.item.input_audio_transcription.completed" or "conversation.item.input_audio_transcription.failed"
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public string Type { get; private set; }

        /// <summary>
        /// The ID of the user message item.
        /// </summary>
        [Preserve]
        [JsonProperty("item_id")]
        public string ItemId { get; private set; }

        /// <summary>
        /// The index of the content part containing the audio.
        /// </summary>
        [Preserve]
        [JsonProperty("content_index")]
        public int ContentIndex { get; private set; }

        /// <summary>
        /// The transcribed text.
        /// </summary>
        [Preserve]
        [JsonProperty("transcription")]
        public string Transcription { get; private set; }

        /// <summary>
        /// Details of the transcription error.
        /// </summary>
        [Preserve]
        public Error Error { get; private set; }
    }
}
