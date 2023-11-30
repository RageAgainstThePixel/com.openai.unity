// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class FilePath
    {
        [Preserve]
        [JsonConstructor]
        public FilePath([JsonProperty("file_id")] string fileId)
        {
            FileId = fileId;
        }

        /// <summary>
        /// The ID of the file that was generated.
        /// </summary>
        [Preserve]
        [JsonProperty("file_id")]
        public string FileId { get; }
    }
}
