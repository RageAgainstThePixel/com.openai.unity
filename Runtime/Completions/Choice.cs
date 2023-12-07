// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Completions
{
    /// <summary>
    /// Represents a completion choice returned by the <see cref="CompletionsEndpoint"/>.
    /// </summary>
    [Preserve]
    [Obsolete("Deprecated")]
    public sealed class Choice
    {
        [Preserve]
        [JsonConstructor]
        public Choice(
            [JsonProperty("text")] string text,
            [JsonProperty("index")] int index,
            [JsonProperty("logprobs")] LogProbabilities logProbabilities,
            [JsonProperty("finish_reason")] string finishReason)
        {
            Text = text;
            Index = index;
            LogProbabilities = logProbabilities;
            FinishReason = finishReason;
        }

        /// <summary>
        /// The main text of the completion
        /// </summary>
        [Preserve]
        [JsonProperty("text")]
        public string Text { get; }

        /// <summary>
        /// If multiple completion choices we returned, this is the index withing the various choices
        /// </summary>
        [Preserve]
        [JsonProperty("index")]
        public int Index { get; }

        /// <summary>
        /// If the request specified <see cref="CompletionRequest.LogProbabilities"/>, this contains the list of the most likely tokens.
        /// </summary>
        [Preserve]
        [JsonProperty("logprobs")]
        public LogProbabilities LogProbabilities { get; }

        /// <summary>
        /// If this is the last segment of the completion result, this specifies why the completion has ended.
        /// </summary>
        [Preserve]
        [JsonProperty("finish_reason")]
        public string FinishReason { get; }

        /// <summary>
        /// Gets the main text of this completion
        /// </summary>
        public override string ToString() => Text ?? string.Empty;

        public static implicit operator string(Choice choice) => choice.Text;
    }
}
