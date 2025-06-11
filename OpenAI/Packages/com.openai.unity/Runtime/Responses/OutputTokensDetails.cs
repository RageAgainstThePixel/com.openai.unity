// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A detailed breakdown of the output tokens.
    /// </summary>
    [Preserve]
    public sealed class OutputTokensDetails
    {
        [Preserve]
        [JsonConstructor]
        internal OutputTokensDetails([JsonProperty("reasoning_tokens")] int reasoningTokens)
        {
            ReasoningTokens = reasoningTokens;
        }

        /// <summary>
        /// The number of reasoning tokens.
        /// </summary>
        [Preserve]
        [JsonProperty("reasoning_tokens")]
        public int ReasoningTokens { get; }

        [Preserve]
        public override string ToString()
            => JsonConvert.SerializeObject(this, Formatting.Indented, OpenAIClient.JsonSerializationOptions);
    }
}
