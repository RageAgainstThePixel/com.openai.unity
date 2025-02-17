﻿// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ConversationItem
    {
        [Preserve]
        [JsonConstructor]
        internal ConversationItem(
            [JsonProperty("id")] string id,
            [JsonProperty("type")] ConversationItemType type,
            [JsonProperty("object")] string @object,
            [JsonProperty("status")] RealtimeResponseStatus status,
            [JsonProperty("role")] Role role,
            [JsonProperty("content")] IReadOnlyList<RealtimeContent> content,
            [JsonProperty("call_id")] string functionCallId,
            [JsonProperty("name")] string functionName,
            [JsonProperty("arguments")] string functionArguments,
            [JsonProperty("output")] string functionOutput)
        {
            Id = id;
            Object = @object;
            Type = type;
            Status = status;
            Role = role;
            Content = content;
            FunctionCallId = functionCallId;
            FunctionName = functionName;
            FunctionArguments = functionArguments;
            FunctionOutput = functionOutput;
        }

        [Preserve]
        public ConversationItem(Role role, IEnumerable<RealtimeContent> content)
        {
            Role = role;
            Type = ConversationItemType.Message;
            Content = content?.ToList() ?? new List<RealtimeContent>();

            if (role is not (Role.Assistant or Role.User))
            {
                throw new ArgumentException("Role must be either 'user' or 'assistant'.");
            }

            if (role == Role.User && !Content.All(c => c.Type is RealtimeContentType.InputAudio or RealtimeContentType.InputText))
            {
                throw new ArgumentException("User messages must contain only input text or input audio content.");
            }

            if (role == Role.Assistant && !Content.All(c => c.Type is RealtimeContentType.Text or RealtimeContentType.Audio))
            {
                throw new ArgumentException("Assistant messages must contain only text or audio content.");
            }
        }

        [Preserve]
        public ConversationItem(Role role, RealtimeContent content)
            : this(role, new[] { content })
        {
        }

        [Preserve]
        public ConversationItem(RealtimeContent content)
            : this(Role.User, new[] { content })
        {
        }

        [Preserve]
        public ConversationItem(ToolCall toolCall, string output)
        {
            Type = ConversationItemType.FunctionCallOutput;
            FunctionCallId = toolCall.Id;
            FunctionOutput = output;
        }

        [Preserve]
        public ConversationItem(Tool tool)
        {
            Type = ConversationItemType.FunctionCall;
            FunctionName = tool.Function.Name;
        }

        [Preserve]
        public static implicit operator ConversationItem(string text) => new(text);

        /// <summary>
        /// The unique ID of the item.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; private set; }

        /// <summary>
        /// The type of the item ("message", "function_call", "function_call_output").
        /// </summary>
        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ConversationItemType Type { get; internal set; }

        /// <summary>
        /// The object type, must be "realtime.item".
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; private set; }

        /// <summary>
        /// The status of the item ("completed", "in_progress", "incomplete").
        /// </summary>
        [Preserve]
        [JsonProperty("status")]
        public RealtimeResponseStatus Status { get; private set; }

        /// <summary>
        /// The role associated with the item ("user", "assistant", "system").
        /// </summary>
        [Preserve]
        [JsonProperty("role")]
        public Role Role { get; private set; }

        /// <summary>
        /// The content of the item.
        /// </summary>
        [Preserve]
        [JsonProperty("content")]
        public IReadOnlyList<RealtimeContent> Content { get; private set; }

        /// <summary>
        /// The ID of the function call (for "function_call" items).
        /// </summary>
        [Preserve]
        [JsonProperty("call_id")]
        public string FunctionCallId { get; private set; }

        /// <summary>
        /// The name of the function being called.
        /// </summary>
        [Preserve]
        [JsonProperty("name")]
        public string FunctionName { get; private set; }

        /// <summary>
        /// The arguments of the function call.
        /// </summary>
        [Preserve]
        [JsonProperty("arguments")]
        public JToken FunctionArguments { get; private set; }

        /// <summary>
        /// The output of the function call (for "function_call_output" items).
        /// </summary>
        [Preserve]
        [JsonProperty("output")]
        public string FunctionOutput { get; private set; }
    }
}
