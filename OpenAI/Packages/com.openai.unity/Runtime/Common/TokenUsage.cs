// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class TokenUsage
    {
        [Preserve]
        [JsonConstructor]
        internal TokenUsage(int? totalTokens,
            int? inputTokens,
            int? outputTokens,
            TokenUsageDetails inputTokenDetails,
            TokenUsageDetails outputTokenDetails)
        {
            TotalTokens = totalTokens;
            InputTokens = inputTokens;
            OutputTokens = outputTokens;
            InputTokenDetails = inputTokenDetails;
            OutputTokenDetails = outputTokenDetails;
        }

        /// <summary>
        /// The total number of tokens in the Response including input and output text and audio tokens.
        /// </summary>
        [Preserve]
        [JsonProperty("total_tokens", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? TotalTokens { get; }

        [Preserve]
        [JsonProperty("input_tokens", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? InputTokens { get; }

        [Preserve]
        [JsonProperty("input_token_details", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TokenUsageDetails InputTokenDetails { get; }

        [Preserve]
        [JsonProperty("output_tokens", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? OutputTokens { get; }

        [Preserve]
        [JsonProperty("output_token_details", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TokenUsageDetails OutputTokenDetails { get; }

        [Preserve]
        public override string ToString()
            => JsonConvert.SerializeObject(this, Formatting.Indented, OpenAIClient.JsonSerializationOptions);
    }
}
