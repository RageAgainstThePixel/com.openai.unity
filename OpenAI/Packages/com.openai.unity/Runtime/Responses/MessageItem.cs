// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class MessageItem : IResponseItem
    {
        [Preserve]
        [JsonConstructor]
        internal MessageItem(
            [JsonProperty("id")] string id,
            [JsonProperty("type")] ResponseItemType type,
            [JsonProperty("object")] string @object,
            [JsonProperty("status")] ResponseStatus status,
            [JsonProperty("role")] Role role,
            [JsonProperty("content")] IReadOnlyList<IResponseContent> content)
        {
            Id = id;
            Type = type;
            Object = @object;
            Status = status;
            Role = role;
            Content = content;
        }

        [Preserve]
        public MessageItem(Role role, IResponseContent content)
            : this(role, new[] { content })
        {
        }

        [Preserve]
        public MessageItem(Role role, IEnumerable<IResponseContent> content)
        {
            Type = ResponseItemType.Message;
            Role = role;
            Content = content?.ToList();
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

        [Preserve]
        [JsonProperty("role", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Role Role { get; }

        [Preserve]
        [JsonProperty("content", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<IResponseContent> Content { get; }
    }
}
