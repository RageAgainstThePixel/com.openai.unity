// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;

namespace OpenAI.Files
{
    public sealed class FileData
    {
        [JsonConstructor]
        public FileData(string id, string @object, int size, int createdUnixTime, string fileName, string purpose, string status)
        {
            Id = id;
            Object = @object;
            Size = size;
            CreatedUnixTime = createdUnixTime;
            FileName = fileName;
            Purpose = purpose;
            Status = status;
        }

        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("object")]
        public string Object { get; }

        [JsonProperty("bytes")]
        public int Size { get; }

        [JsonProperty("created_at")]
        public int CreatedUnixTime { get; }

        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedUnixTime).DateTime;

        [JsonProperty("filename")]
        public string FileName { get; }

        [JsonProperty("purpose")]
        public string Purpose { get; }

        [JsonProperty("status")]
        public string Status { get; }

        public static implicit operator string(FileData fileData) => fileData.Id;
    }
}
