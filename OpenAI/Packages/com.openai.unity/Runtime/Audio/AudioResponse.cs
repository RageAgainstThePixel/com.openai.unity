// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Audio
{
    [Preserve]
    public sealed class AudioResponse
    {
        [Preserve]
        [JsonConstructor]
        internal AudioResponse(
            [JsonProperty("language")] string language,
            [JsonProperty("duration")] double? duration,
            [JsonProperty("text")] string text,
            [JsonProperty("words")] TranscriptionWord[] words,
            [JsonProperty("segments")] TranscriptionSegment[] segments)
        {
            Language = language;
            Duration = duration;
            Text = text;
            Words = words;
            Segments = segments;
        }

        /// <summary>
        /// The language of the input audio.
        /// </summary>
        [Preserve]
        [JsonProperty("language")]
        public string Language { get; }

        /// <summary>
        /// The duration of the input audio.
        /// </summary>
        [Preserve]
        [JsonProperty("duration")]
        public double? Duration { get; }

        /// <summary>
        /// The transcribed text.
        /// </summary>
        [Preserve]
        [JsonProperty("text")]
        public string Text { get; }

        /// <summary>
        /// Extracted words and their corresponding timestamps.
        /// </summary>
        [Preserve]
        [JsonProperty("words")]
        public TranscriptionWord[] Words { get; }

        /// <summary>
        /// Segments of the transcribed text and their corresponding details.
        /// </summary>
        [Preserve]
        [JsonProperty("segments")]
        public TranscriptionSegment[] Segments { get; }
    }
}
