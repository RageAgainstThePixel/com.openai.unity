// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OpenAI.Chat
{
    public sealed class ChatEndpoint : BaseEndPoint
    {
        public ChatEndpoint(OpenAIClient api) : base(api) { }

        protected override string GetEndpoint()
            => $"{Api.BaseUrl}chat";

        /// <summary>
        /// Creates a completion for the chat message
        /// </summary>
        /// <param name="chatRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ChatResponse> GetCompletionAsync(ChatRequest chatRequest, CancellationToken cancellationToken = default)
        {
            var payload = JsonConvert.SerializeObject(chatRequest, Api.JsonSerializationOptions).ToJsonStringContent();
            var result = await Api.Client.PostAsync($"{GetEndpoint()}/completions", payload, cancellationToken);
            var resultAsString = await result.ReadAsStringAsync(true);
            return JsonConvert.DeserializeObject<ChatResponse>(resultAsString, Api.JsonSerializationOptions);
        }
    }
}
