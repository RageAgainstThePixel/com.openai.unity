// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenAI.Moderations
{
    public sealed class ModerationsResponse : BaseResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("results")]
        public List<ModerationResult> Results { get; set; }
    }
}
