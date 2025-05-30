// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI
{
    /// <summary>
    /// **o-series models only**
    /// Configuration options for reasoning models.
    /// </summary>
    public sealed class Reasoning
    {
        /// <summary>
        /// Constrains effort on reasoning for reasoning models.
        /// Currently supported values are: Low, Medium, High.
        /// Reducing reasoning effort can result in faster responses and fewer tokens used on reasoning in a response.
        /// </summary>
        [JsonProperty("effort", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ReasoningEffort Effort { get; }

        /// <summary>
        /// A summary of the reasoning performed by the model.
        /// This can be useful for debugging and understanding the model's reasoning process.
        /// One of `auto`, `concise`, or `detailed`.
        /// </summary>
        [JsonProperty("summary", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ReasoningSummary Summary { get; }
    }
}
