// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Moderations
{
    [Preserve]
    public sealed class ModerationResult
    {
        [Preserve]
        [JsonConstructor]
        public ModerationResult(
            [JsonProperty("categories")] Categories categories,
            [JsonProperty("category_scores")] Scores scores,
            [JsonProperty("flagged")] bool flagged)
        {
            Categories = categories;
            Scores = scores;
            Flagged = flagged;
        }

        [Preserve]
        [JsonProperty("categories")]
        public Categories Categories { get; }

        [Preserve]
        [JsonProperty("category_scores")]
        public Scores Scores { get; }

        [Preserve]
        [JsonProperty("flagged")]
        public bool Flagged { get; }
    }
}
