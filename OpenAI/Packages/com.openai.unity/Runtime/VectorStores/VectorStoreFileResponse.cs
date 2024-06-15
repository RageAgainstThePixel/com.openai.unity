// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.VectorStores
{
    /// <summary>
    /// A list of files attached to a vector store.
    /// </summary>
    [Preserve]
    public sealed class VectorStoreFileResponse : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        internal VectorStoreFileResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("usage_bytes")] long usageBytes,
            [JsonProperty("created_at")] int createdAtUnixTimeSeconds,
            [JsonProperty("vector_store_id")] string vectorStoreId,
            [JsonProperty("status")] VectorStoreFileStatus status,
            [JsonProperty("last_error")] Error lastError,
            [JsonProperty("chunking_strategy")] ChunkingStrategy chunkingStrategy)
        {
            Id = id;
            Object = @object;
            UsageBytes = usageBytes;
            CreatedAtUnixTimeSeconds = createdAtUnixTimeSeconds;
            VectorStoreId = vectorStoreId;
            Status = status;
            LastError = lastError;
            ChunkingStrategy = chunkingStrategy;
        }

        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// The object type, which is always 'vector_store.file'.
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        /// <summary>
        /// The total vector store usage in bytes. Note that this may be different from the original file size.
        /// </summary>
        [Preserve]
        [JsonProperty("usage_bytes")]
        public long UsageBytes { get; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the vector store file was created.
        /// </summary>
        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixTimeSeconds { get; }

        [Preserve]
        [JsonIgnore]
        public DateTimeOffset CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds);

        /// <summary>
        /// The ID of the vector store that the file is attached to.
        /// </summary>
        [Preserve]
        [JsonProperty("vector_store_id")]
        public string VectorStoreId { get; }

        /// <summary>
        /// The status of the vector store file, which can be either 'in_progress', 'completed', 'cancelled', or 'failed'.
        /// The status 'completed' indicates that the vector store file is ready for use.
        /// </summary>
        [Preserve]
        [JsonProperty("status")]
        public VectorStoreFileStatus Status { get; }

        /// <summary>
        /// The last error associated with this vector store file. Will be 'null' if there are no errors.
        /// </summary>
        [Preserve]
        [JsonProperty("last_error")]
        public Error LastError { get; }

        /// <summary>
        /// The strategy used to chunk the file.
        /// </summary>
        [Preserve]
        [JsonProperty("chunking_strategy")]
        public ChunkingStrategy ChunkingStrategy { get; }

        [Preserve]
        public override string ToString() => Id;

        [Preserve]

        public static implicit operator string(VectorStoreFileResponse response) => response?.ToString();
    }
}
