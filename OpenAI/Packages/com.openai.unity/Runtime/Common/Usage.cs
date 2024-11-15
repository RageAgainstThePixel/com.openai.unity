// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class Usage
    {
        [Preserve]
        [JsonConstructor]
        internal Usage(
            [JsonProperty("completion_tokens")] int? completionTokens,
            [JsonProperty("prompt_tokens")] int? promptTokens,
            [JsonProperty("total_tokens")] int? totalTokens,
            [JsonProperty("completion_tokens_details")] CompletionTokensDetails completionTokensDetails,
            [JsonProperty("prompt_tokens_details")] PromptTokensDetails promptTokensDetails)
        {
            PromptTokens = promptTokens;
            CompletionTokens = completionTokens;
            TotalTokens = totalTokens;
            CompletionTokensDetails = completionTokensDetails;
            PromptTokensDetails = promptTokensDetails;
        }

        [Preserve]
        [JsonProperty("completion_tokens")]
        public int? CompletionTokens { get; private set; }

        [Preserve]
        [JsonProperty("prompt_tokens")]
        public int? PromptTokens { get; private set; }

        [Preserve]
        [JsonProperty("total_tokens")]
        public int? TotalTokens { get; private set; }

        [Preserve]
        [JsonProperty("completion_tokens_details")]
        public CompletionTokensDetails CompletionTokensDetails { get; private set; }

        [Preserve]
        [JsonProperty("prompt_tokens_details")]
        public PromptTokensDetails PromptTokensDetails { get; private set; }

        [Preserve]
        internal void AppendFrom(Usage other)
        {
            if (other?.PromptTokens != null)
            {
                PromptTokens = other.PromptTokens.Value;
            }

            if (other?.CompletionTokens != null)
            {
                CompletionTokens = other.CompletionTokens.Value;
            }

            if (other?.TotalTokens != null)
            {
                TotalTokens = other.TotalTokens.Value;
            }

            if (other?.CompletionTokensDetails != null)
            {
                CompletionTokensDetails = other.CompletionTokensDetails;
            }

            if (other?.PromptTokensDetails != null)
            {
                PromptTokensDetails = other.PromptTokensDetails;
            }
        }

        [Preserve]
        public override string ToString()
            => JsonConvert.SerializeObject(this, OpenAIClient.JsonSerializationOptions);

        [Preserve]
        public static Usage operator +(Usage a, Usage b)
            => new(
                (a.PromptTokens ?? 0) + (b.PromptTokens ?? 0),
                (a.CompletionTokens ?? 0) + (b.CompletionTokens ?? 0),
                (a.TotalTokens ?? 0) + (b.TotalTokens ?? 0),
                a.CompletionTokensDetails + b.CompletionTokensDetails,
                a.PromptTokensDetails + b.PromptTokensDetails);
    }
}
