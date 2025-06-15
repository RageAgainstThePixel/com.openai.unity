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
            [JsonProperty("cached_tokens")] int? cachedTokens = null,
            [JsonProperty("text_tokens")] int? textTokens = null,
            [JsonProperty("audio_tokens")] int? audioTokens = null,
            [JsonProperty("image_tokens")] int? imageTokens = null,
            [JsonProperty("reasoning_tokens")] int? reasoningToken = null)
        {
            CachedTokens = cachedTokens;
            TextTokens = textTokens;
            AudioTokens = audioTokens;
            ImageTokens = imageTokens;
            ReasoningToken = reasoningToken;
        }

        /// <summary>
        /// The number of cached tokens used in the Response.
        /// </summary>
        [Preserve]
        [JsonProperty("cached_tokens", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? CachedTokens { get; }

        /// <summary>
        /// The number of text tokens used in the Response.
        /// </summary>
        [Preserve]
        [JsonProperty("text_tokens", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? TextTokens { get; }

        /// <summary>
        /// The number of audio tokens used in the Response.
        /// </summary>
        [Preserve]
        [JsonProperty("audio_tokens", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? AudioTokens { get; }

        /// <summary>
        /// The number of image tokens used in the Response.
        /// </summary>
        [Preserve]
        [JsonProperty("image_tokens", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ImageTokens { get; }

        /// <summary>
        /// The number of reasoning tokens.
        /// </summary>
        [Preserve]
        [JsonProperty("reasoning_tokens", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ReasoningToken { get; }

        [Preserve]
        public override string ToString()
            => JsonConvert.SerializeObject(this, Formatting.Indented, OpenAIClient.JsonSerializationOptions);
    }
}
