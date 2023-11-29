// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.FineTuning
{
    [Preserve]
    [Obsolete("Use ListResponse<FineTuneJobResponse>")]
    public sealed class FineTuneJobList
    {
        [Preserve]
        [JsonConstructor]
        public FineTuneJobList(
            [JsonProperty("object")] string @object,
            [JsonProperty("data")] IReadOnlyList<FineTuneJob> jobs,
            [JsonProperty("has_more")] bool hasMore)
        {
            Object = @object;
            Jobs = jobs;
            HasMore = hasMore;
        }

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        [Preserve]
        [JsonProperty("data")]
        public IReadOnlyList<FineTuneJob> Jobs { get; }

        [Preserve]
        [JsonProperty("has_more")]
        public bool HasMore { get; }
    }
}
