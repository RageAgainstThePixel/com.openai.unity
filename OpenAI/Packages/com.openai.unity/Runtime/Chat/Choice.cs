using Newtonsoft.Json;

namespace OpenAI.Chat
{
    public sealed class Choice
    {
        [JsonConstructor]
        public Choice(
            [JsonProperty("message")] Message message,
            [JsonProperty("finish_reason")] string finishReason,
            [JsonProperty("index")] int index
        )
        {
            Message = message;
            FinishReason = finishReason;
            Index = index;
        }

        [JsonProperty("message")]
        public Message Message { get; }

        [JsonProperty("finish_reason")]
        public string FinishReason { get; }

        [JsonProperty("index")]
        public int Index { get; }
    }
}
