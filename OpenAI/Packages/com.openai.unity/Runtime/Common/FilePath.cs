// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class FilePath
    {
        [Preserve]
        public FilePath(string fileId)
        {
            FileId = fileId;
        }

        [Preserve]
        [JsonConstructor]
        public FilePath(
            [JsonProperty("file_id")] string fileId,
            [JsonProperty("mime_type")] string mimeType)
        {
            FileId = fileId;
            MimeType = mimeType;
        }

        /// <summary>
        /// The ID of the file that was generated.
        /// </summary>
        [Preserve]
        [JsonProperty("file_id")]
        public string FileId { get; }

        /// <summary>
        /// The MIME type of the file.
        /// </summary>
        [Preserve]
        [JsonProperty("mime_type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string MimeType { get; }
    }
}
