// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Moderations
{
    public sealed class ModerationResult
    {
        [JsonProperty("categories")]
        public Categories Categories { get; set; }

        [JsonProperty("category_scores")]
        public Scores Scores { get; set; }

        [JsonProperty("flagged")]
        public bool Flagged { get; set; }
    }
}
