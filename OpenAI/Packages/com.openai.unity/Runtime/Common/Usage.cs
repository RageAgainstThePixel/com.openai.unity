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
        public Usage(
            [JsonProperty("prompt_tokens")] int? promptTokens,
            [JsonProperty("completion_tokens")] int? completionTokens,
            [JsonProperty("total_tokens")] int? totalTokens)
        {
            PromptTokens = promptTokens;
            CompletionTokens = completionTokens;
            TotalTokens = totalTokens;
        }

        [Preserve]
        [JsonProperty("prompt_tokens")]
        public int? PromptTokens { get; internal set; }

        [Preserve]
        [JsonProperty("completion_tokens")]
        public int? CompletionTokens { get; internal set; }

        [Preserve]
        [JsonProperty("total_tokens")]
        public int? TotalTokens { get; internal set; }

        [Preserve]
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
