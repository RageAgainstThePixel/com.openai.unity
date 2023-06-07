// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            var jsonContent = JsonConvert.SerializeObject(chatRequest, client.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl("/completions"), jsonContent, new RestParameters(client.DefaultRequestHeaders), cancellationToken).ConfigureAwait(false);
            response.ValidateResponse();
            return response.DeserializeResponse<ChatResponse>(response.ResponseBody, client.JsonSerializationOptions);
        }

        /// <summary>
        /// Created a completion for the chat message and stream the results to the <paramref name="resultHandler"/> as they come in.
        /// </summary>
        /// <param name="chatRequest">The chat request which contains the message content.</param>
        /// <param name="resultHandler">An action to be called as each new result arrives.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ChatResponse"/>.</returns>
        /// <exception cref="HttpRequestException">Raised when the HTTP request fails</exception>
        public async Task<ChatResponse> StreamCompletionAsync(ChatRequest chatRequest, Action<ChatResponse> resultHandler, CancellationToken cancellationToken = default)
        {
            chatRequest.Stream = true;
            ChatResponse partialResponse = null;

            var choiceCount = chatRequest.Number ?? 1;
            var finishReasons = new List<string>(choiceCount);
            var partials = new List<StringBuilder>(choiceCount);

            for (var i = 0; i < choiceCount; i++)
            {
                finishReasons.Add(string.Empty);
                partials.Add(new StringBuilder());
            }

            var jsonContent = JsonConvert.SerializeObject(chatRequest, client.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl("/completions"), jsonContent, eventData =>
            {
                if (string.IsNullOrWhiteSpace(eventData)) { return; }

                partialResponse = JsonConvert.DeserializeObject<ChatResponse>(eventData, client.JsonSerializationOptions);

                foreach (var choice in partialResponse.Choices)
                {
                    partials[choice.Index].Append(choice.ToString());

                    if (!string.IsNullOrWhiteSpace(choice.FinishReason))
                    {
                        finishReasons[choice.Index] = choice.FinishReason;
                    }
                }

                resultHandler(partialResponse);
            }, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.ValidateResponse();

            if (partialResponse == null) { return null; }

            partialResponse.SetResponseData(response);

            var finalChoices = new List<Choice>(choiceCount);

            for (var i = 0; i < choiceCount; i++)
            {
                finalChoices.Add(new Choice(new Message(Role.Assistant, partials[i].ToString()), null, finishReasons[i], i));
            }

            var finalResponse = new ChatResponse(
                partialResponse.Id,
                partialResponse.Object,
                partialResponse.Created,
                partialResponse.Model,
                partialResponse.Usage,
                finalChoices);
            resultHandler(finalResponse);
            return finalResponse;
        }
    }
}
