// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    /// <summary>
    /// Tool function call output
    /// </summary>
    [Preserve]
    public sealed class ToolOutput
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="toolCallId">
        /// The ID of the tool call in the <see cref="RequiredAction"/> within the <see cref="RunResponse"/> the output is being submitted for.
        /// </param>
        /// <param name="output">
        /// The output of the tool call to be submitted to continue the run.
        /// </param>
        [Preserve]
        [JsonConstructor]
        public ToolOutput(
            [JsonProperty("tool_call_id")] string toolCallId,
            [JsonProperty("output")] string output)
        {
            ToolCallId = toolCallId;
            Output = output;
        }

        /// <summary>
        /// The ID of the tool call in the <see cref="RequiredAction"/> within the <see cref="RunResponse"/> the output is being submitted for.
        /// </summary>
        [Preserve]
        [JsonProperty("tool_call_id")]
        public string ToolCallId { get; }

        /// <summary>
        /// The output of the tool call to be submitted to continue the run.
        /// </summary>
        [Preserve]
        [JsonProperty("output")]
        public string Output { get; }
    }
}
