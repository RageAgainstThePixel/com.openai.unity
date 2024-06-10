// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Utilities.WebRequestRest;

namespace OpenAI.Batch
{
    /// <summary>
    /// Create large batches of API requests for asynchronous processing.
    /// The Batch API returns completions within 24 hours for a 50% discount.
    /// <see href="https://platform.openai.com/docs/api-reference/batch"/>
    /// </summary>
    public sealed class BatchEndpoint : OpenAIBaseEndpoint
    {
        public BatchEndpoint(OpenAIClient client) : base(client) { }

        protected override string Root => "batches";

        /// <summary>
        /// Creates and executes a batch from an uploaded file of requests.
        /// </summary>
        /// <param name="request"><see cref="CreateBatchRequest"/>.</param>
        /// <param name="cancellationToken">Optional <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="BatchResponse"/>.</returns>
        public async Task<BatchResponse> CreateBatchAsync(CreateBatchRequest request, CancellationToken cancellationToken = default)
        {
            var payload = JsonConvert.SerializeObject(request, OpenAIClient.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl(), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<BatchResponse>(client);
        }

        /// <summary>
        /// List your organization's batches.
        /// </summary>
        /// <param name="query"><see cref="ListQuery"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ListResponse{BatchResponse}"/>.</returns>
        public async Task<ListResponse<BatchResponse>> ListBatchesAsync(ListQuery query = null, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl(queryParameters: query), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<ListResponse<BatchResponse>>(client);
        }

        /// <summary>
        /// Retrieves a batch.
        /// </summary>
        /// <param name="batchId">The ID of the batch to retrieve.</param>
        /// <param name="cancellationToken"> Optional <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="BatchResponse"/>.</returns>
        public async Task<BatchResponse> RetrieveBatchAsync(string batchId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/{batchId}"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<BatchResponse>(client);
        }

        /// <summary>
        /// Cancels an in-progress batch.
        /// </summary>
        /// <param name="batchId"></param>
        /// <param name="cancellationToken"> Optional <see cref="CancellationToken"/>.</param>
        /// <returns>True, if the batch was cancelled, otherwise false.</returns>
        public async Task<bool> CancelBatchAsync(string batchId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.PostAsync(GetUrl($"/{batchId}/cancel"), string.Empty, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            var batch = response.Deserialize<BatchResponse>(client);

            if (batch.Status < BatchStatus.Cancelling)
            {
                try
                {
                    batch = await batch.WaitForStatusChangeAsync(cancellationToken: cancellationToken);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return batch.Status >= BatchStatus.Cancelling;
        }
    }
}
