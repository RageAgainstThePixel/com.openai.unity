// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenAI
{
    /// <summary>
    /// Represents a response from the <see cref="SearchEndpoint"/>.
    /// </summary>
    internal sealed class SearchResponse : BaseResponse
    {
        /// <summary>
        /// The list of results
        /// </summary>
        [JsonProperty("data")]
        public List<SearchResult> Results { get; set; }
    }
}
