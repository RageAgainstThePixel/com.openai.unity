// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Collections.Generic;
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
        /// <inheritdoc />
        public ChatEndpoint(OpenAIClient client) : base(client) { }

        /// <inheritdoc />
        protected override string Root => "chat";

        /// <summary>
        /// Creates a completion for the chat message.
        /// </summary>
        /// <param name="chatRequest">The chat request which contains the message content.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ChatResponse"/>.</returns>
        public async Task<ChatResponse> GetCompletionAsync(ChatRequest chatRequest, CancellationToken cancellationToken = default)
        {
            var payload = JsonConvert.SerializeObject(chatRequest, client.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl("/completions"), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate();
            return response.DeserializeResponse<ChatResponse>(response.Body, client.JsonSerializationOptions);
        }

        /// <summary>
        /// Created a completion for the chat message and stream the results to the <paramref name="resultHandler"/> as they come in.
        /// </summary>
        /// <param name="chatRequest">The chat request which contains the message content.</param>
        /// <param name="resultHandler">An action to be called as each new result arrives.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ChatResponse"/>.</returns>
        public async Task<ChatResponse> StreamCompletionAsync(ChatRequest chatRequest, Action<ChatResponse> resultHandler, CancellationToken cancellationToken = default)
        {
            chatRequest.Stream = true;
            ChatResponse chatResponse = null;

            var payload = JsonConvert.SerializeObject(chatRequest, client.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl("/completions"), payload, eventData =>
            {
                try
                {
                    var partialResponse = JsonConvert.DeserializeObject<ChatResponse>(eventData, client.JsonSerializationOptions);

                    if (chatResponse == null)
                    {
                        chatResponse = new ChatResponse(partialResponse);
                    }
                    else
                    {
                        chatResponse.CopyFrom(partialResponse);
                    }

                    resultHandler(partialResponse);
                }
                catch (Exception e)
                {
                    Debug.LogError($"{eventData}\n{e}");
                }
            }, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate();

            if (chatResponse == null) { return null; }

            chatResponse.SetResponseData(response);
            resultHandler(chatResponse);
            return chatResponse;
        }

        [Obsolete("Use StreamCompletionAsync")]
        public IAsyncEnumerable<ChatResponse> StreamCompletionEnumerableAsync(ChatRequest chatRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
