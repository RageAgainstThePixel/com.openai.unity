// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Chat
{
    public sealed class Message
    {
        [JsonConstructor]
        public Message(
            [JsonProperty("role")] string role,
            [JsonProperty("content")] string content)
        {
            Role = role;
            Content = content;
        }

        [JsonProperty("role")]
        public string Role { get; }

        [JsonProperty("content")]
        public string Content { get; }

        public override string ToString() => Content;

        public static implicit operator string(Message message) => message.Content;
    }
}
