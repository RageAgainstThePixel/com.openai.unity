// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class FileSearchResult
    {
        [Preserve]
        [JsonConstructor]
        internal FileSearchResult(
            [JsonProperty("file_id")] string fileId,
            [JsonProperty("text")] string text,
            [JsonProperty("file_name")] string fileName,
            [JsonProperty("attributes")] Dictionary<string, object> attributes,
            [JsonProperty("score")] float? score)
        {
            FileId = fileId;
            Text = text;
            FileName = fileName;
            Attributes = attributes;
            Score = score;
        }

        /// <summary>
        /// The unique ID of the file.
        /// </summary>
        [Preserve]
        [JsonProperty("file_id")]
        public string FileId { get; }

        /// <summary>
        /// The text that was retrieved from the file.
        /// </summary>
        [Preserve]
        [JsonProperty("text")]
        public string Text { get; }

        /// <summary>
        /// The name of the file.
        /// </summary>
        [Preserve]
        [JsonProperty("file_name")]
        public string FileName { get; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a
        /// structured format, and querying for objects via API or the dashboard.
        /// Keys are strings with a maximum length of 64 characters.
        /// Values are strings with a maximum length of 512 characters, booleans, or numbers.
        /// </summary>
        [Preserve]
        [JsonProperty("attributes", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyDictionary<string, object> Attributes { get; }

        /// <summary>
        /// The relevance score of the file - a value between 0 and 1.
        /// </summary>
        [Preserve]
        [JsonProperty("score", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public float? Score { get; }
    }
}
