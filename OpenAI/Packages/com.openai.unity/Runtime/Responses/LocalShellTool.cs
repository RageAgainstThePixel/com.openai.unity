// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Responses
{
    /// <summary>
    /// A tool that allows the model to execute shell commands in a local environment.
    /// </summary>
    public sealed class LocalShellTool : ITool
    {
        public static implicit operator Tool(LocalShellTool localShellTool) => new(localShellTool as ITool);

        [JsonProperty("type")]
        public string Type => "local_shell";
    }
}
