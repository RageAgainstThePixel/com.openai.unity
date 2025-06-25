// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A tool that searches for relevant content from uploaded files.
    /// </summary>
    [Preserve]
    public sealed class FileSearchTool : ITool
    {
        [Preserve]
        public static implicit operator Tool(FileSearchTool fileSearchTool) => new(fileSearchTool as ITool);

        [Preserve]
        public FileSearchTool(string vectorStoreId, int? maxNumberOfResults = null, RankingOptions rankingOptions = null, IEnumerable<IFilter> filters = null)
            : this(new[] { vectorStoreId }, maxNumberOfResults, rankingOptions, filters)
        {
        }

        [Preserve]
        public FileSearchTool(IEnumerable<string> vectorStoreIds, int? maxNumberOfResults = null, RankingOptions rankingOptions = null, IEnumerable<IFilter> filters = null)
        {
            VectorStoreIds = vectorStoreIds?.ToList() ?? throw new NullReferenceException(nameof(vectorStoreIds));
            MaxNumberOfResults = maxNumberOfResults;
            RankingOptions = rankingOptions;
            Filters = filters?.ToList();
        }

        [Preserve]
        [JsonConstructor]
        internal FileSearchTool(
            [JsonProperty("type")] string type,
            [JsonProperty("vector_store_ids")] IReadOnlyList<string> vectorStoreIds,
            [JsonProperty("max_num_results", DefaultValueHandling = DefaultValueHandling.Ignore)] int? maxNumberOfResults,
            [JsonProperty("ranking_options", DefaultValueHandling = DefaultValueHandling.Ignore)] RankingOptions rankingOptions,
            [JsonProperty("filters", DefaultValueHandling = DefaultValueHandling.Ignore)] IReadOnlyList<IFilter> filters)
        {
            Type = type;
            VectorStoreIds = vectorStoreIds;
            MaxNumberOfResults = maxNumberOfResults;
            RankingOptions = rankingOptions;
            Filters = filters;
        }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; } = "file_search";

        [Preserve]
        [JsonProperty("vector_store_ids")]
        public IReadOnlyList<string> VectorStoreIds { get; }

        [Preserve]
        [JsonProperty("max_num_results", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? MaxNumberOfResults { get; }

        [Preserve]
        [JsonProperty("ranking_options", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public RankingOptions RankingOptions { get; }

        [Preserve]
        [JsonProperty("filters", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<IFilter> Filters { get; }
    }
}
