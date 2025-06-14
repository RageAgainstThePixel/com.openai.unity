// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class MCPApprovalFilter
    {
        [Preserve]
        [JsonConstructor]
        public MCPApprovalFilter(MCPToolList always = null, MCPToolList never = null)
        {
            Always = always;
            Never = never;
        }

        [Preserve]
        [JsonProperty("always", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MCPToolList Always { get; }

        [Preserve]
        [JsonProperty("never", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MCPToolList Never { get; }
    }
}
