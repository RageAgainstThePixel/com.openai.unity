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
        internal FunctionCall(
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
        public string Name { get; private set; }

        /// <summary>
        /// The arguments that the model expects you to pass to the function.
        /// </summary>
        [Preserve]
        [JsonProperty("arguments", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Arguments { get; private set; }

        /// <summary>
        /// The output of the function. This will be null if the outputs have not been submitted yet.
        /// </summary>
        [Preserve]
        [JsonProperty("output", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Output { get; private set; }

        internal void AppendFrom(FunctionCall other)
        {
            if (other == null) { return; }

            if (!string.IsNullOrWhiteSpace(other.Name))
            {
                Name += other.Name;
            }

            if (!string.IsNullOrWhiteSpace(other.Arguments))
            {
                Arguments += other.Arguments;
            }

            if (!string.IsNullOrWhiteSpace(other.Output))
            {
                Output += other.Output;
            }
        }
    }
}
