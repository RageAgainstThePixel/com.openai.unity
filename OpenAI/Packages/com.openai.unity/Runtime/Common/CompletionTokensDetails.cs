// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class CompletionTokensDetails
    {
        [Preserve]
        [JsonConstructor]
        internal CompletionTokensDetails(
            [JsonProperty("reasoning_tokens")] int? reasoningTokens,
            [JsonProperty("audio_tokens")] int? audioTokens,
            [JsonProperty("text_tokens")] int? textTokens,
            [JsonProperty("accepted_prediction_tokens")] int? acceptedPredictionTokens,
            [JsonProperty("rejected_prediction_tokens")] int? rejectedPredictionTokens)
        {
            ReasoningTokens = reasoningTokens;
            AudioTokens = audioTokens;
            TextTokens = textTokens;
            AcceptedPredictionTokens = acceptedPredictionTokens;
            RejectedPredictionTokens = rejectedPredictionTokens;
        }

        [Preserve]
        [JsonProperty("reasoning_tokens")]
        public int? ReasoningTokens { get; }

        [Preserve]
        [JsonProperty("audio_tokens")]
        public int? AudioTokens { get; }

        [Preserve]
        [JsonProperty("text_tokens")]
        public int? TextTokens { get; }

        [Preserve]
        [JsonProperty("accepted_prediction_tokens")]
        public int? AcceptedPredictionTokens { get; }

        [Preserve]
        [JsonProperty("rejected_prediction_tokens")]
        public int? RejectedPredictionTokens { get; }

        [Preserve]
        public static CompletionTokensDetails operator +(CompletionTokensDetails a, CompletionTokensDetails b)
            => new(
                (a?.ReasoningTokens ?? 0) + (b?.ReasoningTokens ?? 0),
                (a?.AudioTokens ?? 0) + (b?.AudioTokens ?? 0),
                (a?.TextTokens ?? 0) + (b?.TextTokens ?? 0),
                (a?.AcceptedPredictionTokens ?? 0) + (b?.AcceptedPredictionTokens ?? 0),
                (a?.RejectedPredictionTokens ?? 0) + (b?.RejectedPredictionTokens ?? 0));
    }
}
