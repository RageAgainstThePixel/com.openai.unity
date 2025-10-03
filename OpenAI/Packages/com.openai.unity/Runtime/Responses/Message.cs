// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class Message : BaseResponse, IResponseItem
    {
        [Preserve]
        public static implicit operator Message(string input) => new(Role.User, input);

        [Preserve]
        public static implicit operator Message(Texture2D input) => new(Role.User, input);

        [Preserve]
        public static implicit operator Message(AudioClip input) => new(Role.User, input);

        [Preserve]
        [JsonConstructor]
        internal Message(
            [JsonProperty("id")] string id,
            [JsonProperty("type")] ResponseItemType type,
            [JsonProperty("object")] string @object,
            [JsonProperty("status")] ResponseStatus status,
            [JsonProperty("role")] Role role,
            [JsonProperty("content")] List<IResponseContent> content)
        {
            Id = id;
            Type = type;
            Object = @object;
            Status = status;
            Role = role;
            Content = content;
        }

        [Preserve]
        public Message(Role role, string text)
            : this(role, new TextContent(text))
        {
        }

        [Preserve]
        public Message(Role role, Texture2D image)
            : this(role, new ImageContent(image))
        {
        }

        [Preserve]
        public Message(Role role, AudioClip clip)
            : this(role, new AudioContent(clip))
        {
        }

        [Preserve]
        public Message(Role role, IResponseContent content)
            : this(role, new[] { content })
        {
        }

        [Preserve]
        public Message(Role role, IEnumerable<IResponseContent> content)
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

        private List<IResponseContent> content;

        [Preserve]
        [JsonProperty("content", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<IResponseContent> Content
        {
            get => content;
            private set => content = value?.ToList() ?? new();
        }

        [Preserve]
        internal void AddOrUpdateContentItem(IResponseContent item, int index)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (index > content.Count)
            {
                for (var i = content.Count; i < index; i++)
                {
                    content.Add(null);
                }
            }

            content.Insert(index, item);
        }

        [Preserve]
        public override string ToString()
            => Content?.LastOrDefault()?.ToString() ?? string.Empty;

        [Preserve]
        public T FromSchema<T>(JsonSerializerSettings options = null)
        {
            options ??= OpenAIClient.JsonSerializationOptions;
            return JsonConvert.DeserializeObject<T>(ToString(), options);
        }
    }
}
