using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenAI.Completions
{
    /// <summary>
    /// Object belonging to a <see cref="Choice"/>
    /// </summary>
    public sealed class LogProbabilities
    {
        [JsonConstructor]
        public LogProbabilities(
            IReadOnlyList<string> tokens,
            IReadOnlyList<double> tokenLogProbabilities,
            IReadOnlyList<IReadOnlyDictionary<string, double>> topLogProbabilities,
            IReadOnlyList<int> textOffsets)
        {
            Tokens = tokens;
            TokenLogProbabilities = tokenLogProbabilities;
            TopLogProbabilities = topLogProbabilities;
            TextOffsets = textOffsets;
        }

        [JsonProperty("tokens")]
        public IReadOnlyList<string> Tokens { get; }

        [JsonProperty("token_logprobs")]
        public IReadOnlyList<double> TokenLogProbabilities { get; }

        [JsonProperty("top_logprobs")]
        public IReadOnlyList<IReadOnlyDictionary<string, double>> TopLogProbabilities { get; }

        [JsonProperty("text_offset")]
        public IReadOnlyList<int> TextOffsets { get; }
    }
}
