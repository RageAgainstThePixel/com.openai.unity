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
            [JsonProperty("arguments")] string arguments,
            [JsonProperty("output")] string output)
        {
            Name = name;
            Arguments = arguments;
            Output = output;
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

        /// <summary>
        /// The output of the function. This will be null if the outputs have not been submitted yet.
        /// </summary>
        [Preserve]
        [JsonProperty("output")]
        public string Output { get; }
    }
}
