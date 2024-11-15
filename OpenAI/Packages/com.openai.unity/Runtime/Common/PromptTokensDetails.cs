// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class PromptTokensDetails
    {
        [Preserve]
        [JsonConstructor]
        internal PromptTokensDetails(
            [JsonProperty("cached_tokens")] int? cachedTokens,
            [JsonProperty("audio_tokens")] int? audioTokens,
            [JsonProperty("text_tokens")] int? textTokens,
            [JsonProperty("image_tokens")] int? imageTokens)
        {
            AudioTokens = audioTokens;
            CachedTokens = cachedTokens;
            TextTokens = textTokens;
            ImageTokens = imageTokens;
        }

        [Preserve]
        [JsonProperty("cached_tokens")]
        public int? CachedTokens { get; }

        [Preserve]
        [JsonProperty("audio_tokens")]
        public int? AudioTokens { get; }

        [Preserve]
        [JsonProperty("text_tokens")]
        public int? TextTokens { get; }

        [Preserve]
        [JsonProperty("image_tokens")]
        public int? ImageTokens { get; }

        [Preserve]
        public static PromptTokensDetails operator +(PromptTokensDetails a, PromptTokensDetails b)
            => new(
                (a?.CachedTokens ?? 0) + (b?.CachedTokens ?? 0),
                (a?.AudioTokens ?? 0) + (b?.AudioTokens ?? 0),
                (a?.TextTokens ?? 0) + (b?.TextTokens ?? 0),
                (a?.ImageTokens ?? 0) + (b?.ImageTokens ?? 0));
    }
}
