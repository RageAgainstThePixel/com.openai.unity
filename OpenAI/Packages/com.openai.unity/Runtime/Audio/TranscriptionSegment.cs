using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Audio
{
    [Preserve]
    public sealed class TranscriptionSegment
    {
        [Preserve]
        [JsonConstructor]
        public TranscriptionSegment(
            [JsonProperty("id")] int id,
            [JsonProperty("seek")] int seek,
            [JsonProperty("start")] double start,
            [JsonProperty("end")] double end,
            [JsonProperty("text")] string text,
            [JsonProperty("tokens")] int[] tokens,
            [JsonProperty("temperature")] double temperature,
            [JsonProperty("avg_logprob")] double averageLogProbability,
            [JsonProperty("compression_ratio")] double compressionRatio,
            [JsonProperty("no_speech_prob")] double noSpeechProbability)
        {
            Id = id;
            Seek = seek;
            Start = start;
            End = end;
            Text = text;
            Tokens = tokens;
            Temperature = temperature;
            AverageLogProbability = averageLogProbability;
            CompressionRatio = compressionRatio;
            NoSpeechProbability = noSpeechProbability;
        }

        /// <summary>
        /// Unique identifier of the segment.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public int Id { get; }

        /// <summary>
        /// Seek offset of the segment.
        /// </summary>
        [Preserve]
        [JsonProperty("seek")]
        public int Seek { get; }

        /// <summary>
        /// Start time of the segment in seconds.
        /// </summary>
        [Preserve]
        [JsonProperty("start")]
        public double Start { get; }

        /// <summary>
        /// End time of the segment in seconds.
        /// </summary>
        [Preserve]
        [JsonProperty("end")]
        public double End { get; }

        /// <summary>
        /// Text content of the segment.
        /// </summary>
        [Preserve]
        [JsonProperty("text")]
        public string Text { get; }

        /// <summary>
        /// Array of token IDs for the text content.
        /// </summary>
        [Preserve]
        [JsonProperty("tokens")]
        public int[] Tokens { get; }

        /// <summary>
        /// Temperature parameter used for generating the segment.
        /// </summary>
        [Preserve]
        [JsonProperty("temperature")]
        public double Temperature { get; }

        /// <summary>
        /// Average logprob of the segment.
        /// If the value is lower than -1, consider the logprobs failed.
        /// </summary>
        [Preserve]
        [JsonProperty("avg_logprob")]
        public double AverageLogProbability { get; }

        /// <summary>
        /// Compression ratio of the segment.
        /// If the value is greater than 2.4, consider the compression failed.
        /// </summary>
        [Preserve]
        [JsonProperty("compression_ratio")]
        public double CompressionRatio { get; }

        /// <summary>
        /// Probability of no speech in the segment.
        /// If the value is higher than 1.0 and the avg_logprob is below -1, consider this segment silent.
        /// </summary>
        [Preserve]
        [JsonProperty("no_speech_prob")]
        public double NoSpeechProbability { get; }
    }
}