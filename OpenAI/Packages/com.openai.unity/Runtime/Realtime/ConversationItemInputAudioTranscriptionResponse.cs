// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ConversationItemInputAudioTranscriptionResponse : BaseRealtimeEvent, IServerEvent
    {
        [Preserve]
        [JsonConstructor]
        internal ConversationItemInputAudioTranscriptionResponse(
            [JsonProperty("event_id")] string eventId,
            [JsonProperty("type")] string type,
            [JsonProperty("item_id")] string itemId,
            [JsonProperty("content_index")] int contentIndex,
            [JsonProperty("transcription")] string transcription,
            [JsonProperty("error")] Error error)
        {
            EventId = eventId;
            Type = type;
            ItemId = itemId;
            ContentIndex = contentIndex;
            Transcription = transcription;
            Error = error;
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
        /// The ID of the user message item.
        /// </summary>
        [Preserve]
        [JsonProperty("item_id")]
        public string ItemId { get; }

        /// <summary>
        /// The index of the content part containing the audio.
        /// </summary>
        [Preserve]
        [JsonProperty("content_index")]
        public int ContentIndex { get; }

        /// <summary>
        /// The transcribed text.
        /// </summary>
        [Preserve]
        [JsonProperty("transcription")]
        public string Transcription { get; }

        /// <summary>
        /// Details of the transcription error.
        /// </summary>
        [Preserve]
        public Error Error { get; }
    }
}
