using System;
using OpenAI.Models;
using System.Collections.Generic;
using System.Linq;

namespace OpenAI.Chat
{
    public sealed class ChatRequest
    {
        public ChatRequest(IEnumerable<ChatPrompt> prompts, Model model = null)
        {
            Model = model ?? Model.GPT3_5_Turbo;

            if (!Model.Id.Contains("gpt-3.5-turbo"))
            {
                throw new ArgumentException(nameof(model), $"{Model.Id} not supported");
            }

            Prompts = prompts.ToList();
        }

        private Model Model { get; }

        public IReadOnlyList<ChatPrompt> Prompts { get; }
    }
}
