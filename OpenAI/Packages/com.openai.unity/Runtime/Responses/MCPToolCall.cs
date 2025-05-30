// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class MCPToolCall : IResponseItem
    {
        [Preserve]
        [JsonConstructor]
        internal MCPToolCall(
            [JsonProperty("id")] string id,
            [JsonProperty("type")] ResponseItemType type,
            [JsonProperty("object")] string @object,
            [JsonProperty("status")] ResponseStatus status,
            [JsonProperty("name")] string name,
            [JsonProperty("server_label")] string serverLabel,
            [JsonProperty("arguments")] string arguments,
            [JsonProperty("output")] string output)
        {
            Id = id;
            Type = type;
            Object = @object;
            Status = status;
            Name = name;
            ServerLabel = serverLabel;
            Arguments = arguments;
            Output = output;
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
        /// The name of the tool that was run.
        /// </summary>
        [Preserve]
        [JsonProperty("name")]
        public string Name { get; }

        /// <summary>
        /// The label of the MCP server running the tool.
        /// </summary>
        [Preserve]
        [JsonProperty("server_label")]
        public string ServerLabel { get; }

        /// <summary>
        /// A JSON string of the arguments to pass to the function.
        /// </summary>
        [Preserve]
        [JsonProperty("arguments")]
        public string Arguments { get; }

        /// <summary>
        /// The output from the tool call.
        /// </summary>
        [Preserve]
        [JsonProperty("output")]
        public string Output { get; }

        /// <summary>
        /// The error from the tool call, if any.
        /// </summary>
        [Preserve]
        [JsonProperty("error")]
        public string Error { get; }
    }
}
