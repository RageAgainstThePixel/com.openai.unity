using System.Collections.Generic;
using System.Linq;

namespace OpenAI.Chat
{
    public sealed class ChatRequest
    {
        public ChatRequest(IEnumerable<ChatPrompt> prompts)
        {
            Prompts = prompts.ToList();
        }

        public IReadOnlyList<ChatPrompt> Prompts { get; }
    }
}
