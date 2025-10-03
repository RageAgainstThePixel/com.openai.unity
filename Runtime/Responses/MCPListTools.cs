// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class MCPListTools : BaseResponse, IResponseItem
    {
        [Preserve]
        [JsonConstructor]
        internal MCPListTools(
            [JsonProperty("id")] string id,
            [JsonProperty("type")] ResponseItemType type,
            [JsonProperty("object")] string @object,
            [JsonProperty("status")] ResponseStatus status,
            [JsonProperty("server_label")] string serverLabel,
            [JsonProperty("error")] string error,
            [JsonProperty("tools")] List<MCPServerTool> tools)
        {
            Id = id;
            Type = type;
            Object = @object;
            Status = status;
            ServerLabel = serverLabel;
            Error = error;
            Tools = tools ?? new List<MCPServerTool>();
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
        /// The label of the MCP server running the tool.
        /// </summary>
        [Preserve]
        [JsonProperty("server_label", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ServerLabel { get; }

        /// <summary>
        /// The error from the tool call, if any.
        /// </summary>
        [Preserve]
        [JsonProperty("error", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Error { get; }

        /// <summary>
        /// The tools available on the server.
        /// </summary>
        [Preserve]
        [JsonProperty("tools")]
        public IReadOnlyList<MCPServerTool> Tools { get; }
    }
}
