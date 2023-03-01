// Licensed under the MIT License. See LICENSE in the project root for license information.

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
        public async Task GetCompletionAsync()
        {
            await Task.CompletedTask;
        }
    }
}
