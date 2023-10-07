// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.FineTuning
{
    [Preserve]
    public sealed class FineTuneList
    {
        [Preserve]
        [JsonConstructor]
        public FineTuneList(
            [JsonProperty("object")] string @object,
            [JsonProperty("data")] List<FineTuneJob> data,
            [JsonProperty("has_more")] bool hasMore)
        {
            Object = @object;
            Data = data;
            HasMore = hasMore;
        }

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        [Preserve]
        [JsonProperty("data")]
        public IReadOnlyList<FineTuneJob> Data { get; }

        [Preserve]
        [JsonProperty("has_more")]
        public bool HasMore { get; }
    }
}
