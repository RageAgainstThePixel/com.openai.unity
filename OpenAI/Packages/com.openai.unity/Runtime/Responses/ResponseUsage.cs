// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// Represents token usage details including input tokens, output tokens, a breakdown of output tokens, and the total tokens used.
    /// </summary>
    [Preserve]
    public sealed class ResponseUsage
    {
        [Preserve]
        [JsonConstructor]
        public ResponseUsage(
            [JsonProperty("input_tokens")] int inputTokens,
            [JsonProperty("input_tokens_details")] InputTokensDetails inputTokensDetails,
            [JsonProperty("output_tokens")] int outputTokens,
            [JsonProperty("output_tokens_details")] OutputTokensDetails outputTokensDetails,
            [JsonProperty("total_tokens")] int totalTokens)
        {
            InputTokens = inputTokens;
            InputTokensDetails = inputTokensDetails;
            OutputTokens = outputTokens;
            OutputTokensDetails = outputTokensDetails;
            TotalTokens = totalTokens;
        }

        /// <summary>
        /// The number of input tokens.
        /// </summary>
        [Preserve]
        [JsonProperty("input_tokens")]
        public int InputTokens { get; }

        /// <summary>
        ///  A detailed breakdown of the input tokens.
        /// </summary>
        [Preserve]
        [JsonProperty("input_tokens_details")]
        public InputTokensDetails InputTokensDetails { get; }

        /// <summary>
        /// The number of output tokens.
        /// </summary>
        [Preserve]
        [JsonProperty("output_tokens")]
        public int OutputTokens { get; }

        /// <summary>
        /// A detailed breakdown of the output tokens.
        /// </summary>
        [Preserve]
        [JsonProperty("output_tokens_details")]
        public OutputTokensDetails OutputTokensDetails { get; }

        /// <summary>
        /// The total number of tokens used.
        /// </summary>
        [Preserve]
        [JsonProperty("total_tokens")]
        public int TotalTokens { get; }

        [Preserve]
        public override string ToString()
            => JsonConvert.SerializeObject(this, Formatting.Indented, OpenAIClient.JsonSerializationOptions);
    }
}
