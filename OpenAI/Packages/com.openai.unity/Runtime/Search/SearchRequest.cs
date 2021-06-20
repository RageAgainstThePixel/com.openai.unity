// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace OpenAI
{
    ///<summary>
    /// Represents a request to the <see cref="SearchEndpoint"/>.
    /// </summary>
    internal sealed class SearchRequest
    {
        [JsonProperty("query")]
        public string Query { get; }

        [JsonProperty("documents")]
        public IReadOnlyList<string> Documents { get; }

        public SearchRequest(string query, IEnumerable<string> documents)
        {
            Query = query;
            Documents = documents?.ToList() ?? new List<string>();
        }
    }
}
