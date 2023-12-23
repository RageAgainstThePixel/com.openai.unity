// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;

namespace OpenAI.Images
{
    [Preserve]
    public sealed class ImageResult
    {
        [Preserve]
        [JsonConstructor]
        public ImageResult(
            [JsonProperty("url")] string url,
            [JsonProperty("b64_json")] string b64_json,
            [JsonProperty("revised_prompt")] string revisedPrompt)
        {
            Url = url;
            B64_Json = b64_json;
            RevisedPrompt = revisedPrompt;
        }

        [Preserve]
        [JsonProperty("url")]
        public string Url { get; }

        [Preserve]
        [JsonProperty("b64_json")]
        public string B64_Json { get; private set; }

        [Preserve]
        [JsonProperty("revised_prompt")]
        public string RevisedPrompt { get; private set; }

        [Preserve]
        [JsonIgnore]
        public string CachedPath { get; internal set; }

        [Preserve]
        [JsonIgnore]
        public Texture2D Texture { get; internal set; }

        [Preserve]
        public static implicit operator Texture2D(ImageResult imageResult) => imageResult.Texture;

        [Preserve]
        public static implicit operator string(ImageResult imageResult) => imageResult.ToString();

        [Preserve]
        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(CachedPath))
            {
                return CachedPath;
            }

            if (!string.IsNullOrWhiteSpace(B64_Json))
            {
                return B64_Json;
            }

            if (!string.IsNullOrWhiteSpace(Url))
            {
                return Url;
            }

            return string.Empty;
        }
    }
}
