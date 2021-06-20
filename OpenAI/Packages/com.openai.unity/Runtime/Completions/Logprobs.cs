// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenAI
{
    /// <summary>
    /// Object belonging to a <see cref="Choice"/>
    /// </summary>
    public sealed class Logprobs
    {
        [JsonProperty("tokens")]
        public List<string> Tokens { get; set; }

        [JsonProperty("token_logprobs")]
        public List<double> TokenLogprobs { get; set; }

        [JsonProperty("top_logprobs")]
        public IList<IDictionary<string, double>> TopLogprobs { get; set; }

        [JsonProperty("text_offset")]
        public List<int> TextOffsets { get; set; }
    }
}
