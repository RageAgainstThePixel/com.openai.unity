// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;

namespace OpenAI.Files
{
    public sealed class FileData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("bytes")]
        public int Size { get; set; }

        [JsonProperty("created_at")]
        public int CreatedUnixTime { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedUnixTime).DateTime;

        [JsonProperty("filename")]
        public string FileName { get; set; }

        [JsonProperty("purpose")]
        public string Purpose { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        public static implicit operator string(FileData fileData) => fileData.Id;
    }
}
