// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine;

namespace OpenAI.Chat
{
    [Serializable]
    public sealed class Message
    {
        internal Message(Delta other) => CopyFrom(other);

        /// <summary>
        /// Creates a new message to insert into a chat conversation.
        /// </summary>
        /// <param name="role">
        /// The <see cref="Chat.Role"/> of the author of this message.
        /// </param>
        /// <param name="content">
        /// The contents of the message.
        /// </param>
        /// <param name="name">
        /// Optional, The name of the author of this message.<br/>
        /// May contain a-z, A-Z, 0-9, and underscores, with a maximum length of 64 characters.
        /// </param>
        /// <param name="function">
        /// The function that should be called, as generated by the model.
        /// </param>
        [JsonConstructor]
        public Message(
            [JsonProperty("role")] Role role,
            [JsonProperty("content")] string content,
            [JsonProperty("name")] string name = null,
            [JsonProperty("function_call")] Function function = null)
        {
            this.role = role;
            this.content = content;
            this.name = name;
            Function = function;
        }

        [SerializeField]
        private Role role;

        /// <summary>
        /// The <see cref="Chat.Role"/> of the author of this message.
        /// </summary>
        [JsonProperty("role")]
        public Role Role => role;

        [SerializeField]
        [TextArea(1, 30)]
        private string content;

        /// <summary>
        /// The contents of the message.
        /// </summary>
        [JsonProperty("content")]
        public string Content => content;

        [SerializeField]
        private string name;

        /// <summary>
        /// Optional, The name of the author of this message.<br/>
        /// May contain a-z, A-Z, 0-9, and underscores, with a maximum length of 64 characters.
        /// </summary>
        [JsonProperty("name")]
        public string Name => name;

        /// <summary>
        /// The function that should be called, as generated by the model.
        /// </summary>
        [JsonProperty("function_call")]
        public Function Function { get; private set; }

        public override string ToString() => Content ?? string.Empty;

        public static implicit operator string(Message message) => message.ToString();

        internal void CopyFrom(Delta other)
        {
            if (role == 0 &&
                other?.Role > 0)
            {
                role = other.Role;
            }

            if (!string.IsNullOrWhiteSpace(other?.Content))
            {
                content += other.Content;
            }

            if (!string.IsNullOrWhiteSpace(other?.Name))
            {
                name = other.Name;
            }

            if (other?.Function != null)
            {
                if (Function == null)
                {
                    Function = new Function(other);
                }
                else
                {
                    Function.CopyFrom(other);
                }
            }
        }
    }
}
