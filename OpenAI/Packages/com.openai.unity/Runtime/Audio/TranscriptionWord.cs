using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Audio
{
    /// <summary>
    /// Extracted word and their corresponding timestamps.
    /// </summary>
    [Preserve]
    public sealed class TranscriptionWord
    {
        [Preserve]
        [JsonConstructor]
        public TranscriptionWord(
            [JsonProperty("word")] string word,
            [JsonProperty("start")] double start,
            [JsonProperty("end")] double end)
        {
            Word = word;
            Start = start;
            End = end;
        }

        /// <summary>
        /// The text content of the word.
        /// </summary>
        [Preserve]
        [JsonProperty("word")]
        public string Word { get; }

        /// <summary>
        /// Start time of the word in seconds.
        /// </summary>
        [Preserve]
        [JsonProperty("start")]
        public double Start { get; }

        /// <summary>
        /// End time of the word in seconds.
        /// </summary>
        [Preserve]
        [JsonProperty("end")]
        public double End { get; }
    }
}
