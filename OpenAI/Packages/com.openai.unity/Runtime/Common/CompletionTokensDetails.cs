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
            [JsonProperty("accepted_prediction_tokens")] int? acceptedPredictionTokens,
            [JsonProperty("audio_tokens")] int? audioTokens,
            [JsonProperty("reasoning_tokens")] int? reasoningTokens,
            [JsonProperty("rejected_prediction_tokens")] int? rejectedPredictionTokens)
        {
            AcceptedPredictionTokens = acceptedPredictionTokens;
            AudioTokens = audioTokens;
            ReasoningTokens = reasoningTokens;
            RejectedPredictionTokens = rejectedPredictionTokens;
        }

        [Preserve]
        [JsonProperty("accepted_prediction_tokens")]
        public int? AcceptedPredictionTokens { get; }

        [Preserve]
        [JsonProperty("audio_tokens")]
        public int? AudioTokens { get; }

        [Preserve]
        [JsonProperty("reasoning_tokens")]
        public int? ReasoningTokens { get; }

        [Preserve]
        [JsonProperty("rejected_prediction_tokens")]
        public int? RejectedPredictionTokens { get; }

        [Preserve]
        public static CompletionTokensDetails operator +(CompletionTokensDetails a, CompletionTokensDetails b)
            => new(
                (a?.AcceptedPredictionTokens ?? 0) + (b?.AcceptedPredictionTokens ?? 0),
                (a?.AudioTokens ?? 0) + (b?.AudioTokens ?? 0),
                (a?.ReasoningTokens ?? 0) + (b?.ReasoningTokens ?? 0),
                (a?.RejectedPredictionTokens ?? 0) + (b?.RejectedPredictionTokens ?? 0));
    }
}
