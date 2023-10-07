// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Chat
{
    [Preserve]
    public sealed class Choice
    {
        [Preserve]
        [JsonConstructor]
        public Choice(
            [JsonProperty("message")] Message message,
            [JsonProperty("delta")] Delta delta,
            [JsonProperty("finish_reason")] string finishReason,
            [JsonProperty("index")] int index)
        {
            Message = message;
            Delta = delta;
            FinishReason = finishReason;
            Index = index;
        }

        [Preserve]
        [JsonProperty("message")]
        public Message Message { get; internal set; }

        [Preserve]
        [JsonProperty("delta")]
        public Delta Delta { get; internal set; }

        [Preserve]
        [JsonProperty("finish_reason")]
        public string FinishReason { get; internal set; }

        [Preserve]
        [JsonProperty("index")]
        public int Index { get; internal set; }

        public override string ToString() => Message?.Content ?? Delta?.Content ?? string.Empty;

        public static implicit operator string(Choice choice) => choice.ToString();

        [Preserve]
        internal void CopyFrom(Choice other)
        {
            if (other?.Message != null)
            {
                Message = other.Message;
            }

            if (other?.Delta != null)
            {
                if (Message == null)
                {
                    Message = new Message(other.Delta);
                }
                else
                {
                    Message.CopyFrom(other.Delta);
                }
            }

            if (!string.IsNullOrWhiteSpace(other?.FinishReason))
            {
                FinishReason = other.FinishReason;
            }

            Index = other?.Index ?? 0;
        }
    }
}
