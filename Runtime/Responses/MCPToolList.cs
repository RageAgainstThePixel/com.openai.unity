// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class MCPToolList
    {
        [Preserve]
        [JsonConstructor]
        public MCPToolList(IEnumerable<string> toolNames)
        {
            ToolNames = toolNames?.ToList();
        }

        [Preserve]
        [JsonProperty("tool_names", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<string> ToolNames { get; }
    }
}
