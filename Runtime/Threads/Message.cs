// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class Message
    {
        [Preserve]
        public static implicit operator Message(string content) => new(content);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content">
        /// The contents of the message.
        /// </param>
        /// <param name="role">
        /// The role of the entity that is creating the message.
        /// </param>
        /// <param name="attachments">
        /// A list of files attached to the message, and the tools they were added to.
        /// </param>
        /// <param name="metadata">
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </param>
        [Preserve]
        public Message(
            string content,
            Role role = Role.User,
            IEnumerable<Attachment> attachments = null,
            IReadOnlyDictionary<string, string> metadata = null)
            : this(new List<Content> { new(content) }, role, attachments, metadata)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content">
        /// The contents of the message.
        /// </param>
        /// <param name="role">
        /// The role of the entity that is creating the message.
        /// </param>
        /// <param name="attachments">
        /// A list of files attached to the message, and the tools they were added to.
        /// </param>
        /// <param name="metadata">
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </param>
        [Preserve]
        [JsonConstructor]
        public Message(
            [JsonProperty("content")] IEnumerable<Content> content,
            [JsonProperty("role")] Role role = Role.User,
            [JsonProperty("attachments")] IEnumerable<Attachment> attachments = null,
            [JsonProperty("metadata")] IReadOnlyDictionary<string, string> metadata = null)
        {
            Content = content?.ToList();
            Role = role;
            Attachments = attachments?.ToList();
            Metadata = metadata;
        }

        /// <summary>
        /// The role of the entity that is creating the message.
        /// </summary>
        [Preserve]
        [JsonProperty("role")]
        public Role Role { get; }

        /// <summary>
        /// The contents of the message.
        /// </summary>
        [Preserve]
        [JsonProperty("content", DefaultValueHandling = DefaultValueHandling.Populate, NullValueHandling = NullValueHandling.Include)]
        public IReadOnlyList<Content> Content { get; }

        /// <summary>
        /// A list of files attached to the message, and the tools they were added to.
        /// </summary>
        [Preserve]
        [JsonProperty("attachments")]
        public IReadOnlyList<Attachment> Attachments { get; private set; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </summary>
        [Preserve]
        [JsonProperty("metadata")]
        public IReadOnlyDictionary<string, string> Metadata { get; }

        /// <summary>
        /// Formats all of the <see cref="Content"/> items into a single string,
        /// putting each item on a new line.
        /// </summary>
        /// <returns><see cref="string"/> of all <see cref="Content"/>.</returns>
        [Preserve]
        public string PrintContent() => string.Join("\n", Content.Select(content => content?.ToString()));
    }
}
