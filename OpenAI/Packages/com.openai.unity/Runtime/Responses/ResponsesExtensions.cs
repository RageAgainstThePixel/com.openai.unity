// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Utilities.Async;

namespace OpenAI.Responses
{
    public static class ResponsesExtensions
    {
        public static Task<Response> UpdateAsync(this Response response, CancellationToken cancellationToken = default)
            => response.Client.ResponsesEndpoint.GetModelResponseAsync(response.Id, cancellationToken);

        public static async Task<Response> WaitForStatusChangeAsync(this Response response, int? pollingInterval = null, int? timeout = null, CancellationToken cancellationToken = default)
        {
            using CancellationTokenSource cts = timeout is < 0
                ? new CancellationTokenSource()
                : new CancellationTokenSource(TimeSpan.FromSeconds(timeout ?? 30));
            using var chainedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken);
            Response result;

            do
            {
                await Awaiters.DelayAsync(pollingInterval ?? 500, chainedCts.Token).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();
                result = await response.UpdateAsync(chainedCts.Token);
            } while (result.Status is ResponseStatus.None or ResponseStatus.Queued or ResponseStatus.InProgress or ResponseStatus.Searching);

            return result;
        }
    }
}
