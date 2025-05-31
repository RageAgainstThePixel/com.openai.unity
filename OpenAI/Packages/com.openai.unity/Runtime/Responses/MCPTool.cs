// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class MCPTool
    {
        [Preserve]
        [JsonConstructor]
        internal MCPTool(
            [JsonProperty("name")] string name,
            [JsonProperty("description")] string description,
            [JsonProperty("input_schema")] string inputSchema,
            [JsonProperty("annotations")] object annotations)
        {
            Name = name;
            Description = description;
            InputSchema = inputSchema;
            Annotations = annotations;
        }

        [Preserve]
        [JsonProperty("name")]
        public string Name { get; }

        [Preserve]
        [JsonProperty("description")]
        public string Description { get; }

        [Preserve]
        [JsonProperty("input_schema")]
        public string InputSchema { get; }

        [Preserve]
        [JsonProperty("annotations")]
        public object Annotations { get; }
    }
}
