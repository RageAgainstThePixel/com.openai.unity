// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.VectorStores
{
    /// <summary>
    ///  A vector store is a collection of processed files can be used by the 'file_search' tool.
    /// </summary>
    [Preserve]
    public sealed class VectorStoreResponse : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        internal VectorStoreResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string object_,
            [JsonProperty("created_at")] int createdAtUnixTimeSeconds,
            [JsonProperty("name")] string name,
            [JsonProperty("usage_bytes")] long usageBytes,
            [JsonProperty("file_counts")] FileCounts fileCounts,
            [JsonProperty("status")] VectorStoreStatus status,
            [JsonProperty("expires_after")] ExpirationPolicy expirationPolicy,
            [JsonProperty("expires_at")] int? expiresAtUnixTimeSeconds,
            [JsonProperty("last_active_at")] int? lastActiveAtUnixTimeSeconds,
            [JsonProperty("metadata")] Dictionary<string, object> metadata)
        {
            Id = id;
            Object = object_;
            CreatedAtUnixTimeSeconds = createdAtUnixTimeSeconds;
            Name = name;
            UsageBytes = usageBytes;
            FileCounts = fileCounts;
            Status = status;
            ExpirationPolicy = expirationPolicy;
            ExpiresAtUnixTimeSeconds = expiresAtUnixTimeSeconds;
            LastActiveAtUnixTimeSeconds = lastActiveAtUnixTimeSeconds;
            Metadata = metadata;
        }

        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// The object type, which is always 'vector_store'.
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the vector store was created.
        /// </summary>
        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixTimeSeconds { get; }

        [JsonIgnore]
        public DateTimeOffset CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds);

        /// <summary>
        /// The name of the vector store.
        /// </summary>
        [Preserve]
        [JsonProperty("name")]
        public string Name { get; }

        /// <summary>
        /// The total number of bytes used by the files in the vector store.
        /// </summary>
        [Preserve]
        [JsonProperty("usage_bytes")]
        public long UsageBytes { get; }

        [Preserve]
        [JsonProperty("file_counts")]
        public FileCounts FileCounts { get; }

        /// <summary>
        /// The status of the vector store, which can be either 'expired', 'in_progress', or 'completed'.
        /// A status of 'completed' indicates that the vector store is ready for use.
        /// </summary>
        [Preserve]
        [JsonProperty("status")]
        public VectorStoreStatus Status { get; }

        /// <summary>
        /// The expiration policy for a vector store.
        /// </summary>
        [Preserve]
        [JsonProperty("expires_after")]
        public ExpirationPolicy ExpirationPolicy { get; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the vector store will expire.
        /// </summary>
        [Preserve]
        [JsonProperty("expires_at")]
        public int? ExpiresAtUnixTimeSeconds { get; }

        [JsonIgnore]
        public DateTimeOffset? ExpiresAt
            => ExpiresAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(ExpiresAtUnixTimeSeconds.Value)
                : null;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the vector store was last active.
        /// </summary>
        [Preserve]
        [JsonProperty("last_active_at")]
        public int? LastActiveAtUnixTimeSeconds { get; }

        [JsonIgnore]
        public DateTimeOffset? LastActiveAt
            => LastActiveAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(LastActiveAtUnixTimeSeconds.Value)
                : null;

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </summary>
        [Preserve]
        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; }
    }
}
