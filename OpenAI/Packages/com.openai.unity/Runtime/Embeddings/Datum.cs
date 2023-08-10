// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenAI.Embeddings
{
    public sealed class Datum
    {
        [JsonConstructor]
        public Datum(
            [JsonProperty("object")] string @object,
            [JsonProperty("embedding")] IReadOnlyList<double> embedding,
            [JsonProperty("index")] int index)
        {
            Object = @object;
            Embedding = embedding;
            Index = index;
        }

        [JsonProperty("object")]
        public string Object { get; private set; }

        [JsonProperty("embedding")]
        public IReadOnlyList<double> Embedding { get; private set; }

        [JsonProperty("index")]
        public int Index { get; private set; }
    }
}
