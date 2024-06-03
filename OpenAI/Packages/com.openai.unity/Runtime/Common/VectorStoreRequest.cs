// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI
{
    /// <summary>
    /// A helper to create a vector store with file_ids and attach it to an assistant/thread.
    /// There can be a maximum of 1 vector store attached to the assistant/thread.
    /// </summary>
    [Preserve]
    public sealed class VectorStoreRequest
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileIds">
        /// A list of file IDs to add to the vector store.
        /// There can be a maximum of 10000 files in a vector store.
        /// </param>
        /// <param name="metadata">
        /// Optional, set of 16 key-value pairs that can be attached to a vector store.
        /// This can be useful for storing additional information about the vector store in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </param>
        [Preserve]
        public VectorStoreRequest(IReadOnlyList<string> fileIds, IReadOnlyDictionary<string, string> metadata = null)
        {
            FileIds = fileIds;
            Metadata = metadata;
        }

        /// <summary>
        /// A list of file IDs to add to the vector store.
        /// There can be a maximum of 10000 files in a vector store.
        /// </summary>
        [Preserve]
        [JsonProperty("file_ids")]
        public IReadOnlyList<string> FileIds { get; private set; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to a vector store.
        /// This can be useful for storing additional information about the vector store in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </summary>
        [Preserve]
        [JsonProperty("metadata")]
        public IReadOnlyDictionary<string, string> Metadata { get; private set; }

        public static implicit operator VectorStoreRequest(List<string> fileIds) => new(fileIds);
    }
}
