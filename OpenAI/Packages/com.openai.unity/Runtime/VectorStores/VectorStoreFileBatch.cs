using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.VectorStores
{
    /// <summary>
    /// A batch of files attached to a vector store.
    /// </summary>
    [Preserve]
    public sealed class VectorStoreFileBatch : BaseResponse
    {
        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; private set; }

        /// <summary>
        /// The object type, which is always `vector_store.file_batch`.
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; private set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the vector store files batch was created.
        /// </summary>
        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixTimeSeconds { get; private set; }

        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds).DateTime;

        /// <summary>
        /// The ID of the vector store that the files is attached to.
        /// </summary>
        [Preserve]
        [JsonProperty("vector_store_id")]
        public string VectorStoreId { get; private set; }

        /// <summary>
        /// The status of the vector store files batch, which can be either `in_progress`, `completed`, `cancelled` or `failed`.
        /// </summary>
        [Preserve]
        [JsonProperty("status")]
        public VectorStoreFileStatus Status { get; private set; }

        [Preserve]
        [JsonProperty("file_counts")]
        public FileCounts FileCounts { get; private set; }
    }
}