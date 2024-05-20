// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Obsolete("use Thread.Message instead.")]
    public sealed class CreateMessageRequest
    {
        [Preserve]
        public static implicit operator CreateMessageRequest(string content) => new(content);

        public static implicit operator CreateMessageRequest(Message message) => new(message.Content, message.Role, message.Attachments, message.Metadata);

        public static implicit operator Message(CreateMessageRequest request) => new(request.Content, request.Role, request.Attachments, request.Metadata);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileIds"></param>
        /// <param name="metadata"></param>
        [Obsolete("Removed")]
        public CreateMessageRequest(string content, IEnumerable<string> fileIds, IReadOnlyDictionary<string, string> metadata = null)
        {
        }

        public CreateMessageRequest(string content, Role role = Role.User, IEnumerable<Attachment> attachments = null, IReadOnlyDictionary<string, string> metadata = null)
            : this(new List<Content> { new(content) }, role, attachments, metadata)
        {
        }

        public CreateMessageRequest(IEnumerable<Content> content, Role role = Role.User, IEnumerable<Attachment> attachments = null, IReadOnlyDictionary<string, string> metadata = null)
        {
            Content = content?.ToList();
            Role = role;
            Attachments = attachments?.ToList();
            Metadata = metadata;
        }

        /// <summary>
        /// The role of the entity that is creating the message.
        /// </summary>
        /// <remarks>
        /// Currently only user is supported.
        /// </remarks>
        [JsonProperty("role")]
        public Role Role { get; }

        /// <summary>
        /// The contents of the message.
        /// </summary>
        [JsonProperty("content", DefaultValueHandling = DefaultValueHandling.Populate, NullValueHandling = NullValueHandling.Include)]
        public IReadOnlyList<Content> Content { get; }

        /// <summary>
        /// A list of files attached to the message, and the tools they were added to.
        /// </summary>
        [JsonProperty("Attachments")]
        public IReadOnlyList<Attachment> Attachments { get; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </summary>
        [JsonProperty("metadata")]
        public IReadOnlyDictionary<string, string> Metadata { get; }
    }
}
