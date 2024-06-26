// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Files;
using System.Collections.Generic;
using System.Linq;

namespace OpenAI.VectorStores
{
    public sealed class CreateVectorStoreRequest
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">
        /// Custom name for the vector store.
        /// </param>
        /// <param name="fileIds">
        /// A list of file IDs to add to the vector store.
        /// There can be a maximum of 10000 files in a vector store.
        /// </param>
        /// <param name="expiresAfter"></param>
        /// <param name="chunkingStrategy">
        /// The chunking strategy used to chunk the file(s). If not set, will use the auto strategy. Only applicable if file_ids is non-empty.
        /// </param>
        /// <param name="metadata">
        /// Optional, set of 16 key-value pairs that can be attached to a vector store.
        /// This can be useful for storing additional information about the vector store in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </param>
        public CreateVectorStoreRequest(string name = null, IReadOnlyList<string> fileIds = null, int? expiresAfter = null, ChunkingStrategy chunkingStrategy = null, IReadOnlyDictionary<string, string> metadata = null)
        {
            FileIds = fileIds;
            Name = name;
            ExpiresAfter = expiresAfter.HasValue ? new ExpirationPolicy(expiresAfter.Value) : null;
            ChunkingStrategy = chunkingStrategy;
            Metadata = metadata;
        }

        /// <inheritdoc />
        public CreateVectorStoreRequest(string name, IReadOnlyList<FileResponse> files, int? expiresAfter = null, ChunkingStrategy chunkingStrategy = null, IReadOnlyDictionary<string, string> metadata = null)
            : this(name, files?.Select(file => file.Id).ToList(), expiresAfter, chunkingStrategy, metadata)
        {
        }

        /// <summary>
        /// Custom name for the vector store.
        /// </summary>
        [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name { get; }

        /// <summary>
        /// A list of file IDs to add to the vector store.
        /// There can be a maximum of 10000 files in a vector store.
        /// </summary>
        [JsonProperty("file_ids", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<string> FileIds { get; }

        /// <summary>
        /// The expiration policy for a vector store.
        /// </summary>
        [JsonProperty("expires_after", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ExpirationPolicy ExpiresAfter { get; }

        [JsonProperty("chunking_strategy", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ChunkingStrategy ChunkingStrategy { get; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to a vector store.
        /// This can be useful for storing additional information about the vector store in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </summary>
        [JsonProperty("metadata", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyDictionary<string, string> Metadata { get; }
    }
}
