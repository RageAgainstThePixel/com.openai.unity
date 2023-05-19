// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities.Async;
using Utilities.Rest.Extensions;

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
        /// <exception cref="HttpRequestException">Raised when the HTTP request fails</exception>
        public async Task<ChatResponse> GetCompletionAsync(ChatRequest chatRequest, CancellationToken cancellationToken = default)
        {
            var jsonContent = JsonConvert.SerializeObject(chatRequest, client.JsonSerializationOptions).ToJsonStringContent();
            var response = await client.Client.PostAsync(GetUrl("/completions"), jsonContent, cancellationToken).ConfigureAwait(false);
            var responseAsString = await response.ReadAsStringAsync();
            return response.DeserializeResponse<ChatResponse>(responseAsString, client.JsonSerializationOptions);
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
            var jsonContent = JsonConvert.SerializeObject(chatRequest, client.JsonSerializationOptions).ToJsonStringContent();
            using var request = new HttpRequestMessage(HttpMethod.Post, GetUrl("/completions"))
            {
                Content = jsonContent
            };
            var response = await client.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            await response.CheckResponseAsync();
            await using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);
            var choiceCount = chatRequest.Number ?? 1;
            ChatResponse partialResponse = null;
            var finishReasons = new List<string>(choiceCount);
            var partials = new List<StringBuilder>(choiceCount);

            for (var i = 0; i < choiceCount; i++)
            {
                finishReasons.Add(string.Empty);
                partials.Add(new StringBuilder());
            }

            while (await reader.ReadLineAsync() is { } streamData)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (streamData.TryGetEventStreamData(out var eventData))
                {
                    if (string.IsNullOrWhiteSpace(eventData)) { continue; }

                    partialResponse = response.DeserializeResponse<ChatResponse>(eventData, client.JsonSerializationOptions);

                    foreach (var choice in partialResponse.Choices)
                    {
                        partials[choice.Index].Append(choice.ToString());

                        if (!string.IsNullOrWhiteSpace(choice.FinishReason))
                        {
                            finishReasons[choice.Index] = choice.FinishReason;
                        }
                    }

                    // Always raise event callbacks on main thread
                    await Awaiters.UnityMainThread;
                    resultHandler(partialResponse);
                }
                else
                {
                    if (partialResponse == null) { return null; }

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

                    // Always raise event callbacks on main thread
                    await Awaiters.UnityMainThread;
                    resultHandler(finalResponse);
                    return finalResponse;
                }
            }

            return null;
        }

        /// <summary>
        /// Created a completion for the chat message and stream the results as they come in.<br/>
        /// If you are not using C# 8 supporting IAsyncEnumerable{T} or if you are using the .NET Framework,
        /// you may need to use <see cref="StreamCompletionAsync(ChatRequest, Action{ChatResponse}, CancellationToken)"/> instead.
        /// </summary>
        /// <param name="chatRequest">The chat request which contains the message content.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ChatResponse"/>.</returns>
        /// <exception cref="HttpRequestException">Raised when the HTTP request fails</exception>
        public async IAsyncEnumerable<ChatResponse> StreamCompletionEnumerableAsync(ChatRequest chatRequest, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            chatRequest.Stream = true;
            var jsonContent = JsonConvert.SerializeObject(chatRequest, client.JsonSerializationOptions).ToJsonStringContent();
            using var request = new HttpRequestMessage(HttpMethod.Post, GetUrl("/completions"))
            {
                Content = jsonContent
            };
            var response = await client.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            await response.CheckResponseAsync();
            await using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);
            var choiceCount = chatRequest.Number ?? 1;
            ChatResponse partialResponse = null;
            var finishReasons = new List<string>(choiceCount);
            var partials = new List<StringBuilder>(choiceCount);

            for (var i = 0; i < choiceCount; i++)
            {
                finishReasons.Add(string.Empty);
                partials.Add(new StringBuilder());
            }

            while (await reader.ReadLineAsync() is { } streamData)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (streamData.TryGetEventStreamData(out var eventData))
                {
                    if (string.IsNullOrWhiteSpace(eventData)) { continue; }

                    partialResponse = response.DeserializeResponse<ChatResponse>(eventData, client.JsonSerializationOptions);

                    foreach (var choice in partialResponse.Choices)
                    {
                        partials[choice.Index].Append(choice.ToString());

                        if (!string.IsNullOrWhiteSpace(choice.FinishReason))
                        {
                            finishReasons[choice.Index] = choice.FinishReason;
                        }
                    }

                    yield return partialResponse;
                }
                else
                {
                    if (partialResponse == null) { yield break; }

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
                    yield return finalResponse;
                    yield break;
                }
            }
        }
    }
}
