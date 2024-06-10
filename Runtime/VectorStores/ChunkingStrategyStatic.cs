// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.VectorStores
{
    [Preserve]
    public sealed class ChunkingStrategyStatic
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="maxChunkSizeTokens">
        /// The maximum number of tokens in each chunk.
        /// The default value is 800.
        /// The minimum value is 100 and the maximum value is 4096.
        /// </param>
        /// <param name="chunkOverlapTokens">
        /// The number of tokens that overlap between chunks.
        /// The default value is 400.
        /// Note that the overlap must not exceed half of max_chunk_size_tokens.
        /// </param>
        [Preserve]
        [JsonConstructor]
        public ChunkingStrategyStatic(int? maxChunkSizeTokens = null, int? chunkOverlapTokens = null)
        {
            MaxChunkSizeTokens = maxChunkSizeTokens ?? 800;
            ChunkOverlapTokens = chunkOverlapTokens ?? 400;
        }

        /// <summary>
        /// The maximum number of tokens in each chunk.
        /// The default value is 800.
        /// The minimum value is 100 and the maximum value is 4096.
        /// </summary>
        [Preserve]
        [JsonProperty("max_chunk_size_tokens")]
        public int? MaxChunkSizeTokens { get; }

        /// <summary>
        /// The number of tokens that overlap between chunks.
        /// The default value is 400.
        /// Note that the overlap must not exceed half of max_chunk_size_tokens.
        /// </summary>
        [Preserve]
        [JsonProperty("chunk_overlap_tokens")]
        public int? ChunkOverlapTokens { get; }
    }
}
