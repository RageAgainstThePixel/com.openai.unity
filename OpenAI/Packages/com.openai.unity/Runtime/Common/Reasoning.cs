// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Responses;
using UnityEngine.Scripting;

namespace OpenAI
{
    /// <summary>
    /// **o-series models only**
    /// Configuration options for reasoning models.
    /// </summary>
    public sealed class Reasoning : IResponseItem
    {
        /// <inheritdoc />
        [Preserve]
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Id { get; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ResponseItemType Type { get; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("object", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Object { get; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("status", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ResponseStatus Status { get; }

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
