// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class CodeInterpreter
    {
        [Preserve]
        [JsonConstructor]
        internal CodeInterpreter(
            [JsonProperty("input")] string input,
            [JsonProperty("outputs")] IReadOnlyList<CodeInterpreterOutputs> outputs)
        {
            Input = input;
            Outputs = outputs;
        }

        /// <summary>
        /// The input to the Code Interpreter tool call.
        /// </summary>
        [Preserve]
        [JsonProperty("input")]
        public string Input { get; }

        /// <summary>
        /// The outputs from the Code Interpreter tool call.
        /// Code Interpreter can output one or more items, including text (logs) or images (image).
        /// Each of these are represented by a different object type.
        /// </summary>
        [Preserve]
        [JsonProperty("outputs")]
        public IReadOnlyList<CodeInterpreterOutputs> Outputs { get; }
    }
}
