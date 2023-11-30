// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Utilities.WebRequestRest;

namespace OpenAI.FineTuning
{
    /// <summary>
    /// Manage fine-tuning jobs to tailor a model to your specific training data.<br/>
    /// <see href="https://platform.openai.com/docs/guides/fine-tuning"/><br/>
    /// <see href="https://platform.openai.com/docs/api-reference/fine-tuning"/>
    /// </summary>
    public sealed class FineTuningEndpoint : OpenAIBaseEndpoint
    {
        internal FineTuningEndpoint(OpenAIClient client) : base(client) { }

        protected override string Root => "fine_tuning";

        /// <summary>
        /// Creates a job that fine-tunes a specified model from a given dataset.
        /// Response includes details of the queued job including job status and
        /// the name of the fine-tuned models once complete.
        /// </summary>
        /// <param name="jobRequest"><see cref="CreateFineTuneJobRequest"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="FineTuneJobResponse"/>.</returns>
        public async Task<FineTuneJobResponse> CreateJobAsync(CreateFineTuneJobRequest jobRequest, CancellationToken cancellationToken = default)
        {
            var payload = JsonConvert.SerializeObject(jobRequest, OpenAIClient.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl("/jobs"), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<FineTuneJobResponse>(response.Body, client);
        }

        [Obsolete("Use new overload")]
        public async Task<FineTuneJobList> ListJobsAsync(int? limit, string after, CancellationToken cancellationToken = default)
        {
            var parameters = new Dictionary<string, string>();

            if (limit.HasValue)
            {
                parameters.Add(nameof(limit), limit.ToString());
            }

            if (!string.IsNullOrWhiteSpace(after))
            {
                parameters.Add(nameof(after), after);
            }

            var response = await Rest.GetAsync(GetUrl("/jobs", parameters), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return JsonConvert.DeserializeObject<FineTuneJobList>(response.Body, OpenAIClient.JsonSerializationOptions);
        }

        /// <summary>
        /// List your organization's fine-tuning jobs.
        /// </summary>
        /// <param name="query"><see cref="ListQuery"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>List of <see cref="FineTuneJobResponse"/>s.</returns>
        public async Task<ListResponse<FineTuneJobResponse>> ListJobsAsync(ListQuery query = null, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl("/jobs", query), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<ListResponse<FineTuneJobResponse>>(response.Body, client);
        }

        /// <summary>
        /// Gets info about the fine-tune job.
        /// </summary>
        /// <param name="jobId"><see cref="FineTuneJob.Id"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="FineTuneJobResponse"/>.</returns>
        public async Task<FineTuneJobResponse> GetJobInfoAsync(string jobId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/jobs/{jobId}"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            var job = response.Deserialize<FineTuneJobResponse>(response.Body, client);
            job.Events = (await ListJobEventsAsync(job, query: null, cancellationToken: cancellationToken).ConfigureAwait(true))?.Items;
            return job;
        }

        /// <summary>
        /// Immediately cancel a fine-tune job.
        /// </summary>
        /// <param name="jobId"><see cref="FineTuneJob.Id"/> to cancel.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="FineTuneJobResponse"/>.</returns>
        public async Task<bool> CancelJobAsync(string jobId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.PostAsync(GetUrl($"/jobs/{jobId}/cancel"), jsonData: string.Empty, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            var result = JsonConvert.DeserializeObject<FineTuneJobResponse>(response.Body, OpenAIClient.JsonSerializationOptions);
            return result.Status == JobStatus.Cancelled;
        }

        [Obsolete("use new overload")]
        public async Task<EventList> ListJobEventsAsync(string jobId, int? limit, string after, CancellationToken cancellationToken = default)
        {
            var parameters = new Dictionary<string, string>();

            if (limit.HasValue)
            {
                parameters.Add(nameof(limit), limit.ToString());
            }

            if (!string.IsNullOrWhiteSpace(after))
            {
                parameters.Add(nameof(after), after);
            }

            var response = await Rest.GetAsync(GetUrl($"/jobs/{jobId}/events", parameters), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return JsonConvert.DeserializeObject<EventList>(response.Body, OpenAIClient.JsonSerializationOptions);
        }

        /// <summary>
        /// Get fine-grained status updates for a fine-tune job.
        /// </summary>
        /// <param name="jobId"><see cref="FineTuneJob.Id"/>.</param>
        /// <param name="query"><see cref="ListQuery"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>List of events for <see cref="FineTuneJobResponse"/>.</returns>
        public async Task<ListResponse<EventResponse>> ListJobEventsAsync(string jobId, ListQuery query = null, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/jobs/{jobId}/events", query), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<ListResponse<EventResponse>>(response.Body, client);
        }
    }
}
