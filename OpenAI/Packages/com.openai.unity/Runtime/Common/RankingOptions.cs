// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI
{
    /// <summary>
    /// The ranking options for the file search.
    /// <see href="https://platform.openai.com/docs/assistants/tools/file-search/customizing-file-search-settings"/>
    /// </summary>
    [Preserve]
    public sealed class RankingOptions
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ranker">
        /// The ranker to use for the file search.
        /// If not specified will use the `auto` ranker.
        /// </param>
        /// <param name="scoreThreshold">
        /// The score threshold for the file search.
        /// All values must be a floating point number between 0 and 1.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [JsonConstructor]
        public RankingOptions(
            [JsonProperty("ranker")] string ranker = "auto",
            [JsonProperty("score_threshold")] float scoreThreshold = 0f)
        {
            Ranker = ranker;
            ScoreThreshold = scoreThreshold switch
            {
                < 0 => throw new ArgumentOutOfRangeException(nameof(scoreThreshold), "Score threshold must be greater than or equal to 0."),
                > 1 => throw new ArgumentOutOfRangeException(nameof(scoreThreshold), "Score threshold must be less than or equal to 1."),
                _ => scoreThreshold
            };
        }

        /// <summary>
        /// The ranker to use for the file search.
        /// </summary>
        [Preserve]
        [JsonProperty("ranker")]
        public string Ranker { get; }

        /// <summary>
        /// The score threshold for the file search.
        /// </summary>
        [Preserve]
        [JsonProperty("score_threshold", DefaultValueHandling = DefaultValueHandling.Include)]
        public float ScoreThreshold { get; }
    }
}
