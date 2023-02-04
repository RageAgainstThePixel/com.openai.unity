// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenAI.Embeddings
{
    public sealed class EmbeddingsResponse : BaseResponse
    {
        [JsonConstructor]
        public EmbeddingsResponse(string @object, IReadOnlyList<Datum> data, string model, Usage usage)
        {
            Object = @object;
            Data = data;
            Model = model;
            Usage = usage;
        }

        [JsonProperty("object")]
        public string Object { get; }

        [JsonProperty("data")]
        public IReadOnlyList<Datum> Data { get; }

        [JsonProperty("model")]
        public string Model { get; }

        [JsonProperty("usage")]
        public Usage Usage { get; }
    }
}
