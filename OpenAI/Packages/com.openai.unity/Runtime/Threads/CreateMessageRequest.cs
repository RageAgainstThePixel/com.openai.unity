// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class CreateMessageRequest
    {
        [Preserve]
        public static implicit operator CreateMessageRequest(string content) => new CreateMessageRequest(content);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fieldIds"></param>
        /// <param name="metadata"></param>
        [Preserve]
        public CreateMessageRequest(string content, IEnumerable<string> fieldIds = null, IReadOnlyDictionary<string, string> metadata = null)
        {
            Role = Role.User;
            Content = content;
            FileIds = fieldIds?.ToList();
            Metadata = metadata;
        }

        /// <summary>
        /// The role of the entity that is creating the message.
        /// </summary>
        /// <remarks>
        /// Currently only user is supported.
        /// </remarks>
        [Preserve]
        [JsonProperty("role")]
        public Role Role { get; }

        /// <summary>
        /// The content of the message.
        /// </summary>
        [Preserve]
        [JsonProperty("content", DefaultValueHandling = DefaultValueHandling.Populate, NullValueHandling = NullValueHandling.Include, Required = Required.AllowNull)]
        public string Content { get; }

        /// <summary>
        /// A list of File IDs that the message should use. There can be a maximum of 10 files attached to a message.
        /// Useful for tools like retrieval and code_interpreter that can access and use files.
        /// </summary>
        [Preserve]
        [JsonProperty("file_ids")]
        public IReadOnlyList<string> FileIds { get; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </summary>
        [Preserve]
        [JsonProperty("metadata")]
        public IReadOnlyDictionary<string, string> Metadata { get; }
    }
}
