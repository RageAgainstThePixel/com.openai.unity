// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI
{
    public sealed class Usage
    {
        [JsonConstructor]
        public Usage(
            int promptTokens,
            int completionTokens,
            int totalTokens)
        {
            PromptTokens = promptTokens;
            CompletionTokens = completionTokens;
            TotalTokens = totalTokens;
        }

        [JsonProperty("prompt_tokens")]
        public int PromptTokens { get; }

        [JsonProperty("completion_tokens")]
        public int CompletionTokens { get; }

        [JsonProperty("total_tokens")]
        public int TotalTokens { get; }
    }
}
