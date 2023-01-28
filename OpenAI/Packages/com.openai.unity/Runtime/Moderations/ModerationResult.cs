// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Moderations
{
    public sealed class ModerationResult
    {
        [JsonConstructor]
        public ModerationResult(Categories categories, Scores scores, bool flagged)
        {
            Categories = categories;
            Scores = scores;
            Flagged = flagged;
        }

        [JsonProperty("categories")]
        public Categories Categories { get; }

        [JsonProperty("category_scores")]
        public Scores Scores { get; }

        [JsonProperty("flagged")]
        public bool Flagged { get; }
    }
}
