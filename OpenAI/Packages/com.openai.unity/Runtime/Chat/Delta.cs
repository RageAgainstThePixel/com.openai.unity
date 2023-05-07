// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Chat
{
    public sealed class Delta
    {
        [JsonConstructor]
        public Delta(
            [JsonProperty("role")] Role role,
            [JsonProperty("content")] string content,
            [JsonProperty("name")] string name)
        {
            Role = role;
            Content = content;
            Name = name;
        }

        [JsonProperty("role")]
        public Role Role { get; }

        [JsonProperty("content")]
        public string Content { get; }

        [JsonProperty("name")]
        public string Name { get; }

        public override string ToString() => Content ?? string.Empty;
    }
}
