// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenAI.Completions
{
    /// <summary>
    /// Object belonging to a <see cref="Choice"/>
    /// </summary>
    public sealed class Logprobs
    {
        [JsonConstructor]
        public Logprobs(List<string> tokens, List<double> tokenLogprobs, IList<IDictionary<string, double>> topLogprobs, List<int> textOffsets)
        {
            Tokens = tokens;
            TokenLogprobs = tokenLogprobs;
            TopLogprobs = topLogprobs;
            TextOffsets = textOffsets;
        }

        [JsonProperty("tokens")]
        public List<string> Tokens { get; }

        [JsonProperty("token_logprobs")]
        public List<double> TokenLogprobs { get; }

        [JsonProperty("top_logprobs")]
        public IList<IDictionary<string, double>> TopLogprobs { get; }

        [JsonProperty("text_offset")]
        public List<int> TextOffsets { get; }
    }
}
