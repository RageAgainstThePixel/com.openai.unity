// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Chat
{
    /// <summary>
    /// Log probability information for the choice.
    /// </summary>
    public sealed class LogProbs
    {
        [JsonConstructor]
        public LogProbs([JsonProperty("content")] List<LogProbInfo> content)
        {
            Content = content;
        }

        /// <summary>
        /// A list of message content tokens with log probability information.
        /// </summary>
        [Preserve]
        [JsonProperty("content")]
        public IReadOnlyList<LogProbInfo> Content { get; }
    }
}
