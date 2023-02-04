// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Images
{
    internal class ImageResult
    {
        [JsonConstructor]
        public ImageResult(string url)
        {
            Url = url;
        }

        [JsonProperty("url")]
        public string Url { get; }
    }
}
