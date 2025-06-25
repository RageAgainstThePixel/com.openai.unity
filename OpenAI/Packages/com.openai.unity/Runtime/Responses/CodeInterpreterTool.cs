// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A tool that runs Python code to help generate a response to a prompt.
    /// </summary>
    [Preserve]
    public sealed class CodeInterpreterTool : ITool
    {
        [Preserve]
        public static implicit operator Tool(CodeInterpreterTool codeInterpreterTool) => new(codeInterpreterTool as ITool);

        [Preserve]
        public CodeInterpreterTool(string containerId)
        {
            Container = containerId;
        }

        [Preserve]
        public CodeInterpreterTool(IEnumerable<string> fileIds)
        {
            Container = new CodeInterpreterContainer(fileIds);
        }

        [Preserve]
        [JsonConstructor]
        internal CodeInterpreterTool(
            [JsonProperty("type")] string type,
            [JsonProperty("container")][JsonConverter(typeof(StringOrObjectConverter<CodeInterpreterContainer>))] object container)
        {
            Type = type;
            Container = container;
        }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; } = "code_interpreter";

        /// <summary>
        /// The code interpreter container. Can be a container ID or an object that specifies uploaded file IDs to make available to your code.
        /// </summary>
        [Preserve]
        [JsonProperty("container")]
        [JsonConverter(typeof(StringOrObjectConverter<CodeInterpreterContainer>))]
        public object Container { get; }
    }
}
