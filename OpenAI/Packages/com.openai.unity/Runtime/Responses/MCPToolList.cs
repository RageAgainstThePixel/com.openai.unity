// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenAI.Responses
{
    public sealed class MCPToolList
    {
        [JsonConstructor]
        public MCPToolList(IEnumerable<string> toolNames)
        {
            ToolNames = toolNames?.ToList() ?? throw new ArgumentNullException(nameof(toolNames));
        }

        [JsonProperty("tool_names", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<string> ToolNames { get; }
    }
}
