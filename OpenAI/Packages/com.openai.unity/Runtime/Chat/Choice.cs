// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using UnityEngine.Scripting;

namespace OpenAI.Chat
{
    [Preserve]
    public sealed class Choice : IAppendable<Choice>
    {
        [Preserve]
        public Choice() { }

        /// <summary>
        /// A chat completion message generated by the model.
        /// </summary>
        [Preserve]
        [JsonProperty("message", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Message Message { get; private set; }

        /// <summary>
        /// A chat completion delta generated by streamed model responses.
        /// </summary>
        [Preserve]
        [JsonProperty("delta", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Delta Delta { get; private set; }

        /// <summary>
        /// The reason the model stopped generating tokens.
        /// This will be stop if the model hit a natural stop point or a provided stop sequence,
        /// length if the maximum number of tokens specified in the request was reached,
        /// content_filter if content was omitted due to a flag from our content filters,
        /// tool_calls if the model called a tool, or function_call (deprecated) if the model called a function.
        /// </summary>
        [Preserve]
        [JsonProperty("finish_reason", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FinishReason { get; private set; }

        [Preserve]
        [JsonProperty("finish_details", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public FinishDetails FinishDetails { get; private set; }

        /// <summary>
        /// The index of the choice in the list of choices.
        /// </summary>
        [Preserve]
        [JsonProperty("index", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Index { get; private set; }

        /// <summary>
        /// Log probability information for the choice.
        /// </summary>
        [Preserve]
        [JsonProperty("logprobs", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public LogProbs LogProbs { get; private set; }

        [Preserve]
        public override string ToString() => Message?.ToString() ?? Delta?.Content ?? string.Empty;

        [Preserve]
        public static implicit operator string(Choice choice) => choice?.ToString();

        [Preserve]
        public void AppendFrom(Choice other)
        {
            if (other == null) { return; }

            if (other.Index.HasValue)
            {
                Index = other.Index.Value;
            }

            if (other.Message != null)
            {
                Message = other.Message;
            }

            if (other.Delta != null)
            {
                if (Message == null)
                {
                    Message = new Message(other.Delta);
                }
                else
                {
                    Message.AppendFrom(other.Delta);
                }
            }

            if (other.LogProbs != null)
            {
                LogProbs = other.LogProbs;
            }

            if (!string.IsNullOrWhiteSpace(other.FinishReason))
            {
                FinishReason = other.FinishReason;
            }

            if (other.FinishDetails != null)
            {
                FinishDetails = other.FinishDetails;
            }
        }
    }
}
