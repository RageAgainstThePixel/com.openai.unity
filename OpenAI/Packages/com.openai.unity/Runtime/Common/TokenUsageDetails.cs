// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class TokenUsageDetails
    {
        [Preserve]
        [JsonConstructor]
        internal TokenUsageDetails(
            [JsonProperty("cached_tokens")] int? cachedTokens,
            [JsonProperty("text_tokens")] int? textTokens,
            [JsonProperty("audio_tokens")] int? audioTokens,
            [JsonProperty("image_tokens")] int? imageTokens)
        {
            CachedTokens = cachedTokens;
            TextTokens = textTokens;
            AudioTokens = audioTokens;
            ImageTokens = imageTokens;
        }

        /// <summary>
        /// The number of cached tokens used in the Response.
        /// </summary>
        [Preserve]
        [JsonProperty("cached_tokens")]
        public int? CachedTokens { get; }

        /// <summary>
        /// The number of text tokens used in the Response.
        /// </summary>
        [Preserve]
        [JsonProperty("text_tokens")]
        public int? TextTokens { get; }

        /// <summary>
        /// The number of audio tokens used in the Response.
        /// </summary>
        [Preserve]
        [JsonProperty("audio_tokens")]
        public int? AudioTokens { get; }

        /// <summary>
        /// The number of image tokens used in the Response.
        /// </summary>
        [Preserve]
        [JsonProperty("image_tokens")]
        public int? ImageTokens { get; }
    }
}
