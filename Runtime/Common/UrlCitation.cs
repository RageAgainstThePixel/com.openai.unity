// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class UrlCitation : IAnnotation
    {
        [Preserve]
        [JsonConstructor]
        internal UrlCitation(
            [JsonProperty("type")] AnnotationType type,
            [JsonProperty("url")] string url,
            [JsonProperty("title")] string title,
            [JsonProperty("start_index")] int startIndex,
            [JsonProperty("end_index")] int endIndex)
        {
            Type = type;
            Url = url;
            Title = title;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public AnnotationType Type { get; }

        /// <summary>
        /// The URL of the web resource.
        /// </summary>
        [Preserve]
        [JsonProperty("url")]
        public string Url { get; }

        /// <summary>
        /// The title of the web resource.
        /// </summary>
        [Preserve]
        [JsonProperty("title")]
        public string Title { get; }

        /// <summary>
        /// The index of the last character of the URL citation in the message.
        /// </summary>
        [Preserve]
        [JsonProperty("start_index")]
        public int StartIndex { get; }

        /// <summary>
        /// The index of the first character of the URL citation in the message.
        /// </summary>
        [Preserve]
        [JsonProperty("end_index")]
        public int EndIndex { get; }
    }
}
