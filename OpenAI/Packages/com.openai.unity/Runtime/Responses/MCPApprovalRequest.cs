// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class MCPApprovalRequest : IResponseItem
    {
        [Preserve]
        [JsonConstructor]
        internal MCPApprovalRequest(
            [JsonProperty("id")] string id,
            [JsonProperty("type")] ResponseItemType type,
            [JsonProperty("object")] string @object,
            [JsonProperty("status")] ResponseStatus status,
            [JsonProperty("name")] string name,
            [JsonProperty("server_label")] string serverLabel,
            [JsonProperty("arguments")] string arguments)
        {
            Id = id;
            Type = type;
            Object = @object;
            Status = status;
            Name = name;
            ServerLabel = serverLabel;
            Arguments = arguments;
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
        /// The name of the tool to run.
        /// </summary>
        [Preserve]
        [JsonProperty("name")]
        public string Name { get; }

        /// <summary>
        /// A JSON string of arguments for the tool.
        /// </summary>
        [Preserve]
        [JsonProperty("arguments")]
        public string Arguments { get; }

        /// <summary>
        /// The label of the MCP server making the request.
        /// </summary>
        [Preserve]
        [JsonProperty("server_label")]
        public string ServerLabel { get; }
    }
}
