// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;

namespace OpenAI.Chat
{
    public sealed class Message
    {
        [Obsolete("Use new constructor with enum Role")]
        public Message(string role, string content)
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
        public Message(
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

        public static implicit operator string(Message message) => message.Content;
    }
}
