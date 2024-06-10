// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI
{
    /// <summary>
    /// <see cref="Tool.CodeInterpreter"/> resources.
    /// </summary>
    [Preserve]
    public sealed class CodeInterpreterResources
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileIds">
        /// A list of file IDs made available to the <see cref="Tool.CodeInterpreter"/> tool.
        /// There can be a maximum of 20 files associated with the tool.
        /// </param>
        [Preserve]
        public CodeInterpreterResources(IReadOnlyList<string> fileIds)
        {
            FileIds = fileIds;
        }

        /// <summary>
        /// A list of file IDs made available to the <see cref="Tool.CodeInterpreter"/> tool.
        /// There can be a maximum of 20 files associated with the tool.
        /// </summary>
        [Preserve]
        [JsonProperty("file_ids")]
        public IReadOnlyList<string> FileIds { get; private set; }

        [Preserve]
        public static implicit operator CodeInterpreterResources(List<string> fileIds) => new(fileIds);
    }
}
