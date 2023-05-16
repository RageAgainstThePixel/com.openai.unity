// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Utilities.Async;
using Utilities.Rest.Extensions;

namespace OpenAI.FineTuning
{
    /// <summary>
    /// Manage fine-tuning jobs to tailor a model to your specific training data.<br/>
    /// <see href="https://platform.openai.com/docs/guides/fine-tuning"/>
    /// </summary>
    public sealed class FineTuningEndpoint : OpenAIBaseEndpoint
    {
        private class FineTuneList
        {
            [JsonProperty("object")]
            public string Object { get; set; }

            [JsonProperty("data")]
            public List<FineTuneJob> Data { get; set; }
        }

        private class FineTuneEventList
        {
            [JsonProperty("data")]
            public List<Event> Data { get; set; }
        }

        /// <inheritdoc />
        public FineTuningEndpoint(OpenAIClient client) : base(client) { }

        /// <inheritdoc />
        protected override string Root => "fine-tunes";

        /// <summary>
        /// Creates a job that fine-tunes a specified model from a given dataset.
        /// Response includes details of the enqueued job including job status and
        /// the name of the fine-tuned models once complete.
        /// </summary>
        /// <param name="jobRequest"><see cref="CreateFineTuneJobRequest"/>.</param>
        /// <returns><see cref="FineTuneJob"/>.</returns>
        /// <exception cref="HttpRequestException">.</exception>
        public async Task<FineTuneJob> CreateFineTuneJobAsync(CreateFineTuneJobRequest jobRequest)
        {
            var jsonContent = JsonConvert.SerializeObject(jobRequest, client.JsonSerializationOptions);
            var response = await client.Client.PostAsync(GetUrl(), jsonContent.ToJsonStringContent());
            var responseAsString = await response.ReadAsStringAsync();
            return response.DeserializeResponse<FineTuneJobResponse>(responseAsString, client.JsonSerializationOptions);
        }

        /// <summary>
        /// List your organization's fine-tuning jobs.
        /// </summary>
        /// <returns>List of <see cref="FineTuneJob"/>s.</returns>
        /// <exception cref="HttpRequestException">.</exception>
        public async Task<IReadOnlyList<FineTuneJob>> ListFineTuneJobsAsync()
        {
            var response = await client.Client.GetAsync(GetUrl());
            var responseAsString = await response.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FineTuneList>(responseAsString, client.JsonSerializationOptions)?.Data.OrderBy(job => job.CreatedAtUnixTime).ToArray();
        }

        /// <summary>
        /// Gets info about the fine-tune job.
        /// </summary>
        /// <param name="jobId"><see cref="FineTuneJob.Id"/>.</param>
        /// <returns><see cref="FineTuneJobResponse"/>.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<FineTuneJob> RetrieveFineTuneJobInfoAsync(string jobId)
        {
            var response = await client.Client.GetAsync(GetUrl($"/{jobId}"));
            var responseAsString = await response.ReadAsStringAsync();
            return response.DeserializeResponse<FineTuneJobResponse>(responseAsString, client.JsonSerializationOptions);
        }

        /// <summary>
        /// Immediately cancel a fine-tune job.
        /// </summary>
        /// <param name="jobId"><see cref="FineTuneJob.Id"/> to cancel.</param>
        /// <returns><see cref="FineTuneJobResponse"/>.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<bool> CancelFineTuneJobAsync(string jobId)
        {
            var response = await client.Client.PostAsync(GetUrl($"/{jobId}/cancel"), null!);
            var responseAsString = await response.ReadAsStringAsync();
            var result = response.DeserializeResponse<FineTuneJobResponse>(responseAsString, client.JsonSerializationOptions);
            const string cancelled = "cancelled";
            return result.Status == cancelled;
        }

        /// <summary>
        /// Get fine-grained status updates for a fine-tune job.
        /// </summary>
        /// <param name="jobId"><see cref="FineTuneJob.Id"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>List of events for <see cref="FineTuneJob"/>.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IReadOnlyList<Event>> ListFineTuneEventsAsync(string jobId, CancellationToken cancellationToken = default)
        {
            var response = await client.Client.GetAsync(GetUrl($"/{jobId}/events"), cancellationToken);
            var responseAsString = await response.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FineTuneEventList>(responseAsString, client.JsonSerializationOptions)?.Data.OrderBy(@event => @event.CreatedAtUnixTime).ToArray();
        }

        /// <summary>
        /// Stream the fine-grained status updates for a fine-tune job.
        /// </summary>
        /// <param name="jobId"><see cref="FineTuneJob.Id"/>.</param>
        /// <param name="fineTuneEventCallback">The event callback handler.</param>
        /// <param name="cancelJob">Optional, Cancel the job if streaming is aborted. Default is false.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <exception cref="HttpRequestException"></exception>
        public async Task StreamFineTuneEventsAsync(string jobId, Action<Event> fineTuneEventCallback, bool cancelJob = false, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, GetUrl($"/{jobId}/events?stream=true"));
            var response = await client.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            await response.CheckResponseAsync();
            await using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            while (!cancellationToken.IsCancellationRequested &&
                   await reader.ReadLineAsync() is { } streamData)
            {
                if (streamData.TryGetEventStreamData(out var eventData))
                {
                    if (!string.IsNullOrWhiteSpace(eventData))
                    {
                        // Always raise event callbacks on main thread
                        await Awaiters.UnityMainThread;
                        fineTuneEventCallback(JsonConvert.DeserializeObject<Event>(eventData, client.JsonSerializationOptions));
                    }
                }
                else
                {
                    break;
                }
            }

            if (cancellationToken.IsCancellationRequested && cancelJob)
            {
                var isCancelled = await CancelFineTuneJobAsync(jobId);

                if (!isCancelled)
                {
                    throw new Exception($"Failed to cancel {jobId}");
                }
            }

            cancellationToken.ThrowIfCancellationRequested();
        }

        /// <summary>
        /// Stream the fine-grained status updates for a fine-tune job.
        /// </summary>
        /// <param name="jobId"><see cref="FineTuneJob.Id"/>.</param>
        /// <param name="cancelJob">Optional, Cancel the job if streaming is aborted. Default is false.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <exception cref="HttpRequestException"></exception>
        public async IAsyncEnumerable<Event> StreamFineTuneEventsEnumerableAsync(string jobId, bool cancelJob = false, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, GetUrl($"/{jobId}/events?stream=true"));
            var response = await client.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            await response.CheckResponseAsync();
            await using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            while (!cancellationToken.IsCancellationRequested &&
                   await reader.ReadLineAsync() is { } streamData)
            {
                if (streamData.TryGetEventStreamData(out var eventData))
                {
                    if (!string.IsNullOrWhiteSpace(eventData))
                    {
                        yield return JsonConvert.DeserializeObject<Event>(eventData, client.JsonSerializationOptions);
                    }
                }
                else
                {
                    break;
                }
            }

            if (cancellationToken.IsCancellationRequested && cancelJob)
            {
                var isCancelled = await CancelFineTuneJobAsync(jobId);

                if (!isCancelled)
                {
                    throw new Exception($"Failed to cancel {jobId}");
                }
            }

            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
