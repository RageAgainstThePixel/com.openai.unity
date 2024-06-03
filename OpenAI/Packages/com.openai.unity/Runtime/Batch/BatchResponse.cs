// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Batch
{
    [Preserve]
    public sealed class BatchResponse : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        internal BatchResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("endpoint")] string endpoint,
            [JsonProperty("errors")] BatchErrors batchErrors,
            [JsonProperty("input_file_id")] string inputFileId,
            [JsonProperty("completion_window")] string completionWindow,
            [JsonProperty("status")] BatchStatus status,
            [JsonProperty("output_file_id")] string outputFileId,
            [JsonProperty("error_file_id")] string errorFileId,
            [JsonProperty("created_at")] int createdAt,
            [JsonProperty("in_progress_at")] int inProgressAt,
            [JsonProperty("expires_at")] int expiresAt,
            [JsonProperty("finalizing_at")] int finalizingAt,
            [JsonProperty("completed_at")] int completedAt,
            [JsonProperty("failed_at")] int? failedAt,
            [JsonProperty("expired_at")] int? expiredAt,
            [JsonProperty("cancelled_at")] int? cancelledAt,
            [JsonProperty("request_counts")] RequestCounts requestCounts,
            [JsonProperty("metadata")] IReadOnlyDictionary<string, object> metadata)
        {
            Id = id;
            Object = @object;
            Endpoint = endpoint;
            BatchErrors = batchErrors;
            InputFileId = inputFileId;
            CompletionWindow = completionWindow;
            Status = status;
            OutputFileId = outputFileId;
            ErrorFileId = errorFileId;
            CreatedAtUnixTimeSeconds = createdAt;
            InProgressAtUnixTimeSeconds = inProgressAt;
            ExpiresAtUnixTimeSeconds = expiresAt;
            FinalizingAtUnixTimeSeconds = finalizingAt;
            CompletedAtUnixTimeSeconds = completedAt;
            FailedAtUnixTimeSeconds = failedAt;
            ExpiredAtUnixTimeSeconds = expiredAt;
            CancelledAtUnixTimeSeconds = cancelledAt;
            RequestCounts = requestCounts;
            Metadata = metadata;
        }

        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// The object type, which is always batch.
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        /// <summary>
        /// The OpenAI API endpoint used by the batch.
        /// </summary>
        [Preserve]
        [JsonProperty("endpoint")]
        public string Endpoint { get; }

        /// <summary>
        /// Errors that occured during the batch job.
        /// </summary>
        [Preserve]
        [JsonProperty("errors")]
        public BatchErrors BatchErrors { get; }

        /// <summary>
        /// The ID of the input file for the batch.
        /// </summary>
        [Preserve]
        [JsonProperty("input_file_id")]
        public string InputFileId { get; }

        /// <summary>
        /// The time frame within which the batch should be processed.
        /// </summary>
        [Preserve]
        [JsonProperty("completion_window")]
        public string CompletionWindow { get; }

        /// <summary>
        /// The current status of the batch.
        /// </summary>
        [Preserve]
        [JsonProperty("status")]
        public BatchStatus Status { get; }

        /// <summary>
        /// The ID of the file containing the outputs of successfully executed requests.
        /// </summary>
        [Preserve]
        [JsonProperty("output_file_id")]
        public string OutputFileId { get; }

        /// <summary>
        /// The ID of the file containing the outputs of requests with errors.
        /// </summary>
        [Preserve]
        [JsonProperty("error_file_id")]
        public string ErrorFileId { get; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the batch was created.
        /// </summary>
        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixTimeSeconds { get; }

        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds).DateTime;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the batch started processing.
        /// </summary>
        [Preserve]
        [JsonProperty("in_progress_at")]
        public int InProgressAtUnixTimeSeconds { get; }

        [JsonIgnore]
        public DateTime InProgressAt => DateTimeOffset.FromUnixTimeSeconds(InProgressAtUnixTimeSeconds).DateTime;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the batch will expire.
        /// </summary>
        [Preserve]
        [JsonProperty("expires_at")]
        public int ExpiresAtUnixTimeSeconds { get; }

        [JsonIgnore]
        public DateTime ExpiresAt => DateTimeOffset.FromUnixTimeSeconds(ExpiresAtUnixTimeSeconds).DateTime;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the batch started finalizing.
        /// </summary>
        [Preserve]
        [JsonProperty("finalizing_at")]
        public int FinalizingAtUnixTimeSeconds { get; }

        [Preserve]
        public DateTime FinalizingAt => DateTimeOffset.FromUnixTimeSeconds(FinalizingAtUnixTimeSeconds).DateTime;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the batch was completed.
        /// </summary>
        [Preserve]
        [JsonProperty("completed_at")]
        public int CompletedAtUnixTimeSeconds { get; }

        [JsonIgnore]
        public DateTime CompletedAt => DateTimeOffset.FromUnixTimeSeconds(CompletedAtUnixTimeSeconds).DateTime;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the batch failed.
        /// </summary>
        [Preserve]
        [JsonProperty("failed_at")]
        public int? FailedAtUnixTimeSeconds { get; }

        [JsonIgnore]
        public DateTime? FailedAt
            => FailedAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(FailedAtUnixTimeSeconds.Value).DateTime
                : null;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the batch expired.
        /// </summary>
        [Preserve]
        [JsonProperty("expired_at")]
        public int? ExpiredAtUnixTimeSeconds { get; }

        [JsonIgnore]
        public DateTime? ExpiredAt
            => ExpiredAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(ExpiredAtUnixTimeSeconds.Value).DateTime
                : null;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the batch was cancelled.
        /// </summary>
        [Preserve]
        [JsonProperty("cancelled_at")]
        public int? CancelledAtUnixTimeSeconds { get; }

        [JsonIgnore]
        public DateTime? CancelledAt
            => CancelledAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(CancelledAtUnixTimeSeconds.Value).DateTime
                : null;

        /// <summary>
        /// The request counts for different statuses within the batch.
        /// </summary>
        [Preserve]
        [JsonProperty("request_counts")]
        public RequestCounts RequestCounts { get; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </summary>
        [Preserve]
        [JsonProperty("metadata")]
        public IReadOnlyDictionary<string, object> Metadata { get; }
    }
}
