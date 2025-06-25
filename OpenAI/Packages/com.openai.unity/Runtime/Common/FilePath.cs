// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class FilePath : IAnnotation
    {
        [Preserve]
        [JsonConstructor]
        internal FilePath(
            [JsonProperty("type")] AnnotationType type,
            [JsonProperty("file_id")] string fileId,
            [JsonProperty("mime_type")] string mimeType,
            [JsonProperty("index")] int? index)
        {
            Type = type;
            FileId = fileId;
            MimeType = mimeType;
            Index = index;
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public AnnotationType Type { get; }

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

        [Preserve]
        [JsonProperty("index", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Index { get; }
    }
}
