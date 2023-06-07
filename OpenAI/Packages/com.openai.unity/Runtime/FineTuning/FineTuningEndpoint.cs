﻿// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Scripting;
using Utilities.WebRequestRest;

namespace OpenAI.FineTuning
{
    /// <summary>
    /// Manage fine-tuning jobs to tailor a model to your specific training data.<br/>
    /// <see href="https://platform.openai.com/docs/guides/fine-tuning"/>
    /// </summary>
    public sealed class FineTuningEndpoint : OpenAIBaseEndpoint
    {
        [Preserve]
        private class FineTuneList
        {
            [JsonProperty("object")]
            public string Object { get; set; }

            [JsonProperty("data")]
            public List<FineTuneJob> Data { get; set; }
        }

        [Preserve]
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
        public async Task<FineTuneJob> CreateFineTuneJobAsync(CreateFineTuneJobRequest jobRequest)
        {
            var jsonContent = JsonConvert.SerializeObject(jobRequest, client.JsonSerializationOptions);

            var response = await Rest.PostAsync(GetUrl(), jsonContent, new RestParameters(client.DefaultRequestHeaders));
            response.ValidateResponse();
            return response.DeserializeResponse<FineTuneJobResponse>(response.ResponseBody, client.JsonSerializationOptions);
        }

        /// <summary>
        /// List your organization's fine-tuning jobs.
        /// </summary>
        /// <returns>List of <see cref="FineTuneJob"/>s.</returns>
        public async Task<IReadOnlyList<FineTuneJob>> ListFineTuneJobsAsync()
        {
            var response = await Rest.GetAsync(GetUrl(), new RestParameters(client.DefaultRequestHeaders));
            response.ValidateResponse();
            return JsonConvert.DeserializeObject<FineTuneList>(response.ResponseBody, client.JsonSerializationOptions)?.Data.OrderBy(job => job.CreatedAtUnixTime).ToArray();
        }

        /// <summary>
        /// Gets info about the fine-tune job.
        /// </summary>
        /// <param name="jobId"><see cref="FineTuneJob.Id"/>.</param>
        /// <returns><see cref="FineTuneJobResponse"/>.</returns>
        public async Task<FineTuneJob> RetrieveFineTuneJobInfoAsync(string jobId)
        {
            var response = await Rest.GetAsync(GetUrl($"/{jobId}"), new RestParameters(client.DefaultRequestHeaders));
            response.ValidateResponse();
            return response.DeserializeResponse<FineTuneJobResponse>(response.ResponseBody, client.JsonSerializationOptions);
        }

        /// <summary>
        /// Immediately cancel a fine-tune job.
        /// </summary>
        /// <param name="jobId"><see cref="FineTuneJob.Id"/> to cancel.</param>
        /// <returns><see cref="FineTuneJobResponse"/>.</returns>
        public async Task<bool> CancelFineTuneJobAsync(string jobId)
        {
            var job = await RetrieveFineTuneJobInfoAsync(jobId);
            var response = await Rest.PostAsync(GetUrl($"/{job.Id}/cancel"), new RestParameters(client.DefaultRequestHeaders));
            response.ValidateResponse();

            const string cancelled = "cancelled";

            if (!string.IsNullOrWhiteSpace(response.ResponseBody))
            {
                var result = response.DeserializeResponse<FineTuneJobResponse>(response.ResponseBody, client.JsonSerializationOptions);
                return result.Status == cancelled;
            }

            job = await RetrieveFineTuneJobInfoAsync(jobId);
            return job.Status == cancelled;
        }

        /// <summary>
        /// Get fine-grained status updates for a fine-tune job.
        /// </summary>
        /// <param name="jobId"><see cref="FineTuneJob.Id"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>List of events for <see cref="FineTuneJob"/>.</returns>
        public async Task<IReadOnlyList<Event>> ListFineTuneEventsAsync(string jobId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/{jobId}/events"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.ValidateResponse();
            return JsonConvert.DeserializeObject<FineTuneEventList>(response.ResponseBody, client.JsonSerializationOptions)?.Data.OrderBy(@event => @event.CreatedAtUnixTime).ToArray();
        }

        /// <summary>
        /// Stream the fine-grained status updates for a fine-tune job.
        /// </summary>
        /// <param name="jobId"><see cref="FineTuneJob.Id"/>.</param>
        /// <param name="fineTuneEventCallback">The event callback handler.</param>
        /// <param name="cancelJob">Optional, Cancel the job if streaming is aborted. Default is false.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        public async Task StreamFineTuneEventsAsync(string jobId, Action<Event> fineTuneEventCallback, bool cancelJob = false, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/{jobId}/events?stream=true"), eventData =>
            {
                if (!string.IsNullOrWhiteSpace(eventData))
                {
                    fineTuneEventCallback(JsonConvert.DeserializeObject<Event>(eventData, client.JsonSerializationOptions));
                }
            }, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.ValidateResponse();

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
