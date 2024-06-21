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
            [JsonProperty("outputs")] List<CodeInterpreterOutputs> outputs)
        {
            Input = input;
            this.outputs = outputs;
        }

        /// <summary>
        /// The input to the Code Interpreter tool call.
        /// </summary>
        [Preserve]
        [JsonProperty("input", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Input { get; private set; }

        private List<CodeInterpreterOutputs> outputs;

        /// <summary>
        /// The outputs from the Code Interpreter tool call.
        /// Code Interpreter can output one or more items, including text (logs) or images (image).
        /// Each of these are represented by a different object type.
        /// </summary>
        [Preserve]
        [JsonProperty("outputs", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<CodeInterpreterOutputs> Outputs => outputs;

        internal void AppendFrom(CodeInterpreter other)
        {
            if (other == null) { return; }

            if (!string.IsNullOrWhiteSpace(other.Input))
            {
                Input += other.Input;
            }

            if (other.Outputs != null)
            {
                outputs ??= new List<CodeInterpreterOutputs>();
                outputs.AddRange(other.Outputs);
            }
        }
    }
}
