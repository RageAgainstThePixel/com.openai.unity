// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class ToolCall : IAppendable<ToolCall>
    {
        [Preserve]
        public ToolCall() { }

        [Preserve]
        [JsonConstructor]
        internal ToolCall(
            [JsonProperty("index")] int? index,
            [JsonProperty("id")] string id,
            [JsonProperty("type")] string type,
            [JsonProperty("function")] FunctionCall functionCall,
            [JsonProperty("code_interpreter")] CodeInterpreter codeInterpreter,
            [JsonProperty("file_search")] IReadOnlyDictionary<string, object> fileSearch)
        {
            Index = index;
            Id = id;
            Type = type;
            FunctionCall = functionCall;
            CodeInterpreter = codeInterpreter;
            FileSearch = fileSearch;
        }

        [Preserve]
        [JsonProperty("index")]
        public int? Index { get; }

        /// <summary>
        /// The ID of the tool call.
        /// This ID must be referenced when you submit the tool outputs in using the Submit tool outputs to run endpoint.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; private set; }

        /// <summary>
        /// The type of tool call the output is required for.
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public string Type { get; private set; }

        /// <summary>
        /// The definition of the function that was called.
        /// </summary>
        [Preserve]
        [JsonProperty("function")]
        public FunctionCall FunctionCall { get; private set; }

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

        /// <summary>
        /// For now, this is always going to be an empty object.
        /// </summary>
        [JsonIgnore]
        [Obsolete("Removed")]
        public object Retrieval { get; private set; }

        public void AppendFrom(ToolCall other)
        {
            if (other == null)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(other.Id))
            {
                Id = other.Id;
            }

            if (other.FunctionCall != null)
            {
                if (FunctionCall == null)
                {
                    FunctionCall = other.FunctionCall;
                }
                else
                {
                    FunctionCall.AppendFrom(other.FunctionCall);
                }
            }

            if (other.CodeInterpreter != null)
            {
                if (CodeInterpreter == null)
                {
                    CodeInterpreter = other.CodeInterpreter;
                }
                else
                {
                    CodeInterpreter.AppendFrom(other.CodeInterpreter);
                }
            }

            if (other.FileSearch != null)
            {
                FileSearch = other.FileSearch;
            }
        }
    }
}
