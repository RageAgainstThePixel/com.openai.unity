// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class Attachment
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileId">The ID of the file to attach to the message.</param>
        /// <param name="tool">The tool to add this file to.</param>
        [Preserve]
        public Attachment(string fileId, Tool tool) : this(fileId, new[] { tool }) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileId">The ID of the file to attach to the message.</param>
        /// <param name="tools">The tools to add this file to.</param>
        [Preserve]
        [JsonConstructor]
        public Attachment(
            [JsonProperty("file_id")] string fileId,
            [JsonProperty("tools")] IEnumerable<Tool> tools)
        {
            FileId = fileId;
            Tools = tools?.ToList();
        }

        /// <summary>
        /// The ID of the file to attach to the message.
        /// </summary>
        [Preserve]
        [JsonProperty("file_id")]
        public string FileId { get; private set; }

        /// <summary>
        /// The tools to add this file to.
        /// </summary>
        [Preserve]
        [JsonProperty("tools")]
        public IReadOnlyList<Tool> Tools { get; private set; }
    }
}
