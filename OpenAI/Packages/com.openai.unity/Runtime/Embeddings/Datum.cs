// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenAI.Embeddings
{
    public class Datum
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("embedding")]
        public List<double> Embedding { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }
    }
}
