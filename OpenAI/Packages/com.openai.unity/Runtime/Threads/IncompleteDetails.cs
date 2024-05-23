using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class IncompleteDetails
    {
        [Preserve]
        [JsonProperty("reason")]
        public IncompleteMessageReason Reason { get; private set; }
    }
}