// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.Scripting;
using Utilities.WebRequestRest;

namespace OpenAI.VectorStores
{
    /// <summary>
    /// The expiration policy for a vector store.
    /// </summary>
    [Preserve]
    public sealed class ExpirationPolicy
    {
        [Preserve]
        public static implicit operator ExpirationPolicy(int days) => new(days);

        [Preserve]
        [JsonConstructor]
        public ExpirationPolicy(
            [JsonProperty("anchor")] string anchor,
            [JsonProperty("days")] int days)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="days">
        /// The number of days after the anchor time that the vector store will expire.
        /// </param>
        public ExpirationPolicy(int days)
        {
            Days = days;
        }

        /// <summary>
        /// Anchor timestamp after which the expiration policy applies.
        /// Supported anchors: 'last_active_at'.
        /// </summary>
        [Preserve]
        [JsonProperty("anchor")]
        public string Anchor { get; } = "last_active_at";

        /// <summary>
        /// The number of days after the anchor time that the vector store will expire.
        /// </summary>
        [Preserve]
        [JsonProperty("days")]
        public int Days { get; }
    }

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
    /// <summary>
    /// A list of files attached to a vector store.
    /// </summary>
    public sealed class VectorStoreFileResponse : BaseResponse
    {
        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; private set; }

        /// <summary>
        /// The object type, which is always 'vector_store.file'.
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; private set; }

        /// <summary>
        /// The total vector store usage in bytes. Note that this may be different from the original file size.
        /// </summary>
        [Preserve]
        [JsonProperty("usage_bytes")]
        public long UsageBytes { get; private set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the vector store file was created.
        /// </summary>
        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixTimeSeconds { get; private set; }

        [JsonIgnore]
        public DateTimeOffset CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds);

        /// <summary>
        /// The ID of the vector store that the file is attached to.
        /// </summary>
        [Preserve]
        [JsonProperty("vector_store_id")]
        public string VectorStoreId { get; private set; }

        /// <summary>
        /// The status of the vector store file, which can be either 'in_progress', 'completed', 'cancelled', or 'failed'.
        /// The status 'completed' indicates that the vector store file is ready for use.
        /// </summary>
        [Preserve]
        [JsonProperty("status")]
        public VectorStoreFileStatus Status { get; private set; }

        /// <summary>
        /// The last error associated with this vector store file. Will be 'null' if there are no errors.
        /// </summary>
        [Preserve]
        [JsonProperty("last_error")]
        public Error LastError { get; private set; }
    }

    public enum VectorStoreFileStatus
    {
        NotStarted = 0,
        [EnumMember(Value = "in_progress")]
        InProgress,
        [EnumMember(Value = "completed")]
        Completed,
        [EnumMember(Value = "cancelled")]
        Cancelled,
        [EnumMember(Value = "failed")]
        Expired
    }

    /// <summary>
    ///  A vector store is a collection of processed files can be used by the 'file_search' tool.
    /// </summary>
    public sealed class VectorStoreResponse : BaseResponse
    {
        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; private set; }

        /// <summary>
        /// The object type, which is always 'vector_store'.
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; private set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the vector store was created.
        /// </summary>
        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixTimeSeconds { get; private set; }

        [JsonIgnore]
        public DateTimeOffset CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds);

        /// <summary>
        /// The name of the vector store.
        /// </summary>
        [Preserve]
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// The total number of bytes used by the files in the vector store.
        /// </summary>
        [Preserve]
        [JsonProperty("usage_bytes")]
        public long UsageBytes { get; private set; }

        [Preserve]
        [JsonProperty("file_counts")]
        public FileCounts FileCounts { get; private set; }

        /// <summary>
        /// The status of the vector store, which can be either 'expired', 'in_progress', or 'completed'.
        /// A status of 'completed' indicates that the vector store is ready for use.
        /// </summary>
        [Preserve]
        [JsonProperty("status")]
        public VectorStoreStatus Status { get; private set; }

        /// <summary>
        /// The expiration policy for a vector store.
        /// </summary>
        [Preserve]
        [JsonProperty("expires_after")]
        public ExpirationPolicy ExpirationPolicy { get; private set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the vector store will expire.
        /// </summary>
        [Preserve]
        [JsonProperty("expires_at")]
        public int? ExpiresAtUnixTimeSeconds { get; private set; }

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
        public int? LastActiveAtUnixTimeSeconds { get; private set; }

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
        public Dictionary<string, object> Metadata { get; private set; }
    }
    public enum VectorStoreStatus
    {
        NotStarted = 0,
        [EnumMember(Value = "in_progress")]
        InProgress,
        [EnumMember(Value = "completed")]
        Completed,
        [EnumMember(Value = "failed")]
        Expired
    }
    /// <summary>
    /// Vector stores are used to store files for use by the file_search tool.
    /// <see href="https://platform.openai.com/docs/api-reference/vector-stores"/>
    /// </summary>
    public sealed class VectorStoresEndpoint : OpenAIBaseEndpoint
    {
        public VectorStoresEndpoint(OpenAIClient client) : base(client) { }

        protected override string Root => "vector_stores";

        /// <summary>
        /// Creates a new Vector Store.
        /// </summary>
        /// <param name="fileIds">
        /// A list of file ids that the vector store should use.
        /// Useful for tools like 'file_search' that can access files.
        /// </param>
        /// <param name="name">
        /// Optional, name of the vector store.
        /// </param>
        /// <param name="expiresAfter">
        /// The number of days after the anchor time that the vector store will expire.
        /// </param>
        /// <param name="metadata">
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </param>
        /// <param name="cancellationToken">Optional <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="VectorStoreResponse"/>.</returns>
        public async Task<VectorStoreResponse> CreateVectorStoreAsync(IReadOnlyList<string> fileIds, string name = null, int? expiresAfter = null, IReadOnlyDictionary<string, object> metadata = null, CancellationToken cancellationToken = default)
        {
            if (fileIds is not { Count: not 0 }) { throw new ArgumentNullException(nameof(fileIds)); }
            var expirationPolicy = expiresAfter.HasValue ? new ExpirationPolicy(expiresAfter.Value) : null;
            var request = new { file_ids = fileIds, name, expires_after = expirationPolicy, metadata };
            var jsonContent = JsonConvert.SerializeObject(request, OpenAIClient.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl(), jsonContent, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<VectorStoreResponse>(client);
        }

        /// <summary>
        /// Returns a list of vector stores.
        /// </summary>
        /// <param name="query">Optional, <see cref="ListQuery"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ListResponse{VectorStoreResponse}"/>.</returns>
        public async Task<ListResponse<VectorStoreResponse>> ListVectorStoresAsync(ListQuery query = null, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl(queryParameters: query), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<ListResponse<VectorStoreResponse>>(client);
        }

        /// <summary>
        /// Get a vector store.
        /// </summary>
        /// <param name="vectorStoreId">
        /// The ID of the vector store to retrieve.
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="VectorStoreResponse"/>.</returns>
        public async Task<VectorStoreResponse> GetVectorStoreAsync(string vectorStoreId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/{vectorStoreId}"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<VectorStoreResponse>(client);
        }

        /// <summary>
        /// Modifies a vector store.
        /// </summary>
        /// <param name="vectorStoreId">
        /// The ID of the vector store to retrieve.
        /// </param>
        /// <param name="name">
        /// Optional, name of the vector store.
        /// </param>
        /// <param name="expiresAfter">
        /// The number of days after the anchor time that the vector store will expire.
        /// </param>
        /// <param name="metadata">
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="VectorStoreResponse"/>.</returns>
        public async Task<VectorStoreResponse> UpdateVectorStoreAsync(string vectorStoreId, string name = null, int? expiresAfter = null, IReadOnlyDictionary<string, object> metadata = null, CancellationToken cancellationToken = default)
        {
            var expirationPolicy = expiresAfter.HasValue ? new ExpirationPolicy(expiresAfter.Value) : null;
            var request = new { name, expires_after = expirationPolicy, metadata };
            var jsonContent = JsonConvert.SerializeObject(request, OpenAIClient.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl($"/{vectorStoreId}"), jsonContent, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<VectorStoreResponse>(client);
        }

        /// <summary>
        /// Delete a vector store.
        /// </summary>
        /// <param name="vectorStoreId">
        /// The ID of the vector store to retrieve.
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>True, if the vector store was successfully deleted.</returns>
        public async Task<bool> DeleteVectorStoreAsync(string vectorStoreId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.DeleteAsync(GetUrl($"/{vectorStoreId}"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<DeletedResponse>(client)?.Deleted ?? false;
        }

        #region Files

        /// <summary>
        /// Create a vector store file by attaching a File to a vector store.
        /// </summary>
        /// <param name="vectorStoreId">The ID of the vector store that the file belongs to.</param>
        /// <param name="fileId">
        /// A File ID that the vector store should use.
        /// Useful for tools like file_search that can access files.
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="VectorStoreFileResponse"/>.</returns>
        public async Task<VectorStoreFileResponse> CreateVectorStoreFileAsync(string vectorStoreId, string fileId, CancellationToken cancellationToken = default)
        {
            var jsonContent = JsonConvert.SerializeObject(new { file_id = fileId }, OpenAIClient.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl($"/{vectorStoreId}/files"), jsonContent, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<VectorStoreFileResponse>(client);
        }

        /// <summary>
        /// Returns a list of vector store files.
        /// </summary>
        /// <param name="vectorStoreId">The ID of the vector store that the file belongs to.</param>
        /// <param name="query">Optional, <see cref="ListQuery"/>.</param>
        /// <param name="filter">Optional, Filter by file status <see cref="VectorStoreFileStatus"/> filter.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ListResponse{VectorStoreFileResponse}"/>.</returns>
        public async Task<ListResponse<VectorStoreFileResponse>> ListVectorStoreFilesAsync(string vectorStoreId, ListQuery query = null, VectorStoreFileStatus? filter = null, CancellationToken cancellationToken = default)
        {
            Dictionary<string, string> queryParams = query;

            if (filter.HasValue)
            {
                queryParams ??= new();
                queryParams.Add("filter", $"{filter.Value}");
            }

            var response = await Rest.GetAsync(GetUrl($"/{vectorStoreId}/files", queryParams), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<ListResponse<VectorStoreFileResponse>>(client);
        }

        /// <summary>
        /// Retrieves a vector store file.
        /// </summary>
        /// <param name="vectorStoreId">The ID of the vector store that the file belongs to.</param>
        /// <param name="fileId">The ID of the file being retrieved.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="VectorStoreFileResponse"/>.</returns>
        public async Task<VectorStoreFileResponse> GetVectorStoreFileAsync(string vectorStoreId, string fileId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/{vectorStoreId}/files/{fileId}"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<VectorStoreFileResponse>(client);
        }

        /// <summary>
        /// Delete a vector store file.
        /// This will remove the file from the vector store but the file itself will not be deleted.
        /// To delete the file, use the delete file endpoint.
        /// </summary>
        /// <param name="vectorStoreId">The ID of the vector store that the file belongs to.</param>
        /// <param name="fileId">The ID of the file being deleted.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>True, if the vector store file was successfully deleted.</returns>
        public async Task<bool> DeleteVectorStoreFileAsync(string vectorStoreId, string fileId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.DeleteAsync(GetUrl($"/{vectorStoreId}/files/{fileId}"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<DeletedResponse>(client)?.Deleted ?? false;
        }

        #endregion Files

        #region Batches

        /// <summary>
        /// Create a vector store file batch.
        /// </summary>
        /// <param name="vectorStoreId">
        /// The ID of the vector store for which to create a File Batch.
        /// </param>
        /// <param name="fileIds">
        /// A list of File IDs that the vector store should use. Useful for tools like file_search that can access files.
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="VectorStoreFileBatch"/>.</returns>
        public async Task<VectorStoreFileBatch> CreateVectorStoreFileBatch(string vectorStoreId, IReadOnlyList<string> fileIds, CancellationToken cancellationToken = default)
        {
            if (fileIds is not { Count: not 0 }) { throw new ArgumentNullException(nameof(fileIds)); }
            var jsonContent = JsonConvert.SerializeObject(new { file_ids = fileIds }, OpenAIClient.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl($"/{vectorStoreId}/file_batches"), jsonContent, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            return response.Deserialize<VectorStoreFileBatch>(client);
        }

        /// <summary>
        /// Returns a list of vector store files in a batch.
        /// </summary>
        /// <param name="vectorStoreId">The ID of the vector store that the files belong to.</param>
        /// <param name="query">Optional, <see cref="ListQuery"/>.</param>
        /// <param name="filter">Optional, filter by file status.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ListResponse{VectorStoreFileBatch}"/>.</returns>
        public async Task<ListResponse<VectorStoreFileBatch>> ListVectorStoreFileBatchesAsync(string vectorStoreId, ListQuery query = null, VectorStoreFileStatus? filter = null, CancellationToken cancellationToken = default)
        {
            Dictionary<string, string> queryParams = query;

            if (filter != null)
            {
                queryParams ??= new();
                queryParams.Add("filter", $"{filter.Value}");
            }

            var response = await Rest.GetAsync(GetUrl($"/{vectorStoreId}/file_batches", queryParams), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<ListResponse<VectorStoreFileBatch>>(client);
        }

        /// <summary>
        /// Retrieves a vector store file batch.
        /// </summary>
        /// <param name="vectorStoreId">The ID of the vector store that the files belong to.</param>
        /// <param name="fileBatchId">The ID of the file batch being retrieved.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="VectorStoreFileBatch"/>.</returns>
        public async Task<VectorStoreFileBatch> GetVectorStoreFileBatchAsync(string vectorStoreId, string fileBatchId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/{vectorStoreId}/file_batches/{fileBatchId}"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<VectorStoreFileBatch>(client);
        }

        /// <summary>
        /// Cancel a vector store file batch.
        /// This attempts to cancel the processing of files in this batch as soon as possible.
        /// </summary>
        /// <param name="vectorStoreId">The ID of the vector store that the files belong to.</param>
        /// <param name="fileBatchId">The ID of the file batch being retrieved.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>True, if the vector store file batch was cancelled, otherwise false.</returns>
        public async Task<bool> CancelVectorStoreFileBatchAsync(string vectorStoreId, string fileBatchId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.PostAsync(GetUrl($"/{vectorStoreId}/file_batches/{fileBatchId}/cancel"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            var result = response.Deserialize<VectorStoreFileBatch>(client);
            return result.Status == VectorStoreFileStatus.Cancelled;
        }

        #endregion Batches
    }
}
