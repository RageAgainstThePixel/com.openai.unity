// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Chat
{
    /// <summary>
    /// Contains log probability information.
    /// </summary>
    public sealed class LogProbInfo
    {
        [JsonConstructor]
        public LogProbInfo(
            [JsonProperty("token")] string token,
            [JsonProperty("logprob")] float logProb,
            [JsonProperty("bytes")] int[] bytes,
            [JsonProperty("top_logprobs")] List<LogProbInfo> topLogProbs)
        {
            Token = token;
            LogProb = logProb;
            Bytes = bytes;
            TopLogProbs = topLogProbs;
        }

        /// <summary>
        /// The token.
        /// </summary>
        [Preserve]
        [JsonProperty("token")]
        public string Token { get; }

        /// <summary>
        /// The log probability of this token.
        /// </summary>
        [Preserve]
        [JsonProperty("logprob")]
        public float LogProb { get; }

        /// <summary>
        /// A list of integers representing the UTF-8 bytes representation of the token.
        /// Useful in instances where characters are represented by multiple tokens and their byte
        /// representations must be combined to generate the correct text representation.
        /// Can be null if there is no bytes representation for the token.
        /// </summary>
        [Preserve]
        [JsonProperty("bytes")]
        public int[] Bytes { get; }

        /// <summary>
        /// List of the most likely tokens and their log probability, at this token position.
        /// In rare cases, there may be fewer than the number of requested top_logprobs returned.
        /// </summary>
        [Preserve]
        [JsonProperty("top_logprobs")]
        public IReadOnlyList<LogProbInfo> TopLogProbs { get; }
    }
}
