// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI
{
    public sealed class Usage
    {
        [JsonConstructor]
        public Usage(
            int? promptTokens,
            int? completionTokens,
            int? totalTokens)
        {
            PromptTokens = promptTokens;
            CompletionTokens = completionTokens;
            TotalTokens = totalTokens;
        }

        [JsonProperty("prompt_tokens")]
        public int? PromptTokens { get; internal set; }

        [JsonProperty("completion_tokens")]
        public int? CompletionTokens { get; internal set; }

        [JsonProperty("total_tokens")]
        public int? TotalTokens { get; internal set; }

        internal void CopyFrom(Usage other)
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
        }

        public override string ToString() => JsonConvert.SerializeObject(this);

        public static Usage operator +(Usage a, Usage b)
            => new Usage(
                (a.PromptTokens ?? 0) + (b.PromptTokens ?? 0),
                (a.CompletionTokens ?? 0) + (b.CompletionTokens ?? 0),
                (a.TotalTokens ?? 0) + (b.TotalTokens ?? 0));
    }
}
