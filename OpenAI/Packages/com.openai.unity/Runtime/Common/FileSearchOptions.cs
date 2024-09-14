// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class FileSearchOptions
    {
        [Preserve]
        [JsonConstructor]
        public FileSearchOptions(
            [JsonProperty("max_num_results")] int maxNumberOfResults,
            [JsonProperty("ranking_options")] RankingOptions rankingOptions = null)
        {
            MaxNumberOfResults = maxNumberOfResults switch
            {
                < 1 => throw new ArgumentOutOfRangeException(nameof(maxNumberOfResults), "Max number of results must be greater than 0."),
                > 50 => throw new ArgumentOutOfRangeException(nameof(maxNumberOfResults), "Max number of results must be less than 50."),
                _ => maxNumberOfResults
            };
            RankingOptions = rankingOptions ?? new RankingOptions();
        }

        [Preserve]
        [JsonProperty("max_num_results")]
        public int MaxNumberOfResults { get; }

        [Preserve]
        [JsonProperty("ranking_options")]
        public RankingOptions RankingOptions { get; }
    }
}
