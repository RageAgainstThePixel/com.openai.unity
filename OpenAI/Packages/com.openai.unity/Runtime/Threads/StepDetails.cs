// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    /// <summary>
    /// The details of the run step.
    /// </summary>
    [Preserve]
    public sealed class StepDetails
    {
        [Preserve]
        [JsonConstructor]
        public StepDetails(
            [JsonProperty("message_creation")] RunStepMessageCreation messageCreation,
            [JsonProperty("tool_calls")] IReadOnlyList<ToolCall> toolCalls)
        {
            MessageCreation = messageCreation;
            ToolCalls = toolCalls;
        }

        /// <summary>
        /// Details of the message creation by the run step.
        /// </summary>
        [Preserve]
        [JsonProperty("message_creation")]
        public RunStepMessageCreation MessageCreation { get; }

        /// <summary>
        /// An array of tool calls the run step was involved in.
        /// These can be associated with one of three types of tools: 'code_interpreter', 'retrieval', or 'function'.
        /// </summary>
        [Preserve]
        [JsonProperty("tool_calls")]
        public IReadOnlyList<ToolCall> ToolCalls { get; }
    }
}
