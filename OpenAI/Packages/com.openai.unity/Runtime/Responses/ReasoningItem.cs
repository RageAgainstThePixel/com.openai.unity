// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A description of the chain of thought used by a reasoning model while generating a response.
    /// </summary>
    [Preserve]
    public sealed class ReasoningItem : BaseResponse, IResponseItem
    {
        [Preserve]
        [JsonConstructor]
        internal ReasoningItem(
            [JsonProperty("id")] string id,
            [JsonProperty("type")] ResponseItemType type,
            [JsonProperty("object")] string @object,
            [JsonProperty("status")] ResponseStatus status,
            [JsonProperty("summary")] List<ReasoningSummary> summary,
            [JsonProperty("content")] List<ReasoningContent> content,
            [JsonProperty("encrypted_content")] string encryptedContent)
        {
            Id = id;
            Type = type;
            Object = @object;
            Status = status;
            Summary = summary;
            EncryptedContent = encryptedContent;
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

        private List<ReasoningSummary> summary = new();

        /// <summary>
        /// Reasoning text contents.
        /// </summary>
        [Preserve]
        [JsonProperty("summary", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<ReasoningSummary> Summary
        {
            get => summary;
            private set => summary = value?.ToList() ?? new();
        }

        private List<ReasoningContent> content;

        /// <summary>
        /// Reasoning text content.
        /// </summary>
        [Preserve]
        [JsonProperty("content", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<ReasoningContent> Content
        {
            get => content;
            private set => content = value?.ToList() ?? new();
        }

        /// <summary>
        /// The encrypted content of the reasoning item - populated when a response is generated with reasoning.encrypted_content in the include parameter.
        /// </summary>
        [Preserve]
        [JsonProperty("encrypted_content", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string EncryptedContent { get; }

        [Preserve]
        internal void InsertReasoningContent(ReasoningContent reasoningContent, int index)
        {
            if (reasoningContent == null)
            {
                throw new ArgumentNullException(nameof(reasoningContent));
            }

            content ??= new();

            if (index > content.Count)
            {
                for (var i = content.Count; i < index; i++)
                {
                    content.Add(null);
                }
            }

            content.Insert(index, reasoningContent);
        }

        [Preserve]
        internal void InsertSummary(ReasoningSummary item, int index)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            summary ??= new();

            if (index > summary.Count)
            {
                for (var i = summary.Count; i < index; i++)
                {
                    summary.Add(null);
                }
            }

            summary.Insert(index, item);
        }
    }
}
