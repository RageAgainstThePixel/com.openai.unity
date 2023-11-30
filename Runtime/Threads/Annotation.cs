// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class Annotation
    {
        [Preserve]
        [JsonConstructor]
        public Annotation(
            [JsonProperty("type")] AnnotationType annotationType,
            [JsonProperty("text")] string text,
            [JsonProperty("file_citation")] FileCitation fileCitation,
            [JsonProperty("file_path")] FilePath filePath,
            [JsonProperty("start_index")] int startIndex,
            [JsonProperty("end_index")] int endIndex)
        {
            Type = annotationType;
            Text = text;
            FileCitation = fileCitation;
            FilePath = filePath;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        [Preserve]
        [JsonProperty("type")]
        public AnnotationType Type { get; }

        /// <summary>
        /// The text in the message content that needs to be replaced.
        /// </summary>
        [Preserve]
        [JsonProperty("text")]
        public string Text { get; }

        /// <summary>
        /// A citation within the message that points to a specific quote from a
        /// specific File associated with the assistant or the message.
        /// Generated when the assistant uses the 'retrieval' tool to search files.
        /// </summary>
        [Preserve]
        [JsonProperty("file_citation")]
        public FileCitation FileCitation { get; }

        /// <summary>
        /// A URL for the file that's generated when the assistant used the 'code_interpreter' tool to generate a file.
        /// </summary>
        [Preserve]
        [JsonProperty("file_path")]
        public FilePath FilePath { get; }

        [Preserve]
        [JsonProperty("start_index")]
        public int StartIndex { get; }

        [Preserve]
        [JsonProperty("end_index")]
        public int EndIndex { get; }
    }
}
