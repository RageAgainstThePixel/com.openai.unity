// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class Annotation : IAppendable<Annotation>
    {
        [Preserve]
        public Annotation() { }

        [Preserve]
        [JsonConstructor]
        internal Annotation(
            [JsonProperty("index")] int? index,
            [JsonProperty("type")] AnnotationType annotationType,
            [JsonProperty("text")] string text,
            [JsonProperty("file_citation")] FileCitation fileCitation,
            [JsonProperty("file_path")] FilePath filePath,
            [JsonProperty("start_index")] int startIndex,
            [JsonProperty("end_index")] int endIndex)
        {
            Index = index;
            Type = annotationType;
            Text = text;
            FileCitation = fileCitation;
            FilePath = filePath;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        [Preserve]
        [JsonProperty("index")]
        public int? Index { get; private set; }

        [Preserve]
        [JsonProperty("type")]
        public AnnotationType Type { get; private set; }

        /// <summary>
        /// The text in the message content that needs to be replaced.
        /// </summary>
        [Preserve]
        [JsonProperty("text", DefaultValueHandling = DefaultValueHandling.Populate, NullValueHandling = NullValueHandling.Include)]
        public string Text { get; private set; }

        /// <summary>
        /// A citation within the message that points to a specific quote from a
        /// specific File associated with the assistant or the message.
        /// Generated when the assistant uses the 'retrieval' tool to search files.
        /// </summary>
        [Preserve]
        [JsonProperty("file_citation")]
        public FileCitation FileCitation { get; private set; }

        /// <summary>
        /// A URL for the file that's generated when the assistant used the 'code_interpreter' tool to generate a file.
        /// </summary>
        [Preserve]
        [JsonProperty("file_path")]
        public FilePath FilePath { get; private set; }

        [Preserve]
        [JsonProperty("start_index")]
        public int StartIndex { get; private set; }

        [Preserve]
        [JsonProperty("end_index")]
        public int EndIndex { get; private set; }

        public void AppendFrom(Annotation other)
        {
            if (other == null) { return; }

            if (!string.IsNullOrWhiteSpace(other.Text))
            {
                Text += other.Text;
            }

            if (other.FileCitation != null)
            {
                FileCitation = other.FileCitation;
            }

            if (other.FilePath != null)
            {
                FilePath = other.FilePath;
            }

            if (other.StartIndex > 0)
            {
                StartIndex = other.StartIndex;
            }

            if (other.EndIndex > 0)
            {
                EndIndex = other.EndIndex;
            }
        }
    }
}
