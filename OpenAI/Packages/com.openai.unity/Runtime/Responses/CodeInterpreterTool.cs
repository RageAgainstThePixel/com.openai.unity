// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System.Collections.Generic;

namespace OpenAI.Responses
{
    /// <summary>
    /// A tool that runs Python code to help generate a response to a prompt.
    /// </summary>
    public sealed class CodeInterpreterTool : ITool
    {
        public static implicit operator Tool(CodeInterpreterTool codeInterpreterTool) => new(codeInterpreterTool as ITool);

        public CodeInterpreterTool(string containerId)
        {
            Container = containerId;
        }

        public CodeInterpreterTool(IEnumerable<string> fileIds)
        {
            Container = new CodeInterpreterContainer(fileIds);
        }

        [JsonProperty("type")]
        public string Type => "code_interpreter";

        /// <summary>
        /// The code interpreter container. Can be a container ID or an object that specifies uploaded file IDs to make available to your code.
        /// </summary>
        [JsonProperty("container")]
        [JsonConverter(typeof(StringOrObjectConverter<CodeInterpreterContainer>))]
        public object Container { get; }
    }
}
