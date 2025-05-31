// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A description of the chain of thought used by a reasoning model while generating a response.
    /// </summary>
    [Preserve]
    public sealed class ReasoningItem : IResponseItem
    {
        [Preserve]
        [JsonConstructor]
        internal ReasoningItem(
            [JsonProperty("id")] string id,
            [JsonProperty("type")] ResponseItemType type,
            [JsonProperty("object")] string @object,
            [JsonProperty("status")] ResponseStatus status,
            [JsonProperty("summary")] IReadOnlyList<ReasoningSummary> summary)
        {
            Id = id;
            Type = type;
            Object = @object;
            Status = status;
            Summary = summary;
        }

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
        /// Reasoning text contents.
        /// </summary>
        [Preserve]
        [JsonProperty("summary")]
        public IReadOnlyList<ReasoningSummary> Summary { get; }
    }
}
