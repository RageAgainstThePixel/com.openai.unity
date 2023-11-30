// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class FunctionCall
    {
        [Preserve]
        [JsonConstructor]
        public FunctionCall(
            [JsonProperty("name")] string name,
            [JsonProperty("arguments")] string arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        /// <summary>
        /// The name of the function.
        /// </summary>
        [Preserve]
        [JsonProperty("name")]
        public string Name { get; }

        /// <summary>
        /// The arguments that the model expects you to pass to the function.
        /// </summary>
        [Preserve]
        [JsonProperty("arguments")]
        public string Arguments { get; }
    }
}
