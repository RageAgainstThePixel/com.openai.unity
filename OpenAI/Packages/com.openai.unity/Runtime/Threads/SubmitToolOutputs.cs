// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class SubmitToolOutputs
    {
        [Preserve]
        [JsonConstructor]
        internal SubmitToolOutputs([JsonProperty("tool_calls")] List<ToolCall> toolCalls)
        {
            ToolCalls = toolCalls;
        }

        /// <summary>
        /// A list of the relevant tool calls.
        /// </summary>
        [Preserve]
        [JsonProperty("tool_calls")]
        public IReadOnlyList<ToolCall> ToolCalls { get; }
    }
}
