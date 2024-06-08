// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;
using OpenAI.Extensions;

namespace OpenAI.Threads
{
    /// <summary>
    /// The details of the run step.
    /// </summary>
    [Preserve]
    public sealed class StepDetails
    {
        [Preserve]
        internal StepDetails(StepDetails other) => AppendFrom(other);

        [Preserve]
        [JsonConstructor]
        internal StepDetails(
            [JsonProperty("message_creation")] RunStepMessageCreation messageCreation,
            [JsonProperty("tool_calls")] List<ToolCall> toolCalls)
        {
            MessageCreation = messageCreation;
            this.toolCalls = toolCalls;
        }

        /// <summary>
        /// Details of the message creation by the run step.
        /// </summary>
        [Preserve]
        [JsonProperty("message_creation", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public RunStepMessageCreation MessageCreation { get; private set; }

        private List<ToolCall> toolCalls;

        /// <summary>
        /// An array of tool calls the run step was involved in.
        /// These can be associated with one of three types of tools: 'code_interpreter', 'retrieval', or 'function'.
        /// </summary>
        [Preserve]
        [JsonProperty("tool_calls", ReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
        public IReadOnlyList<ToolCall> ToolCalls => toolCalls;

        internal void AppendFrom(StepDetails other)
        {
            if (other.MessageCreation != null)
            {
                if (MessageCreation == null)
                {
                    MessageCreation = other.MessageCreation;
                }
                else
                {
                    MessageCreation.AppendFrom(other.MessageCreation);
                }
            }

            if (other.ToolCalls != null)
            {
                toolCalls ??= new List<ToolCall>();
                toolCalls.AppendFrom(other.ToolCalls);
            }
        }
    }
}
