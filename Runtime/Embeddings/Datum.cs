// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenAI.Embeddings
{
    public sealed class Datum
    {
        [JsonConstructor]
        public Datum(string @object, IReadOnlyList<double> embedding, int index)
        {
            Object = @object;
            Embedding = embedding;
            Index = index;
        }

        [JsonProperty("object")]
        public string Object { get; }

        [JsonProperty("embedding")]
        public IReadOnlyList<double> Embedding { get; }

        [JsonProperty("index")]
        public int Index { get; }
    }
}
