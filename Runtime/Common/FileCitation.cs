// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class FileCitation : IAnnotation
    {
        [Preserve]
        [JsonConstructor]
        internal FileCitation(
            [JsonProperty("type")] AnnotationType type,
            [JsonProperty("file_id")] string fileId,
            [JsonProperty("file_name")] string fileName,
            [JsonProperty("index")] int? index = null)
        {
            Type = type;
            FileId = fileId;
            FileName = fileName;
            Index = index;
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public AnnotationType Type { get; }

        /// <summary>
        /// The ID of the specific File the citation is from.
        /// </summary>
        [Preserve]
        [JsonProperty("file_id")]
        public string FileId { get; }

        [Preserve]
        [JsonProperty("file_name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FileName { get; }

        [Preserve]
        [JsonProperty("index", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Index { get; }
    }
}
