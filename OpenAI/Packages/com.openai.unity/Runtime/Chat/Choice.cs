// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Chat
{
    [Preserve]
    public sealed class Choice
    {
        [Preserve]
        public Choice() { }

        [Preserve]
        [JsonProperty("message")]
        public Message Message { get; private set; }

        [Preserve]
        [JsonProperty("delta")]
        public Delta Delta { get; private set; }

        [Preserve]
        [JsonProperty("finish_reason")]
        public string FinishReason { get; private set; }

        [Preserve]
        [JsonProperty("finish_details")]
        public FinishDetails FinishDetails { get; private set; }

        [Preserve]
        [JsonProperty("index")]
        public int Index { get; private set; }

        [Preserve]
        public override string ToString() => Message?.Content?.ToString() ?? Delta?.Content ?? string.Empty;

        [Preserve]
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

            if (other?.FinishDetails != null)
            {
                FinishDetails = other.FinishDetails;
            }

            Index = other?.Index ?? 0;
        }
    }
}
