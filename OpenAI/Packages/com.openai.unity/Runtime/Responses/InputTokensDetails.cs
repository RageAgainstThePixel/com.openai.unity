// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    ///  A detailed breakdown of the input tokens.
    /// </summary>
    [Preserve]
    public sealed class InputTokensDetails
    {
        [Preserve]
        [JsonConstructor]
        internal InputTokensDetails([JsonProperty("cached_tokens")] int cachedTokens)
        {
            CachedTokens = cachedTokens;
        }

        /// <summary>
        /// The number of tokens that were retrieved from the cache.
        /// </summary>
        [Preserve]
        [JsonProperty("cached_tokens")]
        public int CachedTokens { get; }

        [Preserve]
        public override string ToString()
            => JsonConvert.SerializeObject(this, Formatting.Indented, OpenAIClient.JsonSerializationOptions);
    }
}
