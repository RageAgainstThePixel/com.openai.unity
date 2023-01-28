// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenAI.Moderations
{
    public sealed class ModerationsResponse : BaseResponse
    {
        [JsonConstructor]
        public ModerationsResponse(string id, string model, List<ModerationResult> results)
        {
            Id = id;
            Model = model;
            Results = results;
        }

        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("model")]
        public string Model { get; }

        [JsonProperty("results")]
        public List<ModerationResult> Results { get; }
    }
}
