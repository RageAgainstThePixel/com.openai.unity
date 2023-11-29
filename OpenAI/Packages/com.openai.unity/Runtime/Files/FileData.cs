// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Files
{
    [Preserve]
    [Obsolete("Use FileResponse")]
    public sealed class FileData
    {
        [Preserve]
        [JsonConstructor]
        public FileData(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("bytes")] int size,
            [JsonProperty("created_at")] int createdUnixTime,
            [JsonProperty("filename")] string fileName,
            [JsonProperty("purpose")] string purpose,
            [JsonProperty("status")] string status)
        {
            Id = id;
            Object = @object;
            Size = size;
            CreatedUnixTime = createdUnixTime;
            FileName = fileName;
            Purpose = purpose;
            Status = status;
        }

        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        [Preserve]
        [JsonProperty("bytes")]
        public int Size { get; }

        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedUnixTime { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedUnixTime).DateTime;

        [Preserve]
        [JsonProperty("filename")]
        public string FileName { get; }

        [Preserve]
        [JsonProperty("purpose")]
        public string Purpose { get; }

        [Preserve]
        [JsonProperty("status")]
        public string Status { get; }

        public static implicit operator string(FileData fileData) => fileData.Id;
    }
}
