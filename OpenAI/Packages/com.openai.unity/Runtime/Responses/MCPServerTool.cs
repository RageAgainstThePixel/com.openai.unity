// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A tool available on an MCP Server
    /// </summary>
    [Preserve]
    public sealed class MCPServerTool
    {
        [Preserve]
        [JsonConstructor]
        internal MCPServerTool(
            [JsonProperty("name")] string name,
            [JsonProperty("description")] string description,
            [JsonProperty("input_schema")] JToken inputSchema,
            [JsonProperty("annotations")] JToken annotations)
        {
            Name = name;
            Description = description;
            InputSchema = inputSchema;
            Annotations = annotations;
        }

        /// <summary>
        /// The name of the tool.
        /// </summary>
        [Preserve]
        [JsonProperty("name")]
        public string Name { get; }

        /// <summary>
        /// The description of the tool.
        /// </summary>
        [Preserve]
        [JsonProperty("description", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Description { get; }

        /// <summary>
        /// The JSON schema describing the tool's input.
        /// </summary>
        [Preserve]
        [JsonProperty("input_schema")]
        public JToken InputSchema { get; }

        /// <summary>
        /// Additional annotations about the tool.
        /// </summary>
        [Preserve]
        [JsonProperty("annotations", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public JToken Annotations { get; }
    }
}
