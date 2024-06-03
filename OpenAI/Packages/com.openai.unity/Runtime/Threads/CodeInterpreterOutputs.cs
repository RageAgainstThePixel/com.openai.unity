// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class CodeInterpreterOutputs
    {
        [Preserve]
        [JsonConstructor]
        internal CodeInterpreterOutputs(
            [JsonProperty("type")] CodeInterpreterOutputType type,
            [JsonProperty("logs")] string logs,
            [JsonProperty("image")] ImageFile image)
        {
            Type = type;
            Logs = logs;
            Image = image;
        }

        /// <summary>
        /// Output type. Can be either 'logs' or 'image'.
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public CodeInterpreterOutputType Type { get; }

        /// <summary>
        /// Text output from the Code Interpreter tool call as part of a run step.
        /// </summary>
        [Preserve]
        [JsonProperty("logs")]
        public string Logs { get; }

        /// <summary>
        /// Code interpreter image output.
        /// </summary>
        [Preserve]
        [JsonProperty("image")]
        public ImageFile Image { get; }
    }
}
