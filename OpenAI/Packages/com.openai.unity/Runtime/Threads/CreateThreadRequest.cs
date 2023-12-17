// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class CreateThreadRequest
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messages">
        /// A list of messages to start the thread with.
        /// </param>
        /// <param name="metadata">
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </param>
        [Preserve]
        public CreateThreadRequest(
            [JsonProperty("messages")] IEnumerable<Message> messages = null,
            [JsonProperty("metadata")] Dictionary<string, string> metadata = null)
        {
            Messages = messages?.ToList();
            Metadata = metadata;
        }

        /// <summary>
        /// A list of messages to start the thread with.
        /// </summary>
        [Preserve]
        [JsonProperty("messages")]
        public IReadOnlyList<Message> Messages { get; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </summary>
        [Preserve]
        [JsonProperty("metadata")]
        public IReadOnlyDictionary<string, string> Metadata { get; }

        [Preserve]
        public static implicit operator CreateThreadRequest(string message) => new CreateThreadRequest(new[] { new Message(message) });
    }
}
