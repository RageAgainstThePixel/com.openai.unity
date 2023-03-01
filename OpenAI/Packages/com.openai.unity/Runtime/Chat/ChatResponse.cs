using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenAI.Chat
{
    public sealed class ChatResponse
    {
        [JsonConstructor]
        public ChatResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("created")] int created,
            [JsonProperty("model")] string model,
            [JsonProperty("usage")] Usage usage,
            [JsonProperty("choices")] List<Choice> choices
        )
        {
            Id = id;
            Object = @object;
            Created = created;
            Model = model;
            Usage = usage;
            Choices = choices;
        }

        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("object")]
        public string Object { get; }

        [JsonProperty("created")]
        public int Created { get; }

        [JsonProperty("model")]
        public string Model { get; }

        [JsonProperty("usage")]
        public Usage Usage { get; }

        [JsonProperty("choices")]
        public IReadOnlyList<Choice> Choices { get; }
    }
}
