// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class ContainerFileCitation : IAnnotation
    {
        [Preserve]
        [JsonConstructor]
        internal ContainerFileCitation(
            [JsonProperty("type")] AnnotationType type,
            [JsonProperty("container_id")] string containerId,
            [JsonProperty("file_id")] string fileId,
            [JsonProperty("filename")] string filename,
            [JsonProperty("start_index")] int startIndex,
            [JsonProperty("end_index")] int endIndex)
        {
            Type = type;
            ContainerId = containerId;
            FileId = fileId;
            Filename = filename;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public AnnotationType Type { get; }

        /// <summary>
        /// The ID of the container file.
        /// </summary>
        [Preserve]
        [JsonProperty("container_id")]
        public string ContainerId { get; }

        /// <summary>
        /// The ID of the file.
        /// </summary>
        [Preserve]
        [JsonProperty("file_id")]
        public string FileId { get; }

        /// <summary>
        /// The filename of the container file cited.
        /// </summary>
        [Preserve]
        [JsonProperty("filename")]
        public string Filename { get; }

        /// <summary>
        /// The index of the first character of the container file citation in the message.
        /// </summary>
        [Preserve]
        [JsonProperty("start_index")]
        public int StartIndex { get; }

        /// <summary>
        /// The index of the last character of the container file citation in the message.
        /// </summary>
        [Preserve]
        [JsonProperty("end_index")]
        public int EndIndex { get; }
    }
}
