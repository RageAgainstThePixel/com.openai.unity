// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenAI.Embeddings
{
    public sealed class EmbeddingsResponse : BaseResponse
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("data")]
        public List<Datum> Data { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("usage")]
        public Usage Usage { get; set; }
    }
}
