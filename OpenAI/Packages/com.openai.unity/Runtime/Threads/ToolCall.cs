// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class ToolCall
    {
        [Preserve]
        [JsonConstructor]
        public ToolCall(
            [JsonProperty("id")] string id,
            [JsonProperty("type")] string type,
            [JsonProperty("function")] FunctionCall functionCall,
            [JsonProperty("code_interpreter")] CodeInterpreter codeInterpreter,
            [JsonProperty("file_search")] IReadOnlyDictionary<string, object> fileSearch)
        {
            Id = id;
            Type = type;
            FunctionCall = functionCall;
            CodeInterpreter = codeInterpreter;
            FileSearch = fileSearch;
        }

        /// <summary>
        /// The ID of the tool call.
        /// This ID must be referenced when you submit the tool outputs in using the Submit tool outputs to run endpoint.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// The type of tool call the output is required for.
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public string Type { get; }

        /// <summary>
        /// The definition of the function that was called.
        /// </summary>
        [Preserve]
        [JsonProperty("function")]
        public FunctionCall FunctionCall { get; }

        /// <summary>
        /// The Code Interpreter tool call definition.
        /// </summary>
        [Preserve]
        [JsonProperty("code_interpreter")]
        public CodeInterpreter CodeInterpreter { get; private set; }

        /// <summary>
        /// The File Search tool call definition.
        /// </summary>
        /// <remarks>
        /// For now, this is always going to be an empty object.
        /// </remarks>
        [Preserve]
        [JsonProperty("file_search")]
        public IReadOnlyDictionary<string, object> FileSearch { get; private set; }

        [JsonIgnore]
        [Obsolete("Removed")]
        public object Retrieval { get; private set; }
    }
}
