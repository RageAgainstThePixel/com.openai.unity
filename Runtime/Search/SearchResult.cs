// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI
{
    /// <summary>
    /// Represents a single search result in <see cref="SearchResponse"/>.
    /// </summary>
    internal sealed class SearchResult
    {
        /// <summary>
        /// The index of the document as originally supplied
        /// </summary>
        [JsonProperty("document")]
        public int DocumentIndex { get; set; }

        /// <summary>
        /// The relative score of this document
        /// </summary>
        [JsonProperty("score")]
        public double Score { get; set; }
    }
}
