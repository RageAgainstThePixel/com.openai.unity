// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

namespace OpenAI.Chat
{
    [Preserve]
    [Serializable]
    public sealed class Message
    {
        [Preserve]
        public Message() { }

        [Preserve]
        internal Message(Delta other) => AppendFrom(other);

        /// <summary>
        /// Creates a new message to insert into a chat conversation.
        /// </summary>
        /// <param name="role">
        /// The <see cref="OpenAI.Role"/> of the author of this message.
        /// </param>
        /// <param name="content">
        /// The contents of the message.
        /// </param>
        /// <param name="name"></param>
        [Preserve]
        public Message(Role role, IEnumerable<Content> content, string name = null)
        {
            Role = role;
            Content = content?.ToList();
            Name = name;
        }

        /// <summary>
        /// Creates a new message to insert into a chat conversation.
        /// </summary>
        /// <param name="role">
        /// The <see cref="OpenAI.Role"/> of the author of this message.
        /// </param>
        /// <param name="content">
        /// The contents of the message.
        /// </param>
        /// <param name="name"></param>
        [Preserve]
        public Message(Role role, string content, string name = null)
        {
            Role = role;
            Content = content;
            Name = name;
        }

        /// <inheritdoc />
        [Preserve]
        [Obsolete("use overload with ToolCall")]
        public Message(Tool tool, string content)
            : this(Role.Tool, content, tool.Function.Name)
        {
            ToolCallId = tool.Id;
        }

        /// <inheritdoc />
        [Preserve]
        public Message(ToolCall toolCall, string content)
            : this(Role.Tool, content, toolCall.Function.Name)
        {
            ToolCallId = toolCall.Id;
        }

        [Preserve]
        [Obsolete("use overload with ToolCall")]
        public Message(Tool tool, IEnumerable<Content> content)
            : this(Role.Tool, content, tool.Function.Name)
        {
            ToolCallId = tool.Id;
        }

        /// <summary>
        /// Creates a new message to insert into a chat conversation.
        /// </summary>
        /// <param name="toolCall">ToolCall used for message.</param>
        /// <param name="content">Tool function response.</param>
        [Preserve]
        public Message(ToolCall toolCall, IEnumerable<Content> content)
            : this(Role.Tool, content, toolCall.Function.Name)
        {
            ToolCallId = toolCall.Id;
        }

        /// <summary>
        /// Creates a new message to insert into a chat conversation.
        /// </summary>
        /// <param name="toolCallId">The tool_call_id to use for the message.</param>
        /// <param name="toolFunctionName">Name of the function call.</param>
        /// <param name="content">Tool function response.</param>
        [Preserve]
        public Message(string toolCallId, string toolFunctionName, IEnumerable<Content> content)
            : this(Role.Tool, content, toolFunctionName)
        {
            ToolCallId = toolCallId;
        }

        [SerializeField]
        private string name;

        /// <summary>
        /// Optional, The name of the author of this message.<br/>
        /// May contain a-z, A-Z, 0-9, and underscores, with a maximum length of 64 characters.
        /// </summary>
        [Preserve]
        [JsonProperty("name")]
        public string Name
        {
            get => name;
            private set => name = value;
        }

        [Preserve]
        [SerializeField]
        private Role role;

        /// <summary>
        /// The <see cref="OpenAI.Role"/> of the author of this message.
        /// </summary>
        [Preserve]
        [JsonProperty("role")]
        public Role Role
        {
            get => role;
            private set => role = value;
        }

        [SerializeField]
        [TextArea(1, 30)]
        private string content;

        private object contentList;

        /// <summary>
        /// The contents of the message.
        /// </summary>
        [Preserve]
        [JsonProperty("content", DefaultValueHandling = DefaultValueHandling.Populate, NullValueHandling = NullValueHandling.Include, Required = Required.AllowNull)]
        public object Content
        {
            get => contentList ?? content;
            private set
            {
                if (value is string s)
                {
                    content = s;
                }
                else
                {
                    contentList = value;
                }
            }
        }

        /// <summary>
        /// The refusal message generated by the model.
        /// </summary>
        [Preserve]
        [JsonProperty("refusal")]
        public string Refusal { get; private set; }

        private List<ToolCall> toolCalls;

        /// <summary>
        /// The tool calls generated by the model, such as function calls.
        /// </summary>
        [Preserve]
        [JsonProperty("tool_calls")]
        public IReadOnlyList<ToolCall> ToolCalls
        {
            get => toolCalls;
            private set => toolCalls = value?.ToList();
        }

        [Preserve]
        [JsonProperty("tool_call_id")]
        public string ToolCallId { get; private set; }

        /// <summary>
        /// If the audio output modality is requested, this object contains data about the audio response from the model. 
        /// </summary>
        [Preserve]
        [JsonProperty("audio")]
        public AudioOutput AudioOutput { get; private set; }

        [Preserve]
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Content?.ToString()))
            {
                return AudioOutput?.ToString() ?? string.Empty;
            }

            return Content?.ToString() ?? string.Empty;
        }

        [Preserve]
        public static implicit operator string(Message message) => message?.ToString();

        [Preserve]
        internal void AppendFrom(Delta other)
        {
            if (Role == 0 &&
                other?.Role > 0)
            {
                Role = other.Role;
            }

            if (other?.Content != null)
            {
                content += other.Content;
            }

            if (!string.IsNullOrWhiteSpace(other?.Refusal))
            {
                Refusal += other?.Refusal;
            }

            if (!string.IsNullOrWhiteSpace(other?.Name))
            {
                Name = other?.Name;
            }

            if (other is { ToolCalls: not null })
            {
                toolCalls ??= new List<ToolCall>();
                toolCalls.AppendFrom(other.ToolCalls);
            }

            if (other is { AudioOutput: not null })
            {
                if (AudioOutput == null)
                {
                    AudioOutput = other.AudioOutput;
                }
                else
                {
                    AudioOutput.AppendFrom(other.AudioOutput);
                }
            }
        }
    }
}
