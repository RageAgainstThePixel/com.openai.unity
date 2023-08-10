// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenAI.Embeddings
{
    public sealed class EmbeddingsResponse : BaseResponse
    {
        [JsonConstructor]
        public EmbeddingsResponse(
            [JsonProperty("object")] string @object,
            [JsonProperty("data")] IReadOnlyList<Datum> data,
            [JsonProperty("model")] string model,
            [JsonProperty("usage")] Usage usage)
        {
            Object = @object;
            Data = data;
            Model = model;
            Usage = usage;
        }

        [JsonProperty("object")]
        public string Object { get; private set; }

        [JsonProperty("data")]
        public IReadOnlyList<Datum> Data { get; private set; }

        [JsonProperty("model")]
        public string Model { get; private set; }

        [JsonProperty("usage")]
        public Usage Usage { get; private set; }
    }
}
