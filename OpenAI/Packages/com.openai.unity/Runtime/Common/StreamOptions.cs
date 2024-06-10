// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class StreamOptions
    {
        [Preserve]
        [JsonProperty("include_usage")]
        public bool IncludeUsage { get; } = true;
    }
}
