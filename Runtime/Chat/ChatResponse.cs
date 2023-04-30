// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace OpenAI.Chat
{
    public sealed class ChatResponse : BaseResponse
    {
        [JsonConstructor]
        public ChatResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("created")] int created,
            [JsonProperty("model")] string model,
            [JsonProperty("usage")] Usage usage,
            [JsonProperty("choices")] List<Choice> choices)
        {
            Id = id;
            Object = @object;
            Created = created;
            Model = model;
            Usage = usage;
            Choices = choices;
        }

        [JsonProperty("id")]
        public string Id { get; internal set; }

        [JsonProperty("object")]
        public string Object { get; internal set; }

        [JsonProperty("created")]
        public int Created { get; internal set; }

        [JsonProperty("model")]
        public string Model { get; internal set; }

        [JsonProperty("usage")]
        public Usage Usage { get; internal set; }

        [JsonProperty("choices")]
        public IReadOnlyList<Choice> Choices { get; }

        [JsonIgnore]
        public Choice FirstChoice => Choices?.FirstOrDefault(choice => choice.Index == 0);

        public override string ToString() => FirstChoice?.ToString() ?? string.Empty;

        public static implicit operator string(ChatResponse response) => response.ToString();
    }
}
