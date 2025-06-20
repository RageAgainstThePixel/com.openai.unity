// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Utilities.Async;

namespace OpenAI.Batch
{
    public static class BatchExtensions
    {
        /// <summary>
        /// Get the latest status of the <see cref="BatchResponse"/>.
        /// </summary>
        /// <param name="batchResponse"><see cref="BatchResponse"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="BatchResponse"/>.</returns>
        public static Task<BatchResponse> UpdateAsync(this BatchResponse batchResponse, CancellationToken cancellationToken = default)
            => batchResponse.Client.BatchEndpoint.RetrieveBatchAsync(batchResponse.Id, cancellationToken);

        /// <summary>
        /// Waits for <see cref="BatchResponse.Status"/> to change.
        /// </summary>
        /// <param name="batchResponse"><see cref="BatchResponse"/>.</param>
        /// <param name="pollingInterval">Optional, time in milliseconds to wait before polling status.</param>
        /// <param name="timeout">Optional, timeout in seconds to cancel polling.<br/>Defaults to 30 seconds.<br/>Set to -1 for indefinite.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="BatchResponse"/>.</returns>
        public static async Task<BatchResponse> WaitForStatusChangeAsync(this BatchResponse batchResponse, int? pollingInterval = null, int? timeout = null, CancellationToken cancellationToken = default)
        {
            using CancellationTokenSource cts = timeout is < 0
                ? new CancellationTokenSource()
                : new CancellationTokenSource(TimeSpan.FromSeconds(timeout ?? 30));
            using var chainedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken);
            BatchResponse result;
            do
            {
                await Awaiters.DelayAsync(pollingInterval ?? 500, chainedCts.Token).ConfigureAwait(true);
                result = await batchResponse.UpdateAsync(cancellationToken: chainedCts.Token);
            } while (result.Status is BatchStatus.NotStarted or BatchStatus.InProgress or BatchStatus.Cancelling);
            return result;
        }
    }
}
