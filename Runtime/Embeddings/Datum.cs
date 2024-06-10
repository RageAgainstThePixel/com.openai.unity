// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Embeddings
{
    [Preserve]
    public sealed class Datum
    {
        [Preserve]
        [JsonConstructor]
        internal Datum(
            [JsonProperty("object")] string @object,
            [JsonProperty("embedding")] IReadOnlyList<double> embedding,
            [JsonProperty("index")] int index)
        {
            Object = @object;
            Embedding = embedding;
            Index = index;
        }

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        [Preserve]
        [JsonProperty("embedding")]
        public IReadOnlyList<double> Embedding { get; }

        [Preserve]
        [JsonProperty("index")]
        public int Index { get; }
    }
}
