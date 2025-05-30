// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// The results of a file search tool call.
    /// </summary>
    [Preserve]
    public sealed class FileSearchToolCall : IResponseItem
    {
        [Preserve]
        [JsonConstructor]
        internal FileSearchToolCall(
            [JsonProperty("id")] string id,
            [JsonProperty("type")] ResponseItemType type,
            [JsonProperty("object")] string @object,
            [JsonProperty("status")] ResponseStatus status,
            [JsonProperty("queries")] IReadOnlyList<string> queries,
            [JsonProperty("results")] IReadOnlyList<FileSearchResult> results)
        {
            Id = id;
            Type = type;
            Object = @object;
            Status = status;
            Queries = queries;
            Results = results;
        }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Id { get; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ResponseItemType Type { get; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("object", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Object { get; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("status", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ResponseStatus Status { get; }

        /// <summary>
        /// The queries used to search for files.
        /// </summary>
        [Preserve]
        [JsonProperty("queries")]
        public IReadOnlyList<string> Queries { get; }

        /// <summary>
        /// The results of the file search tool call.
        /// </summary>
        [Preserve]
        [JsonProperty("results", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<FileSearchResult> Results { get; }
    }
}
