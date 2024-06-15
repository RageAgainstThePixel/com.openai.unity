// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    /// <summary>
    /// A set of resources to be used by Assistants and Threads.
    /// The resources are specific to the type of tool.
    /// For example, the <see cref="Tool.CodeInterpreter"/> requres a list of file ids,
    /// While the <see cref="Tool.FileSearch"/> requires a list vector store ids.
    /// </summary>
    [Preserve]
    public sealed class ToolResources
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileSearch"><see cref="FileSearchResources"/>.</param>
        /// <param name="codeInterpreter"><see cref="CodeInterpreterResources"/>.</param>
        [Preserve]
        public ToolResources(FileSearchResources fileSearch = null, CodeInterpreterResources codeInterpreter = null)
            : this(codeInterpreter, fileSearch)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="codeInterpreter"><see cref="CodeInterpreterResources"/>.</param>
        /// <param name="fileSearch"><see cref="FileSearchResources"/>.</param>
        [Preserve]
        [JsonConstructor]
        public ToolResources(
            [JsonProperty("code_interpreter")] CodeInterpreterResources codeInterpreter = null,
            [JsonProperty("file_search")] FileSearchResources fileSearch = null)
        {
            CodeInterpreter = codeInterpreter;
            FileSearch = fileSearch;
        }

        [Preserve]
        [JsonProperty("code_interpreter")]
        public CodeInterpreterResources CodeInterpreter { get; private set; }

        [Preserve]
        [JsonProperty("file_search")]
        public FileSearchResources FileSearch { get; private set; }

        [Preserve]
        public static implicit operator ToolResources(FileSearchResources fileSearch) => new(fileSearch);

        [Preserve]
        public static implicit operator ToolResources(CodeInterpreterResources codeInterpreter) => new(codeInterpreter);

        [Preserve]
        public static implicit operator ToolResources(VectorStoreRequest vectorStoreRequest) => new(new FileSearchResources(vectorStoreRequest));
    }
}
