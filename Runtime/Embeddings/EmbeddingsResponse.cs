// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Embeddings
{
    [Preserve]
    public sealed class EmbeddingsResponse : BaseResponse
    {
        [Preserve]
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

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; private set; }

        [Preserve]
        [JsonProperty("data")]
        public IReadOnlyList<Datum> Data { get; private set; }

        [Preserve]
        [JsonProperty("model")]
        public string Model { get; private set; }

        [Preserve]
        [JsonProperty("usage")]
        public Usage Usage { get; private set; }
    }
}
