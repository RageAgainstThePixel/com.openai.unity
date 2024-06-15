// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.VectorStores
{
    /// <summary>
    /// A batch of files attached to a vector store.
    /// </summary>
    [Preserve]
    public sealed class VectorStoreFileBatchResponse : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        internal VectorStoreFileBatchResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("created_at")] int createdAtUnixTimeSeconds,
            [JsonProperty("vector_store_id")] string vectorStoreId,
            [JsonProperty("status")] VectorStoreFileStatus status,
            [JsonProperty("file_counts")] FileCounts fileCounts)
        {
            Id = id;
            Object = @object;
            CreatedAtUnixTimeSeconds = createdAtUnixTimeSeconds;
            VectorStoreId = vectorStoreId;
            Status = status;
            FileCounts = fileCounts;
        }

        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// The object type, which is always `vector_store.file_batch`.
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the vector store files batch was created.
        /// </summary>
        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixTimeSeconds { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds).DateTime;

        /// <summary>
        /// The ID of the vector store that the files is attached to.
        /// </summary>
        [Preserve]
        [JsonProperty("vector_store_id")]
        public string VectorStoreId { get; }

        /// <summary>
        /// The status of the vector store files batch, which can be either `in_progress`, `completed`, `cancelled` or `failed`.
        /// </summary>
        [Preserve]
        [JsonProperty("status")]
        public VectorStoreFileStatus Status { get; }

        [Preserve]
        [JsonProperty("file_counts")]
        public FileCounts FileCounts { get; }

        [Preserve]
        public override string ToString() => Id;

        [Preserve]
        public static implicit operator string(VectorStoreFileBatchResponse response) => response?.ToString();
    }
}
