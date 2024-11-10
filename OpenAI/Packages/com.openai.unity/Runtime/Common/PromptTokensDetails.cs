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
            [JsonProperty("audio_tokens")] int? audioTokens,
            [JsonProperty("cached_tokens")] int? cachedTokens)
        {
            AudioTokens = audioTokens;
            CachedTokens = cachedTokens;
        }

        [Preserve]
        [JsonProperty("audio_tokens")]
        public int? AudioTokens { get; }

        [Preserve]
        [JsonProperty("cached_tokens")]
        public int? CachedTokens { get; }

        [Preserve]
        public static PromptTokensDetails operator +(PromptTokensDetails a, PromptTokensDetails b)
            => new(
                (a?.AudioTokens ?? 0) + (b?.AudioTokens ?? 0),
                (a?.CachedTokens ?? 0) + (b?.CachedTokens ?? 0));
    }
}
