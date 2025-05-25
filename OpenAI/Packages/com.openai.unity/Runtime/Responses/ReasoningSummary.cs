// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class ReasoningSummary
    {
        [Preserve]
        [JsonConstructor]
        internal ReasoningSummary(
            [JsonProperty("type")] string type,
            [JsonProperty("text")] string text)
        {
            Type = type;
            Text = text;
        }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; }

        /// <summary>
        /// A short summary of the reasoning used by the model when generating the response.
        /// </summary>
        [Preserve]
        [JsonProperty("text")]
        public string Text { get; }
    }
}
