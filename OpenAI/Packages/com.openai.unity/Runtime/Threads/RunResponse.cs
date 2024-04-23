// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    /// <summary>
    /// An invocation of an Assistant on a Thread.
    /// The Assistant uses it’s configuration and the Thread’s Messages to perform tasks by calling models and tools.
    /// As part of a Run, the Assistant appends Messages to the Thread.
    /// </summary>
    [Preserve]
    public sealed class RunResponse : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        public RunResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("thread_id")] string threadId,
            [JsonProperty("assistant_id")] string assistantId,
            [JsonProperty("status")] RunStatus status,
            [JsonProperty("required_action")] RequiredAction requiredAction,
            [JsonProperty("last_error")] Error lastError,
            [JsonProperty("created_at")] int createdAtUnixTimeSeconds,
            [JsonProperty("expires_at")] int? expiresAtUnixTimeSeconds,
            [JsonProperty("started_at")] int? startedAtUnixTimeSeconds,
            [JsonProperty("cancelled_at")] int? cancelledAtUnixTimeSeconds,
            [JsonProperty("failed_at")] int? failedAtUnixTimeSeconds,
            [JsonProperty("completed_at")] int? completedAtUnixTimeSeconds,
            [JsonProperty("model")] string model,
            [JsonProperty("instructions")] string instructions,
            [JsonProperty("tools")] IReadOnlyList<Tool> tools,
            [JsonProperty("file_ids")] IReadOnlyList<string> fileIds,
            [JsonProperty("metadata")] Dictionary<string, string> metadata,
            [JsonProperty("usage")] Usage usage)
        {
            Id = id;
            Object = @object;
            ThreadId = threadId;
            AssistantId = assistantId;
            Status = status;
            RequiredAction = requiredAction;
            LastError = lastError;
            CreatedAtUnixTimeSeconds = createdAtUnixTimeSeconds;
            ExpiresAtUnixTimeSeconds = expiresAtUnixTimeSeconds;
            StartedAtUnixTimeSeconds = startedAtUnixTimeSeconds;
            CancelledAtUnixTimeSeconds = cancelledAtUnixTimeSeconds;
            FailedAtUnixTimeSeconds = failedAtUnixTimeSeconds;
            CompletedAtUnixTimeSeconds = completedAtUnixTimeSeconds;
            Model = model;
            Instructions = instructions;
            Tools = tools;
            FileIds = fileIds;
            Metadata = metadata;
            Usage = usage;
        }

        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// The object type, which is always run.
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        /// <summary>
        /// The thread ID that this run belongs to.
        /// </summary>
        [Preserve]
        [JsonProperty("thread_id")]
        public string ThreadId { get; }

        /// <summary>
        /// The ID of the assistant used for execution of this run.
        /// </summary>
        [Preserve]
        [JsonProperty("assistant_id")]
        public string AssistantId { get; }

        /// <summary>
        /// The status of the run.
        /// </summary>
        [Preserve]
        [JsonProperty("status")]
        public RunStatus Status { get; }

        /// <summary>
        /// Details on the action required to continue the run.
        /// Will be null if no action is required.
        /// </summary>
        [Preserve]
        [JsonProperty("required_action")]
        public RequiredAction RequiredAction { get; }

        /// <summary>
        /// The Last error Associated with this run.
        /// Will be null if there are no errors.
        /// </summary>
        [Preserve]
        [JsonProperty("last_error")]
        public Error LastError { get; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the thread was created.
        /// </summary>
        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixTimeSeconds { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds).DateTime;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run will expire.
        /// </summary>
        [Preserve]
        [JsonProperty("expires_at")]
        public int? ExpiresAtUnixTimeSeconds { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime? ExpiresAt
            => ExpiresAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(ExpiresAtUnixTimeSeconds.Value).DateTime
                : null;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run was started.
        /// </summary>
        [Preserve]
        [JsonProperty("started_at")]
        public int? StartedAtUnixTimeSeconds { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime? StartedAt
            => StartedAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(StartedAtUnixTimeSeconds.Value).DateTime
                : null;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run was cancelled.
        /// </summary>
        [Preserve]
        [JsonProperty("cancelled_at")]
        public int? CancelledAtUnixTimeSeconds { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime? CancelledAt
            => CancelledAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(CancelledAtUnixTimeSeconds.Value).DateTime
                : null;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run failed.
        /// </summary>
        [Preserve]
        [JsonProperty("failed_at")]
        public int? FailedAtUnixTimeSeconds { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime? FailedAt
            => FailedAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(FailedAtUnixTimeSeconds.Value).DateTime
                : null;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run was completed.
        /// </summary>
        [Preserve]
        [JsonProperty("completed_at")]
        public int? CompletedAtUnixTimeSeconds { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime? CompletedAt
            => CompletedAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(CompletedAtUnixTimeSeconds.Value).DateTime
                : null;

        /// <summary>
        /// The model that the assistant used for this run.
        /// </summary>
        [Preserve]
        [JsonProperty("model")]
        public string Model { get; }

        /// <summary>
        /// The instructions that the assistant used for this run.
        /// </summary>
        [Preserve]
        [JsonProperty("instructions")]
        public string Instructions { get; }

        /// <summary>
        /// The list of tools that the assistant used for this run.
        /// </summary>
        [Preserve]
        [JsonProperty("tools")]
        public IReadOnlyList<Tool> Tools { get; }

        /// <summary>
        /// The list of File IDs the assistant used for this run.
        /// </summary>
        [Preserve]
        [JsonProperty("file_ids")]
        public IReadOnlyList<string> FileIds { get; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </summary>
        [Preserve]
        [JsonProperty("metadata")]
        public IReadOnlyDictionary<string, string> Metadata { get; }

        [Preserve]
        [JsonProperty("usage")]
        public Usage Usage { get; }

        [Preserve]
        public static implicit operator string(RunResponse run) => run?.ToString();

        [Preserve]
        public override string ToString() => Id;
    }
}
