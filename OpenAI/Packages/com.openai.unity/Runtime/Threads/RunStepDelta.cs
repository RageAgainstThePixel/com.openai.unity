// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class RunStepDelta
    {
        [Preserve]
        [JsonConstructor]
        internal RunStepDelta(
            [JsonProperty("step_details")] StepDetails stepDetails,
            [JsonProperty("tool_calls")] List<ToolCall> toolCalls)
        {
            StepDetails = stepDetails;
            ToolCalls = toolCalls;
        }

        [Preserve]
        [JsonProperty("step_details")]
        public StepDetails StepDetails { get; }

        [Preserve]
        [JsonProperty("tool_calls")]
        public IReadOnlyList<ToolCall> ToolCalls { get; }
    }
}
