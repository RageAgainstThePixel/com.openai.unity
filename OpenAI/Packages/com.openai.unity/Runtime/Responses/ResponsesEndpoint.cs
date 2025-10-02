// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenAI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;
using Utilities.WebRequestRest;
using Utilities.WebRequestRest.Interfaces;

namespace OpenAI.Responses
{
    /// <summary>
    /// OpenAI's most advanced interface for generating model responses.
    /// Supports text and image inputs, and text outputs.
    /// Create stateful interactions with the model, using the output of previous responses as input.
    /// Extend the model's capabilities with built-in tools for file search, web search, computer use, and more.
    /// Allow the model access to external systems and data using function calling.
    /// </summary>
    public sealed class ResponsesEndpoint : OpenAIBaseEndpoint
    {
        public ResponsesEndpoint(OpenAIClient client) : base(client) { }

        protected override string Root => "responses";

        /// <summary>
        /// Creates a model response.
        /// Provide text or image inputs to generate text or JSON outputs.
        /// Have the model call your own custom code or use built-in tools like web search or file search to use your own data as input for the model's response.
        /// </summary>
        /// <param name="request"><see cref="CreateResponseRequest"/>.</param>
        /// <param name="streamEventHandler">Optional, <see cref="Func{IServerSentEvent, Task}"/> stream callback handler.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="Response"/>.</returns>
        public async Task<Response> CreateModelResponseAsync(CreateResponseRequest request, Func<IServerSentEvent, Task> streamEventHandler, CancellationToken cancellationToken = default)
            => await CreateModelResponseAsync(request, streamEventHandler == null ? null : (_, e) => streamEventHandler(e), cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Creates a model response.
        /// Provide text or image inputs to generate text or JSON outputs.
        /// Have the model call your own custom code or use built-in tools like web search or file search to use your own data as input for the model's response.
        /// </summary>
        /// <typeparam name="T"><see cref="JsonSchema"/></typeparam>
        /// <param name="request"><see cref="CreateResponseRequest"/>.</param>
        /// <param name="streamEventHandler">Optional, <see cref="Func{IServerSentEvent, Task}"/> stream callback handler.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="Response"/>.</returns>
        public async Task<(T, Response)> CreateModelResponseAsync<T>(CreateResponseRequest request, Func<IServerSentEvent, Task> streamEventHandler, CancellationToken cancellationToken = default)
            => await CreateModelResponseAsync<T>(request, streamEventHandler == null ? null : (_, e) => streamEventHandler(e), cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Creates a model response.
        /// Provide text or image inputs to generate text or JSON outputs.
        /// Have the model call your own custom code or use built-in tools like web search or file search to use your own data as input for the model's response.
        /// </summary>
        /// <param name="request"><see cref="CreateResponseRequest"/>.</param>
        /// <param name="streamEventHandler"></param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="Response"/>.</returns>
        public async Task<Response> CreateModelResponseAsync(CreateResponseRequest request, Func<string, IServerSentEvent, Task> streamEventHandler = null, CancellationToken cancellationToken = default)
        {
            var endpoint = GetUrl();
            request.Stream = streamEventHandler != null;
            var payload = JsonConvert.SerializeObject(request, OpenAIClient.JsonSerializationOptions);

            if (request.Stream)
            {
                return await StreamResponseAsync(endpoint, payload, streamEventHandler, cancellationToken);
            }

            var response = await Rest.PostAsync(endpoint, payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<Response>(client);
        }

        /// <summary>
        /// Creates a model response.
        /// Provide text or image inputs to generate text or JSON outputs.
        /// Have the model call your own custom code or use built-in tools like web search or file search to use your own data as input for the model's response.
        /// </summary>
        /// <typeparam name="T"><see cref="JsonSchema"/></typeparam>
        /// <param name="request"><see cref="CreateResponseRequest"/>.</param>
        /// <param name="streamEventHandler">Optional, <see cref="Func{String, IServerSentEvent, Task}"/> stream callback handler.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="Response"/>.</returns>
        public async Task<(T, Response)> CreateModelResponseAsync<T>(CreateResponseRequest request, Func<string, IServerSentEvent, Task> streamEventHandler = null, CancellationToken cancellationToken = default)
        {
            var endpoint = GetUrl();
            request.Stream = streamEventHandler != null;
            request.TextResponseFormatObject = new TextResponseFormatObject(new TextResponseFormatConfiguration(typeof(T)));
            var payload = JsonConvert.SerializeObject(request, OpenAIClient.JsonSerializationOptions);

            T output = default;
            Response response;

            if (request.Stream)
            {
                response = await StreamResponseAsync(endpoint, payload, streamEventHandler, cancellationToken);
            }
            else
            {
                var httpResponseMessage = await Rest.PostAsync(endpoint, payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
                httpResponseMessage.Validate(EnableDebug);
                response = httpResponseMessage.Deserialize<Response>(client);
            }

            var lastItem = response.Output?.LastOrDefault();


            if (lastItem is Message messageItem)
            {
                if (EnableDebug)
                {
                    Debug.Log($"{messageItem.Role}: {messageItem}");
                }

                output = JsonConvert.DeserializeObject<T>(messageItem.ToString(), OpenAIClient.JsonSerializationOptions);
            }

            return (output, response);
        }

        [Preserve]
        private async Task<Response> StreamResponseAsync(string endpoint, string payload, Func<string, IServerSentEvent, Task> streamEventHandler, CancellationToken cancellationToken)
        {
            Response response = null;

            var streamResponse = await Rest.PostAsync(endpoint, payload, async (sseResponse, ssEvent) =>
            {
                IServerSentEvent serverSentEvent = null;
                var @event = ssEvent.Value.Value<string>();
                var @object = ssEvent.Data ?? ssEvent.Value;

                if (EnableDebug)
                {
                    Debug.Log(@object.ToString(Formatting.None));
                }

                var text = @object["text"]?.Value<string>();
                var delta = @object["delta"]?.Value<string>();
                var itemId = @object["item_id"]?.Value<string>();
                var outputIndex = @object["output_index"]?.Value<int>();
                var contentIndex = @object["content_index"]?.Value<int>();

                // ReSharper disable once AccessToModifiedClosure
                try
                {
                    switch (@event)
                    {
                        case "response.created":
                        case "response.queued":
                        case "response.in_progress":
                        case "response.completed":
                        case "response.failed":
                        case "response.incomplete":
                        {
                            var partialResponse = sseResponse.Deserialize<Response>(@object["response"], client);

                            if (response == null || response.Id == partialResponse.Id)
                            {
                                response = partialResponse;
                            }
                            else
                            {
                                throw new InvalidOperationException($"Response ID mismatch! Expected: {response.Id}, got: {partialResponse.Id}");
                            }

                            serverSentEvent = response;
                            break;
                        }
                        case "response.content_part.added":
                        case "response.content_part.done":
                        {
                            var part = sseResponse.Deserialize<IResponseContent>(@object["part"], client);
                            var messageItem = (Message)response!.Output[outputIndex!.Value];

                            if (messageItem.Id != itemId)
                            {
                                throw new InvalidOperationException($"MessageItem ID mismatch! Expected: {messageItem.Id}, got: {itemId}");
                            }

                            messageItem.AddContentItem(part, contentIndex!.Value);

                            if (@event == "response.content_part.done")
                            {
                                serverSentEvent = part;
                            }

                            break;
                        }
                        case "response.output_item.added":
                        case "response.output_item.done":
                        {
                            var item = sseResponse.Deserialize<IResponseItem>(@object["item"], client);
                            response!.InsertOutputItem(item, outputIndex!.Value);

                            if (@event == "response.output_item.done")
                            {
                                serverSentEvent = item;
                            }
                            break;
                        }
                        case "response.function_call_arguments.delta":
                        case "response.function_call_arguments.done":
                        {
                            var functionToolCall = (FunctionToolCall)response!.Output[outputIndex!.Value];

                            if (functionToolCall.Id != itemId)
                            {
                                throw new InvalidOperationException($"FunctionToolCall ID mismatch! Expected: {functionToolCall.Id}, got: {itemId}");
                            }

                            if (!string.IsNullOrWhiteSpace(delta))
                            {
                                functionToolCall.Delta = delta;
                            }

                            break;
                        }
                        case "response.image_generation_call.in_progress":
                        case "response.image_generation_call.generating":
                        case "response.image_generation_call.partial_image":
                        case "response.image_generation_call.completed":
                        {
                            var imageGenerationCall = (ImageGenerationCall)response!.Output[outputIndex!.Value];

                            if (imageGenerationCall.Id != itemId)
                            {
                                throw new InvalidOperationException($"ImageGenerationCall ID mismatch! Expected: {imageGenerationCall.Id}, got: {itemId}");
                            }

                            imageGenerationCall.Size = @object["size"]?.Value<string>();
                            imageGenerationCall.Quality = @object["quality"]?.Value<string>();
                            imageGenerationCall.Background = @object["background"]?.Value<string>();
                            imageGenerationCall.OutputFormat = @object["output_format"]?.Value<string>();
                            imageGenerationCall.RevisedPrompt = @object["revised_prompt"]?.Value<string>();
                            imageGenerationCall.PartialImageIndex = @object["partial_image_index"]?.Value<int>();
                            imageGenerationCall.PartialImageResult = @object["partial_image_b64"]?.Value<string>();

                            serverSentEvent = imageGenerationCall;
                            break;
                        }
                        case "response.audio.delta":
                        case "response.audio.done":
                        case "response.audio.transcript.delta":
                        case "response.audio.transcript.done":
                        case "response.output_text.annotation.added":
                        case "response.output_text.delta":
                        case "response.output_text.done":
                        case "response.refusal.delta":
                        case "response.refusal.done":
                        case "response.reasoning_summary_text.delta":
                        case "response.reasoning_summary_text.done":
                        {
                            var messageItem = (Message)response!.Output[outputIndex!.Value];

                            if (messageItem.Id != itemId)
                            {
                                throw new InvalidOperationException($"MessageItem ID mismatch! Expected: {messageItem.Id}, got: {itemId}");
                            }

                            var contentItem = messageItem.Content[contentIndex!.Value];

                            switch (contentItem)
                            {
                                case AudioContent audioContent:
                                    AudioContent partialContent;

                                    switch (@event)
                                    {
                                        case "response.audio.delta":
                                            partialContent = new AudioContent(audioContent.Type, base64Data: delta);
                                            audioContent.AppendFrom(partialContent);
                                            serverSentEvent = partialContent;
                                            break;
                                        case "response.audio.transcript.delta":
                                            partialContent = new AudioContent(audioContent.Type, transcript: delta);
                                            audioContent.AppendFrom(partialContent);
                                            serverSentEvent = partialContent;
                                            break;
                                        case "response.audio.done":
                                        case "response.audio.transcript.done":
                                            serverSentEvent = audioContent;
                                            break;
                                        default:
                                            throw new InvalidOperationException($"Unexpected event type: {@event} for AudioContent.");
                                    }

                                    break;
                                case TextContent textContent:
                                    if (!string.IsNullOrWhiteSpace(text))
                                    {
                                        textContent.Text = text;
                                    }

                                    textContent.Delta = !string.IsNullOrWhiteSpace(delta) ? delta : null;

                                    var annotationIndex = @object["annotation_index"]?.Value<int>();

                                    if (annotationIndex.HasValue)
                                    {
                                        var annotation = sseResponse.Deserialize<IAnnotation>(@object["annotation"], client);
                                        textContent.InsertAnnotation(annotation, annotationIndex.Value);
                                    }

                                    serverSentEvent = textContent;
                                    break;
                                case RefusalContent refusalContent:
                                    var refusal = @object["refusal"]?.Value<string>();

                                    if (!string.IsNullOrWhiteSpace(refusal))
                                    {
                                        refusalContent.Refusal = refusal;
                                    }

                                    if (!string.IsNullOrWhiteSpace(delta))
                                    {
                                        refusalContent.Delta = delta;
                                    }

                                    serverSentEvent = refusalContent;
                                    break;
                                case ReasoningContent reasoningContent:
                                    if (!string.IsNullOrWhiteSpace(text))
                                    {
                                        reasoningContent.Text = text;
                                    }

                                    if (!string.IsNullOrWhiteSpace(delta))
                                    {
                                        reasoningContent.Delta = delta;
                                    }

                                    break;
                            }

                            break;
                        }
                        case "response.reasoning_summary_part.added":
                        case "response.reasoning_summary_part.done":
                        {
                            var summaryIndex = @object["summary_index"]!.Value<int>();
                            var reasoningItem = (ReasoningItem)response!.Output[outputIndex!.Value];
                            var summaryItem = sseResponse.Deserialize<ReasoningSummary>(@object["part"], client);
                            reasoningItem.InsertSummary(summaryItem, summaryIndex);

                            if (@event == "response.reasoning_summary_part.done")
                            {
                                serverSentEvent = summaryItem;
                            }

                            break;
                        }
                        case "response.reasoning_text.delta":
                        case "response.reasoning_text.done":
                        {
                            var reasoningContentIndex = @object["content_index"]!.Value<int>();
                            var reasoningItem = (ReasoningItem)response!.Output[outputIndex!.Value];
                            var reasoningContentItem = reasoningItem.Content[reasoningContentIndex];

                            if (!string.IsNullOrWhiteSpace(text))
                            {
                                reasoningContentItem.Text = text;
                            }

                            reasoningContentItem.Delta = !string.IsNullOrWhiteSpace(delta) ? delta : null;
                            serverSentEvent = reasoningContentItem;
                            break;
                        }
                        case "error":
                        {
                            serverSentEvent = sseResponse.Deserialize<Error>(client);
                            break;
                        }
                        // TODO - implement handling for these events:
                        case "response.code_interpreter_call.interpreting":
                        case "response.code_interpreter_call.in_progress":
                        case "response.code_interpreter_call.completed":
                        case "response.code_interpreter_call_code.delta":
                        case "response.code_interpreter_call_code.done":
                        case "response.custom_tool_call_input.delta":
                        case "response.custom_tool_call_input.done":
                        case "response.file_search_call.in_progress":
                        case "response.file_search_call.searching":
                        case "response.file_search_call.completed":
                        case "response.mcp_call_arguments.delta":
                        case "response.mcp_call_arguments.done":
                        case "response.mcp_call.in_progress":
                        case "response.mcp_call.completed":
                        case "response.mcp_call.failed":
                        case "response.mcp_list_tools.in_progress":
                        case "response.mcp_list_tools.completed":
                        case "response.mcp_list_tools.failed":
                        case "response.web_search_call.in_progress":
                        case "response.web_search_call.searching":
                        case "response.web_search_call.completed":
                        default:
                        {
                            // if not properly handled raise it up to caller to deal with it themselves.
                            serverSentEvent = ssEvent;
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    @event = "error";
                    serverSentEvent = new Error(e);
                }
                finally
                {
                    serverSentEvent ??= ssEvent;
                    await streamEventHandler.Invoke(@event, serverSentEvent);
                }
            }, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            // ReSharper restore AccessToModifiedClosure
            streamResponse.Validate(EnableDebug);
            if (response == null) { return null; }
            response = await response.WaitForStatusChangeAsync(timeout: -1, cancellationToken: cancellationToken);
            response.SetResponseData(streamResponse, client);
            return response;
        }

        /// <summary>
        /// Retrieves a model response with the given ID.
        /// </summary>
        /// <param name="responseId">The ID of the response to retrieve.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <param name="include">
        /// Additional fields to include in the response.<br/>
        /// Currently supported values are:<br/>
        /// - file_search_call.results: Include the search results of the file search tool call.<br/>
        /// - message.input_image.image_url: Include image URLs from the computer call output.<br/>
        /// - computer_call_output.output.image_url: Include image urls from the computer call output.<br/>
        /// </param>
        /// <returns><see cref="Response"/>.</returns>
        public async Task<Response> GetModelResponseAsync(string responseId, CancellationToken cancellationToken = default, params string[] include)
        {
            var queryParameters = new Dictionary<string, string>();
            if (include is { Length: > 0 })
            {
                queryParameters.Add("include", string.Join(",", include));
            }
            var response = await Rest.GetAsync(GetUrl($"/{responseId}", queryParameters), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<Response>(client);
        }

        /// <summary>
        /// Returns a list of input items for a given response.
        /// </summary>
        /// <param name="responseId">The ID of the response to retrieve input items for.</param>
        /// <param name="query"><see cref="ListQuery"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ListResponse{BaseResponse}"/>.</returns>
        public async Task<ListResponse<IResponseItem>> ListInputItemsAsync(string responseId, ListQuery query = null, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/{responseId}/input_items", query), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<ListResponse<IResponseItem>>(client);
        }

        /// <summary>
        /// Cancels a model response with the given ID.
        /// </summary>
        /// <remarks>
        /// Only responses created with the background parameter set to true can be cancelled.
        /// </remarks>
        /// <param name="responseId">The ID of the response to cancel.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="Response"/>.</returns>
        public async Task<Response> CancelModelResponsesAsync(string responseId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.PostAsync(GetUrl($"/{responseId}/cancel"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<Response>(client);
        }

        /// <summary>
        /// Deletes a model response with the given ID.
        /// </summary>
        /// <param name="responseId">The ID of the response to delete.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>True if the response was deleted, false otherwise.</returns>
        public async Task<bool> DeleteModelResponseAsync(string responseId, CancellationToken cancellationToken = default)
        {
            var response = await Rest.DeleteAsync(GetUrl($"/{responseId}"), new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            var result = response.Deserialize<DeletedResponse>(client);
            return result.Deleted;
        }
    }
}
