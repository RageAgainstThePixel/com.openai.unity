// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A tool that allows the model to execute shell commands in a local environment.
    /// </summary>
    [Preserve]
    public sealed class LocalShellTool : ITool
    {
        [Preserve]
        public static implicit operator Tool(LocalShellTool localShellTool) => new(localShellTool as ITool);

        [Preserve]
        public LocalShellTool() { }

        [Preserve]
        [JsonConstructor]
        internal LocalShellTool(
            [JsonProperty("type")] string type)
        {
            Type = type;
        }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; } = "local_shell";
    }
}
