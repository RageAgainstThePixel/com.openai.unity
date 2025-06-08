// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class TokenUsage
    {
        /// <summary>
        /// The total number of tokens in the Response including input and output text and audio tokens.
        /// </summary>
        [Preserve]
        [JsonProperty("total_tokens")]
        public int? TotalTokens { get; }

        [Preserve]
        [JsonProperty("input_tokens")]
        public int? InputTokens { get; }

        [Preserve]
        [JsonProperty("output_tokens")]
        public int? OutputTokens { get; }

        [Preserve]
        [JsonProperty("input_token_details")]
        public TokenUsageDetails InputTokenDetails { get; }

        [Preserve]
        [JsonProperty("output_token_details")]
        public TokenUsageDetails OutputTokenDetails { get; }
    }
}
