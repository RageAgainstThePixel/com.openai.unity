// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class TruncationStrategy
    {
        [Preserve]
        [JsonConstructor]
        public TruncationStrategy(
            [JsonProperty("type")] TruncationStrategies type,
            [JsonProperty("last_messages")] int? lastMessages = null)
        {
            Type = type;
            LastMessages = lastMessages;
        }

        /// <summary>
        /// The truncation strategy to use for the thread.
        /// The default is 'auto'. If set to 'last_messages',
        /// the thread will be truncated to the n most recent messages in the thread. When set to 'auto',
        /// messages in the middle of the thread will be dropped to fit the context length of the model, 'max_prompt_tokens'.
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public TruncationStrategies Type { get; }

        /// <summary>
        /// The number of most recent messages from the thread when constructing the context for the run.
        /// </summary>
        [Preserve]
        [JsonProperty("last_messages")]
        public int? LastMessages { get; }
    }
}
