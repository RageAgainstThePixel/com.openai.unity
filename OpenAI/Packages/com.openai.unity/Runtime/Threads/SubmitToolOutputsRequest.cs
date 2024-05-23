// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class SubmitToolOutputsRequest
    {
        /// <summary>
        /// Tool output to be submitted.
        /// </summary>
        /// <param name="toolOutput"><see cref="ToolOutput"/>.</param>
        [Preserve]
        public SubmitToolOutputsRequest(ToolOutput toolOutput)
            : this(new[] { toolOutput })
        {
        }

        /// <summary>
        /// A list of tools for which the outputs are being submitted.
        /// </summary>
        /// <param name="toolOutputs">Collection of tools for which the outputs are being submitted.</param>
        [Preserve]
        public SubmitToolOutputsRequest(IEnumerable<ToolOutput> toolOutputs)
        {
            ToolOutputs = toolOutputs?.ToList();
        }

        /// <summary>
        /// A list of tools for which the outputs are being submitted.
        /// </summary>
        [Preserve]
        [JsonProperty("tool_outputs")]
        public IReadOnlyList<ToolOutput> ToolOutputs { get; }

        /// <summary>
        /// If true, returns a stream of events that happen during the Run as server-sent events,
        /// terminating when the Run enters a terminal state with a data: [DONE] message.
        /// </summary>
        [Preserve]
        [JsonProperty("stream")]
        public bool Stream { get; internal set; }

        [Preserve]
        public static implicit operator SubmitToolOutputsRequest(ToolOutput toolOutput) => new(toolOutput);

        [Preserve]
        public static implicit operator SubmitToolOutputsRequest(ToolOutput[] toolOutputs) => new(toolOutputs);

        [Preserve]
        public static implicit operator SubmitToolOutputsRequest(List<ToolOutput> toolOutputs) => new(toolOutputs);
    }
}
