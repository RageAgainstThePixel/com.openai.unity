// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Moderations
{
    [Preserve]
    public sealed class ModerationsResponse : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        internal ModerationsResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("model")] string model,
            [JsonProperty("results")] List<ModerationResult> results)
        {
            Id = id;
            Model = model;
            Results = results;
        }

        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        [Preserve]
        [JsonProperty("model")]
        public string Model { get; }

        [Preserve]
        [JsonProperty("results")]
        public IReadOnlyList<ModerationResult> Results { get; }
    }
}
