// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Chat
{
    [Preserve]
    [Serializable]
    public sealed class Conversation
    {
        [Preserve]
        [JsonConstructor]
        public Conversation([JsonProperty("messages")] List<Message> messages = null)
        {
            this.messages = messages ?? new List<Message>();
        }

        private readonly List<Message> messages;

        [Preserve]
        [JsonProperty("messages")]
        public IReadOnlyList<Message> Messages => messages;

        /// <summary>
        /// Appends <see cref="Message"/> to the end of <see cref="Messages"/>.
        /// </summary>
        /// <param name="message">The message to add to the <see cref="Conversation"/>.</param>
        [Preserve]
        public void AppendMessage(Message message) => messages.Add(message);

        [Preserve]
        public override string ToString() => JsonConvert.SerializeObject(this, OpenAIClient.JsonSerializationOptions);

        [Preserve]
        public static implicit operator string(Conversation conversation) => conversation?.ToString();
    }
}
