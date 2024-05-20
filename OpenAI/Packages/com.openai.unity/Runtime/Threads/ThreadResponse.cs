// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    /// <summary>
    /// A conversation session between an Assistant and a user.
    /// Threads store Messages and automatically handle truncation to fit content into a model’s context.
    /// </summary>
    [Preserve]
    public sealed class ThreadResponse : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        public ThreadResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("created_at")] int createdAtUnitTimeSeconds,
            [JsonProperty("tool_resources")] ToolResources toolResources,
            [JsonProperty("metadata")] Dictionary<string, string> metadata)
        {
            Id = id;
            Object = @object;
            CreatedAtUnixTimeSeconds = createdAtUnitTimeSeconds;
            ToolResources = toolResources;
            Metadata = metadata;
        }

        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// The object type, which is always thread.
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the thread was created.
        /// </summary>
        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixTimeSeconds { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds).DateTime;

        /// <summary>
        /// A set of resources that are made available to the assistant's tools in this thread.
        /// The resources are specific to the type of tool.
        /// For example, the code_interpreter tool requires a list of file IDs,
        /// while the file_search tool requires a list of vector store IDs.
        /// </summary>
        [Preserve]
        [JsonProperty("tool_resources")]
        public ToolResources ToolResources { get; private set; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </summary>
        [Preserve]
        [JsonProperty("metadata")]
        public IReadOnlyDictionary<string, string> Metadata { get; }

        [Preserve]
        public static implicit operator string(ThreadResponse thread) => thread?.ToString();

        [Preserve]
        public override string ToString() => Id;
    }
}
