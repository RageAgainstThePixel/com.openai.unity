// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
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
            [JsonProperty("content_index")] int? contentIndex,
            [JsonProperty("item_id")] string itemId,
            [JsonProperty("logprobs")] List<LogProbInfo> logProbs,
            [JsonProperty("transcript")] string transcript,
            [JsonProperty("usage")] object usage,
            [JsonProperty("error")] Error error)
        {
            EventId = eventId;
            Type = type;
            ContentIndex = contentIndex;
            ItemId = itemId;
            LogProbs = logProbs;
            Transcript = transcript;
            Usage = usage;
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
        /// The index of the content part containing the audio.
        /// </summary>
        [Preserve]
        [JsonProperty("content_index")]
        public int? ContentIndex { get; }

        /// <summary>
        /// The ID of the user message item.
        /// </summary>
        [Preserve]
        [JsonProperty("item_id")]
        public string ItemId { get; }

        /// <summary>
        /// The log probabilities of the transcription.
        /// </summary>
        [Preserve]
        [JsonProperty("logprobs")]
        public IReadOnlyList<LogProbInfo> LogProbs { get; }

        /// <summary>
        /// The transcribed text.
        /// </summary>
        [Preserve]
        [JsonProperty("transcript")]
        public string Transcript { get; }

        [Preserve]
        [JsonProperty("usage")]
        public object Usage { get; }

        /// <summary>
        /// Details of the transcription error.
        /// </summary>
        [Preserve]
        [JsonProperty("error")]
        public Error Error { get; }

        [Preserve]
        [JsonIgnore]
        public bool IsCompleted => Type.Contains("completed");

        [Preserve]
        [JsonIgnore]
        public bool IsFailed => Type.Contains("failed");

        [Preserve]
        public string PrintUsage()
            => Usage?.ToString() ?? "";
    }
}
