// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Utilities.WebRequestRest;

namespace OpenAI.Embeddings
{
    /// <summary>
    /// Get a vector representation of a given input that can be easily consumed by machine learning models and algorithms.<br/>
    /// <see href="https://platform.openai.com/docs/guides/embeddings"/>
    /// </summary>
    public sealed class EmbeddingsEndpoint : OpenAIBaseEndpoint
    {
        /// <inheritdoc />
        public EmbeddingsEndpoint(OpenAIClient client) : base(client) { }

        /// <inheritdoc />
        protected override string Root => "embeddings";

        /// <summary>
        /// Creates an embedding vector representing the input text.
        /// </summary>
        /// <param name="input">
        /// Input text to get embeddings for, encoded as a string or array of tokens.
        /// To get embeddings for multiple inputs in a single request, pass an array of strings or array of token arrays.
        /// Each input must not exceed 8192 tokens in length.
        /// </param>
        /// <param name="model">
        /// ID of the model to use.
        /// Defaults to: text-embedding-ada-002
        /// </param>
        /// <param name="user">
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="EmbeddingsResponse"/></returns>
        public async Task<EmbeddingsResponse> CreateEmbeddingAsync(string input, string model = null, string user = null, CancellationToken cancellationToken = default)
            => await CreateEmbeddingAsync(new EmbeddingsRequest(input, model, user), cancellationToken);

        /// <summary>
        /// Creates an embedding vector representing the input text.
        /// </summary>
        /// <param name="input">
        /// Input text to get embeddings for, encoded as a string or array of tokens.
        /// To get embeddings for multiple inputs in a single request, pass an array of strings or array of token arrays.
        /// Each input must not exceed 8192 tokens in length.
        /// </param>
        /// <param name="model">
        /// ID of the model to use.
        /// Defaults to: text-embedding-ada-002
        /// </param>
        /// <param name="user">
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="EmbeddingsResponse"/></returns>
        public async Task<EmbeddingsResponse> CreateEmbeddingAsync(IEnumerable<string> input, string model = null, string user = null, CancellationToken cancellationToken = default)
            => await CreateEmbeddingAsync(new EmbeddingsRequest(input, model, user), cancellationToken);

        /// <summary>
        /// Creates an embedding vector representing the input text.
        /// </summary>
        /// <param name="request"><see cref="EmbeddingsRequest"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="EmbeddingsResponse"/></returns>
        public async Task<EmbeddingsResponse> CreateEmbeddingAsync(EmbeddingsRequest request, CancellationToken cancellationToken = default)
        {
            var payload = JsonConvert.SerializeObject(request, OpenAIClient.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl(), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<EmbeddingsResponse>(response.Body, client);
        }
    }
}
