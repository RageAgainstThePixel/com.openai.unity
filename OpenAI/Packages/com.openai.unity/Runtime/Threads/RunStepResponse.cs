// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    /// <summary>
    /// A detailed list of steps the Assistant took as part of a Run.
    /// An Assistant can call tools or create Messages during it's run.
    /// Examining Run Steps allows you to introspect how the Assistant is getting to it's final results.
    /// </summary>
    [Preserve]
    public sealed class RunStepResponse : BaseResponse, IStreamEvent
    {
        [Preserve]
        internal RunStepResponse(RunStepResponse other) => Append(other);

        [Preserve]
        [JsonConstructor]
        internal RunStepResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("created_at")] int? createdAtUnixTimeSeconds,
            [JsonProperty("assistant_id")] string assistantId,
            [JsonProperty("thread_id")] string threadId,
            [JsonProperty("run_id")] string runId,
            [JsonProperty("type")] RunStepType runStepType,
            [JsonProperty("status")] RunStatus runStatus,
            [JsonProperty("step_details")] StepDetails stepDetails,
            [JsonProperty("last_error")] Error lastRunError,
            [JsonProperty("expired_at")] int? expiredAtUnixTimeSeconds,
            [JsonProperty("cancelled_at")] int? cancelledAtUnixTimeSeconds,
            [JsonProperty("failed_at")] int? failedAtUnixTimeSeconds,
            [JsonProperty("completed_at")] int? completedAtUnixTimeSeconds,
            [JsonProperty("metadata")] Dictionary<string, string> metadata,
            [JsonProperty("usage")] Usage usage)
        {
            Id = id;
            Object = @object;
            CreatedAtUnixTimeSeconds = createdAtUnixTimeSeconds;
            AssistantId = assistantId;
            ThreadId = threadId;
            RunId = runId;
            Type = runStepType;
            Status = runStatus;
            StepDetails = stepDetails;
            LastError = lastRunError;
            ExpiredAtUnixTimeSeconds = expiredAtUnixTimeSeconds;
            CancelledAtUnixTimeSeconds = cancelledAtUnixTimeSeconds;
            FailedAtUnixTimeSeconds = failedAtUnixTimeSeconds;
            CompletedAtUnixTimeSeconds = completedAtUnixTimeSeconds;
            Metadata = metadata;
            Usage = usage;
        }

        /// <summary>
        /// The identifier of the run step, which can be referenced in API endpoints.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; private set; }

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; private set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run step was created.
        /// </summary>
        [Preserve]
        [JsonProperty("created_at")]
        public int? CreatedAtUnixTimeSeconds { get; private set; }

        [JsonIgnore]
        public DateTime? CreatedAt
            => CreatedAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds.Value).DateTime
                : null;

        /// <summary>
        /// The ID of the assistant associated with the run step.
        /// </summary>
        [Preserve]
        [JsonProperty("assistant_id")]
        public string AssistantId { get; private set; }

        /// <summary>
        /// The ID of the thread that was run.
        /// </summary>
        [Preserve]
        [JsonProperty("thread_id")]
        public string ThreadId { get; private set; }

        /// <summary>
        /// The ID of the run that this run step is a part of.
        /// </summary>
        [Preserve]
        [JsonProperty("run_id")]
        public string RunId { get; private set; }

        /// <summary>
        /// The type of run step.
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public RunStepType Type { get; private set; }

        /// <summary>
        /// The status of the run step.
        /// </summary>
        [Preserve]
        [JsonProperty("status")]
        public RunStatus Status { get; private set; }

        /// <summary>
        /// The details of the run step.
        /// </summary>
        [Preserve]
        [JsonProperty("step_details")]
        public StepDetails StepDetails { get; private set; }

        /// <summary>
        /// The last error associated with this run step. Will be null if there are no errors.
        /// </summary>
        [Preserve]
        [JsonProperty("last_error")]
        public Error LastError { get; private set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run step expired. A step is considered expired if the parent run is expired.
        /// </summary>
        [Preserve]
        [JsonProperty("expired_at")]
        public int? ExpiredAtUnixTimeSeconds { get; private set; }

        [JsonIgnore]
        [Obsolete("use ExpiredAtUnixTimeSeconds")]
        public int? ExpiresAtUnitTimeSeconds => ExpiredAtUnixTimeSeconds;

        [JsonIgnore]
        public DateTime? ExpiredAt
            => ExpiredAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(ExpiredAtUnixTimeSeconds.Value).DateTime
                : null;

        [JsonIgnore]
        [Obsolete("Use ExpiredAt")]
        public DateTime? ExpiresAt => ExpiredAt;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run step was cancelled.
        /// </summary>
        [Preserve]
        [JsonProperty("cancelled_at")]
        public int? CancelledAtUnixTimeSeconds { get; private set; }

        [Preserve]
        [JsonIgnore]
        public DateTime? CancelledAt
            => CancelledAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(CancelledAtUnixTimeSeconds.Value).DateTime
                : null;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run step failed.
        /// </summary>
        [Preserve]
        [JsonProperty("failed_at")]
        public int? FailedAtUnixTimeSeconds { get; private set; }

        [Preserve]
        [JsonIgnore]
        public DateTime? FailedAt
            => FailedAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(FailedAtUnixTimeSeconds.Value).DateTime
                : null;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run step completed.
        /// </summary>
        [Preserve]
        [JsonProperty("completed_at")]
        public int? CompletedAtUnixTimeSeconds { get; private set; }

        [Preserve]
        [JsonIgnore]
        public DateTime? CompletedAt
            => CompletedAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(CompletedAtUnixTimeSeconds.Value).DateTime
                : null;

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </summary>
        [Preserve]
        [JsonProperty("metadata")]
        public IReadOnlyDictionary<string, string> Metadata { get; private set; }

        [Preserve]
        [JsonProperty("usage")]
        public Usage Usage { get; private set; }

        [Preserve]
        public static implicit operator string(RunStepResponse runStep) => runStep?.ToString();

        [Preserve]
        public override string ToString() => Id;

        internal void Append(RunStepResponse other)
        {
            if (other == null) { return; }

            if (!string.IsNullOrWhiteSpace(other.Id))
            {
                Id = other.Id;
            }

            if (!string.IsNullOrWhiteSpace(other.Object))
            {
                Object = other.Object;
            }

            if (other.CreatedAtUnixTimeSeconds.HasValue)
            {
                CreatedAtUnixTimeSeconds = other.CreatedAtUnixTimeSeconds;
            }

            if (!string.IsNullOrWhiteSpace(other.AssistantId))
            {
                AssistantId = other.AssistantId;
            }

            if (!string.IsNullOrWhiteSpace(other.ThreadId))
            {
                ThreadId = other.ThreadId;
            }

            if (!string.IsNullOrWhiteSpace(other.RunId))
            {
                RunId = other.RunId;
            }

            if (other.Type > 0)
            {
                Type = other.Type;
            }

            if (other.Status > 0)
            {
                Status = other.Status;
            }

            if (other.StepDetails != null)
            {
                StepDetails = other.StepDetails;
            }

            if (other.LastError != null)
            {
                LastError = other.LastError;
            }

            if (other.ExpiredAtUnixTimeSeconds.HasValue)
            {
                ExpiredAtUnixTimeSeconds = other.ExpiredAtUnixTimeSeconds;
            }

            if (other.CancelledAtUnixTimeSeconds.HasValue)
            {
                CancelledAtUnixTimeSeconds = other.CancelledAtUnixTimeSeconds;
            }

            if (other.FailedAtUnixTimeSeconds.HasValue)
            {
                FailedAtUnixTimeSeconds = other.FailedAtUnixTimeSeconds;
            }

            if (other.CompletedAtUnixTimeSeconds.HasValue)
            {
                CompletedAtUnixTimeSeconds = other.CompletedAtUnixTimeSeconds;
            }

            if (other.Metadata is { Count: > 0 })
            {
                Metadata = new Dictionary<string, string>(other.Metadata);
            }

            if (other.Usage != null)
            {
                Usage = other.Usage;
            }
        }
    }
}
