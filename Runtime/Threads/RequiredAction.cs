// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class RequiredAction
    {
        [Preserve]
        [JsonConstructor]
        internal RequiredAction(
            [JsonProperty("type")] string type,
            [JsonProperty("submit_tool_outputs")] SubmitToolOutputs submitToolOutputs)
        {
            Type = type;
            SubmitToolOutputs = submitToolOutputs;
        }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; }

        /// <summary>
        /// Details on the tool outputs needed for this run to continue.
        /// </summary>
        [Preserve]
        [JsonProperty("submit_tool_outputs")]
        public SubmitToolOutputs SubmitToolOutputs { get; }
    }
}
