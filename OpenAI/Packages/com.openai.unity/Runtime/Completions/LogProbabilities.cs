// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Completions
{
    /// <summary>
    /// Object belonging to a <see cref="Choice"/>
    /// </summary>
    [Preserve]
    [Obsolete("Deprecated")]
    public sealed class LogProbabilities
    {
        [Preserve]
        [JsonConstructor]
        public LogProbabilities(
            [JsonProperty("tokens")] IReadOnlyList<string> tokens,
            [JsonProperty("token_logprobs")] IReadOnlyList<double> tokenLogProbabilities,
            [JsonProperty("top_logprobs")] IReadOnlyList<IReadOnlyDictionary<string, double>> topLogProbabilities,
            [JsonProperty("text_offset")] IReadOnlyList<int> textOffsets)
        {
            Tokens = tokens;
            TokenLogProbabilities = tokenLogProbabilities;
            TopLogProbabilities = topLogProbabilities;
            TextOffsets = textOffsets;
        }

        [Preserve]
        [JsonProperty("tokens")]
        public IReadOnlyList<string> Tokens { get; }

        [Preserve]
        [JsonProperty("token_logprobs")]
        public IReadOnlyList<double> TokenLogProbabilities { get; }

        [Preserve]
        [JsonProperty("top_logprobs")]
        public IReadOnlyList<IReadOnlyDictionary<string, double>> TopLogProbabilities { get; }

        [Preserve]
        [JsonProperty("text_offset")]
        public IReadOnlyList<int> TextOffsets { get; }
    }
}
