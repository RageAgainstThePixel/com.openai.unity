// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class RealtimeConversation
    {
        /// <summary>
        /// The unique id of the conversation.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; private set; }

        /// <summary>
        /// The object type, must be "realtime.conversation".
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; private set; }

        [Preserve]
        public static implicit operator string(RealtimeConversation conversation) => conversation?.Id;
    }
}
