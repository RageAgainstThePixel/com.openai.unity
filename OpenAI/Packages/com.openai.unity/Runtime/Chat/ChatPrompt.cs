// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;

namespace OpenAI.Chat
{
    [Obsolete("Use OpenAI.Chat.Message instead")]
    public sealed class ChatPrompt
    {
        [Obsolete("Use OpenAI.Chat.Message instead")]
        public ChatPrompt(string role, string content)
        {
            Role = role.ToLower() switch
            {
                "system" => Role.System,
                "assistant" => Role.Assistant,
                "user" => Role.User,
                _ => throw new ArgumentException(nameof(role))
            };
            Content = content;
        }

        [JsonConstructor]
        public ChatPrompt(
            [JsonProperty("role")] Role role,
            [JsonProperty("content")] string content)
        {
            Role = role;
            Content = content;
        }

        [JsonProperty("role")]
        public Role Role { get; }

        [JsonProperty("content")]
        public string Content { get; }

        public static implicit operator ChatPrompt(Message message) => new ChatPrompt(message.Role, message.Content);

        public static implicit operator Message(ChatPrompt message) => new Message(message.Role, message.Content);
    }
}
