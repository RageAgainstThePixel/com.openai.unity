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
            [Preserve]
            [JsonConstructor]
            public FineTuneList(
                [JsonProperty("object")] string @object,
                [JsonProperty("data")] List<FineTuneJob> data)
            {
                Object = @object;
                Data = data;
            }

            [Preserve]
            [JsonProperty("object")]
            public string Object { get; set; }

            [Preserve]
            [JsonProperty("data")]
            public List<FineTuneJob> Data { get; set; }
        }

        [Preserve]
        private class FineTuneEventList
        {
            [Preserve]
            [JsonConstructor]
            public FineTuneEventList([JsonProperty("data")] List<Event> data)
            {
                Data = data;
            }

            [Preserve]
            [JsonProperty("data")]
            public List<Event> Data { get; }
        }

        /// <inheritdoc />
        public FineTuningEndpoint(OpenAIClient client) : base(client) { }

        /// <inheritdoc />
        protected override string Root => "fine_tuning";

        /// <summary>
        /// Creates a job that fine-tunes a specified model from a given dataset.
        /// Response includes details of the queued job including job status and
        /// the name of the fine-tuned models once complete.
        /// </summary>
        /// <param name="jobRequest"><see cref="CreateFineTuneJobRequest"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="FineTuneJob"/>.</returns>
        public async Task<FineTuneJob> CreateFineTuneJobAsync(CreateFineTuneJobRequest jobRequest, CancellationToken cancellationToken = default)
        {
            var payload = JsonConvert.SerializeObject(jobRequest, client.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl("/jobs"), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate();
            return JsonConvert.DeserializeObject<FineTuneJob>(response.Body, client.JsonSerializationOptions);
        }

        /// <summary>
        /// List your organization's fine-tuning jobs.
        /// </summary>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>List of <see cref="FineTuneJob"/>s.</returns>
        public async Task<IReadOnlyList<FineTuneJob>> ListFineTuneJobsAsync(CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl("/jobs"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(true);
            return JsonConvert.DeserializeObject<FineTuneList>(response.Body, client.JsonSerializationOptions)?.Data.OrderBy(job => job.CreatedAtUnixTime).ToArray();
        }

        /// <summary>
        /// Gets info about the fine-tune job.
        /// </summary>
        /// <param name="jobId"><see cref="FineTuneJob.Id"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="FineTuneJob"/>.</returns>
        public async Task<FineTuneJob> RetrieveFineTuneJobInfoAsync(string jobId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/jobs/{jobId}"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(true);
            return JsonConvert.DeserializeObject<FineTuneJob>(response.Body, client.JsonSerializationOptions);
        }

        /// <summary>
        /// Immediately cancel a fine-tune job.
        /// </summary>
        /// <param name="jobId"><see cref="FineTuneJob.Id"/> to cancel.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="FineTuneJob"/>.</returns>
        public async Task<bool> CancelFineTuneJobAsync(string jobId, CancellationToken cancellationToken = default)
        {
            var job = await RetrieveFineTuneJobInfoAsync(jobId, cancellationToken);
            var response = await Rest.PostAsync(GetUrl($"/jobs/{job.Id}/cancel"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate();

            if (!string.IsNullOrWhiteSpace(response.Body))
            {
                var result = JsonConvert.DeserializeObject<FineTuneJob>(response.Body, client.JsonSerializationOptions);
                return result.Status == JobStatus.Cancelled;
            }

            job = await RetrieveFineTuneJobInfoAsync(jobId, cancellationToken);
            return job.Status == JobStatus.Cancelled;
        }

        /// <summary>
        /// Get fine-grained status updates for a fine-tune job.
        /// </summary>
        /// <param name="jobId"><see cref="FineTuneJob.Id"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>List of events for <see cref="FineTuneJob"/>.</returns>
        public async Task<IReadOnlyList<Event>> ListFineTuneEventsAsync(string jobId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/jobs/{jobId}/events"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate();
            return JsonConvert.DeserializeObject<FineTuneEventList>(response.Body, client.JsonSerializationOptions)?.Data.OrderBy(@event => @event.CreatedAtUnixTime).ToArray();
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
            var response = await Rest.GetAsync(GetUrl($"/jobs/{jobId}/events?stream=true"), eventData =>
            {
                if (!string.IsNullOrWhiteSpace(eventData))
                {
                    fineTuneEventCallback(JsonConvert.DeserializeObject<Event>(eventData, client.JsonSerializationOptions));
                }
            }, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate();

            if (cancellationToken.IsCancellationRequested && cancelJob)
            {
                var isCancelled = await CancelFineTuneJobAsync(jobId, cancellationToken);

                if (!isCancelled)
                {
                    throw new Exception($"Failed to cancel {jobId}");
                }
            }

            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
