// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Images
{
    [Preserve]
    internal class ImageResult
    {
        [Preserve]
        [JsonConstructor]
        public ImageResult(
            [JsonProperty("url")] string url,
            [JsonProperty("b64_json")] string b64_json)
        {
            Url = url;
            B64_Json = b64_json;
        }

        [Preserve]
        [JsonProperty("url")]
        public string Url { get; }

        [Preserve]
        [JsonProperty("b64_json")]
        public string B64_Json { get; private set; }
    }
}
