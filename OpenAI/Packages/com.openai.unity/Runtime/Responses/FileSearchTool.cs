// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenAI.Responses
{
    /// <summary>
    /// A tool that searches for relevant content from uploaded files.
    /// </summary>
    public sealed class FileSearchTool : ITool
    {
        public static implicit operator Tool(FileSearchTool fileSearchTool) => new(fileSearchTool as ITool);

        public FileSearchTool(string vectorStoreId, int? maxNumberOfResults = null, RankingOptions rankingOptions = null, IEnumerable<IFilter> filters = null)
            : this(new[] { vectorStoreId }, maxNumberOfResults, rankingOptions, filters)
        {
        }

        public FileSearchTool(IEnumerable<string> vectorStoreIds, int? maxNumberOfResults = null, RankingOptions rankingOptions = null, IEnumerable<IFilter> filters = null)
        {
            VectorStoreIds = vectorStoreIds?.ToList() ?? throw new NullReferenceException(nameof(vectorStoreIds));
            MaxNumberOfResults = maxNumberOfResults;
            RankingOptions = rankingOptions;
            Filters = filters?.ToList();
        }

        [JsonProperty("type")]
        public string Type => "file_search";

        [JsonProperty("vector_store_ids")]
        public IReadOnlyList<string> VectorStoreIds { get; }

        [JsonProperty("max_num_results", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? MaxNumberOfResults { get; }

        [JsonProperty("ranking_options", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public RankingOptions RankingOptions { get; }

        [JsonProperty("filters", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<IFilter> Filters { get; }
    }
}
