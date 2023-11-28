// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class ImageUrl
    {
        [Preserve]
        [JsonConstructor]
        public ImageUrl(string url, ImageDetail detail = ImageDetail.Auto)
        {
            Url = url;
            Detail = detail;
        }

        [Preserve]
        [JsonProperty("url")]
        public string Url { get; private set; }

        [Preserve]
        [JsonProperty("detail")]
        public ImageDetail Detail { get; private set; }
    }
}
