// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class IncompleteDetails
    {
        [Preserve]
        [JsonConstructor]
        internal IncompleteDetails([JsonProperty("reason")] IncompleteMessageReason reason)
        {
            Reason = reason;
        }

        [Preserve]
        [JsonProperty("reason")]
        public IncompleteMessageReason Reason { get; }
    }
}
