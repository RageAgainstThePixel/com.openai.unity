// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    /// <summary>
    /// An invocation of an Assistant on a Thread.
    /// The Assistant uses it's configuration and the Thread's Messages to perform tasks by calling models and tools.
    /// As part of a Run, the Assistant appends Messages to the Thread.
    /// </summary>
    [Preserve]
    public sealed class RunResponse : BaseResponse, IStreamEvent
    {
        [Preserve]
        [JsonConstructor]
        internal RunResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("created_at")] int createdAtUnixTimeSeconds,
            [JsonProperty("thread_id")] string threadId,
            [JsonProperty("assistant_id")] string assistantId,
            [JsonProperty("status")] RunStatus status,
            [JsonProperty("required_action")] RequiredAction requiredAction,
            [JsonProperty("last_error")] Error lastError,
            [JsonProperty("expires_at")] int? expiresAtUnixTimeSeconds,
            [JsonProperty("started_at")] int? startedAtUnixTimeSeconds,
            [JsonProperty("cancelled_at")] int? cancelledAtUnixTimeSeconds,
            [JsonProperty("failed_at")] int? failedAtUnixTimeSeconds,
            [JsonProperty("completed_at")] int? completedAtUnixTimeSeconds,
            [JsonProperty("incomplete_details")] IncompleteDetails incompleteDetails,
            [JsonProperty("model")] string model,
            [JsonProperty("instructions")] string instructions,
            [JsonProperty("tools")] IReadOnlyList<Tool> tools,
            [JsonProperty("metadata")] Dictionary<string, string> metadata,
            [JsonProperty("usage")] Usage usage,
            [JsonProperty("temperature")] double? temperature,
            [JsonProperty("top_p")] double? topP,
            [JsonProperty("max_prompt_tokens")] int? maxPromptTokens,
            [JsonProperty("max_completion_tokens")] int? maxCompletionTokens,
            [JsonProperty("truncation_strategy")] TruncationStrategy truncationStrategy,
            [JsonProperty("tool_choice")] object toolChoice,
            [JsonProperty("response_format")][JsonConverter(typeof(ResponseFormatConverter))] ChatResponseFormat responseFormat)
        {
            Id = id;
            Object = @object;
            CreatedAtUnixTimeSeconds = createdAtUnixTimeSeconds;
            ThreadId = threadId;
            AssistantId = assistantId;
            Status = status;
            RequiredAction = requiredAction;
            LastError = lastError;
            ExpiresAtUnixTimeSeconds = expiresAtUnixTimeSeconds;
            StartedAtUnixTimeSeconds = startedAtUnixTimeSeconds;
            CancelledAtUnixTimeSeconds = cancelledAtUnixTimeSeconds;
            FailedAtUnixTimeSeconds = failedAtUnixTimeSeconds;
            CompletedAtUnixTimeSeconds = completedAtUnixTimeSeconds;
            IncompleteDetails = incompleteDetails;
            Model = model;
            Instructions = instructions;
            Tools = tools;
            Metadata = metadata;
            Usage = usage;
            Temperature = temperature;
            TopP = topP;
            MaxPromptTokens = maxPromptTokens;
            MaxCompletionTokens = maxCompletionTokens;
            TruncationStrategy = truncationStrategy;
            ToolChoice = toolChoice;
            ResponseFormat = responseFormat;
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
        /// The Unix timestamp (in seconds) for when the thread was created.
        /// </summary>
        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixTimeSeconds { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds).DateTime;

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

        [Preserve]
        [JsonProperty("incomplete_details")]
        public IncompleteDetails IncompleteDetails { get; }

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
        [JsonIgnore]
        [Obsolete("Removed")]
        public IReadOnlyList<string> FileIds => null;

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </summary>
        [Preserve]
        [JsonProperty("metadata")]
        public IReadOnlyDictionary<string, string> Metadata { get; }

        /// <summary>
        /// Usage statistics related to the run. This value will be `null` if the run is not in a terminal state (i.e. `in_progress`, `queued`, etc.).
        /// </summary>
        [Preserve]
        [JsonProperty("usage")]
        public Usage Usage { get; }
        /// <summary>
        /// The sampling temperature used for this run. If not set, defaults to 1.
        /// </summary>
        [Preserve]
        [JsonProperty("temperature")]
        public double? Temperature { get; }

        /// <summary>
        /// The nucleus sampling value used for this run. If not set, defaults to 1.
        /// </summary>
        [Preserve]
        [JsonProperty("top_p")]
        public double? TopP { get; }

        /// <summary>
        /// The maximum number of prompt tokens specified to have been used over the course of the run.
        /// </summary>
        [Preserve]
        [JsonProperty("max_prompt_tokens")]
        public int? MaxPromptTokens { get; }

        /// <summary>
        /// The maximum number of completion tokens specified to have been used over the course of the run.
        /// </summary>
        [Preserve]
        [JsonProperty("max_completion_tokens")]
        public int? MaxCompletionTokens { get; }

        /// <summary>
        /// Controls for how a thread will be truncated prior to the run. Use this to control the initial context window of the run.
        /// </summary>
        [Preserve]
        [JsonProperty("truncation_strategy")]
        public TruncationStrategy TruncationStrategy { get; }

        /// <summary>
        /// Controls which (if any) tool is called by the model.
        /// none means the model will not call any tools and instead generates a message.
        /// auto is the default value and means the model can pick between generating a message or calling one or more tools.
        /// required means the model must call one or more tools before responding to the user.
        /// Specifying a particular tool like {"type": "file_search"} or {"type": "function", "function": {"name": "my_function"}}
        /// forces the model to call that tool.
        /// </summary>
        [Preserve]
        [JsonProperty("tool_choice")]
        public object ToolChoice { get; }

        /// <summary>
        /// Specifies the format that the model must output.
        /// Setting to <see cref="ChatResponseFormat.Json"/> enables JSON mode,
        /// which guarantees the message the model generates is valid JSON.
        /// </summary>
        /// <remarks>
        /// Important: When using JSON mode you must still instruct the model to produce JSON yourself via some conversation message,
        /// for example via your system message. If you don't do this, the model may generate an unending stream of
        /// whitespace until the generation reaches the token limit, which may take a lot of time and give the appearance
        /// of a "stuck" request. Also note that the message content may be partial (i.e. cut off) if finish_reason="length",
        /// which indicates the generation exceeded max_tokens or the conversation exceeded the max context length.
        /// </remarks>
        [Preserve]
        [JsonProperty("response_format")]
        [JsonConverter(typeof(ResponseFormatConverter))]
        public ChatResponseFormat ResponseFormat { get; }

        [Preserve]
        public static implicit operator string(RunResponse run) => run?.ToString();

        [Preserve]
        public override string ToString() => Id;
    }
}
