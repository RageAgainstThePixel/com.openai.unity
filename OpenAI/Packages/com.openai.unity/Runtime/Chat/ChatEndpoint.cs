// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utilities.WebRequestRest;

namespace OpenAI.Chat
{
    /// <summary>
    /// Given a chat conversation, the model will return a chat completion response.<br/>
    /// <see href="https://platform.openai.com/docs/api-reference/chat"/>
    /// </summary>
    public sealed class ChatEndpoint : OpenAIBaseEndpoint
    {
        internal ChatEndpoint(OpenAIClient client) : base(client) { }

        /// <inheritdoc />
        protected override string Root => "chat";

        protected override bool? IsAzureDeployment => true;

        /// <summary>
        /// Creates a completion for the chat message.
        /// </summary>
        /// <param name="chatRequest">The chat request which contains the message content.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ChatResponse"/>.</returns>
        public async Task<ChatResponse> GetCompletionAsync(ChatRequest chatRequest, CancellationToken cancellationToken = default)
        {
            var payload = JsonConvert.SerializeObject(chatRequest, OpenAIClient.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl("/completions"), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<ChatResponse>(client);
        }

        /// <summary>
        /// Creates a completion for the chat message.
        /// </summary>
        /// <typeparam name="T"><see cref="JsonSchema"/> to use for structured outputs.</typeparam>
        /// <param name="chatRequest">The chat request which contains the message content.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ChatResponse"/>.</returns>
        public async Task<(T, ChatResponse)> GetCompletionAsync<T>(ChatRequest chatRequest, CancellationToken cancellationToken = default)
        {
            chatRequest.ResponseFormatObject = new TextResponseFormatConfiguration(typeof(T));
            var response = await GetCompletionAsync(chatRequest, cancellationToken);
            var output = JsonConvert.DeserializeObject<T>(response.FirstChoice, OpenAIClient.JsonSerializationOptions);
            return (output, response);
        }

        /// <summary>
        /// Created a completion for the chat message and stream the results to the <paramref name="resultHandler"/> as they come in.
        /// </summary>
        /// <param name="chatRequest">The chat request which contains the message content.</param>
        /// <param name="resultHandler">A <see cref="Action{ChatResponse}"/> to be invoked as each new result arrives.</param>
        /// <param name="streamUsage">
        /// Optional, If set, an additional chunk will be streamed before the 'data: [DONE]' message.
        /// The 'usage' field on this chunk shows the token usage statistics for the entire request,
        /// and the 'choices' field will always be an empty array. All other chunks will also include a 'usage' field,
        /// but with a null value.
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ChatResponse"/>.</returns>
        public Task<ChatResponse> StreamCompletionAsync(ChatRequest chatRequest, Action<ChatResponse> resultHandler, bool streamUsage = false, CancellationToken cancellationToken = default)
            => StreamCompletionAsync(chatRequest, response =>
            {
                resultHandler.Invoke(response);
                return Task.CompletedTask;
            }, streamUsage, cancellationToken);

        /// <summary>
        /// Created a completion for the chat message and stream the results to the <paramref name="resultHandler"/> as they come in.
        /// </summary>
        /// <typeparam name="T"><see cref="JsonSchema"/> to use for structured outputs.</typeparam>
        /// <param name="chatRequest">The chat request which contains the message content.</param>
        /// <param name="resultHandler">A <see cref="Action{ChatResponse}"/> to be invoked as each new result arrives.</param>
        /// <param name="streamUsage">
        /// Optional, If set, an additional chunk will be streamed before the 'data: [DONE]' message.
        /// The 'usage' field on this chunk shows the token usage statistics for the entire request,
        /// and the 'choices' field will always be an empty array. All other chunks will also include a 'usage' field,
        /// but with a null value.
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ChatResponse"/>.</returns>
        public Task<(T, ChatResponse)> StreamCompletionAsync<T>(ChatRequest chatRequest, Action<ChatResponse> resultHandler, bool streamUsage = false, CancellationToken cancellationToken = default)
            => StreamCompletionAsync<T>(chatRequest, response =>
            {
                resultHandler.Invoke(response);
                return Task.CompletedTask;
            }, streamUsage, cancellationToken);

        /// <summary>
        /// Created a completion for the chat message and stream the results to the <paramref name="resultHandler"/> as they come in.
        /// </summary>
        /// <typeparam name="T"><see cref="JsonSchema"/> to use for structured outputs.</typeparam>
        /// <param name="chatRequest">The chat request which contains the message content.</param>
        /// <param name="resultHandler">A <see cref="Func{ChatResponse, Task}"/> to to be invoked as each new result arrives.</param>
        /// <param name="streamUsage">
        /// Optional, If set, an additional chunk will be streamed before the 'data: [DONE]' message.
        /// The 'usage' field on this chunk shows the token usage statistics for the entire request,
        /// and the 'choices' field will always be an empty array. All other chunks will also include a 'usage' field,
        /// but with a null value.
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ChatResponse"/>.</returns>
        public async Task<(T, ChatResponse)> StreamCompletionAsync<T>(ChatRequest chatRequest, Func<ChatResponse, Task> resultHandler, bool streamUsage = false, CancellationToken cancellationToken = default)
        {
            chatRequest.ResponseFormatObject = new TextResponseFormatConfiguration(typeof(T));
            var response = await StreamCompletionAsync(chatRequest, resultHandler, streamUsage, cancellationToken);
            var output = JsonConvert.DeserializeObject<T>(response.FirstChoice, OpenAIClient.JsonSerializationOptions);
            return (output, response);
        }

        /// <summary>
        /// Created a completion for the chat message and stream the results to the <paramref name="resultHandler"/> as they come in.
        /// </summary>
        /// <param name="chatRequest">The chat request which contains the message content.</param>
        /// <param name="resultHandler">A <see cref="Func{ChatResponse, Task}"/> to to be invoked as each new result arrives.</param>
        /// <param name="streamUsage">
        /// Optional, If set, an additional chunk will be streamed before the 'data: [DONE]' message.
        /// The 'usage' field on this chunk shows the token usage statistics for the entire request,
        /// and the 'choices' field will always be an empty array. All other chunks will also include a 'usage' field,
        /// but with a null value.
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ChatResponse"/>.</returns>
        public async Task<ChatResponse> StreamCompletionAsync(ChatRequest chatRequest, Func<ChatResponse, Task> resultHandler, bool streamUsage = false, CancellationToken cancellationToken = default)
        {
            if (chatRequest == null) { throw new ArgumentNullException(nameof(chatRequest)); }
            if (resultHandler == null) { throw new ArgumentNullException(nameof(resultHandler)); }
            chatRequest.Stream = true;
            chatRequest.StreamOptions = streamUsage ? new StreamOptions() : null;
            ChatResponse chatResponse = null;
            var payload = JsonConvert.SerializeObject(chatRequest, OpenAIClient.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl("/completions"), payload, async (sseResponse, ssEvent) =>
            {
                try
                {
                    if (ssEvent.Event != ServerSentEventKind.Data) { return; }

                    var partialResponse = sseResponse.Deserialize<ChatResponse>(client);

                    if (chatResponse == null)
                    {
                        chatResponse = new ChatResponse(partialResponse);
                    }
                    else
                    {
                        chatResponse.AppendFrom(partialResponse);
                    }

                    await resultHandler.Invoke(partialResponse);
                }
                catch (Exception e)
                {
                    Debug.LogError($"{ssEvent}\n{e}");
                }
            }, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            if (chatResponse == null) { return null; }
            chatResponse.SetResponseData(response, client);
            await resultHandler.Invoke(chatResponse);
            return chatResponse;
        }
    }
}
