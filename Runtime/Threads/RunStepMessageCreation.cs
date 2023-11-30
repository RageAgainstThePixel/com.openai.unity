// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class RunStepMessageCreation
    {
        [Preserve]
        [JsonConstructor]
        public RunStepMessageCreation([JsonProperty("message_id")] string messageId)
        {
            MessageId = messageId;
        }

        /// <summary>
        /// The ID of the message that was created by this run step.
        /// </summary>
        [Preserve]
        [JsonProperty("message_id")]
        public string MessageId { get; }
    }
}
