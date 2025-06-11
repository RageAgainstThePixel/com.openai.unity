// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System.Collections.Generic;

namespace OpenAI.Responses
{
    /// <summary>
    /// Give the model access to additional tools via remote Model Context Protocol (MCP) servers.
    /// </summary>
    public sealed class MCPTool : ITool
    {
        public static implicit operator Tool(MCPTool mcpTool) => new(mcpTool as ITool);

        public MCPTool(
            string serverLabel,
            string serverUrl,
            IReadOnlyList<string> allowedTools,
            IReadOnlyDictionary<string, object> headers,
            string requireApproval)
            : this(serverLabel, serverUrl, allowedTools, headers, (object)requireApproval)
        {
        }

        public MCPTool(
            string serverLabel,
            string serverUrl,
            IReadOnlyList<string> allowedTools,
            IReadOnlyDictionary<string, object> headers,
            MCPApprovalFilter requireApproval)
            : this(serverLabel, serverUrl, allowedTools, headers, (object)requireApproval)
        {
        }

        public MCPTool(
            string serverLabel,
            string serverUrl,
            IReadOnlyList<string> allowedTools = null,
            IReadOnlyDictionary<string, object> headers = null,
            object requireApproval = null)
        {
            ServerLabel = serverLabel;
            ServerUrl = serverUrl;
            AllowedTools = allowedTools;
            Headers = headers ?? new Dictionary<string, object>();
            RequireApproval = requireApproval;
        }

        [JsonProperty("type")]
        public string Type => "mcp";

        /// <summary>
        /// A label for this MCP server, used to identify it in tool calls.
        /// </summary>
        [JsonProperty("server_label", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ServerLabel { get; }

        /// <summary>
        /// The URL for the MCP server.
        /// </summary>
        [JsonProperty("server_url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ServerUrl { get; }

        /// <summary>
        /// List of allowed tool names.
        /// </summary>
        [JsonProperty("allowed_tools", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<string> AllowedTools { get; }

        /// <summary>
        /// Optional HTTP headers to send to the MCP server. Use for authentication or other purposes.
        /// </summary>
        [JsonProperty("headers", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyDictionary<string, object> Headers { get; }

        /// <summary>
        /// Specify which of the MCP server's tools require approval.
        /// Can be one of <see cref="MCPApprovalFilter"/>, "always", or "never".
        /// When set to "never", all tools will not require approval.
        /// </summary>
        [JsonProperty("require_approval", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(StringOrObjectConverter<MCPApprovalFilter>))]
        public object RequireApproval { get; }
    }
}
