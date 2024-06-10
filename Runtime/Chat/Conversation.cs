// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
            this.messages = new ConcurrentQueue<Message>();

            if (messages != null)
            {
                foreach (var message in messages)
                {
                    this.messages.Enqueue(message);
                }
            }
        }

        private readonly ConcurrentQueue<Message> messages;

        [Preserve]
        [JsonProperty("messages")]
        public IReadOnlyList<Message> Messages => messages.ToList();

        /// <summary>
        /// Appends <see cref="Message"/> to the end of <see cref="Messages"/>.
        /// </summary>
        /// <param name="message">The message to add to the <see cref="Conversation"/>.</param>
        [Preserve]
        public void AppendMessage(Message message) => messages.Enqueue(message);

        [Preserve]
        public override string ToString() => JsonConvert.SerializeObject(this, OpenAIClient.JsonSerializationOptions);

        [Preserve]
        public static implicit operator string(Conversation conversation) => conversation?.ToString();
    }
}
