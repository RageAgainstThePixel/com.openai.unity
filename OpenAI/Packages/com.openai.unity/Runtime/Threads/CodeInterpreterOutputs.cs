using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    public sealed class CodeInterpreterOutputs
    {
        /// <summary>
        /// Output type. Can be either 'logs' or 'image'.
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public CodeInterpreterOutputType Type { get; private set; }

        /// <summary>
        /// Text output from the Code Interpreter tool call as part of a run step.
        /// </summary>
        [Preserve]
        [JsonProperty("logs")]
        public string Logs { get; private set; }

        /// <summary>
        /// Code interpreter image output.
        /// </summary>
        [Preserve]
        [JsonProperty("image")]
        public ImageFile Image { get; private set; }
    }
}
