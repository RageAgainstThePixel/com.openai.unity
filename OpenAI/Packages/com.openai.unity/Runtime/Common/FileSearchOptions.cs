// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class FileSearchOptions
    {
        [Preserve]
        [JsonConstructor]
        public FileSearchOptions(int maxNumberOfResults)
        {
            MaxNumberOfResults = maxNumberOfResults;
        }

        [Preserve]
        [JsonProperty("max_num_results")]
        public int MaxNumberOfResults { get; }
    }
}
