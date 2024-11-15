// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Files
{
    /// <summary>
    /// The File object represents a document that has been uploaded to OpenAI.
    /// </summary>
    [Preserve]
    public sealed class FileResponse : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        internal FileResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("bytes")] int? size,
            [JsonProperty("created_at")] int createdAtUnixTimeSeconds,
            [JsonProperty("filename")] string fileName,
            [JsonProperty("purpose")] string purpose)
        {
            Id = id;
            Object = @object;
            Size = size;
            CreatedAtUnixTimeSeconds = createdAtUnixTimeSeconds;
            FileName = fileName;
            Purpose = purpose;
        }

        /// <summary>
        /// The file identifier, which can be referenced in the API endpoints.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// The size of the file, in bytes.
        /// </summary>
        [Preserve]
        [JsonProperty("bytes")]
        public int? Size { get; }

        [Preserve]
        [JsonIgnore]
        public int? Bytes => Size;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the file was created.
        /// </summary>
        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixTimeSeconds { get; }

        [JsonIgnore]
        [Obsolete("Use CreatedAtUnixTimeSeconds instead.")]
        public int CreatedUnixTimeSeconds => CreatedAtUnixTimeSeconds;

        [Preserve]
        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds).DateTime;

        /// <summary>
        /// The name of the file.
        /// </summary>
        [Preserve]
        [JsonProperty("filename")]
        public string FileName { get; }

        /// <summary>
        /// The object type, which is always 'file'.
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        /// <summary>
        /// The intended purpose of the file.
        /// Supported values are 'fine-tune', 'fine-tune-results', 'assistants', and 'assistants_output'.
        /// </summary>
        [Preserve]
        [JsonProperty("purpose")]
        public string Purpose { get; }

        [Preserve]
        public static implicit operator string(FileResponse fileData) => fileData.Id;

        [Preserve]
        public override string ToString() => Id;
    }
}
