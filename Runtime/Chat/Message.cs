// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine;

namespace OpenAI.Chat
{
    [Serializable]
    public sealed class Message
    {
        [Obsolete("Use new constructor with enum Role")]
        public Message(string role, string content)
        {
            this.role = role.ToLower() switch
            {
                "system" => Role.System,
                "assistant" => Role.Assistant,
                "user" => Role.User,
                _ => throw new ArgumentException(nameof(role))
            };
            this.content = content;
        }

        [JsonConstructor]
        public Message(
            [JsonProperty("role")] Role role,
            [JsonProperty("content")] string content)
        {
            this.role = role;
            this.content = content;
        }

        [SerializeField]
        private Role role;

        [JsonProperty("role")]
        public Role Role => role;

        [SerializeField]
        [TextArea(1, 30)]
        private string content;

        [JsonProperty("content")]
        public string Content => content;

        public static implicit operator string(Message message) => message.Content;
    }
}
