using Newtonsoft.Json;

namespace OpenAI.Chat
{
    public sealed class Message
    {
        [JsonConstructor]
        public Message(
            [JsonProperty("role")] string role,
            [JsonProperty("content")] string content
        )
        {
            Role = role;
            Content = content;
        }

        [JsonProperty("role")]
        public string Role { get; }

        [JsonProperty("content")]
        public string Content { get; }
    }
}
