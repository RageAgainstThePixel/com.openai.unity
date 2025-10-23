// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Batch
{
    [Preserve]
    public sealed class BatchOutputFolder
    {
        [JsonConstructor]
        public BatchOutputFolder([JsonProperty("url")] string url)
        {
            Url = url;
        }

        [Preserve]
        [JsonProperty("url")]
        public string Url { get; }

        [Preserve]
        public static implicit operator BatchOutputFolder(string url) => new(url);
    }
}
