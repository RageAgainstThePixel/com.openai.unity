// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Responses
{
    public sealed class MCPApprovalFilter
    {
        [JsonConstructor]
        public MCPApprovalFilter(MCPToolList always = null, MCPToolList never = null)
        {
            Always = always;
            Never = never;
        }

        [JsonProperty("always", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MCPToolList Always { get; }

        [JsonProperty("never", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MCPToolList Never { get; }
    }
}
