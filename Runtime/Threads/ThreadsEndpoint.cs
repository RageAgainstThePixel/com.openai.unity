// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenAI.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Utilities.WebRequestRest;
using Utilities.WebRequestRest.Interfaces;

namespace OpenAI.Threads
{
    /// <summary>
    /// Create threads that assistants can interact with.<br/>
    /// <see href="https://platform.openai.com/docs/api-reference/threads"/>
    /// </summary>
    public sealed class ThreadsEndpoint : OpenAIBaseEndpoint
    {
        internal ThreadsEndpoint(OpenAIClient client) : base(client) { }

        protected override string Root => "threads";

        /// <summary>
        /// Create a thread.
        /// </summary>
        /// <param name="request"><see cref="CreateThreadRequest"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ThreadResponse"/>.</returns>
        public async Task<ThreadResponse> CreateThreadAsync(CreateThreadRequest request = null, CancellationToken cancellationToken = default)
        {
            var payload = request != null ? JsonConvert.SerializeObject(request, OpenAIClient.JsonSerializationOptions) : string.Empty;
            var response = await Rest.PostAsync(GetUrl(), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<ThreadResponse>(client);
        }

        /// <summary>
        /// Retrieves a thread.
        /// </summary>
        /// <param name="threadId">The id of the <see cref="ThreadResponse"/> to retrieve.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ThreadResponse"/>.</returns>
        public async Task<ThreadResponse> RetrieveThreadAsync(string threadId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/{threadId}"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<ThreadResponse>(client);
        }

        /// <summary>
        /// Modifies a thread.
        /// </summary>
        /// <remarks>
        /// Only the <see cref="ThreadResponse.Metadata"/> can be modified.
        /// </remarks>
        /// <param name="threadId">The id of the <see cref="ThreadResponse"/> to modify.</param>
        /// <param name="metadata">Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ThreadResponse"/>.</returns>
        public async Task<ThreadResponse> ModifyThreadAsync(string threadId, IReadOnlyDictionary<string, string> metadata, CancellationToken cancellationToken = default)
        {
            var payload = JsonConvert.SerializeObject(new { metadata }, OpenAIClient.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl($"/{threadId}"), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<ThreadResponse>(client);
        }

        /// <summary>
        /// Delete a thread.
        /// </summary>
        /// <param name="threadId">The id of the <see cref="ThreadResponse"/> to delete.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>True, if was successfully deleted.</returns>
        public async Task<bool> DeleteThreadAsync(string threadId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.DeleteAsync(GetUrl($"/{threadId}"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<DeletedResponse>(client)?.Deleted ?? false;
        }

        #region Messages

        /// <summary>
        /// Create a message.
        /// </summary>
        /// <param name="threadId">The id of the thread to create a message for.</param>
        /// <param name="message"><see cref="Message"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="MessageResponse"/>.</returns>
        public async Task<MessageResponse> CreateMessageAsync(string threadId, Message message, CancellationToken cancellationToken = default)
        {
            var payload = JsonConvert.SerializeObject(message, OpenAIClient.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl($"/{threadId}/messages"), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<MessageResponse>(client);
        }

        /// <summary>
        /// Returns a list of messages for a given thread.
        /// </summary>
        /// <param name="threadId">The id of the thread the messages belong to.</param>
        /// <param name="query"><see cref="ListQuery"/>.</param>
        /// <param name="runId">Optional, filter messages by the run ID that generated them.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ListResponse{ThreadMessage}"/>.</returns>
        public async Task<ListResponse<MessageResponse>> ListMessagesAsync(string threadId, ListQuery query = null, string runId = null, CancellationToken cancellationToken = default)
        {
            Dictionary<string, string> queryParams = query;

            if (!string.IsNullOrWhiteSpace(runId))
            {
                queryParams ??= new();
                queryParams.Add("run_id", runId);
            }

            var response = await Rest.GetAsync(GetUrl($"/{threadId}/messages", queryParams), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<ListResponse<MessageResponse>>(client);
        }

        /// <summary>
        /// Retrieve a message.
        /// </summary>
        /// <param name="threadId">The id of the thread to which this message belongs.</param>
        /// <param name="messageId">The id of the message to retrieve.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="MessageResponse"/>.</returns>
        public async Task<MessageResponse> RetrieveMessageAsync(string threadId, string messageId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/{threadId}/messages/{messageId}"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<MessageResponse>(client);
        }

        /// <summary>
        /// Modifies a message.
        /// </summary>
        /// <remarks>
        /// Only the <see cref="MessageResponse.Metadata"/> can be modified.
        /// </remarks>
        /// <param name="message"><see cref="MessageResponse"/> to modify.</param>
        /// <param name="metadata">Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="MessageResponse"/>.</returns>
        public async Task<MessageResponse> ModifyMessageAsync(MessageResponse message, IReadOnlyDictionary<string, string> metadata, CancellationToken cancellationToken = default)
            => await ModifyMessageAsync(message.ThreadId, message.Id, metadata, cancellationToken);

        /// <summary>
        /// Modifies a message.
        /// </summary>
        /// <remarks>
        /// Only the <see cref="MessageResponse.Metadata"/> can be modified.
        /// </remarks>
        /// <param name="threadId">The id of the thread to which this message belongs.</param>
        /// <param name="messageId">The id of the message to modify.</param>
        /// <param name="metadata">Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="MessageResponse"/>.</returns>
        public async Task<MessageResponse> ModifyMessageAsync(string threadId, string messageId, IReadOnlyDictionary<string, string> metadata, CancellationToken cancellationToken = default)
        {
            var payload = JsonConvert.SerializeObject(new { metadata }, OpenAIClient.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl($"/{threadId}/messages/{messageId}"), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<MessageResponse>(client);
        }

        #endregion Messages

        #region Runs

        /// <summary>
        /// Returns a list of runs belonging to a thread.
        /// </summary>
        /// <param name="threadId">The id of the thread the run belongs to.</param>
        /// <param name="query"><see cref="ListQuery"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ListResponse{RunResponse}"/></returns>
        public async Task<ListResponse<RunResponse>> ListRunsAsync(string threadId, ListQuery query = null, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/{threadId}/runs", query), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<ListResponse<RunResponse>>(client);
        }

        [Obsolete("use new overload with Func<IServerSentEvent, Task> instead.")]
        public async Task<RunResponse> CreateRunAsync(string threadId, CreateRunRequest request, Action<IServerSentEvent> streamEventHandler, CancellationToken cancellationToken = default)
            => await CreateRunAsync(threadId, request, streamEventHandler == null ? null : serverSentEvent =>
            {
                streamEventHandler.Invoke(serverSentEvent);
                return Task.CompletedTask;
            }, cancellationToken);

        /// <summary>
        /// Create a run.
        /// </summary>
        /// <param name="threadId">The id of the thread to run.</param>
        /// <param name="request"><see cref="CreateRunRequest"/>.</param>
        /// <param name="streamEventHandler">Optional, <see cref="Func{IServerSentEvent, Task}"/> stream callback handler.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="RunResponse"/>.</returns>
        public async Task<RunResponse> CreateRunAsync(string threadId, CreateRunRequest request = null, Func<IServerSentEvent, Task> streamEventHandler = null, CancellationToken cancellationToken = default)
            => await CreateRunAsync(threadId, request, streamEventHandler == null ? null : async (_, serverSentEvent) =>
            {
                await streamEventHandler.Invoke(serverSentEvent);
            }, cancellationToken);

        /// <summary>
        /// Create a run.
        /// </summary>
        /// <param name="threadId">The id of the thread to run.</param>
        /// <param name="request"><see cref="CreateRunRequest"/>.</param>
        /// <param name="streamEventHandler">Optional, <see cref="Func{String, IServerSentEvent, Task}"/> stream callback handler.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="RunResponse"/>.</returns>
        public async Task<RunResponse> CreateRunAsync(string threadId, CreateRunRequest request = null, Func<string, IServerSentEvent, Task> streamEventHandler = null, CancellationToken cancellationToken = default)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.AssistantId))
            {
                var assistant = await client.AssistantsEndpoint.CreateAssistantAsync(cancellationToken: cancellationToken);
                request = new CreateRunRequest(assistant, request);
            }

            request.Stream = streamEventHandler != null;
            var endpoint = GetUrl($"/{threadId}/runs");
            var payload = JsonConvert.SerializeObject(request, OpenAIClient.JsonSerializationOptions);

            if (request.Stream)
            {
                return await StreamRunAsync(endpoint, payload, streamEventHandler, cancellationToken);
            }

            var response = await Rest.PostAsync(endpoint, payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<RunResponse>(client);
        }

        [Obsolete("use new overload with Func<IServerSentEvent, Task> instead.")]
        public async Task<RunResponse> CreateThreadAndRunAsync(CreateThreadAndRunRequest request, Action<IServerSentEvent> streamEventHandler, CancellationToken cancellationToken = default)
            => await CreateThreadAndRunAsync(request, streamEventHandler == null ? null : serverSentEvent =>
            {
                streamEventHandler.Invoke(serverSentEvent);
                return Task.CompletedTask;
            }, cancellationToken);

        /// <summary>
        /// Create a thread and run it in one request.
        /// </summary>
        /// <param name="request"><see cref="CreateThreadAndRunRequest"/>.</param>
        /// <param name="streamEventHandler">Optional, <see cref="Func{IServerSentEvent, Task}"/> stream callback handler.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="RunResponse"/>.</returns>
        public async Task<RunResponse> CreateThreadAndRunAsync(CreateThreadAndRunRequest request = null, Func<IServerSentEvent, Task> streamEventHandler = null, CancellationToken cancellationToken = default)
            => await CreateThreadAndRunAsync(request, streamEventHandler == null ? null : async (_, serverSentEvent) =>
            {
                await streamEventHandler.Invoke(serverSentEvent);
            }, cancellationToken);

        /// <summary>
        /// Create a thread and run it in one request.
        /// </summary>
        /// <param name="request"><see cref="CreateThreadAndRunRequest"/>.</param>
        /// <param name="streamEventHandler">Optional, <see cref="Func{String, IServerSentEvent, Task}"/> stream callback handler.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="RunResponse"/>.</returns>
        public async Task<RunResponse> CreateThreadAndRunAsync(CreateThreadAndRunRequest request = null, Func<string, IServerSentEvent, Task> streamEventHandler = null, CancellationToken cancellationToken = default)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.AssistantId))
            {
                var assistant = await client.AssistantsEndpoint.CreateAssistantAsync(cancellationToken: cancellationToken);
                request = new CreateThreadAndRunRequest(assistant, request);
            }

            request.Stream = streamEventHandler != null;
            var endpoint = GetUrl("/runs");
            var payload = JsonConvert.SerializeObject(request, OpenAIClient.JsonSerializationOptions);

            if (request.Stream)
            {
                return await StreamRunAsync(endpoint, payload, streamEventHandler, cancellationToken);
            }

            var response = await Rest.PostAsync(endpoint, payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<RunResponse>(client);
        }

        /// <summary>
        /// Retrieves a run.
        /// </summary>
        /// <param name="threadId">The id of the thread that was run.</param>
        /// <param name="runId">The id of the run to retrieve.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="RunResponse"/>.</returns>
        public async Task<RunResponse> RetrieveRunAsync(string threadId, string runId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/{threadId}/runs/{runId}"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<RunResponse>(client);
        }

        /// <summary>
        /// Modifies a run.
        /// </summary>
        /// <remarks>
        /// Only the <see cref="RunResponse.Metadata"/> can be modified.
        /// </remarks>
        /// <param name="threadId">The id of the thread that was run.</param>
        /// <param name="runId">The id of the <see cref="RunResponse"/> to modify.</param>
        /// <param name="metadata">Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="RunResponse"/>.</returns>
        public async Task<RunResponse> ModifyRunAsync(string threadId, string runId, IReadOnlyDictionary<string, string> metadata, CancellationToken cancellationToken = default)
        {
            var payload = JsonConvert.SerializeObject(new { metadata }, OpenAIClient.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl($"/{threadId}/runs/{runId}"), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<RunResponse>(client);
        }

        [Obsolete("use new overload with Func<IServerSentEvent, Task> instead.")]
        public async Task<RunResponse> SubmitToolOutputsAsync(string threadId, string runId, SubmitToolOutputsRequest request, Action<IServerSentEvent> streamEventHandler, CancellationToken cancellationToken = default)
            => await SubmitToolOutputsAsync(threadId, runId, request, streamEventHandler == null ? null : serverSentEvent =>
            {
                streamEventHandler.Invoke(serverSentEvent);
                return Task.CompletedTask;
            }, cancellationToken);

        /// <summary>
        /// When a run has the status: "requires_action" and required_action.type is submit_tool_outputs,
        /// this endpoint can be used to submit the outputs from the tool calls once they're all completed.
        /// All outputs must be submitted in a single request.
        /// </summary>
        /// <param name="threadId">The id of the thread to which this run belongs.</param>
        /// <param name="runId">The id of the run that requires the tool output submission.</param>
        /// <param name="request"><see cref="SubmitToolOutputsRequest"/>.</param>
        /// <param name="streamEventHandler">Optional, <see cref="Func{IServerSentEvent, Task}"/> stream callback handler.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="RunResponse"/>.</returns>
        public async Task<RunResponse> SubmitToolOutputsAsync(string threadId, string runId, SubmitToolOutputsRequest request, Func<IServerSentEvent, Task> streamEventHandler = null, CancellationToken cancellationToken = default)
            => await SubmitToolOutputsAsync(threadId, runId, request, streamEventHandler == null ? null : async (_, serverSentEvent) =>
            {
                await streamEventHandler.Invoke(serverSentEvent);
            }, cancellationToken);

        /// <summary>
        /// When a run has the status: "requires_action" and required_action.type is submit_tool_outputs,
        /// this endpoint can be used to submit the outputs from the tool calls once they're all completed.
        /// All outputs must be submitted in a single request.
        /// </summary>
        /// <param name="threadId">The id of the thread to which this run belongs.</param>
        /// <param name="runId">The id of the run that requires the tool output submission.</param>
        /// <param name="request"><see cref="SubmitToolOutputsRequest"/>.</param>
        /// <param name="streamEventHandler">Optional, <see cref="Func{String, IServerSentEvent, Task}"/> stream callback handler.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="RunResponse"/>.</returns>
        public async Task<RunResponse> SubmitToolOutputsAsync(string threadId, string runId, SubmitToolOutputsRequest request, Func<string, IServerSentEvent, Task> streamEventHandler = null, CancellationToken cancellationToken = default)
        {
            request.Stream = streamEventHandler != null;
            var payload = JsonConvert.SerializeObject(request, OpenAIClient.JsonSerializationOptions);
            var endpoint = GetUrl($"/{threadId}/runs/{runId}/submit_tool_outputs");

            if (request.Stream)
            {
                return await StreamRunAsync(endpoint, payload, streamEventHandler, cancellationToken);
            }

            var response = await Rest.PostAsync(endpoint, payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<RunResponse>(client);
        }

        /// <summary>
        /// Returns a list of run steps belonging to a run.
        /// </summary>
        /// <param name="threadId">The id of the thread to which the run and run step belongs.</param>
        /// <param name="runId">The id of the run to which the run step belongs.</param>
        /// <param name="query">Optional, <see cref="ListQuery"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ListResponse{RunStep}"/>.</returns>
        public async Task<ListResponse<RunStepResponse>> ListRunStepsAsync(string threadId, string runId, ListQuery query = null, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/{threadId}/runs/{runId}/steps", query), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<ListResponse<RunStepResponse>>(client);
        }

        /// <summary>
        /// Retrieves a run step.
        /// </summary>
        /// <param name="threadId">The id of the thread to which the run and run step belongs.</param>
        /// <param name="runId">The id of the run to which the run step belongs.</param>
        /// <param name="stepId">The id of the run step to retrieve.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="RunStepResponse"/>.</returns>
        public async Task<RunStepResponse> RetrieveRunStepAsync(string threadId, string runId, string stepId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/{threadId}/runs/{runId}/steps/{stepId}"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<RunStepResponse>(client);
        }

        /// <summary>
        /// Cancels a run that is <see cref="RunStatus.InProgress"/>.
        /// </summary>
        /// <param name="threadId">The id of the thread to which this run belongs.</param>
        /// <param name="runId">The id of the run to cancel.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="RunResponse"/>.</returns>
        public async Task<bool> CancelRunAsync(string threadId, string runId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.PostAsync(GetUrl($"/{threadId}/runs/{runId}/cancel"), string.Empty, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            var run = response.Deserialize<RunResponse>(client);

            if (run.Status < RunStatus.Cancelling)
            {
                try
                {
                    run = await run.WaitForStatusChangeAsync(cancellationToken: cancellationToken);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return run.Status >= RunStatus.Cancelling;
        }

        #endregion Runs

        #region Files (Obsolete)

        /// <summary>
        /// Returns a list of message files.
        /// </summary>
        /// <param name="threadId">The id of the thread that the message and files belong to.</param>
        /// <param name="messageId">The id of the message that the files belongs to.</param>
        /// <param name="query"><see cref="ListQuery"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ListResponse{ThreadMessageFile}"/>.</returns>
        [Obsolete("Files removed from Assistants. Files now belong to ToolResources.")]
        public async Task<ListResponse<MessageFileResponse>> ListFilesAsync(string threadId, string messageId, ListQuery query = null, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/{threadId}/messages/{messageId}/files", query), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<ListResponse<MessageFileResponse>>(client);
        }

        /// <summary>
        /// Retrieve message file.
        /// </summary>
        /// <param name="threadId">The id of the thread to which the message and file belong.</param>
        /// <param name="messageId">The id of the message the file belongs to.</param>
        /// <param name="fileId">The id of the file being retrieved.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="MessageFileResponse"/>.</returns>
        [Obsolete("Files removed from Assistants. Files now belong to ToolResources.")]
        public async Task<MessageFileResponse> RetrieveFileAsync(string threadId, string messageId, string fileId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/{threadId}/messages/{messageId}/files/{fileId}"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<MessageFileResponse>(client);
        }

        #endregion Files (Obsolete)

        private async Task<RunResponse> StreamRunAsync(string endpoint, string payload, Func<string, IServerSentEvent, Task> streamEventHandler, CancellationToken cancellationToken = default)
        {
            RunResponse run = null;
            RunStepResponse runStep = null;
            MessageResponse message = null;

            var response = await Rest.PostAsync(endpoint, payload, async (sseResponse, ssEvent) =>
            {
                IServerSentEvent serverSentEvent = default;
                var @event = ssEvent.Value.Value<string>();

                try
                {
                    switch (ssEvent.Value.Value<string>())
                    {
                        case "thread.created":
                            serverSentEvent = sseResponse.Deserialize<ThreadResponse>(client);

                            break;
                        case "thread.run.created":
                        case "thread.run.queued":
                        case "thread.run.in_progress":
                        case "thread.run.requires_action":
                        case "thread.run.completed":
                        case "thread.run.incomplete":
                        case "thread.run.failed":
                        case "thread.run.cancelling":
                        case "thread.run.cancelled":
                        case "thread.run.expired":
                            var partialRun = sseResponse.Deserialize<RunResponse>(client);

                            if (run == null)
                            {
                                run = partialRun;
                            }
                            else
                            {
                                run.AppendFrom(partialRun);
                            }

                            serverSentEvent = run;

                            break;
                        case "thread.run.step.created":
                        case "thread.run.step.in_progress":
                        case "thread.run.step.delta":
                        case "thread.run.step.completed":
                        case "thread.run.step.failed":
                        case "thread.run.step.cancelled":
                        case "thread.run.step.expired":
                            var partialRunStep = sseResponse.Deserialize<RunStepResponse>(client);

                            if (runStep == null ||
                                runStep.Id != partialRunStep.Id)
                            {
                                runStep = partialRunStep;
                            }
                            else
                            {
                                runStep.AppendFrom(partialRunStep);
                            }

                            serverSentEvent = runStep;

                            break;
                        case "thread.message.created":
                        case "thread.message.in_progress":
                        case "thread.message.delta":
                        case "thread.message.completed":
                        case "thread.message.incomplete":
                            var partialMessage = sseResponse.Deserialize<MessageResponse>(client);

                            if (message == null ||
                                message.Id != partialMessage.Id)
                            {
                                message = partialMessage;
                            }
                            else
                            {
                                message.AppendFrom(partialMessage);
                            }

                            serverSentEvent = message;

                            break;
                        case "error":
                            serverSentEvent = sseResponse.Deserialize<Error>(client);

                            break;
                        default:
                            // if not properly handled raise it up to caller to deal with it themselves.
                            serverSentEvent = ssEvent;
                            break;
                    }

                }
                catch (Exception e)
                {
                    @event = "error";
                    serverSentEvent = new Error(e);
                }
                finally
                {
                    await streamEventHandler.Invoke(@event, serverSentEvent);
                }
            }, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            if (run == null) { return null; }
            run = await run.WaitForStatusChangeAsync(timeout: -1, cancellationToken: cancellationToken);
            run.SetResponseData(response, client);
            return run;
        }
    }
}
