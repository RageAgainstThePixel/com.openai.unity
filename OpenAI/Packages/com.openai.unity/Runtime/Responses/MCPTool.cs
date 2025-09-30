// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// Give the model access to additional tools via remote Model Context Protocol (MCP) servers.
    /// <see href="https://platform.openai.com/docs/guides/tools-remote-mcp"/>
    /// </summary>
    [Preserve]
    public sealed class MCPTool : ITool
    {
        [Preserve]
        public static implicit operator Tool(MCPTool mcpTool) => new(mcpTool as ITool);

        [Preserve]
        public MCPTool(
            string serverLabel,
            string serverUrl,
            string connectorId,
            string authorization,
            string serverDescription,
            IReadOnlyList<string> allowedTools,
            IReadOnlyDictionary<string, object> headers,
            MCPToolRequireApproval requireApproval)
            : this(
                serverLabel: serverLabel,
                serverUrl: serverUrl,
                connectorId: connectorId,
                authorization: authorization,
                serverDescription: serverDescription,
                allowedTools: allowedTools,
                headers: headers,
                requireApproval: requireApproval.ToString().ToLower())
        {
        }

        [Preserve]
        public MCPTool(
            string serverLabel,
            string serverUrl,
            string connectorId,
            string authorization,
            string serverDescription,
            IReadOnlyList<string> allowedTools,
            IReadOnlyDictionary<string, object> headers,
            MCPApprovalFilter requireApproval)
            : this(
                serverLabel: serverLabel,
                serverUrl: serverUrl,
                connectorId: connectorId,
                authorization: authorization,
                serverDescription: serverDescription,
                allowedTools: allowedTools,
                headers: headers,
                requireApproval: (object)requireApproval)
        {
        }

        [Preserve]
        public MCPTool(
            string serverLabel,
            string serverUrl = null,
            string connectorId = null,
            string authorization = null,
            string serverDescription = null,
            IReadOnlyList<string> allowedTools = null,
            IReadOnlyDictionary<string, object> headers = null,
            object requireApproval = null)
        {
            ServerLabel = serverLabel;
            ServerUrl = serverUrl;
            ConnectorId = connectorId;
            Authorization = authorization;
            ServerDescription = serverDescription;
            AllowedTools = allowedTools;
            Headers = headers ?? new Dictionary<string, object>();
            RequireApproval = requireApproval;
        }

        [Preserve]
        [JsonConstructor]
        internal MCPTool(
            [JsonProperty("type")] string type,
            [JsonProperty("server_label")] string serverLabel,
            [JsonProperty("server_url")] string serverUrl,
            [JsonProperty("connector_id")] string connectorId,
            [JsonProperty("authorization")] string authorization,
            [JsonProperty("server_description")] string serverDescription,
            [JsonProperty("allowed_tools")] List<object> allowedTools,
            [JsonProperty("headers")] Dictionary<string, object> headers,
            [JsonProperty("require_approval")] object requireApproval)
        {
            Type = type;
            ServerLabel = serverLabel;
            ServerUrl = serverUrl;
            ConnectorId = connectorId;
            Authorization = authorization;
            ServerDescription = serverDescription;
            AllowedTools = allowedTools;
            Headers = headers;
            RequireApproval = requireApproval;
        }

        /// <summary>
        /// The type of the MCP tool. Always `mcp`.
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public string Type { get; } = "mcp";

        /// <summary>
        /// A label for this MCP server, used to identify it in tool calls.
        /// </summary>
        [Preserve]
        [JsonProperty("server_label")]
        public string ServerLabel { get; }

        /// <summary>
        /// Identifier for service connectors, like those available in ChatGPT. One of
        /// <see cref="ServerUrl"/> or <see cref="ConnectorId"/> must be provided. Learn more about service
        /// connectors <see href="https://platform.openai.com/docs/guides/tools-remote-mcp#connectors"/>.<br/>
        /// Currently supported `connector_id` values are:<br/>
        /// <list type="bullet">
        /// <item>
        /// <description>Dropbox: <c>connector_dropbox</c></description>
        /// </item>
        /// <item>
        /// <description>Gmail: <c>connector_gmail</c></description>
        /// </item>
        /// <item>
        /// <description>Google Calendar: <c>connector_googlecalendar</c></description>
        /// </item>
        /// <item>
        /// <description>Google Drive: <c>connector_googledrive</c></description>
        /// </item>
        /// <item>
        /// <description>Microsoft Teams: <c>connector_microsoftteams</c></description>
        /// </item>
        /// <item>
        /// <description>Outlook Calendar: <c>connector_outlookcalendar</c></description>
        /// </item>
        /// <item>
        /// <description>Outlook Email: <c>connector_outlookemail</c></description>
        /// </item>
        /// <item>
        /// <description>SharePoint: <c>connector_sharepoint</c></description>
        /// </item>
        /// </list>
        /// </summary>
        [Preserve]
        [JsonProperty("connector_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ConnectorId { get; }

        /// <summary>
        /// An OAuth access token that can be used with a remote MCP server, either
        /// with a custom MCP server URL or a service connector. Your application
        /// must handle the OAuth authorization flow and provide the token here.
        /// </summary>
        [Preserve]
        [JsonProperty("authorization", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Authorization { get; }

        /// <summary>
        /// Optional description of the MCP server, used to provide more context.
        /// </summary>
        [Preserve]
        [JsonProperty("server_description", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ServerDescription { get; }

        /// <summary>
        /// The URL for the MCP server.
        /// </summary>
        [Preserve]
        [JsonProperty("server_url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ServerUrl { get; }

        /// <summary>
        /// List of allowed tool names.
        /// </summary>
        [Preserve]
        [JsonProperty("allowed_tools", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<object> AllowedTools { get; }

        /// <summary>
        /// Optional HTTP headers to send to the MCP server. Use for authentication or other purposes.
        /// </summary>
        [Preserve]
        [JsonProperty("headers", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyDictionary<string, object> Headers { get; }

        /// <summary>
        /// Specify which of the MCP server's tools require approval.
        /// Can be one of <see cref="MCPApprovalFilter"/>, "always", or "never".
        /// When set to "never", all tools will not require approval.
        /// </summary>
        [Preserve]
        [JsonProperty("require_approval", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(StringOrObjectConverter<MCPApprovalFilter>))]
        public object RequireApproval { get; }
    }
}
